using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// QA System DTO
    /// </summary>
    public class QASystemDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Serial Number
        /// </summary>
        public Int64 SerialNumber { get; set; }
        /// <summary>
        /// Reference
        /// </summary>
        public Int64 Reference { get; set; }
        /// <summary>
        /// Test Date Time
        /// </summary>
        public DateTime TestDateTime { get; set; }
        /// <summary>
        /// Opeator Id
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// Operator Name
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// IsResultPass
        /// </summary>
        public bool IsResultPass { get; set; }
        /// <summary>
        /// Filter Paper Weight First
        /// </summary>
        public decimal FilterPaperWtFirst { get; set; }
        /// <summary>
        /// Filter Paper Weight Second
        /// </summary>
        public decimal FilterPaperWtSecond { get; set; }
        /// <summary>
        /// Filter Paper Weight after Filteration
        /// </summary>
        public decimal FilterPaperWtAfterFilteration { get; set; }
        /// <summary>
        /// Filter Paper Weight and Residue Powder
        /// </summary>
        public decimal FilterPaperWtAndResiduePowder { get; set; }
        /// <summary>
        /// Residue Powder
        /// </summary>
        public decimal ResiduePowder { get; set; }
        /// <summary>
        /// Weight
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// Protein Content
        /// </summary>
        public decimal ProteinContent { get; set; }
        /// <summary>
        /// Test Tab
        /// </summary>
        public string TestTab { get; set; }
        /// <summary>
        /// Workstation Id
        /// </summary>
        public string WorkstationId { get; set; }
    }
}
