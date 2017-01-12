namespace MWizard2015
{
	partial class fmSelTableField
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpSelectTable = new System.Windows.Forms.TabPage();
            this.lvTable = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpSelectFields = new System.Windows.Forms.TabPage();
            this.lvFields = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tpSelectTable.SuspendLayout();
            this.tpSelectFields.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpSelectTable);
            this.tabControl.Controls.Add(this.tpSelectFields);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(369, 336);
            this.tabControl.TabIndex = 0;
            // 
            // tpSelectTable
            // 
            this.tpSelectTable.Controls.Add(this.lvTable);
            this.tpSelectTable.Location = new System.Drawing.Point(4, 22);
            this.tpSelectTable.Name = "tpSelectTable";
            this.tpSelectTable.Padding = new System.Windows.Forms.Padding(3);
            this.tpSelectTable.Size = new System.Drawing.Size(361, 310);
            this.tpSelectTable.TabIndex = 0;
            this.tpSelectTable.Text = "Select Table";
            // 
            // lvTable
            // 
            this.lvTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTable.Location = new System.Drawing.Point(3, 3);
            this.lvTable.MultiSelect = false;
            this.lvTable.Name = "lvTable";
            this.lvTable.Size = new System.Drawing.Size(355, 304);
            this.lvTable.TabIndex = 0;
            this.lvTable.UseCompatibleStateImageBehavior = false;
            this.lvTable.View = System.Windows.Forms.View.Details;
            this.lvTable.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvTable_ColumnClick);
            this.lvTable.SelectedIndexChanged += new System.EventHandler(this.lvTable_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Table Name";
            this.columnHeader1.Width = 184;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.Width = 146;
            // 
            // tpSelectFields
            // 
            this.tpSelectFields.Controls.Add(this.lvFields);
            this.tpSelectFields.Location = new System.Drawing.Point(4, 22);
            this.tpSelectFields.Name = "tpSelectFields";
            this.tpSelectFields.Padding = new System.Windows.Forms.Padding(3);
            this.tpSelectFields.Size = new System.Drawing.Size(361, 310);
            this.tpSelectFields.TabIndex = 1;
            this.tpSelectFields.Text = "Select Fields";
            // 
            // lvFields
            // 
            this.lvFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lvFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFields.Location = new System.Drawing.Point(3, 3);
            this.lvFields.Name = "lvFields";
            this.lvFields.Size = new System.Drawing.Size(355, 304);
            this.lvFields.TabIndex = 0;
            this.lvFields.UseCompatibleStateImageBehavior = false;
            this.lvFields.View = System.Windows.Forms.View.Details;
            this.lvFields.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvFields_ColumnClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Field Name";
            this.columnHeader3.Width = 183;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Description";
            this.columnHeader4.Width = 148;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnSelectAll);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 336);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(369, 43);
            this.panel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(235, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(133, 10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(31, 10);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 4;
            this.btnSelectAll.Text = "&Select All";
            this.btnSelectAll.Visible = false;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // fmSelTableField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 379);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl);
            this.Name = "fmSelTableField";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Fields";
            this.tabControl.ResumeLayout(false);
            this.tpSelectTable.ResumeLayout(false);
            this.tpSelectFields.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tpSelectTable;
		private System.Windows.Forms.TabPage tpSelectFields;
		private System.Windows.Forms.ListView lvTable;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ListView lvFields;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnSelectAll;
	}
}