namespace MWizard2015
{
    partial class fmRefVal
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpRefVal = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSelectCommand = new System.Windows.Forms.TextBox();
            this.lvFieldName = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.lbTableName = new System.Windows.Forms.ListBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgvSYS_REFVAL_D1 = new System.Windows.Forms.DataGridView();
            this.bsSYS_REFVAL_D3 = new System.Windows.Forms.BindingSource(this.components);
            this.bsSYS_REFVAL = new System.Windows.Forms.BindingSource(this.components);
            this.bsSYS_REFVAL_D1 = new System.Windows.Forms.BindingSource(this.components);
            this.bsSYS_REFVAL_D2 = new System.Windows.Forms.BindingSource(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tpRefVal.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSYS_REFVAL_D1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL_D3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL_D1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL_D2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpRefVal);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(698, 460);
            this.tabControl.TabIndex = 0;
            // 
            // tpRefVal
            // 
            this.tpRefVal.Controls.Add(this.panel2);
            this.tpRefVal.Controls.Add(this.panel1);
            this.tpRefVal.Controls.Add(this.tabControl2);
            this.tpRefVal.Location = new System.Drawing.Point(4, 21);
            this.tpRefVal.Name = "tpRefVal";
            this.tpRefVal.Padding = new System.Windows.Forms.Padding(3);
            this.tpRefVal.Size = new System.Drawing.Size(690, 435);
            this.tpRefVal.TabIndex = 1;
            this.tpRefVal.Text = "RefVal";
            this.tpRefVal.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 388);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(684, 44);
            this.panel2.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbSelectCommand);
            this.panel1.Controls.Add(this.lvFieldName);
            this.panel1.Controls.Add(this.lbTableName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(684, 300);
            this.panel1.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(179, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "RefVal define";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "TableName";
            // 
            // tbSelectCommand
            // 
            this.tbSelectCommand.Location = new System.Drawing.Point(12, 196);
            this.tbSelectCommand.Multiline = true;
            this.tbSelectCommand.Name = "tbSelectCommand";
            this.tbSelectCommand.Size = new System.Drawing.Size(674, 95);
            this.tbSelectCommand.TabIndex = 5;
            // 
            // lvFieldName
            // 
            this.lvFieldName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader6,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader3});
            this.lvFieldName.Location = new System.Drawing.Point(181, 24);
            this.lvFieldName.MultiSelect = false;
            this.lvFieldName.Name = "lvFieldName";
            this.lvFieldName.Size = new System.Drawing.Size(505, 160);
            this.lvFieldName.TabIndex = 4;
            this.lvFieldName.UseCompatibleStateImageBehavior = false;
            this.lvFieldName.View = System.Windows.Forms.View.Details;
            this.lvFieldName.SelectedIndexChanged += new System.EventHandler(this.lvFieldName_SelectedIndexChanged);
            this.lvFieldName.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvFieldName_ColumnClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "RefVal_NO";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Description";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value Member";
            this.columnHeader2.Width = 110;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Display Member";
            this.columnHeader4.Width = 110;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Alias";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Select Command";
            this.columnHeader3.Width = 0;
            // 
            // lbTableName
            // 
            this.lbTableName.FormattingEnabled = true;
            this.lbTableName.ItemHeight = 12;
            this.lbTableName.Location = new System.Drawing.Point(12, 24);
            this.lbTableName.Name = "lbTableName";
            this.lbTableName.Size = new System.Drawing.Size(144, 160);
            this.lbTableName.TabIndex = 3;
            this.lbTableName.SelectedIndexChanged += new System.EventHandler(this.lbTableName_SelectedIndexChanged);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Location = new System.Drawing.Point(-4, 277);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(698, 177);
            this.tabControl2.TabIndex = 6;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgvSYS_REFVAL_D1);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(690, 152);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Columns";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvSYS_REFVAL_D1
            // 
            this.dgvSYS_REFVAL_D1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSYS_REFVAL_D1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSYS_REFVAL_D1.Location = new System.Drawing.Point(3, 3);
            this.dgvSYS_REFVAL_D1.Name = "dgvSYS_REFVAL_D1";
            this.dgvSYS_REFVAL_D1.RowTemplate.Height = 24;
            this.dgvSYS_REFVAL_D1.Size = new System.Drawing.Size(684, 146);
            this.dgvSYS_REFVAL_D1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(358, 474);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(220, 474);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // fmRefVal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 517);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabControl);
            this.Name = "fmRefVal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RefVal Setting";
            this.tabControl.ResumeLayout(false);
            this.tpRefVal.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSYS_REFVAL_D1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL_D3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL_D1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSYS_REFVAL_D2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpRefVal;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSelectCommand;
        private System.Windows.Forms.ListView lvFieldName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ListBox lbTableName;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvSYS_REFVAL_D1;
        private System.Windows.Forms.BindingSource bsSYS_REFVAL_D3;
        private System.Windows.Forms.BindingSource bsSYS_REFVAL;
        private System.Windows.Forms.BindingSource bsSYS_REFVAL_D1;
        private System.Windows.Forms.BindingSource bsSYS_REFVAL_D2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;

    }
}