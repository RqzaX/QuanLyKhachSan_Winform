namespace QuanLyKhachSan
{
    partial class DatPhong
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtTimPhong = new System.Windows.Forms.TextBox();
            this.btnTimPhong = new System.Windows.Forms.Button();
            this.cbbLoaiPhong = new System.Windows.Forms.ComboBox();
            this.btnTaoPhongMoi = new System.Windows.Forms.Button();
            this.panelPhong = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(583, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(386, 69);
            this.label1.TabIndex = 0;
            this.label1.Text = "ĐẶT PHÒNG";
            // 
            // txtTimPhong
            // 
            this.txtTimPhong.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimPhong.Location = new System.Drawing.Point(41, 202);
            this.txtTimPhong.Name = "txtTimPhong";
            this.txtTimPhong.Size = new System.Drawing.Size(420, 38);
            this.txtTimPhong.TabIndex = 1;
            // 
            // btnTimPhong
            // 
            this.btnTimPhong.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnTimPhong.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimPhong.ForeColor = System.Drawing.Color.White;
            this.btnTimPhong.Location = new System.Drawing.Point(460, 199);
            this.btnTimPhong.Name = "btnTimPhong";
            this.btnTimPhong.Size = new System.Drawing.Size(111, 46);
            this.btnTimPhong.TabIndex = 2;
            this.btnTimPhong.Text = "Tìm";
            this.btnTimPhong.UseVisualStyleBackColor = false;
            // 
            // cbbLoaiPhong
            // 
            this.cbbLoaiPhong.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbLoaiPhong.FormattingEnabled = true;
            this.cbbLoaiPhong.Location = new System.Drawing.Point(595, 203);
            this.cbbLoaiPhong.Name = "cbbLoaiPhong";
            this.cbbLoaiPhong.Size = new System.Drawing.Size(268, 37);
            this.cbbLoaiPhong.TabIndex = 3;
            // 
            // btnTaoPhongMoi
            // 
            this.btnTaoPhongMoi.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnTaoPhongMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTaoPhongMoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaoPhongMoi.ForeColor = System.Drawing.Color.White;
            this.btnTaoPhongMoi.Location = new System.Drawing.Point(1253, 191);
            this.btnTaoPhongMoi.Name = "btnTaoPhongMoi";
            this.btnTaoPhongMoi.Size = new System.Drawing.Size(309, 58);
            this.btnTaoPhongMoi.TabIndex = 4;
            this.btnTaoPhongMoi.Text = "Tạo phòng mới";
            this.btnTaoPhongMoi.UseVisualStyleBackColor = false;
            this.btnTaoPhongMoi.Click += new System.EventHandler(this.btnTaoPhongMoi_Click);
            // 
            // panelPhong
            // 
            this.panelPhong.AutoScroll = true;
            this.panelPhong.BackColor = System.Drawing.Color.White;
            this.panelPhong.Location = new System.Drawing.Point(41, 263);
            this.panelPhong.Name = "panelPhong";
            this.panelPhong.Size = new System.Drawing.Size(1521, 771);
            this.panelPhong.TabIndex = 5;
            // 
            // DatPhong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1604, 1046);
            this.ControlBox = false;
            this.Controls.Add(this.panelPhong);
            this.Controls.Add(this.btnTaoPhongMoi);
            this.Controls.Add(this.cbbLoaiPhong);
            this.Controls.Add(this.btnTimPhong);
            this.Controls.Add(this.txtTimPhong);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DatPhong";
            this.Text = "DatPhong";
            this.Load += new System.EventHandler(this.DatPhong_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTimPhong;
        private System.Windows.Forms.Button btnTimPhong;
        private System.Windows.Forms.ComboBox cbbLoaiPhong;
        private System.Windows.Forms.Button btnTaoPhongMoi;
        private System.Windows.Forms.Panel panelPhong;
    }
}