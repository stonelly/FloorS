// -----------------------------------------------------------------------
// <copyright file="GISBLL.cs" company="Avanade">
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
    /// Glove Inventory System (GIS) business logic class
    /// </summary>
    public static class GISBLL
    {        
        #region Public Methods
        /// <summary>
        ///  Checks whether the Serial Number already exists in GloveInventorySystem table.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>BinId in which Batch is Scanned In.</returns>        
        public static string ValidateDuplicateSerialNo(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            object result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_GIS_DuplicateSerialNo", lstParameters);
            return Convert.ToString(result);                    
        }

        /// <summary>
        /// This method is used to check whether bin is available.
        /// </summary>
        /// <param name="binNumber"></param>
        /// <returns>Status as a result</returns>        
        public static bool ValidateBin(string binNumber, int location)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();    
            lstParameters.Add(new FloorSqlParameter("@binNumber", binNumber));
            lstParameters.Add(new FloorSqlParameter("@locationId", location));
            object result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_Bin", lstParameters);
            return Convert.ToBoolean(result);             
        }

        /// <summary>
        /// Inserts or updates Batch Card details in the GloveInventorySystem table.
        /// </summary>
        /// <param name="seriaNo"></param>
        /// <param name="location"></param>
        /// <param name="bin"></param>
        /// <param name="lastModifiedOn"></param>
        /// <param name="scanInDate"></param>        
        public static void SaveBatchScanInDetails(GISDTO objGIS)
        {               
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", objGIS.SerialNumber));
            lstParameters.Add(new FloorSqlParameter("@location", objGIS.LocationId));
            lstParameters.Add(new FloorSqlParameter("@bin", objGIS.BinNumber));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOn", objGIS.LastModifiedOn));
            lstParameters.Add(new FloorSqlParameter("@id", objGIS.Id));
            lstParameters.Add(new FloorSqlParameter("@nextProcess", objGIS.NextProcess));
            lstParameters.Add(new FloorSqlParameter("@workstationId", objGIS.WorkstationId));              
            FloorDBAccess.ExecuteNonQuery("USP_SAV_GIS_Batch", lstParameters);                
        }

        public static GISDTO StoreScanInDetails(decimal serialNumber)
        {
            GISDTO objG = null;
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            using (DataTable dt = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_GISUpdateStore", lstParameters))
            {
                objG = (from DataRow row in dt.Rows
                            select new GISDTO
                            {
                                   GISId = Convert.ToInt32(FloorDBAccess.GetString(row, "GISId")),
                                   SerialNumber = Convert.ToDecimal(FloorDBAccess.GetString(row, "SerialNumber")),
                                   LocationId = Convert.ToInt32( FloorDBAccess.GetString(row, "LocationId")),
                                   BinNumber = FloorDBAccess.GetString(row, "BinId"),
                                   LastModifiedOn = Convert.ToDateTime( FloorDBAccess.GetString(row, "LastModifiedOn")),
                                   NextProcess = FloorDBAccess.GetString(row, "NextProcess"),
                                   WorkstationId = Convert.ToInt32(FloorDBAccess.GetString(row, "WorkStationId")),
                                   ScanInDate = Convert.ToDateTime(FloorDBAccess.GetString(row, "ScanInDate")),
                            }).SingleOrDefault();
            }
            return objG;
        }

        /// <summary>
        /// Gets the Batch details based on GloveType, Size and location.
        /// </summary>
        /// <param name="gloveType"></param>
        /// <param name="size"></param>
        /// <param name="locationId"></param>
        /// <returns>DataTable consisting of Batch Details</returns>        
        public static List<GloveInquiryDetails> GetGloveInquiryDetails(string gloveType, string size, int locationId)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            if (String.IsNullOrEmpty(gloveType))
            {
                lstParameters.Add(new FloorSqlParameter("@gloveType", null));
            }
            else
            {
                lstParameters.Add(new FloorSqlParameter("@gloveType", gloveType));
            }
            if (String.IsNullOrEmpty(size))
            {
                lstParameters.Add(new FloorSqlParameter("@size", null));
            }
            else
            {
                lstParameters.Add(new FloorSqlParameter("@size", size));
            }
            lstParameters.Add(new FloorSqlParameter("@locationId", locationId));
            List<GloveInquiryDetails> lstGloveInquiry = new List<GloveInquiryDetails>();
            using (DataTable dtGloveInquiry = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_GloveInquiry", lstParameters))
            {
                try
                {
                    lstGloveInquiry = (from DataRow row in dtGloveInquiry.Rows
                                       select new GloveInquiryDetails
                                       {
                                           SerialNumber = FloorDBAccess.GetString(row, "SerialNumber"),
                                           GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                           Size = FloorDBAccess.GetString(row, "Size"),
                                           QCType = FloorDBAccess.GetString(row, "QCType"),
                                           TenPcsWeight = FloorDBAccess.GetString(row, "TenPCsWeight"),
                                           BatchWeight = FloorDBAccess.GetString(row, "BatchWeight"),
                                           TotalPcs = FloorDBAccess.GetString(row, "TotalPCs"),
                                           BatchDate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate").ToString(ConfigurationManager.AppSettings["dateFormat"]),
                                           BinId = FloorDBAccess.GetString(row, "BinId"),
                                           NoOfDays = FloorDBAccess.GetString(row, "NoOfDays")
                                       }).ToList();
                }
                catch (RowNotInTableException rex)
                {
                    throw new FloorSystemException(Messages.ROWNOTINTABLEEXCEPTION, Constants.BUSINESSLOGIC, rex);
                }
            }
            return lstGloveInquiry;
        }

        /// <summary>
        /// Get the Column Headers for Excel Sheet
        /// </summary>
        /// <returns>List Of Column Headers</returns>
         public static List<string> GetColumnHeaders()
        {
            List<string> lstColumnHeader = new List<string>();
            lstColumnHeader.Add("Serial No");
            lstColumnHeader.Add("Glove Type");
            lstColumnHeader.Add("Size");
            lstColumnHeader.Add("QC Type");
            lstColumnHeader.Add("Kg");
            lstColumnHeader.Add("10 Pcs");
            lstColumnHeader.Add("Total Pcs");
            lstColumnHeader.Add("Batch Date");
            lstColumnHeader.Add("Bin Ref No");
            lstColumnHeader.Add("No Of Days");
            return lstColumnHeader;
         }

         /// <summary>
         /// Delete when there is an Error in AX Posting
         /// </summary>
         /// <param name="qaidto"></param>
         /// <returns></returns>
         public static int DeleteGISScanInData(decimal serialNumber)
         {
             int rowsReturned;
             List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
             PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
           rowsReturned = FloorDBAccess.ExecuteNonQuery("USP_DEL_GISScanInData", PrmList);
           return rowsReturned;
         }
         public static int UpdateGISScanOutData(GISDTO obj)
         {
             int r;
             List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
             PrmList.Add(new FloorSqlParameter("@GisId", obj.GISId));
             PrmList.Add(new FloorSqlParameter("@lastModifiedOn", obj.LastModifiedOn));
             PrmList.Add(new FloorSqlParameter("@workstationId", obj.WorkstationId));
             PrmList.Add(new FloorSqlParameter("@location", obj.LocationId));
             r = FloorDBAccess.ExecuteNonQuery("USP_Update_GISScanOutData", PrmList);
             return r;
         }

         /// <summary>
         /// To Check whether the batch is scanned in from the same Location Area
         /// </summary>
         /// <param name="serialNo"></param>
         /// <returns></returns>
         public static Boolean ValidateBatchScanIn(decimal serialNo, string location, string area)
         {
             List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
             lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
             lstParameters.Add(new FloorSqlParameter("@locationName", location));
             lstParameters.Add(new FloorSqlParameter("@areaName", area));
             bool result = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_Batch_ScanIn", lstParameters));
             return result;
         }

        
         
        #endregion
    }
}
