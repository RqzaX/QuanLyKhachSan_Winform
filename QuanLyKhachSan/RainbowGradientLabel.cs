using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RainbowGradientLabel : Label
{
    private float _hue = 0f;  // giá trị hue hiện tại (0–360)

    public RainbowGradientLabel()
    {
        // bật vẽ tay và double-buffer
        SetStyle(
            ControlStyles.UserPaint
          | ControlStyles.AllPaintingInWmPaint
          | ControlStyles.OptimizedDoubleBuffer,
          true);

        // timer để cập nhật hue và invalidate
        var t = new Timer { Interval = 30 };  // bạn có thể chỉnh nhanh/chậm
        t.Tick += (s, e) =>
        {
            _hue += 1f;                 // mỗi tick tăng 1°
            if (_hue >= 360f) _hue = 0; // quay vòng
            Invalidate();               // gọi lại OnPaint
        };
        t.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var rect = ClientRectangle;

        // Tạo 3 màu cách nhau 120° hue
        Color c1 = ColorFromHSV(_hue, 1, 1);
        Color c2 = ColorFromHSV(_hue + 120, 1, 1);
        Color c3 = ColorFromHSV(_hue + 240, 1, 1);

        using (var brush = new LinearGradientBrush(
            new Rectangle(0, 0, rect.Width * 2, rect.Height),
            Color.Empty, Color.Empty,
            LinearGradientMode.Horizontal))
        {
            brush.InterpolationColors = new ColorBlend
            {
                Colors = new[] { c1, c2, c3 },
                Positions = new[] { 0f, 0.5f, 1f }
            };
            brush.WrapMode = WrapMode.Tile;

            using (var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                g.DrawString(Text, Font, brush, rect, sf);
            }
        }
    }

    // Hàm chuyển từ HSV sang RGB
    private static Color ColorFromHSV(double hue, double saturation, double value)
    {
        hue = hue % 360;
        if (hue < 0) hue += 360;
        int hi = (int)(hue / 60) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);
        value *= 255;
        int v = (int)(value);
        int p = (int)(value * (1 - saturation));
        int q = (int)(value * (1 - f * saturation));
        int t = (int)(value * (1 - (1 - f) * saturation));
        return hi switch
        {
            0 => Color.FromArgb(v, t, p),
            1 => Color.FromArgb(q, v, p),
            2 => Color.FromArgb(p, v, t),
            3 => Color.FromArgb(p, q, v),
            4 => Color.FromArgb(t, p, v),
            _ => Color.FromArgb(v, p, q),
        };
    }
}
