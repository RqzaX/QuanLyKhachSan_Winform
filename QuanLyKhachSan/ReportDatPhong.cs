using System;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using QuanLyKhachSan.Helpers;
using QuanLyKhachSan.Reporting;

namespace QuanLyKhachSan
{
    public partial class ReportDatPhong : Form
    {
        private CrystalReportViewer crystalReportViewer1;
        private ReportDocument reportDocument;
        public ReportDatPhong()
        {
            InitializeComponent();
            this.crystalReportViewer1 = new CrystalReportViewer();
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
            this.crystalReportViewer1.ReportSource = new ThongTinDatPhong();
        }
    }
}
