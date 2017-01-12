namespace EEPSetUpLibrary.Proxy
{
    partial class frmProxy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProxy));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkBoxProxy = new System.Windows.Forms.CheckBox();
            this.panelProxy = new System.Windows.Forms.Panel();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.labelAddress = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.labelAddressError = new System.Windows.Forms.Label();
            this.labelPortError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelProxy.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxProxy
            // 
            resources.ApplyResources(this.checkBoxProxy, "checkBoxProxy");
            this.checkBoxProxy.Name = "checkBoxProxy";
            this.checkBoxProxy.UseVisualStyleBackColor = true;
            this.checkBoxProxy.CheckedChanged += new System.EventHandler(this.checkBoxProxy_CheckedChanged);
            // 
            // panelProxy
            // 
            this.panelProxy.Controls.Add(this.textBoxPort);
            this.panelProxy.Controls.Add(this.textBoxAddress);
            this.panelProxy.Controls.Add(this.labelPort);
            this.panelProxy.Controls.Add(this.labelAddress);
            this.panelProxy.Controls.Add(this.labelPassword);
            this.panelProxy.Controls.Add(this.labelUser);
            this.panelProxy.Controls.Add(this.textBoxPassword);
            this.panelProxy.Controls.Add(this.textBoxUser);
            resources.ApplyResources(this.panelProxy, "panelProxy");
            this.panelProxy.Name = "panelProxy";
            // 
            // textBoxPort
            // 
            resources.ApplyResources(this.textBoxPort, "textBoxPort");
            this.textBoxPort.Name = "textBoxPort";
            // 
            // textBoxAddress
            // 
            resources.ApplyResources(this.textBoxAddress, "textBoxAddress");
            this.textBoxAddress.Name = "textBoxAddress";
            // 
            // labelPort
            // 
            resources.ApplyResources(this.labelPort, "labelPort");
            this.labelPort.Name = "labelPort";
            // 
            // labelAddress
            // 
            resources.ApplyResources(this.labelAddress, "labelAddress");
            this.labelAddress.Name = "labelAddress";
            // 
            // labelPassword
            // 
            resources.ApplyResources(this.labelPassword, "labelPassword");
            this.labelPassword.Name = "labelPassword";
            // 
            // labelUser
            // 
            resources.ApplyResources(this.labelUser, "labelUser");
            this.labelUser.Name = "labelUser";
            // 
            // textBoxPassword
            // 
            resources.ApplyResources(this.textBoxPassword, "textBoxPassword");
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // textBoxUser
            // 
            resources.ApplyResources(this.textBoxUser, "textBoxUser");
            this.textBoxUser.Name = "textBoxUser";
            // 
            // labelAddressError
            // 
            resources.ApplyResources(this.labelAddressError, "labelAddressError");
            this.labelAddressError.Name = "labelAddressError";
            // 
            // labelPortError
            // 
            resources.ApplyResources(this.labelPortError, "labelPortError");
            this.labelPortError.Name = "labelPortError";
            // 
            // frmProxy
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.panelProxy);
            this.Controls.Add(this.checkBoxProxy);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelPortError);
            this.Controls.Add(this.labelAddressError);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProxy";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmProxy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelProxy.ResumeLayout(false);
            this.panelProxy.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBoxProxy;
        private System.Windows.Forms.Panel panelProxy;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.Label labelAddressError;
        private System.Windows.Forms.Label labelPortError;
    }
}