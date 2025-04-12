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
            txtTimKiem.Text = "Tìm kiếm nhân viên";
            txtTimKiem.ForeColor = Color.Gray;
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

        private void FrmNhanVien_Enter(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "Tìm kiếm nhân viên")
            {
                txtTimKiem.Text = "";
                txtTimKiem.ForeColor = Color.Black;
            }
        }

        private void txtTimKiem_Leave(object sender, EventArgs e)
        {
            if (txtTimKiem.Text.Length == 0)
            {
                txtTimKiem.Text = "Tìm kiếm nhân viên";
                txtTimKiem.ForeColor = Color.Gray;
            }
        }

        private void btnThemChucVu_Click(object sender, EventArgs e)
        {
            ThemChucVu frm = new ThemChucVu();
            frm.ShowDialog();
        }
    }
}
