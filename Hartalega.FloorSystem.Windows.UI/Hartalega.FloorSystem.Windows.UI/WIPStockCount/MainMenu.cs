using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.WIPStockCount
{
    public partial class MainMenu : FormBase
    {
        public MainMenu()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnWIPReportByScanData_Click(object sender, EventArgs e)
        {
            new WIPReportByScanData().ShowDialog();
        }

        private void btnWIPReportByCutoffBatches_Click(object sender, EventArgs e)
        {
            new WIPReportByCutoffBatch().ShowDialog();
        }

        private void btnWIPSummaryReport_Click(object sender, EventArgs e)
        {
            new WIPSummaryReport().ShowDialog();
        }

        private void btnWIPReports_Click(object sender, EventArgs e)
        {
            new WIPReportsMenu().ShowDialog();
        }

        private void btnWIPVoidScannedData_Click(object sender, EventArgs e)
        {
            new WIPVoidScannedTxn().ShowDialog();
        }
    }
}
