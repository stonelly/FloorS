using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for Downgrade Batch card
    /// </summary>
    public class DowngradeBatchCardDTO
    {
        /// <summary>
        /// Downgrade batch card ID for a batch
        /// </summary>
        public int DowngradeBatchCardId { get; set; }
        /// <summary>
        /// Serial Number generated for the batch
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Downgrade type generated for the batch
        /// </summary>
        public string DowngradeType { get; set; }
        /// <summary>
        /// Last modified by for the downgrade batch card
        /// </summary>
        public string LastModifiedBy { get; set; }
        /// <summary>
        /// Last modified on for the downgrade batch card
        /// </summary>
        public DateTime LastModifiedOn { get; set; }
        /// <summary>
        /// Workstation Id
        /// </summary>
        public string WorkstationId { get; set; }
    }
}
