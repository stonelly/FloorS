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
using Hartalega.FloorSystem.IntegrationServices;

namespace Hartalega.FloorSystem.Windows.UI.Washer
{
    /// <summary>
    /// ScanBatchCard Screen - Washer System
    /// </summary>
    public partial class ScanBatchCard : FormBase
    {
        #region Private Class Variables
        private WasherScanBatchCardDTO _wsbc = null;
        private BatchDTO _btch = null;
        private bool _isValid = false;
        private bool _isRework = false;
        private List<WasherDTO> _washerList = null;
        private List<WasherProgramDTO> _washerProgramList = null;
        private const string _screenName = "Washer System Scan Batch Card";
        private const string _className = "ScanBatchCard";
        private const string _subSystem = "Washer System";
        private string _workStationId = string.Empty;
        private string _workStationName = string.Empty;
        private bool _isWasherAvailable = true;
        private bool _isWasherProgramAvailable = true;
        private decimal _serialNumberEntered = Constants.ZERO;
        private string _operatorId = string.Empty;
        private DryerScanBatchCardDTO _dsbc = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize components
        /// </summary>
        public ScanBatchCard()
        {
            InitializeComponent();
        }
        
        #endregion

        #region Event Handlers

        /// <summary>
        /// validate operator id and populate Operator name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperatorID_Leave(object sender, EventArgs e)
        {
            OperatorLeaveValidation();
        }

        /// <summary>
        /// Load date and location on screen load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanBatchCard_Load(object sender, EventArgs e)
        {
            try
            {
                ClearForm();
                txtLocation.Text = Convert.ToString(WorkStationDTO.GetInstance().LocationId);
                txtOperatorId.OperatorId();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, null);
                return;
            }
        }

