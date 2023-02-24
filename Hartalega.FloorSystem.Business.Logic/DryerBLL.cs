// -----------------------------------------------------------------------
// <copyright file="DryerBLL.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Business.Logic
{
    using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
    using Hartalega.FloorSystem.Framework;
    using Hartalega.FloorSystem.Framework.Database;
    using Hartalega.FloorSystem.Framework.DbExceptionLog;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Dryer (Cyclone) business logic class
    /// </summary>
    public class DryerBLL : Framework.Business.BusinessBase
    {
        #region Private Class Variables
        private const string _subSystem = "Dryer System";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DryerBLL" /> class.
        /// </summary>
        public DryerBLL()
        {
            // No implementation required
        }
        #endregion

        #region User Methods

        public static bool GetDryerCold(int dryerId)
        {
            bool isCold = false;
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@DryerId", dryerId));
            isCold = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("usp_DryerMaster_DryerIsCold_Get", lstFsp));
            return isCold;
        }

        /// <summary>
        /// Used to get dryer details based on serial number
        /// </summary>
        /// <param name="serialNo">Serial number</param>
        /// <returns>DryerScanBatchCardDTO - object based on serial number, by default it will be null if no records found for that serial number</returns>
        public static DryerScanBatchCardDTO GetDryerBatchDetailsBySerialNo(decimal serialNumber)
        {
            bool isConverted = false;
            TimeSpan startTime = new TimeSpan();
            TimeSpan stopTime = new TimeSpan();
            List<FloorSqlParameter> lstFsp = null;
            DryerScanBatchCardDTO dsbc = null;
            DataRow dr = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@serialNo", serialNumber));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_DryerBatch_SerialNo_Get", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dr = dt.Rows[Constants.ZERO];
                    dsbc = new DryerScanBatchCardDTO();
                    dsbc.SerialNumber = FloorDBAccess.GetValue<decimal>(dr, "SerialNumber");
                    dsbc.DryerId = FloorDBAccess.GetValue<int>(dr, "DryerId");
                    dsbc.DryerProgram = FloorDBAccess.GetString(dr, "DryerProgram");
                    dsbc.DryerProgramId = FloorDBAccess.GetValue<int>(dr, "DryerProgramId");
                    dsbc.ReworkCount = FloorDBAccess.GetValue<int>(dr, "ReworkCount");
                    dsbc.ReworkReason = FloorDBAccess.GetString(dr, "ReworkType");
                    dsbc.LastModifiedOn = FloorDBAccess.GetValue<DateTime>(dr, "LastModifiedOn");
                    isConverted = TimeSpan.TryParse(dr["StartTime"].ToString(), out startTime);
                    if (isConverted)
                    {
                        dsbc.StartTime = startTime;
                    }
                    isConverted = TimeSpan.TryParse(Convert.ToString(dr["StopTime"]), out stopTime);
                    if (isConverted)
                    {
                        dsbc.StopTime = stopTime;
                    }
                }
            }
            return dsbc;
        }

        public static DryerScanBatchCardDTO GetDryerBatchDetailsBySerialNoLocation(decimal serialNumber, int location)
        {
            bool isConverted = false;
            TimeSpan startTime = new TimeSpan();
            TimeSpan stopTime = new TimeSpan();
            List<FloorSqlParameter> lstFsp = null;
            DryerScanBatchCardDTO dsbc = null;
            DataRow dr = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@serialNo", serialNumber));
            lstFsp.Add(new FloorSqlParameter("@location", location));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_DryerBatch_SerialNoLocation_Get", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dr = dt.Rows[Constants.ZERO];
                    dsbc = new DryerScanBatchCardDTO();
                    dsbc.SerialNumber = FloorDBAccess.GetValue<decimal>(dr, "SerialNumber");
                    dsbc.DryerId = FloorDBAccess.GetValue<int>(dr, "DryerId");
                    dsbc.DryerProgram = FloorDBAccess.GetString(dr, "DryerProgram");
                    dsbc.ReworkCount = FloorDBAccess.GetValue<int>(dr, "ReworkCount");
                    dsbc.ReworkReason = FloorDBAccess.GetString(dr, "ReworkType");
                    dsbc.LastModifiedOn = FloorDBAccess.GetValue<DateTime>(dr, "LastModifiedOn");
                    isConverted = TimeSpan.TryParse(dr["StartTime"].ToString(), out startTime);
                    if (isConverted)
                    {
                        dsbc.StartTime = startTime;
                    }
                    isConverted = TimeSpan.TryParse(Convert.ToString(dr["StopTime"]), out stopTime);
                    if (isConverted)
                    {
                        dsbc.StopTime = stopTime;
                    }
                }
            }
            return dsbc;
        }
        /// <summary>
        /// Get Reason Text by ReasonTypeId from ReasonMaster
        /// </summary>
        /// <param name="reasonTypeId">Reasoon type id</param>
        /// <returns>List of Reasontext based on reasonTypeid</returns>
        public static List<ReasonDTO> GetReasonTextByReasonTypeId(int reasonTypeId)
        {
            List<ReasonDTO> reasonTextList = null;
            List<FloorSqlParameter> lstFsp = null;
            reasonTextList = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@ReasonTypeId", reasonTypeId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_Get_ReworkReason", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    reasonTextList = (from DataRow row in dt.Rows
                                      select new ReasonDTO
                                      {
                                          ReasonText = row["ReasonText"].ToString(),
                                      }).ToList();
                }
            }
            return reasonTextList;
        }

        /// <summary>
        /// Get dryer Number
        /// </summary>
        /// <param name="dryerId">Dryer Id</param>
        /// <returns>dryer number based on dryer id, it returns "0" as a default value if no dryer with that washerId found</returns>
        public static int GetDryerNumberByDryerId(int dryerId)
        {
            int dryerNumber = Constants.ZERO;
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@DryerId", dryerId));
            dryerNumber = Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_DryerMaster_DryerNumber_Get", lstFsp));
            return dryerNumber;
        }

        /// <summary>
        /// Get Dryer details
        /// </summary>
        /// <param name="gloveType">Glove Type</param>
        /// <param name="size">Size</param>
        /// <returns>dryer details based on glove type and size, it returns a null object if no dryer found based on glove type and size</returns>
        public static List<DryerDTO> GetDryerDetailsByGloveTypeAndSize(string gloveType, string gloveSize, int locationId)
        {
            List<DryerDTO> dryerList = null;
            List<FloorSqlParameter> lstFsp = null;
            dryerList = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GloveType", gloveType));
            lstFsp.Add(new FloorSqlParameter("@GloveSize", gloveSize));
            lstFsp.Add(new FloorSqlParameter("@LocationId", locationId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_DryerDetails_GloveTypeAndSize_Get", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dryerList = (from DataRow row in dt.Rows
                                 select new DryerDTO
                                 {
                                     DryerNumber = FloorDBAccess.GetValue<int>(row, "DryerNumber"),
                                 }).ToList();
                }
            }
            return dryerList;
        }

        /// <summary>
        /// Get Dryer program details
        /// </summary>
        /// <param name="gloveType">Glove Type</param>
        /// <returns>dryer program id based on glove type and size, it returns null if no washer program found based on glove type and size</returns>
        public static List<DryerProgramDTO> GetDryerProgramIdByGloveType(string gloveType)
        {
            List<DryerProgramDTO> dryerProgramList = null;
            List<FloorSqlParameter> lstFsp = null;
            dryerProgramList = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GloveType", gloveType));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_DryerProgram_GloveType_Get", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dryerProgramList = (from DataRow row in dt.Rows
                                        select new DryerProgramDTO
                                        {
                                            DryerProgramId = FloorDBAccess.GetValue<int>(row, "DryerProgramId"),
                                            DryerProgram = FloorDBAccess.GetString(row, "DryerProgram")
                                        }).ToList();
                }
            }
            return dryerProgramList;
        }

        /// <summary>
        /// Validate dryer program
        /// </summary>
        /// <param name="WasherNumber">Dryer number</param>
        /// <param name="GloveType">Glove Type</param>
        /// <param name="GloveSize">Glove Size</param>
        /// <returns>result of whether the previous batch has finished its assigned time in the dryer process</returns>
        public static bool ValidateDryerProgram(int dryerNumber, string gloveType, string gloveSize)
        {
            List<FloorSqlParameter> lstFsp = null;
            bool isDurationOver = true;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@DryerNumber", dryerNumber));
            lstFsp.Add(new FloorSqlParameter("@GloveType", gloveType));
            lstFsp.Add(new FloorSqlParameter("@GloveSize", gloveSize));
            isDurationOver = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("usp_DryerProgram_Validate", lstFsp));
            return isDurationOver;
        }

        /// <summary>
        /// Save dryer scan batch card details
        /// </summary>
        /// <param name="SerialNo">Serial Number</param>
        /// <param name="OperatorID">Operator Id</param>
        /// <param name="DryerProgram">Dryer Program</param>
        /// <param name="StartTime">Start Time</param>
        /// <param name="IsRework">IsRework</param>
        /// <param name="Reason">Reason</param>
        /// <param name="ReworkCount">Rework Count</param>
        /// <param name="LocationId">Location ID</param>
        /// <param name="LastModifiedOn">Last modified On</param>
        /// <param name="WorkStationNumber">Work Station Number</param>
        /// <param name="DryerNumber">Dryer Number</param>
        /// <param name="GloveType">GloveType</param>
        /// /// <param name="GloveSize">GloveSize</param>
        /// <returns>result after inserting DryerBatch Scan In details in the  “DryerScanBatchCard” table</returns>
        public static int SaveDryerScanBatchCardDetails(decimal SerialNo, string OperatorId, int DryerProgram, TimeSpan StartTime, bool IsRework,
            string Reason, int ReworkCount,
            int LocationId, DateTime LastModifiedOn,
            string WorkStationNumber, int DryerNumber, string GloveType, string GloveSize, string hotboxAuthorizedBy = null
           )
        {
            List<FloorSqlParameter> lstfsp = null;
            int noofrows = Constants.ZERO;
            lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@SerialNo", SerialNo));
            lstfsp.Add(new FloorSqlParameter("@OperatorId", OperatorId));
            lstfsp.Add(new FloorSqlParameter("@DryerProgram", DryerProgram));
            lstfsp.Add(new FloorSqlParameter("@StartTime", StartTime));
            lstfsp.Add(new FloorSqlParameter("@IsRework", IsRework));
            lstfsp.Add(new FloorSqlParameter("@ReworkReason", Reason));
            lstfsp.Add(new FloorSqlParameter("@ReworkCount", ReworkCount));
            lstfsp.Add(new FloorSqlParameter("@LocationId", LocationId));
            lstfsp.Add(new FloorSqlParameter("@LastModifiedOn", LastModifiedOn));
            lstfsp.Add(new FloorSqlParameter("@WorkStationNumber", WorkStationNumber));
            lstfsp.Add(new FloorSqlParameter("@DryerNumber", DryerNumber));
            lstfsp.Add(new FloorSqlParameter("@GloveType", GloveType));
            lstfsp.Add(new FloorSqlParameter("@GloveSize", GloveSize));
            lstfsp.Add(new FloorSqlParameter("@HotboxtestAuthorizedBy", hotboxAuthorizedBy));
            noofrows = FloorDBAccess.ExecuteNonQuery("usp_DryerBatch_ScanIn_Save", lstfsp);
            return noofrows;
        }

        /// <summary>
        /// Save dryer scan out batch card details
        /// </summary>
        /// <param name="SerialNo">Serial Number</param>
        /// <param name="OperatorID">Operator Id</param>
        /// <param name="WasherProgram">DryerProgram</param>
        /// <param name="WasherNumber">DryerNumber</param>
        /// <param name="LastModifiedOn">Last modified On</param>
        /// <param name="GloveType">GloveType</param>
        /// <param name="GloveSize">GloveSize</param>
        /// <returns>result after inserting Dryer Batch Scan out details in the  “DryerScanBatchCard” table</returns>
        public static int SaveDryerScanOutBatchCardDetails(decimal SerialNo, string OperatorId, int DryerProgram, int DryerNumber, DateTime LastModifiedOn, string GloveType, string GloveSize)
        {
            List<FloorSqlParameter> lstfsp = null;
            int noofrows = Constants.ZERO;
            lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@SerialNo", SerialNo));
            lstfsp.Add(new FloorSqlParameter("@OperatorId", OperatorId));
            lstfsp.Add(new FloorSqlParameter("@DryerProgram", DryerProgram));
            lstfsp.Add(new FloorSqlParameter("@DryerNumber", DryerNumber));
            lstfsp.Add(new FloorSqlParameter("@LastModifiedOn", LastModifiedOn));
            lstfsp.Add(new FloorSqlParameter("@GloveType", GloveType));
            lstfsp.Add(new FloorSqlParameter("@GloveSize", GloveSize));
            noofrows = FloorDBAccess.ExecuteNonQuery("usp_DryerBatch_ScanOut_Save", lstfsp);
            return noofrows;
        }

        /// <summary>
        /// Validate whether stop time exists or not
        /// </summary>
        /// <param name="WasherNumber">DryerNumber</param>
        /// <param name="GloveType">GloveType</param>
        /// <param name="GloveSize">GloveSize</param>
        /// <returns>returns true if stop time exists for a scan else returns false</returns>
        public static bool ValidateStopTime(int DryerNumber, string GloveType, string GloveSize)
        {
            bool isStopped = true;
            List<FloorSqlParameter> lstfsp = null;
            lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@DryerNumber", DryerNumber));
            lstfsp.Add(new FloorSqlParameter("@GloveType", GloveType));
            lstfsp.Add(new FloorSqlParameter("@GloveSize", GloveSize));
            isStopped = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("usp_DryerBatch_StopTime_Validate", lstfsp));
            return isStopped;
        }

        /// <summary>
        /// Check whether serial number eligible for scan
        /// </summary>
        /// <param name="washerProgram">dryerProgram</param>
        /// <param name="startTime">startTime</param>
        /// <returns></returns>
        public static bool ValidateSerialNumberEligibilityForScan(DateTime startTime, int dryerId)
        {
            bool isEligible = true;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@StartTime", startTime));
            lstFsp.Add(new FloorSqlParameter("@DryerId", dryerId));
            isEligible = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_ValidateSerialNumberEligibilityForDry", lstFsp));
            return isEligible;
        }

        #endregion


        public static List<DryerProcessDTO> GetDryerProcessDetails()
        {
            List<DryerProcessDTO> list = new List<DryerProcessDTO>();
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_DryerProcessDetails", null))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {

                        list = (from DataRow r in dt.Rows
                                select new DryerProcessDTO
                                {
                                    CycloneProcessID = Convert.ToInt32(r["AVACYCLONEPROCESSTABLE_ID"]),
                                    CycloneProcess = FloorDBAccess.GetString(r, "CYCLONEPROCESS"),
                                    Cold = Convert.ToDecimal(r["COLD"]),
                                    Hot = Convert.ToDecimal(r["HOT"]),
                                    RCold = Convert.ToDecimal(r["RCOLD"]),
                                    RHot = Convert.ToDecimal(r["RHOT"]),
                                    R2Cold = Convert.ToDecimal(r["R2COLD"]),
                                    R2Hot = Convert.ToDecimal(r["R2HOT"]),
                                    Stopped = Convert.ToInt32(r["STOPPED"]),
                                }).ToList();

                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.REPORTEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }


        public static int IsDryerProcessDuplicate(string process)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@Process", process));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsDryerProcessDuplicate", PrmList));
        }

        public static int SaveOrUpdateDryerProcess(DryerProcessDTO objDryerProcess, int cycloneProcessID)
        {
            List<FloorSqlParameter> _params = new List<FloorSqlParameter>();

            _params.Add(new FloorSqlParameter("@CYCLONEPROCESS", objDryerProcess.CycloneProcess));
            _params.Add(new FloorSqlParameter("@COLD", objDryerProcess.Cold));
            _params.Add(new FloorSqlParameter("@HOT", objDryerProcess.Hot));
            _params.Add(new FloorSqlParameter("@RCOLD", objDryerProcess.RCold));
            _params.Add(new FloorSqlParameter("@RHOT", objDryerProcess.RHot));
            _params.Add(new FloorSqlParameter("@R2COLD", objDryerProcess.R2Cold));
            _params.Add(new FloorSqlParameter("@R2HOT", objDryerProcess.R2Hot));
            _params.Add(new FloorSqlParameter("@STOPPED", objDryerProcess.Stopped));
            _params.Add(new FloorSqlParameter("@CREATEDDATETIME", objDryerProcess.CreatedDate));
            _params.Add(new FloorSqlParameter("@MODIFIEDDATETIME", objDryerProcess.ModifiedDate));
            _params.Add(new FloorSqlParameter("@CYCLONEPROCESSID", cycloneProcessID));
            return FloorDBAccess.ExecuteNonQuery("USP_SAV_DryerProcess", _params);
        }

        public static int DeleteDryerProcessRecord(int dryerProcessId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@DryerProcessId", dryerProcessId));
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_DryerProcess", PrmList);
        }

    }
}
