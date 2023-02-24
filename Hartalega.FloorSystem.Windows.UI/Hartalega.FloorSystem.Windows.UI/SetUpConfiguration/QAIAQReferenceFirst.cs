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
    public partial class QAIAQReferenceFirst : FormBase
    {
        #region Member Variables
        private string _screenName = "Configuration SetUp - QAIAQReferenceFirst";
        private string _className = "QAIAQReferenceFirst";
        private int _prevRowindex = -1;
        private PageOffsetList _pg;
        private List<QAIAQReferenceFirstDTO> _qaiAQReferenceFirstlst;
        public List<QAIAQReferenceFirstDTO> QaiAQReferenceFirstlst
        {
            get
            {
                if (_qaiAQReferenceFirstlst == null)
                {
                    _qaiAQReferenceFirstlst = MasterTableBLL.GetQaiAQReferenceFirst();
                    return _qaiAQReferenceFirstlst;
                }
                else
                {
                    return _qaiAQReferenceFirstlst;
                }
            }
        }
        private List<DropdownDTO> _defectCategoryTypeList;
        private List<DropdownDTO> _QAIAQL;
        private List<DropdownDTO> _qCType;
        private List<DropdownDTO> _customerType;
        private bool _isUpdate = false;
        private string _loggedInUser;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public QAIAQReferenceFirst(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
            _defectCategoryTypeList = SetUpConfigurationBLL.GetQAIDefectCategoryTypeList();
            _QAIAQL = MasterTableBLL.GetQAIAQL();
            _qCType = CommonBLL.GetQCType();
            _customerType = CommonBLL.GetEnumText(Constants.CUSTOMER_TYPE);
        }

        /// <summary>
        /// Event Handler for Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAIAQReferenceFirst_Load(object sender, EventArgs e)
        {
            List<QAIAQReferenceFirstDTO> lstQAIAQReferenceFirstDTO = QaiAQReferenceFirstlst.GetRange(0, MasterTableBLL.GetRange(0, QaiAQReferenceFirstlst.Count));
            BindGrid(lstQAIAQReferenceFirstDTO);
        }

        #endregion

        #region Event Handlers
        private void QAIAQReferenceFirst_KeyDown(object sender, KeyEventArgs e)
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
                List<QAIAQReferenceFirstDTO> lstQAIAQReferenceFirstDTO = QaiAQReferenceFirstlst.GetRange(offset, MasterTableBLL.GetRange(offset, QaiAQReferenceFirstlst.Count));
                BindGrid(lstQAIAQReferenceFirstDTO);
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
                            QAIAQReferenceFirstDTO pdmNew = new QAIAQReferenceFirstDTO();
                            pdmNew.RefID = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["RefID"].Value);
                            pdmNew.ActionType = Constants.ActionLog.Update;
                            QAIAQReferenceFirstDTO pdmold = MasterTableBLL.GetQaiAQReferenceFirst().Where(p => p.RefID == pdmNew.RefID).FirstOrDefault();
                            string vldmsg = Validate(Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["WTSamplingSize"].Value),
                                            Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["VTSamplingSize"].Value),
                                            Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMinValue"].Value),
                                            Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMaxValue"].Value),
                                            Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["ResampleRound"].Value));
                            if (string.IsNullOrEmpty(vldmsg))
                            {
                                pdmNew.WTSamplingSize = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["WTSamplingSize"].Value);
                                pdmNew.VTSamplingSize = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["VTSamplingSize"].Value);
                                pdmNew.AQLID = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["AQLID"].Value);
                                pdmNew.QCTypeId = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["QCTypeId"].Value);
                                pdmNew.DefectMinValue = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMinValue"].Value);
                                pdmNew.DefectMaxValue = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMaxValue"].Value);
                                pdmNew.IsResample = Convert.ToBoolean(dgTableMaintenance.Rows[e.RowIndex].Cells["IsResample"].Value);
                                pdmNew.ResampleRound = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["ResampleRound"].Value);
                                pdmNew.CustomerTypeId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["CustomerTypeId"].Value);
                                // set defect category type id new=old
                                pdmNew.DefectCategoryTypeId = pdmold.DefectCategoryTypeId;

                                //changes by MYAdamas 20171016 edit issue
                                //if (!CheckDuplicate(pdmNew))
                                if (!CheckDuplicateForEdit(pdmNew))
                                {
                                    MasterTableBLL.SaveQAIReferenceFirst(pdmold, pdmNew, _loggedInUser);
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DUPLICATE_QAIAQREFERENCEFIRST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                                return;
                            }
                            _qaiAQReferenceFirstlst = null;
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
                            dgTableMaintenance.Columns[e.ColumnIndex].Name = "Update";
                            dgTableMaintenance[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                            dgTableMaintenance.Columns[e.ColumnIndex + 1].Name = "Cancel";
                            dgTableMaintenance[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";
                        }
                        return;
                    }

                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Delete")
                    {
                        btnAdd.Enabled = false;
                        if (GlobalMessageBox.Show(Messages.DELETE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                        {
                            QAIAQReferenceFirstDTO pdmNew = new QAIAQReferenceFirstDTO();
                            pdmNew = MasterTableBLL.GetQaiAQReferenceFirst().Where(p => p.RefID == Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["RefID"].Value)).FirstOrDefault();
                            pdmNew.ActionType = Constants.ActionLog.Delete;
                            pdmNew.IsDeleted = true;
                            QAIAQReferenceFirstDTO pdmold = MasterTableBLL.GetQaiAQReferenceFirst().Where(p => p.RefID == pdmNew.RefID).FirstOrDefault();
                            MasterTableBLL.SaveQAIReferenceFirst(pdmold, pdmNew, _loggedInUser);
                            _qaiAQReferenceFirstlst = null;
                            bindingSource_CurrentChanged(null, null);
                        }
                        return;
                    }

                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert")
                    {
                        QAIAQReferenceFirstDTO pdmNew = new QAIAQReferenceFirstDTO();
                        QAIAQReferenceFirstDTO pdmold = new QAIAQReferenceFirstDTO();
                        pdmNew.ActionType = Constants.ActionLog.Add;
                        string vldmsg = Validate(Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["WTSamplingSize"].Value),
                                                    Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["VTSamplingSize"].Value),
                                                    Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMinValue"].Value),
                                                    Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMaxValue"].Value),
                                                    Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["ResampleRound"].Value));
                        if (string.IsNullOrEmpty(vldmsg))
                        {
                            pdmNew.WTSamplingSize = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["WTSamplingSize"].Value);
                            pdmNew.VTSamplingSize = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["VTSamplingSize"].Value);
                            pdmNew.AQLID = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["AQLID"].Value);
                            pdmNew.QCTypeId = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["QCTypeId"].Value);
                            pdmNew.DefectMinValue = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMinValue"].Value);
                            pdmNew.DefectMaxValue = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectMaxValue"].Value);
                            pdmNew.IsResample = Convert.ToBoolean(dgTableMaintenance.Rows[e.RowIndex].Cells["IsResample"].Value);
                            pdmNew.ResampleRound = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["ResampleRound"].Value);
                            pdmNew.CustomerTypeId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["CustomerTypeId"].Value);
                            // add category id to check duplicate MYAdamas 20171109
                            pdmNew.DefectCategoryTypeId = Convert.ToInt32(MasterTableBLL.GetDefectCategoryTypeId());

                            if (!CheckDuplicate(pdmNew))
                            {
                                MasterTableBLL.SaveQAIReferenceFirst(pdmold, pdmNew, _loggedInUser);
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DUPLICATE_QAIAQREFERENCEFIRST, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                            return;
                        }
                        _qaiAQReferenceFirstlst = null;
                        bindingSource_CurrentChanged(null, null);
                        return;
                    }
                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Cancel")
                    {
                        btnAdd.Enabled = true;
                        _prevRowindex = -1;
                        bindingSource_CurrentChanged(null, null);
                        return;
                    }
                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "IsResample" && (dgTableMaintenance.Columns[e.ColumnIndex + 2].Name == "Update" || dgTableMaintenance.Columns[e.ColumnIndex + 2].Name == "Insert"))
                    {
                        dgTableMaintenance.Rows[e.RowIndex].ReadOnly = false;
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
                // comment out by MYAdamas -> error when sorting after Add button is clicked
                //dgTableMaintenance["RefID", i].Value = string.Empty;
                //dgTableMaintenance["WTSamplingSize", i].Value = string.Empty;
                //dgTableMaintenance["VTSamplingSize", i].Value = string.Empty;
                //dgTableMaintenance["DefectMinValue", i].Value = string.Empty;
                //dgTableMaintenance["DefectMaxValue", i].Value = string.Empty;
                //dgTableMaintenance["ResampleRound", i].Value = string.Empty;

                DataGridViewCheckBoxCell chekCell1 = dgTableMaintenance["IsResample", i] as DataGridViewCheckBoxCell;
                chekCell1.Value = false;

                DataGridViewComboBoxCell comboCell2 = dgTableMaintenance["AQLID", i] as DataGridViewComboBoxCell;
                comboCell2.DataSource = _QAIAQL;
                comboCell2.ValueMember = "IDField";
                comboCell2.DisplayMember = "DisplayField";
                comboCell2.Value = _QAIAQL.FirstOrDefault().IDField;

                DataGridViewComboBoxCell comboCell3 = dgTableMaintenance["QCTypeId", i] as DataGridViewComboBoxCell;
                comboCell3.DataSource = _qCType;
                comboCell3.ValueMember = "DisplayField";
                comboCell3.DisplayMember = "IDField";
                comboCell3.Value = _qCType.FirstOrDefault().DisplayField;

                DataGridViewComboBoxCell comboCell4 = dgTableMaintenance["CustomerTypeId", i] as DataGridViewComboBoxCell;
                comboCell4.DataSource = _customerType;
                comboCell4.ValueMember = "IDField";
                comboCell4.DisplayMember = "DisplayField";
                comboCell4.Value = _customerType.FirstOrDefault().IDField;

                dgTableMaintenance.Columns[10].Name = "Insert";
                dgTableMaintenance[10, 0].Style.NullValue = "Insert";
                dgTableMaintenance.Columns[11].Name = "Cancel";
                dgTableMaintenance[11, 0].Style.NullValue = "Cancel";
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
            validationMesssageLst = new List<ValidationMessage>();
            if ((dgTableMaintenance.CurrentCell.ColumnIndex == dgTableMaintenance.Columns["WTSamplingSize"].Index) ||
                 (dgTableMaintenance.CurrentCell.ColumnIndex == dgTableMaintenance.Columns["VTSamplingSize"].Index) ||
                (dgTableMaintenance.CurrentCell.ColumnIndex == dgTableMaintenance.Columns["DefectMinValue"].Index) ||
                (dgTableMaintenance.CurrentCell.ColumnIndex == dgTableMaintenance.Columns["DefectMaxValue"].Index) ||
                (dgTableMaintenance.CurrentCell.ColumnIndex == dgTableMaintenance.Columns["ResampleRound"].Index))
            {
                TextBox ctrl = e.Control as TextBox;
                if (ctrl != null)
                {
                    ctrl.Sequence();
                    if (ctrl.Name == "WTSamplingSize")
                    {
                        validationMesssageLst.Add(new ValidationMessage(ctrl, "QAI Inspector:", ValidationType.Required));
                    }
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
                _qaiAQReferenceFirstlst = Sort(_qaiAQReferenceFirstlst, newColumn.Name, direction);
                BindGrid(_qaiAQReferenceFirstlst);

                _pg = new PageOffsetList(Constants.TWENTY, _qaiAQReferenceFirstlst.Count);
                bindingNavigatorTable.BindingSource = bindingSourceTable;
                bindingSourceTable.DataSource = _pg.GetList();

                dgTableMaintenance.Columns[10].Name = "Edit";
                dgTableMaintenance.Columns[11].Name = "Delete";
            }
        }

        public List<QAIAQReferenceFirstDTO> Sort(List<QAIAQReferenceFirstDTO> input, string property, ListSortDirection orderSeq)
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

        private string Validate(string WTSamplingSize, string VTSamplingSize, string DefectMinValue, string DefectMaxValue, string ResampleRound)
        {
            string validamsg = string.Empty;

            if (string.IsNullOrEmpty(WTSamplingSize))
            {
                validamsg = "Water Tight Sampling Size";
            }
            if (string.IsNullOrEmpty(VTSamplingSize))
            {
                validamsg = validamsg + Environment.NewLine + "Visual Test Sampling Size";
            }
            if (string.IsNullOrEmpty(DefectMinValue))
            {
                validamsg = validamsg + Environment.NewLine + "Minimum Value";
            }
            if (string.IsNullOrEmpty(DefectMaxValue))
            {
                validamsg = validamsg + Environment.NewLine + "Maximum Value";
            }
            if (string.IsNullOrEmpty(ResampleRound))
            {
                validamsg = validamsg + Environment.NewLine + "Resample Round";
            }
            return validamsg;
        }

        /// <summary>
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void BindGrid(List<QAIAQReferenceFirstDTO> prodefectmaster)
        {
            bindingSourceTable.CurrentChanged -= new System.EventHandler(bindingSource_CurrentChanged);
            dgTableMaintenance.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
            dgTableMaintenance.AutoGenerateColumns = false;
            _pg = new PageOffsetList(Constants.TWENTY, QaiAQReferenceFirstlst.Count);
            bindingSourceTable.DataSource = _pg.GetList();
            bindingNavigatorTable.BindingSource = bindingSourceTable;
            populateGrid(prodefectmaster);
            bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
            dgTableMaintenance.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
        }

        private void populateGrid(List<QAIAQReferenceFirstDTO> prodefectmaster)
        {
            try
            {
                int i = 0;
                dgTableMaintenance.Rows.Clear();
                foreach (QAIAQReferenceFirstDTO dr in prodefectmaster)
                {
                    dgTableMaintenance.Rows.Add();
                    dgTableMaintenance.Rows[i].ReadOnly = true;

                    dgTableMaintenance["RefID", i].Value = dr.RefID;
                    dgTableMaintenance["WTSamplingSize", i].Value = dr.WTSamplingSize;
                    dgTableMaintenance["VTSamplingSize", i].Value = dr.VTSamplingSize;
                    dgTableMaintenance["DefectMinValue", i].Value = dr.DefectMinValue;
                    dgTableMaintenance["DefectMaxValue", i].Value = dr.DefectMaxValue;
                    dgTableMaintenance["ResampleRound", i].Value = dr.ResampleRound;

                    DataGridViewCheckBoxCell chekCell1 = dgTableMaintenance["IsResample", i] as DataGridViewCheckBoxCell;
                    chekCell1.Value = dr.IsResample;

                    DataGridViewComboBoxCell comboCell2 = dgTableMaintenance["AQLID", i] as DataGridViewComboBoxCell;
                    comboCell2.DataSource = _QAIAQL;
                    comboCell2.ValueMember = "IDField";
                    comboCell2.DisplayMember = "DisplayField";
                    comboCell2.Value = Convert.ToString(dr.AQLID);

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
                dgTableMaintenance.Columns[10].Name = "Edit";
                dgTableMaintenance.Columns[11].Name = "Delete";
            }
            catch (Exception ex)
            {
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "populateGrid", null);
                return;
            }
        }

        private bool CheckDuplicate(QAIAQReferenceFirstDTO qainew)
        {
            bool _isduplicate = false;

            int lstcount = (from p in QaiAQReferenceFirstlst
                            where p.DefectMaxValue == qainew.DefectMaxValue && p.DefectMinValue == qainew.DefectMinValue && p.AQLID == qainew.AQLID && p.DefectCategoryTypeId == qainew.DefectCategoryTypeId
                            && p.VTSamplingSize == qainew.VTSamplingSize && p.WTSamplingSize == qainew.WTSamplingSize && p.IsDeleted == qainew.IsDeleted && p.CustomerTypeId == qainew.CustomerTypeId
                            && p.IsResample == qainew.IsResample && p.ResampleRound == qainew.ResampleRound
                            select p).ToList<QAIAQReferenceFirstDTO>().Count;

            if (lstcount > 0)
            {
                _isduplicate = true;
            }
            return _isduplicate;
        }

        // edit not sucess due to keep prompt out duplicate record 
        private bool CheckDuplicateForEdit(QAIAQReferenceFirstDTO qainew)
        {
            bool _isduplicate = false;

            int lstcount = (from p in QaiAQReferenceFirstlst
                            where p.DefectMaxValue == qainew.DefectMaxValue && p.DefectMinValue == qainew.DefectMinValue && p.AQLID == qainew.AQLID && p.DefectCategoryTypeId == qainew.DefectCategoryTypeId
                            && p.VTSamplingSize == qainew.VTSamplingSize && p.WTSamplingSize == qainew.WTSamplingSize && p.IsDeleted == qainew.IsDeleted && p.CustomerTypeId == qainew.CustomerTypeId
                            && p.IsResample == qainew.IsResample && p.ResampleRound == qainew.ResampleRound
                            && p.RefID != qainew.RefID
                            select p).ToList<QAIAQReferenceFirstDTO>().Count;

            if (lstcount > 0)
            {
                _isduplicate = true;
            }
            return _isduplicate;
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
