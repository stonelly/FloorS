using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Framework;
using System.Globalization;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using System.Configuration;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    public partial class DryerStoppageData : FormBase
    {
      #region Member Variables

        private List<DryerDTO> _dryerDetails = null;        
        private DryerDTO _dryerDTO = null;
        private string _screenName = "Configuration SetUp - DryerStoppageData";
        private string _className = "DryerStoppageData";
        private List<string> _validationMsgs = new List<string>();
        private bool _noData = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public DryerStoppageData()
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
            dgvDryerStoppageData.Rows.Clear();

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

                GetDryerStoppageData(_dryerDTO.Id, frmDate, toDate, Constants.ZERO);
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
        /// Get Dryer Details
        /// </summary>
        private void GetDryerDetails()
        {
            _dryerDetails = new List<DryerDTO>();
            try
            {
                _dryerDetails = SetUpConfigurationBLL.GetDryerDetails(WorkStationDTO.GetInstance().LocationId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetDryerDetails", null);
                return;
            }  
        }

        /// <summary>
        /// Get Dryer Stoppage Data
        /// </summary>
        private void GetDryerStoppageData(int? dryerId, DateTime? dateFrom, DateTime? dateTo, int dryerStoppageId)
        {
            dgvDryerStoppageData.Rows.Clear();
            List<DryerStoppageDTO> _dryerStoppageData = null;

            try
            {
                _dryerStoppageData = SetUpConfigurationBLL.GetDryerStoppageData(dryerId, dateFrom, dateTo, dryerStoppageId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetDryerStoppageData", null);
                return;
            }

            if (_dryerStoppageData != null)
            {
                for (int i = 0; i < _dryerStoppageData.Count; i++)
                {
                    dgvDryerStoppageData.Rows.Add();
                    dgvDryerStoppageData[0, i].Value = dgvDryerStoppageData.Rows.Count;
                    dgvDryerStoppageData[1, i].Value = Convert.ToDateTime(_dryerStoppageData[i].StoppageDate).ToString(ConfigurationManager.AppSettings["SetupConfigDateFormat"]);
                    dgvDryerStoppageData[2, i].Value = Convert.ToDateTime(_dryerStoppageData[i].StoppageDate).ToString(Constants.START_TIME);
                    dgvDryerStoppageData[3, i].Value = _dryerStoppageData[i].DryerNumber;
                    dgvDryerStoppageData[4, i].Value = _dryerStoppageData[i].DryerId;
                    dgvDryerStoppageData[5, i].Value = _dryerStoppageData[i].OperatorName;
                    dgvDryerStoppageData[6, i].Value = _dryerStoppageData[i].ReasonText;
                    dgvDryerStoppageData[7, i].Value = _dryerStoppageData[i].Id;
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.NO_DATA_FOUND_SC, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                _noData = true;
            }
        }

        /// <summary>
        /// Get Current Cell Details
        /// </summary>
        private void CurrentCellDetails()
        {
            _dryerDTO = new DryerDTO();
            if (dgvDryerData.Rows.Count > 0)
            {
                int row = dgvDryerData.CurrentRow.Index;
                _dryerDTO.Id = Convert.ToInt32(dgvDryerData[2, row].Value);
                if (_dryerDTO.Id != Constants.ZERO)
                {
                    _dryerDTO.DryerNumber = Convert.ToInt32(dgvDryerData[1, row].Value);
                    btnAdd.Enabled = true;
                    btnGo.Enabled = true;
                    lblMessage.Text = "Dryer " + _dryerDTO.DryerNumber + " Stoppage Details";
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
        private void DryerStoppageData_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
            this.dgvDryerData.AllowUserToAddRows = false;
            this.dgvDryerStoppageData.AllowUserToAddRows = false;
            dpFrom.Controls[Constants.ONE].Text = CommonBLL.GetCurrentDateAndTime().ToString("dd/MM/yyyy");
            dpTo.Controls[Constants.ONE].Text = CommonBLL.GetCurrentDateAndTime().ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Event Handler for datagridview selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void dgvDryerData_SelectionChanged(object sender, EventArgs e)
        {
            CurrentCellDetails();
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
            new AddDryerStoppageData(_dryerDTO.Id, _dryerDTO.DryerNumber, null).ShowDialog();
            if (!string.IsNullOrEmpty(dpFrom.Controls[Constants.ONE].Text.Trim()) &&
                  !string.IsNullOrEmpty(dpTo.Controls[Constants.ONE].Text.Trim()))
            {
                DateValidationMessage();
            }
            else
            {
                GetDryerStoppageData(_dryerDTO.Id, null, null, Constants.ZERO);
            }

            backgroundWorker.RunWorkerAsync();
        }

        private void dgvDryerData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter )
            {
                e.Handled = true;
                _noData = false;
            }            
        }

        private void dgvDryerData_KeyUp(object sender, KeyEventArgs e)
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
            dgvDryerData.Rows.Clear();
            if (_dryerDetails != null)
            {
                for (int i = 0; i < _dryerDetails.Count; i++)
                {
                    dgvDryerData.Rows.Add();
                    dgvDryerData[0, i].Value = dgvDryerData.Rows.Count;
                    dgvDryerData[1, i].Value = _dryerDetails[i].DryerNumber;
                    dgvDryerData[2, i].Value = _dryerDetails[i].Id;
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
            GetDryerDetails();
        }

        /// <summary>
        /// Event Handler for double click event of dryer stoppage data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDryerStoppageData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = dgvDryerStoppageData.CurrentRow.Index;
                int dryerStoppageId = Convert.ToInt16(dgvDryerStoppageData[7, row].Value);

                List<DryerStoppageDTO> dryerStoppageDTO = SetUpConfigurationBLL.GetDryerStoppageData(null, null, null, dryerStoppageId);

                new AddDryerStoppageData(Constants.MINUSONE, Constants.MINUSONE, dryerStoppageDTO[0]).ShowDialog();

                backgroundWorker.RunWorkerAsync();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "dgvDryerStoppageData_CellDoubleClick", null);
                return;
            }
        }
        
        #endregion 
    }
}
