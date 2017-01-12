using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Web.UI.Design;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace AjaxTools
{
    [ParseChildren(false)]
    [Designer(typeof(ContainerControlDesigner))]
    public class AjaxCollapsiblePanel : WebControl, INamingContainer
    {
        public AjaxCollapsiblePanel()
        {
            if (this.ExpandedText == null || this.ExpandedText == "")
            {
                this.ExpandedText = "Collapse";
            }
            if (this.CollapsedText == null || this.CollapsedText == "")
            {
                this.CollapsedText = "Expand";
            }
            if (this.ExpandedImage == null || this.ExpandedImage == "")
            {
                this.ExpandedImage = "~/Image/Ajax/collapse.jpg";
            }
            if (this.CollapsedImage == null || this.CollapsedImage == "")
            {
                this.CollapsedImage = "~/Image/Ajax/expand.jpg";
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            _collapsiblePanelExtender.BehaviorID = this.ClientID + "_cpBehavior";
        }

        private CollapsiblePanelExtender _collapsiblePanelExtender = new CollapsiblePanelExtender();
        private Panel _headerPanel = new Panel();
        private Panel _contentPanel = new Panel();
        private Label _textLabel = new Label();
        private ImageButton _imageControl = new ImageButton();

        #region properties
        [Category("InfoLight")]
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(false)]
        public bool Collapsed
        {
            get
            {
                EnsureChildControls();
                return this._collapsiblePanelExtender.Collapsed;
            }
            set
            {
                EnsureChildControls();
                this._collapsiblePanelExtender.Collapsed = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue("~/Image/Ajax/expand.jpg")]
        public string CollapsedImage
        {
            get
            {
                EnsureChildControls();
                return this._collapsiblePanelExtender.CollapsedImage;
            }
            set
            {
                EnsureChildControls();
                this._collapsiblePanelExtender.CollapsedImage = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue("Expand")]
        public string CollapsedText
        {
            get
            {
                EnsureChildControls();
                return this._collapsiblePanelExtender.CollapsedText;
            }
            set
            {
                EnsureChildControls();
                this._collapsiblePanelExtender.CollapsedText = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(-1)]
        public int CollapsedSize
        {
            get
            {
                EnsureChildControls();
                return this._collapsiblePanelExtender.CollapsedSize;
            }
            set
            {
                EnsureChildControls();
                this._collapsiblePanelExtender.CollapsedSize = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(typeof(CollapsiblePanelExpandDirection), "Vertical")]
        public CollapsiblePanelExpandDirection ExpandDirection
        {
            get
            {
                EnsureChildControls();
                return this._collapsiblePanelExtender.ExpandDirection;
            }
            set
            {
                EnsureChildControls();
                this._collapsiblePanelExtender.ExpandDirection = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue("~/Image/Ajax/collapse.jpg")]
        public string ExpandedImage
        {
            get
            {
                EnsureChildControls();
                return this._collapsiblePanelExtender.ExpandedImage;
            }
            set
            {
                EnsureChildControls();
                this._collapsiblePanelExtender.ExpandedImage = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(-1)]
        public int ExpandedSize
        {
            get
            {
                EnsureChildControls();
                return this._collapsiblePanelExtender.ExpandedSize;
            }
            set
            {
                EnsureChildControls();
                this._collapsiblePanelExtender.ExpandedSize = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue("Collapse")]
        public string ExpandedText
        {
            get
            {
                EnsureChildControls();
                return this._collapsiblePanelExtender.ExpandedText;
            }
            set
            {
                EnsureChildControls();
                this._collapsiblePanelExtender.ExpandedText = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(false)]
        public bool ScrollContents
        {
            get
            {
                EnsureChildControls();
                return this._collapsiblePanelExtender.ScrollContents;
            }
            set
            {
                EnsureChildControls();
                this._collapsiblePanelExtender.ScrollContents = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(true)]
        public bool Animation
        {
            get
            {
                object obj = this.ViewState["Animation"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["Animation"] = value;
                EnsureChildControls();
                if (!DesignMode && !value && this.Controls.Contains(_collapsiblePanelExtender))
                    this.Controls.Remove(_collapsiblePanelExtender);
            }
        }

        [Category("InfoLight")]
        [DefaultValue(typeof(Color), "#FFFFFF")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color TextColor
        {
            get
            {
                object obj = this.ViewState["TextColor"];
                if (obj != null)
                    return (Color)obj;
                return ColorTranslator.FromHtml("#FFFFFF");
            }
            set
            {
                this.ViewState["TextColor"] = value;
            }
        }
        #endregion

        protected override void CreateChildControls()
        {
            Controls.Clear();
            _headerPanel = new Panel();
            _headerPanel.ID = "headerPanel";
            //_headerPanel.BackImageUrl = "~/Image/Ajax/bg-menu-main.png";
            this.Controls.Add(_headerPanel);

            _contentPanel = new Panel();
            _contentPanel.ID = "contentPanel";
            this.Controls.Add(_contentPanel);

            _textLabel = new Label();
            _textLabel.ID = "textLabel";
            this.Controls.Add(_textLabel);

            _imageControl = new ImageButton();
            _imageControl.ID = "imageControl";
            this.Controls.Add(_imageControl);

            if (!this.DesignMode)
            {
                _collapsiblePanelExtender = new CollapsiblePanelExtender();
                _collapsiblePanelExtender.ID = "collapsiblePanelExtender";
                _collapsiblePanelExtender.TargetControlID = "contentPanel";
                _collapsiblePanelExtender.ExpandControlID = "headerPanel";
                _collapsiblePanelExtender.CollapseControlID = "headerPanel";
                _collapsiblePanelExtender.TextLabelID = "textLabel";
                _collapsiblePanelExtender.ImageControlID = "imageControl";
                _collapsiblePanelExtender.SuppressPostBack = true;
                //_collapsiblePanelExtender.BehaviorID = this.ClientID + "_cpBehavior";
                this.Controls.Add(_collapsiblePanelExtender);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            #region _headerPanel
            string srcpath = this.CollapsedImage.Substring(0, this.CollapsedImage.LastIndexOf('/') + 1);
            if (srcpath.IndexOf("~") != -1)
            {
                srcpath = srcpath.Replace("~", "..");
            }
            if (this.ExpandDirection == CollapsiblePanelExpandDirection.Vertical)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxclpan_header_h");
                if (this.Width == Unit.Empty)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
                else
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.Value.ToString() + "px");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, ColorTranslator.ToHtml(this.TextColor));
                _headerPanel.RenderBeginTag(writer);
                #region div container
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxclpan_header_vbox");
                if (this.Animation)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Cursor, "pointer");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
                #region div label
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxclpan_header_label");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
                _textLabel.RenderControl(writer);
                writer.RenderEndTag(); // </div>
                #endregion
                #region div image
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxclpan_header_img");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
                _imageControl.RenderControl(writer);
                writer.RenderEndTag(); // </div>
                #endregion
                writer.RenderEndTag(); // </div>
                #endregion
                _headerPanel.RenderEndTag(writer);
            }
            else if (this.ExpandDirection == CollapsiblePanelExpandDirection.Horizontal)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxclpan_header_v");
                if (this.Height == Unit.Empty)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
                else
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, this.Height.Value.ToString() + "px");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, ColorTranslator.ToHtml(this.TextColor));
                _headerPanel.RenderBeginTag(writer);
                #region table container
                if (this.Animation)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Cursor, "pointer");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxclpan_header_vbox");
                writer.RenderBeginTag(HtmlTextWriterTag.Table); // <table>
                writer.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
                writer.AddStyleAttribute(HtmlTextWriterStyle.VerticalAlign, "top");
                writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
                #region div label
                writer.AddStyleAttribute("align", "top");
                writer.AddStyleAttribute("writing-mode", "tb-rl"); //writer.AddStyleAttribute("layout-flow", "vertical-ideographic");
                writer.AddStyleAttribute(HtmlTextWriterStyle.MarginTop, "20px");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
                _textLabel.RenderControl(writer);
                writer.RenderEndTag(); // </div>
                #endregion
                writer.RenderEndTag(); // </td>
                writer.RenderEndTag(); // </tr>
                writer.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
                writer.AddStyleAttribute(HtmlTextWriterStyle.VerticalAlign, "bottom");
                writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
                #region div image
                writer.AddStyleAttribute("align", "bottom");
                writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
                _imageControl.RenderControl(writer);
                writer.RenderEndTag(); // </div>
                #endregion
                writer.RenderEndTag(); // </td>
                writer.RenderEndTag(); // </tr>
                writer.RenderEndTag(); // </table>
                #endregion
                _headerPanel.RenderEndTag(writer);
            }
            #endregion

            #region _contentPanel
            if (this.Animation || (bool)this.ViewState["visible"])
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxclpan_content");
                if (this.ExpandDirection == CollapsiblePanelExpandDirection.Horizontal)
                    writer.AddStyleAttribute("float", "left");
                _contentPanel.RenderBeginTag(writer);
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl.ID != "headerPanel" && ctrl.ID != "contentPanel" && ctrl.ID != "textLabel" && ctrl.ID != "imageControl")
                        ctrl.RenderControl(writer);
                }
                _contentPanel.RenderEndTag(writer);
            }
            #endregion
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Animation)
            {
                if (!this.Page.IsPostBack)
                {
                    this.ViewState["visible"] = true;
                    this._imageControl.ImageUrl = this.ExpandedImage;
                    this._textLabel.Text = this.ExpandedText;
                }
                this._imageControl.Click += new ImageClickEventHandler(_imageControl_Click);
            }
            base.OnLoad(e);
        }

        void _imageControl_Click(object sender, ImageClickEventArgs e)
        {
            if (this._imageControl.ImageUrl == this.CollapsedImage && this._textLabel.Text == this.CollapsedText)
            {
                this._imageControl.ImageUrl = this.ExpandedImage;
                this._textLabel.Text = this.ExpandedText;
                this.ViewState["visible"]= true;
                //this._contentPanel.Visible = true;
            }
            else if(this._imageControl.ImageUrl == this.ExpandedImage && this._textLabel.Text == this.ExpandedText)
            {
                this._imageControl.ImageUrl = this.CollapsedImage;
                this._textLabel.Text = this.CollapsedText;
                this.ViewState["visible"] = false;
                //this._contentPanel.Visible = false;
            }
        }
    }
}
