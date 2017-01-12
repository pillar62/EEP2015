namespace EEPNetServer
{
    partial class frmWorkflowConfig
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
            this.btnOK = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbSMS = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbEnableSSL = new System.Windows.Forms.CheckBox();
            this.lblEnbleSSL = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkContent2 = new System.Windows.Forms.CheckBox();
            this.chkDescription2 = new System.Windows.Forms.CheckBox();
            this.chkActivityName2 = new System.Windows.Forms.CheckBox();
            this.chkFlowName2 = new System.Windows.Forms.CheckBox();
            this.chkSender2 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxActivityDescription = new System.Windows.Forms.CheckBox();
            this.checkBoxRejectButton = new System.Windows.Forms.CheckBox();
            this.checkBoxReturnButton = new System.Windows.Forms.CheckBox();
            this.checkBoxApproveButton = new System.Windows.Forms.CheckBox();
            this.chkComment = new System.Windows.Forms.CheckBox();
            this.chkHyperLink = new System.Windows.Forms.CheckBox();
            this.chkDateTime = new System.Windows.Forms.CheckBox();
            this.chkDescription = new System.Windows.Forms.CheckBox();
            this.chkContent = new System.Windows.Forms.CheckBox();
            this.chkActivityName = new System.Windows.Forms.CheckBox();
            this.chkFlowName = new System.Windows.Forms.CheckBox();
            this.chkSender = new System.Windows.Forms.CheckBox();
            this.txtSMTP = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblSMTP = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBoxPushService = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBox14 = new System.Windows.Forms.CheckBox();
            this.checkBox15 = new System.Windows.Forms.CheckBox();
            this.checkBox16 = new System.Windows.Forms.CheckBox();
            this.checkBox17 = new System.Windows.Forms.CheckBox();
            this.checkBox18 = new System.Windows.Forms.CheckBox();
            this.checkBox19 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.textBoxP12FileName = new System.Windows.Forms.TextBox();
            this.textBoxP12Password = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(294, 514);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(397, 508);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbSMS);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.cbEnableSSL);
            this.tabPage1.Controls.Add(this.lblEnbleSSL);
            this.tabPage1.Controls.Add(this.tbPort);
            this.tabPage1.Controls.Add(this.lblPort);
            this.tabPage1.Controls.Add(this.chkActive);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.txtSMTP);
            this.tabPage1.Controls.Add(this.txtPassword);
            this.tabPage1.Controls.Add(this.txtEmail);
            this.tabPage1.Controls.Add(this.lblSMTP);
            this.tabPage1.Controls.Add(this.lblPassword);
            this.tabPage1.Controls.Add(this.lblEmail);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(389, 482);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Email";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbSMS
            // 
            this.tbSMS.Location = new System.Drawing.Point(129, 177);
            this.tbSMS.Name = "tbSMS";
            this.tbSMS.Size = new System.Drawing.Size(236, 21);
            this.tbSMS.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 28;
            this.label1.Text = "SMS:";
            // 
            // cbEnableSSL
            // 
            this.cbEnableSSL.AutoSize = true;
            this.cbEnableSSL.Location = new System.Drawing.Point(129, 146);
            this.cbEnableSSL.Name = "cbEnableSSL";
            this.cbEnableSSL.Size = new System.Drawing.Size(15, 14);
            this.cbEnableSSL.TabIndex = 27;
            this.cbEnableSSL.UseVisualStyleBackColor = true;
            // 
            // lblEnbleSSL
            // 
            this.lblEnbleSSL.AutoSize = true;
            this.lblEnbleSSL.Location = new System.Drawing.Point(15, 147);
            this.lblEnbleSSL.Name = "lblEnbleSSL";
            this.lblEnbleSSL.Size = new System.Drawing.Size(95, 12);
            this.lblEnbleSSL.TabIndex = 26;
            this.lblEnbleSSL.Text = "Enable TLS/SSL:";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(129, 115);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(236, 21);
            this.tbPort.TabIndex = 25;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(15, 118);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(35, 12);
            this.lblPort.TabIndex = 24;
            this.lblPort.Text = "Port:";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(17, 6);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(102, 16);
            this.chkActive.TabIndex = 23;
            this.chkActive.Text = "EMail Activie";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkContent2);
            this.groupBox2.Controls.Add(this.chkDescription2);
            this.groupBox2.Controls.Add(this.chkActivityName2);
            this.groupBox2.Controls.Add(this.chkFlowName2);
            this.groupBox2.Controls.Add(this.chkSender2);
            this.groupBox2.Location = new System.Drawing.Point(17, 214);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(348, 86);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mail Subject";
            // 
            // chkContent2
            // 
            this.chkContent2.AutoSize = true;
            this.chkContent2.Location = new System.Drawing.Point(25, 66);
            this.chkContent2.Name = "chkContent2";
            this.chkContent2.Size = new System.Drawing.Size(66, 16);
            this.chkContent2.TabIndex = 7;
            this.chkContent2.Text = "Content";
            this.chkContent2.UseVisualStyleBackColor = true;
            // 
            // chkDescription2
            // 
            this.chkDescription2.AutoSize = true;
            this.chkDescription2.Location = new System.Drawing.Point(213, 44);
            this.chkDescription2.Name = "chkDescription2";
            this.chkDescription2.Size = new System.Drawing.Size(90, 16);
            this.chkDescription2.TabIndex = 8;
            this.chkDescription2.Text = "Description";
            this.chkDescription2.UseVisualStyleBackColor = true;
            // 
            // chkActivityName2
            // 
            this.chkActivityName2.AutoSize = true;
            this.chkActivityName2.Location = new System.Drawing.Point(26, 44);
            this.chkActivityName2.Name = "chkActivityName2";
            this.chkActivityName2.Size = new System.Drawing.Size(96, 16);
            this.chkActivityName2.TabIndex = 9;
            this.chkActivityName2.Text = "ActivityName";
            this.chkActivityName2.UseVisualStyleBackColor = true;
            // 
            // chkFlowName2
            // 
            this.chkFlowName2.AutoSize = true;
            this.chkFlowName2.Location = new System.Drawing.Point(213, 20);
            this.chkFlowName2.Name = "chkFlowName2";
            this.chkFlowName2.Size = new System.Drawing.Size(72, 16);
            this.chkFlowName2.TabIndex = 6;
            this.chkFlowName2.Text = "FlowName";
            this.chkFlowName2.UseVisualStyleBackColor = true;
            // 
            // chkSender2
            // 
            this.chkSender2.AutoSize = true;
            this.chkSender2.Location = new System.Drawing.Point(26, 20);
            this.chkSender2.Name = "chkSender2";
            this.chkSender2.Size = new System.Drawing.Size(60, 16);
            this.chkSender2.TabIndex = 7;
            this.chkSender2.Text = "Sender";
            this.chkSender2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxActivityDescription);
            this.groupBox1.Controls.Add(this.checkBoxRejectButton);
            this.groupBox1.Controls.Add(this.checkBoxReturnButton);
            this.groupBox1.Controls.Add(this.checkBoxApproveButton);
            this.groupBox1.Controls.Add(this.chkComment);
            this.groupBox1.Controls.Add(this.chkHyperLink);
            this.groupBox1.Controls.Add(this.chkDateTime);
            this.groupBox1.Controls.Add(this.chkDescription);
            this.groupBox1.Controls.Add(this.chkContent);
            this.groupBox1.Controls.Add(this.chkActivityName);
            this.groupBox1.Controls.Add(this.chkFlowName);
            this.groupBox1.Controls.Add(this.chkSender);
            this.groupBox1.Location = new System.Drawing.Point(17, 307);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(348, 152);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Email Body";
            // 
            // checkBoxActivityDescription
            // 
            this.checkBoxActivityDescription.AutoSize = true;
            this.checkBoxActivityDescription.Location = new System.Drawing.Point(24, 110);
            this.checkBoxActivityDescription.Name = "checkBoxActivityDescription";
            this.checkBoxActivityDescription.Size = new System.Drawing.Size(144, 16);
            this.checkBoxActivityDescription.TabIndex = 10;
            this.checkBoxActivityDescription.Text = "Activity Description";
            this.checkBoxActivityDescription.UseVisualStyleBackColor = true;
            // 
            // checkBoxRejectButton
            // 
            this.checkBoxRejectButton.AutoSize = true;
            this.checkBoxRejectButton.Location = new System.Drawing.Point(229, 128);
            this.checkBoxRejectButton.Name = "checkBoxRejectButton";
            this.checkBoxRejectButton.Size = new System.Drawing.Size(102, 16);
            this.checkBoxRejectButton.TabIndex = 9;
            this.checkBoxRejectButton.Text = "Reject Button";
            this.checkBoxRejectButton.UseVisualStyleBackColor = true;
            // 
            // checkBoxReturnButton
            // 
            this.checkBoxReturnButton.AutoSize = true;
            this.checkBoxReturnButton.Location = new System.Drawing.Point(121, 128);
            this.checkBoxReturnButton.Name = "checkBoxReturnButton";
            this.checkBoxReturnButton.Size = new System.Drawing.Size(102, 16);
            this.checkBoxReturnButton.TabIndex = 8;
            this.checkBoxReturnButton.Text = "Return Button";
            this.checkBoxReturnButton.UseVisualStyleBackColor = true;
            // 
            // checkBoxApproveButton
            // 
            this.checkBoxApproveButton.AutoSize = true;
            this.checkBoxApproveButton.Location = new System.Drawing.Point(7, 128);
            this.checkBoxApproveButton.Name = "checkBoxApproveButton";
            this.checkBoxApproveButton.Size = new System.Drawing.Size(108, 16);
            this.checkBoxApproveButton.TabIndex = 7;
            this.checkBoxApproveButton.Text = "Approve Button";
            this.checkBoxApproveButton.UseVisualStyleBackColor = true;
            // 
            // chkComment
            // 
            this.chkComment.AutoSize = true;
            this.chkComment.Location = new System.Drawing.Point(213, 88);
            this.chkComment.Name = "chkComment";
            this.chkComment.Size = new System.Drawing.Size(66, 16);
            this.chkComment.TabIndex = 6;
            this.chkComment.Text = "Comment";
            this.chkComment.UseVisualStyleBackColor = true;
            // 
            // chkHyperLink
            // 
            this.chkHyperLink.AutoSize = true;
            this.chkHyperLink.Location = new System.Drawing.Point(24, 88);
            this.chkHyperLink.Name = "chkHyperLink";
            this.chkHyperLink.Size = new System.Drawing.Size(78, 16);
            this.chkHyperLink.TabIndex = 5;
            this.chkHyperLink.Text = "HyperLink";
            this.chkHyperLink.UseVisualStyleBackColor = true;
            // 
            // chkDateTime
            // 
            this.chkDateTime.AutoSize = true;
            this.chkDateTime.Location = new System.Drawing.Point(213, 66);
            this.chkDateTime.Name = "chkDateTime";
            this.chkDateTime.Size = new System.Drawing.Size(72, 16);
            this.chkDateTime.TabIndex = 5;
            this.chkDateTime.Text = "DateTime";
            this.chkDateTime.UseVisualStyleBackColor = true;
            // 
            // chkDescription
            // 
            this.chkDescription.AutoSize = true;
            this.chkDescription.Location = new System.Drawing.Point(25, 66);
            this.chkDescription.Name = "chkDescription";
            this.chkDescription.Size = new System.Drawing.Size(90, 16);
            this.chkDescription.TabIndex = 5;
            this.chkDescription.Text = "Description";
            this.chkDescription.UseVisualStyleBackColor = true;
            // 
            // chkContent
            // 
            this.chkContent.AutoSize = true;
            this.chkContent.Location = new System.Drawing.Point(213, 44);
            this.chkContent.Name = "chkContent";
            this.chkContent.Size = new System.Drawing.Size(66, 16);
            this.chkContent.TabIndex = 5;
            this.chkContent.Text = "Content";
            this.chkContent.UseVisualStyleBackColor = true;
            // 
            // chkActivityName
            // 
            this.chkActivityName.AutoSize = true;
            this.chkActivityName.Location = new System.Drawing.Point(25, 44);
            this.chkActivityName.Name = "chkActivityName";
            this.chkActivityName.Size = new System.Drawing.Size(96, 16);
            this.chkActivityName.TabIndex = 5;
            this.chkActivityName.Text = "ActivityName";
            this.chkActivityName.UseVisualStyleBackColor = true;
            // 
            // chkFlowName
            // 
            this.chkFlowName.AutoSize = true;
            this.chkFlowName.Location = new System.Drawing.Point(213, 20);
            this.chkFlowName.Name = "chkFlowName";
            this.chkFlowName.Size = new System.Drawing.Size(72, 16);
            this.chkFlowName.TabIndex = 5;
            this.chkFlowName.Text = "FlowName";
            this.chkFlowName.UseVisualStyleBackColor = true;
            // 
            // chkSender
            // 
            this.chkSender.AutoSize = true;
            this.chkSender.Location = new System.Drawing.Point(25, 20);
            this.chkSender.Name = "chkSender";
            this.chkSender.Size = new System.Drawing.Size(60, 16);
            this.chkSender.TabIndex = 5;
            this.chkSender.Text = "Sender";
            this.chkSender.UseVisualStyleBackColor = true;
            // 
            // txtSMTP
            // 
            this.txtSMTP.Location = new System.Drawing.Point(129, 86);
            this.txtSMTP.Name = "txtSMTP";
            this.txtSMTP.Size = new System.Drawing.Size(236, 21);
            this.txtSMTP.TabIndex = 20;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(129, 57);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(236, 21);
            this.txtPassword.TabIndex = 19;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(129, 26);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(236, 21);
            this.txtEmail.TabIndex = 18;
            // 
            // lblSMTP
            // 
            this.lblSMTP.AutoSize = true;
            this.lblSMTP.Location = new System.Drawing.Point(15, 89);
            this.lblSMTP.Name = "lblSMTP";
            this.lblSMTP.Size = new System.Drawing.Size(77, 12);
            this.lblSMTP.TabIndex = 15;
            this.lblSMTP.Text = "SMTP Server:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(15, 60);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(59, 12);
            this.lblPassword.TabIndex = 16;
            this.lblPassword.Text = "Password:";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(15, 29);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(41, 12);
            this.lblEmail.TabIndex = 17;
            this.lblEmail.Text = "Email:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBoxP12Password);
            this.tabPage2.Controls.Add(this.textBoxP12FileName);
            this.tabPage2.Controls.Add(this.textBoxApiKey);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.textBoxPushService);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.checkBox2);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(389, 482);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Push";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxPushService
            // 
            this.textBoxPushService.Location = new System.Drawing.Point(108, 41);
            this.textBoxPushService.Name = "textBoxPushService";
            this.textBoxPushService.Size = new System.Drawing.Size(257, 21);
            this.textBoxPushService.TabIndex = 40;
            this.textBoxPushService.Tag = "PushService";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 39;
            this.label2.Text = "Push Service:";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(21, 15);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(96, 16);
            this.checkBox2.TabIndex = 38;
            this.checkBox2.Tag = "Active";
            this.checkBox2.Text = "Push Activie";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox3);
            this.groupBox3.Controls.Add(this.checkBox4);
            this.groupBox3.Controls.Add(this.checkBox5);
            this.groupBox3.Controls.Add(this.checkBox6);
            this.groupBox3.Controls.Add(this.checkBox7);
            this.groupBox3.Location = new System.Drawing.Point(21, 200);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(348, 86);
            this.groupBox3.TabIndex = 37;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Push Subject";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(25, 66);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(66, 16);
            this.checkBox3.TabIndex = 7;
            this.checkBox3.Tag = "SubjectContent";
            this.checkBox3.Text = "Content";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(213, 44);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(90, 16);
            this.checkBox4.TabIndex = 8;
            this.checkBox4.Tag = "SubjectDescription";
            this.checkBox4.Text = "Description";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(26, 44);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(96, 16);
            this.checkBox5.TabIndex = 9;
            this.checkBox5.Tag = "SubjectActivityName";
            this.checkBox5.Text = "ActivityName";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(213, 20);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(72, 16);
            this.checkBox6.TabIndex = 6;
            this.checkBox6.Tag = "SubjectFlowName";
            this.checkBox6.Text = "FlowName";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(26, 20);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(60, 16);
            this.checkBox7.TabIndex = 7;
            this.checkBox7.Tag = "SubjectSender";
            this.checkBox7.Text = "Sender";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBox14);
            this.groupBox4.Controls.Add(this.checkBox15);
            this.groupBox4.Controls.Add(this.checkBox16);
            this.groupBox4.Controls.Add(this.checkBox17);
            this.groupBox4.Controls.Add(this.checkBox18);
            this.groupBox4.Controls.Add(this.checkBox19);
            this.groupBox4.Location = new System.Drawing.Point(21, 293);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(348, 152);
            this.groupBox4.TabIndex = 36;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Push Body";
            // 
            // checkBox14
            // 
            this.checkBox14.AutoSize = true;
            this.checkBox14.Location = new System.Drawing.Point(213, 66);
            this.checkBox14.Name = "checkBox14";
            this.checkBox14.Size = new System.Drawing.Size(72, 16);
            this.checkBox14.TabIndex = 5;
            this.checkBox14.Tag = "BodyDatetime";
            this.checkBox14.Text = "DateTime";
            this.checkBox14.UseVisualStyleBackColor = true;
            // 
            // checkBox15
            // 
            this.checkBox15.AutoSize = true;
            this.checkBox15.Location = new System.Drawing.Point(25, 66);
            this.checkBox15.Name = "checkBox15";
            this.checkBox15.Size = new System.Drawing.Size(90, 16);
            this.checkBox15.TabIndex = 5;
            this.checkBox15.Tag = "BodyDescription";
            this.checkBox15.Text = "Description";
            this.checkBox15.UseVisualStyleBackColor = true;
            // 
            // checkBox16
            // 
            this.checkBox16.AutoSize = true;
            this.checkBox16.Location = new System.Drawing.Point(213, 44);
            this.checkBox16.Name = "checkBox16";
            this.checkBox16.Size = new System.Drawing.Size(66, 16);
            this.checkBox16.TabIndex = 5;
            this.checkBox16.Tag = "BodyContent";
            this.checkBox16.Text = "Content";
            this.checkBox16.UseVisualStyleBackColor = true;
            // 
            // checkBox17
            // 
            this.checkBox17.AutoSize = true;
            this.checkBox17.Location = new System.Drawing.Point(25, 44);
            this.checkBox17.Name = "checkBox17";
            this.checkBox17.Size = new System.Drawing.Size(96, 16);
            this.checkBox17.TabIndex = 5;
            this.checkBox17.Tag = "BodyActivityName";
            this.checkBox17.Text = "ActivityName";
            this.checkBox17.UseVisualStyleBackColor = true;
            // 
            // checkBox18
            // 
            this.checkBox18.AutoSize = true;
            this.checkBox18.Location = new System.Drawing.Point(213, 20);
            this.checkBox18.Name = "checkBox18";
            this.checkBox18.Size = new System.Drawing.Size(72, 16);
            this.checkBox18.TabIndex = 5;
            this.checkBox18.Tag = "BodyFlowName";
            this.checkBox18.Text = "FlowName";
            this.checkBox18.UseVisualStyleBackColor = true;
            // 
            // checkBox19
            // 
            this.checkBox19.AutoSize = true;
            this.checkBox19.Location = new System.Drawing.Point(25, 20);
            this.checkBox19.Name = "checkBox19";
            this.checkBox19.Size = new System.Drawing.Size(60, 16);
            this.checkBox19.TabIndex = 5;
            this.checkBox19.Tag = "BodySender";
            this.checkBox19.Text = "Sender";
            this.checkBox19.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 41;
            this.label3.Text = "API Key:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 42;
            this.label4.Text = "P12 File Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 43;
            this.label5.Text = "File Password:";
            // 
            // textBoxApiKey
            // 
            this.textBoxApiKey.Location = new System.Drawing.Point(108, 81);
            this.textBoxApiKey.Name = "textBoxApiKey";
            this.textBoxApiKey.Size = new System.Drawing.Size(257, 21);
            this.textBoxApiKey.TabIndex = 44;
            this.textBoxApiKey.Tag = "APIKey";
            // 
            // textBoxP12FileName
            // 
            this.textBoxP12FileName.Location = new System.Drawing.Point(108, 114);
            this.textBoxP12FileName.Name = "textBoxP12FileName";
            this.textBoxP12FileName.Size = new System.Drawing.Size(257, 21);
            this.textBoxP12FileName.TabIndex = 45;
            this.textBoxP12FileName.Tag = "P12FileName";
            // 
            // textBoxP12Password
            // 
            this.textBoxP12Password.Location = new System.Drawing.Point(108, 147);
            this.textBoxP12Password.Name = "textBoxP12Password";
            this.textBoxP12Password.PasswordChar = '*';
            this.textBoxP12Password.Size = new System.Drawing.Size(257, 21);
            this.textBoxP12Password.TabIndex = 46;
            this.textBoxP12Password.Tag = "FilePassword";
            // 
            // frmWorkflowConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 549);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWorkflowConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmWorkflowConfig";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox tbSMS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbEnableSSL;
        private System.Windows.Forms.Label lblEnbleSSL;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkContent2;
        private System.Windows.Forms.CheckBox chkDescription2;
        private System.Windows.Forms.CheckBox chkActivityName2;
        private System.Windows.Forms.CheckBox chkFlowName2;
        private System.Windows.Forms.CheckBox chkSender2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxActivityDescription;
        private System.Windows.Forms.CheckBox checkBoxRejectButton;
        private System.Windows.Forms.CheckBox checkBoxReturnButton;
        private System.Windows.Forms.CheckBox checkBoxApproveButton;
        private System.Windows.Forms.CheckBox chkComment;
        private System.Windows.Forms.CheckBox chkHyperLink;
        private System.Windows.Forms.CheckBox chkDateTime;
        private System.Windows.Forms.CheckBox chkDescription;
        private System.Windows.Forms.CheckBox chkContent;
        private System.Windows.Forms.CheckBox chkActivityName;
        private System.Windows.Forms.CheckBox chkFlowName;
        private System.Windows.Forms.CheckBox chkSender;
        private System.Windows.Forms.TextBox txtSMTP;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblSMTP;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBox14;
        private System.Windows.Forms.CheckBox checkBox15;
        private System.Windows.Forms.CheckBox checkBox16;
        private System.Windows.Forms.CheckBox checkBox17;
        private System.Windows.Forms.CheckBox checkBox18;
        private System.Windows.Forms.CheckBox checkBox19;
        private System.Windows.Forms.TextBox textBoxPushService;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxP12Password;
        private System.Windows.Forms.TextBox textBoxP12FileName;
        private System.Windows.Forms.TextBox textBoxApiKey;
    }
}