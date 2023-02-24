// -----------------------------------------------------------------------
// <copyright file="OnlineByPassGloveBatchCard.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Collections.Generic;
using Hartalega.FloorSystem.Framework.Common;

namespace Hartalega.FloorSystem.Windows.UI.Tumbling
{
    /// <summary>
    /// Module: Tumbling
    /// Screen Name: Online ByPass Glove Batch Card
    /// File Type: Code file
    /// </summary> 
    public partial class OnlineByPassGloveBatchCard : FormBase
    {
        #region Static Variables
        private static string _screenName = "Tumbling - OnlineByPassGloveBatchCard";
        private static string _className = "OnlineByPassGloveBatchCard";
        private static bool _chckTenPcs = false;
        private static bool _chckBatchWeight = false;
        #endregion

        #region Load Page
        /// <summary>
        /// Initializes a new instance of the <see cref="OnlineByPassGlove" /> class.
        /// </summary>
        public OnlineByPassGloveBatchCard()
        {
            InitializeComponent();
            txtOperatorId.OperatorId();
            txtSerialNumber.SerialNo();
        }
        #endregion

        #region Load Form
        /// <summary>
        /// Form load event
        /// </summary>
        /// <param name="sender">Request object</param>
        /// <param name="e">Event argument</param>
        private void OnlineByPassGloveBatchCard_Load(object sender, System.EventArgs e)
        {
            try
            {
                // bind reason dropdown
                GetReasons();
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {
                    txtTenPcsWeight.ReadOnly = true;
                    txtTenPcsWeight.TabStop = false;
                    txtBatchWeight.ReadOnly = true;
                    txtBatchWeight.TabStop = false;
                }
                else
                {
                    txtTenPcsWeight.ReadOnly = false;
                    txtTenPcsWeight.TabStop = true;
                    txtBatchWeight.ReadOnly = false;
                    txtBatchWeight.TabStop = true;
                }
                txtOperatorId.Focus();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "OnlineByPassGloveBatchCard_Load", Constants.BYPASS_REASON_TYPE, Constants.TUMBLING_SYSTEM);
                return;
            }
        }
        #endregion

        #region Fill Sources
        private void GetReasons()
        {
            try
            {
                cmbBoxReason.DataSource = CommonBLL.GetReasons(Constants.BYPASS_REASON_TYPE, WorkStationDTO.GetInstance().Module);
                cmbBoxReason.DisplayMember = "DisplayField";
                cmbBoxReason.SelectedItem = "IDField";
                cmbBoxReason.SelectedIndex = -1;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetReasons", WorkStationDTO.GetInstance().Location, Framework.Constants.ShiftGroup.PN);
                return;
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Operator id text box leave event
        /// </summary>
        /// <param name="sender">Operator id text box</param>
        /// <param name="e">On leave event argument</param>
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
                lblOperatorName.Text = String.Empty;
        }

        /// <summary>
        /// Operator id text box leave event
        /// </summary>
        /// <param name="sender">Operator id text box</param>
        /// <param name="e">On leave event argument</param>
        private void txtSerialNumber_Leave(object sender, EventArgs e)
        {
            BatchDTO resultDTO = null;
            if (Framework.Common.Validator.IsValidInput(Constants.ValidationType.Integer, txtSerialNumber.Text) & txtSerialNumber.Text.Length == Constants.SERIAL_LENGTH)
            {
                decimal serialNumber = txtSerialNumber.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtSerialNumber.Text);
                if (serialNumber > 0)
                {
                    try
                    {
                        resultDTO = CommonBLL.ValidateAndGetDetailsBySerialNumber(serialNumber);
                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "txtSerialNumber_Leave", txtSerialNumber.Text);
                        return;
                    }
                    if (resultDTO.QCType != null && resultDTO.QCType != String.Empty)
                    {
                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_ONLINEBYPASS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearForm(Constants.EXCLUDE_OPERATOR);
                        txtSerialNumber.Focus();
                        return;
                    }
                    if (resultDTO.BatchNumber != null)
                    {
                        txtBatchNo.Text = resultDTO.BatchNumber;
                        txtGloveType.Text = resultDTO.GloveType;
                        txtGloveDescription.Text = resultDTO.GloveTypeDescription;
                        txtSize.Text = resultDTO.Size;
                        txtShift.Text = resultDTO.ShiftName;
                        txtBatchDate.Text = Convert.ToDateTime(resultDTO.ShortDate).ToString("dd/MM/yyyy");
                        txtTime.Text = resultDTO.ShortTime;
                        lblSide.Text = resultDTO.Side;
                         if (String.IsNullOrEmpty(txtTenPcsWeight.Text))
                             txtTenPcsWeight.Focus();
                      
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearForm(Constants.EXCLUDE_OPERATOR);
                        txtSerialNumber.Focus();
                    }
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                ClearForm(Constants.EXCLUDE_OPERATOR);
                txtSerialNumber.Focus();
            }
        }

