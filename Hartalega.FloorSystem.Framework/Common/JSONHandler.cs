using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Hartalega.FloorSystem.Framework.Common
{
    /// <summary>
    /// Static class.
    /// It helps to serialize/de-serialize string as JSON class.
    /// </summary>
    public static class JsonHandler
    {
        #region Public Methods
        /// <summary>
        /// Get JSON string from a class
        /// </summary>
        public static string JsonSerialize<T>(T obj)
        {
            DataContractJsonSerializer serialObj = new DataContractJsonSerializer(typeof(T));
            MemoryStream memStr = new MemoryStream();
            serialObj.WriteObject(memStr, obj);
            string retVal = Encoding.UTF8.GetString(memStr.ToArray());
            memStr.Close();
            return retVal;
        }

        /// <summary>
        /// Get class form JSON string 
        /// </summary>
        public static T JsonDeseailize<T>(string jsonstr)
        {
            DataContractJsonSerializer desrialObj = new DataContractJsonSerializer(typeof(T));
            MemoryStream memStr = new MemoryStream(Encoding.UTF8.GetBytes(jsonstr));
            T obj = (T)desrialObj.ReadObject(memStr);
            return obj;
        }
        #endregion
    }
}
