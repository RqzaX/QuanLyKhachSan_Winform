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
    public partial class FrmPhong : Form
    {
        QLKSDataContext db = new QLKSDataContext();
        public FrmPhong()
        {
            InitializeComponent();
            LOAD();
            dgvPhong.RowTemplate.Height = 30;
            dgvPhong.RowTemplate.MinimumHeight = 30;
            LoadLoaiPhong();
            dgvPhong.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvPhong.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void btnThemLoaiPhong_Click(object sender, EventArgs e)
        {
            ThemLoaiPhong frm = new ThemLoaiPhong();
            frm.ShowDialog();
        }
        private void LOAD()
        {
            var ds = (from p in db.Phongs
                      join lp in db.LoaiPhongs on p.loai_phong_id equals lp.loai_phong_id
                      select new
                      {
                          Id = p.phong_id,
                          SoPhong = p.so_phong,
                          LoaiId = p.loai_phong_id,
                          LoaiPhong = lp.ten_loai,
                          GiaTheoDem = lp.gia_theo_dem,
                          TrangThai = p.trang_thai == "trong" ? "Trống"
                                     : p.trang_thai == "dang_su_dung" ? "Đang sử dụng"
                                     : p.trang_thai == "bao_tri" ? "Bảo trì"
                                     : p.trang_thai == "da_dat" ? "Đã đặt"
                                     : p.trang_thai,
                          MoTa = lp.mo_ta
                      }).ToList();

            dgvPhong.DataSource = ds;

            // Ẩn cột
            if (dgvPhong.Columns.Contains("Id"))
                dgvPhong.Columns["Id"].Visible = false;
            if (dgvPhong.Columns.Contains("LoaiId"))
                dgvPhong.Columns["LoaiId"].Visible = false;

            // Đổi header và Name
            if (dgvPhong.Columns.Contains("SoPhong"))
            {
                var c = dgvPhong.Columns["SoPhong"];
                c.HeaderText = "Số phòng";
                c.Name = "SoPhong";
                c.Width = 120;
            }

            if (dgvPhong.Columns.Contains("LoaiPhong"))
            {
                var c = dgvPhong.Columns["LoaiPhong"];
                c.HeaderText = "Loại phòng";
                c.Name = "LoaiPhong";
                c.Width = 140;
            }

            if (dgvPhong.Columns.Contains("TrangThai"))
            {
                var c = dgvPhong.Columns["TrangThai"];
                c.HeaderText = "Trạng thái";
                c.Name = "TrangThai";
                c.Width = 220;
            }

            if (dgvPhong.Columns.Contains("GiaTheoDem"))
            {
                var c = dgvPhong.Columns["GiaTheoDem"];
                c.HeaderText = "Giá theo đêm";
                c.Name = "GiaTheoDem";
                c.Width = 220;
            }

            if (dgvPhong.Columns.Contains("MoTa"))
            {
                var c = dgvPhong.Columns["MoTa"];
                c.HeaderText = "Mô tả";
                c.Name = "MoTa";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // Thêm cột ảnh 1 lần (nếu chưa có)
            if (!dgvPhong.Columns.Contains("ThaoTac"))
            {
                var img = Properties.Resources.delete;
                var imgCol = new DataGridViewImageColumn
                {
                    Name = "ThaoTac",
                    HeaderText = "Thao tác",
                    Image = img,
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                    Width = 95,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                dgvPhong.Columns.Add(imgCol);
            }
        }
        private void LoadLoaiPhong()
        {
            var dsLoai = db.LoaiPhongs
                           .Select(lp => new {
                               lp.loai_phong_id,
                               lp.ten_loai
                           })
                           .ToList();

            cbbLoaiPhong.DataSource = dsLoai;
            cbbLoaiPhong.DisplayMember = "ten_loai";
            cbbLoaiPhong.ValueMember = "loai_phong_id";
            cbbLoaiPhong.SelectedIndex = -1;
        }

        private void dgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string trangThai = "";
            var row = dgvPhong.Rows[e.RowIndex];
            txtMaPhong.Text = row.Cells["Id"].Value?.ToString();
            txtSoPhong.Text = row.Cells[2].Value?.ToString();
            cbbLoaiPhong.Text = row.Cells[4].Value?.ToString();
            txtGiaDem.Text = row.Cells[5].Value?.ToString();
            cbbTrangThai.Text = row.Cells[6].Value?.ToString();
        }

        private void dgvPhong_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvPhong.Columns[e.ColumnIndex].Name != "ThaoTac")
                return;

            int phongId = (int)dgvPhong.Rows[e.RowIndex].Cells["Id"].Value;
            string soPhong = dgvPhong.Rows[e.RowIndex].Cells["SoPhong"].Value.ToString();

            if (MessageBox.Show($"Bạn có chắc muốn xóa phòng {soPhong}?", "Xác nhận",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                != DialogResult.Yes)
                return;

            try
            {
                var phong = db.Phongs.Single(p => p.phong_id == phongId);
                db.Phongs.DeleteOnSubmit(phong);
                db.SubmitChanges();
                LOAD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa phòng thất bại:\n\n" + ex.ToString(),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaPhong.Text)) return;

            int id = int.Parse(txtMaPhong.Text);
            var phong = db.Phongs.Single(p => p.phong_id == id);
            phong.so_phong = txtSoPhong.Text.Trim();
            phong.loai_phong_id = (int)cbbLoaiPhong.SelectedValue;
            string tt = cbbTrangThai.SelectedItem.ToString();
            phong.trang_thai = tt == "Trống" ? "trong"
                             : tt == "Đang sử dụng" ? "dang_su_dung"
                             : tt == "Bảo trì" ? "bao_tri"
                             : tt == "Đã đặt" ? "da_dat"
                             : tt;
            try
            {
                db.SubmitChanges();
                MessageBox.Show("Cập nhật thành công!", "OK",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật:\n" + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LOAD();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoPhong.Text))
            {
                MessageBox.Show("Chưa nhập số phòng!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbbLoaiPhong.SelectedIndex < 0 || cbbTrangThai.SelectedIndex < 0)
            {
                MessageBox.Show("Bạn phải chọn loại phòng và trạng thái!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string trangThai = cbbTrangThai.SelectedItem.ToString();
            string trangThai_Thuc;

            switch (trangThai)
            {
                case "Trống":
                    trangThai_Thuc = "trong";
                    break;
                case "Đang sử dụng":
                    trangThai_Thuc = "dang_su_dung";
                    break;
                case "Đã đặt":
                    trangThai_Thuc = "da_dat";
                    break;
                case "Bảo trì":
                    trangThai_Thuc = "bao_tri";
                    break;
                default:
                    trangThai_Thuc = "trong";
                    break;
            }

            var newPhong = new Phong
            {
                so_phong = txtSoPhong.Text.Trim(),
                loai_phong_id = (int)cbbLoaiPhong.SelectedValue,
                trang_thai = trangThai_Thuc
            };

            try
            {
                db.Phongs.InsertOnSubmit(newPhong);
                db.SubmitChanges();

                MessageBox.Show("Thêm phòng thành công!", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                LOAD();
                txtSoPhong.Clear();
                cbbTrangThai.SelectedIndex = -1;
                cbbLoaiPhong.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm phòng:\n\n" + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
