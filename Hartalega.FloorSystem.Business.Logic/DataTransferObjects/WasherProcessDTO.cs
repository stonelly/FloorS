using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for Washerprocess
    /// </summary>
    public class WasherprocessDTO
    {
        /// <summary>
        /// Instantiate Washer process
        /// </summary>
        static WasherprocessDTO()
        {

        }
        /// <summary>
        /// WasherProcessId for Washerprocess
        /// </summary>
        private int washerprocessId;

        public int WasherprocessId
        {
            get { return washerprocessId; }
            set { washerprocessId = value; }
        }

        /// <summary>
        /// Process for Washerprocess
        /// </summary>
        private string process;

        public string Process
        {
            get { return process; }
            set { process = value; }
        }

        /// <summary>
        /// Minutes for Washerprocess
        /// </summary>
        private decimal minutes;

        public decimal Minutes
        {
            get { return minutes; }
            set { minutes = value; }
        }

        /// <summary>
        /// Stage for Washerprocess
        /// </summary>
        private string stage;

        public string Stage
        {
            get { return stage; }
            set { stage = value; }
        }

        /// <summary>
        /// Recid for Washerprocess
        /// </summary>
        private long recid;

        public long Recid
        {
            get { return recid; }
            set { recid = value; }
        }

        /// <summary>
        /// washerRefId for Washerprocess
        /// </summary>
        private long washerRefId;

        public long WasherRefId
        {
            get { return washerRefId; }
            set { washerRefId = value; }
        }


        private DateTime createdDateTime;
        /// <summary>
        /// 
        /// </summary> 
        public DateTime CreatedDateTime
        {
            get { return createdDateTime; }
            set { createdDateTime = value; }
        }

        private DateTime modifiedDateTime;
        /// <summary>
        /// 
        /// </summary> 
        public DateTime ModifiedDateTime
        {
            get { return modifiedDateTime; }
            set { modifiedDateTime = value; }
        }
    }
}
