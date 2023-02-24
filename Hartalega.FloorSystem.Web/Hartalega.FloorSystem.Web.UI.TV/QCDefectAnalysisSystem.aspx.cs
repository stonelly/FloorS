using System;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System.IO;
using System.Linq;
using System.Globalization;

namespace Hartalega.FloorSystem.Web.UI.TV
{
    public partial class QCDefectAnalysisSystem : System.Web.UI.Page
    {
        DataTable dtQCDefect;
        /// <summary>
        /// Static Variables
        /// </summary>
        public static string _screenName = "TVReports";
        public static string _className = "QCDefectAnalysisSystem";
        double ScreenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DateTime currentTime = DateTime.ParseExact(Global._currentReportTime, Global._dateformat, CultureInfo.InvariantCulture);
                lblError.Visible = false;
                Location.Text = Constants.LOCATION + Global._locationList.First(obj => obj.IDField == Request.QueryString["Location"]).DisplayField.ToString();
                NextRotationTime.Text = Global._currentReportTime;
                NextRefreshTime.Text = Constants.NEXT_REFRESH_TIME + Global._batchJobNextRunTime;
                LastBatchJobRunTime.Text = Constants.LAST_BATCH_RUN_TIME + Global._batchJobLastRunTime;
                Timer1.Interval = (Global._refreshTime * Constants.SIXTY - (Convert.ToDateTime(CommonBLL.GetCurrentDateAndTimeFromServer()).Subtract(currentTime).Seconds)) * Constants.THOUSAND + 5000;
                if (Request.QueryString[Constants.HISTORYDATE] != null && Request.QueryString[Constants.HISTORYDATE] != String.Empty)
                    GenerateHistoryReport();
                else
                    GenerateReport();

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "Page_Load", null);
                return;
            }
        }
        [System.Web.Services.WebMethod]
        protected void GenerateReport()
        {
            try
            {
                dtQCDefect = GetQCDefectData();
                this.Title = Constants.QC_DEFECT_SYSTEM;
                QCDefectGridView.RowDataBound += new GridViewRowEventHandler(QCDefectGridView_RowDataBound);
                QCDefectGridView.DataSource = dtQCDefect;
                QCDefectGridView.DataBind();
                QCDefectGridView.HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                QCDefectGridView.HeaderStyle.Height = new Unit("20px");
            }
            catch (FloorSystemException e)
            {
                ExceptionLogging(e, _screenName, _className, "GenerateReport", null);
                return;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(string.Format(Messages.UNKNOWN_EXCEPTION_THROWN, _screenName), _className, ex);
            }
        }
        protected void GenerateHistoryReport()
        {
            try
            {
                string date = Request.QueryString["HistoryDate"];
                if (date.Length >= Constants.TWELVE)
                {
                    string xmlFilePath = FloorSystemConfiguration.GetInstance().TvReportsHistoryFolderPath +
                        date.Substring(0, 4) + Constants.SLASH + date.Substring(4, 2) + Constants.SLASH + date.Substring(6, 2) + Constants.SLASH +
                        date.Substring(8, 2) + Constants.SLASH + date.Substring(10, 2) + Constants.SLASH + Constants.QCDEFECTFILE;
                    if (!File.Exists(xmlFilePath))
                    {
                        lblError.Visible = true;
                        lblError.Text = Constants.FILE_NOT_PRESENT;
                    }
                    else
                    {
                        DataSet dtQCDefectData = new DataSet();
                        dtQCDefectData.ReadXml(xmlFilePath);
                        this.Title = Constants.QC_DEFECT_SYSTEM;
                        DataTable dtQCDefect = new DataTable();
                        dtQCDefect = (dtQCDefectData.Tables[0].Select("LocationId=" + Request.QueryString["Location"]).CopyToDataTable());
                        dtQCDefect.Columns.Remove("LocationId");
                        QCDefectGridView.RowDataBound += new GridViewRowEventHandler(QCDefectGridView_RowDataBound);
                        double ColumnWidth = ScreenWidth / dtQCDefect.Columns.Count;
                        Session["ColWidth"] = ColumnWidth;
                        QCDefectGridView.DataSource = dtQCDefect;
                        QCDefectGridView.DataBind();
                        QCDefectGridView.HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                        QCDefectGridView.HeaderStyle.Height = new Unit("20px");
                    }
                }
                else
                {

                    lblError.Visible = true;
                    lblError.Text = Constants.FILE_NOT_PRESENT;
                }
            }
            catch (FloorSystemException e)
            {
                ExceptionLogging(e, _screenName, _className, "GenerateHistoryReport", null);
                return;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(string.Format(Messages.UNKNOWN_EXCEPTION_THROWN, _screenName), _className, ex);
            }
        }
        public void QCDefectGridView_RowDataBound(object o, GridViewRowEventArgs e)
        {
            try
            {
                // apply custom formatting to data cells
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // set formatting for the category cell
                    TableCell cell = e.Row.Cells[0];
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        if (i == 0)
                        {
                            cell = e.Row.Cells[i];
                            cell.HorizontalAlign = HorizontalAlign.Center;
                            cell.BorderWidth = new Unit("1px");
                            cell.ForeColor = System.Drawing.Color.White;
                            cell.BorderColor = System.Drawing.Color.Gray;
                            cell.BackColor = System.Drawing.Color.Black;
                        }
                        else
                        {
                            cell = e.Row.Cells[i];
                            cell.HorizontalAlign = HorizontalAlign.Right;
                            if (cell.Text != "&nbsp;")
                            {
                                if (cell.Text.Contains(Constants.ZERO_SLASH))
                                {
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                    cell.Font.Bold = true;
                                    cell.BorderWidth = new Unit("1px");
                                    cell.ForeColor = System.Drawing.Color.Green;
                                    cell.BorderColor = System.Drawing.Color.Gray;
                                    cell.BackColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                    cell.Font.Bold = true;
                                    cell.BorderWidth = new Unit("1px");
                                    cell.ForeColor = System.Drawing.Color.Red;
                                    cell.BorderColor = System.Drawing.Color.Gray;
                                    cell.BackColor = System.Drawing.Color.Black;
                                }
                            }
                            else
                            {
                                cell.Text = String.Empty;
                                cell.BorderWidth = new Unit("1px");
                                cell.ForeColor = System.Drawing.Color.Black;
                                cell.BorderColor = System.Drawing.Color.Gray;
                                cell.BackColor = System.Drawing.Color.Black;
                            }
                        }
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    TableCell cell = e.Row.Cells[1];
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        cell = e.Row.Cells[i];
                        cell.HorizontalAlign = HorizontalAlign.Center;
                        cell.ForeColor = System.Drawing.Color.Black;
                        cell.BorderColor = System.Drawing.Color.Gray;
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "QCDefectGridView_RowDataBound", null);
                return;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(string.Format(Messages.UNKNOWN_EXCEPTION_THROWN, _screenName), _className, ex);
            }
        }
        private DataTable GetQCDefectData()
        {
            try
            {
                DataTable dtCacheQCDefect;
                DataTable dtQCDefect = new DataTable();
                if ((DataTable)Cache["QCDefectData"] != null)
                {
                    dtCacheQCDefect = (DataTable)Cache["QCDefectData"];                    
                    dtQCDefect = (dtCacheQCDefect.Select("LocationId=" + Request.QueryString["Location"]).CopyToDataTable());
                    dtQCDefect.Columns.Remove("LocationId");
                    double ColumnWidth = ScreenWidth / dtQCDefect.Columns.Count;
                    Session["ColWidth"] = ColumnWidth;
                }
                return dtQCDefect;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetTenPcsData", null);
                return null;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(string.Format(Messages.UNKNOWN_EXCEPTION_THROWN, _screenName), _className, ex);
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr", "javascript:ShrinkGrid();", true);
        }

        /// <summary>
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            lblError.Visible = true;
            lblError.Text = "(" + result + ")" + Constants.ERROR_MESSAGE_IN_REPORTS;
        }
    }
}