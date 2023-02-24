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
    /// Screen Name: QC/Packing Group Master
    /// File Type: Code file
    /// </summary>
    public partial class QCGroupMaster : FormBase
    {
        #region Member Variables
        private string _screenName = "Configuration SetUp - QCGroupMaster";
        private string _className = "QCGroupMaster";
        private QCGroupDTO _qcGroupDTO = new QCGroupDTO();
        List<QCGroupDTO> _qcGroupDetails = null;
        private string _loggedInUser;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor initialising the components
        /// </summary>       
        /// <returns></returns>
        public QCGroupMaster(string loggedInUser)
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
            
        }
        /// <summary>
        /// Get QC/Packing Group Details
        /// </summary>
        private void GetQCGroupDetails()
        {
            try
            {
                _qcGroupDetails = SetUpConfigurationBLL.GetQCGroupMasterData();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetQCGroupDetails", null);
                return;
            }
        }

        /// <summary>
        /// Get Current QC Group Details
        /// </summary>
        private void CurrentQCGroupDetails()
        {
            if (dgvQCGroupMaster.Rows.Count > 0 && dgvQCGroupMaster.CurrentRow != null)
            {
                btnEdit.Enabled = true;
                int row = dgvQCGroupMaster.CurrentRow.Index;
                _qcGroupDTO.Id = Convert.ToInt16(dgvQCGroupMaster[6, row].Value);
                _qcGroupDTO.QCGroupType = Convert.ToString(dgvQCGroupMaster[1, row].Value);
                _qcGroupDTO.QCGroupName = Convert.ToString(dgvQCGroupMaster[2, row].Value);
                _qcGroupDTO.QCGroupDescription = Convert.ToString(dgvQCGroupMaster[3, row].Value);
                _qcGroupDTO.LocationName = Convert.ToString(dgvQCGroupMaster[4, row].Value);
                _qcGroupDTO.IsStopped = Convert.ToBoolean(dgvQCGroupMaster[5, row].Value);
                _qcGroupDTO.OperatorId = _loggedInUser;
            }
        }
        
        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for Form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QCGroupMaster_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
            this.dgvQCGroupMaster.AllowUserToAddRows = false;
        }

        /// <summary>
        /// Event Handler for Add image button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            new AddOrEditQCGroup(_loggedInUser,Constants.ADD_CONTROL).ShowDialog();
            backgroundWorker.RunWorkerAsync();
        }

        // <summary>
        /// Event Handler for Edit image button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            new AddOrEditQCGroup(_qcGroupDTO,Constants.EDIT_CONTROL).ShowDialog();
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Event Handler for selection changed of datagrid view for QC/Packing group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>     
        private void dgvQCGroupMaster_SelectionChanged(object sender, EventArgs e)
        {
            CurrentQCGroupDetails();
        }
        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvQCGroupMaster_KeyDown(object sender, KeyEventArgs e)
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
        private void dgvQCGroupMaster_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CurrentQCGroupDetails();
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
            dgvQCGroupMaster.Rows.Clear();
            if (_qcGroupDetails != null)
            {
                for (int i = 0; i < _qcGroupDetails.Count; i++)
                {
                    dgvQCGroupMaster.Rows.Add();
                    dgvQCGroupMaster[0, i].Value = dgvQCGroupMaster.Rows.Count;
                    dgvQCGroupMaster[1, i].Value = _qcGroupDetails[i].QCGroupType;
                    dgvQCGroupMaster[2, i].Value = _qcGroupDetails[i].QCGroupName;
                    dgvQCGroupMaster[3, i].Value = _qcGroupDetails[i].QCGroupDescription;
                    dgvQCGroupMaster[4, i].Value = _qcGroupDetails[i].LocationName;
                    dgvQCGroupMaster[5, i].Value = _qcGroupDetails[i].IsStopped;
                    dgvQCGroupMaster[6, i].Value = _qcGroupDetails[i].Id;
                }
                CurrentQCGroupDetails();
            }            
        }

        /// <summary>
        /// Event Handler to perform the time consuming tasks asynchronously
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            GetQCGroupDetails();
        }
        #endregion
    }
}
