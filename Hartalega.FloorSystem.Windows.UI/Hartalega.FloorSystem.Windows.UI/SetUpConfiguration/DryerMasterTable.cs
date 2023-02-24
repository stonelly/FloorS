using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: DryerMaster Master Maintenance
    /// File Type: Code file
    /// </summary> 
    public partial class DryerMasterTable : Form
    {
        #region Member Variables
        private string _screenName = "Configuration SetUp - DryerMasterTable";
        private string _className = "DryerMasterTable";
        private List<DropdownDTO> _gloveTypeList;
        private List<DropdownDTO> _sizeList;
        private List<DropdownDTO> _locationList;
        private bool _refresh = false;
        private PageOffsetList _pg;
        private DataTable _dtTableDetails;
        private DataGridViewRow _cell = null;
        private string _loggedInUser;
        private string _wsId;
        DataGridViewComboBoxCell _comboCellLoc;
        DataGridViewComboBoxCell _comboCellGloveType;
        DataGridViewComboBoxCell _comboCellSize;
        DryerDTO oldDryerObj = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public DryerMasterTable(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableMaintenance_Load(object sender, EventArgs e)
        {
            dgTableMaintenance.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellValueChanged);
            bindingSourceTable.CurrentChanged -= new System.EventHandler(bindingSource_CurrentChanged);
            dgTableMaintenance.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
            _refresh = false;
            PopulateGrid();
            _wsId = WorkStationDTO.GetInstance().WorkStationId;
            bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
            dgTableMaintenance.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
            dgTableMaintenance.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellValueChanged);
            this.dgTableMaintenance.AllowUserToDeleteRows = false;
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
                bindingSourceTable.CurrentChanged -= new System.EventHandler(bindingSource_CurrentChanged);
                if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Edit")
                {
                    oldDryerObj = new DryerDTO();
                    oldDryerObj.Id = Convert.ToInt32(dgTableMaintenance["DryerId", e.RowIndex].Value);
                    oldDryerObj.DryerNumber = Convert.ToInt32(dgTableMaintenance["DryerNumber", e.RowIndex].Value);
                    oldDryerObj.IsStopped = Convert.ToBoolean(dgTableMaintenance["IsStopped", e.RowIndex].Value);
                    oldDryerObj.Hot = Convert.ToBoolean(dgTableMaintenance["Hot", e.RowIndex].Value);
                    oldDryerObj.Cold = Convert.ToBoolean(dgTableMaintenance["Cold", e.RowIndex].Value);
                    oldDryerObj.HotAndCold = Convert.ToBoolean(dgTableMaintenance["HotAndCold", e.RowIndex].Value);
                    oldDryerObj.CheckGlove = Convert.ToBoolean(dgTableMaintenance["CheckGlove", e.RowIndex].Value);
                    oldDryerObj.CheckSize = Convert.ToBoolean(dgTableMaintenance["CheckSize", e.RowIndex].Value);
                    oldDryerObj.IsScheduledStop = Convert.ToBoolean(dgTableMaintenance["IsScheduledStop", e.RowIndex].Value);
                    oldDryerObj.GloveType = dgTableMaintenance["GloveType", e.RowIndex].Value.ToString();
                    oldDryerObj.GloveSize = dgTableMaintenance["Size", e.RowIndex].Value.ToString();
                    oldDryerObj.LocationId = dgTableMaintenance["LocationId", e.RowIndex].Value.ToString();
                    btnAdd.Enabled = false;
                    bindingNavigatorTable.Enabled = false;
                    dgTableMaintenance.Rows[e.RowIndex].ReadOnly = false;
                    dgTableMaintenance.Columns[e.ColumnIndex].Name = "Update";
                    dgTableMaintenance[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                    _cell = dgTableMaintenance.CurrentRow;
                    dgTableMaintenance[0, e.RowIndex].ReadOnly = true;
                    dgTableMaintenance.Columns[e.ColumnIndex + 1].Name = "Cancel";
                    dgTableMaintenance[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";
                }
                else if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Cancel" && dgTableMaintenance.CurrentRow == _cell)
                {
                    _refresh = true;
                    PopulateGrid();
                    dgTableMaintenance.Columns[e.ColumnIndex].Name = "Delete";
                }
                else if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Delete")
                {
                    if (dgTableMaintenance["DryerNumber", e.RowIndex].Value == null)
                    {
                        dgTableMaintenance.Rows.RemoveAt(0);
                        btnAdd.Enabled = true;
                        bindingNavigatorTable.Enabled = true;
                        dgTableMaintenance.Columns[14].Name = "Edit";
                    }
                    else
                    {
                        if (GlobalMessageBox.Show(Messages.DELETE_DRYER, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                        {
                            try
                            {
                                oldDryerObj = new DryerDTO();
                                oldDryerObj.IsDeleted = false;
                                DryerDTO newDryerObj = new DryerDTO();
                                newDryerObj.IsDeleted = true;
                                //set audit log class
                                AuditLogDTO AuditLog = new AuditLogDTO();
                                AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                                AuditLog.FunctionName = _screenName;
                                AuditLog.CreatedBy = _loggedInUser;
                                Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), "Delete");
                                AuditLog.AuditAction = Convert.ToInt32(audAction);
                                AuditLog.SourceTable = "DryerMaster";
                                string refid = Convert.ToString(dgTableMaintenance["DryerId", e.RowIndex].Value);
                                AuditLog.ReferenceId = refid;
                                AuditLog.UpdateColumns = oldDryerObj.DetailedCompare(newDryerObj).GetPropChanges();

                                string audlog = CommonBLL.SerializeTOXML(AuditLog);


                                int rowsReturned = MasterTableBLL.DeleteDryerMasterRecord(dgTableMaintenance["DryerNumber", e.RowIndex].Value.ToString(), _wsId, _loggedInUser, audlog);
                                if (rowsReturned > 0)
                                {
                                    GlobalMessageBox.Show(Messages.DELETE_RECORD, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    _refresh = true;
                                    PopulateGrid();
                                    oldDryerObj = null;
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
                    int dryerNumber;
                    bool isStopped, hot, cold, hotAndCold, checkGlove, checkSize, isScheduledStop;
                    string locationId = string.Empty;
                    string gloveType = string.Empty;
                    string size = string.Empty;
                    bool isDuplicate = false;

                    if (dgTableMaintenance["DryerNumber", e.RowIndex].Value == null)
                        dryerNumber = 0;
                    else
                        dryerNumber = Convert.ToInt32(dgTableMaintenance["DryerNumber", e.RowIndex].Value);

                    if (dgTableMaintenance["IsStopped", e.RowIndex].Value == null)
                        isStopped = false;
                    else
                        isStopped = Convert.ToBoolean(dgTableMaintenance["IsStopped", e.RowIndex].Value);

                    if (dgTableMaintenance["Hot", e.RowIndex].Value == null)
                        hot = false;
                    else
                        hot = Convert.ToBoolean(dgTableMaintenance["Hot", e.RowIndex].Value);

                    if (dgTableMaintenance["Cold", e.RowIndex].Value == null)
                        cold = false;
                    else
                        cold = Convert.ToBoolean(dgTableMaintenance["Cold", e.RowIndex].Value);

                    if (dgTableMaintenance["HotAndCold", e.RowIndex].Value == null)
                        hotAndCold = false;
                    else
                        hotAndCold = Convert.ToBoolean(dgTableMaintenance["HotAndCold", e.RowIndex].Value);

                    if (dgTableMaintenance["CheckGlove", e.RowIndex].Value == null)
                        checkGlove = false;
                    else
                        checkGlove = Convert.ToBoolean(dgTableMaintenance["CheckGlove", e.RowIndex].Value);

                    if (dgTableMaintenance["CheckSize", e.RowIndex].Value == null)
                        checkSize = false;
                    else
                        checkSize = Convert.ToBoolean(dgTableMaintenance["CheckSize", e.RowIndex].Value);
                    if (dgTableMaintenance["IsScheduledStop", e.RowIndex].Value == null)
                        isScheduledStop = false;
                    else
                        isScheduledStop = Convert.ToBoolean(dgTableMaintenance["IsScheduledStop", e.RowIndex].Value);

                    if (dgTableMaintenance["cmbLocationId", e.RowIndex].Value == null)
                    {
                        if (dgTableMaintenance["LocationId", e.RowIndex].Value != null)
                            locationId = dgTableMaintenance["LocationId", e.RowIndex].Value.ToString();
                    }
                    else
                        locationId = dgTableMaintenance["cmbLocationId", e.RowIndex].Value.ToString();

                    if (dgTableMaintenance["cmbGloveType", e.RowIndex].Value == null)
                    {
                        if (dgTableMaintenance["GloveType", e.RowIndex].Value != null)
                            gloveType = dgTableMaintenance["GloveType", e.RowIndex].Value.ToString();
                    }
                    else
                        gloveType = dgTableMaintenance["cmbGloveType", e.RowIndex].Value.ToString();

                    if (dgTableMaintenance["cmbSize", e.RowIndex].Value == null)
                    {
                        if (dgTableMaintenance["Size", e.RowIndex].Value != null)
                            size = dgTableMaintenance["Size", e.RowIndex].Value.ToString();
                    }
                    else
                        size = dgTableMaintenance["cmbSize", e.RowIndex].Value.ToString();

                    DryerDTO newDryObj = new DryerDTO();
                    newDryObj.DryerNumber = dryerNumber;
                    newDryObj.IsStopped = isStopped;
                    newDryObj.IsScheduledStop = isScheduledStop;
                    newDryObj.Hot = hot;
                    newDryObj.Cold = cold;
                    newDryObj.HotAndCold = hotAndCold;
                    newDryObj.LocationId = locationId;
                    newDryObj.CheckGlove = checkGlove;
                    newDryObj.CheckSize = checkSize;
                    newDryObj.GloveType = gloveType;
                    newDryObj.GloveSize = size;

                    string validationMessage = ValidateRequiredFields(Convert.ToString(dgTableMaintenance["DryerNumber", e.RowIndex].Value),
                                                                       locationId, gloveType, size);
                    if (validationMessage == Messages.REQUIREDFIELDMESSAGE)
                    {
                        try
                        {
                            string actionType = string.Empty;
                            if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert")
                            {
                                isDuplicate = Convert.ToBoolean(MasterTableBLL.IsDryerDuplicate(dryerNumber));
                                actionType = "Add";
                            }
                            else
                                actionType = "Update";
                            if (!isDuplicate)
                            {

                                //set audit log class
                                AuditLogDTO AuditLog = new AuditLogDTO();
                                AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                                AuditLog.FunctionName = _screenName;
                                AuditLog.CreatedBy = _loggedInUser;
                                Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), actionType);
                                AuditLog.AuditAction = Convert.ToInt32(audAction);
                                AuditLog.SourceTable = "DryerMaster";
                                string refid = "";

                                if (audAction != Constants.ActionLog.Add) //for delete and update get reference id , for add set id in stored procedure
                                {
                                    refid = oldDryerObj.Id.ToString();
                                    AuditLog.ReferenceId = refid;
                                    newDryObj.Id = oldDryerObj.Id;
                                }

                                AuditLog.UpdateColumns = oldDryerObj.DetailedCompare(newDryObj).GetPropChanges();

                                string audlog = CommonBLL.SerializeTOXML(AuditLog);


                                int rowsReturned = MasterTableBLL.UpdateDryerMasterDetails(dryerNumber, locationId, gloveType, size,
                                                                        isStopped, hot, cold, hotAndCold, checkGlove, checkSize,
                                                                        isScheduledStop, _loggedInUser, _wsId, audlog);
                                if (rowsReturned > 0)
                                {
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    _refresh = true;
                                    PopulateGrid();
                                    oldDryerObj = null;
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DUPLICATE_DRYER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
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
                bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
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
            dgTableMaintenance["DryerNumber", 0].ReadOnly = false;
            PopulateComboBoxes(Constants.ZERO);
            dgTableMaintenance[14, 0].Style.NullValue = "Insert";
            dgTableMaintenance.Columns[14].Name = "Insert";
            _cell = dgTableMaintenance.Rows[0];
            dgTableMaintenance.Columns[15].Name = "Cancel";
            dgTableMaintenance[15, 0].Style.NullValue = "Cancel";
            oldDryerObj = new DryerDTO();
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
        /// dgTableMaintenance_CellValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgTableMaintenance_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == cmbGloveType.Index && e.RowIndex >= 0) //check if combobox column
            {
                string selectedValue = dgTableMaintenance.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                _sizeList = MasterTableBLL.GetGloveSizesForWasher(selectedValue);
                _comboCellSize = dgTableMaintenance["cmbSize", e.RowIndex] as DataGridViewComboBoxCell;
                _comboCellSize.DataSource = new BindingSource(_sizeList, null);
                _comboCellSize.ValueMember = "IDField";
                _comboCellSize.DisplayMember = "DisplayField";
                dgTableMaintenance["cmbSize", e.RowIndex].Value = _sizeList[0].DisplayField;
            }
        }

        /// <summary>
        /// dgTableMaintenance_CurrentCellDirtyStateChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgTableMaintenance_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgTableMaintenance.IsCurrentCellDirty)
            {
                dgTableMaintenance.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }

        }

        /// <summary>
        /// Event Handler to open combobox on first click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgTableMaintenance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                bool validRow = (e.RowIndex != -1 && dgTableMaintenance.Rows[e.RowIndex].ReadOnly == false); //Make sure the clicked row isn't the header.
                var datagridview = sender as DataGridView;

                // Check to make sure the cell clicked is the cell containing the combobox 
                if (datagridview.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && validRow)
                {
                    datagridview.BeginEdit(true);
                    ((ComboBox)datagridview.EditingControl).DroppedDown = true;
                }
            }
            catch
            { return; }
        }

        /// <summary>
        /// Event to handle only integers in numeric fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgTableMaintenance_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgTableMaintenance.CurrentCell.ColumnIndex == dgTableMaintenance.Columns["DryerNumber"].Index)
            {
                TextBox ctrl = e.Control as TextBox;
                if (ctrl != null)
                {
                    ctrl.Sequence();
                }
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
        private string ValidateRequiredFields(string dryerNumber, string location, string gloveType, string size)
        {
            StringBuilder requiredFieldMessage = new StringBuilder(Messages.REQUIREDFIELDMESSAGE);
            if (string.IsNullOrEmpty(dryerNumber.Trim()))
                requiredFieldMessage.AppendLine("Dryer Number");
            if (string.IsNullOrEmpty(location))
                requiredFieldMessage.AppendLine("Location");
            if (string.IsNullOrEmpty(gloveType))
                requiredFieldMessage.AppendLine("Glove Type");
            if (string.IsNullOrEmpty(size))
                requiredFieldMessage.AppendLine("Size");
            return requiredFieldMessage.ToString();
        }

        /// <summary>
        /// Populate grid
        /// </summary>
        private void PopulateGrid()
        {
            try
            {
                _gloveTypeList = MasterTableBLL.GetGloveTypeFromAX();
                _locationList = MasterTableBLL.GetLocationList();
                _dtTableDetails = MasterTableBLL.GetDryerMasterDetails();
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
                dgTableMaintenance.Columns[14].Name = "Edit";
                dgTableMaintenance.Columns[15].Name = "Delete";
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
                _comboCellGloveType = dgTableMaintenance["cmbGloveType", rowIndex] as DataGridViewComboBoxCell;
                _comboCellGloveType.DataSource = new BindingSource(_gloveTypeList, null);
                _comboCellGloveType.ValueMember = "IDField";
                _comboCellGloveType.DisplayMember = "DisplayField";
                _comboCellSize = dgTableMaintenance["cmbSize", rowIndex] as DataGridViewComboBoxCell;
                _comboCellSize.DataSource = new BindingSource(_sizeList, null);
                _comboCellSize.ValueMember = "IDField";
                _comboCellSize.DisplayMember = "DisplayField";
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

                    dgTableMaintenance["DryerNumber", i].Value = dtTable.Rows[i]["DryerNumber"];
                    _sizeList = MasterTableBLL.GetGloveSizesForWasher(dtTable.Rows[i]["GloveType"].ToString());
                    PopulateComboBoxes(i);
                    _comboCellLoc.Style.NullValue = dtTable.Rows[i]["LocationName"].ToString();
                    _comboCellGloveType.Style.NullValue = dtTable.Rows[i]["GloveType"].ToString();
                    _comboCellSize.Style.NullValue = dtTable.Rows[i]["GloveSize"].ToString();
                    dgTableMaintenance["IsStopped", i].Value = dtTable.Rows[i]["IsStopped"];
                    dgTableMaintenance["Hot", i].Value = dtTable.Rows[i]["Hot"];
                    dgTableMaintenance["Cold", i].Value = dtTable.Rows[i]["Cold"];
                    dgTableMaintenance["HotAndCold", i].Value = dtTable.Rows[i]["HotAndCold"];
                    dgTableMaintenance["CheckGlove", i].Value = dtTable.Rows[i]["CheckGlove"];
                    dgTableMaintenance["CheckSize", i].Value = dtTable.Rows[i]["CheckSize"];
                    dgTableMaintenance["IsScheduledStop", i].Value = dtTable.Rows[i]["IsScheduledStop"];
                    dgTableMaintenance["LocationId", i].Value = dtTable.Rows[i]["LocationId"].ToString();
                    dgTableMaintenance["GloveType", i].Value = dtTable.Rows[i]["GloveType"].ToString();
                    dgTableMaintenance["Size", i].Value = dtTable.Rows[i]["GloveSize"].ToString();
                    dgTableMaintenance["DryerId", i].Value = dtTable.Rows[i]["DryerId"].ToString();
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
