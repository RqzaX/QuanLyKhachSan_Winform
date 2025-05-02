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
    public partial class FrmPhong : Form
    {
        public FrmPhong()
        {
            InitializeComponent();
        }

        private void btnThemLoaiPhong_Click(object sender, EventArgs e)
        {
            ThemLoaiPhong frm = new ThemLoaiPhong();
            frm.ShowDialog();
        }
    }
}
