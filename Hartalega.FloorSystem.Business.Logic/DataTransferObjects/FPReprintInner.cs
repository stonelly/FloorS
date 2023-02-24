using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class FPReprintInner
    {
        public string InternalLotNumber { get; set; }
        public int LocationId { get; set; }
        public string WorkStationNumber { get; set; }
        public string PrinterName { get; set; }
        public string AuthhorizedBy { get; set; }

    }
}
