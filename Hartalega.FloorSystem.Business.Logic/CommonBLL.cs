// <copyright file="CommonBLL.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
#region namespace
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Xml.Serialization;
using System.Xml;
#endregion

namespace Hartalega.FloorSystem.Business.Logic
{
    /// <summary>
    /// Common business logic class
    /// </summary>
    public class CommonBLL : Framework.Business.BusinessBase
    {
        #region Private Static Variables
        private static readonly int _secondRow = 2;
        private static readonly int _one = 1;
        private static readonly int _zero = 0;
        private static readonly string _backgroundColor = "#A6A6A6";
        private static DateTime _currentDateAndTime;
        private static DateTime _currentDateAndTimeSystem;
        private static TimeSpan _timeDifferenceBetweenServerAndSystem;
        #endregion

        #region Private Methods
        /// <summary>
        /// Increment the time every second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _currentDateAndTime = DateTime.Now.Add(_timeDifferenceBetweenServerAndSystem);
        }
        #endregion

        #region Public Methods

        //audit log
        public static void InsertAuditLog(string audlg, string updatecolumnlog)
        {

            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@XMLLog", audlg));
            PrmList.Add(new FloorSqlParameter("@UpdateColumnLog", updatecolumnlog));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Insert_AuditLog", PrmList);

        }

