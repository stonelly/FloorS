/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Dynamics.BusinessConnectorNet;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Database;
using System.Data;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework;

namespace Hartalega.FloorSystem.Business.Logic
{
    public class AX4PostingBLL
    {
        private static bool IsLogon = false;
        private static Axapta Ax;
        const string company = "h01";
        const string passPhrase = "hidden";

        public static bool GetIsLogon()
        { return IsLogon; }

        public static bool PostQAIData(string serialNo, Constants.QAIScreens screenName)
        {
            try
            {
                var axusername = EncryptDecrypt.GetDecryptedString(FloorSystemConfiguration.GetInstance().strAXUserName, passPhrase);
                var axpassword = EncryptDecrypt.GetDecryptedString(FloorSystemConfiguration.GetInstance().strAXPassword, passPhrase);
                var axconfiguration = EncryptDecrypt.GetDecryptedString(FloorSystemConfiguration.GetInstance().strAXConnectionString, passPhrase);

                Ax = new Axapta();
                NetworkCredential nc = new NetworkCredential(axusername, axpassword, Environment.UserDomainName);
                IsLogon = false;
                Ax.LogonAs(axusername, Environment.UserDomainName, nc, company, "", "", axconfiguration);
                IsLogon = true;
                Ax.TTSBegin();
                var batch = GetBatchQAIDetailsForAXPosting(serialNo);

                string functionName = string.Empty;
                int pcs = 0;
                int innerBox = 0;
                var changeQCTypeDate = new DateTime(1900, 1, 1);
                string oldQCType = string.Empty;
                string location = string.Empty;
                string reason = string.Empty;

                //  Function Module
                //1   QAI Scan Batch Weight
                //1   QAI Re-Sampling Scan
                //3   QAI Scan Inner Box
                //4   QAI Change QC Type
                //5   Scan QI Test Result
                switch (screenName)
                {
                    case Constants.QAIScreens.QAIScan:
                        functionName = "1";
                        pcs = batch.TotalPcs;
                        break;
                    case Constants.QAIScreens.QAIScanInnerTenPcs:
                        functionName = "3";
                        pcs = batch.TotalPcs;
                        innerBox = batch.InnerBoxCount;
                        break;
                    default:
                        return false;
                }

                string oCallAxClass = Ax.CallStaticClassMethod("MIS_PQI", "UpdateTable_v5",
                                                                functionName, // Function number
                                                                batch.SerialNumber, // Serial Number
                                                                batch.QCType, // QC Type
                                                                pcs, // Pcs
                                                                innerBox, // Inner Box
                                                                batch.TenPcsWeight, // TTenPcs
                                                                batch.BatchWeight,  // TKGS
                                                                changeQCTypeDate.ToString("HH:mm:ss"), // Change QCType Time
                                                                changeQCTypeDate.Date, // Change QCType Date
                                                                oldQCType, // Old QC Type
                                                                location, // Location
                                                                reason, // Reason
                                                                batch.ShiftName, // Shift
                                                                batch.Line,  // Line
                                                                batch.Size, // Size
                                                                batch.BatchCarddate.Date, // Batch Card Date
                                                                batch.BatchCarddate.ToString("HH:mm:ss"), // Batch Card Time
                                                                batch.GloveType, // Glove
                                                                batch.QAIDate?.Date,// QAI Date
                                                                batch.BatchType).ToString(); //BatchType

                return oCallAxClass == "Successful";
            }
            catch (Exception e)
            {
                throw new FloorSystemException(Messages.AX_SERVICE_UNAVAILABLE, Constants.SERVICEERROR, e);
            }
        }

