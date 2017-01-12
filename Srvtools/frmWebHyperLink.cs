using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Srvtools
{
    public partial class frmWebHyperLink : Form
    {
        public string selectedColumn;
        private ArrayList allColumn;
        string[] arrselectedcolumn;
        public frmWebHyperLink(string scolumn, ArrayList acolumn)
        {
            InitializeComponent();
            selectedColumn = scolumn;
            allColumn = acolumn;
            arrselectedcolumn = selectedColumn.Split(';');
            AddListBox();
        }

        private void AddListBox()
        {
            foreach (object str in allColumn)
            {
                lbAll.Items.Add(str);
            }
            if (selectedColumn != "")
            {
                foreach (string str in arrselectedcolumn)
                {
                    lbAll.Items.Remove(str);
                    lbSelected.Items.Add(str);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            foreach (object selItem in lbAll.SelectedItems)
            {
                lbSelected.Items.Add(selItem);
            }

            ArrayList libRemoveList = new ArrayList();
            for (int i = 0; i < lbAll.SelectedItems.Count; i++)
            {
                libRemoveList.Add(lbAll.SelectedItems[i]);
            }
            for (int j = 0; j < libRemoveList.Count; j++)
            {
                lbAll.Items.Remove(libRemoveList[j]);
            }
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            lbAll.Items.Clear();
            lbSelected.Items.Clear();
            foreach (object str in allColumn)
            {
                lbSelected.Items.Add(str);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            foreach (object selItem in lbSelected.SelectedItems)
            {
                lbAll.Items.Add(selItem);
            }

            ArrayList libRemoveList = new ArrayList();
            for (int i = 0; i < lbSelected.SelectedItems.Count; i++)
            {
                libRemoveList.Add(lbSelected.SelectedItems[i]);
            }
            for (int j = 0; j < libRemoveList.Count; j++)
            {
                lbSelected.Items.Remove(libRemoveList[j]);
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            lbAll.Items.Clear();
            lbSelected.Items.Clear();
            foreach (object str in allColumn)
            {
                lbAll.Items.Add(str);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            selectedColumn = "";
            foreach (object obj in lbSelected.Items)
            {
                selectedColumn += obj.ToString() + ";";
            }
            if (selectedColumn != "")
            {
                selectedColumn = selectedColumn.Substring(0, selectedColumn.LastIndexOf(';'));
            }
            this.Close();
        }
    }
}