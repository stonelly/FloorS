// -----------------------------------------------------------------------
// <copyright file="FinalPackingBLL.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Business.Logic
{
    using FloorSystem.Framework.Database;
    using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
    using Hartalega.FloorSystem.Framework;
    using Hartalega.FloorSystem.Framework.Common;
    using Hartalega.FloorSystem.Framework.DbExceptionLog;
    using Hartalega.FloorSystem.IntegrationServices;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Final packing business logic class
    /// </summary>
    public static class FinalPackingBLL
    //: Framework.Business.BusinessBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinalPackingBLL" /> class.
        /// </summary>
        #region CONSTRUCTOR
        static FinalPackingBLL()
        {
            // No implementation required
        }
        #endregion
        #region 2nd Grade
        /// <summary>
        /// Save Second Grade Transaction details
        /// </summary>
        /// <param name="objFinalPacking">object with transaction data</param>
        /// <returns>return result</returns>
        public static int InsertSecondGradeFinalPacking(FinalPackingDTO objFinalPacking)
        {//
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@LocationId", objFinalPacking.locationId));
            lstFP.Add(new FloorSqlParameter("@WorkStationNumber", objFinalPacking.Workstationnumber));
            lstFP.Add(new FloorSqlParameter("@PrinterName", objFinalPacking.Printername));
            lstFP.Add(new FloorSqlParameter("@PackDate", objFinalPacking.Packdate));
            lstFP.Add(new FloorSqlParameter("@OuterLotNo", objFinalPacking.Outerlotno));
            lstFP.Add(new FloorSqlParameter("@InternalLotNumber", objFinalPacking.Internallotnumber));
            lstFP.Add(new FloorSqlParameter("@PONumber", objFinalPacking.Ponumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", objFinalPacking.ItemNumber));
            lstFP.Add(new FloorSqlParameter("@Size", objFinalPacking.Size));
            lstFP.Add(new FloorSqlParameter("@GroupId", objFinalPacking.GroupId));
            lstFP.Add(new FloorSqlParameter("@BoxesPacked", objFinalPacking.Boxespacked));
            lstFP.Add(new FloorSqlParameter("@CasesPacked", objFinalPacking.Casespacked));
            lstFP.Add(new FloorSqlParameter("@InnerSetLayout", objFinalPacking.Innersetlayout));
            lstFP.Add(new FloorSqlParameter("@OuterSetLayout", objFinalPacking.Outersetlayout));
            lstFP.Add(new FloorSqlParameter("@stationNumber", objFinalPacking.stationNumber));
            lstFP.Add(new FloorSqlParameter("@ManufacturingDate", objFinalPacking.ManufacturingDate));
            lstFP.Add(new FloorSqlParameter("@ExpiryDate", objFinalPacking.ExpiryDate));
            lstFP.Add(new FloorSqlParameter("@SerialNumber", objFinalPacking.listSecondGradeSerialNumber));
            lstFP.Add(new FloorSqlParameter("@InventTransId", objFinalPacking.InventTransId));
            lstFP.Add(new FloorSqlParameter("@FPStationNo", objFinalPacking.FPStationNo)); //Added by Azman 2019-08-22 FPDetailsReport
            lstFP.Add(new FloorSqlParameter("@FGBatchOrderNo", objFinalPacking.FGBatchOrderNo));
            lstFP.Add(new FloorSqlParameter("@Resource", objFinalPacking.Resource));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_FinalPacking_SaveSecondGrade", lstFP);
        }

        /// <summary>
        /// Save Second Grade Print details
        /// </summary>
        /// <param name="qty">Total Cases</param>
        /// <param name="poNumber">Purchase Order Number</param>
        /// <param name="itemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <param name="WorkstationID">Workstation Id</param>
        /// <param name="locationId">Location Id</param>
        /// <param name="internalLotnNumber">Internal lot number</param>
        /// <returns>return result</returns>
        public static int InsertSecondGradeCaseNumbers(int cases, string poNumber, string itemNumber, string size, string WorkstationID, int locationId, string internalLotnNumber)
        {
            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();

            lstfp.Add(new FloorSqlParameter("@Quantity", cases));
            lstfp.Add(new FloorSqlParameter("@PONumber", poNumber));
            lstfp.Add(new FloorSqlParameter("@ItemNumber", itemNumber));
            lstfp.Add(new FloorSqlParameter("@Size", size));
            lstfp.Add(new FloorSqlParameter("@WorkstationID", WorkstationID));
            lstfp.Add(new FloorSqlParameter("@LocationId", locationId));
            lstfp.Add(new FloorSqlParameter("@InternalLotnNumber", internalLotnNumber));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_CaseNumbers_InsertForSecondGrade", lstfp);
        }

        public static Boolean isSecondGradeSerialNumber(decimal serialNumber, string size)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            lstFP.Add(new FloorSqlParameter("@size", size));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_Validate_SecondGradeSticker", lstFP)) == Constants.ONE ? true : false;
        }
        #endregion

        # region PrintInnerOuterLabel
        /// <summary>
        /// Get First rade PO list from AX Soline
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetFirstGradePOList()
        {
            List<DropdownDTO> list = new List<DropdownDTO>();
            using (DataTable dt = FloorDBAccess.ExecuteDataSet("usp_FP_SELECT_FirstGradePOList", new List<FloorSqlParameter>()).Tables[0].DefaultView.ToTable(true))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {

                        list = (from DataRow row in dt.Rows
                                select new DropdownDTO
                                {
                                    IDField = FloorDBAccess.GetString(row, "ItemType"),
                                    DisplayField = string.IsNullOrEmpty(FloorDBAccess.GetString(row, "customerref")) ? FloorDBAccess.GetString(row, "SalesId") : FloorDBAccess.GetString(row, "customerref") + " | " + FloorDBAccess.GetString(row, "SalesId"),
                                    CustomField = FloorDBAccess.GetString(row, "ProdStatus"),
                                }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        # region PrintInnerOuterLabelMTS
        /// <summary>
        /// #MK 01/06/2018 Add new source
        /// Get First grade PO list without SO
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetFirstGradePOMTSList()
        {
            List<DropdownDTO> list = new List<DropdownDTO>();
            using (DataTable dt = FloorDBAccess.ExecuteDataSet("usp_FP_SELECT_FirstGradePOwithoutSOList", new List<FloorSqlParameter>()).Tables[0].DefaultView.ToTable(true))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = (from DataRow row in dt.Rows
                                select new DropdownDTO
                                {
                                    IDField = FloorDBAccess.GetString(row, "BthOrderId"),
                                    DisplayField = string.IsNullOrEmpty(FloorDBAccess.GetString(row, "customerref")) ? FloorDBAccess.GetString(row, "BthOrderId") : FloorDBAccess.GetString(row, "customerref") + " | " + FloorDBAccess.GetString(row, "BthOrderId"),
                                    CustomField = FloorDBAccess.GetString(row, "ProdStatus"),
                                }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        public static List<DropdownDTO> GetFirstGradePOMTSListForReprint()
        {
            List<DropdownDTO> list = new List<DropdownDTO>();
            using (DataTable dt = FloorDBAccess.ExecuteDataSet("USP_DOT_ReprintMTS_FGBO_Selection", new List<FloorSqlParameter>()).Tables[0].DefaultView.ToTable(true))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = (from DataRow row in dt.Rows
                                select new DropdownDTO
                                {
                                    IDField = FloorDBAccess.GetString(row, "BthOrderId"),
                                    DisplayField = string.IsNullOrEmpty(FloorDBAccess.GetString(row, "customerref")) ? FloorDBAccess.GetString(row, "BthOrderId") : FloorDBAccess.GetString(row, "customerref") + " | " + FloorDBAccess.GetString(row, "BthOrderId"),
                                    CustomField = FloorDBAccess.GetString(row, "ProdStatus"),
                                }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        public static List<MadeToStockDTO> GetBOMTSDetails(string BatchOrderNo)
        {
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            List<MadeToStockDTO> lstBatchOrderMTSDetailsDTO = new List<MadeToStockDTO>();
            prmList.Add(new FloorSqlParameter("@BatchOrderNo", BatchOrderNo));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_BatchOrderMTSDetails_Get", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            MadeToStockDTO lnmtsDTO = new MadeToStockDTO();
                            lnmtsDTO.AlternateGloveCode1 = FloorDBAccess.GetString(dr, "AlternateGloveCode1");
                            lnmtsDTO.AlternateGloveCode2 = FloorDBAccess.GetString(dr, "AlternateGloveCode2");
                            lnmtsDTO.AlternateGloveCode3 = FloorDBAccess.GetString(dr, "AlternateGloveCode3");
                            lnmtsDTO.SpecialInnerCode = FloorDBAccess.GetValue<Int32>(dr, "SpecialInnerCode");
                            lnmtsDTO.SpecialInnerCharacter = FloorDBAccess.GetString(dr, "SpecialInnerCharacter");
                            lnmtsDTO.InnerDateFormat = FloorDBAccess.GetString(dr, "InnerDateFormat");
                            lnmtsDTO.OuterDateFormat = FloorDBAccess.GetString(dr, "OuterDateFormat");
                            lnmtsDTO.PrintingSize = FloorDBAccess.GetString(dr, "PrintingSize");
                            lnmtsDTO.GrossWeight = FloorDBAccess.GetValue<decimal>(dr, "GrossWeight");
                            lnmtsDTO.NetWeight = FloorDBAccess.GetValue<decimal>(dr, "NetWeight");
                            lnmtsDTO.InnerboxinCaseNo = FloorDBAccess.GetValue<Int32>(dr, "InnerboxinCaseNo");
                            lnmtsDTO.HartalegaCommonSize = FloorDBAccess.GetString(dr, "HartalegaCommonSize");
                            lnmtsDTO.GlovesInnerboxNo = FloorDBAccess.GetValue<Int32>(dr, "GlovesInnerboxNo");
                            lnmtsDTO.InnerProductCode = FloorDBAccess.GetString(dr, "InnerProductCode");
                            lnmtsDTO.OuterProductCode = FloorDBAccess.GetString(dr, "OuterProductCode");
                            lnmtsDTO.Reference1 = FloorDBAccess.GetString(dr, "Reference1");
                            lnmtsDTO.Reference2 = FloorDBAccess.GetString(dr, "Reference2");
                            lnmtsDTO.DOTCustomerLotID = FloorDBAccess.GetString(dr, "DOTCustomerLotID");
                            lnmtsDTO.MadeToStockStatus = FloorDBAccess.GetString(dr, "MadeToStockStatus");
                            lnmtsDTO.LotVerification = FloorDBAccess.GetValue<Int32>(dr, "LotVerification");
                            lnmtsDTO.PreShipmentPlan = FloorDBAccess.GetValue<Int32>(dr, "PreShipmentPlan");
                            lnmtsDTO.InnerLabelSet = FloorDBAccess.GetString(dr, "InnerLabelSet");
                            lnmtsDTO.OuterLabelSetNo = FloorDBAccess.GetString(dr, "OuterLabelSetNo");
                            lnmtsDTO.PalletCapacity = FloorDBAccess.GetValue<Int32>(dr, "PalletCapacity");
                            lnmtsDTO.ManufacturingDateOn = FloorDBAccess.GetValue<Int32>(dr, "ManufacturingDateOn");
                            lnmtsDTO.Expiry = FloorDBAccess.GetValue<Int32>(dr, "Expiry");
                            lnmtsDTO.GCLabel = FloorDBAccess.GetValue<Int32>(dr, "GCLabel");
                            lnmtsDTO.WarehouseId = FloorDBAccess.GetString(dr, "WarehouseId");
                            lstBatchOrderMTSDetailsDTO.Add(lnmtsDTO);
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
            return lstBatchOrderMTSDetailsDTO;
        }

        public static int UpdateBatcOrderMTSStatus(string batchOrderNo, int mtsStatus, int lastModifiedBy)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();

            lstfsp.Add(new FloorSqlParameter("@BatchOrderNo", batchOrderNo));
            lstfsp.Add(new FloorSqlParameter("@MTSStatus", mtsStatus));
            lstfsp.Add(new FloorSqlParameter("@LastModifiedBy", lastModifiedBy));
            return FloorDBAccess.ExecuteNonQuery("USP_DOT_BatcOrderMTS_Update", lstfsp);
        }

        #endregion
        /// <summary>
        /// Get Preshipment Pallet Id
        /// </summary>
        /// <returns>Return List of Preshipment Pallet Id</returns>
        public static List<DropdownDTO> GetPreshipmentPalletIdList()
        {
            List<DropdownDTO> list = null;
            //add by Cheah (2017-03-20)
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@locationid", WorkStationDTO.GetInstance().LocationId));
            //end add
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_FP_PreshipmetPalletID_get", lstfsp))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = (from DataRow row in dt.Rows
                                select new DropdownDTO
                                {
                                    IDField = FloorDBAccess.GetString(row, "PalletId"),
                                    DisplayField = FloorDBAccess.GetString(row, "PalletId")
                                }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.PRESHIPMENTPALLET_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }
        /// <summary>
        /// Get Pallet Id
        /// </summary>
        /// <returns>Return list of Pallet Id</returns>
        public static List<DropdownDTO> GetPalletIdList(string poNumber = "", string itemNumber = "", string size = "",
                                        bool listAllAssociatedPallet = false)
        {
            List<DropdownDTO> list = null;
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@PONumber", poNumber));
            lstfsp.Add(new FloorSqlParameter("@ItemNumber", itemNumber));
            lstfsp.Add(new FloorSqlParameter("@Size", size));
            lstfsp.Add(new FloorSqlParameter("@locationid", WorkStationDTO.GetInstance().LocationId));
            lstfsp.Add(new FloorSqlParameter("@ListAllAssociatedPallet", listAllAssociatedPallet));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_FP_PalletID_get", lstfsp))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = (from DataRow row in dt.Rows
                                select new DropdownDTO
                                {
                                    IDField = FloorDBAccess.GetString(row, "PalletId"),
                                    DisplayField = FloorDBAccess.GetString(row, "PalletId")
                                }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.PALLET_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }
        /// <summary>
        /// To get the preshipment pallet details for Ewaenavi
        /// </summary>
        /// <param name="palletid"></param>
        /// <returns></returns>
        public static List<SOLineDTO> GetPreshipmentPalletEwarenaviTextFile(string palletid, string ponumber, string itemName)
        {
            List<SOLineDTO> list = new List<SOLineDTO>();
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@PalletId", palletid));
            lstfsp.Add(new FloorSqlParameter("@PONumber", ponumber));
            lstfsp.Add(new FloorSqlParameter("@ItemName", itemName));
            using (DataTable dtPreshipmentpalletDetails = FloorDBAccess.ExecuteDataTable("usp_FP_GetPreshipmentPalletEwarenavi", lstfsp))
            {
                try
                {
                    if (dtPreshipmentpalletDetails != null && dtPreshipmentpalletDetails.Rows.Count != 0)
                    {
                        foreach (DataRow row in dtPreshipmentpalletDetails.Rows)
                        {
                            SOLineDTO objSo = new SOLineDTO();
                            objSo.PONumber = FloorDBAccess.GetString(row, "PONumber");
                            objSo.ItemNumber = FloorDBAccess.GetString(row, "ItemNumber");
                            objSo.ItemSize = FloorDBAccess.GetString(row, "size");
                            objSo.CaseCapacity = FloorDBAccess.GetValue<int>(row, "CaseCount");
                            list.Add(objSo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.PALLET_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }
        /// <summary>
        /// To get the preshipment pallet details with customer size for Ewaenavi
        /// </summary>
        /// <param name="palletid"></param>
        /// <param name="ponumber"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static List<SOLineDTO> GetPreshipmentPalletEwarenaviTextFileWithCustomerSize(string palletid, string ponumber, string itemName)
        {
            List<SOLineDTO> list = new List<SOLineDTO>();
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@PalletId", palletid));
            lstfsp.Add(new FloorSqlParameter("@PONumber", ponumber));
            lstfsp.Add(new FloorSqlParameter("@ItemName", itemName));
            using (DataTable dtPreshipmentpalletDetails = FloorDBAccess.ExecuteDataTable("usp_FP_GetPreshipmentPalletEwarenavi_CustomerSize", lstfsp))
            {
                try
                {
                    if (dtPreshipmentpalletDetails != null && dtPreshipmentpalletDetails.Rows.Count != 0)
                    {
                        foreach (DataRow row in dtPreshipmentpalletDetails.Rows)
                        {
                            SOLineDTO objSo = new SOLineDTO();
                            objSo.PONumber = FloorDBAccess.GetString(row, "PONumber");
                            objSo.ItemNumber = FloorDBAccess.GetString(row, "ItemNumber");
                            objSo.ItemSize = FloorDBAccess.GetString(row, "ItemSize"); // for 9 dots new field: EWN_CompletedPallet.FGCodeAndSize
                            objSo.CustomerSize = FloorDBAccess.GetString(row, "size");
                            objSo.CaseCapacity = FloorDBAccess.GetValue<int>(row, "CaseCount");
                            list.Add(objSo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.PALLET_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }
        //public static List<string> GetPalletIdListstring()
        //{
        //    List<string> list = null;
        //    using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_FP_PalletID_get"))
        //    {
        //        try
        //        {
        //            if (dt != null && dt.Rows.Count != 0)
        //            {

        //                list =
        //                    (from DataRow row in dt.Rows
        //                     select FloorDBAccess.GetString(row, "PalletId")
        //                        ).ToList();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new FloorSystemException(Messages.PALLET_EXCEPTION, Constants.BUSINESSLOGIC, ex);
        //        }
        //    }
        //    return list;
        //}

        /// <summary>
        /// Get Purchase Order List
        /// </summary>
        /// <returns>return list of Available orders</returns>
        public static List<SOLineDTO> GetPurchaseOrderList(string PONumber, string itemType)
        {
            List<SOLineDTO> lstSOLine = new List<SOLineDTO>();
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@PONumber", PONumber));
            lstfsp.Add(new FloorSqlParameter("@ItemType", itemType));

            using (DataTable dtPOlist = FloorDBAccess.ExecuteDataTable("usp_FP_PurchaseOrder_Get", lstfsp))
            {
                try
                {
                    if (dtPOlist != null && dtPOlist.Rows.Count != 0)
                    {
                        foreach (DataRow row in dtPOlist.Rows)
                        {
                            SOLineDTO objSo = new SOLineDTO();
                            objSo.PONumber = FloorDBAccess.GetString(row, "PONumber");
                            objSo.BarcodeVerificationRequired = FloorDBAccess.GetValue<int>(row, "BarcodeVerificationRequired");
                            objSo.PreshipmentPlan = FloorDBAccess.GetValue<int>(row, "PreshipmentPlan");
                            objSo.ItemNumber = FloorDBAccess.GetString(row, "ItemNumber");
                            objSo.ItemNumberDisplay = string.Format("{0}{1}{2}", FloorDBAccess.GetString(row, "ItemNumber"), Constants.UNDERSCORE, FloorDBAccess.GetString(row, "ItemSize"));
                            objSo.ItemSize = FloorDBAccess.GetString(row, "ItemSize");
                            objSo.GloveCode = FloorDBAccess.GetString(row, "GloveCode");
                            objSo.ItemCases = (int)FloorDBAccess.GetValue<decimal>(row, "ItemCases");
                            objSo.InnersetLayout = FloorDBAccess.GetString(row, "InnersetLayout");
                            objSo.OuterSetLayout = FloorDBAccess.GetString(row, "OuterSetLayout");
                            objSo.InnerLabelSetDateFormat = FloorDBAccess.GetString(row, "InnerLabelSetDateFormat"); // yiksiu SEP 2017: Label Set Optimization 
                            objSo.OuterLabelSetDateFormat = FloorDBAccess.GetString(row, "OuterLabelSetDateFormat"); // yiksiu SEP 2017: Label Set Optimization
                            objSo.CustomerSize = FloorDBAccess.GetString(row, "CustomerSize");
                            objSo.CustomerSizeDesc = FloorDBAccess.GetString(row, "CustomerSizeDesc");
                            objSo.GrossWeight = FloorDBAccess.GetValue<decimal>(row, "GrossWeight");
                            objSo.NettWeight = FloorDBAccess.GetValue<decimal>(row, "NettWeight");
                            objSo.CaseCapacity = FloorDBAccess.GetValue<int>(row, "CaseCapacity");
                            objSo.PalletCapacity = FloorDBAccess.GetValue<int>(row, "PalletCapacity");
                            objSo.InnerBoxCapacity = FloorDBAccess.GetValue<int>(row, "InnerBoxCapacity");
                            objSo.CustomerLotNumber = FloorDBAccess.GetString(row, "CustomerLotNumber");
                            objSo.CustomerRefernceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber");
                            objSo.ItemName = FloorDBAccess.GetString(row, "ItemName");
                            objSo.ManufacturingDateBasis = FloorDBAccess.GetValue<int>(row, "MANUFACTURINGDATEBASIS");
                            objSo.CustomerName = FloorDBAccess.GetString(row, "CustomerName");
                            objSo.POStatus = FloorDBAccess.GetValue<int>(row, "POStatus");
                            objSo.InnerProductCode = FloorDBAccess.GetString(row, "InnerProductCode");
                            objSo.OuterProductCode = FloorDBAccess.GetString(row, "OuterProductCode");
                            // objSo.BrandName = FloorDBAccess.GetString(row, "BrandName");
                            objSo.Expiry = FloorDBAccess.GetValue<int>(row, "Expiry");
                            objSo.Reference1 = FloorDBAccess.GetString(row, "ProductReferenceNumber");
                            objSo.SpecialInnerCode = FloorDBAccess.GetValue<int>(row, "SpecialInnerCode");
                            objSo.SpecialInnerCodeCharacter = FloorDBAccess.GetString(row, "SpecialInnerCodeCharacter");
                            objSo.GCLabelPrintingRequired = FloorDBAccess.GetValue<int>(row, "GCLabelPrintingRequired");
                            objSo.AlternateGloveCode1 = FloorDBAccess.GetString(row, "AlternateGloveCode1");
                            objSo.AlternateGloveCode2 = FloorDBAccess.GetString(row, "AlternateGloveCode2");
                            objSo.AlternateGloveCode3 = FloorDBAccess.GetString(row, "AlternateGloveCode3");
                            objSo.RECEIPTDATEREQUESTED = FloorDBAccess.GetValue<DateTime>(row, "RECEIPTDATEREQUESTED");
                            objSo.SHIPPINGDATEREQUESTED = FloorDBAccess.GetValue<DateTime>(row, "SHIPPINGDATEREQUESTED");
                            //objSo.ShippingDateETD = FloorDBAccess.GetValue<DateTime>(row, "STShippingDateConfirmed");
                            //objSo.ManufacturingDateETD = FloorDBAccess.GetValue<DateTime>(row, "ManufacturingDateETD");

                            //KahHeng 29jan2019 added 2 new parameter for PODate  POReceviedDate 
                            objSo.CustPODocumentDate = FloorDBAccess.GetValue<DateTime>(row, "HSB_CustPODocumentDate");
                            objSo.CustPORecvDate = FloorDBAccess.GetValue<DateTime>(row, "HSB_CustPORecvDate");
                            //KahHeng End


                            objSo.ItemType = FloorDBAccess.GetValue<int>(row, "ItemType");
                            objSo.OrderNumber = FloorDBAccess.GetString(row, "OrderNumber");
                            objSo.Reference2 = FloorDBAccess.GetString(row, "Reference2");
                            objSo.InventTransId = FloorDBAccess.GetString(row, "INVENTTRANSID");
                            //objSo.BatchOrder = FloorDBAccess.GetString(row, "BatchOrder"); //Max He, no more using
                            objSo.BatchOrder = FloorDBAccess.GetString(row, "BatchOrder"); //Azrul, use for 2FG
                            objSo.ProdStatus = FloorDBAccess.GetString(row, "ProdStatus");

                            //objSo.BARCODE = FloorDBAccess.GetString(row, "BARCODE");
                            //objSo.BARCODEOUTERBOX = FloorDBAccess.GetString(row, "BARCODEOUTERBOX");
                            //objSo.VSReceiptFilePath = FloorDBAccess.GetString(row, "VisionURL");

                            lstSOLine.Add(objSo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                }
                return lstSOLine;
            }
        }

        /// <summary>
        /// #MK 02/06/2018
        /// Get Purchase Order List for Make to Stock
        /// </summary>
        /// <returns>return list of Available orders</returns>
        public static List<SOLineDTO> GetPurchaseOrderMTSList(string BONumber)
        {
            List<SOLineDTO> lstSOLine = new List<SOLineDTO>();
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@BONumber", BONumber));

            using (DataTable dtPOlist = FloorDBAccess.ExecuteDataTable("usp_FP_PurchaseOrderMTS_Get", lstfsp))
            {
                try
                {
                    if (dtPOlist != null && dtPOlist.Rows.Count != 0)
                    {
                        foreach (DataRow row in dtPOlist.Rows)
                        {
                            SOLineDTO objSo = new SOLineDTO();
                            objSo.PONumber = FloorDBAccess.GetString(row, "PONumber");
                            objSo.BarcodeVerificationRequired = FloorDBAccess.GetValue<int>(row, "BarcodeVerificationRequired");
                            objSo.PreshipmentPlan = FloorDBAccess.GetValue<int>(row, "PreshipmentPlan");
                            objSo.ItemNumber = FloorDBAccess.GetString(row, "ItemNumber");
                            objSo.ItemNumberDisplay = string.Format("{0}{1}{2}", FloorDBAccess.GetString(row, "ItemNumber"), Constants.UNDERSCORE, FloorDBAccess.GetString(row, "ItemSize"));
                            objSo.ItemSize = FloorDBAccess.GetString(row, "ItemSize");
                            objSo.GloveCode = FloorDBAccess.GetString(row, "GloveCode");
                            objSo.ItemCases = Convert.ToInt32(FloorDBAccess.GetValue<decimal>(row, "ItemCases"));
                            objSo.InnersetLayout = FloorDBAccess.GetString(row, "InnersetLayout");
                            objSo.OuterSetLayout = FloorDBAccess.GetString(row, "OuterSetLayout");
                            objSo.CustomerSize = FloorDBAccess.GetString(row, "CustomerSize");
                            objSo.CustomerSizeDesc = FloorDBAccess.GetString(row, "CustomerSizeDesc");
                            objSo.GrossWeight = FloorDBAccess.GetValue<decimal>(row, "GrossWeight");
                            objSo.NettWeight = FloorDBAccess.GetValue<decimal>(row, "NettWeight");
                            objSo.CaseCapacity = FloorDBAccess.GetValue<int>(row, "CaseCapacity");
                            objSo.PalletCapacity = FloorDBAccess.GetValue<int>(row, "PalletCapacity");
                            objSo.InnerBoxCapacity = FloorDBAccess.GetValue<int>(row, "InnerBoxCapacity");
                            objSo.CustomerLotNumber = FloorDBAccess.GetString(row, "CustomerLotNumber");
                            objSo.CustomerRefernceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber");
                            objSo.ItemName = FloorDBAccess.GetString(row, "ItemName");
                            objSo.ManufacturingDateBasis = FloorDBAccess.GetValue<int>(row, "MANUFACTURINGDATEBASIS");
                            objSo.CustomerName = FloorDBAccess.GetString(row, "CustomerName");
                            objSo.POStatus = FloorDBAccess.GetValue<int>(row, "POStatus");
                            objSo.InnerProductCode = FloorDBAccess.GetString(row, "InnerProductCode");
                            objSo.OuterProductCode = FloorDBAccess.GetString(row, "OuterProductCode");
                            // objSo.BrandName = FloorDBAccess.GetString(row, "BrandName");
                            objSo.Expiry = FloorDBAccess.GetValue<int>(row, "Expiry");
                            objSo.Reference1 = FloorDBAccess.GetString(row, "ProductReferenceNumber");
                            objSo.SpecialInnerCode = FloorDBAccess.GetValue<int>(row, "SpecialInnerCode");
                            objSo.SpecialInnerCodeCharacter = FloorDBAccess.GetString(row, "SpecialInnerCodeCharacter");
                            objSo.GCLabelPrintingRequired = FloorDBAccess.GetValue<int>(row, "GCLabelPrintingRequired");
                            objSo.AlternateGloveCode1 = FloorDBAccess.GetString(row, "AlternateGloveCode1");
                            objSo.AlternateGloveCode2 = FloorDBAccess.GetString(row, "AlternateGloveCode2");
                            objSo.AlternateGloveCode3 = FloorDBAccess.GetString(row, "AlternateGloveCode3");
                            objSo.RECEIPTDATEREQUESTED = FloorDBAccess.GetValue<DateTime>(row, "RECEIPTDATEREQUESTED");
                            objSo.SHIPPINGDATEREQUESTED = FloorDBAccess.GetValue<DateTime>(row, "SHIPPINGDATEREQUESTED");
                            objSo.ItemType = FloorDBAccess.GetValue<int>(row, "ItemType");
                            objSo.OrderNumber = FloorDBAccess.GetString(row, "OrderNumber");
                            objSo.Reference2 = FloorDBAccess.GetString(row, "Reference2");
                            objSo.InventTransId = FloorDBAccess.GetString(row, "INVENTTRANSID");
                            objSo.BatchOrder = FloorDBAccess.GetString(row, "BatchOrder");
                            objSo.ProdStatus = FloorDBAccess.GetString(row, "ProdStatus");
                            lstSOLine.Add(objSo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                }
                return lstSOLine;
            }
        }

        /// <summary>
        /// To get GC inner 
        /// </summary>
        /// <param name="internalLotnumber"></param>
        /// <returns></returns>
        public static FinalPackingDTO GetGCLabelInner(string internalLotnumber)
        {
            FloorSqlParameter fsp = new FloorSqlParameter("@InternalLotNumber", internalLotnumber);
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(fsp);
            FinalPackingDTO gcLabelInnerData = new FinalPackingDTO();
            using (DataTable dtPOlist = FloorDBAccess.ExecuteDataTable("usp_FP_Get_GCLabelInner", lstfsp))
            {
                DataRow row = dtPOlist.Rows[0];
                gcLabelInnerData.Size = FloorDBAccess.GetString(row, "size");
                gcLabelInnerData.ExpiryDate = FloorDBAccess.GetValue<DateTime>(row, "expirydate");
                gcLabelInnerData.customerLotNumber = FloorDBAccess.GetString(row, "customerLotNumber");
                gcLabelInnerData.Boxespacked = FloorDBAccess.GetValue<int>(row, "boxesPacked");
                gcLabelInnerData.Casespacked = FloorDBAccess.GetValue<int>(row, "CasesPacked");
            }
            return gcLabelInnerData;
        }


        /// <summary>
        /// Purchase Order Item Details
        /// </summary>
        /// <param name="poNumber">Purchase Order Number</param>
        /// <returns>list of Purchase order Items</returns>
        public static List<SOLineDTO> GetPurchaseOrderItemList(string poNumber)
        {
            FloorSqlParameter fsp = new FloorSqlParameter("@PONumber", poNumber);
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(fsp);
            List<SOLineDTO> lstPODetails = null;
            try
            {
                using (DataTable dtPOlist = FloorDBAccess.ExecuteDataTable("usp_FP_POItem_Get", lstfsp))
                {
                    if (dtPOlist != null && dtPOlist.Rows.Count != 0)
                    {
                        lstPODetails = (from DataRow row in dtPOlist.Rows
                                        select new SOLineDTO
                                        {
                                            PONumber = FloorDBAccess.GetString(row, "PONumber"),
                                            PreshipmentPlan = FloorDBAccess.GetValue<int>(row, "PreshipmentPlan"),
                                            ItemNumber = FloorDBAccess.GetString(row, "ItemNumber"),
                                            ItemName = FloorDBAccess.GetString(row, "ItemName"),
                                            ItemSize = FloorDBAccess.GetString(row, "ItemSize"),
                                            ItemCases = FloorDBAccess.GetValue<int>(row, "ItemCases"),
                                            ItemType = FloorDBAccess.GetValue<int>(row, "ITEMTYPE")
                                        }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
            return lstPODetails;
        }
        /// <summary>
        /// Insert Purchase order details for selected PO Item
        /// </summary>
        /// <param name="objPurchaseOrderItemDTO">Purchase Order DTO object</param>
        /// <returns>Return rows affected</returns>
        public static int InsertPurchaseOrderDetails(SOLineDTO objPurchaseOrderItemDTO)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@PONumber", objPurchaseOrderItemDTO.PONumber));
            lstfsp.Add(new FloorSqlParameter("@BarcodeVerificationRequired", objPurchaseOrderItemDTO.BarcodeVerificationRequired));
            lstfsp.Add(new FloorSqlParameter("@GloveCode", objPurchaseOrderItemDTO.GloveCode));
            lstfsp.Add(new FloorSqlParameter("@PreshipmentPlan", objPurchaseOrderItemDTO.PreshipmentPlan));
            lstfsp.Add(new FloorSqlParameter("@ItemNumber", objPurchaseOrderItemDTO.ItemNumber));
            lstfsp.Add(new FloorSqlParameter("@ItemName", objPurchaseOrderItemDTO.ItemName));
            lstfsp.Add(new FloorSqlParameter("@ItemSize", objPurchaseOrderItemDTO.ItemSize));
            lstfsp.Add(new FloorSqlParameter("@ItemCases", objPurchaseOrderItemDTO.ItemCases));
            lstfsp.Add(new FloorSqlParameter("@InnersetLayout", objPurchaseOrderItemDTO.InnersetLayout));
            lstfsp.Add(new FloorSqlParameter("@OuterSetLayout", objPurchaseOrderItemDTO.OuterSetLayout));
            lstfsp.Add(new FloorSqlParameter("@InnerLabelSetDateFormat", objPurchaseOrderItemDTO.InnerLabelSetDateFormat)); // Label Set Optimization
            lstfsp.Add(new FloorSqlParameter("@OuterLabelSetDateFormat", objPurchaseOrderItemDTO.OuterLabelSetDateFormat)); // Label Set Optimization
            lstfsp.Add(new FloorSqlParameter("@CustomerSize", objPurchaseOrderItemDTO.CustomerSize));
            lstfsp.Add(new FloorSqlParameter("@CustomerSizeDesc", objPurchaseOrderItemDTO.CustomerSizeDesc));
            lstfsp.Add(new FloorSqlParameter("@GrossWeight", objPurchaseOrderItemDTO.GrossWeight));
            lstfsp.Add(new FloorSqlParameter("@NettWeight", objPurchaseOrderItemDTO.NettWeight));
            lstfsp.Add(new FloorSqlParameter("@CaseCapacity", objPurchaseOrderItemDTO.CaseCapacity));
            lstfsp.Add(new FloorSqlParameter("@PalletCapacity", objPurchaseOrderItemDTO.PalletCapacity));
            lstfsp.Add(new FloorSqlParameter("@InnerBoxCapacity", objPurchaseOrderItemDTO.InnerBoxCapacity));
            lstfsp.Add(new FloorSqlParameter("@CustomerLotNumber", objPurchaseOrderItemDTO.CustomerLotNumber));
            lstfsp.Add(new FloorSqlParameter("@MANUFACTURINGDATEBASIS", objPurchaseOrderItemDTO.ManufacturingDateBasis));
            lstfsp.Add(new FloorSqlParameter("@POStatus", objPurchaseOrderItemDTO.POStatus));
            lstfsp.Add(new FloorSqlParameter("@InnerProductCode", objPurchaseOrderItemDTO.InnerProductCode));
            lstfsp.Add(new FloorSqlParameter("@OuterProductCode", objPurchaseOrderItemDTO.OuterProductCode));
            lstfsp.Add(new FloorSqlParameter("@BrandName", objPurchaseOrderItemDTO.BrandName));
            lstfsp.Add(new FloorSqlParameter("@Expiry", objPurchaseOrderItemDTO.Expiry));
            lstfsp.Add(new FloorSqlParameter("@ProductReferenceNumber", objPurchaseOrderItemDTO.Reference1));
            lstfsp.Add(new FloorSqlParameter("@SpecialInnerCode", objPurchaseOrderItemDTO.SpecialInnerCode));
            lstfsp.Add(new FloorSqlParameter("@SpecialInnerCodeCharacter", objPurchaseOrderItemDTO.SpecialInnerCodeCharacter));
            lstfsp.Add(new FloorSqlParameter("@GCLabelPrintingRequired", objPurchaseOrderItemDTO.GCLabelPrintingRequired));
            lstfsp.Add(new FloorSqlParameter("@AlternateGloveCode1", objPurchaseOrderItemDTO.AlternateGloveCode1));
            lstfsp.Add(new FloorSqlParameter("@AlternateGloveCode2", objPurchaseOrderItemDTO.AlternateGloveCode2));
            lstfsp.Add(new FloorSqlParameter("@AlternateGloveCode3", objPurchaseOrderItemDTO.AlternateGloveCode3));
            lstfsp.Add(new FloorSqlParameter("@CustomerName", objPurchaseOrderItemDTO.CustomerName));
            lstfsp.Add(new FloorSqlParameter("@CustomerRefNum", objPurchaseOrderItemDTO.CustomerRefernceNumber));
            lstfsp.Add(new FloorSqlParameter("@LocationId", objPurchaseOrderItemDTO.locationID));
            lstfsp.Add(new FloorSqlParameter("@ItemType", objPurchaseOrderItemDTO.ItemType));
            lstfsp.Add(new FloorSqlParameter("@PreshipmentCases", objPurchaseOrderItemDTO.Preshipmentcases));
            lstfsp.Add(new FloorSqlParameter("@OrderNumber", objPurchaseOrderItemDTO.OrderNumber));
            lstfsp.Add(new FloorSqlParameter("@Reference2", objPurchaseOrderItemDTO.Reference2));
            lstfsp.Add(new FloorSqlParameter("@InventTransId", objPurchaseOrderItemDTO.InventTransId));
            lstfsp.Add(new FloorSqlParameter("@ShippingDateRequested", objPurchaseOrderItemDTO.SHIPPINGDATEREQUESTED));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_UpdatePurchaseOrder_Insert", lstfsp);
        }
        /// <summary>
        /// To return the Workstation Last internalLot Running number
        /// </summary>
        /// <param name="workStation">Packing Station ID</param>
        /// <returns>return Workstation Last running number</returns>
        public static int GetWorkStationLastRunningNumber(string workStation)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            DataRow dr = null;
            try
            {
                lstfsp.Add(new FloorSqlParameter("@workStationNumber", workStation));
                DataTable dtlastRunningNum = FloorDBAccess.ExecuteDataTable("usp_FP_WorkStationNumber_Get", lstfsp);
                if (dtlastRunningNum.Rows.Count > 0)
                {
                    dr = dtlastRunningNum.Rows[0];
                    DateTime lastModifiedLotNumnerdate = FloorDBAccess.GetValue<DateTime>(dr, "LastModifiedOn");
                    if (lastModifiedLotNumnerdate.ToShortDateString() == CommonBLL.GetCurrentDateAndTimeFromServer().ToShortDateString())
                        return FloorDBAccess.GetValue<int>(dr, "LastRunningLotNumber");
                    else
                    {
                        UpdateWorkStationLastRunningNumber(workStation, Constants.ONE);
                        return Constants.ONE;
                    }
                }
                else
                    return Constants.ONE;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.WORKSTATION_LASTRUNNINGNUMBER, Constants.BUSINESSLOGIC, ex);
            }
        }
        /// <summary>
        /// To update workstation last running number
        /// </summary>
        /// <param name="workStation">Workstation Number</param>
        /// <param name="LastRunningNumber">Last Running Number</param>
        /// <param name="OperatorId">Operator Id</param>
        /// <returns>Return rows affected</returns>
        public static int UpdateWorkStationLastRunningNumber(string workStation, int LastRunningNumber)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();

            lstfsp.Add(new FloorSqlParameter("@workStationNumber", workStation));
            lstfsp.Add(new FloorSqlParameter("@LastrunningNum", LastRunningNumber));
            //lstfsp.Add(new FloorSqlParameter("@OperatorId", OperatorId));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_WorkStationNumber_Update", lstfsp);
        }
        /// <summary>
        /// To get preshipment case quantity from Preshipment sampling paln
        /// </summary>
        /// <param name="qty">total Item cases</param>
        /// <param name="preshipmentPlan">Preshipment plan given for the selected Purchase Order</param>
        /// <returns>returns preshipment Case Quantity</returns>
        public static int GetPreshipmentCaseQty(int qty, int preshipmentPlan)
        {

            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();
            try
            {
                lstfp.Add(new FloorSqlParameter("@PlanValue", preshipmentPlan));
                lstfp.Add(new FloorSqlParameter("@cty", qty));
                object objPreshipmentCases = FloorDBAccess.ExecuteScalar("usp_FP_PreshipmentCaseQty_Get", lstfp);
                return Convert.IsDBNull(objPreshipmentCases) ? 0 : Convert.ToInt32(objPreshipmentCases);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.PRESHIPMENT_CASENUMBERS, Constants.BUSINESSLOGIC, ex);
            }
        }
        /// <summary>
        /// Return true if preshipmentQA already sent
        /// </summary>
        /// <param name="poNumber">poNumber</param>
        /// <returns>return true if preshipment QA Email sent</returns>
        public static Boolean isPreshipmentQAEmailSent(string poNumber)
        {
            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();
            try
            {
                lstfp.Add(new FloorSqlParameter("@PONumber", poNumber));
                return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("usp_FP_isPreshipmentQAEmailSent", lstfp));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.PRESHIPMENT_CASENUMBERS, Constants.BUSINESSLOGIC, ex);
            }
        }
        /// <summary>
        /// Update isPreshipmentQAEmailSent for POnumber
        /// </summary>
        /// <param name="poNumber">poNumber</param>
        /// <returns>returns the rows affected</returns>
        public static int UpdatePresshipmentQAEmailSent(string poNumber)
        {
            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();
            lstfp.Add(new FloorSqlParameter("@PONumber", poNumber));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_PreshipmentQAEmailSent", lstfp);
        }
        /// <summary>
        /// Get Batch Card Size
        /// </summary>
        /// <param name="serialnumber">serialnumber</param>
        /// <returns>batch card size</returns>
        public static string GetBatchCardSize(decimal serialnumber)
        {
            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();
            try
            {
                lstfp.Add(new FloorSqlParameter("@serialnumber", serialnumber));
                return Convert.ToString(FloorDBAccess.ExecuteScalar("usp_FP_GetBatchCardSize", lstfp));

            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.PRESHIPMENT_CASENUMBERS, Constants.BUSINESSLOGIC, ex);
            }
        }
        /// <summary>
        /// Insert purchase order item cases for the selected PO item
        /// </summary>
        /// <param name="qty">cases packed</param>
        /// <param name="poNumber">Pourchase order</param>
        /// <param name="itemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <param name="preshipmentcases">preshipment cases</param>
        /// <returns>return rows inserted</returns>
        public static int InsertCaseNumbers(int qty, string poNumber, string itemNumber, string size, string preshipmentcases, string WorkstationID, int locationId, string InventTransId, string CustomerSize)
        {
            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();

            lstfp.Add(new FloorSqlParameter("@Quantity", qty));
            lstfp.Add(new FloorSqlParameter("@PONumber", poNumber));
            lstfp.Add(new FloorSqlParameter("@ItemNumber", itemNumber));
            lstfp.Add(new FloorSqlParameter("@Size", size));
            lstfp.Add(new FloorSqlParameter("@preshipmentCases", preshipmentcases));
            lstfp.Add(new FloorSqlParameter("@WorkstationID", WorkstationID));
            lstfp.Add(new FloorSqlParameter("@LocationId", locationId));
            lstfp.Add(new FloorSqlParameter("@InventTransId", InventTransId));
            lstfp.Add(new FloorSqlParameter("@CustomerSize", CustomerSize));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_CaseNumbers_Insert", lstfp);

        }


        /// <summary>
        /// Final Packing Trasaction method
        /// </summary>
        /// <param name="objFinalPacking">FinalPackign DTO object</param>
        /// <returns>returns rows inserted</returns>
        public static int InsertFinalPacking(FinalPackingDTO objFinalPacking, string BatchInfo = null)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@LocationId", objFinalPacking.locationId));
            lstFP.Add(new FloorSqlParameter("@WorkStationNumber", objFinalPacking.Workstationnumber));
            lstFP.Add(new FloorSqlParameter("@PrinterName", objFinalPacking.Printername));
            lstFP.Add(new FloorSqlParameter("@PackDate", objFinalPacking.Packdate));
            lstFP.Add(new FloorSqlParameter("@OuterLotNo", objFinalPacking.Outerlotno));
            lstFP.Add(new FloorSqlParameter("@InternalLotNumber", objFinalPacking.Internallotnumber));
            lstFP.Add(new FloorSqlParameter("@PONumber", objFinalPacking.Ponumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", objFinalPacking.ItemNumber));
            lstFP.Add(new FloorSqlParameter("@Size", objFinalPacking.Size));
            //lstFP.Add(new FloorSqlParameter("@GroupId", objFinalPacking.GroupId));
            lstFP.Add(new FloorSqlParameter("@SerialNumber", objFinalPacking.Serialnumber));
            lstFP.Add(new FloorSqlParameter("@BoxesPacked", objFinalPacking.Boxespacked));
            lstFP.Add(new FloorSqlParameter("@PalletId", objFinalPacking.Palletid));
            lstFP.Add(new FloorSqlParameter("@CasesPacked", objFinalPacking.Casespacked));
            if (!string.IsNullOrEmpty(objFinalPacking.PreshipmentPLTId))
                lstFP.Add(new FloorSqlParameter("@PreShipmentPalletId", objFinalPacking.PreshipmentPLTId));
            lstFP.Add(new FloorSqlParameter("@PreshipmentCasesPacked", objFinalPacking.Preshipmentcases));
            // lstFP.Add(new FloorSqlParameter("@OperatorId", objFinalPacking.OperatorId));
            lstFP.Add(new FloorSqlParameter("@InnerSetLayout", objFinalPacking.Innersetlayout));
            lstFP.Add(new FloorSqlParameter("@OuterSetLayout", objFinalPacking.Outersetlayout));
            lstFP.Add(new FloorSqlParameter("@palletCapacity", objFinalPacking.palletCapacity));
            lstFP.Add(new FloorSqlParameter("@TotalPcs", objFinalPacking.TotalPcs));
            lstFP.Add(new FloorSqlParameter("@isTempPack", objFinalPacking.isTempPack));
            if (!string.IsNullOrEmpty(BatchInfo))
                lstFP.Add(new FloorSqlParameter("@strXML", BatchInfo));
            lstFP.Add(new FloorSqlParameter("@ManufacturingDate", objFinalPacking.ManufacturingDate));
            lstFP.Add(new FloorSqlParameter("@ExpiryDate", objFinalPacking.ExpiryDate));
            lstFP.Add(new FloorSqlParameter("@stationNumber", objFinalPacking.stationNumber));
            lstFP.Add(new FloorSqlParameter("@InventTransId", objFinalPacking.InventTransId));
            lstFP.Add(new FloorSqlParameter("@FPStationNo", objFinalPacking.FPStationNo)); //Added by Azman 2019-08-22 FPDetailsReport
            lstFP.Add(new FloorSqlParameter("@FGBatchOrderNo", objFinalPacking.FGBatchOrderNo));
            lstFP.Add(new FloorSqlParameter("@Resource", objFinalPacking.Resource));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_FinalPacking_Save", lstFP);
        }
        /// <summary>
        /// get Pallet count
        /// </summary>
        /// <param name="internallotnumber"></param>
        /// <returns></returns>
        public static int GetPalletIdCount(string internallotnumber)
        {
            //usp_FP_POSizeBoxesPacked_Validate
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@internaLotNumber", internallotnumber));
                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_GetPalletIDForInternalLotnumber", lstFP));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }
        /// <summary>
        /// Get Preshipment pallet Count
        /// </summary>
        /// <param name="internallotnumber"></param>
        /// <returns></returns>
        public static int GetPreshipmentPalletIdCount(string internallotnumber)
        {
            //usp_FP_POSizeBoxesPacked_Validate
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@internaLotNumber", internallotnumber));
                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_GetPreshipmentPalletIDForInternalLotnumber", lstFP));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objFinalPacking"></param>
        /// <param name="BatchInfo"></param>
        /// <returns></returns>
        public static int InsertMultiScanFinalPacking(FinalPackingDTO objFinalPacking, string BatchInfo = null)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@LocationId", objFinalPacking.locationId));
            lstFP.Add(new FloorSqlParameter("@WorkStationNumber", objFinalPacking.Workstationnumber));
            lstFP.Add(new FloorSqlParameter("@PrinterName", objFinalPacking.Printername));
            lstFP.Add(new FloorSqlParameter("@PackDate", objFinalPacking.Packdate));
            lstFP.Add(new FloorSqlParameter("@OuterLotNo", objFinalPacking.Outerlotno));
            lstFP.Add(new FloorSqlParameter("@InternalLotNumber", objFinalPacking.Internallotnumber));
            lstFP.Add(new FloorSqlParameter("@PONumber", objFinalPacking.Ponumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", objFinalPacking.ItemNumber));
            lstFP.Add(new FloorSqlParameter("@Size", objFinalPacking.Size));
            lstFP.Add(new FloorSqlParameter("@GroupId", objFinalPacking.GroupId));
            //lstFP.Add(new FloorSqlParameter("@SerialNumber", string.Empty)); //objFinalPacking.Serialnumber));
            lstFP.Add(new FloorSqlParameter("@BoxesPacked", objFinalPacking.Boxespacked));
            lstFP.Add(new FloorSqlParameter("@PalletId", objFinalPacking.Palletid));
            lstFP.Add(new FloorSqlParameter("@CasesPacked", objFinalPacking.Casespacked));
            lstFP.Add(new FloorSqlParameter("@PreShipmentPalletId", objFinalPacking.PreshipmentPLTId));
            lstFP.Add(new FloorSqlParameter("@PreshipmentCasesPacked", objFinalPacking.Preshipmentcases));
            // lstFP.Add(new FloorSqlParameter("@OperatorId", objFinalPacking.OperatorId));
            lstFP.Add(new FloorSqlParameter("@InnerSetLayout", objFinalPacking.Innersetlayout));
            lstFP.Add(new FloorSqlParameter("@OuterSetLayout", objFinalPacking.Outersetlayout));
            lstFP.Add(new FloorSqlParameter("@palletCapacity", objFinalPacking.palletCapacity));
            lstFP.Add(new FloorSqlParameter("@TotalPcs", objFinalPacking.TotalPcs));
            //lstFP.Add(new FloorSqlParameter("@isTempPack", objFinalPacking.isTempPack));
            if (!string.IsNullOrEmpty(BatchInfo))
                lstFP.Add(new FloorSqlParameter("@strXML", BatchInfo));
            lstFP.Add(new FloorSqlParameter("@ManufacturingDate", objFinalPacking.ManufacturingDate));
            lstFP.Add(new FloorSqlParameter("@ExpiryDate", objFinalPacking.ExpiryDate));
            lstFP.Add(new FloorSqlParameter("@stationNumber", objFinalPacking.stationNumber));
            lstFP.Add(new FloorSqlParameter("@InventTransId", objFinalPacking.InventTransId));
            lstFP.Add(new FloorSqlParameter("@FPStationNo", objFinalPacking.FPStationNo)); //Added by Azman 2019-08-22 FPDetailsReport
            lstFP.Add(new FloorSqlParameter("@FGBatchOrderNo", objFinalPacking.FGBatchOrderNo));
            lstFP.Add(new FloorSqlParameter("@Resource", objFinalPacking.Resource));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_FinalPacking_Save_MSBC", lstFP);
        }
        /// <summary>
        /// Update special internal lotnumber, Mfg Date and Exp Date
        /// </summary>
        /// <param name="internalLotnumber"></param>
        /// <param name="objInnerSetReturn"></param>
        /// <returns></returns>
        public static int UpdateSpecialInternalLotNumber(string internalLotnumber, InnerSetReturn objInnerSetReturn)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internalLotnumber", internalLotnumber));
            lstFP.Add(new FloorSqlParameter("@specialInternalLotnumber", objInnerSetReturn.specialInternalLotNumber));
            lstFP.Add(new FloorSqlParameter("@ManufacturingDate", objInnerSetReturn.mfgDate));
            lstFP.Add(new FloorSqlParameter("@ExpiryDate", objInnerSetReturn.expDate));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_UpdateSpecialInternalLotNumber", lstFP);
        }

        /// <summary>
        /// Update final packing OuterLotNo based on special internal lotnumber
        /// </summary>
        /// <param name="specialInternalLotNumber"></param>
        /// <returns></returns>
        public static int UpdateSpecialInternalLotNumberForOuterLotNo(string specialInternalLotNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@specialInternalLotnumber", specialInternalLotNumber));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_UpdateSpecialInternalLotNumberForOuterLotNo", lstFP);
        }
        /// <summary>
        /// Update special internal lotnumber, Mfg Date and Exp Date for Surgical Packing Plan
        /// </summary>
        /// <param name="internalLotnumber"></param>
        /// <param name="objInnerSetReturn"></param>
        /// <returns></returns>
        public static int UpdateSpecialInternalLotNumberforSPP(string internalLotnumber, InnerSetReturn objInnerSetReturn)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internalLotnumber", internalLotnumber));
            lstFP.Add(new FloorSqlParameter("@specialInternalLotnumber", objInnerSetReturn.specialInternalLotNumber));
            lstFP.Add(new FloorSqlParameter("@ManufacturingDate", objInnerSetReturn.mfgDate));
            lstFP.Add(new FloorSqlParameter("@ExpiryDate", objInnerSetReturn.expDate));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_UpdateSpecialInternalLotNumberforSPP", lstFP);
        }
        /// <summary>
        /// validate Pallet PO Item Size
        /// </summary>
        /// <param name="poNumber"></param>
        /// <param name="ItemNumber"></param>
        /// <param name="size"></param>
        /// <param name="palletid"></param>
        /// <returns></returns>
        public static int validatePalletPOItemSize(string poNumber, string ItemNumber, string size, string palletid)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
            lstFP.Add(new FloorSqlParameter("@Size", size));
            lstFP.Add(new FloorSqlParameter("@palletid", palletid));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_Validate_PalletPoItemSize", lstFP));
        }
        /// <summary>
        /// Validate Preshipment Pallet PO
        /// </summary>
        /// <param name="poNumber"></param>
        /// <param name="palletid"></param>
        /// <returns></returns>
        public static int validatePreshipmentPalletPO(string poNumber, string palletid)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
            lstFP.Add(new FloorSqlParameter("@palletid", palletid));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_Validate_PreshipmentPalletPo", lstFP));
        }

        /// <summary>
        /// GEt associated Iteam pallet and preshipment palletid
        /// </summary>
        /// <param name="poNumber">Purchase Order</param>
        /// <param name="ItemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <returns>returns associated pallet and preshipmentPLTID</returns>
        public static String GetItemSizePalletId(string poNumber, string ItemNumber, string size, int locationId)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();

            lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
            lstFP.Add(new FloorSqlParameter("@Size", size));
            lstFP.Add(new FloorSqlParameter("@locationid", locationId));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("usp_FP_POItemPalletId_Get", lstFP));

        }

        //GetItemPreshipmentPalletId
        /// <summary>
        /// GEt associated Iteam pallet and preshipment palletid
        /// </summary>
        /// <param name="poNumber">Purchase Order</param>
        /// <param name="ItemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <returns>returns associated pallet and preshipmentPLTID</returns>
        public static String GetItemPreshipementPalletId(string poNumber, string ItemNumber, int locationId)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
            lstFP.Add(new FloorSqlParameter("@locationid", locationId));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("usp_FP_POPreshipmentPalletId_Get", lstFP));
        }

        public static int ValidatePreshipementPalletCase(string poNumber, string ItemNumber, int caseNumber, string size)
        {
            //usp_FP_POSizeBoxesPacked_Validate
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
                lstFP.Add(new FloorSqlParameter("@CaseNumber", caseNumber));
                lstFP.Add(new FloorSqlParameter("@Size", size));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_PSIPalletCase_Validate", lstFP));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }

        //GetResourceFromBatchOrder
        /// <summary>
        /// GEt Resource from FG Batch Order Number
        /// </summary>
        /// <param name="@FGBatchOrderNo">@FG Batch Order Number</param>
        /// <returns>returns Resource</returns>
        public static String GetResourceFromBatchOrder(string @FGBatchOrderNo)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@FGBatchOrderNo", FGBatchOrderNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("usp_FP_Resource_Get", lstFP));
        }

        /// <summary>
        /// Validate the Boxes Packed for the selected PO Item Size
        /// </summary>
        /// <param name="poNumber">Purchase Order</param>
        /// <param name="ItemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <returns>return boxes packed for the selected PO Item</returns>
        public static int ValidateSecondGradePOSizeBoxesPacked(string poNumber, string ItemNumber, string size)
        {
            //usp_FP_POSizeBoxesPacked_Validate
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
                lstFP.Add(new FloorSqlParameter("@Size", size));
                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_2ndGradePOSizeBoxesPacked_Validate", lstFP));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }


        /// <summary>
        /// Validate the Boxes Packed for the selected PO Item Size
        /// </summary>
        /// <param name="poNumber">Purchase Order</param>
        /// <param name="ItemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <returns>return boxes packed for the selected PO Item</returns>
        public static int ValidatePOSizeBoxesPacked(string poNumber, string ItemNumber, string size)
        {
            //usp_FP_POSizeBoxesPacked_Validate
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
                lstFP.Add(new FloorSqlParameter("@Size", size));
                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_POSizeBoxesPacked_Validate", lstFP));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }
        /// <summary>
        /// To Validate Pallet Capacity
        /// </summary>
        /// <param name="palletId"> Pallet ID</param>
        /// <param name="poNumber">Purchase Order Number</param>
        /// <param name="itemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <returns>returns cases placed on pallet </returns>
        public static int ValidatePalletCapacity(string palletId, string poNumber, string itemNumber, string size)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@PalletId", palletId));
                lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", itemNumber));
                lstFP.Add(new FloorSqlParameter("@Size", size));

                return int.Parse(Convert.ToString(FloorDBAccess.ExecuteScalar("usp_FP_PalletCapacity_validate", lstFP)));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }
        public static int ValidatePreshipmentPalletCapacity(string palletId, string poNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@PalletId", palletId));
                lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
                return int.Parse(Convert.ToString(FloorDBAccess.ExecuteScalar("usp_FP_PreshipmentPalletCapacity_validate", lstFP)));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }


        /// <summary>
        /// To Get Preshipment Case quantiy for given Range
        /// </summary>
        /// <param name="poNumber">Purchase Order</param>
        /// <param name="itemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <param name="endcase">Case Number</param>
        /// <returns>return the preshipment case count</returns>
        public static int GetPreshipmentCaseQuantitybyRange(string poNumber, string itemNumber, string size, int endcase)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", itemNumber));
                lstFP.Add(new FloorSqlParameter("@Size", size));
                lstFP.Add(new FloorSqlParameter("@endcase", endcase));

                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_PreshipmentCaseExist_Get", lstFP));
            }
            catch (Exception ftex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ftex);
            }
        }
        //usp_FP_POlastPreshipment_Validate
        public static int GetPreshipmentCasesQuantitytoPackforPO(string poNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_POlastPreshipment_Validate", lstFP));
            }
            catch (Exception ftex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ftex);
            }
        }
        //[usp_FP_Get_PreshipmentcasesListforPO]

        public static List<SOLineDTO> GetPOPreshipmentcases(string ponumber)
        {
            List<SOLineDTO> lstPOPreshipmentCases = null;
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PONumber", ponumber));
            try
            {
                using (DataTable dtPOlist = FloorDBAccess.ExecuteDataTable("usp_FP_Get_PreshipmentcasesListforPO", lstFP))
                {
                    if (dtPOlist != null && dtPOlist.Rows.Count != 0)
                    {
                        lstPOPreshipmentCases = (from DataRow row in dtPOlist.Rows
                                                 select new SOLineDTO
                                                 {
                                                     PONumber = FloorDBAccess.GetString(row, "PONumber"),
                                                     ItemNumber = FloorDBAccess.GetString(row, "ItemNumber"),
                                                     ItemSize = FloorDBAccess.GetString(row, "ItemSize"),
                                                     Preshipmentcases = FloorDBAccess.GetString(row, "preshipmentcases")
                                                 }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }

            return lstPOPreshipmentCases;
        }
        public static int GetPreshipmentCasesQuantitytoPack(string poNumber, string itemNumber, string size)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", itemNumber));
                lstFP.Add(new FloorSqlParameter("@Size", size));
                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_lastPreshipment_Validate", lstFP));
            }
            catch (Exception ftex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ftex);
            }
        }

        public static int GetPreshipmentCaseCount(string poNumber, string itemNumber, string size)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", itemNumber));
                lstFP.Add(new FloorSqlParameter("@Size", size));
                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_GET_PreshipmentCase", lstFP));
            }
            catch (Exception ftex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ftex);
            }
        }

        /// <summary>
        /// Get cases packed fro the seleted PO Item
        /// </summary>
        /// <param name="poNumber">Purchase Order</param>
        /// <param name="itemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <returns>return cases packed fro the seleted PO Item</returns>
        public static int GetPONumberItemCasesPacked(string poNumber, string itemNumber, string size)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", itemNumber));
                lstFP.Add(new FloorSqlParameter("@Size", size));

                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_POItemCaseBoxesPacked_Get", lstFP));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }
        /// <summary>
        /// Update PO Item Status
        /// </summary>
        /// <param name="poNumber">Purchase Order</param>
        /// <param name="itemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <returns>return update sucess or failure</returns>
        public static int UpdatePONumberItemstatus(string poNumber, string itemNumber, string size)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", itemNumber));
            lstFP.Add(new FloorSqlParameter("@Size", size));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_PurchaseOrderItemStatus_Update", lstFP);
        }

        public static BatchDTO ValidateSerialNumberGloveCode(decimal serialNumber, string gloveCode, string size, string alternativeGloveCode1, string alternativeGloveCode2, string alternativeGloveCode3, int innerBoxCapacity)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            BatchDTO objBatchDTO = new BatchDTO();
            lstFP.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            lstFP.Add(new FloorSqlParameter("@GloveCode", gloveCode));
            lstFP.Add(new FloorSqlParameter("@Size", size));
            lstFP.Add(new FloorSqlParameter("@ALTERNATEGLOVECODE1", alternativeGloveCode1));
            lstFP.Add(new FloorSqlParameter("@ALTERNATEGLOVECODE2", alternativeGloveCode2));
            lstFP.Add(new FloorSqlParameter("@ALTERNATEGLOVECODE3", alternativeGloveCode3));
            lstFP.Add(new FloorSqlParameter("@InnerBoxCapacity", innerBoxCapacity));
            DataTable dtBatch = FloorDBAccess.ExecuteDataTable("usp_FP_ValidateSerialNumberGloveCode", lstFP);
            if (dtBatch.Rows.Count > 0)
            {
                DataRow row = dtBatch.Rows[0];
                objBatchDTO.BatchNumber = FloorDBAccess.GetString(row, "BatchNumber");
                objBatchDTO.BatchOrder = FloorDBAccess.GetString(row, "BatchOrder");
                objBatchDTO.IsFPBatchSplit = FloorDBAccess.GetValue<Boolean>(row, "IsFPBatchSplit");
                objBatchDTO.QAIDate = FloorDBAccess.GetValue<DateTime>(row, "QAIDate");
                objBatchDTO.BatchCarddate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate");
                objBatchDTO.Line = FloorDBAccess.GetString(row, "LineId");
            }
            return objBatchDTO;
        }

        public static BatchDTO GetBatchBySerialNoAndSalesOrder(decimal serialNumber, string gloveCode, string size, string alternativeGloveCode1, string alternativeGloveCode2, string alternativeGloveCode3, int innerBoxCapacity, string PONumber, string ItemNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            BatchDTO objBatchDTO = new BatchDTO();
            lstFP.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            lstFP.Add(new FloorSqlParameter("@GloveCode", gloveCode));
            lstFP.Add(new FloorSqlParameter("@Size", size));
            lstFP.Add(new FloorSqlParameter("@ALTERNATEGLOVECODE1", alternativeGloveCode1));
            lstFP.Add(new FloorSqlParameter("@ALTERNATEGLOVECODE2", alternativeGloveCode2));
            lstFP.Add(new FloorSqlParameter("@ALTERNATEGLOVECODE3", alternativeGloveCode3));
            lstFP.Add(new FloorSqlParameter("@InnerBoxCapacity", innerBoxCapacity));
            lstFP.Add(new FloorSqlParameter("@SalesId", PONumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
            DataTable dtBatch = FloorDBAccess.ExecuteDataTable("usp_FP_GetBatchBySerialNoAndSalesOrder", lstFP);
            if (dtBatch.Rows.Count > 0)
            {
                DataRow row = dtBatch.Rows[0];
                objBatchDTO.BatchNumber = FloorDBAccess.GetString(row, "BatchNumber");
                //objBatchDTO.BatchOrder = FloorDBAccess.GetString(row, "BatchOrder"); //Max He,11/09/2018, maybe have multiple FG Batch Orders
                objBatchDTO.IsFPBatchSplit = FloorDBAccess.GetValue<Boolean>(row, "IsFPBatchSplit");
                objBatchDTO.QAIDate = FloorDBAccess.GetValue<DateTime>(row, "QAIDate");
                objBatchDTO.BatchCarddate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate");
                objBatchDTO.Line = FloorDBAccess.GetString(row, "LineId");
            }
            // Get FG Batch Order based on Item,Size,SO no
            // Max He,11/09/2018, use list to store due to may have multiple FG Batch Orders
            lstFP.Clear();
            lstFP.Add(new FloorSqlParameter("@Size", size));
            lstFP.Add(new FloorSqlParameter("@SalesId", PONumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
            lstFP.Add(new FloorSqlParameter("@Location", WorkStationDTO.GetInstance().Location));
            DataTable dtFGBatchOrder = FloorDBAccess.ExecuteDataTable("usp_FP_DOT_GetFGOrderBySOandItemSize", lstFP);
            for (int i = 0; i < dtFGBatchOrder.Rows.Count; i++)
            {
                DataRow row = dtFGBatchOrder.Rows[i];
                var batchOrder = FloorDBAccess.GetString(row, "BatchOrder");
                objBatchDTO.BatchOrders.Add(new DropdownDTO(batchOrder, batchOrder));
            }
            return objBatchDTO;
        }

        /// <summary>
        /// Validate Serial Number and Return Batch Number and IsSplitBatch
        /// </summary>
        /// <param name="serialNumber">Serial Number</param>
        /// <param name="size">Size</param>
        /// <returns>Return Batch DTO</returns>
        public static BatchDTO ValidateSerialNumber(decimal serialNumber, string size = "")
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            BatchDTO objBatchDTO = new BatchDTO();
            DataTable dtBatch = null;
            try
            {

                lstFP.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
                if (!string.IsNullOrEmpty(size))
                    lstFP.Add(new FloorSqlParameter("@Size", size));
                dtBatch = FloorDBAccess.ExecuteDataTable("usp_FP_ValidateSerialNumber", lstFP);
                if (dtBatch.Rows.Count > 0)
                {
                    DataRow row = dtBatch.Rows[0];
                    objBatchDTO.BatchNumber = FloorDBAccess.GetString(row, "BatchNumber");
                    objBatchDTO.IsFPBatchSplit = FloorDBAccess.GetValue<Boolean>(row, "IsFPBatchSplit");
                }
                return objBatchDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }

        }



        /// <summary>
        /// Validate Serial Number and Return size and GloveType
        /// </summary>
        /// <param name="serialNumber">Serial Number</param>
        /// <param name="size">Size</param>
        /// <returns>Return Batch DTO</returns>
        public static BatchDTO GetBatchDetailsBySNo(decimal serialNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            BatchDTO objBatchDTO = new BatchDTO();
            DataTable dtBatch = null;
            try
            {

                lstFP.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
                dtBatch = FloorDBAccess.ExecuteDataTable("usp_FP_GetBatchDetailsBySerailNo", lstFP);
                if (dtBatch.Rows.Count > 0)
                {
                    DataRow row = dtBatch.Rows[0];
                    objBatchDTO.BatchNumber = FloorDBAccess.GetString(row, "batchNumber");
                    objBatchDTO.IsFPBatchSplit = FloorDBAccess.GetValue<Boolean>(row, "IsFPBatchSplit");
                    objBatchDTO.Size = FloorDBAccess.GetString(row, "Size");
                    objBatchDTO.GloveType = FloorDBAccess.GetString(row, "GloveType");
                }
                return objBatchDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }

        }

        /// <summary>
        /// Validate New Serial Number to Change Batch Card
        /// </summary>
        /// <param name="serialNumber">Serial Number</param>
        /// <param name="newSerialNumber">New Seial Number</param>
        /// <param name="size">Size</param>
        /// <returns>returns Bactch Number and IsFPSplitBatch if Valid</returns>
        public static BatchDTO ValidateChangeBatchNewSerialNumber(string serialNumber, string newSerialNumber, string size = "")
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            BatchDTO objBatchDTO = new BatchDTO();
            DataTable dtBatch = null;
            try
            {
                lstFP.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
                lstFP.Add(new FloorSqlParameter("@NewSerialNumber", newSerialNumber));
                lstFP.Add(new FloorSqlParameter("@Size", size));

                dtBatch = FloorDBAccess.ExecuteDataTable("USP_FP_NewSerialNumberForChangeBatchcard_Validate", lstFP);
                if (dtBatch.Rows.Count > 0)
                {
                    DataRow row = dtBatch.Rows[0];
                    objBatchDTO.BatchNumber = FloorDBAccess.GetString(row, "BatchNumber");
                    objBatchDTO.IsFPBatchSplit = FloorDBAccess.GetValue<Boolean>(row, "IsFPBatchSplit");
                }
                return objBatchDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// Get Batch Total PCs available
        /// </summary>
        /// <param name="serialNumber">Serial Number</param>
        /// <returns> Return Batch Total PCs available</returns>
        public static int GetBatchCapacity(decimal serialNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_BatchCapacity", lstFP));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }

        }

        public static int GetBatchCapacityQCScanOut(decimal serialNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_BatchCapacity_QCScanOut", lstFP));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }

        }
        /// <summary>
        /// Update Batch PackedPcs 
        /// </summary>
        /// <param name="serialNumber"SerialNumber>Serial Number</param>
        /// <returns>return rows affected</returns>
        public static int UpdateBatchCapacity(decimal serialNumber, int PackedPCs, Boolean isFPTempPack)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            lstFP.Add(new FloorSqlParameter("@PackedPCs", PackedPCs));
            lstFP.Add(new FloorSqlParameter("@isTempPack", isFPTempPack));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_BatchPackedPcs_Update", lstFP);
        }

        public static int UpdateMultiScanBatchCapacity(string BatchInfo = null)
        {
            if (!string.IsNullOrEmpty(BatchInfo))
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@strXML", BatchInfo));
                return FloorDBAccess.ExecuteNonQuery("usp_FP_BatchPackedPcs_Update_MSBC", lstFP);
            }
            else
                return 0;
        }

        /// <summary>
        /// Check the batch is from FG or TMP
        /// </summary>
        /// <param name="serialNumber">Enetered Serial Number</param>
        /// <returns>return true if TEmppack Bacth</returns>
        public static FPTempPackDTO CheckIsTemPackBatch(decimal serialNumber)
        {
            FPTempPackDTO objFPTempPackDTO = new FPTempPackDTO();
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
                DataTable dtTempPack = FloorDBAccess.ExecuteDataTable("USP_FP_TempPackBatch", lstFP);
                objFPTempPackDTO.TMPPackBatch = new BatchDTO();
                if (dtTempPack.Rows.Count > 0)
                {
                    DataRow row = dtTempPack.Rows[0];
                    objFPTempPackDTO.TMPPackBatch.BatchNumber = FloorDBAccess.GetString(row, "BatchNumber");
                    objFPTempPackDTO.isTempPackBatch = true;
                    objFPTempPackDTO.TMPPackBatch.Size = FloorDBAccess.GetString(row, "size");
                    objFPTempPackDTO.TMPPackBatch.GloveType = FloorDBAccess.GetString(row, "GloveType");
                    objFPTempPackDTO.TMPPackBatch.IsFPBatchSplit = FloorDBAccess.GetValue<Boolean>(row, "IsSplitBatch");
                    //   objFPTempPackDTO.PackedPcs = FloorDBAccess.GetString(row, "PackedPcs");
                    //   objFPTempPackDTO.TempPackPcs = FloorDBAccess.GetString(row, "TempPackPcs");
                }
                else
                {
                    objFPTempPackDTO.isTempPackBatch = false;
                }
                return objFPTempPackDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
            //USP_FP_TempPackBatch
        }
        /// <summary>
        /// Update the POStatus to Close if transaction completed for all items
        /// </summary>
        /// <param name="PONumber">Selected Purchase Order</param>
        /// <returns>result</returns>
        public static int UpdatePOStatus(string PONumber)
        {
            int rowsReturned = 0;
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PONumber", PONumber));
            return rowsReturned = FloorDBAccess.ExecuteNonQuery("usp_FP_POStatus_Update", lstFP);
        }
        /// <summary>
        /// to verify is Second Grade Batch
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static Boolean isSecondGradeBatch(decimal serialNumber)
        {
            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();
            try
            {
                lstfp.Add(new FloorSqlParameter("@serialnumber", serialNumber));
                return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("usp_FP_SecondGradeBatch", lstfp));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.PRESHIPMENT_CASENUMBERS, Constants.BUSINESSLOGIC, ex);
            }
        }
        /// <summary>
        /// check whether batch is scan out from glove inventory system
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static Boolean isBatchScanOut(decimal serialNumber, out string location)
        {
            bool isScanOut = false;
            location = null;
            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();
            try
            {
                lstfp.Add(new FloorSqlParameter("@serialnumber", serialNumber));
                DataTable result = FloorDBAccess.ExecuteDataTable("usp_IsBatchScanOut", lstfp);
                if (result.Rows.Count > 0)
                {
                    DataRow row = result.Rows[Constants.ZERO];
                    location = FloorDBAccess.GetString(row, "LocationAreaCode");
                    isScanOut = Convert.ToBoolean(FloorDBAccess.GetString(row, "isScanOut"));
                }
                return isScanOut;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.PRESHIPMENT_CASENUMBERS, Constants.BUSINESSLOGIC, ex);
            }
        }
        /// <summary>
        /// To update the Lot Verification Result
        /// </summary>
        /// <param name="internalLotnumber"></param>
        /// <param name="lotVerificatonResult"></param>
        public static void UpdateLotVerificationResult(string internalLotnumber, bool lotVerificatonResult)
        {
            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();
            try
            {
                lstfp.Add(new FloorSqlParameter("@internallotnumber", internalLotnumber));
                lstfp.Add(new FloorSqlParameter("@LotVerificationResult", lotVerificatonResult));
                FloorDBAccess.ExecuteNonQuery("USP_FP_UpdateLotVerificationResult", lstfp);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }
        /// <summary>
        /// To update the OperatorID
        /// </summary>
        /// <param name="internalLotnumber"></param>
        /// <param name="operatorId"></param>
        public static void UpdateOperatorId(string internalLotnumber, string operatorId)
        {
            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();
            try
            {
                lstfp.Add(new FloorSqlParameter("@internallotnumber", internalLotnumber));
                lstfp.Add(new FloorSqlParameter("@OperatorID", operatorId));
                FloorDBAccess.ExecuteNonQuery("USP_FP_UpdateOperatorId", lstfp);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// To validate batch against TOMS
        /// </summary>
        /// <param name="serialNo"></param>
        public static List<FP_TOMSValidateDTO> ValidateBatch_TOMS(string serialNo)
        {
            List<FP_TOMSValidateDTO> lstBatch = new List<FP_TOMSValidateDTO>();
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@SerialNO", serialNo));
                DataTable dtTempPack = FloorDBAccess.ExecuteDataTable("USP_FP_TOMSValidation", lstFP, ConnName.FS);

                lstBatch = (from DataRow dr in dtTempPack.Rows
                            select new FP_TOMSValidateDTO
                            {
                                SerialNo = FloorDBAccess.GetString(dr, "SerialNo"),
                                StockID = FloorDBAccess.GetValue<int>(dr, "StockID"),
                                Status = FloorDBAccess.GetValue<int>(dr, "Status")
                            }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstBatch;
        }
        # endregion

        #region Surgical Pouch Printing
        public static List<DropdownDTO> GetSurgicalPOList()
        {
            List<DropdownDTO> list = new List<DropdownDTO>();
            using (DataTable dt = FloorDBAccess.ExecuteDataSet("usp_FP_SELECT_SurgicalPOList", new List<FloorSqlParameter>()).Tables[0].DefaultView.ToTable(true))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = (from DataRow row in dt.Rows
                                select new DropdownDTO
                                {
                                    IDField = FloorDBAccess.GetString(row, "ItemType"),
                                    DisplayField = string.IsNullOrEmpty(FloorDBAccess.GetString(row, "customerref")) ? FloorDBAccess.GetString(row, "SalesId") : FloorDBAccess.GetString(row, "customerref") + " | " + FloorDBAccess.GetString(row, "SalesId")
                                }).ToList();
                    }
                    else
                    {
                        list = new List<DropdownDTO>();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.PRESHIPMENTPALLET_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        /// <summary>
        /// Final Packing Trasaction method
        /// </summary>
        /// <param name="objFinalPacking">FinalPackign DTO object</param>
        /// <returns>returns rows inserted</returns>
        public static int InsertFinalPackingSurgical(FinalPackingDTO objFinalPacking, string BatchInfo = null)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@LocationId", objFinalPacking.locationId));
            lstFP.Add(new FloorSqlParameter("@WorkStationNumber", objFinalPacking.Workstationnumber));
            lstFP.Add(new FloorSqlParameter("@PrinterName", objFinalPacking.Printername));
            lstFP.Add(new FloorSqlParameter("@PackDate", objFinalPacking.Packdate));
            lstFP.Add(new FloorSqlParameter("@OuterLotNo", objFinalPacking.Outerlotno));
            lstFP.Add(new FloorSqlParameter("@InternalLotNumber", objFinalPacking.Internallotnumber));
            lstFP.Add(new FloorSqlParameter("@PONumber", objFinalPacking.Ponumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", objFinalPacking.ItemNumber));
            lstFP.Add(new FloorSqlParameter("@Size", objFinalPacking.Size));
            lstFP.Add(new FloorSqlParameter("@GroupId", objFinalPacking.GroupId));

            lstFP.Add(new FloorSqlParameter("@BoxesPacked", objFinalPacking.Boxespacked));

            lstFP.Add(new FloorSqlParameter("@CasesPacked", objFinalPacking.Casespacked));

            // lstFP.Add(new FloorSqlParameter("@OperatorId", objFinalPacking.OperatorId));
            lstFP.Add(new FloorSqlParameter("@InnerSetLayout", objFinalPacking.Innersetlayout));
            lstFP.Add(new FloorSqlParameter("@OuterSetLayout", objFinalPacking.Outersetlayout));
            lstFP.Add(new FloorSqlParameter("@palletCapacity", objFinalPacking.palletCapacity));
            lstFP.Add(new FloorSqlParameter("@TotalPcs", objFinalPacking.TotalPcs));
            lstFP.Add(new FloorSqlParameter("@isTempPack", objFinalPacking.isTempPack));
            if (!string.IsNullOrEmpty(BatchInfo))
                lstFP.Add(new FloorSqlParameter("@strXML", BatchInfo));
            lstFP.Add(new FloorSqlParameter("@ManufacturingDate", objFinalPacking.ManufacturingDate));
            lstFP.Add(new FloorSqlParameter("@ExpiryDate", objFinalPacking.ExpiryDate));
            lstFP.Add(new FloorSqlParameter("@stationNumber", objFinalPacking.stationNumber));
            lstFP.Add(new FloorSqlParameter("@InventTransId", objFinalPacking.InventTransId));
            lstFP.Add(new FloorSqlParameter("@FPStationNo", objFinalPacking.FPStationNo)); //Added by Azman 2019-08-22 FPDetailsReport
            return FloorDBAccess.ExecuteNonQuery("usp_FP_FinalPacking_Save_Surgical", lstFP);
        }

        /// <summary>
        /// Validate the Boxes Packed for the selected PO Item Size
        /// </summary>
        /// <param name="poNumber">Purchase Order</param>
        /// <param name="ItemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <returns>return boxes packed for the selected PO Item</returns>
        public static int ValidatePOSizeBoxesPackedForSurgicalPouch(string poNumber, string ItemNumber, string size)
        {
            //usp_FP_POSizeBoxesPacked_Validate
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
                lstFP.Add(new FloorSqlParameter("@Size", size));
                return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_POSizeBoxesPackedforSurgicalPouch_Validate", lstFP));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }
        #endregion

        #region Surgical Inner and Outer
        public static int ValidateSurgicalInnerOuter(string internallotnumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internalLotNumber", internallotnumber));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_Validate_SurgicalInnerOuter", lstFP));
        }

        public static int ValidateSurgicalInnerOuterV2(string internallotnumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internalLotNumber", internallotnumber));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_Validate_SurgicalInnerOuterV2", lstFP));
        }

        public static List<DropdownDTO> GetSecondGradePOList()
        {
            List<DropdownDTO> list = new List<DropdownDTO>();
            //#Max He, Batch Order filter by plant No
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@Location", WorkStationDTO.GetInstance().Location));
            using (DataTable dt = FloorDBAccess.ExecuteDataSet("usp_FP_SELECT_SecondGradePOList", lstFP).Tables[0].DefaultView.ToTable(true))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = (from DataRow row in dt.Rows
                                select new DropdownDTO
                                {
                                    IDField = FloorDBAccess.GetString(row, "ItemType"),
                                    DisplayField = string.IsNullOrEmpty(FloorDBAccess.GetString(row, "customerref")) ? FloorDBAccess.GetString(row, "SalesId") : FloorDBAccess.GetString(row, "customerref") + " | " + FloorDBAccess.GetString(row, "SalesId")
                                }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        public static SurgicalFinalPackingDTO GetSurgicalPOIDetails(string SPPSerialNo)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@SPPSerialNo", SPPSerialNo));
            DataTable dtSOLine = FloorDBAccess.ExecuteDataTable("USP_GET_FP_SurgicalPOIDetails", lstFP);
            SurgicalFinalPackingDTO objSurgicalFinalPackingDTO = new SurgicalFinalPackingDTO();

            if (dtSOLine.Rows.Count > 0)
            {
                DataRow row = dtSOLine.Rows[Constants.ZERO];
                objSurgicalFinalPackingDTO.PoNumber = FloorDBAccess.GetString(row, "PONumber");
                objSurgicalFinalPackingDTO.ItemNumber = FloorDBAccess.GetString(row, "Itemnumber");
                objSurgicalFinalPackingDTO.ItemName = FloorDBAccess.GetString(row, "ItemName");
                objSurgicalFinalPackingDTO.ItemSize = FloorDBAccess.GetString(row, "ItemSize");
                objSurgicalFinalPackingDTO.PalletCapacity = FloorDBAccess.GetValue<int>(row, "PalletCapacity");
                objSurgicalFinalPackingDTO.BoxesPacked = FloorDBAccess.GetValue<int>(row, "BoxesPacked");
                objSurgicalFinalPackingDTO.CaseCapacity = FloorDBAccess.GetValue<int>(row, "CaseCapacity");
                objSurgicalFinalPackingDTO.OrderNumber = FloorDBAccess.GetString(row, "OrderNumber");
                objSurgicalFinalPackingDTO.CustomerReferenceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber");
                objSurgicalFinalPackingDTO.CustomerSize = FloorDBAccess.GetString(row, "CustomerSize");                
            }
            // 20201110 Azrul: Get FG Batch Order No based on Size, SalesId & ItemId
            lstFP.Clear();
            lstFP.Add(new FloorSqlParameter("@Size", objSurgicalFinalPackingDTO.CustomerSize));
            lstFP.Add(new FloorSqlParameter("@SalesId", objSurgicalFinalPackingDTO.PoNumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", objSurgicalFinalPackingDTO.ItemNumber));
            lstFP.Add(new FloorSqlParameter("@Location", WorkStationDTO.GetInstance().Location));
            DataTable dtFGBatchOrder = FloorDBAccess.ExecuteDataTable("usp_FP_DOT_GetFGOrderBySOandItemSize", lstFP);
            for (int i = 0; i < dtFGBatchOrder.Rows.Count; i++)
            {
                DataRow row = dtFGBatchOrder.Rows[i];
                var batchOrder = FloorDBAccess.GetString(row, "BatchOrder");
                objSurgicalFinalPackingDTO.BatchOrders.Add(new DropdownDTO(batchOrder, batchOrder));
            }
            return objSurgicalFinalPackingDTO;
        }

        public static SurgicalFinalPackingDTO GetSurgicalInternalLotNumberDetails(string internalLotNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internalLotNumber", internalLotNumber));
            DataTable dtSOLine = FloorDBAccess.ExecuteDataTable("usp_FP_Get_SurgicalInternalLotNumberDetails", lstFP);
            SurgicalFinalPackingDTO objSurgicalFinalPackingDTO = new SurgicalFinalPackingDTO();

            if (dtSOLine.Rows.Count > 0) // this created to allow non-surgical to use this module
            {
                DataRow drSerialNumberLeft = dtSOLine.Rows[Constants.ZERO]; //surgical right
                objSurgicalFinalPackingDTO.PoNumber = FloorDBAccess.GetString(drSerialNumberLeft, "PONumber");
                objSurgicalFinalPackingDTO.ItemNumber = FloorDBAccess.GetString(drSerialNumberLeft, "Itemnumber");
                objSurgicalFinalPackingDTO.ItemName = FloorDBAccess.GetString(drSerialNumberLeft, "ItemName");
                objSurgicalFinalPackingDTO.ItemSize = FloorDBAccess.GetString(drSerialNumberLeft, "size");
                objSurgicalFinalPackingDTO.GroupId = FloorDBAccess.GetValue<int>(drSerialNumberLeft, "QCGroupId");
                objSurgicalFinalPackingDTO.BoxesPacked = FloorDBAccess.GetValue<int>(drSerialNumberLeft, "BoxesPacked");
                objSurgicalFinalPackingDTO.SerialNumberLeft = FloorDBAccess.GetValue<decimal>(drSerialNumberLeft, "SerialNumber");
                objSurgicalFinalPackingDTO.BatchNumberLeft = FloorDBAccess.GetString(drSerialNumberLeft, "BatchNumber");
                objSurgicalFinalPackingDTO.PalletCapacity = FloorDBAccess.GetValue<int>(drSerialNumberLeft, "PalletCapacity");
                objSurgicalFinalPackingDTO.CaseCapacity = FloorDBAccess.GetValue<int>(drSerialNumberLeft, "CaseCapacity");
                objSurgicalFinalPackingDTO.palletid = FloorDBAccess.GetString(drSerialNumberLeft, "Palletid");
                objSurgicalFinalPackingDTO.BarcodeVerificationRequired = FloorDBAccess.GetValue<int>(drSerialNumberLeft, "BarcodeVerificationRequired");
                objSurgicalFinalPackingDTO.OrderNumber = FloorDBAccess.GetString(drSerialNumberLeft, "OrderNumber");
                objSurgicalFinalPackingDTO.CustomerReferenceNumber = FloorDBAccess.GetString(drSerialNumberLeft, "CustomerReferenceNumber");
                objSurgicalFinalPackingDTO.CustomerLotNumber = FloorDBAccess.GetString(drSerialNumberLeft, "CustomerLotNumber");
                objSurgicalFinalPackingDTO.CustomerSize = FloorDBAccess.GetString(drSerialNumberLeft, "CustomerSize");
                objSurgicalFinalPackingDTO.NettWeight = FloorDBAccess.GetValue<decimal>(drSerialNumberLeft, "NettWeight");
                objSurgicalFinalPackingDTO.GrossWeight = FloorDBAccess.GetValue<decimal>(drSerialNumberLeft, "GrossWeight");
                objSurgicalFinalPackingDTO.ExpiryDate = FloorDBAccess.GetValue<DateTime>(drSerialNumberLeft, "ExpiryDate");
                objSurgicalFinalPackingDTO.ManufacturingDate = FloorDBAccess.GetValue<DateTime>(drSerialNumberLeft, "ManufacturingDate");
                objSurgicalFinalPackingDTO.InnerSetLayout = FloorDBAccess.GetString(drSerialNumberLeft, "InnerSetLayout");
                objSurgicalFinalPackingDTO.OuterSetLayout = FloorDBAccess.GetString(drSerialNumberLeft, "OuterSetLayout");
                objSurgicalFinalPackingDTO.OuterLotNumber = FloorDBAccess.GetString(drSerialNumberLeft, "OuterLotNo");
                objSurgicalFinalPackingDTO.QCGroupName = FloorDBAccess.GetString(drSerialNumberLeft, "QCGroupName");
            }

            if (dtSOLine.Rows.Count > Constants.ONE)
            {
                DataRow drSerialNumberRight = dtSOLine.Rows[Constants.ONE]; //surgical left
                objSurgicalFinalPackingDTO.SerialNumberRight = FloorDBAccess.GetValue<decimal>(drSerialNumberRight, "SerialNumber");
                objSurgicalFinalPackingDTO.BatchNumberRight = FloorDBAccess.GetString(drSerialNumberRight, "BatchNumber");
            }
            // 20201110 Azrul: Get FG Batch Order No based on Size, SalesId & ItemId
            lstFP.Clear();
            lstFP.Add(new FloorSqlParameter("@Size", objSurgicalFinalPackingDTO.CustomerSize));
            lstFP.Add(new FloorSqlParameter("@SalesId", objSurgicalFinalPackingDTO.PoNumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", objSurgicalFinalPackingDTO.ItemNumber));
            lstFP.Add(new FloorSqlParameter("@Location", WorkStationDTO.GetInstance().Location));
            DataTable dtFGBatchOrder = FloorDBAccess.ExecuteDataTable("usp_FP_DOT_GetFGOrderBySOandItemSize", lstFP);
            for (int i = 0; i < dtFGBatchOrder.Rows.Count; i++)
            {
                DataRow row = dtFGBatchOrder.Rows[i];
                var batchOrder = FloorDBAccess.GetString(row, "BatchOrder");
                objSurgicalFinalPackingDTO.BatchOrders.Add(new DropdownDTO(batchOrder, batchOrder));
            }
            return objSurgicalFinalPackingDTO;
        }

        public static int InsertSurgicalInnerOuter(string internalLotNumber, string palletId, int casesToPack, int palletcapacity)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internalLotNumber", internalLotNumber));
            lstFP.Add(new FloorSqlParameter("@palletid", palletId));
            lstFP.Add(new FloorSqlParameter("@CasesPacked", casesToPack));
            lstFP.Add(new FloorSqlParameter("@palletCapacity", palletcapacity));

            return FloorDBAccess.ExecuteNonQuery("usp_FP_SurgicalPrintInnerOuter", lstFP);
        }
        public static int InsertPalletId(string internalLotNumber, string palletId, int casesToPack, int palletcapacity, string ponumber, string itemnumber, string size, int locationId)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internalLotNumber", internalLotNumber));
            lstFP.Add(new FloorSqlParameter("@palletid", palletId));
            lstFP.Add(new FloorSqlParameter("@CasesPacked", casesToPack));
            lstFP.Add(new FloorSqlParameter("@palletCapacity", palletcapacity));
            lstFP.Add(new FloorSqlParameter("@PONumber", ponumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", itemnumber));
            lstFP.Add(new FloorSqlParameter("@Size", size));
            lstFP.Add(new FloorSqlParameter("@LocationId", locationId));

            return FloorDBAccess.ExecuteNonQuery("usp_FP_UpdatePalletId", lstFP);
        }
        public static int InsertPresPalletId(string internalLotNumber, string palletId, int casesToPack, int palletcapacity, string ponumber, string itemnumber, int locationId)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internalLotNumber", internalLotNumber));
            lstFP.Add(new FloorSqlParameter("@PreShipmentPalletId", palletId));
            lstFP.Add(new FloorSqlParameter("@CasesPacked", casesToPack));
            lstFP.Add(new FloorSqlParameter("@palletCapacity", palletcapacity));
            lstFP.Add(new FloorSqlParameter("@PONumber", ponumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", itemnumber));
            lstFP.Add(new FloorSqlParameter("@LocationId", locationId));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_UpdatePreshipmentPalletId", lstFP);
        }

        public static int GetSurgicalLabelCount(string internallotnumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internalLotNumber", internallotnumber));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_Get_SurgicalLabelCount", lstFP));
        }

        //added by KahHeng (09Jan019)
        //Used for update the Preshipment PalletID "isOccupied" flag to 1 in PalletMaster table
        public static int UpdatePreshipmentPalletIDFlag(string palletId, string ponumber, string itemnumber, int locationId)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PreShipmentPalletId", palletId));
            lstFP.Add(new FloorSqlParameter("@PONumber", ponumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", itemnumber));
            lstFP.Add(new FloorSqlParameter("@LocationId", locationId));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_UpdatePreshipmentPalletIdFlag", lstFP);
        }
        #endregion

        #region RePrintInnerLabel
        /// <summary>
        /// To validate Internal Lot Number
        /// </summary>
        /// <param name="innerLotNo">Internal Lot Number</param>
        /// <returns>return true if Valid</returns>
        public static Boolean validateInnerLotNumber(string innerLotNo, int itemtype)
        {
            try
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@InternalLotNumber", innerLotNo));
                lstFP.Add(new FloorSqlParameter("@itemtype", itemtype));
                int rowsreturned = int.Parse(Convert.ToString(FloorDBAccess.ExecuteScalar("usp_FP_InnerLotNumber_validate", lstFP)));
                if (rowsreturned > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }

        }
        /// <summary>
        /// RETRIEVE INTERNALLOT NUMBERS DDETAILS
        /// </summary>
        /// <param name="internalLotNumber">Autogenerated Internal Lot Number</param>
        /// <returns>FinalPacking Data transfer object</returns>
        public static FinalPackingDTO GetInternalLotNumberDetails(string internalLotNumber)
        {
            try
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@InternalLotnumber", internalLotNumber));
                DataTable dtFinalPackingDTO = FloorDBAccess.ExecuteDataTable("usp_FP_InternalLotNumber_Get", lstFP);
                FinalPackingDTO objFinalPackingDTO = new FinalPackingDTO();
                if (dtFinalPackingDTO.Rows.Count > 0)
                {
                    DataRow row = dtFinalPackingDTO.Rows[0];
                    objFinalPackingDTO.Ponumber = row["PONumber"].ToString();
                    objFinalPackingDTO.ItemNumber = Convert.ToString(row["ItemNumber"]);
                    objFinalPackingDTO.ItemName = Convert.ToString(row["ItemName"]);
                    //objFinalPackingDTO.GroupId = Convert.ToInt32(row["QCGroupId"]);
                    //objFinalPackingDTO.QCGroupName = Convert.ToString(row["QCGroupName"]);
                    objFinalPackingDTO.Serialnumber = Convert.ToDecimal(row["SerialNumber"]);
                    objFinalPackingDTO.BatchNumber = Convert.ToString(row["BatchNumber"]);
                    objFinalPackingDTO.Boxespacked = Convert.ToInt32(row["BoxesPacked"]);
                    objFinalPackingDTO.Palletid = Convert.ToString(row["PalletId"]);
                    objFinalPackingDTO.PreshipmentPLTId = Convert.ToString(row["PreShipmentPalletId"]);
                    objFinalPackingDTO.Size = Convert.ToString(row["Size"]);
                    objFinalPackingDTO.GloveType = Convert.ToString(row["GloveType"]);
                    objFinalPackingDTO.ManufacturingDate = FloorDBAccess.GetValue<DateTime>(row, "ManufacturingDate");
                    objFinalPackingDTO.ExpiryDate = FloorDBAccess.GetValue<DateTime>(row, "ExpiryDate");
                    objFinalPackingDTO.BarcodetoValidate = Convert.ToString(row["BarcodetoValidate"]);
                    objFinalPackingDTO.CounttoValidate = FloorDBAccess.GetValue<int>(row, "counttovalidate");
                    objFinalPackingDTO.Barcodevalidationrequired = FloorDBAccess.GetValue<int>(row, "BarcodeVerificationRequired");
                    objFinalPackingDTO.orderNumber = FloorDBAccess.GetString(row, "Ordernumber");
                    objFinalPackingDTO.CustomerReferenceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber");
                    objFinalPackingDTO.locationId = Convert.ToInt32(row["LocationId"]);
                    objFinalPackingDTO.Innersetlayout = FloorDBAccess.GetString(row, "InnerSetLayout");
                    objFinalPackingDTO.Outersetlayout = FloorDBAccess.GetString(row, "OuterSetLayout");
                    //objFinalPackingDTO.caseCapacity = Convert.ToInt32(row["CaseCapacity"]);
                    //objFinalPackingDTO.innerBoxCapacity = Convert.ToInt32(row["InnerBoxCapacity"]);
                }
                return objFinalPackingDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTIONREPRINTINNER, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// RETRIEVE INTERNALLOT NUMBERS DDETAILS
        /// </summary>
        /// <param name="internalLotNumber">Autogenerated Internal Lot Number</param>
        /// <returns>FinalPacking Data transfer object</returns>
        public static FinalPackingDTO GetSecondGradeInternalLotNumberDetails(string internalLotNumber)
        {
            try
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@InternalLotnumber", internalLotNumber));
                DataTable dtFinalPackingDTO = FloorDBAccess.ExecuteDataTable("usp_FP_SecondGradeInternalLotNumber_Get", lstFP);
                FinalPackingDTO objFinalPackingDTO = new FinalPackingDTO();
                if (dtFinalPackingDTO.Rows.Count > Constants.ZERO)
                {
                    DataRow row = dtFinalPackingDTO.Rows[0];
                    objFinalPackingDTO.Ponumber = row["PONumber"].ToString();
                    objFinalPackingDTO.ItemNumber = Convert.ToString(row["ItemNumber"]);
                    objFinalPackingDTO.ItemName = Convert.ToString(row["ItemName"]);
                    objFinalPackingDTO.GroupId = Convert.ToInt32(row["QCGroupId"]);
                    objFinalPackingDTO.QCGroupName = Convert.ToString(row["QCGroupName"]);
                    objFinalPackingDTO.Boxespacked = Convert.ToInt32(row["BoxesPacked"]);
                    objFinalPackingDTO.Size = Convert.ToString(row["Size"]);
                    objFinalPackingDTO.Casespacked = Convert.ToInt32(row["CasesPacked"]);
                    objFinalPackingDTO.ManufacturingDate = FloorDBAccess.GetValue<DateTime>(row, "ManufacturingDate");
                    objFinalPackingDTO.ExpiryDate = FloorDBAccess.GetValue<DateTime>(row, "ExpiryDate");
                    objFinalPackingDTO.CustomerReferenceNumber = Convert.ToString(row["CustomerReferenceNumber"]);
                    objFinalPackingDTO.locationId = Convert.ToInt32(row["LocationId"]);
                    objFinalPackingDTO.Innersetlayout = FloorDBAccess.GetString(row, "InnerSetLayout");
                    objFinalPackingDTO.Outersetlayout = FloorDBAccess.GetString(row, "OuterSetLayout");
                }
                return objFinalPackingDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTIONREPRINTINNER, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// RETRIEVE INTERNALLOT NUMBERS DDETAILS
        /// </summary>
        /// <param name="internalLotNumber">Autogenerated Internal Lot Number</param>
        /// <returns>FinalPacking Data transfer object</returns>
        public static SOLineDTO GetPODetailsByInternalLotNumber(string internalLotNumber)
        {
            try
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@InternalLotnumber", internalLotNumber));
                DataTable dtSoLineDTO = FloorDBAccess.ExecuteDataTable("usp_GetPODetailsByInternalLotNumber", lstFP);
                SOLineDTO objSoLineDTO = new SOLineDTO();
                if (dtSoLineDTO.Rows.Count > Constants.ZERO)
                {
                    DataRow row = dtSoLineDTO.Rows[0];
                    objSoLineDTO.PONumber = row["PONumber"].ToString();
                    objSoLineDTO.ItemNumber = Convert.ToString(row["ItemNumber"]);
                    objSoLineDTO.ItemName = Convert.ToString(row["ItemName"]);
                    objSoLineDTO.AlternateGloveCode1 = FloorDBAccess.GetString(row, "AlternateGloveCode1");
                    objSoLineDTO.AlternateGloveCode2 = FloorDBAccess.GetString(row, "AlternateGloveCode2");
                    objSoLineDTO.AlternateGloveCode3 = FloorDBAccess.GetString(row, "AlternateGloveCode3");
                }
                return objSoLineDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GETPODETAILS_EXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// SAVE REPRINT INNER LABEL DETAILS
        /// </summary>
        /// <param name="objFPReprintInner"></param>
        /// <returns></returns>
        public static int InsertFPReprintInner(FPReprintInner objFPReprintInner)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();

            lstFP.Add(new FloorSqlParameter("@InternalLotNo", objFPReprintInner.InternalLotNumber));
            lstFP.Add(new FloorSqlParameter("@LocationId", objFPReprintInner.LocationId));
            lstFP.Add(new FloorSqlParameter("@WorkStationNumber", objFPReprintInner.WorkStationNumber));
            lstFP.Add(new FloorSqlParameter("@PrinterName", objFPReprintInner.PrinterName));
            lstFP.Add(new FloorSqlParameter("@AuthorizedBy", objFPReprintInner.AuthhorizedBy));

            return FloorDBAccess.ExecuteNonQuery("usp_FP_ReprintInnerBox_Save", lstFP);

        }
        # endregion

        #region RePrintOuterLabel
        /// <summary>
        /// CHECK PO STATUS
        /// </summary>
        /// <param name="PONumber"></param>
        /// <returns></returns>
        public static Boolean CheckPOStatus(string PONumber)
        {
            try
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@PONumber", PONumber));
                if (string.Compare(Convert.ToString(FloorDBAccess.ExecuteScalar("usp_FP_POStatus", lstFP)), "Closed") == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTIONREPRINTOUTER, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// SAVE REPRINT OUTER DETAILS
        /// </summary>
        /// <param name="objRePrintOuter">REPRINT OUTER DETAILS</param>
        /// <returns></returns>

        public static int InsertFPReprintOuter(RePrintOuter objRePrintOuter)
        {

            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@LocationId", objRePrintOuter.LocationId));
            lstFP.Add(new FloorSqlParameter("@WorkStationNumber", objRePrintOuter.WorkStationNumber));
            lstFP.Add(new FloorSqlParameter("@PrinterName", objRePrintOuter.PrinterName));
            lstFP.Add(new FloorSqlParameter("@PONumber", objRePrintOuter.PONumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", objRePrintOuter.ItemNumber));
            lstFP.Add(new FloorSqlParameter("@StartCase", objRePrintOuter.StartCase));
            lstFP.Add(new FloorSqlParameter("@EndCase", objRePrintOuter.EndCase));
            lstFP.Add(new FloorSqlParameter("@AuthorizedBy", objRePrintOuter.AuthorizedBy));
            lstFP.Add(new FloorSqlParameter("@InternalLotNumber", objRePrintOuter.InternalLotNo));
            if (objRePrintOuter.NoOfCopy == 0)
                lstFP.Add(new FloorSqlParameter("@NoOfCopy", DBNull.Value));
            else
                lstFP.Add(new FloorSqlParameter("@NoOfCopy", objRePrintOuter.NoOfCopy));

            return FloorDBAccess.ExecuteNonQuery("usp_FP_FPReprintOuter_Save", lstFP);
        }
        /// <summary>
        /// VALIDATE START OR END CASE
        /// </summary>
        /// <param name="objCaseNumberValidation">CaseNumber details</param>
        /// <returns></returns>
        public static Boolean ValidatestartandEndcase(FinalPackingDTO objCaseNumberValidation)
        {
            try
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@PONumber", objCaseNumberValidation.Ponumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", objCaseNumberValidation.ItemNumber));
                lstFP.Add(new FloorSqlParameter("@Size", objCaseNumberValidation.Size));
                lstFP.Add(new FloorSqlParameter("@CaseNumber", objCaseNumberValidation.CaseNumber));

                int casecount = Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_startandEndcase_Validate", lstFP));
                if (casecount > 0)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTIONCASEVALIDATION, Constants.BUSINESSLOGIC, ex);
            }

        }

        /// <summary>
        /// VALIDATE START OR END CASE
        /// </summary>
        /// <param name="objCaseNumberValidation">CaseNumber details</param>
        /// <returns></returns>
        public static Boolean ValidateInternalLotNo(FinalPackingDTO objCaseNumberValidation)
        {
            try
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@PONumber", objCaseNumberValidation.Ponumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", objCaseNumberValidation.ItemNumber));
                lstFP.Add(new FloorSqlParameter("@Size", objCaseNumberValidation.Size));
                lstFP.Add(new FloorSqlParameter("@InternalLotNumber", objCaseNumberValidation.Internallotnumber));

                int casecount = Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_PurchaseOrderItemCasesForReprint_Validate", lstFP));
                return casecount > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTIONCASEVALIDATION, Constants.BUSINESSLOGIC, ex);
            }

        }

        public static List<DropdownDTO> GetReprintOuterCasePOList(int intDateRange)
        {
            List<DropdownDTO> list = new List<DropdownDTO>();
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@ConfiguredDayRange", intDateRange));
            using (DataTable dt = FloorDBAccess.ExecuteDataSet("usp_FP_SELECT_ReprintOuterCasePOList", lstFP).Tables[0].DefaultView.ToTable(true))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = (from DataRow row in dt.Rows
                                select new DropdownDTO
                                {
                                    IDField = FloorDBAccess.GetString(row, "PONumber"),
                                    DisplayField = string.IsNullOrEmpty(FloorDBAccess.GetString(row, "CustomerReferenceNumber")) ? FloorDBAccess.GetString(row, "PONumber") : FloorDBAccess.GetString(row, "CustomerReferenceNumber") + " | " + FloorDBAccess.GetString(row, "PONumber")
                                }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }
        #endregion

        #region Change BatchCard for Inner

        /// <summary>
        /// CHANGE BATCH CARD FOR INNER
        /// </summary>
        /// <param name="objFPChangeBatchCardDTO">CHANGE BATCH CARD DTO OBJECT </param>
        /// <returns></returns>
        public static int ChangeBatchCardForInner(FPChangeBatchCardDTO objFPChangeBatchCardDTO)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@SerialNumber", objFPChangeBatchCardDTO.NewSerialNumber));
            lstFP.Add(new FloorSqlParameter("@InternalLotNumber", objFPChangeBatchCardDTO.InternalLotNumber));
            lstFP.Add(new FloorSqlParameter("@GroupId", objFPChangeBatchCardDTO.GroupId));
            lstFP.Add(new FloorSqlParameter("@LocationId", objFPChangeBatchCardDTO.LocationId));

            return FloorDBAccess.ExecuteNonQuery("usp_ChangeBatchCardForInner_Save", lstFP);
        }

        /// <summary>
        /// To validate Quantity of Remaining pcs of new serial number
        /// </summary>
        /// <param name="internalLotNumber"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static bool CBCIQuantityValidation(string internalLotNumber, decimal serialNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@InternalLotnumber", internalLotNumber));
            lstFP.Add(new FloorSqlParameter("@NewSerialNumber", serialNumber));

            return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("usp_CBCI_SerilaNoValidation", lstFP));
        }

        /// <summary>
        /// Validate Internal Lotnumber for CBCI
        /// </summary>
        /// <param name="innerLotNo">internal Lotnumber</param>
        /// <param name="itemtype">item type</param>
        /// <returns></returns>
        public static Boolean validateInnerLotNumberforCBCI(string innerLotNo, int itemtype)
        {
            try
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@InternalLotNumber", innerLotNo));
                lstFP.Add(new FloorSqlParameter("@itemtype", itemtype));
                int rowsreturned = int.Parse(Convert.ToString(FloorDBAccess.ExecuteScalar("usp_FP_InnerLotNumber_ValidateForCBCI", lstFP)));
                if (rowsreturned > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// CHANGE BATCH CARD FOR INNER V2
        /// </summary>
        /// <param name="objFPChangeBatchCardDTO"></param>
        /// <returns></returns> #AzmanCBCI
        public static int ChangeBatchCardForInnerV2(FPChangeBatchCardV2DTO objFPChangeBatchCardDTO)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@InternalLotNumber", objFPChangeBatchCardDTO.InternalLotNumber));
            lstFP.Add(new FloorSqlParameter("@GroupId", objFPChangeBatchCardDTO.GroupId));
            lstFP.Add(new FloorSqlParameter("@LocationId", objFPChangeBatchCardDTO.LocationId));
            lstFP.Add(new FloorSqlParameter("@PackSize", objFPChangeBatchCardDTO.PackSize));

            lstFP.Add(new FloorSqlParameter("@SerialNumber1", objFPChangeBatchCardDTO.NewSerialNumber1));
            lstFP.Add(new FloorSqlParameter("@SerialNumber1Qty", objFPChangeBatchCardDTO.NewSerialNumber1QtyUse));
            lstFP.Add(new FloorSqlParameter("@SerialNumber1Inner", objFPChangeBatchCardDTO.NewSerialNumber1QtyInner));
            //lstFP.Add(new FloorSqlParameter("@SerialNumber1Case", objFPChangeBatchCardDTO.NewSerialNumber1QtyCase));

            if (!string.IsNullOrEmpty(objFPChangeBatchCardDTO.NewSerialNumber2.ToString()))
            {
                lstFP.Add(new FloorSqlParameter("@SerialNumber2", objFPChangeBatchCardDTO.NewSerialNumber2));
                lstFP.Add(new FloorSqlParameter("@SerialNumber2Qty", objFPChangeBatchCardDTO.NewSerialNumber2QtyUse));
                lstFP.Add(new FloorSqlParameter("@SerialNumber2Inner", objFPChangeBatchCardDTO.NewSerialNumber2QtyInner));
                //lstFP.Add(new FloorSqlParameter("@SerialNumber2Case", objFPChangeBatchCardDTO.NewSerialNumber2QtyCase));
            }
            else
            {
                lstFP.Add(new FloorSqlParameter("@SerialNumber2Qty", "0"));
            }

            if (!string.IsNullOrEmpty(objFPChangeBatchCardDTO.NewSerialNumber3.ToString()))
            {
                lstFP.Add(new FloorSqlParameter("@SerialNumber3", objFPChangeBatchCardDTO.NewSerialNumber3));
                lstFP.Add(new FloorSqlParameter("@SerialNumber3Qty", objFPChangeBatchCardDTO.NewSerialNumber3QtyUse));
                lstFP.Add(new FloorSqlParameter("@SerialNumber3Inner", objFPChangeBatchCardDTO.NewSerialNumber3QtyInner));
                //lstFP.Add(new FloorSqlParameter("@SerialNumber3Case", objFPChangeBatchCardDTO.NewSerialNumber3QtyCase));
            }
            else
            {
                lstFP.Add(new FloorSqlParameter("@SerialNumber3Qty", "0"));
            }

            return FloorDBAccess.ExecuteNonQuery("usp_ChangeBatchCardForInner_SaveCBCI", lstFP);
        }

        /// <summary>
        /// Get Serial Information including Pcs and Weight
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns> #AzmanCBCI
        public static BatchDTO GetSerialTotalPcsInformation(decimal serialNumber)
        {
            try
            {
                BatchDTO objBatchDTO = new BatchDTO();
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@SerialNumber", serialNumber));

                DataTable dtBatch = FloorDBAccess.ExecuteDataTable("usp_FP_GetBatchWeightPcsBySerialNo", lstFP);
                if (dtBatch.Rows.Count > 0)
                {
                    DataRow row = dtBatch.Rows[0];
                    objBatchDTO.BatchNumber = FloorDBAccess.GetString(row, "BatchNumber");
                    objBatchDTO.BatchWeight = FloorDBAccess.GetValue<decimal>(row, "BatchWeight");
                    objBatchDTO.TenPcsWeight = FloorDBAccess.GetValue<decimal>(row, "TenPCsWeight");
                    objBatchDTO.TotalPcs = FloorDBAccess.GetValue<int>(row, "TotalPcs");
                    objBatchDTO.PackPcs = FloorDBAccess.GetValue<int>(row, "PackedPcs");
                }
                return objBatchDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
        }

        public static BatchDTO GetPackingInformationByLotNo(string internalLotNo)
        {
            try
            {
                BatchDTO objBatchDTO = new BatchDTO();
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@SerialNumber", internalLotNo));

                DataTable dtBatch = FloorDBAccess.ExecuteDataTable("usp_FP_GetBatchWeightPcsBySerialNo", lstFP);
                if (dtBatch.Rows.Count > 0)
                {
                    DataRow row = dtBatch.Rows[0];
                    objBatchDTO.BatchNumber = FloorDBAccess.GetString(row, "BatchNumber");
                    objBatchDTO.BatchWeight = FloorDBAccess.GetValue<decimal>(row, "BatchWeight");
                    objBatchDTO.TenPcsWeight = FloorDBAccess.GetValue<decimal>(row, "TenPCsWeight");
                    objBatchDTO.TotalPcs = FloorDBAccess.GetValue<int>(row, "TotalPcs");
                    objBatchDTO.PackPcs = FloorDBAccess.GetValue<int>(row, "PackedPcs");
                }
                return objBatchDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// Rollback CBCI Changes
        /// </summary>
        /// <param name="objFPChangeBatchCardDTO"></param>
        /// <returns></returns> #AzmanCBCI
        public static int DeleteChangeBatchCardDataV2(FPChangeBatchCardV2DTO objFPChangeBatchCardDTO)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internalLotNumber", objFPChangeBatchCardDTO.InternalLotNumber));

            lstFP.Add(new FloorSqlParameter("@SerialNumber1", objFPChangeBatchCardDTO.NewSerialNumber1));
            lstFP.Add(new FloorSqlParameter("@SerialNumber1Qty", objFPChangeBatchCardDTO.NewSerialNumber1QtyUse));

            if (!string.IsNullOrEmpty(objFPChangeBatchCardDTO.NewSerialNumber2.ToString()))
            {
                lstFP.Add(new FloorSqlParameter("@SerialNumber2", objFPChangeBatchCardDTO.NewSerialNumber2));
                lstFP.Add(new FloorSqlParameter("@SerialNumber2Qty", objFPChangeBatchCardDTO.NewSerialNumber2QtyUse));
            }
            else
            {
                lstFP.Add(new FloorSqlParameter("@SerialNumber2Qty", "0"));
            }

            if (!string.IsNullOrEmpty(objFPChangeBatchCardDTO.NewSerialNumber3.ToString()))
            {
                lstFP.Add(new FloorSqlParameter("@SerialNumber3", objFPChangeBatchCardDTO.NewSerialNumber3));
                lstFP.Add(new FloorSqlParameter("@SerialNumber3Qty", objFPChangeBatchCardDTO.NewSerialNumber3QtyUse));
            }
            else
            {
                lstFP.Add(new FloorSqlParameter("@SerialNumber3Qty", "0"));
            }
            return FloorDBAccess.ExecuteNonQuery("usp_FP_RollBackChangeBatchCardDataCBCI", lstFP);
        }

        /// <summary>
        /// RETRIEVE INTERNALLOT NUMBERS DETAILS WITH PCS COUNT
        /// </summary>
        /// <param name="internalLotNumber">Autogenerated Internal Lot Number</param>
        /// <returns>FinalPacking Data transfer object</returns> #AzmanCBCI
        public static List<FinalPackingDTO> GetInternalLotNumberDetailsWithPcs(string internalLotNumber, string spName)
        {
            try
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@InternalLotnumber", internalLotNumber));
                DataTable dtFinalPackingDTO = FloorDBAccess.ExecuteDataTable(spName, lstFP);
                List<FinalPackingDTO> objFinalPackingDTOList = new List<FinalPackingDTO>();

                for (var i = 0; i < dtFinalPackingDTO.Rows.Count; i++)
                {
                    FinalPackingDTO objFinalPackingDTO = new FinalPackingDTO();
                    DataRow row = dtFinalPackingDTO.Rows[i];
                    objFinalPackingDTO.Ponumber = row["PONumber"].ToString();
                    objFinalPackingDTO.ItemNumber = Convert.ToString(row["ItemNumber"]);
                    objFinalPackingDTO.ItemName = Convert.ToString(row["ItemName"]);
                    objFinalPackingDTO.PSIReworkOrderNo = Convert.ToString(row["PSIReworkOrderNo"]);
                    objFinalPackingDTO.Serialnumber = Convert.ToDecimal(row["SerialNumber"]);
                    objFinalPackingDTO.BatchNumber = Convert.ToString(row["BatchNumber"]);
                    objFinalPackingDTO.Boxespacked = Convert.ToInt32(row["BoxesPacked"]);
                    objFinalPackingDTO.TotalPcs = Convert.ToInt32(row["LotNoPcs"]);
                    objFinalPackingDTO.Palletid = Convert.ToString(row["PalletId"]);
                    objFinalPackingDTO.PreshipmentPLTId = Convert.ToString(row["PreShipmentPalletId"]);
                    objFinalPackingDTO.Size = Convert.ToString(row["Size"]);
                    objFinalPackingDTO.GloveType = Convert.ToString(row["GloveType"]);
                    objFinalPackingDTO.ManufacturingDate = FloorDBAccess.GetValue<DateTime>(row, "ManufacturingDate");
                    objFinalPackingDTO.ExpiryDate = FloorDBAccess.GetValue<DateTime>(row, "ExpiryDate");
                    objFinalPackingDTO.BarcodetoValidate = Convert.ToString(row["BarcodetoValidate"]);
                    objFinalPackingDTO.CounttoValidate = FloorDBAccess.GetValue<int>(row, "counttovalidate");
                    objFinalPackingDTO.Barcodevalidationrequired = FloorDBAccess.GetValue<int>(row, "BarcodeVerificationRequired");
                    objFinalPackingDTO.orderNumber = FloorDBAccess.GetString(row, "Ordernumber");
                    objFinalPackingDTO.CustomerReferenceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber");
                    objFinalPackingDTO.locationId = Convert.ToInt32(row["LocationId"]);
                    objFinalPackingDTO.Innersetlayout = FloorDBAccess.GetString(row, "InnerSetLayout");
                    objFinalPackingDTO.Outersetlayout = FloorDBAccess.GetString(row, "OuterSetLayout");
                    objFinalPackingDTO.Casespacked = Convert.ToInt32(row["CasesPacked"]); // 2017-03-22 CBCI Enhancement
                    objFinalPackingDTO.PSIStatus = FloorDBAccess.GetString(row, "PSIStatus"); //#AZRUL 16-10-2018 BUGS 1207: Prompt message if PSI Rework Order not started.
                    objFinalPackingDTOList.Add(objFinalPackingDTO);
                }
                return objFinalPackingDTOList;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTIONREPRINTINNER, Constants.BUSINESSLOGIC, ex);
            }
        }

        #endregion

        #region Scan TMP Pack Inventory
        public static int ValidateFPTempPack(string serialNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@serialNumber", serialNumber));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_ValidateFPTemppack", lstFP));
        }

        /// <summary>
        /// GET BATCHCARD DETAILS BY SERIALNUMBER
        /// </summary>
        /// <param name="serialNumber">Serial Number</param>
        /// <returns></returns>
        public static BatchDTO BatchCardDetailsbySerialNumber(decimal serialNumber)
        {
            try
            {
                BatchDTO objBatchDTO = new BatchDTO();
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@SerialNumber", serialNumber));

                DataTable dtBatch = FloorDBAccess.ExecuteDataTable("usp_FO_GetBatchDetailsbySerialNumber", lstFP);
                if (dtBatch.Rows.Count > 0)
                {
                    DataRow row = dtBatch.Rows[0];
                    objBatchDTO.BatchNumber = FloorDBAccess.GetString(row, "BatchNumber");
                    objBatchDTO.GloveType = FloorDBAccess.GetString(row, "GloveType");
                    objBatchDTO.Size = FloorDBAccess.GetString(row, "Size");
                    objBatchDTO.BatchWeight = FloorDBAccess.GetValue<decimal>(row, "BatchWeight");
                    objBatchDTO.TenPcsWeight = FloorDBAccess.GetValue<decimal>(row, "TenPCsWeight");
                    objBatchDTO.QCType = FloorDBAccess.GetString(row, "QCType");
                    objBatchDTO.TotalPcs = FloorDBAccess.GetValue<int>(row, "TotalPcs");
                    objBatchDTO.UpdatedPcs = FloorDBAccess.GetValue<int>(row, "UpdatedPcs");
                    objBatchDTO.QCTypeDescription = FloorDBAccess.GetString(row, "DESCRIPTION");
                }
                return objBatchDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// This method will return list of batch card details
        /// </summary>
        /// <param name="internalLotNumber"></param>
        /// <returns></returns>
        public static List<BatchDTO> BatchCardDetailsbyInternalLotNumber(string internalLotNumber)
        {
            try
            {
                BatchDTO objBatchDTO = new BatchDTO();
                List<BatchDTO> lstBatchDTO = new List<BatchDTO>();
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@InternalLotNumber", internalLotNumber));
                using (DataTable dtBatch = FloorDBAccess.ExecuteDataTable("usp_FP_GetBatchDetailsbyInternalLotNumber", lstFP))
                {
                    if (dtBatch.Rows.Count > Constants.ZERO)
                    {
                        DataRow row = dtBatch.Rows[0];
                        objBatchDTO.BatchNumber = FloorDBAccess.GetString(row, "BatchNumber");
                        objBatchDTO.GloveType = FloorDBAccess.GetString(row, "GloveType");
                        objBatchDTO.Size = FloorDBAccess.GetString(row, "Size");
                        objBatchDTO.BatchWeight = FloorDBAccess.GetValue<decimal>(row, "BatchWeight");
                        objBatchDTO.TenPcsWeight = FloorDBAccess.GetValue<decimal>(row, "TenPCsWeight");
                        objBatchDTO.QCType = FloorDBAccess.GetString(row, "QCType");
                        objBatchDTO.TotalPcs = FloorDBAccess.GetValue<int>(row, "TotalPcs");
                        objBatchDTO.Line = FloorDBAccess.GetString(row, "LineId");
                        objBatchDTO.BatchCarddate = FloorDBAccess.GetValue<DateTime>(row, "BatchCarddate");
                        objBatchDTO.ShortDate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate").ToString(Constants.START_DATE);
                        objBatchDTO.ShortTime = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate").ToString(Constants.START_TIME);
                        lstBatchDTO.Add(objBatchDTO);
                    }
                }
                return lstBatchDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// This method will return list of Serial Number from Internal Lot No
        /// </summary>
        /// <param name="internalLotNumber"></param>
        /// <returns></returns>
        public static List<BatchDTO> GetSerialNumberbyInternalLotNumber(string internalLotNumber)
        {
            try
            {
                BatchDTO objBatchDTO = new BatchDTO();
                List<BatchDTO> lstBatchDTO = new List<BatchDTO>();
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@InternalLotNumber", internalLotNumber));
                using (DataTable dtBatch = FloorDBAccess.ExecuteDataTable("USP_DOT_FP_GetSerialNumberbyInternalLotNumber", lstFP))
                {
                    if (dtBatch.Rows.Count > Constants.ZERO)
                    {
                        lstBatchDTO = (from DataRow row in dtBatch.Rows
                                       select new BatchDTO 
                                       {
                                           SerialNumber = FloorDBAccess.GetString(row, "SerialNumber") 
                                       }).ToList();
                    }
                }
                return lstBatchDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// Save Batch to Temp Pack Inventory
        /// </summary>
        /// <param name="objFPTEmpPackDTO">FINAL PACKING TEMPPACK INVENTORY OBJECT </param>
        /// <returns>reuturns the rows affected</returns>
        public static int TMPPackSave(FPTempPackDTO objFPTEmpPackDTO)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@SerialNumber", objFPTEmpPackDTO.SerialNumber));
            lstFP.Add(new FloorSqlParameter("@TempPackReason", objFPTEmpPackDTO.TempPackReason));
            lstFP.Add(new FloorSqlParameter("@TempPackPcs", objFPTEmpPackDTO.TempPackPcs));
            lstFP.Add(new FloorSqlParameter("@PrinterName", objFPTEmpPackDTO.PrinterName));
            lstFP.Add(new FloorSqlParameter("@LocationId", objFPTEmpPackDTO.LocationID));
            lstFP.Add(new FloorSqlParameter("@WorkStationNumber", objFPTEmpPackDTO.WorkStationNumber));

            return FloorDBAccess.ExecuteNonQuery("usp_FP_FPTempack_Save", lstFP);
        }

        #endregion

        # region Printing
        /// <summary>
        /// Get SOLine Data for the selected PO Item for Printing
        /// </summary>
        /// <param name="soNumber">Sales Order Line Number</param>
        /// <param name="itemNumber">Item Number</param>
        /// <param name="Size">Size</param>
        /// <returns>return Soline DTO object</returns>
        public static SOLineDTO GetSOLineData(string soNumber, string itemNumber, string Size)
        {
            try
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@PONumber", soNumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", itemNumber));
                lstFP.Add(new FloorSqlParameter("@ItemSize", Size));
                SOLineDTO objSo = new SOLineDTO();
                DataTable dtSOLine = FloorDBAccess.ExecuteDataTable("usp_FP_SolineData_Get", lstFP);

                if (dtSOLine.Rows.Count > 0)
                {
                    foreach (DataRow row in dtSOLine.Rows)
                    {
                        objSo.PONumber = FloorDBAccess.GetString(row, "PONumber");
                        objSo.BarcodeVerificationRequired = FloorDBAccess.GetValue<int>(row, "BarcodeVerificationRequired");
                        //  BrandBarcode = FloorDBAccess.GetString(row, "BrandBarcode"),
                        //objSo.CustomerSpecification = FloorDBAccess.GetString(row, "CustomerSpecification");
                        objSo.PreshipmentPlan = FloorDBAccess.GetValue<int>(row, "PreshipmentPlan");
                        objSo.ItemNumber = FloorDBAccess.GetString(row, "ItemNumber");
                        objSo.ItemSize = FloorDBAccess.GetString(row, "ItemSize");
                        objSo.ItemCases = FloorDBAccess.GetValue<int>(row, "ItemCases");
                        objSo.InnersetLayout = FloorDBAccess.GetString(row, "InnersetLayout");
                        objSo.OuterSetLayout = FloorDBAccess.GetString(row, "OuterSetLayout");
                        objSo.InnerLabelSetDateFormat = FloorDBAccess.GetString(row, "InnerLabelSetDateFormat"); // yiksiu SEP 2017: Label Set Optimization
                        objSo.OuterLabelSetDateFormat = FloorDBAccess.GetString(row, "OuterLabelSetDateFormat"); // yiksiu SEP 2017: Label Set Optimization
                        objSo.CustomerSize = FloorDBAccess.GetString(row, "CustomerSize");
                        objSo.CustomerSizeDesc = FloorDBAccess.GetString(row, "CustomerSizeDesc");
                        objSo.GrossWeight = FloorDBAccess.GetValue<decimal>(row, "GrossWeight");
                        objSo.NettWeight = FloorDBAccess.GetValue<decimal>(row, "NettWeight");
                        objSo.CaseCapacity = FloorDBAccess.GetValue<int>(row, "CaseCapacity");
                        objSo.PalletCapacity = FloorDBAccess.GetValue<int>(row, "PalletCapacity");
                        objSo.InnerBoxCapacity = FloorDBAccess.GetValue<int>(row, "InnerBoxCapacity");
                        objSo.CustomerLotNumber = FloorDBAccess.GetString(row, "CustomerLotNumber");
                        objSo.ItemName = FloorDBAccess.GetString(row, "ItemName");
                        objSo.ManufacturingOrderBasis = FloorDBAccess.GetValue<int>(row, "MANUFACTURINGDATEBASIS");
                        objSo.POStatus = FloorDBAccess.GetValue<int>(row, "POStatus");
                        objSo.InnerProductCode = FloorDBAccess.GetString(row, "InnerProductCode");
                        objSo.OuterProductCode = FloorDBAccess.GetString(row, "OuterProductCode");
                        //objSo.BrandName = FloorDBAccess.GetString(row, "BrandName");
                        objSo.Expiry = FloorDBAccess.GetValue<int>(row, "Expiry");
                        objSo.Reference1 = FloorDBAccess.GetString(row, "ProductReferenceNumber");
                        objSo.Reference2 = FloorDBAccess.GetString(row, "Reference2"); // Added by HSB Floor System upgrade
                        objSo.SpecialInnerCode = FloorDBAccess.GetValue<int>(row, "SpecialInnerCode");
                        objSo.SpecialInnerCodeCharacter = FloorDBAccess.GetString(row, "SpecialInnerCodeCharacter");
                        objSo.GCLabelPrintingRequired = FloorDBAccess.GetValue<int>(row, "GCLabelPrintingRequired");
                        objSo.AlternateGloveCode1 = FloorDBAccess.GetString(row, "AlternateGloveCode1");
                        objSo.AlternateGloveCode2 = FloorDBAccess.GetString(row, "AlternateGloveCode2");
                        objSo.AlternateGloveCode3 = FloorDBAccess.GetString(row, "AlternateGloveCode3");
                        objSo.OrderNumber = FloorDBAccess.GetString(row, "OrderNumber");
                        objSo.CustomerRefernceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber");
                        objSo.SHIPPINGDATEREQUESTED = FloorDBAccess.GetValue<DateTime>(row, "SHIPPINGDATEREQUESTED");
                        //objSo.BARCODE = FloorDBAccess.GetString(row, "BARCODE");
                        //objSo.BARCODEOUTERBOX = FloorDBAccess.GetString(row, "BARCODEOUTERBOX");
                    }
                }
                return objSo;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
            //usp_FP_SolineData_Get
        }

        /// <summary>
        /// Get the Purchase Order Item cases of internalLotNumber
        /// </summary>
        /// <param name="internalLotNumber">Internal LotNumber</param>
        /// <returns>return list of Case number wih pallet id for Printing</returns>
        public static List<PurchaseOrderItemDTO> GetPurchaseOrderItemCases(string internalLotNumber, string poNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@InternalLotNumber", internalLotNumber));
                lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
                DataTable dInternalLoNumberPurchaseOrderItemCases = FloorDBAccess.ExecuteDataTable("usp_FP_PurchaseOrderItemCases_Get", lstFP);
                List<PurchaseOrderItemDTO> lstPurchaseOrderItem = (from DataRow row in dInternalLoNumberPurchaseOrderItemCases.Rows
                                                                   select new PurchaseOrderItemDTO
                                                                   {
                                                                       CaseNumber = FloorDBAccess.GetValue<int>(row, "CaseNumber"),
                                                                       isPreshipment = FloorDBAccess.GetValue<Boolean>(row, "isPreshipmentCase"),
                                                                       PalletId = FloorDBAccess.GetString(row, "PalletId")
                                                                   }).ToList();
                return lstPurchaseOrderItem;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }


        public static PurchaseOrderItemDTO GetPurchaseOrderItemCasesForSecondGrade(string internalLotNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            PurchaseOrderItemDTO objPurchaseOrderItemDTO = new PurchaseOrderItemDTO();
            try
            {
                lstFP.Add(new FloorSqlParameter("@InternalLotNumber", internalLotNumber));
                DataTable dInternalLoNumberPurchaseOrderItemCases = FloorDBAccess.ExecuteDataTable("usp_FP_PurchaseOrderItemCases_Get_SecondGrade", lstFP);

                if (dInternalLoNumberPurchaseOrderItemCases.Rows.Count > 0)
                {
                    objPurchaseOrderItemDTO = new PurchaseOrderItemDTO();

                    DataRow row = dInternalLoNumberPurchaseOrderItemCases.Rows[0];
                    objPurchaseOrderItemDTO.StartCaseNumber = FloorDBAccess.GetValue<int>(row, "StartCaseNumber");
                    objPurchaseOrderItemDTO.EndCaseNumber = FloorDBAccess.GetValue<int>(row, "EndCaseNumber");
                }
                return objPurchaseOrderItemDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }


        /// <summary>
        /// Get the Purchase Order Item cases for reprint
        /// </summary>
        /// <param name="ponumber">Purchase Order</param>
        /// <param name="itemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <param name="startCase">Start Case</param>
        /// <param name="endcase">End Case</param>
        /// <param name="InternalLotNumber">Internal Lot Number</param>
        /// <returns>return list of Case number wih pallet id for Printing for reprint</returns>
        public static List<PurchaseOrderItemDTO> GetPurchaseOderItemCaseforReprint(string ponumber, string itemNumber, string size, int startCase, int endcase, string _InternalLotNo = null)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            try
            {
                lstFP.Add(new FloorSqlParameter("@PoNumber", ponumber));
                lstFP.Add(new FloorSqlParameter("@itemNumber", itemNumber));
                lstFP.Add(new FloorSqlParameter("@size", size));
                lstFP.Add(new FloorSqlParameter("@StartCase", startCase));
                lstFP.Add(new FloorSqlParameter("@EndCase", endcase));
                lstFP.Add(new FloorSqlParameter("@InternalLotNumber", _InternalLotNo));

                DataTable dInternalLoNumberPurchaseOrderItemCases = FloorDBAccess.ExecuteDataTable("usp_FP_PurchaseOrderItemCasesForReprint_Get", lstFP);
                List<PurchaseOrderItemDTO> lstPurchaseOrderItem = (from DataRow row in dInternalLoNumberPurchaseOrderItemCases.Rows
                                                                   select new PurchaseOrderItemDTO
                                                                   {
                                                                       CaseNumber = FloorDBAccess.GetValue<int>(row, "CaseNumber"),
                                                                       isPreshipment = FloorDBAccess.GetValue<Boolean>(row, "isPreshipmentCase"),
                                                                       PalletId = FloorDBAccess.GetString(row, "PalletId"),
                                                                       internallotnumber = FloorDBAccess.GetString(row, "internalotnumber")
                                                                   }).ToList();
                return lstPurchaseOrderItem;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }

        #endregion

        # region Completed Pallet Slip
        /// <summary>
        /// Pallet Geneaion Reports
        /// </summary>
        /// <param name="plant">Plant</param>
        /// <param name="ponumber">PO Number</param>
        /// <param name="Itemnumber">Iem Number</param>
        /// <param name="size">Size</param>
        /// <param name="palletId">Pallet Id</param>
        /// <returns>FGT slip data</returns>
        public static PalletInfoDTO GetPalletInfo(string plant, string ponumber, string Itemnumber, string size, string palletId)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            PalletInfoDTO objPalletInfoDTO = null;
            lstFP.Add(new FloorSqlParameter("@PalletId", palletId));
            lstFP.Add(new FloorSqlParameter("@PONumber", ponumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", Itemnumber));
            lstFP.Add(new FloorSqlParameter("@Size", size));
            lstFP.Add(new FloorSqlParameter("@LocationId", plant));
            DataTable dtPalletInfo = FloorDBAccess.ExecuteDataTable("USP_FP_GET_PalletInformation", lstFP);
            try
            {
                if (dtPalletInfo.Rows.Count > 0)
                {
                    objPalletInfoDTO = new PalletInfoDTO();
                    DataRow row = dtPalletInfo.Rows[0];
                    objPalletInfoDTO.locationId = FloorDBAccess.GetValue<int>(row, "LocationId");
                    objPalletInfoDTO.Workstationnumber = FloorDBAccess.GetString(row, "WorkStationName");
                    objPalletInfoDTO.OperatorId = FloorDBAccess.GetString(row, "OperatorId");
                    objPalletInfoDTO.Ponumber = FloorDBAccess.GetString(row, "PONumber");
                    objPalletInfoDTO.ItemName = FloorDBAccess.GetString(row, "ItemName");
                    objPalletInfoDTO.Size = FloorDBAccess.GetString(row, "Size");
                    objPalletInfoDTO.Palletid = FloorDBAccess.GetString(row, "PalletId");
                    objPalletInfoDTO.Casespacked = FloorDBAccess.GetValue<int>(row, "CaseCount");
                    //objPalletInfoDTO.Name = FloorDBAccess.GetString(row, "Name");
                    objPalletInfoDTO.Packdate = FloorDBAccess.GetString(row, "PackDate");
                    objPalletInfoDTO.CaseList = FloorDBAccess.GetString(row, "CaseList");
                    objPalletInfoDTO.Isavailable = FloorDBAccess.GetValue<Boolean>(row, "isavailable");
                    objPalletInfoDTO.ispreshipmentCase = FloorDBAccess.GetValue<Boolean>(row, "ispreshipmentcase");
                    objPalletInfoDTO.orderNumber = FloorDBAccess.GetString(row, "ordernumber");
                    objPalletInfoDTO.ItemNumber = FloorDBAccess.GetString(row, "ItemNumber");
                    if (objPalletInfoDTO.ispreshipmentCase)
                        objPalletInfoDTO.Size = FloorDBAccess.GetString(row, "sizeList");
                }
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.REASON_EXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
            return objPalletInfoDTO;

        }

        /// <summary>
        ///Gets Pallet Details based on PalletId
        /// </summary>
        /// <param name="palletId">Pallet Id</param>
        /// <returns>Pallet Info</returns>
        public static PalletInfoDTO GetPalletInfoByPalletId(string palletId)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            PalletInfoDTO objPalletInfoDTO = null;
            lstFP.Add(new FloorSqlParameter("@PalletId", palletId));
            DataTable dtPalletInfo = FloorDBAccess.ExecuteDataTable("usp_FP_GetPalletDetailsByPalletId", lstFP);
            try
            {
                if (dtPalletInfo.Rows.Count > 0)
                {
                    objPalletInfoDTO = new PalletInfoDTO();
                    DataRow row = dtPalletInfo.Rows[0];
                    objPalletInfoDTO.CustomerRefPo = string.IsNullOrEmpty(FloorDBAccess.GetString(row, "CustomerReferenceNumber")) ? FloorDBAccess.GetString(row, "PoNumber") : FloorDBAccess.GetString(row, "CustomerReferenceNumber") + " | " + FloorDBAccess.GetString(row, "PoNumber");
                    objPalletInfoDTO.Ponumber = FloorDBAccess.GetString(row, "PoNumber");
                    objPalletInfoDTO.ItemNumber = FloorDBAccess.GetString(row, "ItemNumber");
                    objPalletInfoDTO.Size = FloorDBAccess.GetString(row, "ItemSize");
                    objPalletInfoDTO.Location = FloorDBAccess.GetString(row, "LocationName");
                    objPalletInfoDTO.locationId = FloorDBAccess.GetValue<int>(row, "LocationId");
                    objPalletInfoDTO.ispreshipmentCase = FloorDBAccess.GetValue<Boolean>(row, "IsPreshipment");
                }
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.REASON_EXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
            return objPalletInfoDTO;

        }


        /// <summary>
        /// Get Location Details
        /// </summary>
        /// <returns>return Location Details</returns>
        public static List<DropdownDTO> GetLocationDetails()
        {
            List<DropdownDTO> list = new List<DropdownDTO>();
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_getLocationData"))
            {
                try
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
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.PRESHIPMENTPALLET_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }
        /// <summary>
        /// Get Pallet Id's involved in Transactions
        /// </summary>
        /// <returns>Return List of Pallet Id</returns>
        public static List<string> GetPalletCompletionSlipPalletIdList()
        {
            List<string> list = new List<string>();
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_FP_SELECT_PalletCompletionSlipID"))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = (from DataRow row in dt.Rows
                                select FloorDBAccess.GetString(row, "PalletId")).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.PRESHIPMENTPALLET_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }
        /// <summary>
        /// Get Inplace Purchase Orders
        /// </summary>
        /// <returns>list of Purchase Order details</returns>
        public static List<SOLineDTO> GetInplacePurchaseOrderList()
        {
            List<SOLineDTO> lstSOLine = new List<SOLineDTO>();
            using (DataTable dtPOlist = FloorDBAccess.ExecuteDataTable("usp_FP_SELECT_InplacePurchaseOrder"))
            {
                try
                {
                    if (dtPOlist != null && dtPOlist.Rows.Count != 0)
                    {
                        lstSOLine = (from DataRow row in dtPOlist.Rows
                                     select new SOLineDTO
                                     {
                                         PONumber = FloorDBAccess.GetString(row, "PONumber"),
                                         ItemNumber = FloorDBAccess.GetString(row, "ItemNumber"),
                                         ItemSize = FloorDBAccess.GetString(row, "ItemSize"),
                                         CustomerReferenceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber")

                                     }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                }
                return lstSOLine;
            }
        }

        public static int UpdatePalletClosure(string palletid)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@Palletid", palletid));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_ClosePallet", lstFP);
        }

        public static void InsertFPFGTFLog(string palletid, string WorkStationId, int LocationId, string LastModifiedBy)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@Palletid", palletid));
            lstFP.Add(new FloorSqlParameter("@WorkStationId", Int32.Parse(WorkStationId)));
            lstFP.Add(new FloorSqlParameter("@LocationId", LocationId));
            lstFP.Add(new FloorSqlParameter("@LastModifiedBy", LastModifiedBy));

            int i = FloorDBAccess.ExecuteNonQuery("usp_FP_Insert_FPFGTFLog", lstFP);
        }

        # endregion

        #region Common Methods
        public static void GeneratePalletFile(string palletid, Boolean isPreshipment, string itemName, int casecapacity, string ponumber, string size)
        {
            //edit by Cheah (24Mar2017)
            //string path = FloorSystemConfiguration.GetInstance().strEWareNaviFolderLocation + palletid.Substring(4, 4) + ".txt";
            string path = "";


            // Commented by Yiksiu, 8digit pallet id implementation
            //if (palletid.Substring(0, 4) == "000F")
            //    path = FloorSystemConfiguration.GetInstance().strEWareNaviFolderLocation + palletid.Substring(4, 4) + ".txt";
            //else
            path = FloorSystemConfiguration.GetInstance().strEWareNaviFolderLocation + palletid + ".txt";

            //end edit
            StringBuilder sb = new StringBuilder();
            try
            {
                if (!File.Exists(path))
                {
                    // Create a file to write to. 
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        if (isPreshipment)
                        {
                            List<SOLineDTO> lstPreshipmentPalletDetails = FinalPackingBLL.GetPreshipmentPalletEwarenaviTextFile(palletid, ponumber, itemName);
                            foreach (SOLineDTO preshipement in lstPreshipmentPalletDetails)
                            {
                                sb.Append(Constants.ZERO + Constants.COMMA);
                                sb.Append(string.Format("{0}{1}{2}", preshipement.ItemNumber, Constants.UNDERSCORE, preshipement.ItemSize) + Constants.COMMA);
                                sb.Append(preshipement.PONumber + Constants.COMMA);
                                sb.Append(preshipement.CaseCapacity + Constants.COMMA + Constants.THREE);
                                sb.AppendLine(Environment.NewLine);
                            }
                            sw.WriteLine(sb.ToString());
                        }
                        else
                        {
                            sb.Append(Constants.ZERO + Constants.COMMA);
                            sb.Append(string.Format("{0}{1}{2}", itemName, Constants.UNDERSCORE, size) + Constants.COMMA);
                            sb.Append(ponumber + Constants.COMMA);
                            sb.Append(casecapacity + Constants.COMMA);
                            sw.WriteLine(sb.ToString() + Constants.ONE);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {

            }
        }

        //add by Cheah (17Aug2017)
        /// <summary>
        /// Adds data to EWN_CompletedPallet table. QAPassed is always 1 as print will only run if QAI result / QI result is available.
        /// </summary>
        /// <param name="palletid"></param>
        /// <param name="isPreshipment"></param>
        /// <param name="itemName"></param>
        /// <param name="casecapacity"></param>
        /// <param name="ponumber"></param>
        /// <returns> result of SQL ExecuteNonQuery</returns>
        public static int InsertEWareNaviData(string palletid, Boolean isPreshipment, string itemName, int casecapacity, string ponumber, string ItemSize, string CustomerSize)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            int result = 0;

            if (isPreshipment)
            {
                List<SOLineDTO> lstPreshipmentPalletDetails = FinalPackingBLL.GetPreshipmentPalletEwarenaviTextFileWithCustomerSize(palletid, ponumber, itemName);
                foreach (SOLineDTO preshipement in lstPreshipmentPalletDetails)
                {
                    lstFP.Add(new FloorSqlParameter("@QAPassed", 3));
                    lstFP.Add(new FloorSqlParameter("@Item", preshipement.ItemNumber + Constants.UNDERSCORE + preshipement.CustomerSize));
                    lstFP.Add(new FloorSqlParameter("@PONumber", preshipement.PONumber));
                    lstFP.Add(new FloorSqlParameter("@Qty", preshipement.CaseCapacity));
                    lstFP.Add(new FloorSqlParameter("@PalletID", palletid));
                    lstFP.Add(new FloorSqlParameter("@DateComplete", DateTime.Now));
                    lstFP.Add(new FloorSqlParameter("@DateStockOut", DBNull.Value));
                    //New column FGCodeAndSize (HartalegaCommonSize) for IOT scan, original Item (CustomerSize) field for EWN and lifter scan.
                    lstFP.Add(new FloorSqlParameter("@FGCodeAndSize", preshipement.ItemNumber + Constants.UNDERSCORE + preshipement.ItemSize));

                    result = FloorDBAccess.ExecuteNonQuery("USP_EWN_InsertPalletData", lstFP);
                    lstFP.Clear();
                }
            }
            else
            {
                lstFP.Add(new FloorSqlParameter("@QAPassed", 1));
                lstFP.Add(new FloorSqlParameter("@Item", itemName + Constants.UNDERSCORE + CustomerSize));
                lstFP.Add(new FloorSqlParameter("@PONumber", ponumber));
                lstFP.Add(new FloorSqlParameter("@Qty", casecapacity));
                lstFP.Add(new FloorSqlParameter("@PalletID", palletid));
                lstFP.Add(new FloorSqlParameter("@DateComplete", DateTime.Now));
                lstFP.Add(new FloorSqlParameter("@DateStockOut", DBNull.Value));
                //New column FGCodeAndSize (HartalegaCommonSize) for IOT scan, original Item (CustomerSize) field for EWN and lifter scan.
                lstFP.Add(new FloorSqlParameter("@FGCodeAndSize", itemName + Constants.UNDERSCORE + ItemSize));

                result = FloorDBAccess.ExecuteNonQuery("USP_EWN_InsertPalletData", lstFP);
            }
            return result;
        }
        //end add (17Aug2017)

        public static string GetItemType(string poNumber, string ItemNumber, string size)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PONumber", poNumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
            lstFP.Add(new FloorSqlParameter("@itemSize", size));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("usp_FP_GetItemType", lstFP));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalLotNumber"></param>
        /// <param name="isGCLabel"></param>
        /// <param name="isLotVerificationRequired"></param>
        /// <returns></returns>
        public static OuterLabelDTO PrintInnerandOuter(string internalLotNumber, Boolean isGCLabel = false, Boolean isLotVerificationRequired = false, string itemType = null)
        {
            return FinalPackingPrint.LabelPrint(internalLotNumber, isGCLabel, isLotVerificationRequired, itemType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalLotNumber"></param>
        /// <param name="ManufacturingDate"></param>
        /// <param name="expirydate"></param>
        /// <param name="line"></param>
        /// <param name="expiryinmonths"></param>
        /// <param name="innersetNumber"></param>
        /// <param name="customerLotNumber"></param>
        /// <returns></returns>
        public static string UpdatespeciallotnumberforInner(string internalLotNumber, DateTime ManufacturingDate, DateTime expirydate, string line,
            int expiryinmonths, string innersetNumber, string customerLotNumber, bool isSurgicalWorkOrder = false)
        {
            switch (innersetNumber)
            {
                #region old label set 25/07/19 LabelSetNamingProject
                case "133":
                case "193":
                case "181":
                case "182":
                case "199":
                case "210":
                case "192":
                case "213":
                case "222":
                case "236":
                case "241":
                case "243":
                case "246":
                case "245":
                case "225":
                case "233":
                case "265":
                case "264":
                case "273":
                case "LAB-IB-ANSL-0012":
                case "298":
                case "LAB-IB-IHCP-0002":
                case "299":
                case "LAB-IB-MCKU-0001":
                case "302":
                case "LAB-IB-IHCP-0003":
                case "305":
                case "LAB-IB-CDNL-0001":
                case "LAB-IB-CDNL-0002":
                case "312":
                case "313":
                case "LAB-IB-S2SG-0001":
                case "316":
                case "LAB-IB-MEDL-0010":
                case "317":
                case "LAB-IB-MEDL-0016":
                case "323":
                case "LAB-IB-MEDL-0017":
                case "328":
                case "LAB-IB-MCKU-0004":
                case "330":
                case "LAB-IB-CDNL-0003":
                case "324":
                case "LAB-IB-MEDL-0019":
                case "336":
                case "LAB-IB-MCKU-0006":
                case "337":
                case "LAB-IB-MEDL-0020":
                case "340":
                case "LAB-IB-VRDT-0002":
                case "341":
                case "LAB-IB-MEDL-0021":
                case "342":
                case "LAB-IB-IHCP-0004":
                case "348":
                case "LAB-IB-IHCP-0005":
                case "353":
                case "LAB-IB-VRDT-0003":
                case "354":
                case "LAB-IB-IHCP-0006":
                case "356":
                    InnerSetReturn objInnerSetReturn = FinalPackingPrint.ReturnInnerSet(internalLotNumber, ManufacturingDate, expirydate, line, expiryinmonths, customerLotNumber, innersetNumber);
                    if (objInnerSetReturn != null)
                    {
                        FinalPackingBLL.UpdateSpecialInternalLotNumber(internalLotNumber, objInnerSetReturn);

                        if (isSurgicalWorkOrder)
                            FinalPackingBLL.UpdateSpecialInternalLotNumberforSPP(internalLotNumber, objInnerSetReturn);

                    }
                    return objInnerSetReturn.specialInternalLotNumber;
                case "226":
                case "227":
                case "247":
                case "270":
                case "274":
                case "278":
                case "280":
                case "283":
                case "284":
                case "286":
                case "215":
                case "LAB-IB-HLYD-0007"://Halyard
                case "301": //Halyard
                case "LAB-IB-HLYD-0008"://Halyard
                case "303": //Halyard
                case "LAB-IB-HLYD-0009": //Halyard
                case "310": //Halyard
                case "LAB-IB-HLYD-0010": //Halyard
                case "311": //Halyard
                case "LAB-IB-HLYD-0011"://Halyard (V Batch)
                case "325": //Halyard (V Batch)
                case "LAB-IB-HLYD-0012"://Halyard (X Batch)
                case "332": //Halyard (X Batch)
                case "LAB-IB-HLYD-0013"://Halyard (V Batch)
                case "333": //Halyard (V Batch)
                case "LAB-IB-HLYD-0014"://Halyard (X Batch)
                case "334": //Halyard (X Batch)
                case "LAB-IB-HLYD-0015"://Halyard (X Batch)
                case "338": //Halyard (X Batch)
                case "LAB-IB-HLYD-0016"://Halyard (V Batch)
                case "339": //Halyard (V Batch)
                case "LAB-IB-HLYD-0017": //Halyard (X Batch)
                case "345":
                    InnerSetReturn objInnerSetReturn2 = FinalPackingPrint.ReturnInnerSet(internalLotNumber, ManufacturingDate, expirydate, line, expiryinmonths, customerLotNumber, innersetNumber);
                    if (objInnerSetReturn2 != null)
                    {
                        int lastRunningNo;
                        if (CheckDuplicateInternalLotNumber(objInnerSetReturn2.specialInternalLotNumber, WorkStationDataConfiguration.GetInstance().stationNumber, out lastRunningNo))
                        {
                            objInnerSetReturn2.specialInternalLotNumber = objInnerSetReturn2.specialInternalLotNumber.Remove((objInnerSetReturn2.specialInternalLotNumber.Trim().Length - 4), 4);
                            objInnerSetReturn2.specialInternalLotNumber += lastRunningNo.ToString().PadLeft(4, '0');
                        }

                        if (isSurgicalWorkOrder)
                            objInnerSetReturn2.specialInternalLotNumber = internalLotNumber; // not overwrite lot no for Surgical PO

                        FinalPackingBLL.UpdateSpecialInternalLotNumber(internalLotNumber, objInnerSetReturn2);

                        if (isSurgicalWorkOrder)
                            FinalPackingBLL.UpdateSpecialInternalLotNumberforSPP(internalLotNumber, objInnerSetReturn2);
                    }
                    return objInnerSetReturn2.specialInternalLotNumber;
                #endregion

                #region new label set 25/07/19 LabelSetNamingProject
                //formerly inner label set 133
                case "LAB-IB-BNZL-0001":
                //formerly inner label set 193
                case "LAB-IB-IHCP-0001":
                //formerly inner label set 182
                case "LAB-IB-HTLG-0037":
                //formerly inner label set 222
                case "LAB-IB-HTLG-0055":
                //formerly inner label set 241
                case "LAB-IB-GPTH-0001":
                //formerly inner label set 243
                case "LAB-IB-HTLG-0066":
                //formerly inner label set 265
                case "LAB-IB-VRDT-0001":
                //formerly inner label set 273
                case "LAB-IB-ANSL-0006":
                    InnerSetReturn objInnerSetReturnNewLabelSet = FinalPackingPrint.ReturnInnerSet(internalLotNumber, ManufacturingDate, expirydate, line, expiryinmonths, customerLotNumber, innersetNumber);
                    if (objInnerSetReturnNewLabelSet != null)
                    {
                        if(isSurgicalWorkOrder)
                            objInnerSetReturnNewLabelSet.specialInternalLotNumber = internalLotNumber; // not overwrite lot no for Surgical PO

                        FinalPackingBLL.UpdateSpecialInternalLotNumber(internalLotNumber, objInnerSetReturnNewLabelSet);
                    }
                    return objInnerSetReturnNewLabelSet.specialInternalLotNumber;
                //formerly inner label set 226
                case "LAB-IB-HLYD-0003":
                //formerly inner label set 227
                case "LAB-IB-HLYD-0004":
                //formerly inner label set 270
                case "LAB-IB-HLYD-0005":
                //formerly inner label set 274
                case "LAB-IB-HLYD-0006":
                //formerly inner label set 278
                case "LAB-IB-ANSL-0009":
                //formerly inner label set 280
                case "LAB-IB-HTLG-0081":
                //formerly inner label set 283
                case "LAB-IB-KCGS-0001":
                //formerly inner label set 284
                case "LAB-IB-KCGS-0002":
                //formerly inner label set 286
                case "LAB-IB-ANSL-0011":
                    InnerSetReturn objInnerSetReturn2NewLabelSet = FinalPackingPrint.ReturnInnerSet(internalLotNumber, ManufacturingDate, expirydate, line, expiryinmonths, customerLotNumber, innersetNumber);
                    if (objInnerSetReturn2NewLabelSet != null)
                    {
                        int lastRunningNo;
                        if (CheckDuplicateInternalLotNumber(objInnerSetReturn2NewLabelSet.specialInternalLotNumber, WorkStationDataConfiguration.GetInstance().stationNumber, out lastRunningNo))
                        {
                            objInnerSetReturn2NewLabelSet.specialInternalLotNumber = objInnerSetReturn2NewLabelSet.specialInternalLotNumber.Remove((objInnerSetReturn2NewLabelSet.specialInternalLotNumber.Trim().Length - 4), 4);
                            objInnerSetReturn2NewLabelSet.specialInternalLotNumber += lastRunningNo.ToString().PadLeft(4, '0');
                        }

                        FinalPackingBLL.UpdateSpecialInternalLotNumber(internalLotNumber, objInnerSetReturn2NewLabelSet);

                        if (isSurgicalWorkOrder)
                            FinalPackingBLL.UpdateSpecialInternalLotNumberforSPP(internalLotNumber, objInnerSetReturn2NewLabelSet);
                    }
                    return objInnerSetReturn2NewLabelSet.specialInternalLotNumber;
                #endregion
                default:
                    return string.Empty;
            }

        }
        /// <summary>
        /// UpdatespeciallotnumberforOuter
        /// </summary>
        /// <param name="internalLotNumber"></param>
        /// <param name="ManufacturingDate"></param>
        /// <param name="expirydate"></param>
        /// <param name="line"></param>
        /// <param name="expiryinmonths"></param>
        /// <param name="outersetNumber"></param>
        /// <param name="customerLotNumber"></param>
        /// <param name="SHIPPINGDATEREQUESTED"></param>
        public static void UpdatespeciallotnumberforOuter(string internalLotNumber, DateTime ManufacturingDate, DateTime expirydate,
           string line, int expiryinmonths, string outersetNumber, string customerLotNumber, DateTime? ManufacturingDateETD, string specialLotNumber, string innersetNumber, bool isSurgicalWorkOrder = false)
        {
            switch (outersetNumber)
            {
                #region old label set 25/07/19 LabelSetNamingProject
                case "114":
                case "96":
                case "122":
                case "73":
                case "85":
                case "90":
                case "152":
                case "136":
                case "158":
                case "172":
                case "173":
                case "169":
                case "183":
                case "141":
                case "150":
                case "193":
                case "206":
                case "209":
                case "228":
                case "234":
                case "LAB-OC-ANSL-0010": // old:234
                case "LAB-OC-ANSL-0011": // old:238
                case "238":
                case "LAB-OC-IHCP-0002":
                case "239":
                case "LAB-OC-ANSL-0012":
                case "240":
                case "LAB-OC-ANSL-0013":
                case "242":
                case "LAB-OC-ANSL-0015":
                case "249":
                case "LAB-OC-IHCP-0003":
                case "250":
                case "LAB-OC-ANSL-0016":
                case "257":
                case "LAB-OC-S2SG-0001":
                case "260":
                #endregion

                #region new label set 25/07/19 LabelSetNamingProject
                //formerly outer label set 96
                case "LAB-OC-HTLG-0065":
                //formerly outer label set 85
                case "LAB-OC-HTLG-0056":
                //formerly outer label set 90
                case "LAB-OC-HTLG-0059":
                //formerly outer label set 152
                case "LAB-OC-HTLG-0095":
                //formerly outer label set 136
                case "LAB-OC-HTLG-0086":
                //formerly outer label set 172
                case "LAB-OC-ANSL-0004":
                //formerly outer label set 173
                case "LAB-OC-ANSL-0005":
                //formerly outer label set 193
                case "LAB-OC-ANSL-0006":
                //formerly outer label print 206
                case "LAB-OC-ANSL-0009":
                    #endregion
                    InnerSetReturn objInnerSetReturn = FinalPackingPrint.ReturnOuterSet(internalLotNumber, ManufacturingDate,
                        expirydate, line, expiryinmonths, outersetNumber, customerLotNumber, ManufacturingDateETD.Value);
                    if (objInnerSetReturn != null)
                    {
                        FinalPackingBLL.UpdateSpecialInternalLotNumber(internalLotNumber, objInnerSetReturn);

                        if (isSurgicalWorkOrder)
                            FinalPackingBLL.UpdateSpecialInternalLotNumberforSPP(internalLotNumber, objInnerSetReturn);
                    }
                    break;
                default:
                    break;
            }

            // Update FinalPacking.OuterLotNo to sync with internallotnumber
            if (!string.IsNullOrEmpty(specialLotNumber))
            {
                switch (innersetNumber)
                {
                    #region old label set 25/07/19 LabelSetNamingProject
                    case "226": //Halyard
                    case "227": //Halyard
                    case "247": //Halyard
                    case "270": //Halyard
                    case "274": //Halyard
                    case "278":
                    case "280":
                    case "283":
                    case "284":
                    case "286":
                    case "215":
                    case "LAB-IB-ANSL-0012":
                    case "298":
                    case "LAB-IB-HLYD-0009": //Halyard
                    case "310": //Halyard
                    case "LAB-IB-HLYD-0010": //Halyard
                    case "311": //Halyard
                    //case "LAB-IB-KCGS-0003"://KIMBERLY CLARK (X Batch)
                    //case "326": //KIMBERLY CLARK (X Batch)
                    //case "LAB-IB-KCGS-0004"://KIMBERLY CLARK (V Batch)
                    //case "327": //KIMBERLY CLARK (V Batch)
                    case "LAB-IB-HLYD-0011"://Halyard (V Batch)
                    case "325": //Halyard (V Batch)
                    case "LAB-IB-HLYD-0012"://Halyard (X Batch)
                    case "332": //Halyard (X Batch)
                    case "LAB-IB-HLYD-0017": //Halyard (X Batch)
                    case "345":
                    #endregion

                    #region new label set 25/07/19 LabelSetNamingProject
                    //formerly inner label set 226
                    case "LAB-IB-HLYD-0003": //Halyard
                    //formerly inner label set 227
                    case "LAB-IB-HLYD-0004": //Halyard
                    //formerly inner label set 278
                    case "LAB-IB-ANSL-0009":
                    //formerly inner label set 280
                    case "LAB-IB-HTLG-0081":
                    //formerly inner label set 283
                    case "LAB-IB-KCGS-0001":
                    //formerly inner label set 284
                    case "LAB-IB-KCGS-0002":
                    //formerly inner label set 286
                    case "LAB-IB-ANSL-0011":
                    //formerly inner label set 215
                    case "LAB-OC-HTLG-0132":
                        #endregion
                        UpdateSpecialInternalLotNumberForOuterLotNo(specialLotNumber);
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Update Barcode to validate
        /// </summary>
        /// <param name="barcodeToValidate"></param>
        /// <param name="countToValidate"></param>
        /// <param name="internalLotnumber"></param>
        public static int UpdateBarcodeToValidate(string barcodeToValidate, int countToValidate, string internalLotnumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@Barcodetovalidate", barcodeToValidate));
            lstFP.Add(new FloorSqlParameter("@counttovalidate", countToValidate));
            lstFP.Add(new FloorSqlParameter("@internallotnumber", internalLotnumber));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_UpdateBarcodetoValidate", lstFP);
        }

        /// <summary>
        /// #AZ 01/03/2022 Get Glove Remaining Qty based on FGBO and Size
        /// </summary>
        /// <param name="FGBatchOrder"></param>
        /// <param name="size"></param>
        /// <returns>List<BatchOrderDetailsDTO></returns>
        public static List<GloveBatchOrderDTO> GetFGBORemainingQty(string FGBatchOrder, string size)
        {
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            List<GloveBatchOrderDTO> lstBatchOrderDetailsDTO = new List<GloveBatchOrderDTO>();
            prmList.Add(new FloorSqlParameter("@BatchOrderId", FGBatchOrder));
            prmList.Add(new FloorSqlParameter("@Size", size));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_FGBORemainingQty_Get", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            GloveBatchOrderDTO lnbodDTO = new GloveBatchOrderDTO();
                            lnbodDTO.ProdStatus = FloorDBAccess.GetString(dr, "ProdStatus");
                            lnbodDTO.BthOrderId = FloorDBAccess.GetString(dr, "BthOrderId");
                            lnbodDTO.ItemId = FloorDBAccess.GetString(dr, "ItemId");
                            lnbodDTO.Size = FloorDBAccess.GetString(dr, "Size");
                            lnbodDTO.QtySched = FloorDBAccess.GetValue<decimal>(dr, "QtySched").ToString("#,##0");
                            lnbodDTO.ReportedQty = FloorDBAccess.GetValue<Int32>(dr, "FGReportedQty").ToString("#,##0");
                            lnbodDTO.RemainingQty = FloorDBAccess.GetValue<decimal>(dr, "RemainingQty").ToString("#,##0");
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

        public static string GetLotCartonRange(string internallotnumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internallotnumber", internallotnumber));

            List<string> list = new List<string>();
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_FP_GetInternalLotnumberCaseRange", lstFP))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            list.Add(FloorDBAccess.GetString(row, "casenumber").PadLeft(5, '0'));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return string.Join(Constants.COMMA, list.ToArray());

        }

        public static void SendPreshipmentQAEmail(string ponumber, string purchaseorderFormnum, string itemNumber, string itemName, string Size, string internalLotnumber, string preshipementPalletid)
        {
            string fromAddress = FloorSystemConfiguration.GetInstance().strHartalegaAlertEmail;
            string toAddress = FloorSystemConfiguration.GetInstance().strQAPreshipmentEmailAddress;
            string hostName = FloorSystemConfiguration.GetInstance().strSMTPSender;

            List<SOLineDTO> lstPurchaseorderitem = FinalPackingBLL.GetPOPreshipmentcases(ponumber);
            string emailSubject = string.Format(Messages.PRESHIPMENTQASUBJECT, ponumber);
            string emailBody = string.Empty;
            emailBody = GetPreshipmentQAEmailBodyContent(ponumber, purchaseorderFormnum, preshipementPalletid, itemName, WorkStationDTO.GetInstance().Location, lstPurchaseorderitem);
            try
            {
                Email.SendEmail(fromAddress, toAddress, string.Empty, string.Empty,
                   emailSubject, emailBody, true, System.Net.Mail.MailPriority.High, string.Empty,
                   hostName, 0, FloorSystemConfiguration.GetInstance().strEmailUserId, FloorSystemConfiguration.GetInstance().strEmailPassword);
                UpdatePresshipmentQAEmailSent(ponumber);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException("Exception occured while sending email to PreshipmentQA.", Constants.BUSINESSLOGIC, ex);
            }
        }

        public static void LotVerificationEmailtoPlantManager(string poNumber, string purchaseOrderFormNum, string itemName, string size,
            string internalLotnUmber, string cartonRange, string scannerRead)
        {
            string fromAddress = FloorSystemConfiguration.GetInstance().strHartalegaAlertEmail;
            string toAddress = WorkStationDataConfiguration.GetInstance().strPlantManagerEmailAddress;
            string hostName = FloorSystemConfiguration.GetInstance().strSMTPSender;
            string emailSubject = string.Format(Messages.LotVerificationSubject, WorkStationDTO.GetInstance().WorkStationNumber);
            string emailBody = string.Empty;
            emailBody = GetLotVerificationFailBodyContent(poNumber, purchaseOrderFormNum, itemName, size, internalLotnUmber, cartonRange, scannerRead, WorkStationDTO.GetInstance().Location);
            try
            {
                Email.SendEmail(fromAddress, toAddress, string.Empty, string.Empty,
                   emailSubject, emailBody, true, System.Net.Mail.MailPriority.High, string.Empty,
                   hostName, 0, FloorSystemConfiguration.GetInstance().strEmailUserId, FloorSystemConfiguration.GetInstance().strEmailPassword);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException("Exception occured while sending email to PreshipmentQA.", Constants.BUSINESSLOGIC, ex);
            }
        }

        public static string GetLotVerificationFailBodyContent(string poNumber, string purchaseOrderFormNum, string itemName, string size, string internalLotnUmber, string cartonRange, string scannerRead, string plantNo)
        {
            StringBuilder stremailbody = new StringBuilder("<html><body>");
            stremailbody.AppendLine(string.Format("PO : {0}", purchaseOrderFormNum));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Brand : {0}", itemName));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("SO : {0}", poNumber));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Size : {0}", size));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Lot : {0}", internalLotnUmber));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Ctn Range : {0}", cartonRange));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Scanner Read : {0}", scannerRead));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Plant: {0}", plantNo));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format(Messages.EMAILFOOTER));
            stremailbody.AppendLine("</body></html>");
            return stremailbody.ToString();
        }

        public static bool PehaLotVerificationEmailtoPlantManager(string poNumber, string itemNumber, string size,
            string internalLotnUmber, int carton, string ValidLotNumber, string scanner1Read, string scanner2Read, string scanner3Read, bool IsSC1Mismatch, bool IsSC2Mismatch, bool IsSC3Mismatch)
        {
            string fromAddress = FloorSystemConfiguration.GetInstance().strHartalegaAlertEmail;
            string toAddress = WorkStationDataConfiguration.GetInstance().strPlantManagerEmailAddress;
            string hostName = FloorSystemConfiguration.GetInstance().strSMTPSender;
            string emailSubject = string.Format(Messages.LotVerificationSubject, WorkStationDTO.GetInstance().WorkStationNumber);
            string emailBody = string.Empty;
            bool IsSent = false;
            emailBody = GetPehaLotVerificationFailBodyContent(poNumber, itemNumber, size, internalLotnUmber, carton,
                ValidLotNumber, scanner1Read, scanner2Read, scanner3Read, WorkStationDTO.GetInstance().Location, IsSC1Mismatch, IsSC2Mismatch, IsSC3Mismatch);
            try
            {
                Email.SendEmail(fromAddress, toAddress, string.Empty, string.Empty,
                   emailSubject, emailBody, true, System.Net.Mail.MailPriority.High, string.Empty,
                   hostName, 0, FloorSystemConfiguration.GetInstance().strEmailUserId, FloorSystemConfiguration.GetInstance().strEmailPassword);
                IsSent = true;
            }
            catch (Exception ex)
            {
                IsSent = false;
                throw new FloorSystemException("Exception occured while sending email.", Constants.BUSINESSLOGIC, ex);
            }
            return IsSent;
        }

        public static string GetPehaLotVerificationFailBodyContent(string poNumber, string itemNumber, string size, string internalLotnUmber, int carton,
            string validLotNumber, string scanner1Read, string scanner2Read, string scanner3Read, string plantNo, bool IsSC1Mismatch, bool IsSC2Mismatch, bool IsSC3Mismatch)
        {
            StringBuilder stremailbody = new StringBuilder("<html><body>");
            stremailbody.AppendLine(string.Format("PO : {0}", poNumber));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Item Number : {0}", itemNumber));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Size : {0}", size));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Lot : {0}", internalLotnUmber));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Ctn : {0}", carton));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Valid Lot Number : {0}", validLotNumber));
            stremailbody.AppendLine("<br/>");
            if (IsSC1Mismatch)
            {
                stremailbody.AppendLine(string.Format("Scanner 1 Read : {0}", scanner1Read));
                stremailbody.AppendLine("<br/>");
            }
            if (IsSC2Mismatch)
            {
                stremailbody.AppendLine(string.Format("Scanner 2 Read : {0}", scanner2Read));
                stremailbody.AppendLine("<br/>");
            }
            if (IsSC3Mismatch)
            {
                stremailbody.AppendLine(string.Format("Scanner 3 Read : {0}", scanner3Read));
                stremailbody.AppendLine("<br/>");
            }

            stremailbody.AppendLine(string.Format("Plant: {0}", plantNo));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format(Messages.EMAILFOOTER));
            stremailbody.AppendLine("</body></html>");
            return stremailbody.ToString();
        }

        public static string GetPreshipmentQAEmailBodyContent(string poNumber, string purchaseOrderFormNum, string preshipmentPalletId, string itemName, string plantNo, List<SOLineDTO> lstpreshipmentdetails)
        {
            StringBuilder stremailbody = new StringBuilder("<html><body>");
            stremailbody.AppendLine(string.Format("PO : {0}", purchaseOrderFormNum));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Brand : {0}", itemName));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("SO : {0}", poNumber));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("PalletID : {0}", preshipmentPalletId));
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format("Plant: {0}", plantNo));
            stremailbody.AppendLine("<br/>");
            foreach (SOLineDTO soPreshipment in lstpreshipmentdetails)
            {
                stremailbody.AppendLine(string.Format("Size: {0}", soPreshipment.ItemSize));
                stremailbody.AppendLine("<br/>");
                stremailbody.AppendLine(string.Format("Ctn Range: {0}", soPreshipment.Preshipmentcases));
                stremailbody.AppendLine("<br/>");
            }
            stremailbody.AppendLine("<br/>");
            stremailbody.AppendLine(string.Format(Messages.EMAILFOOTER));
            stremailbody.AppendLine("</body></html>");
            return stremailbody.ToString();
        }

        public static string CreateXML(Object BatchInfo)
        {
            XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
            // Initializes a new instance of the XmlDocument class.          
            XmlSerializer xmlSerializer = new XmlSerializer(BatchInfo.GetType());
            // Creates a stream whose backing store is memory. 
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, BatchInfo);
                xmlStream.Position = 0;
                //Loads the XML document from the specified string.
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }

        //KahHeng 29jan2019 added 2 new parameter for PODate  POReceviedDate 
        public static DateTime? GetManufacturingDate(int manufacturingOrderBasis, DateTime? ManufacturingDateETD, DateTime? ShippingDateETD, DateTime? batchCardProductionDate, DateTime? HSB_CustPODocumentDate, DateTime? HSB_CustPORecvDate, DateTime FirstManufacturingDate)
        {
            DateTime? manufacturingdate = new DateTime();

            switch (manufacturingOrderBasis)
            {
                /*
                 *  0 -> Packing Date
                 *  1 -> Production Date
                 *  2 -> Shipment ETD (SalesTable.ETD)
                 *  3 -> SO By Manufacturing ETD (SalesTable.ManufacturingDate)
                 *  4 -> Customer Production Order Document Date
                 *  5 -> First Carton Packing Date
                */

                case Constants.ZERO:
                    manufacturingdate = CommonBLL.GetCurrentDateAndTimeFromServer();
                    break;
                case Constants.ONE:
                    manufacturingdate = batchCardProductionDate;
                    break;
                case Constants.TWO:
                    manufacturingdate = ShippingDateETD;
                    break;
                case Constants.THREE:
                    manufacturingdate = ManufacturingDateETD;
                    break;
                case Constants.FOUR:
                    manufacturingdate = HSB_CustPODocumentDate;
                    break;
                //Pang 09/02/2020 First Carton Packing Date
                case Constants.FIVE:
                    manufacturingdate = FirstManufacturingDate; //First Carton Packing Date
                    break;
                    //END
            }
            return manufacturingdate;
        }
        /// <summary>
        /// Return the Rounded value based on the case capacity.
        /// </summary>
        /// <param name="boxesentered">Boxes entered</param>
        /// <returns>returns boxes rounded to case capacity</returns>
        public static string RoundToCaseCapacity(string boxesentered, int casecapacity)
        {
            try
            {
                if (casecapacity != Constants.ZERO)
                    return Convert.ToString(decimal.Truncate((Convert.ToDecimal(boxesentered) / casecapacity)) * casecapacity);
                else
                    return boxesentered;
            }
            catch (ArgumentException argEx)
            {
                throw new FloorSystemException("Argument Exception Occured while Rounding the boxes entered.", Constants.BUSINESSLOGIC, argEx);
            }
            catch (OverflowException ovfEx)
            {
                throw new FloorSystemException("OverFlow Exception Occured while Rounding the boxes entered.", Constants.BUSINESSLOGIC, ovfEx);

            }
        }

        public static string ValidateSerialNoByQIStatus(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNumber", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_SerialNo_QI", lstParameters));
        }

        // 20200808: Azrul FP Exemption Glove Process
        public static string ValidateSerialNoByFPExempt(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNumber", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_DOT_FPExempt_SNValidation", lstParameters));
        }

        public static FP_ExemptValidateDTO ValidateSerialNoByFPExemptProductionDateValidation(decimal serialNo, string FGCode)
        {
            FP_ExemptValidateDTO data = new FP_ExemptValidateDTO();
            DataTable dt;

            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNumber", serialNo));
            lstParameters.Add(new FloorSqlParameter("@FGCode", FGCode));  

            using (dt = FloorDBAccess.ExecuteDataTable("USP_FPExempt_ProductionDateValidation", lstParameters))
            {
                if (dt.Rows.Count == 0)
                    data = null;
                else
                {
                    DataRow row = dt.Rows[0];
                    data.Result = FloorDBAccess.GetString(row, "Result");
                    data.ProductionDateValidationDays = FloorDBAccess.GetValue<Int32>(row, "ProductionDateValidationDays");
                    data.ProductionDateValidationCustomer = FloorDBAccess.GetString(row, "ProductionDateValidationCustomer");                 
                }
            }

            return data;
        }

        public static string ValidateSerialNoByHotBox(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_FP_SEL_HotBoxResult", lstParameters));
        }

        public static int ValidateHotbox(string SerialNo)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@SerialNo", SerialNo));
            int nFromDB = Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_ValidateHotbox", lstfsp));
            return nFromDB;
        }

        public static string ValidateSerialNoByPowder(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_FP_SEL_PowderResult", lstParameters));
        }

        public static string ValidateSerialNoByProtein(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_FP_SEL_ProteinResult", lstParameters));
        }

        public static string ValidateSerialNoByPolymer(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_FP_SEL_PolymerResult", lstParameters));
        }

        public static string ValidateQATestResult(decimal serialNo, string testName)
        {
            string testResult = string.Empty;
            switch (testName)
            {
                case Constants.HOTBOX_TEST:
                    testResult = ValidateSerialNoByHotBox(serialNo);
                    break;
                case Constants.POWDER_TEST:
                    testResult = ValidateSerialNoByPowder(serialNo);
                    break;
                case Constants.PROTEIN_TEST:
                    testResult = ValidateSerialNoByProtein(serialNo);
                    break;
                case Constants.POLYMER_TEST:
                    testResult = ValidateSerialNoByPolymer(serialNo);
                    break;
            }
            return testResult;
        }

        public static int ManualPalletResetLog(string palletId)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PalletId", palletId));
            return FloorDBAccess.ExecuteNonQuery("USP_SAV_ManualPalletResetLog", lstFP);
        }

        public static Boolean ManualPalletResetPalletFlag(string palletId)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PalletId", palletId));
            int ret = Convert.ToInt32(FloorDBAccess.ExecuteNonQuery("USP_GET_ManualPalletReset_PalletAvailableFlag", lstFP));
            return ret == Constants.ONE;
        }

        /// <summary>
        /// Get Pallet Id's involved in Transactions
        /// </summary>
        /// <returns>Return List of Pallet Id</returns>
        public static List<string> GetCompletedPalletIdList()
        {
            List<string> list = new List<string>();
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_FP_SEL_CompletedPalletList"))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = (from DataRow row in dt.Rows
                                select FloorDBAccess.GetString(row, "PalletId")).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.PRESHIPMENTPALLET_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        public static string GetQCTypeBySerial(decimal serialNo)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@SerialNo", serialNo));
            return FloorDBAccess.ExecuteScalar("usp_FP_GetQCTypeBySerial", lstFP).ToString();
        }


        // yiksiu SEP 2017 : Label Set Optimization
        public static string GetLabelSetDateFormat(string format)
        {
            /* To return date format string value
                1.	yyyy-MM     (2017-08) 
                2.	yyyy-MM-dd  (2017-08-01)
                3.	yy-MM-dd    (17-08-01)
                4.	MMM-yyyy    (Aug-2017) 
                5.	MM-yyyy     (08-2017)
                6.	dd-MM-yyyy  (01-08-2017) 
                7.	dd-MMM-yyyy (01-Aug-2017) 
             */
            string dateFormat = string.IsNullOrEmpty(format) ? string.Empty : format.ToLower();
            dateFormat = dateFormat.Replace('m', 'M');

            return dateFormat;
        }

        public static string GetCustomerSizeByPO(string PONumber, string ItemNumber, string Size)
        {
            string customerSize = null;
            List<DropdownDTO> list = new List<DropdownDTO>();
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PONumber", PONumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
            lstFP.Add(new FloorSqlParameter("@Size", Size));

            customerSize = Convert.ToString(FloorDBAccess.ExecuteScalar("usp_FP_GetCustomerSizeByPO", lstFP));
            return customerSize;
        }
        #endregion

        #region AX Posting methods


        //kahheng 26Apr2019
        //add new parameters
        public static int RollBackPrintInnerOuterTransaction(string internalLotNumber, string palletIDConcated, string preshipmentPalletIDConcated,
            string PONumber, string ItemNumber, string Itemsize, bool isContinuedPallet, bool isContinuedPreshipmentPallet,
            string createdBy, string remarks)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@InternalLotnumber", internalLotNumber));
            lstFP.Add(new FloorSqlParameter("@palletIDConcated", palletIDConcated));
            lstFP.Add(new FloorSqlParameter("@preshipmentPalletIDConcated", preshipmentPalletIDConcated));
            lstFP.Add(new FloorSqlParameter("@PONumber", PONumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
            lstFP.Add(new FloorSqlParameter("@ItemSize", Itemsize));
            lstFP.Add(new FloorSqlParameter("@isContinuedPallet", isContinuedPallet));
            lstFP.Add(new FloorSqlParameter("@isContinuedPreshipmentPallet", isContinuedPreshipmentPallet));
            lstFP.Add(new FloorSqlParameter("@CreatedBy", createdBy));
            lstFP.Add(new FloorSqlParameter("@Remarks", remarks));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_RollBackFPPrintInnerOuter", lstFP);
        }

        public static int DeleteChangeBatchCardData(string internalLotNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@InternalLotnumber", internalLotNumber));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_RollBackChangeBatchCardData", lstFP);

        }
        public static int DeleteTempPackData(string serialNumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@serialNumber", serialNumber));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_RollBackTempPackData", lstFP);

        }

        public static int RollBackPrintSurgicalInnerOuter(string internallotnumber)
        {
            //[usp_FP_RollBackFPPrintSurgicalInnerOuter]
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@InternalLotnumber", internallotnumber));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_RollBackFPPrintSurgicalInnerOuter", lstFP);
        }

        public static int RollBackPrintSurgicalInnerOuterV2(string internallotnumber)
        {
            //[usp_FP_RollBackFPPrintSurgicalInnerOuter]
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@InternalLotnumber", internallotnumber));
            return FloorDBAccess.ExecuteNonQuery("USP_FP_RollBackFPPrintSurgicalInnerOuterV2", lstFP);
        }

        public static int RollBackEWNCaseQty(string PONumber, string palletId, string itemNumber, string size)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PONumber", PONumber));
            lstFP.Add(new FloorSqlParameter("@palletId", palletId));
            lstFP.Add(new FloorSqlParameter("@itemNumber", itemNumber));
            lstFP.Add(new FloorSqlParameter("@size", size));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_RollBackEWNCaseQty", lstFP);
        }

        public static int RollBackEWNForSurgical(string PONumber, string palletId, string itemNumber, string size)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PONumber", PONumber));
            lstFP.Add(new FloorSqlParameter("@palletId", palletId));
            lstFP.Add(new FloorSqlParameter("@itemNumber", itemNumber));
            lstFP.Add(new FloorSqlParameter("@size", size));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_RollBackEWNForSurgical", lstFP);
        }

        #endregion

