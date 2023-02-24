// -----------------------------------------------------------------------
// <copyright file="MainMenu.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;

namespace Hartalega.FloorSystem.Windows.UI.Tumbling
{
    /// <summary>
    /// Main Menu for the user 
    /// </summary>
    public partial class MainMenu : FormBase
    {
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public MainMenu()
        {
            InitializeComponent();
            WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.TUMBLING_SYSTEM);
        }

        #region Event Handlers
        /// <summary>
        /// Open PrintNormalBatchCard screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintNormalBatchCard_Click(object sender, EventArgs e)
        {
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.NORMAL_BATCH_CARD );
            new PrintNormalBatchCard().ShowDialog();
        }

        /// <summary>
        /// Open PrintLostBatchCard screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLostNormalBatchCard_Click(object sender, EventArgs e)
        {
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.LOST_BATCH_CARD);
            new PrintLostBatchCard().ShowDialog();
        }

        /// <summary>
        /// Open PrintVisualTestBatchCard screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintVTBatchCard_Click(object sender, EventArgs e)
        {
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.VISUAL_TEST_BATCH_CARD);
            new PrintVisualTestBatchCard().ShowDialog();
        }

        /// <summary>
        /// Open PrintWaterTightBatchCard screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintWTBatchCard_Click(object sender, EventArgs e)
        {
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.WATER_TIGHT_BATCH_CARD);
            new PrintWaterTightBatchCard(string.Empty).ShowDialog();
        }

        /// <summary>
        /// Open OnlineByPassGloveBatchCard screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnByPassGloveBatchCard_Click(object sender, EventArgs e)
        {
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.ONLINE_BYPASS_BATCH_CARD);
            new OnlineByPassGloveBatchCard().ShowDialog();
        }

        /// <summary>
        /// Open RejectGloves screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRejectGloves_Click(object sender, EventArgs e)
        {
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.REJECT_GLOVE_SCREEN );
            new OnlineRejectGlove(string.Empty).ShowDialog();
        }

        /// <summary>
        /// Open RejectGloves screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPolymerTestSlip_Click(object sender, EventArgs e)
        {
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.POLYMER_TEST_SLIP_SCREEN);
            new PrintPolymerTestSlip().ShowDialog();
        }
        #endregion

        private void btnPrintCustomerRejectGloveBatch_Click(object sender, EventArgs e)
        {
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.CUSTOMER_REJECT_GLOVE_BATCH_CARD );
            new PrintCustomerRejectGloveBatchCard().ShowDialog();
        }

        private void btnPrintReproductionVTBatchCard_Click(object sender, EventArgs e)
        {
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.REPRODUCTION_VISUAL_TEST_BATCH_CARD);
            new PrintReproductionVisualTestBatchCard().ShowDialog();
        }

        private void btnPrintReproductionWTBatchCard_Click(object sender, EventArgs e)
        {
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.REPRODUCTION_WATER_TIGHT_BATCH_CARD);
            new PrintReproductionWaterTightBatchCard().ShowDialog();
            //new PrintReproductionWaterTight().ShowDialog();
        }

        private void btnReprintBatchCard_Click(object sender, EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.TUMBLING, "Batch Card Manual Print");
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication))
            {
                ReprintBatchCard _frmManualPrint = new ReprintBatchCard();
                _frmManualPrint.OperatorId = _passwordForm.Authentication;
                _frmManualPrint.ShowDialog();
            }
        }

    }
}
