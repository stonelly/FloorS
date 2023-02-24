using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for ReasonMaster
    /// </summary>
    public class ReasonDTO
    {
        /// <summary>
        /// Reason type for a module
        /// </summary>
        [System.ComponentModel.DisplayName("Reason Type")]
        public string ReasonType { get; set; }
        /// <summary>
        /// Reason text for a module
        /// </summary>
        [System.ComponentModel.DisplayName("Reason Text")]
        public string ReasonText { get; set; }
        /// <summary>
        /// Reason is scheduled for a module
        /// </summary>
        [System.ComponentModel.DisplayName("Scheduled")]
        public bool IsScheduled { get; set; }
        /// <summary>
        /// Reason type ID for a module
        /// </summary>
        public int ReasonTypeId { get; set; }
        /// <summary>
        /// Reason text ID for a module
        /// </summary>
        public int ReasonTextId { get; set; }
        /// <summary>
        /// Workstation Id
        /// </summary>
        public string WorkstationId { get; set; }
        /// <summary>
        /// Id for Operator 
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// Short Code
        /// </summary>
        public string ShortCode { get; set; }
    }
}
