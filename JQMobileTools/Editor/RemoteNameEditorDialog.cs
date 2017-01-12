using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using JQMobileTools;
using EFClientTools;


namespace JQMobileTools.Editor
{
    public partial class RemoteNameEditorDialog : ModalForm
    {
        public override object SelectedValue
        {
            get
            {
                if (lstDataModule.SelectedIndex != -1 && lstCommand.SelectedIndex != -1)
                {
                    return string.Format("{0}.{1}", lstDataModule.SelectedValue, lstCommand.SelectedValue);
                }
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty((string)value))
                {
                    var remoteNames = ((string)value).Split('.');
                    if (remoteNames.Length == 2)
                    {
                        AssemblyName = remoteNames[0];
                        CommandName = remoteNames[1];
                    }
                }
            }
        }

        public string AssemblyName { get; set; }

        public string CommandName { get; set; }

        public RemoteNameEditorDialog()
        {
            InitializeComponent();

            DesignClientUtility.ClientInfo.UseDataSet = true;
            var moduleNames = DesignClientUtility.Client.GetModuleNames(DesignClientUtility.ClientInfo);
            lstDataModule.DataSource = moduleNames;
            if (!string.IsNullOrEmpty(AssemblyName) && moduleNames.Contains(AssemblyName))
            {
                lstDataModule.SelectedItem = AssemblyName;
            }
        }

        private void lstDataModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDataModule.SelectedIndex != -1)
            {
                DesignClientUtility.ClientInfo.UseDataSet = true;
                lstCommand.DataSource = DesignClientUtility.Client.GetCommandNames(DesignClientUtility.ClientInfo, lstDataModule.SelectedValue.ToString());
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lstDataModule.SelectedIndex == -1 || lstCommand.SelectedIndex == -1)
            {
                DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }
    }
}
