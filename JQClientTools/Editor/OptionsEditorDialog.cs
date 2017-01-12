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
    public partial class OptionsEditorDialog : ModalForm
    {
        public OptionsEditorDialog()
        {
            InitializeComponent();

            Options = new JQOptions();
            propertyGridOptions.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridOptions.SelectedObject = Options;
        }

        public JQOptions Options { get; set; }

        public override object SelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(Options.RemoteName)  && !string.IsNullOrEmpty(Options.DataMember)
                    && !string.IsNullOrEmpty(Options.DisplayMember) && !string.IsNullOrEmpty(Options.ValueMember))
                {
                    return Options.InfolightOptions;
                }
                else if (string.IsNullOrEmpty(Options.RemoteName) && Options.Items.Count > 0)
                {
                    return Options.InfolightOptions;

                }
                return string.Empty;
            }
            set
            {
                Options.LoadProperties((string)value);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Options.RemoteName) && Options.Items.Count > 0)
            {

            }
            else if (string.IsNullOrEmpty(Options.RemoteName) || string.IsNullOrEmpty(Options.DataMember)
                || string.IsNullOrEmpty(Options.DisplayMember) || string.IsNullOrEmpty(Options.ValueMember))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }
    }
}
