using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.ServiceModel;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
//using Microsoft.Dynamics.BusinessConnectorNet;
using System.Diagnostics;

namespace Hartalega.FloorSystem.IntegrationServices
{
   
    public class AXAgentOne : IDisposable
    {
        #region Private Class Variable
        private NetTcpBinding _binding;
        private EndpointAddress _endpoint;
        private ChannelFactory<AXServices.AxInterface> _factory;
        private AXServices.AxInterface _channel;
        private static string _response;
        private static AXServices.AxInterfaceCreateRAFJournalRequest axReqRAF = null;
        private static AXServices.AxInterfaceCreateBOMJournalRequest axReqBOM = null;
        private static AXServices.AxInterfaceCreateFGRAFJournalRequest axReqFGRAF = null;
        private static AXServices.AxInterfaceCreateInvMovJournalRequest axReqINVMOV = null;
        private static AXServices.AxInterfaceCreateInvTransJournalRequest axReqINVTRAN = null;
        private static AXServices.AxInterfaceCreateRAFJournalResponse responseRAF = null;
        private static AXServices.AxInterfaceCreateBOMJournalResponse responseBOM = null;
        private static AXServices.AxInterfaceCreateFGRAFJournalResponse responseFGRAF = null;
        private static AXServices.AxInterfaceCreateInvMovJournalResponse responseINVMOV = null;
        private static AXServices.AxInterfaceCreateInvTransJournalResponse responseINVTRAN = null;
        private static AXServices.AxInterfaceFindOrCreateResponse faultResponse = null;

        private static AXServices.AxInterfaceCheckCBDItemRequest axReqCheckCBDItem = null; // #MH 20/10/2016
        private static AXServices.AxInterfaceCheckCBDItemResponse responseCheckCBDItem = null; // #MH 20/10/2016

        private static AXServices.AxInterfaceCreateInvMovJournalCBCIRequest axReqINVMOVCBCI = null; // AZMAN 29/03/2017 #AzmanCBCI
        private static AXServices.AxInterfaceCreateInvMovJournalCBCIResponse responseINVMOVCBCI = null; // AZMAN 29/03/2017 #AzmanCBCI

        #endregion

        #region Public Class Variable
        public static string connectionString;
        public static string userName;
        public static string domain;
        public static string password;
        public static string fullDomain;
        public static Guid GUID;       
        #endregion

        static AXAgentOne()
        {
            //_connectionString = ConfigurationManager.AppSettings["AXConnectionString"];
            //_userName = ConfigurationManager.AppSettings["AXUserName"];
            //_password = ConfigurationManager.AppSettings["AXPassword"];
            //_domain = ConfigurationManager.AppSettings["AXDomain"];
            //_fullDomain = ConfigurationManager.AppSettings["AXDomainFullName"];
            _response = "SUCCESS";
        }

        public AXAgentOne()
        {

        }

        private void OpenChannel()
        {
            try
            {
                _binding = new NetTcpBinding();
                Uri serviceUri = new Uri(connectionString);
                EndpointIdentity epIdentity = EndpointIdentity.CreateUpnIdentity(string.Format("{0}@{1}", userName, fullDomain));
                EndpointAddress _endpint = new EndpointAddress(serviceUri, epIdentity);
                _factory = new ChannelFactory<AXServices.AxInterface>(_binding, _endpint);
                _factory.Endpoint.Behaviors.Remove(_factory.Endpoint.Behaviors.Find<System.ServiceModel.Description.ClientCredentials>());
                System.ServiceModel.Description.ClientCredentials cdrls = new System.ServiceModel.Description.ClientCredentials();
                cdrls.Windows.ClientCredential.UserName = userName;
                cdrls.Windows.ClientCredential.Password = password;
                cdrls.Windows.ClientCredential.Domain = domain;
                _factory.Endpoint.Behaviors.Add(cdrls);
                _factory.Open();
                _channel = _factory.CreateChannel();
                ((IClientChannel)_channel).Open();
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.AX_SERVICE_UNAVAILABLE, Constants.SERVICEERROR, ex);
            }
        }

        private void CloseChannel()
        {
            if (((IClientChannel)_channel).State != CommunicationState.Closed)
            {
                ((IClientChannel)_channel).Close();
            }
            _factory.Close();
        }