        public static bool PostWaterTightBatchCardData(string serialNo)
        {
            try
            {
                var axusername = EncryptDecrypt.GetDecryptedString(FloorSystemConfiguration.GetInstance().strAXUserName, passPhrase);
                var axpassword = EncryptDecrypt.GetDecryptedString(FloorSystemConfiguration.GetInstance().strAXPassword, passPhrase);
                var axconfiguration = EncryptDecrypt.GetDecryptedString(FloorSystemConfiguration.GetInstance().strAXConnectionString, passPhrase);

                Ax = new Axapta();
                NetworkCredential nc = new NetworkCredential(axusername, axpassword, Environment.UserDomainName);
                IsLogon = false;
                Ax.LogonAs(axusername, Environment.UserDomainName, nc, company, "", "", axconfiguration);
                IsLogon = true;
                // begin transaction
                Ax.TTSBegin();

                var batch = GetBatchQAIDetailsForAXPosting(serialNo);

                string oCallAxClass = Ax.CallStaticClassMethod("MIS_Tumble", "AddMbatch",
                                                               batch.GloveType, // Glove
                                                               batch.Size, // Size
                                                               batch.ShiftName, // Shift
                                                               batch.Line,  // Line
                                                               batch.TenPcsWeight.ToString(), // TTenPcs
                                                               batch.BatchWeight.ToString(),  // TKGS
                                                               batch.SerialNumber, // Serial Number
                                                               batch.BatchCarddate.ToString("HH:mm:ss"), // Batch Card Time
                                                               batch.BatchCarddate.Date, // Batch Card Date
                                                               batch.BatchType).ToString(); //BatchType



                return oCallAxClass == "";
            }
            catch (Exception e)
            {
                throw new FloorSystemException(Messages.AX_SERVICE_UNAVAILABLE, Constants.SERVICEERROR, e);
            }
        }

        static bool IsSuccess(string oCallAxClass)
        {
            return oCallAxClass == "Successful";
        }

        public static BatchDTO GetBatchQAIDetailsForAXPosting(string serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNumber", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_BatchQAIDetailForAXPosting", lstParameters))
            {
                try
                {
                    objBatch = (from DataRow row in dtBatch.Rows
                                select new BatchDTO
                                {
                                    SerialNumber = serialNo,
                                    QCType = FloorDBAccess.GetString(row, "QCType"),
                                    TotalPcs = FloorDBAccess.GetValue<Int32>(row, "TotalPCs"),
                                    InnerBoxCount = FloorDBAccess.GetValue<Int32>(row, "InnerBox"),
                                    TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPCsWeight"),
                                    BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                    LocationId = FloorDBAccess.GetValue<int>(row, "LocationId"),
                                    ShiftId = row.Field<int?>("ShiftId"),
                                    ShiftName = FloorDBAccess.GetString(row, "ShiftName"),
                                    Line = FloorDBAccess.GetString(row, "LineId"),
                                    Size = FloorDBAccess.GetString(row, "Size"),
                                    BatchCarddate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate"),
                                    GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                    QAIDate = FloorDBAccess.GetValue<DateTime>(row, "QAIDate"),
                                    BatchType = FloorDBAccess.GetString(row, "BatchType").Trim()
                                }).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return objBatch;
        }

        public static void LogAXPostingInfo(AXPostingDTO axposting)
        {
            int rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ServiceName", axposting.ServiceName));
            PrmList.Add(new FloorSqlParameter("@PostingType", axposting.PostingType));
            PrmList.Add(new FloorSqlParameter("@PostedDate", axposting.PostedDate));
            PrmList.Add(new FloorSqlParameter("@BatchNumber", axposting.BatchNumber));
            PrmList.Add(new FloorSqlParameter("@SerialNumber", axposting.SerialNumber));
            PrmList.Add(new FloorSqlParameter("@IsPostedToAX", axposting.IsPostedToAX));
            PrmList.Add(new FloorSqlParameter("@IsPostedInAX", axposting.IsPostedInAX));
            PrmList.Add(new FloorSqlParameter("@Sequence", axposting.Sequence));
            PrmList.Add(new FloorSqlParameter("@ExceptionCode", axposting.ExceptionCode));
            PrmList.Add(new FloorSqlParameter("@TransactionID", string.Empty)); //axposting.TransactionID
            PrmList.Add(new FloorSqlParameter("@Area", axposting.Area));
            rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_SAVE_AXPOSTINGLOG", PrmList);
            // Throwing AX posting related exceptions to UI Layer.
            if (!axposting.IsPostedInAX)
            {
                throw new FloorSystemException(axposting.ExceptionCode.Trim(), Constants.AXSERVICEERROR, new Exception(axposting.ExceptionCode.Trim()), true);
            }
        }


        public static void AXCommit()
        {
            if (Ax != null)
            {
                Ax.TTSCommit();
                Ax.Logoff();
                Ax = null;
            }
        }

        public static void AXRollback()
        {
            if (Ax != null)
            {
                Ax.TTSAbort();
                Ax.Logoff();
                Ax = null;
            }
        }

    }
}
*/