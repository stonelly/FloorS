using Hartalega.FloorSystem.Framework.Cache;
using System;

namespace Hartalega.FloorSystem.Framework
{
    public static class Messages
    {
        public static string PRESHIPMENTQASUBJECT { get { return CacheManager.Get("PRESHIPMENTQASUBJECT") as string; } }
        public static string SELECT_CORRECT_GROUP { get { return CacheManager.Get("SELECT_CORRECT_GROUP") as string; } }


        #region WorkStationConfiguration
        public static string EMPTY_KEY_TEXT { get { return CacheManager.Get("EMPTY_KEY_TEXT") as string; } }
        public static string ENTER_CONFIGURATION_KEYS { get { return CacheManager.Get("ENTER_CONFIGURATION_KEYS") as string; } }
        public static string MODULE_INFO_NOT_AVAILABLE_IN_DATABASE { get { return CacheManager.Get("MODULE_INFO_NOT_AVAILABLE_IN_DATABASE") as string; } }
        public static string WORKSTATION_NOT_EXIST { get { return CacheManager.Get("WORKSTATION_NOT_EXIST") as string; } }
        public static string LOCATION_INFO_NOT_AVAILABLE_IN_DATABASE { get { return CacheManager.Get("LOCATION_INFO_NOT_AVAILABLE_IN_DATABASE") as string; } }
        public static string WORKSTATION_INFO_NOT_AVAILABLE_IN_DATABASE { get { return CacheManager.Get("WORKSTATION_INFO_NOT_AVAILABLE_IN_DATABASE") as string; } }
        public static string SYSTEM_WIDE_CONFIGURATION_NOT_AVAILABLE { get { return CacheManager.Get("SYSTEM_WIDE_CONFIGURATION_NOT_AVAILABLE") as string; } }
        #endregion
        public static string EX_COLUMN_NOT_FOUND { get { return CacheManager.Get("EX_COLUMN_NOT_FOUND") as string; } }
        public static string EX_GENERIC_SYDB_EXCEPTION { get { return CacheManager.Get("EX_GENERIC_SYDB_EXCEPTION") as string; } }
        public static string NO_DRYER_AVAILABLE { get { return CacheManager.Get("NO_DRYER_AVAILABLE") as string; } }
        public static string NO_DRYERPROGRAM_AVAILABLE { get { return CacheManager.Get("NO_DRYERPROGRAM_AVAILABLE") as string; } }
        public static string NO_WASHER_AVAILABLE { get { return CacheManager.Get("NO_WASHER_AVAILABLE") as string; } }
        public static string NO_WASHER_PROGRAM_AVAILABLE { get { return CacheManager.Get("NO_WASHER_PROGRAM_AVAILABLE") as string; } }
        public static string EX_FORMATEXCEPTION_MESSAGE { get { return CacheManager.Get("EX_FORMATEXCEPTION_MESSAGE") as string; } }
        public static string EX_INVALIDCASTEXCEPTION_MESSAGE { get { return CacheManager.Get("EX_INVALIDCASTEXCEPTION_MESSAGE") as string; } }
        public static string EX_VALUEOVERFLOW_MESSAGE { get { return CacheManager.Get("EX_VALUEOVERFLOW_MESSAGE") as string; } }
        public static string EX_ARGUMENTNULL_MESSAGE { get { return CacheManager.Get("EX_ARGUMENTNULL_MESSAGE") as string; } }
        /// <summary>
        /// staticants defined for Tumbling
        /// </summary>
        #region TumblingModule
        public static string CONFIRM_MESSAGE { get { return CacheManager.Get("CONFIRM_MESSAGE") as string; } }
        public static string SELECT_ACTIVITY_DATE_AND_TIME_GREATER_THAN_LAST_ACTIVITY_DATE { get { return CacheManager.Get("SELECT_ACTIVITY_DATE_AND_TIME_GREATER_THAN_LAST_ACTIVITY_DATE") as string; } }
        public static string DATA_SAVED_SUCCESSFULLY { get { return CacheManager.Get("DATA_SAVED_SUCCESSFULLY") as string; } }
        public static string DATA_SAVED_SUCCESSFULLY_SM { get { return CacheManager.Get("DATA_SAVED_SUCCESSFULLY_SM") as string; } }
        public static string INVALIDDATA { get { return CacheManager.Get("INVALIDDATA") as string; } }
        public static string DATA_SAVED_SUCCESSFULLY_QAI { get { return CacheManager.Get("DATA_SAVED_SUCCESSFULLY_QAI") as string; } }
        public static string DATA_SAVED_UNSUCCESSFUL_QAI { get { return CacheManager.Get("DATA_SAVED_UNSUCCESSFUL_QAI") as string; } }
        public static string QAI_ChangeQctypeValidation { get { return CacheManager.Get("QAI_ChangeQctypeValidation") as string; } }
        public static string CONTACT_MIS { get { return CacheManager.Get("CONTACT_MIS") as string; } }
        public static string ENTER_DATA { get { return CacheManager.Get("Enter_Data") as string; } }
        public static string WEIGHT_OUT_OF_RANGE { get { return CacheManager.Get("WEIGHT_OUT_OF_RANGE") as string; } }
        public static string PCSCOUNT_RANGE { get { return CacheManager.Get("PCSCOUNT_RANGE") as string; } }
        public static string REQUIREDFIELDMESSAGE { get { return (CacheManager.Get("REQUIREDFIELDMESSAGE") as string + Environment.NewLine); } }
        public static string CLEARMESSAGE { get { return CacheManager.Get("CLEARMESSAGE") as string; } }
        public static string IS_CREATE_REWORK { get { return CacheManager.Get("IS_CREATE_REWORK") as string; } }
        public static string REWORK_QTY_IS_ZERO { get { return CacheManager.Get("REWORK_QTY_IS_ZERO") as string; } }

        public static string INVALID_GLOVETYPE_REJECT { get { return CacheManager.Get("INVALID_GLOVETYPE_REJECT") as string; } }

        public static string SAVE_SECONDGRADE_VERIFY { get { return CacheManager.Get("SAVE_SECONDGRADE_VERIFY") as string; } }

        public static string SAVE_SECONDGRADE_DISPOSAL { get { return CacheManager.Get("SAVE_SECONDGRADE_DISPOSAL") as string; } }

        public static string SAVE_REJECTGRADE_DISPOSAL { get { return CacheManager.Get("SAVE_REJECTGRADE_DISPOSAL") as string; } }
        public static string CANCELMESSAGE { get { return CacheManager.Get("CANCELMESSAGE") as string; } }
        public static string INVALID_SERIAL_NUMBER { get { return CacheManager.Get("INVALID_SERIAL_NUMBER") as string; } }

