namespace MWizard2015
{
	partial class fmServerWzd
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpConnection = new System.Windows.Forms.TabPage();
            this.cbChooseLanguage = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbEEPAlias = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cbDatabaseType = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnConnectionString = new System.Windows.Forms.Button();
            this.tbConnectionString = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tpOutputSetting = new System.Windows.Forms.TabPage();
            this.btnAssemblyOutputPath = new System.Windows.Forms.Button();
            this.tbAssemblyOutputPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnOutputPath = new System.Windows.Forms.Button();
            this.tbOutputPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCurrentSolution = new System.Windows.Forms.TextBox();
            this.rbAddToCurrent = new System.Windows.Forms.RadioButton();
            this.btnNewLocation = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tbNewLocation = new System.Windows.Forms.TextBox();
            this.btnSolutionName = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSolutionName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbNewSolutionName = new System.Windows.Forms.TextBox();
            this.rbAddToExistSln = new System.Windows.Forms.RadioButton();
            this.rbNewSolution = new System.Windows.Forms.RadioButton();
            this.tbPackageName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tpTables = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRelation = new System.Windows.Forms.Button();
            this.btnDeleteField = new System.Windows.Forms.Button();
            this.btnNewField = new System.Windows.Forms.Button();
            this.cbCheckNull = new System.Windows.Forms.CheckBox();
            this.cbIsKey = new System.Windows.Forms.CheckBox();
            this.lvSelectedFields = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDeleteDataset = new System.Windows.Forms.Button();
            this.btnNewDataset = new System.Windows.Forms.Button();
            this.btnNewSubDataset = new System.Windows.Forms.Button();
            this.tvTables = new System.Windows.Forms.TreeView();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tpConnection.SuspendLayout();
            this.tpOutputSetting.SuspendLayout();
            this.tpTables.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpConnection);
            this.tabControl.Controls.Add(this.tpOutputSetting);
            this.tabControl.Controls.Add(this.tpTables);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeftLayout = true;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(672, 346);
            this.tabControl.TabIndex = 9;
            // 
            // tpConnection
            // 
            this.tpConnection.Controls.Add(this.cbChooseLanguage);
            this.tpConnection.Controls.Add(this.label8);
            this.tpConnection.Controls.Add(this.cbEEPAlias);
            this.tpConnection.Controls.Add(this.label15);
            this.tpConnection.Controls.Add(this.cbDatabaseType);
            this.tpConnection.Controls.Add(this.label13);
            this.tpConnection.Controls.Add(this.btnConnectionString);
            this.tpConnection.Controls.Add(this.tbConnectionString);
            this.tpConnection.Controls.Add(this.label12);
            this.tpConnection.Location = new System.Drawing.Point(4, 22);
            this.tpConnection.Name = "tpConnection";
            this.tpConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tpConnection.Size = new System.Drawing.Size(664, 320);
            this.tpConnection.TabIndex = 3;
            this.tpConnection.Text = "Connection";
            this.tpConnection.UseVisualStyleBackColor = true;
            // 
            // cbChooseLanguage
            // 
            this.cbChooseLanguage.FormattingEnabled = true;
            this.cbChooseLanguage.Items.AddRange(new object[] {
            "C#",
            "VB"});
            this.cbChooseLanguage.Location = new System.Drawing.Point(171, 215);
            this.cbChooseLanguage.Name = "cbChooseLanguage";
            this.cbChooseLanguage.Size = new System.Drawing.Size(184, 20);
            this.cbChooseLanguage.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 8F);
            this.label8.Location = new System.Drawing.Point(72, 217);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 14);
            this.label8.TabIndex = 12;
            this.label8.Text = "Choose Language";
            // 
            // cbEEPAlias
            // 
            this.cbEEPAlias.FormattingEnabled = true;
            this.cbEEPAlias.Location = new System.Drawing.Point(171, 85);
            this.cbEEPAlias.Name = "cbEEPAlias";
            this.cbEEPAlias.Size = new System.Drawing.Size(419, 20);
            this.cbEEPAlias.TabIndex = 11;
            this.cbEEPAlias.SelectedIndexChanged += new System.EventHandler(this.cbEEPAlias_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Arial", 8F);
            this.label15.Location = new System.Drawing.Point(114, 88);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(52, 14);
            this.label15.TabIndex = 10;
            this.label15.Text = "EEP Alias";
            // 
            // cbDatabaseType
            // 
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
            this.cbDatabaseType.Location = new System.Drawing.Point(171, 172);
            this.cbDatabaseType.Name = "cbDatabaseType";
            this.cbDatabaseType.Size = new System.Drawing.Size(184, 20);
            this.cbDatabaseType.TabIndex = 9;
            this.cbDatabaseType.SelectedIndexChanged += new System.EventHandler(this.cbDatabaseType_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 8F);
            this.label13.Location = new System.Drawing.Point(87, 174);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 14);
            this.label13.TabIndex = 8;
            this.label13.Text = "Database Type";
            // 
            // btnConnectionString
            // 
            this.btnConnectionString.Location = new System.Drawing.Point(564, 124);
            this.btnConnectionString.Name = "btnConnectionString";
            this.btnConnectionString.Size = new System.Drawing.Size(26, 23);
            this.btnConnectionString.TabIndex = 7;
            this.btnConnectionString.Text = "...";
            this.btnConnectionString.UseVisualStyleBackColor = true;
            this.btnConnectionString.Click += new System.EventHandler(this.btnConnectionString_Click);
            // 
            // tbConnectionString
            // 
            this.tbConnectionString.Location = new System.Drawing.Point(171, 127);
            this.tbConnectionString.Name = "tbConnectionString";
            this.tbConnectionString.Size = new System.Drawing.Size(391, 21);
            this.tbConnectionString.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 8F);
            this.label12.Location = new System.Drawing.Point(75, 127);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(92, 14);
            this.label12.TabIndex = 5;
            this.label12.Text = "Connection String";
            // 
            // tpOutputSetting
            // 
            this.tpOutputSetting.AutoScroll = true;
            this.tpOutputSetting.Controls.Add(this.btnAssemblyOutputPath);
            this.tpOutputSetting.Controls.Add(this.tbAssemblyOutputPath);
            this.tpOutputSetting.Controls.Add(this.label7);
            this.tpOutputSetting.Controls.Add(this.btnOutputPath);
            this.tpOutputSetting.Controls.Add(this.tbOutputPath);
            this.tpOutputSetting.Controls.Add(this.label6);
            this.tpOutputSetting.Controls.Add(this.label1);
            this.tpOutputSetting.Controls.Add(this.tbCurrentSolution);
            this.tpOutputSetting.Controls.Add(this.rbAddToCurrent);
            this.tpOutputSetting.Controls.Add(this.btnNewLocation);
            this.tpOutputSetting.Controls.Add(this.label5);
            this.tpOutputSetting.Controls.Add(this.tbNewLocation);
            this.tpOutputSetting.Controls.Add(this.btnSolutionName);
            this.tpOutputSetting.Controls.Add(this.label4);
            this.tpOutputSetting.Controls.Add(this.tbSolutionName);
            this.tpOutputSetting.Controls.Add(this.label3);
            this.tpOutputSetting.Controls.Add(this.tbNewSolutionName);
            this.tpOutputSetting.Controls.Add(this.rbAddToExistSln);
            this.tpOutputSetting.Controls.Add(this.rbNewSolution);
            this.tpOutputSetting.Controls.Add(this.tbPackageName);
            this.tpOutputSetting.Controls.Add(this.label2);
            this.tpOutputSetting.Location = new System.Drawing.Point(4, 22);
            this.tpOutputSetting.Name = "tpOutputSetting";
            this.tpOutputSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tpOutputSetting.Size = new System.Drawing.Size(664, 320);
            this.tpOutputSetting.TabIndex = 0;
            this.tpOutputSetting.Text = "Output Setting";
            this.tpOutputSetting.UseVisualStyleBackColor = true;
            // 
            // btnAssemblyOutputPath
            // 
            this.btnAssemblyOutputPath.Location = new System.Drawing.Point(586, 253);
            this.btnAssemblyOutputPath.Name = "btnAssemblyOutputPath";
            this.btnAssemblyOutputPath.Size = new System.Drawing.Size(27, 23);
            this.btnAssemblyOutputPath.TabIndex = 23;
            this.btnAssemblyOutputPath.Text = "...";
            this.btnAssemblyOutputPath.Click += new System.EventHandler(this.btnAssemblyOutputPath_Click);
            // 
            // tbAssemblyOutputPath
            // 
            this.tbAssemblyOutputPath.Location = new System.Drawing.Point(149, 254);
            this.tbAssemblyOutputPath.Name = "tbAssemblyOutputPath";
            this.tbAssemblyOutputPath.Size = new System.Drawing.Size(431, 21);
            this.tbAssemblyOutputPath.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 8F);
            this.label7.Location = new System.Drawing.Point(26, 257);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 14);
            this.label7.TabIndex = 21;
            this.label7.Text = "Assembly Output Path:";
            // 
            // btnOutputPath
            // 
            this.btnOutputPath.Location = new System.Drawing.Point(586, 225);
            this.btnOutputPath.Name = "btnOutputPath";
            this.btnOutputPath.Size = new System.Drawing.Size(27, 23);
            this.btnOutputPath.TabIndex = 20;
            this.btnOutputPath.Text = "...";
            this.btnOutputPath.Click += new System.EventHandler(this.btnOutputPath_Click);
            // 
            // tbOutputPath
            // 
            this.tbOutputPath.Location = new System.Drawing.Point(149, 226);
            this.tbOutputPath.Name = "tbOutputPath";
            this.tbOutputPath.Size = new System.Drawing.Size(431, 21);
            this.tbOutputPath.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 8F);
            this.label6.Location = new System.Drawing.Point(49, 229);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 14);
            this.label6.TabIndex = 18;
            this.label6.Text = "Code Output Path:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8F);
            this.label1.Location = new System.Drawing.Point(104, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 14);
            this.label1.TabIndex = 17;
            this.label1.Text = "Solution Name:";
            // 
            // tbCurrentSolution
            // 
            this.tbCurrentSolution.Enabled = false;
            this.tbCurrentSolution.Location = new System.Drawing.Point(187, 40);
            this.tbCurrentSolution.Name = "tbCurrentSolution";
            this.tbCurrentSolution.ReadOnly = true;
            this.tbCurrentSolution.Size = new System.Drawing.Size(393, 21);
            this.tbCurrentSolution.TabIndex = 16;
            this.tbCurrentSolution.TextChanged += new System.EventHandler(this.tbCurrentSolution_TextChanged);
            // 
            // rbAddToCurrent
            // 
            this.rbAddToCurrent.AutoSize = true;
            this.rbAddToCurrent.Checked = true;
            this.rbAddToCurrent.Enabled = false;
            this.rbAddToCurrent.Font = new System.Drawing.Font("Arial", 8F);
            this.rbAddToCurrent.Location = new System.Drawing.Point(69, 18);
            this.rbAddToCurrent.Name = "rbAddToCurrent";
            this.rbAddToCurrent.Size = new System.Drawing.Size(140, 18);
            this.rbAddToCurrent.TabIndex = 15;
            this.rbAddToCurrent.TabStop = true;
            this.rbAddToCurrent.Text = "Add To Current Solution";
            this.rbAddToCurrent.UseVisualStyleBackColor = true;
            this.rbAddToCurrent.Click += new System.EventHandler(this.rbAddToCurrent_Click);
            // 
            // btnNewLocation
            // 
            this.btnNewLocation.Location = new System.Drawing.Point(585, 170);
            this.btnNewLocation.Name = "btnNewLocation";
            this.btnNewLocation.Size = new System.Drawing.Size(27, 23);
            this.btnNewLocation.TabIndex = 14;
            this.btnNewLocation.Text = "...";
            this.btnNewLocation.Visible = false;
            this.btnNewLocation.Click += new System.EventHandler(this.btnNewSolution_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 8F);
            this.label5.Location = new System.Drawing.Point(133, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 14);
            this.label5.TabIndex = 13;
            this.label5.Text = "Location:";
            this.label5.Visible = false;
            // 
            // tbNewLocation
            // 
            this.tbNewLocation.Location = new System.Drawing.Point(186, 170);
            this.tbNewLocation.Name = "tbNewLocation";
            this.tbNewLocation.Size = new System.Drawing.Size(393, 21);
            this.tbNewLocation.TabIndex = 12;
            this.tbNewLocation.Text = "D:\\VS2005\\SrvTestSolution";
            this.tbNewLocation.Visible = false;
            this.tbNewLocation.TextChanged += new System.EventHandler(this.tbCurrentSolution_TextChanged);
            // 
            // btnSolutionName
            // 
            this.btnSolutionName.Enabled = false;
            this.btnSolutionName.Location = new System.Drawing.Point(585, 99);
            this.btnSolutionName.Name = "btnSolutionName";
            this.btnSolutionName.Size = new System.Drawing.Size(27, 23);
            this.btnSolutionName.TabIndex = 11;
            this.btnSolutionName.Text = "...";
            this.btnSolutionName.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 8F);
            this.label4.Location = new System.Drawing.Point(104, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 14);
            this.label4.TabIndex = 10;
            this.label4.Text = "Solution Name:";
            // 
            // tbSolutionName
            // 
            this.tbSolutionName.Enabled = false;
            this.tbSolutionName.Location = new System.Drawing.Point(187, 100);
            this.tbSolutionName.Name = "tbSolutionName";
            this.tbSolutionName.Size = new System.Drawing.Size(393, 21);
            this.tbSolutionName.TabIndex = 9;
            this.tbSolutionName.Text = "D:\\VS2005\\SrvTestSolution\\SrvTestSolution.sln";
            this.tbSolutionName.TextChanged += new System.EventHandler(this.tbCurrentSolution_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 8F);
            this.label3.Location = new System.Drawing.Point(105, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 14);
            this.label3.TabIndex = 8;
            this.label3.Text = "Solution Name:";
            this.label3.Visible = false;
            // 
            // tbNewSolutionName
            // 
            this.tbNewSolutionName.Location = new System.Drawing.Point(186, 143);
            this.tbNewSolutionName.Name = "tbNewSolutionName";
            this.tbNewSolutionName.Size = new System.Drawing.Size(393, 21);
            this.tbNewSolutionName.TabIndex = 7;
            this.tbNewSolutionName.Text = "SrvTestSolution";
            this.tbNewSolutionName.Visible = false;
            this.tbNewSolutionName.TextChanged += new System.EventHandler(this.tbNewSolutionName_TextChanged);
            // 
            // rbAddToExistSln
            // 
            this.rbAddToExistSln.AutoSize = true;
            this.rbAddToExistSln.Font = new System.Drawing.Font("Arial", 8F);
            this.rbAddToExistSln.Location = new System.Drawing.Point(69, 78);
            this.rbAddToExistSln.Name = "rbAddToExistSln";
            this.rbAddToExistSln.Size = new System.Drawing.Size(127, 18);
            this.rbAddToExistSln.TabIndex = 6;
            this.rbAddToExistSln.Text = "Add To Exist Solution";
            this.rbAddToExistSln.Click += new System.EventHandler(this.rbAddToExistSln_Click);
            // 
            // rbNewSolution
            // 
            this.rbNewSolution.AutoSize = true;
            this.rbNewSolution.Font = new System.Drawing.Font("Arial", 8F);
            this.rbNewSolution.Location = new System.Drawing.Point(68, 121);
            this.rbNewSolution.Name = "rbNewSolution";
            this.rbNewSolution.Size = new System.Drawing.Size(124, 18);
            this.rbNewSolution.TabIndex = 5;
            this.rbNewSolution.Text = "Create New Solution";
            this.rbNewSolution.Visible = false;
            this.rbNewSolution.Click += new System.EventHandler(this.rbNewSolution_Click);
            // 
            // tbPackageName
            // 
            this.tbPackageName.Location = new System.Drawing.Point(149, 198);
            this.tbPackageName.Name = "tbPackageName";
            this.tbPackageName.Size = new System.Drawing.Size(309, 21);
            this.tbPackageName.TabIndex = 4;
            this.tbPackageName.Text = "SrvTestSolution";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8F);
            this.label2.Location = new System.Drawing.Point(62, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "Package Name:";
            // 
            // tpTables
            // 
            this.tpTables.Controls.Add(this.groupBox2);
            this.tpTables.Controls.Add(this.groupBox1);
            this.tpTables.Location = new System.Drawing.Point(4, 22);
            this.tpTables.Name = "tpTables";
            this.tpTables.Padding = new System.Windows.Forms.Padding(3);
            this.tpTables.Size = new System.Drawing.Size(664, 320);
            this.tpTables.TabIndex = 2;
            this.tpTables.Text = "Select Tables";
            this.tpTables.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRelation);
            this.groupBox2.Controls.Add(this.btnDeleteField);
            this.groupBox2.Controls.Add(this.btnNewField);
            this.groupBox2.Controls.Add(this.cbCheckNull);
            this.groupBox2.Controls.Add(this.cbIsKey);
            this.groupBox2.Controls.Add(this.lvSelectedFields);
            this.groupBox2.Location = new System.Drawing.Point(283, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(373, 312);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fields";
            // 
            // btnRelation
            // 
            this.btnRelation.Enabled = false;
            this.btnRelation.Location = new System.Drawing.Point(281, 78);
            this.btnRelation.Name = "btnRelation";
            this.btnRelation.Size = new System.Drawing.Size(62, 23);
            this.btnRelation.TabIndex = 6;
            this.btnRelation.Text = "Relation";
            this.btnRelation.UseVisualStyleBackColor = true;
            this.btnRelation.Click += new System.EventHandler(this.btnRelation_Click);
            // 
            // btnDeleteField
            // 
            this.btnDeleteField.Location = new System.Drawing.Point(281, 261);
            this.btnDeleteField.Name = "btnDeleteField";
            this.btnDeleteField.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteField.TabIndex = 5;
            this.btnDeleteField.Text = "Delete";
            this.btnDeleteField.Click += new System.EventHandler(this.btnDeleteField_Click);
            // 
            // btnNewField
            // 
            this.btnNewField.Location = new System.Drawing.Point(281, 220);
            this.btnNewField.Name = "btnNewField";
            this.btnNewField.Size = new System.Drawing.Size(75, 23);
            this.btnNewField.TabIndex = 4;
            this.btnNewField.Text = "Add";
            this.btnNewField.Click += new System.EventHandler(this.btnAddField_Click);
            // 
            // cbCheckNull
            // 
            this.cbCheckNull.AutoSize = true;
            this.cbCheckNull.Enabled = false;
            this.cbCheckNull.Font = new System.Drawing.Font("Arial", 8F);
            this.cbCheckNull.Location = new System.Drawing.Point(281, 54);
            this.cbCheckNull.Name = "cbCheckNull";
            this.cbCheckNull.Size = new System.Drawing.Size(62, 18);
            this.cbCheckNull.TabIndex = 2;
            this.cbCheckNull.Text = "Not Null";
            this.cbCheckNull.Click += new System.EventHandler(this.cbCheckNull_Click);
            // 
            // cbIsKey
            // 
            this.cbIsKey.AutoSize = true;
            this.cbIsKey.Enabled = false;
            this.cbIsKey.Font = new System.Drawing.Font("Arial", 8F);
            this.cbIsKey.Location = new System.Drawing.Point(281, 32);
            this.cbIsKey.Name = "cbIsKey";
            this.cbIsKey.Size = new System.Drawing.Size(45, 18);
            this.cbIsKey.TabIndex = 1;
            this.cbIsKey.Text = "Key";
            this.cbIsKey.Click += new System.EventHandler(this.cbIsKey_Click);
            // 
            // lvSelectedFields
            // 
            this.lvSelectedFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvSelectedFields.HideSelection = false;
            this.lvSelectedFields.Location = new System.Drawing.Point(6, 21);
            this.lvSelectedFields.Name = "lvSelectedFields";
            this.lvSelectedFields.Size = new System.Drawing.Size(258, 285);
            this.lvSelectedFields.TabIndex = 0;
            this.lvSelectedFields.UseCompatibleStateImageBehavior = false;
            this.lvSelectedFields.View = System.Windows.Forms.View.Details;
            this.lvSelectedFields.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSelectedFields_ColumnClick);
            this.lvSelectedFields.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvSelectedFields_ItemSelectionChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Field Name";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.Width = 120;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDeleteDataset);
            this.groupBox1.Controls.Add(this.btnNewDataset);
            this.groupBox1.Controls.Add(this.btnNewSubDataset);
            this.groupBox1.Controls.Add(this.tvTables);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 312);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tables";
            // 
            // btnDeleteDataset
            // 
            this.btnDeleteDataset.Location = new System.Drawing.Point(189, 283);
            this.btnDeleteDataset.Name = "btnDeleteDataset";
            this.btnDeleteDataset.Size = new System.Drawing.Size(53, 23);
            this.btnDeleteDataset.TabIndex = 3;
            this.btnDeleteDataset.Text = "Delete";
            this.btnDeleteDataset.Click += new System.EventHandler(this.btnDeleteDataset_Click);
            // 
            // btnNewDataset
            // 
            this.btnNewDataset.Location = new System.Drawing.Point(6, 283);
            this.btnNewDataset.Name = "btnNewDataset";
            this.btnNewDataset.Size = new System.Drawing.Size(53, 23);
            this.btnNewDataset.TabIndex = 2;
            this.btnNewDataset.Text = "Add";
            this.btnNewDataset.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnNewSubDataset
            // 
            this.btnNewSubDataset.Location = new System.Drawing.Point(65, 283);
            this.btnNewSubDataset.Name = "btnNewSubDataset";
            this.btnNewSubDataset.Size = new System.Drawing.Size(79, 23);
            this.btnNewSubDataset.TabIndex = 1;
            this.btnNewSubDataset.Text = "Add Child";
            this.btnNewSubDataset.Click += new System.EventHandler(this.btnAddNext_Click);
            // 
            // tvTables
            // 
            this.tvTables.HideSelection = false;
            this.tvTables.Location = new System.Drawing.Point(6, 21);
            this.tvTables.Name = "tvTables";
            this.tvTables.Size = new System.Drawing.Size(236, 247);
            this.tvTables.TabIndex = 0;
            this.tvTables.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTables_AfterSelect);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(476, 362);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 20;
            this.btnDone.Text = "Done";
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(28, 362);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnPrevious.TabIndex = 19;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(563, 362);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(121, 362);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 17;
            this.btnNext.Text = "Next";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.SelectedPath = "folderBrowserDialog1";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.sln";
            this.openFileDialog.Filter = "Solution Files (*.sln)|*.sln";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(303, 362);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "GetFormImage";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(384, 362);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 22;
            this.button2.Text = "ViewInBrowser";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_2);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(222, 362);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 23;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // fmServerWzd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(672, 401);
            this.ControlBox = false;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.tabControl);
            this.Name = "fmServerWzd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EEP Server Package Wizard";
            this.Load += new System.EventHandler(this.fmServerWzd_Load);
            this.tabControl.ResumeLayout(false);
            this.tpConnection.ResumeLayout(false);
            this.tpConnection.PerformLayout();
            this.tpOutputSetting.ResumeLayout(false);
            this.tpOutputSetting.PerformLayout();
            this.tpTables.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tpOutputSetting;
		private System.Windows.Forms.TextBox tbPackageName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnDone;
		private System.Windows.Forms.Button btnPrevious;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.TabPage tpTables;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnDeleteDataset;
		private System.Windows.Forms.Button btnNewDataset;
		private System.Windows.Forms.Button btnNewSubDataset;
		private System.Windows.Forms.TreeView tvTables;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox cbIsKey;
        private System.Windows.Forms.ListView lvSelectedFields;
		private System.Windows.Forms.CheckBox cbCheckNull;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Button btnDeleteField;
		private System.Windows.Forms.Button btnNewField;
		private System.Windows.Forms.RadioButton rbNewSolution;
		private System.Windows.Forms.RadioButton rbAddToExistSln;
		private System.Windows.Forms.TextBox tbNewSolutionName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbSolutionName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnSolutionName;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button btnNewLocation;
		private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbNewLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCurrentSolution;
        private System.Windows.Forms.RadioButton rbAddToCurrent;
        private System.Windows.Forms.TabPage tpConnection;
        private System.Windows.Forms.ComboBox cbDatabaseType;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnConnectionString;
        private System.Windows.Forms.TextBox tbConnectionString;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbEEPAlias;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnOutputPath;
        private System.Windows.Forms.TextBox tbOutputPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnAssemblyOutputPath;
        private System.Windows.Forms.TextBox tbAssemblyOutputPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnRelation;
        private System.Windows.Forms.ComboBox cbChooseLanguage;
        private System.Windows.Forms.Label label8;
	}
}