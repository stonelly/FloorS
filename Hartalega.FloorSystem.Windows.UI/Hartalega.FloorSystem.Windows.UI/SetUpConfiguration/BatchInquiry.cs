using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Batch Inquiry
    /// File Type: Code file
    /// </summary> 
    public partial class BatchInquiry : FormBase
    {
        #region Member Variables
        private List<string> _validationMsgs = new List<string>();
        private string _screenName = "Configuration SetUp - Batch Inquiry";
        private string _className = "BatchInquiry";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor initialising the components
        /// </summary>       
        /// <returns></returns>
        public BatchInquiry()
        {
            this.InitializeComponent();
            try
            {
                BindBatchType();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BatchInquiry_Load", null);
                return;
            }
        }
        #endregion


        #region User Methods
        /// <summary>
        /// Clearing mouse paste cheracters in Serial Number
        /// </summary>
        /// <param name="txt"></param>
        public void ValidateFields(TextBox txt)
        {
            bool _isValid = Validator.IsValidInput(Constants.ValidationType.Integer, txt.Text);
            if (!_isValid)
            {
                txt.Clear();
            }
        }
        /// <summary>
        /// Get Batch Details
        /// </summary>
        private void GetBatchDetails()
        {
            bool valid = UserValidation();
            dgvBatchDetails.Rows.Clear();

            if (valid)
            {
                DateTime? frmDate = null;
                DateTime? toDate = null;
                Int64? serialNo = null;
                string batchType = null;

                if (!string.IsNullOrEmpty(txtFrom.Controls[Constants.ONE].Text.Trim()))
                {
                    frmDate = DateTime.ParseExact(txtFrom.Controls[Constants.ONE].Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                }
                if (!string.IsNullOrEmpty(txtTo.Controls[Constants.ONE].Text.Trim()))
                {
                    toDate = DateTime.ParseExact(txtTo.Controls[Constants.ONE].Text.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None);
                }
                if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
                {
                    serialNo = Int64.Parse(txtSerialNo.Text.Trim());
                }
                if(!string.IsNullOrEmpty(Convert.ToString(cmbBatchType.SelectedItem)))
                {
                    batchType = Convert.ToString(cmbBatchType.SelectedItem);
                }
                try
                {

                    List<BatchDetails> batchDetails = null;
                    batchDetails = SetUpConfigurationBLL.GetBatchDetails(frmDate, toDate, serialNo, batchType);

                    if (batchDetails != null)
                    {
                        dgBatchDetails.DataSource = null;
                        var source = new BindingSource(batchDetails, null);
                        dgBatchDetails.DataSource = source;
                        dgBatchDetails.Columns[Constants.ZERO].HeaderText = "Serial No";
                        dgBatchDetails.Columns[Constants.ONE].HeaderText = "Time";
                        dgBatchDetails.Columns[Constants.TWO].HeaderText = "Glove Type";
                        dgBatchDetails.Columns[Constants.THREE].HeaderText = "Size";
                        dgBatchDetails.Columns[Constants.FOUR].HeaderText = "Batch Type";
                        dgBatchDetails.Columns[Constants.FIVE].HeaderText = "Shift";
                        dgBatchDetails.Columns[Constants.SIX].HeaderText = "Line";
                        dgBatchDetails.Columns[Constants.SEVEN].HeaderText = "Batch Date";
                        dgBatchDetails.Columns[Constants.EIGHT].HeaderText = "10 Pcs(g)";
                        dgBatchDetails.Columns[Constants.NINE].HeaderText = "Batch(Kg)";
                        dgBatchDetails.Columns[Constants.TEN].HeaderText = "Total Pcs";
                        dgBatchDetails.Columns[Constants.ELEVEN].HeaderText = "Verification – QAI";
                        dgBatchDetails.Columns[Constants.TWELVE].HeaderText = "QAI Completion Date";
                        dgBatchDetails.Columns[Constants.THIRTEEN].HeaderText = "Current Location of the Batch";

                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        dgBatchDetails.DataSource = null;
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "GetBatchDetails", null);
                    return;
                }
            }
        }
        
        /// <summary>
        /// UserValidation
        /// </summary>
        private bool UserValidation()
        {
            string fromDate = txtFrom.Controls[Constants.ONE].Text.Trim();
            string toDate = txtTo.Controls[Constants.ONE].Text.Trim();
            _validationMsgs.Clear();
            //_validationMsgs = DateValidation.DateValidator(txtFrom.Controls[Constants.ONE].Text.Trim(), 
                                                           //txtTo.Controls[Constants.ONE].Text.Trim());
            _validationMsgs = DateValidation.DateValidator(fromDate, 
                                                           toDate);
            // Validation : Either Date or Serial No. or Batch Type filter criteria should be entered
            if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate)
                && string.IsNullOrEmpty(txtSerialNo.Text.Trim()) && (cmbBatchType.SelectedIndex == Constants.MINUSONE  ||
                cmbBatchType.SelectedIndex == Constants.ZERO))
            {
                _validationMsgs.Add(Messages.FILTER_REQUIRED);
            }

            if (_validationMsgs.Count > 0)
            {
                StringBuilder validationsAlert = new StringBuilder();
                foreach (string str in _validationMsgs)
                {
                    validationsAlert.Append(str);
                    validationsAlert.Append(Environment.NewLine);
                }
                GlobalMessageBox.Show(Convert.ToString(validationsAlert), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                txtFrom.Controls[Constants.ONE].Text = string.Empty;
                txtTo.Controls[Constants.ONE].Text = string.Empty;
                return false;
            }
            else
                return true;
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
        /// Clear the values of all the controls 
        /// </summary>
        private void ClearForm()
        {
            txtFrom.Controls[Constants.ONE].Text = string.Empty;
            txtTo.Controls[Constants.ONE].Text = string.Empty;
            txtSerialNo.Text = string.Empty;
            dgvBatchDetails.Rows.Clear();
            txtFrom.Controls[Constants.ONE].Focus();
        }

        /// <summary>
        /// Binds the Batch Type ComboBox
        /// </summary>
        private void BindBatchType()
        {
            DropdownDTO obj = new DropdownDTO();
            if (cmbBatchType.Items.Count == Constants.ZERO)
            {
                try
                {
                    cmbBatchType.Items.Add(string.Empty);
                    List<DropdownDTO> lstDrop = CommonBLL.GetEnumText(Constants.BATCH_TYPE);

                    for ( int i=0;i<lstDrop.Count;i++) 
                    {
                        cmbBatchType.Items.Add(lstDrop[i].DisplayField);
                    }                   
                    cmbBatchType.SelectedIndex = Constants.MINUSONE;                    
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "BindBatchType", null);
                    return;
                }
            }
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for click of button Go
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                GetBatchDetails();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnGo_Click", null);
                return;
            }
        }

        /// <summary>
        /// Event Handler for validating Serial Number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            try
            {
                // Validation for Serial Number
                if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
                {
                    int status;
                    Int64 serialNumber = Int64.Parse(txtSerialNo.Text.Trim());
                    status = SetUpConfigurationBLL.CheckSerialNoStatus(serialNumber);
                    if (status <= 0)
                    {
                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerialNo.Text = String.Empty;
                        txtSerialNo.Focus();
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "txtSerialNo_Leave", null);
                return;
            }
        }

        /// <summary>
        /// Event Handler for page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BatchInquiry_Load(object sender, EventArgs e)
        {
            this.dgvBatchDetails.AllowUserToAddRows = false;
            txtSerialNo.SerialNo();
        }
        private void txtSerialNo_TextChanged_1(object sender, EventArgs e)
        {
            ValidateFields(this.txtSerialNo);
        }
        #endregion

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
