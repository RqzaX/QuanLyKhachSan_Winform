using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan
{
    public partial class ChiTietPhongDatTruoc : Form
    {
        private QLKSDataContext db = new QLKSDataContext();
        private DatPhong _Booking;
        private Phong _Room;
        private readonly int _maPhong;
        private int _DatPhongId;
        // lưu giá trị gốc
        private string _origHoTen, _origDiaChi, _origSDT, _origEmail, _origCCCD;
        private DateTime _origNgayDat, _origCheckIn, _origCheckOut;
        public ChiTietPhongDatTruoc(int maPhong)
        {
            InitializeComponent();
            _maPhong = maPhong;
            LoadChiTiet();
            _origHoTen = txtHoTen.Text;
            _origDiaChi = txtDiaChi.Text;
            _origSDT = txtSDT.Text;
            _origEmail = txtEmail.Text;
            _origCCCD = txtCCCD.Text;
            _origNgayDat = dtNgayDat.Value.Date;
            _origCheckIn = dtNgayCheckIn.Value.Date;
            _origCheckOut = dtNgayCheckOut.Value.Date;
            int day = (dtNgayCheckOut.Value.Date - dtNgayCheckIn.Value.Date).Days;
            if (day < 1) day = 1;
            txtThoiHan.Text = $"{day} Ngày";
            cbDatTruoc.Checked = true;
            cbDatTruoc.Enabled = false;
            txtKhachId.Hide();
            txtKhuyenMaiID.Hide();
            txtSDT.MaxLength = 10;
            txtCCCD.MaxLength = 12;
            txtHoTen.Enabled = false;
            txtDiaChi.Enabled = false;
            txtSDT.Enabled = false;
            txtEmail.Enabled = false;
            txtCCCD.Enabled = false;
            btnChonKhach.Enabled = false;
        }

        private void btnChonKhach_Click(object sender, EventArgs e)
        {
            using (var f = new ChonKhachHang())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    int khId = f.SelectedCustomerId;
                    txtKhachId.Text = khId.ToString();// lưu lại nếu cần
                    LoadKhachHang(khId);
                }
            }
        }
        private void LoadKhachHang(int khachHangId)
        {
            var kh = db.KhachHangs.SingleOrDefault(k => k.khach_hang_id == khachHangId);
            if (kh == null) return; // hoặc clear form nếu không tìm thấy

            txtHoTen.Text = kh.ho_ten;
            txtDiaChi.Text = kh.dia_chi;
            txtSDT.Text = kh.so_dien_thoai;
            txtEmail.Text = kh.email;
            txtCCCD.Text = kh.cccd;

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            DateTime nd = dtNgayDat.Value.Date;
            DateTime ci = dtNgayCheckIn.Value.Date;
            DateTime co = dtNgayCheckOut.Value.Date;
            if (co < ci)
            {
                MessageBox.Show("Ngày check-out phải ≥ ngày check-in!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var datPhong = db.DatPhongs.SingleOrDefault(d => d.phong_id == _maPhong);
            var dp = db.DatPhongs.SingleOrDefault(d => d.dat_phong_id == datPhong.dat_phong_id);
            if (dp == null)
            {
                MessageBox.Show("Không tìm thấy thông tin đặt phòng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var cccdKH = db.KhachHangs.SingleOrDefault(k => k.cccd == txtCCCD.Text.Trim());
            if (cccdKH != null && cccdKH.khach_hang_id != Convert.ToInt32(txtKhachId.Text.Trim()))
            {
                MessageBox.Show("CCCD khách hàng đã tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var sdtKH = db.KhachHangs.SingleOrDefault(k => k.so_dien_thoai == txtSDT.Text.Trim());
            if (sdtKH != null && sdtKH.khach_hang_id != Convert.ToInt32(txtKhachId.Text.Trim()))
            {
                MessageBox.Show("Số điện thoại khách hàng đã tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if (thayDoiKhachHang()) txtKhachId.Text = "";

            if (!cbSuaThongTinKH.Checked)
            {
                int khId = int.Parse(txtKhachId.Text);
                dp.KhachHang = db.KhachHangs.Single(k => k.khach_hang_id == khId);
            }
            else
            {
                //int newId = AddKhachHang(txtHoTen.Text.Trim(), txtDiaChi.Text.Trim(), txtSDT.Text.Trim(), txtEmail.Text.Trim(), txtCCCD.Text.Trim());
                var newKh = new KhachHang { 
                    ho_ten = txtHoTen.Text.Trim(),
                    dia_chi = txtDiaChi.Text.Trim(),
                    so_dien_thoai = txtSDT.Text.Trim(),
                    email = txtEmail.Text.Trim(),
                    cccd = txtCCCD.Text.Trim()
                };
                db.KhachHangs.InsertOnSubmit(newKh);
                db.SubmitChanges();// newKh có ID
                dp.KhachHang = newKh;
            }
            dp.ngay_dat = dtNgayDat.Value.Date;
            dp.ngay_check_in = dtNgayCheckIn.Value.Date;
            dp.ngay_check_out = dtNgayCheckOut.Value.Date;
            dp.nhan_vien_id = InfoNhanVien.CurrentUser.nhan_vien_id;
            dp.trang_thai = "da_dat";

            try
            {
                db.SubmitChanges();
                MessageBox.Show("Lưu lại thông tin phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnChuyenPhong_Click(object sender, EventArgs e)
        {
            var chuyenForm = new ChonPhongChuyen(_DatPhongId);
            if (chuyenForm.ShowDialog() == DialogResult.OK)
            {
                // lấy về phòng mới
                int newPhongId = chuyenForm.SelectPhongId;

                MessageBox.Show($"Chuyển thành công sang phòng mới: {newPhongId}", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Load lại chi tiết với phòng mới
                LoadChiTiet(newPhongId);
            }
        }

        private void dtNgayCheckOut_ValueChanged(object sender, EventArgs e)
        {
            UpdateThoiHan();
        }

        private void dtNgayCheckIn_ValueChanged(object sender, EventArgs e)
        {
            UpdateThoiHan();
        }

        private void LoadChiTiet()
        {
            _Booking = db.DatPhongs.SingleOrDefault(d => d.phong_id == _maPhong);
            _DatPhongId = _Booking.dat_phong_id;

            if (_Booking == null)
            {
                MessageBox.Show(
                    $"Không tìm thấy đặt phòng ID = {_maPhong}",
                    "Lỗi dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
                return;
            }

            _Room = db.Phongs.SingleOrDefault(p => p.phong_id == _Booking.phong_id);
            if (_Room == null)
            {
                MessageBox.Show(
                    $"Không tìm thấy phòng ID = {_Booking.phong_id}",
                    "Lỗi dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
                return;
            }

            //Thông tin phòng
            lbSoPhong.Text = _Room.so_phong;
            txtSoPhong.Text = _Room.so_phong;
            txtLoaiPhong.Text = _Room.LoaiPhong.ten_loai;
            txtTrangThai.Text = TranslateStatus(_Room.trang_thai);
            txtMoTa.Text = _Room.LoaiPhong.mo_ta;
            txtGia.Text = _Room.LoaiPhong.gia_theo_dem.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));

            // Thông tin khách
            var kh = _Booking.KhachHang;
            txtKhachId.Text = kh.khach_hang_id.ToString();
            txtHoTen.Text = kh.ho_ten;
            txtDiaChi.Text = kh.dia_chi;
            txtSDT.Text = kh.so_dien_thoai;
            txtEmail.Text = kh.email;
            txtCCCD.Text = kh.cccd;

            // Thời gian
            dtNgayDat.Value = _Booking.ngay_dat;
            dtNgayCheckIn.Value = _Booking.ngay_check_in;
            dtNgayCheckOut.Value = _Booking.ngay_check_out;

        }
        private void LoadChiTiet(int newPhongId)
        {
            _Booking = db.DatPhongs.SingleOrDefault(d => d.phong_id == newPhongId);
            _DatPhongId = _Booking.dat_phong_id;
            _Room = db.Phongs.SingleOrDefault(p => p.phong_id == _Booking.phong_id);
            if (_Room == null)
            {
                MessageBox.Show(
                    $"Không tìm thấy phòng ID = {_Booking.phong_id}",
                    "Lỗi dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
                return;
            }

            //Thông tin phòng
            lbSoPhong.Text = _Room.so_phong;
            txtSoPhong.Text = _Room.so_phong;
            txtLoaiPhong.Text = _Room.LoaiPhong.ten_loai;
            txtTrangThai.Text = TranslateStatus(_Room.trang_thai);
            txtMoTa.Text = _Room.LoaiPhong.mo_ta;
            txtGia.Text = _Room.LoaiPhong.gia_theo_dem.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));

            // Thông tin khách
            var kh = _Booking.KhachHang;
            txtKhachId.Text = kh.khach_hang_id.ToString();
            txtHoTen.Text = kh.ho_ten;
            txtDiaChi.Text = kh.dia_chi;
            txtSDT.Text = kh.so_dien_thoai;
            txtEmail.Text = kh.email;
            txtCCCD.Text = kh.cccd;

            // Thời gian
            dtNgayDat.Value = _Booking.ngay_dat;
            dtNgayCheckIn.Value = _Booking.ngay_check_in;
            dtNgayCheckOut.Value = _Booking.ngay_check_out;

        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            // Nếu không phải chữ số thì hủy
            if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtCCCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            // Nếu không phải chữ số thì hủy
            if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void cbSuaThongTinKH_CheckedChanged(object sender, EventArgs e)
        {
            if(cbSuaThongTinKH.Checked)
            {
                txtHoTen.Enabled = true;
                txtDiaChi.Enabled = true;
                txtSDT.Enabled = true;
                txtEmail.Enabled = true;
                txtCCCD.Enabled = true;
                btnChonKhach.Enabled = true;
            }
            else
            {
                txtHoTen.Enabled = false;
                txtDiaChi.Enabled = false;
                txtSDT.Enabled = false;
                txtEmail.Enabled = false;
                txtCCCD.Enabled = false;
                btnChonKhach.Enabled = false;
            }
        }

        private void btnNhanPhong_Click(object sender, EventArgs e)
        {
            var dp = db.DatPhongs.SingleOrDefault(d => d.dat_phong_id == _DatPhongId);
            if (dp == null)
            {
                MessageBox.Show("Không tìm thấy thông tin đặt phòng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xác nhận hành động
            var dr = MessageBox.Show(
                $"Bạn có chắc chắn muốn nhận phòng cho đặt phòng #{dp.dat_phong_id}?\n" +
                $"Khách: {dp.KhachHang.ho_ten}\nPhòng: {dp.Phong.so_phong}",
                "Xác nhận nhận phòng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            var phong = db.Phongs.SingleOrDefault(p => p.phong_id == dp.phong_id);
            if (phong != null)
            {
                phong.trang_thai = "dang_su_dung";
            }

            // 5. Lưu thay đổi
            try
            {
                db.SubmitChanges();
                MessageBox.Show("Nhận phòng thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nhận phòng:\n" + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuyDatTruoc_Click(object sender, EventArgs e)
        {
            int datPhongId = _DatPhongId;
            if (datPhongId <= 0)
            {
                MessageBox.Show("Không có đặt phòng để hủy.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var dp = db.DatPhongs.SingleOrDefault(d => d.dat_phong_id == datPhongId);
            if (dp == null)
            {
                MessageBox.Show("Không tìm thấy thông tin đặt phòng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xác nhận hủy
            var dr = MessageBox.Show($"Bạn có chắc muốn hủy đặt phòng #{dp.dat_phong_id} của khách {dp.KhachHang.ho_ten} không?",
                "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;
            db.DatPhongs.DeleteOnSubmit(dp);

            // Cập nhật lại trạng thái phòng
            var phong = db.Phongs.SingleOrDefault(p => p.phong_id == dp.phong_id);
            if (phong != null)
            {
                phong.trang_thai = "trong";
            }
            try
            {
                db.SubmitChanges();
                MessageBox.Show("Hủy đặt phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy đặt phòng:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string TranslateStatus(string st)
        => st switch
        {
            "trong" => "Trống",
            "dang_su_dung" => "Đang sử dụng",
            "da_dat" => "Đã đặt",
            "bao_tri" => "Bảo trì",
            _ => st
        };

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (phatHienThayDoi())
            {
                var dr = MessageBox.Show("Dữ liệu đã thay đổi.\n\n - Nhấn Yes để tiếp tục thoát và bỏ thay đổi.\n - Nhấn No để ở lại và lưu lại.",
                    "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.No)
                    return;   // hủy thoát
            }

            this.Close();
        }
        private int demLoad = 0;
        private void UpdateThoiHan()
        {
            demLoad++;
            int days = (dtNgayCheckOut.Value.Date - dtNgayCheckIn.Value.Date).Days;

            // Nếu âm gán 0
            if (days < 0 && demLoad > 1)
            {
                txtThoiHan.Text = "0 Ngày";
                MessageBox.Show("Ngày check-out phải ≥ ngày check-in!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtThoiHan.Text = days + " Ngày";
        }
        private bool phatHienThayDoi()
        {
            if (_origHoTen != txtHoTen.Text) return true;
            if (_origDiaChi != txtDiaChi.Text) return true;
            if (_origSDT != txtSDT.Text) return true;
            if (_origEmail != txtEmail.Text) return true;
            if (_origCCCD != txtCCCD.Text) return true;
            if (_origNgayDat != dtNgayDat.Value.Date) return true;
            if (_origCheckIn != dtNgayCheckIn.Value.Date) return true;
            if (_origCheckOut != dtNgayCheckOut.Value.Date) return true;

            return false;
        }
    }
}
