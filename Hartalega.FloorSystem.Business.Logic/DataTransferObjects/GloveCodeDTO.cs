using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class GloveCodeDTO
    {       
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// GloveCode
        /// </summary>
        public string GloveCode { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Barcode
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// GloveCategory
        /// </summary>
        public string GloveCategory { get; set; }

        /// <summary>
        /// Hotbox
        /// </summary>
        public int Hotbox { get; set; }
        /// <summary>
        /// ProteinSpecification
        /// </summary>
        public decimal ProteinSpecification { get; set; }
        /// <summary>
        /// Polymer
        /// </summary>
        public int Polymer { get; set; }
        /// <summary>
        /// Powder
        /// </summary>
        public int Powder { get; set; }
        /// <summary>
        /// Protein
        /// </summary>
        public int Protein { get; set; }
        /// <summary>
        /// PowderFormula
        /// </summary>
        public int PowderFormula { get; set; }

    }

    public class WasherGloveDTO
    {
        /// <summary>
        /// WasherId
        /// </summary>
        public int WasherId { get; set; }
        /// <summary>
        /// WasherProgram
        /// </summary>
        public string WasherProgram { get; set; }
        /// <summary>
        /// GloveCode
        /// </summary>
        public string GloveCode { get; set; }

    }

    public class DryerGloveDTO
    {
        /// <summary>
        /// DryerId
        /// </summary>
        public int DryerId { get; set; }
        /// <summary>
        /// WasherProgram
        /// </summary>
        public string DryerProcess { get; set; }
        /// <summary>
        /// GloveCode
        /// </summary>
        public string GloveCode { get; set; }

    }

    public class QCTypeDTO
    {
        /// <summary>
        /// Id for Description
        /// </summary>
        public int DescId { get; set; }
        /// <summary>
        /// Id for QC Type
        /// </summary>
        public int QcTypeId { get; set; }
        /// <summary>
        /// QCType
        /// </summary>
        public string QCType { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// NumOfTester
        /// </summary>
        public int NumOfTester { get; set; }
        /// <summary>
        /// GloveCode
        /// </summary>
        public string GloveCode { get; set; }
        /// <summary>
        /// PiecesHR
        /// </summary>
        public int PiecesHR { get; set; }
    }

    public class SizeDTO
    {
        public string Size { get; set; }
    }
}
