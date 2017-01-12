namespace Scheduling
{
    partial class frmMain
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
            //if (disposing && (components != null))
            //{
            //    components.Dispose();
            //}
            //    base.Dispose(disposing);

            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TSactive = new System.Windows.Forms.ToolStripMenuItem();
            this.TSstop = new System.Windows.Forms.ToolStripMenuItem();
            this.TSsetting = new System.Windows.Forms.ToolStripMenuItem();
            this.TShide = new System.Windows.Forms.ToolStripMenuItem();
            this.TSclose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMainNew = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMainModify = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMainDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMainSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMainExit = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMainOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMainMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMainWatchLog = new System.Windows.Forms.ToolStripMenuItem();
            this.lbScheduling = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.LDescription = new System.Windows.Forms.Label();
            this.tbDescriotion = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbActive = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSchedulingMode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbServerMethod = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbParameters = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbCycleUnit = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbCycle = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbCycleHour = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbWhen = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbErr = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tbLastDateTime = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.cbDataBase = new System.Windows.Forms.ComboBox();
            this.cbLog = new System.Windows.Forms.ComboBox();
            this.btnModify = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSolution = new System.Windows.Forms.ComboBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnServerMethod = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.cbSendMailMode = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.timerConnect = new System.Windows.Forms.Timer(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tbErrN = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.tbUserMsg = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSactive,
            this.TSstop,
            this.TSsetting,
            this.TShide,
            this.TSclose});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(117, 114);
            // 
            // TSactive
            // 
            this.TSactive.Name = "TSactive";
            this.TSactive.Size = new System.Drawing.Size(116, 22);
            this.TSactive.Text = "Active";
            this.TSactive.Click += new System.EventHandler(this.TSactive_Click);
            // 
            // TSstop
            // 
            this.TSstop.Name = "TSstop";
            this.TSstop.Size = new System.Drawing.Size(116, 22);
            this.TSstop.Text = "Stop";
            this.TSstop.Click += new System.EventHandler(this.TSstop_Click);
            // 
            // TSsetting
            // 
            this.TSsetting.Name = "TSsetting";
            this.TSsetting.Size = new System.Drawing.Size(116, 22);
            this.TSsetting.Text = "Setting";
            this.TSsetting.Click += new System.EventHandler(this.TSsetting_Click);
            // 
            // TShide
            // 
            this.TShide.Name = "TShide";
            this.TShide.Size = new System.Drawing.Size(116, 22);
            this.TShide.Text = "Hide";
            this.TShide.Click += new System.EventHandler(this.TShide_Click);
            // 
            // TSclose
            // 
            this.TSclose.Name = "TSclose";
            this.TSclose.Size = new System.Drawing.Size(116, 22);
            this.TSclose.Text = "Close";
            this.TSclose.Click += new System.EventHandler(this.TSclose_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.TSMainOptions,
            this.TSMainMonitor,
            this.TSMainWatchLog});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(681, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMainNew,
            this.TSMainModify,
            this.TSMainDelete,
            this.TSMainSave,
            this.toolStripSeparator1,
            this.TSMainExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // TSMainNew
            // 
            this.TSMainNew.Name = "TSMainNew";
            this.TSMainNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.TSMainNew.Size = new System.Drawing.Size(166, 22);
            this.TSMainNew.Text = "New";
            this.TSMainNew.Click += new System.EventHandler(this.TSMainNew_Click);
            // 
            // TSMainModify
            // 
            this.TSMainModify.Name = "TSMainModify";
            this.TSMainModify.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.TSMainModify.Size = new System.Drawing.Size(166, 22);
            this.TSMainModify.Text = "Modify";
            this.TSMainModify.Click += new System.EventHandler(this.TSMainModify_Click);
            // 
            // TSMainDelete
            // 
            this.TSMainDelete.Name = "TSMainDelete";
            this.TSMainDelete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.TSMainDelete.Size = new System.Drawing.Size(166, 22);
            this.TSMainDelete.Text = "Delete";
            this.TSMainDelete.Click += new System.EventHandler(this.TSMainDelete_Click);
            // 
            // TSMainSave
            // 
            this.TSMainSave.Name = "TSMainSave";
            this.TSMainSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.TSMainSave.Size = new System.Drawing.Size(166, 22);
            this.TSMainSave.Text = "Save";
            this.TSMainSave.Click += new System.EventHandler(this.TSMainSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // TSMainExit
            // 
            this.TSMainExit.Name = "TSMainExit";
            this.TSMainExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.TSMainExit.Size = new System.Drawing.Size(166, 22);
            this.TSMainExit.Text = "Exit";
            this.TSMainExit.Click += new System.EventHandler(this.TSMainClose_Click);
            // 
            // TSMainOptions
            // 
            this.TSMainOptions.Name = "TSMainOptions";
            this.TSMainOptions.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.TSMainOptions.Size = new System.Drawing.Size(66, 21);
            this.TSMainOptions.Text = "Options";
            this.TSMainOptions.Click += new System.EventHandler(this.TSMainOptions_Click);
            // 
            // TSMainMonitor
            // 
            this.TSMainMonitor.Name = "TSMainMonitor";
            this.TSMainMonitor.Size = new System.Drawing.Size(67, 21);
            this.TSMainMonitor.Text = "Monitor";
            this.TSMainMonitor.Click += new System.EventHandler(this.TSMainMonitor_Click);
            // 
            // TSMainWatchLog
            // 
            this.TSMainWatchLog.Name = "TSMainWatchLog";
            this.TSMainWatchLog.Size = new System.Drawing.Size(82, 21);
            this.TSMainWatchLog.Text = "Watch Log";
            this.TSMainWatchLog.Click += new System.EventHandler(this.TSMainWatchLog_Click);
            // 
            // lbScheduling
            // 
            this.lbScheduling.FormattingEnabled = true;
            this.lbScheduling.ItemHeight = 12;
            this.lbScheduling.Location = new System.Drawing.Point(6, 35);
            this.lbScheduling.Name = "lbScheduling";
            this.lbScheduling.Size = new System.Drawing.Size(174, 424);
            this.lbScheduling.TabIndex = 1;
            this.lbScheduling.SelectedIndexChanged += new System.EventHandler(this.lbScheduling_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 25;
            this.label1.Text = "Name:";
            // 
            // tbName
            // 
            this.tbName.Enabled = false;
            this.tbName.Location = new System.Drawing.Point(302, 41);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(109, 21);
            this.tbName.TabIndex = 2;
            // 
            // LDescription
            // 
            this.LDescription.AutoSize = true;
            this.LDescription.Location = new System.Drawing.Point(195, 79);
            this.LDescription.Name = "LDescription";
            this.LDescription.Size = new System.Drawing.Size(77, 12);
            this.LDescription.TabIndex = 27;
            this.LDescription.Text = "Description:";
            // 
            // tbDescriotion
            // 
            this.tbDescriotion.Enabled = false;
            this.tbDescriotion.Location = new System.Drawing.Point(302, 76);
            this.tbDescriotion.Multiline = true;
            this.tbDescriotion.Name = "tbDescriotion";
            this.tbDescriotion.Size = new System.Drawing.Size(325, 66);
            this.tbDescriotion.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(444, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "ActiveScheduling:";
            // 
            // cbActive
            // 
            this.cbActive.AutoSize = true;
            this.cbActive.Enabled = false;
            this.cbActive.Location = new System.Drawing.Point(564, 44);
            this.cbActive.Name = "cbActive";
            this.cbActive.Size = new System.Drawing.Size(15, 14);
            this.cbActive.TabIndex = 3;
            this.cbActive.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(195, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 28;
            this.label4.Text = "Scheduling Mode:";
            // 
            // cbSchedulingMode
            // 
            this.cbSchedulingMode.Enabled = false;
            this.cbSchedulingMode.FormattingEnabled = true;
            this.cbSchedulingMode.Items.AddRange(new object[] {
            "Server Method(sync)",
            "Server FLMethod(sync)",
            "EXE"});
            this.cbSchedulingMode.Location = new System.Drawing.Point(302, 159);
            this.cbSchedulingMode.Name = "cbSchedulingMode";
            this.cbSchedulingMode.Size = new System.Drawing.Size(123, 20);
            this.cbSchedulingMode.TabIndex = 5;
            this.cbSchedulingMode.SelectedIndexChanged += new System.EventHandler(this.cbSchedulingMode_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(195, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 12);
            this.label5.TabIndex = 30;
            this.label5.Text = "ServerMethod:";
            // 
            // tbServerMethod
            // 
            this.tbServerMethod.Enabled = false;
            this.tbServerMethod.Location = new System.Drawing.Point(302, 196);
            this.tbServerMethod.Name = "tbServerMethod";
            this.tbServerMethod.Size = new System.Drawing.Size(293, 21);
            this.tbServerMethod.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(444, 162);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 29;
            this.label6.Text = "Parameters:";
            // 
            // tbParameters
            // 
            this.tbParameters.Enabled = false;
            this.tbParameters.Location = new System.Drawing.Point(542, 159);
            this.tbParameters.Name = "tbParameters";
            this.tbParameters.Size = new System.Drawing.Size(112, 21);
            this.tbParameters.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(444, 370);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 40;
            this.label7.Text = "Database:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(195, 232);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 31;
            this.label8.Text = "CycleUnit:";
            // 
            // cbCycleUnit
            // 
            this.cbCycleUnit.Enabled = false;
            this.cbCycleUnit.FormattingEnabled = true;
            this.cbCycleUnit.Items.AddRange(new object[] {
            "InterVal",
            "Daily",
            "Monthly",
            "Weekly"});
            this.cbCycleUnit.Location = new System.Drawing.Point(302, 229);
            this.cbCycleUnit.Name = "cbCycleUnit";
            this.cbCycleUnit.Size = new System.Drawing.Size(123, 20);
            this.cbCycleUnit.TabIndex = 9;
            this.cbCycleUnit.SelectedIndexChanged += new System.EventHandler(this.cbCycleUnit_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(195, 265);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 33;
            this.label9.Text = "Cycle:";
            // 
            // tbCycle
            // 
            this.tbCycle.Enabled = false;
            this.tbCycle.Location = new System.Drawing.Point(302, 262);
            this.tbCycle.Name = "tbCycle";
            this.tbCycle.Size = new System.Drawing.Size(123, 21);
            this.tbCycle.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(444, 232);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 32;
            this.label10.Text = "CycleHour:";
            // 
            // tbCycleHour
            // 
            this.tbCycleHour.Enabled = false;
            this.tbCycleHour.Location = new System.Drawing.Point(542, 228);
            this.tbCycleHour.Name = "tbCycleHour";
            this.tbCycleHour.Size = new System.Drawing.Size(112, 21);
            this.tbCycleHour.TabIndex = 10;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(444, 265);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 12);
            this.label11.TabIndex = 34;
            this.label11.Text = "When:";
            // 
            // cbWhen
            // 
            this.cbWhen.Enabled = false;
            this.cbWhen.FormattingEnabled = true;
            this.cbWhen.Items.AddRange(new object[] {
            "All",
            "Night",
            "Day",
            "Holiday",
            "WorkDay"});
            this.cbWhen.Location = new System.Drawing.Point(542, 262);
            this.cbWhen.Name = "cbWhen";
            this.cbWhen.Size = new System.Drawing.Size(112, 20);
            this.cbWhen.TabIndex = 12;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(195, 371);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 12);
            this.label12.TabIndex = 37;
            this.label12.Text = "ErrorSendMail:";
            // 
            // tbErr
            // 
            this.tbErr.Enabled = false;
            this.tbErr.Location = new System.Drawing.Point(302, 367);
            this.tbErr.Name = "tbErr";
            this.tbErr.Size = new System.Drawing.Size(123, 21);
            this.tbErr.TabIndex = 15;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(444, 300);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 36;
            this.label13.Text = "Log:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(444, 404);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(83, 12);
            this.label14.TabIndex = 39;
            this.label14.Text = "LastDateTime:";
            // 
            // tbLastDateTime
            // 
            this.tbLastDateTime.Enabled = false;
            this.tbLastDateTime.Location = new System.Drawing.Point(542, 401);
            this.tbLastDateTime.Name = "tbLastDateTime";
            this.tbLastDateTime.Size = new System.Drawing.Size(109, 21);
            this.tbLastDateTime.TabIndex = 18;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(498, 436);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(280, 436);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 21;
            this.btnDel.Text = "Delete";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // cbDataBase
            // 
            this.cbDataBase.Enabled = false;
            this.cbDataBase.FormattingEnabled = true;
            this.cbDataBase.Location = new System.Drawing.Point(542, 367);
            this.cbDataBase.Name = "cbDataBase";
            this.cbDataBase.Size = new System.Drawing.Size(109, 20);
            this.cbDataBase.TabIndex = 19;
            // 
            // cbLog
            // 
            this.cbLog.Enabled = false;
            this.cbLog.FormattingEnabled = true;
            this.cbLog.Items.AddRange(new object[] {
            "None",
            "All",
            "ErrorOnly"});
            this.cbLog.Location = new System.Drawing.Point(542, 297);
            this.cbLog.Name = "cbLog";
            this.cbLog.Size = new System.Drawing.Size(112, 20);
            this.cbLog.TabIndex = 14;
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(199, 436);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 20;
            this.btnModify.Text = "Modify";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(579, 436);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(444, 336);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 38;
            this.label2.Text = "Solution:";
            // 
            // cbSolution
            // 
            this.cbSolution.Enabled = false;
            this.cbSolution.FormattingEnabled = true;
            this.cbSolution.Location = new System.Drawing.Point(542, 333);
            this.cbSolution.Name = "cbSolution";
            this.cbSolution.Size = new System.Drawing.Size(77, 20);
            this.cbSolution.TabIndex = 16;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(361, 436);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 22;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnServerMethod
            // 
            this.btnServerMethod.Enabled = false;
            this.btnServerMethod.Location = new System.Drawing.Point(601, 194);
            this.btnServerMethod.Name = "btnServerMethod";
            this.btnServerMethod.Size = new System.Drawing.Size(26, 23);
            this.btnServerMethod.TabIndex = 8;
            this.btnServerMethod.Text = "...";
            this.btnServerMethod.UseVisualStyleBackColor = true;
            this.btnServerMethod.Click += new System.EventHandler(this.btnServerMethod_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "exe files(*.exe)|*.exe|All files(*.*)|*.*";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Scheduling";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // cbSendMailMode
            // 
            this.cbSendMailMode.Enabled = false;
            this.cbSendMailMode.FormattingEnabled = true;
            this.cbSendMailMode.Items.AddRange(new object[] {
            "None",
            "All",
            "ErrorOnly"});
            this.cbSendMailMode.Location = new System.Drawing.Point(302, 297);
            this.cbSendMailMode.Name = "cbSendMailMode";
            this.cbSendMailMode.Size = new System.Drawing.Size(123, 20);
            this.cbSendMailMode.TabIndex = 13;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(195, 300);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(83, 12);
            this.label15.TabIndex = 35;
            this.label15.Text = "SendMailMode:";
            // 
            // timerConnect
            // 
            this.timerConnect.Interval = 10000;
            this.timerConnect.Tick += new System.EventHandler(this.timerConnect_Tick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(625, 333);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(29, 21);
            this.btnRefresh.TabIndex = 17;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Scheduling16.ico");
            this.imageList1.Images.SetKeyName(1, "Scheduling32.ico");
            this.imageList1.Images.SetKeyName(2, "SchedulingCanConnect.ico");
            this.imageList1.Images.SetKeyName(3, "SchedulingStop16.ico");
            // 
            // tbErrN
            // 
            this.tbErrN.Enabled = false;
            this.tbErrN.Location = new System.Drawing.Point(302, 402);
            this.tbErrN.Name = "tbErrN";
            this.tbErrN.Size = new System.Drawing.Size(123, 21);
            this.tbErrN.TabIndex = 41;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(195, 406);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(101, 12);
            this.label16.TabIndex = 42;
            this.label16.Text = "ErrorSendNumber:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(195, 337);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 12);
            this.label17.TabIndex = 43;
            this.label17.Text = "Subject:";
            // 
            // tbUserMsg
            // 
            this.tbUserMsg.Enabled = false;
            this.tbUserMsg.Location = new System.Drawing.Point(302, 333);
            this.tbUserMsg.Name = "tbUserMsg";
            this.tbUserMsg.Size = new System.Drawing.Size(123, 21);
            this.tbUserMsg.TabIndex = 44;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 481);
            this.Controls.Add(this.tbUserMsg);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.tbErrN);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.cbSendMailMode);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnServerMethod);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.cbSolution);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cbLog);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbErr);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbDataBase);
            this.Controls.Add(this.tbCycleHour);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tbLastDateTime);
            this.Controls.Add(this.cbCycleUnit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbWhen);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbCycle);
            this.Controls.Add(this.cbSchedulingMode);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbParameters);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbServerMethod);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbActive);
            this.Controls.Add(this.lbScheduling);
            this.Controls.Add(this.tbDescriotion);
            this.Controls.Add(this.LDescription);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scheduling";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem TSactive;
        private System.Windows.Forms.ToolStripMenuItem TSsetting;
        private System.Windows.Forms.ToolStripMenuItem TShide;
        private System.Windows.Forms.ToolStripMenuItem TSclose;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TSMainNew;
        private System.Windows.Forms.ListBox lbScheduling;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label LDescription;
        private System.Windows.Forms.TextBox tbDescriotion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbActive;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbSchedulingMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbServerMethod;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbParameters;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbCycleUnit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbCycle;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbCycleHour;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbWhen;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbErr;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbLastDateTime;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.ComboBox cbDataBase;
        private System.Windows.Forms.ComboBox cbLog;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripMenuItem TSstop;
        private System.Windows.Forms.ToolStripMenuItem TSMainOptions;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolStripMenuItem TSMainSave;
        private System.Windows.Forms.ToolStripMenuItem TSMainExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem TSMainModify;
        private System.Windows.Forms.ToolStripMenuItem TSMainDelete;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSolution;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.ToolStripMenuItem TSMainMonitor;
        private System.Windows.Forms.ToolStripMenuItem TSMainWatchLog;
        private System.Windows.Forms.Button btnServerMethod;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ComboBox cbSendMailMode;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Timer timerConnect;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox tbErrN;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tbUserMsg;
    }
}

