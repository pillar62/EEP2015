namespace EEPManager
{
    partial class frmErrorLogMaintenance
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmErrorLogMaintenance));
            this.panel1 = new System.Windows.Forms.Panel();
            this.errDescrip = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ErrorMessage = new System.Windows.Forms.RichTextBox();
            this.Enlarge = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvErrorLog = new Srvtools.InfoDataGridView();
            this.eRRIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uSERIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mODULENAMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eRRMESSAGEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eRRSTACKDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eRRDESCRIPDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eRRDATEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eRRSCREENDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oWNERDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pROCESSDATEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sTATUSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.infoBsErrorLog = new Srvtools.InfoBindingSource(this.components);
            this.infoDsErrorLog = new Srvtools.InfoDataSet(this.components);
            this.infoDateTimeTo = new Srvtools.InfoDateTimePicker();
            this.infoDateTimeFrom = new Srvtools.InfoDateTimePicker();
            this.rtbCallStack = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrorLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoBsErrorLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoDsErrorLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoDateTimeTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoDateTimeFrom)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rtbCallStack);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.errDescrip);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.ErrorMessage);
            this.panel1.Controls.Add(this.Enlarge);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(511, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(247, 514);
            this.panel1.TabIndex = 0;
            // 
            // errDescrip
            // 
            this.errDescrip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.infoBsErrorLog, "ErrDescrip", true));
            this.errDescrip.Location = new System.Drawing.Point(9, 305);
            this.errDescrip.Name = "errDescrip";
            this.errDescrip.Size = new System.Drawing.Size(226, 89);
            this.errDescrip.TabIndex = 6;
            this.errDescrip.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 290);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "Description:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Error Message:";
            // 
            // ErrorMessage
            // 
            this.ErrorMessage.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.infoBsErrorLog, "ErrMessage", true));
            this.ErrorMessage.Location = new System.Drawing.Point(9, 181);
            this.ErrorMessage.Name = "ErrorMessage";
            this.ErrorMessage.Size = new System.Drawing.Size(226, 98);
            this.ErrorMessage.TabIndex = 3;
            this.ErrorMessage.Text = "";
            // 
            // Enlarge
            // 
            this.Enlarge.Location = new System.Drawing.Point(172, 130);
            this.Enlarge.Name = "Enlarge";
            this.Enlarge.Size = new System.Drawing.Size(54, 23);
            this.Enlarge.TabIndex = 2;
            this.Enlarge.Text = "enlarge";
            this.Enlarge.UseVisualStyleBackColor = true;
            this.Enlarge.Click += new System.EventHandler(this.Enlarge_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(9, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(226, 110);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.infoDateTimeTo);
            this.panel2.Controls.Add(this.infoDateTimeFrom);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(511, 74);
            this.panel2.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "All",
            "Error",
            "Processing",
            "Wait QC",
            "Final"});
            this.comboBox1.Location = new System.Drawing.Point(121, 44);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(69, 20);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.Text = "All";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Show Error For:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(453, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Date To:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date From:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 403);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "Call Stack:";
            // 
            // dgvErrorLog
            // 
            this.dgvErrorLog.AllowUserToAddRows = false;
            this.dgvErrorLog.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(225)))));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Blue;
            this.dgvErrorLog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvErrorLog.AutoGenerateColumns = false;
            this.dgvErrorLog.BackgroundColor = System.Drawing.Color.Linen;
            this.dgvErrorLog.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvErrorLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvErrorLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvErrorLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.eRRIDDataGridViewTextBoxColumn,
            this.uSERIDDataGridViewTextBoxColumn,
            this.mODULENAMEDataGridViewTextBoxColumn,
            this.eRRMESSAGEDataGridViewTextBoxColumn,
            this.eRRSTACKDataGridViewTextBoxColumn,
            this.eRRDESCRIPDataGridViewTextBoxColumn,
            this.eRRDATEDataGridViewTextBoxColumn,
            this.eRRSCREENDataGridViewTextBoxColumn,
            this.oWNERDataGridViewTextBoxColumn,
            this.pROCESSDATEDataGridViewTextBoxColumn,
            this.sTATUSDataGridViewTextBoxColumn});
            this.dgvErrorLog.DataSource = this.infoBsErrorLog;
            this.dgvErrorLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvErrorLog.EnterEnable = true;
            this.dgvErrorLog.EnterRefValControl = false;
            this.dgvErrorLog.Location = new System.Drawing.Point(0, 74);
            this.dgvErrorLog.Name = "dgvErrorLog";
            this.dgvErrorLog.RowHeadersWidth = 25;
            this.dgvErrorLog.RowTemplate.Height = 23;
            this.dgvErrorLog.Size = new System.Drawing.Size(511, 440);
            this.dgvErrorLog.SureDelete = false;
            this.dgvErrorLog.TabIndex = 2;
            this.dgvErrorLog.TotalActive = false;
            this.dgvErrorLog.TotalBackColor = System.Drawing.SystemColors.Info;
            this.dgvErrorLog.TotalCaption = null;
            this.dgvErrorLog.TotalCaptionFont = new System.Drawing.Font("宋体", 9F);
            this.dgvErrorLog.TotalFont = new System.Drawing.Font("宋体", 9F);
            this.dgvErrorLog.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvErrorLog_RowEnter);
            // 
            // eRRIDDataGridViewTextBoxColumn
            // 
            this.eRRIDDataGridViewTextBoxColumn.DataPropertyName = "ERRID";
            this.eRRIDDataGridViewTextBoxColumn.HeaderText = "ERRID";
            this.eRRIDDataGridViewTextBoxColumn.Name = "eRRIDDataGridViewTextBoxColumn";
            // 
            // uSERIDDataGridViewTextBoxColumn
            // 
            this.uSERIDDataGridViewTextBoxColumn.DataPropertyName = "USERID";
            this.uSERIDDataGridViewTextBoxColumn.HeaderText = "USERID";
            this.uSERIDDataGridViewTextBoxColumn.Name = "uSERIDDataGridViewTextBoxColumn";
            // 
            // mODULENAMEDataGridViewTextBoxColumn
            // 
            this.mODULENAMEDataGridViewTextBoxColumn.DataPropertyName = "MODULENAME";
            this.mODULENAMEDataGridViewTextBoxColumn.HeaderText = "MODULENAME";
            this.mODULENAMEDataGridViewTextBoxColumn.Name = "mODULENAMEDataGridViewTextBoxColumn";
            // 
            // eRRMESSAGEDataGridViewTextBoxColumn
            // 
            this.eRRMESSAGEDataGridViewTextBoxColumn.DataPropertyName = "ERRMESSAGE";
            this.eRRMESSAGEDataGridViewTextBoxColumn.HeaderText = "ERRMESSAGE";
            this.eRRMESSAGEDataGridViewTextBoxColumn.Name = "eRRMESSAGEDataGridViewTextBoxColumn";
            // 
            // eRRSTACKDataGridViewTextBoxColumn
            // 
            this.eRRSTACKDataGridViewTextBoxColumn.DataPropertyName = "ERRSTACK";
            this.eRRSTACKDataGridViewTextBoxColumn.HeaderText = "ERRSTACK";
            this.eRRSTACKDataGridViewTextBoxColumn.Name = "eRRSTACKDataGridViewTextBoxColumn";
            // 
            // eRRDESCRIPDataGridViewTextBoxColumn
            // 
            this.eRRDESCRIPDataGridViewTextBoxColumn.DataPropertyName = "ERRDESCRIP";
            this.eRRDESCRIPDataGridViewTextBoxColumn.HeaderText = "ERRDESCRIP";
            this.eRRDESCRIPDataGridViewTextBoxColumn.Name = "eRRDESCRIPDataGridViewTextBoxColumn";
            // 
            // eRRDATEDataGridViewTextBoxColumn
            // 
            this.eRRDATEDataGridViewTextBoxColumn.DataPropertyName = "ERRDATE";
            this.eRRDATEDataGridViewTextBoxColumn.HeaderText = "ERRDATE";
            this.eRRDATEDataGridViewTextBoxColumn.Name = "eRRDATEDataGridViewTextBoxColumn";
            // 
            // eRRSCREENDataGridViewTextBoxColumn
            // 
            this.eRRSCREENDataGridViewTextBoxColumn.DataPropertyName = "ERRSCREEN";
            this.eRRSCREENDataGridViewTextBoxColumn.HeaderText = "ERRSCREEN";
            this.eRRSCREENDataGridViewTextBoxColumn.Name = "eRRSCREENDataGridViewTextBoxColumn";
            // 
            // oWNERDataGridViewTextBoxColumn
            // 
            this.oWNERDataGridViewTextBoxColumn.DataPropertyName = "OWNER";
            this.oWNERDataGridViewTextBoxColumn.HeaderText = "OWNER";
            this.oWNERDataGridViewTextBoxColumn.Name = "oWNERDataGridViewTextBoxColumn";
            // 
            // pROCESSDATEDataGridViewTextBoxColumn
            // 
            this.pROCESSDATEDataGridViewTextBoxColumn.DataPropertyName = "PROCESSDATE";
            this.pROCESSDATEDataGridViewTextBoxColumn.HeaderText = "PROCESSDATE";
            this.pROCESSDATEDataGridViewTextBoxColumn.Name = "pROCESSDATEDataGridViewTextBoxColumn";
            // 
            // sTATUSDataGridViewTextBoxColumn
            // 
            this.sTATUSDataGridViewTextBoxColumn.DataPropertyName = "STATUS";
            this.sTATUSDataGridViewTextBoxColumn.HeaderText = "STATUS";
            this.sTATUSDataGridViewTextBoxColumn.Name = "sTATUSDataGridViewTextBoxColumn";
            // 
            // infoBsErrorLog
            // 
            this.infoBsErrorLog.AllowAdd = true;
            this.infoBsErrorLog.AllowDelete = true;
            this.infoBsErrorLog.AllowPrint = true;
            this.infoBsErrorLog.AllowUpdate = true;
            this.infoBsErrorLog.AutoApply = true;
            this.infoBsErrorLog.AutoApplyMaster = false;
            this.infoBsErrorLog.AutoDisableStyle = Srvtools.InfoBindingSource.AutoDisableStyleType.Enabled;
            this.infoBsErrorLog.AutoDisibleControl = false;
            this.infoBsErrorLog.AutoRecordLock = false;
            this.infoBsErrorLog.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload;
            this.infoBsErrorLog.CloseProtect = false;
            this.infoBsErrorLog.DataMember = "cmdERRLOG";
            this.infoBsErrorLog.DataSource = this.infoDsErrorLog;
            this.infoBsErrorLog.DelayInterval = 300;
            this.infoBsErrorLog.DisableKeyFields = false;
            this.infoBsErrorLog.EnableFlag = false;
            this.infoBsErrorLog.FocusedControl = null;
            this.infoBsErrorLog.OwnerComp = null;
            this.infoBsErrorLog.RelationDelay = false;
            this.infoBsErrorLog.text = "infoBsErrorLog";
            // 
            // infoDsErrorLog
            // 
            this.infoDsErrorLog.Active = true;
            this.infoDsErrorLog.AlwaysClose = false;
            this.infoDsErrorLog.DeleteIncomplete = true;
            this.infoDsErrorLog.LastKeyValues = null;
            this.infoDsErrorLog.PacketRecords = -1;
            this.infoDsErrorLog.Position = -1;
            this.infoDsErrorLog.RemoteName = "GLModule.cmdERRLOG";
            this.infoDsErrorLog.ServerModify = false;
            // 
            // infoDateTimeTo
            // 
            this.infoDateTimeTo.DateTimeString = null;
            this.infoDateTimeTo.DateTimeType = Srvtools.InfoDateTimePicker.dtType.DateTime;
            this.infoDateTimeTo.EnterEnable = false;
            this.infoDateTimeTo.Location = new System.Drawing.Point(301, 12);
            this.infoDateTimeTo.Name = "infoDateTimeTo";
            this.infoDateTimeTo.Size = new System.Drawing.Size(146, 21);
            this.infoDateTimeTo.TabIndex = 6;
            this.infoDateTimeTo.Value = new System.DateTime(2006, 5, 19, 0, 0, 0, 0);
            // 
            // infoDateTimeFrom
            // 
            this.infoDateTimeFrom.DateTimeString = null;
            this.infoDateTimeFrom.DateTimeType = Srvtools.InfoDateTimePicker.dtType.DateTime;
            this.infoDateTimeFrom.EnterEnable = false;
            this.infoDateTimeFrom.Location = new System.Drawing.Point(95, 13);
            this.infoDateTimeFrom.Name = "infoDateTimeFrom";
            this.infoDateTimeFrom.Size = new System.Drawing.Size(141, 21);
            this.infoDateTimeFrom.TabIndex = 5;
            this.infoDateTimeFrom.Value = new System.DateTime(2006, 5, 19, 0, 0, 0, 0);
            // 
            // rtbCallStack
            // 
            this.rtbCallStack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.rtbCallStack.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.infoBsErrorLog, "ERRSTACK", true));
            this.rtbCallStack.Location = new System.Drawing.Point(9, 418);
            this.rtbCallStack.Name = "rtbCallStack";
            this.rtbCallStack.Size = new System.Drawing.Size(226, 89);
            this.rtbCallStack.TabIndex = 8;
            this.rtbCallStack.Text = "";
            // 
            // frmErrorLogMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 514);
            this.Controls.Add(this.dgvErrorLog);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmErrorLogMaintenance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ErrorLog Maintenance";
            this.Load += new System.EventHandler(this.frmErrorLogMaintenance_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrorLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoBsErrorLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoDsErrorLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoDateTimeTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoDateTimeFrom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Enlarge;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Srvtools.InfoDataSet infoDsErrorLog;
        private Srvtools.InfoBindingSource infoBsErrorLog;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private Srvtools.InfoDateTimePicker infoDateTimeTo;
        private Srvtools.InfoDateTimePicker infoDateTimeFrom;
        private Srvtools.InfoDataGridView dgvErrorLog;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.RichTextBox ErrorMessage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox errDescrip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn eRRIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uSERIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mODULENAMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eRRMESSAGEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eRRSTACKDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eRRDESCRIPDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eRRDATEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eRRSCREENDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oWNERDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pROCESSDATEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pROCDESCRIPDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sTATUSDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox rtbCallStack;
    }
}