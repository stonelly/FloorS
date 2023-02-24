using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using Hartalega.FloorSystem.Business.Logic;
using System.Configuration;
using System;
using System.Windows.Forms;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Common;

namespace Hartalega.FloorSystem.Windows.UI.Washer
{
    /// <summary>
    /// Main Menu to open washer screens
    /// </summary>
    public partial class MainMenu : FormBase
    {
        private const string _washerMainMenu = "MainMenu";

        #region Constructors
        /// <summary>
        /// To instantiate the class
        /// </summary>
        public MainMenu()
        {
            InitializeComponent();

        }
        #endregion

        #region eventhandlers
        /// <summary>
        /// To open ScanBatchCard window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanBatchCard_Click(object sender, EventArgs e)
        {
            new Washer.ScanBatchCard().ShowDialog();
        }
        /// <summary>
        /// To open ScanStopTime window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanStopTime_Click(object sender, EventArgs e)
        {
            new Washer.ScanStopTime().ShowDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        /// <summary>
        /// To close window when escape is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
        #endregion


    }
}
