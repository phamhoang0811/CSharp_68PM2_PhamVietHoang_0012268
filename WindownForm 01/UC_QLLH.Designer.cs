namespace WindownForm_01
{
    partial class UC_QLLH
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTenLop = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMaLop = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMaID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnXemDSSV = new System.Windows.Forms.Button();
            this.labelTimKiem = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnTim = new System.Windows.Forms.Button();
            this.dgvLopHoc = new System.Windows.Forms.DataGridView();
            this.colMaID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaLop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenLop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGhiChu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblPhanTrang = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLopHoc)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.groupBox1.Controls.Add(this.txtGhiChu);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtTenLop);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtMaLop);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtMaID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(18, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 350);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin lớp học";
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtGhiChu.Location = new System.Drawing.Point(18, 280);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(314, 31);
            this.txtGhiChu.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label4.Location = new System.Drawing.Point(18, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 25);
            this.label4.TabIndex = 6;
            this.label4.Text = "Ghi chú:";
            // 
            // txtTenLop
            // 
            this.txtTenLop.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTenLop.Location = new System.Drawing.Point(18, 205);
            this.txtTenLop.Name = "txtTenLop";
            this.txtTenLop.Size = new System.Drawing.Size(314, 31);
            this.txtTenLop.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.Location = new System.Drawing.Point(18, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tên lớp:";
            // 
            // txtMaLop
            // 
            this.txtMaLop.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMaLop.Location = new System.Drawing.Point(18, 130);
            this.txtMaLop.Name = "txtMaLop";
            this.txtMaLop.Size = new System.Drawing.Size(314, 31);
            this.txtMaLop.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.Location = new System.Drawing.Point(18, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mã lớp:";
            // 
            // txtMaID
            // 
            this.txtMaID.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMaID.Location = new System.Drawing.Point(18, 55);
            this.txtMaID.Name = "txtMaID";
            this.txtMaID.Size = new System.Drawing.Size(314, 31);
            this.txtMaID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.Location = new System.Drawing.Point(18, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã ID:";
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(18, 380);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(169, 45);
            this.btnThem.TabIndex = 1;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = false;
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Location = new System.Drawing.Point(209, 380);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(169, 45);
            this.btnSua.TabIndex = 2;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = false;
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(18, 440);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(169, 45);
            this.btnXoa.TabIndex = 3;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(209, 440);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(169, 45);
            this.btnLamMoi.TabIndex = 4;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            // 
            // btnXemDSSV
            // 
            this.btnXemDSSV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnXemDSSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXemDSSV.ForeColor = System.Drawing.Color.White;
            this.btnXemDSSV.Location = new System.Drawing.Point(18, 500);
            this.btnXemDSSV.Name = "btnXemDSSV";
            this.btnXemDSSV.Size = new System.Drawing.Size(360, 45);
            this.btnXemDSSV.TabIndex = 5;
            this.btnXemDSSV.Text = "Xem danh sách sinh viên";
            this.btnXemDSSV.UseVisualStyleBackColor = false;
            // 
            // labelTimKiem
            // 
            this.labelTimKiem.AutoSize = true;
            this.labelTimKiem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelTimKiem.Location = new System.Drawing.Point(405, 25);
            this.labelTimKiem.Name = "labelTimKiem";
            this.labelTimKiem.Size = new System.Drawing.Size(322, 25);
            this.labelTimKiem.TabIndex = 6;
            this.labelTimKiem.Text = "Tìm kiếm (Mã ID / Mã lớp / Tên lớp):";
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Location = new System.Drawing.Point(405, 55);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(281, 26);
            this.txtTimKiem.TabIndex = 7;
            // 
            // btnTim
            // 
            this.btnTim.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnTim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTim.ForeColor = System.Drawing.Color.White;
            this.btnTim.Location = new System.Drawing.Point(698, 50);
            this.btnTim.Name = "btnTim";
            this.btnTim.Size = new System.Drawing.Size(112, 35);
            this.btnTim.TabIndex = 8;
            this.btnTim.Text = "Tìm";
            this.btnTim.UseVisualStyleBackColor = false;
            // 
            // dgvLopHoc
            // 
            this.dgvLopHoc.AllowUserToAddRows = false;
            this.dgvLopHoc.BackgroundColor = System.Drawing.Color.White;
            this.dgvLopHoc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLopHoc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaID,
            this.colMaLop,
            this.colTenLop,
            this.colGhiChu});
            this.dgvLopHoc.Location = new System.Drawing.Point(405, 100);
            this.dgvLopHoc.Name = "dgvLopHoc";
            this.dgvLopHoc.RowHeadersVisible = false;
            this.dgvLopHoc.RowHeadersWidth = 51;
            this.dgvLopHoc.RowTemplate.Height = 29;
            this.dgvLopHoc.Size = new System.Drawing.Size(861, 400);
            this.dgvLopHoc.TabIndex = 9;
            // 
            // colMaID
            // 
            this.colMaID.HeaderText = "Mã ID";
            this.colMaID.MinimumWidth = 6;
            this.colMaID.Name = "colMaID";
            this.colMaID.Width = 150;
            // 
            // colMaLop
            // 
            this.colMaLop.HeaderText = "Mã lớp";
            this.colMaLop.MinimumWidth = 6;
            this.colMaLop.Name = "colMaLop";
            this.colMaLop.Width = 125;
            // 
            // colTenLop
            // 
            this.colTenLop.HeaderText = "Tên lớp";
            this.colTenLop.MinimumWidth = 6;
            this.colTenLop.Name = "colTenLop";
            this.colTenLop.Width = 200;
            // 
            // colGhiChu
            // 
            this.colGhiChu.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGhiChu.HeaderText = "Ghi chú";
            this.colGhiChu.MinimumWidth = 6;
            this.colGhiChu.Name = "colGhiChu";
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(405, 515);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(56, 30);
            this.btnFirst.TabIndex = 10;
            this.btnFirst.Text = "<<";
            this.btnFirst.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(472, 515);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(56, 30);
            this.btnPrev.TabIndex = 11;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = true;
            // 
            // lblPhanTrang
            // 
            this.lblPhanTrang.AutoSize = true;
            this.lblPhanTrang.Location = new System.Drawing.Point(778, 525);
            this.lblPhanTrang.Name = "lblPhanTrang";
            this.lblPhanTrang.Size = new System.Drawing.Size(154, 20);
            this.lblPhanTrang.TabIndex = 12;
            this.lblPhanTrang.Text = "Trang 1/1 | 2 bản ghi";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(1148, 520);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(56, 30);
            this.btnNext.TabIndex = 13;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(1210, 520);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(56, 30);
            this.btnLast.TabIndex = 14;
            this.btnLast.Text = ">>";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click_1);
            // 
            // UC_QLLH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblPhanTrang);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.dgvLopHoc);
            this.Controls.Add(this.btnTim);
            this.Controls.Add(this.txtTimKiem);
            this.Controls.Add(this.labelTimKiem);
            this.Controls.Add(this.btnXemDSSV);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.groupBox1);
            this.Name = "UC_QLLH";
            this.Size = new System.Drawing.Size(1389, 678);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLopHoc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTenLop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMaLop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMaID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnXemDSSV;
        private System.Windows.Forms.Label labelTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.DataGridView dgvLopHoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaLop;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenLop;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGhiChu;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblPhanTrang;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnLast;
    }
}
