using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using AjaxControlToolkit;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;

namespace AjaxTools
{
    [ParseChildren(false)]
    [Designer(typeof(ContainerControlDesigner))]
    public class AjaxDragPanel : WebControl, INamingContainer
    {
        private DragPanelExtender _dragPanelExtender = new DragPanelExtender();
        private Panel containerPanel = new Panel();
        private Panel dragHandler = new Panel();
        //private Panel contentPanel = new Panel();

        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        [Category("InfoLight")]
        public string HeaderText
        {
            get
            {
                object obj = this.ViewState["HeaderText"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["HeaderText"] = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(false)]
        public bool InnerBorder
        {
            get
            {
                object obj = this.ViewState["InnerBorder"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["InnerBorder"] = value;
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            containerPanel = new Panel();
            containerPanel.ID = "containerPanel";
            containerPanel.CssClass = this.CssClass;
            this.Controls.Add(containerPanel);

            dragHandler = new Panel();
            dragHandler.ID = "dragHandler";
            this.Controls.Add(dragHandler);

            //contentPanel = new Panel();
            //contentPanel.ID = "contentPanel";
            //this.Controls.Add(contentPanel);

            if (!this.DesignMode)
            {
                _dragPanelExtender = new DragPanelExtender();
                _dragPanelExtender.ID = "_dragPanelExtender";
                _dragPanelExtender.TargetControlID = "containerPanel";
                _dragPanelExtender.DragHandleID = "dragHandler";
                this.Controls.Add(_dragPanelExtender);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxdrag_container");
            if (this.Width != Unit.Empty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.Value.ToString() + "px");
            if (this.Height != Unit.Empty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, this.Height.Value.ToString() + "px");
            containerPanel.RenderBeginTag(writer); // <containerPanel>
            if (this.InnerBorder)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxdrag_bodered_drag_handle");
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxdrag_drag_handle");
            }
            dragHandler.RenderBeginTag(writer); // <dragHandler>
            //writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
            //writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "20px");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Cursor, "move");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxdrag_header_container");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(HeaderText);
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxdrag_header_bg");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
            writer.RenderEndTag();
            dragHandler.RenderEndTag(writer);// </dragHandler>
            if (this.InnerBorder)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxdrag_bodered_content");
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxdrag_content");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            foreach (Control ctrl in this.Controls)
            {
                ctrl.RenderControl(writer);
            }
            writer.RenderEndTag();
            containerPanel.RenderEndTag(writer); // </containerPanel>
            //if (!this.DesignMode)
            //    this._dragPanelExtender.RenderControl(writer);
        }
    }
}
