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
    public partial class TextAreaEditorDialog : ModalForm
    {
        public TextAreaEditorDialog()
        {
            InitializeComponent();

            Textarea = new JQTextArea();
            propertyGridRefval.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridRefval.SelectedObject = Textarea;
        }

        public JQTextArea Textarea { get; set; }

        public override object SelectedValue
        {
            get
            {
                return Textarea.InfolightOptions;
            }
            set
            {
                Textarea.LoadProperties((string)value);
            }
        }
    }
}
