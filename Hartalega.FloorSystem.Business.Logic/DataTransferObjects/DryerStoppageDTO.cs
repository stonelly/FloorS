using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for Dryer Stoppage Data
    /// </summary>
    public class DryerStoppageDTO
    {
        /// <summary>
        /// Id for dryer stoppage entry
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id for dryer 
        /// </summary>
        public int DryerId { get; set; }
        /// <summary>
        /// Dryer Number for a dryer
        /// </summary>
        public int DryerNumber { get; set; }
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
