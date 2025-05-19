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
    public partial class QL_DatPhong : Form
    {
        QLKSDataContext db = new QLKSDataContext();
        public QL_DatPhong()
        {
            InitializeComponent();
        }

        private void QL_DatPhong_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        public void LoadData()
        {
            var raw = db.DatPhongs
                .Select(dp => new {
                    dp.dat_phong_id,
                    dp.khach_hang_id,
                    dp.phong_id,
                    dp.khuyen_mai_id,
                    dp.nhan_vien_id,
                    dp.ngay_check_in,
                    dp.ngay_check_out,
                    dp.ngay_dat,
                    dp.trang_thai
                }).ToList();

            var list = raw
                .Select(x => new {
                    DatPhongID = x.dat_phong_id,
                    KhachHangID = x.khach_hang_id,
                    PhongID = x.phong_id,
                    KhuyenMaiID = x.khuyen_mai_id,
                    NhanVienID = x.nhan_vien_id,
                    NgayCheckIn = x.ngay_check_in,
                    NgayCheckOut = x.ngay_check_out,
                    NgayDat = x.ngay_dat,
                    TrangThai = GetStatusName(x.trang_thai)  // <-- gọi được vì đã là in-memory
                }).ToList();

            dgvDatPhong.DataSource = list;

            dgvDatPhong.Columns["DatPhongID"].HeaderText = "Đặt phòng ID";
            dgvDatPhong.Columns["KhachHangID"].HeaderText = "Khách hàng ID";
            dgvDatPhong.Columns["PhongID"].HeaderText = "Phòng ID";
            dgvDatPhong.Columns["KhuyenMaiID"].HeaderText = "Khuyến mãi ID";
            dgvDatPhong.Columns["NhanVienID"].HeaderText = "Nhân viên ID";
            dgvDatPhong.Columns["NgayCheckIn"].HeaderText = "Ngày check-in";
            dgvDatPhong.Columns["NgayCheckOut"].HeaderText = "Ngày check-out";
            dgvDatPhong.Columns["NgayDat"].HeaderText = "Ngày đặt";
            dgvDatPhong.Columns["TrangThai"].HeaderText = "Trạng thái";

            ClearForm();
        }
        private void btnXoaAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Bạn có chắc muốn xóa tất cả đặt phòng?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            // Đổi tất cả phòng về trạng thái "trống"
            var allRooms = db.Phongs.ToList();
            foreach (var room in allRooms)
            {
                room.trang_thai = "trong";
            }

            // Xóa hết booking
            db.DatPhongs.DeleteAllOnSubmit(db.DatPhongs);

            db.SubmitChanges();

            MessageBox.Show("Đã xóa tất cả đặt phòng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMaDP.Text, out int id))
            {
                MessageBox.Show("ID không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var dp = db.DatPhongs.SingleOrDefault(x => x.dat_phong_id == id);
            if (dp == null)
            {
                MessageBox.Show("Không tìm thấy đặt phòng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Gán giá trị từ form lên entity
            dp.khach_hang_id = int.Parse(txtMaKH.Text);
            dp.phong_id = int.Parse(txtMaPhong.Text);
            dp.khuyen_mai_id = string.IsNullOrWhiteSpace(txtMaKM.Text)
                                ? (int?)null
                                : int.Parse(txtMaKM.Text);
            dp.nhan_vien_id = int.Parse(txtMaNV.Text);
            dp.ngay_check_in = dtNgayCheckIn.Value.Date;
            dp.ngay_check_out = dtNgayCheckOut.Value.Date;
            dp.ngay_dat = dtNgayDat.Value.Date;
            dp.trang_thai = txtTrangThai.Text.Trim();

            db.SubmitChanges();
            MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }
        private void ClearForm()
        {
            txtMaDP.Clear();
            txtMaKH.Clear();
            txtMaKM.Clear();
            txtMaNV.Clear();
            txtMaPhong.Clear();
            txtTrangThai.Clear();

            dtNgayCheckIn.Value = DateTime.Today;
            dtNgayCheckOut.Value = DateTime.Today;
            dtNgayDat.Value = DateTime.Today;
        }
        private string GetStatusName(string code)
        {
            return code switch
            {
                "trong" => "Trống",
                "da_dat" => "Đã đặt",
                "dang_su_dung" => "Đang sử dụng",
                _ => code
            };
        }

        private void dgvDatPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Lấy dòng vừa click
            var row = dgvDatPhong.Rows[e.RowIndex];

            txtMaDP.Text = row.Cells["DatPhongID"].Value?.ToString() ?? "";
            txtMaKH.Text = row.Cells["KhachHangID"].Value?.ToString() ?? "";
            txtMaPhong.Text = row.Cells["PhongID"].Value?.ToString() ?? "";
            txtMaKM.Text = row.Cells["KhuyenMaiID"].Value?.ToString() ?? "";
            txtMaNV.Text = row.Cells["NhanVienID"].Value?.ToString() ?? "";

            // alias ngày thành DateTime ngay từ binding thì:
            if (DateTime.TryParse(row.Cells["NgayCheckIn"].Value?.ToString(), out var ci))
                dtNgayCheckIn.Value = ci;
            else
                dtNgayCheckIn.Value = DateTime.Today;

            if (DateTime.TryParse(row.Cells["NgayCheckOut"].Value?.ToString(), out var co))
                dtNgayCheckOut.Value = co;
            else
                dtNgayCheckOut.Value = DateTime.Today;

            if (DateTime.TryParse(row.Cells["NgayDat"].Value?.ToString(), out var nd))
                dtNgayDat.Value = nd;
            else
                dtNgayDat.Value = DateTime.Today;

            txtTrangThai.Text = row.Cells["TrangThai"].Value?.ToString() ?? "";
        }
    }
}