        /// <summary>
        /// Create Bom Journal, _interfaceContract.BOMJournal is required
        /// </summary>
        /// <param name="_interfaceContract"></param>
        /// <returns></returns>
        public string createBOMJournal(AvaInterfaceContract _interfaceContract)
        {
            return string.Empty;
            //return PerformAction(_interfaceContract, Constants.JournalType.BOM.ToString());
        }

        /*
        public bool createBOMJournal_New(BOMJournalContract _BOMJournalContract, AvaInterfaceContract _interfaceContract, string NewGloveType)
        {
            //return PerformAction(_interfaceContract, Constants.JournalType.BOM.ToString());
            //Done by Bijay 01/11/2017---------------------------------------
            bool result = true;
            //--------------------------------------------------------------
            object oAddChange_Glove;
            object oEditMbatch;
            object oChangeWipType, oPost;
            DateTime today = DateTime.Today;

            string cBatch;
            string cCyl;
            string cGdQty;
            string cdata1;
            string cdata;
            string cReader;
            string cBtype = "";
            double nMin, nMax;
            bool lWT = false;
            string gloveType = _BOMJournalContract.ItemNumber.Trim().Substring(3);

            Microsoft.Dynamics.BusinessConnectorNet.Axapta Ax = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
            System.Net.NetworkCredential nc = new System.Net.NetworkCredential(cAxUser, cAxUserPassword, Environment.UserDomainName);
            Ax.LogonAs(cAxUser, Environment.UserDomainName, nc, cAxCompany, "en-us", "", cAxConfig);
            if (_BOMJournalContract.Shift == "C")
            {
                AxaptaRecord HSB_MIS_Shift = Ax.CreateAxaptaRecord("HSB_MIS_Shift");
                HSB_MIS_Shift.ExecuteStmt("select %1 where %1.Shift == '" + _BOMJournalContract.Shift.Trim() + "' ");
                string cTimeout = Convert.ToString(HSB_MIS_Shift.get_Field("Timeout"));
                if (Convert.ToInt32(DateTime.Now.ToString("HH:mm:ss").Trim().Substring(0, 2)) <= Convert.ToInt32(cTimeout.Substring(0, 2)))
                {
                    today = today.AddDays(-1);
                }
            }
            ////if (comboBox3.Text.Trim().Substring(0, 1) == "P")
            ////{
            ////    cCyl = comboBox3.Text.Trim().Substring(4, 2);
            ////}
            ////else
            ////{
            ////    cCyl = comboBox3.Text.Substring(3, 2);
            ////}
            ////cBatch = _BOMJournalContract.Shift + cCyl + "/" + today.ToShortDateString().Substring(6, 4) + today.ToShortDateString().Substring(3, 2) + today.ToShortDateString().Substring(0, 2) + "/" + comboBox1.Text.Trim();
            
            cGdQty = Convert.ToString((int)((Convert.ToDouble(_BOMJournalContract.BatchWeight) * 10000) / Convert.ToDouble(_BOMJournalContract.Weightof10Pcs)));

            if (NewGloveType != string.Empty)
            {
                oAddChange_Glove = Ax.CallStaticClassMethod("MIS_PT", "Add_ChangeGlove", today, DateTime.Now.ToString("HH:mm:ss").Trim(), _interfaceContract.BatchNumber.Trim(), gloveType, NewGloveType, _BOMJournalContract.Warehouse);
                if (oAddChange_Glove.ToString() != "Fail")
                {
                    oEditMbatch = Ax.CallStaticClassMethod("MIS_PT", "Update_Mbatch", _interfaceContract.BatchNumber.Trim(), _BOMJournalContract.Shift.Trim(), "CY001", today, DateTime.Now.ToString("HH:mm:ss").Trim(), _BOMJournalContract.Weightof10Pcs, _BOMJournalContract.BatchWeight, _BOMJournalContract.QCType.Trim(), _BOMJournalContract.Configuration.Trim(), cBtype);
                    if (oEditMbatch.ToString() != "Fail")
                    {
                        //Check If BTYPE == WT post PN
                        if (cBtype == "WT")
                        {
                            oPost = Ax.CallStaticClassMethod("MIS_Tumble", "post", _BOMJournalContract.ItemNumber, _interfaceContract.BatchNumber.Trim(), cGdQty, "0", _BOMJournalContract.Configuration.Trim());
                        }
                        oChangeWipType = Ax.CallStaticClassMethod("MIS_Tumble", "changeWIPType", _BOMJournalContract.ItemNumber.Trim(), _BOMJournalContract.Warehouse + "-" + NewGloveType, _interfaceContract.BatchNumber.Trim(), _BOMJournalContract.Configuration.Trim(), cGdQty);
                        if (oChangeWipType.ToString() != "Fail")
                        {
                            //UpdateOldMbatch(cBatch);
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
            Ax.TTSCommit();
            Ax.Logoff();


            return result;
        }
        */
        /// <summary>
        /// Create FGRAF Journal, _interfaceContract.TransferJournal is required
        /// </summary>
        /// <param name="_interfaceContract"></param>
        /// <returns></returns>
        public int createFGRAFJournal(AvaInterfaceContract _interfaceContract)
        {
            return PerformAction(_interfaceContract, Constants.JournalType.FGRAF.ToString());
        }

