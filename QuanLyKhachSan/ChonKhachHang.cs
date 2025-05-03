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
    public partial class ChonKhachHang : Form
    {
        public int SelectedCustomerId { get; private set; }
        public string SelectedCustomerName { get; private set; }
        public ChonKhachHang()
        {
            InitializeComponent();
            LoadKhachHang();
            dgvKhachHang.MultiSelect = false;
            dgvKhachHang.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvKhachHang.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            txtSearch.Text = "Tìm kiếm khách hàng";
            txtSearch.ForeColor = Color.Gray;
        }
        private void LoadKhachHang()
        {
            using (var db = new QLKSDataContext())
            {
                var ds = db.KhachHangs
                           .Select(k => new {
                               k.khach_hang_id,
                               k.ho_ten,
                               k.dia_chi,
                               k.so_dien_thoai,
                               k.email
                           })
                           .ToList();
                dgvKhachHang.DataSource = ds;

                if (dgvKhachHang.Columns.Contains("khach_hang_id"))
                    dgvKhachHang.Columns["khach_hang_id"].Visible = false;

                dgvKhachHang.Columns["ho_ten"].HeaderText = "Họ và tên";
                dgvKhachHang.Columns["dia_chi"].HeaderText = "Địa chỉ";
                dgvKhachHang.Columns["so_dien_thoai"].HeaderText = "Số điện thoại";
                dgvKhachHang.Columns["email"].HeaderText = "Email";

                if (!dgvKhachHang.Columns.Contains("ThaoTac"))
                {
                    var btnCol = new DataGridViewButtonColumn
                    {
                        Name = "ThaoTac",
                        HeaderText = "Thao tác",
                        Text = "Chọn",
                        UseColumnTextForButtonValue = true,
                        Width = 80,
                        FlatStyle = FlatStyle.Flat
                    };
                    dgvKhachHang.Columns.Add(btnCol);
                }
            }
        }
        private void btnCol_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count == 0) return;

            var row = dgvKhachHang.SelectedRows[0];
            SelectedCustomerId = (int)row.Cells["khach_hang_id"].Value;
            SelectedCustomerName = row.Cells["ho_ten"].Value.ToString();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void dgvKhachHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnCol_Click(sender, e);
        }
        private void dgvKhachHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvKhachHang.Columns[e.ColumnIndex].Name == "ThaoTac")
            {
                var row = dgvKhachHang.Rows[e.RowIndex];
                SelectedCustomerId = (int)row.Cells["khach_hang_id"].Value;
                SelectedCustomerName = row.Cells["ho_ten"].Value.ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Tìm kiếm khách hàng")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Tìm kiếm khách hàng";
                txtSearch.ForeColor = Color.Gray;
            }
        }
    }
}
