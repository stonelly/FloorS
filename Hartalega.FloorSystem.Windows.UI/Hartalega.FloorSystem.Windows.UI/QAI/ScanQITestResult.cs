#region using

using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.IntegrationServices;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace Hartalega.FloorSystem.Windows.UI.QAI
{
    public partial class ScanQITestResult : QAIBase
    {

        #region private Variables

        private static string _screenName = Constants.QI_TEST_RESULT;
        private static string _className = "ScanQITestResult";
        private QAIDTO _qaidto;
        private decimal _tenPicWeight;
        private List<DropdownDTO> _qCType = null;
        private QAIDTO _current_qaidto;
        #endregion

        #region Public Variables
        public Constants.QAIPageTransition _QAITranistion { get; set; }
        #endregion

        #region Load Form

        public ScanQITestResult()
        {
            InitializeComponent();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ScanQITestResult", null);
            }
            this.Text = Constants.QI_TEST_RESULT;
            groupBox1.Text = Constants.QI_TEST_RESULT;
            btnNext.Enabled = false;
            BindQcType();
            txtQaiInspectorId.OperatorId();
            txtSerialNo.SerialNo();
            cmbQcType.QcTypeCombo();
            BindWaterTightSamlSize();
            BindVisaulTesttSamlSize();
            BindQITestReason();
            BindCustomerType();
            txtLocation.Text = WorkStationDTO.GetInstance().Location;
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanQITestResult_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Event Handlers

        private void txtQaiInspectorId_Leave(object sender, EventArgs e)
        {
            ISPageValid = false;
            string operatorName = string.Empty;
            if (!string.IsNullOrEmpty(txtQaiInspectorId.Text.Trim()))
            {
                try
                {
                    operatorName = CommonBLL.GetOperatorNameQAI((txtQaiInspectorId.Text).Trim());
                    if (!string.IsNullOrEmpty(operatorName))
                    {
                        if (SecurityModuleBLL.ValidateOperatorAccess(txtQaiInspectorId.Text.Trim(), Constants.QI_TEST_RESULT))
                        {
                            ISPageValid = true;
                            txtQaiInspectorName.Text = operatorName;
                            txtQaiInspectorName.Enabled = false;
                            //commented 20171127 MyAdamas due to shift+tab go to previous field function not working
                            cmbReason.Focus();
                        }
                        else
                        {
                            txtQaiInspectorId.Text = string.Empty;
                            GlobalMessageBox.Show(Messages.QAI_SCREEN_ACCESS_MSG, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                            txtQaiInspectorName.Text = string.Empty;
                            txtQaiInspectorId.Focus();
                        }
                    }
                    else
                    {
                        txtQaiInspectorId.Text = string.Empty;
                        GlobalMessageBox.Show(Messages.QAI_SCREEN_ACCESS_MSG, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                        //Defect 14771 to 14779 May change later hance commenting the code 
                        //GlobalMessageBox.Show(Messages.EX_GENERIC_EXCEPTION_INVALID_OPERATORID_QAI, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                        txtQaiInspectorName.Text = string.Empty;
                        txtQaiInspectorId.Focus();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "txtOperatorId_Leave", Convert.ToInt16(txtQaiInspectorId.Text));
                    return;
                }
            }
            else
            {
                txtQaiInspectorName.Text = string.Empty;
                GlobalMessageBox.Show(Messages.EX_GENERIC_EXCEPTION_INVALID_OPERATORID_QAIINSPECTOR, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                txtQaiInspectorId.Text = string.Empty;
                txtQaiInspectorId.Focus();
            }
        }

        private void cmbQcType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtQcTypeDescription.Text = Convert.ToString(cmbQcType.SelectedValue);            
        }
        private void cmbQcType_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbQcType.Text))
            {
                // #AZRUL 20180814: Bugs 1127 - Not allow PT pass to proceed to do FP START
                List<DropdownDTO> qctypelst = CommonBLL.GetQCType();
                string STRAIGHT_PACK = (from qc in qctypelst
                                        where qc.IDField.Trim().ToLower() == Constants.STRAIGHT_PACK.ToString().Trim().ToLower()
                                        select qc.DisplayField).FirstOrDefault();

                if (AXPostingBLL.GetPostingStage(Convert.ToDecimal(txtSerialNo.Text.Trim())) == Constants.PT_QI && cmbQcType.Text == STRAIGHT_PACK
                    && (!SurgicalGloveBLL.IsSurgicalGlove(Convert.ToDecimal(txtSerialNo.Text.Trim())))) // #AZRUL 20201117: Surgical Glove to allow QCType is PT
                {
                    GlobalMessageBox.Show(Messages.PTQI_QCTYPE_SP, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                    cmbQcType.Text = string.Empty;
                    txtQcTypeDescription.Text = string.Empty;
                    cmbQcType.Focus();
                }
                // itrf 20211118030833282487 Inactive SP Pass in Scan QI Test Result module prevent scan through barcode
                else if (txtQcTypeDescription.Text.Trim().ToLower() == Constants.STRAIGHT_PACK.ToString().Trim().ToLower())
                {
                    GlobalMessageBox.Show(Messages.SCANQITESTRESULT_QCTYPE_SP, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                    cmbQcType.Text = string.Empty;
                    txtQcTypeDescription.Text = string.Empty;
                    cmbQcType.Focus();
                }
                //added 14/2/2018 by myadamas to disable user to select failed if qc type was straight pack
                else
                {
                    txtQcTypeDescription.Text = QAIBLL.GetQCtypeValue(_qCType, cmbQcType.Text.Trim());
                }
                // #AZRUL 20180814: Bugs 1127 - Not allow PT pass to proceed to do FP END
                if (string.IsNullOrEmpty(txtQcTypeDescription.Text))
                {
                    cmbQcType.Text = string.Empty;
                }
            }
            else
            {
                cmbQcType.Text = string.Empty;
                txtQcTypeDescription.Text = string.Empty;
            }
        }

        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            ISPageValid = false;
            btnNext.Enabled = false;
            txtBatchNo.Text = string.Empty;
            txtCurrentQCType.Text = string.Empty;
            txtCurrentQcTypeDescription.Text = string.Empty;
            lblGlovecode.Text = string.Empty;
            if (!string.IsNullOrEmpty(txtSerialNo.Text))
            {
                //Added by Tan Wei Wah 20190312
                if (CommonBLL.GetDowngradeStatusFromDB(txtSerialNo.Text.Trim()))
                {
                    GlobalMessageBox.Show(Messages.DOWNGRADE_VALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                    return;
                }
                //Ended by Tan Wei Wah 20190312

                //Validate QI Reason
                string errorMessage = QAIBLL.ValidateQITestReason(Convert.ToDecimal(txtSerialNo.Text.Trim()), cmbReason.Text.Trim());
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    GlobalMessageBox.Show(errorMessage, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                    txtSerialNo.Text = string.Empty;
                    txtBatchNo.Text = string.Empty;
                    txtCurrentQCType.Text = string.Empty;
                    txtCurrentQcTypeDescription.Text = string.Empty;
                    lblGlovecode.Text = string.Empty;
                    cmbReason.Focus();
                    return;
                }

                // Max, 07/01/2019, avoid Scan QI Test without Scan In QC
                QITestResultStatusDto qiTestResultStatus = QAIBLL.Get_PT_Or_QC_QITestResult(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                if(!string.IsNullOrEmpty(qiTestResultStatus.Stage))
                {
                    if(qiTestResultStatus.Stage == Constants.PT_QI)
                    {
                        // pang comment - start
                        //// UAT Issue 84,  After PTQI give testing method 100 % V + LC for WT batch card, able to Scan QI Test without Scan In QC. 
                        //// If PTQI pass should go to QC scan in, floor system will block direct go QAI Scan Test Result screen again without Scan In QC
                        //if ((qiTestResultStatus.QIResult == Constants.PASS && qiTestResultStatus.QCScanInCount + qiTestResultStatus.PTScanInCount <= qiTestResultStatus.QITestResultCount)
                        //||
                        // pang comment - end

                        // UAT Issue 34,  Able to Scan QI Test again after QI Test Result Fail without Scan In QC.
                        // If PTQI fail should go back washer and dry process, floor system will block direct go QAI Scan Test Result screen again without Scan In QC
                        // Azrul 20201216: Surgical should allow 2nd PTQI fail during QCQI. 
                        // #Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product)
                        if (qiTestResultStatus.QIResult.ToUpper() == Constants.FAIL && qiTestResultStatus.PTScanInCount <= qiTestResultStatus.QITestResultCount 
                            && (!SurgicalGloveBLL.IsSurgicalGlove(Convert.ToDecimal(txtSerialNo.Text.Trim())) && !WasherBLL.CheckIsPTPFGlove(Convert.ToDecimal(txtSerialNo.Text.Trim()))))
                        {
                            GlobalMessageBox.Show(Messages.SERIAL_NUMBER_NOT_SCANNEDIN, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                            txtSerialNo.Focus();
                            txtSerialNo.Text = string.Empty;
                            txtSerialNo.Focus();
                            return;
                        }
                    }
                    if(qiTestResultStatus.Stage == Constants.QC_QI)
                    {
                        // Azrul 19/12/2019: Validate for QCQI Pass but latest QCType is PT.
                        if (qiTestResultStatus.QCType.Trim() == Constants.PT && qiTestResultStatus.QIResult == Constants.PASS && (qiTestResultStatus.WasherScanInCount == 0
                             || qiTestResultStatus.WasherScanInCount != qiTestResultStatus.DryerScanInCount || qiTestResultStatus.DryerScanInCount != qiTestResultStatus.PTScanInCount))
                        {
                            GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_PT_LOCATION, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                            txtSerialNo.Focus();
                            txtSerialNo.Text = string.Empty;
                            txtSerialNo.Focus();
                            return;
                        }
                        // UAT Issue 34,  Able to Scan QI Test again after QI Test Result Fail without Scan In QC.
                        // If QCQI fail, floor system will block direct go QAI Scan Test Result screen again without Scan In QC
                        else if (qiTestResultStatus.QIResult.ToUpper() == Constants.FAIL && qiTestResultStatus.QCScanInCount <= qiTestResultStatus.QITestResultCount)
                        {
                            GlobalMessageBox.Show(Messages.SERIAL_NUMBER_NOT_SCANNEDIN, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                            txtSerialNo.Focus();
                            txtSerialNo.Text = string.Empty;
                            txtSerialNo.Focus();
                            return;
                        }
                    }
                }
                // Azrul, 15/12/2020, HSB SIT Issue: avoid PTQI without Scan PT
                if (string.IsNullOrEmpty(qiTestResultStatus.Stage) && qiTestResultStatus.WasherScanInCount > 0 && qiTestResultStatus.PTScanInCount == 0)
                {
                    GlobalMessageBox.Show(Messages.PT_QC_NOT_COMPLETE, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                    txtSerialNo.Focus();
                    txtSerialNo.Text = string.Empty;
                    txtSerialNo.Focus();
                    return;
                }
                // Max, 07/01/2019 end

                QAIDTO qaiBatach = null;
                qaiBatach = CommonBLL.GetBatchNumberBySerialNumber(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                _current_qaidto = qaiBatach;
                if (qaiBatach != null && string.IsNullOrEmpty(qaiBatach.BatchNo))
                {
                    GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                    txtSerialNo.Focus();
                    txtSerialNo.Text = string.Empty;
                    //txtSerialNo.Focus();
                    cmbReason.Focus();
                    return;
                }
                else
                {
                    //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. START
                    //#Max He 18/8/2022 HSB special scenario : To cater Straight Pack(HBC) batch direct scan in to TOMS, is gap between NGC and HSB, NGC so far didn’t have scenario STPI after HBC 
                    if (CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerialNo.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPI) &&
                            !CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerialNo.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPO))
                    {
                        GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerialNo.Text = string.Empty;
                        txtBatchNo.Text = string.Empty;
                        txtCurrentQCType.Text = string.Empty;
                        txtCurrentQcTypeDescription.Text = string.Empty;
                        lblGlovecode.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    //#Azrul 24/8/2022 HSB special scenario : To cater Straight Pack(HBC) batch direct scan in to TOMS - additional scenario (HBC>STPI>STPO>STPI)
                    if (QAIBLL.GetLastServiceName(Convert.ToDecimal(txtSerialNo.Text.Trim())) == CreateInvTransJournalFunctionidentifier.STPI)
                    {
                        GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerialNo.Text = string.Empty;
                        txtBatchNo.Text = string.Empty;
                        txtCurrentQCType.Text = string.Empty;
                        txtCurrentQcTypeDescription.Text = string.Empty;
                        lblGlovecode.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    List<DropdownDTO> qctypelst = CommonBLL.GetQCType();
                    string SPQAType = (from qc in qctypelst
                                      where qc.IDField.Trim().ToLower() == Constants.STRAIGHT_PACK.ToString().Trim().ToLower()
                                      select qc.DisplayField).FirstOrDefault();

                    if (FinalPackingBLL.isSecondGradeBatch(Convert.ToDecimal(txtSerialNo.Text.Trim())))
                    {
                        GlobalMessageBox.Show(Messages.SECONDGRADE_SNO_FP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                    }
                    // if last qai is from ScanQITestResult, dont need to check qai date
                    else if (qaiBatach.ScreenName != Constants.QAIScreens.ScanQITestResult && !qaiBatach.QAIDate.HasValue)
                    {
                        GlobalMessageBox.Show(Messages.QAI_NOT_COMPLETED, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtBatchNo.Text = string.Empty;
                        txtCurrentQCType.Text = string.Empty;
                        txtCurrentQcTypeDescription.Text = string.Empty;
                        lblGlovecode.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else if (!(QAIBLL.GetPTStatusForPWT(Convert.ToInt64(txtSerialNo.Text.Trim()))))
                    {
                        GlobalMessageBox.Show(Messages.QAI_PWT_PT_NOT_COMPLETED, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtBatchNo.Text = string.Empty;
                        txtCurrentQCType.Text = string.Empty;
                        txtCurrentQcTypeDescription.Text = string.Empty;
                        lblGlovecode.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else if (string.IsNullOrEmpty(QAIBLL.GetQAIPostingStage(Convert.ToDecimal(txtSerialNo.Text.Trim()))))
                    {
                        //if (cmbReason.Text.Trim() == "Normal Rework" && qaiBatach.QCType == SPQAType)
                        if (qaiBatach.QCType == SPQAType && (cmbReason.Text.Trim() == "Normal Rework"  || (cmbReason.Text.Trim() == "PSI Rework" )))
                        {
                            ISPageValid = true;
                            txtBatchNo.Text = qaiBatach.BatchNo;
                            txtCurrentQCType.Text = qaiBatach.QCType;
                            _tenPicWeight = qaiBatach.TenPcsWeight;
                            txtCurrentQcTypeDescription.Text = (from qc in qctypelst
                                                                where qc.DisplayField.Trim() == qaiBatach.QCType.Trim()
                                                                select qc.IDField).FirstOrDefault();
                            GloveType = qaiBatach.GloveType;
                            lblGlovecode.Text = qaiBatach.GloveType;
                            btnNext.Enabled = true;
                        }
                        else
                        { 
                            if (qaiBatach.QCType == SPQAType && cmbReason.Text.Trim() != "Normal Rework")
                            {
                                GlobalMessageBox.Show(Messages.QI_TEST_REASON_NORMALREWORK_SP, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                                cmbReason.Focus();
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.QAI_EDIT_DEFECT, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                                cmbReason.Focus();
                            }

                            txtSerialNo.Text = string.Empty;
                            txtBatchNo.Text = string.Empty;
                            txtCurrentQCType.Text = string.Empty;
                            txtCurrentQcTypeDescription.Text = string.Empty;
                            lblGlovecode.Text = string.Empty;                            
                            return;
                        }
                    }
                    else if (QAIBLL.GetLastServiceName(Convert.ToDecimal(txtSerialNo.Text.Trim())) == CreateInvTransJournalFunctionidentifier.STPI)
                    {
                        GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerialNo.Text = string.Empty;
                        txtBatchNo.Text = string.Empty;
                        txtCurrentQCType.Text = string.Empty;
                        txtCurrentQcTypeDescription.Text = string.Empty;
                        lblGlovecode.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. END

                    else
                    {
                        ISPageValid = true;
                        txtBatchNo.Text = qaiBatach.BatchNo;
                        txtCurrentQCType.Text = qaiBatach.QCType;
                        _tenPicWeight = qaiBatach.TenPcsWeight;
                        txtCurrentQcTypeDescription.Text = (from qc in qctypelst
                                                            where qc.DisplayField.Trim() == qaiBatach.QCType.Trim()
                                                            select qc.IDField).FirstOrDefault();
                        GloveType = qaiBatach.GloveType;
                        lblGlovecode.Text = qaiBatach.GloveType;
                        btnNext.Enabled = true;
                    }
                }
            }
            else
            {
                txtSerialNo.Text = string.Empty;
                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_EMPTY, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                //txtSerialNo.Focus();
                cmbReason.Focus();
            }
        }

        # endregion

        #region User Methods

        /// <summary>
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
        }

        /// <summary>
        /// Bind Qc Type ComoBox
        /// </summary>
        private void BindQcType()
        {
            try
            {
                this.cmbQcType.SelectedIndexChanged -= new EventHandler(cmbQcType_SelectedIndexChanged);
                cmbQcType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbQcType.AutoCompleteSource = AutoCompleteSource.ListItems;
                _qCType = CommonBLL.GetQCType();
                // Inactive SP Pass in Scan QI Test Result module - itrf  20211118030833282487
                _qCType = _qCType.Where(p => p.DisplayField != "0006020001").ToList();

                cmbQcType.BindComboBox(_qCType, true);
                this.cmbQcType.SelectedIndexChanged += new EventHandler(cmbQcType_SelectedIndexChanged);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindQcType", null);
                return;
            }
        }

        /// <summary>
        /// Bind Water Tight Sample Size
        /// </summary>
        private void BindWaterTightSamlSize()
        {
            try
            {
                cmbwatertghtsampleSize.BindComboBox(QAIBLL.GetSamplingSize(Constants.WATER_TIGHT_SAMPLING_SIZE, Constants.QAIScreens.ScanQITestResult), true);
                cmbwatertghtsampleSize.Text = Constants.DEFAULT_WATER_TIGHT_SAMPLE_SIZE125;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindWaterTightSamlSize", null);
                return;
            }
        }

        /// <summary>
        /// Bind Visaul Test Sample Size
        /// #MH 3/11/2016 1.n FDD@NGC-REM-001-PT_V1.0
        /// </summary>
        private void BindVisaulTesttSamlSize()
        {
            try
            {
                cmbVisualtstsampleSize.BindComboBox(QAIBLL.GetSamplingSize(Constants.VISUAL_TEST_SAMPLING_SIZE, Constants.QAIScreens.ScanQITestResult), true); ;
                cmbVisualtstsampleSize.Text = Constants.DEFAULT_VISUAL_TEST_SAMPLE_SIZE80;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindVisaulTesttSamlSize", null);
                return;
            }
        }

        private void BindQITestReason()
        {
            try
            {
                //Bind QI Test Reason
                List<DropdownDTO> reasonList = new List<DropdownDTO>();
                reasonList = CommonBLL.GetEnumText(Constants.QITestReason);
                cmbReason.DataSource = reasonList;
                cmbReason.DisplayMember = "DisplayField";
                cmbReason.SelectedItem = "IDField";
                if (reasonList.Count > 0)
                {
                    DropdownDTO reason = reasonList.Where(x => x.DisplayField == "New").FirstOrDefault();
                    if (reason.IDField != null)
                    {
                        cmbReason.SelectedItem = reason.IDField;
                    }
                    else
                    {
                        cmbReason.SelectedIndex = -1;
                    }
                }
                else
                {
                    cmbReason.SelectedIndex = -1;
                }
                
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getQITestReason", null);
                return;
            }
        }

        private void BindCustomerType()
        {
            try
            {
                cmbCustomerType.BindComboBox(CommonBLL.GetEnumText(Constants.CUSTOMER_TYPE), true);
                cmbCustomerType.Text = Constants.DEFAULT_CUSTOMER_TYPE;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindCustomerType", null);
                return;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string result = GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE_QAI, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (result == Constants.YES)
            {
                ClearForm();
            }
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnCancel_Click", null);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            txtQaiInspectorId_Leave(txtQaiInspectorId, e);
            if (!ISPageValid)
            {
                return;
            }
            txtSerialNo_Leave(txtSerialNo, e);
            if (!ISPageValid)
            {
                return;
            }
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtQaiInspectorId, "QAI Inspector:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbQcType, "QC Type:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbwatertghtsampleSize, "Water Tight Sampling Size:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbVisualtstsampleSize, "Visual Test Sampling Size:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbResult, "QI Test Result:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbReason, "Reason:", ValidationType.Required));
            if (ValidateForm())
            {
                // #20220826: Azrul - New PTQI Pass/Fail with QC not allowed, will caused RWKCR/SOBC not generated.
                if (txtCurrentQcTypeDescription.Text.Trim() == Constants.PT && cmbReason.Text.Trim() == "New" && 
                    txtQcTypeDescription.Text.Trim().ToLower() != Constants.STRAIGHT_PACK.ToString().Trim().ToLower() && 
                    txtQcTypeDescription.Text.Trim() != Constants.PT)
                {
                    GlobalMessageBox.Show(Messages.INVALID_NEW_PAS_OR_FAIL_PTQI_QC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                    return;
                }

                // #20221222: Azrul - Block PTQI FAIL with QC if previous PTQI not PASS with PT.
                if (AXPostingBLL.GetPostingStage(Convert.ToDecimal(txtSerialNo.Text.Trim())) == Constants.PT_QI &&
                    QAIBLL.GetLastQIResult(Convert.ToDecimal(txtSerialNo.Text.Trim())).ToUpper() == Constants.FAIL && cmbResult.Text == "Fail" 
                    && txtQcTypeDescription.Text.Trim().ToLower() != Constants.STRAIGHT_PACK.ToString().Trim().ToLower() &&
                    txtQcTypeDescription.Text.Trim() != Constants.PT)
                {
                    GlobalMessageBox.Show(Messages.INVALID_NEW_SURGICAL_PTPF_NO_PTQI_PASS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                    return;
                }

                // #20221223: Azrul - Block any PTQI PASS with QC.
                if (AXPostingBLL.GetPostingStage(Convert.ToDecimal(txtSerialNo.Text.Trim())) == Constants.PT_QI && cmbResult.Text == "Pass"
                    && txtQcTypeDescription.Text.Trim().ToLower() != Constants.STRAIGHT_PACK.ToString().Trim().ToLower() &&
                    txtQcTypeDescription.Text.Trim() != Constants.PT)
                {
                    GlobalMessageBox.Show(Messages.INVALID_PTQI_PASS_QC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                    return;
                }

                // #20221116: Azrul/Suren - For surgical batch card/PTPF system allow doing PT-QI FAIL without doing PT-QI PASS 1st.
                // Remark = System should not allow the user to do PT-QI FAIL (QC testing method) without doing PT-QI PASS 1st.
                if ((WasherBLL.CheckIsPTPFGlove(Convert.ToDecimal(txtSerialNo.Text.Trim())) || SurgicalGloveBLL.IsSurgicalGlove(Convert.ToDecimal(txtSerialNo.Text.Trim()))) &&
                    cmbReason.Text.Trim() == "New" && txtQcTypeDescription.Text.Trim().ToLower() != Constants.STRAIGHT_PACK.ToString().Trim().ToLower() &&
                    txtQcTypeDescription.Text.Trim() != Constants.PT && cmbResult.Text == "Fail")
                {
                    GlobalMessageBox.Show(Messages.INVALID_NEW_SURGICAL_PTPF_NO_PTQI_PASS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                    return;
                }

                // #20221116: Azrul/Suren - QI system allows doing continuous PT-QI PASS, which the system should block.
                // If the previous QI test result is PASS, the system should only allow the next QI test result to FAIL only.
                if (QAIBLL.GetLastQIResult(Convert.ToDecimal(txtSerialNo.Text.Trim())) == Constants.PASS && cmbResult.Text == "Pass")
                {
                    GlobalMessageBox.Show(Messages.INVALID_QI_PASS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                    return;
                }

                string errorMessage = QAIBLL.ValidateQITestReason(Convert.ToDecimal(txtSerialNo.Text.Trim()), cmbReason.Text.Trim(), cmbResult.Text.Trim());
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    GlobalMessageBox.Show(errorMessage, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                    return;
                }
                // itrf 20211118030833282487 Inactive SP Pass in Scan QI Test Result module prevent scan through barcode
                else if (txtQcTypeDescription.Text.Trim().ToLower() == Constants.STRAIGHT_PACK.ToString().Trim().ToLower())
                {
                    GlobalMessageBox.Show(Messages.SCANQITESTRESULT_QCTYPE_SP, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                    cmbQcType.Text = string.Empty;
                    txtQcTypeDescription.Text = string.Empty;
                    cmbQcType.Focus();
                    return;
                }
                else
                {
                    _qaidto = new QAIDTO();
                    _qaidto.ScreenTitle = this.Text;
                    _qaidto.InspectorId = txtQaiInspectorId.Text.Trim();
                    _qaidto.SerialNo = txtSerialNo.Text.Trim();
                    _qaidto.BatchNo = txtBatchNo.Text.Trim();
                    _qaidto.QCType = cmbQcType.Text.Trim();
                    _qaidto.BatchNo = txtBatchNo.Text.Trim();
                    _qaidto.QaiInspectorName = txtQaiInspectorName.Text.Trim();
                    _qaidto.WTSamplingSize = cmbwatertghtsampleSize.Text.Trim();
                    _qaidto.VTSamplingSize = cmbVisualtstsampleSize.Text.Trim();// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
                    _qaidto.QAITestResult = cmbResult.Text;
                    _qaidto.QITestReason = cmbReason.Text;
                    _qaidto.TenPcsWeight = _tenPicWeight;
                    _qaidto.ScreenName = Constants.QAIScreens.ScanQITestResult;
                    _qaidto.GloveType = GloveType;
                    _qaidto.PackingSize = _current_qaidto.PackingSize;
                    _qaidto.InnerBoxes = _current_qaidto.InnerBoxes;
                    _qaidto.HBSamplingSize = _current_qaidto.HBSamplingSize;
                    _qaidto.Isonline = _current_qaidto.Isonline;
                    _qaidto.CustomerTypeId = cmbCustomerType.SelectedValue.ToString();

                    QAIScanDefect qaiscandefect = new QAIScanDefect(_qaidto);
                    _QAITranistion = qaiscandefect._DefectTranistion;
                    txtQaiInspectorId.Focus();
                    qaiscandefect.ShowDialog();
                    QAINavigation(qaiscandefect._DefectTranistion);
                }
            }
        }

        private void ClearForm()
        {
            this.txtSerialNo.Leave -= new EventHandler(txtSerialNo_Leave);
            this.txtQaiInspectorId.Leave -= new EventHandler(txtQaiInspectorId_Leave);
            BindQcType();
            BindWaterTightSamlSize();
            BindVisaulTesttSamlSize();
            BindQITestReason();
            BindCustomerType();
            txtQaiInspectorId.Text = string.Empty;
            txtQaiInspectorName.Text = string.Empty;
            txtSerialNo.Text = string.Empty;
            txtBatchNo.Text = string.Empty;
            txtCurrentQCType.Text = string.Empty;
            lblGlovecode.Text = string.Empty;
            txtQcTypeDescription.Text = string.Empty;
            txtCurrentQcTypeDescription.Text = string.Empty;
            txtQcTypeDescription.Text = string.Empty;
            cmbQcType.SelectedIndex = -1;
            cmbResult.SelectedIndex = -1;
            cmbwatertghtsampleSize.SelectedIndex = -1;
            cmbVisualtstsampleSize.SelectedIndex = -1;
            cmbVisualtstsampleSize.Text = Constants.DEFAULT_VISUAL_TEST_SAMPLE_SIZE80;
            cmbwatertghtsampleSize.Text = Constants.DEFAULT_WATER_TIGHT_SAMPLE_SIZE125;
            txtQaiInspectorId.Focus();
            btnNext.Enabled = false;
            this.txtSerialNo.Leave += new EventHandler(txtSerialNo_Leave);
            this.txtQaiInspectorId.Leave += new EventHandler(txtQaiInspectorId_Leave);
        }


        private void QAINavigation(Constants.QAIPageTransition qaitranistion)
        {
            if (qaitranistion == Constants.QAIPageTransition.FormClose)
            {
                this.Close();
            }

            if (qaitranistion == Constants.QAIPageTransition.FormEscape)
            {
                ClearForm();
            }
        }
        # endregion

    }
}
