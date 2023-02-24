using System;
using System.Collections.Generic;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// This class can be used to pass batch information from one layer to other
    /// </summary>
    public class BatchDTO
    {
        public BatchDTO()
        {
            BatchOrders = new List<DropdownDTO>();
        }
        /// <summary>
        /// Serial Number generated for the batch
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Batch Number generated for the batch
        /// </summary>
        public string BatchNumber { get; set; }
        /// <summary>
        /// Batch Order generated for the batch
        /// </summary>
        public string BatchOrder { get; set; }
        /// <summary>
        /// cater multiple Batch Orders generated for the batch(Glove & FG)
        /// </summary>
        public List<DropdownDTO> BatchOrders { get; set; }
        /// <summary>
        /// Shift when batch is produced
        /// </summary>
        public int Shift { get; set; }
        /// <summary>
        /// Line where batch is produced
        /// </summary>
        public string Line { get; set; }
        /// <summary>
        /// GloveType produced
        /// </summary>
        public string GloveType { get; set; }
        /// <summary>
        /// Size of the Glove Type Produced
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// Side of the Tier for Hourly batch
        /// </summary>
        public string TierSide { get; set; }
        /// <summary>
        /// Batch Weight from Platform Scale
        /// </summary>
        public decimal BatchWeight { get; set; }
        /// <summary>
        /// Calculated RAFWTSample
        /// </summary>
        public decimal CalculatedRAFWTSample { get; set; }
        /// <summary>
        /// Ten Pcs Weight from Platform Scale
        /// </summary>
        public decimal TenPcsWeight { get; set; }
        /// <summary>
        /// Area where batch card is lost
        /// </summary>
        public string BatchLostArea { get; set; }
        /// <summary>
        /// Date when batch is produced
        /// </summary>
        public DateTime BatchCarddate { get; set; }
        /// <summary>
        /// QC Type assigned in QAI
        /// </summary>
        public string QCType { get; set; }
        /// <summary>
        /// QAI Performed on
        /// </summary>
        public DateTime? QAIDate { get; set; }
        /// <summary>
        /// Reason to bypass online batch
        /// </summary>
        public string ByPassReason { get; set; }
        /// <summary>
        /// for polymer test
        /// </summary>
        public string ReferenceNumber { get; set; }
        /// <summary>
        /// Is the batch is scanned again
        /// </summary>
        public int ReworkCount { get; set; }
        /// <summary>
        /// Is batchcard is re printed
        /// </summary>
        public bool IsReprint { get; set; }
        /// <summary>
        /// If Hourly or Tumbling
        /// </summary>
        public bool IsOnline { get; set; }
        /// <summary>
        /// Calculate by Batch Weight and Ten Pcs
        /// </summary>
        public int TotalPcs { get; set; }
        public int PackedPcs { get; set; }
        public int UpdatedPcs { get; set; }

        /// <summary>
        /// Pcs that already packed. #AzmanCBCI
        /// </summary>
        public int PackPcs { get; set; }

        /// <summary>
        /// System where batch is scanned
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// Screen where batch is scanned
        /// </summary>
        public string SubModule { get; set; }
        /// <summary>
        /// Location of workstation
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Batch Type T,L,QWT...
        /// </summary>
        public string BatchType { get; set; }
        /// <summary>
        /// Ten Pcs out of range validated by
        /// </summary>
        //public int AuthorizedBy { get; set; }
        public string AuthorizedBy { get; set; }
        /// <summary>
        /// Last modified on date
        /// </summary>
        public DateTime LastModifiedOn { get; set; }
        /// <summary>
        /// Current machine where the application is installed
        /// </summary>
        public string WorkstationNumber { get; set; }
        /// <summary>
        /// Used in Final Packing
        /// </summary>
        public bool IsFPBatchSplit { get; set; }
        /// <summary>
        /// Operator who uses the screen
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// BB oR NBC
        /// </summary>
        public string Site { get; set; }
        /// <summary>
        /// Location ID based on workstation
        /// </summary>
        public int LocationId { get; set; }
        /// <summary>
        /// Shift name of the bacth
        /// </summary>
        public string ShiftName { get; set; }
        /// <summary>
        /// PC Number from Database
        /// </summary>
        public string WorkStationId { get; set; }
        /// <summary>
        /// Description of glove type
        /// </summary>
        public string GloveTypeDescription { get; set; }
        /// <summary>
        /// Only Date
        /// </summary>
        public string ShortDate { get; set; }
        /// <summary>
        /// Only Time
        /// </summary>
        public string ShortTime { get; set; }
        /// <summary>
        /// Hourly Batch TierSide full name for printing
        /// </summary>
        public string Side { get; set; }
        /// <summary>
        /// Description of QC Type
        /// </summary>
        public string QCTypeDescription { get; set; }
        /// <summary>
        /// ReasonId
        /// </summary>
        public int RejectReasonId { get; set; }
        /// <summary>
        /// PT Batch Weight from Platform Scale
        /// </summary>
        public decimal PTBatchWeight { get; set; }
        /// <summary>
        /// PT Ten Pcs Weight from Platform Scale
        /// </summary>
        public decimal PTTenPcsWeight { get; set; }
        /// <summary>
        /// Rework Count for QC Scan Batch Card Weight
        /// </summary>
        public int QCReworkCount { get; set; }
        /// <summary>
        /// QC Group Id of the batch
        /// </summary>
        public int QCGroupId { get; set; }
        /// <summary>
        /// QC Group Name of the batch
        /// </summary>
        public string QCGroupName { get; set; }
          /// <summary>
        /// QC Group Members of the batch
        /// </summary>
        public string QCGroupMembers { get; set; }
        /// <summary>
        /// Start Time of the batch
        /// </summary>
        public string BatchStartTime { get; set; }

        /// <summary>
        /// Area
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// InnerBoxCount
        /// </summary>
        public int InnerBoxCount { get; set; }
        /// <summary>
        /// PackingSize
        /// </summary>
        public int PackingSize { get; set; }
        /// <summary>
        /// ScanIn
        /// </summary>
        public DateTime ScanInDate { get; set; }
        /// <summary>
        /// ScanOut
        /// </summary>
        public DateTime ScanOutDate { get; set; }

        /// <summary>
        /// HotBox Test Slip Validity for the Batch
        /// </summary>
        public int HotBox { get; set; }

        /// <summary>
        /// Protein Test Slip Validity for the Batch
        /// </summary>
        public int Protein { get; set; }
        /// <summary>
        /// Powder Test Slip Validity for the Batch
        /// </summary>
        public int Powder { get; set; }
        /// <summary>
        /// Next Process
        /// </summary>
        public string NextProcess { get; set; }
        /// <summary>
        /// SecondGradeType
        /// </summary>
        public string SecondGradeType { get; set; }
        /// <summary>
        /// SecondGrade PCs
        /// </summary>
        public int SecondGradePCs { get; set; }
        /// <summary>
        /// Authorized for TenPcs or Batch or Both when out of range
        /// </summary>
        public int AuthorizedFor { get; set; }
        /// <summary>
        /// Online Rejection Resource Group(plant id + line id) 
        /// added on 10th Oct 2016 at 5:25PM by Max He, MH#1.n
        /// </summary>
        public string ResourceGroup { get; set; }
        /// <summary>
        /// Online Rejection Warehouse(plant id + "-CPD") 
        /// added on 11th Oct 2016 at 5:13PM by Max He, MH#1.n
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// Visual Test Sampling Size
        /// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
        /// </summary>
        public decimal RAFVTSample;

        /// <summary>
        /// Water Tight Sampling Size
        /// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
        /// </summary>
        public decimal RAFWTSample;

        /// <summary>
        /// Cache the QAI record id
        /// To cater re-sampling get the correct record 
        /// #MH 03/11/2016 1.n FDD@NGC-REM-001-PT_V1.0
        /// </summary>
        public decimal QaiId;

        /// <summary>
        /// Good Qty but loose form
        /// #MH 10/11/2016 1.n FDD:HLTG-REM-003
        /// </summary>
        public decimal RAFGoodQtyLoose;

        /// <summary>
        /// 2nd Grade Qty
        /// #MH 10/11/2016 1.n FDD:HLTG-REM-003
        /// </summary>
        public decimal SecGradeQty;

        /// <summary>
        /// Rejected Qty
        /// #MH 10/11/2016 1.n FDD:HLTG-REM-003
        /// </summary>
        public decimal RejectedQty;

        /// <summary>
        /// Rejected Sample
        /// #MH 10/11/2016 1.n FDD:HLTG-REM-003
        /// </summary>
        public decimal RejectedSampleQty;

        /// <summary>
        /// Calculated Loose Qty
        /// #MH 10/11/2016 1.n FDD:HLTG-REM-003
        /// </summary>
        public decimal CalculatedLooseQty;

        /// <summary>
        /// Detect is ReSampling
        /// #MH 22/12/2016 2.n FDD@NGC-REM-001-PT_V1.0
        /// </summary>
        public bool IsReSampling { get; set; }

        /// <summary>
        /// TenPcs Sampling Size
        /// #Azman 21/02/2018
        /// #Azrul 13/07/2018 Merged from Live AX6
        /// </summary>
        public decimal TenPcsRAFSample;

        /// <summary>
        /// Resource
        /// </summary>
        public string Resource { get; internal set; }

        /// <summary>
        /// SeqNo
        /// </summary>
        public int SeqNo { get; internal set; }

        /// <summary>
        /// Quantity
        /// #MK 28/05/2018 CR_118
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// DeliveryDate
        /// #MK 28/05/2018 CR_118
        /// </summary>
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// Pool
        /// #MK 28/05/2018 CR_118
        /// </summary>
        public string Pool { get; set; }

        /// <summary>
        /// RouteCategory
        /// #MK 28/05/2018 CR_118
        /// </summary>
        public string RouteCategory { get; set; }

        /// <summary>
        /// BatchSequence
        /// #Azrul 21/09/2021 Open batch flag for NGC1.5
        /// </summary>
        public int BatchSequenceNo { get; set; }

        /// <summary>
        /// isConsolidated
        /// #Azrul 21/09/2021 Open batch flag for NGC1.5
        /// </summary>
        public bool IsConsolidated { get; set; }
        public int? ShiftId { get; set; }
    }
}
