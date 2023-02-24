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

namespace Hartalega.FloorSystem.Windows.UI.PostTreatment
{

    /// <summary>
    /// Module: Post Treatment
    /// Screen Name: Scan PT Batch Card
    /// File Type: Code file
    /// </summary> 
    public partial class ScanPTBatchCard : FormBase
    {
        #region Class Variables
        private bool _isRework;  //For checking whether the batch comes for Rework or its the first time entry for the Batch.  
        private bool _pressCancel;
        private static string _screenName = "Scan PT Batch Card";
        private static string _className = "ScanPTBatchCard";
        private string _moduleId;
        private string _gloveType;
        private string _batchType;
        private bool _pressSave;
        private bool _isSaved = false;
        private static string _serialNo;
        private decimal _totalPcs;
        int _authorizedFor = Constants.ZERO;

        string strServiceName = "";
        string cBtype = "";
        string _batch_type = ""; //Added by Tan Wei Wah 20190213
        int _total_pcs = 0; //Added by Tan Wei Wah 20190213
        #endregion

        #region Load Page
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanPTBatchCard" /> class.
        /// </summary>
        public ScanPTBatchCard()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Form load event - Fill ComboBox Shift and Show the current Location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanPTBatchCard_Load(object sender, EventArgs e)
        {
            txtSerialNo.SerialNo();
            ReasonControlsVisibility(false);
            btnSave.Enabled = false;
            this.ActiveControl = txtSerialNo;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
                txtLocation.Text = WorkStationDTO.GetInstance().Location;
                GetShiftName();
                _moduleId = CommonBLL.GetModuleId(Constants.POSTTREATMENTSYSTEM);
                lblTenPcs.Visible = false;
                lblBatchWeight.Visible = false;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ScanPTBatchCard_Load", string.Empty);
                return;
            }
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
            {
                txtBatchWeight.ReadOnly = true;
                txtBatchWeight.TabStop = false;
                txtTenPcs.ReadOnly = true;
                txtTenPcs.TabStop = false;
            }
            else
            {
                txtBatchWeight.ReadOnly = false;
                txtBatchWeight.TabStop = true;
                txtTenPcs.ReadOnly = false;
                txtTenPcs.TabStop = true;
            }
        }

        #endregion

        #region Fill Sources
        /// <summary>
        /// Get the Rework Reason data & Bind the Rework Reason ComboBox  
        /// </summary>
        private void BindReasons()
        {
            try
            {
                cmbReworkReason.BindComboBox(CommonBLL.GetReasons(Constants.SCAN_PT_BATCHCARD_REASONTYPE, _moduleId), true);
                cmbReworkProcess.BindComboBox(CommonBLL.GetEnumText(Constants.REWORK_PROCESS), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindReasons", Constants.SCAN_PT_BATCHCARD_REASONTYPE, Constants.POSTTREATMENTSYSTEM, Constants.REWORK_PROCESS);
                return;
            }
        }

        /// <summary>
        /// Get the Shift data & Bind the Shift ComboBox 
        /// </summary>
        private void GetShiftName()
        {
            try
            {
                List<DropdownDTO> lstShift = CommonBLL.GetShift(Framework.Constants.ShiftGroup.PT);
                cmbShift.BindComboBox(lstShift, true);
                this.cmbShift.SelectedItem = "IDField";
                if (lstShift != null)
                    this.cmbShift.Text = lstShift[Constants.ZERO].SelectedValue;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetShiftName", Framework.Constants.ShiftGroup.PT);
                return;
            }
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
            {
                if (ValidatePTScanInSerialNumber(true))
                {
                    GetBatchDetails();
                }
            }                   
        }

        /// <summary>
        /// Get TenPcs Weight & Batch Weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbShift_Leave(object sender, EventArgs e)
        {
            if (!_pressCancel)
            {
                //***TBC - Made editable for only Testing Purposes - To Comment
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration != true)
                {
                    txtTenPcs.Focus();
                    txtTenPcs.TabStop = true;
                }
                else
                {
                    GetTenPcsWeight();
                    //txtTenPcs.Text = CommonBLL.GetTenPcsWeight().ToString("#,##0.00");
                    ValidateTenPcs();
                    GetBatchWeight();
                }

            }
        }

        /// <summary>
        ///  Validate & Save the Batch Card Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string strValidate = String.Empty;
            bool isPostWT = false;
            Int64 auditLogID = 0;