        private void txtTenPcsWeight_Enter(object sender, EventArgs e)
         {
             if (txtTenPcsWeight.Text == String.Empty)
             {
                 if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                 {
                     txtTenPcsWeight.ReadOnly = true;
                     lblPcsWeight.Visible = true;
                     GetTenPcsWeight();
                     txtTenPcsWeight_Leave(sender, e);
                     txtBatchWeight.Focus();
                 }
                 else
                 {
                     txtTenPcsWeight.ReadOnly = false;
                     txtTenPcsWeight.Focus();
                 }
             }
        }


        /// <summary>
        /// To Save and Print batch Card details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            int authorizedFor = Constants.ZERO;
            if (ValidateRequiredFields())
            {
                if (txtTenPcsWeight.Text == String.Empty && txtBatchWeight.Text == String.Empty)
                {
                    txtSerialNumber_Leave(this, new EventArgs());
                    btnPrint_Click(this, new EventArgs());
                }
                else
                {
                    // Recheck functionality
                    if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                    {
                        GetTenPcsWeight();
                        //ValidateTenPcsWeight();
                        //Added by Tan Wei Wah 28/02/2019 for Negative input validation
                        decimal decim1;
                        if (Decimal.TryParse(txtTenPcsWeight.Text, out decim1) && decim1 > 0)
                        {
                            txtTenPcsWeight.Text = Convert.ToDouble(txtTenPcsWeight.Text).ToString("#,##0.00");
                            ValidateTenPcsWeight();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_10PCS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtTenPcsWeight.Text = String.Empty;
                            txtTenPcsWeight.Focus();
                            return;
                        }
                        //Ended by Tan Wei Wah 28/02/2019

                        GetBatchWeight();
                        //ValidateBatchWeight();
                        //Added by Tan Wei Wah 28/02/2019 for Negative input validation
                        decimal decim2;
                        if (Decimal.TryParse(txtBatchWeight.Text, out decim2) && decim2 > 0)
                        {
                            txtBatchWeight.Text = Convert.ToDouble(txtBatchWeight.Text).ToString("#,##0.00");
                            ValidateBatchWeight();
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
                        if (_chckBatchWeight && _chckTenPcs)
                        {
                            message = string.Format(Constants.TENPCS_BATCH_MESSAGE, txtTenPcsWeight.Text, txtBatchWeight.Text);
                            authorizedFor = Constants.THREE;
                        }
                        else if (_chckTenPcs)
                        {
                            message = string.Format(Constants.TENPCS_MESSAGE, txtTenPcsWeight.Text);
                            authorizedFor = Constants.ONE;
                        }
                        else if (_chckBatchWeight)
                        {
                            message = string.Format(Constants.BATCHWEIGHT_MESSAGE, txtBatchWeight.Text);
                            authorizedFor = Constants.TWO;
                        }
                        if (message != String.Empty)
                        { // added by MYAdamas 20171016 due to if not out of range still prompt password needed
                            Enabled = false;
                            Login passwordForm = new Login(Constants.Modules.TUMBLING, _screenName, string.Empty, true, message, _chckBatchWeight, _chckTenPcs);
                            passwordForm.Authentication = String.Empty;
                            passwordForm.IsCancel = false;
                            passwordForm.ShowDialog();
                            if (passwordForm.Authentication != String.Empty)
                            {
                                SaveBatchCard(passwordForm.Authentication, authorizedFor);
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
                            SaveBatchCard(string.Empty, Constants.ZERO);
                            ClearForm(String.Empty);
                            txtOperatorId.Focus();
                        }
                    }
                }
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
                txtOperatorId.Focus();
            }
        }

        #endregion

        #region User Methods

        /// <summary>
        /// Save Batch card details if valid, 
        /// the SP called in BLL also returns the serial number and batch number useful for printing
        /// </summary>
        /// <param name="authorizedBy"></param>
        /// <returns></returns>
        private void SaveBatchCard(string authorizedBy,int authorizedFor)
        {
            try
            {
                string result = TumblingBLL.SaveOnlineByPassGloveBatch(txtSerialNumber.Text, Convert.ToDouble(txtTenPcsWeight.Text), Convert.ToDouble(txtBatchWeight.Text), ((DropdownDTO)(cmbBoxReason.SelectedValue)).IDField, authorizedBy, txtOperatorId.Text.ToString(), WorkStationDTO.GetInstance().WorkStationId, authorizedFor);
                if (result != null)
                {
                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    //event log myadamas 20190227
                    EventLogDTO EventLog = new EventLogDTO();

                    EventLog.CreatedBy = Convert.ToString(txtOperatorId.Text);
                    Constants.EventLog audAction = Constants.EventLog.Print;
                    EventLog.EventType = Convert.ToInt32(audAction);
                    EventLog.EventLogType = Constants.eventlogtype;

                    var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                    CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());
                    // To get GloveCategory
                    string _GloveCategory;
                    _GloveCategory = TumblingBLL.GetGloveCategory(txtGloveType.Text);

                    CommonBLL.PrintDetails(ServerCurrentDateTime.ToLongTimeString(), txtSerialNumber.Text.ToString(), txtBatchNo.Text.ToString(), txtBatchWeight.Text.ToString(), txtSize.Text.ToString(), txtTenPcsWeight.Text.ToString(), lblPcsWeight.Visible, txtGloveType.Text.ToString() + Environment.NewLine + Constants.TAB + _GloveCategory, lblSide.Text.ToString(), String.Empty, false, _chckBatchWeight);
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "SaveBatchCard", txtSerialNumber.Text, txtTenPcsWeight.Text, txtBatchWeight.Text);
                return;
            }
        }

