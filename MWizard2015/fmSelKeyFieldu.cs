using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MWizard2015
{
    public partial class fmSelKeyField : Form
    {
        private TDatasetItem FMasterItem = null;
        private TDatasetItem FDetailItem = null;

        public fmSelKeyField()
        {
            InitializeComponent();
            lbRelationFields.Items.Clear();
        }

        private void DisplayField(TDatasetItem DataSetItem, ListBox aListBox)
        {
            aListBox.Items.Clear();
            foreach (TFieldAttrItem FAI in DataSetItem.FieldAttrItems)
            {
                aListBox.Items.Add(FAI.DataField);
            }
        }

        public void ShowSelKeyField(TDatasetItem Master, TDatasetItem Detail)
        {
            FMasterItem = Master;
            FDetailItem = Detail;
            DisplayField(Master, lbMasterFields);
            DisplayField(Detail, lbDetailFields);
            foreach (TFieldAttrItem aItem in Detail.FieldAttrItems)
            {
                if (aItem.ParentRelationField != null && aItem.ParentRelationField != "")
                    lbRelationFields.Items.Add(aItem.ParentRelationField + "=" + aItem.DataField);
            }

            ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lbMasterFields.SelectedItem == null)
            {
                MessageBox.Show("Please select a master field !!");
                return;
            }
            if (lbDetailFields.SelectedItem == null)
            {
                MessageBox.Show("Please select a detail field !!");
                return;
            }
            String RelationField = (String)lbMasterFields.SelectedItem + "=" + (String)lbDetailFields.SelectedItem;
            if (lbRelationFields.Items.IndexOf(RelationField) > -1)
            {
                MessageBox.Show("Relation field already exists !!");
                return;
            }
            lbRelationFields.Items.Add(RelationField);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbRelationFields.SelectedItem == null)
                return;
            String DetailField = (String)lbRelationFields.SelectedItem;
            String MasterField = WzdUtils.GetToken(ref DetailField, new char[] { '=' });
            TFieldAttrItem FieldItem = FDetailItem.FieldAttrItems.FindItem(DetailField);
            FieldItem.ParentRelationField = null;
            lbRelationFields.Items.Remove((String)lbRelationFields.SelectedItem);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            foreach (String S in lbRelationFields.Items)
            {
                String DetailField = S;
                String MasterField = WzdUtils.GetToken(ref DetailField, new char[] { '=' });
                TFieldAttrItem DetailFieldItem = FDetailItem.FieldAttrItems.FindItem(DetailField);
                DetailFieldItem.ParentRelationField = MasterField;
                TFieldAttrItem MasterFieldItem = FMasterItem.FieldAttrItems.FindItem(MasterField);
            }
            DialogResult = DialogResult.OK;
        }
    }
}