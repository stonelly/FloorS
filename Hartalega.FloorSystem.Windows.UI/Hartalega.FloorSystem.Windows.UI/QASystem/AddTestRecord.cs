using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.Configuration;
using System.Data;

namespace Hartalega.FloorSystem.Windows.UI.QASystem
{
    /// <summary>
    /// Module: QA System
    /// Screen Name: Add Test Record
    /// File Type: Code file
    /// </summary>
    public partial class AddTestResult : FormBase
    {
        #region Member Variables
        private string _selectedTab = string.Empty;
        private string _screenName = "QA System - AddTestResult";
        private string _className = "AddTestResult";
        private StringBuilder _requiredFieldMessage = new StringBuilder(string.Empty);
        private string _operatorId;
        private int _productType;
        private string _wtFilterPaperFirst;
        private string _wtAfterFilteration;
        private bool _isDuplicate = false;
        private string _serialNumber;
        #endregion
        /// <summary>
        /// Constructor initialising values retrieved from main form
        /// </summary>
        /// <param name="resultTab"></param>
        /// <returns></returns>
        public AddTestResult(string resultTab)
        {
            InitializeComponent();
            if (string.Compare(resultTab,Constants.QA_PROTEIN)==Constants.ZERO)
            {
                TabControl.SelectedTab = tpProtein;
                tpPowder.Enabled = false;
                tpHotBox.Enabled = false;
                _selectedTab = Constants.QA_PROTEIN;
            }
            else if (string.Compare(resultTab, Constants.QA_POWDER) == Constants.ZERO)
            {
                TabControl.SelectedTab = tpPowder;
                tpProtein.Enabled = false;
                tpHotBox.Enabled = false;
                _selectedTab = Constants.QA_POWDER;
            }
            else
            {
                TabControl.SelectedTab = tpHotBox;
                tpProtein.Enabled = false;
                tpPowder.Enabled = false;
                _selectedTab = Constants.QA_HOTBOX;
            }
            try
            {
                txtDate.Text = CommonBLL.GetCurrentDateAndTimeFromServer().ToString(ConfigurationManager.AppSettings["SetupConfigDateFormat"]);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddTestResult_Load", txtOperatorId.Text);
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
            txtReference.Text = string.Empty;
            txtOperatorId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtRemark.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtProteinContent.Text = string.Empty;
            txtFilterPaper1.Text = Constants.DEFAULT_WEIGHT;
            txtFilterPaper2.Text = Constants.DEFAULT_WEIGHT;
            txtResiduePowder.Text = Constants.DEFAULT_WEIGHT;
            txtFilterandResidue.Text = Constants.DEFAULT_WEIGHT;
            txtAfterFilteration.Text = Constants.DEFAULT_WEIGHT;
            btnSave.Enabled = false;
            rbFail.Checked = false;
            rbPass.Checked = false;
            _wtAfterFilteration = Constants.DEFAULT_WEIGHT;
            _wtFilterPaperFirst = Constants.DEFAULT_WEIGHT;
            _isDuplicate = false;
            txtReference.Focus();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ClearForm", null);
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

            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        /// <summary>
        /// Validate required fields
        /// </summary>
        private void ValidateRequiredFields()
        {
            _requiredFieldMessage = new StringBuilder(string.Empty);
            if (string.IsNullOrEmpty(txtOperatorId.Text.Trim()))
            {
                txtName.Text = string.Empty;
            }
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtReference, "Reference", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtOperatorId, "Operator Id", ValidationType.Required));
            _requiredFieldMessage.Append(GetValidateFormMessage());

            if (string.Compare(_selectedTab,Constants.QA_HOTBOX)==Constants.ZERO)
            {
                if (!rbFail.Checked && !rbPass.Checked)
                    _requiredFieldMessage.AppendLine("Result");
            }
        }

       
        /// <summary>
        /// Validate weights inputted by the user
        /// </summary>
        private bool ValidateWeights()
        {
            bool status = false;
            decimal weight;
            StringBuilder weightValidation = new StringBuilder();
            weightValidation.AppendLine(Messages.INVALID_WEIGHT);


            if (string.Compare(_selectedTab, Constants.QA_PROTEIN) == Constants.ZERO)
            {
                if (!string.IsNullOrEmpty(txtWeight.Text.Trim()))
                {
                    if (!Decimal.TryParse(txtWeight.Text.Trim(), out weight))
                    {
                        weightValidation.AppendLine(Constants.WEIGHT);
                        txtWeight.Focus();
                        txtWeight.Text = string.Empty;
                    }
                    else if (Decimal.Parse(txtWeight.Text.Trim()) == Constants.ZERO)
                    {
                        _requiredFieldMessage.AppendLine(Constants.WEIGHT);
                        txtWeight.Text = string.Empty;
                    }
                }
                else
                {
                    _requiredFieldMessage.AppendLine(Constants.WEIGHT);
                    txtWeight.Text = string.Empty;
                }
                if (!string.IsNullOrEmpty(txtProteinContent.Text.Trim()))
                {
                    if (!Decimal.TryParse(txtProteinContent.Text.Trim(), out weight))
                    {
                        weightValidation.AppendLine(Constants.PROTEIN_CONTENT);
                        txtProteinContent.Focus();
                        txtProteinContent.Text = string.Empty;
                    }
                    else if (Decimal.Parse(txtProteinContent.Text.Trim()) == Constants.ZERO)
                    {
                        _requiredFieldMessage.AppendLine(Constants.PROTEIN_CONTENT);
                        txtProteinContent.Text = string.Empty;
                    }
                }
                else
                {
                    _requiredFieldMessage.AppendLine(Constants.PROTEIN_CONTENT);
                    txtProteinContent.Text = string.Empty;
                }
            }
            else if (_selectedTab == Constants.QA_POWDER)
            {
                if (!string.IsNullOrEmpty(txtSerialNo.Text))
                {
                    _productType = QASystemBLL.GetProductType(Convert.ToInt64(txtSerialNo.Text));

                    if (_productType == Constants.TWO)
                    {
                        if (!string.IsNullOrEmpty(txtFilterPaper1.Text.Trim()))
                        {
                            if (!Decimal.TryParse(txtFilterPaper1.Text.Trim(), out weight))
                            {
                                weightValidation.AppendLine(Constants.WEIGHT_FILTER_PAPER);
                                txtFilterPaper1.Text = Constants.DEFAULT_WEIGHT;
                            }
                            else if(Decimal.Parse(txtFilterPaper1.Text.Trim()) == Constants.ZERO)
                            {
                                _requiredFieldMessage.AppendLine(Constants.WEIGHT_FILTER_PAPER);
                                txtFilterPaper1.Text = Constants.DEFAULT_WEIGHT;
                            }
                        }
                        else
                        {
                            _requiredFieldMessage.AppendLine(Constants.WEIGHT_FILTER_PAPER);
                            txtFilterPaper1.Text = Constants.DEFAULT_WEIGHT;
                        }
                        if (!string.IsNullOrEmpty(txtAfterFilteration.Text.Trim()))
                        {
                            if (!Decimal.TryParse(txtAfterFilteration.Text.Trim(), out weight))
                            {
                                weightValidation.AppendLine(Constants.WEIGHT_FILTER_PAPER_AFTER_FILTERATION);
                                txtAfterFilteration.Text = Constants.DEFAULT_WEIGHT;
                            }
                            else if (Decimal.Parse(txtAfterFilteration.Text.Trim()) == Constants.ZERO)
                            {
                                _requiredFieldMessage.AppendLine(Constants.WEIGHT_FILTER_PAPER_AFTER_FILTERATION);
                                txtAfterFilteration.Text = Constants.DEFAULT_WEIGHT;
                            }
                        }
                        else
                        {
                            _requiredFieldMessage.AppendLine(Constants.WEIGHT_FILTER_PAPER_AFTER_FILTERATION);
                            txtAfterFilteration.Text = Constants.DEFAULT_WEIGHT;
                        }
                    }
                    if (!string.IsNullOrEmpty(txtFilterPaper2.Text.Trim()))
                    {
                        if (!Decimal.TryParse(txtFilterPaper2.Text.Trim(), out weight))
                        {
                            weightValidation.AppendLine(Constants.WEIGHT_FILTER_PAPER);
                            txtFilterPaper2.Text = Constants.DEFAULT_WEIGHT;
                        }
                        else if (Decimal.Parse(txtFilterPaper2.Text.Trim()) == Constants.ZERO)
                        {
                            _requiredFieldMessage.AppendLine(Constants.WEIGHT_FILTER_PAPER);
                            txtFilterPaper2.Text = Constants.DEFAULT_WEIGHT;
                        }
                    }
                    else
                    {
                        _requiredFieldMessage.AppendLine(Constants.WEIGHT_FILTER_PAPER);
                        txtFilterPaper2.Text = Constants.DEFAULT_WEIGHT;
                    }
                    if (!string.IsNullOrEmpty(txtFilterandResidue.Text.Trim()))
                    {
                        if (!Decimal.TryParse(txtFilterandResidue.Text.Trim(), out weight))
                        {
                            weightValidation.AppendLine(Constants.WEIGHT_FILTER_PAPER_AND_RESIDUE);
                            txtFilterandResidue.Text = Constants.DEFAULT_WEIGHT;
                        }
                        else if (Decimal.Parse(txtFilterandResidue.Text.Trim()) == Constants.ZERO)
                        {
                            _requiredFieldMessage.AppendLine(Constants.WEIGHT_FILTER_PAPER_AND_RESIDUE);
                            txtFilterandResidue.Text = Constants.DEFAULT_WEIGHT;
                        }
                    }
                    else
                    {
                        _requiredFieldMessage.AppendLine(Constants.WEIGHT_FILTER_PAPER_AND_RESIDUE);
                        txtFilterandResidue.Text = Constants.DEFAULT_WEIGHT;
                    }
                }
            }
            if (string.Compare(weightValidation.ToString().Trim(),Messages.INVALID_WEIGHT)==Constants.ZERO)
            {
                status = true;
            }
            else
            {
                GlobalMessageBox.Show(weightValidation.ToString(), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                status = false;
            }
            return status;
        }

        private void GetRecentResultDetails(Int64 referenceNumber)
        {
            DataTable resultCaptured;
            _isDuplicate = true;
            if (string.Compare(_selectedTab, Constants.QA_PROTEIN) == Constants.ZERO)
            {
                resultCaptured = QASystemBLL.GetQAResultDetails(_selectedTab, referenceNumber);
                txtSerialNo.Text = resultCaptured.Rows[0]["SerialNumber"].ToString();
                txtOperatorId.Text = resultCaptured.Rows[0]["OperatorId"].ToString();
                txtName.Text = resultCaptured.Rows[0]["Name"].ToString();
                txtRemark.Text = resultCaptured.Rows[0]["Remark"].ToString();
                txtProteinContent.Text = string.Format(Constants.DECIMAL_FORMAT,resultCaptured.Rows[0]["ProteinContent"]);
                txtWeight.Text =string.Format(Constants.DECIMAL_FORMAT, resultCaptured.Rows[0]["Weight"]);
            }
            else if (string.Compare(_selectedTab, Constants.QA_POWDER) == Constants.ZERO)
            {
                resultCaptured = QASystemBLL.GetQAResultDetails(_selectedTab, referenceNumber);
                txtSerialNo.Text = resultCaptured.Rows[0]["SerialNumber"].ToString();
                txtOperatorId.Text = resultCaptured.Rows[0]["OperatorId"].ToString();
                txtName.Text = resultCaptured.Rows[0]["Name"].ToString();
                txtRemark.Text = resultCaptured.Rows[0]["Remark"].ToString();
                txtFilterPaper1.Text = string.Format(Constants.DECIMAL_FORMAT,resultCaptured.Rows[0]["FilterPaperWeight_First"]);
                txtAfterFilteration.Text = string.Format(Constants.DECIMAL_FORMAT,resultCaptured.Rows[0]["FilterPaperWeightAfterFilteration"]);
                txtFilterPaper2.Text = string.Format(Constants.DECIMAL_FORMAT,resultCaptured.Rows[0]["FilterPaperWeight_Second"]);
                txtFilterandResidue.Text = string.Format(Constants.DECIMAL_FORMAT,resultCaptured.Rows[0]["FilterPaperWeightAndResiduePowder"]);
                txtResiduePowder.Text = string.Format(Constants.DECIMAL_FORMAT,resultCaptured.Rows[0]["ResiduePowder"]);
            }
            else
            {
                resultCaptured = QASystemBLL.GetQAResultDetails(_selectedTab, referenceNumber);
                txtSerialNo.Text = resultCaptured.Rows[0]["SerialNumber"].ToString();
                txtOperatorId.Text = resultCaptured.Rows[0]["OperatorId"].ToString();
                txtName.Text = resultCaptured.Rows[0]["Name"].ToString();
                txtRemark.Text = resultCaptured.Rows[0]["Remark"].ToString();
                if (Convert.ToBoolean(resultCaptured.Rows[0]["IsResultPass"]))
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;
            }
            _serialNumber = txtSerialNo.Text;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for Tab control selecting event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = !e.TabPage.Enabled;
        }

        /// <summary>
        /// Event Handler for Serial No textbox leave event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            int status;
            string qaiStatus = string.Empty;

            if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                decimal serialNumber = Decimal.Parse(txtSerialNo.Text.Trim());
                try
                {
                    status = QASystemBLL.CheckSerialNoStatus(serialNumber, _selectedTab);

                    if (status <= 0)
                    {
                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_QA, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearForm();
                    }
                    else
                    {
                        qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(serialNumber);
                        if (!string.IsNullOrEmpty(qaiStatus))
                        {
                            GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            txtReference.Focus();
                        }
                        else if(string.IsNullOrEmpty(txtReference.Text) || string.Compare(_serialNumber,txtSerialNo.Text.Trim())!=Constants.ZERO)
                        {
                            txtRemark.Text = string.Empty;
                            txtWeight.Text = string.Empty;
                            txtProteinContent.Text = string.Empty;
                            txtFilterPaper1.Text = Constants.DEFAULT_WEIGHT;
                            txtFilterPaper2.Text = Constants.DEFAULT_WEIGHT;
                            txtResiduePowder.Text = Constants.DEFAULT_WEIGHT;
                            txtFilterandResidue.Text = Constants.DEFAULT_WEIGHT;
                            txtAfterFilteration.Text = Constants.DEFAULT_WEIGHT;
                            rbFail.Checked = false;
                            rbPass.Checked = false;
                            txtReference.Text = Convert.ToString(QASystemBLL.GetReferenceForSerialNumber(serialNumber, _selectedTab)).PadLeft(10, '0');
                            string referenceStatus = QASystemBLL.CheckReferenceStatus(Int64.Parse(txtReference.Text.Trim()), _selectedTab);
                            if (string.Compare(referenceStatus,Constants.DUPLICATE_MESSAGE)==Constants.ZERO)
                            {
                                if (GlobalMessageBox.Show(Messages.REFERENCE_ALREADY_EXIST, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                                {
                                    GetRecentResultDetails(Int64.Parse(txtReference.Text.Trim()));
                                }
                                else
                                {
                                    ClearForm();
                                }
                            }
                        }

                        btnSave.Enabled = true;
                        _serialNumber = txtSerialNo.Text;
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
        /// Event Handler for Reference textbox leave event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReference_Leave(object sender, EventArgs e)
        {
            string status;

            if (!string.IsNullOrEmpty(txtReference.Text.Trim()))
            {
                Int64 referenceNumber = Int64.Parse(txtReference.Text.Trim());
                try
                {
                    status = QASystemBLL.CheckReferenceStatus(referenceNumber, _selectedTab);

                    if (string.Compare(status,Constants.INVALIDMESSAGE)==Constants.ZERO)
                    {
                        GlobalMessageBox.Show(Messages.INVALID_REFERENCE_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearForm();
                        txtReference.Focus();
                    }
                    else if (string.Compare(status, Constants.DUPLICATE_MESSAGE) == Constants.ZERO)
                    {
                        if (GlobalMessageBox.Show(Messages.REFERENCE_ALREADY_EXIST, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                        {
                            GetRecentResultDetails(referenceNumber);
                        }
                        else
                        {
                            ClearForm();
                        }
                    }
                    else
                    {
                        txtRemark.Text = string.Empty;
                        txtWeight.Text = string.Empty;
                        txtProteinContent.Text = string.Empty;
                        txtFilterPaper1.Text = Constants.DEFAULT_WEIGHT;
                        txtFilterPaper2.Text = Constants.DEFAULT_WEIGHT;
                        txtResiduePowder.Text = Constants.DEFAULT_WEIGHT;
                        txtFilterandResidue.Text = Constants.DEFAULT_WEIGHT;
                        txtAfterFilteration.Text = Constants.DEFAULT_WEIGHT;
                        rbFail.Checked = false;
                        rbPass.Checked = false;
                        txtSerialNo.Text = Convert.ToString(QASystemBLL.GetSerialNumberForReference(referenceNumber, _selectedTab));
                        btnSave.Enabled = true;
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "txtReference_Leave", null);
                    return;
                }
            }
        }

        /// <summary>
        /// Event Handler for Operator id textbox leave event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperatorId_Leave(object sender, System.EventArgs e)
        {
            string operatorName = String.Empty;
            _operatorId = txtOperatorId.Text.Trim();
            if (!string.IsNullOrEmpty(txtOperatorId.Text.Trim()))
            {
                try
                {
                    if (SecurityModuleBLL.ValidateOperatorAccess(txtOperatorId.Text.Trim(), _screenName))
                    {
                        operatorName = CommonBLL.GetOperatorNameQAI(txtOperatorId.Text.Trim());

                        if (!string.IsNullOrEmpty(operatorName))
                        {
                            txtName.Text = operatorName;
                        }
                        else
                        {
                            txtOperatorId.Text = String.Empty;
                            txtName.Text = string.Empty;
                            GlobalMessageBox.Show(Messages.INVALID_DATA_SUMMARY + "Operator Id", Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtOperatorId.Focus();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.OPERATOR_SCREEN_ACCESS_MSG, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        txtOperatorId.Text = String.Empty;
                        txtName.Text = String.Empty;
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
                txtName.Text = String.Empty;
            }
        }

        /// <summary>
        /// Event Handler for Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTestResult_Load(object sender, System.EventArgs e)
        {                   
            txtSerialNo.SerialNo();
            txtOperatorId.OperatorId();
            txtReference.Reference();
            txtWeight.Weight();
            txtProteinContent.Weight();
            txtFilterPaper1.Weight();
            txtFilterPaper2.Weight();
            txtFilterandResidue.Weight();
            txtAfterFilteration.Weight();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddTestResult_Load", null);
                return;
            }
        }

        /// <summary>
        /// Event Handler for Weight of filter ppr + Residue powder textbox leave event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFilterandResidue_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSerialNo.Text))
                {
                    ValidateRequiredFields();

                    if (ValidateWeights())
                    {
                        if (string.Compare(_requiredFieldMessage.ToString().Trim(),Messages.REQUIREDFIELDMESSAGE.Trim()) == Constants.ZERO)
                        {
                            _wtFilterPaperFirst = txtFilterPaper1.Text.Trim();
                            _wtAfterFilteration = txtAfterFilteration.Text.Trim();

                            if (_productType == Constants.ONE)
                            {
                                if (string.IsNullOrEmpty(txtFilterPaper1.Text.Trim()))
                                    _wtFilterPaperFirst = Constants.DEFAULT_WEIGHT;
                                if (string.IsNullOrEmpty(txtAfterFilteration.Text.Trim()))
                                    _wtAfterFilteration = Constants.DEFAULT_WEIGHT;
                            }

                            txtResiduePowder.Text = string.Format(Constants.DECIMAL_FORMAT, QASystemBLL.GetResiduePowder(Convert.ToInt64(txtSerialNo.Text.Trim()),
                            Convert.ToDecimal(_wtFilterPaperFirst),
                            Convert.ToDecimal(txtFilterPaper2.Text.Trim()),
                            Convert.ToDecimal(_wtAfterFilteration),
                            Convert.ToDecimal(txtFilterandResidue.Text.Trim())));
                        }
                        else
                        {
                            GlobalMessageBox.Show(_requiredFieldMessage.ToString(), Constants.AlertType.Information, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                        }
                    } 
                }
                else
                {
                    if (!txtReference.Focused)
                    {
                        validationMesssageLst = new List<ValidationMessage>();
                        validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No", ValidationType.Required));
                        ValidateForm();
                    }
                }

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "txtFilterandResidue_Leave", txtOperatorId.Text);
                return;
            }
        }
        /// <summary>
        /// Event Handler for Save button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            int rowsReturned = 0;
            string statusRef;
            int statusSer;
            string qaiStatus = string.Empty;
           
            ValidateRequiredFields();
            
                if (ValidateWeights())
                {
                    if (string.Compare(_requiredFieldMessage.ToString().Trim(), Messages.REQUIREDFIELDMESSAGE.Trim()) == Constants.ZERO)
                    {                        
                            Int64 referenceNumber = Int64.Parse(txtReference.Text.Trim());
                            Int64 serialNumber = Int64.Parse(txtSerialNo.Text.Trim());
                            _wtFilterPaperFirst = txtFilterPaper1.Text.Trim();
                            _wtAfterFilteration = txtAfterFilteration.Text.Trim();

                            if (_productType == Constants.ONE)
                            {
                                if (string.IsNullOrEmpty(txtFilterPaper1.Text.Trim()))
                                    _wtFilterPaperFirst = Constants.DEFAULT_WEIGHT;
                                if (string.IsNullOrEmpty(txtAfterFilteration.Text.Trim()))
                                    _wtAfterFilteration = Constants.DEFAULT_WEIGHT;
                            }

                            try
                            {
                                statusRef = QASystemBLL.CheckReferenceStatus(referenceNumber, _selectedTab);
                                statusSer = QASystemBLL.CheckSerialNoStatus(serialNumber, _selectedTab);

                                if(string.Compare(_operatorId,txtOperatorId.Text.Trim()) !=Constants.ZERO)
                                {
                                    string operatorName = String.Empty;
                                    if (string.IsNullOrEmpty(txtOperatorId.Text.Trim()))
                                    {
                                        txtName.Text = string.Empty;
                                        validationMesssageLst = new List<ValidationMessage>();
                                        if (!btnSave.Focused)
                                        {
                                            validationMesssageLst.Add(new ValidationMessage(txtOperatorId, "Operator Id", ValidationType.Required));
                                        }
                                        ValidateForm();
                                        return;
                                    }
                                    else
                                    {
                                        operatorName = CommonBLL.GetOperatorNameQAI(txtOperatorId.Text.Trim());

                                        if (!string.IsNullOrEmpty(operatorName))
                                        {
                                            txtName.Text = operatorName;
                                        }
                                        else
                                        {
                                            txtOperatorId.Text = String.Empty;
                                            txtName.Text = string.Empty;
                                            GlobalMessageBox.Show(Messages.INVALID_DATA_SUMMARY + "Operator Id", Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                            txtOperatorId.Focus();
                                            return;
                                        }
                                    }
                                }

                                if (string.Compare(statusRef, Constants.INVALIDMESSAGE, true) == Constants.ZERO)
                                {
                                    GlobalMessageBox.Show(Messages.INVALID_REFERENCE_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                    txtReference.Focus();
                                }
                                else if (statusSer <= Constants.ZERO)
                                {
                                    GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_QA, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                }
                                else
                                {
                                    qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(serialNumber);
                                    if (!string.IsNullOrEmpty(qaiStatus))
                                    {
                                        GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        ClearForm();
                                        txtReference.Focus();
                                    }
                                    else
                                    {
                                        if (string.Compare(txtSerialNo.Text, _serialNumber) != Constants.ZERO)
                                        {
                                            txtReference.Text = Convert.ToString(QASystemBLL.GetReferenceForSerialNumber(serialNumber, _selectedTab)).PadLeft(10, '0');

                                            if (string.Compare(statusRef, Constants.DUPLICATE_MESSAGE, true) == Constants.ZERO)
                                            {
                                                if (GlobalMessageBox.Show(Messages.REFERENCE_ALREADY_EXIST, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                                                {
                                                    GetRecentResultDetails(Int64.Parse(txtReference.Text.Trim()));
                                                }
                                                else
                                                {
                                                    ClearForm();
                                                    return;
                                                }
                                            }
                                        }
                                        txtSerialNo.Text = Convert.ToString(QASystemBLL.GetSerialNumberForReference(Int64.Parse(txtReference.Text.Trim()), _selectedTab));
                                        QASystemDTO objQASystem = new QASystemDTO();

                                        objQASystem.Reference = Convert.ToInt64(txtReference.Text.Trim());
                                        objQASystem.SerialNumber = Convert.ToInt64(txtSerialNo.Text.Trim());

                                        objQASystem.TestDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();

                                        objQASystem.OperatorId = txtOperatorId.Text.Trim();
                                        objQASystem.Remark = txtRemark.Text.Trim();
                                        objQASystem.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                                        if (string.Compare(_selectedTab,Constants.QA_PROTEIN)==Constants.ZERO)
                                        {
                                            objQASystem.Weight = Convert.ToDecimal(txtWeight.Text.Trim());
                                            objQASystem.ProteinContent = Convert.ToDecimal(txtProteinContent.Text.Trim());
                                            objQASystem.TestTab = Constants.QA_PROTEIN;
                                        }
                                        if (string.Compare(_selectedTab, Constants.QA_POWDER) == Constants.ZERO)
                                        {
                                            objQASystem.FilterPaperWtFirst = Convert.ToDecimal(_wtFilterPaperFirst);
                                            objQASystem.FilterPaperWtAfterFilteration = Convert.ToDecimal(_wtAfterFilteration);                                            
                                            objQASystem.FilterPaperWtSecond = Convert.ToDecimal(txtFilterPaper2.Text.Trim());
                                            objQASystem.FilterPaperWtAndResiduePowder = Convert.ToDecimal(txtFilterandResidue.Text.Trim());                                          
                                            objQASystem.ResiduePowder = QASystemBLL.GetResiduePowder(Convert.ToInt64(txtSerialNo.Text.Trim()),
                                                                                           objQASystem.FilterPaperWtFirst,
                                                                                           objQASystem.FilterPaperWtSecond,
                                                                                           objQASystem.FilterPaperWtAfterFilteration,
                                                                                           objQASystem.FilterPaperWtAndResiduePowder);
                                            objQASystem.TestTab = Constants.QA_POWDER;
                                        }
                                        if (string.Compare(_selectedTab, Constants.QA_HOTBOX) == Constants.ZERO)
                                        {
                                            objQASystem.IsResultPass = rbPass.Checked;
                                            objQASystem.TestTab = Constants.QA_HOTBOX;
                                        }

                                        rowsReturned = QASystemBLL.SaveQATestResult(objQASystem, _isDuplicate);

                                        if (rowsReturned > 0)
                                        {
                                            //adamas event log 20190228
                                            EventLogDTO EventLog = new EventLogDTO();
                                            EventLog.CreatedBy = txtOperatorId.Text.Trim();
                                            Constants.EventLog evtAction = Constants.EventLog.Save;
                                            EventLog.EventType = Convert.ToInt32(evtAction);
                                            EventLog.EventLogType = Constants.eventlogtype;

                                            var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                                            CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());
                                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                            ClearForm();

                                        }
                                        else
                                        {
                                            GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                        }
                                    }
                                }
                            }
                            catch (FloorSystemException ex)
                            {
                                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                                return;
                            }
                        }
                    
                    else
                    {
                        GlobalMessageBox.Show(_requiredFieldMessage.ToString(), Constants.AlertType.Information, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                    }
                }            
        }

        /// <summary>
        /// Event Handler for Cancel button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CONFIRM_CANCEL_TRANSACTION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
            }
        }

        /// <summary>
        /// Event Handler to grey out the tabs which are currently not in use
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisableTab_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            TabPage page = tabControl.TabPages[e.Index];
            if (!page.Enabled)
            {
                //Draws disabled tab
                using (SolidBrush brush = new SolidBrush(SystemColors.GrayText))
                {
                    e.Graphics.DrawString(page.Text, page.Font, brush, e.Bounds.X + 3, e.Bounds.Y + 3);
                }
            }
            else
            {
                // Draws normal tab
                using (SolidBrush brush = new SolidBrush(page.ForeColor))
                {
                    e.Graphics.DrawString(page.Text, page.Font, brush, e.Bounds.X + 3, e.Bounds.Y + 3);
                }
            }
        }
        #endregion        
    }
}
