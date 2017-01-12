using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Infolight.EasilyReportTools.Tools
{
    internal class MailSender
    {
        #region Variable Definition
        IReport report;
        private MailMessage mailMessage;
        #endregion

        public MailSender(IReport rpt)
        {
            this.report = rpt;
        }

        public void SendMail()
        {
            string[] strMailTo = null;
            SmtpClient smtpClient = new SmtpClient();

            strMailTo = this.report.MailSetting.MailTo.Split(':');
            mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(this.report.MailSetting.MailFrom);
            mailMessage.Subject = this.report.MailSetting.Subject;

            foreach (Attachment attach in this.report.MailSetting.Attachments)
            {
                mailMessage.Attachments.Add(attach);
            }

            smtpClient = new SmtpClient(this.report.MailSetting.Host);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(this.report.MailSetting.MailFrom, this.report.MailSetting.Password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            for (int i = 0; i < strMailTo.Length; i++)
            {
                if (strMailTo[i].Trim().Length > 0)
                {

                    mailMessage.To.Clear();
                    mailMessage.To.Add(new MailAddress(strMailTo[i].Trim()));
                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}
