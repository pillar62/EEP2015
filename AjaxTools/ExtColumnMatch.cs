using System.ComponentModel;
using System.Drawing.Design;

namespace AjaxTools
{
    public class ExtColumnMatch : InfoOwnerCollectionItem
    {
        string _destField = "";
        string _srcField = "";

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string DestField
        {
            get { return _destField; }
            set { _destField = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string SrcField
        {
            get { return _srcField; }
            set { _srcField = value; }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return _destField; }
            set { _destField = value; }
        }

        public override string ToString()
        {
            return _destField;
        }
    }

    public class ExtRefButtonColumnMatch : InfoOwnerCollectionItem
    {
        string _destField = "";
        string _srcField = "";
        string _srcControlId = "";
        string _srcControlValueProp = "Text";

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string DestField
        {
            get { return _destField; }
            set { _destField = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string SrcField
        {
            get { return _srcField; }
            set { _srcField = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string SrcControlId
        {
            get { return _srcControlId; }
            set { _srcControlId = value; }
        }

        [Category("Infolight")]
        [DefaultValue("Text")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string SrcControlValueProperty
        {
            get { return _srcControlValueProp; }
            set { _srcControlValueProp = value; }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return _destField; }
            set { _destField = value; }
        }

        public override string ToString()
        {
            return _destField;
        }
    }
}
