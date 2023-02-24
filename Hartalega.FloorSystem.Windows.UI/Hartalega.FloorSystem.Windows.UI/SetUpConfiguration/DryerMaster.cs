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
    /// Screen Name: Dryer Master
    /// File Type: Code file
    /// </summary> 
    public partial class DryerMaster : FormBase
    {
        #region Method Variables

        private List<DryerDTO> _dryerDetails;
        private DryerDTO _dryerDTO;
        private List<DryerDetailsForExcel> _dryerDetailsForExcel;
        private string _screenName = "Configuration SetUp - DryerMaster";
        private string _className = "DryerMaster";
        private string _loggedInUser;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor initialising the components
        /// </summary>       
        /// <returns></returns>
        public DryerMaster(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
        }
        #endregion

        #region User Methods

        /// <summary>
        /// Get Dryer Details
        /// </summary>
        private void DryerDetails()
        {
            _dryerDetails = new List<DryerDTO>();
            try
            {
                _dryerDetails = SetUpConfigurationBLL.GetDryerDetails(WorkStationDTO.GetInstance().LocationId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "DryerDetails", null);
                return;
            }
        }

        /// <summary>
        /// Get Current Cell Details
        /// </summary>
        private void CurrentCellDetails()
        {
            _dryerDTO = new DryerDTO();
            if (dgvDryerDetails.Rows.Count > 0 && dgvDryerDetails.CurrentRow != null)
            {
                btnEdit.Enabled = true;
                int row = dgvDryerDetails.CurrentRow.Index;                            
                _dryerDTO.LocationId = Convert.ToString(dgvDryerDetails[1, row].Value);
                _dryerDTO.DryerNumber = Convert.ToInt32(dgvDryerDetails[2, row].Value);
                _dryerDTO.GloveTypeDescription = Convert.ToString(dgvDryerDetails[3, row].Value);
                _dryerDTO.GloveSize = Convert.ToString(dgvDryerDetails[4, row].Value);             
                _dryerDTO.Cold = Convert.ToBoolean(dgvDryerDetails[5, row].Value);
                _dryerDTO.Hot = Convert.ToBoolean(dgvDryerDetails[6, row].Value);
                _dryerDTO.HotAndCold = Convert.ToBoolean(dgvDryerDetails[7, row].Value);
                _dryerDTO.IsStopped = Convert.ToBoolean(dgvDryerDetails[8, row].Value);
                _dryerDTO.IsScheduledStop = Convert.ToBoolean(dgvDryerDetails[9, row].Value);
                _dryerDTO.CheckGlove = Convert.ToBoolean(dgvDryerDetails[10, row].Value);
                _dryerDTO.CheckSize = Convert.ToBoolean(dgvDryerDetails[11, row].Value);
                _dryerDTO.Id = Convert.ToInt32(dgvDryerDetails[12, row].Value);
                _dryerDTO.GloveType = Convert.ToString(dgvDryerDetails[13, row].Value);
                _dryerDTO.OperatorId = _loggedInUser;
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
            dgvDryerDetails.Rows.Clear();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for Edit button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            new EditDryerDetails(_dryerDTO).ShowDialog();
            backgroundWorker.RunWorkerAsync();
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
                CommonBLL.ExportToExcel<DryerDetailsForExcel>(SetUpConfigurationBLL.GetColumnHeadersDryer(), "Dryer Details", _dryerDetailsForExcel);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnExcel_Click", null);
                return;
            }
        }

        /// <summary>
        /// Event Handler for form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DryerMaster_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
            btnEdit.Enabled = false;
            this.dgvDryerDetails.AllowUserToAddRows = false;
            this.dgvDryerDetails.AllowUserToDeleteRows = false;
        }

        /// <summary>
        /// Event Handler for datagridview selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void dgvDryerDetails_SelectionChanged(object sender, EventArgs e)
        {
            CurrentCellDetails();
        }

        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDryerDetails_KeyDown(object sender, KeyEventArgs e)
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
        private void dgvDryerDetails_KeyUp(object sender, KeyEventArgs e)
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
            dgvDryerDetails.Rows.Clear();
            _dryerDetailsForExcel = new List<DryerDetailsForExcel>();
            if (_dryerDetails != null)
            {
                for (int i = 0; i < _dryerDetails.Count; i++)
                {
                    dgvDryerDetails.Rows.Add();
                    dgvDryerDetails[0, i].Value = dgvDryerDetails.Rows.Count;
                    dgvDryerDetails[1, i].Value = Convert.ToString(_dryerDetails[i].LocationId);
                    dgvDryerDetails[2, i].Value = Convert.ToString(_dryerDetails[i].DryerNumber);
                    dgvDryerDetails[3, i].Value = Convert.ToString(_dryerDetails[i].GloveType);
                    dgvDryerDetails[4, i].Value = _dryerDetails[i].GloveSize;
                    dgvDryerDetails[5, i].Value = Convert.ToInt32(_dryerDetails[i].Cold);
                    dgvDryerDetails[6, i].Value = Convert.ToInt32(_dryerDetails[i].Hot);
                    dgvDryerDetails[7, i].Value = Convert.ToInt32(_dryerDetails[i].HotAndCold);
                    dgvDryerDetails[8, i].Value = _dryerDetails[i].IsStopped;
                    dgvDryerDetails[9, i].Value = _dryerDetails[i].IsScheduledStop;
                    dgvDryerDetails[10, i].Value = _dryerDetails[i].CheckGlove;
                    dgvDryerDetails[11, i].Value = _dryerDetails[i].CheckSize;
                    dgvDryerDetails[12, i].Value = _dryerDetails[i].Id;
                    dgvDryerDetails[13, i].Value = _dryerDetails[i].GloveType;

                    //Fill list for Excel
                    DryerDetailsForExcel dryerDetailsDTO = new DryerDetailsForExcel();
                    dryerDetailsDTO.Index = dgvDryerDetails.Rows.Count;
                    dryerDetailsDTO.LocationId = _dryerDetails[i].LocationId;
                    dryerDetailsDTO.DryerNumber = _dryerDetails[i].DryerNumber;
                    dryerDetailsDTO.GloveType = _dryerDetails[i].GloveType;
                    dryerDetailsDTO.GloveSize = _dryerDetails[i].GloveSize;
                    dryerDetailsDTO.Cold = _dryerDetails[i].Cold;
                    dryerDetailsDTO.Hot = _dryerDetails[i].Hot;
                    dryerDetailsDTO.HotAndCold = _dryerDetails[i].HotAndCold;
                    dryerDetailsDTO.IsStopped = _dryerDetails[i].IsStopped;
                    dryerDetailsDTO.IsScheduledStop = _dryerDetails[i].IsScheduledStop;
                    dryerDetailsDTO.CheckGlove = _dryerDetails[i].CheckGlove;
                    dryerDetailsDTO.CheckSize = _dryerDetails[i].CheckSize;

                    _dryerDetailsForExcel.Add(dryerDetailsDTO);
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
            DryerDetails();
        }
        #endregion
    }

    #region DryerDetailsForExcel
    public class DryerDetailsForExcel
    {

        /// <summary>
        /// Id for washer
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Id for washer
        /// </summary>
        ///         
        public string LocationId { get; set; }
        public int DryerNumber { get; set; }

        /// <summary>
        /// GloveType
        /// </summary>       
        public string GloveType { get; set; }

        /// <summary>
        /// GloveSize
        /// </summary>
        public string GloveSize { get; set; }
        /// <summary>
        ///Cold
        /// </summary>
        public bool Cold { get; set; }
        /// <summary>
        /// Hot
        /// </summary>
        public bool Hot { get; set; }

        /// <summary>
        /// HotAndCold
        /// </summary>
        public bool HotAndCold { get; set; }
        /// <summary>
        /// IsStopped
        /// </summary>
        public bool IsStopped { get; set; }
        /// <summary>
        /// IsScheduledStop
        /// </summary>
        public bool IsScheduledStop { get; set; }
        /// <summary>
        /// CheckGlove
        /// </summary>
        public bool CheckGlove { get; set; }
        /// <summary>
        /// CheckSize
        /// </summary>
        public bool CheckSize { get; set; }
    }
    #endregion
}

