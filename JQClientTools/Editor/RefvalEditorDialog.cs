using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace JQClientTools.Editor
{
    public partial class RefvalEditorDialog : ModalForm
    {
        public RefvalEditorDialog(ITypeDescriptorContext context)
        {
            InitializeComponent();

            Refval = new JQRefval();
            Refval.DesignFlag = true;
            if (context.Instance is IJQProperty)
            {
                Refval.ParentObject = (context.Instance as IJQProperty).ParentProperty.Component;
            }

            propertyGridRefval.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridRefval.SelectedObject = Refval;
        }

        public JQRefval Refval { get; set; }

        public override object SelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(Refval.RemoteName) && !string.IsNullOrEmpty(Refval.ValueMember))
                {
                    return Refval.InfolightOptions;
                }
                return string.Empty;
            }
            set
            {
                Refval.LoadProperties((string)value);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Refval.RemoteName) || string.IsNullOrEmpty(Refval.ValueMember))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }
    }
}
