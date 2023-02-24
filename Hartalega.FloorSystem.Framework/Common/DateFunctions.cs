using System;

namespace Hartalega.FloorSystem.Framework.Common
{
    /// <summary>
    /// Cannot have instance and cannot be inherited.
    /// This class provides the feature to calculate duration between two dates
    /// </summary>
    public static class DateFunctions
    {
        #region Public Methods
        /// <summary>
        /// This method provides the feature to calculate duration between two dates
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static string GetDuration(DateTime date1, DateTime date2)
        {
            int[] monthDay = new int[Constants.TWELVE] { Constants.THIRTYONE, Constants.MINUSONE, Constants.THIRTYONE, Constants.THIRTY, Constants.THIRTYONE, 
                Constants.THIRTY, Constants.THIRTYONE, Constants.THIRTYONE, Constants.THIRTY, Constants.THIRTYONE, Constants.THIRTY, Constants.THIRTYONE };
            DateTime fromDate = date1;
            DateTime toDate = date2;
            int year =Constants.ZERO;
            int month = Constants.ZERO;
            int day = Constants.ZERO;
            int hour = Constants.ZERO;
            int minute = Constants.ZERO;
            string yearFormat = string.Empty;
            string monthFormat = string.Empty;
            string dayFormat = string.Empty;
            string hourFormat = string.Empty;
            string minuteFormat = string.Empty;
            string secondFormat = "00";
            string duration = string.Empty;
            int increment = Constants.ZERO;
            if (fromDate.Day > toDate.Day)
            {
                increment = monthDay[fromDate.Month - Constants.ONE];
            }
            if (increment == Constants.MINUSONE)
            {
                if (DateTime.IsLeapYear(fromDate.Year))
                {
                    increment = Constants.TWENTYNINE;
                }
                else
                {
                    increment = Constants.TWENTYEIGHT;
                }
            }
            if (increment != Constants.ZERO)
            {
                day = (toDate.Day + increment) - fromDate.Day;
                increment = Constants.ONE;
            }
            else
            {
                day = toDate.Day - fromDate.Day;
            }
            if ((fromDate.Month + increment) > toDate.Month)
            {
                month = (toDate.Month + Constants.TWELVE) - (fromDate.Month + increment);
                increment = Constants.ONE;
            }
            else
            {
                month = (toDate.Month) - (fromDate.Month + increment);
                increment = Constants.ZERO;
            }
            year = toDate.Year - (fromDate.Year + increment);
            TimeSpan span = date2.Subtract(date1);
            hour = span.Hours;
            minute = span.Minutes;
            if (year < Constants.TEN)
            {
                yearFormat = Constants.ZERO.ToString() + year;
            }
            else
            {
                yearFormat = year.ToString();
            }
            if (month < Constants.TEN)
            {
                monthFormat = Constants.ZERO.ToString() + month;
            }
            else
            {
                monthFormat = month.ToString();
            }
            if (day < Constants.TEN)
            {
                dayFormat = Constants.ZERO.ToString() + day;
            }
            else
            {
                dayFormat = day.ToString();
            }
            if (hour < Constants.TEN)
            {
                hourFormat = Constants.ZERO.ToString() + hour;
            }
            else
            {
                hourFormat = hour.ToString();
            }
            if (minute < Constants.TEN)
            {
                minuteFormat = Constants.ZERO.ToString() + minute;
            }
            else
            {
                minuteFormat = minute.ToString();
            }
            duration = yearFormat + Constants.COLON + monthFormat + Constants.COLON + dayFormat + Constants.COLON + hourFormat + Constants.COLON + minuteFormat + Constants.COLON + secondFormat;
            return duration;
        }
        #endregion
    }
}
