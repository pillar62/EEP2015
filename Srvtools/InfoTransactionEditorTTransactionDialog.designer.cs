namespace Srvtools
{
	partial class InfoTransactionEditorTTransactionDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTransStep = new System.Windows.Forms.TextBox();
            this.cbxTransMode = new System.Windows.Forms.ComboBox();
            this.cbxAutoNumber = new System.Windows.Forms.ComboBox();
            this.txtTransKeyFields = new System.Windows.Forms.TextBox();
            this.txtTransFields = new System.Windows.Forms.TextBox();
            this.btnTransKeyFields = new System.Windows.Forms.Button();
            this.btnTransFields = new System.Windows.Forms.Button();
            this.cbInsert = new System.Windows.Forms.CheckBox();
            this.cbUpdate = new System.Windows.Forms.CheckBox();
            this.cbDelete = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbTransTableName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(241, 220);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 24);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(70, 220);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(78, 24);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(55, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "TransStep";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(45, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "TransFields";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(19, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "TransKeyFields";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(35, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "AutoNumber";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(46, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 16);
            this.label8.TabIndex = 15;
            this.label8.Text = "TransMode";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(10, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 16);
            this.label9.TabIndex = 16;
            this.label9.Text = "TransTableName";
            // 
            // txtTransStep
            // 
            this.txtTransStep.BackColor = System.Drawing.SystemColors.Window;
            this.txtTransStep.Location = new System.Drawing.Point(156, 9);
            this.txtTransStep.Name = "txtTransStep";
            this.txtTransStep.ReadOnly = true;
            this.txtTransStep.Size = new System.Drawing.Size(201, 21);
            this.txtTransStep.TabIndex = 19;
            // 
            // cbxTransMode
            // 
            this.cbxTransMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTransMode.FormattingEnabled = true;
            this.cbxTransMode.Location = new System.Drawing.Point(156, 64);
            this.cbxTransMode.Name = "cbxTransMode";
            this.cbxTransMode.Size = new System.Drawing.Size(201, 20);
            this.cbxTransMode.TabIndex = 23;
            // 
            // cbxAutoNumber
            // 
            this.cbxAutoNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAutoNumber.FormattingEnabled = true;
            this.cbxAutoNumber.Location = new System.Drawing.Point(156, 91);
            this.cbxAutoNumber.Name = "cbxAutoNumber";
            this.cbxAutoNumber.Size = new System.Drawing.Size(201, 20);
            this.cbxAutoNumber.TabIndex = 24;
            // 
            // txtTransKeyFields
            // 
            this.txtTransKeyFields.BackColor = System.Drawing.SystemColors.Window;
            this.txtTransKeyFields.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransKeyFields.Location = new System.Drawing.Point(156, 120);
            this.txtTransKeyFields.Name = "txtTransKeyFields";
            this.txtTransKeyFields.ReadOnly = true;
            this.txtTransKeyFields.Size = new System.Drawing.Size(201, 22);
            this.txtTransKeyFields.TabIndex = 26;
            this.txtTransKeyFields.Text = "(Collection)";
            // 
            // txtTransFields
            // 
            this.txtTransFields.BackColor = System.Drawing.SystemColors.Window;
            this.txtTransFields.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransFields.Location = new System.Drawing.Point(156, 151);
            this.txtTransFields.Name = "txtTransFields";
            this.txtTransFields.ReadOnly = true;
            this.txtTransFields.Size = new System.Drawing.Size(201, 22);
            this.txtTransFields.TabIndex = 25;
            this.txtTransFields.Text = "(Collection)";
            // 
            // btnTransKeyFields
            // 
            this.btnTransKeyFields.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransKeyFields.Location = new System.Drawing.Point(338, 122);
            this.btnTransKeyFields.Name = "btnTransKeyFields";
            this.btnTransKeyFields.Size = new System.Drawing.Size(19, 18);
            this.btnTransKeyFields.TabIndex = 27;
            this.btnTransKeyFields.Text = "...";
            this.btnTransKeyFields.Click += new System.EventHandler(this.btnTransKeyFields_Click);
            // 
            // btnTransFields
            // 
            this.btnTransFields.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransFields.Location = new System.Drawing.Point(338, 154);
            this.btnTransFields.Name = "btnTransFields";
            this.btnTransFields.Size = new System.Drawing.Size(18, 18);
            this.btnTransFields.TabIndex = 28;
            this.btnTransFields.Text = "...";
            this.btnTransFields.Click += new System.EventHandler(this.btnTransFields_Click);
            // 
            // cbInsert
            // 
            this.cbInsert.AutoSize = true;
            this.cbInsert.Checked = true;
            this.cbInsert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbInsert.Location = new System.Drawing.Point(84, 188);
            this.cbInsert.Name = "cbInsert";
            this.cbInsert.Size = new System.Drawing.Size(15, 14);
            this.cbInsert.TabIndex = 29;
            this.cbInsert.UseVisualStyleBackColor = true;
            // 
            // cbUpdate
            // 
            this.cbUpdate.AutoSize = true;
            this.cbUpdate.Checked = true;
            this.cbUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUpdate.Location = new System.Drawing.Point(207, 188);
            this.cbUpdate.Name = "cbUpdate";
            this.cbUpdate.Size = new System.Drawing.Size(15, 14);
            this.cbUpdate.TabIndex = 30;
            this.cbUpdate.UseVisualStyleBackColor = true;
            // 
            // cbDelete
            // 
            this.cbDelete.AutoSize = true;
            this.cbDelete.Checked = true;
            this.cbDelete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDelete.Location = new System.Drawing.Point(326, 188);
            this.cbDelete.Name = "cbDelete";
            this.cbDelete.Size = new System.Drawing.Size(15, 14);
            this.cbDelete.TabIndex = 31;
            this.cbDelete.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(19, 185);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 16);
            this.label10.TabIndex = 32;
            this.label10.Text = "OnInsert";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(135, 185);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 16);
            this.label11.TabIndex = 33;
            this.label11.Text = "OnUpdate";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(260, 185);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 16);
            this.label12.TabIndex = 34;
            this.label12.Text = "Ondelete";
            // 
            // cmbTransTableName
            // 
            this.cmbTransTableName.FormattingEnabled = true;
            this.cmbTransTableName.Location = new System.Drawing.Point(156, 37);
            this.cmbTransTableName.Name = "cmbTransTableName";
            this.cmbTransTableName.Size = new System.Drawing.Size(201, 20);
            this.cmbTransTableName.TabIndex = 35;
            // 
            // InfoTransactionEditorTTransactionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 264);
            this.ControlBox = false;
            this.Controls.Add(this.cmbTransTableName);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbDelete);
            this.Controls.Add(this.cbUpdate);
            this.Controls.Add(this.cbInsert);
            this.Controls.Add(this.btnTransFields);
            this.Controls.Add(this.btnTransKeyFields);
            this.Controls.Add(this.txtTransKeyFields);
            this.Controls.Add(this.txtTransFields);
            this.Controls.Add(this.cbxAutoNumber);
            this.Controls.Add(this.cbxTransMode);
            this.Controls.Add(this.txtTransStep);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InfoTransactionEditorTTransactionDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TTransaction Set Up";
            this.Load += new System.EventHandler(this.InfoTransactionEditorTTransactionDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTransStep;
		private System.Windows.Forms.ComboBox cbxTransMode;
		private System.Windows.Forms.ComboBox cbxAutoNumber;
		private System.Windows.Forms.TextBox txtTransKeyFields;
		private System.Windows.Forms.TextBox txtTransFields;
		private System.Windows.Forms.Button btnTransKeyFields;
		private System.Windows.Forms.Button btnTransFields;
        private System.Windows.Forms.CheckBox cbInsert;
        private System.Windows.Forms.CheckBox cbUpdate;
        private System.Windows.Forms.CheckBox cbDelete;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cmbTransTableName;
	}
}