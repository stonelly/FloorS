using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic
{
    public class MasterTableBLL
    {
        #region Member Variables
        private static List<FloorSqlParameter> _params;
        #endregion

        #region WorkstationMaster

        /// <summary>
        /// Get Table details
        /// </summary>
        public static DataTable GetWorkstationDetails()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_WorkstationDetails", null);
            return dtTableDetails;
        }

        /// <summary>
        /// Soft delete the record
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int DeleteWorkstationRecord(string primaryId, string wsId, string loggedInUser)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PrimaryId", primaryId));
            PrmList.Add(new FloorSqlParameter("@WsId", wsId));
            PrmList.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_WorkstationRecord", PrmList);
        }



        /// <summary>
        /// Get configuration list
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetConfigurationList()
        {
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_ConfigurationItems", null))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "ConfigurationId"),
                                DisplayField = FloorDBAccess.GetString(row, "ConfigurationName")
                            }).ToList();
                }

            }
            return list;
        }

        /// <summary>
        /// Get area list
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetAreaList()
        {
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_AreaCodes", null))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "AreaId"),
                                DisplayField = FloorDBAccess.GetString(row, "AreaCode")
                            }).ToList();
                }

            }
            return list;
        }

        /// <summary>
        /// Get location list
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetLocationList()
        {
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_Plants", null))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "LocationId"),
                                DisplayField = FloorDBAccess.GetString(row, "LocationName")
                            }).ToList();
                }

            }
            return list;
        }

        /// <summary>
        /// Method to validate if the Workstation already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsWorkstationDuplicate(string workstationName)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@WorkstationName", workstationName));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsWorkstationDuplicate", PrmList));
        }
        /// <summary>
        /// Update WorkstationDetails Details 
        /// </summary>
        public static int UpdateWorkstationDetails(int workstationId, string workstationName, string locationId,
                                                   string configurationId, string areaCode, bool isAdmin, string loggedInUser,
                                                   string wsId)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@WorkstationId", workstationId));
            _params.Add(new FloorSqlParameter("@WorkstationName", workstationName));
            _params.Add(new FloorSqlParameter("@LocationId", locationId));
            _params.Add(new FloorSqlParameter("@ConfigurationId", configurationId));
            _params.Add(new FloorSqlParameter("@AreaCode", areaCode));
            _params.Add(new FloorSqlParameter("@IsAdmin", isAdmin));
            _params.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            _params.Add(new FloorSqlParameter("@WsId", wsId));
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_WorkStationDetails", _params);
        }

        #endregion

        #region ActivityTypeMaster

        /// <summary>
        /// Get Activity Type Table details
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static DataTable GetActivityTypeDetails()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_ActivityTypeDetails", null);
            return dtTableDetails;
        }

        public static List<ÁctivityTypeMasterDTO> GetActivityTypeMaster()
        {
            
            List<ÁctivityTypeMasterDTO> lstactmstr = new List<ÁctivityTypeMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_ActivityTypeDetails", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ÁctivityTypeMasterDTO actmaster = new ÁctivityTypeMasterDTO();
                    actmaster.ActivityType = FloorDBAccess.GetString(dr, "ActivityType");
                    actmaster.ActivityTypeId = FloorDBAccess.GetValue<int>(dr, "ActivityTypeId");
                    actmaster.IsDeleted = FloorDBAccess.GetValue<bool>(dr, "IsDeleted");
                    lstactmstr.Add(actmaster);
                }
            }
            return lstactmstr;


        }

        /// <summary>
        /// Soft delete the record
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int DeleteActivityTypeRecord(string primaryId, string wsId, string loggedInUser, ÁctivityTypeMasterDTO actNew, ÁctivityTypeMasterDTO actOld)
        {
            //audit log added on 20170927 MYAdamas
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - ActivityTypeMaster";
            AuditLog.CreatedBy = loggedInUser;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), actNew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "ActivityTypeMaster";
            //to get referenceid for add ( add haven get the identity id yet)
            string refid = "";

            refid = actNew.ActivityTypeId.ToString();
            AuditLog.ReferenceId = refid;

            AuditLog.UpdateColumns = actOld.DetailedCompare(actNew).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);


            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PrimaryId", primaryId));
            PrmList.Add(new FloorSqlParameter("@WsId", wsId));
            PrmList.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog));
         
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_ActivityTypeRecord", PrmList);
        }

        /// <summary>
        /// Method to validate if the ActivityType already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsActivityTypeDuplicate(string activityType)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ActivityType", activityType));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsActivityTypeDuplicate", PrmList));
        }

        /// <summary>
        /// Update Activity Type Details 
        /// </summary>
        public static int UpdateActivityTypeDetails(int activityTypeId, string activityType, string loggedInUser, string wsId, ÁctivityTypeMasterDTO actNew, ÁctivityTypeMasterDTO actOld)
        {
            //audit log added on 20170927 MYAdamas
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - ActivityTypeMaster";
            AuditLog.CreatedBy = loggedInUser;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), actNew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "ActivityTypeMaster";
            //add will set in store procedure reference id only update set referenceid
            if (audAction != Constants.ActionLog.Add)
            {
                string refid = "";

                refid = actNew.ActivityTypeId.ToString();
                AuditLog.ReferenceId = refid;
            }
            AuditLog.UpdateColumns = actOld.DetailedCompare(actNew).GetPropChanges();
             
            string audlog = CommonBLL.SerializeTOXML(AuditLog);
 
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@ActivityTypeId", activityTypeId));
            _params.Add(new FloorSqlParameter("@ActivityType", activityType));
            _params.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            _params.Add(new FloorSqlParameter("@WsId", wsId));
            //add audit log parameter MYAdamas
            _params.Add(new FloorSqlParameter("@AuditLog", audlog));
      
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_ActivityTypeDetails", _params);
        }

        #endregion

        #region MessageMaster

        public static List<MessagesDTO> GetMessageMaster()
        {
            List<MessagesDTO> lstmsgmstr = new List<MessagesDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_MessageMasterDetails", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MessagesDTO msgmaster = new MessagesDTO();
                    msgmaster.MessageKey = FloorDBAccess.GetString(dr, "MessageKey");
                    msgmaster.MessageText = FloorDBAccess.GetString(dr, "MessageText");
                    msgmaster.Id = FloorDBAccess.GetValue<int>(dr, "Id");
                    lstmsgmstr.Add(msgmaster);
                }
            }
            return lstmsgmstr;


        }
        /// <summary>
        /// Get Message Master Table details
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static DataTable GetMessageMasterDetails()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_MessageMasterDetails", null);
            return dtTableDetails;
        }

        /// <summary>
        /// Method to validate if the Message Key already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsMessageKeyDuplicate(string messageKey)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@MessageKey", messageKey));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsMessageKeyDuplicate", PrmList));
        }

        /// <summary>
        /// Update Activity Type Details 
        /// </summary>
        public static int UpdateMessageMasterDetails(int id, string messageKey, string messageText, string loggedInUser, string wsId,MessagesDTO msgNew,MessagesDTO msgOld)
        {

            //set audit log

            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - Message Master";
            AuditLog.CreatedBy = loggedInUser;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), msgNew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "MESSAGEMASTER";
            string refid = "";

            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {
                refid = msgNew.Id.ToString();
                AuditLog.ReferenceId = refid;
            }


            AuditLog.UpdateColumns = msgOld.DetailedCompare(msgNew).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@Id", id));
            _params.Add(new FloorSqlParameter("@MessageKey", messageKey));
            _params.Add(new FloorSqlParameter("@MessageText", messageText));
            _params.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            _params.Add(new FloorSqlParameter("@WsId", wsId));
            //add audit log parameter MYAdamas
            _params.Add(new FloorSqlParameter("@AuditLog", audlog));

            return FloorDBAccess.ExecuteNonQuery("USP_UPD_MessageMasterDetails", _params);
        }

        #endregion

        #region WasherMaster
        /// <summary>
        /// Get Table details
        /// </summary>
        public static DataTable GetWasherMasterDetails()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_WasherMasterDetails", null);
            return dtTableDetails;
        }
        /// <summary>
        /// Get Glove Type list from AX
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetGloveTypeFromAX()
        {
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveTypeMaster", null))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "GLOVECODE"),
                                DisplayField = FloorDBAccess.GetString(row, "GLOVECODE")
                            }).ToList();
                }
            }
            return list;
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
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveSizeMaster", _params);

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
        /// Method to validate if the Washer already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsWasherDuplicate(int washerNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@WasherNumber", washerNumber));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsWasherDuplicate", PrmList));
        }
        /// <summary>
        /// Update WasherMaster Details 
        /// </summary>
        public static int UpdateWasherMasterDetails(int washerNumber, string locationId,
                                                   string gloveType, string size, bool isStopped, string loggedInUser, string wsId, string audlog)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@WasherNumber", washerNumber));
            _params.Add(new FloorSqlParameter("@LocationId", locationId));
            _params.Add(new FloorSqlParameter("@GloveType", gloveType));
            _params.Add(new FloorSqlParameter("@Size", size));
            _params.Add(new FloorSqlParameter("@IsStopped", isStopped));
            _params.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            _params.Add(new FloorSqlParameter("@WsId", wsId));
            _params.Add(new FloorSqlParameter("@AuditLog", audlog));
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_WasherMasterDetails", _params);
        }

        /// <summary>
        /// Soft delete the record
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int DeleteWasherMasterRecord(string primaryId, string wsId, string loggedInUser, string audLog)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PrimaryId", primaryId));
            PrmList.Add(new FloorSqlParameter("@WsId", wsId));
            PrmList.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audLog));
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_WasherMasterRecord", PrmList);
        }

        #endregion

        #region PalletMaster
        /// <summary>
        /// Method to validate if the PalletId already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsPalletDuplicate(string palletId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PalletId", palletId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsPalletidDuplicate", PrmList));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPalletMasterDetails()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_PalletMaster", null);
            return dtTableDetails;
        }

        /// <summary>
        /// Soft delete the record
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int DeletePalletMasterRecord(string PalletId, string wsId, string loggedInUser)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PalletId", PalletId));
            PrmList.Add(new FloorSqlParameter("@WsId", wsId));
            PrmList.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_PalletMaster", PrmList);
        }


        public static int UpdatePalletMasterDetails(PalletMasterDTO palletMasterDTO, string loggedInUser,string wsId)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@PalletID", palletMasterDTO.PalletId));
            _params.Add(new FloorSqlParameter("@isPreshipment", palletMasterDTO.IsPreshipment));
            _params.Add(new FloorSqlParameter("@isOccupied", palletMasterDTO.Isoccupied));
            _params.Add(new FloorSqlParameter("@isAvailable", palletMasterDTO.IsAvailable));
            _params.Add(new FloorSqlParameter("@Zone", palletMasterDTO.Zone));
            _params.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            _params.Add(new FloorSqlParameter("@WsId", wsId));
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_PalletmasterDetails", _params);
        }

        /// <summary>
        /// Method to validate if the Palletid already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        //public static int IsWorkstationDuplicate(string workstationName)
        //{
        //    List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
        //    PrmList.Add(new FloorSqlParameter("@WorkstationName", workstationName));
        //    return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsWorkstationDuplicate", PrmList));
        //}
        #endregion

        #region DryerMaster
        /// <summary>
        /// Get Table details
        /// </summary>
        public static DataTable GetDryerMasterDetails()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_DryerMasterDetails", null);
            return dtTableDetails;
        }

        /// <summary>
        /// Method to validate if the Dryer already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsDryerDuplicate(int dryerNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@DryerNumber", dryerNumber));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsDryerDuplicate", PrmList));
        }
        /// <summary>
        /// Update DryerMaster Details 
        /// </summary>
        public static int UpdateDryerMasterDetails(int dryerNumber, string locationId, string gloveType, string size,
                                                  bool isStopped, bool hot, bool cold, bool hotAndCold,
                                                  bool checkGlove, bool checkSize, bool isScheduledStop, string loggedInUser,
                                                  string wsId, string audLog)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@DryerNumber", dryerNumber));
            _params.Add(new FloorSqlParameter("@LocationId", locationId));
            _params.Add(new FloorSqlParameter("@GloveType", gloveType));
            _params.Add(new FloorSqlParameter("@Size", size));
            _params.Add(new FloorSqlParameter("@IsStopped", isStopped));
            _params.Add(new FloorSqlParameter("@Hot", hot));
            _params.Add(new FloorSqlParameter("@Cold", cold));
            _params.Add(new FloorSqlParameter("@HotAndCold", hotAndCold));
            _params.Add(new FloorSqlParameter("@CheckGlove", checkGlove));
            _params.Add(new FloorSqlParameter("@CheckSize", checkSize));
            _params.Add(new FloorSqlParameter("@IsScheduledStop", isScheduledStop));
            _params.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            _params.Add(new FloorSqlParameter("@WsId", wsId));
            _params.Add(new FloorSqlParameter("@AuditLog", audLog));
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_DryerMasterDetails", _params);
        }


        /// <summary>
        /// Soft delete the record
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int DeleteDryerMasterRecord(string primaryId, string wsId, string loggedInUser, string audLog)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PrimaryId", primaryId));
            PrmList.Add(new FloorSqlParameter("@WsId", wsId));
            PrmList.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audLog));
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_DryerMasterRecord", PrmList);
        }
        #endregion

        #region ProductionDefectMaster

        /// <summary>
        /// Get Production Defect Master
        /// </summary>
        /// <returns></returns>
        public static List<ProductionDefectMasterDTO> GetProductionDefectMaster()
        {
            List<ProductionDefectMasterDTO> _lstProductionDefectMasterDTO = new List<ProductionDefectMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_ProductionDefectMaster", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ProductionDefectMasterDTO _productionDefectMasterDTO = new ProductionDefectMasterDTO();
                    _productionDefectMasterDTO.RecIndex = _lstProductionDefectMasterDTO.Count + 1;
                    _productionDefectMasterDTO.ProdDefectId = FloorDBAccess.GetValue<int>(dr, "ProdDefectId");
                    _productionDefectMasterDTO.ProdDefectName = FloorDBAccess.GetString(dr, "ProdDefectName");
                    _productionDefectMasterDTO.DefectDescription = FloorDBAccess.GetString(dr, "DefectDescription");
                    _lstProductionDefectMasterDTO.Add(_productionDefectMasterDTO);
                }
            }
            return _lstProductionDefectMasterDTO;
        }

        /// <summary>
        /// Save Production DefectMaster
        /// </summary>
        /// <param name="pdmold"></param>
        /// <param name="pdmnew"></param>
        /// <param name="userid"></param>
        public static void SaveProductionDefectMaster(ProductionDefectMasterDTO pdmold, ProductionDefectMasterDTO pdmnew, string userid)
        {
            ChangeLogDTO ProductionDefectMasterLog = new ChangeLogDTO();
            ProductionDefectMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            ProductionDefectMasterLog.TableName = "ProductionDefectMaster";
            ProductionDefectMasterLog.UserId = userid;
            ProductionDefectMasterLog.UpdateColumns = pdmold.DetailedCompare(pdmnew).GetPropChanges();
            string ProductionDefectMasterRecord = CommonBLL.SerializeTOXML(pdmnew);
            string tbllog = CommonBLL.SerializeTOXML(ProductionDefectMasterLog);
            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ProductionDefectMasterRecord", ProductionDefectMasterRecord));
            PrmList.Add(new FloorSqlParameter("@ProductionDefectMasterLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_ProductionDefectMaster", PrmList);
        }

        #endregion

        #region Bin Master
        /// <summary>
        /// Get Table details
        /// </summary>
        public static DataTable GetBinDetails()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_BinDetails", null);
            return dtTableDetails;
        }

        /// <summary>
        /// Soft delete the record
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int DeleteBinRecord(string primaryId, string wsId, string loggedInUser,BinMasterDTO binmasterNew,BinMasterDTO binmasterOld)
        {
            //audit log added on 20170927 MYAdamas
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - BinMaster";
            AuditLog.CreatedBy = loggedInUser;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), binmasterNew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "BinMaster";
            //to get referenceid for add ( add haven get the identity id yet)
            string refid = "";
           
            refid = binmasterNew.BinNumber.ToString();
            AuditLog.ReferenceId = refid;
         
            AuditLog.UpdateColumns = binmasterOld.DetailedCompare(binmasterNew).GetPropChanges();

             string audlog = CommonBLL.SerializeTOXML(AuditLog);


            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PrimaryId", primaryId));
            PrmList.Add(new FloorSqlParameter("@WsId", wsId));
            PrmList.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog)); 
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_BinRecord", PrmList);
        }

        public static List<BinMasterDTO> GetBinMaster()
        {
            List<BinMasterDTO> lstbinmstr = new List<BinMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_BinDetails", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BinMasterDTO binmaster = new BinMasterDTO();
                    binmaster.BinNumber= FloorDBAccess.GetString(dr, "BinNumber");
                    binmaster.IsAvailable = FloorDBAccess.GetValue<bool>(dr, "IsAvailable");
                    binmaster.LocationId = FloorDBAccess.GetValue<int>(dr, "LocationId");
                    binmaster.AreaId = FloorDBAccess.GetValue<int>(dr, "AreaId");
                    lstbinmstr.Add(binmaster);
                }
            }
            return lstbinmstr;


        }

     


        /// <summary>
        /// Method to validate if the Bin already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsBinDuplicate(string binNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@BinNumber", binNumber));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsBinDuplicate", PrmList));
        }
        /// <summary>
        /// Update Bin Details 
        /// </summary>
        public static int UpdateBinDetails(string binNumber, string locationId, string areaCode, bool isAvailable, string loggedInUser,
                                           string wsId,BinMasterDTO binmasterNew,BinMasterDTO binmasterOld)
        {
            //audit log added on 20170927 MYAdamas
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - BinMaster";
            AuditLog.CreatedBy = loggedInUser;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), binmasterNew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "BinMaster";
          
            string refid = "";

            refid = binmasterNew.BinNumber.ToString();
            AuditLog.ReferenceId = refid;
            AuditLog.UpdateColumns = binmasterOld.DetailedCompare(binmasterNew).GetPropChanges();

             string audlog = CommonBLL.SerializeTOXML(AuditLog);

            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@BinNumber", binNumber));
            _params.Add(new FloorSqlParameter("@LocationId", locationId));
            _params.Add(new FloorSqlParameter("@AreaCode", areaCode));
            _params.Add(new FloorSqlParameter("@IsAvailable", isAvailable));
            _params.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            _params.Add(new FloorSqlParameter("@WsId", wsId));
            _params.Add(new FloorSqlParameter("@AuditLog", audlog)); 
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_BinDetails", _params);
        }
        #endregion

        #region DefectiveGlove
        /// <summary>
        /// Get DefectiveGlove Table details
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static DataTable GetDefectiveGloveDetails()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_DefectiveGloveDetails", null);
            return dtTableDetails;
        }

        public static List<DefectiveGloveMasterDTO> GetDefectiveGloveMaster()
        {
            List<DefectiveGloveMasterDTO> lstdefectglovemstr = new List<DefectiveGloveMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_DefectiveGloveDetails", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DefectiveGloveMasterDTO defectglovemaster = new DefectiveGloveMasterDTO();
                    defectglovemaster.DefectiveGloveReason = FloorDBAccess.GetString(dr, "DefectiveGloveReason");
                    defectglovemaster.TVConfigurable = FloorDBAccess.GetValue<bool>(dr, "TVConfigurable");
                    defectglovemaster.DefectiveGloveType = FloorDBAccess.GetString(dr, "DefectiveGloveType");
                    defectglovemaster.DefectiveGloveId = FloorDBAccess.GetValue<int>(dr, "DefectiveGloveId");
                    lstdefectglovemstr.Add(defectglovemaster);
                }
            }
            return lstdefectglovemstr;


        }

        /// <summary>
        /// Soft delete the record
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int DeleteDefectiveGloveRecord(string primaryId, string wsId, string loggedInUser, string auditLog)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PrimaryId", primaryId));
            PrmList.Add(new FloorSqlParameter("@WsId", wsId));
            PrmList.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            PrmList.Add(new FloorSqlParameter("@AuditLog", auditLog));
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_DefectiveGloveRecord", PrmList);
        }

        /// <summary>
        /// Method to validate if the Defective Glove Reason already exists in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsDefectiveGloveReasonDuplicate(string defectiveGloveType, string defectiveGloveReason)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@DefectiveGloveType", defectiveGloveType));
            PrmList.Add(new FloorSqlParameter("@DefectiveGloveReason", defectiveGloveReason));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsDefectiveGloveReasonDuplicate", PrmList));
        }

        /// <summary>
        /// Update Defective Glove Details 
        /// </summary>
        public static int UpdateDefectiveGloveDetails(int defectiveGloveId, string defectiveGloveType,
                                                    string defectiveGloveReason, bool tvConfigurable, string loggedInUser, string wsId, string auditLog)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@DefectiveGloveId", defectiveGloveId));
            _params.Add(new FloorSqlParameter("@DefectiveGloveType", defectiveGloveType));
            _params.Add(new FloorSqlParameter("@DefectiveGloveReason", defectiveGloveReason));
            _params.Add(new FloorSqlParameter("@TVConfigurable", tvConfigurable));
            _params.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            _params.Add(new FloorSqlParameter("@WsId", wsId));
            _params.Add(new FloorSqlParameter("@AuditLog", auditLog));
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_DefectiveGloveDetails", _params);
        }
        #endregion

        #region LinetMaster

        /// <summary>
        /// Get Line Master
        /// </summary>
        /// <returns></returns>
        public static List<LineMasterDTO> GetLinetMaster()
        {
            List<LineMasterDTO> lstLinemaster = new List<LineMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_LineMaster", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    LineMasterDTO linemaster = new LineMasterDTO();
                   // linemaster.RecIndex = lstLinemaster.Count + 1;
                    linemaster.LocationId = FloorDBAccess.GetValue<int>(dr, "LocationId");
                    linemaster.LineNumber = FloorDBAccess.GetString(dr, "LineNumber");
                    lstLinemaster.Add(linemaster);
                }
            }
            return lstLinemaster;
        }

        /// <summary>
        /// Save Line Master
        /// </summary>
        /// <param name="lmld"></param>
        /// <param name="lmnew"></param>
        /// <param name="userid"></param>
        public static void SaveLinetMaster(LineMasterDTO lmld, LineMasterDTO lmnew, string userid)
        {
            ChangeLogDTO ProductionDefectMasterLog = new ChangeLogDTO();
            ProductionDefectMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            ProductionDefectMasterLog.TableName = "LineMaster";
            ProductionDefectMasterLog.UserId = userid;
            ProductionDefectMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string ProductionDefectMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(ProductionDefectMasterLog);


            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - LineMaster";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), lmnew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "LineMaster";
            string refid = "";

            refid = lmnew.LineNumber.ToString();

            AuditLog.ReferenceId = refid;
            AuditLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
             
            string audlog = CommonBLL.SerializeTOXML(AuditLog);
            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@LinetMasterRecord", ProductionDefectMasterRecord));
            PrmList.Add(new FloorSqlParameter("@LineMasterLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            //add audit log MYAdamas
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog)); 
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_LineMaster", PrmList);

       
        }

        public static List<DropdownDTO> GetLocation()
        {
            List<DropdownDTO> Locations = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_Plants", null))
            {
                if (dt != null && dt.Rows.Count > Constants.ZERO)
                {
                    Locations = (from DataRow dr in dt.Rows
                                 select new DropdownDTO
                                 {
                                     IDField = FloorDBAccess.GetString(dr, "LocationId"),
                                     DisplayField = FloorDBAccess.GetString(dr, "LocationName")
                                 }).ToList();
                }
            }
            return Locations;
        }

        #endregion

        #region GloveSizeMaster

        /// <summary>
        /// Get Glove Size Master
        /// </summary>
        /// <returns></returns>
        public static List<GloveSizeMasterDTO> GetGloveSizeMaster()
        {
            List<GloveSizeMasterDTO> lstglovesize = new List<GloveSizeMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_GloveSizeMaster", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    GloveSizeMasterDTO glovesize = new GloveSizeMasterDTO();
                    //glovesize.RecIndex = lstglovesize.Count + 1;
                    glovesize.CommonSize = FloorDBAccess.GetString(dr, "CommonSize");
                    glovesize.Description = FloorDBAccess.GetString(dr, "Description");
                    glovesize.CommonSizeID = FloorDBAccess.GetValue<int>(dr, "CommonSizeId");
                    glovesize.Stopped= FloorDBAccess.GetValue<int>(dr, "Stopped");
                    glovesize.OldSize = FloorDBAccess.GetString(dr, "OldSize");
                    lstglovesize.Add(glovesize);
                }
            }
            return lstglovesize;
        }

        public static DataTable GetGloveSizeMasterData()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_GET_GloveSizeMaster", null);
            return dtTableDetails;
        }

        public static void SaveGloveSize(GloveSizeMasterDTO lmld, GloveSizeMasterDTO lmnew, string userid)
        {
            ChangeLogDTO GloveSizeMasterLog = new ChangeLogDTO();
            GloveSizeMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            GloveSizeMasterLog.TableName = "AX_AVACOMMONSIZE";
            GloveSizeMasterLog.UserId = userid;
            GloveSizeMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string GloveSizeMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(GloveSizeMasterLog);

            //set audit log

            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - GloveSizeMaster";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), lmnew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "AX_AVACOMMONSIZE";
            string refid = "";

            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {
                refid = lmnew.CommonSizeID.ToString();
                AuditLog.ReferenceId = refid;
            }
          

            AuditLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);
 
            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveSizeMasterRecord", GloveSizeMasterRecord));
            PrmList.Add(new FloorSqlParameter("@GloveSizeMasterLog", tbllog));
            //add auditlog parameter MYAdamas
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog)); 
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_GloveSizeMaster", PrmList);

          
        }

        public static int IsGloveSizeDuplicate(string CommonSize)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@CommonSize", CommonSize));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsGloveSizeDuplicate", PrmList));
        }

        public static List<DropdownDTO> GetGloveSizeMasterList()
        {
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_GloveSizeMaster", null))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "CommonSize"),
                                DisplayField = FloorDBAccess.GetString(row, "CommonSize")
                            }).ToList();
                }

            }
            return list;
        }
        #endregion

        #region BatchTypeMaster

        /// <summary>
        /// Get Batch Type Master
        /// </summary>
        /// <returns></returns>
        public static List<BatchTypeMasterDTO> GetBatchTypeMaster()
        {
            List<BatchTypeMasterDTO> lstbatchtype = new List<BatchTypeMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_BatchTypeMaster", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BatchTypeMasterDTO batchtype = new BatchTypeMasterDTO();

                    batchtype.BatchTypeId = FloorDBAccess.GetValue<int>(dr, "BatchTypeId");
                    batchtype.BatchType = FloorDBAccess.GetString(dr, "BatchType");
                    batchtype.Description = FloorDBAccess.GetString(dr, "Description");
                    lstbatchtype.Add(batchtype);
                }
            }
            return lstbatchtype;
        }
        public static DataTable GetBatchTypeMasterData()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_GET_BatchTypeMaster", null);
            return dtTableDetails;
        }

        public static void SaveBatchType(BatchTypeMasterDTO lmld, BatchTypeMasterDTO lmnew, string userid)
        {
            ChangeLogDTO BatchTypeMasterLog = new ChangeLogDTO();
            BatchTypeMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            BatchTypeMasterLog.TableName = "EnumMaster";
            BatchTypeMasterLog.UserId = userid;
            BatchTypeMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string BatchTypeMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(BatchTypeMasterLog);

            //audit log
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - BatchTypeMaster";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), lmnew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "EnumMaster";
            string refid = "";
            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {
                refid = lmnew.BatchTypeId.ToString();
                AuditLog.ReferenceId = refid;
            }

            AuditLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
           string audlog = CommonBLL.SerializeTOXML(AuditLog);
             
         
            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@BatchTypeMasterRecord", BatchTypeMasterRecord));
            PrmList.Add(new FloorSqlParameter("@BatchTypeMasterLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog)); 
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_BatchTypeMaster", PrmList);
            
          
        }


        public static int IsBatchTypeDuplicate(string BatchType,string Description)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@BatchType", BatchType));
            PrmList.Add(new FloorSqlParameter("@Description", Description));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsBatchTypeDuplicate", PrmList));
        }

        public static int IsBatchTypeDuplicateEdit(string BatchType, string Description,int BatchTypeId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@BatchType", BatchType));
            PrmList.Add(new FloorSqlParameter("@Description", Description));

            PrmList.Add(new FloorSqlParameter("@EnumId", BatchTypeId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsBatchTypeDuplicateEdit", PrmList));
        }
        #endregion


        #region QCTypeMaster

        /// <summary>
        /// Get Line Master
        /// </summary>
        /// <returns></returns>
        public static List<QCTypeMasterDTO> GetQCTypeMaster()
        {
            List<QCTypeMasterDTO> lstQCTypemaster = new List<QCTypeMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_QCTypeMaster", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    QCTypeMasterDTO qctypemaster = new QCTypeMasterDTO();
                    qctypemaster.QCTypeId = FloorDBAccess.GetValue<int>(dr, "QCTypeId");
                    qctypemaster.QCType = FloorDBAccess.GetString(dr, "QCType");
                    qctypemaster.Description = FloorDBAccess.GetString(dr, "Description");
                    qctypemaster.Stopped = FloorDBAccess.GetValue<int>(dr, "Stopped");
                    qctypemaster.QCEfficiency = FloorDBAccess.GetValue<int>(dr, "QCEfficiency");
                    lstQCTypemaster.Add(qctypemaster);
                }
            }
            return lstQCTypemaster;
        }

        public static DataTable GetQCTypeMasterData()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_GET_QCTypeMaster", null);
            return dtTableDetails;
        }
        /// <summary>
        /// Save Line Master
        /// </summary>
        /// <param name="lmld"></param>
        /// <param name="lmnew"></param>
        /// <param name="userid"></param>
        public static void SaveQCType(QCTypeMasterDTO lmld, QCTypeMasterDTO lmnew, string userid)
        {
            ChangeLogDTO QCMasterLog = new ChangeLogDTO();
            QCMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            QCMasterLog.TableName = "AX_AVAQCTYPETABLE";
            QCMasterLog.UserId = userid;
            QCMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string QCMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(QCMasterLog);
            //audit log
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - QCTypeMaster";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), lmnew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "AX_AVAQCTYPETABLE";
            string refid = "";

            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {

                refid = lmnew.QCTypeId.ToString();


                AuditLog.ReferenceId = refid;
            }

            AuditLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@QCTypeMasterRecord", QCMasterRecord));
            PrmList.Add(new FloorSqlParameter("@QCTypeMasterLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog)); 
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_QCTypeMaster", PrmList);


          
        }

        public static int IsQCTypeDuplicate(string QCType)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@QCType", QCType));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsQCTypeDuplicate", PrmList));
        }

        #endregion

        #region locationmaster

        public static List<LocationMasterDTO> GetLocationMaster()
        {
            List<LocationMasterDTO> lstglocationmstr = new List<LocationMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_LocationMaster", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    LocationMasterDTO locmas = new LocationMasterDTO();

                    locmas.LocationName = FloorDBAccess.GetString(dr, "LocationName");
                    locmas.LocationAreaCode = FloorDBAccess.GetString(dr, "LocationAreaCode");
                    locmas.LocationId = FloorDBAccess.GetValue<int>(dr, "LocationId");
                    lstglocationmstr.Add(locmas);
                }
            }
            return lstglocationmstr;
        }

        public static DataTable GetLocationMasterData()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_GET_LocationMaster", null);
            return dtTableDetails;
        }

        public static void SaveLocationMaster(LocationMasterDTO lmld, LocationMasterDTO lmnew, string userid)
        {
            ChangeLogDTO LocationMasterLog = new ChangeLogDTO();
            LocationMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            LocationMasterLog.TableName = "LocationMaster";
            LocationMasterLog.UserId = userid;
            LocationMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string LocationMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(LocationMasterLog);
            //audit log
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - LocationMaster";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), lmnew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "LocationMaster";
            string refid = "";

            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {

                refid = lmnew.LocationId.ToString();
                AuditLog.ReferenceId = refid;
            }

            AuditLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@LocationMasterRecord", LocationMasterRecord));
            PrmList.Add(new FloorSqlParameter("@LocationMasterLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog));      
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_LocationMaster", PrmList);

        
        }

        public static int IsLocationMasterDuplicate(string LocationName)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@LocationName", LocationName));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsLocationMasterDuplicate", PrmList));
        }
        #endregion

        #region ShiftMaster

        public static List<ShiftMasterDTO> GetShiftMaster()
        {
            List<ShiftMasterDTO> lstshiftmstr = new List<ShiftMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_ShiftMasterData", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ShiftMasterDTO shiftmaster = new ShiftMasterDTO();

                    shiftmaster.Name = FloorDBAccess.GetString(dr, "Name");
              
                    shiftmaster.InTime = FloorDBAccess.GetString(dr, "InTime");

                    shiftmaster.OutTime = FloorDBAccess.GetString(dr, "OutTime");
                    shiftmaster.ShiftId = FloorDBAccess.GetValue<int>(dr, "ShiftId");
                    shiftmaster.GroupType= FloorDBAccess.GetString(dr, "GroupType");

                    shiftmaster.GroupTypeId = FloorDBAccess.GetValue<int>(dr, "GroupTypeId");
                    lstshiftmstr.Add(shiftmaster);
                }
            }
            return lstshiftmstr;
        }

        public static DataTable GetShiftMasterData()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_GET_ShiftMasterData", null);
            return dtTableDetails;
        }

        public static void SaveShiftMaster(ShiftMasterDTO lmld, ShiftMasterDTO lmnew, string userid)
        {
            ChangeLogDTO ShiftMasterLog = new ChangeLogDTO();
            ShiftMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            ShiftMasterLog.TableName = "ShiftMaster";
            ShiftMasterLog.UserId = userid;
            ShiftMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();

          
            string ShiftMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(ShiftMasterLog);
            //audit log
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - ShiftMaster";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), lmnew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "ShiftMaster";
            //to get referenceid for add ( add haven get the identity id yet)
            string refid = "";
            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {

                refid = lmnew.ShiftId.ToString();
                AuditLog.ReferenceId = refid;
            }
            AuditLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();

           string audlog = CommonBLL.SerializeTOXML(AuditLog);

            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ShiftMasterRecord", ShiftMasterRecord));
            PrmList.Add(new FloorSqlParameter("@ShiftMasterLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog)); 
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_ShiftMaster", PrmList);

        
        }

        public static int IsShiftMasterDuplicate(string Name,string GroupType )
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ShiftName", Name));
            PrmList.Add(new FloorSqlParameter("@GroupType", GroupType));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsShiftMasterDuplicate", PrmList));
        }
        public static int IsShiftMasterDuplicate(string Name, string GroupType,int ShiftId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ShiftName", Name));
            PrmList.Add(new FloorSqlParameter("@GroupType", GroupType));

            PrmList.Add(new FloorSqlParameter("@ShiftId", ShiftId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsShiftMasterDuplicateEdit", PrmList));
        }


        public static string GetGroupTypeByGroupId(int GroupTypeId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GroupTypeId", GroupTypeId));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_GroupTypeByGroupTypeId", PrmList));
        }
        #endregion

        #region GloveCategoryMaster

        /// <summary>
        /// Get Glove Category Master
        /// </summary>
        /// <returns></returns>
        public static List<GloveCategoryMasterDTO> GetGloveCategoryMaster()
        {
            List<GloveCategoryMasterDTO> lstglovecategory = new List<GloveCategoryMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_GloveCategoryMaster", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    GloveCategoryMasterDTO glovecategory = new GloveCategoryMasterDTO();

                    glovecategory.GloveCategory = FloorDBAccess.GetString(dr, "GloveCategory");
                    glovecategory.Description = FloorDBAccess.GetString(dr, "Description");
                    glovecategory.GloveCategoryId = FloorDBAccess.GetValue<int>(dr, "GloveCategoryId");
                    glovecategory.Stopped = FloorDBAccess.GetValue<int>(dr, "Stopped");
                    lstglovecategory.Add(glovecategory);
                }
            }
            return lstglovecategory;
        }

        public static DataTable GetGloveCategoryMasterData()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_GET_GloveCategoryMaster", null);
            return dtTableDetails;
        }

        public static void SaveGloveCategory(GloveCategoryMasterDTO lmld, GloveCategoryMasterDTO lmnew, string userid)
        {
            ChangeLogDTO GloveCategoryMasterLog = new ChangeLogDTO();
            GloveCategoryMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            GloveCategoryMasterLog.TableName = "AX_AVAGLOVECATEGORY";
            GloveCategoryMasterLog.UserId = userid;
            GloveCategoryMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string GloveCategoryMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(GloveCategoryMasterLog);
            //audit log
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - GloveCategoryMaster";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), lmnew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "AX_AVAGLOVECATEGORY";
            string refid = "";

            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {

                refid = lmnew.GloveCategoryId.ToString();
                AuditLog.ReferenceId = refid;
            }

            AuditLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveCategoryMasterRecord", GloveCategoryMasterRecord));
            PrmList.Add(new FloorSqlParameter("@GloveCategoryMasterLog", tbllog));
            //add audit log parameter MYAdamas 
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog));
            PrmList.Add(new FloorSqlParameter("@UserId", userid));


            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_GloveCategoryMaster", PrmList);            
        }

        public static int IsGloveCategoryDuplicate(string GloveCategory)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@GloveCategory", GloveCategory));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsGloveCategoryDuplicate", PrmList));
        }
        #endregion

        #region InnerLabelSetMaster

        /// <summary>
        /// Get InnerLabelSetMaster
        /// </summary>
        /// <returns></returns>

        public static List<DropdownDTO> GetLabelSetStatus()
        {
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_LabelSetStatus", null))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "EnumValue"),
                                DisplayField = FloorDBAccess.GetString(row, "EnumText")
                            }).ToList();
                }

            }
            return list;
        }

        public static List<InnerLabelSetMasterDTO> GetInnerLabelSetMaster()
        { 
            List<InnerLabelSetMasterDTO> lstinnerlabelset = new List<InnerLabelSetMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_InnerLabelSetMaster", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    InnerLabelSetMasterDTO innerlabelset = new InnerLabelSetMasterDTO();

                    innerlabelset.InnerLabelSetNo = FloorDBAccess.GetString(dr, "InnerLabelSetNo");
                    innerlabelset.Description = FloorDBAccess.GetString(dr, "Description");
                    innerlabelset.InnerLabelSetId = FloorDBAccess.GetValue<int>(dr, "InnerLabelSetId");
                    innerlabelset.SpecialCodeText = FloorDBAccess.GetString(dr, "SpecialCodeText");
                    innerlabelset.SpecialCode = FloorDBAccess.GetValue<bool>(dr, "SpecialCode");
                    innerlabelset.CustomDate = FloorDBAccess.GetValue<bool>(dr, "CustomDate");
                    innerlabelset.Status =Convert.ToInt32(FloorDBAccess.GetString(dr, "Status"));
                    lstinnerlabelset.Add(innerlabelset);
                }
            }
            return lstinnerlabelset;
        }

        public static DataTable GetInnerLabelSetMasterData()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_GET_InnerLabelSetMaster", null);
            return dtTableDetails;
        }

        public static void SaveInnerLabelSet(InnerLabelSetMasterDTO lmld, InnerLabelSetMasterDTO lmnew, string userid)
        {
            ChangeLogDTO InnerLabelSetMasterLog = new ChangeLogDTO();
            InnerLabelSetMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            InnerLabelSetMasterLog.TableName = "InnerLabelSet";
            InnerLabelSetMasterLog.UserId = userid;
            InnerLabelSetMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string InnerLabelSetMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(InnerLabelSetMasterLog);

            //audit log
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - InnerLabelSetMaster";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), lmnew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "InnerLabelSet";
            string refid = "";

            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {

                refid = lmnew.InnerLabelSetId.ToString();

                AuditLog.ReferenceId = refid;
            }

            AuditLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);
             
            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@InnerLabelSetMasterRecord", InnerLabelSetMasterRecord));
            PrmList.Add(new FloorSqlParameter("@InnerLabelSetLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog)); 
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_InnerLabelSet", PrmList);

             
        }

        public static int IsInnerLabelSetDuplicate(string InnerLabelSetNo)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@InnerLabelSetNo", InnerLabelSetNo));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsInnerLabelSetDuplicate", PrmList));
        }
        #endregion

        #region OuterLabelSetMaster

        /// <summary>
        /// Get OuterLabelSetMaster
        /// </summary>
        /// <returns></returns>

        public static List<OuterLabelSetMasterDTO> GetOuterLabelSetMaster()
        {
            List<OuterLabelSetMasterDTO> lstouterlabelset = new List<OuterLabelSetMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_OuterLabelSetMaster", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OuterLabelSetMasterDTO outerlabelset = new OuterLabelSetMasterDTO();

                    outerlabelset.OuterLabelSetNo = FloorDBAccess.GetString(dr, "OuterLabelSetNo");
                    outerlabelset.Description = FloorDBAccess.GetString(dr, "Description");
                    outerlabelset.OuterLabelSetId = FloorDBAccess.GetValue<int>(dr, "OuterLabelSetId");
                   
                    outerlabelset.GCLabel = FloorDBAccess.GetValue<bool>(dr, "GCLabel");
                    outerlabelset.CustomDate = FloorDBAccess.GetValue<bool>(dr, "CustomDate");
                    outerlabelset.Status = Convert.ToInt32(FloorDBAccess.GetString(dr, "Status"));
                    lstouterlabelset.Add(outerlabelset);
                }
            }
            return lstouterlabelset;
        }

        public static DataTable GetOuterLabelSetMasterData()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_GET_OuterLabelSetMaster", null);
            return dtTableDetails;
        }

        public static void SaveOuterLabelSet(OuterLabelSetMasterDTO lmld, OuterLabelSetMasterDTO lmnew, string userid)
        {
            ChangeLogDTO OuterLabelSetMasterLog = new ChangeLogDTO();
            OuterLabelSetMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            OuterLabelSetMasterLog.TableName = "OuterLabelSet";
            OuterLabelSetMasterLog.UserId = userid;
            OuterLabelSetMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string InnerLabelSetMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(OuterLabelSetMasterLog);
            //audit log
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - OuterLabelSetMaster";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), lmnew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "OuterLabelSet";
            string refid = "";

            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {

                refid = lmnew.OuterLabelSetId.ToString();


                AuditLog.ReferenceId = refid;
            }
            AuditLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();

            string updatecolumnsLog = CommonBLL.SerializeTOXML(AuditLog.UpdateColumns);
            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@OuterLabelSetMasterRecord", InnerLabelSetMasterRecord));
            PrmList.Add(new FloorSqlParameter("@OuterLabelSetLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog)); 
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_OuterLabelSet", PrmList);
 
        }

        public static int IsOuterLabelSetDuplicate(string OuterLabelSetNo)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@OuterLabelSetNo", OuterLabelSetNo));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsOuterLabelSetDuplicate", PrmList));
        }
        #endregion

        #region QAIAQReferenceFirst

        /// <summary>
        /// Get QaiAQReferenceFirst
        /// </summary>
        /// <returns></returns>
        public static List<QAIAQReferenceFirstDTO> GetQaiAQReferenceFirst()
        {
            List<QAIAQReferenceFirstDTO> lstQAIAQReferenceFirstlst = new List<QAIAQReferenceFirstDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_QAIAQReferenceFirst", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    QAIAQReferenceFirstDTO QaiAQReferenceFirst = new QAIAQReferenceFirstDTO();
                    //QaiAQReferenceFirst.RecIndex = lstQAIAQReferenceFirstlst.Count + 1;
                    QaiAQReferenceFirst.RefID = FloorDBAccess.GetValue<int>(dr, "RefID");
                    QaiAQReferenceFirst.DefectCategoryTypeId = FloorDBAccess.GetValue<int>(dr, "DefectCategoryTypeId");
                    QaiAQReferenceFirst.WTSamplingSize = FloorDBAccess.GetValue<int>(dr, "WTSamplingSize");
                    QaiAQReferenceFirst.VTSamplingSize = FloorDBAccess.GetValue<int>(dr, "VTSamplingSize");
                    QaiAQReferenceFirst.AQLID = FloorDBAccess.GetValue<int>(dr, "AQLID");
                    QaiAQReferenceFirst.QCTypeId = FloorDBAccess.GetString(dr, "QCTypeId");
                    QaiAQReferenceFirst.DefectMinValue = FloorDBAccess.GetValue<int>(dr, "DefectMinValue");
                    QaiAQReferenceFirst.DefectMaxValue = FloorDBAccess.GetValue<int>(dr, "DefectMaxValue");
                    QaiAQReferenceFirst.IsResample = FloorDBAccess.GetValue<bool>(dr, "IsResample");
                    QaiAQReferenceFirst.ResampleRound = FloorDBAccess.GetValue<int>(dr, "ResampleRound");
                    QaiAQReferenceFirst.CustomerTypeId = FloorDBAccess.GetValue<int>(dr, "CustomerType");
                    lstQAIAQReferenceFirstlst.Add(QaiAQReferenceFirst);
                }
            }
            return lstQAIAQReferenceFirstlst;
        }

        /// <summary>
        /// Save QAIReferenceFirst
        /// </summary>
        /// <param name="lmld"></param>
        /// <param name="lmnew"></param>
        /// <param name="userid"></param>
        public static void SaveQAIReferenceFirst(QAIAQReferenceFirstDTO lmld, QAIAQReferenceFirstDTO lmnew, string userid)
        {
            ChangeLogDTO ProductionDefectMasterLog = new ChangeLogDTO();
            ProductionDefectMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            ProductionDefectMasterLog.TableName = "QAIAQReferenceFirst";
            ProductionDefectMasterLog.UserId = userid;
            ProductionDefectMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string ProductionDefectMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(ProductionDefectMasterLog);

            //audit log 20170928 MYAdamas
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - QAIAQReferenceFirst";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), lmnew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "QAIAQReferenceFirst";
            string refid = "";

            if (audAction != Constants.ActionLog.Add)            //if add set xml reference id in storedprocedure 
            {
                refid = lmnew.RefID.ToString();

                AuditLog.ReferenceId = refid;
            }
            AuditLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();

            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            int rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@QaiFirsttRecord", ProductionDefectMasterRecord));
            PrmList.Add(new FloorSqlParameter("@QaiLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            //audit log for audit log master 20170928 
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog));
          rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_QAIAQReferenceFirst", PrmList);

           

        }

        /// <summary>
        /// Get QAIAQL
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetQAIAQL()
        {
            List<DropdownDTO> _locations = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_QAIAQL", null))
            {
                if (dt != null && dt.Rows.Count > Constants.ZERO)
                {
                    _locations = (from DataRow dr in dt.Rows
                                 select new DropdownDTO
                                 {
                                     IDField = FloorDBAccess.GetString(dr, "AQLID"),
                                     DisplayField = FloorDBAccess.GetString(dr, "AQLName")
                                 }).ToList();
                }
            }
            return _locations;
        }
        /// <summary>
        /// Get defect category type id to check duplicate when add 
        /// </summary>
        /// <returns></returns>
        
        public static string GetDefectCategoryTypeId()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_GET_QAIAQReferenceFirstDefectCategoryTypeId", null);
            return dtTableDetails.Rows[0][0].ToString();
        }
        #endregion

        #region QITestResultAQL

        /// <summary>
        /// Get QITestResultAQL
        /// </summary>
        /// <returns></returns>
        public static List<QITestResultAQLDTO> GetQITestResultAQL()
        {
            List<QITestResultAQLDTO> lstQITestResultAQL = new List<QITestResultAQLDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_QITestResultAQL", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    QITestResultAQLDTO TestResultID = new QITestResultAQLDTO();
                    TestResultID.RecIndex = lstQITestResultAQL.Count + 1;
                    TestResultID.TestResultID = FloorDBAccess.GetValue<int>(dr, "TestResultID");
                    TestResultID.WTSamplingSize = FloorDBAccess.GetValue<int>(dr, "WTSamplingSize");
                    TestResultID.DefectMinVal = FloorDBAccess.GetValue<int>(dr, "DefectMinVal");
                    TestResultID.DefectMaxVal = FloorDBAccess.GetValue<int>(dr, "DefectMaxVal");
                    TestResultID.AQLID = FloorDBAccess.GetValue<int>(dr, "AQLID");
                    TestResultID.QCTypeId = FloorDBAccess.GetString(dr, "QCTypeId");
                    TestResultID.VTSamplingSize = FloorDBAccess.GetValue<int>(dr, "VTSamplingSize");
                    TestResultID.CustomerTypeId = FloorDBAccess.GetValue<int>(dr, "CustomerType");
                    lstQITestResultAQL.Add(TestResultID);
                }
            }
            return lstQITestResultAQL;
        }

        /// <summary>
        /// Save QITestResultAQL
        /// </summary>
        /// <param name="pdmold"></param>
        /// <param name="pdmnew"></param>
        /// <param name="userid"></param>
        public static void SaveQITestResultAQL(QITestResultAQLDTO lmld, QITestResultAQLDTO lmnew, string userid)
        {
            ChangeLogDTO ProductionDefectMasterLog = new ChangeLogDTO();
            ProductionDefectMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            ProductionDefectMasterLog.TableName = "QITestResultAQL";
            ProductionDefectMasterLog.UserId = userid;
            ProductionDefectMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string ProductionDefectMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(ProductionDefectMasterLog);
            int rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@QITestResultAQLRecord", ProductionDefectMasterRecord));
            PrmList.Add(new FloorSqlParameter("@QiLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_QITestResultAQL", PrmList);
        }

        #endregion

        #region QAIAQCosmeticReferenceSecond
        /// <summary>
        /// Get Table details
        /// </summary>
        public static DataTable GetQAIAQCosmeticReferenceDetails()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_QAIAQCosmeticReferenceDetails", null);
            return dtTableDetails;
        }

        public static List<QAIAQCosmeticReferenceSecondDTO> GetQAIAQCosmeticReference()
        {
            List<QAIAQCosmeticReferenceSecondDTO> lstQAIAQReferenceSecondlst = new List<QAIAQCosmeticReferenceSecondDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QAIAQCosmeticReferenceDetails", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    QAIAQCosmeticReferenceSecondDTO QaiAQReferenceSecond = new QAIAQCosmeticReferenceSecondDTO();
                    // QaiAQReferenceFirst.RecIndex = lstQAIAQReferenceFirstlst.Count + 1;
                    QaiAQReferenceSecond.Id = FloorDBAccess.GetValue<int>(dr, "Id");
                    QaiAQReferenceSecond.AQLID = FloorDBAccess.GetString(dr, "AQLID");
                    QaiAQReferenceSecond.QCType = FloorDBAccess.GetString(dr, "QCTypeId");
                    QaiAQReferenceSecond.MajorDefectMinVal = FloorDBAccess.GetString(dr, "MajorDefectMinValue");
                    QaiAQReferenceSecond.MajorDefectMaxVal = FloorDBAccess.GetString(dr, "MajorDefectMaxValue");
                    QaiAQReferenceSecond.MinorDefectMinVal = FloorDBAccess.GetString(dr, "MinorDefectMinValue");
                    QaiAQReferenceSecond.MinorDefectMaxVal = FloorDBAccess.GetString(dr, "MinorDefectMaxValue");
                    QaiAQReferenceSecond.QCTypeOrder = FloorDBAccess.GetString(dr, "QCTypeOrder");
                    QaiAQReferenceSecond.IsDeleted= FloorDBAccess.GetValue<bool>(dr, "IsDeleted");
                    QaiAQReferenceSecond.WorkStationId= FloorDBAccess.GetString(dr, "QCTypeOrder");
                    QaiAQReferenceSecond.CustomerTypeId = FloorDBAccess.GetString(dr, "CustomerTypeId");
                    QaiAQReferenceSecond.CustomerType = FloorDBAccess.GetString(dr, "CustomerType");
                    QaiAQReferenceSecond.DefectCategoryGroupId = FloorDBAccess.GetString(dr, "DefectCategoryGroupId");
                    QaiAQReferenceSecond.DefectCategoryGroup = FloorDBAccess.GetString(dr, "DefectCategoryGroup");
                    QaiAQReferenceSecond.EnumModuleId = FloorDBAccess.GetString(dr, "EnumModuleId");
                    QaiAQReferenceSecond.Module = FloorDBAccess.GetString(dr, "Module");


                    lstQAIAQReferenceSecondlst.Add(QaiAQReferenceSecond);
                }
            }
            return lstQAIAQReferenceSecondlst;
        }

        /// <summary>
        /// Update QAIAQCosmeticReference Details 
        /// </summary>
        public static int UpdateQAIAQCosmeticRefDetails(QAIAQCosmeticReferenceSecondDTO aqlDTO, QAIAQCosmeticReferenceSecondDTO aqlDTOold,string loggedInUser)
        {
            //audit log added on 20170927 MYAdamas
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - QAIAQCosmeticReferenceSecond";
            AuditLog.CreatedBy = loggedInUser;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), aqlDTO.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "QAIAQCosmeticReferenceSecond";
             
            if (audAction != Constants.ActionLog.Add)
            {
                string refid = "";

                refid = aqlDTO.Id.ToString();
                AuditLog.ReferenceId = refid;
            }
           

            AuditLog.UpdateColumns = aqlDTOold.DetailedCompare(aqlDTO).GetPropChanges();
             
            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@ID", aqlDTO.Id));
            _params.Add(new FloorSqlParameter("@AQLID", aqlDTO.AQLID));
            _params.Add(new FloorSqlParameter("@QCType", aqlDTO.QCType));
            _params.Add(new FloorSqlParameter("@MajorDefectMinVal", aqlDTO.MajorDefectMinVal));
            _params.Add(new FloorSqlParameter("@MajorDefectMaxVal", aqlDTO.MajorDefectMaxVal));
            _params.Add(new FloorSqlParameter("@MinorDefectMinVal", aqlDTO.MinorDefectMinVal));
            _params.Add(new FloorSqlParameter("@MinorDefectMaxVal", aqlDTO.MinorDefectMaxVal));
            _params.Add(new FloorSqlParameter("@QCTypeOrder", aqlDTO.QCTypeOrder));
            _params.Add(new FloorSqlParameter("@LoggedInUser", aqlDTO.OperatorId));
            _params.Add(new FloorSqlParameter("@WsId", aqlDTO.WorkStationId));
            _params.Add(new FloorSqlParameter("@CustomerTypeId", aqlDTO.CustomerTypeId));
            _params.Add(new FloorSqlParameter("@DefectCategoryGroupId", aqlDTO.DefectCategoryGroupId));
            _params.Add(new FloorSqlParameter("@EnumModuleId", aqlDTO.EnumModuleId));
            //add audit log parameter MYAdamas
            _params.Add(new FloorSqlParameter("@AuditLog", audlog)); 
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_QAIAQCosmeticRefDetails", _params);
        }
        /// <summary>
        /// Soft delete the record
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int DeleteQAIAQCosmeticRecord(string primaryId, string wsId, string loggedInUser, QAIAQCosmeticReferenceSecondDTO qainew,QAIAQCosmeticReferenceSecondDTO qaiold)
        {
            //audit log added on 20170927 MYAdamas
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - QAIAQCosmeticReferenceSecond";
            AuditLog.CreatedBy = loggedInUser;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), qainew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "QAIAQCosmeticReferenceSecond";
            
            string refid = "";

            refid = qainew.Id.ToString();
            AuditLog.ReferenceId = refid;
            AuditLog.UpdateColumns = qaiold.DetailedCompare(qainew).GetPropChanges();
             
            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PrimaryId", primaryId));
            PrmList.Add(new FloorSqlParameter("@WsId", wsId));
            PrmList.Add(new FloorSqlParameter("@LoggedInUser", loggedInUser));
            //audit log for audit log master 20170928 
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog)); 
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_QAIAQCosmeticRecord", PrmList);
        }

        /// <summary>
        /// Method to validate if the QAI AQL Cosmetic reference record already exists.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsQAIAQCosmeticRefDuplicate(QAIAQCosmeticReferenceSecondDTO aqlDTO)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ID", aqlDTO.Id));
            PrmList.Add(new FloorSqlParameter("@AQLID", aqlDTO.AQLID));
            PrmList.Add(new FloorSqlParameter("@QCType", aqlDTO.QCType));
            PrmList.Add(new FloorSqlParameter("@MajorDefectMinVal", aqlDTO.MajorDefectMinVal));
            PrmList.Add(new FloorSqlParameter("@MajorDefectMaxVal", aqlDTO.MajorDefectMaxVal));
            PrmList.Add(new FloorSqlParameter("@MinorDefectMinVal", aqlDTO.MinorDefectMinVal));
            PrmList.Add(new FloorSqlParameter("@MinorDefectMaxVal", aqlDTO.MinorDefectMaxVal));
            PrmList.Add(new FloorSqlParameter("@QCTypeOrder", aqlDTO.QCTypeOrder));
            PrmList.Add(new FloorSqlParameter("@CustomerTypeId", aqlDTO.CustomerTypeId));
            PrmList.Add(new FloorSqlParameter("@DefectCategoryGroupId", aqlDTO.DefectCategoryGroupId));
            PrmList.Add(new FloorSqlParameter("@EnumModuleId", aqlDTO.EnumModuleId));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsQAIAQCosmeticRefDuplicate", PrmList));
        }
        #endregion

        #region PreshipmentSamplingPlanMaster

        /// <summary>
        /// Get Batch Type Master
        /// </summary>
        /// <returns></returns>
        public static List<PreshipmentSamplingPlanMasterDTO> GetPreshipmentSamplingPlanMaster()
        {
            List<PreshipmentSamplingPlanMasterDTO> lstpreshipmentsampling = new List<PreshipmentSamplingPlanMasterDTO>();
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_PreshipmentSamplingPlanMaster", null);
            if (dt != null && dt.Rows.Count > Constants.ZERO)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PreshipmentSamplingPlanMasterDTO preshipmentplan = new PreshipmentSamplingPlanMasterDTO();

                    preshipmentplan.PreshipmentSamplingPlanMasterID = FloorDBAccess.GetValue<int>(dr, "PreshipmentSamplingPlanMasterID");
                    preshipmentplan.PreshipmentSamplingPlan = FloorDBAccess.GetString(dr, "PreshipmentSamplingPlan");
                    preshipmentplan.PreshipmentDescription = FloorDBAccess.GetString(dr, "PreshipmentDescription");
                    lstpreshipmentsampling.Add(preshipmentplan);
                }
            }
            return lstpreshipmentsampling;
        }
        public static DataTable GetPreshipmentSamplingPlanMasterData()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_GET_PreshipmentSamplingPlanMaster", null);
            return dtTableDetails;
        }

        public static void SavePreshipmentSamplingPlan(PreshipmentSamplingPlanMasterDTO lmld, PreshipmentSamplingPlanMasterDTO lmnew, string userid)
        {
            ChangeLogDTO PreshipmentSamplingPlanMasterLog = new ChangeLogDTO();
            PreshipmentSamplingPlanMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            PreshipmentSamplingPlanMasterLog.TableName = "PreshipmentSamplingPlanMaster";
            PreshipmentSamplingPlanMasterLog.UserId = userid;
            PreshipmentSamplingPlanMasterLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string PreshipmentSamplingPlanMasterRecord = CommonBLL.SerializeTOXML(lmnew);
            string tbllog = CommonBLL.SerializeTOXML(PreshipmentSamplingPlanMasterLog);

            //audit log
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = "Configuration SetUp - PreshipmentSamplingPlanMaster";
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), lmnew.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "PreshipmentSamplingPlanMaster";
            string refid = "";
            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {
                refid = lmnew.PreshipmentSamplingPlanMasterID.ToString();
                AuditLog.ReferenceId = refid;
            }

            AuditLog.UpdateColumns = lmld.DetailedCompare(lmnew).GetPropChanges();
            string audlog = CommonBLL.SerializeTOXML(AuditLog);
 
            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PreshipmentSamplingPlanMasterRecord", PreshipmentSamplingPlanMasterRecord));
            PrmList.Add(new FloorSqlParameter("@PreshipmentSamplingPlanMasterLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog));
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_PreshipmentSamplingPlanMaster", PrmList);


        }


        public static int IsPreshipmentSamplingPlanMasterDuplicate(string PreshipmentSamplingPlan)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PreshipmentSamplingPlan", PreshipmentSamplingPlan));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsPreshipmentSamplingPlanMasterDuplicate", PrmList));
        }
        #endregion



        /// <summary>
        /// Get Range based on list count
        /// </summary>
        /// <param name="startindex"></param>
        /// <param name="listcount"></param>
        /// <returns></returns>
        public static int GetRange(int startindex, int listcount)
        {
            int _indexrange = listcount - (startindex + Constants.TWENTY);
            if (_indexrange >= 0)
            {
                return Constants.TWENTY;
            }
            if (_indexrange < 0)
            {
                return listcount - startindex;
            }
            return 0;
        }
    }
    public class PageOffsetList : IListSource
    {
        public bool ContainsListCollection
        {
            get;
            protected set;
        }
        public int TotalRecords = 0;
        public int pageSize = 0;
        public PageOffsetList(int pageSize, int total)
        {
            this.TotalRecords = total;
            this.pageSize = pageSize;//FloorSystemConfiguration.GetInstance().RecordsPerPage;
        }
        public System.Collections.IList GetList()
        {
            // Return a list of page offsets based on "totalRecords" and "pageSize"   
            var pageOffsets = new List<int>();
            for (int offset = 0; offset < TotalRecords; offset = offset + pageSize)
            {
                pageOffsets.Add(offset);
            }
            return pageOffsets;
        }
    }
}