            PostTreatmentDTO objPT = new PostTreatmentDTO();
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtTenPcs, "10 Pcs(g)", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtBatchWeight, "Batch(kg)", ValidationType.Required));
            try
            {
                if (!ValidatePTScanInSerialNumber(false))
                {
                    return;
                }

                if (_isRework)
                {
                    validationMesssageLst.Add(new ValidationMessage(cmbReworkReason, "Rework Reason", ValidationType.Required));
                    validationMesssageLst.Add(new ValidationMessage(cmbReworkProcess, "Rework Process", ValidationType.Required));
                    if (cmbReworkReason.SelectedIndex >= Constants.ZERO)
                    {
                        objPT.ReworkReasonId = cmbReworkReason.SelectedValue.ToString();
                    }
                    if (cmbReworkProcess.SelectedIndex >= Constants.ZERO)
                    {
                        objPT.ReworkProcess = cmbReworkProcess.SelectedValue.ToString();
                    }
                    objPT.ReworkCount = Convert.ToInt32(txtReworkCount.Text);
                }
                if (ValidateForm())
                {
                    if (_serialNo != txtSerialNo.Text)
                    {
                        GetBatchDetails();
                    }
                    validationMesssageLst = new List<ValidationMessage>();
                    if (_isRework)
                    {
                        validationMesssageLst.Add(new ValidationMessage(cmbReworkReason, "Rework Reason", ValidationType.Required));
                        validationMesssageLst.Add(new ValidationMessage(cmbReworkProcess, "Rework Process", ValidationType.Required));
                    }
                    _pressSave = true;
                    if (!string.IsNullOrEmpty(txtSerialNo.Text) && ValidateForm())
                    {
                        _pressSave = false;
                        if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                        {
                            GetTenPcsWeight();
                            ValidateTenPcs();
                            GetBatchWeight();
                        }

                        if (CommonBLL.ValidatePositiveValue(Convert.ToDecimal(txtBatchWeight.Text)))
                        {
                            if (ValidateTenPcsWeight())
                            {
                                //if (_totalPcs >= Convert.ToDecimal(Convert.ToDecimal(txtBatchWeight.Text) * Constants.THOUSAND * Constants.TEN / Convert.ToDecimal(txtTenPcs.Text)))
                                if (IsTotalPcsExceed())
                                {
                                    objPT.SerialNumber = Convert.ToDecimal(txtSerialNo.Text.Trim());
                                    objPT.Shift = Convert.ToInt32(cmbShift.SelectedValue);
                                    objPT.LocationId = WorkStationDTO.GetInstance().LocationId;
                                    objPT.LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                                    objPT.TenPcsWeight = Convert.ToDecimal(txtTenPcs.Text);
                                    objPT.BatchWeight = Convert.ToDecimal(txtBatchWeight.Text);
                                    objPT.WorkstationNumber = WorkStationDTO.GetInstance().WorkStationId;
                                    objPT.Id = Constants.ZERO;
                                    objPT.DryerId = PostTreatmentBLL.GetDryerId(Convert.ToDecimal(txtSerialNo.Text));
                                    objPT.AuthorizedFor = _authorizedFor;
                                    if (!_isSaved)
                                    {
                                        int ptID = PostTreatmentBLL.SaveScanPTBatch(objPT);
                                        auditLogID = CommonBLL.InsertBatchAuditLog(Convert.ToDecimal(txtSerialNo.Text.Trim()), ptID, "PT", "Scan PT Batch Card");

                                        //event log myadamas 20190227
                                        EventLogDTO EventLog = new EventLogDTO();

                                        EventLog.CreatedBy = String.Empty;
                                        Constants.EventLog audAction = Constants.EventLog.Save;
                                        EventLog.EventType = Convert.ToInt32(audAction);
                                        EventLog.EventLogType = Constants.eventlogtype;

                                        var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                                        CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());

                                    }
                                    _isSaved = true;
                                    bool isPostingSuccess = false;
                                    if (string.IsNullOrEmpty(PostTreatmentBLL.GetBatchStatus(Convert.ToDecimal(txtSerialNo.Text))) && !_isRework)
                                    {
                                        BatchDTO batchdto = AXPostingBLL.GetCompletePTDetails(Convert.ToDecimal(txtSerialNo.Text));
                                        if (PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text)) == Constants.PWT || PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text)) == Constants.QWT || PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text)) == Constants.OWT || PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text)) == Constants.PSW)
                                        {
                                            if (!CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.PWTBCP.ToString())
                                                 && !CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.PWTBCA.ToString())
                                                && !CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.PWTBCQ.ToString())
                                                && !CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.PWTBCS.ToString()))
                                            {
                                                isPostingSuccess = AXPostingBLL.PostAXDataPrintWaterTightBatchCard(batchdto);
                                                isPostWT = true;
                                            }
                                        }
                                        else
                                        {
                                            if (CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.PVTBCA.ToString()))
                                            {
                                                //#AZRUL 6/3/2019: only generates SPBC after QC route for PVTBCA.
                                                if (CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.SOBC.ToString()))
                                                {
                                                    isPostingSuccess = AXPostingBLL.PostAXDataScanPTBatchCard(Convert.ToDecimal(txtSerialNo.Text));
                                                }
                                                //#Azrul 20201202: QCQI if failed PT, check if no extra rework then generate SPBC.
                                                else if (!QAIBLL.IsPrevReworkWithoutSOBC(txtSerialNo.Text))
                                                {
                                                    isPostingSuccess = AXPostingBLL.PostAXDataScanPTBatchCard(Convert.ToDecimal(txtSerialNo.Text));
                                                }
                                                else
                                                {
                                                    isPostingSuccess = true;
                                                }
                                            }
                                            else
                                            {
                                                isPostingSuccess = AXPostingBLL.PostAXDataScanPTBatchCard(Convert.ToDecimal(txtSerialNo.Text));
                                            }
                                        }
                                        if (isPostingSuccess)
                                        {
                                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        }
                                        else
                                        {
                                            PostTreatmentBLL.DeleteBatch(Convert.ToDecimal(txtSerialNo.Text), objPT.ReworkCount);
                                            if (isPostWT)
                                            {
                                                PostTreatmentBLL.RollbackPWT(txtSerialNo.Text);
                                            }
                                            GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                        }
                                    }
                                    else if (SurgicalGloveBLL.IsOnlineSurgicalGlove(Convert.ToDecimal(txtSerialNo.Text)) //Azrul 20201030 Bypass checking if surgical
                                            || WasherBLL.CheckIsPTPFGlove(Convert.ToDecimal(txtSerialNo.Text.Trim()))) // #Azrul 2021-11-18 - Allow PTPF with SP glove to post SPBC.
                                    {
                                        BatchDTO batchdto = AXPostingBLL.GetCompletePTDetails(Convert.ToDecimal(txtSerialNo.Text));
                                        if (PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text)) == Constants.PWT || PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text)) == Constants.QWT || PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text)) == Constants.OWT || PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text)) == Constants.PSW)
                                        {
                                            if (!CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.PWTBCP.ToString())
                                                 && !CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.PWTBCA.ToString())
                                                && !CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.PWTBCQ.ToString())
                                                && !CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.PWTBCS.ToString()))
                                            {
                                                isPostingSuccess = AXPostingBLL.PostAXDataPrintWaterTightBatchCard(batchdto);
                                                isPostWT = true;
                                            }
                                            else // 20220826: Azrul - Unabled to save 2nd PT Scan Batch Card (Water Tight Batch aldready generated).
                                            {
                                                isPostingSuccess = true;
                                            }
                                        }
                                        else
                                        {
                                            if (CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.PVTBCA.ToString()))
                                            {
                                                //#AZRUL 6/3/2019: only generates SPBC after QC route for PVTBCA.
                                                if (CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.SOBC.ToString()))
                                                {
                                                    isPostingSuccess = AXPostingBLL.PostAXDataScanPTBatchCard(Convert.ToDecimal(txtSerialNo.Text));
                                                }
                                                else
                                                {
                                                    isPostingSuccess = true;
                                                }
                                            }
                                            else
                                            {
                                                isPostingSuccess = AXPostingBLL.PostAXDataScanPTBatchCard(Convert.ToDecimal(txtSerialNo.Text));
                                            }
                                        }
                                        if (isPostingSuccess)
                                        {
                                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        }
                                        else
                                        {
                                            PostTreatmentBLL.DeleteBatch(Convert.ToDecimal(txtSerialNo.Text), objPT.ReworkCount);
                                            if (isPostWT)
                                            {
                                                PostTreatmentBLL.RollbackPWT(txtSerialNo.Text);
                                            }
                                            GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                        }
                                    }
                                    else
                                    {
                                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

                                    }
                                    ClearForm();
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.ACTUAL_WEIGHT_EXCEED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                }
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INCORRECT_BATCH_WEIGHT, Constants.AlertType.Warning, Messages.INVALIDDATA, GlobalMessageBoxButtons.OK);
                            txtBatchWeight.Text = string.Empty;
                        }
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                if (_isSaved)
                {
                    PostTreatmentBLL.DeleteBatch(Convert.ToDecimal(txtSerialNo.Text), objPT.ReworkCount);
                    if (auditLogID != 0)
                    {
                        PostTreatmentBLL.DeleteBatchAuditLog(auditLogID);
                    }
                    if (isPostWT)
                    {
                        PostTreatmentBLL.RollbackPWT(txtSerialNo.Text);
                    }
                }
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
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
                _pressCancel = true;
                ClearForm();
            }
        }

        private void txtBatchWeight_Leave(object sender, EventArgs e)
        {
            if (txtBatchWeight.Text != String.Empty)
            {
                //Modified by Lakshman 11/1/2018 for Negative input validation
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
                //old
                //ValidateBatchWeight();
                //txtBatchWeight.Text = Convert.ToDouble(txtBatchWeight.Text).ToString("#,##0.00");
            }
        }

        private void txtBatchWeight_Enter(object sender, EventArgs e)
        {
            if (txtBatchWeight.Text == String.Empty && txtTenPcs.Text != String.Empty)
            {
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {
                    txtBatchWeight.ReadOnly = true;

                    //Added by Tan Wei Wah 20190325
                    if (!IsWaterTightBatch())
                    {
                        double _ten_pcs_weight = Convert.ToDouble(txtTenPcs.Text);
                        txtBatchWeight.Text = Convert.ToDouble(_total_pcs * _ten_pcs_weight / 10000).ToString("#,##0.00");
                    }
                    else
                    {
                        txtBatchWeight.Text = CommonBLL.GetBatchWeight().ToString("#,##0.00");
                    }
                    //Ended by Tan Wei Wah 20190325
                    txtBatchWeight_Leave(sender, e);
                }
                else
                {
                    txtBatchWeight.ReadOnly = false;
                    txtBatchWeight.Focus();
                }
            }
        }

        private void txtTenPcs_Leave(object sender, EventArgs e)
        {
            if (txtTenPcs.Text != String.Empty)
            {
                //Modified by Lakshman 11/1/2018 for Negative input validation
                decimal decim;
                if (Decimal.TryParse(txtTenPcs.Text, out decim) && decim > 0)
                {
                    ValidateTenPcs();
                    txtTenPcs.Text = Convert.ToDouble(txtTenPcs.Text).ToString("#,##0.00");

                    //Added by Tan Wei Wah 20190213
                    if (!IsWaterTightBatch())
                    {
                        double _ten_pcs_weight = Convert.ToDouble(txtTenPcs.Text);
                        txtBatchWeight.Text = Convert.ToDouble(_total_pcs * _ten_pcs_weight / 10000).ToString("#,##0.00");
                    }
                    //Ended by Tan Wei Wah 20190213
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_10PCS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    txtTenPcs.Text = String.Empty;
                    txtTenPcs.Focus();
                }
                //old
                //ValidateTenPcs();
                //txtTenPcs.Text = Convert.ToDouble(txtTenPcs.Text).ToString("#,##0.00");
            }
            //txtBatchWeight.Focus();
        }

        private void txtTenPcs_Enter(object sender, EventArgs e)
        {
            if (txtTenPcs.Text == String.Empty)
            {
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {
                    txtTenPcs.ReadOnly = true;
                    txtTenPcs.Text = CommonBLL.GetTenPcsWeight().ToString("#,##0.00");
                    txtTenPcs_Leave(sender, e);
                }
                else
                {
                    txtTenPcs.ReadOnly = false;
                    txtTenPcs.Focus();
                }
            }
        }

        private void txtBatchWeight_Validated(object sender, EventArgs e)
        {
            // ValidateBatchWeight();
            if (_isRework)
                cmbReworkReason.Focus();
            else
                btnSave.Focus();
        }
        #endregion

        #region User Methods

        /// <summary>
        /// Validate 10 Pcs Weight based on Glove Type and Size by getting base values from AX
        /// </summary>
        private bool ValidateTenPcsWeight()
        {
            bool chkTenPcs = false;
            string message = string.Empty;
            bool chkBatch = false;
            bool result = false;

            TenPcsDTO weight = new TenPcsDTO();
            weight = CommonBLL.GetMinMaxTenPcsWeight(_gloveType, txtSize.Text);
            WorkStationDataConfiguration data = WorkStationDataConfiguration.GetInstance();
            if (Convert.ToBoolean(data.LWeight) && weight.MinWeight != null)
            {
                if (Convert.ToBoolean(data.LWeight) && (Convert.ToDouble(txtTenPcs.Text) < Convert.ToDouble(weight.MinWeight) || Convert.ToDouble(txtTenPcs.Text) > Convert.ToDouble(weight.MaxWeight)))
                {
                    chkTenPcs = true;
                }
            }

            FloorSystemConfiguration Floordata = FloorSystemConfiguration.GetInstance();
            if (Convert.ToBoolean(data.LWeight) && (Convert.ToDouble(txtBatchWeight.Text) < Convert.ToDouble(Floordata.MinBatchWeight) || Convert.ToDouble(txtBatchWeight.Text) > Convert.ToDouble(Floordata.MaxBatchWeight)))
            {
                chkBatch = true;
            }
            if (chkBatch && chkTenPcs)
            {
                message = string.Format(Constants.TENPCS_BATCH_MESSAGE, txtTenPcs.Text, txtBatchWeight.Text);
                _authorizedFor = Constants.THREE;
            }
            else if (chkTenPcs)
            {
                message = string.Format(Constants.TENPCS_MESSAGE, txtTenPcs.Text);
                _authorizedFor = Constants.ONE;
            }
            else if (chkBatch)
            {
                message = string.Format(Constants.BATCHWEIGHT_MESSAGE, txtBatchWeight.Text);
                _authorizedFor = Constants.TWO;
            }
            else
            {
                result = true;
            }
            if (!result)
            {
                Login passwordForm = new Login(Constants.Modules.POSTTREATMENT, _screenName, string.Empty, true, message, chkBatch, chkTenPcs);
                passwordForm.Authentication = String.Empty;
                passwordForm.IsCancel = false;
                passwordForm.ShowDialog();
                if (passwordForm.Authentication != String.Empty)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Validate 10 Pcs Weight based on Glove Type and Size by getting base values from AX
        /// </summary>
        private void ValidateTenPcs()
        {
            try
            {
                TenPcsDTO weight = new TenPcsDTO();
                weight = CommonBLL.GetMinMaxTenPcsWeight(_gloveType, txtSize.Text);
                WorkStationDataConfiguration data = WorkStationDataConfiguration.GetInstance();
                if (Convert.ToBoolean(data.LWeight) && weight.MinWeight != null)
                {
                    if (Convert.ToBoolean(data.LWeight) && (Convert.ToDouble(txtTenPcs.Text) < Convert.ToDouble(weight.MinWeight) || Convert.ToDouble(txtTenPcs.Text) > Convert.ToDouble(weight.MaxWeight)))
                    {
                        GlobalMessageBox.Show(Constants.TENPCS_WEIGHT_RANGE, Constants.AlertType.Warning, string.Empty, GlobalMessageBoxButtons.OK);
                        lblTenPcs.Visible = true;
                    }
                    else
                        lblTenPcs.Visible = false;
                }
                else
                    lblTenPcs.Visible = false;
                //***TBC - Made editable for only Testing Purposes - To Comment
                //Start

                //End
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ValidateTenPcs", string.Empty);
                return;
            }
        }

        /// <summary>
        /// Make visibility of the Rework Reason Controls based on the visible value passed
        /// </summary>
        /// <param name="visible"></param>
        private void ReasonControlsVisibility(bool visible)
        {
            lblReworkProcess.Visible = visible;
            lblReworkReason.Visible = visible;
            lblReworkCount.Visible = visible;
            cmbReworkProcess.Visible = visible;
            cmbReworkReason.Visible = visible;
            txtReworkCount.Visible = visible;
            if (visible)
            {
                BindReasons();
            }
        }

        /// <summary>
        /// To get 10 PCs weight
        /// </summary>
        /// <returns></returns>
        /// TBD - This is floor system exception because this would be through Integration component
        private void GetTenPcsWeight()
        {
            try
            {
                txtTenPcs.Text = CommonBLL.GetTenPcsWeight().ToString("#,##0.00");
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getTenPcsWeight", null);
                return;
            }

        }

        /// <summary>
        /// To get Batch Weight
        /// </summary>
        /// <returns></returns>
        private void GetBatchWeight()
        {
            try
            {
                //Added by Tan Wei Wah 20190325
                if (!IsWaterTightBatch())
                {
                    double _ten_pcs_weight = Convert.ToDouble(txtTenPcs.Text);
                    txtBatchWeight.Text = Convert.ToDouble(_total_pcs * _ten_pcs_weight / 10000).ToString("#,##0.00");
                }
                else
                {
                    txtBatchWeight.Text = string.Format(Constants.NUMBER_FORMAT, CommonBLL.GetBatchWeight().ToString("#,##0.00"));
                }
                //Ended by Tan Wei Wah 20190325
                ValidateBatchWeight();
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
        /// <returns></returns>
        private void ValidateBatchWeight()
        {
            WorkStationDataConfiguration data = WorkStationDataConfiguration.GetInstance();
            FloorSystemConfiguration Floordata = FloorSystemConfiguration.GetInstance();
            if (Convert.ToBoolean(data.LWeight) && (Convert.ToDouble(txtBatchWeight.Text) < Convert.ToDouble(Floordata.MinBatchWeight) || Convert.ToDouble(txtBatchWeight.Text) > Convert.ToDouble(Floordata.MaxBatchWeight)))
            {
                GlobalMessageBox.Show(Constants.BATCHWEIGHT_RANGE, Constants.AlertType.Warning, string.Empty, GlobalMessageBoxButtons.OK);
                lblBatchWeight.Visible = true;
            }
            else
                lblBatchWeight.Visible = false;
        }

        /// <summary>
        /// Clear the values of all the controls. 
        /// </summary>
        private void ClearForm()
        {
            _pressCancel = true;
            txtSerialNo.Text = String.Empty;
            txtBatchNo.Text = String.Empty;
            txtSize.Text = String.Empty;
            txtQCType.Text = String.Empty;
            txtQCTypeDesc.Text = String.Empty;
            txtTenPcs.Text = String.Empty;
            txtBatchWeight.Text = String.Empty;
            cmbReworkReason.SelectedIndex = Constants.MINUSONE;
            cmbReworkProcess.SelectedIndex = Constants.MINUSONE;
            txtReworkCount.Text = String.Empty;
            txtSerialNo.Focus();
            btnSave.Enabled = false;
            ReasonControlsVisibility(false);
            GetShiftName();
            lblTenPcs.Visible = false;
            lblBatchWeight.Visible = false;
            _authorizedFor = Constants.ZERO;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, null);
                return;
            }
            _isSaved = false;
        }

        /// <summary>
        /// Get the Batch details
        /// </summary>        
        private void GetBatchDetails()
        {
            if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                if (Validator.IsValidInput(Constants.ValidationType.Integer, txtSerialNo.Text.Trim()) & txtSerialNo.Text.Length == Constants.SERIAL_LENGTH)
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

                        BatchDTO objBatch = CommonBLL.GetBatchScanInDetails(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                        if (objBatch != null)
                        {
                            //#AZRUL 09/01/2019: Block batch card to proceed Washer if PTQI Test Result is Pass and no QCQI. START
                            QITestResultStatusDto qiTestResultStatus = QAIBLL.Get_PT_Or_QC_QITestResult(Convert.ToDecimal(txtSerialNo.Text));
                            if (!string.IsNullOrEmpty(qiTestResultStatus.Stage))
                            {
                                if (qiTestResultStatus.Stage == Constants.PT_QI && (qiTestResultStatus.QIResult == Constants.PASS)
                                    && !CommonBLL.ValidateAXPosting(Convert.ToDecimal(txtSerialNo.Text), CreateRAFJournalFunctionidentifier.SOBC.ToString()))
                                {
                                    GlobalMessageBox.Show(Messages.PTQI_COMPLETED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                    return;
                                }
                            }
                            //#AZRUL 09/01/2019: END
                            string qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                            if (!string.IsNullOrEmpty(qaiStatus))
                            {
                                GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ClearForm();
                                return;
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(PostTreatmentBLL.ValidateSerialNoPT(Convert.ToDecimal(txtSerialNo.Text.Trim()))))
                                {
                                    _pressCancel = false;
                                    btnSave.Enabled = true;
                                    txtBatchNo.Text = objBatch.BatchNumber;
                                    txtSize.Text = objBatch.Size;
                                    txtQCType.Text = objBatch.QCType;
                                    txtQCTypeDesc.Text = objBatch.QCTypeDescription;
                                    _gloveType = objBatch.GloveType;
                                    _batchType = objBatch.BatchType.Replace(" ", "");
                                    _totalPcs = Math.Round(PostTreatmentBLL.GetPTLatestBatchWeight(Convert.ToDecimal(txtSerialNo.Text)) * Constants.THOUSAND * Constants.TEN / PostTreatmentBLL.GetPTLatestTenPcsWeight(Convert.ToDecimal(txtSerialNo.Text)), Constants.ZERO);
                                    if (objBatch.ReworkCount >= Constants.ZERO)
                                    {
                                        string confirm = GlobalMessageBox.Show(Messages.REWORK_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
                                        if (confirm == Constants.NO)
                                        {
                                            _isRework = false;
                                            ClearForm();
                                            _isSaved = true;
                                            return;
                                        }
                                        else
                                        {
                                            _isRework = true;
                                            ReasonControlsVisibility(true);
                                            txtReworkCount.Text = Convert.ToString(objBatch.ReworkCount + Constants.ONE);
                                            _serialNo = txtSerialNo.Text;
                                        }
                                    }
                                    else
                                    {
                                        _isRework = false;
                                        ReasonControlsVisibility(false);
                                        txtReworkCount.Text = string.Empty;
                                        _serialNo = txtSerialNo.Text;
                                    }
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.PASS_SERIALNUMBER_WASHERDRYER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            return;
                        }

                        //Added by Tan Wei Wah 20190213
                        _batch_type = PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text.Trim()));

                        if (IsWaterTightBatch())
                        {
                            txtBatchWeight.ReadOnly = false;
                        }
                        else
                        {
                            txtBatchWeight.ReadOnly = true;
                            //_total_pcs = PostTreatmentBLL.GetLatestTotalPcs(Convert.ToDecimal(txtSerialNo.Text.Trim()));   //Change due to round issue if get total pcs from GIS
                            _total_pcs = Convert.ToInt32(_totalPcs);
                        }
                        //Ended by Tan Wei Wah 20190213
                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "GetBatchDetails", txtSerialNo.Text.Trim());
                        return;
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
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
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else if (floorException.subSystem == Constants.AXSERVICEERROR)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.AXPOSTINGERROR + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else if (floorException.subSystem == Constants.SERVICEERROR)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.SERVICE_UNAVAILABLE, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else if (uiControl == "getTenPcsWeight")
                GlobalMessageBox.Show("(" + result + ") - " + Constants.TENPCS_WEIGHT_ERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else if (uiControl == "getBatchWeight")
                GlobalMessageBox.Show("(" + result + ") - " + Constants.BATCH_WEIGHT_ERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);

            ClearForm();
        }
        #endregion

        private void btnCancel_Validated(object sender, EventArgs e)
        {
            txtSerialNo.Focus();
        }

        private bool IsWaterTightBatch()
        {
            bool _result = false;
            if (_batch_type == Constants.WT || _batch_type == Constants.PWT || _batch_type == Constants.QWT || _batch_type == Constants.OWT || _batch_type == Constants.PSW)
            {
                _result = true;
            }

            return _result;
        }

        private Boolean ValidatePTScanInSerialNumber(bool isClearForm)
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                if (Validator.IsValidInput(Constants.ValidationType.Integer, txtSerialNo.Text.Trim()) & txtSerialNo.Text.Length == Constants.SERIAL_LENGTH)
                {
                    decimal serialNumber = Convert.ToDecimal(txtSerialNo.Text.Trim());
                    string errorMessage = PostTreatmentBLL.GetPTScanValidationErrorMessage(serialNumber);

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
                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_EMPTY, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                if (isClearForm)
                    ClearForm();
            }

            return isValid;
        }

        private void txtQCType_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private bool IsTotalPcsExceed()
        {
            bool _result = false;
            if (!IsWaterTightBatch())
            {
                _result = true;
            }
            else
            {
                if (_totalPcs >= Convert.ToDecimal(Convert.ToDecimal(txtBatchWeight.Text) * Constants.THOUSAND * Constants.TEN / Convert.ToDecimal(txtTenPcs.Text)))
                {
                    _result = true;
                }
            }

            return _result;
        }
    }
}
