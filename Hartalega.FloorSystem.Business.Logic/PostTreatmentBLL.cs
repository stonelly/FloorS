// -----------------------------------------------------------------------
// <copyright file="PostTreatmentBLL.cs" company="Avanade">
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
//using Microsoft.Dynamics.BusinessConnectorNet;
//using System.Diagnostics;
//using System.Configuration;

namespace Hartalega.FloorSystem.Business.Logic
{

    /// <summary>
    /// Post Treatment (PT) business logic class
    /// </summary>
    public static class PostTreatmentBLL
    {
        //public static Microsoft.Dynamics.BusinessConnectorNet.Axapta Ax = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
        //static bool isAxLogon = false;

        #region Public Methods

        /// <summary>
        ///  Checks whether the Batch is passed from Washer & Dryer.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>Status</returns>        
        public static string ValidateSerialNoPT(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            object result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_PT_SerialNumber", lstParameters);
            return Convert.ToString(result);
        }


        /// <summary>
        /// Get the Shift Name
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>Shift Name</returns>        
        public static string GetShiftName(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            object result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_CGT_Shift", lstParameters);
            return Convert.ToString(result);
        }

        /// <summary>
        /// Inserts or updates details in the PTScanBatchCard table.
        /// </summary>
        /// <param name="objPostTreatment"></param>
        public static int SaveScanPTBatch(PostTreatmentDTO objPostTreatment)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", objPostTreatment.SerialNumber));
            lstParameters.Add(new FloorSqlParameter("@shiftId", objPostTreatment.Shift));
            lstParameters.Add(new FloorSqlParameter("@reworkReasonId", objPostTreatment.ReworkReasonId));
            lstParameters.Add(new FloorSqlParameter("@reworkProcess", objPostTreatment.ReworkProcess));
            lstParameters.Add(new FloorSqlParameter("@reworkCount", objPostTreatment.ReworkCount));
            lstParameters.Add(new FloorSqlParameter("@locationId", objPostTreatment.LocationId));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOn", objPostTreatment.LastModifiedOn));
            lstParameters.Add(new FloorSqlParameter("@tenPcsWeight", objPostTreatment.TenPcsWeight));
            lstParameters.Add(new FloorSqlParameter("@batchWeight", objPostTreatment.BatchWeight));
            lstParameters.Add(new FloorSqlParameter("@workstationId", objPostTreatment.WorkstationNumber));
            lstParameters.Add(new FloorSqlParameter("@changeGloveType", objPostTreatment.ChangeGloveType));
            lstParameters.Add(new FloorSqlParameter("@prevGloveType", objPostTreatment.OldGloveType));
            lstParameters.Add(new FloorSqlParameter("@id", objPostTreatment.Id));
            lstParameters.Add(new FloorSqlParameter("@dryerId", objPostTreatment.DryerId));
            lstParameters.Add(new FloorSqlParameter("@batchorder", objPostTreatment.BatchOrder));
            lstParameters.Add(new FloorSqlParameter("@oldBatchOrder", objPostTreatment.OldBatchOrder));
            if (objPostTreatment.Id == Constants.ZERO)
                lstParameters.Add(new FloorSqlParameter("@authorizedFor", objPostTreatment.AuthorizedFor));
            object result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_SAV_PT_Batch", lstParameters);
            return Convert.ToInt32(result);
        }
        /*
        //Done By bijay on 08/11/2017--------------------------------
        public static string cAxCompany = "";
        public static string cAxConfig = "";
        public static string cAxUser = "";
        public static string cAxUserPassword = "";

        public static void LoadAXCredential()
        {
            cAxCompany = FloorSystemConfiguration.GetInstance().strAXCompany;
            cAxConfig = Framework.Common.EncryptDecrypt.GetDecryptedString(FloorSystemConfiguration.GetInstance().strAXConnectionString, "hidden");
            cAxUser = Framework.Common.EncryptDecrypt.GetDecryptedString(FloorSystemConfiguration.GetInstance().strAXUserName, "hidden");
            cAxUserPassword = Framework.Common.EncryptDecrypt.GetDecryptedString(FloorSystemConfiguration.GetInstance().strAXPassword, "hidden");
        }
        public static bool PostAXDataScanPTBatchCard(PostTreatmentDTO objPostTreatment, string AxLocation, string AxShift, string AxSize, string AxBatch, string AxQcType, string AxBatchType, string dryerNumber)
        {
            bool result = false;
            string cBatch = AxBatch;
            string cCyl;
            string cGdQty;
            string cdata1;
            string cdata;
            string cReader;
            string cBtype = AxBatchType;
            double nMin, nMax;
            bool lWT = false;
            LoadAXCredential();



            try
            {
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(cAxUser, cAxUserPassword, Environment.UserDomainName);
                Ax = new Axapta();
                Ax.LogonAs(cAxUser, Environment.UserDomainName, nc, cAxCompany, "en-us", "", cAxConfig);
                isAxLogon = true;
                Ax.TTSBegin();
                object oEditMbatch;
                object oPost1, oPost2;
                DateTime today = DateTime.Today;
                if (AxShift == "C")
                {
                    AxaptaRecord HSB_MIS_Shift = Ax.CreateAxaptaRecord("HSB_MIS_Shift");
                    HSB_MIS_Shift.ExecuteStmt("select %1 where %1.Shift == '" + AxShift.Trim() + "' ");
                    string cTimeout = Convert.ToString(HSB_MIS_Shift.get_Field("Timeout"));
                    if (Convert.ToInt32(DateTime.Now.ToString("HH:mm:ss").Trim().Substring(0, 2)) <= Convert.ToInt32(cTimeout.Substring(0, 2)))
                    {
                        today = today.AddDays(-1);
                    }
                }
                //if (comboBox2.Text.Trim().Substring(0, 1) == "P")
                //{
                //    cCyl = comboBox2.Text.Trim().Substring(4, 2);
                //}
                //else
                //{
                //    cCyl = comboBox2.Text.Substring(3, 2);
                //}
                //cBatch = objPostTreatment.Shift + cCyl + "/" + today.ToShortDateString().Substring(6, 4) + today.ToShortDateString().Substring(3, 2) + today.ToShortDateString().Substring(0, 2) + "/" + AxSize.Trim();                
                cGdQty = Convert.ToString((int)((Convert.ToDouble(objPostTreatment.BatchWeight.ToString()) * 10000) / Convert.ToDouble(objPostTreatment.TenPcsWeight.ToString())));
                if (objPostTreatment.OldGloveType.ToString().Trim() == objPostTreatment.OldGloveType.ToString().Trim())
                {
                    oEditMbatch = Ax.CallStaticClassMethod("MIS_PT", "Update_Mbatch", objPostTreatment.SerialNumber.ToString().Trim(), AxShift, dryerNumber, today, DateTime.Now.ToString("HH:mm:ss").Trim(), objPostTreatment.TenPcsWeight.ToString().Trim(), objPostTreatment.BatchWeight.ToString(), AxQcType, AxSize.Trim(), cBtype);
                    if (oEditMbatch.ToString() != "Fail")
                    {
                        //Check If BTYPE == WT post PN
                        if (cBtype == Constants.PWT || cBtype == Constants.QWT || cBtype == Constants.OWT || cBtype == Constants.PSW)
                        {
                         
                                                     
                            oPost2 = Ax.CallStaticClassMethod("MIS_Tumble", "post", "PN-" + objPostTreatment.OldGloveType.ToString().Trim(), objPostTreatment.SerialNumber.ToString().Trim(), cGdQty, "0", AxSize.Trim());
                            oPost1 = Ax.CallStaticClassMethod("MIS_Tumble", "post", "PT-" + objPostTreatment.OldGloveType.ToString().Trim(), objPostTreatment.SerialNumber.ToString().Trim(), cGdQty, "0", AxSize.Trim());
                            if ((oPost1.ToString() != "Fail") && (oPost2.ToString() != "Fail"))
                            {
                                result = true;
                            }
                        }
                        else
                        {
                            oPost1 = Ax.CallStaticClassMethod("MIS_Tumble", "post", "PT-" + objPostTreatment.OldGloveType.ToString().Trim(), objPostTreatment.SerialNumber.ToString().Trim(), cGdQty, "0", AxSize.Trim());
                            if (oPost1.ToString() != "Fail")
                            {
                                //Need Clarification by Bijay date: 08/11/2017-----------------------------------

                                //string ApplicationPath = @"F:\HRP\PS\NEWPS\DATA\PTMBATCH.EXE";
                                //string ApplicationArguments = objPostTreatment.SerialNumber.ToString() + " " + cBatch + " " + DateTime.Now.ToString("HH:mm:ss") + " " + objPostTreatment.TenPcsWeight.ToString() + " " + AxBatch + " " + objPostTreatment.OldGloveType.ToString().Trim() + " " + AxQcType;
                                //Process ProcessObj = new Process();
                                //ProcessObj.StartInfo.WorkingDirectory = @"F:\HRP\PS\NEWPS\DATA";
                                //ProcessObj.StartInfo.FileName = ApplicationPath;
                                //ProcessObj.StartInfo.Arguments = ApplicationArguments;
                                //ProcessObj.StartInfo.UseShellExecute = false;
                                //ProcessObj.StartInfo.CreateNoWindow = true;
                                //ProcessObj.StartInfo.RedirectStandardOutput = true;
                                //ProcessObj.Start();
                                //ProcessObj.WaitForExit();
                                result = true;
                            }

                            else
                            {
                                //MessageBox.Show("Posting To Ax Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Update To Ax MBATCH Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        result = false;
                    }
                }
                //  Ax.TTSCommit();
                //   Ax.Logoff();
            }
            catch (Exception err2)
            {
                if (isAxLogon)
                {
                    Ax.TTSAbort();
                    Ax.Logoff();

                    //MessageBox.Show(err2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //ClearInfo();
                }
                result = false;
            }
            return result;
        }

        public static bool PostAXDataChangeGloveType(PostTreatmentDTO objPostTreatment, string AxLocation, string AxShift, string AxSize, string AxBatch, string AxQcType, string AxBatchType, string AxChangeGloveTypeDesc, string dryerNumber)
        {
            bool result = false;
            string cBatch = AxBatch;
            string cCyl;
            string cGdQty;
            string cdata1;
            string cdata;
            string cReader;
            string cBtype = AxBatchType;
            double nMin, nMax;
            bool lWT = false;
            LoadAXCredential();
            // Microsoft.Dynamics.BusinessConnectorNet.Axapta Ax = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
            try
            {
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(cAxUser, cAxUserPassword, Environment.UserDomainName);
                Ax = new Axapta();
                Ax.LogonAs(cAxUser, Environment.UserDomainName, nc, cAxCompany, "en-us", "", cAxConfig);
                isAxLogon = true;
                Ax.TTSBegin();
                object oAddChange_Glove;
                object oEditMbatch;
                object oChangeWipType, oPost;
                DateTime today = DateTime.Today;
                if (AxShift == "C")
                {
                    AxaptaRecord HSB_MIS_Shift = Ax.CreateAxaptaRecord("HSB_MIS_Shift");
                    HSB_MIS_Shift.ExecuteStmt("select %1 where %1.Shift == '" + AxShift.Trim() + "' ");
                    string cTimeout = Convert.ToString(HSB_MIS_Shift.get_Field("Timeout"));
                    if (Convert.ToInt32(DateTime.Now.ToString("HH:mm:ss").Trim().Substring(0, 2)) <= Convert.ToInt32(cTimeout.Substring(0, 2)))
                    {
                        today = today.AddDays(-1);
                    }
                }
                //if (comboBox3.Text.Trim().Substring(0, 1) == "P")
                //{
                //    cCyl = comboBox3.Text.Trim().Substring(4, 2);
                //}
                //else
                //{
                //    cCyl = comboBox3.Text.Substring(3, 2);
                //}
                //cBatch = AxShift + cCyl + "/" + today.ToShortDateString().Substring(6, 4) + today.ToShortDateString().Substring(3, 2) + today.ToShortDateString().Substring(0, 2) + "/" + AxSize.Trim();                
                cGdQty = Convert.ToString((int)((Convert.ToDouble(objPostTreatment.BatchWeight.ToString()) * 10000) / Convert.ToDouble(objPostTreatment.TenPcsWeight.ToString())));
                if (AxChangeGloveTypeDesc.Trim() != string.Empty)
                {
                    oAddChange_Glove = Ax.CallStaticClassMethod("MIS_PT", "Add_ChangeGlove", today, DateTime.Now.ToString("HH:mm:ss").Trim(), objPostTreatment.SerialNumber.ToString().Trim(), objPostTreatment.OldGloveType.ToString().Trim(), objPostTreatment.ChangeGloveType.Trim(), AxLocation.Trim());
                    if (oAddChange_Glove.ToString() != "Fail")
                    {
                        oEditMbatch = Ax.CallStaticClassMethod("MIS_PT", "Update_Mbatch", objPostTreatment.SerialNumber.ToString().Trim(), AxShift.Trim(), dryerNumber, today, DateTime.Now.ToString("HH:mm:ss").Trim(), objPostTreatment.TenPcsWeight.ToString().Trim(), objPostTreatment.BatchWeight.ToString().Trim(), AxQcType.Trim(), AxSize.Trim(), cBtype);
                        if (oEditMbatch.ToString() != "Fail")
                        {
                            result = true;
                            //Check If BTYPE == WT post PN
                            if (cBtype == "PWT" || cBtype == "OWT" || cBtype == "QWT" || cBtype == "PSW")
                            {
                                oPost = Ax.CallStaticClassMethod("MIS_Tumble", "post", "PN-" + objPostTreatment.OldGloveType.ToString().Trim(), objPostTreatment.SerialNumber.ToString().Trim(), cGdQty, "0", AxSize.Trim());
                            }
                            oChangeWipType = Ax.CallStaticClassMethod("MIS_Tumble", "changeWIPType", "PN-" + objPostTreatment.OldGloveType.ToString().Trim(), "PT-" + objPostTreatment.ChangeGloveType.Trim(), objPostTreatment.SerialNumber.ToString().Trim(), AxSize.Trim(), cGdQty);
                            if (oChangeWipType.ToString() != "Fail")
                            {
                                //string ApplicationPath = @"F:\HRP\PS\NEWPS\DATA\PTMBATCH.EXE";
                                //string ApplicationArguments = objPostTreatment.SerialNumber.ToString().Trim() + " " + cBatch + " " + DateTime.Now.ToString("HH:mm:ss").Trim() + " " + objPostTreatment.TenPcsWeight.ToString().Trim() + " " + objPostTreatment.BatchWeight.ToString().Trim() + " " + objPostTreatment.ChangeGloveType.ToString().Trim() + " " + AxQcType.Trim();
                                //Process ProcessObj = new Process();
                                //ProcessObj.StartInfo.WorkingDirectory = @"F:\HRP\PS\NEWPS\DATA";
                                //ProcessObj.StartInfo.FileName = ApplicationPath;
                                //ProcessObj.StartInfo.Arguments = ApplicationArguments;
                                //ProcessObj.StartInfo.UseShellExecute = false;
                                //ProcessObj.StartInfo.CreateNoWindow = true;
                                //ProcessObj.StartInfo.RedirectStandardOutput = true;
                                //ProcessObj.Start();
                                //ProcessObj.WaitForExit();
                                result = true;
                            }
                            else
                            {
                                //MessageBox.Show("Posting To Ax Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                result = false;
                            }
                        }
                        else
                        {
                            //MessageBox.Show("Update To Ax MBATCH Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            result = false;
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Cannot Insert Record In Change Glove Type DataBase!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        result = false;
                    }
                }
                else
                {
                    //MessageBox.Show("Change Glove Type To ?", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        */
        /// <summary>
        /// Creates a new Reference Number for the Test Slip & Gets that Reference Number 
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>Reference Number</returns>        
        public static string GetReferenceNumber(string testSlip)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@testSlip", testSlip));
            var result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_ReferenceNumber", lstParameters);
            return Convert.ToString(result);
        }

        /// <summary>
        /// Check Serial No status for Protein test.
        /// </summary>
        /// <param name="serialNo">Serial Number</param>
        /// <returns>Status as result.It will not throw exception for null case.</returns>
        public static TestSlipDTO GetStatusProteinTest(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            TestSlipDTO objTestSlip = new TestSlipDTO();
            using (DataTable dtResult = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_SerialNo_ProteinTest", lstParameters))
            {
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    objTestSlip.Status = FloorDBAccess.GetString(dtResult.Rows[0], "Status");
                    objTestSlip.ReworkCount = FloorDBAccess.GetValue<Int32>(dtResult.Rows[0], "ReworkCount");
                    objTestSlip.Reprint = FloorDBAccess.GetValue<Int32>(dtResult.Rows[0], "Reprint");
                    objTestSlip.TesterID = FloorDBAccess.GetString(dtResult.Rows[0], "TesterId");
                    objTestSlip.Name = FloorDBAccess.GetString(dtResult.Rows[0], "Name");
                    objTestSlip.ReferenceId = FloorDBAccess.GetValue<decimal>(dtResult.Rows[0], "ReferenceId");
                    objTestSlip.LastModifiedOn = Convert.ToDateTime(dtResult.Rows[0]["TestSlipGeneratedDate"]);
                }
            }
            return objTestSlip;
        }

        /// <summary>
        /// Check Serial No status for Hot Box test.
        /// </summary>
        /// <param name="serialNo">Serial Number</param>
        /// <returns>Status as result.It will not throw exception for null case.</returns>
        public static TestSlipDTO GetStatusHotBoxTest(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            TestSlipDTO objTestSlip = new TestSlipDTO();
            using (DataTable dtResult = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_SerialNo_HotBoxTest", lstParameters))
            {
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    objTestSlip.Status = FloorDBAccess.GetString(dtResult.Rows[0], "Status");
                    objTestSlip.ReworkCount = FloorDBAccess.GetValue<Int32>(dtResult.Rows[0], "ReworkCount");
                    objTestSlip.Reprint = FloorDBAccess.GetValue<Int32>(dtResult.Rows[0], "Reprint");
                    objTestSlip.TesterID = FloorDBAccess.GetString(dtResult.Rows[0], "TesterId");
                    objTestSlip.Name = FloorDBAccess.GetString(dtResult.Rows[0], "Name");
                    objTestSlip.ReferenceId = FloorDBAccess.GetValue<decimal>(dtResult.Rows[0], "ReferenceId");
                    objTestSlip.LastModifiedOn = Convert.ToDateTime(dtResult.Rows[0]["TestSlipGeneratedDate"]);
                }
            }
            return objTestSlip;
        }

        /// <summary>
        /// Check Serial No status for Powder test.
        /// </summary>
        /// <param name="serialNo">Serial Number</param>
        /// <returns>Status as result.It will not throw exception for null case.</returns>
        public static TestSlipDTO GetStatusPowderTest(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            TestSlipDTO objTestSlip = new TestSlipDTO();
            using (DataTable dtResult = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_SerialNo_PowderTest", lstParameters))
            {
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    objTestSlip.Status = FloorDBAccess.GetString(dtResult.Rows[0], "Status");
                    objTestSlip.ReworkCount = FloorDBAccess.GetValue<Int32>(dtResult.Rows[0], "ReworkCount");
                    objTestSlip.Reprint = FloorDBAccess.GetValue<Int32>(dtResult.Rows[0], "Reprint");
                    objTestSlip.TesterID = FloorDBAccess.GetString(dtResult.Rows[0], "TesterId");
                    objTestSlip.Name = FloorDBAccess.GetString(dtResult.Rows[0], "Name");
                    objTestSlip.ReferenceId = FloorDBAccess.GetValue<decimal>(dtResult.Rows[0], "ReferenceId");
                    objTestSlip.LastModifiedOn = Convert.ToDateTime(dtResult.Rows[0]["TestSlipGeneratedDate"]);
                }
            }
            return objTestSlip;
        }

        /// <summary>
        /// Inserts Protein Test Slip details.
        /// </summary>
        /// <param name="objPostTreatment"></param>
        public static void SaveProteinTestSlip(TestSlipDTO objTestSlip)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@locationId", objTestSlip.LocationId));
            lstParameters.Add(new FloorSqlParameter("@testerId", objTestSlip.TesterID));
            lstParameters.Add(new FloorSqlParameter("@serialNo", objTestSlip.SerialNumber));
            lstParameters.Add(new FloorSqlParameter("@referenceId", objTestSlip.ReferenceId));
            lstParameters.Add(new FloorSqlParameter("@reworkCount", objTestSlip.ReworkCount));
            lstParameters.Add(new FloorSqlParameter("@reprintCount", objTestSlip.Reprint));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOn", objTestSlip.LastModifiedOn));
            lstParameters.Add(new FloorSqlParameter("@workstationId", objTestSlip.WorkstationId));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_SAV_ProteinTestSlip", lstParameters);
        }

        /// <summary>
        /// Updates Protein Test Slip details.
        /// </summary>
        /// <param name="objPostTreatment"></param>
        public static void UpdateProteinTestSlip(decimal serialNo, Int32 reprint, DateTime lastModifiedOn)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@reprintCount", reprint));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOn", lastModifiedOn));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_SAV_ProteinTestSlip_Reprint", lstParameters);
        }

        /// <summary>
        /// Inserts or updates Hot Box Test Slip details.
        /// </summary>
        /// <param name="objPostTreatment"></param>
        public static void SaveHotBoxTestSlip(TestSlipDTO objTestSlip)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@locationId", objTestSlip.LocationId));
            lstParameters.Add(new FloorSqlParameter("@testerId", objTestSlip.TesterID));
            lstParameters.Add(new FloorSqlParameter("@serialNo", objTestSlip.SerialNumber));
            lstParameters.Add(new FloorSqlParameter("@referenceId", objTestSlip.ReferenceId));
            lstParameters.Add(new FloorSqlParameter("@reworkCount", objTestSlip.ReworkCount));
            lstParameters.Add(new FloorSqlParameter("@reprintCount", objTestSlip.Reprint));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOn", objTestSlip.LastModifiedOn));
            lstParameters.Add(new FloorSqlParameter("@workstationId", objTestSlip.WorkstationId));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_SAV_HotBoxTestSlip", lstParameters);
        }

        /// <summary>
        /// Updates Hot Box Test Slip details.
        /// </summary>
        /// <param name="objPostTreatment"></param>
        public static void UpdateHotBoxTestSlip(decimal serialNo, Int32 reprint, DateTime lastModifiedOn)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@reprintCount", reprint));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOn", lastModifiedOn));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_SAV_HotBoxTestSlip_Reprint", lstParameters);
        }

        /// <summary>
        /// Inserts or updates Powder Test Slip details.
        /// </summary>
        /// <param name="objPostTreatment"></param>
        public static void SavePowderTestSlip(TestSlipDTO objTestSlip)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@locationId", objTestSlip.LocationId));
            lstParameters.Add(new FloorSqlParameter("@testerId", objTestSlip.TesterID));
            lstParameters.Add(new FloorSqlParameter("@serialNo", objTestSlip.SerialNumber));
            lstParameters.Add(new FloorSqlParameter("@referenceId", objTestSlip.ReferenceId));
            lstParameters.Add(new FloorSqlParameter("@reworkCount", objTestSlip.ReworkCount));
            lstParameters.Add(new FloorSqlParameter("@reprintCount", objTestSlip.Reprint));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOn", objTestSlip.LastModifiedOn));
            lstParameters.Add(new FloorSqlParameter("@workstationId", objTestSlip.WorkstationId));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_SAV_PowderTestSlip", lstParameters);
        }

        /// <summary>
        /// Updates Powder Test Slip details.
        /// </summary>
        /// <param name="objPostTreatment"></param>
        public static void UpdatePowderTestSlip(decimal serialNo, Int32 reprint, DateTime lastModifiedOn)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@reprintCount", reprint));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOn", lastModifiedOn));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_SAV_PowderTestSlip_Reprint", lstParameters);
        }


        /// <summary>
        /// Print the Test Slip 
        /// </summary>
        /// <param name="printData"></param>
        public static void PrintDetails(PrintTestSlipDTO printData)
        {
            FSDeviceIntegration deviceInt = new FSDeviceIntegration();
            deviceInt.PrintTestSlip(printData);
        }

        /// <summary>
        /// Get Batch details based on Serial Number for Hot box Test Slip printing.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>PrintTestSlipDTO as resut</returns>
        public static PrintTestSlipDTO GetBatchDetailsForHotBox(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            PrintTestSlipDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_SerialNumber_HotBox", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new PrintTestSlipDTO
                            {
                                PTDate = FloorDBAccess.GetString(row, "PTDate"),
                                WasherProgram = FloorDBAccess.GetString(row, "WasherProgram"),
                                WasherNumber = FloorDBAccess.GetString(row, "WasherNumber"),
                                DryerProgram = FloorDBAccess.GetString(row, "DryerProgram"),
                                DryerNumber = FloorDBAccess.GetString(row, "DryerNumber")
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        /// <summary>
        /// Get Batch Weight & Ten Pcs Weight based on Serial Number for Test Slip printing.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>BatchDTO as result</returns>
        public static BatchDTO GetUpdatedBatchWeight(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_BatchWeight", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new BatchDTO
                            {
                                BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPCsWeight")
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        /// <summary>
        /// Get the Reference Number based on Serial No
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>Reference Number</returns>        
        public static string GetReferenceNumber(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            var result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_ReferenceNumber_SerialNo", lstParameters);
            return Convert.ToString(result);
        }

        /// <summary>
        /// Get the Serial No based on Reference Number
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>Reference Number</returns>        
        public static string GetSerialNumber(decimal refNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@referenceNumber", refNo));
            var result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_SerialNo_ReferenceNumber", lstParameters);
            return Convert.ToString(result);
        }

        /// <summary>
        /// Saves the Polymer Test Result details.
        /// </summary>
        /// <param name="objPostTreatment"></param>
        public static void SavePolymerTestResult(TestSlipDTO objTestSlip)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@locationId", objTestSlip.LocationId));
            lstParameters.Add(new FloorSqlParameter("@testerId", objTestSlip.TesterID));
            lstParameters.Add(new FloorSqlParameter("@serialNo", objTestSlip.SerialNumber));
            lstParameters.Add(new FloorSqlParameter("@referenceId", objTestSlip.ReferenceId));
            lstParameters.Add(new FloorSqlParameter("@result", objTestSlip.Result));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOn", objTestSlip.LastModifiedOn));
            lstParameters.Add(new FloorSqlParameter("@workstationId", objTestSlip.WorkstationId));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_SAV_PolymerTestResult", lstParameters);
        }

        /// <summary>
        /// Get the Batch status. If not Straight Pack then return null
        /// </summary>
        /// <returns>Batch Status</returns>
        public static string GetBatchStatus(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            object result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_BatchStatus", lstParameters);
            return Convert.ToString(result);
        }

        /// <summary>
        /// Get the QAI  Batch status. If not Straight Pack then return null
        /// </summary>
        /// <returns>Batch Status</returns>
        public static string GetQAIBatchStatus(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            object result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_QAIBatchStatus", lstParameters);
            return Convert.ToString(result);
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
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_DEL_PT_Batch", lstParameters);
        }

        /// <summary>
        /// Inactives PWTBCA in staging after PTScan rollback
        /// </summary>
        /// <returns></returns>
        public static void RollbackPWT(string serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_Rollback_PWT", lstParameters);
        }

        /// <summary>
        /// Get LastModified Date
        /// </summary>
        /// <returns>Last Modified date</returns>
        public static DateTime GetLastModifiedPT(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToDateTime(Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_ScanPT_LastModified", lstParameters));
        }

        /// <summary>
        /// Rollback the LastModified Date
        /// </summary>
        public static void SaveLastModifiedPT(decimal serialNo, DateTime lastModifiedOn, string prevGloveType, string changeGloveType, DateTime lastModifiedCGT)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOn", lastModifiedOn));
            lstParameters.Add(new FloorSqlParameter("@prevGloveType", prevGloveType));
            lstParameters.Add(new FloorSqlParameter("@changeGloveType", changeGloveType));
            lstParameters.Add(new FloorSqlParameter("@lastModifiedOnCGT", lastModifiedCGT));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_SAV_ScanPT_LastModified", lstParameters);
        }

        /// <summary>
        ///Get The Batch Type
        /// </summary>
        public static string GetBatchType(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            var result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_BatchStatusPosting", lstParameters);
            return result == null ? String.Empty : result.ToString().Trim();
        }

        /// <summary>
        /// Get the DryerId
        /// </summary>
        /// <param name="objPostTreatment"></param>
        public static Int32 GetDryerId(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            Int32 result = Convert.ToInt32(Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_PT_DRYER", lstParameters));
            return result;
        }

        /// <summary>
        /// Check whether the AX posting for the serial number for that Service name has already been done.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static Decimal GetPTLatestBatchWeight(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            decimal result = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("USP_GET_PT_LatestBatchWeight", lstParameters));
            return result;
        }

        /// <summary>
        /// Check whether the AX posting for the serial number for that Service name has already been done.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static Decimal GetPTLatestTenPcsWeight(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            decimal result = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("USP_GET_PT_LatestTenPcsWeight", lstParameters));
            return result;
        }

        /// <summary>
        /// Get total pieces from Batch Card for the serial number has reported.
        /// MH 4/6/2018
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static Decimal GetBatchTotalPcs(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNo));
            decimal result = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("USP_GET_TotalPcsForBatch", lstParameters));
            return result;
        }

        /// <summary>
        /// Get remaining pieces from Batch Card for the serial number has reported.
        /// AZRUL 10/1/2019
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static Decimal GetRemainingBatchTotalPcs(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNo));
            decimal result = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("USP_GET_TotalPcsForQCPackingYield", lstParameters));
            return result;
        }

        /// <summary>
        /// Added by Tan Wei Wah 20190305 - Get the latest total pcs by serial number
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static int GetLatestTotalPcs(decimal serialNo)
        {
            DataTable latest_details;
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            latest_details = FloorDBAccess.ExecuteDataTable("usp_GIS_GetBatchInfo", lstParameters);

            if (latest_details != null && latest_details.Rows.Count > Constants.ZERO)
            {
                return FloorDBAccess.GetValue<int>(latest_details.Rows[0], "TotalPcs");
            }
            else
            {
                return 0;
            }
        }

        public static string GetPTScanValidationErrorMessage(decimal serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return FloorDBAccess.ExecuteScalar("USP_VAL_PTScanInProcess", PrmList).ToString();
        }

        public static void DeleteBatchAuditLog(Int64 auditLogID)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@AuditLogID", auditLogID));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_DEL_BatchAuditLog", lstParameters);
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
