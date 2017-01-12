namespace EEPSetUpLibrary.Server
{
    partial class frmSolution
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
            this.dataGridViewSolution = new System.Windows.Forms.DataGridView();
            this.buttonOK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLoginDataBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLoginSolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLanguage = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSolution)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSolution
            // 
            this.dataGridViewSolution.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSolution.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSolution.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnText,
            this.ColumnIP,
            this.ColumnPort,
            this.ColumnLoginDataBase,
            this.ColumnLoginSolution,
            this.ColumnLanguage});
            this.dataGridViewSolution.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSolution.Name = "dataGridViewSolution";
            this.dataGridViewSolution.RowTemplate.Height = 23;
            this.dataGridViewSolution.Size = new System.Drawing.Size(645, 150);
            this.dataGridViewSolution.TabIndex = 0;
            this.dataGridViewSolution.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridViewSolution_RowsAdded);
            this.dataGridViewSolution.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewSolution_CellValidating);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(452, 157);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(549, 157);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Text";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "IP";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Port";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 40;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "LoginDataBase";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "LoginSolution";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // ColumnName
            // 
            this.ColumnName.HeaderText = "Name";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.Width = 80;
            // 
            // ColumnText
            // 
            this.ColumnText.HeaderText = "Text";
            this.ColumnText.Name = "ColumnText";
            this.ColumnText.Width = 80;
            // 
            // ColumnIP
            // 
            this.ColumnIP.HeaderText = "IP";
            this.ColumnIP.Name = "ColumnIP";
            // 
            // ColumnPort
            // 
            this.ColumnPort.HeaderText = "Port";
            this.ColumnPort.Name = "ColumnPort";
            this.ColumnPort.Width = 40;
            // 
            // ColumnLoginDataBase
            // 
            this.ColumnLoginDataBase.HeaderText = "LoginDataBase";
            this.ColumnLoginDataBase.Name = "ColumnLoginDataBase";
            // 
            // ColumnLoginSolution
            // 
            this.ColumnLoginSolution.HeaderText = "LoginSolution";
            this.ColumnLoginSolution.Name = "ColumnLoginSolution";
            // 
            // ColumnLanguage
            // 
            this.ColumnLanguage.HeaderText = "Language";
            this.ColumnLanguage.Items.AddRange(new object[] {
            "ENG",
            "TRA",
            "SIM",
            "HKG",
            "JPN",
            "LAN1",
            "LAN2",
            "LAN3"});
            this.ColumnLanguage.Name = "ColumnLanguage";
            // 
            // frmSolution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 189);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.dataGridViewSolution);
            this.Name = "frmSolution";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Solution";
            this.Load += new System.EventHandler(this.frmSolution_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSolution)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSolution;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnText;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLoginDataBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLoginSolution;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnLanguage;
    }
}