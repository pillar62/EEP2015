namespace OfficeTools.RunTime
{
    partial class frmEmail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmail));
            this.labelMailTo = new System.Windows.Forms.Label();
            this.labelMailFrom = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelServer = new System.Windows.Forms.Label();
            this.tbMailTo = new System.Windows.Forms.TextBox();
            this.tbMailFrom = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbSmtpServer = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.cbDefault = new System.Windows.Forms.CheckBox();
            this.lbAttachment = new System.Windows.Forms.Label();
            this.pbAttachment = new System.Windows.Forms.PictureBox();
            this.labelAttachment = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lbMessage = new System.Windows.Forms.ListBox();
            this.cbDetail = new System.Windows.Forms.CheckBox();
            this.pnBtn = new System.Windows.Forms.Panel();
            this.pnMessage = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.lbProgress = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbAttachment)).BeginInit();
            this.pnBtn.SuspendLayout();
            this.pnMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelMailTo
            // 
            this.labelMailTo.AutoSize = true;
            this.labelMailTo.Location = new System.Drawing.Point(6, 21);
            this.labelMailTo.Name = "labelMailTo";
            this.labelMailTo.Size = new System.Drawing.Size(53, 12);
            this.labelMailTo.TabIndex = 0;
            this.labelMailTo.Text = "Mail To:";
            // 
            // labelMailFrom
            // 
            this.labelMailFrom.AutoSize = true;
            this.labelMailFrom.Location = new System.Drawing.Point(6, 48);
            this.labelMailFrom.Name = "labelMailFrom";
            this.labelMailFrom.Size = new System.Drawing.Size(65, 12);
            this.labelMailFrom.TabIndex = 1;
            this.labelMailFrom.Text = "Mail From:";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(6, 75);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(59, 12);
            this.labelPassword.TabIndex = 2;
            this.labelPassword.Text = "Password:";
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.Location = new System.Drawing.Point(6, 103);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(47, 12);
            this.labelServer.TabIndex = 3;
            this.labelServer.Text = "Server:";
            // 
            // tbMailTo
            // 
            this.tbMailTo.Location = new System.Drawing.Point(80, 18);
            this.tbMailTo.Name = "tbMailTo";
            this.tbMailTo.Size = new System.Drawing.Size(163, 21);
            this.tbMailTo.TabIndex = 0;
            // 
            // tbMailFrom
            // 
            this.tbMailFrom.Location = new System.Drawing.Point(80, 45);
            this.tbMailFrom.Name = "tbMailFrom";
            this.tbMailFrom.Size = new System.Drawing.Size(163, 21);
            this.tbMailFrom.TabIndex = 1;
            this.tbMailFrom.Leave += new System.EventHandler(this.tbMailFrom_Leave);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(80, 72);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(100, 21);
            this.tbPassword.TabIndex = 2;
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // tbSmtpServer
            // 
            this.tbSmtpServer.Location = new System.Drawing.Point(80, 99);
            this.tbSmtpServer.Name = "tbSmtpServer";
            this.tbSmtpServer.ReadOnly = true;
            this.tbSmtpServer.Size = new System.Drawing.Size(100, 21);
            this.tbSmtpServer.TabIndex = 3;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(112, 6);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(7, 6);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 9;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cbDefault
            // 
            this.cbDefault.AutoSize = true;
            this.cbDefault.Checked = true;
            this.cbDefault.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDefault.Location = new System.Drawing.Point(187, 103);
            this.cbDefault.Name = "cbDefault";
            this.cbDefault.Size = new System.Drawing.Size(66, 16);
            this.cbDefault.TabIndex = 4;
            this.cbDefault.Text = "Default";
            this.cbDefault.UseVisualStyleBackColor = true;
            this.cbDefault.CheckedChanged += new System.EventHandler(this.cbDefault_CheckedChanged);
            // 
            // lbAttachment
            // 
            this.lbAttachment.AutoSize = true;
            this.lbAttachment.Location = new System.Drawing.Point(103, 130);
            this.lbAttachment.Name = "lbAttachment";
            this.lbAttachment.Size = new System.Drawing.Size(0, 12);
            this.lbAttachment.TabIndex = 12;
            // 
            // pbAttachment
            // 
            this.pbAttachment.Location = new System.Drawing.Point(80, 126);
            this.pbAttachment.Name = "pbAttachment";
            this.pbAttachment.Size = new System.Drawing.Size(16, 16);
            this.pbAttachment.TabIndex = 11;
            this.pbAttachment.TabStop = false;
            // 
            // labelAttachment
            // 
            this.labelAttachment.AutoSize = true;
            this.labelAttachment.Location = new System.Drawing.Point(6, 130);
            this.labelAttachment.Name = "labelAttachment";
            this.labelAttachment.Size = new System.Drawing.Size(71, 12);
            this.labelAttachment.TabIndex = 13;
            this.labelAttachment.Text = "Attachment:";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(69, 8);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(121, 14);
            this.progressBar.TabIndex = 14;
            // 
            // lbMessage
            // 
            this.lbMessage.FormattingEnabled = true;
            this.lbMessage.HorizontalScrollbar = true;
            this.lbMessage.ItemHeight = 12;
            this.lbMessage.Location = new System.Drawing.Point(3, 34);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(241, 100);
            this.lbMessage.TabIndex = 15;
            this.lbMessage.Visible = false;
            // 
            // cbDetail
            // 
            this.cbDetail.AutoSize = true;
            this.cbDetail.Location = new System.Drawing.Point(3, 8);
            this.cbDetail.Name = "cbDetail";
            this.cbDetail.Size = new System.Drawing.Size(60, 16);
            this.cbDetail.TabIndex = 16;
            this.cbDetail.Text = "Detail";
            this.cbDetail.UseVisualStyleBackColor = true;
            this.cbDetail.CheckedChanged += new System.EventHandler(this.cbDetail_CheckedChanged);
            // 
            // pnBtn
            // 
            this.pnBtn.Controls.Add(this.btnSend);
            this.pnBtn.Controls.Add(this.btnReset);
            this.pnBtn.Location = new System.Drawing.Point(25, 148);
            this.pnBtn.Name = "pnBtn";
            this.pnBtn.Size = new System.Drawing.Size(200, 34);
            this.pnBtn.TabIndex = 17;
            // 
            // pnMessage
            // 
            this.pnMessage.Controls.Add(this.btnOK);
            this.pnMessage.Controls.Add(this.lbProgress);
            this.pnMessage.Controls.Add(this.lbMessage);
            this.pnMessage.Controls.Add(this.progressBar);
            this.pnMessage.Controls.Add(this.cbDetail);
            this.pnMessage.Location = new System.Drawing.Point(3, 151);
            this.pnMessage.Name = "pnMessage";
            this.pnMessage.Size = new System.Drawing.Size(250, 140);
            this.pnMessage.TabIndex = 5;
            this.pnMessage.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(195, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(51, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lbProgress
            // 
            this.lbProgress.AutoSize = true;
            this.lbProgress.Location = new System.Drawing.Point(69, 12);
            this.lbProgress.Name = "lbProgress";
            this.lbProgress.Size = new System.Drawing.Size(0, 12);
            this.lbProgress.TabIndex = 17;
            // 
            // frmEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 179);
            this.Controls.Add(this.pnMessage);
            this.Controls.Add(this.pnBtn);
            this.Controls.Add(this.labelAttachment);
            this.Controls.Add(this.lbAttachment);
            this.Controls.Add(this.pbAttachment);
            this.Controls.Add(this.cbDefault);
            this.Controls.Add(this.tbSmtpServer);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbMailFrom);
            this.Controls.Add(this.tbMailTo);
            this.Controls.Add(this.labelServer);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelMailFrom);
            this.Controls.Add(this.labelMailTo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEmail";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "E-Mail";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEmail_FormClosing);
            this.Load += new System.EventHandler(this.frmEmail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbAttachment)).EndInit();
            this.pnBtn.ResumeLayout(false);
            this.pnMessage.ResumeLayout(false);
            this.pnMessage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMailTo;
        private System.Windows.Forms.Label labelMailFrom;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.TextBox tbMailTo;
        private System.Windows.Forms.TextBox tbMailFrom;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbSmtpServer;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox cbDefault;
        private System.Windows.Forms.PictureBox pbAttachment;
        private System.Windows.Forms.Label lbAttachment;
        private System.Windows.Forms.Label labelAttachment;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ListBox lbMessage;
        private System.Windows.Forms.CheckBox cbDetail;
        private System.Windows.Forms.Panel pnBtn;
        private System.Windows.Forms.Panel pnMessage;
        private System.Windows.Forms.Label lbProgress;
        private System.Windows.Forms.Button btnOK;
    }
}