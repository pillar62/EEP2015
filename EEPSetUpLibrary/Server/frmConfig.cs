using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace EEPSetUpLibrary.Server
{
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            textBoxFolder.Text = Config.WorkPath;
            textBoxPort.Text = Config.ServerPort.ToString();
            if (Config.ServerPort == 8990)
            {
                checkBoxDefault.Checked = true;
            }
            
#if Remoting
            labelPort.Visible = false;
            textBoxPort.Visible = false;
            checkBoxDefault.Visible = false;
            if (File.Exists(Config.ClientImagePath))
            {
                pictureBoxClient.ImageLocation = Config.ClientImagePath;
            }
            if (File.Exists(Config.ClientMainImagePath))
            {
                pictureBoxClientMain.ImageLocation = Config.ClientMainImagePath;
            }
            if (File.Exists(Config.ClientLoaderImagePath))
            {
                pictureBoxClientLoader.ImageLocation = Config.ClientLoaderImagePath;
            }

#else
            panelImage.Visible = false;
#endif
            if (Config.TempWorkPath.Length > 0)
            {
                CreateFileTreeView(Config.TempWorkPath);
            }
        }

        private void buttonFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Config.TempWorkPath;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBoxFolder.Text = fbd.SelectedPath;
                CreateFileTreeView(fbd.SelectedPath);
            }
        }

        private void CreateFileTreeView(string directory)
        {
            this.treeViewFiles.Nodes.Clear();
            TreeNode node = new TreeNode(directory, 0, 0);
            treeViewFiles.Nodes.Add(node);
            CreateFileTreeNode(node, new DirectoryInfo(directory));

            IEnumerator ie = Config.TempLocalFiles.GetEnumerator();//勾选已经设置过的
            while (ie.MoveNext())
            {
                FileInfo fi = ((DictionaryEntry)ie.Current).Value as FileInfo;
                string fullname = Config.TempWorkPath + fi.ToString();
                TreeNode[] nodefile = treeViewFiles.Nodes.Find(fullname, true);
                if (nodefile.Length > 0)
                {
                    nodefile[0].Checked = true;
                    nodefile[0].Tag = fi.OverWritable;
                }
            }
            SetParentChecked(node);//如果子节点全选的话将父节点勾上
            node.Expand();
        }

        private void SetParentChecked(TreeNode nodeparent)
        {
            if (nodeparent.Nodes.Count > 0)
            {
                bool isallchecked = true;
                for (int i = 0; i < nodeparent.Nodes.Count; i++)
                {
                    SetParentChecked(nodeparent.Nodes[i]);
                    if (!nodeparent.Nodes[i].Checked)
                    {
                        isallchecked = false;
                    }
                }
                if (isallchecked)
                {
                    nodeparent.Checked = true;
                }
            }
            else if (nodeparent.ImageIndex == 0)//空文件夹也要勾选
            {
                nodeparent.Checked = true;
            }
        }

        private void CreateFileTreeNode(TreeNode nodeparent, DirectoryInfo diparent)
        {
            foreach (DirectoryInfo di in diparent.GetDirectories())//访问某些目录有问题
            {
                TreeNode node = new TreeNode(di.Name, 0, 0);
                nodeparent.Nodes.Add(node);
                CreateFileTreeNode(node,di);
            }
            foreach (System.IO.FileInfo fi in diparent.GetFiles())
            {
                TreeNode node = new TreeNode(fi.Name, 1, 1);
                node.Name = fi.FullName;//为了方便查找
                node.ContextMenuStrip = contextMenuStrip;
                nodeparent.Nodes.Add(node);
            }
        }

        private bool Save()
        {
            try
            {   
                Config.ServerPort = Convert.ToInt32(textBoxPort.Text);
            }
            catch
            {
                MessageBox.Show(this, "The port is not valid ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Config.TempWorkPath = textBoxFolder.Text;
            if (pictureBoxClient.ImageLocation != null)
            {
                Config.ClientImagePath = pictureBoxClient.ImageLocation;
            }
            if (pictureBoxClientMain.ImageLocation != null)
            {
                Config.ClientMainImagePath = pictureBoxClientMain.ImageLocation;
            }
            if (pictureBoxClientLoader.ImageLocation != null)
            {
                Config.ClientLoaderImagePath = pictureBoxClientLoader.ImageLocation;
            }
            Config.Save();
            //取得更新文件的集合
            FileInfoCollection fc = new FileInfoCollection();
            GetFileTreeNode(treeViewFiles.Nodes[0],fc);
            Config.TempLocalFiles = fc;
            Config.SaveFiles(fc);
            return true;
        }

        private void GetFileTreeNode(TreeNode nodeparent, FileInfoCollection fc)
        {
            for (int i = 0; i < nodeparent.Nodes.Count; i++)
            {
                TreeNode node = nodeparent.Nodes[i];
                if (node.Nodes.Count > 0)
                {
                    GetFileTreeNode(node, fc);
                }
                else if (node.ImageIndex == 1 && node.Checked)//CheckedFile
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(node.Name);
                    FileInfo finew = new FileInfo(fi);
                    if (node.Tag != null)
                    {
                        finew.OverWritable = (bool)node.Tag;
                    }
                    fc.Add(finew.ID, finew);
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                try
                {
                    Config.LoadFiles();
                    this.Close();
                }
                catch(Exception e1)
                {
                    MessageBox.Show(this, e1.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void treeViewFiles_AfterCheck(object sender, TreeViewEventArgs e)
        {
            for (int i = 0; i < e.Node.Nodes.Count; i++)
            {
                e.Node.Nodes[i].Checked = e.Node.Checked;
            }
        }

        string lastvalidport = string.Empty;
        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {
            //验证
            if (textBoxPort.Text.Length > 0)
            {
                if (!Regex.IsMatch(textBoxPort.Text, @"^(\d{1,4}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$"))
                {
                    textBoxPort.Text = lastvalidport;
                    textBoxPort.SelectionStart = lastvalidport.Length;
                }
                else
                {
                    lastvalidport = textBoxPort.Text;
                }
            }
        }

        private void checkBoxDefault_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxPort.ReadOnly = checkBoxDefault.Checked;
            if (checkBoxDefault.Checked)
            {
                textBoxPort.Text = "8990";
            }
        }

        private void textBoxPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBoxPort.ReadOnly)
            {
                toolTip.Show("if you want to modify the port,\r\n   uncheck the default checkbox"
                    , this, textBoxPort.Location.X, this.textBoxPort.Location.Y, 1500);
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            string[] defaultfiles = new string[] { "EEPNetClient.exe", "EEPNetClient.exe.config"
                , "Srvtools.dll", "InfoRemoteModule.dll", "envdte.dll", "envdte80.dll", "stdole.dll" };
            StringBuilder sbuilder = new StringBuilder();
            foreach (string str in defaultfiles)
            {
                TreeNode[] node = treeViewFiles.Nodes.Find(Config.TempWorkPath + "\\" + str, true);
                if (node.Length > 0)
                {
                    node[0].Checked = true;
                }
                else
                {
                   sbuilder.AppendLine(str);
                }
            }
            if (sbuilder.Length > 0)
            {
                sbuilder.AppendLine();
                sbuilder.Append("These files can not found.");
                MessageBox.Show(this, sbuilder.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(this, "All files selected", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void treeViewFiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.treeViewFiles.SelectedNode = e.Node;
            }
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (treeViewFiles.SelectedNode != null)
            {
                if(treeViewFiles.SelectedNode.Tag != null)
                {
                    overWritableToolStripMenuItem.Checked = (bool)treeViewFiles.SelectedNode.Tag;
                }
            }
        }

        private void overWritableToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (treeViewFiles.SelectedNode != null)
            {
                treeViewFiles.SelectedNode.Tag = overWritableToolStripMenuItem.Checked;
            }
        }

        private void pictureBoxClientLoader_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;//这个属性的说明有问题,不设置的话再用File.Exsit()的目录会更改
            dialog.Filter = "Image File(*.bmp;*.jpg;*.gif;*.png)|*.bmp;*.jpg;*.gif;*.png";
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                pictureBoxClientLoader.ImageLocation = dialog.FileName;
            }
        }

        private void pictureBoxClient_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Filter = "Image File(*.bmp;*.jpg;*.gif;*.png)|*.bmp;*.jpg;*.gif;*.png";
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                pictureBoxClient.ImageLocation = dialog.FileName;
            }
        }

        private void pictureBoxClientMain_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Filter = "Image File(*.bmp;*.jpg;*.gif;*.png)|*.bmp;*.jpg;*.gif;*.png";
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                pictureBoxClientMain.ImageLocation = dialog.FileName;
            }
        }      
    }
}