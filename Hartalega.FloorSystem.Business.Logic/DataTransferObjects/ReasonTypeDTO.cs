using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for reason type master
    /// </summary>
    public class ReasonTypeDTO
    {
        /// <summary>
        /// Instantiate ReasonTypeDTO
        /// </summary>
        static ReasonTypeDTO()
        {
        }

        /// <summary>
        /// Module name
        /// </summary>
        [System.ComponentModel.DisplayName("System")]
        public string ModuleName { get; set; }
        /// <summary>
        /// Reason type for a module
        /// </summary>
        [System.ComponentModel.DisplayName("Reason Type")]
        public string ReasonType { get; set; }
        /// <summary>
        /// Reason type Id for a module
        /// </summary>
        public int ReasonTypeId { get; set; }
        /// <summary>
        /// IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Module Id for a module
        /// </summary>
        public string ModuleId { get; set; }
        
    
    }
}
