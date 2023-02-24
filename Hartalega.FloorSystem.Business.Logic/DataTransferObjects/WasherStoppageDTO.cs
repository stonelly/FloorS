using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for Washer Stoppage Data
    /// </summary>
    public class WasherStoppageDTO
    {
        /// <summary>
        /// Id for washer stoppage entry
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id for washer 
        /// </summary>
        public int WasherId { get; set; }
        /// <summary>
        /// Washer Number for a washer
        /// </summary>
        public int WasherNumber { get; set; }       
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
