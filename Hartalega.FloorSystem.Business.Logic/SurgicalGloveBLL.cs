// -----------------------------------------------------------------------
// <copyright file="SurgicalGloveBLL.cs" company="9Dots">
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
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// Surgical batch card business logic class
    /// </summary>
    public static class SurgicalGloveBLL
    {
        #region constructors
        /*
        /// <summary>
        /// Initializes a new instance of the <see cref="SurgicalGloveBLL" /> class.
        /// </summary>
        /// 
        public SurgicalGloveBLL()
        {
            // No implementation required
        }
        */
        #endregion

        #region Batch Order, Resources & Reprint SRBC

        /// <summary>
        /// #Azrul 22/10/2020: BO List for Surgical-SGR
        /// </summary>
        /// <param name="prodPoolID"></param>
        /// <returns>List<BatchOrderDetailsDTO></returns>
        public static List<GloveBatchOrderDTO> GetBatchOrderDetails(string prodPoolID)
        {
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            List<GloveBatchOrderDTO> lstBatchOrderDetailsDTO = new List<GloveBatchOrderDTO>();
            prmList.Add(new FloorSqlParameter("@prodPoolID", prodPoolID));
            prmList.Add(new FloorSqlParameter("@PlantNo", WorkStationDTO.GetInstance().Location.ToString()));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_BatchOrderDetails_Get", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            GloveBatchOrderDTO lnbodDTO = new GloveBatchOrderDTO();
                            DateTime dtSchedStart = Convert.ToDateTime(FloorDBAccess.GetString(dr, "SchedStart"));
                            DateTime dtSchedFromTime = Convert.ToDateTime(FloorDBAccess.GetString(dr, "SchedFromTime"));
                            DateTime dtSchedEnd = Convert.ToDateTime(FloorDBAccess.GetString(dr, "SchedEnd"));
                            DateTime dtSchedToTime = Convert.ToDateTime(FloorDBAccess.GetString(dr, "SchedToTime"));
                            lnbodDTO.SchedStart = dtSchedStart.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                            lnbodDTO.SchedFromTime = dtSchedFromTime.ToString("H:mm", CultureInfo.InvariantCulture);
                            lnbodDTO.SchedEnd = dtSchedEnd.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                            lnbodDTO.SchedToTime = dtSchedToTime.ToString("H:mm", CultureInfo.InvariantCulture);
                            lnbodDTO.ProdStatus = FloorDBAccess.GetString(dr, "ProdStatus");
                            lnbodDTO.SalesOrder = FloorDBAccess.GetString(dr, "SalesOrder");
                            lnbodDTO.BthOrderId = FloorDBAccess.GetString(dr, "BthOrderId");
                            lnbodDTO.ItemId = FloorDBAccess.GetString(dr, "ItemId");
                            lnbodDTO.Size = FloorDBAccess.GetString(dr, "Size");
                            lnbodDTO.QtySched = FloorDBAccess.GetValue<decimal>(dr, "QtySched").ToString("#,##0");
                            lnbodDTO.ReportedQty = FloorDBAccess.GetValue<Int32>(dr, "ReportedQty").ToString("#,##0");
                            lnbodDTO.RemainingQty = FloorDBAccess.GetValue<decimal>(dr, "RemainingQty").ToString("#,##0");
                            lnbodDTO.Resource = FloorDBAccess.GetString(dr, "Resource");
                            lnbodDTO.ResourceGrp = FloorDBAccess.GetString(dr, "ResourceGrp");
                            lstBatchOrderDetailsDTO.Add(lnbodDTO);
                        }
                    }
                }
                catch (ArgumentException argex)
                {
                    throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETLINESELECTIONDETAILSEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return lstBatchOrderDetailsDTO;
        }

        /// <summary>
        /// #Azrul 22/10/2020: BO List for Surgical-FG
        /// </summary>
        /// <param name="prodPoolID"></param>
        /// <param name="FGBatchOrder"></param>
        /// <param name="size"></param>
        /// <returns>List<BatchOrderDetailsDTO></returns>
        public static List<GloveBatchOrderDTO> GetBatchOrderDetails(string prodPoolID, string FGBatchOrder, string size)
        {
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            List<GloveBatchOrderDTO> lstBatchOrderDetailsDTO = new List<GloveBatchOrderDTO>();
            prmList.Add(new FloorSqlParameter("@prodPoolID", prodPoolID));
            prmList.Add(new FloorSqlParameter("@PlantNo", WorkStationDTO.GetInstance().Location.ToString()));
            prmList.Add(new FloorSqlParameter("@BatchOrderId", FGBatchOrder));
            prmList.Add(new FloorSqlParameter("@Size", size));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_BatchOrderDetails_Get", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            GloveBatchOrderDTO lnbodDTO = new GloveBatchOrderDTO();
                            DateTime dtSchedStart = Convert.ToDateTime(FloorDBAccess.GetString(dr, "SchedStart"));
                            DateTime dtSchedFromTime = Convert.ToDateTime(FloorDBAccess.GetString(dr, "SchedFromTime"));
                            DateTime dtSchedEnd = Convert.ToDateTime(FloorDBAccess.GetString(dr, "SchedEnd"));
                            DateTime dtSchedToTime = Convert.ToDateTime(FloorDBAccess.GetString(dr, "SchedToTime"));
                            lnbodDTO.SchedStart = dtSchedStart.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                            lnbodDTO.SchedFromTime = dtSchedFromTime.ToString("H:mm", CultureInfo.InvariantCulture);
                            lnbodDTO.SchedEnd = dtSchedEnd.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                            lnbodDTO.SchedToTime = dtSchedToTime.ToString("H:mm", CultureInfo.InvariantCulture);
                            lnbodDTO.ProdStatus = FloorDBAccess.GetString(dr, "ProdStatus");
                            lnbodDTO.SalesOrder = FloorDBAccess.GetString(dr, "SalesOrder");
                            lnbodDTO.BthOrderId = FloorDBAccess.GetString(dr, "BthOrderId");
                            lnbodDTO.ItemId = FloorDBAccess.GetString(dr, "ItemId");
                            lnbodDTO.Size = FloorDBAccess.GetString(dr, "Size");
                            lnbodDTO.QtySched = FloorDBAccess.GetValue<decimal>(dr, "QtySched").ToString("#,##0");
                            lnbodDTO.ReportedQty = FloorDBAccess.GetValue<Int32>(dr, "ReportedQty").ToString("#,##0");
                            lnbodDTO.RemainingQty = FloorDBAccess.GetValue<decimal>(dr, "RemainingQty").ToString("#,##0");
                            lnbodDTO.Resource = FloorDBAccess.GetString(dr, "Resource");
                            lnbodDTO.ResourceGrp = FloorDBAccess.GetString(dr, "ResourceGrp");
                            lstBatchOrderDetailsDTO.Add(lnbodDTO);
                        }
                    }
                }
                catch (ArgumentException argex)
                {
                    throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETLINESELECTIONDETAILSEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return lstBatchOrderDetailsDTO;
        }

        /// <summary>
        /// #Azrul 22/10/2020: Printed SRBC from BO List
        /// </summary>
        /// <param name="BatchOrderNo"></param>
        /// <returns>List<BatchOrderDetailsDTO></returns>
        public static List<BatchOrderDetailsDTO> GetBatchOrderPrintDetails(string BatchOrderNo)
        {
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            List<BatchOrderDetailsDTO> lstBatchOrderPrintDetailsDTO = new List<BatchOrderDetailsDTO>();
            prmList.Add(new FloorSqlParameter("@BatchOrderNo", BatchOrderNo));
            prmList.Add(new FloorSqlParameter("@PlantNo", WorkStationDTO.GetInstance().Location.ToString()));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_BatchOrderPrintDetails_Get", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            BatchOrderDetailsDTO lnbodDTO = new BatchOrderDetailsDTO();
                            DateTime dtOutputTime = Convert.ToDateTime(FloorDBAccess.GetString(dr, "OutputTime"));
                            lnbodDTO.SerialNumber = FloorDBAccess.GetString(dr, "SerialNumber");
                            lnbodDTO.BatchNumber = FloorDBAccess.GetString(dr, "BatchNumber");
                            lnbodDTO.Resource = FloorDBAccess.GetString(dr, "Resource");
                            lnbodDTO.ReportedQty = FloorDBAccess.GetValue<Int32>(dr, "TotalQty");
                            lnbodDTO.sOutputDate = dtOutputTime.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                            lnbodDTO.Reprint = FloorDBAccess.GetString(dr, "ReprintHBC");
                            lstBatchOrderPrintDetailsDTO.Add(lnbodDTO);
                        }
                    }
                }
                catch (ArgumentException argex)
                {
                    throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETLINESELECTIONDETAILSEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return lstBatchOrderPrintDetailsDTO;
        }

        public static List<BatchOrderDetailsDTO> GetRePrintHBCSerialNo(DateTime outputTime, string resource)
        {
            List<BatchOrderDetailsDTO> SNDTO = null;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@outputTime", outputTime));
            prmList.Add(new FloorSqlParameter("@resource", resource));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_HBCSerialNo_Get", prmList))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                if (dt != null && dt.Rows.Count != 0)
                {
                    SNDTO = (from DataRow dr in dt.Rows
                             select new BatchOrderDetailsDTO
                             {
                                 SerialNumber = FloorDBAccess.GetString(dr, "SerialNo")
                             }).ToList();
                }
            }
            return SNDTO;
        }

        public static List<DropdownDTO> GetRePrintSRBCResource(DateTime outputTime, int workstationId, string line, string resource, string bo)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@outputTime", outputTime));
            PrmList.Add(new FloorSqlParameter("@workstationId", workstationId));
            PrmList.Add(new FloorSqlParameter("@line", line));
            PrmList.Add(new FloorSqlParameter("@resource", resource));
            PrmList.Add(new FloorSqlParameter("@bo", bo));
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_ReprintSRBCResource_Get", PrmList))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "ddlVal"),
                                DisplayField = FloorDBAccess.GetString(row, "ddlVal"),
                            }).ToList();
                }
            }
            return list;
        }
        #endregion

        #region SRBC Printing

        /// <summary>
        /// #Azrul 19/10/20204: Get Shift for Print SRBC.
        /// </summary>
        /// <returns>List<DropdownDTO></returns>
        public static List<DropdownDTO> GetShift(string area)
        {
            List<DropdownDTO> ShiftForPrintBO = null;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@Area", area));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_BatchOrderShift_Get", prmList))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                if (dt != null && dt.Rows.Count != 0)
                {
                    ShiftForPrintBO = (from DataRow dr in dt.Rows
                                       select new DropdownDTO
                                       {
                                           IDField = FloorDBAccess.GetString(dr, "ShiftId"),
                                           DisplayField = FloorDBAccess.GetString(dr, "Name")
                                       }).ToList();
                }
            }
            return ShiftForPrintBO;
        }

        /// <summary>
        /// #Azrul 19/10/2020: Get Line, Resources and Batch Order details for Print SRBC.
        /// </summary>
        /// <returns>List<DropdownDTO></returns>
        public static List<BatchOrderDetailsDTO> GetSurgicalResource(int locationId, string lineId, string res, string bo)
        {
            List<BatchOrderDetailsDTO> SurgicalResource = null;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@LocationId", locationId));
            prmList.Add(new FloorSqlParameter("@LineId", lineId));
            prmList.Add(new FloorSqlParameter("@Resource", res));
            prmList.Add(new FloorSqlParameter("@BO", bo));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_SurgicalResource_Get", prmList))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                if (dt != null && dt.Rows.Count != 0)
                {
                    SurgicalResource = (from DataRow dr in dt.Rows
                                        select new BatchOrderDetailsDTO
                                        {
                                            Resource = FloorDBAccess.GetString(dr, "Resource"),
                                            ResourceId = FloorDBAccess.GetString(dr, "ResourceId"),
                                            LineId = FloorDBAccess.GetString(dr, "LineId"),
                                            Line = FloorDBAccess.GetString(dr, "Line"),
                                            BthOrderId = FloorDBAccess.GetString(dr, "BatchOrder"),
                                            ItemId = FloorDBAccess.GetString(dr, "GloveCode"),
                                            Size = FloorDBAccess.GetString(dr, "Size")
                                        }).ToList();
                }
            }
            return SurgicalResource;
        }

        /// <summary>
        /// Get Reason for Reprint List
        /// </summary>
        /// <returns>List<DropdownDTO></returns>
        public static List<DropdownDTO> GetReasonForReprint()
        {
            List<DropdownDTO> reasonForReprint = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_Reasons_Get", null))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        reasonForReprint = (from DataRow dr in dt.Rows
                                            select new DropdownDTO
                                            {
                                                IDField = FloorDBAccess.GetString(dr, "ReasonId"),
                                                DisplayField = FloorDBAccess.GetString(dr, "ReasonText")
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
            return reasonForReprint;
        }

        /// <summary>
        /// #Azrul 19/10/2020: Get Batch Details for Print SRBC
        /// </summary>
        /// <returns>List<BatchDTO></returns>
        public static List<BatchOrderDetailsDTO> GetPrintBatchDetails(string userId, int shiftId, string line, DateTime batchCardDate, string moduleId, string subModuleId, int sitenumber, int WorkStationNumber, string resource, string batchOrder, string gloveCode, string size, decimal batchWeight, int quantity, string authorizedBy, int authorizedFor)
        {
            List<BatchOrderDetailsDTO> listBatchDTO = new List<BatchOrderDetailsDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@UserId", userId));
            prmList.Add(new FloorSqlParameter("@ShiftId", shiftId));
            prmList.Add(new FloorSqlParameter("@Line", line));
            prmList.Add(new FloorSqlParameter("@BatchCardDate", batchCardDate));
            prmList.Add(new FloorSqlParameter("@ModuleId", moduleId));
            prmList.Add(new FloorSqlParameter("@SubModuleID", subModuleId));
            prmList.Add(new FloorSqlParameter("@SiteNumber", sitenumber));
            prmList.Add(new FloorSqlParameter("@WorkStationNumber", WorkStationNumber));
            prmList.Add(new FloorSqlParameter("@Resource", resource));
            prmList.Add(new FloorSqlParameter("@BatchOrder", batchOrder));
            prmList.Add(new FloorSqlParameter("@GloveCode", gloveCode));
            prmList.Add(new FloorSqlParameter("@Size", size));
            prmList.Add(new FloorSqlParameter("@BatchWeight", batchWeight));
            prmList.Add(new FloorSqlParameter("@Quantity", quantity));
            prmList.Add(new FloorSqlParameter("@authorizedBy", authorizedBy));
            prmList.Add(new FloorSqlParameter("@authorizedFor", authorizedFor));

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("Usp_DOT_SRBC_Print_Save", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            BatchOrderDetailsDTO batch = new BatchOrderDetailsDTO();
                            batch.SerialNumber = FloorDBAccess.GetString(dr, "SerialNumber");
                            batch.BatchNumber = FloorDBAccess.GetString(dr, "BatchNumber");
                            batch.GloveType = FloorDBAccess.GetString(dr, "GloveCode");
                            batch.GloveCategory = FloorDBAccess.GetString(dr, "GloveCategory");
                            batch.Size = FloorDBAccess.GetString(dr, "Size");
                            batch.BatchCarddate = FloorDBAccess.GetValue<DateTime>(dr, "BatchCardDate");
                            batch.Resource = FloorDBAccess.GetString(dr, "Resource");
                            batch.BatchWeight = FloorDBAccess.GetValue<decimal>(dr, "BatchWeight");
                            batch.Quantity = FloorDBAccess.GetValue<int>(dr, "Quantity");
                            listBatchDTO.Add(batch);
                        }
                    }
                }
                catch (ArgumentException argex)
                {
                    throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.MANUALPRINTBATCHDETAILSGETEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return listBatchDTO;
        }

        /// <summary>
        /// HBC Reprint and Save.
        /// </summary>
        /// <param name="serialNumbers"></param>
        /// <param name="reprintDateTime"></param>
        /// <param name="operatorId"></param>
        /// <param name="reasonId"></param>
        /// <param name="workStationName"></param>
        /// <returns></returns>
        public static List<BatchOrderDetailsDTO> SaveReprintBatchCard(string serialNumber, DateTime reprintDateTime, string operatorId, int reasonId, string workStationName)
        {
            List<BatchOrderDetailsDTO> listBatchDTO = new List<BatchOrderDetailsDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            prmList.Add(new FloorSqlParameter("@ReprintDateTime", reprintDateTime));
            prmList.Add(new FloorSqlParameter("@OperatorId", operatorId));
            prmList.Add(new FloorSqlParameter("@ReasonId", reasonId));
            prmList.Add(new FloorSqlParameter("@WorkStationName", workStationName));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_SRBC_ReprintBatchCard_Save", prmList))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BatchOrderDetailsDTO batch = new BatchOrderDetailsDTO();
                        batch.SerialNumber = FloorDBAccess.GetString(dr, "SerialNumber");
                        batch.BatchNumber = FloorDBAccess.GetString(dr, "BatchNumber");
                        batch.GloveType = FloorDBAccess.GetString(dr, "GloveType");
                        batch.GloveCategory = FloorDBAccess.GetString(dr, "GloveCategory");
                        batch.Size = FloorDBAccess.GetString(dr, "Size");
                        batch.BatchCarddate = FloorDBAccess.GetValue<DateTime>(dr, "BatchCardDate");
                        batch.Resource = FloorDBAccess.GetString(dr, "Resource");
                        batch.BatchWeight = FloorDBAccess.GetValue<decimal>(dr, "BatchWeight");
                        batch.Quantity = FloorDBAccess.GetValue<int>(dr, "TotalQty");
                        listBatchDTO.Add(batch);
                    }
                }
            }
            return listBatchDTO;
        }

        /// <summary>
        /// GetBatchList
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="barcodeText"></param>
        /// <param name="batchNumber"></param>
        /// <param name="batchWeight"></param>
        /// <param name="size"></param>
        /// <param name="tenPcsWeight"></param>
        /// <param name="tenPcsMsg"></param>
        /// <param name="gloveDesc"></param>
        /// <param name="side"></param>
        /// <param name="template"></param>
        /// <param name="isReprint"></param>
        /// <returns></returns>
        public static PrintDTO GetBatchList(string batchCardDate, string batchNumber, string batchWeight, string totalGloveQty, string barcodeText, string size, string gloveDesc, string gloveCategory, string resource, string template, bool isReprint = false)
        {
            PrintDTO printData = new PrintDTO();
            printData.DateTime = batchCardDate;
            printData.BatchNumber = batchNumber;
            printData.BatchWeight = batchWeight;
            printData.TotalGloveQty = totalGloveQty;
            printData.SerialNumber = barcodeText;
            printData.Size = size;
            printData.GloveDesc = gloveDesc;
            printData.GloveCategory = gloveCategory;
            printData.Resource = resource;
            printData.Template = template;
            printData.IsReprint = isReprint;
            return printData;
        }

        /// <summary>
        /// PrintDetails
        /// </summary>
        /// <param name="printdtoLst"></param>
        public static void PrintDetails(List<PrintDTO> printdtoLst)
        {
            FSDeviceIntegration deviceInt = new FSDeviceIntegration();
            deviceInt.SRBCPrint(printdtoLst);
            deviceInt = null;
        }

        #endregion

        #region BatchCard Reprint Log

        /// <summary>
        /// Get Reprint BatchCard details
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="plant"></param>
        /// <returns>List<RePrintBatchCardDTO></returns>
        public static List<RePrintBatchCardDTO> GetBatchCardReprintLog(DateTime fromDate, DateTime toDate, string plant)
        {
            List<RePrintBatchCardDTO> listRePrintBatchCardDTO = new List<RePrintBatchCardDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@FromDate", fromDate));
            prmList.Add(new FloorSqlParameter("@ToDate", toDate));
            prmList.Add(new FloorSqlParameter("@Plant", plant));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_HBC_BatchCardReprintLog_Get", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            listRePrintBatchCardDTO.Add(new RePrintBatchCardDTO(Convert.ToString(dr["BatchNumber"]),
                                Convert.ToString(dr["SerialNumber"]), Convert.ToString(dr["ReprintDateTime"]), Convert.ToString(dr["ProcessArea"]),
                                Convert.ToString(dr["PrintDatetime"]), Convert.ToString(dr["EmpName"]), Convert.ToString(dr["ReasonText"]),
                                Convert.ToString(dr["Plant"]),
                                "" // TODO:add print type for Surgical glove
                                ));
                        }
                    }
                }
                catch (ArgumentException argex)
                {
                    throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETBATCHCARDREPRINTLOGEEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return listRePrintBatchCardDTO;
        }

        /// <summary>
        /// Gets the Plant List
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetPlantList()
        {
            List<DropdownDTO> plantList = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_Plant_Get", null))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {

                        plantList = (from DataRow dr in dt.Rows
                                     select new DropdownDTO
                                     {
                                         IDField = FloorDBAccess.GetString(dr, "LocationName"),
                                         DisplayField = FloorDBAccess.GetString(dr, "LocationName")
                                     }).ToList();
                    }
                }
                catch (ArgumentException argex)
                {
                    throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETPLANTLISTEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return plantList;
        }

        #endregion

        #region QAI
        public static Boolean IsSurgicalGlove(decimal serialNumber)
        {
            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();
            try
            {
                lstfp.Add(new FloorSqlParameter("@serialnumber", serialNumber));
                return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_DOT_IsSurgicalGlove", lstfp));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.SERIALNUMBER_INVALID, Constants.BUSINESSLOGIC, ex);
            }
        }

        public static Boolean IsOnlineSurgicalGlove(decimal serialNumber)
        {
            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();
            try
            {
                lstfp.Add(new FloorSqlParameter("@serialnumber", serialNumber));
                return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_DOT_IsOnlineSurgicaGlove", lstfp));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.SERIALNUMBER_INVALID, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// To Get Complete Batch details by SN & Resource
        /// </summary>
        /// <param name="serialNo, resource"></param>
        /// <returns></returns>
        public static BatchDTO GetQAIDetails_SRBC(decimal serialNo) //insert seqno
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GetQAIDetails_SRBC", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new BatchDTO
                            {
                                SeqNo = FloorDBAccess.GetValue<Int32>(row, "SeqNo"),
                                SerialNumber = Convert.ToString(serialNo),
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                BatchOrder = FloorDBAccess.GetString(row, "BatchOrder"),
                                Resource = FloorDBAccess.GetString(row, "Resource"),
                                GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                Line = FloorDBAccess.GetString(row, "LineId"),
                                ShiftName = FloorDBAccess.GetString(row, "ShiftName"),
                                Size = FloorDBAccess.GetString(row, "Size"),
                                TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPCsWeight"),
                                BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                TotalPcs = FloorDBAccess.GetValue<Int32>(row, "TotalPcs"),
                                IsOnline = FloorDBAccess.GetValue<bool>(row, "IsOnline"),
                                QCType = FloorDBAccess.GetString(row, "QCType"),
                                Module = FloorDBAccess.GetString(row, "ModuleId"),
                                SubModule = FloorDBAccess.GetString(row, "SubModuleId"),
                                BatchType = FloorDBAccess.GetString(row, "BatchType"),
                                Location = FloorDBAccess.GetString(row, "locationname"),
                                BatchCarddate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate"),
                                Area = FloorDBAccess.GetString(row, "Area"),
                                Pool = FloorDBAccess.GetString(row, "Pool"), //#AZ 05/06/2018 Rework additional info.
                                RouteCategory = FloorDBAccess.GetString(row, "RouteCategory"), //#AZ 05/06/2018 Rework additional info.
                                DeliveryDate = FloorDBAccess.GetValue<DateTime>(row, "DeliveryDate"), //#AZ 05/06/2018 Rework additional info.
                            }).SingleOrDefault();
            }
            return objBatch;
        }
        #endregion
    }

}
