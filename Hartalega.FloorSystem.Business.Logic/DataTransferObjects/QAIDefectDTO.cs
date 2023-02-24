using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;


namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// QAI Defect DTO
    /// </summary>
    [XmlRoot("QAIDefect")]
    public class QAIDefectDTO
    {
        /// <summary>
        /// DefectID
        /// </summary>
        [XmlElement("DefectID")]
        public int DefectID { get; set; }
        /// <summary>
        /// DefectItem
        /// </summary>
        [XmlElement("DefectItem")]
        public string DefectItem { get; set; }

        /// <summary>
        /// KeyStroke
        /// </summary>
        [XmlElement("KeyStroke")]
        public char KeyStroke { get; set; }

        [XmlElement("KeyStrokeAltName")]
        public string KeyStrokeAltName { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        [XmlElement("Count")]
        public int Count { get; set; }

        /// <summary>
        /// HasChild
        /// </summary>
        [XmlElement("HasChild")]
        public bool HasChild { get; set; }

        /// <summary>
        /// List QAIDefectPositionDTO
        /// </summary>
        [XmlElement("DefectPositionList")]
        public List<QAIDefectPositionDTO> DefectPositionList { get; set; }
    }

    /// <summary>
    /// QAIDefects
    /// </summary>
    [XmlRoot("QAIDefectType")]
    public class QAIDefectType
    {
        /// <summary>
        /// Defect Category
        /// </summary>
        [XmlElement("DefectCategory")]
        public string DefectCategory { get; set; }

        /// <summary>
        /// Sequence
        /// </summary>
        [XmlElement("Sequence")]
        public int Sequence { get; set; }

        /// <summary>
        /// DefectCategoryID
        /// </summary>
        [XmlElement("DefectCategoryID")]
        public int DefectCategoryID { get; set; }

        /// <summary>
        /// IsCaptured
        /// </summary>
        [XmlElement("IsCaptured")]
        public bool IsCaptured { get; set; }

        /// <summary>
        /// List QAIDefectDTO
        /// </summary>
        [XmlElement("DefectList")]
        public List<QAIDefectDTO> DefectList { get; set; }
    }

    /// <summary>
    /// QAI Defect Positon DTO
    /// </summary>
    [XmlRoot("QAIDefectPosition")]
    public class QAIDefectPositionDTO
    {
        /// <summary>
        /// DefectID
        /// </summary>
        [XmlElement("DefectPositionID")]
        public int DefectPositionID { get; set; }
        /// <summary>
        /// DefectItem
        /// </summary>
        [XmlElement("DefectPositionItem")]
        public string DefectPositionItem { get; set; }

        /// <summary>
        /// KeyStroke
        /// </summary>
        [XmlElement("KeyStroke")]
        public char KeyStroke { get; set; }

        [XmlElement("KeyStrokeAltName")]
        public string KeyStrokeAltName { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        [XmlElement("Count")]
        public int Count { get; set; }
    }

}
