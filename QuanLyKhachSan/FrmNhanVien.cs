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
    public partial class FrmNhanVien : Form
    {
        QLKSDataContext db = new QLKSDataContext();
        public FrmNhanVien()
        {
            InitializeComponent();
        }

        private void cbCapTaiKhoan_CheckedChanged(object sender, EventArgs e)
        {
            txtTaiKhoan.Enabled = cbCapTaiKhoan.Checked;
            txtMatKhau.Enabled = cbCapTaiKhoan.Checked;
        }

        private void FrmNhanVien_Load(object sender, EventArgs e)
        {
            txtTaiKhoan.Enabled = false;
            txtMatKhau.Enabled = false;
            LoadNhanVien();
            LoadComboChucVu();
            dgvNhanVien.RowTemplate.Height = 35;
        }

        private void btnThemChucVu_Click(object sender, EventArgs e)
        {
            ThemChucVu frm = new ThemChucVu();
            frm.ShowDialog();
        }
        private void LoadNhanVien()
        {
            // a) Query lấy nhân viên kèm tên vai trò
            var ds = (from nv in db.NhanViens
                      join vt in db.VaiTros on nv.vai_tro_id equals vt.vai_tro_id
                      select new
                      {
                          nv.nhan_vien_id,
                          nv.ho_ten,
                          nv.sdt,
                          vt.ten_vai_tro,
                          nv.ca_lam_viec,
                          nv.luong,
                          nv.tai_khoan,
                          nv.mat_khau
                      })
                     .ToList();

            dgvNhanVien.DataSource = ds;

            // Ẩn
            if (dgvNhanVien.Columns.Contains("nhan_vien_id"))
                dgvNhanVien.Columns["nhan_vien_id"].Visible = false;

            if (dgvNhanVien.Columns.Contains("ho_ten"))
                dgvNhanVien.Columns["ho_ten"].HeaderText = "Họ và tên";

            if (dgvNhanVien.Columns.Contains("sdt"))
                dgvNhanVien.Columns["sdt"].HeaderText = "SĐT";

            if (dgvNhanVien.Columns.Contains("ten_vai_tro"))
                dgvNhanVien.Columns["ten_vai_tro"].HeaderText = "Chức vụ";

            if (dgvNhanVien.Columns.Contains("ca_lam_viec"))
                dgvNhanVien.Columns["ca_lam_viec"].HeaderText = "Ca làm việc";

            if (dgvNhanVien.Columns.Contains("luong"))
            {
                var col = dgvNhanVien.Columns["luong"];
                col.HeaderText = "Lương (VNĐ)";
                col.DefaultCellStyle.Format = "N0";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvNhanVien.Columns.Contains("tai_khoan"))
                dgvNhanVien.Columns["tai_khoan"].HeaderText = "Tài khoản";
            if (dgvNhanVien.Columns.Contains("mat_khau"))
                dgvNhanVien.Columns["mat_khau"].HeaderText = "Mật khẩu";

            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            if (dgvNhanVien.Columns.Contains("ho_ten"))
                dgvNhanVien.Columns["ho_ten"].Width = 180;
            if (dgvNhanVien.Columns.Contains("sdt"))
                dgvNhanVien.Columns["sdt"].Width = 120;
            if (dgvNhanVien.Columns.Contains("ten_vai_tro"))
                dgvNhanVien.Columns["ten_vai_tro"].Width = 120;
            if (dgvNhanVien.Columns.Contains("ca_lam_viec"))
                dgvNhanVien.Columns["ca_lam_viec"].Width = 270;
            if (dgvNhanVien.Columns.Contains("luong"))
                dgvNhanVien.Columns["luong"].Width = 130;
            if (dgvNhanVien.Columns.Contains("tai_khoan"))
                dgvNhanVien.Columns["tai_khoan"].Width = 100;
            if (dgvNhanVien.Columns.Contains("mat_khau"))
                dgvNhanVien.Columns["mat_khau"].Width = 100;

            
            dgvNhanVien.Columns["ten_vai_tro"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            if (!dgvNhanVien.Columns.Contains("ThaoTac"))
            {
                var img = Properties.Resources.delete;
                var imgCol = new DataGridViewImageColumn
                {
                    Name = "ThaoTac",
                    HeaderText = "Thao tác",
                    Image = img,
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                    Width = 30,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                dgvNhanVien.Columns.Add(imgCol);
            }
            dgvNhanVien.Columns["ThaoTac"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.MultiSelect = false;
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.AllowUserToDeleteRows = false;
            dgvNhanVien.ReadOnly = true;
        }

        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var colName = dgvNhanVien.Columns[e.ColumnIndex].Name;

            if (colName == "ThaoTac")
            {
                var row = dgvNhanVien.Rows[e.RowIndex];
                int id = (int)row.Cells["nhan_vien_id"].Value;
                string ten = row.Cells["ho_ten"].Value.ToString();

                if (MessageBox.Show($"Xác nhận xóa nhân viên {ten}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    using (var db = new QLKSDataContext())
                    {
                        var nv = db.NhanViens.Single(x => x.nhan_vien_id == id);
                        db.NhanViens.DeleteOnSubmit(nv);
                        db.SubmitChanges();
                    }
                    LoadNhanVien();
                }
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaNV.Text = dgvNhanVien.SelectedCells[0].Value.ToString();
            txtHoTen.Text = dgvNhanVien.SelectedCells[1].Value.ToString();
            txtSDT.Text = dgvNhanVien.SelectedCells[2].Value.ToString();
            cbbChucVu.Text = dgvNhanVien.SelectedCells[3].Value.ToString();
            cbbCaLamViec.Text = dgvNhanVien.SelectedCells[4].Value.ToString();

            // parse lương về số rồi format
            if (decimal.TryParse(dgvNhanVien.SelectedCells[5].Value.ToString(), out var luong))
                txtLuong.Text = luong.ToString("N0", new CultureInfo("vi-VN"));

            if (dgvNhanVien.SelectedCells[6].Value.ToString().Length > 0)
            {
                cbCapTaiKhoan.Checked = true;
                txtTaiKhoan.Text = dgvNhanVien.SelectedCells[6].Value.ToString();
                txtMatKhau.Text = dgvNhanVien.SelectedCells[7].Value.ToString();
            }
            else
            {
                cbCapTaiKhoan.Checked = false;
                txtTaiKhoan.Text = "";
                txtMatKhau.Text = "";
            }
        }
        private void LoadComboChucVu()
        {
            using (var db = new QLKSDataContext())
            {
                var ds = db.VaiTros
                           .OrderBy(v => v.vai_tro_id)
                           .Select(v => new
                           {
                               v.vai_tro_id,
                               v.ten_vai_tro
                           })
                           .ToList();

                cbbChucVu.DataSource = ds;
                cbbChucVu.DisplayMember = "ten_vai_tro";
                cbbChucVu.ValueMember = "vai_tro_id";
            }
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNV.Text)) return;

            int id = int.Parse(txtMaNV.Text);
            var nhanVien = db.NhanViens.Single(nv => nv.nhan_vien_id == id);
            nhanVien.ho_ten = txtHoTen.Text.Trim();
            nhanVien.sdt = txtSDT.Text.Trim();
            if (cbbChucVu.SelectedValue != null)
                nhanVien.vai_tro_id = Convert.ToInt32(cbbChucVu.SelectedValue);
            nhanVien.ca_lam_viec = cbbCaLamViec.Text.Trim();

            var vi = new CultureInfo("vi-VN");
            if (decimal.TryParse(txtLuong.Text, NumberStyles.Number, vi, out decimal result))
                nhanVien.luong = result;
            if (txtTaiKhoan.Text.Length > 0 || txtMatKhau.Text.Length > 0)
            {
                nhanVien.tai_khoan = txtTaiKhoan.Text;
                nhanVien.mat_khau = txtMatKhau.Text;
            }
            try
            {
                db.SubmitChanges();
                MessageBox.Show("Cập nhật thành công!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadNhanVien();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {

        }
    }
}
