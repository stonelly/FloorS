using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Production_System_Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.WIPStockCount
{
    public partial class WIPReportByCutoffBatch : FormBase
    {
        #region Constant

        //Ref No
        private const string CUTOFF_BATCH_PREFIX = "CUT";
        private const string CUTOFF_BATCH_DATE_FORMAT = "yyyyMMdd";
        

        //Report
        private const string WIP_REPORT_BY_CUTOFF_BATCH = "WIP Report By Cutoff Batch";
        private const string REPORT_HIDE_PARAM_MENU = "&rc:Parameters=Collapsed";

        #endregion

        #region Variable

        bool isInitialized = false;
        private string fullReportPath = string.Empty;
        private string cutoffBatchRefNo = string.Empty;
        private DateTime cutoffDate;
        private DateTime? lastCutoffBatchCreatedDatetime;

        #endregion

        #region Constructor

        public WIPReportByCutoffBatch()
        {
            this.WindowState = FormWindowState.Maximized;

            InitializeComponent();
            FormLoad();
        }

        #endregion

        #region Private Method

        private void FormLoad()
        {
            InitializeCutoffDate();
            isInitialized = true;
        }

        private void ClearForm()
        {
            InitializeCutoffDate();
        }

        private void InitializeCutoffDate()
        {
            DateTime currentDatetime = DateTime.Now;
            int year = currentDatetime.Month == 1 ? currentDatetime.Year - 1 : currentDatetime.Year;
            int month = currentDatetime.Month == 1 ? 12 : currentDatetime.Month - 1;
            cutoffDate = new DateTime(year, month,DateTime.DaysInMonth(year, month)).Date;

            dtpCutoffDate.Value = cutoffDate;
            dtpCutoffDate.MaxDate = cutoffDate;
            dtpCutoffDate.Refresh();

            cutoffBatchRefNo = CUTOFF_BATCH_PREFIX + cutoffDate.ToString(CUTOFF_BATCH_DATE_FORMAT);
        }

        private bool CheckCutoffBatch()
        {
            bool isMonthAlreadtCutoff = false;

            lastCutoffBatchCreatedDatetime = null;
            lastCutoffBatchCreatedDatetime = WIPStockCountBLL.GetWIPCutoffBatchLastCreatedDatetime(cutoffDate);

            if (!lastCutoffBatchCreatedDatetime.Equals(null))
            {
                isMonthAlreadtCutoff = true;
            }

            return isMonthAlreadtCutoff;
        }

        private void ShowWIPReport()
        {
            try
            {
                List<WIPReportDTO> reportList = new List<WIPReportDTO>();
                reportList.AddRange(WIPStockCountBLL.GetWIPReports(WIP_REPORT_BY_CUTOFF_BATCH));

                ReportViewer reportForm = new ReportViewer();
                reportForm.Text = WIP_REPORT_BY_CUTOFF_BATCH;
                DateTime reportDate = new DateTime(cutoffDate.Year, cutoffDate.Month, cutoffDate.Day);
                string reportURL = reportList.First().ReportURL + string.Format(reportList.First().ReportParam, reportDate.ToString("dd MMM yyyy"), cutoffBatchRefNo) + REPORT_HIDE_PARAM_MENU;
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

        private void dtpCutoffDate_ValueChanged(object sender, EventArgs e)
        {
            if (isInitialized)
            {
                DateTime selectedDatetime = dtpCutoffDate.Value;
                DateTime endOfMonth = new DateTime(selectedDatetime.Year, selectedDatetime.Month, 
                                        DateTime.DaysInMonth(selectedDatetime.Year, selectedDatetime.Month)).Date;
                cutoffDate = endOfMonth;
                dtpCutoffDate.Value = cutoffDate;
                dtpCutoffDate.Refresh();

                cutoffBatchRefNo = CUTOFF_BATCH_PREFIX + cutoffDate.ToString(CUTOFF_BATCH_DATE_FORMAT);
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
            if (GlobalMessageBox.Show(string.Format(Messages.SAVE_CUTOFF_BATCH_CONFIRMATION, Environment.NewLine, cutoffBatchRefNo, cutoffDate.ToString("dd-MM-yyy")), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                bool isProceedToCutoff = true;

                if (CheckCutoffBatch())
                {
                    if (GlobalMessageBox.Show(string.Format(Messages.OVERWRITE_CUTOFF_BATCH_CONFIRMATION, cutoffDate.ToString("dd-MM-yyy"), Environment.NewLine), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.NO)
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
                            errorCode = WIPStockCountBLL.InsertWIPCutoffBatch(cutoffDate, cutoffBatchRefNo, lastCutoffBatchCreatedDatetime);

                            if (errorCode == 0)
                            {
                                scope.Complete();
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                            else if (errorCode == -2)
                            {
                                GlobalMessageBox.Show(string.Format(Messages.DUPLICATE_WIP_CUTOFF_REF_NO, Environment.NewLine), Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
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
