using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for QC/Packing Group Stoppage Data
    /// </summary>
    public class QCGroupStoppageDTO
    {
        /// <summary>
        /// Id for QC/Packing Group stoppage entry
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id for QC/Packing Group 
        /// </summary>
        public int QCGroupId { get; set; }
        /// <summary>
        /// QC/Packing Group Name
        /// </summary>
        public string QCGroupName { get; set; }
        /// <summary>
        /// QC/Packing Group Type
        /// </summary>
        public string GroupType { get; set; }
        /// <summary>
        /// Stoppage Date
        /// </summary>
        public DateTime StoppageDate { get; set; }
        /// <summary>
        /// Id for reason 
        /// </summary>
        public int ReasonId { get; set; }
        /// <summary>
        /// Text for reason 
        /// </summary>
        public string ReasonText { get; set; }
        /// <summary>
        /// Id for Operator 
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// Name of the Operator 
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// Workstation Id
        /// </summary>
        public string WorkstationId { get; set; }
    }
}
