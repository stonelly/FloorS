using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Workstation Master Maintenance
    /// File Type: Code file
    /// </summary> 
    public partial class WorkstationMaster : Form
    {
        #region Member Variables
        private string _screenName = "Configuration SetUp - WorkstationMaster";
        private string _className = "WorkstationMaster";
        private List<DropdownDTO> _configurationList;
        private List<DropdownDTO> _areaList;
        private List<DropdownDTO> _locationList;
        private bool _refresh = false;
        private PageOffsetList _pg;
        private DataTable _dtTableDetails;
        private DataGridViewRow _cell = null;
        private string _loggedInUser;
        private string _wsId;
        DataGridViewComboBoxCell _comboCellLoc;
        DataGridViewComboBoxCell _comboCellConf;
        DataGridViewComboBoxCell _comboCellArea;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public WorkstationMaster(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
        }
        #endregion

        #region Event Handlers
        private void WorkstationMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;

        }
        /// <summary>
        /// Event Handler for Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableMaintenance_Load(object sender, EventArgs e)
        {
            bindingSourceTable.CurrentChanged -= new System.EventHandler(bindingSource_CurrentChanged);
            dgTableMaintenance.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
            _refresh = false;
            PopulateGrid();
            _wsId = WorkStationDTO.GetInstance().WorkStationId;
            dgTableMaintenance.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
        }

        /// <summary>
        /// Used for handling the pagination for datagrid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bindingSource_CurrentChanged(object sender, EventArgs e)
        {
            DataTable dtTableDetails = new DataTable();
            dtTableDetails = _dtTableDetails.Clone();
            int offset = (int)bindingSourceTable.Current;
            for (int i = offset; i < offset + _pg.pageSize && i < _pg.TotalRecords; i++)
            {
                DataRow newRow = dtTableDetails.NewRow();
                dtTableDetails.ImportRow(_dtTableDetails.Rows[i]);
            }
            FillGrid(dtTableDetails);
        }

        /// <summary>
        /// Event Handler for datagrid cell content click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgTableMaintenance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > Constants.MINUSONE)
            {
                if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Edit")
                {
                    btnAdd.Enabled = false;
                    dgTableMaintenance.Rows[e.RowIndex].ReadOnly = false;
                    dgTableMaintenance.Columns[e.ColumnIndex].Name = "Update";
                    dgTableMaintenance[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                    _cell = dgTableMaintenance.CurrentRow;
                    dgTableMaintenance[1, e.RowIndex].ReadOnly = true;
                    dgTableMaintenance.Columns[e.ColumnIndex + 1].Name = "Cancel";
                    dgTableMaintenance[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";
                    bindingNavigatorTable.Enabled = false;
                }
                else if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Cancel" && dgTableMaintenance.CurrentRow == _cell)
                {
                    _refresh = true;
                    PopulateGrid();
                    dgTableMaintenance.Columns[e.ColumnIndex].Name = "Delete";
                }
                else if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Delete")
                {
                    if (dgTableMaintenance["WorkstationId", e.RowIndex].Value == null)
                    {
                        dgTableMaintenance.Rows.RemoveAt(0);
                        btnAdd.Enabled = true;
                        bindingNavigatorTable.Enabled = true;
                        dgTableMaintenance.Columns[9].Name = "Edit";
                    }
                    else
                    {
                        if (GlobalMessageBox.Show(Messages.DELETE_WORKSTATION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                        {
                            try
                            {
                                int rowsReturned = MasterTableBLL.DeleteWorkstationRecord(dgTableMaintenance["WorkstationId", e.RowIndex].Value.ToString(), _wsId, _loggedInUser);

                                if (rowsReturned > 0)
                                {
                                    GlobalMessageBox.Show(Messages.DELETE_RECORD, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    _refresh = true;
                                    PopulateGrid();
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                }
                            }
                            catch (FloorSystemException ex)
                            {
                                ExceptionLogging(ex, _screenName, _className, "dgTableMaintenance_CellContentClick", null);
                                return;
                            }
                        }
                    }
                }
                else if ((dgTableMaintenance.Columns[e.ColumnIndex].Name == "Update" && dgTableMaintenance.CurrentRow == _cell) ||
                         dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert" && dgTableMaintenance.CurrentRow == _cell)
                {
                    int workstationId;
                    bool isAdmin;
                    string locationId = string.Empty;
                    string areaId = string.Empty;
                    string configurationId = string.Empty;
                    bool isDuplicate = false;
                    string workStationName = Convert.ToString(dgTableMaintenance["WorkStationName", e.RowIndex].Value);

                    if (dgTableMaintenance["WorkstationId", e.RowIndex].Value == null)
                        workstationId = 0;
                    else
                        workstationId = Convert.ToInt32(dgTableMaintenance["WorkstationId", e.RowIndex].Value);

                    if (dgTableMaintenance["IsAdmin", e.RowIndex].Value == null)
                        isAdmin = false;
                    else
                        isAdmin = Convert.ToBoolean(dgTableMaintenance["IsAdmin", e.RowIndex].Value);

                    if (dgTableMaintenance["cmbLocationId", e.RowIndex].Value == null)
                    {
                        if (dgTableMaintenance["LocationId", e.RowIndex].Value != null)
                            locationId = dgTableMaintenance["LocationId", e.RowIndex].Value.ToString();
                    }
                    else
                        locationId = dgTableMaintenance["cmbLocationId", e.RowIndex].Value.ToString();

                    if (dgTableMaintenance["cmbConfigurationId", e.RowIndex].Value == null)
                    {
                        if (dgTableMaintenance["ConfigurationId", e.RowIndex].Value != null)
                            configurationId = dgTableMaintenance["ConfigurationId", e.RowIndex].Value.ToString();
                    }
                    else
                        configurationId = dgTableMaintenance["cmbConfigurationId", e.RowIndex].Value.ToString();

                    if (dgTableMaintenance["cmbAreaCode", e.RowIndex].Value == null)
                    {
                        if (dgTableMaintenance["AreaId", e.RowIndex].Value != null)
                            areaId = dgTableMaintenance["AreaId", e.RowIndex].Value.ToString();
                    }
                    else
                        areaId = dgTableMaintenance["cmbAreaCode", e.RowIndex].Value.ToString();

                    string validationMessage = ValidateRequiredFields(workStationName, locationId, configurationId, areaId);
                    if (validationMessage == Messages.REQUIREDFIELDMESSAGE)
                    {
                        try
                        {
                            if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert")
                                isDuplicate = Convert.ToBoolean(MasterTableBLL.IsWorkstationDuplicate(workStationName));
                            if (!isDuplicate)
                            {
                                int rowsReturned = MasterTableBLL.UpdateWorkstationDetails(workstationId, workStationName,
                                                                             locationId, configurationId, areaId, isAdmin, _loggedInUser, _wsId);
                                if (rowsReturned > 0)
                                {
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    _refresh = true;
                                    PopulateGrid();
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DUPLICATE_WORKSTATION, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                        }
                        catch (FloorSystemException ex)
                        {
                            ExceptionLogging(ex, _screenName, _className, "dgTableMaintenance_CellContentClick", null);
                            return;
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(validationMessage, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    }
                }
            }
        }

        /// <summary>
        /// Event Handler for Add button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            bindingNavigatorTable.Enabled = false;
            dgTableMaintenance.Rows.Insert(0, 1);
            PopulateComboBoxes(Constants.ZERO);
            dgTableMaintenance["WorkStationName", 0].ReadOnly = false;
            dgTableMaintenance[9, 0].Style.NullValue = "Insert";
            dgTableMaintenance.Columns[9].Name = "Insert";
            _cell = dgTableMaintenance.Rows[0];
            dgTableMaintenance.Columns[10].Name = "Cancel";
            dgTableMaintenance[10, 0].Style.NullValue = "Cancel";
        }
        /// <summary>
        /// To close form when Esc key is pressed
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                (this.MdiParent as MasterTableMainMenu).cmbTableList.SelectedIndex = Constants.ZERO;
                this.Close();
            }
            bool res = base.ProcessCmdKey(ref msg, keyData);
            return res;
        }

        /// <summary>
        /// Event Handler to open combobox on first click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgTableMaintenance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool validRow = (e.RowIndex != -1 && dgTableMaintenance.Rows[e.RowIndex].ReadOnly == false); //Make sure the clicked row isn't the header.
            var datagridview = sender as DataGridView;
            //commented by MYAdamas 20171011 index out of range on second time select cell
            // Check to make sure the cell clicked is the cell containing the combobox 
            //if (datagridview.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && validRow)
            //{
            //    datagridview.BeginEdit(true);
            //    ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            //}
        }

        private void dgTableMaintenance_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn newColumn = dgTableMaintenance.Columns[e.ColumnIndex];
            DataGridViewColumn oldColumn = dgTableMaintenance.SortedColumn;
            ListSortDirection direction;

            if (newColumn.SortMode == DataGridViewColumnSortMode.Programmatic)
            {
                // If oldColumn is null, then the DataGridView is not sorted.
                if (oldColumn != null)
                {
                    // Sort the same column again, reversing the SortOrder.
                    if (oldColumn == newColumn &&
                        dgTableMaintenance.SortOrder == SortOrder.Ascending)
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        // Sort a new column and remove the old SortGlyph.
                        direction = ListSortDirection.Ascending;
                        oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                    }
                }
                else
                {
                    direction = ListSortDirection.Ascending;
                }

                dgTableMaintenance.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection = direction == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;

                // Sort the selected column.
                DataView dv = _dtTableDetails.DefaultView;
                dv.Sort = newColumn.Name + (direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                DataTable sortedDT = dv.ToTable();
                _dtTableDetails = sortedDT;
                FillGrid(_dtTableDetails);

                _pg = new PageOffsetList(Constants.TWENTY, _dtTableDetails.Rows.Count);
                bindingNavigatorTable.BindingSource = bindingSourceTable;
                bindingSourceTable.DataSource = _pg.GetList();

                dgTableMaintenance.Columns[9].Name = "Edit";
                dgTableMaintenance.Columns[10].Name = "Delete";
            }            
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
        /// Validation of required fields
        /// </summary>
        private string ValidateRequiredFields(string workStationName, string location, string configuration, string areaCode)
        {
            StringBuilder requiredFieldMessage = new StringBuilder(Messages.REQUIREDFIELDMESSAGE);
            if (string.IsNullOrEmpty(workStationName.Trim()))
                requiredFieldMessage.AppendLine("Workstation Name");
            if (string.IsNullOrEmpty(location))
                requiredFieldMessage.AppendLine("Location");
            if (string.IsNullOrEmpty(configuration))
                requiredFieldMessage.AppendLine("Configuration");
            if (string.IsNullOrEmpty(areaCode))
                requiredFieldMessage.AppendLine("Area Code");
            return requiredFieldMessage.ToString();
        }

        /// <summary>
        /// Populate grid
        /// </summary>
        private void PopulateGrid()
        {
            try
            {
                _configurationList = MasterTableBLL.GetConfigurationList();
                _areaList = MasterTableBLL.GetAreaList();
                _locationList = MasterTableBLL.GetLocationList();
                _dtTableDetails = MasterTableBLL.GetWorkstationDetails();
                FillGrid(_dtTableDetails);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PopulateGrid", null);
                return;
            }
            _pg = new PageOffsetList(Constants.TWENTY, _dtTableDetails.Rows.Count);
            bindingNavigatorTable.BindingSource = bindingSourceTable;
            bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
            bindingSourceTable.DataSource = _pg.GetList();

            if (_refresh)
            {
                dgTableMaintenance.Columns[9].Name = "Edit";
                dgTableMaintenance.Columns[10].Name = "Delete";
            }
        }

        /// <summary>
        /// Populate comboboxes
        /// </summary>
        private void PopulateComboBoxes(int rowIndex)
        {
            try
            {
                _comboCellLoc = dgTableMaintenance["cmbLocationId", rowIndex] as DataGridViewComboBoxCell;
                _comboCellLoc.DataSource = new BindingSource(_locationList, null);
                _comboCellLoc.ValueMember = "IDField";
                _comboCellLoc.DisplayMember = "DisplayField";
                _comboCellConf = dgTableMaintenance["cmbConfigurationId", rowIndex] as DataGridViewComboBoxCell;
                _comboCellConf.DataSource = new BindingSource(_configurationList, null);
                _comboCellConf.ValueMember = "IDField";
                _comboCellConf.DisplayMember = "DisplayField";
                _comboCellArea = dgTableMaintenance["cmbAreaCode", rowIndex] as DataGridViewComboBoxCell;
                _comboCellArea.DataSource = new BindingSource(_areaList, null);
                _comboCellArea.ValueMember = "IDField";
                _comboCellArea.DisplayMember = "DisplayField";
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PopulateComboBoxes", null);
                return;
            }
        }

        /// <summary>
        /// Fill Grid
        /// </summary>
        private void FillGrid(DataTable dtTable)
        {
            btnAdd.Enabled = true;
            bindingNavigatorTable.Enabled = true;
            dgTableMaintenance.Rows.Clear();
            if (dtTable != null)
            {
                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    dgTableMaintenance.Rows.Add();
                    dgTableMaintenance.Rows[i].ReadOnly = true;

                    dgTableMaintenance["WorkStationId", i].Value = dtTable.Rows[i]["WorkstationId"];
                    dgTableMaintenance["WorkStationName", i].Value = dtTable.Rows[i]["WorkstationName"];
                    PopulateComboBoxes(i);
                    _comboCellLoc.Style.NullValue = dtTable.Rows[i]["LocationName"].ToString();
                    _comboCellConf.Style.NullValue = dtTable.Rows[i]["ConfigurationName"].ToString();
                    _comboCellArea.Style.NullValue = dtTable.Rows[i]["AreaCode"].ToString();
                    dgTableMaintenance["IsAdmin", i].Value = dtTable.Rows[i]["IsAdmin"];
                    dgTableMaintenance["LocationId", i].Value = dtTable.Rows[i]["LocationId"].ToString();
                    dgTableMaintenance["AreaId", i].Value = dtTable.Rows[i]["AreaId"].ToString();
                    dgTableMaintenance["ConfigurationId", i].Value = dtTable.Rows[i]["ConfigurationId"].ToString();
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                dgTableMaintenance.DataSource = null;
            }
        }
        #endregion       
    }
}
