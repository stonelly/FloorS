using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for WasherProgram
    /// </summary>
    public class WasherProgramDTO
    {
        /// <summary>
        /// Instantiate Washer Program
        /// </summary>
        static WasherProgramDTO()
        {

        }
        /// <summary>
        /// WasherProgramId for WasherProgram
        /// </summary>
        private int washerProgramId;

        public int WasherProgramId
        {
            get { return washerProgramId; }
            set { washerProgramId = value; }
        }

        /// <summary>
        /// WasherProgram for WasherProgram
        /// </summary>
        private string washerProgram;

        public string WasherProgram
        {
            get { return washerProgram; }
            set { washerProgram = value; }
        }

        /// <summary>
        /// Totalminutes for WasherProgram
        /// </summary>
        private decimal totalminutes;

        public decimal Totalminutes
        {
            get { return totalminutes; }
            set { totalminutes = value; }
        }

        /// <summary>
        /// Stopped for WasherProgram
        /// </summary>
        private int stopped;

        public int Stopped
        {
            get { return stopped; }
            set { stopped = value; }
        }

        /// <summary>
        /// Recid for WasherProgram
        /// </summary>
        private long recid;

        public long Recid
        {
            get { return recid; }
            set { recid = value; }
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
