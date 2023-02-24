using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Configuration;
using System.Windows.Forms;
namespace Hartalega.FloorSystem.Windows.UI.Washer
{
    /// <summary>
    /// ScanStopTime
    /// </summary>
    public partial class ScanStopTime : FormBase
    {
        #region Private Class Variable
        private BatchDTO _btch = null;
        private WasherScanBatchCardDTO _wsbc = null;
        private bool _isValid = true;
        private bool _validOperatorId = true;
        private const string _screenName = "Washer System Scan Stop Time";
        private const string _className = "ScanStopTime";
        private string _workStationId = string.Empty;
        private string _workStationName = string.Empty;
        private decimal _serialNumberEntered = Constants.ZERO;
        private string _operatorId = string.Empty;
        private string _qaiResults = string.Empty;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize window
        /// </summary>
        public ScanStopTime()
        {
            InitializeComponent();
        }
        #endregion

        #region User Methods
        /// <summary>
        /// Operator leave validation
        /// </summary>
        public void OperatorLeaveValidation()
        {
            string operatorId = string.Empty;
            string operatorName = string.Empty;
            try
            {
                if (txtOperatorId.Text != string.Empty)
                {
                    if (SecurityModuleBLL.ValidateOperatorAccess(txtOperatorId.Text.Trim(), _screenName))
                    {
                        operatorId = txtOperatorId.Text.Trim();
                        operatorName = WasherBLL.GetOperatorName(operatorId);
                        if (!string.IsNullOrEmpty(operatorName))
                        {
                            _validOperatorId = true;
                            txtOperatorName.Text = operatorName;
                            _operatorId = operatorId;
                            txtSerialNo.Focus();
                        }
                        else
                        {
                            _validOperatorId = false;
                            //MessageBox.Show(Messages.INVALID_OPERATOR_ID_WASHER, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            GlobalMessageBox.Show(Messages.INVALID_OPERATOR_ID_WASHER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ResetOperatorFields();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.OPERATOR_SCREEN_ACCESS_MSG, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        txtOperatorId.Text = String.Empty;
                        txtOperatorName.Text = String.Empty;
                        txtOperatorId.Focus();
                    }
                }
                else
                {
                    txtOperatorName.Clear();
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.txtOperatorId.Name, this.txtOperatorId.Text);
                return;
            }
        }
        /// <summary>
        /// Serial Number leave validation
        /// </summary>
        public void SerialNumberLeaveValidation()
        {
            bool isDurationOver = false;
            int washerId = Constants.ZERO;
            int washerNumber = Constants.ZERO;
            string gloveType = string.Empty;
            string gloveSize = string.Empty;
            string dateFormat = string.Empty;
            decimal serialNumber = Constants.ZERO;
            _qaiResults = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
                {
                    _isValid = Validator.IsValidInput(Constants.ValidationType.Integer, txtSerialNo.Text);
                    if (_isValid)
                    {
                        //Added by Tan Wei Wah 20190312
                        if (CommonBLL.GetDowngradeStatusFromDB(txtSerialNo.Text.Trim()))
                        {
                            GlobalMessageBox.Show(Messages.DOWNGRADE_VALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ResetCommonControls();
                            return;
                        }
                        //Ended by Tan Wei Wah 20190312

                        serialNumber = Convert.ToDecimal(txtSerialNo.Text);
                        _btch = WasherBLL.GetBatchDetailsBySerialNo(serialNumber);
                        if (_btch != null)
                        {
                            _qaiResults = CommonBLL.ValidateSerialNoByQAIStatus(serialNumber);
                            //_wsbc = WasherBLL.GetWasherBatchDetailsBySerialNo(serialNumber);
                            _wsbc = WasherBLL.GetWasherBatchDetailsBySerialNoLocation(serialNumber, WorkStationDTO.GetInstance().LocationId);
                            if (_wsbc != null)
                            {
                                if (_wsbc.StopTime.Ticks == Constants.ZERO)
                                {
                                    txtBatchNo.Text = Convert.ToString(_btch.BatchNumber);
                                    txtGloveType.Text = Convert.ToString(_btch.GloveType);
                                    txtGloveSize.Text = Convert.ToString(_btch.Size);
                                    washerId = _wsbc.WasherId;
                                    washerNumber = WasherBLL.GetWasherNumberByWasherId(washerId);
                                    //txtWasher.Text = Convert.ToString(_wsbc.WasherId);
                                    txtWasher.Text = Convert.ToString(washerNumber);
                                    txtWasherProgram.Text = Convert.ToString(_wsbc.WasherProgram);
                                    dateFormat = ConfigurationManager.AppSettings["dateFormat"];
                                    txtStartTime.Text = _wsbc.LastModifiedOn.ToString(dateFormat);
                                    if (_wsbc.ReworkCount > Constants.ZERO)
                                    {
                                        txtReworkReason.Text = _wsbc.ReworkReason;
                                        txtReworkFrequency.Text = _wsbc.ReworkCount.ToString();
                                        lblReworkFrequency.Visible = true;
                                        txtReworkFrequency.Visible = true;
                                    }
                                    gloveType = txtGloveType.Text;
                                    gloveSize = txtGloveSize.Text;
                                    isDurationOver = WasherBLL.ValidateWasherProgram(washerNumber, gloveType, gloveSize);
                                    if (isDurationOver)
                                    {
                                        _serialNumberEntered = serialNumber;
                                        btnSave.Enabled = true;
                                        btnSave.Focus();
                                    }
                                    else
                                    {
                                        //MessageBox.Show(Messages.WASHER_DURATION_NOT_COMPLETED, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        GlobalMessageBox.Show(Messages.WASHER_DURATION_NOT_COMPLETED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                        ResetCommonControls();
                                    }
                                }
                                else
                                {
                                    //MessageBox.Show(Messages.WASHER_PROGRAM_ALREADY_STOPPED, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    GlobalMessageBox.Show(Messages.WASHER_PROGRAM_ALREADY_STOPPED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    ResetCommonControls();
                                }
                            }
                            else
                            {
                                //MessageBox.Show(Messages.SERIAL_NUMBER_NOT_SCANNED_WASHER, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_NOT_SCANNED_WASHER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ResetCommonControls();
                            }
                        }
                        else
                        {
                            //if (!btnSave.Enabled)
                            //{
                            //MessageBox.Show(Messages.INVALID_SERIAL_NUMBER_WASHER_SC, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_WASHER_SC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ResetCommonControls();
                            //}
                        }
                    }
                    else
                    {
                        //if (!btnSave.Enabled)
                        //{
                        //MessageBox.Show(Messages.INVALID_SERIAL_NUMBER_WASHER_SC, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_WASHER_SC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ResetCommonControls();
                        //}
                    }
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, txtSerialNo.Name, txtSerialNo.Text);
                return;
            }
        }

        /// <summary>
        /// Restrict user from pasting non numeric inputs
        /// </summary>
        /// <param name="txt"></param>
        public void ValidateTextInput(TextBox txt)
        {
            _isValid = Validator.IsValidInput(Constants.ValidationType.Integer, txt.Text);
            if (!_isValid)
            {
                txt.Clear();
            }
        }

        /// <summary>
        /// To call exception log method
        /// </summary>
        /// <param name="floorException">FloorException object </param>
        /// <param name="screenName">ScreenName</param>
        /// <param name="UiClassName">UIClassName</param>
        /// <param name="uiControl">UIControl</param>
        /// <param name="parameters">Parameters</param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                //MessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Messages.CONTACT_MIS, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                //MessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Messages.CONTACT_MIS, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        /// <summary>
        /// Clear text of form controls
        /// </summary>
        private void ClearForm()
        {
            txtOperatorId.Clear();
            txtOperatorName.Clear();
            txtGloveSize.Clear();
            txtGloveType.Clear();
            txtBatchNo.Clear();
            txtSerialNo.Clear();
            txtReworkFrequency.Clear();
            txtWasher.Clear();
            txtWasherProgram.Clear();
            txtReworkReason.Clear();
            lblReworkReason.Visible = false;
            lblReworkFrequency.Visible = false;
            txtReworkFrequency.Visible = false;
            txtReworkReason.Visible = false;
            btnSave.Enabled = false;
            txtStartTime.Clear();
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            txtOperatorId.Focus();
        }

        /// <summary>
        /// Validate whether data enter in required fields or not
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateRequiredFields()
        {
            bool status = false;
            string requiredFieldMessage = Messages.REQUIREDFIELDMESSAGE;
            string validDataMessage = Messages.INVALID_DATA_SUMMARY;

            if (txtOperatorId.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.OPERATORID + Environment.NewLine;
            }
            else
            {
                if (_validOperatorId == false)
                {
                    validDataMessage = validDataMessage + Constants.OPERATORID + Environment.NewLine;
                }
            }
            if (this.txtSerialNo.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.SERIALNO + Environment.NewLine;
            }
            else
            {
                if (_isValid == false || _btch == null)
                {
                    validDataMessage = validDataMessage + Constants.SERIALNO + Environment.NewLine;
                }
            }
            if (requiredFieldMessage.Equals(Messages.REQUIREDFIELDMESSAGE) && validDataMessage.Equals(Messages.INVALID_DATA_SUMMARY))
            {
                status = true;
            }
            else
            {
                if (requiredFieldMessage != Messages.REQUIREDFIELDMESSAGE)
                {
                    //MessageBox.Show(requiredFieldMessage);
                    GlobalMessageBox.Show(requiredFieldMessage, Constants.AlertType.Information, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                    if (txtOperatorId.Text.Equals(string.Empty))
                    {
                        txtOperatorId.Focus();
                    }
                    else
                    {
                        txtSerialNo.Focus();
                    }
                }
                else if (validDataMessage != Messages.INVALID_DATA_SUMMARY)
                {
                    //MessageBox.Show(validDataMessage);
                    GlobalMessageBox.Show(validDataMessage, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    if (!_validOperatorId)
                    {
                        txtOperatorId.Clear();
                        txtOperatorId.Focus();
                    }
                    else
                    {
                        txtSerialNo.Clear();
                        txtSerialNo.Focus();
                    }
                }
                status = false;
            }
            return status;
        }

        /// <summary>
        /// Reset common controls
        /// </summary>
        private void ResetCommonControls()
        {
            txtSerialNo.Clear();
            txtReworkFrequency.Clear();
            txtGloveSize.Clear();
            txtGloveType.Clear();
            txtBatchNo.Clear();
            txtWasher.Clear();
            txtWasherProgram.Clear();
            txtReworkReason.Clear();
            txtStartTime.Clear();
            btnSave.Enabled = false;
            lblReworkReason.Visible = false;
            lblReworkFrequency.Visible = false;
            txtReworkFrequency.Visible = false;
            txtReworkReason.Visible = false;
            txtOperatorId.Clear();
            txtOperatorName.Clear();
            txtOperatorId.Focus();
        }

        /// <summary>
        /// Reset operator related fields
        /// </summary>
        private void ResetOperatorFields()
        {
            txtOperatorId.Clear();
            txtOperatorName.Clear();
            txtOperatorId.Focus();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Validate operator Id and populate operator name based on operator id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperatorId_Leave(object sender, EventArgs e)
        {
            OperatorLeaveValidation();
        }

        /// <summary>
        /// Load date and location on screen load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanStopTime_Load(object sender, EventArgs e)
        {
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
                txtLocation.Text = WorkStationDTO.GetInstance().LocationId.ToString();
                btnSave.Enabled = false;
                lblReworkReason.Visible = false;
                lblReworkFrequency.Visible = false;
                txtReworkFrequency.Visible = false;
                txtReworkReason.Visible = false;
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, null);
                return;
            }
        }

        /// <summary>
        /// validate serial no and populate fields based on that
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            SerialNumberLeaveValidation();
        }

        /// <summary>
        /// Save details in DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsAffected = Constants.ZERO;
            decimal SerialNo = Constants.ZERO;
            string OperatorId = string.Empty;
            int WasherProgram = Constants.ZERO;
            DateTime LastModifiedOn = new DateTime();
            int WasherNumber = Constants.ZERO;
            string GloveType = string.Empty;
            string GloveSize = string.Empty;
             
            try
            {
                LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                if (ValidateRequiredFields())
                {
                    SerialNo = Convert.ToDecimal(txtSerialNo.Text);
                    OperatorId = txtOperatorId.Text;
                    if (OperatorId == _operatorId)
                    {
                        if (SerialNo == _serialNumberEntered)
                        {
                            WasherProgram = Convert.ToInt32(txtWasherProgram.Text);
                            WasherNumber = Convert.ToInt32(txtWasher.Text);
                            GloveType = txtGloveType.Text;
                            GloveSize = txtGloveSize.Text;
                            rowsAffected = WasherBLL.SaveWasherScanOutBatchCardDetails(SerialNo, OperatorId, WasherProgram, WasherNumber, GloveType, GloveSize);
                            if (rowsAffected > Constants.ZERO)
                            {
                                if (!string.IsNullOrEmpty(_qaiResults))
                                {
                                    GlobalMessageBox.Show(Messages.QAI_EXPIRY_INFORMATION, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                }
                                //MessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY);
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                //event log myadamas 20190227
                                EventLogDTO EventLog = new EventLogDTO();

                                EventLog.CreatedBy = OperatorId;
                                Constants.EventLog audAction = Constants.EventLog.Save;
                                EventLog.EventType = Convert.ToInt32(audAction);
                                EventLog.EventLogType = Constants.eventlogtype;

                                var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                                CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());
                                ClearForm();
                            }
                        }
                        else
                        {
                            SerialNumberLeaveValidation();
                        }
                    }
                    else
                    {
                        OperatorLeaveValidation();
                    }
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, btnSave.Name, null);
                return;
            }
        }

        /// <summary>
        /// Clear form fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (GlobalMessageBox.Show(Messages.CONFIRM_CANCEL_TRANSACTION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    ClearForm();
                }
                else
                {
                    this.txtOperatorId.Focus();
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, btnCancel.Name, null);
                return;
            }
        }

        /// <summary>
        /// Validate if a user enters only numeric data and back space
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b') //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

        /// <summary>
        ///  restrict user from entering non numeric values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_TextChanged(object sender, EventArgs e)
        {
            ValidateTextInput(txtSerialNo);
        }
        
        #endregion
    }
}