        /// <summary>
        /// Create InvMov Journal, _interfaceContract.MovementJournal is required
        /// </summary>
        /// <param name="_interfaceContract"></param>
        /// <returns></returns>
        public int createInvMovJournal(AvaInterfaceContract _interfaceContract)
        {
            return PerformAction(_interfaceContract, Constants.JournalType.MOVEMENT.ToString());
        }
        /*
        public int createInvMovJournalCBCI(AvaInterfaceContract _interfaceContract)
        {
            return PerformAction(_interfaceContract, Constants.JournalType.MOVEMENTCBCI.ToString());
        }

        public bool createInvMovJournal_New(AvaInterfaceContract _interfaceContract, AVAMovementJournalContract _MovementJournalContract)
        {
            bool result = true;
            string c2ndGradeWip = "";
            string cGdqty = "";

            Microsoft.Dynamics.BusinessConnectorNet.Axapta Ax = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
            System.Net.NetworkCredential nc = new System.Net.NetworkCredential(cAxUser, cAxUserPassword, Environment.UserDomainName);
            Ax.LogonAs(cAxUser, Environment.UserDomainName, nc, cAxCompany, "en-us", "", cAxConfig);

            Ax.TTSBegin();
            object oEditMbatch;
            DateTime today = DateTime.Today;

            #region Seek ItemId...
            AxaptaRecord GloveType = (AxaptaRecord)Ax.CallStaticRecordMethod("HSB_InventProductType", "find", _MovementJournalContract.ItemNumber.Trim());
            if (GloveType.Found)
            {
                if (_MovementJournalContract.ItemNumber.Trim().Substring(0, 2) == "NR") //Latex
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
                if (_MovementJournalContract.ItemNumber.Trim().Substring(0, 2) == "NB") //Nitrile
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
                //cGdqty = Convert.ToString(((Convert.ToDouble(textBox8.Text) * 10000) / Convert.ToDouble(textBox7.Text)));
                cGdqty = Convert.ToString(_MovementJournalContract.Quantity);
                oEditMbatch = Ax.CallStaticClassMethod("MIS_QC", "DownGrade", "QC-" + _MovementJournalContract.ItemNumber.Trim(), c2ndGradeWip, _interfaceContract.BatchNumber.Trim(), _MovementJournalContract.Configuration.Trim(), cGdqty, today);
                if (oEditMbatch.ToString() != "Fail")
                {
                    try
                    {
                        string ApplicationPath = @"F:\HRP\PS\NEWPS\DATA\DOWNMB.EXE";
                        string ApplicationArguments = _interfaceContract.BatchNumber.Trim();
                        Process ProcessObj = new Process();
                        ProcessObj.StartInfo.WorkingDirectory = @"F:\HRP\PS\NEWPS\DATA";
                        ProcessObj.StartInfo.FileName = ApplicationPath;
                        ProcessObj.StartInfo.Arguments = ApplicationArguments;
                        ProcessObj.StartInfo.UseShellExecute = false;
                        ProcessObj.StartInfo.CreateNoWindow = true;
                        ProcessObj.StartInfo.RedirectStandardOutput = true;
                        ProcessObj.Start();
                        ProcessObj.WaitForExit();
                        
                    }
                    catch (Exception err)
                    {
                        //MessageBox.Show(err.Message, "Contact MIS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else
                {
                    //MessageBox.Show("Update Fail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Ax.TTSCommit();
                Ax.Logoff();
            }
            else
            {
                //MessageBox.Show("Cannot Find 2nd Grade WIP Code!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }



            return result; 
        }
        */
        /// <summary>
        /// Create InvTrans Journal, _interfaceContract.FGJournal is required
        /// </summary>
        /// <param name="_interfaceContract"></param>
        /// <returns></returns>
        public int createInvTransJournal(AvaInterfaceContract _interfaceContract)
        {
            return PerformAction(_interfaceContract, Constants.JournalType.TRANSFER.ToString());
        }

