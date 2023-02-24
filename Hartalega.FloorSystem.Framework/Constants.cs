// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
namespace Hartalega.FloorSystem.Framework
{
    /// <summary>
    /// Cannot have instance and cannot be inherited.
    /// This class provides the enumerator type which can be used across all layers.
    /// </summary>
    public static class Constants
    {
        #region Production Defect Status
        /// <summary>
        /// Production Defect Status
        /// </summary>
        public enum ProductionDefectStatus
        {
            /// <summary>
            /// GREEN Status
            /// </summary>
            GREEN,
            /// <summary>
            /// YELLOW Status
            /// </summary>
            YELLOW,
            /// <summary>
            /// RED Status
            /// </summary>
            RED
        }
        #endregion

        #region Production Line status
        /// <summary>
        /// Production Line Activities
        /// </summary>
        public enum ProductionLineActivities
        {
            /// <summary>
            /// START Activity
            /// </summary>
            START,
            /// <summary>
            /// STOP activity
            /// </summary>
            STOP
        }
        #endregion
        /// <summary>
        /// Exception Log Priority
        /// </summary>
        #region Exception Log Priority
        public enum LogPriority
        {
            /// <summary>
            /// No Priority
            /// </summary>
            None = 0,

            /// <summary>
            /// Low Priority
            /// </summary>
            Low = 1,

            /// <summary>
            /// Medium Priority
            /// </summary>
            Medium = 2,

            /// <summary>
            /// High Priority
            /// </summary>
            High = 3,

            /// <summary>
            /// Urgent Priority
            /// </summary>
            Urgent = 4
        }
        #endregion

        /// <summary>
        /// Exception Log Category
        /// </summary>
        #region Exception Log Category
        public enum LogCategory
        {
            /// <summary>
            /// No Category
            /// </summary>
            None,

            /// <summary>
            /// Just Information
            /// </summary>
            Information,

            /// <summary>
            /// Warning Category
            /// </summary>
            Warning,

            /// <summary>
            /// Error Category
            /// </summary>
            Error
        }
        #endregion

        /// <summary>
        /// Exception Log Event Id
        /// </summary>
        #region Exception Log Event Id
        public enum LogEventId
        {
            /// <summary>
            /// No Event
            /// </summary>
            None = 0,

            /// <summary>
            /// Service Start
            /// </summary>
            ServiceStart = 1,

            /// <summary>
            /// Service Stop
            /// </summary>
            ServiceStop = 2,

            /// <summary>
            /// Event Blah
            /// </summary>
            Blah = 3,

            /// <summary>
            /// Configuration Update
            /// </summary>
            ConfigurationUpdate = 4
        }
        #endregion

        /// <summary>
        /// Validation Type
        /// </summary>
        #region Validation Type
        public enum ValidationType
        {
            /// <summary>
            /// None Type
            /// </summary>
            None,

            /// <summary>
            /// Integer type validation
            /// </summary>
            Integer,

            /// <summary>
            /// Letter type validation
            /// </summary>
            Letter,

            /// <summary>
            /// Letters and Numbers type validation
            /// </summary>
            LettersAndNumbers,

            /// <summary>
            /// Decimal type validation
            /// </summary>
            Decimal,

            /// <summary>
            /// Email validation
            /// </summary>
            Email,

            /// <summary>
            /// United State of Americas ZIP code type
            /// </summary>
            UsZipCode,

            /// <summary>
            /// Percentage validation
            /// </summary>
            Percentage,

            /// <summary>
            /// Positive Integer validation
            /// </summary>
            PositiveInteger,

            /// <summary>
            /// Credit Card validation
            /// </summary>
            CreditCard,

            /// <summary>
            /// Date validation
            /// </summary>
            Date,

            /// <summary>
            /// Date and Time validation
            /// </summary>
            DateAndTime,

            /// <summary>
            /// Site Url validation
            /// </summary>
            Url,

            /// <summary>
            /// Name validation
            /// </summary>
            Name
        }
        #endregion

        /// <summary>
        /// Message status
        /// </summary>
        #region Message status
        public enum MessageStatus
        {
            /// <summary>
            /// When failed
            /// </summary>
            Fail,

            /// <summary>
            /// When success
            /// </summary>
            Success
        }
        #endregion

        /// <summary>
        /// Shift group code
        /// </summary>
        #region ShiftGroup
        public enum ShiftGroup
        {
            /// <summary>
            /// Production group
            /// </summary>
            PN,

            /// <summary>
            /// QC group
            /// </summary>
            QC,

            /// <summary>
            /// PT group
            /// </summary>
            PT
        }
        #endregion

        /// <summary>
        /// Module Names
        /// </summary>
        #region Modules
        public enum Modules
        {
            /// <summary>
            /// Washer
            /// </summary>
            WASHER = 5,

            /// <summary>
            /// Dryer
            /// </summary>
            DRYER = 6,

            /// <summary>
            /// ProductionReports
            /// </summary>
            PRODUCTION = 15,
            /// <summary>
            /// TUMBLING
            /// </summary>
            TUMBLING = 1,
            /// <summary>
            /// HOURLYBATCHCARD
            /// </summary>
            HOURLYBATCHCARD = 2,

            /// <summary>
            /// #AZ 27/02/2018 1.n FDD@NGC_CR_090 BATCHORDER
            /// </summary>
            BATCHORDER = 2,

            /// <summary>
            /// FINALPACKING
            /// </summary>
            FINALPACKING = 10,

            /// <summary>
            /// ConfigurationSetUp
            /// </summary>
            CONFIGURATIONSETUP = 14,

            /// <summary>
            /// ProductionSystemReports
            /// </summary>
            PRODUCTIONSYSTEMREPORTS = 15,

            /// <summary>
            /// QC Scanning System
            /// </summary>
            QCSCANNINGSYSTEM = 9,

            /// <summary>
            /// QC Yield and Packing
            /// </summary>
            QCYIELDANDPACKING = 8,

            /// <summary>
            /// Production Logging
            /// </summary>
            PRODUCTIONLOGGING = 11,
            /// <summary>
            /// QAI System
            /// </summary>
            QAISYSTEM = 3,
            /// <summary>
            /// QAISYSTEMINNERTENPCS
            /// </summary>
            QAISYSTEMINNERTENPCS = 50,
            /// <summary>
            /// QA System
            /// </summary>
            QASYSTEM = 13,
            /// <summary>
            /// PostTreatment
            /// </summary>
            POSTTREATMENT = 7,
            /// <summary>
            /// PostTreatment
            /// </summary>
            REWORKORDER = 4,
            /// <summary>
            /// SurgicalGloveSystem
            /// </summary>
            SURGICALGLOVESYSTEM = 22,
            /// <summary>
            /// NONE
            /// </summary>
            NONE = 0,

            BRANDMASTER = 19,

