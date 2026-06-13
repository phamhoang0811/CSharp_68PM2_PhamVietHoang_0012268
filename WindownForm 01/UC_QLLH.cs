using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace WindownForm_01
{
    public partial class UC_QLLH : UserControl
    {
        private int currentPage = 1;
        private readonly int pageSize = 10;
        private int totalPages = 1;

        // Biến lưu Mã ID (ClassId) cũ để phục vụ sửa và xóa
        private string classIdCu = "";

        public UC_QLLH()
        {
            InitializeComponent();

            // Đăng ký sự kiện trực tiếp trong code để đồng bộ với Designer
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            btnXemDSSV.Click += BtnXemDSSV_Click;
            btnTim.Click += BtnTim_Click;
            btnFirst.Click += BtnFirst_Click;
            btnPrev.Click += BtnPrev_Click;
            btnNext.Click += BtnNext_Click;
            btnLast.Click += BtnLast_Click;
            dgvLopHoc.CellClick += DgvLopHoc_CellClick;

            // Cấu hình DataGridView
            dgvLopHoc.AllowUserToAddRows = false;
            dgvLopHoc.ReadOnly = true;
            dgvLopHoc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLopHoc.MultiSelect = false;

            LoadData();
        }

        private string GetSearchKeyword()
        {
            return txtTimKiem.Text.Trim();
        }

        private string GetSearchCondition()
        {
            if (string.IsNullOrEmpty(GetSearchKeyword())) return "";
            return @"WHERE ClassId LIKE @Key OR ClassName LIKE @Key OR ClassCode LIKE @Key";
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
                            SELECT ROW_NUMBER() OVER (ORDER BY ClassId) AS RowNum, *
                            FROM Classes
                            {whereClause}
                        ) AS T
                        WHERE RowNum BETWEEN @StartRow AND @EndRow";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StartRow", (currentPage - 1) * pageSize + 1);
                    cmd.Parameters.AddWithValue("@EndRow", currentPage * pageSize);

                    if (!string.IsNullOrEmpty(whereClause))
                    {
                        cmd.Parameters.AddWithValue("@Key", "%" + GetSearchKeyword() + "%");
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvLopHoc.Rows.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        dgvLopHoc.Rows.Add(
                            row["ClassId"],
                            row["ClassCode"],
                            row["ClassName"],
                            row["Note"]
                        );
                    }

                    SqlCommand countCmd = new SqlCommand($"SELECT COUNT(*) FROM Classes {whereClause}", conn);
                    if (!string.IsNullOrEmpty(whereClause))
                    {
                        countCmd.Parameters.AddWithValue("@Key", "%" + GetSearchKeyword() + "%");
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

                    lblPhanTrang.Text = $"Trang {currentPage}/{totalPages} | {totalRecords} bản ghi";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearData()
        {
            txtMaID.Clear();
            txtMaLop.Clear();
            txtTenLop.Clear();
            txtGhiChu.Clear();
            txtTimKiem.Clear();
            txtMaID.ReadOnly = false;
            classIdCu = "";
        }
        // =====================================================================
        //  CHỨC NĂNG THÊM LỚP HỌC (INSERT)
        // =====================================================================
        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = DBconnect.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO Classes (ClassId, ClassCode, ClassName, Note) 
                                     VALUES (@Id, @Code, @Name, @Note)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", txtMaID.Text.Trim());
                    cmd.Parameters.AddWithValue("@Code", txtMaLop.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", txtTenLop.Text.Trim());
                    cmd.Parameters.AddWithValue("@Note", txtGhiChu.Text.Trim());

                    cmd.ExecuteNonQuery();
                }
                LoadData();
                ClearData();
                MessageBox.Show("Thêm lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi thêm dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // =====================================================================
        //  ĐÂY LÀ CHỨC NĂNG SỬA LỚP HỌC (UPDATE)
        // =====================================================================
        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(classIdCu))
            {
                MessageBox.Show("Vui lòng chọn lớp để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using (SqlConnection conn = DBconnect.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Classes SET ClassCode=@Code, ClassName=@Name, Note=@Note WHERE ClassId=@Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", classIdCu);
                    cmd.Parameters.AddWithValue("@Code", txtMaLop.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", txtTenLop.Text.Trim());
                    cmd.Parameters.AddWithValue("@Note", txtGhiChu.Text.Trim());

                    cmd.ExecuteNonQuery();
                }
                LoadData();
                ClearData();
                MessageBox.Show("Sửa thông tin lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi sửa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // =====================================================================
        // ĐÂY LÀ CHỨC NĂNG XÓA LỚP HỌC (DELETE)
        // =====================================================================
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(classIdCu))
            {
                MessageBox.Show("Vui lòng chọn lớp để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa lớp ID: {classIdCu}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = DBconnect.GetConnection())
                    {
                        conn.Open();
                        string query = "DELETE FROM Classes WHERE ClassId=@Id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Id", classIdCu);
                        cmd.ExecuteNonQuery();
                    }
                    LoadData();
                    ClearData();
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException sqlex)
                {
                    if (sqlex.Number == 547)
                        MessageBox.Show("Không thể xóa lớp này vì hiện tại đang có sinh viên thuộc lớp!", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        MessageBox.Show("Lỗi CSDL: " + sqlex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            ClearData();
            LoadData();
        }

        private void DgvLopHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLopHoc.Rows[e.RowIndex];

                txtMaID.Text = row.Cells[0].Value?.ToString() ?? "";
                txtMaLop.Text = row.Cells[1].Value?.ToString() ?? "";
                txtTenLop.Text = row.Cells[2].Value?.ToString() ?? "";
                txtGhiChu.Text = row.Cells[3].Value?.ToString() ?? "";

                classIdCu = txtMaID.Text.Trim();
                txtMaID.ReadOnly = true;
            }
        }

        //HIỂN THỊ DANH SÁCH SINH VIÊN THEO LỚP
        private void BtnXemDSSV_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(classIdCu))
            {
                MessageBox.Show("Vui lòng chọn một lớp trong danh sách trước khi xem sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Form_DSSV frm = new Form_DSSV(classIdCu);
            frm.ShowDialog();
        }

        private void BtnTim_Click(object sender, EventArgs e) { currentPage = 1; LoadData(); }
        private void BtnFirst_Click(object sender, EventArgs e) { currentPage = 1; LoadData(); }
        private void BtnPrev_Click(object sender, EventArgs e) { if (currentPage > 1) { currentPage--; LoadData(); } }
        private void BtnNext_Click(object sender, EventArgs e) { if (currentPage < totalPages) { currentPage++; LoadData(); } }
        private void BtnLast_Click(object sender, EventArgs e) { currentPage = totalPages; LoadData(); }

        private void btnLast_Click_1(object sender, EventArgs e)
        {

        }
    }
}