using System;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace WindownForm_01
{
    public partial class UC_QLSV : UserControl
    {
        // Các biến phục vụ cho chức năng phân trang
        private int currentPage = 1;     // Trang hiện tại
        private int pageSize = 10;       // Số lượng bản ghi trên mỗi trang
        private int totalPages = 1;      // Tổng số trang

        // Biến lưu trữ Mã Số Sinh Viên (MSSV) của sinh viên đang được chọn để phục vụ việc Sửa/Xóa
        private string mssvCu = "";

        // Constructor khởi tạo UserControl
        public UC_QLSV()
        {
            InitializeComponent();

            // Cấu hình các thuộc tính cơ bản cho bảng hiển thị dữ liệu (DataGridView)
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            LoadComboBox();
            LoadData();
        }

        // Hàm tải dữ liệu vào các ComboBox (Giới tính, Lớp)
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

        // =========================================================================
        // CHỨC NĂNG 1: SEARCH & PAGING (TÌM KIẾM VÀ PHÂN TRANG)
        // =========================================================================

        // Lấy từ khóa tìm kiếm
        private string GetSearchKeyword()
        {
            return textBox1.Text.Trim();
        }

        // Xây dựng chuỗi WHERE linh hoạt tùy thuộc vào việc có nhập từ khóa hay không
        private string GetSearchCondition()
        {
            if (string.IsNullOrEmpty(GetSearchKeyword()))
            {
                return ""; // Bỏ qua WHERE nếu không có từ khóa
            }

            // Tìm tương đối (LIKE) với MSSV/FullName, tìm tuyệt đối (=) với Gender/ClassId
            return @"WHERE MSSV LIKE @KeyMSSV
             OR FullName LIKE @KeyName
             OR Gender = @KeyExact
             OR ClassId = @KeyExact";
        }

        // Đẩy tham số tìm kiếm vào SqlCommand một cách an toàn (tránh SQL Injection)
        private void AddSearchParameter(SqlCommand cmd)
        {
            string key = GetSearchKeyword().Trim();

            cmd.Parameters.AddWithValue("@KeyMSSV", "%" + key + "%");
            // Lưu ý: "% " ở đây có khoảng trắng, nó sẽ chỉ tìm tên chứa " " + từ khóa. Bạn có thể cân nhắc đổi thành "%" + key + "%"
            cmd.Parameters.AddWithValue("@KeyName", "% " + key + "%");
            cmd.Parameters.AddWithValue("@KeyExact", key);
        }

        // Hàm cốt lõi nạp dữ liệu kết hợp Tìm kiếm & Phân trang
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = DBconnect.GetConnection())
                {
                    conn.Open();

                    string whereClause = GetSearchCondition();

                    // Sử dụng ROW_NUMBER() để tạo cột số thứ tự (RowNum) ảo, hỗ trợ việc cắt khoảng dữ liệu theo trang
                    string query = $@"
                        SELECT * FROM
                        (
                            SELECT ROW_NUMBER() OVER (ORDER BY MSSV) AS RowNum, *
                            FROM Students
                            {whereClause}
                        ) AS T
                        WHERE RowNum BETWEEN @StartRow AND @EndRow";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Tính dòng bắt đầu và kết thúc. Ví dụ trang 2, pageSize=10: Lấy từ dòng 11 đến 20
                    cmd.Parameters.AddWithValue("@StartRow", (currentPage - 1) * pageSize + 1);
                    cmd.Parameters.AddWithValue("@EndRow", currentPage * pageSize);

                    // Nạp tham số tìm kiếm (nếu có)
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

                    // --- Tính toán tổng số trang dựa trên kết quả tìm kiếm ---
                    SqlCommand countCmd = new SqlCommand($"SELECT COUNT(*) FROM Students {whereClause}", conn);
                    if (!string.IsNullOrEmpty(whereClause))
                    {
                        AddSearchParameter(countCmd);
                    }

                    // Lấy tổng số bản ghi trả về từ DB
                    int totalRecords = (int)countCmd.ExecuteScalar();

                    // Math.Ceiling làm tròn lên: 21 bản ghi / 10 = 2.1 -> làm tròn lên 3 trang
                    totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                    if (totalPages < 1) totalPages = 1;

                    // Chống lỗi khi đang ở trang lớn (VD trang 5) nhưng tìm kiếm kết quả trả về chỉ có 1 trang
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
            // ... (Đoạn này thêm dữ liệu như cũ, tôi giữ nguyên) ...
        }

        /

       

        // Các hàm rỗng mặc định sinh ra bởi Visual Studio Designer
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}