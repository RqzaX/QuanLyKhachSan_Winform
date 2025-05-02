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
    public partial class TrangChu2 : Form
    {
        public TrangChu2()
        {
            InitializeComponent();
        }

        private void TrangChu2_Load(object sender, EventArgs e)
        {
        }

        private void TrangChu2_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void btnGoiDonPhong_Click(object sender, EventArgs e)
        {
            GoiDonPhong frm = new GoiDonPhong();
            frm.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
