#region using

using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;

#endregion

namespace Hartalega.FloorSystem.Windows.UI.QAI
{
    public partial class Menu : FormBase
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btnScanBatchCard_Click(object sender, EventArgs e)
        {
            new QAIScan().ShowDialog();
        }

        private void btnScanStopCard_Click(object sender, EventArgs e)
        {
            new QAIResamplingScan().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new EditDefects().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new QAIScanInnerTenPcs().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new QAIChangeQCType().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new ScanQITestResult().ShowDialog();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            btnScanBatchCard.Text = Constants.SCAN_LOOSE_GLOVES_BATCH;
            btnScanStopCard.Text = Constants.RESAMPLING_SCAN;
            button1.Text = Constants.EDIT_DEFECTS;
            button2.Text = Constants.SCAN_INNER_TENPCS;
            button3.Text = Constants.CHANGE_QC_TYPE;
            button4.Text = Constants.QI_TEST_RESULT;
            btnEditOnlineBatchCard.Text = Constants.EDIT_ONLINE_BATCH_CARD_INFO;
        }

        private void btnEditOnlineBatchCard_Click(object sender, EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.QAISYSTEM, Constants.EDIT_ONLINE_BATCH_CARD_INFO);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                new EditOnlineBatchCardInfo().ShowDialog();
            }
        }
    }
}
