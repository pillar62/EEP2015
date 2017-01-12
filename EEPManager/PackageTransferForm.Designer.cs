namespace EEPManager
{
    partial class PackageTransferForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageTransferForm));
            this.dgTransferState = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transferState = new System.Data.DataSet();
            this.transferTable = new System.Data.DataTable();
            this.columnDllName = new System.Data.DataColumn();
            this.columnTransferState = new System.Data.DataColumn();
            this.columnDateTime = new System.Data.DataColumn();
            this.btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgTransferState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transferState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transferTable)).BeginInit();
            this.SuspendLayout();
            // 
            // dgTransferState
            // 
            this.dgTransferState.AllowDrop = true;
            this.dgTransferState.AllowUserToAddRows = false;
            this.dgTransferState.AllowUserToDeleteRows = false;
            this.dgTransferState.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            this.dgTransferState.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgTransferState.AutoGenerateColumns = false;
            this.dgTransferState.BackgroundColor = System.Drawing.Color.Linen;
            this.dgTransferState.ColumnHeadersHeight = 25;
            this.dgTransferState.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.stateDataGridViewTextBoxColumn});
            this.dgTransferState.DataMember = "transfer";
            this.dgTransferState.DataSource = this.transferState;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgTransferState.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgTransferState.Location = new System.Drawing.Point(12, 12);
            this.dgTransferState.Name = "dgTransferState";
            this.dgTransferState.RowHeadersVisible = false;
            this.dgTransferState.RowTemplate.Height = 24;
            this.dgTransferState.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgTransferState.Size = new System.Drawing.Size(493, 220);
            this.dgTransferState.TabIndex = 8;
            this.dgTransferState.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgLocalPackage_CellBeginEdit);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "NAME";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.nameDataGridViewTextBoxColumn.Width = 160;
            // 
            // stateDataGridViewTextBoxColumn
            // 
            this.stateDataGridViewTextBoxColumn.DataPropertyName = "STATE";
            this.stateDataGridViewTextBoxColumn.HeaderText = "State";
            this.stateDataGridViewTextBoxColumn.Name = "stateDataGridViewTextBoxColumn";
            this.stateDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.stateDataGridViewTextBoxColumn.Width = 330;
            // 
            // transferState
            // 
            this.transferState.DataSetName = "NewDataSet";
            this.transferState.Tables.AddRange(new System.Data.DataTable[] {
            this.transferTable});
            // 
            // transferTable
            // 
            this.transferTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.columnDllName,
            this.columnTransferState,
            this.columnDateTime});
            this.transferTable.TableName = "transfer";
            // 
            // columnDllName
            // 
            this.columnDllName.ColumnName = "Name";
            // 
            // columnTransferState
            // 
            this.columnTransferState.ColumnName = "State";
            // 
            // columnDateTime
            // 
            this.columnDateTime.ColumnName = "DateTime";
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(186, 247);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(130, 23);
            this.btn.TabIndex = 9;
            this.btn.Text = "Start";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // PackageTransferForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 282);
            this.Controls.Add(this.btn);
            this.Controls.Add(this.dgTransferState);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PackageTransferForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Package Transfer State";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DllTransferForm_FormClosing);
            this.Load += new System.EventHandler(this.DllTransferForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgTransferState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transferState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transferTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgTransferState;
        private System.Data.DataSet transferState;
        private System.Data.DataTable transferTable;
        private System.Data.DataColumn columnDllName;
        private System.Data.DataColumn columnTransferState;
        private System.Data.DataColumn columnDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stateDataGridViewTextBoxColumn;
        public System.Windows.Forms.Button btn;
    }
}