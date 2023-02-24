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
    /// Screen Name: Edit Dryer Details
    /// File Type: Code file
    /// </summary> 
    public partial class EditDryerDetails : FormBase
    {
        #region Method Variables
        private int _dryerId;
        private string _gloveType;
        private string _gloveSize;
        private string _screenName = "Configuration SetUp - EditDryerDetails";
        private string _className = "EditDryerDetails";
        private string _loggedInUser;
        private DryerDTO oldDryerDto;

        private string _screenMasterNameForEventLog = "Edit Dryer Details";
        #endregion

        #region Parameterised Constructors
        /// <summary>
        /// Constructor initialising values retrieved from main form 
        /// </summary>
        /// <param name="DryerDTO dryerDTO"></param>
        /// <returns></returns>
        public EditDryerDetails(DryerDTO dryerDTO)
        {
            InitializeComponent();
            if (dryerDTO != null)
            {
                txtDryerNumber.Text = Convert.ToString(dryerDTO.DryerNumber);
                txtHot.Text = Convert.ToString(Convert.ToInt32(dryerDTO.Hot));
                txtCold.Text = Convert.ToString(Convert.ToInt32(dryerDTO.Cold));
                txtHotAndCold.Text = Convert.ToString(Convert.ToInt32(dryerDTO.HotAndCold));
                chkStop.Checked = dryerDTO.IsStopped;
                chkScheduleStop.Checked = dryerDTO.IsScheduledStop;
                chkCheckGlove.Checked = dryerDTO.CheckGlove;
                chkCheckSize.Checked = dryerDTO.CheckSize;
                _dryerId = dryerDTO.Id;
                _gloveType = dryerDTO.GloveType;
                _gloveSize = dryerDTO.GloveSize;
                _loggedInUser = dryerDTO.OperatorId;
                oldDryerDto = dryerDTO;
            }
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "EditDryerDetails", null);
                return;
            }
            BindGloveCodes();
            BindSize();
        }
        #endregion

        #region User Methods

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
            _gloveType = oldDryerDto.GloveType;
            cmbGloveDescription.SelectedValue = _gloveType;
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
            _gloveSize = oldDryerDto.GloveSize;
            cmbSize.SelectedValue = _gloveSize;
        }

        /// <summary>
        /// Validation of Required fields
        /// </summary>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbGloveDescription, "Glove Type Description", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtHot, "HOT", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtCold, "COLD", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtHotAndCold, "HOT and COLD", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbSize, "Size", ValidationType.Required));
            return ValidateForm();
        }

        /// <summary>
        /// Validation of data entered in fields
        /// </summary>
        private bool ValidateInputData()
        {
            bool status = false;
            bool isValidSize;
            bool isValidGloveType;
            string validFieldMessage = Messages.INVALID_DATA_MESSAGE;

            if (cmbGloveDescription.Text.Trim().Length > Constants.GLOVE_DESCRIPTION_LENGTH)
            {
                validFieldMessage = validFieldMessage + "Glove Type Description " + Environment.NewLine;
                cmbGloveDescription.Text = string.Empty;
                cmbGloveDescription.Focus();
            }
            if (txtHot.Text.Trim() != Convert.ToString(Convert.ToInt32(false))
                && txtHot.Text.Trim() != Convert.ToString(Convert.ToInt32(true)))
            {
                validFieldMessage = validFieldMessage + "HOT" + Environment.NewLine;
                txtHot.Text = string.Empty;
                txtHot.Focus();
            }
            if (txtCold.Text.Trim() != Convert.ToString(Convert.ToInt32(false))
                && txtCold.Text.Trim() != Convert.ToString(Convert.ToInt32(true)))
            {
                validFieldMessage = validFieldMessage + "COLD" + Environment.NewLine;
                txtCold.Text = string.Empty;
                txtCold.Focus();
            }
            if (txtHotAndCold.Text.Trim() != Convert.ToString(Convert.ToInt32(false))
                 && txtHotAndCold.Text.Trim() != Convert.ToString(Convert.ToInt32(true)))
            {
                validFieldMessage = validFieldMessage + "HOT and COLD" + Environment.NewLine;
                txtHotAndCold.Text = string.Empty;
                txtHotAndCold.Focus();
            }

            try
            {
                isValidSize = Convert.ToBoolean(SetUpConfigurationBLL.ValidateGloveSizeByTypeDescription(cmbSize.Text.Trim(),
                                                  cmbGloveDescription.Text.Trim()));

                isValidGloveType = Convert.ToBoolean(SetUpConfigurationBLL.ValidateGloveTypeDescription(cmbGloveDescription.Text.Trim()));

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ValidateInputData", null);
                return false;
            }
            if (!isValidGloveType)
            {
                validFieldMessage = validFieldMessage + "Glove Type Description" + Environment.NewLine;
                cmbGloveDescription.Text = string.Empty;
                cmbGloveDescription.Focus();
            }

            if (!isValidSize)
            {
                validFieldMessage = validFieldMessage + "Size" + Environment.NewLine;
                cmbSize.Text = string.Empty;
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
                    DryerDTO objDryer = new DryerDTO();
                    try
                    {
                        objDryer.DryerNumber = Convert.ToInt32(txtDryerNumber.Text.Trim());
                        objDryer.GloveTypeDescription = cmbGloveDescription.Text.Trim();
                        objDryer.GloveSize = cmbSize.Text.Trim();
                        objDryer.Hot = Convert.ToBoolean(Convert.ToInt32(txtHot.Text.Trim()));
                        objDryer.Cold = Convert.ToBoolean(Convert.ToInt32(txtCold.Text.Trim()));
                        objDryer.HotAndCold = Convert.ToBoolean(Convert.ToInt32(txtHotAndCold.Text.Trim()));
                        objDryer.IsStopped = chkStop.Checked;
                        objDryer.IsScheduledStop = chkScheduleStop.Checked;
                        objDryer.CheckGlove = chkCheckGlove.Checked;
                        objDryer.CheckSize = chkCheckSize.Checked;
                        objDryer.Id = _dryerId;
                        objDryer.GloveType = _gloveType;
                        objDryer.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                        objDryer.OperatorId = _loggedInUser;

                        if (chkStop.Checked)
                        {
                            isInProcess = Convert.ToBoolean(SetUpConfigurationBLL.IsDryerInUse(objDryer.Id));

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
                            AuditLog.SourceTable = "DryerMaster";
                            string refid = "";

                            refid = oldDryerDto.Id.ToString();
                            AuditLog.ReferenceId = refid;

                            AuditLog.UpdateColumns = oldDryerDto.DetailedCompare(objDryer).GetPropChanges();

                            string audlog = CommonBLL.SerializeTOXML(AuditLog);

                            rowsReturned = SetUpConfigurationBLL.EditDryerDetails(objDryer, audlog);
                            if (rowsReturned > 0)
                            {
                                //event log myadamas 20190227
                                EventLogDTO EventLog = new EventLogDTO();

                                EventLog.CreatedBy = _loggedInUser;
                                Constants.EventLog evtAction = Constants.EventLog.Save;
                                EventLog.EventType = Convert.ToInt32(evtAction);
                                EventLog.EventLogType = Constants.eventlogtype;

                                var screenid = CommonBLL.GetScreenIdByScreenName(_screenMasterNameForEventLog);
                                CommonBLL.InsertEventLog(EventLog, _screenMasterNameForEventLog, screenid.ToString());

                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                oldDryerDto = null;
                                this.Close();
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DRYER_IN_USE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
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

        /// <summary>
        /// Event Handler for setting focus on checkbox on Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkScheduleStop_Paint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox.Focused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
            }
        }

        /// <summary>
        /// Event Handler for setting focus on checkbox on Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCheckGlove_Paint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox.Focused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
            }
        }

        /// <summary>
        /// Event Handler for setting focus on checkbox on Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCheckSize_Paint(object sender, PaintEventArgs e)
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
