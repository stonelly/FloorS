#region using

using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.IntegrationServices;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
#endregion


namespace Hartalega.FloorSystem.Windows.UI.QAI
{
    /// <summary>
    /// QAI Change QC Type
    /// </summary>
    public partial class QAIChangeQCType : QAIBase
    {
        #region  private Variables

        private static string _screenName = Constants.CHANGE_QC_TYPE;
        private static string _className = "QAIChangeQCType";
        //private static string _screenNameForAuthorization = "Change QC Type";
        private QAIDTO _qaidto;
        private decimal _tenPicWeight;
        private int _qAIExpiryDays = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intQAIExpiryDuration);
        private bool _iSchangeQctypeValidation = Convert.ToBoolean(FloorSystemConfiguration.GetInstance().bool_ISchangeQctypeValidation);
        private List<DropdownDTO> _qCType = null;
        private static BatchDTO _batchdto;
        private int authorizedBy;
        #endregion

        #region Load Form

        public QAIChangeQCType()
        {
            InitializeComponent();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "QAIChangeQCType", null);
            }
            this.Text = Constants.CHANGE_QC_TYPE;
            groupBox1.Text = Constants.CHANGE_QC_TYPE;
            btnSave.Enabled = false;
            BindQcType();
            BindChangeReason();
            txtQaiInspectorId.OperatorId();
            txtSerialNo.SerialNo();
            cmbChangeQCType.QcTypeCombo();
        }

        private void QAIChangeQCType_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Clear all the editable fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string result = GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE_QAI, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (result == Constants.YES)
            {
                ClearScreen();
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
                        if (SecurityModuleBLL.ValidateOperatorAccess(txtQaiInspectorId.Text.Trim(), Constants.CHANGE_QC_TYPE))
                        {
                            ISPageValid = true;
                            txtQaiInspectorName.Text = operatorName;
                            txtQaiInspectorName.Enabled = false;
                            txtSerialNo.Focus();
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
                        //GlobalMessageBox.Show(Messages.EX_GENERIC_EXCEPTION_INVALID_OPERATORID_QAI, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning)
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

        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            ISPageValid = false;
            btnSave.Enabled = false;
            txtBatchNo.Text = string.Empty;
            txtQCType.Text = string.Empty;
            txt10PcsGms.Text = string.Empty;
            txtGloveType.Text = string.Empty;
            txtSize.Text = string.Empty;
            txtWeight.Text = string.Empty;
            if (!string.IsNullOrEmpty(txtSerialNo.Text))
            {
                //Added by Tan Wei Wah 20190312
                if (CommonBLL.GetDowngradeStatusFromDB(txtSerialNo.Text.Trim()))
                {
                    GlobalMessageBox.Show(Messages.DOWNGRADE_VALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearScreen();
                    return;
                }
                //Ended by Tan Wei Wah 20190312

                QAIDTO qaiBatach = null;
                qaiBatach = CommonBLL.GetBatchNumberBySerialNumber(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                if (qaiBatach != null && string.IsNullOrEmpty(qaiBatach.BatchNo))
                {
                    GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                    txtSerialNo.Text = string.Empty;
                    txtSerialNo.Focus();
                    return;
                }
                else
                {
                    if (FinalPackingBLL.isSecondGradeBatch(Convert.ToDecimal(txtSerialNo.Text.Trim())))
                    {
                        GlobalMessageBox.Show(Messages.SECONDGRADE_SNO_FP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                    }
                    else if (!qaiBatach.QAIDate.HasValue)
                    {
                        GlobalMessageBox.Show(Messages.QAI_NOT_COMPLETED, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtBatchNo.Text = string.Empty;
                        txtQCType.Text = string.Empty;
                        txt10PcsGms.Text = string.Empty;
                        txtGloveType.Text = string.Empty;
                        txtSize.Text = string.Empty;
                        txtWeight.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else if (CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(qaiBatach.SerialNo)) == Messages.QAI_EXPIRED)
                    {
                        GlobalMessageBox.Show(Messages.QAI_EXPIRED, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtBatchNo.Text = string.Empty;
                        txtQCType.Text = string.Empty;
                        txt10PcsGms.Text = string.Empty;
                        txtGloveType.Text = string.Empty;
                        txtSize.Text = string.Empty;
                        txtWeight.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else if (!(QAIBLL.GetPTStatusForPWT(Convert.ToInt64(txtSerialNo.Text.Trim()))))
                    {
                        GlobalMessageBox.Show(Messages.QAI_PWT_PT_NOT_COMPLETED, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtBatchNo.Text = string.Empty;
                        txtQCType.Text = string.Empty;
                        txt10PcsGms.Text = string.Empty;
                        txtGloveType.Text = string.Empty;
                        txtSize.Text = string.Empty;
                        txtWeight.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. START
                    else if (CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerialNo.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPI) &&
                            !CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerialNo.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPO))
                    {
                        GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerialNo.Text = string.Empty;
                        txtBatchNo.Text = string.Empty;
                        txtQCType.Text = string.Empty;
                        txt10PcsGms.Text = string.Empty;
                        txtGloveType.Text = string.Empty;
                        txtSize.Text = string.Empty;
                        txtWeight.Text = string.Empty;
                        txtSerialNo.Focus();
                    }
                    else if (QAIBLL.GetLastServiceName(Convert.ToDecimal(txtSerialNo.Text.Trim())) == CreateInvTransJournalFunctionidentifier.STPI)
                    {
                        GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerialNo.Text = string.Empty;
                        txtBatchNo.Text = string.Empty;
                        txtQCType.Text = string.Empty;
                        txt10PcsGms.Text = string.Empty;
                        txtGloveType.Text = string.Empty;
                        txtSize.Text = string.Empty;
                        txtWeight.Text = string.Empty;
                        txtSerialNo.Focus();
                    }
                    //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. END

                    else
                    {
                        ISPageValid = true;
                        txtBatchNo.Text = qaiBatach.BatchNo;
                        txtQCType.Text = qaiBatach.QCType;
                        _tenPicWeight = qaiBatach.TenPcsWeight;
                        txt10PcsGms.Text = Convert.ToString(qaiBatach.TenPcsWeight);
                        txtGloveType.Text = qaiBatach.GloveType;
                        txtSize.Text = qaiBatach.Size;
                        txtWeight.Text = Convert.ToString(qaiBatach.BatchWeight);
                        GloveType = qaiBatach.GloveType;
                        btnSave.Enabled = true;
                        cmbChangeQCType.Focus();
                    }
                }
            }
            else
            {
                txtSerialNo.Text = string.Empty;
                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_EMPTY, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                txtSerialNo.Focus();
            }
        }

        private void cmbChangeQCType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtChangeQCTypeDescription.Text = Convert.ToString(cmbChangeQCType.SelectedValue);
        }
        private void cmbChangeQCType_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbChangeQCType.Text))
            {
                txtChangeQCTypeDescription.Text = QAIBLL.GetQCtypeValue(_qCType, cmbChangeQCType.Text.Trim());
                if (string.IsNullOrEmpty(txtChangeQCTypeDescription.Text))
                {
                    cmbChangeQCType.Text = string.Empty;
                }
            }
            else
            {
                cmbChangeQCType.Text = string.Empty;
                txtChangeQCTypeDescription.Text = string.Empty;
            }
            ChangeQctypeValidation();
        }
        private void btnSave_Click(object sender, EventArgs e)
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
            if (!ChangeQctypeValidation())
            {
                return;
            }
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtQaiInspectorId, "QAI Inspector:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbChangeQCType, "Change QC Type:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbReason, "Reason:", ValidationType.Required));
            if (ValidateForm())
            {
                _qaidto = new QAIDTO();
                _qaidto.ScreenTitle = this.Text;
                _qaidto.InspectorId = txtQaiInspectorId.Text.Trim();
                _qaidto.SerialNo = txtSerialNo.Text.Trim();
                _qaidto.BatchNo = txtBatchNo.Text.Trim();
                _qaidto.QCType = Convert.ToString(cmbChangeQCType.Text).Trim();
                _qaidto.ChangeQCTypeReason = cmbReason.Text.Trim();
                _qaidto.BatchNo = txtBatchNo.Text.Trim();
                _qaidto.QaiInspectorName = txtQaiInspectorName.Text.Trim();
                _qaidto.TenPcsWeight = _tenPicWeight;
                List<QAIDefectType> defects = new List<QAIDefectType>();
                _qaidto.Defects = defects;

                // 2020-07-20 FX - Move Authentication Confirmation to Defect Summary and trigger during ScanQITestResult 
                // 2020-09-10 Pang YS - Enable QAIChangeQCType 
                //#AZRUL 18/07/2018: Rework order in change QC type, QAI Scan Inner box & QAI Resampling START
                List<DropdownDTO> qctypelst = CommonBLL.GetQCType();
                string STRAIGHT_PACK = (from qc in qctypelst
                                        where qc.IDField.Trim().ToLower() == Constants.STRAIGHT_PACK.ToString().Trim().ToLower()
                                        select qc.DisplayField).FirstOrDefault();
                bool isChangeQCType = false;

                // 2020-07-20 FX - Move Authentication Confirmation to Defect Summary and trigger during ScanQITestResult
                // 2020-09-10 Pang YS - Enable QAIChangeQCType 
                if (_qaidto.QCType != STRAIGHT_PACK)
                {
                    _batchdto = CommonBLL.GetReworkOrderDetails(Convert.ToDecimal(_qaidto.SerialNo), _qaidto.QCType);
                    if (_batchdto.TotalPcs > Constants.ZERO) //#AZRUL 04/1/2019: Only create rework if Rework qty > 0 
                    {
                        string result = GlobalMessageBox.Show(Messages.IS_CREATE_REWORK, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
                        if (result == Constants.YES)
                        {
                            Login _passwordForm = new Login(Constants.Modules.REWORKORDER, _screenName);
                            _passwordForm.ShowDialog();
                            if (!(string.IsNullOrEmpty(_passwordForm.Authentication)))
                            {
                                authorizedBy = Convert.ToInt32(_passwordForm.Authentication);
                                isChangeQCType = true;
                            }
                            else
                            {
                                ClearScreen();
                                return;
                            }
                        }
                        else
                        {
                            ClearScreen();
                            return;
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.REWORK_QTY_IS_ZERO, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                        ClearScreen();
                        return;
                    }
                }

                if (QAIBLL.QAIChangeQCTypeSave(_qaidto.SerialNo, _qaidto.InspectorId, _qaidto.QCType, _qaidto.ChangeQCTypeReason, WorkStationDTO.GetInstance().WorkStationId, isChangeQCType, authorizedBy) > 0)
                {
                    //2020-07-15 FX - Move Ax Posting Logic to Defect Summary and trigger during ScanQITestResult
                    // 2020-09-10 Pang YS - Enable QAIChangeQCType 
                    if (isChangeQCType)
                    {   
                        //#AZRUL 27/8/2018: If QC Type is OQC
                        if (txtChangeQCTypeDescription.Text.Trim().ToLower() != Constants.STRAIGHT_PACK.ToString().Trim().ToLower() && txtChangeQCTypeDescription.Text.Trim() != Constants.PT)
                        {
                            //#AZRUL 27/8/2018: Change QC Type - Handle Non SP to Non SP, not allow to create double rework order (If OQC to OQC - Block 2nd Rework)
                            //#AZRUL 03/1/2019: Bypass checking if batch already scan out (SOBC) 
                            if ((!(QAIBLL.GetPreviousReworkIsOQC(_qaidto.SerialNo))) || CommonBLL.ValidateAXPosting(Convert.ToDecimal(_qaidto.SerialNo), CreateRAFJournalFunctionidentifier.SOBC.ToString()))
                            {
                                QAIBLL.SaveReworkOrderData(_batchdto);
                            }
                        }
                        //#AZRUL 20221222 - Mantis 0010216: Web Admin Staging: RWKCR is posted before SPBC
                        //#AZRUL 20221222 - Mantis 0010216: Delete last RWKCR, new RWKCR will be created after SPBC. (PT to QC)
                        else if (txtChangeQCTypeDescription.Text.Trim() == Constants.PT && QAIBLL.GetLastServiceName(Convert.ToDecimal(txtSerialNo.Text.Trim())) == ReworkOrderFunctionidentifier.RWKCR)
                        {
                            AXPostingBLL.PostAXDataDeleteReworkOrderPTQIFailPT(txtSerialNo.Text.Trim());
                        }
                    }

                    //#AZRUL 20221222 - Mantis 0010216: Delete last RWKCR, new RWKCR will be created after SPBC. (NonSP to SP)
                    if (txtQCType.Text != cmbChangeQCType.Text && txtChangeQCTypeDescription.Text.ToLower() == Constants.STRAIGHT_PACK.ToString().Trim().ToLower() &&
                            QAIBLL.GetLastServiceName(Convert.ToDecimal(txtSerialNo.Text.Trim())) == ReworkOrderFunctionidentifier.RWKCR)
                    {
                        AXPostingBLL.PostAXDataDeleteReworkOrderPTQIFailPT(txtSerialNo.Text.Trim());
                    }

                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY_QAI, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    ClearScreen();
                }
            }
            //#AZRUL 18/07/2018: Rework order in change QC type, QAI Scan Inner box & QAI Resampling END
        }

        #endregion

        #region User Methods

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

                this.cmbChangeQCType.SelectedIndexChanged -= new EventHandler(cmbChangeQCType_SelectedIndexChanged);
                cmbChangeQCType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbChangeQCType.AutoCompleteSource = AutoCompleteSource.ListItems;
                _qCType = CommonBLL.GetQCType();
                cmbChangeQCType.BindComboBox(_qCType, true);
                this.cmbChangeQCType.SelectedIndexChanged += new EventHandler(cmbChangeQCType_SelectedIndexChanged);

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindQcType", null);
                return;
            }
        }

        private void BindChangeReason()
        {
            try
            {
                cmbReason.BindComboBox(CommonBLL.GetEditReasons(Constants.QAI_REASONTYPE, Constants.QAI_MODULENAME), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindVisaulTesttSamlSize", null);
                return;
            }
        }

        private void ClearScreen()
        {
            this.txtSerialNo.Leave -= new EventHandler(txtSerialNo_Leave);
            this.txtQaiInspectorId.Leave -= new EventHandler(txtQaiInspectorId_Leave);
            BindQcType();
            BindChangeReason();
            txtQaiInspectorId.Text = string.Empty;
            txtQaiInspectorName.Text = string.Empty;
            txtSerialNo.Text = string.Empty;
            txtBatchNo.Text = string.Empty;
            txtGloveType.Text = string.Empty;
            txtSize.Text = string.Empty;
            txt10PcsGms.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtQCType.Text = string.Empty;
            txtChangeQCTypeDescription.Text = string.Empty;
            cmbChangeQCType.SelectedIndex = -1;
            cmbReason.SelectedIndex = -1;
            txtQaiInspectorId.Focus();
            this.txtSerialNo.Leave += new EventHandler(txtSerialNo_Leave);
            this.txtQaiInspectorId.Leave += new EventHandler(txtQaiInspectorId_Leave);

        }

        private bool ChangeQctypeValidation()
        {
            bool isvalid = true;
            if (!string.IsNullOrEmpty(cmbChangeQCType.Text) && _iSchangeQctypeValidation)
            {
                if (QAIBLL.ChangeQctypeValidation(txtQCType.Text.Trim(), cmbChangeQCType.Text))
                {
                    isvalid = false;
                    GlobalMessageBox.Show(Messages.QAI_ChangeQctypeValidation, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                    cmbChangeQCType.Focus();
                }

                //#AZRUL 20221222 - Mantis 0010216: Block Change QCType PT to SP if SPBC already posted.
                if (QAIBLL.GetQCtypeDescription(txtQCType.Text.Trim()) == Constants.PT && 
                    txtChangeQCTypeDescription.Text.Trim().ToLower() == Constants.STRAIGHT_PACK.ToString().Trim().ToLower() &&
                    CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateInvTransJournalFunctionidentifier.SPBC.ToString()))
                {
                    isvalid = false;
                    GlobalMessageBox.Show(Messages.QAI_ChangeQctypeValidation, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                    cmbChangeQCType.Focus();
                }
            }
            return isvalid;
        }

        #endregion

    }
}
