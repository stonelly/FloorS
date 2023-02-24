using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.IntegrationServices;
// -----------------------------------------------------------------------
// <copyright file="AXPostingBLL.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic
{
    /// <summary>
    /// AX Postings business logic class
    /// </summary>getsequence
    public class AXPostingBLL : Framework.Business.BusinessBase
    {
        #region Constructors
        static AXPostingBLL()
        {
            InitPosting();
        }

        #endregion       
        #region Member Methods
        private static void InitPosting()
        {
            //AXAgentOne.connectionString = EncryptDecrypt.GetDecryptedString(FloorSystemConfiguration.GetInstance().strAXConnectionString, "hidden");
            //AXAgentOne.userName = EncryptDecrypt.GetDecryptedString(FloorSystemConfiguration.GetInstance().strAXUserName, "hidden");
            //AXAgentOne.password = EncryptDecrypt.GetDecryptedString(FloorSystemConfiguration.GetInstance().strAXPassword, "hidden");
            //AXAgentOne.domain = FloorSystemConfiguration.GetInstance().strAXDomain;
            //AXAgentOne.fullDomain = FloorSystemConfiguration.GetInstance().strAXDomainFullName;
        }

        /// <summary>
        /// Azrul 26/05/2022: Get posting sequence for checking purpose. 
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="plantNo"></param>
        /// <returns></returns>
        public static BatchDTO GetBatchSequence(string serialNumber, string plantNo)
        {
            BatchDTO batchSequence = new BatchDTO();
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            PrmList.Add(new FloorSqlParameter("@PlantNo", plantNo));
            using (DataTable table = FloorDBAccess.ExecuteDataTable("USP_GET_BATCHSEQUENCE", PrmList))
            {
                try
                {
                    if (table != null && table.Rows.Count > 0)
                    {
                        batchSequence.BatchSequenceNo = Convert.ToInt32(table.Rows[0]["BatchSequence"]);
                        //batchSequence.IsConsolidated = Convert.ToBoolean(table.Rows[0]["IsConsolidated"]);
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                }
            }
            return batchSequence;
        }

        public static bool PostAXDataTumbling(decimal serialNumber)
        {
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            BatchDTO batchdto = SurgicalGloveBLL.GetQAIDetails_SRBC(serialNumber);

            if (batchdto.SubModule == Convert.ToString(Convert.ToInt16(Constants.SubModules.NormalBatchCard)))
            {
                BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, batchdto.Location); //Azrul 2022-05-26: Block Seq 2 onwards for PNBC posting.
                if (batchSequence.BatchSequenceNo == 1)
                {
                    AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
                    AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();
                    avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
                    avarafstgJournalContract.BatchOrder = batchdto.BatchOrder;
                    avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
                    avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                    avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
                    avarafstgJournalContract.Shift = batchdto.ShiftName;
                    avarafstgJournalContract.Configuration = batchdto.Size;
                    avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
                    avarafstgJournalContract.QCType = batchdto.QCType;
                    avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
                    avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                    avarafstgJournalContract.RAFGoodQty = batchdto.TotalPcs;
                    avarafstgJournalContract.ItemNumber = batchdto.GloveType;
                    avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.PNBC.ToString();
                    avarafstgJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID);
                    avarafstgJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID);
                    avarafstgJournalContract.Resource = batchdto.Resource;
                    avarafstgJournalContract.SeqNo = 1;
                    batchdto = CommonBLL.SetQAIBatchDetails(batchdto);// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
                    avarafstgJournalContract.RAFWTSample = batchdto.RAFWTSample;
                    avarafstgJournalContract.RAFVTSample = batchdto.TenPcsRAFSample;
                    avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
                    avaInterfaceContract.PlantNo = plantNo;
                    avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                    AXPostingDTO axposting = new AXPostingDTO()
                    {
                        SerialNumber = batchdto.SerialNumber,
                        BatchNumber = batchdto.BatchNumber,
                        IsPostedToAX = true,
                        ServiceName = CreateRAFJournalFunctionidentifier.PNBC,
                        PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                        PostingType = FSStagingBLL.DOTRAFStgJournalContract,
                        Area = batchdto.Area,
                        IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                        PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5  
                    };

                    /**#AZ 20/2/2018  Decouple AXAgent
                    using (AXAgentOne axAgent = new AXAgentOne())
                    {
                        
                        string[] axResponse = axAgent.createRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                       StringSplitOptions.RemoveEmptyEntries);
                        
                        CaptureResult(axResponse, axposting);
                    }
                    **/
                    var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                    axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertRAFStaging(avaInterfaceContract, identity));
                    axposting.IsPostedInAX = true;
                    LogAXPostingInfo(axposting);
                    return axposting.IsPostedInAX;
                }
                else
                {
                    return true;
                }
            }

            if (batchdto.Module == Convert.ToString(Convert.ToInt16(Constants.Modules.HOURLYBATCHCARD)) && batchdto.IsOnline == false)
            {
                AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
                AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();
                avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
                avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
                avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
                avarafstgJournalContract.Shift = batchdto.ShiftName;
                avarafstgJournalContract.Configuration = batchdto.Size;
                avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
                avarafstgJournalContract.QCType = batchdto.QCType;
                avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
                avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                avarafstgJournalContract.RAFGoodQty = batchdto.TotalPcs;
                avarafstgJournalContract.ItemNumber = batchdto.GloveType;
                avarafstgJournalContract.Warehouse = batchdto.Location + "-" + batchdto.Area;
                avarafstgJournalContract.Resource = batchdto.Location + "-" + batchdto.Line + "-" + batchdto.Size;
                avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.OBGPC.ToString();
                avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
                avaInterfaceContract.PlantNo = plantNo;
                avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                AXPostingDTO axposting = new AXPostingDTO()
                {
                    SerialNumber = batchdto.SerialNumber,
                    BatchNumber = batchdto.BatchNumber,
                    IsPostedToAX = true,
                    ServiceName = CreateRAFJournalFunctionidentifier.OBGPC,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = FSStagingBLL.DOTRAFStgJournalContract,
                    Area = batchdto.Area,
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5  
                };
                /**#AZ 20/2/2018  Decouple AXAgent
                using (AXAgentOne axAgent = new AXAgentOne())
                {
                    
                    string[] axResponse = axAgent.createRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                   StringSplitOptions.RemoveEmptyEntries);
                    
                    CaptureResult(axResponse, axposting);
                }
                **/
                var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertRAFStaging(avaInterfaceContract, identity));
                axposting.IsPostedInAX = true;
                LogAXPostingInfo(axposting);
                return axposting.IsPostedInAX;
            }

            /// #MH 6/18/2018 CR-118
            /// Change to use RAF Journal for all Visual Test function PVTBCA,PVTBCS
            if (batchdto.SubModule == Convert.ToString(Convert.ToInt16(Constants.SubModules.VisualTestBatchCard)) || // #Azrul 13/07/2018: Merged from Live AX6
                (batchdto.SubModule == Convert.ToString(Convert.ToInt16(Constants.SubModules.PrintReproductionVisualTestBatchCard)))) // #Azrul 13/07/2018: Merged from Live AX6
            {
                AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
                AXPostingDTO axposting = null; //#MH 1.n
                // Comment by Max He 2th November 2016 
                // Change PVTBCA to RAF Journal
                //AVAMovementJournalContract avaMovementJournalContract = new AVAMovementJournalContract();
                avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
                avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
                avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.PlantNo = batchdto.Location;
                var identity = 0;
                switch (batchdto.BatchType.Trim())
                {
                    //case "PVT":
                    //avaInterfaceContract.FunctionID = CreateInvMovJournalFunctionidentifier.PVTBCS.ToString();
                    //AVAMovementJournalContract avaMovementJournalContract = new AVAMovementJournalContract();  //#MH 1.n start
                    //avaMovementJournalContract.BatchWeight = batchdto.BatchWeight;
                    //avaMovementJournalContract.Shift = batchdto.ShiftName;
                    //avaMovementJournalContract.Configuration = batchdto.Size;
                    //avaMovementJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
                    //avaMovementJournalContract.QCType = batchdto.QCType;
                    //avaMovementJournalContract.CreatedDateTime = batchdto.BatchCarddate;
                    //avaMovementJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                    //avaMovementJournalContract.Area = batchdto.Area;
                    //avaMovementJournalContract.ItemNumber = batchdto.GloveType;
                    //avaMovementJournalContract.Warehouse = batchdto.Location + "-" + batchdto.Area;
                    //avaMovementJournalContract.Quantity = batchdto.TotalPcs;
                    //avaInterfaceContract.MovementJournal = avaMovementJournalContract;
                    //axposting = new AXPostingDTO()
                    //{
                    //    SerialNumber = batchdto.SerialNumber,
                    //    BatchNumber = batchdto.BatchNumber,
                    //    IsPostedToAX = true,
                    //    ServiceName = avaInterfaceContract.FunctionID,
                    //    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    //    PostingType = FSStagingBLL.DOTMovementJournalContract,
                    //    Area = batchdto.Area
                    //};
                    /**#AZ 20/2/2018  Decouple AXAgent
                    using (AXAgentOne axAgent = new AXAgentOne())
                    {
                        
                        string[] axResponse = axAgent.createInvMovJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                       StringSplitOptions.RemoveEmptyEntries);
                        
                        CaptureResult(axResponse, axposting);
                    }
                    **/
                    //identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                    //axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertMovementJournal(avaInterfaceContract, identity));
                    //axposting.IsPostedInAX = true;
                    //LogAXPostingInfo(axposting);
                    //break;
                    case Constants.PVT:// #MH 6/18/2018 CR-118 change PVTBCS to RAF and Rework process
                    case Constants.QVT:
                        avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.PVTBCA.ToString();
                        // Comment by Max He 2th November 2016 
                        // Change PVTBCA to RAF Journal
                        AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();   //#MH 1.n start
                        avaInterfaceContract.ProcessingStatus = 3; // set to completed
                        avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
                        avarafstgJournalContract.Shift = batchdto.ShiftName;
                        avarafstgJournalContract.Configuration = batchdto.Size;
                        avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
                        avarafstgJournalContract.QCType = batchdto.QCType;
                        avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
                        avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                        avarafstgJournalContract.RAFGoodQty = batchdto.TotalPcs;
                        avarafstgJournalContract.ItemNumber = batchdto.GloveType;
                        avarafstgJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID); //batchdto.Location + "-" + batchdto.Area;
                        avarafstgJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID); //batchdto.Location + "-" + batchdto.RouteCategory; // #MK 30/05/2018
                        avarafstgJournalContract.Resource = string.Empty;
                        batchdto = CommonBLL.SetQAIBatchDetails(batchdto);// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
                        avarafstgJournalContract.RAFVTSample = batchdto.RAFVTSample;
                        avarafstgJournalContract.RAFWTSample = batchdto.RAFWTSample;
                        avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
                        avaInterfaceContract.PlantNo = plantNo;
                        avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                        axposting = new AXPostingDTO()
                        {
                            SerialNumber = batchdto.SerialNumber,
                            BatchNumber = batchdto.BatchNumber,
                            IsPostedToAX = true,
                            ServiceName = CreateRAFJournalFunctionidentifier.PVTBCA,
                            PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                            PostingType = FSStagingBLL.DOTRAFStgJournalContract,
                            Area = batchdto.Area,
                            IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                            PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
                        };
                        /**#AZ 20/2/2018  Decouple AXAgent
                        using (AXAgentOne axAgent = new AXAgentOne())
                        {
                            
                            string[] axResponse = axAgent.createRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                           StringSplitOptions.RemoveEmptyEntries);
                            
                            CaptureResult(axResponse, axposting);
                        }
                        **/
                        identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                        axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertRAFStaging(avaInterfaceContract, identity));
                        axposting.IsPostedInAX = true;
                        LogAXPostingInfo(axposting);
                        break;
                    default:
                        break;
                }
                return axposting.IsPostedInAX;
            }

            if (batchdto.SubModule == Convert.ToString(Convert.ToInt16(Constants.SubModules.LostBatchCard)))
            {
                AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
                AVAMovementJournalContract avaMovementJournalContract = new AVAMovementJournalContract();
                avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
                avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
                avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.FunctionID = CreateInvMovJournalFunctionidentifier.PLBC.ToString();
                avaInterfaceContract.PlantNo = batchdto.Location;
                //avaInterfaceContract.ProcessingStatus = 3; // PLBC not set to completed
                avaMovementJournalContract.BatchWeight = batchdto.BatchWeight;
                avaMovementJournalContract.Shift = batchdto.ShiftName;
                avaMovementJournalContract.Configuration = batchdto.Size;
                avaMovementJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
                avaMovementJournalContract.QCType = batchdto.QCType;
                avaMovementJournalContract.CreatedDateTime = batchdto.BatchCarddate;
                avaMovementJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                avaMovementJournalContract.Quantity = batchdto.TotalPcs;
                avaMovementJournalContract.ItemNumber = batchdto.GloveType;
                avaMovementJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID);
                avaMovementJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID);
                avaMovementJournalContract.Area = batchdto.Area;
                avaMovementJournalContract.ItemNumber = batchdto.GloveType;
                avaMovementJournalContract.Configuration = batchdto.Size;
                avaInterfaceContract.MovementJournal = avaMovementJournalContract;
                avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                var identity = 0;
                AXPostingDTO axposting = new AXPostingDTO()
                {
                    SerialNumber = batchdto.SerialNumber,
                    BatchNumber = batchdto.BatchNumber,
                    IsPostedToAX = true,
                    ServiceName = CreateInvMovJournalFunctionidentifier.PLBC,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = FSStagingBLL.DOTMovementJournalContract,
                    Area = batchdto.Area,
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
                };
                /**#AZ 20/2/2018  Decouple AXAgent
                using (AXAgentOne axAgent = new AXAgentOne())
                {
                    
                    string[] axResponse = axAgent.createInvMovJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                   StringSplitOptions.RemoveEmptyEntries);
                    CaptureResult(axResponse, axposting);
                }
                **/
                identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertMovementJournal(avaInterfaceContract, identity));
                axposting.IsPostedInAX = true;
                LogAXPostingInfo(axposting);
                return axposting.IsPostedInAX;
            }
            else
            {
                return false;
            }
        }

        public static bool PostAXDataCustomerRejectGloves(BatchDTO batchdto)
        {
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            //BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, plantNo); //Azrul 2022-01-07: replace by SQL function
            AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
            AVAMovementJournalContract avaMovementJournalContract = new AVAMovementJournalContract();
            avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
            avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
            avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
            avaInterfaceContract.FunctionID = CreateInvMovJournalFunctionidentifier.PCRGBC.ToString();
            avaMovementJournalContract.BatchWeight = batchdto.BatchWeight; // reject quantity(KG)
            avaMovementJournalContract.Quantity = batchdto.TotalPcs;  // reject TotalPCs
            avaMovementJournalContract.Shift = batchdto.ShiftName;
            avaMovementJournalContract.Configuration = batchdto.Size;
            avaMovementJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
            avaMovementJournalContract.QCType = batchdto.QCType;
            avaMovementJournalContract.CreatedDateTime = batchdto.BatchCarddate;
            avaMovementJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
            avaMovementJournalContract.ItemNumber = batchdto.Area + "-" + batchdto.GloveType;
            avaMovementJournalContract.Warehouse = batchdto.Location + "-" + batchdto.Area;
            avaMovementJournalContract.ReferenceItemNo = batchdto.QCGroupMembers; //  took ItemNo in QCGroupMembers
            avaMovementJournalContract.RefConfiguration = batchdto.QCGroupName; // took ItemSize in QCGroupName
            avaInterfaceContract.MovementJournal = avaMovementJournalContract;
            avaInterfaceContract.PlantNo = plantNo;
            avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

            AXPostingDTO axposting = new AXPostingDTO()
            {
                SerialNumber = batchdto.SerialNumber,
                BatchNumber = batchdto.BatchNumber,
                IsPostedToAX = true,
                ServiceName = CreateInvMovJournalFunctionidentifier.PCRGBC,
                PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                PostingType = FSStagingBLL.DOTMovementJournalContract,
                Area = batchdto.Area,
                IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
            };
            /**#AZ 20/2/2018  Decouple AXAgent
            using (AXAgentOne axAgent = new AXAgentOne())
            {
                            
                string[] axResponse = axAgent.createInvMovJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                StringSplitOptions.RemoveEmptyEntries);
                CaptureResult(axResponse, axposting);
                            
            }
            **/
            var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
            axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertMovementJournal(avaInterfaceContract, identity));
            axposting.IsPostedInAX = true;
            LogAXPostingInfo(axposting);
            return axposting.IsPostedInAX;
        }

        public static bool PostAXDataHourly(BatchDTO batchdto)
        {
            //BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, batchdto.Location); //Azrul 2022-01-07: replace by SQL function
            AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
            AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();
            avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
            avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
            avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
            avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.HBC.ToString();
            avaInterfaceContract.PlantNo = batchdto.Location;
            avarafstgJournalContract.SeqNo = batchdto.SeqNo;
            avarafstgJournalContract.BatchOrder = batchdto.BatchOrder;
            avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
            avarafstgJournalContract.Shift = batchdto.ShiftName;
            avarafstgJournalContract.Configuration = batchdto.Size;
            avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
            avarafstgJournalContract.QCType = batchdto.QCType;
            avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
            avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
            if (!batchdto.IsReSampling)// #MH 22/12/2016 resampling no need post good qty
                avarafstgJournalContract.RAFGoodQty = batchdto.TotalPcs;
            else
                avaInterfaceContract.ReferenceBatchNumber1 = "RESAMPLE";
            avarafstgJournalContract.ItemNumber = batchdto.GloveType;
            avarafstgJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID);
            avarafstgJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID);
            avarafstgJournalContract.Resource = batchdto.Resource;
            batchdto = CommonBLL.SetQAIBatchDetails(batchdto);// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
            if (batchdto.SeqNo == 1)
            {
                avarafstgJournalContract.RAFWTSample = batchdto.RAFWTSample;// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
                if (batchdto.IsReSampling)
                    avarafstgJournalContract.RAFVTSample = batchdto.RAFVTSample;// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0 // #Azrul 13/07/2018: Merged from Live AX6
                else
                    avarafstgJournalContract.RAFVTSample = batchdto.TenPcsRAFSample; // #Azman 21/02/2018 // #Azrul 13/07/2018: Merged from Live AX6
                avarafstgJournalContract.RAFHBSample = batchdto.HotBox;// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
            }
            avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
            avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

            AXPostingDTO axposting = new AXPostingDTO()
            {
                SerialNumber = batchdto.SerialNumber,
                BatchNumber = batchdto.BatchNumber,
                IsPostedToAX = true,
                ServiceName = CreateRAFJournalFunctionidentifier.HBC,
                PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                PostingType = FSStagingBLL.DOTRAFStgJournalContract,
                Area = batchdto.Area,
                IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                PlantNo = batchdto.Location // #AZRUL 20210909: Open batch flag for NGC1.5 
            };

            /**#AZ 20/2/2018  Decouple AXAgent
            using (AXAgentOne axAgent = new AXAgentOne())
            {
                string[] axResponse = axAgent.createRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                               StringSplitOptions.RemoveEmptyEntries);
                CaptureResult(axResponse, axposting);
            }
            **/
            Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "b4 InsertParentStaging", DateTime.Now));
            var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);

            Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "b4 InsertRAFStaging", DateTime.Now));
            axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertRAFStaging(avaInterfaceContract, identity));
            axposting.IsPostedInAX = true;
            //if (batchdto.SeqNo == 1) //#AZRUL 29-10-2018 BUGS 1226: The next seqence number for 2 Tier HBC posting is wrong.
            Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "b4 LogAXPostingInfo", DateTime.Now));
            LogAXPostingInfo(axposting);

            Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "after LogAXPostingInfo", DateTime.Now));
            return axposting.IsPostedInAX;
        }

        public static bool PostAXSurgicalBatchCard(BatchDTO batchdto)
        {
            BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, batchdto.Location); //Azrul 2022-05-26: Block Seq 2 onwards for SRBC posting.
            if (batchSequence.BatchSequenceNo == 1)
            {
                AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
                AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();
                avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
                avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
                avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.SRBC.ToString();
                avaInterfaceContract.PlantNo = batchdto.Location;
                avarafstgJournalContract.SeqNo = batchdto.SeqNo;
                avarafstgJournalContract.BatchOrder = batchdto.BatchOrder;
                avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
                avarafstgJournalContract.Shift = batchdto.ShiftName;
                avarafstgJournalContract.Configuration = batchdto.Size;
                avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
                avarafstgJournalContract.QCType = batchdto.QCType;
                avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
                avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                if (!batchdto.IsReSampling)// #MH 22/12/2016 resampling no need post good qty
                    avarafstgJournalContract.RAFGoodQty = batchdto.TotalPcs;
                else
                    avaInterfaceContract.ReferenceBatchNumber1 = "RESAMPLE";
                avarafstgJournalContract.ItemNumber = batchdto.GloveType;
                avarafstgJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID);
                avarafstgJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID);
                avarafstgJournalContract.Resource = batchdto.Resource;
                batchdto = CommonBLL.SetQAIBatchDetails(batchdto);// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
                if (batchdto.SeqNo == 1)
                {
                    avarafstgJournalContract.RAFWTSample = batchdto.RAFWTSample;// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
                    if (batchdto.IsReSampling)
                        avarafstgJournalContract.RAFVTSample = batchdto.RAFVTSample;// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0 // #Azrul 13/07/2018: Merged from Live AX6
                    else
                        avarafstgJournalContract.RAFVTSample = batchdto.TenPcsRAFSample; // #Azman 21/02/2018 // #Azrul 13/07/2018: Merged from Live AX6
                    avarafstgJournalContract.RAFHBSample = batchdto.HotBox;// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
                }
                avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
                avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                AXPostingDTO axposting = new AXPostingDTO()
                {
                    SerialNumber = batchdto.SerialNumber,
                    BatchNumber = batchdto.BatchNumber,
                    IsPostedToAX = true,
                    ServiceName = CreateRAFJournalFunctionidentifier.SRBC,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = FSStagingBLL.DOTRAFStgJournalContract,
                    Area = batchdto.Area,
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = batchdto.Location // #AZRUL 20210909: Open batch flag for NGC1.5 
                };

                var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertRAFStaging(avaInterfaceContract, identity));
                axposting.IsPostedInAX = true;
                LogAXPostingInfo(axposting);
                return axposting.IsPostedInAX;
            }
            else
            { 
                return true;
            }
        }

        public static bool PostAXOnline2GBatchCard(BatchDTO batchdto)
        {
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            //BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, plantNo); //Azrul 2022-01-07: replace by SQL function
            AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
            AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();
            avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
            avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
            avaInterfaceContract.BatchSequence = 1;
            avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.ON2G.ToString();
            avaInterfaceContract.PlantNo = plantNo;
            avarafstgJournalContract.SeqNo = 1;
            avarafstgJournalContract.BatchOrder = batchdto.BatchOrder;
            avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
            avarafstgJournalContract.Shift = batchdto.ShiftName;
            avarafstgJournalContract.Configuration = batchdto.Size;
            avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
            avarafstgJournalContract.QCType = "";
            avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
            avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
            avarafstgJournalContract.RAFGoodQty = batchdto.Quantity;
            avarafstgJournalContract.ItemNumber = batchdto.GloveType;
            avarafstgJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID);
            avarafstgJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID);
            avarafstgJournalContract.Resource = batchdto.Resource;
            batchdto = CommonBLL.SetQAIBatchDetails(batchdto);// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
            avarafstgJournalContract.RAFWTSample = batchdto.RAFWTSample;// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
            avarafstgJournalContract.RAFVTSample = batchdto.TenPcsRAFSample; // #Azman 21/02/2018 // #Azrul 13/07/2018: Merged from Live AX6
            avarafstgJournalContract.RAFHBSample = batchdto.HotBox;// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
            avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
            avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

            AXPostingDTO axposting = new AXPostingDTO()
            {
                SerialNumber = batchdto.SerialNumber,
                BatchNumber = batchdto.BatchNumber,
                IsPostedToAX = true,
                ServiceName = CreateRAFJournalFunctionidentifier.ON2G,
                PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                PostingType = FSStagingBLL.DOTRAFStgJournalContract,
                Area = batchdto.Area,
                IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
            };

            var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
            axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertRAFStaging(avaInterfaceContract, identity));
            axposting.IsPostedInAX = true;
            LogAXPostingInfo(axposting);
            return axposting.IsPostedInAX;
        }

        public static bool PostAXDataReworkOrder(BatchDTO batchdto)
        {
            if (batchdto.TotalPcs > 0) //#AZRUL 20220518: No RWKCR creation if Qty is 0
            {
                //BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, batchdto.Location); //Azrul 2022-01-07: replace by SQL function
                AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
                DOT_ReworkOrderBatchContract reworkContract = new DOT_ReworkOrderBatchContract();
                avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
                avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
                avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.FunctionID = ReworkOrderFunctionidentifier.RWKCR.ToString();
                avaInterfaceContract.PlantNo = batchdto.Location;
                reworkContract.Configuration = batchdto.Size;
                reworkContract.PostingDateandTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                reworkContract.Quantity = batchdto.TotalPcs;
                reworkContract.ItemNumber = batchdto.GloveType;
                reworkContract.Warehouse = batchdto.Warehouse;
                reworkContract.DeliveryDate = batchdto.DeliveryDate;
                reworkContract.Pool = batchdto.Pool;
                reworkContract.RouteCategory = batchdto.RouteCategory;
                reworkContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID);
                reworkContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID); //batchdto.Area;
                avaInterfaceContract.ReworkStg = reworkContract;
                avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                AXPostingDTO axposting = new AXPostingDTO()
                {
                    SerialNumber = batchdto.SerialNumber,
                    BatchNumber = batchdto.BatchNumber,
                    IsPostedToAX = true,
                    ServiceName = ReworkOrderFunctionidentifier.RWKCR,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = "REWORKORDERJournalContract",
                    Area = batchdto.Area,
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = batchdto.Location // #AZRUL 20210909: Open batch flag for NGC1.5 
                };

                /**20/2/2018  Decouple AXAgent
                using (AXAgentOne axAgent = new AXAgentOne())
                {
                    axposting.TransactionID = Convert.ToString(axAgent.createReworkBatchOrder(avaInterfaceContract));
                    string[] axResponse = axposting.TransactionID.Split(new string[] { "||" },
                                                        StringSplitOptions.RemoveEmptyEntries);
                }
                **/
                var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertRWKCRStaging(avaInterfaceContract, identity));
                axposting.IsPostedInAX = true;
                LogAXPostingInfo(axposting);
                return axposting.IsPostedInAX;
            }
            return true;
        }

        /// <summary>
        /// Max He 30/11/2020: QCQI fail PT, scan washer & dryer need to delete previous rework order if is OQC route
        /// will update AXPostingLog table from RWKCR to RWKCR-Del in USP_CheckAndUpdate_PreviousRework
        /// will update Previous Rework staging to complete not start post to D365 in USP_CheckAndUpdate_PreviousRework
        /// mark Previous Rework Order staging isRWKDeleted=1
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static bool PostAXDataDeleteReworkOrderPTQIFailPT(string serialNumber)
        {
            // Get Previous Rework Order Staging info
            //BatchDTO batchSequence = GetBatchSequence(serialNumber, ""); //Azrul 2022-01-07: replace by SQL function
            AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
            DOT_ReworkOrderBatchContract reworkContract = new DOT_ReworkOrderBatchContract();
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@serialNumber", serialNumber));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_CheckAndUpdate_PreviousRework", PrmList))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    avaInterfaceContract.BatchNumber = serialNumber;
                    avaInterfaceContract.BatchCardNumber = FloorDBAccess.GetString(dr, "BatchCardNumber");
                    avaInterfaceContract.BatchSequence = FloorDBAccess.GetValue<Int32>(dr, "Sequence");
                    avaInterfaceContract.FunctionID = FloorDBAccess.GetString(dr, "FunctionIdentifier");
                    avaInterfaceContract.PlantNo = FloorDBAccess.GetString(dr, "PlantNo");
                    avaInterfaceContract.ProcessingStatus = FloorDBAccess.GetValue<Int32>(dr, "ProcessingStatus");
                    reworkContract.DeliveryDate = CommonBLL.GetCurrentDateAndTimeFromServer();
                    reworkContract.Configuration = FloorDBAccess.GetString(dr, "Configuration");
                    reworkContract.PostingDateandTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                    reworkContract.Quantity = FloorDBAccess.GetValue<decimal>(dr, "Quantity");
                    reworkContract.ItemNumber = FloorDBAccess.GetString(dr, "ItemNumber");
                    reworkContract.Warehouse = FloorDBAccess.GetString(dr, "Warehouse");
                    reworkContract.Pool = FloorDBAccess.GetString(dr, "Pool");
                    reworkContract.RouteCategory = FloorDBAccess.GetString(dr, "RouteCategory");
                    reworkContract.OriRWKNum = FloorDBAccess.GetString(dr, "ReworkOrder");
                    avaInterfaceContract.ReworkStg = reworkContract;
                    avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                }
            }
            // check previous rework order if is OQC route
            if (avaInterfaceContract.FunctionID == ReworkOrderFunctionidentifier.RWKCR && reworkContract.RouteCategory == "OQC" ||
                FinalPackingBLL.ValidateSerialNoByFPExempt(Convert.ToDecimal(serialNumber)) == Constants.PASS)  //#AZRUL 27/07/2022: If got fp exempt, always Post RWKDEL
            {
                // insert to new function RWKDEL to AXPostingLog and Staging Table
                AXPostingDTO axposting = new AXPostingDTO()
                {
                    SerialNumber = serialNumber,
                    BatchNumber = avaInterfaceContract.BatchCardNumber,
                    IsPostedToAX = true,
                    ServiceName = ReworkOrderFunctionidentifier.RWKDEL,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = "REWORKORDERJournalContract",
                    Area = "RWK",
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = avaInterfaceContract.PlantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
                };
                avaInterfaceContract.FunctionID = ReworkOrderFunctionidentifier.RWKDEL;
                avaInterfaceContract.BatchSequence = avaInterfaceContract.BatchSequence + 1;
                avaInterfaceContract.ProcessingStatus = 1; // ready status
                var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertRWKCRStaging(avaInterfaceContract, identity));
                axposting.IsPostedInAX = true;
                LogAXPostingInfo(axposting);
                return axposting.IsPostedInAX;
            }
            return true;
        }

        public static bool PostAXDataQCPackingYield(BatchDTO batchdto)
        {
            //BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, batchdto.Location); //Azrul 2022-01-07: replace by SQL function
            AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
            AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();
            avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
            avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
            avarafstgJournalContract.ItemNumber = batchdto.GloveType;
            avarafstgJournalContract.Configuration = batchdto.Size;
            avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
            avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
            string packingInto = GetPackingInto(Convert.ToDecimal(batchdto.SerialNumber));
            if (packingInto == Constants.TMP_PACK)
            {
                avarafstgJournalContract.Warehouse = batchdto.Location + "-" + Constants.TMP_PACK_AREA;
            }
            else
            {
                avarafstgJournalContract.Warehouse = batchdto.Location + "-" + batchdto.Area;
            }
            avarafstgJournalContract.QCType = batchdto.QCType;
            avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
            avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
            avarafstgJournalContract.RAFGoodQty = batchdto.TotalPcs;
            avarafstgJournalContract.Shift = batchdto.ShiftName;
            // #MH 10/11/2016 1.n FDD:HLTG-REM-003            
            if (QAIBLL.CheckIsPostWT(batchdto.QCType))
                avarafstgJournalContract.RAFWTSample = batchdto.CalculatedRAFWTSample;
            //avarafstgJournalContract.RAFVTSample = batchdto.RAFVTSample;
            avarafstgJournalContract.RAFGoodQtyLoose = batchdto.RAFGoodQtyLoose;
            avarafstgJournalContract.RejectedQty = batchdto.RejectedQty;
            avarafstgJournalContract.SecGradeQty = batchdto.SecGradeQty;
            //#MH no need to care funcation identifier 10/7/2018
            //if (!(batchdto.BatchType == "QVT" || batchdto.BatchType == "QWT" || batchdto.BatchType == "PSW")) // #AZ 12/06/2018 Take out RejectedSampleQty if PWTBCA/PWTBCS/PVTBCA
            avarafstgJournalContract.RejectedSampleQty = batchdto.RejectedSampleQty;
            avarafstgJournalContract.CalculatedLooseQty = batchdto.CalculatedLooseQty;
            // #MH 10/11/2016 1.n FDD:HLTG-REM-003
            avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.SOBC.ToString();
            avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
            avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
            avaInterfaceContract.PlantNo = batchdto.Location;
            avarafstgJournalContract.Resource = string.Empty;
            avarafstgJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID);
            avarafstgJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID);
            avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

            //if (avaInterfaceContract.BatchSequence > Constants.ONE)
            //{
            AXPostingDTO axposting = new AXPostingDTO()
            {
                SerialNumber = batchdto.SerialNumber,
                BatchNumber = batchdto.BatchNumber,
                IsPostedToAX = true,
                ServiceName = CreateRAFJournalFunctionidentifier.SOBC,
                PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                PostingType = FSStagingBLL.DOTRAFStgJournalContract,
                Area = batchdto.Area,
                IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                PlantNo = batchdto.Location // #AZRUL 20210909: Open batch flag for NGC1.5 
            };

            /**#AZ 20/2/2018  Decouple AXAgent
            using (AXAgentOne axAgent = new AXAgentOne())
            {
                string[] axResponse = axAgent.createRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                StringSplitOptions.RemoveEmptyEntries);
                CaptureResult(axResponse, axposting);
                axposting.TransactionID = Convert.ToString(axAgent.createRAFJournal(avaInterfaceContract));
                string[] axResponse = axposting.TransactionID.Split(new string[] { "||" },
                                                    StringSplitOptions.RemoveEmptyEntries);
                CaptureResult(axResponse, axposting);
            }
            **/
            var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
            axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertRAFStaging(avaInterfaceContract, identity));
            axposting.IsPostedInAX = true;
            LogAXPostingInfo(axposting);
            return axposting.IsPostedInAX;
            //}
            //else
            //    return true;
        }

        public static bool PostAXDataQCScanningScanBatchCard(BatchDTO batchdto)
        {
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            //BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, plantNo); //Azrul 2022-01-07: replace by SQL function
            AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
            AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();
            avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
            avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
            avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
            avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.SBCIB.ToString();
            avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
            avarafstgJournalContract.Shift = batchdto.ShiftName;
            avarafstgJournalContract.Configuration = batchdto.Size;
            avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
            avarafstgJournalContract.QCType = batchdto.QCType;
            avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
            avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
            avarafstgJournalContract.RAFGoodQty = batchdto.TotalPcs;
            avarafstgJournalContract.ItemNumber = batchdto.GloveType;
            string packingInto = GetPackingInto(Convert.ToDecimal(batchdto.SerialNumber));
            if (packingInto == Constants.TMP_PACK)
            {
                avarafstgJournalContract.Warehouse = batchdto.Location + "-" + Constants.TMP_PACK_AREA;
            }
            else
            {
                avarafstgJournalContract.Warehouse = batchdto.Location + "-" + batchdto.Area;
            }
            // #MH 10/11/2016 1.n FDD:HLTG-REM-003            
            avarafstgJournalContract.RAFWTSample = batchdto.RAFWTSample;
            avarafstgJournalContract.RAFVTSample = batchdto.RAFVTSample;
            avarafstgJournalContract.RAFGoodQtyLoose = batchdto.RAFGoodQtyLoose;
            avarafstgJournalContract.RejectedQty = batchdto.RejectedQty;
            avarafstgJournalContract.RejectedSampleQty = batchdto.RejectedSampleQty;
            avarafstgJournalContract.SecGradeQty = batchdto.SecGradeQty;
            // #MH 10/11/2016 1.n FDD:HLTG-REM-003
            avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
            avaInterfaceContract.PlantNo = plantNo;
            avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

            //if (avaInterfaceContract.BatchSequence > Constants.ONE)
            //{
            AXPostingDTO axposting = new AXPostingDTO()
            {
                SerialNumber = batchdto.SerialNumber,
                BatchNumber = batchdto.BatchNumber,
                IsPostedToAX = true,
                ServiceName = CreateRAFJournalFunctionidentifier.SBCIB,
                PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                PostingType = "AVARAFStgJournalContract",
                Area = batchdto.Area,
                IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
            };

            /**20/2/2018  Decouple AXAgent
            using (AXAgentOne axAgent = new AXAgentOne())
            {
                string[] axResponse = axAgent.createRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                StringSplitOptions.RemoveEmptyEntries);
                CaptureResult(axResponse, axposting);
                
            }**/
            return axposting.IsPostedInAX;
            //}
            //else
            //    return true;
        }

        //public static bool PostAXDataQCScanningScanBatchCard_New(QCScanningDetails _resultDTO, QCYieldandPackingDTO objQCScanningDTO, decimal totalPcs)
        //{
        //    AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
        //    AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();

        //    avaInterfaceContract.BatchNumber = objQCScanningDTO.SerialNumber;
        //    avaInterfaceContract.BatchCardNumber = _resultDTO.BatchNumber;

        //    avarafstgJournalContract.BatchWeight = Convert.ToDecimal(objQCScanningDTO.BatchWeight);
        //    avarafstgJournalContract.Configuration = _resultDTO.Size;
        //    avarafstgJournalContract.Weightof10Pcs = Convert.ToDecimal(_resultDTO.TenPcsWeight);
        //    avarafstgJournalContract.QCType = _resultDTO.QCTypeDescription;

        //    string GloveType = _resultDTO.GloveType;
        //    string QcGroup = _resultDTO.QCGroup;
        //    decimal InnerBoxCount = objQCScanningDTO.InnerBoxCount;
        //    decimal PackingSize = objQCScanningDTO.PackingSize;
        //    decimal TotalPcs = totalPcs;

        //    using (AXAgentOne axAgent = new AXAgentOne())
        //    {
        //        return (axAgent.QCScanningScanBatchCard_New(avaInterfaceContract, avarafstgJournalContract, GloveType, QcGroup, InnerBoxCount, PackingSize, TotalPcs));
        //    }
        //    //return true;
        //}

        /// <summary>
        /// #MH 6/18/2018 CR-118
        /// Change to use RAF Journal for all PWTBCA,PWTBCP,PWTBCQ,PWTBCS
        /// </summary>
        /// <param name="batchdto"></param>
        /// <returns></returns>
        public static bool PostAXDataPrintWaterTightBatchCard(BatchDTO batchdto)
        {
            //BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, batchdto.Location); //Azrul 2022-01-07: replace by SQL function
            AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
            avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
            avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
            avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
            avaInterfaceContract.PlantNo = batchdto.Location;
            switch (batchdto.BatchType.Trim())
            {
                case "PWT":
                    avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.PWTBCP.ToString();
                    break;
                case "QWT":
                    avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.PWTBCA.ToString();
                    break;
                case "OWT":
                    avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.PWTBCQ.ToString();
                    break;
                case "PSW":
                    avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.PWTBCS.ToString();
                    break;
                default:
                    break;
            }
            // #MH 6/18/2018 CR-118
            // Change to use RAF Journal for all PWTBCA,PWTBCP,PWTBCQ,PWTBCS
            // #MH 3/11/2016 1.n FDD@NGC-REM-001-PT_V1.0
            // Change to use RAF Journal for PWTBCA
            AXPostingDTO axposting = null;
            //if (avaInterfaceContract.FunctionID == CreateInvMovJournalFunctionidentifier.PWTBCA.ToString() || avaInterfaceContract.FunctionID == CreateInvMovJournalFunctionidentifier.PWTBCS.ToString())
            //{
            AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();   //#MH 1.n start
            avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
            avarafstgJournalContract.Shift = batchdto.ShiftName;
            avarafstgJournalContract.Configuration = batchdto.Size;
            avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
            avarafstgJournalContract.QCType = batchdto.QCType;
            avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
            avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
            avarafstgJournalContract.RAFGoodQty = batchdto.TotalPcs;
            avarafstgJournalContract.ItemNumber = batchdto.GloveType;
            avarafstgJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID); //batchdto.Location + "-" + batchdto.Area;
            avarafstgJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID);
            batchdto = CommonBLL.SetQAIBatchDetails(batchdto);// #MH 3/11/2016 1.n FDD@NGC-REM-001-PT_V1.0
                                                              //avarafstgJournalContract.RAFVTSample = batchdto.RAFVTSample;  // #AZ 07/06/2018 Remove sampling for PWTBCA/PWT
                                                              //avarafstgJournalContract.RAFWTSample = batchdto.RAFWTSample; // #AZ 07/06/2018 Remove sampling for PWTBCA/PWT
            avarafstgJournalContract.RAFHBSample = batchdto.HotBox;
            avarafstgJournalContract.Resource = string.Empty; //batchdto.Location + "-" + batchdto.RouteCategory;
            avarafstgJournalContract.Location = string.Empty;
            avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
            avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

            axposting = new AXPostingDTO()
            {
                SerialNumber = batchdto.SerialNumber,
                BatchNumber = batchdto.BatchNumber,
                IsPostedToAX = true,
                ServiceName = avaInterfaceContract.FunctionID,
                PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                PostingType = FSStagingBLL.DOTRAFStgJournalContract,
                Area = batchdto.Area,
                IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                PlantNo = batchdto.Location // #AZRUL 20210909: Open batch flag for NGC1.5 
            };
            /**20/2/2018  Decouple AXAgent
            using (AXAgentOne axAgent = new AXAgentOne())
            {
                string[] axResponse = axAgent.createRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                               StringSplitOptions.RemoveEmptyEntries);
                
                axposting.TransactionID = Convert.ToString(axAgent.createRAFJournal(avaInterfaceContract));
                string[] axResponse = axposting.TransactionID.Split(new string[] { "||" },
                                                    StringSplitOptions.RemoveEmptyEntries);
                CaptureResult(axResponse, axposting);
            }
            **/
            var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
            axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertRAFStaging(avaInterfaceContract, identity));
            axposting.IsPostedInAX = true;
            LogAXPostingInfo(axposting);
            //}
            //else
            //{
            //    AVAMovementJournalContract avaMovementJournalContract = new AVAMovementJournalContract();
            //    avaMovementJournalContract.BatchWeight = batchdto.BatchWeight;
            //    avaMovementJournalContract.Shift = batchdto.ShiftName;
            //    avaMovementJournalContract.Configuration = batchdto.Size;
            //    avaMovementJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
            //    avaMovementJournalContract.QCType = batchdto.QCType;
            //    avaMovementJournalContract.CreatedDateTime = batchdto.BatchCarddate;
            //    avaMovementJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
            //    avaMovementJournalContract.Area = batchdto.Area;
            //    avaMovementJournalContract.ItemNumber = batchdto.GloveType;
            //    avaMovementJournalContract.Warehouse = batchdto.Location + "-" + batchdto.Area;
            //    avaMovementJournalContract.Quantity = batchdto.TotalPcs;
            //    avaInterfaceContract.MovementJournal = avaMovementJournalContract;
            //    axposting = new AXPostingDTO()
            //    {
            //        SerialNumber = batchdto.SerialNumber,
            //        BatchNumber = batchdto.BatchNumber,
            //        IsPostedToAX = true,
            //        ServiceName = avaInterfaceContract.FunctionID,
            //        PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
            //        PostingType = FSStagingBLL.DOTMovementJournalContract,
            //        Area = batchdto.Area
            //    };
            /**20/2/2018  Decouple AXAgent
            using (AXAgentOne axAgent = new AXAgentOne())
            {
                
                string[] axResponse = axAgent.createInvMovJournal(avaInterfaceContract).Split(new string[] { "||" },
                                               StringSplitOptions.RemoveEmptyEntries);
                
            CaptureResult(axResponse, axposting);
            }
            **/
            //    var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
            //    axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertMovementJournal(avaInterfaceContract, identity));
            //    axposting.IsPostedInAX = true;
            //    LogAXPostingInfo(axposting);
            //}
            return axposting.IsPostedInAX;
        }

        public static bool PostAXDataQCScanBatchCardPieces(BatchDTO batchdto)
        {
            bool result = true;
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            //BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, plantNo); //Azrul 2022-01-07: replace by SQL function
            //if (batchSequence.BatchSequenceNo > Constants.ONE)
            //{
                AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
                AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();
                avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
                avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
                avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.QCQI_QC.ToString();
                avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
                avarafstgJournalContract.Shift = batchdto.ShiftName;
                avarafstgJournalContract.Configuration = batchdto.Size;
                avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
                avarafstgJournalContract.QCType = batchdto.QCType;
                avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
                avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                avarafstgJournalContract.RAFGoodQty = batchdto.TotalPcs;
                avarafstgJournalContract.ItemNumber = batchdto.GloveType;
                avarafstgJournalContract.Warehouse = batchdto.Location + "-" + batchdto.Area;
                // #MH 10/11/2016 1.n FDD:HLTG-REM-003                
                avarafstgJournalContract.RAFWTSample = batchdto.RAFWTSample;
                avarafstgJournalContract.RAFVTSample = batchdto.RAFVTSample;
                avarafstgJournalContract.RAFGoodQtyLoose = batchdto.RAFGoodQtyLoose;
                avarafstgJournalContract.RejectedQty = batchdto.RejectedQty;
                avarafstgJournalContract.RejectedSampleQty = batchdto.RejectedSampleQty;
                avarafstgJournalContract.SecGradeQty = batchdto.SecGradeQty;
                // #MH 10/11/2016 1.n FDD:HLTG-REM-003
                avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
                avaInterfaceContract.PlantNo = plantNo;
                avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                AXPostingDTO axposting = new AXPostingDTO()
                {
                    SerialNumber = batchdto.SerialNumber,
                    BatchNumber = batchdto.BatchNumber,
                    IsPostedToAX = true,
                    ServiceName = CreateRAFJournalFunctionidentifier.QCQI_QC,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = "AVARAFStgJournalContract",
                    Area = batchdto.Area,
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
                };

                /**20/2/2018  Decouple AXAgent
                using (AXAgentOne axAgent = new AXAgentOne())
                {
                    string[] axResponse = axAgent.createRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                   StringSplitOptions.RemoveEmptyEntries);
                    CaptureResult(axResponse, axposting);
                    
                }**/
                result = axposting.IsPostedInAX;
            //}
            return result;
        }

        public static bool PostAXDataQCScanBatchCardWeight(BatchDTO batchdto)
        {
            bool result = true;
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            //BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, plantNo); //Azrul 2022-01-07: replace by SQL function
            //if (batchSequence.BatchSequenceNo > Constants.ONE)
            //{
                AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
                AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();
                avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
                avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
                avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.QCQI_QC.ToString();
                avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
                avarafstgJournalContract.Shift = batchdto.ShiftName;
                avarafstgJournalContract.Configuration = batchdto.Size;
                avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
                avarafstgJournalContract.QCType = batchdto.QCType;
                avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
                avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                avarafstgJournalContract.RAFGoodQty = batchdto.TotalPcs;
                avarafstgJournalContract.ItemNumber = batchdto.GloveType;
                avarafstgJournalContract.Warehouse = batchdto.Location + "-" + batchdto.Area;
                // #MH 10/11/2016 1.n FDD:HLTG-REM-003                
                avarafstgJournalContract.RAFWTSample = batchdto.RAFWTSample;
                avarafstgJournalContract.RAFVTSample = batchdto.RAFVTSample;
                avarafstgJournalContract.RAFGoodQtyLoose = batchdto.RAFGoodQtyLoose;
                avarafstgJournalContract.RejectedQty = batchdto.RejectedQty;
                avarafstgJournalContract.RejectedSampleQty = batchdto.RejectedSampleQty;
                avarafstgJournalContract.SecGradeQty = batchdto.SecGradeQty;
                // #MH 10/11/2016 1.n FDD:HLTG-REM-003
                avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
                avaInterfaceContract.PlantNo = plantNo;
                avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                AXPostingDTO axposting = new AXPostingDTO()
                {
                    SerialNumber = batchdto.SerialNumber,
                    BatchNumber = batchdto.BatchNumber,
                    IsPostedToAX = true,
                    ServiceName = CreateRAFJournalFunctionidentifier.QCQI_QC,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = "AVARAFStgJournalContract",
                    Area = batchdto.Area,
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
                };

                /**20/2/2018  Decouple AXAgent
                using (AXAgentOne axAgent = new AXAgentOne())
                {
                    string[] axResponse = axAgent.createRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                   StringSplitOptions.RemoveEmptyEntries);
                    CaptureResult(axResponse, axposting);
                    
                }**/
                result = axposting.IsPostedInAX;
            //}
            return result;
        }



        //public static bool PostAXDataQCScanBatchCardWeight_New(BatchDTO batchdto, string QcTypeDesc)
        //{
        //    bool result = true;
        //    if (GetBatchSequence(batchdto.SerialNumber) > Constants.ONE)
        //    {
        //        AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
        //        AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();
        //        AVAMovementJournalContract avaMovementJournalContract = new AVAMovementJournalContract();
        //        avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
        //        avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
        //        avaInterfaceContract.BatchSequence = GetBatchSequence(batchdto.SerialNumber);
        //        avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.QCQI_QC.ToString();
        //        //QCGroupId
        //        avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
        //        avarafstgJournalContract.Shift = batchdto.ShiftName;
        //        avarafstgJournalContract.Configuration = batchdto.Size;
        //        avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
        //        avarafstgJournalContract.QCType = batchdto.QCType;
        //        avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
        //        avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
        //        avarafstgJournalContract.RAFGoodQty = batchdto.TotalPcs;
        //        avarafstgJournalContract.ItemNumber = batchdto.Area + "-" + batchdto.GloveType;
        //        avarafstgJournalContract.Warehouse = batchdto.Location + "-" + batchdto.Area;
        //        // #MH 10/11/2016 1.n FDD:HLTG-REM-003                
        //        avarafstgJournalContract.RAFWTSample = batchdto.RAFWTSample;
        //        avarafstgJournalContract.RAFVTSample = batchdto.RAFVTSample;
        //        avarafstgJournalContract.RAFGoodQtyLoose = batchdto.RAFGoodQtyLoose;
        //        avarafstgJournalContract.RejectedQty = batchdto.RejectedQty;
        //        avarafstgJournalContract.RejectedSampleQty = batchdto.RejectedSampleQty;
        //        avarafstgJournalContract.SecGradeQty = batchdto.SecGradeQty;
        //        // #MH 10/11/2016 1.n FDD:HLTG-REM-003

        //        avaMovementJournalContract.RefConfiguration = batchdto.QCGroupName;

        //        avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
        //        AXPostingDTO axposting = new AXPostingDTO() { SerialNumber = batchdto.SerialNumber, BatchNumber = batchdto.BatchNumber, IsPostedToAX = true, ServiceName = CreateRAFJournalFunctionidentifier.QCQI_QC, PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(), PostingType = "AVARAFStgJournalContract", Area = batchdto.Area };

 
        //        using (AXAgentOne axAgent = new AXAgentOne())
        //        {
        //            result = axAgent.QCScanBatchCardWeight(avarafstgJournalContract, avaInterfaceContract, avaMovementJournalContract);
        //        }

        //        result = axposting.IsPostedInAX;
        //    }
        //    return result;
        //}

        public static bool PostAXDataQCScanDowngradeBatchCard(decimal serialNumber)
        {
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            BatchDTO batchdto = AXPostingBLL.GetCompleteDowngradeBatchDetails(serialNumber);
            //BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, plantNo); //Azrul 2022-01-07: replace by SQL function
            AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
            AVAMovementJournalContract avaMovementJournalContract = new AVAMovementJournalContract();
            avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
            avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
            avaInterfaceContract.FunctionID = CreateInvMovJournalFunctionidentifier.DBC2G.ToString();
            avaMovementJournalContract.Configuration = batchdto.Size;
            avaMovementJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();

            //TOTT#315
            avaMovementJournalContract.Quantity = Math.Floor(PostTreatmentBLL.GetPTLatestBatchWeight(serialNumber) * Constants.THOUSAND * Constants.TEN / PostTreatmentBLL.GetPTLatestTenPcsWeight(serialNumber));

            string[] gloveCode = batchdto.GloveType.Split('-');
            if (gloveCode[0] == Constants.DOWNGRADE_NB)
            {
                avaMovementJournalContract.ItemNumber = Constants.DOWNGRADE_ITEMNUMBER_NB;
            }
            else
            {
                avaMovementJournalContract.ItemNumber = Constants.DOWNGRADE_ITEMNUMBER_NR;
            }
            avaMovementJournalContract.Warehouse = batchdto.Location + "-" + batchdto.Area;
            avaInterfaceContract.MovementJournal = avaMovementJournalContract;
            avaInterfaceContract.PlantNo = plantNo;
            avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

            //if (avaInterfaceContract.BatchSequence > Constants.ONE)
            //{
            AXPostingDTO axposting = new AXPostingDTO()
            {
                SerialNumber = batchdto.SerialNumber,
                BatchNumber = batchdto.BatchNumber,
                IsPostedToAX = true,
                ServiceName = CreateInvMovJournalFunctionidentifier.DBC2G,
                PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                PostingType = "AVAMovementJournalContract",
                Area = batchdto.Area,
                IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
            };
            /**20/2/2018  Decouple AXAgent
            using (AXAgentOne axAgent = new AXAgentOne())
            {
                string[] axResponse = axAgent.createInvMovJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                StringSplitOptions.RemoveEmptyEntries);
                CaptureResult(axResponse, axposting);
                   
            } **/
            return axposting.IsPostedInAX;
            //}
            //else
            //    return true;
        }

        //public static bool PostAXDataQCScanDowngradeBatchCard_New(decimal serialNumber)
        //{
        //    bool result = true;
        //    BatchDTO batchdto = AXPostingBLL.GetCompleteDowngradeBatchDetails(serialNumber);
        //    AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
        //    AVAMovementJournalContract avaMovementJournalContract = new AVAMovementJournalContract();
        //    avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
        //    avaInterfaceContract.BatchSequence = GetBatchSequence(batchdto.SerialNumber);
        //    avaInterfaceContract.FunctionID = CreateInvMovJournalFunctionidentifier.DBC2G.ToString();
        //    avaMovementJournalContract.Configuration = batchdto.Size;
        //    avaMovementJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();

        //    //TOTT#315
        //    avaMovementJournalContract.Quantity = Math.Floor(PostTreatmentBLL.GetPTLatestBatchWeight(serialNumber) * Constants.THOUSAND * Constants.TEN / PostTreatmentBLL.GetPTLatestTenPcsWeight(serialNumber));

        //    string[] gloveCode = batchdto.GloveType.Split('-');
        //    if (gloveCode[0] == Constants.DOWNGRADE_NB)
        //    {
        //        avaMovementJournalContract.ItemNumber = Constants.DOWNGRADE_ITEMNUMBER_NB;
        //    }
        //    else
        //    {
        //        avaMovementJournalContract.ItemNumber = Constants.DOWNGRADE_ITEMNUMBER_NR;
        //    }
        //    avaMovementJournalContract.Warehouse = batchdto.Location + "-" + batchdto.Area;
        //    avaInterfaceContract.MovementJournal = avaMovementJournalContract;
        //    if (avaInterfaceContract.BatchSequence > Constants.ONE)
        //    {
        //        AXPostingDTO axposting = new AXPostingDTO()
        //        {
        //            SerialNumber = batchdto.SerialNumber,
        //            BatchNumber = batchdto.BatchNumber,
        //            IsPostedToAX = true,
        //            ServiceName = CreateInvMovJournalFunctionidentifier.DBC2G,
        //            PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
        //            PostingType = "AVAMovementJournalContract",
        //            Area = batchdto.Area
        //        };

        //        using (AXAgentOne axAgent = new AXAgentOne())
        //        {
        //            result = axAgent.createInvMovJournal_New(avaInterfaceContract, avaMovementJournalContract);
        //        }

        //        return true;
        //    }
        //    else
        //        return true;
        //}

        /// <summary>
        /// Check Previous SPBC was created or not
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>bool</returns>
        public static bool IsPrevSPBCCreated(decimal serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@serialNumber", serialNumber));
            return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_IsPrevSPBCCreated", PrmList));
        }

        public static bool PostAXDataScanPTBatchCard(decimal serialNumber, string plantNo = null)
        {
            bool result = true;
            //BatchDTO batchSequence = GetBatchSequence(Convert.ToString(serialNumber), ""); //Azrul 2022-01-07: replace by SQL function
            if (IsPrevSPBCCreated(serialNumber)) // 30/11/2020,Max He, avoid re-create SPBC if previous process SPBC created(double PT) 
                return result;

            //if (batchSequence.BatchSequenceNo > Constants.ONE)
            //{
                BatchDTO batchdto = GetCompletePTDetails(serialNumber);
                AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
                AVATransferJournalContract avaTransferJournalContract = new AVATransferJournalContract();
                avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
                avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
                avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.FunctionID = CreateInvTransJournalFunctionidentifier.SPBC.ToString();
                avaInterfaceContract.PlantNo = batchdto.Location;
                //#AZRUL 18-6-2019: Assign PlantNo with the workstation PlantNo for duplicate Transfer.
                if (!string.IsNullOrEmpty(plantNo))
                    avaInterfaceContract.PlantNo = plantNo;
                //#AZRUL 18-6-2019
                avaTransferJournalContract.Brand = string.Empty;
                avaTransferJournalContract.Formula = string.Empty;
                avaTransferJournalContract.TransferJournalId = string.Empty;
                avaTransferJournalContract.Configuration = batchdto.Size;
                avaTransferJournalContract.ScanInDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                avaTransferJournalContract.ScanOutDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                avaTransferJournalContract.Quantity = batchdto.TotalPcs;
                avaTransferJournalContract.ItemNumber = batchdto.GloveType;
                avaTransferJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID); //batchdto.Location + "-" + batchdto.Area;
                avaTransferJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID); //batchdto.Area;
                avaInterfaceContract.TransferJournal = avaTransferJournalContract;
                avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                AXPostingDTO axposting = new AXPostingDTO()
                {
                    SerialNumber = batchdto.SerialNumber,
                    BatchNumber = batchdto.BatchNumber,
                    IsPostedToAX = true,
                    ServiceName = CreateInvTransJournalFunctionidentifier.SPBC,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = FSStagingBLL.DOTTransferJournalContract,
                    Area = batchdto.Area,
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
                };
                /**20/2/2018  Decouple AXAgent
                using (AXAgentOne axAgent = new AXAgentOne())
                {
                    string[] axResponse = axAgent.createRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                   StringSplitOptions.RemoveEmptyEntries);
                    
                    CaptureResult(axResponse, axposting);
                }**/
                var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertTransferJournal(avaInterfaceContract, identity));
                axposting.IsPostedInAX = true;
                LogAXPostingInfo(axposting);
                result = axposting.IsPostedInAX;
            //}
            return result;
        }

        /// <summary>
        /// Change Glove Type Process
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static bool PostAXDataPTChangeGloveType(decimal serialNumber, string OldGloveType)
        {
            BatchDTO batchdto = null;
            //BatchDTO batchSequence = null; //Azrul 2022-01-07: replace by SQL function
            DataSet objData = CommonBLL.GetResourceBySerialNo(serialNumber);
            AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
            foreach (DataTable table in objData.Tables)
            {
                foreach (DataRow dr in table.Rows)
                {
                    string resource = dr["Resource"].ToString();
                    batchdto = CommonBLL.GetCompleteBatchDetailsByResource(serialNumber, resource);
                    //batchSequence = GetBatchSequence(batchdto.SerialNumber, batchdto.Location); //Azrul 2022-01-07: replace by SQL function
                    AVARAFStgJournalContract avarafstgJournalContract = new AVARAFStgJournalContract();
                    avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
                    avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
                    avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                    avaInterfaceContract.FunctionID = CreateRAFJournalFunctionidentifier.CGLV.ToString();
                    avaInterfaceContract.PlantNo = batchdto.Location;
                    avarafstgJournalContract.SeqNo = batchdto.SeqNo;
                    avarafstgJournalContract.BatchOrder = batchdto.BatchOrder;
                    avarafstgJournalContract.BatchWeight = batchdto.BatchWeight;
                    avarafstgJournalContract.Shift = batchdto.ShiftName;
                    avarafstgJournalContract.Configuration = batchdto.Size;
                    avarafstgJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
                    avarafstgJournalContract.QCType = batchdto.QCType;
                    avarafstgJournalContract.CreatedDateTime = batchdto.BatchCarddate;
                    avarafstgJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                    avarafstgJournalContract.RAFGoodQty = FinalPackingBLL.GetBatchCapacity(Convert.ToDecimal(batchdto.SerialNumber)); //get the latest glove qty
                    avarafstgJournalContract.ItemNumber = OldGloveType;
                    avarafstgJournalContract.ChangedItemNumber = batchdto.GloveType;
                    avarafstgJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID);
                    avarafstgJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID);
                    avarafstgJournalContract.Resource = batchdto.Resource;
                    batchdto = CommonBLL.SetQAIBatchDetails(batchdto);// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
                    avaInterfaceContract.RAFStgJournal = avarafstgJournalContract;
                    avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                }
            }

            AXPostingDTO axposting = new AXPostingDTO()
            {
                SerialNumber = batchdto.SerialNumber,
                BatchNumber = batchdto.BatchNumber,
                IsPostedToAX = true,
                ServiceName = CreateRAFJournalFunctionidentifier.CGLV,
                PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                PostingType = FSStagingBLL.DOTRAFStgJournalContract,
                Area = batchdto.Area,
                IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                PlantNo = batchdto.Location // #AZRUL 20210909: Open batch flag for NGC1.5 
            };

            /**#AZ 20/2/2018  Decouple AXAgent
            using (AXAgentOne axAgent = new AXAgentOne())
            {
                string[] axResponse = axAgent.createRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                               StringSplitOptions.RemoveEmptyEntries);
                CaptureResult(axResponse, axposting);
            }
            **/
            var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);

            axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertRAFStaging(avaInterfaceContract, identity));
            axposting.IsPostedInAX = true;
            //if (batchdto.SeqNo == 1) //#AZRUL 29-10-2018 BUGS 1226: The next seqence number for 2 Tier HBC posting is wrong.
            LogAXPostingInfo(axposting);

            return axposting.IsPostedInAX;

            //bool result = false;
            //string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            //BatchDTO batchSequence = GetBatchSequence(Convert.ToString(serialNumber), plantNo);
            //if (batchSequence.BatchSequenceNo > Constants.ONE)
            //{
            //    BatchDTO batchdto = GetCompletePTDetails(serialNumber);
            //    AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
            //    BOMJournalContract avaBOMJournalContract = new BOMJournalContract();
            //    avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
            //    avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
            //    avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
            //    avaInterfaceContract.FunctionID = CreateBOMJournalFunctionidentifier.CGT.ToString();
            //    avaBOMJournalContract.BatchWeight = batchdto.BatchWeight;
            //    avaBOMJournalContract.Shift = batchdto.ShiftName;
            //    avaBOMJournalContract.Configuration = batchdto.Size;
            //    avaBOMJournalContract.Weightof10Pcs = batchdto.TenPcsWeight;
            //    avaBOMJournalContract.QCType = batchdto.QCType;
            //    avaBOMJournalContract.CreatedDateTime = batchdto.BatchCarddate;
            //    avaBOMJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
            //    avaBOMJournalContract.ItemNumber = CommonBLL.GetLatestArea(serialNumber) + "-" + batchdto.GloveType;
            //    avaBOMJournalContract.Warehouse = batchdto.Location + "-" + batchdto.Area;
            //    avaBOMJournalContract.Quantity = batchdto.TotalPcs;
            //    avaInterfaceContract.BOMJournal = avaBOMJournalContract;
            //    avaInterfaceContract.PlantNo = plantNo;
            //    avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

            //    AXPostingDTO axposting = new AXPostingDTO()
            //    {
            //        SerialNumber = batchdto.SerialNumber,
            //        BatchNumber = batchdto.BatchNumber,
            //        IsPostedToAX = true,
            //        ServiceName = CreateBOMJournalFunctionidentifier.CGT,
            //        PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
            //        PostingType = "BOMJournalContract",
            //        Area = CommonBLL.GetLatestArea(serialNumber),
            //        IsConsolidated = false // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
            //    };
            //    /** Decouple AxAgent
            //    using (AXAgentOne axAgent = new AXAgentOne())
            //    {
            //        result = axAgent.createBOMJournal_New(avaBOMJournalContract, avaInterfaceContract, NewGloveType);
            //    }
            //    result = axposting.IsPostedInAX;
            //    **/
            //}
            //return result;
        }

        public static bool PostAXDataFinalPacking(string internalLotNumber, string subModule)
        {
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            var avaInterfaceContract = new AvaInterfaceContract(); // MH 2/6/2018
            var avaFGJournalContract = new AVAFGJournalContract(); // Set the dto to global in order to insert staging in one time
            var avaMovementJournalContract = new AVAMovementJournalContract();
            var axposting = new AXPostingDTO();
            avaInterfaceContract.PlantNo = plantNo;

            if (subModule == Convert.ToString(Convert.ToInt16(Constants.SubModules.ScanBatchCardInnerOuterSurgical)))
            {
                FinalPackingAXpostingDTO finalPackingdto = GetFinalPackingBatchInfoDetails(internalLotNumber);

                //BatchDTO batchSequence1 = GetBatchSequence(Convert.ToString(finalPackingdto.RefSerialNumber1), plantNo); //Azrul 2022-01-07: replace by SQL function
                //BatchDTO batchSequence2 = GetBatchSequence(Convert.ToString(finalPackingdto.RefSerialNumber2), plantNo); //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.FunctionID = CreateFGRAFJournalFunctionidentifier.SUBC;
                avaFGJournalContract.Configuration = finalPackingdto.Size;
                avaFGJournalContract.Warehouse = WorkStationDTO.GetInstance().Location + Constants.FP_AREACODE;
                avaFGJournalContract.CustomerPO = finalPackingdto.orderNumber;
                avaFGJournalContract.CustomerReference = finalPackingdto.CustomerReferenceNumber;
                avaFGJournalContract.InnerLotNumber = finalPackingdto.Internallotnumber;
                avaFGJournalContract.OuterLotNumber = finalPackingdto.Outerlotno;
                avaFGJournalContract.CustomerLotNumber = finalPackingdto.customerLotNumber;
                avaFGJournalContract.ManufacturingDate = finalPackingdto.ManufacturingDate;
                avaFGJournalContract.ExpiryDate = finalPackingdto.ExpiryDate;
                avaFGJournalContract.Preshipment = IntegrationServices.NoYes.No;
                avaFGJournalContract.CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                avaFGJournalContract.PostingDateTime = finalPackingdto.Packdate;
                avaFGJournalContract.PreshipmentCases = Constants.ZERO;
                avaFGJournalContract.Quantity = finalPackingdto.Casespacked;
                avaFGJournalContract.Resource = WorkStationDTO.GetInstance().Location + Constants.FP_RESOURCE + WorkStationDataConfiguration.GetInstance().stationNumber;
                avaFGJournalContract.FGItemNumber = finalPackingdto.ItemNumber;
                avaFGJournalContract.SalesOrderNumber = finalPackingdto.Ponumber;
                avaInterfaceContract.ReferenceBatchNumber1 = finalPackingdto.RefSerialNumber1;
                avaInterfaceContract.ReferenceBatchNumber2 = finalPackingdto.RefSerialNumber2;
                avaFGJournalContract.RefConfiguration1 = finalPackingdto.RefSize1;
                avaFGJournalContract.RefConfiguration2 = finalPackingdto.RefSize2;

                avaInterfaceContract.ReferenceBatchSequence1 = 0; //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.ReferenceBatchSequence2 = 0; //Azrul 2022-01-07: replace by SQL function

                avaFGJournalContract.RefNumberOfPieces1 = finalPackingdto.BoxesPacked1 * (finalPackingdto.InnerBoxCapacity / 2);
                avaFGJournalContract.RefNumberOfPieces2 = finalPackingdto.BoxesPacked2 * (finalPackingdto.InnerBoxCapacity / 2);

                avaInterfaceContract.FGJournal = avaFGJournalContract;
                avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5 //Azrul 2022-01-07: replace by SQL function

                axposting = new AXPostingDTO()
                {
                    SerialNumber = finalPackingdto.RefSerialNumber1,
                    BatchNumber = finalPackingdto.RefBatchNumber1,
                    IsPostedToAX = true,
                    ServiceName = CreateFGRAFJournalFunctionidentifier.SUBC,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = FSStagingBLL.DOTFGJournalContract,
                    Area = Constants.FinalPacking_AREACODE,
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
                };

                /**#AZ 20/2/2018  Decouple AXAgent
                using (AXAgentOne axAgentsurgical = new AXAgentOne())
                {
                    string[] axResponse = axAgent.createFGRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                   StringSplitOptions.RemoveEmptyEntries);
                    CaptureResult(axResponse, axposting);
                }
                return axposting.IsPostedInAX;
                **/
            }
            else if (subModule == Convert.ToString(Convert.ToInt16(Constants.SubModules.ScanBatchCardInnerOuter2ndGrade)))
            {
                FinalPackingAXpostingDTO finalPackingdto = GetCompleteSecondGradeFinalPackingDetails(internalLotNumber);
                //BatchDTO batchSequence = GetBatchSequence(Convert.ToString(Constants.TWO), plantNo);
                avaInterfaceContract.BatchNumber = string.Empty;
                avaInterfaceContract.BatchCardNumber = string.Empty;
                avaInterfaceContract.FunctionID = CreateFGRAFJournalFunctionidentifier.SGBC.ToString();
                avaFGJournalContract.Configuration = finalPackingdto.Size;
                avaFGJournalContract.CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                avaFGJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                avaFGJournalContract.FGItemNumber = finalPackingdto.ItemNumber;
                avaFGJournalContract.Quantity = finalPackingdto.Casespacked;
                avaFGJournalContract.BatchOrder = finalPackingdto.BatchOrder;
                avaFGJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID); //WorkStationDTO.GetInstance().Location + Constants.FP_AREACODE;
                avaFGJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID);
                avaFGJournalContract.CustomerPO = finalPackingdto.orderNumber;
                avaFGJournalContract.CustomerReference = finalPackingdto.CustomerReferenceNumber;
                avaFGJournalContract.SalesOrderNumber = finalPackingdto.Ponumber;
                avaFGJournalContract.InnerLotNumber = finalPackingdto.Internallotnumber;
                avaFGJournalContract.OuterLotNumber = finalPackingdto.Outerlotno;
                avaFGJournalContract.CustomerLotNumber = finalPackingdto.customerLotNumber;
                avaFGJournalContract.PalletId = string.Empty;
                avaFGJournalContract.FGItemNumber = finalPackingdto.ItemNumber;
                avaFGJournalContract.ManufacturingDate = finalPackingdto.ManufacturingDate;
                avaFGJournalContract.ExpiryDate = finalPackingdto.ExpiryDate;
                avaFGJournalContract.Resource = finalPackingdto.Resource;
                avaInterfaceContract.FGJournal = avaFGJournalContract;
                avaInterfaceContract.PlantNo = WorkStationDTO.GetInstance().Location.ToString();
                avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                axposting = new AXPostingDTO()
                {
                    SerialNumber = Convert.ToString(Constants.TWO),
                    BatchNumber = Constants.FP_SecondGrade,
                    IsPostedToAX = true,
                    ServiceName = CreateFGRAFJournalFunctionidentifier.SGBC,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = FSStagingBLL.DOTFGJournalContract,
                    Area = Constants.FinalPacking_AREACODE,
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
                };
            }
            else
            {
                //return false;
            }

            var identity = 0;
            switch (axposting.PostingType)
            {
                case FSStagingBLL.DOTFGJournalContract:
                    identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                    axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertFGRAFStaging(avaInterfaceContract, identity));
                    axposting.IsPostedInAX = true;
                    break;
                case FSStagingBLL.DOTMovementJournalContract:
                    identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                    axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertMovementJournal(avaInterfaceContract, identity));
                    axposting.IsPostedInAX = true;
                    break;
                case FSStagingBLL.DOTTransferJournalContract:
                    //TODO
                    axposting.IsPostedInAX = false;
                    break;
                default:
                    axposting.IsPostedInAX = false;
                    break;
            }
            LogAXPostingInfo(axposting);
            return axposting.IsPostedInAX;
        }

        public static bool PostAXDataFinalPackingMTS(string internalLotNumber, string subModule)
        {
            return true;
            var avaInterfaceContract = new AvaInterfaceContract(); // MH 2/6/2018
            var avaFGJournalContract = new AVAFGJournalContract(); // Set the dto to global in order to insert staging in one time
            var avaMovementJournalContract = new AVAMovementJournalContract();
            var axposting = new AXPostingDTO();
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();

            if (subModule == Convert.ToString(Convert.ToInt16(Constants.SubModules.ScanBatchCardInnerOuter)))
            {
                FinalPackingAXpostingDTO finalPackingdto = GetCompleteFinalPackingDetailsMTS(internalLotNumber);
                //BatchDTO batchSequence = GetBatchSequence(finalPackingdto.SerialNumber, plantNo); //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.BatchNumber = finalPackingdto.SerialNumber;
                avaInterfaceContract.BatchCardNumber = finalPackingdto.BatchNumber;
                avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.FunctionID = CreateFGRAFJournalFunctionidentifier.SBC.ToString();
                avaFGJournalContract.IsWTS = true; //Flag to detect is from Make to Stock, without Sales Order
                avaFGJournalContract.BatchOrder = finalPackingdto.BatchOrder;
                avaFGJournalContract.Configuration = finalPackingdto.Size;
                avaFGJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID);
                avaFGJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID);
                avaFGJournalContract.Resource = finalPackingdto.Resource;

                //
                // For MTS, Not require for without sales order
                //
                //avaFGJournalContract.CustomerPO = finalPackingdto.orderNumber;
                //avaFGJournalContract.CustomerReference = finalPackingdto.CustomerReferenceNumber;
                //avaFGJournalContract.SalesOrderNumber = finalPackingdto.Ponumber;
                avaFGJournalContract.CustomerPO = "";
                avaFGJournalContract.CustomerReference = "";
                avaFGJournalContract.SalesOrderNumber = "";

                avaFGJournalContract.InnerLotNumber = finalPackingdto.Internallotnumber;
                avaFGJournalContract.OuterLotNumber = finalPackingdto.Outerlotno;
                avaFGJournalContract.CustomerLotNumber = finalPackingdto.customerLotNumber;
                avaFGJournalContract.ManufacturingDate = finalPackingdto.ManufacturingDate;
                avaFGJournalContract.ExpiryDate = finalPackingdto.ExpiryDate;
                avaFGJournalContract.PalletId = finalPackingdto.PalletId;
                if (GetPreshipmentCasestopack(finalPackingdto.Ponumber, finalPackingdto.ItemNumber, finalPackingdto.Size) == Constants.ZERO)
                {
                    avaFGJournalContract.Preshipment = IntegrationServices.NoYes.Yes;
                    avaFGJournalContract.PreshipmentCases = FinalPackingBLL.GetPreshipmentCaseCount(finalPackingdto.Ponumber, finalPackingdto.ItemNumber, finalPackingdto.Size);
                }
                else
                {
                    avaFGJournalContract.Preshipment = IntegrationServices.NoYes.No;
                    avaFGJournalContract.PreshipmentCases = Constants.ZERO;
                }

                avaFGJournalContract.CreatedDateTime = finalPackingdto.BatchCardDate;
                avaFGJournalContract.PostingDateTime = finalPackingdto.Packdate;
                avaFGJournalContract.Quantity = finalPackingdto.Casespacked;
                //  avaFGJournalContract.Resource = finalPackingdto.Location + "-" + batchdto.Line + "-" + batchdto.Size;
                avaFGJournalContract.FGItemNumber = finalPackingdto.ItemNumber;
                avaInterfaceContract.FGJournal = avaFGJournalContract;
                avaInterfaceContract.PlantNo = plantNo;
                avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                axposting = new AXPostingDTO()
                {
                    SerialNumber = finalPackingdto.SerialNumber,
                    BatchNumber = finalPackingdto.BatchNumber,
                    IsPostedToAX = true,
                    ServiceName = CreateFGRAFJournalFunctionidentifier.SBC,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = FSStagingBLL.DOTFGJournalContract,
                    Area = Constants.FinalPacking_AREACODE,
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
                };

                //TODO: to test and find rootcause for TOTT# 498 beloe code -> Start
                if (avaInterfaceContract.FGJournal.Configuration.Length > 4)
                {
                    FloorSystemException floorException = new FloorSystemException("Serial number is sent inplace of configuration for" + avaInterfaceContract.BatchNumber + "FP Table Size: " + finalPackingdto.Size, "TOTT 498", new Exception("TOTT 498"), true);
                    CommonBLL.LogExceptionToDB(floorException, "Final Packing TOTT 498 ", "Final Packing TOTT 498 ", "Final Packing TOTT 498 ", "Final Packing TOTT 498 ");
                }
                //END
                /** #AZ 20/2/2018  Decouple AXAgent
                using (AXAgentOne axAgent = new AXAgentOne())
                {
                    // TODO: to test and find rootcause for TOTT# 498 beloe code -> Start
                    if (avaInterfaceContract.FGJournal.Configuration.Length > 4)
                    {
                        FloorSystemException floorException = new FloorSystemException("Serial number is sent inplace of configuration for" + avaInterfaceContract.BatchNumber + "FP Table Size: " + finalPackingdto.Size, "TOTT 498", new Exception("TOTT 498"), true);
                        CommonBLL.LogExceptionToDB(floorException, "Final Packing TOTT 498 ", "Final Packing TOTT 498 ", "Final Packing TOTT 498 ", "Final Packing TOTT 498 ");
                    }
                    //END
                    string[] axResponse = axAgent.createFGRAFJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                   StringSplitOptions.RemoveEmptyEntries);
                    CaptureResult(axResponse, axposting);
                }
                **/
            }

            var identity = 0;
            switch (axposting.PostingType)
            {
                case FSStagingBLL.DOTFGJournalContract:
                    identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                    axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertFGRAFStaging(avaInterfaceContract, identity));
                    axposting.IsPostedInAX = true;
                    break;
                case FSStagingBLL.DOTMovementJournalContract:
                    identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                    axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertMovementJournal(avaInterfaceContract, identity));
                    axposting.IsPostedInAX = true;
                    break;
                case FSStagingBLL.DOTTransferJournalContract:
                    //TODO
                    axposting.IsPostedInAX = false;
                    break;
                default:
                    axposting.IsPostedInAX = false;
                    break;
            }
            LogAXPostingInfo(axposting);
            return axposting.IsPostedInAX;
        }

        public static bool PostAXDataFinalPackingCBCI(FPChangeBatchCardV2DTO FPChgBatchDTO, string subModule) //#AzmanCBCI
        {
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            if (subModule == Convert.ToString(Convert.ToInt16(Constants.SubModules.ChangeBatchCardInner)))
            {
                AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
                DOT_PickingListContract pickingList = new DOT_PickingListContract();
                FinalPackingAXpostingDTO finalPackingdto = GetChangeBatchCardDetails(FPChgBatchDTO.InternalLotNumber);

                //BatchDTO batchSequence = GetBatchSequence(finalPackingdto.oldSerialNumber, plantNo); //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.FunctionID = CreatePickingListFunctionidentifier.CBCI.ToString();
                avaInterfaceContract.BatchNumber = finalPackingdto.oldSerialNumber;
                avaInterfaceContract.BatchCardNumber = finalPackingdto.BatchNumber;
                avaInterfaceContract.PlantNo = plantNo;
                avaInterfaceContract.ReferenceBatchNumber1 = Convert.ToString(FPChgBatchDTO.NewSerialNumber1);
                avaInterfaceContract.ReferenceBatchSequence1 = 0; //GetBatchSequence(Convert.ToString(FPChgBatchDTO.NewSerialNumber1), plantNo).BatchSequenceNo; //Azrul 2022-01-07: replace by SQL function

                pickingList.Configuration = finalPackingdto.Size;
                pickingList.CreatedDateTime = finalPackingdto.BatchCardDate;
                pickingList.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                //pickingList.Warehouse = WorkStationDTO.GetInstance().Location;
                pickingList.SalesOrderNumber = finalPackingdto.Ponumber;
                pickingList.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID);
                pickingList.ItemNumber = finalPackingdto.ItemNumber;
                pickingList.ReferenceItemNumber = FPChgBatchDTO.ItemNumber; //FG item no
                pickingList.RefBatchSequence1 = avaInterfaceContract.ReferenceBatchSequence1;
                pickingList.RefNumberOfPieces1 = FPChgBatchDTO.NewSerialNumber1QtyUse;
                pickingList.RAFGoodQty = FPChgBatchDTO.NewSerialNumber1QtyUse;
                pickingList.PSIReworkOrderNo = FPChgBatchDTO.PSIReworkOrderNo;
                pickingList.InternalReferenceNumber = FPChgBatchDTO.InternalLotNumber;

                if (!string.IsNullOrEmpty(FPChgBatchDTO.NewSerialNumber2.ToString()) && (FPChgBatchDTO.NewSerialNumber2.ToString() != "0"))
                {
                    avaInterfaceContract.ReferenceBatchNumber2 = Convert.ToString(FPChgBatchDTO.NewSerialNumber2);
                    avaInterfaceContract.ReferenceBatchSequence2 = 0; //GetBatchSequence(Convert.ToString(FPChgBatchDTO.NewSerialNumber2), plantNo).BatchSequenceNo; //Azrul 2022-01-07: replace by SQL function
                    pickingList.RefNumberOfPieces2 = FPChgBatchDTO.NewSerialNumber2QtyUse;
                    pickingList.RAFGoodQty = pickingList.RAFGoodQty + FPChgBatchDTO.NewSerialNumber2QtyUse;
                }

                if (!string.IsNullOrEmpty(FPChgBatchDTO.NewSerialNumber3.ToString()) && (FPChgBatchDTO.NewSerialNumber3.ToString() != "0"))
                {
                    avaInterfaceContract.ReferenceBatchNumber3 = Convert.ToString(FPChgBatchDTO.NewSerialNumber3);
                    avaInterfaceContract.ReferenceBatchSequence3 = 0; //GetBatchSequence(Convert.ToString(FPChgBatchDTO.NewSerialNumber3), plantNo).BatchSequenceNo; //Azrul 2022-01-07: replace by SQL function
                    pickingList.RefNumberOfPieces3 = FPChgBatchDTO.NewSerialNumber3QtyUse;
                    pickingList.RAFGoodQty = pickingList.RAFGoodQty + FPChgBatchDTO.NewSerialNumber3QtyUse;
                }
                avaInterfaceContract.PickingList = pickingList;
                avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                AXPostingDTO axposting = new AXPostingDTO() //for FloorSystem logging
                {
                    SerialNumber = finalPackingdto.oldSerialNumber,
                    BatchNumber = finalPackingdto.BatchNumber,
                    IsPostedToAX = true,
                    ServiceName = CreatePickingListFunctionidentifier.CBCI,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = FSStagingBLL.DOTPickingListContract,
                    Area = Constants.FinalPacking_AREACODE,
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
                };

                /** #AZ 8/6/2018 Decouple AXAgent
                using (AXAgentOne axAgent = new AXAgentOne())
                {
                    string[] axResponse = axAgent.createInvMovJournalCBCI(avaInterfaceContract).Split(new string[] { "||" },
                                                   StringSplitOptions.RemoveEmptyEntries);
                    if (axResponse.Length == Constants.ONE)
                    {
                        axposting.IsPostedInAX = true;
                        axposting.TransactionID = axResponse[0];
                    }
                    else
                    {
                        axposting.IsPostedInAX = false;
                        axposting.TransactionID = Convert.ToString(Constants.MINUSONE);
                        axposting.ExceptionCode = axResponse[1];
                    }
                    LogAXPostingInfo(axposting);
                }
                **/
                var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
                axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertPickingList(avaInterfaceContract, identity));
                axposting.IsPostedInAX = true;
                LogAXPostingInfo(axposting);
                return axposting.IsPostedInAX;
            }
            else
                return false;
        }

        public static bool PostAXGloveInventoryScanIn(decimal serialNumber)
        {
            bool result = true;
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            //BatchDTO batchSequence = GetBatchSequence(Convert.ToString(serialNumber), WorkStationDTO.GetInstance().Location.ToString());
            if (!CommonBLL.ValidateAXPosting(serialNumber, CreateInvTransJournalFunctionidentifier.SCIN.ToString(), WorkStationDTO.GetInstance().Area))
            {
                BatchDTO batchdto = GetCompleteGloveInventoryDetails(serialNumber);
                AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
                AVATransferJournalContract avaTransferJournalContract = new AVATransferJournalContract();
                avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
                avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
                avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.FunctionID = CreateInvTransJournalFunctionidentifier.SCIN.ToString();
                avaTransferJournalContract.Location = batchdto.Location;
                avaTransferJournalContract.Configuration = batchdto.Size;

                //TOTT#315
                avaTransferJournalContract.Quantity = Math.Floor(PostTreatmentBLL.GetPTLatestBatchWeight(serialNumber) * Constants.THOUSAND * Constants.TEN / PostTreatmentBLL.GetPTLatestTenPcsWeight(serialNumber));

                avaTransferJournalContract.ScanInDateTime = batchdto.ScanInDate;
                avaTransferJournalContract.Warehouse = WorkStationDTO.GetInstance().LocationAreaCode;
                avaInterfaceContract.TransferJournal = avaTransferJournalContract;
                AXPostingDTO axposting = new AXPostingDTO() { SerialNumber = batchdto.SerialNumber, BatchNumber = batchdto.BatchNumber, IsPostedToAX = true, ServiceName = CreateInvTransJournalFunctionidentifier.SCIN, PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(), PostingType = "AVATransferJournalContract", Area = WorkStationDTO.GetInstance().Area };
                //using (AXAgentOne axAgent = new AXAgentOne())
                //{
                /**#AZ 20/2/2018  Decouple AXAgent
                string[] axResponse = axAgent.createInvTransJournal(avaInterfaceContract).Split(new string[] { "||" },
                                               StringSplitOptions.RemoveEmptyEntries);
                CaptureResult(axResponse, axposting);
                **/
                //}
                result = axposting.IsPostedInAX;
            }
            return result;
        }

        public static bool PostAXGloveInventoryScanOut(decimal serialNumber)
        {
            bool result = true;
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            //BatchDTO batchSequence = GetBatchSequence(Convert.ToString(serialNumber), plantNo);
            if (!CommonBLL.ValidateAXPosting(serialNumber, CreateInvTransJournalFunctionidentifier.SCOT.ToString(), WorkStationDTO.GetInstance().Area))
            {
                BatchDTO batchdto = GetCompleteGloveInventoryDetails(serialNumber);
                BatchDTO obj = GetNextProcessArea(batchdto.NextProcess);
                AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
                AVATransferJournalContract avaTransferJournalContract = new AVATransferJournalContract();
                avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
                avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
                avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
                avaInterfaceContract.FunctionID = CreateInvTransJournalFunctionidentifier.SCOT.ToString();
                avaTransferJournalContract.ItemNumber = CommonBLL.GetLatestArea(serialNumber) + "-" + batchdto.GloveType;
                avaTransferJournalContract.Configuration = batchdto.Size;

                //TOTT#315              
                avaTransferJournalContract.Quantity = Math.Floor(PostTreatmentBLL.GetPTLatestBatchWeight(serialNumber) * Constants.THOUSAND * Constants.TEN / PostTreatmentBLL.GetPTLatestTenPcsWeight(serialNumber));

                avaTransferJournalContract.ScanOutDateTime = batchdto.ScanOutDate;
                avaTransferJournalContract.ToWarehouse = WorkStationDTO.GetInstance().Location + "-" + batchdto.NextProcess;
                avaTransferJournalContract.Warehouse = WorkStationDTO.GetInstance().LocationAreaCode;
                avaInterfaceContract.TransferJournal = avaTransferJournalContract;
                avaInterfaceContract.PlantNo = plantNo;
                avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

                AXPostingDTO axposting = new AXPostingDTO()
                {
                    SerialNumber = batchdto.SerialNumber,
                    BatchNumber = batchdto.BatchNumber,
                    IsPostedToAX = true,
                    ServiceName = CreateInvTransJournalFunctionidentifier.SCOT,
                    PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                    PostingType = "AVATransferJournalContract",
                    Area = WorkStationDTO.GetInstance().Area,
                    IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                    PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5  
                };
                //using (AXAgentOne axAgent = new AXAgentOne())
                //{
                /**#AZ 20/2/2018  Decouple AXAgent
                string[] axResponse = axAgent.createInvTransJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                StringSplitOptions.RemoveEmptyEntries);
                CaptureResult(axResponse, axposting);
                **/
                //}
                result = axposting.IsPostedInAX;
            }
            return result;
        }

        /// <summary>
        /// Post data to AX for online rejection integration, add on resource group( plant id + line id) 
        /// added on 10th Oct 2016 at 5:25 by Max He, MH#1.n
        /// </summary>
        /// <param name="batchdto"></param>
        /// <returns></returns>
        public static bool PostAXDataOnlineRejectGloves(BatchDTO batchdto)
        {
            string plantNo = WorkStationDTO.GetInstance().Location.ToString();
            //BatchDTO batchSequence = GetBatchSequence(batchdto.SerialNumber, plantNo); //Azrul 2022-01-07: replace by SQL function
            AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
            AVAMovementJournalContract avaMovementJournalContract = new AVAMovementJournalContract();
            avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
            avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
            avaInterfaceContract.BatchSequence = 0;  //Azrul 2022-01-07: replace by SQL function
            avaInterfaceContract.FunctionID = CreateInvMovJournalFunctionidentifier.OREJ.ToString();
            avaInterfaceContract.PlantNo = plantNo;
            avaMovementJournalContract.ResourceGroup = batchdto.ResourceGroup;
            avaMovementJournalContract.CreatedDateTime = batchdto.BatchCarddate;
            avaMovementJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
            // added on 10th Oct 2016 at 5:25 by Max He, MH#1.n
            avaMovementJournalContract.Quantity = batchdto.BatchWeight;// Online Rejection Quantity(KG)
            avaMovementJournalContract.Shift = batchdto.ShiftName;
            avaMovementJournalContract.Warehouse = FSStagingBLL.GetWarehouse(avaInterfaceContract.FunctionID);
            avaMovementJournalContract.Location = FSStagingBLL.GetLocation(avaInterfaceContract.FunctionID);
            avaInterfaceContract.MovementJournal = avaMovementJournalContract;
            avaInterfaceContract.IsConsolidated = false; // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function

            AXPostingDTO axposting = new AXPostingDTO()
            {
                SerialNumber = batchdto.SerialNumber,
                BatchNumber = batchdto.BatchNumber,
                IsPostedToAX = true,
                ServiceName = CreateInvMovJournalFunctionidentifier.OREJ,
                PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer(),
                PostingType = FSStagingBLL.DOTMovementJournalContract,
                Area = batchdto.Area,
                IsConsolidated = false, // #AZRUL 20210909: Open batch flag for NGC1.5  //Azrul 2022-01-07: replace by SQL function
                PlantNo = plantNo // #AZRUL 20210909: Open batch flag for NGC1.5 
            };

            /**#AZ 15/5/2018  Change OREJ saved into staging table in FS db
            using (AXAgentOne axAgent = new AXAgentOne())
            {
                string[] axResponse = axAgent.createInvMovJournal(avaInterfaceContract).Split(new string[] { "||" },
                                                StringSplitOptions.RemoveEmptyEntries);
                CaptureResult(axResponse, axposting);
            }
            **/
            var identity = FSStagingBLL.InsertParentStaging(avaInterfaceContract);
            axposting.TransactionID = Convert.ToString(FSStagingBLL.InsertMovementJournal(avaInterfaceContract, identity));
            axposting.IsPostedInAX = true;
            LogAXPostingInfo(axposting);
            return axposting.IsPostedInAX;
        }

        public static int GetPreshipmentCasestopack(string poNumber, string itemNumber, string size)
        {
            return FinalPackingBLL.GetPreshipmentCasesQuantitytoPack(poNumber, itemNumber, size);
        }

        private static void CaptureResult(string[] axResponse, AXPostingDTO axposting)
        {
            if (axResponse.Length == Constants.ONE)
            {
                axposting.IsPostedInAX = true;
                axposting.TransactionID = axResponse[0];
            }
            else
            {
                axposting.IsPostedInAX = false;
                axposting.TransactionID = Convert.ToString(Constants.MINUSONE);
                axposting.ExceptionCode = axResponse[1];
            }
            LogAXPostingInfo(axposting);
        }

        public static void LogAXPostingInfo(AXPostingDTO axposting)
        {
            int rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ServiceName", axposting.ServiceName));
            PrmList.Add(new FloorSqlParameter("@PostingType", axposting.PostingType));
            PrmList.Add(new FloorSqlParameter("@PostedDate", axposting.PostedDate));
            PrmList.Add(new FloorSqlParameter("@BatchNumber", axposting.BatchNumber));
            PrmList.Add(new FloorSqlParameter("@SerialNumber", axposting.SerialNumber));
            PrmList.Add(new FloorSqlParameter("@IsPostedToAX", axposting.IsPostedToAX));
            PrmList.Add(new FloorSqlParameter("@IsPostedInAX", axposting.IsPostedInAX));
            PrmList.Add(new FloorSqlParameter("@Sequence", axposting.Sequence));
            PrmList.Add(new FloorSqlParameter("@ExceptionCode", axposting.ExceptionCode));
            PrmList.Add(new FloorSqlParameter("@TransactionID", axposting.TransactionID));
            PrmList.Add(new FloorSqlParameter("@Area", axposting.Area));
            PrmList.Add(new FloorSqlParameter("@IsConsolidated", axposting.IsConsolidated)); // #AZRUL 20210909: Open batch flag for NGC1.5  
            PrmList.Add(new FloorSqlParameter("@PlantNo", axposting.PlantNo)); // #AZRUL 20210909: Open batch flag for NGC1.5  
            rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_SAVE_AXPOSTINGLOG", PrmList);
            // Throwing AX posting related exceptions to UI Layer.
            if (rowsaffected == 0)
            {
                throw new FloorSystemException("Failed to Insert AxPostingLog!", Constants.AXSERVICEERROR, new Exception("Failed to Insert AxPostingLog!"), true);
            }
            //no more post to AX
            //if (!axposting.IsPostedInAX)
            //{
            //    throw new FloorSystemException(axposting.ExceptionCode.Trim(), Constants.AXSERVICEERROR, new Exception(axposting.ExceptionCode.Trim()), true);
            //}
        }

        /// <summary>
        /// To Get Complete QC Yield & Packing details
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static BatchDTO GetCompleteQCYPDetails(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_CompleteQCYPDetails", lstParameters))
            {
                try
                {
                    if (dtBatch.Rows.Count == 0)
                    {
                        //Max, block when QCQI posting stage mssing QC scan in and scan out
                        string errorMessage = "Call USP_GET_CompleteQCYPDetails fail," + Messages.QCQI_INCOMPLETE;
                        throw new Exception(errorMessage);
                    }
                    objBatch = (from DataRow row in dtBatch.Rows
                                select new BatchDTO
                                {
                                    SerialNumber = Convert.ToString(serialNo),
                                    BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                    TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPcsWeight"),
                                    Size = FloorDBAccess.GetString(row, "Size"),
                                    BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                    QCType = FloorDBAccess.GetString(row, "QCType"),
                                    ShiftName = FloorDBAccess.GetString(row, "Name"),
                                    TotalPcs = FloorDBAccess.GetValue<Int32>(row, "TotalPcs"),
                                    GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                    Area = FloorDBAccess.GetString(row, "Area"),
                                    Location = FloorDBAccess.GetString(row, "LocationName"),
                                    Line = FloorDBAccess.GetString(row, "LineNumber"),
                                    BatchCarddate = FloorDBAccess.GetValue<DateTime>(row, "BatchCarddate"),
                                    // #MH 10/11/2016 1.n FDD:HLTG-REM-003                                    
                                    RAFGoodQtyLoose = FloorDBAccess.GetValue<int>(row, "LooseQty"),
                                    SecGradeQty = FloorDBAccess.GetValue<int>(row, "SecondGradeQty"),
                                    RejectedQty = FloorDBAccess.GetValue<int>(row, "RejectionQty"),
                                    RejectedSampleQty = FloorDBAccess.GetValue<int>(row, "RejectedSample"),
                                    CalculatedLooseQty = FloorDBAccess.GetValue<int>(row, "CalculatedLooseQty"),
                                    BatchType = FloorDBAccess.GetString(row, "BatchType").Replace(" ", ""),
                                    // #MH 10/11/2016 1.n FDD:HLTG-REM-003
                                    CalculatedRAFWTSample = FloorDBAccess.GetValue<int>(row, "RAFWTSample")  // #AZRUL 15/6/2018: BUG_1137 - Accumulate WT sample quantity after QCQI more than 1 times.
                                }).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return objBatch;
        }

        /// <summary>
        /// To Get Complete Downgrade Batch details
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static BatchDTO GetCompleteDowngradeBatchDetails(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_CompleteDowngradeBatchDetails", lstParameters))
            {
                try
                {
                    objBatch = (from DataRow row in dtBatch.Rows
                                select new BatchDTO
                                {
                                    SerialNumber = Convert.ToString(serialNo),
                                    GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                    Area = FloorDBAccess.GetString(row, "Area"),
                                    Size = FloorDBAccess.GetString(row, "Size"),
                                    TotalPcs = FloorDBAccess.GetValue<Int32>(row, "TotalPCs"),
                                    Location = FloorDBAccess.GetString(row, "locationname"),
                                    BatchNumber = FloorDBAccess.GetString(row, "BatchNumber")
                                }).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return objBatch;
        }

        /// <summary>
        /// To Get Complete Post Treatment details
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static BatchDTO GetCompletePTDetails(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_CompletePTDetails", lstParameters))
            {
                try
                {
                    objBatch = (from DataRow row in dtBatch.Rows
                                select new BatchDTO
                                {
                                    SerialNumber = Convert.ToString(serialNo),
                                    BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                    TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPCsWeight"),
                                    Size = FloorDBAccess.GetString(row, "Size"),
                                    BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                    QCType = FloorDBAccess.GetString(row, "QCType"),
                                    ShiftName = FloorDBAccess.GetString(row, "Name"),
                                    GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                    Area = FloorDBAccess.GetString(row, "Area"),
                                    Location = FloorDBAccess.GetString(row, "LocationName"),
                                    BatchCarddate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate"),
                                    TotalPcs = FloorDBAccess.GetValue<Int32>(row, "TotalPcs"),
                                    Line = FloorDBAccess.GetString(row, "LineId"),
                                    BatchType = FloorDBAccess.GetString(row, "BatchType"),
                                    DeliveryDate = FloorDBAccess.GetValue<DateTime>(row, "DeliveryDate"),
                                    Pool = FloorDBAccess.GetString(row, "Pool"), //#MH 18/06/2018 Rework additional info.
                                    RouteCategory = FloorDBAccess.GetString(row, "RouteCategory") //#MH 18/06/2018 Rework additional info.
                                }).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return objBatch;
        }

        /// <summary>
        /// To Get Complete Customer Reject Gloves details
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static BatchDTO GetCompleteCustomerRejectGlovesDetails(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_CompleteRejectedGlovesDetails", lstParameters))
            {
                try
                {
                    objBatch = (from DataRow row in dtBatch.Rows
                                select new BatchDTO
                                {
                                    SerialNumber = Convert.ToString(serialNo),
                                    BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                    GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                    Location = FloorDBAccess.GetString(row, "LocationName"),
                                    Area = FloorDBAccess.GetString(row, "Area"),
                                    TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPCsWeight"),
                                    Size = FloorDBAccess.GetString(row, "Size"),
                                    BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                    QCType = FloorDBAccess.GetString(row, "QCType"),
                                    ShiftName = FloorDBAccess.GetString(row, "ShiftName"),
                                    BatchCarddate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate"),
                                    TotalPcs = FloorDBAccess.GetValue<Int32>(row, "TotalPCs"),
                                    SubModule = FloorDBAccess.GetString(row, "SubModuleID")
                                }).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return objBatch;
        }

        /// <summary>
        /// To Get Complete Glove Inventory details
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static BatchDTO GetCompleteGloveInventoryDetails(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_CompleteGloveInventoryDetails", lstParameters))
            {
                try
                {
                    objBatch = (from DataRow row in dtBatch.Rows
                                select new BatchDTO
                                {
                                    SerialNumber = Convert.ToString(serialNo),
                                    BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                    Area = FloorDBAccess.GetString(row, "Area"),
                                    Location = FloorDBAccess.GetString(row, "LocationName"),
                                    Size = FloorDBAccess.GetString(row, "Size"),
                                    ScanInDate = FloorDBAccess.GetValue<DateTime>(row, "ScanInDate"),
                                    TotalPcs = FloorDBAccess.GetValue<Int32>(row, "TotalPcs"),
                                    GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                    ScanOutDate = FloorDBAccess.GetValue<DateTime>(row, "ScanOutDate"),
                                    NextProcess = FloorDBAccess.GetString(row, "NextProcess")

                                }).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return objBatch;
        }

        /// <summary>
        /// Get Posting Stage
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>string</returns>
        public static string GetPostingStage(decimal serialNumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_PostingStage", lstParameters));
        }

        /// <summary>
        /// Get Area for Next Process
        /// </summary>
        /// <param name="gloveType"></param>
        /// <returns></returns>
        public static BatchDTO GetNextProcessArea(string nextProcess)
        {
            BatchDTO objG = null;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@nextProcess", nextProcess));
            using (DataTable dt = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_NextProcessArea", PrmList))
            {
                objG = (from DataRow row in dt.Rows
                        select new BatchDTO
                        {
                            NextProcess = FloorDBAccess.GetString(row, "Area")
                        }).SingleOrDefault();
            }
            return objG;
        }

        /// <summary>
        /// Get Batch Status
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>string</returns>
        public static string GetBatchStatus(decimal serialNumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_BatchStatusForSerialNo", lstParameters));
        }

        /// <summary>
        /// Get Sub Module Id for Serial No
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>int</returns>
        public static int GetSubModuleIdForSerialNo(decimal serialNumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_SubModuleIdForSerialNo", lstParameters));
        }

        /// <summary>
        /// Get QI Test Result
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>string</returns>
        public static string GetQITestResult(int qaiId)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@QaiId", qaiId));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_QITestResult", lstParameters));
        }

        /// <summary>
        /// Get Packing Into for Serial Number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>string</returns>
        public static string GetPackingInto(decimal serialNumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_QCPackingInto", lstParameters));
        }

        /// <summary>
        /// Get Scan Out Time for Serial Number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>dateTime</returns>
        public static DateTime GetScanOutTime(decimal serialNumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToDateTime(FloorDBAccess.ExecuteScalar("USP_GET_QCScanOutTime", lstParameters));
        }

        /// <summary>
        /// To Get Complete Final Packing details
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static FinalPackingAXpostingDTO GetCompleteFinalPackingDetails(string internallotnumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@internalLotnumber", internallotnumber));
            FinalPackingAXpostingDTO objFPAXPostData = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_FP_Get_ScanBatchCardInnerOuterforPosting", lstParameters))
            {
                try
                {
                    objFPAXPostData = (from DataRow row in dtBatch.Rows
                                       select new FinalPackingAXpostingDTO
                                       {
                                           SerialNumber = FloorDBAccess.GetString(row, "SerialNumber"),
                                           BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                           BatchOrder = FloorDBAccess.GetString(row, "BatchOrder"),
                                           Ponumber = FloorDBAccess.GetString(row, "PONumber"),
                                           ItemNumber = FloorDBAccess.GetString(row, "itemnumber"),
                                           Size = FloorDBAccess.GetString(row, "customersize"),
                                           orderNumber = FloorDBAccess.GetString(row, "ordernumber"),
                                           Internallotnumber = FloorDBAccess.GetString(row, "internalLotnumber"),
                                           Outerlotno = FloorDBAccess.GetString(row, "OuterLotNo"),
                                           Casespacked = FloorDBAccess.GetValue<int>(row, "CasesPacked"),
                                           Preshipmentcases = FloorDBAccess.GetValue<int>(row, "PreshipmentCasesPacked"),
                                           customerLotNumber = FloorDBAccess.GetString(row, "CustomerLotNumber"),
                                           CustomerReferenceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber"),
                                           ManufacturingDate = FloorDBAccess.GetValue<DateTime>(row, "manufacturingdate"),
                                           ExpiryDate = FloorDBAccess.GetValue<DateTime>(row, "ExpiryDate"),
                                           BatchCardDate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate"),
                                           Packdate = FloorDBAccess.GetValue<DateTime>(row, "PackDate"),
                                           PalletId = FloorDBAccess.GetString(row, "PalletId"),
                                           Resource = FloorDBAccess.GetString(row, "Resource")
                                       }).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
                return objFPAXPostData;
            }
        }

        public static FinalPackingAXpostingDTO GetCompleteFinalPackingDetailsMTS(string internallotnumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@internalLotnumber", internallotnumber));
            FinalPackingAXpostingDTO objFPAXPostData = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_FP_Get_ScanBatchCardInnerOuterforPostingMTS", lstParameters))
            {
                try
                {
                    objFPAXPostData = (from DataRow row in dtBatch.Rows
                                       select new FinalPackingAXpostingDTO
                                       {
                                           SerialNumber = FloorDBAccess.GetString(row, "SerialNumber"),
                                           BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                           BatchOrder = FloorDBAccess.GetString(row, "BatchOrder"),
                                           Ponumber = FloorDBAccess.GetString(row, "PONumber"),
                                           ItemNumber = FloorDBAccess.GetString(row, "itemnumber"),
                                           Size = FloorDBAccess.GetString(row, "customersize"),
                                           orderNumber = FloorDBAccess.GetString(row, "ordernumber"),
                                           Internallotnumber = FloorDBAccess.GetString(row, "internalLotnumber"),
                                           Outerlotno = FloorDBAccess.GetString(row, "OuterLotNo"),
                                           Casespacked = FloorDBAccess.GetValue<int>(row, "CasesPacked"),
                                           Preshipmentcases = FloorDBAccess.GetValue<int>(row, "PreshipmentCasesPacked"),
                                           customerLotNumber = FloorDBAccess.GetString(row, "CustomerLotNumber"),
                                           CustomerReferenceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber"),
                                           ManufacturingDate = FloorDBAccess.GetValue<DateTime>(row, "manufacturingdate"),
                                           ExpiryDate = FloorDBAccess.GetValue<DateTime>(row, "ExpiryDate"),
                                           BatchCardDate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate"),
                                           Packdate = FloorDBAccess.GetValue<DateTime>(row, "PackDate"),
                                           PalletId = FloorDBAccess.GetString(row, "PalletId"),
                                           Resource = FloorDBAccess.GetString(row, "Resource")
                                       }).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
                return objFPAXPostData;
            }
        }

        //USP_FP_Get_SecondGradeforPosting
        public static FinalPackingAXpostingDTO GetCompleteSecondGradeFinalPackingDetails(string internallotnumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@internalLotnumber", internallotnumber));
            FinalPackingAXpostingDTO objFPAXPostData = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_FP_Get_SecondGradeforPosting", lstParameters))
            {
                try
                {
                    objFPAXPostData = (from DataRow row in dtBatch.Rows
                                       select new FinalPackingAXpostingDTO
                                       {
                                           Ponumber = FloorDBAccess.GetString(row, "PONumber"),
                                           ItemNumber = FloorDBAccess.GetString(row, "itemnumber"),
                                           Size = FloorDBAccess.GetString(row, "customersize"),
                                           Internallotnumber = internallotnumber,
                                           Casespacked = FloorDBAccess.GetValue<int>(row, "CasesPacked"),
                                           Packdate = FloorDBAccess.GetValue<DateTime>(row, "PackDate"),
                                           BatchOrder = FloorDBAccess.GetString(row, "BatchOrder"),
                                           Resource = FloorDBAccess.GetString(row, "Resource"),
                                           Outerlotno = FloorDBAccess.GetString(row, "Outerlotno"),
                                           CustomerReferenceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber"),
                                           customerLotNumber = FloorDBAccess.GetString(row, "customerLotNumber"),
                                           orderNumber = FloorDBAccess.GetString(row, "orderNumber"),
                                           ManufacturingDate = FloorDBAccess.GetValue<DateTime>(row, "manufacturingdate"),
                                           ExpiryDate = FloorDBAccess.GetValue<DateTime>(row, "ExpiryDate")
                                       }).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
                return objFPAXPostData;
            }
        }

        public static FinalPackingAXpostingDTO GetChangeBatchCardDetails(string internallotnumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@internalLotnumber", internallotnumber));
            FinalPackingAXpostingDTO objFPAXPostData = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_FP_Get_ChangeBatchCardDetails", lstParameters))
            {
                try
                {
                    objFPAXPostData = (from DataRow row in dtBatch.Rows
                                       select new FinalPackingAXpostingDTO
                                       {
                                           oldSerialNumber = FloorDBAccess.GetString(row, "OldSerialNumber"),
                                           newSerialNumber = FloorDBAccess.GetString(row, "NewSerialNumber"),
                                           BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                           Size = FloorDBAccess.GetString(row, "CustomerSize"),
                                           Internallotnumber = internallotnumber,
                                           BatchCardDate = Convert.ToDateTime(FloorDBAccess.GetString(row, "BatchCardDate")),
                                           Ponumber = FloorDBAccess.GetString(row, "PONumber"),
                                           TotalPieces = FloorDBAccess.GetValue<int>(row, "TotalPieces"),
                                           LocationName = FloorDBAccess.GetString(row, "LocationName"),
                                           ItemNumber = FloorDBAccess.GetString(row, "ItemNumber"),
                                       }).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
                return objFPAXPostData;
            }
        }

        public static BatchDTO GetTempPackDetails(string serialNumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNumber", serialNumber));
            BatchDTO batchdto = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_FP_Get_TempPackDetails", lstParameters))
            {
                try
                {
                    batchdto = (from DataRow row in dtBatch.Rows
                                select new BatchDTO
                                {
                                    BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                    SerialNumber = FloorDBAccess.GetString(row, "SerialNumber"),
                                    Size = FloorDBAccess.GetString(row, "Size"),
                                    TotalPcs = Convert.ToInt32(row["TotalPieces"]),
                                    Location = FloorDBAccess.GetString(row, "LocationName"),
                                }).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
                return batchdto;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internallotnumber"></param>
        /// <param name="objFPAXPostData"></param>
        public static FinalPackingAXpostingDTO GetFinalPackingBatchInfoDetails(string internallotnumber)
        {
            FinalPackingAXpostingDTO objFPAXPostData = null;
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@internalLotnumber", internallotnumber));
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_FP_Get_ScanMultipleBatchCardforPosting", lstParameters))
            {
                try
                {
                    if (dtBatch.Rows.Count > 0)
                    {
                        objFPAXPostData = (from DataRow row in dtBatch.Rows
                                           select new FinalPackingAXpostingDTO
                                           {
                                               Ponumber = FloorDBAccess.GetString(row, "PONumber"),
                                               ItemNumber = FloorDBAccess.GetString(row, "itemnumber"),
                                               Size = FloorDBAccess.GetString(row, "customersize"),
                                               orderNumber = FloorDBAccess.GetString(row, "ordernumber"),
                                               Internallotnumber = FloorDBAccess.GetString(row, "internalLotnumber"),
                                               Outerlotno = FloorDBAccess.GetString(row, "OuterLotNo"),
                                               Casespacked = FloorDBAccess.GetValue<int>(row, "CasesPacked"),
                                               Preshipmentcases = FloorDBAccess.GetValue<int>(row, "PreshipmentCasesPacked"),
                                               customerLotNumber = FloorDBAccess.GetString(row, "CustomerLotNumber"),
                                               CustomerReferenceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber"),
                                               ManufacturingDate = FloorDBAccess.GetValue<DateTime>(row, "manufacturingdate"),
                                               ExpiryDate = FloorDBAccess.GetValue<DateTime>(row, "ExpiryDate"),
                                               InnerBoxCapacity = string.IsNullOrEmpty(FloorDBAccess.GetString(row, "InnerBoxCapacity")) ? 0 : Convert.ToInt32(FloorDBAccess.GetString(row, "InnerBoxCapacity")),
                                               Packdate = FloorDBAccess.GetValue<DateTime>(row, "PackDate"),
                                               PalletId = FloorDBAccess.GetString(row, "PalletId"),
                                               Resource = FloorDBAccess.GetString(row, "Resource"),
                                               BatchOrder = FloorDBAccess.GetString(row, "BatchOrder"),
                                           }).SingleOrDefault();

                        using (DataTable dt = GetScanMultipleBatchInfoforPosting(internallotnumber))
                        {
                            if (dt != null && dt.Rows.Count != 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    objFPAXPostData.RefSerialNumber1 = dr["RefSerialNumber1"].ToString();
                                    objFPAXPostData.RefSize1 = dr["RefSize1"].ToString();
                                    objFPAXPostData.RefBatchNumber1 = dr["RefBatchNumber1"].ToString();
                                    objFPAXPostData.BoxesPacked1 = string.IsNullOrEmpty(dr["BoxesPacked1"].ToString()) ? 0 : Convert.ToInt32(dr["BoxesPacked1"].ToString());
                                    objFPAXPostData.RefItemNumber1 = dr["RefItemNumber1"].ToString();//#MH 27/06/2018

                                    objFPAXPostData.RefSerialNumber2 = dr["RefSerialNumber2"].ToString();
                                    objFPAXPostData.RefSize2 = dr["RefSize2"].ToString();
                                    objFPAXPostData.RefBatchNumber2 = dr["RefBatchNumber2"].ToString();
                                    objFPAXPostData.BoxesPacked2 = string.IsNullOrEmpty(dr["BoxesPacked2"].ToString()) ? 0 : Convert.ToInt32(dr["BoxesPacked2"].ToString());
                                    objFPAXPostData.RefItemNumber2 = dr["RefItemNumber2"].ToString();//#MH 27/06/2018

                                    objFPAXPostData.RefSerialNumber3 = dr["RefSerialNumber3"].ToString();
                                    objFPAXPostData.RefSize3 = dr["RefSize3"].ToString();
                                    objFPAXPostData.RefBatchNumber3 = dr["RefBatchNumber3"].ToString();
                                    objFPAXPostData.BoxesPacked3 = string.IsNullOrEmpty(dr["BoxesPacked3"].ToString()) ? 0 : Convert.ToInt32(dr["BoxesPacked3"].ToString());
                                    objFPAXPostData.RefItemNumber3 = dr["RefItemNumber3"].ToString();//#MH 27/06/2018

                                    objFPAXPostData.RefSerialNumber4 = dr["RefSerialNumber4"].ToString();
                                    objFPAXPostData.RefSize4 = dr["RefSize4"].ToString();
                                    objFPAXPostData.RefBatchNumber4 = dr["RefBatchNumber4"].ToString();
                                    objFPAXPostData.BoxesPacked4 = string.IsNullOrEmpty(dr["BoxesPacked4"].ToString()) ? 0 : Convert.ToInt32(dr["BoxesPacked4"].ToString());
                                    objFPAXPostData.RefItemNumber4 = dr["RefItemNumber4"].ToString();//#MH 27/06/2018

                                    objFPAXPostData.RefSerialNumber5 = dr["RefSerialNumber5"].ToString();
                                    objFPAXPostData.RefSize5 = dr["RefSize5"].ToString();
                                    objFPAXPostData.RefBatchNumber5 = dr["RefBatchNumber5"].ToString();
                                    objFPAXPostData.BoxesPacked5 = string.IsNullOrEmpty(dr["BoxesPacked5"].ToString()) ? 0 : Convert.ToInt32(dr["BoxesPacked5"].ToString());
                                    objFPAXPostData.RefItemNumber5 = dr["RefItemNumber5"].ToString();//#MH 27/06/2018
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
                return objFPAXPostData;
            }
        }

        public static DataTable GetScanMultipleBatchInfoforPosting(string internallotnumber)
        {
            DataTable dtMultiBatchInfo = new DataTable();
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@internalLotnumber", internallotnumber));
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_FP_Get_ScanMultipleBatchInfoforPosting", lstParameters))
            {
                try
                {
                    if (dtBatch.Rows.Count > 0)
                    {
                        object[] colData = new object[25];
                        int aryCount = 0;

                        dtMultiBatchInfo = CreateMultiBatchInfoTempTable();

                        for (int row = 0; row < dtBatch.Rows.Count; row++)
                        {
                            for (int col = 0; col <= 4; col++)
                            {
                                colData[aryCount] = dtBatch.Rows[row][col].ToString();
                                aryCount++;
                            }
                        }

                        dtMultiBatchInfo.Rows.Add(colData);
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }

            }
            return dtMultiBatchInfo;
        }

        private static DataTable CreateMultiBatchInfoTempTable()
        {
            DataTable table = new DataTable();
            for (int colCnt = 1; colCnt <= 5; colCnt++)
            {
                table.Columns.Add("RefSerialNumber" + colCnt.ToString());
                table.Columns.Add("RefSize" + colCnt.ToString());
                table.Columns.Add("RefBatchNumber" + colCnt.ToString());
                table.Columns.Add("BoxesPacked" + colCnt.ToString());
                table.Columns.Add("RefItemNumber" + colCnt.ToString());
            }
            return table;
        }

        /// <summary>
        /// To Get Reject Glove Batch details for Unit Test
        /// added on 11th Oct 2016 at 10:32 by Max He, MH#1.n
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static BatchDTO GetRejectGloveBatchDetails(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_RejectGradeBatchInfo", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new BatchDTO
                            {
                                SerialNumber = Convert.ToString(serialNo),
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                Line = FloorDBAccess.GetString(row, "LineId"),
                                Shift = FloorDBAccess.GetValue<int>(row, "ShiftId"),
                                GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                RejectReasonId = FloorDBAccess.GetValue<int>(row, "ReasonId"),
                                LastModifiedOn = FloorDBAccess.GetValue<DateTime>(row, "LastModifiedOn"),
                                WorkStationId = FloorDBAccess.GetString(row, "WorkStationId"),
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        /// <summary>
        /// Check CBD Item before post to AX 
        /// added on 20th Oct 2016 at 5:25 by Max He, MH#1.n
        /// </summary>
        /// <param name="batchdto"></param>
        /// <returns></returns>
        public static bool CheckCBDItem(BatchDTO batchdto) //checkCBDItem
        {
            var ret = false;
            AvaInterfaceContract avaInterfaceContract = new AvaInterfaceContract();
            AVAMovementJournalContract avaMovementJournalContract = new AVAMovementJournalContract();
            //avaInterfaceContract.BatchNumber = batchdto.SerialNumber;
            //avaInterfaceContract.BatchCardNumber = batchdto.BatchNumber;
            //avaInterfaceContract.BatchSequence = GetBatchSequence(batchdto.SerialNumber);
            //avaInterfaceContract.FunctionID = OnlineRejectGloveFunctionidentifier.OREJ.ToString();
            avaMovementJournalContract.ResourceGroup = batchdto.ResourceGroup;
            //avaMovementJournalContract.CreatedDateTime = batchdto.BatchCarddate;
            avaMovementJournalContract.PostingDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
            // added on 10th Oct 2016 at 5:25 by Max He, MH#1.n
            //avaMovementJournalContract.Quantity = batchdto.BatchWeight;// Online Rejection Quantity(KG)
            //avaMovementJournalContract.Warehouse = batchdto.Warehouse;
            avaInterfaceContract.MovementJournal = avaMovementJournalContract;
            //using (AXAgentOne axAgent = new AXAgentOne())
            //{
            //    string[] axResponse = axAgent.checkCBDItem(avaInterfaceContract).Split(new string[] { "||" },
            //                                   StringSplitOptions.RemoveEmptyEntries);
            //    ret = Convert.ToBoolean(axResponse[0]);
            //}

            return ret;
        }
        #endregion
    }


 }
