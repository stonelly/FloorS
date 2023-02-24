using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
        /// <summary>
        /// DTO class for ProductionDefect
        /// </summary>
        public class ProductionDefectDTO
        {
            /// <summary>
            /// Instantiates the class
            /// </summary>
            static ProductionDefectDTO()
            {

            }
            /// <summary>
            /// ProductionDefectId
            /// </summary>
            public int ProductionDefectId { get; set; }
            /// <summary>
            /// SerialNumber
            /// </summary>
            public decimal SerialNumber { get; set; }
            /// <summary>
            /// LineId
            /// </summary>
            public string LineId { get; set; }
            /// <summary>
            /// GloveSize
            /// </summary>
            public string GloveSize { get; set; }
            /// <summary>
            /// TierSide
            /// </summary>
            public string TierSide { get; set; }
            /// <summary>
            /// QAIDefectQuantity
            /// </summary>
            public int QAIDefectQuantity { get; set; }
            /// <summary>
            /// PNDefectQuantity
            /// </summary>
            public int PNDefectQuantitry { get; set; }
            /// <summary>
            /// DefectDate
            /// </summary>
            public DateTime DefectDate { get; set; }
            /// <summary>
            /// DefectTime
            /// </summary>
            public string DefectTime { get; set; }
            /// <summary>
            /// LastModifiedOn
            /// </summary>
            public DateTime LastModifiedOn { get; set; }
            /// <summary>
            /// WorkStationNumber
            /// </summary>
            public string WorkStationNumber { get; set; }
        
    }
}
