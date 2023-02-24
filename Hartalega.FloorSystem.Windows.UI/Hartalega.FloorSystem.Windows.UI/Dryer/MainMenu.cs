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

namespace Hartalega.FloorSystem.Windows.UI.Dryer
{
    /// <summary>
    /// Main menu for the user
    /// </summary>
    public partial class MainMenu : FormBase
    {

        private const string _dryerMainMenu = "MainMenu";
        #region constructors
        /// <summary>
        /// Instantitate the component
        /// </summary>
        public MainMenu()
        {
            InitializeComponent();
        }
        #endregion

        #region eventhandlers
        /// <summary>
        /// to close form when esc key is pressed
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
        /// <summary>
        /// To open ScanBatchCard screen 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanBatchCard_Click(object sender, EventArgs e)
        {
            new Dryer.ScanBatchCard().ShowDialog();
        }
        /// <summary>
        /// To open ScanStopTime screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanStopTime_Click(object sender, EventArgs e)
        {
            new Dryer.ScanStopTime().ShowDialog();
        }
        #endregion

       
    }
}