            WORKORDER = 20
        }
        #endregion

        #region SubModules
        /// <summary>
        /// Screen Ids
        /// </summary>
        public enum SubModules
        {
            /// <summary>
            /// NormalBatchCard
            /// </summary>
            NormalBatchCard = 1,
            /// <summary>
            /// OnlineByPassBatchCard
            /// </summary>
            OnlineByPassBatchCard = 4,
            /// <summary>
            /// Tumbling-WaterTightBatchCard
            /// </summary>
            WaterTightBatchCardTumb = 3,
            /// <summary>
            /// QCScanning-WaterTightBatchCard
            /// </summary>
            WaterTightBatchCardQC = 51,
            /// <summary>
            /// VisualTestBatchCard
            /// </summary>
            VisualTestBatchCard = 5,
            /// <summary>
            /// CustomerRejectGloves
            /// </summary>
            CustomerRejectGloves = 9,
            /// <summary>
            /// LostBatchCard
            /// </summary>
            LostBatchCard = 2,
            /// <summary>
            /// ScanBatchCardInnerBox
            /// </summary>
            ScanBatchCardInnerBox = 43,
            /// <summary>
            /// ScanBatchCardWeight
            /// </summary>
            ScanBatchCardWeight = 47,
            /// <summary>
            /// ScanPTBatchCard
            /// </summary>
            ScanPTBatchCard = 34,
            /// <summary>
            /// ChangeGloveType
            /// </summary>
            ChangeGloveType = 35,
            /// <summary>
            /// DowngradeBatchCard
            /// </summary>
            DowngradeBatchCard = 45,
            /// <summary>
            /// ScanIn
            /// </summary>
            ScanIn = 17,
            /// <summary>
            /// ScanOut
            /// </summary>
            ScanOut = 18,
            /// <summary>
            /// ScanBatchCardInnerOuter
            /// </summary>
            ScanBatchCardInnerOuter = 19,
            /// <summary>
            /// Print 2nd Grade Inner Outer
            /// </summary>
            Print2ndGradeInnerOuter = 21,
            /// <summary>
            /// ScanBatchCardInnerOuterSurgical
            /// </summary>
            ScanBatchCardInnerOuterSurgical = 109,
            /// <summary>
            /// ScanBatchCardInnerOuter2ndGrade
            /// </summary>
            ScanBatchCardInnerOuter2ndGrade = 62,
            /// <summary>
            /// ChangeBatchCardInner
            /// </summary>
            ChangeBatchCardInner = 22,
            /// <summary>
            /// ScanTMPPackInventory
            /// </summary>
            ScanTMPPackInventory = 23,
            /// <summary>
            /// ScanMultipleBatchPacking
            /// </summary>
            ScanMultipleBatchPacking = 24,
            /// <summary>
            /// Tumbling-WaterTightBatchCard
            /// </summary>
            WaterTightBatchCard = 25,
            /// <summary>
            /// Print Surgical Inner Outer
            /// </summary>
            SurgicalInnerOuter = 109,
            /// <summary>
            /// Glove Output Reporting
            /// </summary>
            GloveOutputReporting = 113,
            /// <summary>
            /// NONE
            /// </summary>
            NONE = 0,
            /// <summary>
            /// Print Reproduction Water Tight Batch Card
            /// </summary>
            PrintReproductionWaterTightBatchCard = 119, // #Azrul 13/07/2018: Merged from Live AX6
            /// <summary>
            /// Print Reproduction Visual Test Batch Card
            /// </summary>
            PrintReproductionVisualTestBatchCard = 123, // #Azrul 13/07/2018: Merged from Live AX6
        }
        #endregion

        #region TierSide
        public enum Tierside
        {
            LT = 1,
            LB = 2,
            RT = 3,
            RB = 4,
            NONE = 0
        }
        #endregion

        #region PostingJournalTypes
        public enum JournalType
        {
            RAF,
            FGRAF,
            BOM,
            MOVEMENT,
            TRANSFER,
            CheckCBDItem, // #MH 20/10/2016
            RWKCR,         // #MK 28/05/2018
            MOVEMENTCBCI // #AzmanCBCI
        }
        #endregion

        #region Screen Names
        #region Screen Names
        /// <summary>
        /// Screen Names
        /// </summary>
        public enum ScreenNames
        {
            /// <summary>
            /// MainMenu
            /// </summary>
            MainMenu,
            /// <summary>
            /// Configuration SetUp - Batch Inquiry
            /// </summary>
            [Description("Configuration SetUp - Batch Inquiry")]
            BatchInquiry,
            /// <summary>
            /// Configuration SetUp - ReasonMaster
            /// </summary>
            [Description("Configuration SetUp - ReasonMaster")]
            ReasonMaster,
            /// <summary>
            /// Configuration SetUp - WasherMaster
            /// </summary>
            [Description("Configuration SetUp - WasherMaster")]
            WasherMaster,
            /// <summary>
            /// Configuration SetUp - DryerMaster
            /// </summary>
            [Description("Configuration SetUp - DryerMaster")]
            DryerMaster,
            /// <summary>
            /// Configuration SetUp - WasherProcess
            /// </summary>
            [Description("Configuration SetUp - WasherProcess")]
            WasherProcess,
            /// <summary>
            /// Configuration SetUp - DryerProcess
            /// </summary>
            [Description("Configuration SetUp - DryerProcess")]
            DryerProcess,
            /// <summary>
            /// Configuration SetUp - WasherStoppageData
            /// </summary>
            [Description("Configuration SetUp - WasherStoppageData")]
            WasherStoppageData,
            /// <summary>
            /// Configuration SetUp - DryerStoppageData
            /// </summary>
            [Description("Configuration SetUp - DryerStoppageData")]
            DryerStoppageData,
            /// <summary>
            /// Configuration SetUp - ProductionLineMaintenance
            /// </summary>
            [Description("Configuration SetUp - ProductionLineMaintenance")]
            ProductionLineMaintenance,
            /// <summary>
            /// Configuration SetUp - QCGroupMaster
            /// </summary>
            [Description("Configuration SetUp - QCGroupMaster")]
            QCGroupMaster,
            /// <summary>
            /// Configuration SetUp - QAIDefectMaster
            /// </summary>
            [Description("Configuration SetUp - QAIDefectMaster")]
            QAIDefectMaster,
            /// <summary>
            /// Add QC Group Stoppage Data
            /// </summary>
            [Description("Add QC Group Stoppage Data")]
            QCStoppageData,
            /// <summary>
            /// Production Logging Activity
            /// </summary>
            [Description("Configuration SetUp - ProductionLogging")]
            ProductionLogging,
            /// <summary>
            /// Configuration SetUp - LineClearanceAuthoriseSetup
            /// </summary>
            [Description("Configuration SetUp - Line Clearance Authorise Setup")]
            LineClearanceAuthoriseSetup,
            /// <summary>
            /// Edit QC Efficiency
            /// </summary>
            [Description("Configuration SetUp - EditQCEfficiency")]
            EditQCEfficiency,
            /// Configuration SetUp - QAIDefectMaster
            /// </summary>
            [Description("Configuration SetUp - GloveTypeMaster")]
            GloveTypeMaster,
            /// <summary>
            /// Brand Master Maintenance
            /// </summary>
            [Description("Brand Master - Warehouse")]
            BrandMasterMaintenance,

