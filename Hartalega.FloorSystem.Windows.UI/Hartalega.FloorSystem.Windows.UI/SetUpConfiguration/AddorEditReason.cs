using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Add or Edit Reason
    /// File Type: Code file
    /// </summary>
    public partial class AddorEditReason : FormBase
    {
        #region Member Variables
        private int _reasonTypeId;
        private string _moduleId;
        private int _reasonTextId;
        private string _control;
        private string _screenName = "Configuration SetUp - AddorEditReason";
        private string _className = "AddorEditReason";
        private string _loggedInUser;
        private string _reasonText;
        #endregion

        #region Parameterised Constructors
        /// <summary>
        /// Constructor initialising values retrieved from main form for Add Reason screen
        /// </summary>
        /// <param name="ReasonType"></param>
        /// <param name="ReasonTypeId"></param>
        /// <param name="ModuleId"></param>
        /// <param name="Control"></param>
        /// <returns></returns>
        public AddorEditReason(string ReasonType, int ReasonTypeId, string ModuleId, string loggedInUser, string Control)
        {
            InitializeComponent();
            txtReasonType.Text = ReasonType.Trim();
            _reasonTypeId = ReasonTypeId;
            _moduleId = ModuleId;
            _control = Control;
            this.Text = Constants.ADD_REASON_SCREEN;
            gbAddReason.Text = Constants.ADD_REASON_SCREEN;
            _loggedInUser = loggedInUser;
        }

        /// <summary>
        /// Constructor initialising values retrieved from main form for Edit Reason screen
        /// </summary>
        /// <param name="ReasonType"></param>
        /// <param name="ReasonTypeId"></param>
        /// <param name="ReasonTextId"></param>
        /// <param name="ReasonText"></param>
        /// <param name="IsScheduled"></param>
        /// <param name="ModuleId"></param>
        /// <param name="Control"></param>
        /// <returns></returns>
        public AddorEditReason(string ReasonType, int ReasonTypeId, int ReasonTextId, string ReasonText, bool IsScheduled,
            string ModuleId, string loggedInUser, string Control, string shortCode = null)
        {
            InitializeComponent();
            txtReasonType.Text = ReasonType.Trim();
            txtReasonText.Text = ReasonText;
            if (!string.IsNullOrEmpty(shortCode))
            {
                txtShortCode.Text = Convert.ToString(shortCode);
            }
            chkSchedule.Checked = IsScheduled;
            _reasonTypeId = ReasonTypeId;
            _reasonTextId = ReasonTextId;
            _moduleId = ModuleId;
            _control = Control;
            this.Text = Constants.EDIT_REASON_SCREEN;
            gbAddReason.Text = Constants.EDIT_REASON_SCREEN;
            _loggedInUser = loggedInUser;
            _reasonText = ReasonText;
        }
        #endregion

        #region User Methods

        /// <summary>
        /// Validation of Reason text field
        /// </summary>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtReasonText, "Reason Text", ValidationType.Required));
            return ValidateForm();
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

        /// <summary>
        /// Clear the values of all the controls 
        /// </summary>
        private void ClearForm()
        {
            txtReasonText.Text = string.Empty;
            chkSchedule.Checked = false;
            txtReasonText.Focus();
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
            int rowsReturned = 0;
            bool isDuplicate = false;

            if (ValidateRequiredFields())
            {
                ReasonDTO objReasonText = new ReasonDTO();
                try
                {
                    objReasonText.ReasonText = txtReasonText.Text.Trim();
                    objReasonText.ReasonTypeId = _reasonTypeId;
                    objReasonText.IsScheduled = chkSchedule.Checked;
                    objReasonText.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                    objReasonText.ShortCode = txtShortCode.Text.Trim();
                    objReasonText.OperatorId = _loggedInUser;

                    if (_control == Constants.ADD_CONTROL)
                    {
                        isDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsReasonTextDuplicate(objReasonText.ReasonText,
                                                            objReasonText.ReasonTypeId));
                    }
                    else
                    {
                        if (!txtReasonText.Text.Trim().Equals(_reasonText))
                            isDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsReasonTextDuplicate(objReasonText.ReasonText,
                                                                objReasonText.ReasonTypeId));
                    }
                    if (!isDuplicate)
                    {

                        if (_control == Constants.EDIT_CONTROL)
                        {
                            objReasonText.ReasonTextId = _reasonTextId;
                            rowsReturned = SetUpConfigurationBLL.EditReasonText(objReasonText);
                        }
                        else
                        {
                            rowsReturned = SetUpConfigurationBLL.SaveReasonText(objReasonText);
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
                    else
                    {
                        GlobalMessageBox.Show(Messages.DUPLICATE_REASON_TEXT, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        txtReasonText.Text = string.Empty;
                        txtReasonText.Focus();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                    
                }
            }

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
        private void chkSchedule_Paint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Focused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
            }
        }

        /// <summary>
        /// Event Handler for form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddReason_Load(object sender, EventArgs e)
        {
            lblShortCode.Visible = false;
            txtShortCode.Visible = false;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddReason_Load", null);
                return;
            }
            if (_moduleId == Convert.ToString(Convert.ToInt16(Constants.Modules.WASHER)) ||
                         _moduleId == Convert.ToString(Convert.ToInt16(Constants.Modules.DRYER)) ||
                         _moduleId == Convert.ToString(Convert.ToInt16(Constants.Modules.PRODUCTION)) ||
            txtReasonType.Text.Trim() == Constants.REASON_PL_STOP.Trim())
            {
                lblSchedule.Visible = true;
                chkSchedule.Visible = true;
            }
            if (txtReasonType.Text.Trim() == Constants.REASON_PL_STOP.Trim())
            {
                lblShortCode.Visible = true;
                txtShortCode.Visible = true;
            }
        }
        #endregion
    }
}
