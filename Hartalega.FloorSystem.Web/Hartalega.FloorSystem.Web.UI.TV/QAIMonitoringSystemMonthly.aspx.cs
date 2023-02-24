using System;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System.IO;
using System.Globalization;
using System.Drawing;

namespace Hartalega.FloorSystem.Web.UI.TV
{
    public partial class QAIMonitoringSystemMonthly: System.Web.UI.Page
    {
        /// <summary>
        /// Static Variables
        /// </summary>
        public static int count = Constants.ZERO;
        public static int NextPage = Constants.ZERO;
        public static string _screenName = "TVReports";
        public static string _className = "QAIMonitoringSystemMonthly";
        /// <summary>
        /// On Page Initialization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SetDatePicker", "SetDatePicker();", true);
                    txtDate.Text = string.IsNullOrEmpty(Request.QueryString["date"])?  DateTime.Now.ToString("dd/MM/yyyy") : Request.QueryString["date"].ToString();
                    LoadLineDropDownList();
                    if (!string.IsNullOrEmpty(Request.QueryString["line"]))
                    {
                        ddlLine.Text = Request.QueryString["line"].ToString();
                    }
                    GenerateReport();
                    lblError.Visible = false;                    
                    NextRefreshTime.Text = Constants.NEXT_REFRESH_TIME + Global._batchJobNextRunTime;
                    NextRotationTime.Text = Global._currentReportTime;
                    LastBatchJobRunTime.Text = Constants.LAST_BATCH_RUN_TIME + Global._batchJobLastRunTime;
                }                
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "Page_Load", null);
                return;
            }
        }

        private void LoadLineDropDownList()
        {
            DataTable dtLineMaster = GetLineMaster();

            ddlLine.DataSource = dtLineMaster;
            ddlLine.DataBind();
        }

        private bool LightOrDarkColor(Color cellColor)
        {
            try
            {
                // Counting the perceptive luminance - human eye favors green color... 
                double a = 1 - (0.299 * cellColor.R + 0.587 * cellColor.G + 0.114 * cellColor.B) / 255;

                if (a < 0.5)
                    return true; // bright colors - black font
                else
                    return false; // dark colors - white font

                //return Color.FromArgb(d, d, d);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Services.WebMethod]
        protected void GenerateReport()
        {
            try
            {
                this.Title = Constants.QAI_MONITORING_SYSTEM_MONTHLY;
                QAIGridView.RowDataBound += new GridViewRowEventHandler(QAIGridView_RowDataBound);
                QAIGridView.PageSize = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intQAIPageSize);
                QAIGridView.PageIndex = NextPage;
                QAIGridView.DataSource = GetQAIMonthlyDataByDate();
                QAIGridView.DataBind();
                QAIGridView.HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                QAIGridView.HeaderStyle.Height = new Unit("20px");
                for (int i = 0; i < QAIGridView.Rows.Count - 1; i++)
                {
                    if (QAIGridView.Rows[i].Cells[0].Text != QAIGridView.Rows[i + 1].Cells[0].Text)
                    {
                        QAIGridView.Rows[i].Style.Add("border-bottom", "4px solid DarkBlue");
                    }
                }
            }
            catch (FloorSystemException e)
            {
                ExceptionLogging(e, _screenName, _className, "GenerateReport", null);
                return;
            }
            catch (Exception ex)
            {
                //throw new FloorSystemException(string.Format(Messages.UNKNOWN_EXCEPTION_THROWN, _screenName), _className, ex);
                throw new Exception(ex.Message);
            }
        }

        void QAIGridView_RowDataBound(object o, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TableCell cell = e.Row.Cells[0];
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        cell = e.Row.Cells[i];
                        cell.HorizontalAlign = HorizontalAlign.Right;
                        cell.Width = new Unit("90px");
                        if (!string.IsNullOrEmpty(cell.Text))
                        {
                            if (cell.Text.Contains("L") || cell.Text.Contains("D"))
                            {
                                if (cell.Text.Contains("D"))
                                    cell.Text = cell.Text.Replace("D", string.Empty);
                                if (i == 0 || i == 1)
                                {
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                    cell.Font.Bold = true;
                                    cell.BorderWidth = new Unit("1px");
                                    cell.BackColor = System.Drawing.Color.LightGray;
                                    cell.BorderColor = System.Drawing.Color.White;
                                }
                                else if (Convert.ToString(cell.Text).Length == 2)
                                {
                                    //cell.Text = String.Empty;
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                    cell.ForeColor = System.Drawing.Color.White;
                                    cell.Font.Bold = true;
                                    cell.BorderWidth = new Unit("1px");
                                    cell.BackColor = System.Drawing.Color.Black;
                                    cell.BorderColor = System.Drawing.Color.White;
                                }
                            }
                            else if (Convert.ToString(cell.Text) == Convert.ToString(Constants.ZERO))
                            {
                                cell.Text = String.Empty;
                                cell.BorderWidth = new Unit("1px");
                                cell.BackColor = System.Drawing.Color.Black;
                                cell.BorderColor = System.Drawing.Color.White;
                            }
                            else if (Convert.ToString(cell.Text).Length == 2)
                            {
                                //cell.Text = String.Empty;
                                cell.ForeColor = System.Drawing.Color.White;
                                cell.Font.Bold = true;
                                cell.HorizontalAlign = HorizontalAlign.Center;
                                cell.BorderWidth = new Unit("1px");
                                cell.BackColor = System.Drawing.Color.Black;
                                cell.BorderColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                //cell.Text = String.Empty;
                                cell.BorderWidth = new Unit("1px");
                                DataTable dtColorData = (DataTable)Cache["ColorData"];
                                DataTable dtData = new DataTable();
                                dtData = dtColorData.Select("ColourId=" + cell.Text.ToString()).CopyToDataTable();
                                cell.BackColor = System.Drawing.Color.FromName(dtData.Rows[0]["Colour"].ToString());
                                cell.ForeColor = System.Drawing.Color.FromName(dtData.Rows[0]["Colour"].ToString());
                                cell.BorderColor = System.Drawing.Color.White;
                            }
                        }
                        else
                        {
                            cell.Text = String.Empty;
                            cell.BorderWidth = new Unit("1px");
                            cell.BackColor = System.Drawing.Color.Black;
                            cell.BorderColor = System.Drawing.Color.White;
                        }
                        if (i == 8 || i == 16 || i == 24)
                        {
                            cell.Style.Add("border-right-color", "Aqua");
                            cell.Style.Add("border-right-width", "4px");
                        }
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    TableCell cell = e.Row.Cells[2];
                    for (int i = 2; i < e.Row.Cells.Count; i++)
                    {
                        short result;
                        cell = e.Row.Cells[i];
                        string text = cell.Text;
                        cell.HorizontalAlign = HorizontalAlign.Center;
                        cell.BorderColor = System.Drawing.Color.White;
                        cell.Width = new Unit("90px");
                        if (Int16.TryParse(text.Substring(1), out result) == true &&
                            Convert.ToInt16(text.Substring(1)) == System.DateTime.Now.Hour)
                        {
                            cell.BorderWidth = new Unit("1px");
                            cell.BackColor = System.Drawing.Color.Blue;
                        }
                        //cell.Text = text.Substring(1);
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "QAIGridView_RowDataBound", null);
                return;
            }
            catch (Exception ex)
            {
               throw ex; // throw new FloorSystemException(string.Format(Messages.UNKNOWN_EXCEPTION_THROWN, _screenName), _className, ex);
            }
        }

        private DataTable GetQAIMonthlyDataByDate()
        {
            DateTime selectedDate = string.IsNullOrEmpty(txtDate.Text) ? DateTime.Now : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string selectedLine = ddlLine.SelectedValue;
            return TVReportsBLL.GetQAIMonthlyMonitoringDataByDate(selectedDate, selectedLine);
        }

        private DataTable GetLineMaster()
        {
            return TVReportsBLL.GetLineMaster();
        }
        //private int GetNextPage()
        //{
        //    if (NextPage > dtQAIMonitoringData.Rows.Count / Convert.ToInt32(FloorSystemConfiguration.GetInstance().intQAIPageSize))
        //        NextPage = 0;
        //    return NextPage;
        //}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetDatePicker", "SetDatePicker();", true);
            if (!string.IsNullOrEmpty(txtDate.Text))
            {
                GenerateReport();
            }
            else
            {

            }
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