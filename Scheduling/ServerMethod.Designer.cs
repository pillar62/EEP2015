namespace Scheduling
{
    partial class ServerMethod
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
            this.lablePackage = new System.Windows.Forms.Label();
            this.labelMethod = new System.Windows.Forms.Label();
            this.btnPackage = new System.Windows.Forms.Button();
            this.bthMethod = new System.Windows.Forms.Button();
            this.txtMethod = new System.Windows.Forms.TextBox();
            this.txtPackage = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lablePackage
            // 
            this.lablePackage.AutoSize = true;
            this.lablePackage.Location = new System.Drawing.Point(22, 33);
            this.lablePackage.Name = "lablePackage";
            this.lablePackage.Size = new System.Drawing.Size(77, 12);
            this.lablePackage.TabIndex = 0;
            this.lablePackage.Text = "Package Name";
            // 
            // labelMethod
            // 
            this.labelMethod.AutoSize = true;
            this.labelMethod.Location = new System.Drawing.Point(22, 66);
            this.labelMethod.Name = "labelMethod";
            this.labelMethod.Size = new System.Drawing.Size(71, 12);
            this.labelMethod.TabIndex = 1;
            this.labelMethod.Text = "Method Name";
            // 
            // btnPackage
            // 
            this.btnPackage.Location = new System.Drawing.Point(263, 28);
            this.btnPackage.Name = "btnPackage";
            this.btnPackage.Size = new System.Drawing.Size(23, 23);
            this.btnPackage.TabIndex = 2;
            this.btnPackage.Text = "...";
            this.btnPackage.UseVisualStyleBackColor = true;
            this.btnPackage.Click += new System.EventHandler(this.btnPackage_Click);
            // 
            // bthMethod
            // 
            this.bthMethod.Location = new System.Drawing.Point(263, 61);
            this.bthMethod.Name = "bthMethod";
            this.bthMethod.Size = new System.Drawing.Size(23, 23);
            this.bthMethod.TabIndex = 3;
            this.bthMethod.Text = "...";
            this.bthMethod.UseVisualStyleBackColor = true;
            this.bthMethod.Click += new System.EventHandler(this.bthMethod_Click);
            // 
            // txtMethod
            // 
            this.txtMethod.Location = new System.Drawing.Point(105, 63);
            this.txtMethod.Name = "txtMethod";
            this.txtMethod.Size = new System.Drawing.Size(140, 21);
            this.txtMethod.TabIndex = 4;
            // 
            // txtPackage
            // 
            this.txtPackage.Location = new System.Drawing.Point(105, 30);
            this.txtPackage.Name = "txtPackage";
            this.txtPackage.Size = new System.Drawing.Size(140, 21);
            this.txtPackage.TabIndex = 5;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Package files(*.dll)|*.dll";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(62, 98);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(170, 98);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 7;
            this.btnCancle.Text = "Cancle";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // ServerMethod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 133);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtPackage);
            this.Controls.Add(this.txtMethod);
            this.Controls.Add(this.bthMethod);
            this.Controls.Add(this.btnPackage);
            this.Controls.Add(this.labelMethod);
            this.Controls.Add(this.lablePackage);
            this.MaximizeBox = false;
            this.Name = "ServerMethod";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ServerMethod";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lablePackage;
        private System.Windows.Forms.Label labelMethod;
        private System.Windows.Forms.Button btnPackage;
        private System.Windows.Forms.Button bthMethod;
        private System.Windows.Forms.TextBox txtMethod;
        private System.Windows.Forms.TextBox txtPackage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancle;
    }
}