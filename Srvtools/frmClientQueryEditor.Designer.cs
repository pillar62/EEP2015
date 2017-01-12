namespace Srvtools
{
    partial class frmClientQueryEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbbindingsource = new System.Windows.Forms.ComboBox();
            this.dgvClientQuery = new System.Windows.Forms.DataGridView();
            this.btnPreview = new System.Windows.Forms.Button();
            this.Caption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DefaultValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Condition = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Operator = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.NewLine = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TextAlign = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TextWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InfoRefVal = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ExternalRefVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientQuery)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(489, 311);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(580, 311);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "BindingSource";
            // 
            // cmbbindingsource
            // 
            this.cmbbindingsource.FormattingEnabled = true;
            this.cmbbindingsource.Location = new System.Drawing.Point(110, 12);
            this.cmbbindingsource.Name = "cmbbindingsource";
            this.cmbbindingsource.Size = new System.Drawing.Size(121, 20);
            this.cmbbindingsource.TabIndex = 3;
            this.cmbbindingsource.TextChanged += new System.EventHandler(this.cmbbindingsource_TextChanged);
            // 
            // dgvClientQuery
            // 
            this.dgvClientQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvClientQuery.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvClientQuery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientQuery.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Caption,
            this.Column,
            this.ColumnType,
            this.DefaultValue,
            this.Condition,
            this.Operator,
            this.NewLine,
            this.TextAlign,
            this.TextWidth,
            this.InfoRefVal,
            this.ExternalRefVal,
            this.ColumnVisible});
            this.dgvClientQuery.Location = new System.Drawing.Point(1, 47);
            this.dgvClientQuery.Name = "dgvClientQuery";
            this.dgvClientQuery.RowHeadersWidth = 25;
            this.dgvClientQuery.RowTemplate.Height = 23;
            this.dgvClientQuery.Size = new System.Drawing.Size(663, 249);
            this.dgvClientQuery.TabIndex = 0;
            this.dgvClientQuery.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvClientQuery_CellBeginEdit);
            this.dgvClientQuery.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvClientQuery_RowsAdded);
            this.dgvClientQuery.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClientQuery_CellEndEdit);
            this.dgvClientQuery.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClientQuery_CellValueChanged);
            this.dgvClientQuery.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvClientQuery_DataError);
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreview.Location = new System.Drawing.Point(397, 311);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 5;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // Caption
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Caption.DefaultCellStyle = dataGridViewCellStyle2;
            this.Caption.HeaderText = "Caption";
            this.Caption.Name = "Caption";
            // 
            // Column
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column.HeaderText = "Column";
            this.Column.Name = "Column";
            this.Column.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ColumnType
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnType.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnType.HeaderText = "ColumnType";
            this.ColumnType.Items.AddRange(new object[] {
            "ClientQueryTextBoxColumn",
            "ClientQueryComboBoxColumn",
            "ClientQueryCheckBoxColumn",
            "ClientQueryRefValColumn",
            "ClientQueryCalendarColumn"});
            this.ColumnType.Name = "ColumnType";
            this.ColumnType.Width = 180;
            // 
            // DefaultValue
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DefaultValue.DefaultCellStyle = dataGridViewCellStyle5;
            this.DefaultValue.HeaderText = "DefaultValue";
            this.DefaultValue.Name = "DefaultValue";
            // 
            // Condition
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Condition.DefaultCellStyle = dataGridViewCellStyle6;
            this.Condition.HeaderText = "Condition";
            this.Condition.Items.AddRange(new object[] {
            "And",
            "Or"});
            this.Condition.Name = "Condition";
            this.Condition.Width = 65;
            // 
            // Operator
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Operator.DefaultCellStyle = dataGridViewCellStyle7;
            this.Operator.HeaderText = "Operator";
            this.Operator.Items.AddRange(new object[] {
            "=",
            "!=",
            ">",
            "<",
            ">=",
            "<=",
            "%",
            "%%",
            "in"});
            this.Operator.Name = "Operator";
            this.Operator.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Operator.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Operator.Width = 65;
            // 
            // NewLine
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.NewLine.DefaultCellStyle = dataGridViewCellStyle8;
            this.NewLine.HeaderText = "NewLine";
            this.NewLine.Items.AddRange(new object[] {
            "True",
            "False"});
            this.NewLine.Name = "NewLine";
            this.NewLine.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NewLine.Width = 65;
            // 
            // TextAlign
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TextAlign.DefaultCellStyle = dataGridViewCellStyle9;
            this.TextAlign.HeaderText = "TextAlign";
            this.TextAlign.Items.AddRange(new object[] {
            "Left",
            "Center",
            "Right"});
            this.TextAlign.Name = "TextAlign";
            this.TextAlign.Width = 65;
            // 
            // TextWidth
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TextWidth.DefaultCellStyle = dataGridViewCellStyle10;
            this.TextWidth.HeaderText = "Width";
            this.TextWidth.Name = "TextWidth";
            this.TextWidth.Width = 50;
            // 
            // InfoRefVal
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.InfoRefVal.DefaultCellStyle = dataGridViewCellStyle11;
            this.InfoRefVal.HeaderText = "InfoRefVal";
            this.InfoRefVal.Name = "InfoRefVal";
            this.InfoRefVal.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.InfoRefVal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ExternalRefVal
            // 
            this.ExternalRefVal.HeaderText = "ExternalRefVal";
            this.ExternalRefVal.Name = "ExternalRefVal";
            // 
            // Visible
            // 
            this.ColumnVisible.HeaderText = "Visible";
            this.ColumnVisible.Name = "ColumnVisible";
            // 
            // frmClientQueryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 346);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbbindingsource);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgvClientQuery);
            this.Name = "frmClientQueryEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ClientQueryEditor";
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientQuery)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvClientQuery;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbbindingsource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.DataGridViewTextBoxColumn Caption;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DefaultValue;
        private System.Windows.Forms.DataGridViewComboBoxColumn Condition;
        private System.Windows.Forms.DataGridViewComboBoxColumn Operator;
        private System.Windows.Forms.DataGridViewComboBoxColumn NewLine;
        private System.Windows.Forms.DataGridViewComboBoxColumn TextAlign;
        private System.Windows.Forms.DataGridViewTextBoxColumn TextWidth;
        private System.Windows.Forms.DataGridViewComboBoxColumn InfoRefVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExternalRefVal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnVisible;
    }
}