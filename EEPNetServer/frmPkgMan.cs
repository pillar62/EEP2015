using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.IO;
using System.Reflection;
using Srvtools;
using Microsoft.Win32;
using System.Collections.Generic;

namespace EEPNetServer
{
	/// <summary>
	/// Summary description for WinForm.
	/// </summary>
	public class frmPkgMan : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.Xml.XmlDocument PkgXml;
        private System.Xml.XmlDocument SrvXml;
		private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Label lblPackage;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnSave;
        private ListBox lbxServices;
        private Button button1;
        private Button button2;
        private TreeView treeView1;
        private ContextMenu contextMenu1;
        private MenuItem miAdd;
        private MenuItem miDelete;
        private MenuItem miModify;
        private FolderBrowserDialog dlgPath;
        private Button button3;
        private MenuItem miActive;
        private MenuItem miIActive;
        private MenuItem miUnload;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private Label label1;

        private bool fCanCheck;
        private CheckBox checkBoxAssembly;
        private SplitContainer splitContainer1;
        private bool isNeedSave = false;

		public frmPkgMan()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose (bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lblPackage = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.lbxServices = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.miAdd = new System.Windows.Forms.MenuItem();
            this.miDelete = new System.Windows.Forms.MenuItem();
            this.miModify = new System.Windows.Forms.MenuItem();
            this.miActive = new System.Windows.Forms.MenuItem();
            this.miIActive = new System.Windows.Forms.MenuItem();
            this.miUnload = new System.Windows.Forms.MenuItem();
            this.dlgPath = new System.Windows.Forms.FolderBrowserDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxAssembly = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPackage
            // 
            this.lblPackage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackage.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblPackage.Location = new System.Drawing.Point(12, 11);
            this.lblPackage.Name = "lblPackage";
            this.lblPackage.Size = new System.Drawing.Size(100, 16);
            this.lblPackage.TabIndex = 0;
            this.lblPackage.Text = "Packages";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(81, 377);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(64, 23);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(11, 377);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(64, 23);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Add...";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(221, 377);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "Pakcages files(*.dll)|*.dll";
            this.dlgOpen.InitialDirectory = "sysbpl";
            this.dlgOpen.Multiselect = true;
            // 
            // lbxServices
            // 
            this.lbxServices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxServices.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbxServices.FormattingEnabled = true;
            this.lbxServices.Location = new System.Drawing.Point(15, 36);
            this.lbxServices.Name = "lbxServices";
            this.lbxServices.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxServices.Size = new System.Drawing.Size(253, 316);
            this.lbxServices.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(132, 377);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Add...";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(204, 377);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Delete";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.CheckBoxes = true;
            this.treeView1.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(12, 36);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(273, 335);
            this.treeView1.TabIndex = 14;
            this.treeView1.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeCheck);
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miAdd,
            this.miDelete,
            this.miModify,
            this.miActive,
            this.miIActive,
            this.miUnload});
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // miAdd
            // 
            this.miAdd.Index = 0;
            this.miAdd.Text = "Add Folder...";
            this.miAdd.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // miDelete
            // 
            this.miDelete.Index = 1;
            this.miDelete.Text = "Delete Folder";
            this.miDelete.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // miModify
            // 
            this.miModify.Index = 2;
            this.miModify.Text = "Modify Folder...";
            this.miModify.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // miActive
            // 
            this.miActive.Index = 3;
            this.miActive.Text = "Active";
            this.miActive.Click += new System.EventHandler(this.miActive_Click);
            // 
            // miIActive
            // 
            this.miIActive.Index = 4;
            this.miIActive.Text = "InActive";
            this.miIActive.Click += new System.EventHandler(this.miIActive_Click);
            // 
            // miUnload
            // 
            this.miUnload.Index = 5;
            this.miUnload.Text = "Unload";
            this.miUnload.Click += new System.EventHandler(this.miUnload_Click);
            // 
            // dlgPath
            // 
            this.dlgPath.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.dlgPath.SelectedPath = "folderBrowserDialog1";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(151, 377);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(64, 23);
            this.button3.TabIndex = 15;
            this.button3.Text = "Unload";
            this.button3.Click += new System.EventHandler(this.miUnload_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "Methods";
            // 
            // checkBoxAssembly
            // 
            this.checkBoxAssembly.AutoSize = true;
            this.checkBoxAssembly.Checked = true;
            this.checkBoxAssembly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAssembly.Location = new System.Drawing.Point(114, 12);
            this.checkBoxAssembly.Name = "checkBoxAssembly";
            this.checkBoxAssembly.Size = new System.Drawing.Size(108, 16);
            this.checkBoxAssembly.TabIndex = 17;
            this.checkBoxAssembly.Text = "Load in memory";
            this.checkBoxAssembly.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblPackage);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxAssembly);
            this.splitContainer1.Panel1.Controls.Add(this.btnSave);
            this.splitContainer1.Panel1.Controls.Add(this.btnAdd);
            this.splitContainer1.Panel1.Controls.Add(this.button3);
            this.splitContainer1.Panel1.Controls.Add(this.btnDelete);
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.lbxServices);
            this.splitContainer1.Panel2.Controls.Add(this.button2);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Size = new System.Drawing.Size(583, 412);
            this.splitContainer1.SplitterDistance = 290;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 18;
            // 
            // frmPkgMan
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(583, 412);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmPkgMan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Package Manager";
            this.Activated += new System.EventHandler(this.frmPkgMan_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPkgMan_FormClosing);
            this.Load += new System.EventHandler(this.frmPkgMan_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmPkgMan_Load(object sender, System.EventArgs e)
		{
            fCanCheck = true;
            try
            {
                treeView1.ContextMenu = this.contextMenu1;
                PkgXml = new XmlDocument();
                string s = SystemFile.PackagesFile;
                if (File.Exists(s))
                {
                    PkgXml.Load(s);
                    if (PkgXml.DocumentElement.Attributes["LoadInMemory"] != null)
                    {
                        checkBoxAssembly.Checked = Convert.ToBoolean(PkgXml.DocumentElement.Attributes["LoadInMemory"].Value);
                    }
                    XmlNode aNode = PkgXml.DocumentElement.FirstChild;
                    bool commonExist = false;
                    while (aNode != null)
                    {
                        TreeNode aTreeNode= null;
                        if (string.Compare(aNode.Name, EEPRemoteModule.COMMON_SOLUTION, true) == 0)
                        {
                            commonExist = true;
                            aTreeNode = treeView1.Nodes.Insert(0, aNode.Name);
                        }
                        else
                        {
                            aTreeNode = treeView1.Nodes.Add(aNode.Name);
                        }

                        List<TreeNode> nodeList = new List<TreeNode>();
                        foreach (XmlNode node in aNode.ChildNodes)
                        {
                            TreeNode xNode = new TreeNode();
                            xNode.Text = node.Attributes["Name"].InnerText;
                            if ((node.Attributes["Active"] == null) || (node.Attributes["Active"].InnerText == "1"))
                            {
                                xNode.Checked = true;
                            }
                            else
                            {
                                xNode.Checked = false;
                            }
                            nodeList.Add(xNode);
                        }
                        nodeList.Sort(new Comparison<TreeNode>(SortNode));
                        aTreeNode.Nodes.AddRange(nodeList.ToArray());
                        aNode = aNode.NextSibling;
                    }
                    if(!commonExist)
                    {
                       treeView1.Nodes.Insert(0, EEPRemoteModule.COMMON_SOLUTION);
                    }
                }
            }
            finally
            {
                fCanCheck = false;
            }
        }

        private int SortNode(TreeNode node1, TreeNode node2)
        {
            return node1.Text.CompareTo(node2.Text);
        }

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            string s = SystemFile.PackagesFile;
			FileStream aFileStream = new FileStream(s, FileMode.Create);

			try
			{
				XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
				w.Formatting = Formatting.Indented;
				w.WriteStartElement("InfolightPackages");
                w.WriteAttributeString("LoadInMemory", checkBoxAssembly.Checked.ToString());
                for (int i = 0; i < treeView1.Nodes.Count; i++)
                {
                    TreeNode aNode = treeView1.Nodes[i];
                    w.WriteStartElement(treeView1.Nodes[i].Text);
                    for (int j = 0; j < aNode.Nodes.Count; j ++)
                    {
                        w.WriteStartElement("PackageFile");
                        w.WriteAttributeString("Name", null, aNode.Nodes[j].Text);
                        if (aNode.Nodes[j].Checked)
                            w.WriteAttributeString("Active", null, "1");
                        else
                            w.WriteAttributeString("Active", null, "0");

                        w.WriteEndElement();
                    }
                    w.WriteEndElement();
                }

				w.WriteEndElement();
				w.Close();
			}
			finally
			{
				aFileStream.Close();
			}

            isNeedSave = false;
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			string sFileName;
            if ((treeView1.SelectedNode != null) && (treeView1.SelectedNode.Level == 0))
            {
                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    TreeNode aTreeNode = treeView1.SelectedNode;
                    foreach (string strfile in dlgOpen.FileNames)
                    {
                        sFileName = Path.GetFileName(strfile);

                        bool b = false;
                        for (int i = 0; i < aTreeNode.Nodes.Count; i++)
                        {
                            if (string.Compare(aTreeNode.Nodes[i].Text, sFileName, true) == 0)//IgnoreCase
                            {
                                b = true;
                                break;
                            }
                        }
                        if (!b)
                        {
                            fCanCheck = true;
                            try
                            {
                                TreeNode xTreeNode = aTreeNode.Nodes.Add(sFileName);
                                xTreeNode.Checked = true;
                            }
                            finally
                            {
                                fCanCheck = false;
                            }
                        }
                    }
                    isNeedSave = true;
                }
            }
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Level == 1)
                {
                    treeView1.SelectedNode.Remove();
                    isNeedSave = true;
                }
            }
		}

        private void button1_Click(object sender, EventArgs e)
        {
            if ((null == treeView1.SelectedNode) || (1 != treeView1.SelectedNode.Level))
                return;

            frmSrvMan afrmSrvMan = new frmSrvMan();

            string PackName = treeView1.SelectedNode.Parent.Text;
            String fullSrvName = string.Format("{0}\\{1}\\{2}", EEPRegistry.Server, PackName, treeView1.SelectedNode.Text);
            String srvName = treeView1.SelectedNode.Text;

            afrmSrvMan.PackageFileFullName = fullSrvName;
            afrmSrvMan.PackageFileName = srvName;

            afrmSrvMan.ShowDialog();
            afrmSrvMan.Dispose();
            treeView1_AfterSelect(null, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lbxServices.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a service", "Error");
                return;
            }

            SrvXml = new XmlDocument();
            String srvName = SystemFile.ServiceFile;

            if (File.Exists(srvName))
            {
                // Modify file's attribute,can write.

                SrvXml.Load(srvName);

                XmlNode pkgNode = SrvXml.DocumentElement.SelectSingleNode("PackageFile[@Name = '" + treeView1.SelectedNode.Text + "']");

                String serviceName;
                foreach (Object obj in lbxServices.SelectedItems)
                {
                    serviceName = obj.ToString();

                    XmlNodeList nodeList = pkgNode.SelectNodes("Service[@Name = '" + serviceName + "']");

                    #region Remove service nodes

                    foreach (XmlNode node in nodeList)  // Create the servie node
                    {
                        pkgNode.RemoveChild(node);
                    }

                    #endregion
                }

                SrvXml.Save(srvName);
            }

            treeView1_AfterSelect(null, null);

            //xx lbxPackages_SelectedIndexChanged(lbxPackages, null);
        }

        private void frmPkgMan_Activated(object sender, EventArgs e)
        {
            //xx lbxPackages_SelectedIndexChanged(lbxPackages, null);
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            if ((null == treeView1.SelectedNode) || (treeView1.SelectedNode.Level != 1))
            {
                miActive.Visible = false;
                miIActive.Visible = false;
                miUnload.Visible = false;
                miAdd.Visible = true;
                miDelete.Visible = true;
                miModify.Visible = true;
            }
            else
            {
                miActive.Visible = true;
                miIActive.Visible = true;
                miUnload.Visible = true;
                miAdd.Visible = false;
                miDelete.Visible = false;
                miModify.Visible = false;
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            dlgPath.SelectedPath = EEPRegistry.Server;
            bool b = false;
            string sNew = "";
            if (dlgPath.ShowDialog() == DialogResult.OK)
            {
                sNew = Path.GetFileName(dlgPath.SelectedPath);
                for (int i = 0; i < this.treeView1.Nodes.Count; i++)
                {
                    if (string.Compare(treeView1.Nodes[i].Text, sNew, true) == 0)//IgnoreCase
                    {
                        b = true;
                        break;
                    }
                }
                if (!b)
                    treeView1.Nodes.Add(sNew);

                isNeedSave = true;
            }

        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if ((null != treeView1.SelectedNode) && (0 == treeView1.SelectedNode.Level))
            {
                if ((MessageBox.Show("Are you sure to delete this solution?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    treeView1.SelectedNode.Remove();

                    isNeedSave = true;
                }
            }
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            if ((null != treeView1.SelectedNode) && (0 == treeView1.SelectedNode.Level))
            {
                dlgPath.SelectedPath = EEPRegistry.Server;
                string sNew = "";
                if (dlgPath.ShowDialog() == DialogResult.OK)
                {
                    sNew = Path.GetFileName(dlgPath.SelectedPath);
                    treeView1.SelectedNode.Text = sNew;

                    isNeedSave = true;
                }
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if ((null != treeView1.SelectedNode) && (1 == treeView1.SelectedNode.Level))
            {
                frmPkgInfo aForm = new frmPkgInfo();
                aForm.ShowDialog();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((null == treeView1.SelectedNode) || (1 != treeView1.SelectedNode.Level))
                return;

            lbxServices.Items.Clear();

            String pkgName = treeView1.SelectedNode.Text;

            SrvXml = new XmlDocument();
            String srvName = SystemFile.ServiceFile;

            if (File.Exists(srvName))
            {
                // Modify file's attribute,can write.

                SrvXml.Load(srvName);

                XmlNodeList pkgList = SrvXml.DocumentElement.SelectNodes("PackageFile[@Name = '" + pkgName + "']");

                if(pkgList.Count == 1)
                {
                    #region Read nodes from Services.xml by packagefile's name
                    XmlNode pkgNode = pkgList.Item(0);

                    XmlNode serviceNode = pkgNode.FirstChild;

                    while (serviceNode != null)
                    {
                        lbxServices.Items.Add(serviceNode.Attributes["Name"].Value);
                        serviceNode = serviceNode.NextSibling;
                    }
                    #endregion
                }
                else if (pkgList.Count > 1)
                {
                    // 说明有多个
                }
                else // 说明还没有这个项就创建
                {
                    #region Create "PackageFile" XmlElement
                    XmlElement pkgElement = SrvXml.CreateElement("PackageFile");

                    XmlAttribute nameAttribute = SrvXml.CreateAttribute("Name");
                    nameAttribute.Value = pkgName;

                    pkgElement.Attributes.Append(nameAttribute);

                    XmlNode serviceNode = SrvXml.FirstChild;
                    serviceNode.AppendChild(pkgElement);

                    SrvXml.Save(srvName);
                    #endregion
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Level == 1)
                {
                    String s = string.Format("{0}\\{1}\\{2}", EEPRegistry.Server, treeView1.SelectedNode.Parent.Text, treeView1.SelectedNode.Text);
                    AssemblyContainer.RemoveAssemblyLoader(s);
                }
            }
        }

        private void miActive_Click(object sender, EventArgs e)
        {
            fCanCheck = true;
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    if (treeView1.SelectedNode.Level == 1)
                    {
                        treeView1.SelectedNode.Checked = true;

                        isNeedSave = true;
                    }
                }
            }
            finally
            {
                fCanCheck = false;
            }
        }

        private void miIActive_Click(object sender, EventArgs e)
        {
            fCanCheck = true;
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    if (treeView1.SelectedNode.Level == 1)
                    {
                        treeView1.SelectedNode.Checked = false;

                        isNeedSave = true;
                    }
                }
            }
            finally
            {
                fCanCheck = false;
            }
        }

        private void miUnload_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Level == 1)
                {
                    if (AssemblyCache.Enable)
                    {
                        String s = string.Format("{0}\\{1}\\{2}", EEPRegistry.Server, treeView1.SelectedNode.Parent.Text, treeView1.SelectedNode.Text);
                        AssemblyCache.Remove(s);
                        MessageBox.Show(this,string.Format("Unload {0} successfully", treeView1.SelectedNode.Text),"Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(this, string.Format("Unload can only in load in memory mode", treeView1.SelectedNode.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
        }

        private void treeView1_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (!fCanCheck)
                e.Cancel = true;
        }

        string CategoryName;
        List<string> ExportList = new List<string>();
        private void miExport_Click(object sender, EventArgs e)
        {
            CategoryName = "";
            ExportList.Clear();
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Level != 1)
                {
                    CategoryName = treeView1.SelectedNode.Text;
                    string str = "";
                    foreach (TreeNode tn in treeView1.SelectedNode.Nodes)
                    {
                        str = tn.Text.Substring(0, tn.Text.IndexOf(".dll"));
                        ExportList.Add(str);
                    }
                }
                else
                {
                    string str = treeView1.SelectedNode.Text;
                    str = str.Substring(0, str.IndexOf(".dll"));
                    CategoryName = treeView1.SelectedNode.Parent.Text;
                    ExportList.Add(str);
                }
            }
            frmPkgExportDB frm1 = new frmPkgExportDB(CategoryName, ExportList);
            frm1.ShowDialog();
        }

        private void frmPkgMan_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isNeedSave)
            {
                DialogResult result = MessageBox.Show("Changes has not to saved,do you want to save?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    btnSave_Click(null, null);                    
                }                
            }
        }
    }
}
