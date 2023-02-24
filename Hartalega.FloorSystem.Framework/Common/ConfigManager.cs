// -----------------------------------------------------------------------
// <copyright file="ConfigManager.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
#region namespace
using System;
using System.Configuration;
using System.Text.RegularExpressions;
#endregion
namespace Hartalega.FloorSystem.Framework.Common
{
    /// <summary>
    /// Static class.
    /// It helps to read key value from app/web configuration and xml files.
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        /// Gets configuration XML folder path
        /// </summary>
        #region Public Methods
        public static string GetConfigurationXmlPath
        {
            get
            {
                string appPath = System.AppDomain.CurrentDomain.BaseDirectory;
                return string.Format(Constants.CONFIG, appPath.Substring(0, appPath.IndexOf(Constants.HARTALEGA)));
            }
        }
        
        /// <summary>
        /// Validate whether the input string is empty or not.
        /// </summary>
        /// <param name="value">Input value</param>
        /// <returns>Boolean value indicates whether the string is null or empty</returns>
        public static bool IsEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            Regex nonBlank = new Regex(RegExpPattern.Blank);

            return nonBlank.IsMatch(value) == false;
        }
       
        /// <summary>
        /// Get App Settings value by Key name.
        /// </summary>
        /// <param name="key">Key Name</param>
        /// <returns>Value as string</returns>
        public static string GetAppSettingValueByKey(string key)
        {
            string keyValue = ConfigurationManager.AppSettings[key];

            if (IsEmpty(keyValue))
            {
                throw new NotSupportedException(string.Format(Messages.APP_SETTING_KEY_NOT_FOUND, key));
            }

            return keyValue;
        }
        
        /// <summary>
        /// Get Database Connection Settings value by Key name.
        /// </summary>
        /// <param name="key">Key Name</param>
        /// <returns>Database connection string as string</returns>
        public static string GetDatabaseConnectionStringByKey(string key)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[key] == null ? string.Empty : ConfigurationManager.ConnectionStrings[key].ConnectionString;

            if (IsEmpty(connectionString))
            {
                throw new System.Data.NoNullAllowedException(string.Format(Messages.CONNECTION_STRING_KEY_NOT_FOUND, key));
            }

            return connectionString;
        }
        #endregion
    }
}
