using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Color = System.Drawing.Color;

namespace QuanLyKhachSan
{
    public partial class ChiTietPhongTrong : Form
    {
        private bool checkDichVu = false;
        private string trangThai;
        private int ID;
        private decimal _giaTheoDem, _giaDichVu;
        private QLKSDataContext db = new QLKSDataContext();
        public ChiTietPhongTrong(int ID, string trangThai)
        {
            InitializeComponent();
            this.ID = ID;
            this.trangThai = trangThai;
            txtKhachId.Hide();
            txtKhuyenMaiID.Hide();
            dtCheckOut.Value = dtCheckIn.Value.AddDays(1);

            UpdateSoTien();
            lbTienDichVu.Text = "Tiền dịch vụ: 0";
            lbTienThuePhong.Text = "Tiền thuê phòng: 0";
            lbSoTienCanThanhToan.Text = "0";
            int pct = LayMaxGiaTriPhamTramKhuyenMai();
            lbPhanTramKM.Text = pct > 0 ? pct + "%" : " → giảm 0%";
            if(layMaKhuyenMai() > 0) txtKhuyenMaiID.Text = layMaKhuyenMai().ToString();
        }
        private int? layMaKhuyenMai()
        {
            DateTime today = DateTime.Today;
            using (var db = new QLKSDataContext())
            {
                var top = db.KhuyenMais
                            .Where(km => km.ngay_bat_dau <= today
                                      && km.ngay_ket_thuc >= today)
                            .OrderByDescending(km => km.phan_tram)
                            .Select(km => km.khuyen_mai_id)
                            .FirstOrDefault();
                // FirstOrDefault trả về 0 nếu không tìm thấy
                return top == 0 ? (int?)null : top;
            }
        }
        private int LayMaxGiaTriPhamTramKhuyenMai()
        {
            DateTime today = DateTime.Today;

            using (var db = new QLKSDataContext())
            {
                var active = db.KhuyenMais
                               .Where(km =>
                                   km.ngay_bat_dau <= today &&
                                   km.ngay_ket_thuc >= today
                               );

                if (!active.Any())
                    return 0;

                int maxKM = active.Max(km => km.phan_tram);
                return maxKM;
            }
        }
        private string LayTenKhuyenMai()
        {
            DateTime today = DateTime.Today;

            using (var db = new QLKSDataContext())
            {
                var names = db.KhuyenMais
                              .Where(km => km.ngay_bat_dau <= today
                                        && km.ngay_ket_thuc >= today)
                              .Select(km => km.ten_khuyen_mai)
                              .ToList();

                if (!names.Any())
                    return "Hôm nay không có khuyến mãi";

                // Nối thành chuỗi
                return string.Join(", ", names);
            }
        }
        private void dgvDichVu_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string moTa = dgvDichVu
                .Rows[e.RowIndex]
                .Cells["mo_ta"]
                .Value?.ToString() ?? "";

            foreach (DataGridViewCell cell in dgvDichVu.Rows[e.RowIndex].Cells)
            {
                cell.ToolTipText = moTa;
            }
        }
        private void LoadDichVu()
        {
            var ds = db.DichVus
                       .Select(dv => new
                       {
                           dv.dich_vu_id,
                           dv.ten_dich_vu,
                           dv.mo_ta,
                           dv.gia
                       })
                       .ToList();

            dgvDichVu.DataSource = ds;

            //Ẩn cột ID và Mota
            if (dgvDichVu.Columns.Contains("dich_vu_id"))
                dgvDichVu.Columns["dich_vu_id"].Visible = false;
            if (dgvDichVu.Columns.Contains("mo_ta"))
                dgvDichVu.Columns["mo_ta"].Visible = false;

            if (dgvDichVu.Columns.Contains("ten_dich_vu"))
                dgvDichVu.Columns["ten_dich_vu"].HeaderText = "Tên dịch vụ";

            if (dgvDichVu.Columns.Contains("gia"))
            {
                var c = dgvDichVu.Columns["gia"];
                c.HeaderText = "Đơn giá";
                c.DefaultCellStyle.Format = "N0";// định dạng tièn
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dgvDichVu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvDichVu.Columns.Contains("Tên dịch vụ"))
                dgvDichVu.Columns["ten_dich_vu"].FillWeight = 100;
            if (dgvDichVu.Columns.Contains("don_gia"))
                dgvDichVu.Columns["don_gia"].FillWeight = 40;

            dgvDichVu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDichVu.MultiSelect = false;
            dgvDichVu.AllowUserToAddRows = false;
            dgvDichVu.AllowUserToDeleteRows = false;
            dgvDichVu.ReadOnly = true;
            dgvDichVu.ShowCellToolTips = true;

            if (!checkDichVu) ThemDichVu();
        }
        private void ThemDichVu()
        {
            DataGridViewButtonColumn btnAdd = new DataGridViewButtonColumn();
            btnAdd.Name = "Thao tác";
            btnAdd.Text = "✛";
            btnAdd.UseColumnTextForButtonValue = true;
            dgvDichVu.Columns.Add(btnAdd);
        }

