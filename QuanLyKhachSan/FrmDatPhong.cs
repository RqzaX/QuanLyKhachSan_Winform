using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
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

        private void Btn_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            int phongId = (int)btn.Tag;
            // Xử lý khi click: ví dụ show chi tiết, đặt phòng, ...
            MessageBox.Show($"Bạn vừa click phòng ID = {phongId}");
        }

        private void LoadPhongGrid()
        {
            using (var db = new QLKSDataContext())
            {
                var ds = db.Phongs.OrderBy(p => p.so_phong).ToList();

                int btnWidth = 180;
                int btnHeight = 105;
                int marginX = 20;
                int marginY = 20;
                int cols = 4; // số cột mong muốn
                int x0 = marginX;
                int y0 = marginY;

                for (int i = 0; i < ds.Count; i++)
                {
                    int col = i % cols;
                    int row = i / cols;

                    int x = x0 + col * (btnWidth + marginX);
                    int y = y0 + row * (btnHeight + marginY);

                    var p = ds[i];
                    var btn = new Button
                    {
                        Text = $"Phòng {p.so_phong}\n{p.trang_thai}",
                        Size = new Size(280, 10),
                        Location = new Point(x, y),
                        BackColor = Color.FromArgb(240, 255, 240),// màu nền
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Arial", 13, FontStyle.Bold),
                        Tag = p.phong_id // lưu id để xử lý click
                    };
                    btn.FlatAppearance.BorderColor = Color.LightGray;
                    btn.Click += Btn_Click;

                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }


        private void btnTaoPhongMoi_Click(object sender, EventArgs e)
        {
            ThemLoaiPhong form = new ThemLoaiPhong();
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChiTietPhongTrong chiTietPhong = new ChiTietPhongTrong();
            chiTietPhong.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChiTietPhongSuDung frm = new ChiTietPhongSuDung();
            frm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ChiTietPhongDatTruoc frm = new ChiTietPhongDatTruoc();
            frm.ShowDialog();
        }
    }
}
