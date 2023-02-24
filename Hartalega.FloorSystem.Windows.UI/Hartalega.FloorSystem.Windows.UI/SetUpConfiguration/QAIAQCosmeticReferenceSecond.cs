using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Linq;
using System.ComponentModel;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name:QAIAQCosmeticReferenceSecond Maintenance
    /// File Type: Code file
    /// </summary> 
    public partial class QAIAQCosmeticReferenceSecond : Form
    {
        #region Member Variables
        private string _screenName = "Configuration SetUp - QAIAQCosmeticReferenceSecond";
        private string _className = "QAIAQCosmeticReferenceSecond";
        private List<DropdownDTO> _aqlIdList;
        private List<DropdownDTO> _qcTypeList;
        private List<DropdownDTO> _defectCategoryList;
        private List<DropdownDTO> _customerTypeList;
        private List<DropdownDTO> _moduleList;
        private bool _refresh = false;
        private PageOffsetList _pg;
        private DataTable _dtTableDetails;
        private DataGridViewRow _cell = null;
        private string _loggedInUser;
        private string _wsId;
        DataGridViewComboBoxCell _comboCellAQLId;
        DataGridViewComboBoxCell _comboCellQCType;
        DataGridViewComboBoxCell _comboCellDefectCategoryId;
        DataGridViewComboBoxCell _comboCellCustomerTypeId;
        DataGridViewComboBoxCell _comboCellModuleId;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public QAIAQCosmeticReferenceSecond(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
        }
        #endregion

        #region Event Handlers

        private void QAIAQCosmeticReferenceSecond_KeyDown(object sender, KeyEventArgs e)
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
                    if (dgTableMaintenance["ID", e.RowIndex].Value == null)
                    {
                        dgTableMaintenance.Rows.RemoveAt(0);
                        btnAdd.Enabled = true;
                        bindingNavigatorTable.Enabled = true;
                        dgTableMaintenance.Columns[12].Name = "Edit";
                    }
                    else
                    {
                        if (GlobalMessageBox.Show(Messages.DELETE_QAIAQCOSMETICREF, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                        {
                            try
                            {
                                QAIAQCosmeticReferenceSecondDTO qaicomesticnew = new QAIAQCosmeticReferenceSecondDTO();

                                qaicomesticnew = MasterTableBLL.GetQAIAQCosmeticReference().Where(p => p.Id == Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["Id"].Value)).FirstOrDefault();

                                qaicomesticnew.ActionType = Constants.ActionLog.Delete;
                                qaicomesticnew.IsDeleted = true;
                                QAIAQCosmeticReferenceSecondDTO qaicomesticold = MasterTableBLL.GetQAIAQCosmeticReference().Where(p => p.Id == qaicomesticnew.Id).FirstOrDefault();
                                //add by MYAdamas to compare two data table object for audit log

                                int rowsReturned = MasterTableBLL.DeleteQAIAQCosmeticRecord(dgTableMaintenance["ID", e.RowIndex].Value.ToString(), _wsId, _loggedInUser, qaicomesticnew, qaicomesticold);

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
                    bool isDuplicate = false;
                    QAIAQCosmeticReferenceSecondDTO aqlDTO = new QAIAQCosmeticReferenceSecondDTO();
                    aqlDTO.MajorDefectMinVal = Convert.ToString(dgTableMaintenance["MajorDefectMinValue", e.RowIndex].Value);
                    aqlDTO.MajorDefectMaxVal = Convert.ToString(dgTableMaintenance["MajorDefectMaxValue", e.RowIndex].Value);
                    aqlDTO.MinorDefectMinVal = Convert.ToString(dgTableMaintenance["MinorDefectMinValue", e.RowIndex].Value);
                    aqlDTO.MinorDefectMaxVal = Convert.ToString(dgTableMaintenance["MinorDefectMaxValue", e.RowIndex].Value);
                    aqlDTO.QCTypeOrder = Convert.ToString(dgTableMaintenance["QcTypeOrder", e.RowIndex].Value);
                    aqlDTO.WorkStationId = _wsId;
                    aqlDTO.OperatorId = _loggedInUser;

                    if (dgTableMaintenance["ID", e.RowIndex].Value == null)
                        aqlDTO.Id = 0;
                    else
                        aqlDTO.Id = Convert.ToInt32(dgTableMaintenance["ID", e.RowIndex].Value);

                    if (dgTableMaintenance["AQLID", e.RowIndex].Value == null)
                    {
                        if (dgTableMaintenance["AQLIdDB", e.RowIndex].Value != null)
                            aqlDTO.AQLID = dgTableMaintenance["AQLIdDB", e.RowIndex].Value.ToString();
                    }
                    else
                        aqlDTO.AQLID = dgTableMaintenance["AQLID", e.RowIndex].Value.ToString();

                    if (dgTableMaintenance["QCTypeId", e.RowIndex].Value == null)
                    {
                        if (dgTableMaintenance["QCTypeIdDB", e.RowIndex].Value != null)
                            aqlDTO.QCType = dgTableMaintenance["QCTypeIdDB", e.RowIndex].Value.ToString();
                    }
                    else
                        aqlDTO.QCType = dgTableMaintenance["QCTypeId", e.RowIndex].Value.ToString();



                    if (dgTableMaintenance["CustomerType", e.RowIndex].Value == null)
                    {
                        if (dgTableMaintenance["CustomerType", e.RowIndex].Value != null)
                            aqlDTO.CustomerTypeId = dgTableMaintenance["CustomerType", e.RowIndex].Value.ToString();
                    }
                    else
                        aqlDTO.CustomerTypeId = dgTableMaintenance["CustomerType", e.RowIndex].Value.ToString();

                    if (dgTableMaintenance["DefectCategoryGroup", e.RowIndex].Value == null)
                    {
                        if (dgTableMaintenance["DefectCategoryGroup", e.RowIndex].Value != null)
                            aqlDTO.DefectCategoryGroupId = dgTableMaintenance["DefectCategoryGroup", e.RowIndex].Value.ToString();
                    }
                    else
                        aqlDTO.DefectCategoryGroupId = dgTableMaintenance["DefectCategoryGroup", e.RowIndex].Value.ToString();


                    if (dgTableMaintenance["ModuleID", e.RowIndex].Value == null)
                    {
                        if (dgTableMaintenance["ModuleID", e.RowIndex].Value != null)
                            aqlDTO.EnumModuleId = dgTableMaintenance["ModuleID", e.RowIndex].Value.ToString();
                    }
                    else
                        aqlDTO.EnumModuleId = dgTableMaintenance["ModuleID", e.RowIndex].Value.ToString();

                    string validationMessage = ValidateRequiredFields(aqlDTO);

                    //MYAdamas 20171011 split to two function as must validate required only validate input field due to if empty and click save have conversion error when compare validate input

                    if (validationMessage == Messages.REQUIREDFIELDMESSAGE)
                    {
                        try
                        {
                            string validateInputField = ValidateInputValues(aqlDTO);
                            if (string.Compare(validateInputField, string.Empty) == Constants.ZERO)
                            {
                                isDuplicate = Convert.ToBoolean(MasterTableBLL.IsQAIAQCosmeticRefDuplicate(aqlDTO));
                                if (!isDuplicate)
                                {
                                    QAIAQCosmeticReferenceSecondDTO aqlDTOold = new QAIAQCosmeticReferenceSecondDTO();

                                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert")
                                        aqlDTO.ActionType = Constants.ActionLog.Add;
                                    else
                                    {
                                        aqlDTO.ActionType = Constants.ActionLog.Update;
                                        aqlDTOold = MasterTableBLL.GetQAIAQCosmeticReference().Where(p => p.Id == aqlDTO.Id).FirstOrDefault();
                                        //to set to same so when update can compare in audit log if not currently is empty
                                        aqlDTOold.WorkStationId = aqlDTO.WorkStationId;
                                        aqlDTOold.OperatorId = aqlDTO.OperatorId;

                                    }

                                    int rowsReturned = MasterTableBLL.UpdateQAIAQCosmeticRefDetails(aqlDTO, aqlDTOold, _loggedInUser);
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
                                    GlobalMessageBox.Show(Messages.DUPLICATE_QAIAQCOSMETICREF, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(validateInputField, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
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
                        if (validationMessage != Messages.REQUIREDFIELDMESSAGE)
                            GlobalMessageBox.Show(validationMessage, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        // else if(!string.IsNullOrEmpty(validateInputField))
                        //    GlobalMessageBox.Show(validateInputField, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
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
            dgTableMaintenance[12, 0].Style.NullValue = "Insert";
            dgTableMaintenance.Columns[12].Name = "Insert";
            _cell = dgTableMaintenance.Rows[0];
            dgTableMaintenance.Columns[13].Name = "Cancel";
            dgTableMaintenance[13, 0].Style.NullValue = "Cancel";
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
            // {
            //    datagridview.BeginEdit(true);
            //     ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            // }
        }

        /// <summary>
        /// dgTableMaintenance_EditingControlShowing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgTableMaintenance_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgTableMaintenance.CurrentCell.OwningColumn.CellType == typeof(DataGridViewTextBoxCell))
            {
                e.Control.KeyPress += new KeyPressEventHandler(dgTableMaintenance_KeyPress);
            }
        }

        /// <summary>
        /// Event to handle only integers in numeric fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgTableMaintenance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
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

                dgTableMaintenance.Columns[12].Name = "Edit";
                dgTableMaintenance.Columns[13].Name = "Delete";
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
        private string ValidateRequiredFields(QAIAQCosmeticReferenceSecondDTO aqlDTO)
        {
            StringBuilder requiredFieldMessage = new StringBuilder(Messages.REQUIREDFIELDMESSAGE);
            if (string.IsNullOrEmpty(aqlDTO.AQLID))
                requiredFieldMessage.AppendLine("AQL Name");
            if (string.IsNullOrEmpty(aqlDTO.QCType))
                requiredFieldMessage.AppendLine("QC Type");
            if (string.IsNullOrEmpty(aqlDTO.MajorDefectMinVal.Trim()))
                requiredFieldMessage.AppendLine("Major Defect Min Value");
            if (string.IsNullOrEmpty(aqlDTO.MajorDefectMaxVal.Trim()))
                requiredFieldMessage.AppendLine("Major Defect Max Value");
            if (string.IsNullOrEmpty(aqlDTO.MinorDefectMinVal.Trim()))
                requiredFieldMessage.AppendLine("Minor Defect Min Value");
            if (string.IsNullOrEmpty(aqlDTO.MinorDefectMaxVal.Trim()))
                requiredFieldMessage.AppendLine("Minor Defect Max Value");
            if (string.IsNullOrEmpty(aqlDTO.QCTypeOrder.Trim()))
                requiredFieldMessage.AppendLine("QC Type Order");
            if (string.IsNullOrEmpty(aqlDTO.CustomerTypeId))
                requiredFieldMessage.AppendLine("Customer Type");
            if (string.IsNullOrEmpty(aqlDTO.DefectCategoryGroupId))
                requiredFieldMessage.AppendLine("Cosmetic Defect Category");
            if (string.IsNullOrEmpty(aqlDTO.EnumModuleId))
                requiredFieldMessage.AppendLine("Module");
            return requiredFieldMessage.ToString();
        }

        private string ValidateInputValues(QAIAQCosmeticReferenceSecondDTO aqlDTO)
        {
            StringBuilder invalidFieldMessage = new StringBuilder(string.Empty);
            if (Convert.ToInt32(aqlDTO.MajorDefectMinVal) > Convert.ToInt32(aqlDTO.MajorDefectMaxVal))
            {
                invalidFieldMessage.AppendLine(Messages.MAJOR_DEFECT_RANGE);
            }
            if (Convert.ToInt32(aqlDTO.MinorDefectMinVal) > Convert.ToInt32(aqlDTO.MinorDefectMaxVal))
            {
                invalidFieldMessage.AppendLine(Messages.MINOR_DEFECT_RANGE);
            }
            return invalidFieldMessage.ToString();
        }

        /// <summary>
        /// Populate grid
        /// </summary>
        private void PopulateGrid()
        {
            try
            {
                _aqlIdList = MasterTableBLL.GetQAIAQL();
                _qcTypeList = CommonBLL.GetQCType();
                _defectCategoryList = CommonBLL.GetEnumText(Constants.DEFECT_CATEGORY_TYPE);
                _customerTypeList = CommonBLL.GetEnumText(Constants.CUSTOMER_TYPE);
                _moduleList = CommonBLL.GetEnumText(Constants.MODULE_SCREEN_AQL);
                _dtTableDetails = MasterTableBLL.GetQAIAQCosmeticReferenceDetails();
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
                dgTableMaintenance.Columns[12].Name = "Edit";
                dgTableMaintenance.Columns[13].Name = "Delete";
            }
        }

        /// <summary>
        /// Populate comboboxes
        /// </summary>
        private void PopulateComboBoxes(int rowIndex)
        {
            try
            {
                _comboCellAQLId = dgTableMaintenance["AQLID", rowIndex] as DataGridViewComboBoxCell;
                _comboCellAQLId.DataSource = new BindingSource(_aqlIdList, null);
                _comboCellAQLId.ValueMember = "IDField";
                _comboCellAQLId.DisplayMember = "DisplayField";
                _comboCellQCType = dgTableMaintenance["QCTypeId", rowIndex] as DataGridViewComboBoxCell;
                _comboCellQCType.DataSource = new BindingSource(_qcTypeList, null);
                _comboCellQCType.ValueMember = "DisplayField";
                _comboCellQCType.DisplayMember = "DisplayField";
                _comboCellDefectCategoryId = dgTableMaintenance["DefectCategoryGroup", rowIndex] as DataGridViewComboBoxCell;
                _comboCellDefectCategoryId.DataSource = new BindingSource(_defectCategoryList, null);
                _comboCellDefectCategoryId.ValueMember = "IDField";
                _comboCellDefectCategoryId.DisplayMember = "DisplayField";
                _comboCellCustomerTypeId = dgTableMaintenance["CustomerType", rowIndex] as DataGridViewComboBoxCell;
                _comboCellCustomerTypeId.DataSource = new BindingSource(_customerTypeList, null);
                _comboCellCustomerTypeId.ValueMember = "IDField";
                _comboCellCustomerTypeId.DisplayMember = "DisplayField";
                _comboCellModuleId = dgTableMaintenance["ModuleID", rowIndex] as DataGridViewComboBoxCell;
                _comboCellModuleId.DataSource = new BindingSource(_moduleList, null);
                _comboCellModuleId.ValueMember = "IDField";
                _comboCellModuleId.DisplayMember = "DisplayField";
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

                    dgTableMaintenance["MajorDefectMinValue", i].Value = dtTable.Rows[i]["MajorDefectMinValue"];
                    dgTableMaintenance["MajorDefectMaxValue", i].Value = dtTable.Rows[i]["MajorDefectMaxValue"];
                    PopulateComboBoxes(i);
                    _comboCellAQLId.Style.NullValue = dtTable.Rows[i]["AQLName"].ToString();
                    _comboCellQCType.Style.NullValue = dtTable.Rows[i]["QCTypeId"].ToString();
                    dgTableMaintenance["MinorDefectMinValue", i].Value = dtTable.Rows[i]["MinorDefectMinValue"];
                    dgTableMaintenance["MinorDefectMaxValue", i].Value = dtTable.Rows[i]["MinorDefectMaxValue"].ToString();
                    dgTableMaintenance["QcTypeOrder", i].Value = dtTable.Rows[i]["QcTypeOrder"].ToString();
                    dgTableMaintenance["AQLIDDB", i].Value = dtTable.Rows[i]["AQLID"].ToString();
                    dgTableMaintenance["QCTypeIdDB", i].Value = dtTable.Rows[i]["QCTypeId"].ToString();
                    dgTableMaintenance["ID", i].Value = dtTable.Rows[i]["ID"].ToString();

                    //if(dtTable.Rows[i]["CustomerType"] != null)
                    //    dgTableMaintenance["CustomerType", i].Value = dtTable.Rows[i]["CustomerType"].ToString();

                    //if (dtTable.Rows[i]["DefectCategoryGroup"] != null)
                    //    dgTableMaintenance["DefectCategoryGroup", i].Value = dtTable.Rows[i]["DefectCategoryGroup"].ToString();
                    //_comboCellCustomerTypeId.Style.NullValue = dtTable.Rows[i]["CustomerTypeId"].ToString();
                    //_comboCellDefectCategoryId.Style.NullValue = dtTable.Rows[i]["DefectCategoryGroupId"].ToString();


                    DataGridViewComboBoxCell comboCell3 = dgTableMaintenance["CustomerType", i] as DataGridViewComboBoxCell;
                    comboCell3.DataSource = _customerTypeList;
                    comboCell3.ValueMember = "IDField";
                    comboCell3.DisplayMember = "DisplayField";
                    comboCell3.Value = Convert.ToString(dtTable.Rows[i]["CustomerTypeId"].ToString());

                    DataGridViewComboBoxCell comboCell4 = dgTableMaintenance["DefectCategoryGroup", i] as DataGridViewComboBoxCell;
                    comboCell4.DataSource = _defectCategoryList;
                    comboCell4.ValueMember = "IDField";
                    comboCell4.DisplayMember = "DisplayField";
                    comboCell4.Value = Convert.ToString(dtTable.Rows[i]["DefectCategoryGroupId"].ToString());

                    DataGridViewComboBoxCell comboCell5 = dgTableMaintenance["ModuleID", i] as DataGridViewComboBoxCell;
                    comboCell5.DataSource = _moduleList;
                    comboCell5.ValueMember = "IDField";
                    comboCell5.DisplayMember = "DisplayField";
                    comboCell5.Value = Convert.ToString(dtTable.Rows[i]["EnumModuleId"].ToString());

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
