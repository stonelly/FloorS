using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO class for ProductionLineDetail
    /// </summary>
    public class ProductionLineDetailsDTO
    {
        /// <summary>
        /// To Instantiate the class
        /// </summary>
        static ProductionLineDetailsDTO()
        {
        }
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// LineId
        /// </summary>
        public string LineId { get; set; }
        /// <summary>
        /// Formers
        /// </summary>
        public int Formers { get; set; }
        /// <summary>
        /// Speed
        /// </summary>
        public int Speed { get; set; }
        /// <summary>
        /// Cycle
        /// </summary>
        public decimal Cycle { get; set; }
        /// <summary>
        /// LTGloveType
        /// </summary>
        public string LTGloveType{get;set;}
        /// <summary>
        /// LTAltGlove
        /// </summary>
        public string LTAltGlove{get;set;}
        /// <summary>
        /// LTGloveSize
        /// </summary>
        public string LTGloveSize { get; set; }
        /// <summary>
        /// LBGloveType
        /// </summary>
        public string LBGloveType{get;set;}
        /// <summary>
        /// LBAltGlove
        /// </summary>
        public string LBAltGlove{get;set;}
        /// <summary>
        /// LBGloveSize
        /// </summary>
        public string LBGloveSize { get; set; }
        /// <summary>
        /// RTGloveType
        /// </summary>
        public string RTGloveType{get;set;}
        /// <summary>
        /// RTAltGlove
        /// </summary>
        public string RTAltGlove{get;set;}
        /// <summary>
        /// RTGloveSize
        /// </summary>
        public string RTGloveSize { get; set; }
        /// <summary>
        /// RBGloveType
        /// </summary>
        public string RBGloveType{get;set;}
        /// <summary>
        /// RBAltGlove
        /// </summary>
        public string RBAltGlove { get; set; }
        /// <summary>
        /// RBGloveSize
        /// </summary>
        public string RBGloveSize { get; set; }
        /// <summary>
        /// IsDoubleFormer
        /// </summary>
        public bool IsDoubleFormer { get; set; }
        /// <summary>
        /// InValid
        /// </summary>
        public int InValid { get; set; }

    }
}
