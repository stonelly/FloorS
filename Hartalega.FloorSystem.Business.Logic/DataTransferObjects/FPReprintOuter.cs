using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class RePrintOuter
    {
        public int LocationId { get; set; }
        public string WorkStationNumber { get; set; }
        public string PrinterName { get; set; }
        public string PONumber { get; set; }
        public string ItemNumber { get; set; }

        public string ItemSize { get; set; }
        public int StartCase { get; set; }
        public int EndCase { get; set; }
        public string InternalLotNo { get; set; }
        public int NoOfCopy { get; set; }

        public string AuthorizedBy { get; set; }


    }
}
