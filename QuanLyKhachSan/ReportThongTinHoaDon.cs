using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyKhachSan.Reporting;
using QuanLyKhachSan.Helpers;

namespace QuanLyKhachSan
{
    public partial class ReportThongTinHoaDon : Form
    {
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private int _hoaDonId;
        public ReportThongTinHoaDon(int maHD)
        {
            InitializeComponent();
            _hoaDonId = maHD;
            // tool box ko kéo được
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.Controls.Add(this.crystalReportViewer1);
            this.ResumeLayout(false);
        }

        private void ReportThongTinHoaDon_Load(object sender, EventArgs e)
        {
            ShowInvoice(_hoaDonId);
        }

        private void ShowInvoice(int id)
        {
            using (var db = new QLKSDataContext())
            {
                var raw = db.sp_ThongTinHoaDon(id).ToList();

                // 3. Chuyển List<> thành DataTable
                DataTable dt = raw.ToDataTable();

                // 4. Tạo report và gán dữ liệu
                var rpt = new ThongTinHoaDon();
                rpt.SetDataSource(dt);

                // 5. (Nếu trong rpt có Parameter Field)
                //    Tên param phải trùng với tên bạn tạo trong Crystal Designer
                rpt.SetParameterValue("@hoa_don_id", id);

                // 6. Đẩy vào viewer
                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.Refresh();
            }
        }
    }
}