        /// <summary>
        /// To clear sub form when criteria doesn't match
        /// </summary>
        private void ClearSubForm()
        {
            txtGloveType.Text = String.Empty;
            txtGloveDescription.Text = String.Empty;
            txtSerialNumber.Text = String.Empty;
            txtBatchNo.Text = String.Empty;
            txtSize.Text = String.Empty;
            txtShift.Text = String.Empty;
            txtBatchDate.Text = String.Empty;
            txtTime.Text = String.Empty;
            txtTenPcsWeight.Text = String.Empty;
            txtBatchWeight.Text = String.Empty;
            lblPcsWeight.Visible = false;
            lblBatchWeight.Visible = false;
            cmbBoxReason.SelectedIndex = 0;
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
                txtTenPcsWeight.Text = CommonBLL.GetTenPcsWeight().ToString("#,##0.00");
                lblPcsWeight.Visible = false;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getTenPcsWeight", null);
                return;
            }
            lblPcsWeight.Visible = false;
            txtBatchWeight.Focus();
        }

        /// <summary>
        /// Validate 10 Pcs Weight based on Glove Type and Size by getting base values from AX
        /// </summary>
        private void ValidateTenPcsWeight()
        {
            TenPcsDTO weight = new TenPcsDTO();
            try
            {
                weight = CommonBLL.GetMinMaxTenPcsWeight(txtGloveType.Text, txtSize.Text);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "validateTenPcsWeight", txtSize.Text, txtGloveType.Text);
                return;
            }
            if (Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().LWeight) && weight.MinWeight != null)
            {
                if (Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().LWeight) && (Convert.ToDouble(txtTenPcsWeight.Text) < Convert.ToDouble(weight.MinWeight) || Convert.ToDouble(txtTenPcsWeight.Text) > Convert.ToDouble(weight.MaxWeight)))
                {
                    GlobalMessageBox.Show(Constants.TENPCS_WEIGHT_RANGE, Constants.AlertType.Information, string.Empty, GlobalMessageBoxButtons.OK);
                    lblPcsWeight.Text = Constants.WEIGHT_OUT_OF_RANGE;
                    //add visible true MYAdamas 2017/10/16 due to out of range label not showed if out of range
                    lblPcsWeight.Visible = true;
                    _chckTenPcs = true;
                }
                else
                {
                    lblPcsWeight.Visible = false;
                    _chckTenPcs = false;
                }
            }
        }

        /// <summary>
        /// To get Batch Weight by calling the method in the Integration class through BLL
        /// </summary>
        /// <returns></returns>
        private void GetBatchWeight()
        {
            try
            {
                lblBatchWeight.Visible = true;
                lblBatchWeight.Text = Constants.CONNECTING;
                txtBatchWeight.Text = CommonBLL.GetBatchWeight().ToString("#,##0.00");
                lblBatchWeight.Visible = false;
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
        private void ValidateBatchWeight()
        {
            if (Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().LWeight) && (Convert.ToDouble(txtBatchWeight.Text) < Convert.ToDouble(FloorSystemConfiguration.GetInstance().MinBatchWeight) ||
                Convert.ToDouble(txtBatchWeight.Text) > Convert.ToDouble(FloorSystemConfiguration.GetInstance().MaxBatchWeight)))
            {
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
            txtOperatorId.Leave -= new EventHandler(txtOperatorId_Leave);
            txtSerialNumber.Leave -= new EventHandler(txtSerialNumber_Leave);
            if (excludeOperator != Constants.EXCLUDE_OPERATOR)
            {
                txtOperatorId.Text = String.Empty;
                lblOperatorName.Text = String.Empty;
            }
            txtGloveType.Text = String.Empty;
            txtGloveDescription.Text = String.Empty;
            txtTenPcsWeight.Text = String.Empty;
            txtBatchWeight.Text = String.Empty;
            lblPcsWeight.Visible = false;
            lblBatchWeight.Visible = false;
            txtBatchNo.Text = String.Empty;
            txtSize.Text = String.Empty;
            txtShift.Text = String.Empty;
            txtBatchDate.Text = String.Empty;
            txtTime.Text = String.Empty;
            txtSerialNumber.Text = String.Empty;
            cmbBoxReason.SelectedIndex = -1;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ClearForm", null);
            }
            GetReasons();
            txtOperatorId.Focus();
            txtOperatorId.Leave += new EventHandler(txtOperatorId_Leave);
            txtSerialNumber.Leave += new EventHandler(txtSerialNumber_Leave);

        }

        /// <summary>
        /// Validate required fields
        /// </summary>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtOperatorId, Constants.OPERATORID, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtSerialNumber, Constants.SERIALNO, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtTenPcsWeight, Constants.TENPCS, ValidationType.Required));
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
                if (uiControl == "getTenPcsWeight")
                    GlobalMessageBox.Show("(" + result + ") - " + Constants.TENPCS_WEIGHT_ERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                else if (uiControl == "getBatchWeight")
                    GlobalMessageBox.Show("(" + result + ") - " + Constants.BATCH_WEIGHT_ERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm(String.Empty);
        }

        private void txtTenPcsWeight_Leave(object sender, EventArgs e)
        {
            if (txtTenPcsWeight.Text != String.Empty)
            {
                //added by MYAdamas 11/10/2017 for input validation
                //Modified by Lakshman 7/1/2018 for Negative input validation
                decimal decim; 
                if (Decimal.TryParse(txtTenPcsWeight.Text, out decim) && decim > 0)
                {
                    txtTenPcsWeight.Text = Convert.ToDouble(txtTenPcsWeight.Text).ToString("#,##0.00");
                    ValidateTenPcsWeight();
                    
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_10PCS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    txtTenPcsWeight.Text = String.Empty;
                    txtTenPcsWeight.Focus();

                }
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
                    txtBatchWeight.Text = Convert.ToDouble(txtBatchWeight.Text).ToString("#,##0.00");
                    ValidateBatchWeight();
                   
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
            if (txtBatchWeight.Text == String.Empty && txtTenPcsWeight.Text != String.Empty)
            {
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {
                    txtBatchWeight.ReadOnly = true;
                    lblBatchWeight.Visible = true;
                    GetBatchWeight();
                    txtBatchWeight_Leave(sender, e);
                }
                else
                {
                    txtBatchWeight.ReadOnly = false;
                    txtBatchWeight.Focus();
                }
            }
        }
        #endregion
    }
}
