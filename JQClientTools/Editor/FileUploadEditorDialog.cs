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
    public partial class FileUploadEditorDialog : ModalForm
    {
        public FileUploadEditorDialog()
        {
            InitializeComponent();

            fileUpload = new JQFileUpload();
            propertyGridRefval.BrowsableAttributes = new AttributeCollection(new Attribute[] { new CategoryAttribute("Infolight") });
            propertyGridRefval.SelectedObject = fileUpload;
        }

        public JQFileUpload fileUpload { get; set; }

        public override object SelectedValue
        {
            get
            {
                return fileUpload.InfolightOptions;
            }
            set
            {
                fileUpload.LoadProperties((string)value);
            }
        }

    }
}
