using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using static Hartalega.FloorSystem.Framework.Constants;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    [Serializable]
    public class WorkOrderDTO
    {
        public string SalesId { get; set; }
        public string SalesName { get; set; }
        public byte SalesStatus { get; set; }
        public byte DocumentStatus { get; set; }
        public byte WorkOrderStatus { get; set; }
        public string CustomerPO { get; set; }
        public string CustomerRef { get; set; }        
        public DateTime? LastConfirmDate { get; set; }
        public DateTime ManufacturingDate { get; set; }
        //KahHeng (30Jan2019) Set text for PO Date and PO Received Date
        public DateTime HSB_CustPODocumentDate { get; set; }
        public DateTime HSB_CustPORecvDate { get; set; }
        //KahHeng ENd

        public string CustomerLot { get; set; }
        public string ContainerSize { get; set; }
        public string ShippingAgent { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ETA { get; set; }
        public string SpecialInstruction { get; set; }
        public string VesselName { get; set; }
        public DateTime CreatedDate { get; set; }
        public ActionLog ActionType { get; set; }
        public string DeliveryCountryRegionId { get; set; }
        public string WorkOrderType { get; set; }
    }
}
