using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class DryerProcessDTO
    {
        public int CycloneProcessID { get; set; }
        public string CycloneProcess { get; set; }

        public decimal Cold { get; set; }

        public decimal Hot { get; set; }

        public decimal RCold { get; set; }

        public decimal RHot { get; set; }

        public decimal R2Cold { get; set; }

        public decimal R2Hot { get; set; }

        public string DataAreaId { get; set; }

        public int Recversion { get; set; }
        public long RecId { get; set; }

        public long Partition { get; set; }

        public int Stopped { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

    }
}
