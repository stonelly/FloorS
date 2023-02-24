using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Batch Inquiry
    /// File Type: Code file
    /// </summary> 
    public partial class LineClearanceAuthoriseSetup : FormBase
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
        public LineClearanceAuthoriseSetup()
        {
            this.InitializeComponent();
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
        private void SearchLineClearanceAuthorise()
        {
            bool valid = true;
            //bool valid = UserValidation();
            dgvLineClearanceAuthorise.Rows.Clear();

            if (valid)
            {                
                try
                {

                    List<LineClearanceAuthoriseDTO> lineClearanceAuthoriseList = null;
                    lineClearanceAuthoriseList = SetUpConfigurationBLL.GetLinceClearanceAuthoriseList(rbEmployeeID.Checked, txtSearchText.Text);

                    if (lineClearanceAuthoriseList != null)
                    {
                        dgvLineClearanceAuthorise.AutoGenerateColumns = false;
                        dgvLineClearanceAuthorise.DataSource = null;
                        var source = new BindingSource(lineClearanceAuthoriseList, null);
                        dgvLineClearanceAuthorise.DataSource = source;
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        dgvLineClearanceAuthorise.DataSource = null;
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "SearchLineClearanceAuthorise", null);
                    return;
                }
            }
        }

        private void UpdateLineClearanceAuthorise()
        {
            try
            {
                DataTable dtLineClearanceAuthorise = new DataTable();
                dtLineClearanceAuthorise.Columns.Add("EmployeeID", typeof(string));
                dtLineClearanceAuthorise.Columns.Add("IsAllow", typeof(bool));

                foreach (DataGridViewRow row in dgvLineClearanceAuthorise.Rows)
                {
                    DataRow newRow = dtLineClearanceAuthorise.NewRow();
                    newRow["EmployeeID"] = row.Cells["EmployeeID"].Value;
                    newRow["IsAllow"] = row.Cells["IsAllowAuthoriseLineClearance"].Value;

                    dtLineClearanceAuthorise.Rows.Add(newRow);
                }

                int errorCode = SetUpConfigurationBLL.UpdateLineClearanceAuthorise(dtLineClearanceAuthorise);

                if (errorCode == 0)
                {
                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    this.Close();
                }

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "UpdateLineClearanceAuthorise", null);
                return;
            }
        }

        /// <summary>
        /// UserValidation
        /// </summary>
        private bool UserValidation()
        {
            _validationMsgs.Clear();

            // Validation : search text is empty
            if (string.IsNullOrEmpty(txtSearchText.Text))
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
            txtSearchText.Text = string.Empty;
            txtSearchText.Focus();
            dgvLineClearanceAuthorise.Rows.Clear();
        }


        #endregion

        #region Event Handlers
        /// <summary>
        /// Event Handler for Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineClearanceAuthoriseSetup_Load(object sender, EventArgs e)
        {

        }

            /// <summary>
            /// Event Handler for click of button Go
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                SearchLineClearanceAuthorise();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnGo_Click", null);
                return;
            }
        }        

        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SearchLineClearanceAuthorise();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSearch_Click", null);
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateLineClearanceAuthorise();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                this.Close();
            }
        }
    }
}
