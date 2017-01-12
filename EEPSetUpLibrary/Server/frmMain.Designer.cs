namespace EEPSetUpLibrary.Server
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showActiveUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHistoryUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelLink = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelShow = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerUsers = new System.Windows.Forms.SplitContainer();
            this.listViewActiveUser = new System.Windows.Forms.ListView();
            this.columnHeaderUser = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDateTime = new System.Windows.Forms.ColumnHeader();
            this.listViewHistoryUser = new System.Windows.Forms.ListView();
            this.columnHeaderUserH = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDateTimeH = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDescription = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.splitContainerUsers.Panel1.SuspendLayout();
            this.splitContainerUsers.Panel2.SuspendLayout();
            this.splitContainerUsers.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.windowsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(485, 24);
            this.menuStrip.TabIndex = 4;
            this.menuStrip.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("optionsToolStripMenuItem.Image")));
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.ToolTipText = "Set update options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.ToolTipText = "Restart service";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showActiveUserToolStripMenuItem,
            this.showHistoryUserToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.windowsToolStripMenuItem.Text = "Windows";
            // 
            // showActiveUserToolStripMenuItem
            // 
            this.showActiveUserToolStripMenuItem.Checked = true;
            this.showActiveUserToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showActiveUserToolStripMenuItem.Name = "showActiveUserToolStripMenuItem";
            this.showActiveUserToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.showActiveUserToolStripMenuItem.Text = "Show Active User";
            this.showActiveUserToolStripMenuItem.Click += new System.EventHandler(this.showActiveUserToolStripMenuItem_Click);
            // 
            // showHistoryUserToolStripMenuItem
            // 
            this.showHistoryUserToolStripMenuItem.CheckOnClick = true;
            this.showHistoryUserToolStripMenuItem.Name = "showHistoryUserToolStripMenuItem";
            this.showHistoryUserToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.showHistoryUserToolStripMenuItem.Text = "Show History User";
            this.showHistoryUserToolStripMenuItem.Click += new System.EventHandler(this.showHistoryUserToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelLink,
            this.toolStripStatusLabelShow});
            this.statusStrip.Location = new System.Drawing.Point(0, 254);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(485, 22);
            this.statusStrip.TabIndex = 5;
            // 
            // toolStripStatusLabelLink
            // 
            this.toolStripStatusLabelLink.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabelLink.Name = "toolStripStatusLabelLink";
            this.toolStripStatusLabelLink.Size = new System.Drawing.Size(78, 17);
            this.toolStripStatusLabelLink.Text = "Current Links:";
            // 
            // toolStripStatusLabelShow
            // 
            this.toolStripStatusLabelShow.Name = "toolStripStatusLabelShow";
            this.toolStripStatusLabelShow.Size = new System.Drawing.Size(62, 17);
            this.toolStripStatusLabelShow.Text = "Active User";
            // 
            // splitContainerUsers
            // 
            this.splitContainerUsers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerUsers.IsSplitterFixed = true;
            this.splitContainerUsers.Location = new System.Drawing.Point(0, 24);
            this.splitContainerUsers.Name = "splitContainerUsers";
            // 
            // splitContainerUsers.Panel1
            // 
            this.splitContainerUsers.Panel1.Controls.Add(this.listViewActiveUser);
            this.splitContainerUsers.Panel1MinSize = 0;
            // 
            // splitContainerUsers.Panel2
            // 
            this.splitContainerUsers.Panel2.Controls.Add(this.listViewHistoryUser);
            this.splitContainerUsers.Panel2MinSize = 0;
            this.splitContainerUsers.Size = new System.Drawing.Size(485, 230);
            this.splitContainerUsers.SplitterDistance = 481;
            this.splitContainerUsers.SplitterWidth = 1;
            this.splitContainerUsers.TabIndex = 6;
            // 
            // listViewActiveUser
            // 
            this.listViewActiveUser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUser,
            this.columnHeaderDateTime});
            this.listViewActiveUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewActiveUser.FullRowSelect = true;
            this.listViewActiveUser.GridLines = true;
            this.listViewActiveUser.Location = new System.Drawing.Point(0, 0);
            this.listViewActiveUser.Name = "listViewActiveUser";
            this.listViewActiveUser.Size = new System.Drawing.Size(477, 226);
            this.listViewActiveUser.TabIndex = 1;
            this.listViewActiveUser.UseCompatibleStateImageBehavior = false;
            this.listViewActiveUser.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderUser
            // 
            this.columnHeaderUser.Text = "User";
            this.columnHeaderUser.Width = 150;
            // 
            // columnHeaderDateTime
            // 
            this.columnHeaderDateTime.Text = "Login Time";
            this.columnHeaderDateTime.Width = 150;
            // 
            // listViewHistoryUser
            // 
            this.listViewHistoryUser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUserH,
            this.columnHeaderDateTimeH,
            this.columnHeaderDescription});
            this.listViewHistoryUser.ContextMenuStrip = this.contextMenuStrip;
            this.listViewHistoryUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewHistoryUser.FullRowSelect = true;
            this.listViewHistoryUser.GridLines = true;
            this.listViewHistoryUser.Location = new System.Drawing.Point(0, 0);
            this.listViewHistoryUser.Name = "listViewHistoryUser";
            this.listViewHistoryUser.Size = new System.Drawing.Size(3, 230);
            this.listViewHistoryUser.TabIndex = 0;
            this.listViewHistoryUser.UseCompatibleStateImageBehavior = false;
            this.listViewHistoryUser.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderUserH
            // 
            this.columnHeaderUserH.Text = "User";
            this.columnHeaderUserH.Width = 150;
            // 
            // columnHeaderDateTimeH
            // 
            this.columnHeaderDateTimeH.Text = "DateTime";
            this.columnHeaderDateTimeH.Width = 150;
            // 
            // columnHeaderDescription
            // 
            this.columnHeaderDescription.Text = "Description";
            this.columnHeaderDescription.Width = 150;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearHistoryToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(137, 26);
            // 
            // clearHistoryToolStripMenuItem
            // 
            this.clearHistoryToolStripMenuItem.Name = "clearHistoryToolStripMenuItem";
            this.clearHistoryToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.clearHistoryToolStripMenuItem.Text = "Clear History";
            this.clearHistoryToolStripMenuItem.Click += new System.EventHandler(this.clearHistoryToolStripMenuItem_Click);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 276);
            this.Controls.Add(this.splitContainerUsers);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EEPClient Update Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainerUsers.Panel1.ResumeLayout(false);
            this.splitContainerUsers.Panel2.ResumeLayout(false);
            this.splitContainerUsers.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelLink;
        private System.Windows.Forms.SplitContainer splitContainerUsers;
        private System.Windows.Forms.ListView listViewActiveUser;
        private System.Windows.Forms.ListView listViewHistoryUser;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showActiveUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHistoryUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeaderUser;
        private System.Windows.Forms.ColumnHeader columnHeaderDateTime;
        private System.Windows.Forms.ColumnHeader columnHeaderUserH;
        private System.Windows.Forms.ColumnHeader columnHeaderDateTimeH;
        private System.Windows.Forms.ColumnHeader columnHeaderDescription;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem clearHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelShow;

    }
}