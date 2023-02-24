using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.Production_System_Reports
{
    public partial class DailyDetailPacking : Form
    {
        public DailyDetailPacking()
        {
            InitializeComponent();
        }

        private void DailyDetailPacking_Load(object sender, EventArgs e)
        {
            // Set the processing mode for the ReportViewer to Remote
            reportViewer1.ProcessingMode = ProcessingMode.Remote;

            ServerReport serverReport = reportViewer1.ServerReport;

            // Get a reference to the default credentials
            System.Net.ICredentials credentials =
                System.Net.CredentialCache.DefaultCredentials;

            // Get a reference to the report server credentials
            ReportServerCredentials rsCredentials =
                serverReport.ReportServerCredentials;

            // Set the credentials for the server report
            rsCredentials.NetworkCredentials = credentials;

            // Set the report server URL and report path
            serverReport.ReportServerUrl = new Uri("https://hep-fs-dev.gmd.lab");
            serverReport.ReportPath =
                "https://hep-fs-dev.gmd.lab/Reports/Pages/Report.aspx?ItemPath=%2fProduction+System+Reports%2fFinal+Packing+-+Detail+Packing";

            this.reportViewer1.RefreshReport();
        }
    }
}
