namespace Srvtools
{
    partial class frmAccessMenus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccessMenus));
            this.tvmenu = new System.Windows.Forms.TreeView();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbGroup = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmsGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.accessControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblGroupID = new System.Windows.Forms.Label();
            this.lblGroupName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSolution = new System.Windows.Forms.ComboBox();
            this.idsSolution = new Srvtools.InfoDataSet(this.components);
            this.idsMenu = new Srvtools.InfoDataSet(this.components);
            this.idsGroupMenu = new Srvtools.InfoDataSet(this.components);
            this.btnEqual = new System.Windows.Forms.Button();
            this.btnSelAll = new System.Windows.Forms.Button();
            this.btnCanAll = new System.Windows.Forms.Button();
            this.cmsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idsSolution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.idsMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.idsGroupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // tvmenu
            // 
            this.tvmenu.CheckBoxes = true;
            this.tvmenu.Location = new System.Drawing.Point(21, 120);
            this.tvmenu.Name = "tvmenu";
            this.tvmenu.Size = new System.Drawing.Size(242, 242);
            this.tvmenu.TabIndex = 0;
            this.tvmenu.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvmenu_NodeMouseClick);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(269, 309);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(73, 23);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(239, 47);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(41, 23);
            this.btnCopy.TabIndex = 3;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(269, 338);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbGroup
            // 
            this.cbGroup.FormattingEnabled = true;
            this.cbGroup.Location = new System.Drawing.Point(93, 49);
            this.cbGroup.Name = "cbGroup";
            this.cbGroup.Size = new System.Drawing.Size(140, 20);
            this.cbGroup.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "Solution:";
            // 
            // cmsGroup
            // 
            this.cmsGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accessControlsToolStripMenuItem});
            this.cmsGroup.Name = "cmsGroup";
            this.cmsGroup.Size = new System.Drawing.Size(169, 26);
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
            this.label2.Location = new System.Drawing.Point(38, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "GroupID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(183, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "GroupName:";
            // 
            // lblGroupID
            // 
            this.lblGroupID.AutoSize = true;
            this.lblGroupID.Location = new System.Drawing.Point(102, 20);
            this.lblGroupID.Name = "lblGroupID";
            this.lblGroupID.Size = new System.Drawing.Size(0, 12);
            this.lblGroupID.TabIndex = 10;
            // 
            // lblGroupName
            // 
            this.lblGroupName.AutoSize = true;
            this.lblGroupName.Location = new System.Drawing.Point(254, 20);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(0, 12);
            this.lblGroupName.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "Copy From:";
            // 
            // cbSolution
            // 
            this.cbSolution.DataSource = this.idsSolution;
            this.cbSolution.DisplayMember = "solutionInfo.ITEMNAME";
            this.cbSolution.FormattingEnabled = true;
            this.cbSolution.Location = new System.Drawing.Point(93, 84);
            this.cbSolution.Name = "cbSolution";
            this.cbSolution.Size = new System.Drawing.Size(140, 20);
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
            // idsGroupMenu
            // 
            this.idsGroupMenu.Active = false;
            this.idsGroupMenu.AlwaysClose = false;
            this.idsGroupMenu.DeleteIncomplete = true;
            this.idsGroupMenu.LastKeyValues = null;
            this.idsGroupMenu.PacketRecords = -1;
            this.idsGroupMenu.Position = -1;
            this.idsGroupMenu.RemoteName = "GLModule.sqlMGroupMenus";
            this.idsGroupMenu.ServerModify = false;
            // 
            // btnEqual
            // 
            this.btnEqual.Location = new System.Drawing.Point(286, 47);
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(50, 23);
            this.btnEqual.TabIndex = 13;
            this.btnEqual.Text = "Equal";
            this.btnEqual.UseVisualStyleBackColor = true;
            this.btnEqual.Click += new System.EventHandler(this.btnEqual_Click);
            // 
            // btnSelAll
            // 
            this.btnSelAll.Location = new System.Drawing.Point(269, 120);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(73, 23);
            this.btnSelAll.TabIndex = 14;
            this.btnSelAll.Text = "Select All";
            this.btnSelAll.UseVisualStyleBackColor = true;
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnCanAll
            // 
            this.btnCanAll.Location = new System.Drawing.Point(269, 149);
            this.btnCanAll.Name = "btnCanAll";
            this.btnCanAll.Size = new System.Drawing.Size(73, 23);
            this.btnCanAll.TabIndex = 15;
            this.btnCanAll.Text = "Cancel All";
            this.btnCanAll.UseVisualStyleBackColor = true;
            this.btnCanAll.Click += new System.EventHandler(this.btnCanAll_Click);
            // 
            // frmAccessMenus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 374);
            this.Controls.Add(this.btnCanAll);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.btnEqual);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblGroupID);
            this.Controls.Add(this.lblGroupName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbSolution);
            this.Controls.Add(this.cbGroup);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tvmenu);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnApply);
            this.Name = "frmAccessMenus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AccessMenus";
            this.Load += new System.EventHandler(this.frmAccessMenus_Load);
            this.cmsGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.idsSolution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.idsMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.idsGroupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvmenu;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbGroup;
        private InfoDataSet idsMenu;
        private InfoDataSet idsGroupMenu;
        private System.Windows.Forms.ComboBox cbSolution;
        private System.Windows.Forms.Label label1;
        private InfoDataSet idsSolution;
        private System.Windows.Forms.ContextMenuStrip cmsGroup;
        private System.Windows.Forms.ToolStripMenuItem accessControlsToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblGroupID;
        private System.Windows.Forms.Label lblGroupName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnEqual;
        private System.Windows.Forms.Button btnSelAll;
        private System.Windows.Forms.Button btnCanAll;
    }
}