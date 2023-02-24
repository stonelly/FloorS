using CheckComboBoxTest;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.WorkOrder
{
    public partial class WorkOrderMaintenanceList : FormBase
    {
        #region Member Variables

        private string _screenName = "Work Order - Maintenance";
        private string _className = "Work Order";
        private string _loggedInUser;
        private WorkOrderDTO _workOrderDTO = null;
        private List<WorkOrderDTO> _workOrderDTOList = null;

        private PageOffsetList _pg;
        private DataTable _dtWorkOrderList;

        #endregion

        #region Constructor

        public WorkOrderMaintenanceList(string loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }

        #endregion

        #region Event Handler

        public void dtOrderDateStart_Changed(object sender, EventArgs e)
        {
            dtOrderDateStart.CustomFormat = "dd/MM/yyyy";
        }

        public void dtOrderDateEnd_Changed(object sender, EventArgs e)
        {
            dtOrderDateEnd.CustomFormat = "dd/MM/yyyy";
        }

        public void dtETDStart_Changed(object sender, EventArgs e)
        {
            dtETDStart.CustomFormat = "dd/MM/yyyy";
        }

        public void dtETDEnd_Changed(object sender, EventArgs e)
        {
            dtETDEnd.CustomFormat = "dd/MM/yyyy";
        }

        private void dtOrderDateStart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                dtOrderDateStart.CustomFormat = " ";
            }

        }

        private void dtOrderDateEnd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                dtOrderDateEnd.CustomFormat = " ";
            }

        }

        private void dtETDStart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                dtETDStart.CustomFormat = " ";
            }

        }

        private void dtETDEnd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                dtETDEnd.CustomFormat = " ";
            }

        }
        private void WorkOrderMaintainanceList_Load(object sender, EventArgs e)
        {
            //dtOrderDateStart.Value = System.DateTime.Today.AddYears(-1);
            //dtETDStart.Value = System.DateTime.Today.AddYears(-1);
            //dtOrderDateStart.Value = WorkOrderBLL.GetOldestDate("OrderDate");
            //dtETDStart.Value = WorkOrderBLL.GetOldestDate("ETD");
            //set default to empty
            dtOrderDateStart.CustomFormat = " ";
            dtOrderDateEnd.CustomFormat = " ";
            dtETDStart.CustomFormat = " ";
            dtETDEnd.CustomFormat = " ";

            //bindingSourceTable.CurrentChanged -= new System.EventHandler(bindingSource_CurrentChanged);
            //dgvLineSelection.CellContentDoubleClick -= new System.Windows.Forms.DataGridViewCellEventHandler(dgvLineSelection_CellDoubleClick);
            BindItem();
            PopulateGrid();
            //bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
            //dgvLineSelection.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvLineSelection_CellDoubleClick);
        }

        private void bindingSource_CurrentChanged(object sender, EventArgs e)
        {
            DataTable dtWorkOrder = new DataTable();
            dtWorkOrder = _dtWorkOrderList.Clone();
            if (bindingSourceTable.Current != null)
            {
                int offset = (int)bindingSourceTable.Current;
                for (int i = offset; i < offset + _pg.pageSize && i < _pg.TotalRecords; i++)
                {
                    DataRow newRow = dtWorkOrder.NewRow();
                    dtWorkOrder.ImportRow(_dtWorkOrderList.Rows[i]);
                }
                FillGrid(dtWorkOrder);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string vldmsg = string.Empty;
            if (dtOrderDateStart.Text != " " && dtOrderDateEnd.Text != " ")
            {
                if (dtOrderDateStart.Value.Date > dtOrderDateEnd.Value.Date)
                    vldmsg += "\nOrder start date cannot be later than order end date.";
            }
            if (dtETDStart.Text != " " && dtETDEnd.Text != " ")
            {
                if (dtETDStart.Value.Date > dtETDEnd.Value.Date)
                    vldmsg += "\nETD start cannot be later than ETD end.";
            }

            if (string.IsNullOrEmpty(vldmsg))
            {
                PopulateGrid();
            }
            else
            {
                GlobalMessageBox.Show(vldmsg, Messages.INVALIDDATA, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                return;
            }
        }

        private void dgvLineSelection_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            _workOrderDTO = new WorkOrderDTO();           
            if (e.RowIndex > Constants.MINUSONE)
            {
                if (dgvLineSelection.Rows.Count > 0 && dgvLineSelection.Rows[e.RowIndex] != null)
                {
                    _workOrderDTO = _workOrderDTOList.Where(p => p.SalesId == Convert.ToString(dgvLineSelection["OrderNo", e.RowIndex].Value)).FirstOrDefault();
                    new WorkOrderMaintenanceEdit(_workOrderDTO, _loggedInUser).ShowDialog();
                    PopulateGrid();
                }
            }
        }

        private void dgvLineSelection_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;
        }

        private void dgvLineSelection_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn newColumn = dgvLineSelection.Columns[e.ColumnIndex];
            DataGridViewColumn oldColumn = dgvLineSelection.SortedColumn;
            ListSortDirection direction;

            // If oldColumn is null, then the DataGridView is not sorted.
            if (oldColumn != null)
            {
                // Sort the same column again, reversing the SortOrder.
                if (oldColumn == newColumn &&
                    dgvLineSelection.SortOrder == SortOrder.Ascending)
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

            dgvLineSelection.Sort(newColumn, direction);
            newColumn.HeaderCell.SortGlyphDirection = direction == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;

            // Sort the selected column.
            DataView dv = _dtWorkOrderList.DefaultView;

            switch (newColumn.Name)
            {
                case "Status":
                    dv.Sort = "WorkOrderStatus" + (direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                    break;
                case "OrderDate":
                    dv.Sort = "CreatedDate" + (direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                    break;
                case "OrderNo":
                    dv.Sort = "SalesId" + (direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                    break;
                case "Customer":
                    dv.Sort = "SalesName" + (direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                    break;
                default:
                    dv.Sort = newColumn.Name + (direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                    break;
            }

            DataTable sortedDT = dv.ToTable();
            _dtWorkOrderList = sortedDT;
            FillGrid(_dtWorkOrderList);

            _pg = new PageOffsetList(Constants.TWENTY, _dtWorkOrderList.Rows.Count);
            bindingNavigatorTable.BindingSource = bindingSourceTable;
            bindingSourceTable.DataSource = _pg.GetList();
        }

        #endregion

        #region User Methods

        private void BindItem()
        {
            List<DropdownDTO> items = WorkOrderBLL.GetEnumMaster(Constants.WORKORDERSTATUS);
            for (int i = 0; i < items.Count; i++)
            {
                CCBoxItem item = new CCBoxItem(items[i].DisplayField, Convert.ToInt32(items[i].IDField));
                if (item.Name == "Open" || item.Name == "Approved")
                {
                    cmbStatus.Items.Add(item);
                    cmbStatus.SetItemChecked(i, true);
                }
                else
                    cmbStatus.Items.Add(item);
            }
        }

        private void FillGrid(DataTable dtworkorder)
        {
            dgvLineSelection.Rows.Clear();
            try
            {
                if (dtworkorder.Rows.Count > 0)
                {
                    bindingNavigatorTable.Enabled = true;
                    _workOrderDTOList = WorkOrderBLL.WorkOrderListFromDT(dtworkorder);
                    if (_workOrderDTOList != null)
                    {
                        for (int i = 0; i < _workOrderDTOList.Count; i++)
                        {
                            dgvLineSelection.Rows.Add();
                            dgvLineSelection.Rows[i].ReadOnly = true;

                            //dgvLineSelection["Index", i].Value = dgvLineSelection.Rows.Count;
                            switch (_workOrderDTOList[i].WorkOrderStatus)
                            {
                                case 1:
                                    dgvLineSelection["Status", i].Value = Constants.OPEN;
                                    break;
                                case 2:
                                    dgvLineSelection["Status", i].Value = Constants.APPROVED;
                                    break;
                                case 3:
                                    dgvLineSelection["Status", i].Value = Constants.CLOSED;
                                    break;
                                default:
                                    dgvLineSelection["Status", i].Value = "";
                                    break;
                            }
                            dgvLineSelection["OrderDate", i].Value = _workOrderDTOList[i].CreatedDate.ToString("dd/MM/yyyy");
                            dgvLineSelection["OrderNo", i].Value = _workOrderDTOList[i].SalesId;
                            dgvLineSelection["OrderType", i].Value = _workOrderDTOList[i].WorkOrderType;
                            dgvLineSelection["CustomerRef", i].Value = _workOrderDTOList[i].CustomerRef;
                            dgvLineSelection["CustomerPO", i].Value = _workOrderDTOList[i].CustomerPO;
                            dgvLineSelection["Customer", i].Value = _workOrderDTOList[i].SalesName;                            
                            dgvLineSelection["ShippingAgent", i].Value = _workOrderDTOList[i].ShippingAgent;
                            dgvLineSelection["ETD", i].Value = _workOrderDTOList[i].ETD.ToString("dd/MM/yyyy");
                            dgvLineSelection["ETA", i].Value = _workOrderDTOList[i].ETA.ToString("dd/MM/yyyy");
                            dgvLineSelection["VesselName", i].Value = _workOrderDTOList[i].VesselName;
                        }
                    }
                }
                else
                {
                    //GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    dgvLineSelection.DataSource = null;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "FillGrid", null);
                return;
            }
        }

        private void PopulateGrid()
        {
            try
            {
                List<byte> workorderstatus = new List<byte>();
                int count = 0;
                for (int i = 0; i < cmbStatus.Items.Count; i++)
                {
                    if (cmbStatus.GetItemCheckState(i) == CheckState.Unchecked)
                    {
                        CCBoxItem item = cmbStatus.Items[i] as CCBoxItem;
                        workorderstatus.Add(Convert.ToByte(item.Value));
                        count++;
                    }
                }

                DateTime? OrderStartDate;
                DateTime? OrderEndDate;
                DateTime? ETDStartDate;
                DateTime? ETDEndDate;

                if (dtOrderDateStart.Text != " ")
                    OrderStartDate = dtOrderDateStart.Value;
                else
                    OrderStartDate = null;

                if (dtOrderDateEnd.Text != " ")
                    OrderEndDate = dtOrderDateEnd.Value;
                else
                    OrderEndDate = null;

                if (dtETDStart.Text != " ")
                    ETDStartDate = dtETDStart.Value;
                else
                    ETDStartDate = null;

                if (dtETDEnd.Text != " ")
                    ETDEndDate = dtETDEnd.Value;
                else
                    ETDEndDate = null;

                _dtWorkOrderList = WorkOrderBLL.GetDTWorkOrderList(
                    workorderstatus,
                    txtOrderNo.Text,
                    txtCustomer.Text,
                    txtCustomerPO.Text,
                    txtCustomerRef.Text,
                    OrderStartDate,
                    OrderEndDate,
                    ETDStartDate,
                    ETDEndDate);
                FillGrid(_dtWorkOrderList);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PopulateGrid", null);
                return;
            }

            _pg = new PageOffsetList(Constants.TWENTY, _dtWorkOrderList.Rows.Count);
            bindingNavigatorTable.BindingSource = bindingSourceTable;
            bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
            bindingSourceTable.DataSource = _pg.GetList();
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
