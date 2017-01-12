namespace EEPSetUpLibrary.Server
{
    partial class frmConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfig));
            this.treeViewFiles = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelPort = new System.Windows.Forms.Label();
            this.textBoxFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.overWritableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxDefault = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBoxClientMain = new System.Windows.Forms.PictureBox();
            this.pictureBoxClient = new System.Windows.Forms.PictureBox();
            this.pictureBoxClientLoader = new System.Windows.Forms.PictureBox();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.panelImage = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonFolder = new System.Windows.Forms.Button();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClientMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClientLoader)).BeginInit();
            this.panelImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewFiles
            // 
            this.treeViewFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewFiles.CheckBoxes = true;
            this.treeViewFiles.ImageIndex = 1;
            this.treeViewFiles.ImageList = this.imageList;
            this.treeViewFiles.Location = new System.Drawing.Point(268, 24);
            this.treeViewFiles.Name = "treeViewFiles";
            this.treeViewFiles.SelectedImageIndex = 0;
            this.treeViewFiles.Size = new System.Drawing.Size(270, 415);
            this.treeViewFiles.TabIndex = 7;
            this.treeViewFiles.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFiles_AfterCheck);
            this.treeViewFiles.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewFiles_NodeMouseClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Folder");
            this.imageList.Images.SetKeyName(1, "File");
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.Location = new System.Drawing.Point(41, 396);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 9;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.Location = new System.Drawing.Point(152, 396);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(14, 33);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(35, 12);
            this.labelPort.TabIndex = 0;
            this.labelPort.Text = "Port:";
            // 
            // textBoxFolder
            // 
            this.textBoxFolder.BackColor = System.Drawing.Color.White;
            this.textBoxFolder.Location = new System.Drawing.Point(65, 61);
            this.textBoxFolder.Name = "textBoxFolder";
            this.textBoxFolder.ReadOnly = true;
            this.textBoxFolder.Size = new System.Drawing.Size(184, 21);
            this.textBoxFolder.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Folder:";
            // 
            // textBoxPort
            // 
            this.textBoxPort.BackColor = System.Drawing.Color.White;
            this.textBoxPort.ContextMenuStrip = this.contextMenuStrip;
            this.textBoxPort.Location = new System.Drawing.Point(65, 28);
            this.textBoxPort.MaxLength = 5;
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(75, 21);
            this.textBoxPort.TabIndex = 1;
            this.textBoxPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPort_KeyPress);
            this.textBoxPort.TextChanged += new System.EventHandler(this.textBoxPort_TextChanged);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.overWritableToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(139, 26);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // overWritableToolStripMenuItem
            // 
            this.overWritableToolStripMenuItem.CheckOnClick = true;
            this.overWritableToolStripMenuItem.Name = "overWritableToolStripMenuItem";
            this.overWritableToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.overWritableToolStripMenuItem.Text = "OverWritable";
            this.overWritableToolStripMenuItem.CheckedChanged += new System.EventHandler(this.overWritableToolStripMenuItem_CheckedChanged);
            // 
            // checkBoxDefault
            // 
            this.checkBoxDefault.AutoSize = true;
            this.checkBoxDefault.Location = new System.Drawing.Point(146, 33);
            this.checkBoxDefault.Name = "checkBoxDefault";
            this.checkBoxDefault.Size = new System.Drawing.Size(66, 16);
            this.checkBoxDefault.TabIndex = 2;
            this.checkBoxDefault.Text = "Default";
            this.checkBoxDefault.UseVisualStyleBackColor = true;
            this.checkBoxDefault.CheckedChanged += new System.EventHandler(this.checkBoxDefault_CheckedChanged);
            // 
            // pictureBoxClientMain
            // 
            this.pictureBoxClientMain.Image = global::EEPSetUpLibrary.Properties.Resources.MainImage;
            this.pictureBoxClientMain.Location = new System.Drawing.Point(63, 164);
            this.pictureBoxClientMain.Name = "pictureBoxClientMain";
            this.pictureBoxClientMain.Size = new System.Drawing.Size(120, 90);
            this.pictureBoxClientMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxClientMain.TabIndex = 13;
            this.pictureBoxClientMain.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxClientMain, "Click to change image");
            this.pictureBoxClientMain.Click += new System.EventHandler(this.pictureBoxClientMain_Click);
            // 
            // pictureBoxClient
            // 
            this.pictureBoxClient.Image = global::EEPSetUpLibrary.Properties.Resources.BackgroundImage;
            this.pictureBoxClient.Location = new System.Drawing.Point(137, 44);
            this.pictureBoxClient.Name = "pictureBoxClient";
            this.pictureBoxClient.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxClient.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxClient.TabIndex = 12;
            this.pictureBoxClient.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxClient, "Click to change image");
            this.pictureBoxClient.Click += new System.EventHandler(this.pictureBoxClient_Click);
            // 
            // pictureBoxClientLoader
            // 
            this.pictureBoxClientLoader.Image = global::EEPSetUpLibrary.Properties.Resources.BackgroundImage;
            this.pictureBoxClientLoader.Location = new System.Drawing.Point(20, 46);
            this.pictureBoxClientLoader.Name = "pictureBoxClientLoader";
            this.pictureBoxClientLoader.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxClientLoader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxClientLoader.TabIndex = 11;
            this.pictureBoxClientLoader.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxClientLoader, "Click to change image");
            this.pictureBoxClientLoader.Click += new System.EventHandler(this.pictureBoxClientLoader_Click);
            // 
            // buttonDefault
            // 
            this.buttonDefault.Location = new System.Drawing.Point(267, 1);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(150, 23);
            this.buttonDefault.TabIndex = 6;
            this.buttonDefault.Text = "Select Default Files";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // panelImage
            // 
            this.panelImage.Controls.Add(this.label1);
            this.panelImage.Controls.Add(this.pictureBoxClientMain);
            this.panelImage.Controls.Add(this.label4);
            this.panelImage.Controls.Add(this.label3);
            this.panelImage.Controls.Add(this.pictureBoxClient);
            this.panelImage.Controls.Add(this.pictureBoxClientLoader);
            this.panelImage.Location = new System.Drawing.Point(2, 99);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(257, 281);
            this.panelImage.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "Client Main Image\r\n    (640*480)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(145, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 24);
            this.label4.TabIndex = 1;
            this.label4.Text = "Client Image\r\n  (400*90)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "Client Loader Image\r\n     (420*90)";
            // 
            // buttonFolder
            // 
            this.buttonFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFolder.Image = ((System.Drawing.Image)(resources.GetObject("buttonFolder.Image")));
            this.buttonFolder.Location = new System.Drawing.Point(228, 65);
            this.buttonFolder.Name = "buttonFolder";
            this.buttonFolder.Size = new System.Drawing.Size(21, 21);
            this.buttonFolder.TabIndex = 5;
            this.buttonFolder.UseVisualStyleBackColor = true;
            this.buttonFolder.Visible = false;
            this.buttonFolder.Click += new System.EventHandler(this.buttonFolder_Click);
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 446);
            this.ControlBox = false;
            this.Controls.Add(this.buttonDefault);
            this.Controls.Add(this.panelImage);
            this.Controls.Add(this.checkBoxDefault);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonFolder);
            this.Controls.Add(this.treeViewFiles);
            this.Name = "frmConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.frmConfig_Load);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClientMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClientLoader)).EndInit();
            this.panelImage.ResumeLayout(false);
            this.panelImage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewFiles;
        private System.Windows.Forms.Button buttonFolder;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox textBoxFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.CheckBox checkBoxDefault;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.ToolStripMenuItem overWritableToolStripMenuItem;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.PictureBox pictureBoxClient;
        private System.Windows.Forms.PictureBox pictureBoxClientLoader;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBoxClientMain;
        private System.Windows.Forms.Label label1;


    }
}