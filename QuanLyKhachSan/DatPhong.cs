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
    public partial class DatPhong : Form
    {
        public DatPhong()
        {
            InitializeComponent();
        }

        private void DatPhong_Load(object sender, EventArgs e)
        {
            //cbbLoaiPhong.SelectedIndex = 0;
        }
        private void AddNewButtonNextToExisting(Button existingButton, string soPhong, string trangThai, string loaiGiuong)
        {
            // Tạo Button mới
            Button newButton = new Button
            {
                Size = new Size(200, 150), // Kích thước hình vuông
                Text = $"➜ {soPhong}\n\n➜ {trangThai}\n\n➜ {loaiGiuong}",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = GetStatusColor(trangThai), // Màu nền theo trạng thái
                Tag = soPhong // Lưu thông tin phòng (tùy chọn)
            };

            // Đặt vị trí kế bên Button hiện tại
            newButton.Location = new Point(
                existingButton.Right + 10, // Cách Button cũ 10 pixels
                existingButton.Top
            );

            // Kiểm tra nếu vượt quá chiều rộng Panel thì xuống dòng
            if (newButton.Right > panelPhong.Width)
            {
                newButton.Location = new Point(
                    10, // Về đầu dòng
                    existingButton.Bottom + 10 // Xuống dưới Button cũ
                );
            }

            // Thêm sự kiện Click
            newButton.Click += (sender, e) =>
            {
                MessageBox.Show($"Đã chọn: {soPhong}\nTrạng thái: {trangThai}");
            };

            // Thêm Button vào Panel
            panelPhong.Controls.Add(newButton);
        }
        private Color GetStatusColor(string trangThai)
        {
            switch (trangThai.ToLower())
            {
                case "trống": return Color.LightGreen;
                case "đã đặt": return Color.LightSalmon;
                case "đang sửa": return Color.LightGray;
                default: return Color.White;
            }
        }
        private void RearrangeButtonsInPanel()
        {
            int x = 10, y = 10; // Vị trí bắt đầu
            int spacing = 10;    // Khoảng cách giữa các Button

            foreach (Control control in panelPhong.Controls)
            {
                if (control is Button btn)
                {
                    btn.Location = new Point(x, y);
                    x += btn.Width + spacing;

                    // Xuống dòng nếu vượt quá chiều rộng Panel
                    if (x + btn.Width > panelPhong.Width)
                    {
                        x = 10;
                        y += btn.Height + spacing;
                    }
                }
            }
        }

        private void btnPhong101_Click(object sender, EventArgs e)
        {
            // Ví dụ: Thêm phòng mới kế bên btnPhong101
            //AddNewButtonNextToExisting(btnPhong101, "Phòng số 102", "Trống", "Giường đôi");
            //RearrangeButtonsInPanel();
        }

        private void btnTaoPhongMoi_Click(object sender, EventArgs e)
        {
            TaoPhongMoi form = new TaoPhongMoi();
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChiTietPhong chiTietPhong = new ChiTietPhong();
            chiTietPhong.ShowDialog();
        }
    }
}
