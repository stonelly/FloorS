#region using

using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace Hartalega.FloorSystem.Windows.UI.QAI
{
    public partial class QAIScanInnerTenPcs : QAIBase
    {

        #region  private Variables

        private static string _screenName = Constants.SCAN_INNER_TENPCS;
        private static string _className = "QAIScanInnerTenPcs";
        private QAIDTO _qaidto;
        private int _qAIExpiryDays = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intQAIExpiryDuration);
        private List<DropdownDTO> _qCType = null;
        private bool skip2ndLeaveEvent = false;
        #endregion

        #region Public Variables
        public Constants.QAIPageTransition _QAITranistion { get; set; }
        public string _gloveBoxSize { get; set; }

        #endregion

        #region Load Form

        public QAIScanInnerTenPcs()
        {
            InitializeComponent();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "QAIScanInnerTenPcs", null);
            }
            this.Text = Constants.SCAN_INNER_TENPCS;
            groupBox1.Text = Constants.SCAN_INNER_TENPCS;
            btnNext.Enabled = false;
            BindQcType();
            //BindPackingSize();
            BindHotBoxsampleSize();//#MH on 27th October 2016
            BindWaterTightSamlSize();
            BindVisaulTesttSamlSize();
            BindCustomerType();
            txtQaiInspectorId.OperatorId();
            txtSerialNo.SerialNo();
            cmbQcType.QcTypeCombo();
            lblPcsWeight.Visible = false;
            skip2ndLeaveEvent = false;
        }

        private void QAIScanInnerTenPcs_Load(object sender, EventArgs e)
        {
            try
            {
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {
                    txt10pcweight.ReadOnly = true;
                    txt10pcweight.TabStop = false;
                    txt10pcweight.ReadOnly = true;
                    txt10pcweight.TabStop = false;
                }
                else
                {
                    txt10pcweight.ReadOnly = false;
                    txt10pcweight.TabStop = true;
                    txt10pcweight.ReadOnly = false;
                    txt10pcweight.TabStop = true;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "QAIScanInnerTenPcs_Load", null);
                return;
            }
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
                        if (SecurityModuleBLL.ValidateOperatorAccess(txtQaiInspectorId.Text.Trim(), Constants.SCAN_INNER_TENPCS))
                        {
                            ISPageValid = true;
                            txtQaiInspectorName.Text = operatorName;
                            txtQaiInspectorName.Enabled = false;
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

                //20201024 Azrul: Check for Online Surgical
                if (SurgicalGloveBLL.IsOnlineSurgicalGlove(Convert.ToDecimal(txtSerialNo.Text.Trim())))
                {
                    GlobalMessageBox.Show(Messages.QAI_SCAN_LOOSE_GLOVES, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                    return;
                }

                QAIDTO qaiBatch = null;
                qaiBatch = CommonBLL.GetBatchNumberBySerialNumber(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                if (qaiBatch != null && string.IsNullOrEmpty(qaiBatch.BatchNo))
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
                    else if (!(QAIBLL.GetPTStatusForPWT(Convert.ToInt64(txtSerialNo.Text.Trim()))))
                    {
                        GlobalMessageBox.Show(Messages.QAI_PWT_PT_NOT_COMPLETED, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else if (qaiBatch.QAIDate.HasValue && (ServerCurrentDateTime - qaiBatch.QAIDate.Value).Days < _qAIExpiryDays)
                    {
                        GlobalMessageBox.Show(Messages.QAI_SCAN_RESAMPLING_SCREEN, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else if (!string.IsNullOrEmpty(qaiBatch.QCType) && !qaiBatch.QAIDate.HasValue)
                    {
                        GlobalMessageBox.Show(Messages.QAI_SCAN_RESAMPLING_SCREEN, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else if ((qaiBatch.Isonline.HasValue && qaiBatch.Isonline.Value == false) || qaiBatch.Isonline.HasValue == false)
                    {
                        GlobalMessageBox.Show(Messages.QAI_SCAN_LOOSE_GLOVES, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else
                    {
                        ISPageValid = true;
                        txtBatchNo.Text = qaiBatch.BatchNo;
                        txtInnerBox.Text = qaiBatch.InnerBoxes.ToString();
                        txtPackingSize.Text = qaiBatch.PackingSize;
                        lblGlovecode.Text = qaiBatch.GloveType;
                        GloveType = qaiBatch.GloveType;
                        _gloveBoxSize = qaiBatch.Size;
                        btnNext.Enabled = true;
                        cmbQcType.Focus();
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
        /// cmbQcType SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            // Max He,21/12/2018, fix bug: When user tab on QC type not auto connect to small scale
            txtInnerBox_Leave(sender, e);
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
            validationMesssageLst.Add(new ValidationMessage(txtInnerBox, "Inner Box:", ValidationType.Required));
            //if (!txt10pcweight.ReadOnly)
            //{
                validationMesssageLst.Add(new ValidationMessage(txt10pcweight, "10 Pcs(g):", ValidationType.Required));
            //}
            validationMesssageLst.Add(new ValidationMessage(txtPackingSize, "Packing Size:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbwatertghtsampleSize, "Water Tight Sampling Size:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbVisualtstsampleSize, "Visual Test Sampling Size:", ValidationType.Required));
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
            {
                GetTenPcsWeight();
                if (!string.IsNullOrEmpty(txt10pcweight.Text))
                {
                    ValidateTenPcsWeight(true);
                }
            }
            if (txt10pcweight.Text == "0" || txt10pcweight.Text == "0.00")
            {
                GlobalMessageBox.Show(Messages.TEN_PCS_WEIGHT_IS_ZERO, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                txt10pcweight.Text = string.Empty;
                txt10pcweight.Focus();
                return;
            }
            if (ValidateForm())
            {
                if (Convert.ToDecimal(txt10pcweight.Text.Trim()) > 0)
                {
                    if (Convert.ToDecimal(txt10pcweight.Text.Trim()) > 0)
                    {
                        if (lblPcsWeight.Visible && Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().LPassword))
                        {
                            Login passwordForm = new Login(Constants.Modules.QAISYSTEMINNERTENPCS, _screenName, txt10pcweight.Text.Trim());
                            passwordForm.Authentication = string.Empty;
                            passwordForm.ShowDialog();
                            if (string.IsNullOrEmpty(passwordForm.Authentication))
                            {
                                return;
                            }
                        }
                        _qaidto = new QAIDTO();
                        _qaidto.ScreenTitle = this.Text;
                        _qaidto.InspectorId = txtQaiInspectorId.Text.Trim();
                        _qaidto.SerialNo = txtSerialNo.Text.Trim();
                        _qaidto.BatchNo = txtBatchNo.Text.Trim();
                        _qaidto.QCType = cmbQcType.Text.Trim();
                        _qaidto.WTSamplingSize = cmbwatertghtsampleSize.Text.Trim();
                        // #Azrul 13/07/2018: Merged from Live AX6 Start
                        //_qaidto.VTSamplingSize = cmbVisualtstsampleSize.Text.Trim(); #Azman 21/02/2018 
                        // START
                        _qaidto.VTSamplingSize = GetVTTenPcsSampleSize(false, cmbVisualtstsampleSize.SelectedValue.ToString().Trim());
                        _qaidto.TenPCSSamplingSize = GetVTTenPcsSampleSize(true, cmbVisualtstsampleSize.SelectedValue.ToString().Trim());
                        // END
                        // #Azrul 13/07/2018: Merged from Live AX6 End
                        _qaidto.InnerBoxes = 0; // #AZ 27/05/2018 1.n InnerBoxes set at print HBC.
                        _qaidto.PackingSize = "0"; // #AZ 27/05/2018 2.n PackingSize set at print HBC.
                        _qaidto.QaiInspectorName = txtQaiInspectorName.Text.Trim();
                        _qaidto.TenPcsWeight = string.IsNullOrEmpty(txt10pcweight.Text) ? 0 : Convert.ToDecimal(txt10pcweight.Text.Trim());
                        _qaidto.ScreenName = Constants.QAIScreens.QAIScanInnerTenPcs;
                        _qaidto.GloveType = GloveType;
                        _qaidto.TotalPCs = QAIBLL.GetHBCTotalPcsFromSerialNo(Convert.ToDecimal(_qaidto.SerialNo)); // #AZ 27/05/2018 3.n TotalPcs get from print HBC.
                        _qaidto.BatchWeight = _qaidto.TotalPCs / (10 * 1000);
                        _qaidto.HBSamplingSize = cmbHotBoxsampleSize.Text.Trim();// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
                        _qaidto.CustomerTypeId = cmbCustomerType.SelectedValue.ToString();
                        QAIScanDefect qaiscandefect = new QAIScanDefect(_qaidto);
                        _QAITranistion = qaiscandefect._DefectTranistion;
                        qaiscandefect.ShowDialog();
                        QAINavigation(qaiscandefect._DefectTranistion);
                    }
                    else
                    {
                        string errormassge = Constants.INVALID_DATA_VALIDATION_ALERT + Environment.NewLine + "10 Pcs(g):";
                        GlobalMessageBox.Show(errormassge, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                        txt10pcweight.Text = string.Empty;
                        txt10pcweight.Focus();
                    }

                }
                else
                {
                    string errormassge = Constants.INVALID_DATA_VALIDATION_ALERT + Environment.NewLine + "Inner Box:";
                    GlobalMessageBox.Show(errormassge, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                    txtInnerBox.Text = string.Empty;
                    txtInnerBox.Focus();
                }
            }
        }

        bool txtInnerBoxTenPcsOverRangePopedUp;

        /// <summary>
        /// Max He,21/12/2018, fix bug: When user tab on QC type not auto connect to small scale
        /// Change UI logic, manual call from QC type dropdown leave event
        /// Due to Inner Box textbox was hided, info fill up from glove RAF report screen and not require user keyin in QAI Scan any more
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInnerBox_Leave(object sender, EventArgs e)
        {
            //commented by MYAdamas due to shift+ tab to go to previous cell not need to focus on leave
            //txt10pcweight.Focus();
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true && !skip2ndLeaveEvent)
            {                
                GetTenPcsWeight();
                if (!string.IsNullOrEmpty(txt10pcweight.Text))
                {
                    skip2ndLeaveEvent = true;
                    ValidateTenPcsWeight(txtInnerBoxTenPcsOverRangePopedUp);
                    txtInnerBoxTenPcsOverRangePopedUp = true;
                }
            }
        }

        /// <summary>
        /// ***TBC - Made editable for only Testing Purposes - To Comment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt10pcweight_Leave(object sender, EventArgs e)
        {
            /*
            if (string.IsNullOrEmpty(txt10pcweight.Text))
            {
                GetTenPcsWeight();
            }
            if (!string.IsNullOrEmpty(txt10pcweight.Text))
            {
                txt10pcweight.Text = Convert.ToDouble(txt10pcweight.Text).ToString("#,##0.00");
            }
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == false)
            {
                ValidateTenPcsWeight();
            }
             * */
        }

        /// <summary>
        /// ***TBC - Made editable for only Testing Purposes - To Comment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt10pcweight_Validated(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt10pcweight.Text))
            {
                txt10pcweight.Text = Convert.ToDouble(txt10pcweight.Text).ToString("#,##0.00");
            }
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == false)
            {
                ValidateTenPcsWeight();
            }
            txtInnerBoxTenPcsOverRangePopedUp = false;
            cmbwatertghtsampleSize.Focus();
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
        /// Bind Packing Size
        /// </summary>
        //private void BindPackingSize()
        //{
        //    try
        //    {
        //        cmbPakcingSize.BindComboBox(CommonBLL.GetPackingSize(), true);
        //    }
        //    catch (FloorSystemException ex)
        //    {
        //        ExceptionLogging(ex, _screenName, _className, "BindQcType", null);
        //        return;
        //    }

        //}


        /// <summary>
        /// Bind Hot Box packing Size
        /// #MH on 27th October 2016
        /// </summary>
        private void BindHotBoxsampleSize()
        {
            try
            {
                cmbHotBoxsampleSize.BindComboBox(CommonBLL.GetPackingSize(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindQcType", null);
                return;
            }

        }


        /// <summary>
        /// Bind Water Tight Sample Siz
        /// </summary>
        private void BindWaterTightSamlSize()
        {
            try
            {
                cmbwatertghtsampleSize.BindComboBox(QAIBLL.GetSamplingSize(Constants.WATER_TIGHT_SAMPLING_SIZE, Constants.QAIScreens.QAIScanInnerTenPcs), true);
                //cmbwatertghtsampleSize.Text = Constants.DEFAULT_WATER_TIGHT_SAMPLE_SIZE125;            
                cmbwatertghtsampleSize.Text = Constants.DEFAULT_WATER_TIGHT_SAMPLE_SIZE80; //itrf: 20201203072551228585
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
                cmbVisualtstsampleSize.BindComboBox(QAIBLL.GetSamplingSize(Constants.VISUAL_TEST_SAMPLING_SIZE, Constants.QAIScreens.QAIScanInnerTenPcs), true);
                cmbVisualtstsampleSize.Text = Constants.DEFAULT_VISUAL_TEST_SAMPLE_SIZE50PLUS10; // #Azrul 13/07/2018: Merged from Live AX6
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindVisualTestSamlSize", null); // #Azrul 13/07/2018: Merged from Live AX6
                return;
            }
        }


        /// <summary>
        /// Bind Packing Size dropdown
        /// </summary>
        /// <param name="gloveType"></param>
        /// <param name="size"></param>
        private void BindPackingSize(string gloveType, string size)
        {
            try
            {
                cmbVisualtstsampleSize.BindComboBox(CommonBLL.GetPackingSize(gloveType, size), true);
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
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else if (floorException.subSystem == Constants.DAL)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                if (uiControl == "getTenPcsWeight")
                GlobalMessageBox.Show("(" + result + ") - " + Constants.TENPCS_WEIGHT_ERROR, Constants.AlertType.Error, Messages.WARNING, GlobalMessageBoxButtons.OK);
            cmbQcType.Focus();
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
            this.txt10pcweight.Leave -= new EventHandler(txt10pcweight_Leave);
            this.cmbQcType.Leave -= new EventHandler(cmbQcType_Leave);
            BindQcType();
            //BindPackingSize();
            BindHotBoxsampleSize();//#MH on 27th October 2016
            BindWaterTightSamlSize();
            BindVisaulTesttSamlSize();
            BindCustomerType();
            txtQaiInspectorId.Text = string.Empty;
            txtQaiInspectorName.Text = string.Empty;
            lblGlovecode.Text = string.Empty;
            txtQcTypeDescription.Text = string.Empty;
            txtSerialNo.Text = string.Empty;
            txtBatchNo.Text = string.Empty;
            txtInnerBox.Text = string.Empty;
            txt10pcweight.Text = string.Empty;
            cmbwatertghtsampleSize.SelectedIndex = -1;
            cmbVisualtstsampleSize.SelectedIndex = -1;
            cmbVisualtstsampleSize.Text = Constants.DEFAULT_VISUAL_TEST_SAMPLE_SIZE50PLUS10;
            cmbwatertghtsampleSize.Text = Constants.DEFAULT_WATER_TIGHT_SAMPLE_SIZE80; //itrf: 20201203072551228585
            cmbQcType.SelectedIndex = -1;
            txtQcTypeDescription.Text = string.Empty;
            //cmbPakcingSize.SelectedIndex = -1;
            txtQaiInspectorId.Focus();
            btnNext.Enabled = false;
            lblPcsWeight.Visible = false;
            txt10pcweight.Text = string.Empty;
            this.txt10pcweight.Leave += new EventHandler(txt10pcweight_Leave);
            this.txtSerialNo.Leave += new EventHandler(txtSerialNo_Leave);
            this.txtQaiInspectorId.Leave += new EventHandler(txtQaiInspectorId_Leave);
            this.cmbQcType.Leave += new EventHandler(cmbQcType_Leave);
            skip2ndLeaveEvent = false;  // reset the flag for next scan
        }

        /// <summary>
        /// Validate 10 Pcs Weight based on Glove Type and Size by getting base values from AX
        /// </summary>
        private void ValidateTenPcsWeight(bool beforeSave = false)
        {
            TenPcsDTO weight = new TenPcsDTO();
            try
            {
                weight = CommonBLL.GetMinMaxTenPcsWeight(GloveType, _gloveBoxSize);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "validateTenPcsWeight", _gloveBoxSize, GloveType);
                return;
            }
            if (string.IsNullOrEmpty(txt10pcweight.Text)) txt10pcweight.Text = Constants.ZERO.ToString();
            if (Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().LWeight) && weight.MinWeight != null)
            {
                if (Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().LWeight) && !CommonBLL.InRange(Convert.ToDouble(txt10pcweight.Text), Convert.ToDouble(weight.MinWeight), Convert.ToDouble(weight.MaxWeight)))
                {
                    if (!beforeSave)
                    {
                        GlobalMessageBox.Show(Constants.TENPCS_WEIGHT_RANGE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                    lblPcsWeight.Visible = true;
                    lblPcsWeight.Text = Constants.TENPCS_WEIGHT_RANGE;
                    //if (!beforeSave)
                    //{
                    //    //txtPackingSize.Focus();
                    //    cmbwatertghtsampleSize.Focus();
                    //}
                }
                else
                {
                    lblPcsWeight.Visible = false;
                    //txt10pcweight.Text = txt10pcweight.Text.ToString();
                }
            }
            else
            {
                lblPcsWeight.Visible = false;
                //txt10pcweight.Text = txt10pcweight.Text.ToString();
            }
        }

        /// <summary>
        /// To get 10 PCs weight by calling the method in the Integration class through BLL
        /// </summary>
        /// <returns></returns>
        private void GetTenPcsWeight()
        {
            try
            {
                lblPcsWeight.Visible = true;
                lblPcsWeight.Text = Constants.CONNECTING;
                double val = CommonBLL.GetTenPcsWeight();
                //double val = 90.54d;// for debug only
                txt10pcweight.Text = val.ToString("#,##0.00");
                lblPcsWeight.Visible = false;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getTenPcsWeight", null);
                return;
            }
            lblPcsWeight.Visible = false;
        }

        #endregion

        private void cmbPakcingSize_Leave(object sender, EventArgs e)
        {
            cmbwatertghtsampleSize.Focus();
        }

            /// <summary>
            /// #Azrul 13/07/2018: Merged from Live AX6
            /// GetVTTenPcsSampleSize - split selection value to different 10pcs and VT #Azman 21/02/2018
            /// </summary>
            /// <param name="isTenPcs"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            private string GetVTTenPcsSampleSize(bool isTenPcs, string value)
            {
                string[] val = value.Trim().Split(',');
                if (isTenPcs)
                    return val[1];
                else
                    return val[0];
            }
        }
}
