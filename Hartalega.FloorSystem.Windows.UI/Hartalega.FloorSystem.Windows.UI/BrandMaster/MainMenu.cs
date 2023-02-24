using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;

namespace Hartalega.FloorSystem.Windows.UI.BrandMaster
{
    public partial class MainMenu : FormBase
    {

        //private string _loggedInUser;
        private string _screenName = "Brand Master SetUp - MainMenu";
        private string _className = "MainMenu";
        //private string[] _moduleId;
        private string LoggedInUserPassword;

        public MainMenu()
        {
            InitializeComponent();
            //_loggedInUser = Convert.ToString(loggedInUser);
            //try
            //{
            //    LoggedInUserPassword = SetUpConfigurationBLL.GetModuleIdForLoggedInUserPassword(Convert.ToString(loggedInUser));
            //    string moduleId = SetUpConfigurationBLL.GetModuleIdForLoggedInUser(_loggedInUser);
            //    _moduleId = moduleId.Trim(' ', ',').Split(new char[] { ',' });
            //}
            //catch (FloorSystemException ex)
            //{
            //    ExceptionLogging(ex, _screenName, _className, "MainMenu", null);
            //    return;
            //}
        }

        private void btnBrandMasterMaintenance_Click(object sender, EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.BRANDMASTER, Constants.BRANDMASTER_MAINTENANCE);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                new BrandMasterMaintenanceList(_passwordForm.Authentication).ShowDialog();
            }
        }

        private void btnBrandMasterPreshipment_Click(object sender, EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.BRANDMASTER, Constants.BRANDMASTER_PRESHIPMENT);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                new BrandMasterPreshipmentList(_passwordForm.Authentication).ShowDialog();
            }
        }

        private void btnBrandMasterWarehouse_Click(object sender, EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.BRANDMASTER, Constants.BRANDMASTER_WAREHOUSE);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                new BrandMasterWarehouseList(_passwordForm.Authentication).ShowDialog();
            }
        }

        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }
    }
}
