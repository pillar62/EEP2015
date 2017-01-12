using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI.Design;

namespace AjaxTools
{
    public class ExtGridToolItem : InfoOwnerCollectionItem
    {
        string _buttonName = "";
        string _text = "";
        string _cssClass = "x-btn-text-icon details";
        string _iconUrl = "";
        string _handler = "";
        ExtGridSystemHandler _sysHandlerType = ExtGridSystemHandler.Refresh;
        ExtGridToolItemType _toolItemType = ExtGridToolItemType.Button;


        [Category("Button")]
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        public string ButtonName
        {
            get { return _buttonName; }
            set { _buttonName = value; }
        }

        [Category("Label(Button)")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        [Category("Global")]
        [DefaultValue(typeof(ExtGridToolItemType), "Button")]
        [NotifyParentProperty(true)]
        public ExtGridToolItemType ToolItemType
        {
            get { return _toolItemType; }
            set { _toolItemType = value; }
        }

        [Category("Button")]
        [DefaultValue(typeof(ExtGridSystemHandler), "Refresh")]
        [NotifyParentProperty(true)]
        public ExtGridSystemHandler SysHandlerType
        {
            get { return _sysHandlerType; }
            set { _sysHandlerType = value; }
        }

        [Category("Button")]
        [DefaultValue("x-btn-text-icon details")]
        [NotifyParentProperty(true)]
        public string CssClass
        {
            get { return _cssClass; }
            set { _cssClass = value; }
        }

        [Category("Button")]
        [DefaultValue("")]
        [Editor(typeof(UrlEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string IconUrl
        {
            get { return _iconUrl; }
            set { _iconUrl = value; }
        }

        [Category("Button")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string Handler
        {
            get { return _handler; }
            set { _handler = value; }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get
            {
                return getname();
            }
            set { _text = value; }
        }

        public override string ToString()
        {
            return getname();
        }

        string getname()
        {
            PropertyDescriptor propToolItemType = TypeDescriptor.GetProperties(this)["ToolItemType"];
            ExtGridToolItemType itemtype = (ExtGridToolItemType)propToolItemType.GetValue(this);
            if (itemtype == ExtGridToolItemType.Button)
            {
                if (!string.IsNullOrEmpty(_buttonName))
                    return _buttonName;
                else if (!string.IsNullOrEmpty(_text))
                    return _text;
                return "<button>";
            }
            else if (itemtype == ExtGridToolItemType.Label)
            {
                if (!string.IsNullOrEmpty(_text))
                    return _text;
                return "<text>";
            }
            else if (itemtype == ExtGridToolItemType.Separation)
            {
                return "<separation>";
            }
            else if (itemtype == ExtGridToolItemType.Fill)
            {
                return "<fill>";
            }
            return "toolitem";
        }
    }

}
