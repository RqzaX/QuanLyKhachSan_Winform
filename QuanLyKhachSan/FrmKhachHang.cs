using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan
{
    public partial class FrmKhachHang : Form
    {
        private QLKSDataContext db = new QLKSDataContext();
        public FrmKhachHang()
        {
            InitializeComponent();
        }

        private void Khach_Load(object sender, EventArgs e)
        {
            LoadKhachHang();
            FixColumnHeaders();
            dgvKhachHang.Columns[0].Visible = false;
            dgvKhachHang.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvKhachHang.RowTemplate.Height = 35;
        }

        private void Khach_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        private void dgvKhachHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dgvKhachHang.Rows[e.RowIndex].Cells["khach_hang_id"].Value);

                if (dgvKhachHang.Columns[e.ColumnIndex].Name == "ThaoTac")
                {
                    // Xử lý xóa
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var kh = db.KhachHangs.SingleOrDefault(x => x.khach_hang_id == id);
                        if (kh != null)
                        {
                            db.KhachHangs.DeleteOnSubmit(kh);
                            db.SubmitChanges();
                            LoadKhachHang();
                        }
                    }
                }
            }
        }
        private void LoadKhachHang()
        {
            var khachHangs = from kh in db.KhachHangs
                             select new
                             {
                                 kh.khach_hang_id,
                                 kh.ho_ten,
                                 kh.dia_chi,
                                 kh.so_dien_thoai,
                                 kh.email,
                                 kh.cccd
                             };

            dgvKhachHang.DataSource = khachHangs.ToList();
            if (!dgvKhachHang.Columns.Contains("ThaoTac"))
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
                dgvKhachHang.Columns.Add(imgCol);
            }
        }
        private void FixColumnHeaders()
        {
            Dictionary<string, string> columnMappings = new Dictionary<string, string>
            {
                { "khach_hang_id", "Mã khách hàng" },
                { "ho_ten", "Họ và tên" },
                { "dia_chi", "Địa chỉ" },
                { "so_dien_thoai", "Số điện thoại" },
                { "email", "Email" },
                { "cccd", "CCCD" }
            };

            // Duyệt qua các cột và đổi tên header
            foreach (DataGridViewColumn column in dgvKhachHang.Columns)
            {
                if (columnMappings.ContainsKey(column.Name))
                {
                    column.HeaderText = columnMappings[column.Name];
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                MessageBox.Show("Chưa nhập tên kháchh hàng!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Chưa nhập địa chỉ khách hàng!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Chưa nhập số điện thoại khách hàng!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCCCD.Text))
            {
                MessageBox.Show("Chưa nhập số CCCD khách hàng!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool cccd = db.KhachHangs.Any(p => p.cccd == txtCCCD.Text);
            if (cccd)
            {
                MessageBox.Show($"Số CCCD \"{txtCCCD.Text}\" đã tồn tại.",
                                "Lỗi trùng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var newKhachHang = new KhachHang
            {
                ho_ten = txtTenKH.Text.Trim(),
                dia_chi = txtDiaChi.Text.Trim(),
                so_dien_thoai = txtSDT.Text.Trim(),
                email = txtEmail.Text.Trim(),
                cccd = txtCCCD.Text.Trim()
            };

            try
            {
                db.KhachHangs.InsertOnSubmit(newKhachHang);
                db.SubmitChanges();
                MessageBox.Show("Thêm khách hàng thành công!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadKhachHang();
                txtMaKH.Clear();
                txtTenKH.Clear();
                txtDiaChi.Clear();
                txtSDT.Clear();
                txtEmail.Clear();
                txtCCCD.Clear();
                txtTenKH.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvKhachHang.Rows[e.RowIndex];
            txtMaKH.Text = row.Cells["khach_hang_id"].Value?.ToString();
            txtTenKH.Text = dgvKhachHang.SelectedCells[2].Value.ToString();
            txtDiaChi.Text = dgvKhachHang.SelectedCells[3].Value.ToString();
            txtSDT.Text = dgvKhachHang.SelectedCells[4].Value.ToString();
            txtEmail.Text = dgvKhachHang.SelectedCells[5].Value?.ToString();
            txtCCCD.Text = dgvKhachHang.SelectedCells[6].Value.ToString();
        }
    }
}
