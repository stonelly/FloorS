using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Configuration;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.Dryer
{
    public partial class ScanStopTime : FormBase
    {
        #region private class variables
        private BatchDTO _btch;
        private DryerScanBatchCardDTO _dsbc;
        private bool _isValid;
        private const string _screenName = "Dryer System Scan Stop Time";
        private const string _className = "ScanStopTime";
        private const string _subSystem = "Dryer System";
        string _qaiResults = string.Empty;
        private bool _validOperatorId = true;
        #endregion

        #region constructors
        public ScanStopTime()
        {
            InitializeComponent();
        }
        #endregion

        #region usermethods
        /// <summary>
        /// Log exception to DB, show message to the user and clear the form
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
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }
        /// <summary>
        /// Reset Common fields of the screen
        /// </summary>
        private void ResetCommonControls()
        {
            txtSerialNo.Clear();
            txtReworkFrequency.Clear();
            txtGloveSize.Clear();
            txtGloveType.Clear();
            txtBatchNo.Clear();
            txtDryer.Clear();
            txtDryerProgram.Clear();
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
        /// Reset Operator fields of the screen
        /// </summary>
        private void ResetOperatorFields()
        {
            txtOperatorId.Clear();
            txtOperatorName.Clear();
            txtOperatorId.Focus();
        }
        /// <summary>
        /// Clear the text of form controls
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
            txtDryer.Clear();
            txtDryerProgram.Clear();
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
        /// Validate whether data entered in required fields
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateRequiredFields()
        {
            //if (this.txtOperatorId.Text.Equals(""))
            //{
            //    MessageBox.Show("Please enter operator id");
            //    this.txtOperatorId.Focus();
            //    return false;
            //}
            //else if (this.txtSerialNo.Text.Equals(""))
            //{
            //    MessageBox.Show("Please enter serial number");
            //    this.txtSerialNo.Focus();
            //    return false;
            //}
            //else
            //    return true;
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
            if (txtSerialNo.Text.Equals(string.Empty))
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
                    GlobalMessageBox.Show(validDataMessage, Constants.AlertType.Error, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
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
        #endregion

        #region eventhandlers
        /// <summary>
        /// Validate if a user enters only numeric data and back space
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperatorId_KeyPress(object sender, KeyPressEventArgs e)
        {

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
        /// Validate operator id and fetch operator name based on that
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperatorId_Leave(object sender, EventArgs e)
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
                            this.txtOperatorName.Text = operatorName;
                            this.txtSerialNo.Focus();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INVALID_OPERATOR_ID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
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
                ExceptionLogging(fsEX, _screenName, _className, txtOperatorId.Name, txtOperatorId.Text);
                return;
            }
        }
        /// <summary>
        /// validate serial no, and fetch details based on that
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            bool isDurationOver = false;
            int dryerId = Constants.ZERO;
            int dryerNumber = Constants.ZERO;
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
                            //_dsbc = DryerBLL.GetDryerBatchDetailsBySerialNo(serialNumber);
                            _dsbc = DryerBLL.GetDryerBatchDetailsBySerialNoLocation(serialNumber, WorkStationDTO.GetInstance().LocationId);
                            if (_dsbc != null)
                            {
                                if (_dsbc.StopTime.Ticks == Constants.ZERO)
                                {
                                    txtBatchNo.Text = Convert.ToString(_btch.BatchNumber);
                                    txtGloveType.Text = Convert.ToString(_btch.GloveType);
                                    txtGloveSize.Text = Convert.ToString(_btch.Size);
                                    dryerId = _dsbc.DryerId;
                                    dryerNumber = DryerBLL.GetDryerNumberByDryerId(dryerId);
                                    txtDryer.Text = Convert.ToString(dryerNumber);
                                    //txtDryerProgram.Text = _dsbc.DryerProgram.ToString();
                                    txtDryerProgram.Text = _dsbc.DryerProgram.ToString();
                                    dateFormat = ConfigurationManager.AppSettings["dateFormat"];
                                    txtStartTime.Text = _dsbc.LastModifiedOn.ToString(dateFormat);
                                    if (_dsbc.ReworkCount > Constants.ZERO)
                                    {
                                        txtReworkReason.Text = _dsbc.ReworkReason;
                                        txtReworkFrequency.Text = _dsbc.ReworkCount.ToString();
                                        lblReworkReason.Visible = true;
                                        lblReworkFrequency.Visible = true;
                                        txtReworkFrequency.Visible = true;
                                        txtReworkReason.Visible = true;
                                    }
                                    gloveType = txtGloveType.Text;
                                    gloveSize = txtGloveSize.Text;
                                    isDurationOver = DryerBLL.ValidateDryerProgram(dryerNumber, gloveType, gloveSize);
                                    if (isDurationOver)
                                    {
                                        btnSave.Enabled = true;
                                        btnSave.Focus();
                                    }
                                    else
                                    {
                                        GlobalMessageBox.Show(Messages.DRYER_DURATION_NOT_COMPLETED, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        ResetCommonControls();
                                    }
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DRYER_PROGRAM_ALREADY_STOPPED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    ResetCommonControls();
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_NOT_SCANNED_DRYER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ResetCommonControls();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_DRYER_SC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ResetCommonControls();
                        }

                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_DRYER_SC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ResetCommonControls();
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
        /// Save details to DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int RowsAffected = Constants.ZERO;
            decimal SerialNo = Constants.ZERO;
            string OperatorId = string.Empty;
            int DryerProgram = Constants.ZERO;
            string Reason = string.Empty;
            DateTime LastModifiedOn = new DateTime();
            int DryerNumber = Constants.ZERO;
            string GloveType = string.Empty;
            string GloveSize = string.Empty;
            try
            {
                if (ValidateRequiredFields())
                {
                    SerialNo = Convert.ToDecimal(txtSerialNo.Text);
                    OperatorId = txtOperatorId.Text;
                    DryerProgram = Convert.ToInt32(txtDryerProgram.Text);
                    DryerNumber = Convert.ToInt32(txtDryer.Text);
                    GloveType = txtGloveType.Text;
                    GloveSize = txtGloveSize.Text;
                    LastModifiedOn = CommonBLL.GetCurrentDateAndTime(); 
                    RowsAffected = DryerBLL.SaveDryerScanOutBatchCardDetails(SerialNo, OperatorId, DryerProgram, DryerNumber, LastModifiedOn, GloveType, GloveSize);
                    if (RowsAffected > Constants.ZERO)
                    {
                        if (!string.IsNullOrEmpty(_qaiResults))
                        {
                            GlobalMessageBox.Show(Messages.QAI_EXPIRY_INFORMATION, Constants.AlertType.Information,Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        }
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        ClearForm();
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
        /// Clear Form fields and focus first editable field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CONFIRM_CANCEL_TRANSACTION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
            }
        }
        /// <summary>
        /// Load date and Location on screen load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanStopTime_Load(object sender, EventArgs e)
        {
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, null);
                return;
            }
            txtLocation.Text = WorkStationDTO.GetInstance().LocationId.ToString();
            btnSave.Enabled = false;
            lblReworkReason.Visible = false;
            lblReworkFrequency.Visible = false;
            txtReworkFrequency.Visible = false;
            txtReworkReason.Visible = false;
            txtOperatorId.OperatorId();
        }

        /// <summary>
        /// Close the screen when user press ESC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanStopTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
        #endregion


    }
}
