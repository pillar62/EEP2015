using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class frmNavigatorCopy : Form
    {

        private InfoBindingSource bindingSourceCopy;

        public InfoBindingSource BindingSourceCopy
        {
            get { return bindingSourceCopy; }
        }

        private DataSet dataSetDD;

        public DataSet DataSetDD
        {
            get 
            {
                if (dataSetDD == null)
                {
                    dataSetDD =  DBUtils.GetDataDictionary(BindingSourceCopy, false);
                }
                return dataSetDD;
            }
        }

        private DefaultValidate defaultValue;

        public DefaultValidate DefaultValue
        {
            get 
            {
                if (defaultValue == null)
                {
                    defaultValue = new DefaultValidate();
                    foreach (Component cp in BindingSourceCopy.Container.Components)
                    {
                        if (cp is DefaultValidate && (cp as DefaultValidate).BindingSource == BindingSourceCopy)
                        {
                            if ((cp as DefaultValidate).DefaultActive)
                            {
                                defaultValue = cp as DefaultValidate;
                            }
                            //defaultValue = cp as DefaultValidate;
                            break;
                        }
                    }
                }
                return defaultValue;
            }
        }

        public frmNavigatorCopy(InfoBindingSource ibs)
        {
            InitializeComponent();
            bindingSourceCopy = ibs;
            InitControls();
        }

        private object[] values;

        public object[] Values
        {
            get { return values; }
        }

        private void InitControls()
        {
            DataTable table = (BindingSourceCopy.List as DataView).Table;
            values = new object[table.PrimaryKey.Length];
            for (int i = 0; i < table.PrimaryKey.Length; i++)
            {
                Label label = new Label();
                label.BackColor = Color.Transparent;
                label.AutoSize = true;
                label.Text = GetCaption(table.PrimaryKey[i].ColumnName);
                label.Location = new Point(30, i * 30 + 23);
                TextBox textbox = new TextBox();
                textbox.Text = GetDefaultValue(table.PrimaryKey[i].ColumnName);
                textbox.Tag = i;
                textbox.Location = new Point(120, i * 30 + 20);
                this.splitContainer1.Panel1.Controls.Add(label);
                this.splitContainer1.Panel1.Controls.Add(textbox);
            }
            this.Height = table.PrimaryKey.Length * 30 + 100;
            
            String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoNavigator", "ButtonName");
            string[] buttons = message.Split(';');
            this.btnOK.Text = buttons[0];
            this.btnCancel.Text = buttons[1];
        }

        private string GetCaption(string columnname)
        {
            string caption = columnname;
            DataRow[] dr = DataSetDD.Tables[0].Select(string.Format("FIELD_NAME='{0}'", columnname));
            if (dr.Length > 0)
            {
                string value = dr[0][string.Format("Caption{0}", (int)CliUtils.fClientLang + 1)].ToString();
                if (value.Length == 0)
                {
                    value = dr[0]["Caption"].ToString();
                }
                if (value.Length > 0)
                {
                    caption = value;
                }
            }
            return caption;
        }

        private string GetDefaultValue(string columnname)
        {
            foreach (FieldItem item in DefaultValue.FieldItems)
            {
                if (string.Compare(item.FieldName, columnname, true) == 0)
                {
                    return CliUtils.GetValue(item.DefaultValue, DefaultValue.OwnerComp).ToString();
                }
            }
            return string.Empty;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DataTable table = (BindingSourceCopy.List as DataView).Table;
            foreach (Control ct in splitContainer1.Panel1.Controls)
            {
                if (ct is TextBox)
                {
                    TextBox textbox = ct as TextBox;
                    DataColumn column = table.PrimaryKey[(int)textbox.Tag];
                    if (textbox.Text.Trim().Length == 0)
                    {
                        string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "DefaultValidate", "msg_DefaultValidateCheckNull");
                        MessageBox.Show(this, string.Format(message, GetCaption(column.ColumnName)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                        return;
                    }
                    else
                    {
                        try
                        {
                            if (column.DataType != typeof(Guid))
                            {
                                Values[(int)textbox.Tag] = Convert.ChangeType(textbox.Text, column.DataType);
                            }
                            else
                            {
                                Guid id = new Guid(textbox.Text);
                            }
                        }
                        catch
                        {
                            MessageBox.Show(this, string.Format("Can not convert '{0}' to {1} type", textbox.Text, column.DataType.Name), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.DialogResult = DialogResult.None;
                            return;
                        }
                    }
                }
            }
            DataRow dr = table.Rows.Find(Values);
            if (dr != null)
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "DefaultValidate", "DeplicateWarning");
                MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }
    }
}