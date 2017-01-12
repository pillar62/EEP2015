namespace EEPNetServer
{
	partial class frmServerConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmServerConfig));
            this.cbxIsMaster = new System.Windows.Forms.CheckBox();
            this.lblMaxUser = new System.Windows.Forms.Label();
            this.lblRemoteMaxUser = new System.Windows.Forms.Label();
            this.lblRemoteIpAddress = new System.Windows.Forms.Label();
            this.txtMaxUser = new System.Windows.Forms.TextBox();
            this.lbxRemoteServer = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtRemoteMaxUser = new System.Windows.Forms.TextBox();
            this.txtRemoteIpAddress = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMasterServerIP = new System.Windows.Forms.TextBox();
            this.lblMasterServerIP = new System.Windows.Forms.Label();
            this.lblMaxTimeOut = new System.Windows.Forms.Label();
            this.txtMaxTimeOut = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBoxSlave = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemoteCurrentUser = new System.Windows.Forms.TextBox();
            this.checkBoxActive = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMasterServerKey = new System.Windows.Forms.TextBox();
            this.txtSSOTimeout = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSSOKey = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxRecordLock = new System.Windows.Forms.CheckBox();
            this.groupBoxSlave.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxIsMaster
            // 
            this.cbxIsMaster.AutoSize = true;
            this.cbxIsMaster.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxIsMaster.ForeColor = System.Drawing.Color.MidnightBlue;
            this.cbxIsMaster.Location = new System.Drawing.Point(12, 440);
            this.cbxIsMaster.Name = "cbxIsMaster";
            this.cbxIsMaster.Size = new System.Drawing.Size(102, 19);
            this.cbxIsMaster.TabIndex = 0;
            this.cbxIsMaster.Text = "Master Server";
            this.cbxIsMaster.Visible = false;
            this.cbxIsMaster.CheckedChanged += new System.EventHandler(this.cbxIsMaster_CheckedChanged);
            // 
            // lblMaxUser
            // 
            this.lblMaxUser.AutoSize = true;
            this.lblMaxUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxUser.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblMaxUser.Location = new System.Drawing.Point(253, 24);
            this.lblMaxUser.Name = "lblMaxUser";
            this.lblMaxUser.Size = new System.Drawing.Size(60, 15);
            this.lblMaxUser.TabIndex = 1;
            this.lblMaxUser.Text = "Max User";
            // 
            // lblRemoteMaxUser
            // 
            this.lblRemoteMaxUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemoteMaxUser.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblRemoteMaxUser.Location = new System.Drawing.Point(233, 98);
            this.lblRemoteMaxUser.Name = "lblRemoteMaxUser";
            this.lblRemoteMaxUser.Size = new System.Drawing.Size(116, 13);
            this.lblRemoteMaxUser.TabIndex = 2;
            this.lblRemoteMaxUser.Text = "Max User Number";
            // 
            // lblRemoteIpAddress
            // 
            this.lblRemoteIpAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemoteIpAddress.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblRemoteIpAddress.Location = new System.Drawing.Point(233, 47);
            this.lblRemoteIpAddress.Name = "lblRemoteIpAddress";
            this.lblRemoteIpAddress.Size = new System.Drawing.Size(124, 15);
            this.lblRemoteIpAddress.TabIndex = 3;
            this.lblRemoteIpAddress.Text = "IP Address";
            // 
            // txtMaxUser
            // 
            this.txtMaxUser.Location = new System.Drawing.Point(324, 21);
            this.txtMaxUser.Name = "txtMaxUser";
            this.txtMaxUser.Size = new System.Drawing.Size(45, 21);
            this.txtMaxUser.TabIndex = 5;
            // 
            // lbxRemoteServer
            // 
            this.lbxRemoteServer.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbxRemoteServer.FormattingEnabled = true;
            this.lbxRemoteServer.ItemHeight = 12;
            this.lbxRemoteServer.Location = new System.Drawing.Point(6, 20);
            this.lbxRemoteServer.Name = "lbxRemoteServer";
            this.lbxRemoteServer.Size = new System.Drawing.Size(206, 148);
            this.lbxRemoteServer.TabIndex = 8;
            this.lbxRemoteServer.SelectedIndexChanged += new System.EventHandler(this.lbxRemoteServer_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(6, 172);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(62, 23);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(359, 440);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnModify
            // 
            this.btnModify.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModify.Location = new System.Drawing.Point(151, 172);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(62, 23);
            this.btnModify.TabIndex = 11;
            this.btnModify.Text = "Modify";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(78, 172);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(62, 23);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // txtRemoteMaxUser
            // 
            this.txtRemoteMaxUser.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemoteMaxUser.Location = new System.Drawing.Point(232, 120);
            this.txtRemoteMaxUser.Name = "txtRemoteMaxUser";
            this.txtRemoteMaxUser.ReadOnly = true;
            this.txtRemoteMaxUser.Size = new System.Drawing.Size(157, 21);
            this.txtRemoteMaxUser.TabIndex = 7;
            // 
            // txtRemoteIpAddress
            // 
            this.txtRemoteIpAddress.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemoteIpAddress.Location = new System.Drawing.Point(232, 71);
            this.txtRemoteIpAddress.Name = "txtRemoteIpAddress";
            this.txtRemoteIpAddress.ReadOnly = true;
            this.txtRemoteIpAddress.Size = new System.Drawing.Size(157, 21);
            this.txtRemoteIpAddress.TabIndex = 6;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.Location = new System.Drawing.Point(324, 51);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(45, 21);
            this.textBox1.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(237, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "Current User";
            // 
            // txtMasterServerIP
            // 
            this.txtMasterServerIP.Location = new System.Drawing.Point(117, 21);
            this.txtMasterServerIP.Name = "txtMasterServerIP";
            this.txtMasterServerIP.Size = new System.Drawing.Size(110, 21);
            this.txtMasterServerIP.TabIndex = 18;
            // 
            // lblMasterServerIP
            // 
            this.lblMasterServerIP.AutoSize = true;
            this.lblMasterServerIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMasterServerIP.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblMasterServerIP.Location = new System.Drawing.Point(15, 24);
            this.lblMasterServerIP.Name = "lblMasterServerIP";
            this.lblMasterServerIP.Size = new System.Drawing.Size(91, 15);
            this.lblMasterServerIP.TabIndex = 17;
            this.lblMasterServerIP.Text = "Mater Server IP";
            // 
            // lblMaxTimeOut
            // 
            this.lblMaxTimeOut.AutoSize = true;
            this.lblMaxTimeOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblMaxTimeOut.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblMaxTimeOut.Location = new System.Drawing.Point(6, 20);
            this.lblMaxTimeOut.Name = "lblMaxTimeOut";
            this.lblMaxTimeOut.Size = new System.Drawing.Size(79, 15);
            this.lblMaxTimeOut.TabIndex = 19;
            this.lblMaxTimeOut.Text = "Max Timeout";
            // 
            // txtMaxTimeOut
            // 
            this.txtMaxTimeOut.Location = new System.Drawing.Point(95, 17);
            this.txtMaxTimeOut.MaxLength = 10;
            this.txtMaxTimeOut.Name = "txtMaxTimeOut";
            this.txtMaxTimeOut.Size = new System.Drawing.Size(74, 21);
            this.txtMaxTimeOut.TabIndex = 20;
            this.txtMaxTimeOut.Text = "0";
            this.txtMaxTimeOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 21;
            this.label2.Text = "hours";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(260, 440);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(73, 23);
            this.btnOk.TabIndex = 22;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBoxSlave
            // 
            this.groupBoxSlave.Controls.Add(this.label3);
            this.groupBoxSlave.Controls.Add(this.txtRemoteCurrentUser);
            this.groupBoxSlave.Controls.Add(this.checkBoxActive);
            this.groupBoxSlave.Controls.Add(this.lbxRemoteServer);
            this.groupBoxSlave.Controls.Add(this.btnAdd);
            this.groupBoxSlave.Controls.Add(this.btnModify);
            this.groupBoxSlave.Controls.Add(this.btnDelete);
            this.groupBoxSlave.Controls.Add(this.lblRemoteIpAddress);
            this.groupBoxSlave.Controls.Add(this.lblRemoteMaxUser);
            this.groupBoxSlave.Controls.Add(this.txtRemoteIpAddress);
            this.groupBoxSlave.Controls.Add(this.txtRemoteMaxUser);
            this.groupBoxSlave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSlave.Location = new System.Drawing.Point(22, 233);
            this.groupBoxSlave.Name = "groupBoxSlave";
            this.groupBoxSlave.Size = new System.Drawing.Size(410, 201);
            this.groupBoxSlave.TabIndex = 23;
            this.groupBoxSlave.TabStop = false;
            this.groupBoxSlave.Text = "Slave Server";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label3.Location = new System.Drawing.Point(234, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 15);
            this.label3.TabIndex = 19;
            this.label3.Text = "Current User Number";
            // 
            // txtRemoteCurrentUser
            // 
            this.txtRemoteCurrentUser.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemoteCurrentUser.Location = new System.Drawing.Point(232, 168);
            this.txtRemoteCurrentUser.Name = "txtRemoteCurrentUser";
            this.txtRemoteCurrentUser.ReadOnly = true;
            this.txtRemoteCurrentUser.Size = new System.Drawing.Size(157, 21);
            this.txtRemoteCurrentUser.TabIndex = 18;
            // 
            // checkBoxActive
            // 
            this.checkBoxActive.AutoSize = true;
            this.checkBoxActive.ForeColor = System.Drawing.Color.MidnightBlue;
            this.checkBoxActive.Location = new System.Drawing.Point(235, 23);
            this.checkBoxActive.Name = "checkBoxActive";
            this.checkBoxActive.Size = new System.Drawing.Size(57, 19);
            this.checkBoxActive.TabIndex = 17;
            this.checkBoxActive.Text = "Active";
            this.checkBoxActive.UseVisualStyleBackColor = true;
            this.checkBoxActive.CheckedChanged += new System.EventHandler(this.checkBoxActive_CheckedChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(375, 49);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(29, 22);
            this.btnRefresh.TabIndex = 24;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label4.Location = new System.Drawing.Point(6, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 15);
            this.label4.TabIndex = 25;
            this.label4.Text = "Mater Server Key";
            // 
            // txtMasterServerKey
            // 
            this.txtMasterServerKey.Location = new System.Drawing.Point(117, 51);
            this.txtMasterServerKey.Name = "txtMasterServerKey";
            this.txtMasterServerKey.Size = new System.Drawing.Size(110, 21);
            this.txtMasterServerKey.TabIndex = 26;
            // 
            // txtSSOTimeout
            // 
            this.txtSSOTimeout.Location = new System.Drawing.Point(95, 44);
            this.txtSSOTimeout.MaxLength = 10;
            this.txtSSOTimeout.Name = "txtSSOTimeout";
            this.txtSSOTimeout.Size = new System.Drawing.Size(74, 21);
            this.txtSSOTimeout.TabIndex = 27;
            this.txtSSOTimeout.Text = "24";
            this.txtSSOTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label5.Location = new System.Drawing.Point(6, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 15);
            this.label5.TabIndex = 28;
            this.label5.Text = "SSO Timeout";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(175, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 15);
            this.label6.TabIndex = 29;
            this.label6.Text = "hours";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtSSOKey);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblMaxTimeOut);
            this.groupBox1.Controls.Add(this.txtSSOTimeout);
            this.groupBox1.Controls.Add(this.txtMaxTimeOut);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.groupBox1.Location = new System.Drawing.Point(22, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 71);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Timeout";
            // 
            // txtSSOKey
            // 
            this.txtSSOKey.Location = new System.Drawing.Point(280, 44);
            this.txtSSOKey.MaxLength = 10;
            this.txtSSOKey.Name = "txtSSOKey";
            this.txtSSOKey.Size = new System.Drawing.Size(112, 21);
            this.txtSSOKey.TabIndex = 30;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label7.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label7.Location = new System.Drawing.Point(219, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 15);
            this.label7.TabIndex = 31;
            this.label7.Text = "SSO Key";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtMasterServerIP);
            this.groupBox2.Controls.Add(this.lblMaxUser);
            this.groupBox2.Controls.Add(this.txtMasterServerKey);
            this.groupBox2.Controls.Add(this.txtMaxUser);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnRefresh);
            this.groupBox2.Controls.Add(this.lblMasterServerIP);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.groupBox2.Location = new System.Drawing.Point(22, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(410, 86);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Master Server";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxRecordLock);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.groupBox3.Location = new System.Drawing.Point(22, 83);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(410, 52);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Other";
            // 
            // checkBoxRecordLock
            // 
            this.checkBoxRecordLock.AutoSize = true;
            this.checkBoxRecordLock.ForeColor = System.Drawing.Color.MidnightBlue;
            this.checkBoxRecordLock.Location = new System.Drawing.Point(10, 21);
            this.checkBoxRecordLock.Name = "checkBoxRecordLock";
            this.checkBoxRecordLock.Size = new System.Drawing.Size(198, 19);
            this.checkBoxRecordLock.TabIndex = 0;
            this.checkBoxRecordLock.Text = "Save Record Lock To Database";
            this.checkBoxRecordLock.UseVisualStyleBackColor = true;
            // 
            // frmServerConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 474);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbxIsMaster);
            this.Controls.Add(this.groupBoxSlave);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmServerConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server Config";
            this.Load += new System.EventHandler(this.frmServerConfig_Load);
            this.groupBoxSlave.ResumeLayout(false);
            this.groupBoxSlave.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox cbxIsMaster;
		private System.Windows.Forms.Label lblMaxUser;
		private System.Windows.Forms.Label lblRemoteMaxUser;
        private System.Windows.Forms.Label lblRemoteIpAddress;
		private System.Windows.Forms.TextBox txtMaxUser;
		private System.Windows.Forms.ListBox lbxRemoteServer;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnModify;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.TextBox txtRemoteMaxUser;
        private System.Windows.Forms.TextBox txtRemoteIpAddress;
		private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtMasterServerIP;
		private System.Windows.Forms.Label lblMasterServerIP;
        private System.Windows.Forms.Label lblMaxTimeOut;
        private System.Windows.Forms.TextBox txtMaxTimeOut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBoxSlave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox checkBoxActive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRemoteCurrentUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMasterServerKey;
        private System.Windows.Forms.TextBox txtSSOTimeout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSSOKey;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxRecordLock;
	}
}