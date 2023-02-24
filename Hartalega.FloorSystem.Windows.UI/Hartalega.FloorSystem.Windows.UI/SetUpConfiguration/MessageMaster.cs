using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.ComponentModel;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Message Master Maintenance
    /// File Type: Code file
    /// </summary> 
    public partial class MessageMaster : Form
    {
        #region Member Variables
        private string _screenName = "Configuration SetUp - MessageMaster";
        private string _className = "MessageMaster";
        private bool _refresh = false;
        private PageOffsetList _pg;
        private DataTable _dtTableDetails;
        private DataGridViewRow _cell = null;
        private string _loggedInUser;
        private string _wsId;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public MessageMaster(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
        }
        #endregion

        #region Event Handlers
        private void MessageMaster_KeyDown(object sender, KeyEventArgs e)
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
                    dgTableMaintenance["MessageKey", e.RowIndex].ReadOnly = true;
                    dgTableMaintenance.Columns["Cancel"].Visible = true;
                    dgTableMaintenance["Cancel", e.RowIndex].Style.NullValue = "Cancel";
                }
                else if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Cancel" && dgTableMaintenance.CurrentRow == _cell)
                {
                    _refresh = true;
                    PopulateGrid();
                }
                else if ((dgTableMaintenance.Columns[e.ColumnIndex].Name == "Update" && dgTableMaintenance.CurrentRow == _cell) ||
                         dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert" && dgTableMaintenance.CurrentRow == _cell)
                {
                    int messageId;
                    bool isDuplicate = false;
                    string messageKey = Convert.ToString(dgTableMaintenance["MessageKey", e.RowIndex].Value);
                    string messageText = Convert.ToString(dgTableMaintenance["MessageText", e.RowIndex].Value);

                    if (dgTableMaintenance["MessageId", e.RowIndex].Value == null)
                        messageId = 0;
                    else
                        messageId = Convert.ToInt32(dgTableMaintenance["MessageId", e.RowIndex].Value);

                    string validationMessage = ValidateRequiredFields(messageKey, messageText);
                    if (validationMessage == Messages.REQUIREDFIELDMESSAGE)
                    {
                        try
                        {
                            if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert")
                                isDuplicate = Convert.ToBoolean(MasterTableBLL.IsMessageKeyDuplicate(messageKey));
                            if (!isDuplicate)
                            {
                                MessagesDTO msgNew = new MessagesDTO();

                                msgNew.MessageKey = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["MessageKey"].Value);
                                msgNew.MessageText = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["MessageText"].Value);

                                MessagesDTO msgOld = new MessagesDTO();

                                if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert")
                                    msgNew.ActionType = Constants.ActionLog.Add;
                                else
                                {
                                    msgNew.ActionType = Constants.ActionLog.Update;
                                    msgOld = MasterTableBLL.GetMessageMaster().Where(p => p.Id == msgNew.Id).FirstOrDefault();
                                }


                                int rowsReturned = MasterTableBLL.UpdateMessageMasterDetails(messageId, messageKey, messageText, _loggedInUser, _wsId, msgNew, msgOld);
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
                                GlobalMessageBox.Show(Messages.DUPLICATE_MESSAGE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
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
            dgTableMaintenance["MessageKey", 0].ReadOnly = false;
            dgTableMaintenance[3, 0].Style.NullValue = "Insert";
            dgTableMaintenance.Columns[3].Name = "Insert";
            _cell = dgTableMaintenance.Rows[0];
            dgTableMaintenance.Columns["Cancel"].Visible = true;
            dgTableMaintenance["Cancel", 0].Style.NullValue = "Cancel";
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
            //{
            //    datagridview.BeginEdit(true);
            //    ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            //}
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
                    case "MessageId":
                        dv.Sort = "Id" + (direction == ListSortDirection.Ascending ? " ASC" : " DESC");
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

                dgTableMaintenance.Columns[3].Name = "Edit";
                dgTableMaintenance.Columns["Cancel"].Visible = false;
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
        private string ValidateRequiredFields(string messageKey, string messageText)
        {
            StringBuilder requiredFieldMessage = new StringBuilder(Messages.REQUIREDFIELDMESSAGE);
            if (string.IsNullOrEmpty(messageKey.Trim()))
                requiredFieldMessage.AppendLine("Message Key");
            if (string.IsNullOrEmpty(messageText.Trim()))
                requiredFieldMessage.AppendLine("Message Text");
            return requiredFieldMessage.ToString();
        }

        /// <summary>
        /// Fill Grid
        /// </summary>
        private void PopulateGrid()
        {
            try
            {
                _dtTableDetails = MasterTableBLL.GetMessageMasterDetails();
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
                dgTableMaintenance.Columns[3].Name = "Edit";
                dgTableMaintenance.Columns["Cancel"].Visible = false;
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

                    dgTableMaintenance["MessageId", i].Value = dtTable.Rows[i]["Id"];
                    dgTableMaintenance["MessageKey", i].Value = dtTable.Rows[i]["MessageKey"];
                    dgTableMaintenance["MessageText", i].Value = dtTable.Rows[i]["MessageText"];
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
