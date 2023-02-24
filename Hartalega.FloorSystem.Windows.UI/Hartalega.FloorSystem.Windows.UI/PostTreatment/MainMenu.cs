using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.PostTreatment
{
    public partial class MainMenu : FormBase
    {
        public MainMenu()
        {
            InitializeComponent();
        }       

        #region Event Handlers
        /// <summary>
        /// Open ScanPTBatchCard screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanPTBatchCard_Click(object sender, EventArgs e)
        {
            new ScanPTBatchCard().ShowDialog();
        }

        /// <summary>
        /// Open ChangeGloveType screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeGloveType_Click(object sender, EventArgs e)
        {
            new ChangeGloveType().ShowDialog();
        }

        /// <summary>
        /// Open Print Protein Test Slip screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProteinTest_Click(object sender, EventArgs e)
        {
            new PrintTestSlip(Constants.PRINT_PROTEIN_TEST).ShowDialog();
        }

        /// <summary>
        /// Open Print Hot Box Test Slip screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHotBoxTest_Click(object sender, EventArgs e)
        {
            new PrintTestSlip(Constants.PRINT_HOTBOX_TEST).ShowDialog();
        }

        /// <summary>
        /// Open Print Powder Test Slip screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPowderTest_Click(object sender, EventArgs e)
        {
            new PrintTestSlip(Constants.PRINT_POWDER_TEST).ShowDialog();
        }

        /// <summary>
        /// Open Scan Polymer Test screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanPolymerTest_Click(object sender, EventArgs e)
        {
            new ScanPolymerTestResult().ShowDialog();
        }
        #endregion
    }
}
