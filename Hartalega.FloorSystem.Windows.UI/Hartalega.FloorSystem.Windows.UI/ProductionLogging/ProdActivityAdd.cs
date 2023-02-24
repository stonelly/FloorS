#region using

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
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace Hartalega.FloorSystem.Windows.UI.ProductionLogging
{
    public partial class ProdActivityAdd : FormBase
    {
        #region private Variables

        private static string _screenName = "Add Product Activity";
        private static string _className = "ProdActivityAdd";
        private static int _productionLineActivityId;
        private static bool _isUpdate;
        private DateTime _maxtime;
        private DateTime _editDate;
        private DateTime _ts;

        #endregion

        #region Public Variables
        private string _line { get; set; }
        #endregion

        #region Load Form
        public ProdActivityAdd(string line)
        {
            InitializeComponent();
            _isUpdate = false;
            _maxtime = CommonBLL.GetCurrentDateAndTimeFromServer();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
                _line = line;
                txtLine.Text = _line;
                datepicker.Format = DateTimePickerFormat.Custom;
                datepicker.CustomFormat = Constants.CUSTOM_DATE_FORMAT;
                datepicker.MaxDate = _maxtime;
                BindActivityType();
                txtOperatorId.OperatorId();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ProdActivityAdd", null);
            }
        }

        public ProdActivityAdd(int ProductionLineActivityId)
        {
            InitializeComponent();
            _isUpdate = true;
            _maxtime = CommonBLL.GetCurrentDateAndTimeFromServer();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
                ProductionLineActivityDTO objProductionLineActivityDTO = ProductionLoggingBLL.GetLineActivitiesByID(ProductionLineActivityId);
                _productionLineActivityId = ProductionLineActivityId;
                txtLine.Text = objProductionLineActivityDTO.Line;
                datepicker.Format = DateTimePickerFormat.Custom;
                datepicker.CustomFormat = Constants.CUSTOM_DATE_FORMAT;
                datepicker.MaxDate = _maxtime;
                BindActivityType();
                txtOperatorId.OperatorId();
                txtOperatorId.Text = objProductionLineActivityDTO.ID;
                txtOperatorName.Text = objProductionLineActivityDTO.Name;
                cmbboxActivityType.Text = objProductionLineActivityDTO.ActivityType.Trim();
                txtDetailsofActivities.Text = objProductionLineActivityDTO.ActivityDetails;
                _editDate = Convert.ToDateTime(objProductionLineActivityDTO.Date);
                _ts = Convert.ToDateTime(objProductionLineActivityDTO.Time);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ProdActivityAdd", null);
            }
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProdActivityAdd_Load(object sender, EventArgs e)
        {
            try
            {
                datepicker.Text = _maxtime.ToString();
                timepicker.Text = _maxtime.ToString();
                timepicker.MaxDate = _maxtime;
                if (_isUpdate)
                {
                    datepicker.Value = _editDate;
                    timepicker.Value = _ts;
                    datepicker.Enabled = false;
                    timepicker.Enabled = false;
                    this.Text = "Update Production Activities";
                }
                else
                {
                    datepicker.Enabled = true;
                    timepicker.Enabled = true;
                    this.Text = "Add Production Activities";
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ProdActivityAdd", null);
            }
        }

        #endregion

        #region Event Handlers
        /// <summary>
        /// OperatorId textbox Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperatorId_Leave(object sender, EventArgs e)
        {
            string operatorName = string.Empty;
            if (!string.IsNullOrEmpty(txtOperatorId.Text.Trim()))
            {
                try
                {
                    if (SecurityModuleBLL.ValidateOperatorAccess(txtOperatorId.Text.Trim(), _screenName))
                    {
                        operatorName = CommonBLL.GetOperatorNameQAI((txtOperatorId.Text).Trim());

                        if (!string.IsNullOrEmpty(operatorName))
                        {
                            txtOperatorName.Text = operatorName;
                            txtOperatorName.Enabled = false;
                            txtDetailsofActivities.Focus();
                        }
                        else
                        {
                            txtOperatorName.Text = string.Empty;
                            GlobalMessageBox.Show(Messages.INVALID_OPERATOR_ID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtOperatorId.Text = string.Empty;
                            txtOperatorId.Focus();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.OPERATOR_SCREEN_ACCESS_MSG, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        txtOperatorId.Text = String.Empty;
                        txtOperatorName.Text = String.Empty;
                        txtOperatorId.Focus();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "txtOperatorId_Leave", Convert.ToInt16(txtOperatorId.Text));
                    return;
                }
            }
            else
            {
                txtOperatorName.Text = string.Empty;
                GlobalMessageBox.Show(Messages.INVALID_OPERATOR_ID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                txtOperatorId.Text = string.Empty;
                txtOperatorId.Focus();
            }
        }

        /// <summary>
        /// SAve Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbboxActivityType, "Activity Type", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtOperatorId, "Operator ID", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtDetailsofActivities, "Details of Activities", ValidationType.Required));
            if (ValidateForm())
            {
                SaveProductionActivity();
            }
        }

        /// <summary>
        /// Cancel Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            BindActivityType();
            string result = GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE_QAI, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (result == Constants.YES)
            {
                this.Close();
            }
        }

        #endregion

        #region User Methods
        /// <summary>
        /// Save Production Activity
        /// </summary>
        private void SaveProductionActivity()
        {
            try
            {
                bool _dataSaved = false;
                if (_isUpdate)
                {
                    _dataSaved = ProductionLoggingBLL.UpdateProductionActivity(txtLine.Text.Trim(), datepicker.Value, timepicker.Text.Trim(), Convert.ToString(cmbboxActivityType.SelectedValue), txtOperatorId.Text.Trim(), txtDetailsofActivities.Text.Trim(), WorkStationDTO.GetInstance().WorkStationId, _productionLineActivityId);
                }
                else
                {
                    _dataSaved = ProductionLoggingBLL.SaveProductionActivity(txtLine.Text.Trim(), datepicker.Value, timepicker.Text.Trim(), Convert.ToString(cmbboxActivityType.SelectedValue), txtOperatorId.Text.Trim(), txtDetailsofActivities.Text.Trim(), WorkStationDTO.GetInstance().WorkStationId);
                }

                if (_dataSaved)
                {
                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    this.Close();
                }
                else
                {
                    GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "SaveProductionActivity", txtLine.Text.Trim(), Convert.ToDateTime(datepicker.Text.Trim()), timepicker.Text.Trim(), Convert.ToString(cmbboxActivityType.SelectedValue), txtOperatorId.Text.Trim(), txtDetailsofActivities.Text.Trim(), WorkStationDTO.GetInstance().WorkStationNumber);
                return;
            }
        }

        /// <summary>
        /// Bind Activity Type
        /// </summary>
        private void BindActivityType()
        {
            try
            {
                cmbboxActivityType.BindComboBox(CommonBLL.GetActivityType(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindActivityType", null);
                return;
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
        }

        #endregion

        private void datepicker_ValueChanged(object sender, EventArgs e)
        {
            timepicker.MaxDate = (_maxtime - datepicker.Value).Days > Constants.ZERO ? _maxtime.AddDays(Constants.TWO) : _maxtime;
        }
    }
}
