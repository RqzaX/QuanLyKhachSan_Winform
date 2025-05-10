using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace QuanLyKhachSan
{
    public partial class ChiTietPhongSuDung : Form
    {
        QLKSDataContext db = new QLKSDataContext();
        private bool checkDichVu = false;
        private int soLanLoadCheckOut = 0;
        private readonly int _datPhongId;
        private DatPhong _Booking;
        private Phong _Room;
        private int _currentDatPhongId; // đã được gán khi load chi tiết
        private int _currentPhongId;// id phòng hiện tại
        private BindingList<DatDichVuDTO> _dichVuBinding;

        public ChiTietPhongSuDung(int datPhongId)
        {
            InitializeComponent();
            _datPhongId = datPhongId;
            dgvDichVu.RowTemplate.Height = 30;
            dgvDatDichVu.RowTemplate.Height = 30;
            txtKhachId.Hide();
            txtKhuyenMaiID.Hide();
        }
        private void ChiTietPhongSuDung_Load(object sender, EventArgs e)
        {
            LoadChiTiet();
        }
        private void LoadChiTiet()
        {
            //Load DatPhong cùng Phong & KhachHang & KhuyenMai
            _Booking = db.DatPhongs.SingleOrDefault(d => d.phong_id == _datPhongId);

            if (_Booking == null)
            {
                MessageBox.Show(
                    $"Không tìm thấy đặt phòng ID = {_datPhongId}",
                    "Lỗi dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
                return;
            }
            _currentDatPhongId = _Booking.dat_phong_id;
            _currentPhongId = _Booking.phong_id;
            _Room = db.Phongs
                .SingleOrDefault(p => p.phong_id == _Booking.phong_id);
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

            // Dịch vụ hiện có 
            dgvDichVu.DataSource = db.DichVus.Select(x => new
            {
                x.dich_vu_id,
                x.ten_dich_vu,
                Gia = x.gia
            }).ToList();
            dgvDichVu.Columns["dich_vu_id"].Visible = false;
            dgvDichVu.Columns["ten_dich_vu"].HeaderText = "Tên dịch vụ";
            dgvDichVu.Columns["Gia"].HeaderText = "Giá";
            dgvDichVu.Columns["Gia"].DefaultCellStyle.Format = "N0";
            if (!checkDichVu) ThemDichVu();
            // Dịch vụ khách đã đặt
            var initial = _Booking.DichVuDatPhongs
            .Select(x => new DatDichVuDTO
            {
                DichVuId = x.dich_vu_id,
                TenDichVu = x.DichVu.ten_dich_vu,
                SoLuong = Convert.ToInt32(x.so_luong),
                ThanhTien = Convert.ToDecimal(x.so_luong * x.DichVu.gia)
            }).ToList();

            _dichVuBinding = new BindingList<DatDichVuDTO>(initial);
            dgvDatDichVu.DataSource = _dichVuBinding;

            // thiết lập header, format …
            dgvDatDichVu.Columns["DichVuId"].Visible = false;
            dgvDatDichVu.Columns["TenDichVu"].HeaderText = "Tên dịch vụ";
            dgvDatDichVu.Columns["SoLuong"].HeaderText = "Số lượng";
            dgvDatDichVu.Columns["ThanhTien"].HeaderText = "Giá";
            dgvDatDichVu.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";

            // thêm cột Xóa
            ThemCotXoaDV();
            UpdateSoNgayVaKM();

        }
        private void UpdateSoNgayVaKM()
        {
            // Tính số ngày
            int day = (dtNgayCheckOut.Value.Date - dtNgayCheckIn.Value.Date).Days;
            if (day < 1) day = 1;
            txtThoiHan.Text = $"{day} Ngày";

            // Cập nhật khuyến mãi có % cao nhất
            DateTime today = dtNgayCheckIn.Value.Date;
            var top = db.KhuyenMais.Where(k => k.ngay_bat_dau <= today
                                     && k.ngay_ket_thuc >= today)
                            .OrderByDescending(k => k.phan_tram)
                            .FirstOrDefault();
            // Tổng tiền thuê phòng (ngày * giá 1 đêm)
            decimal giaNgay = _Room.LoaiPhong.gia_theo_dem;
            decimal tongTienThue = giaNgay * day;
            decimal tienDV = UpdateTongTienDichVu();
            // Tính khuyến mãi nếu có
            if (top != null)
            {
                decimal giamGia = (tongTienThue + tienDV) * top.phan_tram / 100m;
                lbPhanTramKM.Text = top.phan_tram + "% → giảm " + giamGia.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                lbTongTienThanhToan.Text = "Tổng tiền thanh toán: " + ((tongTienThue + tienDV) - giamGia).ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
            }
            else
            {
                lbPhanTramKM.Text = "0%";
                lbTongTienThanhToan.Text = "Tổng tiền thanh toán: " + (tongTienThue + tienDV).ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
            }
            lbTienThuePhong.Text = "Tiền thuê phòng: " + tongTienThue.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
            lbTienDichVu.Text = "Tiền dịch vụ: " + tienDV.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
        }
        private decimal UpdateTongTienDichVu()
        {
            decimal total = 0m;

            foreach (DataGridViewRow row in dgvDatDichVu.Rows)
            {
                // Bỏ qua hàng new-row
                if (row.IsNewRow)
                    continue;

                var cellValue = row.Cells["ThanhTien"].Value;
                if (cellValue == null)
                    continue;

                // Nếu Value đã là decimal
                if (cellValue is decimal d)
                {
                    total += d;
                }
                else
                {
                    // Nếu Value là string -> vi
                    var s = cellValue.ToString();
                    if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.GetCultureInfo("vi-VN"), out decimal val))
                    {
                        total += val;
                    }
                }
            }
            return total;
        }
        private void ThemDichVu()
        {
            if (dgvDichVu.Columns.Contains("Thao tác")) return;
            DataGridViewButtonColumn btnAdd = new DataGridViewButtonColumn();
            btnAdd.Name = "Plus";
            btnAdd.Text = "Thêm";
            btnAdd.UseColumnTextForButtonValue = true;
            dgvDichVu.Columns.Add(btnAdd);
        }
        private void ThemCotXoaDV()
        {
            if (!dgvDatDichVu.Columns.Contains("Delete"))
            {
                var btn = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    HeaderText = "Thao tác",
                    Text = "Xóa",
                    UseColumnTextForButtonValue = true,
                    Width = 60
                };
                dgvDatDichVu.Columns.Add(btn);
            }
        }
        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbGiaHan.Checked) dtNgayCheckOut.Enabled = true;
            else dtNgayCheckOut.Enabled = false;
        }

        private void btnChuyenPhong_Click(object sender, EventArgs e)
        {
            var chuyenForm = new ChonPhongChuyen(_currentDatPhongId);
            if (chuyenForm.ShowDialog() == DialogResult.OK)
            {
                // lấy về phòng mới
                int newPhongId = chuyenForm.SelectPhongId;

                MessageBox.Show($"Chuyển thành công sang phòng mới: {newPhongId}", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Load lại chi tiết với phòng mới
                LoadChiTietPhongSuDung(newPhongId);

                //LoadPhongGrid();
            }
        }
        private void LoadChiTietPhongSuDung(int phongId)
        {
            _Booking = db.DatPhongs.SingleOrDefault(d => d.phong_id == phongId);

            if (_Booking == null)
            {
                MessageBox.Show(
                    $"Không tìm thấy đặt phòng ID = {phongId}",
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
            UpdateSoNgayVaKM();

            // Dịch vụ hiện có 
            dgvDichVu.DataSource = db.DichVus.Select(x => new
            {
                x.dich_vu_id,
                x.ten_dich_vu,
                Gia = x.gia
            }).ToList();
            dgvDichVu.Columns["dich_vu_id"].Visible = false;
            dgvDichVu.Columns["ten_dich_vu"].HeaderText = "Tên dịch vụ";
            dgvDichVu.Columns["Gia"].HeaderText = "Giá";
            dgvDichVu.Columns["Gia"].DefaultCellStyle.Format = "N0";
            if (!checkDichVu) ThemDichVu();
            // Dịch vụ khách đã đặt
            dgvDatDichVu.DataSource = _Booking.DichVuDatPhongs.Select(x => new
            {
                x.dich_vu_id,
                ten_dich_vu = x.DichVu.ten_dich_vu,
                x.so_luong,
                ThanhTien = x.so_luong * x.DichVu.gia
            }).ToList();
            dgvDatDichVu.Columns["dich_vu_id"].Visible = false;
            dgvDatDichVu.Columns["ten_dich_vu"].HeaderText = "Tên dịch vụ";
            dgvDatDichVu.Columns["so_luong"].HeaderText = "Số lượng";
            dgvDatDichVu.Columns["ThanhTien"].HeaderText = "Giá";
            dgvDatDichVu.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            ThemCotXoaDV();

            // Khuyến mãi hiện có
            if (_Booking.khuyen_mai_id.HasValue)
            {
                var km = _Booking.KhuyenMai;
                //lbMoTaKM.Text = km.ten_khuyen_mai;
                lbPhanTramKM.Text = $"{km.phan_tram}%";
            }
            else
            {
                //lbMoTaKM.Text = "Không có KM";
                lbPhanTramKM.Text = "0%";
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

        private void dtNgayCheckOut_ValueChanged(object sender, EventArgs e)
        {
            soLanLoadCheckOut++;
            DateTime today = DateTime.Today;
            DateTime ngayDat = dtNgayDat.Value.Date;
            DateTime ngayCheckIn = dtNgayCheckIn.Value.Date;
            DateTime ngayCheckOut = dtNgayCheckOut.Value.Date;
            // FirstOrDefault để an toàn nếu không tìm thấy
            DateTime? ngayTra = db.DatPhongs.Where(dp => dp.phong_id == _datPhongId)
            .Select(dp => (DateTime?)dp.ngay_check_out)
            .FirstOrDefault();
            if (soLanLoadCheckOut >= 2 && dtNgayCheckOut.Value.Date <= ngayTra)
            {
                MessageBox.Show("Ngày gia hạn Check out không được < ngày Check out hiện tại","Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Tạm bỏ đăng ký handler trước khi reset
                dtNgayCheckOut.ValueChanged -= dtNgayCheckOut_ValueChanged;
                dtNgayCheckOut.Value = ngayTra.Value;
                dtNgayCheckOut.ValueChanged += dtNgayCheckOut_ValueChanged;
                return;
            }
            int soNgay = (ngayCheckOut - ngayCheckIn).Days;
            if (soNgay == 0) soNgay = 1;
            if (soNgay > 0)
            {
                UpdateSoNgayVaKM();
            }
            else
            {
                //txtThoiHan.Text = "Vui lòng chọn ngày Check in < ngày Check out";
            }
        }

        private void dgvDatDichVu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvDatDichVu.Columns[e.ColumnIndex].Name != "Delete") return;

            // xoá theo index
            _dichVuBinding.RemoveAt(e.RowIndex);

            // cập nhật lại tổng tiền
            UpdateSoNgayVaKM();
        }

        private void dgvDichVu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvDichVu.Columns[e.ColumnIndex].Name != "Plus") return;

            // Lấy thông tin dịch vụ từ hàng source
            int dvId = (int)dgvDichVu.Rows[e.RowIndex].Cells["dich_vu_id"].Value;
            string ten = dgvDichVu.Rows[e.RowIndex].Cells["ten_dich_vu"].Value.ToString();
            decimal gia = Convert.ToDecimal(dgvDichVu.Rows[e.RowIndex].Cells["Gia"].Value);

            // Kiểm tra có đã tồn tại trong danh sách chưa?
            var exist = _dichVuBinding.FirstOrDefault(x => x.DichVuId == dvId);
            if (exist != null)
            {
                // nếu đã có thì tăng số lượng & tính lại giá
                exist.SoLuong++;
                exist.ThanhTien = exist.SoLuong * gia;
                // thông báo BindingList update cell
                dgvDatDichVu.Refresh();
            }
            else
            {
                // thêm mới
                _dichVuBinding.Add(new DatDichVuDTO
                {
                    DichVuId = dvId,
                    TenDichVu = ten,
                    SoLuong = 1,
                    ThanhTien = gia
                });
            }

            UpdateSoNgayVaKM();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                var booking = db.DatPhongs.SingleOrDefault(d => d.dat_phong_id == _currentDatPhongId);
                if (booking == null)
                {
                    MessageBox.Show("Không tìm thấy booking để cập nhật", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (cbGiaHan.Checked) booking.ngay_check_out = dtNgayCheckOut.Value;
                // Lấy ds dịch vụ hiện tại
                var dsDichVu = booking.DichVuDatPhongs.ToList();
                // Xóa dịch vụ đã bị xóa trên dgv
                foreach (var dv in dsDichVu)
                {
                    if (!_dichVuBinding.Any(x => x.DichVuId == dv.dich_vu_id))
                    {
                        db.DichVuDatPhongs.DeleteOnSubmit(dv);
                    }
                }
                // nếu đã tồn tại thì chỉ update số lượng
                // nếu chưa tồn tại thì thêm mới
                foreach (var dto in _dichVuBinding)
                {
                    var found = dsDichVu.FirstOrDefault(x => x.dich_vu_id == dto.DichVuId);

                    if (found != null)
                    {
                        // update số lượng
                        found.so_luong = dto.SoLuong;
                    }
                    else
                    {
                        // thêm mới
                        var newLink = new DichVuDatPhong
                        {
                            dat_phong_id = booking.dat_phong_id,
                            dich_vu_id = dto.DichVuId,
                            so_luong = dto.SoLuong
                        };
                        db.DichVuDatPhongs.InsertOnSubmit(newLink);
                    }
                }
                db.SubmitChanges();
                MessageBox.Show("Lưu thông tin phòng thành công!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Lỗi khi lưu lại dữ liệu:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            // Tính các dữ liệu cầnn thiết
            int days = (dtNgayCheckOut.Value.Date - dtNgayCheckIn.Value.Date).Days;
            if (days < 1) days = 1;
            // Tổng tiền thuê phòng
            decimal giaNgay = _Room.LoaiPhong.gia_theo_dem;
            decimal tongTienThue = giaNgay * days;
            // Tổng tiền dịch vụ
            decimal tienDV = UpdateTongTienDichVu();
            // Tính khuyến mãi nếu có
            int pct = db.KhuyenMais.Where(km => km.ngay_bat_dau <= today && km.ngay_ket_thuc >= today)
                                   .OrderByDescending(km => km.phan_tram)
                                   .Select(km => km.phan_tram).FirstOrDefault();
            //Tính giảm giá và tổng cuối cùng
            decimal rawTotal = (tongTienThue + tienDV);
            decimal giamGia = (rawTotal * pct) / 100m;
            decimal tongCuoiCung = rawTotal - giamGia;
            // Lấy phương thức thanh toán
            string phuongThuc = "";
            if (rdoTienMat.Checked) phuongThuc = "Tiền mặt";
            else if (rdoChuyenKhoan.Checked) phuongThuc = "Chuyển khoản";
            else if (rdoTheTinDung.Checked) phuongThuc = "Thẻ tín dụng";
            else
            {
                MessageBox.Show("Vui lòng chọn phương thức thanh toán!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Thực hiện ghi vào database
            try
            {
                // Tạo và lưu hóa đơn
                var hd = new HoaDon
                {
                    dat_phong_id = _currentDatPhongId,
                    nhan_vien_id = InfoNhanVien.CurrentUser.nhan_vien_id,
                    ten_nhan_vien = InfoNhanVien.CurrentUser.ho_ten,
                    ngay_tao = DateTime.Now,
                    tong_tien = tongCuoiCung
                };
                db.HoaDons.InsertOnSubmit(hd);
                db.SubmitChanges();
                // Tạo và lưu thanh toán
                var tt = new ThanhToan
                {
                    hoa_don_id = hd.hoa_don_id,
                    so_tien = tongCuoiCung,
                    phuong_thuc = phuongThuc,
                    ngay_thanh_toan = DateTime.Now,
                };
                db.ThanhToans.InsertOnSubmit(tt);
                // Cập nhật lại trạng thái phòng
                var phong = db.Phongs.SingleOrDefault(p => p.phong_id == _currentPhongId);
                if (phong != null)
                {
                    phong.trang_thai = "trong";
                }
                db.SubmitChanges();
                MessageBox.Show(
                "Thanh toán thành công!\n" +
                $"Tổng tiền phòng: {tongTienThue:N0} VNĐ\n" +
                $"Tổng tiền DV  : {tienDV:N0} VNĐ\n" +
                $"Giảm giá      : -{giamGia:N0} VNĐ\n" +
                $"=> Thanh toán : {tongCuoiCung:N0} VNĐ",
                "Thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi Thanh toán:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
