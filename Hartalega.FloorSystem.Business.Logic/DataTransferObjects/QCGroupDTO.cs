using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for QC Group master
    /// </summary>
    public class QCGroupDTO
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// QC Group Name
        /// </summary>
        public string QCGroupName { get; set; }
        /// <summary>
        /// QC Group Description
        /// </summary>
        public string QCGroupDescription { get; set; }
        /// <summary>
        /// QC Group Type
        /// </summary>
        public string QCGroupType { get; set; }
        /// <summary>
        /// IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// IsStopped
        /// </summary>
        public bool IsStopped { get; set; }
        /// <summary>
        /// Workstation Id
        /// </summary>
        public string WorkstationId { get; set; }
        /// <summary>
        /// Id for Operator 
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// plant name
        /// </summary>
        public string LocationName { get; set; }
    }
}
