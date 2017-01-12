namespace EFServerTools.Design.EFTransactionDesign
{
    partial class EFTransactionTransKeyFieldsAddDialog
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
            this.txtSrcGetValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxWhereMode = new System.Windows.Forms.ComboBox();
            this.cmbDesField = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSrcField = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSrcGetValue
            // 
            this.txtSrcGetValue.Location = new System.Drawing.Point(113, 85);
            this.txtSrcGetValue.Name = "txtSrcGetValue";
            this.txtSrcGetValue.Size = new System.Drawing.Size(163, 21);
            this.txtSrcGetValue.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(10, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "ScrGetValue";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(12, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Where Mode";
            // 
            // cbxWhereMode
            // 
            this.cbxWhereMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxWhereMode.FormattingEnabled = true;
            this.cbxWhereMode.Location = new System.Drawing.Point(113, 121);
            this.cbxWhereMode.Name = "cbxWhereMode";
            this.cbxWhereMode.Size = new System.Drawing.Size(163, 20);
            this.cbxWhereMode.TabIndex = 2;
            // 
            // cmbDesField
            // 
            this.cmbDesField.Location = new System.Drawing.Point(113, 48);
            this.cmbDesField.Name = "cmbDesField";
            this.cmbDesField.Size = new System.Drawing.Size(163, 20);
            this.cmbDesField.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(36, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "DesField";
            // 
            // cmbSrcField
            // 
            this.cmbSrcField.Location = new System.Drawing.Point(113, 12);
            this.cmbSrcField.Name = "cmbSrcField";
            this.cmbSrcField.Size = new System.Drawing.Size(163, 20);
            this.cmbSrcField.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(36, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "ScrField";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.Location = new System.Drawing.Point(170, 166);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 26);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOK.Location = new System.Drawing.Point(45, 166);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 26);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            // 
            // EFTransactionTransKeyFieldsAddDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(291, 204);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbSrcField);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbDesField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSrcGetValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxWhereMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EFTransactionTransKeyFieldsAddDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add TransKeyField ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.TextBox txtSrcGetValue;
        internal System.Windows.Forms.ComboBox cbxWhereMode;
        internal System.Windows.Forms.ComboBox cmbDesField;
        internal System.Windows.Forms.ComboBox cmbSrcField;
    }
}

