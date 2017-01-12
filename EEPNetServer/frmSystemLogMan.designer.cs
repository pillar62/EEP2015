namespace EEPNetServer
{
    partial class frmSystemLogMan
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
            this.chkDisable = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblMaxRecord = new System.Windows.Forms.Label();
            this.txtMaxRecord = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMaxLogDays = new System.Windows.Forms.TextBox();
            this.chkLogSql = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMaxSize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonDB = new System.Windows.Forms.RadioButton();
            this.linkLabelDetail = new System.Windows.Forms.LinkLabel();
            this.radioButtonFile = new System.Windows.Forms.RadioButton();
            this.groupBoxEnableLogs = new System.Windows.Forms.GroupBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkBoxEmail = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxInterval = new System.Windows.Forms.TextBox();
            this.checkBoxUnkown = new System.Windows.Forms.CheckBox();
            this.checkBoxError = new System.Windows.Forms.CheckBox();
            this.checkBoxWarning = new System.Windows.Forms.CheckBox();
            this.checkBoxNormal = new System.Windows.Forms.CheckBox();
            this.CheckBoxUserDefine = new System.Windows.Forms.CheckBox();
            this.checkBoxCallMethod = new System.Windows.Forms.CheckBox();
            this.checkBoxProvider = new System.Windows.Forms.CheckBox();
            this.checkBoxSystem = new System.Windows.Forms.CheckBox();
            this.groupBoxNotifySetting = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbUnkownNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbErrorNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbWarningNo = new System.Windows.Forms.TextBox();
            this.tbWarningEMail = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbErrorEMail = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbUnkownEMail = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBoxEnableLogs.SuspendLayout();
            this.groupBoxNotifySetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkDisable
            // 
            this.chkDisable.AutoSize = true;
            this.chkDisable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDisable.ForeColor = System.Drawing.Color.MidnightBlue;
            this.chkDisable.Location = new System.Drawing.Point(26, 8);
            this.chkDisable.Name = "chkDisable";
            this.chkDisable.Size = new System.Drawing.Size(130, 19);
            this.chkDisable.TabIndex = 0;
            this.chkDisable.Text = "Disable Server Log";
            this.chkDisable.CheckedChanged += new System.EventHandler(this.chkDisable_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(307, 154);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(64, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblMaxRecord
            // 
            this.lblMaxRecord.AutoSize = true;
            this.lblMaxRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxRecord.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblMaxRecord.Location = new System.Drawing.Point(259, 70);
            this.lblMaxRecord.Name = "lblMaxRecord";
            this.lblMaxRecord.Size = new System.Drawing.Size(99, 15);
            this.lblMaxRecord.TabIndex = 5;
            this.lblMaxRecord.Text = "Keep records:(K)";
            // 
            // txtMaxRecord
            // 
            this.txtMaxRecord.Location = new System.Drawing.Point(364, 69);
            this.txtMaxRecord.Name = "txtMaxRecord";
            this.txtMaxRecord.Size = new System.Drawing.Size(80, 21);
            this.txtMaxRecord.TabIndex = 6;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(393, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(259, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Reserve days:(D)";
            // 
            // txtMaxLogDays
            // 
            this.txtMaxLogDays.Location = new System.Drawing.Point(364, 19);
            this.txtMaxLogDays.Name = "txtMaxLogDays";
            this.txtMaxLogDays.Size = new System.Drawing.Size(80, 21);
            this.txtMaxLogDays.TabIndex = 4;
            // 
            // chkLogSql
            // 
            this.chkLogSql.AutoSize = true;
            this.chkLogSql.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.chkLogSql.ForeColor = System.Drawing.Color.MidnightBlue;
            this.chkLogSql.Location = new System.Drawing.Point(15, 93);
            this.chkLogSql.Name = "chkLogSql";
            this.chkLogSql.Size = new System.Drawing.Size(193, 19);
            this.chkLogSql.TabIndex = 2;
            this.chkLogSql.Text = "Also Log SQL  (save to text file)";
            this.chkLogSql.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMaxSize);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.radioButtonDB);
            this.groupBox1.Controls.Add(this.linkLabelDetail);
            this.groupBox1.Controls.Add(this.lblMaxRecord);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtMaxLogDays);
            this.groupBox1.Controls.Add(this.chkLogSql);
            this.groupBox1.Controls.Add(this.txtMaxRecord);
            this.groupBox1.Controls.Add(this.radioButtonFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(459, 119);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtMaxSize
            // 
            this.txtMaxSize.Location = new System.Drawing.Point(364, 44);
            this.txtMaxSize.Name = "txtMaxSize";
            this.txtMaxSize.Size = new System.Drawing.Size(80, 21);
            this.txtMaxSize.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label3.Location = new System.Drawing.Point(259, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Max size:(K)";
            // 
            // radioButtonDB
            // 
            this.radioButtonDB.AutoSize = true;
            this.radioButtonDB.Location = new System.Drawing.Point(15, 69);
            this.radioButtonDB.Name = "radioButtonDB";
            this.radioButtonDB.Size = new System.Drawing.Size(215, 16);
            this.radioButtonDB.TabIndex = 1;
            this.radioButtonDB.TabStop = true;
            this.radioButtonDB.Text = "Log to Database(table.syseeplog)";
            this.radioButtonDB.UseVisualStyleBackColor = true;
            // 
            // linkLabelDetail
            // 
            this.linkLabelDetail.AutoSize = true;
            this.linkLabelDetail.Location = new System.Drawing.Point(339, 98);
            this.linkLabelDetail.Name = "linkLabelDetail";
            this.linkLabelDetail.Size = new System.Drawing.Size(95, 12);
            this.linkLabelDetail.TabIndex = 2;
            this.linkLabelDetail.TabStop = true;
            this.linkLabelDetail.Text = "Detail Settings";
            this.linkLabelDetail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDetail_LinkClicked);
            // 
            // radioButtonFile
            // 
            this.radioButtonFile.AutoSize = true;
            this.radioButtonFile.Location = new System.Drawing.Point(15, 19);
            this.radioButtonFile.Name = "radioButtonFile";
            this.radioButtonFile.Size = new System.Drawing.Size(227, 16);
            this.radioButtonFile.TabIndex = 0;
            this.radioButtonFile.TabStop = true;
            this.radioButtonFile.Text = "Log to text file(separate by days)";
            this.radioButtonFile.UseVisualStyleBackColor = true;
            this.radioButtonFile.CheckedChanged += new System.EventHandler(this.chkLogToFile_CheckedChanged);
            // 
            // groupBoxEnableLogs
            // 
            this.groupBoxEnableLogs.Controls.Add(this.linkLabel1);
            this.groupBoxEnableLogs.Controls.Add(this.checkBoxEmail);
            this.groupBoxEnableLogs.Controls.Add(this.label2);
            this.groupBoxEnableLogs.Controls.Add(this.textBoxInterval);
            this.groupBoxEnableLogs.Controls.Add(this.checkBoxUnkown);
            this.groupBoxEnableLogs.Controls.Add(this.checkBoxError);
            this.groupBoxEnableLogs.Controls.Add(this.checkBoxWarning);
            this.groupBoxEnableLogs.Controls.Add(this.checkBoxNormal);
            this.groupBoxEnableLogs.Controls.Add(this.CheckBoxUserDefine);
            this.groupBoxEnableLogs.Controls.Add(this.checkBoxCallMethod);
            this.groupBoxEnableLogs.Controls.Add(this.checkBoxProvider);
            this.groupBoxEnableLogs.Controls.Add(this.checkBoxSystem);
            this.groupBoxEnableLogs.Location = new System.Drawing.Point(12, 146);
            this.groupBoxEnableLogs.Name = "groupBoxEnableLogs";
            this.groupBoxEnableLogs.Size = new System.Drawing.Size(456, 119);
            this.groupBoxEnableLogs.TabIndex = 3;
            this.groupBoxEnableLogs.TabStop = false;
            this.groupBoxEnableLogs.Text = "Enable Logs";
            this.groupBoxEnableLogs.Visible = false;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(339, 96);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(95, 12);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Notify Settings";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // checkBoxEmail
            // 
            this.checkBoxEmail.AutoSize = true;
            this.checkBoxEmail.Location = new System.Drawing.Point(370, 29);
            this.checkBoxEmail.Name = "checkBoxEmail";
            this.checkBoxEmail.Size = new System.Drawing.Size(54, 16);
            this.checkBoxEmail.TabIndex = 10;
            this.checkBoxEmail.Text = "Email";
            this.checkBoxEmail.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Warning Interval:";
            // 
            // textBoxInterval
            // 
            this.textBoxInterval.Location = new System.Drawing.Point(132, 91);
            this.textBoxInterval.Name = "textBoxInterval";
            this.textBoxInterval.Size = new System.Drawing.Size(100, 21);
            this.textBoxInterval.TabIndex = 9;
            // 
            // checkBoxUnkown
            // 
            this.checkBoxUnkown.AutoSize = true;
            this.checkBoxUnkown.Location = new System.Drawing.Point(355, 62);
            this.checkBoxUnkown.Name = "checkBoxUnkown";
            this.checkBoxUnkown.Size = new System.Drawing.Size(60, 16);
            this.checkBoxUnkown.TabIndex = 7;
            this.checkBoxUnkown.Text = "&Unkown";
            this.checkBoxUnkown.UseVisualStyleBackColor = true;
            // 
            // checkBoxError
            // 
            this.checkBoxError.AutoSize = true;
            this.checkBoxError.Location = new System.Drawing.Point(243, 62);
            this.checkBoxError.Name = "checkBoxError";
            this.checkBoxError.Size = new System.Drawing.Size(54, 16);
            this.checkBoxError.TabIndex = 6;
            this.checkBoxError.Text = "&Error";
            this.checkBoxError.UseVisualStyleBackColor = true;
            // 
            // checkBoxWarning
            // 
            this.checkBoxWarning.AutoSize = true;
            this.checkBoxWarning.Location = new System.Drawing.Point(131, 62);
            this.checkBoxWarning.Name = "checkBoxWarning";
            this.checkBoxWarning.Size = new System.Drawing.Size(66, 16);
            this.checkBoxWarning.TabIndex = 5;
            this.checkBoxWarning.Text = "&Warning";
            this.checkBoxWarning.UseVisualStyleBackColor = true;
            // 
            // checkBoxNormal
            // 
            this.checkBoxNormal.AutoSize = true;
            this.checkBoxNormal.Enabled = false;
            this.checkBoxNormal.Location = new System.Drawing.Point(19, 62);
            this.checkBoxNormal.Name = "checkBoxNormal";
            this.checkBoxNormal.Size = new System.Drawing.Size(60, 16);
            this.checkBoxNormal.TabIndex = 4;
            this.checkBoxNormal.Text = "&Normal";
            this.checkBoxNormal.UseVisualStyleBackColor = true;
            // 
            // CheckBoxUserDefine
            // 
            this.CheckBoxUserDefine.AutoSize = true;
            this.CheckBoxUserDefine.Location = new System.Drawing.Point(270, 30);
            this.CheckBoxUserDefine.Name = "CheckBoxUserDefine";
            this.CheckBoxUserDefine.Size = new System.Drawing.Size(90, 16);
            this.CheckBoxUserDefine.TabIndex = 3;
            this.CheckBoxUserDefine.Text = "&User Define";
            this.CheckBoxUserDefine.UseVisualStyleBackColor = true;
            // 
            // checkBoxCallMethod
            // 
            this.checkBoxCallMethod.AutoSize = true;
            this.checkBoxCallMethod.Location = new System.Drawing.Point(170, 30);
            this.checkBoxCallMethod.Name = "checkBoxCallMethod";
            this.checkBoxCallMethod.Size = new System.Drawing.Size(90, 16);
            this.checkBoxCallMethod.TabIndex = 2;
            this.checkBoxCallMethod.Text = "&Call Method";
            this.checkBoxCallMethod.UseVisualStyleBackColor = true;
            // 
            // checkBoxProvider
            // 
            this.checkBoxProvider.AutoSize = true;
            this.checkBoxProvider.Location = new System.Drawing.Point(88, 30);
            this.checkBoxProvider.Name = "checkBoxProvider";
            this.checkBoxProvider.Size = new System.Drawing.Size(72, 16);
            this.checkBoxProvider.TabIndex = 1;
            this.checkBoxProvider.Text = "&Provider";
            this.checkBoxProvider.UseVisualStyleBackColor = true;
            // 
            // checkBoxSystem
            // 
            this.checkBoxSystem.AutoSize = true;
            this.checkBoxSystem.Location = new System.Drawing.Point(18, 30);
            this.checkBoxSystem.Name = "checkBoxSystem";
            this.checkBoxSystem.Size = new System.Drawing.Size(60, 16);
            this.checkBoxSystem.TabIndex = 0;
            this.checkBoxSystem.Text = "&System";
            this.checkBoxSystem.UseVisualStyleBackColor = true;
            // 
            // groupBoxNotifySetting
            // 
            this.groupBoxNotifySetting.Controls.Add(this.tbUnkownEMail);
            this.groupBoxNotifySetting.Controls.Add(this.label14);
            this.groupBoxNotifySetting.Controls.Add(this.tbErrorEMail);
            this.groupBoxNotifySetting.Controls.Add(this.label9);
            this.groupBoxNotifySetting.Controls.Add(this.tbWarningEMail);
            this.groupBoxNotifySetting.Controls.Add(this.label8);
            this.groupBoxNotifySetting.Controls.Add(this.label7);
            this.groupBoxNotifySetting.Controls.Add(this.tbUnkownNo);
            this.groupBoxNotifySetting.Controls.Add(this.label6);
            this.groupBoxNotifySetting.Controls.Add(this.tbErrorNo);
            this.groupBoxNotifySetting.Controls.Add(this.label5);
            this.groupBoxNotifySetting.Controls.Add(this.tbWarningNo);
            this.groupBoxNotifySetting.Location = new System.Drawing.Point(12, 271);
            this.groupBoxNotifySetting.Name = "groupBoxNotifySetting";
            this.groupBoxNotifySetting.Size = new System.Drawing.Size(456, 159);
            this.groupBoxNotifySetting.TabIndex = 11;
            this.groupBoxNotifySetting.TabStop = false;
            this.groupBoxNotifySetting.Text = "Notify Settings";
            this.groupBoxNotifySetting.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "Unkown No.:";
            // 
            // tbUnkownNo
            // 
            this.tbUnkownNo.Location = new System.Drawing.Point(118, 129);
            this.tbUnkownNo.Name = "tbUnkownNo";
            this.tbUnkownNo.Size = new System.Drawing.Size(134, 21);
            this.tbUnkownNo.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "Error No.:";
            // 
            // tbErrorNo
            // 
            this.tbErrorNo.Location = new System.Drawing.Point(118, 110);
            this.tbErrorNo.Name = "tbErrorNo";
            this.tbErrorNo.Size = new System.Drawing.Size(134, 21);
            this.tbErrorNo.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "Warning No.:";
            // 
            // tbWarningNo
            // 
            this.tbWarningNo.Location = new System.Drawing.Point(118, 91);
            this.tbWarningNo.Name = "tbWarningNo";
            this.tbWarningNo.Size = new System.Drawing.Size(132, 21);
            this.tbWarningNo.TabIndex = 11;
            // 
            // tbWarningEMail
            // 
            this.tbWarningEMail.Location = new System.Drawing.Point(117, 20);
            this.tbWarningEMail.Name = "tbWarningEMail";
            this.tbWarningEMail.Size = new System.Drawing.Size(200, 21);
            this.tbWarningEMail.TabIndex = 37;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 12);
            this.label8.TabIndex = 38;
            this.label8.Text = "Warning E-mail:";
            // 
            // tbErrorEMail
            // 
            this.tbErrorEMail.Location = new System.Drawing.Point(117, 41);
            this.tbErrorEMail.Name = "tbErrorEMail";
            this.tbErrorEMail.Size = new System.Drawing.Size(200, 21);
            this.tbErrorEMail.TabIndex = 39;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 12);
            this.label9.TabIndex = 40;
            this.label9.Text = "Error E-mail:";
            // 
            // tbUnkownEMail
            // 
            this.tbUnkownEMail.Location = new System.Drawing.Point(118, 64);
            this.tbUnkownEMail.Name = "tbUnkownEMail";
            this.tbUnkownEMail.Size = new System.Drawing.Size(199, 21);
            this.tbUnkownEMail.TabIndex = 41;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 64);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(89, 12);
            this.label14.TabIndex = 42;
            this.label14.Text = "Unkown E-mail:";
            // 
            // frmSystemLogMan
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(487, 186);
            this.Controls.Add(this.groupBoxNotifySetting);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkDisable);
            this.Controls.Add(this.groupBoxEnableLogs);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSystemLogMan";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Log Manager";
            this.Load += new System.EventHandler(this.frmSystemLogMan_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxEnableLogs.ResumeLayout(false);
            this.groupBoxEnableLogs.PerformLayout();
            this.groupBoxNotifySetting.ResumeLayout(false);
            this.groupBoxNotifySetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDisable;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtMaxRecord;
        private System.Windows.Forms.Label lblMaxRecord;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMaxLogDays;
        private System.Windows.Forms.CheckBox chkLogSql;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonDB;
        private System.Windows.Forms.RadioButton radioButtonFile;
        private System.Windows.Forms.LinkLabel linkLabelDetail;
        private System.Windows.Forms.GroupBox groupBoxEnableLogs;
        private System.Windows.Forms.CheckBox CheckBoxUserDefine;
        private System.Windows.Forms.CheckBox checkBoxCallMethod;
        private System.Windows.Forms.CheckBox checkBoxProvider;
        private System.Windows.Forms.CheckBox checkBoxSystem;
        private System.Windows.Forms.CheckBox checkBoxUnkown;
        private System.Windows.Forms.CheckBox checkBoxError;
        private System.Windows.Forms.CheckBox checkBoxWarning;
        private System.Windows.Forms.CheckBox checkBoxNormal;
        private System.Windows.Forms.TextBox textBoxInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMaxSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxEmail;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.GroupBox groupBoxNotifySetting;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbUnkownNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbErrorNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbWarningNo;
        private System.Windows.Forms.TextBox tbUnkownEMail;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbErrorEMail;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbWarningEMail;
        private System.Windows.Forms.Label label8;
    }
}