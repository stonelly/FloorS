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
    public partial class EditEmployeeDetails : FormBase
    {
        private DataGridViewRow _objSelectedGridRow = null;
        private int _eId = 0;
        private string _oldEmployeeId = string.Empty;
        private string _oldPassword = string.Empty;
        public event EventHandler OnEmployeeDataInsertUpdate;

        /// <summary>
        /// 
        /// </summary>
        public EditEmployeeDetails()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEditPageMaster"></param>
        internal void LoadEditEmployeePage(DataGridViewRow objEditPageMaster)
        {
            _oldEmployeeId = string.Empty;
            _objSelectedGridRow = objEditPageMaster;
            _eId = Convert.ToInt32(_objSelectedGridRow.Cells["Id"].Value.ToString());
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
            cmbEmployeeRole.BindComboBox(SecurityModuleBLL.GetRoleMasterData(), true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditEmployeeDetails_Load(object sender, EventArgs e)
        {
            txtEmployeeId.TextBoxLength(6);
            txtEmpIdSearch.TextBoxLength(6);
            txtPassword.TextBoxLength(6);

            BindRoleList();

            txtEmployeeId.Text = _objSelectedGridRow.Cells["EmployeeId"].Value.ToString();
            _oldEmployeeId = _objSelectedGridRow.Cells["EmployeeId"].Value.ToString();
            txtEmployeeName.Text = _objSelectedGridRow.Cells["EmployeeName"].Value.ToString();
            cmbEmployeeRole.SelectedValue = _objSelectedGridRow.Cells["RoleId"].Value.ToString();
           
            string pwd = _objSelectedGridRow.Cells["Password"].Value.ToString();
            _oldPassword = _objSelectedGridRow.Cells["Password"].Value.ToString();
            if (pwd.Length == 1)
            {
                txtPassword.Text = pwd.ToString().PadLeft(pwd.Length + 5, '0');
            }
            else if (pwd.Length == 2)
            {
                txtPassword.Text = pwd.ToString().PadLeft(pwd.Length + 4, '0');
            }
            else if (pwd.Length == 3)
            {
                txtPassword.Text = pwd.ToString().PadLeft(pwd.Length + 3, '0');
            }
            else if (pwd.Length == 4)
            {
                txtPassword.Text = pwd.ToString().PadLeft(pwd.Length + 2, '0');
            }
            else if (pwd.Length == 5)
            {
                txtPassword.Text = pwd.ToString().PadLeft(pwd.Length +1, '0');
            }
            else
            {
                txtPassword.Text = pwd;
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
            validationMesssageLst.Add(new ValidationMessage(txtEmployeeId  , "Employee ID.", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtEmployeeName , "Employee Name.", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbEmployeeRole , "Role.", ValidationType.Required));

            if (ValidateForm())
            {
                string empId = txtEmployeeId.Text;
                string passwordCheck = txtPassword.Text;

                Boolean isUpdate = false;
                if (string.Compare(empId, _oldEmployeeId, true) == 0)
                {
                    isUpdate = true;
                }                
                else 
                {
                    if (SecurityModuleBLL.IsEmployeeAlreadyExist(empId))
                    {
                        GlobalMessageBox.Show(Messages.EMPLOYEE_ID_ALREADY_EXIST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }                   
                    else
                    {
                        isUpdate = true;
                    }
                }
                if (isUpdate)
                {
                    if (string.Compare(passwordCheck, _oldPassword, true) == 0)
                    {
                        isUpdate = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(passwordCheck) && SecurityModuleBLL.IsPasswordAlreadyExist(passwordCheck))
                        {
                            GlobalMessageBox.Show(Messages.PASSWORD_ALREADY_EXIST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtPassword.Focus();
                            isUpdate = false;
                        }
                        else
                        {
                            isUpdate = true;
                        }
                    }
                }
                if (isUpdate)
                {
                    string empName = txtEmployeeName.Text;
                    int roleId = Convert.ToInt32(cmbEmployeeRole.SelectedValue.ToString());

                    string password = string.Empty;
                    if (!(string.IsNullOrEmpty(txtPassword.Text)))
                    {
                        password = txtPassword.Text.PadLeft(6,'0');
                    }

                    string workStationConfig = WorkStationDTO.GetInstance().WorkStationId; //"HDC2-L-H2DH102";

                    SecurityModuleBLL.UpdateEmployeeMasterData(_eId, empId, empName, password, roleId, workStationConfig);
                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

                    BacktoParentPage();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtEmpIdSearch  , "Enter Employee ID.", ValidationType.Required));

            if (ValidateForm())
            {
                int searchId = Convert.ToInt32(txtEmpIdSearch.Text);
                DataTable dt = SecurityModuleBLL.GetEmployeeMasterDataByEmpId(searchId);

                if (dt.Rows.Count > 0)
                {
                    BindRoleList();
                    _eId = Convert.ToInt32(dt.Rows[0]["Id"].ToString());
                    txtEmployeeId.Text = dt.Rows[0]["EmployeeId"].ToString();
                    _oldEmployeeId = dt.Rows[0]["EmployeeId"].ToString();
                    txtEmployeeName.Text = dt.Rows[0]["EmployeeName"].ToString();
                    cmbEmployeeRole.SelectedValue = dt.Rows[0]["RoleId"].ToString();

                    string pwd = dt.Rows[0]["Password"].ToString();
                    _oldPassword = dt.Rows[0]["Password"].ToString();
                    if (pwd.Length == 1)
                    {
                        txtPassword.Text = pwd.ToString().PadLeft(pwd.Length + 5, '0');
                    }
                    else if (pwd.Length == 2)
                    {
                        txtPassword.Text = pwd.ToString().PadLeft(pwd.Length + 4, '0');
                    }
                    else if (pwd.Length == 3)
                    {
                        txtPassword.Text = pwd.ToString().PadLeft(pwd.Length + 3, '0');
                    }
                    else if (pwd.Length == 4)
                    {
                        txtPassword.Text = pwd.ToString().PadLeft(pwd.Length + 2, '0');
                    }
                    else if (pwd.Length == 5)
                    {
                        txtPassword.Text = pwd.ToString().PadLeft(pwd.Length + 1, '0');
                    }
                    else
                    {
                        txtPassword.Text = pwd;
                    }
                }
                else {
                    GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                }
            }
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
        private void EditEmployeeDetails_Leave(object sender, EventArgs e)
        {
            this.MdiParent.Text = Messages.EMPLOYEE_MASTER_PAGE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEmployeeName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == 32);
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
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            string passwordCheck = txtPassword.Text;
            if (!btnCancel.Focused && txtPassword.Focused)
            {
                if (string.Compare(passwordCheck, _oldPassword, true) != 0)
                {
                    if (!string.IsNullOrEmpty(passwordCheck) && SecurityModuleBLL.IsPasswordAlreadyExist(passwordCheck))
                    {
                        GlobalMessageBox.Show(Messages.PASSWORD_ALREADY_EXIST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        txtPassword.Focus();
                    }
                }
            } 
            else if(btnCancel.Focused)
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
