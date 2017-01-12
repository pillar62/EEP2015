using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace Srvtools
{
    public partial class frmExternalRefvalEditor : Form
    {
        public frmExternalRefvalEditor()
        {
            InitializeComponent();
        }

        public string SelectedValue
        {
            get 
            {
                TreeNode node = treeViewOther.SelectedNode;
                if (node != null && node.Level == 2)
                {
                    return node.Name;
                }
                return null;
            }
        }

        private void InitialTreeView(TreeView treeView, Assembly assembly)
        {
            treeView.Nodes.Clear();
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.BaseType == typeof(Form) || type.BaseType == typeof(InfoForm))
                {
                    FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (FieldInfo field in fields)
                    {
                        if (field.FieldType == typeof(InfoRefVal))
                        {
                            if (treeView.Nodes[type.Namespace] == null)
                            {
                                TreeNode node = new TreeNode();
                                node.Name = type.Namespace;
                                node.Text = type.Namespace;
                                node.ImageKey = "Namespace";
                                node.SelectedImageKey = "Namespace";
                                InsertNode(treeView.Nodes, node);
                            }
                            if (treeView.Nodes[type.Namespace].Nodes[type.FullName] == null)
                            {
                                TreeNode node = new TreeNode();
                                node.Name = type.FullName;
                                node.Text = type.Name;
                                node.ImageKey = "Form";
                                node.SelectedImageKey = "Form";
                                InsertNode(treeView.Nodes[type.Namespace].Nodes, node);
                            }
                            TreeNode nodeRefval = new TreeNode();
                            nodeRefval.Name = string.Format("{0}.{1}", type.FullName, field.Name);
                            nodeRefval.Text = field.Name;
                            nodeRefval.ImageKey = "InfoRefVal";
                            nodeRefval.SelectedImageKey = "InfoRefVal";
                            InsertNode(treeView.Nodes[type.Namespace].Nodes[type.FullName].Nodes, nodeRefval);
                        }
                    }
                }
            }
        }

        private void InsertNode(TreeNodeCollection nodeCollection, TreeNode node)
        {
            int index = 0;
            for (int i = 0; i < nodeCollection.Count; i++)
            {
                if (nodeCollection[i].Name.CompareTo(node.Name) > 0)
                {
                    index = i;
                    break;
                }
            }
            nodeCollection.Insert(index, node);
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select assembly file";
            dialog.Filter = "Assembly File(*.exe;*.dll)|*.exe;*.dll";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxAssembly.Text = dialog.FileName;
                try
                {
                    byte[] fileByte = File.ReadAllBytes(dialog.FileName);
                    Assembly assembly = Assembly.Load(fileByte);
                    InitialTreeView(treeViewOther, assembly);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedValue))
            {
                MessageBox.Show(this, "Selected a refval node first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }
    }
}