        /// <summary>
        /// Create RAF Journal into staging, _interfaceContract.RAFStgJournal is required
        /// </summary>
        /// <param name="_interfaceContract, location"></param>
        /// <returns></returns>
        public int createRAFJournal(AvaInterfaceContract _interfaceContract)
        {
            return PerformAction(_interfaceContract, Constants.JournalType.RAF.ToString());
        }
        /*
        /// <summary>
        /// Create Rework Batch Order into staging
        /// </summary>
        /// <param name="_interfaceContract, location"></param>
        /// <returns></returns>
        public int createReworkBatchOrder(AvaInterfaceContract _interfaceContract)
        {
            return PerformAction(_interfaceContract, Constants.JournalType.RWKCR.ToString());
        }

        public bool QCScanningScanBatchCard_New(AvaInterfaceContract _interfaceContract, AVARAFStgJournalContract avarafstgJournalContract, string GloveType, string QcGroup, decimal InnerBoxCount, decimal PackingSize, decimal TotalPcs)
        {
            bool result = true;
            string cQKgs; string cTTenpcs; string cTime; string cQcGroup; string cOQctype = ""; bool lCQctype; string cGdQty; bool lSmall = false;
            string cBtype = ""; string cOT=""; string cArea = ""; string cTKgs = "";

            Microsoft.Dynamics.BusinessConnectorNet.Axapta Ax = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
            System.Net.NetworkCredential nc = new System.Net.NetworkCredential(cAxUser, cAxUserPassword, Environment.UserDomainName);
            Ax.LogonAs(cAxUser, Environment.UserDomainName, nc, cAxCompany, "en-us", "", cAxConfig);

            Ax.TTSBegin();
            object oEditMbatch, oPost;
            DateTime today = DateTime.Today;
            cQKgs = avarafstgJournalContract.BatchWeight.ToString();
            cTTenpcs = avarafstgJournalContract.Weightof10Pcs.ToString();
            cTime = DateTime.Now.ToString("HH:mm:ss").Trim();
            cQcGroup = QcGroup.Substring(2, 4);
            string _QCPlant="";
            //if (Form1.Var.cPlant != string.Empty)
            //{
            //    _QCPlant = oClass.StrZero(Form1.Var.cPlant, 2, Form1.Var.cPlant.Length);
            //}
            //else
            //{
            //    _QCPlant = "";
            //}
            if (cOQctype == avarafstgJournalContract.QCType.Trim())
            {
                lCQctype = false;
            }
            else
            {
                lCQctype = true;
            }
            //cGdQty = Convert.ToString((int)((Convert.ToDouble(cQKgs) * 10000) / Convert.ToDouble(cTTenpcs)));
            cGdQty = TotalPcs.ToString();
            if (Convert.ToDouble(cGdQty) > 0)
            {
                oEditMbatch = Ax.CallStaticClassMethod("MIS_QC", "Edit_Mbatch", lCQctype, lSmall, _interfaceContract.BatchNumber.Trim(), avarafstgJournalContract.QCType.Trim(), QcGroup.Trim(), today, cQKgs, cTTenpcs, cBtype, cOQctype, cTime, "", cOT, cArea, "", "", cTKgs, cGdQty);
                if (oEditMbatch.ToString() != "Fail")
                {
                    oPost = Ax.CallStaticClassMethod("MIS_Tumble", "post", "" + "-" + GloveType.Trim(), _interfaceContract.BatchNumber.Trim(), cGdQty, "0", avarafstgJournalContract.Configuration.Trim());
                    if (oPost.ToString() != "Fail")
                    {
                        try
                        {
                            string ApplicationPath = @"F:\HRP\PS\NEWPS\DATA\QCMBATCH.EXE";
                            //string ApplicationArguments = textBox1.Text.Trim() + " " + textBox5.Text.Trim() + " " + cQcGroup + " " + cTTenpcs + " " + cQKgs + " " + cOT + " " + cArea + " " + QC.Form1.Var.cWeight + " " + QC.Form1.Var.cScanLoc;
                            string ApplicationArguments = _interfaceContract.BatchNumber.Trim() + " " + avarafstgJournalContract.QCType.Trim() + " " + cQcGroup + " " + cTTenpcs + " " + cQKgs + " " + cOT + " " + cArea + " " + "" + " " + _QCPlant;
                            Process ProcessObj = new Process();
                            ProcessObj.StartInfo.WorkingDirectory = @"F:\HRP\PS\NEWPS\DATA";
                            ProcessObj.StartInfo.FileName = ApplicationPath;
                            ProcessObj.StartInfo.Arguments = ApplicationArguments;
                            ProcessObj.StartInfo.UseShellExecute = false;
                            ProcessObj.StartInfo.CreateNoWindow = true;
                            ProcessObj.StartInfo.RedirectStandardOutput = true;
                            ProcessObj.Start();
                            ProcessObj.WaitForExit();
                            //button1.Enabled = false;
                            //ClearInfo();
                        }
                        catch (Exception err3)
                        {
                            //MessageBox.Show(err3.Message, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            //button1.Enabled = false;
                            //ClearInfo();
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Posting Fail !", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //button1.Enabled = false;
                        //ClearInfo();
                    }
                }
                else
                {
                    //MessageBox.Show("Fail To Update !", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //button1.Enabled = false;
                    //ClearInfo();
                }
            }
            else
            {
                //MessageBox.Show("Weight Is Empty, Process NOT Allowed !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //button1.Enabled = false;
                //ClearInfo();
            }
            Ax.TTSCommit();
            Ax.Logoff();

            return result;
        }

        public bool createRAFJournal_New(AVARAFStgJournalContract _rafstgJournalContract, AvaInterfaceContract _interfaceContract)
        {
            //Done by Bijay 01/11/2017---------------------------------------
            bool result = true;
            string cBatch;
            string cCyl;
            string cGdQty;
            string cdata1;
            string cdata;
            string cReader;
            string cBtype = "";
            double nMin, nMax;
            bool lWT = false;
            //--------------------------------------------------------------

            Microsoft.Dynamics.BusinessConnectorNet.Axapta Ax = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
            System.Net.NetworkCredential nc = new System.Net.NetworkCredential(cAxUser, cAxUserPassword, Environment.UserDomainName);
            Ax.LogonAs(cAxUser, Environment.UserDomainName, nc, cAxCompany, "en-us", "", cAxConfig);

            Ax.TTSBegin();
            object oEditMbatch;
            object oPost1, oPost2;
            DateTime today = DateTime.Today;

            if (_rafstgJournalContract.Shift == "C")
            {
                AxaptaRecord HSB_MIS_Shift = Ax.CreateAxaptaRecord("HSB_MIS_Shift");
                HSB_MIS_Shift.ExecuteStmt("select %1 where %1.Shift == '" + _rafstgJournalContract.Shift.Trim() + "' ");
                string cTimeout = Convert.ToString(HSB_MIS_Shift.get_Field("Timeout"));
                if (Convert.ToInt32(DateTime.Now.ToString("HH:mm:ss").Substring(0, 2)) <= Convert.ToInt32(cTimeout.Substring(0, 2)))
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

            //cCyl = "01";
            //cBatch = _rafstgJournalContract.Shift.Trim() + cCyl + "/" + today.ToShortDateString().Substring(6, 4) + today.ToShortDateString().Substring(3, 2) + today.ToShortDateString().Substring(0, 2) + "/" + _rafstgJournalContract.Configuration.Trim();

            cGdQty = Convert.ToString((int)((Convert.ToDouble(_rafstgJournalContract.BatchWeight) * 10000) / Convert.ToDouble(_rafstgJournalContract.Weightof10Pcs)));
            //if (batchdto.GloveType.Trim() == textBox9.Text.Trim())
            //if (batchdto.GloveType.Trim() == batchdto.GloveType.Trim())
            //{
                oEditMbatch = Ax.CallStaticClassMethod("MIS_PT", "Update_Mbatch", _interfaceContract.BatchNumber.Trim(), _rafstgJournalContract.Shift.Trim(), "CY001", today, DateTime.Now.ToString("HH:mm:ss").Trim(), _rafstgJournalContract.Weightof10Pcs, _rafstgJournalContract.BatchWeight, _rafstgJournalContract.QCType.Trim(), _rafstgJournalContract.Configuration.Trim(), cBtype);
            if (oEditMbatch.ToString() != "Fail")
            {
                //Check If BTYPE == WT post PN
                if (cBtype == "WT")
                {
                    oPost2 = Ax.CallStaticClassMethod("MIS_Tumble", "post", _rafstgJournalContract.ItemNumber.Trim(), _interfaceContract.BatchNumber.Trim(), cGdQty, "0", _rafstgJournalContract.Configuration.Trim());
                }
                oPost1 = Ax.CallStaticClassMethod("MIS_Tumble", "post", _rafstgJournalContract.Warehouse, _interfaceContract.BatchNumber.Trim(), cGdQty, "0", _rafstgJournalContract.Configuration.Trim());

                result = true;

                if (oPost1.ToString() != "Fail")
                {
                    //UpdateOldMbatch(cBatch);
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
            //}
            Ax.TTSCommit();
            Ax.Logoff();

            return result;
        }

        public bool QCScanBatchCardWeight(AVARAFStgJournalContract _rafstgJournalContract, AvaInterfaceContract _interfaceContract, AVAMovementJournalContract _MovementJournalContract)
        {
            bool result = true;

            Microsoft.Dynamics.BusinessConnectorNet.Axapta Ax = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
            System.Net.NetworkCredential nc = new System.Net.NetworkCredential(cAxUser, cAxUserPassword, Environment.UserDomainName);
            Ax.LogonAs(cAxUser, Environment.UserDomainName, nc, cAxCompany, "en-us", "", cAxConfig);

            Ax.TTSBegin();
            bool lSmall = false;
            Int32 nStable;
            string cdata;
            string cdata1;
            string cReader;
            string cBWeight;
            string cGdQty;
            string cErrQty = "0";
            string cOQctype = "";
            string cBtype = "";
            string cDate;
            string cLine;
            string cTKgs;
            string cWeight;           
            bool lCQctype;
            double nMin;
            double nMax;
            string cOT = "";            
            string cArea = _rafstgJournalContract.Warehouse;
            bool lWT = false;
            object oEditMbatch, oPost;
            DateTime today = DateTime.Today;
            decimal cQKgs = _rafstgJournalContract.BatchWeight;
            decimal cTTenpcs = _rafstgJournalContract.Weightof10Pcs;
            string cTime = DateTime.Now.ToString("HH:mm:ss").Trim();
            
            string cQcGroup = _MovementJournalContract.RefConfiguration.Substring(2, 4);
            string _QCPlant;
            string gloveType = _rafstgJournalContract.ItemNumber.Trim().Substring(3);

            

            if (_rafstgJournalContract.Warehouse != string.Empty)
            {
                //_QCPlant = oClass.StrZero(Form1.Var.cPlant, 2, Form1.Var.cPlant.Length);
                _QCPlant = _rafstgJournalContract.Warehouse;
            }
            else
            {
                _QCPlant = "";
            }
            if (_MovementJournalContract.RefConfiguration.Contains("QMAX"))
            {
                cArea = "QMX";
            }
            if (cOQctype == _rafstgJournalContract.QCType)
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
                //oEditMbatch = Ax.CallStaticClassMethod("MIS_QC", "Edit_Mbatch", lCQctype, lSmall, textBox1.Text.Trim(), textBox5.Text.Trim(), comboBox1.Text.Trim(), today, cQKgs, cTTenpcs, cBtype, cOQctype, cTime, QC.Form1.Var.cLocation.Trim(), cOT, cArea, QC.Form1.Var.cWeight, QC.Form1.Var.cScanLoc, cTKgs, cGdQty);
                oEditMbatch = Ax.CallStaticClassMethod("MIS_QC", "Edit_Mbatch", lCQctype, lSmall, _interfaceContract.BatchNumber.Trim(), _rafstgJournalContract.QCType, _MovementJournalContract.RefConfiguration.Trim(), today, cQKgs, cTTenpcs, cBtype, cOQctype, cTime, _rafstgJournalContract.Warehouse, cOT, cArea, "", "", "", cGdQty);
                if (oEditMbatch.ToString() != "Fail")
                {
                    oPost = Ax.CallStaticClassMethod("MIS_Tumble", "post", _rafstgJournalContract.ItemNumber, _interfaceContract.BatchNumber.Trim(), cGdQty, cErrQty, _rafstgJournalContract.Configuration);
                    if (oPost.ToString() != "Fail")
                    {
                        string ApplicationPath = @"F:\HRP\PS\NEWPS\DATA\QCMBATCH.EXE";
                        //string ApplicationArguments = textBox1.Text.Trim() + " " + textBox5.Text.Trim() + " " + cQcGroup + " " + cTTenpcs + " " + cQKgs + " " + cOT + " " + cArea + " " + QC.Form1.Var.cWeight + " " + QC.Form1.Var.cScanLoc;
                        string ApplicationArguments = _interfaceContract.BatchNumber.Trim() + " " + _rafstgJournalContract.QCType + " " + cQcGroup + " " + cTTenpcs + " " + cQKgs + " " + cOT + " " + cArea + " " + "" + " " + _QCPlant;
                        Process ProcessObj = new Process();
                        ProcessObj.StartInfo.WorkingDirectory = @"F:\HRP\PS\NEWPS\DATA";
                        ProcessObj.StartInfo.FileName = ApplicationPath;
                        ProcessObj.StartInfo.Arguments = ApplicationArguments;
                        ProcessObj.StartInfo.UseShellExecute = false;
                        ProcessObj.StartInfo.CreateNoWindow = true;
                        ProcessObj.StartInfo.RedirectStandardOutput = true;
                        ProcessObj.Start();
                        ProcessObj.WaitForExit();
                    }
                    else
                    {
                        //MessageBox.Show("Posting Fail !", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    //MessageBox.Show("Fail To Update !", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //MessageBox.Show("Weight Is Empty, Process NOT Allowed !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);                
            }
            Ax.TTSCommit();
            Ax.Logoff();


            return result;
        }
        */
        /// <summary>
        /// Check CBD Item before post to AX, _interfaceContract.MovementJournal is required
        /// </summary>
        /// <param name="_interfaceContract"></param>
        /// <returns></returns>
        public string checkCBDItem(AvaInterfaceContract _interfaceContract)
        {
            return string.Empty;
            //return PerformAction(_interfaceContract, Constants.JournalType.CheckCBDItem.ToString());
        }
       