            [Description("Brand Master - Preshipment")]
            BrandMasterPreshipment,

            [Description("Brand Master - Warehouse")]
            BrandMasterWarehouse,

            [Description("Work Order -  Maintenance")]
            WorkOrderMaintenance,

            [Description("Work Order - Status")]
            WorkOrderStatus,
        }
        #endregion
        #endregion

        public enum QAIScreens
        {
            EditDefects = 1,
            QAIChangeQCType = 2,
            QAIResamplingScan = 3,
            QAIScan = 4,
            QAIScanInnerTenPcs = 5,
            ScanQITestResult = 6,
            //add new screen MYAdamas Edit Online Batch Card Info
            EditOnlineBatchCardInfo=7,
            NONE = 0
        }

        public enum QAIPageTransition
        {
            FormClose = 1,
            FormEscape = 2,
            FormCancel = 3,
            FormNavigation = 4,
            NONE = 0
        }

        #region Final Packing
        public const string FINALPACKING = "Final Packing";
        public const string FP_TMP_REASONTYPE = "TMP Pack Reason (Master)";
        public const string INVALIDMESSAGE = "Invalid";
        public const string INCOMPLETE = "Incomplete";
        public const string EXPIRED = "Expired";
        public const string INPROGRESS = "In Progress";
        public const string COMPLETED = "Completed";
        public const string RECORD = "Record";
        public const string PASS = "Pass";
        public const string FAIL = "FAIL";
        public const string FP_NORECORD = "No Record Exists";
        public const string FP_NORESULTCAPTURED = "Result Not Captured";
        public const string FP_RESULTPASS = "1";
        public const string FP_RESULTFAIL = "0";
        public const string PT = "PT";
        public const string QA = "QA";
        public const char charRight = 'R';
        public const char charLeft = 'L';
        public const string FINALPACKINGPRINTER = "Final Packing Printer";
        public const string FAIL1 = "Fail1";
        public const string FP_TOWERLIGHT = "Final Packing Tower Light";
        public const string FP_SCANNER = "Final Packing Scanner";
        public const string FP_TOWERLIGHT_SCANNER = "Final Packing Tower Light and Scanner";
        #endregion

        #region Tumbling
        // This is used to indicate that the batch is produced offline
        public const bool OFFLINE_PRODUCTION = false;
        // For all the batches produced Production Type is "T" even for HourlyBatch
        public const string PRODUCTION_TYPE = "T";
        // This is used to fetch online by passreasons from DB. This should be in ReasonTypeMaster Table 
        public const string BYPASS_REASON_TYPE = "Online ByPass Reason (Tumbling System)";
        // Message to be displayed when 10Pcs weight is out of range
        public const string TENPCS_WEIGHT_RANGE = "Ten Pcs Weight out of range";
        // Maximum length operator can have
        public const int OPERATOR_LENGTH = 6;
        public const string LEFT = "LEFT";
        public const string RIGHT = "RIGHT";
        public const string RESULT = "Result";
        public const string SHORTLEFT = "L";
        // Message to be displayed when Batch weight is out of range
        public const string BATCHWEIGHT_RANGE = "Batch Weight out of range";
        public const string TENPCSWEIGHT_RANGE = "10 Pcs Weight Out Of Range";
        public const string PCSCOUNT_RANGE = "Pcs Count out of range.";
        public const int ZERO = 0;
        public const int ONE = 1;
        public const int TWO = 2;
        public const int TWOHUNDRED = 200;
        public const int ONETWENTY = 120;
        public const int ONEZEROZEROTWO = 1002;
        public const int THREE = 3;
        public const int FOUR = 4;
        public const int SIX = 6;
        public const int SEVEN = 7;
        public const int EIGHT = 8;
        public const int NINE = 9;
        public const int FIVE = 5;
        public const int TEN = 10;
        public const int ELEVEN = 11;
        public const int TWELVE = 12;
        public const int THIRTEEN = 13;
        public const int FOURTEEN = 14;
        public const int THIRTYONE = 31;
        public const int THIRTY = 30;
        public const int TWENTYEIGHT = 28;
        public const int TWENTYNINE = 29;
        public const int MINUSONE = -1;
        public const int THOUSAND = 1000;
        public const int TWENTY = 20;
        public const string  USED_GLOVE = "Used Glove";       
        // BatchType for Lost Batch Card
        public const string LOST_BATCH_TYPE = "X";
        public const int SERIAL_LENGTH = 10;
        public const string SERIAL_NUMBER_SECONDGRADE = "4";
        // Used to fetch ModuleId from ModuleMaster
        public const string TUMBLING_SYSTEM = "Tumbling System";
        //// Used to fetch SubModuleId from SubModuleMaster
        public const string NORMAL_BATCH_CARD = "Tumbling - PrintNormalBatchCard";
        //// Used to fetch SubModuleId from SubModuleMaster
        public const string LOST_BATCH_CARD = "Tumbling - PrintLostBatchCard";
        public const string BUSINESSLOGIC = "BLL";
        public const string ENCRYPTDECRYPT = "EncryptDecrypt";
        public const string SAVEORUPDATEAPPCONFIG = "SAVEORUPDATEAPPCONFIG";
        //// Used to fetch SubModuleId from SubModuleMaster
        public const string VISUAL_TEST_BATCH_CARD = "Tumbling - PrintVisualTestBatchCard";
        /// <summary>
        /// used to fetch submoduleid from submodulemaster
          public const string REPRODUCTION_VISUAL_TEST_BATCH_CARD = "Tumbling - PrintReproductionVisualTestBatchCard";
        /// </summary>
        //// Used to fetch SubModuleId from SubModuleMaster
        public const string WATER_TIGHT_BATCH_CARD = "Tumbling - PrintWaterTightBatchCard";
        /// <summary>
        /// used to fetch submoduleid from submodulemaster
        public const string REPRODUCTION_WATER_TIGHT_BATCH_CARD = "Tumbling - PrintReproductionWaterTightBatchCard";
        //// Used to fetch SubModuleId from SubModuleMaster
        public const string CUSTOMER_REJECT_GLOVE_BATCH_CARD = "Tumbling - PrintCustomerRejectBatchCard";
        public const string TUMBLING_REPRINT_BATCH_CARD = "Tumbling - ReprintBatchCard";
        //// Used to fetch Reasons, this should be present in ReasonTypeMaster
        public const string REJECT_REASON_TYPE = "Reject Gloves Reason (Tumbling System)";
        //Batch Type for RejectGlove
        public const string REJECTGLOVE = "PR";
        //// Used to fetch SubModuleId from SubModuleMaster
        public const string REJECT_GLOVE_SCREEN = "Tumbling - RejectGlove";
        //// Used to fetch SubModuleId from SubModuleMaster
        public const string POLYMER_TEST_SLIP_SCREEN = "Tumbling - PrintPolymerTestSlip";
        //// Used to fetch SubModuleId from SubModuleMaster
        public const string ONLINE_BYPASS_BATCH_CARD = "Tumbling - OnlineByPassGloveBatchCard";        
        ////

