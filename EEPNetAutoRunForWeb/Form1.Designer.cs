namespace EEPNetAutoRunForWeb
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.btnChooseXML = new System.Windows.Forms.Button();
            this.txtChooseXML = new System.Windows.Forms.TextBox();
            this.lblChoose = new System.Windows.Forms.Label();
            this.btnLogToFile = new System.Windows.Forms.Button();
            this.itbTM = new System.Windows.Forms.TextBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.itbLTF = new System.Windows.Forms.TextBox();
            this.itbInterval = new System.Windows.Forms.TextBox();
            this.itbTU = new System.Windows.Forms.TextBox();
            this.lblInterval = new System.Windows.Forms.Label();
            this.lblLog = new System.Windows.Forms.Label();
            this.lblUsers = new System.Windows.Forms.Label();
            this.lblModules = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.itbUrl = new System.Windows.Forms.TextBox();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.cbTestMode = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnChooseXML
            // 
            this.btnChooseXML.Location = new System.Drawing.Point(224, 20);
            this.btnChooseXML.Name = "btnChooseXML";
            this.btnChooseXML.Size = new System.Drawing.Size(22, 18);
            this.btnChooseXML.TabIndex = 31;
            this.btnChooseXML.Text = "...";
            this.btnChooseXML.UseVisualStyleBackColor = true;
            this.btnChooseXML.Click += new System.EventHandler(this.btnChooseXML_Click);
            // 
            // txtChooseXML
            // 
            this.txtChooseXML.Location = new System.Drawing.Point(138, 19);
            this.txtChooseXML.Name = "txtChooseXML";
            this.txtChooseXML.Size = new System.Drawing.Size(108, 21);
            this.txtChooseXML.TabIndex = 30;
            // 
            // lblChoose
            // 
            this.lblChoose.AutoSize = true;
            this.lblChoose.Location = new System.Drawing.Point(22, 22);
            this.lblChoose.Name = "lblChoose";
            this.lblChoose.Size = new System.Drawing.Size(71, 12);
            this.lblChoose.TabIndex = 29;
            this.lblChoose.Text = "Choose XML:";
            // 
            // btnLogToFile
            // 
            this.btnLogToFile.Location = new System.Drawing.Point(475, 61);
            this.btnLogToFile.Name = "btnLogToFile";
            this.btnLogToFile.Size = new System.Drawing.Size(22, 18);
            this.btnLogToFile.TabIndex = 28;
            this.btnLogToFile.Text = "...";
            this.btnLogToFile.UseVisualStyleBackColor = true;
            this.btnLogToFile.Click += new System.EventHandler(this.btnLogToFile_Click);
            // 
            // itbTM
            // 
            this.itbTM.Location = new System.Drawing.Point(138, 95);
            this.itbTM.Name = "itbTM";
            this.itbTM.Size = new System.Drawing.Size(108, 21);
            this.itbTM.TabIndex = 25;
            // 
            // buttonGo
            // 
            this.buttonGo.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonGo.Location = new System.Drawing.Point(218, 160);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 20);
            this.buttonGo.TabIndex = 23;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // itbLTF
            // 
            this.itbLTF.Location = new System.Drawing.Point(389, 60);
            this.itbLTF.Name = "itbLTF";
            this.itbLTF.Size = new System.Drawing.Size(108, 21);
            this.itbLTF.TabIndex = 27;
            // 
            // itbInterval
            // 
            this.itbInterval.Location = new System.Drawing.Point(389, 19);
            this.itbInterval.Name = "itbInterval";
            this.itbInterval.Size = new System.Drawing.Size(108, 21);
            this.itbInterval.TabIndex = 26;
            this.itbInterval.Text = "2000";
            // 
            // itbTU
            // 
            this.itbTU.Location = new System.Drawing.Point(138, 58);
            this.itbTU.Name = "itbTU";
            this.itbTU.Size = new System.Drawing.Size(108, 21);
            this.itbTU.TabIndex = 24;
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(282, 22);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(59, 12);
            this.lblInterval.TabIndex = 21;
            this.lblInterval.Text = "Interval:";
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(282, 63);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(77, 12);
            this.lblLog.TabIndex = 22;
            this.lblLog.Text = "Log To File:";
            // 
            // lblUsers
            // 
            this.lblUsers.AutoSize = true;
            this.lblUsers.Location = new System.Drawing.Point(22, 60);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(101, 12);
            this.lblUsers.TabIndex = 19;
            this.lblUsers.Text = "Test User Count:";
            // 
            // lblModules
            // 
            this.lblModules.AutoSize = true;
            this.lblModules.Location = new System.Drawing.Point(22, 98);
            this.lblModules.Name = "lblModules";
            this.lblModules.Size = new System.Drawing.Size(113, 12);
            this.lblModules.TabIndex = 20;
            this.lblModules.Text = "Test Module Count:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 32;
            this.label1.Text = "Web Site Url:";
            // 
            // itbUrl
            // 
            this.itbUrl.Location = new System.Drawing.Point(138, 133);
            this.itbUrl.Name = "itbUrl";
            this.itbUrl.Size = new System.Drawing.Size(368, 21);
            this.itbUrl.TabIndex = 33;
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "*.xml";
            this.openFileDialog2.Filter = "*.xml|*.xml|All|*.*";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.clearAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 48);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "*.txt";
            this.openFileDialog1.Filter = "*.txt|*.txt|All|*.*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(282, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 34;
            this.label2.Text = "Test Mode:";
            // 
            // cbTestMode
            // 
            this.cbTestMode.FormattingEnabled = true;
            this.cbTestMode.Items.AddRange(new object[] {
            "Auto Add",
            "Auto Select"});
            this.cbTestMode.Location = new System.Drawing.Point(389, 95);
            this.cbTestMode.Name = "cbTestMode";
            this.cbTestMode.Size = new System.Drawing.Size(108, 20);
            this.cbTestMode.TabIndex = 35;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(518, 192);
            this.Controls.Add(this.cbTestMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.itbUrl);
            this.Controls.Add(this.btnChooseXML);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtChooseXML);
            this.Controls.Add(this.lblChoose);
            this.Controls.Add(this.btnLogToFile);
            this.Controls.Add(this.itbLTF);
            this.Controls.Add(this.itbTM);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.itbInterval);
            this.Controls.Add(this.lblInterval);
            this.Controls.Add(this.itbTU);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.lblModules);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnChooseXML;
        private System.Windows.Forms.TextBox txtChooseXML;
        private System.Windows.Forms.Label lblChoose;
        private System.Windows.Forms.Button btnLogToFile;
        private System.Windows.Forms.TextBox itbTM;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.TextBox itbLTF;
        private System.Windows.Forms.TextBox itbInterval;
        private System.Windows.Forms.TextBox itbTU;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.Label lblModules;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox itbUrl;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbTestMode;
    }
}

