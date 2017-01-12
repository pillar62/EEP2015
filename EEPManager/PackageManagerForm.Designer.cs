namespace EEPManager
{
    partial class PackageManagerForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageManagerForm));
            this.cbxPackageType = new System.Windows.Forms.ComboBox();
            this.trvSolution = new System.Windows.Forms.TreeView();
            this.localPackage = new System.Data.DataSet();
            this.tableState = new System.Data.DataTable();
            this.columnDllName = new System.Data.DataColumn();
            this.columnDateTime = new System.Data.DataColumn();
            this.lblSolution = new System.Windows.Forms.Label();
            this.lblRemotePackage = new System.Windows.Forms.Label();
            this.lblLocalPackage = new System.Windows.Forms.Label();
            this.dgLocalPackage = new System.Windows.Forms.DataGridView();
            this.contextMenuStripVersionManager = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.versionHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgRemotePackage = new System.Windows.Forms.DataGridView();
            this.dgvremotepackage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvremotepackagedate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvremotefiledate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ibsRemodePackage = new Srvtools.InfoBindingSource(this.components);
            this.remotePackage = new Srvtools.InfoDataSet(this.components);
            this.solution = new Srvtools.InfoDataSet(this.components);
            this.dgvlocalpackage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvlocalfiledate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.localPackage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgLocalPackage)).BeginInit();
            this.contextMenuStripVersionManager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRemotePackage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsRemodePackage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.remotePackage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.solution)).BeginInit();
            this.SuspendLayout();
            // 
            // cbxPackageType
            // 
            this.cbxPackageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPackageType.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbxPackageType.FormattingEnabled = true;
            this.cbxPackageType.Items.AddRange(new object[] {
            "Server Package",
            "Client Package",
            "Web Client Package"});
            this.cbxPackageType.Location = new System.Drawing.Point(12, 30);
            this.cbxPackageType.Name = "cbxPackageType";
            this.cbxPackageType.Size = new System.Drawing.Size(134, 24);
            this.cbxPackageType.TabIndex = 0;
            this.cbxPackageType.SelectedIndexChanged += new System.EventHandler(this.cbxPackageType_SelectedIndexChanged);
            // 
            // trvSolution
            // 
            this.trvSolution.Location = new System.Drawing.Point(12, 86);
            this.trvSolution.Name = "trvSolution";
            this.trvSolution.Size = new System.Drawing.Size(134, 306);
            this.trvSolution.TabIndex = 1;
            this.trvSolution.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvSolution_AfterSelect);
            this.trvSolution.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvSolution_BeforeSelect);
            // 
            // localPackage
            // 
            this.localPackage.DataSetName = "NewDataSet";
            this.localPackage.Tables.AddRange(new System.Data.DataTable[] {
            this.tableState});
            // 
            // tableState
            // 
            this.tableState.Columns.AddRange(new System.Data.DataColumn[] {
            this.columnDllName,
            this.columnDateTime});
            this.tableState.TableName = "package";
            // 
            // columnDllName
            // 
            this.columnDllName.ColumnName = "Name";
            // 
            // columnDateTime
            // 
            this.columnDateTime.ColumnName = "DateTime";
            // 
            // lblSolution
            // 
            this.lblSolution.AutoSize = true;
            this.lblSolution.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblSolution.Location = new System.Drawing.Point(12, 57);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(60, 16);
            this.lblSolution.TabIndex = 4;
            this.lblSolution.Text = "Solution";
            // 
            // lblRemotePackage
            // 
            this.lblRemotePackage.AutoSize = true;
            this.lblRemotePackage.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRemotePackage.Location = new System.Drawing.Point(149, 57);
            this.lblRemotePackage.Name = "lblRemotePackage";
            this.lblRemotePackage.Size = new System.Drawing.Size(112, 16);
            this.lblRemotePackage.TabIndex = 5;
            this.lblRemotePackage.Text = "Remote Package";
            // 
            // lblLocalPackage
            // 
            this.lblLocalPackage.AutoSize = true;
            this.lblLocalPackage.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblLocalPackage.Location = new System.Drawing.Point(518, 57);
            this.lblLocalPackage.Name = "lblLocalPackage";
            this.lblLocalPackage.Size = new System.Drawing.Size(99, 16);
            this.lblLocalPackage.TabIndex = 6;
            this.lblLocalPackage.Text = "Local Package";
            // 
            // dgLocalPackage
            // 
            this.dgLocalPackage.AllowDrop = true;
            this.dgLocalPackage.AllowUserToAddRows = false;
            this.dgLocalPackage.AllowUserToDeleteRows = false;
            this.dgLocalPackage.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            this.dgLocalPackage.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgLocalPackage.AutoGenerateColumns = false;
            this.dgLocalPackage.BackgroundColor = System.Drawing.Color.Linen;
            this.dgLocalPackage.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgLocalPackage.ColumnHeadersHeight = 25;
            this.dgLocalPackage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvlocalpackage,
            this.dgvlocalfiledate});
            this.dgLocalPackage.DataMember = "package";
            this.dgLocalPackage.DataSource = this.localPackage;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgLocalPackage.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgLocalPackage.Location = new System.Drawing.Point(521, 86);
            this.dgLocalPackage.Name = "dgLocalPackage";
            this.dgLocalPackage.RowHeadersVisible = false;
            this.dgLocalPackage.RowTemplate.Height = 24;
            this.dgLocalPackage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgLocalPackage.Size = new System.Drawing.Size(235, 306);
            this.dgLocalPackage.TabIndex = 7;
            this.dgLocalPackage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgLocalPackage_MouseDown);
            this.dgLocalPackage.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dgLocalPackage.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgLocalPackage_DragEnter);
            this.dgLocalPackage.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgLocalPackage_DragDrop);
            // 
            // contextMenuStripVersionManager
            // 
            this.contextMenuStripVersionManager.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versionHistoryToolStripMenuItem});
            this.contextMenuStripVersionManager.Name = "contextMenuStripVersionManager";
            this.contextMenuStripVersionManager.Size = new System.Drawing.Size(147, 26);
            // 
            // versionHistoryToolStripMenuItem
            // 
            this.versionHistoryToolStripMenuItem.Name = "versionHistoryToolStripMenuItem";
            this.versionHistoryToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.versionHistoryToolStripMenuItem.Text = "Version History";
            this.versionHistoryToolStripMenuItem.Click += new System.EventHandler(this.versionHistoryToolStripMenuItem_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "NAME";
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "DATETIME";
            dataGridViewCellStyle3.Format = "G";
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn2.HeaderText = "DateTime";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dgRemotePackage
            // 
            this.dgRemotePackage.AllowDrop = true;
            this.dgRemotePackage.AllowUserToAddRows = false;
            this.dgRemotePackage.AllowUserToDeleteRows = false;
            this.dgRemotePackage.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Blue;
            this.dgRemotePackage.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgRemotePackage.AutoGenerateColumns = false;
            this.dgRemotePackage.BackgroundColor = System.Drawing.Color.Linen;
            this.dgRemotePackage.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgRemotePackage.ColumnHeadersHeight = 25;
            this.dgRemotePackage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvremotepackage,
            this.dgvremotepackagedate,
            this.dgvremotefiledate});
            this.dgRemotePackage.ContextMenuStrip = this.contextMenuStripVersionManager;
            this.dgRemotePackage.DataSource = this.ibsRemodePackage;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgRemotePackage.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgRemotePackage.Location = new System.Drawing.Point(152, 86);
            this.dgRemotePackage.Name = "dgRemotePackage";
            this.dgRemotePackage.RowHeadersVisible = false;
            this.dgRemotePackage.RowTemplate.Height = 24;
            this.dgRemotePackage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgRemotePackage.Size = new System.Drawing.Size(363, 306);
            this.dgRemotePackage.TabIndex = 2;
            this.dgRemotePackage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgRemotePackage_MouseDown);
            this.dgRemotePackage.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgServerPackage_CellBeginEdit);
            this.dgRemotePackage.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgRemotePackage_DragEnter);
            this.dgRemotePackage.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgRemotePackage_DragDrop);
            this.dgRemotePackage.SelectionChanged += new System.EventHandler(this.dgRemotePackage_SelectionChanged);
            // 
            // dgvremotepackage
            // 
            this.dgvremotepackage.DataPropertyName = "FILENAME";
            this.dgvremotepackage.HeaderText = "FILENAME";
            this.dgvremotepackage.Name = "dgvremotepackage";
            // 
            // dgvremotepackagedate
            // 
            this.dgvremotepackagedate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvremotepackagedate.DataPropertyName = "PACKAGEDATE";
            dataGridViewCellStyle5.Format = "G";
            this.dgvremotepackagedate.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvremotepackagedate.HeaderText = "Package Date";
            this.dgvremotepackagedate.Name = "dgvremotepackagedate";
            this.dgvremotepackagedate.Width = 130;
            // 
            // dgvremotefiledate
            // 
            this.dgvremotefiledate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvremotefiledate.DataPropertyName = "FILEDATE";
            dataGridViewCellStyle6.Format = "G";
            this.dgvremotefiledate.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvremotefiledate.HeaderText = "File Date";
            this.dgvremotefiledate.Name = "dgvremotefiledate";
            this.dgvremotefiledate.Width = 130;
            // 
            // ibsRemodePackage
            // 
            this.ibsRemodePackage.AllowAdd = true;
            this.ibsRemodePackage.AllowDelete = true;
            this.ibsRemodePackage.AllowPrint = true;
            this.ibsRemodePackage.AllowUpdate = true;
            this.ibsRemodePackage.AutoApply = false;
            this.ibsRemodePackage.AutoApplyMaster = false;
            this.ibsRemodePackage.AutoDisableStyle = Srvtools.InfoBindingSource.AutoDisableStyleType.Enabled;
            this.ibsRemodePackage.AutoDisibleControl = true;
            this.ibsRemodePackage.AutoRecordLock = false;
            this.ibsRemodePackage.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload;
            this.ibsRemodePackage.CloseProtect = false;
            this.ibsRemodePackage.DataMember = "packageInfo";
            this.ibsRemodePackage.DataSource = this.remotePackage;
            this.ibsRemodePackage.DelayInterval = 300;
            this.ibsRemodePackage.DisableKeyFields = false;
            this.ibsRemodePackage.EnableFlag = false;
            this.ibsRemodePackage.FocusedControl = null;
            this.ibsRemodePackage.OwnerComp = null;
            this.ibsRemodePackage.Position = 0;
            this.ibsRemodePackage.RelationDelay = false;
            this.ibsRemodePackage.text = "ibsRemodePackage";
            // 
            // remotePackage
            // 
            this.remotePackage.Active = true;
            this.remotePackage.AlwaysClose = false;
            this.remotePackage.DeleteIncomplete = true;
            this.remotePackage.LastKeyValues = null;
            this.remotePackage.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.remotePackage.PacketRecords = -1;
            this.remotePackage.Position = -1;
            this.remotePackage.RemoteName = "GLModule.packageInfo";
            this.remotePackage.ServerModify = false;
            // 
            // solution
            // 
            this.solution.Active = true;
            this.solution.AlwaysClose = false;
            this.solution.DeleteIncomplete = true;
            this.solution.LastKeyValues = null;
            this.solution.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.solution.PacketRecords = -1;
            this.solution.Position = -1;
            this.solution.RemoteName = "GLModule.solutionInfo";
            this.solution.ServerModify = false;
            // 
            // dgvlocalpackage
            // 
            this.dgvlocalpackage.DataPropertyName = "Name";
            this.dgvlocalpackage.HeaderText = "Name";
            this.dgvlocalpackage.Name = "dgvlocalpackage";
            // 
            // dgvlocalfiledate
            // 
            this.dgvlocalfiledate.DataPropertyName = "DateTime";
            this.dgvlocalfiledate.HeaderText = "DateTime";
            this.dgvlocalfiledate.Name = "dgvlocalfiledate";
            // 
            // PackageManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 412);
            this.Controls.Add(this.lblRemotePackage);
            this.Controls.Add(this.lblLocalPackage);
            this.Controls.Add(this.lblSolution);
            this.Controls.Add(this.trvSolution);
            this.Controls.Add(this.dgRemotePackage);
            this.Controls.Add(this.cbxPackageType);
            this.Controls.Add(this.dgLocalPackage);
            this.Name = "PackageManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Package Manager";
            this.Load += new System.EventHandler(this.PackageManagerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.localPackage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgLocalPackage)).EndInit();
            this.contextMenuStripVersionManager.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgRemotePackage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsRemodePackage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.remotePackage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.solution)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxPackageType;
        private System.Windows.Forms.TreeView trvSolution;
        private System.Windows.Forms.DataGridView dgRemotePackage;
        private System.Data.DataSet localPackage;
        private Srvtools.InfoDataSet solution;
        private System.Windows.Forms.Label lblSolution;
        private System.Windows.Forms.Label lblRemotePackage;
        private System.Windows.Forms.Label lblLocalPackage;
        private Srvtools.InfoDataSet remotePackage;
        private System.Windows.Forms.DataGridView dgLocalPackage;
        private System.Data.DataTable tableState;
        private System.Data.DataColumn columnDllName;
        private System.Data.DataColumn columnDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripVersionManager;
        private System.Windows.Forms.ToolStripMenuItem versionHistoryToolStripMenuItem;
        private Srvtools.InfoBindingSource ibsRemodePackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvremotepackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvremotepackagedate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvremotefiledate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvlocalpackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvlocalfiledate;
    }
}