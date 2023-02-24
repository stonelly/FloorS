using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System.Data;
using Hartalega.FloorSystem.Framework.Cache;


namespace Hartalega.FloorSystem.Business.Logic
{
    public class SendEmailBLL : Framework.Business.BusinessBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void QueueEmail(string recipients, string subject, string content)
        {
           
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
            PrmList.Add(new FloorSqlParameter("@Recipients", recipients));
            PrmList.Add(new FloorSqlParameter("@Subject", subject));
            PrmList.Add(new FloorSqlParameter("@Content", content));
             FloorDBAccess.ExecuteNonQuery("USP_FS_InsertEmailQueue", PrmList);
           
        }

        public static void SendEmailsFromQueue()
        {
            try
            {
                //fill configuration data for email host
                CommonBLL.GetFloorSystemConfiguration();

                var emailQueue = GetEmailQueue();
                log.InfoFormat("Total Queue: {0}", emailQueue.Rows.Count);
                var totalSent = 0;
                foreach (DataRow row in emailQueue.Rows)
                {
                    if (SendEmail(row["Subject"].ToString(), row["Content"].ToString(), row["Recipients"].ToString(), row["EmailQueueId"].ToString()))
                        totalSent += 1;
                }
                log.InfoFormat("Total Send: {0}", totalSent);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException("Exception occured while sending email.", Constants.BUSINESSLOGIC, ex);
            }
        }

        static bool SendEmail(string subject, string content, string recipientEmail, string emailQueueId)
        {
            string fromAddress = FloorSystemConfiguration.GetInstance().strHartalegaAlertEmail;
            string toAddress = recipientEmail;
            string hostName = FloorSystemConfiguration.GetInstance().strSMTPSender;
            string emailSubject = subject;
            string emailBody = content;
            try
            {
                Email.SendEmail(fromAddress, toAddress, string.Empty, string.Empty,
                                emailSubject, emailBody, true, System.Net.Mail.MailPriority.High, string.Empty,
                                hostName, 0, FloorSystemConfiguration.GetInstance().strEmailUserId, 
                                FloorSystemConfiguration.GetInstance().strEmailPassword);

                UpdateEmailSent(emailQueueId);
                return true;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error: {0}; EmailQueueId: {1}", ex.Message, emailQueueId);
                return false;
            }
        }

        /// <summary>
        /// Get email from emailqueue
        /// </summary>
        /// <returns></returns>

        public static DataTable GetEmailQueue()
        {
            DataTable dtTableDetails;
            dtTableDetails = FloorDBAccess.ExecuteDataTable("USP_GET_WorkOrderEmailQueue", null);
            return dtTableDetails;
        }

        /// <summary>
        /// Update EmailQueue sent date and issent column
        /// </summary>
        /// <param name="emailQueueId"></param>
        /// <returns></returns>
        public static int UpdateEmailSent(string emailQueueId)
        {
            List<FloorSqlParameter> lstfp = new List<FloorSqlParameter>();
            lstfp.Add(new FloorSqlParameter("@EmailQueueId", emailQueueId));
            return FloorDBAccess.ExecuteNonQuery("USP_FP_UpdateEmailSent", lstfp);
        }

    }
}
