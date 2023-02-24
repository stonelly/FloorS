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

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    public partial class InnerLabelSetMaster : Form
    {
        #region Member Variables

        private string _screenName = "Configuration SetUp - InnerLabelSetMaster";
        private string _className = "InnerLabelSetMaster";
        private bool _refresh = false;
        private DataGridViewRow _cell = null;
        DataGridViewComboBoxCell _comboCellStatus;
        private List<DropdownDTO> _statusList;
        private DataTable _dtTableDetails;
        private PageOffsetList _pg;
        private List<InnerLabelSetMasterDTO> _innerlabelsetmaster;
        public List<InnerLabelSetMasterDTO> InnerLabelSetmaster
        {
            get
            {
                if (_innerlabelsetmaster == null)
                {
                    _innerlabelsetmaster = MasterTableBLL.GetInnerLabelSetMaster();
                    return _innerlabelsetmaster;
                }
                else
                {
                    return _innerlabelsetmaster;
                }
            }
        }


        private string _loggedInUser;

        #endregion

        #region Constructor

        public InnerLabelSetMaster(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;

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
        private void InnerLabelSetMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;

        }
        private void InnerLabelSetMaster_Load(object sender, EventArgs e)
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

                        InnerLabelSetMasterDTO pdmNew = new InnerLabelSetMasterDTO();
                        pdmNew.InnerLabelSetNo = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["InnerLabelSetNo"].Value);
                        pdmNew.InnerLabelSetId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["InnerLabelSetId"].Value);
                        pdmNew.Description = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["Description"].Value);
                        pdmNew.SpecialCodeText = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["SpecialCodeText"].Value);
                        pdmNew.SpecialCode = Convert.ToBoolean(dgTableMaintenance.Rows[e.RowIndex].Cells["SpecialCode"].Value);
                        pdmNew.Status = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["cmbStatus"].Value);
                        pdmNew.CustomDate = Convert.ToBoolean(dgTableMaintenance.Rows[e.RowIndex].Cells["CustomDate"].Value);
                        pdmNew.MandatoryCustLotId = Convert.ToBoolean(dgTableMaintenance.Rows[e.RowIndex].Cells["MandatoryCustLotId"].Value);

                        pdmNew.ActionType = Constants.ActionLog.Update;
                        InnerLabelSetMasterDTO pdmold = MasterTableBLL.GetInnerLabelSetMaster().Where(p => p.InnerLabelSetId == pdmNew.InnerLabelSetId).FirstOrDefault();
                        string vldmsg = string.Empty;


                        if (string.IsNullOrEmpty(pdmNew.Description))
                            vldmsg += "Description";

                        if (string.IsNullOrEmpty(vldmsg))
                        {

                            MasterTableBLL.SaveInnerLabelSet(pdmold, pdmNew, _loggedInUser);
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            _refresh = true;
                            PopulateGrid();

                            _innerlabelsetmaster = null;
                            bindingSource_CurrentChanged(null, null);
                            return;
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                            return;
                        }
                    }
                    else if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Edit")
                    {
                        btnAdd.Enabled = false;
                        bindingNavigatorTable.Enabled = false;
                        dgTableMaintenance.Rows[e.RowIndex].ReadOnly = false;
                        dgTableMaintenance.Rows[e.RowIndex].Cells["InnerLabelSetNo"].ReadOnly = true;
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
                            InnerLabelSetMasterDTO pdmNew = new InnerLabelSetMasterDTO();
                            pdmNew = MasterTableBLL.GetInnerLabelSetMaster().Where(p => p.InnerLabelSetId == Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["InnerLabelSetId"].Value)).FirstOrDefault();

                            pdmNew.ActionType = Constants.ActionLog.Delete;
                            pdmNew.IsDeleted = true;
                            InnerLabelSetMasterDTO pdmold = MasterTableBLL.GetInnerLabelSetMaster().Where(p => p.InnerLabelSetId == Convert.ToInt32(pdmNew.InnerLabelSetId)).FirstOrDefault();
                            MasterTableBLL.SaveInnerLabelSet(pdmold, pdmNew, _loggedInUser);
                            _innerlabelsetmaster = null;
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
                        InnerLabelSetMasterDTO pdmNew = new InnerLabelSetMasterDTO();
                        pdmNew.Description = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["Description"].Value);

                        pdmNew.InnerLabelSetNo = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["InnerLabelSetNo"].Value);
                        pdmNew.SpecialCodeText = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["SpecialCodeText"].Value);
                        pdmNew.SpecialCode = Convert.ToBoolean(dgTableMaintenance.Rows[e.RowIndex].Cells["SpecialCode"].Value);
                        pdmNew.CustomDate = Convert.ToBoolean(dgTableMaintenance.Rows[e.RowIndex].Cells["CustomDate"].Value);
                        pdmNew.Status = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["cmbStatus"].Value);
                        pdmNew.MandatoryCustLotId = Convert.ToBoolean(dgTableMaintenance.Rows[e.RowIndex].Cells["MandatoryCustLotId"].Value);

                        InnerLabelSetMasterDTO pdmold = new InnerLabelSetMasterDTO();
                        pdmNew.ActionType = Constants.ActionLog.Add;
                        string vldmsg = string.Empty;

                        if (string.IsNullOrEmpty(pdmNew.InnerLabelSetNo))
                            vldmsg += "Label Set No";

                        if (string.IsNullOrEmpty(pdmNew.Description))
                            vldmsg += "\nDescription";

                        if (string.IsNullOrEmpty(vldmsg))
                        {
                            Int64 output;
                            bool isValidInteger = Int64.TryParse(pdmNew.InnerLabelSetNo, out output);
                            if (isValidInteger)
                            {
                                bool isDuplicate = false;
                                string InnerLabelSetNo = Convert.ToString(dgTableMaintenance["InnerLabelSetNo", e.RowIndex].Value);

                                isDuplicate = Convert.ToBoolean(MasterTableBLL.IsInnerLabelSetDuplicate(InnerLabelSetNo));
                                if (!isDuplicate)
                                {
                                    MasterTableBLL.SaveInnerLabelSet(pdmold, pdmNew, _loggedInUser);
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

                                    _refresh = true;
                                    PopulateGrid();
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DUPLICATE_INNERLABELSETMASTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    _refresh = true;
                                    PopulateGrid();
                                    return;
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.INVALID_DATA_SUMMARY + "Label Set No", Messages.INVALIDDATA, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                                return;
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                            return;
                        }
                        btnAdd.Enabled = true;
                        _innerlabelsetmaster = null;
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
                dgTableMaintenance["InnerLabelSetNo", 0].Value = string.Empty;
                dgTableMaintenance["SpecialCodeText", 0].Value = string.Empty;
                dgTableMaintenance["Description", 0].Value = string.Empty;
                PopulateComboBoxes(Constants.ZERO);
                _comboCellStatus.Value = "1"; //set default status to active

                _cell = dgTableMaintenance.Rows[0];
                dgTableMaintenance.Columns[8].Name = "Insert";
                dgTableMaintenance[8, 0].Style.NullValue = "Insert";
                dgTableMaintenance.Columns[9].Name = "Cancel";
                dgTableMaintenance[9, 0].Style.NullValue = "Cancel";
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
                dv.Sort = newColumn.Name + (direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                DataTable sortedDT = dv.ToTable();
                _dtTableDetails = sortedDT;
                FillGrid(_dtTableDetails);

                _pg = new PageOffsetList(Constants.TWENTY, _dtTableDetails.Rows.Count);
                bindingNavigatorTable.BindingSource = bindingSourceTable;
                bindingSourceTable.DataSource = _pg.GetList();

                dgTableMaintenance.Columns[8].Name = "Edit";
                dgTableMaintenance.Columns[9].Name = "Delete";
            }
        }
        #endregion

        #region User Methods
        private void PopulateGrid()
        {
            try
            {

                _statusList = MasterTableBLL.GetLabelSetStatus();
                _dtTableDetails = MasterTableBLL.GetInnerLabelSetMasterData();
                FillGrid(_dtTableDetails);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PopulateGrid", null);
                return;
            }

            _pg = new PageOffsetList(Constants.TWENTY, _dtTableDetails.Rows.Count);

            if (_dtTableDetails.Rows.Count > 0)
            {
                bindingNavigatorTable.BindingSource = bindingSourceTable;
                bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
                bindingSourceTable.DataSource = _pg.GetList();

                if (_refresh)
                {
                    dgTableMaintenance.Columns[8].Name = "Edit";
                    dgTableMaintenance.Columns[9].Name = "Delete";
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

                    dgTableMaintenance["InnerLabelSetId", i].Value = dtTable.Rows[i]["InnerLabelSetId"];
                    dgTableMaintenance["InnerLabelSetNo", i].Value = dtTable.Rows[i]["InnerLabelSetNo"];
                    dgTableMaintenance["Description", i].Value = dtTable.Rows[i]["Description"];
                    dgTableMaintenance["SpecialCodeText", i].Value = dtTable.Rows[i]["SpecialCodeText"];
                    dgTableMaintenance["SpecialCode", i].Value = dtTable.Rows[i]["SpecialCode"];
                    dgTableMaintenance["CustomDate", i].Value = dtTable.Rows[i]["CustomDate"];
                    dgTableMaintenance["MandatoryCustLotId", i].Value = dtTable.Rows[i]["MandatoryCustLotId"];

                    PopulateComboBoxes(i);
                    _comboCellStatus.Style.NullValue = dtTable.Rows[i]["StatusName"].ToString();
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                dgTableMaintenance.DataSource = null;
            }
        }

        private void PopulateComboBoxes(int rowIndex)
        {
            try
            {
                _comboCellStatus = dgTableMaintenance["cmbStatus", rowIndex] as DataGridViewComboBoxCell;
                _comboCellStatus.DataSource = new BindingSource(_statusList, null);
                _comboCellStatus.ValueMember = "IDField";
                _comboCellStatus.DisplayMember = "DisplayField";

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PopulateComboBoxes", null);
                return;
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
        #endregion
    }
}
