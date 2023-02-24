// -----------------------------------------------------------------------
// <copyright file="SetUpConfigurationBLL.cs" company="Avanade">
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
    /// Set Up Configuration business logic class
    /// </summary>
    public class SetUpConfigurationBLL
    {

        /// <summary>
        /// Get Reasons Modified 
        /// </summary>
        /// <param name="reasonType"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetReasonsDetails(string reasonType)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@reasonType", reasonType));
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_ReasonTextDetailsForReasonType", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "ReasonId"),
                                DisplayField = FloorDBAccess.GetString(row, "ReasonText")
                            }).ToList();
                }
            }
            return list;
        }

        public static List<DropdownDTO> GetEnumMaster(string enumType)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@enumType", enumType));
            List<DropdownDTO> list = null;
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_Enum", PrmList);

            if (dt != null && dt.Rows.Count != 0)
            {
                list = (from DataRow row in dt.Rows
                        select new DropdownDTO
                        {
                            IDField = FloorDBAccess.GetString(row, "Value"),
                            DisplayField = FloorDBAccess.GetString(row, "Text")
                        }).ToList();
            }
            return list;
        }

        public static List<DropdownDTO> GetCommonSize()
        {
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_CommonSizeMaster", null))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "COMMONSIZE"),
                                DisplayField = FloorDBAccess.GetString(row, "COMMONSIZE")
                            }).ToList();
                }
            }
            return list;
        }

        public static List<DropdownDTO> GetInnerLabelSetNo(string fgCode)
        {
            List<DropdownDTO> list = null;

            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@FGCode", fgCode));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_InnerLabelSet", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                //display field changed to inner label set no not description 20171123 
                                IDField = FloorDBAccess.GetString(row, "InnerLabelSetId"),
                                DisplayField = FloorDBAccess.GetString(row, "InnerLabelSetNo")
                            }).ToList();
                }
            }
            return list;

        }

        public static List<DropdownDTO> GetOuterLabelSetNo(string fgCode)
        {
            List<DropdownDTO> list = null;

            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@FGCode", fgCode));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_OuterLabelSet", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                //display field changed to inner label set no not description 20171123 
                                IDField = FloorDBAccess.GetString(row, "OuterLabelSetId"),
                                DisplayField = FloorDBAccess.GetString(row, "OuterLabelSetNo")
                            }).ToList();
                }
            }
            return list;
        }

        public static bool GetInnerLabelCustomDate(string innerLabelSetId)
        {
            bool check = false;
            if (!string.IsNullOrEmpty(innerLabelSetId))
            {
                List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
                PrmList.Add(new FloorSqlParameter("@InnerLabelSetId", innerLabelSetId));
                using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_InnerLabelSetCustomDate", PrmList))
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        check = (from DataRow row in dt.Rows
                                 select FloorDBAccess.GetValue<bool>(row, "CustomDate")).FirstOrDefault();
                    }
                }
            }
            return check;
        }

        public static bool GetOuterLabelCustomDate(string outerLabelSetId)
        {
            bool check = false;
            if (!string.IsNullOrEmpty(outerLabelSetId))
            {
                List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
                PrmList.Add(new FloorSqlParameter("@OuterLabelSetId", outerLabelSetId));
                using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_OuterLabelSetCustomDate", PrmList))
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        check = (from DataRow row in dt.Rows
                                 select FloorDBAccess.GetValue<bool>(row, "CustomDate")).FirstOrDefault();
                    }
                }
            }
            return check;
        }

        #region  Getting Password Through Loggeduserid
        /// <summary>
        ///  GetModuleIdPermissiosnForLoggedInUserPassword
        /// </summary>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
        public static string GetModuleIdForLoggedInUserPassword(string loggedInUser)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_ModuleIdForLoggedInUserPassword", PrmList));
        }
        #endregion

        #region Member Variables
        private static List<FloorSqlParameter> _params;
        #endregion

        #region Main Menu

        /// <summary>
        /// Method to retrieve the module Ids on which the logged in user has access
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetModuleIdForLoggedInUser(string loggedInUser)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_ModuleIdForLoggedInUser", PrmList));
        }
        #endregion

        #region Batch Inquiry

        /// <summary>
        /// Get Batch details
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="serialNumber"></param>
        /// <returns>List<BatchDetails></returns>
        public static List<BatchDetails> GetBatchDetails(DateTime? fromDate, DateTime? toDate, Int64? serialNumber, string batchType)
        {
            List<BatchDetails> batchDetails = null;
            _params = new List<FloorSqlParameter>();
            DataTable dtBatchDetails;

            _params.Add(new FloorSqlParameter("@FromDate", fromDate));
            _params.Add(new FloorSqlParameter("@ToDate", toDate));
            _params.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            _params.Add(new FloorSqlParameter("@BatchType", batchType));
            dtBatchDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_BatchDetails", _params);

            if (dtBatchDetails != null && dtBatchDetails.Rows.Count > 0)
            {
                batchDetails = (from DataRow r in dtBatchDetails.Rows
                                select new BatchDetails
                                {
                                    SerialNumber = Convert.ToInt64(r["SerialNumber"]),
                                    Time = FloorDBAccess.GetString(r, "Time"),
                                    GloveType = FloorDBAccess.GetString(r, "GloveType"),
                                    Size = FloorDBAccess.GetString(r, "Size"),
                                    BatchType = FloorDBAccess.GetString(r, "BatchType"),
                                    Shift = FloorDBAccess.GetString(r, "Shift"),
                                    Line = FloorDBAccess.GetString(r, "Line"),
                                    BatchCarddate = FloorDBAccess.GetString(r, "BatchDate"),
                                    TenPcsWeight = FloorDBAccess.GetValue<decimal>(r, "TenPCsWeight"),
                                    BatchWeight = FloorDBAccess.GetValue<decimal>(r, "BatchWeight"),
                                    TotalPcs = FloorDBAccess.GetValue<Int32>(r, "TotalPCs"),
                                    VerificationQAI = FloorDBAccess.GetString(r, "VerificationQAI"),
                                    QAIDate = FloorDBAccess.GetValue<DateTime>(r, "QAIDate"),
                                    BatchCurrentLocation = FloorDBAccess.GetString(r, "BatchCardCurrentLocation")
                                }).ToList();
            }

            return batchDetails;
        }

        /// <summary>
        /// Check if Serial Number is valid.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>int</returns>
        public static int CheckSerialNoStatus(Int64 serialNumber)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_SerialNoValidationStatus", _params));
        }
        #endregion

        #region Reason master

        /// <summary>
        /// Method to validate if the logged in user is admin and is the owner of the module
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int IsUserModuleOwner(string _loggedInUser, string moduleId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@LoggedInUser", _loggedInUser));
            PrmList.Add(new FloorSqlParameter("@ModuleId", moduleId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsUserModuleOwner", PrmList));
        }

        /// <summary>
        /// Get Reason Type Details 
        /// </summary>
        /// <returns>List<ReasonDTO></returns>
        public static List<ReasonTypeDTO> GetReasonTypes(string loggedInUser)
        {
            List<ReasonTypeDTO> reasonDTO = null;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            DataTable dtReason;
            dtReason = FloorDBAccess.ExecuteDataTable("USP_SEL_ReasonType", PrmList);

            if (dtReason != null && dtReason.Rows.Count > 0)
            {
                reasonDTO = (from DataRow r in dtReason.Rows
                             select new ReasonTypeDTO
                             {
                                 ReasonType = FloorDBAccess.GetString(r, "Reason Type"),
                                 ModuleName = FloorDBAccess.GetString(r, "Module Name"),
                                 ReasonTypeId = FloorDBAccess.GetValue<Int32>(r, "ReasonTypeId"),
                                 ModuleId = FloorDBAccess.GetString(r, "ModuleId")
                             }).ToList();
            }
            return reasonDTO;
        }

        /// <summary>
        /// Get Reason Text Details 
        /// </summary>
        /// <returns>List<ReasonTextDTO></returns>
        public static List<ReasonDTO> GetReasonTextDetails(int reasonId)
        {
            List<ReasonDTO> reasonDTO = null;
            DataTable dtReasonDetails;
            _params = new List<FloorSqlParameter>();

            _params.Add(new FloorSqlParameter("@ReasonId", reasonId));
            dtReasonDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_ReasonDetails", _params);
            if (dtReasonDetails != null && dtReasonDetails.Rows.Count > 0)
            {
                reasonDTO = (from DataRow r in dtReasonDetails.Rows
                             select new ReasonDTO
                             {
                                 ReasonType = FloorDBAccess.GetString(r, "Reason Type"),
                                 ReasonText = FloorDBAccess.GetString(r, "Reason Text"),
                                 IsScheduled = FloorDBAccess.GetValue<bool>(r, "Scheduled"),
                                 ReasonTypeId = FloorDBAccess.GetValue<int>(r, "ReasonTypeId"),
                                 ReasonTextId = FloorDBAccess.GetValue<int>(r, "ReasonTextId"),
                                 ShortCode = FloorDBAccess.GetString(r, "ShortCode")
                             }).ToList();
            }
            return reasonDTO;
        }

        /// <summary>
        /// Save new reason Text 
        /// </summary>
        public static int SaveReasonText(ReasonDTO objReasonText)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@ReasonText", objReasonText.ReasonText));
            _params.Add(new FloorSqlParameter("@ReasonTypeId", objReasonText.ReasonTypeId));
            _params.Add(new FloorSqlParameter("@IsScheduled", objReasonText.IsScheduled));
            _params.Add(new FloorSqlParameter("@WorkstationId", objReasonText.WorkstationId));
            _params.Add(new FloorSqlParameter("@OperatorId", objReasonText.OperatorId));
            _params.Add(new FloorSqlParameter("@ShortCode", objReasonText.ShortCode));
            return FloorDBAccess.ExecuteNonQuery("USP_SAV_Reason", _params);
        }

        /// <summary>
        /// Edit new reason Text 
        /// </summary>
        public static int EditReasonText(ReasonDTO objReasonText)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@ReasonText", objReasonText.ReasonText));
            _params.Add(new FloorSqlParameter("@ReasonTypeId", objReasonText.ReasonTypeId));
            _params.Add(new FloorSqlParameter("@IsScheduled", objReasonText.IsScheduled));
            _params.Add(new FloorSqlParameter("@ReasonTextId", objReasonText.ReasonTextId));
            _params.Add(new FloorSqlParameter("@WorkstationId", objReasonText.WorkstationId));
            _params.Add(new FloorSqlParameter("@OperatorId", objReasonText.OperatorId));
            _params.Add(new FloorSqlParameter("@ShortCode", objReasonText.ShortCode));
            return FloorDBAccess.ExecuteNonQuery("USP_SAV_Reason", _params);
        }

        /// <summary>
        /// Method to validate if the Reason Text already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsReasonTextDuplicate(string reasonText, int reasonTypeId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ReasonText", reasonText));
            PrmList.Add(new FloorSqlParameter("@ReasonTypeId", reasonTypeId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsReasonTextDuplicate", PrmList));
        }
        #endregion

        #region Washer Master

        /// <summary>
        /// Get Washer Details 
        /// </summary>
        /// <returns>List<WasherDTO></returns>
        public static List<WasherDTO> GetWasherDetails(int location)
        {
            List<WasherDTO> washerDTO = null;
            DataTable dtWasherDetails;

            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@location", location));

            dtWasherDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_Washer", PrmList);
            if (dtWasherDetails != null && dtWasherDetails.Rows.Count > 0)
            {
                washerDTO = (from DataRow r in dtWasherDetails.Rows
                             select new WasherDTO
                             {
                                 Id = FloorDBAccess.GetValue<int>(r, "WasherId"),
                                 WasherNumber = FloorDBAccess.GetValue<int>(r, "WasherNumber"),
                                 GloveType = FloorDBAccess.GetString(r, "GloveType"),
                                 GloveTypeDescription = FloorDBAccess.GetString(r, "Description"),
                                 GloveSize = FloorDBAccess.GetString(r, "GloveSize"),
                                 Stop = FloorDBAccess.GetValue<bool>(r, "IsStopped"),
                                 LocationId = FloorDBAccess.GetString(r, "LocationId"),
                             }).ToList();
            }
            return washerDTO;
        }

        /// <summary>
        /// Get Column Headers for Washer Master to be displayed in Excel Sheet
        /// </summary>
        /// <returns></returns>
        public static List<string> GetColumnHeadersWasher()
        {
            List<string> lstColumnHeader = new List<string>();
            lstColumnHeader.Add("Index");
            lstColumnHeader.Add("Location");
            lstColumnHeader.Add("Washer No");
            lstColumnHeader.Add("Glove Description");
            lstColumnHeader.Add("Size");
            lstColumnHeader.Add("Stop");
            return lstColumnHeader;
        }

        /// <summary>
        /// Get Glove sizes based on the Glove Type
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetGloveSizesForWasher(string gloveType)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@GloveType", gloveType));
            List<DropdownDTO> list = null;
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveSizeForWasher", _params);

            if (dt != null && dt.Rows.Count != 0)
            {
                list = (from DataRow row in dt.Rows
                        select new DropdownDTO
                        {
                            IDField = FloorDBAccess.GetString(row, "COMMONSIZE"),
                            DisplayField = FloorDBAccess.GetString(row, "COMMONSIZE")
                        }).ToList();
            }
            return list;
        }

        /// <summary>
        /// Get Glove Codes for Washer and Dryer
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetGloveCodes()
        {
            List<DropdownDTO> list = null;
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveCodes", null);

            if (dt != null && dt.Rows.Count != 0)
            {
                list = (from DataRow row in dt.Rows
                        select new DropdownDTO
                        {
                            IDField = FloorDBAccess.GetString(row, "GLOVECODE"),
                            DisplayField = FloorDBAccess.GetString(row, "GLOVECODE")
                        }).ToList();
            }
            return list;
        }

        /// <summary>
        /// Edit Washer Details 
        /// </summary>
        public static int EditWasherDetails(WasherDTO objWasher, string audLog)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@WasherId", objWasher.Id));
            _params.Add(new FloorSqlParameter("@GloveTypeDescription", objWasher.GloveTypeDescription));
            _params.Add(new FloorSqlParameter("@GloveType", objWasher.GloveType));
            _params.Add(new FloorSqlParameter("@GloveSize", objWasher.GloveSize));
            _params.Add(new FloorSqlParameter("@WasherNumber", objWasher.WasherNumber));
            _params.Add(new FloorSqlParameter("@IsStopped", objWasher.Stop));
            _params.Add(new FloorSqlParameter("@WorkstationId", objWasher.WorkstationId));
            _params.Add(new FloorSqlParameter("@OperatorId", objWasher.OperatorId));
            _params.Add(new FloorSqlParameter("@AuditLog", audLog));
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_Washer", _params);
        }

        /// <summary>
        /// Method to get the Glove Type by Glove Description
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetGloveTypeByDescription(string gloveDescription)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveDescription", gloveDescription));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_GloveTypeByDescription", PrmList));
        }

        /// <summary>
        /// Method to validate if the Washer is in use.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsWasherInUse(int washerId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@WasherId", washerId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsWasherInUse", PrmList));
        }
        #endregion

        #region Dryer Master

        /// <summary>
        /// Get Dryer Details 
        /// </summary>
        /// <returns>List<DryerDTO></returns>
        public static List<DryerDTO> GetDryerDetails(int location)
        {
            List<DryerDTO> dryerDTO = null;
            DataTable dtDryerDetails;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@location", location));
            dtDryerDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_Dryer", PrmList);
            if (dtDryerDetails != null && dtDryerDetails.Rows.Count > 0)
            {
                dryerDTO = (from DataRow r in dtDryerDetails.Rows
                            select new DryerDTO
                            {
                                Id = FloorDBAccess.GetValue<Int32>(r, "DryerId"),
                                LocationId = FloorDBAccess.GetString(r, "LocationId"),
                                DryerNumber = FloorDBAccess.GetValue<Int32>(r, "DryerNumber"),
                                GloveTypeDescription = FloorDBAccess.GetString(r, "Description"),
                                GloveType = FloorDBAccess.GetString(r, "GloveType"),
                                GloveSize = FloorDBAccess.GetString(r, "GloveSize"),
                                Cold = FloorDBAccess.GetValue<bool>(r, "Cold"),
                                Hot = FloorDBAccess.GetValue<bool>(r, "Hot"),
                                HotAndCold = FloorDBAccess.GetValue<bool>(r, "HotAndCold"),
                                IsStopped = FloorDBAccess.GetValue<bool>(r, "IsStopped"),
                                IsScheduledStop = FloorDBAccess.GetValue<bool>(r, "IsScheduledStop"),
                                CheckGlove = FloorDBAccess.GetValue<bool>(r, "CheckGlove"),
                                CheckSize = FloorDBAccess.GetValue<bool>(r, "CheckSize")
                            }).ToList();
            }
            return dryerDTO;
        }

        /// <summary>
        /// Get Column Headers for Dryer Master to be displayed in Excel Sheet
        /// </summary>
        /// <returns></returns>
        public static List<string> GetColumnHeadersDryer()
        {
            List<string> lstColumnHeader = new List<string>();
            lstColumnHeader.Add("Index");
            lstColumnHeader.Add("Location");
            lstColumnHeader.Add("Dryer Number");
            lstColumnHeader.Add("Glove Type Description");
            lstColumnHeader.Add("Size");
            lstColumnHeader.Add("COLD");
            lstColumnHeader.Add("HOT");
            lstColumnHeader.Add("Hot and COLD");
            lstColumnHeader.Add("Stop");
            lstColumnHeader.Add("Scheduled Stop");
            lstColumnHeader.Add("Check Glove");
            lstColumnHeader.Add("Check Size");
            return lstColumnHeader;

        }

        /// <summary>
        /// Edit Dryer Details 
        /// </summary>
        public static int EditDryerDetails(DryerDTO objDryer, string audLog)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@DryerId", objDryer.Id));
            _params.Add(new FloorSqlParameter("@GloveTypeDescription", objDryer.GloveTypeDescription));
            _params.Add(new FloorSqlParameter("@GloveType", objDryer.GloveType));
            _params.Add(new FloorSqlParameter("@GloveSize", objDryer.GloveSize));
            _params.Add(new FloorSqlParameter("@DryerNumber", objDryer.DryerNumber));
            _params.Add(new FloorSqlParameter("@IsStopped", objDryer.IsStopped));
            _params.Add(new FloorSqlParameter("@Hot", objDryer.Hot));
            _params.Add(new FloorSqlParameter("@Cold", objDryer.Cold));
            _params.Add(new FloorSqlParameter("@HotAndCold", objDryer.HotAndCold));
            _params.Add(new FloorSqlParameter("@IsScheduledStop", objDryer.IsScheduledStop));
            _params.Add(new FloorSqlParameter("@CheckGlove", objDryer.CheckGlove));
            _params.Add(new FloorSqlParameter("@CheckSize", objDryer.CheckSize));
            _params.Add(new FloorSqlParameter("@WorkstationId", objDryer.WorkstationId));
            _params.Add(new FloorSqlParameter("@OperatorId", objDryer.OperatorId));
            _params.Add(new FloorSqlParameter("@AuditLog", audLog));

            return FloorDBAccess.ExecuteNonQuery("USP_UPD_Dryer", _params);
        }

        /// <summary>
        /// Method to validate Size based on glove description entered
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int ValidateGloveSizeByTypeDescription(string size, string gloveDescription)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@Size", size));
            PrmList.Add(new FloorSqlParameter("@GloveDescription", gloveDescription));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_SizeValidationStatus", PrmList));
        }


        /// <summary>
        /// Method to validate  glove description entered
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int ValidateGloveTypeDescription(string gloveDescription)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveDescription", gloveDescription));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_GloveTypeValidationStatus", PrmList));
        }

        /// <summary>
        /// Method to validate if the Dryer is in use.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsDryerInUse(int dryerId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@DryerId", dryerId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsDryerInUse", PrmList));
        }

        #endregion

        #region CR

        #region Dryer Stoppage Data

        /// <summary>
        /// Get Dryer Stoppage Data 
        /// </summary>
        /// <returns>List<DryerStoppageDTO></returns>
        public static List<DryerStoppageDTO> GetDryerStoppageData(int? dryerId, DateTime? dateFrom, DateTime? dateTo, int dryerStoppageId = Constants.ZERO)
        {
            List<DryerStoppageDTO> dryerStoppageDTO = null;
            DataTable dtDryerStoppageData;

            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@DryerId", dryerId));
            _params.Add(new FloorSqlParameter("@FromDate", dateFrom));
            _params.Add(new FloorSqlParameter("@ToDate", dateTo));
            _params.Add(new FloorSqlParameter("@DryerStoppageId", dryerStoppageId));

            dtDryerStoppageData = FloorDBAccess.ExecuteDataTable("USP_SEL_DryerStoppageData", _params);
            if (dtDryerStoppageData != null && dtDryerStoppageData.Rows.Count > 0)
            {
                dryerStoppageDTO = (from DataRow r in dtDryerStoppageData.Rows
                                    select new DryerStoppageDTO
                                    {
                                        Id = FloorDBAccess.GetValue<int>(r, "DryerStoppageId"),
                                        DryerNumber = FloorDBAccess.GetValue<int>(r, "DryerNumber"),
                                        DryerId = FloorDBAccess.GetValue<int>(r, "DryerId"),
                                        StoppageDate = FloorDBAccess.GetValue<DateTime>(r, "StoppageStartDate"),
                                        ReasonId = FloorDBAccess.GetValue<int>(r, "ReasonId"),
                                        ReasonText = FloorDBAccess.GetString(r, "ReasonText"),
                                        OperatorName = FloorDBAccess.GetString(r, "OperatorName"),
                                        OperatorId = FloorDBAccess.GetString(r, "OperatorId")
                                    }).ToList();
            }

            return dryerStoppageDTO;
        }

        /// <summary>
        /// Save new Dryer Stoppage Data
        /// </summary>
        public static int SaveDryerStoppageData(DryerStoppageDTO objDryerStoppage)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@DryerId", objDryerStoppage.DryerId));
            _params.Add(new FloorSqlParameter("@DryerNumber", objDryerStoppage.DryerNumber));
            _params.Add(new FloorSqlParameter("@OperatorId", objDryerStoppage.OperatorId));
            _params.Add(new FloorSqlParameter("@ReasonId", objDryerStoppage.ReasonId));
            _params.Add(new FloorSqlParameter("@StoppageDate", objDryerStoppage.StoppageDate));
            _params.Add(new FloorSqlParameter("@WorkstationId", objDryerStoppage.WorkstationId));
            _params.Add(new FloorSqlParameter("@DryerStoppageId", objDryerStoppage.Id));

            return FloorDBAccess.ExecuteNonQuery("USP_SAV_DryerStoppageData", _params);
        }

        /// <summary>
        /// Update the batch end time for a dryer for which dryer stoppage data is being saved
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int UpdateDryerBatchEndTime(int dryerId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@DryerId", dryerId));
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_DryerBatchEndTime", PrmList);
        }

        #endregion

        #region Washer Stoppage Data

        /// <summary>
        /// Get Washer Stoppage Data 
        /// </summary>
        /// <returns>List<WasherStoppageDTO></returns>
        public static List<WasherStoppageDTO> GetWasherStoppageData(int? washerId, DateTime? dateFrom, DateTime? dateTo, int washerStoppageId = Constants.ZERO)
        {
            List<WasherStoppageDTO> washerStoppageDTO = null;
            DataTable dtWasherStoppageData;

            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@WasherId", washerId));
            _params.Add(new FloorSqlParameter("@FromDate", dateFrom));
            _params.Add(new FloorSqlParameter("@ToDate", dateTo));
            _params.Add(new FloorSqlParameter("@WasherStoppageId", washerStoppageId));

            dtWasherStoppageData = FloorDBAccess.ExecuteDataTable("USP_SEL_WasherStoppageData", _params);
            if (dtWasherStoppageData != null && dtWasherStoppageData.Rows.Count > 0)
            {
                washerStoppageDTO = (from DataRow r in dtWasherStoppageData.Rows
                                     select new WasherStoppageDTO
                                     {
                                         Id = FloorDBAccess.GetValue<int>(r, "WasherStoppageId"),
                                         WasherNumber = FloorDBAccess.GetValue<int>(r, "WasherNumber"),
                                         WasherId = FloorDBAccess.GetValue<int>(r, "WasherId"),
                                         StoppageDate = FloorDBAccess.GetValue<DateTime>(r, "StoppageStartDate"),
                                         ReasonId = FloorDBAccess.GetValue<int>(r, "ReasonId"),
                                         ReasonText = FloorDBAccess.GetString(r, "ReasonText"),
                                         OperatorName = FloorDBAccess.GetString(r, "OperatorName"),
                                         OperatorId = FloorDBAccess.GetString(r, "OperatorId")
                                     }).ToList();
            }
            return washerStoppageDTO;
        }

        /// <summary>
        /// Save new Washer Stoppage Data
        /// </summary>
        public static int SaveWasherStoppageData(WasherStoppageDTO objWasherStoppage)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@WasherId", objWasherStoppage.WasherId));
            _params.Add(new FloorSqlParameter("@WasherNumber", objWasherStoppage.WasherNumber));
            _params.Add(new FloorSqlParameter("@OperatorId", objWasherStoppage.OperatorId));
            _params.Add(new FloorSqlParameter("@ReasonId", objWasherStoppage.ReasonId));
            _params.Add(new FloorSqlParameter("@StoppageDate", objWasherStoppage.StoppageDate));
            _params.Add(new FloorSqlParameter("@WorkstationId", objWasherStoppage.WorkstationId));
            _params.Add(new FloorSqlParameter("@WasherStoppageId", objWasherStoppage.Id));

            return FloorDBAccess.ExecuteNonQuery("USP_SAV_WasherStoppageData", _params);
        }

        /// <summary>
        /// Update the batch end time for a washer or which washer stoppage data is being saved
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int UpdateWasherBatchEndTime(int washerId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@WasherId", washerId));
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_WasherBatchEndTime", PrmList);
        }
        #endregion

        #region Production Line Maintenance

        /// <summary>
        /// Get Production Line Details 
        /// </summary>
        /// <returns>List<ProductionLineDTO></returns>
        public static List<ProductionLineDTO> GetProductionLineDetails()
        {
            List<ProductionLineDTO> productionLineDTO = null;
            DataTable dtProductionLineDetails;

            dtProductionLineDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_ProductionLineData", null);
            if (dtProductionLineDetails != null && dtProductionLineDetails.Rows.Count > 0)
            {
                productionLineDTO = (from DataRow r in dtProductionLineDetails.Rows
                                     select new ProductionLineDTO
                                     {
                                         LineNumber = FloorDBAccess.GetString(r, "LineNumber"),
                                         BatchFrequency = FloorDBAccess.GetString(r, "BatchFrequency").TrimEnd(), //for audit log compare purpose due to when select is trim
                                         LTGloveType = FloorDBAccess.GetString(r, "LTGloveType"),
                                         LBGloveType = FloorDBAccess.GetString(r, "LBGloveType"),
                                         RTGloveType = FloorDBAccess.GetString(r, "RTGloveType"),
                                         RBGloveType = FloorDBAccess.GetString(r, "RBGloveType"),
                                         LTGloveSize = FloorDBAccess.GetString(r, "LTGloveSize"),
                                         RTGloveSize = FloorDBAccess.GetString(r, "RTGloveSize"),
                                         LBGloveSize = FloorDBAccess.GetString(r, "LBGloveSize"),
                                         RBGloveSize = FloorDBAccess.GetString(r, "RBGloveSize"),
                                         IsOnline = FloorDBAccess.GetValue<bool>(r, "IsOnline"),
                                         IsPrintByFormer = FloorDBAccess.GetValue<bool>(r, "IsPrintByFormer"),
                                         Plant = FloorDBAccess.GetString(r, "Plant"),
                                         IsDoubleFormer = FloorDBAccess.GetValue<bool>(r, "IsDoubleFormer"),
                                         ProdLoggingStartDateTime = FloorDBAccess.GetValue<DateTime>(r, "prodloggingstartdatetime"),
                                         LastModifiedOn = FloorDBAccess.GetValue<DateTime>(r, "LastModifiedOn"),
                                         LTAltGlove = FloorDBAccess.GetString(r, "LTAltGlove"),
                                         LBAltGlove = FloorDBAccess.GetString(r, "LBAltGlove"),
                                         RTAltGlove = FloorDBAccess.GetString(r, "RTAltGlove"),
                                         RBAltGlove = FloorDBAccess.GetString(r, "RBAltGlove"),
                                         LocationId = FloorDBAccess.GetValue<int>(r, "LocationId"),
                                         WorkstationId = FloorDBAccess.GetString(r, "WorkstationId"),
                                         OperatorId = FloorDBAccess.GetString(r, "OperatorId"),
                                         //LTFormerType = FloorDBAccess.GetString(r, "LTFormerType"),
                                         //RTFormerType = FloorDBAccess.GetString(r, "RTFormerType"),
                                         //LBFormerType = FloorDBAccess.GetString(r, "LBFormerType"),
                                         //RBFormerType = FloorDBAccess.GetString(r, "RBFormerType"),
                                         //LatexType = FloorDBAccess.GetString(r, "LatexItemId")

                                     }).ToList();
            }
            return productionLineDTO;
        }

        /// <summary>
        /// Edit Production Line Details 
        /// </summary>
        public static int EditProductionLineDetails(ProductionLineDTO objProductionLine, ProductionLineDTO objProductionLineold, string userid)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@BatchFrequency", objProductionLine.BatchFrequency));
            _params.Add(new FloorSqlParameter("@IsOnline", objProductionLine.IsOnline));
            _params.Add(new FloorSqlParameter("@IsDoubleFormer", objProductionLine.IsDoubleFormer));
            _params.Add(new FloorSqlParameter("@LTGloveType", objProductionLine.LTGloveType));
            _params.Add(new FloorSqlParameter("@RTGloveType", objProductionLine.RTGloveType));
            _params.Add(new FloorSqlParameter("@LBGloveType", objProductionLine.LBGloveType));
            _params.Add(new FloorSqlParameter("@RBGloveType", objProductionLine.RBGloveType));
            _params.Add(new FloorSqlParameter("@LTGloveSize", objProductionLine.LTGloveSize));
            _params.Add(new FloorSqlParameter("@RTGloveSize", objProductionLine.RTGloveSize));
            _params.Add(new FloorSqlParameter("@LBGloveSize", objProductionLine.LBGloveSize));
            _params.Add(new FloorSqlParameter("@RBGloveSize", objProductionLine.RBGloveSize));
            _params.Add(new FloorSqlParameter("@IsPrintByFormer", objProductionLine.IsPrintByFormer));
            _params.Add(new FloorSqlParameter("@LineNumber", objProductionLine.LineNumber));
            _params.Add(new FloorSqlParameter("@WorkstationId", objProductionLine.WorkstationId));
            _params.Add(new FloorSqlParameter("@OperatorId", objProductionLine.OperatorId));
            _params.Add(new FloorSqlParameter("@LocationId", objProductionLine.LocationId));
            _params.Add(new FloorSqlParameter("@LTAltGlove", objProductionLine.LTAltGlove));
            _params.Add(new FloorSqlParameter("@RTAltGlove", objProductionLine.RTAltGlove));
            _params.Add(new FloorSqlParameter("@LBAltGlove", objProductionLine.LBAltGlove));
            _params.Add(new FloorSqlParameter("@RBAltGlove", objProductionLine.RBAltGlove));
            _params.Add(new FloorSqlParameter("@Plant", objProductionLine.Plant));
            //_params.Add(new FloorSqlParameter("@LTFormerType", objProductionLine.LTFormerType));
            //_params.Add(new FloorSqlParameter("@LBFormerType", objProductionLine.LBFormerType));
            //_params.Add(new FloorSqlParameter("@RTFormerType", objProductionLine.RTFormerType));
            //_params.Add(new FloorSqlParameter("@RBFormerType", objProductionLine.RBFormerType));
            //_params.Add(new FloorSqlParameter("@LatexItemId", objProductionLine.LatexType));

            //set audit log
            objProductionLine.LastModifiedOn = objProductionLineold.LastModifiedOn;

            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - EditLineRecord";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), objProductionLine.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "ProductionLine";
            AuditLog.ReferenceId = objProductionLine.LineNumber;
            AuditLog.UpdateColumns = objProductionLineold.DetailedCompare(objProductionLine).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);
            //add auditlog parameter MYAdamas
            _params.Add(new FloorSqlParameter("@AuditLog", audlog));
            _params.Add(new FloorSqlParameter("@UserId", userid));


            return FloorDBAccess.ExecuteNonQuery("USP_UPD_ProductionLineData", _params);
        }

        /// <summary>
        /// Get Plants list
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetPlantsList()
        {
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_Plants", null))
            {

                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "LocationName"),
                                DisplayField = FloorDBAccess.GetString(row, "LocationName")
                            }).ToList();
                }

            }
            return list;
        }

        /// <summary>
        /// Get Glove Type list from AX
        /// </summary>
        /// <returns></returns>
        //public static DataTable GetGloveTypeFromAX(string lineNumber)
        public static List<DropdownDTO> GetGloveTypeFromAX(string lineNumber)
        {
            List<DropdownDTO> list = null;

            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@LineNumber", lineNumber));
            //return FloorDBAccess.ExecuteDataTable("USP_SEL_GloveTypeFromAX", _params);
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveTypeFromAX", _params))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "GloveCode"),
                                DisplayField = FloorDBAccess.GetString(row, "GloveCode")
                            }).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// Get Glove sizes based on the Glove Type
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetGloveSizes(string gloveType, string lineNumber)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@GloveType", gloveType));
            _params.Add(new FloorSqlParameter("@LineNumber", lineNumber));
            List<DropdownDTO> list = null;
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveSize", _params);

            if (dt != null && dt.Rows.Count != 0)
            {
                list = (from DataRow row in dt.Rows
                        select new DropdownDTO
                        {
                            IDField = FloorDBAccess.GetString(row, "Configuration"),
                            DisplayField = FloorDBAccess.GetString(row, "Configuration")
                        }).ToList();
            }
            return list;
        }


        /// <summary>
        /// Get Glove sizes based on the Glove Type
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetGloveSizesByGloveType(string gloveType)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@GloveType", gloveType));

            List<DropdownDTO> list = null;
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveSizeByGloveType", _params);

            if (dt != null && dt.Rows.Count != 0)
            {
                list = (from DataRow row in dt.Rows
                        select new DropdownDTO
                        {
                            IDField = FloorDBAccess.GetString(row, "Size"),
                            DisplayField = FloorDBAccess.GetString(row, "Size")
                        }).ToList();
            }
            return list;
        }

        #endregion

        #region QC/Packing Stoppage Data

        /// <summary>
        /// Save new QC/Packing group Stoppage Data
        /// </summary>
        public static int SaveQCGroupStoppageData(QCGroupStoppageDTO objQCGroupStoppage)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@QCGroupId", objQCGroupStoppage.QCGroupId));
            _params.Add(new FloorSqlParameter("@OperatorId", objQCGroupStoppage.OperatorId));
            _params.Add(new FloorSqlParameter("@ReasonId", objQCGroupStoppage.ReasonId));
            _params.Add(new FloorSqlParameter("@StoppageDate", objQCGroupStoppage.StoppageDate));
            _params.Add(new FloorSqlParameter("@WorkstationId", objQCGroupStoppage.WorkstationId));

            return FloorDBAccess.ExecuteNonQuery("USP_SAV_QCGroupStoppageData", _params);
        }
        #endregion

        #region Line Clearance Authorise Setup
        public static List<LineClearanceAuthoriseDTO> GetLinceClearanceAuthoriseList(bool isSearchByEmployeeID, string searchText)
        {
            List<LineClearanceAuthoriseDTO> lineClearanceAuthoriseList = null;
            _params = new List<FloorSqlParameter>();
            DataTable dtLineClearanceAuthorise;

            _params.Add(new FloorSqlParameter("@EmployeeID", isSearchByEmployeeID? searchText : null));
            _params.Add(new FloorSqlParameter("@EmployeeName", isSearchByEmployeeID? null : searchText));
            dtLineClearanceAuthorise = FloorDBAccess.ExecuteDataTable("usp_FP_GetEmployeeMasterDataByEmployeeIDOrName", _params);

            if (dtLineClearanceAuthorise != null && dtLineClearanceAuthorise.Rows.Count > 0)
            {
                lineClearanceAuthoriseList = (from DataRow r in dtLineClearanceAuthorise.Rows
                                select new LineClearanceAuthoriseDTO
                                {
                                    EmployeeID = FloorDBAccess.GetString(r, "EmployeeID"),
                                    EmployeeName = FloorDBAccess.GetString(r, "EmployeeName"),
                                    Role = FloorDBAccess.GetString(r, "EmployeeRole"),
                                    IsAllowAuthoriseLineClearance = bool.Parse(FloorDBAccess.GetString(r, "IsAllowAuthoriseLineClearance"))
                                }).ToList();
            }

            return lineClearanceAuthoriseList;
        }

        public static int UpdateLineClearanceAuthorise(DataTable lineClearanceAuthoriseTable)
        {
            int errorCode = 0;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@LineClearanceAuthorise", lineClearanceAuthoriseTable));
            PrmList.Add(new FloorSqlParameter("@ErrorCode", errorCode, ParameterDirection.Output));
            FloorDBAccess.ExecuteNonQuery("usp_FP_SetUpdateLineClearanceAuthorise", PrmList);

            return errorCode;
        }
        #endregion

        #region QC/Packing group Master

        /// <summary>
        /// Get QC Group Master Data 
        /// </summary>
        /// <returns>List<QCGroupDTO></returns>
        public static List<QCGroupDTO> GetQCGroupMasterData(string groupType = null)
        {
            List<QCGroupDTO> qcGroupDTO = null;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GroupType", groupType));
            DataTable dtQCGroupMasterData;

            dtQCGroupMasterData = FloorDBAccess.ExecuteDataTable("USP_SEL_QCGroupDetails", PrmList);
            if (dtQCGroupMasterData != null && dtQCGroupMasterData.Rows.Count > 0)
            {
                qcGroupDTO = (from DataRow r in dtQCGroupMasterData.Rows
                              select new QCGroupDTO
                              {
                                  Id = FloorDBAccess.GetValue<int>(r, "QCGroupId"),
                                  QCGroupName = FloorDBAccess.GetString(r, "QCGroupName"),
                                  IsStopped = FloorDBAccess.GetValue<bool>(r, "IsStopped"),
                                  QCGroupDescription = FloorDBAccess.GetString(r, "QCGroupDescription"),
                                  QCGroupType = FloorDBAccess.GetString(r, "GroupType"),
                                  LocationName = FloorDBAccess.GetString(r,"LocationName")
                              }).ToList();
            }

            return qcGroupDTO;
        }

        /// <summary>
        /// Get QC Group master details
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetQCGroupsData(string groupType = null)
        {
            List<DropdownDTO> list = null;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GroupType", groupType));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QCGroupDetails", PrmList))
            {
                if (dt != null && dt.Rows.Count != Constants.ZERO)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {

                                IDField = FloorDBAccess.GetString(row, "QCGroupId"),
                                DisplayField = FloorDBAccess.GetString(row, "QCGroupName")
                            }).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// Method to validate if the QC/Packing group already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsQCGroupDuplicate(string groupName)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GroupName", groupName));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsQCGroupDuplicate", PrmList));
        }

        /// <summary>
        /// Save QC/Packing Group
        /// </summary>
        public static int SaveQCGroup(QCGroupDTO objQCGroup)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@QCGroupType", objQCGroup.QCGroupType));
            _params.Add(new FloorSqlParameter("@QCGroupName", objQCGroup.QCGroupName));
            _params.Add(new FloorSqlParameter("@QCGroupDescription", objQCGroup.QCGroupDescription));
            _params.Add(new FloorSqlParameter("@IsStopped", objQCGroup.IsStopped));
            _params.Add(new FloorSqlParameter("@Id", objQCGroup.Id));
            _params.Add(new FloorSqlParameter("@WorkstationId", objQCGroup.WorkstationId));
            _params.Add(new FloorSqlParameter("@OperatorId", objQCGroup.OperatorId));
            _params.Add(new FloorSqlParameter("@LocationName", objQCGroup.LocationName));

            return FloorDBAccess.ExecuteNonQuery("USP_SAV_QCGroups", _params);
        }
        #endregion

        #region Glove Type Master

        public static List<GloveTypeMasterDTO> GetGloveType()
        {
            List<GloveTypeMasterDTO> GloveTypeDTO = null;
            DataTable dtGloveType = FloorDBAccess.ExecuteDataTable("USP_SEL_GLOVETYPELISTMASTER", null);

            if (dtGloveType != null && dtGloveType.Rows.Count > 0)
            {
                GloveTypeDTO = (from DataRow r in dtGloveType.Rows
                                select new GloveTypeMasterDTO
                                {
                                    AVAGLOVECODETABLE_ID = FloorDBAccess.GetValue<int>(r, "AVAGLOVECODETABLE_ID"),
                                    BARCODE = FloorDBAccess.GetString(r, "BARCODE"),
                                    GLOVECODE = FloorDBAccess.GetString(r, "GLOVECODE"),
                                    DESCRIPTION = FloorDBAccess.GetString(r, "DESCRIPTION"),
                                    GLOVECATEGORY = FloorDBAccess.GetString(r, "GLOVECATEGORY"),
                                    HOTBOX = FloorDBAccess.GetValue<int>(r, "HOTBOX"),
                                    POLYMER = FloorDBAccess.GetValue<int>(r, "POLYMER"),
                                    POWDER = FloorDBAccess.GetValue<int>(r, "POWDER"),
                                    PROTEIN = FloorDBAccess.GetValue<int>(r, "PROTEIN"),
                                }).ToList();
            }
            return GloveTypeDTO;
        }

        public static List<GloveTypeMasterDTO> GetGloveTypeTest(int AVAGLOVECODETABLE_ID)
        {
            List<GloveTypeMasterDTO> GloveTypeDTO = null;
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@AVAGLOVECODETABLE_ID", AVAGLOVECODETABLE_ID));
            DataTable dtGloveType = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveTypeMasterById", _params);

            if (dtGloveType != null && dtGloveType.Rows.Count > 0)
            {
                GloveTypeDTO = (from DataRow r in dtGloveType.Rows
                                select new GloveTypeMasterDTO
                                {
                                    PROTEIN = FloorDBAccess.GetValue<int>(r, "PROTEIN"),
                                    PROTEINSPEC = FloorDBAccess.GetValue<decimal>(r, "PROTEINSPEC"),
                                    POWDER = FloorDBAccess.GetValue<int>(r, "POWDER"),
                                    POWDERFORMULA = FloorDBAccess.GetValue<int>(r, "POWDERFORMULA"),
                                    HOTBOX = FloorDBAccess.GetValue<int>(r, "HOTBOX"),
                                    POLYMER = FloorDBAccess.GetValue<int>(r, "POLYMER")
                                }).ToList();
            }
            return GloveTypeDTO;
        }

        public static List<GloveTypeSizeRelationMasterDTO> GetGloveTypeSize(int AVAGLOVECODETABLE_ID)
        {
            List<GloveTypeSizeRelationMasterDTO> GloveTypeDTO = null;
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@GLOVETYPEID", AVAGLOVECODETABLE_ID));
            DataTable dtGloveType = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveTypeSizeRelationById", _params);

            if (dtGloveType != null && dtGloveType.Rows.Count > 0)
            {
                GloveTypeDTO = (from DataRow r in dtGloveType.Rows
                                select new GloveTypeSizeRelationMasterDTO
                                {
                                    COMMONSIZE = FloorDBAccess.GetString(r, "COMMONSIZE"),
                                    GLOVEWEIGHT = FloorDBAccess.GetValue<decimal>(r, "GLOVEWEIGHT"),
                                    MAX10PCSWT = FloorDBAccess.GetValue<decimal>(r, "MAX10PCSWT"),
                                    MIN10PCSWT = FloorDBAccess.GetValue<decimal>(r, "MIN10PCSWT"),
                                    Stopped = FloorDBAccess.GetValue<int>(r, "Stopped")
                                }).ToList();
            }
            return GloveTypeDTO;
        }

        public static int AddGloveType(GloveTypeMasterDTO olditem, GloveTypeMasterDTO newitem, string userid)
        {
            using (var scope = new TransactionScope())
            {
                Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), newitem.ActionType.ToString());

                if (audAction == Constants.ActionLog.Add)
                    newitem.BARCODE = RunningNoBLL.NextRunningNo(RunningNoBLL.RunningNoType.GloveTypeBarcode, userid);

                ChangeLogDTO GloveTypeMasterLog = new ChangeLogDTO();
                GloveTypeMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                GloveTypeMasterLog.TableName = "AX_AVAGLOVECODETABLE";
                GloveTypeMasterLog.UserId = userid;
                GloveTypeMasterLog.UpdateColumns = olditem.DetailedCompare(newitem).GetPropChanges();

                string GloveTypeMasterRecord = CommonBLL.SerializeTOXML(newitem);
                string tbllog = CommonBLL.SerializeTOXML(GloveTypeMasterLog);
                //audit log
                AuditLogDTO AuditLog = new AuditLogDTO();
                AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                AuditLog.FunctionName = "Configuration SetUp - Glove Type";
                AuditLog.CreatedBy = userid;
                AuditLog.AuditAction = Convert.ToInt32(audAction);
                AuditLog.SourceTable = "AX_AVAGLOVECODETABLE";
                //to get referenceid for add ( add haven get the identity id yet)
                string refid = "";
                if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
                {
                    refid = newitem.AVAGLOVECODETABLE_ID.ToString();
                    AuditLog.ReferenceId = refid;
                }
                AuditLog.UpdateColumns = olditem.DetailedCompare(newitem).GetPropChanges();
                string audlog = CommonBLL.SerializeTOXML(AuditLog);

                int _rowsaffected;
                List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
                PrmList.Add(new FloorSqlParameter("@GloveCodeMasterRecord", GloveTypeMasterRecord));
                PrmList.Add(new FloorSqlParameter("@GloveCodeMasterLog", tbllog));
                PrmList.Add(new FloorSqlParameter("@AuditLog", audlog));
                PrmList.Add(new FloorSqlParameter("@UserId", userid));
                _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_AVAGLOVECODETABLE", PrmList);

                scope.Complete();

                return _rowsaffected;
            }
        }

        public static List<DropdownDTO> GetGloveCategory()
        {
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_GloveCategoryMaster", null))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "GloveCategory"),
                                DisplayField = FloorDBAccess.GetString(row, "Description")
                            }).ToList();
                }
            }
            return list;
        }
        public static int IsGloveTypeMasterDuplicate(string barcode, string glovecode)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@Barcode", barcode));
            PrmList.Add(new FloorSqlParameter("@GloveCode", glovecode));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsGloveTypeMasterDuplicate", PrmList));
        }

        public static int IsGloveTypeMasterDuplicate(int glovetypeId, string barcode, string glovecode)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@glovetypeId", glovetypeId));
            PrmList.Add(new FloorSqlParameter("@Barcode", barcode));
            PrmList.Add(new FloorSqlParameter("@GloveCode", glovecode));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsGloveTypeMasterDuplicateEdit", PrmList));
        }

        public static GloveTypeMasterDTO GetGloveTypeDetails(int gloveTypeId)
        {
            GloveTypeMasterDTO gloveTypeDTO = null;

            DataTable dtGloveType;

            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@AVAGLOVECODETABLE_ID", gloveTypeId));

            dtGloveType = FloorDBAccess.ExecuteDataTable("USP_GET_AX_AVAGLOVECODETABLE", PrmList);
            if (dtGloveType != null && dtGloveType.Rows.Count > 0)
            {
                gloveTypeDTO = (from DataRow r in dtGloveType.Rows
                                select new GloveTypeMasterDTO
                                {
                                    AVAGLOVECODETABLE_ID = FloorDBAccess.GetValue<int>(r, "AVAGLOVECODETABLE_ID"),
                                    BARCODE = FloorDBAccess.GetString(r, "BARCODE"),
                                    DESCRIPTION = FloorDBAccess.GetString(r, "DESCRIPTION"),
                                    GLOVECATEGORY = FloorDBAccess.GetString(r, "GLOVECATEGORY"),
                                    GLOVECODE = FloorDBAccess.GetString(r, "GLOVECODE"),
                                    HOTBOX = FloorDBAccess.GetValue<int>(r, "HOTBOX"),
                                    POLYMER = FloorDBAccess.GetValue<int>(r, "POLYMER"),
                                    POWDER = FloorDBAccess.GetValue<int>(r, "POWDER"),
                                    POWDERFORMULA = FloorDBAccess.GetValue<int>(r, "POWDERFORMULA"),
                                    PROTEIN = FloorDBAccess.GetValue<int>(r, "PROTEIN"),
                                    PROTEINSPEC = FloorDBAccess.GetValue<decimal>(r, "PROTEINSPEC"),


                                }).FirstOrDefault();
            }

            return gloveTypeDTO;
        }

        #endregion

        #region QAI Defect List

        /// <summary>
        /// Get QAI Defect category
        /// </summary>
        /// <returns>List<QAIDefectCategory></returns>
        public static List<QAIDefectCategory> GetQAIDefectCategory()
        {
            List<QAIDefectCategory> defectCategoryDTO = null;
            DataTable dtQAIDefectCategory = new DataTable();

            dtQAIDefectCategory = FloorDBAccess.ExecuteDataTable("USP_SEL_QAIDEFECTLISTMASTER", null);
            if (dtQAIDefectCategory != null && dtQAIDefectCategory.Rows.Count > 0)
            {
                defectCategoryDTO = (from DataRow r in dtQAIDefectCategory.Rows
                                     select new QAIDefectCategory
                                     {
                                         ID = FloorDBAccess.GetValue<int>(r, "DefectCategoryId"),
                                         DefectCategory = FloorDBAccess.GetString(r, "DefectCategory"),
                                         Sequence = FloorDBAccess.GetValue<int>(r, "Sequence"),
                                         DefectCategoryType = FloorDBAccess.GetString(r, "DefectCategoryType"),
                                         DefectCategoryTypeId = FloorDBAccess.GetValue<int>(r, "DefectCategoryTypeId"),
                                         IsDeleted = FloorDBAccess.GetValue<bool>(r, "IsDeleted")
                                     }).ToList();
            }
            return defectCategoryDTO;
        }

        /// <summary>
        /// Get QAI defect Details 
        /// </summary>
        /// <returns>List<QAIDefectDetails></returns>
        public static List<QAIDefectDetails> GetQAIDefectDetails(int defectCategoryId)
        {
            List<QAIDefectDetails> defectDetailsDTO = null;
            DataTable dtDefectDetails;
            _params = new List<FloorSqlParameter>();

            _params.Add(new FloorSqlParameter("@DefectCategoryId", defectCategoryId));
            dtDefectDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_DefectMasterByCategoryId", _params);
            if (dtDefectDetails != null && dtDefectDetails.Rows.Count > 0)
            {
                defectDetailsDTO = (from DataRow r in dtDefectDetails.Rows
                                    select new QAIDefectDetails
                                    {
                                        DefectID = FloorDBAccess.GetValue<int>(r, "DefectId"),
                                        DefectItem = FloorDBAccess.GetString(r, "DefectItem"),
                                        DefectCode = FloorDBAccess.GetString(r, "DefectCode"),
                                        DefectCategory = FloorDBAccess.GetString(r, "DefectCategory"),
                                        KeyStroke = FloorDBAccess.GetString(r, "KeyStroke"),
                                        IsAnd = FloorDBAccess.GetValue<bool>(r, "IsAND"),
                                        QCType = FloorDBAccess.GetString(r, "QCType"),
                                        IsDeleted = FloorDBAccess.GetValue<bool>(r, "IsDeleted"),
                                        DefectCategoryId = FloorDBAccess.GetValue<int>(r, "DefectCategoryId"),
                                        DefectCategoryGroup = FloorDBAccess.GetString(r, "DefectCategoryGroup"),
                                        DefectCategoryGroupId = FloorDBAccess.GetValue<int>(r, "DefectCategoryGroupId"),

                                    }).ToList();
                return defectDetailsDTO;
            }
            else
            {
                return Enumerable.Empty<QAIDefectDetails>().ToList<QAIDefectDetails>();
            }
        }

        /// <summary>
        /// Get QAI defect Positions 
        /// </summary>
        /// <returns>List<QAIDefectPositions></returns>
        public static List<QAIDefectPositions> GetQAIDefectPositions(int defectId)
        {
            List<QAIDefectPositions> defectPositionsDTO = null;
            DataTable dtDefectPositions;
            _params = new List<FloorSqlParameter>();

            _params.Add(new FloorSqlParameter("@DefectID", defectId));
            dtDefectPositions = FloorDBAccess.ExecuteDataTable("USP_SEL_DefectPositionByDefectId", _params);
            if (dtDefectPositions != null && dtDefectPositions.Rows.Count > 0)
            {
                defectPositionsDTO = (from DataRow r in dtDefectPositions.Rows
                                      select new QAIDefectPositions
                                      {
                                          DefectPositionId = FloorDBAccess.GetValue<int>(r, "DefectPositionId"),
                                          DefectPositionItem = FloorDBAccess.GetString(r, "DefectPositionItem"),
                                          DefectItem = FloorDBAccess.GetString(r, "DefectItem"),
                                          KeyStroke = FloorDBAccess.GetString(r, "KeyStroke"),
                                          IsDeleted = FloorDBAccess.GetValue<bool>(r, "IsDeleted"),
                                          DefectId = FloorDBAccess.GetValue<int>(r, "DefectId"),
                                      }).ToList();
                return defectPositionsDTO;
            }
            else
            {
                return Enumerable.Empty<QAIDefectPositions>().ToList<QAIDefectPositions>();
            }
        }

        /// <summary>
        /// Save QAI Defect Category 
        /// </summary>
        public static int SaveQAIDefectCategory(QAIDefectCategory objDefectCategory, QAIDefectCategory objDefectCategoryold, string userid)
        {
            //audit log 
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - AddOrEditQAIDefectCategory";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), objDefectCategory.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "DefectCategory";
            string refid = "";
            if (audAction == Constants.ActionLog.Update)
            {
                refid = objDefectCategory.ID.ToString();

                AuditLog.ReferenceId = refid;
            }
            AuditLog.UpdateColumns = objDefectCategoryold.DetailedCompare(objDefectCategory).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@ID", objDefectCategory.ID));
            _params.Add(new FloorSqlParameter("@DefectCategory", objDefectCategory.DefectCategory));
            _params.Add(new FloorSqlParameter("@Sequence", objDefectCategory.Sequence));
            _params.Add(new FloorSqlParameter("@WorkstationId", objDefectCategory.WorkstationId));
            _params.Add(new FloorSqlParameter("@OperatorId", objDefectCategory.OperatorId));
            _params.Add(new FloorSqlParameter("@DefectCategoryTypeId", objDefectCategory.DefectCategoryTypeId));
            _params.Add(new FloorSqlParameter("@UserId", userid));
            //audit log parameter
            _params.Add(new FloorSqlParameter("@AuditLog", audlog));
            return FloorDBAccess.ExecuteNonQuery("USP_SAV_QAIDefectCategory", _params);
        }

        /// <summary>
        /// Method to retrieve the next ID for Defect Category or Defect Master Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetNextRecordId(string tableName)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@TableName", tableName));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_NextRecordId", PrmList));
        }

        /// <summary>
        /// Get QAI defect category
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetQAIDefectCategoryList()
        {
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QAIDEFECTLISTMASTER", null))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "DefectCategoryId"),
                                DisplayField = FloorDBAccess.GetString(row, "DefectCategory")
                            }).ToList();
                }

            }
            return list;
        }

        /// <summary>
        /// Get QAI defect category
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetQAIDefectList(int defectCategoryId)
        {
            List<DropdownDTO> list = null;
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@DefectCategoryId", defectCategoryId));

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_DefectMasterByCategoryId", _params))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "DefectId"),
                                DisplayField = FloorDBAccess.GetString(row, "DefectItem")
                            }).ToList();
                }

            }
            return list;
        }

        /// <summary>
        /// Get QAI defect category type
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetQAIDefectCategoryTypeList()
        {
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QAIDefectCategoryType", null))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "DefectCategoryTypeId"),
                                DisplayField = FloorDBAccess.GetString(row, "DefectCategoryType")
                            }).ToList();
                }

            }
            return list;
        }

        /// <summary>
        /// Save QAI Defect Details 
        /// </summary>
        public static int SaveQAIDefectDetails(QAIDefectDetails objDefectDetails, QAIDefectDetails objDefectDetailsOld, string userid)
        {
            //audit log 
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - AddOrEditQAIDefect";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), objDefectDetails.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "DefectMaster";
            string refid = "";

            refid = objDefectDetails.DefectID.ToString();

            AuditLog.ReferenceId = refid;
            AuditLog.UpdateColumns = objDefectDetailsOld.DetailedCompare(objDefectDetails).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);


            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@DefectID", objDefectDetails.DefectID));
            _params.Add(new FloorSqlParameter("@DefectCategoryId", objDefectDetails.DefectCategoryId));
            _params.Add(new FloorSqlParameter("@DefectItem", objDefectDetails.DefectItem));
            _params.Add(new FloorSqlParameter("@DefectCode", objDefectDetails.DefectCode));
            _params.Add(new FloorSqlParameter("@KeyStroke", objDefectDetails.KeyStroke));
            _params.Add(new FloorSqlParameter("@IsAnd", objDefectDetails.IsAnd));
            _params.Add(new FloorSqlParameter("@DefectCategoryGroup", objDefectDetails.DefectCategoryGroupId));
            _params.Add(new FloorSqlParameter("@WorkstationId", objDefectDetails.WorkstationId));
            _params.Add(new FloorSqlParameter("@OperatorId", objDefectDetails.OperatorId));
            _params.Add(new FloorSqlParameter("@QCType", objDefectDetails.QCType));
            _params.Add(new FloorSqlParameter("@UserId", userid));
            //audit log parameter
            _params.Add(new FloorSqlParameter("@AuditLog", audlog));
            return FloorDBAccess.ExecuteNonQuery("USP_SAV_QAIDefects", _params);
        }


        /// <summary>
        /// Save QAI Defect Details 
        /// </summary>
        public static int SaveQAIDefectPositions(QAIDefectPositions objDefectPositions, QAIDefectPositions objDefectPositionsOld, string userid)
        {
            //audit log 
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - AddOrEditQAIDefectPosition";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), objDefectPositions.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "DefectPosition";
            string refid = "";

            refid = objDefectPositions.DefectPositionId.ToString();

            AuditLog.ReferenceId = refid;
            AuditLog.UpdateColumns = objDefectPositionsOld.DetailedCompare(objDefectPositions).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);


            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@DefectPositionID", objDefectPositions.DefectPositionId));
            _params.Add(new FloorSqlParameter("@DefectId", objDefectPositions.DefectId));
            _params.Add(new FloorSqlParameter("@DefectPositionItem", objDefectPositions.DefectPositionItem));
            _params.Add(new FloorSqlParameter("@KeyStroke", objDefectPositions.KeyStroke));
            _params.Add(new FloorSqlParameter("@UserId", userid));
            //audit log parameter
            _params.Add(new FloorSqlParameter("@AuditLog", audlog));
            return FloorDBAccess.ExecuteNonQuery("USP_SAV_QAIDefectPositions", _params);
        }

        /// <summary>
        /// Method to validate if the Defect category already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsDefectCategoryDuplicate(string defectCategory)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@DefectCategory", defectCategory));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsQAIDefectCategoryDuplicate", PrmList));
        }

        /// <summary>
        /// Method to validate if the Defect sequence already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsSequenceDuplicate(int defectSequence)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@DefectSequence", defectSequence));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsQAISequenceDuplicate", PrmList));
        }

        /// <summary>
        /// Method to validate if the Defect already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsDefectDuplicate(string defectName, int defectCategoryId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@DefectName", defectName));
            PrmList.Add(new FloorSqlParameter("@DefectCategoryId", defectCategoryId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsQAIDefectDuplicate", PrmList));
        }

        /// <summary>
        /// Method to validate if the KeyStroke already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsKeyStrokeDuplicate(string keyStroke, int defectCategoryId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@KeyStroke", keyStroke));
            PrmList.Add(new FloorSqlParameter("@DefectCategoryId", defectCategoryId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsQAIKeyStrokeDuplicate", PrmList));
        }
        /// <summary>
        /// Method to validate if the Defect Code already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsDefectCodeDuplicate(string defectCode, int defectCategoryId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@DefectCode", defectCode));
            PrmList.Add(new FloorSqlParameter("@DefectCategoryId", defectCategoryId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("[USP_GET_IsQAIDefectCodeDuplicate]", PrmList));
        }


        /// <summary>
        /// Method to validate if the Defect already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsDefectPositionDuplicate(string defectPositionName, int defectId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@DefectPositionName", defectPositionName));
            PrmList.Add(new FloorSqlParameter("@DefectId", defectId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsDefectPositionDuplicate", PrmList));
        }

        /// <summary>
        /// Method to validate if the KeyStroke already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsDefectPositionKeyStrokeDuplicate(string keyStroke, int defectId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@KeyStroke", keyStroke));
            PrmList.Add(new FloorSqlParameter("@DefectId", defectId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsPositionKeyStrokeDuplicate", PrmList));
        }
        #endregion



        #endregion

        #region GloveTypeMaster
        public static DataTable GetGloveCommonSizeByGloveType(int GlovetypeId)
        {
            DataTable dtTableDetails;

            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveTypeId", GlovetypeId));

            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveCommonSizeByGloveTypeID", PrmList);
            return dtTableDetails;
        }

        public static List<GloveTypeSizeRelationMasterDTO> GetGloveCommonSizeGloveTypeMaster(int GlovetypeId)
        {

            List<GloveTypeSizeRelationMasterDTO> lstglovesizemstr = new List<GloveTypeSizeRelationMasterDTO>();
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveTypeId", GlovetypeId));

            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveCommonSizeByGloveTypeID", PrmList);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    GloveTypeSizeRelationMasterDTO glovesizemaster = new GloveTypeSizeRelationMasterDTO();
                    glovesizemaster.AVAGLOVERELCOMSIZE_ID = FloorDBAccess.GetValue<int>(dr, "AVAGLOVERELCOMSIZE_ID");
                    glovesizemaster.COMMONSIZE = FloorDBAccess.GetString(dr, "CommonSize");
                    glovesizemaster.GLOVEWEIGHT = FloorDBAccess.GetValue<decimal>(dr, "GloveWeight");
                    glovesizemaster.MAX10PCSWT = FloorDBAccess.GetValue<decimal>(dr, "MAX10PCSWT");
                    glovesizemaster.MIN10PCSWT = FloorDBAccess.GetValue<decimal>(dr, "MIN10PCSWT");
                    glovesizemaster.Stopped = FloorDBAccess.GetValue<int>(dr, "Stopped");
                    glovesizemaster.GLOVETYPEID = FloorDBAccess.GetValue<int>(dr, "GLOVETYPEID");
                    lstglovesizemstr.Add(glovesizemaster);
                }
            }
            return lstglovesizemstr;


        }


        public static void SaveGloveCommonSize(GloveTypeSizeRelationMasterDTO lmld, GloveTypeSizeRelationMasterDTO lmnew, string userid)
        {
            ChangeLogDTO GloveCommonSizeMasterLog = new ChangeLogDTO();
            GloveCommonSizeMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            GloveCommonSizeMasterLog.TableName = "AX_AVAGLOVERELCOMSIZE";
            GloveCommonSizeMasterLog.UserId = userid;
            GloveCommonSizeMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string GloveCommonSizeMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(GloveCommonSizeMasterLog);
            //audit log
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - Glove Type";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), lmnew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "AX_AVAGLOVERELCOMSIZE";
            string refid = "";

            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {

                refid = lmnew.AVAGLOVERELCOMSIZE_ID.ToString();
                AuditLog.ReferenceId = refid;
            }

            AuditLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveCommonSizeMasterRecord", GloveCommonSizeMasterRecord));
            PrmList.Add(new FloorSqlParameter("@GloveCommonSizeMasterLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog));
            PrmList.Add(new FloorSqlParameter("@UserId", userid));


            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_GloveTypeCommonSizeMaster", PrmList);
        }

        public static int IsGloveTypeCommonSizeDuplicate(string CommonSize, int GloveTypeId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@CommonSize", CommonSize));
            PrmList.Add(new FloorSqlParameter("@GloveTypeId", GloveTypeId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsGloveTypeCommonSizeDuplicate", PrmList));
        }
        #endregion
    }
    #region Custom Data Structure

    public class BatchDetails
    {
        public BatchDetails()
        {

        }
        /// <summary>
        /// Serial Number generated for the batch
        /// </summary>
        public Int64 SerialNumber { get; set; }

        /// <summary>
        /// Time for the batch
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// GloveType produced
        /// </summary>
        public string GloveType { get; set; }
        /// <summary>
        /// Size of the Glove Type Produced
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        ///Batch Type
        /// </summary>
        public string BatchType { get; set; }
        /// <summary>
        /// Shift when batch is produced
        /// </summary>
        public string Shift { get; set; }
        /// <summary>
        /// Line where batch is produced
        /// </summary>
        public string Line { get; set; }
        /// <summary>
        /// Date when batch is produced
        /// </summary>
        public string BatchCarddate { get; set; }
        /// <summary>
        /// Ten Pcs Weight from Platform Scale
        /// </summary>
        public decimal TenPcsWeight { get; set; }
        /// <summary>
        /// Batch Weight from Platform Scale
        /// </summary>
        public decimal BatchWeight { get; set; }

        /// <summary>
        /// Calculate by Batch Weight and Ten Pcs
        /// </summary>
        public int TotalPcs { get; set; }
        /// <summary>
        /// Verification QAI
        /// </summary>
        public string VerificationQAI { get; set; }
        /// <summary>
        /// QAI Performed on
        /// </summary>
        public DateTime QAIDate { get; set; }
        /// <summary>
        ///Current location of batch
        /// </summary>
        public string BatchCurrentLocation { get; set; }

    }

    #endregion
}
