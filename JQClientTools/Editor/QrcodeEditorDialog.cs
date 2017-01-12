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
    public partial class QrcodeEditorDialog : ModalForm
    {
        public QrcodeEditorDialog()
        {
            InitializeComponent();

            qrcode = new JQQrcode();
            propertyGridDateBox.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridDateBox.SelectedObject = qrcode;
        }

        public JQQrcode qrcode { get; set; }

        public override object SelectedValue
        {
            get
            {
                return qrcode.DataOptions;
            }
            set
            {
                qrcode.LoadProperties((string)value);
            }
        }
    }
}
