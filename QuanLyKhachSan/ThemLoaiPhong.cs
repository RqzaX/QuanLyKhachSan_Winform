using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Net.NetworkInformation;

namespace QuanLyKhachSan
{
    public partial class ThemLoaiPhong : Form
    {
        private bool _formatting = false;
        QLKSDataContext db = new QLKSDataContext();
        public ThemLoaiPhong()
        {
            InitializeComponent();
            LoadLoaiPhong();
            dgvLoaiPhong.RowTemplate.Height = 36;
            dgvLoaiPhong.RowTemplate.MinimumHeight = 36;
        }

        private void txtGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
        private void LoadLoaiPhong()
        {
            var ds = db.LoaiPhongs
                       .Select(lp => new
                       {
                           lp.loai_phong_id,
                           lp.ten_loai,
                           lp.mo_ta,
                           lp.gia_theo_dem
                       })
                       .ToList();

            dgvLoaiPhong.DataSource = ds;

            if (dgvLoaiPhong.Columns.Contains("loai_phong_id"))
                dgvLoaiPhong.Columns["loai_phong_id"].Visible = false;

            if (dgvLoaiPhong.Columns.Contains("ten_loai"))
                dgvLoaiPhong.Columns["ten_loai"].HeaderText = "Tên loại phòng";

            if (dgvLoaiPhong.Columns.Contains("mo_ta"))
                dgvLoaiPhong.Columns["mo_ta"].HeaderText = "Mô tả";

            if (dgvLoaiPhong.Columns.Contains("gia_theo_dem"))
            {
                dgvLoaiPhong.Columns["gia_theo_dem"].HeaderText = "Giá theo đêm";
                dgvLoaiPhong.Columns["gia_theo_dem"].DefaultCellStyle.Format = "N0"; // hàng nghìn
            }
            dgvLoaiPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvLoaiPhong.Columns.Contains("Tên loại phòng"))
                dgvLoaiPhong.Columns["Tên loại phòng"].FillWeight = 40;

            if (dgvLoaiPhong.Columns.Contains("Mô tả"))
                dgvLoaiPhong.Columns["Mô tả"].FillWeight = 110;

            if (dgvLoaiPhong.Columns.Contains("Giá theo đêm"))
                dgvLoaiPhong.Columns["Giá theo đêm"].FillWeight = 30;

            if (!dgvLoaiPhong.Columns.Contains("ThaoTac"))
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
                dgvLoaiPhong.Columns.Add(imgCol);
            }
        }
        private void txtGia_TextChanged(object sender, EventArgs e)
        {
            if (_formatting) return;
            _formatting = true;

            int selStart = txtGia.SelectionStart;
            int origLen = txtGia.Text.Length;

            string digitsOnly = new string(txtGia.Text.Where(char.IsDigit).ToArray());
            if (long.TryParse(digitsOnly, out long value))
            {
                var vi = new CultureInfo("vi-VN");
                txtGia.Text = value.ToString("N0", vi);
                int newLen = txtGia.Text.Length;
                txtGia.SelectionStart = Math.Max(0, selStart + (newLen - origLen));
            }
            _formatting = false;
        }

        private void dgvLoaiPhong_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLoaiPhong.CurrentRow == null) return;
            var row = dgvLoaiPhong.CurrentRow;

            txtMaLoaiPhong.Text = row.Cells["loai_phong_id"].Value?.ToString();
            txtTenLoaiPhong.Text = row.Cells["ten_loai"].Value?.ToString();
            txtMoTa.Text = row.Cells["mo_ta"].Value?.ToString();

            var cellVal = row.Cells["gia_theo_dem"].Value;
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
            if (string.IsNullOrWhiteSpace(txtMaLoaiPhong.Text)) return;
            int id = int.Parse(txtMaLoaiPhong.Text);
            var loaiPhong = db.LoaiPhongs.Single(p => p.loai_phong_id == id);
            loaiPhong.ten_loai = txtTenLoaiPhong.Text;
            loaiPhong.mo_ta = txtMoTa.Text;

            string giaDem = txtGia.Text;
            string digitsOnly = giaDem.Replace(".", "").Trim();
            //Parse về số
            if (!decimal.TryParse(digitsOnly, NumberStyles.None, CultureInfo.InvariantCulture, out var gia))
            {
                MessageBox.Show("Giá không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            loaiPhong.gia_theo_dem = gia;
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
            LoadLoaiPhong();
        }
    }
}
