using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class EditQCEfficiencyDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// SerialNo
        /// </summary>
        public string SerialNo { get; set; }
        /// <summary>
        /// Glove
        /// </summary>
        public string Glove { get; set; }
        /// <summary>
        /// BatchNo
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// Brand
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// BatchWeight
        /// </summary>
        public decimal BatchWeight { get; set; }
        /// <summary>
        /// BatchStatus
        /// </summary>
        public string BatchStatus { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Group
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// StartTime
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// EndTime
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// NoPerson
        /// </summary>
        public int NoPerson { get; set; }

        /// <summary>
        /// Rework
        /// </summary>
        public int Rework { get; set; }

        /// <summary>
        /// ReworkReason
        /// </summary>
        public string ReworkReason { get; set; }

        /// <summary>
        /// PackingSize
        /// </summary>
        public int PackingSize { get; set; }

        /// <summary>
        /// InnerBoxCount
        /// </summary>
        public int InnerBoxCount { get; set; }

        /// <summary>
        /// QCType
        /// </summary>
        public string QCType { get; set; }

        /// <summary>
        /// QCGroupMembers
        /// </summary>
        public string QCGroupMembers { get; set; }

        /// <summary>
        /// TenPcsWeight
        /// </summary>
        /// Added by Tan Wei Wah 20190131
        public decimal TenPcsWeight { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        /// Added by Tan Wei Wah 20190131
        public string Reason { get; set; }
    }

    public class QCGroupMembersDTO
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }

}
