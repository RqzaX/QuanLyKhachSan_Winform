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

namespace QuanLyKhachSan
{
    public partial class QL_KhuyenMai : Form
    {
        public QL_KhuyenMai()
        {
            InitializeComponent();
            LoadKhuyenMai();
        }
        private void LoadKhuyenMai()
        {
            using (var db = new QLKSDataContext())
            {
                var ds = db.KhuyenMais
                           .Select(km => new
                           {
                               km.khuyen_mai_id,
                               km.ten_khuyen_mai,
                               km.phan_tram,
                               km.ngay_bat_dau,
                               km.ngay_ket_thuc
                           })
                           .ToList();

                dgvKhuyenMai.DataSource = ds;
            }

            // ẨN
            if (dgvKhuyenMai.Columns.Contains("khuyen_mai_id"))
                dgvKhuyenMai.Columns["khuyen_mai_id"].Visible = false;

            if (dgvKhuyenMai.Columns.Contains("ten_khuyen_mai"))
                dgvKhuyenMai.Columns["ten_khuyen_mai"].HeaderText = "Tên KM";

            if (dgvKhuyenMai.Columns.Contains("phan_tram"))
            {
                var c = dgvKhuyenMai.Columns["phan_tram"];
                c.HeaderText = "Giảm (%)";
                c.DefaultCellStyle.Format = "0'%'";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvKhuyenMai.Columns.Contains("ngay_bat_dau"))
            {
                var c = dgvKhuyenMai.Columns["ngay_bat_dau"];
                c.HeaderText = "Từ ngày";
                c.DefaultCellStyle.Format = "dd/MM/yyyy";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvKhuyenMai.Columns.Contains("ngay_ket_thuc"))
            {
                var c = dgvKhuyenMai.Columns["ngay_ket_thuc"];
                c.HeaderText = "Đến ngày";
                c.DefaultCellStyle.Format = "dd/MM/yyyy";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dgvKhuyenMai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvKhuyenMai.Columns.Contains("ten_khuyen_mai"))
                dgvKhuyenMai.Columns["ten_khuyen_mai"].FillWeight = 170;  // rộng hơn
            if (dgvKhuyenMai.Columns.Contains("phan_tram"))
                dgvKhuyenMai.Columns["phan_tram"].FillWeight = 45;
            if (dgvKhuyenMai.Columns.Contains("ngay_bat_dau"))
                dgvKhuyenMai.Columns["ngay_bat_dau"].FillWeight = 55;
            if (dgvKhuyenMai.Columns.Contains("ngay_ket_thuc"))
                dgvKhuyenMai.Columns["ngay_ket_thuc"].FillWeight = 55;
            if (dgvKhuyenMai.Columns.Contains("ThaoTac"))
                dgvKhuyenMai.Columns["ThaoTac"].FillWeight = 44;

            dgvKhuyenMai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKhuyenMai.MultiSelect = false;
            dgvKhuyenMai.AllowUserToAddRows = false;
            dgvKhuyenMai.AllowUserToDeleteRows = false;
            dgvKhuyenMai.ReadOnly = true;

            if (!dgvKhuyenMai.Columns.Contains("ThaoTac"))
            {
                var btnCol = new DataGridViewButtonColumn
                {
                    Name = "ThaoTac",
                    HeaderText = "Thao tác",
                    Text = "Xóa",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat
                };
                dgvKhuyenMai.Columns.Add(btnCol);
            }
        }
        private void dgvKhuyenMai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgvKhuyenMai.Rows[e.RowIndex];
            txtMaKM.Text = row.Cells["khuyen_mai_id"].Value?.ToString();
            txtTenKM.Text = row.Cells["ten_khuyen_mai"].Value?.ToString();
            txtSoPhanTram.Text = row.Cells["phan_tram"].Value?.ToString();
            dtNgayBatDau.Text = row.Cells["ngay_bat_dau"].Value?.ToString();
            dtNgayKetThuc.Text = row.Cells["ngay_ket_thuc"].Value?.ToString();
        }

