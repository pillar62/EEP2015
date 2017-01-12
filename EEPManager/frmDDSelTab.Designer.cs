namespace EEPManager
{
    partial class frmDDSelTab
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDDSelTab));
            this.clstTableName = new System.Windows.Forms.CheckedListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.infoDsDBTables = new Srvtools.InfoDataSet(this.components);
            this.btnQuery = new System.Windows.Forms.Button();
            this.tbQuery = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.infoDsDBTables)).BeginInit();
            this.SuspendLayout();
            // 
            // clstTableName
            // 
            this.clstTableName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clstTableName.CheckOnClick = true;
            this.clstTableName.ColumnWidth = 300;
            this.clstTableName.FormattingEnabled = true;
            this.clstTableName.HorizontalScrollbar = true;
            this.clstTableName.Location = new System.Drawing.Point(12, 47);
            this.clstTableName.MultiColumn = true;
            this.clstTableName.Name = "clstTableName";
            this.clstTableName.Size = new System.Drawing.Size(294, 212);
            this.clstTableName.Sorted = true;
            this.clstTableName.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(312, 65);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(312, 105);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // infoDsDBTables
            // 
            this.infoDsDBTables.Active = false;
            this.infoDsDBTables.AlwaysClose = false;
            this.infoDsDBTables.DataCompressed = false;
            this.infoDsDBTables.DeleteIncomplete = true;
            this.infoDsDBTables.LastKeyValues = null;
            this.infoDsDBTables.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.infoDsDBTables.PacketRecords = -1;
            this.infoDsDBTables.Position = -1;
            this.infoDsDBTables.RemoteName = null;
            this.infoDsDBTables.ServerModify = false;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(231, 10);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbQuery
            // 
            this.tbQuery.Location = new System.Drawing.Point(68, 12);
            this.tbQuery.Name = "tbQuery";
            this.tbQuery.Size = new System.Drawing.Size(145, 21);
            this.tbQuery.TabIndex = 4;
            // 
            // frmDDSelTab
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(399, 274);
            this.Controls.Add(this.tbQuery);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.clstTableName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDDSelTab";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "D.D. Select";
            this.Load += new System.EventHandler(this.frmDDSelTab_Load);
            ((System.ComponentModel.ISupportInitialize)(this.infoDsDBTables)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clstTableName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Srvtools.InfoDataSet infoDsDBTables;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.TextBox tbQuery;
    }
}