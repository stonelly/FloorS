using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.GIS
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
        }

        #region Event Handlers
        /// <summary>
        /// Open ScanIn screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanIn_Click(object sender, EventArgs e)
        {
            new ScanIn().ShowDialog();
        }

        /// <summary>
        /// Open ScanOut screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanOut_Click(object sender, EventArgs e)
        {
            new ScanOut().ShowDialog();
        }

        /// <summary>
        /// Open GloveInquiry screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGloveInquiry_Click(object sender, EventArgs e)
        {
            new GloveInquiry().ShowDialog();
        }
        #endregion

        /// <summary>
        /// To close form when Esc key is pressed
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) this.Close();
            bool res = base.ProcessCmdKey(ref msg, keyData);
            return res;
        }
        
    }
}
