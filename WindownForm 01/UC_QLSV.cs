using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace WindownForm_01
{
    public partial class UC_QLSV : UserControl
    {
        public UC_QLSV()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;

            MSSV.DataPropertyName = "MSSV";
            Column1.DataPropertyName = "FullName";
            Column2.DataPropertyName = "Gender";
            Column3.DataPropertyName = "DateOfBirth";
            Column4.DataPropertyName = "ClassName";

            // Giới tính
            comboBox2.Items.Add("Nam");
            comboBox2.Items.Add("Nữ");

            // Lớp
            comboBox3.Items.Add("68PM1");
            comboBox3.Items.Add("68PM2");

            // Chọn mặc định
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

            // Load dữ liệu
            LoadData();
        }

        void LoadData()
        {
            SqlConnection conn = DBconnect.GetConnection();

            try
            {
                conn.Open();

                string sql = "SELECT * FROM Students";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = DBconnect.GetConnection();

            try
            {
                conn.Open();

                string sql = @"
        INSERT INTO Students
        (MSSV, FullName, Gender, DateOfBirth, ClassId)
        VALUES
        (@MSSV, @FullName, @Gender, @DateOfBirth, @ClassId)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@MSSV", txt_mssv.Text);
                cmd.Parameters.AddWithValue("@FullName", txt_name.Text);
                cmd.Parameters.AddWithValue("@Gender", comboBox2.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", dateTimePicker2.Value);

                // 68PM1 = 1 | 68PM2 = 2
                cmd.Parameters.AddWithValue("@ClassId", comboBox3.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Thêm sinh viên thành công");

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}