using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    public partial class PalletMaster : Form
    {
        private DataTable _dtTableDetails;
        private DataTable dtTableDetails;
        private PageOffsetList _pg;
        private string _screenName = "Configuration SetUp - PalletMaster";
        private string _className = "PalletMaster";
        private bool _refresh = false;
        private DataGridViewRow _cell = null;        
        private string _loggedInUser;
        private string _wsId;

        private void PalletMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;

        }
        public PalletMaster(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
        }

        private void PalletMaster_Load(object sender, EventArgs e)
        {
            try
            {
                _refresh = false;
                bindingSourceTable.CurrentChanged -= new System.EventHandler(bindingSource_CurrentChanged);
                dgvPalletMaster.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(dgvPalletMaster_CellContentClick);
                PopulateGrid();
                _wsId = WorkStationDTO.GetInstance().WorkStationId;
                bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
                dgvPalletMaster.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvPalletMaster_CellContentClick);
            }
            catch (Exception ex)
            {
                FloorSystemException fsexcep = new FloorSystemException(ex.Message, "Palletmaster Presentation Layer", ex.InnerException);
                ExceptionLogging(fsexcep, _screenName, _className, "PalletMaster_Load", null);
            }
        }

        /// <summary>
        /// Populate grid
        /// </summary>
        private void PopulateGrid()
        {
            try
            {
                _dtTableDetails = MasterTableBLL.GetPalletMasterDetails();
                _pg = new PageOffsetList(Constants.TWENTY, _dtTableDetails.Rows.Count);
                bindingNavigatorTable.BindingSource = bindingSourceTable;
                bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
                bindingSourceTable.DataSource = _pg.GetList();

                if (!_refresh)
                {
                    DataGridViewLinkColumn Editlink = new DataGridViewLinkColumn();
                    Editlink.HeaderText = "Edit";
                    Editlink.Text = "Edit";
                    Editlink.Name = "Edit";
                    Editlink.DataPropertyName = "lnkColumn";
                    Editlink.DefaultCellStyle.NullValue = "Edit";
                    Editlink.LinkBehavior = LinkBehavior.SystemDefault;
                    dgvPalletMaster.Columns.Add(Editlink);

                    DataGridViewLinkColumn Deletelink = new DataGridViewLinkColumn();
                    Deletelink.Name = "Delete";
                    Deletelink.HeaderText = "Delete";
                    Deletelink.DataPropertyName = "lnkColumn";
                    Deletelink.LinkBehavior = LinkBehavior.SystemDefault;
                    Deletelink.Text = "Delete";
                    Deletelink.DefaultCellStyle.NullValue = "Delete";
                    dgvPalletMaster.Columns.Add(Deletelink);
                }
                else
                {
                    btnAdd.Enabled = true;
                    if (dgvPalletMaster.Columns["Insert"] != null)
                        dgvPalletMaster.Columns["Insert"].Name = "Edit";

                    if (dgvPalletMaster.Columns["Update"] != null)
                        dgvPalletMaster.Columns["Update"].Name = "Edit";

                    if (dgvPalletMaster.Columns["Cancel"] != null)
                        dgvPalletMaster.Columns["Cancel"].Name = "Delete";
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PopulateGrid", null);
                return;
            }
            catch (Exception ex)
            {
                FloorSystemException fsexcep = new FloorSystemException(ex.Message, "Palletmaster Presentation Layer", ex.InnerException);
                ExceptionLogging(fsexcep, _screenName, _className, "PopulateGrid", null);
            }                     
        }

        /// <summary>
        /// Used for handling the pagination for datagrid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bindingSource_CurrentChanged(object sender, EventArgs e)
        {
            dtTableDetails = new DataTable();
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
        /// Fill Grid
        /// </summary>
        private void FillGrid(DataTable dtTable)
        {
            btnAdd.Enabled = true;
            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Marquee;
            dgvPalletMaster.DataSource = dtTable;
            for (int i = 0; i < dtTable.Rows.Count; i++)
            {
                dgvPalletMaster.Rows[i].ReadOnly = true;
            }
            progressBar1.Visible = false;
        }

        /// <summary>
        /// Validation of required fields
        /// </summary>
        private string ValidateRequiredFields(PalletMasterDTO palletMasterDTO)
        {
            StringBuilder requiredFieldMessage = new StringBuilder(Messages.REQUIREDFIELDMESSAGE);
            if (string.IsNullOrEmpty(palletMasterDTO.PalletId))
                requiredFieldMessage.AppendLine("PalletId");            
            return requiredFieldMessage.ToString();
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
        /// Event Handler for datagrid cell content click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPalletMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > Constants.MINUSONE)
                {
                    if (dgvPalletMaster.Columns[e.ColumnIndex].Name == "Edit")
                    {
                        btnAdd.Enabled = false;
                        dgvPalletMaster.Rows[e.RowIndex].ReadOnly = false;
                        dgvPalletMaster["PalletId", e.RowIndex].ReadOnly = true;
                        dgvPalletMaster.Columns[e.ColumnIndex].Name = "Update";
                        dgvPalletMaster[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                        _cell = dgvPalletMaster.CurrentRow;
                        dgvPalletMaster[1, e.RowIndex].ReadOnly = true;
                        dgvPalletMaster.Columns[e.ColumnIndex + 1].Name = "Cancel";
                        dgvPalletMaster[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";
                    }
                    else if (dgvPalletMaster.Columns[e.ColumnIndex].Name == "Cancel" && dgvPalletMaster.CurrentRow == _cell)
                    {
                        _refresh = true;
                        PopulateGrid();
                        dgvPalletMaster.Columns[e.ColumnIndex].Name = "Delete";
                    }
                    else if (dgvPalletMaster.Columns[e.ColumnIndex].Name == "Delete")
                    {
                        if (string.IsNullOrEmpty(dgvPalletMaster.Rows[e.RowIndex].Cells["PalletId"].Value.ToString()))
                        {
                            dtTableDetails.Rows.RemoveAt(0);
                            dgvPalletMaster.Columns["Insert"].Name = "Edit";
                            btnAdd.Enabled = true;
                        }
                        else
                        {
                            if (GlobalMessageBox.Show(Messages.DELETE_PALLETID, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                            {
                                try
                                {
                                    int rowsReturned = MasterTableBLL.DeletePalletMasterRecord(dgvPalletMaster.Rows[e.RowIndex].Cells["PalletId"].Value.ToString(),_wsId, _loggedInUser);
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
                                    ExceptionLogging(ex, _screenName, _className, "dgvPalletMaster_CellContentClick", null);
                                    return;
                                }
                            }
                        }
                    }
                    else if ((dgvPalletMaster.Columns[e.ColumnIndex].Name == "Update" && dgvPalletMaster.CurrentRow == _cell) ||
                             dgvPalletMaster.Columns[e.ColumnIndex].Name == "Insert" && dgvPalletMaster.CurrentRow == _cell)
                    {
                        PalletMasterDTO palletMasterDTO = new PalletMasterDTO();
                        bool isDuplicate = false;

                        if (string.IsNullOrEmpty(dgvPalletMaster.Rows[e.RowIndex].Cells["Palletid"].Value.ToString()))
                            palletMasterDTO.PalletId = string.Empty;
                        else
                            palletMasterDTO.PalletId = Convert.ToString(dgvPalletMaster.Rows[e.RowIndex].Cells["Palletid"].Value);

                        if (string.IsNullOrEmpty(dgvPalletMaster.Rows[e.RowIndex].Cells["isPreshipment"].Value.ToString()))
                            palletMasterDTO.IsPreshipment = false;
                        else
                            palletMasterDTO.IsPreshipment = Convert.ToBoolean(dgvPalletMaster.Rows[e.RowIndex].Cells["isPreshipment"].Value);

                        if (string.IsNullOrEmpty(dgvPalletMaster.Rows[e.RowIndex].Cells["isAvailable"].Value.ToString()))
                            palletMasterDTO.IsAvailable = false;
                        else
                            palletMasterDTO.IsAvailable = Convert.ToBoolean(dgvPalletMaster.Rows[e.RowIndex].Cells["isAvailable"].Value);

                        if (string.IsNullOrEmpty(dgvPalletMaster.Rows[e.RowIndex].Cells["isOccupied"].Value.ToString()))
                            palletMasterDTO.Isoccupied = false;
                        else
                            palletMasterDTO.Isoccupied = Convert.ToBoolean(dgvPalletMaster.Rows[e.RowIndex].Cells["isOccupied"].Value);

                        if (string.IsNullOrEmpty(dgvPalletMaster.Rows[e.RowIndex].Cells["Zone"].Value.ToString()))
                            palletMasterDTO.Zone = string.Empty;
                        else
                            palletMasterDTO.Zone = Convert.ToString(dgvPalletMaster.Rows[e.RowIndex].Cells["Zone"].Value);

                        string validationMessage = ValidateRequiredFields(palletMasterDTO);
                        if (validationMessage == Messages.REQUIREDFIELDMESSAGE)
                        {
                            try
                            {
                                if (dgvPalletMaster.Columns[e.ColumnIndex].Name == "Insert")
                                    isDuplicate = Convert.ToBoolean(MasterTableBLL.IsPalletDuplicate(dgvPalletMaster.Rows[e.RowIndex].Cells["PalletId"].Value.ToString()));
                                if (!isDuplicate)
                                {
                                    dgvPalletMaster.Columns[e.ColumnIndex].Name = "Edit";
                                    int rowsReturned = MasterTableBLL.UpdatePalletMasterDetails(palletMasterDTO, _loggedInUser,_wsId);
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
                                    GlobalMessageBox.Show(Messages.DUPLICATE_PALLETID, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                }
                            }
                            catch (FloorSystemException ex)
                            {
                                ExceptionLogging(ex, _screenName, _className, "dgvPalletMaster_CellContentClick", null);
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
            catch (Exception ex)
            {
                FloorSystemException fsexcep = new FloorSystemException(ex.Message,"Palletmaster Presentation Layer", ex.InnerException);
                ExceptionLogging(fsexcep, _screenName, _className, "dgvPalletMaster_CellContentClick", null);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                btnAdd.Enabled = false;
                DataRow row = dtTableDetails.NewRow();
                dtTableDetails.Rows.InsertAt(row, 0);
                dgvPalletMaster.Rows[0].Cells["Edit"].Value = "Insert";
                dgvPalletMaster.Columns["Edit"].Name = "Insert";
                dgvPalletMaster.Rows[0].ReadOnly = false;
                _cell = dgvPalletMaster.Rows[0];
                if (dgvPalletMaster.Columns["Delete"] != null)
                {
                    dgvPalletMaster.Columns["Delete"].Name = "Cancel";
                    dgvPalletMaster["Cancel", 0].Style.NullValue = "Cancel";
                }
            }                
            catch (Exception ex)
            {
                FloorSystemException fsexcep = new FloorSystemException(ex.Message,"Palletmaster Presentation Layer", ex.InnerException);
                ExceptionLogging(fsexcep, _screenName, _className, "btnAdd_Click", null);
            }
        }

    }
}

