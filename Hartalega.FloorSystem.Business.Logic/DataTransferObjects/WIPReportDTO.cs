using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class WIPReportDTO
    {
        /// <summary>
        /// Intantiate the class
        /// </summary>
        static WIPReportDTO()
        {

        }

        /// <summary>
        /// WIP Report ID
        /// </summary>
        private int _WIPReportID;

        public int WIPReportID
        {
            get { return _WIPReportID; }
            set { _WIPReportID = value; }
        }

        /// <summary>
        /// Report Name
        /// </summary>
        private string _ReportName;

        public string ReportName
        {
            get { return _ReportName; }
            set { _ReportName = value; }
        }

        /// <summary>
        /// Report URL
        /// </summary>
        private string _ReportURL;

        public string ReportURL
        {
            get { return _ReportURL; }
            set { _ReportURL = value; }
        }

        /// <summary>
        /// Report Parameter
        /// </summary>
        private string _ReportParam;

        public string ReportParam
        {
            get { return _ReportParam; }
            set { _ReportParam = value; }
        }

        /// <summary>
        /// Report Display Order
        /// </summary>
        private int _DisplayOrder;

        public int DisplayOrder
        {
            get { return _DisplayOrder; }
            set { _DisplayOrder = value; }
        }
    }
}
