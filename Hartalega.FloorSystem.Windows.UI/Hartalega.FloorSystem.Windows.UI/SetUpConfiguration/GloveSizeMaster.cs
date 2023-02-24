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
    public partial class GloveSizeMaster : Form
    {
        #region Member Variables

        private string _screenName = "Configuration SetUp - GloveSizeMaster";
        private string _className = "GloveSizeMaster";
        private bool _refresh = false;
        private DataGridViewRow _cell = null;

        private DataTable _dtTableDetails;
        private PageOffsetList _pg;
        private List<GloveSizeMasterDTO> _glovesizemaster;
        public List<GloveSizeMasterDTO> GloveSizemaster
        {
            get
            {
                if (_glovesizemaster == null)
                {
                    _glovesizemaster = MasterTableBLL.GetGloveSizeMaster();
                    return _glovesizemaster;
                }
                else
                {
                    return _glovesizemaster;
                }
            }
        }


        private string _loggedInUser;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor initialising the components
        /// </summary>       
        /// <returns></returns>

        public GloveSizeMaster(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;

        }

        #endregion

        #region Event Handlers

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
        private void GloveSizeMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;

        }
        /// <summary>
        /// Event Handler for Form load
        private void GloveSizeMaster_Load(object sender, EventArgs e)
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

                        GloveSizeMasterDTO pdmNew = new GloveSizeMasterDTO();
                        pdmNew.CommonSize = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["CommonSize"].Value);
                        pdmNew.CommonSizeID = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["CommonSizeID"].Value);
                        pdmNew.Description = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["Description"].Value);
                        pdmNew.OldSize = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["OldSize"].Value);

                        pdmNew.ActionType = Constants.ActionLog.Update;

                        GloveSizeMasterDTO pdmold = MasterTableBLL.GetGloveSizeMaster().Where(p => p.CommonSizeID == pdmNew.CommonSizeID).FirstOrDefault();

                        string vldmsg = string.Empty;

                        if (string.IsNullOrEmpty(pdmNew.CommonSize))
                            vldmsg += "\nSize Name";
                        if (string.IsNullOrEmpty(pdmNew.Description))
                            vldmsg += "\nDescription";
                        if (string.IsNullOrEmpty(pdmNew.OldSize))
                            vldmsg += "\nOld Size";



                        if (string.IsNullOrEmpty(vldmsg))
                        {
                            MasterTableBLL.SaveGloveSize(pdmold, pdmNew, _loggedInUser);
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            _refresh = true;
                            PopulateGrid();

                            _glovesizemaster = null;
                            bindingSource_CurrentChanged(null, null);
                            return;
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

                        dgTableMaintenance.Rows[e.RowIndex].Cells["CommonSize"].ReadOnly = true;
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
                            GloveSizeMasterDTO pdmNew = new GloveSizeMasterDTO();
                            pdmNew = MasterTableBLL.GetGloveSizeMaster().Where(p => p.CommonSizeID == Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["CommonSizeId"].Value)).FirstOrDefault();
                            pdmNew.Stopped = 1;
                            pdmNew.ActionType = Constants.ActionLog.Delete;
                            // pdmNew.IsDeleted = true;
                            GloveSizeMasterDTO pdmold = MasterTableBLL.GetGloveSizeMaster().Where(p => p.CommonSizeID == Convert.ToInt32(pdmNew.CommonSizeID)).FirstOrDefault();
                            MasterTableBLL.SaveGloveSize(pdmold, pdmNew, _loggedInUser);
                            _glovesizemaster = null;
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
                        GloveSizeMasterDTO pdmNew = new GloveSizeMasterDTO();
                        pdmNew.Description = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["Description"].Value);

                        pdmNew.CommonSize = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["CommonSize"].Value);
                        pdmNew.OldSize = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["OldSize"].Value);

                        GloveSizeMasterDTO pdmold = new GloveSizeMasterDTO();
                        pdmNew.ActionType = Constants.ActionLog.Add;

                        string vldmsg = string.Empty;

                        if (string.IsNullOrEmpty(pdmNew.CommonSize))
                            vldmsg += "\nSize Name";
                        if (string.IsNullOrEmpty(pdmNew.Description))
                            vldmsg += "\nDescription";
                        if (string.IsNullOrEmpty(pdmNew.OldSize))
                            vldmsg += "\nOld Size";



                        if (string.IsNullOrEmpty(vldmsg))
                        {
                            bool isDuplicate = false;
                            string CommonSize = Convert.ToString(dgTableMaintenance["CommonSize", e.RowIndex].Value);

                            isDuplicate = Convert.ToBoolean(MasterTableBLL.IsGloveSizeDuplicate(CommonSize));
                            if (!isDuplicate)
                            {
                                MasterTableBLL.SaveGloveSize(pdmold, pdmNew, _loggedInUser);
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

                                _refresh = true;
                                PopulateGrid();
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DUPLICATE_GLOVESIZEMASTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
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
                        btnAdd.Enabled = true;
                        _glovesizemaster = null;
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
                dgTableMaintenance["CommonSize", 0].Value = string.Empty;
                dgTableMaintenance["Description", 0].Value = string.Empty;
                _cell = dgTableMaintenance.Rows[0];
                dgTableMaintenance.Columns[4].Name = "Insert";
                dgTableMaintenance[4, 0].Style.NullValue = "Insert";
                dgTableMaintenance.Columns[5].Name = "Cancel";
                dgTableMaintenance[5, 0].Style.NullValue = "Cancel";
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

                dgTableMaintenance.Columns[4].Name = "Edit";
                dgTableMaintenance.Columns[5].Name = "Delete";
            }
        }
        #endregion

        #region User Methods
        private void PopulateGrid()
        {
            try
            {
                _dtTableDetails = MasterTableBLL.GetGloveSizeMasterData();
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
                    dgTableMaintenance.Columns[4].Name = "Edit";
                    dgTableMaintenance.Columns[5].Name = "Delete";

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

                    dgTableMaintenance["CommonSizeId", i].Value = dtTable.Rows[i]["CommonSizeId"];
                    dgTableMaintenance["CommonSize", i].Value = dtTable.Rows[i]["CommonSize"];
                    dgTableMaintenance["Description", i].Value = dtTable.Rows[i]["Description"];
                    dgTableMaintenance["OldSize", i].Value = dtTable.Rows[i]["OldSize"];
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
        #endregion
    }
}
