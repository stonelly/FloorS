using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// QCMemberDetailsDTO class
    /// </summary>
    public class QCMemberDetailsDTO
    {
        /// <summary>
        /// QCGroupId
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// MemberId
        /// </summary>
        public string MemberId { get; set; }
        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
    }
}
