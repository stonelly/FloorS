using System;
using System.Linq;
using System.Windows.Forms;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Collections.Generic;
using Hartalega.FloorSystem.IntegrationServices;

namespace Hartalega.FloorSystem.Windows.UI.QCScanning
{
    /// <summary>
    /// Module: QC Scanning
    /// Screen Name: Scanbatchcard
    /// File Type: Code file
    /// </summary>
    public partial class ScanBatchCard : FormBase
    {
        #region Member variables
        private QCScanningDetails _resultDTO = null;
        private string _screenName = "QC Scanning - ScanBatchCard";
        private string _className = "ScanBatchCard";
        private string _moduleName = string.Empty;
        private string _moduleId;
        private string _subModuleId;
        private string _serialNo;
        private string _size;
        // #MH 10/11/2016 1.n FDD:HLTG-REM-003
        // For call scale weigh interface 
        private decimal _tenPcsWeight;
        private decimal HBCBatchTotalPcs;
        private decimal CalculatedSample { get; set; }

        private int _isNewModule;
        // #MH 10/11/2016 1.n FDD:HLTG-REM-003
        #endregion
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public ScanBatchCard(string moduleName)
        {
            InitializeComponent();
            _moduleName = moduleName;
            if (_moduleName == Constants.QC_PACKING_YIELD_SYSTEM)
            {
                _screenName = "QC Packing Yield - ScanBatchCard";
            }
            else
            {
                gbScanOutBatchCard.Text = Constants.QC_SCAN_BATCHCARD_INNER;
                this.Text = Constants.QC_SCAN_BATCHCARD_INNER;
            }
            try
            {
                BindPackInto();
                BindBatchStatus();
            }

            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ScanBatchCard_Load", null);
                return;
            }
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
            txtQCGroup.Text = string.Empty;
            txtGroupMembers.Text = string.Empty;
            txtInnerBox.Text = string.Empty;
            // #MH 10/11/2016 1.n FDD:HLTG-REM-003
            textLooseQty.Text = string.Empty;
            textRejectionQty.Text = string.Empty;
            text2ndGradeQty.Text = string.Empty;
            // #MH 10/11/2016 1.n FDD:HLTG-REM-003
            cmbPackingSize.SelectedIndex = Constants.MINUSONE;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ClearForm", null);
                return;
            }
            BindPackInto();
            BindBatchStatus();
            if (_moduleName == Constants.QC_SCANNING_SYSTEM)
            {
                cmbBatchStatus.SelectedIndex = Constants.MINUSONE;
            }
            else
            {
                cmbBatchStatus.SelectedValue = Constants.BATCH_STATUS_COMPLETE;
            }
            cmbBrand.SelectedIndex = Constants.MINUSONE;
            txtSerialNo.Focus();
            btnSave.Enabled = false;
        }

        /// <summary>
        /// Validate required fields
        /// </summary>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtInnerBox, "Inner Box", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbPackingSize, "Packing Size", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbBatchStatus, "Batch Status", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbPackingInto, "Pack Into", ValidationType.Required));
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
            ClearForm();
        }

        /// <summary>
        /// Functionality to be performed on save of data
        /// </summary>
        private void SaveData()
        {
            int rowsReturned = 0;

            try
            {
                if (string.Compare(_serialNo, txtSerialNo.Text) != 0)
                {
                    string qaiStatus = string.Empty;
                    if (string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
                    {
                        validationMesssageLst = new List<ValidationMessage>();
                        validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No", ValidationType.Required));
                        ValidateForm();
                        ClearForm();
                    }
                    else
                    {
                        Int64 serialNumber = Int64.Parse(txtSerialNo.Text.Trim());

                        if (QCPackingYieldBLL.GetBatchScanInDetails(serialNumber) != null)
                        {
                            _resultDTO = QCScanningBLL.ValidateAndGetDetailsBySerialNumber(serialNumber);

                            if (_resultDTO != null)
                            {
                                if ((_resultDTO.BatchEndTime == null && _resultDTO.BatchStartTime == null)
                                     || _resultDTO.BatchEndTime != null)
                                {
                                    GlobalMessageBox.Show(Messages.BATCH_SCANNED_OUT, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                    return;
                                }
                                else
                                {
                                    qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(serialNumber);
                                    if (string.Compare(qaiStatus, Messages.QAI_INCOMPLETE) == Constants.ZERO)
                                    {
                                        GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        ClearForm();
                                        return;
                                    }
                                    else
                                    {
                                        if (string.Compare(qaiStatus, Messages.QAI_EXPIRED) == Constants.ZERO)
                                        {
                                            GlobalMessageBox.Show(Messages.QAI_EXPIRY_INFORMATION, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        }
                                        txtBatchNo.Text = _resultDTO.BatchNumber;
                                        txtGloveType.Text = _resultDTO.GloveType;
                                        txtSize.Text = _resultDTO.Size;
                                        txtQCType.Text = _resultDTO.QCTypeDescription;
                                        txtQCGroup.Text = _resultDTO.QCGroup;
                                        txtGroupMembers.Text = _resultDTO.QCGroupMembers;
                                        cmbBrand.Text = _resultDTO.Brand;
                                        // #MH 10/11/2016 1.n FDD:HLTG-REM-003
                                        // For call scale weigh interface 
                                        txt10pcweight.Text = _resultDTO.TenPcsWeight;
                                        // #MH 10/11/2016 1.n
                                    }
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_NOT_SCANNEDIN, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearForm();
                                return;
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INVALID_DATA_MESSAGE + "Serial Number", Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            return;
                        }
                    }
                }
                if (_moduleName == Constants.QC_PACKING_YIELD_SYSTEM)
                {
                    _moduleId = CommonBLL.GetModuleId(Constants.QC_PACKING_YIELD_SYSTEM);
                    _subModuleId = CommonBLL.GetSubModuleId(Constants.QCYP_SCAN_OUT_BATCHCARD);
                }
                else
                {
                    _moduleId = CommonBLL.GetModuleId(Constants.QC_SCANNING_SYSTEM);
                    _subModuleId = CommonBLL.GetSubModuleId(Constants.QC_SCAN_OUT_BATCHCARD);
                }

                if (ValidateRequiredFields())
                {
                    //decimal totalPcs = Math.Round(PostTreatmentBLL.GetPTLatestBatchWeight(Convert.ToDecimal(txtSerialNo.Text)) * Constants.THOUSAND * Constants.TEN / PostTreatmentBLL.GetPTLatestTenPcsWeight(Convert.ToDecimal(txtSerialNo.Text)), Constants.ZERO);
                    // need to check total pieces on Batch table in D365 model
                    //decimal totalPcs = PostTreatmentBLL.GetBatchTotalPcs(Convert.ToDecimal(txtSerialNo.Text));

                    //#AZRUL 03-07-2018: Get latest TotalPcs for 2nd SOBC onwards.
                    //#AZRUL 05-03-2019: Get latest TotalPcs if batch done CBCI.
                    //if (CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.SOBC)
                    //    || CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreatePickingListFunctionidentifier.CBCI))
                    //{
                    //    totalPcs = PostTreatmentBLL.GetRemainingBatchTotalPcs(Convert.ToDecimal(txtSerialNo.Text)); 
                    //}

                    decimal totalPcs = FinalPackingBLL.GetBatchCapacityQCScanOut(Convert.ToDecimal(txtSerialNo.Text));

                    Int64 splitPcs = QCScanningBLL.GetSplitPcs(Convert.ToInt64(txtSerialNo.Text.Trim()));
                    QCYieldandPackingDTO objQCScanningDTO = new QCYieldandPackingDTO();
                    objQCScanningDTO.SerialNumber = txtSerialNo.Text.Trim();
                    objQCScanningDTO.QCGroupId = _resultDTO.QCGroupId;
                    objQCScanningDTO.QCGroupMembers = txtGroupMembers.Text.Trim();
                    objQCScanningDTO.BatchEndTime = Convert.ToString(CommonBLL.GetCurrentDateAndTime());
                    objQCScanningDTO.InnerBoxCount = Convert.ToInt64(txtInnerBox.Text.Trim());
                    objQCScanningDTO.BatchStatus = Convert.ToString(cmbBatchStatus.SelectedValue);
                    objQCScanningDTO.PackingSize = Convert.ToInt32(cmbPackingSize.SelectedValue);
                    objQCScanningDTO.PackInto = Convert.ToString(cmbPackingInto.SelectedValue);
                    objQCScanningDTO.ModuleName = _moduleId;
                    objQCScanningDTO.SubModuleName = _subModuleId;
                    objQCScanningDTO.LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                    objQCScanningDTO.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                    objQCScanningDTO.TenPcsWeight = _resultDTO.TenPcsWeight;
                    objQCScanningDTO.QCType = _resultDTO.QCType;
                    objQCScanningDTO.Brand = Convert.ToString(cmbBrand.SelectedValue);
                    // #MH 10/11/2016 1.n FDD:HLTG-REM-003
                    if (!string.IsNullOrEmpty(textLooseQty.Text.Trim()))
                        objQCScanningDTO.LooseQty = Convert.ToInt32(textLooseQty.Text.Trim());
                    if (!string.IsNullOrEmpty(textRejectionQty.Text.Trim()))
                        objQCScanningDTO.RejectionQty = Convert.ToInt32(textRejectionQty.Text.Trim());
                    else
                        objQCScanningDTO.RejectionQty = 0;
                    if (!string.IsNullOrEmpty(text2ndGradeQty.Text.Trim()))
                        objQCScanningDTO.SecondGradeQty = Convert.ToInt32(text2ndGradeQty.Text.Trim());
                    else
                        objQCScanningDTO.SecondGradeQty = 0;
                    
                    //if (FloorSystemConfiguration.GetInstance().IScalculatedLooseQty.HasValue && FloorSystemConfiguration.GetInstance().IScalculatedLooseQty.Value) //MH#2.n 23/12/2016
                    //    objQCScanningDTO.CalculatedLooseQty = Convert.ToInt32(CalculatedSample); //MH#2.n 23/12/2016
                    //else
                    //    objQCScanningDTO.RejectedSample = Convert.ToInt32(CalculatedSample); //MH#2.n 23/12/2016

                    // #MH 10/11/2016 1.n FDD:HLTG-REM-003


                    if (_isNewModule > 0) /*Split batch been handle during new scan in module*/
                    {
                        if ((Convert.ToInt64(txtInnerBox.Text.Trim()) * Convert.ToInt32(cmbPackingSize.SelectedValue)) > totalPcs)
                        {
                            GlobalMessageBox.Show(Messages.INCORRECT_TOTAL_PCS, Constants.AlertType.Warning,
                                Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtInnerBox.Text = string.Empty;
                            cmbPackingSize.SelectedIndex = Constants.MINUSONE;
                            txtInnerBox.Focus();
                            return;
                        }
                        else if ((((objQCScanningDTO.PackingSize * objQCScanningDTO.InnerBoxCount) + objQCScanningDTO.RejectionQty + objQCScanningDTO.SecondGradeQty)) > totalPcs)
                        {
                            GlobalMessageBox.Show(Messages.INCORRECT_SCANOUT_PCS, Constants.AlertType.Error,
                                Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            text2ndGradeQty.Focus();
                            return;
                        }
                        else if ((((objQCScanningDTO.PackingSize * objQCScanningDTO.InnerBoxCount) + objQCScanningDTO.RejectionQty + objQCScanningDTO.SecondGradeQty)) != totalPcs)
                        {
                            //If split batch, not need to equal to total pcs during scan in new module
                            if (Convert.ToString(cmbBatchStatus.SelectedValue) != Constants.SPLIT_BATCH)
                            {
                                GlobalMessageBox.Show(Messages.INCORRECT_SCANOUT_NEWMODULE_PCS,
                                    Constants.AlertType.Error,
                                    Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                text2ndGradeQty.Focus();
                                return;
                            }
                        }
                    }
                    else
                    {
                        if ((Convert.ToInt64(txtInnerBox.Text.Trim()) * Convert.ToInt32(cmbPackingSize.SelectedValue)) > totalPcs)
                        {
                            GlobalMessageBox.Show(Messages.INCORRECT_TOTAL_PCS, Constants.AlertType.Warning,
                                Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtInnerBox.Text = string.Empty;
                            cmbPackingSize.SelectedIndex = Constants.MINUSONE;
                            txtInnerBox.Focus();
                            return;
                        }
                        else if ((splitPcs + (Convert.ToInt64(txtInnerBox.Text.Trim()) * Convert.ToInt32(cmbPackingSize.SelectedValue))) > totalPcs)
                        {
                            GlobalMessageBox.Show(Messages.INCORRECT_SCANOUT_PCS, Constants.AlertType.Warning,
                                Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtInnerBox.Text = string.Empty;
                            cmbPackingSize.SelectedIndex = Constants.MINUSONE;
                            txtInnerBox.Focus();
                            return;
                        }
                        else
                        {
                            if (Convert.ToString(cmbBatchStatus.SelectedValue) != Constants.SPLIT_BATCH)
                            {
                                if (objQCScanningDTO.RejectionQty + objQCScanningDTO.SecondGradeQty <= 0)
                                {
                                    if (GlobalMessageBox.Show(Messages.CONFIRM_NO_REJECT_2G, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.NO)
                                    {
                                        text2ndGradeQty.Focus();
                                        return;
                                    }
                                }

                                // IF  Batch quantity less than 300 pcs glove, system will prompt an error and block user from reporting START
                                if ((totalPcs - (splitPcs + (objQCScanningDTO.PackingSize * objQCScanningDTO.InnerBoxCount) + objQCScanningDTO.RejectionQty + objQCScanningDTO.SecondGradeQty)) >= 300)
                                {
                                    GlobalMessageBox.Show(Messages.BATCH_MIN_IS_300, Constants.AlertType.Error,
                                        Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    text2ndGradeQty.Focus();
                                    return;
                                }
                            }

                            //#AZRUL 5/12/2018: When the summation of Good Qty, Reject Qty and Second Grade Qty are more than Batch quantity by 300 pcs glove, system will prompt an error and block user from reporting START
                            if ((((objQCScanningDTO.PackingSize * objQCScanningDTO.InnerBoxCount) + objQCScanningDTO.RejectionQty + objQCScanningDTO.SecondGradeQty) - totalPcs) >= 300)
                            {
                                GlobalMessageBox.Show(Messages.BATCH_MAX_IS_300, Constants.AlertType.Error,
                                    Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                text2ndGradeQty.Focus();
                                return;
                            }
                            else if (((splitPcs + (objQCScanningDTO.PackingSize * objQCScanningDTO.InnerBoxCount) + objQCScanningDTO.RejectionQty + objQCScanningDTO.SecondGradeQty) - totalPcs) >= 300)
                            {
                                GlobalMessageBox.Show(Messages.BATCH_MAX_IS_300, Constants.AlertType.Error,
                                    Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                text2ndGradeQty.Focus();
                                return;
                            }
                        }
                    }
                    //#AZRUL 5/12/2018 END

                    int splitBatchCount = QCScanningBLL.GetSplitBatchCount(objQCScanningDTO.SerialNumber);
                    if ((splitBatchCount == 1 && Convert.ToString(cmbBatchStatus.SelectedValue) != Constants.SPLIT_BATCH) ||
                        (splitBatchCount < 1))
                    {
                        rowsReturned = QCScanningBLL.SaveQCScanDetails(objQCScanningDTO);

                        if (rowsReturned > 0)
                        {
                            //event log myadamas 20190227
                            EventLogDTO EventLog = new EventLogDTO();

                            EventLog.CreatedBy = String.Empty;
                            Constants.EventLog audAction = Constants.EventLog.Save;
                            EventLog.EventType = Convert.ToInt32(audAction);
                            EventLog.EventLogType = Constants.eventlogtype;

                            CommonBLL.InsertEventLog(EventLog, _screenName, _subModuleId);

                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            ClearForm();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.BATCH_SPLIT, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        cmbBatchStatus.Focus();
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                return;
            }
        }

        #endregion

        #region Fill Sources
        /// <summary>
        /// Binds the Packing Size ComboBox
        /// </summary>
        private void BindPackingSize()
        {
            try
            {
                List<DropdownDTO> objListOrder = CommonBLL.GetEnumText(Constants.PACKING_SIZE).OrderBy(row => Int32.Parse(row.DisplayField)).ToList();
                cmbPackingSize.BindComboBox(objListOrder, true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindPackingSize", null);
                return;
            }
        }

        /// <summary>
        /// Binds the Batch Status ComboBox
        /// </summary>
        private void BindBatchStatus()
        {
            try
            {
                cmbBatchStatus.BindComboBox(CommonBLL.GetEnumText(Constants.BATCH_STATUS), true);
                if (_moduleName == Constants.QC_SCANNING_SYSTEM)
                {
                    cmbBatchStatus.SelectedIndex = Constants.MINUSONE;
                }
                else
                {
                    cmbBatchStatus.SelectedValue = Constants.BATCH_STATUS_COMPLETE;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindBatchStatus", null);
                return;
            }
        }

        /// <summary>
        /// Binds the Pack into ComboBox
        /// </summary>
        private void BindPackInto()
        {
            try
            {
                cmbPackingInto.BindComboBox(CommonBLL.GetEnumText(Constants.PACK_INTO), true);
                cmbPackingInto.SelectedValue = Constants.FG;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindPackInto", null);
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
        #endregion

        #region Event handlers

        /// <summary>
        /// Serial No text box leave event
        /// </summary>
        /// <param name="sender">Serial No text box</param>
        /// <param name="e">On leave event argument</param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            string qaiStatus = string.Empty;
            _serialNo = txtSerialNo.Text.Trim();
            if (string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                validationMesssageLst = new List<ValidationMessage>();
                validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No", ValidationType.Required));
                ValidateForm();
                ClearForm();
            }
            else
            {
                Int64 serialNumber = Int64.Parse(txtSerialNo.Text.Trim());

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

                    //20201024 Azrul: Check for Surgical Batch Card
                    if (SurgicalGloveBLL.IsSurgicalGlove(Convert.ToDecimal(txtSerialNo.Text.Trim())))
                    {
                        GlobalMessageBox.Show(Messages.SURGICAL_BLOCKED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearForm();
                        return;
                    }

                    //Fill Data Sources  

                    BindPackingSize();
                    BindBrand();

                    if (QCPackingYieldBLL.GetBatchScanInDetails(serialNumber) != null)
                    {
                        _resultDTO = QCScanningBLL.ValidateAndGetDetailsBySerialNumber(serialNumber);

                        if (_resultDTO != null)
                        {
                            if (!string.IsNullOrEmpty(_resultDTO.Brand))
                            {
                                cmbBrand.Enabled = false;
                            }
                            if ((_resultDTO.BatchEndTime == null && _resultDTO.BatchStartTime == null)
                                 || _resultDTO.BatchEndTime != null)
                            {
                                GlobalMessageBox.Show(Messages.BATCH_SCANNED_OUT, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                txtSerialNo.Text = string.Empty;
                                txtSerialNo.Focus();
                            }
                            else
                            {
                                qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(serialNumber);
                                if (string.Compare(qaiStatus, Messages.QAI_INCOMPLETE) == Constants.ZERO)
                                {
                                    GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                }
                                else
                                {
                                    if (string.Compare(qaiStatus, Messages.QAI_EXPIRED) == Constants.ZERO)
                                    {
                                        GlobalMessageBox.Show(Messages.QAI_EXPIRY_INFORMATION, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    }
                                    txtBatchNo.Text = _resultDTO.BatchNumber;
                                    txtGloveType.Text = _resultDTO.GloveType;
                                    txtSize.Text = _resultDTO.Size;
                                    txtQCType.Text = _resultDTO.QCTypeDescription;
                                    txtQCGroup.Text = _resultDTO.QCGroup;
                                    txtGroupMembers.Text = _resultDTO.QCGroupMembers;
                                    cmbBrand.Text = _resultDTO.Brand;
                                    btnSave.Enabled = true;
                                    // #MH 10/11/2016 1.n FDD:HLTG-REM-003
                                    // For call scale weigh interface 
                                    txt10pcweight.Text = _resultDTO.TenPcsWeight;
                                    _tenPcsWeight = Convert.ToDecimal(txt10pcweight.Text);
                                    _size = _resultDTO.Size;
                                    // #MH 10/11/2016 1.n
                                }
                            }

                            _isNewModule = _resultDTO.IsNewScanInModule;
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.SERIAL_NUMBER_NOT_SCANNEDIN, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            ClearForm();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearForm();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "txtSerialNo_Leave", null);
                    return;
                }
            }
        }

        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender">Scan Batch card Page</param>
        /// <param name="e">On load event argument</param>
        private void ScanBatchCard_Load(object sender, EventArgs e)
        {
            txtSerialNo.SerialNo();
            txtInnerBox.InnerBoxCount();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ScanBatchCard_Load", null);
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
                SaveData();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                return;
            }
        }

        private void textQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        #endregion         

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

        private void txtInnerBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInnerBox.Text))
                return;
            //HBCBatchTotalPcs = Math.Round(PostTreatmentBLL.GetPTLatestBatchWeight(Convert.ToDecimal(txtSerialNo.Text)) * Constants.THOUSAND * Constants.TEN / PostTreatmentBLL.GetPTLatestTenPcsWeight(Convert.ToDecimal(txtSerialNo.Text)), Constants.ZERO);//MH#2.n
            //calculateCalculatedSample();//MH#2.n
        }

        //MH#2.n 23/12/2016 Move logic to BLL to handle multiple scan in & scan out
        //MH#2.n Centralize logic to BLL, UI shoulde not handle business logic 
        //private void calculateCalculatedSample()
        //{
        //    if (cmbPackingSize.SelectedValue == null || string.IsNullOrEmpty(txtInnerBox.Text))
        //        return;
        //    var batchSize = Convert.ToDecimal(txtInnerBox.Text.Trim()) * Convert.ToDecimal(cmbPackingSize.SelectedValue);

        //    decimal RejectionQty = 0;
        //    decimal LooseQty = 0;
        //    decimal SecGradeQty = 0;
        //    if(!string.IsNullOrEmpty(textRejectionQty.Text))
        //        RejectionQty = Convert.ToInt64(textRejectionQty.Text);
        //    if(!string.IsNullOrEmpty(textLooseQty.Text))
        //        LooseQty = Convert.ToInt64(textLooseQty.Text);
        //    if(!string.IsNullOrEmpty(text2ndGradeQty.Text))
        //        SecGradeQty = Convert.ToInt64(text2ndGradeQty.Text);
        //    CalculatedSample = HBCBatchTotalPcs - batchSize - LooseQty - RejectionQty - SecGradeQty;
        //    if (CalculatedSample < 0)
        //        CalculatedSample = 0;
        //    //textRejectedSample.Text = CalculatedSample.ToString();
        //}

        private void textLooseQty_TextChanged(object sender, EventArgs e)
        {
            //calculateCalculatedSample();//MH#2.n
        }

        private void textRejectionQty_TextChanged(object sender, EventArgs e)
        {
            //calculateCalculatedSample();//MH#2.n
        }

        private void text2ndGradeQty_TextChanged(object sender, EventArgs e)
        {
            //calculateCalculatedSample();//MH#2.n
        }

        private void textLooseQty_Leave(object sender, EventArgs e)
        {
            //Modified by Lakshman 17 / 1 / 2018 for Negative input validation    

            if (textLooseQty.Text != String.Empty)
            {
                //decimal decim;
                //if (Decimal.TryParse(textLooseQty.Text, out decim) && decim >= 0)
                //{
                //    textLooseQty.Text = Convert.ToDouble(textLooseQty.Text).ToString();
                //}
                int value = 0;
                if (Int32.TryParse(textLooseQty.Text, out value) && value >= 0)
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
            //Modified by Lakshman 17 / 1 / 2018 for Negative input validation    
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
            //Modified by Lakshman 17 / 1 / 2018 for Negative input validation    
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
        private void chkByWeight_CheckedChanged(object sender, EventArgs e)
        {
            textLooseQty.ReadOnly
            = textRejectionQty.ReadOnly
            = text2ndGradeQty.ReadOnly
            = chkBigScaleLooseQty.Checked;
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

                        /*
                        // add back Pallentiner Weight for 2nd grade
                        if(!string.IsNullOrEmpty(weight))
                        var PallentinerWeight = Convert.ToDecimal(WorkStationDataConfiguration.GetInstance().dec_PallentinerWeight); 
                        weightG = (Convert.ToDecimal(weight) + PallentinerWeight) * 1000;
                         */ //comment off by cpkoo 17-April-2017 bug fix 

                        if (!string.IsNullOrEmpty(weight))
                        {
                            weightG = Convert.ToDecimal(weight) * 1000;
                        }

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
