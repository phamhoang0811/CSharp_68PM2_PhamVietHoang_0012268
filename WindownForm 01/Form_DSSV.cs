using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace WindownForm_01
{
    public partial class Form_DSSV : Form
    {
        private readonly string classId;
        private int currentPage = 1;
        private readonly int pageSize = 10;
        private int totalPages = 1;

        public Form_DSSV(string _classId)
        {
            InitializeComponent();
            this.classId = _classId;
            this.Text = $"Danh sách sinh viên lớp: {classId}";

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            LoadData();
        }

        // Hàm này sinh ra do lỡ click đúp vào form, cứ để trống là hết lỗi
        private void Form_DSSV_Load(object sender, EventArgs e)
        {
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = DBconnect.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT * FROM
                        (
                            SELECT ROW_NUMBER() OVER (ORDER BY MSSV) AS RowNum, *
                            FROM Students
                            WHERE ClassId = @ClassId
                        ) AS T
                        WHERE RowNum BETWEEN @StartRow AND @EndRow";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ClassId", classId);
                    cmd.Parameters.AddWithValue("@StartRow", (currentPage - 1) * pageSize + 1);
                    cmd.Parameters.AddWithValue("@EndRow", currentPage * pageSize);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    SqlCommand countCmd = new SqlCommand("SELECT COUNT(*) FROM Students WHERE ClassId = @ClassId", conn);
                    countCmd.Parameters.AddWithValue("@ClassId", classId);

                    int totalRecords = (int)countCmd.ExecuteScalar();
                    totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                    if (totalPages < 1) totalPages = 1;

                    lbl_PageInfo.Text = $"Trang {currentPage}/{totalPages} | Tổng: {totalRecords} SV";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BtnFirst_Click(object sender, EventArgs e) { currentPage = 1; LoadData(); }
        public void BtnPrev_Click(object sender, EventArgs e) { if (currentPage > 1) { currentPage--; LoadData(); } }
        public void BtnNext_Click(object sender, EventArgs e) { if (currentPage < totalPages) { currentPage++; LoadData(); } }
        public void BtnLast_Click(object sender, EventArgs e) { currentPage = totalPages; LoadData(); }

        private void lbl_PageInfo_Click(object sender, EventArgs e)
        {

        }
    }
}