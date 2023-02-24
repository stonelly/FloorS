using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class SyncSalesTableDTO
    {
        public int Num { get; set; }
        public string SalesId { get; set; }
        public string SalesName { get; set; }
        public byte SalesStatus { get; set; }
        public byte DocumentStatus { get; set; }
        public byte WorkOrderType { get; set; }
        public string CustomerPO { get; set; }
        public string CustomerRef { get; set; }
        public string ConfirmId { get; set; }
        public DateTime ConfirmDate { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public string CustomerLot { get; set; }
        public string ContainerSize { get; set; }
        public string ShippingAgent { get; set; }
        public DateTime? ShippingDateRequested { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public string SpecialInstruction { get; set; }
        public string VesselName { get; set; }
        public string DeliveryCountryRegionId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        //KahHeng 29Jan2019 added get-set method for PODate and POReceivedDate
        public DateTime HSB_CustPODocumentDate { get; set; }
        public DateTime HSB_CustPORecvDate { get; set; }
        //KahHeng End
    }
}
