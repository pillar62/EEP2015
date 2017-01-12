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
    public partial class NumberBoxEditorDialog : ModalForm
    {
        public NumberBoxEditorDialog()
        {
            InitializeComponent();
            NumberBox = new JQNumberBox();
            propertyGridRefval.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridRefval.SelectedObject = NumberBox;
        }

        public JQNumberBox NumberBox { get; set; }

        public override object SelectedValue
        {
            get
            {
                return NumberBox.DataOptions;
            }
            set
            {
                NumberBox.LoadProperties((string)value);
            }
        }
    }
}
