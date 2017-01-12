namespace JQClientTools
{
    partial class MultiLanguageEditorDialog
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.iDENTIFICATIONDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kEYSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eNDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cHTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cHSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hKDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jADataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lAN1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lAN2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.dataColumnid = new System.Data.DataColumn();
            this.dataColumnIDENTIFICATION = new System.Data.DataColumn();
            this.dataColumnKEYS = new System.Data.DataColumn();
            this.dataColumnEN = new System.Data.DataColumn();
            this.dataColumnCHT = new System.Data.DataColumn();
            this.dataColumnCHS = new System.Data.DataColumn();
            this.dataColumnHK = new System.Data.DataColumn();
            this.dataColumnJA = new System.Data.DataColumn();
            this.dataColumnKO = new System.Data.DataColumn();
            this.dataColumnLAN1 = new System.Data.DataColumn();
            this.dataColumnLAN2 = new System.Data.DataColumn();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBoxDB = new System.Windows.Forms.GroupBox();
            this.buttonWriteDB = new System.Windows.Forms.Button();
            this.buttonReadDB = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            this.groupBoxDB.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonSelect);
            this.splitContainer1.Panel2.Controls.Add(this.buttonRefresh);
            this.splitContainer1.Panel2.Controls.Add(this.buttonOK);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxDB);
            this.splitContainer1.Panel2.Controls.Add(this.buttonCancel);
            this.splitContainer1.Size = new System.Drawing.Size(592, 366);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // dataGridView
            // 
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDENTIFICATIONDataGridViewTextBoxColumn,
            this.kEYSDataGridViewTextBoxColumn,
            this.eNDataGridViewTextBoxColumn,
            this.cHTDataGridViewTextBoxColumn,
            this.cHSDataGridViewTextBoxColumn,
            this.hKDataGridViewTextBoxColumn,
            this.jADataGridViewTextBoxColumn,
            this.KO,
            this.lAN1DataGridViewTextBoxColumn,
            this.lAN2DataGridViewTextBoxColumn});
            this.dataGridView.DataSource = this.bindingSource;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(592, 257);
            this.dataGridView.TabIndex = 0;
            // 
            // iDENTIFICATIONDataGridViewTextBoxColumn
            // 
            this.iDENTIFICATIONDataGridViewTextBoxColumn.DataPropertyName = "IDENTIFICATION";
            this.iDENTIFICATIONDataGridViewTextBoxColumn.HeaderText = "IDENTIFICATION";
            this.iDENTIFICATIONDataGridViewTextBoxColumn.Name = "iDENTIFICATIONDataGridViewTextBoxColumn";
            this.iDENTIFICATIONDataGridViewTextBoxColumn.Visible = false;
            // 
            // kEYSDataGridViewTextBoxColumn
            // 
            this.kEYSDataGridViewTextBoxColumn.DataPropertyName = "KEYS";
            this.kEYSDataGridViewTextBoxColumn.HeaderText = "KEYS";
            this.kEYSDataGridViewTextBoxColumn.Name = "kEYSDataGridViewTextBoxColumn";
            // 
            // eNDataGridViewTextBoxColumn
            // 
            this.eNDataGridViewTextBoxColumn.DataPropertyName = "EN";
            this.eNDataGridViewTextBoxColumn.HeaderText = "EN";
            this.eNDataGridViewTextBoxColumn.Name = "eNDataGridViewTextBoxColumn";
            // 
            // cHTDataGridViewTextBoxColumn
            // 
            this.cHTDataGridViewTextBoxColumn.DataPropertyName = "CHT";
            this.cHTDataGridViewTextBoxColumn.HeaderText = "CHT";
            this.cHTDataGridViewTextBoxColumn.Name = "cHTDataGridViewTextBoxColumn";
            // 
            // cHSDataGridViewTextBoxColumn
            // 
            this.cHSDataGridViewTextBoxColumn.DataPropertyName = "CHS";
            this.cHSDataGridViewTextBoxColumn.HeaderText = "CHS";
            this.cHSDataGridViewTextBoxColumn.Name = "cHSDataGridViewTextBoxColumn";
            // 
            // hKDataGridViewTextBoxColumn
            // 
            this.hKDataGridViewTextBoxColumn.DataPropertyName = "HK";
            this.hKDataGridViewTextBoxColumn.HeaderText = "HK";
            this.hKDataGridViewTextBoxColumn.Name = "hKDataGridViewTextBoxColumn";
            // 
            // jADataGridViewTextBoxColumn
            // 
            this.jADataGridViewTextBoxColumn.DataPropertyName = "JA";
            this.jADataGridViewTextBoxColumn.HeaderText = "JA";
            this.jADataGridViewTextBoxColumn.Name = "jADataGridViewTextBoxColumn";
            // 
            // KO
            // 
            this.KO.DataPropertyName = "KO";
            this.KO.HeaderText = "KO";
            this.KO.Name = "KO";
            // 
            // lAN1DataGridViewTextBoxColumn
            // 
            this.lAN1DataGridViewTextBoxColumn.DataPropertyName = "LAN1";
            this.lAN1DataGridViewTextBoxColumn.HeaderText = "LAN1";
            this.lAN1DataGridViewTextBoxColumn.Name = "lAN1DataGridViewTextBoxColumn";
            // 
            // lAN2DataGridViewTextBoxColumn
            // 
            this.lAN2DataGridViewTextBoxColumn.DataPropertyName = "LAN2";
            this.lAN2DataGridViewTextBoxColumn.HeaderText = "LAN2";
            this.lAN2DataGridViewTextBoxColumn.Name = "lAN2DataGridViewTextBoxColumn";
            // 
            // bindingSource
            // 
            this.bindingSource.DataMember = "Table";
            this.bindingSource.DataSource = this.dataSet;
            // 
            // dataSet
            // 
            this.dataSet.DataSetName = "dataSet";
            this.dataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1});
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumnid,
            this.dataColumnIDENTIFICATION,
            this.dataColumnKEYS,
            this.dataColumnEN,
            this.dataColumnCHT,
            this.dataColumnCHS,
            this.dataColumnHK,
            this.dataColumnJA,
            this.dataColumnKO,
            this.dataColumnLAN1,
            this.dataColumnLAN2});
            this.dataTable1.TableName = "Table";
            // 
            // dataColumnid
            // 
            this.dataColumnid.ColumnName = "ID";
            this.dataColumnid.DataType = typeof(int);
            // 
            // dataColumnIDENTIFICATION
            // 
            this.dataColumnIDENTIFICATION.ColumnName = "IDENTIFICATION";
            // 
            // dataColumnKEYS
            // 
            this.dataColumnKEYS.ColumnName = "KEYS";
            // 
            // dataColumnEN
            // 
            this.dataColumnEN.ColumnName = "EN";
            // 
            // dataColumnCHT
            // 
            this.dataColumnCHT.ColumnName = "CHT";
            // 
            // dataColumnCHS
            // 
            this.dataColumnCHS.ColumnName = "CHS";
            // 
            // dataColumnHK
            // 
            this.dataColumnHK.ColumnName = "HK";
            // 
            // dataColumnJA
            // 
            this.dataColumnJA.ColumnName = "JA";
            // 
            // dataColumnKO
            // 
            this.dataColumnKO.ColumnName = "KO";
            // 
            // dataColumnLAN1
            // 
            this.dataColumnLAN1.ColumnName = "LAN1";
            // 
            // dataColumnLAN2
            // 
            this.dataColumnLAN2.ColumnName = "LAN2";
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(12, 36);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(130, 23);
            this.buttonSelect.TabIndex = 2;
            this.buttonSelect.Text = "Select language";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(167, 36);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(130, 23);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "Refresh controls";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(502, 21);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBoxDB
            // 
            this.groupBoxDB.Controls.Add(this.buttonWriteDB);
            this.groupBoxDB.Controls.Add(this.buttonReadDB);
            this.groupBoxDB.Location = new System.Drawing.Point(316, 7);
            this.groupBoxDB.Name = "groupBoxDB";
            this.groupBoxDB.Size = new System.Drawing.Size(162, 75);
            this.groupBoxDB.TabIndex = 3;
            this.groupBoxDB.TabStop = false;
            this.groupBoxDB.Text = "DataBase";
            // 
            // buttonWriteDB
            // 
            this.buttonWriteDB.Location = new System.Drawing.Point(13, 51);
            this.buttonWriteDB.Name = "buttonWriteDB";
            this.buttonWriteDB.Size = new System.Drawing.Size(130, 23);
            this.buttonWriteDB.TabIndex = 1;
            this.buttonWriteDB.Text = "Write to dataBase";
            this.buttonWriteDB.UseVisualStyleBackColor = true;
            this.buttonWriteDB.Click += new System.EventHandler(this.buttonWriteDB_Click);
            // 
            // buttonReadDB
            // 
            this.buttonReadDB.Location = new System.Drawing.Point(14, 17);
            this.buttonReadDB.Name = "buttonReadDB";
            this.buttonReadDB.Size = new System.Drawing.Size(130, 23);
            this.buttonReadDB.TabIndex = 0;
            this.buttonReadDB.Text = "Read from dataBase";
            this.buttonReadDB.UseVisualStyleBackColor = true;
            this.buttonReadDB.Click += new System.EventHandler(this.buttonReadDB_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(502, 56);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "IDENTIFICATION";
            this.dataGridViewTextBoxColumn1.HeaderText = "IDENTIFICATION";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "KEYS";
            this.dataGridViewTextBoxColumn2.HeaderText = "KEYS";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "EN";
            this.dataGridViewTextBoxColumn3.HeaderText = "EN";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "CHT";
            this.dataGridViewTextBoxColumn4.HeaderText = "CHT";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "CHS";
            this.dataGridViewTextBoxColumn5.HeaderText = "CHS";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "HK";
            this.dataGridViewTextBoxColumn6.HeaderText = "HK";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "JA";
            this.dataGridViewTextBoxColumn7.HeaderText = "JA";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "KO";
            this.dataGridViewTextBoxColumn8.HeaderText = "KO";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "LAN1";
            this.dataGridViewTextBoxColumn9.HeaderText = "LAN1";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "LAN2";
            this.dataGridViewTextBoxColumn10.HeaderText = "LAN2";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // MultiLanguageEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 366);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MultiLanguageEditorDialog";
            this.Text = "BaseMultiLanguageEditorDialog";
            this.Load += new System.EventHandler(this.BaseMultiLanguageEditorDialog_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            this.groupBoxDB.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonReadDB;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.GroupBox groupBoxDB;
        private System.Data.DataSet dataSet;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Data.DataTable dataTable1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonWriteDB;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonSelect;
        private System.Data.DataColumn dataColumnIDENTIFICATION;
        private System.Data.DataColumn dataColumnKEYS;
        private System.Data.DataColumn dataColumnEN;
        private System.Data.DataColumn dataColumnCHT;
        private System.Data.DataColumn dataColumnCHS;
        private System.Data.DataColumn dataColumnHK;
        private System.Data.DataColumn dataColumnJA;
        private System.Data.DataColumn dataColumnKO;
        private System.Data.DataColumn dataColumnLAN1;
        private System.Data.DataColumn dataColumnLAN2;
        private System.Data.DataColumn dataColumnid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDENTIFICATIONDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn kEYSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eNDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cHTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cHSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hKDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jADataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn KO;
        private System.Windows.Forms.DataGridViewTextBoxColumn lAN1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lAN2DataGridViewTextBoxColumn;

    }
}