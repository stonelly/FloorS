using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.ProductionLogging
{
    public partial class EditProductionLineStop : FormBase
    {
       
        #region Private Class Members
        private int _production_Logging_Time_Configuration = Constants.ZERO;
        private string _productionLineId = string.Empty;
        private ProductionLineDetailsDTO _pld = null;
        private ProductionLineStartStop _plss = null;
        private int _id = Constants.ZERO;
        private DateTime _lineDate = new DateTime();
        private TimeSpan _lineTime = new TimeSpan();

        private DateTime lastactivity_lineDate = new DateTime();
        private TimeSpan lastactivity_lineTime = new TimeSpan();
        private DateTime lastactivity_DateTime = new DateTime();

        private const string _screenName = "ProductionLineStop";
        private const string _className = "ProductionLineStop";
        private string _moduleName = string.Empty;
        private int _reasontypeid = Constants.ZERO;
        private int _areaid = Constants.ZERO;
        private ProductionLoggingActivitiesDTO old_plaDTO = null;
        int next_productionLineId = 0;
        private string _modifiedby = "NA";

        private readonly string _department = string.Empty;
        #endregion

        #region Constructors
        public EditProductionLineStop()
        {
            InitializeComponent();
        }

        public EditProductionLineStop(string productionLineId, ProductionLineStartStop plss)
            : this()
        {
            _productionLineId = productionLineId;
            _plss = plss;
            _moduleName = SetUpConfiguration.MainMenu.GetEnumDescription(Constants.ScreenNames.ProductionLogging);
        }

        public EditProductionLineStop(string productionLineId, int id, DateTime lineDate, TimeSpan lineTime, 
            ProductionLineStartStop plss, ProductionLoggingActivitiesDTO _plaDTO,int reasontypeid, string _loggedinuser,
            string department)
            : this()
        {
            _productionLineId = productionLineId;
            _id = id;
            _lineDate = lineDate;
            _lineTime = lineTime;
            _plss = plss;
            old_plaDTO = _plaDTO;
            _reasontypeid = reasontypeid;
            _modifiedby = _loggedinuser;
            _department = department;
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Populate form fields on form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductionLineStop_Load(object sender, EventArgs e)
        {
            try
            {
                //dtpDate.Text = CommonBLL.GetCurrentDateAndTime().ToString();
                //dtpTime.Text = CommonBLL.GetCurrentDateAndTime().ToString();
                dtpDate.Text = _lineDate.ToString(ConfigurationManager.AppSettings["smallDateFormat"]);
                //dtpDate.Enabled = false;
                dtpTime.Text = _lineTime.ToString();
                PopulateFormFields();
                BindArea();
                BindReason();
                BindDepartment();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, _productionLineId);
                return;
            }
        }

        private void BindArea()
        {
            if (_reasontypeid != 0)
            {
                int Areaid = ProductionLoggingBLL.GetAreaId(_reasontypeid);
                ddlArea.SelectedValue =  Areaid.ToString();
                ddReason.Enabled = true;
                FilterReasonsByArea(Areaid);
                ddReason.SelectedValue = _reasontypeid.ToString();
            }
        }

        private void BindReason()
        {
            try
            {
                cmbReason.BindComboBox(CommonBLL.GetReasons(Constants.EDIT_REASON_PL, Convert.ToString(Convert.ToInt16((Constants.Modules.PRODUCTIONLOGGING)))), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindEditReason", null);
                return;
            }

            if (old_plaDTO.ReasonStartStopId != 0)
            {
                cmbReason.SelectedValue = old_plaDTO.ReasonStartStopId.ToString();
            }
        }

        private void BindDepartment()
        {
            if (!string.IsNullOrEmpty(_department))
            {
                ddlDepartment.Text = _department;

            }
        }

        /// <summary>
        /// Clear form fields on cancel button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CONFIRM_CANCEL_TRANSACTION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                //ClearForm();
                Close();
            }
        }

        /// <summary>
        /// Save activity details on Save button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //bool proceed = false;
            //if (!txtRemarks.Enabled)
            //{
            //    proceed = true;
            //}
            //else if ((txtRemarks.Enabled) && (txtRemarks.TextLength > 0))
            //{
            //    proceed = true;
            //}
            ProductionLoggingActivitiesDTO obj_plaDTO = new ProductionLoggingActivitiesDTO();
            if (ValidateEditReasonField())
            {

                //bool isPLUpdateRequired = false;
                //string workStationId = WorkStationDTO.GetInstance().WorkStationId;
                //DateTime lastActivityStartDateTime = new DateTime(_lineDate.Year, _lineDate.Month, _lineDate.Day, _lineTime.Hours, _lineTime.Minutes, _lineTime.Seconds);
                //string lineId = txtLine.Text;
                DateTime lineDate = dtpDate.Value.Date;
                TimeSpan lineTime = new TimeSpan(dtpTime.Value.Hour, dtpTime.Value.Minute, Constants.ZERO);
                //string activityType = txtActivityType.Text;

                //int reasonId = Convert.ToInt32(ddReason.SelectedValue);
                obj_plaDTO.ProductionLineId = txtLine.Text;int reasonId = Constants.ZERO;reasonId = int.Parse(cmbReason.SelectedValue.ToString());
                obj_plaDTO.ReasonStartStopId = reasonId;
                obj_plaDTO.LineTime = new TimeSpan(dtpTime.Value.Hour, dtpTime.Value.Minute, Constants.ZERO);
                obj_plaDTO.ReasonTypeId = int.Parse(ddReason.SelectedValue.ToString());
                obj_plaDTO.Department = ddlDepartment.SelectedValue.ToString().Trim();

                DateTime currentActivityStartDateTime = new DateTime(lineDate.Year, lineDate.Month, lineDate.Day, lineTime.Hours, lineTime.Minutes, lineTime.Seconds);
               DateTime currentActivityStopDateTime = new DateTime(lineDate.Year, lineDate.Month, lineDate.Day, lineTime.Hours, lineTime.Minutes, lineTime.Seconds);
                SetlastActivityDatetime(_productionLineId);
                string duration = Framework.Common.DateFunctions.GetDuration(lastactivity_DateTime, currentActivityStartDateTime);
                TimeSpan activityTimeDifference = CommonBLL.GetCurrentDateAndTime().Subtract(currentActivityStartDateTime);
                int activityTimeDifferenceInMinutes = (activityTimeDifference.Days * Constants.TWENTYFOUR * Constants.SIXTY) + (activityTimeDifference.Hours * Constants.SIXTY) + (activityTimeDifference.Minutes);
                string remarks = txtRemarks.Text.Trim();

                //int speed = ProductionLoggingBLL.GetLineSpeed(lineId);               
                int noofrows = ProductionLoggingBLL.UpdateProductionLoggingActivityStop(lineDate, lineTime, _id,
                    reasonId,int.Parse(ddReason.SelectedValue.ToString()), duration,"STOP", obj_plaDTO.Department);

                //int noofrows = ProductionLoggingBLL.SaveProductionLoggingActivityStop(lineId, lineDate, lineTime, activityType, reasonId, duration, isPLUpdateRequired, workStationId, remarks, speed);
                if (noofrows > Constants.ZERO)
                {
                    //if (SetNextActivityDatetime(_productionLineId))
                    //{
                    //    //duration = Framework.Common.DateFunctions.GetDuration(currentActivityStopDateTime, lastactivity_DateTime);
                    //    duration = string.Empty;
                    //    noofrows = ProductionLoggingBLL.UpdateProductionLoggingActivityStart(lineDate, lineTime, next_productionLineId, 0, "STOP", duration);
                    //}

                    AuditLogDTO AuditLog = new AuditLogDTO();
                    AuditLog.SourceTable = "ProductionLoggingActivity";
                    AuditLog.CreatedBy = _modifiedby;
                    AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId; string refid = ""; refid = _id.ToString();
                    AuditLog.ReferenceId = refid;               
                    AuditLog.FunctionName = "Edit-ProductionLoggingActivityStop";Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), Constants.Update);
                    AuditLog.AuditAction = Convert.ToInt32(audAction);
                    AuditLog.UpdateColumns = old_plaDTO.DetailedCompare(obj_plaDTO).GetPropChanges();
                    string audlog = CommonBLL.SerializeTOXML(AuditLog);
                    int rowsReturned = ProductionLoggingBLL.UpdateProductionLoggingActivityStart_Audit(audlog);

                    GlobalMessageBox.Show("Successfully Updated", Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    Close();
                }
            }
            //else
            //{
            //    GlobalMessageBox.Show(Messages.REQUIRED_DATA, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            //}
        }

        private void SetlastActivityDatetime(string _productionLineId)
        {
            ProductionLoggingActivitiesDTO _lastactivity_pla = new ProductionLoggingActivitiesDTO();
            DateTime currentActivityStartDateTime = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, dtpDate.Value.Day, dtpTime.Value.Hour, dtpTime.Value.Minute, dtpTime.Value.Second);

            _lastactivity_pla = ProductionLoggingBLL.GetLastActivityForProductionLine(_productionLineId, currentActivityStartDateTime);

            if(_lastactivity_pla.Id == _id)
            {
                currentActivityStartDateTime = new DateTime(_lineDate.Year, _lineDate.Month,
                    _lineDate.Day, _lineTime.Hours, _lineTime.Minutes, _lineTime.Seconds);
                _lastactivity_pla = ProductionLoggingBLL.GetLastActivityForProductionLine(_productionLineId, currentActivityStartDateTime);
            }
            lastactivity_lineDate = _lastactivity_pla.LineDate;
            lastactivity_lineTime = _lastactivity_pla.LineTime;
            lastactivity_DateTime = new DateTime(lastactivity_lineDate.Year, lastactivity_lineDate.Month, lastactivity_lineDate.Day,
            lastactivity_lineTime.Hours, lastactivity_lineTime.Minutes, lastactivity_lineTime.Seconds);
        }

        /// <summary>
        /// Makes the reason combo box filter out records by area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlArea_SelectedChangeCommited(object sender, EventArgs e)
        {
            ddReason.Enabled = true;
            FilterReasonsByArea(int.Parse(ddlArea.SelectedValue.ToString().Trim()));
        }

        /// <summary>
        /// Makes the reason combo box filter out records by area
        /// </summary>
        private void ddReason_SelectedChangeCommitted(object sender, EventArgs e)
        {
            bool ret = RemarkCheck(int.Parse(ddReason.SelectedValue.ToString().Trim()));
            if (ret)
            {
                txtRemarks.Enabled = true;
            }
            else
            {
                txtRemarks.Enabled = false;
                txtRemarks.Text = "";
            }
        }

        #endregion

        #region User Methods
        
        private void FilterReasonsByArea(int AreaID)
        {
            try
            {
                List<DropdownDTO> reasonList = null;
                reasonList = ProductionLoggingBLL.GetReasonTextByArea(AreaID);
                ddReason.DataSource = reasonList;
                ddReason.DisplayMember = "DisplayField";
                ddReason.ValueMember = "IDField";
                ddReason.SelectedIndex = Constants.MINUSONE;
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, null);
                return;
            }
        }

        /// <summary>
        /// Check Remark enabled for Downtime Reason
        /// </summary>
        /// 

        private bool RemarkCheck(int ReasonId)
        {
            return ProductionLoggingBLL.CheckReasonRemark(ReasonId);
        }

        /// <summary>
        /// Populate form fields
        /// </summary>

        private void PopulateFormFields()
        {
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, null);
                return;
            }
            if (string.IsNullOrEmpty(_moduleName))
            {
                _production_Logging_Time_Configuration = Convert.ToInt32(FloorSystemConfiguration.GetInstance().PLTimeConfiguration);
                if (_production_Logging_Time_Configuration == 0)
                {
                    GlobalMessageBox.Show(Messages.PRODCUCTION_LINE_TIME_CONFIGURATION_NOT_DONE, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    Close();
                }
            }

            //added by Cheah (2016-10-10)
            List<DropdownDTO> areaList = null;
            areaList = ProductionLoggingBLL.GetArea();
            ddlArea.DataSource = areaList;
            ddlArea.DisplayMember = "DisplayField";
            ddlArea.ValueMember = "IDField";
            ddlArea.SelectedIndex = Constants.MINUSONE;
            //end add

            List<DropdownDTO> reasonList = null;
            int reasonTypeId = Constants.ZERO;
            _pld = ProductionLoggingBLL.GetProductionLineForStop(_productionLineId);
            txtLine.Text = _productionLineId;
            txtFormer.Text = String.Format(Constants.NUMBER_FORMAT, _pld.Formers);
            txtPcsPerHour.Text = String.Format(Constants.NUMBER_FORMAT, _pld.Speed);
            txtCyclePerHour.Text = Convert.ToString(_pld.Cycle);
            reasonTypeId = CommonBLL.GetReasonTypeIdForReasonType(Constants.REASON_PL_STOP);
            reasonList = ProductionLoggingBLL.GetReasonTextByReasonTypeId(reasonTypeId);
            ddReason.DataSource = reasonList;
            ddReason.DisplayMember = "DisplayField";
            ddReason.ValueMember = "IDField";
            ddReason.SelectedIndex = Constants.MINUSONE;

            //populate Department DDL
            ddlDepartment.DataSource = ProductionLoggingBLL.GetPLDepartment();
            ddlDepartment.DisplayMember = "DisplayField";
            ddlDepartment.ValueMember = "IDField";
            ddlDepartment.SelectedIndex = Constants.MINUSONE;
        }

        /// <summary>
        /// Validate form fields
        /// </summary>
        /// <returns>True if validation succeeds else false</returns>
        private bool ValidateFormFields()
        {
            List<string> validationMsgs = new List<string>();
            DateTime currentTime = CommonBLL.GetCurrentDateAndTime();
            DateTime lastActivityStartDateTime = new DateTime(_lineDate.Year, _lineDate.Month, _lineDate.Day, _lineTime.Hours, _lineTime.Minutes, _lineTime.Seconds);
            DateTime lineDate = dtpDate.Value.Date;
            TimeSpan lineTime = new TimeSpan(dtpTime.Value.Hour, dtpTime.Value.Minute, Constants.ZERO);

            DateTime newActivityStartDateTime = new DateTime(lineDate.Year, lineDate.Month, lineDate.Day, lineTime.Hours, lineTime.Minutes, lineTime.Seconds);
            DateTime allowedDateTime = CommonBLL.GetCurrentDateAndTime().Subtract(new TimeSpan(_production_Logging_Time_Configuration, Constants.ZERO, Constants.ZERO));

            if (string.IsNullOrEmpty(_moduleName))
            {
                if (newActivityStartDateTime < allowedDateTime)
                {
                    validationMsgs.Add(Messages.NEW_ACTIVITY_DATE_LESSER_THAN_PL_TIME_CONFIGURATION + " " + _production_Logging_Time_Configuration + " hours");
                }

                else if (newActivityStartDateTime <= lastActivityStartDateTime)
                {
                    validationMsgs.Add(Messages.NEW_ACTIVITY_DATE_LESSER_THAN_LAST_ACTIVITY_DATE);
                }
            }
            else
            {
                ProductionLoggingActivitiesDTO pla = null;
                pla = ProductionLoggingBLL.GetLastActivityForProductionLine(txtLine.Text, newActivityStartDateTime);

                if (pla != null)
                {
                    _lineDate = pla.LineDate;
                    _lineTime = pla.LineTime;
                    if (pla.ActivityType == Constants.ProductionLineActivities.STOP.ToString())
                    {
                        validationMsgs.Add(Messages.CONSECUTIVE_STOP);
                    }
                }
            }

            if (newActivityStartDateTime > currentTime)
            {
                validationMsgs.Add(Messages.NEW_ACTIVITY_DATE_GREATER_THAN_CURRENT_DATE);
            }
            if (ddReason.SelectedIndex == Constants.MINUSONE)
            {
                validationMsgs.Add(Messages.SELECT_A_REASON);
            }
            if (validationMsgs.Count > Constants.ZERO)
            {
                StringBuilder validationslert = new StringBuilder();
                foreach (string msg in validationMsgs)
                {
                    validationslert.Append(msg);
                    validationslert.Append(Environment.NewLine);
                }
                GlobalMessageBox.Show(validationslert.ToString(), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateEditReasonField()
        {
            if (cmbReason.SelectedIndex == Constants.MINUSONE)
            {
                GlobalMessageBox.Show("Please Select Edit Reason", Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// To call exception log method
        /// </summary>
        /// <param name="floorException">FloorException object </param>
        /// <param name="screenName">ScreenName</param>
        /// <param name="UiClassName">UIClassName</param>
        /// <param name="uiControl">UIControl</param>
        /// <param name="parameters">Parameters</param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string uiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, uiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        /// <summary>
        /// Clear form fields
        /// </summary>
        private void ClearForm()
        {
            dtpDate.Value = ServerCurrentDateTime.Date;
            dtpTime.Value = ServerCurrentDateTime.ToLocalTime();
            ddReason.SelectedIndex = Constants.MINUSONE;
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            dtpDate.Focus();
        }

        private bool SetNextActivityDatetime(string _productionLineId)
        {
            bool _result = true;
            ProductionLoggingActivitiesDTO _lastactivity_pla = new ProductionLoggingActivitiesDTO();
            DateTime currentActivityStartDateTime = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, dtpDate.Value.Day, dtpTime.Value.Hour, dtpTime.Value.Minute, dtpTime.Value.Second);
            _lastactivity_pla = ProductionLoggingBLL.GetNextActivityForProductionLine(_productionLineId, currentActivityStartDateTime);
            if (_lastactivity_pla != null)
            {
                next_productionLineId = _lastactivity_pla.Id;
                lastactivity_lineDate = _lastactivity_pla.LineDate;
                lastactivity_lineTime = _lastactivity_pla.LineTime;
                lastactivity_DateTime = new DateTime(lastactivity_lineDate.Year, lastactivity_lineDate.Month, lastactivity_lineDate.Day,
                lastactivity_lineTime.Hours, lastactivity_lineTime.Minutes, lastactivity_lineTime.Seconds);
            }
            else
            {
                _result = false;
            }

            return _result;
        }


        #endregion
    }
}
