using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace MWizard2015
{
    public partial class fmSelectProvider : Form
    {
        public fmSelectProvider()
        {
            InitializeComponent();
        }

        public Boolean ShowSelectProviderForm(List<InfoDataSet> RootList, ref String ProviderName)
        {
            if (!Init(RootList))
                return false;

            Boolean Result = ShowDialog() == DialogResult.OK;
            if (Result)
                ProviderName = tvProviderTree.SelectedNode.Text;
            return Result;
        }

        private bool Init(List<InfoDataSet> RootList)
        {
            tvProviderTree.Nodes.Clear();
            foreach (InfoDataSet aDataSet in RootList)
            {
                if (aDataSet.RealDataSet.Tables.Count == 0)
                {
                    MessageBox.Show("Please select provider name first.");
                    return false;
                }
                TreeNode RootNode = tvProviderTree.Nodes.Add(aDataSet.RealDataSet.Tables[0].TableName);
                GenRelationBindingSource(aDataSet.RealDataSet.Relations, RootNode);
                RootNode.ExpandAll();
            }
            return true;
            //foreach (TreeNode node in tvProviderTree.Nodes)
            //{
            //    node.Expand();
            //}
        }

        private void GenRelationBindingSource(DataRelationCollection Relations, TreeNode ParentNode)
        {
            foreach (DataRelation Relation in Relations)
            {
                TreeNode ChildNode = null;
                if (ParentNode.Text == Relation.ParentTable.ToString())
                    ChildNode = ParentNode.Nodes.Add(Relation.ChildTable.TableName);
                GenRelationBindingSource(Relation.ChildTable.ChildRelations, ChildNode);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tvProviderTree.SelectedNode == null)
                throw new Exception("Please select a provider !!");
            DialogResult = DialogResult.OK;
        }

    }
}