using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using EFClientTools.Common;

namespace EFClientTools.Editor
{
    public partial class RemoteNameEditorDialog : EditorDialog
    {
        private string _remoteName;
        private string[] list = null;

        private string returnClassName;

        public string ReturnClassName
        {
            get { return returnClassName; }
            set { returnClassName = value; }
        }

        private string selectedCommandName;

        public string SelectedCommandName
        {
            get { return selectedCommandName; }
            set { selectedCommandName = value; }
        }

        private string selectedAssemblyName;

        public string SelectedAssemblyName
        {
            get { return selectedAssemblyName; }
            set { selectedAssemblyName = value; }
        }

        private string entitySetName;

        public string EntitySetName
        {
            get { return entitySetName; }
            set { entitySetName = value; }
        }

        public RemoteNameEditorDialog()
        {
            InitializeComponent();
        }
        
        public RemoteNameEditorDialog(string remoteName): this()
        {
            _remoteName = remoteName;
            if (!string.IsNullOrWhiteSpace(_remoteName))
            {
                list = _remoteName.Split('.');
            }
        }

        private void RemoteNameEditorDialog_Load(object sender, EventArgs e)
        {
            lstDataModule.DataSource = DesignClientUtility.GetModuleNames();
            if (!string.IsNullOrWhiteSpace(_remoteName))
            {
                if (list.Length != 0)
                {
                    lstDataModule.SelectedItem = list[0];
                    lstEFCommand.SelectedItem = list[1];
                }
            }
        }

        private void lstDataModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDataModule.SelectedIndex != -1)
            {
                lstEFCommand.DataSource = DesignClientUtility.GetCommandNames(lstDataModule.SelectedValue.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lstDataModule.SelectedIndex != -1 && lstEFCommand.SelectedIndex != -1)
            {
                this.ReturnValue = string.Format("{0}.{1}", lstDataModule.SelectedValue, lstEFCommand.SelectedValue);
                this.SelectedAssemblyName = lstDataModule.SelectedValue.ToString();
                this.SelectedCommandName = lstEFCommand.SelectedValue.ToString();
                this.ReturnClassName = DesignClientUtility.Client.GetObjectClassName(DesignClientUtility.ClientInfo, lstDataModule.SelectedValue.ToString(), lstEFCommand.SelectedValue.ToString(), string.Empty);
                this.ReturnClassName = EntityProvider.GetClientEntityClassName(this.SelectedAssemblyName, this.ReturnClassName);
                List<String> entitySetNames = DesignClientUtility.Client.GetEntitySetNames(DesignClientUtility.ClientInfo, lstDataModule.SelectedValue.ToString(), lstEFCommand.SelectedValue.ToString(), this.ReturnClassName);
                if (entitySetNames != null && entitySetNames.Count > 0)
                    this.EntitySetName = entitySetNames[0];
            }
        }

    }
}