        public const string CONNECTING = "Connecting...";
        public const string START_DATE = "yyyy-MM-dd";
        public const string LOST_AREA = "LostArea";
        public const string START_TIME = "HH:mm";
        public const string WATER_TIGHT_TYPE = "WTType";
        public const string VISUAL_TEST_TYPE = "VTType";
        public const string NEXTPROCESS = "Next Process";
        public const string DEFECT_TYPE = "Defect Type";
        public const string PRINTING = "Printing.......";
        public const string BARCODE_ERROR = "Could not generate Barcode and Print";
        public const string PRINTER_ERROR = "No printers are installed.";
        public const string INTEGRATION = "Integration";
        public const string WEIGHT_ERROR = "Could not connect to scaling system and get weight";
        public const string TENPCS_WEIGHT_ERROR = "Scale connection error, please check hardware integration for 10 Pcs(g).";
        public const string BATCH_WEIGHT_ERROR = "Scale connection error, please check hardware integration for Batch(Kg).";
        public const string TENPCS = "10 Pcs(g)";
        public const string BATCH = "Batch(Kg)";
        //Constant used to print Reject Glove Template
        public const string TIME = "TIME :";
        //Constant used to print Reject Glove Template
        public const string BATCH_TEXT = "BATCH NO. :";
        //Constant used to print Reject Glove Template
        public const string BATCH_WEIGHT = "BATCH WEIGHT :";
        //Constant used to print Reject Glove Template
        public const string BACKGROUND_COLOR = "#A6A6A6";
        public const string REWORK_PROCESS = "Rework Process";
        public const string EXCLUDE_OPERATOR = "Operator";
        public const string POLYMER_TEST = "Polymer";
        public const string REASON = "Reason";
        public const string SIZE = "Size";
        public const string SHIFT = "Shift";
        public const string GLOVETYPE = "Glove Type";
        public const string LINE = "Line";
        public const string TYPE = "Type";
        public const string LOTNO = " Add Lot No(s)";
        public const string TENPIECESWEIGHT = "Ten Pieces Weight";
        public const string BATCHWEIGHT = "Batch Weight";
        public const string QCTYPE = "QCType";
        public const string TAB = "\t";
        public const string TOTAL_DISPOSE = "Total dispose stock count : {0} {1} ";
        public const string TOTAL_VALID = "Total valid stock count : {0} {1} ";
        public const string TENPCS_BATCH_MESSAGE = "10 PCs weight out of range : {0}\nBatch Weight out of Range : {1}\nEnter password to print. Cancel to re-weight.";
        public const string TENPCS_BATCH_TITLE = "10Pcs & Batch Weight Out of Range";
        public const string WEIGHT_OUT_OF_RANGE = "Weight out of range!";
        public const string TENPCS_MESSAGE = "10 PCs weight out of range : {0}\nEnter password to print. Cancel to re-weight.";
        public const string BATCHWEIGHT_MESSAGE = "Batch Weight out of Range : {0}\nEnter password to print. Cancel to re-weight.";
        #endregion

        #region Hourly Batch
        /// <summary>
        /// Hourly Batch Card
        /// </summary>
        public const string HOURLYBATCHCARD = "Hourly Batch Card";
        /// <summary>
        /// Glove Output Reporting
        /// </summary>
        public const string GLOVEOUTPUTREPORTING = "Glove Output Reporting";
        /// <summary>
        /// Line Selection
        /// </summary>
        public const string LINESELECTION = "Line Selection";
        /// <summary>
        /// Batch Card Reprint Log
        /// </summary>
        public const string BATCH_CARD_REPRINT_LOG = "Batch Card Reprint Log";
        /// <summary>
        /// Manual Print Batch Card
        /// </summary>
        public const string MANUAL_PRINT_BATCH_CARD = "Manual Print Batch Card";
        /// <summary>
        /// Online 2nd Grade Glove
        /// </summary>
        public const string ONLINE_2G = "Online 2nd Grade Glove";
        /// <summary>
        /// Reprint Batch Card
        /// </summary>
        public const string REPRINT_BATCH_CARD = "Reprint Hourly Batch Card";

        /// <summary>
        /// Reprint Batch Card
        /// </summary>
        public const string REPRINT_ON2G_BATCH_CARD = "Reprint ON2G Batch Card";

        /// <summary>
        /// Max length of Reason Text
        /// </summary>
        public const int REASON_TEXT_LENGTH = 20;

        /// <summary>
        /// Max length of Glove Description
        /// </summary>
        public const int GLOVE_DESCRIPTION_LENGTH = 50;

        /// <summary>
        /// Max length of Glove Size
        /// </summary>
        public const int GLOVE_SIZE_LENGTH = 50;
        public const string START = "Start";
        public const string END = "End";
        public const string QC = "QC";
        public const string UNDERSCORE = "_";
        public const string COMMA = ",";
        public const char CHARCOMMA = ',';
        #endregion

        #region Surgical
        /// <summary>
        /// Surgical Glove System
        /// </summary>
        public const string SURGICALGLOVESYSTEM = "Surgical Glove System";
        /// <summary>
        /// Print Surgical Batch Card
        /// </summary>
        public const string PRINTSURGICALBATCHCARD = "Print Surgical Batch Card";
        #endregion

        #region GIS
        public const string GLOVEINVENTORYSYSTEM = "Glove Inventory";
        #endregion

        #region QA system

        /// <summary>
        /// QA Test Result
        /// </summary>
        public const string ADD_QA_TEST_RESULT = "AddTestResult";

        /// <summary>
        /// Default Weight
        /// </summary>
        public const string DEFAULT_WEIGHT = "0.0000";

        /// <summary>
        /// Protein Test
        /// </summary>
        public const string QA_PROTEIN = "Protein";

        /// <summary>
        /// Powder Test
        /// </summary>
        public const string QA_POWDER = "Powder";

        /// <summary>
        /// HotBox Test
        /// </summary>
        public const string QA_HOTBOX = "HotBox";

