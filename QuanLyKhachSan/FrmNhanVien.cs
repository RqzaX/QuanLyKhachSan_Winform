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

            if (dgvNhanVien.Columns.Contains("ma_nhan_vien"))
                dgvNhanVien.Columns["ma_nhan_vien"].HeaderText = "Mã NV";

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

            if (dgvNhanVien.Columns.Contains("co_tai_khoan"))
                dgvNhanVien.Columns["co_tai_khoan"].HeaderText = "Cấp TK";

            if (dgvNhanVien.Columns.Contains("tai_khoan"))
                dgvNhanVien.Columns["tai_khoan"].HeaderText = "Tài khoản";

            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvNhanVien.Columns.Contains("Mã NV")) dgvNhanVien.Columns["ma_nhan_vien"].FillWeight = 15;
            if (dgvNhanVien.Columns.Contains("Họ và tên")) dgvNhanVien.Columns["ho_ten"].FillWeight = 25;
            if (dgvNhanVien.Columns.Contains("SĐT")) dgvNhanVien.Columns["sdt"].FillWeight = 15;
            if (dgvNhanVien.Columns.Contains("Chức vụ")) dgvNhanVien.Columns["ten_vai_tro"].FillWeight = 20;
            if (dgvNhanVien.Columns.Contains("Ca làm việc")) dgvNhanVien.Columns["ca_lam_viec"].FillWeight = 15;
            if (dgvNhanVien.Columns.Contains("Lương (VNĐ)")) dgvNhanVien.Columns["luong"].FillWeight = 20;

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

                if (MessageBox.Show($"Xác nhận xóa nhân viên {ten}?",
                                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    == DialogResult.Yes)
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
            // Ngược lại, click cell bình thường thì load chi tiết lên control
            else
            {
                var row = dgvNhanVien.Rows[e.RowIndex];
                txtMaNV.Text = row.Cells["ma_nhan_vien"].Value?.ToString();
                txtHoTen.Text = row.Cells["ho_ten"].Value?.ToString();
                txtSDT.Text = row.Cells["sdt"].Value?.ToString();
                cbbChucVu.Text = row.Cells["ten_vai_tro"].Value?.ToString();
                cbbCaLamViec.Text = row.Cells["ca_lam_viec"].Value?.ToString();

                // parse lương về số rồi format
                if (decimal.TryParse(row.Cells["luong"].Value.ToString(), out var luong))
                    txtLuong.Text = luong.ToString("N0", new CultureInfo("vi-VN"));

                cbCapTaiKhoan.Checked = row.Cells["co_tai_khoan"].Value as bool? == true;
                txtTaiKhoan.Text = row.Cells["tai_khoan"].Value?.ToString();

                // bật các nút Sửa / Xóa
                btnSua.Enabled = true;
            }
        }
    }
}
