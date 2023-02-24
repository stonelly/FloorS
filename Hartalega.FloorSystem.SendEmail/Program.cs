using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System.Transactions;

namespace Hartalega.FloorSystem.SendEmail
{
    public class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            log.Info("Email service STARTED");

            try
            {
               SendEmailBLL.SendEmailsFromQueue();
            }
            catch (FloorSystemException ex)
            {
                Exception baseException = ex.GetBaseException() ?? ex;
                log.Error(baseException.Message);
                log.Error(baseException.StackTrace);
            }
            catch (Exception ex)
            {
                var baseException = GetInnerException(ex);
                log.Error(baseException.Message);
                log.Error(baseException.StackTrace);
            }
            finally
            {
                // log success log
                log.InfoFormat("Email service ENDED. Time elapsed: {0}", stopwatch.Elapsed);
            }
        }

        static Exception GetInnerException(Exception ex)
        {
            if (ex.InnerException == null) return ex;
            else return GetInnerException(ex.InnerException);
        }
    }
}
