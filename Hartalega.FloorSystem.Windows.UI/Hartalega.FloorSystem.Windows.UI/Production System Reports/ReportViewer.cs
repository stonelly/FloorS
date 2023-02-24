using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.Production_System_Reports
{
    public partial class ReportViewer : FormBase
    {
        public ReportViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Refresh the form On Load
        /// </summary>
        private void ReportViewer_Load(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        /// <summary>
        /// Binds the URL to form
        /// </summary>
        public void GetReportUrl(string repUrl)
        {
         webBrowser1.Url = new Uri(repUrl); 
        }
    }
}
