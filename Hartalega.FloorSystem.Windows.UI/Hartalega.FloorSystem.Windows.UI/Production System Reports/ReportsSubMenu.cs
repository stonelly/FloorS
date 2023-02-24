using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.ProductionSystemReports
{
    public partial class ReportsSubMenu : FormBase
    {
        private const string _ReportsMainMenu = "ProductionReports - Main Menu";
        public ReportsSubMenu()
        {
            InitializeComponent();
        }

        private void btnOEE_Click(object sender, EventArgs e)
        {
            new OEE.Menu().ShowDialog();
        }

        private void btnPSR_Click(object sender, EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.PRODUCTIONSYSTEMREPORTS, _ReportsMainMenu);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication )&& (_passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL))
            {
                new ProductionSystemReports.MainMenu().ShowDialog();
            }
        }

        
    }
}