        private void ChiTietPhongTrong_Load(object sender, EventArgs e)
        {
            LoadThongTin(ID);
        }

        private void dgvDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var vi = new CultureInfo("vi-VN");
            if (dgvDichVu.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var src = dgvDichVu.Rows[e.RowIndex];
                string ten = src.Cells["ten_dich_vu"].Value.ToString();
                decimal gia = Convert.ToDecimal(src.Cells["gia"].Value);

                // Kiểm tra trùng
                var exist = dgvDatDichVu.Rows
                    .OfType<DataGridViewRow>()
                    .FirstOrDefault(r => r.Cells["ten_dich_vu"].Value?.ToString() == ten);

                if (exist != null)
                {
                    // tăng số lượng
                    int qty = Convert.ToInt32(exist.Cells["so_luong"].Value) + 1;
                    exist.Cells["so_luong"].Value = qty.ToString("N0", vi);
                    exist.Cells["gia"].Value = (gia * qty).ToString("N0", vi);
                    lbTienDichVu.Text = "Tiền dịch vụ: " + UpdateTongTienDichVu().ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                    _giaDichVu = UpdateTongTienDichVu();
                    UpdateSoTien();
                }
                else
                {
                    // Thêm row mới
                    int idx = dgvDatDichVu.Rows.Add();
                    var r2 = dgvDatDichVu.Rows[idx];
                    r2.Cells["dich_vu_id"].Value = src.Cells["dich_vu_id"].Value;
                    r2.Cells["ten_dich_vu"].Value = ten;
                    r2.Cells["so_luong"].Value = 1;
                    r2.Cells["gia"].Value = gia.ToString("N0", vi);
                    if (dgvDatDichVu.Columns["Delete"] == null)
                    {
                        DataGridViewButtonColumn btnDeleteColumn = new DataGridViewButtonColumn();
                        btnDeleteColumn.Name = "Delete";
                        btnDeleteColumn.HeaderText = "Thao tác";
                        btnDeleteColumn.Text = "Xóa";
                        btnDeleteColumn.UseColumnTextForButtonValue = true;
                        dgvDatDichVu.Columns.Add(btnDeleteColumn);
                        lbTienDichVu.Text = "Tiền dịch vụ: " + UpdateTongTienDichVu().ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                        _giaDichVu = UpdateTongTienDichVu();
                        UpdateSoTien();
                    }
                    lbTienDichVu.Text = "Tiền dịch vụ: " + UpdateTongTienDichVu().ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                    _giaDichVu = UpdateTongTienDichVu();
                    UpdateSoTien();
                }
            }
        }
        private decimal UpdateTongTienDichVu()
        {
            decimal total = 0m;

            foreach (DataGridViewRow row in dgvDatDichVu.Rows)
            {
                // Bỏ qua hàng new-row
                if (row.IsNewRow)
                    continue;

                var cellValue = row.Cells["gia"].Value;
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
                    if (decimal.TryParse(s, NumberStyles.Number,
                                         CultureInfo.GetCultureInfo("vi-VN"),
                                         out decimal val))
                    {
                        total += val;
                    }
                }
            }
            return total;
        }
        private void dgvDatDichVu_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvDatDichVu.Columns[e.ColumnIndex].Name == "Delete" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = dgvDatDichVu.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.ForeColor = Color.Red;
                cell.Style.Font = new Font("Arial", 10, FontStyle.Bold);
                lbTienDichVu.Text = "Tiền dịch vụ: " + UpdateTongTienDichVu().ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                _giaDichVu = UpdateTongTienDichVu();
                UpdateSoTien();
            }
        }

        private void dgvDatDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvDatDichVu.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                dgvDatDichVu.Rows.RemoveAt(e.RowIndex);
                lbTienDichVu.Text = "Tiền dịch vụ: " + UpdateTongTienDichVu().ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                _giaDichVu = UpdateTongTienDichVu();
                UpdateSoTien();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDatTruoc.Checked)
            {
                btnDatPhong.Text = "Đặt Trước Ngay";
                btnDatPhong.BackColor = Color.FromArgb(0, 192, 192);
            }
            else
            {
                btnDatPhong.Text = "Đặt Phòng Ngay";
                btnDatPhong.BackColor = Color.FromArgb(0, 192, 0);
            }
        }

        private void cbDichVu_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDichVu.Checked)
            {
                LoadDichVu();
                dgvDatDichVu.Enabled = true;
                checkDichVu = true;
                if (dgvDatDichVu.Columns.Count == 0)
                {
                    dgvDatDichVu.Columns.Add("dich_vu_id", "Mã DV");
                    dgvDatDichVu.Columns.Add("ten_dich_vu", "Tên dịch vụ");
                    dgvDatDichVu.Columns.Add("so_luong", "Số lượng");
                    dgvDatDichVu.Columns.Add("gia", "Giá");
                    if (dgvDatDichVu.Columns.Contains("dich_vu_id"))
                        dgvDatDichVu.Columns["dich_vu_id"].Visible = false;
                    var btnDelete = new DataGridViewButtonColumn
                    {
                        Name = "Delete",
                        HeaderText = "Thao tác",
                        Text = "Xóa",
                        UseColumnTextForButtonValue = true,
                        Width = 60
                    };
                    dgvDatDichVu.Columns.Add(btnDelete);
                }
            }
            else
            {
                dgvDichVu.DataSource = null;
                dgvDichVu.Columns.Clear();
                dgvDatDichVu.Enabled = false;
                checkDichVu = false;
                dgvDatDichVu.Columns.Clear();
                lbTienDichVu.Text = "Tiền dịch vụ: 0";
                _giaDichVu = 0;
            }
        }
        private void LoadThongTin(int phongId)
        {
            using (var db = new QLKSDataContext())
            {
                var room = (from p in db.Phongs
                            join lp in db.LoaiPhongs on p.loai_phong_id equals lp.loai_phong_id
                            where p.phong_id == phongId
                            select new
                            {
                                p.phong_id,
                                p.so_phong,
                                p.loai_phong_id,
                                TenLoai = lp.ten_loai,
                                MoTa = lp.mo_ta,
                                Gia = lp.gia_theo_dem,
                                p.trang_thai
                            }).SingleOrDefault();
                if (room == null) return;
                var val = room.Gia;

                _giaTheoDem = val;

                if (val != null && decimal.TryParse(val.ToString(), out var gia))
                {
                    var vi = new CultureInfo("vi-VN");
                    txtGia.Text = gia.ToString("N0", vi);
                }
                else txtGia.Text = "";
                lbSoPhong.Text = room.so_phong;
                txtSoPhong.Text = room.so_phong;
                txtLoaiPhong.Text = room.TenLoai;
                txtTrangThai.Text = TranslateStatus(trangThai);
                txtMoTa.Text = room.MoTa;

                txtHoTen.Clear();
                txtDiaChi.Clear();
                txtSDT.Clear();
                txtEmail.Clear();
                txtCCCD.Clear();

                dtNgayDat.Value = DateTime.Today;
                dtCheckIn.Value = DateTime.Today;
                dtCheckOut.Value = DateTime.Today;
                cbDatTruoc.Checked = false;
            }
        }
        private string TranslateStatus(string status)
        {
            switch (status)
            {
                case "trong": return "Trống";
                case "dang_su_dung": return "Đang sử dụng";
                case "bao_tri": return "Bảo trì";
                case "da_dat": return "Đã đặt";
                default: return status;
            }
        }

        private void btnChonKhach_Click(object sender, EventArgs e)
        {
            using (var f = new ChonKhachHang())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    int khId = f.SelectedCustomerId;
                    txtKhachId.Text = khId.ToString();    // lưu lại nếu cần
                    LoadKhachHang(khId);
                }
            }
        }
        private void LoadKhachHang(int khachHangId)
        {
            using (var db = new QLKSDataContext())
            {
                var kh = db.KhachHangs
                           .SingleOrDefault(k => k.khach_hang_id == khachHangId);
                if (kh == null) return; // hoặc clear form nếu không tìm thấy

                txtHoTen.Text = kh.ho_ten;
                txtDiaChi.Text = kh.dia_chi;
                txtSDT.Text = kh.so_dien_thoai;
                txtEmail.Text = kh.email;
                txtCCCD.Text = kh.cccd;
            }
        }
        private bool KiemTraDate()
        {
            DateTime today = DateTime.Today;
            DateTime ngayDat = dtNgayDat.Value.Date;
            DateTime ngayCheckIn = dtCheckIn.Value.Date;
            DateTime ngayCheckOut = dtCheckOut.Value.Date;
            bool datTruoc = cbDatTruoc.Checked;

            if (!datTruoc)
            {
                // Nếu không đặt trước thì ngày đặt phải là hôm nay
                if (ngayDat != today)
                {
                    MessageBox.Show("Nếu không đặt trước, Ngày đặt phải là hôm nay.",
                                    "Lỗi ngày đặt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtNgayDat.Focus();
                    return false;
                }
            }
            else
            {
                // Nếu đặt trước thì ngày đặt phải > hôm nay
                if (ngayDat <= today)
                {
                    MessageBox.Show("Nếu đánh dấu Đặt trước, Ngày đặt phải lớn hơn hôm nay.",
                                    "Lỗi ngày đặt trước", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtNgayDat.Focus();
                    return false;
                }
            }

            // Ngày check-in ≥ ngày đặt
            if (ngayCheckIn < ngayDat)
            {
                MessageBox.Show("Ngày check-in phải bằng hoặc sau Ngày đặt.",
                                "Lỗi ngày check-in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtCheckIn.Focus();
                return false;
            }

            // Ngày check-out > ngày check-in
            if (ngayCheckOut <= ngayCheckIn)
            {
                MessageBox.Show("Ngày check-out phải sau Ngày check-in.",
                                "Lỗi ngày check-out", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtCheckOut.Focus();
                return false;
            }

            return true;
        }
        private bool DieuKienKhachHang()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Bạn phải nhập Họ và tên.", "Lỗi bắt buộc",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            // ko đc bỏ trống địa chỉ
            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Bạn phải nhập Địa chỉ.", "Lỗi bắt buộc",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return false;
            }

            // định dạng sdt
            var phone = txtSDT.Text.Trim();
            if (!Regex.IsMatch(phone, @"^\d{9,11}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ (9–11 chữ số).", "Lỗi định dạng",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }

            // định dạng email
            var email = txtEmail.Text.Trim();
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không đúng định dạng.", "Lỗi định dạng",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // định dạng cccd
            var cccd = txtCCCD.Text.Trim();
            if (!Regex.IsMatch(cccd, @"^\d{9}(\d{3})?$"))
            {
                MessageBox.Show("CMT/CCCD phải là 9 hoặc 12 chữ số.", "Lỗi định dạng",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCCCD.Focus();
                return false;
            }

            return true;
        }

        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            if (DieuKienKhachHang())
            {
                DateTime ngayDat = dtNgayDat.Value.Date;
                DateTime ngayNhanPhong = dtCheckIn.Value.Date;
                DateTime ngayTraPhong = dtCheckOut.Value.Date;
                bool datTruoc = cbDatTruoc.Checked;
                var infoNhanVien = InfoNhanVien.CurrentUser;

                if (ngayNhanPhong < ngayDat)
                {
                    MessageBox.Show("Ngày nhận phòng phải ≥ ngày đặt.", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (ngayTraPhong <= ngayNhanPhong)
                {
                    MessageBox.Show("Ngày trả phải lớn hơn ngày nhận.", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var db = new QLKSDataContext())
                {
                    string trangThaiPhong = "";
                    if (datTruoc) trangThaiPhong = "dat_truoc";
                    else trangThaiPhong = "dang_su_dung";
                    // Thêm DatPhong
                    var dp = new DatPhong();
                    if(txtKhachId.Text.Length > 0)
                    {
                        dp = new DatPhong
                        {
                            khach_hang_id = Convert.ToInt32(txtKhachId.Text),
                            phong_id = ID,
                            nhan_vien_id = infoNhanVien.nhan_vien_id,
                            khuyen_mai_id = Convert.ToInt32(txtKhuyenMaiID.Text),
                            ngay_dat = ngayDat,
                            ngay_check_in = ngayNhanPhong,
                            ngay_check_out = ngayTraPhong,
                            trang_thai = trangThaiPhong
                        };
                    } else
                    {
                        dp = new DatPhong
                        {
                            phong_id = ID,
                            nhan_vien_id = infoNhanVien.nhan_vien_id,
                            khuyen_mai_id = Convert.ToInt32(txtKhuyenMaiID.Text),
                            ngay_dat = ngayDat,
                            ngay_check_in = ngayNhanPhong,
                            ngay_check_out = ngayTraPhong,
                            trang_thai = trangThaiPhong
                        };
                    }
                    db.DatPhongs.InsertOnSubmit(dp);
                    db.SubmitChanges();

                    // Thêm DichVuDatPhong cho từng dịch vụ
                    foreach (DataGridViewRow row in dgvDatDichVu.Rows)
                    {
                        if (row.IsNewRow) continue;

                        int dvId = Convert.ToInt32(row.Cells["dich_vu_id"].Value);
                        int soLuong = Convert.ToInt32(row.Cells["so_luong"].Value);

                        var dvdp = new DichVuDatPhong
                        {
                            dat_phong_id = dp.dat_phong_id,
                            dich_vu_id = dvId,
                            so_luong = soLuong,
                            ngay_su_dung = ngayNhanPhong 
                        };
                        db.DichVuDatPhongs.InsertOnSubmit(dvdp);
                    }

                    var phong = db.Phongs.Single(p => p.phong_id == ID);
                    phong.trang_thai = datTruoc ? "da_dat" : "dang_su_dung";

                    db.SubmitChanges();
                }

                MessageBox.Show("Đặt phòng thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void dtCheckOut_ValueChanged(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime ngayDat = dtNgayDat.Value.Date;
            DateTime ngayCheckIn = dtCheckIn.Value.Date;
            DateTime ngayCheckOut = dtCheckOut.Value.Date;
            int soNgay = (ngayCheckOut - ngayCheckIn).Days;
            if (soNgay == 0) soNgay = 1;
            if (soNgay > 0)
            {
                UpdateSoTien();
            }
            else
            {
                txtThoiHan.Text = "Vui lòng chọn ngày Check in < ngày Check out";
            }
        }
        private void UpdateSoTien()
        {
            DateTime ngayCheckIn = dtCheckIn.Value.Date;
            DateTime ngayCheckOut = dtCheckOut.Value.Date;

            int nights = (ngayCheckOut - ngayCheckIn).Days;
            if (nights < 1) nights = 1;
            txtThoiHan.Text = nights + " Ngày";
            // Tính tiền
            UpdateTongTienDichVu();
            decimal tongTienThue = _giaTheoDem * nights;
            decimal giamGia = (tongTienThue + _giaDichVu) * LayMaxGiaTriPhamTramKhuyenMai() / 100m;
            decimal finalTotal = (tongTienThue + _giaDichVu) - giamGia;

            var vi = new CultureInfo("vi-VN");
            lbTienThuePhong.Text = "Tiền thuê phòng: " + tongTienThue.ToString("N0", vi);
            lbMoTaKM.Text = LayTenKhuyenMai();
            lbPhanTramKM.Text = $"{LayMaxGiaTriPhamTramKhuyenMai()}% → giảm {giamGia.ToString("N0", vi)}";
            lbSoTienCanThanhToan.Text = finalTotal.ToString("N0", vi);
        }

        private void dtCheckIn_ValueChanged(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime ngayDat = dtNgayDat.Value.Date;
            DateTime ngayCheckIn = dtCheckIn.Value.Date;
            DateTime ngayCheckOut = dtCheckOut.Value.Date;
            if (ngayCheckIn > ngayDat)
            {
                cbDatTruoc.Checked = true;
            }
            else
            {
                cbDatTruoc.Checked = false;
            }
            UpdateSoTien();
        }
    }
}
