using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class GloveBatchOrderDTO
    {
        public string SchedStart { get; set; }
        public string SchedFromTime { get; set; }
        public string ProdStatus { get; set; }
        public string BthOrderId { get; set; }
        public string SalesOrder { get; set; }
        public string ItemId { get; set; }
        public string Size { get; set; }
        public string QtySched { get; set; }
        public string ReportedQty { get; set; }
        public string RemainingQty { get; set; }
        public string ResourceGrp { get; set; }
        public string Resource { get; set; }
        public string SchedEnd { get; set; }
        public string SchedToTime { get; set; }
    }
}
