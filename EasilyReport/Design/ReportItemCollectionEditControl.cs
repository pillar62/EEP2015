using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Infolight.EasilyReportTools.Design
{
    [ToolboxItem(false)]
    public partial class ReportItemCollectionEditControl : UserControl
    {
        public ReportItemCollectionEditControl()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                InitAvailableItems();
            }
        }

        private void InitAvailableItems()
        {
            Assembly reportAssembly = Assembly.GetAssembly(typeof(ReportItemCollection));
            Type[] types = reportAssembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(ReportItem)) && !type.IsAbstract)
                {
                    ReportItem item = (ReportItem)Activator.CreateInstance(type);
                    AddNode(treeViewItems, item);
                }
            }
        }

        private void RefreshSelectedItems()
        {
            treeViewCollection.Nodes.Clear();
            foreach (ReportItem item in Collection)
            {
                AddNode(treeViewCollection, item);
            }
        }

        private void AddNode(TreeView view, ReportItem item)
        {
            TreeNode node = new TreeNode();
            string itemName = item.GetType().Name;
            node.Name = itemName;
            node.Text = itemName;
            node.ImageKey = itemName;
            node.SelectedImageKey = itemName;
            node.Tag = item;
            view.Nodes.Add(node);
        }

        private ReportItemCollection collection;

        public ReportItemCollection Collection
        {
            get { return collection; }
            set { collection = value; }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (treeViewItems.SelectedNode != null)
            {
                ReportItem item = treeViewItems.SelectedNode.Tag as ReportItem;
                Collection.Add(item.New());
                RefreshSelectedItems();
                treeViewCollection.SelectedNode = treeViewCollection.Nodes[treeViewCollection.Nodes.Count - 1];
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (treeViewCollection.SelectedNode != null && treeViewCollection.SelectedNode.Index != 0)
            {
                int index = treeViewCollection.SelectedNode.Index;
                ReportItem item = Collection[index];
                Collection.RemoveAt(index);
                Collection.Insert(index - 1, item);
                RefreshSelectedItems();
                treeViewCollection.SelectedNode = treeViewCollection.Nodes[index - 1];
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (treeViewCollection.SelectedNode != null && treeViewCollection.SelectedNode.Index != treeViewCollection.Nodes.Count - 1)
            {
                int index = treeViewCollection.SelectedNode.Index;
                ReportItem item = Collection[index];
                Collection.RemoveAt(index);
                Collection.Insert(index + 1, item);
                RefreshSelectedItems();
                treeViewCollection.SelectedNode = treeViewCollection.Nodes[index + 1];
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (treeViewCollection.SelectedNode != null)
            {
                int index = treeViewCollection.SelectedNode.Index;
                Collection.RemoveAt(index);
                RefreshSelectedItems();
                index = Math.Min(index, treeViewCollection.Nodes.Count - 1);
                if (index >= 0)
                {
                    treeViewCollection.SelectedNode = treeViewCollection.Nodes[index];
                }
            }
        }

        private void treeViewCollection_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                labelItem.Text = string.Format("{0} property", e.Node.Tag.GetType().Name);
                propertyGridItem.SelectedObject = e.Node.Tag;
            }
        }

       

      
	
    }
}
