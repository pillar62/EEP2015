namespace EEPNetServer
{
	partial class frmCreateDB
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioWorkFlow = new System.Windows.Forms.RadioButton();
            this.radioEEP2006 = new System.Windows.Forms.RadioButton();
            this.radioEEP7M = new System.Windows.Forms.RadioButton();
            this.radioTypical = new System.Windows.Forms.RadioButton();
            this.radioSimplified = new System.Windows.Forms.RadioButton();
            this.OKbutton = new System.Windows.Forms.Button();
            this.radioEEPCloudSystemTable = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioEEPCloudSystemTable);
            this.groupBox1.Controls.Add(this.radioWorkFlow);
            this.groupBox1.Controls.Add(this.radioEEP2006);
            this.groupBox1.Controls.Add(this.radioEEP7M);
            this.groupBox1.Controls.Add(this.radioTypical);
            this.groupBox1.Controls.Add(this.radioSimplified);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 239);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // radioWorkFlow
            // 
            this.radioWorkFlow.AutoSize = true;
            this.radioWorkFlow.Location = new System.Drawing.Point(35, 166);
            this.radioWorkFlow.Name = "radioWorkFlow";
            this.radioWorkFlow.Size = new System.Drawing.Size(113, 16);
            this.radioWorkFlow.TabIndex = 5;
            this.radioWorkFlow.TabStop = true;
            this.radioWorkFlow.Text = "Work Flow Table";
            this.radioWorkFlow.UseVisualStyleBackColor = true;
            // 
            // radioEEP2006
            // 
            this.radioEEP2006.AutoSize = true;
            this.radioEEP2006.Location = new System.Drawing.Point(35, 131);
            this.radioEEP2006.Name = "radioEEP2006";
            this.radioEEP2006.Size = new System.Drawing.Size(137, 16);
            this.radioEEP2006.TabIndex = 4;
            this.radioEEP2006.TabStop = true;
            this.radioEEP2006.Text = "EEP2006 2.2.0.0 SP3";
            this.radioEEP2006.UseVisualStyleBackColor = true;
            // 
            // radioEEP7M
            // 
            this.radioEEP7M.AutoSize = true;
            this.radioEEP7M.Location = new System.Drawing.Point(35, 98);
            this.radioEEP7M.Name = "radioEEP7M";
            this.radioEEP7M.Size = new System.Drawing.Size(107, 16);
            this.radioEEP7M.TabIndex = 3;
            this.radioEEP7M.TabStop = true;
            this.radioEEP7M.Text = "EEP7 migration";
            this.radioEEP7M.UseVisualStyleBackColor = true;
            // 
            // radioTypical
            // 
            this.radioTypical.AutoSize = true;
            this.radioTypical.Location = new System.Drawing.Point(35, 63);
            this.radioTypical.Name = "radioTypical";
            this.radioTypical.Size = new System.Drawing.Size(65, 16);
            this.radioTypical.TabIndex = 1;
            this.radioTypical.TabStop = true;
            this.radioTypical.Text = "Typical";
            this.radioTypical.UseVisualStyleBackColor = true;
            this.radioTypical.CheckedChanged += new System.EventHandler(this.radioTypical_CheckedChanged);
            // 
            // radioSimplified
            // 
            this.radioSimplified.AutoSize = true;
            this.radioSimplified.Location = new System.Drawing.Point(35, 29);
            this.radioSimplified.Name = "radioSimplified";
            this.radioSimplified.Size = new System.Drawing.Size(83, 16);
            this.radioSimplified.TabIndex = 0;
            this.radioSimplified.TabStop = true;
            this.radioSimplified.Text = "Simplified";
            this.radioSimplified.UseVisualStyleBackColor = true;
            // 
            // OKbutton
            // 
            this.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKbutton.Location = new System.Drawing.Point(90, 271);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(50, 23);
            this.OKbutton.TabIndex = 2;
            this.OKbutton.Text = "OK";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // radioEEPCloudSystemTable
            // 
            this.radioEEPCloudSystemTable.AutoSize = true;
            this.radioEEPCloudSystemTable.Location = new System.Drawing.Point(35, 203);
            this.radioEEPCloudSystemTable.Name = "radioEEPCloudSystemTable";
            this.radioEEPCloudSystemTable.Size = new System.Drawing.Size(155, 16);
            this.radioEEPCloudSystemTable.TabIndex = 6;
            this.radioEEPCloudSystemTable.TabStop = true;
            this.radioEEPCloudSystemTable.Text = "EEP Cloud System Table";
            this.radioEEPCloudSystemTable.UseVisualStyleBackColor = true;
            // 
            // frmCreateDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 306);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmCreateDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Style";
            this.Load += new System.EventHandler(this.frmCreateDB_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioTypical;
        private System.Windows.Forms.RadioButton radioSimplified;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.RadioButton radioEEP7M;
        private System.Windows.Forms.RadioButton radioEEP2006;
        private System.Windows.Forms.RadioButton radioWorkFlow;
        private System.Windows.Forms.RadioButton radioEEPCloudSystemTable;
	}
}