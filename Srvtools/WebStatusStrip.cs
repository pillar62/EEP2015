using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing.Design;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace Srvtools
{
    [ToolboxBitmap(typeof(WebStatusStrip), "Resources.WebStatusStrip.ico")]
    public class WebStatusStrip: WebInfoBaseControl
    {
        public WebStatusStrip()
        {
            _orientation = orientationtype.Horizontal;
            _ShowUserID = false;
            _ShowUserName = false;
            _ShowDate = false;
            _ShowEEPAlias = false;
            _ShowSolution = false;
            _ShowCompany = false;
            _ShowNavigatorStatus = false;
            _ShowTitle = false;
            _ShowCount = false;
            _CountFormat = "{0}";
            //_ContentForeColor = SystemColors.ControlText;
            //_ContentBackColor = Color.White;
        
        }

        public enum orientationtype
        {
            Horizontal,
            Vertical
        
        }

        private orientationtype _orientation;
        [Category("Infolight"),
        Description("Orientation of StausStrip")]
        public orientationtype Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
            }
        }

        private bool _ShowUserID;
        [Category("Infolight"),
        Description("Indicates whether the information of user's id is displayed on WebStatusStrip")]
        public bool ShowUserID
        {
            get 
            {
                return _ShowUserID; 
            }
            set
            {
                _ShowUserID = value;
            }
        }

        private bool _ShowUserName;
        [Category("Infolight"),
        Description("Indicates whether the information of user's name is displayed on WebStatusStrip")]
        public bool ShowUserName
        {
            get 
            { 
                return _ShowUserName; 
            }
            set
            {
                _ShowUserName = value;
            }
        }

        private bool _ShowDate;
        [Category("Infolight"),
        Description("Indicates whether the information of date is displayed on WebStatusStrip")]
        public bool ShowDate
        {
            get 
            {
                return _ShowDate; 
            }
            set
            {
                _ShowDate = value;
            }
        }

        private bool _ShowEEPAlias;
        [Category("Infolight"),
        Description("Indicates whether the information of database is displayed on WebStatusStrip")]
        public bool ShowEEPAlias
        {
            get 
            {
                return _ShowEEPAlias; 
            }
            set
            {
                _ShowEEPAlias = value;
            }

        }

        private bool _ShowSolution;
        [Category("Infolight"),
        Description("Indicates whether the information of solution is displayed on WebStatusStrip")]
        public bool ShowSolution
        {
            get
            {
                return _ShowSolution; 
            }
            set
            {
                _ShowSolution = value;
             
            }

        }

        private bool _ShowCompany;
        [Category("Infolight"),
        Description("Indicates whether the information of company is displayed on WebStatusStrip")]
        public bool ShowCompany
        {
            get 
            {
                return _ShowCompany;
            }
            set
            {
                _ShowCompany = value;
            }
        }

        private bool _ShowNavigatorStatus;
        [Category("Infolight"),
        Description("Indicates whether the information of status of WebNavigator is displayed on WebStatusStrip")]
        public bool ShowNavigatorStatus
        {
            get 
            { 
                return _ShowNavigatorStatus; 
            }
            set
            {
                _ShowNavigatorStatus = value;
            }
        }

        private bool _ShowTitle;
        [Category("Infolight"),
        Description("Indicates whether the title of Page is displayed on WebStatusStrip")]
        public bool ShowTitle
        {
            get
            {
                return _ShowTitle;
            }
            set
            {
                _ShowTitle = value;
            }
        }

        private bool _ShowCount;
        [Category("Infolight"),
        Description("Indicates whether the count of data is displayed on WebStatusStrip")]
        public bool ShowCount
        {
            get { return _ShowCount; }
            set { _ShowCount = value; }
        }

        private string _CountFormat;
        [Category("Infolight"),
        Description("Specifies the format of count item")]
        public string CountFormat
        {
            get { return _CountFormat; }
            set { _CountFormat = value; }
        }

        private string _titleCss = "";
        [Category("Infolight")]
        [DefaultValue("")]
        public string TitleCss
        {
            get { return _titleCss; }
            set { _titleCss = value; }
        }

        private string _statusCss = "";
        [Category("Infolight")]
        [DefaultValue("")]
        public string StatusCss
        {
            get { return _statusCss; }
            set { _statusCss = value; }
        }

        private string _captionCss = "";
        [Category("Infolight")]
        [DefaultValue("")]
        public string CaptionCss
        {
            get { return _captionCss; }
            set { _captionCss = value; }
        }

        private string _contentCss = "";
        [Category("Infolight")]
        [DefaultValue("")]
        public string ContentCss
        {
            get { return _contentCss; }
            set { _contentCss = value; }
        }

        private string _groupCss = "";
        [Category("Infolight")]
        [DefaultValue("")]
        public string GroupCss
        {
            get { return _groupCss; }
            set { _groupCss = value; }
        }

        private Color _ContentForeColor;
        [Category("Infolight"),
        Description("Color of the text of the content")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color ContentForeColor
        {
            get
            {
                return _ContentForeColor;
            }
            set
            {
                _ContentForeColor = value;
            }
        }


        private Color _ContentBackColor;
        [Category("Infolight"),
        Description("Color of the background of the content")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color ContentBackColor
        {
            get
            {
                return _ContentBackColor;
            }
            set
            {
                _ContentBackColor = value;
            }
        }

        private Color _TitleForeColor;
        [Category("Infolight"),
        Description("Color of the text of the title text")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color TitleForeColor
        {
            get
            {
                return _TitleForeColor;
            }
            set
            {
                _TitleForeColor = value;
            }
        }

        private Color _TitleBackColor;
        [Category("Infolight"),
        Description("Color of the background of the title text")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color TitleBackColor
        {
            get
            {
                return _TitleBackColor;
            }
            set
            {
                _TitleBackColor = value;
            }
        }

        private Color _StatusForeColor;
        [Category("Infolight"),
        Description("Color of the text of the status text")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color StatusForeColor
        {
            get
            {
                return _StatusForeColor;
            }
            set
            {
                _StatusForeColor = value;
            }
        }

        private Color _StatusBackColor;
        [Category("Infolight"),
        Description("Color of the background of the status text")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color StatusBackColor
        {
            get
            {
                return _StatusBackColor;
            }
            set
            {
                _StatusBackColor = value;
            }
        }

        private bool _GetRealRecordsCount;
        [Category("Infolight")]
        public bool GetRealRecordsCount
        {
            get
            {
                return _GetRealRecordsCount;
            }
            set
            {
                _GetRealRecordsCount = value;
            }
        }

        [Browsable(false)]
        public string NavigatorStatusText
        {
            get
            {
                object obj = this.ViewState["NavigatorStatusText"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["NavigatorStatusText"] = value;
            }
        }

        [Category("Infolight"),
        Description("Separator between the items of statusstrip")]
        public string Separator
        {
            get
            {
                object obj = this.ViewState["Separator"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["Separator"] = value;
            }
        }

        [Browsable(false)]
        public string CountViewID
        {
            get
            {
                object obj = this.ViewState["CountViewID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["CountViewID"] = value;
            }
        }

        private string fItemParam;
        [Category("Infolight")]
        [Browsable(false)]
        public string ItemParam
        {
            get { return fItemParam; }
            set { fItemParam = value; }
        }

        private string GetHtmlText(string text)
        {
            string htmltext = "";
            char[] chartext = text.ToCharArray();
            foreach (char ch in chartext)
            {
                switch (ch)
                {
                    case ' ': htmltext += "&nbsp;"; break;
                    case '&': htmltext += "&amp;"; break;
                    case '<': htmltext += "&lt;"; break;
                    case '>': htmltext += "&gt;"; break;
                    //case '"': htmltext += "&quto"; break;
                    default: htmltext += ch.ToString(); break;
                }
            }
            return htmltext;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.Page.Request.QueryString["itemparam"] != null)
            {
                ItemParam = this.Page.Request.QueryString["itemparam"].ToString();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            string rscUrl = "../css/controls/WebStatusStrip.css";
            bool isCssExist = false;
            foreach (Control ctrl in this.Page.Header.Controls)
            {
                if (ctrl is HtmlLink && ((HtmlLink)ctrl).Href == rscUrl)
                    isCssExist = true;
            }
            if (!isCssExist)
            {
                HtmlLink cssLink = new HtmlLink();
                cssLink.Href = rscUrl;
                cssLink.Attributes.Add("rel", "stylesheet");
                cssLink.Attributes.Add("type", "text/css");
                this.Page.Header.Controls.Add(cssLink);
            }
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoStatusStrip", "StripMsg", true);
            string[] text = message.Split(';');
            string[] striptext = new string[18];
            if (this.DesignMode)
            {
                this.NavigatorStatusText = "NavStatus";
            }
            string title = (this.DesignMode ? "Title" : this.Page.Title);
            string count = (this.DesignMode ? CountFormat : GetCount());
            striptext[0] = (this.ShowTitle ? title : "");
            striptext[1] = "";
            striptext[2] = (this.ShowUserID ? text[0] : "");
            striptext[3] = (this.ShowUserID ? CliUtils.fLoginUser : "");
            striptext[4] = (this.ShowUserName ? text[1] : "");
            striptext[5] = (this.ShowUserName ? CliUtils.fUserName : "");
            striptext[6] = (this.ShowDate ? text[2] : "");
            striptext[7] = (this.ShowDate ? DateTime.Now.ToShortDateString() : "");
            striptext[8] = (this.ShowEEPAlias ? text[3] : "");
            striptext[9] = (this.ShowEEPAlias ? CliUtils.fLoginDB : "");
            striptext[10] = (this.ShowSolution ? text[4] : "");
            striptext[11] = (this.ShowSolution ? CliUtils.fCurrentProject : "");
            striptext[12] = (this.ShowCompany ? text[5] : "");
            striptext[13] = (this.ShowCompany ? CliUtils.fSiteCode : "");
            striptext[14] = (this.ShowCount ? text[6] : "");
            striptext[15] = (this.ShowCount ? count : "");
            striptext[16] = (this.ShowNavigatorStatus ? this.NavigatorStatusText : "");
            striptext[17] = "";
            string sep = GetHtmlText(Separator);

            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            if (this.Orientation == orientationtype.Horizontal)
            {
                CreatLabel(writer, striptext, false, sep);
            }
            else
            {
                CreatLabel(writer, striptext, true, sep);
            }
            writer.RenderEndTag();
        }

        private string GetCount()//
        {
            string count = string.Empty;
            if (this.ShowCount && CountViewID.Length > 0)
            {
                object view = this.GetObjByID(CountViewID);
                if (view != null)
                {
                    string datasourceid = view.GetType().GetProperty("DataSourceID").GetValue(view, null).ToString();
                    object datasource = this.GetObjByID(datasourceid);
                    if (datasource != null && datasource is WebDataSource)
                    {
                        WebDataSource wds = datasource as WebDataSource;
                        if (this.GetRealRecordsCount)
                        {
                            if (!string.IsNullOrEmpty(wds.RemoteName))
                            {
                                int index = wds.RemoteName.LastIndexOf('.');
                                count = CliUtils.GetRecordsCount(wds.RemoteName.Substring(0, index), wds.RemoteName.Substring(index + 1)
                                    , wds.WhereStr, CliUtils.fCurrentProject).ToString();

                            }
                            else
                            {
                                throw new EEPException(EEPException.ExceptionType.MethodNotSupported,this.GetType(), this.ID, "GetRealRecordsCount", null);
                            }
                        }
                        else
                        {
                            count = string.Format(CountFormat, wds.View.Count);

                        }
                    }
                }
            }
            return count;
        }

        private void GenLabel(HtmlTextWriter writer, int i, string text, bool isCaption)
        {
            //writer.RenderBeginTag(HtmlTextWriterTag.Td);
            //if (i == 0)
            //{
            //    writer.AddAttribute(HtmlTextWriterAttribute.Class, "TitleCss");
            //}
            //else if (i == 7)
            //{
            //    writer.AddAttribute(HtmlTextWriterAttribute.Class, "StatusCss");
            //}
            //else
            //{
            //    if (isCaption)
            //        writer.AddAttribute(HtmlTextWriterAttribute.Class, "CaptionCss");
            //    else
            //        writer.AddAttribute(HtmlTextWriterAttribute.Class, "ContentCss");
            //}
            //writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //writer.Write(text);
            //writer.RenderEndTag();
            //writer.RenderEndTag();

            if (i == 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "statusstrip_title_1");
            }
            else if (i == 8)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "statusstrip_status_1");
            }
            else
            {
                if (isCaption)
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "statusstrip_caption_1");
                else
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "statusstrip_content_1");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(text);
            writer.RenderEndTag();
        }

        private void CreatLabel(System.Web.UI.HtmlTextWriter writer, string[] labeltext, bool newline, string splitchar)
        {
            if (!newline)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            }
            for (int i = 0; i < labeltext.Length / 2; i++)
            {
                if (labeltext[2 * i] != "" || labeltext[2 * i + 1] != "")
                {
                    if (newline)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    }
                    if (labeltext[2 * i] != "")
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        if (i != 0 && i != 8)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "statusstrip_group_1");
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);
                            GenLabel(writer, i, labeltext[2 * i], true);
                            GenLabel(writer, i, labeltext[2 * i + 1], false);
                            writer.RenderEndTag();
                        }
                        else
                        {
                            GenLabel(writer, i, labeltext[2 * i], true);
                        }
                        writer.RenderEndTag();
                    }
                    if (newline)
                    {
                        writer.RenderEndTag();
                    }
                }
            }
            if (!newline)
            {
                writer.RenderEndTag();
            }
            //if (!newline)
            //{
            //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            //}
            //for (int i = 0; i < labeltext.Length / 2; i++)
            //{
            //    if (labeltext[2 * i] != "" || labeltext[2 * i + 1] != "")
            //    {
            //        if (newline)
            //        {
            //            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            //        }
            //        if (labeltext[2 * i] != "")
            //        {
            //            GenLabel(writer, i, labeltext[2 * i], true);
            //            if (i != 0 && i != 7)
            //                GenLabel(writer, i, labeltext[2 * i + 1], false);
            //        }
            //        if (newline)
            //        {
            //            writer.RenderEndTag();
            //        }
            //    }
            //}
            //if (!newline)
            //{
            //    writer.RenderEndTag();
            //}
        }
    }

    public class StripItemCollection: InfoOwnerCollection
    {
        public StripItemCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(StripItem))
        {
        }

        public new StripItem this[int index]
        {
            get
            {
                return (StripItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is StripItem)
                    {
                        //原来的Collection设置为0
                        ((StripItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((StripItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class StripItem:InfoOwnerCollectionItem
    {
        public StripItem()
        {
        
        }

        private string _name;
        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
    }
}
