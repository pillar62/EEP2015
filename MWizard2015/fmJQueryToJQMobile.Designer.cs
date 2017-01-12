namespace MWizard2015
{
    partial class fmJQueryToJQMobile
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tpFormSetting = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSelectJQPage = new System.Windows.Forms.TextBox();
            this.btnSelectJQPage = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbFormName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbFormTitle = new System.Windows.Forms.TextBox();
            this.tpOutputSetting = new System.Windows.Forms.TabPage();
            this.rbAddToExistFolder = new System.Windows.Forms.RadioButton();
            this.cbAddToExistFolder = new System.Windows.Forms.ComboBox();
            this.rbAddToNewFolder = new System.Windows.Forms.RadioButton();
            this.tbAddToNewFolder = new System.Windows.Forms.TextBox();
            this.rbAddToRootFolder = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cbWebSite = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSolutionName = new System.Windows.Forms.Button();
            this.tbCurrentSolution = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbSolutionName = new System.Windows.Forms.TextBox();
            this.rbAddToExistSolution = new System.Windows.Forms.RadioButton();
            this.rbCurrentSolution = new System.Windows.Forms.RadioButton();
            this.tpConnection = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.tbConnectionString = new System.Windows.Forms.TextBox();
            this.btnConnectionString = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.cbDatabaseType = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cbChooseLanguage = new System.Windows.Forms.ComboBox();
            this.cbEEPAlias = new System.Windows.Forms.ComboBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpFormSetting.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tpOutputSetting.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tpConnection.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(573, 387);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(159, 387);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 12;
            this.btnNext.Text = "Next";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.SelectedPath = "folderBrowserDialog1";
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(57, 387);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnPrevious.TabIndex = 14;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(481, 387);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 15;
            this.btnDone.Text = "Done";
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.sln";
            this.openFileDialog.Filter = "Solution Files (*.sln)|*.sln";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tpFormSetting
            // 
            this.tpFormSetting.Controls.Add(this.tbFormTitle);
            this.tpFormSetting.Controls.Add(this.tbFormName);
            this.tpFormSetting.Controls.Add(this.label10);
            this.tpFormSetting.Controls.Add(this.label4);
            this.tpFormSetting.Controls.Add(this.groupBox1);
            this.tpFormSetting.Location = new System.Drawing.Point(4, 22);
            this.tpFormSetting.Name = "tpFormSetting";
            this.tpFormSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tpFormSetting.Size = new System.Drawing.Size(725, 347);
            this.tpFormSetting.TabIndex = 1;
            this.tpFormSetting.Text = "Form Setting";
            this.tpFormSetting.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelectJQPage);
            this.groupBox1.Controls.Add(this.txtSelectJQPage);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(61, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(538, 125);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inherit From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 8F);
            this.label3.Location = new System.Drawing.Point(16, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "JQuery Web Page";
            // 
            // txtSelectJQPage
            // 
            this.txtSelectJQPage.Location = new System.Drawing.Point(19, 46);
            this.txtSelectJQPage.Name = "txtSelectJQPage";
            this.txtSelectJQPage.Size = new System.Drawing.Size(394, 21);
            this.txtSelectJQPage.TabIndex = 6;
            // 
            // btnSelectJQPage
            // 
            this.btnSelectJQPage.Location = new System.Drawing.Point(419, 44);
            this.btnSelectJQPage.Name = "btnSelectJQPage";
            this.btnSelectJQPage.Size = new System.Drawing.Size(31, 23);
            this.btnSelectJQPage.TabIndex = 7;
            this.btnSelectJQPage.Text = "...";
            this.btnSelectJQPage.UseVisualStyleBackColor = true;
            this.btnSelectJQPage.Click += new System.EventHandler(this.btnSelectJQPage_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Please input Form Name";
            // 
            // tbFormName
            // 
            this.tbFormName.Location = new System.Drawing.Point(61, 210);
            this.tbFormName.Name = "tbFormName";
            this.tbFormName.Size = new System.Drawing.Size(450, 21);
            this.tbFormName.TabIndex = 4;
            this.tbFormName.TextChanged += new System.EventHandler(this.tbFormName_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(59, 243);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(143, 12);
            this.label10.TabIndex = 5;
            this.label10.Text = "Please input Form Title";
            // 
            // tbFormTitle
            // 
            this.tbFormTitle.Location = new System.Drawing.Point(61, 264);
            this.tbFormTitle.Name = "tbFormTitle";
            this.tbFormTitle.Size = new System.Drawing.Size(450, 21);
            this.tbFormTitle.TabIndex = 6;
            // 
            // tpOutputSetting
            // 
            this.tpOutputSetting.AutoScroll = true;
            this.tpOutputSetting.Controls.Add(this.panel1);
            this.tpOutputSetting.Controls.Add(this.cbWebSite);
            this.tpOutputSetting.Controls.Add(this.label2);
            this.tpOutputSetting.Controls.Add(this.rbAddToRootFolder);
            this.tpOutputSetting.Controls.Add(this.tbAddToNewFolder);
            this.tpOutputSetting.Controls.Add(this.rbAddToNewFolder);
            this.tpOutputSetting.Controls.Add(this.cbAddToExistFolder);
            this.tpOutputSetting.Controls.Add(this.rbAddToExistFolder);
            this.tpOutputSetting.Location = new System.Drawing.Point(4, 22);
            this.tpOutputSetting.Name = "tpOutputSetting";
            this.tpOutputSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tpOutputSetting.Size = new System.Drawing.Size(725, 347);
            this.tpOutputSetting.TabIndex = 0;
            this.tpOutputSetting.Text = "Output Setting";
            this.tpOutputSetting.UseVisualStyleBackColor = true;
            // 
            // rbAddToExistFolder
            // 
            this.rbAddToExistFolder.AutoSize = true;
            this.rbAddToExistFolder.Checked = true;
            this.rbAddToExistFolder.Font = new System.Drawing.Font("Arial", 8F);
            this.rbAddToExistFolder.Location = new System.Drawing.Point(73, 219);
            this.rbAddToExistFolder.Name = "rbAddToExistFolder";
            this.rbAddToExistFolder.Size = new System.Drawing.Size(118, 18);
            this.rbAddToExistFolder.TabIndex = 39;
            this.rbAddToExistFolder.TabStop = true;
            this.rbAddToExistFolder.Text = "Add To Exist Folder";
            this.rbAddToExistFolder.UseVisualStyleBackColor = true;
            this.rbAddToExistFolder.CheckedChanged += new System.EventHandler(this.rbAddToExistFolder_CheckedChanged);
            // 
            // cbAddToExistFolder
            // 
            this.cbAddToExistFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAddToExistFolder.FormattingEnabled = true;
            this.cbAddToExistFolder.Location = new System.Drawing.Point(195, 218);
            this.cbAddToExistFolder.Name = "cbAddToExistFolder";
            this.cbAddToExistFolder.Size = new System.Drawing.Size(376, 20);
            this.cbAddToExistFolder.TabIndex = 40;
            // 
            // rbAddToNewFolder
            // 
            this.rbAddToNewFolder.AutoSize = true;
            this.rbAddToNewFolder.Font = new System.Drawing.Font("Arial", 8F);
            this.rbAddToNewFolder.Location = new System.Drawing.Point(73, 254);
            this.rbAddToNewFolder.Name = "rbAddToNewFolder";
            this.rbAddToNewFolder.Size = new System.Drawing.Size(118, 18);
            this.rbAddToNewFolder.TabIndex = 41;
            this.rbAddToNewFolder.Text = "Add To New Folder";
            this.rbAddToNewFolder.UseVisualStyleBackColor = true;
            this.rbAddToNewFolder.CheckedChanged += new System.EventHandler(this.rbAddToNewFolder_CheckedChanged);
            // 
            // tbAddToNewFolder
            // 
            this.tbAddToNewFolder.Location = new System.Drawing.Point(195, 253);
            this.tbAddToNewFolder.Name = "tbAddToNewFolder";
            this.tbAddToNewFolder.Size = new System.Drawing.Size(376, 21);
            this.tbAddToNewFolder.TabIndex = 42;
            // 
            // rbAddToRootFolder
            // 
            this.rbAddToRootFolder.AutoSize = true;
            this.rbAddToRootFolder.Font = new System.Drawing.Font("Arial", 8F);
            this.rbAddToRootFolder.Location = new System.Drawing.Point(73, 287);
            this.rbAddToRootFolder.Name = "rbAddToRootFolder";
            this.rbAddToRootFolder.Size = new System.Drawing.Size(117, 18);
            this.rbAddToRootFolder.TabIndex = 43;
            this.rbAddToRootFolder.Text = "Add To Root Folder";
            this.rbAddToRootFolder.UseVisualStyleBackColor = true;
            this.rbAddToRootFolder.CheckedChanged += new System.EventHandler(this.rbAddToRootFolder_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8F);
            this.label2.Location = new System.Drawing.Point(113, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 14);
            this.label2.TabIndex = 44;
            this.label2.Text = "WebSite: ";
            // 
            // cbWebSite
            // 
            this.cbWebSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWebSite.FormattingEnabled = true;
            this.cbWebSite.Location = new System.Drawing.Point(171, 168);
            this.cbWebSite.Name = "cbWebSite";
            this.cbWebSite.Size = new System.Drawing.Size(400, 20);
            this.cbWebSite.TabIndex = 45;
            this.cbWebSite.SelectedIndexChanged += new System.EventHandler(this.cbWebSite_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbCurrentSolution);
            this.panel1.Controls.Add(this.rbAddToExistSolution);
            this.panel1.Controls.Add(this.tbSolutionName);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbCurrentSolution);
            this.panel1.Controls.Add(this.btnSolutionName);
            this.panel1.Location = new System.Drawing.Point(52, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(556, 142);
            this.panel1.TabIndex = 50;
            // 
            // btnSolutionName
            // 
            this.btnSolutionName.Location = new System.Drawing.Point(520, 103);
            this.btnSolutionName.Name = "btnSolutionName";
            this.btnSolutionName.Size = new System.Drawing.Size(27, 23);
            this.btnSolutionName.TabIndex = 38;
            this.btnSolutionName.Text = "...";
            this.btnSolutionName.Click += new System.EventHandler(this.btnSolutionName_Click);
            // 
            // tbCurrentSolution
            // 
            this.tbCurrentSolution.BackColor = System.Drawing.SystemColors.Menu;
            this.tbCurrentSolution.Enabled = false;
            this.tbCurrentSolution.Location = new System.Drawing.Point(119, 41);
            this.tbCurrentSolution.Name = "tbCurrentSolution";
            this.tbCurrentSolution.Size = new System.Drawing.Size(400, 21);
            this.tbCurrentSolution.TabIndex = 47;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8F);
            this.label1.Location = new System.Drawing.Point(36, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 14);
            this.label1.TabIndex = 37;
            this.label1.Text = "Solution Name:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 8F);
            this.label7.Location = new System.Drawing.Point(36, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 14);
            this.label7.TabIndex = 48;
            this.label7.Text = "Solution Name:";
            // 
            // tbSolutionName
            // 
            this.tbSolutionName.Location = new System.Drawing.Point(119, 104);
            this.tbSolutionName.Name = "tbSolutionName";
            this.tbSolutionName.Size = new System.Drawing.Size(400, 21);
            this.tbSolutionName.TabIndex = 36;
            // 
            // rbAddToExistSolution
            // 
            this.rbAddToExistSolution.AutoSize = true;
            this.rbAddToExistSolution.Font = new System.Drawing.Font("Arial", 8F);
            this.rbAddToExistSolution.Location = new System.Drawing.Point(18, 82);
            this.rbAddToExistSolution.Name = "rbAddToExistSolution";
            this.rbAddToExistSolution.Size = new System.Drawing.Size(126, 18);
            this.rbAddToExistSolution.TabIndex = 49;
            this.rbAddToExistSolution.TabStop = true;
            this.rbAddToExistSolution.Text = "Add To Exist Solution";
            this.rbAddToExistSolution.UseVisualStyleBackColor = true;
            this.rbAddToExistSolution.CheckedChanged += new System.EventHandler(this.rbAddToExistSolution_CheckedChanged);
            // 
            // rbCurrentSolution
            // 
            this.rbCurrentSolution.AutoSize = true;
            this.rbCurrentSolution.Font = new System.Drawing.Font("Arial", 8F);
            this.rbCurrentSolution.Location = new System.Drawing.Point(18, 19);
            this.rbCurrentSolution.Name = "rbCurrentSolution";
            this.rbCurrentSolution.Size = new System.Drawing.Size(139, 18);
            this.rbCurrentSolution.TabIndex = 46;
            this.rbCurrentSolution.TabStop = true;
            this.rbCurrentSolution.Text = "Add To Current Solution";
            this.rbCurrentSolution.UseVisualStyleBackColor = true;
            this.rbCurrentSolution.CheckedChanged += new System.EventHandler(this.rbCurrentSolution_CheckedChanged);
            // 
            // tpConnection
            // 
            this.tpConnection.Controls.Add(this.cbEEPAlias);
            this.tpConnection.Controls.Add(this.cbChooseLanguage);
            this.tpConnection.Controls.Add(this.label11);
            this.tpConnection.Controls.Add(this.label15);
            this.tpConnection.Controls.Add(this.cbDatabaseType);
            this.tpConnection.Controls.Add(this.label13);
            this.tpConnection.Controls.Add(this.btnConnectionString);
            this.tpConnection.Controls.Add(this.tbConnectionString);
            this.tpConnection.Controls.Add(this.label12);
            this.tpConnection.Location = new System.Drawing.Point(4, 22);
            this.tpConnection.Name = "tpConnection";
            this.tpConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tpConnection.Size = new System.Drawing.Size(725, 347);
            this.tpConnection.TabIndex = 6;
            this.tpConnection.Text = "Connection";
            this.tpConnection.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 8F);
            this.label12.Location = new System.Drawing.Point(90, 117);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(92, 14);
            this.label12.TabIndex = 0;
            this.label12.Text = "Connection String";
            // 
            // tbConnectionString
            // 
            this.tbConnectionString.Location = new System.Drawing.Point(188, 113);
            this.tbConnectionString.Name = "tbConnectionString";
            this.tbConnectionString.Size = new System.Drawing.Size(391, 21);
            this.tbConnectionString.TabIndex = 1;
            // 
            // btnConnectionString
            // 
            this.btnConnectionString.Location = new System.Drawing.Point(581, 113);
            this.btnConnectionString.Name = "btnConnectionString";
            this.btnConnectionString.Size = new System.Drawing.Size(26, 23);
            this.btnConnectionString.TabIndex = 2;
            this.btnConnectionString.Text = "...";
            this.btnConnectionString.UseVisualStyleBackColor = true;
            this.btnConnectionString.Click += new System.EventHandler(this.btnConnectionString_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 8F);
            this.label13.Location = new System.Drawing.Point(102, 167);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 14);
            this.label13.TabIndex = 3;
            this.label13.Text = "Database Type";
            // 
            // cbDatabaseType
            // 
            this.cbDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDatabaseType.FormattingEnabled = true;
            this.cbDatabaseType.Items.AddRange(new object[] {
            "None",
            "MSSQL",
            "OleDB",
            "Oracle",
            "ODBC",
            "MySql",
            "Informix",
            "Sybase"});
            this.cbDatabaseType.Location = new System.Drawing.Point(188, 165);
            this.cbDatabaseType.Name = "cbDatabaseType";
            this.cbDatabaseType.Size = new System.Drawing.Size(184, 20);
            this.cbDatabaseType.TabIndex = 4;
            this.cbDatabaseType.SelectedIndexChanged += new System.EventHandler(this.cbDatabaseType_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Arial", 8F);
            this.label15.Location = new System.Drawing.Point(130, 64);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(51, 14);
            this.label15.TabIndex = 5;
            this.label15.Text = "EEP Alias";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 8F);
            this.label11.Location = new System.Drawing.Point(87, 213);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 14);
            this.label11.TabIndex = 14;
            this.label11.Text = "Choose Language";
            // 
            // cbChooseLanguage
            // 
            this.cbChooseLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChooseLanguage.FormattingEnabled = true;
            this.cbChooseLanguage.Items.AddRange(new object[] {
            "C#",
            "VB"});
            this.cbChooseLanguage.Location = new System.Drawing.Point(186, 211);
            this.cbChooseLanguage.Name = "cbChooseLanguage";
            this.cbChooseLanguage.Size = new System.Drawing.Size(184, 20);
            this.cbChooseLanguage.TabIndex = 15;
            // 
            // cbEEPAlias
            // 
            this.cbEEPAlias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEEPAlias.FormattingEnabled = true;
            this.cbEEPAlias.Location = new System.Drawing.Point(186, 62);
            this.cbEEPAlias.Name = "cbEEPAlias";
            this.cbEEPAlias.Size = new System.Drawing.Size(421, 20);
            this.cbEEPAlias.TabIndex = 16;
            this.cbEEPAlias.SelectedIndexChanged += new System.EventHandler(this.cbEEPAlias_SelectedIndexChanged);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpConnection);
            this.tabControl.Controls.Add(this.tpOutputSetting);
            this.tabControl.Controls.Add(this.tpFormSetting);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeftLayout = true;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(733, 373);
            this.tabControl.TabIndex = 8;
            // 
            // fmJQueryToJQMobile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 425);
            this.ControlBox = false;
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnNext);
            this.Name = "fmJQueryToJQMobile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JQuery Web Form Wizard";
            this.tpFormSetting.ResumeLayout(false);
            this.tpFormSetting.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tpOutputSetting.ResumeLayout(false);
            this.tpOutputSetting.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tpConnection.ResumeLayout(false);
            this.tpConnection.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabPage tpFormSetting;
        private System.Windows.Forms.TextBox tbFormTitle;
        private System.Windows.Forms.TextBox tbFormName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelectJQPage;
        private System.Windows.Forms.TextBox txtSelectJQPage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tpOutputSetting;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbCurrentSolution;
        private System.Windows.Forms.RadioButton rbAddToExistSolution;
        private System.Windows.Forms.TextBox tbSolutionName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCurrentSolution;
        private System.Windows.Forms.Button btnSolutionName;
        private System.Windows.Forms.ComboBox cbWebSite;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbAddToRootFolder;
        private System.Windows.Forms.TextBox tbAddToNewFolder;
        private System.Windows.Forms.RadioButton rbAddToNewFolder;
        private System.Windows.Forms.ComboBox cbAddToExistFolder;
        private System.Windows.Forms.RadioButton rbAddToExistFolder;
        private System.Windows.Forms.TabPage tpConnection;
        private System.Windows.Forms.ComboBox cbEEPAlias;
        private System.Windows.Forms.ComboBox cbChooseLanguage;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cbDatabaseType;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnConnectionString;
        private System.Windows.Forms.TextBox tbConnectionString;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabControl tabControl;


	}
}