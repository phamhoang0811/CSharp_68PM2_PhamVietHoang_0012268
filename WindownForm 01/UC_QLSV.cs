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
            using (SqlConnection conn = DBconnect.GetConnection())
            {
                conn.Open();

                string query = @"INSERT INTO Students
                        (MSSV, FullName, DateOfBirth, Gender, ClassId)
                        VALUES
                        (@MSSV, @FullName, @DateOfBirth, @Gender, @ClassId)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@MSSV", txt_mssv.Text.Trim());
                cmd.Parameters.AddWithValue("@FullName", txt_name.Text.Trim());
                cmd.Parameters.AddWithValue("@DateOfBirth", dateTimePicker2.Value);
                cmd.Parameters.AddWithValue("@Gender", comboBox2.Text);
                cmd.Parameters.AddWithValue("@ClassId", comboBox3.Text);

                cmd.ExecuteNonQuery();
            }

            LoadData();
            ClearData();

            MessageBox.Show("Thêm sinh viên thành công!");
        }

        // =========================================================================
        // CHỨC NĂNG 2: UPDATE (CẬP NHẬT/SỬA)
        // =========================================================================
        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Ràng buộc: Bắt buộc chọn sinh viên (đã click lên grid và lưu mssvCu) trước khi bấm Sửa
                if (string.IsNullOrEmpty(mssvCu))
                {
                    MessageBox.Show("Vui lòng chọn sinh viên cần sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = DBconnect.GetConnection())
                {
                    conn.Open();

                    // Cập nhật dữ liệu dựa trên WHERE MSSV cũ (vì không được phép thay đổi Khóa chính)
                    string query = @"
                UPDATE Students
                SET FullName = @FullName,
                    DateOfBirth = @DateOfBirth,
                    Gender = @Gender,
                    ClassId = @ClassId
                WHERE MSSV = @MSSV";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Truyền Parameter để tránh lỗi ký tự đặc biệt và SQL Injection
                    cmd.Parameters.AddWithValue("@MSSV", mssvCu); 
                    cmd.Parameters.AddWithValue("@FullName", txt_name.Text.Trim());
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateTimePicker2.Value);
                    cmd.Parameters.AddWithValue("@Gender", comboBox2.Text);
                    cmd.Parameters.AddWithValue("@ClassId", comboBox3.Text);

                    // Lấy số lượng dòng bị ảnh hưởng bởi câu lệnh UPDATE
                    int rows = cmd.ExecuteNonQuery(); 

                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Cập nhật lại Grid và xóa form nhập liệu
                LoadData();
                ClearData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================================
        // CHỨC NĂNG 3: DELETE (XÓA)
        // =========================================================================
        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem người dùng đã chọn sinh viên trên lưới chưa
                if (string.IsNullOrEmpty(mssvCu))
                {
                    MessageBox.Show("Vui lòng chọn sinh viên cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xác nhận với người dùng để tránh bấm nhầm nút Xóa
                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc muốn xóa sinh viên có MSSV " + mssvCu + "?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                {
                    return; // Hủy thao tác nếu người dùng bấm 'No'
                }

                using (SqlConnection conn = DBconnect.GetConnection())
                {
                    conn.Open();

                    // Thực thi lệnh xóa theo Khóa chính (MSSV)
                    string query = "DELETE FROM Students WHERE MSSV = @MSSV";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MSSV", mssvCu);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                ClearData();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        // =========================================================================
        // CHỨC NĂNG 1.2: CÁC NÚT ĐIỀU HƯỚNG TÌM KIẾM & PHÂN TRANG
        // =========================================================================
        private void Button5_Click(object sender, EventArgs e)
        {
            currentPage = 1; // Khi tìm kiếm từ khóa mới, bắt buộc phải nhảy về trang 1
            LoadData();      
        }

        // Nút Trang đầu
        private void Button6_Click(object sender, EventArgs e) { currentPage = 1; LoadData(); }
        // Nút Trang trước (giảm trang hiện tại đi 1, với điều kiện trang hiện tại > 1)
        private void Button7_Click(object sender, EventArgs e) { if (currentPage > 1) { currentPage--; LoadData(); } }
        // Nút Trang sau (tăng trang hiện tại lên 1, với điều kiện chưa vượt qua tổng số trang)
        private void Button8_Click(object sender, EventArgs e) { if (currentPage < totalPages) { currentPage++; LoadData(); } }
        // Nút Trang cuối
        private void Button9_Click(object sender, EventArgs e) { currentPage = totalPages; LoadData(); }


        // =========================================================================
        // CHỨC NĂNG 4: DATAGRIDVIEW CELLCLICKEVENT (CLICK CHỌN DÒNG)
        // =========================================================================
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ràng buộc: Chỉ thao tác khi người dùng click vào dòng dữ liệu thật, bỏ qua dòng tiêu đề (có RowIndex = -1)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Đẩy ngược dữ liệu từ lưới lên các ô TextBox, ComboBox tương ứng
                txt_mssv.Text = row.Cells[0].Value?.ToString() ?? "";
                txt_name.Text = row.Cells[1].Value?.ToString() ?? "";
                comboBox2.Text = row.Cells[2].Value?.ToString() ?? "";

                // Dữ liệu ngày tháng lấy từ grid là dạng chuỗi, cần Parse về dạng DateTime chuẩn để gán vào DateTimePicker
                if (row.Cells[3].Value != null && DateTime.TryParseExact(row.Cells[3].Value.ToString(), "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime dob))
                    dateTimePicker2.Value = dob;
                else if (row.Cells[3].Value != null && DateTime.TryParse(row.Cells[3].Value.ToString(), out DateTime dobAlt))
                    dateTimePicker2.Value = dobAlt;

                comboBox3.Text = row.Cells[4].Value?.ToString() ?? "";

                // QUAN TRỌNG: Lưu MSSV vừa chọn vào biến toàn cục. Biến này chính là chìa khóa để 2 hàm Update và Delete biết phải sửa/xóa ai.
                mssvCu = txt_mssv.Text.Trim();

                // Chặn không cho người dùng sửa ô nhập MSSV trên giao diện (vì đây là khóa chính database)
                txt_mssv.ReadOnly = true;
            }
        }


        // Các hàm rỗng mặc định sinh ra bởi Visual Studio Designer
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}