        public static string POLYMERTEST_NOT_REQUIRED { get { return CacheManager.Get("POLYMERTEST_NOT_REQUIRED") as string; } }
        public static string INVALID_SERIAL_NUMBER_ONLINEBYPASS { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_ONLINEBYPASS") as string; } }

        // public static string INVALID_GLOVETYPE_REJECT { get { return CacheManager.Get("INVALID_GLOVETYPE_REJECT") as string; } }
        public static string INVALID_GLOVETYPE_POWDER { get { return CacheManager.Get("INVALID_GLOVETYPE_POWDER") as string; } }
        public static string INVALID_GLOVETYPE_PROTEIN { get { return CacheManager.Get("INVALID_GLOVETYPE_PROTEIN") as string; } }
        public static string INVALID_GLOVETYPE_HOTBOX { get { return CacheManager.Get("INVALID_GLOVETYPE_HOTBOX") as string; } }
        public static string INVALID_SERIAL_NUMBER_QC { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_QC") as string; } }
        public static string INVALID_SERIAL_NUMBER_QA { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_QA") as string; } }
        public static string INVALID_SERIAL_NUMBER_PT { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_PT") as string; } }
        public static string INVALID_OPERATOR_ID_WASHER { get { return CacheManager.Get("INVALID_OPERATOR_ID_WASHER") as string; } }
        public static string INVALID_SERIAL_NUMBER_WASHER { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_WASHER") as string; } }
        public static string INVALID_DATA_SUMMARY { get { return (CacheManager.Get("INVALID_DATA_SUMMARY") as string + Environment.NewLine); } }
        public static string INVALID_SERIAL_NUMBER_WASHER_SC { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_WASHER_SC") as string; } }
        public static string INVALID_SERIAL_NUMBER_LOCATION { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_LOCATION") as string; } }
        public static string INVALID_SERIAL_NUMBER_PT_LOCATION { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_PT_LOCATION") as string; } }

        public static string INVALID_SERIAL_NUMBER_PN_LOCATION { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_PN_LOCATION") as string; } }
        public static string QCQI_WITHOUT_PTQI { get { return CacheManager.Get("QCQI_WITHOUT_PTQI") as string; } }
        public static string INVALID_NEW_PAS_OR_FAIL_PTQI_QC { get { return CacheManager.Get("INVALID_NEW_PAS_OR_FAIL_PTQI_QC") as string; } }
        public static string INVALID_PTQI_PASS_QC { get { return CacheManager.Get("INVALID_PTQI_PASS_QC") as string; } }
        public static string INVALID_NEW_SURGICAL_PTPF_NO_PTQI_PASS { get { return CacheManager.Get("INVALID_NEW_SURGICAL_PTPF_NO_PTQI_PASS") as string; } }
        public static string INVALID_QI_PASS { get { return CacheManager.Get("INVALID_QI_PASS") as string; } }
        public static string INVALID_SERIAL_NUMBER_PN_WATERTIGHT { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_PN_WATERTIGHT") as string; } }
        public static string INVALID_SERIAL_NUMBER_DRYER_SC { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_DRYER_SC") as string; } }
        public static string EMPTY_TESTER_ID { get { return CacheManager.Get("EMPTY_TESTER_ID") as string; } }
        public static string INVALID_TESTER_ID { get { return CacheManager.Get("INVALID_TESTER_ID") as string; } }
        public static string INVALID_OPERATOR_ID_PWB { get { return CacheManager.Get("INVALID_OPERATOR_ID_PWB") as string; } }
        public static string INVALID_OPERATOR_ID { get { return CacheManager.Get("INVALID_OPERATOR_ID") as string; } }
        public static string INVALID_MEMBER_ID { get { return CacheManager.Get("INVALID_MEMBER_ID") as string; } }
        public static string INVALID_GLOVETYPE { get { return CacheManager.Get("INVALID_GLOVETYPE") as string; } }
        public static string INVALID_GLOVETYPE_FOR_LINE { get { return CacheManager.Get("INVALID_GLOVETYPE_FOR_LINE") as string; } }
        public static string GLOVETYPE_STOPPED { get { return CacheManager.Get("GLOVETYPE_STOPPED") as string; } }
        public static string INVALIDGLOVETYPELINESIZE { get { return CacheManager.Get("INVALIDGLOVETYPELINESIZE") as string; } }
        public static string INVALID_SIZE_FOR_GLOVETYPE { get { return CacheManager.Get("INVALID_SIZE_FOR_GLOVETYPE") as string; } }
        public static string INVALID_SIZE_FOR_LINE_AND_GLOVETYPE { get { return CacheManager.Get("INVALID_SIZE_FOR_LINE_AND_GLOVETYPE") as string; } }
        public static string INVALIDGLOVETYPEANDSIZE { get { return CacheManager.Get("INVALIDGLOVETYPEANDSIZE") as string; } }

        public static string INVALIDWEIGHT_GLOVETYPEMASTER { get { return CacheManager.Get("INVALIDWEIGHT_GLOVETYPEMASTER") as string; } }

        //invalid batch and 10pcs for tumbling module
        public static string INVALID_VALUE_FOR_10PCS { get { return CacheManager.Get("INVALID_VALUE_FOR_10PCS") as string; } }
        public static string INVALID_VALUE_FOR_BATCH { get { return CacheManager.Get("INVALID_VALUE_FOR_BATCH") as string; } }
        //glove type protein spec invalid message
        public static string INVALID_VALUE_FOR_PROTEINSPEC { get { return CacheManager.Get("INVALID_VALUE_FOR_PROTEINSPEC") as string; } }

        public static string QAI_ChangeBetterQCType { get { return CacheManager.Get("QAI_ChangeBetterQCType") as string; } }
        
        public static string EXCEEDED_YOUR_TRIAL { get { return CacheManager.Get("EXCEEDED_YOUR_TRIAL") as string; } }
        public static string PASSWORD_EMPTY { get { return CacheManager.Get("PASSWORD_EMPTY") as string; } }
        public static string APPLICATIONERROR { get { return CacheManager.Get("APPLICATIONERROR") as string; } }
        public static string AXPOSTINGERROR { get { return CacheManager.Get("AXPOSTINGERROR") as string; } }
        public static string INTEGRATIONERROR { get { return CacheManager.Get("INTEGRATIONERROR") as string; } }
        public static string ROWNOTINTABLEEXCEPTION { get { return CacheManager.Get("ROWNOTINTABLEEXCEPTION") as string; } }
        public static string ARGUMENTEXCEPTION { get { return CacheManager.Get("ARGUMENTEXCEPTION") as string; } }
        public static string CONFIGURATIONERRORSEXCEPTION { get { return CacheManager.Get("CONFIGURATIONERRORSEXCEPTION") as string; } }
        public static string DATABASEERROR { get { return CacheManager.Get("DATABASEERROR") as string; } }
        public static string TenPcsWeightException { get { return CacheManager.Get("TenPcsWeightException") as string; } }
        public static string TEN_PCS_WEIGHT_IS_ZERO { get { return CacheManager.Get("TEN_PCS_WEIGHT_IS_ZERO") as string; } }
        public static string GETSIZEBYGLOVETYPEEXCEPTION { get { return CacheManager.Get("GETSIZEBYGLOVETYPEEXCEPTION") as string; } }
        public static string GETSIZEEXCEPTION { get { return CacheManager.Get("GETSIZEEXCEPTION") as string; } }
        public static string GETSHIFTEXCEPTION { get { return CacheManager.Get("GETSHIFTEXCEPTION") as string; } }
        public static string GETMINMAXMETHODEXCEPTION { get { return CacheManager.Get("GETMINMAXMETHODEXCEPTION") as string; } }
        public static string GETSERIALDETAILSMETHODEXCEPTION { get { return CacheManager.Get("GETSERIALDETAILSMETHODEXCEPTION") as string; } }
        public static string GETREASONSMETHODEXCEPTION { get { return CacheManager.Get("GETREASONSMETHODEXCEPTION") as string; } }
        public static string GETDATEMETHODEXCEPTION { get { return CacheManager.Get("GETDATEMETHODEXCEPTION") as string; } }
        public static string GETSAVEMETHODEXCEPTION { get { return CacheManager.Get("GETSAVEMETHODEXCEPTION") as string; } }
        public static string GETENUMEXCEPTION { get { return CacheManager.Get("GETENUMEXCEPTION") as string; } }
        public static string GETLINEBYLOCATIONEXCEPTION { get { return CacheManager.Get("GETLINEBYLOCATIONEXCEPTION") as string; } }
        public static string GETPLANTLISTEXCEPTION { get { return CacheManager.Get("GETPLANTLISTEXCEPTION") as string; } }
        public static string GETBATCHCARDREPRINTLOGEEXCEPTION { get { return CacheManager.Get("GETBATCHCARDREPRINTLOGEEXCEPTION") as string; } }
        public static string MANUALPRINTBATCHDETAILSGETEXCEPTION { get { return CacheManager.Get("MANUALPRINTBATCHDETAILSGETEXCEPTION") as string; } }
        public static string GETLINEEXCEPTION { get { return CacheManager.Get("GETLINEEXCEPTION") as string; } }
        public static string GETREASONFORREPRINTEXCEPTION { get { return CacheManager.Get("GETREASONFORREPRINTEXCEPTION") as string; } }
        public static string GETLINESELECTIONDETAILSEXCEPTION { get { return CacheManager.Get("GETLINESELECTIONDETAILSEXCEPTION") as string; } }
        public static string INVALID_VALUE_FOR_BATCH_PCS { get { return CacheManager.Get("INVALID_VALUE_FOR_BATCH_PCS") as string; } } // #GARY 06/11/2020: HTLG_P7CR_014&015: Invalid msg for Batch(Pcs)

        public static string INVALID_VALUE_FOR_BATCH_GRAMS { get { return CacheManager.Get("INVALID_VALUE_FOR_BATCH_GRAMS") as string; } }

        public static string INVALID_LOOSE_QTY { get { return CacheManager.Get("INVALID_LOOSE_QTY") as string; } }

        public static string INVALID_REJECTION_QTY { get { return CacheManager.Get("INVALID_REJECTION_QTY") as string; } }

        public static string INVALID_2NDGRADE_QTY { get { return CacheManager.Get("INVALID_2NDGRADE_QTY") as string; } }

        #endregion

        #region HBC
        public static string SHIFT_NOT_SELECT { get { return CacheManager.Get("SHIFT_NOT_SELECT") as string; } }
        public static string RESOURCE_OUTPUT_1_NOT_SELECT { get { return CacheManager.Get("RESOURCE_OUTPUT_1_NOT_SELECT") as string; } }
        public static string BATCHORDER_OUTPUT_1_NOT_SELECT { get { return CacheManager.Get("BATCHORDER_OUTPUT_1_NOT_SELECT") as string; } }
        public static string BATCHORDER_OUTPUT_2_NOT_SELECT { get { return CacheManager.Get("BATCHORDER_OUTPUT_2_NOT_SELECT") as string; } }
        public static string BATCHORDER_OUTPUT_3_NOT_SELECT { get { return CacheManager.Get("BATCHORDER_OUTPUT_3_NOT_SELECT") as string; } }
        public static string BATCHORDER_OUTPUT_4_NOT_SELECT { get { return CacheManager.Get("BATCHORDER_OUTPUT_4_NOT_SELECT") as string; } }
        public static string PACKSIZE_1_NOT_SELECT { get { return CacheManager.Get("PACKSIZE_1_NOT_SELECT") as string; } }
        public static string PACKSIZE_2_NOT_SELECT { get { return CacheManager.Get("PACKSIZE_2_NOT_SELECT") as string; } }
        public static string PACKSIZE_3_NOT_SELECT { get { return CacheManager.Get("PACKSIZE_3_NOT_SELECT") as string; } }
        public static string PACKSIZE_4_NOT_SELECT { get { return CacheManager.Get("PACKSIZE_4_NOT_SELECT") as string; } }
        public static string INBOX_1_NOT_SELECT { get { return CacheManager.Get("INBOX_1_NOT_SELECT") as string; } }
        public static string INBOX_2_NOT_SELECT { get { return CacheManager.Get("INBOX_2_NOT_SELECT") as string; } }
        public static string INBOX_3_NOT_SELECT { get { return CacheManager.Get("INBOX_3_NOT_SELECT") as string; } }
        public static string INBOX_4_NOT_SELECT { get { return CacheManager.Get("INBOX_4_NOT_SELECT") as string; } }
        public static string IDENTICAL_OUTPUT1_2 { get { return CacheManager.Get("IDENTICAL_OUTPUT1_2") as string; } }
        public static string IDENTICAL_OUTPUT2_3 { get { return CacheManager.Get("IDENTICAL_OUTPUT2_3") as string; } }
        public static string IDENTICAL_OUTPUT2_4 { get { return CacheManager.Get("IDENTICAL_OUTPUT2_4") as string; } }
        public static string IDENTICAL_OUTPUT1_3 { get { return CacheManager.Get("IDENTICAL_OUTPUT1_3") as string; } }
        public static string IDENTICAL_OUTPUT3_4 { get { return CacheManager.Get("IDENTICAL_OUTPUT3_4") as string; } }
        public static string IDENTICAL_OUTPUT1_4 { get { return CacheManager.Get("IDENTICAL_OUTPUT1_4") as string; } }
        public static string UNSUFFICIENT_GLOVE { get { return CacheManager.Get("UNSUFFICIENT_GLOVE") as string; } }
        public static string L_TIER_UNIDENTICAL { get { return CacheManager.Get("L_TIER_UNIDENTICAL") as string; } }
        public static string R_TIER_UNIDENTICAL { get { return CacheManager.Get("R_TIER_UNIDENTICAL") as string; } }
        public static string OUTPUT1_2_TIER_NOT_SAME { get { return CacheManager.Get("OUTPUT1_2_TIER_NOT_SAME") as string; } }
        public static string OUTPUT3_4_TIER_NOT_SAME { get { return CacheManager.Get("OUTPUT3_4_TIER_NOT_SAME") as string; } }
        public static string THREE_TIER_BLOCKED { get { return CacheManager.Get("3_TIER_BLOCKED") as string; } }
        #endregion

        #region Line Clearence Verification
        public static string EMPTY_EMPLOYEE_ID { get { return CacheManager.Get("EMPTY_EMPLOYEE_ID") as string; } }
        public static string INVALID_PIN { get { return CacheManager.Get("INVALID_PIN") as string; } }

        public static string VERIFY_DENIED { get { return CacheManager.Get("VERIFY_DENIED") as string; } }

        public static string EMP_NOT_EXISTS { get { return CacheManager.Get("EMP_NOT_EXISTS") as string; } }
        #endregion

        #region SurgicalGloveSystem
        public static string RESOURCE_NOT_SELECT { get { return CacheManager.Get("RESOURCE_NOT_SELECT") as string; } }
        public static string BATCHORDER_NOT_SELECT { get { return CacheManager.Get("BATCHORDER_NOT_SELECT") as string; } }
        public static string BATCHKG_IS_EMPTY { get { return CacheManager.Get("BATCHKG_IS_EMPTY") as string; } }
        public static string QTYPCS_IS_EMPTY { get { return CacheManager.Get("QTYPCS_IS_EMPTY") as string; } }
        public static string BATCHKG_IS_0 { get { return CacheManager.Get("BATCHKG_IS_0") as string; } }
        public static string QTYPCS_IS_0 { get { return CacheManager.Get("QTYPCS_IS_0") as string; } }
        public static string NON_SURGICAL_BLOCKED { get { return CacheManager.Get("NON_SURGICAL_BLOCKED") as string; } }
        public static string SURGICAL_BLOCKED { get { return CacheManager.Get("SURGICAL_BLOCKED") as string; } }
        #endregion

        public static string BIN_INVALID { get { return CacheManager.Get("BIN_INVALID") as string; } }
        public static string BIN_DUPLICATE { get { return CacheManager.Get("BIN_DUPLICATE") as string; } }
        public static string BIN_EMPTY { get { return CacheManager.Get("BIN_EMPTY") as string; } }
        public static string BIN_DUPLICATE_CONFIRM { get { return CacheManager.Get("BIN_DUPLICATE_CONFIRM") as string; } }
        public static string SERIAL_NUMBER_EMPTY { get { return CacheManager.Get("SERIAL_NUMBER_EMPTY") as string; } }
        public static string SERIAL_NUMBER_INVALID { get { return CacheManager.Get("SERIAL_NUMBER_INVALID") as string; } }
        public static string BATCH_NOT_EXISTS { get { return CacheManager.Get("BATCH_NOT_EXISTS") as string; } }
        public static string BATCH_NOT_SCANIN { get { return CacheManager.Get("BATCH_NOT_SCANIN") as string; } }
        public static string QAI_INCOMPLETE { get { return CacheManager.Get("QAI_INCOMPLETE") as string; } }
        public static string SLIP_REPRINT { get { return CacheManager.Get("SLIP_REPRINT") as string; } }
        public static string SLIP_REWORK { get { return CacheManager.Get("SLIP_REWORK") as string; } }
        public static string QAI_EXPIRED { get { return CacheManager.Get("QAI_EXPIRED") as string; } }
        public static string REWORK_MESSAGE { get { return CacheManager.Get("REWORK_MESSAGE") as string; } }
        public static string NO_DATA_FOUND { get { return CacheManager.Get("NO_DATA_FOUND") as string; } }
        public static string NO_DATA_FOUND_SC { get { return CacheManager.Get("NO_DATA_FOUND_SC") as string; } }
        public static string SELECT_NEXTPROCESS { get { return CacheManager.Get("SELECT_NEXTPROCESS") as string; } }
        public static string EX_GENERIC_EXCEPTION_GETBATCHDETAILS { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_GETBATCHDETAILS") as string; } }
        public static string EX_GENERIC_EXCEPTION_WASHERDETAILS_FOR_GLOVETYPE_AND_SIZE { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_WASHERDETAILS_FOR_GLOVETYPE_AND_SIZE") as string; } }
        public static string EX_GENERIC_EXCEPTION_WASHERPROGRAM_FOR_GLOVETYPE { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_WASHERPROGRAM_FOR_GLOVETYPE") as string; } }
        public static string EX_GENERIC_EXCEPTION_REASONTEXT_FOR_REASONTYPE { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_REASONTEXT_FOR_REASONTYPE") as string; } }
        public static string EX_GENERIC_EXCEPTION_OPERATORNAME { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_OPERATORNAME") as string; } }
        public static string EX_GENERIC_EXCEPTION_SUCCESS { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_SUCCESS") as string; } }
        public static string EX_GENERIC_EXCEPTION_FAILURE { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_FAILURE") as string; } }
        public static string EX_GENERIC_EXCEPTION_INVALID_OPERATORID_QAIINSPECTOR { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_INVALID_OPERATORID_QAIINSPECTOR") as string; } }
        public static string EX_GENERIC_EXCEPTION_INVALID_OPERATORID { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_INVALID_OPERATORID") as string; } }
        public static string RESOURCE_ALREADY_USED_WITH_THE_SELECTED_TIME { get { return CacheManager.Get("RESOURCE_ALREADY_USED_WITH_THE_SELECTED_TIME") as string; } }
        public static string EX_GENERIC_EXCEPTION_INVALID_OPERATORID_QAI { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_INVALID_OPERATORID_QAI") as string; } }
        public static string EX_GENERIC_EXCEPTION_QCTYPE_COMBINATION { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_QCTYPE_COMBINATION") as string; } }
        public static string EX_GENERIC_EXCEPTION_WASHERNUMBER_FOR_WASHERID { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_WASHERNUMBER_FOR_WASHERID") as string; } }
        //public static string EX_GENERIC_EXCEPTION_QCTYPE_COMBINATION { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_QCTYPE_COMBINATION") as string; } }
        public static string EX_GENERIC_EXCEPTION_VALIDATEWASHERPROGRAM { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_VALIDATEWASHERPROGRAM") as string; } }
        public static string EX_GENERIC_EXCEPTION_SAVE_SCANBATCHCARD { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_SAVE_SCANBATCHCARD") as string; } }
        public static string EX_GENERIC_EXCEPTION_SAVE_SCAN_OUT_BATCHCARD { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_SAVE_SCAN_OUT_BATCHCARD") as string; } }
        public static string EX_GENERIC_EXCEPTION_VALIDATE_STOPTIME { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_VALIDATE_STOPTIME") as string; } }
        public static string EX_GENERIC_EXCEPTION_DRYERNUMBER_FOR_DRYERID { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_DRYERNUMBER_FOR_DRYERID") as string; } }
        public static string EX_GENERIC_EXCEPTION_DRYERDETAILS_FOR_GLOVETYPE_AND_SIZE { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_DRYERDETAILS_FOR_GLOVETYPE_AND_SIZE") as string; } }
        public static string EX_GENERIC_EXCEPTION_DRYERPROGRAM_FOR_GLOVETYPE_AND_SIZE { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_DRYERPROGRAM_FOR_GLOVETYPE_AND_SIZE") as string; } }
        public static string EX_GENERIC_EXCEPTION_VALIDATEDRYERPROGRAM { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_VALIDATEDRYERPROGRAM") as string; } }
        public static string INVALIDFORMAT_OPERATORID { get { return CacheManager.Get("INVALIDFORMAT_OPERATORID") as string; } }
        public static string DUPLICATE_WASHER_SERIAL_NUMBER { get { return CacheManager.Get("DUPLICATE_WASHER_SERIAL_NUMBER") as string; } } //"Washer details for this serial number already exists. Do you want to rework?";
        public static string BATCH_IS_IN_QC_PROCESS { get { return CacheManager.Get("BATCH_IS_IN_QC_PROCESS") as string; } }
        public static string PREVIOUS_DRYER_PROCESS_FOR_SERIAL_NUMBER_NOT_COMPLETED { get { return CacheManager.Get("PREVIOUS_DRYER_PROCESS_FOR_SERIAL_NUMBER_NOT_COMPLETED") as string; } }
        public static string DUPLICATE_WASHER_SERIAL_NUMBER_ENTRY_DISCARDED_BY_USER { get { return CacheManager.Get("DUPLICATE_WASHER_SERIAL_NUMBER_ENTRY_DISCARDED_BY_USER") as string; } }
        public static string INVALIDFORMAT_SERIALNUMBER { get { return CacheManager.Get("INVALIDFORMAT_SERIALNUMBER") as string; } }
        public static string WASHER_NOT_SELECTED { get { return CacheManager.Get("WASHER_NOT_SELECTED") as string; } }
        public static string WASHER_NOT_COMPLETED { get { return CacheManager.Get("WASHER_NOT_COMPLETED") as string; } }//"The last scanned serial number for this washer has not completed its assigned time. Please wait till the current running process is over.";
        public static string CONFIRM_SAVE { get { return CacheManager.Get("CONFIRM_SAVE") as string; } }
        public static string CONFIRM_APPROVE { get { return CacheManager.Get("CONFIRM_APPROVE") as string; } }
        public static string CONFIRM_REOPEN { get { return CacheManager.Get("CONFIRM_REOPEN") as string; } }
        public static string REWORK_REASON_NOT_SELECTED { get { return CacheManager.Get("REWORK_REASON_NOT_SELECTED") as string; } }
        public static string CONFIRM_CANCEL { get { return CacheManager.Get("CONFIRM_CANCEL") as string; } }
        public static string CONFIRM_CANCEL_TRANSACTION { get { return CacheManager.Get("CONFIRM_CANCEL_TRANSACTION") as string; } }
        public static string REQUIREDFIELD_OPERATORID { get { return CacheManager.Get("REQUIREDFIELD_OPERATORID") as string; } }
        public static string REQUIREDFIELD_WASHER { get { return CacheManager.Get("REQUIREDFIELD_WASHER") as string; } }
        public static string REQUIRED_DATA { get { return CacheManager.Get("REQUIRED_DATA") as string; } }
        public static string REQUIREDFIELD_WASHER_PROGRAM { get { return CacheManager.Get("REQUIREDFIELD_WASHER_PROGRAM") as string; } }
        public static string REQUIREDFIELD_DRYER_PROGRAM { get { return CacheManager.Get("REQUIREDFIELD_DRYER_PROGRAM") as string; } }
        public static string PREVIOUS_WASHER_PROCESS_FOR_SERIAL_NUMBER_NOT_COMPLETED { get { return CacheManager.Get("PREVIOUS_WASHER_PROCESS_FOR_SERIAL_NUMBER_NOT_COMPLETED") as string; } }
        public static string PREVIOUS_WASHER_DRYER_PROCESS_FOR_SERIAL_NUMBER_NOT_COMPLETED { get { return CacheManager.Get("PREVIOUS_WASHER_DRYER_PROCESS_FOR_SERIAL_NUMBER_NOT_COMPLETED") as string; } }
        public static string ERROR_CAPTION { get { return CacheManager.Get("ERROR_CAPTION") as string; } }
        public static string ERROR_TITLE { get { return CacheManager.Get("ERROR_TITLE") as string; } }
        public static string WASHER_DURATION_NOT_COMPLETED { get { return CacheManager.Get("WASHER_DURATION_NOT_COMPLETED") as string; } }
        public static string WASHER_PROGRAM_ALREADY_STOPPED { get { return CacheManager.Get("WASHER_PROGRAM_ALREADY_STOPPED") as string; } }
        public static string SERIAL_NUMBER_NOT_SCANNED_WASHER { get { return CacheManager.Get("SERIAL_NUMBER_NOT_SCANNED_WASHER") as string; } }
        public static string DRYER_DURATION_NOT_COMPLETED { get { return CacheManager.Get("DRYER_DURATION_NOT_COMPLETED") as string; } }
        public static string QAI_EXPIRY_INFORMATION { get { return CacheManager.Get("QAI_EXPIRY_INFORMATION") as string; } }
        public static string DRYER_PROGRAM_ALREADY_STOPPED { get { return CacheManager.Get("DRYER_PROGRAM_ALREADY_STOPPED") as string; } }
        public static string DUPLICATE_DRYER_SERIAL_NUMBER { get { return CacheManager.Get("DUPLICATE_DRYER_SERIAL_NUMBER") as string; } }
        public static string DUPLICATE_DRYER_SERIAL_NUMBER_ENTRY_DISCARDED_BY_USER { get { return CacheManager.Get("DUPLICATE_DRYER_SERIAL_NUMBER_ENTRY_DISCARDED_BY_USER") as string; } }
        public static string DRYER_NOT_SELECTED { get { return CacheManager.Get("DRYER_NOT_SELECTED") as string; } }
        public static string DRYER_NOT_COMPLETED { get { return CacheManager.Get("DRYER_NOT_COMPLETED") as string; } }
        public static string SERIAL_NUMBER_NOT_SCANNED_DRYER { get { return CacheManager.Get("SERIAL_NUMBER_NOT_SCANNED_DRYER") as string; } }
        public static string WASHER_NOT_COMPLETED_ONE_TIME { get { return CacheManager.Get("WASHER_NOT_COMPLETED_ONE_TIME") as string; } }
        public static string MACHINE_HAS_NOT_CONFIGURE { get { return CacheManager.Get("MACHINE_HAS_NOT_CONFIGURE") as string; } }
        public static string SCAN_MULTIPLE_BATCH_TABLE { get { return CacheManager.Get("SCANMULTIPLEBATCHTABLE") as string; } }
        public static string BOXES_PACKED_COUNT { get { return CacheManager.Get("BOXES_PACKED_COUNT") as string; } }
        public static string INNERBOXES_IS_MAX { get { return CacheManager.Get("INNERBOXES_IS_MAX") as string; } }
        public static string TOTAL_INNERBOXES_MOD_GOT_BALANCE { get { return CacheManager.Get("TOTAL_INNERBOXES_MOD_GOT_BALANCE") as string; } }
        public static string BOXES_LESSTHANCASECAPACITY { get { return CacheManager.Get("BOXES_LESSTHANCASECAPACITY") as string; } }
        public static string SECONDGRADE_LESSTHANCASECAPACITY { get { return CacheManager.Get("SECONDGRADE_LESSTHANCASECAPACITY") as string; } }
        public static string SERIALNO_ALREADY_SCANNEDIN { get { return CacheManager.Get("SERIALNO_ALREADY_SCANNEDIN") as string; } }
        public static string LOTNUMBERLIMITEXCEEDS { get { return CacheManager.Get("LOTNUMBERLIMITEXCEEDS") as string; } }
        public static string EXCEED_CASESPACKED_LIMIT { get { return CacheManager.Get("EXCEED_CASESPACKED_LIMIT") as string; } }
        public static string SERIALNUMBER_INVALID { get { return CacheManager.Get("SERIALNUMBER_INVALID") as string; } }
        public static string DUPLICATE_BATCH { get { return CacheManager.Get("DUPLICATE_BATCH") as string; } }
        /// <summary>
        /// BATCH DETAILS EXCEPTION MESSAGE
        /// </summary>
        public static string BATCH_EXCEPTION { get { return CacheManager.Get("BATCH_EXCEPTION") as string; } }
        /// <summary>
        /// REASON DETAILS EXCEPTION MESSAGE
        /// </summary>
        public static string REASON_EXCEPTION { get { return CacheManager.Get("REASON_EXCEPTION") as string; } }
        /// <summary>
        /// REASON SAVE EXCEPTION MESSAGE
        /// </summary>
        public static string REASON_SAVE_EXCEPTION { get { return CacheManager.Get("REASON_SAVE_EXCEPTION") as string; } }
        /// <summary>
        /// WASHER DETAILS EXCEPTION MESSAGE
        /// </summary>
        public static string WASHER_EXCEPTION { get { return CacheManager.Get("WASHER_EXCEPTION") as string; } }

        public static string GETLINEMASTEREXCEPTION { get { return CacheManager.Get("GETLINEMASTEREXCEPTION") as string; } }

        public static string EX_GENERIC_EXCEPTION { get { return CacheManager.Get("EX_GENERIC_EXCEPTION") as string; } }
        public static string SELECT_LOCATION { get { return CacheManager.Get("SELECT_LOCATION") as string; } }
        public static string GETLOCATION_EXCEPTION { get { return CacheManager.Get("GETLOCATION_EXCEPTION") as string; } }
        public static string EXPORTTOEXCEL_EXCEPTION { get { return CacheManager.Get("EXPORTTOEXCEL_EXCEPTION") as string; } }
        public static string EXPORTTOEXCEL_EXCEPTION_NO_RECORDS_EXPORT { get { return CacheManager.Get("EXPORTTOEXCEL_EXCEPTION_NO_RECORDS_EXPORT") as string; } }
        public static string LISTTODATATABLE_EXCEPTION { get { return CacheManager.Get("LISTTODATATABLE_EXCEPTION") as string; } }
        public static string GET_GLOVETYPE_EXCEPTION { get { return CacheManager.Get("GET_GLOVETYPE_EXCEPTION") as string; } }
        public static string VALIDATE_BIN_EXCEPTION { get { return CacheManager.Get("VALIDATE_BIN_EXCEPTION") as string; } }
        public static string GET_GLOVEINQUIRY_EXCEPTION { get { return CacheManager.Get("GET_GLOVEINQUIRY_EXCEPTION") as string; } }
        public static string GET_TENPCSWEIGHT_EXCEPTION { get { return CacheManager.Get("GET_TENPCSWEIGHT_EXCEPTION") as string; } }
        public static string ARGUMENTNULLEXCEPTION { get { return CacheManager.Get("ARGUMENTNULLEXCEPTION") as string; } }
        public static string VERSIONNOTFOUNDEXCEPTION { get { return CacheManager.Get("VERSIONNOTFOUNDEXCEPTION") as string; } }
        public static string INFORMATION { get { return CacheManager.Get("INFORMATION") as string; } }
        public static string SELECT_REWORKREASON { get { return CacheManager.Get("SELECT_REWORKREASON") as string; } }
        public static string SELECT_DEFECTTYPE { get { return CacheManager.Get("SELECT_DEFECTTYPE") as string; } }
        public static string SELECT_REWORKPROCESS { get { return CacheManager.Get("SELECT_REWORKPROCESS") as string; } }
        public static string INVALID_SHIFT { get { return CacheManager.Get("INVALID_SHIFT") as string; } }
        public static string INVALID_CUSTOMER_BATCH { get { return CacheManager.Get("INVALID_CUSTOMER_BATCH") as string; } }
        public static string GETCALCULATESUGGESTEDQCTYPEEXCEPTION { get { return CacheManager.Get("GETCALCULATESUGGESTEDQCTYPEEXCEPTION") as string; } }

        /// <summary>
        /// DRYER EXCEPTION MESSAGE
        /// </summary>
        public static string DRYER_EXCEPTION { get { return CacheManager.Get("DRYER_EXCEPTION") as string; } }
        /// <summary>
        /// WASHER EDIT EXCEPTION MESSAGE
        /// </summary>
        public static string WASHER_EDIT_EXCEPTION { get { return CacheManager.Get("WASHER_EDIT_EXCEPTION") as string; } }
        public static string NO_REWORK_REASON_AVAILABLE { get { return CacheManager.Get("NO_REWORK_REASON_AVAILABLE") as string; } }
        /// <summary>
        /// DRYER EDIT EXCEPTION MESSAGE
        /// </summary>
        public static string DRYER_EDIT_EXCEPTION { get { return CacheManager.Get("DRYER_EDIT_EXCEPTION") as string; } }
        public static string PASS_SERIALNUMBER_WASHERDRYER { get { return CacheManager.Get("PASS_SERIALNUMBER_WASHERDRYER") as string; } }
        public static string PASS_SERIALNUMBER { get { return CacheManager.Get("PASS_SERIALNUMBER") as string; } }
        public static string SELECT_CHANGEGLOVETYPE { get { return CacheManager.Get("SELECT_CHANGEGLOVETYPE") as string; } }
        public static string REPRINT_CHANGEGLOVETYPE { get { return CacheManager.Get("REPRINT_CHANGEGLOVETYPE") as string; } }

        /// <summary>
        /// LEAVE PAGE MESSAGE
        /// </summary>
        public static string LEAVE_PAGE_MESSAGE { get { return CacheManager.Get("LEAVE_PAGE_MESSAGE") as string; } }
        public static string LEAVE_PAGE_MESSAGE_QAI { get { return CacheManager.Get("LEAVE_PAGE_MESSAGE_QAI") as string; } }
        /// <summary>
        /// INVALID DATA MESSAGE
        /// </summary>
        public static string INVALID_DATA_MESSAGE { get { return (CacheManager.Get("INVALID_DATA_MESSAGE") as string + Environment.NewLine); } }
        /// <summary>
        /// DOWNGRADE CONFIRMATION
        /// </summary>
        public static string DOWNGRADE_CONFIRMATION { get { return CacheManager.Get("DOWNGRADE_CONFIRMATION") as string; } }
        /// <summary>
        /// DOWNGRADETYPE NOT SELECTED MESSAGE
        /// </summary>
        public static string DOWNGRADETYPE_NOT_SELECTED { get { return CacheManager.Get("DOWNGRADETYPE_NOT_SELECTED") as string; } }
        /// <summary>
        /// Batch Already Downgraded
        /// </summary>
        public static string BATCH_ALREADY_DOWNGRADED { get { return CacheManager.Get("BATCH_ALREADY_DOWNGRADED") as string; } }

        /// <summary>
        /// INVALID INNER BOX COUNT MESSAGE
        /// </summary>
        public static string INNERBOX_COUNT_INVALID { get { return CacheManager.Get("INNERBOX_COUNT_INVALID") as string; } }
        /// <summary>
        /// INVALID REASON MESSAGE
        /// </summary>
        public static string INVALID_REASON_TEXT { get { return CacheManager.Get("INVALID_REASON_TEXT") as string; } }
        /// <summary>
        /// FILTER REQUIRED
        /// </summary>
        public static string FILTER_REQUIRED { get { return CacheManager.Get("FILTER_REQUIRED") as string; } }

        public static string DATE_REQUIRED { get { return CacheManager.Get("DATE_REQUIRED") as string; } }


        //Final Packing starts

        public static string STATIONNUMBER_NOTCONFIGURED { get { return CacheManager.Get("STATIONNUMBER_NOTCONFIGURED") as string; } }
        public static string BARCODE_NOT_CORRECTFORMAT { get { return CacheManager.Get("BARCODE_NOT_CORRECTFORMAT") as string; } }

        public static string SELCORRECT_PO { get { return CacheManager.Get("SELCORRECT_PO") as string; } }

        public static string PRINTING_EXCEPTION { get { return CacheManager.Get("PRINTING_EXCEPTION") as string; } }

        public static string EXCEPTION_SURGICAL_INNER { get { return CacheManager.Get("EXCEPTION_SURGICAL_INNER") as string; } }

        public static string EXCEPTION_CUSTOMER_BARCODE { get { return CacheManager.Get("EXCEPTION_CUSTOMER_BARCODE") as string; } }

        public static string EXCEPTION_REPRINT_OUTERLABEL { get { return CacheManager.Get("EXCEPTION_REPRINT_OUTERLABEL") as string; } }


        public static string ITEMSZ_RQ { get { return CacheManager.Get("ITEMSZ_RQ") as string; } }
        public static string PRINT_CONFIRM { get { return CacheManager.Get("PRINT_CONFIRM") as string; } }
        public static string BOXES_VALID_COUNT { get { return CacheManager.Get("BOXES_VALID_COUNT") as string; } }

        public static string PRINT_COMPLETE { get { return CacheManager.Get("PRINT_COMPLETE") as string; } }
        public static string LABELCOUNT_VALIDATION { get { return CacheManager.Get("LABELCOUNT_VALIDATION") as string; } }

        public static string BARCODE_VALIDATION { get { return CacheManager.Get("BARCODE_VALIDATION") as string; } }

        public static string PRINTER_RQ { get { return CacheManager.Get("PRINTER_RQ") as string; } }

        public static string MBT { get { return CacheManager.Get("MBT") as string; } }

        public static string BCH_TMPI { get { return CacheManager.Get("BCH_TMPI") as string; } }

        public static string PRINT_EX { get { return CacheManager.Get("PRINT_EX") as string; } }

        // FX - Add Alert Message for Change Batch Card for Inner Screen
        public static string NEW_BATCH_PACKING_QTY_ZERO { get { return CacheManager.Get("NEW_BATCH_PACKING_QTY_ZERO") as string; } }
        public static string NEW_BATCH_PACKING_QTY_LESS { get { return CacheManager.Get("NEW_BATCH_PACKING_QTY_LESS") as string; } }

        /// <summary>
        /// NO PSI REWORK ORDER NO MESSAGE
        /// </summary>
        /// 
        public static string NOPSIREWORKORDERNO { get { return CacheManager.Get("NOPSIREWORKORDERNO") as string; } }
        public static string INTERNALLOTNOISPOSTED { get { return CacheManager.Get("INTERNALLOTNOISPOSTED") as string; } }

        /// <summary>
        /// PSI REWORK ORDER NOT STARTED MESSAGE
        /// </summary>
        /// 
        public static string PSIREWORKORDERNOTSTARTED { get { return CacheManager.Get("PSIREWORKORDERNOTSTART") as string; } }
        //Validate FG Label
        public static string VALIDATELABEL_CETD { get { return CacheManager.Get("VALIDATELABEL_CETD") as string; } }
        public static string VALIDATELABEL_CUSTLOTID { get { return CacheManager.Get("VALIDATELABEL_CUSTLOTID") as string; } }
        public static string VALIDATELABEL_CETDCUSTLOTID { get { return CacheManager.Get("VALIDATELABEL_CETDCUSTLOTID") as string; } }
        public static string VALIDATELABEL_NOTCONFIGURED { get { return CacheManager.Get("VALIDATELABEL_NOTCONFIGURED") as string; } }
        public static string VALIDATELABEL_PODATE { get { return CacheManager.Get("VALIDATELABEL_PODATE") as string; } }
        public static string VALIDATELABEL_PORDATE { get { return CacheManager.Get("VALIDATELABEL_PORDATE") as string; } }
        //KahHeng 06May2019
        public static string INNEROUTERROLLBACKERROR { get { return CacheManager.Get("INNEROUTERROLLBACKERROR") as string; } }
        //KahHeng end

        /// <summary>
        /// INVALID INTERNALLOTNUMBER MESSAGE
        /// </summary>
        /// 
        public static string INVALIDINTERNALLOTNUMBER { get { return CacheManager.Get("INVALIDINTERNALLOTNUMBER") as string; } }
        /// <summary>
        /// PALLET MAXIMUM CAPACITY REACHED MESSAGE
        /// </summary>
        public static string PALLETMAXIMUMCAPACITYREACHED { get { return CacheManager.Get("PALLETMAXIMUMCAPACITYREACHED") as string; } }
        /// <summary>
        /// SELECT PALLETID Message
        /// </summary>
        public static string SELECTPALLETID { get { return CacheManager.Get("SELECTPALLETID") as string; } }
        /// <summary>
        /// INVALID GLOVECODE FOR SELECTED PO
        /// </summary>
        public static string INVALID_GLOVECODE { get { return CacheManager.Get("INVALID_GLOVECODE") as string; } }

        public static string INVALID_SNO_FP { get { return CacheManager.Get("INVALID_SNO_FP") as string; } }
        public static string SECONDGRADE_SNO_FP { get { return CacheManager.Get("SECONDGRADE_SNO_FP") as string; } }
        public static string NO_SCAN_OUT_SNO_FP { get { return CacheManager.Get("NO_SCAN_OUT_SNO_FP") as string; } }
        public static string TEMPPACK_NOT_SCAN_OUT { get { return CacheManager.Get("TEMPPACK_NOT_SCAN_OUT") as string; } }
        public static string PT_QC_NOT_COMPLETE { get { return CacheManager.Get("PT_QC_NOT_COMPLETE") as string; } }
        public static string PTPF_NOT_ALLOW { get { return CacheManager.Get("PTPF_NOT_ALLOW") as string; } }
        public static string INVALID_SECONDGRADESTICKER { get { return CacheManager.Get("INVALID_SECONDGRADESTICKER") as string; } }
        public static string LOTVERIFICATION_MAILSUBJECT { get { return CacheManager.Get("LOTVERIFICATION_MAILSUBJECT") as string; } }
        public static string FPVISIONE_MAILSUBJECT { get { return CacheManager.Get("FPVISIONE_MAILSUBJECT") as string; } }
        public static string FPVISION_VSAPI_NOT_CONFIGURE { get { return CacheManager.Get("FPVISION_VSAPI_NOT_CONFIGURE") as string; } }
        public static string FPVISION_VSRECIPE_NOT_CONFIGURE { get { return CacheManager.Get("FPVISION_VSRECIPE_NOT_CONFIGURE") as string; } }
        public static string FPVISION_VSAPI_DOWN { get { return CacheManager.Get("FPVISION_VSAPI_DOWN") as string; } }
        public static string FPVISIONVAL_LINECLEAR_CLOSEWINDOW { get { return CacheManager.Get("FPVISIONVAL_LINECLEAR_CLOSEWINDOW") as string; } }
        public static string FPVISIONVAL_LINECLEAR_CLOSEWINDOW_INNERBOX { get { return CacheManager.Get("FPVISIONVAL_LINECLEAR_CLOSEWINDOW_INNERBOX") as string; } }
        public static string FPVISIONVAL_LINECLEAR_CLOSEWINDOW_OUTERBOX { get { return CacheManager.Get("FPVISIONVAL_LINECLEAR_CLOSEWINDOW_OUTERBOX") as string; } }
        public static string FPVISIONVAL_LINECLEAR_REQUIRED { get { return CacheManager.Get("FPVISIONVAL_LINECLEAR_REQUIRED") as string; } }
        public static string FPVISIONVAL_RESPONSE_VALIDATEFAIL { get { return CacheManager.Get("FPVISIONVAL_RESPONSE_VALIDATEFAIL") as string; } }
        public static string FPVISIONVAL_RESPONSE_INTERNALSERVERERROR { get { return CacheManager.Get("FPVISIONVAL_RESPONSE_INTERNALSERVERERROR") as string; } }
        public static string FPVISIONVAL_RESPONSE_UPDATESCANNO { get { return CacheManager.Get("FPVISIONVAL_RESPONSE_UPDATESCANNO") as string; } }
        public static string FPVISIONVAL_NA_MESSAGE { get { return CacheManager.Get("FPVISIONVAL_NA_MESSAGE") as string; } }
        public static string FPVISIONVAL_DEFAULT_APICALL_ERRORMESSAGE { get { return CacheManager.Get("FPVISIONVAL_DEFAULT_APICALL_ERRORMESSAGE") as string; } }
        public static string QC_SCAN_IN_NOT_QCTYPE { get { return CacheManager.Get("QC_SCAN_IN_NOT_QCTYPE") as string; } }

        public static string EMAILFOOTER { get { return CacheManager.Get("EMAILFOOTER") as string; } }
        public static string LotVerificationSubject { get { return "Inner Box Barcode Verification Fail!{0}" as string; } }

        //KahHeng 31Jan2019
        public static string VALIDATELABEL_HSB_CUSTPODOCUMENTDATE { get { return CacheManager.Get("HSB_CustPODocumentDate") as string; } }
        //KahHeng end

        // Final Packing ends

        // Hotbox Reason
        public static string HOTBOX_FAILMSG { get { return CacheManager.Get("HOTBOX_FAILMSG") as string; } }
        public static string HOTBOX_PENDINGRESULT { get { return CacheManager.Get("HOTBOX_PENDINGRESULT") as string; } }
        public static string HOTBOX_RESULTEXPIRED { get { return CacheManager.Get("HOTBOX_RESULTEXPIRED") as string; } }
        public static string HOTBOX_OLDBATCH { get { return CacheManager.Get("HOTBOX_OLDBATCH") as string; } }

        // end Hotbox Reason

        /// <summary>
        /// ERROR MESSAGE WHILE SAVING  CHANGE BATCHCARD
        /// </summary>
        public static string CHANGEBATCHCARDERROR { get { return CacheManager.Get("CHANGEBATCHCARDERROR") as string; } }
        /// <summary>
        /// GENERAL EXCEPTION WHILE BATCH DATA RETRIVAL
        /// </summary>
        public static string GENERALEXCEPTION { get { return CacheManager.Get("GENERALEXCEPTION") as string; } }
        /// <summary>
        /// GENERAL EXCEPTION FO TMPPACK
        /// </summary>
        public static string GENERALEXCEPTIONFORTMPPACK { get { return CacheManager.Get("GENERALEXCEPTIONFORTMPPACK") as string; } }
        /// <summary>
        /// GENERAL EXCEPTION ON CASE VALIDATION
        /// </summary>
        public static string GENERALEXCEPTIONCASEVALIDATION { get { return CacheManager.Get("GENERALEXCEPTIONCASEVALIDATION") as string; } }
        /// <summary>
        /// GENERAL EXCEPTION WHILE SAVIN REPRINT OUTER
        /// </summary>
        public static string GENERALEXCEPTIONREPRINTOUTER { get { return CacheManager.Get("GENERALEXCEPTIONREPRINTOUTER") as string; } }
        /// <summary>
        /// GENERAL EXCEPTION WHILE SAVIN REPRINT INNER
        /// </summary>
        public static string GENERALEXCEPTIONREPRINTINNER { get { return CacheManager.Get("GENERALEXCEPTIONREPRINTINNER") as string; } }
        /// <summary>
        /// INTERNAL LOT NUMBER IS REQUIRED
        /// </summary>
        public static string REQINTERNALLOTNUMBER { get { return CacheManager.Get("REQINTERNALLOTNUMBER") as string; } }
        /// <summary>
        /// SPP SERIAL NUMBER IS REQUIRED
        /// </summary>
        public static string REQISPPSERIALNO { get { return CacheManager.Get("REQISPPSERIALNO") as string; } }
        /// <summary>
        /// BATCH ORDER IS REQUIRED
        /// </summary>
        public static string REQBATCHORDER { get { return CacheManager.Get("REQBATCHORDER") as string; } }
        /// <summary>
        /// No Of Copy IS REQUIRED
        /// </summary>
        public static string REQCOPYNO { get { return CacheManager.Get("REQCOPYNO") as string; } }

        public static string REQLABELCOUNT { get { return CacheManager.Get("REQLABELCOUNT") as string; } }
        /// <summary>

        /// <summary>
        /// gROUP iD IS REQUIRED
        /// </summary>
        public static string REQGROUPID { get { return CacheManager.Get("REQGROUPID") as string; } }
        /// <summary>
        /// SERIAL NUMBER IS REQUIRED
        /// </summary>
        public static string REQSERIALNUMBER { get { return CacheManager.Get("REQSERIALNUMBER") as string; } }

        public static string REQSERIALNUMBER1 { get { return CacheManager.Get("REQSERIALNUMBER1") as string; } }

        public static string REQSERIALNUMBER2 { get { return CacheManager.Get("REQSERIALNUMBER2") as string; } }

        /// <summary>
        /// NEW SERIAL NUMBER IS REQUIRED
        /// </summary>
        public static string REQNEWSERIALNUMBER { get { return CacheManager.Get("REQNEWSERIALNUMBER") as string; } }
        /// <summary>
        /// PONUMBER IS REQUIRED
        /// </summary>
        public static string REQPONUMBER { get { return CacheManager.Get("REQPONUMBER") as string; } }
        /// <summary>
        /// Plant is required
        /// </summary>
        public static string REQPLANT { get { return CacheManager.Get("REQPLANT") as string; } }
        /// <summary>
        /// ITEM NUMBER IS REQUIRED
        /// </summary>
        public static string REQITEMNUMBER { get { return CacheManager.Get("REQITEMNUMBER") as string; } }

        public static string REQID { get { return CacheManager.Get("REQID") as string; } }
        public static string REQITEM { get { return CacheManager.Get("REQITEM") as string; } }
        public static string REQITEMSIZE { get { return CacheManager.Get("REQITEMSIZE") as string; } }

        public static string QA_TESTRESULT { get { return CacheManager.Get("QA_TESTRESULT") as string; } }
        public static string QI_TESTRESULT { get { return CacheManager.Get("QI_TESTRESULT") as string; } }

        public static string INVALIDPO { get { return CacheManager.Get("INVALIDPO") as string; } }





        /// <summary>
        /// SIZE IS REQUIRED
        /// </summary>
        public static string REQSIZE { get { return CacheManager.Get("REQSIZE") as string; } }
        public static string REQUIRESIZE { get { return CacheManager.Get("REQUIRESIZE") as string; } }

        public static string REQUIRESNO { get { return CacheManager.Get("REQUIRESNO") as string; } }

        public static string SEL_PRESHIP_PALLID { get { return CacheManager.Get("SEL_PRESHIP_PALLID") as string; } }

        public static string SEL_PALLID { get { return CacheManager.Get("SEL_PALLID") as string; } }

        /// <summary>
        /// START CASE IS REQUIRED
        /// </summary>
        public static string REQSTARTCASE { get { return CacheManager.Get("REQSTARTCASE") as string; } }
        /// <summary>
        /// END CASE IS REQUIRED
        /// </summary>
        public static string REQENDCASE { get { return CacheManager.Get("REQENDCASE") as string; } }
        /// <summary>
        /// BOXES ENTERED
        /// </summary>
        public static string REQBOXESPACKED { get { return CacheManager.Get("REQBOXESPACKED") as string; } }
        /// <summary>
        /// PALLET REQUIRED
        /// </summary>
        public static string REQPALLETREQUIRED { get { return CacheManager.Get("REQPALLETREQUIRED") as string; } }
        /// <summary>
        /// PRESHIPMENT PALLET REQUIRED
        /// </summary>
        public static string REQPRESHIPMENTPALLETREQUIRED { get { return CacheManager.Get("REQPRESHIPMENTPALLETREQUIRED") as string; } }
        /// <summary>
        /// TMPPACK REASON
        /// </summary>
        public static string REQTMPPACKREASON { get { return CacheManager.Get("REQTMPPACKREASON") as string; } }
        /// <summary>
        /// TMPPACK PCS 
        /// </summary>
        public static string REQTMPPCS { get { return CacheManager.Get("REQTMPPCS") as string; } }
        /// <summary>
        /// TMPPACK PCS 
        /// </summary>
        public static string REQCOMPPALLETLIST { get { return CacheManager.Get("REQCOMPPALLETLIST") as string; } }
        /// <summary>
        /// FINAL PACKING SAVE AND PRINT ERROR MESSAGE
        /// </summary>
        public static string FPSAVEANDPRINT { get { return CacheManager.Get("FPSAVEANDPRINT") as string; } }
        /// <summary>
        /// FINAL PACKING DATA INSERTION ERROR
        /// </summary>
        public static string FPERROR { get { return CacheManager.Get("FPERROR") as string; } }
        /// <summary>
        /// POREOPEN MESSAGE
        /// </summary>
        public static string POREOPEN { get { return CacheManager.Get("POREOPEN") as string; } }
        /// <summary>
        /// START CASE VALIDATION 
        /// </summary>
        public static string CASEVALIDATION { get { return CacheManager.Get("CASEVALIDATION") as string; } }
        /// <summary>
        /// START CASE VALIDATION
        /// </summary>
        public static string STARTCASEVALIDATION { get { return CacheManager.Get("STARTCASEVALIDATION") as string; } }
        /// <summary>
        /// END CASE VALIDATION
        /// </summary>
        public static string ENDCASEVALIDATION { get { return CacheManager.Get("ENDCASEVALIDATION") as string; } }
        /// <summary>
        /// No Of Copy VALIDATION
        /// </summary>
        public static string INTERNALLOTNUMBERVALIDATION { get { return CacheManager.Get("INTERNALLOTNUMBERVALIDATION") as string; } }
        /// <summary>
        /// No Of Copy VALIDATION
        /// </summary>
        public static string NOCOPYVALIDATION { get { return CacheManager.Get("NOCOPYVALIDATION") as string; } }
        /// <summary>
        /// Pallet Closure
        /// </summary>
        public static string PALLETCLOSURE { get { return CacheManager.Get("PALLETCLOSURE") as string; } }
        /// <summary>
        /// ASSOCIATED PALLET ID
        /// </summary>
        public static string ASSOCIATEDPALLETID { get { return CacheManager.Get("ASSOCIATEDPALLETID") as string; } }
        /// <summary>
        /// ASSOCIATED PRESHIPMENT PALLET ID
        /// </summary>
        public static string ASSOCIATEDPRESHIPMENTPALLETID { get { return CacheManager.Get("ASSOCIATEDPRESHIPMENTPALLETID") as string; } }
        /// <summary>
        /// PALLET CAPACITY REACHED
        /// </summary>
        public static string PALLETCAPACITYREACHED { get { return CacheManager.Get("PALLETCAPACITYREACHED") as string; } }
        /// <summary>
        /// 
        /// </summary>
        public static string FP_PALLETALREADYUSED { get { return CacheManager.Get("PALLETALREADYUSED") as string; } }
        public static string TMPBATCHPCS { get { return CacheManager.Get("TMPBATCHPCS") as string; } }

        /// <summary>
        /// TO ENTER ONLY REQUIRED BOXES FOR ORDER
        /// </summary>
        public static string REQUIREDBOXES { get { return CacheManager.Get("REQUIREDBOXES") as string; } }
        /// <summary>
        /// TO ENTER AS PER MULTIPLE OF INNERBOXES
        /// </summary>
        public static string INVALID_TOTAL_INNER_BOX_QUANTITY { get { return CacheManager.Get("INVALID_TOTAL_INNER_BOX_QUANTITY") as string; } }
        // <summary>
        /// SCAN MORE THAN ONE SERIAL IN "SCAN MULTIPLE BATCH"
        /// </summary>
        public static string SCAN_MORE_THAN_ONE_SERIAL { get { return CacheManager.Get("SCAN_MORE_THAN_ONE_SERIAL") as string; } }
        /// <summary>
        /// TO ENTER AS PER BATCH CAPACITY 
        /// </summary>
        public static string BATCHCAPACITY { get { return CacheManager.Get("BATCHCAPACITY") as string; } }

        /// <summary>
        /// DATE FORMAT FRO INTERNALLOTNUMBER
        /// </summary>
        public static string INTERNALLOTNUMBER_DATEFORMAT { get { return CacheManager.Get("INTERNALLOTNUMBER_DATEFORMAT") as string; } }
        /// <summary>
        /// PO Item details Exception Message.
        /// </summary>
        public static string INSERT_PURCHASEORDER_DETAILS { get { return CacheManager.Get("INSERT_PURCHASEORDER_DETAILS") as string; } }
        /// <summary>
        /// PO Item Cases Exception Message.
        /// </summary>
        public static string INSERT_PURCHASEORDER_CASES { get { return CacheManager.Get("INSERT_PURCHASEORDER_CASES") as string; } }
        /// <summary>
        /// Work Station Last Running number error message.
        /// </summary>
        public static string WORKSTATION_LASTRUNNINGNUMBER { get { return CacheManager.Get("WORKSTATION_LASTRUNNINGNUMBER") as string; } }
        /// <summary>
        /// Preshipment case number generation exception message.
        /// </summary>
        public static string PRESHIPMENT_CASENUMBERS { get { return CacheManager.Get("PRESHIPMENT_CASENUMBERS") as string; } }
        public static string PALLET_EXCEPTION { get { return CacheManager.Get("PALLET_EXCEPTION") as string; } }
        public static string PRESHIPMENTPALLET_EXCEPTION { get { return CacheManager.Get("PRESHIPMENTPALLET_EXCEPTION") as string; } }



        public static string TYPE_SIZE_NOTMATCH { get { return CacheManager.Get("TYPE_SIZE_NOTMATCH") as string; } }
        public static string SIZE_NOTMATCH { get { return CacheManager.Get("SIZE_NOTMATCH") as string; } }
        public static string TYPE_NOTMATCH { get { return CacheManager.Get("TYPE_NOTMATCH") as string; } }

        //pang 09/02/202 First Carton Packing Date
        public static string FIRSTMANUFACTURINGDATE_GET { get { return CacheManager.Get("FIRSTMANUFACTURINGDATE_GET") as string; } }
        public static string FIRSTMANUFACTURINGDATE_UPDATE { get { return CacheManager.Get("FIRSTMANUFACTURINGDATE_UPDATE") as string; } }



        // Final Packing ends

        // Production defect messages start
        /// <summary>
        /// no records found message for left grid
        /// </summary>
        public static string NORECORDS_AVAILABLE_LEFT_GRID { get { return CacheManager.Get("NORECORDS_AVAILABLE_LEFT_GRID") as string; } }
        public static string PLEASE_SELECT_A_RECORD_FROM_SUMMARY { get { return CacheManager.Get("PLEASE_SELECT_A_RECORD_FROM_SUMMARY") as string; } }
        public static string PLEASE_SELECT_A_RECORD_FROM_DETAIL { get { return CacheManager.Get("PLEASE_SELECT_A_RECORD_FROM_DETAIL") as string; } }
        public static string PNDEFECTCOUNT_GREATERTHAN_QAIDEFECTCOUNT { get { return CacheManager.Get("PNDEFECTCOUNT_GREATERTHAN_QAIDEFECTCOUNT") as string; } }
        public static string PRODUCTIONLINE_NOT_SELECTED { get { return CacheManager.Get("PRODUCTIONLINE_NOT_SELECTED") as string; } }
        public static string DATE_GREATERTHAN_TODAY { get { return CacheManager.Get("DATE_GREATERTHAN_TODAY") as string; } }
        public static string DEFECT_NOT_SELECTED { get { return CacheManager.Get("DEFECT_NOT_SELECTED") as string; } }
        public static string DEFECT_QUANTITY_ZERO { get { return CacheManager.Get("DEFECT_QUANTITY_ZERO") as string; } }
        public static string TIERSIDE_NOT_SELECTED { get { return CacheManager.Get("TIERSIDE_NOT_SELECTED") as string; } }
        public static string NODEFECTDETAIL_ALREADY_EXIST { get { return CacheManager.Get("NODEFECTDETAIL_ALREADY_EXIST") as string; } }
        public static string EX_GENERIC_EXCEPTION_GET_DEFECT_SUMMARY_LIST { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_GET_DEFECT_SUMMARY_LIST") as string; } }
        public static string EX_GENERIC_EXCEPTION_DEFECT_DETAIL_LIST_FOR_SERIALNUMBER { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_DEFECT_DETAIL_LIST_FOR_SERIALNUMBER") as string; } }
        public static string EX_GENERIC_EXCEPTION_LINELIST_FOR_LOCATION { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_LINELIST_FOR_LOCATION") as string; } }
        public static string EX_GENERIC_EXCEPTION_DEFECT_TYPE_LIST { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_DEFECT_TYPE_LIST") as string; } }
        public static string EX_GENERIC_EXCEPTION_TIER_SIDE { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_TIER_SIDE") as string; } }
        //Production defect messages end

        public static string GETLINEACTIVITIESEXCEPTION { get { return CacheManager.Get("GETLINEACTIVITIESEXCEPTION") as string; } }
        public static string ENTER_DETAILS_ACTIVITIES { get { return CacheManager.Get("ENTER_DETAILS_ACTIVITIES") as string; } }
        public static string INVALID_GLOVE_TYPE { get { return CacheManager.Get("INVALID_GLOVE_TYPE") as string; } }
        public static string SECONDNDGRADEFILEVERFICATION { get { return CacheManager.Get("SECONDNDGRADEFILEVERFICATION") as string; } }
        public static string EMPTY_GLOVE_TYPE { get { return CacheManager.Get("EMPTY_GLOVE_TYPE") as string; } }
        public static string INVALID_NO_OF_LABEL { get { return CacheManager.Get("INVALID_NO_OF_LABEL") as string; } }
        public static string NO_OF_LABEL_EXCEEDING { get { return CacheManager.Get("NO_OF_LABEL_EXCEEDING") as string; } }

        public static string PORTABLE_BARCODE_SCANNER_NOT_FOUND { get { return CacheManager.Get("PORTABLE_BARCODE_SCANNER_NOT_FOUND") as string; } }

        #region QC Yield & Packing
        /// <summary>
        /// Rework Prompt
        /// </summary>
        public static string REWORK_CONFIRMATION { get { return CacheManager.Get("REWORK_CONFIRMATION") as string; } }
        /// <summary>
        /// QA Test failed message
        /// </summary>
        public static string QA_TEST_FAIL { get { return CacheManager.Get("QA_TEST_FAIL") as string; } }
        /// <summary>
        /// Batch In Process
        /// </summary>
        public static string BATCH_IN_PROCESS { get { return CacheManager.Get("BATCH_IN_PROCESS") as string; } }
        /// <summary>
        /// Batch Already scanned out
        /// </summary>
        public static string BATCH_SCANNED_OUT { get { return CacheManager.Get("BATCH_SCANNED_OUT") as string; } }
        /// <summary>
        /// Batch Split
        /// </summary>
        public static string BATCH_SPLIT { get { return CacheManager.Get("BATCH_SPLIT") as string; } }
        /// <summary>
        /// Batch Split 
        /// </summary>
        public static string BATCH_MAX_IS_300 { get { return CacheManager.Get("BATCH_MAX_IS_300") as string; } }
        public static string BATCH_MIN_IS_300 { get { return CacheManager.Get("BATCH_MIN_IS_300") as string; } }
        public static string CONFIRM_NO_REJECT_2G { get { return CacheManager.Get("CONFIRM_NO_REJECT_2G") as string; } }
        /// <summary>
        /// Incorrect Total Pcs
        /// </summary>
        public static string INCORRECT_TOTAL_PCS { get { return CacheManager.Get("INCORRECT_TOTAL_PCS") as string; } }
        /// <summary>
        /// Incorrect Batch Weight
        /// </summary>
        public static string INCORRECT_BATCH_WEIGHT { get { return CacheManager.Get("INCORRECT_BATCH_WEIGHT") as string; } }
        /// <summary>
        /// Incorrect Total Pcs
        /// </summary>
        public static string ACTUAL_WEIGHT_EXCEED { get { return CacheManager.Get("ACTUAL_WEIGHT_EXCEED") as string; } }
        /// <summary>
        /// Incorrect Total Pcs
        /// </summary>
        public static string INCORRECT_SCANOUT_PCS { get { return CacheManager.Get("INCORRECT_SCANOUT_PCS") as string; } }
        /// <summary>
        /// Incorrect Total Pcs(Scan Batch by pieces
        /// </summary>
        public static string INCORRECT_TOTAL_PCS_SCAN_By_PCS { get { return CacheManager.Get("INCORRECT_TOTAL_PCS_SCAN_By_PCS") as string; } }

        public static string INCORRECT_SCANOUT_NEWMODULE_PCS { get { return CacheManager.Get("INCORRECT_SCANOUT_NEWMODULE_PCS") as string; } }
        /// <summary>
        /// MINIMUM Member Count
        /// </summary>
        public static string MINIMUM_MEMBER_COUNT { get { return CacheManager.Get("MINIMUM_MEMBER_COUNT") as string; } }
        /// <summary>
        /// Select Packing Group Message
        /// </summary>
        public static string SELECT_PACKING_GROUP { get { return CacheManager.Get("SELECT_PACKING_GROUP") as string; } }
        /// <summary>
        /// Select QC & Packing Group Message
        /// </summary>
        public static string SELECT_QCPACKING_GROUP { get { return CacheManager.Get("SELECT_QCPACKING_GROUP") as string; } }
        /// <summary>
        /// Serial Number not scanned in 
        /// </summary>
        public static string SERIAL_NUMBER_NOT_SCANNEDIN { get { return CacheManager.Get("SERIAL_NUMBER_NOT_SCANNEDIN") as string; } }
        /// <summary>
        /// Workstation In Use
        /// </summary>
        public static string WORKSTATION_IN_USE { get { return CacheManager.Get("WORKSTATION_IN_USE") as string; } }
        /// <summary>
        /// QC Group In Use
        /// </summary>
        public static string QCGROUP_IN_USE { get { return CacheManager.Get("QCGROUP_IN_USE") as string; } }

        /// <summary>
        /// PT_INCOMPLETE
        /// </summary>
        public static string PT_INCOMPLETE { get { return CacheManager.Get("PT_INCOMPLETE") as string; } }
        /// <summary>
        /// PTQI_INCOMPLETE
        /// </summary>
        public static string PTQI_INCOMPLETE { get { return CacheManager.Get("PTQI_INCOMPLETE") as string; } }
        /// <summary>
        /// PTQI_COMPLETED
        /// </summary>
        public static string PTQI_COMPLETED { get { return CacheManager.Get("PTQI_COMPLETED") as string; } }
        /// <summary>
        /// QCQI_INCOMPLETE
        /// </summary>
        public static string QCQI_INCOMPLETE { get { return CacheManager.Get("QCQI_INCOMPLETE") as string; } }
        /// <summary>
        /// PTQI_QCTYPE_SP
        /// </summary>
        public static string PTQI_QCTYPE_SP { get { return CacheManager.Get("PTQI_QCTYPE_SP") as string; } }
        /// <summary>
        /// QCQI Remaining TotalPcs = 0
        /// </summary>
        public static string TOTAL_PCS_IS_ZERO { get { return CacheManager.Get("TOTAL_PCS_IS_ZERO") as string; } }
        /// <summary>
        /// QCQI Remaining TotalPcs = 0
        /// </summary>
        public static string NO_RWK_AFTER_SOBC_FOR_SECOND_SOBC { get { return CacheManager.Get("NO_RWK_AFTER_SOBC_FOR_SECOND_SOBC") as string; } }

        /// <summary>
        /// QCEFFICIENCY_INCORRECT_TOTAL_PCS
        /// Created by - Loo Kah Heng
        /// 21June2019
        /// </summary>
        public static string QCEFFICIENCY_INCORRECT_TOTAL_PCS { get { return CacheManager.Get("QCEFFICIENCY_INCORRECT_TOTAL_PCS") as string; } }

        /// <summary>
        /// Empty Reason
        /// Created by - Loo Kah Heng
        /// 21June2019
        /// </summary>
        public static string EMPTY_REASON { get { return CacheManager.Get("EMPTY_REASON") as string; } }

        /// <summary>
        /// Empty Reason
        /// Created by - Loo Kah Heng
        /// 16 July 2019
        /// </summary>
        public static string EMPTY_BATCH_STATUS { get { return CacheManager.Get("EMPTY_BATCH_STATUS") as string; } }

        /// <summary>
        /// Invalid Batch End Time
        /// Created by - Loo Kah Heng
        /// 16 July 2019
        /// </summary>
        public static string QCEFFICIENCY_INVALID_BATCHENDTIME { get { return CacheManager.Get("QCEFFICIENCY_INVALID_BATCHENDTIME") as string; } }

        #endregion
        public static string PT_NOT_ALLOW { get { return CacheManager.Get("PT_NOT_ALLOW") as string; } }

        public static string GetQCTypeEXCEPTION { get { return CacheManager.Get("GetQCTypeEXCEPTION") as string; } }
    
        public static string SCANQITESTRESULT_QCTYPE_SP { get { return CacheManager.Get("SCANQITESTRESULT_QCTYPE_SP") as string; } }


        //Production Logging CR messages start
        public static string FORMER_NOT_ENTERED { get { return CacheManager.Get("FORMER_NOT_ENTERED") as string; } }
        public static string INVALID_VALUE_FOR_FORMER { get { return CacheManager.Get("INVALID_VALUE_FOR_FORMER") as string; } }
        public static string SPEED_NOT_ENTERED { get { return CacheManager.Get("SPEED_NOT_ENTERED") as string; } }
        public static string INVALID_VALUE_FOR_SPEED { get { return CacheManager.Get("INVALID_VALUE_FOR_SPEED") as string; } }
        public static string CYCLE_NOT_ENTERED { get { return CacheManager.Get("CYCLE_NOT_ENTERED") as string; } }
        public static string INVALID_VALUE_FOR_CYCLE { get { return CacheManager.Get("INVALID_VALUE_FOR_CYCLE") as string; } }
        public static string RECORD_NOT_SELECTED { get { return CacheManager.Get("RECORD_NOT_SELECTED") as string; } }
        public static string FROM_DATE_GREATERTHAN_TO_DATE { get { return CacheManager.Get("FROM_DATE_GREATERTHAN_TO_DATE") as string; } }
        public static string PRODUCTION_LOGGING_ACTIVITIES_NOT_POPULATED { get { return CacheManager.Get("PRODUCTION_LOGGING_ACTIVITIES_NOT_POPULATED") as string; } }
        public static string EX_GENERIC_EXCEPTION_PRODUCTIONLINEDETAILS_FOR_LOCATIONID { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_PRODUCTIONLINEDETAILS_FOR_LOCATIONID") as string; } }
        public static string EX_GENERIC_EXCEPTION_PRODUCTIONLOGGINGACTIVITY_FOR_LINE_FROMDATE_TODATE { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_PRODUCTIONLOGGINGACTIVITY_FOR_LINE_FROMDATE_TODATE") as string; } }
        public static string EX_GENERIC_EXCEPTION_LASTACTIVITY_FOR_LINE { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_LASTACTIVITY_FOR_LINE") as string; } }
        public static string EX_GENERIC_EXCEPTION_PRODUCTIONLINEDETAIL_FOR_START { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_PRODUCTIONLINEDETAIL_FOR_START") as string; } }
        public static string EX_GENERIC_EXCEPTION_PL_REASONTEXT_FOR_REASONTYPE { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_PL_REASONTEXT_FOR_REASONTYPE") as string; } }
        public static string EX_GENERIC_EXCEPTION_PRODUCTIONLINEDETAIL_FOR_STOP { get { return CacheManager.Get("EX_GENERIC_EXCEPTION_PRODUCTIONLINEDETAIL_FOR_STOP") as string; } }
        public static string NEW_ACTIVITY_DATE_LESSER_THAN_LAST_ACTIVITY_DATE { get { return CacheManager.Get("NEW_ACTIVITY_DATE_LESSER_THAN_LAST_ACTIVITY_DATE") as string; } }
        public static string NEW_ACTIVITY_DATE_GREATER_THAN_CURRENT_DATE { get { return CacheManager.Get("NEW_ACTIVITY_DATE_GREATER_THAN_CURRENT_DATE") as string; } }
        public static string NEW_ACTIVITY_DATE_LESSER_THAN_PL_TIME_CONFIGURATION { get { return CacheManager.Get("NEW_ACTIVITY_DATE_LESSER_THAN_PL_TIME_CONFIGURATION") as string; } }
        public static string SELECT_SIZE_FOR_ALL_TIERS { get { return CacheManager.Get("SELECT_SIZE_FOR_ALL_TIERS") as string; } }
        public static string SELECT_A_REASON { get { return CacheManager.Get("SELECT_A_REASON") as string; } }
        public static string PRODCUCTION_LINE_TIME_CONFIGURATION_NOT_DONE { get { return CacheManager.Get("PRODCUCTION_LINE_TIME_CONFIGURATION_NOT_DONE") as string; } }
        public static string GLOVETYPE_STOPPED_FOR_THE_LINE { get { return CacheManager.Get("GLOVETYPE_STOPPED_FOR_THE_LINE") as string; } }
        //LineSpeed 
        public static string GLOVE_SPEED_NOT_CONFIGURED { get { return CacheManager.Get("GLOVE_SPEED_NOT_CONFIGURED") as string; } }
        public static string GLOVE_MIX_SPEED { get { return CacheManager.Get("GLOVE_MIX_SPEED") as string; } }
        //Production Logging CR messages end
        #region Date validation messages
        /// <summary>
        /// Invalid from date
        /// </summary>
        public static string INVALID_FROM_DATE { get { return CacheManager.Get("INVALID_FROM_DATE") as string; } }
        /// <summary>
        /// Invalid from date
        /// </summary>
        public static string FROM_DATE_IN_FUTURE { get { return CacheManager.Get("FROM_DATE_IN_FUTURE") as string; } }
        /// <summary>
        /// Invalid TO date
        /// </summary>
        public static string INVALID_TO_DATE { get { return CacheManager.Get("INVALID_TO_DATE") as string; } }
        /// <summary>
        /// Invalid TO date
        /// </summary>
        public static string TO_DATE_IN_FUTURE { get { return CacheManager.Get("TO_DATE_IN_FUTURE") as string; } }
        /// <summary>
        /// To date less than From date
        /// </summary>
        public static string TO_DATE_LESSTHAN_FROM_DATE { get { return CacheManager.Get("TO_DATE_LESSTHAN_FROM_DATE") as string; } }
        /// <summary>
        /// Required From Date
        /// </summary>
        public static string REQUIRED_FROM_DATE { get { return CacheManager.Get("REQUIRED_FROM_DATE") as string; } }
        /// <summary>
        /// Required To Date
        /// </summary>
        public static string REQUIRED_TO_DATE { get { return CacheManager.Get("REQUIRED_TO_DATE") as string; } }
        #endregion

        public static string QAI_SCAN_ISONLINE_USE { get { return CacheManager.Get("QAI_SCAN_ISONLINE_USE") as string; } }
        public static string QAI_SCAN_LOOSE_GLOVES { get { return CacheManager.Get("QAI_SCAN_LOOSE_GLOVES") as string; } }
        public static string QAI_SCAN_RESAMPLING_SCREEN { get { return CacheManager.Get("QAI_SCAN_RESAMPLING_SCREEN") as string; } }
        // public static string QAI_SCAN_RESAMPLING_SCREEN_RE = "Please use Resampling screen for defect capturing.";
        public static string QAI_NOT_COMPLETED { get { return CacheManager.Get("QAI_NOT_COMPLETED") as string; } }

        public static string QAI_EDIT_DEFECT { get { return CacheManager.Get("QAI_EDIT_DEFECT") as string; } }

        public static string QAI_PWT_PT_NOT_COMPLETED { get { return CacheManager.Get("QAI_PWT_PT_NOT_COMPLETED") as string; } }
        public static string QAI_SCAN { get { return CacheManager.Get("QAI_SCAN") as string; } }
        public static string QAI_SCAN_DT { get { return CacheManager.Get("QAI_SCAN_DT") as string; } }

        public static string INVALID_SERIAL_NUMBER_GIS { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_GIS") as string; } }


        /// <summary>
        /// QC Group DETAILS EXCEPTION MESSAGE
        /// </summary>
        public static string QC_GROUP_EXCEPTION { get { return CacheManager.Get("QC_GROUP_EXCEPTION") as string; } }
        /// <summary>
        /// Duplicate QC Group
        /// </summary>
        public static string DUPLICATE_QC_GROUP { get { return CacheManager.Get("DUPLICATE_QC_GROUP") as string; } }

        /// <summary>
        /// QAI Defect DETAILS EXCEPTION MESSAGE
        /// </summary>
        public static string QAI_DEFECT_EXCEPTION { get { return CacheManager.Get("QAI_DEFECT_EXCEPTION") as string; } }
        /// <summary>
        /// Duplicate Defect Category
        /// </summary>
        public static string DUPLICATE_DEFECT_CATEGORY { get { return CacheManager.Get("DUPLICATE_DEFECT_CATEGORY") as string; } }
        /// <summary>
        /// Duplicate Sequence
        /// </summary>
        public static string DUPLICATE_DEFECT_SEQUENCE { get { return CacheManager.Get("DUPLICATE_DEFECT_SEQUENCE") as string; } }
        /// <summary>
        /// Duplicate Defect 
        /// </summary>
        public static string DUPLICATE_DEFECT { get { return CacheManager.Get("DUPLICATE_DEFECT") as string; } }
        /// <summary>
        /// Duplicate Defect Code 
        /// </summary>
        public static string DUPLICATE_DEFECT_CODE { get { return CacheManager.Get("DUPLICATE_DEFECT_CODE") as string; } }
        /// <summary>
        /// Duplicate Defect 
        /// </summary>
        public static string DUPLICATE_DEFECT_POSITION { get { return CacheManager.Get("DUPLICATE_DEFECT_POSITION") as string; } }
        /// <summary>
        /// Duplicate KeyStroke 
        /// </summary>
        public static string DUPLICATE_KEYSTROKE { get { return CacheManager.Get("DUPLICATE_KEYSTROKE") as string; } }

        public static string DUPLICATE_HARTALEGACOMMONSIZE { get { return CacheManager.Get("DUPLICATE_HARTALEGACOMMONSIZE") as string; } }
        /// <summary>
        /// Valid KeyStroke 
        /// </summary>
        public static string VALID_KEYSTROKE { get { return CacheManager.Get("VALID_KEYSTROKE") as string; } }
        /// <summary>
        /// Duplicate Reason Text
        /// </summary>
        public static string DUPLICATE_REASON_TEXT { get { return CacheManager.Get("DUPLICATE_REASON_TEXT") as string; } }

        /// <summary>
        /// Duplicate Washer Program Text
        /// </summary>
        public static string DUPLICATE_WASHERPROGRAM_TEXT { get { return CacheManager.Get("DUPLICATE_WASHERPROGRAM_TEXT") as string; } }

        /// <summary>
        /// Duplicate Washer Stage Text
        /// </summary>
        public static string DUPLICATE_WASHERSTAGE_TEXT { get { return CacheManager.Get("DUPLICATE_WASHERSTAGE_TEXT") as string; } }

        /// <summary>
        /// Duplicate Dryer Process Text
        /// </summary>
        public static string DUPLICATE_DRYERPROCESS_TEXT { get { return CacheManager.Get("DUPLICATE_DRYERPROCESS_TEXT") as string; } }
        /// <summary>
        /// Washer is in use
        /// </summary>
        public static string WASHER_IN_USE { get { return CacheManager.Get("WASHER_IN_USE") as string; } }
        /// <summary>
        /// Dryer is in use
        /// </summary>
        public static string DRYER_IN_USE { get { return CacheManager.Get("DRYER_IN_USE") as string; } }
        /// <summary>
        /// Washer Batch End time update
        /// </summary>
        public static string WASHER_BATCH_UPDATE { get { return CacheManager.Get("WASHER_BATCH_UPDATE") as string; } }
        /// <summary>
        /// Validation message for future date
        /// </summary>
        public static string FUTURE_DATE { get { return CacheManager.Get("FUTURE_DATE") as string; } }
        /// <summary>
        /// Dryer Batch End time update
        /// </summary>
        public static string DRYER_BATCH_UPDATE { get { return CacheManager.Get("DRYER_BATCH_UPDATE") as string; } }
        /// <summary>
        /// Plant list Exception 
        /// </summary>
        public static string PLANT_LIST_EXCEPTION { get { return CacheManager.Get("PLANT_LIST_EXCEPTION") as string; } }
        /// <summary>
        /// Glove Type Exception 
        /// </summary>
        public static string DUPLICATE_QCTYPE_VALUES { get { return CacheManager.Get("DUPLICATE_QC_TYPE_VALUES") as string; } }
        /// <summary>
        /// Glove Type Exception 
        /// </summary>
        public static string DUPLICATE_WASHER_VALUES { get { return CacheManager.Get("DUPLICATE_WASHER_VALUES") as string; } }
        /// <summary>
        /// Glove Type Exception 
        /// </summary>
        public static string DUPLICATE_DRYER_VALUES { get { return CacheManager.Get("DUPLICATE_DRYER_VALUES") as string; } }
        /// <summary>
        /// Glove Type Exception 
        /// </summary>
        public static string GLOVE_TYPE_EXCEPTION { get { return CacheManager.Get("GLOVE_TYPE_EXCEPTION") as string; } }
        /// <summary>
        /// Production Line
        /// </summary>
        public static string PRODUCTION_LINE_EXCEPTION { get { return CacheManager.Get("PRODUCTION_LINE_EXCEPTION") as string; } }
        /// <summary>
        /// Production Line - Production Logging message
        /// </summary>
        public static string PRODUCTION_LOGGING_MESSAGE { get { return CacheManager.Get("PRODUCTION_LOGGING_MESSAGE") as string; } }


        public static string TARGET_TYPE_BATCH_SPLIT_FOUND { get { return CacheManager.Get("TARGET_TYPE_BATCH_SPLIT_FOUND") as string; } }

        #region QA System
        public static string INVALID_WEIGHT { get { return CacheManager.Get("INVALID_WEIGHT") as string; } }

        public static string INVALID_REFERENCE_NUMBER { get { return CacheManager.Get("INVALID_REFERENCE_NUMBER") as string; } }
        public static string REFERENCE_ALREADY_EXIST { get { return CacheManager.Get("REFERENCE_ALREADY_EXIST") as string; } }
        #endregion

        public static string DUPLICATE_REFERENCE_NUMBER { get { return CacheManager.Get("DUPLICATE_REFERENCE_NUMBER") as string; } }
        public static string DUPLICATE_REFERENCE_NUMBER_ALERT { get { return CacheManager.Get("DUPLICATE_REFERENCE_NUMBER_ALERT") as string; } }

        ///// <summary>
        ///// MINIMUM Member Count
        ///// </summary>
        //public static string MINIMUM_MEMBER_COUNT { get { return CacheManager.Get("MINIMUM_MEMBER_COUNT") as string; } }
        ///// <summary>
        ///// Select Packing Group Message
        ///// </summary>
        //public static string SELECT_PACKING_GROUP { get { return CacheManager.Get("SELECT_PACKING_GROUP") as string; } }
        ///// <summary>
        ///// Select QC & Packing Group Message
        ///// </summary>
        //public static string SELECT_QCPACKING_GROUP { get { return CacheManager.Get("SELECT_QCPACKING_GROUP") as string; } }
        ///// <summary>
        ///// Serial Number not scanned in 
        ///// </summary>
        //public static string SERIAL_NUMBER_NOT_SCANNEDIN { get { return CacheManager.Get("SERIAL_NUMBER_NOT_SCANNEDIN") as string; } }
        #region QYP CR
        public static string USER_ALREADY_IN_GROUP { get { return CacheManager.Get("USER_ALREADY_IN_GROUP") as string; } }
        public static string QCMEMBER_CANNOT_SWITCH_GROUP { get { return CacheManager.Get("QCMEMBER_CANNOT_SWITCH_GROUP") as string; } }
        public static string QCMEMBER_ALREADY_IN_THIS_GROUP { get { return CacheManager.Get("QCMEMBER_ALREADY_IN_THIS_GROUP") as string; } }
        public static string QCMEMBER_NOT_IN_ANY_GROUP { get { return CacheManager.Get("QCMEMBER_NOT_IN_ANY_GROUP") as string; } }
        public static string QCMEMBER_NOT_IN_THIS_GROUP { get { return CacheManager.Get("QCMEMBER_NOT_IN_THIS_GROUP") as string; } }
        public static string QCMEMBER_CANNOT_SCAN_OUT { get { return CacheManager.Get("QCMEMBER_CANNOT_SCAN_OUT") as string; } }
        // QYP CR Messages end
        #endregion
        /// <summary>
        /// TV Reports Exception
        /// </summary>
        public static string REPORTEXCEPTION { get { return CacheManager.Get("REPORTEXCEPTION") as string; } }

        /// <summary>
        /// Reason Add access denied
        /// </summary>
        public static string REASON_ADD_ACCESS_DENIED { get { return CacheManager.Get("REASON_ADD_ACCESS_DENIED") as string; } }
        /// <summary>
        /// Reason Edit access denied
        /// </summary>
        public static string REASON_EDIT_ACCESS_DENIED { get { return CacheManager.Get("REASON_EDIT_ACCESS_DENIED") as string; } }

        /// <summary>
        /// Screen access denied
        /// </summary>
        public static string SCREEN_ACCESS_DENIED { get { return CacheManager.Get("SCREEN_ACCESS_DENIED") as string; } }

        public static string LEAVE_SECURITY_PAGE { get { return CacheManager.Get("LEAVE_SECURITY_PAGE") as string; } }

        /// <summary>
        /// AX Integration EXCEPTION MESSAGE
        /// </summary>
        public static string AX_INTEGRATION_EXCEPTION { get { return CacheManager.Get("AX_INTEGRATION_EXCEPTION") as string; } }

        public static string AX_TIMEOUT_EXCEPTION { get { return CacheManager.Get("AX_TIMEOUT_EXCEPTION") as string; } }

        /// <summary>
        /// //#MH 09/02/2018 To cater time out issue without update Ax posting success
        /// </summary>
        public static string QAI_DataSaved_AX_Fail { get { return CacheManager.Get("QAI_DataSaved_AX_Fail") as string; } } // #Azrul 13/07/2018: Merged from Live AX6
        public static string QAI_DataSaved_AX_Fail_DetectOnFinalPack { get { return CacheManager.Get("QAI_DataSaved_AX_Fail_DetectOnFinalPack") as string; } } // #Azrul 13/07/2018: Merged from Live AX6

        public static string AX_SERVICE_UNAVAILABLE { get { return CacheManager.Get("AX_SERVICE_UNAVAILABLE") as string; } }


        public static string PERMISSION_SEQ_ALREADY_EXIST { get { return CacheManager.Get("PERMISSION_SEQ_ALREADY_EXIST") as string; } }
        public static string EMPLOYEE_ID_ALREADY_EXIST { get { return CacheManager.Get("EMPLOYEE_ID_ALREADY_EXIST") as string; } }
        public static string PASSWORD_ALREADY_EXIST { get { return CacheManager.Get("PASSWORD_ALREADY_EXIST") as string; } }
        public static string MODULE_PERMISSION_COMBINATION_ALREADY_EXIST { get { return CacheManager.Get("MODULE_PERMISSION_COMBINATION_ALREADY_EXIST") as string; } }
        public static string ROLENAME_ALREADY_EXIST { get { return CacheManager.Get("ROLENAME_ALREADY_EXIST") as string; } }
        public static string MAINE_MENU_NAME { get { return CacheManager.Get("MAINE_MENU_NAME") as string; } }
        public static string EMPLOYEE_MASTER_PAGE { get { return CacheManager.Get("EMPLOYEE_MASTER_PAGE") as string; } }
        public static string ROLE_MASTER_PAGE { get { return CacheManager.Get("ROLE_MASTER_PAGE") as string; } }
        public static string PERMISSION_MASTER_MASTER_PAGE { get { return CacheManager.Get("PERMISSION_MASTER_MASTER_PAGE") as string; } }
        public static string SCREEN_MASTER_PAGE { get { return CacheManager.Get("SCREEN_MASTER_PAGE") as string; } }
        public static string OPERATOR_SCREEN_ACCESS_MASTER { get { return CacheManager.Get("OPERATOR_SCREEN_ACCESS_MASTER") as string; } }
        public static string OPERATOR_SCREEN_ACCESS_MSG { get { return CacheManager.Get("OPERATOR_SCREEN_ACCESS_MSG") as string; } }
        public static string TESTER_SCREEN_ACCESS_MSG { get { return CacheManager.Get("TESTER_SCREEN_ACCESS_MSG") as string; } }
        public static string MEMBER_SCREEN_ACCESS_MSG { get { return CacheManager.Get("MEMBER_SCREEN_ACCESS_MSG") as string; } }
        public static string QAI_SCREEN_ACCESS_MSG { get { return CacheManager.Get("QAI_SCREEN_ACCESS_MSG") as string; } }
        public static string SERVICE_UNAVAILABLE { get { return CacheManager.Get("SERVICE_UNAVAILABLE") as string; } }
        public static string NO_PRINTER { get { return CacheManager.Get("NO_PRINTER") as string; } }
        public static string WORKSTAION_IS_NOT_SELECTED { get { return CacheManager.Get("WORKSTAION_IS_NOT_SELECTED") as string; } }
        public static string INVALID_LOT_NUMBER { get { return CacheManager.Get("INVALID_LOT_NUMBER") as string; } }

        public static string NO_BATCH_CARDS_AVAILABLE { get { return CacheManager.Get("NO_BATCH_CARDS_AVAILABLE") as string; } }
        public static string NO_ON2G_BATCH_CARDS_AVAILABLE { get { return CacheManager.Get("NO_ON2G_BATCH_CARDS_AVAILABLE") as string; } }
        public static string TUMBLING_REPRINT_HBC { get { return CacheManager.Get("TUMBLING_REPRINT_HBC") as string; } }

        public static string ENTER_SERIAL_NUMBER_OR_SELECT_LINE { get { return CacheManager.Get("ENTER_SERIAL_NUMBER_OR_SELECT_LINE") as string; } }

        public static string CANCEL_SCONFIG { get { return CacheManager.Get("CANCEL_SCONFIG") as string; } }

        #region Print
        public static string DRAWING_EXCEPTION { get { return CacheManager.Get("DRAWING_EXCEPTION") as string; } }

        public static string PRINTING_ERROR { get { return CacheManager.Get("PRINTING_ERROR") as string; } }
        #endregion
        public static string BATCH_DETAILS_SERIALNUMBER { get { return CacheManager.Get("BATCH_DETAILS_SERIALNUMBER") as string; } }
        public static string PACKING_SIZE_EXCEPTION { get { return CacheManager.Get("PACKING_SIZE_EXCEPTION") as string; } }
        public static string QAI_EDIT_REASONS_EXCEPTION { get { return CacheManager.Get("QAI_EDIT_REASONS_EXCEPTION") as string; } }
        public static string QAI_DEFECTLIST_EXCEPTION { get { return CacheManager.Get("QAI_DEFECTLIST_EXCEPTION") as string; } }
        public static string DEFECT_DETAILS_EXCEPTION { get { return CacheManager.Get("DEFECT_DETAILS_EXCEPTION") as string; } }
        public static string SIZE_DESC_EXCEPTION { get { return CacheManager.Get("SIZE_DESC_EXCEPTION") as string; } }


        #region ProgramClass
        public static string APPLICATION_ALREADY_RUNNING { get { return CacheManager.Get("APPLICATION_ALREADY_RUNNING") as string; } }
        public static string MAIN_PROGRAM { get { return CacheManager.Get("MAIN_PROGRAM") as string; } }
        public static string MAIN { get { return CacheManager.Get("MAIN") as string; } }
        public static string DATABASE_NETWORK_ISSUE { get { return CacheManager.Get("DATABASE_NETWORK_ISSUE") as string; } }
        public static string MACHINE_CONFIGURATION_ERROR { get { return CacheManager.Get("MACHINE_CONFIGURATION_ERROR") as string; } }
        public static string SYSTEMERROR { get { return CacheManager.Get("SYSTEMERROR") as string; } }
        #endregion

        #region FloorDBAccess
        public static string INDIVIDUAL_OPERATION_EXCEPTION { get { return CacheManager.Get("INDIVIDUAL_OPERATION_EXCEPTION") as string; } }
        public static string ARGUMENT_EXCEPTION_OCCURED { get { return CacheManager.Get("ARGUMENT_EXCEPTION_OCCURED") as string; } }
        public static string INVALID_CAST_EXPECTION { get { return CacheManager.Get("INVALID_CAST_EXPECTION") as string; } }
        public static string UNKNOWN_EXCEPTION_THROWN { get { return CacheManager.Get("UNKNOWN_EXCEPTION_THROWN") as string; } }
        public static string FLOOR_DBACCESS_ERROR { get { return CacheManager.Get("FLOOR_DBACCESS_ERROR") as string; } }
        public static string ROW_LOCKED { get { return CacheManager.Get("ROW_LOCKED") as string; } }
        public static string UN_KNOWN_EXCEPTION { get { return CacheManager.Get("UN_KNOWN_EXCEPTION") as string; } }
        public static string FLOOR_DB_ACCESS_ERROR { get { return CacheManager.Get("FLOOR_DB_ACCESS_ERROR") as string; } }
        public static string FLOOR_DB_ERROR { get { return CacheManager.Get("FLOOR_DB_ERROR") as string; } }
        public static string FLOOR_DB_EXECUTION_ERROR { get { return CacheManager.Get("FLOOR_DB_EXECUTION_ERROR") as string; } }
        public static string APP_SETTING_KEY_NOT_FOUND { get { return CacheManager.Get("APP_SETTING_KEY_NOT_FOUND") as string; } }
        public static string CONNECTION_STRING_KEY_NOT_FOUND { get { return CacheManager.Get("CONNECTION_STRING_KEY_NOT_FOUND") as string; } }
        #endregion


        #region WSConfig

        public static string CANCEL_WS { get { return CacheManager.Get("CANCEL_WS") as string; } }
        public static string EX_WS { get { return CacheManager.Get("EX_WS") as string; } }
        public static string NOCONF_WS { get { return CacheManager.Get("NOCONF_WS") as string; } }
        public static string EMPTYCONF_WS { get { return CacheManager.Get("EMPTYCONF_WS") as string; } }
        public static string FAILSAVE_WS { get { return CacheManager.Get("FAILSAVE_WS") as string; } }
        public static string CONFNAME_WS { get { return CacheManager.Get("CONFNAME_WS") as string; } }
        public static string SELMOD_WS { get { return CacheManager.Get("SELMOD_WS") as string; } }
        public static string NOWSCONF_WS { get { return CacheManager.Get("NOWSCONF_WS") as string; } }
        public static string NOKEY_WS { get { return CacheManager.Get("NOKEY_WS") as string; } }
        public static string FAILEDSAVE_CONF_WS { get { return CacheManager.Get("FAILEDSAVE_CONF_WS") as string; } }
        public static string NOWS_SAVE_WS { get { return CacheManager.Get("NOWS_SAVE_WS") as string; } }
        public static string LOC_EXISTS_WS { get { return CacheManager.Get("LOC_EXISTS_WS") as string; } }
        public static string WSSAVE_SUCCESS_WS { get { return CacheManager.Get("WSSAVE_SUCCESS_WS") as string; } }
        public static string UPDATE_LOC_FAIL_WS { get { return CacheManager.Get("UPDATE_LOC_FAIL_WS") as string; } }
        public static string NEWCONF_SAVE_WS { get { return CacheManager.Get("NEWCONF_SAVE_WS") as string; } }
        public static string UPDATE_EXSTCONF_WS { get { return CacheManager.Get("UPDATE_EXSTCONF_WS") as string; } }

        #endregion

        /// <summary>
        /// Production Reports Invalid URL
        /// </summary>
        public static string INVALID_URL { get { return CacheManager.Get("INVALID_URL") as string; } }


        public static string DATAREQUIRED { get { return CacheManager.Get("DATAREQUIRED") as string; } }
        public static string CONFIRM { get { return CacheManager.Get("CONFIRM") as string; } }
        public static string INVALID_DATA { get { return CacheManager.Get("INVALID_DATA") as string; } }


        public static string ten_Pcs_Weight_outof_range { get { return CacheManager.Get("ten_Pcs_Weight_outof_range") as string; } }

        public static string QUANTITY_NULL_PRODUCTION_DEFECT { get { return CacheManager.Get("QUANTITY_NULL_PRODUCTION_DEFECT") as string; } }


        public static string WSLOCCHANGE_CONFRIM { get { return CacheManager.Get("WSLOCCHANGE_CONFRIM") as string; } }

        public static string GETDECRYPTEDSTRINGMETHODEXCEPTION { get { return CacheManager.Get("GETDECRYPTEDSTRINGMETHODEXCEPTION") as string; } }

        public static string GETENCRYPTEDSTRINGMETHODEXCEPTION { get { return CacheManager.Get("GETENCRYPTEDSTRINGMETHODEXCEPTION") as string; } }

        public static string SAVEORUPDATEAPPCONFIGMETHODEXCEPTION { get { return CacheManager.Get("SAVEORUPDATEAPPCONFIGMETHODEXCEPTION") as string; } }

        public static string CONNECTIONSTRING_ALREADY_ENCRYPTED { get { return CacheManager.Get("CONNECTIONSTRING_ALREADY_ENCRYPTED") as string; } }

        public static string CONFIRM_CONNECTIONSTRING { get { return CacheManager.Get("CONFIRM_CONNECTIONSTRING") as string; } }
        public static string DEPLOYED_CONNECTIONSTRING { get { return CacheManager.Get("DEPLOYED_CONNECTIONSTRING") as string; } }

        public static string DATA_SAVED_SUCCESSFULLY_CONNECTIONSTRING { get { return CacheManager.Get("DATA_SAVED_SUCCESSFULLY_CONNECTIONSTRING") as string; } }

        #region Master Table Maintenance

        public static string DELETE_MESSAGE { get { return CacheManager.Get("DELETE_MESSAGE") as string; } }

        /// <summary>
        /// DELETE_WORKSTATION
        /// </summary>
        public static string DELETE_WORKSTATION { get { return CacheManager.Get("DELETE_WORKSTATION") as string; } }
        /// <summary>
        /// DUPLICATE_WORKSTATION
        /// </summary>
        public static string DUPLICATE_WORKSTATION { get { return CacheManager.Get("DUPLICATE_WORKSTATION") as string; } }

        /// <summary>
        /// DUPLICATE QAIAQREFERENCEFIRST
        /// </summary>
        public static string DUPLICATE_QAIAQREFERENCEFIRST { get { return CacheManager.Get("DUPLICATE_QAIAQREFERENCEFIRST") as string; } }

        /// <summary>
        /// DUPLICATE PRODUCTIONDEFECTMASTER
        /// </summary>
        public static string DUPLICATE_PRODUCTIONDEFECTMASTER { get { return CacheManager.Get("DUPLICATE_PRODUCTIONDEFECTMASTER") as string; } }

        /// <summary>
        /// DUPLICATE QITESTRESULTAQL
        /// </summary>
        public static string DUPLICATE_QITESTRESULTAQL { get { return CacheManager.Get("DUPLICATE_QITESTRESULTAQL") as string; } }

        /// <summary>
        /// Line Master Duplicate
        /// </summary>
        public static string DUPLICATE_LINEMASTER { get { return CacheManager.Get("DUPLICATE_LINEMASTER") as string; } }


        public static string DUPLICATE_QCTYPEMASTER { get { return CacheManager.Get("DUPLICATE_QCTYPEMASTER") as string; } }

        public static string DUPLICATE_BATCHTYPEMASTER { get { return CacheManager.Get("DUPLICATE_BATCHTYPEMASTER") as string; } }
        public static string DUPLICATE_GLOVESIZEMASTER { get { return CacheManager.Get("DUPLICATE_GLOVESIZEMASTER") as string; } }
        public static string DUPLICATE_LOCATIONMASTER { get { return CacheManager.Get("DUPLICATE_LOCATIONMASTER") as string; } }
        public static string DUPLICATE_SHIFTMASTER { get { return CacheManager.Get("DUPLICATE_SHIFTMASTER") as string; } }
        public static string DUPLICATE_SHIFTMASTER_TIMEOVERLAP { get { return CacheManager.Get("DUPLICATE_SHIFTMASTER_TIMEOVERLAP") as string; } }
        public static string DUPLICATE_GLOVETYPEMASTER_GloveCode { get { return CacheManager.Get("DUPLICATE_GLOVETYPEMASTER_GLOVECODE") as string; } }
        public static string DUPLICATE_GLOVETYPEMASTER_BarCode { get { return CacheManager.Get("DUPLICATE_GLOVETYPEMASTER_BARCODE") as string; } }
        public static string DUPLICATE_GLOVECATEGORYMASTER { get { return CacheManager.Get("DUPLICATE_GLOVECATEGORYMASTER") as string; } }
        public static string DUPLICATE_INNERLABELSETMASTER { get { return CacheManager.Get("DUPLICATE_INNERLABELSETMASTER") as string; } }
        public static string DUPLICATE_OUTERLABELSETMASTER { get { return CacheManager.Get("DUPLICATE_OUTERLABELSETMASTER") as string; } }
        public static string DUPLICATE_GLOVETYPECOMMONSIZEMASTER { get { return CacheManager.Get("DUPLICATE_GLOVETYPECOMMONSIZEMASTER") as string; } }
        public static string DUPLICATE_PRESHIPMENTSAMPLINGPLANMASTER { get { return CacheManager.Get("DUPLICATE_PRESHIPMENTSAMPLINGPLANMASTER") as string; } }

        /// <summary>
        /// DUPLICATE_MESSAGE
        /// </summary>
        public static string DUPLICATE_MESSAGE { get { return CacheManager.Get("DUPLICATE_MESSAGE") as string; } }
        /// <summary>
        /// DELETE_ACTIVITYTYPE
        /// </summary>
        public static string DELETE_ACTIVITYTYPE { get { return CacheManager.Get("DELETE_ACTIVITYTYPE") as string; } }
        /// <summary>
        /// DUPLICATE_ACTIVITYTYPE
        /// </summary>
        public static string DUPLICATE_ACTIVITYTYPE { get { return CacheManager.Get("DUPLICATE_ACTIVITYTYPE") as string; } }
        /// <summary>
        /// DELETE_WASHER
        /// </summary>
        public static string DELETE_WASHERPROGRAM { get { return CacheManager.Get("DELETE_WASHERPROGRAM") as string; } }
        /// <summary>
        /// DELETE_WASHER_PROGRAM
        /// </summary>
        public static string DELETE_WASHERSTAGE { get { return CacheManager.Get("DELETE_WASHERSTAGE") as string; } }
        /// <summary>
        /// DELETE_WASHER_STAGE
        /// </summary>
        public static string DELETE_DRYERPROCESS { get { return CacheManager.Get("DELETE_DRYERPROCESS") as string; } }
        /// <summary>
        /// DELETE_DRYER_PROCESS
        /// </summary>
        public static string DELETE_WASHER { get { return CacheManager.Get("DELETE_WASHER") as string; } }
        /// <summary>
        /// DUPLICATE_WASHER
        /// </summary>
        public static string DUPLICATE_WASHER { get { return CacheManager.Get("DUPLICATE_WASHER") as string; } }
        /// <summary>
        /// DELETE_DRYER
        /// </summary>
        public static string DELETE_DRYER { get { return CacheManager.Get("DELETE_DRYER") as string; } }
        /// <summary>
        /// DUPLICATE_DRYER
        /// </summary>
        public static string DUPLICATE_DRYER { get { return CacheManager.Get("DUPLICATE_DRYER") as string; } }
        /// <summary>
        /// DELETE_BIN
        /// </summary>
        public static string DELETE_BIN { get { return CacheManager.Get("DELETE_BIN") as string; } }
        /// <summary>
        /// DUPLICATE_BIN
        /// </summary>
        public static string DUPLICATE_BIN { get { return CacheManager.Get("DUPLICATE_BIN") as string; } }
        /// <summary>
        /// DELETE_RECORD
        /// </summary>
        public static string DELETE_RECORD { get { return CacheManager.Get("DELETE_RECORD") as string; } }
        /// <summary>
        /// DELETE_DEFECTIVE_GLOVE_REASON
        /// </summary>
        public static string DELETE_DEFECTIVE_GLOVE_REASON { get { return CacheManager.Get("DELETE_DEFECTIVE_GLOVE_REASON") as string; } }
        /// <summary>
        /// DUPLICATE_DEFECTIVE_GLOVE_REASON
        /// </summary>
        public static string DUPLICATE_DEFECTIVE_GLOVE_REASON { get { return CacheManager.Get("DUPLICATE_DEFECTIVE_GLOVE_REASON") as string; } }
        /// <summary>
        /// DELETE_QAIAQCOSMETICREF
        /// </summary>
        public static string DELETE_QAIAQCOSMETICREF { get { return CacheManager.Get("DELETE_QAIAQCOSMETICREF") as string; } }
        /// <summary>
        /// DUPLICATE_QAIAQCOSMETICREF
        /// </summary>
        public static string DUPLICATE_QAIAQCOSMETICREF { get { return CacheManager.Get("DUPLICATE_QAIAQCOSMETICREF") as string; } }
        /// <summary>
        /// MAJOR_DEFECT_RANGE
        /// </summary>
        public static string MAJOR_DEFECT_RANGE { get { return CacheManager.Get("MAJOR_DEFECT_RANGE") as string; } }
        /// <summary>
        /// MINOR_DEFECT_RANGE
        /// </summary>
        public static string MINOR_DEFECT_RANGE { get { return CacheManager.Get("MINOR_DEFECT_RANGE") as string; } }
        public static string DUPLICATE_PALLETID { get { return CacheManager.Get("DUPLICATE_PALLETID") as string; } }
        public static string DELETE_PALLETID { get { return CacheManager.Get("DELETE_PALLETID") as string; } }

        #endregion

        public static string NON_STRAIGHT_PACK_QAI { get { return CacheManager.Get("NON_STRAIGHT_PACK_QAI") as string; } }

        public static string NON_START_ACTIVITY { get { return CacheManager.Get("NON_START_ACTIVITY") as string; } }

        public static string CONSECUTIVE_START { get { return CacheManager.Get("CONSECUTIVE_START") as string; } }
        public static string CONSECUTIVE_STOP { get { return CacheManager.Get("CONSECUTIVE_STOP") as string; } }
        public static string ORPHAN_START { get { return CacheManager.Get("ORPHAN_START") as string; } }
        public static string ORPHAN_STOP { get { return CacheManager.Get("ORPHAN_STOP") as string; } }
        public static string DUPLICATE_PALLET { get { return CacheManager.Get("DUPLICATE_PALLET") as string; } }
        public static string PALLET_RESET_CONFIRM { get { return CacheManager.Get("PALLET_RESET_CONFIRM") as string; } }
        public static string PALLET_RESET_SUCCESS { get { return CacheManager.Get("PALLET_RESET_SUCCESS") as string; } }
        public static string PALLET_RESET_UNSUCCESS { get { return CacheManager.Get("PALLET_RESET_UNSUCCESS") as string; } }
        public static string NOT_COMPLETED_PALLET { get { return CacheManager.Get("NOT_COMPLETED_PALLET") as string; } }
        public static string CASE_LIST_FOR_PALLET { get { return CacheManager.Get("CASE_LIST_FOR_PALLET") as string; } }
        public static string LABEL_PRINTER_COMMUNICATION_ERROR { get { return CacheManager.Get("LABEL_PRINTER_COM_NOTREADY") as string; } }
        public static string INKJET_PRINTER_COMMUNICATION_ERROR { get { return CacheManager.Get("INKJET_PRINTER_COM_NOTREADY") as string; } }
        public static string VALIDATE_24HOURS { get { return CacheManager.Get("VALIDATE_24HOURS") as string; } }

        #region WIP Stock Count

        public static string SCAN_DATA_NOT_FOUND { get { return CacheManager.Get("SCAN_DATA_NOT_FOUND") as string; } }
        public static string SCAN_DATA_FILE_PATH_NOT_FOUND { get { return CacheManager.Get("SCAN_DATA_FILE_PATH_NOT_FOUND") as string; } }
        public static string SCAN_DATA_ARCHIEVE_PATH_NOT_FOUND { get { return CacheManager.Get("SCAN_DATA_ARCHIEVE_PATH_NOT_FOUND") as string; } }
        public static string SCAN_DATA_FILE_ARCHIEVE_PATH_NOT_FOUND { get { return CacheManager.Get("SCAN_DATA_FILE_ARCHIEVE_PATH_NOT_FOUND") as string; } }
        public static string SAVE_SCAN_DATA_CONFIRMATION { get { return CacheManager.Get("SAVE_SCAN_DATA_CONFIRMATION") as string; } }
        public static string DUPLICATE_WIP_REF_NO { get { return CacheManager.Get("DUPLICATE_WIP_REF_NO") as string; } }
        public static string INVALID_SCAN_DATA_TO_PROCEED { get { return CacheManager.Get("INVALID_SCAN_DATA_TO_PROCEED") as string; } }
        public static string SAVE_CUTOFF_BATCH_CONFIRMATION { get { return CacheManager.Get("SAVE_CUTOFF_BATCH_CONFIRMATION") as string; } }
        public static string DUPLICATE_WIP_CUTOFF_REF_NO { get { return CacheManager.Get("DUPLICATE_WIP_CUTOFF_REF_NO") as string; } }
        public static string OVERWRITE_CUTOFF_BATCH_CONFIRMATION { get { return CacheManager.Get("OVERWRITE_CUTOFF_BATCH_CONFIRMATION") as string; } }
        public static string SAVE_WIP_SUMMARY_CONFIRMATION { get { return CacheManager.Get("SAVE_WIP_SUMMARY_CONFIRMATION") as string; } }
        public static string CONCURRENCY_ERROR { get { return CacheManager.Get("CONCURRENCY_ERROR") as string; } }
        public static string OVERWRITE_WIP_SUMMARY_CONFIRMATION { get { return CacheManager.Get("OVERWRITE_WIP_SUMMARY_CONFIRMATION") as string; } }
        public static string DATA_FILE_NOT_FOUND { get { return CacheManager.Get("DATA_FILE_NOT_FOUND") as string; } }
        public static string DATA_FILE_FILE_PATH_NOT_FOUND { get { return CacheManager.Get("DATA_FILE_FILE_PATH_NOT_FOUND") as string; } }
        public static string DATA_FILE_ARCHIEVE_PATH_NOT_FOUND { get { return CacheManager.Get("DATA_FILE_ARCHIEVE_PATH_NOT_FOUND") as string; } }
        public static string NO_DATA_CHECKED_TO_VOID { get { return CacheManager.Get("NO_DATA_CHECKED_TO_VOID") as string; } }
        public static string VOID_WIP_SCAN_DATA_CONFIRMATION { get { return CacheManager.Get("VOID_WIP_SCAN_DATA_CONFIRMATION") as string; } }
        public static string CLEAR_WIP_SCAN_DATA_CONFIRMATION { get { return CacheManager.Get("CLEAR_WIP_SCAN_DATA_CONFIRMATION") as string; } }

        #endregion

        #region FX
        public static string EMPTY_INNER_OUTER_LABEL_SET { get { return CacheManager.Get("EMPTY_INNER_OUTER_LABEL_SET") as string; } }

        public static string TOWERLIGHT_COMMUNICATION_ERROR { get { return CacheManager.Get("TOWERLIGHT_COMMUNICATION_ERROR") as string; } }
        public static string SCANNER_COMMUNICATION_ERROR { get { return CacheManager.Get("SCANNER_COMMUNICATION_ERROR") as string; } }
        public static string TOWERLIGHT_SCANNER_COMMUNICATION_ERROR { get { return CacheManager.Get("TOWERLIGHT_SCANNER_COMMUNICATION_ERROR") as string; } }

        public static string PEHA_SARAYA_HARDWARE_INTEGRATION_FLAG_ERROR { get { return CacheManager.Get("PEHA_SARAYA_HARDWARE_INTEGRATION_FLAG_ERROR") as string; } }

        public static string PALLET_IS_FULL { get { return CacheManager.Get("PALLET_IS_FULL") as string; } }

        public static string GETSNLIST_EXCEPTION { get { return CacheManager.Get("GETSNLIST_EXCEPTION") as string; } }
        public static string GETPODETAILS_EXCEPTION { get { return CacheManager.Get("GETPODETAILS_EXCEPTION") as string; } }
        public static string DOWNGRADE_VALIDATION { get { return CacheManager.Get("DOWNGRADE_VALIDATION") as string; } }
        public static string FPEXEMPT_PRODUCTIONDATEVALIDATION_ERR { get { return CacheManager.Get("FPEXEMPT_PRODUCTIONDATEVALIDATION_ERR") as string; } }
        #endregion

        public static string INVALIDFORMAT_TIME{ get { return CacheManager.Get("INVALIDFORMAT_TIME") as string; } }
        public static string INVALID_SERIAL_NUMBER_PLANT { get { return CacheManager.Get("INVALID_SERIAL_NUMBER_PLANT") as string; } }

        public static string EMPTY_WORKORDERITEM { get { return CacheManager.Get("EMPTY_WORKORDERITEM") as string; } }
        public static string BRANDMASTER_NOT_CONFIGURED { get { return CacheManager.Get("BRANDMASTER_NOT_CONFIGURED") as string; } }

        public static string BRANDSIZE_NOT_CONFIGURED { get { return CacheManager.Get("BRANDSIZE_NOT_CONFIGURED") as string; } }

        public static string WORKORDERSUBJECT { get { return CacheManager.Get("WORKORDERSUBJECT") as string; } }
        public static string WORKORDERCONTENT { get { return CacheManager.Get("WORKORDERCONTENT") as string; } }
        public static string WARNING { get { return CacheManager.Get("WARNING") as string; } }

        public static string QI_TEST_REASON_NEW { get { return CacheManager.Get("QI_TEST_REASON_NEW") as string; } }
        public static string QI_TEST_REASON_PSIREWORK { get { return CacheManager.Get("QI_TEST_REASON_PSIREWORK") as string; } }
        public static string QI_TEST_REASON_NORMALREWORK_SP { get { return CacheManager.Get("QI_TEST_REASON_NORMALREWORK_SP") as string; } }

        #region Department

        public static string REQUIRED_DEPT { get { return CacheManager.Get("REQUIRED_DEPT") as string; } }
        public static string GETPLDEPARTMENTEXCEPTION { get { return CacheManager.Get("GETPLDEPARTMENTEXCEPTION") as string; } }

        #endregion
        
        #region Surgical Packing Plan
        public static string SPP_INVALID_LOT { get { return CacheManager.Get("SPP_INVALID_LOT") as string; } }
        public static string SPP_LOTNO_ZERO { get { return CacheManager.Get("SPP_LOTNO_ZERO") as string; } }
        public static string SPP_LOTNO_ONE { get { return CacheManager.Get("SPP_LOTNO_ONE") as string; } }
        public static string SPP_LOTNO_THREE { get { return CacheManager.Get("SPP_LOTNO_THREE") as string; } }
        public static string SPP_LOTNO_TWO { get { return CacheManager.Get("SPP_LOTNO_TWO") as string; } }
        public static string SPP_LOTNO_FOUR { get { return CacheManager.Get("SPP_LOTNO_FOUR") as string; } }
        public static string SPP_LOTNO_SIX { get { return CacheManager.Get("SPP_LOTNO_SIX") as string; } }
        #endregion
    }
}
