using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// QAI Defect Master DTO
    /// </summary>
    public class QAIDefectDetails
    {
        /// <summary>
        /// DefectID
        /// </summary>
        public int DefectID { get; set; }
        /// <summary>
        /// DefectItem
        /// </summary>
        public string DefectItem { get; set; }
        /// <summary>
        /// DefectCode
        /// </summary>
        public string DefectCode{ get; set; }
        /// <summary>
        /// KeyStroke
        /// </summary>
        public string KeyStroke { get; set; }
        /// <summary>
        /// DefectCategory
        /// </summary>
        public string DefectCategory { get; set; }
        /// <summary>
        /// DefectCategoryId
        /// </summary>
        public int DefectCategoryId { get; set; }
        /// <summary>
        /// IsAND
        /// </summary>
        public bool IsAnd { get; set; }
        /// <summary>
        /// Workstation Id
        /// </summary>
        public string WorkstationId { get; set; }
        /// <summary>
        /// Id for Operator 
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// QC Type
        /// </summary>
        public string QCType { get; set; }

        public bool IsDeleted { get; set; }

        public Constants.ActionLog ActionType { get; set; }
        public string DefectCategoryGroup { get; set; }
        public int DefectCategoryGroupId { get; set; }
    }

    /// <summary>
    /// QAI Defect Category
    /// </summary>

    
    public class QAIDefectCategory
    {
        /// <summary>
        /// Defect Category
        /// </summary>
        public string DefectCategory { get; set; }

        /// <summary>
        /// Sequence
        /// </summary>
        public int Sequence { get; set; }      

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Workstation Id
        /// </summary>
        public string WorkstationId { get; set; }
        /// <summary>
        /// Id for Operator 
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// Defect category Type Id
        /// </summary>
        public int DefectCategoryTypeId { get; set; }
        /// <summary>
        /// Defect Category Type
        /// </summary>
        public string DefectCategoryType { get; set; }

        public bool IsDeleted { get; set; }

        public Constants.ActionLog ActionType { get; set; }

    }

    /// <summary>
    /// QAI Defect Master DTO
    /// </summary>
    public class QAIDefectPositions
    {
        /// <summary>
        /// DefectPositionId
        /// </summary>
        public int DefectPositionId { get; set; }
        /// <summary>
        /// DefectPositionItem
        /// </summary>
        public string DefectPositionItem { get; set; }
        /// <summary>
        /// KeyStroke
        /// </summary>
        public string KeyStroke { get; set; }
        /// <summary>
        /// Defect
        /// </summary>
        public string DefectItem{ get; set; }
        /// <summary>
        /// DefectId
        /// </summary>
        public int DefectId { get; set; }

        public bool IsDeleted { get; set; }

        public Constants.ActionLog ActionType { get; set; }
    }
}
