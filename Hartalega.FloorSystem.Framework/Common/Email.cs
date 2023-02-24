// -----------------------------------------------------------------------
// <copyright file="Email.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Framework.Common
{
    using System;
    using System.Data;
    using System.Net;
    using System.Net.Mail;

    /// <summary>
    /// Cannot create instance and inherit this class.
    /// This class can be used to send an email with different parameters.
    /// </summary>
    public static class Email
    {
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="fromAddress">From email address</param>
        /// <param name="toAddresses">To email address. Use semi (;) colon to separate emails.</param>
        /// <param name="additionalAddresses">Cc email address. Use semi (;) colon to separate emails.</param>
        /// <param name="bccAddresses">Bcc email address. Use semi (;) colon to separate emails.</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="body">Mail body details</param>
        /// <param name="isHtml">Mail style</param>
        /// <param name="mailPriority">Mail Priority</param>
        /// <param name="filePath">Attachment file path</param>
        #region
        public static void SendEmail(
                            string fromAddress,
                            string toAddresses,
                            string additionalAddresses,
                            string bccAddresses,
                            string subject,
                            string body,
                            bool isHtml,
                            MailPriority mailPriority,
                            string filePath, string hostName, int portNumber, string userName, string passWord)
        {
            //string hostName = ConfigManager.GetAppSettingValueByKey("HostName");           
            //int portNumber = Convert.ToInt16(ConfigManager.GetAppSettingValueByKey("PortNumber"));

            if (string.IsNullOrEmpty(hostName))
            {
                throw new NoNullAllowedException("SMTP host name and port number was not exist in app/web.config file App Settings.");
            }

            using (SmtpClient smtpClient = new SmtpClient(hostName))
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    if (!string.IsNullOrEmpty(fromAddress) && !string.IsNullOrEmpty(toAddresses))
                    {
                        mailMessage.From = new MailAddress(fromAddress);
                        foreach (string toAddress in toAddresses.Split(';'))
                        {
                            mailMessage.To.Add(toAddress);
                        }
                        
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = isHtml;
                        mailMessage.Priority = mailPriority;

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            using (Attachment attachment = new Attachment(filePath))
                            {
                                mailMessage.Attachments.Add(attachment);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(additionalAddresses))
                    {
                        foreach (string address in additionalAddresses.Split(';'))
                        {
                            mailMessage.CC.Add(address);
                        }
                    }

                    if (!string.IsNullOrEmpty(bccAddresses))
                    {
                        foreach (string bccAddress in bccAddresses.Split(';'))
                        {
                            mailMessage.Bcc.Add(bccAddress);
                        }
                    }

                    smtpClient.Credentials = new NetworkCredential(EncryptDecrypt.GetDecryptedString(userName, "hidden"), EncryptDecrypt.GetDecryptedString(passWord,"hidden"));
                    //smtpClient.UseDefaultCredentials = false;
                    smtpClient.Send(mailMessage);
                }
            }
        }
        #endregion
    }
}
