using System;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Net.Mail;
using System.Net;
using System.Web.UI;

namespace Srvtools
{
    public class WebMail: WebControl
    {
        public WebMail() { }

        private MailMessage message = new MailMessage();

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

        private string from;
        /// <summary>
        /// Get or set sender of mail
        /// </summary>
        [Category("Infolight"),
        Description("Sender of mail")]
        public string From
        {
            get { return from; }
            set { from = value; }
        }

        private string to;
        /// <summary>
        /// Get or set receiver of mail
        /// </summary>
        [Category("Infolight"),
        Description("Receiver of mail")]
        public string To
        {
            get { return to; }
            set { to = value; }
        }

        private string bCC;
        /// <summary>
        /// Get or set BCC of mail
        /// </summary>
        [Category("Infolight"),
        Description("BCC of mail")]
        public string BCC
        {
            get { return bCC; }
            set { bCC = value; }
        }

        private string cC;
        /// <summary>
        /// Get or set CC of mail
        /// </summary>
        [Category("Infolight"),
        Description("CC of mail")]
        public string CC
        {
            get { return cC; }
            set { cC = value; }
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

        private bool isBodyHtml;
        /// <summary>
        /// Get or set isbodyhtml of mail
        /// </summary>
        [Category("Infolight"),
        Description("IsBodyHtml of mail")]
        public bool IsBodyHtml
        {
            get { return isBodyHtml; }
            set { isBodyHtml = value; }
        }
	

        private string encoding = System.Text.Encoding.Default.WebName;
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

        /// <summary>
        /// Get the attachments of mail
        /// </summary>
        [Category("Infolight"),
        Description("Attachments of mail"),
        Browsable(false)]
        public AttachmentCollection Attachments
        {
            get { return message.Attachments; }
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

        private int port = 25;
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

        private bool enableSsl;
        /// <summary>
        /// Get or set enable ssl
        /// </summary>
        [Category("Infolight"),
        Description("Enable ssl in smtp server")]
        public bool EnableSsl
        {
            get { return enableSsl; }
            set { enableSsl = value; }
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "blue");
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                writer.Write(this.ID);
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="SendAsync">send in async mode</param>
        public void Send(bool SendAsync)
        {
            message.From = new MailAddress(From);
            AddMailAddress(message.To, To);
            AddMailAddress(message.Bcc, BCC);
            AddMailAddress(message.CC, CC);
            message.SubjectEncoding = System.Text.Encoding.GetEncoding(Encoding);
            message.BodyEncoding = System.Text.Encoding.GetEncoding(Encoding);
            message.Subject = Subject;
            message.Body = Body;
            message.IsBodyHtml = IsBodyHtml;

            SmtpClient client = new SmtpClient(Host, Port);
            client.EnableSsl = EnableSsl;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(From, Password);
            if (SendAsync)
            {
                client.SendAsync(message, null);
            }
            else
            {
                client.Send(message);
            }
        }

        private void AddMailAddress(MailAddressCollection collection, string address)
        {
            if (!string.IsNullOrEmpty(address))
            {
                string[] straddresses = address.Split(';');
                foreach (string str in straddresses)
                {
                    collection.Add(str);
                }
            }
        }
    }
}
