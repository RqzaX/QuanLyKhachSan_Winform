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
    public partial class ChiTietPhongDatTruoc : Form
    {
        public ChiTietPhongDatTruoc()
        {
            InitializeComponent();
        }

        private void btnChuyenPhong_Click(object sender, EventArgs e)
        {
            ChonPhongChuyen frm = new ChonPhongChuyen();
            frm.ShowDialog();
        }
    }
}
