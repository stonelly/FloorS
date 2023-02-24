using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO class for defect summary procedure
    /// </summary>
    public class ProductionDefectSummaryDTO
    {
        /// <summary>
        /// Instantiates the class
        /// </summary>
        static ProductionDefectSummaryDTO()
        {

        }
        /// <summary>
        /// SerialNumber
        /// </summary>
        public decimal SerialNumber { get; set; }
        /// <summary>
        /// QAIDefectQuantity
        /// </summary>
        public int QAIDefectQuantity { get; set; }
        /// <summary>
        /// LineNumber
        /// </summary>
        public string LineNumber { get; set;}
        /// <summary>
        /// QAIDate
        /// </summary>
        public DateTime QAIDate { get; set; }
        /// <summary>
        /// DefectTime
        /// </summary>
        public string DefectTime { get; set; }
        /// <summary>
        /// Size
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// TierSide
        /// </summary>
        public string TierSide { get; set; }
        /// <summary>
        /// PNDefectQuantity
        /// </summary>
        public int PNDefectQuantity { get; set; }
        /// <summary>
        /// ProductionDefectId
        /// </summary>
        public int ProductionDefectId { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// DefectCategoryId
        /// </summary>
        public int DefectCategoryId { get; set; }

    }
}
