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
    public partial class RelationEditorDialog : ModalForm
    {
        public RelationEditorDialog()
        {
            InitializeComponent();

            Relation = new JQRelation();
            propertyGridRefval.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridRefval.SelectedObject = Relation;
        }

        public JQRelation Relation { get; set; }

        public override object SelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(Relation.RemoteName))
                {
                    return Relation.InfolightOptions;
                }
                return string.Empty;
            }
            set
            {
                Relation.LoadProperties((string)value);
            }
        }
    }
}
