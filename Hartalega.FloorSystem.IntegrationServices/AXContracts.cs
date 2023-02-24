using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Hartalega.FloorSystem.IntegrationServices
{
    #region AX Contracts

    /// <summary>
    /// Ava Interface Contract
    /// </summary>
    public class AvaInterfaceContract
    {
        /*
        public AvaInterfaceContract(string functionID)
        {
            FunctionID = functionID;
        }*/

        //Parent class properties
        [Display(Name = "Function Identifier")]
        [Required]
        [StringLength(20)]
        public string FunctionID;

        [Display(Name = "Running Sequence Number")]
        [Required]
        public int BatchSequence;
        public string PlantNo;
        public int ReferenceBatchSequence1;
        public int ReferenceBatchSequence2;
        public int ReferenceBatchSequence3;
        public int ReferenceBatchSequence4;
        public int ReferenceBatchSequence5;

        [Display(Name = "Serial Number")]
        [Required]
        [StringLength(20)]
        public string BatchNumber;
        public Guid FSIdentifier;
        public string ReferenceBatchNumber1;
        public string ReferenceBatchNumber2;
        public string ReferenceBatchNumber3;
        public string ReferenceBatchNumber4;
        public string ReferenceBatchNumber5;
        public int ProcessingStatus = 0; // Default is not started
        public bool IsConsolidated = false;

        [Display(Name = "Batch Card Number")]
        [Required]
        [StringLength(50)]
        public string BatchCardNumber;

        //Child class objects
        public BOMJournalContract BOMJournal;
        public AVATransferJournalContract TransferJournal;
        public AVAMovementJournalContract MovementJournal;
        public AVAFGJournalContract FGJournal;
        public AVARAFStgJournalContract RAFStgJournal;
        public DOT_ReworkOrderBatchContract ReworkStg;
        public DOT_PickingListContract PickingList;
    }

    public class BOMJournalContract
    {
        [Display(Name = "Station (PT)-Glove code (NB-AB-PF-VBLU)")]
        [Required]
        [StringLength(30)]
        public string ItemNumber;
        [Display(Name = "Size")]
        [Required]
        [StringLength(10)]
        public string Configuration;
        [Display(Name = "Plant(P1)-Station(PT)")]
        [Required]
        [StringLength(10)]
        public string Warehouse;
        [Display(Name = "10 Pieces Weight")]
        [Required]
        public decimal Weightof10Pcs;
        [Display(Name = "Batch Weight")]
        [Required]
        public decimal BatchWeight;
        [Display(Name = "QC Type")]
        [Required]
        [StringLength(30)]
        public string QCType;
        [Display(Name = "Posting date and time")]
        [Required]
        public DateTime PostingDateTime;
        [Display(Name = "Created date and time")]
        [Required]
        public DateTime CreatedDateTime;
        [Display(Name = "Number of Pieces")]
        [Required]
        public decimal Quantity;
        [Display(Name = "Shift")]
        [Required]
        [StringLength(5)]
        public string Shift;
        [Display(Name = "Area")]
        [Required]
        [StringLength(30)]
        public string Area;
    }

    public class AVATransferJournalContract
    {
        [Display(Name = "Station (PN)-Glove code (NB-AB-PF-VBLU)")]
        [Required]
        [StringLength(30)]
        public string ItemNumber;
        [Display(Name = "Size")]
        [Required]
        [StringLength(10)]
        public string Configuration;
        [Display(Name = "Location")]
        [Required]
        [StringLength(10)]
        public string Location;
        [Display(Name = "Number of Pieces")]
        [Required]
        public decimal Quantity;
        [Display(Name = "Scan In date and time")]
        [Required]
        public DateTime ScanInDateTime;
        [Display(Name = "Scan Out date and time")]
        [Required]
        public DateTime ScanOutDateTime;
        [Display(Name = "Plant(P1)- Station(PT)")]
        [Required]
        [StringLength(10)]
        public string ToWarehouse;
        [Display(Name = "Plant(P1)-Station(PN)")]
        [Required]
        [StringLength(10)]
        public string FromWarehouse;
        [Display(Name = "Plant(P1)-Station(PN)")]
        [Required]
        [StringLength(10)]
        public string Warehouse;
        public string Brand { get; set; }
        public string Formula { get; set; }
        public string TransferJournalId { get; set; }
    }

    public class AVAMovementJournalContract
    {
        [Display(Name = "Station (PN)-Glove code (NB-AB-PF-VBLU)")]
        [Required]
        [StringLength(40)]
        public string ItemNumber;
        [Display(Name = "Reference Item No")]
        [Required]
        [StringLength(40)]
        public string ReferenceItemNo;
        [Display(Name = "Size")]
        [Required]
        [StringLength(10)]
        public string Configuration;
        [Display(Name = "Reference Size")]
        [Required]
        [StringLength(10)]
        public string RefConfiguration;
        [Display(Name = "Plant(P1)-Station(PT/QC)")]
        [Required]
        [StringLength(10)]
        public string Warehouse;
        [Display(Name = "10 Pieces Weight")]
        [Required]
        public decimal Weightof10Pcs;
        [Display(Name = "Batch Weight")]
        [Required]
        public decimal BatchWeight;
        [Display(Name = "QC Type")]
        [Required]
        [StringLength(15)]
        public string QCType;
        [Display(Name = "Posting date and time")]
        [Required]
        public DateTime PostingDateTime;
        [Display(Name = "Created date and time")]
        [Required]
        public DateTime CreatedDateTime;
        [Display(Name = "Number of Pieces")]
        [Required]
        public decimal Quantity;
        [Display(Name = "Shift")]
        [Required]
        [StringLength(5)]
        public string Shift;
        [Display(Name = "Area")]
        [Required]
        [StringLength(30)]
        public string Area;

        [Display(Name = "Serial Number (New)")]
        [Required]
        [StringLength(30)]
        public string ToBatchNumber;

        [Display(Name = "Serial Number (Old)")]
        [Required]
        [StringLength(30)]
        public string FromBatchNumber;
        
        [Display(Name = "Ref. Batch sequence 1")]
        [Required]
        public int RefBatchSequence1;

        [Display(Name = "Ref. Batch sequence 2")]
        [Required]
        public int RefBatchSequence2;

        // #AzmanCBCI - Adding no3 batch sequence
        [Display(Name = "Ref. Batch sequence 3")]
        [Required]
        public int RefBatchSequence3;

        [Display(Name = "Sales order (Old Serial number)")]        
        public string SalesOrderNumber;

        // added on 10th Oct 2016 at 5:25 by Max He, MH#1.n
        // for online rejection integration, add on resource group( plant id + line id) 
        [Display(Name = "Resource Group (AX)")]
        public string ResourceGroup;

        public string Location { get; set; }
    }

    // MK 28/05/2018
    // Add new contract class for Rework Batch Order
    public class DOT_ReworkOrderBatchContract
    {
        [Required]
        [StringLength(50)]
        public string ItemNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Configuration { get; set; }

        [Required]
        [StringLength(50)]
        public string Warehouse { get; set; }

        [Required]
        [StringLength(50)]
        public string RouteCategory { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Pool { get; set; }

        [Required]
        public DateTime PostingDateandTime { get; set; }
        public string Resource { get; set; }
        public string Location { get; set; }
        public string OriRWKNum { get; set; }
    }

    // #AZ 11/06/2018 Add new contract class for Picking List
    public class DOT_PickingListContract
    {
        [StringLength(50)]
        public string Configuration { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [StringLength(50)]
        public string InternalReferenceNumber { get; set; }

        [StringLength(50)]
        public string PSIReworkOrderNo { get; set; }

        public DateTime PostingDateTime { get; set; }

        public decimal QCType { get; set; }

        public decimal RAFGoodQty { get; set; }

        [StringLength(50)]
        public string Warehouse { get; set; }

        [StringLength(50)]
        public string SalesOrderNumber { get; set; }

        public decimal TenPcsWt { get; set; }

        public int RefBatchSequence1 { get; set; }

        public int RefBatchSequence2 { get; set; }

        public int RefBatchSequence3 { get; set; }

        public string Location { get; set; }

        public string ItemNumber { get; set; }

        public string ReferenceItemNumber { get; set; }

        public int RefNumberOfPieces1 { get; set; }

        public int RefNumberOfPieces2 { get; set; }

        public int RefNumberOfPieces3 { get; set; }
    }

    public class AVAFGJournalContract
    {
        [Display(Name = "Station (PN)-Glove code (NB-AB-PF-VBLU)")]
        [Required]
        [StringLength(40)]
        public string ItemNumber;

        [Display(Name = "Size")]
        [Required]
        [StringLength(10)]
        public string Configuration;

        [Display(Name = "Plant(P1)-Station(PS)")]
        [Required]
        [StringLength(10)]
        public string Warehouse;

        [Display(Name = "Plant (P1)-Packing station(PACK1)")]
        [Required]
        [StringLength(10)]
        public string Resource;

        [Display(Name = "Customer PO")]
        [Required]
        [StringLength(20)]
        public string CustomerPO;

        [Display(Name = "Customer Reference")]
        [Required]
        [StringLength(30)]
        public string CustomerReference;

        [Display(Name = "Sales order Number")]
        [Required]
        [StringLength(20)]
        public string SalesOrderNumber;

        [Display(Name = "Inner Lot number")]
        [Required]
        [StringLength(45)]
        public string InnerLotNumber;

        [Display(Name = "Outer lot Number")]
        [Required]
        [StringLength(45)]
        public string OuterLotNumber;

        [Display(Name = "Customer lot Number")]
        [Required]
        [StringLength(45)]
        public string CustomerLotNumber;

        [Display(Name = "Manufacturing date")]
        [Required]
        public DateTime ManufacturingDate;

        [Display(Name = "Expiry date")]
        [Required]
        public DateTime ExpiryDate;

        [Display(Name = "Preshipment (Flag)")]
        [Required]
        public NoYes Preshipment;

        [Display(Name = "Preshipment cases")]
        [Required]
        public int PreshipmentCases;

        [Display(Name = "Ref. Size 1")]
        [Required]
        [StringLength(10)]
        public string RefConfiguration1;

        [Display(Name = "Ref. Size 2")]
        [StringLength(10)]
        public string RefConfiguration2;

        [Display(Name = "Ref. Size 3")]
        [StringLength(10)]
        public string RefConfiguration3;

        [Display(Name = "Ref. Size 4")]
        [StringLength(10)]
        public string RefConfiguration4;

        [Display(Name = "Ref. Size 5")]
        [StringLength(10)]
        public string RefConfiguration5;

        [Display(Name = "FG Item Number")]
        [Required]
        [StringLength(40)]
        public string FGItemNumber;

        [Display(Name = "Ref. Number of Pieces 1")]
        [Required]
        public decimal RefNumberOfPieces1;

        [Display(Name = "Ref. Number of Pieces 2")]
        public decimal RefNumberOfPieces2;

        [Display(Name = "Ref. Number of Pieces 3")]
        public decimal RefNumberOfPieces3;

        [Display(Name = "Ref. Number of Pieces 4")]
        public decimal RefNumberOfPieces4;

        [Display(Name = "Ref. Number of Pieces 5")]
        public decimal RefNumberOfPieces5;

        [Display(Name = "Ref. Glove Code 1")]
        [StringLength(40)]
        public string RefItemNumber1;

        [Display(Name = "Ref. Glove Code 2")]
        [StringLength(40)]
        public string RefItemNumber2;

        [Display(Name = "Ref. Glove Code 3")]
        [StringLength(40)]
        public string RefItemNumber3;

        [Display(Name = "Ref. Glove Code 4")]
        [StringLength(40)]
        public string RefItemNumber4;

        [Display(Name = "Ref. Glove Code 5")]
        [StringLength(40)]
        public string RefItemNumber5;

        [Display(Name = "Created date and time")]
        [Required]
        public DateTime CreatedDateTime;

        [Display(Name = "Posting date and time")]
        [Required]
        public DateTime PostingDateTime;

        [Display(Name = "Number of Pieces (Case)")]
        [Required]
        public decimal Quantity;

        public string BatchOrder { get; set; }
        public string Location { get; set; }
        public string PalletId { get; set; }

        //[Display(Name = "Ref. Batch Sequence 1")]
        //[Required]
        //public int RefBatchSequence1;

        //[Display(Name = "Ref. Batch Sequence 2")]
        //public int RefBatchSequence2;

        //[Display(Name = "Ref. Batch Sequence 3")]
        //public int RefBatchSequence3;

        //[Display(Name = "Ref. Batch Sequence 4")]
        //public int RefBatchSequence4;

        //[Display(Name = "Ref. Batch Sequence 5")]
        //public int RefBatchSequence5;

        //Flag to detect is from Make to Stock
        public bool IsWTS { get; set; }
    }

    public class AVARAFStgJournalContract
    {
        public AVARAFStgJournalContract()
        {
            this.RAFVTSample = 0m;
            this.RAFWTSample = 0m;
            this.Quantity = 0m;
        }

        [Display(Name = "Station (PN)-Glove code (NB-AB-PF-VBLU)")]
        [Required]
        [StringLength(40)]
        public string ItemNumber;

        [Display(Name = "Size")]
        [Required]
        [StringLength(10)]
        public string Configuration;

        [Display(Name = "Plant(P1)-Station(PN)")]
        [Required]
        [StringLength(10)]
        public string Warehouse;

        [Display(Name = "Plant(P1)-Production Line (L01)-Size (M)")]
        [Required]
        [StringLength(10)]
        public string Resource;

        [Display(Name = "Batch Weight")]
        [Required]
        public decimal BatchWeight;

        [Display(Name = "10 Pieces Weight")]
        [Required]
        public decimal Weightof10Pcs;

        [Display(Name = "QC Type")]
        [Required]
        [StringLength(15)]
        public string QCType;

        [Display(Name = "Created date and time")]
        [Required]
        public DateTime CreatedDateTime;

        [Display(Name = "Posting date and time")]
        [Required]
        public DateTime PostingDateTime;

        [Display(Name = "Number of Pieces")]
        [Required]
        public decimal RAFGoodQty;

        [Display(Name = "Shift")]
        [Required]
        [StringLength(5)]
        public string Shift;

        [Display(Name = "Visual Test Sampling Size")]
        public decimal RAFVTSample; // #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0

        [Display(Name = "Water Tight Sampling Size")]
        public decimal RAFWTSample; // #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0

        [Display(Name = "Hot Box Sampling Size")]
        public decimal RAFHBSample; // #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0

        
        [Display(Name = "Good Qty but loose form")]
        public decimal RAFGoodQtyLoose; // #MH 10/11/2016 1.n FDD:HLTG-REM-003

        [Display(Name = "2nd Grade Qty")]
        public decimal SecGradeQty; // #MH 10/11/2016 1.n FDD:HLTG-REM-003

        [Display(Name = "Rejected Qty")]
        public decimal RejectedQty; // #MH 10/11/2016 1.n FDD:HLTG-REM-003

        [Display(Name = "Rejected Sample")]
        public decimal RejectedSampleQty; // #MH 10/11/2016 1.n FDD:HLTG-REM-003

        [Display(Name = "Calculated Loose Qty")]
        public decimal CalculatedLooseQty; // #MH 08/12/2016 1.n FDD:HLTG-REM-003

        public string Location { get; set; }
        public string BatchOrder { get; set; }
        public int SeqNo { get; set; }
        public string OperationNo { get; set; }
        public decimal Quantity { get; set; }

        [StringLength(40)]
        public string ChangedItemNumber; // #Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product)
    }
    #endregion
}
