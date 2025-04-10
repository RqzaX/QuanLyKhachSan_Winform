﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan
{
    public partial class TrangChu : Form
    {
        public TrangChu()
        {
            InitializeComponent();
        }
        private void LoadFormIntoPanel(Form form)
        {
            panelChinh.Controls.Clear();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panelChinh.Controls.Add(form);
            form.Show();
            contextMenuProfile.BackColor = Color.LightSteelBlue;
            contextMenuProfile.ForeColor = Color.Black;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TrangChu2 form = new TrangChu2();
            LoadFormIntoPanel(form);
        }

        private void TrangChu_Load(object sender, EventArgs e)
        {
            TrangChu2 form = new TrangChu2();
            LoadFormIntoPanel(form);
        }

        private void btnQuyDinh_Click(object sender, EventArgs e)
        {
            FrmQuyDinh frm = new FrmQuyDinh();
            LoadFormIntoPanel(frm);
        }

        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            DatPhong datPhong = new DatPhong();
            LoadFormIntoPanel(datPhong);
        }

        private void btnMyProfile_Click(object sender, EventArgs e)
        {
            contextMenuProfile.Show(btnMyProfile, new Point(0, btnMyProfile.Height));
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void btnKhach_Click(object sender, EventArgs e)
        {
            Khach k = new Khach();
            LoadFormIntoPanel(k);
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            FrmNhanVien frmNhanVien = new FrmNhanVien();
            LoadFormIntoPanel(frmNhanVien);
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            FrmHoaDon frmHoa = new FrmHoaDon();
            LoadFormIntoPanel(frmHoa);
        }

        private void btnDichVu_Click(object sender, EventArgs e)
        {
            FrmDichVu frm = new FrmDichVu();
            LoadFormIntoPanel(frm);
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            FrmBaoCao frm = new FrmBaoCao();
            LoadFormIntoPanel(frm);
        }
    }
}
