using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    ///
    ///</summary>
    public class WorkStationConfigurationData
    {
        /// <summary>
        ///
        ///</summary>
        private int _confId;

        /// <summary>
        ///
        ///</summary>
        public int ConfId
        {
            get { return _confId; }
            internal set { _confId = value; }
        }

        /// <summary>
        ///
        ///</summary>
        private string _confName;


        /// <summary>
        ///
        ///</summary>
        public string ConfName
        {
            get { return _confName; }
            internal set { _confName = value; }
        }

        /// <summary>
        ///
        ///</summary>
        private string _moduleIds;

        /// <summary>
        ///
        ///</summary>
        public string ModuleIds
        {
            get { return _moduleIds; }
            internal set { _moduleIds = value; }
        }

        /// <summary>
        ///
        ///</summary>
        private string _confData;

        /// <summary>
        ///
        ///</summary>
        public string ConfData
        {
            get { return _confData; }
            internal set { _confData = value; }
        }

        /// <summary>
        /// List of work stations in this configuration
        ///</summary>
        private List<WorkStationData> _lstWorkStations = new List<WorkStationData>();


        internal List<WorkStationData> LstWorkStationForConf
        {
            get { return _lstWorkStations; }
            set { _lstWorkStations = value; }
        }

    }



    /// <summary>
    ///
    ///</summary>
    public class WorkStationData
    {
        /// <summary>
        ///
        ///</summary>
        private int _wsId;

        /// <summary>
        ///
        ///</summary>
        public int WsId
        {
            get { return _wsId; }
            internal set { _wsId = value; }
        }

        /// <summary>
        ///
        ///</summary>
        private string _wsName;

        public string WsName
        {
            get { return _wsName; }
            internal set { _wsName = value; }
        }

        /// <summary>
        ///
        ///</summary>
        private int _locationId;

        public int LocationId
        {
            get { return _locationId; }
            internal set { _locationId = value; }
        }

        /// <summary>
        ///
        ///</summary>
        private string _areaId;

        public string AreaId
        {
            get { return _areaId; }
            internal set { _areaId = value; }
        }

        private string _areaCode;

        public string AreaCode
        {
            get { return _areaCode; }
            internal set { _areaCode = value; }
        }

        private string _locationName;

        public string LocationName
        {
            get { return _locationName; }
            internal set { _locationName = value; }
        }


        private string _configName;

        public string ConfigName
        {
            get { return _configName; }
            internal set { _configName = value; }
        }

       

    }
}
