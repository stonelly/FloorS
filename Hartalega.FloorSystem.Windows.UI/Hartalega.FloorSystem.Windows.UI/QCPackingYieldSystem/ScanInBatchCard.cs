using System;
using System.Windows.Forms;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System.Collections.Generic;
using Hartalega.FloorSystem.IntegrationServices;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Linq;
using System.Configuration;
using System.Drawing;
using System.Transactions;

namespace Hartalega.FloorSystem.Windows.UI.QCPackingYieldSystem
{
    /// <summary>
    /// Module: QC Packing Yield System
    /// Screen Name: ScanInBatchcard
    /// File Type: Code file
    /// </summary>
    public partial class ScanInBatchCard : FormBase
    {
        #region Member Variables
        private string _screenName = "QC PackingYield - ScanInBatchCard";
        private string _className = "ScanInBatchCard";
        private QCScanningDetails _resultDTO = null;
        private bool _isRework = false;
        private string _moduleId;
        private string _subModuleId;
        private string _serialNumber;
        private string _startDate;
        private string _targetDate;
        private string _qcType;
        private string _groupType;
        #endregion
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public ScanInBatchCard()
        {
            InitializeComponent();
            _moduleId = CommonBLL.GetModuleId(Constants.QC_PACKING_YIELD_SYSTEM);
            _subModuleId = CommonBLL.GetSubModuleId(Constants.SCAN_IN_BATCHCARD);            
        }

        #region User Methods

        /// <summary>
        /// Clear the values of all the controls 
        /// </summary>
        private void ClearForm()
        {
            txtSerialNo.Text = string.Empty;
            txtBatchNo.Text = string.Empty;
            txtGloveType.Text = string.Empty;
            txtSize.Text = string.Empty;
            txtQCType.Text = string.Empty;
            _qcType = string.Empty;
            cmbPlant.SelectedIndex = Constants.MINUSONE;
            cmbQCGroup.SelectedIndex = Constants.MINUSONE;
            cmbQCTargetType.SelectedIndex = Constants.MINUSONE;
            txtGroupMembers.Text = string.Empty;
            cmbReworkReason.SelectedIndex = Constants.MINUSONE;
            txtReworkCount.Text = string.Empty;
            txtBatchStartDate.Text = string.Empty;
            txtBatchTargetDate.Text = string.Empty;
            btnSave.Enabled = false;
            cmbBrand.SelectedIndex = Constants.MINUSONE;
            txtSerialNo.Focus();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ClearForm", null);
                return;
            }

        }
        /// <summary>
        /// Reset rework controls
        /// </summary>
        private void ResetReworkControls()
        {
            cmbReworkReason.SelectedIndex = Constants.MINUSONE;
            txtReworkCount.Text = string.Empty;
            cmbReworkReason.Visible = false;
            lblReworkReason.Visible = false;
            txtReworkCount.Visible = false;
            lblReworkCount.Visible = false;
            _isRework = false;
        }

