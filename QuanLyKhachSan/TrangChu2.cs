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
    public partial class TrangChu2 : Form
    {
        private Timer _colorTimer;
        private float _hue = 0f; // Giá trị Hue (0-360) trong mô hình HSL
        private const float HueSpeed = 1f; // Tốc độ thay đổi màu
        public TrangChu2()
        {
            InitializeComponent();
        }
        private void ColorTimer_Tick(object sender, EventArgs e)
        {
            // Tăng giá trị Hue và reset nếu vượt 360
            _hue = (_hue + HueSpeed) % 360;

            // Chuyển HSL -> RGB (sửa lại cách gọi hàm)
            Color rgbColor = HslToRgb(_hue, 1f, 0.5f); // Bỏ named arguments

            // Áp dụng màu cho Button
            btnCheckINOUT.BackColor = rgbColor;
        }

        // Hàm chuyển HSL sang RGB (đã sửa công thức)
        private Color HslToRgb(float h, float s, float l)
        {
            float c = (1 - Math.Abs(2 * l - 1)) * s;
            float x = c * (1 - Math.Abs((h / 60) % 2 - 1));
            float m = l - c / 2;

            float r, g, b;
            if (h < 60) { r = c; g = x; b = 0; }
            else if (h < 120) { r = x; g = c; b = 0; }
            else if (h < 180) { r = 0; g = c; b = x; }
            else if (h < 240) { r = 0; g = x; b = c; }
            else if (h < 300) { r = x; g = 0; b = c; }
            else { r = c; g = 0; b = x; }

            return Color.FromArgb(
                (int)((r + m) * 255),
                (int)((g + m) * 255),
                (int)((b + m) * 255)
            );
        }

        private void TrangChu2_Load(object sender, EventArgs e)
        {
            // Khởi tạo Timer
            _colorTimer = new System.Windows.Forms.Timer
            {
                Interval = 30 // ~33 FPS (mượt mà)
            };
            _colorTimer.Tick += ColorTimer_Tick;
            _colorTimer.Start(); // Bắt đầu animation ngay khi Form load
        }

        private void TrangChu2_FormClosing(object sender, FormClosingEventArgs e)
        {
            _colorTimer.Stop();
            _colorTimer.Dispose();
        }
    }
}
