using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework;
using System.Globalization;

namespace Hartalega.FloorSystem.Business.Logic
{
    /// <summary>
    /// TV Report BLL
    /// </summary>
    public static class TVReportsBLL
    {
        #region Public Methods
        /// <summary>
        /// This method is used to get report details from database
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetReportDetails()
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            List<DropdownDTO> list = new List<DropdownDTO>(); ;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_ReportDetails", null))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            list.Add(new DropdownDTO() { IDField = FloorDBAccess.GetString(dr, "ReportURL"), DisplayField = FloorDBAccess.GetString(dr, "ReportName")});
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.REPORTEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        public static List<DropdownDTO> GetTVReportDetails()
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            List<DropdownDTO> list = new List<DropdownDTO>(); ;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_TVReportDetails", null))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            list.Add(new DropdownDTO() { IDField = FloorDBAccess.GetString(dr, "ReportURL"), DisplayField = FloorDBAccess.GetString(dr, "ReportName") });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.REPORTEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }


        public static List<DropdownDTO> GetOEEReportDetails()
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            List<DropdownDTO> list = new List<DropdownDTO>(); ;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_OEE_ReportDetails", null))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            list.Add(new DropdownDTO() { IDField = Convert.ToString(dr["ReportURL"]), DisplayField = Convert.ToString(dr["ReportName"]) });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.REPORTEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        public static DataTable GetQAIMonitoringData()
        {
            return FloorDBAccess.ExecuteDataTable("USP_SEL_QCTypeData", null);
        }
        /// <summary>
        /// GetQAIMonitoring report based on date
        /// </summary>
        /// <returns></returns>
        public static DataTable GetQAIMonitoringDataByDate(DateTime date)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@Date", date));
            return FloorDBAccess.ExecuteDataTable("usp_TV_GetQAIMonitoringReportByDate", PrmList);
        }

        public static DataTable GetQAIMonthlyMonitoringDataByDate(DateTime date, string line)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@DateMonth", date));
            PrmList.Add(new FloorSqlParameter("@Line", line));
            return FloorDBAccess.ExecuteDataTable("usp_TV_GetQAIMonthlyMonitoringReportByDate", PrmList);
        }

        //add by Cheah (27/02/2017)
        public static DataTable GetQAIPercentData()
        {
            return FloorDBAccess.ExecuteDataTable("USP_SEL_QCPctData", null);
        }
        //end add

        /// <summary>
        /// GetQAIPercentData By Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataTable GetQAIPercentageDataByDate(DateTime date)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@Date", date));
            return FloorDBAccess.ExecuteDataTable("usp_TV_GetQAIPercentageDataByDate", PrmList);
        }

        public static DataTable GetLineMaster()
        {
            return FloorDBAccess.ExecuteDataTable("USP_GET_LineMaster");
        }

        public static DataTable GetLocationMaster()
        {
            return FloorDBAccess.ExecuteDataTable("USP_GET_LocationMaster");
        }
        public static DataTable GetPlantsList()
        {
            return FloorDBAccess.ExecuteDataTable("USP_SEL_Plants", null);
        }

        public static DataTable GetRejectGlovesummary(DateTime fromdate, DateTime todate, string Plant)
        {
               List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
               PrmList.Add(new FloorSqlParameter("@FROMDate", fromdate));
               PrmList.Add(new FloorSqlParameter("@ToDate", todate));
                PrmList.Add(new FloorSqlParameter("@Plant", Plant));

            return FloorDBAccess.ExecuteDataTable("usp_GET_RejectGloveSummary", PrmList);
        }

        public static DataTable GetTenPcsWeightData()
        {
           return FloorDBAccess.ExecuteDataTable("USP_SEL_TenPcsWeight", null);
        }

        public static DataTable GetQCDefectData()
        {
            return FloorDBAccess.ExecuteDataTable("USP_SEL_QCDefect", null);
        }
        public static DataTable GetCosmeticDefectData()
        {
            return FloorDBAccess.ExecuteDataTable("USP_SEL_CosmeticDefect", null);
        }
        public static DataTable GetProdDefectData()
        {
            return FloorDBAccess.ExecuteDataTable("USP_SEL_ProductionDefect", null);
        }
        public static String GetLastBatchJobRunTime()
        {
            var strLastBatchJobRunTime = FloorDBAccess.ExecuteScalar("USP_GET_LastTVBatchJobRunTime", null);
            return strLastBatchJobRunTime == null ? string.Empty : strLastBatchJobRunTime.ToString(); 
        }
        public static String GetNextBatchJobRunTime()
        {
            return FloorDBAccess.ExecuteScalar("USP_GET_NextTVBatchJobRunTime", null).ToString ();
        }
        public static DataTable GetColor()
        {
            return FloorDBAccess.ExecuteDataTable("USP_SEL_Color", null);
        }
    }
        #endregion
}