        private void dgvKhuyenMai_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvKhuyenMai.Columns[e.ColumnIndex].Name != "ThaoTac")
                return;

            var row = dgvKhuyenMai.Rows[e.RowIndex];
            int kmId = (int)row.Cells["khuyen_mai_id"].Value;
            DateTime start = (DateTime)row.Cells["ngay_bat_dau"].Value;
            DateTime end = (DateTime)row.Cells["ngay_ket_thuc"].Value;
            DateTime today = DateTime.Today;

            using (var db = new QLKSDataContext())
            {
                if (start <= today && today <= end)
                {
                    MessageBox.Show("Không thể xóa khuyến mãi đang còn hiệu lực.","Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool used = db.DatPhongs.Any(dp => dp.khuyen_mai_id == kmId);
                if (used)
                {
                    MessageBox.Show("Không thể xóa khuyến mãi đã được áp dụng trong đặt phòng.","Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string ten = row.Cells["ten_khuyen_mai"].Value.ToString();
                if (MessageBox.Show($"Xác nhận xóa khuyến mãi “{ten}”?", "Xác nhận",MessageBoxButtons.YesNo, MessageBoxIcon.Question)!= DialogResult.Yes) return;

                var km = db.KhuyenMais.Single(k => k.khuyen_mai_id == kmId);
                db.KhuyenMais.DeleteOnSubmit(km);
                db.SubmitChanges();

                MessageBox.Show("Xóa thành công!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadKhuyenMai();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMaKM.Text, out int kmId)) return;

            using (var db = new QLKSDataContext())
            {
                var km = db.KhuyenMais.Single(k => k.khuyen_mai_id == kmId);
                DateTime today = DateTime.Today;

                if (km.ngay_bat_dau <= today && today <= km.ngay_ket_thuc)
                {
                    MessageBox.Show("Không thể sửa khuyến mãi đang có hiệu lực.","Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dtNgayBatDau.Value.Date > dtNgayKetThuc.Value.Date)
                {
                    MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.","Lỗi ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                km.ten_khuyen_mai = txtTenKM.Text.Trim();
                km.phan_tram = int.Parse(txtSoPhanTram.Text.Trim());
                km.ngay_bat_dau = dtNgayBatDau.Value.Date;
                km.ngay_ket_thuc = dtNgayKetThuc.Value.Date;

                db.SubmitChanges();
                MessageBox.Show("Cập nhật thành công!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadKhuyenMai();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string ten = txtTenKM.Text.Trim();
            if (ten == "")
            {
                MessageBox.Show("Bạn phải nhập Tên khuyến mãi.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKM.Focus();
                return;
            }

            if (!int.TryParse(txtSoPhanTram.Text.Trim(), out int pct) || pct < 0 || pct > 100)
            {
                MessageBox.Show("Phần trăm phải là số nguyên từ 0 đến 100.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoPhanTram.Focus();
                return;
            }

            DateTime bd = dtNgayBatDau.Value.Date;
            DateTime kt = dtNgayKetThuc.Value.Date;
            if (bd > kt)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtNgayBatDau.Focus();
                return;
            }

            using (var db = new QLKSDataContext())
            {
                DateTime today = DateTime.Today;
                bool overlap = db.KhuyenMais.Any(km =>
                    // Nếu new bd..kt chồng lên bất kỳ km tồn tại nào
                    !(km.ngay_ket_thuc < bd || km.ngay_bat_dau > kt)
                );
                if (overlap)
                {
                    MessageBox.Show(
                      "Khoảng thời gian này đã có khuyến mãi khác chồng lắp.\n" +
                      "Vui lòng chọn ngày khác hoặc kết thúc khuyến mãi cũ trước.",
                      "Lỗi trùng lắp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var kmMoi = new KhuyenMai
                {
                    ten_khuyen_mai = ten,
                    phan_tram = pct,
                    ngay_bat_dau = bd,
                    ngay_ket_thuc = kt
                };
                db.KhuyenMais.InsertOnSubmit(kmMoi);
                db.SubmitChanges();
            }

            MessageBox.Show("Thêm khuyến mãi thành công!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadKhuyenMai();

            txtMaKM.Clear();
            txtTenKM.Clear();
            txtSoPhanTram.Clear();
            dtNgayBatDau.Value = DateTime.Today;
            dtNgayKetThuc.Value = DateTime.Today;
        }
    }
}
