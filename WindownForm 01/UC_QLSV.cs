using System;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace WindownForm_01
{
    public partial class UC_QLSV : UserControl
    {
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalPages = 1;

        private string mssvCu = "";

        public UC_QLSV()
        {
            InitializeComponent();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            LoadComboBox();
            LoadData();
        }

        private void LoadComboBox()
        {
            try
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("Nam");
                comboBox2.Items.Add("Nữ");

                comboBox3.Items.Clear();

                using (SqlConnection conn = DBconnect.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT ClassId FROM Classes", conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox3.Items.Add(reader["ClassId"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách lớp: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetSearchKeyword()
        {
            return textBox1.Text.Trim();
        }

        private string GetSearchCondition()
        {
            if (string.IsNullOrEmpty(GetSearchKeyword()))
            {
                return "";
            }

            return @"WHERE MSSV LIKE @KeyMSSV
             OR FullName LIKE @KeyName
             OR Gender = @KeyExact
             OR ClassId = @KeyExact";
        }

        private void AddSearchParameter(SqlCommand cmd)
        {
            string key = GetSearchKeyword().Trim();

            cmd.Parameters.AddWithValue("@KeyMSSV", "%" + key + "%");
            cmd.Parameters.AddWithValue("@KeyName", "% " + key + "%");
            cmd.Parameters.AddWithValue("@KeyExact", key);
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = DBconnect.GetConnection())
                {
                    conn.Open();

                    string whereClause = GetSearchCondition();
                    string query = $@"
                        SELECT * FROM
                        (
                            SELECT ROW_NUMBER() OVER (ORDER BY MSSV) AS RowNum, *
                            FROM Students
                            {whereClause}
                        ) AS T
                        WHERE RowNum BETWEEN @StartRow AND @EndRow";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StartRow", (currentPage - 1) * pageSize + 1);
                    cmd.Parameters.AddWithValue("@EndRow", currentPage * pageSize);
                    if (!string.IsNullOrEmpty(whereClause))
                    {
                        AddSearchParameter(cmd);
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.Rows.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        dataGridView1.Rows.Add(
                            row["MSSV"],
                            row["FullName"],
                            row["Gender"],
                            Convert.ToDateTime(row["DateOfBirth"]).ToString("dd/MM/yyyy"),
                            row["ClassId"]
                        );
                    }

                    SqlCommand countCmd = new SqlCommand($"SELECT COUNT(*) FROM Students {whereClause}", conn);
                    if (!string.IsNullOrEmpty(whereClause))
                    {
                        AddSearchParameter(countCmd);
                    }

                    int totalRecords = (int)countCmd.ExecuteScalar();

                    totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                    if (totalPages < 1) totalPages = 1;
                    if (currentPage > totalPages)
                    {
                        currentPage = totalPages;
                        LoadData();
                        return;
                    }

                    label4.Text = $"Trang {currentPage}/{totalPages} | {totalRecords} bản ghi";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearData()
        {
            txt_mssv.Clear();
            txt_name.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            dateTimePicker2.Value = DateTime.Now;
            txt_mssv.ReadOnly = false;
            mssvCu = "";
        }

        // Sự kiện nút Thêm
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txt_mssv.Text.Trim()) || string.IsNullOrEmpty(txt_name.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin MSSV và Họ tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = DBconnect.GetConnection())
                {
                    conn.Open();

                    string query = @"INSERT INTO Students (MSSV, FullName, DateOfBirth, Gender, ClassId)
                                    VALUES (@MSSV, @FullName, @DateOfBirth, @Gender, @ClassId)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MSSV", txt_mssv.Text.Trim());
                    cmd.Parameters.AddWithValue("@FullName", txt_name.Text.Trim());
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateTimePicker2.Value);
                    cmd.Parameters.AddWithValue("@Gender", comboBox2.Text);
                    cmd.Parameters.AddWithValue("@ClassId", comboBox3.Text);

                    cmd.ExecuteNonQuery();
                }

                currentPage = 1;
                LoadData();
                ClearData();
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm dữ liệu (Có thể trùng MSSV): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện nút Sửa
        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(mssvCu))
                {
                    MessageBox.Show("Vui lòng chọn sinh viên cần sửa!",
                        "Cảnh báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = DBconnect.GetConnection())
                {
                    conn.Open();

                    string query = @"
                UPDATE Students
                SET FullName = @FullName,
                    DateOfBirth = @DateOfBirth,
                    Gender = @Gender,
                    ClassId = @ClassId
                WHERE MSSV = @MSSV";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@MSSV", mssvCu);
                    cmd.Parameters.AddWithValue("@FullName", txt_name.Text.Trim());
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateTimePicker2.Value);
                    cmd.Parameters.AddWithValue("@Gender", comboBox2.Text);
                    cmd.Parameters.AddWithValue("@ClassId", comboBox3.Text);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật sinh viên thành công!",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên cần cập nhật!",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }

                LoadData();
                ClearData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Sự kiện nút Xóa
        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(mssvCu))
                {
                    MessageBox.Show("Vui lòng chọn sinh viên cần xóa!",
                        "Cảnh báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc muốn xóa sinh viên có MSSV " + mssvCu + "?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                {
                    return;
                }

                using (SqlConnection conn = DBconnect.GetConnection())
                {
                    conn.Open();

                    string query = "DELETE FROM Students WHERE MSSV = @MSSV";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MSSV", mssvCu);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Xóa sinh viên thành công!",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên cần xóa!",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }

                ClearData();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa sinh viên: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Sự kiện nút Làm mới
        private void Button4_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        // Sự kiện nút Tìm kiếm
        private void Button5_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadData();
        }

        // Click vào bảng để lấy dữ liệu ngược lên Form
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txt_mssv.Text = row.Cells[0].Value?.ToString() ?? "";
                txt_name.Text = row.Cells[1].Value?.ToString() ?? "";
                comboBox2.Text = row.Cells[2].Value?.ToString() ?? "";

                if (row.Cells[3].Value != null && DateTime.TryParseExact(row.Cells[3].Value.ToString(), "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime dob))
                    dateTimePicker2.Value = dob;
                else if (row.Cells[3].Value != null && DateTime.TryParse(row.Cells[3].Value.ToString(), out DateTime dobAlt))
                    dateTimePicker2.Value = dobAlt;

                comboBox3.Text = row.Cells[4].Value?.ToString() ?? "";

                mssvCu = txt_mssv.Text.Trim();

                txt_mssv.ReadOnly = true;
            }
        }

        // Phân trang
        private void Button6_Click(object sender, EventArgs e) { currentPage = 1; LoadData(); }
        private void Button7_Click(object sender, EventArgs e) { if (currentPage > 1) { currentPage--; LoadData(); } }
        private void Button8_Click(object sender, EventArgs e) { if (currentPage < totalPages) { currentPage++; LoadData(); } }
        private void Button9_Click(object sender, EventArgs e) { currentPage = totalPages; LoadData(); }

        // Sự kiện Xem danh sách sinh viên
    

        // Chống lỗi Designer
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
