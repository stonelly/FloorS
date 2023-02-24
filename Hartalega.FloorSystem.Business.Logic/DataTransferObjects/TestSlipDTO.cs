using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// This class can be used to pass Test Slip information from one layer to other
    /// </summary>
   public class TestSlipDTO
    {
        /// <summary>
        /// Status of the Serial Number for the Test Slip
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Rework Count for the batch
        /// </summary>
        public int ReworkCount { get; set; }

        /// <summary>
        /// Location Id
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Last Modified Date Time
        /// </summary>
        public DateTime LastModifiedOn { get; set; }

        /// <summary>
        /// Tester Id
        /// </summary>
        public string TesterID { get; set; }

        /// <summary>
        ///  Serial Number for the Test Slip
        /// </summary>
        public decimal SerialNumber { get; set; }

        /// <summary>
        /// Reference Id of the Batch
        /// </summary>
        public decimal ReferenceId { get; set; }

        /// <summary>
        /// Reprint Count of the Batch
        /// </summary>
        public int Reprint { get; set; }

        /// <summary>
        /// Tester Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Result is used in Polymer Test Result
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// WorkstationId
        /// </summary>
        public string WorkstationId { get; set; }
    }     
}
