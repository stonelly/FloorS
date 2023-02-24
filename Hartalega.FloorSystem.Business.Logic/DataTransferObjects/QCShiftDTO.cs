using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// QCShiftDTO class
    /// </summary>
    public class QCShiftDTO
    {
        /// <summary>
        /// ShiftId
        /// </summary>
        public int ShiftId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// InTime
        /// </summary>
        public TimeSpan InTime { get; set; }
        /// <summary>
        /// OutTime
        /// </summary>
        public TimeSpan OutTime { get; set; }
        /// <summary>
        /// GroupType
        /// </summary>
        public string GroupType { get; set; }
    }
}
