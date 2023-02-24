using System;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Globalization;

namespace Hartalega.FloorSystem.Web.UI.TV
{
    public partial class CosmeticDefectAnalysisSystem : System.Web.UI.Page
    {
        DataTable dtCosmeticDefect;
        /// <summary>
        /// Static Variables
        /// </summary>
        public static string _screenName = "TVReports";
        public static string _className = "CosmeticDefectAnalysisSystem";
        double ScreenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DateTime currentTime = DateTime.ParseExact(Global._currentReportTime, Global._dateformat, CultureInfo.InvariantCulture);
                lblError.Visible = false;
                // Location.Text = Constants.LOCATION + Global._locationList.First(obj => obj.IDField == Request.QueryString["Location"]).DisplayField.ToString(); //KahHeng = 20200609
                if (Request.QueryString["Location"] == null)
                {
                    Location.Text = "All Plants";
                }
                else
                {
                    Location.Text = Constants.LOCATION + Global._locationList.First(obj => obj.IDField == Request.QueryString["Location"]).DisplayField.ToString();
                }
                //end KahHeng 20200609
                NextRotationTime.Text = Global._currentReportTime;
                NextRefreshTime.Text = Constants.NEXT_REFRESH_TIME + Global._batchJobNextRunTime;
                LastBatchJobRunTime.Text = Constants.LAST_BATCH_RUN_TIME + Global._batchJobLastRunTime;
                Timer1.Interval = (Global._refreshTime * Constants.SIXTY - (Convert.ToDateTime(CommonBLL.GetCurrentDateAndTimeFromServer()).Subtract(currentTime).Seconds)) * Constants.THOUSAND + 5000;
                Timer2.Interval = 1; //KahHeng 20200609
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
                dtCosmeticDefect = GetCosmeticDefectData();
                this.Title = Constants.COSMETIC_DEFECT_SYSTEM;
                CosmeticDefectGridView.RowDataBound += new GridViewRowEventHandler(CosmeticDefectGridView_RowDataBound);
                CosmeticDefectGridView.DataSource = dtCosmeticDefect;
                CosmeticDefectGridView.DataBind();
                CosmeticDefectGridView.HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                CosmeticDefectGridView.HeaderStyle.Height = new Unit("20px");
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
                        date.Substring(8, 2) + Constants.SLASH + date.Substring(10, 2) + Constants.SLASH + Constants.COSMETICDEFECTFILE;
                    if (!File.Exists(xmlFilePath))
                    {
                        lblError.Visible = true;
                        lblError.Text = Constants.FILE_NOT_PRESENT;
                    }
                    else
                    {
                        DataSet dtCosmetic = new DataSet();
                        dtCosmetic.ReadXml(xmlFilePath);
                        this.Title = Constants.COSMETIC_DEFECT_SYSTEM;
                        DataTable dtCosmeticDefect = new DataTable();
                        dtCosmeticDefect = (dtCosmetic.Tables[0].Select("LocationId=" + Request.QueryString["Location"]).CopyToDataTable());
                        dtCosmeticDefect.Columns.Remove("LocationId");
                        CosmeticDefectGridView.RowDataBound += new GridViewRowEventHandler(CosmeticDefectGridView_RowDataBound);
                        double ColumnWidth = ScreenWidth / dtCosmeticDefect.Columns.Count;
                        Session["ColWidth"] = ColumnWidth;
                        CosmeticDefectGridView.DataSource = dtCosmeticDefect;
                        CosmeticDefectGridView.DataBind();
                        CosmeticDefectGridView.HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                        CosmeticDefectGridView.HeaderStyle.Height = new Unit("20px");
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

        public void CosmeticDefectGridView_RowDataBound(object o, GridViewRowEventArgs e)
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
                        else if (i == 6)
                        {
                            cell = e.Row.Cells[i];
                            if (Convert.ToInt32(cell.Text) > Convert.ToInt32(FloorSystemConfiguration.GetInstance().intCosmeticDefectDPM))
                            {
                                cell.ForeColor = System.Drawing.Color.Red;
                            }

                            else if (Convert.ToInt32(cell.Text) == 0)
                                cell.ForeColor = System.Drawing.Color.Black;
                            else
                                cell.ForeColor = System.Drawing.Color.Green;
                            cell.Text = String.Format(Constants.NUMBER_FORMAT, Convert.ToInt32(cell.Text));
                            cell.HorizontalAlign = HorizontalAlign.Center;
                            cell.BorderWidth = new Unit("1px");
                            cell.BorderColor = System.Drawing.Color.Gray;
                            cell.BackColor = System.Drawing.Color.Black;
                        }
                        else
                        {
                            cell = e.Row.Cells[i];
                            cell.HorizontalAlign = HorizontalAlign.Center;
                            if (cell.Text != "&nbsp;")
                            {
                                if (i == 5)
                                {
                                    cell.BorderWidth = new Unit("1px");
                                    cell.ForeColor = System.Drawing.Color.Green;
                                    cell.BorderColor = System.Drawing.Color.Gray;
                                    cell.BackColor = System.Drawing.Color.Black;
                                }
                                else if (cell.Text.Contains("-"))
                                {
                                    string strCell = cell.Text;
                                    string[] substr = strCell.Split('-');
                                    cell.Text = substr[0] + Constants.BR + substr[1] + Constants.PERCENT;
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                    cell.Font.Bold = true;
                                    cell.BorderWidth = new Unit("1px");
                                    if (cell.Text.Contains(" 0%"))
                                        cell.ForeColor = System.Drawing.Color.Black;
                                    else
                                        cell.ForeColor = System.Drawing.Color.Green;
                                    cell.BorderColor = System.Drawing.Color.Gray;
                                    cell.BackColor = System.Drawing.Color.Black;
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
                if (e.Row.Cells[6].ForeColor.Name == Constants.RED_COLOR)
                {
                    for (int i = 1; i < 6; i++)
                    {
                        e.Row.Cells[i].ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "CosmeticDefectGridView_RowDataBound", null);
                return;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(string.Format(Messages.UNKNOWN_EXCEPTION_THROWN, _screenName), _className, ex);
            }
        }

        private DataTable GetCosmeticDefectData()
        {
            try
            {
                DataTable dtCacheQCDefect = (DataTable)Cache["CosmeticDefectData"];
                DataTable dtCosmeticDefect = new DataTable();
                //dtCosmeticDefect = (dtCacheQCDefect.Select("LocationId=" + Request.QueryString["Location"]).CopyToDataTable());
                //KahHeng 20200609 - Update location text to "All Plants" if location = 0
                if (Request.QueryString["Location"] == null)
                {
                    //dtCosmeticDefect = (dtCacheQCDefect);
                    dtCosmeticDefect = TVReportsBLL.GetCosmeticDefectData();
                    Timer1.Enabled = false;
                    Timer2.Enabled = false;
                }
                else
                {
                    dtCosmeticDefect = (dtCacheQCDefect.Select("LocationId=" + Request.QueryString["Location"]).CopyToDataTable());
                }
                //end KahHeng 20200609
                dtCosmeticDefect.Columns.Remove("LocationId");
                double ColumnWidth = ScreenWidth / dtCosmeticDefect.Columns.Count;
                Session["ColWidth"] = ColumnWidth;
                return dtCosmeticDefect;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetCosmeticDefectData", null);
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
        protected void Timer2_Tick(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr", "javascript:ShrinkGrid();", true);
            Timer2.Enabled = false;
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