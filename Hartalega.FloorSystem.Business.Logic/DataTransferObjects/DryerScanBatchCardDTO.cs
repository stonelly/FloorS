using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO class for DryerScanBatchCard
    /// </summary>
    public class DryerScanBatchCardDTO
    {
        /// <summary>
        /// Instantiate the class
        /// </summary>
        static DryerScanBatchCardDTO()
        {

        }
        private decimal serialNumber;
        /// <summary>
        /// Serial Number generated for the batch
        /// </summary>        
        public decimal SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }

        private int dryerId;
        /// <summary>
        /// Dryer Id of the batch
        /// </summary>  
        public int DryerId
        {
            get { return dryerId; }
            set { dryerId = value; }
        }

        private string dryerProgram;
        /// <summary>
        /// Dryer Program for the batch
        /// </summary>  
        public string DryerProgram
        {
            get { return dryerProgram; }
            set { dryerProgram = value; }
        }
        public int DryerProgramId { get; set; }

        private TimeSpan startTime;
        /// <summary>
        /// Dryer Start Time of the batch
        /// </summary> 
        public TimeSpan StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private TimeSpan stopTime;
        /// <summary>
        /// Dryer Stop Time of the batch
        /// </summary> 
        public TimeSpan StopTime
        {
            get { return stopTime; }
            set { stopTime = value; }
        }

        private int reworkCount;
        /// <summary>
        /// Rework Count of the batch
        /// </summary> 
        public int ReworkCount
        {
            get { return reworkCount; }
            set { reworkCount = value; }
        }

        private string reworkReason;
        /// <summary>
        /// Rework Reason of the batch
        /// </summary> 
        public string ReworkReason
        {
            get { return reworkReason; }
            set { reworkReason = value; }
        }

        private DateTime lastModifiedOn;
        /// <summary>
        /// Last Modified Date of the batch
        /// </summary> 
        public DateTime LastModifiedOn
        {
            get { return lastModifiedOn; }
            set { lastModifiedOn = value; }
        }
    }
}
