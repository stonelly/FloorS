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
    /// <summary>
    /// QAI - Edit Defects
    /// </summary>
    public partial class EditDefects : QAIBase
    {
        #region private Variables

        private static string _screenName = Constants.EDIT_DEFECTS;
        private static string _className = "EditDefects";
        private QAIDTO _qaidto;
        private decimal _tenPicWeight;
        private int _qAIExpiryDays = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intQAIExpiryDuration);
        private List<DropdownDTO> _qCType = null;
        private QAIDTO _current_qaidto;
        #endregion

        #region Public Variables
        public Constants.QAIPageTransition _QAITranistion { get; set; }
        #endregion

        #region Load Form

        public EditDefects()
        {
            InitializeComponent();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "EditDefects", null);
            }
            this.Text = Constants.EDIT_DEFECTS;
            groupBox1.Text = Constants.EDIT_DEFECTS;
            btnNext.Enabled = false;
            BindQcType();
            BindWaterTightSamlSize();
            BindVisaulTesttSamlSize();
            BindChangeReason();
            BindCustomerType();
            txtQaiInspectorId.OperatorId();
            txtSerialNo.SerialNo();
            cmbQcType.QcTypeCombo();

            // hide QC Type & Description - MYAdamas
            tableLayoutPanel1.RowStyles[7].Height = 0;
            tableLayoutPanel1.RowStyles[8].Height = 0;
            label10.Visible = false;
            cmbQcType.Visible = false;
            label5.Visible = false;
            txtQcTypeDescription.Visible = false;
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditDefects_Load(object sender, EventArgs e)
        {

        }



        #endregion

        #region Event Handlers

        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            ISPageValid = false;
            btnNext.Enabled = false;
            txtBatchNo.Text = string.Empty;
            lblGloveCode.Text = string.Empty;
            txtCurrentQCType.Text = string.Empty;
            txtCurrentQcTypeDescription.Text = string.Empty;
            if (!string.IsNullOrEmpty(txtSerialNo.Text))
            {
                //Added by Tan Wei Wah 20190312
                if (CommonBLL.GetDowngradeStatusFromDB(txtSerialNo.Text.Trim()))
                {
                    GlobalMessageBox.Show(Messages.DOWNGRADE_VALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearData();
                    return;
                }
                //Ended by Tan Wei Wah 20190312

                QAIDTO qaiBatach = null;
                qaiBatach = CommonBLL.GetBatchNumberBySerialNumber(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                _current_qaidto = qaiBatach;

                if (qaiBatach != null && string.IsNullOrEmpty(qaiBatach.BatchNo))
                {
                    GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    txtSerialNo.Text = string.Empty;
                    txtSerialNo.Focus();
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

                        GlobalMessageBox.Show(Messages.QAI_NOT_COMPLETED, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        txtBatchNo.Text = string.Empty;
                        txtCurrentQCType.Text = string.Empty;
                        txtCurrentQcTypeDescription.Text = string.Empty;
                        lblGloveCode.Text = string.Empty;
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                    }
                    else if (CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(qaiBatach.SerialNo)) == Messages.QAI_EXPIRED)
                    {
                        GlobalMessageBox.Show(Messages.QAI_EXPIRED, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        txtBatchNo.Text = string.Empty;
                        txtCurrentQCType.Text = string.Empty;
                        txtCurrentQcTypeDescription.Text = string.Empty;
                        lblGloveCode.Text = string.Empty;
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                    }
                    else if (!(QAIBLL.GetPTStatusForPWT(Convert.ToInt64(txtSerialNo.Text.Trim()))))
                    {
                        GlobalMessageBox.Show(Messages.QAI_PWT_PT_NOT_COMPLETED, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtBatchNo.Text = string.Empty;
                        txtCurrentQCType.Text = string.Empty;
                        txtCurrentQcTypeDescription.Text = string.Empty;
                        lblGloveCode.Text = string.Empty;
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                    }
                    //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. START
                    else if (CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerialNo.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPI) &&
                            !CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerialNo.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPO))
                    {
                        GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtBatchNo.Text = string.Empty;
                        txtCurrentQCType.Text = string.Empty;
                        txtCurrentQcTypeDescription.Text = string.Empty;
                        lblGloveCode.Text = string.Empty;
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                    }
                    else if (QAIBLL.GetLastServiceName(Convert.ToDecimal(txtSerialNo.Text.Trim())) == CreateInvTransJournalFunctionidentifier.STPI)
                    {
                        GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtBatchNo.Text = string.Empty;
                        txtCurrentQCType.Text = string.Empty;
                        txtCurrentQcTypeDescription.Text = string.Empty;
                        lblGloveCode.Text = string.Empty;
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                    }
                    //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. END

                    else
                    {
                        ISPageValid = true;
                        txtBatchNo.Text = qaiBatach.BatchNo;
                        txtCurrentQCType.Text = qaiBatach.QCType;
                        _tenPicWeight = qaiBatach.TenPcsWeight;
                        lblGloveCode.Text = qaiBatach.GloveType;
                        List<DropdownDTO> qctypelst = CommonBLL.GetQCTypeALL();
                        txtCurrentQcTypeDescription.Text = (from qc in qctypelst
                                                            where qc.DisplayField.Trim() == qaiBatach.QCType.Trim()
                                                            select qc.IDField).FirstOrDefault();

                        //assign QC Type
                        if (!string.IsNullOrEmpty(qaiBatach.QCType))
                        {
                            cmbQcType.Text = qaiBatach.QCType;
                            txtQcTypeDescription.Text = QAIBLL.GetQCtypeValue(_qCType, qaiBatach.QCType);
                        }

                        GloveType = qaiBatach.GloveType;
                        btnNext.Enabled = true;
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
                ClearData();
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
                        if (SecurityModuleBLL.ValidateOperatorAccess(txtQaiInspectorId.Text.Trim(), Constants.EDIT_DEFECTS))
                        {
                            ISPageValid = true;
                            txtQaiInspectorName.Text = operatorName;
                            txtQaiInspectorName.Enabled = false;
                            //commented by MYAdamas 20171127 due to shift +tab not working
                            cmbCustomerType.Focus();
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


        private void cmbQcType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtQcTypeDescription.Text = Convert.ToString(cmbQcType.SelectedValue);
        }
        private void cmbQcType_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbQcType.Text))
            {
                txtQcTypeDescription.Text = QAIBLL.GetQCtypeValue(_qCType, cmbQcType.Text.Trim());
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
            //validationMesssageLst.Add(new ValidationMessage(cmbQcType, "QC Type:", ValidationType.Required));     //hide QC Type - MYAdamas
            validationMesssageLst.Add(new ValidationMessage(cmbChangeReason, "Change Reason:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbwatertghtsampleSize, "Water Tight Sampling Size:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbVisualtstsampleSize, "Visual Test Sampling Size:", ValidationType.Required));
            if (ValidateForm())
            {
                _qaidto = new QAIDTO();
                _qaidto.ScreenTitle = this.Text;
                _qaidto.InspectorId = txtQaiInspectorId.Text.Trim();
                _qaidto.SerialNo = txtSerialNo.Text.Trim();
                _qaidto.BatchNo = txtBatchNo.Text.Trim();
                _qaidto.QCType = cmbQcType.Text.Trim();
                _qaidto.WTSamplingSize = cmbwatertghtsampleSize.Text.Trim();
                _qaidto.VTSamplingSize = cmbVisualtstsampleSize.Text.Trim();
                _qaidto.BatchNo = txtBatchNo.Text.Trim();
                _qaidto.QaiInspectorName = txtQaiInspectorName.Text.Trim();
                _qaidto.TenPcsWeight = _tenPicWeight;
                _qaidto.ScreenName = Constants.QAIScreens.EditDefects;
                _qaidto.GloveType = GloveType;
                _qaidto.PackingSize = _current_qaidto.PackingSize;
                _qaidto.InnerBoxes = _current_qaidto.InnerBoxes;
                _qaidto.HBSamplingSize = _current_qaidto.HBSamplingSize;
                _qaidto.Isonline = _current_qaidto.Isonline;
                _qaidto.CustomerTypeId = cmbCustomerType.SelectedValue.ToString();

                QAIScanDefect qaiscandefect = new QAIScanDefect(_qaidto);
                _QAITranistion = qaiscandefect._DefectTranistion;
                qaiscandefect.ShowDialog();
                QAINavigation(qaiscandefect._DefectTranistion);
            }
        }

        #endregion

        #region User Methods

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
                cmbwatertghtsampleSize.BindComboBox(QAIBLL.GetSamplingSize(Constants.WATER_TIGHT_SAMPLING_SIZE, Constants.QAIScreens.EditDefects), true);
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
        /// </summary>
        private void BindVisaulTesttSamlSize()
        {
            //TODO: default is 50 need to set after master data insert into tables
            try
            {
                cmbVisualtstsampleSize.BindComboBox(QAIBLL.GetSamplingSize(Constants.VISUAL_TEST_SAMPLING_SIZE, Constants.QAIScreens.EditDefects), true);
                cmbVisualtstsampleSize.Text = Constants.DEFAULT_VISUAL_TEST_SAMPLE_SIZE50;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindVisaulTesttSamlSize", null);
                return;
            }
        }


        private void BindChangeReason()
        {
            try
            {
                cmbChangeReason.BindComboBox(CommonBLL.GetEditReasons(Constants.QAI_EDIT_DEFECTSREASONTYPE, Constants.QAI_MODULENAME), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindVisaulTesttSamlSize", null);
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


        private void ClearData()
        {
            this.txtSerialNo.Leave -= new EventHandler(txtSerialNo_Leave);
            this.txtQaiInspectorId.Leave -= new EventHandler(txtQaiInspectorId_Leave);
            BindQcType();
            BindWaterTightSamlSize();
            BindVisaulTesttSamlSize();
            BindChangeReason();
            BindCustomerType();
            cmbwatertghtsampleSize.SelectedIndex = -1;
            cmbVisualtstsampleSize.SelectedIndex = -1;
            cmbVisualtstsampleSize.Text = Constants.DEFAULT_VISUAL_TEST_SAMPLE_SIZE50;
            cmbwatertghtsampleSize.Text = Constants.DEFAULT_WATER_TIGHT_SAMPLE_SIZE125;
            txtCurrentQCType.Text = string.Empty;
            txtCurrentQcTypeDescription.Text = string.Empty;
            cmbQcType.SelectedIndex = -1;
            txtQcTypeDescription.Text = string.Empty;
            cmbChangeReason.SelectedIndex = -1;
            txtBatchNo.Text = string.Empty;
            txtSerialNo.Text = string.Empty;
            txtQaiInspectorId.Text = string.Empty;
            txtQaiInspectorName.Text = string.Empty;
            lblGloveCode.Text = string.Empty;
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
                ClearData();
            }
        }
        #endregion
    }
}
