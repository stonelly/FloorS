using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO class for TierSideMaster
    /// </summary>
    public class TierSideMasterDTO
    {
        /// <summary>
        /// Instantiates the class
        /// </summary>
        static TierSideMasterDTO()
        {

        }
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// TierSide
        /// </summary>
        public string TierSide { get; set; }
    }
}
