using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Washer Master
    /// File Type: Code file
    /// </summary> 
    public partial class WasherMaster : FormBase
    {
        #region Member Variables

        private List<WasherDTO> _washerDetails = null;
        private WasherDTO _washerDTO = null;
        private List<WasherDetailsForExcel> _washerDetailsForExcel = null;
        private string _screenName = "Configuration SetUp - WasherMaster";
        private string _className = "WasherMaster";
        private string _loggedInUser;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor initialising the components
        /// </summary>       
        /// <returns></returns>
        public WasherMaster(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
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
            dgvWasherDetails.Rows.Clear();
        }

        /// <summary>
        /// Get Washer Details
        /// </summary>
        private void WasherDetails()
        {
            _washerDetails = new List<WasherDTO>();
            try
            {
                _washerDetails = SetUpConfigurationBLL.GetWasherDetails(WorkStationDTO.GetInstance().LocationId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "WasherDetails", null);
                return;
            }
        }

        /// <summary>
        /// Get Current Cell Details
        /// </summary>
        private void CurrentCellDetails()
        {
            _washerDTO = new WasherDTO();
            if (dgvWasherDetails.Rows.Count > 0 && dgvWasherDetails.CurrentRow != null)
            {
                btnEdit.Enabled = true;
                int row = dgvWasherDetails.CurrentRow.Index;
                _washerDTO.LocationId = Convert.ToString(dgvWasherDetails[1, row].Value);
                _washerDTO.WasherNumber = Convert.ToInt32(dgvWasherDetails[2, row].Value);               
                _washerDTO.GloveTypeDescription = Convert.ToString(dgvWasherDetails[3, row].Value);
                _washerDTO.GloveSize = Convert.ToString(dgvWasherDetails[4, row].Value);
                _washerDTO.Stop = Convert.ToBoolean(dgvWasherDetails[5, row].Value);
                _washerDTO.Id = Convert.ToInt32(dgvWasherDetails[6, row].Value);
                _washerDTO.GloveType = Convert.ToString(dgvWasherDetails[7, row].Value);
                _washerDTO.OperatorId = _loggedInUser;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WasherMaster_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
            this.dgvWasherDetails.AllowUserToAddRows = false;
            this.dgvWasherDetails.AllowUserToDeleteRows = false;
        }

        /// <summary>
        /// Event Handler for Excel button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                CommonBLL.ExportToExcel<WasherDetailsForExcel>(SetUpConfigurationBLL.GetColumnHeadersWasher(), "Washer Details", _washerDetailsForExcel);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnExcel_Click", null);
                return;
            }
        }

        /// <summary>
        /// Event Handler for Edit button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            new EditWasherDetails(_washerDTO).ShowDialog();
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Event Handler for datagridview selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void dgvWasherDetails_SelectionChanged(object sender, EventArgs e)
        {
            CurrentCellDetails();
        }
        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvWasherDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvWasherDetails_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
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
            btnEdit.Enabled = false;
            dgvWasherDetails.Rows.Clear();
            _washerDetailsForExcel = new List<WasherDetailsForExcel>();
            if (_washerDetails != null)
            {
                for (int i = 0; i < _washerDetails.Count; i++)
                {
                    dgvWasherDetails.Rows.Add();
                    dgvWasherDetails[0, i].Value = dgvWasherDetails.Rows.Count;
                    dgvWasherDetails[1, i].Value = _washerDetails[i].LocationId;
                    dgvWasherDetails[2, i].Value = _washerDetails[i].WasherNumber;
                    dgvWasherDetails[3, i].Value = _washerDetails[i].GloveType;
                    dgvWasherDetails[4, i].Value = _washerDetails[i].GloveSize;
                    dgvWasherDetails[5, i].Value = _washerDetails[i].Stop;
                    dgvWasherDetails[6, i].Value = _washerDetails[i].Id;
                    dgvWasherDetails[7, i].Value = _washerDetails[i].GloveType;

                    //Fill list for Excel
                    WasherDetailsForExcel washerDetailsDTO = new WasherDetailsForExcel();
                    washerDetailsDTO.Index = dgvWasherDetails.Rows.Count;
                    washerDetailsDTO.LocationId = _washerDetails[i].LocationId;
                    washerDetailsDTO.WasherNumber = _washerDetails[i].WasherNumber;
                    washerDetailsDTO.GloveType = _washerDetails[i].GloveType;
                    washerDetailsDTO.GloveSize = _washerDetails[i].GloveSize;
                    washerDetailsDTO.Stop = _washerDetails[i].Stop;

                    _washerDetailsForExcel.Add(washerDetailsDTO);
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
            WasherDetails();
        }
        #endregion 
    }

    #region WasherDetailsForExcel
    public class WasherDetailsForExcel
    {
        /// <summary>
        /// Index for washer
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Washer Number for a washer
        /// </summary>
        
        public string LocationId { get; set; }
        public int WasherNumber { get; set; }
        /// <summary>
        /// Glove Type
        /// </summary>
        public string GloveType { get; set; }

        /// <summary>
        /// Glove Size
        /// </summary>
        public string GloveSize { get; set; }
        /// <summary>
        /// IStopped
        /// </summary>
        public bool Stop { get; set; }
    }
    #endregion
}
