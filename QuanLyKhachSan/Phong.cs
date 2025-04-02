using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKhachSan
{
    internal class Phong
    {
        public int PhongId { get; set; }
        public string SoPhong { get; set; }
        public int LoaiPhongId { get; set; }
        public string TrangThai { get; set; }
        public virtual LoaiPhong LoaiPhong { get; set; }
    }
}
