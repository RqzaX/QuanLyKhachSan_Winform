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
        private DataTable GetInvoiceDataTable(int hoaDonId)
        {
            DataTable dt = new DataTable();
            string connStr = @"Server=LAPTOP-R1ZAX\MSSQLSERVER01;Database=QLKS;Integrated Security=true;";

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("dbo.sp_ThongTinHoaDon", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@hoa_don_id", hoaDonId);
                conn.Open();

                using (var rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
            }

            return dt;
        }

        private void ShowInvoice(int id)
        {
            DataTable dt = GetInvoiceDataTable(id);

            var rpt = new ThongTinHoaDon();
            rpt.SetDataSource(dt);

            rpt.SetParameterValue("@hoa_don_id", id);

            // Gán vào viewer
            crystalReportViewer1.ReportSource = rpt;
            crystalReportViewer1.Refresh();
        }


    }
}
