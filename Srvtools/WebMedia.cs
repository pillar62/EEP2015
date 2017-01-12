using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

namespace Srvtools
{
    public class WebMediaPlayer: WebControl
    {
        public WebMediaPlayer()
        {
            //_AutoSize = true;
            _AutoStart = false;
            _ShowControl = true;
            //_Height = 45;
            //_Width = 300;
            _Type = MediaType.Audio;
        }

        //private int _Height;
        //[Category("Infolight"),
        //Description("Specifies the height of the media player")]
        //public int PlayerHeight
        //{
        //    get { return Math.Max(_Height, 1); }
        //    set { _Height = value; }
        //}

        //private int _Width;
        //[Category("Infolight"),
        //Description("Specifies the width of the media player")]
        //public int PlayerWidth
        //{
        //    get { return Math.Max(_Width,1); }
        //    set { _Width = value; }
        //}

        //private bool _AutoSize;
        //[Category("Infolight"),
        //Description("Indicates whether the size of media player adjusts automatically")]
        //public bool AutoSize
        //{
        //    get { return _AutoSize; }
        //    set { _AutoSize = value; }
        //}
	
	
        private bool _AutoStart;
        [Category("Infolight"),
        Description("Indicates whether the media player start automtically")]
        public bool AutoStart
        {
            get { return _AutoStart; }
            set { _AutoStart = value; }
        }

        private bool _ShowControl;
        [Category("Infolight"),
        Description("Indicates whether show the media player")]
        public bool ShowControl
        {
            get { return _ShowControl; }
            set { _ShowControl = value; }
        }

        //[Bindable(true)]
        //[Category("Infolight"),
        //Description("Specifies the file to play")]
        //public string FileName
        //{
        //    get { return this.ViewState["FileName"] == null?string.Empty:this.ViewState["FileName"].ToString(); }
        //    set { this.ViewState["FileName"] = value; }
        //}

        [Bindable(true)]
        [Category("Infolight"),
        Description("Specifies the url to play")]
        public string URL
        {
            get { return this.ViewState["URL"] == null ? string.Empty : this.ViewState["URL"].ToString(); }
            set { this.ViewState["URL"] = value; }
        }

        private MediaType _Type;
        [Category("Infolight"),
        Description("Specifies the media type to play")]
        public MediaType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
	

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                if (this.Type == MediaType.Video)
                {
                    int width = 320;
                    int height = 300;
                    if (!this.Width.IsEmpty && this.Width.Type == UnitType.Pixel)
                    {
                        width = Math.Max((int)this.Width.Value, 227);
                    }
                    if (!this.Height.IsEmpty && this.Height.Type == UnitType.Pixel)
                    {
                        height = Math.Max((int)this.Height.Value, 66);
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "3");
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("height: {0}px;width:{1}px; background-image: url(../Image/WebMediaPlayer/Video1.gif);", height - 65,width));
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "height: 65px;width:194px; background-image: url(../Image/WebMediaPlayer/Video2.gif);");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.RenderEndTag();
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("height: 65px;width:{0}px; background-image: url(../Image/WebMediaPlayer/Video3.gif);", width - 226));
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.RenderEndTag();
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "height: 65px;width:32px; background-image: url(../Image/WebMediaPlayer/Video4.gif);");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
                else
                {
                    int width = 320;
                    if (!this.Width.IsEmpty && this.Width.Type == UnitType.Pixel)
                    {
                        width = Math.Max((int)this.Width.Value, 264);
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "height: 46px;width:177px; background-image: url(../Image/WebMediaPlayer/Audio1.gif);");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.RenderEndTag();
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("height: 46px;width:{0}px; background-image: url(../Image/WebMediaPlayer/Audio2.gif);",width - 263));
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.RenderEndTag();
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "height: 46px;width:86px; background-image: url(../Image/WebMediaPlayer/Audio3.gif);");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
                
                writer.RenderEndTag();
            }
            else
            {
                StringBuilder sbulider = new StringBuilder();
                sbulider.Append(string.Format("<OBJECT ID= '{0}' ", this.ID));
                if (!this.Width.IsEmpty)
                {
                    sbulider.Append(string.Format("WIDTH='{0}' ", Width.ToString()));
                }
                if (!this.Height.IsEmpty && this.Type == MediaType.Video)
                {
                    sbulider.Append(string.Format("HEIGHT='{0}' ", Height.ToString()));
                }
                if (this.Type == MediaType.Audio)
                {
                    sbulider.Append("CLASSID='CLSID:22d6f312-b0f6-11d0-94ab-0080c74c7e95' ");
                    //sbulider.Append("CODEBASE='http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701' ");
                    //sbulider.Append("STANDBY='Loading Microsoft Windows Media Player components...' ");
                }
                else
                {
                    sbulider.Append("CLASSID='CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6' ");
                }
                sbulider.Append("TYPE='application/x-oleobject'>");
                sbulider.Append("<PARAM NAME='animationatStart' VALUE='true'>");
                sbulider.Append("<PARAM NAME='transparentatStart' VALUE='true'>");
                sbulider.Append(string.Format("<PARAM NAME='autoStart' VALUE='{0}'>", AutoStart.ToString()));
                sbulider.Append(string.Format("<PARAM NAME='showControls' VALUE='{0}'>", ShowControl.ToString()));
                if (this.Type == MediaType.Audio)
                {
                    sbulider.Append(string.Format("<PARAM NAME='fileName' VALUE='{0}'>", URL));
                }
                else
                {
                    sbulider.Append(string.Format("<PARAM NAME='url' VALUE='{0}'>", URL));
                }
                sbulider.Append("</OBJECT>");
                writer.Write(sbulider.ToString());
            }
            
        }

        public enum MediaType
        {
            Audio,
            Video
        }
    }
}
