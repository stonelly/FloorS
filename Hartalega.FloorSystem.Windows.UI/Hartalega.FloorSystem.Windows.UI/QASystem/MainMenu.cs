using Hartalega.FloorSystem.Framework;
using System;
using System.Windows.Forms;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;

namespace Hartalega.FloorSystem.Windows.UI.QASystem
{
    /// <summary>
    /// Module: QA System
    /// Screen Name: Main Menu
    /// File Type: Code file
    /// </summary>
    public partial class MainMenu : FormBase
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        #region Event Handlers

        /// <summary>
        /// Event Handler for Add protein button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddProtein_Click(object sender, EventArgs e)
        {
            new AddTestResult(Constants.QA_PROTEIN).ShowDialog();
        }

        /// <summary>
        /// Event Handler for Add Powder button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPowder_Click(object sender, EventArgs e)
        {
            new AddTestResult(Constants.QA_POWDER).ShowDialog();
        }

        /// <summary>
        /// Event Handler for Add HotBox button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddHotBox_Click(object sender, EventArgs e)
        {
            new AddTestResult(Constants.QA_HOTBOX).ShowDialog();
        }

        #endregion
    }
}
