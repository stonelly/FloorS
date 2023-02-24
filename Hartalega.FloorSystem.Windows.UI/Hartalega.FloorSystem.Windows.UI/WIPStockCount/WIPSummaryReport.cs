using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Production_System_Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.WIPStockCount
{
    public partial class WIPSummaryReport : FormBase
    {
        #region Constant

        //Report
        private const string WIP_SUMMARY_REPORT = "WIP Summary Report";
        private const string REPORT_HIDE_PARAM_MENU = "&rc:Parameters=Collapsed";

        #endregion

        #region Variable

        bool isInitialized = false;
        private string fullReportPath = string.Empty;
        private DateTime countingMonth;
        private DateTime? lastWIPSummaryCreatedDatetime;

        #endregion

        #region Constructor

        public WIPSummaryReport()
        {
            this.WindowState = FormWindowState.Maximized;

            InitializeComponent();
            FormLoad();
        }

        #endregion

        #region Private Method

        private void FormLoad()
        {
            InitializeCountingMonth();
            isInitialized = true;
        }

        private void ClearForm()
        {
            InitializeCountingMonth();
        }

        private void InitializeCountingMonth()
        {
            DateTime currentDatetime = DateTime.Now;
            int year = currentDatetime.Month == 1 ? currentDatetime.Year - 1 : currentDatetime.Year;
            int month = currentDatetime.Month == 1 ? 12 : currentDatetime.Month - 1;
            countingMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month)).Date;

            dtpCountingMonth.Value = countingMonth;
            dtpCountingMonth.MaxDate = countingMonth;
            dtpCountingMonth.Refresh();
        }

        private bool CheckWIPSummary()
        {
            bool isMonthAlreadyCountWIPSummary = false;

            lastWIPSummaryCreatedDatetime = null;
            lastWIPSummaryCreatedDatetime = WIPStockCountBLL.GetWIPSummaryBatchByCountingDate(countingMonth);

            if (!lastWIPSummaryCreatedDatetime.Equals(null))
            {
                isMonthAlreadyCountWIPSummary = true;
            }

            return isMonthAlreadyCountWIPSummary;
        }

        private void ShowWIPReport()
        {
            try
            {
                List<WIPReportDTO> reportList = new List<WIPReportDTO>();
                reportList.AddRange(WIPStockCountBLL.GetWIPReports(WIP_SUMMARY_REPORT));

                ReportViewer reportForm = new ReportViewer();
                reportForm.Text = WIP_SUMMARY_REPORT;
                DateTime reportDate = new DateTime(countingMonth.Year, countingMonth.Month, countingMonth.Day);
                string reportURL = reportList.First().ReportURL + string.Format(reportList.First().ReportParam, reportDate.ToString("dd MMM yyyy")) + REPORT_HIDE_PARAM_MENU;
                reportForm.GetReportUrl(reportURL);
                reportForm.ShowDialog();
            }
            catch (FloorSystemException ex)
            {
                GlobalMessageBox.Show(ex.Message, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
        }

        #endregion

        #region Event

        private void dtpCountingMonth_ValueChanged(object sender, EventArgs e)
        {
            if (isInitialized)
            {
                DateTime selectedDatetime = dtpCountingMonth.Value;
                DateTime endOfMonth = new DateTime(selectedDatetime.Year, selectedDatetime.Month,
                                        DateTime.DaysInMonth(selectedDatetime.Year, selectedDatetime.Month)).Date;
                countingMonth = endOfMonth;
                dtpCountingMonth.Value = countingMonth;
                dtpCountingMonth.Refresh();
            }
        }

        #endregion

        #region Button Command

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(string.Format(Messages.SAVE_WIP_SUMMARY_CONFIRMATION, Environment.NewLine, countingMonth.ToString("dd-MM-yyy")), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                bool isProceedToCutoff = true;

                if (CheckWIPSummary())
                {
                    if (GlobalMessageBox.Show(string.Format(Messages.OVERWRITE_WIP_SUMMARY_CONFIRMATION, countingMonth.ToString("dd-MM-yyy"), Environment.NewLine), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.NO)
                    {
                        isProceedToCutoff = false;
                    }
                }

                if (isProceedToCutoff)
                {
                    try
                    {
                        int errorCode = 0;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            errorCode = WIPStockCountBLL.InsertWIPSummary(countingMonth, lastWIPSummaryCreatedDatetime);

                            if (errorCode == 0)
                            {
                                scope.Complete();
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                            else if (errorCode == -2)
                            {
                                GlobalMessageBox.Show(string.Format(Messages.CONCURRENCY_ERROR, Environment.NewLine), Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                        }

                        if (errorCode == 0)
                        {
                            ShowWIPReport();
                            ClearForm();
                        }
                    }
                    catch (FloorSystemException ex)
                    {
                        GlobalMessageBox.Show(ex.Message, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                }
            }
        }

        #endregion

    }
}
