using System;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for QC yield and Packing
    /// </summary>
    public class QCYieldandPackingDTO
    {
        /// <summary>
        /// QC yield and packing ID
        /// </summary>
        public int QCYieldAndPackinId { get; set; }
        /// <summary>
        /// Serial Number generated for the batch
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Glove Type of the batch
        /// </summary>
        public string GloveType { get; set; }
        /// <summary>
        /// QC group ID
        /// </summary>
        public int QCGroupId { get; set; }
        /// <summary>
        /// QC group members
        /// </summary>
        public string QCGroupMembers { get; set; }
        /// <summary>
        /// Batch End Time
        /// </summary>
        public string BatchEndTime { get; set; }
        /// <summary>
        /// Rework Reason Id
        /// </summary>
        public string ReworkReasonId { get; set; }
        /// <summary>
        /// Inner Box Count
        /// </summary>
        public Int64 InnerBoxCount { get; set; }
        /// <summary>
        /// Batch Status
        /// </summary>
        public string BatchStatus { get; set; }
        /// <summary>
        /// Packing Size
        /// </summary>
        public int PackingSize { get; set; }
        /// <summary>
        /// Pack Into
        /// </summary>
        public string PackInto { get; set; }
        /// <summary>
        /// Module Name
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// Sub Module Name
        /// </summary>
        public string SubModuleName { get; set; }
        /// <summary>
        /// Last modified on for the downgrade batch card
        /// </summary>
        public DateTime LastModifiedOn { get; set; }
        /// <summary>
        /// Workstation Id
        /// </summary>
        public string WorkstationId { get; set; }
        /// <summary>
        /// Batch Weight In KG
        /// </summary>
        public string BatchWeight { get; set; }

        /// <summary>
        /// Batch Weight In Grm
        /// </summary>
        public string BatchWeightGrm { get; set; }

        /// <summary>
        /// Ten Pieces Weight of the batch
        /// </summary>
        public string TenPcsWeight { get; set; }
        /// <summary>
        /// Group Members Count
        /// </summary>
        public int GroupMemberCount { get; set; }
        /// <summary>
        /// Batch Start Time
        /// </summary>
        public string BatchStartTime { get; set; }
        /// <summary>
        /// Batch Target Time
        /// </summary>
        public string BatchTargetTime { get; set; }
        /// <summary>
        /// Rework Count
        /// </summary>
        //public string ReworkCount { get; set; }
        /// <summary>
        /// Rework Count
        /// </summary>
        public string ReworkCount { get; set; }
        /// <summary>
        /// Defect Type Id of the Batch
        /// </summary>
        public string DefectTypeId { get; set; }
        /// <summary>
        /// Defect Type Id of the Batch
        /// </summary>
        public decimal PiecesCount { get; set; } 
        /// <summary>
        /// Defect Type Id of the Batch
        /// </summary>
        public bool IsPlatform { get; set; }
        /// <summary>
        /// Current Shift Id of the Batch
        /// </summary>
        public int ShiftId { get; set; }
        /// <summary>
        /// QC Type of the Batch
        /// </summary>
        public string QCType { get; set; }
        /// <summary>
        /// Brand
        /// </summary>
        public string Brand { get; set; }

        // #MH 10/11/2016 1.n FDD:HLTG-REM-003
        public int LooseQty { get; set; }
        public int RejectionQty { get; set; }
        public int RejectedSample { get; set; }
        public int SecondGradeQty { get; set; }
        public int CalculatedLooseQty { get; set; }
        // #MH 10/11/2016 1.n

        /// <summary>
        /// BatchPcs
        /// </summary>
        public int BatchPcs { get; set; }
        public int QCTargetTypeID { get; set; }
        public int TotalPcs { get; set; }
    }
}
