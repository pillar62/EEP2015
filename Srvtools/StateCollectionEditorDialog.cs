using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class StateCollectionEditorDialog : Form
    {
        public StateCollection Collection = null;

        public StateCollectionEditorDialog(StateCollection collection)
        {
            InitializeComponent();

            Collection = new StateCollection(collection.Owner, collection.ItemType);
            foreach (StateItem stateItem in collection)
            {
                if (stateItem.StateText == "Initial"
                    || stateItem.StateText == "Browsed"
                    || stateItem.StateText == "Inserting"
                    || stateItem.StateText == "Editing"
                    || stateItem.StateText == "Applying"
                    || stateItem.StateText == "Changing"
                    || stateItem.StateText == "Querying"
                    || stateItem.StateText == "Printing")
                {
                    foreach (StateItem si in Collection)
                    {
                        if (si.StateText == stateItem.StateText)
                        {
                            si.Collection = Collection;
                            foreach (string ctrlName in stateItem.EnabledControls)
                            {
                                si.EnabledControls.Add(ctrlName);
                            }
                            si.Name = stateItem.Name;
                            si.Description = stateItem.Description;
                            si.EnabledControlsEdited = stateItem.EnabledControlsEdited;
                            break;
                        }
                    }
                }
                else
                {
                    StateItem si = new StateItem();
                    Collection.Add(si);

                    si.Collection = Collection;
                    foreach (string ctrlName in stateItem.EnabledControls)
                    {
                        si.EnabledControls.Add(ctrlName);
                    }
                    si.Name = stateItem.Name;
                    si.StateText = stateItem.StateText;
                    si.Description = stateItem.Description;
                    si.EnabledControlsEdited = stateItem.EnabledControlsEdited;
                }
            }
        }

        private void StateCollectionEditorDialog_Load(object sender, EventArgs e)
        {
            RefreshStateItem();
            this.lbxStates.SelectedIndex = 0;
        }

        private void RefreshStateItem()
        {
            lbxStates.Items.Clear();
            foreach (StateItem stateItem in Collection)
            {
                lbxStates.Items.Add(stateItem.StateText);
            }
        }

        private void lbxStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lbxStates.SelectedIndex;
            if (index != -1)
            {
                this.pgStateItem.SelectedObject = this.Collection.GetItem(index);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = this.lbxStates.SelectedIndex;
            if (index != -1)
            {
                if (this.lbxStates.Items[index].ToString() == "Initial"
                        || this.lbxStates.Items[index].ToString() == "Browsed"
                        || this.lbxStates.Items[index].ToString() == "Inserting"
                        || this.lbxStates.Items[index].ToString() == "Editing"
                        || this.lbxStates.Items[index].ToString() == "Applying"
                        || this.lbxStates.Items[index].ToString() == "Changing"
                        || this.lbxStates.Items[index].ToString() == "Querying"
                        || this.lbxStates.Items[index].ToString() == "Printing")
                {
                    MessageBox.Show("Default StateItem can not be removed");
                    return;
                }
                this.lbxStates.Items.RemoveAt(index);
                Collection.RemoveAt(index);
                if (index < this.lbxStates.Items.Count)
                {
                    this.lbxStates.SelectedIndex = index;
                }
                else
                {
                    this.lbxStates.SelectedIndex = this.lbxStates.Items.Count - 1;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            StateItem stateItem = new StateItem();

            // Determine StateText
            bool stateTextExists = true;
            int loopCounter = 0;
            while (stateTextExists)
            {
                loopCounter++;
                stateTextExists = false;
                foreach (StateItem si in Collection)
                {
                    if (si.StateText == "State" + loopCounter.ToString())
                    {
                        stateTextExists = true;
                        break;
                    }
                }
            }
            stateItem.StateText = "State" + loopCounter.ToString();
            stateItem.Description = stateItem.StateText;

            Collection.Add(stateItem);
            this.lbxStates.Items.Add(stateItem.StateText);
            this.lbxStates.SelectedIndex = this.lbxStates.Items.Count - 1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}