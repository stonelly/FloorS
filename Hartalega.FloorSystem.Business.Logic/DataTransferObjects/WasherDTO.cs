using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for Washer
    /// </summary>
   public class WasherDTO
    {
        /// <summary>
        /// Id for washer
        /// </summary>
        [System.ComponentModel.DisplayName("Index")]
        public int Id { get; set; }
        /// <summary>
        /// Washer Number for a washer
        /// </summary>
        [System.ComponentModel.DisplayName("Washer No")]
        public int WasherNumber { get; set; }
        /// <summary>
        /// Glove Type
        /// </summary>
        public string GloveType { get; set; }
        /// <summary>
        /// Glove Type Description
        /// </summary>
        [System.ComponentModel.DisplayName("Glove Description")]
        public string GloveTypeDescription { get; set; }
        /// <summary>
        /// Glove Size
        /// </summary>
        [System.ComponentModel.DisplayName("Size")]
        public string GloveSize { get; set; }
        /// <summary>
        /// IStopped
        /// </summary>
        public bool Stop { get; set; }
        /// <summary>
        /// ISDeleted
        /// </summary>
        public bool Delete { get; set; }
        /// <summary>
        /// Workstation Id
        /// </summary>
        public string WorkstationId { get; set; }
        /// <summary>
        /// Id for Operator 
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// Location Id
        /// </summary>
        public string LocationId { get; set; }
    }
}
