// -----------------------------------------------------------------------
// <copyright file="QASystemBLL.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hartalega.FloorSystem.Business.Logic
{
    /// <summary>
    /// QA System business logic class
    /// </summary>
    public class QASystemBLL
    {

        #region Member Variables
        private static List<FloorSqlParameter> _params;
        #endregion

        #region Member Methods

        /// <summary>
        /// Check if Serial Number is valid.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>int</returns>
        public static int CheckSerialNoStatus(decimal serialNumber,string selectedTab)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            _params.Add(new FloorSqlParameter("@SelectedTab", selectedTab));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_SerialNoExist", _params));
        }

        /// <summary>
        /// Check if Reference Number is valid.
        /// </summary>
        /// <param name="referenceNumber"></param>
        /// <returns>string</returns>
        public static string CheckReferenceStatus(Int64 referenceNumber, string selectedTab)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@ReferenceNumber", referenceNumber));
            _params.Add(new FloorSqlParameter("@SelectedTab", selectedTab));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_ReferenceExist", _params));
        }

        /// <summary>
        /// Get most recent Reference for Serial Number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>int</returns>
        public static Int64 GetReferenceForSerialNumber(decimal serialNumber, string selectedTab)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            _params.Add(new FloorSqlParameter("@SelectedTab", selectedTab));
            return Convert.ToInt64(FloorDBAccess.ExecuteScalar("USP_GET_QA_ReferenceNumber", _params));
        }

        /// <summary>
        /// Get Serial Number for reference
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>int</returns>
        public static decimal GetSerialNumberForReference(Int64 referenceNumber, string selectedTab)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@ReferenceNumber", referenceNumber));
            _params.Add(new FloorSqlParameter("@SelectedTab", selectedTab));
            return Convert.ToDecimal(FloorDBAccess.ExecuteScalar("USP_GET_QA_SerialNumber", _params));
        }

        /// <summary>
        /// Method to save Protein/Powder/HotBox test result
        /// </summary>
        /// <param name="objQASysem"></param>
        /// <returns></returns>
        public static int SaveQATestResult(QASystemDTO objQASystem,bool isDuplicate)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@Reference", objQASystem.Reference));
            PrmList.Add(new FloorSqlParameter("@SerialNumber", objQASystem.SerialNumber));
            PrmList.Add(new FloorSqlParameter("@TestDateTime", objQASystem.TestDateTime));
            PrmList.Add(new FloorSqlParameter("@OperatorId", objQASystem.OperatorId));
            PrmList.Add(new FloorSqlParameter("@Remark", objQASystem.Remark));
            PrmList.Add(new FloorSqlParameter("@Weight", objQASystem.Weight));
            PrmList.Add(new FloorSqlParameter("@ProteinContent", objQASystem.ProteinContent));
            PrmList.Add(new FloorSqlParameter("@FilterPaperWtFirst", objQASystem.FilterPaperWtFirst));
            PrmList.Add(new FloorSqlParameter("@FilterPaperWtSecond", objQASystem.FilterPaperWtSecond));
            PrmList.Add(new FloorSqlParameter("@FilterPaperWtAfterFilteration", objQASystem.FilterPaperWtAfterFilteration));
            PrmList.Add(new FloorSqlParameter("@FilterPaperWtAndResiduePowder", objQASystem.FilterPaperWtAndResiduePowder));
            PrmList.Add(new FloorSqlParameter("@ResiduePowder", objQASystem.ResiduePowder));
            PrmList.Add(new FloorSqlParameter("@IsResultPass", objQASystem.IsResultPass));
            PrmList.Add(new FloorSqlParameter("@TestTab", objQASystem.TestTab));
            PrmList.Add(new FloorSqlParameter("@WorkstationNumber", objQASystem.WorkstationId));
            PrmList.Add(new FloorSqlParameter("@IsDuplicate", isDuplicate));

            return FloorDBAccess.ExecuteNonQuery("USP_INS_QA_TestRecord", PrmList);
        }

        /// <summary>
        /// Method to get Residue powder after calculation
        /// </summary>
        /// <param name="objQASysem"></param>
        /// <returns></returns>
        public static decimal GetResiduePowder(Int64 serialNumber, decimal filterPaperWtFirst,decimal filterPaperWtSecond,
                                               decimal filterPaperWtAfterFilteration, decimal filterPaperWtAndResiduePowder)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            PrmList.Add(new FloorSqlParameter("@FilterPaperWtFirst", filterPaperWtFirst));
            PrmList.Add(new FloorSqlParameter("@FilterPaperWtSecond", filterPaperWtSecond));
            PrmList.Add(new FloorSqlParameter("@FilterPaperWtAfterFilteration", filterPaperWtAfterFilteration));
            PrmList.Add(new FloorSqlParameter("@FilterPaperWtAndResiduePowder", filterPaperWtAndResiduePowder));

            return Convert.ToDecimal(FloorDBAccess.ExecuteScalar("USP_GET_QA_ResiduePowder", PrmList));
        }

        #endregion

        #region CR

        /// <summary>
        /// Get QA Result Details 
        /// </summary>
        /// <returns>List<QASystemDTO></returns>
        public static DataTable GetQAResultDetails(string selectedTab,Int64 referenceNumber)
        {           
            DataTable dtQASystemDetails;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SelectedTab", selectedTab));
            PrmList.Add(new FloorSqlParameter("@ReferenceNumber", referenceNumber));
            dtQASystemDetails = FloorDBAccess.ExecuteDataTable("USP_SEL_QAResultData", PrmList);
            return dtQASystemDetails;
        }

        /// <summary>
        /// Method to get Product typ for product formula
        /// </summary>
        /// <param name="objQASysem"></param>
        /// <returns></returns>
        public static int GetProductType(Int64 serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_ProductTypeforPowderTest", PrmList));
        }
        #endregion
    }
}
