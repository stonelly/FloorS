// -----------------------------------------------------------------------
// <copyright file="PrintWaterTightBatchCard.cs" company="Avanade">
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
using Hartalega.FloorSystem.Framework.Common;
using System.Collections.Generic;

namespace Hartalega.FloorSystem.Windows.UI.Tumbling
{
    /// <summary>
    /// Module: Tumbling
    /// Screen Name: Print WaterTight Batch Card
    /// File Type: Code file
    /// </summary> 
    public partial class PrintWaterTightBatchCard : FormBase
    {
        #region Static Variables
        public static BatchDTO _resultDTO;
        public static string _previousGloveType;
        private static string _screenName = "Tumbling - PrintWaterTightBatchCard";
        private static string _screenNameQC = "QC Scanning - Print Watertight Batch Cards";
        private static string _className = "PrintWaterTightBatchCard";
        private string _moduleName;
        private static bool _chckBatchWeight = false;
        #endregion

        #region Load Page
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintWaterTightBatchCard" /> class.
        /// </summary>
        public PrintWaterTightBatchCard(string moduleName)
        {
            InitializeComponent();
            txtOperatorId.OperatorId();
            if (moduleName == Constants.QC_SCANNING_SYSTEM)
            {
                _screenName = "QC Scanning - Print Watertight Batch Cards";
                _moduleName = Constants.QC_SCANNING_SYSTEM;
            }
        }
        #endregion

        #region Load Form
        /// <summary>
        /// Form load event - Fill Dropdowns Line,Size and Shift and assign the current date to label
        /// </summary>
        /// <param name="sender">Request object</param>
        /// <param name="e">Event argument</param>
        private void PrintWaterTightBatchCard_Load(object sender, System.EventArgs e)
        {
            try
            {
                GetType();
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
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PrintWaterTightBatchCard_Load", null);
                return;
            }
        }
        #endregion

        #region Fill Sources
        /// <summary>
        /// Fill Type
        /// </summary>
        private void GetType()
        {
            try
            {
                //Bind WaterTight Area 
                cmbBoxType.DataSource = CommonBLL.GetEnumText(Constants.WATER_TIGHT_TYPE);
                cmbBoxType.DisplayMember = "DisplayField";
                cmbBoxType.SelectedItem = "IDField";
                cmbBoxType.SelectedIndex = -1;

                //Set the focus field to OperatorID
                txtOperatorId.Focus();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getSizeAndType", null);
                return;
            }
        }
        #endregion  

        #region Event Handlers

        /// <summary>
        /// This method validates the operatorId entered and gets the operator name if valid
        /// </summary>
        /// <param name="sender">Operator id text box</param>
        /// <param name="e">On leave event argument</param>
        private void txtOperatorId_Leave(object sender, EventArgs e)
        {
            string operatorId = txtOperatorId.Text.Trim();
            string screenName = string.Empty;
            if (operatorId != String.Empty)
            {
                try
                {
                    if (string.Compare(_moduleName, Constants.QC_SCANNING_SYSTEM, true) == Constants.ZERO)
                    {
                        screenName = _screenNameQC;
                    }
                    else
                    {
                        screenName = _screenName;
                    }

                    if (SecurityModuleBLL.ValidateOperatorAccess(operatorId, screenName))
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
                    ExceptionLogging(ex, screenName, _className, "txtOperatorId_Leave", txtOperatorId.Text);
                    return;
                }
            }
            else
                lblOperatorName.Text = String.Empty;
        }

        /// <summary>
        /// Method to get Glove Type Description 
        /// </summary>
        /// <param name="sender">Glove type text box leave event</param>
        /// <param name="e">On leave event argument</param>   
        private void txtGloveType_Leave(object sender, EventArgs e)
        {
            GetGloveDescriptionIfValid();
        }

