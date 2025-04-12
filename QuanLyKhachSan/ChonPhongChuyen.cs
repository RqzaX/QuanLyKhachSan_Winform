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
    public partial class ChonPhongChuyen : Form
    {
        public ChonPhongChuyen()
        {
            InitializeComponent();
            LoadDataToDataGridView();
            dgvPhong.RowTemplate.Height = 30;
        }
        private void LoadDataToDataGridView()
        {
            try
            {
                using (var db = new QLKSDataContext())
                {
                    var query = from p in db.Phongs
                                join lp in db.LoaiPhongs on p.loai_phong_id equals lp.loai_phong_id
                                select new
                                {
                                    p.so_phong,
                                    lp.ten_loai,
                                    trang_thai = GetTrangThai(p.trang_thai)
                                };

                    dgvPhong.DataSource = query.ToList();
                    AddButtonColumn();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private static string GetTrangThai(string trangThai)
        {
            switch (trangThai)
            {
                case "dang_su_dung":
                    return "Đang sử dụng";
                case "trong":
                    return "Trống";
                case "bao_tri":
                    return "Bảo trì";
                default:
                    return trangThai;
            }
        }
        private void AddButtonColumn()
        {
            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.Name = "Thao tác";
            btnDelete.Text = "Chuyển";
            btnDelete.UseColumnTextForButtonValue = true;
            dgvPhong.Columns.Add(btnDelete);
        }
    }
}
