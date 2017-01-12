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
    public partial class AutoCompleteEditorDialog : ModalForm
    {
        public AutoCompleteEditorDialog()
        {
            InitializeComponent();

            AutoComplete = new JQAutoComplete();
            propertyGridRefval.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridRefval.SelectedObject = AutoComplete;
        }

        public JQAutoComplete AutoComplete { get; set; }

        public override object SelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(AutoComplete.RemoteName) && !string.IsNullOrEmpty(AutoComplete.ValueField) && !string.IsNullOrEmpty(AutoComplete.DataMember))
                {
                    return AutoComplete.InfolightOptions;
                }
                return string.Empty;
            }
            set
            {
                AutoComplete.LoadProperties((string)value);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AutoComplete.RemoteName) || string.IsNullOrEmpty(AutoComplete.DataMember) || string.IsNullOrEmpty(AutoComplete.ValueField))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }
    }
}
