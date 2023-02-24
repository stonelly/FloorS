using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for Production Line
    /// </summary>
    public class ProductionLineDTO
    {
        /// <summary>
        /// Id 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Line No 
        /// </summary>
        public string LineNumber { get; set; }
        /// <summary>
        /// Glove Tier 
        /// </summary>
        public string GloveTier { get; set; }
        /// <summary>
        /// Glove Type 
        /// </summary>
        public string GloveType { get; set; }
        /// <summary>
        /// Glove Size 
        /// </summary>
        public string GloveSize { get; set; }
        /// <summary>
        /// Batch Frequency
        /// </summary>
        public string BatchFrequency { get; set; }
        /// <summary>
        /// LTGloveType
        /// </summary>
        public string LTGloveType { get; set; }
        /// <summary>
        /// LTAltGlove
        /// </summary>
        public string LTAltGlove { get; set; }
        /// <summary>
        /// LBGloveType
        /// </summary>
        public string LBGloveType { get; set; }
        /// <summary>
        /// LBAltGlove
        /// </summary>
        public string LBAltGlove { get; set; }
        /// <summary>
        /// RTGloveType
        /// </summary>
        public string RTGloveType { get; set; }
        /// <summary>
        /// RTAltGlove
        /// </summary>
        public string RTAltGlove { get; set; }
        /// <summary>
        /// RBGloveType
        /// </summary>
        public string RBGloveType { get; set; }
        /// <summary>
        /// RBAltGlove
        /// </summary>
        public string RBAltGlove { get; set; }
        /// <summary>
        /// LTGloveSize
        /// </summary>
        public string LTGloveSize { get; set; }
        /// <summary>
        /// LBGloveSize
        /// </summary>
        public string LBGloveSize { get; set; }
        /// <summary>
        /// RTGloveSize
        /// </summary>
        public string RTGloveSize { get; set; }
        /// <summary>
        /// RBGloveSize
        /// </summary>
        public string RBGloveSize { get; set; }
        /// <summary>
        /// IsOnline
        /// </summary>
        public bool IsOnline { get; set; }
        /// <summary>
        /// IsPrintByFormer
        /// </summary>
        public bool IsPrintByFormer { get; set; }
        /// <summary>
        /// Plant
        /// </summary>
        public string Plant { get; set; }
        /// <summary>
        /// IsDoubleFormer
        /// </summary>
        public bool IsDoubleFormer { get; set; }
        /// <summary>
        /// Prod logging start date time
        /// </summary>
        public DateTime ProdLoggingStartDateTime { get; set; }
        /// <summary>
        /// Workstation Id
        /// </summary>
        public string WorkstationId { get; set; }

        /// <summary>
        /// Location Id
        /// </summary>
        public int LocationId { get; set; }
        /// <summary>
        /// Id for Operator 
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// Last Modified date time
        /// </summary>
        public DateTime LastModifiedOn { get; set; }
        public Constants.ActionLog ActionType { get; set; }
        
        public string LBFormerType { get; set; }
        public string LTFormerType { get; set; }
        public string RBFormerType { get; set; }
        public string RTFormerType { get; set; }
        public string LatexType { get; set; }

    }

}
