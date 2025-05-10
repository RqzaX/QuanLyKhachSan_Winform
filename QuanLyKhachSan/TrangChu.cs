using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan
{
    public partial class TrangChu : Form
    {
        private string _hoTen, _chucVu;
        public TrangChu(string HoTen, string ChucVu)
        {
            InitializeComponent();
            //panel1.GetType()
            //      .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
            //      .SetValue(panel1, true, null);

            //panel1.Paint += panel1_Paint;
            //panel1.Resize += (s, e) => panel1.Invalidate();

            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                // Ví dụ: gán icon, khởi tạo dữ liệu, gọi service,...
                _hoTen = HoTen;
                _chucVu = ChucVu;
                lbHoTen.Text = HoTen;
                lbChucVu.Text = ChucVu;
            }
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
            FrmDatPhong datPhong = new FrmDatPhong();
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
            InfoNhanVien.CurrentUser = null;
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void btnKhach_Click(object sender, EventArgs e)
        {
            FrmKhachHang k = new FrmKhachHang();
            LoadFormIntoPanel(k);
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            FrmNhanVien frm = new FrmNhanVien();
            LoadFormIntoPanel(frm);
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            FrmHoaDon frm = new FrmHoaDon();
            LoadFormIntoPanel(frm);
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

        private void btnPhong_Click(object sender, EventArgs e)
        {
            FrmPhong frm = new FrmPhong();
            LoadFormIntoPanel(frm);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //Color cStart = Color.FromArgb(72, 209, 204);
            //Color cEnd = Color.FromArgb(37, 187, 179);

            //using (var brush = new LinearGradientBrush(
            //    panel1.ClientRectangle,
            //    cStart,
            //    cEnd,
            //    LinearGradientMode.Horizontal))
            //{
            //    e.Graphics.FillRectangle(brush, panel1.ClientRectangle);
            //}
        }

        private void btnTrangChu_MouseEnter(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.Black;
        }

        private void btnTrangChu_MouseLeave(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.White;
        }

        private void btnDatPhong_MouseEnter(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.Black;
        }

        private void btnDatPhong_MouseLeave(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.White;
        }

        private void btnKhach_MouseEnter(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.Black;
        }

        private void btnKhach_MouseLeave(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.White;
        }

        private void btnNhanVien_MouseEnter(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.Black;
        }

        private void btnNhanVien_MouseLeave(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.White;
        }

        private void btnPhong_MouseEnter(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.Black;
        }

        private void btnPhong_MouseLeave(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.White;
        }

        private void btnHoaDon_MouseEnter(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.Black;
        }

        private void btnHoaDon_MouseLeave(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.White;
        }

        private void btnDichVu_MouseEnter(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.Black;
        }

        private void btnDichVu_MouseLeave(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.White;
        }

        private void btnBaoCao_MouseEnter(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.Black;
        }

        private void btnBaoCao_MouseLeave(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.White;
        }

        private void btnQuyDinh_MouseEnter(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.Black;
        }

        private void btnQuyDinh_MouseLeave(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.ForeColor = Color.White;
        }

        private void quảnLýKhuyếnMãiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QL_KhuyenMai f = new QL_KhuyenMai();
            f.ShowDialog();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void báoCáoĐặtPhòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportDatPhong f = new ReportDatPhong();
            f.ShowDialog();
        }

        private void báoCáoKháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportKhachHang f = new ReportKhachHang();
            f.ShowDialog();
        }

        private void báoCáoNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportNhanVien f = new ReportNhanVien();
            f.ShowDialog();
        }

        private void báoCáoPhòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportPhong f = new ReportPhong();
            f.ShowDialog();
        }

        private void TrangChu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
