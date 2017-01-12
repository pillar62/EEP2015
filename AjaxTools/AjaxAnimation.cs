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
using System.Drawing;
using Srvtools;

namespace AjaxTools
{
    public class AjaxAnimation : AnimationExtender
    {
        [Category("InfoLight")]
        [DefaultValue(typeof(AnimationAction), "Open")]
        public AnimationAction Action
        {
            get
            {
                object obj = this.ViewState["Action"];
                if (obj != null)
                    return (AnimationAction)obj;
                return AnimationAction.Open;
            }
            set
            {
                this.ViewState["Action"] = value;
            }
        }

        [Category("InfoLight")]
        [Editor(typeof(PanelEditor), typeof(UITypeEditor))]
        public string ActionContainer
        {
            get
            {
                object obj = this.ViewState["ActionContainer"];
                if (obj != null)
                    return (string)obj;
                return "";
            }
            set
            {
                this.ViewState["ActionContainer"] = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(16)]
        public int ShowFontSize
        {
            get
            {
                object obj = this.ViewState["ShowFontSize"];
                if (obj != null)
                    return (int)obj;
                return 16;
            }
            set
            {
                this.ViewState["ShowFontSize"] = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(typeof(Unit), "100px")]
        public Unit ShowPanelWidth
        {
            get
            {
                object obj = this.ViewState["ShowPanelWidth"];
                if (obj != null)
                    return (Unit)obj;
                return Unit.Pixel(100);
            }
            set
            {
                this.ViewState["ShowPanelWidth"] = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(typeof(Unit), "100px")]
        public Unit ShowPanelHeight
        {
            get
            {
                object obj = this.ViewState["ShowPanelHeight"];
                if (obj != null)
                    return (Unit)obj;
                return Unit.Pixel(100);
            }
            set
            {
                this.ViewState["ShowPanelHeight"] = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(typeof(Color), "Maroon")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color FadeStartFontColor
        {
            get
            {
                object obj = this.ViewState["FadeStartFontColor"];
                if (obj != null)
                    return (Color)obj;
                return Color.Maroon;
            }
            set
            {
                this.ViewState["FadeStartFontColor"] = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(typeof(Color), "#0000FF")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color FadeEndFontColor
        {
            get
            {
                object obj = this.ViewState["FadeEndFontColor"];
                if (obj != null)
                    return (Color)obj;
                return ColorTranslator.FromHtml("#0000FF");
            }
            set
            {
                this.ViewState["FadeEndFontColor"] = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(typeof(Color), "Maroon")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color FadeStartBorderColor
        {
            get
            {
                object obj = this.ViewState["FadeStartBorderColor"];
                if (obj != null)
                    return (Color)obj;
                return Color.Maroon;
            }
            set
            {
                this.ViewState["FadeStartBorderColor"] = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(typeof(Color), "Gray")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color FadeEndBorderColor
        {
            get
            {
                object obj = this.ViewState["FadeEndBorderColor"];
                if (obj != null)
                    return (Color)obj;
                return Color.Gray;
            }
            set
            {
                this.ViewState["FadeEndBorderColor"] = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(typeof(Color), "#FFFFFF")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color FadeStartBackColor
        {
            get
            {
                object obj = this.ViewState["FadeStartBackColor"];
                if (obj != null)
                    return (Color)obj;
                return ColorTranslator.FromHtml("#FFFFFF");
            }
            set
            {
                this.ViewState["FadeStartBackColor"] = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(typeof(Color), "#E0E0E0")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color FadeEndBackColor
        {
            get
            {
                object obj = this.ViewState["FadeEndBackColor"];
                if (obj != null)
                    return (Color)obj;
                return ColorTranslator.FromHtml("#E0E0E0");
            }
            set
            {
                this.ViewState["FadeEndBackColor"] = value;
            }
        }

        [Category("InfoLight")]
        [DefaultValue(0.5)]
        public float Fadeinterval
        {
            get
            {
                object obj = this.ViewState["Fadeinterval"];
                if (obj != null)
                    return (float)obj;
                return 0.5f;
            }
            set
            {
                this.ViewState["Fadeinterval"] = value;
            }
        }

        public virtual string getAnimations(AnimationAction action)
        {
            StringBuilder builder = new StringBuilder();
            switch (action)
            {
                case AnimationAction.Open:
                    builder.AppendLine("<OnLoad>");
                    builder.AppendLine("<OpacityAction AnimationTarget=\"" + this.ActionContainer + "\" Opacity=\"0\" />");
                    builder.AppendLine("</OnLoad>");
                    builder.AppendLine("<OnClick>");
                    builder.AppendLine("<Sequence>");
                    builder.AppendLine("<ScriptAction Script=\"var target = $get('" + this.TargetControl.ClientID + "');"
                        + "var body = $get('" + this.ActionContainer + "');"
                        + "var location = Sys.UI.DomElement.getLocation(target);"
                        + "body.style.position = 'absolute';"
                        + "body.style.top = location.y;"
                        + "body.style.left = location.x + target.clientWidth + 5;\" />");
                    builder.AppendLine("<StyleAction AnimationTarget=\"" + this.ActionContainer + "\" Attribute=\"display\" Value=\"block\"/>");
                    builder.AppendLine("<FadeIn AnimationTarget=\"" + this.ActionContainer + "\" Duration=\".2\"/>");
                    builder.AppendLine("<Parallel Duration=\"" + (this.Fadeinterval).ToString() + "\">");
                    builder.AppendLine("<Color AnimationTarget=\"" + this.ActionContainer + "\" StartValue=\"" + 
                        GloFix.ToHtmlColor(this.FadeStartFontColor) + "\" EndValue=\"" + 
                        GloFix.ToHtmlColor(this.FadeEndFontColor) + "\" Property=\"style\" PropertyKey=\"color\" />");
                    builder.AppendLine("<Color AnimationTarget=\"" + this.ActionContainer + "\" StartValue=\"" + 
                        GloFix.ToHtmlColor(this.FadeStartBorderColor) + "\" EndValue=\"" + 
                        GloFix.ToHtmlColor(this.FadeEndBorderColor) + "\" Property=\"style\" PropertyKey=\"borderColor\" />");
                    builder.AppendLine("<Color AnimationTarget=\"" + this.ActionContainer + "\" StartValue=\"" + 
                        GloFix.ToHtmlColor(this.FadeStartBackColor) + "\" EndValue=\"" + 
                        GloFix.ToHtmlColor(this.FadeEndBackColor) + "\" Property=\"style\" PropertyKey=\"backgroundColor\" />");
                    builder.AppendLine("</Parallel>");
                    builder.AppendLine("</Sequence>");
                    builder.AppendLine("</OnClick>");
                    break;
                case AnimationAction.Close:
                    builder.AppendLine("<OnClick>");
                    builder.AppendLine("<Sequence>");
                    builder.AppendLine("<Parallel AnimationTarget=\"" + this.ActionContainer + "\" Duration=\"" + this.Fadeinterval.ToString() + "\" Fps=\"15\">");
                    builder.AppendLine("<Color AnimationTarget=\"" + this.ActionContainer + "\" StartValue=\"" +
    GloFix.ToHtmlColor(this.FadeStartFontColor) + "\" EndValue=\"" +
    GloFix.ToHtmlColor(this.FadeEndFontColor) + "\" Property=\"style\" PropertyKey=\"color\" />");
                    builder.AppendLine("<Color AnimationTarget=\"" + this.ActionContainer + "\" StartValue=\"" +
                        GloFix.ToHtmlColor(this.FadeStartBorderColor) + "\" EndValue=\"" +
                        GloFix.ToHtmlColor(this.FadeEndBorderColor) + "\" Property=\"style\" PropertyKey=\"borderColor\" />");
                    builder.AppendLine("<Color AnimationTarget=\"" + this.ActionContainer + "\" StartValue=\"" +
                        GloFix.ToHtmlColor(this.FadeStartBackColor) + "\" EndValue=\"" +
                        GloFix.ToHtmlColor(this.FadeEndBackColor) + "\" Property=\"style\" PropertyKey=\"backgroundColor\" />");
                    builder.AppendLine("<Scale ScaleFactor=\"0.05\" Center=\"true\" ScaleFont=\"true\" FontUnit=\"px\" />");
                    builder.AppendLine("<FadeOut />");
                    builder.AppendLine("</Parallel>");
                    builder.AppendLine("<StyleAction AnimationTarget=\"" + this.ActionContainer + "\" Attribute=\"display\" Value=\"none\"/>");
                    builder.AppendLine("<StyleAction AnimationTarget=\"" + this.ActionContainer + "\" Attribute=\"width\" Value=\"" + this.ShowPanelWidth.Value.ToString() + "px\"/>");
                    builder.AppendLine("<StyleAction AnimationTarget=\"" + this.ActionContainer + "\" Attribute=\"height\" Value=\"" + this.ShowPanelHeight.Value.ToString() + "px\"/>");
                    builder.AppendLine("<StyleAction AnimationTarget=\"" + this.ActionContainer + "\" Attribute=\"fontSize\" Value=\"" + this.ShowFontSize.ToString() + "px\"/>");
                    builder.AppendLine("</Sequence>");
                    builder.AppendLine("</OnClick>");
                    //builder.AppendLine("<OnMouseOver>");
                    //builder.AppendLine("<Color Duration=\".2\" StartValue=\"#FFFFFF\" EndValue=\"#FF0000\" Property=\"style\" PropertyKey=\"color\" />");
                    //builder.AppendLine("</OnMouseOver>");
                    //builder.AppendLine("<OnMouseOut>");
                    //builder.AppendLine("<Color Duration=\".2\" EndValue=\"#FFFFFF\" StartValue=\"#FF0000\" Property=\"style\" PropertyKey=\"color\" /> ");
                    //builder.AppendLine("</OnMouseOut>");
                    break;
            }

            return builder.ToString();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.Animations = getAnimations(this.Action);
            base.Render(writer);
        }
    }

    public class PanelEditor : UITypeEditor
    {
        public PanelEditor()
            : base()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance != null)
            {
                ControlCollection ctrlList = ((Control)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is Panel)
                    {
                        objName.Add(ctrl.ID);
                    }
                    else if (ctrl is AjaxDragPanel)
                    {
                        objName.Add(ctrl.ID + "_containerPanel");
                    }
                }
            }

            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, objName.ToArray());
                string strValue = (string)value;
                if (selector.Execute(ref strValue))
                    value = strValue;
            }
            return value;
        }
    }
}
