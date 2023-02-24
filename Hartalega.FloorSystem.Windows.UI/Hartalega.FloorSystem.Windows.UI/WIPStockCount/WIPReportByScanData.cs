using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Production_System_Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.WIPStockCount
{
    public partial class WIPReportByScanData : FormBase
    {
        #region Constant

        //Combo Box Setting Name
        private const string DISPLAY_MEMBER = "DisplayField";
        private const string VALUE_MEMBER = "IDField";

        //Grid View Header Column Name
        private const string BATCH_STATUS = "BatchStatus";

        //Batch Valid Status
        private const string VALID_STATUS = "Valid";

        //SQL Server Error Message
        private const string WIP_RUNNING_NUMBER_ALREADY_EXISTS = "WIP Running Number Already Exists.";

        //Scan Data File Name
        private const string ARCHIEVE_FILE_NAME = "WIPScanData_{0}.txt";
        private const string ARCHIEVE_FILE_DATETIME_FORMAT = "yyyyMMdd_HHmmss";

        //Report
        private const string WIP_REPORT_BY_SCAN_DATA = "WIP Report By Scan Data";
        private const string REPORT_HIDE_PARAM_MENU = "&rc:Parameters=Collapsed";

        #endregion

        #region Variable

        private bool isInitialized = false;
        private bool isUploadByScanner = false;
        private DateTime currentDate;
        private DateTime selectedDate;
        private DateTime endOfMonthDate;
        private int uploadCount = 0;
        private string referenceNo = string.Empty;
        private List<string> serialNoList;
        private List<WIPTransactionDTO> wipScannedDataList;
        private string scanDataFilePath = string.Empty;
        private string archieveDataFilePath = string.Empty;

        #endregion

        #region Constructor

        public WIPReportByScanData()
        {
            InitializeComponent();
            FormLoad();
        }

        #endregion

        #region Private Method

        private void FormLoad()
        {
            this.WindowState = FormWindowState.Maximized;

            serialNoList = new List<string>();
            wipScannedDataList = new List<WIPTransactionDTO>();
            dgvWIPScannedData.AutoGenerateColumns = false;

            PopulateAreaComboBox();
            PopulatePlantComboBox();
            InitializeMonthDateTimePicker();

            btnProceed.Enabled = false;
            isInitialized = true;
            UpdatereferenceNo();
        }

        private void ClearForm()
        {
            isUploadByScanner = false;
            dtpMonth.Focus();
            dgvWIPScannedData.DataSource = null;
            referenceNo = string.Empty;
            serialNoList.Clear();
            wipScannedDataList.Clear();
            
            PopulateAreaComboBox();
            PopulatePlantComboBox();
            InitializeMonthDateTimePicker();

            btnProceed.Enabled = false;
            isInitialized = true;
            UpdatereferenceNo();

            btnUpload.Enabled = true;
            btnUploadByFile.Enabled = true;
        }

        private void InitializeMonthDateTimePicker()
        {
            selectedDate = currentDate  = DateTime.Now.Date;
            endOfMonthDate = new DateTime(selectedDate.Year, selectedDate.Month,
                                         DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month)).Date;
            dtpMonth.Value = currentDate;
            dtpMonth.MaxDate = currentDate;
            dtpMonth.Refresh();
        }

        private void PopulateAreaComboBox()
        {
            List<DropdownDTO> areaList = new List<DropdownDTO>();
            areaList = MasterTableBLL.GetAreaList();
            cbArea.DataSource = areaList;
            cbArea.DisplayMember = DISPLAY_MEMBER;
            cbArea.ValueMember = VALUE_MEMBER;
            cbArea.SelectedItem = areaList[0];
        }

        private void PopulatePlantComboBox()
        {
            List<DropdownDTO> planList = new List<DropdownDTO>();
            planList = MasterTableBLL.GetLocation();
            cbPlant.DataSource = planList;
            cbPlant.DisplayMember = DISPLAY_MEMBER;
            cbPlant.ValueMember = VALUE_MEMBER;
            cbPlant.SelectedItem = planList[0];
        }

        private void UpdatereferenceNo()
        {
            if (cbArea.SelectedItem != null && cbPlant.SelectedItem != null && isInitialized)
            {
                string area = cbArea.GetItemText(cbArea.SelectedItem);
                string plant = cbPlant.GetItemText(cbPlant.SelectedItem).Substring(1, 1);
                DateTime date = selectedDate > currentDate ? currentDate : selectedDate;
                uploadCount = WIPStockCountBLL.GetWIPRunningNumber(selectedDate, Convert.ToInt32(cbPlant.SelectedValue), Convert.ToInt32(cbArea.SelectedValue));

                referenceNo = area + plant + date.ToString("yyyyMMdd") + "-" + (uploadCount + 1);
                txbRefNum.Text = referenceNo;
            }
        }

        private void ShowWIPReport()
        {
            try
            {
                List<WIPReportDTO> reportList = new List<WIPReportDTO>();
                reportList.AddRange(WIPStockCountBLL.GetWIPReports(WIP_REPORT_BY_SCAN_DATA));

                ReportViewer reportForm = new ReportViewer();
                reportForm.Text = WIP_REPORT_BY_SCAN_DATA;
                DateTime reportDate = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day);
                string reportURL = reportList.First().ReportURL + string.Format(reportList.First().ReportParam, reportDate.ToString("dd MMM yyyy"), referenceNo, cbPlant.SelectedValue, cbArea.SelectedValue) + REPORT_HIDE_PARAM_MENU;
                reportForm.GetReportUrl(reportURL);
                reportForm.ShowDialog();
            }
            catch (FloorSystemException ex)
            {
                GlobalMessageBox.Show(ex.Message, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
        }

        private void ReadWIPSannerData()
        {
            ProcessStartInfo startinfo = new ProcessStartInfo();

            try
            {
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {
                    if (string.IsNullOrEmpty(WorkStationDataConfiguration.GetInstance().WIPScanDataExecutableFileLocation))
                    {
                        GlobalMessageBox.Show(Messages.PORTABLE_BARCODE_SCANNER_NOT_FOUND, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        return;
                    }
                    else
                    {
                        startinfo.FileName = WorkStationDataConfiguration.GetInstance().WIPScanDataExecutableFileLocation;
                        startinfo.Arguments = WorkStationDataConfiguration.GetInstance().WIPScanDataArgumentFileLocation;
                        Process myprocess = Process.Start(startinfo);
                        while (myprocess.HasExited == false)
                        {
                        }
                    }
                }

                scanDataFilePath = WorkStationDataConfiguration.GetInstance().WIPScanDataTextFileLocation;
                archieveDataFilePath = WorkStationDataConfiguration.GetInstance().WIPScanDataArchieveTextFileLocation;

                if (!string.IsNullOrEmpty(scanDataFilePath) && !string.IsNullOrEmpty(archieveDataFilePath))
                {
                    if (File.Exists(scanDataFilePath))
                    {                        
                        serialNoList.Clear();
                        serialNoList = File.ReadAllLines(scanDataFilePath).Select(p => p.ToString()).ToList();

                        if (serialNoList.Count > 0)
                        {
                            UpdateWIPScanDataGridView(false);
                            MoveScanDataFile(true);

                            isUploadByScanner = true;
                            btnProceed.Enabled = true;
                            btnProceed.Focus();
                            btnUpload.Enabled = false;
                            btnUploadByFile.Enabled = false;
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.SCAN_DATA_NOT_FOUND, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(scanDataFilePath))
                        GlobalMessageBox.Show(Messages.SCAN_DATA_FILE_PATH_NOT_FOUND, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    else if (!string.IsNullOrEmpty(archieveDataFilePath))
                        GlobalMessageBox.Show(Messages.SCAN_DATA_ARCHIEVE_PATH_NOT_FOUND, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    else
                        GlobalMessageBox.Show(Messages.SCAN_DATA_FILE_ARCHIEVE_PATH_NOT_FOUND, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                }
            }
            catch (FloorSystemException ex)
            {
                GlobalMessageBox.Show(ex.Message, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
        }

        private void MoveScanDataFile(bool IsDelete)
        {
            if (!string.IsNullOrEmpty(scanDataFilePath) && !string.IsNullOrEmpty(archieveDataFilePath))
            {
                if (File.Exists(scanDataFilePath))
                {
                    DateTime curDatetime = DateTime.Now;
                    Directory.CreateDirectory(archieveDataFilePath);
                    string fileName = string.Format(ARCHIEVE_FILE_NAME, curDatetime.ToString(ARCHIEVE_FILE_DATETIME_FORMAT));
                    string fullArchievePath = Path.Combine(archieveDataFilePath, fileName);
                    File.Copy(scanDataFilePath, fullArchievePath);

                    if (IsDelete)
                    {
                        File.Delete(scanDataFilePath);
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.SCAN_DATA_NOT_FOUND, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(scanDataFilePath))
                    GlobalMessageBox.Show(Messages.SCAN_DATA_FILE_PATH_NOT_FOUND, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                else if (!string.IsNullOrEmpty(archieveDataFilePath))
                    GlobalMessageBox.Show(Messages.SCAN_DATA_ARCHIEVE_PATH_NOT_FOUND, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                else
                    GlobalMessageBox.Show(Messages.SCAN_DATA_FILE_ARCHIEVE_PATH_NOT_FOUND, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
        }

        #endregion

        #region Event

        private void cbPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatereferenceNo();
        }

        private void cbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatereferenceNo();
        }

        private void dtpMonth_ValueChanged(object sender, EventArgs e)
        {
            selectedDate = dtpMonth.Value;
            endOfMonthDate = new DateTime(selectedDate.Year, selectedDate.Month,
                                DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month)).Date;

            UpdatereferenceNo();
            UpdateWIPScanDataGridView(true);
        }

        private void UpdateWIPScanDataGridView(Boolean isFromDateChangeEvent)
        {
            bool isGetWIPDate = true;

            if (isInitialized)
            {
                if (isFromDateChangeEvent)
                {
                    if (wipScannedDataList.Count == 0)
                    {
                        isGetWIPDate = false;
                    }
                }

                if (isGetWIPDate)
                {
                    wipScannedDataList.Clear();
                    wipScannedDataList.AddRange(WIPStockCountBLL.GetWIPBatchBySerialNoList(String.Join(",", serialNoList), selectedDate));

                    dgvWIPScannedData.DataSource = null;
                    BindingSource source = new BindingSource(wipScannedDataList, null);
                    dgvWIPScannedData.DataSource = source;
                }
            }
        }

        private void dgvWIPScannedData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv.Columns[e.ColumnIndex].Name == BATCH_STATUS)
            {
                if (e.Value != null)
                {
                    if (e.Value.ToString() == VALID_STATUS)
                        dgv.Rows[e.RowIndex].Cells[BATCH_STATUS].Style.BackColor = System.Drawing.Color.Green;
                    else
                        dgv.Rows[e.RowIndex].Cells[BATCH_STATUS].Style.BackColor = System.Drawing.Color.Red;
                }
            }
        }
        #endregion

        #region Button Command

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bool IsClearValid = true;

            if (wipScannedDataList.Count > 0 && isUploadByScanner)
            {
                if (GlobalMessageBox.Show(string.Format(Messages.CLEAR_WIP_SCAN_DATA_CONFIRMATION, Environment.NewLine), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.NO)
                {
                    IsClearValid = false;
                }
            }

            if (IsClearValid)
            {
                ClearForm();
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            ReadWIPSannerData();
        }

        private void btnUploadByFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Filter = "txt Files (*.txt)|*.txt";
                    //ofd.Multiselect
                    DialogResult result = ofd.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        if (!string.IsNullOrWhiteSpace(ofd.FileName))
                        {
                            scanDataFilePath = ofd.FileName;
                            archieveDataFilePath = WorkStationDataConfiguration.GetInstance().WIPScanDataArchieveTextFileLocation;

                            if (!string.IsNullOrEmpty(archieveDataFilePath))
                            {
                                serialNoList.Clear();
                                serialNoList = File.ReadAllLines(ofd.FileName).Select(p => p.ToString()).ToList();

                                if (File.Exists(scanDataFilePath))
                                {
                                    serialNoList.Clear();
                                    serialNoList = File.ReadAllLines(scanDataFilePath).Select(p => p.ToString()).ToList();

                                    if (serialNoList.Count > 0)
                                    {
                                        UpdateWIPScanDataGridView(false);
                                        MoveScanDataFile(false);

                                        btnProceed.Enabled = true;
                                        btnProceed.Focus();
                                        btnUpload.Enabled = false;
                                        btnUploadByFile.Enabled = false;
                                    }
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DATA_FILE_NOT_FOUND, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DATA_FILE_ARCHIEVE_PATH_NOT_FOUND, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DATA_FILE_FILE_PATH_NOT_FOUND, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        }
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                GlobalMessageBox.Show(ex.Message, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (wipScannedDataList.Count > 0)
            {
                if (GlobalMessageBox.Show(string.Format(Messages.SAVE_SCAN_DATA_CONFIRMATION, Environment.NewLine, referenceNo, selectedDate.ToString("dd-MM-yyy")), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    #region Declare Data Table
                    DataTable WIPTransactionTable = new DataTable();
                    WIPTransactionTable.Columns.Add("Month", typeof(DateTime));
                    WIPTransactionTable.Columns.Add("LocationID", typeof(int));
                    WIPTransactionTable.Columns.Add("AreaID", typeof(int));
                    WIPTransactionTable.Columns.Add("UploadCount", typeof(int));
                    WIPTransactionTable.Columns.Add("Reference", typeof(string));
                    WIPTransactionTable.Columns.Add("SerialNumber", typeof(decimal));
                    WIPTransactionTable.Columns.Add("BatchNumber", typeof(string));
                    WIPTransactionTable.Columns.Add("GloveType", typeof(string));
                    WIPTransactionTable.Columns.Add("Size", typeof(string));
                    WIPTransactionTable.Columns.Add("BatchWeight", typeof(decimal));
                    WIPTransactionTable.Columns.Add("TenPCsWeight", typeof(decimal));
                    WIPTransactionTable.Columns.Add("PCs", typeof(int));
                    WIPTransactionTable.Columns.Add("IsCutOff", typeof(bool));
                    WIPTransactionTable.Columns.Add("IsVoided", typeof(bool));
                    WIPTransactionTable.Columns.Add("WIPScanStatusID", typeof(int));
                    #endregion

                    #region Populate Data Table
                    foreach (var item in wipScannedDataList)
                    {
                        DataRow dr = WIPTransactionTable.NewRow();
                        dr["Month"] = selectedDate;
                        dr["LocationID"] = cbPlant.SelectedValue;
                        dr["AreaID"] = cbArea.SelectedValue;
                        dr["UploadCount"] = (uploadCount + 1);
                        dr["Reference"] = referenceNo;
                        dr["SerialNumber"] = item.SerialNo;
                        dr["BatchNumber"] = item.BatchNo;
                        dr["GloveType"] = item.GloveType;
                        dr["Size"] = item.GloveSize;

                        if(item.BatchWeight == null)
                            dr["BatchWeight"] =DBNull.Value;
                        else
                            dr["BatchWeight"] = item.BatchWeight;

                        if(item.TenPCsWeight == null)
                            dr["TenPCsWeight"] = DBNull.Value;
                        else
                            dr["TenPCsWeight"] = item.TenPCsWeight;

                        if (item.TotalPCs == null)
                            dr["PCs"] = DBNull.Value;
                        else
                            dr["PCs"] = item.TotalPCs;

                        dr["IsCutOff"] = false;
                        dr["IsVoided"] = false;
                        dr["WIPScanStatusID"] = item.WIPScanStatusID;
                        WIPTransactionTable.Rows.Add(dr);
                    }

                    WIPTransactionTable.AcceptChanges();
                    #endregion

                    try
                    {
                        int errorCode = 0;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            errorCode = WIPStockCountBLL.InsertWIPScannedData(WIPTransactionTable);
                            if (errorCode == 0)
                            {
                                scope.Complete();
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                            else if (errorCode == -2)
                            {
                                GlobalMessageBox.Show(string.Format(Messages.DUPLICATE_WIP_REF_NO, Environment.NewLine), Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                        }

                        if (errorCode == 0)
                        {
                            btnProceed.Enabled = false;

                            ShowWIPReport();
                            ClearForm();
                        }
                        else if (errorCode == -2)
                        {
                            UpdatereferenceNo();
                        }
                    }
                    catch (FloorSystemException ex)
                    {
                        GlobalMessageBox.Show(ex.Message, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        ClearForm();
                    }
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.INVALID_SCAN_DATA_TO_PROCEED, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
        }

        #endregion

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
