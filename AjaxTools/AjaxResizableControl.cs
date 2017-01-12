using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using AjaxControlToolkit;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Security.Permissions;
using Srvtools;
using System.Web.UI.HtmlControls;

namespace AjaxTools
{
    public class AjaxResizableControl : Panel, INamingContainer
    {
        public AjaxResizableControl()
        {
            if (this.HandleCssClass == null || this.HandleCssClass == "")
            {
                this.HandleCssClass = "ajaxresize_handleResizableControl";
            }
            if (this.ResizableCssClass == null || this.ResizableCssClass == "")
            {
                this.ResizableCssClass = "ajaxresize_resizingResizableControl";
            }
        }

        private ResizableControlExtender _resizableControl = new ResizableControlExtender();

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
                EnsureChildControls();
                this._resizableControl.ID = "_resizableControl";
                this._resizableControl.TargetControlID = this.ClientID;
            }
        }

        [Category("Infolight")]
        public string BehaviorID
        {
            get
            {
                EnsureChildControls();
                return this._resizableControl.BehaviorID;
            }
            set
            {
                EnsureChildControls();
                this._resizableControl.BehaviorID = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("ajaxresize_handleResizableControl")]
        public string HandleCssClass
        {
            get
            {
                EnsureChildControls();
                return this._resizableControl.HandleCssClass;
            }
            set
            {
                EnsureChildControls();
                this._resizableControl.HandleCssClass = value;
            }
        }

        [Category("Infolight")]
        public int HandleOffsetX
        {
            get
            {
                EnsureChildControls();
                return this._resizableControl.HandleOffsetX;
            }
            set
            {
                EnsureChildControls();
                this._resizableControl.HandleOffsetX = value;
            }
        }

        [Category("Infolight")]
        public int HandleOffsetY
        {
            get
            {
                EnsureChildControls();
                return this._resizableControl.HandleOffsetY;
            }
            set
            {
                EnsureChildControls();
                this._resizableControl.HandleOffsetY = value;
            }
        }

        [Category("Infolight")]
        public int MaximumHeight
        {
            get
            {
                EnsureChildControls();
                return this._resizableControl.MaximumHeight;
            }
            set
            {
                EnsureChildControls();
                this._resizableControl.MaximumHeight = value;
            }
        }

        [Category("Infolight")]
        public int MaximumWidth
        {
            get
            {
                EnsureChildControls();
                return this._resizableControl.MaximumWidth;
            }
            set
            {
                EnsureChildControls();
                this._resizableControl.MaximumWidth = value;
            }
        }

        [Category("Infolight")]
        public int MinimumHeight
        {
            get
            {
                EnsureChildControls();
                return this._resizableControl.MinimumHeight;
            }
            set
            {
                EnsureChildControls();
                this._resizableControl.MinimumHeight = value;
            }
        }

        [Category("Infolight")]
        public int MinimumWidth
        {
            get
            {
                EnsureChildControls();
                return this._resizableControl.MinimumWidth;
            }
            set
            {
                EnsureChildControls();
                this._resizableControl.MinimumWidth = value;
            }
        }

        [Category("Infolight")]
        [Editor(typeof(TextEditor), typeof(UITypeEditor))]
        public string OnClientResize
        {
            get
            {
                EnsureChildControls();
                return this._resizableControl.OnClientResize;
            }
            set
            {
                EnsureChildControls();
                this._resizableControl.OnClientResize = value;
            }
        }

        [Category("Infolight")]
        public string OnClientResizeBegin
        {
            get
            {
                EnsureChildControls();
                return this._resizableControl.OnClientResizeBegin;
            }
            set
            {
                EnsureChildControls();
                this._resizableControl.OnClientResizeBegin = value;
            }
        }

        [Category("Infolight")]
        public string OnClientResizing
        {
            get
            {
                EnsureChildControls();
                return this._resizableControl.OnClientResizing;
            }
            set
            {
                EnsureChildControls();
                this._resizableControl.OnClientResizing = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("ajaxresize_resizingResizableControl")]
        public string ResizableCssClass
        {
            get
            {
                EnsureChildControls();
                return this._resizableControl.ResizableCssClass;
            }
            set
            {
                EnsureChildControls();
                this._resizableControl.ResizableCssClass = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        public bool ResizeFont
        {
            get
            {
                object obj = this.ViewState["ResizeFont"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["ResizeFont"] = value;
            }
        }

        [Category("Infolight")]
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (!this.DesignMode)
            {
                _resizableControl = new ResizableControlExtender();
                _resizableControl.ID = "_resizableControl";
                Controls.Add(_resizableControl);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ClientScriptManager csm = Page.ClientScript;
            writer.AddStyleAttribute(HtmlTextWriterStyle.Overflow, "hidden");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "3px");
            base.Render(writer);
            //if (!this.DesignMode)
            //    _resizableControl.RenderControl(writer);
            
            //if (!csm.IsClientScriptBlockRegistered("clientScript"))
            //{
            //    writer.Write(this.ResizeScript());
            //    csm.RegisterClientScriptBlock(this.GetType(), "clientScript", "");
            //}
            //if (!csm.IsClientScriptIncludeRegistered("clientScript"))
            //{
            //    csm.RegisterClientScriptInclude(this.Page.GetType(), "clientScript", "methods.js");
            //}
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ClientScriptManager csm = Page.ClientScript;
            if (!csm.IsClientScriptIncludeRegistered("clientScript"))
            {
                csm.RegisterClientScriptInclude(this.Page.GetType(), "clientScript", "../methods.js");
            }
        }

        /*protected virtual string ResizeScript()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<script language=\"javascript\">");
            builder.AppendLine("var fontSize = 12;");
            builder.AppendLine("function OnClientResizeText(sender, eventArgs)");
            builder.AppendLine("{");
            if (this.ResizeFont)
            {
                builder.AppendLine("var e = sender.get_element();");
                builder.AppendLine("while((e.scrollWidth <= e.clientWidth) || (e.scrollHeight <= e.clientHeight))");
                builder.AppendLine("{");
                builder.AppendLine("e.style.fontSize = (fontSize++)+'pt';");
                builder.AppendLine("}");
                builder.AppendLine("var lastScrollWidth = -1;");
                builder.AppendLine("var lastScrollHeight = -1;");
                builder.AppendLine("while (((e.clientWidth < e.scrollWidth) || (e.clientHeight < e.scrollHeight)) && ((e.scrollWidth != lastScrollWidth) || (e.scrollHeight != lastScrollHeight)) && fontSize > 0)");
                builder.AppendLine("{");
                builder.AppendLine("lastScrollWidth = e.scrollWidth;");
                builder.AppendLine("lastScrollHeight = e.scrollHeight;");
                builder.AppendLine("e.style.fontSize = (fontSize--)+'pt';");
                builder.AppendLine("}");
            }
            builder.AppendLine("}");
            builder.AppendLine("</script>");

            return builder.ToString();
        }*/
    }

    public class TextEditor : UITypeEditor
    {
        [PermissionSet(SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [PermissionSet(SecurityAction.Demand)]
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance is AjaxResizableControl)
            {
                objName.Add("OnClientResizeText");
            }
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, objName.ToArray());
                string strValue = (string)value;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }
}
