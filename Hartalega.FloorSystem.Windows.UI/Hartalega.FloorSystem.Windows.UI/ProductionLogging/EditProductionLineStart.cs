using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Database;
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
    public partial class EditProductionLineStart : FormBase
    {
        #region Private Class Members
        private int _production_Logging_Time_Configuration = Constants.ZERO;
        private string _productionLineId = string.Empty;
        ProductionLineStartStop _plss = null;
        ProductionLineDetailsDTO _pld = null;
        private int _id = Constants.ZERO;
        private DateTime _lineDate = new DateTime();
        private TimeSpan _lineTime = new TimeSpan();
        private List<int> _lbLTSize_Selection = new List<int>();
        private List<int> _lbLBSize_Selection = new List<int>();
        private List<int> _lbRTSize_Selection = new List<int>();
        private List<int> _lbRBSize_Selection = new List<int>();
        private const string _screenName = "ProductionLineStart";
        private const string _className = "ProductionLineStart";
        string[] _ltValues = new string[Constants.TWO];
        string[] _lbValues = new string[Constants.TWO];
        string[] _rtValues = new string[Constants.TWO];
        string[] _rbValues = new string[Constants.TWO];
        private int _reasontypeid = Constants.ZERO;
        private string _moduleName = string.Empty;
        private string _modifiedby = "NA";

        private DateTime lastactivity_lineDate = new DateTime();
        private TimeSpan lastactivity_lineTime = new TimeSpan();
        private DateTime lastactivity_DateTime = new DateTime();

        private ProductionLoggingActivitiesDTO old_plaDTO = null;
        int next_productionLineId = 0;
        #endregion

        #region Constructors
        public EditProductionLineStart()
        {
            InitializeComponent();

        }

        public EditProductionLineStart(string productionLineId, ProductionLineStartStop plss)
            : this()
        {
            _productionLineId = productionLineId;
            _plss = plss;
            _moduleName = SetUpConfiguration.MainMenu.GetEnumDescription(Constants.ScreenNames.ProductionLogging);
        }

        public EditProductionLineStart(string productionLineId, int id, DateTime lineDate, TimeSpan lineTime, 
            ProductionLineStartStop plss, ProductionLoggingActivitiesDTO _plaDTO, int reasontypeid,string _loggedinuser)
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
        }

        private void EditProductionLineStart_Load(object sender, EventArgs e)
        {
            try
            {
                //dtpDate.Text = CommonBLL.GetCurrentDateAndTime().ToString();
                //dtpTime.Text = CommonBLL.GetCurrentDateAndTime().ToString();
                dtpDate.Text = _lineDate.ToString(ConfigurationManager.AppSettings["smallDateFormat"]);
                //dtpDate.Enabled = false;
                dtpTime.Text = _lineTime.ToString();
                PopulateFormFields();
                ddLbGlove.Enabled = ddLtGlove.Enabled = false;
                ddRbGlove.Enabled = ddRtGlove.Enabled = false;
                GetLineSpeed();
                BindReason();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, btnSave.Name, _productionLineId);
                return;
            }
        }

        #endregion

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

        #region User Methods
        /// <summary>
        /// Limit maximum selections in Size list box to 2
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="selection"></param>
        private void TrackSelectionChange(ListBox lb, List<int> selection)
        {
            ListBox.SelectedIndexCollection selectedIndices = lb.SelectedIndices;
            foreach (int index in selectedIndices)
                if (!selection.Contains(index)) selection.Add(index);
            foreach (int index in new List<int>(selection))
                if (!selectedIndices.Contains(index)) selection.Remove(index);
        }
        /// <summary>
        /// Populate fields of the screen for Start activity
        /// </summary>
        private void PopulateFormFields()
        {
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, _productionLineId);
                return;
            }
            _production_Logging_Time_Configuration = Convert.ToInt32(FloorSystemConfiguration.GetInstance().PLTimeConfiguration);
            if (string.IsNullOrEmpty(_moduleName))
            {
                if (_production_Logging_Time_Configuration == 0)
                {
                    GlobalMessageBox.Show(Messages.PRODCUCTION_LINE_TIME_CONFIGURATION_NOT_DONE, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    Close();
                }
            }
            _pld = ProductionLoggingBLL.GetProductionLineForStart(_productionLineId);
            if (_pld.InValid != 0 && string.IsNullOrEmpty(_moduleName))
            {
                GlobalMessageBox.Show(Messages.GLOVETYPE_STOPPED_FOR_THE_LINE, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                this.Close();
            }
            else
            {
                lbLTSize.Items.Clear();
                lbLBSize.Items.Clear();
                lbRTSize.Items.Clear();
                lbRBSize.Items.Clear();
                txtLine.Text = _pld.LineId;
                txtFormers.Text = String.Format(Constants.NUMBER_FORMAT, _pld.Formers);
                txtPcsPerHour.Text = String.Format(Constants.NUMBER_FORMAT, _pld.Speed);
                txtCyclePerHour.Text = String.Format(Constants.NUMBER_FORMAT, _pld.Cycle);
                ddLtGlove.Items.Clear();
                ddLtGlove.Items.Add(_pld.LTGloveType);
                ddLtGlove.Items.Add(_pld.LTAltGlove);
                ddLtGlove.SelectedIndex = Constants.ZERO;
                ddRtGlove.Items.Clear();
                ddRtGlove.Items.Add(_pld.RTGloveType);
                ddRtGlove.Items.Add(_pld.RTAltGlove);
                ddRtGlove.SelectedIndex = Constants.ZERO;
                txtLTSize.Text = _pld.LTGloveSize;
                txtRTSize.Text = _pld.RTGloveSize;
                PopulateLTSizeOptions();
                PopulateRTSizeOptions();
                _ltValues[Constants.ZERO] = txtLTSize.Text;
                _ltValues[Constants.ONE] = string.Empty;
                LoadTextLT();
                _rtValues[Constants.ZERO] = txtRTSize.Text;
                _rtValues[Constants.ONE] = string.Empty;
                LoadTextRT();
                if (_pld.IsDoubleFormer)
                {
                    ddLbGlove.Items.Clear();
                    ddLbGlove.Items.Add(_pld.LBGloveType);
                    ddLbGlove.Items.Add(_pld.LBAltGlove);
                    ddLbGlove.SelectedIndex = Constants.ZERO;
                    ddRbGlove.Items.Clear();
                    ddRbGlove.Items.Add(_pld.RBGloveType);
                    ddRbGlove.Items.Add(_pld.RBAltGlove);
                    ddRbGlove.SelectedIndex = Constants.ZERO;
                    txtLBSize.Text = _pld.LBGloveSize;
                    txtRBSize.Text = _pld.RBGloveSize;
                    PopulateLBSizeOptions();
                    PopulateRBSizeOptions();
                    _lbValues[Constants.ZERO] = txtLBSize.Text;
                    _lbValues[Constants.ONE] = string.Empty;
                    LoadTextLB();
                    _rbValues[Constants.ZERO] = txtRBSize.Text;
                    _rbValues[Constants.ONE] = string.Empty;
                    LoadTextRB();
                }
                else
                {
                    grpLeftBottom.Enabled = false;
                    grpRightBottom.Enabled = false;
                }
            }
        }
        private void PopulateLTSizeOptions()
        {
            string sizeEntered = string.Empty;
            lbLTSize.Items.Clear();
            List<GloveSizeDTO> lstLTGloveSize = ProductionLoggingBLL.GetGloveSizeForGloveType(ddLtGlove.Text, _pld.LineId);
            if (lstLTGloveSize != null && lstLTGloveSize.Count > Constants.ZERO)
            {
                foreach (GloveSizeDTO gloveSize in lstLTGloveSize)
                {
                    lbLTSize.Items.Add(gloveSize.Size);
                }
            }
            sizeEntered = txtLTSize.Text;
            string[] sizes = sizeEntered.Split(new char[] { ',' });
            for (int i = Constants.ZERO; i < lbLTSize.Items.Count; i++)
            {
                foreach (string s in sizes)
                {
                    if (Convert.ToString(lbLTSize.Items[i]).Equals(s))
                    {
                        lbLTSize.SelectedItems.Add(lbLTSize.Items[i]);
                    }
                }
            }
        }
        private void PopulateRTSizeOptions()
        {
            string sizeEntered = string.Empty;
            lbRTSize.Items.Clear();
            List<GloveSizeDTO> lstRTGloveSize = ProductionLoggingBLL.GetGloveSizeForGloveType(ddRtGlove.Text, _pld.LineId);
            if (lstRTGloveSize != null && lstRTGloveSize.Count > Constants.ZERO)
            {
                foreach (GloveSizeDTO gloveSize in lstRTGloveSize)
                {
                    lbRTSize.Items.Add(gloveSize.Size);
                }
            }
            sizeEntered = txtRTSize.Text;
            string[] sizes = sizeEntered.Split(new char[] { ',' });
            for (int i = 0; i < lbRTSize.Items.Count; i++)
            {
                foreach (string s in sizes)
                {
                    if (Convert.ToString(lbRTSize.Items[i]).Equals(s))
                    {
                        lbRTSize.SelectedItems.Add(lbRTSize.Items[i]);
                    }
                }
            }
        }
        private void PopulateLBSizeOptions()
        {
            string sizeEntered = string.Empty;
            lbLBSize.Items.Clear();
            List<GloveSizeDTO> lstLBGloveSize = ProductionLoggingBLL.GetGloveSizeForGloveType(ddLbGlove.Text, _pld.LineId);
            if (lstLBGloveSize != null && lstLBGloveSize.Count > Constants.ZERO)
            {
                foreach (GloveSizeDTO gloveSize in lstLBGloveSize)
                {
                    lbLBSize.Items.Add(gloveSize.Size);
                }
            }
            sizeEntered = txtLBSize.Text;
            string[] sizes = sizeEntered.Split(new char[] { ',' });
            for (int i = 0; i < lbLBSize.Items.Count; i++)
            {
                foreach (string s in sizes)
                {
                    if (Convert.ToString(lbLBSize.Items[i]).Equals(s))
                    {
                        lbLBSize.SelectedItems.Add(lbLBSize.Items[i]);
                    }
                }
            }
        }
        private void PopulateRBSizeOptions()
        {
            string sizeEntered = string.Empty;
            lbRBSize.Items.Clear();
            List<GloveSizeDTO> lstRBGloveSize = ProductionLoggingBLL.GetGloveSizeForGloveType(ddRbGlove.Text, _pld.LineId);
            if (lstRBGloveSize != null && lstRBGloveSize.Count > Constants.ZERO)
            {
                foreach (GloveSizeDTO gloveSize in lstRBGloveSize)
                {
                    lbRBSize.Items.Add(gloveSize.Size);
                }
            }
            sizeEntered = txtRBSize.Text;
            string[] sizes = sizeEntered.Split(new char[] { ',' });
            for (int i = 0; i < lbRBSize.Items.Count; i++)
            {
                foreach (string s in sizes)
                {
                    if (Convert.ToString(lbRBSize.Items[i]).Equals(s))
                    {
                        lbRBSize.SelectedItems.Add(lbRBSize.Items[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Clear data in form fields
        /// </summary>
        private void ClearForm()
        {
            dtpDate.Value = ServerCurrentDateTime;
            dtpTime.Value = ServerCurrentDateTime.ToLocalTime();
            dtpDate.Focus();
            ddLbGlove.SelectedIndex = Constants.ZERO;
            ddLtGlove.SelectedIndex = Constants.ZERO;
            ddRtGlove.SelectedIndex = Constants.ZERO;
            ddRbGlove.SelectedIndex = Constants.ZERO;
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
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
        /// Validate Form Fields
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
                    if (pla.ActivityType == Constants.ProductionLineActivities.START.ToString())
                    {
                        validationMsgs.Add(Messages.CONSECUTIVE_START);
                    }
                }
            }

            if (newActivityStartDateTime > currentTime)
            {
                validationMsgs.Add(Messages.NEW_ACTIVITY_DATE_GREATER_THAN_CURRENT_DATE);
            }
            if (_pld.IsDoubleFormer)
            {
                if (string.IsNullOrEmpty(txtLTSize.Text) || string.IsNullOrEmpty(txtLBSize.Text) || string.IsNullOrEmpty(txtRTSize.Text) || string.IsNullOrEmpty(txtRBSize.Text))
                {
                    validationMsgs.Add(Messages.SELECT_SIZE_FOR_ALL_TIERS);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtLTSize.Text) || string.IsNullOrEmpty(txtRTSize.Text))
                {
                    validationMsgs.Add(Messages.SELECT_SIZE_FOR_ALL_TIERS);
                }
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

        #endregion

        #region Event Handlers
        /// <summary>
        /// Clear form fields on cancel button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CONFIRM_CANCEL_TRANSACTION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                Close();
            }

        }

        /// <summary>
        /// Save details on clicking Save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            ProductionLoggingActivitiesDTO obj_plaDTO = new ProductionLoggingActivitiesDTO();

            int noofrows = Constants.ZERO;
            string lbGloveType = string.Empty;
            string lbAltGlove = string.Empty;
            string lbGloveSize = string.Empty;
            string rbGloveType = string.Empty;
            string rbAltGlove = string.Empty;
            string rbGloveSize = string.Empty;
            string ltGloveType = string.Empty;
            string ltAltGlove = string.Empty;
            string ltGloveSize = string.Empty;
            string rtGloveType = string.Empty;
            string rtAltGlove = string.Empty;
            string rtGloveSize = string.Empty;
            string workStationId = string.Empty;
            string lineId = string.Empty;
            string activityType = string.Empty;           
            if (ValidateEditReasonField())
            {
                obj_plaDTO.ProductionLineId = txtLine.Text;
                int reason_startstopId = Constants.ZERO;
                reason_startstopId = int.Parse(cmbReason.SelectedValue.ToString());
                obj_plaDTO.ReasonStartStopId = reason_startstopId;
                obj_plaDTO.LineTime = new TimeSpan(dtpTime.Value.Hour, dtpTime.Value.Minute, Constants.ZERO);
                DateTime lastActivityStartDateTime = new DateTime();
                DateTime lineDate = new DateTime();
                TimeSpan lineTime = new TimeSpan();
                bool isUpdateNextActivity = false;
                string updateDuration = string.Empty;
                

                workStationId = WorkStationDTO.GetInstance().WorkStationId;
                lastActivityStartDateTime = new DateTime(_lineDate.Year, _lineDate.Month, _lineDate.Day, _lineTime.Hours, _lineTime.Minutes, _lineTime.Seconds);
                lineId = txtLine.Text;
                //ltGloveType = ddLtGlove.SelectedItem.ToString();
                //ltAltGlove = ddLtGlove.SelectedIndex == Constants.ZERO ? ddLtGlove.Items[Constants.ONE].ToString() : ddLtGlove.Items[Constants.ZERO].ToString();
                //ltGloveSize = txtLTSize.Text;
                //rtGloveType = ddRtGlove.SelectedItem.ToString();
                //rtAltGlove = ddRtGlove.SelectedIndex == Constants.ZERO ? ddRtGlove.Items[Constants.ONE].ToString() : ddRtGlove.Items[Constants.ZERO].ToString();
                //rtGloveSize = txtRTSize.Text;
                //2019-05-19 - Unused code cause error
                lineDate = dtpDate.Value.Date;
                lineTime = new TimeSpan(dtpTime.Value.Hour, dtpTime.Value.Minute, Constants.ZERO);
                activityType = txtActivity.Text;

                DateTime currentActivityStartDateTime = new DateTime(lineDate.Year, lineDate.Month, lineDate.Day, lineTime.Hours, lineTime.Minutes, lineTime.Seconds);
                try
                {
                    //lbGloveType = ddLbGlove.SelectedItem.ToString();
                    //lbAltGlove = ddLbGlove.SelectedIndex == Constants.ZERO ? ddLbGlove.Items[Constants.ONE].ToString() : ddLbGlove.Items[Constants.ZERO].ToString();
                    //lbGloveSize = txtLBSize.Text;
                    //rbGloveType = ddRbGlove.SelectedItem.ToString();
                    //rbAltGlove = ddRbGlove.SelectedIndex == Constants.ZERO ? ddRbGlove.Items[Constants.ONE].ToString() : ddRbGlove.Items[Constants.ZERO].ToString();
                    //rbGloveSize = txtRBSize.Text;
                    //2019-05-19 - Unused code cause error

                    //// get downtime duration 
                    DateTime currentActivityStopDateTime = new DateTime(lineDate.Year, lineDate.Month, lineDate.Day, lineTime.Hours, lineTime.Minutes, lineTime.Seconds);
                    SetlastActivityDatetime(_productionLineId);
                    //string dtDuration = Framework.Common.DateFunctions.GetDuration(lastactivity_DateTime, currentActivityStopDateTime);
                    string dtDuration = string.Empty;


                    noofrows = ProductionLoggingBLL.UpdateProductionLoggingActivityStart(lineDate, lineTime, _id, reason_startstopId, "START", dtDuration);
                    
                    if (noofrows > Constants.ZERO)
                    {
                        if (SetNextActivityDatetime(_productionLineId))
                        {
                            string duration = Framework.Common.DateFunctions.GetDuration(currentActivityStartDateTime, lastactivity_DateTime);
                            noofrows = ProductionLoggingBLL.UpdateProductionLoggingActivityStop(lineDate, lineTime, next_productionLineId, 0, 0, duration, "START");
                        }
                        AuditLogDTO AuditLog = new AuditLogDTO();
                        AuditLog.SourceTable = "ProductionLoggingActivity";
                        AuditLog.CreatedBy = _modifiedby;
                        AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;string refid = ""; refid = _id.ToString();
                        AuditLog.ReferenceId = refid;
                        AuditLog.FunctionName = "Edit-ProductionLoggingActivityStart";Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), Constants.Update);
                        AuditLog.AuditAction = Convert.ToInt32(audAction);
                        AuditLog.UpdateColumns = old_plaDTO.DetailedCompare(obj_plaDTO).GetPropChanges();
                        string audlog = CommonBLL.SerializeTOXML(AuditLog);
                        int rowsReturned = ProductionLoggingBLL.UpdateProductionLoggingActivityStart_Audit(audlog);

                        GlobalMessageBox.Show("Successfully Updated", Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            Close();
                        //_plss.PopulateProductionLoggingActivity();
                    }
                }
                catch (FloorSystemException fsEX)
                {
                    ExceptionLogging(fsEX, _screenName, _className, btnSave.Name, _productionLineId);
                    return;
                }
            }
        }

        private void SetlastActivityDatetime(string _productionLineId)
        {
            ProductionLoggingActivitiesDTO _lastactivity_pla = new ProductionLoggingActivitiesDTO();
            DateTime currentActivityStopDateTime = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, dtpDate.Value.Day, dtpTime.Value.Hour, dtpTime.Value.Minute, dtpTime.Value.Second);

            _lastactivity_pla = ProductionLoggingBLL.GetLastActivityForProductionLine(_productionLineId, currentActivityStopDateTime);

            if (_lastactivity_pla.Id == _id)
            {
                currentActivityStopDateTime = new DateTime(_lineDate.Year, _lineDate.Month,
                    _lineDate.Day, _lineTime.Hours, _lineTime.Minutes, _lineTime.Seconds);
                _lastactivity_pla = ProductionLoggingBLL.GetLastActivityForProductionLine(_productionLineId, currentActivityStopDateTime);
            }
            lastactivity_lineDate = _lastactivity_pla.LineDate;
            lastactivity_lineTime = _lastactivity_pla.LineTime;
            lastactivity_DateTime = new DateTime(lastactivity_lineDate.Year, lastactivity_lineDate.Month, lastactivity_lineDate.Day,
            lastactivity_lineTime.Hours, lastactivity_lineTime.Minutes, lastactivity_lineTime.Seconds);
        }

        private void LoadTextLT()
        {
            int selectedIndex = ddLtGlove.SelectedIndex;
            switch (selectedIndex)
            {
                case Constants.ZERO:
                    txtLTSize.Text = _ltValues[Constants.ZERO];
                    break;
                case Constants.ONE:
                    txtLTSize.Text = _ltValues[Constants.ONE];
                    break;
            }
        }
        private void LoadTextLB()
        {
            int selectedIndex = ddLbGlove.SelectedIndex;
            switch (selectedIndex)
            {
                case Constants.ZERO:
                    txtLBSize.Text = _lbValues[Constants.ZERO];
                    break;
                case Constants.ONE:
                    txtLBSize.Text = _lbValues[Constants.ONE];
                    break;
            }
        }
        private void LoadTextRT()
        {
            int selectedIndex = ddRtGlove.SelectedIndex;
            switch (selectedIndex)
            {
                case Constants.ZERO:
                    txtRTSize.Text = _rtValues[Constants.ZERO];
                    break;
                case Constants.ONE:
                    txtRTSize.Text = _rtValues[Constants.ONE];
                    break;
            }
        }
        private void LoadTextRB()
        {
            int selectedIndex = ddRbGlove.SelectedIndex;
            switch (selectedIndex)
            {
                case Constants.ZERO:
                    txtRBSize.Text = _rbValues[Constants.ZERO];
                    break;
                case Constants.ONE:
                    txtRBSize.Text = _rbValues[Constants.ONE];
                    break;
            }
        }
        private void lbLTSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            string itemsSeletedForSize = string.Empty;
            StringBuilder items = new StringBuilder();
            TrackSelectionChange((ListBox)sender, _lbLTSize_Selection);
            if (lbLTSize.SelectedItems.Count > Constants.TWO)
            {
                lbLTSize.SelectedItems.Remove(lbLTSize.Items[_lbLTSize_Selection[_lbLTSize_Selection.Count - Constants.ONE]]);
            }

            for (int i = Constants.ZERO; i < _lbLTSize_Selection.Count; i++)
            {
                items.Append(lbLTSize.Items[_lbLTSize_Selection[i]]);
                items.Append(",");
            }
            itemsSeletedForSize = items.ToString();
            if (!string.IsNullOrEmpty(itemsSeletedForSize))
            {
                txtLTSize.Text = itemsSeletedForSize.Substring(Constants.ZERO, items.Length - Constants.ONE);
            }
            else
            {
                txtLTSize.Text = string.Empty;
            }
        }
        private void lbRTSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            string itemsSeletedForSize = string.Empty;
            StringBuilder items = new StringBuilder();
            TrackSelectionChange((ListBox)sender, _lbRTSize_Selection);
            if (lbRTSize.SelectedItems.Count > Constants.TWO)
            {
                lbRTSize.SelectedItems.Remove(lbRTSize.Items[_lbRTSize_Selection[_lbRTSize_Selection.Count - Constants.ONE]]);
            }

            for (int i = Constants.ZERO; i < _lbRTSize_Selection.Count; i++)
            {
                items.Append(lbRTSize.Items[_lbRTSize_Selection[i]]);
                items.Append(",");
            }
            itemsSeletedForSize = items.ToString();
            if (!string.IsNullOrEmpty(itemsSeletedForSize))
            {
                txtRTSize.Text = itemsSeletedForSize.Substring(Constants.ZERO, items.Length - Constants.ONE);
            }
            else
            {
                txtRTSize.Text = string.Empty;
            }
        }

        private void lbLBSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            string itemsSeletedForSize = string.Empty;
            StringBuilder items = new StringBuilder();
            TrackSelectionChange((ListBox)sender, _lbLBSize_Selection);
            if (lbLBSize.SelectedItems.Count > Constants.TWO)
            {
                lbLBSize.SelectedItems.Remove(lbLBSize.Items[_lbLBSize_Selection[_lbLBSize_Selection.Count - Constants.ONE]]);
            }

            for (int i = Constants.ZERO; i < _lbLBSize_Selection.Count; i++)
            {
                items.Append(lbLBSize.Items[_lbLBSize_Selection[i]]);
                items.Append(",");
            }
            itemsSeletedForSize = items.ToString();
            if (!string.IsNullOrEmpty(itemsSeletedForSize))
            {
                txtLBSize.Text = itemsSeletedForSize.Substring(Constants.ZERO, items.Length - Constants.ONE);
            }
            else
            {
                txtLBSize.Text = string.Empty;
            }
        }

        private void lbRBSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            string itemsSeletedForSize = string.Empty;
            StringBuilder items = new StringBuilder();
            TrackSelectionChange((ListBox)sender, _lbRBSize_Selection);
            if (lbRBSize.SelectedItems.Count > Constants.TWO)
            {
                lbRBSize.SelectedItems.Remove(lbRBSize.Items[_lbRBSize_Selection[_lbRBSize_Selection.Count - Constants.ONE]]);
            }

            for (int i = Constants.ZERO; i < _lbRBSize_Selection.Count; i++)
            {
                items.Append(lbRBSize.Items[_lbRBSize_Selection[i]]);
                items.Append(",");
            }
            itemsSeletedForSize = items.ToString();
            if (!string.IsNullOrEmpty(itemsSeletedForSize))
            {
                txtRBSize.Text = itemsSeletedForSize.Substring(Constants.ZERO, items.Length - Constants.ONE);
            }
            else
            {
                txtRBSize.Text = string.Empty;
            }
        }

        private void ddLtGlove_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTextLT();
            PopulateLTSizeOptions();
        }

        private void ddRtGlove_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTextRT();
            PopulateRTSizeOptions();
        }

        private void ddLbGlove_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTextLB();
            PopulateLBSizeOptions();
        }

        private void ddRbGlove_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTextRB();
            PopulateRBSizeOptions();
        }

        #endregion

        private void txtLTSize_TextChanged(object sender, EventArgs e)
        {
            int selectedIndex = ddLtGlove.SelectedIndex;
            switch (selectedIndex)
            {
                case Constants.ZERO:
                    _ltValues[Constants.ZERO] = txtLTSize.Text;
                    break;
                case Constants.ONE:
                    _ltValues[Constants.ONE] = txtLTSize.Text;
                    break;
            }
        }

        private void txtRTSize_TextChanged(object sender, EventArgs e)
        {
            int selectedIndex = ddRtGlove.SelectedIndex;
            switch (selectedIndex)
            {
                case Constants.ZERO:
                    _rtValues[Constants.ZERO] = txtRTSize.Text;
                    break;
                case Constants.ONE:
                    _rtValues[Constants.ONE] = txtRTSize.Text;
                    break;
            }
        }

        private void txtLBSize_TextChanged(object sender, EventArgs e)
        {
            int selectedIndex = ddLbGlove.SelectedIndex;
            switch (selectedIndex)
            {
                case Constants.ZERO:
                    _lbValues[Constants.ZERO] = txtLBSize.Text;
                    break;
                case Constants.ONE:
                    _lbValues[Constants.ONE] = txtLBSize.Text;
                    break;
            }
        }

        private void txtRBSize_TextChanged(object sender, EventArgs e)
        {
            int selectedIndex = ddRbGlove.SelectedIndex;
            switch (selectedIndex)
            {
                case Constants.ZERO:
                    _rbValues[Constants.ZERO] = txtRBSize.Text;
                    break;
                case Constants.ONE:
                    _rbValues[Constants.ONE] = txtRBSize.Text;
                    break;
            }
        }

        /// <summary>
        /// Get Line Speed
        /// </summary>
        private int GetLineSpeed()
        {
            ProductionLoggingTierDTO dto = new ProductionLoggingTierDTO();
            string dtp = dtpDate.Value.ToString("yyyy-MM-dd") + dtpTime.Value.ToString(" HH:mm:ss");
            dto.LineDate = Convert.ToDateTime(dtp);
            dto.Line = txtLine.Text;
            if (ddLtGlove.Text != string.Empty)
                dto.LTGlove = ddLtGlove.Text;
            if (ddLbGlove.Text != string.Empty)
                dto.LBGlove = ddLbGlove.Text;
            if (ddRtGlove.Text != string.Empty)
                dto.RTGlove = ddRtGlove.Text;
            if (ddRbGlove.Text != string.Empty)
                dto.RBGlove = ddRbGlove.Text;

            dto = ProductionLoggingBLL.LoadGloveTierSpeed(dto);
            if (dto.LowestSpeed == 0)
            {
                txtPcsPerHour.Text = dto.LowestSpeed.ToString();
                //btnSave.Enabled = false;
                GlobalMessageBox.Show(Hartalega.FloorSystem.Framework.Messages.GLOVE_SPEED_NOT_CONFIGURED);
            }
            else
            {
                txtPcsPerHour.Text = dto.LowestSpeed.ToString("###,###");
                //btnSave.Enabled = true;
            }

            if (dto.ConflictSpeed)
                GlobalMessageBox.Show(Hartalega.FloorSystem.Framework.Messages.GLOVE_MIX_SPEED);

            return dto.LowestSpeed;

        }

        /// <summary>
        /// Check Line Speed before saving
        /// </summary>
        private void CheckLineSpeed(Constants.Tier tier)
        {
            string speedStr = txtPcsPerHour.Text.Replace(",", "");
            int speed = Convert.ToInt32(speedStr);
            int gloveSpeed = 0;
            string glovecode = string.Empty;
            string SelTier = string.Empty;

            string dtp = dtpDate.Value.ToString("yyyy-MM-dd") + dtpTime.Value.ToString(" HH:mm:ss");

            if (tier == Constants.Tier.LT)
            {
                gloveSpeed = ProductionLoggingBLL.GetGloveSpeedByLine(ddLtGlove.Text, txtLine.Text, Convert.ToDateTime(dtp));
                glovecode = ddLtGlove.Text;
            }
            else if (tier == Constants.Tier.LB)
            {
                gloveSpeed = ProductionLoggingBLL.GetGloveSpeedByLine(ddLbGlove.Text, txtLine.Text, Convert.ToDateTime(dtp));
                glovecode = ddLbGlove.Text;
            }
            else if (tier == Constants.Tier.RB)
            {
                gloveSpeed = ProductionLoggingBLL.GetGloveSpeedByLine(ddRbGlove.Text, txtLine.Text, Convert.ToDateTime(dtp));
                glovecode = ddRbGlove.Text;
            }

            else if (tier == Constants.Tier.RT)
            {
                gloveSpeed = ProductionLoggingBLL.GetGloveSpeedByLine(ddRtGlove.Text, txtLine.Text, Convert.ToDateTime(dtp));
                glovecode = ddRtGlove.Text;
            }

            if (gloveSpeed == Constants.ZERO)
            {
                string alert = "Please configure glove speed for " + glovecode + " on " + tier.ToString() + " tier";
                GlobalMessageBox.Show(alert);
                //btnSave.Enabled = false;
            }
            else if (gloveSpeed != speed)
            {
                GetLineSpeed();
                //btnSave.Enabled = true;
            }

        }

        private void SaveLineSpeed()
        {
            string speedStr = txtPcsPerHour.Text.Replace(",", "");
            int speed = Convert.ToInt32(speedStr);
            ProductionLoggingBLL.SaveLineSpeed(txtLine.Text, speed);
        }

        private void ddLtGlove_Validated(object sender, EventArgs e)
        {
            //if (ddLtGlove.Text != string.Empty)
            //    CheckLineSpeed(Constants.Tier.LT);
        }

        private void ddRtGlove_Validated(object sender, EventArgs e)
        {
            //if (ddRtGlove.Text != string.Empty)
            //    CheckLineSpeed(Constants.Tier.RT);
        }

        private void ddLbGlove_Validated(object sender, EventArgs e)
        {
            //if (ddLbGlove.Text != string.Empty)
            //   CheckLineSpeed(Constants.Tier.LB);
        }

        private void ddRbGlove_Validated(object sender, EventArgs e)
        {
            //if (ddRbGlove.Text != string.Empty)
            //    CheckLineSpeed(Constants.Tier.RB);
        }

        private void dtpTime_ValueChanged(object sender, EventArgs e)
        {
            //no implementation
        }

        private void dtpTime_Leave(object sender, EventArgs e)
        {
            GetLineSpeed();
        }

        private bool ValidateLine24Hours()
        {
            bool _edit = true;
            if (string.IsNullOrEmpty(_moduleName))
            {
                if (DateTime.Now.Subtract(DateTime.Parse(dtpDate.Text + " " + dtpTime.Text)).TotalHours < 24)
                {
                    _edit = true;
                }
                else
                {
                    _edit = false;
                }
            }
            return _edit;
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            //if (ValidateLine24Hours())
            //{

            //}
            //else
            //{
            //    dtpDate.Text = _lineDate.ToString(ConfigurationManager.AppSettings["smallDateFormat"]);
            //    dtpTime.Text = _lineTime.ToString();
            //}
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
    }
}
