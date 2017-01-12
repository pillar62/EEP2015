namespace OfficeTools.Design
{
    partial class frmValue
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
            this.tbScript = new System.Windows.Forms.TextBox();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.dataSourceItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemParameterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.todayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataBaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.siteCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ipAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbScript
            // 
            this.tbScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbScript.ContextMenuStrip = this.contextMenu;
            this.tbScript.Location = new System.Drawing.Point(22, 42);
            this.tbScript.Multiline = true;
            this.tbScript.Name = "tbScript";
            this.tbScript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbScript.Size = new System.Drawing.Size(419, 101);
            this.tbScript.TabIndex = 0;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemInsert});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(143, 26);
            // 
            // toolStripMenuItemInsert
            // 
            this.toolStripMenuItemInsert.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataSourceItemToolStripMenuItem,
            this.controlItemToolStripMenuItem,
            this.systemParameterToolStripMenuItem});
            this.toolStripMenuItemInsert.Image = global::OfficeTools.Properties.Resources.micro;
            this.toolStripMenuItemInsert.Name = "toolStripMenuItemInsert";
            this.toolStripMenuItemInsert.Size = new System.Drawing.Size(142, 22);
            this.toolStripMenuItemInsert.Text = "InsertScript";
            // 
            // dataSourceItemToolStripMenuItem
            // 
            this.dataSourceItemToolStripMenuItem.Image = global::OfficeTools.Properties.Resources.DataSet;
            this.dataSourceItemToolStripMenuItem.Name = "dataSourceItemToolStripMenuItem";
            this.dataSourceItemToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.dataSourceItemToolStripMenuItem.Text = "DataSourceItem";
            // 
            // controlItemToolStripMenuItem
            // 
            this.controlItemToolStripMenuItem.Image = global::OfficeTools.Properties.Resources.property;
            this.controlItemToolStripMenuItem.Name = "controlItemToolStripMenuItem";
            this.controlItemToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.controlItemToolStripMenuItem.Text = "ControlItem";
            // 
            // systemParameterToolStripMenuItem
            // 
            this.systemParameterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.todayToolStripMenuItem,
            this.userIDToolStripMenuItem,
            this.userNameToolStripMenuItem,
            this.dataBaseToolStripMenuItem,
            this.solutionToolStripMenuItem,
            this.languageToolStripMenuItem,
            this.siteCodeToolStripMenuItem,
            this.ipAddressToolStripMenuItem});
            this.systemParameterToolStripMenuItem.Image = global::OfficeTools.Properties.Resources.property;
            this.systemParameterToolStripMenuItem.Name = "systemParameterToolStripMenuItem";
            this.systemParameterToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.systemParameterToolStripMenuItem.Text = "SystemParameter";
            // 
            // todayToolStripMenuItem
            // 
            this.todayToolStripMenuItem.Image = global::OfficeTools.Properties.Resources.property;
            this.todayToolStripMenuItem.Name = "todayToolStripMenuItem";
            this.todayToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.todayToolStripMenuItem.Tag = "_today";
            this.todayToolStripMenuItem.Text = "Today";
            this.todayToolStripMenuItem.Click += new System.EventHandler(this.stripMenuItem_Click);
            // 
            // userIDToolStripMenuItem
            // 
            this.userIDToolStripMenuItem.Image = global::OfficeTools.Properties.Resources.property;
            this.userIDToolStripMenuItem.Name = "userIDToolStripMenuItem";
            this.userIDToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.userIDToolStripMenuItem.Tag = "_userid";
            this.userIDToolStripMenuItem.Text = "UserID";
            this.userIDToolStripMenuItem.Click += new System.EventHandler(this.stripMenuItem_Click);
            // 
            // userNameToolStripMenuItem
            // 
            this.userNameToolStripMenuItem.Image = global::OfficeTools.Properties.Resources.property;
            this.userNameToolStripMenuItem.Name = "userNameToolStripMenuItem";
            this.userNameToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.userNameToolStripMenuItem.Tag = "_username";
            this.userNameToolStripMenuItem.Text = "UserName";
            this.userNameToolStripMenuItem.Click += new System.EventHandler(this.stripMenuItem_Click);
            // 
            // dataBaseToolStripMenuItem
            // 
            this.dataBaseToolStripMenuItem.Image = global::OfficeTools.Properties.Resources.property;
            this.dataBaseToolStripMenuItem.Name = "dataBaseToolStripMenuItem";
            this.dataBaseToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.dataBaseToolStripMenuItem.Tag = "_database";
            this.dataBaseToolStripMenuItem.Text = "DataBase";
            this.dataBaseToolStripMenuItem.Click += new System.EventHandler(this.stripMenuItem_Click);
            // 
            // solutionToolStripMenuItem
            // 
            this.solutionToolStripMenuItem.Image = global::OfficeTools.Properties.Resources.property;
            this.solutionToolStripMenuItem.Name = "solutionToolStripMenuItem";
            this.solutionToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.solutionToolStripMenuItem.Tag = "_solution";
            this.solutionToolStripMenuItem.Text = "Solution";
            this.solutionToolStripMenuItem.Click += new System.EventHandler(this.stripMenuItem_Click);
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.Image = global::OfficeTools.Properties.Resources.property;
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.languageToolStripMenuItem.Tag = "_language";
            this.languageToolStripMenuItem.Text = "Language";
            this.languageToolStripMenuItem.Click += new System.EventHandler(this.stripMenuItem_Click);
            // 
            // siteCodeToolStripMenuItem
            // 
            this.siteCodeToolStripMenuItem.Image = global::OfficeTools.Properties.Resources.property;
            this.siteCodeToolStripMenuItem.Name = "siteCodeToolStripMenuItem";
            this.siteCodeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.siteCodeToolStripMenuItem.Tag = "_sitecode";
            this.siteCodeToolStripMenuItem.Text = "SiteCode";
            this.siteCodeToolStripMenuItem.Click += new System.EventHandler(this.stripMenuItem_Click);
            // 
            // ipAddressToolStripMenuItem
            // 
            this.ipAddressToolStripMenuItem.Image = global::OfficeTools.Properties.Resources.property;
            this.ipAddressToolStripMenuItem.Name = "ipAddressToolStripMenuItem";
            this.ipAddressToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.ipAddressToolStripMenuItem.Tag = "_ipaddress";
            this.ipAddressToolStripMenuItem.Text = "IpAddress";
            this.ipAddressToolStripMenuItem.Click += new System.EventHandler(this.stripMenuItem_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(285, 154);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(366, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(245, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "right-click to insert expression script:";
            // 
            // frmValue
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(459, 187);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbScript);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MinimizeBox = false;
            this.Name = "frmValue";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Expression Editor";
            this.Load += new System.EventHandler(this.frmValue_Load);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox tbScript;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemInsert;
        private System.Windows.Forms.ToolStripMenuItem dataSourceItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem controlItemToolStripMenuItem;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolStripMenuItem systemParameterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem todayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataBaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem siteCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ipAddressToolStripMenuItem;
        private System.Windows.Forms.Label label1;
    }
}