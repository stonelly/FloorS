using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SecurityModule
{
    public partial class AddEditRoleModulePermission : FormBase
    {
        private static string _screenName = "AddEditRoleModulePermission - frmAddEditRoleModulePermission";
        private static string _className = "frmAddEditRoleModulePermission";
        private static string _addEdit = "ADD";
        private DataGridViewRow _objSelectedGridRow = null;
        private int _roleId = 0;
        private string _oldRole = string.Empty;
        public event EventHandler OnRoleDataInsertUpdate;

        /// <summary>
        /// 
        /// </summary>
        public AddEditRoleModulePermission()
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
            validationMesssageLst.Add(new ValidationMessage(txtRoleName, "Role Name", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtRoleDescription, "Role Description", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbPermission, "Permission", ValidationType.Required));

            if (!(cblModule.CheckedItems.Count > 0))
            {
                validationMesssageLst.Add(new ValidationMessage(cblModule, "Module", ValidationType.Custom));
            }

            if (ValidateForm())
            {
                string role = txtRoleName.Text;
                string roleDescription = txtRoleDescription.Text;
                int permissionId = Convert.ToInt32(cmbPermission.SelectedValue);
                string workStationConfig = WorkStationDTO.GetInstance().WorkStationId; //"HDC2-L-H2DH102";
                StringBuilder objStringBuilder = new StringBuilder();
                string moduleIds = string.Empty;

                foreach (object itemChecked in cblModule.CheckedItems)
                {
                    DataRowView castedItem = itemChecked as DataRowView;
                    objStringBuilder.Append("," + (castedItem["ModuleId"].ToString()));
                }

                if (objStringBuilder.Length > 0)
                    moduleIds = objStringBuilder.ToString().Substring(1);

                if (string.Compare(_addEdit, "EDIT", true) == 0)
                {
                    Boolean isUpdate = false;
                    if (string.Compare(_oldRole, role, true) == 0)
                    {
                        isUpdate = true;
                    }
                    else
                    {
                        if (SecurityModuleBLL.IsRoleAlreadyExist(role))
                        {
                            GlobalMessageBox.Show(Messages.ROLENAME_ALREADY_EXIST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        }
                        else
                        {
                            isUpdate = true;
                        }
                    }
                    if (isUpdate)
                    {
                        SecurityModuleBLL.UpdateRoleMaintenanceData(_roleId, role, roleDescription, permissionId, workStationConfig, moduleIds);
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        BacktoParentPage();
                    }
                }
                else if (string.Compare(_addEdit, "ADD", true) == 0)
                {
                    if (SecurityModuleBLL.IsRoleAlreadyExist(role))
                    {
                        GlobalMessageBox.Show(Messages.ROLENAME_ALREADY_EXIST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                    else
                    {
                        SecurityModuleBLL.InsertRoleMaintenanceData(role, roleDescription, permissionId, workStationConfig, moduleIds);
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        BacktoParentPage();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string dialogResult = GlobalMessageBox.Show(Messages.LEAVE_SECURITY_PAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (string.Compare(dialogResult, Constants.YES) == 0) 
            {
                BacktoParentPage();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEditPageMaster"></param>
        /// <param name="addEdit"></param>
        internal void LoadAddEditRoleMaster(DataGridViewRow objEditPageMaster, string addEdit)
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
        private void BacktoParentPage()
        {
            if (OnRoleDataInsertUpdate != null)
                OnRoleDataInsertUpdate(null, null);
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindModuleCheckBoxList()
        {
            cblModule.DataSource = SecurityModuleBLL.GetModuleMasterDetails();
            cblModule.DisplayMember = "ModuleName";
            cblModule.ValueMember = "ModuleId";
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
        private void AddEditRoleModulePermission_Load(object sender, EventArgs e)
        {
            _oldRole = string.Empty;
            BindPermissionList();
            BindModuleCheckBoxList();

            if (string.Compare(_addEdit, "EDIT", true) == 0)
            {
                _roleId = Convert.ToInt32(_objSelectedGridRow.Cells["RoleId"].Value.ToString());
                txtRoleName.Text = _objSelectedGridRow.Cells["RoleName"].Value.ToString();
                _oldRole = _objSelectedGridRow.Cells["RoleName"].Value.ToString();
                txtRoleDescription.Text = _objSelectedGridRow.Cells["RoleDescription"].Value.ToString();
                cmbPermission.SelectedValue = _objSelectedGridRow.Cells["PermissionId"].Value.ToString();
                string moduleIds = _objSelectedGridRow.Cells["ModuleIds"].Value.ToString();

                char[] delimiter = { ',' };
                string[] k = moduleIds.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                foreach (string m in k)
                {
                    int tempModuleId = Convert.ToInt32(m);
                    int value = 0;
                    for (int i = 0; i < cblModule.Items.Count; i++)
                    {
                        DataRowView view = cblModule.Items[i] as DataRowView;
                        value = (int)view["ModuleId"];
                        if (tempModuleId == value)
                            cblModule.SetItemChecked(i, true);
                    }
                }
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEditRoleModulePermission_Leave(object sender, EventArgs e)
        {
            this.MdiParent.Text = Messages.ROLE_MASTER_PAGE;
        }

    }
}
