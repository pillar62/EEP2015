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
    public partial class ComboGridEditorDialog : ModalForm
    {
        public ComboGridEditorDialog()
        {
            InitializeComponent();

            ComboGrid = new JQComboGrid();
            ComboGrid.DesignFlag = true;
            propertyGridRefval.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridRefval.SelectedObject = ComboGrid;
        }

        public JQComboGrid ComboGrid { get; set; }

        public override object SelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(ComboGrid.RemoteName) && !string.IsNullOrEmpty(ComboGrid.ValueMember) && !string.IsNullOrEmpty(ComboGrid.DisplayMember))
                {
                    return ComboGrid.InfolightOptions;
                }
                return string.Empty;
            }
            set
            {
                ComboGrid.LoadProperties((string)value);
            }
        }

    }
}
