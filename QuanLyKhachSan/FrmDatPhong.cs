using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.ViewerObjectModel;

namespace QuanLyKhachSan
{
    public partial class FrmDatPhong : Form
    {
        QLKSDataContext db = new QLKSDataContext();
        public FrmDatPhong()
        {
            InitializeComponent();
            LoadComboLoaiPhong();
            ShowKhuyenMaiActive();
            LoadPhongGrid();
        }

        private void DatPhong_Load(object sender, EventArgs e)
        {

        }
        private void ShowKhuyenMaiActive()
        {
            DateTime today = DateTime.Today;
            using (var db = new QLKSDataContext())
            {
                // Lấy danh sách tên KM đang có hiệu lực hôm nay
                var names = db.KhuyenMais
                              .Where(km => km.ngay_bat_dau <= today
                                        && km.ngay_ket_thuc >= today)
                              .Select(km => km.ten_khuyen_mai)
                              .ToList();

                if (names.Count == 0)
                {
                    lbKhuyenMai.Text = "Hôm nay không có khuyến mãi (≧﹏≦)";
                }
                else
                {
                    string topName = db.KhuyenMais
                        .Where(km => km.ngay_bat_dau <= today && km.ngay_ket_thuc >= today)
                        .OrderByDescending(km => km.phan_tram)
                        .Select(km => km.ten_khuyen_mai)
                        .FirstOrDefault();
                    lbKhuyenMai.Text = "Khuyến mãi hôm nay: " + topName;
                }
            }
        }