        public static Int64 InsertBatchAuditLog(decimal serialNumber, int referenceID, string process, string processFullName)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            PrmList.Add(new FloorSqlParameter("@ReferenceID", referenceID));
            PrmList.Add(new FloorSqlParameter("@Process", process));
            PrmList.Add(new FloorSqlParameter("@ProcessFullName", processFullName));
            Int64 auditLogID = Convert.ToInt64(FloorDBAccess.ExecuteScalar("USP_INS_BatchAuditLog", PrmList));
            return auditLogID;
        }

        public static void InsertEventLog(EventLogDTO evtlg, string screenname, string screenid)
        {
            // convert dataField to xml
            var objContainer = new Hartalega.FloorSystem.Business.Logic.EventLog.EventLogDataFieldContainer();
            List<Hartalega.FloorSystem.Business.Logic.EventLog.EventLogDataField> evtdatafield = new List<EventLog.EventLogDataField>();
            evtdatafield.Add(new Hartalega.FloorSystem.Business.Logic.EventLog.EventLogDataField() { Display = true, FieldValue = Constants.eventlogsource, ResourcesKey = "Display" });
            evtdatafield.Add(new Hartalega.FloorSystem.Business.Logic.EventLog.EventLogDataField() { Display = true, FieldValue = screenid, ResourcesKey = "FunctionId" });
            evtdatafield.Add(new Hartalega.FloorSystem.Business.Logic.EventLog.EventLogDataField() { Display = true, FieldValue = screenname, ResourcesKey = "FunctionName" });
            string xml = null;
            if (evtdatafield.Count > 0)
            {
                objContainer.DataFields = evtdatafield.ToArray(); 
                var xmlserializer = new XmlSerializer(typeof(EventLog.EventLogDataFieldContainer));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, objContainer);
                    xml = stringWriter.ToString();
                }
                 
            }
            string finalxml = CommonBLL.SerializeTOXML(evtlg);

            int _rowsaffected;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@XMLLog", finalxml));
            PrmList.Add(new FloorSqlParameter("@XMLData", xml));
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_Insert_EventLog", PrmList);

        }

        public static int GetScreenIdByScreenName(string screenName)
        {
            int screenId = Constants.ZERO;
            List<FloorSqlParameter> fspList = new List<FloorSqlParameter>();
            fspList.Add(new FloorSqlParameter("@ScreenName", screenName));
            screenId = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_ScreenIdForScreenName", fspList));
            return screenId;
        }

        /// <summary>
        /// ReloadWorkStationAndFloorSystemConfiguration
        /// </summary>
        public static void ReloadWorkStationAndFloorSystemConfiguration()
        {
            CommonBLL.GetWorkStationdetails(System.Net.Dns.GetHostName());
            CommonBLL.GetFloorSystemConfiguration();
        }

        ///// <summary>
        ///// ReloadWorkStationAndFloorSystemConfiguration
        ///// </summary>
        //public static void ReloadWorkStationAndFloorSystemConfiguration()
        //{
        //    CommonBLL.GetWorkStationdetails(System.Net.Dns.GetHostName());
        //    CommonBLL.GetFloorSystemConfiguration();
        //}

        /// <summary>
        /// Validate employee/operator id
        /// </summary>
        /// <param name="operatorId">Operator id</param>
        /// <returns>String - Employee name - by default it will be empty when the operator do not match</returns>
        public static string GetOperatorName(string operatorId, string screenname)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@operatorId", operatorId));
            PrmList.Add(new FloorSqlParameter("@screenname", screenname));
            var employeeName = FloorDBAccess.ExecuteScalar("USP_GET_OperatorName", PrmList);
            return employeeName == null ? Constants.INVALID_MESSAGE : Convert.ToString(employeeName);
        }

        /// <summary>
        /// GetOperator Name By ID
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public static string GetOperatorNameQAI(string operatorId)
        {
            string employeeName = string.Empty;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@EmployeeID", operatorId));
            employeeName = Convert.ToString(FloorDBAccess.ExecuteScalar("usp_OperatorName_Get", PrmList));
            return employeeName;
        }

        /// <summary>
        /// Validate employee/operator id
        /// </summary>
        /// <param name="operatorId">Operator id</param>
        /// <returns>String - Employee name - by default it will be empty when the operator do not match</returns>
        public static string GetOperatorName(string operatorId)
        {
            string employeeName = string.Empty;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@EmployeeID", operatorId));
            employeeName = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_OperatorName", PrmList));
            return employeeName;
        }

        /// <summary>
        /// Initialize local variable with server current date and time and associate a timer
        /// </summary>
        public static void InitializeCurrentDateAndTime()
        {
            _currentDateAndTime = GetCurrentDateAndTimeFromServer();
            _currentDateAndTimeSystem = DateTime.Now;
            _timeDifferenceBetweenServerAndSystem = _currentDateAndTime.Subtract(_currentDateAndTimeSystem);
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Elapsed += timer_Elapsed;
        }

        /// <summary>
        /// To get Glove Size by Type
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetSizeByGloveType(string gloveType)
        {
            return GetItemsByFilter("@gloveType", gloveType, "USP_SEL_SizeByGloveType", "sizeName", "sizeName");
        }

        /// <summary>
        /// To get Glove Size by Type
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetSizeByGloveTypeTumbling(string gloveType)
        {
            return GetItemsByFilter("@gloveType", gloveType, "USP_SEL_SizeByGloveTypeTumbling", "sizeName", "sizeName");
        }

        /// <summary>
        /// To get Glove Size 
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetSize()
        {
            return GetItemsByFilter(String.Empty, String.Empty, "USP_SEL_Size", "COMMONSIZE", "COMMONSIZE");
        }

        /// <summary>
        /// Get shift table record(s)
        /// </summary>
        /// <param name="shiftType">Shift group type</param>
        /// <returns>Data table as result</returns>
        public static List<DropdownDTO> GetShift(Framework.Constants.ShiftGroup shiftType)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@shiftType", shiftType.ToString()));
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_Shift", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "Id"),
                                DisplayField = FloorDBAccess.GetString(row, "Name"),
                                SelectedValue = FloorDBAccess.GetString(row, "CurrentShift")
                            }).ToList();

                }
            }
            return list;
        }

        /// <summary>
        /// Get shift table record(s) for HBC
        /// </summary>
        /// <param name="shiftType">Shift group type</param>
        /// <param name="shiftTime">Shift group type</param>
        /// <returns>Data table as result</returns>
        public static List<DropdownDTO> GetShiftByTime(Framework.Constants.ShiftGroup shiftType, string shiftTime)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@shiftType", shiftType.ToString()));
            PrmList.Add(new FloorSqlParameter("@shiftTime", DateTime.ParseExact(shiftTime, "HH00", null, System.Globalization.DateTimeStyles.None)));
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_ShiftByTime", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "Id"),
                                DisplayField = FloorDBAccess.GetString(row, "Name"),
                                SelectedValue = FloorDBAccess.GetString(row, "CurrentShift")
                            }).ToList();

                }
            }
            return list;
        }

        public static List<DropdownDTO> GetTierForRePrint(DateTime outputTime, string resourceGrp)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@outputTime", outputTime));
            PrmList.Add(new FloorSqlParameter("@resourceGrp", resourceGrp));
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_HBCRePrintTierSide", PrmList))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "TierSide"),
                                DisplayField = FloorDBAccess.GetString(row, "TierSide"),
                            }).ToList();

                }
            }
            return list;
        }

        /// <summary>
        /// Get shift table record(s)
        /// </summary>
        /// <param name="shiftType">Shift group type</param>
        /// <returns>Data table as result</returns>
        public static List<DropdownDTO> GetEnumText(string enumType)
        {
            return GetItemsByFilter("@enumType", enumType, "USP_SEL_Enum", "Value", "Text");
        }

        /// <summary>
        /// Get line by location
        /// </summary>
        /// <param name="location">Location code or name</param>
        /// <returns>List<string> as result</returns>
        public static List<DropdownDTO> GetLineByLocation(string location)
        {
            return GetItemsByFilter("@location", location, "USP_SEL_Line", "LineNumber", "LineNumber");
        }

        /// <summary>
        /// Get line by location
        /// </summary>
        /// <param name="location">Location code or name</param>
        /// <returns>List<string> as result</returns>
        public static List<DropdownDTO> GetAllLinesByLocation(string location)
        {
            return GetItemsByFilter("@location", location, "[USP_SEL_Line_All]", "LineNumber", "LineNumber");
        }

        /// To get Glove Size by Type and line //added by MYAdamas 20171109 to get valid size from production line based on line and type in normal batch card
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetSizeByGloveTypeAndLine(string gloveType, string line)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();

            PrmList.Add(new FloorSqlParameter("@gloveType", gloveType));
            PrmList.Add(new FloorSqlParameter("@line", line));
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_SIZEBYGLOVETYPEANDLINE", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "Size"),
                                DisplayField = FloorDBAccess.GetString(row, "Size"),
                                // SelectedValue = FloorDBAccess.GetString(row, "CurrentShift")
                            }).ToList();

                }
            }
            return list;
        }


        /// <summary>
        /// Get Resource Group for post online rejection batch to AX
        /// added on 11th Oct 2016 at 4:21PM by Max He, MH#1.n
        /// </summary>
        /// <param name="gloveType"></param>
        /// <returns></returns>
        public static ResourceGroupDTO GetResourceGroup(string lineNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@lineNumber", lineNumber));
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_ResourceGroup", PrmList);
            var retDto = new ResourceGroupDTO();
            if (dt != null && dt.Rows.Count > 0)
            {
                retDto.LocationName = FloorDBAccess.GetString(dt.Rows[0], "LocationName");
                retDto.LineDesc = FloorDBAccess.GetString(dt.Rows[0], "LineDesc");
            }
            return retDto;
        }

        /// <summary>
        /// Get Glove Description
        /// </summary>
        /// <param name="gloveType"></param>
        /// <returns></returns>
        public static string GetGloveDescription(string gloveType)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@gloveType", gloveType));
            var result = FloorDBAccess.ExecuteScalar("USP_GET_GloveDescByType", PrmList);
            return result == null ? Constants.INVALID_MESSAGE : Convert.ToString(result);
        }

        /// <summary>
        /// Validate Glove Type and Size based on Line
        /// </summary>
        /// <param name="line"></param>
        /// <param name="size"></param>
        /// <param name="gloveType"></param>
        /// <returns></returns>
        public static string ValidateGloveTypeSizeAndLine(string line, string size, string gloveType, bool IsReproductionModule = false)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@line", line));
            PrmList.Add(new FloorSqlParameter("@size", size));
            PrmList.Add(new FloorSqlParameter("@gloveType", gloveType));
            PrmList.Add(new FloorSqlParameter("@IsReproductionModule", IsReproductionModule));
            var result = FloorDBAccess.ExecuteScalar("USP_GET_LineGloveTypeSize", PrmList);
            return result == null ? Constants.INVALID_MESSAGE : Convert.ToString(result);
        }

        /// <summary>
        /// Get Size description
        /// </summary>
        /// <param name="location">Location code or name</param>
        /// <returns>Data table as result</returns>
        public static string GetSizeDescription(string size)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@size", size));
            var result = FloorDBAccess.ExecuteScalar("USP_GET_SizeDesc", PrmList);
            if (result == null || result.ToString() == String.Empty)
            {
                throw new FloorSystemException(Messages.SIZE_DESC_EXCEPTION, Constants.DAL, new ArgumentException());
            }
            else
                return result.ToString();
        }

        /// <summary>
        /// Get Workstation Details
        /// </summary>
        /// <param name="workStationName"></param>
        public static void GetWorkStationdetails(string workStationName)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@workstationName", workStationName));
            DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_WorkStationDetails", PrmList);
            string configData = FloorDBAccess.GetString(dt.Rows[0], "ConfigurationData");
            WorkStationDataConfiguration.GetInstance().UpdateInstanceWithJSON(configData);
            WorkStationDTO objWorkStation = WorkStationDTO.GetInstance();
            objWorkStation.Location = FloorDBAccess.GetString(dt.Rows[0], "locationName");
            objWorkStation.LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"]);
            objWorkStation.WorkStationNumber = FloorDBAccess.GetString(dt.Rows[0], "WorkStationName");
            objWorkStation.LocationAreaCode = FloorDBAccess.GetString(dt.Rows[0], "LocationAreaCode");
            objWorkStation.WorkStationId = FloorDBAccess.GetString(dt.Rows[0], "WorkStationId");
            objWorkStation.Area = FloorDBAccess.GetString(dt.Rows[0], "Area");
            objWorkStation.ModuleIds = FloorDBAccess.GetString(dt.Rows[0], "ModuleIds");
            objWorkStation.IsAdmin = FloorDBAccess.GetValue<Boolean>(dt.Rows[0], "IsAdmin");
            objWorkStation.Site = FloorDBAccess.GetString(dt.Rows[0], "Site");
            objWorkStation.LocationAreaId = FloorDBAccess.GetValue<int>(dt.Rows[0], "LocationAreaId");
        }

        public static int ConvertBooleanToInt(string s)
        {
            bool b = Convert.ToBoolean(s);
            return Convert.ToInt16(b);
        }

        public static void GetFloorSystemConfiguration()
        {
            object configDataObject = FloorDBAccess.ExecuteScalar("USP_GET_FloorSystemDetails");
            if (configDataObject != null)
            {
                string configData = Convert.ToString(configDataObject);
                FloorSystemConfiguration.GetInstance().UpdateInstanceWithJSON(configData);
            }
        }

        public static string CheckConfigurationKeys()
        {
            StringBuilder emptyKeys = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();
            Dictionary<string, object> applicationKeys = FloorSystemConfiguration.GetInstance().GetAllPropertyValues();
            var applicationValues = applicationKeys.Values;
            for (int i = Constants.ZERO; i < applicationKeys.Count; i++)
            {
                object keyValue = applicationKeys.Values.ElementAt(i);
                if (keyValue.Equals("0") || keyValue.Equals(null))
                {
                    emptyKeys.Append(applicationKeys.Keys.ElementAt(i));
                    emptyKeys.Append(",");
                }
            }
            if (emptyKeys.Length != Constants.ZERO)
            {
                errorMessage.Append(Messages.EMPTY_KEY_TEXT);
                errorMessage.Append(emptyKeys.Remove(emptyKeys.Length - Constants.ONE, Constants.ONE));
            }
            return Convert.ToString(errorMessage);
        }

        /// <summary>
        /// Check whether QAI is completed and not expired.
        /// </summary>
        /// <param name="serialNo">Serial Number</param>
        /// <returns>Status as result.It will not throw exception for null case.</returns>
        public static string ValidateSerialNoByQAIStatus(decimal serialNo)
        {
            int qaiExpiryDuration = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intQAIExpiryDuration);
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@qaiExpiryDuration", qaiExpiryDuration));
            object result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_SerialNo_QAI", lstParameters);
            if (Convert.ToString(result) == Constants.INCOMPLETE)
            {
                return Messages.QAI_INCOMPLETE;
            }
            else if (Convert.ToString(result) == Constants.EXPIRED)
            {
                return Messages.QAI_EXPIRED;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// This method is used to get the data for binding the Location Dropdown.
        /// </summary>
        /// <returns>DataTable consisting of LocationId and LocationName</returns>
        public static List<DropdownDTO> GetLocation()
        {
            return GetItemsByFilter(String.Empty, String.Empty, "USP_GET_Location", "LocationId", "LocationAreaCode");
        }

        /// <summary>
        /// This method is used to get the data for binding the Location Dropdown.
        /// </summary>
        /// <returns>DataTable consisting of LocationId and LocationName</returns>
        public static List<DropdownDTO> GetLocationIDandName()
        {
            return GetItemsByFilter(String.Empty, String.Empty, "USP_GET_LocationIDName", "LocationId", "LocationName");
        }

        /// <summary>
        /// This method is used to get the data for binding the Location Dropdown.
        /// </summary>
        /// <returns>DataTable consisting of LocationId and LocationName</returns>
        public static List<DropdownDTO> GetLocationName()
        {
            return GetItemsByFilter(String.Empty, String.Empty, "USP_GET_Location_Name", "LocationName", "LocationName");
        }

        /// <summary>
        /// Exports the Data To Excel
        /// </summary>
        /// <param name="dtColumnHeader"></param>
        /// <param name="strSheetName"></param>
        /// <param name="dtColumnData"></param>
        public static void ExportToExcel<T>(List<string> lstColumnHeader, string strSheetName, List<T> lstColumnData)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook excelworkBook;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet;
            Microsoft.Office.Interop.Excel.Range excelCellrange;
            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true;
                excel.DisplayAlerts = true;
                excelworkBook = excel.Workbooks.Add(Type.Missing);
                excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
                excelSheet.Name = strSheetName;
                int rowcount = _one;
                using (DataTable dtColumnData = ConvertListToDataTable(lstColumnData))
                {
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
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.EXPORTTOEXCEL_EXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
        }

        /// <summary>
        /// Converts List To DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns>DataTable</returns>
        public static DataTable ConvertListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            try
            {
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.LISTTODATATABLE_EXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
            return table;
        }

        /// <summary>
        /// Get Batch Weight
        /// </summary>
        /// <returns></returns>
        public static double GetBatchWeight()
        {
            return Math.Round(FSDeviceIntegration.GetBatchWeight(WorkStationDataConfiguration.GetInstance().psCOM, WorkStationDataConfiguration.GetInstance().psBaudRate, WorkStationDataConfiguration.GetInstance().psParity, WorkStationDataConfiguration.GetInstance().psDataBit, WorkStationDataConfiguration.GetInstance().psStopBit, WorkStationDataConfiguration.GetInstance().psReadSec, Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().bool_LBucket), Convert.ToDecimal(WorkStationDataConfiguration.GetInstance().dec_PallentinerWeight), WorkStationDataConfiguration.GetInstance().platformScalingSystem), Constants.TWO);
        }

        /// <summary>
        /// Get Batch Weight for Reject Gloves - LBucket is given as false as Reject Gloves are weighed with out palentiner
        /// </summary>
        /// <returns></returns>
        public static double GetRejectGlovesBatchWeight()
        {
            return FSDeviceIntegration.GetBatchWeight(WorkStationDataConfiguration.GetInstance().psCOM, WorkStationDataConfiguration.GetInstance().psBaudRate,
            WorkStationDataConfiguration.GetInstance().psParity, WorkStationDataConfiguration.GetInstance().psDataBit, WorkStationDataConfiguration.GetInstance().psStopBit,
            WorkStationDataConfiguration.GetInstance().psReadSec, false, Constants.ZERO, WorkStationDataConfiguration.GetInstance().platformScalingSystem);
        }

        /// <summary>
        /// Get Ten Pcs Weight
        /// </summary>
        /// <returns></returns>
        public static double GetTenPcsWeight()
        {
            return Math.Round(FSDeviceIntegration.GetTenPcsWeight(WorkStationDataConfiguration.GetInstance().ssCOM, WorkStationDataConfiguration.GetInstance().ssBaudRate,
            WorkStationDataConfiguration.GetInstance().ssParity, WorkStationDataConfiguration.GetInstance().ssDataBit, WorkStationDataConfiguration.GetInstance().ssStopBit,
            WorkStationDataConfiguration.GetInstance().ssReadSec, WorkStationDataConfiguration.GetInstance().smallScalingSystem), Constants.TWO);
        }

        /// <summary>
        /// To get Min Max details based on glove type and size
        /// </summary>
        /// <param name="gloveType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static TenPcsDTO GetMinMaxTenPcsWeight(string gloveType, string size)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            TenPcsDTO minMaxWeight = new TenPcsDTO();
            PrmList.Add(new FloorSqlParameter("@size", size));
            PrmList.Add(new FloorSqlParameter("@gloveType", gloveType));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_TenPiecesMinMaxWeight", PrmList))
            {
                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        minMaxWeight.MinWeight = FloorDBAccess.GetString(dt.Rows[0], "MinWeight");
                        minMaxWeight.MaxWeight = FloorDBAccess.GetString(dt.Rows[0], "MaxWeight");
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETMINMAXMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return minMaxWeight;
        }

        /// <summary>
        /// To get Min Max details based on glove type and size irrespective of stopped filter
        /// </summary>
        /// <param name="gloveType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static TenPcsDTO GetMinMaxTenPcsWeightTumbling(string gloveType, string size)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            TenPcsDTO minMaxWeight = new TenPcsDTO();
            PrmList.Add(new FloorSqlParameter("@size", size));
            PrmList.Add(new FloorSqlParameter("@gloveType", gloveType));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_TenPiecesMinMaxWeightTumbling", PrmList))
            {
                try
                {
                    minMaxWeight.MinWeight = FloorDBAccess.GetString(dt.Rows[0], "MinWeight");
                    minMaxWeight.MaxWeight = FloorDBAccess.GetString(dt.Rows[0], "MaxWeight");
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETMINMAXMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return minMaxWeight;
        }

        /// <summary>
        /// Online ByPass - Validate and Get details if SerialNumber is valid
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static BatchDTO ValidateAndGetDetailsBySerialNumber(decimal serialNumber)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@serialNumber", serialNumber));
            BatchDTO objBatchDTO = new BatchDTO();
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_GET_SerialNumberOnline", PrmList))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        objBatchDTO.BatchNumber = FloorDBAccess.GetString(dt.Rows[0], "BatchNumber");
                        objBatchDTO.ShiftName = FloorDBAccess.GetString(dt.Rows[0], "Shift");
                        objBatchDTO.Size = FloorDBAccess.GetString(dt.Rows[0], "Size");
                        objBatchDTO.GloveType = FloorDBAccess.GetString(dt.Rows[0], "GloveType");
                        objBatchDTO.GloveTypeDescription = FloorDBAccess.GetString(dt.Rows[0], "Description");
                        objBatchDTO.ShortDate = Convert.ToDateTime(dt.Rows[0]["BatchCardDate"]).ToString(Constants.START_DATE);
                        objBatchDTO.ShortTime = Convert.ToDateTime(dt.Rows[0]["BatchCardDate"]).ToString(Constants.START_TIME);
                        objBatchDTO.Side = FloorDBAccess.GetString(dt.Rows[0], "TierSide");
                        objBatchDTO.QCType = FloorDBAccess.GetString(dt.Rows[0], "QCType");
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSERIALDETAILSMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return objBatchDTO;
        }

        /// <summary>
        /// Get Online ByPass reasons
        /// </summary>
        /// <param name="reasonType"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetReasons(string reasonType, string system)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@reasonType", reasonType));
            PrmList.Add(new FloorSqlParameter("@system", system));
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_ReasonsBySystem", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "ReasonId"),
                                DisplayField = FloorDBAccess.GetString(row, "ReasonText")
                            }).ToList();
                }
            }
            return list;
        }

        public static List<DropdownDTO> GetEnumWithID(string enumType)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@enumType", enumType));

            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_EnumByID", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "EnumId"),
                                DisplayField = FloorDBAccess.GetString(row, "EnumValue")
                            }).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// To get continously ticking Date Time
        /// </summary>
        /// <returns></returns>
        public static string GetContinouslyTickingDate()
        {
            string dateFormat;
            try
            {
                dateFormat = ConfigurationManager.AppSettings["dateFormat"];
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GETDATEMETHODEXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
            return CommonBLL.GetCurrentDateAndTimeFromServer().ToString(dateFormat);
        }

        /// <summary>
        /// Logging exception to Db
        /// </summary>
        /// <param name="floorException"></param>
        /// <param name="screenName"></param>
        /// <param name="UiClassName"></param>
        /// <param name="uiControl"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int LogExceptionToDB(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            if (floorException.CanLogExceptionToDB)
            {
                floorException.screenName = screenName;
                floorException.uiClassName = UiClassName;
                floorException.uiControlName = uiControl;
                floorException.WorkStationId = WorkStationDTO.GetInstance().WorkStationId;
                floorException.baseexception = floorException.GetBaseException().ToString();
                return floorException.LogExceptionToDB(parameters); // you need to return id in floor system exception. So code in floor system exception class will be changed 
            }
            return Constants.ZERO;
        }

        /// <summary>
        /// Get Batch details based on Serial Number.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns>DataTable as resut</returns>
        public static BatchDTO GetBatchScanInDetails(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_BatchDetails_ScanIn", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new BatchDTO
                            {
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                Line = FloorDBAccess.GetString(row, "Line"),
                                ShiftName = FloorDBAccess.GetString(row, "Name"),
                                Size = FloorDBAccess.GetString(row, "Size"),
                                TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPCsWeight"),
                                BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                TotalPcs = FloorDBAccess.GetValue<int>(row, "TotalPCs"),
                                QCType = FloorDBAccess.GetString(row, "QCType"),
                                QCTypeDescription = FloorDBAccess.GetString(row, "QCTypeDescription"),
                                ReworkCount = FloorDBAccess.GetValue<Int32>(row, "Rework"),
                                PTTenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "PTTenPCsWeight"),
                                PTBatchWeight = FloorDBAccess.GetValue<Decimal>(row, "PTBatchWeight"),
                                HotBox = FloorDBAccess.GetValue<Int32>(row, "HotBox"),
                                Protein = FloorDBAccess.GetValue<Int32>(row, "Protein"),
                                Powder = FloorDBAccess.GetValue<Int32>(row, "Powder"),
                                BatchType = FloorDBAccess.GetString(row, "BatchType"),
                                PackedPcs = FloorDBAccess.GetValue<int>(row, "PackedPcs")
                            }).SingleOrDefault();
            }

            return objBatch;
        }

        /// <summary>
        /// Get Glove Type
        /// </summary>
        /// <returns>List of Glove Type</returns>
        public static List<DropdownDTO> GetGloveType()
        {
            return GetItemsByFilter(String.Empty, String.Empty, "USP_SEL_GloveType", "GloveCode", "GloveCode");
        }

        public static List<DropdownDTO> GetFormerType()
        {
            return GetItemsByFilter(String.Empty, String.Empty, "USP_SEL_FormerType", "CODE", "NAME");
        }

        public static List<DropdownDTO> GetLatexType()
        {
            return GetItemsByFilter(String.Empty, String.Empty, "USP_SEL_LatexType", "ITEMID", "ITEMNAME");
        }

        /// <summary>
        /// Get Glove Type
        /// </summary>
        /// <returns>List of Glove Type</returns>
        public static List<DropdownDTO> GetGloveType(string gloveType)
        {
            return GetItemsByFilter("@gloveType", gloveType, "USP_SEL_ChangeGloveType", "TOGLOVECODE", "TOGLOVECODE"); //#Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product)
        }

        /// <summary>
        /// Method to Print
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="barcodeText"></param>
        /// <param name="batchNumber"></param>
        /// <param name="batchWeight"></param>
        /// <param name="size"></param>
        /// <param name="tenPcsWeight"></param>
        /// <param name="tenPcsMsg"></param>
        /// <param name="gloveDesc"></param>
        /// <param name="side"></param>
        /// <param name="template"></param>
        /// <param name="isReprint"></param>
        public static void PrintDetails(string dateTime, string barcodeText, string batchNumber, string batchWeight, string size, string tenPcsWeight, bool tenPcsMsg, string gloveDesc, string side, string template, bool isReprint = false, bool batchWeightMsg = false, string resource = "")
        {
            PrintDTO printData = new PrintDTO();
            printData.DateTime = dateTime;
            printData.BatchNumber = batchNumber;
            printData.BatchWeight = batchWeight;
            printData.Size = size;
            printData.TenPcsWeight = tenPcsWeight;
            printData.TenPcsMsg = tenPcsMsg;
            printData.GloveDesc = gloveDesc;
            printData.TierSide = side;
            printData.SerialNumber = barcodeText;
            printData.Template = template;
            printData.IsReprint = isReprint;
            printData.BatchWeightMsg = batchWeightMsg;
            printData.Resource = resource;
            FSDeviceIntegration deviceInt = new FSDeviceIntegration();
            deviceInt.Print(printData);
            deviceInt = null;
        }

        /// <summary>
        /// Get Activity Type List
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetActivityType()
        {
            return GetItemsByFilter(String.Empty, String.Empty, "usp_ActivityTypeMaster_Get", "Id", "ActivityType");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetQCType()
        {
            return GetItemsByFilter(String.Empty, String.Empty, "USP_SEL_QCType", "Description", "QCType");
        }

        /// <summary>
        /// Gets All QC types event stopped also
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetQCTypeALL()
        {
            return GetItemsByFilter(String.Empty, String.Empty, "USP_SEL_QCTypeALL", "Description", "QCType");
        }

        /// <summary>
        /// Get Packing Size
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetPackingSize()
        {
            return GetItemsByFilter(String.Empty, String.Empty, "usp_PackingSize_Get", "size", "size");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static QAIDTO GetBatchNumberBySerialNumber(Decimal serialNumber)
        {
            QAIDTO qaiBatch = new QAIDTO();
            if (Convert.ToString(serialNumber).Length != 10)
            {
                return qaiBatch;
            }
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            using (DataTable table = FloorDBAccess.ExecuteDataTable("USP_SEL_QAIDetailsBySerialNo", PrmList))
            {
                try
                {
                    if (table != null && table.Rows.Count > 0)
                    {
                        qaiBatch.SerialNo = Convert.ToString(serialNumber);
                        qaiBatch.BatchNo = FloorDBAccess.GetString(table.Rows[0], "BatchNumber");
                        qaiBatch.InnerBoxes = 0; // #AZ 27/05/2018 1.n InnerBoxes set at print HBC.
                        qaiBatch.PackingSize = "0"; // #AZ 27/05/2018 2.n PackingSize set at print HBC.

                        if (!Convert.IsDBNull(table.Rows[0]["IsOnline"]))
                        {
                            qaiBatch.Isonline = (Boolean?)table.Rows[0]["IsOnline"];
                        }
                        qaiBatch.InspectorId = FloorDBAccess.GetString(table.Rows[0], "QAIInspectorId");
                        qaiBatch.QCType = FloorDBAccess.GetString(table.Rows[0], "QCType");

                        qaiBatch.GloveType = FloorDBAccess.GetString(table.Rows[0], "GloveType");
                        qaiBatch.Size = FloorDBAccess.GetString(table.Rows[0], "Size");

                        if (!Convert.IsDBNull(table.Rows[0]["BatchWeight"]))
                        {
                            if (Convert.ToDecimal(table.Rows[0]["BatchWeight"]) > 0)
                            {
                                qaiBatch.BatchWeight = Math.Round(Convert.ToDecimal(table.Rows[0]["BatchWeight"]), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                qaiBatch.BatchWeight = Convert.ToDecimal(table.Rows[0]["BatchWeight"]);
                            }
                        }

                        qaiBatch.WTSamplingSize = FloorDBAccess.GetString(table.Rows[0], "WTSampliingSize");
                        qaiBatch.VTSamplingSize = FloorDBAccess.GetString(table.Rows[0], "VTSamplingSize");
                        if (!Convert.IsDBNull(table.Rows[0]["TenPCsWeight"]))
                        {
                            if (Convert.ToDecimal(table.Rows[0]["TenPCsWeight"]) > 0)
                            {
                                qaiBatch.TenPcsWeight = Math.Round(Convert.ToDecimal(table.Rows[0]["TenPCsWeight"]), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                qaiBatch.TenPcsWeight = Convert.ToDecimal(table.Rows[0]["TenPCsWeight"]);
                            }
                        }

                        if (!Convert.IsDBNull(table.Rows[0]["QAIDate"]))
                        {
                            qaiBatch.QAIDate = (DateTime?)table.Rows[0]["QAIDate"];
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.BATCH_DETAILS_SERIALNUMBER, Constants.BUSINESSLOGIC, ex);
                }
            }
            return qaiBatch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static QAIDTO GetBatchNumberBySerialNumberEditOnlineBatchCard(Decimal serialNumber, string screenNameInnerTenPcs, string screenNameEditOnlineBatchCardInfo)
        {
            QAIDTO qaiBatch = new QAIDTO();
            if (Convert.ToString(serialNumber).Length != 10)
            {
                return qaiBatch;
            }
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@SerialNumber", serialNumber));
            PrmList.Add(new FloorSqlParameter("@ScreenNameInnerTenPcs", screenNameInnerTenPcs));
            PrmList.Add(new FloorSqlParameter("@ScreenNameEditOnlineBatchCardInfo", screenNameEditOnlineBatchCardInfo));

            using (DataTable table = FloorDBAccess.ExecuteDataTable("USP_SEL_QAIDetailsBySerialNoEditOnlineBatchCard", PrmList))
            {
                try
                {
                    if (table != null && table.Rows.Count > 0)
                    {
                        qaiBatch.SerialNo = Convert.ToString(serialNumber);
                        qaiBatch.BatchNo = FloorDBAccess.GetString(table.Rows[0], "BatchNumber");

                        if (!Convert.IsDBNull(table.Rows[0]["IsOnline"]))
                        {
                            qaiBatch.Isonline = (Boolean?)table.Rows[0]["IsOnline"];
                        }
                        qaiBatch.InspectorId = FloorDBAccess.GetString(table.Rows[0], "QAIInspectorId");
                        qaiBatch.QCType = FloorDBAccess.GetString(table.Rows[0], "QCType");

                        qaiBatch.GloveType = FloorDBAccess.GetString(table.Rows[0], "GloveType");
                        qaiBatch.Size = FloorDBAccess.GetString(table.Rows[0], "Size");

                        if (!Convert.IsDBNull(table.Rows[0]["BatchWeight"]))
                        {
                            if (Convert.ToDecimal(table.Rows[0]["BatchWeight"]) > 0)
                            {
                                qaiBatch.BatchWeight = Math.Round(Convert.ToDecimal(table.Rows[0]["BatchWeight"]), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                qaiBatch.BatchWeight = Convert.ToDecimal(table.Rows[0]["BatchWeight"]);
                            }
                        }

                        qaiBatch.WTSamplingSize = FloorDBAccess.GetString(table.Rows[0], "WTSampliingSize");
                        qaiBatch.VTSamplingSize = FloorDBAccess.GetString(table.Rows[0], "VTSamplingSize");
                        if (!Convert.IsDBNull(table.Rows[0]["TenPCsWeight"]))
                        {
                            if (Convert.ToDecimal(table.Rows[0]["TenPCsWeight"]) > 0)
                            {
                                qaiBatch.TenPcsWeight = Math.Round(Convert.ToDecimal(table.Rows[0]["TenPCsWeight"]), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                qaiBatch.TenPcsWeight = Convert.ToDecimal(table.Rows[0]["TenPCsWeight"]);
                            }
                        }

                        if (!Convert.IsDBNull(table.Rows[0]["QAIDate"]))
                        {
                            qaiBatch.QAIDate = (DateTime?)table.Rows[0]["QAIDate"];
                        }

                        if (!Convert.IsDBNull(table.Rows[0]["InnerBox"]))
                        {
                            qaiBatch.InnerBoxes = (int)table.Rows[0]["InnerBox"];
                        }

                        if (!Convert.IsDBNull(table.Rows[0]["TotalPCs"]))
                        {
                            qaiBatch.TotalPCs = (int)table.Rows[0]["TotalPCs"];
                        }

                        if (!Convert.IsDBNull(table.Rows[0]["BatchWeight"]))
                        {
                            qaiBatch.BatchWeight_NoRounding = (decimal)table.Rows[0]["BatchWeight"];
                        }

                        if (!Convert.IsDBNull(table.Rows[0]["TenPcsWeight"]))
                        {
                            qaiBatch.TenPcsWeight_NoRounding = (decimal)table.Rows[0]["TenPcsWeight"];
                        }

                        qaiBatch.PackingSize = FloorDBAccess.GetString(table.Rows[0], "PackingSize");
                        qaiBatch.HBSamplingSize = FloorDBAccess.GetString(table.Rows[0], "HBSamplingSize");
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.BATCH_DETAILS_SERIALNUMBER, Constants.BUSINESSLOGIC, ex);
                }
            }
            return qaiBatch;
        }

        /// <summary>
        /// Get list of Packing size values for WT and VT
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetPackingSize(string gloveType, string size)
        {
            List<DropdownDTO> list = new List<DropdownDTO>(); ;
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@GloveType", gloveType));
            lstParameters.Add(new FloorSqlParameter("@Size", size));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_PACKINGSIZE", lstParameters))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            list.Add(new DropdownDTO() { IDField = Convert.ToString(dr["Size"]), DisplayField = Convert.ToString(dr["Size"]) });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.PACKING_SIZE_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        /// <summary>
        /// Get list of Edit Reasons
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetEditReasons(string reasonType, string moduleName)
        {
            List<DropdownDTO> list = new List<DropdownDTO>();
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@ReasonType", reasonType));
            lstParameters.Add(new FloorSqlParameter("@ModuleName", moduleName));
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_QAIEDITREASONS", lstParameters))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            list.Add(new DropdownDTO() { IDField = Convert.ToString(dr["ReasonId"]), DisplayField = Convert.ToString(dr["ReasonText"]) });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.QAI_EDIT_REASONS_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }

        /// <summary>
        /// Get ModuleId
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public static string GetModuleId(string moduleName)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@moduleName", moduleName));
            string result = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_ModuleId", lstParameters));
            return result;
        }

        /// <summary>
        /// Get Sub Module Id
        /// </summary>
        /// <returns></returns>
        public static string GetSubModuleId(string subModuleName)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@subModuleName", subModuleName));
            string result = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_SubModuleId", lstParameters));
            return result;
        }

        /// <summary>
        /// Serializes Class object to XML
        /// </summary>
        /// <param name="classObject"></param>
        /// <returns></returns>
        public static string SerializeTOXML(object classObject)
        {
            if (classObject == null) return null;

            using (StringWriter stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(classObject.GetType());
                serializer.Serialize(stringwriter, classObject);
                return stringwriter.ToString();
            }
        }

        /// <summary>
        /// Get Current date and time
        /// </summary>
        /// <returns>Current date and time from server</returns>
        public static DateTime GetCurrentDateAndTime()
        {
            return _currentDateAndTime;
        }

        /// <summary>
        /// Get Current date and time from server
        /// </summary>
        /// <returns>Current date and time from server</returns>
        public static DateTime GetCurrentDateAndTimeFromServer()
        {
            return Convert.ToDateTime(FloorDBAccess.ExecuteScalar("USP_GET_CurrentDateAndTime"));
        }

        /// <summary>
        /// Get Current date and hour from server
        /// </summary>
        /// <returns>Current date and hour from server</returns>
        public static DateTime GetCurrentDateAndHourFromServer()
        {
            return Convert.ToDateTime(FloorDBAccess.ExecuteScalar("USP_GET_CurrentDateAndHour"));
        }

        /// <summary>
        /// Get Reason type id for Reason type
        /// </summary>
        /// <param name="reasonType">Reason Type</param>
        /// <returns>ReasonTypeId for ReasonType</returns>
        public static int GetReasonTypeIdForReasonType(string reasonType)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@ReasonType", reasonType));
            int reasonTypeId = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GET_ReasonTypeIdForReasonType", lstParameters));
            return reasonTypeId;
        }

        /// <summary>
        /// To Get Resource details by SN
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static DataSet GetResourceBySerialNo(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            return FloorDBAccess.ExecuteDataSet("USP_GET_ResourceBySerialNo", lstParameters);
        }

        /// <summary>
        /// To Get Batch Order by SN
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static string GetBatchOrderBySerialNo(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            return Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_BatchOrderBySerialNo", lstParameters));
        }

        /// <summary>
        /// To Get Complete Batch details by SN & Resource
        /// </summary>
        /// <param name="serialNo, resource"></param>
        /// <returns></returns>
        public static BatchDTO GetCompleteBatchDetailsByResource(decimal serialNo, string resource) //insert seqno
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@resource", resource));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_CompleteBatchDetailsByResource", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new BatchDTO
                            {
                                SeqNo = FloorDBAccess.GetValue<Int32>(row, "SeqNo"),
                                SerialNumber = Convert.ToString(serialNo),
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                BatchOrder = FloorDBAccess.GetString(row, "BatchOrder"),
                                Resource = FloorDBAccess.GetString(row, "Resource"),
                                GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                Line = FloorDBAccess.GetString(row, "LineId"),
                                ShiftName = FloorDBAccess.GetString(row, "ShiftName"),
                                Size = FloorDBAccess.GetString(row, "Size"),
                                TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPCsWeight"),
                                BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                TotalPcs = FloorDBAccess.GetValue<Int32>(row, "TotalPCs"),
                                IsOnline = FloorDBAccess.GetValue<bool>(row, "IsOnline"),
                                QCType = FloorDBAccess.GetString(row, "QCType"),
                                Module = FloorDBAccess.GetString(row, "ModuleId"),
                                SubModule = FloorDBAccess.GetString(row, "SubModuleId"),
                                BatchType = FloorDBAccess.GetString(row, "BatchType"),
                                Location = FloorDBAccess.GetString(row, "locationname"),
                                BatchCarddate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate"),
                                Area = FloorDBAccess.GetString(row, "Area"),
                                Pool = FloorDBAccess.GetString(row, "Pool"), //#AZ 05/06/2018 Rework additional info.
                                RouteCategory = FloorDBAccess.GetString(row, "RouteCategory"), //#AZ 05/06/2018 Rework additional info.
                                DeliveryDate = FloorDBAccess.GetValue<DateTime>(row, "DeliveryDate"), //#AZ 05/06/2018 Rework additional info.
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        /// <summary>
        /// To Get Complete Batch details
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static BatchDTO GetCompleteBatchDetails(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_CompleteBatchDetails", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new BatchDTO
                            {
                                SeqNo = 1, //#AZ 05/06/2018 Non HBC only have 1 seq.
                                SerialNumber = Convert.ToString(serialNo),
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                Line = FloorDBAccess.GetString(row, "LineId"),
                                ShiftName = FloorDBAccess.GetString(row, "ShiftName"),
                                Size = FloorDBAccess.GetString(row, "Size"),
                                TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPCsWeight"),
                                BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                TotalPcs = FloorDBAccess.GetValue<Int32>(row, "TotalPCs"),
                                IsOnline = FloorDBAccess.GetValue<bool>(row, "IsOnline"),
                                QCType = FloorDBAccess.GetString(row, "QCType"),
                                Module = FloorDBAccess.GetString(row, "ModuleId"),
                                SubModule = FloorDBAccess.GetString(row, "SubModuleId"),
                                BatchType = FloorDBAccess.GetString(row, "BatchType"),
                                Location = FloorDBAccess.GetString(row, "locationname"),
                                BatchCarddate = FloorDBAccess.GetValue<DateTime>(row, "BatchCardDate"),
                                Area = FloorDBAccess.GetString(row, "Area"),
                                Pool = FloorDBAccess.GetString(row, "Pool"), //#AZRUL 05/06/2018 Rework additional info.
                                RouteCategory = FloorDBAccess.GetString(row, "RouteCategory"), //#AZRUL 05/06/2018 Rework additional info.
                                DeliveryDate = FloorDBAccess.GetValue<DateTime>(row, "DeliveryDate"), //#AZRUL 05/06/2018 Rework additional info.
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        public static ON2GBatchDTO GetCompleteON2GBatchDetails(decimal serialNo, int location , string line, DateTime selectedDateTime)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@location", location));
            lstParameters.Add(new FloorSqlParameter("@line", line));
            lstParameters.Add(new FloorSqlParameter("@selectedDateTime", selectedDateTime));
            ON2GBatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_CompleteON2GBatchDetails", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new ON2GBatchDTO
                            {
                                //SeqNo = 1, //#AZ 05/06/2018 Non HBC only have 1 seq.
                                SerialNumber = Convert.ToString(serialNo),
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                Plant = FloorDBAccess.GetString(row, "Plant"),
                                CurrentDateandTime = FloorDBAccess.GetValue<DateTime>(row, "CurrentDateandTime"),
                                LineId = FloorDBAccess.GetString(row, "LineId"),
                                ShiftId = FloorDBAccess.GetValue<Int32>(row, "ShiftId"),
                                ShiftName = FloorDBAccess.GetString(row, "ShiftName"),
                                Resource = FloorDBAccess.GetString(row, "Resource"),
                                GloveCode = FloorDBAccess.GetString(row, "GloveCode"),
                                Size = FloorDBAccess.GetString(row, "Size"),
                                BatchOrder = FloorDBAccess.GetString(row, "BatchOrder"),
                                PackingSize = FloorDBAccess.GetValue<Int32>(row, "PackingSize"),
                                InnerBox = FloorDBAccess.GetValue<Int32>(row, "InnerBox"),
                                WorkStationId = FloorDBAccess.GetValue<Int32>(row, "LocationId"),
                               
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        public static Boolean GetDowngradeStatusFromDB(string serialNumber)
        {
            //return Framework.Database.FloorDBAccess.GetDowngradeStatus(serialNumber);

            bool result = false;
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNumber", serialNumber));
            string _batch_type = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_CheckDowngradeValidation", lstParameters));

            if ((_batch_type.Trim() == "2G") || (_batch_type.Trim() == "RJ"))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// To Get Rework Order details
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="qcType"></param>
        /// <returns></returns>
        public static BatchDTO GetReworkOrderDetails(decimal serialNo, string qcType)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@qcType", qcType));
            BatchDTO objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_ReworkOrderDetails", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new BatchDTO
                            {
                                SerialNumber = Convert.ToString(serialNo),
                                BatchNumber = FloorDBAccess.GetString(row, "BatchNumber"),
                                Size = FloorDBAccess.GetString(row, "Size"),
                                TotalPcs = Convert.ToInt32(FloorDBAccess.GetValue<Decimal>(row, "TotalPCs")),
                                GloveType = FloorDBAccess.GetString(row, "GloveType"),
                                DeliveryDate = FloorDBAccess.GetValue<DateTime>(row, "DeliveryDate"),
                                Pool = FloorDBAccess.GetString(row, "Pool"),
                                RouteCategory = FloorDBAccess.GetString(row, "RouteCategory"),
                                Area = FloorDBAccess.GetString(row, "Area"),
                                BatchType = FloorDBAccess.GetString(row, "BatchType")
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        /// <summary>
        /// Set QAI Batch Sample data for post to AX
        /// #MH 24/10/2016 1.n FDD@NGC-REM-001-PT_V1.0
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static BatchDTO SetQAIBatchDetails(BatchDTO input)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", input.SerialNumber));
            lstParameters.Add(new FloorSqlParameter("@QaiId", input.QaiId));
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_GET_QAIBatchDetails", lstParameters))
            {
                if (dtBatch.Rows.Count == 0)
                    return input;
                var row = dtBatch.Rows[0];
                input.RAFWTSample = FloorDBAccess.GetValue<int>(row, "WTSampliingSize");
                input.RAFVTSample = FloorDBAccess.GetValue<int>(row, "VTSamplingSize");
                input.TenPcsRAFSample = FloorDBAccess.GetValue<int>(row, "TenPcsSamplingSize"); // #Azrul 13/07/2018: Merged from Live AX6
                input.HotBox = FloorDBAccess.GetValue<int>(row, "HBSamplingSize");
            }
            return input;
        }

        /// <summary>
        /// To Get Complete Batch details
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static Boolean ValidateBatch(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            bool result = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_Batch_SerialNo", lstParameters));
            return result;
        }

        /// <summary>
        /// Generic Base method to get items based on filter value
        /// </summary>
        /// <param name="filterName"></param>
        /// <param name="filtervalue"></param>
        /// <param name="spName"></param>
        /// <param name="dtoValueName"></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetItemsByFilter(string filterName, string filtervalue, string spName, string dtoIDName, string dtoDisplayName)
        {
            List<DropdownDTO> list = null;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            if (!String.IsNullOrEmpty(filtervalue))
            {
                PrmList.Add(new FloorSqlParameter(filterName, filtervalue.ToString()));
            }
            using (DataTable dt = FloorDBAccess.ExecuteDataTable(spName, PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, dtoIDName),
                                DisplayField = FloorDBAccess.GetString(row, dtoDisplayName)
                            }).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// To validate Tenpecs weight Range
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="minweight"></param>
        /// <param name="maxweight"></param>
        /// <returns></returns>
        public static bool InRange(double weight, double minweight, double maxweight)
        {
            return weight >= minweight && weight <= maxweight;
        }

        /// <summary>
        /// To Get Last Execution of Batch Job
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static DateTime GetBatchJobLastExecution()
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            DateTime result = Convert.ToDateTime(FloorDBAccess.ExecuteScalar("USP_GET_BatchJobExecution", lstParameters));
            return result;
        }

        /// <summary>
        /// Check whether the Batch Job is again executed after its last execution
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static Boolean ValidateBatchJob(DateTime lastExecutionTime, DateTime currentDateTime)
        {
            bool result = false;
            if (currentDateTime < lastExecutionTime)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Check whether the AX posting for the serial number for that Service name has already been done.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static Boolean ValidateAXPosting(decimal serialNo, string serviceName, string area = null)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@serviceName", serviceName));
            if (!string.IsNullOrEmpty(area))
                lstParameters.Add(new FloorSqlParameter("@area", area));
            bool result = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_AXPostingStatus", lstParameters));
            return result;
        }

        /// <summary>
        /// If PTPF Glove Batch, Create Rework is after SPBC except for Water Tight Batch.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static bool ReworkCreationForPTPF(BatchDTO _batchDto)
        {
            List<FloorSqlParameter> lstFsp = new List<FloorSqlParameter>();
            lstFsp.Add(new FloorSqlParameter("@SerialNumber", (Convert.ToDecimal(_batchDto.SerialNumber))));
            bool isPTPFGlove = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_CheckIsPTPF", lstFsp));

            bool isCreateRework = true;
            if (isPTPFGlove)
            {
                switch (_batchDto.BatchType.Trim())
                {
                    case "PWT":
                    case "QWT":
                    case "OWT":
                    case "PSW":
                        isCreateRework = true;
                        break;
                    default:
                        isCreateRework = false;
                        break;
                }
            }
            return isCreateRework;
        }

        /// <summary>
        /// Check whether the AX posting for the serial number for that Service name has already been done.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static Decimal GetLatestBatchWeight(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            decimal result = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("USP_GET_LatestBatchWeight", lstParameters));
            return result;
        }

        /// <summary>
        /// Check whether the AX posting for the serial number for that Service name has already been done.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static Decimal GetLatestTenPcsWeight(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            decimal result = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("USP_GET_LatestTenPcsWeight", lstParameters));
            return result;
        }

        // GIS modification by Azman - 26-09-2018 START
        public static GISBatchInfo GISGetBatchInfo(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            GISBatchInfo objBatch = null;
            using (DataTable dtBatch = Framework.Database.FloorDBAccess.ExecuteDataTable("usp_GIS_GetBatchInfo", lstParameters))
            {
                objBatch = (from DataRow row in dtBatch.Rows
                            select new GISBatchInfo
                            {
                                SerialNumber = Convert.ToString(serialNo),
                                TenPcsWeight = FloorDBAccess.GetValue<Decimal>(row, "TenPcsWeight"),
                                BatchWeight = FloorDBAccess.GetValue<Decimal>(row, "BatchWeight"),
                                TotalPcs = FloorDBAccess.GetValue<Int32>(row, "TotalPcs"),
                            }).SingleOrDefault();
            }
            return objBatch;
        }

        // GIS modification by Azman - 26-09-2018 END

        /// <summary>
        /// Get the latest area from AX Posting
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static string GetLatestArea(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            string result = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_LatestArea", lstParameters));
            return result;
        }

        /// <summary>
        /// Get the QC Type
        /// </summary>
        /// <returns></returns>
        public static string GetQCType(string qcType)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@qcType", qcType));
            string result = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_QCType", lstParameters));
            return result;
        }

        /// <summary>
        /// Check whether the AX posting for the serial number for that Service name has already been done.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static Boolean ValidateAXPostingForArea(decimal serialNo, string area)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            lstParameters.Add(new FloorSqlParameter("@area", area));
            bool result = Convert.ToBoolean(FloorDBAccess.ExecuteScalar("USP_GET_AXPostingStatus_Area", lstParameters));
            return result;
        }
        public static bool ValidatePositiveValue(decimal weight)
        {
            return weight > 0;
        }
        #endregion


        /// <summary>
        /// To get Production Line by Glove Type and Location
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetLineByGloveTypeTumbling(string location, string gloveType, string size)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();

            PrmList.Add(new FloorSqlParameter("@location", location.ToString()));
            PrmList.Add(new FloorSqlParameter("@gloveType", gloveType.ToString()));
            PrmList.Add(new FloorSqlParameter("@size", size.ToString()));
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_SEL_Line_ALL_LocationAndGloveType", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "LineNumber"),
                                DisplayField = FloorDBAccess.GetString(row, "LineNumber"),
                                // SelectedValue = FloorDBAccess.GetString(row, "CurrentShift")
                            }).ToList();

                }
            }
            return list;

        }

        /// <summary>
        /// #Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product)
        /// List of Batch Order Number By Glove Type and Plant No
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetBatchOrderByGloveTypeAndLocation(string location, string gloveType, string size)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@location", location.ToString()));
            PrmList.Add(new FloorSqlParameter("@gloveType", gloveType.ToString()));
            PrmList.Add(new FloorSqlParameter("@size", size.ToString()));
            List<DropdownDTO> list = null;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("USP_DOT_SEL_ChangeGloveTypeBatchOrder", PrmList))
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    list = (from DataRow row in dt.Rows
                            select new DropdownDTO
                            {
                                IDField = FloorDBAccess.GetString(row, "BatchOrder"),
                                DisplayField = FloorDBAccess.GetString(row, "BatchOrder"),
                            }).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// Check whether SOBC can post to stagaing.
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static int ValidateSOBCPosting(decimal serialNo)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            int result = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_DOT_ValidateSOBCPosting", lstParameters));
            return result;
        }

        public static string GetDryerNumber(decimal serialNo)
        {
            DataTable dt = new DataTable();
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNo", serialNo));
            string result = Convert.ToString(FloorDBAccess.ExecuteScalar("USP_GET_DryerNumber", lstParameters));

            return result;
        }

        /// GetNoOfLabel FinalPacking
        /// </summary>
        /// <param name="internallotnumber"></param>
        /// <returns></returns>
        public static int GetNoOfLabelPrintByInternalLotNumber(string internallotno)
        {
            int noofprintlabel;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@InternalLotNo", internallotno));
            noofprintlabel = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GetNoOfLabelPrintByInternalLotNo", PrmList));
            return noofprintlabel;
        }

        public static int GetNoOfLabelPrintByItemNumber(string itemnumber)
        {
            int noofprintlabel;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@ItemNumber", itemnumber));
            noofprintlabel = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_GetNoOfLabelPrintByItemNo", PrmList));
            return noofprintlabel;
        }
    }

    public static class extentions
    {
        public static List<Variance> DetailedCompare<T>(this T val1, T val2)
        {
            List<Variance> variances = new List<Variance>();
            PropertyInfo[] fi = val1.GetType().GetProperties();
            foreach (PropertyInfo f in fi)
            {
                Variance v = new Variance();
                v.Prop = f.Name;
                v.valA = f.GetValue(val1, null);
                v.valB = f.GetValue(val2, null);
                if (v.Prop != "ActionType")
                {
                    if (!Equals(v.valA, v.valB))
                    {
                        variances.Add(v);
                    }
                }
            }
            return variances;
        }

        public static List<ColumnChange> GetPropChanges(this List<Variance> ltv)
        {
            List<ColumnChange> lstprop = new List<ColumnChange>();
            if (ltv != null)
            {
                foreach (Variance vri in ltv)
                {
                    ColumnChange cc = new ColumnChange() { ColumnName = vri.Prop, OldValue = Convert.ToString(vri.valA), NewValue = Convert.ToString(vri.valB) };
                    lstprop.Add(cc);
                }
            }

            return lstprop;
        }
    }


    public class Variance
    {
        public string Prop { get; set; }
        public object valA { get; set; }
        public object valB { get; set; }
    }
}
