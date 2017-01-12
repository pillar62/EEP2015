namespace EEPManager
{
    partial class frmSysLogViewerForDB
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
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("All Log");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("System");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Access Provider");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Call Method");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Warning");
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Error");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Unknown");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("UserDefine");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("Email");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("EEP Log", new System.Windows.Forms.TreeNode[] {
            treeNode26,
            treeNode27,
            treeNode28,
            treeNode29,
            treeNode30,
            treeNode31,
            treeNode32,
            treeNode33,
            treeNode34});
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("All Log");
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("System");
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("Access Provider");
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("Call Method");
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("Warning");
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("Error");
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("Unknown");
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("UserDefine");
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("Email");
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("EEPLog", new System.Windows.Forms.TreeNode[] {
            treeNode36,
            treeNode37,
            treeNode38,
            treeNode39,
            treeNode40,
            treeNode41,
            treeNode42,
            treeNode43,
            treeNode44});
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("AllLog");
            System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("ExecuteSql");
            System.Windows.Forms.TreeNode treeNode48 = new System.Windows.Forms.TreeNode("InfoCommand");
            System.Windows.Forms.TreeNode treeNode49 = new System.Windows.Forms.TreeNode("UpdateComp");
            System.Windows.Forms.TreeNode treeNode50 = new System.Windows.Forms.TreeNode("EEP Log", new System.Windows.Forms.TreeNode[] {
            treeNode46,
            treeNode47,
            treeNode48,
            treeNode49});
            this.tp2 = new System.Windows.Forms.TabPage();
            this.dgvDBLog = new System.Windows.Forms.DataGridView();
            this.cmsDBGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDBSearch = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbDBLogStyle = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbDBLogType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbDBUserId = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.dtpDBTo = new System.Windows.Forms.DateTimePicker();
            this.dtpDBFrom = new System.Windows.Forms.DateTimePicker();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tViewDB = new System.Windows.Forms.TreeView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnDBRead = new System.Windows.Forms.Button();
            this.tp1 = new System.Windows.Forms.TabPage();
            this.dgvFileLog = new System.Windows.Forms.DataGridView();
            this.cmsGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyConnIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.locateConnIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panFileSearch = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbLogStyle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbLogType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbUserId = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tView = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnFileRead = new System.Windows.Forms.Button();
            this.tab = new System.Windows.Forms.TabControl();
            this.tp3 = new System.Windows.Forms.TabPage();
            this.txtSqlSentence = new System.Windows.Forms.TextBox();
            this.dgvSqlLog = new System.Windows.Forms.DataGridView();
            this.panel8 = new System.Windows.Forms.Panel();
            this.tbKeyWord = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSqlSearch = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbSqlLogType = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbSqlUserId = new System.Windows.Forms.ComboBox();
            this.dtpSqlTo = new System.Windows.Forms.DateTimePicker();
            this.dtpSqlFrom = new System.Windows.Forms.DateTimePicker();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tViewSql = new System.Windows.Forms.TreeView();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnSqlFileRead = new System.Windows.Forms.Button();
            this.tp2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDBLog)).BeginInit();
            this.cmsDBGrid.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tp1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileLog)).BeginInit();
            this.cmsGrid.SuspendLayout();
            this.panFileSearch.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tab.SuspendLayout();
            this.tp3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSqlLog)).BeginInit();
            this.panel8.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // tp2
            // 
            this.tp2.Controls.Add(this.dgvDBLog);
            this.tp2.Controls.Add(this.panel5);
            this.tp2.Controls.Add(this.panel3);
            this.tp2.Location = new System.Drawing.Point(4, 21);
            this.tp2.Name = "tp2";
            this.tp2.Size = new System.Drawing.Size(810, 434);
            this.tp2.TabIndex = 2;
            this.tp2.Text = "Log To DataBase";
            this.tp2.UseVisualStyleBackColor = true;
            // 
            // dgvDBLog
            // 
            this.dgvDBLog.AllowUserToAddRows = false;
            this.dgvDBLog.AllowUserToDeleteRows = false;
            this.dgvDBLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDBLog.ContextMenuStrip = this.cmsDBGrid;
            this.dgvDBLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDBLog.Location = new System.Drawing.Point(200, 49);
            this.dgvDBLog.MultiSelect = false;
            this.dgvDBLog.Name = "dgvDBLog";
            this.dgvDBLog.ReadOnly = true;
            this.dgvDBLog.RowTemplate.Height = 23;
            this.dgvDBLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDBLog.Size = new System.Drawing.Size(610, 385);
            this.dgvDBLog.TabIndex = 18;
            this.dgvDBLog.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvDBLog_CellPainting);
            // 
            // cmsDBGrid
            // 
            this.cmsDBGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.cmsDBGrid.Name = "cmsGrid";
            this.cmsDBGrid.Size = new System.Drawing.Size(146, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem1.Text = "Copy ConnID";
            this.toolStripMenuItem1.Visible = false;
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem2.Text = "Locate ConnID";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.btnDBSearch);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.cmbDBLogStyle);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.cmbDBLogType);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.cmbDBUserId);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.dtpDBTo);
            this.panel5.Controls.Add(this.dtpDBFrom);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(200, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(610, 49);
            this.panel5.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(162, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "Log Type:";
            // 
            // btnDBSearch
            // 
            this.btnDBSearch.Location = new System.Drawing.Point(527, 11);
            this.btnDBSearch.Name = "btnDBSearch";
            this.btnDBSearch.Size = new System.Drawing.Size(75, 23);
            this.btnDBSearch.TabIndex = 12;
            this.btnDBSearch.Text = "Search";
            this.btnDBSearch.UseVisualStyleBackColor = true;
            this.btnDBSearch.Click += new System.EventHandler(this.btnDBSearch_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "Date From:";
            // 
            // cmbDBLogStyle
            // 
            this.cmbDBLogStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDBLogStyle.FormattingEnabled = true;
            this.cmbDBLogStyle.Items.AddRange(new object[] {
            "All",
            "System",
            "Provider",
            "Method",
            "UserDefine",
            "Email"});
            this.cmbDBLogStyle.Location = new System.Drawing.Point(401, 13);
            this.cmbDBLogStyle.Name = "cmbDBLogStyle";
            this.cmbDBLogStyle.Size = new System.Drawing.Size(88, 20);
            this.cmbDBLogStyle.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(259, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "Date To:";
            // 
            // cmbDBLogType
            // 
            this.cmbDBLogType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDBLogType.FormattingEnabled = true;
            this.cmbDBLogType.Items.AddRange(new object[] {
            "All",
            "Normal",
            "Warning",
            "Error",
            "Unknown"});
            this.cmbDBLogType.Location = new System.Drawing.Point(227, 13);
            this.cmbDBLogType.Name = "cmbDBLogType";
            this.cmbDBLogType.Size = new System.Drawing.Size(88, 20);
            this.cmbDBLogType.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 4;
            this.label9.Text = "User Id:";
            // 
            // cmbDBUserId
            // 
            this.cmbDBUserId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDBUserId.FormattingEnabled = true;
            this.cmbDBUserId.Location = new System.Drawing.Point(64, 13);
            this.cmbDBUserId.Name = "cmbDBUserId";
            this.cmbDBUserId.Size = new System.Drawing.Size(88, 20);
            this.cmbDBUserId.TabIndex = 9;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(330, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 6;
            this.label10.Text = "Log Style:";
            // 
            // dtpDBTo
            // 
            this.dtpDBTo.Checked = false;
            this.dtpDBTo.Location = new System.Drawing.Point(318, 61);
            this.dtpDBTo.Name = "dtpDBTo";
            this.dtpDBTo.ShowCheckBox = true;
            this.dtpDBTo.Size = new System.Drawing.Size(130, 21);
            this.dtpDBTo.TabIndex = 8;
            // 
            // dtpDBFrom
            // 
            this.dtpDBFrom.Checked = false;
            this.dtpDBFrom.Location = new System.Drawing.Point(103, 61);
            this.dtpDBFrom.Name = "dtpDBFrom";
            this.dtpDBFrom.ShowCheckBox = true;
            this.dtpDBFrom.Size = new System.Drawing.Size(132, 21);
            this.dtpDBFrom.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.LightYellow;
            this.panel3.Controls.Add(this.tViewDB);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 434);
            this.panel3.TabIndex = 16;
            // 
            // tViewDB
            // 
            this.tViewDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tViewDB.Location = new System.Drawing.Point(0, 0);
            this.tViewDB.Name = "tViewDB";
            treeNode26.Name = "nAllLog";
            treeNode26.Text = "All Log";
            treeNode27.Name = "nSystem";
            treeNode27.Text = "System";
            treeNode28.Name = "nAccessProvider";
            treeNode28.Text = "Access Provider";
            treeNode29.Name = "nCallMethod";
            treeNode29.Text = "Call Method";
            treeNode30.Name = "nWarning";
            treeNode30.Text = "Warning";
            treeNode31.Name = "nError";
            treeNode31.Text = "Error";
            treeNode32.Name = "nUnknown";
            treeNode32.Text = "Unknown";
            treeNode33.Name = "nUserDefine";
            treeNode33.Text = "UserDefine";
            treeNode34.Name = "nEmail";
            treeNode34.Text = "Email";
            treeNode35.Name = "nEEPLog";
            treeNode35.Text = "EEP Log";
            this.tViewDB.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode35});
            this.tViewDB.Size = new System.Drawing.Size(200, 399);
            this.tViewDB.TabIndex = 1;
            this.tViewDB.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tViewDB_AfterSelect);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnDBRead);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 399);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(200, 35);
            this.panel4.TabIndex = 0;
            // 
            // btnDBRead
            // 
            this.btnDBRead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDBRead.Location = new System.Drawing.Point(0, 0);
            this.btnDBRead.Name = "btnDBRead";
            this.btnDBRead.Size = new System.Drawing.Size(200, 35);
            this.btnDBRead.TabIndex = 17;
            this.btnDBRead.Text = "read from database";
            this.btnDBRead.UseVisualStyleBackColor = true;
            this.btnDBRead.Click += new System.EventHandler(this.btnDBRead_Click);
            // 
            // tp1
            // 
            this.tp1.Controls.Add(this.dgvFileLog);
            this.tp1.Controls.Add(this.panFileSearch);
            this.tp1.Controls.Add(this.panel1);
            this.tp1.Location = new System.Drawing.Point(4, 21);
            this.tp1.Name = "tp1";
            this.tp1.Padding = new System.Windows.Forms.Padding(3);
            this.tp1.Size = new System.Drawing.Size(810, 434);
            this.tp1.TabIndex = 0;
            this.tp1.Text = "Log To File";
            this.tp1.UseVisualStyleBackColor = true;
            // 
            // dgvFileLog
            // 
            this.dgvFileLog.AllowUserToAddRows = false;
            this.dgvFileLog.AllowUserToDeleteRows = false;
            this.dgvFileLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFileLog.ContextMenuStrip = this.cmsGrid;
            this.dgvFileLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFileLog.Location = new System.Drawing.Point(203, 52);
            this.dgvFileLog.MultiSelect = false;
            this.dgvFileLog.Name = "dgvFileLog";
            this.dgvFileLog.ReadOnly = true;
            this.dgvFileLog.RowTemplate.Height = 23;
            this.dgvFileLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFileLog.Size = new System.Drawing.Size(604, 379);
            this.dgvFileLog.TabIndex = 14;
            this.dgvFileLog.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvFileLog_CellPainting);
            // 
            // cmsGrid
            // 
            this.cmsGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyConnIDToolStripMenuItem,
            this.locateConnIDToolStripMenuItem});
            this.cmsGrid.Name = "cmsGrid";
            this.cmsGrid.Size = new System.Drawing.Size(146, 48);
            // 
            // copyConnIDToolStripMenuItem
            // 
            this.copyConnIDToolStripMenuItem.Name = "copyConnIDToolStripMenuItem";
            this.copyConnIDToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.copyConnIDToolStripMenuItem.Text = "Copy ConnID";
            this.copyConnIDToolStripMenuItem.Visible = false;
            this.copyConnIDToolStripMenuItem.Click += new System.EventHandler(this.copyConnIDToolStripMenuItem_Click);
            // 
            // locateConnIDToolStripMenuItem
            // 
            this.locateConnIDToolStripMenuItem.Name = "locateConnIDToolStripMenuItem";
            this.locateConnIDToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.locateConnIDToolStripMenuItem.Text = "Locate ConnID";
            this.locateConnIDToolStripMenuItem.Click += new System.EventHandler(this.locateConnIDToolStripMenuItem_Click);
            // 
            // panFileSearch
            // 
            this.panFileSearch.Controls.Add(this.label4);
            this.panFileSearch.Controls.Add(this.btnSearch);
            this.panFileSearch.Controls.Add(this.label1);
            this.panFileSearch.Controls.Add(this.cmbLogStyle);
            this.panFileSearch.Controls.Add(this.label2);
            this.panFileSearch.Controls.Add(this.cmbLogType);
            this.panFileSearch.Controls.Add(this.label3);
            this.panFileSearch.Controls.Add(this.cmbUserId);
            this.panFileSearch.Controls.Add(this.label5);
            this.panFileSearch.Controls.Add(this.dtpTo);
            this.panFileSearch.Controls.Add(this.dtpFrom);
            this.panFileSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panFileSearch.Location = new System.Drawing.Point(203, 3);
            this.panFileSearch.Name = "panFileSearch";
            this.panFileSearch.Size = new System.Drawing.Size(604, 49);
            this.panFileSearch.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(163, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "Log Type:";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(515, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Date From:";
            // 
            // cmbLogStyle
            // 
            this.cmbLogStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogStyle.FormattingEnabled = true;
            this.cmbLogStyle.Items.AddRange(new object[] {
            "All",
            "System",
            "Provider",
            "Method",
            "UserDefine",
            "Email"});
            this.cmbLogStyle.Location = new System.Drawing.Point(398, 14);
            this.cmbLogStyle.Name = "cmbLogStyle";
            this.cmbLogStyle.Size = new System.Drawing.Size(88, 20);
            this.cmbLogStyle.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(259, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Date To:";
            // 
            // cmbLogType
            // 
            this.cmbLogType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogType.FormattingEnabled = true;
            this.cmbLogType.Items.AddRange(new object[] {
            "All",
            "Normal",
            "Warning",
            "Error",
            "Unknown"});
            this.cmbLogType.Location = new System.Drawing.Point(228, 14);
            this.cmbLogType.Name = "cmbLogType";
            this.cmbLogType.Size = new System.Drawing.Size(88, 20);
            this.cmbLogType.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "User Id:";
            // 
            // cmbUserId
            // 
            this.cmbUserId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserId.FormattingEnabled = true;
            this.cmbUserId.Location = new System.Drawing.Point(64, 14);
            this.cmbUserId.Name = "cmbUserId";
            this.cmbUserId.Size = new System.Drawing.Size(88, 20);
            this.cmbUserId.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(327, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "Log Style:";
            // 
            // dtpTo
            // 
            this.dtpTo.Checked = false;
            this.dtpTo.Location = new System.Drawing.Point(318, 61);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowCheckBox = true;
            this.dtpTo.Size = new System.Drawing.Size(130, 21);
            this.dtpTo.TabIndex = 8;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Checked = false;
            this.dtpFrom.Location = new System.Drawing.Point(103, 61);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowCheckBox = true;
            this.dtpFrom.Size = new System.Drawing.Size(132, 21);
            this.dtpFrom.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightYellow;
            this.panel1.Controls.Add(this.tView);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 428);
            this.panel1.TabIndex = 0;
            // 
            // tView
            // 
            this.tView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tView.Location = new System.Drawing.Point(0, 0);
            this.tView.Name = "tView";
            treeNode36.Name = "nAllLog";
            treeNode36.Text = "All Log";
            treeNode37.Name = "nSystem";
            treeNode37.Text = "System";
            treeNode38.Name = "nAccessProvider";
            treeNode38.Text = "Access Provider";
            treeNode39.Name = "nCallMethod";
            treeNode39.Text = "Call Method";
            treeNode40.Name = "nWarning";
            treeNode40.Text = "Warning";
            treeNode41.Name = "nError";
            treeNode41.Text = "Error";
            treeNode42.Name = "nUnknown";
            treeNode42.Text = "Unknown";
            treeNode43.Name = "nUserDefine";
            treeNode43.Text = "UserDefine";
            treeNode44.Name = "nEmail";
            treeNode44.Text = "Email";
            treeNode45.Name = "nEEPLog";
            treeNode45.Text = "EEPLog";
            this.tView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode45});
            this.tView.Size = new System.Drawing.Size(200, 393);
            this.tView.TabIndex = 1;
            this.tView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tView_AfterSelect);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnFileRead);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 393);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 35);
            this.panel2.TabIndex = 0;
            // 
            // btnFileRead
            // 
            this.btnFileRead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFileRead.Location = new System.Drawing.Point(0, 0);
            this.btnFileRead.Name = "btnFileRead";
            this.btnFileRead.Size = new System.Drawing.Size(200, 35);
            this.btnFileRead.TabIndex = 15;
            this.btnFileRead.Text = "read from files";
            this.btnFileRead.UseVisualStyleBackColor = true;
            this.btnFileRead.Click += new System.EventHandler(this.btnFileRead_Click);
            // 
            // tab
            // 
            this.tab.Controls.Add(this.tp1);
            this.tab.Controls.Add(this.tp2);
            this.tab.Controls.Add(this.tp3);
            this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab.Location = new System.Drawing.Point(0, 0);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(818, 459);
            this.tab.TabIndex = 16;
            // 
            // tp3
            // 
            this.tp3.Controls.Add(this.txtSqlSentence);
            this.tp3.Controls.Add(this.dgvSqlLog);
            this.tp3.Controls.Add(this.panel8);
            this.tp3.Controls.Add(this.panel6);
            this.tp3.Location = new System.Drawing.Point(4, 21);
            this.tp3.Name = "tp3";
            this.tp3.Size = new System.Drawing.Size(810, 434);
            this.tp3.TabIndex = 3;
            this.tp3.Text = "Log Sql Sentence";
            this.tp3.UseVisualStyleBackColor = true;
            // 
            // txtSqlSentence
            // 
            this.txtSqlSentence.BackColor = System.Drawing.Color.AliceBlue;
            this.txtSqlSentence.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSqlSentence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSqlSentence.ForeColor = System.Drawing.Color.DimGray;
            this.txtSqlSentence.Location = new System.Drawing.Point(200, 271);
            this.txtSqlSentence.Multiline = true;
            this.txtSqlSentence.Name = "txtSqlSentence";
            this.txtSqlSentence.ReadOnly = true;
            this.txtSqlSentence.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSqlSentence.Size = new System.Drawing.Size(610, 163);
            this.txtSqlSentence.TabIndex = 20;
            // 
            // dgvSqlLog
            // 
            this.dgvSqlLog.AllowUserToAddRows = false;
            this.dgvSqlLog.AllowUserToDeleteRows = false;
            this.dgvSqlLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSqlLog.ContextMenuStrip = this.cmsDBGrid;
            this.dgvSqlLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvSqlLog.Location = new System.Drawing.Point(200, 47);
            this.dgvSqlLog.MultiSelect = false;
            this.dgvSqlLog.Name = "dgvSqlLog";
            this.dgvSqlLog.ReadOnly = true;
            this.dgvSqlLog.RowTemplate.Height = 23;
            this.dgvSqlLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSqlLog.Size = new System.Drawing.Size(610, 224);
            this.dgvSqlLog.TabIndex = 19;
            this.dgvSqlLog.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvSqlLog_CellPainting);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.tbKeyWord);
            this.panel8.Controls.Add(this.label15);
            this.panel8.Controls.Add(this.label11);
            this.panel8.Controls.Add(this.btnSqlSearch);
            this.panel8.Controls.Add(this.label12);
            this.panel8.Controls.Add(this.label13);
            this.panel8.Controls.Add(this.cmbSqlLogType);
            this.panel8.Controls.Add(this.label14);
            this.panel8.Controls.Add(this.cmbSqlUserId);
            this.panel8.Controls.Add(this.dtpSqlTo);
            this.panel8.Controls.Add(this.dtpSqlFrom);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(200, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(610, 47);
            this.panel8.TabIndex = 1;
            // 
            // tbKeyWord
            // 
            this.tbKeyWord.Location = new System.Drawing.Point(400, 11);
            this.tbKeyWord.Name = "tbKeyWord";
            this.tbKeyWord.Size = new System.Drawing.Size(100, 21);
            this.tbKeyWord.TabIndex = 25;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(333, 15);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(59, 12);
            this.label15.TabIndex = 24;
            this.label15.Text = "Key Word:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(158, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 12);
            this.label11.TabIndex = 16;
            this.label11.Text = "Sql Log Type:";
            // 
            // btnSqlSearch
            // 
            this.btnSqlSearch.Location = new System.Drawing.Point(517, 10);
            this.btnSqlSearch.Name = "btnSqlSearch";
            this.btnSqlSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSqlSearch.TabIndex = 23;
            this.btnSqlSearch.Text = "Search";
            this.btnSqlSearch.UseVisualStyleBackColor = true;
            this.btnSqlSearch.Click += new System.EventHandler(this.btnSqlSearch_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(43, 75);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 13;
            this.label12.Text = "Date From:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(270, 75);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 14;
            this.label13.Text = "Date To:";
            // 
            // cmbSqlLogType
            // 
            this.cmbSqlLogType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSqlLogType.FormattingEnabled = true;
            this.cmbSqlLogType.Items.AddRange(new object[] {
            "All",
            "ExecuteSql",
            "InfoCommand",
            "UpdateComp"});
            this.cmbSqlLogType.Location = new System.Drawing.Point(248, 12);
            this.cmbSqlLogType.Name = "cmbSqlLogType";
            this.cmbSqlLogType.Size = new System.Drawing.Size(71, 20);
            this.cmbSqlLogType.TabIndex = 21;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 15);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 15;
            this.label14.Text = "User Id:";
            // 
            // cmbSqlUserId
            // 
            this.cmbSqlUserId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSqlUserId.FormattingEnabled = true;
            this.cmbSqlUserId.Location = new System.Drawing.Point(67, 12);
            this.cmbSqlUserId.Name = "cmbSqlUserId";
            this.cmbSqlUserId.Size = new System.Drawing.Size(81, 20);
            this.cmbSqlUserId.TabIndex = 20;
            // 
            // dtpSqlTo
            // 
            this.dtpSqlTo.Checked = false;
            this.dtpSqlTo.Location = new System.Drawing.Point(329, 71);
            this.dtpSqlTo.Name = "dtpSqlTo";
            this.dtpSqlTo.ShowCheckBox = true;
            this.dtpSqlTo.Size = new System.Drawing.Size(130, 21);
            this.dtpSqlTo.TabIndex = 19;
            // 
            // dtpSqlFrom
            // 
            this.dtpSqlFrom.Checked = false;
            this.dtpSqlFrom.Location = new System.Drawing.Point(114, 71);
            this.dtpSqlFrom.Name = "dtpSqlFrom";
            this.dtpSqlFrom.ShowCheckBox = true;
            this.dtpSqlFrom.Size = new System.Drawing.Size(132, 21);
            this.dtpSqlFrom.TabIndex = 18;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.tViewSql);
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(200, 434);
            this.panel6.TabIndex = 0;
            // 
            // tViewSql
            // 
            this.tViewSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tViewSql.Location = new System.Drawing.Point(0, 0);
            this.tViewSql.Name = "tViewSql";
            treeNode46.Name = "nAllLog";
            treeNode46.Text = "AllLog";
            treeNode47.Name = "nExecuteSql";
            treeNode47.Text = "ExecuteSql";
            treeNode48.Name = "nInfoCommand";
            treeNode48.Text = "InfoCommand";
            treeNode49.Name = "nUpdateComp";
            treeNode49.Text = "UpdateComp";
            treeNode50.Name = "nEEPLog";
            treeNode50.Text = "EEP Log";
            this.tViewSql.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode50});
            this.tViewSql.Size = new System.Drawing.Size(200, 399);
            this.tViewSql.TabIndex = 2;
            this.tViewSql.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tViewSql_AfterSelect);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Transparent;
            this.panel7.Controls.Add(this.btnSqlFileRead);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 399);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(200, 35);
            this.panel7.TabIndex = 1;
            // 
            // btnSqlFileRead
            // 
            this.btnSqlFileRead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSqlFileRead.Location = new System.Drawing.Point(0, 0);
            this.btnSqlFileRead.Name = "btnSqlFileRead";
            this.btnSqlFileRead.Size = new System.Drawing.Size(200, 35);
            this.btnSqlFileRead.TabIndex = 0;
            this.btnSqlFileRead.Text = "Read";
            this.btnSqlFileRead.UseVisualStyleBackColor = true;
            this.btnSqlFileRead.Click += new System.EventHandler(this.btnSqlFileRead_Click);
            // 
            // frmSysLogViewerForDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 459);
            this.Controls.Add(this.tab);
            this.Name = "frmSysLogViewerForDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Log Viewer";
            this.Load += new System.EventHandler(this.frmSysLogViewerForDB_Load);
            this.tp2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDBLog)).EndInit();
            this.cmsDBGrid.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tp1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileLog)).EndInit();
            this.cmsGrid.ResumeLayout(false);
            this.panFileSearch.ResumeLayout(false);
            this.panFileSearch.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tab.ResumeLayout(false);
            this.tp3.ResumeLayout(false);
            this.tp3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSqlLog)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tp2;
        private System.Windows.Forms.TabPage tp1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbLogStyle;
        private System.Windows.Forms.ComboBox cmbLogType;
        private System.Windows.Forms.ComboBox cmbUserId;
        private System.Windows.Forms.DataGridView dgvFileLog;
        private System.Windows.Forms.Panel panFileSearch;
        private System.Windows.Forms.TreeView tView;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnFileRead;
        private System.Windows.Forms.ContextMenuStrip cmsGrid;
        private System.Windows.Forms.ToolStripMenuItem copyConnIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem locateConnIDToolStripMenuItem;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnDBSearch;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbDBLogStyle;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbDBLogType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbDBUserId;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtpDBTo;
        private System.Windows.Forms.DateTimePicker dtpDBFrom;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TreeView tViewDB;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnDBRead;
        private System.Windows.Forms.DataGridView dgvDBLog;
        private System.Windows.Forms.ContextMenuStrip cmsDBGrid;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.TabPage tp3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TreeView tViewSql;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnSqlFileRead;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnSqlSearch;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbSqlLogType;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbSqlUserId;
        private System.Windows.Forms.DateTimePicker dtpSqlTo;
        private System.Windows.Forms.DateTimePicker dtpSqlFrom;
        private System.Windows.Forms.DataGridView dgvSqlLog;
        private System.Windows.Forms.TextBox txtSqlSentence;
        private System.Windows.Forms.TextBox tbKeyWord;
        private System.Windows.Forms.Label label15;
    }
}