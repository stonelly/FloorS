using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO class for DefectTypeMaster
    /// </summary>
    public class DefectTypeDTO
    {
        /// <summary>
        /// Instantiates the class
        /// </summary>
        static DefectTypeDTO()
        {

        }
        /// <summary>
        /// ProdDefectId
        /// </summary>
        public int ProdDefectId { set; get; }
        /// <summary>
        /// ProdDefectName
        /// </summary>
        public string ProdDefectName { set; get; }
        /// <summary>
        /// DefectDescription
        /// </summary>
        public string DefectDescription { set; get; }
    }
}
