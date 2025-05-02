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
    public partial class FrmQuyDinh : Form
    {
        private bool checkKH = true, checkNV = true;
        public FrmQuyDinh()
        {
            InitializeComponent();
        }

        private void btnSuaNQKH_Click(object sender, EventArgs e)
        {
            if(checkKH)
            {
                txtNQKH.ReadOnly = false;
                btnSuaNQKH.Text = "Lưu lại";
                checkKH = false;
            } else
            {
                txtNQKH.ReadOnly = true;
                btnSuaNQKH.Text = "Sửa nội quy khách hàng";
                checkKH = true;
            }
        }

        private void btnSuaNQNV_Click(object sender, EventArgs e)
        {
            if (checkNV)
            {
                txtNQNV.ReadOnly = false;
                btnSuaNQNV.Text = "Lưu lại";
                checkNV = false;
            }
            else
            {
                txtNQNV.ReadOnly = true;
                btnSuaNQNV.Text = "Sửa nội quy nhân viên";
                checkNV = true;
            }
        }
    }
}
