using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JQMobileTools.Editor
{
    public partial class JQDataFormEditorDialog : ModalForm
    {
        public JQDataFormEditorDialog(string typeName, ITypeDescriptorContext context)
        {
            InitializeComponent();
            TypeName = typeName;
            Context = context;
        }

        private ITypeDescriptorContext Context { get; set; }

        public string TypeName { get; set; }

        public JQControl Control { get; set; }

        public override object SelectedValue
        {
            get
            {
                return Control.Options;
            }
            set
            {
                this.Control = JQControl.CreateControl(TypeName, (string)value);
                this.Control.Context = Context;
                this.Text = string.Format("Edit {0}", this.Control.GetType().Name);
                propertyGridRefval.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
                propertyGridRefval.SelectedObject = Control;
            }
        }
    }
}
