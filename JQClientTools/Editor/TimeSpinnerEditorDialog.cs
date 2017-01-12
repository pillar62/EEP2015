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
    public partial class TimeSpinnerEditorDialog : ModalForm
    {
        public TimeSpinnerEditorDialog()
        {
            InitializeComponent();

            TimeSpinner = new JQTimeSpinner();
            propertyGridDateBox.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridDateBox.SelectedObject = TimeSpinner;
        }

        public JQTimeSpinner TimeSpinner { get; set; }

        public override object SelectedValue
        {
            get
            {
                return TimeSpinner.DataOptions;
            }
            set
            {
                TimeSpinner.LoadProperties((string)value);
            }
        }
    }
}
