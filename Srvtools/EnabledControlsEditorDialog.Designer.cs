namespace Srvtools
{
    partial class EnabledControlsEditorDialog
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
            this.lbxEnabledControls = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbxDisabledControls = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMoveAllFromEnToDis = new System.Windows.Forms.Button();
            this.btnMoveFromEnToDis = new System.Windows.Forms.Button();
            this.btnMoveFromDisToEn = new System.Windows.Forms.Button();
            this.btnMoveAllFromDisToEn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbxEnabledControls
            // 
            this.lbxEnabledControls.FormattingEnabled = true;
            this.lbxEnabledControls.ItemHeight = 12;
            this.lbxEnabledControls.Location = new System.Drawing.Point(12, 33);
            this.lbxEnabledControls.Name = "lbxEnabledControls";
            this.lbxEnabledControls.Size = new System.Drawing.Size(215, 244);
            this.lbxEnabledControls.TabIndex = 0;
            this.lbxEnabledControls.DoubleClick += new System.EventHandler(this.lbxEnabledControls_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enabled Controls";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(321, 297);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(418, 297);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lbxDisabledControls
            // 
            this.lbxDisabledControls.FormattingEnabled = true;
            this.lbxDisabledControls.ItemHeight = 12;
            this.lbxDisabledControls.Location = new System.Drawing.Point(297, 33);
            this.lbxDisabledControls.Name = "lbxDisabledControls";
            this.lbxDisabledControls.Size = new System.Drawing.Size(215, 244);
            this.lbxDisabledControls.TabIndex = 4;
            this.lbxDisabledControls.DoubleClick += new System.EventHandler(this.lbxDisabledControls_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(294, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Disabled Controls";
            // 
            // btnMoveAllFromEnToDis
            // 
            this.btnMoveAllFromEnToDis.Location = new System.Drawing.Point(239, 54);
            this.btnMoveAllFromEnToDis.Name = "btnMoveAllFromEnToDis";
            this.btnMoveAllFromEnToDis.Size = new System.Drawing.Size(45, 25);
            this.btnMoveAllFromEnToDis.TabIndex = 6;
            this.btnMoveAllFromEnToDis.Text = ">>";
            this.btnMoveAllFromEnToDis.UseVisualStyleBackColor = true;
            this.btnMoveAllFromEnToDis.Click += new System.EventHandler(this.btnMoveAllFromEnToDis_Click);
            // 
            // btnMoveFromEnToDis
            // 
            this.btnMoveFromEnToDis.Location = new System.Drawing.Point(239, 85);
            this.btnMoveFromEnToDis.Name = "btnMoveFromEnToDis";
            this.btnMoveFromEnToDis.Size = new System.Drawing.Size(45, 25);
            this.btnMoveFromEnToDis.TabIndex = 7;
            this.btnMoveFromEnToDis.Text = ">";
            this.btnMoveFromEnToDis.UseVisualStyleBackColor = true;
            this.btnMoveFromEnToDis.Click += new System.EventHandler(this.btnMoveFromEnToDis_Click);
            // 
            // btnMoveFromDisToEn
            // 
            this.btnMoveFromDisToEn.Location = new System.Drawing.Point(239, 137);
            this.btnMoveFromDisToEn.Name = "btnMoveFromDisToEn";
            this.btnMoveFromDisToEn.Size = new System.Drawing.Size(45, 25);
            this.btnMoveFromDisToEn.TabIndex = 8;
            this.btnMoveFromDisToEn.Text = "<";
            this.btnMoveFromDisToEn.UseVisualStyleBackColor = true;
            this.btnMoveFromDisToEn.Click += new System.EventHandler(this.btnMoveFromDisToEn_Click);
            // 
            // btnMoveAllFromDisToEn
            // 
            this.btnMoveAllFromDisToEn.Location = new System.Drawing.Point(239, 168);
            this.btnMoveAllFromDisToEn.Name = "btnMoveAllFromDisToEn";
            this.btnMoveAllFromDisToEn.Size = new System.Drawing.Size(45, 25);
            this.btnMoveAllFromDisToEn.TabIndex = 9;
            this.btnMoveAllFromDisToEn.Text = "<<";
            this.btnMoveAllFromDisToEn.UseVisualStyleBackColor = true;
            this.btnMoveAllFromDisToEn.Click += new System.EventHandler(this.btnMoveAllFromDisToEn_Click);
            // 
            // EnabledControlsEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 332);
            this.Controls.Add(this.btnMoveAllFromDisToEn);
            this.Controls.Add(this.btnMoveFromDisToEn);
            this.Controls.Add(this.btnMoveFromEnToDis);
            this.Controls.Add(this.btnMoveAllFromEnToDis);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbxDisabledControls);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbxEnabledControls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnabledControlsEditorDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enabled Controls Editor";
            this.Load += new System.EventHandler(this.EnabledControlsEditorDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbxEnabledControls;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lbxDisabledControls;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMoveAllFromEnToDis;
        private System.Windows.Forms.Button btnMoveFromEnToDis;
        private System.Windows.Forms.Button btnMoveFromDisToEn;
        private System.Windows.Forms.Button btnMoveAllFromDisToEn;
    }
}