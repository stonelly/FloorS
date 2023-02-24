using Hartalega.FloorSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// QAIAQCosmeticReferenceSecondDTO
    /// </summary>
    public class QAIAQCosmeticReferenceSecondDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// AQLName
        /// </summary>
        public string AQLName { get; set; }
        /// <summary>
        /// AQLID
        /// </summary>
        public string AQLID { get; set; }
        /// <summary>
        /// QCType
        /// </summary>
        public string QCType { get; set; }
        /// <summary>
        /// MajorDefectMinVal
        /// </summary>
        public string MajorDefectMinVal { get; set; }
        /// <summary>
        ///MajorDefectMaxVal
        /// </summary>
        public string MajorDefectMaxVal { get; set; }
        /// <summary>
        /// MinorDefectMinVal
        /// </summary>
        public string MinorDefectMinVal { get; set; }
        /// <summary>
        /// MinorDefectMinVal
        /// </summary>
        public string MinorDefectMaxVal { get; set; }
        /// <summary>
        /// QCTypeOrder
        /// </summary>
        public string QCTypeOrder { get; set; }
        /// <summary>
        /// WorkStationId
        /// </summary>
        public string WorkStationId { get; set; }
        /// <summary>
        /// OperatorId
        /// </summary>
        public string OperatorId { get; set; }

        //added MYAdamas 20170928 for audit log purpose
        public Constants.ActionLog ActionType { get; set; }
        public bool IsDeleted { get; set; }

        public string CustomerTypeId { get; set; }
        public string CustomerType { get; set; }

        public string DefectCategoryGroupId { get; set; }
        public string DefectCategoryGroup { get; set; }
        public string EnumModuleId { get; set; }
        public string Module { get; set; }
    }
}
