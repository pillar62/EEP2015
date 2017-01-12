using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class EnabledControlsEditorDialog : Form
    {
        public List<string> EnabledControls = new List<string>();
        private BindingNavigator Navigator = null;

        public EnabledControlsEditorDialog(List<string> enabledControls, BindingNavigator navigator)
        {
            InitializeComponent();

            foreach (string ctrl in enabledControls)
            {
                EnabledControls.Add(ctrl);
            }

            Navigator = navigator;
        }

        private void EnabledControlsEditorDialog_Load(object sender, EventArgs e)
        {
            foreach (ToolStripItem item in Navigator.Items)
            {
                if (!(item is ToolStripSeparator)
                    && item.Name != null && item.Name.Trim() != "")
                {
                    if (EnabledControls.Contains(item.Name))
                    {
                        this.lbxEnabledControls.Items.Add(item.Name);
                    }
                    else if(!IsFlowNavigatorItem(item))
                    {
                        this.lbxDisabledControls.Items.Add(item.Name);
                    }
                }
            }
        }

        private bool IsFlowNavigatorItem(ToolStripItem item)
        {
            if (item.Name != "toolStripSubmitItem"
                && item.Name != "toolStripApproveItem"
                && item.Name != "toolStripReturnItem"
                && item.Name != "toolStripRejectItem"
                && item.Name != "toolStripNotifyItem"
                && item.Name != "toolStripFlowDeleteItem"
                && item.Name != "toolStripPlusItem"
                && item.Name != "toolStripPauseItem"
                && item.Name != "toolStripCommentItem")
            {
                return false;
            }
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            EnabledControls.Clear();
            foreach (object obj in this.lbxEnabledControls.Items)
            {
                EnabledControls.Add(obj.ToString());
            }
        }

        private void btnMoveAllFromEnToDis_Click(object sender, EventArgs e)
        {
            for (int index = this.lbxEnabledControls.Items.Count - 1; index >= 0; --index)
            {
                this.lbxDisabledControls.Items.Add(this.lbxEnabledControls.Items[index]);
                this.lbxEnabledControls.Items.RemoveAt(index);
            }
        }

        private void btnMoveAllFromDisToEn_Click(object sender, EventArgs e)
        {
            for (int index = this.lbxDisabledControls.Items.Count - 1; index >= 0; --index)
            {
                this.lbxEnabledControls.Items.Add(this.lbxDisabledControls.Items[index]);
                this.lbxDisabledControls.Items.RemoveAt(index);
            }
        }

        private void btnMoveFromEnToDis_Click(object sender, EventArgs e)
        {
            int index = this.lbxEnabledControls.SelectedIndex;
            if (index != -1)
            {
                this.lbxDisabledControls.Items.Add(this.lbxEnabledControls.Items[index]);
                this.lbxEnabledControls.Items.RemoveAt(index);

                if (index < this.lbxEnabledControls.Items.Count)
                {
                    this.lbxEnabledControls.SelectedIndex = index;
                }
                else
                {
                    this.lbxEnabledControls.SelectedIndex = this.lbxEnabledControls.Items.Count - 1;
                }

                this.lbxDisabledControls.SelectedIndex = this.lbxDisabledControls.Items.Count - 1;
            }
        }

        private void btnMoveFromDisToEn_Click(object sender, EventArgs e)
        {
            int index = this.lbxDisabledControls.SelectedIndex;
            if (index != -1)
            {
                this.lbxEnabledControls.Items.Add(this.lbxDisabledControls.Items[index]);
                this.lbxDisabledControls.Items.RemoveAt(index);

                if (index < this.lbxDisabledControls.Items.Count)
                {
                    this.lbxDisabledControls.SelectedIndex = index;
                }
                else
                {
                    this.lbxDisabledControls.SelectedIndex = this.lbxDisabledControls.Items.Count - 1;
                }
                this.lbxEnabledControls.SelectedIndex = this.lbxEnabledControls.Items.Count - 1;
            }
        }

        private void lbxEnabledControls_DoubleClick(object sender, EventArgs e)
        {
            this.btnMoveFromEnToDis.PerformClick();
        }

        private void lbxDisabledControls_DoubleClick(object sender, EventArgs e)
        {
            this.btnMoveFromDisToEn.PerformClick();
        }
    }
}