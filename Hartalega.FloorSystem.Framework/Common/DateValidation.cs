using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace Hartalega.FloorSystem.Framework.Common
{
    /// <summary>
    /// Cannot have instance and cannot be inherited.
    /// This class provides the feature to validate from date and To date
    /// </summary>
    public static class DateValidation
    {
        #region Member Varibales
        static List<string> _validationMsgs = null;
        #endregion

        /// <summary>
        /// Date Validation
        /// </summary>
        /// <param name="Fromdate"></param>
        /// <param name="ToDate"></param>
        /// <returns>string</returns>
        public static List<string> DateValidator(string Fromdate, string ToDate)
        {
            _validationMsgs = new List<string>();
            // Validation  for Date From
            if (!string.IsNullOrEmpty(Fromdate))
            {
                DateTime date;

                if (!DateTime.TryParseExact(Fromdate, ConfigurationManager.AppSettings["SetupConfigDateFormat"], null, DateTimeStyles.None, out date))
                {
                    _validationMsgs.Add(Messages.INVALID_FROM_DATE);
                }
                else if (date > DateTime.Now.Date)
                {
                    _validationMsgs.Add(Messages.FROM_DATE_IN_FUTURE);
                }
            }


            // Validation for Date To
            if (!string.IsNullOrEmpty(ToDate))
            {
                DateTime date;
                if (!DateTime.TryParseExact(ToDate, ConfigurationManager.AppSettings["SetupConfigDateFormat"], null, DateTimeStyles.None, out date))
                {
                    _validationMsgs.Add(Messages.INVALID_TO_DATE);
                }
                else if (date > DateTime.Now.Date)
                {
                    _validationMsgs.Add(Messages.TO_DATE_IN_FUTURE);
                }
            }



            // Validation : From date should not be greater than To date
            if (!string.IsNullOrEmpty(Fromdate) && !string.IsNullOrEmpty(ToDate))
            {
                if (_validationMsgs.Count == Constants.ZERO && DateTime.ParseExact(Fromdate, ConfigurationManager.AppSettings["SetupConfigDateFormat"], null) > DateTime.ParseExact(ToDate, ConfigurationManager.AppSettings["SetupConfigDateFormat"], null))
                {
                    _validationMsgs.Add(Messages.TO_DATE_LESSTHAN_FROM_DATE);
                }
            }

            if (!string.IsNullOrEmpty(Fromdate) && string.IsNullOrEmpty(ToDate))
            {
                if (_validationMsgs.Count == Constants.ZERO)
                {
                    _validationMsgs.Add(Messages.REQUIRED_TO_DATE);
                }
            }

            if (!string.IsNullOrEmpty(ToDate) && string.IsNullOrEmpty(Fromdate))
            {
                if (_validationMsgs.Count == Constants.ZERO)
                {
                    _validationMsgs.Add(Messages.REQUIRED_FROM_DATE);
                }
            }
            return _validationMsgs;

        }
    }
}
