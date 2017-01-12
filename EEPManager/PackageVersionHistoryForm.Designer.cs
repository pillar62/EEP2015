namespace EEPManager
{
    partial class PackageVersionHistoryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageVersionHistoryForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgVersionHistory = new System.Windows.Forms.DataGridView();
            this.ibsPackageVersion = new Srvtools.InfoBindingSource(this.components);
            this.idsPackageVersion = new Srvtools.InfoDataSet(this.components);
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnRollback = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbHistory = new System.Windows.Forms.Label();
            this.ColumnVerNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iTEMTYPEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pACKAGEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pACKAGEDATEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fILEDATEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgVersionHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsPackageVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.idsPackageVersion)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(348, 307);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgVersionHistory
            // 
            this.dgVersionHistory.AllowUserToAddRows = false;
            this.dgVersionHistory.AllowUserToDeleteRows = false;
            this.dgVersionHistory.AllowUserToOrderColumns = true;
            this.dgVersionHistory.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            this.dgVersionHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgVersionHistory.AutoGenerateColumns = false;
            this.dgVersionHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgVersionHistory.BackgroundColor = System.Drawing.Color.Linen;
            this.dgVersionHistory.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgVersionHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgVersionHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnVerNo,
            this.iTEMTYPEDataGridViewTextBoxColumn,
            this.pACKAGEDataGridViewTextBoxColumn,
            this.pACKAGEDATEDataGridViewTextBoxColumn,
            this.fILEDATEDataGridViewTextBoxColumn});
            this.dgVersionHistory.DataSource = this.ibsPackageVersion;
            this.dgVersionHistory.Location = new System.Drawing.Point(1, 43);
            this.dgVersionHistory.Name = "dgVersionHistory";
            this.dgVersionHistory.ReadOnly = true;
            this.dgVersionHistory.RowHeadersVisible = false;
            this.dgVersionHistory.RowTemplate.Height = 24;
            this.dgVersionHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgVersionHistory.Size = new System.Drawing.Size(502, 255);
            this.dgVersionHistory.TabIndex = 2;
            this.dgVersionHistory.SelectionChanged += new System.EventHandler(this.dgVersionHistory_SelectionChanged);
            // 
            // ibsPackageVersion
            // 
            this.ibsPackageVersion.AllowAdd = true;
            this.ibsPackageVersion.AllowDelete = true;
            this.ibsPackageVersion.AllowPrint = true;
            this.ibsPackageVersion.AllowUpdate = true;
            this.ibsPackageVersion.AutoApply = false;
            this.ibsPackageVersion.AutoApplyMaster = false;
            this.ibsPackageVersion.AutoDisableStyle = Srvtools.InfoBindingSource.AutoDisableStyleType.Enabled;
            this.ibsPackageVersion.AutoDisibleControl = false;
            this.ibsPackageVersion.AutoRecordLock = false;
            this.ibsPackageVersion.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload;
            this.ibsPackageVersion.CloseProtect = false;
            this.ibsPackageVersion.DataMember = "packageversion";
            this.ibsPackageVersion.DataSource = this.idsPackageVersion;
            this.ibsPackageVersion.DelayInterval = 300;
            this.ibsPackageVersion.DisableKeyFields = false;
            this.ibsPackageVersion.EnableFlag = false;
            this.ibsPackageVersion.FocusedControl = null;
            this.ibsPackageVersion.OwnerComp = null;
            this.ibsPackageVersion.Position = 0;
            this.ibsPackageVersion.RelationDelay = false;
            this.ibsPackageVersion.text = "ibsPackageVersion";
            // 
            // idsPackageVersion
            // 
            this.idsPackageVersion.Active = true;
            this.idsPackageVersion.AlwaysClose = false;
            this.idsPackageVersion.DeleteIncomplete = true;
            this.idsPackageVersion.LastKeyValues = null;
            this.idsPackageVersion.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.idsPackageVersion.PacketRecords = -1;
            this.idsPackageVersion.Position = -1;
            this.idsPackageVersion.RemoteName = "GLModule.packageversion";
            this.idsPackageVersion.ServerModify = false;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(47, 307);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 3;
            this.btnDownload.Text = "DownLoad";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnRollback
            // 
            this.btnRollback.Location = new System.Drawing.Point(200, 307);
            this.btnRollback.Name = "btnRollback";
            this.btnRollback.Size = new System.Drawing.Size(75, 23);
            this.btnRollback.TabIndex = 4;
            this.btnRollback.Text = "Rollback";
            this.btnRollback.UseVisualStyleBackColor = true;
            this.btnRollback.Click += new System.EventHandler(this.btnRollback_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "History:";
            // 
            // lbHistory
            // 
            this.lbHistory.AutoSize = true;
            this.lbHistory.Location = new System.Drawing.Point(69, 20);
            this.lbHistory.Name = "lbHistory";
            this.lbHistory.Size = new System.Drawing.Size(0, 12);
            this.lbHistory.TabIndex = 6;
            // 
            // ColumnVerNo
            // 
            this.ColumnVerNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = null;
            this.ColumnVerNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnVerNo.HeaderText = "Ver";
            this.ColumnVerNo.Name = "ColumnVerNo";
            this.ColumnVerNo.ReadOnly = true;
            this.ColumnVerNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnVerNo.Width = 40;
            // 
            // iTEMTYPEDataGridViewTextBoxColumn
            // 
            this.iTEMTYPEDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.iTEMTYPEDataGridViewTextBoxColumn.DataPropertyName = "ITEMTYPE";
            this.iTEMTYPEDataGridViewTextBoxColumn.HeaderText = "Solution";
            this.iTEMTYPEDataGridViewTextBoxColumn.Name = "iTEMTYPEDataGridViewTextBoxColumn";
            this.iTEMTYPEDataGridViewTextBoxColumn.ReadOnly = true;
            this.iTEMTYPEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // pACKAGEDataGridViewTextBoxColumn
            // 
            this.pACKAGEDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.pACKAGEDataGridViewTextBoxColumn.DataPropertyName = "FILENAME";
            this.pACKAGEDataGridViewTextBoxColumn.HeaderText = "Package";
            this.pACKAGEDataGridViewTextBoxColumn.Name = "pACKAGEDataGridViewTextBoxColumn";
            this.pACKAGEDataGridViewTextBoxColumn.ReadOnly = true;
            this.pACKAGEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // pACKAGEDATEDataGridViewTextBoxColumn
            // 
            this.pACKAGEDATEDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.pACKAGEDATEDataGridViewTextBoxColumn.DataPropertyName = "PACKAGEDATE";
            dataGridViewCellStyle3.Format = "G";
            this.pACKAGEDATEDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.pACKAGEDATEDataGridViewTextBoxColumn.HeaderText = "PackageDate";
            this.pACKAGEDATEDataGridViewTextBoxColumn.Name = "pACKAGEDATEDataGridViewTextBoxColumn";
            this.pACKAGEDATEDataGridViewTextBoxColumn.ReadOnly = true;
            this.pACKAGEDATEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.pACKAGEDATEDataGridViewTextBoxColumn.Width = 130;
            // 
            // fILEDATEDataGridViewTextBoxColumn
            // 
            this.fILEDATEDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.fILEDATEDataGridViewTextBoxColumn.DataPropertyName = "FILEDATE";
            dataGridViewCellStyle4.Format = "G";
            dataGridViewCellStyle4.NullValue = null;
            this.fILEDATEDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.fILEDATEDataGridViewTextBoxColumn.HeaderText = "FileDate";
            this.fILEDATEDataGridViewTextBoxColumn.Name = "fILEDATEDataGridViewTextBoxColumn";
            this.fILEDATEDataGridViewTextBoxColumn.ReadOnly = true;
            this.fILEDATEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.fILEDATEDataGridViewTextBoxColumn.Width = 130;
            // 
            // PackageVersionHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 339);
            this.Controls.Add(this.dgVersionHistory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbHistory);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnRollback);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PackageVersionHistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Package Version History";
            this.Load += new System.EventHandler(this.PackageVersionHistoryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgVersionHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibsPackageVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.idsPackageVersion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgVersionHistory;
        private Srvtools.InfoDataSet idsPackageVersion;
        private Srvtools.InfoBindingSource ibsPackageVersion;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnRollback;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVerNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn iTEMTYPEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pACKAGEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pACKAGEDATEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fILEDATEDataGridViewTextBoxColumn;
    }
}