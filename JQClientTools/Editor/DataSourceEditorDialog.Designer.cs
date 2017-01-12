namespace JQClientTools.Editor
{
    partial class DataSourceEditorDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRemoteName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.comboBoxDisplayMember = new System.Windows.Forms.ComboBox();
            this.comboBoxValueMember = new System.Windows.Forms.ComboBox();
            this.buttonRemoteName = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "RemoteName:";
            // 
            // textBoxRemoteName
            // 
            this.textBoxRemoteName.Location = new System.Drawing.Point(126, 23);
            this.textBoxRemoteName.Name = "textBoxRemoteName";
            this.textBoxRemoteName.Size = new System.Drawing.Size(141, 21);
            this.textBoxRemoteName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "ValueMember:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "DisplayMember:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(210, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(120, 127);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // comboBoxDisplayMember
            // 
            this.comboBoxDisplayMember.FormattingEnabled = true;
            this.comboBoxDisplayMember.Location = new System.Drawing.Point(126, 56);
            this.comboBoxDisplayMember.Name = "comboBoxDisplayMember";
            this.comboBoxDisplayMember.Size = new System.Drawing.Size(161, 20);
            this.comboBoxDisplayMember.TabIndex = 8;
            // 
            // comboBoxValueMember
            // 
            this.comboBoxValueMember.FormattingEnabled = true;
            this.comboBoxValueMember.Location = new System.Drawing.Point(126, 88);
            this.comboBoxValueMember.Name = "comboBoxValueMember";
            this.comboBoxValueMember.Size = new System.Drawing.Size(161, 20);
            this.comboBoxValueMember.TabIndex = 9;
            // 
            // buttonRemoteName
            // 
            this.buttonRemoteName.Location = new System.Drawing.Point(267, 23);
            this.buttonRemoteName.Name = "buttonRemoteName";
            this.buttonRemoteName.Size = new System.Drawing.Size(21, 21);
            this.buttonRemoteName.TabIndex = 10;
            this.buttonRemoteName.Text = "...";
            this.buttonRemoteName.UseVisualStyleBackColor = true;
            this.buttonRemoteName.Click += new System.EventHandler(this.buttonRemoteName_Click);
            // 
            // DataSourceEditorDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(301, 167);
            this.Controls.Add(this.buttonRemoteName);
            this.Controls.Add(this.comboBoxValueMember);
            this.Controls.Add(this.comboBoxDisplayMember);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxRemoteName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataSourceEditorDialog";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxRemoteName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox comboBoxDisplayMember;
        private System.Windows.Forms.ComboBox comboBoxValueMember;
        private System.Windows.Forms.Button buttonRemoteName;
    }
}