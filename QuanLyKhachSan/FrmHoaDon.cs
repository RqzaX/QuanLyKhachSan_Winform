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
    public partial class FrmHoaDon : Form
    {
        QLKSDataContext db = new QLKSDataContext();
        public FrmHoaDon()
        {
            
            InitializeComponent();
            LoadHoaDon();
        }
        
        private void LoadHoaDon()
        {
            var ds = (from tt in db.ThanhToans
                     join hd in db.HoaDons on tt.hoa_don_id equals hd.hoa_don_id
                     join dp in db.DatPhongs on hd.dat_phong_id equals dp.dat_phong_id
                     join p in db.Phongs on dp.phong_id equals p.phong_id
                     select new
                     {
                         hd.hoa_don_id,
                         hd.dat_phong_id,
                         hd.nhan_vien_id,
                         hd.ten_nhan_vien,
                         SoPhong = p.so_phong,
                         hd.ngay_tao,
                         PhuongThuc = tt.phuong_thuc,
                         hd.tong_tien,
                        
                         
                     }).ToList();
            dgvHoaDon.DataSource = ds;
            if (dgvHoaDon.Columns.Contains("hoa_don_id"))
            {
                var c = dgvHoaDon.Columns["hoa_don_id"];
                c.HeaderText = "Mã Hóa Đơn";
                c.Name = "hoa_don_id";
                
            }
            if (dgvHoaDon.Columns.Contains("dat_phong_id"))
            {
                var c = dgvHoaDon.Columns["dat_phong_id"];
                c.HeaderText = "Mã Đặt Phòng";
                c.Name = "dat_phong_id";

            }
            if (dgvHoaDon.Columns.Contains("SoPhong"))
            {
                var c = dgvHoaDon.Columns["SoPhong"];
                c.HeaderText = "Số Phòng";
                c.Name = "SoPhong";

            }
            if (dgvHoaDon.Columns.Contains("nhan_vien_id"))
            {
                var c = dgvHoaDon.Columns["nhan_vien_id"];
                c.HeaderText = "Mã Nhân Viên";
                c.Name = "nhan_vien_id";

            }
            if (dgvHoaDon.Columns.Contains("ten_nhan_vien"))
            {
                var c = dgvHoaDon.Columns["ten_nhan_vien"];
                c.HeaderText = "Tên Nhân Viên";
                c.Name = "ten_nhan_vien";

            }
            if (dgvHoaDon.Columns.Contains("ngay_tao"))
            {
                var c = dgvHoaDon.Columns["ngay_tao"];
                c.HeaderText = "Ngày Tạo";
                c.Name = "ngay_tao";

            }
            if (dgvHoaDon.Columns.Contains("PhuongThuc"))
            {
                var c = dgvHoaDon.Columns["PhuongThuc"];
                c.HeaderText = "Phương Thức";
                c.Name = "PhuongThuc";

            }
            if (dgvHoaDon.Columns.Contains("tong_tien"))
            {
                var c = dgvHoaDon.Columns["tong_tien"];
                c.HeaderText = "Tổng Tiền";
                c.Name = "tong_tien";

            }



        }
    }
}
