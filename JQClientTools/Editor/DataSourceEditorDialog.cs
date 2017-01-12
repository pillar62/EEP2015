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
    public partial class DataSourceEditorDialog : ModalForm
    {
        public DataSourceEditorDialog()
        {
            InitializeComponent();
        }

        public override object SelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(textBoxRemoteName.Text) && comboBoxDisplayMember.SelectedIndex != -1
                    && comboBoxValueMember.SelectedIndex != -1)
                {
                    var remoteNames = ((string)textBoxRemoteName.Text).Split('.');
                    if (remoteNames.Length == 2)
                    {
                        var commandName = remoteNames[1];
                        return string.Format("onBeforeLoad:comboBeforeLoad,mode:'remote',valueField:'{0}',textField:'{1}',url:'../handler/jqDataHandle.ashx?RemoteName={2}&TableName={3}'"
                            , comboBoxValueMember.SelectedValue, comboBoxDisplayMember.SelectedValue, textBoxRemoteName.Text, commandName);
                    }
                }
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty((string)value))
                {
                    var options = ((string)value).Split(',');
                    var url = string.Empty;
                    var textField = string.Empty;
                    var valueField = string.Empty;
                    foreach (var op in options)
                    {
                        var parts = op.Split(':');
                        if (parts.Length == 2)
                        {
                            var pname = parts[0].Trim();
                            var pvalue = parts[1].Trim('\'');
                            if (pname == "url")
                            { 
                                url = pvalue;
                            }
                            else if (pname == "textField")
                            {
                                textField = pvalue;
                            }
                            else if (pname == "valueField")
                            {
                                valueField = pvalue;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(url))
                    {
                        var remoteNameMatch = Regex.Match(url, @"(?<=RemoteName=)\w+\.\w+");
                        if (remoteNameMatch.Success)
                        {
                            textBoxRemoteName.Text = remoteNameMatch.Value;
                            var remoteNames = remoteNameMatch.Value.Split('.');
                            var assemblyName = remoteNames[0];
                            var commandName = remoteNames[1];

                            var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                            clientInfo.UseDataSet = true;
                            var fields = EFClientTools.DesignClientUtility.Client.GetEntityFields(clientInfo, assemblyName, commandName, null);
                            comboBoxDisplayMember.DataSource = fields.ToList();
                            comboBoxValueMember.DataSource = fields.ToList();
                            if (fields.Contains(textField))
                            {
                                comboBoxDisplayMember.SelectedIndex = fields.IndexOf(textField);
                            }
                            if (fields.Contains(valueField))
                            {
                                comboBoxValueMember.SelectedIndex = fields.IndexOf(valueField);
                            }
                        }
                    }
                }
            }
        }

        private void buttonRemoteName_Click(object sender, EventArgs e)
        {
            var dialog = new RemoteNameEditorDialog();
            dialog.SelectedValue = textBoxRemoteName.Text;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxRemoteName.Text = (string)dialog.SelectedValue;
                var remoteNames = textBoxRemoteName.Text.Split('.');
                if (remoteNames.Length == 2)
                {
                    var assemblyName = remoteNames[0];
                    var commandName = remoteNames[1];

                    var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                    clientInfo.UseDataSet = true;
                    var fields = EFClientTools.DesignClientUtility.Client.GetEntityFields(clientInfo, assemblyName, commandName, null);
                    comboBoxDisplayMember.DataSource = fields.ToList();
                    comboBoxValueMember.DataSource = fields.ToList();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxRemoteName.Text) || comboBoxDisplayMember.SelectedIndex == -1 
                || comboBoxValueMember.SelectedIndex == -1)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }

      
    }
}
