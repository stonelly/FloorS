using Hartalega.FloorSystem.Framework.Database;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Hartalega.FloorSystem.Framework.DbExceptionLog
{
    [Serializable]
    public class FloorSystemException : Exception, ISerializable
    {
        #region Private Variables
        private FloorSystemException() { _dateTime = Convert.ToDateTime(FloorDBAccess.ExecuteScalar("USP_GET_CurrentDateAndTime")); }

        private string _floorExceptionMessage;

        private Exception _innerException;

        private bool _canLogExceptionToDB;
        private DateTime _dateTime;
        private string _workStationId;
        #endregion

        #region Properties
        public bool CanLogExceptionToDB
        {
            get { return _canLogExceptionToDB; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string WorkStationId
        {
            get { return _workStationId; }
            set { _workStationId = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public DateTime ExceptionDt
        {
            get { return _dateTime; }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public FloorSystemException(string message)
            : this()
        {
            _floorExceptionMessage = message;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public FloorSystemException(string message, Exception innerException)
            : this()
        {
            _floorExceptionMessage = message;
            _innerException = innerException;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public FloorSystemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _workStationId = info.GetString("WorkStationId");
            _subsystem = info.GetString("SubSystem");
            _screenName = info.GetString("ScreenName");
            _uiClassName = info.GetString("UIClassName");
            _uiControlName = info.GetString("UIControlName");
            _dateTime = info.GetDateTime("DateTime");
            _innerExceptionMessages = info.GetString("InnerExceptionMessages");
            _canLogExceptionToDB = info.GetBoolean("CanLogException");
        }

        private string _subsystem;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="subSystem"></param>
        /// <param name="innerException"></param>
        public FloorSystemException(string message, string subSystem, Exception innerException, bool canLogException = true)
            : this()
        {
            _floorExceptionMessage = message;
            _innerException = innerException;
            _subSystem = subSystem;
            _canLogExceptionToDB = canLogException;
        }


        /// <summary>
        /// 
        /// </summary>
        public override string Message
        {
            get
            {
                return _floorExceptionMessage;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Exception GetBaseException()
        {
            return _innerException;
        }
        /// <summary>
        /// 
        /// </summary>
        public override string StackTrace
        {
            get
            {
                if (_innerException != null)
                    return _innerException.StackTrace;
                return base.StackTrace;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            base.GetObjectData(info, context);

            //add the serialization support for the newly added data members
            info.AddValue("WorkStationId", _workStationId);
            info.AddValue("SubSystem", _subsystem);
            info.AddValue("ScreenName", _screenName);
            info.AddValue("UIClassName", _uiClassName);
            info.AddValue("UIControlName", _uiControlName);
            info.AddValue("DateTime", _dateTime);
            info.AddValue("InnerExceptionMessages", _innerExceptionMessages);
            info.AddValue("CanLogException", _canLogExceptionToDB);
        }

        private string _innerExceptionMessages = null;
        /// <summary>
        /// 
        /// </summary>
        public string InnerExceptionMessages
        {
            get
            {
                if (_innerExceptionMessages == null)
                {
                    Exception innerExceptions = _innerException;

                    while (innerExceptions != null)
                    {
                        _innerExceptionMessages = innerExceptions.Message;
                        innerExceptions = innerExceptions.InnerException;
                    }

                }
                return _innerExceptionMessages;
            }
        }
        private string _subSystem;
        public string subSystem
        {
            get { return _subSystem; }
            set { _subSystem = value; }
        }

        private string _screenName;
        public string screenName
        {
            get { return _screenName; }
            set { _screenName = value; }
        }

        private string _uiClassName;
        public string uiClassName
        {
            get { return _uiClassName; }
            set { _uiClassName = value; }
        }

        private string _uiControlName;
        public string uiControlName
        {
            get { return _uiControlName; }
            set { _uiControlName = value; }
        }

        private string _baseexception;
        public string baseexception
        {
            get { return _baseexception; }
            set { _baseexception = value; }
        }


        /// <summary>
        /// Used to insert exception to Exception Table
        /// </summary>
        /// <param name="subSystem"></param>
        /// <param name="screenName"></param>
        /// <param name="uiClassName"></param>l
        /// <param name="uiControlName"></param>
        /// <param name="exceptionDetails"></param>
        /// <returns></returns>
        public int LogExceptionToDB(params object[] parameters)
        {
            if (this.WorkStationId == Messages.WORKSTATION_NOT_EXIST) //#Max He, avoid hit error below line when covert to int32 but value is string
            {
                this.WorkStationId = "1";
            }
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@Message", Message));
            lstParameters.Add(new FloorSqlParameter("@StackTrace", StackTrace));
            lstParameters.Add(new FloorSqlParameter("@WorstationId", Convert.ToInt32(WorkStationId)));
            lstParameters.Add(new FloorSqlParameter("@Exceptiondatetime", ExceptionDt));
            lstParameters.Add(new FloorSqlParameter("@InnerExceptionMessage", _innerException.Message));
            lstParameters.Add(new FloorSqlParameter("@SubSystem", _subsystem));
            lstParameters.Add(new FloorSqlParameter("@SystemBaseexception", _innerException.GetBaseException().ToString()));
            lstParameters.Add(new FloorSqlParameter("@ScreenName", screenName));
            lstParameters.Add(new FloorSqlParameter("@UIClassName", uiClassName));
            lstParameters.Add(new FloorSqlParameter("@UIControlName", uiControlName));
            lstParameters.Add(new FloorSqlParameter("@MethodParameter", validateParameters(parameters)));
            int result = Convert.ToInt32(FloorDBAccess.ExecuteScalar("USP_INS_WriteLog", lstParameters));
            if (result > 0)
            {
                return result;
            }
            return Constants.ZERO;
        }

        /// <summary>
        /// Used to convert object to stringbuilder
        /// </summary>
        /// <param name="methodParameters"></param>
        /// <returns></returns>
        private string validateParameters(params object[] methodParameters)
        {
            StringBuilder resultStringBuilder = new StringBuilder();
            if (methodParameters != null)
            {
                foreach (object objParam in methodParameters)
                {
                    Type paramType = objParam.GetType();
                    bool isStruct = paramType.IsValueType && !paramType.IsPrimitive;
                    if (isStruct)
                        resultStringBuilder.Append(objParam.ToString() + ",");
                    if (objParam.GetType() == typeof(string))
                        resultStringBuilder.Append(objParam.ToString() + ",");
                }
            }
            if (resultStringBuilder.Length > 1)
            {
                resultStringBuilder.Remove(resultStringBuilder.Length - 1, 1).ToString();
            }
            return resultStringBuilder.ToString();
        }
    }
}
