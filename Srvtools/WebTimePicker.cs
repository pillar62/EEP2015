using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.ComponentModel;
using System.Drawing;

namespace Srvtools
{
    [ToolboxBitmap(typeof(WebTimePicker), "Resources.WebTimePicker.png")]
    public class WebTimePicker : TextBox, IPostBackEventHandler
    {
        [Browsable(false)]
        public string Hour
        {
            get
            {
                string time = this.Text;
                if (this.Text != null && this.Text != "")
                    return time.Substring(0, 2);
                else
                {
                    if (!DayLight)
                        return "00";
                    else
                        return "08";
                }
            }
        }

        [Browsable(false)]
        public string Minute
        {
            get
            {
                string time = this.Text;
                if (this.Text != null && this.Text != "")
                    return time.Substring(3, 2);
                return "00";
            }
        }

        [Category("Infolight"),
        Description("Specifies the interval of time")]
        public int MinuteInterval
        {
            get
            {
                object obj = this.ViewState["MinuteInterval"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 1;
            }
            set
            {
                this.ViewState["MinuteInterval"] = value;
            }
        }

        [Category("Infolight"),
        Description("Indicates whether the control display the data during the daylight time")]
        public bool DayLight
        {
            get
            {
                object obj = this.ViewState["DayLight"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["DayLight"] = value;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table); // <table>
            writer.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:0px; visibility: hidden; display: none;");
            string defTime = "00:00";
            if (this.DayLight)
            {
                defTime = "08:00";
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Value, (this.Text == null || this.Text.Trim() == "") ? defTime : this.Text);
            this.RenderBeginTag(writer);
            base.RenderContents(writer);
            this.RenderEndTag(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "Hour");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + "Hour");
            if (this.Page.Site == null)
            {
                if (this.ReadOnly)
                {
                    writer.AddAttribute("disabled", "true");
                }
                string script = this.UniqueID + ".value = this.value + ':' + " + this.UniqueID + "Minute.value;";
                if (this.AutoPostBack)
                {
                    script += Page.ClientScript.GetPostBackEventReference(this, "");
                }
                writer.AddAttribute("onchange", script);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Select);
            int startHour = 0, endHour = 24;
            if (DayLight)
            {
                startHour = 8;
                endHour = 21;
            }
            for (int i = startHour; i < endHour; i++)
            {
                StringBuilder bhour = new StringBuilder(i.ToString());
                if (bhour.Length < 2)
                    bhour.Insert(0, "0");
                if (this.Page.Site == null && bhour.ToString() == this.Hour)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Selected, "selected");
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Value, bhour.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Option);
                writer.Write(bhour.ToString());
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
            writer.RenderEndTag(); // <td>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            writer.Write(":");
            writer.RenderEndTag(); // <td>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "Minute");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + "Minute");
            if (this.Page.Site == null)
            {
                if (this.ReadOnly)
                {
                    writer.AddAttribute("disabled", "true");
                }
                string script = this.UniqueID + ".value = " + this.UniqueID + "Hour.value + ':' + this.value;";
                if (this.AutoPostBack)
                {
                    script += Page.ClientScript.GetPostBackEventReference(this, "");
                }
                writer.AddAttribute("onchange", script);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Select);
            for (int i = 0; i < 60; i+=this.MinuteInterval)
            {
                StringBuilder bminute = new StringBuilder(i.ToString());
                if (bminute.Length < 2)
                    bminute.Insert(0, "0");
                if (this.Page.Site == null && bminute.ToString() == this.Minute)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Selected, "selected");
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Value, bminute.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Option);
                writer.Write(bminute.ToString());
                writer.RenderEndTag();
            }
            writer.RenderEndTag();

            writer.RenderEndTag(); // <td>
            writer.RenderEndTag(); // <tr>
            writer.RenderEndTag(); // <table>
        }

        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            base.OnTextChanged(new EventArgs());
        }

        #endregion
    }
}
