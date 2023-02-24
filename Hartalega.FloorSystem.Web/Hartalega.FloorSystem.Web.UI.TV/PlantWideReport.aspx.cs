using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;

namespace Hartalega.FloorSystem.Web.UI.TV
{
    public partial class PlantWideReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshTimeInterval.Value = (Global._refreshTime * Constants.SIXTY * Constants.THOUSAND).ToString();

                List<DropdownDTO> _reportsList = TVReportsBLL.GetTVReportDetails();
                PlantFrame.Attributes["src"] = _reportsList[0].IDField + "?Location=" + Request.QueryString["Location"];
                List<string> reportStr = new List<string>();
                foreach (DropdownDTO record in _reportsList)
                {
                    reportStr.Add(record.IDField + "?Location=" + Request.QueryString["Location"]);
                }

                string reportULS = string.Join(",", reportStr);
                ClientScript.RegisterHiddenField("HiddenReportULS", reportULS);
            }
        }
    }
}