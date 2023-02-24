using System;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using System.Collections.Generic;
using Hartalega.FloorSystem.Business.Logic;
using System.Collections;
using System.Web.UI;
using Hartalega.FloorSystem.Framework;

namespace Hartalega.FloorSystem.Web.UI.TV
{
    public partial class OfficeWideReport : System.Web.UI.Page
    {
        public static int _count = 0;
        public static int _countLoc = 0;
        List<DropdownDTO> _reportsList = TVReportsBLL.GetTVReportDetails();
        public static String _location;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshTimeInterval.Value = (Global._refreshTime * Constants.SIXTY * Constants.THOUSAND).ToString();

                List<DropdownDTO> _reportsList = TVReportsBLL.GetTVReportDetails();
                OfficeFrame.Attributes["src"] = _reportsList[0].IDField + "?Location=" + Global._locationList[_countLoc].IDField;
                List<string> reportStr = new List<string>();
                foreach (DropdownDTO loc in Global._locationList)
                {
                    foreach (DropdownDTO record in _reportsList)
                    {
                        reportStr.Add(record.IDField + "?Location=" + loc.IDField);
                    }
                }

                string reportULS = string.Join(",", reportStr);
                ClientScript.RegisterHiddenField("HiddenReportULS", reportULS);
            }
        }
    }
}