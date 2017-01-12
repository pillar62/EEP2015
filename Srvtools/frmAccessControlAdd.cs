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
    public partial class frmAccessControlAdd : Form
    {
        public ArrayList SelColbList = new ArrayList();
        
        public frmAccessControlAdd()
        {
            InitializeComponent();
        }

        private ArrayList listControlName = new ArrayList();
        private ArrayList listDescription = new ArrayList();
        public frmAccessControlAdd(ArrayList list, ArrayList colList, ArrayList listD)
        {
            InitializeComponent();
            listControlName = list;
            SelColbList = colList;
            listDescription = listD;
        }

        private bool bHasLoaded = false;
        private void clstControlName_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (bHasLoaded)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    this.SelColbList.Add(this.clstControlName.SelectedItem.ToString());
                }
                else
                {
                    this.SelColbList.Remove(this.clstControlName.SelectedItem.ToString());
                }
            }
        }

        private void frmAccessControlAdd_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < listControlName.Count; i++)
            {
                clstControlName.Items.Add(listControlName[i] + ":" + listDescription[i]);
            }

            for (int j = 0; j < SelColbList.Count; j++)
            {
                if (this.clstControlName.Items.Contains(SelColbList[j]))
                {
                    int m = this.clstControlName.Items.IndexOf(SelColbList[j]);
                    this.clstControlName.SetItemChecked(m, true);
                }
            }

            bHasLoaded = true;    
        }
    }
}