using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// Glove Inquiry DTO Class
    /// </summary>
    public class GloveInquiryDetails
    {
        /// <summary>
        /// Serial Number generated for the batch
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Glove Type of the batch
        /// </summary>
        public string GloveType { get; set; }
        /// <summary>
        /// QC Type of the batch
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// QC Type of the batch
        /// </summary>
        public string QCType { get; set; }
        /// <summary>
        /// Batch Weight
        /// </summary>
        public string BatchWeight { get; set; }
        /// <summary>
        /// Ten Pieces Weight of the batch
        /// </summary>
        public string TenPcsWeight { get; set; }
        /// <summary>
        /// Total Pieces in the batch
        /// </summary>
        public string TotalPcs { get; set; }
        /// <summary>
        /// Batch Creation Date
        /// </summary>
        public string BatchDate { get; set; }
        /// <summary>
        /// Bin Number of the batch
        /// </summary>
        public string BinId { get; set; }
        /// <summary>
        /// Number of days the batch is scanned in
        /// </summary>
        public string NoOfDays { get; set; }
    }
}
