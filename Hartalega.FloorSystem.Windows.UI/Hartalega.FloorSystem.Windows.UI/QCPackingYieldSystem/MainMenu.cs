using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.QCEfficiencyData;
using System;
using Hartalega.FloorSystem.Windows.UI.Tumbling;


namespace Hartalega.FloorSystem.Windows.UI.QCPackingYieldSystem
{
    /// <summary>
    /// Module: QC Packing Yield System
    /// Screen Name: Main Menu
    /// File Type: Code file
    /// </summary>
    public partial class MainMenu : FormBase
    {
        //private const string _qcYeildandPackingMainMenu = "MainMenu";

        //kahheng 04June2019 declare constant string of "QC packing & Yielding - Edit QC Efficiency"
        private const string _qcYeildandPackingEditQCEfficiency = "QC packing & Yielding - Edit QC Efficiency";
        //end kahheng 04june2019 

        private string _login = string.Empty;
        private string _mode = string.Empty;
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public MainMenu(string login, string mode)
        {
            InitializeComponent();
            _login = login;
            _mode = mode;
            if (!string.IsNullOrEmpty(_login))
            {
                new EditQCEfficiency(_login, _mode).ShowDialog();
            }
        }

        #region Event Handlers

        /// <summary>
        /// Open Scan In Batch card screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanInBatchCard_Click(object sender, EventArgs e)
        {
            new ScanInBatchCard().ShowDialog();
        }

        /// <summary>
        /// Open Scan Out Batch card screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanOutBatchCard_Click(object sender, EventArgs e)
        {
            new QCScanning.ScanBatchCard(Constants.QC_PACKING_YIELD_SYSTEM).ShowDialog();
        }
        #endregion

        private void btnScanInQCPackingGroup_Click(object sender, EventArgs e)
        {
            new ScanInQCPackingGroup().ShowDialog();
        }

        private void btnScanOutQCPackingGroup_Click(object sender, EventArgs e)
        {
            new ScanOutQCPackingGroup().ShowDialog();
        }

        private void btnEditQcEfficiency_Click(object sender, EventArgs e)
        {
            //kahheng 04June2019 pass _qcYeildandPackingEditQCEfficiency into Login Parameter
            Login _passwordForm = new Login(Constants.Modules.QCYIELDANDPACKING, _qcYeildandPackingEditQCEfficiency);
            //End KahHeng 04June2019

            //Login _passwordForm = new Login(Constants.Modules.QCYIELDANDPACKING, _qcYeildandPackingMainMenu);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                new EditQCEfficiency(_passwordForm.Authentication, _mode).ShowDialog();
            }
        }

        private void btnScanBatchCardPieces_Click(object sender, EventArgs e)
        {
            new ScanBatchCardPieces().ShowDialog();
        }

        private void btnScanInBatchCardExtend_Click(object sender, EventArgs e)
        {
            new ScanInBatchCardExtend().ShowDialog();

        }
    }
}
