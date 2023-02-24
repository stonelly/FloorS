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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.WorkOrder
{
    public partial class WorkOrderMaintenanceEdit : FormBase
    {
        #region Member Variables

        private string _screenName = "Work Order Maintenance Edit";
        private string _className = "Work Order Maintenance";
        private string _loggedInUser;
        WorkOrderDTO _workOrder = new WorkOrderDTO();
        List<WorkOrderSalesLineDTO> _workOrderSalesLineList = null;
        List<WorkOrderDTO> _workOrderDTOGetPODates = null;

        #endregion

        #region Constructor

        public WorkOrderMaintenanceEdit(WorkOrderDTO workOrder, string loggedInUser)
        {
            InitializeComponent();
            try
            {
                _loggedInUser = loggedInUser;
                _workOrder = workOrder;
                //KahHeng 30Jan2019 pass PODate and POReceivedDate to _workorder
                _workOrderDTOGetPODates = WorkOrderBLL.GetPODateAndPOReceivedDate(_workOrder.SalesId);
                foreach (WorkOrderDTO _woDTO in _workOrderDTOGetPODates)
                {
                    _workOrder.HSB_CustPODocumentDate = _woDTO.HSB_CustPODocumentDate;
                    _workOrder.HSB_CustPORecvDate = _woDTO.HSB_CustPORecvDate;
                }
                //KahHeng End
                
                BindItem();
                BindList();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "WorkOrderMaintenanceEdit", null);
                return;
            }
        }

        #endregion

        #region Event Handler

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (GlobalMessageBox.Show(Messages.CONFIRM_SAVE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    int rowsReturned = 0;

                    WorkOrderDTO newitem = new WorkOrderDTO();
                    newitem.ContainerSize = _workOrder.ContainerSize;
                    newitem.CreatedDate = _workOrder.CreatedDate;
                    newitem.CustomerLot = _workOrder.CustomerLot;
                    newitem.CustomerPO = _workOrder.CustomerPO;
                    newitem.CustomerRef = _workOrder.CustomerRef;
                    newitem.DocumentStatus = _workOrder.DocumentStatus;
                    newitem.ETA = txtETA.Value;
                    newitem.ETD = txtETD.Value;
                    newitem.LastConfirmDate = _workOrder.LastConfirmDate;
                    newitem.ManufacturingDate = _workOrder.ManufacturingDate;
                    newitem.SalesId = _workOrder.SalesId;
                    newitem.SalesName = _workOrder.SalesName;
                    newitem.SalesStatus = _workOrder.SalesStatus;
                    newitem.ShippingAgent = _workOrder.ShippingAgent;
                    newitem.SpecialInstruction = txtSpecialInstruction.Text;
                    newitem.VesselName = txtVesselNames.Text;
                    newitem.WorkOrderStatus = _workOrder.WorkOrderStatus;
                    newitem.DeliveryCountryRegionId = _workOrder.DeliveryCountryRegionId;
                    newitem.ActionType = Constants.ActionLog.Update;
                    //KahHeng (30Jan2019) Set text for PO Date and PO Received Date
                    newitem.HSB_CustPODocumentDate = _workOrder.HSB_CustPODocumentDate;
                    newitem.HSB_CustPORecvDate = _workOrder.HSB_CustPORecvDate;
                    //KahHeng ENd

                    rowsReturned = WorkOrderBLL.UpdateWorkOrder(_workOrder, newitem, _loggedInUser, _screenName);

                    _workOrder = newitem;

                    if (rowsReturned > 0)
                    {
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    }

                }

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "WorkOrderMaintenanceEdit", null);
                return;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                this.Close();
            }
        }

        private void btnReopen_Click(object sender, EventArgs e)
        {
            int rowsReturned = 0;

            WorkOrderDTO newitem = new WorkOrderDTO();
            newitem.ContainerSize = _workOrder.ContainerSize;
            newitem.CreatedDate = _workOrder.CreatedDate;
            newitem.CustomerLot = _workOrder.CustomerLot;
            newitem.CustomerPO = _workOrder.CustomerPO;
            newitem.CustomerRef = _workOrder.CustomerRef;
            newitem.DocumentStatus = _workOrder.DocumentStatus;
            newitem.ETA = _workOrder.ETA;
            newitem.ETD = _workOrder.ETD;
            newitem.LastConfirmDate = _workOrder.LastConfirmDate;
            newitem.ManufacturingDate = _workOrder.ManufacturingDate;
            newitem.SalesId = _workOrder.SalesId;
            newitem.SalesName = _workOrder.SalesName;
            newitem.SalesStatus = _workOrder.SalesStatus;
            newitem.ShippingAgent = _workOrder.ShippingAgent;
            newitem.SpecialInstruction = _workOrder.SpecialInstruction;
            newitem.VesselName = _workOrder.VesselName;
            //KahHeng (30Jan2019) Set text for PO Date and PO Received Date
            newitem.HSB_CustPODocumentDate = _workOrder.HSB_CustPODocumentDate;
            newitem.HSB_CustPORecvDate = _workOrder.HSB_CustPORecvDate;
            //KahHeng ENd

            try
            {
                if (GlobalMessageBox.Show(Messages.CONFIRM_REOPEN, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    newitem.WorkOrderStatus = 1;
                    newitem.ActionType = Constants.ActionLog.Update;
                    rowsReturned = WorkOrderBLL.UpdateWorkOrder(_workOrder, newitem, _loggedInUser, _screenName);
                    if (rowsReturned > 0)
                    {
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        _workOrder.WorkOrderStatus = newitem.WorkOrderStatus;
                        this.Close();
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnReopen_Click", null);
                return;
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            int rowsReturned = 0;

            WorkOrderDTO newitem = new WorkOrderDTO();
            newitem.ContainerSize = _workOrder.ContainerSize;
            newitem.CreatedDate = _workOrder.CreatedDate;
            newitem.CustomerLot = _workOrder.CustomerLot;
            newitem.CustomerPO = _workOrder.CustomerPO;
            newitem.CustomerRef = _workOrder.CustomerRef;
            newitem.DocumentStatus = _workOrder.DocumentStatus;
            newitem.ETA = txtETA.Value;
            newitem.ETD = txtETD.Value;
            newitem.LastConfirmDate = _workOrder.LastConfirmDate;
            newitem.ManufacturingDate = _workOrder.ManufacturingDate;
            newitem.SalesId = _workOrder.SalesId;
            newitem.SalesName = _workOrder.SalesName;
            newitem.SalesStatus = _workOrder.SalesStatus;
            newitem.ShippingAgent = _workOrder.ShippingAgent;
            newitem.SpecialInstruction = txtSpecialInstruction.Text;
            newitem.VesselName = txtVesselNames.Text;
            //KahHeng (30Jan2019) Set text for PO Date and PO Received Date
            newitem.HSB_CustPODocumentDate = _workOrder.HSB_CustPODocumentDate;
            newitem.HSB_CustPORecvDate = _workOrder.HSB_CustPORecvDate;
            //KahHeng ENd


            try
            {
                // check is work order item empty
                if (_workOrderSalesLineList == null)
                {
                    GlobalMessageBox.Show(Messages.EMPTY_WORKORDERITEM, Constants.AlertType.Warning, Messages.EMPTY_WORKORDERITEM, GlobalMessageBoxButtons.OK);
                    return;
                }
                else
                {

                    //before approve work order item for selected fg code must have value for inner label set no, outer label set no, pallet capacity and preshipment plan
                    //FS_ItemID is a workaround to allow packging material validation
                    var _listFGCode = _workOrderSalesLineList.Where(c => !string.IsNullOrEmpty(c.FS_ItemId)).GroupBy(p => p.ItemId);
                    bool validsize = true;

                    //checking size is  configured in brand item, if not not allow approve MYAdamas 20171124

                    foreach (var saleslineitem in _workOrderSalesLineList)
                    {
                        // FS_ItemID is a workaround to allow packging material validation
                        if (String.IsNullOrEmpty(saleslineitem.HartalegaCommonSize) && !string.IsNullOrEmpty(saleslineitem.FS_ItemId))
                            validsize = false;
                    }

                    if (validsize)
                    {
                        bool valid = false;
                        var listinvalid = string.Empty;
                        var listinvalidfield = string.Empty;


                        foreach (var fgcode in _listFGCode)
                        {
                            valid = WorkOrderBLL.ValidateWorkOrderItem(fgcode.Key);

                            if (!valid)
                            {

                                listinvalid += "\n" + fgcode.Key;

                            }
                        }
                        //valid data
                        if (listinvalid == String.Empty)
                        {
                            if (GlobalMessageBox.Show(Messages.CONFIRM_APPROVE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                            {
                                newitem.WorkOrderStatus = 2;
                                newitem.ActionType = Constants.ActionLog.Update;
                                rowsReturned = WorkOrderBLL.UpdateWorkOrder(_workOrder, newitem, _loggedInUser, _screenName);
                                if (rowsReturned > 0)
                                {
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    _workOrder.WorkOrderStatus = newitem.WorkOrderStatus;
                                    this.Close();
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.BRANDMASTER_NOT_CONFIGURED + listinvalid, Constants.AlertType.Warning, Messages.INVALIDDATA, GlobalMessageBoxButtons.OK);
                            return;

                        }

                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.BRANDSIZE_NOT_CONFIGURED , Constants.AlertType.Warning, Messages.INVALIDDATA, GlobalMessageBoxButtons.OK);
                        return;

                    }
                }

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnApprove_Click", null);
                return;
            }
        }

        private void dgvLineSelection_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;
        }

        #endregion

        #region User Method

        private void BindList()
        {
            dgvLineSelection.Rows.Clear();
            dgvLineSelection.Columns[10].Name = "Edit";
            dgvLineSelection.Columns[11].Name = "Delete";

            try
            {
                _workOrderSalesLineList = WorkOrderBLL.GetWorkOrderSalesLineList(_workOrder.SalesId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindList", null);
                return;
            }
            if (_workOrderSalesLineList != null)
            {
                for (int i = 0; i < _workOrderSalesLineList.Count; i++)
                {
                    dgvLineSelection.Rows.Add();
                    dgvLineSelection.Rows[i].ReadOnly = true;
                    dgvLineSelection["Index", i].Value = dgvLineSelection.Rows.Count;
                    dgvLineSelection["BrandName", i].Value = _workOrderSalesLineList[i].Name;
                    dgvLineSelection["FGCode", i].Value = _workOrderSalesLineList[i].ItemId;
                    dgvLineSelection["GloveType", i].Value = _workOrderSalesLineList[i].GLOVECODE;
                    dgvLineSelection["PackingSize", i].Value = _workOrderSalesLineList[i].GLOVESINNERBOXNO * 
                        _workOrderSalesLineList[i].INNERBOXINCASENO;
                    dgvLineSelection["HartalegaSize", i].Value = _workOrderSalesLineList[i].HARTALEGACOMMONSIZE;
                    dgvLineSelection["CustomerSize", i].Value = _workOrderSalesLineList[i].CUSTOMERSIZE;
                    dgvLineSelection["TotalCartons", i].Value = _workOrderSalesLineList[i].SalesQty;
                    dgvLineSelection["TotalPcs", i].Value = _workOrderSalesLineList[i].SalesQty * 
                        _workOrderSalesLineList[i].GLOVESINNERBOXNO * _workOrderSalesLineList[i].INNERBOXINCASENO;
                    dgvLineSelection["CustomerLotNumber", i].Value = _workOrderSalesLineList[i].CustomerLot;
                    dgvLineSelection["InventTransId", i].Value = _workOrderSalesLineList[i].InventTransId;
                }
            }
        }

        private void BindItem()
        {
            switch (_workOrder.WorkOrderStatus)
            {
                case 1:
                    txtStatus.Text = Constants.OPEN;
                    btnApprove.Visible = true;
                    break;
                case 2:
                    txtStatus.Text = Constants.APPROVED;
                    btnReopen.Visible = true;
                    break;
                case 3:
                    txtStatus.Text = Constants.CLOSED;
                    btnReopen.Visible = true;
                    break;
                default:
                    txtStatus.Text = "";
                    break;
            }
            txtOrderNo.Text = _workOrder.SalesId;
            txtOrderDate.Text = _workOrder.CreatedDate.ToString("dd/MM/yyyy");
            txtCustomer.Text = _workOrder.SalesName;
            txtCustomerPO.Text = _workOrder.CustomerPO;
            txtCustomerRef.Text = _workOrder.CustomerRef;
            txtWorkOrderType.Text = _workOrder.WorkOrderType;
            if (_workOrder.LastConfirmDate.HasValue)
                txtLastConfirmDate.Text = _workOrder.LastConfirmDate.Value.ToString("dd/MM/yyyy");
            else
                txtLastConfirmDate.Text = string.Empty;
            txtManufacturingDate.Text = _workOrder.ManufacturingDate.ToString("dd/MM/yyyy");

            //KahHeng (30Jan2019) Set text for PO Date and PO Received Date
            txt_HSB_CustPODocumentDate.Text = _workOrder.HSB_CustPODocumentDate.ToString("dd/MM/yyyy");
            txt_HSB_CustPORecvDate.Text = _workOrder.HSB_CustPORecvDate.ToString("dd/MM/yyyy");
            //KahHeng ENd
            txtCustomerLotNo.Text = _workOrder.CustomerLot;
            txtContainerSize.Text = _workOrder.ContainerSize;
            txtShippingAgent.Text = _workOrder.ShippingAgent;
            txtDeliveryCountry.Text = _workOrder.DeliveryCountryRegionId;
            txtETD.Value = _workOrder.ETD;
            txtETA.Value = _workOrder.ETA;
            txtSpecialInstruction.Text = _workOrder.SpecialInstruction.FixNewLine();
            txtVesselNames.Text = _workOrder.VesselName.FixNewLine();

            if (txtStatus.Text.Trim().ToLower() == "open")
            {
                btnSave.Visible = true;
                txtETD.Enabled = true;
                txtETA.Enabled = true;
                EnableTextBoxEdit(txtSpecialInstruction);
                EnableTextBoxEdit(txtVesselNames);

                dgvLineSelection.Columns["Edit"].Visible = true;
                dgvLineSelection.Columns["Delete"].Visible = true;
            }
        }

        private void EnableTextBoxEdit(TextBox box)
        {
            box.Enabled = true;
            box.BackColor = Color.White;
            box.ReadOnly = false;
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

        private void dgvLineSelection_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLineSelection.Columns[e.ColumnIndex].Name == "Edit")
            {
                dgvLineSelection.Rows[e.RowIndex].Cells["TotalCartons"].ReadOnly = false;
                dgvLineSelection.Columns[e.ColumnIndex].Name = "Update";
                dgvLineSelection[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                dgvLineSelection.Columns[e.ColumnIndex + 1].Name = "Cancel";
                dgvLineSelection[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";
            }
            else if (dgvLineSelection.Columns[e.ColumnIndex].Name == "Delete")
            {
                if (GlobalMessageBox.Show(Messages.DELETE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    //do delete child here
                    var inventTransId = dgvLineSelection.Rows[e.RowIndex].Cells["InventTransId"].FormattedValue.ToString();
                    WorkOrderSalesLineDTO newItem = new WorkOrderSalesLineDTO();
                    newItem = _workOrderSalesLineList.Where(x => x.InventTransId == inventTransId).FirstOrDefault();

                    newItem.ActionType = Constants.ActionLog.Delete;
                    var oldItem = new WorkOrderSalesLineDTO();
                    WorkOrderBLL.UpdateSalesLinesWorkOrder(oldItem, newItem, _loggedInUser, _screenName);
                    BindList();
                }
            }
            else if (dgvLineSelection.Columns[e.ColumnIndex].Name == "Update")
            {
                WorkOrderSalesLineDTO newitem = new WorkOrderSalesLineDTO();

                var salesQty = dgvLineSelection.Rows[e.RowIndex].Cells["TotalCartons"].FormattedValue.ToString();
                var inventTransId = dgvLineSelection.Rows[e.RowIndex].Cells["InventTransId"].FormattedValue.ToString();

                if (string.IsNullOrEmpty(salesQty))
                {
                    salesQty = "0";
                }

                var oldItem = _workOrderSalesLineList.Where(x => x.InventTransId == inventTransId).FirstOrDefault();

                newitem.SalesId = oldItem.SalesId;
                newitem.LineNum = oldItem.LineNum;
                newitem.Name = oldItem.Name;
                newitem.ItemId = oldItem.ItemId;
                newitem.CUSTOMERSIZE = oldItem.CUSTOMERSIZE;
                newitem.CustomerLot = oldItem.CustomerLot;
                newitem.InventTransId = oldItem.InventTransId;
                newitem.ReceiptDateRequested = oldItem.ReceiptDateRequested;
                newitem.ShippingDateConfirmed = oldItem.ShippingDateConfirmed;
                newitem.FS_ItemId = oldItem.FS_ItemId;
                newitem.HartalegaCommonSize = oldItem.HartalegaCommonSize;
                newitem.HARTALEGACOMMONSIZE = oldItem.HARTALEGACOMMONSIZE;
                newitem.GLOVECODE = oldItem.GLOVECODE;
                newitem.INNERBOXINCASENO = oldItem.INNERBOXINCASENO;
                newitem.GLOVESINNERBOXNO = oldItem.GLOVESINNERBOXNO;
            
                newitem.SalesQty = Convert.ToInt32(salesQty);
                newitem.ActionType = Constants.ActionLog.Update;

                WorkOrderBLL.UpdateSalesLinesWorkOrder(oldItem, newitem, _loggedInUser, _screenName);
                BindList();
            }
            else if (dgvLineSelection.Columns[e.ColumnIndex].Name == "Cancel")
            {
                BindList();
            }
        }

    }
}