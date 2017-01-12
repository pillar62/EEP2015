namespace Srvtools
{
    partial class WebNavigatorEnableControlsEditorDialog
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
            this.btnMoveAllFromDisenableToEnable = new System.Windows.Forms.Button();
            this.btnMoveFromDisenableToEnable = new System.Windows.Forms.Button();
            this.btnMoveFromEnableToDisenable = new System.Windows.Forms.Button();
            this.btnMoveAllFromEnableToDisenable = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbxDisenableControls = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbxEnableControls = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnMoveAllFromDisenableToEnable
            // 
            this.btnMoveAllFromDisenableToEnable.Location = new System.Drawing.Point(240, 169);
            this.btnMoveAllFromDisenableToEnable.Name = "btnMoveAllFromDisenableToEnable";
            this.btnMoveAllFromDisenableToEnable.Size = new System.Drawing.Size(45, 25);
            this.btnMoveAllFromDisenableToEnable.TabIndex = 19;
            this.btnMoveAllFromDisenableToEnable.Text = "<<";
            this.btnMoveAllFromDisenableToEnable.UseVisualStyleBackColor = true;
            this.btnMoveAllFromDisenableToEnable.Click += new System.EventHandler(this.btnMoveAllFromDisenableToEnable_Click);
            // 
            // btnMoveFromDisenableToEnable
            // 
            this.btnMoveFromDisenableToEnable.Location = new System.Drawing.Point(240, 138);
            this.btnMoveFromDisenableToEnable.Name = "btnMoveFromDisenableToEnable";
            this.btnMoveFromDisenableToEnable.Size = new System.Drawing.Size(45, 25);
            this.btnMoveFromDisenableToEnable.TabIndex = 18;
            this.btnMoveFromDisenableToEnable.Text = "<";
            this.btnMoveFromDisenableToEnable.UseVisualStyleBackColor = true;
            this.btnMoveFromDisenableToEnable.Click += new System.EventHandler(this.btnMoveFromDisenableToEnable_Click);
            // 
            // btnMoveFromEnableToDisenable
            // 
            this.btnMoveFromEnableToDisenable.Location = new System.Drawing.Point(240, 86);
            this.btnMoveFromEnableToDisenable.Name = "btnMoveFromEnableToDisenable";
            this.btnMoveFromEnableToDisenable.Size = new System.Drawing.Size(45, 25);
            this.btnMoveFromEnableToDisenable.TabIndex = 17;
            this.btnMoveFromEnableToDisenable.Text = ">";
            this.btnMoveFromEnableToDisenable.UseVisualStyleBackColor = true;
            this.btnMoveFromEnableToDisenable.Click += new System.EventHandler(this.btnMoveFromEnableToDisenable_Click);
            // 
            // btnMoveAllFromEnableToDisenable
            // 
            this.btnMoveAllFromEnableToDisenable.Location = new System.Drawing.Point(240, 55);
            this.btnMoveAllFromEnableToDisenable.Name = "btnMoveAllFromEnableToDisenable";
            this.btnMoveAllFromEnableToDisenable.Size = new System.Drawing.Size(45, 25);
            this.btnMoveAllFromEnableToDisenable.TabIndex = 16;
            this.btnMoveAllFromEnableToDisenable.Text = ">>";
            this.btnMoveAllFromEnableToDisenable.UseVisualStyleBackColor = true;
            this.btnMoveAllFromEnableToDisenable.Click += new System.EventHandler(this.btnMoveAllFromEnableToDisenable_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(295, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "disenable controls";
            // 
            // lbxDisenableControls
            // 
            this.lbxDisenableControls.FormattingEnabled = true;
            this.lbxDisenableControls.ItemHeight = 12;
            this.lbxDisenableControls.Location = new System.Drawing.Point(297, 34);
            this.lbxDisenableControls.Name = "lbxDisenableControls";
            this.lbxDisenableControls.Size = new System.Drawing.Size(215, 244);
            this.lbxDisenableControls.TabIndex = 14;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(419, 298);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(322, 298);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "enable controls";
            // 
            // lbxEnableControls
            // 
            this.lbxEnableControls.FormattingEnabled = true;
            this.lbxEnableControls.ItemHeight = 12;
            this.lbxEnableControls.Location = new System.Drawing.Point(12, 34);
            this.lbxEnableControls.Name = "lbxEnableControls";
            this.lbxEnableControls.Size = new System.Drawing.Size(215, 244);
            this.lbxEnableControls.TabIndex = 10;
            // 
            // WebNavigatorEnableControlsEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 330);
            this.Controls.Add(this.btnMoveAllFromDisenableToEnable);
            this.Controls.Add(this.btnMoveFromDisenableToEnable);
            this.Controls.Add(this.btnMoveFromEnableToDisenable);
            this.Controls.Add(this.btnMoveAllFromEnableToDisenable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbxDisenableControls);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbxEnableControls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WebNavigatorEnableControlsEditorDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WebNavigatorEnableControlsEditorDialog";
            this.Load += new System.EventHandler(this.WebNavigatorEnableControlsEditorDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMoveAllFromDisenableToEnable;
        private System.Windows.Forms.Button btnMoveFromDisenableToEnable;
        private System.Windows.Forms.Button btnMoveFromEnableToDisenable;
        private System.Windows.Forms.Button btnMoveAllFromEnableToDisenable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lbxDisenableControls;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbxEnableControls;
    }
}