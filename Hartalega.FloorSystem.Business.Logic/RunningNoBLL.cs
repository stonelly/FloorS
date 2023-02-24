using Hartalega.FloorSystem.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic
{
    public class RunningNoBLL
    {
        protected static object _Lock = new object();

        public enum RunningNoType : byte
        {
            GloveTypeBarcode = 1
        }

        public enum ResetPeriodType : byte
        {
            Yearly = 1,
            Monthly = 2,
            Daily = 3
        }

        static DataTable GetRunningNo(RunningNoType runningNoType)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>
            {
                new FloorSqlParameter("@runningNoType", (byte)runningNoType)
            };
            return FloorDBAccess.ExecuteDataTable("USP_SEL_RunningNo", lstParameters);
        }

        static int UpdateRunningNo(int runningNoId, int lastRunningNo, string currentPeriod, string loggedInUser)
        {
            int _rowsaffected = 0;
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>
            {
                new FloorSqlParameter("@RunningNoId", runningNoId),
                new FloorSqlParameter("@LastRunningNo", lastRunningNo),
                new FloorSqlParameter("@CurrentPeriod", currentPeriod),
                new FloorSqlParameter("@ModifiedBy", loggedInUser)
            };
            _rowsaffected = FloorDBAccess.ExecuteNonQuery("USP_SAV_RunningNo", PrmList);
            return _rowsaffected;
        }

        public static string NextRunningNo(RunningNoType runningNoType, string loggedInUser = null, bool updateRunningNo = true)
        {
            lock (_Lock)
            {
                var runningNoData = GetRunningNo(runningNoType);

                var runningNoId = runningNoData.Rows[0].Field<int>("RunningNoId");
                var numberOfDigits = runningNoData.Rows[0].Field<byte>("NumberOfDigits");
                var lastRunningNo = runningNoData.Rows[0].Field<int>("LastRunningNo");
                var masking = runningNoData.Rows[0].Field<string>("Masking");
                var resetPeriodType = (ResetPeriodType?)runningNoData.Rows[0].Field<byte?>("ResetPeriodType");
                var currentPeriod = runningNoData.Rows[0].Field<string>("CurrentPeriod");

                var nextRunningNo = lastRunningNo + 1;

                // try reset period
                string period = null;
                if (resetPeriodType != null)
                {
                    switch (resetPeriodType.Value)
                    {
                        case ResetPeriodType.Daily:
                            period = DateTime.Now.ToString("yyyyMMdd");
                            break;
                        case ResetPeriodType.Monthly:
                            period = DateTime.Now.ToString("yyyyMM");
                            break;
                        case ResetPeriodType.Yearly:
                            period = DateTime.Now.ToString("yyyy");
                            break;
                    }
                    if (period != currentPeriod)
                        nextRunningNo = 1;
                }

                // update to table
                if (updateRunningNo) UpdateRunningNo(runningNoId, nextRunningNo, period, loggedInUser);

                return FormatRunningNo(masking, nextRunningNo, numberOfDigits);
            }
        }

        static string FormatRunningNo(string masking, int runningNo, int numberOfDigits)
        {
            string lResult;

            // Bind running no
            lResult = masking.Replace("{#}", runningNo.ToString().PadLeft(numberOfDigits, '0'));

            // Bind {yyyy}
            lResult = lResult.Replace("{yyyy}", DateTime.Now.ToString("yyyy"));

            // Bind {yy}
            lResult = lResult.Replace("{yy}", DateTime.Now.ToString("yy"));

            // Bind {mm}
            lResult = lResult.Replace("{MM}", DateTime.Now.ToString("MM"));

            // Bind {dd}
            lResult = lResult.Replace("{dd}", DateTime.Now.ToString("dd"));

            return lResult;
        }
    }
}
