using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.IntegrationServices;
using System;
using System.Collections.Generic;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;

namespace Hartalega.FloorSystem.Windows.UI.Dryer
{
    /// <summary>
    /// To provide ScanBatchCard screen to the user
    /// </summary>
    public partial class ScanBatchCard : FormBase
    {
        #region Private Class Variables
        private DryerScanBatchCardDTO _dsbc = null;
        private BatchDTO _btch = null;
        private bool _isValid = false;
        private bool _isRework = false;
        private List<Hartalega.FloorSystem.Business.Logic.DataTransferObjects.DryerDTO> _dryerList = null;
        private List<DryerProgramDTO> _dryerProgramList = null;
        private const string _screenName = "Dryer System Scan Batch Card";
        private const string _className = "ScanBatchCard";
        private const string _subSystem = "Dryer System";
        private WasherScanBatchCardDTO _wsbc = null;
        private bool _isDryerAvailable = true;
        private bool _isDryerProgramAvailable = true;
        private string _HotBoxAuthorizedBy = string.Empty;
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public ScanBatchCard()
        {
            InitializeComponent();
        }
        #endregion

        #region User Methods
        /// <summary>
        /// populate fields of the screen based upon the serial number entered by the user
        /// </summary>
        private void PopulateFieldsBasedOnSerialNumber()
        {
            string gloveType = string.Empty;
            string gloveSize = string.Empty;
            int locationId = WorkStationDTO.GetInstance().LocationId;
            ddDryer.Items.Clear();
            txtGloveType.Text = _btch.GloveType;
            txtGloveSize.Text = _btch.Size;
            txtBatchNo.Text = _btch.BatchNumber;
            gloveType = txtGloveType.Text;
            gloveSize = txtGloveSize.Text;
            if (!gloveType.Equals(string.Empty) && !gloveSize.Equals(string.Empty))
            {
                _dryerList = DryerBLL.GetDryerDetailsByGloveTypeAndSize(gloveType, gloveSize, locationId);
                if (_dryerList != null)
                {
                    foreach (Hartalega.FloorSystem.Business.Logic.DataTransferObjects.DryerDTO dryer in _dryerList)
                    {
                        ddDryer.Items.Add(dryer.DryerNumber);
                    }
                    _dryerProgramList = DryerBLL.GetDryerProgramIdByGloveType(gloveType);
                    if (_dryerProgramList != null)
                    {
                        //foreach (DryerProgramDTO dryerProgram in _dryerProgramList)
                        //{
                        //    ddDryerProgram.Items.Add(dryerProgram.DryerProgramId);
                        //}

                        this.ddDryerProgram.SelectedIndexChanged -= new System.EventHandler(this.ddDryerProgram_SelectedIndexChanged);
                        this.ddDryerProgram.ValueMember = null;
                        this.ddDryerProgram.DataSource = null;
                        this.ddDryerProgram.Items.Clear();
                        this.ddDryerProgram.DataSource = _dryerProgramList;
                        this.ddDryerProgram.ValueMember = "DryerProgramId";
                        this.ddDryerProgram.DisplayMember = "DryerProgram";
                        this.ddDryerProgram.SelectedIndexChanged += new System.EventHandler(this.ddDryerProgram_SelectedIndexChanged);
                        txtBatchNo.Text = _btch.BatchNumber;
                        btnSave.Enabled = true;
                        ddDryer.Focus();
                        _isDryerAvailable = true;
                        _isDryerProgramAvailable = true;
                    }
                    else
                    {
                        _isDryerProgramAvailable = false;
                        GlobalMessageBox.Show(Messages.NO_DRYERPROGRAM_AVAILABLE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearForm();
                    }
                }
                else
                {
                    _isDryerAvailable = false;
                    GlobalMessageBox.Show(Messages.NO_DRYER_AVAILABLE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                }
            }
        }

        /// <summary>
        /// Enable the Rework related fields on the screen
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
        /// Disable the rework related fields on the screen
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
        /// Validate whether a particular batch has completed its total required duration for dryer program or not
        /// </summary>
        /// <returns>True if the duration is over else returns false</returns>
        private Boolean ValidateDryerProgramCompleted()
        {
            bool isDurationOver = true;
            int dryerNumber = Constants.ZERO;
            string gloveType = string.Empty;
            string gloveSize = string.Empty;
            dryerNumber = Convert.ToInt32(ddDryer.Text);
            gloveType = txtGloveType.Text;
            gloveSize = txtGloveSize.Text;
            isDurationOver = DryerBLL.ValidateDryerProgram(dryerNumber, gloveType, gloveSize);
            return isDurationOver;
        }

        /// <summary>
        /// Clear data from all the fields of the form, sets visibilty of rework field to false and focus on the first first editable control on the screen
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
            ddDryer.Items.Clear();
            //ddDryerProgram.Items.Clear();
            ddDryerProgram.DataSource = null;
            ddReworkReason.Items.Clear();
            lblReworkReason.Visible = false;
            lblReworkFrequency.Visible = false;
            txtReworkFrequency.Visible = false;
            ddReworkReason.Visible = false;
            btnSave.Enabled = false;
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            chkhotbox.Checked = false;
            txtOperatorId.Focus();
        }

        /// <summary>
        /// Log Excpetion to DB,Show message to the user and clear the form
        /// </summary>
        /// <param name="floorException">FloorException object </param>
        /// <param name="screenName">ScreenName</param>
        /// <param name="UiClassName">UIClassName</param>
        /// <param name="uiControl">UIControl</param>
        /// <param name="parameters">Parameters</param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string uiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, uiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        /// <summary>
        /// Validates whether data is inserted in all the required fields and dryer program has finished its duration or not
        /// </summary>
        /// <returns>True if all the validations succeed else returns false</returns>
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
            if (ddDryer.SelectedIndex == Constants.MINUSONE)
            {
                requiredFieldMessage = requiredFieldMessage + Constants.DRYER + Environment.NewLine;
            }
            if (string.IsNullOrEmpty(ddDryerProgram.Text))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.DRYERPROGRAM + Environment.NewLine;
            }
            if (ddReworkReason.Visible && ddReworkReason.SelectedIndex == Constants.MINUSONE)
            {
                requiredFieldMessage = requiredFieldMessage + Constants.REWORKREASON + Environment.NewLine;
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
                    else if (ddDryer.SelectedIndex == Constants.MINUSONE)
                    {
                        ddDryer.Focus();
                    }
                    else if (string.IsNullOrEmpty(this.ddDryerProgram.Text))
                    {
                        ddDryerProgram.Focus();
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

        /// <summary>
        /// Reset all the Operator related fields on the screen
        /// </summary>
        private void ResetOperatorFields()
        {
            txtOperatorId.Clear();
            txtOperatorName.Clear();
            txtOperatorId.Focus();
        }

        /// <summary>
        /// Resets all the controls on the screen , apart from Operator and rework related fields
        /// </summary>
        private void ResetCommonControls()
        {
            chkhotbox.Checked = false;
            txtSerialNo.Clear();
            txtGloveSize.Clear();
            txtGloveType.Clear();
            txtBatchNo.Clear();
            txtSerialNo.Clear();
            ddDryer.Items.Clear();
            //ddDryerProgram.Items.Clear();
            ddDryerProgram.DataSource = null;
            btnSave.Enabled = false;
            txtOperatorId.Clear();
            txtOperatorName.Clear();
            txtOperatorId.Focus();
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Validate OperatorId entered by the user and fetch OperatorName if valid operator id is entered
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
                            txtOperatorName.Text = operatorName;
                            txtSerialNo.Focus();
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
        /// Validate Serial number entered by the user and populate other fields on the screen based on the serial number entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            int reasonTypeId = Constants.ZERO;
            int reworkCount = Constants.ZERO;
            List<ReasonDTO> reasonList = null;
            string qaiResults = string.Empty;
            decimal serialNumber = Constants.ZERO;
            bool isBatchInQCProcess = false;
            try
            {
                if (!string.IsNullOrEmpty(this.txtSerialNo.Text.Trim()))
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

                        serialNumber = Convert.ToDecimal(this.txtSerialNo.Text);
                        _btch = WasherBLL.GetBatchDetailsBySerialNo(serialNumber);
                        if (_btch != null)
                        {
                            //#AZRUL 09/01/2019: Block batch card to proceed Washer if PTQI Test Result is Pass and no QCQI. START
                            QITestResultStatusDto qiTestResultStatus = QAIBLL.Get_PT_Or_QC_QITestResult(Convert.ToDecimal(serialNumber));
                            if (!string.IsNullOrEmpty(qiTestResultStatus.Stage))
                            {
                                if (qiTestResultStatus.Stage == Constants.PT_QI && (qiTestResultStatus.QIResult == Constants.PASS)
                                    && !CommonBLL.ValidateAXPosting(serialNumber, CreateRAFJournalFunctionidentifier.SOBC.ToString()))
                                {
                                    GlobalMessageBox.Show(Messages.PTQI_COMPLETED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    ResetCommonControls();
                                    DisableReworkFields();
                                    return;
                                }
                            }
                            //#AZRUL 09/01/2019: END
                            qaiResults = CommonBLL.ValidateSerialNoByQAIStatus(serialNumber);
                            if (!string.IsNullOrEmpty(qaiResults))
                            {
                                GlobalMessageBox.Show(qaiResults, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ResetCommonControls();
                                DisableReworkFields();
                                return;
                            }
                            isBatchInQCProcess = WasherBLL.CheckIsBatchInQCProcess(serialNumber);
                            if (isBatchInQCProcess)
                            {
                                GlobalMessageBox.Show(Messages.BATCH_IS_IN_QC_PROCESS, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ResetCommonControls();
                                DisableReworkFields();
                                return;
                            }
                            _wsbc = WasherBLL.GetWasherBatchDetailsBySerialNo(serialNumber);
                            //_wsbc = WasherBLL.GetWasherBatchDetailsBySerialNoLocation(serialNumber, LocationId);
                            if (_wsbc == null)
                            {
                                //TOTT ID 257 CR HotBox test failed batches is not required to go through Washer process, all the batches only required to rework in Dryer process. 
                                if (chkhotbox.Checked == false && string.IsNullOrEmpty(_HotBoxAuthorizedBy))
                                {
                                    GlobalMessageBox.Show(Messages.WASHER_NOT_COMPLETED_ONE_TIME, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    ResetCommonControls();
                                    DisableReworkFields();
                                    return;
                                }
                            }
                            // It will check whether this serial number scanned in parallely in any other washer is completed or not
                            else if (_wsbc.StopTime == TimeSpan.Zero)
                            {
                                GlobalMessageBox.Show(Messages.PREVIOUS_WASHER_DRYER_PROCESS_FOR_SERIAL_NUMBER_NOT_COMPLETED, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ResetCommonControls();
                                DisableReworkFields();
                                return;
                            }
                            _dsbc = DryerBLL.GetDryerBatchDetailsBySerialNo(serialNumber);
                            if (_dsbc == null)
                            {
                                PopulateFieldsBasedOnSerialNumber();
                                DisableReworkFields();
                            }
                            else
                            {
                                if (ValidateSerialNumberEligibilityForScan(_dsbc))
                                {
                                    if (GlobalMessageBox.Show(Messages.DUPLICATE_DRYER_SERIAL_NUMBER, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                                    {
                                        PopulateFieldsBasedOnSerialNumber();
                                        if (_isDryerAvailable && _isDryerProgramAvailable)
                                        {
                                            ddReworkReason.Items.Clear();
                                            reworkCount = _dsbc.ReworkCount + Constants.ONE;
                                            this.txtReworkFrequency.Text = reworkCount.ToString();
                                            reasonTypeId = CommonBLL.GetReasonTypeIdForReasonType(Constants.DRYER_REWORK_REASON_TYPE);
                                            reasonList = DryerBLL.GetReasonTextByReasonTypeId(reasonTypeId);
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
                                            ddDryer.Focus();
                                        }
                                    }
                                    else
                                    {
                                        DisableReworkFields();
                                        ddReworkReason.Items.Clear();
                                        ResetCommonControls();
                                    }
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.PREVIOUS_WASHER_DRYER_PROCESS_FOR_SERIAL_NUMBER_NOT_COMPLETED, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    ResetCommonControls();
                                    DisableReworkFields();
                                }
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_DRYER_SC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ResetCommonControls();
                            this.ddReworkReason.Items.Clear();
                            DisableReworkFields();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_DRYER_SC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        this.ddReworkReason.Items.Clear();
                        DisableReworkFields();
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
        /// Check whether serial number eligible for scan
        /// </summary>
        /// <param name="_wsbc">washerscanbatchcard dto object</param>
        /// <returns></returns>
        private bool ValidateSerialNumberEligibilityForScan(DryerScanBatchCardDTO _dsbc)
        {
            bool isEligible = true;
            if (_dsbc.StopTime > TimeSpan.Zero)
            {
                isEligible = true;
            }
            else
            {
                isEligible = DryerBLL.ValidateSerialNumberEligibilityForScan(_dsbc.LastModifiedOn, _dsbc.DryerId);
            }
            return isEligible;
        }

        /// <summary>
        /// Save the details entered by the user on the screen to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsAffected = Constants.ZERO;
            decimal SerialNo = Constants.ZERO;
            string OperatorId = string.Empty;
            int DryerProgram = Constants.ZERO;
            bool isRework = false;
            TimeSpan StartTime = new TimeSpan();
            string Reason = string.Empty;
            int ReworkCount = Constants.ZERO;
            int LocationId = Constants.ZERO;
            DateTime LastModifiedOn = new DateTime();
            string WorkStationNumber = string.Empty;
            int DryerNumber = Constants.ZERO;
            string GloveType = string.Empty;
            string GloveSize = string.Empty;
            string WorkStationName = string.Empty;
            DateTime dtStartTime = new DateTime();
            try
            {
                if (ValidateRequiredFields())
                {
                    SerialNo = Convert.ToDecimal(this.txtSerialNo.Text);
                    OperatorId = txtOperatorId.Text;
                    DryerProgram = Convert.ToInt32(ddDryerProgram.SelectedValue);
                    LocationId = WorkStationDTO.GetInstance().LocationId;
                    WorkStationNumber = WorkStationDTO.GetInstance().WorkStationId;
                    DryerNumber = Convert.ToInt32(this.ddDryer.Text);
                    GloveType = txtGloveType.Text;
                    GloveSize = txtGloveSize.Text;
                    LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                    isRework = _isRework;
                    dtStartTime = CommonBLL.GetCurrentDateAndTime();
                    StartTime = new TimeSpan(Constants.ZERO, dtStartTime.Hour, dtStartTime.Minute, dtStartTime.Second, dtStartTime.Millisecond);
                    if (isRework)
                    {
                        if (ddReworkReason.SelectedIndex != Constants.MINUSONE)
                        {
                            ReworkCount = Convert.ToInt32(txtReworkFrequency.Text);
                            Reason = ddReworkReason.Text;
                            rowsAffected = DryerBLL.SaveDryerScanBatchCardDetails(SerialNo, OperatorId, DryerProgram, StartTime, isRework, Reason, ReworkCount, LocationId, LastModifiedOn, WorkStationNumber, DryerNumber, GloveType, GloveSize, _HotBoxAuthorizedBy);
                            if (rowsAffected > Constants.ZERO)
                            {
                                ////event log myadamas 20190227
                                //EventLogDTO EventLog = new EventLogDTO();

                                //EventLog.CreatedBy = OperatorId;
                                //Constants.EventLog audAction = Constants.EventLog.Save;
                                //EventLog.EventType = Convert.ToInt32(audAction);
                                //EventLog.EventLogType = Constants.eventlogtype;

                                //var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                                //CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());

                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ClearForm();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.REWORK_REASON_NOT_SELECTED, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                            this.ddReworkReason.Focus();
                        }

                    }
                    else
                    {
                        rowsAffected = DryerBLL.SaveDryerScanBatchCardDetails(SerialNo, OperatorId, DryerProgram, StartTime, isRework, Reason, ReworkCount, LocationId, LastModifiedOn, WorkStationNumber, DryerNumber, GloveType, GloveSize, _HotBoxAuthorizedBy);
                        if (rowsAffected > Constants.ZERO)
                        {
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            ClearForm();
                        }
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
        /// Reset all the fields of the screen and focus first editable field
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
        /// Load continuous ticking time and make visibility of rework related fields to false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanBatchCard_Load(object sender, EventArgs e)
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
            txtLocation.Text = Convert.ToString(WorkStationDTO.GetInstance().LocationId);
            btnSave.Enabled = false;
            lblReworkReason.Visible = false;
            lblReworkFrequency.Visible = false;
            txtReworkFrequency.Visible = false;
            ddReworkReason.Visible = false;
            _isRework = false;
            txtOperatorId.OperatorId();
        }

        /// <summary>
        /// Return the system time at an interval of 1000ms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTimer_Tick(object sender, EventArgs e)
        {
            //try
            //{
            //    txtDate.Text = CommonBLL.GetContinouslyTickingDate();
            //}
            //catch (FloorSystemException fsex)
            //{
            //    ExceptionLogging(fsex, _screenName, _className, this.frmTimer.ToString(), txtOperatorId.Text);
            //    return;
            //}
        }

        /// <summary>
        /// Validate whether the batch has completed its required duration in the dryer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddDryerProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            string gloveType = string.Empty;
            string gloveSize = string.Empty;
            try
            {
                if (this.ddDryerProgram.SelectedIndex != Constants.MINUSONE)
                {

                    if (string.IsNullOrEmpty(this.ddDryerProgram.Text))
                    {
                        GlobalMessageBox.Show(Messages.REQUIREDFIELD_DRYER_PROGRAM, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                        //this.ddDryerProgram.SelectedIndex = Constants.MINUSONE;
                        ddDryerProgram.Focus();
                    }
                    else
                    {
                        if (ValidateDryerProgramCompleted())
                        {
                            btnSave.Enabled = true;
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.REQUIREDFIELD_DRYER_PROGRAM, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            ResetCommonControls();
                            DisableReworkFields();
                        }
                    }
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.ddDryerProgram.Name, null);
                return;
            }
        }

        /// <summary>
        /// Return to the main screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanBatchCard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        /// <summary>
        /// Set the default value of dryer program field, if only one dryerprogram is available on for the batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddDryer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddDryerProgram.Items.Count == Constants.TWO)
            {
                ddDryerProgram.SelectedIndex = Constants.ONE;
            }
            try
            {
                if (DryerBLL.GetDryerCold(Convert.ToInt32(this.ddDryer.Text)))
                {
                    DisableReworkFields();
                }
                else
                {
                    if (_dsbc == null)
                        DisableReworkFields();
                    else
                        EnableReworkFields();
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.ddDryerProgram.Name, null);
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
        #endregion

        private void chkhotbox_CheckedChanged(object sender, EventArgs e)
        {
            if (chkhotbox.Checked)
            {
                if (!string.IsNullOrEmpty(txtOperatorId.Text))
                {

                    Login passwordForm = new Login(Constants.Modules.CONFIGURATIONSETUP, _screenName, string.Empty);
                    passwordForm.Authentication = string.Empty;
                    passwordForm.ShowDialog();
                    if (string.IsNullOrEmpty(passwordForm.Authentication))
                    {
                        chkhotbox.Checked = false;
                        return;
                    }
                    else
                    {
                        _HotBoxAuthorizedBy = passwordForm.Authentication;
                    }
                }
                else
                {
                    chkhotbox.Checked = false;
                    GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Constants.OPERATORID, Constants.AlertType.Information, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                }
            }
            else
            {
                _HotBoxAuthorizedBy = string.Empty;
                ResetCommonControls();
            }
        }
    }
}
