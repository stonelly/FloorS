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
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Workstation Master Maintenance
    /// File Type: Code file
    /// </summary> 
    public partial class LineMaster : Form
    {
        #region Member Variables

        private string _screenName = "Configuration SetUp - LineMaster";
        private string _className = "LineMaster";
        private int _prevRowindex = -1;
        private PageOffsetList _pg;
        private List<LineMasterDTO> _linemaster;
        public List<LineMasterDTO> Linemaster
        {
            get
            {
                if (_linemaster == null)
                {
                    _linemaster = MasterTableBLL.GetLinetMaster();
                    return _linemaster;
                }
                else
                {
                    return _linemaster;
                }
            }
        }
        private List<DropdownDTO> _locationMaster;
        private bool _isUpdate = false;
        private string _loggedInUser;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public LineMaster(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
            _locationMaster = MasterTableBLL.GetLocation();
        }

        /// <summary>
        /// Event Handler for Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineMaster_Load(object sender, EventArgs e)
        {
            try
            {
                List<LineMasterDTO> lstLineMasterDTO = Linemaster.GetRange(0, MasterTableBLL.GetRange(0, Linemaster.Count));
                BindGrid(lstLineMasterDTO);
            }

            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Event Handlers

        private void LineMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true; 
        }
            /// <summary>
            /// Used for handling the pagination for datagrid view
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void bindingSource_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                int offset = (int)bindingSourceTable.Current;
                List<LineMasterDTO> lstLineMasterDTO = Linemaster.GetRange(offset, MasterTableBLL.GetRange(offset, Linemaster.Count));
                BindGrid(lstLineMasterDTO);
                btnAdd.Enabled = true;
                _prevRowindex = -1;
                _isUpdate = false;
            }

            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "bindingSource_CurrentChanged", null);
                return;
            }
            catch (Exception ex)
            {
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "bindingSource_CurrentChanged", null);
                return;
            }
        }

        /// <summary>
        /// Event Handler for datagrid cell content click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgTableMaintenance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //added MYAdamas 20171016 to check is user click on header edit/delete
                if (e.RowIndex > Constants.MINUSONE)
                {
                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Update")
                    {
                        if (_prevRowindex != e.RowIndex)
                        {
                            btnAdd.Enabled = false;
                            dgTableMaintenance.Rows[_prevRowindex].ReadOnly = true;

                            dgTableMaintenance.Columns[e.ColumnIndex].Name = "Edit";
                            dgTableMaintenance[e.ColumnIndex, _prevRowindex].Style.NullValue = "Edit";
                            dgTableMaintenance.Columns[e.ColumnIndex + 1].Name = "Delete";
                            dgTableMaintenance[e.ColumnIndex + 1, _prevRowindex].Style.NullValue = "Delete";
                            _prevRowindex = e.RowIndex;
                            dgTableMaintenance.Rows[e.RowIndex].ReadOnly = false;
                            dgTableMaintenance.Columns[e.ColumnIndex].Name = "Update";
                            dgTableMaintenance[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                            dgTableMaintenance.Columns[e.ColumnIndex + 1].Name = "Cancel";
                            dgTableMaintenance[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";
                        }
                        else
                        {
                            LineMasterDTO pdmNew = new LineMasterDTO();
                            pdmNew.LineNumber = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["LineNumber"].Value);
                            pdmNew.ActionType = Constants.ActionLog.Update;
                            LineMasterDTO pdmold = MasterTableBLL.GetLinetMaster().Where(p => p.LineNumber == pdmNew.LineNumber).FirstOrDefault();
                            pdmNew.LocationId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["LocationId"].Value);
                            // if (!CheckDuplicate(pdmNew))
                            //{
                            MasterTableBLL.SaveLinetMaster(pdmold, pdmNew, _loggedInUser);
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            //}
                            //else
                            //{
                            //    GlobalMessageBox.Show(Messages.DUPLICATE_LINEMASTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            //}
                            _linemaster = null;
                            bindingSource_CurrentChanged(null, null);
                        }
                        return;
                    }


                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Edit")
                    {
                        btnAdd.Enabled = false;
                        if (_prevRowindex != e.RowIndex)
                        {
                            btnAdd.Enabled = false;
                            _prevRowindex = e.RowIndex;
                            dgTableMaintenance.Rows[e.RowIndex].ReadOnly = false;
                            dgTableMaintenance.Rows[e.RowIndex].Cells[0].ReadOnly = true;
                            dgTableMaintenance.Columns[e.ColumnIndex].Name = "Update";
                            dgTableMaintenance[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                            dgTableMaintenance.Columns[e.ColumnIndex + 1].Name = "Cancel";
                            dgTableMaintenance[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";
                            return;
                        }
                    }

                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Delete")
                    {
                        btnAdd.Enabled = false;
                        if (GlobalMessageBox.Show(Messages.DELETE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                        {
                            LineMasterDTO pdmNew = new LineMasterDTO();
                            pdmNew = MasterTableBLL.GetLinetMaster().Where(p => p.LineNumber == Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["LineNumber"].Value)).FirstOrDefault();

                            pdmNew.ActionType = Constants.ActionLog.Delete;
                            pdmNew.IsDeleted = true;
                            LineMasterDTO pdmold = MasterTableBLL.GetLinetMaster().Where(p => p.LineNumber == pdmNew.LineNumber).FirstOrDefault();
                            MasterTableBLL.SaveLinetMaster(pdmold, pdmNew, _loggedInUser);
                            _linemaster = null;
                            bindingSource_CurrentChanged(null, null);
                            return;
                        }
                    }
                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert")
                    {
                        LineMasterDTO pdmNew = new LineMasterDTO();
                        pdmNew.LineNumber = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["LineNumber"].Value);
                        LineMasterDTO pdmold = new LineMasterDTO();
                        pdmNew.ActionType = Constants.ActionLog.Add;
                        pdmNew.LocationId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["LocationId"].Value);
                        string vldmsg = string.IsNullOrEmpty(pdmNew.LineNumber) ? "Line Number" : string.Empty;
                        if (string.IsNullOrEmpty(vldmsg))
                        {
                            if (!CheckDuplicate(pdmNew))
                            {
                                MasterTableBLL.SaveLinetMaster(pdmold, pdmNew, _loggedInUser);
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DUPLICATE_LINEMASTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                            return;
                        }
                        btnAdd.Enabled = true;
                        _linemaster = null;
                        bindingSource_CurrentChanged(null, null);
                        return;
                    }
                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Cancel")
                    {
                        btnAdd.Enabled = true;
                        _prevRowindex = -1;
                        bindingSource_CurrentChanged(null, null);
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

        /// <summary>
        /// Event Handler for Add button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dgTableMaintenance.Rows.Insert(0, 1);
                dgTableMaintenance.Rows[0].ReadOnly = false;
                dgTableMaintenance["LineNumber", 0].Value = string.Empty;
                DataGridViewComboBoxCell comboCell1 = dgTableMaintenance["LocationId", 0] as DataGridViewComboBoxCell;
                comboCell1.DataSource = _locationMaster;
                comboCell1.ValueMember = "IDField";
                comboCell1.DisplayMember = "DisplayField";
                comboCell1.Value = Convert.ToString(_locationMaster.FirstOrDefault().IDField);
                dgTableMaintenance.Columns[2].Name = "Insert";
                dgTableMaintenance[2, 0].Style.NullValue = "Insert";
                dgTableMaintenance.Columns[3].Name = "Cancel";
                dgTableMaintenance[3, 0].Style.NullValue = "Cancel";
                btnAdd.Enabled = false;
            }
            catch (Exception ex)
            {
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "btnAdd_Click", null);
                return;
            }
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
        private void dgTableMaintenance_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

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
                _linemaster = Sort(_linemaster, newColumn.Name, direction);
                BindGrid(_linemaster);

                _pg = new PageOffsetList(Constants.TWENTY, _linemaster.Count);
                bindingNavigatorTable.BindingSource = bindingSourceTable;
                bindingSourceTable.DataSource = _pg.GetList();

                dgTableMaintenance.Columns[2].Name = "Edit";
                dgTableMaintenance.Columns[3].Name = "Delete";
            }
        }

        public List<LineMasterDTO> Sort(List<LineMasterDTO> input, string property, ListSortDirection orderSeq)
        {
            if (orderSeq == ListSortDirection.Ascending)
            {
                return input.OrderBy(p => p.GetType()
                                       .GetProperty(property)
                                       .GetValue(p, null)).ToList();
            }
            else
            {
                return input.OrderByDescending(p => p.GetType()
                                                     .GetProperty(property)
                                                     .GetValue(p, null)).ToList();
            }
        }

        #endregion

        #region User Methods

        private void BindGrid(List<LineMasterDTO> prodefectmaster)
        {
            bindingSourceTable.CurrentChanged -= new System.EventHandler(bindingSource_CurrentChanged);
            dgTableMaintenance.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
            dgTableMaintenance.AutoGenerateColumns = false;
            _pg = new PageOffsetList(Constants.TWENTY, Linemaster.Count);
            bindingSourceTable.DataSource = _pg.GetList();
            bindingNavigatorTable.BindingSource = bindingSourceTable;
            populateGrid(prodefectmaster);
            bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
            dgTableMaintenance.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
        }

        private void populateGrid(List<LineMasterDTO> prodefectmaster)
        {
            try
            {
                int i = 0;
                dgTableMaintenance.Rows.Clear();
                foreach (LineMasterDTO dr in prodefectmaster)
                {
                    dgTableMaintenance.Rows.Add();
                    dgTableMaintenance.Rows[i].ReadOnly = true;
                    dgTableMaintenance["LineNumber", i].Value = dr.LineNumber;

                    DataGridViewComboBoxCell comboCell1 = dgTableMaintenance["LocationId", i] as DataGridViewComboBoxCell;
                    comboCell1.DataSource = _locationMaster;
                    comboCell1.ValueMember = "IDField";
                    comboCell1.DisplayMember = "DisplayField";
                    comboCell1.Value = Convert.ToString(dr.LocationId);
                    i++;
                }
                dgTableMaintenance.Columns[2].Name = "Edit";
                dgTableMaintenance.Columns[3].Name = "Delete";
            }
            catch (Exception ex)
            {
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "populateGrid", null);
                return;
            }
        }

        private bool CheckDuplicate(LineMasterDTO lmnew)
        {
            bool _isduplicate = false;

            int lstcount = (from p in Linemaster
                            where p.LineNumber.ToUpper() == lmnew.LineNumber.ToUpper() && p.IsDeleted == lmnew.IsDeleted  // 20170927 MYAdamas commented due to line number unique && p.LocationId == lmnew.LocationId
                            select p).ToList<LineMasterDTO>().Count;
            if (lstcount > 0)
            {
                _isduplicate = true;
            }
            return _isduplicate;
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
    }
}
