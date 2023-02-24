using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using System.Data;
using Hartalega.FloorSystem.Framework;
using System.Globalization;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Data.Sql;


namespace Hartalega.FloorSystem.Web.UI.TV
{
    public partial class RejectGloveSummary : System.Web.UI.Page
    {
        private static readonly int _secondRow = 2;
        private static readonly int _one = 1;
        private static readonly int _zero = 0;
        private static readonly string _backgroundColor = "#A6A6A6";
        private static string rptName = "Quick summary Online Rejection";
        private System.Data.DataTable _dtexport;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnexport.Visible = false;
                LoadPlantList();
            }
        }

        private void LoadPlantList()
        {
            ddlPlant.DataTextField = "LocationName";
            ddlPlant.DataValueField = "LocationId";
            ddlPlant.DataSource = TVReportsBLL.GetPlantsList();
            ddlPlant.DataBind();

            ddlPlant.Items.Insert(0, new ListItem("ALL", string.Empty));
        }

        protected void btnvwReport_Click(object sender, EventArgs e)
        {
            pnlReport.Visible = true;
            lblstartDate.Text = txtfrom.Text.Trim();
            lblendate.Text = txtTo.Text.Trim();

            //added by Cheah (2018-04-24)            
            DateTime dateFrom = parsedate(txtfrom.Text);
            DateTime dateTo = parsedate(txtTo.Text);
            lblShift.Text = ddlShift.SelectedItem.Text.Trim();

            lblPlant.Text = ddlPlant.SelectedItem.Text.Trim();

            switch (ddlShift.SelectedValue)
            {
                case "0":
                    dateFrom = dateFrom.AddHours(7);
                    dateTo = dateTo.AddDays(1).AddHours(7).AddSeconds(-1);
                    break;
                case "1":
                    dateTo = dateTo.AddDays(1).AddSeconds(-1);
                    break;
            }

            //end add

            System.Data.DataTable dt = TVReportsBLL.GetRejectGlovesummary(dateFrom, dateTo, ddlPlant.SelectedValue);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    hdnfrom.Value = txtfrom.Text.Trim();
                    hdnTo.Value = txtTo.Text.Trim();
                    hdnPlant.Value = ddlPlant.SelectedItem.Text;
                    gvRejectSummary.DataSource = dt;
                    gvRejectSummary.DataBind();
                    btnexport.Visible = true;
                }
                else
                {
                    btnexport.Visible = false;
                }
            }
        }


        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {

        }

        protected void btnexport_Click(object sender, EventArgs e)
        {

            #region HTMLStream;
            
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=MyFiles.xls");
            Response.Charset = "";
            this.EnableViewState = false;

            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            pnlReport.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End(); 
             
            #endregion
            _dtexport = TVReportsBLL.GetRejectGlovesummary(parsedate(hdnfrom.Value), parsedate(hdnTo.Value), hdnPlant.Value);
            ExportToExcel(_dtexport);
        }

        private DateTime parsedate(string dt)
        {
            return Convert.ToDateTime(DateTime.ParseExact(dt.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture));
        }

        private static void ExportToExcel(System.Data.DataTable dtColumnData)
        {
            List<string> lstColumnHeader = dtColumnData.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToList();
            Application excel;
            Workbook excelworkBook;
            Worksheet excelSheet;
            Range excelCellrange;
            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true;
                excel.DisplayAlerts = true;
                excelworkBook = excel.Workbooks.Add(Type.Missing);
                excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
                excelSheet.Name = rptName;
                int rowcount = _one;

                foreach (DataRow datarow in dtColumnData.Rows)
                {
                    rowcount += _one;
                    for (int i = _one; i <= dtColumnData.Columns.Count; i++)
                    {
                        // on the first iteration we add the column headers
                        if (rowcount == _secondRow)
                        {
                            excelSheet.Cells[_one, i] = lstColumnHeader[i - _one].ToString();
                            excelSheet.Cells.Font.Color = System.Drawing.Color.Black;
                        }
                        excelSheet.Cells[rowcount, i] = "'" + datarow[i - _one].ToString();
                        //for alternate rows
                        if (rowcount > _secondRow)
                        {
                            if (i == dtColumnData.Columns.Count)
                            {
                                if (rowcount % _secondRow == _zero)
                                {
                                    excelCellrange = excelSheet.Range[excelSheet.Cells[rowcount, _one], excelSheet.Cells[rowcount, dtColumnData.Columns.Count]];
                                }
                            }
                        }
                    }
                }
                // now we resize the columns
                excelCellrange = excelSheet.Range[excelSheet.Cells[_one, _one], excelSheet.Cells[(dtColumnData.Rows.Count + _one), dtColumnData.Columns.Count]];
                excelCellrange.EntireColumn.AutoFit();
                Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;

                //formatting the header
                excelCellrange = excelSheet.Range[excelSheet.Cells[_one, _one], excelSheet.Cells[_one, dtColumnData.Columns.Count]];
                excelCellrange.Interior.Color = System.Drawing.ColorTranslator.FromHtml(_backgroundColor);
                excelCellrange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                excelCellrange.Font.Bold = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}