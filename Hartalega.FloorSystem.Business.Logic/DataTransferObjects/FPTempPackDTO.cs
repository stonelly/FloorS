using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class FPTempPackDTO 
    {
        public decimal SerialNumber { get; set; }
        public string TempPackReason { get; set; }
        public int TempPackPcs { get; set; }
        public int LocationID { get; set; }
        public string PrinterName { get; set; }
        public string WorkStationNumber { get; set; }
        public Boolean isTempPackBatch { get; set; }

        public BatchDTO TMPPackBatch { get; set; }

    }
}
