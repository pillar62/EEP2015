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
    public partial class YearMonthEditorDialog : ModalForm
    {
        public YearMonthEditorDialog()
        {
            InitializeComponent();

            YearMonthBox = new JQYearMonthBox();
            propertyGridRefval.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridRefval.SelectedObject = YearMonthBox;
        }

        public JQYearMonthBox YearMonthBox { get; set; }

        public override object SelectedValue
        {
            get
            {
                return YearMonthBox.InfolightOptions;
            }
            set
            {
                YearMonthBox.LoadProperties((string)value);
            }
        }

    }
}