        /// <summary>
        /// Validate required fields
        /// </summary>
        private bool ValidateRequiredFields()
        {
            bool status = false;
            if (btnSave.Enabled)
            {
                validationMesssageLst = new List<ValidationMessage>();
                validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No", ValidationType.Required));
                validationMesssageLst.Add(new ValidationMessage(cmbQCGroup, "Group Id", ValidationType.Required));
                validationMesssageLst.Add(new ValidationMessage(cmbQCTargetType, "Target Type", ValidationType.Required));
                if (_isRework)
                {
                    validationMesssageLst.Add(new ValidationMessage(cmbReworkReason, "Rework Reason", ValidationType.Required));
                }
                if (string.Compare(_groupType, Constants.PACKING_GROUP_TYPE) == Constants.ZERO)
                {
                    validationMesssageLst.Add(new ValidationMessage(cmbBrand, "Brand", ValidationType.Required));
                }
                return ValidateForm();
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
            ClearForm();
        }

        /// <summary>
        /// Make visibility of the Rework Reason Controls based on the visible value passed
        /// </summary>
        /// <param name="visible"></param>
        private void ReasonControlsVisibility(bool visible)
        {
            lblReworkReason.Visible = visible;
            lblReworkCount.Visible = visible;
            cmbReworkReason.Visible = visible;
            txtReworkCount.Visible = visible;
            _isRework = visible;
            if (visible)
            {
                BindReworkReason();
            }
        }

        /// <summary>
        /// Functionality to be performed on change of Serail Number
        /// </summary>
        private void SerialNoLeaveFunctionality()
        {
            ResetReworkControls();
            string qaiStatus = string.Empty;

            _serialNumber = txtSerialNo.Text.Trim();
            if (string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                validationMesssageLst = new List<ValidationMessage>();
                validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No", ValidationType.Required));
                ValidateForm();
                ClearForm();
            }
            else
            {
                if (txtSerialNo.Text.Length == Constants.SERIAL_LENGTH)
                {
                    Int64 serialNumber = Int64.Parse(txtSerialNo.Text.Trim());
                    QITestResultStatusDto qiTestResultStatus = QAIBLL.Get_PT_Or_QC_QITestResult(Convert.ToDecimal(serialNumber));
                    var isOnlineSurgicalGlove = SurgicalGloveBLL.IsOnlineSurgicalGlove(serialNumber);
                    var isPTPFGlove = WasherBLL.CheckIsPTPFGlove(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                    try
                    {
                        //20201102 Azrul: Surgical must complete PT 1st.
                        //20201214 Azrul: Block if pass PTQI. Fail PTQI only allow scan in QC
                        //20211129 Azrul: HTLG_HSB_002: Special Glove (Clean Room Product) - PTPF Glove to follow Surgical logics.
                        if (isOnlineSurgicalGlove || isPTPFGlove)
                        {
                            //must PASS PTQI then Failed PTQI only allowed
                            if (string.IsNullOrEmpty(qiTestResultStatus.Stage) || (qiTestResultStatus.Stage == Constants.PT_QI && qiTestResultStatus.QIResult == Constants.PASS))
                            {
                                GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_PT_LOCATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearForm();
                                return;
                            }
                            //must have PTQI only allowed
                            if (qiTestResultStatus.Stage == Constants.PT_QI && qiTestResultStatus.QIResult == "")
                            {
                                GlobalMessageBox.Show(Messages.QCQI_WITHOUT_PTQI, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearForm();
                                return;
                            }
                        }

                        //Added by Tan Wei Wah 20190312
                        if (CommonBLL.GetDowngradeStatusFromDB(txtSerialNo.Text.Trim()))
                        {
                            GlobalMessageBox.Show(Messages.DOWNGRADE_VALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            return;
                        }
                        //Ended by Tan Wei Wah 20190312

                        decimal totalPcs = PostTreatmentBLL.GetRemainingBatchTotalPcs(Convert.ToDecimal(serialNumber));
                        if (totalPcs > Constants.ZERO)
                        {
                            //#AZRUL 09/01/2019: Block batch card to proceed if previous PTQI Test Result is failed. START
                            //#AZRUL 15/12/2020: Bypass if Surgical
                            //#Azrul 29/11/2021: HTLG_HSB_002: Special Glove (Clean Room Product) - PTPF Glove to follow Surgical logics.
                            //#Azrul 14/12/2022: Bypass PTQI Fail for 2nd PT onwards if QCType is QC
                            if (!string.IsNullOrEmpty(qiTestResultStatus.Stage) && (!isOnlineSurgicalGlove && !isPTPFGlove) 
                                && (qiTestResultStatus.PTScanInCount < 2 && qiTestResultStatus.QCScanInCount > qiTestResultStatus.PTScanInCount))
                            {
                                if (qiTestResultStatus.Stage == Constants.PT_QI && (string.IsNullOrEmpty(qiTestResultStatus.QIResult) || qiTestResultStatus.QIResult.ToUpper() == Constants.FAIL || string.IsNullOrEmpty(qiTestResultStatus.Stage)))
                                {
                                    GlobalMessageBox.Show(Messages.PTQI_INCOMPLETE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                    ResetReworkControls();
                                    return;
                                }
                            }
                            //#AZRUL 09/01/2019: END
                            string ptStatus = QCPackingYieldBLL.GetPTStatus(serialNumber);
                            if (string.Compare(ptStatus, Constants.PT_INCOMPLETE) == Constants.ZERO)
                            {
                                GlobalMessageBox.Show(Messages.PT_INCOMPLETE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearForm();
                                ResetReworkControls();
                                return;
                            }
                            else if (string.Compare(ptStatus, Constants.PTQI_INCOMPLETE) == Constants.ZERO)
                            {
                                GlobalMessageBox.Show(Messages.PTQI_INCOMPLETE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearForm();
                                ResetReworkControls();
                                return;
                            }
                            //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. START
                            else if (CommonBLL.ValidateAXPosting(serialNumber, CreateInvTransJournalFunctionidentifier.STPI)
                                && !CommonBLL.ValidateAXPosting(serialNumber, CreateInvTransJournalFunctionidentifier.STPO))
                            {
                                GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearForm();
                                ResetReworkControls();
                                return;
                            }
                            else if (QAIBLL.GetLastServiceName(serialNumber) == CreateInvTransJournalFunctionidentifier.STPI)
                            {
                                GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearForm();
                                ResetReworkControls();
                                return;
                            }
                            //#AZRUL 29-10-2018 END
                            //#AZRUL 15/01/2019: 2nd SOBC must performed Change QC Type to create Rework after CBCI. START
                            else if (CommonBLL.ValidateAXPosting(Convert.ToDecimal(serialNumber), CreateRAFJournalFunctionidentifier.SOBC)
                                && !QAIBLL.GetReworkAfterSOBCForSecondSOBC(serialNumber.ToString()))
                            {
                                GlobalMessageBox.Show(Messages.NO_RWK_AFTER_SOBC_FOR_SECOND_SOBC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearForm();
                                ResetReworkControls();
                                return;
                            }
                            //#AZRUL 15/01/2019: END
                            else
                            {
                                //BindQCGroup();

                                BindBrand();
                                _resultDTO = QCScanningBLL.ValidateAndGetDetailsBySerialNumber(serialNumber);
                                // Check test results for Protein/Powder and Hotbox tests
                                string qaTestStatus = QCPackingYieldBLL.GetQATestStatus(serialNumber);

                                if (qaTestStatus == Constants.QA_TEST_PASS)
                                {
                                    // Check the QAI status of the serial number
                                    qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(serialNumber);
                                    if (!string.IsNullOrEmpty(qaiStatus))
                                    {
                                        GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        ClearForm();
                                    }
                                    else
                                    {
                                        #region Checking WorkStationId,WorkStationName;
                                        BindPLGroup();
                                        #endregion

                                        if (_resultDTO != null)
                                        {
                                            //#AZRUL 18/12/2019: Block HBC if QCType is Straight Pack can not do Scan in batch card(SOBC)
                                            //#Max He 09/11/2020: Surgical Glove(SGBC) regardless  of whether the QC type is straight pack or not
                                            //#Azrul 29/11/2021: HTLG_HSB_002: Special Glove (Clean Room Product) - PTPF Glove to follow Surgical logics.
                                            if (_resultDTO.QCTypeDescription.Trim() == Constants.STRAIGHT_PACK && (!isOnlineSurgicalGlove && !isPTPFGlove))
                                            {
                                                GlobalMessageBox.Show(Messages.PTQI_QCTYPE_SP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                                ClearForm();
                                                ResetReworkControls();
                                            }
                                            //#AZRUL 18/12/2019: Block SOBC if QCType is Straight Pack END
                                            else if (_resultDTO.BatchStartTime != null && _resultDTO.BatchEndTime == null)
                                            {
                                                GlobalMessageBox.Show(Messages.BATCH_IN_PROCESS, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                                ClearForm();
                                                ResetReworkControls();
                                            }
                                            else
                                            {
                                                if (_resultDTO.BatchStatus != Constants.SPLIT_BATCH)
                                                {
                                                    // On Serial No Rescan, Confirm from the user if its a rework

                                                    if (GlobalMessageBox.Show(Messages.REWORK_CONFIRMATION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.NO)
                                                    {
                                                        _isRework = false;
                                                        ClearForm();
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        _isRework = true;
                                                        ReasonControlsVisibility(true);
                                                    }
                                                }
                                                txtBatchNo.Text = _resultDTO.BatchNumber;
                                                txtGloveType.Text = _resultDTO.GloveType;
                                                txtSize.Text = _resultDTO.Size;
                                                txtQCType.Text = _resultDTO.QCTypeDescription;
                                                _qcType = _resultDTO.QCType;
                                                if (_isRework)
                                                {
                                                    txtReworkCount.Text = Convert.ToString(_resultDTO.QCReworkCount + Constants.ONE);
                                                }
                                                btnSave.Enabled = true;
                                                txtBatchStartDate.Text = CommonBLL.GetCurrentDateAndTime().ToString(ConfigurationManager.AppSettings["dateFormat"]);
                                                _startDate = CommonBLL.GetCurrentDateAndTime().ToString();
                                                if (QCPackingYieldBLL.GetQCGroupForWorkstation(WorkStationDTO.GetInstance().WorkStationId) != null)
                                                {
                                                    int shiftId = 0;
                                                    cmbQCGroup.SelectedValue = QCPackingYieldBLL.GetQCGroupForWorkstation(WorkStationDTO.GetInstance().WorkStationId);
                                                    List<DropdownDTO> lstShift = CommonBLL.GetShift(Framework.Constants.ShiftGroup.QC);
                                                    foreach (DropdownDTO objDrop in lstShift)
                                                    {
                                                        if (objDrop.DisplayField == objDrop.SelectedValue)
                                                        {
                                                            shiftId = Convert.ToInt32(objDrop.IDField);
                                                            break;
                                                        }
                                                    }

                                                    List<QCMemberDetailsDTO> qcMemberDetailsList = null;
                                                    qcMemberDetailsList = QCPackingYieldBLL.GetQCMemberDetailsForGroupAndShift(Convert.ToInt32(cmbQCGroup.SelectedValue), shiftId);
                                                    if (qcMemberDetailsList != null)
                                                    {
                                                        txtGroupMembers.Text = Convert.ToString(qcMemberDetailsList.Count);
                                                    }
                                                    else
                                                    {
                                                        txtGroupMembers.Text = string.Empty;
                                                    }

                                                }
                                                else
                                                {
                                                    cmbQCGroup.SelectedValue = Constants.MINUSONE;
                                                    txtGroupMembers.Text = string.Empty;
                                                }
                                            }
                                        }

                                        else
                                        {
                                            BatchDTO batchDTO = new BatchDTO();
                                            batchDTO = QCPackingYieldBLL.GetBatchScanInDetails(serialNumber);
                                            if (batchDTO == null)
                                            {
                                                GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                                ClearForm();
                                                ResetReworkControls();
                                            }
                                            //#AZRUL 10/11/2018: Block HBC if QCType is Straight Pack can not do Scan in batch card(SOBC)
                                            //#AZRUL 02/11/2020: Bypass SP checking for surgical
                                            //#Azrul 29/11/2021: HTLG_HSB_002: Special Glove (Clean Room Product) - PTPF Glove to follow Surgical logics.
                                            else if (batchDTO.BatchType.Trim() == Constants.PRODUCTION_TYPE && batchDTO.QCTypeDescription == Constants.STRAIGHT_PACK && (!isOnlineSurgicalGlove && !isPTPFGlove))
                                            {
                                                GlobalMessageBox.Show(Messages.PTQI_QCTYPE_SP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                                ClearForm();
                                                ResetReworkControls();
                                            }
                                            //#AZRUL 10/11/2018: Block SOBC if QCType is Straight Pack END
                                            //#AZRUL 18/12/2019: Block if PTQI Pass with QCType PT. START
                                            if (batchDTO.QCTypeDescription.Trim() == Constants.PT)
                                            {
                                                GlobalMessageBox.Show(Messages.PT_NOT_ALLOW, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                                ClearForm();
                                                ResetReworkControls();
                                            }
                                            //#AZRUL 18/12/2019: END
                                            else
                                            {

                                                txtBatchNo.Text = batchDTO.BatchNumber;
                                                txtGloveType.Text = batchDTO.GloveType;
                                                txtQCType.Text = batchDTO.QCTypeDescription;
                                                _qcType = batchDTO.QCType;
                                                txtSize.Text = batchDTO.Size;
                                                btnSave.Enabled = true;
                                                txtBatchStartDate.Text = CommonBLL.GetCurrentDateAndTime().ToString(ConfigurationManager.AppSettings["dateFormat"]);
                                                _startDate = CommonBLL.GetCurrentDateAndTime().ToString();
                                                if (QCPackingYieldBLL.GetQCGroupForWorkstation(WorkStationDTO.GetInstance().WorkStationId) != null)
                                                {
                                                    int shiftId = 0;
                                                    cmbQCGroup.SelectedValue = QCPackingYieldBLL.GetQCGroupForWorkstation(WorkStationDTO.GetInstance().WorkStationId);
                                                    List<DropdownDTO> lstShift = CommonBLL.GetShift(Framework.Constants.ShiftGroup.QC);
                                                    foreach (DropdownDTO objDrop in lstShift)
                                                    {
                                                        if (objDrop.DisplayField == objDrop.SelectedValue)
                                                        {
                                                            shiftId = Convert.ToInt32(objDrop.IDField);
                                                            break;
                                                        }
                                                    }

                                                    List<QCMemberDetailsDTO> qcMemberDetailsList = null;
                                                    qcMemberDetailsList = QCPackingYieldBLL.GetQCMemberDetailsForGroupAndShift(Convert.ToInt32(cmbQCGroup.SelectedValue), shiftId);
                                                    if (qcMemberDetailsList != null)
                                                    {
                                                        txtGroupMembers.Text = Convert.ToString(qcMemberDetailsList.Count);
                                                    }
                                                    else
                                                    {
                                                        txtGroupMembers.Text = string.Empty;
                                                    }

                                                }
                                                else
                                                {
                                                    cmbQCGroup.SelectedValue = Constants.MINUSONE;
                                                    txtGroupMembers.Text = string.Empty;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.QA_TEST_FAIL, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                    ResetReworkControls();
                                }

                                if (!string.IsNullOrEmpty(txtSerialNo.Text))
                                {
                                    BindQCTargetType();
                                }
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.TOTAL_PCS_IS_ZERO, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            ResetReworkControls();
                        }
                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "txtSerialNo_Leave", null);
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                }
            }
        }

        /// <summary>
        /// Functionality to be performed on save of data
        /// </summary>
        private void SaveData()
        {
            int rowsReturned = 0;

            if (string.Compare(_serialNumber, txtSerialNo.Text) != 0)
            {
                SerialNoLeaveFunctionality();
            }
            else
            {
                if (QCGroupValidations())
                {
                    if (ValidateRequiredFields())
                    {

                        QCYieldandPackingDTO objQCPackingYieldDTO = new QCYieldandPackingDTO();
                        try
                        {
                            using (var scope = new TransactionScope())
                            {
                                if (_isRework)
                                {
                                    objQCPackingYieldDTO.ReworkReasonId = Convert.ToString(cmbReworkReason.SelectedValue);
                                    objQCPackingYieldDTO.ReworkCount = Convert.ToString(txtReworkCount.Text.Trim());
                                }
                                else
                                {
                                    objQCPackingYieldDTO.ReworkCount = Convert.ToString(Constants.ZERO);
                                }

                                objQCPackingYieldDTO.SerialNumber = txtSerialNo.Text.Trim();
                                objQCPackingYieldDTO.QCGroupId = Convert.ToInt32(cmbQCGroup.SelectedValue);
                                objQCPackingYieldDTO.BatchStartTime = _startDate;
                                objQCPackingYieldDTO.QCGroupMembers = txtGroupMembers.Text.Trim();
                                objQCPackingYieldDTO.ModuleName = _moduleId;
                                objQCPackingYieldDTO.SubModuleName = _subModuleId;
                                objQCPackingYieldDTO.LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                                objQCPackingYieldDTO.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                                objQCPackingYieldDTO.GroupMemberCount = Convert.ToInt16(txtGroupMembers.Text.Trim());
                                objQCPackingYieldDTO.BatchTargetTime = _targetDate;
                                objQCPackingYieldDTO.ShiftId = QCPackingYieldBLL.GetShiftIdForGroup(Convert.ToInt16(cmbQCGroup.SelectedValue));
                                objQCPackingYieldDTO.QCType = _qcType;
                                objQCPackingYieldDTO.Brand = Convert.ToString(cmbBrand.SelectedValue);
                                objQCPackingYieldDTO.QCTargetTypeID = Convert.ToInt32(cmbQCTargetType.SelectedValue);

                                int qcID = 0;
                                rowsReturned = QCPackingYieldBLL.SaveQCYPScanInDetails(objQCPackingYieldDTO, ref qcID);
                                CommonBLL.InsertBatchAuditLog(Convert.ToDecimal(txtSerialNo.Text.Trim()), qcID, "QC", "Scan In Batch Card");
                                scope.Complete(); // commit if success
                            }
                        }
                        catch (FloorSystemException ex)
                        {
                            ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                        }
                        if (rowsReturned > 0)
                        {
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            ResetReworkControls();
                        }
                        else
                        {
                            //event log myadamas 20190227
                            EventLogDTO EventLog = new EventLogDTO();

                            EventLog.CreatedBy = String.Empty;
                            Constants.EventLog audAction = Constants.EventLog.Save;
                            EventLog.EventType = Convert.ToInt32(audAction);
                            EventLog.EventLogType = Constants.eventlogtype;

                            CommonBLL.InsertEventLog(EventLog, _screenName, _subModuleId);

                            GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            ResetReworkControls();
                        }
                    }
                }
            }
        }

        private Boolean ValidateQCScanInSerialNumber(bool isClearForm)
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                if (txtSerialNo.Text.Length == Constants.SERIAL_LENGTH)
                {
                    decimal serialNumber = Convert.ToDecimal(txtSerialNo.Text.Trim());
                    string errorMessage = QCPackingYieldBLL.GetQCScanValidationErrorMessage(serialNumber);

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        GlobalMessageBox.Show(errorMessage, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        if (isClearForm)
                            ClearForm();
                    }
                    else
                    {
                        isValid = true;
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    if (isClearForm)
                        ClearForm();
                }
            }
            else
            {
                validationMesssageLst = new List<ValidationMessage>();
                validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No", ValidationType.Required));
                ValidateForm();
                if (isClearForm)
                    ClearForm();
            }

            return isValid;
        }

        #endregion

        #region Fill Sources

        private void BindPLGroup()
        {
            try
            {
                cmbPlant.BindComboBox(QCPackingYieldBLL.GetPlants(), true);
                if (cmbPlant.Items.Count > 0)
                {
                    cmbPlant.SelectedValue = WorkStationDTO.GetInstance().LocationId.ToString();
                    BindQCGroupByPlantId(WorkStationDTO.GetInstance().LocationId.ToString());
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindPLGroup", null);
                return;
            }
        }

        /// <summary>
        /// Binds the QC Group ComboBox
        /// </summary>
        private void BindQCGroup()
        {
            try
            {
                cmbQCGroup.BindComboBox(QCPackingYieldBLL.GetQCGroups(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindQCGroup", null);
                return;
            }
        }

        /// <summary>
        /// Binds the Rework Reason ComboBox
        /// </summary>
        private void BindReworkReason()
        {
            try
            {
                cmbReworkReason.BindComboBox(CommonBLL.GetReasons(Constants.REWORK_REASON_TYPE,
                                                                    Convert.ToString(Convert.ToInt16((Constants.Modules.QCYIELDANDPACKING)))), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindReworkReason", null);
                return;
            }
        }

        /// <summary>
        /// Binds the Brand ComboBox
        /// </summary>
        private void BindBrand()
        {
            try
            {
                cmbBrand.BindComboBox(QCPackingYieldBLL.GetBrands(txtSerialNo.Text.Trim()), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindBrand", null);
                return;
            }
        }


        /// <summary>
        /// Binds the Rework Reason ComboBox
        /// </summary>
        private void BindQCTargetType()
        {
            try
            {
                cmbQCTargetType.BindComboBox(QCPackingYieldBLL.GetQCTargetType(txtSerialNo.Text.Trim(), Constants.SCAN_IN_BATCHCARD), true);
                if (cmbQCTargetType.Items.Count > 0)
                {
                    cmbQCTargetType.SelectedIndex = 0;
                }
                else
                {
                    GlobalMessageBox.Show(Messages.QC_SCAN_IN_NOT_QCTYPE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                    ResetReworkControls();
                    return;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindQCTargetType", null);
                return;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender">Scan Batch card Page</param>
        /// <param name="e">On load event argument</param>
        private void ScanInBatchCard_Load(object sender, EventArgs e)
        {
            txtSerialNo.SerialNo();
            ReasonControlsVisibility(false);
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ScanInBatchCard_Load", null);
                return;
            }
        }

        /// <summary>
        /// Serial No text box leave event
        /// </summary>
        /// <param name="sender">Serial No text box</param>
        /// <param name="e">On leave event argument</param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
                if (ValidateQCScanInSerialNumber(true))
            {
                SerialNoLeaveFunctionality();
            }
        }

        /// <summary>
        /// Save button Click event
        /// </summary>
        /// <param name="sender">Save button</param>
        /// <param name="e">On click event argument</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateQCScanInSerialNumber(false))
                {
                    SaveData();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                return;
            }
        }

        /// <summary>
        /// Alerts the User for clearing the values of all the controls.If yes, clear all the values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CLEARMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
                ResetReworkControls();
            }
        }


        private void cmbQCGroup_Leave(object sender, EventArgs e)
        {
            QCGroupValidations();
        }

        private bool QCGroupValidations()
        {
            int shiftId;

            try
            {
                if (cmbQCGroup.SelectedIndex != Constants.MINUSONE)
                {
                    shiftId = QCPackingYieldBLL.GetShiftIdForGroup(Convert.ToInt16(cmbQCGroup.SelectedValue));
                    if (shiftId == Constants.ZERO)
                    {
                        GlobalMessageBox.Show(Messages.MINIMUM_MEMBER_COUNT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        cmbQCGroup.SelectedIndex = Constants.MINUSONE;
                        cmbQCGroup.Focus();
                        txtGroupMembers.Text = string.Empty;
                        return false;
                    }
                    List<QCMemberDetailsDTO> qcMemberDetailsList = null;
                    qcMemberDetailsList = QCPackingYieldBLL.GetQCMemberDetailsForGroupAndShift(Convert.ToInt32(cmbQCGroup.SelectedValue), shiftId);


                    if (qcMemberDetailsList == null)
                    {
                        GlobalMessageBox.Show(Messages.MINIMUM_MEMBER_COUNT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        cmbQCGroup.SelectedIndex = Constants.MINUSONE;
                        cmbQCGroup.Focus();
                        txtGroupMembers.Text = string.Empty;
                        return false;
                    }
                    else
                    {
                        txtGroupMembers.Text = Convert.ToString(qcMemberDetailsList.Count);
                    }

                    //#AZRUL 02/11/2020: Bypass SP checking for surgical
                    //#Azrul 29/11/2021: HTLG_HSB_002: Special Glove (Clean Room Product) - PTPF Glove to follow Surgical logics.
                    if (!SurgicalGloveBLL.IsOnlineSurgicalGlove(Convert.ToDecimal(txtSerialNo.Text.Trim())) || !WasherBLL.CheckIsPTPFGlove(Convert.ToDecimal(txtSerialNo.Text.Trim())))
                    {
                        List<DropdownDTO> qctypelst = CommonBLL.GetQCType();
                        string Qctype = (from qc in qctypelst
                                     where qc.IDField.Trim().ToLower() == Constants.STRAIGHT_PACK.ToString().Trim().ToLower()
                                     select qc.DisplayField).FirstOrDefault();

                        _groupType = QCPackingYieldBLL.GetGroupTypeById(Convert.ToInt16(cmbQCGroup.SelectedValue));

                        if (_qcType == Qctype)
                        {
                            if (_groupType != Constants.PACKING_GROUP_TYPE)
                            {
                                GlobalMessageBox.Show(string.Format(Messages.SELECT_CORRECT_GROUP, Constants.QCPACKING_GROUP_TYPE_DISPLAY, Constants.PACKING_GROUP_TYPE),
                                                       Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                                cmbQCGroup.SelectedIndex = Constants.MINUSONE;
                                cmbQCGroup.Focus();
                                txtGroupMembers.Text = string.Empty;
                                return false;
                            }
                        }
                        else
                        {
                            if (string.Compare(_groupType, Constants.PACKING_GROUP_TYPE) == Constants.ZERO)
                            {
                                GlobalMessageBox.Show(string.Format(Messages.SELECT_CORRECT_GROUP, Constants.PACKING_GROUP_TYPE, Constants.QCPACKING_GROUP_TYPE_DISPLAY), Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                                cmbQCGroup.SelectedIndex = Constants.MINUSONE;
                                cmbQCGroup.Focus();
                                txtGroupMembers.Text = string.Empty;
                                return false;
                            }
                        }
                    }
                }

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "cmbQCGroup_Leave", null);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Leave event for Brand combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBrand_Leave(object sender, EventArgs e)
        {
            try
            {
                txtBatchTargetDate.Text = string.Empty;
                if (cmbQCGroup.SelectedIndex == Constants.MINUSONE)
                {
                    validationMesssageLst = new List<ValidationMessage>();
                    validationMesssageLst.Add(new ValidationMessage(cmbQCGroup, "Group ID", ValidationType.Required));
                    ValidateForm();
                }
                else
                {
                    if (cmbBrand.SelectedIndex == Constants.MINUSONE && _groupType == Constants.PACKING_GROUP_TYPE)
                    {
                        validationMesssageLst = new List<ValidationMessage>();
                        validationMesssageLst.Add(new ValidationMessage(cmbBrand, "Brand", ValidationType.Required));
                        ValidateForm();
                    }
                    else if(cmbQCTargetType.SelectedValue != null)
                    {
                        if (_resultDTO != null)
                        {
                            _targetDate = Convert.ToString(QCPackingYieldBLL.GetBatchTargetTime(txtSerialNo.Text.Trim(),
                                                                                       Convert.ToInt16(txtGroupMembers.Text), Convert.ToDateTime(_startDate),
                                                                                       Convert.ToInt16(cmbQCGroup.SelectedValue),
                                                                                       Convert.ToString(cmbBrand.SelectedValue),
                                                                                        _resultDTO.BatchStatus,
                                                                                       _resultDTO.InnerBoxCount, _resultDTO.PackingSize,
                                                                                       Convert.ToInt16(cmbQCTargetType.SelectedValue)
                                                                                       ));

                        }
                        else
                        {
                            _targetDate = Convert.ToString(QCPackingYieldBLL.GetBatchTargetTime(txtSerialNo.Text.Trim(),
                                                                                       Convert.ToInt16(txtGroupMembers.Text), Convert.ToDateTime(_startDate),
                                                                                       Convert.ToInt16(cmbQCGroup.SelectedValue), Convert.ToString(cmbBrand.SelectedValue),
                                                                                       string.Empty, Constants.ZERO,Constants.ZERO, Convert.ToInt16(cmbQCTargetType.SelectedValue)));
                        }

                        if (!string.IsNullOrEmpty(_targetDate))
                        {
                            txtBatchTargetDate.Text = Convert.ToDateTime(_targetDate).ToString(ConfigurationManager.AppSettings["dateFormat"]);
                        }
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "cmbBrand_Leave", null);
                return;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBrand.Text != string.Empty)
                {
                    txtBrandName.Text = QCPackingYieldBLL.GetBrandNameByBrand(cmbBrand.Text);
                }
                else
                { txtBrandName.Text = string.Empty; }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindBrandName", null);
                return;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cmbPlant_Leave(object sender, EventArgs e)
        {
            if (cmbPlant.SelectedValue != null && !string.IsNullOrEmpty(cmbPlant.SelectedValue.ToString()))
            {
                BindQCGroupByPlantId(cmbPlant.SelectedValue.ToString());
            }
        }

        private void BindQCGroupByPlantId(string PlantId)
        {
            try
            {
                cmbQCGroup.BindComboBox(QCPackingYieldBLL.GetQCGroupsByPlantId(PlantId), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindQCGroupByPlantId", null);
                return;
            }
        }

        private void cmbQCTargetType_Leave(object sender, EventArgs e)
        {
            cmbBrand_Leave(sender, e);
        }
    }
    #endregion
}
