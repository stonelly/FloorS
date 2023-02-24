using Hartalega.FloorSystem.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic
{
  

    /// <summary>
    ///
    ///</summary>
    ///
    public class ConfMgr
    {

        /// <summary>
        ///
        ///</summary>
        ///
        private List<LocationDTO> _lstLocation = new List<LocationDTO>();

        private List<LocationDTO> _lstLocationArea = new List<LocationDTO>();

        public List<LocationDTO> LstLocations
        {
            get { return _lstLocation; }           
        }

        public List<LocationDTO> LstLocationArea
        {
            get { return _lstLocationArea; }
        }



        /// <summary>
        ///
        ///</summary>
        private SortedDictionary<string, bool> _confItems=null;

        private SortedDictionary<string, bool> _sysConfItems = null;

        /// <summary>
        ///
        ///</summary>
        private readonly string _subSystem = "WSConfMgrBLL";

        /// <summary>
        ///
        ///</summary>
        private Dictionary<int, string> _moduleList = new Dictionary<int, string>();
                

        /// <summary>
        ///
        ///</summary>
        private Dictionary<int, string> _selectedModuleDetails = new Dictionary<int,string>();

        /// <summary>
        ///
        ///</summary>
        public Dictionary<int, string> SelectedModuleDetails
        {
            get { return _selectedModuleDetails; }
        }

        /// <summary>
        ///
        ///</summary>
        private Dictionary<int, string> _unSelectedModuleDetails = null;
                

        /// <summary>
        ///
        ///</summary>
        
        private void UpdateSelectedAndUnSelectedModuleList(WorkStationConfigurationData confData)
        {
             string [] lstModules = confData.ModuleIds.TrimEnd(',').Split(',');
             _selectedModuleDetails.Clear();
             _unSelectedModuleDetails.Clear();
            foreach (string module in lstModules.Distinct())
            {
                int moduleId = Convert.ToInt32(module);
                string moduleName;
                _moduleList.TryGetValue(moduleId, out moduleName);
                _selectedModuleDetails.Add(moduleId, moduleName);
            }
            
            IEnumerable<KeyValuePair <int,string>> unSelectedItems =  _moduleList.Except(_selectedModuleDetails);
            foreach (KeyValuePair<int, string> item in unSelectedItems)
            {
                _unSelectedModuleDetails.Add(item.Key,item.Value);
            }
        }
        
        /// <summary>
        ///
        ///</summary>
        private List<WorkStationData> _lstWorkStations = new List<WorkStationData>();

        /// <summary>
        ///
        ///</summary>
        private List<WorkStationData> _lstUnSelectedWS = null;

        /// <summary>
        ///
        ///</summary>
        public List<WorkStationData> LstUnSelectedWS
        {
            get
            {
                if (_lstUnSelectedWS == null)
                {
                    _lstUnSelectedWS = new List<WorkStationData>(_lstWorkStations);
                }

                return _lstUnSelectedWS;
            }
        }

        /// <summary>
        ///
        ///</summary>
        private List<WorkStationData> _lstSelectedWS = new List<WorkStationData>();

        /// <summary>
        ///
        ///</summary>
        public List<WorkStationData> LstSelectedWS
        {
            get
            {
                return _lstSelectedWS;
            }
        }


        /// <summary>
        ///
        ///</summary>
        private List<WorkStationConfigurationData> _lstWSConfData = new List<WorkStationConfigurationData>();

        private string GetSelectedWS()
        {
            if (_lstSelectedWS != null && _lstSelectedWS.Count > 0)
            {
                StringBuilder selWS = new StringBuilder();
                foreach (WorkStationData wsData in _lstSelectedWS)
                {
                    selWS.Append(wsData.WsId);
                    selWS.Append(",");
                }
                return selWS.Remove(selWS.Length - 1, 1).ToString();
            }
            return null;
        }

        private string GetSelectedModules()
        {
            StringBuilder selectedModules = new StringBuilder();
            foreach (KeyValuePair<int, string> module in _selectedModuleDetails)
            {
                selectedModules.Append(module.Key);
                selectedModules.Append(",");
            }

            return selectedModules.Remove(selectedModules.Length - 1, 1).ToString();
        }
        /// <summary>
        ///
        ///</summary>
        public List<WorkStationConfigurationData> LstWSConfData
        {
            get { return _lstWSConfData; }            
        }

        private void UpdateSelectedAndUnSelectedWorkStatList(WorkStationConfigurationData wsData)
        {
            _lstSelectedWS = new List<WorkStationData>(wsData.LstWorkStationForConf);            
            _lstUnSelectedWS = new List<WorkStationData>(_lstWorkStations.Except(_lstSelectedWS));
            
 
        }


        #region Public Methods
        /// <summary>
        /// 
        /// </summary>

        public LocationDTO GetLocationDetailsForWS(WorkStationData wsData)
        {
            return _lstLocation.Find(x => x.LocationId == wsData.LocationId);
        }

        public bool IsExistingConfiguration(string confName)
        {
            return _lstWSConfData.Exists(x => String.Equals(x.ConfName, confName)); 
        }

        public bool VerifyLocationForSelectedWS(LocationDTO location,string areaId)
        {
            bool isDifferentLocation = false;

            foreach (WorkStationData wsData in _lstSelectedWS)
            {
                if (wsData.LocationId != location.LocationId || wsData.AreaId!=areaId)
                {
                    isDifferentLocation = true;
                    break;
                }
            }
            return isDifferentLocation;
        }

        public Dictionary<int, string> UnSelectedModuleDetails
        {
            get
            {
                if (_unSelectedModuleDetails == null)
                {
                    _unSelectedModuleDetails = new Dictionary<int, string>(_moduleList);
                }
                return _unSelectedModuleDetails;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name ="wsData">WorkStationConfigurationData wsData</param>
        /// <returns></returns>
        public void SetWSDataFields(int index)
        {
            WorkStationConfigurationData wsData = _lstWSConfData[index];
            WorkStationDataConfiguration.GetInstance().UpdateInstanceWithJSON(wsData.ConfData);
            UpdateSelectedAndUnSelectedModuleList(wsData);
            UpdateSelectedAndUnSelectedWorkStatList(wsData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name ="field">string field</param>
        /// <param name ="value">string value</param>
        /// <returns></returns>
        public void UpdateSystemConfFiled(string field, string value)
        {
            FloorSystemConfiguration.GetInstance().UpdateField(field, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name ="field">string field</param>
        /// <param name ="value">string value</param>
        /// <returns></returns>
        public void UpdateWSConfFiled(string field, string value)
        {
            WorkStationDataConfiguration.GetInstance().UpdateField(field, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns> IEnumerable<string> </returns>
        public IEnumerable<string> GetConfigurableItems()
        {
            if (_confItems == null)
            {
                _confItems = new SortedDictionary<string, bool>();
                WorkStationDataConfiguration.GetInstance().FillConfigurableList(_confItems);
            }
            return _confItems.Where(x => x.Value == false).Select(x => x.Key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns> IEnumerable<string> </returns>
        public IEnumerable<string> GetSystemConfigurableItems()
        {
            if (_sysConfItems == null)
            {
                _sysConfItems = new SortedDictionary<string, bool>();
                FloorSystemConfiguration.GetInstance().FillConfigurableList(_sysConfItems);
            }
            return _sysConfItems.Where(x => x.Value == false).Select(x => x.Key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        public void ClearConfItems()
        {
            _confItems = null;
            WorkStationDataConfiguration.GetInstance().ReInitInstance();
            _selectedModuleDetails.Clear();
            _unSelectedModuleDetails.Clear();
            _lstSelectedWS = null;
            _lstUnSelectedWS = null;
        }

        public void ClearWorkStations()
        {
            _lstUnSelectedWS = null;
            _lstSelectedWS = new List<WorkStationData>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name ="Key">string Key</param>
        /// <returns></returns>

        public void SetSelectedKey(string key, bool isSystemConf)
        {
            if (isSystemConf)
            {
                _sysConfItems[key] = true;
            }
            else
            {
                _confItems[key] = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns>Dictionary<string, object></returns>
        public Dictionary<string, object> GetAllWSConfValues()
        {
            return WorkStationDataConfiguration.GetInstance().GetAllPropertyValues();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns>Dictionary<string, object></returns>
        public Dictionary<string, object> GetAllSystemConfValues()
        {
            return FloorSystemConfiguration.GetInstance().GetAllPropertyValues();
        }

        public void RemoveSelectedModule(object selectedModuleItem)
        {
            KeyValuePair<int, string> moduleItem = (KeyValuePair<int, string>)selectedModuleItem;
            _selectedModuleDetails.Remove(moduleItem.Key);
            _unSelectedModuleDetails.Add(moduleItem.Key, moduleItem.Value);
        }

        /// <summary>
        ///
        ///</summary>

        public void AddSelectedModule(object selectedModuleItem)
        {
            KeyValuePair<int, string> moduleItem = (KeyValuePair<int, string>)selectedModuleItem;
            _unSelectedModuleDetails.Remove(moduleItem.Key);
            _selectedModuleDetails.Add(moduleItem.Key, moduleItem.Value);
        }

        public void RemoveSelectedWorkStation(object selectedWSItem, int confIndex)
        {

            WorkStationData wsData = selectedWSItem as WorkStationData;
            _lstUnSelectedWS.Add(wsData);
            _lstSelectedWS.Remove(wsData);
            if (confIndex >= 0)
            {
                _lstWSConfData[confIndex].LstWorkStationForConf.Remove(wsData);
            }
        }

        /// <summary>
        ///
        ///</summary>

        public void AddSelectedWorkStation(object selectedWSItem, int confIndex)
        {
            WorkStationData wsData = selectedWSItem as WorkStationData;
            _lstUnSelectedWS.Remove(wsData);
            _lstSelectedWS.Add(wsData);
            if (confIndex >= 0)
            {
                _lstWSConfData[confIndex].LstWorkStationForConf.Add(wsData);
            }
        }

        #endregion
        #region Data Load
        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        public void LoadModuleData()
        {
            SqlDataReader moduleData = FloorDBAccess.ExecuteReader("usp_getModuleNamesAndId", new List<FloorSqlParameter>());

            while (moduleData.Read())
            {
                int moduleId = int.Parse(FloorDBAccess.GetString(moduleData, "ModuleId"));
                string moduleName = FloorDBAccess.GetString(moduleData, "ModuleName");
                _moduleList.Add(moduleId, moduleName);
            }

            moduleData.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        public void LoadWorkStationAndConfData()
        {
            SqlDataReader wsDetails = FloorDBAccess.ExecuteReader("usp_getWSConfDetails", new List<FloorSqlParameter>());
            _lstWorkStations = new List<WorkStationData>();
            _lstWSConfData = new List<WorkStationConfigurationData>();

            try
            {
                while (wsDetails.Read())
                {
                    WorkStationData wsData = null;
                    WorkStationConfigurationData wsConfData = null;

                    int wsID = FloorDBAccess.GetValue<int>(wsDetails, "WorkstationId");
                    if (wsID != 0)
                    {
                        wsData = new WorkStationData();

                        wsData.WsId = wsID;
                        wsData.WsName = FloorDBAccess.GetString(wsDetails, "WorkStationName");
                        wsData.LocationId = FloorDBAccess.GetValue<int>(wsDetails, "LocationId");
                        wsData.AreaId = FloorDBAccess.GetString(wsDetails, "AreaCode");
                        wsData.AreaCode = FloorDBAccess.GetString(wsDetails, "AreaName");
                        wsData.LocationName = FloorDBAccess.GetString(wsDetails, "LocationName");
                        wsData.ConfigName = FloorDBAccess.GetString(wsDetails, "ConfigurationName");
                        _lstWorkStations.Add(wsData);
                    }



                    int wsConfId = Convert.ToInt32(FloorDBAccess.GetString(wsDetails, "ConfigurationId"));

                    if (wsConfId != 0)
                    {
                        wsConfData = _lstWSConfData.Find(x => x.ConfId == wsConfId);
                        if (wsConfData == null)
                        {
                            wsConfData = new WorkStationConfigurationData();
                            wsConfData.ConfId = wsConfId;
                            wsConfData.ConfName = FloorDBAccess.GetString(wsDetails, "ConfigurationName");
                            wsConfData.ConfData = FloorDBAccess.GetString(wsDetails, "ConfigurationData");
                            wsConfData.ModuleIds = FloorDBAccess.GetString(wsDetails, "ModuleIds");
                            _lstWSConfData.Add(wsConfData);
                        }
                        if (wsData != null)
                        {
                            wsConfData.LstWorkStationForConf.Add(wsData);
                        }
                    }

                }

                wsDetails.Close();
            }
            catch (IndexOutOfRangeException iorEx)
            {
                throw new FloorSystemException(Messages.EX_COLUMN_NOT_FOUND, _subSystem, iorEx);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.EX_GENERIC_SYDB_EXCEPTION, _subSystem, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        public void LoadAllLocations()
        {
            SqlDataReader locDetails = FloorDBAccess.ExecuteReader("usp_getLocationData", new List<FloorSqlParameter>());

            while (locDetails.Read())
            {
                LocationDTO locData = null;
                int locationId = FloorDBAccess.GetValue<int>(locDetails, "LocationId");
                if (locationId != 0)
                {
                    locData = new LocationDTO();
                    locData.LocationId = locationId;
                    locData.LocationName = FloorDBAccess.GetString(locDetails, "LocationName");
                    locData.CompanyName = FloorDBAccess.GetString(locDetails, "CompanyName");
                    locData.IsDeleted = FloorDBAccess.GetValue<bool>(locDetails, "IsDeleted");
                    locData.Zone = FloorDBAccess.GetString(locDetails, "ZoneName");
                    locData.Area = FloorDBAccess.GetString(locDetails, "Area");
                    locData.LocationAreaCode = FloorDBAccess.GetString(locDetails, "LocationAreaCode");

                    _lstLocation.Add(locData);
                }
            }

            locDetails.Close();
        }

        public void LoadAreaLocations()
        {
            SqlDataReader locDetails = FloorDBAccess.ExecuteReader("usp_getAreaLocationMapping", new List<FloorSqlParameter>());

            while (locDetails.Read())
            {
                LocationDTO locData = null;
               
                    locData = new LocationDTO();
                    locData.LocationId = FloorDBAccess.GetValue<int>(locDetails, "LocationId"); 
                    locData.LocationName = FloorDBAccess.GetString(locDetails, "LocationName");
                    locData.Area = FloorDBAccess.GetString(locDetails, "AreaCode");
                    locData.AreaId = FloorDBAccess.GetValue<int>(locDetails, "AreaID");
                    _lstLocationArea.Add(locData);
            }

            locDetails.Close();
        }

        public void LoadSystemConfigurationData()
        {
            string systemConfData = FloorDBAccess.ExecuteScalar("usp_GetSystemConf", new List<FloorSqlParameter>()) as string;

            if (systemConfData != null)
            {
                FloorSystemConfiguration.GetInstance().UpdateInstanceWithJSON(systemConfData);
            }

        }

        #endregion
        #region Data Save

        public void UpdateLocationInformation(LocationDTO location, string AreaCode)
        {
            List<FloorSqlParameter> sqlParams = new List<FloorSqlParameter>();
            sqlParams.Add(new FloorSqlParameter("@wsIds", GetSelectedWS()));
            sqlParams.Add(new FloorSqlParameter("@locationId", location.LocationId));
            sqlParams.Add(new FloorSqlParameter("@AreaCode", AreaCode)); 

            FloorDBAccess.ExecuteNonQuery("usp_UpdLocData", sqlParams);

            foreach (WorkStationData wsData in _lstSelectedWS)
            {
                wsData.AreaId = AreaCode;
                wsData.LocationId = location.LocationId;
            }

        }

        public bool ApplyConfiguration(string confName)
        {
            string confData = JsonHandler.JsonSerialize<WorkStationDataConfiguration>(WorkStationDataConfiguration.GetInstance());
            string moduleIds = GetSelectedModules();
            int confId = 0;
            List<FloorSqlParameter> sqlParams = new List<FloorSqlParameter>();
            sqlParams.Add(new FloorSqlParameter("@ConfName", confName));
            sqlParams.Add(new FloorSqlParameter("@wsName", GetSelectedWS()));
            sqlParams.Add(new FloorSqlParameter("@modules", moduleIds));
            sqlParams.Add(new FloorSqlParameter("@confData", confData));
            sqlParams.Add(new FloorSqlParameter("@confId", System.Data.SqlDbType.Int));
            FloorDBAccess.ExecuteNonQuery("usp_AddConfData", sqlParams);

            confId = (int)sqlParams[sqlParams.Count - 1].ParamaterValue;
            WorkStationConfigurationData wsConfData = _lstWSConfData.Find(x => confId == x.ConfId);
            bool isNewConfiguration = wsConfData == null ? true : false;
            if (wsConfData == null)
            {
                wsConfData = new WorkStationConfigurationData();
                wsConfData.ConfData = confData;
                wsConfData.ConfName = confName;
                wsConfData.ModuleIds = moduleIds;
                wsConfData.ConfId = confId;
                wsConfData.LstWorkStationForConf = _lstSelectedWS;
                _lstWSConfData.Add(wsConfData);
            }

            else
            {
                wsConfData.ConfData = confData;
                wsConfData.ModuleIds = moduleIds;
            }

            try
            {
                if(UpdateWorkstationCOnfiguration(confId, GetSelectedWS())>0)
                {
                    isNewConfiguration = true;
                }
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message);
            }

            return isNewConfiguration;

           

        }

        public int UpdateWorkstationCOnfiguration(int ConfigId, string WorkstationIds)
        {
             List<FloorSqlParameter> sqlParams = new List<FloorSqlParameter>();
            sqlParams.Add(new FloorSqlParameter("@ConfigurationId", ConfigId));
            sqlParams.Add(new FloorSqlParameter("@workstationList", WorkstationIds));
          return  FloorDBAccess.ExecuteNonQuery("usp_UPDATE_WorkstationConfiguration", sqlParams);
        }


        public void SaveSystemConfiguration()
        {
            List<FloorSqlParameter> sqlParams = new List<FloorSqlParameter>();
            sqlParams.Add(new FloorSqlParameter("@confData",JsonHandler.JsonSerialize<FloorSystemConfiguration>(FloorSystemConfiguration.GetInstance())));
            FloorDBAccess.ExecuteNonQuery("usp_UpdSystemConf", sqlParams);
        }
        #endregion

        // <summary>
        /// Get Configid through workstationname
        /// </summary>
        /// <param name="workStationName"></param>

        public string GetSelectWorkStation(string workStationName)
        {
            //CommonBLL.FillCache();
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@workstationName", workStationName));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_Select_WorkStationDetails", PrmList));
        }
    }
}
