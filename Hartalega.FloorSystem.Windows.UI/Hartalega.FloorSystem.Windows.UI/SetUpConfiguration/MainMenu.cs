using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Main Menu
    /// File Type: Code file
    /// </summary>
    public partial class MainMenu : FormBase
    {
        #region Member Variables
        private string _loggedInUser;
        private string _screenName = "Configuration SetUp - MainMenu";
        private string _className = "MainMenu";
        private string[] _moduleId;
        private string LoggedInUserPassword;
        #endregion
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public MainMenu(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = Convert.ToString(loggedInUser);
            try
            {
                LoggedInUserPassword = SetUpConfigurationBLL.GetModuleIdForLoggedInUserPassword(Convert.ToString(loggedInUser));
                string moduleId = SetUpConfigurationBLL.GetModuleIdForLoggedInUser(_loggedInUser);
                _moduleId = moduleId.Trim(' ', ',').Split(new char[] { ',' });
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "MainMenu", null);
                return;
            }
        }
        #region User Methods

        /// <summary>
        /// Fetching Password Through LoggedIdUser
        /// </summary>
        /// <returns></returns>
        private bool GetFormsPermissionsForLoggedInUser(string screenName)
        {
            bool isAuthorized = SecurityModuleBLL.ValidateEmployeeCredential(LoggedInUserPassword, screenName);
            return isAuthorized;
        }

        /// <summary>
        /// Get Enum Description
        /// </summary>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        { 
            FieldInfo fi = value.GetType().GetField(value.ToString()); 
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false); 
            if (attributes != null && attributes.Length > 0)        
                return attributes[0].Description; 
            else        
                return value.ToString(); 
        }

        /// <summary>
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for Form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_Load(object sender, EventArgs e)
        {
            int qcGroupCount = 0;
            int productionLineCount = 0;

            for(int i=0; i<_moduleId.Length; i++)
            {
                if (Convert.ToInt16(_moduleId[i]) == Convert.ToInt16(Constants.Modules.DRYER))
                {
                    btnDryerMaster.Enabled = true;
                    btnDryerStoppageData.Enabled = true;
                }
                else if(Convert.ToInt16(_moduleId[i]) == Convert.ToInt16(Constants.Modules.WASHER))
                {
                    btnWasherMaster.Enabled = true;
                    btnWasherStoppageData.Enabled = true;
                }
                else if (Convert.ToInt16(_moduleId[i]) == Convert.ToInt16(Constants.Modules.QAISYSTEM))
                {
                    btnQAIDefectMaster.Enabled = true;
                }
                else if (Convert.ToInt16(_moduleId[i]) == Convert.ToInt16(Constants.Modules.QCSCANNINGSYSTEM) ||
                         Convert.ToInt16(_moduleId[i]) == Convert.ToInt16(Constants.Modules.QCYIELDANDPACKING))
                {
                    qcGroupCount = qcGroupCount + 1;
                }
                else if (Convert.ToInt16(_moduleId[i]) == Convert.ToInt16(Constants.Modules.HOURLYBATCHCARD)||
                         Convert.ToInt16(_moduleId[i]) == Convert.ToInt16(Constants.Modules.PRODUCTIONLOGGING) ||
                         Convert.ToInt16(_moduleId[i]) == Convert.ToInt16(Constants.Modules.TUMBLING))
                {
                    productionLineCount = productionLineCount + 1;
                }

                if(qcGroupCount == Constants.TWO)
                {
                    btnQCGroupMaster.Enabled = true;
                    btnQCGroupStoppageData.Enabled = true;
                }

                if (productionLineCount == Constants.THREE)
                {
                    btnProdLineMaintenance.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Open BatchInquiry screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBatchInquiry_Click(object sender, EventArgs e)
        {
            if (GetFormsPermissionsForLoggedInUser(GetEnumDescription(Constants.ScreenNames.BatchInquiry)))
            {
                new BatchInquiry().ShowDialog();
            }
            else
            {
                GlobalMessageBox.Show(Messages.SCREEN_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Open ReasonMaster screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReasonMaster_Click(object sender, EventArgs e)
        {
            if (GetFormsPermissionsForLoggedInUser(GetEnumDescription(Constants.ScreenNames.ReasonMaster)))
            {
                new ReasonMaster(_loggedInUser).ShowDialog();
            }
            else
            {
                GlobalMessageBox.Show(Messages.SCREEN_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Open WasherMaster screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWasherMaster_Click(object sender, EventArgs e)
        {
            if (GetFormsPermissionsForLoggedInUser(GetEnumDescription(Constants.ScreenNames.WasherMaster)))
            {
                new WasherMaster(_loggedInUser).ShowDialog();
            }
            else
            {
                GlobalMessageBox.Show(Messages.SCREEN_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Open DryerMaster screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDryerMaster_Click(object sender, EventArgs e)
        {
            if (GetFormsPermissionsForLoggedInUser(GetEnumDescription(Constants.ScreenNames.DryerMaster)))
            {
                new DryerMaster(_loggedInUser).ShowDialog();
            }
            else
            {
                GlobalMessageBox.Show(Messages.SCREEN_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Open Washer Stoppage data screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWasherStoppageData_Click(object sender, EventArgs e)
        {
            if (GetFormsPermissionsForLoggedInUser(GetEnumDescription(Constants.ScreenNames.WasherStoppageData)))
            {
                new WasherStoppageData().ShowDialog();
            }
            else
            {
                GlobalMessageBox.Show(Messages.SCREEN_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Open Dryer Stoppage data screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDryerStoppageData_Click(object sender, EventArgs e)
        {
            if (GetFormsPermissionsForLoggedInUser(GetEnumDescription(Constants.ScreenNames.DryerStoppageData)))
            {
                new DryerStoppageData().ShowDialog();
            }
            else
            {
                GlobalMessageBox.Show(Messages.SCREEN_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Open Production Line Maintenance screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProdLineMaintenance_Click(object sender, EventArgs e)
        {
            if (GetFormsPermissionsForLoggedInUser(GetEnumDescription(Constants.ScreenNames.ProductionLineMaintenance)))
            {
                new ProductionLineMaintenance(_loggedInUser).ShowDialog();
            }
            else
            {
                GlobalMessageBox.Show(Messages.SCREEN_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Open QC/Packing Group Master screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQCGroupMaster_Click(object sender, EventArgs e)
        {
            if (GetFormsPermissionsForLoggedInUser(GetEnumDescription(Constants.ScreenNames.QCGroupMaster)))
            {
                new QCGroupMaster(_loggedInUser).ShowDialog();
            }
            else
            {
                GlobalMessageBox.Show(Messages.SCREEN_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Open QC/Packing Group Stoppage data screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQCGroupStoppageData_Click(object sender, EventArgs e)
        {
            if (GetFormsPermissionsForLoggedInUser(GetEnumDescription(Constants.ScreenNames.QCStoppageData)))
            {
                new AddQCGroupStoppageData().ShowDialog();
            }
            else
            {
                GlobalMessageBox.Show(Messages.SCREEN_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Open QAI Defect Master screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQAIDefectMaster_Click(object sender, EventArgs e)
        {
            if (GetFormsPermissionsForLoggedInUser(GetEnumDescription(Constants.ScreenNames.QAIDefectMaster)))
            {
                new QAIDefectMaster(_loggedInUser).ShowDialog();
            }
            else
            {
                GlobalMessageBox.Show(Messages.SCREEN_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Open Master Data Maintenance screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTableMaintenance_Click(object sender, EventArgs e)
        {
            new SetUpConfiguration.MasterTableMainMenu(_loggedInUser).ShowDialog();
        }
        /// <summary>
        /// Open Production Logging Activity screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProductionLoggingActivity_Click(object sender, EventArgs e)
        {
            new ProductionLogging.MainMenu(GetEnumDescription(Constants.ScreenNames.ProductionLogging)).ShowDialog();             
        }
        #endregion      

        private void btnEditQcEfficiency_Click(object sender, EventArgs e)
        {
            if (GetFormsPermissionsForLoggedInUser(GetEnumDescription(Constants.ScreenNames.EditQCEfficiency)))
            {
                new QCPackingYieldSystem.MainMenu(_loggedInUser, "admin").ShowDialog();
            }
            else
            {
                GlobalMessageBox.Show(Messages.SCREEN_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }

        private void btnLineClearanceAuthoriseSetup_Click(object sender, EventArgs e)
        {
            //if (GetFormsPermissionsForLoggedInUser(GetEnumDescription(Constants.ScreenNames.QCGroupMaster)))
            //{
            new LineClearanceAuthoriseSetup().ShowDialog();
            //}
            //else
            //{
            //    GlobalMessageBox.Show(Messages.SCREEN_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            //}
        }
    }
}
