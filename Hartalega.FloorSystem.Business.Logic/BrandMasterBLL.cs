using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using System.Data;

namespace Hartalega.FloorSystem.Business.Logic
{
    public class BrandMasterBLL : Framework.Business.BusinessBase
    {
        public static int SyncBrandMasterFromAX()
        {
            List<FloorSqlParameter> parameters = new List<FloorSqlParameter>();
            return FloorDBAccess.ExecuteNonQuery("USP_FS_AX_BrandMasterSyncBatchJob", parameters);
        }

        #region Brand Master Maintenance

        public static List<BrandMasterDTO> GetBrandMasterList(string txtFilterFGCode, string txtFilterBrandName, string txtFilterGloveType, Int32? cmbFilterStatus)
        {
            List<BrandMasterDTO> brandMasterDTO = null;

            DataTable dtBrandMasterDetails;

            dtBrandMasterDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_BrandMasterList", null);
            if (dtBrandMasterDetails != null && dtBrandMasterDetails.Rows.Count > 0)
            {
                brandMasterDTO = (from DataRow r in dtBrandMasterDetails.Rows
                                  select new BrandMasterDTO
                                  {
                                      ITEMID = FloorDBAccess.GetString(r, "ITEMID"),
                                      BRANDNAME = FloorDBAccess.GetString(r, "BRANDNAME"),
                                      GLOVECODE = FloorDBAccess.GetString(r, "GLOVECODE"),
                                      ACTIVE = FloorDBAccess.GetValue<int>(r, "ACTIVE"),
                                      PRESHIPMENTPLAN = FloorDBAccess.GetValue<int>(r, "PRESHIPMENTPLAN"),
                                      PALLETCAPACITY = FloorDBAccess.GetValue<int>(r, "PALLETCAPACITY")
                                  }).ToList();
            }

            if (!string.IsNullOrEmpty(txtFilterFGCode) && brandMasterDTO!=null)
                brandMasterDTO = brandMasterDTO.Where(p => p.ITEMID.ToLower().Contains(txtFilterFGCode.ToLower())).ToList();
            if (!string.IsNullOrEmpty(txtFilterBrandName) && brandMasterDTO != null)
                brandMasterDTO = brandMasterDTO.Where(p => p.BRANDNAME.ToLower().Contains(txtFilterBrandName.ToLower())).ToList();
            if (!string.IsNullOrEmpty(txtFilterGloveType) && brandMasterDTO != null)
                brandMasterDTO = brandMasterDTO.Where(p => p.GLOVECODE.ToLower().Contains(txtFilterGloveType.ToLower())).ToList();
            if (cmbFilterStatus.HasValue && brandMasterDTO != null)
                brandMasterDTO = brandMasterDTO.Where(p => p.ACTIVE == cmbFilterStatus).ToList();

            return brandMasterDTO;
        }

