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
    public partial class ComboBoxEditorDialog : ModalForm
    {
        public ComboBoxEditorDialog()
        {
            InitializeComponent();

            ComboBox = new JQComboBox();
            propertyGridRefval.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridRefval.SelectedObject = ComboBox;
        }

        public JQComboBox ComboBox { get; set; }

        public override object SelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(ComboBox.RemoteName) && !string.IsNullOrEmpty(ComboBox.DisplayMember) && !string.IsNullOrEmpty(ComboBox.ValueMember))
                {
                    return ComboBox.InfolightOptions;
                }
                else if (string.IsNullOrEmpty(ComboBox.RemoteName) && ComboBox.Items.Count > 0)
                {
                    return ComboBox.InfolightOptions;

                    //var columns = new List<string>();
                    //foreach (var column in ComboBox.Items)
                    //{
                    //    columns.Add(string.Format("{{value:'{0}',text:'{1}',selected:'{2}'}}"
                    //        , column.Value, column.Text, column.Selected.ToString().ToLower()));
                    //}

                    //return string.Format("items:[{0}]", string.Join(",", columns));
                }
                return string.Empty;
            }
            set
            {
                ComboBox.LoadProperties((string)value);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ComboBox.RemoteName) && ComboBox.Items.Count > 0)
            {

            }
            else if (string.IsNullOrEmpty(ComboBox.RemoteName) || string.IsNullOrEmpty(ComboBox.DisplayMember) || string.IsNullOrEmpty(ComboBox.ValueMember))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }
    }
}
