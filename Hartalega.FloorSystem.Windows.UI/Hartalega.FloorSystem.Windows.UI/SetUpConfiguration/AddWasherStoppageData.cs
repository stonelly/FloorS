using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Add Washer Stoppage Data
    /// File Type: Code file
    /// </summary>
    public partial class AddWasherStoppageData : FormBase
    {
        #region Member Variables

        private string _screenName = "Add Washer Stoppage Data";
        private string _className = "AddWasherStoppageData";
        private List<string> _validationMsgs = new List<string>();
        private int _washerId;
        private string _control;
        private int _washerStoppageId;
        private DateTime _maxtime;
        #endregion

        #region Constructor
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public AddWasherStoppageData(int washerId, int washerNumber,WasherStoppageDTO washerStoppageDTO)
        {
            InitializeComponent();
            _maxtime = CommonBLL.GetCurrentDateAndTimeFromServer();
            try
            {
                BindReasons();
                datepicker.Format = DateTimePickerFormat.Custom;
                datepicker.CustomFormat = Constants.CUSTOM_DATE_FORMAT;               
                if (washerStoppageDTO != null)
                {
                    txtWasherNumber.Text = washerStoppageDTO.WasherNumber.ToString();
                    txtOperatorId.Text = washerStoppageDTO.OperatorId;
                    txtName.Text = washerStoppageDTO.OperatorName;
                    datepicker.Text = washerStoppageDTO.StoppageDate.ToString();
                    timepicker.Text = washerStoppageDTO.StoppageDate.ToString();
                    cmbReason.SelectedValue = washerStoppageDTO.ReasonId.ToString();
                    _washerStoppageId = washerStoppageDTO.Id;
                    chkAllWasher.Enabled = false;
                    _control = Constants.EDIT_CONTROL;
                    _washerId = washerStoppageDTO.WasherId;
                }
                else
                {                   
                    _washerId = washerId;
                    txtWasherNumber.Text = Convert.ToString(washerNumber);
                    _control = Constants.ADD_CONTROL;
                    _washerStoppageId = Constants.ZERO;
                    datepicker.Text = _maxtime.ToString();
                    timepicker.Text = _maxtime.ToString();
                }

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddWasherStoppageData_Load", null);
                return;
            }
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
            chkAllWasher.Checked = false;
            cmbReason.SelectedIndex = Constants.MINUSONE;
            txtOperatorId.Focus();
        }

        /// <summary>
        /// Validation of Required Fields
        /// </summary>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtOperatorId, "Operator Id", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbReason, "Reason", ValidationType.Required));
            return ValidateForm();
        }

        /// <summary>
        /// Binds the Reasons ComboBox
        /// </summary>
        private void BindReasons()
        {
            if (cmbReason.Items.Count == Constants.ZERO)
            {
                try
                {
                    cmbReason.BindComboBox(SetUpConfigurationBLL.GetReasonsDetails(Constants.WASHER_STOPPAGE_REASON), true);
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "BindReasons", null);
                    return;
                }
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
                WasherStoppageDTO objWasherStoppage = new WasherStoppageDTO();
                try
                {
                    if(string.Compare(_control,Constants.EDIT_CONTROL) == Constants.ZERO)
                    {
                        objWasherStoppage.Id = _washerStoppageId;
                    }
                    if (chkAllWasher.Checked)
                    {
                        objWasherStoppage.WasherId = Constants.ZERO;
                    }
                    else
                    {
                        objWasherStoppage.WasherId = _washerId;
                    }
                    objWasherStoppage.WasherNumber = Convert.ToInt16(txtWasherNumber.Text.Trim());
                    objWasherStoppage.OperatorId = txtOperatorId.Text.Trim();
                    objWasherStoppage.ReasonId = Convert.ToInt16(cmbReason.SelectedValue);
                    objWasherStoppage.StoppageDate =Convert.ToDateTime(datepicker.Value.ToShortDateString()+" "+timepicker.Text.Trim());

                    if (objWasherStoppage.StoppageDate > _maxtime)
                    {
                        GlobalMessageBox.Show(Messages.FUTURE_DATE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        return;
                    }
                    objWasherStoppage.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;

                    int isWasherInUse = SetUpConfigurationBLL.UpdateWasherBatchEndTime(objWasherStoppage.WasherId);

                    if (isWasherInUse >= Constants.ONE)
                    {
                        GlobalMessageBox.Show(Messages.WASHER_BATCH_UPDATE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }

                    rowsReturned = SetUpConfigurationBLL.SaveWasherStoppageData(objWasherStoppage);
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                    return;
                }

                if (rowsReturned > 0)
                {
                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    this.Close();
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
            if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Event Handler for setting focus on checkbox on Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAllWasher_Paint(object sender, PaintEventArgs e)
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
        private void AddWasherStoppageData_Load(object sender, EventArgs e)
        {
            txtOperatorId.OperatorId();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddWasherStoppageData_Load", null);
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

                            if (!string.IsNullOrEmpty(operatorName) && operatorName != Constants.INVALIDMESSAGE)
                            {
                                txtName.Text = operatorName;
                            }
                            else
                            {
                                txtOperatorId.Text = string.Empty;
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
                if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    this.Close();
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
