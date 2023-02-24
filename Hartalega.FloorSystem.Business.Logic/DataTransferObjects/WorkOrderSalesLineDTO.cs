using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Hartalega.FloorSystem.Framework.Constants;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class WorkOrderSalesLineDTO
    {
        public string SalesId { get; set; }
        public int LineNum { get; set; }
        public string Name { get; set; }
        public string ItemId { get; set; }
        public string FS_ItemId { get; set; }
        public string HartalegaCommonSize { get; set; }
        public int SalesQty { get; set; }
        public string CustomerLot { get; set; }
        public string InventTransId { get; set; }
        public DateTime ReceiptDateRequested { get; set; }
        public DateTime ShippingDateConfirmed { get; set; }
        //KahHeng (30Jan2019) Set text for PO Date and PO Received Date
        public DateTime HSB_CustPODocumentDate { get; set; }
        public DateTime HSB_CustPORecvDate { get; set; }
        //KahHeng ENd
        public string GLOVECODE { get; set; }
        public int INNERBOXINCASENO { get; set; }
        public int GLOVESINNERBOXNO { get; set; }
        public string CUSTOMERSIZE { get; set; }
        public string HARTALEGACOMMONSIZE { get; set; }
        public int ItemCaseCount { get; set; }
        public ActionLog ActionType { get; set; }
    }
}
