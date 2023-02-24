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
    public partial class QITestResultAQL : Form
    {
        #region Member Variables
        private string _screenName = "Configuration SetUp - LineMaster";
        private string _className = "LineMaster";
        private int _prevRowindex = -1;
        private List<QITestResultAQLDTO> _lstQITestResultAQL;
        private PageOffsetList _pg;
        public List<QITestResultAQLDTO> LstQITestResultAQL
        {
            get
            {
                if (_lstQITestResultAQL == null)
                {
                    _lstQITestResultAQL = MasterTableBLL.GetQITestResultAQL();
                    return _lstQITestResultAQL;
                }
                else
                {
                    return _lstQITestResultAQL;
                }
            }
        }
        private bool _isUpdate = false;
        private string _loggedInUser;
        private List<DropdownDTO> _QAIAQL;
        private List<DropdownDTO> _qCType;
        private List<DropdownDTO> _customerType;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public QITestResultAQL(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
            _QAIAQL = MasterTableBLL.GetQAIAQL();
            _qCType = CommonBLL.GetQCType();
            _customerType = CommonBLL.GetEnumText(Constants.CUSTOMER_TYPE);
        }

        /// <summary>
        /// Event Handler for Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QITestResultAQL_Load(object sender, EventArgs e)
        {
            List<QITestResultAQLDTO> lstQITestResultAQL = LstQITestResultAQL.GetRange(0, MasterTableBLL.GetRange(0, LstQITestResultAQL.Count));
            BindGrid(lstQITestResultAQL);
        }

        #endregion

        #region Event Handlers
        private void QITestResult_KeyDown(object sender, KeyEventArgs e)
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
                List<QITestResultAQLDTO> lstQITestResultAQLDTO = LstQITestResultAQL.GetRange(offset, MasterTableBLL.GetRange(offset, LstQITestResultAQL.Count));
                BindGrid(lstQITestResultAQLDTO);
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
                //added by MYAdamas 20171016 to check is user click on header
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
                            QITestResultAQLDTO pdmNew = new QITestResultAQLDTO();
                            pdmNew.TestResultID = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["TestResultID"].Value);
                            pdmNew.ActionType = Constants.ActionLog.Update;
                            QITestResultAQLDTO pdmold = MasterTableBLL.GetQITestResultAQL().Where(p => p.TestResultID == pdmNew.TestResultID).FirstOrDefault();
                            string vldmsg = Validate(Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["WTSamplingSize"].Value),
                                                        Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["VTSamplingSize"].Value),
                                                         Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMinVal"].Value),
                                                         Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMaxVal"].Value));
                            if (string.IsNullOrEmpty(vldmsg))
                            {
                                pdmNew.DefectMaxVal = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMaxVal"].Value);
                                pdmNew.DefectMinVal = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMinVal"].Value);
                                pdmNew.VTSamplingSize = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["VTSamplingSize"].Value);
                                pdmNew.AQLID = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["AQLID"].Value);
                                pdmNew.QCTypeId = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["QCTypeId"].Value);
                                pdmNew.WTSamplingSize = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["WTSamplingSize"].Value);
                                pdmNew.CustomerTypeId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["CustomerTypeId"].Value);
                                if (!CheckDuplicate(pdmNew))
                                {
                                    MasterTableBLL.SaveQITestResultAQL(pdmold, pdmNew, _loggedInUser);
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DUPLICATE_QITESTRESULTAQL, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                                return;
                            }
                            _lstQITestResultAQL = null;
                            bindingSource_CurrentChanged(null, null);
                            return;
                        }
                    }

                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Edit")
                    {
                        if (_prevRowindex != e.RowIndex)
                        {
                            btnAdd.Enabled = false;
                            _prevRowindex = e.RowIndex;
                            dgTableMaintenance.Rows[e.RowIndex].ReadOnly = false;
                            dgTableMaintenance.Columns[e.ColumnIndex].Name = "Update";
                            dgTableMaintenance[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                            dgTableMaintenance.Columns[e.ColumnIndex + 1].Name = "Cancel";
                            dgTableMaintenance[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";
                        }
                    }

                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Delete")
                    {
                        if (GlobalMessageBox.Show(Messages.DELETE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                        {
                            QITestResultAQLDTO pdmNew = new QITestResultAQLDTO();
                            pdmNew = MasterTableBLL.GetQITestResultAQL().Where(p => p.TestResultID == Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["TestResultID"].Value)).FirstOrDefault();

                            pdmNew.ActionType = Constants.ActionLog.Delete;
                            pdmNew.IsDeleted = true;
                            QITestResultAQLDTO pdmold = MasterTableBLL.GetQITestResultAQL().Where(p => p.TestResultID == Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["TestResultID"].Value)).FirstOrDefault();
                            MasterTableBLL.SaveQITestResultAQL(pdmold, pdmNew, _loggedInUser);
                            _lstQITestResultAQL = null;
                            bindingSource_CurrentChanged(null, null);
                        }
                    }
                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert")
                    {
                        QITestResultAQLDTO pdmNew = new QITestResultAQLDTO();
                        QITestResultAQLDTO pdmold = new QITestResultAQLDTO();
                        pdmNew.ActionType = Constants.ActionLog.Add;
                        string vldmsg = Validate(Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["WTSamplingSize"].Value),
                                                    Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["VTSamplingSize"].Value),
                                                    Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMinVal"].Value),
                                                    Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMaxVal"].Value));
                        if (string.IsNullOrEmpty(vldmsg))
                        {
                            pdmNew.DefectMaxVal = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMaxVal"].Value);
                            pdmNew.DefectMinVal = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMinVal"].Value);
                            pdmNew.VTSamplingSize = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["VTSamplingSize"].Value);
                            pdmNew.AQLID = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["AQLID"].Value);
                            pdmNew.QCTypeId = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["QCTypeId"].Value);
                            pdmNew.WTSamplingSize = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["WTSamplingSize"].Value);
                            pdmNew.CustomerTypeId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["CustomerTypeId"].Value);
                            if (!CheckDuplicate(pdmNew))
                            {
                                MasterTableBLL.SaveQITestResultAQL(pdmold, pdmNew, _loggedInUser);
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DUPLICATE_QITESTRESULTAQL, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                            return;
                        }
                        _lstQITestResultAQL = null;
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
            int i = 0;
            try
            {
                dgTableMaintenance.Rows.Insert(0, 1);
                dgTableMaintenance.Rows[0].ReadOnly = false;

                //comment out by MYAdamas -> Error when sorting after Add button is clicked
                //dgTableMaintenance["TestResultID", i].Value = string.Empty;
                //dgTableMaintenance["DefectMaxVal", i].Value = string.Empty;
                //dgTableMaintenance["DefectMinVal", i].Value = string.Empty;
                //dgTableMaintenance["WTSamplingSize", i].Value = string.Empty;

                DataGridViewComboBoxCell comboCell1 = dgTableMaintenance["AQLID", i] as DataGridViewComboBoxCell;
                comboCell1.DataSource = _QAIAQL;
                comboCell1.ValueMember = "IDField";
                comboCell1.DisplayMember = "DisplayField";
                comboCell1.Value = _QAIAQL.FirstOrDefault().IDField;
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
        private void dgTableMaintenance_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if ((dgTableMaintenance.CurrentCell.ColumnIndex == dgTableMaintenance.Columns["WTSamplingSize"].Index) ||
                (dgTableMaintenance.CurrentCell.ColumnIndex == dgTableMaintenance.Columns["VTSamplingSize"].Index) ||
                 (dgTableMaintenance.CurrentCell.ColumnIndex == dgTableMaintenance.Columns["DefectMaxVal"].Index) ||
                (dgTableMaintenance.CurrentCell.ColumnIndex == dgTableMaintenance.Columns["DefectMinVal"].Index))
            {
                TextBox ctrl = e.Control as TextBox;
                if (ctrl != null)
                {
                    ctrl.Sequence();
                }
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
                _lstQITestResultAQL = Sort(_lstQITestResultAQL, newColumn.Name, direction);
                BindGrid(LstQITestResultAQL);

                _pg = new PageOffsetList(Constants.TWENTY, _lstQITestResultAQL.Count);
                bindingNavigatorTable.BindingSource = bindingSourceTable;
                bindingSourceTable.DataSource = _pg.GetList();

                dgTableMaintenance.Columns[8].Name = "Edit";
                dgTableMaintenance.Columns[9].Name = "Delete";
            }
        }

        public List<QITestResultAQLDTO> Sort(List<QITestResultAQLDTO> input, string property, ListSortDirection orderSeq)
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

        private void BindGrid(List<QITestResultAQLDTO> prodefectmaster)
        {
            bindingSourceTable.CurrentChanged -= new System.EventHandler(bindingSource_CurrentChanged);
            dgTableMaintenance.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);

            dgTableMaintenance.AutoGenerateColumns = false;
            _pg = new PageOffsetList(Constants.TWENTY, LstQITestResultAQL.Count);
            bindingSourceTable.DataSource = _pg.GetList();
            bindingNavigatorTable.BindingSource = bindingSourceTable;
            populateGrid(prodefectmaster);
            bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
            dgTableMaintenance.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
        }

        private void populateGrid(List<QITestResultAQLDTO> prodefectmaster)
        {
            try
            {
                int i = 0;
                dgTableMaintenance.Rows.Clear();
                foreach (QITestResultAQLDTO dr in prodefectmaster)
                {
                    dgTableMaintenance.Rows.Add();
                    dgTableMaintenance.Rows[i].ReadOnly = true;

                    dgTableMaintenance["TestResultID", i].Value = dr.TestResultID;
                    dgTableMaintenance["DefectMaxVal", i].Value = dr.DefectMaxVal;
                    dgTableMaintenance["DefectMinVal", i].Value = dr.DefectMinVal;
                    dgTableMaintenance["WTSamplingSize", i].Value = dr.WTSamplingSize;
                    dgTableMaintenance["VTSamplingSize", i].Value = dr.VTSamplingSize;

                    DataGridViewComboBoxCell comboCell1 = dgTableMaintenance["AQLID", i] as DataGridViewComboBoxCell;
                    comboCell1.DataSource = _QAIAQL;
                    comboCell1.ValueMember = "IDField";
                    comboCell1.DisplayMember = "DisplayField";
                    comboCell1.Value = Convert.ToString(dr.AQLID);

                    DataGridViewComboBoxCell comboCell3 = dgTableMaintenance["QCTypeId", i] as DataGridViewComboBoxCell;
                    comboCell3.DataSource = _qCType;
                    comboCell3.ValueMember = "DisplayField";
                    comboCell3.DisplayMember = "IDField";
                    comboCell3.Value = Convert.ToString(dr.QCTypeId);

                    DataGridViewComboBoxCell comboCell4 = dgTableMaintenance["CustomerTypeId", i] as DataGridViewComboBoxCell;
                    comboCell4.DataSource = _customerType;
                    comboCell4.ValueMember = "IDField";
                    comboCell4.DisplayMember = "DisplayField";
                    comboCell4.Value = Convert.ToString(dr.CustomerTypeId);

                    i++;
                }
                dgTableMaintenance.Columns[8].Name = "Edit";
                dgTableMaintenance.Columns[9].Name = "Delete";
            }
            catch (Exception ex)
            {
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "populateGrid", null);
                return;
            }
        }

        private string Validate(string WTSamplingSize, string VTSamplingSize, string DefectMinValue, string DefectMaxValue)
        {
            string validamsg = string.Empty;

            if (string.IsNullOrEmpty(WTSamplingSize))
            {
                validamsg = "WTSamplingSize";
            }
            if (string.IsNullOrEmpty(VTSamplingSize))
            {
                validamsg = validamsg + Environment.NewLine + "Visual Test Sampling Size";
            }
            if (string.IsNullOrEmpty(DefectMinValue))
            {
                validamsg = validamsg + Environment.NewLine + "DefectMinValue";
            }
            if (string.IsNullOrEmpty(DefectMaxValue))
            {
                validamsg = validamsg + Environment.NewLine + "DefectMaxValue";
            }
            return validamsg;
        }

        private bool CheckDuplicate(QITestResultAQLDTO qrnew)
        {
            bool _isduplicate = false;

            int lstcount = (from p in LstQITestResultAQL
                            where p.DefectMaxVal == qrnew.DefectMaxVal && p.DefectMinVal == qrnew.DefectMinVal && p.WTSamplingSize == qrnew.WTSamplingSize
                                  && p.VTSamplingSize == qrnew.VTSamplingSize && p.IsDeleted == qrnew.IsDeleted && p.CustomerTypeId == qrnew.CustomerTypeId
                             && p.AQLID == qrnew.AQLID
                            select p).ToList<QITestResultAQLDTO>().Count;

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
