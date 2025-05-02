namespace QuanLyKhachSan
{
    partial class FrmQuyDinh
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmQuyDinh));
            this.label1 = new System.Windows.Forms.Label();
            this.txtNQKH = new System.Windows.Forms.TextBox();
            this.txtNQNV = new System.Windows.Forms.TextBox();
            this.btnSuaNQKH = new System.Windows.Forms.Button();
            this.btnSuaNQNV = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(466, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(664, 69);
            this.label1.TabIndex = 9;
            this.label1.Text = "NỘI QUY KHÁCH SẠN";
            // 
            // txtNQKH
            // 
            this.txtNQKH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNQKH.Location = new System.Drawing.Point(152, 156);
            this.txtNQKH.Multiline = true;
            this.txtNQKH.Name = "txtNQKH";
            this.txtNQKH.ReadOnly = true;
            this.txtNQKH.Size = new System.Drawing.Size(651, 747);
            this.txtNQKH.TabIndex = 10;
            this.txtNQKH.Text = resources.GetString("txtNQKH.Text");
            // 
            // txtNQNV
            // 
            this.txtNQNV.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNQNV.Location = new System.Drawing.Point(840, 156);
            this.txtNQNV.Multiline = true;
            this.txtNQNV.Name = "txtNQNV";
            this.txtNQNV.ReadOnly = true;
            this.txtNQNV.Size = new System.Drawing.Size(651, 747);
            this.txtNQNV.TabIndex = 11;
            this.txtNQNV.Text = resources.GetString("txtNQNV.Text");
            // 
            // btnSuaNQKH
            // 
            this.btnSuaNQKH.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSuaNQKH.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuaNQKH.ForeColor = System.Drawing.Color.White;
            this.btnSuaNQKH.Location = new System.Drawing.Point(152, 920);
            this.btnSuaNQKH.Name = "btnSuaNQKH";
            this.btnSuaNQKH.Size = new System.Drawing.Size(305, 46);
            this.btnSuaNQKH.TabIndex = 12;
            this.btnSuaNQKH.Text = "Sửa nội quy khách hàng";
            this.btnSuaNQKH.UseVisualStyleBackColor = false;
            this.btnSuaNQKH.Click += new System.EventHandler(this.btnSuaNQKH_Click);
            // 
            // btnSuaNQNV
            // 
            this.btnSuaNQNV.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSuaNQNV.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuaNQNV.ForeColor = System.Drawing.Color.White;
            this.btnSuaNQNV.Location = new System.Drawing.Point(840, 920);
            this.btnSuaNQNV.Name = "btnSuaNQNV";
            this.btnSuaNQNV.Size = new System.Drawing.Size(305, 46);
            this.btnSuaNQNV.TabIndex = 13;
            this.btnSuaNQNV.Text = "Sửa nội quy nhân viên";
            this.btnSuaNQNV.UseVisualStyleBackColor = false;
            this.btnSuaNQNV.Click += new System.EventHandler(this.btnSuaNQNV_Click);
            // 
            // FrmQuyDinh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1641, 999);
            this.ControlBox = false;
            this.Controls.Add(this.btnSuaNQNV);
            this.Controls.Add(this.btnSuaNQKH);
            this.Controls.Add(this.txtNQNV);
            this.Controls.Add(this.txtNQKH);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmQuyDinh";
            this.Text = "FrmBaoCao";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNQKH;
        private System.Windows.Forms.TextBox txtNQNV;
        private System.Windows.Forms.Button btnSuaNQKH;
        private System.Windows.Forms.Button btnSuaNQNV;
    }
}