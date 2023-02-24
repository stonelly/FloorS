using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class MainMenu : FormBase
    {
        public MainMenu()
        {
            InitializeComponent();
        }
       
        private void btnPrintInnerOuterBoxLabel_Click(object sender, EventArgs e)
        {
            new ScanBatchCardInnerOuter().ShowDialog();
        }

        private void btnReprintInnerBox_Click(object sender, EventArgs e)
        {
            new ReprintInnerBox().ShowDialog();
        }

        private void btnReprintOuterCase_Click(object sender, EventArgs e)
        {
            new ReprintOuterMenu().ShowDialog();
        }

        private void btnChangeBatchCardForInner_Click(object sender, EventArgs e)
        {
            new ChangeBatchCardForInnerV2().ShowDialog();
        }       

        private void btnScanTmpPackInventory_Click(object sender, EventArgs e)
        {
            new ScanTMPPackInventory().ShowDialog();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private void txtScanMultipleBatchCard_Click(object sender, EventArgs e)
        {
            new ScanMultipleBatch().ShowDialog();
        }

        private void btnSecondGradeInnerOuter_Click(object sender, EventArgs e)
        {
            new Print2ndGradeInnerOuter().ShowDialog();
        }

        private void btnPalletCompletionSlip_Click(object sender, EventArgs e)
        {
            new PalletCompletionSlip().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new RePrint2ndGradeInner().ShowDialog();
            
        }

        private void btnSurgicalPouchPrinting_Click(object sender, EventArgs e)
        {
            new SurgicalPouchPrinting().ShowDialog();         
        }

        private void btnPrintSurgicalInnerOuter_Click(object sender, EventArgs e)
        {
            new PrintSurgicalInnerOuter().ShowDialog();
        }

        private void btnSurgicalInner_Click(object sender, EventArgs e)
        {
            new ReprintSurgicalInner().ShowDialog();
        }

        private void btnManualPalletReset_Click(object sender, EventArgs e)
        {
            new ManualPalletReset().ShowDialog();
        }

        private void btnPrintInnerOuterBoxLabelMTS_Click(object sender, EventArgs e)
        {
            new ScanBatchCardInnerOuterMTS().ShowDialog();
        }

        private void btnReprintInnerBoxMTS_Click(object sender, EventArgs e)
        {
            new ReprintInnerBoxMTS().ShowDialog();
        }

        private void btnReprintOuterCaseMTS_Click(object sender, EventArgs e)
        {
            new ReprintOuterBoxMTS().ShowDialog();
        }

        private void btnFGBatchOrder_Click(object sender, EventArgs e)
        {
            new FGBatchOrder().ShowDialog();
        }

        private void btnSurgicalPouchSPP_Click(object sender, EventArgs e)
        {
            new SurgicalPouchPrintingV2().ShowDialog();
        }

        private void btnSurgicalPrintSPP_Click(object sender, EventArgs e)
        {
            new PrintSurgicalInnerOuterV3().ShowDialog();
        }

        private void btnScanMultipleBatchCardMTS_Click(object sender, EventArgs e)
        {
            new ScanMultipleBatchMTS().ShowDialog();
        }
    }
}
