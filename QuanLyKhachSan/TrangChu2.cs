using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;


namespace QuanLyKhachSan
{
    public partial class TrangChu2 : Form
    {
        QLKSDataContext db = new QLKSDataContext();
        public TrangChu2()
        {
            InitializeComponent();
        }

        private void TrangChu2_Load(object sender, EventArgs e)
        {
            int tongSoPhong, phongTrong, phongSuDung, baoTri;
            tongSoPhong = db.Phongs.Count();
            phongTrong = db.Phongs.Count(p => p.trang_thai == "trong");
            phongSuDung = db.Phongs.Count(p => p.trang_thai == "dang_su_dung");
            baoTri = db.Phongs.Count(p => p.trang_thai == "bao_tri");
            btnThongTinPhong.Text = $"Tổng số phòng: {tongSoPhong}\n- Phòng trống: {phongTrong}\n- Phòng đã sử dụng: {phongSuDung}\n- Phòng bảo trì: {baoTri}";
            rdoPhong.Checked = true;
        }
        private void DoSearch()
        {
            string term = txtTimKiem.Text.Trim();
            if (term == "")
            {
                dgvTimKiem.DataSource = null;
                return;
            }
            if (rdoPhong.Checked)
            {
                // Tìm theo số phòng (so_phong) hoặc trạng thái
                var ds = (from p in db.Phongs
                          join lp in db.LoaiPhongs on p.loai_phong_id equals lp.loai_phong_id
                          where p.so_phong.Contains(term) || lp.ten_loai.Contains(term)
                          orderby p.so_phong
                          select new
                          {
                              ID = p.phong_id,
                              SoPhong = p.so_phong,
                              Loai = lp.ten_loai,
                              TrangThai = p.trang_thai == "trong" ? "Trống"
                                     : p.trang_thai == "dang_su_dung" ? "Đang sử dụng"
                                     : p.trang_thai == "bao_tri" ? "Bảo trì"
                                     : p.trang_thai == "da_dat" ? "Đã đặt"
                                     : p.trang_thai,
                          })
                         .ToList();
                dgvTimKiem.DataSource = ds;
            }
            else if (rdoKhach.Checked)
            {
                // Tìm theo tên khách hoặc CCCD
                var ds = db.KhachHangs
                           .Where(k => k.ho_ten.Contains(term)
                                    || k.cccd.Contains(term)
                                    || k.so_dien_thoai.Contains(term))
                           .Select(k => new
                           {
                               ID = k.khach_hang_id,
                               HoTen = k.ho_ten,
                               SDT = k.so_dien_thoai,
                               CCCD = k.cccd,
                               Email = k.email
                           })
                           .ToList();
                dgvTimKiem.DataSource = ds;
            }
            else if (rdoNhanVien.Checked)
            {
                // Tìm theo tên NV hoặc SĐT hoặc tài khoản
                var ds = (from nv in db.NhanViens
                          join vt in db.VaiTros on nv.vai_tro_id equals vt.vai_tro_id
                          where nv.ho_ten.Contains(term)
                                || nv.sdt.Contains(term)
                                || nv.tai_khoan.Contains(term)
                          select new
                          {
                              ID = nv.nhan_vien_id,
                              HoTen = nv.ho_ten,
                              SDT = nv.sdt,
                              ChucVu = vt.ten_vai_tro,
                              TaiKhoan = nv.tai_khoan
                          })
                          .ToList();
                dgvTimKiem.DataSource = ds;
            }
            else if (rdoHoaDon.Checked)
            {
                // Tìm theo mã hóa đơn, ngày lập, mã phòng, tên NV
                var ds = (from hd in db.HoaDons
                          join dp in db.DatPhongs on hd.dat_phong_id equals dp.dat_phong_id
                          join nv in db.NhanViens on hd.nhan_vien_id equals nv.nhan_vien_id
                          join p in db.Phongs on dp.phong_id equals p.phong_id
                          where SqlMethods.Like(hd.hoa_don_id.ToString(), $"%{term}%")
                             || nv.ho_ten.Contains(term)
                             || p.so_phong.Contains(term)
                             || SqlMethods.Like(hd.ngay_tao.ToString(), $"%{term}%")
                          select new
                          {
                              MaHD = hd.hoa_don_id,
                              NgayLap = hd.ngay_tao,
                              SoPhong = p.so_phong,
                              NhanVien = nv.ho_ten,
                              ThanhToan = hd.ThanhToans.Sum(t => t.so_tien)  // nếu muốn tổng
                          })
                          .ToList();
                dgvTimKiem.DataSource = ds;
            }

            if (dgvTimKiem.Columns.Count > 0)
            {
                dgvTimKiem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

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
            DoSearch();
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

        private void rdoPhong_CheckedChanged(object sender, EventArgs e)
        {
            DoSearch();
        }

        private void rdoKhach_CheckedChanged(object sender, EventArgs e)
        {
            DoSearch();
        }

        private void rdoNhanVien_CheckedChanged(object sender, EventArgs e)
        {
            DoSearch();
        }

        private void rdoHoaDon_CheckedChanged(object sender, EventArgs e)
        {
            DoSearch();
        }
    }
}
