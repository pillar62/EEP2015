using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EFBase.Design;

namespace EFServerTools.Design.EFCommandDesign
{
    public partial class EFParameterEditorDialog : ModalForm
    {
        public EFParameterEditorDialog()
        {
            InitializeComponent();
        }

        private void EFParameterEditorDialog_Load(object sender, EventArgs e)
        {
            comboBoxType.Items.Add(typeof(string));
            comboBoxType.Items.Add(typeof(int));
            comboBoxType.Items.Add(typeof(decimal));
            comboBoxType.Items.Add(typeof(double));
        }

        public override object SelectedValue
        {
            get
            {
                var type = (Type)comboBoxType.SelectedItem;
                return Convert.ChangeType(textBoxValue.Text, type);
            }
            set
            {
                if (value != null)
                {
                    textBoxValue.Text = value.ToString();
                    comboBoxType.SelectedItem = value.GetType();
                }
            }
        }

       
    }
}
