using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: DefectiveGlove Master Maintenance
    /// File Type: Code file
    /// </summary> 
    public partial class DefectiveGlove : Form
    {
        #region Member Variables
        private string _screenName = "Configuration SetUp - DefectiveGlove";
        private string _className = "DefectiveGlove";
        private bool _refresh = false;
        private PageOffsetList _pg;
        private DataTable _dtTableDetails;
        private DataGridViewRow _cell = null;
        private string _loggedInUser;
        private string _wsId;
        DefectGloveDTO oldObj;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public DefectiveGlove(string loggedInUser)
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
            this.dgTableMaintenance.AllowUserToDeleteRows = false;
            bindingSourceTable.CurrentChanged -= new System.EventHandler(bindingSource_CurrentChanged);
            dgTableMaintenance.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
            _refresh = false;
            PopulateGrid();
            _wsId = WorkStationDTO.GetInstance().WorkStationId;
            bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
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
                    bindingNavigatorTable.Enabled = false;
                    dgTableMaintenance.Rows[e.RowIndex].ReadOnly = false;
                    dgTableMaintenance.Columns[e.ColumnIndex].Name = "Update";
                    dgTableMaintenance[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                    _cell = dgTableMaintenance.CurrentRow;
                    dgTableMaintenance.Columns[e.ColumnIndex + 1].Name = "Cancel";
                    dgTableMaintenance[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";

                    oldObj = new DefectGloveDTO();
                    oldObj.defectiveGloveId = Convert.ToString(dgTableMaintenance["DefectiveGloveId", e.RowIndex].Value);
                    oldObj.defectiveGloveType = Convert.ToString(dgTableMaintenance["DefectiveGloveType", e.RowIndex].Value);
                    oldObj.defectiveGloveReason = Convert.ToString(dgTableMaintenance["DefectiveGloveReason", e.RowIndex].Value);
                    oldObj.tvConfigurable = Convert.ToString(dgTableMaintenance["TVConfigurable", e.RowIndex].Value);
                    oldObj.IsDeleted = false;
                }
                else if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Cancel" && dgTableMaintenance.CurrentRow == _cell)
                {
                    _refresh = true;
                    PopulateGrid();
                    dgTableMaintenance.Columns[e.ColumnIndex].Name = "Delete";
                }
                else if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Delete")
                {
                    if (dgTableMaintenance["DefectiveGloveId", e.RowIndex].Value == null)
                    {
                        dgTableMaintenance.Rows.RemoveAt(0);
                        btnAdd.Enabled = true;
                        bindingNavigatorTable.Enabled = true;
                        dgTableMaintenance.Columns[6].Name = "Edit";
                    }
                    else
                    {
                        if (GlobalMessageBox.Show(Messages.DELETE_DEFECTIVE_GLOVE_REASON, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                        {
                            try
                            {
                                DefectGloveDTO newObj = new DefectGloveDTO();
                                newObj.defectiveGloveId = Convert.ToString(dgTableMaintenance["DefectiveGloveId", e.RowIndex].Value);
                                newObj.IsDeleted = true;
                                int rowsReturned = MasterTableBLL.DeleteDefectiveGloveRecord(dgTableMaintenance["DefectiveGloveId", e.RowIndex].Value.ToString(), _wsId, _loggedInUser, prepareAuditLog(oldObj,newObj,3));
                                if (rowsReturned > 0)
                                {
                                    GlobalMessageBox.Show(Messages.DELETE_RECORD, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    _refresh = true;
                                    PopulateGrid();
                                    oldObj = null;
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

                    int auditAction = 0;
                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert")
                        auditAction = 1;
                    else auditAction = 2;

                    DefectGloveDTO newObj = new DefectGloveDTO();
                    int defectiveGloveId;
                    bool isDuplicate = false;
                    bool tvConfigurable;

                    string defectiveGloveType = Convert.ToString(dgTableMaintenance["DefectiveGloveType", e.RowIndex].Value);
                    string defectiveGloveReason = Convert.ToString(dgTableMaintenance["DefectiveGloveReason", e.RowIndex].Value);
                    newObj.defectiveGloveType = defectiveGloveType;
                    newObj.defectiveGloveReason = defectiveGloveReason;

                    if (dgTableMaintenance["DefectiveGloveId", e.RowIndex].Value == null)
                        defectiveGloveId = 0;
                    else
                    {
                        defectiveGloveId = Convert.ToInt32(dgTableMaintenance["DefectiveGloveId", e.RowIndex].Value);
                        newObj.defectiveGloveId = Convert.ToString(dgTableMaintenance["DefectiveGloveId", e.RowIndex].Value);
                    }

                    if (dgTableMaintenance["TVConfigurable", e.RowIndex].Value == null)
                        tvConfigurable = false;
                    else
                    {
                        tvConfigurable = Convert.ToBoolean(dgTableMaintenance["TVConfigurable", e.RowIndex].Value);
                        newObj.tvConfigurable = Convert.ToString(dgTableMaintenance["TVConfigurable", e.RowIndex].Value);
                    }


                        string validationMessage = ValidateRequiredFields(defectiveGloveType, defectiveGloveReason);
                    if (validationMessage == Messages.REQUIREDFIELDMESSAGE)
                    {
                        try
                        {
                            if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert" ||
                                (string.Compare(defectiveGloveReason, dgTableMaintenance["DefectiveGloveReasonDB", e.RowIndex].Value.ToString()) != Constants.ZERO) ||
                                (string.Compare(defectiveGloveType, dgTableMaintenance["DefectiveGloveTypeDB", e.RowIndex].Value.ToString()) != Constants.ZERO))
                                isDuplicate = Convert.ToBoolean(MasterTableBLL.IsDefectiveGloveReasonDuplicate(defectiveGloveType, defectiveGloveReason));

                            if (!isDuplicate)
                            {
                                int rowsReturned = MasterTableBLL.UpdateDefectiveGloveDetails(defectiveGloveId, defectiveGloveType,
                                                                                               defectiveGloveReason, tvConfigurable, _loggedInUser, _wsId,prepareAuditLog(oldObj,newObj,auditAction));
                                if (rowsReturned > 0)
                                {
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    _refresh = true;
                                    PopulateGrid();
                                    oldObj = null;
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DUPLICATE_DEFECTIVE_GLOVE_REASON, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
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
            dgTableMaintenance[6, 0].Style.NullValue = "Insert";
            dgTableMaintenance.Columns[6].Name = "Insert";
            _cell = dgTableMaintenance.Rows[0];
            dgTableMaintenance.Columns[7].Name = "Cancel";
            dgTableMaintenance[7, 0].Style.NullValue = "Cancel";
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
            catch (Exception ex)
            {
                return;
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
        private string ValidateRequiredFields(string defectiveGloveType, string defectiveGloveReason)
        {
            StringBuilder requiredFieldMessage = new StringBuilder(Messages.REQUIREDFIELDMESSAGE);
            if (string.IsNullOrEmpty(defectiveGloveType.Trim()))
                requiredFieldMessage.AppendLine("Defective Glove Type");
            if (string.IsNullOrEmpty(defectiveGloveReason.Trim()))
                requiredFieldMessage.AppendLine("Defective Glove Reason");
            return requiredFieldMessage.ToString();
        }

        /// <summary>
        /// Fill Grid
        /// </summary>
        private void PopulateGrid()
        {
            try
            {
                _dtTableDetails = MasterTableBLL.GetDefectiveGloveDetails();
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
                dgTableMaintenance.Columns[6].Name = "Edit";
                dgTableMaintenance.Columns[7].Name = "Delete";
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

                    dgTableMaintenance["DefectiveGloveType", i].Value = dtTable.Rows[i]["DefectiveGloveType"];
                    dgTableMaintenance["DefectiveGloveReason", i].Value = dtTable.Rows[i]["DefectiveGloveReason"];
                    dgTableMaintenance["TVConfigurable", i].Value = dtTable.Rows[i]["TVConfigurable"];
                    dgTableMaintenance["DefectiveGloveId", i].Value = dtTable.Rows[i]["DefectiveGloveId"];
                    dgTableMaintenance["DefectiveGloveReasonDB", i].Value = dtTable.Rows[i]["DefectiveGloveReason"];
                    dgTableMaintenance["DefectiveGloveTypeDB", i].Value = dtTable.Rows[i]["DefectiveGloveType"];
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                dgTableMaintenance.DataSource = null;
            }
        }

        private string prepareAuditLog(DefectGloveDTO oldObject,DefectGloveDTO newObject, int action)
        {
            string operation = "";
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = _screenName;
            AuditLog.CreatedBy = _loggedInUser;
            if (action == 1) operation = Constants.ActionLog.Add.ToString();
            else if(action==2) operation = Constants.ActionLog.Update.ToString(); 
            else operation = Constants.ActionLog.Delete.ToString();
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), operation);

            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "DefectiveGlove";
            string refid = "";

            if (audAction != Constants.ActionLog.Add) 
            {
                refid = newObject.defectiveGloveId;
                AuditLog.ReferenceId = refid;
            }
            if (oldObject == null)
            {
                oldObject = new DefectGloveDTO();
                oldObject.defectiveGloveId = newObject.defectiveGloveId;
                AuditLog.UpdateColumns = oldObject.DetailedCompare(newObject).GetPropChanges();
            }
            else
                AuditLog.UpdateColumns = oldObject.DetailedCompare(newObject).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            return audlog;
        }
        #endregion

        public class DefectGloveDTO
        {
            public string defectiveGloveId { get; set; }
            public string defectiveGloveType { get; set; }
            public string defectiveGloveReason { get; set; }
            public string tvConfigurable { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
}