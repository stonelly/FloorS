using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{   
    /// <summary>
    /// DTO class for ProductionDefectDetail
    /// </summary>
    public class ProductionDefectDetailDTO
    {
        /// <summary>
        /// Instantiates the class
        /// </summary>
        static ProductionDefectDetailDTO()
        {

        }
        /// <summary>
        /// ProductionDefectDetailId
        /// </summary>
        public int Id { get; set;}
        /// <summary>
        /// ProductionDefectId
        /// </summary>
        public int ProductionDefectId { get; set; }
        /// <summary>
        /// DefectQuantity
        /// </summary>
        public int DefectQuantity { get; set; }
        /// <summary>
        /// DefectType
        /// </summary>
        public string DefectType { get; set; }
        /// <summary>
        /// DefectDescription
        /// </summary>
        public string DefectDescription { get; set; }
    }
}
