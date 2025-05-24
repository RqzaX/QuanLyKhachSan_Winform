namespace QuanLyKhachSan
{
    partial class FrmBaoCao
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbDoangThuTrongNgay = new System.Windows.Forms.Label();
            this.lbDateTrongNgay = new System.Windows.Forms.Label();
            this.lbDoanhThuTrongThang = new System.Windows.Forms.Label();
            this.lbDateTrongThang = new System.Windows.Forms.Label();
            this.btnXemBaoCao_DoanhThu = new System.Windows.Forms.Button();
            this.panelThongKe = new System.Windows.Forms.GroupBox();
            this.panelDichVu = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTotalServices = new System.Windows.Forms.Label();
            this.panelTongQuanDichVu = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbTongSoPhongDat = new System.Windows.Forms.Label();
            this.panelPhongDat = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lbNhanVienNgoaiCa = new System.Windows.Forms.Label();
            this.lbNhanVienTrongCa = new System.Windows.Forms.Label();
            this.lbTongNhanVien = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.lbDoangThuTrongNgay);
            this.groupBox2.Controls.Add(this.lbDateTrongNgay);
            this.groupBox2.Controls.Add(this.lbDoanhThuTrongThang);
            this.groupBox2.Controls.Add(this.lbDateTrongThang);
            this.groupBox2.Controls.Add(this.btnXemBaoCao_DoanhThu);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(43, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(386, 337);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tổng quan doanh thu";
            // 
            // lbDoangThuTrongNgay
            // 
            this.lbDoangThuTrongNgay.AutoSize = true;
            this.lbDoangThuTrongNgay.BackColor = System.Drawing.Color.RoyalBlue;
            this.lbDoangThuTrongNgay.Font = new System.Drawing.Font("Arial", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDoangThuTrongNgay.ForeColor = System.Drawing.Color.White;
            this.lbDoangThuTrongNgay.Location = new System.Drawing.Point(55, 90);
            this.lbDoangThuTrongNgay.Name = "lbDoangThuTrongNgay";
            this.lbDoangThuTrongNgay.Size = new System.Drawing.Size(248, 38);
            this.lbDoangThuTrongNgay.TabIndex = 4;
            this.lbDoangThuTrongNgay.Text = "12.345.567 vnđ";
            // 
            // lbDateTrongNgay
            // 
            this.lbDateTrongNgay.AutoSize = true;
            this.lbDateTrongNgay.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDateTrongNgay.Location = new System.Drawing.Point(55, 53);
            this.lbDateTrongNgay.Name = "lbDateTrongNgay";
            this.lbDateTrongNgay.Size = new System.Drawing.Size(277, 24);
            this.lbDateTrongNgay.TabIndex = 3;
            this.lbDateTrongNgay.Text = "Doanh thu trong ngày ??/??";
            // 
            // lbDoanhThuTrongThang
            // 
            this.lbDoanhThuTrongThang.AutoSize = true;
            this.lbDoanhThuTrongThang.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.lbDoanhThuTrongThang.Font = new System.Drawing.Font("Arial", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDoanhThuTrongThang.ForeColor = System.Drawing.Color.White;
            this.lbDoanhThuTrongThang.Location = new System.Drawing.Point(55, 202);
            this.lbDoanhThuTrongThang.Name = "lbDoanhThuTrongThang";
            this.lbDoanhThuTrongThang.Size = new System.Drawing.Size(248, 38);
            this.lbDoanhThuTrongThang.TabIndex = 2;
            this.lbDoanhThuTrongThang.Text = "12.345.567 vnđ";
            // 
            // lbDateTrongThang
            // 
            this.lbDateTrongThang.AutoSize = true;
            this.lbDateTrongThang.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDateTrongThang.Location = new System.Drawing.Point(24, 167);
            this.lbDateTrongThang.Name = "lbDateTrongThang";
            this.lbDateTrongThang.Size = new System.Drawing.Size(243, 24);
            this.lbDateTrongThang.TabIndex = 1;
            this.lbDateTrongThang.Text = "Doanh thu trong tháng ?";
            // 
            // btnXemBaoCao_DoanhThu
            // 
            this.btnXemBaoCao_DoanhThu.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnXemBaoCao_DoanhThu.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXemBaoCao_DoanhThu.ForeColor = System.Drawing.Color.White;
            this.btnXemBaoCao_DoanhThu.Location = new System.Drawing.Point(68, 279);
            this.btnXemBaoCao_DoanhThu.Name = "btnXemBaoCao_DoanhThu";
            this.btnXemBaoCao_DoanhThu.Size = new System.Drawing.Size(231, 52);
            this.btnXemBaoCao_DoanhThu.TabIndex = 0;
            this.btnXemBaoCao_DoanhThu.Text = "Xem báo cáo";
            this.btnXemBaoCao_DoanhThu.UseVisualStyleBackColor = false;
            this.btnXemBaoCao_DoanhThu.Click += new System.EventHandler(this.btnXemBaoCao_DoanhThu_Click);
            // 
            // panelThongKe
            // 
            this.panelThongKe.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelThongKe.Location = new System.Drawing.Point(36, 473);
            this.panelThongKe.Name = "panelThongKe";
            this.panelThongKe.Size = new System.Drawing.Size(779, 488);
            this.panelThongKe.TabIndex = 12;
            this.panelThongKe.TabStop = false;
            this.panelThongKe.Text = "Thống kê doanh thu các tháng";
            // 
            // panelDichVu
            // 
            this.panelDichVu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelDichVu.Location = new System.Drawing.Point(821, 473);
            this.panelDichVu.Name = "panelDichVu";
            this.panelDichVu.Size = new System.Drawing.Size(808, 488);
            this.panelDichVu.TabIndex = 13;
            this.panelDichVu.TabStop = false;
            this.panelDichVu.Text = "Thống kê dịch vụ sử dụng";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.lblTotalServices);
            this.groupBox1.Controls.Add(this.panelTongQuanDichVu);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(435, 130);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(425, 337);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tổng quan dịch vụ";
            // 
            // lblTotalServices
            // 
            this.lblTotalServices.AutoSize = true;
            this.lblTotalServices.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalServices.Location = new System.Drawing.Point(39, 243);
            this.lblTotalServices.Name = "lblTotalServices";
            this.lblTotalServices.Size = new System.Drawing.Size(109, 23);
            this.lblTotalServices.TabIndex = 5;
            this.lblTotalServices.Text = "Text dichVu";
            // 
            // panelTongQuanDichVu
            // 
            this.panelTongQuanDichVu.Location = new System.Drawing.Point(6, 29);
            this.panelTongQuanDichVu.Name = "panelTongQuanDichVu";
            this.panelTongQuanDichVu.Size = new System.Drawing.Size(413, 211);
            this.panelTongQuanDichVu.TabIndex = 6;
            this.panelTongQuanDichVu.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTongQuanDichVu_Paint);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.MediumTurquoise;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(68, 279);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(231, 52);
            this.button1.TabIndex = 5;
            this.button1.Text = "Xem báo cáo";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.White;
            this.groupBox3.Controls.Add(this.lbTongSoPhongDat);
            this.groupBox3.Controls.Add(this.panelPhongDat);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(866, 130);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(433, 337);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tổng quan phòng đặt";
            // 
            // lbTongSoPhongDat
            // 
            this.lbTongSoPhongDat.AutoSize = true;
            this.lbTongSoPhongDat.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTongSoPhongDat.Location = new System.Drawing.Point(40, 243);
            this.lbTongSoPhongDat.Name = "lbTongSoPhongDat";
            this.lbTongSoPhongDat.Size = new System.Drawing.Size(109, 23);
            this.lbTongSoPhongDat.TabIndex = 7;
            this.lbTongSoPhongDat.Text = "Text dichVu";
            // 
            // panelPhongDat
            // 
            this.panelPhongDat.Location = new System.Drawing.Point(6, 29);
            this.panelPhongDat.Name = "panelPhongDat";
            this.panelPhongDat.Size = new System.Drawing.Size(421, 211);
            this.panelPhongDat.TabIndex = 7;
            this.panelPhongDat.Paint += new System.Windows.Forms.PaintEventHandler(this.panelPhongDat_Paint);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.MediumTurquoise;
            this.button3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(79, 279);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(231, 52);
            this.button3.TabIndex = 6;
            this.button3.Text = "Xem báo cáo";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.White;
            this.groupBox6.Controls.Add(this.lbNhanVienNgoaiCa);
            this.groupBox6.Controls.Add(this.lbNhanVienTrongCa);
            this.groupBox6.Controls.Add(this.lbTongNhanVien);
            this.groupBox6.Controls.Add(this.button4);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(1305, 130);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(386, 337);
            this.groupBox6.TabIndex = 12;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Tổng quan nhân viên";
            // 
            // lbNhanVienNgoaiCa
            // 
            this.lbNhanVienNgoaiCa.AutoSize = true;
            this.lbNhanVienNgoaiCa.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNhanVienNgoaiCa.Location = new System.Drawing.Point(34, 180);
            this.lbNhanVienNgoaiCa.Name = "lbNhanVienNgoaiCa";
            this.lbNhanVienNgoaiCa.Size = new System.Drawing.Size(365, 24);
            this.lbNhanVienNgoaiCa.TabIndex = 9;
            this.lbNhanVienNgoaiCa.Text = "Tổng nhân viên ngoài ca hôm nay: 99";
            // 
            // lbNhanVienTrongCa
            // 
            this.lbNhanVienTrongCa.AutoSize = true;
            this.lbNhanVienTrongCa.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNhanVienTrongCa.Location = new System.Drawing.Point(35, 145);
            this.lbNhanVienTrongCa.Name = "lbNhanVienTrongCa";
            this.lbNhanVienTrongCa.Size = new System.Drawing.Size(364, 24);
            this.lbNhanVienTrongCa.TabIndex = 8;
            this.lbNhanVienTrongCa.Text = "Tổng nhân viên trong ca hôm nay: 99";
            // 
            // lbTongNhanVien
            // 
            this.lbTongNhanVien.AutoSize = true;
            this.lbTongNhanVien.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTongNhanVien.Location = new System.Drawing.Point(35, 102);
            this.lbTongNhanVien.Name = "lbTongNhanVien";
            this.lbTongNhanVien.Size = new System.Drawing.Size(192, 24);
            this.lbTongNhanVien.TabIndex = 5;
            this.lbTongNhanVien.Text = "Tổng nhân viên: 99";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.MediumTurquoise;
            this.button4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(80, 279);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(231, 52);
            this.button4.TabIndex = 7;
            this.button4.Text = "Xem báo cáo";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(603, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(549, 70);
            this.label1.TabIndex = 9;
            this.label1.Text = "BÁO CÁO CHUNG";
            // 
            // FrmBaoCao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1942, 999);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panelDichVu);
            this.Controls.Add(this.panelThongKe);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmBaoCao";
            this.Text = "FrmBaoCao";
            this.Load += new System.EventHandler(this.FrmBaoCao_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnXemBaoCao_DoanhThu;
        private System.Windows.Forms.GroupBox panelThongKe;
        private System.Windows.Forms.GroupBox panelDichVu;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lbDoanhThuTrongThang;
        private System.Windows.Forms.Label lbDateTrongThang;
        private System.Windows.Forms.Label lbDoangThuTrongNgay;
        private System.Windows.Forms.Label lbDateTrongNgay;
        private System.Windows.Forms.Panel panelTongQuanDichVu;
        private System.Windows.Forms.Label lblTotalServices;
        private System.Windows.Forms.Label lbTongSoPhongDat;
        private System.Windows.Forms.Panel panelPhongDat;
        private System.Windows.Forms.Label lbNhanVienNgoaiCa;
        private System.Windows.Forms.Label lbNhanVienTrongCa;
        private System.Windows.Forms.Label lbTongNhanVien;
    }
}