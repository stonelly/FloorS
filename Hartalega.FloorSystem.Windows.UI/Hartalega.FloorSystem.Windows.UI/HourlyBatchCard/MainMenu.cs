using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.HourlyBatchCard
{
    public partial class MainMenu : FormBase
    {
        private const string _screenNameForAuthorization = "Hourly Batch Card Main Menu";
        private static string _ManualPrintscreenName = "Batch Card Manual Print";
        private static string _ON2GRePrintscreenName = "Reprint ON2G Batch";
        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnReprintHBC_Click(object sender, EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.BATCHORDER, _ManualPrintscreenName);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication))
            {
                ReprintHBC _frmManualPrint = new ReprintHBC();
                _frmManualPrint.OperatorId = _passwordForm.Authentication;
                _frmManualPrint.ShowDialog();
            }
            //new HBC.ManualPrint().ShowDialog();
        }

        private void btnGloveBatchOrder_Click(object sender, EventArgs e)
        {
            new HourlyBatchCard.GloveBatchOrder().ShowDialog();
        }

        private void btnGloveOutputReport_Click(object sender, EventArgs e)
        {
            new HourlyBatchCard.GloveOutputReport().ShowDialog();
        }

        private void btnBatchCardReprintLog_Click(object sender, EventArgs e)
        {
            new HourlyBatchCard.BatchCardReprintLog().ShowDialog();
        }

        private void btnOnline2GGlove_Click(object sender, EventArgs e)
        {
            new HourlyBatchCard.Online2ndGradeGlove().ShowDialog();
        }

        private void btnReprintON2G_Click(object sender, EventArgs e)
        {

            Login _passwordForm = new Login(Constants.Modules.HOURLYBATCHCARD, _ON2GRePrintscreenName);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication))
            {
                ReprintON2GBatchCard _frmManualPrint = new ReprintON2GBatchCard();
                _frmManualPrint.OperatorId = _passwordForm.Authentication;
                _frmManualPrint.ShowDialog();
            }

            //new HourlyBatchCard.ReprintON2GBatchCard().ShowDialog();
        }

    }
}
