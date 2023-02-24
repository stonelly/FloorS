using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class GISDTO
    {
        /// <summary>
        /// Serial Number generated for the batch
        /// </summary>
        public decimal SerialNumber { get; set; }
        /// <summary>
        /// Location Id of the Workstation
        /// </summary>
        public int LocationId { get; set; }
        /// <summary>
        /// Bin Number of the batch
        /// </summary>
        public string BinNumber { get; set; }
        /// <summary>
        /// Last Modified Date
        /// </summary>
        public DateTime LastModifiedOn { get; set; }
        /// <summary>
        /// Used for checking whether we are updating the existing row or inserting a new row
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Next Process for the Batch
        /// </summary>
        public string NextProcess { get; set; }

        /// <summary>
        /// Workstation Id for the Batch
        /// </summary>
        public int WorkstationId { get; set; }

        /// <summary>
        /// GIS Identifier for Glove Inventory System
        /// </summary>
        public int GISId { get; set; }
        /// <summary>
        /// ScanIn Date required to store before update
        /// </summary>
        public DateTime ScanInDate { get; set; }
      
    }

    public class GISBatchInfo
    {
        public string SerialNumber { get; set; }

        public decimal BatchWeight { get; set; }

        public decimal TenPcsWeight { get; set; }

        public int TotalPcs { get; set; }
    }
}
