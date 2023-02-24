using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.IntegrationServices
{
    /*
    public static class AXContracts
    {
        /// <summary>
        /// Get AvaInterfaceContract
        /// </summary>
        /// <param name="_interfaceContract"></param>
        /// <returns></returns>
        public static AXServices.AvaInterfaceContract GetAvaInterfaceContract(this AvaInterfaceContract _interfaceContract)
        {
            AXServices.AvaInterfaceContract interfaceContract = new AXServices.AvaInterfaceContract();
            if (_interfaceContract != null)
            {

                interfaceContract.FunctionID = _interfaceContract.FunctionID;
                interfaceContract.FSIdentifier = _interfaceContract.FSIdentifier; 
                interfaceContract.BatchSequence = _interfaceContract.BatchSequence;
                interfaceContract.ReferenceBatchSequence1 = _interfaceContract.ReferenceBatchSequence1;
                interfaceContract.ReferenceBatchSequence2 = _interfaceContract.ReferenceBatchSequence2;
                interfaceContract.ReferenceBatchSequence3 = _interfaceContract.ReferenceBatchSequence3;
                interfaceContract.ReferenceBatchSequence4 = _interfaceContract.ReferenceBatchSequence4;
                interfaceContract.ReferenceBatchSequence5 = _interfaceContract.ReferenceBatchSequence5;
                interfaceContract.BatchNumber = _interfaceContract.BatchNumber;
                interfaceContract.ReferenceBatchNumber1 = _interfaceContract.ReferenceBatchNumber1;
                interfaceContract.ReferenceBatchNumber2 = _interfaceContract.ReferenceBatchNumber2;
                interfaceContract.ReferenceBatchNumber3 = _interfaceContract.ReferenceBatchNumber3;
                interfaceContract.ReferenceBatchNumber4 = _interfaceContract.ReferenceBatchNumber4;
                interfaceContract.ReferenceBatchNumber5 = _interfaceContract.ReferenceBatchNumber5;
                interfaceContract.BatchCardNumber = _interfaceContract.BatchCardNumber;
            }
            return interfaceContract;
        }

        /// <summary>
        /// Get BOMJournalContract  
        /// </summary>
        /// <param name="bomJournalContract"></param>
        /// <returns></returns>
        public static AXServices.AVABOMJournalContract GetBOMJournalContract(this BOMJournalContract bomJournalContract)
        {
            AXServices.AVABOMJournalContract avabomjournalContract = new AXServices.AVABOMJournalContract();
            if (bomJournalContract != null)
            {
                avabomjournalContract.ItemNumber = bomJournalContract.ItemNumber;
                avabomjournalContract.Configuration = bomJournalContract.Configuration;
                avabomjournalContract.Warehouse = bomJournalContract.Warehouse;
                avabomjournalContract.Weightof10Pcs = bomJournalContract.Weightof10Pcs;
                avabomjournalContract.BatchWeight = bomJournalContract.BatchWeight;
                avabomjournalContract.QCType = bomJournalContract.QCType;
                avabomjournalContract.PostingDateTime = bomJournalContract.PostingDateTime;
                avabomjournalContract.CreatedDateTime = bomJournalContract.CreatedDateTime;
                avabomjournalContract.Quantity = bomJournalContract.Quantity;
                avabomjournalContract.Shift = bomJournalContract.Shift;
                avabomjournalContract.Area = bomJournalContract.Area;
            }
            return avabomjournalContract;
        }

        /// <summary>
        /// Get AVATransferJournalContract
        /// </summary>
        /// <param name="avatransferJournalContract"></param>
        /// <returns></returns>
        public static AXServices.AVATransferJournalContract GetAVATransferJournalContract(this AVATransferJournalContract avatransferJournalContract)
        {
            AXServices.AVATransferJournalContract _AVATransferJournalContract = new AXServices.AVATransferJournalContract();
            if (avatransferJournalContract != null)
            {
                _AVATransferJournalContract.ItemNumber = avatransferJournalContract.ItemNumber;
                _AVATransferJournalContract.Configuration = avatransferJournalContract.Configuration;
                _AVATransferJournalContract.Location = avatransferJournalContract.Location;
                _AVATransferJournalContract.Quantity = avatransferJournalContract.Quantity;
                _AVATransferJournalContract.ScanInDateTime = avatransferJournalContract.ScanInDateTime;
                _AVATransferJournalContract.ScanOutDateTime = avatransferJournalContract.ScanOutDateTime;
                _AVATransferJournalContract.ToWarehouse = avatransferJournalContract.ToWarehouse;
                _AVATransferJournalContract.Warehouse = avatransferJournalContract.Warehouse;
            }
            return _AVATransferJournalContract;
        }

        /// <summary>
        /// Get AVAMovementJournalContract
        /// </summary>
        /// <param name="avamovementJournalContract"></param>
        /// <returns></returns>
        public static AXServices.AVAMovementJournalContract GetAVAMovementJournalContract(this AVAMovementJournalContract avamovementJournalContract)
        {
            AXServices.AVAMovementJournalContract _AVAMovementJournalContract = new AXServices.AVAMovementJournalContract();
            if (avamovementJournalContract != null)
            {
                _AVAMovementJournalContract.ItemNumber = avamovementJournalContract.ItemNumber;
                _AVAMovementJournalContract.ReferenceItemNo = avamovementJournalContract.ReferenceItemNo;
                _AVAMovementJournalContract.Configuration = avamovementJournalContract.Configuration;
                _AVAMovementJournalContract.RefConfiguration = avamovementJournalContract.RefConfiguration;
                _AVAMovementJournalContract.Warehouse = avamovementJournalContract.Warehouse;
                _AVAMovementJournalContract.Weightof10Pcs = avamovementJournalContract.Weightof10Pcs;
                _AVAMovementJournalContract.BatchWeight = avamovementJournalContract.BatchWeight;
                _AVAMovementJournalContract.QCType = avamovementJournalContract.QCType;
                _AVAMovementJournalContract.PostingDateTime = avamovementJournalContract.PostingDateTime;
                _AVAMovementJournalContract.CreatedDateTime = avamovementJournalContract.CreatedDateTime;
                _AVAMovementJournalContract.Quantity = avamovementJournalContract.Quantity;
                _AVAMovementJournalContract.Shift = avamovementJournalContract.Shift;
                _AVAMovementJournalContract.Area = avamovementJournalContract.Area;
                _AVAMovementJournalContract.ToBatchNumber = avamovementJournalContract.ToBatchNumber;
                _AVAMovementJournalContract.SalesOrderNumber = avamovementJournalContract.SalesOrderNumber;
                // added on 11th Oct 2016 at 10:32 by Max He, MH#1.n
                _AVAMovementJournalContract.ResourceGroup = avamovementJournalContract.ResourceGroup;// Online Reject Glove Resource Group
   
            }
            return _AVAMovementJournalContract;
        }

        /// <summary>
        /// Get AVAFGJournalContract
        /// </summary>
        /// <param name="avafgjournalContract"></param>
        /// <returns></returns>
        public static AXServices.AVAFGJournalContract GetAVAFGJournalContract(this AVAFGJournalContract avafgjournalContract)
        {
            AXServices.AVAFGJournalContract _AVAFGJournalContract = new AXServices.AVAFGJournalContract();
            if (avafgjournalContract != null)
            {
                _AVAFGJournalContract.ItemNumber = avafgjournalContract.ItemNumber;
                _AVAFGJournalContract.Configuration = avafgjournalContract.Configuration;
                _AVAFGJournalContract.Warehouse = avafgjournalContract.Warehouse;
                _AVAFGJournalContract.Resource = avafgjournalContract.Resource;
                _AVAFGJournalContract.CustomerPO = avafgjournalContract.CustomerPO;
                _AVAFGJournalContract.CustomerReference = avafgjournalContract.CustomerReference;
                _AVAFGJournalContract.SalesOrderNumber = avafgjournalContract.SalesOrderNumber;
                _AVAFGJournalContract.InnerLotNumber = avafgjournalContract.InnerLotNumber;
                _AVAFGJournalContract.OuterLotNumber = avafgjournalContract.OuterLotNumber;
                _AVAFGJournalContract.CustomerLotNumber = avafgjournalContract.CustomerLotNumber;
                _AVAFGJournalContract.ManufacturingDate = avafgjournalContract.ManufacturingDate;
                _AVAFGJournalContract.ExpiryDate = avafgjournalContract.ExpiryDate;

                if (avafgjournalContract.Preshipment == NoYes.Yes)
                {
                    _AVAFGJournalContract.Preshipment = AXServices.NoYes.Yes;
                }
                if (avafgjournalContract.Preshipment == NoYes.No)
                {
                    _AVAFGJournalContract.Preshipment = AXServices.NoYes.No;
                }

                _AVAFGJournalContract.PreshipmentCases = avafgjournalContract.PreshipmentCases;
                _AVAFGJournalContract.RefConfiguration1 = avafgjournalContract.RefConfiguration1;
                _AVAFGJournalContract.RefConfiguration2 = avafgjournalContract.RefConfiguration2;
                _AVAFGJournalContract.RefConfiguration3 = avafgjournalContract.RefConfiguration3;
                _AVAFGJournalContract.RefConfiguration4 = avafgjournalContract.RefConfiguration4;
                _AVAFGJournalContract.RefConfiguration5 = avafgjournalContract.RefConfiguration5;
                _AVAFGJournalContract.RefItemNumber = avafgjournalContract.RefItemNumber;
                _AVAFGJournalContract.RefNumberOfPieces1 = avafgjournalContract.RefNumberOfPieces1;
                _AVAFGJournalContract.RefNumberOfPieces2 = avafgjournalContract.RefNumberOfPieces2;
                _AVAFGJournalContract.RefNumberOfPieces3 = avafgjournalContract.RefNumberOfPieces3;
                _AVAFGJournalContract.RefNumberOfPieces4 = avafgjournalContract.RefNumberOfPieces4;
                _AVAFGJournalContract.RefNumberOfPieces5 = avafgjournalContract.RefNumberOfPieces5;
                _AVAFGJournalContract.RefBatchNumber4 = avafgjournalContract.RefBatchNumber4;
                _AVAFGJournalContract.CreatedDateTime = avafgjournalContract.CreatedDateTime;
                _AVAFGJournalContract.PostingDateTime = avafgjournalContract.PostingDateTime;
                _AVAFGJournalContract.Quantity = avafgjournalContract.Quantity;

            }
            return _AVAFGJournalContract;
        }

        /// <summary>
        /// Get AVARAFStgJournalContract
        /// </summary>
        /// <param name="avarafstgJournalContract"></param>
        /// <returns></returns>
        public static AXServices.AVARAFStgJournalContract GetAVARAFStgJournalContract(this AVARAFStgJournalContract avarafstgJournalContract)
        {
            AXServices.AVARAFStgJournalContract _AVARAFStgJournalContract = new AXServices.AVARAFStgJournalContract();
            if (avarafstgJournalContract != null)
            {
                _AVARAFStgJournalContract.ItemNumber = avarafstgJournalContract.ItemNumber;
                _AVARAFStgJournalContract.Configuration = avarafstgJournalContract.Configuration;
                _AVARAFStgJournalContract.Warehouse = avarafstgJournalContract.Warehouse;
                _AVARAFStgJournalContract.Resource = avarafstgJournalContract.Resource;
                _AVARAFStgJournalContract.BatchWeight = avarafstgJournalContract.BatchWeight;
                _AVARAFStgJournalContract.Weightof10Pcs = avarafstgJournalContract.Weightof10Pcs;
                _AVARAFStgJournalContract.QCType = avarafstgJournalContract.QCType;
                _AVARAFStgJournalContract.CreatedDateTime = avarafstgJournalContract.CreatedDateTime;
                _AVARAFStgJournalContract.PostingDateTime = avarafstgJournalContract.PostingDateTime;
                _AVARAFStgJournalContract.RAFGoodQty = avarafstgJournalContract.RAFGoodQty;
                _AVARAFStgJournalContract.Shift = avarafstgJournalContract.Shift;
                _AVARAFStgJournalContract.RAFWTSample = avarafstgJournalContract.RAFWTSample;// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
                _AVARAFStgJournalContract.RAFVTSample = avarafstgJournalContract.RAFVTSample;// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
                _AVARAFStgJournalContract.RAFHBSample = avarafstgJournalContract.RAFHBSample;// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0                
                // #MH 05/12/2016 1.n FDD:HLTG-REM-003
                _AVARAFStgJournalContract.RAFGoodQtyLoose = avarafstgJournalContract.RAFGoodQtyLoose;
                _AVARAFStgJournalContract.RejectedQty = avarafstgJournalContract.RejectedQty;
                _AVARAFStgJournalContract.RejectedSampleQty = avarafstgJournalContract.RejectedSampleQty;
                _AVARAFStgJournalContract.SecGradeQty = avarafstgJournalContract.SecGradeQty;
                _AVARAFStgJournalContract.CalcLooseQty = avarafstgJournalContract.CalculatedLooseQty;
                // #MH 05/12/2016 1.n FDD:HLTG-REM-003
             
            }
            return _AVARAFStgJournalContract;
        }

    }
    */



    /// <summary>
    /// Enum for YES/NO
    /// </summary>
    public enum NoYes : int
    {
        [System.Runtime.Serialization.EnumMemberAttribute()]
        No = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Yes = 1,
    }

    /// <summary>
    /// Create RAFJournal Function identifier
    /// </summary>
    public static class CreateRAFJournalFunctionidentifier
    {
        public const string PNBC = "PNBC";
        public const string OBGPC = "OBGPC";
        public const string HBC = "HBC";
        public const string SOBC = "SOBC";
        public const string SBCIB = "SBCIB";
        public const string QCQI_QC = "QCQI(QC)";

        public const string PVTBCS = "PVTBCS";
        public const string PVTBCA = "PVTBCA";
        public const string PWTBCP = "PWTBCP";
        public const string PWTBCA = "PWTBCA";
        public const string PWTBCQ = "PWTBCQ";
        public const string PWTBCS = "PWTBCS";

        public const string SRBC = "SRBC";
        public const string ON2G = "ON2G";
        public const string CGLV = "CGLV";
    }

    /// <summary>
    /// Create FGRAFJournal Function identifier
    /// </summary>
    public static class CreateFGRAFJournalFunctionidentifier
    {
        public const string SBC = "SBC";
        public const string SUBC = "SUBC";
        public const string SMBP = "SMBP";
        public const string SGBC = "SGBC";
    }

    /// <summary>
    /// Create BOMJournal Function identifier
    /// </summary>
    public static class CreateBOMJournalFunctionidentifier
    {
        //public const string PWTBC = "PWTBC";
        //public const string PVTBC = "PVTBC";       
        public const string CGT = "CGT";
        public const string SGBC = "SGBC";
    }

    /// <summary>
    /// Create InvMovJournal Function identifier
    /// </summary>
    public static class CreateInvMovJournalFunctionidentifier
    {
        public const string DBC2G = "DBC2G";
        public const string PCRGBC = "PCRGBC";
        public const string CBCI = "CBCI";
        public const string PLBC = "PLBC";
        //public const string RJOLNT = "RJOLNT";
        //public const string RJOLNQ = "RJOLNQ";
        public const string OREJ = "OREJ"; // Online Reject Glove Function identifier
    }

    /// <summary>
    /// Create InvTransJournal Function identifier
    /// </summary>
    public static class CreateInvTransJournalFunctionidentifier
    {
        public const string SCIN = "SCIN";
        public const string STPI = "STPI";
        public const string STPO = "STPO";
        public const string SCOT = "SCOT";
        public const string SPBC = "SPBC";//Scan Online PT(SPBC)
    }

    /// <summary>
    /// Rework Batch Order Function identifier
    /// </summary>
    public static class ReworkOrderFunctionidentifier
    {
        public const string RWKCR = "RWKCR";
        public const string RWKDEL = "RWKDEL";
    }

    /// <summary>
    /// Rework Batch Order Function identifier
    /// </summary>
    public static class CreatePickingListFunctionidentifier
    {
        public const string CBCI = "CBCI";
    }
}
