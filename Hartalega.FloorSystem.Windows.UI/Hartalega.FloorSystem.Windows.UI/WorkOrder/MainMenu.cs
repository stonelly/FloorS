using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
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

namespace Hartalega.FloorSystem.Windows.UI.WorkOrder
{
    public partial class MainMenu : FormBase
    {
        #region Member Variables

        //private string _loggedInUser;
        private string _screenName = "Work Order - Main Menu";
        private string _className = "Main Menu";
        //private string[] _moduleId;
        //private string LoggedInUserPassword;

        #endregion

        #region Constructor

        public MainMenu()
        {
            InitializeComponent();
            //_loggedInUser = Convert.ToString(loggedInUser);
        }

        #endregion

        #region Event Handler

        private void btnWorkOrderMaintenance_Click(object sender, EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.WORKORDER, Constants.WORKORDER_MAINTENANCE);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                new WorkOrderMaintenanceList(_passwordForm.Authentication).ShowDialog();
            }
        }

        private void btnWorkOrderStatus_Click(object sender, EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.WORKORDER, Constants.WORKORDER_STATUS);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                new WorkOrderStatusList(_passwordForm.Authentication).ShowDialog();
            }
        }

        #endregion

        #region User Method

        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }

        #endregion

       
    }
}
