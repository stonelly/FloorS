using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.Database;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    public partial class ShiftMaster : Form
    {
        #region Member Variables

        private string _screenName = "Configuration SetUp - ShiftMaster";
        private string _className = "ShiftMaster";
        private bool _refresh = false;
        private DataGridViewRow _cell = null;
        private List<DropdownDTO> _grouptype;
        private DataTable _dtTableDetails;
        private PageOffsetList _pg;
        private List<ShiftMasterDTO> _shiftmaster;
        public List<ShiftMasterDTO> Shiftmaster
        {
            get
            {
                if (_shiftmaster == null)
                {
                    _shiftmaster = MasterTableBLL.GetShiftMaster();
                    return _shiftmaster;
                }
                else
                {
                    return _shiftmaster;
                }
            }
        }


        private string _loggedInUser;
        #endregion

        #region Constructor

        public ShiftMaster(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
            _grouptype = MasterTableBLL.GetAreaList();
        }
        #endregion

        #region Event Handler

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

        private void ShiftMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;

        }
        private void ShiftMaster_Load(object sender, EventArgs e)
        {
            try
            {
                bindingSourceTable.CurrentChanged -= new System.EventHandler(bindingSource_CurrentChanged);
                dgTableMaintenance.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
                _refresh = false;
                PopulateGrid();
                bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
                dgTableMaintenance.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);

            }

            catch (Exception ex)
            {

            }
        }

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

        private void dgTableMaintenance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > Constants.MINUSONE)
                {
                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Update")
                    {
                        ShiftMasterDTO pdmNew = new ShiftMasterDTO();
                        pdmNew.Name = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["ShiftName"].Value);
                        pdmNew.ShiftId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["ShiftId"].Value);
                        pdmNew.GroupTypeId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["GroupType"].Value);
                        pdmNew.InTime = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["InTime"].Value);
                        pdmNew.OutTime = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["OutTime"].Value);
                        string GroupType = MasterTableBLL.GetGroupTypeByGroupId(pdmNew.GroupTypeId);
                        pdmNew.GroupType = GroupType;
                        pdmNew.ActionType = Constants.ActionLog.Update;
                        ShiftMasterDTO pdmold = MasterTableBLL.GetShiftMaster().Where(p => p.ShiftId == pdmNew.ShiftId).FirstOrDefault();


                        string vldmsg = string.Empty;
                        if (string.IsNullOrEmpty(pdmNew.Name))
                            vldmsg += "\nShift Name";
                        if (string.IsNullOrEmpty(pdmNew.InTime))
                            vldmsg += "\nIn Time";
                        if (string.IsNullOrEmpty(pdmNew.OutTime))
                            vldmsg += "\nOut Time";
                        if (string.IsNullOrEmpty(vldmsg))
                        {


                            string Name = Convert.ToString(dgTableMaintenance["ShiftName", e.RowIndex].Value);
                            int GroupTypeId = Convert.ToInt32(dgTableMaintenance["GroupType", e.RowIndex].Value);
                            bool isDuplicate = false;

                            isDuplicate = Convert.ToBoolean(MasterTableBLL.IsShiftMasterDuplicate(Name, pdmNew.GroupType, pdmNew.ShiftId));
                            if (!isDuplicate)
                            {

                                if (IsValidTimeFormat(pdmNew.InTime) && IsValidTimeFormat(pdmNew.OutTime))
                                {
                                    if (!CompareTimeEdit(TimeSpan.Parse(pdmNew.InTime), TimeSpan.Parse(pdmNew.OutTime), GroupTypeId, pdmNew.ShiftId))
                                    {
                                        MasterTableBLL.SaveShiftMaster(pdmold, pdmNew, _loggedInUser);
                                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        _refresh = true;
                                        PopulateGrid();
                                        _shiftmaster = null;
                                        bindingSource_CurrentChanged(null, null);
                                        return;
                                    }
                                    else
                                    {
                                        GlobalMessageBox.Show(Messages.DUPLICATE_SHIFTMASTER_TIMEOVERLAP, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        _refresh = true;
                                        PopulateGrid();
                                        return;
                                    }
                                }
                                else
                                {
                                    string invalidtime = "";
                                    if (!IsValidTimeFormat(pdmNew.InTime))
                                        invalidtime = "\nIn Time";
                                    if (!IsValidTimeFormat(pdmNew.OutTime))
                                        invalidtime += "\nOut Time";
                                    GlobalMessageBox.Show(Messages.INVALID_DATA_SUMMARY + invalidtime, Messages.INVALIDDATA, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                                    return;
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DUPLICATE_SHIFTMASTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                _refresh = true;
                                PopulateGrid();
                                return;
                            }

                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                            return;
                        }
                    }
                    else if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Edit")
                    {
                        btnAdd.Enabled = false;
                        bindingNavigatorTable.Enabled = false;
                        dgTableMaintenance.Rows[e.RowIndex].ReadOnly = false;
                        dgTableMaintenance.Columns[e.ColumnIndex].Name = "Update";
                        dgTableMaintenance[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                        dgTableMaintenance.Columns[e.ColumnIndex + 1].Name = "Cancel";
                        dgTableMaintenance[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";
                        _cell = dgTableMaintenance.CurrentRow;
                    }
                    else if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Cancel" && dgTableMaintenance.CurrentRow == _cell)
                    {
                        _refresh = true;
                        PopulateGrid();
                    }
                    else if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Delete")
                    {
                        btnAdd.Enabled = false;
                        if (GlobalMessageBox.Show(Messages.DELETE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                        {
                            ShiftMasterDTO pdmNew = new ShiftMasterDTO();
                            pdmNew = MasterTableBLL.GetShiftMaster().Where(p => p.ShiftId == Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["ShiftId"].Value)).FirstOrDefault();
                            pdmNew.ActionType = Constants.ActionLog.Delete;
                            pdmNew.IsDeleted = true;
                            ShiftMasterDTO pdmold = MasterTableBLL.GetShiftMaster().Where(p => p.ShiftId == Convert.ToInt32(pdmNew.ShiftId)).FirstOrDefault();
                            MasterTableBLL.SaveShiftMaster(pdmold, pdmNew, _loggedInUser);
                            _shiftmaster = null;
                            bindingSource_CurrentChanged(null, null);
                            PopulateGrid();
                        }
                        else
                        {
                            btnAdd.Enabled = true; // if user choose no confirm
                        }
                    }
                    else if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert")
                    {


                        ShiftMasterDTO pdmNew = new ShiftMasterDTO();
                        pdmNew.Name = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["ShiftName"].Value);
                        string a = dgTableMaintenance.Rows[e.RowIndex].Cells["GroupType"].ToString();
                        pdmNew.GroupTypeId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["GroupType"].Value);
                        pdmNew.InTime = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["InTime"].Value);
                        pdmNew.OutTime = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["OutTime"].Value);
                        pdmNew.ActionType = Constants.ActionLog.Add;

                        pdmNew.GroupType = MasterTableBLL.GetGroupTypeByGroupId(pdmNew.GroupTypeId);
                        string vldmsg = string.Empty;
                        if (string.IsNullOrEmpty(pdmNew.Name))
                            vldmsg += "\nShift Name";
                        if (string.IsNullOrEmpty(pdmNew.InTime))
                            vldmsg += "\nIn Time";
                        if (string.IsNullOrEmpty(pdmNew.OutTime))
                            vldmsg += "\nOut Time";
                        if (string.IsNullOrEmpty(vldmsg))
                        {
                            if (IsValidTimeFormat(pdmNew.InTime) && IsValidTimeFormat(pdmNew.OutTime))
                            {
                                ShiftMasterDTO pdmold = new ShiftMasterDTO();


                                bool isDuplicate = false;
                                string Name = Convert.ToString(dgTableMaintenance["ShiftName", e.RowIndex].Value);
                                int GroupTypeId = Convert.ToInt32(dgTableMaintenance["GroupType", e.RowIndex].Value);
                                isDuplicate = Convert.ToBoolean(MasterTableBLL.IsShiftMasterDuplicate(Name, pdmNew.GroupType));
                                if (!isDuplicate)
                                {
                                    if (!CompareTime(TimeSpan.Parse(pdmNew.InTime), TimeSpan.Parse(pdmNew.OutTime), GroupTypeId))
                                    {
                                        MasterTableBLL.SaveShiftMaster(pdmold, pdmNew, _loggedInUser);
                                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        _refresh = true;
                                        PopulateGrid();
                                    }
                                    else
                                    {
                                        GlobalMessageBox.Show(Messages.DUPLICATE_SHIFTMASTER_TIMEOVERLAP, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        _refresh = true;
                                        PopulateGrid();
                                        return;
                                    }
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DUPLICATE_SHIFTMASTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    _refresh = true;
                                    PopulateGrid();
                                    return;
                                }
                            }
                            else
                            {
                                string invalidtime = "";
                                if (!IsValidTimeFormat(pdmNew.InTime))
                                    invalidtime = "\nIn Time";
                                if (!IsValidTimeFormat(pdmNew.OutTime))
                                    invalidtime += "\nOut Time";
                                GlobalMessageBox.Show(Messages.INVALID_DATA_SUMMARY + invalidtime, Messages.INVALIDDATA, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                                return;
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                            return;
                        }

                        btnAdd.Enabled = true;
                        _shiftmaster = null;
                        bindingSource_CurrentChanged(null, null);
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "dgTableMaintenance_CellContentClick", null);
                return;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dgTableMaintenance.Rows.Insert(0, 1);
                dgTableMaintenance.Rows[0].ReadOnly = false;

                DataGridViewComboBoxCell comboCell1 = dgTableMaintenance["GroupType", 0] as DataGridViewComboBoxCell;
                comboCell1.DataSource = _grouptype;
                comboCell1.ValueMember = "IDField";
                comboCell1.DisplayMember = "DisplayField";
                comboCell1.Value = Convert.ToString(_grouptype.FirstOrDefault().IDField);

                _cell = dgTableMaintenance.Rows[0];
                dgTableMaintenance.Columns[5].Name = "Insert";
                dgTableMaintenance[5, 0].Style.NullValue = "Insert";
                dgTableMaintenance.Columns[6].Name = "Cancel";
                dgTableMaintenance[6, 0].Style.NullValue = "Cancel";
                btnAdd.Enabled = false;
            }
            catch (Exception ex)
            {
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "btnAdd_Click", null);
                return;
            }
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

                switch (newColumn.Name)
                {
                    case "ShiftName":
                        dv.Sort = "Name" + (direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                        break;
                    default:
                        dv.Sort = newColumn.Name + (direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                        break;
                }

                DataTable sortedDT = dv.ToTable();
                _dtTableDetails = sortedDT;
                FillGrid(_dtTableDetails);

                _pg = new PageOffsetList(Constants.TWENTY, _dtTableDetails.Rows.Count);
                bindingNavigatorTable.BindingSource = bindingSourceTable;
                bindingSourceTable.DataSource = _pg.GetList();

                dgTableMaintenance.Columns[5].Name = "Edit";
                dgTableMaintenance.Columns[6].Name = "Delete";
            }
        }


        #endregion

        #region User Method
        private void PopulateGrid()
        {
            try
            {
                _dtTableDetails = MasterTableBLL.GetShiftMasterData();
                FillGrid(_dtTableDetails);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PopulateGrid", null);
                return;
            }

            _pg = new PageOffsetList(Constants.TWENTY, _dtTableDetails.Rows.Count);
            if (_dtTableDetails.Rows.Count > 0) // to prevent no rows will hit error on bindingsource table
            {
                bindingNavigatorTable.BindingSource = bindingSourceTable;
                bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
                bindingSourceTable.DataSource = _pg.GetList();

                if (_refresh)
                {
                    dgTableMaintenance.Columns[5].Name = "Edit";
                    dgTableMaintenance.Columns[6].Name = "Delete";
                }
            }
        }

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

                    dgTableMaintenance["ShiftId", i].Value = dtTable.Rows[i]["ShiftId"];
                    dgTableMaintenance["ShiftName", i].Value = dtTable.Rows[i]["Name"];
                    dgTableMaintenance["InTime", i].Value = dtTable.Rows[i]["InTime"];
                    dgTableMaintenance["OutTime", i].Value = dtTable.Rows[i]["OutTime"];

                    DataGridViewComboBoxCell comboCell1 = dgTableMaintenance["GroupType", i] as DataGridViewComboBoxCell;
                    comboCell1.DataSource = _grouptype;
                    comboCell1.ValueMember = "IDField";
                    comboCell1.DisplayMember = "DisplayField";
                    //database store description noy area master id??
                    comboCell1.Value = dtTable.Rows[i]["GroupTypeId"].ToString();
                }


            }
            else
            {
                GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                dgTableMaintenance.DataSource = null;
            }
        }


        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }

        public bool IsValidTimeFormat(string input)
        {
            TimeSpan output;
            return TimeSpan.TryParse(input, out output);
        }

        public bool CompareTime(TimeSpan intime, TimeSpan outtime, int grouptypeid)
        {
            bool overlap = false;

            if (_dtTableDetails != null && _dtTableDetails.Rows.Count > 0)
            {
                _shiftmaster = (from DataRow r in _dtTableDetails.Rows
                                select new ShiftMasterDTO
                                {
                                    Name = FloorDBAccess.GetString(r, "Name"),
                                    GroupTypeId = FloorDBAccess.GetValue<int>(r, "GroupTypeId"),
                                    InTime = FloorDBAccess.GetString(r, "InTime"),
                                    OutTime = FloorDBAccess.GetString(r, "OutTime")
                                }).ToList();
            }

            List<ShiftMasterDTO> item = _shiftmaster.Where(p => p.GroupTypeId == grouptypeid).ToList();

            for (int i = 0; i < item.Count; i++)
            {
                overlap = TimeSpan.Parse(item[i].InTime) < outtime && intime < TimeSpan.Parse(item[i].OutTime);
                if (overlap)
                    return overlap;

                if (outtime < intime)
                {
                    return intime > TimeSpan.Parse(item[i].InTime) && outtime < TimeSpan.Parse(item[i].InTime);
                }
            }

            return overlap;
        }

        public bool CompareTimeEdit(TimeSpan intime, TimeSpan outtime, int grouptypeid, int ShiftId)
        {
            bool overlap = false;

            if (_dtTableDetails != null && _dtTableDetails.Rows.Count > 0)
            {
                _shiftmaster = (from DataRow r in _dtTableDetails.Rows
                                select new ShiftMasterDTO
                                {
                                    Name = FloorDBAccess.GetString(r, "Name"),
                                    GroupTypeId = FloorDBAccess.GetValue<int>(r, "GroupTypeId"),
                                    InTime = FloorDBAccess.GetString(r, "InTime"),
                                    OutTime = FloorDBAccess.GetString(r, "OutTime"),
                                    ShiftId = FloorDBAccess.GetValue<int>(r, "ShiftId"),
                                }).ToList();
            }

            List<ShiftMasterDTO> item = _shiftmaster.Where(p => p.GroupTypeId == grouptypeid && p.ShiftId != ShiftId).ToList();

            for (int i = 0; i < item.Count; i++)
            {
                overlap = TimeSpan.Parse(item[i].InTime) < outtime && intime < TimeSpan.Parse(item[i].OutTime);
                if (overlap)
                    return overlap;

                if (outtime < intime)
                {
                    return intime > TimeSpan.Parse(item[i].InTime) && outtime < TimeSpan.Parse(item[i].InTime);
                }
            }

            return overlap;
        }
        #endregion

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
