using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using System.Data;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework;

namespace Hartalega.FloorSystem.Business.Logic
{
    public static class QcEfficiencyBll
    {
        private static List<FloorSqlParameter> _params;

        public static List<EditQCEfficiencyDTO> GetQCEfficiencyDetails(string serialNo)
        {
            DataTable dtQcEfficiency;
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            dtQcEfficiency = FloorDBAccess.ExecuteDataTable("USP_SEL_EditQCefficiency", lstParameters);

            List<EditQCEfficiencyDTO> objQCEfficiencyDTOList = null;

            if (dtQcEfficiency != null && dtQcEfficiency.Rows.Count > Constants.ZERO)
            {
                objQCEfficiencyDTOList = (from DataRow dr in dtQcEfficiency.Rows
                                          select new EditQCEfficiencyDTO
                                          {
                                              Id = FloorDBAccess.GetValue<int>(dr, "Id"),
                                              BatchNo = FloorDBAccess.GetString(dr, "BatchNumber"),
                                              Brand = FloorDBAccess.GetString(dr, "Brand"),
                                              //BatchWeight = FloorDBAccess.GetValue<decimal>(dr, "BatchWeight"), //Commended by Tan Wei Wah 20190130
                                              BatchWeight = FloorDBAccess.GetValue<decimal>(dr, "QCBatchWeight"), //Added by Tan Wei Wah 20190130
                                              TenPcsWeight = FloorDBAccess.GetValue<decimal>(dr, "QCTenPcsWeight"), //Added by Tan Wei Wah 20190130
                                              Reason = FloorDBAccess.GetString(dr, "ReasonId"), //Added by Tan Wei Wah 20190130
                                              BatchStatus = FloorDBAccess.GetString(dr, "BatchStatus"),
                                              Group = FloorDBAccess.GetString(dr, "QCGroupName"),
                                              Date = FloorDBAccess.GetString(dr, "BatchTargetTime"),
                                              // Date = Convert.ToDateTime(dr["BatchTargetTime"]),

                                              StartTime = FloorDBAccess.GetString(dr, "BatchStartTime"),
                                              EndTime = FloorDBAccess.GetString(dr, "BatchEndTime"),
                                              //StartTime = Convert.ToDateTime(dr["BatchStartTime"]),
                                              // EndTime = Convert.ToDateTime(dr["BatchEndTime"]),
                                              NoPerson = FloorDBAccess.GetValue<int>(dr, "QCGroupCount"),
                                              ReworkReason = FloorDBAccess.GetString(dr, "ReasonText"),
                                              Rework = FloorDBAccess.GetValue<int>(dr, "ReworkCount"),
                                              PackingSize = FloorDBAccess.GetValue<int>(dr, "PackingSize"),
                                              InnerBoxCount = FloorDBAccess.GetValue<int>(dr, "InnerBoxCount"),
                                              QCType = FloorDBAccess.GetString(dr, "QCType"),
                                              QCGroupMembers = FloorDBAccess.GetString(dr, "QCGroupMembers"),
                                              Glove = FloorDBAccess.GetString(dr, "GloveType")
                                          }).ToList();
            }

            return objQCEfficiencyDTOList;
        }

        public static List<QCGroupMembersDTO> GetGroupMembers(string qcGroup)
        {
            DataTable dtEmployee;
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@EmployeeIds", qcGroup));
            dtEmployee = FloorDBAccess.ExecuteDataTable("USP_SEL_QCMembers", lstParameters);

            List<QCGroupMembersDTO> objQCGroupList = null;

            if (dtEmployee != null && dtEmployee.Rows.Count > Constants.ZERO)
            {
                objQCGroupList = (from DataRow dr in dtEmployee.Rows
                                  select new QCGroupMembersDTO
                                  {
                                      EmployeeName = FloorDBAccess.GetString(dr, "Name"),
                                      EmployeeId = FloorDBAccess.GetString(dr, "EmployeeId")
                                  }).ToList();
            }

            return objQCGroupList;
        }


        //public static int isQcTypeFormDuplicate(QCTypeDTO objQcTypeDto)
        //{
        //    _params = new List<FloorSqlParameter>();
        //    _params.Add(new FloorSqlParameter("@qcType", objQcTypeDto.QCType));
        //    _params.Add(new FloorSqlParameter("@numTester", objQcTypeDto.NumOfTester));
        //    _params.Add(new FloorSqlParameter("@desc", objQcTypeDto.Description));
        //    _params.Add(new FloorSqlParameter("@gloveCode", objQcTypeDto.GloveCode));
        //    return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsQCTypeGloveDuplicate", _params));
        //    //return 0;

        //}

        /// <summary>
        /// Updated by Tan Wei Wah 20190227 - add audit log function
        /// </summary>
        /// <param name="qCTypeDTO"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static int EditQCTypeForm(EditQCEfficiencyDTO qCTypeDTO, string userid, bool isBatchTargetTimeNull)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@id", qCTypeDTO.Id));
            // _params.Add(new FloorSqlParameter("@batchWeight", qCTypeDTO.BatchWeight));
            _params.Add(new FloorSqlParameter("@batchStatus", qCTypeDTO.BatchStatus));
            _params.Add(new FloorSqlParameter("@date", Convert.ToDateTime(qCTypeDTO.Date)));
            _params.Add(new FloorSqlParameter("@startTime", Convert.ToDateTime(qCTypeDTO.StartTime)));
            _params.Add(new FloorSqlParameter("@endTime", Convert.ToDateTime(qCTypeDTO.EndTime)));
            _params.Add(new FloorSqlParameter("@qcType", qCTypeDTO.QCType));
            _params.Add(new FloorSqlParameter("@innerBoxCount", qCTypeDTO.InnerBoxCount));
            _params.Add(new FloorSqlParameter("@packingSize", qCTypeDTO.PackingSize));
            
            _params.Add(new FloorSqlParameter("@qcBatchWeight", qCTypeDTO.BatchWeight)); //Added by Tan Wei Wah 20190131
            _params.Add(new FloorSqlParameter("@SerialNo", qCTypeDTO.SerialNo)); //Added by Tan Wei Wah 20190131
            _params.Add(new FloorSqlParameter("@reasonId", qCTypeDTO.Reason)); //Added by Tan Wei Wah 20190131
            _params.Add(new FloorSqlParameter("@isBatchTargetTimeNull", isBatchTargetTimeNull)); //Added by KahHeng 16072019
      
            //Added by Tan Wei Wah 20190227
            EditQCEfficiencyDTO qCTypeDTOold = QcEfficiencyBll.GetQCEfficiencyDetails(qCTypeDTO.SerialNo).Where(p => p.Id == qCTypeDTO.Id).FirstOrDefault();

            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "QC & Packing Yield - Edit QC Efficiency";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = Constants.ActionLog.Update;
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "QCYieldAndPacking";
            AuditLog.ReferenceId = qCTypeDTO.Id.ToString();
            AuditLog.UpdateColumns = qCTypeDTOold.DetailedCompare(qCTypeDTO).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);
            _params.Add(new FloorSqlParameter("@AuditLog", audlog));
            //_params.Add(new FloorSqlParameter("@UserId", userid));
            //End by Tan Wei Wah 20190227

            return FloorDBAccess.ExecuteNonQuery("USP_UPD_QCTypeEdit", _params);
        }

        /// <summary>
        /// Created by Loo Kah Heng 20190613
        /// To get the TotalPcs by serial number
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static Decimal GetLatestTotalPcs(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            decimal result = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("USP_GET_LatestTotalPCs", lstParameters));
            return result;
        }

    }
}
