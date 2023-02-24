using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.ProductionLogging
{
    public partial class MainMenu : FormBase
    {      
        private string _moduleName = string.Empty;
        public MainMenu(string moduleName)
        {
            InitializeComponent();
            _moduleName = moduleName;
            if (!string.IsNullOrEmpty(_moduleName))
            {
                productionActivitiesToolStripMenuItem.Enabled = false;
                new ProductionLogging.ProductionLineStartStop(SetUpConfiguration.MainMenu.GetEnumDescription(Constants.ScreenNames.ProductionLogging)).ShowDialog();
            }
        }
        
        private void btnProductionLineActivities_Click(object sender, EventArgs e)
        {
            new ProductionActivity().ShowDialog();
        }

        private void productionActivitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ProductionActivity().ShowDialog();
        }

        private void lineStartStopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_moduleName))
            {
                new ProductionLogging.ProductionLineStartStop(string.Empty).ShowDialog();
            }
            else
            {
                new ProductionLogging.ProductionLineStartStop(SetUpConfiguration.MainMenu.GetEnumDescription(Constants.ScreenNames.ProductionLogging)).ShowDialog();
            }
        }

        private void speedControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string _setupConfigurationMainMenu = "Line Speed Control";
            Login _passwordForm = new Login(Constants.Modules.CONFIGURATIONSETUP, _setupConfigurationMainMenu);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                new ProductionLogging.SpeedControl(_passwordForm.Authentication).ShowDialog();
            }

            //if (string.IsNullOrEmpty(_moduleName))
            //{
            //    new ProductionLogging.SpeedControl(string.Empty).ShowDialog();
            //}
            //else
            //{
            //    new ProductionLogging.SpeedControl(SetUpConfiguration.MainMenu.GetEnumDescription(Constants.ScreenNames.ProductionLogging)).ShowDialog();
            //}
        }
    }
}
