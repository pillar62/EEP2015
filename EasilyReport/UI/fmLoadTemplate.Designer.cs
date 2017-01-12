namespace Infolight.EasilyReportTools.UI
{
    partial class fmLoadTemplate
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
            this.lbxTemplate = new System.Windows.Forms.ListBox();
            this.lbSelectTemplate = new System.Windows.Forms.Label();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbxTemplate
            // 
            this.lbxTemplate.FormattingEnabled = true;
            this.lbxTemplate.ItemHeight = 12;
            this.lbxTemplate.Location = new System.Drawing.Point(25, 36);
            this.lbxTemplate.Name = "lbxTemplate";
            this.lbxTemplate.Size = new System.Drawing.Size(230, 208);
            this.lbxTemplate.TabIndex = 0;
            this.lbxTemplate.DoubleClick += new System.EventHandler(this.btOK_Click);
            // 
            // lbSelectTemplate
            // 
            this.lbSelectTemplate.AutoSize = true;
            this.lbSelectTemplate.Font = new System.Drawing.Font("SimSun", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSelectTemplate.Location = new System.Drawing.Point(26, 14);
            this.lbSelectTemplate.Name = "lbSelectTemplate";
            this.lbSelectTemplate.Size = new System.Drawing.Size(223, 15);
            this.lbSelectTemplate.TabIndex = 1;
            this.lbSelectTemplate.Text = "Please select one template:";
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(102, 248);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 2;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(183, 248);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(21, 248);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 4;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // fmLoadTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 294);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.lbxTemplate);
            this.Controls.Add(this.lbSelectTemplate);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmLoadTemplate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Templates";
            this.Load += new System.EventHandler(this.fmLoadTemplate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbxTemplate;
        private System.Windows.Forms.Label lbSelectTemplate;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button buttonDelete;
    }
}