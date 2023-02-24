using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public abstract class Configuration
    {
        protected void UpdateAllFields(Configuration inObj, Configuration target)
        {
            PropertyInfo[] allProps = inObj.GetType().GetProperties();
            foreach (PropertyInfo pI in allProps)
            {
                object val = pI.GetValue(inObj, null);
                if (val != null)
                {
                    Type objType = val.GetType();
                    if (objType.IsClass)
                    {
                        pI.SetValue(target, val, null);
                    }
                }
            }
        }

        protected void FillConfigurableList(SortedDictionary<string, bool> lstConfItems, Configuration instance)
        {
            Type type = instance.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (pi.PropertyType.IsClass)
                {
                    lstConfItems.Add(pi.Name, (pi.GetValue(instance, null) != null));
                }
            }
        }

        protected Dictionary<string, object> GetAllPropertyValues(Configuration instance)
        {
            Dictionary<string, object> propNameValues = new Dictionary<string, object>();
            PropertyInfo[] allProps = instance.GetType().GetProperties();
            foreach (PropertyInfo pI in allProps)
            {
                object val = pI.GetValue(instance, null);
                if (val != null && pI.PropertyType.IsClass)
                {
                    propNameValues.Add(pI.Name, val);
                }
            }
            return propNameValues;
        }

        protected void UpdateField(string field, string value, Configuration instance)
        {
            Type type = instance.GetType();
            PropertyInfo pI = type.GetProperty(field);
            object obj = value;
            pI.SetValue(instance, obj, null);
        }

        abstract internal void UpdateInstanceWithJSON(string JSONData);
        abstract internal void FillConfigurableList(SortedDictionary<string, bool> lstConfItems);
        abstract internal void UpdateField(string field, string value);
        abstract internal Dictionary<string, object> GetAllPropertyValues();
    }

    public class WorkStationDataConfiguration : Configuration
    {
        #region Properties
        public string smallScalingSystem
        {
            get;
            set;
        }

        // ssCOM
        public string ssCOM
        {
            get;
            set;
        }

        public string ssBaudRate
        {
            get;
            set;
        }

        public string ssParity
        {
            get;
            set;
        }

        public string ssDataBit
        {
            get;
            set;
        }

        public string ssStopBit
        {
            get;
            set;
        }

        public string ssReadSec
        {
            get;
            set;
        }


        public string psCOM
        {
            get;
            set;
        }

        public string psBaudRate
        {
            get;
            set;
        }

        public string psParity
        {
            get;
            set;
        }

        public string psDataBit
        {
            get;
            set;
        }

        public string psStopBit
        {
            get;
            set;
        }

        public string psReadSec
        {
            get;
            set;
        }




        public string platformScalingSystem
        {
            get;
            set;
        }


        private bool? _HBCCanShowSerialNo;

        public bool? HBCCanShowSerialNo
        {
            get { return _HBCCanShowSerialNo; }
            private set
            {
                _HBCCanShowSerialNo = value;
            }
        }

        public string bool_HBCCanShowSerialNo
        {
            get { return HBCCanShowSerialNo.HasValue ? HBCCanShowSerialNo.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    HBCCanShowSerialNo = Convert.ToBoolean(value);
                }
            }
        }

        /// <summary>
        /// #AZ 14/03/2018 1.NGC_CR_094: Creation of HBC based on Registered Output 
        /// </summary>
        private bool? _GloveRptShow4Outputs;

        public bool? GloveRptShow4Outputs
        {
            get { return _GloveRptShow4Outputs; }
            private set
            {
                _GloveRptShow4Outputs = value;
            }
        }

        public string bool_GloveRptShow4Outputs
        {
            get { return GloveRptShow4Outputs.HasValue ? GloveRptShow4Outputs.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    GloveRptShow4Outputs = Convert.ToBoolean(value);
                }
            }
        }

        private bool? _APMStatus;
        public bool? APMStatus
        {
            get { return _APMStatus; }
            private set
            {
                _APMStatus = value;
            }
        }

        public string bool_APMStatus
        {
            get { return APMStatus.HasValue ? APMStatus.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    APMStatus = Convert.ToBoolean(value);
                }
            }
        }

        private bool? _lBucket = null;

        public bool? LBucket
        {
            get { return _lBucket; }
            private set
            {
                _lBucket = value;
            }
        }

        public string bool_LBucket
        {
            get { return LBucket.HasValue ? LBucket.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    LBucket = Convert.ToBoolean(value);
                }
            }
        }

        private bool? _lWeight = null;

        public bool? LWeight
        {
            get { return _lWeight; }
            private set
            {
                _lWeight = value;
            }
        }

        public bool? _lPassword = null;

        public bool? LPassword
        {
            get { return _lPassword; }
            private set
            {
                _lPassword = value;
            }
        }
        public bool? _lHardwareIntegration = true;

        public bool? LHardwareIntegration
        {
            get { return _lHardwareIntegration; }
            private set
            {
                _lHardwareIntegration = value;
            }
        }

        public string bool_LWeight
        {
            get { return LWeight.HasValue ? LWeight.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    LWeight = Convert.ToBoolean(value);
                }
            }
        }

        public string bool_LPassword
        {
            get { return LPassword.HasValue ? LPassword.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    LPassword = Convert.ToBoolean(value);
                }
            }
        }

        public bool? FPVisionValidation = false;

        public bool? _FPVisionValidation
        {
            get { return FPVisionValidation; }
            private set
            {
                FPVisionValidation = value;
            }
        }

        public string ComPort
        {
            get;
            set;
        }

        public int PoductionLineTime
        {
            get;
            set;
        }

        private decimal? _pallentinerWeight = null;

        public decimal? PallentinerWeight
        {
            get
            {
                return _pallentinerWeight;
            }
            private set
            {
                _pallentinerWeight = value;
            }
        }

        public string dec_PallentinerWeight
        {
            get { return PallentinerWeight.HasValue ? PallentinerWeight.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    PallentinerWeight = Convert.ToDecimal(value);
                }
            }
        }

        public string bool_LHardwareIntegration
        {
            get { return LHardwareIntegration.HasValue ? LHardwareIntegration.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    LHardwareIntegration = Convert.ToBoolean(value);
                }
            }
        }

        public string strPlantManagerEmailAddress
        {
            get;
            set;
        }

        public string isHardwareConnected
        {
            get;
            set;
        }

        public string stationNumber
        {
            get;
            set;
        }

        public string preshipmentQA
        {
            get;
            set;
        }

        public string innerPrinter
        {
            get;
            set;
        }

        public string OuterPrinter
        {
            get;
            set;
        }

        public string SecondLabelPrinter
        {
            get;
            set;
        }

        public string GCLabelPrinter
        {
            get;
            set;
        }

        public string FirstInkjet_Comport { get; set; }
        public string FirstInkjet_BaudRate { get; set; }
        public string FirstInkjet_Parity { get; set; }
        public string FirstInkjet_StopBits { get; set; }
        public string FirstInkjet_Timeout { get; set; }
        public string FirstInkjet_Retry { get; set; }

        public string SecondInkjet_Comport { get; set; }
        public string SecondInkjet_BaudRate { get; set; }
        public string SecondInkjet_Parity { get; set; }
        public string SecondInkjet_StopBits { get; set; }
        public string SecondInkjet_Timeout { get; set; }
        public string SecondInkjet_Retry { get; set; }

        public string OuteLabel_Comport { get; set; }
        public string OuteLabel_BaudRate { get; set; }
        public string OuteLabel_Parity { get; set; }
        public string OuteLabel_StopBits { get; set; }
        public string OuteLabel_DataBits { get; set; }

        public string ConMntScanner_Comport { get; set; }
        public string ConMntScanner_BaudRate { get; set; }
        public string ConMntScanner_Parity { get; set; }
        public string ConMntScanner_StopBits { get; set; }
        public string ConMntScanner_DataBits { get; set; }



        public string InnerLabel_Comport { get; set; }
        public string InnerLabel_BaudRate { get; set; }
        public string InnerLabel_Parity { get; set; }
        public string InnerLabel_StopBits { get; set; }
        public string InnerLabel_Databits { get; set; }
        public string SecondGradeExecutableFileLocation { get; set; }
        public string SecondGradeArgumentFileLocation { get; set; }
        public string SecondGradeTextFileLocation { get; set; }

        public string bool_FPVisionValidation
        {
            get { return _FPVisionValidation.HasValue ? _FPVisionValidation.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    _FPVisionValidation = Convert.ToBoolean(value);
                }
            }
        }

        public string VisionHostUrl { get; set; }
        #region WIP Stock Count for Scanner
        //Scanner Configuration for WIP Stock Count
        public string WIPScanDataExecutableFileLocation { get; set; }
        public string WIPScanDataArgumentFileLocation { get; set; }
        public string WIPScanDataTextFileLocation { get; set; }
        public string WIPScanDataArchieveTextFileLocation { get; set; }
        #endregion

        #region Tower Light
        //Configuration for Tower Light
        public string TowerLight_ComPort { get; set; }
        public string TowerLight_BaudRate { get; set; }
        public string TowerLight_Parity { get; set; }
        public string TowerLight_DataBit { get; set; }
        public string TowerLight_StopBits { get; set; }
        public string TowerLight_Timeout { get; set; }
        #endregion


        public string bool_SkipSarayaBarcodeValidation
        {
            get { return _skipBarcodeValidation.HasValue ? _skipBarcodeValidation.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    _skipBarcodeValidation = Convert.ToBoolean(value);
                }
            }
        }
        
        private static WorkStationDataConfiguration _instance = new WorkStationDataConfiguration();

        public static WorkStationDataConfiguration GetInstance()
        {
            return _instance;
        }

        #endregion

        public WorkStationDataConfiguration() { }

        internal override void UpdateInstanceWithJSON(string jsonData)
        {
            WorkStationDataConfiguration wsDTO = JsonHandler.JsonDeseailize<WorkStationDataConfiguration>(jsonData);

            UpdateAllFields(wsDTO as Configuration, _instance as Configuration);
        }

        public void ReInitInstance()
        {
            _instance = null;
            _instance = new WorkStationDataConfiguration();
        }

        internal override void FillConfigurableList(SortedDictionary<string, bool> lstConfItems)
        {
            FillConfigurableList(lstConfItems, _instance as Configuration);
        }
        internal override void UpdateField(string field, string value)
        {
            UpdateField(field, value, _instance as Configuration);
        }
        internal override Dictionary<string, object> GetAllPropertyValues()
        {
            return GetAllPropertyValues(_instance as Configuration);
        }


        public bool? skipBarcodeValidation = false;

        public bool? _skipBarcodeValidation
        {
            get { return skipBarcodeValidation; }
            private set
            {
                skipBarcodeValidation = value;
            }
        }

    }


    public class FloorSystemConfiguration : Configuration
    {
        private int? _plTimeConfiguration = Constants.ZERO;

        public int? PLTimeConfiguration
        {
            get
            {
                return _plTimeConfiguration;
            }

            private set
            {
                _plTimeConfiguration = value;
            }
        }



        public string intPLTimeConfiguration
        {
            get { return PLTimeConfiguration.HasValue ? Convert.ToString(PLTimeConfiguration.Value) : null; }
            set
            {
                PLTimeConfiguration = Convert.ToInt32(value);
            }

        }

        private bool? _ISchangeQctypeValidation = false;

        public bool? ISchangeQctypeValidation
        {
            get { return _ISchangeQctypeValidation; }
            private set { _ISchangeQctypeValidation = value; }
        }


        public string bool_ISchangeQctypeValidation
        {
            get { return ISchangeQctypeValidation.HasValue ? ISchangeQctypeValidation.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    ISchangeQctypeValidation = Convert.ToBoolean(value);
                }
            }
        }

        private string _eWareNaviBackupLocation = string.Empty;
        public string strEWareNaviBackupLocation
        {
            get { return _eWareNaviBackupLocation; }
            set
            {
                _eWareNaviBackupLocation = Convert.ToString(value);
            }
        }

        private int? _qaiExpiryDuration = Constants.ZERO;

        public int? QAIExpiryDuration
        {
            get { return _qaiExpiryDuration; }
            private set
            {
                _qaiExpiryDuration = value;
            }
        }

        public string intQAIExpiryDuration
        {
            get { return QAIExpiryDuration.HasValue ? Convert.ToString(QAIExpiryDuration.Value) : null; }
            set
            {
                QAIExpiryDuration = Convert.ToInt32(value);
            }
        }

        private int? _siteNumber = Constants.ZERO;

        public int? SiteNumber
        {
            get { return _siteNumber; }
            private set
            {
                _siteNumber = value;
            }
        }

        public string intSiteNumber
        {
            get { return SiteNumber.HasValue ? Convert.ToString(SiteNumber.Value) : null; }
            set
            {
                SiteNumber = Convert.ToInt32(value);
            }
        }

        private int? _logDeleteHours = Constants.ZERO;

        public int? LogDeleteHours
        {
            get { return _logDeleteHours; }
            private set
            {
                _logDeleteHours = value;
            }
        }

        public string IntLogDeleteHours
        {
            get { return LogDeleteHours.HasValue ? Convert.ToString(LogDeleteHours.Value) : null; }
            set
            {
                LogDeleteHours = Convert.ToInt32(value);
            }
        }



        private int? _pcsCountSmallScale = Constants.ZERO;
        public int? PcsCountSmallScale
        {
            get { return _pcsCountSmallScale; }
            private set
            {
                _pcsCountSmallScale = value;
            }
        }

        public string intPcsCountSmallScale
        {
            get { return PcsCountSmallScale.HasValue ? Convert.ToString(PcsCountSmallScale.Value) : null; }
            set
            {
                PcsCountSmallScale = Convert.ToInt32(value);
            }
        }

        public string intHBCReprintHours
        {
            get { return HBCReprintHours.HasValue ? Convert.ToString(HBCReprintHours.Value) : null; }
            set
            {
                HBCReprintHours = Convert.ToInt32(value);
            }
        }

        private bool? _isPasswordValidationFor10PcsRequired = true;

        public bool? IsPasswordValidationFor10PcsRequired
        {
            get { return _isPasswordValidationFor10PcsRequired; }
            private set
            {
                _isPasswordValidationFor10PcsRequired = value;
            }
        }

        public string boolIsPasswordValidationFor10PcsRequired
        {
            get { return Convert.ToString(IsPasswordValidationFor10PcsRequired); }
            set
            {
                if (value != null)
                {
                    IsPasswordValidationFor10PcsRequired = Convert.ToBoolean(value);
                }
            }
        }

        private int? _basketWeight = Constants.ZERO;

        public int? BasketWeight
        {
            get { return _basketWeight; }
            private set
            {
                _basketWeight = value;
            }
        }

        public string intBasketWeight
        {
            get { return BasketWeight.HasValue ? Convert.ToString(BasketWeight.Value) : null; }
            set
            {
                BasketWeight = Convert.ToInt32(value);
            }
        }

        private int? _prRefreshTime = Constants.ZERO;

        public int? PRRefreshTime
        {
            get { return _prRefreshTime; }
            private set
            {
                _prRefreshTime = value;
            }
        }

        public string intPRRefreshTime
        {
            get { return PRRefreshTime.HasValue ? Convert.ToString(PRRefreshTime.Value) : null; }
            set
            {
                PRRefreshTime = Convert.ToInt32(value);
            }
        }

        private int? _qmHrsDataDisplay = Constants.ZERO;

        public int? QMHrsDataDisplay
        {
            get { return _qmHrsDataDisplay; }
            private set
            {
                _qmHrsDataDisplay = value;
            }
        }

        public string intQMHrsDataDisplay
        {
            get { return QMHrsDataDisplay.HasValue ? Convert.ToString(QMHrsDataDisplay.Value) : null; }
            set
            {
                QMHrsDataDisplay = Convert.ToInt32(value);
            }
        }

        private int? _pdaHrsDataDisplay = Constants.ZERO;

        public int? PDAHrsDataDisplay
        {
            get { return _pdaHrsDataDisplay; }
            private set
            {
                _pdaHrsDataDisplay = value;
            }
        }

        public string intPDAHrsDataDisplay
        {
            get { return PDAHrsDataDisplay.HasValue ? Convert.ToString(PDAHrsDataDisplay.Value) : null; }
            set
            {
                PDAHrsDataDisplay = Convert.ToInt32(value);
            }
        }

        private int? _cdaHrsDataDisplay = Constants.ZERO;

        public int? CDAHrsDataDisplay
        {
            get { return _cdaHrsDataDisplay; }
            private set
            {
                _cdaHrsDataDisplay = value;
            }
        }

        public string intCDAHrsDataDisplay
        {
            get { return CDAHrsDataDisplay.HasValue ? Convert.ToString(CDAHrsDataDisplay.Value) : null; }
            set
            {
                CDAHrsDataDisplay = Convert.ToInt32(value);
            }
        }

        private bool? _EWareNaviSQL = false;

        public bool? EWareNaviSQL
        {
            get { return _EWareNaviSQL; }
            private set { _EWareNaviSQL = value; }
        }
        public string boolEWareNaviSQL
        {
            get { return EWareNaviSQL.HasValue ? EWareNaviSQL.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    EWareNaviSQL = Convert.ToBoolean(value);
                }
            }
        }

        private bool? _EWareNaviFileGenerator = false;

        public bool? EWareNaviFileGenerator
        {
            get { return _EWareNaviFileGenerator; }
            private set { _EWareNaviFileGenerator = value; }
        }
        public string boolEWareNaviFileGenerator
        {
            get { return EWareNaviFileGenerator.HasValue ? EWareNaviFileGenerator.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    EWareNaviFileGenerator = Convert.ToBoolean(value);
                }
            }
        }

        private string _eWareNaviFolderLocation = string.Empty;

        public string strEWareNaviFolderLocation
        {
            get { return _eWareNaviFolderLocation; }
            set
            {
                _eWareNaviFolderLocation = Convert.ToString(value);
            }
        }

        private int? _eWareNaviDuration = Constants.ZERO;

        public int? EWareNaviDuration
        {
            get { return _eWareNaviDuration; }
            private set
            {
                _eWareNaviDuration = value;
            }
        }

        public string intEWareNaviDuration
        {
            get { return EWareNaviDuration.HasValue ? Convert.ToString(EWareNaviDuration.Value) : null; }
            set
            {
                EWareNaviDuration = Convert.ToInt32(value);
            }
        }

        private string _qaPreshipmentEmailAddress = string.Empty;

        public string strQAPreshipmentEmailAddress
        {
            get { return _qaPreshipmentEmailAddress; }
            set
            {
                _qaPreshipmentEmailAddress = Convert.ToString(value);
            }
        }
        private string _strHartalegaAlertEmail = string.Empty;
        public string strHartalegaAlertEmail
        {
            get { return _strHartalegaAlertEmail; }
            set
            {
                _strHartalegaAlertEmail = Convert.ToString(value);
            }
        }


        private string _strEmailPassword = string.Empty;
        public string strEmailPassword
        {
            get { return _strEmailPassword; }
            set
            {
                _strEmailPassword = Convert.ToString(value);
            }
        }



        private string _strEmailUserId = string.Empty;
        public string strEmailUserId
        {
            get { return _strEmailUserId; }
            set
            {
                _strEmailUserId = Convert.ToString(value);
            }
        }

        private string _smtpPort = string.Empty;

        public string strSMTPPort
        {
            get { return _smtpPort; }
            set
            {
                _smtpPort = Convert.ToString(value);
            }
        }

        private string _smtpSender = string.Empty;

        public string strSMTPSender
        {
            get { return _smtpSender; }
            set
            {
                _smtpSender = Convert.ToString(value);
            }
        }

        private decimal? _maxBatchWeight = 0;

        public decimal? MaxBatchWeight
        {
            get { return _maxBatchWeight; }
            private set { _maxBatchWeight = value; }

        }

        public string dec_MaxBatchWeight
        {
            get { return MaxBatchWeight.HasValue ? MaxBatchWeight.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    MaxBatchWeight = Convert.ToDecimal(value);
                }
            }
        }


        private decimal? _minBatchWeight = 0;

        public decimal? MinBatchWeight
        {
            get { return _minBatchWeight; }
            private set
            {
                _minBatchWeight = value;
            }
        }

        public string dec_MinBatchWeight
        {
            get { return MinBatchWeight.HasValue ? MinBatchWeight.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    MinBatchWeight = Convert.ToDecimal(value);
                }
            }
        }

        private decimal? _wtMaxBatchWeight = 0;

        public decimal? WTMaxBatchWeight
        {
            get { return _wtMaxBatchWeight; }
            private set
            {
                _wtMaxBatchWeight = value;
            }
        }

        public string dec_WTMaxBatchWeight
        {
            get { return WTMaxBatchWeight.HasValue ? WTMaxBatchWeight.Value.ToString() : null; }
            set
            {
                if (value != null)
                {
                    WTMaxBatchWeight = Convert.ToDecimal(value);
                }
            }
        }

        private string _axConnectionString = string.Empty;

        public string strAXConnectionString
        {
            get { return _axConnectionString; }
            set
            {
                _axConnectionString = Convert.ToString(value);
            }
        }

        private string _axDomain = string.Empty;

        public string strAXDomain
        {
            get { return _axDomain; }
            set
            {
                _axDomain = Convert.ToString(value);
            }
        }

        private string _axDomainFullName = string.Empty;

        public string strAXDomainFullName
        {
            get { return _axDomainFullName; }
            set
            {
                _axDomainFullName = Convert.ToString(value);
            }
        }

        private string _axUserName = string.Empty;

        public string strAXUserName
        {
            get { return _axUserName; }
            set
            {
                _axUserName = Convert.ToString(value);
            }
        }

        private string _axPassword = string.Empty;

        public string strAXPassword
        {
            get { return _axPassword; }
            set
            {
                _axPassword = Convert.ToString(value);
            }
        }


        private int? _hbcReprintHours = Constants.ZERO;

        public int? HBCReprintHours
        {
            get { return _hbcReprintHours; }
            private set
            {
                _hbcReprintHours = value;
            }
        }


        private bool? _IScalculatedLooseQty = false;

        // #MH 10/11/2016 1.n FDD:HLTG-REM-003
        public bool? IScalculatedLooseQty
        {
            get { return _IScalculatedLooseQty; }
            private set { _IScalculatedLooseQty = value; }
        }

        public string boolIScalculatedLooseQty
        {
            get { return Convert.ToString(IScalculatedLooseQty); }
            set
            {
                if (value != null)
                {
                    IScalculatedLooseQty = Convert.ToBoolean(value);
                }
            }
        }

        private string _strWorkOrderNotifyEmail = string.Empty;

        public string strWorkOrderNotifyEmail
        {
            get { return Convert.ToString(_strWorkOrderNotifyEmail); }
            set
            {
                if (value != null)
                {
                    _strWorkOrderNotifyEmail = Convert.ToString(value);
                }
            }
        }

        #region AX new key
        private string _strAXCompany = string.Empty;

        public string strAXCompany
        {
            get { return _strAXCompany; }
            set
            {
                _strAXCompany = Convert.ToString(value);
            }
        }

        private string _strAXInnerPath = string.Empty;

        public string strAXInnerPath
        {
            get { return _strAXInnerPath; }
            set
            {
                _strAXInnerPath = Convert.ToString(value);
            }
        }

        private string _strAXOuterPath = string.Empty;

        public string strAXOuterPath
        {
            get { return _strAXOuterPath; }
            set
            {
                _strAXOuterPath = Convert.ToString(value);
            }
        }

        // YS 12/06/2018 to trace log
        private string _strFPLogPath = string.Empty;

        public string strFPLogPath
        {
            get { return _strFPLogPath; }
            set
            {
                _strFPLogPath = Convert.ToString(value);
            }
        }
        #endregion


        // #MH 10/11/2016 1.n FDD:HLTG-REM-003

        #region "TVReports"
        private int? _refreshTimeForReports = Constants.ZERO;

        public int? RefreshTimeForReports
        {
            get
            {
                return _refreshTimeForReports;
            }

            private set
            {
                _refreshTimeForReports = value;
            }
        }

        private int? _rotationTimeForReports = Constants.ZERO;

        public int? RotationTimeForReports
        {
            get
            {
                return _rotationTimeForReports;
            }

            private set
            {
                _rotationTimeForReports = value;
            }
        }

        public string intRefreshTimeForReports
        {
            get { return RefreshTimeForReports.HasValue ? Convert.ToString(RefreshTimeForReports.Value) : null; }
            set
            {
                RefreshTimeForReports = Convert.ToInt32(value);
            }

        }

        public string intRotationTimeForReports
        {
            get { return RotationTimeForReports.HasValue ? Convert.ToString(RotationTimeForReports.Value) : null; }
            set
            {
                RotationTimeForReports = Convert.ToInt32(value);
            }

        }

        private int? _batchesForTenPcsReport = Constants.ZERO;

        public int? BatchesForTenPcsReport
        {
            get
            {
                return _batchesForTenPcsReport;
            }

            private set
            {
                _batchesForTenPcsReport = value;
            }
        }
        public string intBatchesForTenPcsReport
        {
            get { return BatchesForTenPcsReport.HasValue ? Convert.ToString(BatchesForTenPcsReport.Value) : null; }
            set
            {
                BatchesForTenPcsReport = Convert.ToInt32(value);
            }

        }

        private int? _tenPcsReportHours = Constants.ZERO;
        public int? TenPcsReportHours
        {
            get
            {
                return _tenPcsReportHours;
            }

            private set
            {
                _tenPcsReportHours = value;
            }
        }
        public string intTenPcsReportHours
        {
            get { return TenPcsReportHours.HasValue ? Convert.ToString(TenPcsReportHours.Value) : null; }
            set
            {
                TenPcsReportHours = Convert.ToInt32(value);
            }

        }

        private int? _qaiReportHours = Constants.ZERO;
        public int? QaiReportHours
        {
            get
            {
                return _qaiReportHours;
            }

            private set
            {
                _qaiReportHours = value;
            }
        }
        public string intQaiReportHours
        {
            get { return QaiReportHours.HasValue ? Convert.ToString(QaiReportHours.Value) : null; }
            set
            {
                QaiReportHours = Convert.ToInt32(value);
            }

        }

        private int? _productionDefectReportHours = Constants.ZERO;
        public int? ProductionDefectReportHours
        {
            get
            {
                return _productionDefectReportHours;
            }

            private set
            {
                _productionDefectReportHours = value;
            }
        }
        public string intProductionDefectReportHours
        {
            get { return ProductionDefectReportHours.HasValue ? Convert.ToString(ProductionDefectReportHours.Value) : null; }
            set
            {
                ProductionDefectReportHours = Convert.ToInt32(value);
            }

        }

        private int? _productionDefectDPM = Constants.ZERO;
        public int? ProductionDefectDPM
        {
            get
            {
                return _productionDefectDPM;
            }

            private set
            {
                _productionDefectDPM = value;
            }
        }
        public string intProductionDefectDPM
        {
            get { return ProductionDefectDPM.HasValue ? Convert.ToString(ProductionDefectDPM.Value) : null; }
            set
            {
                ProductionDefectDPM = Convert.ToInt32(value);
            }

        }

        private int? _cosmeticDefectReportHours = Constants.ZERO;
        public int? CosmeticDefectReportHours
        {
            get
            {
                return _cosmeticDefectReportHours;
            }

            private set
            {
                _cosmeticDefectReportHours = value;
            }
        }
        public string intCosmeticDefectReportHours
        {
            get { return CosmeticDefectReportHours.HasValue ? Convert.ToString(CosmeticDefectReportHours.Value) : null; }
            set
            {
                CosmeticDefectReportHours = Convert.ToInt32(value);
            }

        }

        private int? _cosmeticDefectDPM = Constants.ZERO;
        public int? CosmeticDefectDPM
        {
            get
            {
                return _cosmeticDefectDPM;
            }

            private set
            {
                _cosmeticDefectDPM = value;
            }
        }
        public string intCosmeticDefectDPM
        {
            get { return CosmeticDefectDPM.HasValue ? Convert.ToString(CosmeticDefectDPM.Value) : null; }
            set
            {
                CosmeticDefectDPM = Convert.ToInt32(value);
            }

        }

        private int? _qaiPageSize = Constants.ZERO;
        public int? QAIPageSize
        {
            get
            {
                return _qaiPageSize;
            }

            private set
            {
                _qaiPageSize = value;
            }
        }
        public string intQAIPageSize
        {
            get { return QAIPageSize.HasValue ? Convert.ToString(QAIPageSize.Value) : null; }
            set
            {
                QAIPageSize = Convert.ToInt32(value);
            }

        }

        private int? _qcDefectReportDays = Constants.ZERO;
        public int? QCDefectReportDays
        {
            get
            {
                return _qcDefectReportDays;
            }

            private set
            {
                _qcDefectReportDays = value;
            }
        }
        public string intQCDefectReportDays
        {
            get { return QCDefectReportDays.HasValue ? Convert.ToString(QCDefectReportDays.Value) : null; }
            set
            {
                QCDefectReportDays = Convert.ToInt32(value);
            }

        }

        private string _tvReportsHistoryFolderPath = string.Empty;

        public string TvReportsHistoryFolderPath
        {
            get { return _tvReportsHistoryFolderPath; }
            set
            {
                _tvReportsHistoryFolderPath = Convert.ToString(value);
            }
        }
        #endregion

        #region Reprint Outer Case Date Range Configuration

        private int? _ReprintOuterCaseDayRange = Constants.ZERO;

        public int? ReprintOuterCaseDayRange
        {
            get
            {
                return _ReprintOuterCaseDayRange;
            }

            private set
            {
                _ReprintOuterCaseDayRange = value;
            }
        }

        public string intReprintOuterCaseDayRange
        {
            get { return ReprintOuterCaseDayRange.HasValue ? Convert.ToString(ReprintOuterCaseDayRange.Value) : null; }
            set
            {
                ReprintOuterCaseDayRange = Convert.ToInt32(value);
            }

        }

        #endregion

        #region  Valid Closed PO Day Range Configuration for PO Transfer

        private int? _ValidClosedPODayRangePOTransfer = Constants.ZERO;

        public int? ValidClosedPODayRangePOTransfer
        {
            get
            {
                return _ValidClosedPODayRangePOTransfer;
            }

            private set
            {
                _ValidClosedPODayRangePOTransfer = value;
            }
        }

        public string intValidClosedPODayRangePOTransfer
        {
            get { return ValidClosedPODayRangePOTransfer.HasValue ? Convert.ToString(ValidClosedPODayRangePOTransfer.Value) : null; }
            set
            {
                ValidClosedPODayRangePOTransfer = Convert.ToInt32(value);
            }

        }

        #endregion

        #region eFloor System Final Pack Date Range Configuration

        private int? _eFloorSystemFinalPackDayRange = Constants.ZERO;

        public int? eFloorSystemFinalPackDayRange
        {
            get
            {
                return _eFloorSystemFinalPackDayRange;
            }

            private set
            {
                _eFloorSystemFinalPackDayRange = value;
            }
        }

        public string intEFloorSystemFinalPackDayRange
        {
            get { return eFloorSystemFinalPackDayRange.HasValue ? Convert.ToString(eFloorSystemFinalPackDayRange.Value) : null; }
            set
            {
                eFloorSystemFinalPackDayRange = Convert.ToInt32(value);
            }

        }

        #endregion

        #region eFloor System IT Role Configuration

        private string _ITRole = string.Empty;

        public string strITRole
        {
            get { return _ITRole; }
            set { _ITRole = value; }

        }

        #endregion

        private static FloorSystemConfiguration _instance = new FloorSystemConfiguration();

        public static FloorSystemConfiguration GetInstance()
        {
            return _instance;
        }



        private FloorSystemConfiguration() { }



        internal override void UpdateInstanceWithJSON(string jsonData)
        {
            FloorSystemConfiguration fsDTO = JsonHandler.JsonDeseailize<FloorSystemConfiguration>(jsonData);

            UpdateAllFields(fsDTO as Configuration, _instance as Configuration);
        }


        internal override void FillConfigurableList(SortedDictionary<string, bool> lstConfItems)
        {
            FillConfigurableList(lstConfItems, _instance as Configuration);
        }

        internal override Dictionary<string, object> GetAllPropertyValues()
        {
            return GetAllPropertyValues(_instance as Configuration);
        }

        internal override void UpdateField(string field, string value)
        {
            UpdateField(field, value, _instance as Configuration);
        }
    }

    public class WorkStationDTO
    {
        public string Location { get; set; }
        public string Site { get; set; }
        public string WorkStationNumber { get; set; }
        //public string ComPort { get; set; }
        //public decimal PallentinerWeight { get; set; }
        public int LocationId { get; set; }
        public string WorkStationId { get; set; }
        public string Area { get; set; }

        public string LocationAreaCode { get; set; }
        public string Module { get; set; }
        public string SubModule { get; set; }

        public string ModuleIds { get; set; }

        public bool IsAdmin { get; set; }

        public int LocationAreaId { get; set; }

        private static WorkStationDTO _instance = new WorkStationDTO();

        public static WorkStationDTO GetInstance()
        {
            return _instance;
        }

        private WorkStationDTO() { }
    }
}