        /// <summary>
        /// QA Test Pass
        /// </summary>
        public const string QA_TEST_PASS = "Pass";
        /// <summary>
        /// QA duplicate message
        /// </summary>
        public const string DUPLICATE_MESSAGE = "Duplicate";
        /// <summary>
        /// Label Weight
        /// </summary>
        public const string WEIGHT = "Weight";
        /// <summary>
        /// Label Protein content
        /// </summary>
        public const string PROTEIN_CONTENT = "Protein Content (ug/g)";
        /// <summary>
        /// Label wt of filter paper
        /// </summary>
        public const string WEIGHT_FILTER_PAPER = "Weight of Filter Paper (g)";
        /// <summary>
        /// Label wt of filter paper after filteration
        /// </summary>
        public const string WEIGHT_FILTER_PAPER_AFTER_FILTERATION = "Weight of Filter Paper after Filteration (g)";
        /// <summary>
        /// Label wt of filter paper after filteration
        /// </summary>
        public const string WEIGHT_FILTER_PAPER_AND_RESIDUE = "Weight of Filter Paper + Residue Powder (g)";
        /// <summary>
        /// Label Residue Powder
        /// </summary>
        public const string RESIDUE_POWDER = "Residue Powder Per Glove (mg)";
        /// <summary>
        /// Validation Constant
        /// </summary>
        public const string VALIDATION_ALERT = "Please enter the data for following fields:";
        #endregion

        #region PT
        /// <summary>
        /// Post Treatment - Print Protein Test Slip
        /// </summary>
        public const string POSTTREATMENTSYSTEM = "Post Treatment";
        public const string SCAN_PT_BATCHCARD_REASONTYPE = "PT Rework  Reason (Master)";
        public const string PRINT_PROTEIN_TEST = "Print Protein Test Slip";
        public const string PROTEIN_TEST = "Protein";
        public const string POWDER_TEST = "Powder";
        public const string HOTBOX_TEST = "Hot Box";

        // use these constants for ax4 posting
        public const string PWTBCP = "PWTBCP";
        public const string PWTBCA = "PWTBCA";
        public const string PWTBCQ = "PWTBCQ";
        public const string PWTBCS = "PWTBCS";
        public const string SPBC = "SPBC";
        public const string SBCIB = "SBCIB";
        public const string DBC2G = "DBC2G";
        






        public const string PROTEIN_POWDER_REFERENCE = "8";
        public const string HOTBOX_REFERENCE = "0";

        /// <summary>
        /// Post Treatment - Print Hot Box Test Slip
        /// </summary>
        public const string PRINT_HOTBOX_TEST = "Print Hot Box Test Slip";

        /// <summary>
        /// Post Treatment - Print Powder Test Slip
        /// </summary>
        public const string PRINT_POWDER_TEST = "Print Powder Test Slip";

        /// <summary>
        /// Configuration and Setup - Add reason screen
        /// </summary>
        public const string ADD_REASON_SCREEN = "Add Reason";
        #endregion

        #region QC Packing Yield System
        /// <summary>
        /// QCYP - Scan Batch Card 
        /// </summary>
        public const string QCYP_SCAN_OUT_BATCHCARD = "Scan Out Batch Card";
        /// <summary>
        /// QCYP - Scan In Batch Card 
        /// </summary>
        public const string SCAN_IN_BATCHCARD = "Scan In Batch card";
        public const string QC_GROUPTYPE = "QC";
        public const string QYP_SCAN_IN_QCPACKINGGROUPSUBMODULE = "Scan In QC Packing Group";
        public const string END_TIME_NOT_AVAILABLE = "Not Available";
        public const string QYP_SCAN_OUT_QCPACKINGGROUPSUBMODULE = "Scan Out QC Packing Group";
        public const string SCAN_IN_BATCHCARD_EXTEND = "Scan In TMP/MSR/FG Rework/Concession";
        /// <summary>
        /// QC Scanning System
        /// </summary>
        public const string QC_PACKING_YIELD_SYSTEM = "QC & Packing Yield";
        /// <summary>
        /// Rework Reason type
        /// </summary>
        public const string REWORK_REASON_TYPE = "Rework Reason (QC & Packing Yield System)";
        /// <summary>
        /// Reason type
        /// </summary>
        /// Added by Tan Wei Wah 20190131
        public const string QCPY_REASON_TYPE = "Reason (QC & Packing Yield System)";

        /// <summary>
        /// Split Batch
        /// </summary>
        public const string SPLIT_BATCH = "Split Batch";
        /// <summary>
        /// Qc Type Changed
        /// </summary>
        public const string QC_TYPE_CHANGED = "Qc Type Changed";
        /// <summary>
        /// Inner box count length
        /// </summary>
        public const int INNER_BOX_COUNT_LENGTH = 10;
        /// <summary>
        /// Batch Status Complete
        /// </summary>
        public const string BATCH_STATUS_COMPLETE = "Completed";
        /// <summary>
        /// Packing Group Type
        /// </summary>
        public const string PACKING_GROUP_TYPE = "Packing Group";
        /// <summary>
        /// QC & Packing Group Type
        /// </summary>
        public const string QCPACKING_GROUP_TYPE = "QC & Packing Group";
        /// <summary>
        /// QC & Packing Group Type for display
        /// </summary>
        public const string QCPACKING_GROUP_TYPE_DISPLAY = "QC && Packing Group";
        /// <summary>
        /// PT Status
        /// </summary>
        public const string PT_INCOMPLETE = "PT Incomplete";
        /// <summary>
        /// PTQI Status
        /// </summary>
        public const string PTQI_INCOMPLETE = "PTQI Incomplete";
        /// <summary>
        /// FG
        /// </summary>
        public const string FG = "FG";


        public const string QC_TARGET_TYPE = "QCTargetType";
        #endregion

        //TODO: Need to change after master data populated
        #region QAI
        public const string WATER_TIGHT_SAMPLING_SIZE = "WTSamplingSize";
        public const string VISUAL_TEST_SAMPLING_SIZE = "VTSamplingSize";

        public const string QAI_EDIT_DEFECTSREASONTYPE = "Edit Defects Change Reason (QAI Scanning)";
        public const string QAI_REASONTYPE = "QC Change Reason (QAI Scanning)";
        public const string QAI_MODULENAME = "QAI Scanning System";
        public const string QAI_DefaultAQL = "AQL 0.65";
        #endregion

        #region ProductionLine
        public const string INVALID_DATA_VALIDATION_ALERT = "Invalid data for the field.";
        public const int REASON_ID_FOR_PL = 1003;
        //public const string REASON_PL_STOP = "Production Line Stop";
        public const string REASON_PL_STOP = "Production Stoppage reason maintenance (Production Logging)";
        
        public enum Tier
        {
            LT = 0,
            LB = 1,
            RT = 2,
            RB = 3
        }

        #endregion


        #region Configuration & Setup
        public const string LOGIN_MESSAGE = "Enter Password";
        /// <summary>
        /// Configuration and Setup - Edit reason screen
        /// </summary>
        public const string EDIT_REASON_SCREEN = "Edit Reason";


