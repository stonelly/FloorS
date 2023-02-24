using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{

    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Add QC/Packing Stoppage Data
    /// File Type: Code file
    /// </summary>
    public partial class AddQCGroupStoppageData : FormBase
    {
        #region Member Variables

        private string _screenName = "Add QC Group Stoppage Data";
        private string _className = "AddQCGroupStoppageData";
        private List<string> _validationMsgs = new List<string>();
        private DateTime _maxtime;
        //private int _QCGroupId;
        #endregion

        #region Constructor
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public AddQCGroupStoppageData()
        {
            InitializeComponent();
            _maxtime = CommonBLL.GetCurrentDateAndTimeFromServer();
            datepicker.Text = _maxtime.ToString();
            timepicker.Text = _maxtime.ToString();
            BindGroupType();
            BindReasons();
        }
        #endregion

        #region User Methods

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
            ClearForm();
        }

        /// <summary>
        /// Clear the values of all the controls 
        /// </summary>
        private void ClearForm()
        {
            txtOperatorId.Text = string.Empty;
            chkAllQCGroup.Checked = false;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ClearForm", null);
                return;
            }
            BindGroupType();
            BindReasons();
            cmbReason.SelectedIndex = Constants.MINUSONE;
            cmbQCGroup.SelectedIndex = Constants.MINUSONE;
            cmbGroupType.SelectedIndex = Constants.MINUSONE;
            txtName.Text = string.Empty;
            cmbGroupType.Focus();           
        }

        /// <summary>
        /// Validation of Required Fields
        /// </summary>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbGroupType, "Group Type", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbQCGroup, "QC/Packing Group", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtOperatorId, "Operator Id", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbReason, "Reason", ValidationType.Required));
            return ValidateForm();
        }

        /// <summary>
        /// Binds the Reasons ComboBox
        /// </summary>
        private void BindReasons()
        {
            try
            {
                cmbReason.BindComboBox(SetUpConfigurationBLL.GetReasonsDetails(Constants.QCGROUP_STOPPAGE_REASON), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindReasons", null);
                return;
            }
        }

        /// <summary>
        /// Binds the Group Type ComboBox
        /// </summary>
        private void BindGroupType()
        {
            try
            {
                cmbGroupType.BindComboBox(CommonBLL.GetEnumText(Constants.QC_GROUP_TYPE), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindGroupType", null);
                return;
            }
        }

        /// <summary>
        /// Binds the QC/Packing Group Name ComboBox
        /// </summary>
        private void BindGroupName()
        {
            try
            {
                cmbQCGroup.BindComboBox(SetUpConfigurationBLL.GetQCGroupsData(cmbGroupType.SelectedValue.ToString()), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindGroupName", null);
                return;
            }  
        }

         /// <summary>
        /// Validates and Saves the data on click of Save
        /// </summary>
        private void SaveData()
        {
            int rowsReturned = 0;

            if (ValidateRequiredFields())
            {
                QCGroupStoppageDTO objQCGroupStoppage = new QCGroupStoppageDTO();
                try
                {

                    if (chkAllQCGroup.Checked)
                    {
                        objQCGroupStoppage.QCGroupId = Constants.ZERO;
                    }
                    else
                    {
                        objQCGroupStoppage.QCGroupId = Convert.ToInt16(cmbQCGroup.SelectedValue);
                    }

                    objQCGroupStoppage.OperatorId = txtOperatorId.Text.Trim();
                    //objQCGroupStoppage.StoppageDate = CommonBLL.GetCurrentDateAndTime(); // issues-102 , changed by Ajay Varma, reference to mail from Hartalega.
                    objQCGroupStoppage.StoppageDate = Convert.ToDateTime(datepicker.Value.ToShortDateString() + " " + timepicker.Text.Trim());
                    objQCGroupStoppage.ReasonId = Convert.ToInt16(cmbReason.SelectedValue);
                    objQCGroupStoppage.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                    if (objQCGroupStoppage.StoppageDate > _maxtime)
                    {
                        GlobalMessageBox.Show(Messages.FUTURE_DATE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        return;
                    }

                    rowsReturned = SetUpConfigurationBLL.SaveQCGroupStoppageData(objQCGroupStoppage);
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                    return;
                }

                if (rowsReturned > 0)
                {
                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    ClearForm();
                }
                else
                {
                    GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                }
            }
        }
        #endregion


        #region Event Handlers

        /// <summary>
        /// Event Handler for Save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        /// <summary>
        /// Event Handler for Cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CANCEL_SCONFIG, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
            }
        }

        /// <summary>
        /// Event Handler for setting focus on checkbox on Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAllQCGroup_Paint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Focused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
            }
        }

        /// <summary>
        /// Event Handler for page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddQCGroupStoppageData_Load(object sender, EventArgs e)
        {           
            try
            {
                txtOperatorId.OperatorId();
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddQCGroupStoppageData_Load", null);
                return;
            }
        }

        /// <summary>
        /// Event Handler for Selected index changed event of Group Type combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGroupType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbGroupType.SelectedIndex != Constants.MINUSONE)
                {
                    BindGroupName();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "cmbGroupType_SelectedIndexChanged", null);
                return;
            }
        }

        /// <summary>
        /// Event Handler for operator Id textbox leave event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperatorId_Leave(object sender, EventArgs e)
        {
            string operatorName = String.Empty;
            if (!btnCancel.Focused && txtOperatorId.Focused && !btnSave.Focused)
            {
                if (string.IsNullOrEmpty(txtOperatorId.Text.Trim()))
                {
                    txtName.Text = string.Empty;
                    validationMesssageLst = new List<ValidationMessage>();
                    validationMesssageLst.Add(new ValidationMessage(txtOperatorId, "Operator Id", ValidationType.Required));
                    ValidateForm();
                }
                else
                {
                    try
                    {
                        if (SecurityModuleBLL.ValidateOperatorAccess(txtOperatorId.Text.Trim(), _screenName))
                        {
                            operatorName = CommonBLL.GetOperatorNameQAI(txtOperatorId.Text.Trim());
                            if (!string.IsNullOrEmpty(operatorName) & operatorName != Constants.INVALIDMESSAGE)
                            {
                                txtName.Text = operatorName;
                            }
                            else
                            {
                                txtOperatorId.Text = String.Empty;
                                txtName.Text = string.Empty;
                                GlobalMessageBox.Show(Messages.INVALID_OPERATOR_ID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                txtOperatorId.Focus();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.OPERATOR_SCREEN_ACCESS_MSG, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                            txtOperatorId.Text = String.Empty;
                            txtName.Text = String.Empty;
                            txtOperatorId.Focus();
                        }

                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "txtOperatorId_Leave", txtOperatorId.Text);
                        return;
                    }
                }
            }
            else if (btnCancel.Focused)
            {
                if (GlobalMessageBox.Show(Messages.CANCEL_SCONFIG, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    ClearForm();
                }
            }
            else if (btnSave.Focused)
            {
                SaveData();
            }
        }
        #endregion
    }
}
