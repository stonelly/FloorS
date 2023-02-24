using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.IntegrationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic
{
    public class FSStagingBLL
    {
        public static string GetLocation(string FunctionID)
        {
            string location = string.Empty;
            switch (FunctionID)
            {
                case CreateRAFJournalFunctionidentifier.HBC:
                case CreateRAFJournalFunctionidentifier.SRBC:
                case CreateRAFJournalFunctionidentifier.ON2G:
                case CreateRAFJournalFunctionidentifier.PNBC:
                    location = "PN";
                    break;
                case CreateInvMovJournalFunctionidentifier.OREJ:
                    location = "";
                    break;
                case CreateFGRAFJournalFunctionidentifier.SBC:
                case CreateFGRAFJournalFunctionidentifier.SMBP:
                    location = "";
                    break;
                case CreateFGRAFJournalFunctionidentifier.SGBC:
                    location = "";
                    break;
                case CreateRAFJournalFunctionidentifier.PVTBCA:
                    location = "";
                    break;
                case CreateRAFJournalFunctionidentifier.PWTBCA:
                    location = "";
                    break;
                case CreateRAFJournalFunctionidentifier.PWTBCS:
                    location = "";
                    break;
                case CreateInvMovJournalFunctionidentifier.PLBC:
                    location = "PN";
                    break;
                case ReworkOrderFunctionidentifier.RWKCR:
                    location = "";
                    break;
                case CreateRAFJournalFunctionidentifier.SOBC:
                    location = "QC";
                    break;
                case CreateInvTransJournalFunctionidentifier.SPBC:
                    location = "PT";
                    break;
                case CreateRAFJournalFunctionidentifier.CGLV:
                    location = "CRQC";
                    break;
                default:
                    location = "";
                    break;
            }
            return location;
        }

        public static string GetWarehouse(string FunctionID)
        {
            string warehouse = string.Empty;
            switch (FunctionID)
            {
                case CreateRAFJournalFunctionidentifier.HBC:
                case CreateRAFJournalFunctionidentifier.SRBC:
                case CreateRAFJournalFunctionidentifier.ON2G:
                    warehouse = "";
                    break;
                case CreateInvMovJournalFunctionidentifier.OREJ:
                    warehouse = "";
                    break;
                case CreateFGRAFJournalFunctionidentifier.SBC:
                case CreateFGRAFJournalFunctionidentifier.SMBP:
                    warehouse = "FG";
                    break;
                case CreateFGRAFJournalFunctionidentifier.SGBC:
                    warehouse = "";
                    break;
                case CreateRAFJournalFunctionidentifier.PVTBCA:
                    warehouse = "";
                    break;
                case CreateRAFJournalFunctionidentifier.PWTBCA:
                    warehouse = "";
                    break;
                case CreateRAFJournalFunctionidentifier.PWTBCS:
                    warehouse = "";
                    break;
                case CreateInvMovJournalFunctionidentifier.PLBC:
                    warehouse = "";
                    break;
                case ReworkOrderFunctionidentifier.RWKCR:
                    warehouse = "";
                    break;
                case CreateRAFJournalFunctionidentifier.SOBC:
                    warehouse = "";
                    break;
                case CreateInvTransJournalFunctionidentifier.SPBC:
                    warehouse = "";
                    break;
                default:
                    warehouse = "";
                    break;
            }
            return warehouse;
        }

        public const string DOTFGJournalContract = "DOTFGJournalContract";
        public const string DOTMovementJournalContract = "DOTMovementJournalContract";
        public const string DOTRAFStgJournalContract = "DOTRAFStgJournalContract";
        public const string DOTTransferJournalContract = "DOTTransferJournalContract";
        public const string DOTReworkJournalContract = "DOTReworkJournalContract";
        public const string DOTPickingListContract = "DOTPickingListContract";

        public static int InsertParentStaging(AvaInterfaceContract _interfaceContract)
        {
            _interfaceContract.FSIdentifier = System.Guid.NewGuid();
            #region #AZ 12/2/2018 get all parent table info
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@BATCHCARDNUMBER", _interfaceContract.BatchCardNumber));
            PrmList.Add(new FloorSqlParameter("@BATCHNUMBER", _interfaceContract.BatchNumber));
            PrmList.Add(new FloorSqlParameter("@FSIDENTIFIER", _interfaceContract.FSIdentifier));
            PrmList.Add(new FloorSqlParameter("@FUNCTIONIDENTIFIER", _interfaceContract.FunctionID));
            PrmList.Add(new FloorSqlParameter("@PLANTNO", _interfaceContract.PlantNo));
            PrmList.Add(new FloorSqlParameter("@REFERENCEBATCHNUMBER1", _interfaceContract.ReferenceBatchNumber1));
            PrmList.Add(new FloorSqlParameter("@REFERENCEBATCHNUMBER2", _interfaceContract.ReferenceBatchNumber2));
            PrmList.Add(new FloorSqlParameter("@REFERENCEBATCHNUMBER3", _interfaceContract.ReferenceBatchNumber3));
            PrmList.Add(new FloorSqlParameter("@REFERENCEBATCHNUMBER4", _interfaceContract.ReferenceBatchNumber4));
            PrmList.Add(new FloorSqlParameter("@REFERENCEBATCHNUMBER5", _interfaceContract.ReferenceBatchNumber5));
            PrmList.Add(new FloorSqlParameter("@REFERENCEBATCHSEQUENCE1", _interfaceContract.ReferenceBatchSequence1));
            PrmList.Add(new FloorSqlParameter("@REFERENCEBATCHSEQUENCE2", _interfaceContract.ReferenceBatchSequence2));
            PrmList.Add(new FloorSqlParameter("@REFERENCEBATCHSEQUENCE3", _interfaceContract.ReferenceBatchSequence3));
            PrmList.Add(new FloorSqlParameter("@REFERENCEBATCHSEQUENCE4", _interfaceContract.ReferenceBatchSequence4));
            PrmList.Add(new FloorSqlParameter("@REFERENCEBATCHSEQUENCE5", _interfaceContract.ReferenceBatchSequence5));
            PrmList.Add(new FloorSqlParameter("@SEQUENCE", _interfaceContract.BatchSequence));
            PrmList.Add(new FloorSqlParameter("@PROCESSINGSTATUS", _interfaceContract.ProcessingStatus));
            PrmList.Add(new FloorSqlParameter("@ISCONSOLIDATED", _interfaceContract.IsConsolidated));

            int identity = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_DOT_CreateFLOORAXINTPARENTTABLE", PrmList));
            #endregion
            return identity;
        }

        public static int InsertRAFStaging(AvaInterfaceContract _interfaceContract, int identity)
        {
            #region #AZ 12/2/2018 get all child table info
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@BATCHORDERNUMBER", _interfaceContract.RAFStgJournal.BatchOrder));
            PrmList.Add(new FloorSqlParameter("@BATCHWT", _interfaceContract.RAFStgJournal.BatchWeight));
            PrmList.Add(new FloorSqlParameter("@CONFIGURATION", _interfaceContract.RAFStgJournal.Configuration));
            PrmList.Add(new FloorSqlParameter("@ITEMNUMBER", _interfaceContract.RAFStgJournal.ItemNumber));
            PrmList.Add(new FloorSqlParameter("@PARENTREFRECID", identity));
            PrmList.Add(new FloorSqlParameter("@POSTINGDATETIME", _interfaceContract.RAFStgJournal.PostingDateTime));
            PrmList.Add(new FloorSqlParameter("@QCTYPE", _interfaceContract.RAFStgJournal.QCType));
            PrmList.Add(new FloorSqlParameter("@RAFGOODQTY ", _interfaceContract.RAFStgJournal.RAFGoodQty));
            PrmList.Add(new FloorSqlParameter("@RAFHBSAMPLE", _interfaceContract.RAFStgJournal.RAFHBSample));
            PrmList.Add(new FloorSqlParameter("@RAFVTSAMPLE", _interfaceContract.RAFStgJournal.RAFVTSample));
            PrmList.Add(new FloorSqlParameter("@RAFWTSAMPLE", _interfaceContract.RAFStgJournal.RAFWTSample));
            PrmList.Add(new FloorSqlParameter("@RESOURCE", _interfaceContract.RAFStgJournal.Resource));
            PrmList.Add(new FloorSqlParameter("@SHIFT", _interfaceContract.RAFStgJournal.Shift));
            PrmList.Add(new FloorSqlParameter("@TENPCSWT", _interfaceContract.RAFStgJournal.Weightof10Pcs));
            PrmList.Add(new FloorSqlParameter("@WAREHOUSE", _interfaceContract.RAFStgJournal.Warehouse));
            PrmList.Add(new FloorSqlParameter("@LOCATION", _interfaceContract.RAFStgJournal.Location));
            PrmList.Add(new FloorSqlParameter("@RejectedQuantity", _interfaceContract.RAFStgJournal.RejectedQty));
            PrmList.Add(new FloorSqlParameter("@SecondGradeQuantity", _interfaceContract.RAFStgJournal.SecGradeQty));
            PrmList.Add(new FloorSqlParameter("@RejectedSampleQuantity", _interfaceContract.RAFStgJournal.RejectedSampleQty));
            PrmList.Add(new FloorSqlParameter("@Quantity", _interfaceContract.RAFStgJournal.Quantity));
            PrmList.Add(new FloorSqlParameter("@CHANGEDITEMNUMBER", _interfaceContract.RAFStgJournal.ChangedItemNumber));
            #endregion
            return FloorDBAccess.ExecuteNonQuery("USP_DOT_CreateRAFJournal", PrmList);
        }

        public static int InsertRWKCRStaging(AvaInterfaceContract _interfaceContract, int identity)
        {
            #region #MK 12/2/2018 get all child table info
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@CONFIGURATION", _interfaceContract.ReworkStg.Configuration));
            PrmList.Add(new FloorSqlParameter("@DELIVERYDATE", _interfaceContract.ReworkStg.DeliveryDate));
            PrmList.Add(new FloorSqlParameter("@ITEMNUMBER", _interfaceContract.ReworkStg.ItemNumber));
            PrmList.Add(new FloorSqlParameter("@PARENTREFRECID", identity));
            PrmList.Add(new FloorSqlParameter("@POOL", _interfaceContract.ReworkStg.Pool));
            PrmList.Add(new FloorSqlParameter("@POSTINGDATETIME", _interfaceContract.ReworkStg.PostingDateandTime));
            PrmList.Add(new FloorSqlParameter("@QUANTITY ", _interfaceContract.ReworkStg.Quantity));
            PrmList.Add(new FloorSqlParameter("@ROUTECATEGORY", _interfaceContract.ReworkStg.RouteCategory));
            PrmList.Add(new FloorSqlParameter("@WAREHOUSE", _interfaceContract.ReworkStg.Warehouse));
            PrmList.Add(new FloorSqlParameter("@OriRWKNum", _interfaceContract.ReworkStg.OriRWKNum));
            #endregion
            return FloorDBAccess.ExecuteNonQuery("USP_DOT_CreateReworkOrder", PrmList);
        }

        public static int InsertMovementJournal(AvaInterfaceContract _interfaceContract, int identity)
        {
            #region #AZ 15/5/2018 get all child table info
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PARENTREFRECID", identity));
            PrmList.Add(new FloorSqlParameter("@POSTINGDATEANDTIME", _interfaceContract.MovementJournal.PostingDateTime));
            PrmList.Add(new FloorSqlParameter("@QUANTITY", _interfaceContract.MovementJournal.Quantity));
            PrmList.Add(new FloorSqlParameter("@RESOURCEGROUP ", _interfaceContract.MovementJournal.ResourceGroup));
            PrmList.Add(new FloorSqlParameter("@SHIFT", _interfaceContract.MovementJournal.Shift));
            PrmList.Add(new FloorSqlParameter("@WAREHOUSE", _interfaceContract.MovementJournal.Warehouse));
            PrmList.Add(new FloorSqlParameter("@LOCATION", _interfaceContract.MovementJournal.Location));
            PrmList.Add(new FloorSqlParameter("@ITEMNUMBER", _interfaceContract.MovementJournal.ItemNumber));
            PrmList.Add(new FloorSqlParameter("@CONFIGURATION", _interfaceContract.MovementJournal.Configuration));
            #endregion
            return FloorDBAccess.ExecuteNonQuery("USP_DOT_CreateMovementJournal", PrmList);
        }

        public static int InsertFGRAFStaging(AvaInterfaceContract _interfaceContract, int identity)
        {
            #region #AZ 20/2/2018 get all child table info
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@BATCHORDERNUMBER", _interfaceContract.FGJournal.BatchOrder));
            PrmList.Add(new FloorSqlParameter("@REFERENCEITEMNUMBER", _interfaceContract.FGJournal.FGItemNumber));//FG Item Code
            PrmList.Add(new FloorSqlParameter("@CONFIGURATION", _interfaceContract.FGJournal.Configuration));
            PrmList.Add(new FloorSqlParameter("@WAREHOUSE", _interfaceContract.FGJournal.Warehouse));
            PrmList.Add(new FloorSqlParameter("@RESOURCE", _interfaceContract.FGJournal.Resource));
            PrmList.Add(new FloorSqlParameter("@CUSTOMERPO", _interfaceContract.FGJournal.CustomerPO));
            PrmList.Add(new FloorSqlParameter("@CUSTOMERREFERENCE", _interfaceContract.FGJournal.CustomerReference));
            PrmList.Add(new FloorSqlParameter("@SALESORDERNUMBER", _interfaceContract.FGJournal.SalesOrderNumber));
            PrmList.Add(new FloorSqlParameter("@INNERLOTNUMBER", _interfaceContract.FGJournal.InnerLotNumber));
            PrmList.Add(new FloorSqlParameter("@OUTERLOTNUMBER", _interfaceContract.FGJournal.OuterLotNumber));
            PrmList.Add(new FloorSqlParameter("@CUSTOMERLOTNUMBER", _interfaceContract.FGJournal.CustomerLotNumber));
            PrmList.Add(new FloorSqlParameter("@PRESHIPMENT", _interfaceContract.FGJournal.Preshipment));
            PrmList.Add(new FloorSqlParameter("@PRESHIPMENTCASES", _interfaceContract.FGJournal.PreshipmentCases));
            PrmList.Add(new FloorSqlParameter("@QUANTITY", _interfaceContract.FGJournal.Quantity));
            PrmList.Add(new FloorSqlParameter("@PARENTREFRECID", identity));
            PrmList.Add(new FloorSqlParameter("@LOCATION", _interfaceContract.FGJournal.Location));
            PrmList.Add(new FloorSqlParameter("@PALLETNUMBER", _interfaceContract.FGJournal.PalletId));
            PrmList.Add(new FloorSqlParameter("@POSTINGDATETIME", _interfaceContract.FGJournal.PostingDateTime));

            PrmList.Add(new FloorSqlParameter("@EXPIRYDATE", _interfaceContract.FGJournal.ExpiryDate));
            PrmList.Add(new FloorSqlParameter("@MANUFACTURINGDATE", _interfaceContract.FGJournal.ManufacturingDate));
            //Set glove code 
            PrmList.Add(new FloorSqlParameter("@IsWTS", _interfaceContract.FGJournal.IsWTS)); // without SO or not
            //flag to detect is from Make to Stock
            PrmList.Add(new FloorSqlParameter("@ItemNumber", _interfaceContract.FGJournal.ItemNumber));//Golve Code No
            // Set parameters for SMBP
            //PrmList.Add(new FloorSqlParameter("@REFBATCHNUMBER4", _interfaceContract.FGJournal.RefBatchNumber4));
            //PrmList.Add(new FloorSqlParameter("@REFCONFIGURATION1", _interfaceContract.FGJournal.RefConfiguration1));// not in use
            //PrmList.Add(new FloorSqlParameter("@REFCONFIGURATION2", _interfaceContract.FGJournal.RefConfiguration2));// not in use
            //PrmList.Add(new FloorSqlParameter("@REFCONFIGURATION3", _interfaceContract.FGJournal.RefConfiguration3));// not in use
            //PrmList.Add(new FloorSqlParameter("@REFCONFIGURATION4", _interfaceContract.FGJournal.RefConfiguration4));// not in use
            //PrmList.Add(new FloorSqlParameter("@REFCONFIGURATION5", _interfaceContract.FGJournal.RefConfiguration5));// not in use

            PrmList.Add(new FloorSqlParameter("@RefNumberOfPieces1", _interfaceContract.FGJournal.RefNumberOfPieces1));
            PrmList.Add(new FloorSqlParameter("@RefNumberOfPieces2", _interfaceContract.FGJournal.RefNumberOfPieces2));
            PrmList.Add(new FloorSqlParameter("@RefNumberOfPieces3", _interfaceContract.FGJournal.RefNumberOfPieces3));
            PrmList.Add(new FloorSqlParameter("@RefNumberOfPieces4", _interfaceContract.FGJournal.RefNumberOfPieces4));
            PrmList.Add(new FloorSqlParameter("@RefNumberOfPieces5", _interfaceContract.FGJournal.RefNumberOfPieces5));

            PrmList.Add(new FloorSqlParameter("@RefItemNumber1", _interfaceContract.FGJournal.RefItemNumber1));
            PrmList.Add(new FloorSqlParameter("@RefItemNumber2", _interfaceContract.FGJournal.RefItemNumber2));
            PrmList.Add(new FloorSqlParameter("@RefItemNumber3", _interfaceContract.FGJournal.RefItemNumber3));
            PrmList.Add(new FloorSqlParameter("@RefItemNumber4", _interfaceContract.FGJournal.RefItemNumber4));
            PrmList.Add(new FloorSqlParameter("@RefItemNumber5", _interfaceContract.FGJournal.RefItemNumber5));
            #endregion
            return FloorDBAccess.ExecuteNonQuery("USP_DOT_CreateFGRAFJournal", PrmList);
        }

        public static int InsertTransferJournal(AvaInterfaceContract _interfaceContract, int identity)
        {
            #region #AZ 20/2/2018 get all child table info
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@BATCHCARDNUMBER", _interfaceContract.BatchCardNumber));
            PrmList.Add(new FloorSqlParameter("@BATCHNUMBER", _interfaceContract.BatchNumber));
            PrmList.Add(new FloorSqlParameter("@BRAND", _interfaceContract.TransferJournal.Brand));
            PrmList.Add(new FloorSqlParameter("@CONFIGURATION", _interfaceContract.TransferJournal.Configuration));
            PrmList.Add(new FloorSqlParameter("@FORMULA", _interfaceContract.TransferJournal.Formula));
            PrmList.Add(new FloorSqlParameter("@ITEMNUMBER", _interfaceContract.TransferJournal.ItemNumber));
            PrmList.Add(new FloorSqlParameter("@PARENTREFRECID", identity));
            PrmList.Add(new FloorSqlParameter("@QUANTITY", _interfaceContract.TransferJournal.Quantity));
            PrmList.Add(new FloorSqlParameter("@SCANINDATETIME", _interfaceContract.TransferJournal.ScanInDateTime));
            PrmList.Add(new FloorSqlParameter("@SCANOUTDATETIME", _interfaceContract.TransferJournal.ScanOutDateTime));
            PrmList.Add(new FloorSqlParameter("@TRANSFERJOURNALID", _interfaceContract.TransferJournal.TransferJournalId));
            PrmList.Add(new FloorSqlParameter("@WAREHOUSE", _interfaceContract.TransferJournal.Warehouse));
            PrmList.Add(new FloorSqlParameter("@LOCATION", _interfaceContract.TransferJournal.Location));
            #endregion
            return FloorDBAccess.ExecuteNonQuery("USP_DOT_CreateTransferJournal", PrmList);
        }

        public static int InsertPickingList(AvaInterfaceContract _interfaceContract, int identity)
        {
            #region #AZ 11/06/2018 get all child table info
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@Configuration", _interfaceContract.PickingList.Configuration));
            PrmList.Add(new FloorSqlParameter("@CreateDateAndTime", _interfaceContract.PickingList.CreatedDateTime));
            PrmList.Add(new FloorSqlParameter("@InternalReferenceNumber", _interfaceContract.PickingList.InternalReferenceNumber));
            PrmList.Add(new FloorSqlParameter("@Location", _interfaceContract.PickingList.Location));
            PrmList.Add(new FloorSqlParameter("@PSIReworkOrderNo", _interfaceContract.PickingList.PSIReworkOrderNo));
            PrmList.Add(new FloorSqlParameter("@ParentRefRecId", identity));
            PrmList.Add(new FloorSqlParameter("@PostingDateTime", _interfaceContract.PickingList.PostingDateTime));
            PrmList.Add(new FloorSqlParameter("@QCType", _interfaceContract.PickingList.QCType));
            PrmList.Add(new FloorSqlParameter("@OldBatchQTy", _interfaceContract.PickingList.RAFGoodQty));
            PrmList.Add(new FloorSqlParameter("@SalesOrderNumber", _interfaceContract.PickingList.SalesOrderNumber));
            PrmList.Add(new FloorSqlParameter("@TenPcsWt", _interfaceContract.PickingList.TenPcsWt));
            PrmList.Add(new FloorSqlParameter("@Warehouse", _interfaceContract.PickingList.Warehouse));
            PrmList.Add(new FloorSqlParameter("@ItemNumber", _interfaceContract.PickingList.ItemNumber));
            PrmList.Add(new FloorSqlParameter("@ReferenceItemNumber", _interfaceContract.PickingList.ReferenceItemNumber));
            PrmList.Add(new FloorSqlParameter("@RefNumberOfPieces1", _interfaceContract.PickingList.RefNumberOfPieces1));
            PrmList.Add(new FloorSqlParameter("@RefNumberOfPieces2", _interfaceContract.PickingList.RefNumberOfPieces2));
            PrmList.Add(new FloorSqlParameter("@RefNumberOfPieces3", _interfaceContract.PickingList.RefNumberOfPieces3));
            #endregion
            return FloorDBAccess.ExecuteNonQuery("USP_DOT_CreatePickingList", PrmList);
        }
    }
}
