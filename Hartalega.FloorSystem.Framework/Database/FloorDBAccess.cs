using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Hartalega.FloorSystem.Framework.Common;
namespace Hartalega.FloorSystem.Framework.Database
{
    /// <summary>
    /// 
    /// </summary>
    public enum ConnName
    {
        FS, AX, EX
    }
    /// <summary>
    /// 
    /// </summary>
    public class FloorSqlParameter : IDisposable
    {
        //create 3 attributes as per the arguments in the constructor
        private string _paremetrName;
        private object _parameterValue;
        private ParameterDirection _direction;
        private SqlDbType _sqlType;
        /// <summary>
        /// Creates SQL Parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="direction"></param>
        public FloorSqlParameter(string parameterName, object value, System.Data.ParameterDirection direction = ParameterDirection.Input)
        {
            ParameterName = parameterName;
            ParamaterValue = value;
            Direction = direction;
        }

        /// <summary>
        /// Creates SQL Parameter of the sql type
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="sqlType"></param>

        public FloorSqlParameter(string parameterName, SqlDbType sqlType)
        {
            ParameterName = parameterName;
            SqlType = sqlType;
            ParamaterValue = null;
            Direction = ParameterDirection.Output;
        }

        //provide properties as well. 
        /// <summary>
        /// 
        /// </summary>
        public string ParameterName
        {
            get { return _paremetrName; }
            set { _paremetrName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public object ParamaterValue
        {
            get { return _parameterValue; }
            set
            {
                if (value == null)
                {
                    _parameterValue = DBNull.Value;
                }
                else
                {
                    _parameterValue = value;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ParameterDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlDbType SqlType
        {
            get { return _sqlType; }
            set { _sqlType = value; }
        }

        public void Dispose()
        {
            //this can be deleted if it doesn't affect the functionality
            // GC.SuppressFinalize(this);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class FloorDBAccess
    {
        private static SqlConnection _conn;
        private static string _floorDBconnstring;
        //private static string _axDBconnstring;
        private static string _exceptionDBconnstring;

        private const string _connName = "FS";
        private const string _subsystem = Constants.DAL;

        static FloorDBAccess()
        {
            _floorDBconnstring = EncryptDecrypt.GetDecryptedString(ConfigurationManager.ConnectionStrings["FSDB"].ToString(), "hidden");
            //if (ConfigurationManager.ConnectionStrings["AXDB"] != null)
            //    _axDBconnstring = EncryptDecrypt.GetDecryptedString(ConfigurationManager.ConnectionStrings["AXDB"].ToString(), "hidden");
        }

        private static void Init(ConnName connName)
        {
            try
            {
                if (_conn == null || _conn.State != ConnectionState.Open)
                {
                    switch (connName)
                    {
                        case ConnName.FS:
                            _conn = new SqlConnection(_floorDBconnstring);
                            break;
                        //case ConnName.AX:
                        //    _conn = new SqlConnection(_axDBconnstring);
                        //    break;
                        case ConnName.EX:
                            _conn = new SqlConnection(_exceptionDBconnstring);
                            break;
                    }
                    _conn.Open();
                }

            }

            catch (Exception ex)
            {
                throw;
            }

        }
        public static void TearDown()
        {
            try
            {
                if (_conn != null)
                {
                    if (_conn.State == ConnectionState.Open)
                    {
                        _conn.Close();
                    }
                    _conn = null;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// To build SQL Command
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="lstParameters"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        private static SqlCommand BuildSqlCommand(string spName, List<FloorSqlParameter> lstParameters)
        {

            SqlCommand cmd = new SqlCommand();
            try
            {
                if (lstParameters == null)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                cmd.Connection = _conn;
                cmd.CommandText = spName;

                if (lstParameters != null)
                {
                    foreach (FloorSqlParameter param in lstParameters)
                    {
                        if (param.Direction == ParameterDirection.Input)
                        {
                            cmd.Parameters.Add(new SqlParameter(param.ParameterName, param.ParamaterValue));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter(param.ParameterName, param.SqlType));
                        }
                        cmd.Parameters[cmd.Parameters.Count - 1].Direction = param.Direction;
                    }
                }
                return cmd;
            }
            catch (InvalidOperationException ioex)
            {
                throw new FloorSystemException(String.Format(Messages.INDIVIDUAL_OPERATION_EXCEPTION, spName), _subsystem, ioex);
            }
            catch (ArgumentException argex)
            {
                throw new FloorSystemException(String.Format(Messages.ARGUMENT_EXCEPTION_OCCURED, spName), _subsystem, argex);
            }
            catch (InvalidCastException icex)
            {
                throw new FloorSystemException(String.Format(Messages.INVALID_CAST_EXPECTION, spName), _subsystem, icex);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(string.Format(Messages.UNKNOWN_EXCEPTION_THROWN, spName), _subsystem, ex);
            }
        }

        /// <summary>
        /// To execute SP which return dataset
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="lstParameters"></param>
        /// <param name="timeOut"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static System.Data.DataSet ExecuteDataSet(string spName, List<FloorSqlParameter> lstParameters, ConnName connectionName = ConnName.FS)
        {
            Init(connectionName);
            DataSet ds = new DataSet();
            SqlDataAdapter sqlAd = new SqlDataAdapter(BuildSqlCommand(spName, lstParameters));
            try
            {
                sqlAd.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, spName), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
        }
        /// <summary>
        /// to execute sp which returns datatable
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="lstParameters"></param>
        /// <param name="timeOut"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static System.Data.DataTable ExecuteDataTable(string spName, List<FloorSqlParameter> lstParameters = null, ConnName connectionName = ConnName.FS)
        {
            Init(connectionName);
            DataSet ds = new DataSet();
            SqlDataAdapter sqlAd = new SqlDataAdapter(BuildSqlCommand(spName, lstParameters));
            try
            {
                sqlAd.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, spName), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
        }
        /// <summary>
        /// to execute sp which return single value
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="lstParameters"></param>
        /// <param name="timeOut"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string spName, List<FloorSqlParameter> lstParameters = null, ConnName connectionName = ConnName.FS)
        {
            Init(connectionName);
            try
            {
                return BuildSqlCommand(spName, lstParameters).ExecuteScalar();
            }
            catch (SqlException sqlEx)
            {
                throw new FloorSystemException(String.Format(sqlEx.Message, spName), _subsystem, sqlEx);
                //Messages.ROW_LOCKED
            }

            catch (FloorSystemException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(String.Format(Messages.UN_KNOWN_EXCEPTION, spName, _subsystem, ex));
            }
            finally
            {
                TearDown();
            }
        }
        /// <summary>
        /// to have connection established as reader
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="lstParameters"></param>
        /// <param name="timeOut"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string spName, List<FloorSqlParameter> lstParameters, ConnName connectionName = ConnName.FS)
        {
            Init(connectionName);
            try
            {
                return BuildSqlCommand(spName, lstParameters).ExecuteReader(CommandBehavior.CloseConnection);
                //Important- The client code must call SqlDataReader::Close();
            }

            catch (FloorSystemException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(String.Format(Messages.FLOOR_DB_ACCESS_ERROR, spName), _subsystem, ex);
            }
        }
        /// <summary>
        /// retruns the result or the selected values
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="lstParameters"></param>
        /// <param name="timeOut"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string spName, List<FloorSqlParameter> lstParameters, ConnName connectionName = ConnName.FS)
        {
            Init(connectionName);
            try
            {
                SqlCommand cmd = BuildSqlCommand(spName, lstParameters);
                int retVal = cmd.ExecuteNonQuery();
                RetrieveOutPutData(cmd, lstParameters);
                return retVal;

            }
            catch (SqlException sqlEx)
            {
                throw new FloorSystemException(String.Format(sqlEx.Message, spName), _subsystem, sqlEx);
            }
            catch (FloorSystemException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(String.Format(Messages.FLOOR_DB_ERROR, spName), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
        }

        /// <summary>
        /// Copy output data
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="lstParameters"></param>
        /// <returns></returns>

        private static void RetrieveOutPutData(SqlCommand cmd, List<FloorSqlParameter> lstParameters)
        {
            if (lstParameters == null) return;
            foreach (FloorSqlParameter sqlParam in lstParameters)
            {
                if (sqlParam.Direction == ParameterDirection.Output)
                {
                    sqlParam.ParamaterValue = cmd.Parameters[sqlParam.ParameterName].Value;
                }
            }

        }

        /// <summary>
        /// Method will check for SQL NUll values
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="colName"></param>
        /// <returns>T</returns>

        public static T GetValue<T>(DataRow dr, string colName) where T : struct
        {
            if (dr[colName] == DBNull.Value)
            {
                return default(T);
            }
            else
            {
                return (T)dr[colName];
            }
        }

        /// <summary>
        /// Method will check for SQL NUll values
        /// </summary>
        /// <param name="sqlRdr"></param>
        /// <param name="colName"></param>
        /// <returns>T</returns>

        public static T GetValue<T>(SqlDataReader sqlRdr, string colName) where T : struct
        {
            if (sqlRdr[colName] == DBNull.Value)
            {
                return default(T);
            }
            else
            {
                return (T)sqlRdr[colName];
            }
        }

        /// <summary>
        /// Method will check for SQL NUll values
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="colName"></param>
        /// <returns>string</returns>

        public static string GetString(DataRow dr, string colName)
        {
            return Convert.ToString(dr[colName]);
        }

        /// <summary>
        /// Method will check for SQL NUll values
        /// </summary>
        /// <param name="sqlRdr"></param>
        /// <param name="colName"></param>
        /// <returns>string</returns>
        public static string GetString(SqlDataReader sqlRdr, string colName)
        {
            return Convert.ToString(sqlRdr[colName]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="lstParameters"></param>
        /// <param name="timeOut"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static int ExecuteNonQueryUnitTest(string sqlQuery, ConnName connectionName = ConnName.FS)
        {
            Init(connectionName);
            try
            {
                SqlCommand cmd = BuildSqlCommand(sqlQuery, null);
                int retVal = cmd.ExecuteNonQuery();
                return retVal;
            }
            catch (SqlException sqlEx)
            {
                throw new FloorSystemException(String.Format(Messages.ROW_LOCKED, sqlQuery), _subsystem, sqlEx);
            }

            catch (FloorSystemException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(String.Format(Messages.FLOOR_DB_EXECUTION_ERROR, sqlQuery), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
        }

    }
}