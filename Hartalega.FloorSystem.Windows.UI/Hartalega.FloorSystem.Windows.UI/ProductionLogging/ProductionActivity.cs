#region using

using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.Common;

#endregion

namespace Hartalega.FloorSystem.Windows.UI.ProductionLogging
{
    public partial class ProductionActivity : FormBase
    {
        #region private Variables

        /// <summary>
        /// Global varibale contains Production line activity list to Export.
        /// </summary>
        private List<ProductionLineActivityDTO> _lstProductionLineActivity;
        private List<ProductionLineActivityExcel> _lstProductionLineActivityExcel;
        private static string _screenName = "Production Logging - ProductionActivity";
        private static string _className = "ProductionActivity";
        private string lineid;
        private bool _FirstLoad = true;
        #endregion

        #region Load Form

        public ProductionActivity()
        {
            InitializeComponent();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ProductionActivity", null);
            }
            toolStrip1.GotFocus += toolStrip1_GotFocus;
            GetLine();
            frmdatepicker.NofutureDateSelection();
            todatepicker.NofutureDateSelection();
            if (dgvProdLine.Rows.Count > 0)
            {
                _FirstLoad = true;
                BindLineActivities(Convert.ToString(dgvProdLine.Rows[0].Cells[0].Value), null, null);
            }
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductionActivity_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// toolStrip1_GotFocus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void toolStrip1_GotFocus(object sender, EventArgs e)
        {
            ToolStrip toolstrip = sender as ToolStrip;

            if (toolstrip != null)
            {
                foreach (ToolStripItem item in toolstrip.Items)
                {
                    if (item.CanSelect)
                    {
                        ToolStripControlHost host = item as ToolStripControlHost;
                        Control control = host != null ? host.Control : null;

                        if (control != null && control.CanSelect)
                        {
                            control.Select();
                        }
                        else
                        {
                            item.Select();
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Grid row click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProdLine_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvProdLine.Columns[0].Index && e.RowIndex != -1)
            {
                dgvProdLine.EndEdit();
                dgvLine.AutoGenerateColumns = false;
                BindLineActivities(Convert.ToString(dgvProdLine.Rows[e.RowIndex].Cells[0].Value), null, null);
            }
        }

        /// <summary>
        /// Toolstrip button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string itemText = e.ClickedItem.Text;
            switch (itemText)
            {
                case "Add":
                    if (dgvProdLine.SelectedRows.Count != 0)
                    {
                        DataGridViewRow row = this.dgvProdLine.SelectedRows[0];
                        var frmActivityAdd = new ProdActivityAdd(Convert.ToString(row.Cells[0].Value));
                        lineid = Convert.ToString(row.Cells[0].Value);
                        frmActivityAdd.ShowDialog();
                        BindLineActivities(lineid, null, null);
                    }
                    break;
                case "Edit":
                    DataGridViewRow row1 = this.dgvLine.SelectedRows[0];
                    DataGridViewRow row2 = this.dgvProdLine.SelectedRows[0];
                    int activityId = Convert.ToInt32(row1.Cells[0].Value);
                    var frmActivityAddnew = new ProdActivityAdd(activityId);
                    lineid = Convert.ToString(row2.Cells[0].Value);
                    frmActivityAddnew.ShowDialog();
                    BindLineActivities(lineid, frmdatepicker.Value, todatepicker.Value);
                    break;
                case "Export":
                    ExportToExcel();
                    break;
                case "Refresh":
                    _FirstLoad = false;
                    btnGo.PerformClick();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Go Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            if (frmdatepicker.Value.Date > todatepicker.Value.Date)
            {
                GlobalMessageBox.Show(Messages.FROM_DATE_GREATERTHAN_TO_DATE, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                frmdatepicker.Focus();
            }
            else
            {
                BindProductionLineSearch();
            }
        }
        /// <summary>
        /// Production line start stop screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lineStartStopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            new ProductionLogging.ProductionLineStartStop(string.Empty).ShowDialog();
        }
        /// <summary>
        /// Open Production activity screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productionActivitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            new ProductionLogging.ProductionActivity().ShowDialog();
        }
        #endregion

        #region User Methods

        /// <summary>
        /// Bind Line Activities based on Seleted line
        /// </summary>
        /// <param name="lineid"></param>
        private void BindLineActivities(string lineid, DateTime? from, DateTime? to)
        {
            try
            {
                _lstProductionLineActivity = ProductionLoggingBLL.GetLineActivities(lineid, frmdatepicker.Value, todatepicker.Value);
                EnableEdit(_lstProductionLineActivity.Count == Constants.ZERO ? false : true);
                dgvLine.DataSource = _lstProductionLineActivity;
                dgvLine.Columns["ActivityDetails"].Visible = false;
                dgvLine.Columns["ProductionLineActivityId"].Visible = false;
                if (_lstProductionLineActivity.Count == 0 && !_FirstLoad)
                {
                    GlobalMessageBox.Show(Messages.NORECORDS_AVAILABLE_LEFT_GRID, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                }
                _FirstLoad = false;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindLineActivities", lineid);
                return;
            }
        }

        /// <summary>
        /// Gets Line list
        /// </summary>
        private void GetLine()
        {
            try
            {
                dgvProdLine.AutoGenerateColumns = false;
                dgvProdLine.DataSource = ProductionLoggingBLL.GetLines(WorkStationDTO.GetInstance().Location);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetLine", WorkStationDTO.GetInstance().Location);
                return;
            }
        }

        /// <summary>
        /// Search Production line Activites 
        /// </summary>
        private void BindProductionLineSearch()
        {
            if (dgvProdLine.SelectedRows.Count != 0)
            {
                DataGridViewRow row = this.dgvProdLine.SelectedRows[0];
                dgvLine.AutoGenerateColumns = false;
                BindLineActivities(Convert.ToString(row.Cells[0].Value), frmdatepicker.Value, todatepicker.Value);
            }
        }

        /// <summary>
        /// Export To Excel
        /// </summary>
        private void ExportToExcel()
        {
            List<string> hearColumns = new List<string>();
            hearColumns.Add("Line");
            hearColumns.Add("Date");
            hearColumns.Add("Time");
            hearColumns.Add("Id");
            hearColumns.Add("Name");
            hearColumns.Add("Activity Type");
            hearColumns.Add("Details of Activities");
            try
            {
                if (_lstProductionLineActivity != null && _lstProductionLineActivity.Count > 0)
                {
                    _lstProductionLineActivityExcel = new List<ProductionLineActivityExcel>();
                    for (int i = 0; i < _lstProductionLineActivity.Count; i++)
                    {
                        ProductionLineActivityExcel prodLineDTO = new ProductionLineActivityExcel();

                        prodLineDTO.Line = _lstProductionLineActivity[i].Line;
                        prodLineDTO.Date = _lstProductionLineActivity[i].Date;
                        prodLineDTO.Time = _lstProductionLineActivity[i].Time;
                        prodLineDTO.ID = _lstProductionLineActivity[i].ID;
                        prodLineDTO.Name = _lstProductionLineActivity[i].Name;
                        prodLineDTO.ActivityType = _lstProductionLineActivity[i].ActivityType;
                        prodLineDTO.ActivityDetails = _lstProductionLineActivity[i].ActivityDetails;

                        _lstProductionLineActivityExcel.Add(prodLineDTO);

                    }
                    CommonBLL.ExportToExcel<ProductionLineActivityExcel>(hearColumns, "Production Line Activity", _lstProductionLineActivityExcel);
                }
                else
                {
                    GlobalMessageBox.Show(Messages.EXPORTTOEXCEL_EXCEPTION_NO_RECORDS_EXPORT, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ExportToExcel", null);
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


        private void EnableEdit(bool enable)
        {
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                if (item.Text == "Edit")
                {
                    item.Enabled = enable;
                }
            }
        }
        #endregion

        private void speedControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            string _setupConfigurationMainMenu = "Line Speed Control";
            Hartalega.FloorSystem.Windows.UI.Tumbling.Login _passwordForm = new Hartalega.FloorSystem.Windows.UI.Tumbling.Login(Constants.Modules.CONFIGURATIONSETUP, _setupConfigurationMainMenu);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                new ProductionLogging.SpeedControl(_passwordForm.Authentication).ShowDialog();
            }
        }

    }
}
