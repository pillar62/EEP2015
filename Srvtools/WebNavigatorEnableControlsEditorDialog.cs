using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class WebNavigatorEnableControlsEditorDialog : Form
    {
        public WebNavigatorEnableControlsEditorDialog(string enableControls, WebNavigator navigator)
        {
            InitializeComponent();
            EnableControls = enableControls;
            Navigator = navigator;
        }

        public string EnableControls;
        private WebNavigator Navigator = null;

        private void btnMoveAllFromEnableToDisenable_Click(object sender, EventArgs e)
        {
            for (int index = this.lbxEnableControls.Items.Count - 1; index >= 0; --index)
            {
                this.lbxDisenableControls.Items.Add(this.lbxEnableControls.Items[index]);
                this.lbxEnableControls.Items.RemoveAt(index);
            }
        }

        private void btnMoveFromEnableToDisenable_Click(object sender, EventArgs e)
        {
            int index = this.lbxEnableControls.SelectedIndex;
            if (index != -1)
            {
                this.lbxDisenableControls.Items.Add(this.lbxEnableControls.Items[index]);
                this.lbxEnableControls.Items.RemoveAt(index);

                if (index < this.lbxEnableControls.Items.Count)
                {
                    this.lbxEnableControls.SelectedIndex = index;
                }
                else
                {
                    this.lbxEnableControls.SelectedIndex = this.lbxEnableControls.Items.Count - 1;
                }

                this.lbxDisenableControls.SelectedIndex = this.lbxDisenableControls.Items.Count - 1;
            }
        }

        private void btnMoveFromDisenableToEnable_Click(object sender, EventArgs e)
        {
            int index = this.lbxDisenableControls.SelectedIndex;
            if (index != -1)
            {
                this.lbxEnableControls.Items.Add(this.lbxDisenableControls.Items[index]);
                this.lbxDisenableControls.Items.RemoveAt(index);

                if (index < this.lbxDisenableControls.Items.Count)
                {
                    this.lbxDisenableControls.SelectedIndex = index;
                }
                else
                {
                    this.lbxDisenableControls.SelectedIndex = this.lbxDisenableControls.Items.Count - 1;
                }
                this.lbxEnableControls.SelectedIndex = this.lbxEnableControls.Items.Count - 1;
            }
        }

        private void btnMoveAllFromDisenableToEnable_Click(object sender, EventArgs e)
        {
            for (int index = this.lbxDisenableControls.Items.Count - 1; index >= 0; --index)
            {
                this.lbxEnableControls.Items.Add(this.lbxDisenableControls.Items[index]);
                this.lbxDisenableControls.Items.RemoveAt(index);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            EnableControls = "";
            foreach (object obj in this.lbxEnableControls.Items)
            {
                EnableControls += obj.ToString() + ";";
            }
            EnableControls = EnableControls.Substring(0, EnableControls.LastIndexOf(';'));
        }

        private void WebNavigatorEnableControlsEditorDialog_Load(object sender, EventArgs e)
        {
            foreach (ControlItem item in Navigator.NavControls)
            {
                if (item.Name != null && item.Name.Trim() != ""
                    && !IsFlowNavigatorItem(item))
                {
                    if (containsItem(item.Name))
                    {
                        this.lbxEnableControls.Items.Add(item.Name);
                    }
                    else
                    {
                        this.lbxDisenableControls.Items.Add(item.Name);
                    }
                }
            }
        }

        private bool containsItem(string itemName)
        {
            if (EnableControls != null && EnableControls != "")
            {
                string[] vcs = EnableControls.Split(';');
                foreach (string vc in vcs)
                {
                    if (vc == itemName)
                        return true;
                }
            }
            return false;
        }

        private bool IsFlowNavigatorItem(ControlItem item)
        {
            if (item.Name != "Submit"
                && item.Name != "Approve"
                && item.Name != "Return"
                && item.Name != "Reject"
                && item.Name != "Notify"
                && item.Name != "FlowDelete"
                && item.Name != "Plus"
                && item.Name != "Pause"
                && item.Name != "Comment")
            {
                return false;
            }
            return true;
        }
    }
}