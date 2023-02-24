using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SurgicalGloveSystem
{
    public partial class MainMenu : FormBase
    {
        private static string _printScreenName = "Reprint SRBC";
        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnReprintSRBC_Click(object sender, EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.SURGICALGLOVESYSTEM, _printScreenName);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication))
            {
                ReprintSRBC _frmManualPrint = new ReprintSRBC();
                _frmManualPrint.OperatorId = _passwordForm.Authentication;
                _frmManualPrint.ShowDialog();
            }
        }

        private void btnPrintSRBC_Click(object sender, EventArgs e)
        {
            new SurgicalGloveSystem.PrintSurgicalBatchCard().ShowDialog();
        }

        private void btnSurgicalBatchOrder_Click(object sender, EventArgs e)
        {
            new SurgicalGloveSystem.GloveBatchOrder().ShowDialog();
        }

        private void btnBatchCardReprintLog_Click(object sender, EventArgs e)
        {
            new SurgicalGloveSystem.BatchCardReprintLog().ShowDialog();
        }
    }
}
