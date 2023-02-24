// -----------------------------------------------------------------------
// <copyright file="QCScanningBLL.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------


using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hartalega.FloorSystem.Business.Logic
{


    /// <summary>
    /// QC Scanning DTO Class
    /// </summary>
    public class QCScanningDetails
    {
        /// <summary>
        /// Serial Number generated for the batch
        /// </summary>
        public Int64 SerialNumber { get; set; }
        /// <summary>
        /// Batch Number generated for the batch
        /// </summary>
        public string BatchNumber { get; set; }
        /// <summary>
        /// Glove Type of the batch
        /// </summary>
        public string GloveType { get; set; }
        /// <summary>
        /// QC Type of the batch
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// QC Type of the batch
        /// </summary>
        public string QCType { get; set; }
        /// <summary>
        /// QC Group of the batch
        /// </summary>
        public string QCGroup { get; set; }
        /// <summary>
        /// QC Group members of the batch
        /// </summary>
        public string QCGroupMembers { get; set; }

        /// <summary>
        /// Inner box count for QC
        /// </summary>
        public int InnerBoxCount { get; set; }
        /// <summary>
        /// Packing Size for QC
        /// </summary>
        public int PackingSize { get; set; }
        /// <summary>
        /// Batch Status
        /// </summary>
        public string BatchStatus { get; set; }
        /// <summary>
        /// Packing Into
        /// </summary>
        public string PackingInto { get; set; }
        /// <summary>
        /// Batch Weight
        /// </summary>
        public string BatchWeight { get; set; }
        /// <summary>
        /// Ten Pieces Weight of the batch
        /// </summary>
        public string TenPcsWeight { get; set; }
        /// <summary>
        /// System where batch is scanned
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// Current machine where the application is installed
        /// </summary>
        public string WorkstationNumber { get; set; }
        /// <summary>
        /// Operator who uses the screen
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// PC Number from Database
        /// </summary>
        public string WorkStationId { get; set; }
        /// <summary>
        /// Description of glove type
        /// </summary>
        public string GloveTypeDescription { get; set; }
        /// <summary>
        /// Only Date
        /// </summary>
        public string ShortDate { get; set; }
        /// <summary>
        /// Only Time
        /// </summary>
        public string ShortTime { get; set; }
        /// <summary>
        /// Description of QC Type
        /// </summary>
        public string QCTypeDescription { get; set; }
        /// <summary>
        /// QC Group ID
        /// </summary>
        public int QCGroupId { get; set; }
        /// <summary>
        /// QC Rework Count
        /// </summary>
        public int QCReworkCount { get; set; }
        /// <summary>
        /// Total Pieces in the Batch
        /// </summary>
        public Int64 TotalPcs { get; set; }
        /// <summary>
        /// Batch End Time
        /// </summary>
        public string BatchEndTime { get; set; }
        /// <summary>
        /// Batch Start Time
        /// </summary>
        public string BatchStartTime { get; set; }
        /// <summary>
        /// Brand
        /// </summary>
        public string Brand { get; set; }
        public int IsNewScanInModule { get; set; }
        public string TargetTypeEnumId { get; set; }
    }

    /// <summary>
    /// QC Scanning business logic class
    /// </summary>
    public class QCScanningBLL
    {
        //public static Microsoft.Dynamics.BusinessConnectorNet.Axapta Ax = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
        //static bool isAxLogon = false;
        #region User Methods
        /// <summary>
        /// Validate and Get details if SerialNumber is valid
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static QCScanningDetails ValidateAndGetDetailsBySerialNumber(Int64 serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            DataTable dtQCScanDetails = FloorDBAccess.ExecuteDataTable("USP_GET_QCScanningDetails", PrmList);

            QCScanningDetails objQCScanDTO = new QCScanningDetails();
            if (dtQCScanDetails != null && dtQCScanDetails.Rows.Count > 0)
            {
                objQCScanDTO.SerialNumber = Convert.ToInt64(dtQCScanDetails.Rows[0]["SerialNumber"]);
                objQCScanDTO.BatchNumber = Convert.ToString(dtQCScanDetails.Rows[0]["BatchNumber"]);
                objQCScanDTO.GloveType = Convert.ToString(dtQCScanDetails.Rows[0]["GloveType"]);
                objQCScanDTO.Size = Convert.ToString(dtQCScanDetails.Rows[0]["Size"]);
                objQCScanDTO.QCType = Convert.ToString(dtQCScanDetails.Rows[0]["QCType"]);
                objQCScanDTO.QCGroup = Convert.ToString(dtQCScanDetails.Rows[0]["QCGroupName"]);
                objQCScanDTO.QCGroupMembers = Convert.ToString(dtQCScanDetails.Rows[0]["QCGroupCount"]);
                objQCScanDTO.ShortDate = Convert.ToDateTime(dtQCScanDetails.Rows[0]["BatchCardDate"]).ToString(Constants.START_DATE);
                objQCScanDTO.ShortTime = Convert.ToDateTime(dtQCScanDetails.Rows[0]["BatchCardDate"]).ToString(Constants.START_TIME);
                objQCScanDTO.QCTypeDescription = Convert.ToString(dtQCScanDetails.Rows[0]["DESCRIPTION"]);
                objQCScanDTO.TenPcsWeight = Convert.ToString(dtQCScanDetails.Rows[0]["TenPcsWeight"]);
                objQCScanDTO.BatchWeight = Convert.ToString(dtQCScanDetails.Rows[0]["BatchWeight"]);
                objQCScanDTO.ModuleName = Convert.ToString(dtQCScanDetails.Rows[0]["ModuleName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtQCScanDetails.Rows[0]["TotalPcs"])))
                {
                    objQCScanDTO.TotalPcs = Convert.ToInt64(dtQCScanDetails.Rows[0]["TotalPcs"]);
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dtQCScanDetails.Rows[0]["QCGroupId"])))
                {
                    objQCScanDTO.QCGroupId = Convert.ToInt32(dtQCScanDetails.Rows[0]["QCGroupId"]);
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dtQCScanDetails.Rows[0]["ReworkCount"])))
                {
                    objQCScanDTO.QCReworkCount = Convert.ToInt32(dtQCScanDetails.Rows[0]["ReworkCount"]);
                }
                objQCScanDTO.BatchStatus = Convert.ToString(dtQCScanDetails.Rows[0]["BatchStatus"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtQCScanDetails.Rows[0]["BatchEndTime"])))
                {
                    objQCScanDTO.BatchEndTime = Convert.ToString(dtQCScanDetails.Rows[0]["BatchEndTime"]);
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dtQCScanDetails.Rows[0]["BatchStartTime"])))
                {
                    objQCScanDTO.BatchStartTime = Convert.ToString(dtQCScanDetails.Rows[0]["BatchStartTime"]);
                }
                objQCScanDTO.Brand = Convert.ToString(dtQCScanDetails.Rows[0]["Brand"]);
                objQCScanDTO.IsNewScanInModule = Convert.ToInt32(dtQCScanDetails.Rows[0]["IsNewModule"]);
                objQCScanDTO.TargetTypeEnumId = Convert.ToString(dtQCScanDetails.Rows[0]["TargetTypeEnumId"]);

                return objQCScanDTO;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method to save Downgrade batch Cards 
        /// </summary>
        /// <param name="objDowngradeDTO"></param>
        /// <returns></returns>
        public static int SaveDowngradeBatchCard(DowngradeBatchCardDTO objDowngradeDTO)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", objDowngradeDTO.SerialNumber));
            PrmList.Add(new FloorSqlParameter("@LastModifiedBy", objDowngradeDTO.LastModifiedBy));
            PrmList.Add(new FloorSqlParameter("@LastModifiedOn", objDowngradeDTO.LastModifiedOn));
            PrmList.Add(new FloorSqlParameter("@WorkstationNumber", objDowngradeDTO.WorkstationId));
            PrmList.Add(new FloorSqlParameter("@DowngradeType", objDowngradeDTO.DowngradeType));

            return FloorDBAccess.ExecuteNonQuery("USP_SAV_DowngradeBatchCard", PrmList);
        }

        /// <summary>
        /// Method to save QC Scan batch details 
        /// </summary>
        /// <param name="objQCScanningDTO"></param>
        /// <returns></returns>
        public static int SaveQCScanDetails(QCYieldandPackingDTO objQCScanningDTO)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", objQCScanningDTO.SerialNumber));
            PrmList.Add(new FloorSqlParameter("@BatchEndTime", Convert.ToDateTime(objQCScanningDTO.BatchEndTime)));
            PrmList.Add(new FloorSqlParameter("@InnerBoxCount", objQCScanningDTO.InnerBoxCount));
            PrmList.Add(new FloorSqlParameter("@BatchStatus", objQCScanningDTO.BatchStatus));
            PrmList.Add(new FloorSqlParameter("@PackInto", objQCScanningDTO.PackInto));
            PrmList.Add(new FloorSqlParameter("@PackingSize", objQCScanningDTO.PackingSize));
            PrmList.Add(new FloorSqlParameter("@LastModifiedOn", objQCScanningDTO.LastModifiedOn));
            PrmList.Add(new FloorSqlParameter("@WorkstationNumber", objQCScanningDTO.WorkstationId));
            PrmList.Add(new FloorSqlParameter("@TenPcsWeight", objQCScanningDTO.TenPcsWeight));
            PrmList.Add(new FloorSqlParameter("@GroupMemberCount", objQCScanningDTO.QCGroupMembers));
            PrmList.Add(new FloorSqlParameter("@ModuleId", objQCScanningDTO.ModuleName));
            PrmList.Add(new FloorSqlParameter("@SubModuleId", objQCScanningDTO.SubModuleName));
            PrmList.Add(new FloorSqlParameter("@QCType", objQCScanningDTO.QCType));
            PrmList.Add(new FloorSqlParameter("@Brand", objQCScanningDTO.Brand));
            // #MH 10/11/2016 1.n FDD:HLTG-REM-003
            PrmList.Add(new FloorSqlParameter("@LooseQty", objQCScanningDTO.LooseQty));
            PrmList.Add(new FloorSqlParameter("@RejectionQty", objQCScanningDTO.RejectionQty));
            PrmList.Add(new FloorSqlParameter("@RejectedSample", objQCScanningDTO.RejectedSample));
            PrmList.Add(new FloorSqlParameter("@2ndGradeQty", objQCScanningDTO.SecondGradeQty));
            PrmList.Add(new FloorSqlParameter("@CalculatedLooseQty", objQCScanningDTO.CalculatedLooseQty));
            // #MH 10/11/2016 1.n FDD:HLTG-REM-003

            return FloorDBAccess.ExecuteNonQuery("USP_SAV_QCScanBatchCardDetails", PrmList);
        }

        /// <summary>
        /// Get TotalPcs
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static Int64 GetTotalPcs(Int64 serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToInt64(FloorDBAccess.ExecuteScalar("USP_GET_TotalPcsForBatch", PrmList));
        }


        /// <summary>
        /// Get Split Pcs
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static Int64 GetSplitPcs(Int64 serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToInt64(FloorDBAccess.ExecuteScalar("USP_GET_SplitPcsForBatch", PrmList));
        }

        /// <summary>
        /// Get Split Pcs with reject and 2nd grade Pcs
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static Int64 GetSplitPcsWithReject2ndgrade(Int64 serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToInt64(FloorDBAccess.ExecuteScalar("USP_GET_SplitPcsWithReject2ndGradePcForBatch", PrmList));
        }

        /// <summary>
        /// Method to validate if the Batch has already been downgraded.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int IsBatchDowngraded(decimal serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsBatchAlreadyDowngraded", PrmList));
        }

        /// <summary>
        /// Method to save QC Scan batch Card Pieces details 
        /// </summary>
        /// <param name="objQCScanningDTO"></param>
        /// <returns></returns>
        public static void SaveQCScanBatchCardPieces(QCYieldandPackingDTO objQCScanningDTO)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@serialNo", objQCScanningDTO.SerialNumber));
            PrmList.Add(new FloorSqlParameter("@tenPcsWeight", objQCScanningDTO.TenPcsWeight));
            PrmList.Add(new FloorSqlParameter("@batchPieces", objQCScanningDTO.BatchPcs));
            PrmList.Add(new FloorSqlParameter("@reworkReasonId", objQCScanningDTO.ReworkReasonId));
            PrmList.Add(new FloorSqlParameter("@reworkCount", objQCScanningDTO.ReworkCount));
            PrmList.Add(new FloorSqlParameter("@batchStatus", objQCScanningDTO.BatchStatus));
            PrmList.Add(new FloorSqlParameter("@lastModifiedOn", objQCScanningDTO.LastModifiedOn));
            PrmList.Add(new FloorSqlParameter("@workstationNumber", objQCScanningDTO.WorkstationId));
            PrmList.Add(new FloorSqlParameter("@module", objQCScanningDTO.ModuleName));
            if (!string.IsNullOrEmpty(objQCScanningDTO.BatchTargetTime))
            {
                PrmList.Add(new FloorSqlParameter("@targetTime", Convert.ToDateTime(objQCScanningDTO.BatchTargetTime)));
            }
            else
            {
                PrmList.Add(new FloorSqlParameter("@targetTime", objQCScanningDTO.BatchTargetTime));
            }

            PrmList.Add(new FloorSqlParameter("@subModule", objQCScanningDTO.SubModuleName));
            PrmList.Add(new FloorSqlParameter("@id", objQCScanningDTO.QCYieldAndPackinId));
            PrmList.Add(new FloorSqlParameter("@qcGroupId", objQCScanningDTO.QCGroupId));
            PrmList.Add(new FloorSqlParameter("@qcGroupMember", objQCScanningDTO.QCGroupMembers));
            PrmList.Add(new FloorSqlParameter("@memberCount", objQCScanningDTO.GroupMemberCount));
            PrmList.Add(new FloorSqlParameter("@shift", objQCScanningDTO.ShiftId));
            FloorDBAccess.ExecuteNonQuery("USP_SAV_ScanBatchCardPieces", PrmList);
        }

        /// <summary>
        /// Method to save QC Scan batch Card Weight details 
        /// </summary>
        /// <param name="objQCScanningDTO"></param>
        /// <returns></returns>
        public static void SaveQCScanBatchCardWeight(QCYieldandPackingDTO objQCScanningDTO)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@serialNo", objQCScanningDTO.SerialNumber));
            PrmList.Add(new FloorSqlParameter("@tenPcsWeight", objQCScanningDTO.TenPcsWeight));
            PrmList.Add(new FloorSqlParameter("@batchWeight", objQCScanningDTO.BatchWeight));
            PrmList.Add(new FloorSqlParameter("@reworkReasonId", objQCScanningDTO.ReworkReasonId));
            PrmList.Add(new FloorSqlParameter("@reworkCount", objQCScanningDTO.ReworkCount));
            PrmList.Add(new FloorSqlParameter("@batchStatus", objQCScanningDTO.BatchStatus));
            PrmList.Add(new FloorSqlParameter("@lastModifiedOn", objQCScanningDTO.LastModifiedOn));
            PrmList.Add(new FloorSqlParameter("@workstationNumber", objQCScanningDTO.WorkstationId));
            PrmList.Add(new FloorSqlParameter("@module", objQCScanningDTO.ModuleName));
            if (!string.IsNullOrEmpty(objQCScanningDTO.BatchTargetTime))
            {
                PrmList.Add(new FloorSqlParameter("@targetTime", Convert.ToDateTime(objQCScanningDTO.BatchTargetTime)));
            }
            else
            {
                PrmList.Add(new FloorSqlParameter("@targetTime", objQCScanningDTO.BatchTargetTime));
            }

            PrmList.Add(new FloorSqlParameter("@subModule", objQCScanningDTO.SubModuleName));
            PrmList.Add(new FloorSqlParameter("@id", objQCScanningDTO.QCYieldAndPackinId));
            PrmList.Add(new FloorSqlParameter("@qcGroupId", objQCScanningDTO.QCGroupId));
            PrmList.Add(new FloorSqlParameter("@qcGroupMember", objQCScanningDTO.QCGroupMembers));
            PrmList.Add(new FloorSqlParameter("@memberCount", objQCScanningDTO.GroupMemberCount));
            PrmList.Add(new FloorSqlParameter("@shift", objQCScanningDTO.ShiftId));
            FloorDBAccess.ExecuteNonQuery("USP_SAV_ScanBatchCardWeight", PrmList);
        }
        /*
        public static bool PostAXDataScanBatchCardWeight(QCYieldandPackingDTO objQCScanningDTO, string AxQcGroup, string AxQcTypeDesc, string AxQcType, string AxGloveType, string AxSize, string AxLocation)
        {
            bool result = true;
            string cBatch = "";
            string cCyl;
            string cGdQty;
            string cdata1;
            string cdata;
            string cReader;
            string cBtype = "";
            double nMin, nMax;
            string cQKgs; string cTTenpcs; string cTime; string cErrQty; string cQcGroup; bool lCQctype; string cOQctype = ""; string cArea = ""; bool lSmall = false;
            string cOT = ""; string cTKgs = "0";
            bool lWT = false;
            PostTreatmentBLL.LoadAXCredential();

            try
            {
                //ax.Logon(QC.Form1.Var.cAxName, "", "", QC.Form1.Var.cAxConfig);
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(PostTreatmentBLL.cAxUser, PostTreatmentBLL.cAxUserPassword, Environment.UserDomainName);
                Ax = new Axapta();
                Ax.LogonAs(PostTreatmentBLL.cAxUser, Environment.UserDomainName, nc, PostTreatmentBLL.cAxCompany, "en-us", "", PostTreatmentBLL.cAxConfig);
                isAxLogon = true;
                Ax.TTSBegin();
                object oEditMbatch, oPost;
                DateTime today = DateTime.Today;
                cQKgs = objQCScanningDTO.BatchWeight.ToString().Trim();
                cTTenpcs = objQCScanningDTO.TenPcsWeight.ToString().Trim();
                cTime = DateTime.Now.ToString("HH:mm:ss").Trim();
                cErrQty = "0";
                // cQcGroup = AxQcGroup.Substring(2, 4);
                string _QCPlant;
                //if (Form1.Var.cPlant != string.Empty)
                //{
                //    _QCPlant = oClass.StrZero("Plant", 2, Form1.Var.cPlant.Length);
                //}
                //else
                //{
                //    _QCPlant = "";
                //}
                _QCPlant = "";
                if (AxQcTypeDesc.Contains("QMAX"))
                {
                    cArea = "QMX";
                }
                if (cOQctype == AxQcType.Trim())
                {
                    lCQctype = false;
                }
                else
                {
                    lCQctype = true;
                }
                //cGdQty = Convert.ToString((int)Math.Round(((Convert.ToDouble(cQKgs) * 10000) / Convert.ToDouble(cTTenpcs))));
                cGdQty = Convert.ToString((int)((Convert.ToDouble(cQKgs) * 10000) / Convert.ToDouble(cTTenpcs)));
                if (Convert.ToDouble(cGdQty) > 0)
                {
                    oEditMbatch = Ax.CallStaticClassMethod("MIS_QC", "Edit_Mbatch", lCQctype, lSmall, objQCScanningDTO.SerialNumber.ToString().Trim(), AxQcType.Trim(), AxQcGroup.Trim(), today, cQKgs, cTTenpcs, cBtype, cOQctype, cTime, AxLocation.Trim(), cOT, cArea, "1", "02", cTKgs, cGdQty);
                    if (oEditMbatch.ToString() != "Fail")
                    {
                        oPost = Ax.CallStaticClassMethod("MIS_Tumble", "post", "QC-" + AxGloveType.Trim(), objQCScanningDTO.SerialNumber.ToString().Trim(), cGdQty, cErrQty, AxSize.Trim());
                        if (oPost.ToString() != "Fail")
                        {
                            try
                            {
                                //string ApplicationPath = @"F:\HRP\PS\NEWPS\DATA\QCMBATCH.EXE";                                
                                //string ApplicationArguments = objQCScanningDTO.SerialNumber.ToString().Trim() + " " + AxQcType.Trim() + " " + cQcGroup + " " + cTTenpcs + " " + cQKgs + " " + cOT + " " + cArea + " " + "1" + " " + _QCPlant;
                                //Process ProcessObj = new Process();
                                //ProcessObj.StartInfo.WorkingDirectory = @"F:\HRP\PS\NEWPS\DATA";
                                //ProcessObj.StartInfo.FileName = ApplicationPath;
                                //ProcessObj.StartInfo.Arguments = ApplicationArguments;
                                //ProcessObj.StartInfo.UseShellExecute = false;
                                //ProcessObj.StartInfo.CreateNoWindow = true;
                                //ProcessObj.StartInfo.RedirectStandardOutput = true;
                                //ProcessObj.Start();
                                //ProcessObj.WaitForExit();

                                //button1.Enabled = false;
                                //ClearInfo();

                                result = true;
                            }
                            catch (Exception err3)
                            {
                                ////MessageBox.Show(err3.Message, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                ////button1.Enabled = false;
                                ////ClearInfo();
                                result = false;
                            }
                        }
                        else
                        {
                            //MessageBox.Show("Posting Fail !", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //button1.Enabled = false;
                            //ClearInfo();
                            result = false;
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Fail To Update !", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //button1.Enabled = false;
                        //ClearInfo();
                        result = false;
                    }
                }
                else
                {
                    //MessageBox.Show("Weight Is Empty, Process NOT Allowed !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //button1.Enabled = false;
                    //ClearInfo();
                    result = false;
                }
                //Ax.TTSCommit();
                //Ax.Logoff();
            }
            catch (Exception err2)
            {
                if (isAxLogon)
                {
                    Ax.TTSAbort();
                    Ax.Logoff();
                    //MessageBox.Show(err2.Message, "Contact MIS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //ClearInfo();

                }
                result = false;
            }
            return result;
        }

        public static bool PostAXDataScanBatchCardInnerbox(QCYieldandPackingDTO objQCScanningDTO, string AxQcGroup, string AxQcType, string AxGloveType, string AxSize, string AxLocation)
        {
            bool result = true;
            string cBatch = "";
            string cCyl;
            string cGdQty;
            string cdata1;
            string cdata;
            string cReader;
            string cBtype = "";
            double nMin, nMax;
            string cQKgs; string cTTenpcs; string cTime; string cErrQty; string cQcGroup; bool lCQctype; string cOQctype = ""; string cArea = ""; bool lSmall = false;
            string cOT = ""; string cTKgs = "0";
            bool lWT = false;

            PostTreatmentBLL.LoadAXCredential();
            // Microsoft.Dynamics.BusinessConnectorNet.Axapta Ax = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
            try
            {
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(PostTreatmentBLL.cAxUser, PostTreatmentBLL.cAxUserPassword, Environment.UserDomainName);
                Ax = new Axapta();
                Ax.LogonAs(PostTreatmentBLL.cAxUser, Environment.UserDomainName, nc, PostTreatmentBLL.cAxCompany, "en-us", "", PostTreatmentBLL.cAxConfig);
                isAxLogon = true;
                Ax.TTSBegin();
                object oEditMbatch, oPost;
                DateTime today = DateTime.Today;
                cQKgs = objQCScanningDTO.BatchWeight.ToString().Trim();
                cTTenpcs = objQCScanningDTO.TenPcsWeight.ToString().Trim();
                cTime = DateTime.Now.ToString("HH:mm:ss").Trim();
                //cQcGroup = AxQcGroup.Substring(2, 4);
                string _QCPlant;
                //if (Form1.Var.cPlant != string.Empty)
                //{
                //    _QCPlant = oClass.StrZero(Form1.Var.cPlant, 2, Form1.Var.cPlant.Length);
                //}
                //else
                //{
                //    _QCPlant = "";
                //}
                _QCPlant = "";
                if (cOQctype == AxQcType.Trim())
                {
                    lCQctype = false;
                }
                else
                {
                    lCQctype = true;
                }
                cGdQty = Convert.ToString((int)((Convert.ToDouble(cQKgs) * 10000) / Convert.ToDouble(cTTenpcs)));
                if (Convert.ToDouble(cGdQty) > 0)
                {
                    oEditMbatch = Ax.CallStaticClassMethod("MIS_QC", "Edit_Mbatch", lCQctype, lSmall, objQCScanningDTO.SerialNumber.ToString().Trim(), AxQcType.Trim(), AxQcGroup.Trim(), today, cQKgs, cTTenpcs, cBtype, cOQctype, cTime, WorkStationDTO.GetInstance().Location.Trim(), cOT, cArea, "1", "02", cTKgs, cGdQty);
                    if (oEditMbatch.ToString() != "Fail")
                    {
                        oPost = Ax.CallStaticClassMethod("MIS_Tumble", "post", "QC-" + AxGloveType.Trim(), objQCScanningDTO.SerialNumber.ToString().Trim(), cGdQty, "0", AxSize.Trim());
                        if (oPost.ToString() != "Fail")
                        {
                            try
                            {
                                //string ApplicationPath = @"F:\HRP\PS\NEWPS\DATA\QCMBATCH.EXE";                                
                                //string ApplicationArguments = objQCScanningDTO.SerialNumber.ToString().Trim() + " " + AxQcType.Trim() + " " + cQcGroup + " " + cTTenpcs + " " + cQKgs + " " + cOT + " " + cArea + " " + QC.Form1.Var.cWeight + " " + _QCPlant;
                                //Process ProcessObj = new Process();
                                //ProcessObj.StartInfo.WorkingDirectory = @"F:\HRP\PS\NEWPS\DATA";
                                //ProcessObj.StartInfo.FileName = ApplicationPath;
                                //ProcessObj.StartInfo.Arguments = ApplicationArguments;
                                //ProcessObj.StartInfo.UseShellExecute = false;
                                //ProcessObj.StartInfo.CreateNoWindow = true;
                                //ProcessObj.StartInfo.RedirectStandardOutput = true;
                                //ProcessObj.Start();
                                //ProcessObj.WaitForExit();

                                //button1.Enabled = false;
                                //ClearInfo();
                                result = true;
                            }
                            catch (Exception err3)
                            {
                                //MessageBox.Show(err3.Message, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                //button1.Enabled = false;
                                //ClearInfo();
                                result = false;
                            }
                        }
                        else
                        {
                            //MessageBox.Show("Posting Fail !", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //button1.Enabled = false;
                            //ClearInfo();
                            result = false;
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Fail To Update !", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //button1.Enabled = false;
                        //ClearInfo();
                        result = false;
                    }
                }
                else
                {
                    //MessageBox.Show("Weight Is Empty, Process NOT Allowed !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //button1.Enabled = false;
                    //ClearInfo();
                    result = false;
                }
                // Ax.TTSCommit();
                //Ax.Logoff();
            }
            catch (Exception err2)
            {
                if (isAxLogon)
                {
                    Ax.TTSAbort();
                    Ax.Logoff();
                    //MessageBox.Show(err2.Message, "Contact MIS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //ClearInfo();
                }

                result = false;
            }
            return result;
        }

        public static bool PostAXDataQCScanDowngradeBatchCard(DowngradeBatchCardDTO objDowngradeDTO, string AxGloveType, string AxSize, string AxTenPcsWeight, string AxBatchWeight)
        {
            bool result = true;
            string c2ndGradeWip = ""; string cGdqty = "";
            PostTreatmentBLL.LoadAXCredential();

            try
            {
                //ax.Logon(QC.Form1.Var.cAxName, "", "", QC.Form1.Var.cAxConfig);
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(PostTreatmentBLL.cAxUser, PostTreatmentBLL.cAxUserPassword, Environment.UserDomainName);
                Ax = new Axapta();
                Ax.LogonAs(PostTreatmentBLL.cAxUser, Environment.UserDomainName, nc, PostTreatmentBLL.cAxCompany, "en-us", "", PostTreatmentBLL.cAxConfig);
                isAxLogon = true;
                Ax.TTSBegin();
                object oEditMbatch;
                DateTime today = DateTime.Today;
                #region Seek ItemId...
                AxaptaRecord GloveType = (AxaptaRecord)Ax.CallStaticRecordMethod("HSB_InventProductType", "find", AxGloveType.Trim());
                if (GloveType.Found)
                {
                    if (AxGloveType.Trim().Substring(0, 2) == "NR") //Latex
                    {
                        if (GloveType.get_Field("HSB_GloveCategory").ToString() == "11")//PD
                        {
                            c2ndGradeWip = "2G-001";
                        }
                        if (GloveType.get_Field("HSB_GloveCategory").ToString() == "12")//PF
                        {
                            c2ndGradeWip = "2G-002";
                        }
                    }
                    if (AxGloveType.Trim().Substring(0, 2) == "NB") //Nitrile
                    {
                        if (GloveType.get_Field("HSB_GloveCategory").ToString() == "11") //PD
                        {
                            c2ndGradeWip = "2G-003";
                        }
                        if (GloveType.get_Field("HSB_GloveCategory").ToString() == "12") //PF
                        {
                            c2ndGradeWip = "2G-004";
                        }
                    }
                }
                #endregion
                if (c2ndGradeWip != string.Empty)
                {
                    cGdqty = Convert.ToString(((Convert.ToDouble(AxBatchWeight) * 10000) / Convert.ToDouble(AxTenPcsWeight)));
                    oEditMbatch = Ax.CallStaticClassMethod("MIS_QC", "DownGrade", "QC-" + AxGloveType.Trim(), c2ndGradeWip, objDowngradeDTO.SerialNumber.ToString().Trim(), AxSize.Trim(), cGdqty, today);
                    if (oEditMbatch.ToString() != "Fail")
                    {
                        try
                        {
                            //string ApplicationPath = @"F:\HRP\PS\NEWPS\DATA\DOWNMB.EXE";
                            //string ApplicationArguments = objDowngradeDTO.SerialNumber.ToString().Trim();
                            //Process ProcessObj = new Process();
                            //ProcessObj.StartInfo.WorkingDirectory = @"F:\HRP\PS\NEWPS\DATA";
                            //ProcessObj.StartInfo.FileName = ApplicationPath;
                            //ProcessObj.StartInfo.Arguments = ApplicationArguments;
                            //ProcessObj.StartInfo.UseShellExecute = false;
                            //ProcessObj.StartInfo.CreateNoWindow = true;
                            //ProcessObj.StartInfo.RedirectStandardOutput = true;
                            //ProcessObj.Start();
                            //ProcessObj.WaitForExit();

                            //button1.Enabled = false;
                            //ClearInfo();

                            result = true;
                        }
                        catch (Exception err)
                        {
                            //MessageBox.Show(err.Message, "Contact MIS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            result = false;
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Update Fail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result = false;
                    }
                    //Ax.TTSCommit();
                    //Ax.Logoff();
                }
                else
                {
                    //MessageBox.Show("Cannot Find 2nd Grade WIP Code!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    result = false;
                }
            }
            catch (Exception err2)
            {
                if (isAxLogon)
                {
                    Ax.TTSAbort();
                    Ax.Logoff();
                    //MessageBox.Show(err2.Message, "Contact MIS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //ClearInfo();
                }
                result = false;
            }

            return result;
        }
        */
        /// <summary>
        /// Method to save QC Scan batch Card Weight details 
        /// </summary>
        /// <param name="objQCScanningDTO"></param>
        /// <returns></returns>
        public static void SaveDefectiveGlovePlatform(QCYieldandPackingDTO objQCScanningDTO)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@serialNo", objQCScanningDTO.SerialNumber));
            PrmList.Add(new FloorSqlParameter("@gloveType", objQCScanningDTO.GloveType));
            PrmList.Add(new FloorSqlParameter("@qcgroupId", objQCScanningDTO.QCGroupId));
            PrmList.Add(new FloorSqlParameter("@defectTypeId", objQCScanningDTO.DefectTypeId));
            PrmList.Add(new FloorSqlParameter("@batchKg", objQCScanningDTO.BatchWeight));
            PrmList.Add(new FloorSqlParameter("@batchGrm", objQCScanningDTO.BatchWeightGrm));
            PrmList.Add(new FloorSqlParameter("@reworkReasonId", Convert.ToInt32(objQCScanningDTO.ReworkReasonId)));
            PrmList.Add(new FloorSqlParameter("@lastModifiedOn", objQCScanningDTO.LastModifiedOn));
            PrmList.Add(new FloorSqlParameter("@workstationNumber", objQCScanningDTO.WorkstationId));
            PrmList.Add(new FloorSqlParameter("@piecesCount", objQCScanningDTO.PiecesCount));
            PrmList.Add(new FloorSqlParameter("@module", objQCScanningDTO.ModuleName));
            PrmList.Add(new FloorSqlParameter("@subModule", objQCScanningDTO.SubModuleName));
            //PrmList.Add(new FloorSqlParameter("@tenPcsWeight", objQCScanningDTO.TenPcsWeight));
            FloorDBAccess.ExecuteNonQuery("USP_SAV_DefectiveGlovePlatform", PrmList);
        }


        /// <summary>
        /// Get Defect type master data
        /// </summary>
        /// <param name="reasonType"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetDefectType()
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_DefectType", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "ProdDefectId"),
                                DisplayField = FloorDBAccess.GetString(row, "ProdDefectName")
                            }).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// Get Batch details based on Serial Number.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>DataTable as resut</returns>
        public static BatchDTO GetBatchDetails(decimal serialNo, int isDefective)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@isDefective", isDefective));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_QC_BatchDetails", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new BatchDTO
                            {
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                Size = FloorDBAccess.GetString(row, "Size"),
                                QCType = FloorDBAccess.GetString(row, "QCType"),
                                QCTypeDescription = FloorDBAccess.GetString(row, "QCTypeDescription"),
                                QCReworkCount = FloorDBAccess.GetValue<Int32>(row, "QCRework"),
                                QCGroupId = FloorDBAccess.GetValue<Int32>(row, "QCGroupId"),
                                QCGroupName = FloorDBAccess.GetString(row, "QCGroupName"),
                                BatchStartTime = FloorDBAccess.GetString(row, "BatchStartTime"),
                                QCGroupMembers = FloorDBAccess.GetString(row, "QCGroupMembers"),
                                RejectReasonId = FloorDBAccess.GetValue<Int32>(row, "ReworkReasonId"),
                                BatchWeight = FloorDBAccess.GetValue<decimal>(row, "BatchWeight"),
                                TenPcsWeight = FloorDBAccess.GetValue<decimal>(row, "TenPcsWeight")
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        /// <summary>
        /// Get Serial Number for Second Grade Sticker.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>DataTable as resut</returns>
        public static string GetSerialNumberSecondGrade()
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            string result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_SerialNumber_SecondGrade", lstParameters).ToString();
            return result;
        }

        /// <summary>
        /// Save Second Grade Sticker
        /// </summary>
        /// <param name="objQCScanningDTO"></param>
        /// <returns></returns>
        public static void SaveSecondGradeSticker(BatchDTO objBatch)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@gloveType", objBatch.GloveType));
            PrmList.Add(new FloorSqlParameter("@size", objBatch.Size));
            PrmList.Add(new FloorSqlParameter("@workstationNo", objBatch.WorkstationNumber));
            PrmList.Add(new FloorSqlParameter("@serialNo", objBatch.SerialNumber));
            PrmList.Add(new FloorSqlParameter("@locationId", objBatch.LocationId));
            PrmList.Add(new FloorSqlParameter("@lastModifiedOn", objBatch.LastModifiedOn));
            PrmList.Add(new FloorSqlParameter("@secondGradeType", objBatch.SecondGradeType));
            PrmList.Add(new FloorSqlParameter("@secondGradePCs", objBatch.SecondGradePCs));
            FloorDBAccess.ExecuteNonQuery("USP_SAV_SecondGradeSticker", PrmList);
        }

        /// <summary>
        /// Print the Second Grade Sticker 
        /// </summary>
        /// <param name="printData"></param>
        public static void PrintDetails(BatchDTO printData)
        {
            PrintDTO objPrintDTO = new PrintDTO();
            objPrintDTO.SerialNumber = printData.SerialNumber;
            objPrintDTO.GloveDesc = printData.GloveType;
            objPrintDTO.Size = printData.Size;
            objPrintDTO.DateTime = printData.LastModifiedOn.ToString();
            objPrintDTO.BatchNumber = Constants.QC_SecondGradeBatch + CommonBLL.GetCurrentDateAndTimeFromServer().ToString("yyMMdd") + "/" + printData.Size;
            objPrintDTO.SecondGradeType = printData.SecondGradeType;
            objPrintDTO.SecondGradePCs = printData.SecondGradePCs.ToString();
            FSDeviceIntegration deviceInt = new FSDeviceIntegration();
            deviceInt.PrintSticker(objPrintDTO);
        }


        /// <summary>
        /// Get how many times the batch has been split
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int GetSplitBatchCount(string serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_SplitBatchCount", PrmList));
        }

        /// <summary>
        /// Get boolean if ist QC Scan result is 'QC Type Changed'
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public static int GetQCTypeChangedForFirstTime(string serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GETQCTypeChangedForFirstTime", PrmList));
        }

        /// <summary>
        /// Delete when there is an Error in AX Posting
        /// </summary>
        /// <param name="qaidto"></param>
        /// <returns></returns>
        public static int DeleteDowngradeBatchCardData(decimal serialNumber)
        {
            int rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            PrmList.Add(new FloorSqlParameter("@LogDeleteHours", Convert.ToInt32(FloorSystemConfiguration.GetInstance().IntLogDeleteHours)));
            rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_DEL_DowngradeBatchCardData", PrmList);
            return rowsaffected;
        }

        /// <summary>
        /// Count The number of members in QC Group
        /// </summary>
        /// <param name="qaidto"></param>
        /// <returns></returns>
        public static string CountQCGroupMembers(int qcGroupId)
        {
            string members;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@qcGroupId", qcGroupId));
            members = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_QCGroup_Count", PrmList));
            return members;
        }

        /// <summary>
        /// Get Batch details based on Serial Number.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>DataTable as resut</returns>
        public static BatchDTO GetBatchDetailsPosting(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_QC_BatchDetails_Posting", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new BatchDTO
                            {
                                SerialNumber = Convert.ToString(serialNo),
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                QCType = FloorDBAccess.GetString(row, "QCType"),
                                Line = FloorDBAccess.GetString(row, "LineId"),
                                Size = FloorDBAccess.GetString(row, "Size"),
                                ShiftName = FloorDBAccess.GetString(row, "Name"),
                                TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPiecesWeight"),
                                BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                TotalPcs = FloorDBAccess.GetValue<Int32>(row, "TotalPcs"),
                                BatchType = FloorDBAccess.GetString(row, "BatchType"),
                                Location = FloorDBAccess.GetString(row, "locationname"),
                                BatchCarddate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate"),
                                Area = FloorDBAccess.GetString(row, "Area"),

                                // #MH 10/11/2016 1.n FDD:HLTG-REM-003
                                RAFGoodQtyLoose = FloorDBAccess.GetValue<int>(row, "LooseQty"),
                                SecGradeQty = FloorDBAccess.GetValue<int>(row, "SecondGradeQty"),
                                RejectedQty = FloorDBAccess.GetValue<int>(row, "RejectionQty"),
                                RejectedSampleQty = FloorDBAccess.GetValue<int>(row, "RejectedSample"),
                                CalculatedLooseQty = FloorDBAccess.GetValue<int>(row, "CalculatedLooseQty")
                                // #MH 10/11/2016 1.n FDD:HLTG-REM-003

                            }).SingleOrDefault();
            }
            return objBatch;
        }

        /// <summary>
        /// Delete the Record based on SerialNo & ReworkCount
        /// </summary>
        /// <returns>Batch Status</returns>
        public static void DeleteBatch(decimal serialNo, int reworkCount)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@reworkCount", reworkCount));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_DEL_QC_Batch", lstParameters);
        }

        /// <summary>
        /// Get Batch details based on Serial Number.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>DataTable as resut</returns>
        public static QCYieldandPackingDTO GetQCDetails(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            QCYieldandPackingDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_QCDetails", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new QCYieldandPackingDTO
                            {
                                WorkstationId = FloorDBAccess.GetString(row, "WorkStationId"),
                                LastModifiedOn = FloorDBAccess.GetValue<DateTime>(row, "LastModifiedOn"),
                                ReworkReasonId = FloorDBAccess.GetString(row, "ReworkReasonId"),
                                ModuleName = FloorDBAccess.GetString(row, "ModuleId"),
                                SubModuleName = FloorDBAccess.GetString(row, "SubModuleId"),
                                QCGroupId = FloorDBAccess.GetValue<Int32>(row, "QCGroupId"),
                                QCGroupMembers = FloorDBAccess.GetString(row, "QcGroupMembers"),
                                GroupMemberCount = FloorDBAccess.GetValue<Int32>(row, "QCGroupCount"),
                                BatchTargetTime = FloorDBAccess.GetString(row, "BatchTargetTime"),
                                BatchWeight = FloorDBAccess.GetString(row, "TotalWeight")
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        /// <summary>
        /// Method to Update QC Scan batch Card Weight details 
        /// </summary>
        /// <param name="objQCScanningDTO"></param>
        /// <returns></returns>
        public static void UpdateQCScanBatchCardWeight(QCYieldandPackingDTO objQCScanningDTO)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@serialNo", objQCScanningDTO.SerialNumber));
            if (!string.IsNullOrEmpty(objQCScanningDTO.ReworkReasonId))
            {
                PrmList.Add(new FloorSqlParameter("@reworkReasonId", objQCScanningDTO.ReworkReasonId));
            }
            PrmList.Add(new FloorSqlParameter("@reworkCount", objQCScanningDTO.ReworkCount));
            PrmList.Add(new FloorSqlParameter("@lastModifiedOn", objQCScanningDTO.LastModifiedOn));
            PrmList.Add(new FloorSqlParameter("@workstationNumber", objQCScanningDTO.WorkstationId));
            PrmList.Add(new FloorSqlParameter("@module", objQCScanningDTO.ModuleName));
            PrmList.Add(new FloorSqlParameter("@targetTime", objQCScanningDTO.BatchTargetTime));
            PrmList.Add(new FloorSqlParameter("@subModule", objQCScanningDTO.SubModuleName));
            PrmList.Add(new FloorSqlParameter("@qcGroupId", objQCScanningDTO.QCGroupId));
            PrmList.Add(new FloorSqlParameter("@qcGroupMember", objQCScanningDTO.QCGroupMembers));
            PrmList.Add(new FloorSqlParameter("@memberCount", objQCScanningDTO.GroupMemberCount));
            PrmList.Add(new FloorSqlParameter("@totalWeight", objQCScanningDTO.BatchWeight));
            FloorDBAccess.ExecuteNonQuery("USP_SAV_QC_ScanBatchCardWeight", PrmList);
        }

        /// <summary>
        /// Method to Update QC Scan batch Card Weight details 
        /// </summary>
        /// <param name="objQCScanningDTO"></param>
        /// <returns></returns>
        public static Int32 CountRework(decimal serialNo)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_ReworkCount_QC", PrmList));
        }


        /// <summary>
        /// Get the Defective Type
        /// </summary>
        /// <param name="objQCScanningDTO"></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetDefectiveGloveType()
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_DefectiveGloveType", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "DefectiveGloveType"),
                                DisplayField = FloorDBAccess.GetString(row, "DefectiveGloveType")
                            }).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// Get the Defective Reasons based on Defect Type
        /// </summary>
        /// <param name="defectType"></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetDefectiveReasons(string defectType)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@DefectType", defectType));
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_DefectiveReason", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "DefectiveGloveId"),
                                DisplayField = FloorDBAccess.GetString(row, "DefectiveGloveReason")
                            }).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// Get Glove Description
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public static string GetGloveDescription(string barcode)
        {
            List<FloorSqlParameter> lstParameter = new List<FloorSqlParameter>();
            lstParameter.Add(new FloorSqlParameter("@barcode", barcode));
            var result = FloorDBAccess.ExecuteScalar("USP_GET_GloveDescByBarcode", lstParameter);
            return result == null ? Constants.INVALID_MESSAGE : Convert.ToString(result);
        }

        /// <summary>
        /// Get Glove Description
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public static string GetGloveType(string barcode)
        {
            List<FloorSqlParameter> lstParameter = new List<FloorSqlParameter>();
            lstParameter.Add(new FloorSqlParameter("@barcode", barcode));
            var result = FloorDBAccess.ExecuteScalar("USP_GET_GloveTypeByBarcode", lstParameter);
            return result == null ? Constants.INVALID_MESSAGE : Convert.ToString(result);
        }

        /// <summary>
        /// Validate whether Serial Number is a valid Second Grade
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public static string ValidateSecondGradeSticker(string serialNo)
        {
            List<FloorSqlParameter> lstParameter = new List<FloorSqlParameter>();
            lstParameter.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_SecondGrade", lstParameter));
        }

        /// <summary>
        /// Update the Second Grade Serial Number to Verified
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public static string VerifySecondGrade(string serialNo)
        {
            List<FloorSqlParameter> lstParameter = new List<FloorSqlParameter>();
            lstParameter.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_VerifySecondGrade", lstParameter));
        }

        /// <summary>
        /// Update the Second Grade Serial Number to Disposed
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public static string DisposeSecondGrade(string serialNo)
        {
            List<FloorSqlParameter> lstParameter = new List<FloorSqlParameter>();
            lstParameter.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_DisposeSecondGrade", lstParameter));
        }

        /// <summary>
        /// Validate whether Serial Number is a valid Second Grade
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public static string ValidateRejectGradeSticker(string serialNo)
        {
            List<FloorSqlParameter> lstParameter = new List<FloorSqlParameter>();
            lstParameter.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_RejectGrade", lstParameter));
        }


        /// <summary>
        /// Validate whether Serial Number is a valid Second Grade
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public static decimal GetBatchWeightRejectGradeSticker(string serialNo)
        {
            List<FloorSqlParameter> lstParameter = new List<FloorSqlParameter>();
            lstParameter.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToDecimal(FloorDBAccess.ExecuteScalar("USP_GET_BatchWeight_SecondGrade", lstParameter));
        }

        /// <summary>
        /// Validate whether total Second Grade PCs
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static int GetTotalPCs2ndGradeSticker(string serialNo)
        {
            List<FloorSqlParameter> lstParameter = new List<FloorSqlParameter>();
            lstParameter.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_TotalPCs_SecondGrade", lstParameter));
        }

        /// <summary>
        /// Update the Second Grade Serial Number to Disposed
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public static string DisposeRejectGrade(string serialNo)
        {
            List<FloorSqlParameter> lstParameter = new List<FloorSqlParameter>();
            lstParameter.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_DisposeRejectGrade", lstParameter));
        }
        #endregion
        //public static void AXCommit()
        //{
        //    if (Ax != null)
        //    {
        //        Ax.TTSCommit();
        //        Ax.Logoff();
        //        Ax = null;
        //    }
        //}

        //public static void AXRollback()
        //{
        //    if (Ax != null)
        //    {
        //        Ax.TTSAbort();
        //        Ax.Logoff();
        //        Ax = null;
        //    }
        //}
    }
}
