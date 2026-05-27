using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
namespace WindownForm_01
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txt_dangnhap_Click(object sender, EventArgs e)
        {
          
            if (txt_email.Text == "hoang@gmail.com" && txt_mssv.Text == "123456")
            {

                MainForm main = new MainForm();
                this.Hide();
                main.ShowDialog();
                this.Close();
            }
            else
            {
                // Báo lỗi nếu nhập sai
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}
