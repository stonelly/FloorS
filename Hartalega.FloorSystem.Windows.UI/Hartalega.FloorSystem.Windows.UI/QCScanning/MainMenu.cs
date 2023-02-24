using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;

namespace Hartalega.FloorSystem.Windows.UI.QCScanning
{
    /// <summary>
    /// Module: QC Scanning
    /// Screen Name: Main Menu
    /// File Type: Code file
    /// </summary>
    public partial class MainMenu : FormBase
    {
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public MainMenu()
        {
            InitializeComponent();
            WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.QC_SCANNING_SYSTEM);
        }



        #region Event Handlers

        /// <summary>
        /// Open Scan Batch card screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanBatchCard_Click(object sender, EventArgs e)
        {
            new ScanBatchCard(Constants.QC_SCANNING_SYSTEM).ShowDialog();
        }

        /// <summary>
        /// Open Downgrade batch card screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDowngradeBatchCards_Click(object sender, EventArgs e)
        {
            new DowngradeBatchCards().ShowDialog();
        }

        /// <summary>
        /// Open Print Water Tight batch card screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintWaterTightBatchCard_Click(object sender, EventArgs e)
        {
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.WATER_TIGHT_BATCH_CARD);
            //  new Tumbling.PrintWaterTightBatchCard(Constants.QC_SCANNING_SYSTEM).ShowDialog();
        }

        /// <summary>
        /// Open Scan batch card weight screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanBatchCardWeight_Click(object sender, EventArgs e)
        {
            new ScanBatchCardWeight().ShowDialog();
        }

        /// <summary>
        /// Open Print Second Grade Sticker screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintSecondGradeSticker_Click(object sender, EventArgs e)
        {
            new PrintSecondGradeSticker().ShowDialog();
        }

        /// <summary>
        /// Open Defective Glove Platform screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDefectiveGlovePlatform_Click(object sender, EventArgs e)
        {
            new DefectiveGlovePlatform(Constants.DEFECTIVE_GLOVE_PLATFORM).ShowDialog();
        }

        /// <summary>
        /// Open Defective Glove Small Scale screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDefectiveGloveSmallScale_Click(object sender, EventArgs e)
        {
            new DefectiveGlovePlatform(Constants.DEFECTIVE_GLOVE_SMALLSCALE).ShowDialog();
        }


        private void btnSecondGradeVerification_Click(object sender, EventArgs e)
        {
            new SecondGradeStockVerification(Constants.SECOND_GRADE_VERIFICATION).ShowDialog();
        }

        private void btnSecondGradeDisposal_Click(object sender, EventArgs e)
        {
            new SecondGradeStockVerification(Constants.SECOND_GRADE_VERIFICATION_DISPOSAL).ShowDialog();
        }

        private void btnRejectStock_Click(object sender, EventArgs e)
        {
            new SecondGradeStockVerification(Constants.REJECT_VERIFICATION_DISPOSAL).ShowDialog();
        }


        private void btnRejectSticker_Click(object sender, EventArgs e)
        {
            new RejectGlove(Constants.QC_SCANNING_SYSTEM).ShowDialog();
        }
        #endregion
    }
}
