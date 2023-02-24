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
    public partial class ProductionDefectMaster : Form
    {
        #region Member Variables

        private string _screenName = "Configuration SetUp - ProductionDefectMaster";
        private string _className = "ProductionDefectMaster";
        private int _prevRowindex = -1;
        private PageOffsetList _pg;
        private List<ProductionDefectMasterDTO> _proddefectmaster;
        public List<ProductionDefectMasterDTO> Proddefectmaster
        {
            get
            {
                if (_proddefectmaster == null)
                {
                    _proddefectmaster = MasterTableBLL.GetProductionDefectMaster();
                    return _proddefectmaster;
                }
                else
                {
                    return _proddefectmaster;
                }
            }
        }

        private string _loggedInUser;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public ProductionDefectMaster(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;

        }

        /// <summary>
        /// Event Handler for Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductionDefectMaster_Load(object sender, EventArgs e)
        {
            List<ProductionDefectMasterDTO> lstProductionDefectMasterDTO = Proddefectmaster.GetRange(0, MasterTableBLL.GetRange(0, Proddefectmaster.Count));
            BindGrid(lstProductionDefectMasterDTO);
        }

        #endregion

        #region Event Handlers
        private void ProductionDefectMaster_KeyDown(object sender, KeyEventArgs e)
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
                List<ProductionDefectMasterDTO> lstProductionDefectMasterDTO = Proddefectmaster.GetRange(offset, MasterTableBLL.GetRange(offset, Proddefectmaster.Count));
                BindGrid(lstProductionDefectMasterDTO);
                btnAdd.Enabled = true;
                _prevRowindex = -1;
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

                            //_prevRowindex = e.RowIndex;
                            dgTableMaintenance.Rows[e.RowIndex].ReadOnly = false;
                            dgTableMaintenance.Columns[e.ColumnIndex].Name = "Update";
                            dgTableMaintenance[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                            dgTableMaintenance.Columns[e.ColumnIndex + 1].Name = "Cancel";
                            dgTableMaintenance[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";
                        }
                        else
                        {
                            ProductionDefectMasterDTO pdmNew = new ProductionDefectMasterDTO();
                            pdmNew.ProdDefectId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["ProdDefectId"].Value);
                            pdmNew.ProdDefectName = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["ProdDefectName"].Value);
                            pdmNew.DefectDescription = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectDescription"].Value);
                            pdmNew.ActionType = Constants.ActionLog.Update;
                            ProductionDefectMasterDTO pdmold = MasterTableBLL.GetProductionDefectMaster().Where(p => p.ProdDefectId == pdmNew.ProdDefectId).FirstOrDefault();
                            if (string.IsNullOrEmpty(Validate(pdmNew)))
                            {
                                if (!CheckDuplicate(pdmNew))
                                {
                                    MasterTableBLL.SaveProductionDefectMaster(pdmold, pdmNew, _loggedInUser);
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DUPLICATE_PRODUCTIONDEFECTMASTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + Validate(pdmNew), Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                                return;
                            }
                            _proddefectmaster = null;
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
                            ProductionDefectMasterDTO pdmNew = new ProductionDefectMasterDTO();
                            pdmNew = MasterTableBLL.GetProductionDefectMaster().Where(p => p.ProdDefectId == Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["ProdDefectId"].Value)).FirstOrDefault();

                            pdmNew.ActionType = Constants.ActionLog.Delete;
                            pdmNew.IsDeleted = true;
                            ProductionDefectMasterDTO pdmold = MasterTableBLL.GetProductionDefectMaster().Where(p => p.ProdDefectId == pdmNew.ProdDefectId).FirstOrDefault();
                            MasterTableBLL.SaveProductionDefectMaster(pdmold, pdmNew, _loggedInUser);
                            _proddefectmaster = null;
                            bindingSource_CurrentChanged(null, null);
                        }
                        return;
                    }
                    if (dgTableMaintenance.Columns[e.ColumnIndex].Name == "Insert")
                    {
                        ProductionDefectMasterDTO pdmNew = new ProductionDefectMasterDTO();
                        pdmNew.ProdDefectId = Convert.ToInt32(dgTableMaintenance.Rows[e.RowIndex].Cells["ProdDefectId"].Value);
                        pdmNew.ProdDefectName = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["ProdDefectName"].Value);
                        pdmNew.DefectDescription = Convert.ToString(dgTableMaintenance.Rows[e.RowIndex].Cells["DefectDescription"].Value);
                        ProductionDefectMasterDTO pdmold = new ProductionDefectMasterDTO();
                        pdmNew.ActionType = Constants.ActionLog.Add;
                        if (string.IsNullOrEmpty(Validate(pdmNew)))
                        {
                            if (!CheckDuplicate(pdmNew))
                            {
                                MasterTableBLL.SaveProductionDefectMaster(pdmold, pdmNew, _loggedInUser);
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DUPLICATE_PRODUCTIONDEFECTMASTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + Validate(pdmNew), Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                            return;
                        }
                        _proddefectmaster = null;
                        btnAdd.Enabled = true;
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
                dgTableMaintenance["ProdDefectName", 0].Value = string.Empty;
                dgTableMaintenance["DefectDescription", 0].Value = string.Empty;
                dgTableMaintenance["ProdDefectId", 0].Value = "0";
                dgTableMaintenance.Columns[3].Name = "Insert";
                dgTableMaintenance[3, 0].Style.NullValue = "Insert";
                dgTableMaintenance.Columns[4].Name = "Cancel";
                dgTableMaintenance[4, 0].Style.NullValue = "Cancel";
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
                _proddefectmaster = Sort(_proddefectmaster, newColumn.Name, direction);
                BindGrid(_proddefectmaster);

                _pg = new PageOffsetList(Constants.TWENTY, _proddefectmaster.Count);
                bindingNavigatorTable.BindingSource = bindingSourceTable;
                bindingSourceTable.DataSource = _pg.GetList();

                dgTableMaintenance.Columns[3].Name = "Edit";
                dgTableMaintenance.Columns[4].Name = "Delete";
            }
        }

        public List<ProductionDefectMasterDTO> Sort(List<ProductionDefectMasterDTO> input, string property, ListSortDirection orderSeq)
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

        private void BindGrid(List<ProductionDefectMasterDTO> prodefectmaster)
        {
            bindingSourceTable.CurrentChanged -= new System.EventHandler(bindingSource_CurrentChanged);
            dgTableMaintenance.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
            dgTableMaintenance.AutoGenerateColumns = false;
            _pg = new PageOffsetList(Constants.TWENTY, Proddefectmaster.Count);
            bindingSourceTable.DataSource = _pg.GetList();
            bindingNavigatorTable.BindingSource = bindingSourceTable;
            populateGrid(prodefectmaster);
            bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
            dgTableMaintenance.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgTableMaintenance_CellContentClick);
        }

        private void populateGrid(List<ProductionDefectMasterDTO> prodefectmaster)
        {
            int i = 0;
            dgTableMaintenance.Rows.Clear();
            try
            {
                foreach (ProductionDefectMasterDTO dr in prodefectmaster)
                {
                    dgTableMaintenance.Rows.Add();
                    dgTableMaintenance.Rows[i].ReadOnly = true;
                    dgTableMaintenance["ProdDefectName", i].Value = dr.ProdDefectName;
                    dgTableMaintenance["DefectDescription", i].Value = dr.DefectDescription;
                    dgTableMaintenance["ProdDefectId", i].Value = dr.ProdDefectId;

                    dgTableMaintenance.Columns[3].Name = "Edit";
                    dgTableMaintenance[3, i].Style.NullValue = "Edit";
                    dgTableMaintenance.Columns[4].Name = "Delete";
                    dgTableMaintenance[4, i].Style.NullValue = "Delete";
                    i++;
                }
            }
            catch (Exception ex)
            {
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "populateGrid", null);
                return;
            }
        }

        private string Validate(ProductionDefectMasterDTO pdmNew)
        {
            string validamsg = string.Empty;
            if (string.IsNullOrEmpty(pdmNew.ProdDefectName) || string.IsNullOrEmpty(pdmNew.DefectDescription))
            {
                if (string.IsNullOrEmpty(pdmNew.ProdDefectName))
                {
                    validamsg = "DefectName" + Environment.NewLine;
                }
                if (string.IsNullOrEmpty(pdmNew.DefectDescription))
                {
                    validamsg = validamsg + "Description";
                }
            }
            return validamsg;
        }

        private bool CheckDuplicate(ProductionDefectMasterDTO pDMnew)
        {
            bool _isduplicate = false;

            int lstcount = (from p in Proddefectmaster
                            where p.DefectDescription == pDMnew.DefectDescription && p.ProdDefectName == pDMnew.ProdDefectName && p.IsDeleted == pDMnew.IsDeleted
                            select p).ToList<ProductionDefectMasterDTO>().Count;

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
