using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace QuanLyKhachSan
{
    public partial class ChiTietPhongTrong : Form
    {
        private QLKSDataContext db = new QLKSDataContext();
        public ChiTietPhongTrong()
        {
            InitializeComponent();
        }
        private void LoadKhachHang()
        {
            var khachHangs = from dv in db.DichVus
                             select new
                             {
                                 dv.dich_vu_id,
                                 dv.ten_dich_vu,
                                 dv.mo_ta,
                                 dv.gia
                             };

            dgvDichVu.DataSource = khachHangs.ToList();
            ThemDichVu();
        }
        private void ThemDichVu()
        {
            DataGridViewButtonColumn btnAdd = new DataGridViewButtonColumn();
            btnAdd.Name = "Thao tác";
            btnAdd.Text = "✛";
            btnAdd.UseColumnTextForButtonValue = true;
            dgvDichVu.Columns.Add(btnAdd);
        }
        private void FixColumnHeaders()
        {
            Dictionary<string, string> columnMappings = new Dictionary<string, string>
            {
                { "dich_vu_id", "Mã dịch vụ" },
                { "ten_dich_vu", "Tên dịch vụ" },
                { "mo_ta", "Mô tả" },
                { "gia", "Giá" }
            };

            foreach (DataGridViewColumn column in dgvDichVu.Columns)
            {
                if (columnMappings.ContainsKey(column.Name))
                {
                    column.HeaderText = columnMappings[column.Name];
                }
            }
        }

        private void ChiTietPhongTrong_Load(object sender, EventArgs e)
        {
            LoadKhachHang();
            FixColumnHeaders();
            // Khởi tạo cột cho dataGridView2 (thêm cột "Số lượng")
            dgvDatDichVu.Columns.Add("MaDichVu", "Mã dịch vụ");
            dgvDatDichVu.Columns.Add("TenDichVu", "Tên dịch vụ");
            dgvDatDichVu.Columns.Add("MoTa", "Mô tả");
            dgvDatDichVu.Columns.Add("SoLuong", "Số lượng");
            dgvDatDichVu.Columns.Add("Gia", "Giá");
            //dgvDatDichVu.Columns.Add("ThaoTac", "Thao Tác");
            dgvDichVu.Columns[0].Visible = false;
            dgvDatDichVu.Columns[0].Visible = false;
        }

        private void dgvDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvDichVu.Rows[e.RowIndex];
                string tenDichVu = selectedRow.Cells[1].Value.ToString();

                // Kiểm tra trùng lặp
                bool isDuplicate = false;
                foreach (DataGridViewRow row in dgvDatDichVu.Rows)
                {
                    if (row.Cells["TenDichVu"].Value != null &&
                        row.Cells["TenDichVu"].Value.ToString() == tenDichVu)
                    {
                        // Tăng số lượng nếu trùng
                        int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                        double gia = Convert.ToDouble(row.Cells["Gia"].Value);
                        row.Cells["SoLuong"].Value = soLuong + 1;
                        row.Cells["Gia"].Value = gia * soLuong;
                        isDuplicate = true;
                        break;
                    }
                }

                // Nếu không trùng
                if (!isDuplicate)
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(dgvDatDichVu);

                    newRow.Cells[0].Value = selectedRow.Cells[0].Value; // mã dịch vụ
                    newRow.Cells[1].Value = selectedRow.Cells[1].Value;
                    newRow.Cells[2].Value = selectedRow.Cells[2].Value;
                    newRow.Cells[4].Value = selectedRow.Cells[3].Value;
                    newRow.Cells[3].Value = 1;
                    dgvDatDichVu.Rows.Add(newRow);
                    if (dgvDatDichVu.Columns["Delete"] == null)
                    {
                        DataGridViewButtonColumn btnDeleteColumn = new DataGridViewButtonColumn();
                        btnDeleteColumn.Name = "Delete"; // Tên cột (dùng để kiểm tra)
                        btnDeleteColumn.HeaderText = "Thao tác";
                        btnDeleteColumn.Text = "Xóa";
                        btnDeleteColumn.UseColumnTextForButtonValue = true;
                        dgvDatDichVu.Columns.Add(btnDeleteColumn);
                    }
                }
            }
        }

        private void dgvDatDichVu_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvDatDichVu.Columns[e.ColumnIndex].Name == "Delete" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = dgvDatDichVu.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.ForeColor = Color.Red;
                cell.Style.Font = new Font("Arial", 10, FontStyle.Bold);
            }
        }

        private void dgvDatDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvDatDichVu.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                dgvDatDichVu.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                btnDatPhong.Text = "Đặt Trước Ngay";
                btnDatPhong.BackColor = Color.FromArgb(0, 192, 192);
            } else
            {
                btnDatPhong.Text = "Đặt Phòng Ngay";
                btnDatPhong.BackColor = Color.FromArgb(0, 192, 0);
            }
        }
    }
}
