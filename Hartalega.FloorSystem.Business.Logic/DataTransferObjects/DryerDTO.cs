using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO class Dryer
    /// </summary>
    public class DryerDTO
    {
        /// <summary>
        /// Intantiate the class
        /// </summary>
        static DryerDTO()
        {

        }
        /// <summary>
        /// Dryer number for Dryer
        /// </summary>
        private int dryerNumber;

        public int DryerNumber
        {
            get { return dryerNumber; }
            set { dryerNumber = value; }
        }

        /// <summary>
        /// Id for washer
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// GloveType
        /// </summary>       
        public string GloveType { get; set; }
        /// <summary>
        /// GloveType Description
        /// </summary>
        [System.ComponentModel.DisplayName("Glove Type Description")]
        public string GloveTypeDescription { get; set; }
        /// <summary>
        /// GloveSize
        /// </summary>
        [System.ComponentModel.DisplayName("Size")]
        public string GloveSize { get; set; }
        /// <summary>
        /// Hot
        /// </summary>
        [System.ComponentModel.DisplayName("HOT")]
        public bool Hot { get; set; }
        /// <summary>
        ///Cold
        /// </summary>
        [System.ComponentModel.DisplayName("COLD")]
        public bool Cold { get; set; }
        /// <summary>
        /// HotAndCold
        /// </summary>
        [System.ComponentModel.DisplayName("HOT and COLD")]
        public bool HotAndCold { get; set; }
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
        /// Workstation Id
        /// </summary>
        public string WorkstationId { get; set; }
        /// <summary>
        /// Id for Operator 
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// Id for Location 
        /// </summary>
        public string LocationId { get; set; }
        /// <summary>
        /// IsDeleted
        /// </summary>
        [System.ComponentModel.DisplayName("Delete")]
        public bool IsDeleted { get; set; }
    }
}
