using EFClientTools.EFServerReference;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Windows.Forms.Design;
using System.Xml;

namespace JQClientTools
{
    public class JQMail : WebControl
    {
        public JQMail()
        {
        }
        private MailMessage message = new MailMessage();

        private string password;
        /// <summary>
        /// Get or set password to send mail
        /// </summary>
        [Category("Infolight"),
        Description("Password to send mail")]
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
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

        [Category("Infolight"),
        Description("The Event triggered from send success")]
        public string SendSuccess { get; set; }
        [Category("Infolight"),
        Description("The Event triggered from send error")]
        public string SendError { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Mail);
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                RenderChildren(writer);
                writer.RenderEndTag();
            }
        }
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8220;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Mail);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px;padding:10px", Width.Value));
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal string InfolightOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("to:'{0}'", To));
                options.Add(string.Format("bCC:'{0}'", BCC));
                options.Add(string.Format("cC:'{0}'", CC));
                options.Add(string.Format("subject:'{0}'", Subject));
                options.Add(string.Format("body:'{0}'", Body));
                options.Add(string.Format("isBodyHtml:'{0}'", IsBodyHtml.ToString().ToLower()));
                options.Add(string.Format("encoding:'{0}'", Encoding));
                options.Add(string.Format("enableSsl:{0}", EnableSsl.ToString().ToLower()));
                if(!String.IsNullOrEmpty(SendSuccess))
                    options.Add(string.Format("sendSuccess:'{0}'",SendSuccess));
                if (!String.IsNullOrEmpty(SendError))
                    options.Add(string.Format("sendError:'{0}'", SendError));
                return string.Join(",", options);
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                //var optionBuilder = new StringBuilder();
                //if (!string.IsNullOrEmpty(Icon))
                //{
                //    optionBuilder.AppendFormat("iconCls:'{0}'", Icon);
                //    optionBuilder.Append(",");
                //}
                //optionBuilder.AppendFormat("closed:'{0}'", Closed);
                //return optionBuilder.ToString();
                var options = new List<string>();
                options.Add(string.Format("closed:'{0}'", "true"));
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    options.Add(string.Format("width:{0}", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    options.Add(string.Format("height:{0}", Height.Value));
                }
                return string.Join(",", options);
            }
        }
    }
}