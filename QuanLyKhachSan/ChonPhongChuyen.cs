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
    public partial class ChonPhongChuyen : Form
    {
        private readonly QLKSDataContext db = new QLKSDataContext();
        private readonly int _datPhongId;
        /// <summary>
        /// SelectPhongId sẽ được parent đọc sau khi đóng form
        /// </summary>
        public int SelectPhongId { get; private set; }
        public ChonPhongChuyen(int datPhongId)
        {
            InitializeComponent();
            LoadDanhSachPhong();
            dgvPhong.RowTemplate.Height = 40;
            _datPhongId = datPhongId;
        }
        private void LoadDanhSachPhong()
        {
            var ds = (from p in db.Phongs
                      join lp in db.LoaiPhongs on p.loai_phong_id equals lp.loai_phong_id
                      select new
                      {
                          p.phong_id,
                          p.so_phong,
                          Loai = lp.ten_loai,
                          TrangThai = GetTrangThai(p.trang_thai)
                      })
                     .ToList();

            dgvPhong.DataSource = ds;

            // Thêm cột button “Chuyển” nếu chưa có
            if (!dgvPhong.Columns.Contains("Chuyen"))
            {
                var btn = new DataGridViewButtonColumn
                {
                    Name = "Chuyen",
                    HeaderText = "Thao tác",
                    Text = "Chuyển",
                    UseColumnTextForButtonValue = true,
                    Width = 80
                };
                dgvPhong.Columns.Add(btn);
            }

            // Ẩn cột id
            dgvPhong.Columns["phong_id"].Visible = false;
        }
        private static string GetTrangThai(string trangThai)
        {
            switch (trangThai)
            {
                case "dang_su_dung":
                    return "Đang sử dụng";
                case "trong":
                    return "Trống";
                case "bao_tri":
                    return "Bảo trì";
                default:
                    return trangThai;
            }
        }

        private void dgvPhong_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (!(dgvPhong.Columns[e.ColumnIndex] is DataGridViewButtonColumn)) return;

            int newPhongId = (int)dgvPhong.Rows[e.RowIndex].Cells["phong_id"].Value;

            //Gọi confirm
            var soPhong = dgvPhong.Rows[e.RowIndex].Cells["so_phong"].Value;
            if (MessageBox.Show($"Bạn có chắc muốn chuyển sang phòng {soPhong} không?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            // Cập nhật booking
            var dp = db.DatPhongs.SingleOrDefault(x => x.dat_phong_id == _datPhongId);
            if (dp == null)
            {
                MessageBox.Show("Không tìm thấy booking đang mở để chuyển.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string trangThaiPhongOld = dp.trang_thai;
            // Phòng mới không được trùng với phòng cũ
            if (dp.phong_id == newPhongId)
            {
                MessageBox.Show("Phòng mới không được trùng với phòng cũ.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Phòng mới không được đang sử dụng
            var phongMoi = db.Phongs.SingleOrDefault(p => p.phong_id == newPhongId);
            if (phongMoi == null || phongMoi.trang_thai != "trong")
            {
                MessageBox.Show("Phòng mới không hợp lệ hoặc đang sử dụng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // trả lại trạng thái trống phòng cũ
            var oldPhong = db.Phongs.Single(p => p.phong_id == dp.phong_id);
            oldPhong.trang_thai = "trong";

            dp.phong_id = newPhongId;
            //dp.ngay_chuyen = DateTime.Now;
            db.SubmitChanges();

            // Cập nhật trạng thái phòng mới
            var newPhong = db.Phongs.Single(p => p.phong_id == newPhongId);
            if(trangThaiPhongOld == "da_dat")
            {
                newPhong.trang_thai = "da_dat";
            }
            else
            {
                newPhong.trang_thai = "dang_su_dung";
            }
            db.SubmitChanges();

            // Trả về ID mới cho form cha
            SelectPhongId = newPhongId;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
