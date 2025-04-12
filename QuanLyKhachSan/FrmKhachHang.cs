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
            txtTimKiem.Text = "Tìm kiếm khách hàng";
            txtTimKiem.ForeColor = Color.Gray;
        }

        private void Khach_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        private void dgvKhachHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dgvKhachHang.Rows[e.RowIndex].Cells["khach_hang_id"].Value);

                if (dgvKhachHang.Columns[e.ColumnIndex].Name == "Thao tác")
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
            AddButtonColumn();
        }
        private void AddButtonColumn()
        {
            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.Name = "Thao tác";
            btnDelete.Text = "Xóa";
            btnDelete.UseColumnTextForButtonValue = true;
            dgvKhachHang.Columns.Add(btnDelete);
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

        private void txtTimKiem_Enter(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "Tìm kiếm khách hàng")
            {
                txtTimKiem.Text = "";
                txtTimKiem.ForeColor = Color.Black;
            }
        }

        private void txtTimKiem_Leave(object sender, EventArgs e)
        {
            if(txtTimKiem.Text.Length == 0)
            {
                txtTimKiem.Text = "Tìm kiếm khách hàng";
                txtTimKiem.ForeColor = Color.Gray;
            }
        }
    }
}
