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
    public partial class FrmHoaDon : Form
    {
        public FrmHoaDon()
        {
            InitializeComponent();
            txtTimKiem.Text = "Tìm kiếm hóa đơn";
            txtTimKiem.ForeColor = Color.Gray;
        }

        private void txtTimKiem_Enter(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "Tìm kiếm hóa đơn")
            {
                txtTimKiem.Text = "";
                txtTimKiem.ForeColor = Color.Black;
            }
        }

        private void txtTimKiem_Leave(object sender, EventArgs e)
        {
            if (txtTimKiem.Text.Length == 0)
            {
                txtTimKiem.Text = "Tìm kiếm hóa đơn";
                txtTimKiem.ForeColor = Color.Gray;
            }
        }
    }
}