        public void Dispose()
        {          
            GC.SuppressFinalize(this);
        }

        public int PerformAction(AvaInterfaceContract _interfaceContract,string journalType)
        {            
            AXServices.AvaInterfaceContract interfaceContract = _interfaceContract.GetAvaInterfaceContract();
            AXServices.CallContext contextCall = new AXServices.CallContext();
            contextCall.Company = Constants.CONTEXT_COMPANY;
            interfaceContract.FSIdentifier = GUID;
            int responseArray = 0;
            //string[] responseArray = null;
            Constants.JournalType switchJournalType = (Constants.JournalType)Enum.Parse(typeof(Constants.JournalType),journalType);
            try
            {
                //OpenChannel(); // #AZ 3/5/2018 AX integration decoupled
                switch (switchJournalType)
                {
                    case Constants.JournalType.RAF:

                        //interfaceContract.RAFStgJournal = _interfaceContract.RAFStgJournal.GetAVARAFStgJournalContract();
                        
                        //axReqRAF = new AXServices.AxInterfaceCreateRAFJournalRequest(contextCall, interfaceContract);
                        //responseRAF = _channel.createRAFJournal(axReqRAF);
                        //responseArray = responseRAF.response.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case Constants.JournalType.MOVEMENT:

                        //interfaceContract.MovementJournal = _interfaceContract.MovementJournal.GetAVAMovementJournalContract();

                        //axReqINVMOV = new AXServices.AxInterfaceCreateInvMovJournalRequest(contextCall, interfaceContract);
                        //responseINVMOV = _channel.createInvMovJournal(axReqINVMOV);
                        //responseArray = responseINVMOV.response.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case Constants.JournalType.FGRAF:

                        //interfaceContract.FGJournal = _interfaceContract.FGJournal.GetAVAFGJournalContract();

                        //axReqFGRAF = new AXServices.AxInterfaceCreateFGRAFJournalRequest(contextCall, interfaceContract);
                        //responseFGRAF = _channel.createFGRAFJournal(axReqFGRAF);
                        //responseArray = responseFGRAF.response.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case Constants.JournalType.BOM:

                        //interfaceContract.BOMJournal = _interfaceContract.BOMJournal.GetBOMJournalContract();

                        //axReqBOM = new AXServices.AxInterfaceCreateBOMJournalRequest(contextCall, interfaceContract);
                        //responseBOM = _channel.createBOMJournal(axReqBOM);
                        //responseArray = responseBOM.response.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                        break;

                    case Constants.JournalType.TRANSFER:

                        //interfaceContract.TransferJournal = _interfaceContract.TransferJournal.GetAVATransferJournalContract();

                        //axReqINVTRAN = new AXServices.AxInterfaceCreateInvTransJournalRequest(contextCall, interfaceContract);
                        //responseINVTRAN = _channel.createInvTransJournal(axReqINVTRAN);
                        //responseArray = responseINVTRAN.response.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case Constants.JournalType.CheckCBDItem:// #MH 20/10/2016

                        //interfaceContract.MovementJournal = _interfaceContract.MovementJournal.GetAVAMovementJournalContract();

                        //axReqCheckCBDItem = new AXServices.AxInterfaceCheckCBDItemRequest(contextCall, interfaceContract);
                        //responseCheckCBDItem = _channel.checkCBDItem(axReqCheckCBDItem);
                        //responseArray = new string[] { "SUCCESS", responseCheckCBDItem.response.ToString() };
                        break;
                }

                return responseArray;
                //return ResponseReceived(responseArray);
            }
            catch (FaultException ex)
            {
                throw new FloorSystemException(Messages.AX_INTEGRATION_EXCEPTION, Constants.SERVICEERROR, ex);               
            }
            catch (TimeoutException)
            {
                /* Nested Try Catch used because in case of TimeOutException, a retry is made to post the data in AX.
                   This requires call to AX webservice. */   
                try
                {
                    switch (switchJournalType)
                    {
                        case Constants.JournalType.RAF:
                            faultResponse = _channel.findOrCreate(new AXServices.AxInterfaceFindOrCreateRequest(axReqRAF.CallContext, axReqRAF._interfaceContract));
                            break;
                        case Constants.JournalType.FGRAF:
                            faultResponse = _channel.findOrCreate(new AXServices.AxInterfaceFindOrCreateRequest(axReqFGRAF.CallContext, axReqFGRAF._interfaceContract));
                            break;
                        case Constants.JournalType.BOM:
                            faultResponse = _channel.findOrCreate(new AXServices.AxInterfaceFindOrCreateRequest(axReqBOM.CallContext, axReqBOM._interfaceContract));
                            break;
                        case Constants.JournalType.MOVEMENT:
                            faultResponse = _channel.findOrCreate(new AXServices.AxInterfaceFindOrCreateRequest(axReqINVMOV.CallContext, axReqINVMOV._interfaceContract));
                            break;
                        case Constants.JournalType.TRANSFER:
                            faultResponse = _channel.findOrCreate(new AXServices.AxInterfaceFindOrCreateRequest(axReqINVTRAN.CallContext, axReqINVTRAN._interfaceContract));
                            break;
                    }
                    /**#AZ 18/4/2018  Decoupled AX servives to insert into staging table in FS db
                    responseArray = faultResponse.response.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    return ResponseReceived(responseArray);
                     **/
                    return responseArray;
                }
                catch (FaultException rex)
                {
                    throw new FloorSystemException(Messages.AX_INTEGRATION_EXCEPTION, Constants.SERVICEERROR, rex);
                }
                catch (TimeoutException rex)
                {
                    throw new FloorSystemException(Messages.AX_TIMEOUT_EXCEPTION, Constants.SERVICEERROR, rex);
                }
                catch (Exception rex)
                {
                    throw new FloorSystemException(Messages.AX_SERVICE_UNAVAILABLE, Constants.SERVICEERROR, rex);
                } 
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.AX_SERVICE_UNAVAILABLE, Constants.SERVICEERROR, ex);
            }  
        }

        public string ResponseReceived(string[] responseArray)
        {
            string axTransactionId;
            string errorCode;
            string returnString;

            if (responseArray[0] == _response)
            {
                axTransactionId = responseArray[1];
                returnString = axTransactionId;
            }
            else
            {
                errorCode = responseArray[1];
                axTransactionId = Convert.ToString(Constants.MINUSONE);
                returnString = axTransactionId + "||" + errorCode;
            }
            CloseChannel();
            return returnString;
        }
    }
}

