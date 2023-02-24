// -----------------------------------------------------------------------
// <copyright file="QCPackingYieldBLL.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Hartalega.FloorSystem.Business.Logic
{
    /// <summary>
    /// QC Packing Yield business logic class
    /// </summary>
    public class QCPackingYieldBLL
    {
        #region InScope

        /// <summary>
        /// Get QC Group master details
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetQCGroups(string groupType = null)
        {
            List<DropdownDTO> list = null;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GroupType", groupType));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QCGroups", PrmList))
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
        /// Get Plant master details
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetPlants()
        {
            List<DropdownDTO> list = null;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            //PrmList.Add(new FloorSqlParameter("@GroupType", groupType));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_Locations"))
            {
                if (dt != null && dt.Rows.Count != Constants.ZERO)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {

                                IDField = FloorDBAccess.GetString(row, "LocationID"),
                                DisplayField = FloorDBAccess.GetString(row, "LocationName")
                            }).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// Get QA Test Results
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static string GetQATestStatus(Int64 serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_QATestResult", PrmList));
        }

        /// <summary>
        /// Method to save QC Packing Yield Scan In batch details 
        /// </summary>
        /// <param name="objQCPackingYieldDTO"></param>
        /// <returns></returns>
        public static int SaveQCYPScanInDetails(QCYieldandPackingDTO objQCPackingYieldDTO)
        {
            int qcID = 0;
            return SaveQCYPScanInDetails(objQCPackingYieldDTO, ref qcID);
        }

        /// <summary>
        /// Method to save QC Packing Yield Scan In batch details 
        /// </summary>
        /// <param name="objQCPackingYieldDTO"></param>
        /// <param name="qcID"></param>
        /// <returns></returns>
        public static int SaveQCYPScanInDetails(QCYieldandPackingDTO objQCPackingYieldDTO, ref int qcID)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", objQCPackingYieldDTO.SerialNumber));
            PrmList.Add(new FloorSqlParameter("@QCGroupId", objQCPackingYieldDTO.QCGroupId));
            PrmList.Add(new FloorSqlParameter("@BatchStartTime", Convert.ToDateTime(objQCPackingYieldDTO.BatchStartTime)));
            if (!string.IsNullOrEmpty(objQCPackingYieldDTO.BatchTargetTime))
            {
                PrmList.Add(new FloorSqlParameter("@BatchTargetTime", Convert.ToDateTime(objQCPackingYieldDTO.BatchTargetTime)));
            }
            else
            {
                PrmList.Add(new FloorSqlParameter("@BatchTargetTime", null));
            }
            PrmList.Add(new FloorSqlParameter("@ModuleId", objQCPackingYieldDTO.ModuleName));
            PrmList.Add(new FloorSqlParameter("@SubModuleId", objQCPackingYieldDTO.SubModuleName));
            PrmList.Add(new FloorSqlParameter("@GroupMemberCount", objQCPackingYieldDTO.GroupMemberCount));
            PrmList.Add(new FloorSqlParameter("@LastModifiedOn", objQCPackingYieldDTO.LastModifiedOn));
            PrmList.Add(new FloorSqlParameter("@WorkstationId", objQCPackingYieldDTO.WorkstationId));
            PrmList.Add(new FloorSqlParameter("@ReworkReasonId", objQCPackingYieldDTO.ReworkReasonId));
            PrmList.Add(new FloorSqlParameter("@ReworkCount", objQCPackingYieldDTO.ReworkCount));
            PrmList.Add(new FloorSqlParameter("@QCGroupMembers", objQCPackingYieldDTO.QCGroupMembers));
            PrmList.Add(new FloorSqlParameter("@Shift", objQCPackingYieldDTO.ShiftId));
            PrmList.Add(new FloorSqlParameter("@QCType", objQCPackingYieldDTO.QCType));
            PrmList.Add(new FloorSqlParameter("@Brand", objQCPackingYieldDTO.Brand));
            PrmList.Add(new FloorSqlParameter("@QCTargetTypeID", objQCPackingYieldDTO.QCTargetTypeID));

            PrmList.Add(new FloorSqlParameter("@QCID", qcID, ParameterDirection.Output));

            int result = FloorDBAccess.ExecuteNonQuery("USP_SAV_QCYPScanInBatchCardDetails", PrmList);
            FloorSqlParameter qcIDParam = PrmList.Where(x => x.ParameterName == "@QCID").FirstOrDefault();
            if (qcIDParam != null)
            {
                qcID = Convert.ToInt32(qcIDParam.ParamaterValue);
            }
            return result;
        }

        public static int SaveQCYPScanInExtendDetails(QCYieldandPackingDTO objQCPackingYieldDTO, ref int qcID)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", objQCPackingYieldDTO.SerialNumber));
            PrmList.Add(new FloorSqlParameter("@QCGroupId", objQCPackingYieldDTO.QCGroupId));
            PrmList.Add(new FloorSqlParameter("@BatchStartTime", Convert.ToDateTime(objQCPackingYieldDTO.BatchStartTime)));
            if (!string.IsNullOrEmpty(objQCPackingYieldDTO.BatchTargetTime))
            {
                PrmList.Add(new FloorSqlParameter("@BatchTargetTime", Convert.ToDateTime(objQCPackingYieldDTO.BatchTargetTime)));
            }
            else
            {
                PrmList.Add(new FloorSqlParameter("@BatchTargetTime", null));
            }
            PrmList.Add(new FloorSqlParameter("@ModuleId", objQCPackingYieldDTO.ModuleName));
            PrmList.Add(new FloorSqlParameter("@SubModuleId", objQCPackingYieldDTO.SubModuleName));
            PrmList.Add(new FloorSqlParameter("@GroupMemberCount", objQCPackingYieldDTO.GroupMemberCount));
            PrmList.Add(new FloorSqlParameter("@LastModifiedOn", objQCPackingYieldDTO.LastModifiedOn));
            PrmList.Add(new FloorSqlParameter("@WorkstationId", objQCPackingYieldDTO.WorkstationId));
            PrmList.Add(new FloorSqlParameter("@ReworkReasonId", objQCPackingYieldDTO.ReworkReasonId));
            PrmList.Add(new FloorSqlParameter("@ReworkCount", objQCPackingYieldDTO.ReworkCount));
            PrmList.Add(new FloorSqlParameter("@QCGroupMembers", objQCPackingYieldDTO.QCGroupMembers));
            PrmList.Add(new FloorSqlParameter("@Shift", objQCPackingYieldDTO.ShiftId));
            PrmList.Add(new FloorSqlParameter("@QCType", objQCPackingYieldDTO.QCType));
            PrmList.Add(new FloorSqlParameter("@Brand", objQCPackingYieldDTO.Brand));
            PrmList.Add(new FloorSqlParameter("@QCTargetTypeID", objQCPackingYieldDTO.QCTargetTypeID));
            PrmList.Add(new FloorSqlParameter("@TotalPCs", objQCPackingYieldDTO.TotalPcs));

            PrmList.Add(new FloorSqlParameter("@QCID", qcID, ParameterDirection.Output));

            int result = FloorDBAccess.ExecuteNonQuery("USP_SAV_QCYPScanInBatchCardExtendDetails", PrmList);
            FloorSqlParameter qcIDParam = PrmList.Where(x => x.ParameterName == "@QCID").FirstOrDefault();
            if (qcIDParam != null)
            {
                qcID = Convert.ToInt32(qcIDParam.ParamaterValue);
            }
            return result;
        }
        /// <summary>
        /// GetGroupMembersById
        /// </summary>
        /// <param name=QCGroupId></param>
        /// <returns>string</returns>
        public static int GetGroupMembersById(int groupId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@QCGroupId", groupId));

            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_QCGroupMembers", PrmList));
        }

        /// <summary>
        /// Get Batch Target Time
        /// </summary>
        /// <param name=QCGroupId></param>
        /// <returns></returns>
        public static string GetBatchTargetTime(string serialNo, int qcGroupMemberCount, DateTime batchStartDate, int groupId, string brand = "",
            string batchStatus = "", int innerBoxCount = Constants.ZERO, int packingSize = Constants.ZERO, int targetTypeID = Constants.ZERO, int totalPcs = Constants.ZERO)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNo));
            PrmList.Add(new FloorSqlParameter("@QCGroupMemberCount", qcGroupMemberCount));
            PrmList.Add(new FloorSqlParameter("@BatchStatus", batchStatus));
            PrmList.Add(new FloorSqlParameter("@InnerBoxCount", innerBoxCount));
            PrmList.Add(new FloorSqlParameter("@PackingSize", packingSize));
            PrmList.Add(new FloorSqlParameter("@BatchStartDate", batchStartDate));
            PrmList.Add(new FloorSqlParameter("@GroupId", groupId));
            PrmList.Add(new FloorSqlParameter("@Brand", brand));
            PrmList.Add(new FloorSqlParameter("@TargetTypeID", targetTypeID));
            PrmList.Add(new FloorSqlParameter("@NewScanInTotalPcs", totalPcs));

            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_BatchTargetTime", PrmList));

        }

        #endregion

        #region Inscope CR
        /// <summary>
        /// GetGroupTypeById
        /// </summary>
        /// <param name=QCGroupId></param>
        /// <returns>string</returns>
        public static string GetGroupTypeById(int groupId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@QCGroupId", groupId));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_QCGroupTypeById", PrmList));
        }

        /// <summary>
        /// Get QC Group for current workstation
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static string GetQCGroupForWorkstation(string workstationId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@WorkstationId", workstationId));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_QCGroupForWorkstation", PrmList));
        }

        /// <summary>
        /// Get Batch details based on Serial Number.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>DataTable as resut</returns>
        public static BatchDTO GetBatchScanInDetails(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_BatchDetailsQC", lstParameters))
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
                                ShortDate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate").ToString(ConfigurationManager.AppSettings["SetupConfigDateFormat"]),
                                ShortTime = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate").ToString(Constants.START_TIME),
                                QCTypeDescription = FloorDBAccess.GetString(row, "Description"),
                                BatchType = FloorDBAccess.GetString(row, "BatchType") //#Max He, 28/12/2018, need to know Batch Type also
                            }).SingleOrDefault();
            }

            return objBatch;
        }

        /// <summary>
        /// GetShiftIdForGroup
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <returns>shift Name for the group</returns>
        public static int GetShiftIdForGroup(int groupId)
        {
            int shift = Constants.ZERO;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GroupId", groupId));
            shift = Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_ShiftId", lstFsp));
            return shift;
        }

        #endregion

        #region CR

        /// <summary>
        /// Get QCGroup list
        /// </summary>
        /// <returns>QCGroup List</returns>
        public static List<DropdownDTO> GetQCGroupForPG()
        {
            List<DropdownDTO> lstGroup = null;
            //using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QCGroup"))
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QCGroupByOrder"))
            {
                if (dt != null && dt.Rows.Count > Constants.ZERO)
                {
                    lstGroup = (from DataRow dr in dt.Rows
                                select new DropdownDTO
                                {
                                    IDField = FloorDBAccess.GetString(dr, "QCGroupId"),
                                    DisplayField = FloorDBAccess.GetString(dr, "QCGroupName"),
                                    SelectedValue = FloorDBAccess.GetString(dr, "QCGroupId")
                                }).ToList();
                }
            }
            return lstGroup;
        }

        public static List<DropdownDTO> GetQCGroupPG(int LocationId)
        {
            List<DropdownDTO> lstGroup = null;
            List<FloorSqlParameter> lstGpg = new List<FloorSqlParameter>();
            lstGpg.Add(new FloorSqlParameter("@LocationId", LocationId));
            //using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QCGroup"))
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QCGroupByOrder", lstGpg))
            {
                if (dt != null && dt.Rows.Count > Constants.ZERO)
                {
                    lstGroup = (from DataRow dr in dt.Rows
                                select new DropdownDTO
                                {
                                    IDField = FloorDBAccess.GetString(dr, "QCGroupId"),
                                    DisplayField = FloorDBAccess.GetString(dr, "QCGroupName"),
                                    SelectedValue = FloorDBAccess.GetString(dr, "QCGroupId")
                                }).ToList();
                }
            }
            return lstGroup;
        }

        /// <summary>
        /// Get QCShift List
        /// </summary>
        /// <param name="groupType">GroupType</param>
        /// <returns>List of QC Shifts</returns>
        public static List<QCShiftDTO> GetQCShiftForPG(string groupType)
        {
            List<QCShiftDTO> lstShift = null;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GroupType", groupType));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QCShift", lstFsp))
            {
                if (dt != null && dt.Rows.Count > Constants.ZERO)
                {
                    lstShift = (from DataRow dr in dt.Rows
                                select new QCShiftDTO
                                {
                                    ShiftId = FloorDBAccess.GetValue<int>(dr, "ShiftId"),
                                    Name = FloorDBAccess.GetString(dr, "Name"),
                                    InTime = FloorDBAccess.GetValue<TimeSpan>(dr, "InTime"),
                                    OutTime = FloorDBAccess.GetValue<TimeSpan>(dr, "OutTime"),
                                    GroupType = FloorDBAccess.GetString(dr, "GroupType")
                                }).ToList();
                }
            }
            return lstShift;
        }

        /// <summary>
        /// Get QC member Details for group and Shift
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <param name="shiftId">shiftId</param>
        /// <returns>QCMemberDetails for Group and Shift</returns>
        public static List<QCMemberDetailsDTO> GetQCMemberDetailsForGroupAndShift(int groupId, int shiftId)
        {
            List<QCMemberDetailsDTO> lstQCMembers = null;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GroupId", groupId));
            lstFsp.Add(new FloorSqlParameter("@ShiftId", shiftId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QCMemberDetails", lstFsp))
            {
                if (dt != null && dt.Rows.Count > Constants.ZERO)
                {
                    lstQCMembers = (from DataRow dr in dt.Rows
                                    select new QCMemberDetailsDTO
                                    {
                                        GroupId = FloorDBAccess.GetValue<int>(dr, "GroupId"),
                                        MemberId = FloorDBAccess.GetString(dr, "MemberId"),
                                        StartTime = FloorDBAccess.GetValue<DateTime>(dr, "StartTime"),
                                        EndTime = FloorDBAccess.GetValue<DateTime>(dr, "EndTime"),
                                        Name = FloorDBAccess.GetString(dr, "Name")
                                    }).ToList();
                }
            }
            return lstQCMembers;
        }

        /// <summary>
        /// Get Qc Member details for group and shift for scan out
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <param name="shiftId">shiftId</param>
        /// <returns>QC Member Details for Group and Shift for scan out</returns>
        public static List<QCMemberDetailsDTO> GetQCMemberDetailsForGroupAndShiftForScanOut(int groupId, int shiftId)
        {
            List<QCMemberDetailsDTO> lstQCMembers = null;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GroupId", groupId));
            lstFsp.Add(new FloorSqlParameter("@ShiftId", shiftId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QCMemberDetailsForScanOut", lstFsp))
            {
                if (dt != null && dt.Rows.Count > Constants.ZERO)
                {
                    lstQCMembers = (from DataRow dr in dt.Rows
                                    select new QCMemberDetailsDTO
                                    {
                                        GroupId = FloorDBAccess.GetValue<int>(dr, "GroupID"),
                                        MemberId = FloorDBAccess.GetString(dr, "MemberId"),
                                        StartTime = FloorDBAccess.GetValue<DateTime>(dr, "StartTime"),
                                        EndTime = FloorDBAccess.GetValue<DateTime>(dr, "EndTime"),
                                        Name = FloorDBAccess.GetString(dr, "Name")
                                    }).ToList();
                }
            }
            return lstQCMembers;
        }

        /// <summary>
        /// Check whether member is a valid employee or not
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <returns>no of rows for that member</returns>
        public static int CheckValidMember(string memberId)
        {
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@MemberId", memberId));
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_QCMember", lstFsp);
            return dt.Rows.Count;
        }

        /// <summary>
        /// check whether member belongs to any group and return that group
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <returns>group to which a member belongs to<</returns>
        public static string CheckUserGroup(string memberId)
        {
            string userGroup = null;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@OperatorId", memberId));
            userGroup = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_UserGroup", lstFsp));
            return userGroup;
        }

        /// <summary>
        /// get group name for group id
        /// </summary>
        /// <param name="qcGroupId">QCGroupId</param>
        /// <returns>QCGroupName</returns>
        public static string GetGroupNameForGroupId(int qcGroupId)
        {
            string groupName = null;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@QCGroupId", qcGroupId));
            groupName = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_GroupNameForGroupId", lstFsp));
            return groupName;
        }

        /// <summary>
        /// Get Screen Id based on Screen Name
        /// </summary>
        /// <param name="screenName">screenName</param>
        /// <returns>ScreenId</returns>
        public static int GetScreenIdForScreenName(string screenName)
        {
            int screenId = Constants.ZERO;
            List<FloorSqlParameter> fspList = new List<FloorSqlParameter>();
            fspList.Add(new FloorSqlParameter("@ScreenName", screenName));
            screenId = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_ScreenIdForScreenName", fspList));
            return screenId;
        }

        /// <summary>
        /// Return member count in a group
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <returns>member count in a group</returns>
        public static int CheckMemberCountInGroup(int groupId)
        {
            int qcMemberCount = Constants.ZERO;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GroupId", groupId));
            qcMemberCount = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_CheckMemberCountInGroup", lstFsp));
            return qcMemberCount;
        }

        /// <summary>
        /// SaveQCPackingScanInData
        /// </summary>
        /// <param name="oldGroupId">oldGroupId</param>
        /// <param name="memberId">memberId</param>
        /// <param name="newGroupId">newGroupId</param>
        /// <param name="shiftId">shiftId</param>
        /// <param name="subModule">subModule</param>
        /// <param name="workStationNumber">workStationNumber</param>
        /// <param name="isEndTimeToBeUpdated">isEndTimeToBeUpdated</param>
        /// <returns>no of rows affected by transaction</returns>
        public static int SaveQCPackingScanInData(int oldGroupId, string memberId, int newGroupId, int shiftId, int subModule, string workStationNumber, bool isEndTimeToBeUpdated)
        {
            int noofrows = Constants.ZERO;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@OldGroupId", oldGroupId));
            lstFsp.Add(new FloorSqlParameter("@MemberId", memberId));
            lstFsp.Add(new FloorSqlParameter("@NewGroupId", newGroupId));
            lstFsp.Add(new FloorSqlParameter("@ShiftId", shiftId));
            lstFsp.Add(new FloorSqlParameter("@SubModule", subModule));
            lstFsp.Add(new FloorSqlParameter("@WorkStationNumber", workStationNumber));
            lstFsp.Add(new FloorSqlParameter("@IsEndTimeToBeUpdated", isEndTimeToBeUpdated));
            noofrows = FloorDBAccess.ExecuteNonQuery("USP_SAV_QCPackingScanInData", lstFsp);
            return noofrows;
        }

        /// <summary>
        /// SaveQCPackingScanOutData
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <param name="memberId">memberId</param>
        /// <param name="subModule">subModule</param>
        /// <returns>no of rows affected by the transaction</returns>
        public static int SaveQCPackingScanOutData(int groupId, string memberId, int subModule)
        {
            int noofrows = Constants.ZERO;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GroupId", groupId));
            lstFsp.Add(new FloorSqlParameter("@MemberId", memberId));
            lstFsp.Add(new FloorSqlParameter("@SubModule", subModule));
            noofrows = FloorDBAccess.ExecuteNonQuery("USP_SAV_QCPackingScanOutData", lstFsp);
            return noofrows;
        }

        /// <summary>
        /// GetShiftForGroup
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <returns>shift Name for the group</returns>
        public static string GetShiftForGroup(int groupId)
        {
            string shift = string.Empty;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GroupId", groupId));
            shift = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_Shift", lstFsp));
            return shift;
        }

        /// <summary>
        /// GetShiftIdForShiftName
        /// </summary>
        /// <param name="shiftName">shiftName</param>
        /// <returns>ShiftId For ShiftName</returns>
        public static int GetShiftIdForShiftName(string shiftName)
        {
            string groupType = Constants.QC_GROUPTYPE;
            int shiftId = Constants.ZERO;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@ShiftName", shiftName));
            lstFsp.Add(new FloorSqlParameter("@GroupType", groupType));
            shiftId = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_ShiftIdForShiftName", lstFsp));
            return shiftId;
        }

        /// <summary>
        /// Get List of groups for a workstation
        /// </summary>
        /// <param name="workStationNumber">workStationNumber</param>
        /// <returns>List of groups for a workstation</returns>
        public static List<DropdownDTO> GetGroupsForWorkStation(string workStationNumber)
        {
            List<DropdownDTO> lstGroup = null;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@WorkStationNumber", workStationNumber));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_GroupsForWorkStation", lstFsp))
            {
                if (dt != null && dt.Rows.Count > Constants.ZERO)
                {
                    lstGroup = (from DataRow dr in dt.Rows
                                select new DropdownDTO
                                {
                                    IDField = FloorDBAccess.GetString(dr, "QCGroupId"),
                                    DisplayField = FloorDBAccess.GetString(dr, "QCGroupName"),
                                    SelectedValue = FloorDBAccess.GetString(dr, "QCGroupId")
                                }).ToList();
                }
            }
            return lstGroup;
        }

        /// <summary>
        /// Get Last group for the workstation
        /// </summary>
        /// <param name="workStationNumber">workStationNumber</param>
        /// <returns>LastGroup</returns>
        public static int GetLastGroupForWorkStation(string workStationNumber)
        {
            int groupId = Constants.ZERO;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@WorkStationNumber", workStationNumber));
            groupId = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_LastGroupForWorkStation", lstFsp));
            return groupId;
        }

        /// <summary>
        /// Check whether batch with which the group is associated is ended or not
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <returns>Truw if the batch is ended else returns false</returns>
        public static bool CheckBatchEnd(int groupId)
        {
            bool isEnded = true;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@GroupId", groupId));
            isEnded = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_BatchEndStatusForGroup", lstFsp));
            return isEnded;
        }

        #endregion

        /// <summary>
        /// Get Brand list
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetBrands(string serialNumber)
        {
            List<DropdownDTO> list = null;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_Brands", lstFsp))
            {
                if (dt != null && dt.Rows.Count != Constants.ZERO)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {

                                IDField = FloorDBAccess.GetString(row, "ItemId"),
                                DisplayField = FloorDBAccess.GetString(row, "ItemId")
                            }).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// Get PT status
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static string GetPTStatus(Int64 serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_PTStatus", PrmList));
        }

        /// <summary>
        /// Get BrandName
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static string GetBrandNameByBrand(string itemId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ItemId", itemId));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_SEL_BrandNameByBrand", PrmList));
        }

        /// <summary>
        /// Get QC Group master details by plant(location id)
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetQCGroupsByPlantId(string plantId)
        {
            List<DropdownDTO> list = null;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PlantId", plantId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QCGroupsByPlant", PrmList))
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

        public static string GetQCScanValidationErrorMessage(decimal serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return FloorDBAccess.ExecuteScalar("USP_VAL_QCScanInProcess", PrmList).ToString();
        }

        public static string GetQCScanExtValidationErrorMessage(decimal serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return FloorDBAccess.ExecuteScalar("USP_VAL_QCScanInExtProcess", PrmList).ToString();
        }

        public static string GetQCScanTotalPcsValidationErrorMessage(decimal serialNumber, int targetTypeID, int totalPcs, bool _isRework)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            PrmList.Add(new FloorSqlParameter("@targetTypeID", targetTypeID));
            PrmList.Add(new FloorSqlParameter("@totalPcs", totalPcs));
            PrmList.Add(new FloorSqlParameter("@IsRework", _isRework));
            return FloorDBAccess.ExecuteScalar("Validate_QCScanInTotalPcs", PrmList).ToString();
        }


        public static List<DropdownDTO> GetQCTargetType(string serialNumber, string screenType)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@enumType", Constants.QC_TARGET_TYPE));
            PrmList.Add(new FloorSqlParameter("@screenType", screenType));
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));

            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_EnumByID", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                        select new DropdownDTO
                        {
                            IDField = FloorDBAccess.GetString(row, "EnumId"),
                            DisplayField = FloorDBAccess.GetString(row, "EnumValue")
                        }).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// To-Do
        /// To retrieve maximum Total Pcs
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static int GetQcScanInMaxQuantity(string serialNumber, string targetType)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@serialNumber", serialNumber));
            PrmList.Add(new FloorSqlParameter("@targetType", targetType));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_SEL_BrandNameByBrand", PrmList));
        }
    }
}
