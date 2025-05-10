using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using QuanLyKhachSan.Helpers;
using QuanLyKhachSan.Reporting;

namespace QuanLyKhachSan
{
    public partial class ReportDatPhong : Form
    {
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        public ReportDatPhong()
        {
            InitializeComponent();
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
            this.crystalReportViewer1.ReportSource = new QuanLyKhachSan.Reporting.ThongTinDatPhong();
        }
    }
}