        public static BrandMasterDTO GetBrandMasterDetails(string itemID)
        {
            BrandMasterDTO brandMasterMaintenanceDTO = null;

            DataTable dtBrandMasterMaintenanceDetails;

            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ItemID", itemID));

            dtBrandMasterMaintenanceDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_BrandMasterDetails", PrmList);
            if (dtBrandMasterMaintenanceDetails != null && dtBrandMasterMaintenanceDetails.Rows.Count > 0)
            {
                brandMasterMaintenanceDTO = (from DataRow r in dtBrandMasterMaintenanceDetails.Rows
                                             select new BrandMasterDTO
                                             {
                                                 ITEMID = FloorDBAccess.GetString(r, "ITEMID"),
                                                 BRANDNAME = FloorDBAccess.GetString(r, "BRANDNAME"),
                                                 GLOVECODE = FloorDBAccess.GetString(r, "GLOVECODE"),
                                                 ACTIVE = FloorDBAccess.GetValue<int>(r, "ACTIVE"),
                                                 ALTERNATEGLOVECODE1 = FloorDBAccess.GetString(r, "ALTERNATEGLOVECODE1"),
                                                 ALTERNATEGLOVECODE2 = FloorDBAccess.GetString(r, "ALTERNATEGLOVECODE2"),
                                                 ALTERNATEGLOVECODE3 = FloorDBAccess.GetString(r, "ALTERNATEGLOVECODE3"),
                                                 EXPIRY = String.IsNullOrEmpty(FloorDBAccess.GetString(r, "EXPIRY")) ? (int?)null : FloorDBAccess.GetValue<int>(r, "EXPIRY"),
                                                 NOOFLABELPRINT = String.IsNullOrEmpty(FloorDBAccess.GetString(r, "NOOFLABELPRINT")) ? (int?)null : FloorDBAccess.GetValue<int>(r, "NOOFLABELPRINT"),
                                                 GCLABEL = String.IsNullOrEmpty(FloorDBAccess.GetString(r, "GCLABEL")) ? (int?)null : FloorDBAccess.GetValue<int>(r, "GCLABEL"),
                                                 INNERLABELSET = FloorDBAccess.GetString(r, "INNERLABELSET"),
                                                 INNERLABELSETDATEFORMAT = FloorDBAccess.GetString(r, "INNERLABELSETDATEFORMAT"),
                                                 LOTVERIFICATION = FloorDBAccess.GetValue<int>(r, "LOTVERIFICATION"),
                                                 MANUFACTURINGDATEON = String.IsNullOrEmpty(FloorDBAccess.GetString(r, "MANUFACTURINGDATEON")) ? (int?)null : FloorDBAccess.GetValue<int>(r, "MANUFACTURINGDATEON"),
                                                 OUTERLABELSETNO = FloorDBAccess.GetString(r, "OUTERLABELSETNO"),
                                                 OUTERLABELSETDATEFORMAT = FloorDBAccess.GetString(r, "OUTERLABELSETDATEFORMAT"),
                                                 INNERPRINTER = String.IsNullOrEmpty(FloorDBAccess.GetString(r, "INNERPRINTER")) ? (int?)null : FloorDBAccess.GetValue<int>(r, "INNERPRINTER"),
                                                 SPECIALINNERCODE = String.IsNullOrEmpty(FloorDBAccess.GetString(r, "SPECIALINNERCODE")) ? (int?)null : FloorDBAccess.GetValue<int>(r, "SPECIALINNERCODE"),
                                                 SPECIALINNERCHARACTER = FloorDBAccess.GetString(r, "SPECIALINNERCHARACTER"),
                                                 PALLETCAPACITY =  String.IsNullOrEmpty(FloorDBAccess.GetString(r, "PALLETCAPACITY")) ? (int?)null : FloorDBAccess.GetValue<int>(r, "PALLETCAPACITY"),
                                                 PRESHIPMENTPLAN = String.IsNullOrEmpty(FloorDBAccess.GetString(r, "PRESHIPMENTPLAN")) ? (int?)null : FloorDBAccess.GetValue<int>(r, "PRESHIPMENTPLAN"),

                                             }).FirstOrDefault();
            }

            return brandMasterMaintenanceDTO;
        }

        public static List<BrandLineDTO> GetBrandLineList(string itemID)
        {
            List<BrandLineDTO> brandLineListDTO = null;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ItemID", itemID));
            DataTable dtBrandLineList;

            dtBrandLineList = FloorDBAccess.ExecuteDataTable("USP_SEL_BrandLineList", PrmList);
            if (dtBrandLineList != null && dtBrandLineList.Rows.Count > 0)
            {
                brandLineListDTO = (from DataRow r in dtBrandLineList.Rows
                                    select new BrandLineDTO
                                    {
                                        BASEUNIT = FloorDBAccess.GetString(r, "BASEUNIT"),
                                        AVABRANDLINE_ID = FloorDBAccess.GetValue<int>(r, "AVABRANDLINE_ID"),
                                        COMPANYCATEGORYCODE = String.IsNullOrEmpty(FloorDBAccess.GetString(r, "COMPANYCATEGORYCODE")) ? null : FloorDBAccess.GetString(r, "COMPANYCATEGORYCODE"),
                                        CUSTOMERSIZE = FloorDBAccess.GetString(r, "CUSTOMERSIZE"),
                                        GLOVESINNERBOXNO = FloorDBAccess.GetValue<int>(r, "GLOVESINNERBOXNO"),
                                        GROSSWEIGHT = Convert.ToDouble(r["GROSSWEIGHT"]),
                                        HARTALEGACOMMONSIZE = FloorDBAccess.GetString(r, "HARTALEGACOMMONSIZE"),
                                        INNERBOXINCASENO = FloorDBAccess.GetValue<int>(r, "INNERBOXINCASENO"),
                                        INNERPRODUCTCODE = String.IsNullOrEmpty(FloorDBAccess.GetString(r, "INNERPRODUCTCODE")) ? null : FloorDBAccess.GetString(r, "INNERPRODUCTCODE"),
                                        ITEMID = FloorDBAccess.GetString(r, "ITEMID"),
                                        NETWEIGHT = Convert.ToDouble(r["NETWEIGHT"]),
                                        NUMOFBASEUNITPIECE = FloorDBAccess.GetValue<int>(r, "NUMOFBASEUNITPIECE"),
                                        NUMOFPACKERS = FloorDBAccess.GetValue<int>(r, "NUMOFPACKERS"),
                                        OUTERPRODUCTCODE = String.IsNullOrEmpty(FloorDBAccess.GetString(r, "OUTERPRODUCTCODE")) ? null : FloorDBAccess.GetString(r, "OUTERPRODUCTCODE"),
                                        PACKAGINGWEIGHT = Convert.ToDouble(r["PACKAGINGWEIGHT"]),
                                        PACKPCSPERHR = FloorDBAccess.GetValue<int>(r, "PACKPCSPERHR"),
                                        REFERENCE1 = String.IsNullOrEmpty(FloorDBAccess.GetString(r, "REFERENCE1")) ? null : FloorDBAccess.GetString(r, "REFERENCE1"),
                                        REFERENCE2 = String.IsNullOrEmpty(FloorDBAccess.GetString(r, "REFERENCE2")) ? null : FloorDBAccess.GetString(r, "REFERENCE2"),
                                        PRINTINGSIZE = FloorDBAccess.GetString(r, "PRINTINGSIZE")
                                    }).ToList();
            }

            return brandLineListDTO;
        }

