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
            MessageBox.Show("Chức năng đang trong tình trạng Phát triển.", "Thử lại sau", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            DatPhong datPhong = new DatPhong();
            LoadFormIntoPanel(datPhong);
        }
    }
}
