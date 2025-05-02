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
    public partial class FrmNhanVien : Form
    {
        public FrmNhanVien()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtTaiKhoan.Enabled = checkBox1.Checked;
            txtMatKhau.Enabled = checkBox1.Checked;
        }

        private void FrmNhanVien_Load(object sender, EventArgs e)
        {
            txtTaiKhoan.Enabled = false;
            txtMatKhau.Enabled = false;
        }

        private void btnThemChucVu_Click(object sender, EventArgs e)
        {
            ThemChucVu frm = new ThemChucVu();
            frm.ShowDialog();
        }
    }
}
