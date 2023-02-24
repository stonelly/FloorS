using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Windows.UI.Tumbling;

namespace Hartalega.FloorSystem.Windows.UI.SecurityModule
{
    public partial class AddEditPageMaster : FormBase
    {
        private static string _screenName = "AddEditPageMaster - frmAddEditPageMaster";
        private static string _className = "frmAddEditPageMaster";
        private static string _addEdit = "ADD";
        private DataGridViewRow _objSelectedGridRow = null;
        private int _moduleScreenMappingId = 0;

        public event EventHandler OnScreenDataInsertUpdate;

        /// <summary>
        /// 
        /// </summary>
        public AddEditPageMaster()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbModule, "Module", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbScreenName, "Screen Name", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbPermissionOperator, "Permission Operator", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbPermission, "Permission", ValidationType.Required));

            if (ValidateForm())
            {
                int moduleId = Convert.ToInt32(cmbModule.SelectedValue);
                int screenId = Convert.ToInt32(cmbScreenName.SelectedValue);

                string permissionOper = cmbPermissionOperator.SelectedItem.ToString();
                int permissionId = Convert.ToInt32(cmbPermission.SelectedValue);
                string workStationConfig = WorkStationDTO.GetInstance().WorkStationId;

                if (string.Compare(_addEdit, "EDIT", true) == 0)
                {
                    SecurityModuleBLL.UpdateModuleScreenPermissionMapping(_moduleScreenMappingId, moduleId, screenId, permissionOper, permissionId, workStationConfig);
                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                }
                else if (string.Compare(_addEdit, "ADD", true) == 0)
                {
                    if (SecurityModuleBLL.IsModuleScreenMappingExist(moduleId, screenId))
                    {
                        GlobalMessageBox.Show(Messages.MODULE_PERMISSION_COMBINATION_ALREADY_EXIST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        ClearPageSelection();
                    }
                    else
                    {
                        SecurityModuleBLL.InsertModuleScreenPermissionMapping(moduleId, screenId, permissionOper, permissionId, workStationConfig);
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                }
                BacktoParentPage();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearPageSelection()
        {
            cmbModule.SelectedIndexChanged -= cmbModule_SelectedIndexChanged;
            BindModuleList();
            cmbModule.SelectedIndexChanged += cmbModule_SelectedIndexChanged;
            cmbScreenName.DataSource = null;
            BindPermissionList();
            cmbPermissionOperator.SelectedIndex = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string dialogResult = GlobalMessageBox.Show(Messages.LEAVE_SECURITY_PAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (string.Compare(dialogResult, Constants.YES)==0) 
            {
                BacktoParentPage();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BacktoParentPage()
        {
            if (OnScreenDataInsertUpdate != null)
                OnScreenDataInsertUpdate(null, null);
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEditPageMaster"></param>
        /// <param name="addEdit"></param>
        internal void LoadOrders(DataGridViewRow objEditPageMaster, string addEdit)
        {
            _addEdit = addEdit;
            if (objEditPageMaster.Cells.Count > 0 && (string.Compare(_addEdit, "EDIT", true) == 0))
            {
                _objSelectedGridRow = objEditPageMaster;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEditPageMaster_Load(object sender, EventArgs e)
        {
            BindModuleList();
            cmbModule.SelectedIndexChanged += cmbModule_SelectedIndexChanged;
            BindPermissionList();

            if (string.Compare(_addEdit, "EDIT", true) == 0)
            {
                _moduleScreenMappingId = Convert.ToInt32(_objSelectedGridRow.Cells["ModuleScreenMappingId"].Value.ToString());
                BindScreenList(Convert.ToInt32(_objSelectedGridRow.Cells["ModuleId"].Value.ToString()));
                cmbModule.SelectedValue = _objSelectedGridRow.Cells["ModuleId"].Value.ToString();
                cmbScreenName.SelectedValue = _objSelectedGridRow.Cells["ScreenId"].Value.ToString();
                cmbPermission.SelectedValue = _objSelectedGridRow.Cells["PermissionId"].Value.ToString();
                cmbPermissionOperator.SelectedItem = _objSelectedGridRow.Cells["PermissionOperator"].Value.ToString();
                cmbModule.Enabled = false;
                cmbScreenName.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            int moduleId = Convert.ToInt32(cmbModule.SelectedValue.ToString());
            BindScreenList(moduleId);
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindModuleList()
        {
            try
            {
                cmbModule.BindComboBox(SecurityModuleBLL.GetModuleMasterData(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindModuleList", null);
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorException"></param>
        /// <param name="screenName"></param>
        /// <param name="UiClassName"></param>
        /// <param name="uiControl"></param>
        /// <param name="parameters"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleId"></param>
        private void BindScreenList(int moduleId)
        {
            try
            {
                cmbScreenName.BindComboBox(SecurityModuleBLL.GetScreenMasterByModuleId(moduleId), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindScreenList", null);
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindPermissionList()
        {
            try
            {
                cmbPermission.BindComboBox(SecurityModuleBLL.GetPermissionMaster(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindPermissionList", null);
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEditPageMaster_Leave(object sender, EventArgs e)
        {
            this.MdiParent.Text = Messages.SCREEN_MASTER_PAGE;
        }
    }
}
