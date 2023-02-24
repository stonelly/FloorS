using System;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Globalization;

namespace Hartalega.FloorSystem.Web.UI.TV
{
    public partial class Production10PcsAnalysisSystem : System.Web.UI.Page
    {
        /// <summary>
        /// Static Variables
        /// </summary>
        public static string _screenName = "TVReports";
        public static string _className = "Production10PcsAnalysisSystem";
        double ScreenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        DataTable dtTenPcsWeight;

        /// <summary>
        /// On Page Initialization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DateTime currentTime = DateTime.ParseExact(Global._currentReportTime, Global._dateformat, CultureInfo.InvariantCulture);
                lblError.Visible = false;
                NextRotationTime.Text = Global._currentReportTime;
                NextRefreshTime.Text = Constants.NEXT_REFRESH_TIME + Global._batchJobNextRunTime;
                LastBatchJobRunTime.Text = Constants.LAST_BATCH_RUN_TIME + Global._batchJobLastRunTime; 
                Location.Text = Constants.LOCATION + Global._locationList.First(obj => obj.IDField == Request.QueryString["Location"]).DisplayField.ToString();
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
                dtTenPcsWeight = GetTenPcsData();
                this.Title = Constants.TEN_PCS_WEIGHT_SYSTEM;
                TenPcsGridView.RowDataBound += new GridViewRowEventHandler(TenPcsGridView_RowDataBound);
                double ColumnWidth = ScreenWidth / dtTenPcsWeight.Columns.Count;
                Session["ColWidth"] = ColumnWidth;
                TenPcsGridView.DataSource = dtTenPcsWeight;
                TenPcsGridView.DataBind();
                TenPcsGridView.HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                TenPcsGridView.HeaderStyle.Height = new Unit("20px");
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
                    string xmlFilePath = FloorSystemConfiguration.GetInstance().TvReportsHistoryFolderPath + date.Substring(0, 4) +
                            Constants.SLASH + date.Substring(4, 2) + Constants.SLASH + date.Substring(6, 2) + Constants.SLASH + date.Substring(8, 2) +
                            Constants.SLASH + date.Substring(10, 2) + Constants.SLASH + Constants.TENPCSWEIGHTFILE;
                    if (!File.Exists(xmlFilePath))
                    {
                        lblError.Visible = true;
                        lblError.Text = Constants.FILE_NOT_PRESENT;
                    }
                    else
                    {
                        DataSet dtTenPcsData = new DataSet();
                        dtTenPcsData.ReadXml(xmlFilePath);
                        DataTable dtTenPcs = new DataTable();
                        dtTenPcs = (dtTenPcsData.Tables[0].Select("Location=" + Request.QueryString["Location"]).CopyToDataTable());
                        dtTenPcs.Columns.Remove("Location");
                        double ColumnWidth = ScreenWidth / dtTenPcs.Columns.Count;
                        Session["ColWidth"] = ColumnWidth;
                        this.Title = Constants.TEN_PCS_WEIGHT_SYSTEM;
                        TenPcsGridView.RowDataBound += new GridViewRowEventHandler(TenPcsGridView_RowDataBound);
                        TenPcsGridView.DataSource = dtTenPcs;
                        TenPcsGridView.DataBind();
                        TenPcsGridView.HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                        TenPcsGridView.HeaderStyle.Height = new Unit("20px");
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

        public void TenPcsGridView_RowDataBound(object o, GridViewRowEventArgs e)
        {
            try
            {
                // apply custom formatting to data cells
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // set formatting for the category cell
                    TableCell cell = e.Row.Cells[0];
                    // set formatting for value cells
                    for (int i = 0; i <= 3; i++)
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
                            decimal result;
                            cell = e.Row.Cells[i];
                            if (Decimal.TryParse(cell.Text, out result) == true && Convert.ToDecimal(cell.Text) == Constants.ZERO )
                            {
                                cell.Text = String.Format(Constants.QC_DECIMAL_FORMAT, Convert.ToDecimal(cell.Text));
                                cell.HorizontalAlign = HorizontalAlign.Center;
                                cell.Font.Bold = true;
                                cell.BorderWidth = new Unit("1px");
                                cell.ForeColor = System.Drawing.Color.Black;
                                cell.BorderColor = System.Drawing.Color.Gray;
                                cell.BackColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                if (i != 1)
                                {
                                    cell.Text = String.Format(Constants.QC_DECIMAL_FORMAT, Convert.ToDecimal(cell.Text));
                                }   
                                    cell = e.Row.Cells[i];
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                    cell.BorderWidth = new Unit("1px");
                                    cell.ForeColor = System.Drawing.Color.Green;
                                    cell.BorderColor = System.Drawing.Color.Gray;
                                    cell.BackColor = System.Drawing.Color.Black;
                               
                            }
                        }
                        if (i == 3)
                        {
                            cell.Style.Add("border-right-color", "Aqua");
                            cell.Style.Add("border-right-width", "4px");
                        }
                    }
                    for (int i = 4; i < e.Row.Cells.Count; i++)
                    {
                        cell = e.Row.Cells[i];
                        cell.HorizontalAlign = HorizontalAlign.Center;
                        if (cell.Text != "&nbsp;")
                        {
                            if (Convert.ToDecimal(cell.Text) == Constants.ZERO)
                            {
                                cell.HorizontalAlign = HorizontalAlign.Center;
                                cell.Font.Bold = true;
                                cell.BorderWidth = new Unit("1px");
                                cell.ForeColor = System.Drawing.Color.Black;
                                cell.BorderColor = System.Drawing.Color.Gray;
                                cell.BackColor = System.Drawing.Color.Black;
                            }
                            else if (Convert.ToDecimal(cell.Text) < Convert.ToDecimal(e.Row.Cells[2].Text))
                            {
                                cell.Text = String.Format(Constants.QC_DECIMAL_FORMAT, Convert.ToDecimal(cell.Text));
                                cell.HorizontalAlign = HorizontalAlign.Center;
                                cell.Font.Bold = true;
                                cell.BorderWidth = new Unit("1px");
                                cell.ForeColor = System.Drawing.Color.Yellow;
                                cell.BorderColor = System.Drawing.Color.Gray;
                                cell.BackColor = System.Drawing.Color.Black;
                            }
                            else if (Convert.ToDecimal(cell.Text) > Convert.ToDecimal(e.Row.Cells[3].Text))
                            {
                                cell.Text = String.Format(Constants.QC_DECIMAL_FORMAT, Convert.ToDecimal(cell.Text));
                                cell.HorizontalAlign = HorizontalAlign.Center;
                                cell.Font.Bold = true;
                                cell.BorderWidth = new Unit("1px");
                                cell.ForeColor = System.Drawing.Color.Red;
                                cell.BorderColor = System.Drawing.Color.Gray;
                                cell.BackColor = System.Drawing.Color.Black;
                            }
                            else if (Convert.ToDecimal(cell.Text) >= Convert.ToDecimal(e.Row.Cells[2].Text) && Convert.ToDecimal(cell.Text) <= Convert.ToDecimal(e.Row.Cells[3].Text))
                            {
                                cell.Text = String.Format(Constants.QC_DECIMAL_FORMAT, Convert.ToDecimal(cell.Text));
                                cell.HorizontalAlign = HorizontalAlign.Center;
                                cell.Font.Bold = true;
                                cell.BorderWidth = new Unit("1px");
                                cell.ForeColor = System.Drawing.Color.Green;
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
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    TableCell cell = e.Row.Cells[1];
                    // set formatting for value cells
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
                ExceptionLogging(ex, _screenName, _className, "TenPcsGridView_RowDataBound", null);
                return;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(string.Format(Messages.UNKNOWN_EXCEPTION_THROWN, _screenName), _className, ex);
            }
        }
        private DataTable GetTenPcsData()
        {
            try
            {

                DataTable dtCacheTenPcs = (DataTable)Cache["TenPcsData"];
                DataTable dtTenPcs = new DataTable();
                dtTenPcs = (dtCacheTenPcs.Select("Location=" + Request.QueryString["Location"]).CopyToDataTable());
                dtTenPcs.Columns.Remove("Location");
                double ColumnWidth = ScreenWidth / dtTenPcs.Columns.Count;
                Session["ColWidth"] = ColumnWidth;
                return dtTenPcs;
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