        public const string DAL = "DAL";
        public const string INVALID_MESSAGE = "Invalid";

        public const string EDIT_DEFECT_SCREEN = "Edit Defect";

        public const string ADD_DEFECT_SCREEN = "Add Defect";
        /// <summary>
        /// Add Washer Stoppage data
        /// </summary>
        public const string ADD_WASHER_STOPPAGE_DATA = "AddWasherStoppageData";
        /// <summary>
        /// Washer Stoppage Reason
        /// </summary>
        public const string WASHER_STOPPAGE_REASON = "Washer Stoppage Reason (Washer Yield)";
        /// <summary>
        /// Dryer Stoppage Reason
        /// </summary>
        public const string DRYER_STOPPAGE_REASON = "Dryer Stoppage Reason (Dryer Yield)";
        /// <summary>
        /// Batch Type
        /// </summary>
        public const string BATCH_TYPE = "BatchType";
        /// <summary>
        /// QC/Packing group Stoppage Reason
        /// </summary>
        public const string QCGROUP_STOPPAGE_REASON = "QC & Packing Stoppage Reason (QC & Packing Yield System)";
        /// <summary>
        /// QC/Packing group type
        /// </summary>
        public const string QC_GROUP_TYPE = "QC Group Type";
        /// <summary>
        /// Configuration and Setup - Edit control
        /// </summary>
        public const string EDIT_CONTROL = "Edit";
        /// <summary>
        /// Configuration and Setup - Add control
        /// </summary>
        public const string ADD_CONTROL = "Add";
        /// <summary>
        /// Configuration and Setup - Batch Card Print Frequency
        /// </summary>
        public const string BATCH_PRINT_FREQUENCY = "BatchCardPrintFrequency";
        /// <summary>
        /// Production Line Start Activity
        /// </summary>
        public const string START_ACTIVITY = "Start Activity";
        #endregion
        /// <summary>
        /// Reporting Path for QAI
        /// </summary>
        public const string QAIMONITORINGREPORTPATH = "QAIMontoringReportPath";
        /// <summary>
        /// Ten Pcs Report Path
        /// </summary>
        public const string TENPCSREPORTPATH = "TenPcsWeight.rdlc";
        /// <summary>
        /// Report Processing mode
        /// </summary>
        public const string PROCESSINGMODE = "ReportProcessingMode";
        /// <summary>
        /// Local Processing mode
        /// </summary>
        public const string LOCAL = "Local";
        

        #region QC Scanning
        public const string SCAN_BATCHCARDWEIGHT_REASONTYPE = "QC Scanning - Scan Batch Card Weight";
        public const string DEFECTIVEGLOVE_PLATFORM_REASONTYPE = "DefectiveGlovePlatform";
        /// <summary>
        /// Packing size of inner box
        /// </summary>
        public const string PACKING_SIZE = "Packing Size";
        /// <summary>
        /// QC Scanning - Batch Status
        /// </summary>
        public const string BATCH_STATUS = "Batch Status";

        /// <summary>
        /// QC Scanning - Pack Into
        /// </summary>
        public const string PACK_INTO = "Pack Into";

        /// <summary>
        /// QC Scanning - TMP Pack
        /// </summary>
        public const string TMP_PACK = "TMP";

        /// <summary>
        /// QC Scanning - TMP Pack Area
        /// </summary>
        public const string TMP_PACK_AREA = "TP";
        /// <summary>
        /// FP AreaCode
        /// </summary>
        public const string FP_AREACODE = "-PS";

        /// <summary>
        /// FP AreaCode
        /// </summary>
        public const string FinalPacking_AREACODE = "PS";
        /// <summary>
        /// FP Resource
        /// </summary>
        public const string FP_RESOURCE = "-P";
        /// <summary>
        /// FP SecondGrade
        /// </summary>
        public const string FP_SecondGrade = "2nd Grade";

        public const string QC_SecondGradeBatch = "2G/";
        /// <summary>
        /// QC Type
        /// </summary>
        public const string STRAIGHT_PACK = "STRAIGHT PACK";

        /// <summary>
        /// QC Scanning - Downgrade Type
        /// </summary>
        public const string DOWNGRADE_TYPE = "Downgrade Type";
        public const string QC_DECIMAL_FORMAT = "{0:#,#.00}";

        /// <summary>
        /// QC Scanning System
        /// </summary>
        public const string QC_SCANNING_SYSTEM = "QC Scanning System";

        /// <summary>
        /// QC Scanning System
        /// </summary>
        public const string QCSCANNINGSYSTEM = "QCScanningSystem";

        /// <summary>
        /// SECOND GRADE TYPE
        /// </summary>
        public const string SECOND_GRADETYPE = "Second Grade Type";

        /// <summary>
        /// SECOND GRADE TYPE
        /// </summary>
        public const string SECOND_GRADETYPE_PCs = "2ndGradeSticker PCs";

        /// <summary>
        /// Downgrade Type Reject
        /// </summary>
        public const string DOWNGRADE_REJECT = "Reject";

        /// <summary>
        /// QC Scanning - Scan Batch Card Weight
        /// </summary>
        public const string SCAN_BATCHCARD_WEIGHT = "QC Scanning - Scan Batch Card Weight";

        /// <summary>
        /// HTLG_P7CR_014&015 2nd Grade Surgical Glove Reporting
        /// QC Scanning - Scan Batch Card Pieces
        /// </summary>
        public const string SCAN_BATCHCARD_PIECES = "QC Scanning - Scan Batch Card Pieces";

        /// <summary>
        /// QCScanning - Scan Batch Card 
        /// </summary>
        public const string QC_SCAN_OUT_BATCHCARD = "QC Scanning - ScanBatchCard";

        public const string BATCH_PCSCOUNT_INVALID = "Either Batch or Pcs Count should be greater than 0.";
        public const string BATCH_PCSCOUNT_ENTERED = "You have entered both batch(g) and pcs count field, please ensure only one of this field are use.";
        public const string RESAMPLE = "RESAMPLE";
        /// <summary>
        /// QC Scanning - Print Watertight Batch Cards
        /// </summary>
        public const string QCSCAN_PWTBC = "QC Scanning - Print Watertight Batch Cards";


        /// <summary>
        /// QC Scanning - Defective Glove (Platform)
        /// </summary>
        public const string QC_DEFECTIVE_GLOVE_PLATFORM = "QC Scanning - Defective Glove (Big Scale)";

        /// <summary>
        /// QC Scanning - Defective Glove (Platform)
        /// </summary>
        public const string DEFECTIVE_GLOVE_PLATFORM = "Defective Glove (Big Scale)";

