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
    public partial class TextBoxEditorDialog : ModalForm
    {
        public TextBoxEditorDialog()
        {
            InitializeComponent();

            text = new JQTextBox();
            propertyGridRefval.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridRefval.SelectedObject = text;
        }
        public override object SelectedValue
        {
            get
            {
                return text.InfolightOptions;
            }
            set
            {
                text.LoadProperties((string)value);
            }
        }
        public JQTextBox text { get; set; }

    }
}
