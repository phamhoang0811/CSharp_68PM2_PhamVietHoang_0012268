using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindownForm_01
{
    public partial class MainForm : Form
    {
        // Khai báo các thành phần
        private Panel panelMenu;
        private Panel panelContainer;
        private Button btnQLLH;
        private Button btnQLSV;
        private Button btnDangXuat;

        // Khởi tạo các UserControl
        private UC_QLLH uc_qllh = new UC_QLLH();
        private UC_QLSV uc_qlsv = new UC_QLSV();

        public MainForm()
        {
            TaoGiaoDienBangCode();
        }

        private void TaoGiaoDienBangCode()
        {
            // 1. Cấu hình Form chính
            this.Size = new Size(1150, 720); // Kích thước hiển thị vừa vặn trên hầu hết màn hình
            this.Text = "Quản lý Sinh Viên";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // 2. Tạo thanh Menu ngang bên trên
            panelMenu = new Panel();
            panelMenu.Dock = DockStyle.Top;
            panelMenu.Height = 45;
            panelMenu.BackColor = Color.White;

            // 3. Tạo vùng chứa nội dung bên dưới
            panelContainer = new Panel();
            panelContainer.Dock = DockStyle.Fill;
            panelContainer.BackColor = Color.White;
            panelContainer.AutoScroll = true; // Bật thanh cuộn cho vùng chứa nếu nội dung bên trong quá to

            // 4. Khởi tạo các nút trên Menu
            btnQLSV = TaoNutMenu("Quản lý Sinh Viên", true);
            btnQLSV.Click += new EventHandler(btnQLSV_Click);

            btnQLLH = TaoNutMenu("Quản lý Lớp Học", false);
            btnQLLH.Click += new EventHandler(btnQLLH_Click);

            btnDangXuat = TaoNutMenu("Đăng xuất", false);
            btnDangXuat.ForeColor = Color.IndianRed;
            btnDangXuat.Click += (s, e) => {
                // Xử lý sự kiện đăng xuất quay về màn hình Login nếu cần
                this.Close();
            };

            // Thêm các nút vào thanh Menu
            panelMenu.Controls.Add(btnDangXuat);
            panelMenu.Controls.Add(btnQLLH);
            panelMenu.Controls.Add(btnQLSV);

            // Thêm các Panel vào Form chính
            this.Controls.Add(panelContainer);
            this.Controls.Add(panelMenu);

            // Hiển thị màn hình Quản lý sinh viên mặc định lúc khởi động
            ShowUserControl(uc_qlsv);
        }

        private Button TaoNutMenu(string text, bool isActive)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Dock = DockStyle.Left;
            btn.AutoSize = true;
            btn.Padding = new Padding(15, 0, 15, 0);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.WhiteSmoke;
            btn.FlatAppearance.MouseDownBackColor = Color.White;
            btn.BackColor = Color.White;
            btn.ForeColor = Color.Black;
            btn.Cursor = Cursors.Hand;

            if (isActive)
                btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            else
                btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            return btn;
        }

        private void ShowUserControl(UserControl uc)
        {
            panelContainer.Controls.Clear();

            // Giữ nguyên kích thước thiết kế gốc của UserControl để không bị bóp méo layout
            uc.Dock = DockStyle.None;
            uc.Size = new Size(1412, 760);
            uc.Location = new Point(0, 0);

            panelContainer.Controls.Add(uc);
        }

        private void btnQLLH_Click(object sender, EventArgs e)
        {
            ShowUserControl(uc_qllh);
            DoiTrangThaiNut(btnQLLH);
        }

        private void btnQLSV_Click(object sender, EventArgs e)
        {
            ShowUserControl(uc_qlsv);
            DoiTrangThaiNut(btnQLSV);
        }

        private void DoiTrangThaiNut(Button activeBtn)
        {
            btnQLSV.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btnQLLH.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            activeBtn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }
    }
}