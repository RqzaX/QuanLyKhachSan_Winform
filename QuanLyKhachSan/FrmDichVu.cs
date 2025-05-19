using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan
{
   
    public partial class FrmDichVu : Form
    {
        QLKSDataContext db = new QLKSDataContext();
        public FrmDichVu()
        {
            InitializeComponent();
            LoadDichVu();
            dgvDichVu.RowTemplate.Height = 30;
        }
        public void LoadDichVu()
        {
            var ds = (from dv in db.DichVus
                      select new
                      {
                          dv.dich_vu_id,
                          dv.ten_dich_vu,
                          dv.mo_ta,
                          dv.gia
                      }).ToList();
            dgvDichVu.DataSource = ds;
            if (dgvDichVu.Columns.Contains("dich_vu_id"))
            {
                var c = dgvDichVu.Columns["dich_vu_id"];
                c.HeaderText = "Mã Dịch Vụ";
                c.Name = "dich_vu_id";

            }
            if (dgvDichVu.Columns.Contains("ten_dich_vu"))
            {
                var c = dgvDichVu.Columns["ten_dich_vu"];
                c.HeaderText = "Tên Dịch Vụ";
                c.Name = "ten_dich_vu";

            }
            if (dgvDichVu.Columns.Contains("mo_ta"))
            {
                var c = dgvDichVu.Columns["mo_ta"];
                c.HeaderText = "Mô Tả";
                c.Name = "mo_ta";

            }
            if (dgvDichVu.Columns.Contains("gia"))
            {
                var c = dgvDichVu.Columns["gia"];
                c.HeaderText = "Giá";
                c.Name = "gia";

            }
            if (dgvDichVu.Columns.Contains("gia"))
            {
                var col = dgvDichVu.Columns["gia"];
                col.HeaderText = "Lương (VNĐ)";
                col.DefaultCellStyle.Format = "N0";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            // Thêm cột ảnh 1 lần (nếu chưa có)
            if (!dgvDichVu.Columns.Contains("ThaoTac"))
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
                dgvDichVu.Columns.Add(imgCol);
            }
        }

        private void dgvDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row= dgvDichVu.Rows[e.RowIndex];
            txtMaDichVu.Text = row.Cells["dich_vu_id"].Value?.ToString();
            txtTenDichVu.Text = dgvDichVu.SelectedCells[2].Value.ToString();
            txtMoTa.Text = dgvDichVu.SelectedCells[3].Value?.ToString();
            var cellVal = row.Cells["gia"].Value;
            if (cellVal != null && decimal.TryParse(cellVal.ToString(), out decimal gia))
            {
                // Format theo vi-VN
                var vi = new CultureInfo("vi-VN");
                txtGia.Text = gia.ToString("N0", vi);
            }
            else
            {
                txtGia.Clear();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDichVu.Text)) return;
            int id = int.Parse(txtMaDichVu.Text);
            var dichvu= db.DichVus.Single(p=>p.dich_vu_id == id);
            dichvu.ten_dich_vu=txtTenDichVu.Text.Trim();
            dichvu.mo_ta=txtMoTa.Text.Trim();
            string giaDem = txtGia.Text;
            string digitsOnly = giaDem.Replace(".", "").Trim();
            //Parse về số
            if (!decimal.TryParse(digitsOnly, NumberStyles.None, CultureInfo.InvariantCulture, out var gia))
            {
                MessageBox.Show("Giá không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dichvu.gia = gia;
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
            LoadDichVu();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            
            var newDichVu = new DichVu
            {
                ten_dich_vu = txtTenDichVu.Text.Trim(),
                mo_ta=txtMoTa.Text.Trim(),
                gia=decimal.Parse(txtGia.Text),

            };
            try
            {
                db.DichVus.InsertOnSubmit(newDichVu);
                db.SubmitChanges();
                MessageBox.Show("Thêm phòng thành công!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDichVu();
            }
           
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm phòng:\n" + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDichVu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDichVu.Columns[e.ColumnIndex].Name != "ThaoTac")
                return;

            int dichvuId = (int)dgvDichVu.Rows[e.RowIndex].Cells["dich_vu_id"].Value;
            string tenDichVu = dgvDichVu.Rows[e.RowIndex].Cells["ten_dich_vu"].Value.ToString();

            if (MessageBox.Show($"Bạn có chắc muốn xóa phòng {tenDichVu}?", "Xác nhận",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                != DialogResult.Yes)
                return;

            try
            {
                var dichVu = db.DichVus.Single(p => p.dich_vu_id == dichvuId);
                db.DichVus.DeleteOnSubmit(dichVu);
                db.SubmitChanges();
                LoadDichVu();
                txtMaDichVu.Clear();
                txtTenDichVu.Clear();
                txtMoTa.Clear   ();
                txtGia.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa phòng thất bại:\n\n" + ex.ToString(),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