        private void LoadComboLoaiPhong()
        {
            var ds = db.LoaiPhongs
                       .Select(lp => new
                       {
                           loai_phong_id = lp.loai_phong_id,
                           ten_loai = lp.ten_loai
                       })
                       .ToList();

            // Chèn "Tất cả" ở đầu
            ds.Insert(0, new
            {
                loai_phong_id = 0,
                ten_loai = "Tất cả"
            });

            cbbLoaiPhong.DataSource = ds;
            cbbLoaiPhong.ValueMember = "loai_phong_id";
            cbbLoaiPhong.DisplayMember = "ten_loai";
            cbbLoaiPhong.SelectedIndex = 0;
        }
        public class PhongTagInfo
        {
            public int PhongId { get; set; }
            public string TrangThai { get; set; }
            public DateTime? NgayCheckIn { get; set; }
        }
        public static void UpdatePhongStateAfterCheckout(int phongId)
        {
            using (var db = new QLKSDataContext())
            {
                try
                {
                    // Kiểm tra xem phòng có booking nào trong tương lai không
                    DateTime today = DateTime.Today;
                    var futureBooking = db.DatPhongs
                        .Where(b => b.phong_id == phongId &&
                               b.ngay_check_in > today &&
                               b.trang_thai != "da_huy" &&
                               b.trang_thai != "qua_han_check_in" &&
                               b.trang_thai != "da_thanh_toan")
                        .OrderBy(b => b.ngay_check_in)
                        .FirstOrDefault();

                    // Lấy phòng cần cập nhật
                    var phong = db.Phongs.Where(p => p.phong_id == phongId).FirstOrDefault();
                    if (phong != null)
                    {
                        if (futureBooking != null)
                        {
                            // Nếu có booking tương lai, đặt trạng thái phòng là "da_dat"
                            phong.trang_thai = "da_dat";
                        }
                        else
                        {
                            // Nếu không có booking nào trong tương lai, đặt trạng thái là "trong"
                            phong.trang_thai = "trong";
                        }
                        db.SubmitChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật trạng thái phòng sau thanh toán: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // Sửa đổi cho phương thức LoadPhongGrid để xử lý tốt hơn các phòng đã đặt trước
        private void LoadPhongGrid(int? loaiId = null)
        {
            if (db != null)
            {
                db.Dispose();
            }
            db = new QLKSDataContext();

            DateTime today = DateTime.Today;
            var raw = db.DatPhongs.ToList();

            // Chỉ lấy các đặt phòng có trạng thái "da_dat" nhưng không đến check-in đúng ngày
            //var expired = raw.Where(bk =>
            //    bk.ngay_check_in < today &&// Ngày check-in đã qua
            //    bk.trang_thai == "da_dat"); // Chỉ áp dụng cho phòng đã đặt

            //foreach (var bk in expired)
            //{
            //    // Cập nhật trạng thái phòng nếu cần
            //    var room = db.Phongs.SingleOrDefault(p => p.phong_id == bk.phong_id);
            //    if (room != null)
            //    {
            //        // Kiểm tra còn booking tương lai không (khác chính booking này)
            //        var futureBooking = raw.FirstOrDefault(fb =>
            //            fb.phong_id == bk.phong_id &&
            //            fb.dat_phong_id != bk.dat_phong_id &&
            //            fb.ngay_check_in > today &&
            //            fb.trang_thai != "da_huy" &&
            //            fb.trang_thai != "qua_han_check_in");

            //        room.trang_thai = futureBooking != null ? "da_dat" : "trong";
            //    }

            //    // Cập nhật trạng thái booking này thành "qua_han_check_in"
            //    bk.trang_thai = "qua_han_check_in";
            //}

            db.SubmitChanges();

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Padding = new Padding(20);
            flowLayoutPanel1.BackColor = Color.White;

            const int btnWidth = 270, btnHeight = 140, margin = 5;

            // Lấy danh sách phòng + loại
            var rooms = (from p in db.Phongs
                         join lp in db.LoaiPhongs on p.loai_phong_id equals lp.loai_phong_id
                         where loaiId == null || p.loai_phong_id == loaiId
                         orderby p.so_phong
                         select new { p, lp })
                        .ToList();

            //  Lấy toàn bộ booking hợp lệ (không bị hủy, không quá hạn)
            var bookings = db.DatPhongs
                .Where(b => b.trang_thai != "da_huy" && b.trang_thai != "qua_han_check_in")
                .ToList();

            // Xây dựng danh sách hiển thị, mỗi phòng 1 item
            var ds = rooms.Select(x =>
            {
                // Chọn booking đại diện
                var chosen = (DatPhong)null;
                if (x.p.trang_thai == "dang_su_dung")
                {
                    chosen = bookings
                        .Where(b => b.phong_id == x.p.phong_id &&
                                    b.ngay_check_in <= today)
                        .OrderByDescending(b => b.ngay_check_in)
                        .FirstOrDefault();
                }
                //else if (x.p.trang_thai == "da_dat" || x.p.trang_thai == "trong")
                //{
                //    // Với phòng trống hoặc đã đặt, kiểm tra có đặt trước trong tương lai không
                //    chosen = bookings
                //        .Where(b => b.phong_id == x.p.phong_id
                //                 && b.ngay_check_in >= today)
                //        .OrderBy(b => b.ngay_check_in)
                //        .FirstOrDefault();

                //    // Nếu có đặt trước và phòng đang hiển thị trống, cập nhật lại trạng thái
                //    if (chosen != null && x.p.trang_thai == "trong")
                //    {
                //        // Cập nhật trạng thái trong cơ sở dữ liệu
                //        var phong = db.Phongs.SingleOrDefault(p => p.phong_id == x.p.phong_id);
                //        if (phong != null)
                //        {
                //            phong.trang_thai = "da_dat";
                //            x.p.trang_thai = "da_dat"; // Cập nhật biến local để hiển thị đúng
                //            db.SubmitChanges();
                //        }
                //    }
                //}

                return new
                {
                    x.p.phong_id,
                    x.p.so_phong,
                    x.p.trang_thai,
                    x.p.loai_phong_id,
                    TenLoai = x.lp.ten_loai,
                    NgayCheckIn = (DateTime?)chosen?.ngay_check_in,
                    NgayCheckInString = chosen?.ngay_check_in.ToString("dd/MM/yyyy"),
                    CheckOutDate = (DateTime?)chosen?.ngay_check_out
                };
            })
            .ToList();

            foreach (var r in ds)
            {
                // Màu mặc định theo trạng thái
                var defaultColor = GetStatusColor(r.trang_thai);
                var btn = new Button
                {
                    Size = new Size(btnWidth, btnHeight),
                    BackColor = defaultColor,
                    FlatStyle = FlatStyle.Flat,
                    Tag = r.phong_id,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Image = GetStatusIcon(r.trang_thai),
                    ImageAlign = ContentAlignment.MiddleRight,
                    Margin = new Padding(margin),
                    UseVisualStyleBackColor = false
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                btn.AutoSize = false;
                btn.UseCompatibleTextRendering = true;
                BoTronButton(btn, 20);

                // Xử lý trạng thái đặt phòng
                if (r.trang_thai == "da_dat" && r.NgayCheckIn.HasValue)
                {
                    // Tính số ngày còn lại đến ngày check-in
                    int soNgayConLai = (r.NgayCheckIn.Value.Date - today).Days;

                    // 1: Ngày hôm nay < check-in 2 ngày (có thể đặt phòng)
                    if (soNgayConLai > 2)
                    {
                        btn.BackColor = Color.FromArgb(200, 230, 255); // Màu xanh dương nhạt (da_dat)
                        btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\nĐã đặt trước\n➜ Ngày {r.NgayCheckInString}\nHiện tại có thể sử dụng ngay";
                    }
                    // 2: Ngày hôm nay cách check-in ≤ 2 ngày
                    else
                    {
                        if (soNgayConLai == 0)
                        {
                            btn.BackColor = Color.FromArgb(200, 230, 255); // Màu xanh dương nhạt
                            btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\nPhòng đã đặt\n➜ Nhận phòng hôm nay";
                        }
                        else if (soNgayConLai < 0)
                        {
                            btn.BackColor = Color.Plum; // Màu tím nhạt
                            btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\nĐã quá hạn nhận phòng\n➜ {Math.Abs(soNgayConLai)} ngày";
                        }
                        else
                        {
                            btn.BackColor = Color.FromArgb(200, 230, 255); // Màu xanh dương nhạt
                            btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\nPhòng đã đặt\n➜ Ngày {r.NgayCheckInString}";
                        }
                    }
                }
                // Xử lý trạng thái đang sử dụng
                else if (r.trang_thai == "dang_su_dung" && r.CheckOutDate.HasValue)
                {
                    var co = r.CheckOutDate.Value.Date;
                    int soNgayQuaHan = (today - co).Days;
                    // Nếu trả phòng đúng hôm nay
                    if (soNgayQuaHan == 0)
                    {
                        btn.BackColor = Color.Lavender;
                        btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\nTrả Phòng hôm nay!";
                    }
                    // Nếu quá hạn
                    else if (co < today)
                    {
                        btn.BackColor = Color.Plum;
                        btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\nĐã quá hạn trả phòng\n➜ {soNgayQuaHan} ngày";
                    }
                    // Trường hợp còn lại
                    else
                    {
                        btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\n{TranslateStatus(r.trang_thai)}";
                    }
                }
                // Các trạng thái khác
                else
                {
                    if (r.trang_thai == "trong")
                    {
                        btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\nPhòng trống";
                    }
                    else
                    {
                        btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\n{TranslateStatus(r.trang_thai)}";
                    }
                }

                // Gắn Tag để xử lý Click
                if (r.trang_thai == "da_dat" && r.NgayCheckIn.HasValue)
                {
                    int soNgayConLai = (r.NgayCheckIn.Value.Date - today).Days;
                    btn.Tag = new PhongTagInfo
                    {
                        PhongId = r.phong_id,
                        TrangThai = soNgayConLai > 2 ? "co_the_dat" : "da_dat",
                        NgayCheckIn = r.NgayCheckIn
                    };
                }
                else
                {
                    btn.Tag = r.phong_id;
                }

                btn.Click += (s, e) =>
                {
                    if (btn.Tag is PhongTagInfo tagInfo)
                    {
                        if (tagInfo.TrangThai == "co_the_dat")
                            MoFormDatPhong(tagInfo.PhongId, tagInfo.NgayCheckIn);
                        else
                            OnRoomClicked(tagInfo.PhongId, r.trang_thai);
                    }
                    else if (btn.Tag is int id)
                    {
                        OnRoomClicked(id, r.trang_thai);
                    }
                };

                flowLayoutPanel1.Controls.Add(btn);
            }
        }
        private void MoFormDatPhong(int phongId, DateTime? ngayCheckInDaTon)
        {
            // Tạo form đặt phòng
            using (var frmDatPhong = new ChiTietPhongTrong(phongId, null, null))
            {
                if (frmDatPhong.ShowDialog() == DialogResult.OK)
                {
                    LoadPhongGrid(); // Tải lại dữ liệu
                }
            }
        }
        private Color GetStatusColor(string status)
        {
            switch (status)
            {
                case "trong": return Color.FromArgb(220, 255, 220);
                case "dang_su_dung": return Color.FromArgb(255, 220, 220);
                case "bao_tri": return Color.FromArgb(255, 245, 200);
                case "da_dat": return Color.FromArgb(200, 230, 255);
                default: return Color.LightGray;
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

        private Image GetStatusIcon(string status)
        {
            switch (status)
            {
                case "trong": return Properties.Resources.phongTrong;
                case "dang_su_dung": return Properties.Resources.PhongDaCoKhach;
                case "bao_tri": return Properties.Resources.PhongBaoTri;
                case "da_dat": return Properties.Resources.PhongDaDat;
                default: return Properties.Resources.phongTrong;
            }
        }

        private void OnRoomClicked(int phongId, string trangThai)
        {
            switch (trangThai)
            {
                case "trong":
                    {
                        var f = new ChiTietPhongTrong(phongId, trangThai, null);
                        f.FormClosed += (s, args) => LoadPhongGrid();
                        f.ShowDialog();

                    }
                    break;
                case "dang_su_dung":
                    {
                        var f = new ChiTietPhongSuDung(phongId);
                        f.FormClosed += (s, args) => LoadPhongGrid();
                        f.ShowDialog();
                    }
                    break;
                case "da_dat":
                    {
                        var f = new ChiTietPhongDatTruoc(phongId);
                        f.FormClosed += (s, args) => LoadPhongGrid();
                        f.ShowDialog();
                    }
                    break;
                case "bao_tri":
                    {
                        MessageBox.Show($"Phòng hiện đang BẢO TRÌ !!!", "Tình Trạng Phòng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
                default:
                    break;
            }
        }

        void AttachClickToAll(Control parent, EventHandler handler)
        {
            parent.Click += handler;
            foreach (Control child in parent.Controls)
                AttachClickToAll(child, handler);
        }
        private void BoTronButton(Button btn, int radius)
        {
            var bounds = btn.ClientRectangle;
            var path = new GraphicsPath();
            int d = radius * 2;

            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            btn.Region = new Region(path);
        }

        private void cbbLoaiPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbLoaiPhong.SelectedValue is int id && id != 0)
                LoadPhongGrid(id);
            else
                LoadPhongGrid();
        }

        private void cbbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
