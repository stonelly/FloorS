// -----------------------------------------------------------------------
// <copyright file="HourlyBatchCardBLL.cs" company="Avanade">
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
    /// Hourly batch card business logic class
    /// </summary>
    public static class HourlyBatchCardBLL
    {
        #region constructors
        /*
        /// <summary>
        /// Initializes a new instance of the <see cref="HourlyBatchCardBLL" /> class.
        /// </summary>
        /// 
        public HourlyBatchCardBLL()
        {
            // No implementation required
        }
        */
        #endregion

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Line Selection

        #region private Methods

        /// <summary>
        /// #AZ 26/02/2018 1.n FDD@NGC_CR_090 To Get Line Batch Order Details
        /// </summary>
        /// <param name="prodPoolID"></param>
        /// <returns>List<BatchOrderDetailsDTO></returns>
        public static List<GloveBatchOrderDTO> GetBatchOrderDetails(string prodPoolID, string plantNo)
        {
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            List<GloveBatchOrderDTO> lstBatchOrderDetailsDTO = new List<GloveBatchOrderDTO>();
            prmList.Add(new FloorSqlParameter("@prodPoolID", prodPoolID));
            prmList.Add(new FloorSqlParameter("@PlantNo", plantNo));
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

        #region HSB not use
        ///// <summary>
        ///// #MK 23/03/2019 1.n To Get Line Batch Order Details based on BatchOrderId and Size
        ///// </summary>
        ///// <param name="prodPoolID"></param>
        ///// <param name="FGBatchOrder"></param>
        ///// <param name="size"></param>
        ///// <returns>List<BatchOrderDetailsDTO></returns>
        //public static List<GloveBatchOrderDTO> GetBatchOrderDetails(string prodPoolID, string FGBatchOrder, string size)
        //{
        //    List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
        //    List<GloveBatchOrderDTO> lstBatchOrderDetailsDTO = new List<GloveBatchOrderDTO>();
        //    prmList.Add(new FloorSqlParameter("@prodPoolID", prodPoolID));
        //    prmList.Add(new FloorSqlParameter("@PlantNo", WorkStationDTO.GetInstance().Location.ToString()));
        //    prmList.Add(new FloorSqlParameter("@BatchOrderId", FGBatchOrder));
        //    prmList.Add(new FloorSqlParameter("@Size", size));
        //    using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_BatchOrderDetails_Get", prmList))
        //    {
        //        try
        //        {
        //            if (dt != null && dt.Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    GloveBatchOrderDTO lnbodDTO = new GloveBatchOrderDTO();
        //                    DateTime dtSchedStart = Convert.ToDateTime(FloorDBAccess.GetString(dr, "SchedStart"));
        //                    DateTime dtSchedFromTime = Convert.ToDateTime(FloorDBAccess.GetString(dr, "SchedFromTime"));
        //                    DateTime dtSchedEnd = Convert.ToDateTime(FloorDBAccess.GetString(dr, "SchedEnd"));
        //                    DateTime dtSchedToTime = Convert.ToDateTime(FloorDBAccess.GetString(dr, "SchedToTime"));
        //                    lnbodDTO.SchedStart = dtSchedStart.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
        //                    lnbodDTO.SchedFromTime = dtSchedFromTime.ToString("H:mm", CultureInfo.InvariantCulture);
        //                    lnbodDTO.SchedEnd = dtSchedEnd.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
        //                    lnbodDTO.SchedToTime = dtSchedToTime.ToString("H:mm", CultureInfo.InvariantCulture);
        //                    lnbodDTO.ProdStatus = FloorDBAccess.GetString(dr, "ProdStatus");
        //                    lnbodDTO.SalesOrder = FloorDBAccess.GetString(dr, "SalesOrder");
        //                    lnbodDTO.BthOrderId = FloorDBAccess.GetString(dr, "BthOrderId");
        //                    lnbodDTO.ItemId = FloorDBAccess.GetString(dr, "ItemId");
        //                    lnbodDTO.Size = FloorDBAccess.GetString(dr, "Size");
        //                    lnbodDTO.QtySched = FloorDBAccess.GetValue<decimal>(dr, "QtySched").ToString("#,##0");
        //                    lnbodDTO.ReportedQty = FloorDBAccess.GetValue<Int32>(dr, "ReportedQty").ToString("#,##0");
        //                    lnbodDTO.RemainingQty = FloorDBAccess.GetValue<decimal>(dr, "RemainingQty").ToString("#,##0");
        //                    lnbodDTO.Resource = FloorDBAccess.GetString(dr, "Resource");
        //                    lnbodDTO.ResourceGrp = FloorDBAccess.GetString(dr, "ResourceGrp");
        //                    lstBatchOrderDetailsDTO.Add(lnbodDTO);
        //                }
        //            }
        //        }
        //        catch (ArgumentException argex)
        //        {
        //            throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new FloorSystemException(Messages.GETLINESELECTIONDETAILSEXCEPTION, Constants.BUSINESSLOGIC, ex);
        //        }
        //    }
        //    return lstBatchOrderDetailsDTO;
        //}
        #endregion

        /// <summary>
        /// #AZ 26/06/2018 1.n FDD@NGC_CR_094 Creation of HBC based on Registered Output
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
                            lnbodDTO.PackSize = FloorDBAccess.GetValue<Int32>(dr, "PackingSize");
                            lnbodDTO.InBox = FloorDBAccess.GetValue<Int32>(dr, "InnerBox");
                            lnbodDTO.ReportedQty = FloorDBAccess.GetValue<Int32>(dr, "ReportedQty");
                            lnbodDTO.sOutputDate = dtOutputTime.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                            lnbodDTO.sOutputTime = dtOutputTime.ToString("H:mm", CultureInfo.InvariantCulture);
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

        /// <summary>
        /// To Get Line Selsction Details
        /// </summary>
        /// <param name="workStationName"></param>
        /// <returns>List<LineSelectionDTO></returns>
        public static List<LineSelectionDTO> GetLineSelectionDetails(string workStationName)
        {
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            List<LineSelectionDTO> lstLineSelectionDTO = new List<LineSelectionDTO>();
            prmList.Add(new FloorSqlParameter("@WorkStationName", workStationName));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_HBC_LineSelectionDetails_Get", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            LineSelectionDTO lnselDTO = new LineSelectionDTO();
                            lnselDTO.Id = FloorDBAccess.GetString(dr, "Id");
                            lnselDTO.LineId = FloorDBAccess.GetString(dr, "LineId");
                            lnselDTO.StartPrint = FloorDBAccess.GetValue<Boolean>(dr, "StartPrint");
                            lnselDTO.LineStartDateTime = FloorDBAccess.GetString(dr, "LineStartDateTime");
                            lnselDTO.TierSide = SplitintoLine(FloorDBAccess.GetString(dr, "TierSide"));
                            lnselDTO.GloveType = SplitintoLine(FloorDBAccess.GetString(dr, "GloveType"));
                            lnselDTO.GloveSize = SplitintoLine(FloorDBAccess.GetString(dr, "GloveSize"));
                            lnselDTO.IsDoubleFormer = FloorDBAccess.GetValue<Boolean>(dr, "IsDoubleFormer");
                            lnselDTO.IsPrintByFormer = FloorDBAccess.GetValue<Boolean>(dr, "IsPrintByFormer");
                            lstLineSelectionDTO.Add(lnselDTO);
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
            return lstLineSelectionDTO;
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

        public static object getHBCSerialNo(DateTime printDate, string Line)
        {
            List<DropdownDTO> HBCSerialNo = null;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@PlantNo", WorkStationDTO.GetInstance().Location));
            PrmList.Add(new FloorSqlParameter("@PrintDate", printDate));
            PrmList.Add(new FloorSqlParameter("@Line", Line));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_HBCSerialNo_Get", PrmList))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        HBCSerialNo = (from DataRow dr in dt.Rows
                                       select new DropdownDTO
                                       {
                                           IDField = FloorDBAccess.GetString(dr, "SerialNo"),
                                           DisplayField = FloorDBAccess.GetString(dr, "SerialNo")
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
            return HBCSerialNo;
        }

        /// <summary>
        /// To Update Line Start 
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="isStartPrint"></param>
        /// <param name="workStationName"></param>
        /// <param name="operaterId"></param>
        /// <returns></returns>
        public static int UpdateLineStartDateTimeToStart(string lineId, bool isStartPrint, string workStationName, string operaterId)
        {
            int rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@Line", lineId));
            PrmList.Add(new FloorSqlParameter("@isStartPrint", isStartPrint));
            PrmList.Add(new FloorSqlParameter("@WorkStationName", workStationName));
            PrmList.Add(new FloorSqlParameter("@OperaterId", operaterId));
            rowsaffected = FloorDBAccess.ExecuteNonQuery("usp_HBC_LineStartDateTime_Update", PrmList);
            return rowsaffected;
        }

        /// <summary>
        /// To Update Line Stop
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="isStartPrint"></param>
        /// <param name="workStationName"></param>
        /// <param name="operaterId"></param>
        /// <returns></returns>
        public static int UpdateLineStopDateTimeToStart(string lineId, bool isStartPrint, string workStationName, string operaterId)
        {
            int rowsaffected;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@Line", lineId));
            prmList.Add(new FloorSqlParameter("@isStartPrint", isStartPrint));
            prmList.Add(new FloorSqlParameter("@WorkStationName", workStationName));
            prmList.Add(new FloorSqlParameter("@OperaterId", operaterId));
            rowsaffected = FloorDBAccess.ExecuteNonQuery("usp_HBC_LineStopDateTime_Update", prmList);
            return rowsaffected;
        }

        /// <summary>
        /// HourlyBatchCardSave
        /// </summary>
        /// <param name="batchCardDate"></param>
        /// <param name="workStationName"></param>
        /// <param name="ModuleId"></param>
        /// <param name="SubModuleID"></param>
        /// <returns></returns>
        public static List<BatchDTO> HourlyBatchCardSave(DateTime batchCardDate, string workStationName, string ModuleId, string SubModuleID)
        {
            List<BatchDTO> listBatchDTO = new List<BatchDTO>();
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@BatchCardDate", batchCardDate));
            PrmList.Add(new FloorSqlParameter("@WorkStationName", workStationName));
            PrmList.Add(new FloorSqlParameter("@ModuleId", Convert.ToInt32(ModuleId)));
            PrmList.Add(new FloorSqlParameter("@SubModuleID", Convert.ToInt32(SubModuleID)));
            PrmList.Add(new FloorSqlParameter("@SIteNumber", Convert.ToInt32(FloorSystemConfiguration.GetInstance().intSiteNumber)));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("Usp_HBC_HourlyBatchCard_Save", PrmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            BatchDTO batch = new BatchDTO();
                            batch.SerialNumber = FloorDBAccess.GetString(dr, "SerialNumber");
                            batch.BatchNumber = FloorDBAccess.GetString(dr, "BatchNumber");
                            batch.GloveType = FloorDBAccess.GetString(dr, "GloveType");
                            batch.Size = FloorDBAccess.GetString(dr, "GloveSize");
                            batch.TierSide = FloorDBAccess.GetString(dr, "TierSide");
                            batch.BatchCarddate = FloorDBAccess.GetValue<DateTime>(dr, "BatchCardDate");
                            listBatchDTO.Add(batch);
                            log.InfoFormat("SerialNo {0}", batch.SerialNumber);
                        }
                    }
                }
                catch (ArgumentException argex)
                {
                    throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSIZEBYGLOVETYPEEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return listBatchDTO;
        }

        public static bool LineSelectionClosed(string operatorId, string workStationName)
        {
            bool issave = false;
            List<BatchDTO> listBatchDTO = new List<BatchDTO>();
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@OperatorId", operatorId));
            PrmList.Add(new FloorSqlParameter("@WorkStationName", workStationName));
            if (FloorDBAccess.ExecuteNonQuery("usp_HBC_LineSelection_Close", PrmList) > 0)
            {
                issave = true;
            }
            return issave;
        }

        #endregion

        #region private Methods
        /// <summary>
        /// Split the string into multiline
        /// </summary>
        /// <param name="lineval"></param>
        /// <returns></returns>
        private static string SplitintoLine(string lineval)
        {
            StringBuilder sb = new StringBuilder("");
            if (lineval.Length > 0)
            {
                //lineval wil have delimiter '~' to separate line even at the end of string has '~'
                string[] rowValues = lineval.Substring(0, lineval.Length - 1).Split('~');
                for (int i = 0; i < rowValues.Length; i++)
                {
                    sb.Append(rowValues[i]);
                    if (i + 1 != rowValues.Length)
                    {
                        sb.Append(Environment.NewLine);
                    }
                }
            }
            return sb.ToString();
        }

        #endregion

        #endregion

        #region Manual Printing

        #region public Methods
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
        /// Get Reason for Reprint List
        /// </summary>
        /// <returns>List<DropdownDTO></returns>
        public static List<DropdownDTO> GetReasonForReprintON2GBatcgCard()
        {
            List<DropdownDTO> reasonForReprint = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_Reasons_Reprint_ON2G_Get", null))
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

        public static List<DropdownDTO> GetReasonForAPM()
        {
            List<DropdownDTO> reasonForAPM = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_Reasons_APM_Get", null))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        reasonForAPM = (from DataRow dr in dt.Rows
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
            return reasonForAPM;
        }


        public static List<DropdownDTO> GetReasonForManualPrint()
        {
            List<DropdownDTO> ReasonList = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_ManualPrintReasons_Get", null))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);

                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        ReasonList = (from DataRow dr in dt.Rows
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
            return ReasonList;
        }

        /// <summary>
        /// Get Line Details
        /// </summary>
        /// <param name="workStationName"></param>
        /// <returns>List<DropdownDTO></returns>
        public static List<DropdownDTO> GetLine(string workStationName)
        {
            List<DropdownDTO> lineList = null;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@WorkStationName", workStationName));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_LineMaster_Get", prmList))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        lineList = (from r in dt.AsEnumerable()
                                    select new DropdownDTO
                                    {
                                        IDField = r.Field<string>("LineNumber"),
                                        DisplayField = r.Field<string>("LineNumber")
                                    }).ToList();
                    }
                }
                catch (ArgumentException argex)
                {
                    throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETLINEEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return lineList;
        }

        /// <summary>
        /// Validate saved data before Print
        /// </summary>
        /// <param name="Resource"></param>
        /// <param name="OutputTime"></param>
        /// <param name="BatchOrder"></param>
        /// <returns>boolean</returns>
        public static bool CheckSavedHBCToValidate(string Resource, DateTime OutputTime, string BatchOrder)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@Resource", Resource));
            lstParameters.Add(new FloorSqlParameter("@OutputTime", OutputTime));
            lstParameters.Add(new FloorSqlParameter("@BatchOrder", BatchOrder));
            return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_SavedHBCToValidate", lstParameters));
        }

        /// <summary>
        /// To get Glove size foe lilne
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns>LineSelection</returns>
        public static LineSelectionReprint GetTierSideGloveSize(string lineId)
        {
            LineSelectionReprint lineSelection = new LineSelectionReprint();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@LineId", lineId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_TierSideGloveSize_Get", prmList))
            {
                try
                {
                    lineSelection.LineId = lineId;
                    string trSide = string.Empty;
                    string size = string.Empty;
                    foreach (DataRow dr in dt.Rows)
                    {
                        lineSelection.IsDoubleFormer = Convert.IsDBNull(dr["IsDoubleFormer"]) ? false : Convert.ToBoolean(dr["IsDoubleFormer"]);
                        trSide = Convert.ToString(dr["TierSide"]);
                        Constants.Tierside tierSd;
                        size = Convert.ToString(dr["GloveSize"]);
                        if (Enum.TryParse(trSide, out tierSd))
                        {
                            switch (tierSd)
                            {
                                case Constants.Tierside.LT:
                                    lineSelection.LTSize = size;
                                    break;
                                case Constants.Tierside.LB:
                                    lineSelection.LBSize = size;
                                    break;
                                case Constants.Tierside.RT:
                                    lineSelection.RTSize = size;
                                    break;
                                case Constants.Tierside.RB:
                                    lineSelection.RBSize = size;
                                    break;
                            }
                            trSide = string.Empty;
                            size = string.Empty;
                        }
                    }
                }
                catch (RowNotInTableException rex)
                {
                    throw new FloorSystemException(Messages.ROWNOTINTABLEEXCEPTION, Constants.BUSINESSLOGIC, rex);
                }
                catch (ArgumentException argex)
                {
                    throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSIZEBYGLOVETYPEEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return lineSelection;
        }

        /// <summary>
        /// To Get Last Six Hours
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetLastReprinthrs()
        {
            int reprintHrs = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intHBCReprintHours);
            List<DropdownDTO> LastReprinthrs = new List<DropdownDTO>();
            int currentHours = CommonBLL.GetCurrentDateAndTimeFromServer().Hour;
            for (int i = currentHours; i > currentHours - reprintHrs; i--)
            {
                int k = i;
                k = k < Constants.ZERO ? 24 + k : k;
                LastReprinthrs.Add(new DropdownDTO(Convert.ToString(k), Convert.ToString(k).Length == 2 ? Convert.ToString(k) + ":00" : "0" + Convert.ToString(k) + ":00"));
            }
            return LastReprinthrs;
        }

        /// <summary>
        /// Manual Print Batch Details Get
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="hour"></param>
        /// <param name="tierSide"></param>
        /// <param name="batchCardDate"></param>
        /// <returns>List<BatchDTO></returns>
        public static List<BatchDTO> ManualPrintBatchDetailsGet(string lineId, int hour, string tierSide, DateTime batchCardDate, string ModuleId, string SubModuleID, int WorkStationNumber)
        {
            List<BatchDTO> listBatchDTO = new List<BatchDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@LineId", lineId));
            prmList.Add(new FloorSqlParameter("@hour", hour));
            prmList.Add(new FloorSqlParameter("@TierSide", tierSide));
            prmList.Add(new FloorSqlParameter("@BatchCardDate", batchCardDate));
            prmList.Add(new FloorSqlParameter("@ModuleId", ModuleId));
            prmList.Add(new FloorSqlParameter("@SubModuleID", SubModuleID));
            prmList.Add(new FloorSqlParameter("@SIteNumber", Convert.ToInt32(FloorSystemConfiguration.GetInstance().intSiteNumber)));
            prmList.Add(new FloorSqlParameter("@WorkStationNumber", WorkStationNumber));


            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_ManualPrintBatchDetails_Get", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            BatchDTO batch = new BatchDTO();
                            batch.SerialNumber = FloorDBAccess.GetString(dr, "SerialNumber");
                            batch.BatchNumber = FloorDBAccess.GetString(dr, "BatchNumber");
                            batch.GloveType = FloorDBAccess.GetString(dr, "GloveType");
                            batch.Size = FloorDBAccess.GetString(dr, "Size");
                            batch.TierSide = FloorDBAccess.GetString(dr, "TierSide");
                            batch.BatchCarddate = FloorDBAccess.GetValue<DateTime>(dr, "BatchCardDate");
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
        /// #Azrul 05/01/2021: Get Batch Details for Print Online 2G
        /// </summary>
        /// <returns>List<BatchDTO></returns>
        public static List<BatchDTO> GetPrintOnline2GDetails(string userId, int shiftId, string line, string gloveCode, string size, string batchOrder, int packingSize, int innerBox, decimal tenPcsWeight, DateTime batchCardDate, string moduleId, string subModuleId, int sitenumber, int workStationNumber, string resource)
        {
            List<BatchDTO> listBatchDTO = new List<BatchDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@UserId", userId));
            prmList.Add(new FloorSqlParameter("@ShiftId", shiftId));
            prmList.Add(new FloorSqlParameter("@Line", line));
            prmList.Add(new FloorSqlParameter("@GloveCode", gloveCode));
            prmList.Add(new FloorSqlParameter("@Size", size));
            prmList.Add(new FloorSqlParameter("@BatchOrder", batchOrder));
            prmList.Add(new FloorSqlParameter("@PackingSize", packingSize));
            prmList.Add(new FloorSqlParameter("@InnerBox", innerBox));
            prmList.Add(new FloorSqlParameter("@TenPcsWeight", tenPcsWeight));
            prmList.Add(new FloorSqlParameter("@BatchCardDate", batchCardDate));
            prmList.Add(new FloorSqlParameter("@ModuleId", moduleId));
            prmList.Add(new FloorSqlParameter("@SubModuleID", subModuleId));
            prmList.Add(new FloorSqlParameter("@SiteNumber", sitenumber));
            prmList.Add(new FloorSqlParameter("@WorkStationNumber", workStationNumber));
            prmList.Add(new FloorSqlParameter("@Resource", resource));

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("Usp_DOT_Online2G_Print_Save", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            BatchDTO batch = new BatchDTO();
                            batch.SerialNumber = FloorDBAccess.GetString(dr, "SerialNumber");
                            batch.BatchNumber = FloorDBAccess.GetString(dr, "BatchNumber");
                            batch.BatchOrder = FloorDBAccess.GetString(dr, "BatchOrder");
                            batch.ShiftName = FloorDBAccess.GetString(dr, "ShiftName");
                            batch.Resource = FloorDBAccess.GetString(dr, "Resource");
                            batch.GloveType = FloorDBAccess.GetString(dr, "GloveCode");
                            batch.GloveTypeDescription = FloorDBAccess.GetString(dr, "GloveCategory");
                            batch.BatchWeight = FloorDBAccess.GetValue<decimal>(dr, "BatchWeight");
                            batch.TenPcsWeight = FloorDBAccess.GetValue<decimal>(dr, "TenPcsWeight");
                            batch.Size = FloorDBAccess.GetString(dr, "Size");
                            batch.BatchCarddate = FloorDBAccess.GetValue<DateTime>(dr, "BatchCardDate");
                            batch.PackingSize = FloorDBAccess.GetValue<int>(dr, "PackingSize");
                            batch.InnerBoxCount = FloorDBAccess.GetValue<int>(dr, "InnerBox");
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
        /// #AZ 27/03/2018 1.NGC_CR_094: Get Batch Details for Print Batch Order
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="hour"></param>
        /// <param name="tierSide"></param>
        /// <param name="batchCardDate"></param>
        /// <returns>List<BatchDTO></returns>
        public static List<BatchOrderDetailsDTO> GetPrintBatchDetails(string userId, int shiftId, DateTime outputTime, string lineId, DateTime batchCardDate, string ModuleId, string SubModuleID, int sitenumber, int WorkStationNumber, string resourceId, string packingSize, string innerBox)
        {
            List<BatchOrderDetailsDTO> listBatchDTO = new List<BatchOrderDetailsDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@UserId", userId));
            prmList.Add(new FloorSqlParameter("@ShiftId", shiftId));
            prmList.Add(new FloorSqlParameter("@OutputTime", outputTime));
            prmList.Add(new FloorSqlParameter("@LineId", lineId));
            prmList.Add(new FloorSqlParameter("@BatchCardDate", batchCardDate));
            prmList.Add(new FloorSqlParameter("@ModuleId", ModuleId));
            prmList.Add(new FloorSqlParameter("@SubModuleID", SubModuleID));
            prmList.Add(new FloorSqlParameter("@SIteNumber", sitenumber));
            prmList.Add(new FloorSqlParameter("@WorkStationNumber", WorkStationNumber));
            prmList.Add(new FloorSqlParameter("@ResourceId", resourceId));
            prmList.Add(new FloorSqlParameter("@PackingSize", packingSize));
            prmList.Add(new FloorSqlParameter("@InnerBox", innerBox));

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("Usp_DOT_HBC_Print_Save", prmList))
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
                            batch.GloveType = FloorDBAccess.GetString(dr, "GloveType");
                            batch.Size = FloorDBAccess.GetString(dr, "Size");
                            batch.BatchCarddate = FloorDBAccess.GetValue<DateTime>(dr, "BatchCardDate");
                            batch.Resource = FloorDBAccess.GetString(dr, "Resource");
                            batch.PackingSize = FloorDBAccess.GetString(dr, "PackingSize");
                            batch.InnerBox = FloorDBAccess.GetString(dr, "InnerBox");
                            batch.TotalGloveQty = FloorDBAccess.GetString(dr, "TotalGloveQty");
                            batch.OutputTime = FloorDBAccess.GetValue<DateTime>(dr, "OutputTime");
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
        /// #AZ 14/03/2018 1.NGC_CR_094: Get Shift Info for Print Batch Order
        /// </summary>
        /// <returns>List<DropdownDTO></returns>
        public static List<DropdownDTO> GetShiftInfo(string area)
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
        /// #AZ 30/04/2018 1.NGC_CR_090: Get Output Time for Print Batch Order
        /// </summary>
        /// <returns>List<DropdownDTO></returns>
        public static List<DropdownDTO> GetOutputTime(int HBCReprintHours)
        {
            List<DropdownDTO> OutputTime = null;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@HBCReprintHours", HBCReprintHours));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_GET_OutputTime", prmList))
            {
                //dt.Rows.InsertAt(dt.NewRow(), 0);
                if (dt != null && dt.Rows.Count != 0)
                {
                    OutputTime = (from DataRow dr in dt.Rows
                                  select new DropdownDTO
                                  {
                                      IDField = FloorDBAccess.GetString(dr, "Time"),
                                      DisplayField = FloorDBAccess.GetString(dr, "Time")
                                  }).ToList();
                }
            }
            return OutputTime;
        }

        /// <summary>
        /// #AZ 14/03/2018 1.NGC_CR_094: Get ResourceGrp, Resources and Batch Order details for Glove Output screen.
        /// </summary>
        /// <returns>List<DropdownDTO></returns>
        public static List<BatchOrderDetailsDTO> GetResourceNBODetails(int locationId, string lineId, string res, string bo, string resFilter1, string resFilter2, string resFilter3, string resFilter4)
        {
            List<BatchOrderDetailsDTO> ResourceNBODetails = null;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@LocationId", locationId));
            prmList.Add(new FloorSqlParameter("@LineId", lineId));
            prmList.Add(new FloorSqlParameter("@Resource", res));
            prmList.Add(new FloorSqlParameter("@BO", bo));
            prmList.Add(new FloorSqlParameter("@ResFilter1", resFilter1));
            prmList.Add(new FloorSqlParameter("@ResFilter2", resFilter2));
            prmList.Add(new FloorSqlParameter("@ResFilter3", resFilter3));
            prmList.Add(new FloorSqlParameter("@ResFilter4", resFilter4));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_ResourceNBODetails_Get", prmList))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                if (dt != null && dt.Rows.Count != 0)
                {
                    ResourceNBODetails = (from DataRow dr in dt.Rows
                                          select new BatchOrderDetailsDTO
                                          {
                                              Resource = FloorDBAccess.GetString(dr, "Resource"),
                                              ResourceId = FloorDBAccess.GetString(dr, "ResourceId"),
                                              LineId = FloorDBAccess.GetString(dr, "LineId"),
                                              Line = FloorDBAccess.GetString(dr, "Line"),
                                              TierSide = FloorDBAccess.GetString(dr, "TierSide"),
                                              BthOrderId = FloorDBAccess.GetString(dr, "BatchOrder"),
                                              ItemId = FloorDBAccess.GetString(dr, "GloveCode"),
                                              Size = FloorDBAccess.GetString(dr, "Size")
                                          }).ToList();
                }
            }
            return ResourceNBODetails;
        }

        /// <summary>
        /// #AZRUL 24/11/2020 HTLG_P7CR_025 Online 2G Glove.
        /// </summary>
        /// <returns>List<DropdownDTO></returns>
        public static List<BatchOrderDetailsDTO> GetOnline2GResources(int locationId, string lineId, string itemId, string size)
        {
            List<BatchOrderDetailsDTO> Online2GResources = null;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@LocationId", locationId));
            prmList.Add(new FloorSqlParameter("@LineId", lineId));
            prmList.Add(new FloorSqlParameter("@ItemId", itemId));
            prmList.Add(new FloorSqlParameter("@Size", size));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_Online2GResources_Get", prmList))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                if (dt != null && dt.Rows.Count != 0)
                {
                    Online2GResources = (from DataRow dr in dt.Rows
                                         select new BatchOrderDetailsDTO
                                         {
                                             LineId = FloorDBAccess.GetString(dr, "LineId"),
                                             ItemId = FloorDBAccess.GetString(dr, "GloveCode"),
                                             Size = FloorDBAccess.GetString(dr, "Size"),
                                             BthOrderId = FloorDBAccess.GetString(dr, "BatchOrder")
                                         }).ToList();
                }
            }
            return Online2GResources;
        }

        /// <summary>
        /// </summary>
        /// <returns>List<DropdownDTO></returns>
        public static List<BatchOrderDetailsDTO> GetON2GReprintLineByPlant(int locationId)
        {
            List<BatchOrderDetailsDTO> ON2GReprintLine = null;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@LocationId", locationId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_ReprintON2G_GetAllLineByPlant", prmList))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                if (dt != null && dt.Rows.Count != 0)
                {
                    ON2GReprintLine = (from DataRow dr in dt.Rows
                                         select new BatchOrderDetailsDTO
                                         {
                                             LineId = FloorDBAccess.GetString(dr, "LineNumber"),
                                         }).ToList();
                }
            }
            return ON2GReprintLine;
        }

        /// <summary>
        /// Manual Print Batch Details Get By Serial Number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>List<BatchDTO></returns>
        public static List<BatchDTO> ManualPrintBatchDetailsBySerialNumberGet(string serialNumber)
        {
            List<BatchDTO> listBatchDTO = new List<BatchDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_ManualPrintBatchDetailsBySerialNumber_Get", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            BatchDTO batch = new BatchDTO();
                            batch.SerialNumber = FloorDBAccess.GetString(dr, "SerialNumber");
                            batch.BatchNumber = FloorDBAccess.GetString(dr, "BatchNumber");
                            batch.Line = FloorDBAccess.GetString(dr, "LineId");
                            batch.GloveType = FloorDBAccess.GetString(dr, "GloveType");
                            batch.Size = FloorDBAccess.GetString(dr, "Size");
                            batch.TierSide = FloorDBAccess.GetString(dr, "TierSide");
                            batch.BatchCarddate = FloorDBAccess.GetValue<DateTime>(dr, "BatchCardDate");
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
        public static List<BatchOrderDetailsDTO> SaveReprintBatchCard(string serialNumbers, DateTime reprintDateTime, string operatorId, int reasonId, string workStationName)
        {
            List<BatchOrderDetailsDTO> listBatchDTO = new List<BatchOrderDetailsDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@SerialNumber", serialNumbers));
            prmList.Add(new FloorSqlParameter("@ReprintDateTime", reprintDateTime));
            prmList.Add(new FloorSqlParameter("@OperatorId", operatorId));
            prmList.Add(new FloorSqlParameter("@ReasonId", reasonId));
            prmList.Add(new FloorSqlParameter("@WorkStationName", workStationName));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_HBC_ReprintBatchCard_Save", prmList))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BatchOrderDetailsDTO batch = new BatchOrderDetailsDTO();
                        batch.SerialNumber = FloorDBAccess.GetString(dr, "SerialNumber");
                        batch.BatchNumber = FloorDBAccess.GetString(dr, "BatchNumber");
                        batch.GloveType = FloorDBAccess.GetString(dr, "GloveType");
                        batch.Size = FloorDBAccess.GetString(dr, "Size");
                        batch.BatchCarddate = FloorDBAccess.GetValue<DateTime>(dr, "BatchCardDate");
                        batch.Resource = FloorDBAccess.GetString(dr, "Resource");
                        batch.OutputTime = FloorDBAccess.GetValue<DateTime>(dr, "OutTime");
                        batch.PackingSize = FloorDBAccess.GetString(dr, "PackingSize");
                        batch.InnerBox = FloorDBAccess.GetString(dr, "InnerBox");
                        batch.TotalGloveQty = FloorDBAccess.GetString(dr, "TotalGloveQty");
                        listBatchDTO.Add(batch);
                    }
                }
            }
            return listBatchDTO;
        }

        /// <summary>
        /// HBC Reprint Bind Data.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static List<BatchOrderDetailsDTO> GetReprintDetails(string serialNumber, string resource)
        {
            List<BatchOrderDetailsDTO> listBatchDTO = new List<BatchOrderDetailsDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            prmList.Add(new FloorSqlParameter("@Resource", resource));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_HBC_ReprintBatchCard_Get", prmList))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BatchOrderDetailsDTO batch = new BatchOrderDetailsDTO();
                        batch.OutputTime = FloorDBAccess.GetValue<DateTime>(dr, "OutTime");
                        batch.LineId = FloorDBAccess.GetString(dr, "LineId");
                        batch.TierSide = FloorDBAccess.GetString(dr, "Tier");
                        batch.SerialNumber = FloorDBAccess.GetString(dr, "SerialNo");
                        listBatchDTO.Add(batch);
                    }
                }
            }
            return listBatchDTO;
        }

        /// <summary>
        /// HBC Reprint Bind Data.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static List<BatchOrderDetailsDTO> GetReprintON2GDetails(string serialNumber, string resource)
        {
            List<BatchOrderDetailsDTO> listBatchDTO = new List<BatchOrderDetailsDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            prmList.Add(new FloorSqlParameter("@Resource", resource));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_HBC_ReprintBatchCard_Get", prmList))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BatchOrderDetailsDTO batch = new BatchOrderDetailsDTO();
                        batch.OutputTime = FloorDBAccess.GetValue<DateTime>(dr, "OutTime");
                        batch.LineId = FloorDBAccess.GetString(dr, "LineId");
                        batch.TierSide = FloorDBAccess.GetString(dr, "Tier");
                        batch.SerialNumber = FloorDBAccess.GetString(dr, "SerialNo");
                        listBatchDTO.Add(batch);
                    }
                }
            }
            return listBatchDTO;
        }

        public static List<BatchDTO> usp_ManualPrintBatchCard_Get(string lineId, int hour, string tierSide, DateTime batchCardDate, string ModuleId, string SubModuleID, int WorkStationNumber)
        {
            List<BatchDTO> listBatchDTO = new List<BatchDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@LineId", lineId));
            prmList.Add(new FloorSqlParameter("@Hour", hour));
            prmList.Add(new FloorSqlParameter("@TierSide", tierSide));
            prmList.Add(new FloorSqlParameter("@BatchCardDate", batchCardDate));
            prmList.Add(new FloorSqlParameter("@ModuleId", ModuleId));
            prmList.Add(new FloorSqlParameter("@SubModuleID", SubModuleID));
            prmList.Add(new FloorSqlParameter("@SIteNumber", Convert.ToInt32(FloorSystemConfiguration.GetInstance().intSiteNumber)));
            prmList.Add(new FloorSqlParameter("@WorkStationNumber", WorkStationNumber));

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_ManualPrintBatchCard_Get", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            BatchDTO batch = new BatchDTO();
                            batch.SerialNumber = FloorDBAccess.GetString(dr, "SerialNumber");
                            batch.BatchNumber = FloorDBAccess.GetString(dr, "BatchNumber");
                            batch.GloveType = FloorDBAccess.GetString(dr, "GloveType");
                            batch.Size = FloorDBAccess.GetString(dr, "Size");
                            batch.TierSide = FloorDBAccess.GetString(dr, "TierSide");
                            batch.BatchCarddate = FloorDBAccess.GetValue<DateTime>(dr, "BatchCardDate");
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
        /// To Check Is SerialNumber
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static bool IsSerialNumber(decimal serialNumber)
        {
            if (Convert.ToString(serialNumber).Length != 10)
            {
                return false;
            }
            bool IsSerialNumber = false;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            IsSerialNumber = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_SEL_SERIALNUMBER", PrmList));
            return IsSerialNumber;
        }

        /// <summary>
        /// Get Glove Category
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        private static string GetGloveCategory(string glovecode)
        {
            string glovecategory = string.Empty;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@Glovecode", glovecode));
            glovecategory = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_DOT_GET_GLOVECATEGORY", PrmList));
            return glovecategory;
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
        //public static PrintDTO GetBatchList(string dateTime, string barcodeText, string batchNumber, string batchWeight, string size, string tenPcsWeight, bool tenPcsMsg, string gloveDesc, string side, string template, bool isReprint = false, bool isManual = false)
        //{
        //    PrintDTO printData = new PrintDTO();
        //    printData.DateTime = dateTime;
        //    printData.BatchNumber = batchNumber;
        //    printData.BatchWeight = batchWeight;
        //    printData.Size = size;
        //    printData.TenPcsWeight = tenPcsWeight;
        //    printData.TenPcsMsg = tenPcsMsg;
        //    printData.GloveDesc = gloveDesc;
        //    printData.TierSide = side;
        //    printData.SerialNumber = barcodeText;
        //    printData.Template = template;
        //    printData.IsReprint = isReprint;
        //    printData.IsManual = isManual;
        //    printData.GloveCategory = GetGloveCategory(gloveDesc);
        //    return printData;
        //}
        public static PrintDTO GetBatchList(string outputTime, string barcodeText, string batchNumber, string resource, string size, string gloveDesc, string packingSize, string innerBox, string totalGloveQty, string template, bool isReprint = false)
        {
            PrintDTO printData = new PrintDTO();
            printData.DateTime = outputTime;
            printData.BatchNumber = batchNumber;
            printData.SerialNumber = barcodeText;
            printData.Size = size;
            printData.GloveDesc = gloveDesc;
            printData.PackingSize = packingSize;
            printData.InnerBox = innerBox;
            printData.TotalGloveQty = totalGloveQty;
            printData.GloveCategory = GetGloveCategory(gloveDesc);
            printData.Resource = resource;
            printData.Template = template;
            printData.IsReprint = isReprint;
            return printData;
        }

        public static bool CheckDuplicateBatchCard(string LineId, string TierSide, int SelectedHour, DateTime BatchCardDate)
        {
            bool IsDuplicate = false;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@LineId", LineId));
            PrmList.Add(new FloorSqlParameter("@TierSide", TierSide));
            PrmList.Add(new FloorSqlParameter("@SelectedHour", SelectedHour));
            PrmList.Add(new FloorSqlParameter("@BatchCardDate", BatchCardDate));

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_CheckDuplicateBatchCard", PrmList))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    IsDuplicate = true;
                }
            }

            return IsDuplicate;
        }

        /// <summary>
        /// PrintDetails
        /// </summary>
        /// <param name="printdtoLst"></param>
        public static void PrintDetails(List<PrintDTO> printdtoLst)
        {
            log.InfoFormat("HourlyBatchCardBLL.PrintDetails - printdtoLst COUNT : {0}", printdtoLst.Count.ToString());
            FSDeviceIntegration deviceInt = new FSDeviceIntegration();
            deviceInt.HBCPrint(printdtoLst);
            deviceInt = null;
        }

        /// <summary>
        /// Method to Print
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
        /// 
        public static void PrintDetailsON2G(string dateTime, string barcodeText, string batchNumber,  string size, string gloveDesc, string packingSize, string innerBox, string totalGloveQty, string template, bool isReprint = false, bool batchWeightMsg = false, string resource = "", string GloveTypeDescription = "")
        {

            PrintDTO printData = new PrintDTO();
            printData.DateTime = dateTime;
            printData.SerialNumber = barcodeText;
            printData.BatchNumber = batchNumber;
            printData.Size = size;
            printData.GloveDesc = gloveDesc;
            printData.PackingSize = packingSize;
            printData.InnerBox = innerBox;
            printData.TotalGloveQty = totalGloveQty;
            printData.GloveCategory = GloveTypeDescription;
            printData.Template = template;
            printData.IsReprint = isReprint;  
            printData.Resource = resource;
            FSDeviceIntegration deviceInt = new FSDeviceIntegration();
            deviceInt.PrintON2G(printData);
            deviceInt = null;
        }

        public static bool CheckON2GBCReprintLocation(string serialNo, string workstationID, out int plantLocation)
        {
            bool isCorrectPlant = false;
            plantLocation = 0;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@SerialNumber", serialNo));
            prmList.Add(new FloorSqlParameter("@WorkstationID", workstationID));
            try
            {
                using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_CheckON2GReprintLocation", prmList))
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        isCorrectPlant = Convert.ToBoolean(FloorDBAccess.GetValue<int>(dt.Rows[0], "isCorrectPlant"));
                        plantLocation = FloorDBAccess.GetValue<int>(dt.Rows[0], "Plant");
                    }
                }

                return isCorrectPlant;
            }
            catch (Exception ex)
            {
                var abc = ex.ToString();
                return false;
            }
        }

        public static APMLog SaveAPMLog(APMLog item)
        //public static APMLog SaveAPMLog(decimal serialNumber, int APMPackable, int APMStatus, int APMReason, DateTime CreatedDate, int CreatedBy)
        {
            APMLog objAPMDTO = new APMLog();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@SerialNumber", item.SerialNumber));
            prmList.Add(new FloorSqlParameter("@Resource", item.Resource));
            prmList.Add(new FloorSqlParameter("@APMPackable", item.APMPackable));
            prmList.Add(new FloorSqlParameter("@APMStatus", item.APMStatus));
            prmList.Add(new FloorSqlParameter("@APMReason", item.APMReason));
            prmList.Add(new FloorSqlParameter("@CreatedDate", item.CreatedDate));
            prmList.Add(new FloorSqlParameter("@CreatedBy", item.CreatedBy));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("Usp_APMLog_Save", prmList))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    //objAPMDTO.SerialNumber = FloorDBAccess.GetValue<decimal>(dt.Rows[0], "SerialNumber");
                    //objAPMDTO.APMPackable = FloorDBAccess.GetValue<bool>(dt.Rows[0], "APMPackable");
                    //objAPMDTO.APMStatus = FloorDBAccess.GetValue<bool>(dt.Rows[0], "APMStatus");
                    //objAPMDTO.APMReason = FloorDBAccess.GetValue<Int32>(dt.Rows[0], "APMReason");
                    //objAPMDTO.CreatedDate = FloorDBAccess.GetValue<DateTime>(dt.Rows[0], "CreatedDate");
                    //objAPMDTO.CreatedBy = FloorDBAccess.GetValue<Int32>(dt.Rows[0], "CreatedBy");
                }
            }
            return objAPMDTO;
        }

        public static APMLog GetDefaultAPMPackable(string batchOrder)
        {
            APMLog objAPMDTO = new APMLog();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@batchOrder", batchOrder));

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("Usp_APMPackable_Get", prmList))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    //objAPMDTO.SerialNumber = FloorDBAccess.GetValue<decimal>(dt.Rows[0], "SerialNumber");
                    objAPMDTO.APMPackable = FloorDBAccess.GetValue<bool>(dt.Rows[0], "APMPackable");
                    //objAPMDTO.APMStatus = FloorDBAccess.GetValue<bool>(dt.Rows[0], "APMStatus");
                    //objAPMDTO.APMReason = FloorDBAccess.GetValue<Int32>(dt.Rows[0], "APMReason");
                    //objAPMDTO.CreatedDate = FloorDBAccess.GetValue<DateTime>(dt.Rows[0], "CreatedDate");
                    //objAPMDTO.CreatedBy = FloorDBAccess.GetValue<Int32>(dt.Rows[0], "CreatedBy");
                }
            }
            return objAPMDTO;
        }
        #endregion
        #endregion

        #region BatchCard Reprint Log

        #region public Methods

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
                                Convert.ToString(dr["Plant"]), Convert.ToString(dr["PrintType"])));
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
            //added by MYAdamas sort by reprintdatetime desc
            // return listRePrintBatchCardDTO
            return listRePrintBatchCardDTO.OrderByDescending(p=>p.ReprintDateTime).ToList();
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

        public static ON2GBatchDTO SaveReprintON2GBatchCard(string serialNumber, DateTime reprintDateTime, string operatorId, int reasonId,string line, string workStationName)
        {
            ON2GBatchDTO objBatchDTO = new ON2GBatchDTO();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            prmList.Add(new FloorSqlParameter("@ReprintDateTime", reprintDateTime));
            prmList.Add(new FloorSqlParameter("@OperatorId", operatorId));
            prmList.Add(new FloorSqlParameter("@ReasonId", reasonId));
            prmList.Add(new FloorSqlParameter("@WorkStationName", workStationName));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_HBC_ReprintON2GBatchCard_Save", prmList))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    objBatchDTO.CurrentDateandTime = FloorDBAccess.GetValue<DateTime>(dt.Rows[0], "CurrentDateandTime");
                    objBatchDTO.SerialNumber = FloorDBAccess.GetString(dt.Rows[0], "SerialNumber");
                    objBatchDTO.BatchNumber = FloorDBAccess.GetString(dt.Rows[0], "BatchNumber");
                    objBatchDTO.Size = FloorDBAccess.GetString(dt.Rows[0], "Size");
                    objBatchDTO.GloveCode = FloorDBAccess.GetString(dt.Rows[0], "GloveCode");
                    objBatchDTO.PackingSize = FloorDBAccess.GetValue<Int32>(dt.Rows[0], "PackingSize");
                    objBatchDTO.InnerBox = FloorDBAccess.GetValue<Int32>(dt.Rows[0], "InnerBox");
                    objBatchDTO.TotalGloveQty = FloorDBAccess.GetValue<Int32>(dt.Rows[0], "TotalGloveQty");
                    objBatchDTO.GloveTypeDescription = FloorDBAccess.GetString(dt.Rows[0], "GloveTypeDescription");
                    objBatchDTO.Resource = FloorDBAccess.GetString(dt.Rows[0], "Resource");
                }
            }
            return objBatchDTO;
        }
        #endregion

        #endregion
    }

}
