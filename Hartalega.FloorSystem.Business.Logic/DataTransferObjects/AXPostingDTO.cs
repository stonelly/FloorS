using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class AXPostingDTO
    {
        public string ServiceName { get; set; }
        public string PostingType { get; set; }
        public DateTime PostedDate { get; set; }
        public string BatchNumber { get; set; }
        public string SerialNumber { get; set; }
        public bool IsPostedToAX { get; set; }
        public bool IsPostedInAX { get; set; }
        public int Sequence { get; set; }
        public string ExceptionCode { get; set; }

        public string TransactionID { get; set; }

        public string Area { get; set; }

        public bool IsConsolidated { get; set; } // #AZRUL 20210909: Open batch flag for NGC1.5
        public string PlantNo { get; set; } // #AZRUL 20210909: Open batch flag for NGC1.5
    }
}
