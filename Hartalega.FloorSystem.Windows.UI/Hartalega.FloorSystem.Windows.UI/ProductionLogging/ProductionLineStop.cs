using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.ProductionLogging
{
    public partial class ProductionLineStop : FormBase
    {
        #region Private Class Members
        private int _production_Logging_Time_Configuration = Constants.ZERO;
        private string _productionLineId = string.Empty;
        private ProductionLineDetailsDTO _pld = null;
        private ProductionLineStartStop _plss = null;
        private int _id = Constants.ZERO;
        private DateTime _lineDate = new DateTime();
        private TimeSpan _lineTime = new TimeSpan();
        private const string _screenName = "ProductionLineStop";
        private const string _className = "ProductionLineStop";
        private string _moduleName = string.Empty;
        #endregion

        #region Constructors
        public ProductionLineStop()
        {
            InitializeComponent();           
        }

        public ProductionLineStop(string productionLineId, ProductionLineStartStop plss)
            : this()
        {
            _productionLineId = productionLineId;
            _plss = plss;
            _moduleName = SetUpConfiguration.MainMenu.GetEnumDescription(Constants.ScreenNames.ProductionLogging);
        }

        public ProductionLineStop(string productionLineId, int id, DateTime lineDate, TimeSpan lineTime, ProductionLineStartStop plss)
            : this()
        {
            _productionLineId = productionLineId;
            _id = id;
            _lineDate = lineDate;
            _lineTime = lineTime;
            _plss = plss;
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
                dtpDate.Text = CommonBLL.GetCurrentDateAndTime().ToString();
                dtpTime.Text = CommonBLL.GetCurrentDateAndTime().ToString();
                PopulateFormFields();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, _productionLineId);
                return;
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
            if (ddlDepartment.SelectedValue == null)
            {
                GlobalMessageBox.Show(Messages.REQUIRED_DEPT, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                return;
            }

            bool proceed = false;

            if (!txtRemarks.Enabled)
            {
                proceed = true;
            }
            else if ((txtRemarks.Enabled) && (txtRemarks.TextLength > 0))
            {
                proceed = true;
            }

            if ((ValidateFormFields() && (proceed)))
            {
                
                bool isPLUpdateRequired = false;
                string workStationId = WorkStationDTO.GetInstance().WorkStationId;
                DateTime lastActivityStartDateTime = new DateTime(_lineDate.Year, _lineDate.Month, _lineDate.Day, _lineTime.Hours, _lineTime.Minutes, _lineTime.Seconds);
                string lineId = txtLine.Text;
                DateTime lineDate = dtpDate.Value.Date;
                TimeSpan lineTime = new TimeSpan(dtpTime.Value.Hour, dtpTime.Value.Minute, Constants.ZERO);
                string activityType = txtActivityType.Text;
                int reasonId = Convert.ToInt32(ddReason.SelectedValue);
                DateTime currentActivityStartDateTime = new DateTime(lineDate.Year, lineDate.Month, lineDate.Day, lineTime.Hours, lineTime.Minutes, lineTime.Seconds);
                string duration = Framework.Common.DateFunctions.GetDuration(lastActivityStartDateTime, currentActivityStartDateTime);
                TimeSpan activityTimeDifference = CommonBLL.GetCurrentDateAndTime().Subtract(currentActivityStartDateTime);
                int activityTimeDifferenceInMinutes = (activityTimeDifference.Days * Constants.TWENTYFOUR * Constants.SIXTY) + (activityTimeDifference.Hours * Constants.SIXTY) + (activityTimeDifference.Minutes);
                string remarks = txtRemarks.Text.Trim();

                int speed = ProductionLoggingBLL.GetLineSpeed(lineId);

                if (string.IsNullOrEmpty(_moduleName))
                {
                    isPLUpdateRequired = activityTimeDifferenceInMinutes > Constants.TWOFORTY ? false : true;
                }

                ProductionLoggingActivitiesDTO pla = null;
                pla = ProductionLoggingBLL.GetLastActivityForProductionLine(txtLine.Text, currentActivityStartDateTime);

                if (pla != null)
                {
                    if (pla.ActivityType == Constants.ProductionLineActivities.STOP.ToString())
                    {
                        GlobalMessageBox.Show(Messages.CONSECUTIVE_STOP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        Close();
                    }
                }

                // department
                var department = ddlDepartment.SelectedValue.ToString().Trim(); ;
             
                int noofrows = ProductionLoggingBLL.SaveProductionLoggingActivityStop(lineId, lineDate, lineTime,
                    activityType, reasonId, duration, isPLUpdateRequired, workStationId, department, remarks, speed);
                if (noofrows > Constants.ZERO)
                {
                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    Close();
                    try
                    {
                        _plss.PopulateProductionLoggingActivity();
                    }
                    catch (FloorSystemException fsEX)
                    {
                        ExceptionLogging(fsEX, _screenName, _className, btnSave.Name, null);
                        return;
                    }
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.REQUIRED_DATA, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }    
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
            catch(FloorSystemException fsEX)
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
            catch(FloorSystemException fsEX)
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
            if (ddReason.SelectedIndex==Constants.MINUSONE)
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

       
        #endregion  

    }
}
