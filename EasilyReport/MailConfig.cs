using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.ComponentModel;

namespace Infolight.EasilyReportTools
{
    public class MailConfig
    {
        private MailMessage message;
        
        public MailConfig()
        {
            port = 25;
            encoding = System.Text.Encoding.Default.WebName;
            message = new MailMessage();
        }
        
        private string mailFrom;
        /// <summary>
        /// Get or set sender of mail
        /// </summary>
        [Category("Infolight"),
        Description("Sender of mail")]
        public string MailFrom
        {
            get { return mailFrom; }
            set { mailFrom = value; }
        }

        private string mailTo;
        /// <summary>
        /// Get or set receiver of mail
        /// </summary>
        [Category("Infolight"),
        Description("Receiver of mail")]
        public string MailTo
        {
            get { return mailTo; }
            set { mailTo = value; }
        }

        private string subject;
        /// <summary>
        /// Get or set subject of mail
        /// </summary>
        [Category("Infolight"),
        Description("Subject of mail")]
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string body;
        /// <summary>
        /// Get or set body of mail
        /// </summary>
        [Category("Infolight"),
        Description("Body of mail")]
        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        private string host;
        /// <summary>
        /// Get or set address of smtp server
        /// </summary>
        [Category("Infolight"),
        Description("Address of smtp server")]
        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        private int port;
        /// <summary>
        /// Get or set port of smtp server
        /// </summary>
        [Category("Infolight"),
        Description("Port of smtp server")]
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        private string password;
        /// <summary>
        /// Get or set password to send mail
        /// </summary>
        [Category("Infolight"),
        Description("Password to send mail")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }


        /// <summary>
        /// Get the attachments of mail
        /// </summary>
        [Category("Infolight"),
        Description("Attachments of mail"),
        Browsable(false)]
        internal AttachmentCollection Attachments
        {
            get { return message.Attachments; }
        }

        private string encoding;
        /// <summary>
        /// Get or set encoding of mail
        /// </summary>
        [Category("Infolight"),
        Description("Encoding of mail")]
        public string Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        public MailConfig Copy()
        {
            MailConfig mail = new MailConfig();
            CopyTo(mail);
            return mail;
        }

        public void CopyTo(MailConfig mail)
        {
            foreach (Attachment attach in this.Attachments)
            {
                mail.Attachments.Add(attach);
            }
            mail.Body = this.Body;
            mail.Encoding = this.Encoding;
            mail.Host = this.Host;
            mail.MailFrom = this.MailFrom;
            mail.MailTo = this.MailTo;
            mail.Password = this.Password;
            mail.Port = this.Port;
            mail.Subject = this.Subject;
        }
    }
}
