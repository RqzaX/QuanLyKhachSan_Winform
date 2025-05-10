using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        private void LoadPhongGrid(int? loaiId = null)
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Padding = new Padding(20);
            flowLayoutPanel1.BackColor = Color.White;

            int btnWidth = 270, btnHeight = 140, margin = 5;
            DateTime today = DateTime.Today;

            var ds = (from p in db.Phongs
                      join lp in db.LoaiPhongs on p.loai_phong_id equals lp.loai_phong_id
                      join bk in db.DatPhongs on p.phong_id equals bk.phong_id into g
                      from bk in g.DefaultIfEmpty()
                      orderby p.so_phong
                      select new
                      {
                          p.phong_id,
                          p.so_phong,
                          p.trang_thai,
                          TenLoai = lp.ten_loai,
                          CheckInDT = (DateTime?)bk.ngay_check_in,
                          CheckOutDT = (DateTime?)bk.ngay_check_out
                      }).AsEnumerable().Select(x => new
                      {
                          x.phong_id,
                          x.so_phong,
                          x.trang_thai,
                          x.TenLoai,
                          NgayCheckIn = x.CheckInDT,
                          NgayCheckInString = x.CheckInDT?.ToString("dd/MM/yyyy"),
                          CheckOutDate = x.CheckOutDT
                      }).ToList();

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

                    // Trường hợp 1: Ngày hôm nay < check-in 2 ngày (có thể đặt phòng)
                    if (soNgayConLai > 2)
                    {
                        btn.BackColor = Color.FromArgb(220, 255, 220); // Màu xanh lá nhạt
                        btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\nPhòng trống";
                        // PhongTagInfo được tạo ở phần sự kiện click
                    }
                    // Trường hợp 2: Ngày hôm nay cách check-in ≤ 2 ngày
                    else
                    {
                        btn.BackColor = Color.FromArgb(200, 230, 255); // Màu xanh dương nhạt
                        btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\nPhòng đã đặt\n➜ Nhận phòng vào {r.NgayCheckInString}";
                        // PhongTagInfo được tạo ở phần sự kiện click
                    }
                }
                // Xử lý trạng thái đang sử dụng
                else if (r.trang_thai == "dang_su_dung" && r.CheckOutDate.HasValue)
                {
                    var co = r.CheckOutDate.Value.Date;

                    // Nếu trả phòng đúng hôm nay
                    if (co == today)
                    {
                        btn.BackColor = Color.Lavender;
                        btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\nTrả Phòng hôm nay!";
                    }
                    // Nếu quá hạn
                    else if (co < today)
                    {
                        int soNgayQuaHan = (DateTime.Today - r.CheckOutDate.Value).Days;
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
                    btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\n{TranslateStatus(r.trang_thai)}";
                }

                // Tạo một đối tượng PhongTagInfo để lưu trữ thông tin
                if (r.trang_thai == "da_dat" && r.NgayCheckIn.HasValue)
                {
                    int soNgayConLai = (r.NgayCheckIn.Value.Date - today).Days;
                    if (soNgayConLai > 2)
                    {
                        btn.Tag = new PhongTagInfo
                        {
                            PhongId = r.phong_id,
                            TrangThai = "co_the_dat",
                            NgayCheckIn = r.NgayCheckIn
                        };
                    }
                    else
                    {
                        btn.Tag = new PhongTagInfo
                        {
                            PhongId = r.phong_id,
                            TrangThai = "da_dat",
                            NgayCheckIn = r.NgayCheckIn
                        };
                    }
                }
                else
                {
                    btn.Tag = r.phong_id;
                }

                btn.Click += (s, e) =>
                {
                    // Kiểm tra loại của Tag
                    if (btn.Tag is PhongTagInfo tagInfo)
                    {
                        int id = tagInfo.PhongId;
                        string trangThai = tagInfo.TrangThai;

                        if (trangThai == "co_the_dat")
                        {
                            // Mở form đặt phòng với hạn chế
                            MoFormDatPhong(id, tagInfo.NgayCheckIn);
                        }
                        else
                        {
                            OnRoomClicked(id, r.trang_thai);
                        }
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
                // Thiết lập giới hạn ngày check-out không được vượt quá ngày check-in đã tồn tại
                //if (ngayCheckInDaTon.HasValue)
                //{
                //    frmDatPhong. = ngayCheckInDaTon.Value.AddDays(-1);
                //}

                // Hiển thị form
                if (frmDatPhong.ShowDialog() == DialogResult.OK)
                {
                    // Xử lý sau khi đặt phòng thành công
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
                        f.Show();

                    }
                    break;
                case "dang_su_dung":
                    {
                        var f = new ChiTietPhongSuDung(phongId);
                        f.FormClosed += (s, args) => LoadPhongGrid();
                        f.Show();
                    }
                    break;
                case "da_dat":
                    {
                        var f = new ChiTietPhongDatTruoc(phongId);
                        f.FormClosed += (s, args) => LoadPhongGrid();
                        f.Show();
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
