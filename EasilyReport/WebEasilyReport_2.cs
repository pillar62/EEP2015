using System;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infolight.EasilyReportTools.DataCenter;
using Infolight.EasilyReportTools.Tools;
using Srvtools;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Infolight.EasilyReportTools.Config;
using AjaxControlToolkit;
using Infolight.EasilyReportTools.UI;

namespace Infolight.EasilyReportTools
{
    public partial class WebEasilyReport
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(this.ID);
                writer.RenderEndTag();
            }
            else
            {
                //RenderIframe(writer);
                base.Render(writer);

            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Page != null)
            {
                if (this.UIType == EasilyReportUIType.AspNet)
                {
                    string script = AspNetScriptsProvider.GetScripts();
                    AspNetScriptsProvider.RegisterStartupScript(this, script);
                }
            }

            base.OnPreRender(e);

            if (this.UIType == EasilyReportUIType.AspNet)
            {
                string rscUrl = WebEasilyReportCSS.CssUrl;
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
        }
       

        protected override void CreateChildControls()
        {
            if (!this.DesignMode)
            {
                render.CreateChildControls();
            }
        }
    }
}
