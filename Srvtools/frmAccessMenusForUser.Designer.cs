namespace Srvtools
{
    partial class frmAccessMenusForUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccessMenusForUser));
            this.tvmenu = new System.Windows.Forms.TreeView();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbUser = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmsUser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.accessControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblUserID = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnEqual = new System.Windows.Forms.Button();
            this.btnSelAll = new System.Windows.Forms.Button();
            this.cbSolution = new System.Windows.Forms.ComboBox();
            this.idsSolution = new Srvtools.InfoDataSet(this.components);
            this.idsMenu = new Srvtools.InfoDataSet(this.components);
            this.idsUserMenu = new Srvtools.InfoDataSet(this.components);
            this.btnCanAll = new System.Windows.Forms.Button();
            this.cmsUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idsSolution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.idsMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.idsUserMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // tvmenu
            // 
            this.tvmenu.CheckBoxes = true;
            this.tvmenu.Location = new System.Drawing.Point(26, 117);
            this.tvmenu.Name = "tvmenu";
            this.tvmenu.Size = new System.Drawing.Size(238, 257);
            this.tvmenu.TabIndex = 0;
            this.tvmenu.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvmenu_NodeMouseClick);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(275, 320);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(76, 23);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(258, 52);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(37, 23);
            this.btnCopy.TabIndex = 3;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(275, 349);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbUser
            // 
            this.cbUser.FormattingEnabled = true;
            this.cbUser.Location = new System.Drawing.Point(101, 54);
            this.cbUser.Name = "cbUser";
            this.cbUser.Size = new System.Drawing.Size(151, 20);
            this.cbUser.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "Solution:";
            // 
            // cmsUser
            // 
            this.cmsUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accessControlsToolStripMenuItem});
            this.cmsUser.Name = "cmsUser";
            this.cmsUser.Size = new System.Drawing.Size(169, 26);
            // 
            // accessControlsToolStripMenuItem
            // 
            this.accessControlsToolStripMenuItem.Name = "accessControlsToolStripMenuItem";
            this.accessControlsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.accessControlsToolStripMenuItem.Text = "Access Controls";
            this.accessControlsToolStripMenuItem.Click += new System.EventHandler(this.accessControlsToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "UserID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(193, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "UserName:";
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.Location = new System.Drawing.Point(113, 26);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(0, 12);
            this.lblUserID.TabIndex = 11;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(264, 26);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(0, 12);
            this.lblUserName.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "Copy From:";
            // 
            // btnEqual
            // 
            this.btnEqual.Location = new System.Drawing.Point(301, 52);
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(50, 23);
            this.btnEqual.TabIndex = 14;
            this.btnEqual.Text = "Equal";
            this.btnEqual.UseVisualStyleBackColor = true;
            this.btnEqual.Click += new System.EventHandler(this.btnEqual_Click);
            // 
            // btnSelAll
            // 
            this.btnSelAll.Location = new System.Drawing.Point(275, 117);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(76, 23);
            this.btnSelAll.TabIndex = 15;
            this.btnSelAll.Text = "Select All";
            this.btnSelAll.UseVisualStyleBackColor = true;
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // cbSolution
            // 
            this.cbSolution.DataSource = this.idsSolution;
            this.cbSolution.DisplayMember = "solutionInfo.ITEMNAME";
            this.cbSolution.FormattingEnabled = true;
            this.cbSolution.Location = new System.Drawing.Point(101, 86);
            this.cbSolution.Name = "cbSolution";
            this.cbSolution.Size = new System.Drawing.Size(151, 20);
            this.cbSolution.TabIndex = 6;
            this.cbSolution.ValueMember = "solutionInfo.ITEMTYPE";
            this.cbSolution.SelectedIndexChanged += new System.EventHandler(this.cbSolution_SelectedIndexChanged);
            // 
            // idsSolution
            // 
            this.idsSolution.Active = true;
            this.idsSolution.AlwaysClose = false;
            this.idsSolution.DeleteIncomplete = true;
            this.idsSolution.LastKeyValues = null;
            this.idsSolution.PacketRecords = -1;
            this.idsSolution.Position = -1;
            this.idsSolution.RemoteName = "GLModule.solutionInfo";
            this.idsSolution.ServerModify = false;
            // 
            // idsMenu
            // 
            this.idsMenu.Active = true;
            this.idsMenu.AlwaysClose = false;
            this.idsMenu.DeleteIncomplete = true;
            this.idsMenu.LastKeyValues = null;
            this.idsMenu.PacketRecords = -1;
            this.idsMenu.Position = -1;
            this.idsMenu.RemoteName = "GLModule.sqlMenus";
            this.idsMenu.ServerModify = false;
            // 
            // idsUserMenu
            // 
            this.idsUserMenu.Active = true;
            this.idsUserMenu.AlwaysClose = false;
            this.idsUserMenu.DeleteIncomplete = true;
            this.idsUserMenu.LastKeyValues = null;
            this.idsUserMenu.PacketRecords = -1;
            this.idsUserMenu.Position = -1;
            this.idsUserMenu.RemoteName = "GLModule.userMenus";
            this.idsUserMenu.ServerModify = false;
            // 
            // btnCanAll
            // 
            this.btnCanAll.Location = new System.Drawing.Point(275, 155);
            this.btnCanAll.Name = "btnCanAll";
            this.btnCanAll.Size = new System.Drawing.Size(76, 23);
            this.btnCanAll.TabIndex = 16;
            this.btnCanAll.Text = "Cancel All";
            this.btnCanAll.UseVisualStyleBackColor = true;
            this.btnCanAll.Click += new System.EventHandler(this.btnCanAll_Click);
            // 
            // frmAccessMenusForUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 386);
            this.Controls.Add(this.btnCanAll);
            this.Controls.Add(this.btnEqual);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbUser);
            this.Controls.Add(this.cbSolution);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblUserID);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.tvmenu);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.Name = "frmAccessMenusForUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AccessMenus";
            this.Load += new System.EventHandler(this.frmAccessMenusForUser_Load);
            this.cmsUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.idsSolution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.idsMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.idsUserMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvmenu;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbUser;
        private InfoDataSet idsMenu;
        private InfoDataSet idsUserMenu;
        private System.Windows.Forms.ComboBox cbSolution;
        private System.Windows.Forms.Label label1;
        private InfoDataSet idsSolution;
        private System.Windows.Forms.ContextMenuStrip cmsUser;
        private System.Windows.Forms.ToolStripMenuItem accessControlsToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnEqual;
        private System.Windows.Forms.Button btnSelAll;
        private System.Windows.Forms.Button btnCanAll;
    }
}