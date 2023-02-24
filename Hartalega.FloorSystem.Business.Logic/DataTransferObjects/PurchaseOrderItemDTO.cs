using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class PurchaseOrderItemDTO
    {
        public int CaseNumber { get; set; }
        public int StartCaseNumber { get; set; }
        public int EndCaseNumber { get; set; }        
        public Boolean isPreshipment { get; set; }
        public string PalletId { get; set; }
        public string internallotnumber { get; set; }
    }
}
