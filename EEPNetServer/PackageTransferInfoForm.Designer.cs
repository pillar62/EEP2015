namespace EEPNetServer
{
    partial class PackageTransferInfoForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageTransferInfoForm));
            this.btnOK = new System.Windows.Forms.Button();
            this.dgTransferInfo = new System.Windows.Forms.DataGridView();
            this.cbxAutoTransfer = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgTransferInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(254, 270);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dgTransferInfo
            // 
            this.dgTransferInfo.AllowUserToAddRows = false;
            this.dgTransferInfo.AllowUserToDeleteRows = false;
            this.dgTransferInfo.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            this.dgTransferInfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgTransferInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgTransferInfo.BackgroundColor = System.Drawing.Color.Linen;
            this.dgTransferInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgTransferInfo.ColumnHeadersHeight = 25;
            this.dgTransferInfo.Location = new System.Drawing.Point(12, 34);
            this.dgTransferInfo.Name = "dgTransferInfo";
            this.dgTransferInfo.RowHeadersVisible = false;
            this.dgTransferInfo.RowHeadersWidth = 25;
            this.dgTransferInfo.RowTemplate.Height = 24;
            this.dgTransferInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgTransferInfo.Size = new System.Drawing.Size(548, 214);
            this.dgTransferInfo.TabIndex = 1;
            this.dgTransferInfo.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgTransferInfo_CellBeginEdit);
            // 
            // cbxAutoTransfer
            // 
            this.cbxAutoTransfer.AutoSize = true;
            this.cbxAutoTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxAutoTransfer.Location = new System.Drawing.Point(12, 9);
            this.cbxAutoTransfer.Name = "cbxAutoTransfer";
            this.cbxAutoTransfer.Size = new System.Drawing.Size(149, 19);
            this.cbxAutoTransfer.TabIndex = 2;
            this.cbxAutoTransfer.Text = "Auto Transfer Package";
            this.cbxAutoTransfer.UseVisualStyleBackColor = true;
            // 
            // PackageTransferInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 305);
            this.Controls.Add(this.cbxAutoTransfer);
            this.Controls.Add(this.dgTransferInfo);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PackageTransferInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Package Transfer Infomation";
            this.Load += new System.EventHandler(this.PackageTransferInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgTransferInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridView dgTransferInfo;
        private System.Windows.Forms.CheckBox cbxAutoTransfer;
    }
}