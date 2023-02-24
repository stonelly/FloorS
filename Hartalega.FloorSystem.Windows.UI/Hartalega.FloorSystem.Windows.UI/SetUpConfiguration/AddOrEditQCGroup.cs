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

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Add or Edit QC/Packing Group
    /// File Type: Code file
    /// </summary>
    public partial class AddOrEditQCGroup : FormBase
    {

        #region Member Variables
        private QCGroupDTO _qcGroupDTO;        
        private string _control;
        private string _screenName = "Configuration SetUp - AddOrEditQCGroup";
        private string _className = "AddOrEditQCGroup";
        private string _loggedInUser;

        private string _logScreenName = "Add or Edit QC Group";
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor initialising values retrieved from main form for Edit
        /// </summary>
        /// <param name="ReasonType"></param>
        /// <param name="ReasonTypeId"></param>
        /// <param name="ModuleId"></param>
        /// <param name="Control"></param>
        /// <returns></returns>
        public AddOrEditQCGroup(QCGroupDTO qcGroupDTO, string control)
        {
            InitializeComponent();
            _qcGroupDTO = qcGroupDTO;
            _control = control;
            BindGroupType();
            BindLocationName();
            cmbGroupType.SelectedValue = qcGroupDTO.QCGroupType;
            cmbLocationName.Text = string.IsNullOrEmpty(qcGroupDTO.LocationName) ? string.Empty : qcGroupDTO.LocationName;
            txtGroupName.Text = qcGroupDTO.QCGroupName;
            txtGroupDescription.Text = qcGroupDTO.QCGroupDescription;
            
            chkStop.Checked = qcGroupDTO.IsStopped;
            _loggedInUser = qcGroupDTO.OperatorId;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddOrEditQCGroup", null);
                return;
            }
        }

        /// <summary>
        /// Constructor initialising values retrieved from main form for Add
        /// </summary>
        /// <returns></returns>
        public AddOrEditQCGroup(string loggedInUser,string control)
        {
            InitializeComponent();
            _control = control;
            BindGroupType();
            BindLocationName();
            _loggedInUser = loggedInUser;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddOrEditQCGroup", null);
                return;
            }
        }

        #endregion

        #region User Methods

        /// <summary>
        /// Validation of required fields
        /// </summary>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbGroupType, "Group Type", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtGroupName, "QC/Packing Group", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtGroupDescription, "QC/Packing Group Description", ValidationType.Required));

            if (cmbGroupType.SelectedIndex != Constants.MINUSONE)
                if (cmbGroupType.SelectedValue.ToString().Equals("QC & Packing Group"))
                    validationMesssageLst.Add(new ValidationMessage(cmbLocationName, "Plant", ValidationType.Required));
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
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        /// <summary>
        /// Clear the values of all the controls 
        /// </summary>
        private void ClearForm()
        {
            cmbGroupType.SelectedIndex = Constants.MINUSONE;
            chkStop.Checked = false;
            txtGroupName.Text = string.Empty;
            txtGroupDescription.Text = string.Empty;
            cmbGroupType.Focus();
        }

        /// <summary>
        /// Binds the Group Type ComboBox
        /// </summary>
        private void BindGroupType()
        {
            if (cmbGroupType.Items.Count == Constants.ZERO)
            {
                try
                {
                    cmbGroupType.BindComboBox(CommonBLL.GetEnumText(Constants.QC_GROUP_TYPE), true);
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "BindGroupType", null);
                    return;
                }
            }
        }

        /// <summary>
        /// Binds the Plant ComboBox
        /// </summary>
        private void BindLocationName()
        {
            if (cmbLocationName.Items.Count == Constants.ZERO)
            {
                try
                {
                    cmbLocationName.BindComboBox(MasterTableBLL.GetLocationList(), true);
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "BindLocationName", null);
                    return;
                }
            }
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for Save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsReturned = 0;
            bool isDuplicate = false;

            if (ValidateRequiredFields())
            {
                QCGroupDTO objQCGroup = new QCGroupDTO();
                try
                { 
                    objQCGroup.QCGroupType = Convert.ToString(cmbGroupType.SelectedValue);
                    objQCGroup.QCGroupName = txtGroupName.Text.Trim();
                    objQCGroup.QCGroupDescription = txtGroupDescription.Text.Trim();
                    objQCGroup.IsStopped = chkStop.Checked;
                    objQCGroup.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                    objQCGroup.OperatorId = _loggedInUser;
                    objQCGroup.LocationName = Convert.ToString(cmbLocationName.Text);
                
                    if (_control == Constants.ADD_CONTROL)
                    {
                        isDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsQCGroupDuplicate(objQCGroup.QCGroupName));
                    }
                    if (!isDuplicate)
                    {
                        if (_control == Constants.EDIT_CONTROL)
                        {
                            objQCGroup.Id = _qcGroupDTO.Id;
                        }
                        rowsReturned = SetUpConfigurationBLL.SaveQCGroup(objQCGroup);

                        if (rowsReturned > 0)
                        { 
                            //event log myadamas 20190227
                            EventLogDTO EventLog = new EventLogDTO();
                            EventLog.CreatedBy = _loggedInUser;
                            Constants.EventLog evtAction = Constants.EventLog.Save;
                            EventLog.EventType = Convert.ToInt32(evtAction);
                            EventLog.EventLogType = Constants.eventlogtype;

                            var screenid = CommonBLL.GetScreenIdByScreenName(_logScreenName);
                            CommonBLL.InsertEventLog(EventLog, _logScreenName, screenid.ToString());

                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            this.Close();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.DUPLICATE_QC_GROUP, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        txtGroupName.Text = string.Empty;
                        txtGroupName.Focus();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                    return;
                }
            }
        }

        /// <summary>
        /// Event Handler for Cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Event Handler for setting focus on checkbox on Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkStop_Paint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Focused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
            }
        }
        #endregion

        private void cmbGroupType_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cmbGroupType.SelectedIndex != Constants.MINUSONE) 
            {
                if (cmbGroupType.SelectedValue.ToString().Equals("QC & Packing Group"))
                    cmbLocationName.Enabled = true;
                else
                {
                    cmbLocationName.Enabled = false;
                    cmbLocationName.Text = string.Empty;
                }
            }
        }
    }
}
