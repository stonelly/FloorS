using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Hartalega.FloorSystem.Framework;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// QAI DTO
    /// </summary>        
    [Serializable]
    [XmlRoot("QAIDetails")]
    public class QAIDTO : ICloneable
    {
        /// <summary>
        /// QAIId
        /// </summary>
        [XmlElement("Id")]
        public int QAIId { get; set; }

        /// <summary>
        /// QAIDate
        /// </summary>
        [XmlElement("QAIDateTime")]
        public DateTime? QAIDate { get; set; }

        /// <summary>
        /// InspectorId
        /// </summary>
        [XmlElement("QAIInspectorId")]
        public string InspectorId { get; set; }

        /// <summary>
        /// SerialNo
        /// </summary>
        [XmlElement("SerialNumber")]
        public string SerialNo { get; set; }

        /// <summary>
        /// BatchNo
        /// </summary>
        [XmlElement("BatchNumber")]
        public string BatchNo { get; set; }

        /// <summary>
        /// Selected QCType
        /// </summary>
        [XmlElement("QCType")]
        public string QCType { get; set; }

        /// <summary>
        ///Suggested QCType 
        /// </summary>
        [XmlElement("SuggestedQCType")]
        public string SuggestedQCType { get; set; }


        /// <summary>
        ///Suggested QCType 
        /// </summary>
        [XmlElement("SuggestedQCTypeYESNO")]
        public string SuggestedQCTypeYESNO { get; set; }

        /// <summary>
        /// WTSamplingSize
        /// </summary>
        [XmlElement("WTSampliingSize")]
        public string WTSamplingSize { get; set; }

        /// <summary>
        /// VTSamplingSize
        /// </summary>
        [XmlElement("VTSamplingSize")]
        public string VTSamplingSize { get; set; }

        /// <summary>
        /// IsReSampling
        /// </summary>    

        [XmlElement("IsResampling")]
        public Boolean IsReSampling { get; set; }

        /// <summary>
        /// InnerBoxes
        /// </summary>
        [XmlElement("InnerBox")]
        public int InnerBoxes { get; set; }

        /// <summary>
        /// Packing Size
        /// </summary>
        [XmlElement("PackingSize")]
        public string PackingSize { get; set; }

        /// <summary>
        /// EditChangeReason
        /// </summary>
        [XmlElement("EditChangeReason")]
        public string EditChangeReason { get; set; }

        /// <summary>
        /// ChangeQCTypeReason
        /// </summary>
        [XmlElement("QAIChangeReason")]
        public string ChangeQCTypeReason { get; set; }

        /// <summary>
        /// Defects
        /// </summary>
        //[XmlElement("QAIDefectTypeList")]
        public List<QAIDefectType> Defects { get; set; }

        /// <summary>
        /// QITestResult
        /// </summary>
        [XmlElement("QAITestResult")]
        public string QAITestResult { get; set; }

        /// <summary>
        /// QITestReason
        /// </summary>
        [XmlElement("QITestReason")]
        public string QITestReason { get; set; }

        /// <summary>
        /// WorkStation Number
        /// </summary>
        [XmlElement("WorkStationNumber")]
        public string WorkStationNumber { get; set; }

        /// <summary>
        /// SubModuleName
        /// </summary>        
        public string SubModuleName { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string QaiInspectorName { get; set; }

        /// <summary>
        /// Isonline
        /// </summary>        
        public bool? Isonline { get; set; }

        /// <summary>
        /// TenPicWeight
        /// </summary>
        [XmlElement("TenPcsWeight")]
        public decimal TenPcsWeight { get; set; }

        /// <summary>
        /// GloveType produced
        /// </summary>
        [XmlElement("GloveType")]
        public string GloveType { get; set; }
        /// <summary>
        /// Size of the Glove Type Produced
        /// </summary>
        public string Size { get; set; }


        /// <summary>
        /// Batch Weight from Platform Scale
        /// </summary>        
        [XmlElement("BatchWeight")]
        public decimal BatchWeight { get; set; }

        /// <summary>
        /// Screen Names
        /// </summary>
        public Constants.QAIScreens? ScreenName { get; set; }

        /// <summary>
        /// TotalPCs =Innerbox x Packing Size
        /// </summary>
        [XmlElement("TotalPCs")]
        public int TotalPCs { get; set; }

        /// <summary>
        /// Screen Title
        /// </summary>
        public string ScreenTitle { get; set; }

        /// <summary>
        /// QCTypeAuthorizedBy
        /// </summary>
        [XmlElement("QCTypeAuthorizedBy")]
        public string QCTypeAuthorizedBy { get; set; }

        /// <summary>
        /// AQLValue
        /// </summary>
        [XmlElement("AQLValue")]
        public string AQLValue { get; set; }

        /// <summary>
        /// Is Straing Pack No Defects
        /// </summary>
        public bool IsStraingPack { get; set; }

        /// <summary>
        ///Need to clone to avoid pointing to same reference when instantiated
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            QAIDTO objqaidto = (QAIDTO)this.MemberwiseClone();
            return objqaidto;
        }

        /// <summary>
        /// Hot Box Sampling Size
        /// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
        /// </summary>
        [XmlElement("HBSamplingSize")]
        public string HBSamplingSize { get; set; }

        public decimal BatchWeight_NoRounding { get; set; }
        public decimal TenPcsWeight_NoRounding { get; set; }
        /// <summary>
        /// 10PCSSamplingSize 
        /// #Azman 21/02/2018 
        /// #Azrul 13/07/2018: Merged from Live AX6
        /// </summary>
        [XmlElement("TenPcsSamplingSize")]
        public string TenPCSSamplingSize { get; set; }


        [XmlElement("CustomerTypeId")]
        public string CustomerTypeId { get; set; }
    }








}
