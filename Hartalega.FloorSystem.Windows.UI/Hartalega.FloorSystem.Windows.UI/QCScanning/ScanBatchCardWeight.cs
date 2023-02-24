using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.QCScanning
{
    /// <summary>
    /// Module: QC Scanning
    /// Screen Name: Scan Batch Card (Weight)
    /// File Type: Code file
    /// </summary> 
    public partial class ScanBatchCardWeight : FormBase
    {
        #region Class Variables
        private bool _isRework; //For checking whether the batch comes for Rework or its the first time entry for the Batch. 
        private int _cntRework = Constants.ZERO;
        private int _qcGroupId;
        private string _startTime;
        private static string _screenName = "QC Scanning - ScanBatchCardWeight";
        private static string _className = "ScanBatchCardWeight";
        private string _subModuleName;
        private string _moduleId;
        private string _reasonId;
        private const string _screenNameForAuthorization = "QC Scanning - Scan Batch Card Weight";
        private static string _serialNo;
        private bool _pressSave;
        private decimal _totalPcs;
        // #MH 10/11/2016 1.n FDD:HLTG-REM-003
        // For call scale weigh interface 
        private string _size;
        private decimal _tenPcsWeight;
        //private decimal CalculatedSample { get; set; } //MH#2.n 23/12/2016
        // #MH 10/11/2016 1.n FDD:HLTG-REM-003
        #endregion

        #region Load Page
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanBatchCardWeight" /> class.
        /// </summary>
        public ScanBatchCardWeight()
        {
            InitializeComponent();
            try
            {
                _moduleId = CommonBLL.GetModuleId(Constants.QC_SCANNING_SYSTEM);
                _subModuleName = CommonBLL.GetSubModuleId(Constants.SCAN_BATCHCARD_WEIGHT);
                cmbPlant.BindComboBox(QCPackingYieldBLL.GetPlants(), true);
                cmbQCGroup.BindComboBox(QCPackingYieldBLL.GetQCGroups(), true);
                cmbBatchStatus.BindComboBox(CommonBLL.GetEnumText(Constants.BATCH_STATUS), true);

                if (cmbPlant.Items.Count > 0)
                {
                    cmbPlant.SelectedValue = WorkStationDTO.GetInstance().LocationId.ToString();
                    BindQCGroupByPlantId(WorkStationDTO.GetInstance().LocationId.ToString());
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ScanBatchCardWeight", string.Empty);
                return;
            }
        }

        /// <summary>
        /// Form load event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanBatchCardWeight_Load(object sender, EventArgs e)
        {
            ReasonControlsVisibility(false);
            btnSave.Enabled = false;
            this.ActiveControl = txtSerialNo;
            txtSerialNo.SerialNo();
            lblBatchWeight.Visible = false;

            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
            {
                txtBatchWeight.ReadOnly = true;
                txtBatchWeight.TabStop = false;
            }
            else
            {
                txtBatchWeight.ReadOnly = false;
                txtBatchWeight.TabStop = true;
            }
        }
        #endregion

        #region Fill Sources
        /// <summary>
        /// Get the Rework Reason data & Bind the Rework Reason ComboBox  
        /// </summary>
        private void BindReasons()
        {
            cmbReworkReason.BindComboBox(CommonBLL.GetReasons(Constants.REWORK_REASON_TYPE, Convert.ToString(Convert.ToInt16(Constants.Modules.QCYIELDANDPACKING))), true);
            if (!string.IsNullOrEmpty(_startTime))
                cmbReworkReason.SelectedValue = _reasonId;
            else
                cmbReworkReason.SelectedIndex = Constants.MINUSONE;
        }

        #endregion

        #region Event Handlers
        /// <summary>
        /// Checks whether Serial Number is valid and gets the Batch Details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            if (!_pressSave)
                ShowBatchDetails();
        }

        /// <summary>
        /// Validate & Save the Batch Card Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbQCGroup, "QC Group", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtBatchWeight, "Batch(Kg)", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbBatchStatus, "Batch Status", ValidationType.Required));

            //Added by Bijay on 12/11/2017------------------------------------------------------------------------------------
            validationMesssageLst.Add(new ValidationMessage(textLooseQty, "Loose Qty", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(textRejectionQty, "Rejection Qty", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(text2ndGradeQty, "2nd Grade Qty", ValidationType.Required));
            //----------------------------------------------------------------------------------------------------------------

            if (_isRework)
                validationMesssageLst.Add(new ValidationMessage(cmbReworkReason, "Rework Reason", ValidationType.Required));
            try
            {
                if (ValidateForm() && ValidateBatchWeightPassword())
                {
                    _pressSave = false;
                    if (_serialNo != txtSerialNo.Text)
                    {
                        ShowBatchDetails();
                    }
                    validationMesssageLst = new List<ValidationMessage>();
                    validationMesssageLst.Add(new ValidationMessage(cmbQCGroup, "QC Group", ValidationType.Required));
                    validationMesssageLst.Add(new ValidationMessage(txtBatchWeight, "Batch(Kg)", ValidationType.Required));
                    validationMesssageLst.Add(new ValidationMessage(cmbBatchStatus, "Batch Status", ValidationType.Required));
                    if (_isRework)
                        validationMesssageLst.Add(new ValidationMessage(cmbReworkReason, "Rework Reason", ValidationType.Required));
                    _pressSave = true;
                    if (!string.IsNullOrEmpty(txtSerialNo.Text) && ValidateForm())
                    {
                        SaveBatchCard();
                    }
                    _pressSave = false;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", string.Empty);
                return;
            }
        }

        /// <summary>
        ///  Alerts the User for clearing the values of all the controls.If yes, clear all the values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string confirm = GlobalMessageBox.Show(Messages.CLEARMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (confirm == Constants.YES)
            {
                ClearForm();
            }
        }
        #endregion

        #region User Methods
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
            if (visible && !_pressSave)
            {
                BindReasons();
            }
        }

        /// <summary>
        /// Saves the Scan Batch Card Details
        /// </summary>
        private void SaveBatchCard()
        {
            if (_totalPcs >= Convert.ToDecimal(Convert.ToDecimal(txtBatchWeight.Text) * Constants.THOUSAND * Constants.TEN / Convert.ToDecimal(txtTenPcs.Text)))
            {
                // Recheck functionality
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {
                    txtBatchWeight.Text = CommonBLL.GetBatchWeight().ToString("#,##0.00");
                    ValidateBatchWeight();

                }


                QCYieldandPackingDTO objQCYPStore = QCScanningBLL.GetQCDetails(Convert.ToDecimal(txtSerialNo.Text));
                string strValidate = String.Empty;
                QCYieldandPackingDTO objQCYPDTO = new QCYieldandPackingDTO();
                if (_isRework)
                    objQCYPDTO.ReworkReasonId = Convert.ToString(cmbReworkReason.SelectedValue);
                objQCYPDTO.ReworkCount = _cntRework.ToString();
                objQCYPDTO.SerialNumber = txtSerialNo.Text.Trim();
                objQCYPDTO.QCGroupId = _qcGroupId;
                objQCYPDTO.TenPcsWeight = txtTenPcs.Text;
                objQCYPDTO.BatchWeight = Convert.ToString(Convert.ToDecimal(txtBatchWeight.Text));
                objQCYPDTO.BatchStatus = cmbBatchStatus.SelectedValue.ToString();
                objQCYPDTO.LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                objQCYPDTO.QCGroupId = Convert.ToInt32(cmbQCGroup.SelectedValue);
                objQCYPDTO.ModuleName = _moduleId;
                objQCYPDTO.SubModuleName = _subModuleName;
                objQCYPDTO.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                if (!string.IsNullOrEmpty(_startTime))
                    objQCYPDTO.QCYieldAndPackinId = Constants.ONE;
                else
                    objQCYPDTO.QCYieldAndPackinId = Constants.ZERO;
                string qcGroupMembers = QCScanningBLL.CountQCGroupMembers(Convert.ToInt32(cmbQCGroup.SelectedValue));
                string[] groupMembers = qcGroupMembers.Trim(' ', ',').Split(new char[] { ',' });
                objQCYPDTO.QCGroupMembers = qcGroupMembers;
                objQCYPDTO.GroupMemberCount = groupMembers.Length;
                if (cmbBatchStatus.Text == "Split Batch" && !string.IsNullOrEmpty(_startTime))
                {
                    objQCYPDTO.BatchTargetTime = Convert.ToString(QCPackingYieldBLL.GetBatchTargetTime(txtSerialNo.Text.Trim(), groupMembers.Length, Convert.ToDateTime(_startTime), Constants.ZERO, String.Empty, cmbBatchStatus.Text));
                }
                if (cmbBatchStatus.Text != "Split Batch" && !string.IsNullOrEmpty(_startTime))
                {
                    objQCYPDTO.BatchTargetTime = objQCYPStore.BatchTargetTime;
                }
                List<DropdownDTO> lstShift = CommonBLL.GetShift(Framework.Constants.ShiftGroup.QC);
                foreach (DropdownDTO objDrop in lstShift)
                {
                    if (objDrop.DisplayField == objDrop.SelectedValue)
                    {
                        objQCYPDTO.ShiftId = Convert.ToInt32(objDrop.IDField);
                        break;
                    }
                }
                // #MH 10/11/2016 1.n FDD:HLTG-REM-003
                objQCYPDTO.LooseQty = Convert.ToInt32(textLooseQty.Text.Trim());
                objQCYPDTO.RejectionQty = Convert.ToInt32(textRejectionQty.Text.Trim());
                objQCYPDTO.SecondGradeQty = Convert.ToInt32(text2ndGradeQty.Text.Trim());
                // #MH 10/11/2016 1.n FDD:HLTG-REM-003
                QCScanningBLL.SaveQCScanBatchCardWeight(objQCYPDTO);

                //event log myadamas 20190227
                EventLogDTO EventLog = new EventLogDTO();

                EventLog.CreatedBy = String.Empty;
                Constants.EventLog audAction = Constants.EventLog.Save;
                EventLog.EventType = Convert.ToInt32(audAction);
                EventLog.EventLogType = Constants.eventlogtype;

                CommonBLL.InsertEventLog(EventLog, _screenName, _subModuleName);

                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                ClearForm();
            }
            else
            {
                GlobalMessageBox.Show(Messages.ACTUAL_WEIGHT_EXCEED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// Shows the Batch details based on Serial Number
        /// </summary>
        private void ShowBatchDetails()
        {

            if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                if (txtSerialNo.Text.Length == Constants.SERIAL_LENGTH)
                {
                    try
                    {
                        //Added by Tan Wei Wah 20190312
                        if (CommonBLL.GetDowngradeStatusFromDB(txtSerialNo.Text.Trim()))
                        {
                            GlobalMessageBox.Show(Messages.DOWNGRADE_VALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            return;
                        }
                        //Ended by Tan Wei Wah 20190312

                        //20201024 Azrul: Check for Online Surgical
                        if (SurgicalGloveBLL.IsOnlineSurgicalGlove(Convert.ToDecimal(txtSerialNo.Text.Trim())))
                        {
                            GlobalMessageBox.Show(Messages.SURGICAL_BLOCKED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            return;
                        }

                        string ptStatus = QCPackingYieldBLL.GetPTStatus(Convert.ToInt64(txtSerialNo.Text.Trim()));
                        if (string.Compare(ptStatus, Constants.PT_INCOMPLETE) == Constants.ZERO)
                        {
                            GlobalMessageBox.Show(Messages.PT_INCOMPLETE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                        }
                        else if (string.Compare(ptStatus, Constants.PTQI_INCOMPLETE) == Constants.ZERO)
                        {
                            GlobalMessageBox.Show(Messages.PTQI_INCOMPLETE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                        }
                        else
                        {

                            BatchDTO objBatch = QCScanningBLL.GetBatchDetails(Convert.ToDecimal(txtSerialNo.Text.Trim()), Constants.ZERO);
                            if (objBatch != null)
                            {
                                string qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                                if ((!string.IsNullOrEmpty(qaiStatus) && string.IsNullOrEmpty(objBatch.BatchStartTime)) || (!string.IsNullOrEmpty(qaiStatus) && qaiStatus != Messages.QAI_EXPIRED && !string.IsNullOrEmpty(objBatch.BatchStartTime)))
                                {
                                    GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                }
                                else
                                {
                                    if (qaiStatus == Messages.QAI_EXPIRED)
                                        GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    BindBatchDetails(objBatch);
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearForm();
                            }
                        }
                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "txtSerialNo_Leave", txtSerialNo.Text);
                        return;
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_EMPTY, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                ClearForm();
            }
        }

        /// <summary>
        /// Fill the Batch details
        /// </summary>
        /// <param name="objBatch"></param>
        private void BindBatchDetails(BatchDTO objBatch)
        {
            txtBatchNo.Text = objBatch.BatchNumber;
            txtGloveType.Text = objBatch.GloveType;
            txtSize.Text = objBatch.Size;
            txtQCType.Text = objBatch.QCType;
            txtQCTypeDesc.Text = objBatch.QCTypeDescription;
            btnSave.Enabled = true;
            _tenPcsWeight = objBatch.TenPcsWeight;// MH#1.n
            _size = objBatch.Size;
            // #MH 10/11/2016 1.n
            txtTenPcs.Text = objBatch.TenPcsWeight.ToString("#,##0.00");
            _serialNo = txtSerialNo.Text;
            _totalPcs = Math.Round(PostTreatmentBLL.GetPTLatestBatchWeight(Convert.ToDecimal(txtSerialNo.Text)) * Constants.THOUSAND * Constants.TEN / PostTreatmentBLL.GetPTLatestTenPcsWeight(Convert.ToDecimal(txtSerialNo.Text)), Constants.ZERO);
            if (objBatch.QCReworkCount >= Constants.ZERO || objBatch.QCGroupId > Constants.ZERO)
            {
                if (!string.IsNullOrEmpty(objBatch.BatchStartTime))
                    cmbQCGroup.SelectedValue = Convert.ToString(objBatch.QCGroupId);
                _startTime = objBatch.BatchStartTime;
                GetReworkCount(objBatch.QCReworkCount, Convert.ToString(objBatch.RejectReasonId));
            }
            else
            {
                int rework = QCScanningBLL.CountRework(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                if (rework >= Constants.ZERO)
                {
                    GetReworkCount(rework, string.Empty);
                }
                else
                {
                    ReasonControlsVisibility(false);
                    _cntRework = Constants.ZERO;
                    _isRework = false;
                }
            }
        }

        /// <summary>
        /// Gets the Rework count for the batch
        /// </summary>
        /// <param name="reworkCount"></param>
        private void GetReworkCount(int reworkCount, string reasonId)
        {
            if (reworkCount >= Constants.ZERO)
            {
                if (string.IsNullOrEmpty(_startTime))
                {
                    string confirm = GlobalMessageBox.Show(Messages.REWORK_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
                    if (confirm == Constants.NO)
                    {
                        _isRework = false;
                        ClearForm();
                    }
                    else
                    {

                        _isRework = true;
                        _cntRework = reworkCount + Constants.ONE;
                        ReasonControlsVisibility(true);
                        txtReworkCount.Text = Convert.ToString(reworkCount + Constants.ONE);
                    }
                }
                else
                {
                    if (reworkCount > Constants.ZERO)
                    {
                        _reasonId = reasonId;
                        _isRework = true;
                        _cntRework = reworkCount;
                        ReasonControlsVisibility(true);
                        txtReworkCount.Text = Convert.ToString(reworkCount);
                    }
                    else
                    {
                        ReasonControlsVisibility(false);
                        _cntRework = Constants.ZERO;
                        _isRework = false;
                    }
                }
            }
        }

        /// <summary>
        /// Clear the values of all the controls. 
        /// </summary>
        private void ClearForm()
        {
            txtSerialNo.Text = String.Empty;
            txtBatchNo.Text = String.Empty;
            txtGloveType.Text = String.Empty;
            txtSize.Text = String.Empty;
            txtQCType.Text = String.Empty;
            txtQCTypeDesc.Text = String.Empty;
            txtTenPcs.Text = String.Empty;
            txtBatchWeight.Text = String.Empty;
            txtReworkCount.Text = String.Empty;
            txtQCType.Text = String.Empty;
            cmbBatchStatus.SelectedIndex = Constants.MINUSONE;
            cmbQCGroup.SelectedIndex = Constants.MINUSONE;
            txtSerialNo.Focus();
            btnSave.Enabled = false;
            ReasonControlsVisibility(false);
            _startTime = string.Empty;
            lblBatchWeight.Visible = false;
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            BindReasons();
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
            else if (uiControl == "getBatchWeight")
                GlobalMessageBox.Show("(" + result + ") - " + Constants.BATCH_WEIGHT_ERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        private void txtBatchWeight_Leave(object sender, EventArgs e)
        {
            if (txtBatchWeight.Text != String.Empty)
            {
                //Modified by Lakshman 17 / 1 / 2018 for Negative input validation    
                decimal decim;
                if (Decimal.TryParse(txtBatchWeight.Text, out decim) && decim > 0)
                {
                    ValidateBatchWeight();
                    txtBatchWeight.Text = Convert.ToDouble(txtBatchWeight.Text).ToString("#,##0.00");
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_BATCH, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    txtBatchWeight.Text = String.Empty;
                    txtBatchWeight.Focus();
                }
            }           
        }

        private void txtBatchWeight_Enter(object sender, EventArgs e)
        {
            try
            {
                if (txtBatchWeight.Text == String.Empty)
                {
                    if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                    {
                        txtBatchWeight.ReadOnly = true;
                        txtBatchWeight.Text = CommonBLL.GetBatchWeight().ToString("#,##0.00");
                        txtBatchWeight_Leave(sender, e);
                    }
                    else
                    {
                        txtBatchWeight.ReadOnly = false;
                        txtBatchWeight.Focus();
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getBatchWeight", null);
                return;
            }
        }

        /// <summary>
        /// To validate Batch Weight
        /// </summary>
        /// <returns> //***TBC - Made editable for only Testing Purposes - To Comment</returns>
        private void ValidateBatchWeight()
        {
            WorkStationDataConfiguration data = WorkStationDataConfiguration.GetInstance();
            FloorSystemConfiguration Floordata = FloorSystemConfiguration.GetInstance();
            if (Convert.ToBoolean(data.LWeight) && (Convert.ToDouble(txtBatchWeight.Text) < Convert.ToDouble(Floordata.MinBatchWeight) || Convert.ToDouble(txtBatchWeight.Text) > Convert.ToDouble(Floordata.MaxBatchWeight)))
            {
                GlobalMessageBox.Show(Constants.BATCHWEIGHT_RANGE, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                lblBatchWeight.Visible = true;
            }
            else
                lblBatchWeight.Visible = false;
        }
        #endregion


        private void cmbQCGroup_Leave(object sender, EventArgs e)
        {
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
            {
                txtBatchWeight.Text = CommonBLL.GetBatchWeight().ToString("#,##0.00");
                ValidateBatchWeight();
                cmbBatchStatus.Focus();
            }
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

        /// <summary>
        /// Validate 10 Pcs Weight based on Glove Type and Size by getting base values from AX
        /// </summary>
        private bool ValidateBatchWeightPassword()
        {
            bool result = false;
            WorkStationDataConfiguration data = WorkStationDataConfiguration.GetInstance();
            FloorSystemConfiguration Floordata = FloorSystemConfiguration.GetInstance();
            if (Convert.ToBoolean(data.LWeight) && (Convert.ToDouble(txtBatchWeight.Text) < Convert.ToDouble(Floordata.MinBatchWeight) || Convert.ToDouble(txtBatchWeight.Text) > Convert.ToDouble(Floordata.MaxBatchWeight)))
            {
                Login passwordForm = new Login(Constants.Modules.QCSCANNINGSYSTEM, _screenNameForAuthorization, string.Empty, true, string.Format(Constants.BATCHWEIGHT_MESSAGE, txtBatchWeight.Text), true);
                passwordForm.Authentication = String.Empty;
                passwordForm.IsCancel = false;
                passwordForm.ShowDialog();
                if (passwordForm.Authentication != String.Empty)
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        private void cmbQCGroup_Validated(object sender, EventArgs e)
        {
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                cmbBatchStatus.Focus();
            else
                txtBatchWeight.Focus();
        }

        private void txtBatchWeight_Validated(object sender, EventArgs e)
        {
            cmbBatchStatus.Focus();
        }

        private void btnCancel_Validated(object sender, EventArgs e)
        {
            txtSerialNo.Focus();
        }

        #region #MH 10/11/2016 1.n FDD:HLTG-REM-003

        /// <summary>
        /// Validate 10 Pcs Weight based on Glove Type and Size by getting base values from AX
        /// </summary>
        private void ValidateTenPcsWeight(string BatchWeight)
        {
            try
            {
                TenPcsDTO weight = new TenPcsDTO();
                weight = CommonBLL.GetMinMaxTenPcsWeight(txtGloveType.Text, _size);
                WorkStationDataConfiguration data = WorkStationDataConfiguration.GetInstance();
                if (Convert.ToBoolean(data.LWeight) && weight.MinWeight != null)
                {
                    if (Convert.ToBoolean(data.LWeight) && (Convert.ToDouble(BatchWeight) < Convert.ToDouble(weight.MinWeight) || Convert.ToDouble(BatchWeight) > Convert.ToDouble(weight.MaxWeight)))
                    {
                        GlobalMessageBox.Show(Constants.BATCHWEIGHT_RANGE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ValidateTenPcsWeight", string.Empty);
                return;
            }
        }


        /// <summary>
        /// To validate Batch Weight
        /// </summary>
        /// <returns></returns>
        private void ValidateBatchWeight(string BatchWeight)
        {
            try
            {
                WorkStationDataConfiguration data = WorkStationDataConfiguration.GetInstance();
                FloorSystemConfiguration Floordata = FloorSystemConfiguration.GetInstance();
                if (Convert.ToBoolean(data.LWeight) && (Convert.ToDouble(BatchWeight) < Convert.ToDouble(Floordata.MinBatchWeight) || Convert.ToDouble(BatchWeight) > Convert.ToDouble(Floordata.MaxBatchWeight)))
                {
                    GlobalMessageBox.Show(Constants.BATCHWEIGHT_RANGE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ValidateBatchWeight", string.Empty);
                return;
            }
        }

        //MH#2.n 23/12/2016 Move logic to BLL to handle multiple scan in & scan out
        //MH#2.n Centralize logic to BLL, UI shoulde not handle business logic 
        //private void calculateCalculatedSample()
        //{
        //    decimal RejectionQty = 0;
        //    decimal LooseQty = 0;
        //    decimal SecGradeQty = 0;
        //    if (!string.IsNullOrEmpty(textRejectionQty.Text))
        //        RejectionQty = Convert.ToInt64(textRejectionQty.Text);
        //    if (!string.IsNullOrEmpty(textLooseQty.Text))
        //        LooseQty = Convert.ToInt64(textLooseQty.Text);
        //    if (!string.IsNullOrEmpty(text2ndGradeQty.Text))
        //        SecGradeQty = Convert.ToInt64(text2ndGradeQty.Text);
        //    CalculatedSample = _totalPcs - LooseQty - RejectionQty - SecGradeQty;
        //    if (CalculatedSample < 0)
        //        CalculatedSample = 0;
        //    //textRejectedSample.Text = CalculatedSample.ToString();
        //}

        private void textLooseQty_TextChanged(object sender, EventArgs e)
        {
            //calculateCalculatedSample();//MH#2.n
            //GlobalMessageBox.Show(textLooseQty.Text, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

        }

        private void textRejectionQty_TextChanged(object sender, EventArgs e)
        {
            //calculateCalculatedSample();
        }

        private void text2ndGradeQty_TextChanged(object sender, EventArgs e)
        {
            //calculateCalculatedSample();
        }

        private void textLooseQty_Leave(object sender, EventArgs e)
        {
            //Modified by Lakshman 22 / 1 / 2018 for Negative input validation   
          
            if (textLooseQty.Text != String.Empty)
            {
                int value = 0;
                if (Int32.TryParse(textLooseQty.Text, out value) && value >= 0 )
                {
                    textLooseQty.Text = Convert.ToDouble(textLooseQty.Text).ToString();
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_LOOSE_QTY, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    textLooseQty.Text = String.Empty;
                    textLooseQty.Focus();
                }
            }
        }
        private void textRejectionQty_Leave(object sender, EventArgs e)
        {
            //Modified by Lakshman 22 / 1 / 2018 for Negative input validation    
            if (textRejectionQty.Text != String.Empty)
            {
                //decimal decim;
                //if (Decimal.TryParse(textRejectionQty.Text, out decim) && decim >= 0)
                //{
                //    textRejectionQty.Text = Convert.ToDouble(textRejectionQty.Text).ToString();
                //}
                int value = 0;
                if (Int32.TryParse(textRejectionQty.Text, out value) && value >= 0)
                {
                    textRejectionQty.Text = Convert.ToDouble(textRejectionQty.Text).ToString();
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_REJECTION_QTY, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    textRejectionQty.Text = String.Empty;
                    textRejectionQty.Focus();
                }
            }
        }
        private void text2ndGradeQty_Leave(object sender, EventArgs e)
        {
            //Modified by Lakshman 22 / 1 / 2018 for Negative input validation    
            if (text2ndGradeQty.Text != String.Empty)
            {
                //decimal decim;
                //if (Decimal.TryParse(text2ndGradeQty.Text, out decim) && decim >= 0)
                //{
                //    text2ndGradeQty.Text = Convert.ToDouble(text2ndGradeQty.Text).ToString();
                //}
                int value = 0;
                if (Int32.TryParse(text2ndGradeQty.Text, out value) && value >= 0)
                {
                    text2ndGradeQty.Text = Convert.ToDouble(text2ndGradeQty.Text).ToString();
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_2NDGRADE_QTY, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    text2ndGradeQty.Text = String.Empty;
                    text2ndGradeQty.Focus();
                }
            }
        }

        private void chkBigScaleLooseQty_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBigScaleLooseQty.Checked)
            {
                chkSmallScaleLooseQty.Checked = !chkBigScaleLooseQty.Checked;
                textLooseQty.ReadOnly = true;
                textLooseQty.Text = "";
                GetTotalPcs(textLooseQty, false);
            }
            else
            {
                textLooseQty.ReadOnly = false;
            }
        }

        private void chkSmallScaleLooseQty_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSmallScaleLooseQty.Checked)
            {
                chkBigScaleLooseQty.Checked = !chkSmallScaleLooseQty.Checked;
                textLooseQty.ReadOnly = true;
                textLooseQty.Text = "";
                GetTotalPcs(textLooseQty, true);
            }
            else
            {
                textLooseQty.ReadOnly = false;
            }
        }

        private void chkBigScaleRejection_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBigScaleRejection.Checked)
            {
                chkSmallScaleRejection.Checked = !chkBigScaleRejection.Checked;
                textRejectionQty.ReadOnly = true;
                textRejectionQty.Text = "";
                GetTotalPcs(textRejectionQty, false);
            }
            else
            {
                textRejectionQty.ReadOnly = false;
            }
        }

        private void chkSmallScaleRejection_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSmallScaleRejection.Checked)
            {
                chkBigScaleRejection.Checked = !chkSmallScaleRejection.Checked;
                textRejectionQty.ReadOnly = true;
                textRejectionQty.Text = "";
                GetTotalPcs(textRejectionQty, true);
            }
            else
            {
                textRejectionQty.ReadOnly = false;
            }
        }

        private void chkBigScale2ndGrade_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBigScale2ndGrade.Checked)
            {
                chkSmallScale2ndGrade.Checked = !chkBigScale2ndGrade.Checked;
                text2ndGradeQty.ReadOnly = true;
                text2ndGradeQty.Text = "";
                GetTotalPcs(text2ndGradeQty, false);
            }
            else
            {
                text2ndGradeQty.ReadOnly = false;
            }
        }

        private void chkSmallScale2ndGrade_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSmallScale2ndGrade.Checked)
            {
                chkBigScale2ndGrade.Checked = !chkSmallScale2ndGrade.Checked;
                text2ndGradeQty.ReadOnly = true;
                text2ndGradeQty.Text = "";
                GetTotalPcs(text2ndGradeQty, true);
            }
            else
            {
                text2ndGradeQty.ReadOnly = false;
            }
        }

        private void GetTotalPcs(TextBox source, bool isSmallScale)
        {
            if (source.Text == String.Empty)
            {
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {
                    var weight = "";
                    decimal weightG = 0m;
                    if (isSmallScale)
                    {
                        weight = CommonBLL.GetTenPcsWeight().ToString("#,##0.00");
                        //ValidateTenPcsWeight(weight); //MH#3.n 29/12/2016 skip validate ten pieces weight
                        weightG = Convert.ToDecimal(weight);
                    }
                    else
                    {
                        weight = CommonBLL.GetBatchWeight().ToString("#,##0.00");
                        ValidateBatchWeight(weight);
                        var PallentinerWeight = Convert.ToDecimal(WorkStationDataConfiguration.GetInstance().dec_PallentinerWeight);
                        // add back Pallentiner Weight for 2nd grade
                        if (!string.IsNullOrEmpty(weight))
                            weightG = (Convert.ToDecimal(weight) + PallentinerWeight) * 1000;
                    }
                    //MessageBox.Show(weightG.ToString() + " G"); //debug
                    source.Text = (Math.Round(weightG * Constants.TEN / _tenPcsWeight, Constants.ZERO)).ToString();
                }
                else
                {
                    source.ReadOnly = false;
                    source.Focus();
                }
            }
        }

        #endregion


    }
}
