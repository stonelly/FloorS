// -----------------------------------------------------------------------
// <copyright file="OnlineRejectGlove.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.Common;
using System.Drawing;
using System.Transactions;

namespace Hartalega.FloorSystem.Windows.UI.Tumbling
{
    /// <summary>
    /// Module: Tumbling
    /// Screen Name: Reject Glove
    /// File Type: Code file
    /// </summary> 
    public partial class OnlineRejectGlove : FormBase
    {
        #region Static Variables
        public static BatchDTO _resultDTO;
        public BatchDTO _onlineRejectionBatchDTO;
        public static string _previousGloveType;
        private static string _screenName = "Tumbling - RejectGlove";
        private static string _className = "RejectGlove";
        private static bool _chckBatchWeight = false;
        private static string _moduleName;
        #endregion

        #region Load Page
        /// <summary>
        /// Initializes a new instance of the <see cref="RejectGlove" /> class.
        /// </summary>
        public OnlineRejectGlove(string moduleName)
        {
            InitializeComponent();
            txtOperatorId.OperatorId();
            if (moduleName == Constants.QC_SCANNING_SYSTEM)
            {
                _screenName = "QCScanning - RejectGlove";
                WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.QC_SCANNING_SYSTEM);
                cmbBoxLine.Enabled = false;
                cmbBoxShift.Enabled = false;
                _moduleName = moduleName;
            }
            else
            {
                _moduleName = string.Empty;
                _screenName = "Tumbling - RejectGlove";
                WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.TUMBLING_SYSTEM);
            }
            _onlineRejectionBatchDTO = new BatchDTO();
        }
        #endregion

        #region Load Form
        /// <summary>
        /// Form load event - Fill Dropdowns Line,Size and Shift and assign the current date to label
        /// </summary>
        /// <param name="sender">Request object</param>
        /// <param name="e">Event argument</param>
        private void OnlineRejectGlove_Load(object sender, System.EventArgs e)
        {
            // lblOperatorName.Visible = false;
            try
            {
                GetLineShiftAndReason();
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
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "RejectGlove_Load", null);
                return;
            }
            txtOperatorId.Focus();
        }
        #endregion

        #region Fill Sources
        /// <summary>
        /// Fill Line shift and Reason
        /// </summary>
        private void GetLineShiftAndReason()
        {
            try
            {
                // Bind line data
                List<DropdownDTO> LineDTO = CommonBLL.GetAllLinesByLocation(WorkStationDTO.GetInstance().Location);
                LineDTO.Add(new DropdownDTO() { DisplayField = Constants.USED_GLOVE });
                cmbBoxLine.DataSource = LineDTO;
                cmbBoxLine.DisplayMember = "DisplayField";
                cmbBoxLine.SelectedIndex = -1;

                // Bind shift data
                List<DropdownDTO> shiftDTO = CommonBLL.GetShift(Framework.Constants.ShiftGroup.PN);
                shiftDTO.RemoveAll(r => r.DisplayField == "C");
                cmbBoxShift.DataSource = shiftDTO;
                cmbBoxShift.DisplayMember = "DisplayField";
                cmbBoxShift.SelectedItem = "IDField";
                cmbBoxShift.Text = shiftDTO[Constants.ZERO].SelectedValue;

                if (_moduleName == Constants.QC_SCANNING_SYSTEM)
                {
                    cmbBoxShift.SelectedIndex = Constants.MINUSONE;
                }

                //bind reason dropdown
                cmbBoxReason.DataSource = CommonBLL.GetReasons(Constants.REJECT_REASON_TYPE, WorkStationDTO.GetInstance().Module);
                cmbBoxReason.DisplayMember = "DisplayField";
                cmbBoxReason.SelectedItem = "IDField";
                cmbBoxReason.SelectedIndex = -1;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getLineAndShift", WorkStationDTO.GetInstance().Location, Constants.BYPASS_REASON_TYPE, Constants.TUMBLING_SYSTEM);
                return;
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// This event will allow only Numbers in OperatorID textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperatorId_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        /// <summary>
        /// Method to get Glove Type Description 
        /// </summary>
        /// <param name="sender">Glove type text box leave event</param>
        /// <param name="e">On leave event argument</param>   
        private void cmbGloveType_Leave(object sender, EventArgs e)
        {
            GetGloveDescriptionIfValid();
        }

        /// <summary>
        /// To Save and Print batch Card details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            if (validateRequiredFields())
            {
                if (txtBatchWeight.Text == String.Empty)
                {
                    cmbGloveType_Leave(this, new EventArgs());
                    cmbGloveType_Validated(this, new EventArgs());
                    btnPrint_Click(this, new EventArgs());
                }
                else
                {
                    // Recheck functionality
                    if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                    {
                        getBatchWeight();
                        //validateBatchWeight();
                        //Added by Tan Wei Wah 28/02/2019 for Negative input validation
                        decimal decim2;
                        if (Decimal.TryParse(txtBatchWeight.Text, out decim2) && decim2 > 0)
                        {
                            txtBatchWeight.Text = Convert.ToDouble(txtBatchWeight.Text).ToString("#,##0.00");
                            validateBatchWeight();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_BATCH, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtBatchWeight.Text = String.Empty;
                            txtBatchWeight.Focus();
                            return;
                        }
                        //Ended by Tan Wei Wah 28/02/2019
                    }

                    if (GlobalMessageBox.Show(Messages.CONFIRM_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                    {
                        if (_chckBatchWeight)
                        {
                            message = string.Format(Constants.BATCHWEIGHT_MESSAGE, txtBatchWeight.Text);
                        }
                        if (message != String.Empty)
                        {
                            Enabled = false;
                            Login passwordForm = new Login(Constants.Modules.TUMBLING, _screenName, string.Empty, true, message, _chckBatchWeight);
                            passwordForm.Authentication = String.Empty;
                            passwordForm.IsCancel = false;
                            passwordForm.ShowDialog();
                            if (passwordForm.Authentication != String.Empty)
                            {
                                SaveBatchCard(passwordForm.Authentication);
                                Enabled = true;
                                ClearForm(String.Empty);
                                txtOperatorId.Focus();
                            }
                            else if (passwordForm.Authentication == String.Empty && passwordForm.IsCancel != true)
                            {
                                Enabled = true;
                                ClearForm(String.Empty);
                                txtOperatorId.Focus();
                            }
                            else if (passwordForm.IsCancel == true)
                                Enabled = true;
                        }
                        else
                        {
                            Enabled = true;
                            SaveBatchCard(String.Empty);
                            ClearForm(String.Empty);
                            txtOperatorId.Focus();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// OperatorId validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperatorId_Leave(object sender, EventArgs e)
        {
            string operatorId = txtOperatorId.Text.Trim();
            if (operatorId != String.Empty)
            {
                try
                {
                    if (SecurityModuleBLL.ValidateOperatorAccess(operatorId, _screenName))
                    {
                        lblOperatorName.Text = CommonBLL.GetOperatorNameQAI(operatorId);
                        if (lblOperatorName.Text == String.Empty)
                        {
                            GlobalMessageBox.Show(Messages.INVALID_OPERATOR_ID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtOperatorId.Text = String.Empty;
                            lblOperatorName.Text = String.Empty;
                            txtOperatorId.Focus();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.OPERATOR_SCREEN_ACCESS_MSG, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        txtOperatorId.Text = String.Empty;
                        lblOperatorName.Text = String.Empty;
                        txtOperatorId.Focus();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "txtOperatorId_Leave", txtOperatorId.Text);
                    return;
                }
            }
            else
            {
                lblOperatorName.Text = String.Empty;
            }
        }

        /// <summary>
        /// Clears the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE_QAI, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm(String.Empty);
            }
        }

        /// <summary>
        /// Validate Glove description 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBoxLine_Leave(object sender, EventArgs e)
        {
            if (cmbBoxLine.SelectedIndex != -1)
                GetGloveTypes();
        }

        /// <summary>
        /// Weigh Batch Weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBatchWeight_Enter(object sender, EventArgs e)
        {
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
            {
                lblBatchWeight.Visible = true;
                getBatchWeight();
                lblBatchWeight.Visible = false;
                cmbBoxReason.Focus();
            }
            else
            {
                txtBatchWeight.ReadOnly = false;
                txtBatchWeight.Focus();
            }
        }

        private void txtBatchWeight_Leave(object sender, EventArgs e)
        {
            if (txtBatchWeight.Text != String.Empty)
            {
                //added by MYAdamas 11/10/2017 for input validation
                //Modified by Lakshman 7/1/2018 for Negative input validation
                decimal decim;
                if (Decimal.TryParse(txtBatchWeight.Text, out decim) && decim > 0)
                {
                    validateBatchWeight();
                    txtBatchWeight.Text = Convert.ToDouble(txtBatchWeight.Text).ToString("#,##0.00");
                    cmbBoxReason.Focus();
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_BATCH, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    txtBatchWeight.Text = String.Empty;
                    txtBatchWeight.Focus();

                }
            }
        }

        /// <summary>
        /// Focus to Ten Pcs Weight 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGloveType_Validated(object sender, EventArgs e)
        {
            if (cmbGloveType.Text != String.Empty)
            {
                if (txtBatchWeight.Text == String.Empty)
                    txtBatchWeight.Focus();
            }
        }

        private void cmbGloveType_KeyPress(object sender, KeyPressEventArgs e)
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
        # endregion

        #region User Methods


        private void GetGloveTypes()
        {
            List<DropdownDTO> gloveDTO = TumblingBLL.GetRejectGloveByLine(cmbBoxLine.Text);
            if (gloveDTO == null)
            {
                GlobalMessageBox.Show((string.Format("No Glove Type exists for {0}!", cmbBoxLine.Text)), Constants.AlertType.Error, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                cmbGloveType.SelectedIndex = -1;
                this.ActiveControl = cmbBoxLine;
                return;
            }
            cmbGloveType.DataSource = gloveDTO;
            cmbGloveType.DisplayMember = "DisplayField";
            cmbGloveType.SelectedItem = "IDField";
            if (gloveDTO.Count > 1)
            {
                cmbGloveType.SelectedIndex = -1;
            }

        }
        /// <summary>
        /// Get Glove Description if valid and populate Size dropdown if any for the glovetype
        /// </summary>
        private void GetGloveDescriptionIfValid()
        {
            string gloveDescription = String.Empty;
            try
            {
                if (cmbGloveType.Text != String.Empty)
                {
                    txtBatchWeight.Text = String.Empty;
                    cmbBoxReason.SelectedIndex = -1;
                    txtBatchWeight.Focus();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getGloveDescriptionIfValid", cmbGloveType.Text, cmbBoxLine.Text);
                return;
            }
        }

        /// <summary>
        /// Save Batch card details if valid, 
        /// the SP called in BLL also returns the serial number and batch number useful for printing
        /// </summary>
        /// <param name="authorizedBy"></param>
        /// <returns></returns>
        private void SaveBatchCard(string authorizedBy)
        {
            DateTime batchDateTime = ServerCurrentDateTime;
            BatchDTO objBatchDTO = new BatchDTO();
            // if (_moduleName != Constants.QC_SCANNING_SYSTEM)
            //added by MYAdamas 9/10/2017 due to for used glove line and shift should be null
            if (_moduleName != Constants.QC_SCANNING_SYSTEM && cmbBoxLine.Text != Constants.USED_GLOVE)
            {
                objBatchDTO.Shift = Convert.ToInt32(((DropdownDTO)(cmbBoxShift.SelectedValue)).IDField);
                objBatchDTO.Line = cmbBoxLine.Text;
                objBatchDTO.ShiftName = cmbBoxShift.Text;
            }
            objBatchDTO.GloveType = cmbGloveType.Text;
            objBatchDTO.BatchWeight = Convert.ToDecimal(txtBatchWeight.Text);
            objBatchDTO.BatchCarddate = batchDateTime;
            objBatchDTO.OperatorId = String.Empty;
            objBatchDTO.WorkstationNumber = WorkStationDTO.GetInstance().WorkStationNumber;
            objBatchDTO.WorkStationId = WorkStationDTO.GetInstance().WorkStationId;
            objBatchDTO.BatchType = Constants.REJECTGLOVE;
            objBatchDTO.LocationId = WorkStationDTO.GetInstance().LocationId;
            objBatchDTO.Module = WorkStationDTO.GetInstance().Module;
            objBatchDTO.SubModule = WorkStationDTO.GetInstance().SubModule;
            objBatchDTO.Site = FloorSystemConfiguration.GetInstance().intSiteNumber;
            objBatchDTO.RejectReasonId = Convert.ToInt32(((DropdownDTO)(cmbBoxReason.SelectedValue)).IDField);
            objBatchDTO.GloveTypeDescription = cmbGloveType.Text;
            objBatchDTO.ResourceGroup = WorkStationDTO.GetInstance().Location + cmbBoxLine.Text; //_onlineRejectionBatchDTO.ResourceGroup;// for online rejection integration
            objBatchDTO.Warehouse = _onlineRejectionBatchDTO.Warehouse;// for online rejection integration
            try
            {
                // Create the TransactionScope to execute the commands, guaranteeing
                // that both commands can commit or roll back as a single unit of work.
                //https://msdn.microsoft.com/en-us/library/system.transactions.transactionscope(v=vs.110).aspx
                using (TransactionScope scope = new TransactionScope())
                {
                    /* comment off before CAB Approval */
                    //TODO::direct connect to D365
                    //if (!AXPostingBLL.CheckCBDItem(objBatchDTO))
                    //    throw new FloorSystemException(string.Format("Configuration for CPD consumption on {0} not valid on this date && time range!", objBatchDTO.ResourceGroup));                                 
                    _resultDTO = TumblingBLL.SaveRejectSticker(objBatchDTO);

                    // added on 10th Oct 2016 at 5:25 by Max He, MH#1.n
                    // for online rejection integration
                    Boolean isPostingSuccess = false;
                    objBatchDTO.SerialNumber = _resultDTO.SerialNumber;
                    objBatchDTO.BatchNumber = _resultDTO.BatchNumber;
                    isPostingSuccess = AXPostingBLL.PostAXDataOnlineRejectGloves(objBatchDTO);
                    if (!isPostingSuccess)
                    {
                        TumblingBLL.DelCustomerRejectEntry(Convert.ToDecimal(_resultDTO.SerialNumber));
                        GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY_SM, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        // To get GloveCategory
                        if (!String.IsNullOrEmpty(cmbGloveType.Text))
                        {
                            //string _GloveCategory;
                            //_GloveCategory = TumblingBLL.GetRejectGloveCategory(cmb.Text);
                            objBatchDTO.GloveTypeDescription = cmbGloveType.Text;// +Environment.NewLine + Constants.TAB + _GloveCategory;
                        }
                        else
                        {
                            objBatchDTO.GloveTypeDescription = String.Empty;
                        }

                        //event log myadamas 20190227
                        EventLogDTO EventLog = new EventLogDTO();

                        EventLog.CreatedBy = Convert.ToString(txtOperatorId.Text);
                        Constants.EventLog audAction = Constants.EventLog.Print;
                        EventLog.EventType = Convert.ToInt32(audAction);
                        EventLog.EventLogType = Constants.eventlogtype;

                        var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                        CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());

                        CommonBLL.PrintDetails(Constants.TIME + batchDateTime.ToString("HH:mm:ss"), _resultDTO.SerialNumber, Constants.BATCH_TEXT + _resultDTO.BatchNumber, Constants.BATCH_WEIGHT + objBatchDTO.BatchWeight.ToString(), String.Empty, String.Empty, false, objBatchDTO.GloveTypeDescription, String.Empty, Constants.REJECT_GLOVE_SCREEN, false, _chckBatchWeight);

                    }
                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.
                    scope.Complete();
                }
            }
            catch (FloorSystemException ex)
            {
                TumblingBLL.DelCustomerRejectEntry(Convert.ToDecimal(_resultDTO.SerialNumber)); // sould be auto rollback just ensure all records have been deleted
                ExceptionLogging(ex, _screenName, _className, "SaveBatchCard", cmbGloveType.Text, cmbBoxLine.Text, txtBatchWeight.Text);
                GlobalMessageBox.Show(ex.Message, Messages.APPLICATIONERROR, GlobalMessageBoxButtons.OK, SystemIcons.Error);
                return;
            }
        }

        /// <summary>
        /// To get Batch Weight by calling the method in the Integration class through BLL
        /// </summary>
        /// <returns></returns>
        private void getBatchWeight()
        {
            try
            {
                txtBatchWeight.Text = CommonBLL.GetRejectGlovesBatchWeight().ToString("#,##0.00");
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getBatchWeight", null);
                return;
            }
            lblBatchWeight.Visible = false;
        }

        /// <summary>
        /// To validate Batch Weight from WorkStation Specific values
        /// </summary>
        /// <returns></returns>
        private void validateBatchWeight()
        {
            if (Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().LWeight) && (Convert.ToDouble(txtBatchWeight.Text) < Convert.ToDouble(FloorSystemConfiguration.GetInstance().MinBatchWeight) || Convert.ToDouble(txtBatchWeight.Text) > Convert.ToDouble(FloorSystemConfiguration.GetInstance().MaxBatchWeight)))
            {
                //GlobalMessageBox.Show(Constants.BATCHWEIGHT_RANGE, Constants.AlertType.Information, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                GlobalMessageBox.Show(Constants.BATCHWEIGHT_RANGE, Constants.AlertType.Information, string.Empty, GlobalMessageBoxButtons.OK);
                lblBatchWeight.Visible = true;
                lblBatchWeight.Text = Constants.WEIGHT_OUT_OF_RANGE;
                _chckBatchWeight = true;
            }
            else
            {
                lblBatchWeight.Visible = false;
                lblBatchWeight.Text = String.Empty;
                _chckBatchWeight = false;
            }

        }

        /// <summary>
        /// To clear all controls on form
        /// </summary>
        private void ClearForm(string excludeOperator)
        {
            if (excludeOperator != Constants.EXCLUDE_OPERATOR)
            {
                txtOperatorId.Text = String.Empty;
                lblOperatorName.Text = String.Empty;
            }
            cmbGloveType.SelectedIndex = -1;
            //txtGloveDescription.Text = String.Empty;
            txtBatchWeight.Text = String.Empty;
            lblBatchWeight.Visible = false;
            cmbBoxReason.SelectedIndex = -1;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ClearForm", null);
            }
            GetLineShiftAndReason();
            txtOperatorId.Focus();
        }

        /// <summary>
        /// Validate required fields
        /// </summary>
        private bool validateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtOperatorId, Constants.OPERATORID, ValidationType.Required));
            if (_moduleName != Constants.QC_SCANNING_SYSTEM)
            {
                validationMesssageLst.Add(new ValidationMessage(cmbBoxLine, Constants.LINE, ValidationType.Required));
                validationMesssageLst.Add(new ValidationMessage(cmbBoxShift, Constants.SHIFT, ValidationType.Required));
                validationMesssageLst.Add(new ValidationMessage(cmbGloveType, Constants.GLOVETYPE, ValidationType.Required));
            }
            validationMesssageLst.Add(new ValidationMessage(txtBatchWeight, Constants.BATCH, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbBoxReason, Constants.REASON, ValidationType.Required));
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
            else if (floorException.subSystem == Constants.DAL)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                if (uiControl == "getBatchWeight")
                    GlobalMessageBox.Show("(" + result + ") - " + Constants.BATCH_WEIGHT_ERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm(String.Empty);
        }

        #endregion

        /// <summary>
        /// Get resource group when production line changed
        /// added on 11th Oct 2016 at 4:40PM by Max He, MH#1.n
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBoxLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cmbBoxLine.Text))
                {
                    lblResourceGroup.Text = "";
                    _onlineRejectionBatchDTO.ResourceGroup = "";
                    _onlineRejectionBatchDTO.Warehouse = "";
                }
                else
                {
                    var resourceGroup = CommonBLL.GetResourceGroup(cmbBoxLine.Text);
                    if (resourceGroup != null)
                    {
                        lblResourceGroup.Text = resourceGroup.LocationName + "-" + resourceGroup.LineDesc;
                        _onlineRejectionBatchDTO.ResourceGroup = lblResourceGroup.Text;
                        _onlineRejectionBatchDTO.Warehouse = resourceGroup.LocationName + "-CPD";
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "cmbBoxLine_SelectedIndexChanged", cmbBoxLine.Text);
                return;
            }
        }

    }
}