using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Production_System_Reports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.WIPStockCount
{
    public partial class WIPReportsMenu : FormBase
    {
        #region Variable

        List<WIPReportDTO> reportsList;

        #endregion

        #region Constructor

        public WIPReportsMenu()
        {
            this.WindowState = FormWindowState.Maximized;

            InitializeComponent();
            PopulateReportButton();
        }

        #endregion

        #region Private Function

        private void PopulateReportButton()
        {
            reportsList = WIPStockCountBLL.GetWIPReports(null);
            Button[] dynamicBtn = new Button[reportsList.Count];
            int i = 0;

            TableLayoutPanel Table = new TableLayoutPanel();
            Table.SuspendLayout();
            //Table.Location = new Point(170, 20);
            //Table.Size = new Size(900, 400);
            Table.AutoSize = true;
            Table.AutoScroll = true;
            Table.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Table.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddRows;
            Table.ColumnCount = 1;
            Table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
                    | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
            foreach (WIPReportDTO report in reportsList)
            {
                dynamicBtn[i] = new Button();
                dynamicBtn[i].Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top 
                    | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
                dynamicBtn[i].Text = report.ReportName;
                dynamicBtn[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dynamicBtn[i].Width = Constants.bWidth;
                dynamicBtn[i].Height = Constants.bHeigth;
                Table.RowCount = i + 1;
                Table.Controls.Add(dynamicBtn[i]);
                pnlreports.Controls.Add(Table);
                dynamicBtn[i].Click += new EventHandler(ButtonClick);
                i++;
            }
            Table.ResumeLayout();
        }

        #endregion

        #region Button Command

        void ButtonClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            foreach (WIPReportDTO record in reportsList)
            {
                if (clickedButton.Text == record.ReportName)
                {
                    ReportViewer reportForm = new ReportViewer();
                    reportForm.Text = record.ReportName;
                    reportForm.GetReportUrl(record.ReportURL);
                    reportForm.ShowDialog();
                    break;
                }
            }
        }

        #endregion
    }
}
