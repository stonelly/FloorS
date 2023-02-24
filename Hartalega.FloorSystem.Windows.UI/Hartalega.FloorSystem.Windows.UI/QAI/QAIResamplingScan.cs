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
    /// QAI Resampling Scan Screen
    /// </summary>
    public partial class QAIResamplingScan : QAIBase
    {

        #region private Variables

        private static string _screenName = Constants.RESAMPLING_SCAN;
        private static string _className = "QAIResamplingScan";
        //private static string _screenNameForAuthorization = "Resampling Scan";
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
        public QAIResamplingScan()
        {
            InitializeComponent();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "QAIResamplingScan", null);
            }
            this.Text = Constants.RESAMPLING_SCAN;
            groupBox1.Text = Constants.RESAMPLING_SCAN;
            btnNext.Enabled = false;
            BindQcType();
            BindWaterTightSamlSize();
            BindVisaulTesttSamlSize();
            BindCustomerType();
            txtQaiInspectorId.OperatorId();
            txtSerialNo.SerialNo();
            cmbQcType.QcTypeCombo();
        }

        private void QAIResamplingScan_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// txtQaiInspectorId Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        if (SecurityModuleBLL.ValidateOperatorAccess(txtQaiInspectorId.Text.Trim(), Constants.RESAMPLING_SCAN))
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

        /// <summary>
        /// txtSerialNo Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            ISPageValid = false;
            btnNext.Enabled = false;
            txtBatchNo.Text = string.Empty;
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

                QAIDTO qaiBatach = null;
                qaiBatach = CommonBLL.GetBatchNumberBySerialNumber(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                _current_qaidto = qaiBatach;
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
                    else if (qaiBatach.QAIDate.HasValue && CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(qaiBatach.SerialNo)) == Messages.QAI_EXPIRED)
                    {
                        GlobalMessageBox.Show(Messages.QAI_EXPIRED, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else if (!(QAIBLL.GetPTStatusForPWT(Convert.ToInt64(txtSerialNo.Text.Trim()))))
                    {
                        GlobalMessageBox.Show(Messages.QAI_PWT_PT_NOT_COMPLETED, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else if (string.IsNullOrEmpty(qaiBatach.QCType))
                    {
                        GlobalMessageBox.Show(Messages.QAI_SCAN_DT, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. START
                    else if (CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerialNo.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPI) &&
                            !CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerialNo.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPO))
                    {
                        GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else if (QAIBLL.GetLastServiceName(Convert.ToDecimal(txtSerialNo.Text.Trim())) == CreateInvTransJournalFunctionidentifier.STPI)
                    {
                        GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. END

                    else
                    {
                        txtBatchNo.Text = qaiBatach.BatchNo;
                        _tenPicWeight = qaiBatach.TenPcsWeight;
                        GloveType = qaiBatach.GloveType;
                        lblGlovecode.Text = qaiBatach.GloveType;
                        btnNext.Enabled = true;
                        ISPageValid = true;
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
        /// Prompt Message. If Yes, Clear the Data and set focus to first editable field. If No, stay on same page retaining the data entered on screen 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
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

        /// <summary>
        /// btnNext Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                _qaidto.IsReSampling = true;
                _qaidto.ScreenName = Constants.QAIScreens.QAIResamplingScan;
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
                cmbwatertghtsampleSize.BindComboBox(QAIBLL.GetSamplingSize(Constants.WATER_TIGHT_SAMPLING_SIZE, Constants.QAIScreens.QAIResamplingScan), true);
                cmbwatertghtsampleSize.Text = Constants.DEFAULT_WATER_TIGHT_SAMPLE_SIZE80;
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
            try
            {
                cmbVisualtstsampleSize.BindComboBox(QAIBLL.GetSamplingSize(Constants.VISUAL_TEST_SAMPLING_SIZE, Constants.QAIScreens.QAIResamplingScan), true); ;
                cmbVisualtstsampleSize.Text = Constants.DEFAULT_VISUAL_TEST_SAMPLE_SIZE50;
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

        private void ClearForm()
        {
            this.txtSerialNo.Leave -= new EventHandler(txtSerialNo_Leave);
            this.txtQaiInspectorId.Leave -= new EventHandler(txtQaiInspectorId_Leave);
            BindQcType();
            BindWaterTightSamlSize();
            BindVisaulTesttSamlSize();
            BindCustomerType();
            txtQaiInspectorId.Text = string.Empty;
            txtQaiInspectorName.Text = string.Empty;
            txtSerialNo.Text = string.Empty;
            txtBatchNo.Text = string.Empty;
            cmbwatertghtsampleSize.SelectedIndex = -1;
            cmbVisualtstsampleSize.SelectedIndex = -1;
            cmbVisualtstsampleSize.Text = Constants.DEFAULT_VISUAL_TEST_SAMPLE_SIZE50;
            cmbwatertghtsampleSize.Text = Constants.DEFAULT_WATER_TIGHT_SAMPLE_SIZE80;
            lblGlovecode.Text = string.Empty;
            txtQcTypeDescription.Text = string.Empty;
            cmbQcType.SelectedIndex = -1;
            txtQaiInspectorId.Focus();
            btnNext.Enabled = false;
            this.txtSerialNo.Leave += new EventHandler(txtSerialNo_Leave);
            this.txtQaiInspectorId.Leave += new EventHandler(txtQaiInspectorId_Leave);
        }

        #endregion
        

    }
}
