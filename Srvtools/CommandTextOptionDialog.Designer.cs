namespace Srvtools
{
    partial class CommandTextOptionDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtCommandText = new System.Windows.Forms.TextBox();
            this.tabCommandText = new System.Windows.Forms.TabControl();
            this.tabPageCommandText = new System.Windows.Forms.TabPage();
            this.tabPageView = new System.Windows.Forms.TabPage();
            this.dataGridView = new System.Windows.Forms.DataGrid();
            this.listTables = new System.Windows.Forms.ListBox();
            this.listColumns = new System.Windows.Forms.ListBox();
            this.btnAddColumn = new System.Windows.Forms.Button();
            this.checkShowData = new System.Windows.Forms.CheckBox();
            this.btnAddTable = new System.Windows.Forms.Button();
            this.lblTables = new System.Windows.Forms.Label();
            this.lblColumns = new System.Windows.Forms.Label();
            this.btnAddAllColumn = new System.Windows.Forms.Button();
            this.btnCheckCommandText = new System.Windows.Forms.Button();
            this.lklNext = new System.Windows.Forms.LinkLabel();
            this.lblCount = new System.Windows.Forms.Label();
            this.tabCommandText.SuspendLayout();
            this.tabPageCommandText.SuspendLayout();
            this.tabPageView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(315, 450);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(396, 450);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtCommandText
            // 
            this.txtCommandText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommandText.Location = new System.Drawing.Point(3, 3);
            this.txtCommandText.Multiline = true;
            this.txtCommandText.Name = "txtCommandText";
            this.txtCommandText.Size = new System.Drawing.Size(452, 190);
            this.txtCommandText.TabIndex = 2;
            this.txtCommandText.TextChanged += new System.EventHandler(this.txtCommandText_TextChanged);
            // 
            // tabCommandText
            // 
            this.tabCommandText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCommandText.Controls.Add(this.tabPageCommandText);
            this.tabCommandText.Controls.Add(this.tabPageView);
            this.tabCommandText.Location = new System.Drawing.Point(13, 223);
            this.tabCommandText.Name = "tabCommandText";
            this.tabCommandText.SelectedIndex = 0;
            this.tabCommandText.Size = new System.Drawing.Size(466, 221);
            this.tabCommandText.TabIndex = 3;
            this.tabCommandText.SelectedIndexChanged += new System.EventHandler(this.tabCommandText_SelectedIndexChanged);
            // 
            // tabPageCommandText
            // 
            this.tabPageCommandText.Controls.Add(this.txtCommandText);
            this.tabPageCommandText.Location = new System.Drawing.Point(4, 21);
            this.tabPageCommandText.Name = "tabPageCommandText";
            this.tabPageCommandText.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCommandText.Size = new System.Drawing.Size(458, 196);
            this.tabPageCommandText.TabIndex = 0;
            this.tabPageCommandText.Text = "Command Text";
            this.tabPageCommandText.UseVisualStyleBackColor = true;
            // 
            // tabPageView
            // 
            this.tabPageView.Controls.Add(this.lblCount);
            this.tabPageView.Controls.Add(this.lklNext);
            this.tabPageView.Controls.Add(this.dataGridView);
            this.tabPageView.Location = new System.Drawing.Point(4, 21);
            this.tabPageView.Name = "tabPageView";
            this.tabPageView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageView.Size = new System.Drawing.Size(458, 196);
            this.tabPageView.TabIndex = 1;
            this.tabPageView.Text = "View";
            this.tabPageView.UseVisualStyleBackColor = true;
            // 
            // dataGridView
            // 
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.DataMember = "";
            this.dataGridView.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(452, 170);
            this.dataGridView.TabIndex = 0;
            // 
            // listTables
            // 
            this.listTables.FormattingEnabled = true;
            this.listTables.ItemHeight = 12;
            this.listTables.Location = new System.Drawing.Point(13, 26);
            this.listTables.Name = "listTables";
            this.listTables.Size = new System.Drawing.Size(193, 148);
            this.listTables.TabIndex = 4;
            this.listTables.SelectedIndexChanged += new System.EventHandler(this.listTables_SelectedIndexChanged);
            // 
            // listColumns
            // 
            this.listColumns.AllowDrop = true;
            this.listColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listColumns.FormattingEnabled = true;
            this.listColumns.ItemHeight = 12;
            this.listColumns.Location = new System.Drawing.Point(225, 26);
            this.listColumns.MultiColumn = true;
            this.listColumns.Name = "listColumns";
            this.listColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listColumns.Size = new System.Drawing.Size(254, 172);
            this.listColumns.TabIndex = 5;
            // 
            // btnAddColumn
            // 
            this.btnAddColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddColumn.Location = new System.Drawing.Point(396, 207);
            this.btnAddColumn.Name = "btnAddColumn";
            this.btnAddColumn.Size = new System.Drawing.Size(75, 23);
            this.btnAddColumn.TabIndex = 6;
            this.btnAddColumn.Text = "Add";
            this.btnAddColumn.UseVisualStyleBackColor = true;
            this.btnAddColumn.Click += new System.EventHandler(this.btnAddColumn_Click);
            // 
            // checkShowData
            // 
            this.checkShowData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkShowData.AutoSize = true;
            this.checkShowData.Location = new System.Drawing.Point(224, 221);
            this.checkShowData.Name = "checkShowData";
            this.checkShowData.Size = new System.Drawing.Size(78, 16);
            this.checkShowData.TabIndex = 7;
            this.checkShowData.Text = "Show Data";
            this.checkShowData.UseVisualStyleBackColor = true;
            this.checkShowData.CheckedChanged += new System.EventHandler(this.checkShowData_CheckedChanged);
            // 
            // btnAddTable
            // 
            this.btnAddTable.Location = new System.Drawing.Point(20, 183);
            this.btnAddTable.Name = "btnAddTable";
            this.btnAddTable.Size = new System.Drawing.Size(75, 23);
            this.btnAddTable.TabIndex = 8;
            this.btnAddTable.Text = "Add";
            this.btnAddTable.UseVisualStyleBackColor = true;
            this.btnAddTable.Click += new System.EventHandler(this.btnAddTable_Click);
            // 
            // lblTables
            // 
            this.lblTables.AutoSize = true;
            this.lblTables.Location = new System.Drawing.Point(11, 9);
            this.lblTables.Name = "lblTables";
            this.lblTables.Size = new System.Drawing.Size(47, 12);
            this.lblTables.TabIndex = 9;
            this.lblTables.Text = "Tables:";
            // 
            // lblColumns
            // 
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(223, 9);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(53, 12);
            this.lblColumns.TabIndex = 10;
            this.lblColumns.Text = "Columns:";
            // 
            // btnAddAllColumn
            // 
            this.btnAddAllColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAllColumn.Location = new System.Drawing.Point(316, 207);
            this.btnAddAllColumn.Name = "btnAddAllColumn";
            this.btnAddAllColumn.Size = new System.Drawing.Size(75, 23);
            this.btnAddAllColumn.TabIndex = 11;
            this.btnAddAllColumn.Text = "Add *";
            this.btnAddAllColumn.UseVisualStyleBackColor = true;
            this.btnAddAllColumn.Click += new System.EventHandler(this.btnAddAllColumn_Click);
            // 
            // btnCheckCommandText
            // 
            this.btnCheckCommandText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheckCommandText.Location = new System.Drawing.Point(20, 450);
            this.btnCheckCommandText.Name = "btnCheckCommandText";
            this.btnCheckCommandText.Size = new System.Drawing.Size(132, 23);
            this.btnCheckCommandText.TabIndex = 12;
            this.btnCheckCommandText.Text = "Check CommandText";
            this.btnCheckCommandText.UseVisualStyleBackColor = true;
            this.btnCheckCommandText.Click += new System.EventHandler(this.btnCheckCommandText_Click);
            // 
            // lklNext
            // 
            this.lklNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lklNext.AutoSize = true;
            this.lklNext.Location = new System.Drawing.Point(387, 180);
            this.lklNext.Name = "lklNext";
            this.lklNext.Size = new System.Drawing.Size(59, 12);
            this.lklNext.TabIndex = 2;
            this.lklNext.TabStop = true;
            this.lklNext.Text = "Next 1000";
            this.lklNext.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lklNext_LinkClicked);
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(263, 180);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(41, 12);
            this.lblCount.TabIndex = 3;
            this.lblCount.Text = "Count:";
            // 
            // CommandTextOptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 484);
            this.Controls.Add(this.btnCheckCommandText);
            this.Controls.Add(this.btnAddAllColumn);
            this.Controls.Add(this.lblColumns);
            this.Controls.Add(this.lblTables);
            this.Controls.Add(this.btnAddTable);
            this.Controls.Add(this.checkShowData);
            this.Controls.Add(this.btnAddColumn);
            this.Controls.Add(this.listColumns);
            this.Controls.Add(this.listTables);
            this.Controls.Add(this.tabCommandText);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommandTextOptionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CommandText Editor";
            this.Load += new System.EventHandler(this.CommandTextOptionDialog_Load);
            this.tabCommandText.ResumeLayout(false);
            this.tabPageCommandText.ResumeLayout(false);
            this.tabPageCommandText.PerformLayout();
            this.tabPageView.ResumeLayout(false);
            this.tabPageView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtCommandText;
        private System.Windows.Forms.TabControl tabCommandText;
        private System.Windows.Forms.TabPage tabPageCommandText;
        private System.Windows.Forms.TabPage tabPageView;
        private System.Windows.Forms.ListBox listTables;
        private System.Windows.Forms.ListBox listColumns;
        private System.Windows.Forms.Button btnAddColumn;
        private System.Windows.Forms.CheckBox checkShowData;
        private System.Windows.Forms.Button btnAddTable;
        private System.Windows.Forms.Label lblTables;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.DataGrid dataGridView;
        private System.Windows.Forms.Button btnAddAllColumn;
        private System.Windows.Forms.Button btnCheckCommandText;
        private System.Windows.Forms.LinkLabel lklNext;
        private System.Windows.Forms.Label lblCount;
    }
}