using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QuanLyKhachSan.Reporting;

namespace QuanLyKhachSan
{
    public partial class FrmBaoCao : Form
    {
        private QLKSDataContext db = new QLKSDataContext();
        private List<(string ServiceName, int Count)> _usageDV;
        private int _totalCountDV;
        private List<(string LoaiPhong, int Count)> _usagePD;
        private int _totalCountPD;
        private List<string> _bookedRooms;

        public FrmBaoCao()
        {
            InitializeComponent();
            int thang = DateTime.Now.Month;
            int nam = DateTime.Now.Year;

            lbDateTrongNgay.Text = $"Doanh thi trong ngày {DateTime.Now:dd/MM}";
            lbDoangThuTrongNgay.Text = FormatCurrency(GetDailyRevenue());
            lbDateTrongThang.Text = $"Doanh thu trong tháng {DateTime.Today:MM/yyyy}";
            lbDoanhThuTrongThang.Text = FormatCurrency(GetMonthlyRevenue());

            // LINQ lấy data
            var data = db.DichVuDatPhongs
                         .Where(ddp => ddp.ngay_su_dung.Month == thang
                                    && ddp.ngay_su_dung.Year == nam)
                         .GroupBy(ddp => ddp.DichVu.ten_dich_vu)
                         .Select(g => new
                         {
                             DichVu = g.Key,
                             SoLan = g.Sum(x => x.so_luong)
                         })
                         .OrderByDescending(x => x.SoLan)
                         .ToList();

            // 1. Khởi tạo Chart
            var chart = new Chart { Dock = DockStyle.Fill };
            chart.ChartAreas.Add(new ChartArea("Default"));
            chart.Titles.Add($"Thống kê dịch vụ {thang}/{nam}");

            // 2. Tạo Series (chỉ cần 1 series cho mọi cột)
            var series = new Series("Sử dụng")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                LabelFormat = "{0:#,0}",
            };
            chart.Series.Add(series);

            // 3. Định nghĩa mảng màu
            Color[] colors = new[]
            {
        Color.FromArgb(192,  80,  77),
        Color.FromArgb(155, 187,  89),
        Color.FromArgb(128, 100, 162),
        Color.FromArgb( 75, 172, 198),
        Color.FromArgb(247, 150,  70),
        Color.FromArgb( 91, 155, 213)
             };

            // 4. Thêm từng DataPoint, gán màu, tool-tip
            for (int i = 0; i < data.Count; i++)
            {
                var pt = new DataPoint();
                pt.AxisLabel = data[i].DichVu;
                pt.YValues = new[] { (double)data[i].SoLan };
                pt.Color = colors[i % colors.Length];
                pt.ToolTip = $"{data[i].DichVu}: {data[i].SoLan} lần";
                series.Points.Add(pt);
            }

            // 5. Xây legend custom: mỗi dịch vụ là một mục
            chart.Legends.Clear();
            var legend = new Legend()
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center
            };
            foreach (var pt in series.Points)
            {
                var li = new LegendItem()
                {
                    Name = pt.AxisLabel,
                    Color = pt.Color
                };
                legend.CustomItems.Add(li);
            }
            chart.Legends.Add(legend);

            // 6. Thêm ghi chú (Title ở bottom)
            var note = new Title(
                text: $"Chú thích: dữ liệu tính đến ngày {DateTime.Now:dd/MM/yyyy}",
                docking: Docking.Bottom,
                font: new Font("Arial", 11),
                color: Color.Gray
            );
            chart.Titles.Add(note);

