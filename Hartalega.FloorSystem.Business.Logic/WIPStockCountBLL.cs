// -----------------------------------------------------------------------
// <copyright file="FinalPackingBLL.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Business.Logic
{
    using FloorSystem.Framework.Database;
    using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
    using Hartalega.FloorSystem.Framework;
    using Hartalega.FloorSystem.Framework.DbExceptionLog;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// WIP Stock Count business logic class
    /// </summary>
    public static class WIPStockCountBLL
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WIPStockCountBLL" /> class.
        /// </summary>
        #region CONSTRUCTOR
        static WIPStockCountBLL()
        {
            // No implementation required
        }
        #endregion

        #region GetWIPReports

        public static List<WIPReportDTO> GetWIPReports(string reportName)
        {
            List<WIPReportDTO> list = new List<WIPReportDTO>();
            List<FloorSqlParameter> paramList = new List<FloorSqlParameter>();
            paramList.Add(new FloorSqlParameter("@ReportName", reportName));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_WIP_GetWIPReports", paramList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = dt.AsEnumerable().Select(p => new WIPReportDTO()
                        {
                            WIPReportID = Convert.ToInt32(p["DBO_WIPReports_WIPReportID"]),
                            ReportName = p["DBO_WIPReports_ReportName"].ToString(),
                            ReportURL = p["DBO_WIPReports_ReportURL"].ToString(),
                            ReportParam = p["DBO_WIPReports_ReportParam"].ToString(),
                            DisplayOrder = Convert.ToInt32(p["DBO_WIPReports_DisplayOrder"]),
                        }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        #endregion

        #region GetWIPRunningNumber

        public static int GetWIPRunningNumber(DateTime selectedDate, int locationID, int areaID)
        {
            List<FloorSqlParameter> paramList = new List<FloorSqlParameter>();
            paramList.Add(new FloorSqlParameter("@Month", selectedDate));
            paramList.Add(new FloorSqlParameter("@LocationID", locationID));
            paramList.Add(new FloorSqlParameter("@AreaID", areaID));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_WIP_GetWIPRunningNumber", paramList));
        }

        #endregion

        #region GetWIPBatchBySerialNoList

        public static List<WIPTransactionDTO> GetWIPBatchBySerialNoList(string serialNoList, DateTime selectedDateTime)
        {
            List<WIPTransactionDTO> list = new List<WIPTransactionDTO>();
            List<FloorSqlParameter> paramList = new List<FloorSqlParameter>();
            paramList.Add(new FloorSqlParameter("@SelectedDate", selectedDateTime));
            paramList.Add(new FloorSqlParameter("@SerialNoList", serialNoList));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_WIP_GetBatcBySerialNoList", paramList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = dt.AsEnumerable().Select(p => new WIPTransactionDTO() {
                            RowNum = Convert.ToInt32(p["RowNo"]),
                            SerialNo = p["SerialNumber"].ToString(),
                            BatchNo = p["dbo_Batch_BatchNumber"].ToString(),
                            GloveType = p["dbo_Batch_GloveType"].ToString(),
                            GloveSize = p["dbo_Batch_Size"].ToString(),
                            BatchWeight = Convert.IsDBNull(p["dbo_Batch_BatchWeight"]) ? null : (Decimal?)p["dbo_Batch_BatchWeight"],
                            TenPCsWeight = Convert.IsDBNull(p["dbo_Batch_TenPCsWeight"]) ? null : (Decimal?)p["dbo_Batch_TenPCsWeight"],
                            TotalPCs = Convert.IsDBNull(p["dbo_Batch_TotalPCs"]) ? null : (int?)p["dbo_Batch_TotalPCs"],
                            WIPScanStatusName = p["dbo_WIPScanStatus_WIPScanStatusName"].ToString(),
                            WIPScanStatusID = Convert.ToInt32(p["dbo_WIPScanStatus_WIPScanStatusID"])
                        }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        #endregion

        #region GetWIPScannedData

        public static List<WIPTransactionDTO> GetWIPScannedData(DateTime selectedDate)
        {
            List<WIPTransactionDTO> list = new List<WIPTransactionDTO>();
            List<FloorSqlParameter> paramList = new List<FloorSqlParameter>();
            paramList.Add(new FloorSqlParameter("@SelectedDate", selectedDate));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_WIP_GetWIPScanDataTxn", paramList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = dt.AsEnumerable().Select(p => new WIPTransactionDTO()
                        {
                            ReferenceNumber = p["DBO_WIPTxn_Reference"].ToString(),
                            TotalBatch = Convert.ToInt32(p["DBO_WIPTxn_TotalBatch"]),
                        }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        #endregion

        #region GetWIPCutoffBatchByCutoffDate

        public static DateTime? GetWIPCutoffBatchLastCreatedDatetime(DateTime cutoffDate)
        {
            DateTime? lastCreatedDatetime;
            List<FloorSqlParameter> paramList = new List<FloorSqlParameter>();
            paramList.Add(new FloorSqlParameter("@CutoffDate", cutoffDate));
            lastCreatedDatetime =  Convert.ToDateTime(FloorDBAccess.ExecuteScalar("usp_WIP_GetLatestCreatedDatetimeForCutoffBatch", paramList));
            if (lastCreatedDatetime == DateTime.MinValue)
                lastCreatedDatetime = null;
            return lastCreatedDatetime;
        }

        #endregion

        #region GetWIPSummaryBatchByCountingDate

        public static DateTime? GetWIPSummaryBatchByCountingDate(DateTime countingDate)
        {
            DateTime? lastCreatedDatetime;
            List<FloorSqlParameter> paramList = new List<FloorSqlParameter>();
            paramList.Add(new FloorSqlParameter("@CountingDate", countingDate));
            lastCreatedDatetime =  Convert.ToDateTime(FloorDBAccess.ExecuteScalar("usp_WIP_GetLatestCreatedDatetimeForWIPSummary", paramList));
            if (lastCreatedDatetime == DateTime.MinValue)
                lastCreatedDatetime = null;
            return lastCreatedDatetime;
        }

        #endregion

        #region UpdateVoidWIPTxn

        public static int UpdateVoidWIPTxn(string voidReferenceNoList)
        {
            int errorCode = 0;
            List<FloorSqlParameter> paramList = new List<FloorSqlParameter>();
            paramList.Add(new FloorSqlParameter("@ReferenceNumberList", voidReferenceNoList));
            paramList.Add(new FloorSqlParameter("@ErrorCode", 0, ParameterDirection.Output));
            FloorDBAccess.ExecuteNonQuery("usp_WIP_SetUpdateVoidWIPTxn", paramList);
            errorCode = Convert.ToInt32(paramList.Where(p => p.ParameterName == "@ErrorCode").FirstOrDefault().ParamaterValue);
            return errorCode;
        }

        #endregion

        #region InsertWIPScannedData

        public static int InsertWIPScannedData(DataTable WIPTransactionTable)
        {
            int errorCode = 0;
            List<FloorSqlParameter> paramList = new List<FloorSqlParameter>();
            paramList.Add(new FloorSqlParameter("@WIPTransactionTable", WIPTransactionTable));
            paramList.Add(new FloorSqlParameter("@ErrorCode", 0,ParameterDirection.Output));
            FloorDBAccess.ExecuteNonQuery("USP_WIP_SetInsertWIPTransation", paramList);
            errorCode = Convert.ToInt32(paramList.Where(p => p.ParameterName == "@ErrorCode").FirstOrDefault().ParamaterValue);
            return errorCode;
        }

        #endregion

        #region InsertWIPCutoffBatch

        public static int InsertWIPCutoffBatch(DateTime cutoffDate, string cutoffRefno,DateTime? lastCutoffCreatedDatetime)
        {
            int errorCode = 0;
            List<FloorSqlParameter> paramList = new List<FloorSqlParameter>();
            paramList.Add(new FloorSqlParameter("@CutoffDate", cutoffDate));
            paramList.Add(new FloorSqlParameter("@CutoffRefNo", cutoffRefno));
            paramList.Add(new FloorSqlParameter("@LastCreatedDatetime", lastCutoffCreatedDatetime));
            paramList.Add(new FloorSqlParameter("@ErrorCode", 0, ParameterDirection.Output));
            FloorDBAccess.ExecuteNonQuery("usp_WIP_SetInsertWIPTransactionForCutoffBatch", paramList);
            errorCode = Convert.ToInt32(paramList.Where(p => p.ParameterName == "@ErrorCode").FirstOrDefault().ParamaterValue);
            return errorCode;
        }

        #endregion

        #region InsertWIPSummary

        public static int InsertWIPSummary(DateTime countingDate, DateTime? lastWIPSummaryCreatedDatetime)
        {
            int errorCode = 0;
            List<FloorSqlParameter> paramList = new List<FloorSqlParameter>();
            paramList.Add(new FloorSqlParameter("@CountingDate", countingDate));
            paramList.Add(new FloorSqlParameter("@LastCreatedDatetime", lastWIPSummaryCreatedDatetime));
            paramList.Add(new FloorSqlParameter("@ErrorCode", 0, ParameterDirection.Output));
            FloorDBAccess.ExecuteNonQuery("usp_WIP_SetInsertWIPSummary", paramList);
            errorCode = Convert.ToInt32(paramList.Where(p => p.ParameterName == "@ErrorCode").FirstOrDefault().ParamaterValue);
            return errorCode;
        }

        #endregion
    }
}
