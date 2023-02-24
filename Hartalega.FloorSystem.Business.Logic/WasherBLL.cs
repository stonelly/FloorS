// -----------------------------------------------------------------------
// <copyright file="WasherBLL.cs" company="Avanade">
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
    /// Washer business logic class
    /// </summary>
    public class WasherBLL : Framework.Business.BusinessBase
    {
        #region Private Class Variables
        private const string _subSystem = "Washer System";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WasherBLL" /> class.
        /// </summary> 
        public WasherBLL()
        {
            // No implementation required
        }
        #endregion

        #region User Methods

        /// <summary>
        /// Used to get washer details based on serial number
        /// </summary>
        /// <param name="serialNo">Serial number</param>
        /// <returns>WasherScanBatchCardDTODTO - object based on serial number, by default it will be null if no records found for that serial number</returns>
        public static WasherScanBatchCardDTO GetWasherBatchDetailsBySerialNo(decimal serialNo)
        {
            WasherScanBatchCardDTO wsbc = null;
            DataRow dr = null;
            bool isConverted = false;
            TimeSpan startTime = new TimeSpan();
            TimeSpan stopTime = new TimeSpan();
            List<FloorSqlParameter> lstFsp = null;
            isConverted = false;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@serialNo", serialNo));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_WasherBatch_SerialNo_Get", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dr = dt.Rows[Constants.ZERO];
                    wsbc = new WasherScanBatchCardDTO();
                    wsbc.SerialNumber = FloorDBAccess.GetValue<decimal>(dr, "SerialNumber");
                    wsbc.WasherId = FloorDBAccess.GetValue<int>(dr, "WasherId");
                    wsbc.WasherProgram = FloorDBAccess.GetString(dr, "WasherProgram");
                    wsbc.ReworkCount = FloorDBAccess.GetValue<int>(dr, "ReworkCount");
                    wsbc.ReworkReason = FloorDBAccess.GetString(dr, "ReworkReason");
                    wsbc.LastModifiedOn = FloorDBAccess.GetValue<DateTime>(dr, "LastModifiedOn");
                    isConverted = TimeSpan.TryParse(Convert.ToString(dr["StartTime"]), out startTime);
                    if (isConverted)
                    {
                        wsbc.StartTime = startTime;
                    }
                    isConverted = TimeSpan.TryParse(Convert.ToString(dr["StopTime"]), out stopTime);
                    if (isConverted)
                    {
                        wsbc.StopTime = stopTime;
                    }
                }
            }
            return wsbc;
        }

        public static WasherScanBatchCardDTO GetWasherBatchDetailsBySerialNoLocation(decimal serialNo, int location)
        {
            WasherScanBatchCardDTO wsbc = null;
            DataRow dr = null;
            bool isConverted = false;
            TimeSpan startTime = new TimeSpan();
            TimeSpan stopTime = new TimeSpan();
            List<FloorSqlParameter> lstFsp = null;
            isConverted = false;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstFsp.Add(new FloorSqlParameter("@location", location));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_WasherBatch_SerialNoLocation_Get", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dr = dt.Rows[Constants.ZERO];
                    wsbc = new WasherScanBatchCardDTO();
                    wsbc.SerialNumber = FloorDBAccess.GetValue<decimal>(dr, "SerialNumber");
                    wsbc.WasherId = FloorDBAccess.GetValue<int>(dr, "WasherId");
                    wsbc.WasherProgram = FloorDBAccess.GetString(dr, "WasherProgram");
                    wsbc.ReworkCount = FloorDBAccess.GetValue<int>(dr, "ReworkCount");
                    wsbc.ReworkReason = FloorDBAccess.GetString(dr, "ReworkReason");
                    wsbc.LastModifiedOn = FloorDBAccess.GetValue<DateTime>(dr, "LastModifiedOn");
                    isConverted = TimeSpan.TryParse(Convert.ToString(dr["StartTime"]), out startTime);
                    if (isConverted)
                    {
                        wsbc.StartTime = startTime;
                    }
                    isConverted = TimeSpan.TryParse(Convert.ToString(dr["StopTime"]), out stopTime);
                    if (isConverted)
                    {
                        wsbc.StopTime = stopTime;
                    }
                }
            }
            return wsbc;
        }

        /// <summary>
        /// Used to get batch details based on serial number
        /// </summary>
        /// <param name="serialNo">Serial number</param>
        /// <returns>BatchDTO - object based on serial number, by default it will be null if no records found for that serial number</returns>
        public static BatchDTO GetBatchDetailsBySerialNo(decimal serialNo)
        {
            List<FloorSqlParameter> lstFsp = null;
            BatchDTO btch = null;
            DataRow dr = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@serialNo", serialNo));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_Batch_SerialNo_Get", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dr = dt.Rows[Constants.ZERO];
                    btch = new BatchDTO();
                    btch.BatchNumber = FloorDBAccess.GetString(dr, "BatchNumber");
                    btch.GloveType = FloorDBAccess.GetString(dr, "GloveType");
                    btch.Size = Convert.ToString(dr["Size"]);
                    btch.QCType = FloorDBAccess.GetString(dr, "QCType");
                    btch.QCTypeDescription = FloorDBAccess.GetString(dr, "Description");
                    btch.BatchType = FloorDBAccess.GetString(dr, "BatchType"); // MH#, 06/01/2019, dectect batch type for UI checking
                }
            }
            return btch;
        }

        public static BatchDTO GetBatchDetailsBySerialNoLocation(decimal serialNo, int location)
        {
            List<FloorSqlParameter> lstFsp = null;
            BatchDTO btch = null;
            DataRow dr = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstFsp.Add(new FloorSqlParameter("@location", location));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_Batch_SerialNoLocation_Get", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dr = dt.Rows[Constants.ZERO];
                    btch = new BatchDTO();
                    btch.BatchNumber = FloorDBAccess.GetString(dr, "BatchNumber");
                    btch.GloveType = FloorDBAccess.GetString(dr, "GloveType");
                    btch.Size = Convert.ToString(dr["Size"]);
                }
            }
            return btch;
        }

        /// <summary>
        /// Get Washer details
        /// </summary>
        /// <param name="gloveType">Glove Type</param>
        /// <param name="size">Size</param>
        /// <returns>washer details based on glove type and size, it returns a null object if no washer found based on glove type and size</returns>
        public static List<WasherDTO> GetWasherDetailsByGloveTypeAndSize(string gloveType, string gloveSize, int locationId)
        {
            List<WasherDTO> washerList = null;
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GloveType", gloveType));
            lstFsp.Add(new FloorSqlParameter("@GloveSize", gloveSize));
            lstFsp.Add(new FloorSqlParameter("@LocationId", locationId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_WasherDetails_GloveTypeAndSize_Get", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    washerList = (from DataRow row in dt.Rows
                                  select new WasherDTO
                                  {
                                      WasherNumber = FloorDBAccess.GetValue<int>(row, "WasherNumber"),
                                      //Convert.ToInt32(row["WasherNumber"]),
                                  }).ToList();
                }
            }
            return washerList;
        }

        /// <summary>
        /// Get Washer program details
        /// </summary>
        /// <param name="gloveType">Glove Type</param>
        /// <returns>washer program id based on glove type, it returns null if no washer program found based on glove type</returns>
        public static List<WasherProgramDTO> GetWasherProgramIdByGloveType(string gloveType)
        {
            List<WasherProgramDTO> WasherProgramList = null;
            List<FloorSqlParameter> lstFsp = null;
            WasherProgramList = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GloveType", gloveType));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_WasherProgram_GloveType_Get", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    WasherProgramList = (from DataRow row in dt.Rows
                                         select new WasherProgramDTO
                                         {
                                             WasherProgramId = FloorDBAccess.GetValue<int>(row, "WasherProgramId"),
                                             WasherProgram = FloorDBAccess.GetString(row, "WasherProgram")
                                         }).ToList();
                }
            }
            return WasherProgramList;
        }

        /// <summary>
        /// Get Reason
        /// </summary>
        /// <param name="reasonTypeId">Reason Type</param>
        /// <returns>get Reasons based on ReasonType, it returns null if no values fount for the specified reasonTypeId</returns>
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
                                          ReasonText = FloorDBAccess.GetString(row, "ReasonText"),
                                      }).ToList();
                }
            }
            return reasonTextList;
        }

        /// <summary>
        /// Gets Operator name
        /// </summary>
        /// <param name="operatorId">Operator id</param>
        /// <returns>String - Employee name - by default it will be empty if no employee with that Id found</returns>
        public static string GetOperatorName(string operatorId)
        {
            string employeeName = string.Empty;
            List<FloorSqlParameter> PrmList = null;
            PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@operatorId", operatorId, System.Data.ParameterDirection.Input));
            employeeName = Convert.ToString(FloorDBAccess.ExecuteScalar("usp_Get_OperatorName_Washer_Dryer", PrmList));
            return employeeName;
        }

        /// <summary>
        /// Get Washer Number
        /// </summary>
        /// <param name="WasherId">Washer Id</param>
        /// <returns>washer number based on washer id, it returns "0" as a default value if no washer with that washerId found</returns>
        public static int GetWasherNumberByWasherId(int washerId)
        {
            int washerNumber = Constants.ZERO;
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@WasherId", washerId));
            washerNumber = Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_WasherMaster_WasherNumber_Get", lstFsp));
            return washerNumber;
        }

        /// <summary>
        /// Validate washer program
        /// </summary>
        /// <param name="WasherNumber">Washer number</param>
        /// <param name="GloveType">Glove Type</param>
        /// <param name="GloveSize">Glove Size</param>
        /// <returns>result of whether the previous batch has finished its assigned time in the washer process</returns>
        public static bool ValidateWasherProgram(int washerNumber, string gloveType, string gloveSize)
        {
            List<FloorSqlParameter> lstFsp = null;
            bool isDurationOver = true;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@WasherNumber", washerNumber));
            lstFsp.Add(new FloorSqlParameter("@GloveType", gloveType));
            lstFsp.Add(new FloorSqlParameter("@GloveSize", gloveSize));
            isDurationOver = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("usp_WasherProgram_Validate", lstFsp));
            return isDurationOver;
        }

        /// <summary>
        /// Save washer scan batch card details
        /// </summary>
        /// <param name="OperatorID">Operator Id</param>
        /// <param name="SerialNo">Serial Number</param>
        /// <param name="ReworkCount">Rework Count</param>
        /// <param name="Reason">Reason</param>
        /// /// <param name="StartTime">Start Time</param>
        /// <param name="LocationId">Location ID</param>
        /// /// <param name="WasherId">Washer ID</param>
        /// <param name="WasherProgram">Washer Program</param>
        /// <param name="LastModifiedOn">Last modified On</param>
        /// <param name="WorkStationNumber">Work Station Number</param>
        /// <returns>result after inserting Washer Batch Scan In details in the  “WasherScanBatchCardDTO” table</returns>
        public static int SaveWasherScanBatchCardDetails(decimal SerialNo, string OperatorId, int WasherProgram, TimeSpan StartTime, bool IsRework,
            string Reason, int ReworkCount,
            int LocationId, DateTime LastModifiedOn,
            int WorkStationNumber, int WasherNumber, string GloveType, string GloveSize
           )
        {
            List<FloorSqlParameter> lstfsp = null;
            int noofrows = Constants.ZERO;
            lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@SerialNo", SerialNo));
            lstfsp.Add(new FloorSqlParameter("@OperatorId", OperatorId));
            lstfsp.Add(new FloorSqlParameter("@WasherProgram", WasherProgram));
            lstfsp.Add(new FloorSqlParameter("@StartTime", StartTime));
            lstfsp.Add(new FloorSqlParameter("@IsRework", IsRework));
            lstfsp.Add(new FloorSqlParameter("@ReworkReason", Reason));
            lstfsp.Add(new FloorSqlParameter("@ReworkCount", ReworkCount));
            lstfsp.Add(new FloorSqlParameter("@LocationId", LocationId));
            lstfsp.Add(new FloorSqlParameter("@LastModifiedOn", LastModifiedOn));
            lstfsp.Add(new FloorSqlParameter("@WorkStationNumber", WorkStationNumber));
            lstfsp.Add(new FloorSqlParameter("@WasherNumber", WasherNumber));
            lstfsp.Add(new FloorSqlParameter("@GloveType", GloveType));
            lstfsp.Add(new FloorSqlParameter("@GloveSize", GloveSize));
            noofrows = FloorDBAccess.ExecuteNonQuery("usp_WasherBatch_ScanIn_Save", lstfsp);
            return noofrows;
        }

        /// <summary>
        /// Save washer scan out batch card details
        /// </summary>
        /// <param name="OperatorID">Operator Id</param>
        /// <param name="SerialNo">Serial Number</param>
        /// <param name="WasherProgram">WasherProgram</param>
        /// <param name="WasherNumber">WasherNumber</param>
        /// <param name="LastModifiedOn">Last modified On</param>
        /// <param name="GloveType">GloveType</param>
        /// <param name="GloveSize">GloveSize</param>
        /// <returns>result after inserting Washer Batch Scan In details in the  “WasherScanBatchCardDTO” table</returns>
        public static int SaveWasherScanOutBatchCardDetails(decimal SerialNo, string OperatorId, int WasherProgram, int WasherNumber, string GloveType, string GloveSize)
        {
            List<FloorSqlParameter> lstfsp = null;
            int noofrows = Constants.ZERO;
            lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@SerialNo", SerialNo));
            lstfsp.Add(new FloorSqlParameter("@OperatorId", OperatorId));
            lstfsp.Add(new FloorSqlParameter("@WasherProgram", WasherProgram));
            lstfsp.Add(new FloorSqlParameter("@WasherNumber", WasherNumber));
            lstfsp.Add(new FloorSqlParameter("@GloveType", GloveType));
            lstfsp.Add(new FloorSqlParameter("@GloveSize", GloveSize));
            noofrows = FloorDBAccess.ExecuteNonQuery("usp_WasherBatch_ScanOut_Save", lstfsp);
            return noofrows;
        }

        /// <summary>
        /// Validate whether stop time exists or not
        /// </summary>
        /// <param name="WasherNumber">WasherNumber</param>
        /// <param name="GloveType">GloveType</param>
        /// <param name="GloveSize">GloveSize</param>
        /// <returns>returns true if stop time exists for a scan else returns false</returns>
        public static bool ValidateStopTime(int WasherNumber, string GloveType, string GloveSize)
        {
            bool isStopped = false;
            List<FloorSqlParameter> lstfsp = null;
            lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@WasherNumber", WasherNumber));
            lstfsp.Add(new FloorSqlParameter("@GloveType", GloveType));
            lstfsp.Add(new FloorSqlParameter("@GloveSize", GloveSize));
            isStopped = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("usp_WasherBatch_StopTime_Validate", lstfsp));
            return isStopped;
        }

        /// <summary>
        /// Check whether serial number eligible for scan
        /// </summary>
        /// <param name="washerProgram">washerProgram</param>
        /// <param name="startTime">startTime</param>
        /// <returns></returns>
        public static bool ValidateSerialNumberEligibilityForScan(DateTime startTime, int washerId)
        {
            bool isEligible = false;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@StartTime", startTime));
            lstFsp.Add(new FloorSqlParameter("@WasherId", washerId));
            isEligible = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_ValidateSerialNumberEligibilityForWash", lstFsp));
            return isEligible;
        }
        #endregion

        /// <summary>
        /// check if batch is currently in qc process
        /// </summary>
        /// <param name="serialNumber">serialNumber</param>
        /// <returns>true if the batch is in QC process else returns false</returns>
        public static bool CheckIsBatchInQCProcess(decimal serialNumber)
        {
            bool isBatchInQCProcess = false;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            isBatchInQCProcess = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_CheckIsBatchInQCProcess", lstFsp));
            return isBatchInQCProcess;
        }


        public static List<WasherProgramDTO> GetWasherProgramDetails()
        {
            List<WasherProgramDTO> list = new List<WasherProgramDTO>();
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_WasherProgramDetails", null))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            list.Add(new WasherProgramDTO() { WasherProgramId = Convert.ToInt32(dr["Washer Program Id"]), WasherProgram = FloorDBAccess.GetString(dr, "Washer Program"), Totalminutes = Convert.ToDecimal(dr["Total Minutes"]), Stopped = Convert.ToInt32(dr["Stopped"]), });
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

        public static List<WasherprocessDTO> GetWasherProcessDetails(long washerRefId)
        {
            List<WasherprocessDTO> washerprocessDTO = null;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@WasherRefId", washerRefId));
            DataTable dtProcess;
            dtProcess = FloorDBAccess.ExecuteDataTable("USP_SEL_WasherStageDetails", PrmList);

            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                washerprocessDTO = (from DataRow r in dtProcess.Rows
                                    select new WasherprocessDTO
                                    {
                                        WasherprocessId = Convert.ToInt32(r["WasherProcessId"]),
                                        Process = FloorDBAccess.GetString(r, "Process"),
                                        Minutes = Convert.ToDecimal(r["Minutes"]),
                                        Stage = FloorDBAccess.GetString(r, "Stage")
                                    }).ToList();
            }
            return washerprocessDTO;
        }

        public static int IsWasherProgramDuplicate(string program)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@Program", program));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsWasherProgramDuplicate", PrmList));
        }

        public static int SaveOrUpdateWasherProgram(WasherProgramDTO objWasherProgram, int washerProgramId)
        {
            List<FloorSqlParameter> _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@WASHERPROGRAM", objWasherProgram.WasherProgram));
            // _params.Add(new FloorSqlParameter("@RECID", objWasherProgram.Recid));
            _params.Add(new FloorSqlParameter("@CREATEDDATETIME", objWasherProgram.CreatedDateTime));
            _params.Add(new FloorSqlParameter("@TOTALMINUTES", objWasherProgram.Totalminutes));
            _params.Add(new FloorSqlParameter("@STOPPED", objWasherProgram.Stopped));
            _params.Add(new FloorSqlParameter("@MODIFIEDDATETIME", objWasherProgram.ModifiedDateTime));
            _params.Add(new FloorSqlParameter("@WASHERPROGRAMID", washerProgramId));
            return FloorDBAccess.ExecuteNonQuery("USP_SAV_WasherProgram", _params);
        }

        public static int UpdateWasherProgramTotalMinutes(decimal totalMinutes, long washerRefId)
        {
            List<FloorSqlParameter> _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@WASHERPROGRAM_ID", washerRefId));
            _params.Add(new FloorSqlParameter("@TOTALMINUTES", totalMinutes));
            _params.Add(new FloorSqlParameter("@MODIFIEDDATETIME", DateTime.Now));
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_WasherProgramTotalMinSum", _params);
        }
        public static int DeleteWasherProgramRecord(int washerProgramId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@WasherProgramId", washerProgramId));
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_WasherProgram", PrmList);
        }


        public static int IsWasherStageDuplicate(string stage, long washerRefId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@Stage", stage));
            PrmList.Add(new FloorSqlParameter("@WasherRefId", washerRefId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsWasherProcessDuplicate", PrmList));
        }

        public static int SaveOrUpdateWasherProcess(WasherprocessDTO objWasherProcess, int washerProcessId)
        {
            List<FloorSqlParameter> _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@WASHERPROCESSID", washerProcessId));
            _params.Add(new FloorSqlParameter("@STAGE", objWasherProcess.Stage));
            _params.Add(new FloorSqlParameter("@PROCESS", objWasherProcess.Process));
            _params.Add(new FloorSqlParameter("@MINUTES", objWasherProcess.Minutes));
            //_params.Add(new FloorSqlParameter("@RECID", objWasherProcess.Recid));
            _params.Add(new FloorSqlParameter("@WASHERREFID", objWasherProcess.WasherRefId));
            _params.Add(new FloorSqlParameter("@CREATEDDATETIME", objWasherProcess.CreatedDateTime));
            _params.Add(new FloorSqlParameter("@MODIFIEDDATETIME", objWasherProcess.ModifiedDateTime));
            return FloorDBAccess.ExecuteNonQuery("USP_SAV_WasherProcess", _params);
        }

        public static int DeleteWasherProcessRecord(int washerProcessId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@WasherProcessId", washerProcessId));
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_WasherProcess", PrmList);
        }

        public static Decimal GetWasherStageTotalMinutesSum(long washerRefId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@WasherRefId", washerRefId));
            string totalMinutes = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_WasherProgramTotalMinSum", PrmList));
            return totalMinutes != string.Empty ? Convert.ToDecimal(totalMinutes) : 0;
        }
		
        public static bool CheckIsPTPFGlove(decimal serialNumber)
        {
            bool isPTPFGlove = false;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            isPTPFGlove = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_CheckIsPTPF", lstFsp));
            return isPTPFGlove;
        }

        //public static bool CheckIsPTPFGloveForFinalPack(decimal serialNumber)
        //{
        //    bool isPTPFGlove = false;
        //    List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
        //    lstFsp.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
        //    isPTPFGlove = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_CheckIsPTPFForFinalPack", lstFsp));
        //    return isPTPFGlove;
        //}
    }
}
