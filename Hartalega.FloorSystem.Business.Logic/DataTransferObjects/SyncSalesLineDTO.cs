using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class SyncSalesLineDTO
    {
        public string SalesId { get; set; }
        public decimal LineNum{get;set;}
        public string Name { get; set; }
        public string ItemId { get; set; }
        public string CustomerSize { get; set; }
        public decimal SalesQty { get; set; }
        public string CustomerLot { get; set; }
        public string InventTransId { get; set; }
        public DateTime? ReceiptDateRequested { get; set; }
        public DateTime? ShippingDateConfirmed { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        //KahHeng 30Jan2019 added get-set method for PODate and POReceivedDate
        public DateTime HSB_CustPODocumentDate { get; set; }
        public DateTime HSB_CustPORecvDate { get; set; }
        //KahHeng End
    }
}
