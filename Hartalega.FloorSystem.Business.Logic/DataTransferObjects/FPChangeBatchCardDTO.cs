using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class FPChangeBatchCardDTO
    {
        public string InternalLotNumber { get; set; }
        public decimal OldSerialNumber { get; set; }
        public decimal NewSerialNumber { get; set; }
        public int GroupId { get; set; }
        public int LocationId { get; set; }
    }
    public class FPChangeBatchCardV2DTO // #AzmanCBCI
    {
        public string InternalLotNumber { get; set; }
        public int InternalLotNumberQty { get; set; }
        public decimal OrigSerialNumber { get; set; }

        public decimal NewSerialNumber1 { get; set; }
        public decimal NewSerialNumber2 { get; set; }
        public decimal NewSerialNumber3 { get; set; }

        public int NewSerialNumber1Qty { get; set; }
        public int NewSerialNumber2Qty { get; set; }
        public int NewSerialNumber3Qty { get; set; }

        public int NewSerialNumber1QtyUse { get; set; }
        public int NewSerialNumber2QtyUse { get; set; }
        public int NewSerialNumber3QtyUse { get; set; }

        public int NewSerialNumber1QtyCase { get; set; }
        public int NewSerialNumber2QtyCase { get; set; }
        public int NewSerialNumber3QtyCase { get; set; }

        public int NewSerialNumber1QtyInner { get; set; }
        public int NewSerialNumber2QtyInner { get; set; }
        public int NewSerialNumber3QtyInner { get; set; }

        public int GroupId { get; set; }
        public int LocationId { get; set; }
        public int PackSize { get; set; }
        public string ItemNumber { get; set; }
        public string PSIReworkOrderNo { get; set; }
    }
}
