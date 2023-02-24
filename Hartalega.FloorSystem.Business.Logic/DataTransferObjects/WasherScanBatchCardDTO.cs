using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for WasherScanBatchCard
    /// </summary>
    public class WasherScanBatchCardDTO
    {
        /// <summary>
        /// Instantiate WasherScanBatchCard
        /// </summary>
       static WasherScanBatchCardDTO()
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

        private int washerId;
        /// <summary>
        /// Washer Id of the batch
        /// </summary>  
        public int WasherId
        {
            get { return washerId; }
            set { washerId = value; }
        }

        private string washerProgram;
        /// <summary>
        /// Washer Program for the batch
        /// </summary>  
        public string WasherProgram
        {
            get { return washerProgram; }
            set { washerProgram = value; }
        }

        private TimeSpan startTime;
        /// <summary>
        /// Washer Start Time of the batch
        /// </summary> 
        public TimeSpan StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private TimeSpan stopTime;
        /// <summary>
        /// Washer Stop Time of the batch
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
