using System;
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
    public partial class ChiTietPhongSuDung : Form
    {
        public ChiTietPhongSuDung()
        {
            InitializeComponent();
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) dateTimePicker3.Enabled = true;
            else dateTimePicker3.Enabled = false;
        }

        private void ChiTietPhongSuDung_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void btnChuyenPhong_Click(object sender, EventArgs e)
        {
            ChonPhongChuyen frm = new ChonPhongChuyen();
            frm.ShowDialog();
        }
    }
}
