using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Configuration;
using Hartalega.FloorSystem.Business.Logic;
using System.IO;

namespace ScreenCapture
{
    public partial class Form1 : Form
    {
        private string OutputPath;
        private string OutputExtension = ".JPG";
        private int RetryCount = 0;
        private string MonthlyGenFlag;
        private string CompanyName;//06012020

        private List<string> WebSiteList = new List<string>();
        private List<string> WebSiteValList = new List<string>();
        private List<string> WebSiteFileNameList = new List<string>();

        public Form1()
        {
            InitializeComponent();
            LoadAppSettings();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //webBrowser1.Navigate(PQISite); // default run PQI 1st
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            string webSite = string.Empty;
            string webSiteVal = string.Empty;
            string webSiteFileName = string.Empty;

            try
            {
                for (int i = 0; i < WebSiteList.Count; i++)
                {
                    webSite = WebSiteList[i];
                    webSiteVal = WebSiteValList[i];
                    webSiteFileName = WebSiteFileNameList[i];

                    if (!string.IsNullOrEmpty(webSite))
                    {
                        // loop by Plant/Line
                        switch (webSiteFileName)
                        {
                            case "PQI":
                                webBrowser1.Navigate(webSite);
                                GetPage(webSiteVal, webSiteFileName);
                                break;
                            case "PQIMONTHLY":
                                GetPQIMonthlyPage(webSite, webSiteVal, webSiteFileName);
                                break;
                            case "DEFECT_P":
                            case "COSMETIC_P":
                            case "TENPCS_P":
                                GetPlantPage(webSite, webSiteVal, webSiteFileName);
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                LogErrorFile(OutputPath, "error.log", ex.ToString());
            }
            finally
            {
                webBrowser1.Dispose();
                this.Close();
            }            
        }

        private void GetPage(string webSiteVal, string webSiteFileName)
        {
            int refreshCount = 0;

            while (refreshCount < RetryCount)
            {
                refreshCount++;

                while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }

                if (webBrowser1.DocumentTitle.Contains(webSiteVal) && webBrowser1.DocumentText.Contains(CompanyName))
                {
                    PrintImage(webSiteFileName, OutputExtension);
                    break;
                }
                else
                {
                    webBrowser1.Refresh();
                }
            }
        }

        private void GetPQIMonthlyPage(string website, string webSiteVal, string webSiteFileName)
        {
            DateTime _now = DateTime.Now;
            string lineNumber = string.Empty;
            string fileName = string.Empty;
            string webSiteWithQueryStr = string.Empty;
            string dateNow = _now.AddMonths(-1).ToString("yyyyMM");
            string datePreviousMonth = _now.AddMonths(-1).ToString("dd/MM/yyyy");
                        
            DataTable dtLine = GetLineMaster();

            if (dtLine.Rows.Count > 0 && GetMonthlyGenerationFlag(_now))
            {
                // For each line number, do print
                foreach (DataRow row in dtLine.Rows)
                {
                    webSiteWithQueryStr = website;
                    lineNumber = row["LineNumber"].ToString();
                    fileName = lineNumber.Substring(1, 2) + dateNow;
                    webSiteWithQueryStr = webSiteWithQueryStr.Replace("_line_", lineNumber);
                    webSiteWithQueryStr = webSiteWithQueryStr.Replace("_date_", datePreviousMonth);

                    webBrowser1.Navigate(webSiteWithQueryStr);
                    GetPage(webSiteVal, fileName);
                }
            }
        }

        private void GetPlantPage(string website, string webSiteVal, string webSiteFileName)
        {
            string plant = string.Empty;
            string fileName = string.Empty;
            string webSiteWithQueryStr = string.Empty;

            DataTable dtLocation = GetLocationMaster();

            if (dtLocation.Rows.Count > 0)
            {
                // For each line number, do print
                foreach (DataRow row in dtLocation.Rows)
                {
                    webSiteWithQueryStr = website;
                    plant = row["LocationName"].ToString().Substring(1,1);
                    fileName = webSiteFileName + plant;
                    webSiteWithQueryStr = webSiteWithQueryStr.Replace("_plant_", plant);

                    webBrowser1.Navigate(webSiteWithQueryStr);
                    GetPage(webSiteVal, fileName);
                }
            }
        }

        private void PrintImage(string fileName, string fileNameExtension)
        {
            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics graphics = Graphics.FromImage(printscreen as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

            string output = Path.GetFullPath(OutputPath + fileName + fileNameExtension);

            printscreen.Save(output, ImageFormat.Png);
            //printscreen.Save(@"C:\PQI\IMAGES\" + fileName + fileNameExtension, ImageFormat.Png);

            printscreen.Dispose();
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private void LoadAppSettings()
        {
            OutputPath = GetAppKey("OutputPath");
            RetryCount = string.IsNullOrEmpty(GetAppKey("RetryCount")) ? 0 : int.Parse(GetAppKey("RetryCount"));
            MonthlyGenFlag = GetAppKey("MonthlyGenFlag").ToLower();
            CompanyName = GetAppKey("CompanyName");
            //0
            WebSiteList.Add(GetAppKey("PQISite"));
            WebSiteValList.Add(GetAppKey("PQISiteValidationMessage"));
            WebSiteFileNameList.Add(GetAppKey("PQISiteFileName"));
            //1
            WebSiteList.Add(GetAppKey("PQIMonthlySite"));
            WebSiteValList.Add(GetAppKey("PQIMonthlySiteValidationMessage"));
            WebSiteFileNameList.Add(GetAppKey("PQIMonthlySiteFileName"));
            //2
            WebSiteList.Add(GetAppKey("ProductionDefectSite"));
            WebSiteValList.Add(GetAppKey("ProductionDefectSiteValidationMessage"));
            WebSiteFileNameList.Add(GetAppKey("ProductionDefectSiteFileName"));
            //3
            WebSiteList.Add(GetAppKey("CosmeticDefectSite"));
            WebSiteValList.Add(GetAppKey("CosmeticDefectSiteValidationMessage"));
            WebSiteFileNameList.Add(GetAppKey("CosmeticDefectSiteFileName"));
            //4
            WebSiteList.Add(GetAppKey("TenPcsWeightSite"));
            WebSiteValList.Add(GetAppKey("TenPcsWeightSiteValidationMessage"));
            WebSiteFileNameList.Add(GetAppKey("TenPcsWeightSiteFileName"));
        }

        private string GetAppKey(string key)
        {
            string value = ConfigurationManager.AppSettings[key];

            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        private DataTable GetLineMaster()
        {
            return TVReportsBLL.GetLineMaster();
        }

        private DataTable GetLocationMaster()
        {
            return TVReportsBLL.GetLocationMaster();
        }

        private bool GetMonthlyGenerationFlag(DateTime _now)
        {
            bool flag = false;

            if (MonthlyGenFlag.Equals("true"))
            {
                if (_now.Hour >= 7 && _now.Hour < 8 && _now.Minute <= 15) // 7 a.m - 7:15 a.m
                {
                    flag = true;
                }
            }
            else // this is for manual run trigger
            {
                flag = true;
            }

            return flag;
        }

        private void LogErrorFile(string path, string fileNameWithExtension, string content)
        {
            Directory.CreateDirectory(path);

            using (StreamWriter writer = new StreamWriter(path + fileNameWithExtension, true))
            {
                writer.WriteLine("Message : " + content);
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
    }
}
