using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Production_System_Reports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.TVReports
{
    public partial class MainMenu : FormBase
    {

        #region Class Variables
        private static string _screenName = "TV Reports";
        private static string _className = "Main Menu";
        List<DropdownDTO> reportsList;
        #endregion
        public MainMenu()
        {
            InitializeComponent();
            try
            {
                reportsList = TVReportsBLL.GetReportDetails();
                int i = Constants.ZERO;
                Button[] dynamicBtn = new Button[100];
                TableLayoutPanel Table = new TableLayoutPanel();
                Table.Location = new Point(170, 20);
                Table.Size = new Size(400, 400);
                Table.AutoSize = true;
                Table.AutoScroll = true;
                Table.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddRows;
                Table.ColumnCount = 1;
                foreach (DropdownDTO report in reportsList)
                {
                    dynamicBtn[i] = new Button();
                    dynamicBtn[i].Text = report.DisplayField;
                    dynamicBtn[i].Width = Constants.bWidth;
                    dynamicBtn[i].Height = Constants.bHeigth;
                    Table.RowCount = i + 1;
                    Table.Controls.Add(dynamicBtn[i]);
                    panel1.Controls.Add(Table);
                 //   groupBox1.Controls.Add(panel1);
                    dynamicBtn[i].Click += new EventHandler(ButtonClick);
                    i++;
                }
            }
            catch (FloorSystemException e)
            {
                ExceptionLogging(e, _screenName, _className, "Report Buttons", null);
                return;
            }
        }
        /// <summary>
        /// Button click event
        /// </summary>
        void ButtonClick(object sender, EventArgs e)
        {
            WebBrowser webBrowser1 = new WebBrowser();
            Button clickedButton = (Button)sender;
            foreach (DropdownDTO record in reportsList)
            {
                if (clickedButton.Text == record.DisplayField)
                {
                    Process.Start(record.IDField + "?Location=" + WorkStationDTO.GetInstance().LocationId);
                    break;
                }
            }
        }

        /// <summary>
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)

                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else

                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
        }
    }
}
