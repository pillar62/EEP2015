using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JQClientTools.Editor
{
    public partial class CheckBoxEditorDialog : ModalForm
    {
        public CheckBoxEditorDialog()
        {
            InitializeComponent();

            CheckBox = new JQCheckBox();
            propertyGridRefval.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridRefval.SelectedObject = CheckBox;
        }

        public JQCheckBox CheckBox { get; set; }

        public override object SelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(CheckBox.On) && !string.IsNullOrEmpty(CheckBox.Off))
                {
                    return CheckBox.DataOptions;
                }
                return string.Empty;
            }
            set
            {
                CheckBox.LoadProperties((string)value);
            }
        }
    }
}
