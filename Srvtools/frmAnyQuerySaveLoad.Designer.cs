namespace Srvtools
{
    partial class frmAnyQuerySaveLoad
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.cbSaveLoad = new System.Windows.Forms.ComboBox();
            this.labelMessage = new System.Windows.Forms.Label();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.lbLoad = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(21, 110);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(122, 110);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // cbSaveLoad
            // 
            this.cbSaveLoad.FormattingEnabled = true;
            this.cbSaveLoad.Location = new System.Drawing.Point(62, 60);
            this.cbSaveLoad.Name = "cbSaveLoad";
            this.cbSaveLoad.Size = new System.Drawing.Size(177, 20);
            this.cbSaveLoad.TabIndex = 0;
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(38, 28);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(41, 12);
            this.labelMessage.TabIndex = 3;
            this.labelMessage.Text = "label1";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(217, 110);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 4;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // lbLoad
            // 
            this.lbLoad.FormattingEnabled = true;
            this.lbLoad.ItemHeight = 12;
            this.lbLoad.Location = new System.Drawing.Point(62, 60);
            this.lbLoad.Name = "lbLoad";
            this.lbLoad.Size = new System.Drawing.Size(177, 28);
            this.lbLoad.TabIndex = 5;
            // 
            // frmAnyQuerySaveLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 161);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.cbSaveLoad);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.lbLoad);
            this.MaximizeBox = false;
            this.Name = "frmAnyQuerySaveLoad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmQuerySaveLoad_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox cbSaveLoad;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.ListBox lbLoad;
    }
}