            panelDichVu.Controls.Clear();
            panelDichVu.Controls.Add(chart);
        }
        private void FrmBaoCao_Load(object sender, EventArgs e)
        {
            int nam = DateTime.Now.Year;
            int thangHienTai = DateTime.Now.Month;

            // 1. Lấy data
            var data = db.HoaDons
                         .Where(hd => hd.ngay_tao.Year == nam
                                   && hd.ngay_tao.Month <= thangHienTai)
                         .GroupBy(hd => hd.ngay_tao.Month)
                         .Select(g => new
                         {
                             Thang = g.Key,
                             TongTien = g.Sum(hd => hd.tong_tien)
                         })
                         .OrderBy(x => x.Thang)
                         .ToList();

            // 2. Mảng màu
            Color[] colors = {
        Color.FromArgb(91,155,213),
        Color.FromArgb(192,80,77),
        Color.FromArgb(155,187,89),
        Color.FromArgb(128,100,162),
        Color.FromArgb(247,150,70),
        Color.FromArgb(75,172,198)
    };

            // 3. Khởi tạo Chart
            var chart = new Chart { Dock = DockStyle.Fill };
            var area = new ChartArea("CArea");
            chart.ChartAreas.Add(area);
            chart.Titles.Add($"Thống kê doanh thu năm {nam}");

            // 4. Tạo series cho mỗi tháng
            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                var s = new Series($"Th{item.Thang}")
                {
                    ChartType = SeriesChartType.Column,
                    Color = colors[i % colors.Length],
                    IsValueShownAsLabel = true,
                    LabelFormat = "{0:#,0}₫",
                    Font = new Font("Arial", 9, FontStyle.Bold)
                };
                s.ChartArea = area.Name;
                s.LegendText = s.Name;

                // **Thay đổi ở đây**: gán XValue = tháng, YValue = tổng tiền
                s.Points.AddXY(item.Thang, item.TongTien);

                chart.Series.Add(s);
            }

            // 5. Cấu hình trục X để hiển thị 2,3,4,5
            var ax = chart.ChartAreas[0].AxisX;
            ax.Minimum = data.First().Thang - 1;      // 2
            ax.Maximum = thangHienTai + 1;            // 5
            ax.Interval = 1;
            ax.Title = "Tháng";
            ax.LabelStyle.Format = "'Th'0";           // prefix "Th" trước số

            // 6. Legend mặc định (hiện tên series)
            chart.Legends.Add(new Legend
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center
            });

            // 7. Ghi chú
            chart.Titles.Add(new Title(
                text: $"Chú thích: số liệu tính đến ngày {DateTime.Now:dd/MM/yyyy}",
                docking: Docking.Bottom,
                font: new Font("Arial", 11),
                color: Color.Gray
            ));

            // 8. Đưa Chart vào Panel
            panelThongKe.Controls.Clear();
            panelThongKe.Controls.Add(chart);

            ////////////////////////////////////////// Tổng quản dịch vụ
            var temp = db.DichVuDatPhongs.GroupBy(dvdp => dvdp.DichVu.ten_dich_vu).Select(g => new { ServiceName = g.Key, Count = g.Count() }).ToList();

            // 2) Tính tổng và chuyển về tuple
            _usageDV = temp.Select(x => (ServiceName: x.ServiceName, Count: x.Count)).ToList();

            _totalCountDV = _usageDV.Sum(x => x.Count);
            lblTotalServices.Text = $"Tổng số số lần đã sử dụng: {_totalCountDV}";

            // Yêu cầu panel vẽ lại
            panelTongQuanDichVu.Invalidate();
            ///////////////////////////////////////// Tổng quan phòng đặt
            var bookings = db.DatPhongs.Where(d => d.trang_thai != "trong").ToList();

            //Nhóm theo tên loại phòng và đếm
            var tempPD = bookings.GroupBy(d => d.Phong.LoaiPhong.ten_loai).Select(g => new { Loai = g.Key, SoLuong = g.Count() }).ToList();

            _usagePD = tempPD.Select(x => (LoaiPhong: x.Loai, Count: x.SoLuong)).ToList();

            _totalCountPD = _usagePD.Sum(x => x.Count);

            lbTongSoPhongDat.Text = $"Tổng số phòng đã đặt: {_totalCountPD}";

            panelPhongDat.Invalidate();
        }

        private void btnXemBaoCao_DoanhThu_Click(object sender, EventArgs e)
        {
        }

        private void panelTongQuanDichVu_Paint(object sender, PaintEventArgs e)
        {
            if (_usageDV == null || _usageDV.Count == 0) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Khung vẽ hình tròn
            var rect = new Rectangle(10, 10,
                Math.Min(panelTongQuanDichVu.Width, panelTongQuanDichVu.Height) - 20,
                Math.Min(panelTongQuanDichVu.Width, panelTongQuanDichVu.Height) - 20);

            float startAngle = 0f;
            var rnd = new Random();

            foreach (var item in _usageDV)
            {
                // Góc cho miếng pie
                float sweepAngle = (float)item.Count / _totalCountDV * 360f;

                // Chọn màu ngẫu nhiên (hoặc define mảng màu sẵn)
                Color color = Color.FromArgb(
                    rnd.Next(100, 256),
                    rnd.Next(100, 256),
                    rnd.Next(100, 256));

                using (var brush = new SolidBrush(color))
                {
                    g.FillPie(brush, rect, startAngle, sweepAngle);
                }

                // Vẽ legend nhỏ bên cạnh
                var legendRect = new Rectangle(
                    rect.Right + 20,
                    10 + _usageDV.IndexOf(item) * 25,
                    20, 20);
                g.FillRectangle(new SolidBrush(color), legendRect);
                g.DrawString(
                    $"{item.ServiceName} ({item.Count}/{_totalCountDV})",
                    this.Font,
                    Brushes.Black,
                    legendRect.Right + 5,
                    legendRect.Top);

                startAngle += sweepAngle;
            }
        }
        // Format số thành "12.345.567 vnđ"
        private string FormatCurrency(decimal value)
            => string.Format("{0:N0} vnđ", value);

        // Lấy tổng doanh thu trong ngày (LINQ-to-SQL)
        private decimal GetDailyRevenue()
        {
            using (var db = new QLKSDataContext())
            {
                return db.HoaDons
                         .Where(h => h.ngay_tao == DateTime.Today)
                         .Sum(h => (decimal?)h.tong_tien) ?? 0;
            }
        }

        // Lấy tổng doanh thu trong tháng
        private decimal GetMonthlyRevenue()
        {
            var today = DateTime.Today;
            using (var db = new QLKSDataContext())
            {
                return db.HoaDons
                         .Where(h => h.ngay_tao.Year == today.Year
                                  && h.ngay_tao.Month == today.Month)
                         .Sum(h => (decimal?)h.tong_tien) ?? 0;
            }
        }

        private void panelPhongDat_Paint(object sender, PaintEventArgs e)
        {
            if (_usagePD == null || _usagePD.Count == 0) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Hình tròn vẽ pie
            var size = Math.Min(panelPhongDat.Width, panelPhongDat.Height) - 40;
            var rect = new Rectangle(20, 20, size, size);

            float startAngle = 0f;
            var rnd = new Random();

            for (int i = 0; i < _usagePD.Count; i++)
            {
                var item = _usagePD[i];
                float sweep = (float)item.Count / _totalCountPD * 360f;

                // Chọn màu
                Color c = Color.FromArgb(
                    rnd.Next(80, 256),
                    rnd.Next(80, 256),
                    rnd.Next(80, 256));

                using (var brush = new SolidBrush(c))
                    g.FillPie(brush, rect, startAngle, sweep);

                // Vẽ legend
                var legendX = rect.Right + 20;
                var legendY = 20 + i * 25;
                var legendRect = new Rectangle(legendX, legendY, 20, 20);
                g.FillRectangle(new SolidBrush(c), legendRect);
                g.DrawRectangle(Pens.Black, legendRect);

                string text = $"{item.LoaiPhong} ({item.Count}/{_totalCountPD})";
                g.DrawString(text, this.Font, Brushes.Black, legendX + 25, legendY + 2);

                startAngle += sweep;
            }

            // Vẽ khung ngoài
            g.DrawEllipse(Pens.Gray, rect);
            LoadEmployeeOverview();
        }
        private void LoadEmployeeOverview()
        {
            List<NhanVien> employees;
            employees = db.NhanViens.ToList();

            int total = employees.Count;
            TimeSpan now = DateTime.Now.TimeOfDay;

            // 2) Tính số NV đang “trong ca” và “ngoài ca”
            int inShift = employees.Count(e => IsInShift(e.ca_lam_viec, now));
            int outShift = total - inShift;

            lbTongNhanVien.Text = $"Tổng số nhân viên: {total}";
            lbNhanVienTrongCa.Text = $"Trong ca hôm nay: {inShift}";
            lbNhanVienNgoaiCa.Text = $"Ngoài ca hôm nay: {outShift}";
        }
        private bool IsInShift(string caLamViec, TimeSpan now)
        {
            if (string.IsNullOrEmpty(caLamViec))
                return false;

            caLamViec = caLamViec.Trim();

            // case 24/24
            if (caLamViec.Equals("24/24", StringComparison.OrdinalIgnoreCase))
                return true;

            // tìm dấu ':' đầu tiên để tách phần thời gian
            int idx = caLamViec.IndexOf(':');
            if (idx < 0) return false;

            // lấy substring sau dấu ':' 
            // ví dụ " 08:00 -> 17:00" hoặc " 22:00 -> 06:00 (+1)"
            string schedule = caLamViec.Substring(idx + 1).Trim();

            // tách start và end qua "->"
            var parts = schedule.Split(new[] { "->" }, StringSplitOptions.None);
            if (parts.Length != 2) return false;

            // parse giờ bắt đầu
            if (!TimeSpan.TryParse(parts[0].Trim(), out var startTime))
                return false;

            // parse giờ kết thúc (loại bỏ "(+1)" nếu có)
            string endStr = parts[1].Trim();
            bool nextDay = endStr.EndsWith("(+1)");
            if (nextDay)
                endStr = endStr.Replace("(+1)", "").Trim();

            if (!TimeSpan.TryParse(endStr, out var endTime))
                return false;

            // nếu ca không qua đêm
            if (!nextDay)
                return now >= startTime && now <= endTime;

            // nếu ca qua đêm (vd 22:00->06:00)
            return now >= startTime || now <= endTime;
        }
    }
}