#region Line Clearance Verification
        public static int IsEmployeeAllowToVerifyLineClearance(string EmpID, string Password)
        {
            int ret = 0;
            List<FloorSqlParameter> param = new List<FloorSqlParameter>();
            param.Add(new FloorSqlParameter("@EmployeeID", EmpID));
            param.Add(new FloorSqlParameter("@Password", Password));
            ret = Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_CheckLineClearanceVerificationByEmployeeIDAndPassword", param));
            return ret;
        }

        public static LineClearanceLogDTO GetLineClearanceLog(string workstationID)
        {
            try
            {
                LineClearanceLogDTO objLineClearanceLogDTO = new LineClearanceLogDTO();
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@WorkStationID", workstationID));

                DataTable dtLineClearanceLog = FloorDBAccess.ExecuteDataTable("usp_GetLatestLineClearanceLogByWorkStationID", lstFP);
                if (dtLineClearanceLog.Rows.Count > 0)
                {
                    DataRow row = dtLineClearanceLog.Rows[0];
                    objLineClearanceLogDTO.PONumber = FloorDBAccess.GetString(row, "PONumber");
                    objLineClearanceLogDTO.Size = FloorDBAccess.GetString(row, "Size");
                    objLineClearanceLogDTO.ItemNumber = FloorDBAccess.GetString(row, "ItemNumber");
                    objLineClearanceLogDTO.PalletID = FloorDBAccess.GetString(row, "PalletID");
                    objLineClearanceLogDTO.ScreenName = FloorDBAccess.GetString(row, "ScreenName");
                    objLineClearanceLogDTO.WorkStationID = FloorDBAccess.GetValue<int>(row, "WorkStationID");
                }
                return objLineClearanceLogDTO;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// Insert Line Clearance Log
        /// </summary>
        /// <param name="objLineClearanceLogDTO">Line Clearance DTO object</param>
        /// <returns>Return rows affected</returns>
        public static void InsertLineClearanceLog(LineClearanceLogDTO objLineClearanceLogDTO)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            try
            {
                lstfsp.Add(new FloorSqlParameter("@PONumber", objLineClearanceLogDTO.PONumber));
                lstfsp.Add(new FloorSqlParameter("@Size", objLineClearanceLogDTO.Size));
                lstfsp.Add(new FloorSqlParameter("@ItemNumber", objLineClearanceLogDTO.ItemNumber));
                lstfsp.Add(new FloorSqlParameter("@PalletID", objLineClearanceLogDTO.PalletID));
                lstfsp.Add(new FloorSqlParameter("@ScreenName", objLineClearanceLogDTO.ScreenName));
                lstfsp.Add(new FloorSqlParameter("@AuthorisedBy", objLineClearanceLogDTO.AuthorisedBy));
                lstfsp.Add(new FloorSqlParameter("@CreatedDateTime", objLineClearanceLogDTO.CreatedDateTime));
                //lstfsp.Add(new FloorSqlParameter("@WorkStationID", objLineClearanceLogDTO.WorkStationID));
                lstfsp.Add(new FloorSqlParameter("@WorkStationID", Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId)));
                FloorDBAccess.ExecuteNonQuery("usp_SetInsertLineClearanceLog", lstfsp);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }
        #endregion

        public static Boolean isSalesOrderFGInnerLabelsetCOMPrinting(string SONumber, string FGNumber)
        {
            Boolean ret = false;
            int nFromDB;
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@PONumber", SONumber));
            lstFP.Add(new FloorSqlParameter("@FG", FGNumber));
            nFromDB = Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_CheckSOFGBulkdPackInnerCOMPrinting", lstFP));
            ret = nFromDB == Constants.ONE ? true : false;
            return ret;
        }

        /// <summary>
        /// Check current item whether is from PEHA
        /// 
        /// </summary>
        /// <param name="itemNumber">Item Number</param>
        /// <returns>Return boolean</returns>
        public static bool CheckDuplicateInternalLotNumber(string internalLotNumber, string workstationId, out int runningNo)
        {
            Boolean ret = false;
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@InternalLotNumber", internalLotNumber));
            lstFP.Add(new FloorSqlParameter("@WorkStationID", workstationId));
            DataTable nFromDB = FloorDBAccess.ExecuteDataTable("usp_CheckDuplicateInternalLotNumber", lstFP);

            DataRow row = nFromDB.Rows[0];
            ret = Convert.ToInt32(row["IsDuplicate"]) == Constants.ONE ? true : false;
            runningNo = Convert.ToInt32(row["NextRunningNo"]);
            return ret;
        }


        public static int ValidateFGLabel(string FGCode, string WorkOrder, string SerialNo)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@ItemID", FGCode));
            lstfsp.Add(new FloorSqlParameter("@WorkOrder", WorkOrder));
            lstfsp.Add(new FloorSqlParameter("@SerialNumber", SerialNo));
            int nFromDB = Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_ValidateFGLabel", lstfsp));
            return nFromDB;
        }
        #region Halyard
        /// <summary>
        /// Get Julian Day
        /// </summary>
        /// <param name="CurrentDate">Current Date Time</param>
        /// <returns>Return Julian Day</returns>
        public static string GetJulianDay(DateTime CurrentDate)
        {
            int DayOfWeek = 0;
            int JulianDay = 0;

            DayOfWeek = CurrentDate.DayOfWeek == 0 ? Constants.SEVEN : (int)CurrentDate.DayOfWeek;

            JulianDay = DayOfWeek == Constants.ONE ? CurrentDate.DayOfYear : CurrentDate.DayOfYear - (DayOfWeek - 1);

            if (JulianDay <= 0)  // Handle cross year issue : current year -1day + Calculated JulianDay
                JulianDay = new DateTime(DateTime.Now.Year, 1, 1).AddDays(-1).DayOfYear + JulianDay;

            // This hard-coded condition is requested by Customer to identify problem cross month on May 1 - May 3
            if (CurrentDate >= Convert.ToDateTime("2018-04-30") && CurrentDate < Convert.ToDateTime("2018-05-07"))
                JulianDay = 123;

            return JulianDay.ToString("D" + 3);
        }

        /// <summary>
        /// Get Julian Month
        /// </summary>
        /// <param name="JulianDay">String JulianDay</param>
        /// <returns>Return Julian Day</returns>
        public static DateTime GetJulianMonth(String JulianDay) //monday: 90day
        {
            DateTime JulianMonth = new DateTime(DateTime.Now.Year, 1, 1); // get 1 of January date 2018-01-01
            JulianMonth = JulianMonth.AddDays(Convert.ToInt16(JulianDay)).AddDays(-1);

            if (JulianMonth.DayOfYear > CommonBLL.GetCurrentDateAndTimeFromServer().DayOfYear) // cross year 2019 DEC > 2019 JAN
            {
                JulianMonth = new DateTime(DateTime.Now.Year, 1, 1).AddYears(-1).AddDays(Convert.ToInt16(JulianDay)).AddDays(-1);
            }

            return JulianMonth;
        }
        #endregion

        #region First Carton Packing Date
        public static DateTime GetFirstCartonPackingDate(string PONumber, string ItemNumber, string ItemSize)
        {
            try
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@PONumber", PONumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
                lstFP.Add(new FloorSqlParameter("@ItemSize", ItemSize));
                return Convert.ToDateTime(FloorDBAccess.ExecuteScalar("usp_FP_FirstManufacturingDate_Get", lstFP));
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.FIRSTMANUFACTURINGDATE_GET, Constants.BUSINESSLOGIC, ex);
            }

        }
        public static void UpdateFirstCartonPackingDate(string PONumber, string ItemNumber, string ItemSize)
        {
            try
            {
                List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
                lstFP.Add(new FloorSqlParameter("@PONumber", PONumber));
                lstFP.Add(new FloorSqlParameter("@ItemNumber", ItemNumber));
                lstFP.Add(new FloorSqlParameter("@ItemSize", ItemSize));
                FloorDBAccess.ExecuteScalar("usp_FP_FirstManufacturingDate_Set", lstFP);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.FIRSTMANUFACTURINGDATE_UPDATE, Constants.BUSINESSLOGIC, ex);
            }
        }
        #endregion
        #region PH Lot Verification
        /// <summary>
        /// Insert Lot Verification Log
        /// </summary>
        /// <param name="objLotVerificationLogDTO">Lot Verification DTO object</param>
        /// <returns>Return rows affected</returns>
        public static void InsertLotVerificationLog(LotVerificationLogDTO objLotVerificationLogDTO)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            try
            {
                lstfsp.Add(new FloorSqlParameter("@PONumber", objLotVerificationLogDTO.PONumber));
                lstfsp.Add(new FloorSqlParameter("@Size", objLotVerificationLogDTO.Size));
                lstfsp.Add(new FloorSqlParameter("@ItemNumber", objLotVerificationLogDTO.ItemNumber));
                lstfsp.Add(new FloorSqlParameter("@PalletID", objLotVerificationLogDTO.PalletID));
                lstfsp.Add(new FloorSqlParameter("@LotNumberScanned", objLotVerificationLogDTO.LotNumberScanned));
                lstfsp.Add(new FloorSqlParameter("@CreatedDateTime", objLotVerificationLogDTO.CreatedDateTime));
                FloorDBAccess.ExecuteNonQuery("usp_SetInsertLotVerificationLog", lstfsp);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// Insert Final Packing Txn
        /// </summary>
        /// <param name="objFinalPackingTxnDTOList">Final Packing Txn DTO object List</param>
        /// <returns>Return rows affected</returns>
        public static void InsertFinalPackingTxn(List<FinalPackingTxnDTO> objFinalPackingTxnDTOList)
        {

            try
            {
                foreach (FinalPackingTxnDTO objFinalPackingTxnDTO in objFinalPackingTxnDTOList)
                {
                    List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
                    lstfsp.Add(new FloorSqlParameter("@PONumber", objFinalPackingTxnDTO.PONumber));
                    lstfsp.Add(new FloorSqlParameter("@Size", objFinalPackingTxnDTO.Size));
                    lstfsp.Add(new FloorSqlParameter("@ItemNumber", objFinalPackingTxnDTO.ItemNumber));
                    lstfsp.Add(new FloorSqlParameter("@LotNumber", objFinalPackingTxnDTO.LotNumber));
                    lstfsp.Add(new FloorSqlParameter("@LotNumberScanned", objFinalPackingTxnDTO.LotNumberScanned));
                    lstfsp.Add(new FloorSqlParameter("@CartonNo", objFinalPackingTxnDTO.CartonNo));
                    lstfsp.Add(new FloorSqlParameter("@Scanner", objFinalPackingTxnDTO.Scanner));
                    lstfsp.Add(new FloorSqlParameter("@IsMismatch", objFinalPackingTxnDTO.IsMismatch));
                    lstfsp.Add(new FloorSqlParameter("@IsEmail", objFinalPackingTxnDTO.IsEmail));
                    lstfsp.Add(new FloorSqlParameter("@CreatedDateTime", objFinalPackingTxnDTO.CreatedDateTime));
                    lstfsp.Add(new FloorSqlParameter("@ScnMisscannedCount", objFinalPackingTxnDTO.ScnMisscannedCount));
                    FloorDBAccess.ExecuteNonQuery("usp_SetInsertFinalPackingTxn", lstfsp);
                }
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// Check current item whether is from PEHA
        /// </summary>
        /// <param name="itemNumber">Item Number</param>
        /// <returns>Return boolean</returns>
        public static Boolean IsPEHA(string itemNumber)
        {
            Boolean ret = false;
            int nFromDB;
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@ItemNumber", itemNumber));
            nFromDB = Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_CheckIsPEHASO", lstFP));
            ret = nFromDB == Constants.ONE ? true : false;
            return ret;
        }
        #endregion


        // YS 12/06/2018 to generate log file
        public static void GenerateFPLogFile(string content)
        {
            try
            {
                // Can empty the configuration to disable log file generation in [SystemConfData] table
                string path = FloorSystemConfiguration.GetInstance().strFPLogPath;
                if (!string.IsNullOrEmpty(path))
                {
                    if (path.Trim().ToLower() != "none")
                    {
                        if (!File.Exists(path))
                        {
                            File.Create(path).Close();
                        }

                        using (StreamWriter writer = new StreamWriter(path, true, Encoding.GetEncoding("iso-8859-1")))
                        {
                            writer.WriteLine(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt") + " : " + content);
                            writer.Close();
                        }
                    }

                }
            }
            catch (Exception ex)
            { }

        }

        /// <summary>
        /// Update Pallet to not Available
        /// </summary>
        /// <param name="palletID"></param>
        /// <returns></returns>
        public static int ClosePallet(string palletID)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@PalletId", palletID));
            return FloorDBAccess.ExecuteNonQuery("usp_FP_ClosePalletOnFull", lstfsp);
        }


        /// <summary>
        /// Check Work Order Type. This changes related with OBP-WMS project.
        /// </summary>
        /// <param name="SalesId"></param>
        /// <returns></returns>
        public static bool OBPWMSCheckWorkOrderType(string SalesId)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@SalesId", SalesId));
            int nFromDB = Convert.ToInt32(FloorDBAccess.ExecuteScalar("usp_FP_OBPWMSValidateOrder", lstfsp));
            if (nFromDB == Constants.ONE) //Identified as Forecast MTS Sales Order
                return true;
            else
                return false;
        }

        #region Surgical Packing Plan Slip

        public static SurgicalPackingPlanDTO SurgicalPackingPlan_Get(string SPPSerialNo)
        {
            SurgicalPackingPlanDTO data = new SurgicalPackingPlanDTO();

            //FloorSqlParameter fsp = new FloorSqlParameter("@InternalLotNumber", _internalLotNo);
            FloorSqlParameter fsp = new FloorSqlParameter("@SPPSerialNo", SPPSerialNo);
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(fsp);
            DataTable dt;

            decimal serialNo = 0;
            if (!decimal.TryParse(SPPSerialNo, out serialNo))
            {
                data = null;
            }
            else
            {
                using (dt = FloorDBAccess.ExecuteDataTable("USP_GET_SurgicalPackingPlan_BySPPSerialNo", lstfsp))
                {
                    if (dt.Rows.Count == 0)
                        data = null;
                    else
                    {
                        DataRow row = dt.Rows[0];
                        data.SPPSerialNo = SPPSerialNo;
                        data.PONumber = FloorDBAccess.GetString(row, "PONumber");
                        data.ItemNumber = FloorDBAccess.GetString(row, "ItemNumber");
                        data.ItemBrand = FloorDBAccess.GetString(row, "ItemBrand");
                        data.ItemSize = FloorDBAccess.GetString(row, "ItemSize");
                        data.GloveCode = FloorDBAccess.GetString(row, "GloveCode");
                        data.CustomerReferenceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber");
                        data.GloveSize = FloorDBAccess.GetString(row, "GloveSize");
                        data.PlanStatus = FloorDBAccess.GetValue<Int32>(row, "PlanStatus");
                        data.PlannedCases = FloorDBAccess.GetValue<Int32>(row, "PlannedCases");
                        data.InternalLotNo = FloorDBAccess.GetString(row, "InternalLotNo");
                        data.ManufacturingDate = FloorDBAccess.GetValue<DateTime>(row, "ManufacturingDate");
                        data.ExpiryDate = FloorDBAccess.GetValue<DateTime>(row, "ExpiryDate");
                    }
                }
            }
            
            return data;
        }

        public static SOLineDTO GetPurchaseOrderSingle(string PONumber, string ItemId, string ItemSize)
        {
            SOLineDTO lstSOLine = new SOLineDTO();
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@PONumber", PONumber));
            lstfsp.Add(new FloorSqlParameter("@ItemNumber", ItemId));
            lstfsp.Add(new FloorSqlParameter("@ItemSize", ItemSize));

            using (DataTable dtPOlist = FloorDBAccess.ExecuteDataTable("usp_GET_SurgicalPackingPlan_GetPO", lstfsp))
            {
                try
                {
                    if (dtPOlist != null && dtPOlist.Rows.Count != 0)
                    {
                        foreach (DataRow row in dtPOlist.Rows)
                        {
                            SOLineDTO objSo = new SOLineDTO();
                            objSo.PONumber = FloorDBAccess.GetString(row, "PONumber");
                            objSo.BarcodeVerificationRequired = FloorDBAccess.GetValue<int>(row, "BarcodeVerificationRequired");
                            if (!string.IsNullOrEmpty(FloorDBAccess.GetString(row, "PreshipmentPlan")))
                                objSo.PreshipmentPlan = int.Parse(FloorDBAccess.GetString(row, "PreshipmentPlan"));
                            objSo.ItemNumber = FloorDBAccess.GetString(row, "ItemNumber");
                            objSo.ItemNumberDisplay = string.Format("{0}{1}{2}", FloorDBAccess.GetString(row, "ItemNumber"), Constants.UNDERSCORE, FloorDBAccess.GetString(row, "ItemSize"));
                            objSo.ItemSize = FloorDBAccess.GetString(row, "ItemSize");
                            objSo.GloveCode = FloorDBAccess.GetString(row, "GloveCode");
                            objSo.ItemCases = (int)FloorDBAccess.GetValue<decimal>(row, "ItemCases");
                            objSo.InnersetLayout = FloorDBAccess.GetString(row, "InnersetLayout");
                            objSo.OuterSetLayout = FloorDBAccess.GetString(row, "OuterSetLayout");
                            objSo.InnerLabelSetDateFormat = FloorDBAccess.GetString(row, "InnerLabelSetDateFormat"); // yiksiu SEP 2017: Label Set Optimization 
                            objSo.OuterLabelSetDateFormat = FloorDBAccess.GetString(row, "OuterLabelSetDateFormat"); // yiksiu SEP 2017: Label Set Optimization
                            objSo.CustomerSize = FloorDBAccess.GetString(row, "CustomerSize");
                            objSo.CustomerSizeDesc = FloorDBAccess.GetString(row, "CustomerSizeDesc");
                            objSo.GrossWeight = FloorDBAccess.GetValue<decimal>(row, "GrossWeight");
                            objSo.NettWeight = FloorDBAccess.GetValue<decimal>(row, "NettWeight");
                            objSo.CaseCapacity = FloorDBAccess.GetValue<int>(row, "CaseCapacity");
                            objSo.PalletCapacity = FloorDBAccess.GetValue<int>(row, "PalletCapacity");
                            objSo.InnerBoxCapacity = FloorDBAccess.GetValue<int>(row, "InnerBoxCapacity");
                            objSo.CustomerLotNumber = FloorDBAccess.GetString(row, "CustomerLotNumber");
                            objSo.CustomerRefernceNumber = FloorDBAccess.GetString(row, "CustomerReferenceNumber");
                            objSo.ItemName = FloorDBAccess.GetString(row, "ItemName");
                            objSo.ManufacturingDateBasis = FloorDBAccess.GetValue<int>(row, "MANUFACTURINGDATEBASIS");
                            objSo.CustomerName = FloorDBAccess.GetString(row, "CustomerName");
                            objSo.POStatus = FloorDBAccess.GetValue<int>(row, "POStatus");
                            objSo.InnerProductCode = FloorDBAccess.GetString(row, "InnerProductCode");
                            objSo.OuterProductCode = FloorDBAccess.GetString(row, "OuterProductCode");
                            // objSo.BrandName = FloorDBAccess.GetString(row, "BrandName");
                            objSo.Expiry = FloorDBAccess.GetValue<int>(row, "Expiry");
                            objSo.Reference1 = FloorDBAccess.GetString(row, "ProductReferenceNumber");
                            objSo.SpecialInnerCode = FloorDBAccess.GetValue<int>(row, "SpecialInnerCode");
                            objSo.SpecialInnerCodeCharacter = FloorDBAccess.GetString(row, "SpecialInnerCodeCharacter");
                            objSo.GCLabelPrintingRequired = FloorDBAccess.GetValue<int>(row, "GCLabelPrintingRequired");
                            objSo.AlternateGloveCode1 = FloorDBAccess.GetString(row, "AlternateGloveCode1");
                            objSo.AlternateGloveCode2 = FloorDBAccess.GetString(row, "AlternateGloveCode2");
                            objSo.AlternateGloveCode3 = FloorDBAccess.GetString(row, "AlternateGloveCode3");
                            objSo.RECEIPTDATEREQUESTED = FloorDBAccess.GetValue<DateTime>(row, "RECEIPTDATEREQUESTED");
                            objSo.SHIPPINGDATEREQUESTED = FloorDBAccess.GetValue<DateTime>(row, "SHIPPINGDATEREQUESTED");
                            objSo.ShippingDateETD = FloorDBAccess.GetValue<DateTime>(row, "STShippingDateConfirmed");
                            objSo.ManufacturingDateETD = FloorDBAccess.GetValue<DateTime>(row, "ManufacturingDateETD");
                            objSo.ItemType = FloorDBAccess.GetValue<int>(row, "ItemType");
                            objSo.OrderNumber = FloorDBAccess.GetString(row, "OrderNumber");
                            objSo.Reference2 = FloorDBAccess.GetString(row, "Reference2");
                            objSo.InventTransId = FloorDBAccess.GetString(row, "INVENTTRANSID");
                            //KahHeng 29jan2019 added 2 new parameter for PODate  POReceviedDate 
                            //objSo.HSB_CustPODocumentDate = FloorDBAccess.GetValue<DateTime>(row, "HSB_CustPODocumentDate");
                            //objSo.HSB_CustPORecvDate = FloorDBAccess.GetValue<DateTime>(row, "HSB_CustPORecvDate");
                            //KahHeng End
                            //Azrul 18Mar2021 added 2 new parameter for PODate  POReceviedDate 
                            objSo.CustPODocumentDate = FloorDBAccess.GetValue<DateTime>(row, "HSB_CustPODocumentDate");
                            objSo.CustPORecvDate = FloorDBAccess.GetValue<DateTime>(row, "HSB_CustPORecvDate");
                            //Azrul End
                            lstSOLine = objSo;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                }
                return lstSOLine;
            }
        }

        public static List<SurgicalPackingSNDetailsDTO> SurgicalPackingPlan_GetDetails(string _internalLotNo)
        {
            List<SurgicalPackingSNDetailsDTO> list = new List<SurgicalPackingSNDetailsDTO>();
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@InternalLotNo", _internalLotNo));
            using (DataTable dtPreshipmentpalletDetails = FloorDBAccess.ExecuteDataTable("usp_GET_SurgicalPackingPlan_FPDetails", lstfsp))
            {
                try
                {
                    if (dtPreshipmentpalletDetails != null && dtPreshipmentpalletDetails.Rows.Count != 0)
                    {
                        foreach (DataRow row in dtPreshipmentpalletDetails.Rows)
                        {
                            SurgicalPackingSNDetailsDTO objSo = new SurgicalPackingSNDetailsDTO();
                            objSo.SerialNumber = FloorDBAccess.GetString(row, "SerialNumber");
                            objSo.BatchNumber = FloorDBAccess.GetString(row, "BatchNumber");
                            objSo.GloveSide = FloorDBAccess.GetString(row, "GloveSide");
                            objSo.ReservedQty = FloorDBAccess.GetValue<int>(row, "ReservedQty");
                            list.Add(objSo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.PALLET_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        public static DateTime SurgicalPackingPlan_GetProductionDate(string _internalLotNo)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@InternalLotNo", _internalLotNo));
            return Convert.ToDateTime(FloorDBAccess.ExecuteScalar("USP_GET_SurgicalPackingPlan_ProductionDate", lstFP));
        }

        public static void SurgicalPackingPlan_UpdatePlan(string _internalLotNo, int status, int WorkStationId)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@InternalLotNo", _internalLotNo));
            lstFP.Add(new FloorSqlParameter("@Status", status));
            lstFP.Add(new FloorSqlParameter("@WorkStationId", WorkStationId));       
            FloorDBAccess.ExecuteScalar("USP_UPD_SurgicalPackingPlan_UpdatePlanStatus", lstFP);
        }

        public static void SurgicalPackingPlan_UpdatePlanBySPP(string SPPSerialNo, int status)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@SPPSerialNo", SPPSerialNo));
            lstFP.Add(new FloorSqlParameter("@Status", status));
            FloorDBAccess.ExecuteScalar("USP_UPD_SurgicalPackingPlan_UpdatePlanStatusbySPP", lstFP);
        }

        //Azrul 20201111: Update FGBO & Resource for Surgical Packing Plan
        public static void SurgicalPackingPlan_UpdateFGBatchOrderNo(string _internalLotNo, string FGBatchOrderNo)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internalLotNumber", _internalLotNo));
            lstFP.Add(new FloorSqlParameter("@fgBatchOrderNo", FGBatchOrderNo));
            FloorDBAccess.ExecuteScalar("usp_DOT_FP_UpdateFGBatchOrderNoandResource", lstFP);
        }

        public static int InsertFinalPackingSurgicalSPP(FinalPackingDTO objFinalPacking)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@LocationId", objFinalPacking.locationId));
            lstFP.Add(new FloorSqlParameter("@WorkStationNumber", objFinalPacking.Workstationnumber));
            lstFP.Add(new FloorSqlParameter("@PrinterName", objFinalPacking.Printername));
            lstFP.Add(new FloorSqlParameter("@PackDate", objFinalPacking.Packdate));
            lstFP.Add(new FloorSqlParameter("@OuterLotNo", objFinalPacking.Outerlotno));
            lstFP.Add(new FloorSqlParameter("@InternalLotNumber", objFinalPacking.Internallotnumber));
            lstFP.Add(new FloorSqlParameter("@PONumber", objFinalPacking.Ponumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", objFinalPacking.ItemNumber));
            lstFP.Add(new FloorSqlParameter("@Size", objFinalPacking.Size));
            lstFP.Add(new FloorSqlParameter("@GroupId", objFinalPacking.GroupId));

            lstFP.Add(new FloorSqlParameter("@BoxesPacked", objFinalPacking.Boxespacked));

            lstFP.Add(new FloorSqlParameter("@CasesPacked", objFinalPacking.Casespacked));

            // lstFP.Add(new FloorSqlParameter("@OperatorId", objFinalPacking.OperatorId));
            lstFP.Add(new FloorSqlParameter("@InnerSetLayout", objFinalPacking.Innersetlayout));
            lstFP.Add(new FloorSqlParameter("@OuterSetLayout", objFinalPacking.Outersetlayout));
            lstFP.Add(new FloorSqlParameter("@palletCapacity", objFinalPacking.palletCapacity));
            lstFP.Add(new FloorSqlParameter("@TotalPcs", objFinalPacking.TotalPcs));
            lstFP.Add(new FloorSqlParameter("@isTempPack", objFinalPacking.isTempPack));
            lstFP.Add(new FloorSqlParameter("@ManufacturingDate", objFinalPacking.ManufacturingDate));
            lstFP.Add(new FloorSqlParameter("@ExpiryDate", objFinalPacking.ExpiryDate));
            lstFP.Add(new FloorSqlParameter("@stationNumber", objFinalPacking.stationNumber));
            lstFP.Add(new FloorSqlParameter("@InventTransId", objFinalPacking.InventTransId));
            lstFP.Add(new FloorSqlParameter("@FPStationNo", objFinalPacking.FPStationNo)); //Added by Azman 2019-08-22 FPDetailsReport
            return FloorDBAccess.ExecuteNonQuery("usp_SAV_SurgicalPacking_PouchPrinting", lstFP);
        }

        public static int InsertFinalPackingSurgicalSPPV2(FinalPackingDTO objFinalPacking)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@LocationId", objFinalPacking.locationId));
            lstFP.Add(new FloorSqlParameter("@WorkStationNumber", objFinalPacking.Workstationnumber));
            lstFP.Add(new FloorSqlParameter("@PrinterName", objFinalPacking.Printername));
            lstFP.Add(new FloorSqlParameter("@PackDate", objFinalPacking.Packdate));
            lstFP.Add(new FloorSqlParameter("@OuterLotNo", objFinalPacking.Outerlotno));
            lstFP.Add(new FloorSqlParameter("@InternalLotNumber", objFinalPacking.Internallotnumber));
            lstFP.Add(new FloorSqlParameter("@PONumber", objFinalPacking.Ponumber));
            lstFP.Add(new FloorSqlParameter("@ItemNumber", objFinalPacking.ItemNumber));
            lstFP.Add(new FloorSqlParameter("@Size", objFinalPacking.Size));

            lstFP.Add(new FloorSqlParameter("@BoxesPacked", objFinalPacking.Boxespacked));

            lstFP.Add(new FloorSqlParameter("@CasesPacked", objFinalPacking.Casespacked));

            lstFP.Add(new FloorSqlParameter("@InnerSetLayout", objFinalPacking.Innersetlayout));
            lstFP.Add(new FloorSqlParameter("@OuterSetLayout", objFinalPacking.Outersetlayout));
            lstFP.Add(new FloorSqlParameter("@palletCapacity", objFinalPacking.palletCapacity));
            lstFP.Add(new FloorSqlParameter("@ManufacturingDate", objFinalPacking.ManufacturingDate));
            lstFP.Add(new FloorSqlParameter("@ExpiryDate", objFinalPacking.ExpiryDate));
            lstFP.Add(new FloorSqlParameter("@stationNumber", objFinalPacking.stationNumber));
            lstFP.Add(new FloorSqlParameter("@InventTransId", objFinalPacking.InventTransId));
            lstFP.Add(new FloorSqlParameter("@FPStationNo", objFinalPacking.FPStationNo));
            return FloorDBAccess.ExecuteNonQuery("usp_SAV_SurgicalPacking_FinalPackingData", lstFP);
        }

        /// <summary>
        /// Insert Purchase order details for selected PO Item
        /// </summary>
        /// <param name="objPurchaseOrderItemDTO">Purchase Order DTO object</param>
        /// <returns>Return rows affected</returns>
        public static int UpdateSPPPurchaseOrderDetails(SOLineDTO objPurchaseOrderItemDTO)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@SalesId ", objPurchaseOrderItemDTO.PONumber));
            lstfsp.Add(new FloorSqlParameter("@ItemNumber ", objPurchaseOrderItemDTO.ItemNumber));
            lstfsp.Add(new FloorSqlParameter("@ItemSize ", objPurchaseOrderItemDTO.ItemSize));
            lstfsp.Add(new FloorSqlParameter("@LocationId ", objPurchaseOrderItemDTO.locationID));
            lstfsp.Add(new FloorSqlParameter("@PreshipmentCases  ", objPurchaseOrderItemDTO.Preshipmentcases));
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_FP_UpdatePurchaseOrderItemforSPP", lstfsp);
        }
        #endregion
        #region AX4
        /// <summary>
        /// Generate AX inner/outer txt file
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="path"></param>
        public static void GenerateAXInnerFile(string[] mapping, string path, bool? isAppend = false)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            using (StreamWriter writer = new StreamWriter(path, isAppend.GetValueOrDefault()))
            {
                writer.WriteLine(String.Join("^", mapping));
            }
        }

        public static void GenerateAXOuterFile(string[] mapping, string path, bool? isAppend = false)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            using (StreamWriter writer = new StreamWriter(path, isAppend.GetValueOrDefault(), Encoding.GetEncoding("iso-8859-1")))
            {
                writer.WriteLine(String.Join(",", mapping));
                writer.Close();
            }
        }
        #endregion

        public static List<int> GetLotCartonRangeList(string internallotnumber)
        {
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internallotnumber", internallotnumber));

            List<int> list = new List<int>();
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_FP_GetInternalLotnumberCaseRange", lstFP))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            list.Add(FloorDBAccess.GetValue<int>(row, "casenumber"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GENERALEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }


        /// <summary>
        /// backup AX File
        /// </summary>
        /// <param name="path"></param>
        public static void BackupAXFile(string path)
        {
            if (File.Exists(path))
            {
                string fileName = Path.GetFileNameWithoutExtension(path);
                File.Copy(path, path.Replace(fileName, fileName + "_" + DateTime.Now.ToString("ddMMyyyyhhmm")));
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// GENERATE RANDOM PRESHIPMENT CASE NUMBER
        /// </summary>
        /// <param name="preshipmentQty">No of Preshipmetn casenumbers required</param>
        /// <param name="cty">Total Case Quantity</param>
        /// <returns>returns , separated Preshipment case numbers</returns>
        public static string GenerateRandomNumbers(int preshipmentQty, int cty)
        {
            string strresult = string.Empty;
            try
            {
                List<Int32> result;
                //TOTT id :317 when prishipment qty is same as actual qty
                if (preshipmentQty == cty)
                {
                    result = new List<Int32>();
                    for (Int32 presqtycount = Constants.ONE; presqtycount <= preshipmentQty; presqtycount++)
                    {
                        strresult += presqtycount.ToString() + Constants.COMMA;
                    }
                }
                else
                {
                    Random rand = new Random();
                    Int32 curValue = 0;
                    int preshipmentQtyStart = 0;
                    int preshipmentQtyEnd = 0;
                    // Calculate rounded interval amount - always rounding down to avoid out of range 
                    int preshipmentQtyInterval = Decimal.ToInt32((decimal)cty / preshipmentQty);

                    result = new List<Int32>();
                    for (Int32 presqtycount = Constants.ZERO; presqtycount < preshipmentQty; presqtycount++)
                    {
                        // Start will be 1 and Last interval will always be the cty amount
                        preshipmentQtyStart = preshipmentQtyEnd + 1;
                        preshipmentQtyEnd = ((presqtycount + 1) == preshipmentQty) ? cty : preshipmentQtyInterval * (presqtycount + 1);
                        curValue = rand.Next(preshipmentQtyStart, preshipmentQtyEnd + 1);

                        result.Add(curValue);
                        strresult += curValue + Constants.COMMA;
                    }
                }
                return strresult.TrimEnd(Constants.CHARCOMMA);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message);
            }

        }
    }
}
