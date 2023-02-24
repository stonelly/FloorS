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
    public partial class QCTypeMaster : Form
    {
        #region Member Variables
        private bool _refresh = false;
        private string _screenName = "Configuration SetUp - QCTypeMaster";
        private string _className = "QCTypeMaster";
        private DataGridViewRow _cell = null;

        private PageOffsetList _pg;

        private DataTable _dtTableDetails;
        private List<QCTypeMasterDTO> _qctypemaster;
        public List<QCTypeMasterDTO> QCTypemaster
        {
            get
            {
                if (_qctypemaster == null)
                {
                    _qctypemaster = MasterTableBLL.GetQCTypeMaster();
                    return _qctypemaster;
                }
                else
                {
                    return _qctypemaster;
                }
            }
        }

        private bool _isUpdate = false;
        private string _loggedInUser;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public QCTypeMaster(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;

        }

        #endregion

        #region Event handler

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
        private void QCTypeMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;

        }

        private void QCTypeeMaster_Load(object sender, EventArgs e)
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dgTableMaintenance.Rows.Insert(0, 1);
                dgTableMaintenance.Rows[0].ReadOnly = false;
                dgTableMaintenance["QCType", 0].Value = string.Empty;
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


        private void dgTableMaintenance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > Constants.MINUSONE)
                {
                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Update")
                    {

                        QCTypeMasterDTO pdmNew = new QCTypeMasterDTO();
                        pdmNew.QCType = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["QCType"].Value);
                        pdmNew.QCTypeId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["QCTypeId"].Value);
                        pdmNew.Description = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["Description"].Value);
                        pdmNew.ActionType = Constants.ActionLog.Update;
                        pdmNew.QCEfficiency = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["QCEfficiency"].Value);

                        QCTypeMasterDTO pdmold = MasterTableBLL.GetQCTypeMaster().Where(p => p.QCTypeId == pdmNew.QCTypeId).FirstOrDefault();
                        string vldmsg = string.Empty;

                        if (string.IsNullOrEmpty(pdmNew.Description))
                            vldmsg += "\nDescription";
                        if (string.IsNullOrEmpty(vldmsg))
                        {
                            MasterTableBLL.SaveQCType(pdmold, pdmNew, _loggedInUser);
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            _refresh = true;
                            PopulateGrid();
                            _qctypemaster = null;
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

                        dgTableMaintenance.Rows[e.RowIndex].Cells["QCType"].ReadOnly = true;
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
                            QCTypeMasterDTO pdmNew = new QCTypeMasterDTO();
                            pdmNew = MasterTableBLL.GetQCTypeMaster().Where(p => p.QCTypeId == Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["QCTypeId"].Value)).FirstOrDefault();

                            pdmNew.ActionType = Constants.ActionLog.Delete;
                            pdmNew.Stopped = 1;
                            QCTypeMasterDTO pdmold = MasterTableBLL.GetQCTypeMaster().Where(p => p.QCTypeId == Convert.ToInt32(pdmNew.QCTypeId)).FirstOrDefault();
                            MasterTableBLL.SaveQCType(pdmold, pdmNew, _loggedInUser);
                            _qctypemaster = null;
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
                        QCTypeMasterDTO pdmNew = new QCTypeMasterDTO();
                        pdmNew.Description = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["Description"].Value);
                        QCTypeMasterDTO pdmold = new QCTypeMasterDTO();
                        pdmNew.ActionType = Constants.ActionLog.Add;
                        pdmNew.QCType = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["QCType"].Value);
                        pdmNew.QCEfficiency = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["QCEfficiency"].Value);

                        string vldmsg = string.Empty;
                        if (string.IsNullOrEmpty(pdmNew.QCType))
                            vldmsg += "\nQC Type";
                        if (string.IsNullOrEmpty(pdmNew.Description))
                            vldmsg += "\nDescription";
                        if (string.IsNullOrEmpty(vldmsg))
                        {
                            bool isDuplicate = false;
                            string QCType = Convert.ToString(dgTableMaintenance["QCType", e.RowIndex].Value);

                            isDuplicate = Convert.ToBoolean(MasterTableBLL.IsQCTypeDuplicate(QCType));
                            if (!isDuplicate)
                            {
                                MasterTableBLL.SaveQCType(pdmold, pdmNew, _loggedInUser);
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

                                _refresh = true;
                                PopulateGrid();
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DUPLICATE_QCTYPEMASTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
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
                        _qctypemaster = null;
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

        #region User Method

        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }

        private void PopulateGrid()
        {
            try
            {
                _dtTableDetails = MasterTableBLL.GetQCTypeMasterData();
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

                    dgTableMaintenance["QCTypeId", i].Value = dtTable.Rows[i]["QCTypeId"];
                    dgTableMaintenance["QCType", i].Value = dtTable.Rows[i]["QCType"];
                    dgTableMaintenance["Description", i].Value = dtTable.Rows[i]["Description"];
                    dgTableMaintenance["QCEfficiency", i].Value = dtTable.Rows[i]["QCEfficiency"];
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