        /// <summary>
        /// QC Scanning - Defective Glove (Small Scale)
        /// </summary>
        public const string QC_DEFECTIVE_GLOVE_SMALLSCALE = "QC Scanning - Defective Glove (Small Scale)";

        /// <summary>
        /// QC Scanning - Defective Glove (Small Scale)
        /// </summary>
        public const string DEFECTIVE_GLOVE_SMALLSCALE = "Defective Glove (Small Scale)";

        /// <summary>
        /// QC Scanning - Print Second Grade Sticker
        /// </summary>
        public const string PRINT_SECONDGRADE_STICKER = "QC Scanning - Print Second Grade Sticker";

        /// <summary>
        /// QCScanning - Scan Batch Card by Inner Box
        /// </summary>
        public const string QC_SCAN_BATCHCARD_INNER = "Scan Batch Card by Inner Box";

        /// <summary>
        /// QC Scanning - 2nd Grade Stock Verification
        /// </summary>
        public const string SECOND_GRADE_VERIFICATION = "2nd Grade Stock Verification";

        /// <summary>
        /// QC Scanning - 2nd Grade Stock Verification & Disposal
        /// </summary>
        public const string SECOND_GRADE_VERIFICATION_DISPOSAL = "2nd Grade Stock Verification & Disposal";

        /// <summary>
        /// QC Scanning - Reject Stock Verification & Disposal
        /// </summary>
        public const string REJECT_VERIFICATION_DISPOSAL = "Reject Stock Verification & Disposal";
        #endregion

        #region AX Integration

        public const string CONTEXT_COMPANY = "HNGC";
        public const string SERVICEERROR = "Service Unavailable";
        public const string AXSERVICEERROR = "AX Posting Error";
        public const string PT_QI = "PTQI";
        public const string QC_QI = "QCQI";
        public const string PWT = "PWT";
        public const string QWT = "QWT";
        public const string OWT = "OWT";
        public const string PSW = "PSW";
        public const string PVT = "PVT";// Visual Test batch type(PVTBCS)
        public const string QVT = "QVT"; // Visual Test batch type(PVTBCA)
        public const string WT = "WT";
        public const string T = "T";
        public const string X = "X";
        //cpkoo 20/2/2017 bug fix on item number
        public const string DOWNGRADE_ITEMNUMBER_NB = "2G-NBR-WIP";
        public const string DOWNGRADE_ITEMNUMBER_NR = "2G-NR-WIP";
        public const string DOWNGRADE_NB = "NB";
        #endregion

        #region Washer
        public const string WASHER_REWORK_REASON_TYPE = "Rework Reason (Washer System)";
        public const string DRYER_REWORK_REASON_TYPE = "Rework Reason (Cyclone System)";
        public const string OPERATORID = "Operator ID";
        public const string SERIALNO = "Serial No";
        public const string LINENUMBER = "Production Line";
        public const string PRODUCTIONDATE = "Date";
        public const string DRYER = "Dryer";
        public const string DRYERSYSTEM = "Cyclone System";
        public const string DRYERPROGRAM = "Dryer Program";
        public const string REWORKREASON = "Rework Type";
        public const string WASHER = "Washer";
        public const string WASHERSYSTEM = "Washer System";
        public const string WASHERPROGRAM = "Washer Program";
        public const string WASHERPROCESS = "Washer Process";
        public const string REWORK_REASON_WASHER = "Rework Reason";
        public const string MINUTES = "Minutes";
        public const string STAGE = "Stage";
        public const string TOTALMINUTES = "Total Minutes";
        public const string DRYERPROCESS = "Dryer Process";
        public const string COLDCYCLE = "Cold Cycle";
        public const string COLDCYCLE1 = "Cold Cycle1";
        public const string COLDCYCLE2 = "Cold Cycle2";
        public const string HOTCYCLE = "Hot Cycle";
        public const string HOTCYCLE1 = "Hot Cycle1";
        public const string HOTCYCLE2 = "Hot Cycle2";
        #endregion

        #region GlobalMessageBox
        public const string YES = "Yes";
        public const string NO = "No";
        public const string OK = "OK";
        public const string Cancel = "Cancel";
        #endregion

        #region ProductionLogging
        public const string NOFORMERS = "No. of Formers";
        public const string PCSPERHR = "Pcs Per Hour";
        public const string CYCLEPERHR = "Cycle Per Hour";
        public const int TWENTYFOUR = 24;
        public const int SIXTY = 60;
        public const int TWOFORTY = 240;
        public const string PROUCTION_LOGGING_TIME_CONFIGURATION = "ProuctionLoggingTimeConfiguration";
        public const string PCS = "pcs";
        public const string KG = "Kg";

        public const string EDIT_REASON_PL = "Production Line Start Stop Reason (Production Logging)"; //Added by Tan Wei Wah 20190321
        #endregion

        #region ProductionDefect
        public const string SIDE = "Side";
        public const string DEFECT = "Defect";
        public const string NODEFECT = "No Defect";
        #endregion

        #region ProductionSystem Reports
        public const int bWidth = 600;
        public const int bHeigth = 50;
        #endregion

        #region Scaling Properties
        public const string PLATFORMSCALE_DI10 = "DI-10";
        public const string PLATFORMSCALE_DI28 = "DI-28";
        public const string SMALLSCALE_OHAUS = "ohaus";
        public const string SMALLSCALE_TXB622L = "txb622l";
        public const string EVEN_PARITY = "Even";
        public const string ODD_PARITY = "Odd";
        public const string MARK_PARITY = "Mark";
        public const string NONE_PARITY = "None";
        public const string SPACE_PARITY = "Space";
        #endregion

        #region DateFunction
        public const string COLON = ":";
        public const string DATE_FORMAT = "dd-MM-yy";
        public const string CONFIG = @"{0}\Configuration\";
        public const string HARTALEGA = "Hartalega.";
        public const string CUSTOM_DATE_FORMAT = "dd/MM/yyyy";

        public const string DEFAULT_VISUAL_TEST_SAMPLE_SIZE0 = "0";
        public const string DEFAULT_VISUAL_TEST_SAMPLE_SIZE10 = "10";
        public const string DEFAULT_VISUAL_TEST_SAMPLE_SIZE50 = "50";
        public const string DEFAULT_VISUAL_TEST_SAMPLE_SIZE80 = "80";

        public const string DEFAULT_VISUAL_TEST_SAMPLE_SIZE10PLUS10 = "10 + 10"; // #Azrul 13/07/2018: Merged from Live AX6
        public const string DEFAULT_VISUAL_TEST_SAMPLE_SIZE50PLUS10 = "50 + 10"; // #Azrul 13/07/2018: Merged from Live AX6
        public const string DEFAULT_VISUAL_TEST_SAMPLE_SIZE80PLUS10 = "80 + 10"; // #Azrul 13/07/2018: Merged from Live AX6

