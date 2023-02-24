// -----------------------------------------------------------------------
// <copyright file="TumblingBLL.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace Hartalega.FloorSystem.Business.Logic
{
    /// <summary>
    /// Tumbling business logic class
    /// </summary>
    public class TumblingBLL : Framework.Business.BusinessBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TumblingBLL" /> class.
        /// </summary>
        public TumblingBLL()
        {
            // No implementation required
        }

        #region Tumbling User Methods
        /// <summary>
        /// Method to save batch Cards - Normal,Lost,Water Tight, Visual Test
        /// </summary>
        /// <param name="objBatchDTO"></param>
        /// <returns></returns>
        public static BatchDTO SaveBatchCard(BatchDTO objBatchDTO)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@shift", objBatchDTO.Shift));
            PrmList.Add(new FloorSqlParameter("@line", objBatchDTO.Line));
            PrmList.Add(new FloorSqlParameter("@size", objBatchDTO.Size));
            PrmList.Add(new FloorSqlParameter("@gloveType", objBatchDTO.GloveType));
            PrmList.Add(new FloorSqlParameter("@batchWeight", objBatchDTO.BatchWeight));
            PrmList.Add(new FloorSqlParameter("@tenPcsWeight", objBatchDTO.TenPcsWeight));
            PrmList.Add(new FloorSqlParameter("@batchCardDate", objBatchDTO.BatchCarddate));
            PrmList.Add(new FloorSqlParameter("@isOnline", objBatchDTO.IsOnline));
            PrmList.Add(new FloorSqlParameter("@operatorId", objBatchDTO.OperatorId));
            PrmList.Add(new FloorSqlParameter("@workStationNumber", objBatchDTO.WorkStationId));
            PrmList.Add(new FloorSqlParameter("@batchType", objBatchDTO.BatchType));
            PrmList.Add(new FloorSqlParameter("@location", objBatchDTO.LocationId));
            PrmList.Add(new FloorSqlParameter("@module", objBatchDTO.Module));
            PrmList.Add(new FloorSqlParameter("@subModule", objBatchDTO.SubModule));
            PrmList.Add(new FloorSqlParameter("@authorizedBy", objBatchDTO.AuthorizedBy));
            PrmList.Add(new FloorSqlParameter("@lostArea", objBatchDTO.BatchLostArea));
            PrmList.Add(new FloorSqlParameter("@site", objBatchDTO.Site));
            PrmList.Add(new FloorSqlParameter("@shiftName", objBatchDTO.ShiftName));
            PrmList.Add(new FloorSqlParameter("@authorizedFor", objBatchDTO.AuthorizedFor));
            BatchDTO objBatch = null;
            using (DataTable dtresult = FloorDBAccess.ExecuteDataTable("USP_SAV_BatchCard", PrmList))
            {
                objBatch = (from DataRow row in dtresult.Rows
                            select new BatchDTO
                            {
                                SerialNumber = FloorDBAccess.GetString(row, "SerialNumber"),
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber")
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        /// <summary>
        /// Tumbling Reprint and Save.
        /// </summary>
        /// <param name="serialNumbers"></param>
        /// <param name="reprintDateTime"></param>
        /// <param name="operatorId"></param>
        /// <param name="reasonId"></param>
        /// <param name="workStationName"></param>
        /// <returns></returns>
        public static BatchDTO SaveReprintBatchCard(string serialNumber, DateTime reprintDateTime, string operatorId, int reasonId, string workStationName)
        {
            BatchDTO objBatchDTO = new BatchDTO();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            prmList.Add(new FloorSqlParameter("@ReprintDateTime", reprintDateTime));
            prmList.Add(new FloorSqlParameter("@OperatorId", operatorId));
            prmList.Add(new FloorSqlParameter("@ReasonId", reasonId));
            prmList.Add(new FloorSqlParameter("@WorkStationName", workStationName));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_HBC_ReprintBatchCard_Save", prmList))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    objBatchDTO.SerialNumber = FloorDBAccess.GetString(dt.Rows[0], "SerialNumber");
                    objBatchDTO.BatchNumber = FloorDBAccess.GetString(dt.Rows[0], "BatchNumber");
                    objBatchDTO.Size = FloorDBAccess.GetString(dt.Rows[0], "Size");
                    objBatchDTO.GloveType = FloorDBAccess.GetString(dt.Rows[0], "GloveType");
                    objBatchDTO.BatchWeight = FloorDBAccess.GetValue<Decimal>(dt.Rows[0], "BatchWeight");
                    objBatchDTO.TenPcsWeight = FloorDBAccess.GetValue<Decimal>(dt.Rows[0], "TenPcsWeight");
                    objBatchDTO.BatchCarddate = FloorDBAccess.GetValue<DateTime>(dt.Rows[0], "BatchCardDate");
                    objBatchDTO.BatchType = FloorDBAccess.GetString(dt.Rows[0], "BatchType");
                }
            }
            return objBatchDTO;
        }

        /// <summary>
        /// #Azrul 19/10/2020: Get Batch Details for Print PNBC
        /// </summary>
        /// <returns>List<BatchDTO></returns>
        public static List<BatchOrderDetailsDTO> GetPrintBatchDetails(string userId, int shiftId, string line, DateTime batchCardDate, string moduleId, string subModuleId, int sitenumber, int WorkStationNumber, string resource, string batchOrder, string gloveCode, string size, decimal batchWeight, int quantity, string authorizedBy, int authorizedFor)
        {
            List<BatchOrderDetailsDTO> listBatchDTO = new List<BatchOrderDetailsDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@UserId", userId));
            prmList.Add(new FloorSqlParameter("@ShiftId", shiftId));
            prmList.Add(new FloorSqlParameter("@Line", line));
            prmList.Add(new FloorSqlParameter("@BatchCardDate", batchCardDate));
            prmList.Add(new FloorSqlParameter("@ModuleId", moduleId));
            prmList.Add(new FloorSqlParameter("@SubModuleID", subModuleId));
            prmList.Add(new FloorSqlParameter("@SiteNumber", sitenumber));
            prmList.Add(new FloorSqlParameter("@WorkStationNumber", WorkStationNumber));
            prmList.Add(new FloorSqlParameter("@Resource", resource));
            prmList.Add(new FloorSqlParameter("@BatchOrder", batchOrder));
            prmList.Add(new FloorSqlParameter("@GloveCode", gloveCode));
            prmList.Add(new FloorSqlParameter("@Size", size));
            prmList.Add(new FloorSqlParameter("@BatchWeight", batchWeight));
            prmList.Add(new FloorSqlParameter("@Quantity", quantity));
            prmList.Add(new FloorSqlParameter("@authorizedBy", authorizedBy));
            prmList.Add(new FloorSqlParameter("@authorizedFor", authorizedFor));

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("Usp_DOT_PNBC_Print_Save", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            BatchOrderDetailsDTO batch = new BatchOrderDetailsDTO();
                            batch.SerialNumber = FloorDBAccess.GetString(dr, "SerialNumber");
                            batch.BatchNumber = FloorDBAccess.GetString(dr, "BatchNumber");
                            batch.GloveType = FloorDBAccess.GetString(dr, "GloveCode");
                            batch.GloveCategory = FloorDBAccess.GetString(dr, "GloveCategory");
                            batch.Size = FloorDBAccess.GetString(dr, "Size");
                            batch.BatchCarddate = FloorDBAccess.GetValue<DateTime>(dr, "BatchCardDate");
                            batch.Resource = FloorDBAccess.GetString(dr, "Resource");
                            batch.BatchWeight = FloorDBAccess.GetValue<decimal>(dr, "BatchWeight");
                            batch.Quantity = FloorDBAccess.GetValue<int>(dr, "Quantity");
                            listBatchDTO.Add(batch);
                        }
                    }
                }
                catch (ArgumentException argex)
                {
                    throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.MANUALPRINTBATCHDETAILSGETEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return listBatchDTO;
        }

        /// <summary>
        /// #Azrul 24/12/2021: Get Line, Resources and Batch Order details for Print Normal Batch Card.
        /// </summary>
        /// <returns>List<DropdownDTO></returns>
        public static List<BatchOrderDetailsDTO> GetTumblingResource(int locationId, string lineId, string res, string bo)
        {
            List<BatchOrderDetailsDTO> TumblingResource = null;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@LocationId", locationId));
            prmList.Add(new FloorSqlParameter("@LineId", lineId));
            prmList.Add(new FloorSqlParameter("@Resource", res));
            prmList.Add(new FloorSqlParameter("@BO", bo));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_TumblingResource_Get", prmList))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                if (dt != null && dt.Rows.Count != 0)
                {
                    TumblingResource = (from DataRow dr in dt.Rows
                                        select new BatchOrderDetailsDTO
                                        {
                                            Resource = FloorDBAccess.GetString(dr, "Resource"),
                                            ResourceId = FloorDBAccess.GetString(dr, "ResourceId"),
                                            LineId = FloorDBAccess.GetString(dr, "LineId"),
                                            Line = FloorDBAccess.GetString(dr, "Line"),
                                            BthOrderId = FloorDBAccess.GetString(dr, "BatchOrder"),
                                            ItemId = FloorDBAccess.GetString(dr, "GloveCode"),
                                            Size = FloorDBAccess.GetString(dr, "Size")
                                        }).ToList();
                }
            }
            return TumblingResource;
        }


        //public static BatchDTO SaveWaterTightBatchCard(BatchDTO objBatchDTO)
        //{
        //    BatchDTO result;
        //    AXPostingDTO axposting = new AXPostingDTO();
        //    using (var scope = new TransactionScope())
        //    {
        //        // save batch data
        //       result = SaveBatchCard(objBatchDTO);

        //        // post to AX4
        //        try
        //        {
        //            //set log data
        //            axposting.ServiceName = "PostWaterTightBatchCardData";
        //            axposting.PostedDate = CommonBLL.GetCurrentDateAndTimeFromServer();
        //            axposting.PostingType = objBatchDTO.BatchType;
        //            axposting.Area = WorkStationDTO.GetInstance().Area;
        //            axposting.SerialNumber = result.SerialNumber;
        //            axposting.BatchNumber = result.BatchNumber;

        //            var isPostingSuccess = AX4PostingBLL.PostWaterTightBatchCardData(result.SerialNumber);
        //            if (isPostingSuccess)
        //            {
        //                axposting.IsPostedToAX = true;
        //                axposting.IsPostedInAX = true;
        //                AX4PostingBLL.LogAXPostingInfo(axposting); //log for success posting
        //                scope.Complete(); // commit if success 
        //                AX4PostingBLL.AXCommit(); //commit ax4

        //            }
        //            else
        //            {
        //                axposting.ExceptionCode = Messages.AX_INTEGRATION_EXCEPTION;
        //                axposting.IsPostedToAX = false;
        //                axposting.IsPostedInAX = false;
        //                  AX4PostingBLL.AXRollback (); //rollback ax4
        //                throw new FloorSystemException(Messages.AX_INTEGRATION_EXCEPTION, Constants.AXSERVICEERROR, new Exception("Posting to AX failed."));

        //            }
        //        }
        //        catch(FloorSystemException ex)
        //        {
        //            if (AX4PostingBLL.GetIsLogon())  //check islogon before  rollback
        //                AX4PostingBLL.AXRollback(); //rollback ax4
        //            throw ex;
        //        }
        //        catch (Exception ex)
        //        {
        //            if (AX4PostingBLL.GetIsLogon())  //check islogon before  rollback
        //                AX4PostingBLL.AXRollback(); //rollback ax4
        //            throw new FloorSystemException(Messages.AX_SERVICE_UNAVAILABLE, Constants.SERVICEERROR, ex);
        //        }


        //    }
        //    if (!axposting.IsPostedToAX) //for failed transaction
        //        AX4PostingBLL.LogAXPostingInfo(axposting);

        //    return result;
        //}

        /// <summary>
        /// Method to save batch Cards - Normal,Lost,Water Tight, Visual Test
        /// </summary>
        /// <param name="objBatchDTO"></param>
        /// <returns></returns>
        public static BatchDTO SaveRejectSticker(BatchDTO objBatchDTO)
        {
            //commented by MYAdamas for used glove column line and shift should be null will be passing null for line if the line is used_glove
            //if(objBatchDTO.Line==Constants.USED_GLOVE)
            //{
            //    objBatchDTO.Line = Constants.TWENTYNINE.ToString();
            //}
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            if (!string.IsNullOrEmpty(objBatchDTO.Line))
            {
                PrmList.Add(new FloorSqlParameter("@shift", objBatchDTO.Shift));
                PrmList.Add(new FloorSqlParameter("@line", objBatchDTO.Line));
                PrmList.Add(new FloorSqlParameter("@shiftName", objBatchDTO.ShiftName));
            }
            PrmList.Add(new FloorSqlParameter("@gloveType", objBatchDTO.GloveType));
            PrmList.Add(new FloorSqlParameter("@batchWeight", objBatchDTO.BatchWeight));
            PrmList.Add(new FloorSqlParameter("@reasonId", objBatchDTO.RejectReasonId));
            PrmList.Add(new FloorSqlParameter("@batchCardDate", objBatchDTO.BatchCarddate));
            PrmList.Add(new FloorSqlParameter("@operatorId", objBatchDTO.OperatorId));
            PrmList.Add(new FloorSqlParameter("@workStationNumber", objBatchDTO.WorkStationId));
            PrmList.Add(new FloorSqlParameter("@batchType", objBatchDTO.BatchType));
            PrmList.Add(new FloorSqlParameter("@location", objBatchDTO.LocationId));
            PrmList.Add(new FloorSqlParameter("@moduleName", objBatchDTO.Module));
            PrmList.Add(new FloorSqlParameter("@subModuleName", objBatchDTO.SubModule));
            PrmList.Add(new FloorSqlParameter("@site", objBatchDTO.Site));
            BatchDTO objBatch = null;
            using (DataTable dtresult = FloorDBAccess.ExecuteDataTable("USP_SAV_RejectGlove", PrmList))
            {
                objBatch = (from DataRow row in dtresult.Rows
                            select new BatchDTO
                            {
                                SerialNumber = FloorDBAccess.GetString(row, "SerialNumber"),
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber")
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        /// <summary>
        /// Save Online ByPassGloveBatch
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="tenPcsWeight"></param>
        /// <param name="batchWeight"></param>
        /// <param name="reason"></param>
        /// <param name="authorizedBy"></param>
        /// <param name="operatorId"></param>
        /// <param name="workStationNumber"></param>
        /// <returns></returns>
        public static string SaveOnlineByPassGloveBatch(string serialNumber, double tenPcsWeight, double batchWeight, string reason, string authorizedBy, string operatorId, string workStationNumber, int authorizedFor)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@serialNumber", serialNumber));
            PrmList.Add(new FloorSqlParameter("@tenPcsWeight", tenPcsWeight));
            PrmList.Add(new FloorSqlParameter("@batchWeight", batchWeight));
            PrmList.Add(new FloorSqlParameter("@reason", reason));
            PrmList.Add(new FloorSqlParameter("@authorizedBy", authorizedBy));
            PrmList.Add(new FloorSqlParameter("@operatorId", operatorId));
            PrmList.Add(new FloorSqlParameter("@location", WorkStationDTO.GetInstance().LocationId));
            PrmList.Add(new FloorSqlParameter("@workStationNumber", workStationNumber));
            PrmList.Add(new FloorSqlParameter("@authorizedFor", authorizedFor));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_UPD_OnlineByPassGlove", PrmList));
        }

        /// <summary>
        /// Check Serial No status for Polymer Test
        /// </summary>
        /// <param name="serialNo">Serial Number</param>
        /// <returns>Status as result.It will not throw exception for null case.</returns>
        public static TestSlipDTO GetStatusPolymerTest(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            TestSlipDTO objTestSlip = new TestSlipDTO();
            using (DataTable dtResult = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_SerialNo_PolymerTest", lstParameters))
            {
                objTestSlip = (from DataRow row in dtResult.Rows
                               select new TestSlipDTO
                               {
                                   Status = FloorDBAccess.GetString(row, "Status"),
                                   ReworkCount = Convert.ToInt32(FloorDBAccess.GetString(row, "ReworkCount")),
                                   Reprint = Convert.ToInt32(FloorDBAccess.GetString(row, "ReprintCount")),
                                   TesterID = FloorDBAccess.GetString(row, "OperatorId"),
                                   Name = FloorDBAccess.GetString(row, "Name"),
                                   ReferenceId = Convert.ToDecimal(FloorDBAccess.GetString(row, "ReferenceNumber")),
                                   LastModifiedOn = Convert.ToDateTime(FloorDBAccess.GetString(row, "SlipGenerationDate")),
                               }).SingleOrDefault();
            }
            return objTestSlip;
        }

        /// <summary>
        /// Inserts Polymer Test Slip details.
        /// </summary>
        /// <param name="objPostTreatment"></param>
        public static void SavePolymerTestSlip(TestSlipDTO objTestSlip)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@operatorId", objTestSlip.TesterID));
            lstParameters.Add(new FloorSqlParameter("@serialNo", objTestSlip.SerialNumber));
            lstParameters.Add(new FloorSqlParameter("@referenceId", objTestSlip.ReferenceId));
            lstParameters.Add(new FloorSqlParameter("@reworkCount", objTestSlip.ReworkCount));
            lstParameters.Add(new FloorSqlParameter("@reprintCount", objTestSlip.Reprint));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOn", objTestSlip.LastModifiedOn));
            lstParameters.Add(new FloorSqlParameter("@workstationId", objTestSlip.WorkstationId));
            lstParameters.Add(new FloorSqlParameter("@moduleId", WorkStationDTO.GetInstance().Module));
            lstParameters.Add(new FloorSqlParameter("@subModuleId", WorkStationDTO.GetInstance().SubModule));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_SAV_PolymerTestSlip", lstParameters);
        }

        /// <summary>
        /// Get Internal Lot Details
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>DataTable as resut</returns>
        public static CustomerRejectDTO GetLotDetails(string lotNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@lotNo", lotNo.ToString()));
            CustomerRejectDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_LotDetails", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new CustomerRejectDTO
                            {
                                CustomerName = FloorDBAccess.GetString(row, "CustomerName"),
                                GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                Description = FloorDBAccess.GetString(row, "Description"),
                                SizeSelected = FloorDBAccess.GetString(row, "Description"),
                                Line = FloorDBAccess.GetString(row, "LineId"),
                                ShiftName = FloorDBAccess.GetString(row, "ShiftName"),
                                ShiftId = Convert.ToInt32(FloorDBAccess.GetString(row, "ShiftId")),
                                Size = FloorDBAccess.GetString(row, "Size"),
                                TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPCsWeight"),
                                BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                FGCode = FloorDBAccess.GetString(row, "FGCode"),
                                FGSize = FloorDBAccess.GetString(row, "FGSize"),
                                CasesPacked = Convert.ToInt32(FloorDBAccess.GetString(row, "CasesPacked")),
                                CaseCapacity = Convert.ToInt32(FloorDBAccess.GetString(row, "CaseCapacity"))
                            }).SingleOrDefault();
            }

            return objBatch;
        }

        /// <summary>
        /// Get Batch details based on Serial Number.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>DataTable as resut</returns>
        public static BatchDTO GetBatchScanInDetailsforPolymer(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_BatchDetailsForPolymerTest", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new BatchDTO
                            {
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                Size = FloorDBAccess.GetString(row, "Size"),
                                TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPCsWeight"),
                                BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                TotalPcs = FloorDBAccess.GetValue<Int32>(row, "TotalPCs"),
                                QCType = FloorDBAccess.GetString(row, "QCType"),
                                ReworkCount = FloorDBAccess.GetValue<Int32>(row, "Rework"),
                                PTTenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "PTTenPCsWeight"),
                                PTBatchWeight = FloorDBAccess.GetValue<Decimal>(row, "PTBatchWeight")
                            }).SingleOrDefault();
            }

            return objBatch;
        }

        /// <summary>
        /// Updates Polymer Test Slip details.
        /// </summary>
        /// <param name="objPostTreatment"></param>
        public static void UpdatePolymerTestSlip(decimal serialNo, Int32 reprint, DateTime lastModifiedOn)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@reprintCount", reprint));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOn", lastModifiedOn));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_SAV_PolymerTestSlip_Reprint", lstParameters);
        }

        /// <summary>
        /// Save Customer reject gloves.
        /// </summary>
        /// <param name="objPostTreatment"></param>
        public static CustomerRejectDTO SaveCustomerRejectGloves(List<CustomerRejectDTO> cutomerRejectList, string QCType, string Site)
        {
            string customerRejectDetails = CommonBLL.SerializeTOXML(cutomerRejectList);
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@customerRejectDetails", customerRejectDetails));
            lstParameters.Add(new FloorSqlParameter("@QCType", QCType));
            lstParameters.Add(new FloorSqlParameter("@WorkStationId", WorkStationDTO.GetInstance().WorkStationId));
            lstParameters.Add(new FloorSqlParameter("@Site", Site));
            CustomerRejectDTO objBatch = null;
            using (DataTable dtresult = FloorDBAccess.ExecuteDataTable("USP_SAV_CustomerRejectGloves", lstParameters))
            {
                objBatch = (from DataRow row in dtresult.Rows
                            select new CustomerRejectDTO
                            {
                                SerialNumber = FloorDBAccess.GetString(row, "SerialNumber"),
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber")
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        /// <summary>
        /// Gets Glove category
        /// </summary>
        /// <param name="Glovetype"></param>
        /// <returns>Glove category as string</returns>
        public static string GetGloveCategory(string Glovetype)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveType", Glovetype));
            var GloveCategory = FloorDBAccess.ExecuteScalar("USP_GET_GlovesCategory", PrmList);
            return Convert.ToString(GloveCategory);
        }

        public static string GetRejectGloveCategory(string Glovetype)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveType", Glovetype));
            var GloveCategory = FloorDBAccess.ExecuteScalar("USP_GET_RejectGlovesCategory", PrmList);
            return Convert.ToString(GloveCategory);
        }

        public static string GetRejectGloveType(string Barcode)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveType", Barcode));
            var GloveCategory = FloorDBAccess.ExecuteScalar("USP_GET_RejectGloveType", PrmList);
            return Convert.ToString(GloveCategory);
        }

        public static string GetGloveType(string Glovetype)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveType", Glovetype));
            var GloveCategory = FloorDBAccess.ExecuteScalar("USP_GET_GloveType", PrmList);
            return Convert.ToString(GloveCategory);
        }

        public static string GetGloveDescription(string Glovetype)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveType", Glovetype));
            var GloveCategory = FloorDBAccess.ExecuteScalar("USP_GET_GloveDescription", PrmList);
            return Convert.ToString(GloveCategory);
        }

        /// <summary>
        /// To Get Glove types for regect Gloves based on line id
        /// </summary>
        /// <param name="Lineid"></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetRejectGloveByLine(string Lineid)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@LineId", Lineid));
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_Get_GetRejectGloveByLine", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "GloveType"),
                                DisplayField = FloorDBAccess.GetString(row, "GloveType"),
                                // SelectedValue = FloorDBAccess.GetString(row, "CurrentShift")
                            }).ToList();

                }
            }
            return list;
        }

        /// <summary>
        /// Deletes Batch entry if AX posting is unsuccessful 
        /// </summary>
        /// <param name="SerialNo"></param>
        public static bool DelCustomerRejectEntry(decimal SerialNo)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNo", SerialNo));
            FloorDBAccess.ExecuteNonQuery("USP_DEL_CustomerRejectEntryDeletion", PrmList);
            return false;
        }

        public static bool CheckSerialNumber(decimal serialNo)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNo", serialNo));
            bool GloveCategory = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_CHK_SerialNumber", PrmList));
            return GloveCategory;
        }

        /// <summary>
        /// Deletes Batch entry if AX posting is unsuccessful 
        /// </summary>
        /// <param name="SerialNo"></param>
        public static int GetCustomerTableLotCount(string lotNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@lotNo", lotNo.ToString()));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_CustomerTableLotCount", lstParameters));
        }

        public static bool CheckReprintLocation(string serialNo, string workstationID, out int plantLocation)
        {
            bool isCorrectPlant = false;
            plantLocation = 0;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@SerialNumber", serialNo));
            prmList.Add(new FloorSqlParameter("@WorkstationID", workstationID));

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_CheckReprintLocation", prmList))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    isCorrectPlant = Convert.ToBoolean(FloorDBAccess.GetValue<int>(dt.Rows[0], "isCorrectPlant"));
                    plantLocation = FloorDBAccess.GetValue<int>(dt.Rows[0], "Plant");
                }
            }

            return isCorrectPlant;
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


        #endregion
    }
}