        /// <summary>
        /// validate serial no and populate fields based on it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            SerialNumberLeaveValidation();
        }

        /// <summary>
        /// Validate washer duration for last batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddWasherProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            string gloveType = string.Empty;
            string gloveSize = string.Empty;
            try
            {
                if (this.ddWasherProgram.SelectedIndex != Constants.MINUSONE)
                {
                    if (string.IsNullOrEmpty(this.ddWasherProgram.Text))
                    {
                        GlobalMessageBox.Show(Messages.REQUIREDFIELD_WASHER_PROGRAM, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                        //ddWasherProgram.SelectedIndex = Constants.ONE;
                        ddWasherProgram.Focus();
                    }
                    else
                    {
                        if (ValidateWasherProgramCompleted())
                        {
                            btnSave.Enabled = true;
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.REQUIREDFIELD_WASHER_PROGRAM, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            ResetCommonControls();
                            DisableReworkFields();
                        }
                    }
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.ddWasherProgram.Name, null);
                return;
            }
        }

        /// <summary>
        /// Save data into database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int RowsAffected = Constants.ZERO;
            decimal SerialNo = Constants.ZERO;
            string OperatorId = string.Empty;
            int WasherProgram = Constants.ZERO;
            bool isRework = false;
            TimeSpan StartTime = new TimeSpan();
            string Reason = string.Empty;
            int ReworkCount = Constants.ZERO;
            int LocationId = Constants.ZERO;
            DateTime LastModifiedOn = new DateTime();
            int WorkStationNumber = Constants.ZERO;
            int WasherNumber = Constants.ZERO;
            string GloveType = string.Empty;
            string GloveSize = string.Empty;
            DateTime dtStartTime = new DateTime();
            try
            {
                if (ValidateRequiredFields())
                {
                    SerialNo = Convert.ToDecimal(txtSerialNo.Text);
                    OperatorId = txtOperatorId.Text;
                    if (OperatorId == _operatorId)
                    {
                        if (SerialNo == _serialNumberEntered)
                        {
                            
                            WasherProgram = Convert.ToInt32(ddWasherProgram.SelectedValue);
                            LocationId = WorkStationDTO.GetInstance().LocationId;
                            WorkStationNumber = Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId);
                            WasherNumber = Convert.ToInt32(ddWasher.Text);
                            GloveType = txtGloveType.Text;
                            GloveSize = txtGloveSize.Text;
                            LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                            isRework = _isRework;
                            dtStartTime = CommonBLL.GetCurrentDateAndTime(); 
                            StartTime = new TimeSpan(Constants.ZERO, dtStartTime.Hour, dtStartTime.Minute, dtStartTime.Second, dtStartTime.Millisecond);
                            if (isRework)
                            {
                                if (this.ddReworkReason.SelectedIndex != Constants.MINUSONE)
                                {
                                    ReworkCount = Convert.ToInt32(txtReworkFrequency.Text);
                                    Reason = ddReworkReason.Text;
                                    RowsAffected = WasherBLL.SaveWasherScanBatchCardDetails(SerialNo, OperatorId, WasherProgram, StartTime, isRework, Reason, ReworkCount, LocationId, LastModifiedOn, WorkStationNumber, WasherNumber, GloveType, GloveSize);
                                    if (RowsAffected > Constants.ZERO)
                                    {
                                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        ClearForm();
                                    }
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.REWORK_REASON_NOT_SELECTED, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                                    ddReworkReason.Focus();
                                }
                            }
                            else
                            {
                                RowsAffected = WasherBLL.SaveWasherScanBatchCardDetails(SerialNo, OperatorId, WasherProgram, StartTime, isRework, Reason, ReworkCount, LocationId, LastModifiedOn, WorkStationNumber, WasherNumber, GloveType, GloveSize);
                                if (RowsAffected > Constants.ZERO)
                                {
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                }
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
                ExceptionLogging(fsEX, _screenName, _className, btnSave.Name, txtOperatorId.Text);
                return;
            }
        }

        /// <summary>
        /// Clear form 
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
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, btnCancel.Name, txtOperatorId.Text);
                return;
            }
        }

        /// <summary>
        /// to close window when a user press escape
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanBatchCard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
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
        /// Set the washer program to default value if only one washer program exists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddWasher_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddWasherProgram.Items.Count == Constants.TWO)
            {
                ddWasherProgram.SelectedIndex = Constants.ONE;
            }
        }

        /// <summary>
        /// restrict user from entering non numeric values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_TextChanged(object sender, EventArgs e)
        {
            ValidateTextInput(txtSerialNo);
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
                            txtOperatorName.Text = operatorName;
                            _operatorId = operatorId;
                            txtSerialNo.Focus();
                        }
                        else
                        {
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
                ExceptionLogging(fsEX, _screenName, _className, txtOperatorId.Name, txtOperatorId.Text);
                return;
            }
        }

        /// <summary>
        /// Serial Number leave validation
        /// </summary>
        public void SerialNumberLeaveValidation()
        {
            int reasonTypeId = Constants.ZERO;
            int reworkCount = Constants.ZERO;
            List<ReasonDTO> reasonList = null;
            decimal serialNumber = Constants.ZERO;
            string qaiResults = string.Empty;
            bool isBatchInQCProcess = false;
            bool isPTPFGlove = false;
            bool isSurgicalGlove = false;   //Added by Gary 20201029
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
                            DisableReworkFields();
                            return;
                        }
                        //Ended by Tan Wei Wah 20190312

                        serialNumber = Convert.ToDecimal(txtSerialNo.Text);
                        _btch = WasherBLL.GetBatchDetailsBySerialNo(serialNumber);
                        isPTPFGlove = WasherBLL.CheckIsPTPFGlove(serialNumber); // pangys: allow PTPF glove with SP to washer
                        isSurgicalGlove = SurgicalGloveBLL.IsOnlineSurgicalGlove(serialNumber); // Gary: allow by pass QC Type check for surgical glove
                        if (_btch != null)
                        {
                            //#GARY 29/10/2020: allow bypass QC type check if material = surgical glove. START
                            if (!isSurgicalGlove)
                            {
                                //#AZRUL 03/01/2019: Block HBC if QCType is Straight Pack can not do Washer. START
                                if (_btch.BatchType.Trim() == Constants.PRODUCTION_TYPE && _btch.QCTypeDescription == Constants.STRAIGHT_PACK && !isPTPFGlove)
                                {
                                    GlobalMessageBox.Show(Messages.PTQI_QCTYPE_SP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    ResetCommonControls();
                                    DisableReworkFields();
                                    return;
                                }
                                //#AZRUL 03/04/2019: END
                            }
                            //#GARY 29/10/2020: END

                            QITestResultStatusDto qiTestResultStatus = QAIBLL.Get_PT_Or_QC_QITestResult(Convert.ToDecimal(serialNumber));
                            if (!string.IsNullOrEmpty(qiTestResultStatus.Stage))
                            {
                                //#AZRUL 09/01/2019: Block batch card to proceed Washer if PTQI Test Result is Pass and no QCQI. 
                                if (qiTestResultStatus.Stage == Constants.PT_QI && (qiTestResultStatus.QIResult == Constants.PASS)
                                    && !CommonBLL.ValidateAXPosting(serialNumber, CreateRAFJournalFunctionidentifier.SOBC.ToString()))
                                {
                                    GlobalMessageBox.Show(Messages.PTQI_COMPLETED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    ResetCommonControls();
                                    DisableReworkFields();
                                    return;
                                }
                                //#AZRUL 03/04/2019: END
                                //AZRUL 03/04/2019: Block PT Process if QCQI failed.
                                if (qiTestResultStatus.Stage == Constants.QC_QI && (qiTestResultStatus.QIResult.ToUpper() == Constants.FAIL) && _btch.QCType != "0006020020") //0006020020: PT
                                {
                                    GlobalMessageBox.Show(Messages.QCQI_INCOMPLETE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    ResetCommonControls();
                                    DisableReworkFields();
                                    return;
                                }
                                //#AZRUL 03/04/2019: END
                            }

                            _wsbc = WasherBLL.GetWasherBatchDetailsBySerialNo(serialNumber);
                            qaiResults = CommonBLL.ValidateSerialNoByQAIStatus(serialNumber);
                            if (!string.IsNullOrEmpty(qaiResults))
                            {
                                GlobalMessageBox.Show(qaiResults, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ResetCommonControls();
                                DisableReworkFields();
                                return;
                            }
                            _dsbc = DryerBLL.GetDryerBatchDetailsBySerialNo(serialNumber);
                            if (_dsbc != null)
                            {
                                if (_dsbc.StopTime == TimeSpan.Zero)
                                {
                                    GlobalMessageBox.Show(Messages.PREVIOUS_DRYER_PROCESS_FOR_SERIAL_NUMBER_NOT_COMPLETED, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    ResetCommonControls();
                                    DisableReworkFields();
                                    return;
                                }
                            }
                            isBatchInQCProcess = WasherBLL.CheckIsBatchInQCProcess(serialNumber);
                            if (isBatchInQCProcess)
                            {
                                GlobalMessageBox.Show(Messages.BATCH_IS_IN_QC_PROCESS, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ResetCommonControls();
                                DisableReworkFields();
                                return;
                            }
                            //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. START
                            if (CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerialNo.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPI) &&
                                !CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerialNo.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPO))
                            {
                                GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ResetCommonControls();
                                DisableReworkFields();
                                return;
                            }
                            if (QAIBLL.GetLastServiceName(serialNumber) == CreateInvTransJournalFunctionidentifier.STPI)
                            {
                                GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ResetCommonControls();
                                DisableReworkFields();
                                return;
                            }
                            //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. END
                            if (ValidateSerialNumberEligibilityForScan(_wsbc))
                            {
                                if (_wsbc == null)
                                {
                                    PopulateFieldsBasedOnSerialNumber();
                                    DisableReworkFields();
                                }
                                else if (GlobalMessageBox.Show(Messages.DUPLICATE_WASHER_SERIAL_NUMBER, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                                {
                                    PopulateFieldsBasedOnSerialNumber();
                                    if (_isWasherAvailable && _isWasherProgramAvailable)
                                    {
                                        ddReworkReason.Items.Clear();
                                        reworkCount = _wsbc.ReworkCount + Constants.ONE;
                                        txtReworkFrequency.Text = reworkCount.ToString();
                                        reasonTypeId = CommonBLL.GetReasonTypeIdForReasonType(Constants.WASHER_REWORK_REASON_TYPE);
                                        reasonList = WasherBLL.GetReasonTextByReasonTypeId(reasonTypeId);
                                        if (reasonList != null)
                                        {
                                            foreach (ReasonDTO reason in reasonList)
                                            {
                                                ddReworkReason.Items.Add(reason.ReasonText);
                                            }
                                            EnableReworkFields();
                                        }
                                        else
                                        {
                                            ExceptionLogging(new FloorSystemException(Messages.NO_REWORK_REASON_AVAILABLE, _subSystem, new NullReferenceException(Messages.NO_REWORK_REASON_AVAILABLE)), _screenName, _className, ddReworkReason.Name, null);
                                            return;
                                        }
                                        ddWasher.Focus();
                                    }
                                }
                                else
                                {
                                    txtReworkFrequency.Text = string.Empty;
                                    DisableReworkFields();
                                    ResetCommonControls();
                                }
                                _serialNumberEntered = serialNumber;
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.PREVIOUS_WASHER_PROCESS_FOR_SERIAL_NUMBER_NOT_COMPLETED, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ResetCommonControls();
                                DisableReworkFields();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_WASHER_SC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ResetCommonControls();
                            DisableReworkFields();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_WASHER_SC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ResetCommonControls();
                        DisableReworkFields();
                    }
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, txtSerialNo.Name, txtOperatorId.Text);
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
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        /// <summary>
        /// Check whether serial number eligible for scan
        /// </summary>
        /// <param name="_wsbc">washerscanbatchcard dto object</param>
        /// <returns></returns>
        private bool ValidateSerialNumberEligibilityForScan(WasherScanBatchCardDTO _wsbc)
        {
            bool isEligible = false;
            if (_wsbc == null)
            {
                isEligible = true;
            }
            else if (_wsbc.StopTime > TimeSpan.Zero)
            {
                isEligible = true;
            }
            else
            {
                isEligible = WasherBLL.ValidateSerialNumberEligibilityForScan( _wsbc.LastModifiedOn,_wsbc.WasherId);
            }
            return isEligible;
        }

        /// <summary>
        /// Populate fields based on serial number
        /// </summary>
        private void PopulateFieldsBasedOnSerialNumber()
        {
            string gloveType = string.Empty;
            string gloveSize = string.Empty;
            int locationId = WorkStationDTO.GetInstance().LocationId;
            ddWasher.Items.Clear();
            txtGloveType.Text = _btch.GloveType;
            txtGloveSize.Text = _btch.Size;
            gloveType = txtGloveType.Text;
            gloveSize = txtGloveSize.Text;
            if (!gloveType.Equals(string.Empty) && !gloveSize.Equals(string.Empty))
            {
                _washerList = WasherBLL.GetWasherDetailsByGloveTypeAndSize(gloveType, gloveSize, locationId);
                if (_washerList != null)
                {
                    foreach (WasherDTO washer in _washerList)
                    {
                        ddWasher.Items.Add(washer.WasherNumber);
                    }
                    _washerProgramList = WasherBLL.GetWasherProgramIdByGloveType(gloveType);
                    if (_washerProgramList != null)
                    {
                        //Max He
                        //foreach (WasherProgramDTO washerProgram in _washerProgramList)
                        //{
                        //    this.ddWasherProgram.Items.Add(washerProgram.WasherProgramId);
                        //}

                        this.ddWasherProgram.SelectedIndexChanged -= new System.EventHandler(this.ddWasherProgram_SelectedIndexChanged);
                        this.ddWasherProgram.ValueMember = null;
                        this.ddWasherProgram.DataSource = null;
                        this.ddWasherProgram.Items.Clear();
                        this.ddWasherProgram.DataSource = _washerProgramList;
                        this.ddWasherProgram.ValueMember = "WasherProgramId";
                        this.ddWasherProgram.DisplayMember = "WasherProgram";
                        this.ddWasherProgram.SelectedIndexChanged += new System.EventHandler(this.ddWasherProgram_SelectedIndexChanged);
                        _isWasherAvailable = true;
                        _isWasherProgramAvailable = true;
                        txtBatchNo.Text = _btch.BatchNumber;
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        _isWasherProgramAvailable = false;
                        GlobalMessageBox.Show(Messages.NO_WASHER_PROGRAM_AVAILABLE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        ClearForm();
                    }
                }
                else
                {
                    _isWasherAvailable = false;
                    GlobalMessageBox.Show(Messages.NO_WASHER_AVAILABLE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    ClearForm();
                }
            }
        }

        /// <summary>
        /// Validate completion of washer program
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateWasherProgramCompleted()
        {
            bool isDurationOver = true;
            int washerNumber = Constants.ZERO;
            string gloveType = string.Empty;
            string gloveSize = string.Empty;
            washerNumber = Convert.ToInt32(ddWasher.Text);
            gloveType = txtGloveType.Text;
            gloveSize = txtGloveSize.Text;
            isDurationOver = WasherBLL.ValidateWasherProgram(washerNumber, gloveType, gloveSize);
            return isDurationOver;
        }

        /// <summary>
        /// enable rework fields
        /// </summary>
        private void EnableReworkFields()
        {
            ddReworkReason.Enabled = true;
            lblReworkReason.Visible = true;
            lblReworkFrequency.Visible = true;
            txtReworkFrequency.Visible = true;
            ddReworkReason.Visible = true;
            _isRework = true;
        }

        /// <summary>
        /// disbale rework related fields
        /// </summary>
        private void DisableReworkFields()
        {
            lblReworkReason.Visible = false;
            lblReworkFrequency.Visible = false;
            txtReworkFrequency.Visible = false;
            ddReworkReason.Visible = false;
            _isRework = false;
        }

        /// <summary>
        /// clear text of the form controls
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
            ddWasher.Items.Clear();
            //ddWasherProgram.Items.Clear();
            ddWasherProgram.DataSource = null;
            ddReworkReason.Items.Clear();
            lblReworkReason.Visible = false;
            lblReworkFrequency.Visible = false;
            txtReworkFrequency.Visible = false;
            ddReworkReason.Visible = false;
            btnSave.Enabled = false;
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
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

        /// <summary>
        /// Reset common controls
        /// </summary>
        private void ResetCommonControls()
        {
            txtSerialNo.Clear();
            txtGloveSize.Clear();
            txtGloveType.Clear();
            txtBatchNo.Clear();
            txtSerialNo.Clear();
            ddWasher.Items.Clear();
            //ddWasherProgram.Items.Clear();
            ddWasherProgram.DataSource = null;
            txtOperatorId.Clear();
            txtOperatorName.Clear();
            btnSave.Enabled = false;
            txtOperatorId.Focus();
        }

        /// <summary>
        /// Validate whether data is entered in the required fields and valid washer program duration is over
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
            if (txtSerialNo.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.SERIALNO + Environment.NewLine;
            }
            if (ddWasher.SelectedIndex == Constants.MINUSONE)
            {
                requiredFieldMessage = requiredFieldMessage + Constants.WASHER + Environment.NewLine;
            }
            if (string.IsNullOrEmpty(ddWasherProgram.Text))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.WASHERPROGRAM + Environment.NewLine;
            }
            if (ddReworkReason.Visible && ddReworkReason.SelectedIndex == Constants.MINUSONE)
            {
                requiredFieldMessage = requiredFieldMessage + Constants.REWORK_REASON_WASHER + Environment.NewLine;
            }
            if (requiredFieldMessage.Equals(Messages.REQUIREDFIELDMESSAGE))
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
                    else if (txtSerialNo.Text.Equals(string.Empty))
                    {
                        txtSerialNo.Focus();
                    }
                    else if (ddWasher.SelectedIndex == Constants.MINUSONE)
                    {
                        ddWasher.Focus();
                    }
                    else if (string.IsNullOrEmpty(this.ddWasherProgram.Text))
                    {
                        ddWasherProgram.Focus();
                    }
                    else if (ddReworkReason.Visible && ddReworkReason.SelectedIndex == Constants.MINUSONE)
                    {
                        ddReworkReason.Focus();
                    }
                }
                status = false;
            }
            return status;
        }
        
        #endregion

        //Test Changes by Ajay.
    }
}
