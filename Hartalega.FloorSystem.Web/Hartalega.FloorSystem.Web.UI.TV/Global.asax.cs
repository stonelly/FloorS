using System;
using System.Web.Caching;
using System.Web;
using System.Collections.Generic;
using System.Web.Routing;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using System.Data;
using Hartalega.FloorSystem.Framework;
// -----------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Web.UI.TV
{
    /// <summary>
    /// Global file to maintain application and session objects event handler
    /// </summary>
    public class Global : HttpApplication
    {

        ///Static Variables
        public static List<DropdownDTO> _locationList = null;
        public static int _refreshTime = Constants.ZERO;
        public static int _rotationTime = Constants.ZERO;
        public static string _dateformat = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
        public static string _batchJobLastRunTime;
        public static string _batchJobNextRunTime;
        public static string _currentReportTime;
               
        /// <summary>
        /// Application start event
        /// </summary>
        /// <param name="sender">Request object</param>
        /// <param name="e">Event Argument</param>
        #region
        public void Application_Start(object sender, EventArgs e)
        {
            // Dynamically create new timer
            CommonBLL.GetFloorSystemConfiguration();
            _refreshTime = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intRefreshTimeForReports);
            _rotationTime = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intRotationTimeForReports);
            _batchJobLastRunTime = TVReportsBLL.GetLastBatchJobRunTime();
            _batchJobLastRunTime = TVReportsBLL.GetNextBatchJobRunTime();
            _currentReportTime = CommonBLL.GetCurrentDateAndTimeFromServer().ToString(Global._dateformat);
            System.Timers.Timer timScheduledTask = new System.Timers.Timer();
            timScheduledTask.Interval = _refreshTime * Constants.SIXTY * Constants.THOUSAND;
            timScheduledTask.Enabled = true;
            timScheduledTask.Elapsed +=
            new System.Timers.ElapsedEventHandler(timScheduledTask_Elapsed);
            timScheduledTask.Stop();
            timScheduledTask.Start();
            StartActivities();
        }

        void timScheduledTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StartActivities();
        }

        void StartActivities()
        {
            if (_locationList == null)
                _locationList = CommonBLL.GetLocationIDandName();
            CommonBLL.GetFloorSystemConfiguration();
            _refreshTime = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intRefreshTimeForReports);
            _rotationTime = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intRotationTimeForReports);
            _batchJobLastRunTime = TVReportsBLL.GetLastBatchJobRunTime();
            _batchJobNextRunTime = TVReportsBLL.GetNextBatchJobRunTime();
            _currentReportTime = CommonBLL.GetCurrentDateAndTimeFromServer().ToString(Global._dateformat);
             DataTable dtReports = new DataTable();
            if (HttpRuntime.Cache["QAIData"] == null)
            {
                System.Web.Caching.Cache cacheObject = System.Web.HttpContext.Current.Cache;
                dtReports = TVReportsBLL.GetQAIMonitoringData();
                cacheObject.Insert("QAIData", dtReports, null, Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
                //add by Cheah (27/02/2017)
                dtReports = TVReportsBLL.GetQAIPercentData();
                cacheObject.Insert("QAIPctData", dtReports, null, Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
                //end add
                dtReports = TVReportsBLL.GetTenPcsWeightData();
                cacheObject.Insert("TenPcsData", dtReports, null, Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
                dtReports = TVReportsBLL.GetQCDefectData();
                if (dtReports != null)
                    cacheObject.Insert("QCDefectData", dtReports, null, Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
                dtReports = TVReportsBLL.GetCosmeticDefectData();
                cacheObject.Insert("CosmeticDefectData", dtReports, null, Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
                dtReports = TVReportsBLL.GetProdDefectData();
                cacheObject.Insert("ProdDefectData", dtReports, null, Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
                dtReports = TVReportsBLL.GetColor();
                cacheObject.Insert("ColorData", dtReports, null, Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);

            }
            else
            {
                dtReports = TVReportsBLL.GetQAIMonitoringData();
                HttpRuntime.Cache["QAIData"] = dtReports;
                //add by Cheah (27/02/2017)
                dtReports = TVReportsBLL.GetQAIPercentData();
                HttpRuntime.Cache["QAIPctData"] = dtReports;
                //end add
                dtReports = TVReportsBLL.GetTenPcsWeightData();
                HttpRuntime.Cache["TenPcsData"] = dtReports;
                dtReports = TVReportsBLL.GetQCDefectData();
                HttpRuntime.Cache["QCDefectData"] = dtReports;
                dtReports = TVReportsBLL.GetCosmeticDefectData();
                HttpRuntime.Cache["CosmeticDefectData"] = dtReports;
                dtReports = TVReportsBLL.GetProdDefectData();
                HttpRuntime.Cache["ProdDefectData"] = dtReports;
                dtReports = TVReportsBLL.GetColor();
                HttpRuntime.Cache["ColorData"] = dtReports;

            }
        }
        #endregion

        /// <summary>
        /// Application start event
        /// </summary>
        /// <param name="sender">Request object</param>
        /// <param name="e">Event Argument</param>
        #region
        public void Application_End(object sender, EventArgs e)
        {
            // Code that runs on application shutdown
        }
        #endregion

        /// <summary>
        /// Application start event
        /// </summary>
        /// <param name="sender">Request object</param>
        /// <param name="e">Event Argument</param>
        #region
        public void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
        }
        #endregion
    }
}
