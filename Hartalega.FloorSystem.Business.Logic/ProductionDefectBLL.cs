// -----------------------------------------------------------------------
// <copyright file="ProductionDefectBLL.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Business.Logic
{
    using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
    using Hartalega.FloorSystem.Framework;
    using Hartalega.FloorSystem.Framework.Database;
    using Hartalega.FloorSystem.Framework.DbExceptionLog;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Production defect business logic class
    /// </summary>
    public class ProductionDefectBLL : Framework.Business.BusinessBase
    {
        #region Private Class Members
        private const string _subSystem = "Production Defect System";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionDefectBLL" /> class.
        /// </summary>
        public ProductionDefectBLL()
        {
            // No implementation required
        }
        #endregion

        #region User Methods
        /// <summary>
        /// Fetch List of ProductionDefectSummary based on ProductionLine and QAIDate
        /// </summary>
        /// <param name="lineNumber">LineNumber</param>
        /// <param name="qaiDate">QAIDate</param>
        /// <returns>List of ProductionDefectSummary</returns>
        public static List<ProductionDefectSummaryDTO> GetDefectSummaryList(string lineNumber, DateTime qaiDate)
        {
            List<ProductionDefectSummaryDTO> productionDefectList = null;
            List<FloorSqlParameter> fspList = new List<FloorSqlParameter>();
            fspList.Add(new FloorSqlParameter("@LineNumber", lineNumber));
            fspList.Add(new FloorSqlParameter("@QAIDate", qaiDate));
            using (DataTable dtProductionDefectSummary = FloorDBAccess.ExecuteDataTable("USP_GET_DefectSummary", fspList))
            {
                if (dtProductionDefectSummary != null && dtProductionDefectSummary.Rows.Count > Constants.ZERO)
                {
                    productionDefectList = (from DataRow dr in dtProductionDefectSummary.Rows
                                            select new ProductionDefectSummaryDTO
                                            {
                                                ProductionDefectId = FloorDBAccess.GetValue<int>(dr, "ProductionDefectId"),
                                                SerialNumber = FloorDBAccess.GetValue<decimal>(dr, "SerialNumber"),
                                                QAIDefectQuantity = FloorDBAccess.GetValue<int>(dr, "QAIDefectQuantity"),
                                                PNDefectQuantity = FloorDBAccess.GetValue<int>(dr, "PNDefectQuantity"),
                                                LineNumber = FloorDBAccess.GetString(dr, "LineNumber"),
                                                QAIDate = FloorDBAccess.GetValue<DateTime>(dr, "QAIDate"),
                                                DefectTime = FloorDBAccess.GetString(dr, "DefectTime"),
                                                Size = FloorDBAccess.GetString(dr, "Size"),
                                                TierSide = FloorDBAccess.GetString(dr, "TierSide"),
                                                Status = FloorDBAccess.GetString(dr, "Status")
                                                //,DefectCategoryId = FloorDBAccess.GetValue<int>(dr, "DefectCategoryId")
                                            }).ToList();
                }
            }
            return productionDefectList;
        }

        /// <summary>
        /// Fetch list of ProductionDefectDetail based on Serial Number
        /// </summary>
        /// <param name="serialNumber">productionDefectId</param>
        /// <returns>list of ProductionDefectDetail based on Serial Number</returns>
        public static List<ProductionDefectDetailDTO> GetDefectDetailList(int productionDefectId
            //,int defectCategoryId
            )
        {
            List<ProductionDefectDetailDTO> productionDefectDetailList = null;
            List<FloorSqlParameter> fspList = new List<FloorSqlParameter>();
            fspList.Add(new FloorSqlParameter("@ProductionDefectId", productionDefectId));
            //fspList.Add(new FloorSqlParameter("@DefectCategoryId", defectCategoryId));
            using (DataTable dtProductionDefectDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_DefectDetail", fspList))
            {
                if (dtProductionDefectDetails != null && dtProductionDefectDetails.Rows.Count > Constants.ZERO)
                {
                    productionDefectDetailList = (from DataRow dr in dtProductionDefectDetails.Rows
                                                  select new ProductionDefectDetailDTO
                                                  {
                                                      Id = FloorDBAccess.GetValue<int>(dr, "Id"),
                                                      ProductionDefectId = FloorDBAccess.GetValue<int>(dr, "ProductionDefectId"),
                                                      DefectQuantity = FloorDBAccess.GetValue<int>(dr, "DefectQuantity"),
                                                      DefectType = FloorDBAccess.GetString(dr, "DefectType"),
                                                      DefectDescription = FloorDBAccess.GetString(dr, "DefectDescription")
                                                  }).ToList();
                }
            }
            return productionDefectDetailList;
        }

        /// <summary>
        /// Get List of production line based on Location id
        /// </summary>
        /// <param name="locationId">LocationId</param>
        /// <returns>List of Production Lines</returns>
        public static List<LineDTO> GetLineList(int locationId)
        {
            List<LineDTO> lineList = null;
            List<FloorSqlParameter> lstFsp = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@LocationId", locationId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_LineMaster", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    lineList = (from DataRow row in dt.Rows
                                select new LineDTO
                                {
                                    LineNumber = FloorDBAccess.GetString(row,"LineNumber")
                                }).ToList();
                }
            }
            return lineList;
        }

        /// <summary>
        /// fecth defect description based on Defect type
        /// </summary>
        /// <param name="defectType">DefectType</param>
        /// <returns>defect description based on Defect type</returns>
        public static string GetDefectDescriptionByDefectType(string defectType)
        {
            string defectDescription = string.Empty;
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@DefectType", defectType));
            defectDescription =Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_DefectDescription", lstFsp));
            return defectDescription;
        }

        /// <summary>
        /// fetch list of defect types 
        /// </summary>
        /// <returns>list of defect types</returns>
        public static List<DefectTypeDTO> GetDefectTypeList()
        {
            List<DefectTypeDTO> defectList = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_DefectType"))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    defectList = (from DataRow row in dt.Rows
                                  select new DefectTypeDTO
                                  {
                                      ProdDefectName = FloorDBAccess.GetString(row, "ProdDefectName"),
                                      ProdDefectId = FloorDBAccess.GetValue<int>(row, "ProdDefectId")
                                  }).ToList();
                }
            }
            return defectList;
        }

        /// <summary>
        /// Update Production Defct details
        /// </summary>
        /// <param name="productionDefectId">ProductionDefectId</param>
        /// <param name="defectType">DefectType</param>
        /// <param name="DefectQuantity">DefectQuantity</param>
        /// <returns>No of rows affected</returns>
        public static int UpdateProductionDefectDetails(int productionDefectId, string defectType, int DefectQuantity, string tierSide
           , decimal serialNumber, string lineId, string gloveSize, int qaiDefectQuantity, DateTime defectDate, string defectTime,
            string workStationNumber
            //,int defectCategoryId
            )
        {
            List<FloorSqlParameter> lstfsp = null;
            int noofrows = Constants.ZERO;
            lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@ProductionDefectId", productionDefectId));
            lstfsp.Add(new FloorSqlParameter("@DefectType", defectType));
            lstfsp.Add(new FloorSqlParameter("@DefectQuantity", DefectQuantity));
            lstfsp.Add(new FloorSqlParameter("@TierSide", tierSide));
            lstfsp.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            lstfsp.Add(new FloorSqlParameter("@LineId", lineId));
            lstfsp.Add(new FloorSqlParameter("@GloveSize", gloveSize));
            lstfsp.Add(new FloorSqlParameter("@QAIDefectQuantity", qaiDefectQuantity));
            lstfsp.Add(new FloorSqlParameter("@DefectDate", defectDate.Date));
            lstfsp.Add(new FloorSqlParameter("@DefectTime", defectTime));
            lstfsp.Add(new FloorSqlParameter("@WorkStationNumber", workStationNumber));
            //lstfsp.Add(new FloorSqlParameter("@DefectCategoryId", defectCategoryId));
            noofrows = FloorDBAccess.ExecuteNonQuery("USP_SAV_DefectDetail", lstfsp);
            return noofrows;
        }

        /// <summary>
        /// Edit Production defect details
        /// </summary>
        /// <param name="id">ProductionDefectDetailId</param>
        /// <param name="productionDefectId">ProductionDefectId</param>
        /// <param name="newDefectQuantity">NewDefectQuantity</param>
        /// <returns>No of rows affected</returns>
        public static int EditProductionDefectDetails(int id, int productionDefectId, int newDefectQuantity)
        {
            List<FloorSqlParameter> lstfsp = null;
            int noofrows = Constants.ZERO;
            lstfsp = new List<FloorSqlParameter>();
            lstfsp.Add(new FloorSqlParameter("@Id", id));
            lstfsp.Add(new FloorSqlParameter("@ProductionDefectId", productionDefectId));
            lstfsp.Add(new FloorSqlParameter("@NewDefectQuantity", newDefectQuantity));
            noofrows = FloorDBAccess.ExecuteNonQuery("USP_UPD_DefectDetail", lstfsp);
            return noofrows;
        }

        /// <summary>
        /// Get Production defect base on ProductionDefectId
        /// </summary>
        /// <param name="productionDefectId">ProductionDefectId</param>
        /// <returns>Production defect instance</returns>
        public static ProductionDefectDTO GetProductionDefect(int productionDefectId)
        {
            List<FloorSqlParameter> lstFsp = null;
            ProductionDefectDTO pd = null;
            DataRow dr = null;
            lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@ProductionDefectId", productionDefectId));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_ProductionDefect", lstFsp))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    dr = dt.Rows[Constants.ZERO];
                    pd = new ProductionDefectDTO();
                    pd.LineId = FloorDBAccess.GetString(dr, "LineId");
                    pd.DefectDate = FloorDBAccess.GetValue<DateTime>(dr, "DefectDate");
                    pd.DefectTime = FloorDBAccess.GetString(dr, "DefectTime");
                    pd.GloveSize = FloorDBAccess.GetString(dr, "GloveSize");
                    pd.TierSide = FloorDBAccess.GetString(dr, "TierSide");
                }
            }
            return pd;
        }

        /// <summary>
        /// Get master list of TierSide
        /// </summary>
        /// <returns>master list of TierSide</returns>
        public static List<TierSideMasterDTO> GetTierSide()
        {
            List<TierSideMasterDTO> tierSideList = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_TierSide"))
            {
                if (dt.Rows.Count > Constants.ZERO)
                {
                    tierSideList = (from DataRow row in dt.Rows
                                    select new TierSideMasterDTO
                                    {
                                        TierSide = FloorDBAccess.GetString(row, "TierSide")
                                    }).ToList();
                }
            }
            return tierSideList;
        }

        /// <summary>
        /// Edit Production defect details
        /// </summary>
        /// <param name="SerialNumber">SerialNumber</param>
        /// <returns>PNDefectQuantity</returns>
        public static int GetPNQuantityDetails(int productionDefectId)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            int pnDefectQuantity = Constants.ZERO;
            lstfsp.Add(new FloorSqlParameter("@ProductionDefectId", productionDefectId));
            pnDefectQuantity = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_PNDefectQuantity", lstfsp));
            return pnDefectQuantity;
        }

        public static int GetPNQuantityDetailsBySerialNo(decimal serialNo)
        {
            List<FloorSqlParameter> lstfsp = new List<FloorSqlParameter>();
            int pnDefectQuantity = Constants.ZERO;
            lstfsp.Add(new FloorSqlParameter("@SerialNo", serialNo));
            pnDefectQuantity = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_PNDefectQuantityBySerialNo", lstfsp));
            return pnDefectQuantity;
        }
        #endregion
    }
}
