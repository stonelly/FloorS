using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name:Edit Washer Details
    /// File Type: Code file
    /// </summary> 
    public partial class EditWasherDetails : FormBase
    {
        #region Member Variables
        private int _washerId;
        private string _gloveType = string.Empty;
        private string _gloveSize = string.Empty;
        private string _screenName = "Configuration SetUp - EditWasherDetails";
        private string _className = "EditWasherDetails";
        private string _loggedInUser;
        private WasherDTO oldwasherDTO = null;
        #endregion

        #region Parameterised Constructors
        /// <summary>
        /// Constructor initialising values retrieved from main form 
        /// </summary>
        /// <param name="WasherDTO washerDTO"></param>
        /// <returns></returns>
        public EditWasherDetails(WasherDTO washerDTO)
        {
            InitializeComponent();
            if (washerDTO != null)
            {
                _washerId = washerDTO.Id;
                txtWasherNo.Text = Convert.ToString(washerDTO.WasherNumber);
                chkStop.Checked = washerDTO.Stop;
                _gloveSize = washerDTO.GloveSize;
                _gloveType = washerDTO.GloveType;
                _loggedInUser = washerDTO.OperatorId;
                oldwasherDTO = washerDTO;
            }
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "EditWasherDetails", null);
                return;
            }
            BindGloveCodes();
            BindSize();
        }
        #endregion


        #region User Methods

        /// <summary>
        /// Validation of Reason text field
        /// </summary>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbGloveDescription, "Glove Description", ValidationType.Required));
            bool isValidGloveType = Convert.ToBoolean(SetUpConfigurationBLL.ValidateGloveTypeDescription(cmbGloveDescription.Text.Trim()));
            if (isValidGloveType)
            {
                validationMesssageLst.Add(new ValidationMessage(cmbSize, "Size", ValidationType.Required));
            }
            return ValidateForm();
        }

        /// <summary>
        /// Validation of data entered in fields
        /// </summary>
        private bool ValidateInputData()
        {
            bool status = false;
            bool isValidGloveType;
            string validFieldMessage = Messages.INVALID_DATA_MESSAGE;

            if (cmbGloveDescription.Text.Trim().Length > Constants.GLOVE_DESCRIPTION_LENGTH)
            {
                validFieldMessage = validFieldMessage + "Glove Description " + Environment.NewLine;
                cmbGloveDescription.Text = string.Empty;
                cmbGloveDescription.Focus();
            }

            try
            {
                isValidGloveType = Convert.ToBoolean(SetUpConfigurationBLL.ValidateGloveTypeDescription(cmbGloveDescription.Text.Trim()));
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ValidateInputData", null);
                return false;
            }

            if (!isValidGloveType)
            {
                validFieldMessage = validFieldMessage + "Glove Description " + Environment.NewLine;
                cmbSize.SelectedIndex = Constants.MINUSONE;
                cmbSize.Focus();
            }

            if (validFieldMessage == Messages.INVALID_DATA_MESSAGE)
            {
                status = true;
            }
            else
            {
                GlobalMessageBox.Show(validFieldMessage, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                status = false;
            }
            return status;
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
        /// Binds the Size ComboBox
        /// </summary>
        private void BindSize()
        {
            try
            {
                cmbSize.BindComboBox(SetUpConfigurationBLL.GetGloveSizesForWasher(_gloveType), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindSize", null);
                return;
            }
            cmbSize.SelectedValue = _gloveSize;
        }

        /// <summary>
        /// Binds the Glove codes to ComboBox
        /// </summary>
        private void BindGloveCodes()
        {
            try
            {
                cmbGloveDescription.BindComboBox(SetUpConfigurationBLL.GetGloveCodes(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindGloveCodes", null);
                return;
            }
            _gloveType = oldwasherDTO.GloveType;
            cmbGloveDescription.SelectedValue = _gloveType;
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
            int rowsReturned;
            bool isInProcess = false;

            if (ValidateRequiredFields())
            {
                if (ValidateInputData())
                {
                    WasherDTO objWasher = new WasherDTO();
                    try
                    {
                        objWasher.GloveTypeDescription = cmbGloveDescription.Text.Trim();
                        objWasher.Stop = chkStop.Checked;
                        objWasher.WasherNumber = Convert.ToInt32(txtWasherNo.Text.Trim());
                        objWasher.Id = _washerId;
                        objWasher.GloveSize = Convert.ToString(cmbSize.SelectedValue);
                        objWasher.GloveType = _gloveType;
                        objWasher.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                        objWasher.OperatorId = _loggedInUser;

                        if (chkStop.Checked)
                        {
                            isInProcess = Convert.ToBoolean(SetUpConfigurationBLL.IsWasherInUse(objWasher.Id));
                        }
                        if (!isInProcess)
                        {
                            //set audit log class
                            AuditLogDTO AuditLog = new AuditLogDTO();
                            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                            AuditLog.FunctionName = _screenName;
                            AuditLog.CreatedBy = _loggedInUser;
                            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), Constants.Update);
                            AuditLog.AuditAction = Convert.ToInt32(audAction);
                            AuditLog.SourceTable = "WasherMaster";
                            string refid = "";

                            refid = oldwasherDTO.Id.ToString();
                            AuditLog.ReferenceId = refid;

                            AuditLog.UpdateColumns = oldwasherDTO.DetailedCompare(objWasher).GetPropChanges();

                            string audlog = CommonBLL.SerializeTOXML(AuditLog);

                            rowsReturned = SetUpConfigurationBLL.EditWasherDetails(objWasher, audlog);
                            if (rowsReturned > 0)
                            {
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                oldwasherDTO = null;
                                this.Close();
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.WASHER_IN_USE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            this.Close();
                        }
                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                        return;
                    }

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
        private void chkStop_Paint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox.Focused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
            }
        }

        #endregion

        private void cmbGloveDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _gloveType = cmbGloveDescription.Text.Trim();
                BindSize();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "cmbGloveDescription_SelectedIndexChanged", null);
                return;
            }
        }
    }
}
