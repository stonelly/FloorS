// -----------------------------------------------------------------------
// <copyright file="CacheManager.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Framework.Cache
{
    using Hartalega.FloorSystem.Framework.Database;
    using Hartalega.FloorSystem.Framework.DbExceptionLog;
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Static class.
    /// It helps to cache master data. It stores the data into dictionary object.
    /// </summary>
    public static class CacheManager
    {
        /// <summary>
        /// Object to catch master data
        /// </summary>
        private static Dictionary<string, object> cache = new Dictionary<string, object>();

        #region CatchMessages

        /// <summary>
        /// Fill Cache on Application Load
        /// </summary>
        public static void FillCache()
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            try
            {
                using (DataTable dtMessages = Framework.Database.FloorDBAccess.ExecuteDataTable("USP_SEL_Messages", lstParameters))
                {
                    foreach (DataRow item in dtMessages.Rows)
                    {
                        if (!CacheManager.IsExists(Convert.ToString(item["MessageKey"])))
                        {
                            CacheManager.Add(Convert.ToString(item["MessageKey"]), item["MessageText"]);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Add master data to cache collection
        /// </summary>
        /// <param name="key">Cache key name</param>
        /// <param name="value">Cache object value</param>
        /// <returns>Boolean value indicates whether the object has been added to cache or not</returns>
        #region
        public static bool Add(string key, object value)
        {
            bool result = false;
            lock (cache)
            {
                try
                {
                    cache.Add(key, value);
                    result = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return result;
        }
        #endregion

        /// <summary>
        /// Remove particular cache object from cache collection.
        /// </summary>
        /// <param name="key">Cache key name</param>
        /// <returns>Boolean value indicates whether the cache has been removed or not</returns>
        #region
        public static bool Remove(string key)
        {
            bool result = false;
            lock (cache)
            {
                try
                {
                    if (cache.ContainsKey(key))
                    {
                        cache.Remove(key);
                        result = true;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return result;
        }
        #endregion

        /// <summary>
        /// Get particular cache object from cache collection by key.
        /// </summary>
        /// <param name="key">Cache key name</param>
        /// <returns>Object as result</returns>
        #region
        public static object Get(string key)
        {
            if (cache.Keys.Count > 0)
            {
                object result = null;

                try
                {
                    cache.TryGetValue(key.Trim(), out result);
                }
                catch (Exception)
                {
                    throw;
                }
                return result;
            }
            else
                return null;
        }
        #endregion

        /// <summary>
        /// Check whether the cache is exist or not by key.
        /// </summary>
        /// <param name="key">Cache key name</param>
        /// <returns>Boolean value indicates whether the cache object is exist or not</returns>
        #region
        public static bool IsExists(string key)
        {
            return cache.ContainsKey(key.Trim());
        }
        #endregion

        /// <summary>
        /// Clears all available keys and values from the cache collection.
        /// </summary>
        #region
        public static void Flush()
        {
            try
            {
                cache.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