        /// <summary>
        /// Method to get Size Description 
        /// </summary>
        /// <param name="sender">Size combo box leave or change event</param>
        /// <param name="e">On change event argument</param>
        private void cmbBoxSize_Change(object sender, EventArgs e)
        {
            string sizeName = cmbBoxSize.Text.ToString();
            string sizeDescription = String.Empty;
            if (txtGloveType.Text != String.Empty && txtGloveDescription.Text != String.Empty && cmbBoxSize.DataSource != null)
            {
                if (sizeName != String.Empty)
                {
                    try
                    {
                        sizeDescription = CommonBLL.GetSizeDescription(sizeName);
                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "cmbBoxSize_Change", cmbBoxSize.Text, txtGloveDescription.Text);
                        return;
                    }
                    if (sizeDescription != Constants.INVALID_MESSAGE)
                    {
                            txtSizeSelected.Text = sizeDescription;
                    }
                    else
                    {
                        cmbBoxSize.Text = String.Empty;
                        txtSizeSelected.Text = String.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Get Ten Pcs Weight and Batch Weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// Focus to Ten Pcs Weight 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBoxSize_Validated(object sender, EventArgs e)
        {
            if (txtSizeSelected.Text != String.Empty)
            {
                if (txtTenPcsWeight.Text == String.Empty)
                    txtTenPcsWeight.Focus();
            }
        }

        /// <summary>
        /// To Save and Print batch Card details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string message = string.Empty;
                int authorizedFor = Constants.ZERO;
                if (ValidateRequiredFields())
                {
                    if (txtSizeSelected.Text == String.Empty && txtTenPcsWeight.Text == String.Empty && txtBatchWeight.Text == String.Empty)
                    {
                        cmbBoxSize_Change(this, new EventArgs());
                        cmbBoxSize_Validated(this, new EventArgs());
                        btnPrint_Click(this, new EventArgs());
                    }
                    else
                    {
                        // Recheck functionality
                        if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                        {
                            GetTenPcsWeight();
                            //Added by Tan Wei Wah 28/02/2019 for Negative input validation
                            decimal decim1;
                            if (Decimal.TryParse(txtTenPcsWeight.Text, out decim1) && decim1 > 0)
                            {
                                txtTenPcsWeight.Text = Convert.ToDouble(txtTenPcsWeight.Text).ToString("#,##0.00");
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
                            if (_chckBatchWeight)
                            {
                                message = string.Format(Constants.BATCHWEIGHT_MESSAGE, txtBatchWeight.Text);
                                authorizedFor = Constants.TWO;
                            }
                            if (message != String.Empty)
                            {
                                Enabled = false;
                                if (_moduleName == Constants.QC_SCANNING_SYSTEM)
                                {
                                    _screenName = "QC Scanning - Print Watertight Batch Cards";
                                }
                                else
                                {
                                    _screenName = "Tumbling - PrintWaterTightBatchCard";
                                }
                                Login passwordForm = new Login(Constants.Modules.TUMBLING, _screenName, string.Empty, true, message, _chckBatchWeight);
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
                                SaveBatchCard(String.Empty, Constants.ZERO);
                                ClearForm(String.Empty);
                                txtOperatorId.Focus();
                            }
                        }
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnPrint_Click", null);
                return;
            }
            catch (Exception ex)
            {
                ExceptionLogging(new FloorSystemException(ex.Message, _screenName, ex, true), _screenName, _className, "btnPrint_Click", null);
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
        private void txtGloveType_KeyPress(object sender, KeyPressEventArgs e)
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
        /// <summary>
        /// Get Size by GloveType
        /// </summary>
        /// <param name="gloveType"></param>
        private void GetSizeByGloveType()
        {
            try
            {
                // Bind Size Data
                cmbBoxSize.DataSource = CommonBLL.GetSizeByGloveTypeTumbling(txtGloveType.Text);
                cmbBoxSize.DisplayMember = "DisplayField";
                cmbBoxSize.SelectedIndex = -1;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getSizeByGloveType", txtGloveType.Text);
            }
        }

        /// <summary>
        /// Get Glove Description if valid and populate Size dropdown if any for the glovetype
        /// </summary>
        private void GetGloveDescriptionIfValid()
        {
            string gloveDescription = String.Empty;
            string gloveType = String.Empty;
            try
            {
                if (txtGloveType.Text != String.Empty)
                {
                    gloveDescription = Convert.ToString(TumblingBLL.GetGloveDescription(txtGloveType.Text.Trim()));
                    gloveType = Convert.ToString(TumblingBLL.GetGloveType(txtGloveType.Text.Trim()));
                    if (gloveDescription != String.Empty && gloveDescription != null)
                    {

                        txtGloveDescription.Text = gloveDescription;
                        txtItemId.Text = gloveType;
                        if (cmbBoxSize.DataSource == null || _previousGloveType != txtGloveType.Text)
                        {
                            GetSizeByGloveType();
                            txtSizeSelected.Text = String.Empty;
                            txtTenPcsWeight.Text = String.Empty;
                            lblPcsWeight.Visible = false;
                            txtBatchWeight.Text = String.Empty;
                        }

                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALID_GLOVE_TYPE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearForm(Constants.EXCLUDE_OPERATOR);
                    }
                    _previousGloveType = txtGloveType.Text;
                }
                else if (_previousGloveType != String.Empty)
                {
                    ClearForm(Constants.EXCLUDE_OPERATOR);
                    _previousGloveType = String.Empty;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getGloveDescriptionIfValid", cmbBoxSize.Text, txtGloveType.Text);
                return;
            }
        }

        /// <summary>
        /// Save Batch card details if valid, 
        /// the SP called in BLL also returns the serial number and batch number useful for printing
        /// </summary>
        /// <param name="authorizedBy"></param>
        /// <returns></returns>
        private void SaveBatchCard(string authorizedBy, int authorizedFor)
        {
            DateTime batchDateTime = ServerCurrentDateTime;
            BatchDTO objBatchDTO = new BatchDTO();
            objBatchDTO.Shift = Constants.ZERO;
            objBatchDTO.Line = String.Empty;
            objBatchDTO.Size = cmbBoxSize.Text;
            objBatchDTO.GloveType = txtItemId.Text;
            objBatchDTO.BatchWeight = Convert.ToDecimal(txtBatchWeight.Text); 
            objBatchDTO.TenPcsWeight = Convert.ToDecimal(txtTenPcsWeight.Text);
            objBatchDTO.BatchCarddate = batchDateTime;
            objBatchDTO.IsOnline = Constants.OFFLINE_PRODUCTION;
            objBatchDTO.OperatorId = Convert.ToString(txtOperatorId.Text);
            objBatchDTO.WorkstationNumber = WorkStationDTO.GetInstance().WorkStationNumber;
            objBatchDTO.WorkStationId = WorkStationDTO.GetInstance().WorkStationId;
            objBatchDTO.BatchType = ((DropdownDTO)(cmbBoxType.SelectedValue)).IDField.ToString();
            objBatchDTO.LocationId = WorkStationDTO.GetInstance().LocationId;
            objBatchDTO.Module = WorkStationDTO.GetInstance().Module;
            objBatchDTO.SubModule = WorkStationDTO.GetInstance().SubModule;
            objBatchDTO.AuthorizedBy = authorizedBy;
            objBatchDTO.BatchLostArea = String.Empty;
            objBatchDTO.Site = FloorSystemConfiguration.GetInstance().intSiteNumber;
            objBatchDTO.ShiftName = String.Empty;
            objBatchDTO.GloveTypeDescription = txtItemId.Text.ToString();
            objBatchDTO.AuthorizedFor = authorizedFor;
            try
            {
                _resultDTO = TumblingBLL.SaveBatchCard(objBatchDTO);
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
                if (!String.IsNullOrEmpty(txtGloveDescription.Text))
                {
                    string _GloveCategory;
                    _GloveCategory = TumblingBLL.GetRejectGloveCategory(txtGloveType.Text);
                    objBatchDTO.GloveTypeDescription = txtItemId.Text.ToString() + Environment.NewLine + Constants.TAB + _GloveCategory;
                }
                CommonBLL.PrintDetails(batchDateTime.ToString("HH:mm:ss"), _resultDTO.SerialNumber, _resultDTO.BatchNumber, objBatchDTO.BatchWeight.ToString(), objBatchDTO.Size, objBatchDTO.TenPcsWeight.ToString(), lblPcsWeight.Visible, objBatchDTO.GloveTypeDescription, String.Empty, String.Empty, false, _chckBatchWeight);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "SaveBatchCard", cmbBoxSize.Text, txtGloveDescription.Text, txtTenPcsWeight.Text, txtBatchWeight.Text);
                return;
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
                txtTenPcsWeight.Text = CommonBLL.GetTenPcsWeight().ToString("#,##0.00");
                lblPcsWeight.Visible = false;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getTenPcsWeight", null);
                return;
            }
            lblPcsWeight.Visible = false;
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
            if (Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().LWeight) && (Convert.ToDouble(txtBatchWeight.Text) > Convert.ToDouble(FloorSystemConfiguration.GetInstance().WTMaxBatchWeight)))
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
                cmbBoxType.SelectedIndex = -1;
            }
            txtGloveType.Text = String.Empty;
            txtGloveDescription.Text = String.Empty;
            txtItemId.Text = string.Empty;
            cmbBoxSize.DataSource = null;
            txtSizeSelected.Text = String.Empty;
            txtTenPcsWeight.Text = String.Empty;
            txtBatchWeight.Text = String.Empty;
            lblPcsWeight.Visible = false;
            lblBatchWeight.Visible = false;
            _previousGloveType = String.Empty;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ClearForm", null);
            }
            GetType();
        }

        /// <summary>
        /// Validate required fields
        /// </summary>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtOperatorId, Constants.OPERATORID, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbBoxType, Constants.TYPE, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtGloveType, Constants.GLOVETYPE, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbBoxSize, Constants.SIZE, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtTenPcsWeight, Constants.TENPCS, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtBatchWeight, Constants.BATCH, ValidationType.Required));
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
            else if(floorException.subSystem == Constants.AXSERVICEERROR)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.AXPOSTINGERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                if (uiControl == "getTenPcsWeight")
                    GlobalMessageBox.Show("(" + result + ") - " + Constants.TENPCS_WEIGHT_ERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                else if (uiControl == "getBatchWeight")
                    GlobalMessageBox.Show("(" + result + ") - " + Constants.BATCH_WEIGHT_ERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm(String.Empty);
            txtOperatorId.Focus();
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
                //added by adamas 11/10/2017 for input validation
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