namespace Srvtools
{
	partial class frmAnyQuery
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonSubtract = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbCondition = new System.Windows.Forms.ComboBox();
            this.labCondition = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.labelColumn = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonQuit);
            this.panel1.Controls.Add(this.buttonLoad);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.buttonSubtract);
            this.panel1.Controls.Add(this.buttonAdd);
            this.panel1.Controls.Add(this.buttonQuery);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(674, 37);
            this.panel1.TabIndex = 0;
            // 
            // buttonQuit
            // 
            this.buttonQuit.AutoSize = true;
            this.buttonQuit.DialogResult = System.Windows.Forms.DialogResult.No;
            this.buttonQuit.Location = new System.Drawing.Point(308, 2);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(61, 30);
            this.buttonQuit.TabIndex = 5;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            // 
            // buttonLoad
            // 
            this.buttonLoad.AutoSize = true;
            this.buttonLoad.Location = new System.Drawing.Point(243, 2);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(61, 30);
            this.buttonLoad.TabIndex = 4;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.AutoSize = true;
            this.buttonSave.Location = new System.Drawing.Point(179, 2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(61, 30);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonSubtract
            // 
            this.buttonSubtract.AutoSize = true;
            this.buttonSubtract.Location = new System.Drawing.Point(106, 2);
            this.buttonSubtract.Name = "buttonSubtract";
            this.buttonSubtract.Size = new System.Drawing.Size(69, 30);
            this.buttonSubtract.TabIndex = 2;
            this.buttonSubtract.Text = "Subtract";
            this.buttonSubtract.UseVisualStyleBackColor = true;
            this.buttonSubtract.Click += new System.EventHandler(this.buttonSubtract_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.AutoSize = true;
            this.buttonAdd.Location = new System.Drawing.Point(53, 2);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(49, 30);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonQuery
            // 
            this.buttonQuery.AutoSize = true;
            this.buttonQuery.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonQuery.Location = new System.Drawing.Point(1, 2);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(49, 30);
            this.buttonQuery.TabIndex = 0;
            this.buttonQuery.Text = "Query";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cbCondition);
            this.panel2.Controls.Add(this.labCondition);
            this.panel2.Controls.Add(this.labelValue);
            this.panel2.Controls.Add(this.labelColumn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(674, 348);
            this.panel2.TabIndex = 1;
            // 
            // cbCondition
            // 
            this.cbCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCondition.FormattingEnabled = true;
            this.cbCondition.Items.AddRange(new object[] {
            "AND",
            "OR"});
            this.cbCondition.Location = new System.Drawing.Point(557, 13);
            this.cbCondition.Name = "cbCondition";
            this.cbCondition.Size = new System.Drawing.Size(54, 20);
            this.cbCondition.TabIndex = 4;
            // 
            // labCondition
            // 
            this.labCondition.AutoSize = true;
            this.labCondition.Location = new System.Drawing.Point(477, 16);
            this.labCondition.Name = "labCondition";
            this.labCondition.Size = new System.Drawing.Size(65, 12);
            this.labCondition.TabIndex = 2;
            this.labCondition.Text = "Condition:";
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(335, 16);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(35, 12);
            this.labelValue.TabIndex = 1;
            this.labelValue.Text = "Value";
            // 
            // labelColumn
            // 
            this.labelColumn.AutoSize = true;
            this.labelColumn.Location = new System.Drawing.Point(124, 16);
            this.labelColumn.Name = "labelColumn";
            this.labelColumn.Size = new System.Drawing.Size(41, 12);
            this.labelColumn.TabIndex = 0;
            this.labelColumn.Text = "Column";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "XML File|*.xml|All File|*.*";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "XML File|*.xml|All File|*.*";
            // 
            // frmAnyQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 385);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAnyQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AnyQuery";
            this.Load += new System.EventHandler(this.AnyQuery_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonSubtract;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Label labelColumn;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.Label labCondition;
        private System.Windows.Forms.ComboBox cbCondition;
	}
}