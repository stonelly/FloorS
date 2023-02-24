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

namespace Hartalega.FloorSystem.Windows.UI.WorkOrder
{
    public partial class WorkOrderStatusView : FormBase
    {
        #region Member Variables

        private string _screenName = "Work Order Status View";
        private string _className = "Work Order Status";
        private string _loggedInUser;
        WorkOrderDTO _workOrder = new WorkOrderDTO();
        List<WorkOrderSalesLineDTO> _workOrderSalesLineList = null;
        List<WorkOrderDTO> _workOrderDTOGetPODates = null;
        #endregion

        #region Constructor
        public WorkOrderStatusView(WorkOrderDTO workOrder, string loggedInUser)
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
                ExceptionLogging(ex, _screenName, _className, "WorkOrderStatusView", null);
                return;
            }
        }
        #endregion

        #region Event Handler
        private void dgvLineSelection_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;
        }
        #endregion

        #region User Method

        private void BindItem()
        {
            switch (_workOrder.WorkOrderStatus)
            {
                case 1:
                    txtStatus.Text = Constants.OPEN;
                    break;
                case 2:
                    txtStatus.Text = Constants.APPROVED;
                    break;
                case 3:
                    txtStatus.Text = Constants.CLOSED;
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
            if (_workOrder.LastConfirmDate.HasValue)
                txtLastConfirmDate.Text = _workOrder.LastConfirmDate.Value.ToString("dd/MM/yyyy");
            else
                txtLastConfirmDate.Text = string.Empty;
            txtManufacturingDate.Text = _workOrder.ManufacturingDate.ToString("dd/MM/yyyy");
            txtCustomerLotNo.Text = _workOrder.CustomerLot;
            txtContainerSize.Text = _workOrder.ContainerSize;
            txtShippingAgent.Text = _workOrder.ShippingAgent;
            txtDeliveryCountry.Text = _workOrder.DeliveryCountryRegionId;
            txtETD.Text = _workOrder.ETD.ToString("dd/MM/yyyy");
            txtETA.Text = _workOrder.ETA.ToString("dd/MM/yyyy");
            txtSpecialInstruction.Text = _workOrder.SpecialInstruction.FixNewLine();
            txtVesselNames.Text = _workOrder.VesselName.FixNewLine();
            tbWorkOrderType.Text = _workOrder.WorkOrderType;
            //KahHeng (30Jan2019) Set text for PO Date and PO Received Date
            txt_HSB_CustPODocumentDate.Text = _workOrder.HSB_CustPODocumentDate.ToString("dd/MM/yyyy");
            txt_HSB_CustPORecvDate.Text = _workOrder.HSB_CustPORecvDate.ToString("dd/MM/yyyy");
            //KahHeng ENd
        }

        private void BindList()
        {
            dgvLineSelection.Rows.Clear();
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
                    dgvLineSelection["Index", i].Value = dgvLineSelection.Rows.Count;
                    dgvLineSelection["BrandName", i].Value = _workOrderSalesLineList[i].Name;
                    dgvLineSelection["FGCode", i].Value = _workOrderSalesLineList[i].ItemId;
                    dgvLineSelection["GloveType", i].Value = _workOrderSalesLineList[i].GLOVECODE;
                    dgvLineSelection["PackingSize", i].Value = _workOrderSalesLineList[i].GLOVESINNERBOXNO *
                        _workOrderSalesLineList[i].INNERBOXINCASENO ;
                    dgvLineSelection["HartalegaSize", i].Value = _workOrderSalesLineList[i].HARTALEGACOMMONSIZE;
                    dgvLineSelection["CustomerSize", i].Value = _workOrderSalesLineList[i].CUSTOMERSIZE;
                    dgvLineSelection["TotalCartons", i].Value = Convert.ToString(_workOrderSalesLineList[i].ItemCaseCount) + " / " +
                        Convert.ToString(_workOrderSalesLineList[i].SalesQty);
                    dgvLineSelection["TotalPcs", i].Value = Convert.ToString(_workOrderSalesLineList[i].ItemCaseCount *
                        _workOrderSalesLineList[i].GLOVESINNERBOXNO * _workOrderSalesLineList[i].INNERBOXINCASENO) + " / " +
                        Convert.ToString(_workOrderSalesLineList[i].SalesQty *
                        _workOrderSalesLineList[i].GLOVESINNERBOXNO * _workOrderSalesLineList[i].INNERBOXINCASENO);
                    dgvLineSelection["CustomerLotNumber", i].Value = _workOrderSalesLineList[i].CustomerLot;
                }
            }
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
