using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Srvtools
{
    [ToolboxBitmap(typeof(InfoMaskedTextBox), "Resources.InfoMaskedTextBox.ico")]
    public partial class InfoMaskedTextBox : MaskedTextBox, IWriteValue
    {
        public InfoMaskedTextBox()
        {
            InitializeComponent();
            this.KeyPress += delegate(object sender, KeyPressEventArgs e)
            {
                if (!this.ReadOnly)
                {
                    Binding TextBinding = this.DataBindings["Text"];
                    if (TextBinding != null)
                    {
                        InfoBindingSource bindingSource = TextBinding.DataSource as InfoBindingSource;
                        if (bindingSource != null)
                        {
                            if (!bindingSource.BeginEdit())
                            {
                                e.KeyChar = (char)0;
                                return;
                            }
                        }
                    }
                }
            };
        }

        public InfoMaskedTextBox(IContainer container):this()
        {
            container.Add(this);

            //InitializeComponent();
        }

        private bool _enterEnable;
        public bool EnterEnable
        {
            get { return _enterEnable; }
            set { _enterEnable = value; }
        }

        private void InfoMaskedTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && this.EnterEnable == true)
            {
                SendKeys.Send("{Tab}");
            }
        }

        #region IWriteValue Members

        public void WriteValue(object value)
        {
            this.Text = value.ToString();
            WriteValue();
        }

        public void WriteValue()
        {
            if (this.DataBindings["Text"] != null)
            {
                this.DataBindings["Text"].WriteValue();
            }
        }

        #endregion
    }
}
