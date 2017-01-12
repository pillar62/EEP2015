namespace EEPManager
{
    partial class frmLocateLogConnId
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
            this.txtConnId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLocate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtConnId
            // 
            this.txtConnId.Location = new System.Drawing.Point(65, 19);
            this.txtConnId.Name = "txtConnId";
            this.txtConnId.Size = new System.Drawing.Size(134, 21);
            this.txtConnId.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "ConnId:";
            // 
            // btnLocate
            // 
            this.btnLocate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLocate.Location = new System.Drawing.Point(205, 17);
            this.btnLocate.Name = "btnLocate";
            this.btnLocate.Size = new System.Drawing.Size(67, 23);
            this.btnLocate.TabIndex = 2;
            this.btnLocate.Text = "Locate";
            this.btnLocate.UseVisualStyleBackColor = true;
            this.btnLocate.Click += new System.EventHandler(this.btnLocate_Click);
            // 
            // frmLocateLogConnId
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 63);
            this.Controls.Add(this.btnLocate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtConnId);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLocateLogConnId";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLocateLogConnId";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtConnId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLocate;
    }
}