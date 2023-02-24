using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Washer Stoppage Data
    /// File Type: Code file
    /// </summary>
    public partial class WasherStoppageData : FormBase
    {
        #region Member Variables

        private List<WasherDTO> _washerDetails = null;        
        private WasherDTO _washerDTO = null;
        private string _screenName = "Configuration SetUp - WasherStoppageData";
        private string _className = "WasherStoppageData";
        private List<string> _validationMsgs = new List<string>();
        private bool _noData = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public WasherStoppageData()
        {
            InitializeComponent();
        }
        #endregion

        #region User Methods
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
        }

        private void DateValidationMessage()
        {
            _validationMsgs.Clear();
            _validationMsgs = DateValidation.DateValidator(dpFrom.Controls[Constants.ONE].Text.Trim(),
                                                             dpTo.Controls[Constants.ONE].Text.Trim());
            dgvWasherStoppageData.Rows.Clear();

            if (_validationMsgs.Count == Constants.ZERO && !string.IsNullOrEmpty(dpFrom.Controls[Constants.ONE].Text.Trim()) &&
                  !string.IsNullOrEmpty(dpTo.Controls[Constants.ONE].Text.Trim()))
            {
                DateTime? frmDate = null;
                DateTime? toDate = null;

                if (!string.IsNullOrEmpty(dpFrom.Controls[Constants.ONE].Text.Trim()))
                {
                    frmDate = DateTime.ParseExact(dpFrom.Controls[Constants.ONE].Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                }


                if (!string.IsNullOrEmpty(dpTo.Controls[Constants.ONE].Text.Trim()))
                {
                    toDate = DateTime.ParseExact(dpTo.Controls[Constants.ONE].Text.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None);
                }

                GetWasherStoppageData(_washerDTO.Id, frmDate, toDate,Constants.ZERO);
            }
            else
            {
                StringBuilder validationsAlert = new StringBuilder();
                if (_validationMsgs.Count != Constants.ZERO)
                {
                    foreach (string str in _validationMsgs)
                    {
                        validationsAlert.Append(str);
                        validationsAlert.Append(Environment.NewLine);
                    }
                }
                else
                {
                    validationsAlert.Append(Messages.DATE_REQUIRED);
                }
                GlobalMessageBox.Show(Convert.ToString(validationsAlert), Constants.AlertType.Information, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                dpFrom.Controls[Constants.ONE].Text = string.Empty;
                dpTo.Controls[Constants.ONE].Text = string.Empty;

            }
        }
        /// <summary>
        /// Get Washer Details
        /// </summary>
        private void GetWasherDetails()
        {
            _washerDetails = new List<WasherDTO>();
            try
            {
                _washerDetails = SetUpConfigurationBLL.GetWasherDetails(WorkStationDTO.GetInstance().LocationId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetWasherDetails", null);
                return;
            }
        }

        /// <summary>
        /// Get Washer Stoppage Data
        /// </summary>
        private void GetWasherStoppageData(int? washerId, DateTime? dateFrom, DateTime? dateTo,int washerStoppageId)
        {
            dgvWasherStoppageData.Rows.Clear();
            List<WasherStoppageDTO> _washerStoppageData = null;

            try
            {
                _washerStoppageData = SetUpConfigurationBLL.GetWasherStoppageData(washerId, dateFrom, dateTo, washerStoppageId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetWasherStoppageData", null);
                return;
            }

            if (_washerStoppageData != null)
            {
                for (int i = 0; i < _washerStoppageData.Count; i++)
                {
                    dgvWasherStoppageData.Rows.Add();
                    dgvWasherStoppageData[0, i].Value = dgvWasherStoppageData.Rows.Count;
                    dgvWasherStoppageData[1, i].Value = Convert.ToDateTime(_washerStoppageData[i].StoppageDate).ToString(ConfigurationManager.AppSettings["SetupConfigDateFormat"]);
                    dgvWasherStoppageData[2, i].Value = Convert.ToDateTime(_washerStoppageData[i].StoppageDate).ToString(Constants.START_TIME);
                    dgvWasherStoppageData[3, i].Value = _washerStoppageData[i].WasherNumber;
                    dgvWasherStoppageData[4, i].Value = _washerStoppageData[i].WasherId;
                    dgvWasherStoppageData[5, i].Value = _washerStoppageData[i].OperatorName;
                    dgvWasherStoppageData[6, i].Value = _washerStoppageData[i].ReasonText;
                    dgvWasherStoppageData[7, i].Value = _washerStoppageData[i].Id;
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                _noData = true;
            }
        }

        /// <summary>
        /// Get Current Cell Details
        /// </summary>
        private void CurrentCellDetails()
        {
            _washerDTO = new WasherDTO();
            if (dgvWasherData.Rows.Count > 0)
            {
                int row = dgvWasherData.CurrentRow.Index;
                _washerDTO.Id = Convert.ToInt32(dgvWasherData[2, row].Value);
                if (_washerDTO.Id != Constants.ZERO)
                {
                    _washerDTO.WasherNumber = Convert.ToInt32(dgvWasherData[1, row].Value);
                    btnAdd.Enabled = true;
                    btnGo.Enabled = true;
                    lblMessage.Text = "Washer " + _washerDTO.WasherNumber + " Stoppage Details";
                    DateValidationMessage();
                } 
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WasherStoppageData_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
            this.dgvWasherData.AllowUserToAddRows = false;
            this.dgvWasherStoppageData.AllowUserToAddRows = false;
            dpFrom.Controls[Constants.ONE].Text = CommonBLL.GetCurrentDateAndTime().ToString("dd/MM/yyyy");
            dpTo.Controls[Constants.ONE].Text = CommonBLL.GetCurrentDateAndTime().ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Event Handler for datagridview selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void dgvWasherData_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                CurrentCellDetails();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "dgvWasherData_SelectionChanged", null);
                return;
            }
        }
        /// <summary>
        /// Event Handler for Button Go
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            DateValidationMessage();
        }

        /// <summary>
        /// Event Handler for Button Add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                new AddWasherStoppageData(_washerDTO.Id, _washerDTO.WasherNumber,null).ShowDialog();
                if (!string.IsNullOrEmpty(dpFrom.Controls[Constants.ONE].Text.Trim()) &&
                    !string.IsNullOrEmpty(dpTo.Controls[Constants.ONE].Text.Trim()))
                {
                    DateValidationMessage();
                }
                else
                {
                    GetWasherStoppageData(_washerDTO.Id, null, null,Constants.ZERO);
                }
                backgroundWorker.RunWorkerAsync();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnAdd_Click", null);
                return;
            }
        }

        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvWasherData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                _noData = false;
            }
        }

        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvWasherData_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && !_noData)
            {
                CurrentCellDetails();
            }
        }
        /// <summary>
        /// Event Handler to perform the time consuming tasks asynchronously
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            dgvWasherData.Rows.Clear();
            if (_washerDetails != null)
            {
                for (int i = 0; i < _washerDetails.Count; i++)
                {
                    dgvWasherData.Rows.Add();
                    dgvWasherData[0, i].Value = dgvWasherData.Rows.Count;
                    dgvWasherData[1, i].Value = _washerDetails[i].WasherNumber;
                    dgvWasherData[2, i].Value = _washerDetails[i].Id;
                }
                CurrentCellDetails();
            }           
        }

        /// <summary>
        /// Event Handler to perform the time consuming tasks asynchronously
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                GetWasherDetails();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "backgroundWorker_DoWork", null);
                return;
            }
        }
        #endregion 

        private void dgvWasherStoppageData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = dgvWasherStoppageData.CurrentRow.Index;
                int washerStoppageId = Convert.ToInt16(dgvWasherStoppageData[7, row].Value);

                List<WasherStoppageDTO> washerStoppageDTO = SetUpConfigurationBLL.GetWasherStoppageData(null, null, null, washerStoppageId);

                new AddWasherStoppageData(Constants.MINUSONE, Constants.MINUSONE, washerStoppageDTO[0]).ShowDialog();

                backgroundWorker.RunWorkerAsync();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "dgvWasherStoppageData_CellDoubleClick", null);
                return;
            }
        }
    }
}
