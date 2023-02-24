// -----------------------------------------------------------------------
// <copyright file="ProductionLoggingBLL.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Business.Logic
{
    #region using
    using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
    using Hartalega.FloorSystem.Framework;
    using Hartalega.FloorSystem.Framework.Database;
    using Hartalega.FloorSystem.Framework.DbExceptionLog;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    #endregion

    /// <summary>
    /// Production logging business logic static class
    /// </summary>
    public static class ProductionLoggingBLL
    {
        #region Private Class Variables
        private const string _subSystem = "Production Logging System";
        #endregion

        #region InScope
        #region ProductionActivity
        /// <summary>
        /// Get Line Activities
        /// </summary>
        /// <param name="line"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static List<ProductionLineActivityDTO> GetLineActivities(string line, DateTime? from, DateTime? to)
        {
            List<ProductionLineActivityDTO> lstProductionLineActivity = new List<ProductionLineActivityDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();

            prmList.Add(new FloorSqlParameter("@LineId", line));
            if (from.HasValue && to.HasValue)
            {
                prmList.Add(new FloorSqlParameter("@Fromdate", from));
                prmList.Add(new FloorSqlParameter("@Todate", to));
            }
            DataTable dt = FloorDBAccess.ExecuteDataTable("usp_ProductionLineActivity_Get", prmList);

            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ProductionLineActivityDTO prodLineActivityDTO = new ProductionLineActivityDTO();
                    prodLineActivityDTO.ProductionLineActivityId = FloorDBAccess.GetValue<int>(dr, "ProductionLineActivityId");
                    prodLineActivityDTO.Line = FloorDBAccess.GetString(dr, "LineId");
                    prodLineActivityDTO.Date = FloorDBAccess.GetString(dr, "StartDate");
                    prodLineActivityDTO.Time = FloorDBAccess.GetString(dr, "StartTime");
                    prodLineActivityDTO.ID = FloorDBAccess.GetString(dr, "OperatorId");
                    prodLineActivityDTO.Name = FloorDBAccess.GetString(dr, "Name");
                    prodLineActivityDTO.ActivityType = FloorDBAccess.GetString(dr, "ActivityType");
                    prodLineActivityDTO.ActivityDetails = FloorDBAccess.GetString(dr, "ActivityDetails");                    
                    lstProductionLineActivity.Add(prodLineActivityDTO);
                }
            }
            return lstProductionLineActivity;
        }

        public static ProductionLineActivityDTO GetLineActivitiesByID(int ProductionLineActivityId)
        {
            ProductionLineActivityDTO prodLineActivityDTO = new ProductionLineActivityDTO();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@ProductionLineActivityId", ProductionLineActivityId));
            DataTable dt = FloorDBAccess.ExecuteDataTable("usp_ProductionLineActivity_GetByID", prmList);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    prodLineActivityDTO.ProductionLineActivityId = FloorDBAccess.GetValue<int>(dr, "ProductionLineActivityId");
                    prodLineActivityDTO.Line = FloorDBAccess.GetString(dr, "LineId");
                    prodLineActivityDTO.Date = FloorDBAccess.GetString(dr, "StartDate");
                    prodLineActivityDTO.Time = FloorDBAccess.GetString(dr, "StartTime");
                    prodLineActivityDTO.ID = FloorDBAccess.GetString(dr, "OperatorId");
                    prodLineActivityDTO.Name = FloorDBAccess.GetString(dr, "Name");
                    prodLineActivityDTO.ActivityType = FloorDBAccess.GetString(dr, "ActivityType");
                    prodLineActivityDTO.ActivityDetails = FloorDBAccess.GetString(dr, "ActivityDetails");

                }
            }
            return prodLineActivityDTO;
        }

        public static List<DropdownDTO> GetLines(string location)
        {
            List<DropdownDTO> prodlines = null;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@location", location));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_Line_ALL", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        prodlines = (from DataRow dr in dt.Rows
                                     select new DropdownDTO
                                     {
                                         IDField = FloorDBAccess.GetString(dr, "LineNumber"),
                                         DisplayField = FloorDBAccess.GetString(dr, "LineNumber")
                                     }).ToList();
                    }
                }
                catch (ArgumentException argex)
                {
                    throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETREASONFORREPRINTEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return prodlines;
        }

        #endregion

        #region ProdActivityAdd

        /// <summary>
        /// Insert  Production Activity
        /// </summary>
        /// <param name="line"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <param name="activityType"></param>
        /// <param name="operatorId"></param>
        /// <param name="detailsOfActivity"></param>
        /// <param name="workstationNumber"></param>
        /// <returns></returns>
        public static bool SaveProductionActivity(string line, DateTime date, string time, string activityType, string operatorId, string detailsOfActivity, string workstationNumber)
        {
            bool isSaved = false;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@LineId", line));
            prmList.Add(new FloorSqlParameter("@OperatorId", operatorId));
            prmList.Add(new FloorSqlParameter("@ActivityTypeId", activityType));
            prmList.Add(new FloorSqlParameter("@ActivityDetails", detailsOfActivity));
            prmList.Add(new FloorSqlParameter("@StartDate", date));
            prmList.Add(new FloorSqlParameter("@StartTime", time));
            prmList.Add(new FloorSqlParameter("@WorkStationumber", workstationNumber));
      
            if (FloorDBAccess.ExecuteNonQuery("usp_ProductionLineActivity_Save", prmList) > Constants.ZERO)
            {
                isSaved = true;
            }
            return isSaved;
        }


        public static bool UpdateProductionActivity(string line, DateTime date, string time, string activityType, string operatorId, string detailsOfActivity, string workstationNumber, int ProductionLineActivityId)
        {
            bool isSaved = false;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@LineId", line));
            prmList.Add(new FloorSqlParameter("@OperatorId", operatorId));
            prmList.Add(new FloorSqlParameter("@ActivityTypeId", activityType));
            prmList.Add(new FloorSqlParameter("@ActivityDetails", detailsOfActivity));
            prmList.Add(new FloorSqlParameter("@StartDate", date));
            prmList.Add(new FloorSqlParameter("@StartTime", time));
            prmList.Add(new FloorSqlParameter("@WorkStationumber", workstationNumber));
            prmList.Add(new FloorSqlParameter("@ProductionLineActivityId", ProductionLineActivityId));

            if (FloorDBAccess.ExecuteNonQuery("usp_ProductionLineActivity_Update", prmList) > Constants.ZERO)
            {
                isSaved = true;
            }
            return isSaved;
        }

        public static DateTime GetLastSpeedDateTime(string line, string glovetype)
        {
            DateTime dt = new DateTime(2017, 4, 1);
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@LineId", line));
            prmList.Add(new FloorSqlParameter("@GloveType", glovetype));
            var result = FloorDBAccess.ExecuteScalar("usp_GloveSpeed_GetLastDateTime", prmList);
            if (result != null)
                dt = Convert.ToDateTime(result);
            return dt;
        }

        /// <summary>
        /// ProductionLineActivity Validation
        /// </summary>
        /// <param name="line"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <param name="activityType"></param>
        /// <returns></returns>
        public static bool ValidateProductionActivity(string line, DateTime date, string time, string activityType)
        {
            bool isSaved = false;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@LineId", line));
            prmList.Add(new FloorSqlParameter("@ActivityTypeId", activityType));
            prmList.Add(new FloorSqlParameter("@StartDate", date));
            prmList.Add(new FloorSqlParameter("@StartTime", time));
            isSaved = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("usp_ProductionLineActivity_Validation", prmList));
            return isSaved;
        }


        #endregion
        #endregion

        #region CR

        /// <summary>
        /// GetPLTimeConfiguration
        /// </summary>
        /// <param name="enumType">enumType</param>
        /// <returns></returns>
        public static int GetPLTimeConfiguration(string enumType)
        {
            List<FloorSqlParameter> fspList = new List<FloorSqlParameter>();
            fspList.Add(new FloorSqlParameter("@enumType", enumType));
            int plTimeConfiguration = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_PLTimeConfiguration", fspList));
            return plTimeConfiguration;
        }

        /// <summary>
        /// Fetches ProductionLineDetails based on LocationId
        /// </summary>
        /// <param name="locationId">locationId</param>
        /// <returns>ProductionLineDetails based on LocationId</returns>
        public static List<ProductionLineDetailsDTO> GetProductionLineDetails(int locationId)
        {
            List<ProductionLineDetailsDTO> productionLineDetailsList = null;
            List<FloorSqlParameter> fspList = new List<FloorSqlParameter>();
            fspList.Add(new FloorSqlParameter("@LocationId", locationId));
            using (DataTable dtProductionLineDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_PL_ProductionLineDetails", fspList))
            {
                if (dtProductionLineDetails != null && dtProductionLineDetails.Rows.Count > Constants.ZERO)
                {
                    productionLineDetailsList = (from DataRow dr in dtProductionLineDetails.Rows
                                                 select new ProductionLineDetailsDTO
                                                 {
                                                     LineId = FloorDBAccess.GetString(dr, "LineId"),
                                                     Formers = FloorDBAccess.GetValue<int>(dr, "Formers"),
                                                     Speed = FloorDBAccess.GetValue<int>(dr, "Speed"),
                                                     Cycle = FloorDBAccess.GetValue<decimal>(dr, "Cycle")
                                                 }).ToList();
                }
            }
            return productionLineDetailsList;
        }

        /// <summary>
        /// GetProductionLogging activity based on productionLineId,fromdate and todate
        /// </summary>
        /// <param name="productionLineId">productionLineId</param>
        /// <param name="fromLineDate">fromLineDate</param>
        /// <param name="toLineDate">toLineDate</param>
        /// <returns>List of productionLogging activities</returns>
        public static List<ProductionLoggingActivitiesDTO> GetProductionLoggingActivity(string productionLineId, DateTime fromLineDate, DateTime toLineDate)
        {
            List<ProductionLoggingActivitiesDTO> productionLoggingActivitiesList = null;
            List<FloorSqlParameter> fspList = new List<FloorSqlParameter>();
            fspList.Add(new FloorSqlParameter("@ProductionLineId", productionLineId));
            fspList.Add(new FloorSqlParameter("@FromLineDate", fromLineDate));
            fspList.Add(new FloorSqlParameter("@ToLineDate", toLineDate));
            using (DataTable dtProductionLoggingActivities = FloorDBAccess.ExecuteDataTable("USP_SEL_PL_ProductionLoggingActivities", fspList))
            {
                if (dtProductionLoggingActivities != null && dtProductionLoggingActivities.Rows.Count > Constants.ZERO)
                {
                    productionLoggingActivitiesList = (from DataRow dr in dtProductionLoggingActivities.Rows
                                                       select new ProductionLoggingActivitiesDTO
                                                       {
                                                           Id = FloorDBAccess.GetValue<int>(dr, "Id"),
                                                           ReasonStartStopId = FloorDBAccess.GetValue<int>(dr, "ReasonStartStopId"),
                                                           ProductionLineId = FloorDBAccess.GetString(dr, "ProductionLineId"),
                                                           LineDate = FloorDBAccess.GetValue<DateTime>(dr, "LineDate"),
                                                           LineTime = FloorDBAccess.GetValue<TimeSpan>(dr, "LineTime"),
                                                           Glove = FloorDBAccess.GetString(dr, "Glove"),
                                                           ActivityType = FloorDBAccess.GetString(dr, "ActivityType"),
                                                           ReasonTypeId = FloorDBAccess.GetValue<int>(dr, "ReasonTypeId"),
                                                           ReasonText = FloorDBAccess.GetString(dr, "ReasonText"),
                                                           Duration = FloorDBAccess.GetString(dr, "Duration"),
                                                           LastModifiedOn = FloorDBAccess.GetValue<DateTime>(dr, "LastModifiedOn"),
                                                           Remarks = FloorDBAccess.GetString(dr, "Remarks"),
                                                           IsBatchInsert = FloorDBAccess.GetValue<Boolean>(dr, "IsBatchInsert"),
                                                           Department = FloorDBAccess.GetString(dr, "Department"),
                                                       }).ToList();
                }
            }
            return productionLoggingActivitiesList;
        }

        /// <summary>
        /// GetLastActivityForProductionLine based on productionLineId
        /// </summary>
        /// <param name="lineId">lineId</param>
        /// <returns>LastActivityForProductionLine</returns>
        public static ProductionLoggingActivitiesDTO GetLastActivityForProductionLine(string lineId, DateTime? currentActivityDate)
        {
            ProductionLoggingActivitiesDTO pla = null;
            DataRow dr = null;
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@ProductionLineId", lineId));
            lstfsp.Add(new FloorSqlParameter("@CurrentActivityDate", currentActivityDate));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_PL_LastActivity", lstfsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dr = dt.Rows[Constants.ZERO];
                    pla = new ProductionLoggingActivitiesDTO();
                    pla.Id = FloorDBAccess.GetValue<int>(dr, "Id");
                    pla.ProductionLineId = FloorDBAccess.GetString(dr, "ProductionLineId");
                    pla.LineDate = FloorDBAccess.GetValue<DateTime>(dr, "LineDate");
                    pla.LineTime = FloorDBAccess.GetValue<TimeSpan>(dr, "LineTime");
                    pla.ActivityType = FloorDBAccess.GetString(dr, "ActivityType");
                }
            }
            return pla;
        }

        public static ProductionLoggingActivitiesDTO GetLastActivityForProductionLine_Edit(int lineId, DateTime? currentActivityDate)
        {
            ProductionLoggingActivitiesDTO pla = null;
            DataRow dr = null;
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@ProductionLineId", lineId));
            lstfsp.Add(new FloorSqlParameter("@CurrentActivityDate", currentActivityDate));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_PL_LastActivity_Edit", lstfsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dr = dt.Rows[Constants.ZERO];
                    pla = new ProductionLoggingActivitiesDTO();
                    pla.Id = FloorDBAccess.GetValue<int>(dr, "Id");
                    pla.ProductionLineId = FloorDBAccess.GetString(dr, "ProductionLineId");
                    pla.ReasonStartStopId = FloorDBAccess.GetValue<int>(dr, "ReasonStartStopId");
                    pla.LineDate = FloorDBAccess.GetValue<DateTime>(dr, "LineDate");
                    pla.LineTime = FloorDBAccess.GetValue<TimeSpan>(dr, "LineTime");
                    pla.ActivityType = FloorDBAccess.GetString(dr, "ActivityType");
                    pla.ReasonTypeId = FloorDBAccess.GetValue<int>(dr, "ReasonTypeId");
                }
            }
            return pla;
        }

        /// <summary>
        /// GetNextActivityForProductionLine based on productionLineId
        /// </summary>
        /// <param name="lineId">lineId</param>
        /// <returns>NextActivityForProductionLine</returns>
        public static ProductionLoggingActivitiesDTO GetNextActivityForProductionLine(string lineId, DateTime? currentActivityDate)
        {
            ProductionLoggingActivitiesDTO pla = null;
            DataRow dr = null;
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@ProductionLineId", lineId));
            lstfsp.Add(new FloorSqlParameter("@CurrentActivityDate", currentActivityDate));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_PL_NextActivity", lstfsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dr = dt.Rows[Constants.ZERO];
                    pla = new ProductionLoggingActivitiesDTO();
                    pla.Id = FloorDBAccess.GetValue<int>(dr, "Id");
                    pla.ProductionLineId = FloorDBAccess.GetString(dr, "ProductionLineId");
                    pla.LineDate = FloorDBAccess.GetValue<DateTime>(dr, "LineDate");
                    pla.LineTime = FloorDBAccess.GetValue<TimeSpan>(dr, "LineTime");
                    pla.ActivityType = FloorDBAccess.GetString(dr, "ActivityType");
                }
            }
            return pla;
        }

        /// <summary>
        /// GetProductionLineForStart
        /// </summary>
        /// <param name="lineId">lineId</param>
        /// <returns>ProductionLine details for Start activity</returns>
        public static ProductionLineDetailsDTO GetProductionLineForStart(string lineId)
        {
            ProductionLineDetailsDTO pld = null;
            DataRow dr = null;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@ProductionLineId", lineId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_PL_PRODUCTIONLINE_FOR_START", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dr = dt.Rows[Constants.ZERO];
                    pld = new ProductionLineDetailsDTO();
                    pld.Id = FloorDBAccess.GetValue<int>(dr, "ProductionLineId");
                    pld.LineId = FloorDBAccess.GetString(dr, "LineId");
                    pld.LTGloveType = FloorDBAccess.GetString(dr, "LTGloveType");
                    pld.LTAltGlove = FloorDBAccess.GetString(dr, "LTAltGlove");
                    pld.LTGloveSize = FloorDBAccess.GetString(dr, "LTGloveSize");
                    pld.LBGloveType = FloorDBAccess.GetString(dr, "LBGloveType");
                    pld.LBAltGlove = FloorDBAccess.GetString(dr, "LBAltGlove");
                    pld.LBGloveSize = FloorDBAccess.GetString(dr, "LBGloveSize");
                    pld.RTGloveType = FloorDBAccess.GetString(dr, "RTGloveType");
                    pld.RTAltGlove = FloorDBAccess.GetString(dr, "RTAltGlove");
                    pld.RTGloveSize = FloorDBAccess.GetString(dr, "RTGloveSize");
                    pld.RBGloveType = FloorDBAccess.GetString(dr, "RBGloveType");
                    pld.RBAltGlove = FloorDBAccess.GetString(dr, "RBAltGlove");
                    pld.RBGloveSize = FloorDBAccess.GetString(dr, "RBGloveSize");
                    pld.Formers = FloorDBAccess.GetValue<int>(dr, "Formers");
                    pld.Speed = FloorDBAccess.GetValue<int>(dr, "Speed");
                    pld.Cycle = FloorDBAccess.GetValue<decimal>(dr, "Cycle");
                    pld.IsDoubleFormer = FloorDBAccess.GetValue<bool>(dr, "IsDoubleFormer");
                    pld.InValid = FloorDBAccess.GetValue<int>(dr, "InValid");
                }
            }
            return pld;
        }

        /// <summary>
        /// Get Reason
        /// </summary>
        /// <param name="reasonTypeId">Reason Type</param>
        /// <returns>get Reasons based on ReasonType, it returns null if no values found for the specified reasonTypeId</returns>
        public static List<DropdownDTO> GetReasonTextByReasonTypeId(int reasonTypeId)
        {
            List<DropdownDTO> reasonTextList = null;
            List<FloorSqlParameter> lstFsp = null;
            reasonTextList = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@ReasonTypeId", reasonTypeId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_Reasons", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    reasonTextList = (from DataRow row in dt.Rows
                                      select new DropdownDTO
                                      {
                                          DisplayField = FloorDBAccess.GetString(row, "ReasonText"),
                                          IDField = FloorDBAccess.GetString(row, "ReasonId"),
                                          SelectedValue = FloorDBAccess.GetString(row, "ReasonId")
                                      }).ToList();
                }
            }
            return reasonTextList;
        }

        /// <summary>
        /// Get Glove Size For GloveType and LineNumber
        /// </summary>
        /// <param name="gloveType"></param>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public static List<GloveSizeDTO> GetGloveSizeForGloveType(string gloveType, string lineNumber)
        {
            List<GloveSizeDTO> sizeList = null;
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@GloveType", gloveType));
            lstfsp.Add(new FloorSqlParameter("@LineNumber", lineNumber));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveSize", lstfsp))
            {
                if (dt != null && dt.Rows.Count != Constants.ZERO)
                {
                    sizeList = (from DataRow row in dt.Rows
                                select new GloveSizeDTO
                                {
                                    Size = FloorDBAccess.GetString(row, "Configuration")
                                }).ToList();
                }
            }
            return sizeList;
        }

        /// <summary>
        /// GetProductionLine for stop
        /// </summary>
        /// <param name="lineId">lineId</param>
        /// <returns>ProductionLine details for STOP activity</returns>
        public static ProductionLineDetailsDTO GetProductionLineForStop(string lineId)
        {
            ProductionLineDetailsDTO pld = null;
            DataRow dr = null;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@ProductionLineId", lineId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_PL_PRODUCTIONLINE_FOR_STOP", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dr = dt.Rows[Constants.ZERO];
                    pld = new ProductionLineDetailsDTO();
                    pld.Id = FloorDBAccess.GetValue<int>(dr, "ProductionLineId");
                    pld.LineId = FloorDBAccess.GetString(dr, "LineId");
                    pld.Formers = FloorDBAccess.GetValue<int>(dr, "Formers");
                    pld.Speed = FloorDBAccess.GetValue<int>(dr, "Speed");
                    pld.Cycle = FloorDBAccess.GetValue<decimal>(dr, "Cycle");
                }
            }
            return pld;
        }

        /// <summary>
        /// Save Line Control details from line maintainence screen
        /// </summary>
        /// <param name="lineId">lineId</param>
        /// <param name="former">former</param>
        /// <param name="speed">speed</param>
        /// <param name="cycle">cycle</param>
        /// <returns></returns>
        public static int SaveLineControlDetails(string lineId, int former, int speed, float cycle)
        {
            List<FloorSqlParameter> lstfsp = null;
            int noofrows = Constants.ZERO;
            lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@LineId", lineId));
            lstfsp.Add(new FloorSqlParameter("@Formers", former));
            lstfsp.Add(new FloorSqlParameter("@Speed", speed));
            lstfsp.Add(new FloorSqlParameter("@Cycle", cycle));
            noofrows = FloorDBAccess.ExecuteNonQuery("USP_UPD_PL_LineControlMaintenance", lstfsp);
            return noofrows;
        }

        public static List<DropdownDTO> GetPLDepartment()
        {
            List<DropdownDTO> departmentList = null;

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_PL_ProductionLoggingDepartment"))
            {
                try
                {
                    if (dt.Rows.Count > Constants.ZERO)
                    {
                        departmentList = (from DataRow row in dt.Rows
                                    select new DropdownDTO
                                    {
                                        DisplayField = FloorDBAccess.GetString(row, "DepartmentText"),
                                        IDField = FloorDBAccess.GetString(row, "DepartmentText")
                                        //SelectedValue = FloorDBAccess.GetString(row, "Value")
                                    }).ToList();
                    }
                }
                catch (ArgumentException argex)
                {
                    throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETPLDEPARTMENTEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return departmentList;
        }

        #region OLD SaveProductionLoggingActivityStart
        /*
        /// <summary>
        /// Inserts records and ProductionLine and ProductionLoggingActivity table for START activity
        /// </summary>
        /// <param name="lineId">lineId</param>
        /// <param name="ltGloveType">ltGloveType</param>
        /// <param name="ltAltGlove">ltAltGlove</param>
        /// <param name="ltGloveSize">ltGloveSize</param>
        /// <param name="lbGloveType">lbGloveType</param>
        /// <param name="lbAltGlove">lbAltGlove</param>
        /// <param name="lbGloveSize">lbGloveSize</param>
        /// <param name="rtGloveType">rtGloveType</param>
        /// <param name="rtAltGlove">rtAltGlove</param>
        /// <param name="rtGloveSize">rtGloveSize</param>
        /// <param name="rbGloveType">rbGloveType</param>
        /// <param name="rbAltGlove">rbAltGlove</param>
        /// <param name="rbGloveSize">rbGloveSize</param>
        /// <param name="lineDate">lineDate</param>
        /// <param name="lineTime">lineTime</param>
        /// <param name="activityType">activityType</param>
        /// <param name="reasonId">reasonId</param>
        /// <param name="duration">duration</param>
        /// <param name="lastModifiedOn">lastModifiedOn</param>
        /// <returns></returns>
        public static int SaveProductionLoggingActivityStart(string lineId, string ltGloveType, string ltAltGlove, string ltGloveSize, string lbGloveType,
            string lbAltGlove, string lbGloveSize, string rtGloveType, string rtAltGlove, string rtGloveSize, string rbGloveType, string rbAltGlove, string rbGloveSize,
            DateTime lineDate, TimeSpan lineTime, string activityType, int reasonId, string duration, string workStationId, bool isUpdateNextActivity, string updateDuration)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            int noofrows = Constants.ZERO;
            lstfsp.Add(new FloorSqlParameter("@LineId", lineId));
            lstfsp.Add(new FloorSqlParameter("@ltGloveType", ltGloveType));
            lstfsp.Add(new FloorSqlParameter("@ltAltGlove", ltAltGlove));
            lstfsp.Add(new FloorSqlParameter("@ltGloveSize", ltGloveSize));
            lstfsp.Add(new FloorSqlParameter("@lbGloveType", lbGloveType));
            lstfsp.Add(new FloorSqlParameter("@lbAltGlove", lbAltGlove));
            lstfsp.Add(new FloorSqlParameter("@lbGloveSize", lbGloveSize));
            lstfsp.Add(new FloorSqlParameter("@rtGloveType", rtGloveType));
            lstfsp.Add(new FloorSqlParameter("@rtAltGlove", rtAltGlove));
            lstfsp.Add(new FloorSqlParameter("@rtGloveSize", rtGloveSize));
            lstfsp.Add(new FloorSqlParameter("@rbGloveType", rbGloveType));
            lstfsp.Add(new FloorSqlParameter("@rbAltGlove", rbAltGlove));
            lstfsp.Add(new FloorSqlParameter("@rbGloveSize", rbGloveSize));
            lstfsp.Add(new FloorSqlParameter("@lineDate", lineDate));
            lstfsp.Add(new FloorSqlParameter("@lineTime", lineTime));
            lstfsp.Add(new FloorSqlParameter("@activityType", activityType));
            lstfsp.Add(new FloorSqlParameter("@reasonId", null));
            lstfsp.Add(new FloorSqlParameter("@duration", duration));
            lstfsp.Add(new FloorSqlParameter("@WorkStationId", workStationId));
            lstfsp.Add(new FloorSqlParameter("@IsUpdateNextActivity", isUpdateNextActivity));
            lstfsp.Add(new FloorSqlParameter("@UpdateDuration", updateDuration));
            noofrows = FloorDBAccess.ExecuteNonQuery("USP_SAV_PL_ProductionLoggingActivity_START", lstfsp);
            return noofrows;
        }
        */
        #endregion

        /// <summary>
        /// Inserts records and ProductionLine and ProductionLoggingActivity table for START activity
        /// </summary>
        /// <param name="lineId">lineId</param>
        /// <param name="ltGloveType">ltGloveType</param>
        /// <param name="ltAltGlove">ltAltGlove</param>
        /// <param name="ltGloveSize">ltGloveSize</param>
        /// <param name="lbGloveType">lbGloveType</param>
        /// <param name="lbAltGlove">lbAltGlove</param>
        /// <param name="lbGloveSize">lbGloveSize</param>
        /// <param name="rtGloveType">rtGloveType</param>
        /// <param name="rtAltGlove">rtAltGlove</param>
        /// <param name="rtGloveSize">rtGloveSize</param>
        /// <param name="rbGloveType">rbGloveType</param>
        /// <param name="rbAltGlove">rbAltGlove</param>
        /// <param name="rbGloveSize">rbGloveSize</param>
        /// <param name="lineDate">lineDate</param>
        /// <param name="lineTime">lineTime</param>
        /// <param name="activityType">activityType</param>
        /// <param name="reasonId">reasonId</param>
        /// <param name="duration">duration</param>
        /// <param name="lastModifiedOn">lastModifiedOn</param>
        /// <returns></returns>
        public static int SaveProductionLoggingActivityStart(string lineId, string ltGloveType, string ltAltGlove, string ltGloveSize, string lbGloveType,
            string lbAltGlove, string lbGloveSize, string rtGloveType, string rtAltGlove, string rtGloveSize, string rbGloveType, string rbAltGlove, string rbGloveSize,
            DateTime lineDate, TimeSpan lineTime, string activityType, int reasonId, string duration, string workStationId, bool isUpdateNextActivity, string updateDuration, int speed = 0, 
            string downtimeDuration = null)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            int noofrows = Constants.ZERO;
            lstfsp.Add(new FloorSqlParameter("@LineId", lineId));
            lstfsp.Add(new FloorSqlParameter("@ltGloveType", ltGloveType));
            lstfsp.Add(new FloorSqlParameter("@ltAltGlove", ltAltGlove));
            lstfsp.Add(new FloorSqlParameter("@ltGloveSize", ltGloveSize));
            lstfsp.Add(new FloorSqlParameter("@lbGloveType", lbGloveType));
            lstfsp.Add(new FloorSqlParameter("@lbAltGlove", lbAltGlove));
            lstfsp.Add(new FloorSqlParameter("@lbGloveSize", lbGloveSize));
            lstfsp.Add(new FloorSqlParameter("@rtGloveType", rtGloveType));
            lstfsp.Add(new FloorSqlParameter("@rtAltGlove", rtAltGlove));
            lstfsp.Add(new FloorSqlParameter("@rtGloveSize", rtGloveSize));
            lstfsp.Add(new FloorSqlParameter("@rbGloveType", rbGloveType));
            lstfsp.Add(new FloorSqlParameter("@rbAltGlove", rbAltGlove));
            lstfsp.Add(new FloorSqlParameter("@rbGloveSize", rbGloveSize));
            lstfsp.Add(new FloorSqlParameter("@lineDate", lineDate));
            lstfsp.Add(new FloorSqlParameter("@lineTime", lineTime));
            lstfsp.Add(new FloorSqlParameter("@activityType", activityType));
            lstfsp.Add(new FloorSqlParameter("@reasonId", null));
            lstfsp.Add(new FloorSqlParameter("@duration", duration));
            lstfsp.Add(new FloorSqlParameter("@WorkStationId", workStationId));
            lstfsp.Add(new FloorSqlParameter("@IsUpdateNextActivity", isUpdateNextActivity));
            lstfsp.Add(new FloorSqlParameter("@UpdateDuration", updateDuration));
            lstfsp.Add(new FloorSqlParameter("@Speed", speed));
            lstfsp.Add(new FloorSqlParameter("@DownDuration", downtimeDuration));
            noofrows = FloorDBAccess.ExecuteNonQuery("USP_SAV_PL_ProductionLoggingActivity_START", lstfsp);
            return noofrows;
        }


        public static int UpdateProductionLoggingActivityStop(DateTime lineDate, TimeSpan lineTime, int ProductionLoggingActivityid,
            int reasonId, int reasontypeid, string duration, string ActivityType, string department = null)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            int noofrows = Constants.ZERO;

            lstfsp.Add(new FloorSqlParameter("@lineDate", lineDate));
            lstfsp.Add(new FloorSqlParameter("@lineTime", lineTime));
            lstfsp.Add(new FloorSqlParameter("@ProductionLoggingActivityid", ProductionLoggingActivityid));
            lstfsp.Add(new FloorSqlParameter("@reasonId", reasonId));
            lstfsp.Add(new FloorSqlParameter("@reasontypeid", reasontypeid));
            lstfsp.Add(new FloorSqlParameter("@Duration", duration));
            lstfsp.Add(new FloorSqlParameter("@ActivityType", ActivityType));
            lstfsp.Add(new FloorSqlParameter("@Department", department));

            noofrows = FloorDBAccess.ExecuteNonQuery("USP_UPDATE_PL_ProductionLoggingActivity_STOP", lstfsp);
            return noofrows;
        }

        public static int UpdateProductionLoggingActivityStart(DateTime lineDate, TimeSpan lineTime,
            int ProductionLoggingActivityid, int reasonId, string ActivityType, string downtimeDuration)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            int noofrows = Constants.ZERO;

            lstfsp.Add(new FloorSqlParameter("@lineDate", lineDate));
            lstfsp.Add(new FloorSqlParameter("@lineTime", lineTime));
            lstfsp.Add(new FloorSqlParameter("@ProductionLoggingActivityid", ProductionLoggingActivityid));
            lstfsp.Add(new FloorSqlParameter("@reasonId", reasonId));
            lstfsp.Add(new FloorSqlParameter("@ActivityType", ActivityType));
            lstfsp.Add(new FloorSqlParameter("@DownDuration", downtimeDuration));

            noofrows = FloorDBAccess.ExecuteNonQuery("USP_UPDATE_PL_ProductionLoggingActivity_START", lstfsp);
            return noofrows;
        }

        public static int UpdateProductionLoggingActivityStart_Audit(string audLog)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            int noofrows = Constants.ZERO;

            lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@xmlLog", audLog));
            //noofrows = FloorDBAccess.ExecuteNonQuery("USP_Insert_AuditLog_ProductionStartStop", lstfsp);
            noofrows = FloorDBAccess.ExecuteNonQuery("USP_Insert_AuditLog", lstfsp);
            return noofrows;
        }


        #region Old SaveProductionLoggingActivityStart
        /*
        /// <summary>
        /// Inserts records and ProductionLine and ProductionLoggingActivity table for START activity
        /// </summary>
        /// <param name="lineId">lineId</param>
        /// <param name="ltGloveType">ltGloveType</param>
        /// <param name="ltAltGlove">ltAltGlove</param>
        /// <param name="ltGloveSize">ltGloveSize</param>
        /// <param name="rtGloveType">rtGloveType</param>
        /// <param name="rtAltGlove">rtAltGlove</param>
        /// <param name="rtGloveSize">rtGloveSize</param>
        /// <param name="lineDate">lineDate</param>
        /// <param name="lineTime">lineTime</param>
        /// <param name="activityType">activityType</param>
        /// <param name="reasonId">reasonId</param>
        /// <param name="duration">duration</param>
        /// <param name="lastModifiedOn">lastModifiedOn</param>
        /// <returns></returns>
        public static int SaveProductionLoggingActivityStart(string lineId, string ltGloveType, string ltAltGlove, string ltGloveSize,
            string rtGloveType, string rtAltGlove, string rtGloveSize, DateTime lineDate, TimeSpan lineTime, string activityType,
            int reasonId, string duration, string workStationId, bool isUpdateNextActivity, string updateDuration)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            int noofrows = Constants.ZERO;
            lstfsp.Add(new FloorSqlParameter("@LineId", lineId));
            lstfsp.Add(new FloorSqlParameter("@ltGloveType", ltGloveType));
            lstfsp.Add(new FloorSqlParameter("@ltAltGlove", ltAltGlove));
            lstfsp.Add(new FloorSqlParameter("@ltGloveSize", ltGloveSize));
            lstfsp.Add(new FloorSqlParameter("@rtGloveType", rtGloveType));
            lstfsp.Add(new FloorSqlParameter("@rtAltGlove", rtAltGlove));
            lstfsp.Add(new FloorSqlParameter("@rtGloveSize", rtGloveSize));
            lstfsp.Add(new FloorSqlParameter("@lineDate", lineDate));
            lstfsp.Add(new FloorSqlParameter("@lineTime", lineTime));
            lstfsp.Add(new FloorSqlParameter("@activityType", activityType));
            lstfsp.Add(new FloorSqlParameter("@reasonId", null));
            lstfsp.Add(new FloorSqlParameter("@duration", duration));
            lstfsp.Add(new FloorSqlParameter("@WorkStationId", workStationId));
            lstfsp.Add(new FloorSqlParameter("@IsUpdateNextActivity", isUpdateNextActivity));
            lstfsp.Add(new FloorSqlParameter("@UpdateDuration", updateDuration));
            noofrows = FloorDBAccess.ExecuteNonQuery("USP_SAV_PL_ProductionLoggingActivity_START_FOR_NON_DOUBLEFORMER_LINE", lstfsp);
            return noofrows;
        }
        */
        #endregion

        /// <summary>
        /// Inserts records and ProductionLine and ProductionLoggingActivity table for START activity
        /// </summary>
        /// <param name="lineId">lineId</param>
        /// <param name="ltGloveType">ltGloveType</param>
        /// <param name="ltAltGlove">ltAltGlove</param>
        /// <param name="ltGloveSize">ltGloveSize</param>
        /// <param name="rtGloveType">rtGloveType</param>
        /// <param name="rtAltGlove">rtAltGlove</param>
        /// <param name="rtGloveSize">rtGloveSize</param>
        /// <param name="lineDate">lineDate</param>
        /// <param name="lineTime">lineTime</param>
        /// <param name="activityType">activityType</param>
        /// <param name="reasonId">reasonId</param>
        /// <param name="duration">duration</param>
        /// <param name="lastModifiedOn">lastModifiedOn</param>
        /// <returns></returns>
        public static int SaveProductionLoggingActivityStart(string lineId, string ltGloveType, string ltAltGlove, string ltGloveSize,
            string rtGloveType, string rtAltGlove, string rtGloveSize, DateTime lineDate, TimeSpan lineTime, string activityType,
            int reasonId, string duration, string workStationId, bool isUpdateNextActivity, string updateDuration, int speed = 0, 
            string downtimeDuration = null)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            int noofrows = Constants.ZERO;
            lstfsp.Add(new FloorSqlParameter("@LineId", lineId));
            lstfsp.Add(new FloorSqlParameter("@ltGloveType", ltGloveType));
            lstfsp.Add(new FloorSqlParameter("@ltAltGlove", ltAltGlove));
            lstfsp.Add(new FloorSqlParameter("@ltGloveSize", ltGloveSize));
            lstfsp.Add(new FloorSqlParameter("@rtGloveType", rtGloveType));
            lstfsp.Add(new FloorSqlParameter("@rtAltGlove", rtAltGlove));
            lstfsp.Add(new FloorSqlParameter("@rtGloveSize", rtGloveSize));
            lstfsp.Add(new FloorSqlParameter("@lineDate", lineDate));
            lstfsp.Add(new FloorSqlParameter("@lineTime", lineTime));
            lstfsp.Add(new FloorSqlParameter("@activityType", activityType));
            lstfsp.Add(new FloorSqlParameter("@reasonId", null));
            lstfsp.Add(new FloorSqlParameter("@duration", duration));
            lstfsp.Add(new FloorSqlParameter("@WorkStationId", workStationId));
            lstfsp.Add(new FloorSqlParameter("@IsUpdateNextActivity", isUpdateNextActivity));
            lstfsp.Add(new FloorSqlParameter("@UpdateDuration", updateDuration));
            lstfsp.Add(new FloorSqlParameter("@Speed", speed));
            lstfsp.Add(new FloorSqlParameter("@DownDuration", downtimeDuration));
            noofrows = FloorDBAccess.ExecuteNonQuery("USP_SAV_PL_ProductionLoggingActivity_START_FOR_NON_DOUBLEFORMER_LINE", lstfsp);
            return noofrows;
        }

        #region old SaveProductionLoggingActivityStop
        /*
        /// <summary>
        /// Inserts records and ProductionLine and ProductionLoggingActivity table for STOP activity
        /// </summary>
        /// <param name="lineId">lineId</param>
        /// <param name="lineDate">lineDate</param>
        /// <param name="lineTime">lineTime</param>
        /// <param name="activityType">activityType</param>
        /// <param name="reasonId">reasonId</param>
        /// <param name="duration">duration</param>
        /// <param name="isPLUpdateRequired">isPLUpdateRequired</param>
        /// <returns></returns>
        public static int SaveProductionLoggingActivityStop(string lineId, DateTime lineDate, TimeSpan lineTime, string activityType,
            int reasonId, string duration, bool isPLUpdateRequired, string workStationId)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            int noofrows = Constants.ZERO;
            lstfsp.Add(new FloorSqlParameter("@LineId", lineId));
            lstfsp.Add(new FloorSqlParameter("@lineDate", lineDate));
            lstfsp.Add(new FloorSqlParameter("@lineTime", lineTime));
            lstfsp.Add(new FloorSqlParameter("@activityType", activityType));
            lstfsp.Add(new FloorSqlParameter("@reasonId", reasonId));
            lstfsp.Add(new FloorSqlParameter("@duration", duration));
            lstfsp.Add(new FloorSqlParameter("@isPLUpdateRequired", isPLUpdateRequired));
            lstfsp.Add(new FloorSqlParameter("@WorkStationId", workStationId));
            noofrows = FloorDBAccess.ExecuteNonQuery("USP_SAV_PL_ProductionLoggingActivity_STOP", lstfsp);
            return noofrows;
        }
        */
        #endregion

        /// <summary>
        /// Inserts records and ProductionLine and ProductionLoggingActivity table for STOP activity
        /// </summary>
        /// <param name="lineId">lineId</param>
        /// <param name="lineDate">lineDate</param>
        /// <param name="lineTime">lineTime</param>
        /// <param name="activityType">activityType</param>
        /// <param name="reasonId">reasonId</param>
        /// <param name="duration">duration</param>
        /// <param name="isPLUpdateRequired">isPLUpdateRequired</param>
        /// <param name="workStationId">workStationId</param>
        /// <param name="remarks">remarks (Optional)</param>
        /// <param name="speed">speed (Optional)</param>
        /// <param name="department">department</param>
        /// <returns></returns>
        public static int SaveProductionLoggingActivityStop(string lineId, DateTime lineDate, TimeSpan lineTime, string activityType,
            int reasonId, string duration, bool isPLUpdateRequired, string workStationId, string department, string remarks = "", int speed = 0)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            int noofrows = Constants.ZERO;
            lstfsp.Add(new FloorSqlParameter("@LineId", lineId));
            lstfsp.Add(new FloorSqlParameter("@lineDate", lineDate));
            lstfsp.Add(new FloorSqlParameter("@lineTime", lineTime));
            lstfsp.Add(new FloorSqlParameter("@activityType", activityType));
            lstfsp.Add(new FloorSqlParameter("@reasonId", reasonId));
            lstfsp.Add(new FloorSqlParameter("@duration", duration));
            lstfsp.Add(new FloorSqlParameter("@isPLUpdateRequired", isPLUpdateRequired));
            lstfsp.Add(new FloorSqlParameter("@WorkStationId", workStationId));

            if (remarks == "")
                lstfsp.Add(new FloorSqlParameter("@Remarks", DBNull.Value));
            else
                lstfsp.Add(new FloorSqlParameter("@Remarks", remarks));

            lstfsp.Add(new FloorSqlParameter("@Speed", speed));
            lstfsp.Add(new FloorSqlParameter("@Department", department));

            noofrows = FloorDBAccess.ExecuteNonQuery("USP_SAV_PL_ProductionLoggingActivity_STOP", lstfsp);
            return noofrows;
        }


        #endregion

        #region Custom Implementation NGC OEE

        /// <summary>
        /// Check if Reason allowed remark to be inserted
        /// </summary>
        /// <param name="ReasonId">ReasonI</param>
        /// <returns></returns>
        public static bool CheckReasonRemark(int ReasonId)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@ReasonId", ReasonId));
            return (bool)FloorDBAccess.ExecuteScalar("[OEE_SP_CheckRemark]", lstfsp);
        }

        /// <summary>
        /// Check current Line Speed
        /// </summary>
        /// <param name="LineId">LineId</param>
        /// <returns>Line Speed</returns>
        public static int GetLineSpeed(string LineId)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@LineId", LineId));
            return (int)FloorDBAccess.ExecuteScalar("[OEE_SP_CheckLineSpeed]", lstfsp);
        }

        /// <summary>
        /// Get Reason by Area ID
        /// </summary>
        /// <param name="areaID">LineId</param>
        /// <returns>Returns list of reason to load into drop down list</returns>
        public static List<DropdownDTO> GetReasonTextByArea(int areaID)
        {
            List<DropdownDTO> reasonTextList = null;
            List<FloorSqlParameter> lstFsp = null;
            reasonTextList = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@AreaID", areaID));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_ReasonsByArea", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    reasonTextList = (from DataRow row in dt.Rows
                                      select new DropdownDTO
                                      {
                                          DisplayField = FloorDBAccess.GetString(row, "ReasonText"),
                                          IDField = FloorDBAccess.GetString(row, "ReasonId"),
                                          SelectedValue = FloorDBAccess.GetString(row, "ReasonId")
                                      }).ToList();
                }
            }
            return reasonTextList;
        }

        /// <summary>
        /// Gets list of areas
        /// </summary>
        /// <returns>Returns list of areas to load into drop down list</returns>
        public static List<DropdownDTO> GetArea()
        {
            List<DropdownDTO> areaList = null;

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_Area"))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    areaList = (from DataRow row in dt.Rows
                                select new DropdownDTO
                                {
                                    DisplayField = FloorDBAccess.GetString(row, "Description"),
                                    IDField = FloorDBAccess.GetString(row, "AreaID"),
                                    SelectedValue = FloorDBAccess.GetString(row, "AreaID")
                                }).ToList();
                }
            }

            return areaList;
        }
        #endregion

        #region Custom Implementation for LineSpeedControl
        /// <summary>
        /// Get List of production line based on Location id
        /// </summary>
        /// <param name="locationId">LocationId</param>
        /// <returns>List of Production Lines</returns>
        public static List<LineDTO> GetLineList(int locationId)
        {
            List<LineDTO> lineList = null;
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@LocationId", locationId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_LineMaster", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    lineList = (from DataRow row in dt.Rows
                                select new LineDTO
                                {
                                    LineNumber = FloorDBAccess.GetString(row, "LineNumber")
                                }).ToList();
                }
            }
            return lineList;
        }

        /// <summary>
        /// LineSpeedControl - Get all GloveCode list
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public static List<GloveCodeDTO> GetGloveCodeList()
        {
            List<GloveCodeDTO> lineList = null;
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveCodeDetails", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    lineList = (from DataRow row in dt.Rows
                                select new GloveCodeDTO
                                {
                                    GloveCode = FloorDBAccess.GetString(row, "GLOVECODE")
                                }).ToList();
                }
            }
            return lineList;
        }

        /// <summary>
        /// LineSpeedControl - Get GloveCode list by Line
        /// </summary>
        /// <returns></returns>
        public static List<GloveCodeDTO> GetGloveCodeListByLine(string locationId)
        {
            List<GloveCodeDTO> lineList = null;
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@ProductionLineId", locationId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GetGloveCodeListByLine", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    lineList = (from DataRow row in dt.Rows
                                select new GloveCodeDTO
                                {
                                    GloveCode = FloorDBAccess.GetString(row, "GLOVECODE")
                                }).ToList();
                }
            }
            return lineList;
        }

        /// <summary>
        ///  LineSpeedControl - Get glove data based on line and glove code
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="GloveCode"></param>
        /// <returns></returns>
        public static DataTable GetGloveData(string LineId, string GloveCode)
        {
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@LineId", LineId));
            lstFsp.Add(new FloorSqlParameter("@GloveCode", GloveCode));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GetGloveDataLineSpeedControl", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    return dt;
                }
            }
            return null;
        }

        /// <summary>
        ///  LineSpeedControl - Get glove size
        /// </summary>
        /// <param name="GloveCode"></param>
        /// <returns></returns>
        public static List<SizeDTO> GetGloveSize(string GloveCode)
        {
            List<SizeDTO> lineList = null;
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GloveCode", GloveCode));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GetGloveSize", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    lineList = (from DataRow row in dt.Rows
                                select new SizeDTO
                                {
                                    Size = FloorDBAccess.GetString(row, "COMMONSIZE")
                                }).ToList();
                }
            }
            return lineList;
        }

        /// <summary>
        ///  LineSpeedControl - Insert data
        /// </summary>
        /// <param name="Line"></param>
        /// <param name="GloveCode"></param>
        /// <param name="LineSpeed"></param>
        /// <param name="EffectiveDateTime"></param>
        /// <param name="WorkstationId"></param>
        /// <returns></returns>
        public static bool SaveLineSpeedData(string Line, string GloveCode, int LineSpeed, DateTime EffectiveDateTime, int WorkstationId)
        {
            bool isSaved = false;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@Line", Line));
            prmList.Add(new FloorSqlParameter("@GloveCode", GloveCode));
            prmList.Add(new FloorSqlParameter("@LineSpeed", LineSpeed));
            prmList.Add(new FloorSqlParameter("@EffectiveDateTime", EffectiveDateTime));
            prmList.Add(new FloorSqlParameter("@WorkstationId", WorkstationId));

            if (FloorDBAccess.ExecuteNonQuery("usp_LineSpeedControl_Insert", prmList) > Constants.ZERO)
            {
                isSaved = true;
            }
            return isSaved;
        }
        
        /// <summary>
        /// Production Logging - Start - Load glove speed information for all tier
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static ProductionLoggingTierDTO LoadGloveTierSpeed(ProductionLoggingTierDTO dto)
        {
            ProductionLoggingTierDTO pDTO = new ProductionLoggingTierDTO();
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@Line", dto.Line));
            lstFsp.Add(new FloorSqlParameter("@LineDate", dto.LineDate));
            lstFsp.Add(new FloorSqlParameter("@LTGlove", dto.LTGlove));

            if (dto.LBGlove != string.Empty)
                lstFsp.Add(new FloorSqlParameter("@LBGlove", dto.LBGlove));
            if (dto.RTGlove != string.Empty)
                lstFsp.Add(new FloorSqlParameter("@RTGlove", dto.RTGlove));
            if (dto.RBGlove != string.Empty)
                lstFsp.Add(new FloorSqlParameter("@RBGlove", dto.RBGlove));

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_LineSpeed", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        pDTO.Line = FloorDBAccess.GetString(dr, "Line");
                        pDTO.LTGlove = FloorDBAccess.GetString(dr, "LTGlove");
                        pDTO.LTSpeed = FloorDBAccess.GetValue<int>(dr, "LTSpeed");
                        pDTO.ConflictSpeed = FloorDBAccess.GetValue<bool>(dr, "ConflictSpeed");
                        pDTO.LowestSpeed = FloorDBAccess.GetValue<int>(dr, "LowestSpeed");
                        if (dto.LBGlove != string.Empty)
                        {
                            pDTO.LBGlove = FloorDBAccess.GetString(dr, "LBGlove");
                            pDTO.LBSpeed = FloorDBAccess.GetValue<int>(dr, "LBSpeed");
                        }
                        if (dto.RTGlove != string.Empty)
                        {
                            pDTO.RTGlove = FloorDBAccess.GetString(dr, "RTGlove");
                            pDTO.RTSpeed = FloorDBAccess.GetValue<int>(dr, "RTSpeed");
                        }
                        if (dto.RBGlove != string.Empty)
                        {
                            pDTO.RBGlove = FloorDBAccess.GetString(dr, "RBGlove");
                            pDTO.RBSpeed = FloorDBAccess.GetValue<int>(dr, "RBSpeed");
                        }
                    }
                }
            }
            return pDTO;
        }

        /// <summary>
        /// Get glove speed by line
        /// </summary>
        /// <param name="glove"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static int GetGloveSpeedByLine(string glove, string line, DateTime dateTime)
        {
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@line", line));
            lstFsp.Add(new FloorSqlParameter("@glove", glove));
            lstFsp.Add(new FloorSqlParameter("@dateTime", dateTime));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_SEL_GloveSpeed", lstFsp));
        }

        public static int SaveLineSpeed(string line, int speed)
        {
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@line", line));
            lstFsp.Add(new FloorSqlParameter("@speed", speed));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_SAV_LineSpeed", lstFsp));
        }
        #endregion

        public static int Delete_ProductionLoggingActivity(int ProductionLoggingActivityId)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@ProductionLoggingActivityId", ProductionLoggingActivityId));
            return (int)FloorDBAccess.ExecuteNonQuery("USP_DEL_PL_ProductionLoggingActivity_STARTSTOP", lstfsp);
        }

        public static int GetAreaId(int ReasonId)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@ReasonId", ReasonId));
            return (int)FloorDBAccess.ExecuteScalar("USP_SEL_ReasonArea", lstfsp);
        }
    }
}
