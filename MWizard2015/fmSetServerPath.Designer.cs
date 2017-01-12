namespace MWizard2015
{
    partial class fmSetServerPath
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
            this.tbServerPath = new System.Windows.Forms.TextBox();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectFolder2 = new System.Windows.Forms.Button();
            this.tbWebClientPath = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // tbServerPath
            // 
            this.tbServerPath.Location = new System.Drawing.Point(107, 29);
            this.tbServerPath.Name = "tbServerPath";
            this.tbServerPath.Size = new System.Drawing.Size(313, 21);
            this.tbServerPath.TabIndex = 0;
            this.tbServerPath.Text = "C:\\Program Files\\Infolight\\EEP2010\\EEPNetServer";
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(426, 27);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(32, 23);
            this.btnSelectFolder.TabIndex = 1;
            this.btnSelectFolder.Text = "...";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.SelectedPath = "C:\\Program Files\\Infolight\\EEP2010\\EEPNetServer";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(107, 132);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(293, 132);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "WCFServer Path:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "WebClient Path:";
            // 
            // btnSelectFolder2
            // 
            this.btnSelectFolder2.Location = new System.Drawing.Point(426, 70);
            this.btnSelectFolder2.Name = "btnSelectFolder2";
            this.btnSelectFolder2.Size = new System.Drawing.Size(32, 23);
            this.btnSelectFolder2.TabIndex = 6;
            this.btnSelectFolder2.Text = "...";
            this.btnSelectFolder2.UseVisualStyleBackColor = true;
            this.btnSelectFolder2.Click += new System.EventHandler(this.btnSelectFolder2_Click);
            // 
            // tbWebClientPath
            // 
            this.tbWebClientPath.Location = new System.Drawing.Point(107, 72);
            this.tbWebClientPath.Name = "tbWebClientPath";
            this.tbWebClientPath.Size = new System.Drawing.Size(313, 21);
            this.tbWebClientPath.TabIndex = 5;
            this.tbWebClientPath.Text = "C:\\Program Files\\Infolight\\EEP2010\\EFWebClient";
            // 
            // folderBrowserDialog2
            // 
            this.folderBrowserDialog2.SelectedPath = "C:\\Program Files\\Infolight\\EEP2010\\EFWebClient";
            // 
            // fmSetServerPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 179);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelectFolder2);
            this.Controls.Add(this.tbWebClientPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.tbServerPath);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmSetServerPath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set Config";
            this.Load += new System.EventHandler(this.fmSetServerPath_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbServerPath;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectFolder2;
        private System.Windows.Forms.TextBox tbWebClientPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
    }
}