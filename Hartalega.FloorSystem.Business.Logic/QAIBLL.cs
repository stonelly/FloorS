// -----------------------------------------------------------------------
// <copyright file="QAIBLL.cs" company="Avanade">
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
    using System.Linq;
    using System.ServiceModel;
    using System.Transactions;
    using System.Diagnostics;
    using Hartalega.FloorSystem.IntegrationServices;
    using Hartalega.FloorSystem.Framework.Common;
    #endregion



    /// <summary>
    /// QAI business logic class
    /// </summary>
    public class QAIBLL : Framework.Business.BusinessBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QAIBLL" /> class.
        /// </summary>

        #region Member Variables
        private string _axTransactionId;
        private static BatchDTO _batchdto;
        private static int _subModuleId;
        private static bool _isPostingSuccess;
        private static string _message;
        private static int _qAIExpiryDays = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intQAIExpiryDuration);
        private static string _wt_SuggestedQCType;
        #endregion

        #region QAI
        public static string KeyStokeReturnChar(string Key)
        {
            switch (Key)
            {
                case "oemquestion":                
                    return (@"/");
                case "oempipe": 
                case "oem5":
                    return (@"\");
                case "oemclosebrackets":
                case "oem6":
                    return (@"]");
                case "oemopenbrackets":
                    return (@"[");
                case "oemcomma":
                    return (@",");
                case "oemperiod":
                    return (@".");
                case "oem1":
                    return (@";");
                case "oem7":
                    return (@"'");
                default:
                    return Key;
            }
            
        }

        /// <summary>
        /// Get QAIDefect Type List
        /// </summary>
        /// <returns></returns>
        public static List<QAIDefectType> GetQAIDefectTypeList(string serialNumber = null)
        {
            List<QAIDefectType> defecttypeLst = new List<QAIDefectType>();
            List<QAIDefectDTO> defectList = new List<QAIDefectDTO>();

            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QAIDEFECTLIST", null))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            QAIDefectType qaidefecttype = new QAIDefectType();
                            qaidefecttype.DefectCategory = FloorDBAccess.GetString(dr, "DefectCategory");
                            qaidefecttype.Sequence = FloorDBAccess.GetValue<int>(dr, "Sequence");
                            qaidefecttype.DefectCategoryID = FloorDBAccess.GetValue<int>(dr, "DefectCategoryId");
                            if (string.IsNullOrEmpty(serialNumber))
                            {
                                qaidefecttype.DefectList = GetDefectDetails(FloorDBAccess.GetValue<int>(dr, "DefectCategoryId"));
                            }
                            else
                            {
                                qaidefecttype.DefectList = GetDefectDetails(FloorDBAccess.GetValue<int>(dr, "DefectCategoryId"), serialNumber);
                            }
                            defecttypeLst.Add(qaidefecttype);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.QAI_DEFECTLIST_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return defecttypeLst;
        }

        private static List<QAIDefectDTO> GetDefectDetails(int DefectCategoryID, string serialNumber = null)
        {
            List<QAIDefectDTO> defectList = new List<QAIDefectDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@DefectCategoryID", DefectCategoryID));
            if (!string.IsNullOrEmpty(serialNumber))
            {
                prmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            }
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_DEFECTMASTER", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            QAIDefectDTO defectDTO = new QAIDefectDTO();
                            defectDTO.DefectItem = FloorDBAccess.GetString(dr, "DefectItem");
                            defectDTO.KeyStroke = Convert.ToChar(dr["KeyStroke"]);
                            defectDTO.DefectID = FloorDBAccess.GetValue<Int32>(dr, "DefectID");
                            defectDTO.KeyStrokeAltName = FloorDBAccess.GetString(dr, "KeyStrokeAltName");
                            defectDTO.Count = FloorDBAccess.GetValue<Int32>(dr, "NoOfDefects");
                            if (string.IsNullOrEmpty(serialNumber))
                            {
                                defectDTO.DefectPositionList = GetDefectPositions(FloorDBAccess.GetValue<int>(dr, "DefectID"));
                            }
                            else
                            {
                                defectDTO.DefectPositionList = GetDefectPositions(FloorDBAccess.GetValue<int>(dr, "DefectID"), serialNumber);
                            }
                            defectDTO.HasChild = (defectDTO.DefectPositionList.Count() > 0);
                            defectList.Add(defectDTO);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.DEFECT_DETAILS_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return defectList;
        }
        private static List<QAIDefectPositionDTO> GetDefectPositions(int DefectID, string serialNumber = null)
        {
            List<QAIDefectPositionDTO> defectPositionList = new List<QAIDefectPositionDTO>();
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@DefectID", DefectID));
            if (!string.IsNullOrEmpty(serialNumber))
            {
                prmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            }
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_DefectPositionByDefectId", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            QAIDefectPositionDTO defectPositionDTO = new QAIDefectPositionDTO();
                            defectPositionDTO.DefectPositionItem = FloorDBAccess.GetString(dr, "DefectPositionItem");
                            defectPositionDTO.KeyStroke = Convert.ToChar(dr["KeyStroke"]);
                            defectPositionDTO.DefectPositionID = FloorDBAccess.GetValue<Int32>(dr, "DefectPositionID");
                            defectPositionDTO.KeyStrokeAltName = FloorDBAccess.GetString(dr, "KeyStrokeAltName");
                            defectPositionDTO.Count = FloorDBAccess.GetValue<Int32>(dr, "NoOfDefects");
                            defectPositionList.Add(defectPositionDTO);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.DEFECT_DETAILS_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return defectPositionList;
        }

        /// <summary>
        /// Qc type change Validation 
        /// </summary>
        /// <param name="QCTypeOld"></param>
        /// <param name="QCTypeNew"></param>
        /// <returns>bool</returns>
        public static bool ChangeQctypeValidation(string QCTypeOld, string QCTypeNew)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@QCTypeOld", QCTypeOld));
            PrmList.Add(new FloorSqlParameter("@QCTypeNew", QCTypeNew));
            return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("Usp_QAI_Get_ChangeQctypeValidation", PrmList));
        }

        /// <summary>
        /// Get Route Category 
        /// </summary>
        /// <param name="QCType"></param>
        /// <returns>string</returns>
        public static string GetRouteCategory(string QCType)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@QCType", QCType));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_ReworkCategory", PrmList));
        }

        /// <summary>
        /// Get Previous OQC Rework
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>bool</returns>
        public static bool GetPreviousReworkIsOQC(string serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@serialNumber", serialNumber));
            return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_PreviousReworkIsOQC", PrmList));
        }

        /// <summary>
        /// Get Previous OQC Rework is without SOBC
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>bool</returns>
        public static bool IsPrevReworkWithoutSOBC(string serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@serialNumber", serialNumber));
            return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_IsPrevReworkWithoutSOBC", PrmList));
        }

        /// <summary>
        /// Save Accumulated WTSampling
        /// </summary>
        /// <param name="WTSamplingSize"></param>
        /// <returns>bool</returns>
        public static void SaveWTSamplingQCQI(string SerialNo, Decimal WTSamplingSize)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNo", SerialNo));
            PrmList.Add(new FloorSqlParameter("@WTSamplingSize", WTSamplingSize));
            Convert.ToString(FloorDBAccess.ExecuteScalar("USP_SaveWTSamplingQCQI", PrmList));
        }

        /// <summary>
        /// SaveQAIData
        /// </summary>
        /// <param name="qaidto"></param>
        /// <param name="WorkStationId"></param>
        /// <returns></returns>
        public static int SaveQAIData(QAIDTO qaidto, string WorkStationId, bool? IsCreateRework = null)
        {
            Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "strat SaveQAIData", DateTime.Now));
            int qaiId = 0;

            try
            {
                List<DropdownDTO> qctypelst = CommonBLL.GetQCType();
                string Qctype = (from qc in qctypelst
                                 where qc.IDField.Trim().ToLower() == Constants.RESAMPLE.ToString().Trim().ToLower()
                                 select qc.DisplayField).FirstOrDefault();

                bool isPostingSuccess = false;
                string isQIPass;
                string qaidetails = CommonBLL.SerializeTOXML(qaidto);
                bool IsExpiredBatch = false;
                if (qaidto.QAIDate.HasValue && ((CommonBLL.GetCurrentDateAndTime() - qaidto.QAIDate.Value).Days > _qAIExpiryDays))
                    IsExpiredBatch = true;
                List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
                PrmList.Add(new FloorSqlParameter("@QAIDETAILS", qaidetails));
                PrmList.Add(new FloorSqlParameter("@WorkStationId", WorkStationId));
                PrmList.Add(new FloorSqlParameter("@IsChangeQCType", IsCreateRework)); //#AZRUL 31/07/2018: Rework Order default value is null
                qaiId = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_SAV_QAIData", PrmList));
                if (qaidto.ScreenName == Constants.QAIScreens.ScanQITestResult)
                {
                    CommonBLL.InsertBatchAuditLog(Convert.ToDecimal(qaidto.SerialNo), qaiId, "QI", "Scan QI Test Result");
                }
                if (qaiId > 0)
                {
                    Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "after USP_SAV_QAIData", DateTime.Now));
                    if (qaidto.ScreenName != Constants.QAIScreens.EditDefects)
                    {
                        //#AZRUL 16/11/2021:  Mantis#0008464: Enable RESAMPLE to post HBC from QAI Scan Inner Box
                        if (qaidto.QCType != Qctype || qaidto.ScreenName == Constants.QAIScreens.QAIScanInnerTenPcs) //Qctype = RESAMPLE
                        {

                            //#AZRUL 23/08/2018:  Accumulate WT sample quantity after QCQI more than 1 times START
                            if (AXPostingBLL.GetPostingStage(Convert.ToDecimal(qaidto.SerialNo)) == Constants.QC_QI
                                && !qaidto.IsReSampling)  //#AZ 31/05/2019 Allowed resample when posting stage = QCQI
                            {
                                if (QAIBLL.CheckIsPostWT(qaidto.QCType))
                                {
                                    QAIBLL.SaveWTSamplingQCQI(qaidto.SerialNo, Convert.ToDecimal(qaidto.WTSamplingSize));
                                }
                            }
                            //#AZRUL 23/08/2018:  Accumulate WT sample quantity after QCQI more than 1 times END

                            if (qaidto.ScreenName == Constants.QAIScreens.ScanQITestResult)
                            {
                                isPostingSuccess = PostAXData(Convert.ToDecimal(qaidto.SerialNo), qaiId, false, "", true, false, true, qaidto.SuggestedQCType);
                            }
                            else
                            {
                                Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "before PostAXData", DateTime.Now));
                                // comment by #MH on 27th October 2016
                                // As resampling also need to post to AX
                                //if (QCTypeBasedPosting(Convert.ToDecimal(qaidto.SerialNo), qaiId))
                                //{                                
                                //#MH 22/12/2016 if have record before set isReSampling flag
                                if (!QCTypeBasedPosting(Convert.ToDecimal(qaidto.SerialNo), qaiId))
                                    qaidto.IsReSampling = true;

                                isPostingSuccess = PostAXData(Convert.ToDecimal(qaidto.SerialNo), qaiId, qaidto.IsReSampling, "", IsCreateRework, IsExpiredBatch); //#MH 22/12/2016 add isReSampling flag
                                Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "after PostAXData", DateTime.Now));

                            }

                        }
                    }
                }
            }
            catch (FloorSystemException fex)
            {
                DeleteQAIData(qaiId, qaidto.ScreenName.Value);
                throw fex;
            }
            catch (Exception ex)
            {
                DeleteQAIData(qaiId, qaidto.ScreenName.Value);
                throw new FloorSystemException(Messages.APPLICATIONERROR, Constants.AXSERVICEERROR, ex);
            }
            //finally
            //{
            //    CommitQAIAXPosting(qaiId);
            //}
            return qaiId;
        }


        /// <summary>
        /// #AZRUL 11/01/2019: Increase SOBC posting count for handle multiple SOBC.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>void</returns>
        public static void IncreaseSOBCCount(string serialNumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            FloorDBAccess.ExecuteScalar("USP_SAV_IncreaseSOBCCount", lstParameters);
        }

        /// <summary>
        /// SaveReworkOrder to staging
        /// <param name="_batchDto"></param>
        /// <paramref name="validateSP"> 
        ///   27/12/2018, Max He, if PTQI not require check QC type
        /// </paramref>
        /// <returns></returns>
        /// </summary>
        public static bool SaveReworkOrderData(BatchDTO _batchDto, bool validateSP = true)
        {
            List<DropdownDTO> qctypelst = validateSP ? CommonBLL.GetQCType() : new List<DropdownDTO>(); // 27/12/2018, Max He, by pass SP checking
            string STRAIGHT_PACK = (from qc in qctypelst
                                    where qc.IDField.Trim().ToLower() == Constants.STRAIGHT_PACK.ToString().Trim().ToLower()
                                    select qc.DisplayField).FirstOrDefault();

            bool isPostingSuccess = true;

            if (!SurgicalGloveBLL.IsOnlineSurgicalGlove(Convert.ToDecimal(_batchDto.SerialNumber)) && //Azrul 20201030 Rework logics for surgical
                CommonBLL.ReworkCreationForPTPF(_batchDto)) //Azrul 20220411 PTPF Glove Batch required SPBC except for Water Tight Batch
            {
                if (!validateSP || _batchDto.QCType != STRAIGHT_PACK) //27/12/2018, Max He,not check QC type when validateSP = false
                {
                    _batchdto = CommonBLL.GetCompleteBatchDetails(Convert.ToDecimal(_batchDto.SerialNumber));
                    _batchdto.TotalPcs = FinalPackingBLL.GetBatchCapacity(Convert.ToDecimal(_batchDto.SerialNumber)); //#AZRUL 24-6-2019: Calculate RWK qty based on Final Packing SP.
                    string LastServiceName = GetLastServiceName(Convert.ToDecimal(_batchDto.SerialNumber));

                    //#AZRUL 01-03-2019: Default Pool and RouteCategorry for PTQI.
                    if (AXPostingBLL.GetPostingStage(Convert.ToDecimal(_batchDto.SerialNumber)) == Constants.PT_QI)
                    {
                        _batchdto.Pool = Constants.PT;
                        _batchdto.RouteCategory = Constants.PT;
                    }
                    //#AZRUL 24-6-2019: If previous seq is CBCI, plantNo to follow CBCI plantNo.
                    if (LastServiceName == CreatePickingListFunctionidentifier.CBCI.ToString())
                    {
                        _batchdto.Location = GetCBCILocation(Convert.ToDecimal(_batchDto.SerialNumber));
                    }
                    //#AZRUL 2/7/2019: block if last prev seq is rework
                    if (LastServiceName == ReworkOrderFunctionidentifier.RWKCR.ToString())
                    {
                        isPostingSuccess = true;
                    }
                    //#AZRUL 17/7/2019: block if last rework is without SOBC (OQC to OQC)
                    else if (IsPrevReworkWithoutSOBC(_batchDto.SerialNumber))
                    {
                        isPostingSuccess = true;
                    }
                    else
                    {
                        isPostingSuccess = AXPostingBLL.PostAXDataReworkOrder(_batchdto);
                    }
                }
            }
            //Azrul 20201030 Rework logics for surgical
            else if (CommonBLL.ValidateAXPosting(Convert.ToDecimal(_batchDto.SerialNumber), CreateInvTransJournalFunctionidentifier.SPBC.ToString())  //Azrul 20201030 must have SPBC
                    || (_batchdto.BatchType.Trim() == Constants.PWT || _batchdto.BatchType.Trim() == Constants.QWT 
                    || _batchdto.BatchType.Trim() == Constants.OWT || _batchdto.BatchType.Trim() == Constants.PSW)) //Azrul 20220726 HSB also use Watertight batch for Surgical.
            {
                _batchdto = CommonBLL.GetCompleteBatchDetails(Convert.ToDecimal(_batchDto.SerialNumber));
                _batchdto.TotalPcs = FinalPackingBLL.GetBatchCapacity(Convert.ToDecimal(_batchDto.SerialNumber)); //#AZRUL 24-6-2019: Calculate RWK qty based on Final Packing SP.
                string LastServiceName = GetLastServiceName(Convert.ToDecimal(_batchDto.SerialNumber));

                //#AZRUL 01-03-2019: Default Pool and RouteCategorry for PTQI.
                if (AXPostingBLL.GetPostingStage(Convert.ToDecimal(_batchDto.SerialNumber)) == Constants.PT_QI)
                {
                    _batchdto.Pool = Constants.PT;
                    _batchdto.RouteCategory = Constants.PT;
                }
                //#AZRUL 2/7/2019: block if last prev seq is rework
                if (LastServiceName == ReworkOrderFunctionidentifier.RWKCR.ToString())
                {
                    isPostingSuccess = true;
                }
                //#AZRUL 17/7/2019: block if last rework is without SOBC (OQC to OQC)
                else if (IsPrevReworkWithoutSOBC(_batchDto.SerialNumber))
                {
                    isPostingSuccess = true;
                }
                else
                {
                    isPostingSuccess = AXPostingBLL.PostAXDataReworkOrder(_batchdto);
                }
            }
            return isPostingSuccess;
        }

        public static int QAIChangeQCTypeSave(string serialNumber, string qAIInspectorId, string changedQCType, string changeQCTypeReason, string workstationId, bool isChangeQCType = false, int authorizedBy = 0)
        {
            int rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            PrmList.Add(new FloorSqlParameter("@QAIInspectorId", qAIInspectorId));
            PrmList.Add(new FloorSqlParameter("@ChangedQCType", changedQCType));
            PrmList.Add(new FloorSqlParameter("@ChangeQCTypeReason", changeQCTypeReason));
            PrmList.Add(new FloorSqlParameter("@workstationId", workstationId));
            PrmList.Add(new FloorSqlParameter("@IsChangeQCType", isChangeQCType));
            PrmList.Add(new FloorSqlParameter("@AuthorizedBy", authorizedBy));
            rowsaffected = FloorDBAccess.ExecuteNonQuery("usp_QAIChangeQCType_Save", PrmList);
            return rowsaffected;
        }

        /// <summary>
        /// #AZRUL 18/07/2018: Rework order in change QC type, QAI Scan Inner box & QAI Resampling
        /// </summary>
        /// <param name="">serialNumber</param>
        /// <returns>boolean</returns>
        public static bool IsChangeQCType(string serialNumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_IsChangeQCType", lstParameters));
        }

        public static int QAIEditBatchCardInfoSave(string serialNumber, string qAIInspectorId, int innerBox, int packingSize, string workstationId, string screenNameScanInnerTenPcs, string screenNameEditOnlineBatchCard)
        {
            int rowsaffected;

            // var qaiBatch = CommonBLL.GetBatchNumberBySerialNumber(Convert.ToDecimal(serialNumber));
            var qaiBatch = CommonBLL.GetBatchNumberBySerialNumberEditOnlineBatchCard(Convert.ToDecimal(serialNumber), screenNameScanInnerTenPcs, screenNameEditOnlineBatchCard);
            //audit log
            var oldItem = new QAIEditOnlineBatchCardInfoDTO()
            {
                QAI_InnerBox = qaiBatch.InnerBoxes,
                QAI_PackingSize = qaiBatch.PackingSize == "" ? 0 : Convert.ToInt32(qaiBatch.PackingSize),
                Batch_TotalPCs = qaiBatch.TotalPCs,
                Batch_BatchWeight = qaiBatch.BatchWeight_NoRounding
            };
            var newItem = new QAIEditOnlineBatchCardInfoDTO()
            {
                QAI_InnerBox = innerBox,
                QAI_PackingSize = packingSize,
                Batch_TotalPCs = innerBox * packingSize, // calculate Total PCs
                Batch_BatchWeight = Math.Round((innerBox * packingSize * qaiBatch.TenPcsWeight_NoRounding) / (10 * 1000), 3, MidpointRounding.AwayFromZero) //calculate Batch Weight
            };
            var auditLog = new AuditLogDTO();
            auditLog.WorkstationId = workstationId;
            auditLog.FunctionName = Constants.EDIT_ONLINE_BATCH_CARD_INFO;
            auditLog.CreatedBy = qAIInspectorId;
            Constants.ActionLog audAction = Constants.ActionLog.Update;
            auditLog.AuditAction = Convert.ToInt32(audAction);
            auditLog.SourceTable = "Batch";
            auditLog.ReferenceId = serialNumber;
            auditLog.UpdateColumns = oldItem.DetailedCompare(newItem).GetPropChanges();
            string auditlog = CommonBLL.SerializeTOXML(auditLog);

            // return 0 if no changes
            if (auditLog.UpdateColumns == null || auditLog.UpdateColumns.Count == 0) return 0;

            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            PrmList.Add(new FloorSqlParameter("@QAIInspectorId", qAIInspectorId));
            PrmList.Add(new FloorSqlParameter("@innerBox", innerBox));
            PrmList.Add(new FloorSqlParameter("@packingSize", packingSize));
            PrmList.Add(new FloorSqlParameter("@workstationId", workstationId));
            PrmList.Add(new FloorSqlParameter("@TotalPCs", newItem.Batch_TotalPCs));
            PrmList.Add(new FloorSqlParameter("@BatchWeight", newItem.Batch_BatchWeight));
            PrmList.Add(new FloorSqlParameter("@auditlog", auditlog));
            rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_QAIEditOnlineBatchCardInfo_Save", PrmList);
            return rowsaffected;
        }

        /// <summary>
        /// Delete when there is an Error in AX Posting
        /// </summary>
        /// <param name="qaidto"></param>
        /// <returns></returns>
        public static int DeleteQAIData(int qaidid, Constants.QAIScreens screenname)
        {
            int rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@QAIID", qaidid));
            PrmList.Add(new FloorSqlParameter("@Screenname", screenname.ToString()));
            PrmList.Add(new FloorSqlParameter("@LogDeleteHours", Convert.ToInt32(FloorSystemConfiguration.GetInstance().IntLogDeleteHours)));
            rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_DEL_QAIData", PrmList);
            return rowsaffected;
        }

        /// <summary>
        /// To Change the AXposting Commit Flag to true.
        /// </summary>
        /// <param name="qaidid"></param>
        /// <returns></returns>
        public static int CommitQAIAXPosting(int qaidid)
        {
            int rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@QAIID", qaidid));
            rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Commit_QAIAX_Posting", PrmList);
            return rowsaffected;
        }

        /// <summary>
        /// Calculate QAI Result
        /// </summary>
        /// <param name="qaidto"></param>
        /// <returns></returns>
        public static string CalculateQAIResult(QAIDTO qaidto)
        {
            string QAIResult = string.Empty;


            return QAIResult;
        }
        /// <summary>
        /// Calculate Suggested QCType
        /// </summary>
        /// <param name="qaidto"></param>
        /// <returns>DropdownDTO</returns>
        public static DropdownDTO CombineQcTypes(string QCTypeFirst, string QCTypeSecond)
        {
            DropdownDTO dropdowndto = null;
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@QCTypeFirst", QCTypeFirst));
            prmList.Add(new FloorSqlParameter("@QCTypeSecond", QCTypeSecond));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_QAI_CombineQcType", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dropdowndto = new DropdownDTO() { IDField = Convert.ToString(dr["QCTYPE"]), DisplayField = Convert.ToString(dr["DESCRIPTION"]) };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETCALCULATESUGGESTEDQCTYPEEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return dropdowndto;
        }

        /// <summary>
        /// Check CBCI Location to update Rework Location
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        private static string GetCBCILocation(decimal SerialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", SerialNumber.ToString()));
            return FloorDBAccess.ExecuteScalar("USP_GET_CBCILocation", PrmList).ToString();
        }

        /// <summary>
        /// Get last Sequence of Service Name
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static string GetLastServiceName(decimal SerialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", SerialNumber));
            return FloorDBAccess.ExecuteScalar("USP_GET_LastServiceName", PrmList).ToString();
        }

        /// <summary>
        /// To verify previous posting is Other than RESAMPLE
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="qaiId"></param>
        /// <returns></returns>
        private static bool QCTypeBasedPosting(decimal serialNumber, int qaiId)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            PrmList.Add(new FloorSqlParameter("@QaiId", qaiId));
            var isAxposting = FloorDBAccess.ExecuteScalar("USP_SEL_ISAXPost", PrmList);
            return Convert.ToBoolean(isAxposting);
        }

        /// <summary>
        /// Calculate Suggested QCType **** AQL Values is assigned to SelectedValue Property..*****
        /// </summary>
        /// <param name="qaidto"></param>
        /// <returns>DropdownDTO</returns>
        public static DropdownDTO CalculateSuggestedQCType(QAIDTO qaidto)
        {
            if (!QAIBLL.HasDefects(qaidto.Defects))
            {
                qaidto.IsStraingPack = true;
            }
            string qaidetails = CommonBLL.SerializeTOXML(qaidto);

            DropdownDTO dropdowndto = new DropdownDTO();
            if (qaidto.IsStraingPack)
            {
                List<DropdownDTO> qctypelst = CommonBLL.GetQCType();
                DropdownDTO dropdto = (from qc in qctypelst
                                       where qc.IDField.Trim().ToLower() == Convert.ToString(Hartalega.FloorSystem.Framework.Constants.STRAIGHT_PACK).ToLower()
                                       select qc).FirstOrDefault();
                dropdowndto.DisplayField = dropdto.IDField;
                dropdowndto.IDField = dropdto.DisplayField;
                dropdowndto.SelectedValue = Constants.QAI_DefaultAQL;
                return dropdowndto;
            }
            List<FloorSqlParameter> prmList = new List<FloorSqlParameter>();
            prmList.Add(new FloorSqlParameter("@QAIDETAILS", qaidetails));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_QAI_QcType_Suggested", prmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            //AQL Values is assigned to SelectedValue Property.
                            dropdowndto = new DropdownDTO() { IDField = Convert.ToString(dr["QCTYPE"]), DisplayField = Convert.ToString(dr["DESCRIPTION"]), SelectedValue = Convert.ToString(dr["AQLValue"]) };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETCALCULATESUGGESTEDQCTYPEEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return dropdowndto;
        }

        /// <summary>
        /// Pin Hole QCtype
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> PinHoleQCtype()
        {
            List<DropdownDTO> pinholeqctype = new List<DropdownDTO>();
            pinholeqctype.Add(new DropdownDTO() { IDField = Constants.QMAX, DisplayField = Constants.QMAX });
            pinholeqctype.Add(new DropdownDTO() { IDField = Constants.QMAX_F, DisplayField = Constants.QMAX_F });
            pinholeqctype.Add(new DropdownDTO() { IDField = Constants.QMAX_MP, DisplayField = Constants.QMAX_MP });
            return pinholeqctype;
        }

        /// <summary>
        /// To get Pin hole defect or Not
        /// </summary>
        /// <param name="Defects"></param>
        /// <returns></returns>
        public static bool IsPinHoleDefect(List<QAIDefectType> Defects)
        {
            bool IsPinhole = false;
            foreach (QAIDefectType dt in Defects)
            {
                foreach (QAIDefectDTO dlst in dt.DefectList)
                {
                    if (dlst.Count > 0 && dlst.DefectItem.ToLower().Contains(Constants.PINHOLE.ToLower()))
                    {
                        return true;
                    }
                }
            }
            return IsPinhole;
        }

        /// <summary>
        /// QAI Sampling Sizes
        /// </summary>
        /// <param name="SamplingType"></param>
        /// <param name="Screenname"></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetSamplingSize(string SamplingType, Hartalega.FloorSystem.Framework.Constants.QAIScreens Screenname)
        {
            List<DropdownDTO> samplingSize = null;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SamplingType", SamplingType));
            PrmList.Add(new FloorSqlParameter("@ScreenName", Screenname.ToString()));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_QAI_GETSamplingSizes", PrmList))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        // #Azrul 13/07/2018: Merged from Live AX6 Start
                        if (SamplingType == Constants.VISUAL_TEST_SAMPLING_SIZE) // #Azman 01/03/2018 - Get value from both column
                        {
                            samplingSize = (from DataRow dr in dt.Rows
                                            select new DropdownDTO
                                            {
                                                IDField = FloorDBAccess.GetString(dr, "Value"),
                                                DisplayField = FloorDBAccess.GetString(dr, "Size")
                                            }).ToList();
                        }
                        else
                        {
                            // #Azrul 13/07/2018: Merged from Live AX6 End
                            samplingSize = (from DataRow dr in dt.Rows
                                            select new DropdownDTO
                                            {
                                                IDField = FloorDBAccess.GetString(dr, "Size"),
                                                DisplayField = FloorDBAccess.GetString(dr, "Size")
                                            }).ToList();
                        }
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
            return samplingSize;
        }

        /// <summary>
        /// Get QCtype Value from List
        /// </summary>
        /// <param name="qctypelst"></param>
        /// <param name="qctypeid"></param>
        /// <returns></returns>
        public static string GetQCtypeValue(List<DropdownDTO> qctypelst, string qctypeid)
        {
            string qctypeValue = string.Empty;
            List<DropdownDTO> qctypelst1 = CommonBLL.GetQCType();
            qctypeValue = (from qc in qctypelst1
                           where qc.DisplayField.ToLower().Trim() == qctypeid.ToLower().Trim()
                           select qc.IDField).FirstOrDefault();
            return qctypeValue;
        }

        /// <summary>
        /// Get QAI Posting Stage
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>string</returns>
        public static string GetQAIPostingStage(decimal serialNumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_QAIPostingStage", lstParameters));
        }

        /// <summary>
        /// Get Last QAI Result
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>string</returns>
        public static string GetLastQIResult(decimal serialNumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_LastQITestResult", lstParameters));
        }

        /// <summary>
        /// QAI User Permission by Screen Name
        /// </summary>
        /// <param name="operatorId"></param>
        /// <param name="Screenid"></param>
        /// <returns></returns>
        public static bool GetOperatorNameQAI(string operatorId, int Screenid)
        {
            bool IsvalidRole = false;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@QAIUserID", operatorId));
            PrmList.Add(new FloorSqlParameter("@ScreenID", Screenid));
            IsvalidRole = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_QAIUser_Permission", PrmList));
            return IsvalidRole;
        }


        /// <summary>
        /// Get QCtype Description
        /// </summary>
        /// <param name="qctypeid"></param>
        /// <returns></returns>
        public static string GetQCtypeDescription(string qctypeid)
        {
            string qctypeValue = string.Empty;
            List<DropdownDTO> qctypelst = CommonBLL.GetQCTypeALL();
            qctypeValue = (from qc in qctypelst
                           where qc.DisplayField.ToLower().Trim() == qctypeid.ToLower().Trim()
                           select qc.IDField).FirstOrDefault();
            return qctypeValue;
        }

        /// <summary>
        /// Get HBC Packing Size from Serial Number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static string GetHBCPackSizeFromSerialNo(Int64 serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return FloorDBAccess.ExecuteScalar("USP_GET_HBCPackSizeFromSerialNo", PrmList).ToString();
        }

        /// <summary>
        /// Get HBC Inner Box from Serial Number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static string GetHBCInBoxFromSerialNo(Int64 serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return FloorDBAccess.ExecuteScalar("USP_GET_HBCInBoxFromSerialNo", PrmList).ToString();
        }

        /// <summary>
        /// Get HBC Total Pieces from Serial Number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static Int32 GetHBCTotalPcsFromSerialNo(Decimal serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNo", serialNumber));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_HBCTotalPcs_Get", PrmList));
        }

        /// <summary>
        /// Get PT Status For PWT batch
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static bool GetPTStatusForPWT(Int64 serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_PTStatusForPWT", PrmList));
        }

        /// <summary>
        /// Get latest rework qty to do calculate rejected sample when qcqi
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static int GetLatestReworkQty(string serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_GetLatestReworkQty", PrmList));
        }

        /// <summary>
        /// To get if there are any defects
        /// </summary>
        /// <param name="Defects"></param>
        /// <returns></returns>
        public static bool HasDefects(List<QAIDefectType> Defects)
        {
            foreach (QAIDefectType dt in Defects)
            {
                foreach (QAIDefectDTO dlst in dt.DefectList)
                {
                    if (dlst.Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Validate Reason and Return Error Message
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="qiTestReason"></param>
        /// <param name="qiTestResult"></param>
        /// <returns></returns>
        public static string ValidateQITestReason(decimal serialNumber, string qiTestReason, string qiTestResult = "")
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            PrmList.Add(new FloorSqlParameter("@QITestReason", qiTestReason));
            PrmList.Add(new FloorSqlParameter("@QITestResult", qiTestResult));
            return FloorDBAccess.ExecuteScalar("USP_VAL_QITestReason", PrmList).ToString();
        }
        #endregion

        #region AX Posting

        public static bool PostAXData(decimal serialNo, int qaiId = 0, bool IsReSampling = false, string PTQITestResult = "", bool? IsCreateRework = true, bool IsExpiredBatch = false, bool IsFromQAQI = false, string wt_SuggestedQCType = "")
        {
            _isPostingSuccess = false;
            _message = string.Empty;
            Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "strat PostAXData", DateTime.Now));
            _batchdto = CommonBLL.GetCompleteBatchDetails(serialNo);
            _batchdto.QaiId = qaiId; // #MH 3/11/2016 1.n search by QAI id if is qaiId avaiable 
            _batchdto.IsReSampling = IsReSampling; // #MH 22/12/2016 2.n detect is resampling
            _subModuleId = AXPostingBLL.GetSubModuleIdForSerialNo(serialNo);

            string postingStage = AXPostingBLL.GetPostingStage(serialNo);
            string batchType = PostTreatmentBLL.GetBatchType(serialNo);
            Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "after GetBatchType", DateTime.Now));

            //amir - fix route category for water tight 
            if (IsFromQAQI && (_batchdto.BatchType.Trim() == Constants.PWT    //PWTBCP
                  || _batchdto.BatchType.Trim() == Constants.QWT //PWTBCA
                  || _batchdto.BatchType.Trim() == Constants.OWT //PWTBCQ
                  || _batchdto.BatchType.Trim() == Constants.PSW)) //PWTBCS
            {
                _batchdto.QCType = wt_SuggestedQCType;
                _wt_SuggestedQCType = wt_SuggestedQCType;
            }


            if (_batchdto.Module == Convert.ToString(Convert.ToInt16(Constants.Modules.TUMBLING)) || _batchdto.Module == Convert.ToString(Convert.ToInt16(Constants.Modules.QCSCANNINGSYSTEM)))
            {
                if (postingStage == Constants.PT_QI)
                {
                    //First check on Batch Type, all source from water tight(WT) test must create rework order
                    if (batchType == Constants.PWT    //PWTBCP
                        || batchType == Constants.QWT //PWTBCA
                        || batchType == Constants.OWT //PWTBCQ
                        || batchType == Constants.PSW) //PWTBCS
                    {
                        if (AXPostingBLL.GetQITestResult(qaiId) == Constants.PASS) //#Max 28-12-2018 BUGS 1219: RWKCR only generates when PTQI = Pass
                        {
                            //Scan QI Test Result need to validate the IsPostedToAX column in AxPostingLog, if status = 1 then no need to insert to staging
                            if (!CommonBLL.ValidateAXPosting(serialNo, CreateRAFJournalFunctionidentifier.PWTBCP.ToString())
                            && !CommonBLL.ValidateAXPosting(serialNo, CreateRAFJournalFunctionidentifier.PWTBCA.ToString())
                            && !CommonBLL.ValidateAXPosting(serialNo, CreateRAFJournalFunctionidentifier.PWTBCQ.ToString())
                            && !CommonBLL.ValidateAXPosting(serialNo, CreateRAFJournalFunctionidentifier.PWTBCS.ToString())
                            )
                            {
                                _batchdto = AXPostingBLL.GetCompletePTDetails(serialNo);
                                _batchdto.QaiId = qaiId; // #MH 3/11/2016 1.n search by QAI id if is qaiId avaiable 
                                _isPostingSuccess = AXPostingBLL.PostAXDataPrintWaterTightBatchCard(_batchdto);
                                _isPostingSuccess = SaveReworkOrderData(_batchdto, false);
                                _message = Constants.PT_QI;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(PostTreatmentBLL.GetBatchStatus(serialNo)) || !string.IsNullOrEmpty(PostTreatmentBLL.GetQAIBatchStatus(serialNo)))
                        {
                            if (!CommonBLL.ValidateAXPosting(serialNo, CreateInvTransJournalFunctionidentifier.SPBC.ToString()))
                            {
                                _isPostingSuccess = AXPostingBLL.PostAXDataScanPTBatchCard(serialNo);
                                if (AXPostingBLL.GetQITestResult(qaiId) == Constants.PASS) //#AZRUL 29-10-2018 BUGS 1219: RWKCR only generates when PTQI = Pass
                                    _isPostingSuccess = SaveReworkOrderData(_batchdto);
                                _message = Constants.PT_QI;
                            }
                        }
                        else
                        {
                            if (AXPostingBLL.GetQITestResult(qaiId).ToUpper() == Constants.FAIL) //#AZRUL 13/4/2022: RWKCR only generates when PTQI = Fail
                            {
                                //#AZRUL 18-6-2019: Validate previous SPBC PlantNo, create another transfer if PlantNo different with the workstation PlantNo.
                                //#AZRUL 10-10-2019: Removed and KIV, causing duplicate SPBC ex: SPBC P1, SPBC P2, RWKCR P1.
                                //string plantNo = WorkStationDTO.GetInstance().Location;
                                //if (GetPrevSPBCPlantNo(serialNo.ToString()) != plantNo)
                                //{
                                //    AXPostingBLL.PostAXDataScanPTBatchCard(serialNo, plantNo);
                                //}
                                _isPostingSuccess = SaveReworkOrderData(_batchdto);
                            }
                        }
                    }
                }
                else if (postingStage == Constants.QC_QI)
                {
                    PostingQCQI(serialNo, AXPostingBLL.GetQITestResult(qaiId), _batchdto, IsExpiredBatch);
                }
                else //if batch type not water tight 
                {
                    if (!CommonBLL.ValidateAXPosting(serialNo, CreateRAFJournalFunctionidentifier.PVTBCA.ToString())
                        || !CommonBLL.ValidateAXPosting(serialNo, CreateRAFJournalFunctionidentifier.PVTBCS.ToString())
                        || !CommonBLL.ValidateAXPosting(serialNo, CreateInvMovJournalFunctionidentifier.PLBC.ToString())
                        || !CommonBLL.ValidateAXPosting(serialNo, CreateRAFJournalFunctionidentifier.PNBC.ToString())) // #AZRUL 25/5/2022: Prevent multiple PNBC posting.
                    {
                        _isPostingSuccess = AXPostingBLL.PostAXDataTumbling(serialNo);
                        if (QAIBLL.GetRouteCategory(_batchdto.QCType) != Constants.PT)  //#AZRUL 18/07/2018: Rework order only create after PTQI if route category is PT.
                            _isPostingSuccess = SaveReworkOrderData(_batchdto);
                        _message = Constants.QC_QI;
                    }
                }
            }
            if (_batchdto.Module == Convert.ToString(Convert.ToInt16(Constants.Modules.HOURLYBATCHCARD)))
            {
                if (_batchdto.IsOnline == false)
                {
                    #region Currently not used
                    //if (postingStage == Constants.PT_QI)
                    //{
                    //    if (!string.IsNullOrEmpty(PostTreatmentBLL.GetBatchStatus(serialNo)) || !string.IsNullOrEmpty(PostTreatmentBLL.GetQAIBatchStatus(serialNo)))
                    //    {
                    //        if (!CommonBLL.ValidateAXPosting(serialNo, CreateInvTransJournalFunctionidentifier.SPBC.ToString()))
                    //        {
                    //            _isPostingSuccess = AXPostingBLL.PostAXDataScanPTBatchCard(serialNo);
                    //            _message = Constants.PT_QI;
                    //        }
                    //    }
                    //}
                    //else if (postingStage == Constants.QC_QI)
                    //{
                    //    PostingQCQI(serialNo, AXPostingBLL.GetQITestResult(qaiId)); // #AZ 22/06/2018 no posting for failed QC
                    //}
                    //else //if batch type not water tight
                    //{
                    //    _isPostingSuccess = AXPostingBLL.PostAXDataTumbling(serialNo);
                    //    _isPostingSuccess = SaveReworkOrderData(_batchdto);
                    //    _message = Constants.QC_QI;
                    //}
                    #endregion
                }
                else
                {
                    if (postingStage == Constants.QC_QI
                        && !_batchdto.IsReSampling) //#AZ 31/05/2019 Allowed resample when posting stage = QCQI
                    {
                        Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "start QC_QI", DateTime.Now));

                        PostingQCQI(serialNo, AXPostingBLL.GetQITestResult(qaiId), _batchdto, IsExpiredBatch); // #AZ 22/06/2018 no posting for failed QC
                    }
                    else
                    {
                        if (postingStage == Constants.PT_QI)
                        {
                            Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "start PT_QI", DateTime.Now));
                            if (!string.IsNullOrEmpty(PostTreatmentBLL.GetBatchStatus(serialNo)) || !string.IsNullOrEmpty(PostTreatmentBLL.GetQAIBatchStatus(serialNo)))
                            {
                                if (!CommonBLL.ValidateAXPosting(serialNo, CreateInvTransJournalFunctionidentifier.SPBC.ToString()))
                                {
                                    _isPostingSuccess = AXPostingBLL.PostAXDataScanPTBatchCard(serialNo);
                                    if (AXPostingBLL.GetQITestResult(qaiId) == Constants.PASS) //#AZRUL 29-10-2018 BUGS 1219: RWKCR only generates when PTQI = Pass
                                        _isPostingSuccess = SaveReworkOrderData(_batchdto);
                                    _message = Constants.PT_QI;
                                }
                                else
                                {
                                    if (AXPostingBLL.GetQITestResult(qaiId) == Constants.PASS) //#AZRUL 29-10-2018 BUGS 1219: RWKCR only generates when PTQI = Pass
                                    {
                                        //#AZRUL 18-6-2019: Validate previous SPBC PlantNo, create another transfer if PlantNo different with the workstation PlantNo.
                                        //#AZRUL 10-10-2019: Removed and KIV, causing duplicate SPBC ex: SPBC P1, SPBC P2, RWKCR P1.
                                        //string plantNo = WorkStationDTO.GetInstance().Location;
                                        //if (GetPrevSPBCPlantNo(serialNo.ToString()) != plantNo)
                                        //{
                                        //    AXPostingBLL.PostAXDataScanPTBatchCard(serialNo, plantNo);
                                        //}
                                        _isPostingSuccess = SaveReworkOrderData(_batchdto);
                                    }
                                    _message = Constants.PT_QI;
                                }
                            }
                            else
                            {
                                /**2020-01-05 - HSB SIT2 Issue# 23: During PTQI Pass at floor system not prompting for "Do you want to create Rework Order" message but after PTQI pass at web admin RWKCR already created.
                                   During PTQI Fail, there is a prompt message in the Floor system for "Do you want to create Rework Order" message but no RWKCR created in we admin. **/
                                if (AXPostingBLL.GetQITestResult(qaiId).ToUpper() == Constants.FAIL) //2022-01-05: RWKCR generates when Fail PTQI with PT
                                    _isPostingSuccess = SaveReworkOrderData(_batchdto);
                                _message = Constants.PT_QI;
                            }
                        }
                        else
                        {
                            if (IsFromQAQI == false)
                            {
                                Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "start GetResourceBySerialNo", DateTime.Now));

                                DataSet objData = CommonBLL.GetResourceBySerialNo(serialNo); //#AZ 26/05/2018 1.n loop for insert details into staging for mutiple resource/batch order no
                                Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "after GetResourceBySerialNo", DateTime.Now));

                                List<DropdownDTO> qctypelst = CommonBLL.GetQCType();
                                string Resample = (from qc in qctypelst
                                                 where qc.IDField.Trim().ToLower() == Constants.RESAMPLE.ToString().Trim().ToLower()
                                                 select qc.DisplayField).FirstOrDefault();

                                foreach (DataTable table in objData.Tables)
                                {
                                    foreach (DataRow dr in table.Rows)
                                    {
                                        string resource = dr["Resource"].ToString(); //#AZ 26/05/2018 1.n loop for insert details into staging for mutiple resource/batch order no
                                        _batchdto = CommonBLL.GetCompleteBatchDetailsByResource(serialNo, resource);
                                        _batchdto.QaiId = qaiId; // #MH 3/11/2016 1.n search by QAI id if is qaiId avaiable 
                                        _batchdto.IsReSampling = IsReSampling; // #MH 22/12/2016 2.n detect is resampling

                                        _isPostingSuccess = AXPostingBLL.PostAXDataHourly(_batchdto);
                                        if (table.Rows.IndexOf(dr) == table.Rows.Count - 1)     //#AZRUL 8/11/2018 BUG 1231: Only post rework at last seq for 2 or 4 tier
                                        {
                                            //#AZRUL 16/11/2021:  Mantis#0008464: RWKCR will be post if select non Straight Pack in QAI Re-Sampling Scan
                                            if (QAIBLL.GetRouteCategory(_batchdto.QCType) != Constants.PT && (!IsCreateRework.HasValue || IsCreateRework.GetValueOrDefault())
                                                && _batchdto.QCType != Resample)
                                            {
                                                _isPostingSuccess = SaveReworkOrderData(_batchdto);
                                            }
                                        }
                                        _message = Constants.QC_QI;
                                    }
                                }
                                Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "after loop ResourceBySerialNo", DateTime.Now));
                            }
                        }
                    }
                }
            }
            if (_batchdto.Module == Convert.ToString(Convert.ToInt16(Constants.Modules.SURGICALGLOVESYSTEM)))
            {
                if (_batchdto.IsOnline == false)
                {
                    #region Currently not used
                    //if (postingStage == Constants.PT_QI)
                    //{
                    //    if (!string.IsNullOrEmpty(PostTreatmentBLL.GetBatchStatus(serialNo)) || !string.IsNullOrEmpty(PostTreatmentBLL.GetQAIBatchStatus(serialNo)))
                    //    {
                    //        if (!CommonBLL.ValidateAXPosting(serialNo, CreateInvTransJournalFunctionidentifier.SPBC.ToString()))
                    //        {
                    //            _isPostingSuccess = AXPostingBLL.PostAXDataScanPTBatchCard(serialNo);
                    //            _message = Constants.PT_QI;
                    //        }
                    //    }
                    //}
                    //else if (postingStage == Constants.QC_QI)
                    //{
                    //    PostingQCQI(serialNo, AXPostingBLL.GetQITestResult(qaiId)); // #AZ 22/06/2018 no posting for failed QC
                    //}
                    //else //if batch type not water tight
                    //{
                    //    _isPostingSuccess = AXPostingBLL.PostAXDataTumbling(serialNo);
                    //    _isPostingSuccess = SaveReworkOrderData(_batchdto);
                    //    _message = Constants.QC_QI;
                    //}
                    #endregion
                }
                else
                {
                    if (postingStage == Constants.QC_QI && !_batchdto.IsReSampling) //#AZ 31/05/2019 Allowed resample when posting stage = QCQI
                    {
                        Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "start QC_QI", DateTime.Now));

                        PostingQCQI(serialNo, AXPostingBLL.GetQITestResult(qaiId), _batchdto, IsExpiredBatch); // #AZ 22/06/2018 no posting for failed QC
                    }
                    else
                    {
                        if (postingStage == Constants.PT_QI)
                        {
                            Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "start PT_QI", DateTime.Now));
                            if (!string.IsNullOrEmpty(PostTreatmentBLL.GetBatchStatus(serialNo)) || !string.IsNullOrEmpty(PostTreatmentBLL.GetQAIBatchStatus(serialNo)))
                            {
                                if (!CommonBLL.ValidateAXPosting(serialNo, CreateInvTransJournalFunctionidentifier.SPBC.ToString()))
                                {
                                    _isPostingSuccess = AXPostingBLL.PostAXDataScanPTBatchCard(serialNo);
                                    if (AXPostingBLL.GetQITestResult(qaiId) == Constants.PASS) //#AZRUL 29-10-2018 BUGS 1219: RWKCR only generates when PTQI = Pass
                                        _isPostingSuccess = SaveReworkOrderData(_batchdto);
                                    _message = Constants.PT_QI;
                                }
                                else
                                {
                                    if (AXPostingBLL.GetQITestResult(qaiId) == Constants.PASS) //#AZRUL 29-10-2018 BUGS 1219: RWKCR only generates when PTQI = Pass
                                    {
                                        _isPostingSuccess = SaveReworkOrderData(_batchdto);
                                    }
                                    _message = Constants.PT_QI;
                                }
                            }
                            else
                            {
                                if (AXPostingBLL.GetQITestResult(qaiId) == Constants.PASS) //#AZRUL 29-10-2018 BUGS 1219: RWKCR only generates when PTQI = Pass
                                    _isPostingSuccess = SaveReworkOrderData(_batchdto);
                                _message = Constants.PT_QI;
                            }
                        }
                        else
                        {
                            if (IsFromQAQI == false)
                            {
                                Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "start GetResourceBySerialNo", DateTime.Now));

                                DataSet objData = CommonBLL.GetResourceBySerialNo(serialNo); //#AZ 26/05/2018 1.n loop for insert details into staging for mutiple resource/batch order no
                                Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "after GetResourceBySerialNo", DateTime.Now));

                                foreach (DataTable table in objData.Tables)
                                {
                                    foreach (DataRow dr in table.Rows)
                                    {
                                        string resource = dr["Resource"].ToString(); //#AZ 26/05/2018 1.n loop for insert details into staging for mutiple resource/batch order no
                                        _batchdto = SurgicalGloveBLL.GetQAIDetails_SRBC(serialNo);
                                        _batchdto.QaiId = qaiId; // #MH 3/11/2016 1.n search by QAI id if is qaiId avaiable 
                                        _batchdto.IsReSampling = IsReSampling; // #MH 22/12/2016 2.n detect is resampling

                                        _isPostingSuccess = AXPostingBLL.PostAXSurgicalBatchCard(_batchdto);
                                        if (table.Rows.IndexOf(dr) == table.Rows.Count - 1)     //#AZRUL 8/11/2018 BUG 1231: Only post rework at last seq for 2 or 4 tier
                                        {
                                            if (QAIBLL.GetRouteCategory(_batchdto.QCType) != Constants.PT && (!IsCreateRework.HasValue || IsCreateRework.GetValueOrDefault()))
                                            {
                                                _isPostingSuccess = SaveReworkOrderData(_batchdto);
                                            }
                                        }
                                        _message = Constants.QC_QI;
                                    }
                                }
                                Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "after loop ResourceBySerialNo", DateTime.Now));
                            }
                        }
                    }
                }
            }

            if (_message == string.Empty)
            {
                return true;
            }
            else
            {
                return _isPostingSuccess;
            }
        }

        public static void PostingQCQI(decimal serialNo, string isQIPass, BatchDTO _batchdto, bool IsExpiredBatch)
        {
            string batchStatus = AXPostingBLL.GetBatchStatus(serialNo);
            if (batchStatus == Constants.COMPLETED)
            {
                if (CommonBLL.ValidateAXPosting(serialNo, CreateRAFJournalFunctionidentifier.SOBC) && IsExpiredBatch) //#AZRUL 13/02/2019: Expired batch but posting stage is SOBC, will create only rework. 
                {
                    SaveReworkOrderData(_batchdto);
                    return;
                }
                _batchdto = AXPostingBLL.GetCompleteQCYPDetails(serialNo);


                //amir - fix route category for water tight 
                if (_batchdto.BatchType.Trim() == Constants.PWT    //PWTBCP
                      || _batchdto.BatchType.Trim() == Constants.QWT //PWTBCA
                      || _batchdto.BatchType.Trim() == Constants.OWT //PWTBCQ
                      || _batchdto.BatchType.Trim() == Constants.PSW) //PWTBCS
                {
                    _batchdto.QCType = _wt_SuggestedQCType;
                }

                List<DropdownDTO> qctypelst = CommonBLL.GetQCType();
                string Qctype = (from qc in qctypelst
                                 where qc.IDField.Trim().ToLower() == Constants.STRAIGHT_PACK.ToString().Trim().ToLower()
                                 select qc.DisplayField).FirstOrDefault();

                //string[] identifiersQCQIArray = new string[] { CreateRAFJournalFunctionidentifier.QCQI_QC.ToString(),CreateRAFJournalFunctionidentifier.SBCIB.ToString(),
                                                          //CreateRAFJournalFunctionidentifier.SOBC.ToString() //#Max, 07/01/2019, will use special for checking SOBC positng
                //};
                //string identifiersQCQI = string.Join(",", identifiersQCQIArray);

                //#Max, 07/01/2019, if SOBC positng balance is 1 only can insert to staging
                int SOBCPostingBalance = CommonBLL.ValidateSOBCPosting(serialNo);

                //if (_batchdto.QCType != Qctype) // #AZRUL 16/8/2108 SP also should post to D365
                //{
                //if (SOBCPostingBalance == 1 && !CommonBLL.ValidateAXPosting(serialNo, identifiersQCQI))
                if (SOBCPostingBalance == 1)
                {
                    int subModuleIdQCScan = Convert.ToInt16(CommonBLL.GetSubModuleId(Constants.QC_SCAN_OUT_BATCHCARD));
                    int subModuleIdQCYP = Convert.ToInt16(CommonBLL.GetSubModuleId(Constants.QCYP_SCAN_OUT_BATCHCARD));
                    int subModuleIdQCSBCW = Convert.ToInt16(CommonBLL.GetSubModuleId(Constants.SCAN_BATCHCARD_WEIGHT));
                    int subModuleIdQCSBCP = Convert.ToInt16(CommonBLL.GetSubModuleId(Constants.SCAN_BATCHCARD_PIECES)); // #GL 04/11-2020 New screen - HTLG_P7CR_014&015 2nd Grade Surgical Glove Reporting

                    if (_subModuleId == subModuleIdQCYP || _subModuleId == subModuleIdQCSBCP) // #GL 04/11-2020 New screen
                    {
                        CommonBLL.SetQAIBatchDetails(_batchdto);// #MH 10/11/2016 1.n FDD:HLTG-REM-003
                        CalculateAXRejectedOrLooseQty(_batchdto);
                        if (isQIPass == Constants.PASS) //#AZ 22/06/2018 no posting for failed QC
                        {
                            _isPostingSuccess = AXPostingBLL.PostAXDataQCPackingYield(_batchdto);
                            if (_isPostingSuccess)
                            {
                                IncreaseSOBCCount(_batchdto.SerialNumber); //#AZ 11/01/2019 Increase SOBC posting count for handle multiple SOBC.

                            }
                            if (IsChangeQCType(_batchdto.SerialNumber) && QAIBLL.GetRouteCategory(_batchdto.QCType) == Constants.PT) //#AZRUL 18/07/2018: Rework order in change QC type, QAI Scan Inner box & QAI Resampling, default IS NULL will not create Rework Order.
                            {
                                SaveReworkOrderData(_batchdto);
                            }
                            _message = Constants.QC_QI;
                        }
                    }
                    else if (_subModuleId == subModuleIdQCScan)
                    {
                        CommonBLL.SetQAIBatchDetails(_batchdto);// #MH 10/11/2016 1.n FDD:HLTG-REM-003
                        CalculateAXRejectedOrLooseQty(_batchdto);
                        _isPostingSuccess = AXPostingBLL.PostAXDataQCScanningScanBatchCard(_batchdto);
                        _message = Constants.QC_QI;
                    }
                    else if (_subModuleId == subModuleIdQCSBCW)
                    {
                        BatchDTO batchdto = QCScanningBLL.GetBatchDetailsPosting(serialNo);
                        CommonBLL.SetQAIBatchDetails(batchdto);// #MH 10/11/2016 1.n FDD:HLTG-REM-003                             
                        CalculateAXRejectedOrLooseQty(batchdto);
                        _isPostingSuccess = AXPostingBLL.PostAXDataQCScanBatchCardWeight(_batchdto);
                        _message = Constants.QC_QI;
                    }
                }
                //}
            }
        }

        /// <summary>
        /// #MH 10/11/2016 1.n FDD:HLTG-REM-003
        /// Calculate rejected sample or loose qty when post to AX(QCQI)
        /// #MH 23/12/2016 2.n         
        /// Change calculate logic,move calcualtion from UI to BLL for handle scan in/out many times
        /// #MH 3/1/2016 3.n
        /// Fix rounding bugs
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public static BatchDTO CalculateAXRejectedOrLooseQty(BatchDTO input)
        {
            //var isCalculatedLooseQty = false;
            //if (FloorSystemConfiguration.GetInstance().IScalculatedLooseQty.HasValue && FloorSystemConfiguration.GetInstance().IScalculatedLooseQty.Value)
            //    isCalculatedLooseQty = FloorSystemConfiguration.GetInstance().IScalculatedLooseQty.Value;

            // MH3.n
            int BalancePcs = GetLatestReworkQty(input.SerialNumber);
            // MH3.n
            //#AZRUL 11/01/2019: Get TotalPcs from previous SOBC for multiple SOBC posting.
            //if (CommonBLL.ValidateAXPosting(Convert.ToDecimal(input.SerialNumber), CreateRAFJournalFunctionidentifier.SOBC))
            //{
            //    HBCBatchTotalPcs = GetPrevTotalPcsQCPackingYield(Convert.ToDecimal(input.SerialNumber));
            //}
            //#AZRUL 11/01/2019
            //#AZRUL 17/6/2019: Get TotalPcs if partial packed.
            // TODO
            //#AZRUL 17/6/2019
            decimal calculatedSampleQty = 0;
            calculatedSampleQty = BalancePcs -
                (input.TotalPcs + input.RAFGoodQtyLoose + input.RejectedQty + input.SecGradeQty);

            if (CheckIsPostWT(input.QCType))
                calculatedSampleQty = calculatedSampleQty - input.RAFWTSample;
            //+ input.RAFVTSample); // #MH 07/11/2018 VT Sample not require anymore

            if (calculatedSampleQty < 0)
                calculatedSampleQty = 0;
            //if (isCalculatedLooseQty)
            //    input.CalculatedLooseQty = calculatedSampleQty;
            //else
            input.RejectedSampleQty = calculatedSampleQty;
            return input;
        }

        /// <summary>
        /// Get Is Post Water Tight to D365
        /// </summary>
        /// <param name="QCType"></param>
        /// <returns>boolean</returns>
        public static bool CheckIsPostWT(string QCType)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@QCType", QCType));
            return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_IsPostWT", lstParameters));
        }

        /// <summary>
        /// Get Previous SPBC PlantNo for compare with Rework PlantNo
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>string</returns>
        public static string GetPrevSPBCPlantNo(string serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_Get_Prev_SPBC_PlantNo", lstParameters));
        }

        /// <summary>
        /// Get Rework After SOBC for 2nd SOBC
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>boolean</returns>
        public static bool GetReworkAfterSOBCForSecondSOBC(String serialNumber)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            return Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_RWK_After_SOBC_For_Second_SOBC", lstParameters));
        }

        /// <summary>
        /// Get QI Test Result Status to avoid user scan QI Test again without Scan In QC.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static QITestResultStatusDto Get_PT_Or_QC_QITestResult(Decimal serialNumber)
        {
            QITestResultStatusDto qiResultStatus = new QITestResultStatusDto();
            if (Convert.ToString(serialNumber).Length != 10)
            {
                return qiResultStatus;
            }
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            using (DataTable table = FloorDBAccess.ExecuteDataTable("USP_DOT_GET_PT_OR_QC_QITestResult", PrmList))
            {
                try
                {
                    if (table != null && table.Rows.Count > 0)
                    {
                        qiResultStatus.Stage = FloorDBAccess.GetString(table.Rows[0], "Stage");
                        qiResultStatus.QIResult = FloorDBAccess.GetString(table.Rows[0], "QIResult");
                        qiResultStatus.WasherScanInCount = Convert.ToInt32(table.Rows[0]["WasherScanInCount"]);
                        qiResultStatus.DryerScanInCount = Convert.ToInt32(table.Rows[0]["DryerScanInCount"]);
                        qiResultStatus.PTScanInCount = Convert.ToInt32(table.Rows[0]["PTScanInCount"]);
                        qiResultStatus.QCScanInCount = Convert.ToInt32(table.Rows[0]["QCScanInCount"]);
                        qiResultStatus.QITestResultCount = Convert.ToInt32(table.Rows[0]["QITestResultCount"]);
                        qiResultStatus.QCType = FloorDBAccess.GetString(table.Rows[0], "QCType");
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                }
            }
            return qiResultStatus;
        }
        #endregion
    }
}
