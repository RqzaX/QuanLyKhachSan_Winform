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
            LoadPhongGrid();
        }

        private void DatPhong_Load(object sender, EventArgs e)
        {

        }

        private void LoadPhongGrid()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Padding = new Padding(20);
            flowLayoutPanel1.BackColor = Color.White;

            int btnWidth = 270;
            int btnHeight = 140;
            int margin = 5;

            using (var db = new QLKSDataContext())
            {
                var ds = db.Phongs
                           .Join(db.LoaiPhongs,
                                 p => p.loai_phong_id,
                                 lp => lp.loai_phong_id,
                                 (p, lp) => new { p.phong_id, p.so_phong, p.trang_thai, TenLoai = lp.ten_loai })
                           .OrderBy(x => x.so_phong)
                           .ToList();

                foreach (var r in ds)
                {
                    var btn = new Button
                    {
                        Size = new Size(btnWidth, btnHeight),
                        BackColor = GetStatusColor(r.trang_thai),
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderColor = Color.LightGray, BorderSize = 1 },
                        Tag = r.phong_id,
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        Text = $"Phòng {r.so_phong}\n{r.TenLoai}\n\n{TranslateStatus(r.trang_thai)}",
                        TextAlign = ContentAlignment.MiddleLeft,
                        Image = GetStatusIcon(r.trang_thai),
                        ImageAlign = ContentAlignment.MiddleRight,
                        Margin = new Padding(margin),
                        UseVisualStyleBackColor = false,
                    };
                    // cho phép multiline
                    btn.AutoSize = false;
                    btn.UseCompatibleTextRendering = true;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;

                    btn.Click += (s, e) =>
                    {
                        int id = (int)((Button)s).Tag;
                        string trangThai = r.trang_thai;
                        OnRoomClicked(id, trangThai);
                    };
                    BoTronButton(btn, 20);
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
                        ChiTietPhongTrong frm = new ChiTietPhongTrong(phongId, trangThai);
                        frm.ShowDialog();
                    }
                    break;
                case "dang_su_dung":
                    {
                        ChiTietPhongSuDung frm = new ChiTietPhongSuDung();
                        frm.ShowDialog();
                    }
                    break;
                case "da_dat":
                    {
                        ChiTietPhongDatTruoc frm = new ChiTietPhongDatTruoc();
                        frm.ShowDialog();
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
    }
}
