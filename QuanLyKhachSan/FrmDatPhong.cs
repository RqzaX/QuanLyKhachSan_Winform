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
            using (var db = new QLKSDataContext())
            {
                var ds = db.LoaiPhongs
                           .Select(lp => new
                           {
                               lp.loai_phong_id,
                               lp.ten_loai
                           })
                           .ToList();

                cbbLoaiPhong.DataSource = ds;
                cbbLoaiPhong.ValueMember = "loai_phong_id";
                cbbLoaiPhong.DisplayMember = "ten_loai";
                cbbLoaiPhong.SelectedIndex = -1; // chưa chọn
            }
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

            using (var db = new QLKSDataContext())
            {
                var ds = (from p in db.Phongs
                          join lp in db.LoaiPhongs on p.loai_phong_id equals lp.loai_phong_id
                          where !loaiId.HasValue || p.loai_phong_id == loaiId
                          let bk = db.DatPhongs
                                     .Where(dp => dp.phong_id == p.phong_id)
                                     .OrderByDescending(dp => dp.ngay_check_out)
                                     .FirstOrDefault()
                          orderby p.so_phong
                          select new
                          {
                              p.phong_id,
                              p.so_phong,
                              p.trang_thai,
                              TenLoai = lp.ten_loai,
                              CheckOutDate = (DateTime?)bk.ngay_check_out
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
                        Font = new Font("Segoe UI", 13, FontStyle.Bold),
                        Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\n{TranslateStatus(r.trang_thai)}",
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

                    // Chỉ áp dụng cho phòng đang sử dụng
                    if (r.trang_thai == "dang_su_dung" && r.CheckOutDate.HasValue)
                    {
                        var co = r.CheckOutDate.Value.Date;

                        // 1) Nếu trả phòng đúng hôm nay → nền Lavender + hover sang Purple
                        if (co == today)
                        {
                            btn.BackColor = Color.Lavender;
                            btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\nTrả Phòng hôm nay!";
                        }
                        // 2) Nếu quá hạn (today > checkout) → thêm vào list để nhấp nháy
                        else if (co < today)
                        {
                            int soNgayQuaHan = (DateTime.Today - r.CheckOutDate.Value).Days;
                            btn.BackColor = Color.Plum;
                            btn.Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\nĐã quá hạn trả phòng\n➜ {soNgayQuaHan} ngày";
                        }
                    }

                    btn.Click += (s, e) =>
                    {
                        int id = (int)btn.Tag;
                        OnRoomClicked(id, r.trang_thai);
                    };

                    flowLayoutPanel1.Controls.Add(btn);
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
                        var f = new ChiTietPhongTrong(phongId, trangThai);
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
            if (cbbLoaiPhong.SelectedValue is int id)
                LoadPhongGrid_Loc(id);
        }
        private void LoadPhongGrid_Loc(int? loaiId = null)
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Padding = new Padding(20);
            flowLayoutPanel1.BackColor = Color.White;
            DateTime today = DateTime.Today;
            int btnWidth = 270;
            int btnHeight = 140;
            int margin = 5;


            var ds = (from p in db.Phongs
                      join lp in db.LoaiPhongs on p.loai_phong_id equals lp.loai_phong_id
                      join dp in db.DatPhongs
                        on p.phong_id equals dp.phong_id into gj
                      let last = gj
                        .OrderByDescending(x => x.ngay_check_out)
                        .FirstOrDefault()

                      where !loaiId.HasValue
                            || p.loai_phong_id == loaiId
                      orderby p.so_phong

                      select new
                      {
                          p.phong_id,
                          p.so_phong,
                          p.trang_thai,
                          TenLoai = lp.ten_loai,
                          // nếu last == null thì null, ngược lại lấy ngày trả
                          CheckOutDate = (DateTime?)last.ngay_check_out
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
                    Font = new Font("Segoe UI", 13, FontStyle.Bold),
                    Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\n{TranslateStatus(r.trang_thai)}",
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

                // Chỉ áp dụng cho phòng đang sử dụng
                if (r.trang_thai == "dang_su_dung" && r.CheckOutDate.HasValue)
                {
                    var co = r.CheckOutDate.Value.Date;

                    // nền Lavender
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
                }

                btn.Click += (s, e) =>
                {
                    int id = (int)btn.Tag;
                    OnRoomClicked(id, r.trang_thai);
                };

                flowLayoutPanel1.Controls.Add(btn);
            }
        }
    }
}
