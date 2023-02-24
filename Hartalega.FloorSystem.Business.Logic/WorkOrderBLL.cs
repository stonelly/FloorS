using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Business.Logic
{
    public class WorkOrderBLL : Framework.Business.BusinessBase
    {
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

        public static DateTime GetOldestDate(string type)
        {
            DateTime orderDate, etd;
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_SalesTableOldestRecord", null);
            if (dt != null && dt.Rows.Count != 0)
            {
                if (type == "OrderDate")
                {
                    orderDate = (from DataRow row in dt.Rows
                                 select FloorDBAccess.GetValue<DateTime>(row, "CreatedDate")).First();
                    return orderDate;
                }
                if(type == "ETD")
                {
                    etd = (from DataRow row in dt.Rows
                           select FloorDBAccess.GetValue<DateTime>(row, "ETD")).First();
                    return etd;
                }                
            }
            return DateTime.Now;
        }

        public static DataTable GetDTWorkOrderList(List<byte> workorderstatus, string salesid,
            string salesname, string customerpo, string CustomerRef, DateTime? createddatestart, DateTime? createddateend,
            DateTime? etdstart, DateTime? etdend)
        {
            DataTable dtWorkOrder;
            bool hasvalue;
            string expression = string.Empty;
            dtWorkOrder = FloorDBAccess.ExecuteDataTable("USP_SEL_WorkOrderList", null);

            DataTable temp;

            if (workorderstatus.Any() && dtWorkOrder.Rows.Count > 0)
            {
                for (int i = 0; i < workorderstatus.Count(); i++)
                {
                    hasvalue = dtWorkOrder.AsEnumerable()
                        .Where(row => row.Field<byte>("WorkOrderStatus") != workorderstatus[i])
                        .Any();
                    if (hasvalue)
                        dtWorkOrder = dtWorkOrder.AsEnumerable()
                        .Where(row => row.Field<byte>("WorkOrderStatus") != workorderstatus[i])
                        .CopyToDataTable();
                }                
            }
            if (!string.IsNullOrEmpty(salesid) && dtWorkOrder.Rows.Count > 0)
            {
                hasvalue = dtWorkOrder.AsEnumerable()
                    .Where(row => row.Field<string>("SalesId").ToLower().Contains(salesid.ToLower()))
                    .Any();
                if (hasvalue)
                    dtWorkOrder = dtWorkOrder.AsEnumerable()
                        .Where(row => row.Field<string>("SalesId").ToLower().Contains(salesid.ToLower()))
                        .CopyToDataTable();
                else return new DataTable();
            }
            if (!string.IsNullOrEmpty(salesname) && dtWorkOrder.Rows.Count > 0)
            {
                hasvalue = dtWorkOrder.AsEnumerable()
                    .Where(row => row.Field<string>("SalesName").ToLower().Contains(salesname.ToLower()))
                    .Any();
                if (hasvalue)
                    dtWorkOrder = dtWorkOrder.AsEnumerable()
                        .Where(row => row.Field<string>("SalesName").ToLower().Contains(salesname.ToLower()))
                        .CopyToDataTable();
                else return new DataTable();
            }
            if (!string.IsNullOrEmpty(customerpo) && dtWorkOrder.Rows.Count > 0)
            {
                hasvalue = dtWorkOrder.AsEnumerable()
                    .Where(row => row.Field<string>("CustomerPO").ToLower().Contains(customerpo.ToLower()))
                    .Any();
                if (hasvalue)
                    dtWorkOrder = dtWorkOrder.AsEnumerable()
                        .Where(row => row.Field<string>("CustomerPO").ToLower().Contains(customerpo.ToLower()))
                        .CopyToDataTable();
                else return new DataTable();
            }
            if (!string.IsNullOrEmpty(CustomerRef) && dtWorkOrder.Rows.Count > 0)
            {
                hasvalue = dtWorkOrder.AsEnumerable()
                    .Where(row => row.Field<string>("CustomerRef").ToLower().Contains(CustomerRef.ToLower()))
                    .Any();
                if (hasvalue)
                    dtWorkOrder = dtWorkOrder.AsEnumerable()
                        .Where(row => row.Field<string>("CustomerRef").ToLower().Contains(CustomerRef.ToLower()))
                        .CopyToDataTable();
                else return new DataTable();
            }
            if (createddatestart != null && createddateend != null && dtWorkOrder.Rows.Count > 0)
            {
                hasvalue = dtWorkOrder.AsEnumerable()
                    .Where(row => row.Field<DateTime>("CreatedDate") >= createddatestart
                    && row.Field<DateTime>("CreatedDate") <= createddateend)
                    .Any();
                if (hasvalue)
                    dtWorkOrder = dtWorkOrder.AsEnumerable()
                        .Where(row => row.Field<DateTime>("CreatedDate") >= createddatestart
                        && row.Field<DateTime>("CreatedDate") <= createddateend)
                        .CopyToDataTable();
                else return new DataTable();
            }
            else if (createddatestart != null && createddateend == null && dtWorkOrder.Rows.Count > 0)
            {
                hasvalue = dtWorkOrder.AsEnumerable()
                    .Where(row => row.Field<DateTime>("CreatedDate") >= createddatestart)
                    .Any();
                if (hasvalue)
                    dtWorkOrder = dtWorkOrder.AsEnumerable()
                        .Where(row => row.Field<DateTime>("CreatedDate") >= createddatestart)
                        .CopyToDataTable();
                else return new DataTable();
            }
            else if (createddatestart == null && createddateend != null && dtWorkOrder.Rows.Count > 0)
            {
                hasvalue = dtWorkOrder.AsEnumerable()
                   .Where(row =>row.Field<DateTime>("CreatedDate") <= createddateend)
                   .Any();
                if (hasvalue)
                    dtWorkOrder = dtWorkOrder.AsEnumerable()
                        .Where(row => row.Field<DateTime>("CreatedDate") <= createddateend)
                        .CopyToDataTable();
                else return new DataTable();
            }

                if (etdstart != null && etdend != null && dtWorkOrder.Rows.Count > 0)
            {
                hasvalue = dtWorkOrder.AsEnumerable()
                    .Where(row => row.Field<DateTime>("ETD") >= etdstart
                    && row.Field<DateTime>("ETD") <= etdend)
                    .Any();
                if (hasvalue)
                    dtWorkOrder = dtWorkOrder.AsEnumerable()
                        .Where(row => row.Field<DateTime>("ETD") >= etdstart
                        && row.Field<DateTime>("ETD") <= etdend)
                        .CopyToDataTable();
                else return new DataTable();
            }
            else if (etdstart != null && etdend == null && dtWorkOrder.Rows.Count > 0)
            {
                hasvalue = dtWorkOrder.AsEnumerable()
                    .Where(row => row.Field<DateTime>("ETD") >= etdstart)
                    .Any();
                if (hasvalue)
                    dtWorkOrder = dtWorkOrder.AsEnumerable()
                        .Where(row => row.Field<DateTime>("ETD") >= etdstart)
                        .CopyToDataTable();
                else return new DataTable();
            }
            else if (etdstart == null && etdend != null && dtWorkOrder.Rows.Count > 0)
            {
                hasvalue = dtWorkOrder.AsEnumerable()
                   .Where(row => row.Field<DateTime>("ETD") <= etdend)
                   .Any();
                if (hasvalue)
                    dtWorkOrder = dtWorkOrder.AsEnumerable()
                        .Where(row => row.Field<DateTime>("ETD") <= etdend)
                        .CopyToDataTable();
                else return new DataTable();
            }


            DataView dv = dtWorkOrder.DefaultView;
            dv.Sort = "CreatedDate desc";
             DataTable sortedDT = dv.ToTable();
            return sortedDT;
        }

        public static List<WorkOrderDTO> WorkOrderListFromDT(DataTable dtWorkOrder)
        {
            List<WorkOrderDTO> workOrderDTO = null;

            if (dtWorkOrder != null && dtWorkOrder.Rows.Count > 0)
            {
                workOrderDTO = (from DataRow r in dtWorkOrder.Rows
                                select new WorkOrderDTO
                                {
                                    ContainerSize = FloorDBAccess.GetString(r, "ContainerSize"),
                                    CustomerLot = FloorDBAccess.GetString(r, "CustomerLot"),
                                    CustomerPO = FloorDBAccess.GetString(r, "CustomerPO"),
                                    CustomerRef = FloorDBAccess.GetString(r, "CustomerRef"),
                                    DocumentStatus = FloorDBAccess.GetValue<byte>(r, "DocumentStatus"),
                                    ETA = FloorDBAccess.GetValue<DateTime>(r, "ETA"),
                                    ETD = FloorDBAccess.GetValue<DateTime>(r, "ETD"),
                                    LastConfirmDate = r.Field<DateTime?>("LastConfirmDate"),// FloorDBAccess.GetValue<DateTime?>(r, "LastConfirmDate"),
                                    ManufacturingDate = FloorDBAccess.GetValue<DateTime>(r, "ManufacturingDate"),
                                    SalesId = FloorDBAccess.GetString(r, "SalesId"),
                                    SalesName = FloorDBAccess.GetString(r, "SalesName"),
                                    SalesStatus = FloorDBAccess.GetValue<byte>(r, "SalesStatus"),
                                    ShippingAgent = FloorDBAccess.GetString(r, "ShippingAgent"),
                                    SpecialInstruction = FloorDBAccess.GetString(r, "SpecialInstruction"),
                                    VesselName = FloorDBAccess.GetString(r, "VesselName"),
                                    WorkOrderStatus = FloorDBAccess.GetValue<byte>(r, "WorkOrderStatus"),
                                    DeliveryCountryRegionId = FloorDBAccess.GetString(r, "DeliveryCountryRegionId"),
                                    CreatedDate = FloorDBAccess.GetValue<DateTime>(r, "CreatedDate"),
                                    WorkOrderType = FloorDBAccess.GetString(r, "WorkOrderText")
                                }).ToList();
            }

            return workOrderDTO;
        }

        public static List<WorkOrderDTO> GetWorkOrderList()
        {
            List<WorkOrderDTO> workOrderDTO = null;

            DataTable dtWorkOrder;

            dtWorkOrder = FloorDBAccess.ExecuteDataTable("USP_SEL_WorkOrderList", null);

            if (dtWorkOrder != null && dtWorkOrder.Rows.Count > 0)
            {
                workOrderDTO = (from DataRow r in dtWorkOrder.Rows
                                select new WorkOrderDTO
                                {
                                    ContainerSize = FloorDBAccess.GetString(r, "ContainerSize"),
                                    CustomerLot = FloorDBAccess.GetString(r, "CustomerLot"),
                                    CustomerPO = FloorDBAccess.GetString(r, "CustomerPO"),
                                    CustomerRef = FloorDBAccess.GetString(r, "CustomerRef"),
                                    DocumentStatus = FloorDBAccess.GetValue<byte>(r, "DocumentStatus"),
                                    ETA = FloorDBAccess.GetValue<DateTime>(r, "ETA"),
                                    ETD = FloorDBAccess.GetValue<DateTime>(r, "ETD"),
                                    LastConfirmDate = r.Field<DateTime?>("LastConfirmDate"),// FloorDBAccess.GetValue<DateTime?>(r, "LastConfirmDate"),
                                    ManufacturingDate = FloorDBAccess.GetValue<DateTime>(r, "ManufacturingDate"),
                                    SalesId = FloorDBAccess.GetString(r, "SalesId"),
                                    SalesName = FloorDBAccess.GetString(r, "SalesName"),
                                    SalesStatus = FloorDBAccess.GetValue<byte>(r, "SalesStatus"),
                                    ShippingAgent = FloorDBAccess.GetString(r, "ShippingAgent"),
                                    SpecialInstruction = FloorDBAccess.GetString(r, "SpecialInstruction"),
                                    VesselName = FloorDBAccess.GetString(r, "VesselName"),
                                    WorkOrderStatus = FloorDBAccess.GetValue<byte>(r, "WorkOrderStatus"),
                                    CreatedDate = FloorDBAccess.GetValue<DateTime>(r, "CreatedDate")
                                }).ToList();
            }

            return workOrderDTO;
        }

        public static List<WorkOrderSalesLineDTO> GetWorkOrderSalesLineList(string salesId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SalesId", salesId));

            List<WorkOrderSalesLineDTO> workOrderSalesLineDTO = null;

            DataTable dtWorkOrderSalesLine;

            dtWorkOrderSalesLine = FloorDBAccess.ExecuteDataTable("USP_SEL_WorkOrderDetails", PrmList);

            if (dtWorkOrderSalesLine != null && dtWorkOrderSalesLine.Rows.Count > 0)
            {
                workOrderSalesLineDTO = (from DataRow r in dtWorkOrderSalesLine.Rows
                                         select new WorkOrderSalesLineDTO
                                         {
                                             CustomerLot = FloorDBAccess.GetString(r, "CustomerLot"),
                                             HartalegaCommonSize = FloorDBAccess.GetString(r, "HartalegaCommonSize"),
                                             InventTransId = FloorDBAccess.GetString(r, "InventTransId"),
                                             ItemId = FloorDBAccess.GetString(r, "ItemId"),
                                             FS_ItemId = FloorDBAccess.GetString(r, "FS_ItemId"),
                                             LineNum = FloorDBAccess.GetValue<int>(r, "LineNum"),
                                             Name = FloorDBAccess.GetString(r, "Name"),
                                             ReceiptDateRequested = FloorDBAccess.GetValue<DateTime>(r, "ReceiptDateRequested"),
                                             SalesId = FloorDBAccess.GetString(r, "SalesId"),
                                             SalesQty = FloorDBAccess.GetValue<int>(r, "SalesQty"),
                                             ShippingDateConfirmed = FloorDBAccess.GetValue<DateTime>(r, "ShippingDateConfirmed"),
                                             GLOVECODE = FloorDBAccess.GetString(r, "GLOVECODE"),
                                             GLOVESINNERBOXNO = FloorDBAccess.GetValue<int>(r, "GLOVESINNERBOXNO"),
                                             INNERBOXINCASENO = FloorDBAccess.GetValue<int>(r, "INNERBOXINCASENO"),
                                             CUSTOMERSIZE = FloorDBAccess.GetString(r, "CUSTOMERSIZE"),
                                             HARTALEGACOMMONSIZE = FloorDBAccess.GetString(r, "HARTALEGACOMMONSIZE"),
                                             ItemCaseCount = FloorDBAccess.GetValue<int>(r, "ItemCaseCount")
                                         }).ToList();
            }

            return workOrderSalesLineDTO;
        }

        public static int UpdateWorkOrder(WorkOrderDTO olditem, WorkOrderDTO newitem, string userid, string screenName)
        {
            ChangeLogDTO WorkOrderLog = new ChangeLogDTO();
            WorkOrderLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            WorkOrderLog.TableName = "SalesTable";
            WorkOrderLog.UserId = userid;
            WorkOrderLog.UpdateColumns = olditem.DetailedCompare(newitem).GetPropChanges();


            string WorkOrderRecord = CommonBLL.SerializeTOXML(newitem);
            string tbllog = CommonBLL.SerializeTOXML(WorkOrderLog);
            //audit log
            AuditLogDTO AuditLog = new AuditLogDTO();
            AuditLog.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            AuditLog.FunctionName = screenName;
            AuditLog.CreatedBy = userid;
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), newitem.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "SalesTable";
            //to get referenceid for add ( add haven get the identity id yet)
            string refid = "";
            if (audAction != Constants.ActionLog.Add) //for delete update get reference id else set id in store procedure
            {
                refid = newitem.SalesId.ToString();
                AuditLog.ReferenceId = refid;
            }
            AuditLog.UpdateColumns = olditem.DetailedCompare(newitem).GetPropChanges();
            
            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            int _rowsaffected = 0;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SalesTableRecord", WorkOrderRecord));
            PrmList.Add(new FloorSqlParameter("@SalesTableLog", tbllog));
            PrmList.Add(new FloorSqlParameter("@AuditLog", audlog));
            PrmList.Add(new FloorSqlParameter("@UserId", userid));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_SalesTable", PrmList);
            return _rowsaffected;
        }
         
        public static bool ValidateWorkOrderItem(string itemId)
        { 
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ItemId", itemId));
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_IsWorkItemValid", PrmList);

            if (dt.Rows[0][0].ToString() != "0")
                return true;
            else
                return false;
        }


        public static int UpdateSalesLinesWorkOrder(WorkOrderSalesLineDTO olditem, WorkOrderSalesLineDTO newitem, string userid, string screenName)
        {
            ChangeLogDTO WorkOrderLog = new ChangeLogDTO()
            {
                WorkstationId = WorkStationDTO.GetInstance().WorkStationId,
                TableName = "SalesLineWorkOrder",
                UserId = userid,
                UpdateColumns = olditem.DetailedCompare(newitem).GetPropChanges()
            };

            string WorkOrderRecord = CommonBLL.SerializeTOXML(newitem);
            string tbllog = CommonBLL.SerializeTOXML(WorkOrderLog);

            AuditLogDTO AuditLog = new AuditLogDTO()
            {
                WorkstationId = WorkStationDTO.GetInstance().WorkStationId,
                FunctionName = screenName,
                CreatedBy = userid
            };
            Constants.ActionLog audAction = (Constants.ActionLog)System.Enum.Parse(typeof(Constants.ActionLog), newitem.ActionType.ToString());
            AuditLog.AuditAction = Convert.ToInt32(audAction);
            AuditLog.SourceTable = "SalesLineWorkOrder";

            string refid = "";
            if (audAction != Constants.ActionLog.Add)
            {
                refid = newitem.SalesId.ToString();
                AuditLog.ReferenceId = refid;
            }

            if (audAction == Constants.ActionLog.Delete)
            {
                AuditLog.UpdateColumns = newitem.DetailedCompare(olditem).GetPropChanges();
            }
            else
            {
                AuditLog.UpdateColumns = olditem.DetailedCompare(newitem).GetPropChanges();

            }

            string audlog = CommonBLL.SerializeTOXML(AuditLog);

            int _rowsaffected = 0;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>
            {
                new FloorSqlParameter("@SalesTableRecord", WorkOrderRecord),
                new FloorSqlParameter("@SalesTableLog", tbllog),
                new FloorSqlParameter("@AuditLog", audlog),
                new FloorSqlParameter("@UserId", userid)
            };

            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Update_SalesLineWorkOrder", PrmList);
            return _rowsaffected;
        }


        //KahHeng 30Jan2019 method to get the PODate and POReceivedDate
        public static List<WorkOrderDTO> GetPODateAndPOReceivedDate(string PONumber)
        {
            List<WorkOrderDTO> _workorder = new List<WorkOrderDTO>();
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@PONumber", PONumber));

            using (DataTable dtPOlist = FloorDBAccess.ExecuteDataTable("usp_FP_PODateAndPORcvdDate_Get", lstfsp))
            {
                try
                {
                    if (dtPOlist != null && dtPOlist.Rows.Count != 0)
                    {
                        foreach (DataRow row in dtPOlist.Rows)
                        {
                            WorkOrderDTO objSo = new WorkOrderDTO();
                            //KahHeng 29jan2019 added 2 new parameter for PODate  POReceviedDate 
                            objSo.HSB_CustPODocumentDate = FloorDBAccess.GetValue<DateTime>(row, "HSB_CustPODocumentDate");
                            objSo.HSB_CustPORecvDate = FloorDBAccess.GetValue<DateTime>(row, "HSB_CustPORecvDate");
                            //KahHeng End
                            _workorder.Add(objSo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return _workorder;
            }
        }
        //KahHeng
    }
}
