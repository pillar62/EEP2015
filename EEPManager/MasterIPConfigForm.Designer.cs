namespace EEPManager
{
    partial class MasterIPConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterIPConfigForm));
            this.txtMasterServerIP = new System.Windows.Forms.TextBox();
            this.lblMasterServerIP = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMasterServerIP
            // 
            this.txtMasterServerIP.Location = new System.Drawing.Point(110, 30);
            this.txtMasterServerIP.Name = "txtMasterServerIP";
            this.txtMasterServerIP.Size = new System.Drawing.Size(181, 22);
            this.txtMasterServerIP.TabIndex = 20;
            // 
            // lblMasterServerIP
            // 
            this.lblMasterServerIP.AutoSize = true;
            this.lblMasterServerIP.Location = new System.Drawing.Point(12, 33);
            this.lblMasterServerIP.Name = "lblMasterServerIP";
            this.lblMasterServerIP.Size = new System.Drawing.Size(81, 12);
            this.lblMasterServerIP.TabIndex = 19;
            this.lblMasterServerIP.Text = "Mater Server IP:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(163, 80);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 22);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(56, 80);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 22);
            this.btnOK.TabIndex = 21;
            this.btnOK.Text = "OK";
            // 
            // MasterIPConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 114);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtMasterServerIP);
            this.Controls.Add(this.lblMasterServerIP);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MasterIPConfigForm";
            this.Text = "Master IP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MasterIPConfigForm_FormClosing);
            this.Load += new System.EventHandler(this.MasterIPConfigForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMasterServerIP;
        private System.Windows.Forms.Label lblMasterServerIP;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}