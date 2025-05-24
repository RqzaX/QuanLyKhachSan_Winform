using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKhachSan
{
    public class HoaDonDto
    {
        public int HoaDonId { get; set; }
        public string TenPhong { get; set; }
        public string TenNhanVien { get; set; }
        public DateTime NgayTao { get; set; }
        public decimal TongTien { get; set; }
    }
}
