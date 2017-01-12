using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

[assembly: System.Web.UI.WebResource("AjaxTools.UpdateProgressPanel.js", "text/javascript")]
namespace AjaxTools
{
    [Designer(typeof(DataSourceDesigner), typeof(IDesigner))]
    public class UpdateProgressPanel : WebControl
    {
        UpdateProgressContentType _contentType = UpdateProgressContentType.Image;
        string _imageUrl = "";
        string _text = "";

        [Category("InfoLight")]
        [DefaultValue(typeof(UpdateProgressContentType), "Image")]
        public UpdateProgressContentType ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        [Category("InfoLight")]
        [DefaultValue("")]
        [Editor(typeof(UrlEditor), typeof(UITypeEditor))]
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        [Category("InfoLight")]
        [DefaultValue("")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string width = this.Width.Value.ToString();
            string height = this.Height.Value.ToString();
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.Value.ToString());
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, this.Height.Value.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "updateProgressPanel");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if (this.ContentType == UpdateProgressContentType.Image)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, this.ResolveClientUrl(this.ImageUrl));
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, this.Text);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
            }
            else
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write(this.Text);
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
            //ClientScriptManager csm = this.Page.ClientScript;
            //string scriptKey = this.ClientID + "_LoadScript";
            //if (!csm.IsClientScriptBlockRegistered(scriptKey))
            //{
            //    string script = string.Format("SetUpdateProgressPanelSytle('{0}', {{width:{1}, height:{2}}});", this.ClientID, width, height);
            //    csm.RegisterClientScriptBlock(this.GetType(), scriptKey, script, true);
            //}
        }
    }
}
