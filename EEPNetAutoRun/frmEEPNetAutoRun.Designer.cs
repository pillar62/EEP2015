namespace EEPNetAutoRun
{
    partial class frmEEPNetAutoRun
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblUsers = new System.Windows.Forms.Label();
            this.lblModules = new System.Windows.Forms.Label();
            this.lblInterval = new System.Windows.Forms.Label();
            this.lblLog = new System.Windows.Forms.Label();
            this.buttonGo = new System.Windows.Forms.Button();
            this.itbTU = new System.Windows.Forms.TextBox();
            this.itbTM = new System.Windows.Forms.TextBox();
            this.itbInterval = new System.Windows.Forms.TextBox();
            this.itbLTF = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnLogToFile = new System.Windows.Forms.Button();
            this.lvAutoRun = new System.Windows.Forms.ListView();
            this.UserID = new System.Windows.Forms.ColumnHeader();
            this.Module = new System.Windows.Forms.ColumnHeader();
            this.StartTime = new System.Windows.Forms.ColumnHeader();
            this.Number = new System.Windows.Forms.ColumnHeader();
            this.Times = new System.Windows.Forms.ColumnHeader();
            this.Status = new System.Windows.Forms.ColumnHeader();
            this.CompleteTime = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lblChoose = new System.Windows.Forms.Label();
            this.txtChooseXML = new System.Windows.Forms.TextBox();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.btnChooseXML = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTestMode = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUsers
            // 
            this.lblUsers.AutoSize = true;
            this.lblUsers.Location = new System.Drawing.Point(17, 296);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(55, 12);
            this.lblUsers.TabIndex = 0;
            this.lblUsers.Text = "Test Users:";
            // 
            // lblModules
            // 
            this.lblModules.AutoSize = true;
            this.lblModules.Location = new System.Drawing.Point(17, 334);
            this.lblModules.Name = "lblModules";
            this.lblModules.Size = new System.Drawing.Size(70, 12);
            this.lblModules.TabIndex = 2;
            this.lblModules.Text = "Test Modules:";
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(277, 258);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(44, 12);
            this.lblInterval.TabIndex = 3;
            this.lblInterval.Text = "Interval:";
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(277, 299);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(63, 12);
            this.lblLog.TabIndex = 4;
            this.lblLog.Text = "Log To File:";
            // 
            // buttonGo
            // 
            this.buttonGo.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonGo.Location = new System.Drawing.Point(216, 380);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 23);
            this.buttonGo.TabIndex = 8;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // itbTU
            // 
            this.itbTU.Location = new System.Drawing.Point(114, 293);
            this.itbTU.Name = "itbTU";
            this.itbTU.Size = new System.Drawing.Size(108, 22);
            this.itbTU.TabIndex = 9;
            // 
            // itbTM
            // 
            this.itbTM.Location = new System.Drawing.Point(114, 331);
            this.itbTM.Name = "itbTM";
            this.itbTM.Size = new System.Drawing.Size(108, 22);
            this.itbTM.TabIndex = 10;
            // 
            // itbInterval
            // 
            this.itbInterval.Location = new System.Drawing.Point(384, 255);
            this.itbInterval.Name = "itbInterval";
            this.itbInterval.Size = new System.Drawing.Size(108, 22);
            this.itbInterval.TabIndex = 11;
            this.itbInterval.Text = "300";
            // 
            // itbLTF
            // 
            this.itbLTF.Location = new System.Drawing.Point(384, 296);
            this.itbLTF.Name = "itbLTF";
            this.itbLTF.Size = new System.Drawing.Size(108, 22);
            this.itbLTF.TabIndex = 12;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "*.txt";
            this.openFileDialog1.Filter = "*.txt|*.txt|All|*.*";
            // 
            // btnLogToFile
            // 
            this.btnLogToFile.Location = new System.Drawing.Point(470, 296);
            this.btnLogToFile.Name = "btnLogToFile";
            this.btnLogToFile.Size = new System.Drawing.Size(22, 21);
            this.btnLogToFile.TabIndex = 14;
            this.btnLogToFile.Text = "...";
            this.btnLogToFile.UseVisualStyleBackColor = true;
            this.btnLogToFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // lvAutoRun
            // 
            this.lvAutoRun.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.UserID,
            this.Module,
            this.StartTime,
            this.Number,
            this.Times,
            this.Status,
            this.CompleteTime});
            this.lvAutoRun.ContextMenuStrip = this.contextMenuStrip1;
            this.lvAutoRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvAutoRun.FullRowSelect = true;
            this.lvAutoRun.GridLines = true;
            this.lvAutoRun.HideSelection = false;
            this.lvAutoRun.Location = new System.Drawing.Point(8, 12);
            this.lvAutoRun.Name = "lvAutoRun";
            this.lvAutoRun.Size = new System.Drawing.Size(514, 222);
            this.lvAutoRun.TabIndex = 15;
            this.lvAutoRun.UseCompatibleStateImageBehavior = false;
            this.lvAutoRun.View = System.Windows.Forms.View.Details;
            // 
            // UserID
            // 
            this.UserID.Text = "User ID";
            // 
            // Module
            // 
            this.Module.Text = "Module";
            // 
            // StartTime
            // 
            this.StartTime.Text = "Start Time";
            this.StartTime.Width = 100;
            // 
            // Number
            // 
            this.Number.Text = "Number";
            // 
            // Times
            // 
            this.Times.Text = "Times";
            // 
            // Status
            // 
            this.Status.Text = "Status";
            this.Status.Width = 70;
            // 
            // CompleteTime
            // 
            this.CompleteTime.Text = "CompleteTime";
            this.CompleteTime.Width = 100;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.clearAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 48);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.contextMenuStrip1_Opened);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // timer
            // 
            this.timer.Interval = 3000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lblChoose
            // 
            this.lblChoose.AutoSize = true;
            this.lblChoose.Location = new System.Drawing.Point(17, 258);
            this.lblChoose.Name = "lblChoose";
            this.lblChoose.Size = new System.Drawing.Size(71, 12);
            this.lblChoose.TabIndex = 16;
            this.lblChoose.Text = "Choose XML:";
            // 
            // txtChooseXML
            // 
            this.txtChooseXML.Location = new System.Drawing.Point(114, 255);
            this.txtChooseXML.Name = "txtChooseXML";
            this.txtChooseXML.Size = new System.Drawing.Size(108, 22);
            this.txtChooseXML.TabIndex = 17;
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "*.xml";
            this.openFileDialog2.Filter = "*.xml|*.xml|All|*.*";
            // 
            // btnChooseXML
            // 
            this.btnChooseXML.Location = new System.Drawing.Point(200, 255);
            this.btnChooseXML.Name = "btnChooseXML";
            this.btnChooseXML.Size = new System.Drawing.Size(22, 21);
            this.btnChooseXML.TabIndex = 18;
            this.btnChooseXML.Text = "...";
            this.btnChooseXML.UseVisualStyleBackColor = true;
            this.btnChooseXML.Click += new System.EventHandler(this.btnChooseXML_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(277, 334);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "Test Mode:";
            // 
            // cbTestMode
            // 
            this.cbTestMode.FormattingEnabled = true;
            this.cbTestMode.Items.AddRange(new object[] {
            "Auto Add",
            "Auto Select",
            "Auto Exec",
            "Auto Run"});
            this.cbTestMode.Location = new System.Drawing.Point(384, 331);
            this.cbTestMode.Name = "cbTestMode";
            this.cbTestMode.Size = new System.Drawing.Size(108, 20);
            this.cbTestMode.TabIndex = 20;
            // 
            // frmEEPNetAutoRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 427);
            this.Controls.Add(this.cbTestMode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnChooseXML);
            this.Controls.Add(this.txtChooseXML);
            this.Controls.Add(this.lblChoose);
            this.Controls.Add(this.lvAutoRun);
            this.Controls.Add(this.btnLogToFile);
            this.Controls.Add(this.itbTM);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.itbLTF);
            this.Controls.Add(this.itbInterval);
            this.Controls.Add(this.itbTU);
            this.Controls.Add(this.lblInterval);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.lblModules);
            this.MaximizeBox = false;
            this.Name = "frmEEPNetAutoRun";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EEPAutoRun";
            this.Load += new System.EventHandler(this.frmEEPNetAutoRun_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.Label lblModules;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.TextBox itbTU;
        private System.Windows.Forms.TextBox itbTM;
        private System.Windows.Forms.TextBox itbInterval;
        private System.Windows.Forms.TextBox itbLTF;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnLogToFile;
        private System.Windows.Forms.ListView lvAutoRun;
        private System.Windows.Forms.ColumnHeader UserID;
        private System.Windows.Forms.ColumnHeader Module;
        private System.Windows.Forms.ColumnHeader StartTime;
        private System.Windows.Forms.ColumnHeader Times;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.ColumnHeader Number;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ColumnHeader CompleteTime;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.Label lblChoose;
        private System.Windows.Forms.TextBox txtChooseXML;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button btnChooseXML;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTestMode;
    }
}

