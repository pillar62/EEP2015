namespace EEPSetUpLibrary.OldClient
{
    partial class frmSetUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetUp));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonUninstall = new System.Windows.Forms.Button();
            this.linkLabelOK = new System.Windows.Forms.LinkLabel();
            this.linkLabelCancel = new System.Windows.Forms.LinkLabel();
            this.comboBoxServer = new System.Windows.Forms.ComboBox();
            this.labelFolder = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabelSetUp = new System.Windows.Forms.LinkLabel();
            this.labelInfo = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.progressBarDetail = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBarTotal = new System.Windows.Forms.ProgressBar();
            this.textBoxFolder = new System.Windows.Forms.TextBox();
            this.buttonFolder = new System.Windows.Forms.Button();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("splitContainer1.Panel1.BackgroundImage")));
            this.splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("splitContainer1.Panel2.BackgroundImage")));
            this.splitContainer1.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitContainer1.Panel2.Controls.Add(this.buttonUninstall);
            this.splitContainer1.Panel2.Controls.Add(this.linkLabelOK);
            this.splitContainer1.Panel2.Controls.Add(this.linkLabelCancel);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxServer);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.linkLabelSetUp);
            this.splitContainer1.Panel2.Controls.Add(this.labelInfo);
            this.splitContainer1.Panel2.Controls.Add(this.buttonStart);
            this.splitContainer1.Panel2.Controls.Add(this.progressBarDetail);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.progressBarTotal);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxFolder);
            this.splitContainer1.Panel2.Controls.Add(this.buttonFolder);
            this.splitContainer1.Panel2.Controls.Add(this.labelFolder);
            this.splitContainer1.Size = new System.Drawing.Size(418, 249);
            this.splitContainer1.SplitterDistance = 90;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonUninstall
            // 
            this.buttonUninstall.Location = new System.Drawing.Point(318, 95);
            this.buttonUninstall.Name = "buttonUninstall";
            this.buttonUninstall.Size = new System.Drawing.Size(67, 22);
            this.buttonUninstall.TabIndex = 10;
            this.buttonUninstall.Text = "&Uninstall";
            this.buttonUninstall.UseVisualStyleBackColor = true;
            this.buttonUninstall.Click += new System.EventHandler(this.buttonUninstall_Click);
            // 
            // linkLabelOK
            // 
            this.linkLabelOK.AutoSize = true;
            this.linkLabelOK.BackColor = System.Drawing.Color.Transparent;
            this.linkLabelOK.Location = new System.Drawing.Point(328, 125);
            this.linkLabelOK.Name = "linkLabelOK";
            this.linkLabelOK.Size = new System.Drawing.Size(17, 12);
            this.linkLabelOK.TabIndex = 9;
            this.linkLabelOK.TabStop = true;
            this.linkLabelOK.Text = "OK";
            this.linkLabelOK.Visible = false;
            this.linkLabelOK.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelOK_LinkClicked);
            // 
            // linkLabelCancel
            // 
            this.linkLabelCancel.AutoSize = true;
            this.linkLabelCancel.BackColor = System.Drawing.Color.Transparent;
            this.linkLabelCancel.Location = new System.Drawing.Point(359, 125);
            this.linkLabelCancel.Name = "linkLabelCancel";
            this.linkLabelCancel.Size = new System.Drawing.Size(41, 12);
            this.linkLabelCancel.TabIndex = 8;
            this.linkLabelCancel.TabStop = true;
            this.linkLabelCancel.Text = "Cancel";
            this.linkLabelCancel.Visible = false;
            this.linkLabelCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLableCancel_LinkClicked);
            // 
            // comboBoxServer
            // 
            this.comboBoxServer.FormattingEnabled = true;
            this.comboBoxServer.Location = new System.Drawing.Point(77, 16);
            this.comboBoxServer.Name = "comboBoxServer";
            this.comboBoxServer.Size = new System.Drawing.Size(179, 20);
            this.comboBoxServer.TabIndex = 7;
            // 
            // labelFolder
            // 
            this.labelFolder.BackColor = System.Drawing.Color.Transparent;
            this.labelFolder.Location = new System.Drawing.Point(77, 121);
            this.labelFolder.Name = "labelFolder";
            this.labelFolder.Size = new System.Drawing.Size(234, 24);
            this.labelFolder.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(24, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Server:";
            // 
            // linkLabelSetUp
            // 
            this.linkLabelSetUp.AutoSize = true;
            this.linkLabelSetUp.BackColor = System.Drawing.Color.Transparent;
            this.linkLabelSetUp.Location = new System.Drawing.Point(317, 125);
            this.linkLabelSetUp.Name = "linkLabelSetUp";
            this.linkLabelSetUp.Size = new System.Drawing.Size(83, 12);
            this.linkLabelSetUp.TabIndex = 5;
            this.linkLabelSetUp.TabStop = true;
            this.linkLabelSetUp.Text = "Change Folder";
            this.linkLabelSetUp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSetUp_LinkClicked);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.BackColor = System.Drawing.Color.Transparent;
            this.labelInfo.Location = new System.Drawing.Point(21, 47);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(0, 12);
            this.labelInfo.TabIndex = 4;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(319, 68);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(65, 22);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "&Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // progressBarDetail
            // 
            this.progressBarDetail.Location = new System.Drawing.Point(23, 71);
            this.progressBarDetail.Name = "progressBarDetail";
            this.progressBarDetail.Size = new System.Drawing.Size(290, 15);
            this.progressBarDetail.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarDetail.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(24, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Folder:";
            // 
            // progressBarTotal
            // 
            this.progressBarTotal.Location = new System.Drawing.Point(23, 97);
            this.progressBarTotal.Name = "progressBarTotal";
            this.progressBarTotal.Size = new System.Drawing.Size(290, 15);
            this.progressBarTotal.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarTotal.TabIndex = 0;
            // 
            // textBoxFolder
            // 
            this.textBoxFolder.BackColor = System.Drawing.Color.White;
            this.textBoxFolder.Location = new System.Drawing.Point(77, 121);
            this.textBoxFolder.Name = "textBoxFolder";
            this.textBoxFolder.ReadOnly = true;
            this.textBoxFolder.Size = new System.Drawing.Size(213, 21);
            this.textBoxFolder.TabIndex = 2;
            this.textBoxFolder.Visible = false;
            // 
            // buttonFolder
            // 
            this.buttonFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFolder.Image = ((System.Drawing.Image)(resources.GetObject("buttonFolder.Image")));
            this.buttonFolder.Location = new System.Drawing.Point(290, 121);
            this.buttonFolder.Name = "buttonFolder";
            this.buttonFolder.Size = new System.Drawing.Size(21, 21);
            this.buttonFolder.TabIndex = 2;
            this.buttonFolder.UseVisualStyleBackColor = true;
            this.buttonFolder.Visible = false;
            this.buttonFolder.Click += new System.EventHandler(this.buttonFolder_Click);
            // 
            // frmSetUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 249);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EEP WinLoader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSetUp_FormClosing);
            this.Load += new System.EventHandler(this.frmSetUp_Load);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ProgressBar progressBarDetail;
        private System.Windows.Forms.ProgressBar progressBarTotal;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.TextBox textBoxFolder;
        private System.Windows.Forms.Button buttonFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabelSetUp;
        private System.Windows.Forms.Label labelFolder;
        private System.Windows.Forms.ComboBox comboBoxServer;
        private System.Windows.Forms.LinkLabel linkLabelCancel;
        private System.Windows.Forms.LinkLabel linkLabelOK;
        private System.Windows.Forms.Button buttonUninstall;
    }
}