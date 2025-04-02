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
            LoadPhongIntoPanel();
        }
        private void LoadPhongIntoPanel()
        {
            using (var db = new AppDbContext())
            {
                // Lấy dữ liệu kết hợp 2 bảng bằng LINQ
                var query = from p in db.Phongs.Include(p => p.LoaiPhong)
                            select p;

                var phongList = query.ToList();
                RenderPhongButtons(phongList);
            }
        }
        private void RenderPhongButtons(List<Phong> phongList)
        {
            panelPhong.Controls.Clear(); // Xóa nút cũ

            int buttonSize = 150; // Kích thước button (vuông)
            int spacing = 10;     // Khoảng cách giữa các button
            int x = spacing, y = spacing;

            foreach (var phong in phongList)
            {
                // Tạo button
                Button btn = new Button
                {
                    Size = new Size(buttonSize, buttonSize),
                    Location = new Point(x, y),
                    Text = $"Số phòng: {phong.SoPhong}\n" +
                           $"Loại: {phong.LoaiPhong.TenLoai}\n" +
                           $"Trạng thái: {phong.TrangThai}",
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = GetStatusColor(phong.TrangThai),
                    Tag = phong.PhongId // Lưu ID để xử lý sự kiện
                };

                // Định dạng button
                btn.Font = new Font("Arial", 10, FontStyle.Bold);
                btn.Click += PhongButton_Click; // Gắn sự kiện click

                // Thêm vào Panel
                panelPhong.Controls.Add(btn);

                // Cập nhật vị trí cho button tiếp theo
                x += buttonSize + spacing;
                if (x + buttonSize > panelPhong.Width)
                {
                    x = spacing;
                    y += buttonSize + spacing;
                }
            }
        }

        // Hàm lấy màu theo trạng thái
        private Color GetStatusColor(string trangThai)
        {
            switch (trangThai.ToLower())
            {
                case "trống": return Color.LightGreen;
                case "đã đặt": return Color.Orange;
                case "đang sửa": return Color.LightCoral;
                default: return Color.White;
            }
        }
        private void PhongButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int phongId = (int)btn.Tag;

            using (var db = new AppDbContext())
            {
                var phong = db.Phongs.FirstOrDefault(p => p.PhongId == phongId);
                if (phong != null)
                {
                    MessageBox.Show($"Đã chọn phòng: {phong.SoPhong}\nTrạng thái: {phong.TrangThai}");
                }
            }
        }
    }
}
