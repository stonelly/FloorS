using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Properties;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Hartalega.FloorSystem.Windows.UI.ProductionLogging
{
    public partial class ProductionLineStartStop : FormBase
    {
        #region Class Members
        private const string _screenName = "ProductionLineStartStop";
        private const string _className = "ProductionLineStartStop";
        private const string _screenNameLineControlMaintainence = "Line Control Maintainence";
        private string _lineId = string.Empty;
        private int _former = Constants.ZERO;
        private int _speed = Constants.ZERO;
        private decimal _cycle = Constants.ZERO;
        private string _moduleName;
        private string _loggedinuser = string.Empty;
        private bool _promptlogin = false;
        private object dataGridView1;
        private const string _editscreenName = "Production Line Start Stop";
        private const string _editscreenStartStop = "Edit Production Line Start Stop";
        private const string _admineditscreenStartStop = "Admin Edit Production Line Start Stop";
        #endregion

        #region Constructors
        public ProductionLineStartStop(string moduleName)
        {
            InitializeComponent();
            _moduleName = moduleName;
            try
            {
                ClearForm();
                PopulateProductionLineGrid();
                if (!string.IsNullOrEmpty(_moduleName))
                {
                    productionActivitiesToolStripMenuItem.Enabled = false;
                    Show_DeleteAction();
                    tsbStop.Visible = true;
                    tsbAdd.Image = Resources.start;
                    tsbAdd.ToolTipText = Constants.START_ACTIVITY;
                    tsbProdEdit.Enabled = false;
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.Name, this.Text);
                return;
            }
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Clear form fields and load data in master grid on form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductionLineStartStop_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// List details in activity grid on clicking GO button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fromLineDate = dtpFromDate.Value;
                DateTime toLineDate = dtpToDate.Value;
                if (ValidateParameters(fromLineDate, toLineDate))
                {
                    PopulateProductionLoggingActivity();
                    if (grdProductionLineActivity.Rows.Count == Constants.ZERO)
                    {
                        GlobalMessageBox.Show(Messages.NORECORDS_AVAILABLE_LEFT_GRID, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    }
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, btnGo.Name, Text);
                return;
            }
        }

        /// <summary>
        /// Open LineControlMaintainence screen for editing production line properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbProdEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdProductionLine.SelectedRows.Count > Constants.ZERO)
                {
                    Login _passwordForm = new Login(Constants.Modules.PRODUCTIONLOGGING, _screenNameLineControlMaintainence);
                    _passwordForm.ShowDialog();
                    if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
                    {
                        _lineId = grdProductionLine.SelectedRows[Constants.ZERO].Cells[Constants.ZERO].Value.ToString();
                        _former = int.Parse(grdProductionLine.SelectedRows[Constants.ZERO].Cells[Constants.ONE].Value.ToString(), NumberStyles.AllowThousands);
                        _speed = int.Parse(grdProductionLine.SelectedRows[Constants.ZERO].Cells[Constants.TWO].Value.ToString(), NumberStyles.AllowThousands);
                        _cycle = Convert.ToDecimal(grdProductionLine.SelectedRows[Constants.ZERO].Cells[Constants.THREE].Value);
                        new ProductionLogging.LineControlMaintainence(_lineId, _former, _speed, _cycle, this).ShowDialog();
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.RECORD_NOT_SELECTED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, tsbProdEdit.Name, this.Text);
                return;
            }
        }

        /// <summary>
        /// Open Start /Stop activity screen for the production line selected by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbAdd_Click(object sender, EventArgs e)
        {
            Constants.ProductionLineActivities activity;
            try
            {
                ProductionLoggingActivitiesDTO pla = null;
                string productionLineId = string.Empty;
                string lastActivityType = string.Empty;
                if (grdProductionLine.SelectedRows.Count > Constants.ZERO)
                {
                    productionLineId = grdProductionLine.SelectedRows[Constants.ZERO].Cells["colPdlLine"].Value.ToString();

                    if (!string.IsNullOrEmpty(_moduleName))
                    {
                        new ProductionLogging.ProductionLineStart(productionLineId, this).ShowDialog();
                    }
                    else
                    {
                        pla = ProductionLoggingBLL.GetLastActivityForProductionLine(productionLineId, null);
                        if (pla != null)
                        {
                            activity = (Constants.ProductionLineActivities)Enum.Parse(typeof(Constants.ProductionLineActivities), pla.ActivityType);
                            switch (activity)
                            {
                                case Constants.ProductionLineActivities.START:
                                    new ProductionLogging.ProductionLineStop(productionLineId, pla.Id, pla.LineDate, pla.LineTime, this).ShowDialog();
                                    break;
                                case Constants.ProductionLineActivities.STOP:
                                    new ProductionLogging.ProductionLineStart(productionLineId, pla.Id, pla.LineDate, pla.LineTime, this).ShowDialog();
                                    break;
                            }
                        }
                        else
                        {
                            new ProductionLogging.ProductionLineStart(productionLineId, this).ShowDialog();
                        }
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.RECORD_NOT_SELECTED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, tsbAdd.Name, this.Text);
                return;
            }
        }

        /// <summary>
        /// Open Stop activity screen for the production line selected by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbStop_Click(object sender, EventArgs e)
        {
            try
            {
                ProductionLoggingActivitiesDTO pla = null;
                string productionLineId = string.Empty;
                if (grdProductionLine.SelectedRows.Count > Constants.ZERO)
                {
                    productionLineId = grdProductionLine.SelectedRows[Constants.ZERO].Cells["colPdlLine"].Value.ToString();
                   
                    pla = ProductionLoggingBLL.GetLastActivityForProductionLine(productionLineId, null);
                    if (pla != null)
                    {
                        new ProductionLogging.ProductionLineStop(productionLineId, this).ShowDialog();
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.NON_START_ACTIVITY, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        Close();
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.RECORD_NOT_SELECTED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, tsbStop.Name, this.Text);
                return;
            }
        }
        /// <summary>
        /// Handle Production line selection change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdProductionLine_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdProductionLine.SelectedRows.Count > Constants.ZERO)
                {
                    PopulateProductionLoggingActivity();
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, btnGo.Name, this.Text);
                return;
            }
        }
        /// <summary>
        /// Open Production activity screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productionActivitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
            new ProductionLogging.ProductionActivity().ShowDialog();
        }
        /// <summary>
        /// Production line start stop screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lineStartStopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
            if (string.IsNullOrEmpty(_moduleName))
            {
                new ProductionLogging.ProductionLineStartStop(string.Empty).ShowDialog();
            }
            else
            {
                new ProductionLogging.ProductionLineStartStop(_moduleName).ShowDialog();
            }
        }

        /// <summary>
        /// Form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductionLineStartStop_FormClosing(object sender, FormClosingEventArgs e)
        {
            IdentifyIncompleteActivities();
        }
        #endregion

        #region User Methods
        /// <summary>
        /// Clear data from all the controls of form
        /// </summary>
        private void ClearForm()
        {
            grdProductionLine.Rows.Clear();
            grdProductionLineActivity.Rows.Clear();
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
        }

        /// <summary>
        /// Populate data in the production line grid
        /// </summary>
        public void PopulateProductionLineGrid()
        {
            DateTime currentDate = CommonBLL.GetCurrentDateAndTime();
            List<ProductionLineDetailsDTO> productionLineDetailsList = null;
            int locationId = Constants.ZERO;
            grdProductionLine.Rows.Clear();
            grdProductionLineActivity.Rows.Clear();
            dtpFromDate.MaxDate = currentDate;
            dtpToDate.MaxDate = currentDate;
            locationId = WorkStationDTO.GetInstance().LocationId;
            productionLineDetailsList = ProductionLoggingBLL.GetProductionLineDetails(locationId);
            if (productionLineDetailsList != null)
            {
                for (int i = Constants.ZERO; i < productionLineDetailsList.Count; i++)
                {
                    grdProductionLine.Rows.Add();
                    grdProductionLine[Constants.ZERO, i].Value = productionLineDetailsList[i].LineId;
                    grdProductionLine[Constants.ONE, i].Value = productionLineDetailsList[i].Formers;
                    grdProductionLine[Constants.TWO, i].Value = productionLineDetailsList[i].Speed;
                    grdProductionLine[Constants.THREE, i].Value = productionLineDetailsList[i].Cycle;
                }
                grdProductionLine.ClearSelection();
            }
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
        }

        /// <summary>
        /// Populate production logging activity grid based on ProductionLineId, fromLinedate and toLineDate
        /// </summary>
        /// <param name="productionLineId">productionLineId</param>
        /// <param name="fromLineDate">fromLineDate</param>
        /// <param name="toLineDate">toLineDate</param>
        public void PopulateProductionLoggingActivity()
        {
            DateTime fromLineDate = dtpFromDate.Value;
            DateTime toLineDate = dtpToDate.Value;
            string productionLineId = Convert.ToString(grdProductionLine.SelectedRows[Constants.ZERO].Cells[Constants.ZERO].Value);
            string gloveSelected = string.Empty;
            List<ProductionLoggingActivitiesDTO> productionLoggingActivityList = null;

            if (!string.IsNullOrEmpty(productionLineId))
            {
                grdProductionLineActivity.Rows.Clear();
                productionLoggingActivityList = ProductionLoggingBLL.GetProductionLoggingActivity(productionLineId, fromLineDate, toLineDate);
                if (productionLoggingActivityList != null)
                {
                    for (int i = Constants.ZERO; i < productionLoggingActivityList.Count; i++)
                    {
                        StringBuilder glovesToPrint = new StringBuilder();
                        gloveSelected = string.Empty;
                        grdProductionLineActivity.Rows.Add();
                        grdProductionLineActivity.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
                        grdProductionLineActivity.DefaultCellStyle.WrapMode = DataGridViewTriState.True;                     
                        grdProductionLineActivity[Constants.ZERO, i].Value = productionLoggingActivityList[i].LineDate.Date.ToString(ConfigurationManager.AppSettings["smallDateFormat"]);
                        grdProductionLineActivity[Constants.ONE, i].Value = productionLoggingActivityList[i].LineTime;
                        grdProductionLineActivity[Constants.TWO, i].Value = productionLoggingActivityList[i].ActivityType;
                        gloveSelected = productionLoggingActivityList[i].Glove;
                        string[] gloves = gloveSelected.Split(new char[] { ',' });
                        foreach (string glove in gloves)
                        {
                            glovesToPrint.Append(glove);
                            glovesToPrint.Append(Environment.NewLine);
                        }
                        grdProductionLineActivity[Constants.THREE, i].Value = glovesToPrint;
                        grdProductionLineActivity.Columns[Constants.THREE].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        grdProductionLineActivity[Constants.FOUR, i].Value = productionLoggingActivityList[i].ReasonText;
                        grdProductionLineActivity[Constants.FIVE, i].Value = productionLoggingActivityList[i].Duration;
                        grdProductionLineActivity[Constants.SIX, i].Value = productionLoggingActivityList[i].LastModifiedOn;
                        grdProductionLineActivity[Constants.SEVEN, i].Value = productionLoggingActivityList[i].Remarks;
                        grdProductionLineActivity[Constants.EIGHT, i].Value = productionLoggingActivityList[i].IsBatchInsert.ToString();
                        grdProductionLineActivity[Constants.ELEVEN, i].Value = productionLoggingActivityList[i].Id.ToString();
                        grdProductionLineActivity[Constants.TWELVE, i].Value = productionLoggingActivityList[i].ReasonStartStopId.ToString();
                        grdProductionLineActivity[Constants.THIRTEEN, i].Value = productionLoggingActivityList[i].ReasonTypeId.ToString();
                        //  grdProductionLineActivity.Sort(grdProductionLineActivity.Columns["LastModifiedOn"], System.ComponentModel.ListSortDirection.Ascending);

                        grdProductionLineActivity.Rows[i].Cells["Department"].Value = productionLoggingActivityList[i].Department;
                    }
                    grdProductionLineActivity.ClearSelection();
                    CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();

                    IdentifyIncompleteActivities();

                    foreach (DataGridViewRow row in grdProductionLineActivity.Rows)
                    {
                        string RowType = row.Cells["IsBatchInsert"].Value.ToString();

                        if (RowType == "True")
                        {
                            row.DefaultCellStyle.ForeColor = Color.Red;
                            ((DataGridViewDisableButtonCell)row.Cells["btnaction"]).Enabled = false;
                            ((DataGridViewDisableButtonCell)row.Cells["Delete"]).Enabled = false;
                        }
                    }
                    grdProductionLineActivity.Refresh();
                }
            }
        }

        private void IdentifyIncompleteActivities()
        {
            DateTime fromLineDate = dtpFromDate.Value;
            DateTime toLineDate = dtpToDate.Value;
            string productionLineId = string.Empty;
            try
            {
                productionLineId = Convert.ToString(grdProductionLine.SelectedRows[Constants.ZERO].Cells[Constants.ZERO].Value);
            }
            catch { return; }
            string gloveSelected = string.Empty;
            List<ProductionLoggingActivitiesDTO> productionLoggingActivityList = null;
            try
            { 
                if (!string.IsNullOrEmpty(productionLineId))
                {
                    productionLoggingActivityList = ProductionLoggingBLL.GetProductionLoggingActivity(productionLineId, fromLineDate, toLineDate);
                    if (productionLoggingActivityList != null)
                    {
                        for (int j = 1; j < productionLoggingActivityList.Count; j++)
                        {
                            if (string.Compare(productionLoggingActivityList[j].ActivityType,
                                productionLoggingActivityList[j - 1].ActivityType) == Constants.ZERO)
                            {
                                if (string.Compare(productionLoggingActivityList[j].ActivityType,
                                    Constants.ProductionLineActivities.START.ToString()) == Constants.ZERO)
                                {
                                    GlobalMessageBox.Show(Messages.ORPHAN_START, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    break;
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.ORPHAN_STOP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, null, productionLineId);
                return;
            }
        }

        /// <summary>
        /// To call exception log method
        /// </summary>
        /// <param name="floorException">FloorException object </param>
        /// <param name="screenName">ScreenName</param>
        /// <param name="uiClassName">UIClassName</param>
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
        /// Validate parameters for production logging activity
        /// </summary>
        /// <param name="fromLineDate"></param>
        /// <param name="toLineDate"></param>
        /// <returns></returns>
        private bool ValidateParameters(DateTime fromLineDate, DateTime toLineDate)
        {
            if (grdProductionLine.SelectedRows.Count == Constants.ZERO)
            {
                GlobalMessageBox.Show(Messages.RECORD_NOT_SELECTED, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                return false;
            }
            if (fromLineDate.Date > toLineDate.Date)
            {
                GlobalMessageBox.Show(Messages.FROM_DATE_GREATERTHAN_TO_DATE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion 

        private void speedControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            string _setupConfigurationMainMenu = "Line Speed Control";
            Login _passwordForm = new Login(Constants.Modules.CONFIGURATIONSETUP, _setupConfigurationMainMenu);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                new ProductionLogging.SpeedControl(_passwordForm.Authentication).ShowDialog();
            }
        }

        private void grdProductionLineActivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 &&
                (grdProductionLineActivity.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Edit" ||
                 grdProductionLineActivity.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Delete"))
            {
                DataGridViewDisableButtonCell cell = (DataGridViewDisableButtonCell)grdProductionLineActivity.Rows[e.RowIndex].Cells[e.ColumnIndex];
                //DataGridViewButtonCell cell = (DataGridViewButtonCell)grdProductionLineActivity.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Enabled)
                {
                    DataGridViewTextBoxCell pla_id = (DataGridViewTextBoxCell)grdProductionLineActivity.Rows[e.RowIndex].Cells["colPdlaId"];
                    DataGridViewTextBoxCell pla_reasontypeid = (DataGridViewTextBoxCell)grdProductionLineActivity.Rows[e.RowIndex].Cells["ReasonTypeId"];
                    string ScreeName = string.Empty;
                    if (_moduleName == "")
                    {
                        ScreeName = _editscreenStartStop;
                    }
                    else
                    {
                        ScreeName = _admineditscreenStartStop;
                    }
                    if (CheckLogin(ScreeName))
                    {
                        Constants.ProductionLineActivities activity; ProductionLoggingActivitiesDTO pla = null;

                        if (cell.Value.ToString() == "Edit")
                        {

                            ProductionLoggingActivitiesDTO old_plaReasonStartStopId = new ProductionLoggingActivitiesDTO();
                            string productionLineId = string.Empty;
                            string colPdlaActivityType = grdProductionLineActivity.SelectedRows[Constants.ZERO].Cells["colPdlaActivityType"].Value.ToString();
                            try
                            {

                                productionLineId = grdProductionLine.SelectedRows[Constants.ZERO].Cells["colPdlLine"].Value.ToString();
                                pla = ProductionLoggingBLL.GetLastActivityForProductionLine_Edit(int.Parse(pla_id.Value.ToString()), null);
                                old_plaReasonStartStopId.ProductionLineId = pla.ProductionLineId;
                                old_plaReasonStartStopId.LineTime = pla.LineTime;
                                old_plaReasonStartStopId.ReasonStartStopId = pla.ReasonStartStopId;
                                old_plaReasonStartStopId.ReasonTypeId = pla.ReasonTypeId;

                                var department = (string) grdProductionLineActivity.Rows[e.RowIndex].Cells["Department"]?.Value;

                                if (ValidateLine24Hours(pla.LineDate, pla.LineTime))
                                {
                                    if (pla != null)
                                    {
                                        activity = (Constants.ProductionLineActivities)Enum.Parse(typeof(Constants.ProductionLineActivities), colPdlaActivityType);
                                        switch (activity)
                                        {
                                            case Constants.ProductionLineActivities.START:
                                                new ProductionLogging.EditProductionLineStart(productionLineId, int.Parse(pla_id.Value.ToString()),
                                                    pla.LineDate, pla.LineTime, this, old_plaReasonStartStopId, int.Parse(pla_reasontypeid.Value.ToString()), _loggedinuser).ShowDialog();
                                                break;
                                            case Constants.ProductionLineActivities.STOP:
                                                new ProductionLogging.EditProductionLineStop(productionLineId, int.Parse(pla_id.Value.ToString()), pla.LineDate, pla.LineTime,
                                                    this, old_plaReasonStartStopId, int.Parse(pla_reasontypeid.Value.ToString()), _loggedinuser, department).ShowDialog();
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        new ProductionLogging.ProductionLineStart(productionLineId, this).ShowDialog();
                                    }
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.VALIDATE_24HOURS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                }

                            }
                            catch (FloorSystemException fsEX)
                            {
                                ExceptionLogging(fsEX, _screenName, _className, tsbAdd.Name, this.Text);
                                return;
                            }
                        }
                        if (cell.Value.ToString() == "Delete")
                        {
                            pla = ProductionLoggingBLL.GetLastActivityForProductionLine_Edit(int.Parse(pla_id.Value.ToString()), null);
                            if (DialogResult.Yes == MessageBox.Show("Do you want to delete ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                            {
                                DataGridViewTextBoxCell _id = (DataGridViewTextBoxCell)grdProductionLineActivity.Rows[e.RowIndex].Cells["colPdlaId"];
                                if (Constants.ONE == ProductionLoggingBLL.Delete_ProductionLoggingActivity(int.Parse(_id.Value.ToString())))
                                {
                                    AuditLogDelete(pla);
                                    PopulateProductionLoggingActivity();
                                }
                            }

                        }
                        PopulateProductionLoggingActivity();
                    }
                }
            }

        }

        private void AuditLogDelete(ProductionLoggingActivitiesDTO old_plaDTO)
        {
            ProductionLoggingActivitiesDTO obj_plaDTO = new ProductionLoggingActivitiesDTO();
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.SourceTable = "ProductionLoggingActivity";
            AuditLog.CreatedBy = _loggedinuser;
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            string refid  = old_plaDTO.Id.ToString();
            AuditLog.ReferenceId = refid;
            AuditLog.FunctionName = "Delete-ProductionLoggingActivity";
            AuditLog.AuditAction = Convert.ToInt32(Constants.ActionLog.Delete);
            AuditLog.UpdateColumns = old_plaDTO.DetailedCompare(obj_plaDTO).GetPropChanges();
            string audlog = CommonBLL.SerializeTOXML(AuditLog);
            int rowsReturned = ProductionLoggingBLL.UpdateProductionLoggingActivityStart_Audit(audlog);
        }

        private bool CheckLogin(string _screenname)
        {
            Login _passwordForm = new Login(Constants.Modules.PRODUCTIONLOGGING, _screenname);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                _loggedinuser = _passwordForm.Authentication;
                _promptlogin = true;
            }
            else
            {
                _promptlogin = false;
            }

            return _promptlogin;
        }

        /// <summary>
        /// Validate parameters for production logging activity
        /// </summary>
        /// <param name="LineDateTIme"></param>
        /// <returns></returns>
        private bool ValidateLine24Hours(DateTime LineDate, TimeSpan LineTime) //DateTime LineDateTime
        {
            bool _edit = true;

            DateTime LineDateTime = new DateTime(LineDate.Year, LineDate.Month, LineDate.Day, LineTime.Hours, LineTime.Minutes, LineTime.Seconds);

            if (string.IsNullOrEmpty(_moduleName))
            {
                if (DateTime.Now.Subtract(LineDateTime).TotalHours < 24)
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

        private void Show_DeleteAction()
        {
            this.grdProductionLineActivity.Columns["Delete"].Visible = true;
        }
    }


    #region DisableActionButton
    public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
    {
        public DataGridViewDisableButtonColumn()
        {
            this.CellTemplate = new DataGridViewDisableButtonCell();
        }
    }

    public class DataGridViewDisableButtonCell : DataGridViewButtonCell
    {
        private bool enabledValue;
        public bool Enabled
        {
            get
            {
                return enabledValue;
            }
            set
            {
                enabledValue = value;
            }
        }

        // Override the Clone method so that the Enabled property is copied.
        public override object Clone()
        {
            DataGridViewDisableButtonCell cell =
                (DataGridViewDisableButtonCell)base.Clone();
            cell.Enabled = this.Enabled;
            return cell;
        }

        // By default, enable the button cell.
        public DataGridViewDisableButtonCell()
        {
            this.enabledValue = true;
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // The button cell is disabled, so paint the border,  
            // background, and disabled button for the cell.
            if (!this.enabledValue)
            {
                // Draw the cell background, if specified.
                if ((paintParts & DataGridViewPaintParts.Background) ==
                    DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground =
                        new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }

                // Draw the cell borders, if specified.
                if ((paintParts & DataGridViewPaintParts.Border) ==
                    DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                        advancedBorderStyle);
                }

                // Calculate the area in which to draw the button.
                Rectangle buttonArea = cellBounds;
                Rectangle buttonAdjustment =
                    this.BorderWidths(advancedBorderStyle);
                buttonArea.X += buttonAdjustment.X;
                buttonArea.Y += buttonAdjustment.Y;
                buttonArea.Height -= buttonAdjustment.Height;
                buttonArea.Width -= buttonAdjustment.Width;

                // Draw the disabled button.                
                ButtonRenderer.DrawButton(graphics, buttonArea,
                    PushButtonState.Disabled);

                // Draw the disabled button text. 
                if (this.FormattedValue is String)
                {
                    TextRenderer.DrawText(graphics,
                        (string)this.FormattedValue,
                        this.DataGridView.Font,
                        buttonArea, SystemColors.GrayText);
                }
            }
            else
            {
                // The button cell is enabled, so let the base class 
                // handle the painting.
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    elementState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }
    #endregion
}
