using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class APMLog
    {
        public decimal SerialNumber { get; set; }
        public string Resource { get; set; }
        public bool? APMPackable { get; set; }
        public bool APMStatus { get; set; }
        public int? APMReason { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }

    }
}
