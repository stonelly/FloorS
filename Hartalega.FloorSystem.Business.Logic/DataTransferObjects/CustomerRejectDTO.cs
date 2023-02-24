using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// CustomerRejectDTO
    /// </summary>  
   [XmlRoot("CustomerRejectBatchDetails")]
    public class CustomerRejectDTO
    {
        /// <summary>
        /// InternalLotNumber
        /// </summary>
        [XmlElement("InternalLotNumber")]
        public string InternalLotNumber { get; set; }

        /// <summary>
        /// GloveType
        /// </summary>
        [XmlElement("GloveType")]
        public string GloveType { get; set; }

        /// <summary>
        /// Size
        /// </summary>
        [XmlElement("Size")]
        public string Size { get; set; }

        /// <summary>
        /// SizeSelected
        /// </summary>
        [XmlElement("SizeSelected")]
        public string SizeSelected { get; set; }

        /// <summary>
        /// ShiftName
        /// </summary>
        [XmlElement("ShiftName")]
        public string ShiftName { get; set; }

        /// <summary>
        /// ShiftId
        /// </summary>
        [XmlElement("ShiftId")]
        public int ShiftId { get; set; }

        /// <summary>
        /// Line
        /// </summary>
        [XmlElement("Line")]
        public string Line { get; set; }

        /// <summary>
        /// TenPcsWeight
        /// </summary>
        [XmlElement("TenPcsWeight")]
        public decimal TenPcsWeight { get; set; }

        /// <summary>
        /// BatchWeight
        /// </summary>
        [XmlElement("BatchWeight")]
        public decimal BatchWeight { get; set; }

        /// <summary>
        /// CustomerName
        /// </summary>
        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [XmlElement("Description")]
        public string Description { get; set; }

         /// <summary>
        /// Batch Number
        /// </summary>
        public string BatchNumber { get; set; }       

        /// <summary>
        /// FGCode
        /// </summary>
        public string FGCode { get; set; }

        /// <summary>
        /// FGCode
        /// </summary>
        public string FGSize { get; set; }

        public string SerialNumber;

        /// <summary>
        /// OperatorId
        /// </summary>
        [XmlElement("OperatorId")]
        public string OperatorId { get; set; }

        /// <summary>
        /// CasesPacked
        /// </summary>
        [XmlElement("CasesPacked")]
        public int CasesPacked { get; set; }
        /// <summary>
        /// CaseCapacity
        /// </summary>
        [XmlElement("CaseCapacity")]
        public int CaseCapacity { get; set; }
    }
}
