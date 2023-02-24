using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
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
    public partial class AddEditPermissionMaster : FormBase
    {
       
        private static string _addEdit = "ADD";
        private DataGridViewRow _objSelectedGridRow = null;
        private int _permissionId = 0;
        private string _oldPermissionSeq = string.Empty;
        public event EventHandler OnPermissionDataInsertUpdate;

        /// <summary>
        /// 
        /// </summary>
        public AddEditPermissionMaster()
        {
            InitializeComponent();
        }

        internal void LoadOrders(DataGridViewRow objEditPermissionMaster, string addEdit)
        {
            _oldPermissionSeq = string.Empty;
            _addEdit = addEdit;
            if (objEditPermissionMaster.Cells.Count > 0 && (string.Compare(_addEdit, "EDIT", true) == 0))
            {
                _objSelectedGridRow = objEditPermissionMaster;
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
            validationMesssageLst.Add(new ValidationMessage(txtPermission, "Permission", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtPermissionDesc, "Permission Description", ValidationType.Required));

            if (ValidateForm())
            {
                int permissionSeq = Convert.ToInt32(txtPermission.Text);
                string permissionDescription = txtPermissionDesc.Text;
                string workStationConfig = WorkStationDTO.GetInstance().WorkStationId;  //"HDC2-L-H2DH102";

                if (string.Compare(_addEdit, "EDIT", true) == 0)
                {
                    Boolean isUpdate = false;
                    if (string.Compare(_oldPermissionSeq, permissionSeq.ToString(), true) == 0)
                    {
                        isUpdate = true;
                    }
                    else
                    {
                        if (SecurityModuleBLL.IsPermissionSequenceAlreadyExist(permissionSeq.ToString()))
                        {
                            GlobalMessageBox.Show(Messages.PERMISSION_SEQ_ALREADY_EXIST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            ClearFieldControls("EDIT");
                        }
                        else
                        {
                            isUpdate = true;
                        }
                    }
                    if (isUpdate)
                    {
                        SecurityModuleBLL.UpdatePageMasterDetails(_permissionId, permissionSeq, permissionDescription, workStationConfig);
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        BacktoParentPage();
                    }
                    
                }
                else if (string.Compare(_addEdit, "ADD", true) == 0)
                {
                    if (SecurityModuleBLL.IsPermissionSequenceAlreadyExist(permissionSeq.ToString()))
                    {
                        GlobalMessageBox.Show(Messages.PERMISSION_SEQ_ALREADY_EXIST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        ClearFieldControls("ADD");
                    }
                    else
                    {
                        SecurityModuleBLL.InsertPageMasterDetails(permissionSeq, permissionDescription, workStationConfig);
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        BacktoParentPage();
                    }
                    
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_addEdit"></param>
        private void ClearFieldControls(string _addEdit)
        {
            if (string.Compare(_addEdit, "ADD", true) == 0)
            {
                txtPermission.Text = string.Empty;
                txtPermissionDesc.Text = string.Empty;
                txtPermission.Focus();
            }
            else if (string.Compare(_addEdit, "EDIT", true) == 0)
            {
                int count = _objSelectedGridRow.Cells.Count;
                _permissionId = Convert.ToInt32(_objSelectedGridRow.Cells["PermissionId"].Value.ToString());
                txtPermission.Text = _objSelectedGridRow.Cells["Permission"].Value.ToString();
                _oldPermissionSeq = _objSelectedGridRow.Cells["Permission"].Value.ToString();
                txtPermissionDesc.Text = _objSelectedGridRow.Cells["Description"].Value.ToString();
                txtPermission.Focus();
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
        private void BacktoParentPage()
        {
            if (OnPermissionDataInsertUpdate != null)
                OnPermissionDataInsertUpdate(null, null);
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEditPermissionMaster_Load(object sender, EventArgs e)
        {
            txtPermission.TextBoxLength(3);

            if (string.Compare(_addEdit, "EDIT", true) == 0)
            {
                int count = _objSelectedGridRow.Cells.Count;
                _permissionId = Convert.ToInt32(_objSelectedGridRow.Cells["PermissionId"].Value.ToString());
                txtPermission.Text = _objSelectedGridRow.Cells["Permission"].Value.ToString();
                _oldPermissionSeq = _objSelectedGridRow.Cells["Permission"].Value.ToString();
                txtPermissionDesc.Text = _objSelectedGridRow.Cells["Description"].Value.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEditPermissionMaster_Leave(object sender, EventArgs e)
        {
            this.MdiParent.Text = Messages.PERMISSION_MASTER_MASTER_PAGE;
        }
    }
}
