using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
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
    public partial class AddEmployee : FormBase 
    {
        private static string _screenName = "AddEmployee - frmAddEmployee";
        private static string _className = "frmAddEmployee";
        public event EventHandler OnEmployeeDataInsertUpdate;

        /// <summary>
        /// 
        /// </summary>
        public AddEmployee()
        {
            InitializeComponent();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtEmployeeId , "Employee ID.", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtEmployeeName , "Employee Name.", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbEmployeeRole , "Role.", ValidationType.Required));
            
            if (ValidateForm())
            {
                string empId = txtEmployeeId.Text;
                if (SecurityModuleBLL.IsEmployeeAlreadyExist(empId))
                {
                    GlobalMessageBox.Show(Messages.EMPLOYEE_ID_ALREADY_EXIST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    txtEmployeeId.Focus();
                }
                else if (!string.IsNullOrEmpty(txtPassword.Text) && SecurityModuleBLL.IsPasswordAlreadyExist(txtPassword.Text))
                {
                    GlobalMessageBox.Show(Messages.PASSWORD_ALREADY_EXIST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    txtPassword.Focus();
                }
                else 
                {
                    string empName = txtEmployeeName.Text;
                    int roleId = Convert.ToInt32(cmbEmployeeRole.SelectedValue.ToString());

                    string password = string.Empty;
                    if (!(string.IsNullOrEmpty(txtPassword.Text)))
                    {
                        password = txtPassword.Text.PadLeft(6, '0');
                    }

                    string workStationConfig = WorkStationDTO.GetInstance().WorkStationId; 

                    SecurityModuleBLL.InsertEmployeeMasterData(empId, empName, password, roleId, workStationConfig);

                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY_SM, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    BacktoParentPage();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BacktoParentPage()
        {
            if (OnEmployeeDataInsertUpdate != null)
                OnEmployeeDataInsertUpdate(null, null);
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindRoleList()
        {
            try
            {
                cmbEmployeeRole.BindComboBox(SecurityModuleBLL.GetRoleMasterData(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddEmployee", null);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGeneratePIN_Click(object sender, EventArgs e)
        {
            txtPassword.Text = SecurityModuleBLL.GenerateRandomPassword().ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEmployee_Load(object sender, EventArgs e)
        {
            MainMenu objMain = new MainMenu();
            txtEmployeeId.TextBoxLength(6);
            txtPassword.TextBoxLength(6);
            txtEmployeeId.Focus();
            BindRoleList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEmployeeName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar==32);
            if (e.Handled)
            {
                GlobalMessageBox.Show(Messages.INVALIDDATA, Constants.AlertType.Warning, Messages.INVALIDDATA, GlobalMessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEmployee_Leave(object sender, EventArgs e)
        {
            this.MdiParent.Text = Messages.EMPLOYEE_MASTER_PAGE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (!btnCancel.Focused && txtPassword.Focused)
            {
                if (!string.IsNullOrEmpty(txtPassword.Text) && SecurityModuleBLL.IsPasswordAlreadyExist(txtPassword.Text))
                {
                    GlobalMessageBox.Show(Messages.PASSWORD_ALREADY_EXIST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    txtPassword.Focus();
                }
            }
            else if (btnCancel.Focused)
            {
                string dialogResult = GlobalMessageBox.Show(Messages.LEAVE_SECURITY_PAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
                if (string.Compare(dialogResult, Constants.YES) == 0) 
                {
                    BacktoParentPage();
                }
            }
        }
    }
}
