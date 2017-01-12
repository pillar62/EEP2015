namespace Srvtools
{
    partial class frmTreeView
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
            this.tbKeyField = new System.Windows.Forms.TextBox();
            this.tbTextField = new System.Windows.Forms.TextBox();
            this.lblKeyField = new System.Windows.Forms.Label();
            this.lblParentField = new System.Windows.Forms.Label();
            this.lblText = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbbParentField = new Srvtools.InfoComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.cbbParentField)).BeginInit();
            this.SuspendLayout();
            // 
            // tbKeyField
            // 
            this.tbKeyField.Location = new System.Drawing.Point(86, 18);
            this.tbKeyField.Name = "tbKeyField";
            this.tbKeyField.ReadOnly = true;
            this.tbKeyField.Size = new System.Drawing.Size(121, 21);
            this.tbKeyField.TabIndex = 0;
            // 
            // tbTextField
            // 
            this.tbTextField.Location = new System.Drawing.Point(86, 89);
            this.tbTextField.Name = "tbTextField";
            this.tbTextField.Size = new System.Drawing.Size(121, 21);
            this.tbTextField.TabIndex = 0;
            // 
            // lblKeyField
            // 
            this.lblKeyField.AutoSize = true;
            this.lblKeyField.Location = new System.Drawing.Point(22, 21);
            this.lblKeyField.Name = "lblKeyField";
            this.lblKeyField.Size = new System.Drawing.Size(23, 12);
            this.lblKeyField.TabIndex = 1;
            this.lblKeyField.Text = "Key";
            // 
            // lblParentField
            // 
            this.lblParentField.AutoSize = true;
            this.lblParentField.Location = new System.Drawing.Point(22, 55);
            this.lblParentField.Name = "lblParentField";
            this.lblParentField.Size = new System.Drawing.Size(41, 12);
            this.lblParentField.TabIndex = 2;
            this.lblParentField.Text = "Parent";
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(22, 92);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(29, 12);
            this.lblText.TabIndex = 3;
            this.lblText.Text = "Text";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(24, 127);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "button1";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(132, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "button2";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbbParentField
            // 
            this.cbbParentField.Expression = null;
            this.cbbParentField.FormattingEnabled = true;
            this.cbbParentField.Location = new System.Drawing.Point(86, 52);
            this.cbbParentField.Name = "cbbParentField";
            this.cbbParentField.SelectAlias = null;
            this.cbbParentField.SelectCommand = null;
            this.cbbParentField.SelectTop = null;
            this.cbbParentField.Size = new System.Drawing.Size(121, 20);
            this.cbbParentField.TabIndex = 6;
            // 
            // frmTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 170);
            this.Controls.Add(this.cbbParentField);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.lblParentField);
            this.Controls.Add(this.lblKeyField);
            this.Controls.Add(this.tbTextField);
            this.Controls.Add(this.tbKeyField);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmTreeView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TreeViewEditor";
            this.Load += new System.EventHandler(this.frmTreeView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cbbParentField)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbKeyField;
        private System.Windows.Forms.TextBox tbTextField;
        private System.Windows.Forms.Label lblKeyField;
        private System.Windows.Forms.Label lblParentField;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private InfoComboBox cbbParentField;
    }
}