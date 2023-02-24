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
    /// Screen Name: Reason Master
    /// File Type: Code file
    /// </summary>
    public partial class ReasonMaster : FormBase
    {
        #region Member Variables
        private string _screenName = "Configuration SetUp - ReasonMaster";
        private string _className = "ReasonMaster";
        private string _moduleId;
        private int _reasonTypeId;
        private string _reasonType;
        private int _reasonTextId;
        private string _reasonText;
        private string _shortCode;
        private bool _isScheduled;
        private string _loggedInUser;
        private bool _hasAddEditAccess = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor initialising the components
        /// </summary>       
        /// <returns></returns>
        public ReasonMaster(string loggedInUser)
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
            dgvReasonType.Rows.Clear();
            dgvReasonText.Rows.Clear();
        }

        /// <summary>
        /// Get Reason Type Details
        /// </summary>
        private void getReasonTypeDetails()
        {
            dgvReasonType.Rows.Clear();
            List<ReasonTypeDTO> reasonDTO = null;
            try
            {
                reasonDTO = SetUpConfigurationBLL.GetReasonTypes(_loggedInUser);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getReasonTypeDetails", null);
                return;
            }
            if (reasonDTO != null)
            {
                for (int i = 0; i < reasonDTO.Count; i++)
                {
                    dgvReasonType.Rows.Add();
                    dgvReasonType[0, i].Value = reasonDTO[i].ModuleName;
                    dgvReasonType[1, i].Value = reasonDTO[i].ReasonType;
                    dgvReasonType[2, i].Value = reasonDTO[i].ReasonTypeId;
                    dgvReasonType[3, i].Value = reasonDTO[i].ModuleId;
                }
                CurrentReasonTypeDetails();
            }
        }

        /// <summary>
        /// Get Reason Text Details
        /// </summary>
        /// <param name="reasonId"></param>
        private void getReasonTextDetails(int reasonId, string moduleId)
        {
            List<ReasonDTO> reasonDTO = null;
            pbEdit.Enabled = false;
            dgvReasonText.Rows.Clear();
            try
            {
                reasonDTO = SetUpConfigurationBLL.GetReasonTextDetails(reasonId);

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getReasonTextDetails", null);
                return;
            }

            if (reasonDTO != null)
            {
                for (int i = 0; i < reasonDTO.Count; i++)
                {
                    dgvReasonText.Rows.Add();
                    dgvReasonText[0, i].Value = reasonDTO[i].ReasonType;
                    dgvReasonText[1, i].Value = reasonDTO[i].ReasonText;
                    dgvReasonText[2, i].Value = reasonDTO[i].IsScheduled;
                    dgvReasonText[3, i].Value = reasonDTO[i].ReasonTypeId;
                    dgvReasonText[4, i].Value = reasonDTO[i].ReasonTextId;
                    dgvReasonText[5, i].Value = reasonDTO[i].ShortCode;
                }
                CurrentReasonTextDetails();
            }

            dgvReasonText.Columns[2].Visible = false;
            if (moduleId == Convert.ToString(Convert.ToInt16(Constants.Modules.WASHER)) ||
                moduleId == Convert.ToString(Convert.ToInt16(Constants.Modules.DRYER)) ||
                moduleId == Convert.ToString(Convert.ToInt16(Constants.Modules.PRODUCTION)) ||
                _reasonType == Constants.REASON_PL_STOP.Trim())
            {
                dgvReasonText.Columns[2].Visible = true;
            }
            dgvReasonText.Columns[5].Visible = false;
            if (_reasonType == Constants.REASON_PL_STOP.Trim())
            {
                dgvReasonText.Columns[5].Visible = true;
            }


        }

        /// <summary>
        /// Get Current Reason Text Details
        /// </summary>
        private void CurrentReasonTextDetails()
        {
            if (dgvReasonText.Rows.Count > 0 && dgvReasonText.CurrentRow != null)
            {
                pbEdit.Enabled = true;
                int row = dgvReasonText.CurrentRow.Index;
                _reasonTextId = Convert.ToInt32(dgvReasonText[4, row].Value);
                _reasonText = Convert.ToString(dgvReasonText[1, row].Value);
                _isScheduled = Convert.ToBoolean(dgvReasonText[2, row].Value);
                _shortCode = Convert.ToString(dgvReasonText[5, row].Value);
            }
            else
                pbEdit.Enabled = false;
        }

        /// <summary>
        /// Get Current Reason Type Details
        /// </summary>
        private void CurrentReasonTypeDetails()
        {
            // Added IF condition Amar
            if (dgvReasonType.Rows.Count > 0 && dgvReasonType.CurrentRow != null)
            {
                pbAdd.Enabled = true;
                pbEdit.Enabled = false;
                int row = dgvReasonType.CurrentRow.Index;
                _reasonTypeId = Convert.ToInt32(dgvReasonType[2, row].Value);
                _moduleId = Convert.ToString(dgvReasonType[3, row].Value);
                _reasonType = Convert.ToString(dgvReasonType[1, row].Value);

                getReasonTextDetails(_reasonTypeId, _moduleId);
                try
                {
                    _hasAddEditAccess = Convert.ToBoolean(SetUpConfigurationBLL.IsUserModuleOwner(_loggedInUser, _moduleId));
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "CurrentReasonTypeDetails", null);
                    return;
                }
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for Form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReasonMaster_Load(object sender, EventArgs e)
        {
            this.dgvReasonType.AllowUserToAddRows = false;
            this.dgvReasonText.AllowUserToAddRows = false;
            this.dgvReasonType.AllowUserToDeleteRows = false;
            this.dgvReasonText.AllowUserToDeleteRows = false;
            getReasonTypeDetails();
        }

        /// <summary>
        /// Event Handler for Add image button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbAdd_Click(object sender, EventArgs e)
        {
            if (_hasAddEditAccess)
            {
                new AddorEditReason(_reasonType, _reasonTypeId, _moduleId, _loggedInUser, Constants.ADD_CONTROL).ShowDialog();
                getReasonTypeDetails();
                getReasonTextDetails(_reasonTypeId, _moduleId);
            }
            else
            {
                GlobalMessageBox.Show(Messages.REASON_ADD_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// Event Handler for Edit image button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (_hasAddEditAccess)
            {
                new AddorEditReason(_reasonType, _reasonTypeId, _reasonTextId, _reasonText, _isScheduled, _moduleId,
                                    _loggedInUser, Constants.EDIT_CONTROL, _shortCode).ShowDialog();
                getReasonTypeDetails();
                getReasonTextDetails(_reasonTypeId, _moduleId);

            }
            else
            {
                GlobalMessageBox.Show(Messages.REASON_EDIT_ACCESS_DENIED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// Event Handler for selection changed of datagrid view for Reason type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void dgvReasonType_SelectionChanged(object sender, EventArgs e)
        {
            CurrentReasonTypeDetails();
        }
        /// <summary>
        /// Event Handler for selection changed of datagrid view for Reason text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void dgvReasonText_SelectionChanged(object sender, EventArgs e)
        {
            CurrentReasonTextDetails();
        }
        /// <summary>
        /// Event Handler for click of cell of datagrid view for Reason type
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReasonType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event Handler for click of cell of datagrid view for Reason type
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReasonType_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CurrentReasonTypeDetails();
            }
        }

        /// <summary>
        /// Event Handler for click of cell of datagrid view for Reason text
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReasonText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event Handler for click of cell of datagrid view for Reason text
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReasonText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CurrentReasonTextDetails();
            }
        }
        #endregion
    }
}
