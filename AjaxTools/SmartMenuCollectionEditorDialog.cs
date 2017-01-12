using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace AjaxTools
{
    public partial class SmartMenuCollectionEditorDialog : Form
    {
        public SmartMenuItemCollection Collection = null;
        private List<SmartMenuItem> RootMenus = new List<SmartMenuItem>();
        private List<SmartMenuItem> ChildMenus = new List<SmartMenuItem>();

        public SmartMenuCollectionEditorDialog(SmartMenuItemCollection collection)
        {
            InitializeComponent();
            GenerateTree(collection);
        }

        private void GenerateTree(SmartMenuItemCollection collection)
        {
            //Collection = new SmartMenuItemCollection(collection.Owner, collection.ItemType);
            Collection = collection;
            foreach (SmartMenuItem item in collection)
            {
                if (item.Parent == null || item.Parent == "")
                    RootMenus.Add(item);
                else
                    ChildMenus.Add(item);
            }

            foreach (SmartMenuItem item in RootMenus)
            {
                TreeNode node = new TreeNode();
                node.Text = item.Ind;
                node.Name = item.MenuId;
                List<SmartMenuItem> childMenus = this.GetChildMenus(item);
                if (childMenus.Count > 0)
                {
                    node.Nodes.AddRange(this.GenerateChildTreeNodes(childMenus));
                }
                this.tView.Nodes[0].Nodes.Add(node);
            }
            this.tView.ExpandAll();
        }

        private List<SmartMenuItem> GetChildMenus(SmartMenuItem parentMenu)
        {
            List<SmartMenuItem> childMenus = new List<SmartMenuItem>();
            foreach (SmartMenuItem item in ChildMenus)
            {
                if (item.Parent == parentMenu.MenuId)
                {
                    childMenus.Add(item);
                }
            }
            return childMenus;
        }

        private TreeNode[] GenerateChildTreeNodes(List<SmartMenuItem> childMenus)
        {
            List<TreeNode> childNodes = new List<TreeNode>();
            foreach (SmartMenuItem item in childMenus)
            {
                TreeNode node = new TreeNode();
                node.Text = item.Ind;
                node.Name = item.MenuId;
                List<SmartMenuItem> _childMenus = this.GetChildMenus(item);
                if (childMenus.Count > 0)
                {
                    TreeNode[] nodes = GenerateChildTreeNodes(_childMenus);
                    node.Nodes.AddRange(nodes);
                }
                childNodes.Add(node);
            }
            return childNodes.ToArray();
        }

        private void SmartMenuCollectionEditorDialog_Load(object sender, EventArgs e)
        {
            if (this.tView.HasChildren)
            {
                this.tView.SelectedNode = this.tView.Nodes[0];
            }
        }

        private void tView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == tView.Nodes[0])
            {
                this.propGrid.SelectedObject = null;
                return;
            }
            foreach (SmartMenuItem item in this.Collection)
            {
                if (item.MenuId == e.Node.Name)
                {
                    this.propGrid.SelectedObject = item;
                    break;
                }
            }
        }

        private void tsAdd_Click(object sender, EventArgs e)
        {
            if (this.tView.SelectedNode != null)
            {
                string parent = "";
                if (!this.tView.Nodes[0].IsSelected)
                {
                    parent = this.tView.SelectedNode.Name;
                }
                TreeNode node = new TreeNode("");
                tView.SelectedNode.Nodes.Add(node);
                tView.SelectedNode.Expand();
                this.Collection.Add(new SmartMenuItem("", parent, "", "", "", ""));
                tView.SelectedNode = node;
            }
        }

        private void tsDelete_Click(object sender, EventArgs e)
        {
            if (this.tView.SelectedNode != null && this.tView.SelectedNode != this.tView.Nodes[0])
            {
                SmartMenuItem parentMenuItem = this.Collection[this.tView.SelectedNode.Text] as SmartMenuItem;
                removeChildMenus(parentMenuItem);
                this.tView.SelectedNode.Remove();
            }
        }

        private void removeChildMenus(SmartMenuItem parentMenuItem)
        {
            if (parentMenuItem == null)
                return;
            List<SmartMenuItem> childMenus = this.GetChildMenus(parentMenuItem);
            if (childMenus.Count > 0)
            {
                foreach (SmartMenuItem item in childMenus)
                {
                    removeChildMenus(item);
                }
            }
            this.Collection.Remove(parentMenuItem);
        }

        private void tsMoveUp_Click(object sender, EventArgs e)
        {
            TreeNode targetNode = this.tView.SelectedNode;
            if (targetNode != null && targetNode != this.tView.Nodes[0])
            {
                TreeNode parentNode = targetNode.Parent;
                TreeNode preNode = this.tView.SelectedNode.PrevNode;
                if (preNode != null)
                {
                    string preName = preNode.Name;
                    string targetName = targetNode.Name;
                    SmartMenuItem preItem = null;
                    SmartMenuItem targetItem = null;
                    foreach (SmartMenuItem item in this.Collection)
                    {
                        if (item.MenuId == preName)
                        {
                            preItem = item;
                        }
                        else if (item.MenuId == targetName)
                        {
                            targetItem = item;
                        }
                    }
                    if (preItem != null && targetItem != null)
                    {
                        this.Collection.Remove(preItem);
                        int targetIndex = this.Collection.GetItemIndex(targetItem);
                        this.Collection.Insert(targetIndex + 1, preItem);

                        parentNode.Nodes.Remove(preNode);
                        parentNode.Nodes.Insert(targetNode.Index + 1, preNode);

                    }
                }
            }
        }

        private void tsMoveDown_Click(object sender, EventArgs e)
        {
            TreeNode targetNode = this.tView.SelectedNode;
            if (targetNode != null && targetNode != this.tView.Nodes[0])
            {
                TreeNode parentNode = targetNode.Parent;
                TreeNode nextNode = this.tView.SelectedNode.NextNode;
                if (nextNode != null)
                {
                    string nextName = nextNode.Name;
                    string targetName = targetNode.Name;
                    SmartMenuItem nextItem = null;
                    SmartMenuItem targetItem = null;
                    foreach (SmartMenuItem item in this.Collection)
                    {
                        if (item.MenuId == nextName)
                        {
                            nextItem = item;
                        }
                        else if (item.MenuId == targetName)
                        {
                            targetItem = item;
                        }
                    }
                    if (nextItem != null && targetItem != null)
                    {
                        this.Collection.Remove(nextItem);
                        int targetIndex = this.Collection.GetItemIndex(targetItem);
                        this.Collection.Insert(targetIndex, nextItem);

                        parentNode.Nodes.Remove(nextNode);
                        parentNode.Nodes.Insert(targetNode.Index, nextNode);

                    }
                }
            }
        }

        private void propGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            SmartMenuItem item = ((PropertyGrid)s).SelectedObject as SmartMenuItem;

            this.tView.SelectedNode.Name = item.MenuId;
            this.tView.SelectedNode.Text = item.Ind;
        }
    }
}