#region using

using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#endregion
namespace Hartalega.FloorSystem.Windows.UI.HourlyBatchCard
{
    public delegate void BindGrid();
    public delegate void PrintAsync(List<BatchDTO> lstBatchDTO);
    public partial class LineSelection : FormBase
    {
        #region Private Varibale

        private string _operaterId = string.Empty;
        private static System.Threading.Timer _btchTimer;
        private static string _screenName = "Line Selection";
        private static string _className = "LineSelection";
        private static int _selectedLine = Constants.ZERO;
        private static string _screenNameForAuthorization = "Line Selection";
        private static string _ManualPrintscreenName = "Batch Card Manual Print";
        private static bool _ThreadStart = false;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);   
        #endregion

        #region Load Form

        public LineSelection()
        {
            InitializeComponent();
            UIRefresh();
            try
            {
                log.Info("############## START: HBC Open Window ##############");
                BatchPrint = true;
                WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.HOURLYBATCHCARD);
                WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.LINESELECTION);
                menuStrip2.Items[0].Enabled = false;
                dgvLineSelection.AutoGenerateColumns = false;
                DateTime serverDatetime = CommonBLL.GetCurrentDateAndTimeFromServer();
                string[] datetime = serverDatetime.ToString("HH:mm:ss").Split(new char[] { ':' });
                int Nextcall = ((60 - Convert.ToInt32(datetime[1])) * 1000 * 60) - Convert.ToInt32(datetime[2]) * 1000;
                _btchTimer = new System.Threading.Timer(new TimerCallback(HBC_HourlyBatchCard_Save), null, Nextcall, Timeout.Infinite);
                log.InfoFormat("HBC First initialized timer. Time: {0}. , Next call in {1} minute(s) AND {2} second(s)"
                    , serverDatetime.ToString("HH:mm:ss")
                    , (Nextcall/ (1000 * 60)).ToString()
                    , (Math.Abs( Nextcall / (1000 * 60))).ToString());
            }
            catch (FloorSystemException ex)
            {
                log.ErrorFormat("Error LineSelection FloorSystemException : {0}", ex.Message);
                ExceptionLogging(ex, _screenName, _className, "frmLineSelection_Load", null);
                return;
            }
            catch (StackOverflowException soex)
            {
                log.ErrorFormat("Error LineSelection StackOverflowException : {0}", soex.Message);
                FloorSystemException fex = new FloorSystemException(soex.Message, soex.Message, soex, true);
                ExceptionLogging(fex, _screenName, _className, "frmLineSelection_Load", null);
                return;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error LineSelection Exception : {0}", ex.Message);
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "frmLineSelection_Load", null);
                return;
            }
            getLineSelectionDetails();

            //PrintBatchCard();
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLineSelection_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Event Handlers

        private void dgvLineSelection_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvLineSelection.Columns["dataGridViewStartPrint"].Index && e.RowIndex != -1)
            {
                dgvLineSelection.EndEdit();
                _selectedLine = e.RowIndex;
                DataGridViewCheckBoxCell chk = dgvLineSelection.Rows[e.RowIndex].Cells[2] as DataGridViewCheckBoxCell;

                updateLineStartDateTimeToStart(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells[1].Value), Convert.ToBoolean(dgvLineSelection.Rows[e.RowIndex].Cells[2].Value));
                dgvLineSelection.BeginInvoke(new BindGrid(BinddgvLineSelection));
            }
        }

        /// <summary>
        ///  Start Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbStart_Click(object sender, EventArgs e)
        {
            DateTime _now = ServerCurrentDateTime;
            if (dgvLineSelection.SelectedRows.Count != Constants.ZERO)
            {
                DataGridViewRow row = this.dgvLineSelection.SelectedRows[0];
                _selectedLine = row.Index;
                updateLineStartDateTimeToStart(Convert.ToString(row.Cells[1].Value), true);
            }
            getLineSelectionDetails();
        }

        /// <summary>
        /// stop Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbStop_Click(object sender, EventArgs e)
        {
            DateTime _now = ServerCurrentDateTime;
            if (dgvLineSelection.SelectedRows.Count != Constants.ZERO)
            {
                DataGridViewRow row = this.dgvLineSelection.SelectedRows[0];
                _selectedLine = row.Index;
                updateLineStartDateTimeToStart(Convert.ToString(row.Cells[1].Value), false);
            }
            getLineSelectionDetails();
        }

        /// <summary>
        /// Refresh image click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            getLineSelectionDetails();
        }

        /// <summary>
        /// Print Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbPrint_Click(object sender, System.EventArgs e)
        {
            Login passwordForm = new Login(Constants.Modules.HOURLYBATCHCARD, _ManualPrintscreenName);
            passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(passwordForm.Authentication))
            {
                if (dgvLineSelection.SelectedRows.Count != Constants.ZERO)
                {
                    DataGridViewRow row = this.dgvLineSelection.SelectedRows[0];
                    ManualPrint frmManualPrint = new ManualPrint(Convert.ToString(row.Cells[1].Value));
                    frmManualPrint.OperatorId = passwordForm.Authentication;
                    frmManualPrint.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLineSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.InfoFormat("Start HBC form closing by reason : {0}", e.CloseReason.ToString());
            Login passwordForm = new Login(Constants.Modules.HOURLYBATCHCARD, _screenNameForAuthorization);
            passwordForm.ShowDialog();
            try
            {
                if (!string.IsNullOrEmpty(passwordForm.Authentication))
                {
                    BatchPrint = false;
                    {
                        HourlyBatchCardBLL.LineSelectionClosed(passwordForm.Authentication, WorkStationDTO.GetInstance().WorkStationId);
                        log.Info("############## END: HBC Closed Window - password validated ##############");
                    }
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (FloorSystemException ex)
            {
                log.ErrorFormat("Error HBC Close window frmLineSelection_FormClosing : {0}", ex.Message);
                ExceptionLogging(ex, _screenName, _className, "frmLineSelection_FormClosing", null);
                return;
            }

        }

        /// <summary>
        /// Timer Control Setting
        /// </summary>
        /// <param name="sender"></param>
        private void HBC_HourlyBatchCard_Save(object sender)
        {
            log.Info("HBC Timer triggered");
            if (!_ThreadStart)
            {
                if (InvokeRequired)
                    Invoke(new MethodInvoker(getLineSelectionDetails));

                _btchTimer = new System.Threading.Timer(new TimerCallback(HBC_HourlyBatchCard_Save), null, 1000 * 60 * 60, Timeout.Infinite);
                _ThreadStart = true;
                log.Info("HBC Thread Started. Timer set to next hour");                

                PrintBatchCard();
                _ThreadStart = false;
                log.Info("HBC Thread End");
            }
            else
            {
                log.Info("HBC Timer End without running Thread");
            }
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string itemText = e.ClickedItem.Text;
            switch (itemText)
            {
                case Constants.REPRINT_BATCH_CARD:
                    Login _passwordForm = new Login(Constants.Modules.HOURLYBATCHCARD, _ManualPrintscreenName);
                    _passwordForm.ShowDialog();
                    if (!string.IsNullOrEmpty(_passwordForm.Authentication))
                    {
                        ManualPrint _frmManualPrint = new ManualPrint();
                        _frmManualPrint.OperatorId = _passwordForm.Authentication;
                        _frmManualPrint.Show(); //changed by MyAdamas on 23/10/2017 -> Successful Message always show at the background if using .ShowDialog()
                    }
                    break;
                case Constants.MANUAL_PRINT_BATCH_CARD:
                    Login _passwordForm1 = new Login(Constants.Modules.HOURLYBATCHCARD, _ManualPrintscreenName);
                    _passwordForm1.ShowDialog();
                    if (!string.IsNullOrEmpty(_passwordForm1.Authentication))
                    {
                        ManualHourlyBatchCard _frmManualPrintBatchCard = new ManualHourlyBatchCard();
                        _frmManualPrintBatchCard.OperatorId = _passwordForm1.Authentication;
                        _frmManualPrintBatchCard.Show();    //changed by MyAdamas on 23/10/2017 -> Successful Message always show at the background if using .ShowDialog()
                    }
                    break;
                case Constants.BATCH_CARD_REPRINT_LOG:
                    new BatchCardReprintLog().ShowDialog();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region User Methods

        /// <summary>
        /// Print Batch card All
        /// </summary>
        private void PrintBatchCard()
        {

            List<BatchDTO> lstBatchDTO = new List<BatchDTO>();
            try
            {
                if (BatchPrint)
                {
                    log.Info("## Start HBC Insert Data to DB ##");
                    lstBatchDTO = HourlyBatchCardBLL.HourlyBatchCardSave(ServerCurrentDateTime, System.Windows.Forms.SystemInformation.ComputerName, WorkStationDTO.GetInstance().Module, WorkStationDTO.GetInstance().SubModule);
                    log.Info("## End HBC Insert Data to DB ##");
                }
            }
            catch (FloorSystemException ex)
            {
                log.ErrorFormat("Error PrintBatchCard() FloorSystemException : {0}", ex.Message);
                ExceptionLogging(ex, _screenName, _className, "PrintBatchCard", null);
                return;
            }

            try
            {
                if (lstBatchDTO.Count > Constants.ZERO)
                {
                    //if (this.Handle != IntPtr.Zero)
                    //{
                    //    log.Info("## Print physical Batch card execute ASYNC call ##");
                    //    IAsyncResult asyncRes = this.BeginInvoke(new PrintAsync(PrintBatchAsync), lstBatchDTO);
                    //    this.EndInvoke(asyncRes);
                    //    log.Info("## Print physical Batch card execute ASYNC call END ##");
                    //}
                    //else
                    //{
                        log.Info("## Print physical Batch card execute NON-ASYNC call ##");
                        PrintBatchAsync(lstBatchDTO);
                        log.Info("## Print physical Batch card execute NON-ASYNC call END ##");
                    //}
                }
                else
                {
                    log.InfoFormat("No batch card print due to lstBatchDTO count is {0}", lstBatchDTO.Count.ToString());
                }
            }
            catch (FloorSystemException ex)
            {
                log.ErrorFormat("Error Async call PrintBatchCard() FloorSystemException : {0}", ex.Message);
                ExceptionLogging(ex, _screenName, _className, "PrintBatchCard", null);
                return;
            }

        }

        /// <summary>
        /// Print Batch card by Each Row
        /// </summary>
        /// <param name="objbatchDTO"></param>
        private void PrintBatchCard(List<PrintDTO> printdtoLst)
        {
            try
            {
                HourlyBatchCardBLL.PrintDetails(printdtoLst);
            }
            catch (FloorSystemException ex)
            {
                log.ErrorFormat("Error PrintBatchCard FloorSystemException : {0}", ex.Message);
                ExceptionLogging(ex, _screenName, _className, "PrintBatchCard", printdtoLst);
                return;
            }
            catch (StackOverflowException soex)
            {
                log.ErrorFormat("Error PrintBatchCard StackOverflowException : {0}", soex.Message);
                FloorSystemException fex = new FloorSystemException(soex.Message, soex.Message, soex, true);
                ExceptionLogging(fex, _screenName, _className, "PrintBatchCard", printdtoLst);
                return;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error PrintBatchCard Exception : {0}", ex.Message);
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "PrintBatchCard", printdtoLst);
                return;
            }
        }

        /// <summary>
        /// Get Line Selection Details
        /// </summary>
        private void getLineSelectionDetails()
        {
            try
            {
                List<LineSelectionDTO> dtLineSelection;
                dtLineSelection = HourlyBatchCardBLL.GetLineSelectionDetails(System.Windows.Forms.SystemInformation.ComputerName);
                dgvLineSelection.DataSource = dtLineSelection;
                if (dtLineSelection.Count > Constants.ZERO)
                {
                    dgvLineSelection.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
                    dgvLineSelection.Columns[4].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgvLineSelection.Columns[5].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgvLineSelection.Columns[6].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgvLineSelection.CurrentCell = dgvLineSelection[0, _selectedLine];
                    dgvLineSelection.Rows[_selectedLine].Selected = true;
                }
            }
            catch (FloorSystemException ex)
            {
                log.ErrorFormat("Error getLineSelectionDetails FloorSystemException : {0}", ex.Message);
                ExceptionLogging(ex, _screenName, _className, "getLineSelectionDetails", null);
                return;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error getLineSelectionDetails Exception : {0}", ex.Message);
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "getLineSelectionDetails", null);
                return;
            }
        }

        /// <summary>
        /// Update Line Start/Stop 
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="isStartPrint"></param>
        private void updateLineStartDateTimeToStart(string lineId, bool isStartPrint)
        {
            int rowsaffected;
            try
            {
                if (isStartPrint)
                {
                    rowsaffected = HourlyBatchCardBLL.UpdateLineStartDateTimeToStart(lineId, isStartPrint, System.Windows.Forms.SystemInformation.ComputerName, _operaterId);
                }
                else
                {
                    rowsaffected = HourlyBatchCardBLL.UpdateLineStopDateTimeToStart(lineId, isStartPrint, System.Windows.Forms.SystemInformation.ComputerName, _operaterId);
                }
            }
            catch (FloorSystemException ex)
            {
                log.ErrorFormat("Error updateLineStartDateTimeToStart FloorSystemException : {0}", ex.Message);
                ExceptionLogging(ex, _screenName, _className, "updateLineStartDateTimeToStart", lineId, isStartPrint);
                return;
            }
        }

        /// <summary>
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            _ThreadStart = false;
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
        }

        private void BinddgvLineSelection()
        {
            getLineSelectionDetails();
        }

        /// <summary>
        /// PrintBatchAsync
        /// </summary>
        /// <param name="lstBatchDTO"></param>
        private void PrintBatchAsync(List<BatchDTO> lstBatchDTO)
        {
            log.Info("Start PrintBatchAsync");
            if (PrinterClass.IsPrinterExists)
            {   
                List<PrintDTO> printdtolst = new List<PrintDTO>();
                this.SuspendLayout();
                this.Cursor = Cursors.WaitCursor;
                Enabled = false;
                foreach (BatchDTO objbatchDTO in lstBatchDTO)
                {
                    log.InfoFormat("Prepare PrintData  {0}", objbatchDTO.SerialNumber);
                    PrintDTO printdto = HourlyBatchCardBLL.GetBatchList(Convert.ToDateTime(objbatchDTO.BatchCarddate).ToLongTimeString(), objbatchDTO.SerialNumber, objbatchDTO.BatchNumber, String.Empty, objbatchDTO.Size, String.Empty, false, objbatchDTO.GloveType, objbatchDTO.TierSide, Constants.HOURLYBATCHCARD);
                    printdtolst.Add(printdto);
                }
                log.Info("Start Print Data");
                PrintBatchCard(printdtolst);
                log.Info("End Print Data");
                Enabled = true;
                this.Cursor = Cursors.Default;
                this.ResumeLayout();
            }
            else
            {
                log.Info("HBC PrintBatchAsync PrinterClass.IsPrinterExists is FALSE");
            }
            log.Info("END PrintBatchAsync");
        }

        private void UIRefresh()
        {

            try
            {
                _ThreadStart = false;
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                log.ErrorFormat("Error UIRefresh FloorSystemException : {0}", ex.Message);
                ExceptionLogging(ex, _screenName, _className, "ManualPrint", null);
            }
        }
        #endregion

    }
}