        public const string DEFAULT_WATER_TIGHT_SAMPLE_SIZE80 = "80";
        public const string DEFAULT_WATER_TIGHT_SAMPLE_SIZE125 = "125";
        public const string DEFAULT_CUSTOMER_TYPE = "Normal Product";
        #endregion

        #region QAI Constatants
        public const string QAI_SUMMARY_Y_N = "Y/N – Do you want to maintain your QC Type?";
        public const string QMAX = "QMAX";
        public const string QMAX_F = "QMAX F";
        public const string QMAX_MP = "QMAX MP";
        // QAI Screens
        public const string QAI_MAIN_MENU = "QAI Main Menu";
        public const string SCAN_LOOSE_GLOVES_BATCH = "QAI Scan Batch Weight";
        public const string SCAN_INNER_TENPCS = "QAI Scan Inner Box";
        public const string RESAMPLING_SCAN = "QAI Re-Sampling Scan";
        public const string EDIT_DEFECTS = "Edit Defects";
        public const string CHANGE_QC_TYPE = "QAI Change QC Type";
        public const string QI_TEST_RESULT = "Scan QI Test Result";
        public const string QAI_DEFECT_SCREENS = "QAI Defect Screens";
        public const string SCAN_DEFECTS_SUMMARY = "Scan Defects Summary";
        public const string DEFECTSUMMARY = "DefectSummary";
        public const string SCAN_OUT_BATCH_CARD = "Scan Out Batch Card";
        public const string SCAN_BATCH_CARD_INNER_BOX = "Scan Batch Card (Inner Box)";
        public const string PINHOLE = "PinHole";
        public const string QAI_SCAN = "QAI Scan";
        public const string QAI_SUMMARY = "Defects Summary ";
        public const string EDIT_ONLINE_BATCH_CARD_INFO = "Edit Online Batch Card Info";
        /// <summary>
        /// Defect Key stroke A-Z(26) _ 0-9(10) 
        /// </summary>
        public const int QAI_DEFECT_KEYSTORKE_COUNT = 50;
        public const string OEE_MENU = "OEE Menu";

        public const string QITestReason = "QITestReason";

        #endregion


        public const string PROGRAM = "Program.cs";
        public const string Save = "Save";
        public const string Update = "Update";
        public const string Delete = "Delete";

        #region GlobalMessageBox
        public enum AlertType
        {
            Warning,
            Error,
            Information,
            Question,
            Exclamation
        }

        #endregion

        #region TVReports
        public const string QAIMONITORINGFILE = "QAIMonitoringSystem.xml";
        public const string TENPCSWEIGHTFILE = "TenPcsWeight.xml";
        public const string NEXT_ROTATION_TIME = "Next Batch Run Time:";
        public const string NEXT_REFRESH_TIME = "Next Batch Run Time:";
        public const string LAST_BATCH_RUN_TIME = "Batch Job Last Run:";
        public const string QAI_MONITORING_SYSTEM = "QAI Monitoring System";
        public const string QAI_MONITORING_SYSTEM_MONTHLY = "QAI Monitoring System Monthly";
        public const string COSMETIC_DEFECT_SYSTEM = "Top 4 Major Cosmetic Defects Analysis System"; // old name: Cosmetic Defect Analysis System
        public const string PRODUCTION_DEFECT_SYSTEM = "Top 4 Major Pinhole Defects Analysis System"; // old name: Production Defect Analysis System
        public const string QC_DEFECT_SYSTEM = "QC Defect Analysis System";
        public const string HISTORYDATE = "HistoryDate";
        public const string TEN_PCS_WEIGHT_SYSTEM = "Production TenPcs Weight Analysis System";
        public const string QCDEFECTFILE = "QCDefectSystem.xml";
        public const string COSMETICDEFECTFILE = "CosmeticDefectSystem.xml";
        public const string PRODUCTIONDEFECTFILE = "ProductionDefectSystem.xml";
        public const string ERROR_MESSAGE_IN_REPORTS = "Error while generating reports. Please contact MIS!";
        public const string SLASH = "\\";
        public const string NUMBER_FORMAT = "{0:#,##0}";
        public const string DECIMAL_FORMAT = "{0:#,0.0000}";
        public const string BR = "<br/>";
        public const string PERCENT = "%";
        public const string RED_COLOR = "Red";
        public const string ZERO_SLASH = "0/";
        public const string FILE_NOT_PRESENT = "File not present for the History Date in URL";
        public const string LOCATION = "Location :";
        #endregion

        #region Encryption
        public const string ENCRYPT = "Encrypt";
        public const string DECRYPT = "Decrypt";
        public const string ENCRYPTEDCONNECTIONSTRING = "Encrypted Connection String :";
        public const string DECRYPTEDCONNECTIONSTRING = "Decrypted Connection String :";
        public const string CONNECTIONSTRING = "Connection String :";

        #endregion

        #region MasterTables
        public enum ActionLog
        {
            Add = 1,
            Update = 2,
            Delete = 3
        }
        public enum EventLog
        {
            Save = 1,
            Update = 2,
            Print = 3
        }
        public const string eventlogsource = "Floor System";
        public const string eventlogtype = "Hartalega.FloorSystem.Business.Logic.EventLog.EventLogType.ModuleAccessEventLogType";


        public const string CUSTOMER_TYPE = "CustomerType";
        public const string DEFECT_CATEGORY_TYPE = "DefectCategoryGroup";
        public const string MODULE_SCREEN_AQL = "ModuleScreenAQL";
        #endregion

        #region Work Order Status

        public const string OPEN = "Open";
        public const string APPROVED = "Approved";
        public const string CLOSED = "Closed";

        #endregion

        #region EnumMaster Type

        public const string WORKORDERSTATUS = "WorkOrderStatus";
        public const string STATUS = "Status";
        public const string LABELSETSTATUS = "Label Set Status";
        public const string LABELSETDATEFORMAT = "LabelSetDateFormat";
        public const string PRINTER = "Printer";
        public const string LOTVERIFICATION = "LotVerification";
        public const string MANUFACTURINGDATEON = "ManufacturingDateOn";
        public const string POWDERFORMULA = "PowderFormula";

        #endregion

        public const string BRANDMASTERMAINMENU = "Brand Master - Main Menu";
        public const string WORKORDERMAINMENU = "Work Order - Main Menu";

        public const string BRANDMASTER_MAINTENANCE = "Brand Master - Maintenance";
        public const string BRANDMASTER_PRESHIPMENT = "Brand Master - Preshipment";
        public const string BRANDMASTER_WAREHOUSE = "Brand Master - Warehouse";

        public const string WORKORDER_MAINTENANCE = "Work Order - Maintenance";
        public const string WORKORDER_STATUS = "Work Order - Status";
        //public const string WORKORDER_NEW = "Work Order - Edit";

        public const string ACTIVE = "Active";
        public const string INACTIVE = "Inactive";
    }
}

