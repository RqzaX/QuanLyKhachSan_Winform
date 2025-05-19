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
            // 1. Ngắt DataSource và xóa hết column cũ
            dgvDatPhong.DataSource = null;
            dgvDatPhong.Columns.Clear();

            var ds = from dp in db.DatPhongs
                     join kh in db.KhachHangs on dp.khach_hang_id equals kh.khach_hang_id
                     join p in db.Phongs on dp.phong_id equals p.phong_id
                     join nv in db.NhanViens on dp.nhan_vien_id equals nv.nhan_vien_id
                     join km in db.KhuyenMais on dp.khuyen_mai_id equals km.khuyen_mai_id into kmJoin
                     from km in kmJoin.DefaultIfEmpty()
                     select new
                     {
                         dp.dat_phong_id,
                         dp.khach_hang_id,
                         dp.phong_id,
                         dp.khuyen_mai_id,
                         dp.nhan_vien_id,
                         TenKhach = kh.ho_ten,
                         SoPhong = p.so_phong,
                         TenKhuyenMai = km != null ? km.ten_khuyen_mai : "",
                         TenNhanVien = nv.ho_ten,
                         dp.ngay_check_in,
                         dp.ngay_check_out,
                         dp.ngay_dat,
                         TrangThai = GetStatusName(dp.trang_thai)
                     };

            dgvDatPhong.DataSource = ds.ToList();

            // Ẩn các ID
            dgvDatPhong.Columns["dat_phong_id"].Visible = false;
            dgvDatPhong.Columns["khach_hang_id"].Visible = false;
            dgvDatPhong.Columns["phong_id"].Visible = false;
            dgvDatPhong.Columns["khuyen_mai_id"].Visible = false;
            dgvDatPhong.Columns["nhan_vien_id"].Visible = false;

            dgvDatPhong.Columns["TenKhach"].HeaderText = "Tên khách hàng";
            dgvDatPhong.Columns["SoPhong"].HeaderText = "Số phòng";
            dgvDatPhong.Columns["TenKhuyenMai"].HeaderText = "Khuyến mãi";
            dgvDatPhong.Columns["TenNhanVien"].HeaderText = "Nhân viên";
            dgvDatPhong.Columns["ngay_check_in"].HeaderText = "Check-in";
            dgvDatPhong.Columns["ngay_check_out"].HeaderText = "Check-out";
            dgvDatPhong.Columns["ngay_dat"].HeaderText = "Ngày đặt";
            dgvDatPhong.Columns["TrangThai"].HeaderText = "Trạng thái";

            dgvDatPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //Thêm cột button “Xóa” nếu chưa có
            if (!dgvDatPhong.Columns.Contains("ThaoTac"))
            {
                var btnCol = new DataGridViewButtonColumn()
                {
                    Name = "ThaoTac",
                    HeaderText = "Xóa",
                    Text = "❌",
                    UseColumnTextForButtonValue = true,
                    Width = 50
                };
                dgvDatPhong.Columns.Add(btnCol);
            }
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
            string status = txtTrangThai.Text.Trim();
            switch(status)
            {
                case "Trống":
                    status = "trong";
                    break;
                case "Đã đặt":
                    status = "da_dat";
                    break;
                case "Đang sử dụng":
                    status = "dang_su_dung";
                    break;
                case "Đã thanh toán":
                    status = "da_thanh_toan";
                    break;
                case "Hủy đặt":
                    status = "huy_dat";
                    break;
                case "Quán hạn check-in":
                    status = "qua_han_check_in";
                    break;
                default:
                    MessageBox.Show("Trạng thái không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            dp.trang_thai = status;

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
                "da_thanh_toan" => "Đã thanh toán",
                "huy_dat" => "Hủy đặt phòng",
                "qua_han_check_in" => "Quá hạn check-in",
                _ => code
            };
        }

        private void dgvDatPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDatPhong.Columns[e.ColumnIndex].Name == "ThaoTac") return;

            var row = dgvDatPhong.Rows[e.RowIndex];

            txtMaDP.Text = row.Cells["dat_phong_id"].Value?.ToString();
            txtMaKH.Text = row.Cells["khach_hang_id"].Value?.ToString();
            txtMaPhong.Text = row.Cells["phong_id"].Value?.ToString();
            txtMaKM.Text = row.Cells["khuyen_mai_id"].Value?.ToString();
            txtMaNV.Text = row.Cells["nhan_vien_id"].Value?.ToString();
            txtTrangThai.Text = row.Cells["TrangThai"].Value?.ToString();

            dtNgayDat.Value = (DateTime)row.Cells["ngay_dat"].Value;
            dtNgayCheckIn.Value = (DateTime)row.Cells["ngay_check_in"].Value;
            dtNgayCheckOut.Value = (DateTime)row.Cells["ngay_check_out"].Value;

            txtTenKH.Text = row.Cells["TenKhach"].Value?.ToString();
            txtSoPhong.Text = row.Cells["SoPhong"].Value?.ToString();
            txtTenKM.Text = row.Cells["TenKhuyenMai"].Value?.ToString();
            txtTenNV.Text = row.Cells["TenNhanVien"].Value?.ToString();
        }


        private void dgvDatPhong_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDatPhong.Columns[e.ColumnIndex].Name != "ThaoTac")
                return;

            string soPhong = dgvDatPhong.Rows[e.RowIndex].Cells["SoPhong"].Value?.ToString();
            DateTime checkIn = (DateTime)dgvDatPhong.Rows[e.RowIndex].Cells["ngay_check_in"].Value;

            if (MessageBox.Show($"Xóa đặt phòng cho phòng {soPhong}?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            int phongId = db.Phongs
                .Where(p => p.so_phong == soPhong)
                .Select(p => p.phong_id)
                .FirstOrDefault();

            var booking = db.DatPhongs.FirstOrDefault(d => d.phong_id == phongId && d.ngay_check_in == checkIn);
            if (booking != null)
            {
                var phong = db.Phongs.FirstOrDefault(p => p.phong_id == phongId);
                if (phong != null)
                {
                    phong.trang_thai = "trong";
                }
                db.DatPhongs.DeleteOnSubmit(booking);
                db.SubmitChanges();
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            } else
            {
                MessageBox.Show("Không tìm thấy đặt phòng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}