        public static int isDuplicateHartalegaCommonSizeUpdate(string itemID, string size, int brandlineID)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ItemID", itemID));
            PrmList.Add(new FloorSqlParameter("@Size", size));
            PrmList.Add(new FloorSqlParameter("@BrandLineID", brandlineID));

            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsDuplicateHartalegaCommonSize", PrmList));
        }
        public static int isDuplicateHartalegaCommonSizeInsert(string itemID, string size)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ItemID", itemID));
            PrmList.Add(new FloorSqlParameter("@Size", size));

            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsDuplicateHartalegaCommonSizeInsert", PrmList));
        }


        public static void SaveBrandLineListChanges(BrandLineDTO olditem, BrandLineDTO newitem, string userid, string itemid, string screenName)
        {
            ChangeLogDTO BrandLineLog = new ChangeLogDTO();
            BrandLineLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            BrandLineLog.TableName = "AX_AVABRANDLINE";
            BrandLineLog.UserId = userid;
            BrandLineLog.UpdateColumns = olditem.DetailedCompare(newitem).GetPropChanges();


            string BrandLineRecord = CommonBLL.SerializeTOXML(newitem);
            string tbllog = CommonBLL.SerializeTOXML(BrandLineLog);
            //audit log
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = screenName;
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), newitem.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "AX_AVABRANDLINE";
            //to get referenceid for add ( add haven get the identity id yet)
            string refid = "";
            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {
                refid = newitem.AVABRANDLINE_ID.ToString();
                AuditLog.ReferenceId = refid;
            }
            AuditLog.UpdateColumns = olditem.DetailedCompare(newitem).GetPropChanges();
            
            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@BrandLineRecord", BrandLineRecord));
            PrmList.Add(new FloorSqlParameter("@BrandLineLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog));
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_BrandLine", PrmList);
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

        public static int EditBrandMaster(BrandMasterDTO olditem, BrandMasterDTO newitem, string userid, string screenName)
        {
            ChangeLogDTO BrandMasterLog = new ChangeLogDTO();
            BrandMasterLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            BrandMasterLog.TableName = "AX_AVABRANDHEADER";
            BrandMasterLog.UserId = userid;
            BrandMasterLog.UpdateColumns = olditem.DetailedCompare(newitem).GetPropChanges();


            string BrandMasterRecord = CommonBLL.SerializeTOXML(newitem);
            string tbllog = CommonBLL.SerializeTOXML(BrandMasterLog);
            //audit log
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = screenName;
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), newitem.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "AX_AVABRANDHEADER";
            //to get referenceid for add ( add haven get the identity id yet)
            string refid = "";
            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {
                refid = newitem.ITEMID.ToString();
                AuditLog.ReferenceId = refid;
            }
            AuditLog.UpdateColumns = olditem.DetailedCompare(newitem).GetPropChanges();
            
            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            int _rowsaffected = 0;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@BrandMasterRecord", BrandMasterRecord));
            PrmList.Add(new FloorSqlParameter("@BrandMasterLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog));
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_BrandMaster", PrmList);
            return _rowsaffected;
        }

        public static int GetManufacturingDateValue(string txtVal)
        {
            int ret = 0;
            List<DropdownDTO> dd = new List<DropdownDTO>();
            dd = GetEnumMaster("MANUFACTURINGDATEON");
            foreach (DropdownDTO d in dd)
            {
                if (d.DisplayField.Trim().Equals(txtVal))
                    ret = Convert.ToInt32(d.IDField);
            }

            return ret;
        }
        #endregion
    }
}
