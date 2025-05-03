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
using Color = System.Drawing.Color;

namespace QuanLyKhachSan
{
    public partial class ChiTietPhongTrong : Form
    {
        private bool checkDichVu = false;
        private string trangThai;
        private int ID;
        private QLKSDataContext db = new QLKSDataContext();
        public ChiTietPhongTrong(int ID, string trangThai)
        {
            InitializeComponent();
            this.ID = ID;
            this.trangThai = trangThai;
            txtKhachId.Hide();
        }
        private void LoadDichVu()
        {
            var ds = db.DichVus
                       .Select(dv => new
                       {
                           dv.dich_vu_id,
                           dv.ten_dich_vu,
                           dv.gia
                       })
                       .ToList();

            dgvDichVu.DataSource = ds;

            //Ẩn cột ID
            if (dgvDichVu.Columns.Contains("dich_vu_id"))
                dgvDichVu.Columns["dich_vu_id"].Visible = false;
            if (dgvDichVu.Columns.Contains("ten_dich_vu"))
                dgvDichVu.Columns["ten_dich_vu"].HeaderText = "Tên dịch vụ";

            if (dgvDichVu.Columns.Contains("gia"))
            {
                var c = dgvDichVu.Columns["gia"];
                c.HeaderText = "Đơn giá";
                c.DefaultCellStyle.Format = "N0";            // định dạng tièn
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Auto-size
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
            if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvDichVu.Rows[e.RowIndex];
                string tenDichVu = selectedRow.Cells[1].Value.ToString();

                // Kiểm tra trùng lặp
                bool isDuplicate = false;
                foreach (DataGridViewRow row in dgvDatDichVu.Rows)
                {
                    if (row.Cells["TenDichVu"].Value != null &&
                        row.Cells["TenDichVu"].Value.ToString() == tenDichVu)
                    {
                        // Tăng số lượng nếu trùng
                        int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                        double gia = Convert.ToDouble(row.Cells["Gia"].Value);
                        row.Cells["SoLuong"].Value = soLuong + 1;
                        row.Cells["Gia"].Value = gia * soLuong;
                        isDuplicate = true;
                        break;
                    }
                }

                // Nếu không trùng
                if (!isDuplicate)
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(dgvDatDichVu);

                    newRow.Cells[0].Value = selectedRow.Cells[0].Value; // mã dịch vụ
                    newRow.Cells[1].Value = selectedRow.Cells[1].Value;
                    newRow.Cells[2].Value = selectedRow.Cells[2].Value;
                    newRow.Cells[4].Value = selectedRow.Cells[3].Value;
                    newRow.Cells[3].Value = 1;
                    dgvDatDichVu.Rows.Add(newRow);
                    if (dgvDatDichVu.Columns["Delete"] == null)
                    {
                        DataGridViewButtonColumn btnDeleteColumn = new DataGridViewButtonColumn();
                        btnDeleteColumn.Name = "Delete"; // Tên cột (dùng để kiểm tra)
                        btnDeleteColumn.HeaderText = "Thao tác";
                        btnDeleteColumn.Text = "Xóa";
                        btnDeleteColumn.UseColumnTextForButtonValue = true;
                        dgvDatDichVu.Columns.Add(btnDeleteColumn);
                    }
                }
            }
        }

        private void dgvDatDichVu_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvDatDichVu.Columns[e.ColumnIndex].Name == "Delete" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = dgvDatDichVu.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.ForeColor = Color.Red;
                cell.Style.Font = new Font("Arial", 10, FontStyle.Bold);
            }
        }

        private void dgvDatDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvDatDichVu.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                dgvDatDichVu.Rows.RemoveAt(e.RowIndex);
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
            }
            else
            {
                dgvDichVu.DataSource = null;
                dgvDichVu.Columns.Clear();
                dgvDatDichVu.Enabled = false;
                checkDichVu = false;
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
                if (val != null && decimal.TryParse(val.ToString(), out var gia))
                {
                    var vi = new CultureInfo("vi-VN");
                    txtGia.Text = gia.ToString("N0", vi);
                } else txtGia.Text = "";
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
            if (!DieuKienKhachHang())
            {
                return;
            } else if (!KiemTraDate()) return;
            int khachId = int.Parse(txtKhachId.Text);
            //int nvId = CurrentUser.Id;   // hoặc lấy từ biến/phiên
            //int? kmId = cbbKhuyenMai.SelectedIndex >= 0
            //                ? (int?)cbbKhuyenMai.SelectedValue
            //                : null;
            DateTime ngayDat = dtNgayDat.Value.Date;
            DateTime ngayCheckIn = dtCheckIn.Value.Date;
            DateTime ngayCheckOut = dtCheckOut.Value.Date;

            // 3. Tự động set dat_truoc: nếu check-in > hôm nay
            bool isDatTruoc = ngayCheckIn > DateTime.Today;
            cbDatTruoc.Checked = isDatTruoc;  // đồng bộ UI if bạn muốn

            using (var db = new QLKSDataContext())
            {
                // 4. Tạo bản ghi DatPhong mới
                var booking = new DatPhong
                {
                    khach_hang_id = khachId,
                    phong_id = ID,
                    //nhan_vien_id = nvId,
                    //khuyen_mai_id = kmId,
                    ngay_dat = ngayDat,
                    ngay_check_in = ngayCheckIn,
                    ngay_check_out = ngayCheckOut,
                    trang_thai = "dat_truoc"
                };
                db.DatPhongs.InsertOnSubmit(booking);

                // 5. Cập nhật lại trạng thái phòng
                var room = db.Phongs.Single(p => p.phong_id == ID);
                room.trang_thai = isDatTruoc
                    ? "da_dat"         // đặt trước
                    : "dang_su_dung";

                db.SubmitChanges();
            }

            MessageBox.Show("Đặt phòng thành công!", "OK",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 7. Refresh UI: load lại danh sách phòng, báo cáo… nếu có
            
        }

        private void dtCheckOut_ValueChanged(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime ngayDat = dtNgayDat.Value.Date;
            DateTime ngayCheckIn = dtCheckIn.Value.Date;
            DateTime ngayCheckOut = dtCheckOut.Value.Date;

            if(ngayCheckIn > ngayDat)
            {
                cbDatTruoc.Checked = true;
            } else
            {
                cbDatTruoc.Checked = false;
            }
        }
    }
}
