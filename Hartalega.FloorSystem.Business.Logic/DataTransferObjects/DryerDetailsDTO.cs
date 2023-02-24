using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class DryerDetailsDTO
    {
        /// <summary>
        /// Id for washer
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// DryerNumber
        /// </summary>
        [System.ComponentModel.DisplayName("Dryer Number")]
        public int DryerNumber { get; set; }
        /// <summary>
        /// GloveType
        /// </summary>
        [System.ComponentModel.DisplayName("Glove Type Description")]
        public string GloveType { get; set; }
        /// <summary>
        /// GloveSize
        /// </summary>
        [System.ComponentModel.DisplayName("Size")]
        public string GloveSize { get; set; }
        /// <summary>
        ///Cold
        /// </summary>
        [System.ComponentModel.DisplayName("COLD")]
        public int Cold { get; set; }
        /// <summary>
        /// HotAndCold
        /// </summary>
        [System.ComponentModel.DisplayName("HOT and COLD")]
        public int HotAndCold { get; set; }
        /// <summary>
        /// IsStopped
        /// </summary>
        [System.ComponentModel.DisplayName("Stop")]
        public bool IsStopped { get; set; }
        /// <summary>
        /// IsScheduledStop
        /// </summary>
        [System.ComponentModel.DisplayName("Scheduled Stop")]
        public bool IsScheduledStop { get; set; }
        /// <summary>
        /// CheckGlove
        /// </summary>
        [System.ComponentModel.DisplayName("Check Glove")]
        public bool CheckGlove { get; set; }
        /// <summary>
        /// CheckSize
        /// </summary>
        [System.ComponentModel.DisplayName("Check Size")]
        public bool CheckSize { get; set; }
        /// <summary>
        /// Hot
        /// </summary>
        [System.ComponentModel.DisplayName("HOT")]
        public int Hot { get; set; }
    }
}
