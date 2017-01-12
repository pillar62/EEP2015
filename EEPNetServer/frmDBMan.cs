using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Srvtools;

namespace EEPNetServer
{
    /// <summary>
    /// Summary description for WinForm2.
    /// </summary>
    public class frmDBMan : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private ListBox lbxIsMaster;
        private ListBox lbxTimeOut;
        private ListBox lbxMaxCount;
        private ListBox lbxDbType;
        private Button btnDelete;
        private Button btnMod;
        private Button btnAdd;
        private ListBox lbxDBString;
        private Button btnSave;
        private Panel panel1;
        private Button btnCreateSystemTable;
        private Button btnTestConnection;
        private CheckBox cbIsMaster;
        private MaskedTextBox txtTimeOut;
        private MaskedTextBox txtMaxCount;
        private Label label4;
        private Label label3;
        private Label label2;
        private ComboBox cbxDatabaseType;
        private TextBox edtDBString;
        private Label label1;
        private ListBox lbxDB;
        private TabPage tabPage2;
        private ComboBox cbxSysDB;
        private Label label5;
        private ListBox lbxPwd;
        private bool bHasSaved = true;
        private Label label6;
        private ComboBox cbxOdbcType;
        private ListBox lbxOdbcType;
        private Button buttonView;
        private ListBox lbxEncrypt;
        private Button btnAssociatedContext;
        private ListBox lstContext;
        private GroupBox gbEntityConnection;
        private ListBox lbxEncoding;

        //		private System.Collections.ArrayList DBList;
        private System.Xml.XmlDocument DBXML;

        public frmDBMan()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDBMan));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbxEncrypt = new System.Windows.Forms.ListBox();
            this.lbxOdbcType = new System.Windows.Forms.ListBox();
            this.lbxMaxCount = new System.Windows.Forms.ListBox();
            this.lbxDbType = new System.Windows.Forms.ListBox();
            this.lbxDBString = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbEntityConnection = new System.Windows.Forms.GroupBox();
            this.lstContext = new System.Windows.Forms.ListBox();
            this.btnAssociatedContext = new System.Windows.Forms.Button();
            this.buttonView = new System.Windows.Forms.Button();
            this.lbxPwd = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCreateSystemTable = new System.Windows.Forms.Button();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cbxOdbcType = new System.Windows.Forms.ComboBox();
            this.btnMod = new System.Windows.Forms.Button();
            this.cbIsMaster = new System.Windows.Forms.CheckBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lbxIsMaster = new System.Windows.Forms.ListBox();
            this.txtTimeOut = new System.Windows.Forms.MaskedTextBox();
            this.lbxTimeOut = new System.Windows.Forms.ListBox();
            this.txtMaxCount = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxDatabaseType = new System.Windows.Forms.ComboBox();
            this.edtDBString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbxDB = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbxSysDB = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lbxEncoding = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbEntityConnection.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(594, 486);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lbxEncrypt);
            this.tabPage1.Controls.Add(this.lbxOdbcType);
            this.tabPage1.Controls.Add(this.lbxMaxCount);
            this.tabPage1.Controls.Add(this.lbxDbType);
            this.tabPage1.Controls.Add(this.lbxDBString);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.lbxDB);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(586, 459);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DataBase";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lbxEncrypt
            // 
            this.lbxEncrypt.FormattingEnabled = true;
            this.lbxEncrypt.Location = new System.Drawing.Point(181, 450);
            this.lbxEncrypt.Name = "lbxEncrypt";
            this.lbxEncrypt.Size = new System.Drawing.Size(75, 4);
            this.lbxEncrypt.TabIndex = 24;
            this.lbxEncrypt.Visible = false;
            // 
            // lbxOdbcType
            // 
            this.lbxOdbcType.FormattingEnabled = true;
            this.lbxOdbcType.Location = new System.Drawing.Point(90, 447);
            this.lbxOdbcType.Name = "lbxOdbcType";
            this.lbxOdbcType.Size = new System.Drawing.Size(75, 4);
            this.lbxOdbcType.TabIndex = 23;
            this.lbxOdbcType.Visible = false;
            // 
            // lbxMaxCount
            // 
            this.lbxMaxCount.FormattingEnabled = true;
            this.lbxMaxCount.Location = new System.Drawing.Point(180, 437);
            this.lbxMaxCount.Name = "lbxMaxCount";
            this.lbxMaxCount.Size = new System.Drawing.Size(75, 4);
            this.lbxMaxCount.TabIndex = 19;
            this.lbxMaxCount.Visible = false;
            // 
            // lbxDbType
            // 
            this.lbxDbType.FormattingEnabled = true;
            this.lbxDbType.Location = new System.Drawing.Point(90, 437);
            this.lbxDbType.Name = "lbxDbType";
            this.lbxDbType.Size = new System.Drawing.Size(75, 4);
            this.lbxDbType.TabIndex = 18;
            this.lbxDbType.Visible = false;
            // 
            // lbxDBString
            // 
            this.lbxDBString.FormattingEnabled = true;
            this.lbxDBString.Location = new System.Drawing.Point(8, 437);
            this.lbxDBString.Name = "lbxDBString";
            this.lbxDBString.Size = new System.Drawing.Size(76, 4);
            this.lbxDBString.TabIndex = 14;
            this.lbxDBString.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.lbxEncoding);
            this.panel1.Controls.Add(this.gbEntityConnection);
            this.panel1.Controls.Add(this.buttonView);
            this.panel1.Controls.Add(this.lbxPwd);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnCreateSystemTable);
            this.panel1.Controls.Add(this.btnTestConnection);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.cbxOdbcType);
            this.panel1.Controls.Add(this.btnMod);
            this.panel1.Controls.Add(this.cbIsMaster);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.lbxIsMaster);
            this.panel1.Controls.Add(this.txtTimeOut);
            this.panel1.Controls.Add(this.lbxTimeOut);
            this.panel1.Controls.Add(this.txtMaxCount);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbxDatabaseType);
            this.panel1.Controls.Add(this.edtDBString);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(263, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(317, 441);
            this.panel1.TabIndex = 12;
            // 
            // gbEntityConnection
            // 
            this.gbEntityConnection.BackColor = System.Drawing.Color.Beige;
            this.gbEntityConnection.Controls.Add(this.lstContext);
            this.gbEntityConnection.Controls.Add(this.btnAssociatedContext);
            this.gbEntityConnection.Location = new System.Drawing.Point(20, 206);
            this.gbEntityConnection.Name = "gbEntityConnection";
            this.gbEntityConnection.Size = new System.Drawing.Size(279, 151);
            this.gbEntityConnection.TabIndex = 24;
            this.gbEntityConnection.TabStop = false;
            this.gbEntityConnection.Text = "entity connection selector";
            this.gbEntityConnection.Visible = false;
            // 
            // lstContext
            // 
            this.lstContext.FormattingEnabled = true;
            this.lstContext.Location = new System.Drawing.Point(6, 21);
            this.lstContext.Name = "lstContext";
            this.lstContext.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstContext.Size = new System.Drawing.Size(267, 95);
            this.lstContext.TabIndex = 23;
            // 
            // btnAssociatedContext
            // 
            this.btnAssociatedContext.Location = new System.Drawing.Point(6, 122);
            this.btnAssociatedContext.Name = "btnAssociatedContext";
            this.btnAssociatedContext.Size = new System.Drawing.Size(126, 23);
            this.btnAssociatedContext.TabIndex = 21;
            this.btnAssociatedContext.Text = "associated context";
            this.btnAssociatedContext.UseVisualStyleBackColor = true;
            this.btnAssociatedContext.Click += new System.EventHandler(this.btnAssociatedContext_Click);
            // 
            // buttonView
            // 
            this.buttonView.Location = new System.Drawing.Point(198, 107);
            this.buttonView.Name = "buttonView";
            this.buttonView.Size = new System.Drawing.Size(100, 23);
            this.buttonView.TabIndex = 20;
            this.buttonView.Text = "Status";
            this.buttonView.UseVisualStyleBackColor = true;
            this.buttonView.Click += new System.EventHandler(this.buttonView_Click);
            // 
            // lbxPwd
            // 
            this.lbxPwd.FormattingEnabled = true;
            this.lbxPwd.Location = new System.Drawing.Point(172, 426);
            this.lbxPwd.Name = "lbxPwd";
            this.lbxPwd.Size = new System.Drawing.Size(75, 4);
            this.lbxPwd.TabIndex = 22;
            this.lbxPwd.Visible = false;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label6.Location = new System.Drawing.Point(196, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 19);
            this.label6.TabIndex = 19;
            this.label6.Text = "OdbcType";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCreateSystemTable
            // 
            this.btnCreateSystemTable.Location = new System.Drawing.Point(156, 394);
            this.btnCreateSystemTable.Name = "btnCreateSystemTable";
            this.btnCreateSystemTable.Size = new System.Drawing.Size(142, 25);
            this.btnCreateSystemTable.TabIndex = 12;
            this.btnCreateSystemTable.Text = "Create System Table";
            this.btnCreateSystemTable.UseVisualStyleBackColor = true;
            this.btnCreateSystemTable.Click += new System.EventHandler(this.btnCreateSystemTable_Click);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(20, 394);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(123, 25);
            this.btnTestConnection.TabIndex = 11;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(142, 363);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(64, 25);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(236, 363);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(62, 25);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbxOdbcType
            // 
            this.cbxOdbcType.BackColor = System.Drawing.SystemColors.Control;
            this.cbxOdbcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOdbcType.Enabled = false;
            this.cbxOdbcType.FormattingEnabled = true;
            this.cbxOdbcType.Items.AddRange(new object[] {
            "Informix",
            "FoxPro",
            "DB2"});
            this.cbxOdbcType.Location = new System.Drawing.Point(199, 81);
            this.cbxOdbcType.Name = "cbxOdbcType";
            this.cbxOdbcType.Size = new System.Drawing.Size(100, 21);
            this.cbxOdbcType.TabIndex = 18;
            // 
            // btnMod
            // 
            this.btnMod.Location = new System.Drawing.Point(81, 363);
            this.btnMod.Name = "btnMod";
            this.btnMod.Size = new System.Drawing.Size(55, 25);
            this.btnMod.TabIndex = 16;
            this.btnMod.Text = "Modify...";
            this.btnMod.Click += new System.EventHandler(this.btnMod_Click);
            // 
            // cbIsMaster
            // 
            this.cbIsMaster.AutoSize = true;
            this.cbIsMaster.Enabled = false;
            this.cbIsMaster.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbIsMaster.ForeColor = System.Drawing.Color.MidnightBlue;
            this.cbIsMaster.Location = new System.Drawing.Point(172, 177);
            this.cbIsMaster.Name = "cbIsMaster";
            this.cbIsMaster.Size = new System.Drawing.Size(127, 19);
            this.cbIsMaster.TabIndex = 10;
            this.cbIsMaster.Text = "Split System Table";
            this.cbIsMaster.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(20, 363);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(55, 25);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "Add...";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbxIsMaster
            // 
            this.lbxIsMaster.FormattingEnabled = true;
            this.lbxIsMaster.Location = new System.Drawing.Point(90, 426);
            this.lbxIsMaster.Name = "lbxIsMaster";
            this.lbxIsMaster.Size = new System.Drawing.Size(75, 4);
            this.lbxIsMaster.TabIndex = 21;
            this.lbxIsMaster.Visible = false;
            // 
            // txtTimeOut
            // 
            this.txtTimeOut.BackColor = System.Drawing.SystemColors.Control;
            this.txtTimeOut.Location = new System.Drawing.Point(20, 173);
            this.txtTimeOut.Name = "txtTimeOut";
            this.txtTimeOut.ReadOnly = true;
            this.txtTimeOut.Size = new System.Drawing.Size(141, 23);
            this.txtTimeOut.TabIndex = 9;
            // 
            // lbxTimeOut
            // 
            this.lbxTimeOut.FormattingEnabled = true;
            this.lbxTimeOut.Location = new System.Drawing.Point(5, 426);
            this.lbxTimeOut.Name = "lbxTimeOut";
            this.lbxTimeOut.Size = new System.Drawing.Size(75, 4);
            this.lbxTimeOut.TabIndex = 20;
            this.lbxTimeOut.Visible = false;
            // 
            // txtMaxCount
            // 
            this.txtMaxCount.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaxCount.Location = new System.Drawing.Point(20, 126);
            this.txtMaxCount.Name = "txtMaxCount";
            this.txtMaxCount.ReadOnly = true;
            this.txtMaxCount.Size = new System.Drawing.Size(143, 23);
            this.txtMaxCount.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label4.Location = new System.Drawing.Point(18, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "Time Out";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label3.Location = new System.Drawing.Point(18, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "Max Count";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(18, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "Database Type";
            // 
            // cbxDatabaseType
            // 
            this.cbxDatabaseType.BackColor = System.Drawing.SystemColors.Control;
            this.cbxDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDatabaseType.Enabled = false;
            this.cbxDatabaseType.FormattingEnabled = true;
            this.cbxDatabaseType.Items.AddRange(new object[] {
            "None",
            "MsSql",
            "OleDB",
            "Oracle",
            "ODBC",
            "MySql",
            "Informix",
            "Sybase"});
            this.cbxDatabaseType.Location = new System.Drawing.Point(20, 81);
            this.cbxDatabaseType.Name = "cbxDatabaseType";
            this.cbxDatabaseType.Size = new System.Drawing.Size(143, 21);
            this.cbxDatabaseType.TabIndex = 2;
            // 
            // edtDBString
            // 
            this.edtDBString.BackColor = System.Drawing.SystemColors.Control;
            this.edtDBString.Location = new System.Drawing.Point(20, 29);
            this.edtDBString.Name = "edtDBString";
            this.edtDBString.ReadOnly = true;
            this.edtDBString.Size = new System.Drawing.Size(279, 23);
            this.edtDBString.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(18, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database String";
            // 
            // lbxDB
            // 
            this.lbxDB.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbxDB.FormattingEnabled = true;
            this.lbxDB.ItemHeight = 15;
            this.lbxDB.Location = new System.Drawing.Point(8, 11);
            this.lbxDB.Name = "lbxDB";
            this.lbxDB.Size = new System.Drawing.Size(248, 409);
            this.lbxDB.TabIndex = 11;
            this.lbxDB.SelectedIndexChanged += new System.EventHandler(this.lbxDB_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbxSysDB);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(586, 459);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "System DataBase";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cbxSysDB
            // 
            this.cbxSysDB.FormattingEnabled = true;
            this.cbxSysDB.Location = new System.Drawing.Point(39, 60);
            this.cbxSysDB.Name = "cbxSysDB";
            this.cbxSysDB.Size = new System.Drawing.Size(139, 21);
            this.cbxSysDB.TabIndex = 6;
            this.cbxSysDB.DropDown += new System.EventHandler(this.cbxSysDB_DropDown);
            this.cbxSysDB.SelectedIndexChanged += new System.EventHandler(this.cbxSysDB_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label5.Location = new System.Drawing.Point(36, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 15);
            this.label5.TabIndex = 5;
            this.label5.Text = "System DataBase:";
            // 
            // lbxEncoding
            // 
            this.lbxEncoding.FormattingEnabled = true;
            this.lbxEncoding.Location = new System.Drawing.Point(252, 425);
            this.lbxEncoding.Name = "lbxEncoding";
            this.lbxEncoding.Size = new System.Drawing.Size(75, 4);
            this.lbxEncoding.TabIndex = 25;
            this.lbxEncoding.Visible = false;
            // 
            // frmDBMan
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(594, 486);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDBMan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataBase Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDBMan_FormClosing);
            this.Load += new System.EventHandler(this.frmDBMan_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbEntityConnection.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private string GetPwdString(string s)
        {
            string sRet = "";
            for (int i = 0; i < s.Length; i++)
            {
                sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
            }
            return sRet;
        }

        private void frmDBMan_Load(object sender, System.EventArgs e)
        {
#if WCF
            this.gbEntityConnection.Visible = true;
            List<string> lstDll = ServerHelper.GetServerPackages();
            if (lstDll.Count > 0)
            {
                this.lstContext.DataSource = ServerHelper.GetEntityContextTypes(lstDll);
                this.lstContext.DisplayMember = "ContextType";
                this.lstContext.ValueMember = "FileName";
            }
#endif
            string s = SystemFile.DBFile;
            DBXML = new XmlDocument();
            if (File.Exists(s))
            {
                DBXML.Load(s);
                XmlNode aNode = DBXML.DocumentElement.FirstChild;

                while (aNode != null)
                {
                    if (string.Compare(aNode.Name, "DATABASE", true) == 0)//IgnoreCase
                    {
                        XmlNode bNode = aNode.FirstChild;
                        while (bNode != null)
                        {
                            lbxDB.Items.Add(bNode.LocalName);
                            lbxDBString.Items.Add(bNode.Attributes["String"].InnerText);
                            // Add By Chenjian
                            lbxDbType.Items.Add(bNode.Attributes["Type"].InnerText);
                            if (bNode.Attributes["OdbcType"] != null)
                                lbxOdbcType.Items.Add(bNode.Attributes["OdbcType"].InnerText);
                            else
                                lbxOdbcType.Items.Add("-1");
                            lbxMaxCount.Items.Add(bNode.Attributes["MaxCount"].InnerText);
                            lbxTimeOut.Items.Add(bNode.Attributes["TimeOut"].InnerText);
                            lbxIsMaster.Items.Add(bNode.Attributes["Master"].InnerText);
                            bool encrypt = bNode.Attributes["Encrypt"] == null ? false : Convert.ToBoolean(bNode.Attributes["Encrypt"].Value);
                            lbxEncrypt.Items.Add(encrypt);
                            lbxEncoding.Items.Add(bNode.Attributes["Encoding"] == null ? "" : bNode.Attributes["Encoding"].Value);
                            if (bNode.Attributes["Password"] != null)
                                lbxPwd.Items.Add(GetPwdString(bNode.Attributes["Password"].InnerText));
                            else
                                lbxPwd.Items.Add("");
                            // End
                            bNode = bNode.NextSibling;
                        }
                        if (lbxDB.Items.Count > 0)
                            lbxDB.SelectedIndex = 0;
                    }
                    else if (string.Compare(aNode.Name, "SYSTEMDB", true) == 0)//IgnoreCase
                    {
                        cbxSysDB.Text = aNode.InnerText.Trim();
                    }
                    aNode = aNode.NextSibling;
                }
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            //modify by ccm only remote dataBase element instead of deleting file

            XmlDocument xml = new XmlDocument();

            if (File.Exists(SystemFile.DBFile))
            {
                xml.Load(SystemFile.DBFile);
            }
            XmlNode nodeInfolight = xml.SelectSingleNode("InfolightDB");
            if (nodeInfolight == null)
            {
                nodeInfolight = xml.CreateElement("InfolightDB");
                xml.AppendChild(nodeInfolight);
            }
            XmlNode nodeDataBase = nodeInfolight.SelectSingleNode("DataBase");
            if (nodeDataBase == null)
            {
                nodeDataBase = xml.CreateElement("DataBase");
                nodeInfolight.AppendChild(nodeDataBase);
            }
            nodeDataBase.RemoveAll();

            for (int i = 0; i < lbxDB.Items.Count; i++)
            {
                XmlNode node = xml.CreateElement(lbxDB.Items[i].ToString());
                XmlAttribute att = xml.CreateAttribute("String");
                att.Value = lbxDBString.Items[i].ToString();
                node.Attributes.Append(att);

                att = xml.CreateAttribute("Type");
                att.Value = lbxDbType.Items[i].ToString();
                node.Attributes.Append(att);

                att = xml.CreateAttribute("OdbcType");
                att.Value = lbxOdbcType.Items[i].ToString();
                node.Attributes.Append(att);

                att = xml.CreateAttribute("MaxCount");
                att.Value = lbxMaxCount.Items[i].ToString();
                node.Attributes.Append(att);

                att = xml.CreateAttribute("TimeOut");
                att.Value = lbxTimeOut.Items[i].ToString();
                node.Attributes.Append(att);

                att = xml.CreateAttribute("Master");
                att.Value = lbxIsMaster.Items[i].ToString();
                node.Attributes.Append(att);

                att = xml.CreateAttribute("Encrypt");
                att.Value = lbxEncrypt.Items[i].ToString();
                node.Attributes.Append(att);

                var encoding = lbxEncoding.Items[i].ToString();
                if (!string.IsNullOrEmpty(encoding))
                {
                    att = xml.CreateAttribute("Encoding");
                    att.Value = encoding;
                    node.Attributes.Append(att);
                }


                att = xml.CreateAttribute("Password");
                att.Value = GetPwdString(lbxPwd.Items[i].ToString());
                node.Attributes.Append(att);
                nodeDataBase.AppendChild(node);
            }

            XmlNode nodeSystemDB = nodeInfolight.SelectSingleNode("SystemDB");
            if (nodeSystemDB == null)
            {
                nodeSystemDB = xml.CreateElement("SystemDB");
                nodeInfolight.AppendChild(nodeSystemDB);
            }
            nodeSystemDB.InnerText = cbxSysDB.Text.Trim();
            xml.Save(SystemFile.DBFile);

            this.bHasSaved = true;
        }

        private void lbxDB_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lbxDB.SelectedIndex > -1)
            {
                edtDBString.Text = lbxDBString.Items[lbxDB.SelectedIndex].ToString();
                // Add By Chenjian
                cbxDatabaseType.SelectedIndex = int.Parse(lbxDbType.Items[lbxDB.SelectedIndex].ToString());
                cbxOdbcType.SelectedIndex = int.Parse(lbxOdbcType.Items[lbxDB.SelectedIndex].ToString());
                txtMaxCount.Text = lbxMaxCount.Items[lbxDB.SelectedIndex].ToString();
                txtTimeOut.Text = lbxTimeOut.Items[lbxDB.SelectedIndex].ToString();
                if (lbxIsMaster.Items[lbxDB.SelectedIndex].ToString() == "1")
                {
                    cbIsMaster.Checked = true;
                }
                else
                {
                    cbIsMaster.Checked = false;
                }
                //edtPwd.Text = lbxPwd.Items[lbxDB.SelectedIndex].ToString();
                // End
            }
        }

        private void SetIsMasterAt(int index)
        {
            for (int i = 0; i < this.lbxDB.Items.Count; ++i)
            {
                if (i == index)
                {
                    this.lbxIsMaster.Items[i] = "1";
                }
                /*else
                {
                    this.lbxIsMaster.Items[i] = "0";
                }*/
            }
        }

        private void ClearIsMasterAt(int index)
        {
            this.lbxIsMaster.Items[index] = "0";
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            frmAddDB aForm = new frmAddDB();
            // Add By Chenjian
            aForm.Text = "Add one EEP DataBase";
            aForm.DBType = "1";
            aForm.MaxCount = "20";
            aForm.TimeOut = "30";
            // End

            aForm.ShowDialog();
            try
            {
                if (aForm.DialogResult == DialogResult.OK)
                {

                    lbxDB.Items.Add(aForm.DBName);
                    lbxDBString.Items.Add(aForm.DBString);
                    // Add By Chenjian
                    lbxDbType.Items.Add(aForm.DBType);
                    lbxOdbcType.Items.Add(aForm.OdbcType);
                    lbxMaxCount.Items.Add(aForm.MaxCount);
                    lbxTimeOut.Items.Add(aForm.TimeOut);
                    lbxEncrypt.Items.Add(aForm.IsEncrypt);
                    lbxEncoding.Items.Add(aForm.Encoding);
                    if (aForm.IsMaster)
                    {
                        lbxIsMaster.Items.Add("1");
                        SetIsMasterAt(lbxDB.Items.Count - 1);
                    }
                    else
                    {
                        lbxIsMaster.Items.Add("0");
                        ClearIsMasterAt(lbxDB.Items.Count - 1);
                    }
                    lbxPwd.Items.Add(aForm.Pwd);
                    // End
                    this.bHasSaved = false;
                }
            }
            finally
            {
                aForm.Dispose();
            }
        }

        private void btnMod_Click(object sender, System.EventArgs e)
        {
            if (lbxDB.SelectedIndex == -1) return;
            string s = lbxDB.Items[lbxDB.SelectedIndex].ToString();
            string s1 = lbxDBString.Items[lbxDB.SelectedIndex].ToString();
            // Add By Chenjian
            string dbType = lbxDbType.Items[lbxDB.SelectedIndex].ToString();
            string odbcType = lbxOdbcType.Items[lbxDB.SelectedIndex].ToString();
            string maxCount = lbxMaxCount.Items[lbxDB.SelectedIndex].ToString();
            string timeOut = lbxTimeOut.Items[lbxDB.SelectedIndex].ToString();
            string isMaster = lbxIsMaster.Items[lbxDB.SelectedIndex].ToString();
            bool isEncrypt = (bool)lbxEncrypt.Items[lbxDB.SelectedIndex];
            string pwd = lbxPwd.Items[lbxDB.SelectedIndex].ToString();
            string encoding = lbxEncoding.Items[lbxDB.SelectedIndex].ToString();
            // End
            frmAddDB aForm = new frmAddDB(s, s1, dbType, true);
            aForm.DBName = s;
            aForm.DBString = s1;
            // Add By Chenjian
            aForm.Text = "Modify one EEP DataBase";
            aForm.DBType = dbType;
            aForm.OdbcType = odbcType;
            aForm.MaxCount = maxCount;
            aForm.TimeOut = timeOut;
            aForm.Pwd = pwd;
            aForm.Encoding = encoding;
            if (isMaster == "1")
            {
                aForm.IsMaster = true;
            }
            else
            {
                aForm.IsMaster = false;
            }
            aForm.IsEncrypt = isEncrypt;
            // End
            aForm.ShowDialog();
            try
            {
                if (aForm.DialogResult == DialogResult.OK)
                {
                    lbxDB.Items[lbxDB.SelectedIndex] = aForm.DBName;
                    lbxDBString.Items[lbxDB.SelectedIndex] = aForm.DBString;
                    // Add By Chenjian
                    lbxDbType.Items[lbxDB.SelectedIndex] = aForm.DBType;
                    lbxOdbcType.Items[lbxDB.SelectedIndex] = aForm.OdbcType;
                    lbxMaxCount.Items[lbxDB.SelectedIndex] = aForm.MaxCount;
                    lbxTimeOut.Items[lbxDB.SelectedIndex] = aForm.TimeOut;
                    lbxEncrypt.Items[lbxDB.SelectedIndex] = aForm.IsEncrypt;
                    lbxEncoding.Items[lbxDB.SelectedIndex] = aForm.Encoding;
                    if (aForm.IsMaster)
                    {
                        lbxIsMaster.Items.Add("1");

                        SetIsMasterAt(lbxDB.SelectedIndex);
                    }
                    else
                    {
                        lbxIsMaster.Items.Add("0");
                        ClearIsMasterAt(lbxDB.SelectedIndex);
                    }
                    lbxPwd.Items[lbxDB.SelectedIndex] = aForm.Pwd;
                    // End
                    this.bHasSaved = false;
                }
            }
            finally
            {
                aForm.Dispose();
            }
            // Add By Chenjian
            lbxDB_SelectedIndexChanged(null, null);
            // End
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            if (lbxDB.SelectedIndex == -1) return;
            if (MessageBox.Show("Are you sure delete it?", "Confirmation", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                // Add By Chenjian
                lbxDBString.Items.RemoveAt(lbxDB.SelectedIndex);
                lbxDbType.Items.RemoveAt(lbxDB.SelectedIndex);
                lbxOdbcType.Items.RemoveAt(lbxDB.SelectedIndex);
                lbxMaxCount.Items.RemoveAt(lbxDB.SelectedIndex);
                lbxTimeOut.Items.RemoveAt(lbxDB.SelectedIndex);
                lbxIsMaster.Items.RemoveAt(lbxDB.SelectedIndex);
                lbxEncrypt.Items.RemoveAt(lbxDB.SelectedIndex);
                lbxEncoding.Items.RemoveAt(lbxDB.SelectedIndex);
                // End
                lbxDB.Items.RemoveAt(lbxDB.SelectedIndex);
                this.bHasSaved = false;
            }
        }

        private bool ConnectToDatabase(IDbConnection connection)
        {
            try
            {
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private IDbConnection AllocateConnection(string DbString, ClientType ct)
        {
            IDbConnection conn = null;
            try
            {
                if (ClientType.ctMsSql == ct)
                {
                    conn = new SqlConnection(DbString);
                }
                else if (ClientType.ctOleDB == ct)
                {
                    conn = new OleDbConnection(DbString);
                }
                else if (ClientType.ctOracle == ct)
                {
                    conn = new OracleConnection(DbString);
                }
                else if (ClientType.ctODBC == ct)
                {
                    conn = new OdbcConnection(DbString);
                }
                else if (ClientType.ctMySql == ct)
                {
                    Assembly assembly = Assembly.LoadFrom("MySql.Data.dll");
                    conn = assembly.CreateInstance("MySql.Data.MySqlClient.MySqlConnection") as IDbConnection;
                    conn.ConnectionString = DbString;
                }
                else if (ClientType.ctInformix == ct)
                {
                    Assembly assembly = Assembly.LoadFrom("IBM.Data.Informix.dll");
                    conn = assembly.CreateInstance("IBM.Data.Informix.IfxConnection") as IDbConnection;
                    conn.ConnectionString = DbString;
                }
                else if (ClientType.ctSybase == ct)
                {
                    Assembly assembly = Assembly.LoadFrom("Sybase.Data.AseClient.dll");
                    conn = assembly.CreateInstance("Sybase.Data.AseClient.AseConnection") as IDbConnection;
                    conn.ConnectionString = DbString;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return conn;
        }

        private IDbCommand AllocateCommand(ClientType ct)
        {
            IDbCommand command = null;
            try
            {
                if (ClientType.ctMsSql == ct)
                {
                    command = new SqlCommand();
                }
                else if (ClientType.ctOleDB == ct)
                {
                    command = new OleDbCommand();
                }
                else if (ClientType.ctOracle == ct)
                {
                    command = new OracleCommand();
                }
                else if (ClientType.ctODBC == ct)
                {
                    command = new OdbcCommand();
                }
                else if (ClientType.ctMySql == ct)
                {
                    Assembly assembly = Assembly.LoadFrom("MySql.Data.dll");
                    command = assembly.CreateInstance("MySql.Data.MySqlClient.MySqlCommand") as IDbCommand;
                }
                else if (ClientType.ctInformix == ct)
                {
                    Assembly assembly = Assembly.LoadFrom("IBM.Data.Informix.dll");
                    command = assembly.CreateInstance("IBM.Data.Informix.IfxCommand") as IDbCommand;
                }
                else if (ClientType.ctSybase == ct)
                {
                    Assembly assembly = Assembly.LoadFrom("Sybase.Data.AseClient.dll");
                    command = assembly.CreateInstance("Sybase.Data.AseClient.AseCommand") as IDbCommand;
                }
            }
            catch
            {
            }

            return command;
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            int index = lbxDB.SelectedIndex;

            if (index != -1)
            {
                string DbString = string.Empty;
                bool encrypt = (bool)lbxEncrypt.Items[index];
                if (encrypt)
                {
                    DbString = ServerConfig.LoginObject.GetDBConnection((string)lbxDB.Items[index]);
                }
                else
                {
                    DbString = lbxDBString.Items[index].ToString();
                    if (!lbxPwd.Items[index].ToString().Trim().Equals(""))
                    {
                        if (DbString.Length > 0 && lbxPwd.Items[index].ToString().Trim() != String.Empty)
                        {
                            if (DbString[DbString.Length - 1] == ';')
                                DbString = DbString + "Password=" + lbxPwd.Items[index].ToString().Trim();
                            else
                                DbString = DbString + ";Password=" + lbxPwd.Items[index].ToString().Trim();
                        }
                    }
                    else
                        DbString = lbxDBString.Items[index].ToString();
                }

                ClientType ct = (ClientType)int.Parse(lbxDbType.Items[index].ToString());

                IDbConnection connection = AllocateConnection(DbString, ct);


                if (ConnectToDatabase(connection) == true)
                {
                    MessageBox.Show("Connect to database successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to connect to database.");
                }
            }
        }

        private void btnCreateSystemTable_Click(object sender, EventArgs e)
        {
            int index = lbxDB.SelectedIndex;

            if (index != -1)
            {
                string DbString = string.Empty;
                bool encrypt = (bool)lbxEncrypt.Items[index];
                if (encrypt)
                {
                    DbString = ServerConfig.LoginObject.GetDBConnection((string)lbxDB.Items[index]);
                }
                else
                {
                    DbString = lbxDBString.Items[index].ToString();
                    if (!lbxPwd.Items[index].ToString().Trim().Equals(""))
                    {
                        if (DbString.Length > 0 && lbxPwd.Items[index].ToString().Trim() != String.Empty)
                        {
                            if (DbString[DbString.Length - 1] == ';')
                                DbString = DbString + "Password=" + lbxPwd.Items[index].ToString().Trim();
                            else
                                DbString = DbString + ";Password=" + lbxPwd.Items[index].ToString().Trim();
                        }
                    }
                    else
                        DbString = lbxDBString.Items[index].ToString();
                }

                ClientType ct = (ClientType)int.Parse(lbxDbType.Items[index].ToString());

                IDbConnection connection = AllocateConnection(DbString, ct);
                if (ConnectToDatabase(connection) == true)
                {
                    // Create Tables
                    IDbCommand command = AllocateCommand(ct);
                    command.Connection = connection;

                    connection.Open();

                    if (ct == ClientType.ctMsSql)
                    {
                        CreateSqlServerSystemTable(command);
                    }
                    else if (ct == ClientType.ctODBC)
                    {
                        if (cbxOdbcType.Text == "DB2")
                            CreateDB2SystemTable(command);
                        else
                            CreateODBCSystemTable(command);
                    }
                    else if (ct == ClientType.ctOleDB)
                    {
                        CreateOleDbSystemTable(command);
                    }
                    else if (ct == ClientType.ctOracle)
                    {
                        CreateOracleSystemTable(command);
                    }
                    else if (ct == ClientType.ctMySql)
                    {
                        CreateMySqlSystemTable(command);
                    }
                    else if (ct == ClientType.ctInformix)
                    {
                        CreateInformixSystemTable(command);
                    }
                    else if (ct == ClientType.ctSybase)
                    {
                        CreateSybaseSystemTable(command);
                    }


                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Failed to connect to database.");
                }
            }
        }

        public void CreateODBCSystemTable(IDbCommand command)
        {
            frmCreateDB cdb = new frmCreateDB();
            cdb.ShowDialog();

            if (cdb.DialogResult == DialogResult.OK)
            {
                string result = "";

                if (cdb.x == radioType.simplified || cdb.x == radioType.typical)
                {
                    #region Simplifield
                    command.CommandText = "CREATE TABLE COLDEF"
                                                     + "("
                                                     + "TABLE_NAME nvarchar(20) NOT NULL, "
                                                     + "FIELD_NAME nvarchar(20) NOT NULL, "
                                                     + "SEQ decimal(12,0), "
                                                     + "FIELD_TYPE nvarchar(20), "
                                                     + "IS_KEY nvarchar(1) NOT NULL, "
                                                     + "FIELD_LENGTH decimal(12,0), "
                                                     + "CAPTION nvarchar(40), "
                                                     + "EDITMASK nvarchar(10), "
                                                     + "NEEDBOX nvarchar(13), "
                                                     + "CANREPORT nvarchar(1), "
                                                     + "EXT_MENUID nvarchar(20), "
                                                     + "FIELD_SCALE decimal(12,0), "
                                                     + "DD_NAME nvarchar(40), "
                                                     + "DEFAULT_VALUE nvarchar(100), "
                                                     + "CHECK_NULL nvarchar(1), "
                                                     + "QUERYMODE nvarchar(20), "
                                                     + "CAPTION1 nvarchar(40), "
                                                     + "CAPTION2 nvarchar(40), "
                                                     + "CAPTION3 nvarchar(40), "
                                                     + "CAPTION4 nvarchar(40), "
                                                     + "CAPTION5 nvarchar(40), "
                                                     + "CAPTION6 nvarchar(40), "
                                                     + "CAPTION7 nvarchar(40), "
                                                     + "CAPTION8 nvarchar(40) "
                                                     + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create COLDEF table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX TABLENAME ON COLDEF (TABLE_NAME,FIELD_NAME)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX TABLENAME on table COLDEF .\n\r";
                    }

                    command.CommandText = "CREATE TABLE SYSAUTONUM "
                                            + "("
                                            + "AUTOID VARCHAR(20) NOT NULL, "
                                            + "FIXED VARCHAR(20) NOT NULL, "
                                            + "CURRNUM decimal(10,0), "
                                            + "DESCRIPTION VARCHAR(50),"
                                            + "PRIMARY KEY (AUTOID, FIXED)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYSAUTONUM table.\n\r";
                    }
                    #endregion

                    #region Typical
                    if (cdb.x == radioType.typical)
                    {
                        //// Create GROUPFORMS
                        //command.CommandText = "CREATE TABLE GROUPFORMS ("
                        //    + "GROUPID nvarchar (20) NOT NULL ,"
                        //    + "PACKAGE_NAME nvarchar (50) NOT NULL ,"
                        //    + "FORM_NAME nvarchar (50) NULL ,"
                        //    + "PARENT_MENU nvarchar (50) NULL "
                        //    + ")";
                        //try
                        //{
                        //    command.ExecuteNonQuery();
                        //}
                        //catch
                        //{
                        //    result += "Can not create GROUPFORMS table.\n\r";
                        //}

                        // Create GROUPMENUS
                        command.CommandText = "CREATE TABLE GROUPMENUS ("
                            + "GROUPID varchar (20) NOT NULL ,"
                            + "MENUID nvarchar (30) NOT NULL ,"
                            + "PRIMARY KEY (GROUPID, MENUID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUS table.\n\r";
                        }

                        // Create USERMENUS
                        command.CommandText = "CREATE TABLE USERMENUS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "MENUID nvarchar (30) NOT NULL "
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUS table.\n\r";
                        }

                        // Create GROUPS
                        command.CommandText = "CREATE TABLE GROUPS ("
                            + "GROUPID varchar (20) NOT NULL ,"
                            + "GROUPNAME nvarchar (50),"
                            + "DESCRIPTION nvarchar (100),"
                            + "MSAD nvarchar (1),"
                            + "ISROLE char(1) ,"
                            + "PRIMARY KEY (GROUPID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('00', 'EveryOne', 'N')";
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('01', 'DEPARTMENT1', 'N')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPS table.\n\r";
                        }

                        // Create MENUITEMTYPE
                        command.CommandText = "CREATE TABLE MENUITEMTYPE ("
                            + "ITEMTYPE nvarchar (20) NOT NULL ,"
                            + "ITEMNAME nvarchar (20),"
                            + "DBALIAS nvarchar (50) "
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUITEMTYPE(ITEMTYPE, ITEMNAME) VALUES('SOLUTION1', 'DEFAULT SOLUTION')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUITEMTYPE table.\n\r";
                        }

                        // Create MENUTABLE
                        command.CommandText = "CREATE TABLE MENUTABLE ("
                            + "MENUID nvarchar (30) NOT NULL ,"
                            + "CAPTION nvarchar (50) NOT NULL ,"
                            + "PARENT nvarchar (20) ,"
                            + "PACKAGE nvarchar (60),"
                            + "MODULETYPE nvarchar (1),"
                            + "ITEMPARAM nvarchar (200),"
                            + "FORM nvarchar (32),"
                            + "ISSHOWMODAL nvarchar (1),"
                            + "ITEMTYPE nvarchar (20),"
                            + "SEQ_NO nvarchar (4),"
                            + "PACKAGEDATE DateTime year to FRACTION,"
                            + "IMAGE byte,"
                            + "OWNER nvarchar(20),"
                            + "ISSERVER nvarchar(1),"
                            + "VERSIONNO nvarchar(20),"
                            + "CHECKOUT nvarchar(20),"
                            + "CHECKOUTDATE datetime year to FRACTION,"
                            + "CAPTION0 nvarchar(50),"
                            + "CAPTION1 nvarchar(50),"
                            + "CAPTION2 nvarchar(50),"
                            + "CAPTION3 nvarchar(50),"
                            + "CAPTION4 nvarchar(50),"
                            + "CAPTION5 nvarchar(50),"
                            + "CAPTION6 nvarchar(50),"
                            + "CAPTION7 nvarchar(50),"
                            + "IMAGEURL nvarchar(100), "
                            + "PRIMARY KEY (MENUID)"
                            + ") ";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUTABLE(MENUID, CAPTION, ITEMTYPE, MODULETYPE) VALUES('0', 'ROOT', 'SOLUTION1', 'F')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLE table.\n\r";
                        }

                        // Create USERGROUPS
                        command.CommandText = "CREATE TABLE USERGROUPS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "GROUPID varchar (20) NOT NULL "
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERGROUPS(USERID, GROUPID) VALUES('001', '01')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERGROUPS table.\n\r";
                        }

                        // Create USERS
                        command.CommandText = "CREATE TABLE USERS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "USERNAME nvarchar (30),"
                            + "AGENT nvarchar (20),"
                            + "PWD nvarchar (10),"
                            + "CREATEDATE nvarchar (8),"
                            + "CREATER nvarchar (20),"
                            + "DESCRIPTION nvarchar (100),"
                            + "EMAIL nvarchar (40),"
                            + "LASTTIME nvarchar (8),"
                            + "AUTOLOGIN nvarchar (1),"
                            + "LASTDATE nvarchar (8),"
                            + "SIGNATURE nvarchar (30),"
                            + "MSAD nvarchar (1) "
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERS(USERID, USERNAME, PWD, MSAD,AUTOLOGIN) VALUES('001', 'TEST', '', 'N','S')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERS table.\n\r";
                        }

                        // Create MENUTABLELOG
                        command.CommandText = "CREATE TABLE MENUTABLELOG"
                            + "("
                            + "LOGID serial,"
                            + "MENUID nvarchar(30) not null,"
                            + "PACKAGE nvarchar(20) not null,"
                            + "PACKAGEDATE DATETIME year to FRACTION,"
                            + "LASTDATE DATETIME year to FRACTION,"
                            + "OWNER nvarchar(20),"
                            + "OLDVERSION byte,"
                            + "OLDDATE nvarchar(20), "
                            + "PRIMARY KEY (LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLELOG table.\n\r";
                        }

                        // Create MENUCHECKLOG
                        command.CommandText = "CREATE TABLE MENUCHECKLOG"
                            + "("
                            + "LOGID serial ,"
                            + "ITEMTYPE nvarchar(20) not null,"
                            + "PACKAGE nvarchar(50) not null,"
                            + "PACKAGEDATE DateTime year to FRACTION,"
                            + "FILETYPE nvarchar(10),"
                            + "FILENAME nvarchar(60),"
                            + "FILEDATE DateTime year to FRACTION,"
                            + "FILECONTENT byte, "
                            + "PRIMARY KEY (LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUCHECKLOG table.\n\r";
                        }

                        // Create SYSEEPLOG
                        command.CommandText = "CREATE TABLE SYSEEPLOG"
                            + "("
                            + "CONNID nvarchar(20) NOT NULL,"
                            + "LOGID serial NOT NULL,"
                            + "LOGSTYLE nvarchar(1) NOT NULL,"
                            + "LOGDATETIME DATETIME year to FRACTION NOT NULL,"
                            + "DOMAINID nvarchar(30),"
                            + "USERID varchar(20),"
                            + "LOGTYPE nvarchar(1),"
                            + "TITLE nvarchar(64),"
                            + "DESCRIPTION nvarchar(128),"
                            + "COMPUTERIP nvarchar(16),"
                            + "COMPUTERNAME nvarchar(64),"
                            + "EXECUTIONTIME integer"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSEEPLOG table.\n\r";
                        }

                        // Create SYSSQLLOG
                        command.CommandText = "CREATE TABLE SYSSQLLOG("
                             + "LOGSTYLE nvarchar(1),"
                             + "LOGDATETIME DateTime year to FRACTION,"
                             + "USERID varchar(20),"
                             + "DEVELOPERID varchar(20),"
                             + "DESCRIPTION text,"
                             + "SQLSENTENCE nvarchar(max)"
                             + ");";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSSQLLOG table.\n\r";
                        }

                        // Create SYSERRLOG
                        command.CommandText = "CREATE TABLE SYSERRLOG"
                            + "("
                            + "ERRID serial,"
                            + "USERID varchar(20), "
                            + "MODULENAME nvarchar(30),"
                            + "ERRMESSAGE nvarchar(255),"
                            + "ERRSTACK text,"
                            + "ERRDESCRIP nvarchar(255),"
                            + "ERRDATE DateTime year to FRACTION,"
                            + "ERRSCREEN byte,"
                            + "OWNER nvarchar(20),"
                            + "PROCESSDATE DateTime year to FRACTION,"
                            + "PROCDESCRIP nvarchar(255),"
                            + "STATUS nvarchar(2) ,"
                            + "PRIMARY KEY (ERRID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSERRLOG table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_LANGUAGE"
                            + "("
                            + "ID serial NOT NULL,"
                            + "IDENTIFICATION nvarchar(80),"
                            + "KEYS nvarchar(80),"
                            + "EN nvarchar(80),"
                            + "CHT nvarchar(80),"
                            + "CHS nvarchar(80),"
                            + "HK nvarchar(80),"
                            + "JA nvarchar(80),"
                            + "KO nvarchar(80),"
                            + "LAN1 nvarchar(80),"
                            + "LAN2 nvarchar(80) "
                            + ")";

                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_LANGUAGE table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_MESSENGER"
                            + "("
                            + "USERID varchar(20) NOT NULL,"
                            + "MESSAGE nvarchar(255),"
                            + "PARAS nvarchar(255),"
                            + "SENDTIME nvarchar(14),"
                            + "SENDERID nvarchar(20),"
                            + "RECTIME nvarchar(14),"
                            + "STATUS char(1)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_MESSENGER table.\n\r";
                        }

                        //CreateMenuTableControl
                        command.CommandText = "CREATE TABLE MENUTABLECONTROL"
                                                + "("
                                                + "MENUID varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "DESCRIPTION Varchar (50), "
                                                + "TYPE Varchar (20) ,"
                                                + "PRIMARY KEY (MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLECONTROL table.\n\r";
                        }

                        //CreateGroupMenuControl
                        command.CommandText = "CREATE TABLE GROUPMENUCONTROL"
                                                + "("
                                                + "GROUPID Varchar (20) NOT NULL, "
                                                + "MENUID Varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "TYPE Varchar (20), "
                                                + "ENABLED CHAR (1), "
                                                + "VISIBLE CHAR (1), "
                                                + "ALLOWADD CHAR (1), "
                                                + "ALLOWUPDATE CHAR (1), "
                                                + "ALLOWDELETE CHAR (1), "
                                                + "ALLOWPRINT CHAR (1), "
                                                + "PRIMARY KEY (GROUPID, MENUID, CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUCONTROL table.\n\r";
                        }

                        //CreateUserMenuControl
                        command.CommandText = "CREATE TABLE USERMENUCONTROL"
                                                + "("
                                                + "USERID Varchar (20) NOT NULL, "
                                                + "MENUID Varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "TYPE Varchar (20), "
                                                + "ENABLED CHAR (1), "
                                                + "VISIBLE CHAR (1), "
                                                + "ALLOWADD CHAR (1), "
                                                + "ALLOWUPDATE CHAR (1), "
                                                + "ALLOWDELETE CHAR (1), "
                                                + "ALLOWPRINT CHAR (1) "
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUCONTROL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL"
                                            + "("
                                            + "REFVAL_NO Varchar(100) Not NULL, "
                                            + "DESCRIPTION Varchar(250), "
                                            + "TABLE_NAME Varchar(100), "
                                            + "CAPTION Varchar(100), "
                                            + "DISPLAY_MEMBER Varchar(100), "
                                            + "SELECT_ALIAS Varchar(250), "
                                            + "SELECT_COMMAND Varchar(250), "
                                            + "VALUE_MEMBER Varchar(100) "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL_D1"
                                            + "("
                                            + "REFVAL_NO Varchar(30) Not NULL, "
                                            + "FIELD_NAME Varchar(30) Not NULL, "
                                            + "HEADER_TEXT Varchar(20), "
                                            + "WIDTH integer "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL_D1 table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE MENUFAVOR"
                                            + "("
                                            + "MENUID Varchar(30) Not NULL, "
                                            + "CAPTION Varchar(50) Not NULL, "
                                            + "USERID Varchar(20) not null, "
                                            + "ITEMTYPE Varchar(20), "
                                            + "GROUPNAME VarChar(20) "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUFAVOR table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_ANYQUERY"
                                            + "("
                                            + "QUERYID Varchar(20) Not NULL, "
                                            + "USERID Varchar(20) Not NULL, "
                                            + "TEMPLATEID Varchar(20), "
                                            + "TABLENAME Varchar(50), "
                                            + "LASTDATE DateTime year to FRACTION, "
                                            + "CONTENT text, "
                                            + "PRIMARY Key (QUERYID,USERID,TEMPLATEID)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_ANYQUERY table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REPORT"
                                           + "("
                                           + "REPORTID nVarchar(50) Not NULL, "
                                           + "FILENAME nVarchar(50) Not NULL, "
                                           + "REPORTNAME nVarchar(255), "
                                           + "DESCRIPTION nVarchar(255), "
                                           + "FILEPATH nVarchar(255), "
                                           + "OUTPUTMODE nVarchar(20), "
                                           + "HEADERREPEAT nVarchar(5), "
                                           + "HEADERFONT byte, "
                                           + "HEADERITEMS byte, "
                                           + "FOOTERFONT byte, "
                                           + "FOOTERITEMS byte, "
                                           + "FIELDFONT byte, "
                                           + "FIELDITEMS byte, "
                                           + "SETTING byte, "
                                           + "FORMAT byte, "
                                           + "PARAMETERS byte, "
                                           + "IMAGES byte, "
                                           + "MAILSETTING byte, "
                                           + "DATASOURCE_PROVIDER nVarchar(50),"
                                           + "DATASOURCES byte,"
                                           + "CLIENT_QUERY byte,"
                                           + "REPORT_TYPE nVarchar(1),"
                                           + "TEMPLATE_DESC nVarchar(50),"
                                           + "PRIMARY Key (REPORTID,FILENAME)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REPORT table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_PERSONAL"
                                           + "("
                                           + "FORMNAME NVARCHAR(60) NOT NULL,"
                                           + "COMPNAME NVARCHAR(30) NOT NULL,"
                                           + "USERID NVARCHAR(20) NOT NULL,"
                                           + "REMARK NVARCHAR(30),"
                                           + "PROPCONTENT TEXT,"
                                           + "CREATEDATE DATETIME year to FRACTION,"
                                           + "PRIMARY KEY (FORMNAME,COMPNAME,USERID)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_PERSONAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_EEP_USERS"
                                           + "("
                                           + "USERID NVARCHAR(50) NOT NULL,"
                                           + "USERNAME NVARCHAR(50) NULL,"
                                           + "COMPUTER NVARCHAR(50) NOT NULL,"
                                           + "LOGINTIME NVARCHAR(50),"
                                           + "LASTACTIVETIME NVARCHAR(50),"
                                           + "LOGINCOUNT integer,"
                                           + "PRIMARY KEY (USERID,COMPUTER)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_EEP_USERS table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE UserDevices"
                           + "("
                           + "UserID varchar(50) NULL,"
                           + "UUID varchar(50) NULL,"
                           + "Active varchar(1) NULL,"
                           + "CreateDate DATETIME year to FRACTION,"
                           + "LoginDate DATETIME year to FRACTION,"
                           + "ExpiryDate DATETIME year to FRACTION,"
                           + "RegID nvarchar(max),"
                           + "TokenID nvarchar(max)"
                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create UserDevices table.\n\r";
                        }
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP7m)
                {
                    #region EEP7m
                    command.CommandText = "Alter Table COLDEF add "
                                                      + "DEFAULT_VALUE NVARCHAR(100), "
                                                      + "CHECK_NULL NVARCHAR(1), "
                                                      + "QUERYMODE NVARCHAR(20), "
                                                      + "CAPTION1 NVARCHAR (40), "
                                                      + "CAPTION2 NVARCHAR (40), "
                                                      + "CAPTION3 NVARCHAR (40), "
                                                      + "CAPTION4 NVARCHAR (40), "
                                                      + "CAPTION5 NVARCHAR (40), "
                                                      + "CAPTION6 NVARCHAR (40), "
                                                      + "CAPTION7 NVARCHAR (40), "
                                                      + "CAPTION8 NVARCHAR (40)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPMENUS alter column MENUID nvarchar (30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table GROUPS add MSAD NVARCHAR (1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS alter column GROUPNAME nvarchar (50)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS alter column DESCRIPTION nvarchar (200)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table MENUTABLE add "
                                           + "MODULETYPE NVARCHAR(1), "
                                           + "PACKAGEDATE DATETIME year to FRACTION, "
                                           + "IMAGE byte, "
                                           + "OWNER NVARCHAR(20), "
                                           + "ISSERVER NVARCHAR(1), "
                                           + "VERSIONNO NVARCHAR(20), "
                                           + "CHECKOUT NVARCHAR(20), "
                                           + "CHECKOUTDATE DATETIME year to FRACTION, "
                                           + "CAPTION0 NVARCHAR(50), "
                                           + "CAPTION1 NVARCHAR(50), "
                                           + "CAPTION2 NVARCHAR(50), "
                                           + "CAPTION3 NVARCHAR(50), "
                                           + "CAPTION4 NVARCHAR(50), "
                                           + "CAPTION5 NVARCHAR(50), "
                                           + "CAPTION6 NVARCHAR(50), "
                                           + "CAPTION7 NVARCHAR(50), "
                                           + "IMAGEURL NVARCHAR(100)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE alter column MENUID nvarchar(30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE alter column ITEMTYPE nvarchar(20)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table USERMENUS alter column MENUID nvarchar(30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table USERS add "
                                           + "EMAIL NVARCHAR(40), "
                                           + "LASTTIME NVARCHAR(8), "
                                           + "AUTOLOGIN NVARCHAR(1), "
                                           + "LASTDATE NVARCHAR(8), "
                                           + "SIGNATURE NVARCHAR(30), "
                                           + "MSAD NVARCHAR(1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP2006m)
                {
                    #region EEP2006m
                    command.CommandText = "Alter Table MENUFAVOR ADD GROUPNAME NVARCHAR(20)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table SYS_TODOLIST  ADD  ATTACHMENTS NVARCHAR(255)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "Alter table SYS_TODOHIS  ADD  ATTACHMENTS NVARCHAR(255)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                //odbc未测试
                else if (cdb.x == radioType.WorkFlow)
                {
                    #region WorkFlow
                    //GROUPS
                    command.CommandText = "ALTER TABLE GROUPS ADD ISROLE CHAR(1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not add ISROLE column to GROUPS table.\n\r";
                    }
                    // SYS_ORG
                    command.CommandText = "CREATE TABLE SYS_ORG(" +
                                        "ORG_NO nvarchar(8) NOT NULL," +
                                        "ORG_DESC nvarchar(40) NOT NULL," +
                                        "ORG_KIND nvarchar(4) NOT NULL," +
                                        "UPPER_ORG nvarchar(8)," +
                                        "ORG_MAN nvarchar(20) NOT NULL," +
                                        "LEVEL_NO nvarchar(6) NOT NULL," +
                                        "ORG_TREE nvarchar(40)," +
                                        "END_ORG nvarchar(4)," +
                                        "ORG_FULLNAME nvarchar(254)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORG(ORG_NO,ORG_DESC,ORG_KIND,ORG_MAN,LEVEL_NO) Values ('1','總公司','0','001','9')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORG table.\n\r";
                    }

                    // SYS_ORGKIND
                    command.CommandText = "CREATE TABLE SYS_ORGKIND(" +
                                        "ORG_KIND nvarchar(4) NOT NULL," +
                                        "KIND_DESC nvarchar(40) NOT NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORGKIND(ORG_KIND,KIND_DESC) Values ('0','公司組織')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGKIND table.\n\r";
                    }

                    // SYS_ORGLEVEL
                    command.CommandText = "CREATE TABLE SYS_ORGLEVEL(" +
                                        "LEVEL_NO nvarchar(6) NOT NULL," +
                                        "LEVEL_DESC nvarchar(40) NOT NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('1','主任/課長/副理')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('2','經理')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('3','副總')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('9','總經理')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGLEVEL table.\n\r";
                    }

                    // SYS_ORGROLES
                    command.CommandText = "CREATE TABLE SYS_ORGROLES(" +
                                        "ORG_NO nvarchar(8) NOT NULL," +
                                        "ROLE_ID varchar(20) NOT NULL," +
                                        "ORG_KIND nvarchar(4)," +
                                        "PRIMARY KEY(ORG_NO,ROLE_ID)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGROLES table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ORGNO ON SYS_ORGROLES (ORG_NO, ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX ORGNO on table SYS_ORGROLES .\n\r";
                    }

                    // SYS_ROLES_AGENT
                    command.CommandText = "CREATE TABLE SYS_ROLES_AGENT(" +
                                        "ROLE_ID varchar(20) NOT NULL," +
                                        "AGENT nvarchar(20) NOT NULL," +
                                        "FLOW_DESC nvarchar(40) NOT NULL," +
                                        "START_DATE nvarchar(8) NOT NULL," +
                                        "START_TIME nvarchar(6)," +
                                        "END_DATE nvarchar(8) NOT NULL," +
                                        "END_TIME nvarchar(6)," +
                                        "PAR_AGENT nvarchar(4) NOT NULL," +
                                        "REMARK nvarchar(254)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ROLES_AGENT table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ROLEID ON SYS_ROLES_AGENT (ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX ROLEID on table SYS_ROLES_AGENT .\n\r";
                    }

                    // SYS_TODOHIS
                    command.CommandText = "CREATE TABLE SYS_TODOHIS(" +
                                        "LISTID nvarchar(40) NOT NULL," +
                                        "FLOW_ID nvarchar(40) NOT NULL," +
                                        "FLOW_DESC nvarchar(40)," +
                                        "ROLE_ID varchar(20) NOT NULL," +
                                        "S_ROLE_ID varchar(20) NOT NULL," +
                                        "S_STEP_ID nvarchar(20) NOT NULL," +
                                        "D_STEP_ID nvarchar(20) NOT NULL," +
                                        "S_STEP_DESC nvarchar(64)," +
                                        "S_USER_ID nvarchar(20) NOT NULL," +
                                        "USER_ID nvarchar(20) NOT NULL," +
                                        "USERNAME nvarchar(30)," +
                                        "FORM_NAME nvarchar(30)," +
                                        "WEBFORM_NAME nvarchar(50) NOT NULL," +
                                        "S_USERNAME nvarchar(30)," +
                                        "NAVIGATOR_MODE nvarchar(2) NOT NULL," +
                                        "FLNAVIGATOR_MODE nvarchar(2) NOT NULL," +
                                        "PARAMETERS nvarchar(254)," +
                                        "STATUS nvarchar(4)," +
                                        "PROC_TIME decimal(8, 2) NOT NULL," +
                                        "EXP_TIME decimal(8, 2) NOT NULL," +
                                        "TIME_UNIT nvarchar(4) NOT NULL," +
                                        "FLOWIMPORTANT varchar(1) NOT NULL," +
                                        "FLOWURGENT varchar(1) NOT NULL," +
                                        "FORM_TABLE nvarchar(30)," +
                                        "FORM_KEYS nvarchar(254)," +
                                        "FORM_PRESENTATION nvarchar(254)," +
                                        "REMARK nvarchar(254)," +
                                        "VERSION nvarchar(2)," +
                                        "VDSNAME nvarchar(40)," +
                                        "SENDBACKSTEP nvarchar(2)," +
                                        "LEVEL_NO nvarchar(6)," +
                                        "UPDATE_DATE nvarchar(10)," +
                                        "UPDATE_TIME nvarchar(8)," +
                                        "FORM_PRESENT_CT nvarchar(254)," +
                                        "ATTACHMENTS nvarchar(255)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOHIS table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOHIS (LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX LISTID on table SYS_TODOHIS.\n\r";
                    }

                    command.CommandText = "CREATE INDEX USERID ON SYS_TODOHIS (USER_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX USERID on table SYS_TODOHIS.\n\r";
                    }

                    // SYS_TODOLIST
                    command.CommandText = "CREATE TABLE SYS_TODOLIST(" +
                                        "LISTID nvarchar(40) NOT NULL," +
                                        "FLOW_ID nvarchar(40) NOT NULL," +
                                        "FLOW_DESC nvarchar(40)," +
                                        "APPLICANT nvarchar(20) NOT NULL," +
                                        "S_USER_ID nvarchar(20) NOT NULL," +
                                        "S_STEP_ID nvarchar(20) NOT NULL," +
                                        "S_STEP_DESC nvarchar(64)," +
                                        "D_STEP_ID nvarchar(20) NOT NULL," +
                                        "D_STEP_DESC nvarchar(64)," +
                                        "EXP_TIME decimal(8, 2) NOT NULL," +
                                        "URGENT_TIME decimal(8, 2) NOT NULL," +
                                        "TIME_UNIT nvarchar(4) NOT NULL," +
                                        "USERNAME nvarchar(30)," +
                                        "FORM_NAME nvarchar(30)," +
                                        "NAVIGATOR_MODE nvarchar(2) NOT NULL," +
                                        "FLNAVIGATOR_MODE nvarchar(2) NOT NULL," +
                                        "PARAMETERS nvarchar(254)," +
                                        "SENDTO_KIND nvarchar(4) NOT NULL," +
                                        "SENDTO_ID nvarchar(20) NOT NULL," +
                                        "FLOWIMPORTANT varchar(1) NOT NULL," +
                                        "FLOWURGENT nvarchar(1) NOT NULL," +
                                        "STATUS nvarchar(4)," +
                                        "FORM_TABLE nvarchar(30)," +
                                        "FORM_KEYS nvarchar(254)," +
                                        "FORM_PRESENTATION nvarchar(254)," +
                                        "FORM_PRESENT_CT nvarchar(254) NOT NULL," +
                                        "REMARK nvarchar(254)," +
                                        "PROVIDER_NAME nvarchar(254)," +
                                        "VERSION nvarchar(2)," +
                                        "EMAIL_ADD nvarchar(40)," +
                                        "EMAIL_STATUS varchar(1)," +
                                        "VDSNAME nvarchar(40)," +
                                        "SENDBACKSTEP nvarchar(2)," +
                                        "LEVEL_NO nvarchar(6)," +
                                        "WEBFORM_NAME nvarchar(50) NOT NULL," +
                                        "UPDATE_DATE nvarchar(10)," +
                                        "UPDATE_TIME nvarchar(8)," +
                                        "FLOWPATH nvarchar(100) NOT NULL," +
                                        "PLUSAPPROVE varchar(1) NOT NULL," +
                                        "PLUSROLES nvarchar(254) NOT NULL," +
                                        "MULTISTEPRETURN varchar(1)," +
                                        "SENDTO_NAME nvarchar(30)," +
                                        "ATTACHMENTS NVARCHAR(255)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOLIST table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOLIST (LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX LISTID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX SENDTOID ON SYS_TODOLIST (SENDTO_ID, SENDTO_KIND)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX SENDTOID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX FLOWDESC ON SYS_TODOLIST (FLOW_DESC)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX FLOWDESC on table SYS_TODOLIST.\n\r";
                    }

                    // SYS_FLDefinition
                    command.CommandText = "CREATE TABLE SYS_FLDEFINITION"
                                            + "("
                                            + "FLTYPEID nvarchar(50) NOT NULL, "
                                            + "FLTYPENAME nvarchar(200) NOT NULL, "
                                            + "FLDEFINITION text NOT NULL, "
                                            + "VERSION integer, "
                                            + "PRIMARY KEY(FLTYPEID) "
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLDEFINITION table.\n\r";
                    }

                    // SYS_FLInstanceState
                    command.CommandText = "CREATE TABLE SYS_FLINSTANCESTATE"
                                            + "("
                                            + "FLINSTANCEID nvarchar(50) NOT NULL, "
                                            + "STATE byte NOT NULL, "
                                            + "STATUS integer, "
                                            + "INFO nvarchar(200)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLINSTANCESTATE table.\n\r";
                    }

                    // Sys_ExtApprove
                    command.CommandText = "CREATE TABLE SYS_EXTAPPROVE"
                                            + "("
                                            + "APPROVEID nvarchar(50) NULL, "
                                            + "GROUPID nvarchar(50) NULL, "
                                            + "MINIMUM nvarchar(50) NULL, "
                                            + "MAXIMUM nvarchar(50) NULL,"
                                            + "ROLEID nvarchar(50) NULL"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_EXTAPPROVE table.\n\r";
                    }
                    #endregion
                }

                if (result != "")
                    MessageBox.Show(result);
                else if (cdb.x == radioType.typical)
                    MessageBox.Show("Create all System Tables successfully.");
                else if (cdb.x == radioType.simplified)
                    MessageBox.Show("Create table COLDEF table and table SYSAUTONUM successfully.");
                else if (cdb.x == radioType.EEP7m)
                    MessageBox.Show("Alter successfully.");
                else if (cdb.x == radioType.EEP2006m)
                    MessageBox.Show("EEP 2006 2.1.0.1(SP2) Migration success");
                else if (cdb.x == radioType.WorkFlow)
                    MessageBox.Show("Create WorkFlow Tables successfully.");
            }
        }

        public void CreateDB2SystemTable(IDbCommand command)
        {
            frmCreateDB cdb = new frmCreateDB();
            cdb.ShowDialog();

            if (cdb.DialogResult == DialogResult.OK)
            {
                string result = "";

                if (cdb.x == radioType.simplified || cdb.x == radioType.typical)
                {
                    #region Simplifield
                    command.CommandText = "CREATE TABLE COLDEF"
                                                     + "("
                                                     + "TABLE_NAME varchar(20) NOT NULL, "
                                                     + "FIELD_NAME varchar(20) NOT NULL, "
                                                     + "SEQ decimal(12,0), "
                                                     + "FIELD_TYPE varchar(20), "
                                                     + "IS_KEY varchar(1) NOT NULL, "
                                                     + "FIELD_LENGTH decimal(12,0), "
                                                     + "CAPTION varchar(40), "
                                                     + "EDITMASK varchar(10), "
                                                     + "NEEDBOX varchar(13), "
                                                     + "CANREPORT varchar(1), "
                                                     + "EXT_MENUID varchar(20), "
                                                     + "FIELD_SCALE decimal(12,0), "
                                                     + "DD_NAME varchar(40), "
                                                     + "DEFAULT_VALUE varchar(100), "
                                                     + "CHECK_NULL varchar(1), "
                                                     + "QUERYMODE varchar(20), "
                                                     + "CAPTION1 varchar(40), "
                                                     + "CAPTION2 varchar(40), "
                                                     + "CAPTION3 varchar(40), "
                                                     + "CAPTION4 varchar(40), "
                                                     + "CAPTION5 varchar(40), "
                                                     + "CAPTION6 varchar(40), "
                                                     + "CAPTION7 varchar(40), "
                                                     + "CAPTION8 varchar(40) "
                                                     + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create COLDEF table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX TABLENAME ON COLDEF (TABLE_NAME,FIELD_NAME)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX TABLENAME on table COLDEF .\n\r";
                    }

                    command.CommandText = "CREATE TABLE SYSAUTONUM "
                                            + "("
                                            + "AUTOID VARCHAR(20) NOT NULL, "
                                            + "FIXED VARCHAR(20) NOT NULL, "
                                            + "CURRNUM decimal(10,0), "
                                            + "DESCRIPTION VARCHAR(50),"
                                            + "PRIMARY KEY (AUTOID, FIXED)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYSAUTONUM table.\n\r";
                    }
                    #endregion

                    #region Typical
                    if (cdb.x == radioType.typical)
                    {
                        //// Create GROUPFORMS
                        //command.CommandText = "CREATE TABLE GROUPFORMS ("
                        //    + "GROUPID nvarchar (20) NOT NULL ,"
                        //    + "PACKAGE_NAME nvarchar (50) NOT NULL ,"
                        //    + "FORM_NAME nvarchar (50) NULL ,"
                        //    + "PARENT_MENU nvarchar (50) NULL "
                        //    + ")";
                        //try
                        //{
                        //    command.ExecuteNonQuery();
                        //}
                        //catch
                        //{
                        //    result += "Can not create GROUPFORMS table.\n\r";
                        //}

                        // Create GROUPMENUS
                        command.CommandText = "CREATE TABLE GROUPMENUS ("
                            + "GROUPID varchar (20) NOT NULL ,"
                            + "MENUID varchar (30) NOT NULL ,"
                            + "PRIMARY KEY (GROUPID, MENUID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUS table.\n\r";
                        }

                        // Create USERMENUS
                        command.CommandText = "CREATE TABLE USERMENUS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "MENUID varchar (30) NOT NULL "
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUS table.\n\r";
                        }

                        // Create GROUPS
                        command.CommandText = "CREATE TABLE GROUPS ("
                            + "GROUPID varchar (20) NOT NULL ,"
                            + "GROUPNAME varchar (50),"
                            + "DESCRIPTION varchar (100),"
                            + "MSAD varchar (1),"
                            + "ISROLE char(1) ,"
                            + "PRIMARY KEY (GROUPID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('00', 'EveryOne', 'N')";
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('01', 'DEPARTMENT1', 'N')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPS table.\n\r";
                        }

                        // Create MENUITEMTYPE
                        command.CommandText = "CREATE TABLE MENUITEMTYPE ("
                            + "ITEMTYPE varchar (20) NOT NULL ,"
                            + "ITEMNAME varchar (20),"
                            + "DBALIAS varchar (50) "
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUITEMTYPE(ITEMTYPE, ITEMNAME) VALUES('SOLUTION1', 'DEFAULT SOLUTION')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUITEMTYPE table.\n\r";
                        }

                        // Create MENUTABLE
                        command.CommandText = "CREATE TABLE MENUTABLE ("
                            + "MENUID varchar (30) NOT NULL ,"
                            + "CAPTION varchar (50) NOT NULL ,"
                            + "PARENT varchar (20) ,"
                            + "PACKAGE varchar (60),"
                            + "MODULETYPE varchar (1),"
                            + "ITEMPARAM varchar (200),"
                            + "FORM varchar (32),"
                            + "ISSHOWMODAL varchar (1),"
                            + "ITEMTYPE varchar (20),"
                            + "SEQ_NO varchar (4),"
                            + "PACKAGEDATE Date,"
                            + "IMAGE GRAPHIC,"
                            + "OWNER varchar(20),"
                            + "ISSERVER varchar(1),"
                            + "VERSIONNO varchar(20),"
                            + "CHECKOUT varchar(20),"
                            + "CHECKOUTDATE date,"
                            + "CAPTION0 varchar(50),"
                            + "CAPTION1 varchar(50),"
                            + "CAPTION2 varchar(50),"
                            + "CAPTION3 varchar(50),"
                            + "CAPTION4 varchar(50),"
                            + "CAPTION5 varchar(50),"
                            + "CAPTION6 varchar(50),"
                            + "CAPTION7 varchar(50),"
                            + "IMAGEURL varchar(100), "
                            + "PRIMARY KEY (MENUID)"
                            + ") ";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUTABLE(MENUID, CAPTION, ITEMTYPE, MODULETYPE) VALUES('0', 'ROOT', 'SOLUTION1', 'F')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLE table.\n\r";
                        }

                        // Create USERGROUPS
                        command.CommandText = "CREATE TABLE USERGROUPS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "GROUPID varchar (20) NOT NULL "
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERGROUPS(USERID, GROUPID) VALUES('001', '01')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERGROUPS table.\n\r";
                        }

                        // Create USERS
                        command.CommandText = "CREATE TABLE USERS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "USERNAME varchar (30),"
                            + "AGENT varchar (20),"
                            + "PWD varchar (10),"
                            + "CREATEDATE varchar (8),"
                            + "CREATER varchar (20),"
                            + "DESCRIPTION varchar (100),"
                            + "EMAIL varchar (40),"
                            + "LASTTIME varchar (8),"
                            + "AUTOLOGIN varchar (1),"
                            + "LASTDATE varchar (8),"
                            + "SIGNATURE varchar (30),"
                            + "MSAD varchar (1) "
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERS(USERID, USERNAME, PWD, MSAD,AUTOLOGIN) VALUES('001', 'TEST', '', 'N','S')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERS table.\n\r";
                        }

                        // Create MENUTABLELOG
                        command.CommandText = "CREATE TABLE MENUTABLELOG"
                            + "("
                            + "LOGID INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),"
                            + "MENUID varchar(30) not null,"
                            + "PACKAGE varchar(20) not null,"
                            + "PACKAGEDATE DATE,"
                            + "LASTDATE DATE,"
                            + "OWNER varchar(20),"
                            + "OLDVERSION BLOB,"
                            + "OLDDATE varchar(20), "
                            + "PRIMARY KEY (LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLELOG table.\n\r";
                        }

                        // Create MENUCHECKLOG
                        command.CommandText = "CREATE TABLE MENUCHECKLOG"
                            + "("
                            + "LOGID INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1) ,"
                            + "ITEMTYPE varchar(20) not null,"
                            + "PACKAGE varchar(50) not null,"
                            + "PACKAGEDATE Date,"
                            + "FILETYPE varchar(10),"
                            + "FILENAME varchar(60),"
                            + "FILEDATE Date,"
                            + "FILECONTENT BLOB, "
                            + "PRIMARY KEY (LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUCHECKLOG table.\n\r";
                        }

                        // Create SYSEEPLOG
                        command.CommandText = "CREATE TABLE SYSEEPLOG"
                            + "("
                            + "CONNID varchar(20) NOT NULL,"
                            + "LOGID INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),"
                            + "LOGSTYLE varchar(1) NOT NULL,"
                            + "LOGDATETIME DATE NOT NULL,"
                            + "DOMAINID varchar(30),"
                            + "USERID varchar(20),"
                            + "LOGTYPE varchar(1),"
                            + "TITLE varchar(64),"
                            + "DESCRIPTION varchar(128),"
                            + "COMPUTERIP varchar(16),"
                            + "COMPUTERNAME varchar(64),"
                            + "EXECUTIONTIME integer"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSEEPLOG table.\n\r";
                        }

                        // Create SYSSQLLOG
                        command.CommandText = "CREATE TABLE SYSSQLLOG("
                             + "LOGSTYLE nvarchar(1),"
                             + "LOGDATETIME Date,"
                             + "USERID varchar(20),"
                             + "DEVELOPERID varchar(20),"
                             + "DESCRIPTION CLOB,"
                             + "SQLSENTENCE nvarchar(max)"
                             + ");";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSSQLLOG table.\n\r";
                        }

                        // Create SYSERRLOG
                        command.CommandText = "CREATE TABLE SYSERRLOG"
                            + "("
                            + "ERRID INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),"
                            + "USERID varchar(20), "
                            + "MODULENAME varchar(30),"
                            + "ERRMESSAGE varchar(255),"
                            + "ERRSTACK CLOB,"
                            + "ERRDESCRIP varchar(255),"
                            + "ERRDATE Date,"
                            + "ERRSCREEN BLOB,"
                            + "OWNER varchar(20),"
                            + "PROCESSDATE Date,"
                            + "PROCDESCRIP varchar(255),"
                            + "STATUS varchar(2) ,"
                            + "PRIMARY KEY (ERRID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSERRLOG table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_LANGUAGE"
                            + "("
                            + "ID INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),"
                            + "IDENTIFICATION varchar(80),"
                            + "KEYS varchar(80),"
                            + "EN varchar(80),"
                            + "CHT varchar(80),"
                            + "CHS varchar(80),"
                            + "HK varchar(80),"
                            + "JA varchar(80),"
                            + "KO varchar(80),"
                            + "LAN1 varchar(80),"
                            + "LAN2 varchar(80) "
                            + ")";

                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_LANGUAGE table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_MESSENGER"
                            + "("
                            + "USERID varchar(20) NOT NULL,"
                            + "MESSAGE varchar(255),"
                            + "PARAS varchar(255),"
                            + "SENDTIME varchar(14),"
                            + "SENDERID varchar(20),"
                            + "RECTIME varchar(14),"
                            + "STATUS char(1)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_MESSENGER table.\n\r";
                        }

                        //CreateMenuTableControl
                        command.CommandText = "CREATE TABLE MENUTABLECONTROL"
                                                + "("
                                                + "MENUID varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "DESCRIPTION Varchar (50), "
                                                + "TYPE Varchar (20) ,"
                                                + "PRIMARY KEY (MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLECONTROL table.\n\r";
                        }

                        //CreateGroupMenuControl
                        command.CommandText = "CREATE TABLE GROUPMENUCONTROL"
                                                + "("
                                                + "GROUPID Varchar (20) NOT NULL, "
                                                + "MENUID Varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "TYPE Varchar (20), "
                                                + "ENABLED CHAR (1), "
                                                + "VISIBLE CHAR (1), "
                                                + "ALLOWADD CHAR (1), "
                                                + "ALLOWUPDATE CHAR (1), "
                                                + "ALLOWDELETE CHAR (1), "
                                                + "ALLOWPRINT CHAR (1), "
                                                + "PRIMARY KEY (GROUPID, MENUID, CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUCONTROL table.\n\r";
                        }

                        //CreateUserMenuControl
                        command.CommandText = "CREATE TABLE USERMENUCONTROL"
                                                + "("
                                                + "USERID Varchar (20) NOT NULL, "
                                                + "MENUID Varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "TYPE Varchar (20), "
                                                + "ENABLED CHAR (1), "
                                                + "VISIBLE CHAR (1), "
                                                + "ALLOWADD CHAR (1), "
                                                + "ALLOWUPDATE CHAR (1), "
                                                + "ALLOWDELETE CHAR (1), "
                                                + "ALLOWPRINT CHAR (1) "
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUCONTROL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL"
                                            + "("
                                            + "REFVAL_NO Varchar(100) Not NULL, "
                                            + "DESCRIPTION Varchar(250), "
                                            + "TABLE_NAME Varchar(100), "
                                            + "CAPTION Varchar(100), "
                                            + "DISPLAY_MEMBER Varchar(100), "
                                            + "SELECT_ALIAS Varchar(250), "
                                            + "SELECT_COMMAND Varchar(250), "
                                            + "VALUE_MEMBER Varchar(100) "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL_D1"
                                            + "("
                                            + "REFVAL_NO Varchar(30) Not NULL, "
                                            + "FIELD_NAME Varchar(30) Not NULL, "
                                            + "HEADER_TEXT Varchar(20), "
                                            + "WIDTH integer "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL_D1 table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE MENUFAVOR"
                                            + "("
                                            + "MENUID Varchar(30) Not NULL, "
                                            + "CAPTION Varchar(50) Not NULL, "
                                            + "USERID Varchar(20) not null, "
                                            + "ITEMTYPE Varchar(20), "
                                            + "GROUPNAME VarChar(20) "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUFAVOR table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_ANYQUERY"
                                            + "("
                                            + "QUERYID Varchar(20) Not NULL, "
                                            + "USERID Varchar(20) Not NULL, "
                                            + "TEMPLATEID Varchar(20), "
                                            + "TABLENAME Varchar(50), "
                                            + "LASTDATE Date, "
                                            + "CONTENT CLOB, "
                                            + "PRIMARY Key (QUERYID,USERID,TEMPLATEID)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_ANYQUERY table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REPORT"
                                           + "("
                                           + "REPORTID varchar(50) Not NULL, "
                                           + "FILENAME varchar(50) Not NULL, "
                                           + "REPORTNAME varchar(255), "
                                           + "DESCRIPTION varchar(255), "
                                           + "FILEPATH varchar(255), "
                                           + "OUTPUTMODE varchar(20), "
                                           + "HEADERREPEAT varchar(5), "
                                           + "HEADERFONT BLOB, "
                                           + "HEADERITEMS BLOB, "
                                           + "FOOTERFONT BLOB, "
                                           + "FOOTERITEMS BLOB, "
                                           + "FIELDFONT BLOB, "
                                           + "FIELDITEMS BLOB, "
                                           + "SETTING BLOB, "
                                           + "FORMAT BLOB, "
                                           + "PARAMETERS BLOB, "
                                           + "IMAGES BLOB, "
                                           + "MAILSETTING BLOB, "
                                           + "DATASOURCE_PROVIDER varchar(50),"
                                           + "DATASOURCES BLOB,"
                                           + "CLIENT_QUERY BLOB,"
                                           + "REPORT_TYPE varchar(1),"
                                           + "TEMPLATE_DESC varchar(50),"
                                           + "PRIMARY Key (REPORTID,FILENAME)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REPORT table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_PERSONAL"
                                           + "("
                                           + "FORMNAME varchar(60) NOT NULL,"
                                           + "COMPNAME varchar(30) NOT NULL,"
                                           + "USERID varchar(20) NOT NULL,"
                                           + "REMARK varchar(30),"
                                           + "PROPCONTENT CLOB,"
                                           + "CREATEDATE DATE,"
                                           + "PRIMARY KEY (FORMNAME,COMPNAME,USERID)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_PERSONAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_EEP_USERS"
                                           + "("
                                           + "USERID varchar(50) NOT NULL,"
                                           + "USERNAME varchar(50),"
                                           + "COMPUTER varchar(50) NOT NULL,"
                                           + "LOGINTIME varchar(50),"
                                           + "LASTACTIVETIME varchar(50),"
                                           + "LOGINCOUNT integer,"
                                           + "PRIMARY KEY (USERID,COMPUTER)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_EEP_USERS table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE UserDevices"
                                           + "("
                                           + "UserID varchar(50) NULL,"
                                           + "UUID varchar(50) NULL,"
                                           + "Active varchar(1) NULL,"
                                           + "CreateDate date,"
                                           + "LoginDate date,"
                                           + "ExpiryDate date,"
                                           + "RegID nvarchar(255),"
                                           + "TokenID nvarchar(255)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create UserDevices table.\n\r";
                        }
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP7m)
                {
                    #region EEP7m
                    command.CommandText = "Alter Table COLDEF add "
                                                      + "DEFAULT_VALUE varchar(100), "
                                                      + "CHECK_NULL varchar(1), "
                                                      + "QUERYMODE varchar(20), "
                                                      + "CAPTION1 varchar (40), "
                                                      + "CAPTION2 varchar (40), "
                                                      + "CAPTION3 varchar (40), "
                                                      + "CAPTION4 varchar (40), "
                                                      + "CAPTION5 varchar (40), "
                                                      + "CAPTION6 varchar (40), "
                                                      + "CAPTION7 varchar (40), "
                                                      + "CAPTION8 varchar (40)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPMENUS alter column MENUID varchar (30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table GROUPS add MSAD varchar (1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS alter column GROUPNAME varchar (50)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS alter column DESCRIPTION varchar (200)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table MENUTABLE add "
                                           + "MODULETYPE varchar(1), "
                                           + "PACKAGEDATE DATE, "
                                           + "IMAGE BLOB, "
                                           + "OWNER varchar(20), "
                                           + "ISSERVER varchar(1), "
                                           + "VERSIONNO varchar(20), "
                                           + "CHECKOUT varchar(20), "
                                           + "CHECKOUTDATE DATE, "
                                           + "CAPTION0 varchar(50), "
                                           + "CAPTION1 varchar(50), "
                                           + "CAPTION2 varchar(50), "
                                           + "CAPTION3 varchar(50), "
                                           + "CAPTION4 varchar(50), "
                                           + "CAPTION5 varchar(50), "
                                           + "CAPTION6 varchar(50), "
                                           + "CAPTION7 varchar(50), "
                                           + "IMAGEURL varchar(100)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE alter column MENUID varchar(30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE alter column ITEMTYPE varchar(20)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table USERMENUS alter column MENUID varchar(30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table USERS add "
                                           + "EMAIL varchar(40), "
                                           + "LASTTIME varchar(8), "
                                           + "AUTOLOGIN varchar(1), "
                                           + "LASTDATE varchar(8), "
                                           + "SIGNATURE varchar(30), "
                                           + "MSAD varchar(1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP2006m)
                {
                    #region EEP2006m
                    command.CommandText = "Alter Table MENUFAVOR ADD GROUPNAME varchar(20)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table SYS_TODOLIST  ADD  ATTACHMENTS varchar(255)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "Alter table SYS_TODOHIS  ADD  ATTACHMENTS varchar(255)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                //odbc未测试
                else if (cdb.x == radioType.WorkFlow)
                {
                    #region WorkFlow
                    //GROUPS
                    command.CommandText = "ALTER TABLE GROUPS ADD ISROLE CHAR(1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not add ISROLE column to GROUPS table.\n\r";
                    }
                    // SYS_ORG
                    command.CommandText = "CREATE TABLE SYS_ORG(" +
                                        "ORG_NO varchar(8) NOT NULL," +
                                        "ORG_DESC varchar(40) NOT NULL," +
                                        "ORG_KIND varchar(4) NOT NULL," +
                                        "UPPER_ORG varchar(8)," +
                                        "ORG_MAN varchar(20) NOT NULL," +
                                        "LEVEL_NO varchar(6) NOT NULL," +
                                        "ORG_TREE varchar(40)," +
                                        "END_ORG varchar(4)," +
                                        "ORG_FULLNAME varchar(254)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORG(ORG_NO,ORG_DESC,ORG_KIND,ORG_MAN,LEVEL_NO) Values ('1','總公司','0','001','9')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORG table.\n\r";
                    }

                    // SYS_ORGKIND
                    command.CommandText = "CREATE TABLE SYS_ORGKIND(" +
                                        "ORG_KIND varchar(4) NOT NULL," +
                                        "KIND_DESC varchar(40) NOT NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORGKIND(ORG_KIND,KIND_DESC) Values ('0','公司組織')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGKIND table.\n\r";
                    }

                    // SYS_ORGLEVEL
                    command.CommandText = "CREATE TABLE SYS_ORGLEVEL(" +
                                        "LEVEL_NO varchar(6) NOT NULL," +
                                        "LEVEL_DESC varchar(40) NOT NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('1','主任/課長/副理')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('2','經理')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('3','副總')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('9','總經理')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGLEVEL table.\n\r";
                    }

                    // SYS_ORGROLES
                    command.CommandText = "CREATE TABLE SYS_ORGROLES(" +
                                        "ORG_NO varchar(8) NOT NULL," +
                                        "ROLE_ID varchar(20) NOT NULL," +
                                        "ORG_KIND varchar(4)," +
                                        "PRIMARY KEY(ORG_NO,ROLE_ID)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGROLES table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ORGNO ON SYS_ORGROLES (ORG_NO, ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX ORGNO on table SYS_ORGROLES .\n\r";
                    }

                    // SYS_ROLES_AGENT
                    command.CommandText = "CREATE TABLE SYS_ROLES_AGENT(" +
                                        "ROLE_ID varchar(20) NOT NULL," +
                                        "AGENT varchar(20) NOT NULL," +
                                        "FLOW_DESC varchar(40) NOT NULL," +
                                        "START_DATE varchar(8) NOT NULL," +
                                        "START_TIME varchar(6)," +
                                        "END_DATE varchar(8) NOT NULL," +
                                        "END_TIME varchar(6)," +
                                        "PAR_AGENT varchar(4) NOT NULL," +
                                        "REMARK varchar(254)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ROLES_AGENT table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ROLEID ON SYS_ROLES_AGENT (ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX ROLEID on table SYS_ROLES_AGENT .\n\r";
                    }

                    // SYS_TODOHIS
                    command.CommandText = "CREATE TABLE SYS_TODOHIS(" +
                                        "LISTID varchar(40) NOT NULL," +
                                        "FLOW_ID varchar(40) NOT NULL," +
                                        "FLOW_DESC varchar(40)," +
                                        "ROLE_ID varchar(20) NOT NULL," +
                                        "S_ROLE_ID varchar(20) NOT NULL," +
                                        "S_STEP_ID varchar(20) NOT NULL," +
                                        "D_STEP_ID varchar(20) NOT NULL," +
                                        "S_STEP_DESC varchar(64)," +
                                        "S_USER_ID varchar(20) NOT NULL," +
                                        "USER_ID varchar(20) NOT NULL," +
                                        "USERNAME varchar(30)," +
                                        "FORM_NAME varchar(30)," +
                                        "WEBFORM_NAME varchar(50) NOT NULL," +
                                        "S_USERNAME varchar(30)," +
                                        "NAVIGATOR_MODE varchar(2) NOT NULL," +
                                        "FLNAVIGATOR_MODE varchar(2) NOT NULL," +
                                        "PARAMETERS varchar(254)," +
                                        "STATUS varchar(4)," +
                                        "PROC_TIME decimal(8, 2) NOT NULL," +
                                        "EXP_TIME decimal(8, 2) NOT NULL," +
                                        "TIME_UNIT varchar(4) NOT NULL," +
                                        "FLOWIMPORTANT varchar(1) NOT NULL," +
                                        "FLOWURGENT varchar(1) NOT NULL," +
                                        "FORM_TABLE varchar(30)," +
                                        "FORM_KEYS varchar(254)," +
                                        "FORM_PRESENTATION varchar(254)," +
                                        "REMARK varchar(254)," +
                                        "VERSION varchar(2)," +
                                        "VDSNAME varchar(40)," +
                                        "SENDBACKSTEP varchar(2)," +
                                        "LEVEL_NO varchar(6)," +
                                        "UPDATE_DATE varchar(10)," +
                                        "UPDATE_TIME varchar(8)," +
                                        "FORM_PRESENT_CT varchar(254)," +
                                        "ATTACHMENTS varchar(255)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOHIS table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOHIS (LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX LISTID on table SYS_TODOHIS.\n\r";
                    }

                    command.CommandText = "CREATE INDEX USERID ON SYS_TODOHIS (USER_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX USERID on table SYS_TODOHIS.\n\r";
                    }

                    // SYS_TODOLIST
                    command.CommandText = "CREATE TABLE SYS_TODOLIST(" +
                                        "LISTID varchar(40) NOT NULL," +
                                        "FLOW_ID varchar(40) NOT NULL," +
                                        "FLOW_DESC varchar(40)," +
                                        "APPLICANT varchar(20) NOT NULL," +
                                        "S_USER_ID varchar(20) NOT NULL," +
                                        "S_STEP_ID varchar(20) NOT NULL," +
                                        "S_STEP_DESC varchar(64)," +
                                        "D_STEP_ID varchar(20) NOT NULL," +
                                        "D_STEP_DESC varchar(64)," +
                                        "EXP_TIME decimal(8, 2) NOT NULL," +
                                        "URGENT_TIME decimal(8, 2) NOT NULL," +
                                        "TIME_UNIT varchar(4) NOT NULL," +
                                        "USERNAME varchar(30)," +
                                        "FORM_NAME varchar(30)," +
                                        "NAVIGATOR_MODE varchar(2) NOT NULL," +
                                        "FLNAVIGATOR_MODE varchar(2) NOT NULL," +
                                        "PARAMETERS varchar(254)," +
                                        "SENDTO_KIND varchar(4) NOT NULL," +
                                        "SENDTO_ID varchar(20) NOT NULL," +
                                        "FLOWIMPORTANT varchar(1) NOT NULL," +
                                        "FLOWURGENT varchar(1) NOT NULL," +
                                        "STATUS varchar(4)," +
                                        "FORM_TABLE varchar(30)," +
                                        "FORM_KEYS varchar(254)," +
                                        "FORM_PRESENTATION varchar(254)," +
                                        "FORM_PRESENT_CT varchar(254) NOT NULL," +
                                        "REMARK varchar(254)," +
                                        "PROVIDER_NAME varchar(254)," +
                                        "VERSION varchar(2)," +
                                        "EMAIL_ADD varchar(40)," +
                                        "EMAIL_STATUS varchar(1)," +
                                        "VDSNAME varchar(40)," +
                                        "SENDBACKSTEP varchar(2)," +
                                        "LEVEL_NO varchar(6)," +
                                        "WEBFORM_NAME varchar(50) NOT NULL," +
                                        "UPDATE_DATE varchar(10)," +
                                        "UPDATE_TIME varchar(8)," +
                                        "FLOWPATH varchar(100) NOT NULL," +
                                        "PLUSAPPROVE varchar(1) NOT NULL," +
                                        "PLUSROLES varchar(254) NOT NULL," +
                                        "MULTISTEPRETURN varchar(1)," +
                                        "SENDTO_NAME varchar(30)," +
                                        "ATTACHMENTS varchar(255)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOLIST table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOLIST (LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX LISTID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX SENDTOID ON SYS_TODOLIST (SENDTO_ID, SENDTO_KIND)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX SENDTOID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX FLOWDESC ON SYS_TODOLIST (FLOW_DESC)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX FLOWDESC on table SYS_TODOLIST.\n\r";
                    }

                    // SYS_FLDefinition
                    command.CommandText = "CREATE TABLE SYS_FLDEFINITION"
                                            + "("
                                            + "FLTYPEID varchar(50) NOT NULL, "
                                            + "FLTYPENAME varchar(200) NOT NULL, "
                                            + "FLDEFINITION CLOB NOT NULL, "
                                            + "VERSION integer, "
                                            + "PRIMARY KEY(FLTYPEID) "
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLDEFINITION table.\n\r";
                    }

                    // SYS_FLInstanceState
                    command.CommandText = "CREATE TABLE SYS_FLINSTANCESTATE"
                                            + "("
                                            + "FLINSTANCEID varchar(50) NOT NULL, "
                                            + "STATE BLOB NOT NULL, "
                                            + "STATUS integer, "
                                            + "INFO varchar(200)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLINSTANCESTATE table.\n\r";
                    }

                    // Sys_ExtApprove
                    command.CommandText = "CREATE TABLE SYS_EXTAPPROVE"
                                            + "("
                                            + "APPROVEID varchavarcharULL, "
                                            + "GROUPID varchavarcharULL, "
                                            + "MINIMUM varchavarcharULL, "
                                            + "MAXIMUM varchavarcharULL,"
                                            + "ROLEID varchavarcharULL"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_EXTAPPROVE table.\n\r";
                    }
                    #endregion
                }

                if (result != "")
                    MessageBox.Show(result);
                else if (cdb.x == radioType.typical)
                    MessageBox.Show("Create all System Tables successfully.");
                else if (cdb.x == radioType.simplified)
                    MessageBox.Show("Create table COLDEF table and table SYSAUTONUM successfully.");
                else if (cdb.x == radioType.EEP7m)
                    MessageBox.Show("Alter successfully.");
                else if (cdb.x == radioType.EEP2006m)
                    MessageBox.Show("EEP 2006 2.1.0.1(SP2) Migration success");
                else if (cdb.x == radioType.WorkFlow)
                    MessageBox.Show("Create WorkFlow Tables successfully.");
            }
        }

        public void CreateInformixSystemTable(IDbCommand command)
        {
            frmCreateDB cdb = new frmCreateDB();
            cdb.ShowDialog();

            if (cdb.DialogResult == DialogResult.OK)
            {
                string result = "";

                if (cdb.x == radioType.simplified || cdb.x == radioType.typical)
                {
                    #region Simplifield
                    command.CommandText = "CREATE TABLE COLDEF"
                                                     + "("
                                                     + "TABLE_NAME nvarchar(20) NOT NULL, "
                                                     + "FIELD_NAME nvarchar(20) NOT NULL, "
                                                     + "SEQ decimal(12,0), "
                                                     + "FIELD_TYPE nvarchar(20), "
                                                     + "IS_KEY nvarchar(1) NOT NULL, "
                                                     + "FIELD_LENGTH decimal(12,0), "
                                                     + "CAPTION nvarchar(40), "
                                                     + "EDITMASK nvarchar(10), "
                                                     + "NEEDBOX nvarchar(13), "
                                                     + "CANREPORT nvarchar(1), "
                                                     + "EXT_MENUID nvarchar(20), "
                                                     + "FIELD_SCALE decimal(12,0), "
                                                     + "DD_NAME nvarchar(40), "
                                                     + "DEFAULT_VALUE nvarchar(100), "
                                                     + "CHECK_NULL nvarchar(1), "
                                                     + "QUERYMODE nvarchar(20), "
                                                     + "CAPTION1 nvarchar(40), "
                                                     + "CAPTION2 nvarchar(40), "
                                                     + "CAPTION3 nvarchar(40), "
                                                     + "CAPTION4 nvarchar(40), "
                                                     + "CAPTION5 nvarchar(40), "
                                                     + "CAPTION6 nvarchar(40), "
                                                     + "CAPTION7 nvarchar(40), "
                                                     + "CAPTION8 nvarchar(40) "
                                                     + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create COLDEF table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX TABLENAME ON COLDEF (TABLE_NAME,FIELD_NAME)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX TABLENAME on table COLDEF .\n\r";
                    }

                    command.CommandText = "CREATE TABLE SYSAUTONUM "
                                            + "("
                                            + "AUTOID VARCHAR(20) NOT NULL, "
                                            + "FIXED VARCHAR(20) NOT NULL, "
                                            + "CURRNUM decimal(10,0), "
                                            + "DESCRIPTION VARCHAR(50),"
                                            + "PRIMARY KEY (AUTOID, FIXED)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYSAUTONUM table.\n\r";
                    }
                    #endregion

                    #region Typical
                    if (cdb.x == radioType.typical)
                    {
                        //// Create GROUPFORMS
                        //command.CommandText = "CREATE TABLE GROUPFORMS ("
                        //    + "GROUPID nvarchar (20) NOT NULL ,"
                        //    + "PACKAGE_NAME nvarchar (50) NOT NULL ,"
                        //    + "FORM_NAME nvarchar (50) NULL ,"
                        //    + "PARENT_MENU nvarchar (50) NULL "
                        //    + ")";
                        //try
                        //{
                        //    command.ExecuteNonQuery();
                        //}
                        //catch
                        //{
                        //    result += "Can not create GROUPFORMS table.\n\r";
                        //}

                        // Create GROUPMENUS
                        command.CommandText = "CREATE TABLE GROUPMENUS ("
                            + "GROUPID varchar (20) NOT NULL ,"
                            + "MENUID nvarchar (30) NOT NULL ,"
                            + "PRIMARY KEY (GROUPID,MENUID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUS table.\n\r";
                        }

                        // Create USERMENUS
                        command.CommandText = "CREATE TABLE USERMENUS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "MENUID nvarchar (30) NOT NULL "
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUS table.\n\r";
                        }

                        // Create GROUPS
                        command.CommandText = "CREATE TABLE GROUPS ("
                            + "GROUPID varchar (20) NOT NULL ,"
                            + "GROUPNAME nvarchar (50),"
                            + "DESCRIPTION nvarchar (100),"
                            + "MSAD nvarchar (1),"
                            + "ISROLE char(1) ,"
                            + "PRIMARY KEY (GROUPID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('00', 'EveryOne', 'N')";
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('01', 'DEPARTMENT1', 'N')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPS table.\n\r";
                        }

                        // Create MENUITEMTYPE
                        command.CommandText = "CREATE TABLE MENUITEMTYPE ("
                            + "ITEMTYPE nvarchar (20) NOT NULL ,"
                            + "ITEMNAME nvarchar (20), "
                            + "DBALIAS nvarchar (50) "
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUITEMTYPE(ITEMTYPE, ITEMNAME) VALUES('SOLUTION1', 'DEFAULT SOLUTION')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUITEMTYPE table.\n\r";
                        }

                        // Create MENUTABLE
                        command.CommandText = "CREATE TABLE MENUTABLE ("
                            + "MENUID nvarchar (30) NOT NULL ,"
                            + "CAPTION nvarchar (50) NOT NULL ,"
                            + "PARENT nvarchar (20) ,"
                            + "PACKAGE nvarchar (60),"
                            + "MODULETYPE nvarchar (1),"
                            + "ITEMPARAM nvarchar (200),"
                            + "FORM nvarchar (32),"
                            + "ISSHOWMODAL nvarchar (1),"
                            + "ITEMTYPE nvarchar (20),"
                            + "SEQ_NO nvarchar (4),"
                            + "PACKAGEDATE DateTime year to FRACTION,"
                            + "IMAGE byte,"
                            + "OWNER nvarchar(20),"
                            + "ISSERVER nvarchar(1),"
                            + "VERSIONNO nvarchar(20),"
                            + "CHECKOUT nvarchar(20),"
                            + "CHECKOUTDATE datetime year to FRACTION,"
                            + "CAPTION0 nvarchar(50),"
                            + "CAPTION1 nvarchar(50),"
                            + "CAPTION2 nvarchar(50),"
                            + "CAPTION3 nvarchar(50),"
                            + "CAPTION4 nvarchar(50),"
                            + "CAPTION5 nvarchar(50),"
                            + "CAPTION6 nvarchar(50),"
                            + "CAPTION7 nvarchar(50),"
                            + "IMAGEURL nvarchar(100),"
                            + "PRIMARY KEY (MENUID)"
                            + ") ";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUTABLE(MENUID, CAPTION, ITEMTYPE, MODULETYPE) VALUES('0', 'ROOT', 'SOLUTION1', 'F')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLE table.\n\r";
                        }

                        // Create USERGROUPS
                        command.CommandText = "CREATE TABLE USERGROUPS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "GROUPID varchar (20) NOT NULL "
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERGROUPS(USERID, GROUPID) VALUES('001', '01')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERGROUPS table.\n\r";
                        }

                        // Create USERS
                        command.CommandText = "CREATE TABLE USERS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "USERNAME nvarchar (30),"
                            + "AGENT nvarchar (20),"
                            + "PWD nvarchar (10),"
                            + "CREATEDATE nvarchar (8),"
                            + "CREATER nvarchar (20),"
                            + "DESCRIPTION nvarchar (100),"
                            + "EMAIL nvarchar (40),"
                            + "LASTTIME nvarchar (8),"
                            + "AUTOLOGIN nvarchar (1),"
                            + "LASTDATE nvarchar (8),"
                            + "SIGNATURE nvarchar (30),"
                            + "MSAD nvarchar (1) "
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERS(USERID, USERNAME, PWD, MSAD,AUTOLOGIN) VALUES('001', 'TEST', '', 'N','S')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERS table.\n\r";
                        }

                        // Create MENUTABLELOG
                        command.CommandText = "CREATE TABLE MENUTABLELOG"
                            + "("
                            + "LOGID serial,"
                            + "MENUID nvarchar(30) not null,"
                            + "PACKAGE nvarchar(20) not null,"
                            + "PACKAGEDATE DATETIME year to FRACTION,"
                            + "LASTDATE DATETIME year to FRACTION,"
                            + "OWNER nvarchar(20),"
                            + "OLDVERSION byte,"
                            + "OLDDATE nvarchar(20), "
                            + "PRIMARY KEY (LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLELOG table.\n\r";
                        }

                        // Create MENUCHECKLOG
                        command.CommandText = "CREATE TABLE MENUCHECKLOG"
                            + "("
                            + "LOGID serial ,"
                            + "ITEMTYPE nvarchar(20) not null,"
                            + "PACKAGE nvarchar(50) not null,"
                            + "PACKAGEDATE DateTime year to FRACTION,"
                            + "FILETYPE nvarchar(10),"
                            + "FILENAME nvarchar(60),"
                            + "FILEDATE DateTime year to FRACTION,"
                            + "FILECONTENT byte, "
                            + "PRIMARY KEY (LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUCHECKLOG table.\n\r";
                        }

                        // Create SYSEEPLOG
                        command.CommandText = "CREATE TABLE SYSEEPLOG"
                            + "("
                            + "CONNID nvarchar(20) NOT NULL,"
                            + "LOGID serial NOT NULL,"
                            + "LOGSTYLE nvarchar(1) NOT NULL,"
                            + "LOGDATETIME DATETIME year to FRACTION NOT NULL,"
                            + "DOMAINID nvarchar(30),"
                            + "USERID varchar(20),"
                            + "LOGTYPE nvarchar(1),"
                            + "TITLE nvarchar(64),"
                            + "DESCRIPTION nvarchar(128),"
                            + "COMPUTERIP nvarchar(16),"
                            + "COMPUTERNAME nvarchar(64),"
                            + "EXECUTIONTIME integer"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSEEPLOG table.\n\r";
                        }

                        // Create SYSSQLLOG
                        command.CommandText = "CREATE TABLE SYSSQLLOG("
                             + "LOGSTYLE nvarchar(1),"
                             + "LOGDATETIME DateTime year to FRACTION,"
                             + "USERID varchar(20),"
                             + "DEVELOPERID varchar(20),"
                             + "DESCRIPTION blob,"
                             + "SQLSENTENCE nvarchar(max)"
                             + ");";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSSQLLOG table.\n\r";
                        }

                        // Create SYSERRLOG
                        command.CommandText = "CREATE TABLE SYSERRLOG"
                            + "("
                            + "ERRID serial,"
                            + "USERID varchar(20), "
                            + "MODULENAME nvarchar(30),"
                            + "ERRMESSAGE nvarchar(255),"
                            + "ERRSTACK text,"
                            + "ERRDESCRIP nvarchar(255),"
                            + "ERRDATE DateTime year to FRACTION,"
                            + "ERRSCREEN byte,"
                            + "OWNER nvarchar(20),"
                            + "PROCESSDATE DateTime year to FRACTION,"
                            + "PROCDESCRIP nvarchar(255),"
                            + "STATUS nvarchar(2) ,"
                            + "PRIMARY KEY (ERRID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSERRLOG table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_LANGUAGE"
                            + "("
                            + "ID serial NOT NULL,"
                            + "IDENTIFICATION nvarchar(80),"
                            + "KEYS nvarchar(80),"
                            + "EN nvarchar(80),"
                            + "CHT nvarchar(80),"
                            + "CHS nvarchar(80),"
                            + "HK nvarchar(80),"
                            + "JA nvarchar(80),"
                            + "KO nvarchar(80),"
                            + "LAN1 nvarchar(80),"
                            + "LAN2 nvarchar(80) "
                            + ")";

                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_LANGUAGE table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_MESSENGER"
                            + "("
                            + "USERID varchar(20) NOT NULL,"
                            + "MESSAGE nvarchar(255),"
                            + "PARAS nvarchar(255),"
                            + "SENDTIME nvarchar(14),"
                            + "SENDERID nvarchar(20),"
                            + "RECTIME nvarchar(14),"
                            + "STATUS char(1)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_MESSENGER table.\n\r";
                        }

                        //CreateMenuTableControl
                        command.CommandText = "CREATE TABLE MENUTABLECONTROL"
                                                + "("
                                                + "MENUID varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "DESCRIPTION Varchar (50), "
                                                + "TYPE Varchar (20) ,"
                                                + "PRIMARY KEY (MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLECONTROL table.\n\r";
                        }

                        //CreateGroupMenuControl
                        command.CommandText = "CREATE TABLE GROUPMENUCONTROL"
                                                + "("
                                                + "GROUPID Varchar (20) NOT NULL, "
                                                + "MENUID Varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "TYPE Varchar (20), "
                                                + "ENABLED CHAR (1), "
                                                + "VISIBLE CHAR (1), "
                                                + "ALLOWADD CHAR (1), "
                                                + "ALLOWUPDATE CHAR (1), "
                                                + "ALLOWDELETE CHAR (1), "
                                                + "ALLOWPRINT CHAR (1), "
                                                + "PRIMARY KEY (GROUPID, MENUID, CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUCONTROL table.\n\r";
                        }

                        //CreateUserMenuControl
                        command.CommandText = "CREATE TABLE USERMENUCONTROL"
                                                + "("
                                                + "USERID Varchar (20) NOT NULL, "
                                                + "MENUID Varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "TYPE Varchar (20), "
                                                + "ENABLED CHAR (1), "
                                                + "VISIBLE CHAR (1), "
                                                + "ALLOWADD CHAR (1), "
                                                + "ALLOWUPDATE CHAR (1), "
                                                + "ALLOWDELETE CHAR (1), "
                                                + "ALLOWPRINT CHAR (1) "
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUCONTROL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL"
                                            + "("
                                            + "REFVAL_NO Varchar(100) Not NULL, "
                                            + "DESCRIPTION Varchar(250), "
                                            + "TABLE_NAME Varchar(100), "
                                            + "CAPTION Varchar(100), "
                                            + "DISPLAY_MEMBER Varchar(100), "
                                            + "SELECT_ALIAS Varchar(250), "
                                            + "SELECT_COMMAND Varchar(250), "
                                            + "VALUE_MEMBER Varchar(100) "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL_D1"
                                            + "("
                                            + "REFVAL_NO Varchar(30) Not NULL, "
                                            + "FIELD_NAME Varchar(30) Not NULL, "
                                            + "HEADER_TEXT Varchar(20), "
                                            + "WIDTH integer "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL_D1 table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE MENUFAVOR"
                                            + "("
                                            + "MENUID Varchar(30) Not NULL, "
                                            + "CAPTION Varchar(50) Not NULL, "
                                            + "USERID Varchar(20) not null, "
                                            + "ITEMTYPE Varchar(20), "
                                            + "GROUPNAME VarChar(20) "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUFAVOR table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_ANYQUERY"
                                            + "("
                                            + "QUERYID Varchar(20) Not NULL, "
                                            + "USERID Varchar(20) Not NULL, "
                                            + "TEMPLATEID Varchar(20), "
                                            + "TABLENAME Varchar(50), "
                                            + "LASTDATE DateTime year to FRACTION, "
                                            + "CONTENT text, "
                                            + "PRIMARY Key (QUERYID,USERID,TEMPLATEID)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_ANYQUERY table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REPORT"
                                           + "("
                                           + "REPORTID nVarchar(50) Not NULL, "
                                           + "FILENAME nVarchar(50) Not NULL, "
                                           + "REPORTNAME nVarchar(255), "
                                           + "DESCRIPTION nVarchar(255), "
                                           + "FILEPATH nVarchar(255), "
                                           + "OUTPUTMODE nVarchar(20), "
                                           + "HEADERREPEAT nVarchar(5), "
                                           + "HEADERFONT byte, "
                                           + "HEADERITEMS byte, "
                                           + "FOOTERFONT byte, "
                                           + "FOOTERITEMS byte, "
                                           + "FIELDFONT byte, "
                                           + "FIELDITEMS byte, "
                                           + "SETTING byte, "
                                           + "FORMAT byte, "
                                           + "PARAMETERS byte, "
                                           + "IMAGES byte, "
                                           + "MAILSETTING byte, "
                                           + "DATASOURCE_PROVIDER nVarchar(50),"
                                           + "DATASOURCES byte,"
                                           + "CLIENT_QUERY byte,"
                                           + "REPORT_TYPE nVarchar(1),"
                                           + "TEMPLATE_DESC nVarchar(50),"
                                           + "PRIMARY Key (REPORTID,FILENAME)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REPORT table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_PERSONAL"
                                           + "("
                                           + "FORMNAME NVARCHAR(60) NOT NULL,"
                                           + "COMPNAME NVARCHAR(30) NOT NULL,"
                                           + "USERID NVARCHAR(20) NOT NULL,"
                                           + "REMARK NVARCHAR(30),"
                                           + "PROPCONTENT TEXT,"
                                           + "CREATEDATE DATETIME year to FRACTION,"
                                           + "PRIMARY KEY (FORMNAME,COMPNAME,USERID)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_PERSONAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_EEP_USERS"
                                           + "("
                                           + "USERID NVARCHAR(50) NOT NULL,"
                                           + "USERNAME NVARCHAR(50),"
                                           + "COMPUTER NVARCHAR(50) NOT NULL,"
                                           + "LOGINTIME NVARCHAR(50),"
                                           + "LASTACTIVETIME NVARCHAR(50),"
                                           + "LOGINCOUNT integer,"
                                           + "PRIMARY KEY (USERID,COMPUTER)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_EEP_USERS table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE UserDevices"
                                           + "("
                                           + "UserID varchar(50) NULL,"
                                           + "UUID varchar(50) NULL,"
                                           + "Active varchar(1) NULL,"
                                           + "CreateDate DATETIME year to FRACTION,"
                                           + "LoginDate DATETIME year to FRACTION ,"
                                           + "ExpiryDate DATETIME year to FRACTION,"
                                           + "RegID NVARCHAR(250),"
                                           + "TokenID NVARCHAR(250)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create UserDevices table.\n\r";
                        }
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP7m)
                {
                    #region EEP7m
                    command.CommandText = "Alter Table COLDEF add "
                                                      + "DEFAULT_VALUE NVARCHAR(100), "
                                                      + "CHECK_NULL NVARCHAR(1), "
                                                      + "QUERYMODE NVARCHAR(20), "
                                                      + "CAPTION1 NVARCHAR (40), "
                                                      + "CAPTION2 NVARCHAR (40), "
                                                      + "CAPTION3 NVARCHAR (40), "
                                                      + "CAPTION4 NVARCHAR (40), "
                                                      + "CAPTION5 NVARCHAR (40), "
                                                      + "CAPTION6 NVARCHAR (40), "
                                                      + "CAPTION7 NVARCHAR (40), "
                                                      + "CAPTION8 NVARCHAR (40)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPMENUS alter column MENUID nvarchar (30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table GROUPS add MSAD NVARCHAR (1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS alter column GROUPNAME nvarchar (50)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS alter column DESCRIPTION nvarchar (200)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table MENUTABLE add "
                                           + "MODULETYPE NVARCHAR(1), "
                                           + "PACKAGEDATE DATETIME year to FRACTION, "
                                           + "IMAGE byte, "
                                           + "OWNER NVARCHAR(20), "
                                           + "ISSERVER NVARCHAR(1), "
                                           + "VERSIONNO NVARCHAR(20), "
                                           + "CHECKOUT NVARCHAR(20), "
                                           + "CHECKOUTDATE DATETIME year to FRACTION, "
                                           + "CAPTION0 NVARCHAR(50), "
                                           + "CAPTION1 NVARCHAR(50), "
                                           + "CAPTION2 NVARCHAR(50), "
                                           + "CAPTION3 NVARCHAR(50), "
                                           + "CAPTION4 NVARCHAR(50), "
                                           + "CAPTION5 NVARCHAR(50), "
                                           + "CAPTION6 NVARCHAR(50), "
                                           + "CAPTION7 NVARCHAR(50), "
                                           + "IMAGEURL NVARCHAR(100)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE alter column MENUID nvarchar(30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE alter column ITEMTYPE nvarchar(20)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table USERMENUS alter column MENUID nvarchar(30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table USERS add "
                                           + "EMAIL NVARCHAR(40), "
                                           + "LASTTIME NVARCHAR(8), "
                                           + "AUTOLOGIN NVARCHAR(1), "
                                           + "LASTDATE NVARCHAR(8), "
                                           + "SIGNATURE NVARCHAR(30), "
                                           + "MSAD NVARCHAR(1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP2006m)
                {
                    #region EEP2006m
                    command.CommandText = "Alter Table MENUFAVOR ADD GROUPNAME NVARCHAR(20)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table SYS_TODOLIST  ADD  ATTACHMENTS NVARCHAR(255)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "Alter table SYS_TODOHIS  ADD  ATTACHMENTS NVARCHAR(255)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                //odbc未测试
                else if (cdb.x == radioType.WorkFlow)
                {
                    #region WorkFlow
                    //GROUPS
                    command.CommandText = "ALTER TABLE GROUPS ADD ISROLE CHAR(1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not add ISROLE column to GROUPS table.\n\r";
                    }
                    // SYS_ORG
                    command.CommandText = "CREATE TABLE SYS_ORG(" +
                                        "ORG_NO nvarchar(8) NOT NULL," +
                                        "ORG_DESC nvarchar(40) NOT NULL," +
                                        "ORG_KIND nvarchar(4) NOT NULL," +
                                        "UPPER_ORG nvarchar(8)," +
                                        "ORG_MAN nvarchar(20) NOT NULL," +
                                        "LEVEL_NO nvarchar(6) NOT NULL," +
                                        "ORG_TREE nvarchar(40)," +
                                        "END_ORG nvarchar(4)," +
                                        "ORG_FULLNAME nvarchar(254)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORG(ORG_NO,ORG_DESC,ORG_KIND,ORG_MAN,LEVEL_NO) Values ('1','Corporation','0','001','9')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORG table.\n\r";
                    }

                    // SYS_ORGKIND
                    command.CommandText = "CREATE TABLE SYS_ORGKIND(" +
                                        "ORG_KIND nvarchar(4) NOT NULL," +
                                        "KIND_DESC nvarchar(40) NOT NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORGKIND(ORG_KIND,KIND_DESC) Values ('0','Company Organization')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGKIND table.\n\r";
                    }

                    // SYS_ORGLEVEL
                    command.CommandText = "CREATE TABLE SYS_ORGLEVEL(" +
                                        "LEVEL_NO nvarchar(6) NOT NULL," +
                                        "LEVEL_DESC nvarchar(40) NOT NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','Charge ')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('1','Director/Section Chief/Assistant')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('2','Manager')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('3','Deputy Director')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('9','General Manager')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGLEVEL table.\n\r";
                    }

                    // SYS_ORGROLES
                    command.CommandText = "CREATE TABLE SYS_ORGROLES(" +
                                        "ORG_NO nvarchar(8) NOT NULL," +
                                        "ROLE_ID varchar(20) NOT NULL," +
                                        "ORG_KIND nvarchar(4)," +
                                        "PRIMARY KEY(ORG_NO,ROLE_ID)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGROLES table.\n\r";
                    }

                    //建立表单时已经创建了Index，这段就拿掉了
                    //command.CommandText = "CREATE INDEX ORGNO ON SYS_ORGROLES (ORG_NO, ROLE_ID)";
                    //try
                    //{
                    //    command.ExecuteNonQuery();
                    //}
                    //catch
                    //{
                    //    result += "Can not create INDEX ORGNO on table SYS_ORGROLES .\n\r";
                    //}

                    // SYS_ROLES_AGENT
                    command.CommandText = "CREATE TABLE SYS_ROLES_AGENT(" +
                                        "ROLE_ID varchar(20) NOT NULL," +
                                        "AGENT nvarchar(20) NOT NULL," +
                                        "FLOW_DESC nvarchar(40) NOT NULL," +
                                        "START_DATE nvarchar(8) NOT NULL," +
                                        "START_TIME nvarchar(6)," +
                                        "END_DATE nvarchar(8) NOT NULL," +
                                        "END_TIME nvarchar(6)," +
                                        "PAR_AGENT nvarchar(4) NOT NULL," +
                                        "REMARK nvarchar(254)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ROLES_AGENT table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ROLEID ON SYS_ROLES_AGENT (ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX ROLEID on table SYS_ROLES_AGENT .\n\r";
                    }

                    // SYS_TODOHIS
                    command.CommandText = "CREATE TABLE SYS_TODOHIS(" +
                                        "LISTID nvarchar(40) NOT NULL," +
                                        "FLOW_ID nvarchar(40) NOT NULL," +
                                        "FLOW_DESC nvarchar(40)," +
                                        "ROLE_ID varchar(20) NOT NULL," +
                                        "S_ROLE_ID varchar(20) NOT NULL," +
                                        "S_STEP_ID nvarchar(20) NOT NULL," +
                                        "D_STEP_ID nvarchar(20) NOT NULL," +
                                        "S_STEP_DESC nvarchar(64)," +
                                        "S_USER_ID nvarchar(20) NOT NULL," +
                                        "USER_ID nvarchar(20) NOT NULL," +
                                        "USERNAME nvarchar(30)," +
                                        "FORM_NAME nvarchar(30)," +
                                        "WEBFORM_NAME nvarchar(50) NOT NULL," +
                                        "S_USERNAME nvarchar(30)," +
                                        "NAVIGATOR_MODE nvarchar(2) NOT NULL," +
                                        "FLNAVIGATOR_MODE nvarchar(2) NOT NULL," +
                                        "PARAMETERS nvarchar(254)," +
                                        "STATUS nvarchar(4)," +
                                        "PROC_TIME decimal(8, 2) NOT NULL," +
                                        "EXP_TIME decimal(8, 2) NOT NULL," +
                                        "TIME_UNIT nvarchar(4) NOT NULL," +
                                        "FLOWIMPORTANT varchar(1) NOT NULL," +
                                        "FLOWURGENT varchar(1) NOT NULL," +
                                        "FORM_TABLE nvarchar(30)," +
                                        "FORM_KEYS nvarchar(254)," +
                                        "FORM_PRESENTATION nvarchar(254)," +
                                        "REMARK nvarchar(254)," +
                                        "VERSION nvarchar(2)," +
                                        "VDSNAME nvarchar(40)," +
                                        "SENDBACKSTEP nvarchar(2)," +
                                        "LEVEL_NO nvarchar(6)," +
                                        "UPDATE_DATE nvarchar(10)," +
                                        "UPDATE_TIME nvarchar(8)," +
                                        "FORM_PRESENT_CT nvarchar(254)," +
                                        "ATTACHMENTS nvarchar(255)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOHIS table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOHIS (LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX LISTID on table SYS_TODOHIS.\n\r";
                    }

                    command.CommandText = "CREATE INDEX USERID ON SYS_TODOHIS (USER_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX USERID on table SYS_TODOHIS.\n\r";
                    }

                    // SYS_TODOLIST
                    command.CommandText = "CREATE TABLE SYS_TODOLIST(" +
                                        "LISTID nvarchar(40) NOT NULL," +
                                        "FLOW_ID nvarchar(40) NOT NULL," +
                                        "FLOW_DESC nvarchar(40)," +
                                        "APPLICANT nvarchar(20) NOT NULL," +
                                        "S_USER_ID nvarchar(20) NOT NULL," +
                                        "S_STEP_ID nvarchar(20) NOT NULL," +
                                        "S_STEP_DESC nvarchar(64)," +
                                        "D_STEP_ID nvarchar(20) NOT NULL," +
                                        "D_STEP_DESC nvarchar(64)," +
                                        "EXP_TIME decimal(8, 2) NOT NULL," +
                                        "URGENT_TIME decimal(8, 2) NOT NULL," +
                                        "TIME_UNIT nvarchar(4) NOT NULL," +
                                        "USERNAME nvarchar(30)," +
                                        "FORM_NAME nvarchar(30)," +
                                        "NAVIGATOR_MODE nvarchar(2) NOT NULL," +
                                        "FLNAVIGATOR_MODE nvarchar(2) NOT NULL," +
                                        "PARAMETERS nvarchar(254)," +
                                        "SENDTO_KIND nvarchar(4) NOT NULL," +
                                        "SENDTO_ID nvarchar(20) NOT NULL," +
                                        "FLOWIMPORTANT varchar(1) NOT NULL," +
                                        "FLOWURGENT nvarchar(1) NOT NULL," +
                                        "STATUS nvarchar(4)," +
                                        "FORM_TABLE nvarchar(30)," +
                                        "FORM_KEYS nvarchar(254)," +
                                        "FORM_PRESENTATION nvarchar(254)," +
                                        "FORM_PRESENT_CT nvarchar(254) NOT NULL," +
                                        "REMARK nvarchar(254)," +
                                        "PROVIDER_NAME nvarchar(254)," +
                                        "VERSION nvarchar(2)," +
                                        "EMAIL_ADD nvarchar(40)," +
                                        "EMAIL_STATUS varchar(1)," +
                                        "VDSNAME nvarchar(40)," +
                                        "SENDBACKSTEP nvarchar(2)," +
                                        "LEVEL_NO nvarchar(6)," +
                                        "WEBFORM_NAME nvarchar(50) NOT NULL," +
                                        "UPDATE_DATE nvarchar(10)," +
                                        "UPDATE_TIME nvarchar(8)," +
                                        "FLOWPATH nvarchar(100) NOT NULL," +
                                        "PLUSAPPROVE varchar(1) NOT NULL," +
                                        "PLUSROLES nvarchar(254) NOT NULL," +
                                        "MULTISTEPRETURN varchar(1)," +
                                        "SENDTO_NAME nvarchar(30)," +
                                        "ATTACHMENTS NVARCHAR(255)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOLIST table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID2 ON SYS_TODOLIST (LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX LISTID2 on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX SENDTOID ON SYS_TODOLIST (SENDTO_ID, SENDTO_KIND)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX SENDTOID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX FLOWDESC ON SYS_TODOLIST (FLOW_DESC)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX FLOWDESC on table SYS_TODOLIST.\n\r";
                    }

                    // SYS_FLDefinition
                    command.CommandText = "CREATE TABLE SYS_FLDEFINITION"
                                            + "("
                                            + "FLTYPEID nvarchar(50) NOT NULL, "
                                            + "FLTYPENAME nvarchar(200) NOT NULL, "
                                            + "FLDEFINITION text NOT NULL, "
                                            + "VERSION integer, "
                                            + "PRIMARY KEY(FLTYPEID) "
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLDEFINITION table.\n\r";
                    }

                    // SYS_FLInstanceState
                    command.CommandText = "CREATE TABLE SYS_FLINSTANCESTATE"
                                            + "("
                                            + "FLINSTANCEID nvarchar(50) NOT NULL, "
                                            + "STATE byte NOT NULL, "
                                            + "STATUS integer, "
                                            + "INFO nvarchar(200)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLINSTANCESTATE table.\n\r";
                    }

                    // Sys_ExtApprove
                    command.CommandText = "CREATE TABLE SYS_EXTAPPROVE"
                                            + "("
                                            + "APPROVEID nvarchar(50) NULL, "
                                            + "GROUPID nvarchar(50) NULL, "
                                            + "MINIMUM nvarchar(50) NULL, "
                                            + "MAXIMUM nvarchar(50) NULL,"
                                            + "ROLEID nvarchar(50) NULL"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_EXTAPPROVE table.\n\r";
                    }
                    #endregion
                }

                if (result != "")
                    MessageBox.Show(result);
                else if (cdb.x == radioType.typical)
                    MessageBox.Show("Create all System Tables successfully.");
                else if (cdb.x == radioType.simplified)
                    MessageBox.Show("Create table COLDEF table and table SYSAUTONUM successfully.");
                else if (cdb.x == radioType.EEP7m)
                    MessageBox.Show("Alter successfully.");
                else if (cdb.x == radioType.EEP2006m)
                    MessageBox.Show("EEP 2006 2.1.0.1(SP2) Migration success");
                else if (cdb.x == radioType.WorkFlow)
                    MessageBox.Show("Create WorkFlow Tables successfully.");
            }
        }

        public void CreateOracleSystemTable(IDbCommand command)
        {
            frmCreateDB cdb = new frmCreateDB();
            cdb.ShowDialog();

            if (cdb.DialogResult == DialogResult.OK)
            {
                string result = "";

                if (cdb.x == radioType.simplified || cdb.x == radioType.typical)
                {
                    #region Simplified
                    command.CommandText = "CREATE TABLE COLDEF"
                                  + "("
                                  + "TABLE_NAME varchar2(20) NOT NULL, "
                                  + "FIELD_NAME varchar2(20) NOT NULL, "
                                  + "SEQ NUMERIC(12,0) NULL, "
                                  + "FIELD_TYPE varchar2(20) NULL, "
                                  + "IS_KEY varchar2(1) NOT NULL, "
                                  + "FIELD_LENGTH NUMERIC(12,0) NULL, "
                                  + "CAPTION varchar2(40) NULL, "
                                  + "EDITMASK varchar2(10) NULL, "
                                  + "NEEDBOX varchar2(13) NULL, "
                                  + "CANREPORT varchar2(1) NULL, "
                                  + "EXT_MENUID varchar2(20) NULL, "
                                  + "FIELD_SCALE NUMERIC(12,0) NULL, "
                                  + "DD_NAME varchar2(40) NULL, "
                                  + "DEFAULT_VALUE varchar2(100) NULL, "
                                  + "CHECK_NULL varchar2(1) NULL, "
                                  + "QUERYMODE varchar2(20) NULL, "
                                  + "CAPTION1 varchar2(40) NULL, "
                                  + "CAPTION2 varchar2(40) NULL, "
                                  + "CAPTION3 varchar2(40) NULL, "
                                  + "CAPTION4 varchar2(40) NULL, "
                                  + "CAPTION5 varchar2(40) NULL, "
                                  + "CAPTION6 varchar2(40) NULL, "
                                  + "CAPTION7 varchar2(40) NULL, "
                                  + "CAPTION8 varchar2(40) NULL, "
                                  + "PRIMARY KEY(TABLE_NAME,FIELD_NAME)"
                                  + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create COLDEF table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX TABLENAME ON COLDEF(TABLE_NAME,FIELD_NAME)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not create INDEX TABLENAME on table COLDEF .\n\r";
                    }

                    command.CommandText = "CREATE TABLE SYSAUTONUM "
                                            + "("
                                            + "AUTOID VARCHAR2(20) NOT NULL, "
                                            + "FIXED VARCHAR2(20) NOT NULL, "
                                            + "CURRNUM NUMERIC(10,0) NULL, "
                                            + "DESCRIPTION VARCHAR2(50) NULL, "
                                            + "PRIMARY KEY (AUTOID, FIXED)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYSAUTONUM table.\n\r";
                    }
                    #endregion

                    #region Typical
                    if (cdb.x == radioType.typical)
                    {
                        //// Create GROUPFORMS
                        //command.CommandText = "CREATE TABLE GROUPFORMS ("
                        //    + "GROUPID nvarchar (20) NOT NULL ,"
                        //    + "PACKAGE_NAME nvarchar (50) NOT NULL ,"
                        //    + "FORM_NAME nvarchar (50) NULL ,"
                        //    + "PARENT_MENU nvarchar (50) NULL "
                        //    + ")";
                        //try
                        //{
                        //    command.ExecuteNonQuery();
                        //}
                        //catch
                        //{
                        //    result += "Can not create GROUPFORMS table.\n\r";
                        //}

                        // Create GROUPMENUS
                        command.CommandText = "CREATE TABLE GROUPMENUS ("
                            + "GROUPID varchar2 (20) NOT NULL ,"
                            + "MENUID varchar2 (30) NOT NULL, "
                            + "PRIMARY KEY(GROUPID,MENUID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUS table.\n\r";
                        }

                        // Create USERMENUS
                        command.CommandText = "CREATE TABLE USERMENUS ("
                            + "USERID varchar2 (20) NOT NULL ,"
                            + "MENUID varchar2 (30) NOT NULL, "
                            + "PRIMARY KEY(USERID,MENUID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUS table.\n\r";
                        }

                        // Create GROUPS
                        command.CommandText = "CREATE TABLE GROUPS ("
                            + "GROUPID varchar2 (20) NOT NULL ,"
                            + "GROUPNAME varchar2 (50) NULL ,"
                            + "DESCRIPTION varchar2 (100) NULL ,"
                            + "MSAD varchar2 (1) NULL, "
                            + "ISROLE char(1) NULL ,"
                            + "PRIMARY KEY(GROUPID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('00', 'EveryOne', 'N')";
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('01', 'DEPARTMENT1', 'N')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPS table.\n\r";
                        }

                        // Create MENUITEMTYPE
                        command.CommandText = "CREATE TABLE MENUITEMTYPE ("
                            + "ITEMTYPE varchar2 (20) NOT NULL ,"
                            + "ITEMNAME varchar2 (20) NULL, "
                            + "DBALIAS varchar2(50) NULL, "
                            + "PRIMARY KEY(ITEMTYPE)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUITEMTYPE(ITEMTYPE, ITEMNAME) VALUES('SOLUTION1', 'DEFAULT SOLUTION')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUITEMTYPE table.\n\r";
                        }

                        // Create MENUTABLE
                        command.CommandText = "CREATE TABLE MENUTABLE ("
                            + "MENUID nvarchar2 (30) NOT NULL ,"
                            + "CAPTION nvarchar2 (50) NOT NULL ,"
                            + "PARENT nvarchar2 (20) NULL ,"
                            + "PACKAGE nvarchar2 (60) NULL ,"
                            + "MODULETYPE nvarchar2 (1) NULL ,"
                            + "ITEMPARAM nvarchar2 (200) NULL ,"
                            + "FORM nvarchar2 (32) NULL ,"
                            + "ISSHOWMODAL nvarchar2 (1) NULL ,"
                            + "ITEMTYPE nvarchar2 (20) NULL ,"
                            + "SEQ_NO nvarchar2 (4) NULL,"
                            + "PACKAGEDATE Date,"
                            + "ISSERVER nvarchar2(1),"
                            + "VERSIONNO nvarchar2(20),"
                            + "CHECKOUT nvarchar2(20),"
                            + "CHECKOUTDATE date,"
                            + "CAPTION0 nvarchar2(50) NULL,"
                            + "CAPTION1 nvarchar2(50) NULL,"
                            + "CAPTION2 nvarchar2(50) NULL,"
                            + "CAPTION3 nvarchar2(50) NULL,"
                            + "CAPTION4 nvarchar2(50) NULL,"
                            + "CAPTION5 nvarchar2(50) NULL,"
                            + "CAPTION6 nvarchar2(50) NULL,"
                            + "CAPTION7 nvarchar2(50) NULL,"
                            + "IMAGE blob NULL,"
                            + "IMAGEURL varchar2(100) NULL, "
                            + "OWNER varchar2(20),"
                            + "PRIMARY KEY(MENUID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUTABLE(MENUID, CAPTION, ITEMTYPE, MODULETYPE) VALUES('0', 'ROOT', 'SOLUTION1', 'F')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLE table.\n\r";
                        }

                        // Create USERGROUPS
                        command.CommandText = "CREATE TABLE USERGROUPS ("
                            + "USERID varchar2 (20) NOT NULL ,"
                            + "GROUPID varchar2 (20) NOT NULL, "
                            + "PRIMARY KEY(USERID,GROUPID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERGROUPS(USERID, GROUPID) VALUES('001', '01')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERGROUPS table.\n\r";
                        }

                        // Create USERS
                        command.CommandText = "CREATE TABLE USERS ("
                            + "USERID varchar2 (20) NOT NULL ,"
                            + "USERNAME varchar2 (30) NULL ,"
                            + "AGENT varchar2 (20) NULL ,"
                            + "PWD varchar2 (10) NULL ,"
                            + "CREATEDATE varchar2 (8) NULL ,"
                            + "CREATER varchar2 (20) NULL ,"
                            + "DESCRIPTION varchar2 (100) NULL ,"
                            + "EMAIL varchar2 (40) NULL ,"
                            + "LASTTIME varchar2 (8) NULL ,"
                            + "AUTOLOGIN varchar2 (1),"
                            + "LASTDATE varchar2 (8) NULL ,"
                            + "SIGNATURE varchar2 (30) NULL ,"
                            + "MSAD varchar2 (1) NULL, "
                            + "PRIMARY KEY(USERID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERS(USERID, USERNAME, PWD, MSAD,AUTOLOGIN) VALUES('001', 'TEST', '', 'N','S')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERS table.\n\r";
                        }

                        // Create MENUTABLELOG
                        command.CommandText = "CREATE TABLE MENUTABLELOG"
                            + "("
                            + "LOGID number(10),"
                            + "MENUID varchar2(30) not null,"
                            + "PACKAGE varchar2(20) not null,"
                            + "PACKAGEDATE DATE,"
                            + "LASTDATE DATE,"
                            + "OWNER varchar2(20),"
                            + "OLDVERSION blob,"
                            + "OLDDATE varchar2(20), "
                            + "PRIMARY KEY(LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE SEQUENCE MENUTABLELOG_LODID_SEQ " +
                                                    "START WITH 1 " +
                                                    "MAXVALUE 9999999999 " +
                                                    "MINVALUE 1 " +
                                                    "NOCYCLE " +
                                                    "NOCACHE " +
                                                    "NOORDER";
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE OR REPLACE TRIGGER MENUTABLELOG_LODID" +
                                        " BEFORE INSERT" +
                                        " ON MENUTABLELOG" +
                                        " FOR EACH ROW" +
                                        " DECLARE" +
                                        " NEXT_ID NUMBER;" +
                                        " BEGIN" +
                                        " SELECT MENUTABLELOG_LODID_SEQ.NEXTVAL INTO NEXT_ID FROM dual;" +
                                        " :NEW.LOGID := NEXT_ID;" +
                                        " END;";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLELOG table.\n\r";
                        }

                        // Create MENUCHECKLOG
                        command.CommandText = "CREATE TABLE MENUCHECKLOG"
                            + "("
                            + "LOGID number(10),"
                            + "ITEMTYPE varchar2(20) not null,"
                            + "PACKAGE varchar2(50) not null,"
                            + "PACKAGEDATE Date,"
                            + "FILETYPE varchar2(10),"
                            + "FILENAME varchar2(60),"
                            + "FILEDATE Date,"
                            + "FILECONTENT blob, "
                            + "PRIMARY KEY(LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE SEQUENCE MENUCHECKLOG_LODID_SEQ " +
                                                    "START WITH 1 " +
                                                    "MAXVALUE 9999999999 " +
                                                    "MINVALUE 1 " +
                                                    "NOCYCLE " +
                                                    "NOCACHE " +
                                                    "NOORDER";
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE OR REPLACE TRIGGER MENUCHECKLOG_LODID" +
                                                    " BEFORE INSERT" +
                                                    " ON MENUCHECKLOG" +
                                                    " FOR EACH ROW" +
                                                    " DECLARE" +
                                                    " NEXT_ID NUMBER;" +
                                                    " BEGIN" +
                                                    " SELECT MENUCHECKLOG_LODID_SEQ.NEXTVAL INTO NEXT_ID FROM dual;" +
                                                    " :NEW.LOGID := NEXT_ID;" +
                                                    " END;";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUCHECKLOG table.\n\r";
                        }

                        // Create SYSEEPLOG
                        command.CommandText = "CREATE TABLE SYSEEPLOG"
                            + "("
                            + "CONNID varchar2(20) NOT NULL,"
                            + "LOGID number(10) NOT NULL,"
                            + "LOGSTYLE varchar2(1) NOT NULL,"
                            + "LOGDATETIME DATE NOT NULL,"
                            + "DOMAINID varchar2(30) NULL,"
                            + "USERID varchar2(20) NULL,"
                            + "LOGTYPE varchar2(1) NULL,"
                            + "TITLE varchar2(64) NULL,"
                            + "DESCRIPTION LONG NULL,"
                            + "COMPUTERIP varchar2(16) NULL,"
                            + "COMPUTERNAME varchar2(64) NULL,"
                            + "EXECUTIONTIME INT NULL,"
                            + "PRIMARY KEY(CONNID,LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE SEQUENCE SYSEEPLOG_LOGID_SEQ " +
                                                    "START WITH 1 " +
                                                    "MAXVALUE 9999999999 " +
                                                    "MINVALUE 1 " +
                                                    "CYCLE " +
                                                    "NOCACHE " +
                                                    "NOORDER";
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE OR REPLACE TRIGGER SYSEEPLOG_LOGID" +
                                                    " BEFORE INSERT" +
                                                    " ON SYSEEPLOG" +
                                                    " FOR EACH ROW" +
                                                    " DECLARE" +
                                                    " NEXT_ID NUMBER;" +
                                                    " BEGIN" +
                                                    " SELECT SYSEEPLOG_LOGID_SEQ.NEXTVAL INTO NEXT_ID FROM dual;" +
                                                    " :NEW.LOGID := NEXT_ID;" +
                                                    " END;";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSEEPLOG table.\n\r";
                        }

                        // Create SYSSQLLOG
                        command.CommandText = "CREATE TABLE SYSSQLLOG("
                             + "LOGSTYLE varchar2(1),"
                             + "LOGDATETIME Date,"
                             + "USERID varchar2(20),"
                             + "DEVELOPERID varchar2(20),"
                             + "DESCRIPTION blob,"
                             + "SQLSENTENCE varchar2(1000)"
                             + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSSQLLOG table.\n\r";
                        }

                        // Create SYSERRLOG
                        command.CommandText = "CREATE TABLE SYSERRLOG"
                            + "("
                            + "ERRID number(10),"
                            + "USERID varchar2(20), "
                            + "MODULENAME varchar2(30),"
                            + "ERRMESSAGE varchar2(255),"
                            + "ERRSTACK varchar2(255),"
                            + "ERRDESCRIP varchar2(255),"
                            + "ERRDATE Date,"
                            + "ERRSCREEN blob,"
                            + "OWNER varchar2(20),"
                            + "PROCESSDATE Date,"
                            + "PROCDESCRIP varchar2(255),"
                            + "STATUS varchar2(2), "
                            + "PRIMARY KEY(ERRID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE SEQUENCE SYSERRLOG_ErrID_SEQ " +
                                                    "START WITH 1 " +
                                                    "MAXVALUE 9999999999 " +
                                                    "MINVALUE 1 " +
                                                    "NOCYCLE " +
                                                    "NOCACHE " +
                                                    "NOORDER";
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE OR REPLACE TRIGGER SYSERRLOG_ErrID" +
                                                    " BEFORE INSERT" +
                                                    " ON SYSERRLOG" +
                                                    " FOR EACH ROW" +
                                                    " DECLARE" +
                                                    " NEXT_ID NUMBER;" +
                                                    " BEGIN" +
                                                    " SELECT SYSERRLOG_ErrID_SEQ.NEXTVAL INTO NEXT_ID FROM dual;" +
                                                    " :NEW.ErrID := NEXT_ID;" +
                                                    " END;";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSERRLOG table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_LANGUAGE"
                            + "("
                            + "ID number(10) NOT NULL,"
                            + "IDENTIFICATION varchar2(80),"
                            + "KEYS varchar2(80),"
                            + "EN varchar2(80),"
                            + "CHT varchar2(80),"
                            + "CHS varchar2(80),"
                            + "HK varchar2(80),"
                            + "JA varchar2(80),"
                            + "KO varchar2(80),"
                            + "LAN1 varchar2(80),"
                            + "LAN2 varchar2(80), "
                            + "PRIMARY KEY(ID)"
                            + ")";

                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE SEQUENCE SYS_LANGUAGE_ID_SEQ " +
                                                    "START WITH 1 " +
                                                    "MAXVALUE 9999999999 " +
                                                    "MINVALUE 1 " +
                                                    "NOCYCLE " +
                                                    "NOCACHE " +
                                                    "NOORDER";
                            command.ExecuteNonQuery();

                            command.CommandText = "CREATE OR REPLACE TRIGGER SYS_LANGUAGE_ID" +
                                                    " BEFORE INSERT" +
                                                    " ON SYS_LANGUAGE" +
                                                    " FOR EACH ROW" +
                                                    " DECLARE" +
                                                    " NEXT_ID NUMBER;" +
                                                    " BEGIN" +
                                                    " SELECT SYS_LANGUAGE_ID_SEQ.NEXTVAL INTO NEXT_ID FROM dual;" +
                                                    " :NEW.ID := NEXT_ID;" +
                                                    " END;";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_LANGUAGE table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_MESSENGER"
                            + "("
                            + "USERID varchar2(20) NOT NULL,"
                            + "MESSAGE varchar2(255),"
                            + "PARAS varchar2(255),"
                            + "SENDTIME varchar2(14),"
                            + "SENDERID varchar2(20),"
                            + "RECTIME varchar2(14),"
                            + "STATUS char(1)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_MESSENGER table.\n\r";
                        }

                        //CreateMenuTableControl
                        command.CommandText = "CREATE TABLE MENUTABLECONTROL"
                                                + "("
                                                + "MENUID varchar2 (30) NOT NULL, "
                                                + "CONTROLNAME Varchar2 (50) NOT NULL, "
                                                + "DESCRIPTION Varchar2 (50) NULL, "
                                                + "TYPE Varchar (20) NULL, "
                                                + "PRIMARY KEY(MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLECONTROL table.\n\r";
                        }

                        //CreateGroupMenuControl
                        command.CommandText = "CREATE TABLE GROUPMENUCONTROL"
                                                + "("
                                                + "GROUPID Varchar2 (20) NOT NULL, "
                                                + "MENUID Varchar2 (30) NOT NULL, "
                                                + "CONTROLNAME Varchar2 (50) NOT NULL, "
                                                + "TYPE Varchar2 (20) NULL, "
                                                + "ENABLED CHAR (1) NULL, "
                                                + "VISIBLE CHAR (1) NULL, "
                                                + "ALLOWADD CHAR (1) NULL, "
                                                + "ALLOWUPDATE CHAR (1) NULL, "
                                                + "ALLOWDELETE CHAR (1) NULL, "
                                                + "ALLOWPRINT CHAR (1) NULL, "
                                                + "PRIMARY KEY(GROUPID,MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUCONTROL table.\n\r";
                        }

                        //CreateUserMenuControl
                        command.CommandText = "CREATE TABLE USERMENUCONTROL"
                                                + "("
                                                + "USERID Varchar2 (20) NOT NULL, "
                                                + "MENUID Varchar2 (30) NOT NULL, "
                                                + "CONTROLNAME Varchar2 (50) NOT NULL, "
                                                + "TYPE Varchar2 (20) NULL, "
                                                + "ENABLED CHAR (1) NULL, "
                                                + "VISIBLE CHAR (1) NULL, "
                                                + "ALLOWADD CHAR (1) NULL, "
                                                + "ALLOWUPDATE CHAR (1) NULL, "
                                                + "ALLOWDELETE CHAR (1) NULL, "
                                                + "ALLOWPRINT CHAR (1) NULL, "
                                                + "PRIMARY KEY(USERID,MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUCONTROL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL"
                                            + "("
                                            + "REFVAL_NO Varchar2(100) Not NULL, "
                                            + "DESCRIPTION Varchar2(250), "
                                            + "TABLE_NAME Varchar2(100), "
                                            + "CAPTION Varchar2(100), "
                                            + "DISPLAY_MEMBER Varchar2(100), "
                                            + "SELECT_ALIAS Varchar2(250), "
                                            + "SELECT_COMMAND Varchar2(250), "
                                            + "VALUE_MEMBER Varchar2(100), "
                                            + "CONSTRAINT PK_SYS_REFVAL PRIMARY KEY(REFVAL_NO) "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL_D1"
                                            + "("
                                            + "REFVAL_NO Varchar2(30) NOT NULL, "
                                            + "FIELD_NAME Varchar2(30) NOT NULL, "
                                            + "HEADER_TEXT Varchar2(20), "
                                            + "WIDTH INT, "
                                            + "CONSTRAINT PK_SYS_REFVAL_D1 PRIMARY KEY(REFVAL_NO, FIELD_NAME) "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE MENUFAVOR"
                                            + "("
                                            + "MENUID Varchar2(30) NOT NULL, "
                                            + "CAPTION Varchar2(50) NOT NULL, "
                                            + "USERID Varchar2(20) NOT NULL, "
                                            + "ITEMTYPE Varchar(20), "
                                            + "GROUPNAME VarChar(20), "
                                            + "PRIMARY KEY (MENUID,USERID)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUFAVOR table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_ANYQUERY"
                                            + "("
                                            + "QUERYID Varchar2(20) Not NULL, "
                                            + "USERID Varchar2(20) Not NULL, "
                                            + "TEMPLATEID Varchar2(20), "
                                            + "TABLENAME Varchar2(50), "
                                            + "LASTDATE date, "
                                            + "CONTENT blob, "
                                            + "PRIMARY Key (QUERYID,USERID,TEMPLATEID)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_ANYQUERY table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REPORT"
                                            + "("
                                            + "REPORTID nVarchar2(50) Not NULL, "
                                            + "FILENAME nVarchar2(50) Not NULL, "
                                            + "REPORTNAME nVarchar2(255), "
                                            + "DESCRIPTION nVarchar2(255), "
                                            + "FILEPATH nVarchar2(255), "
                                            + "OUTPUTMODE nVarchar2(20), "
                                            + "HEADERREPEAT nVarchar2(5), "
                                            + "HEADERFONT blob, "
                                            + "HEADERITEMS blob, "
                                            + "FOOTERFONT blob, "
                                            + "FOOTERITEMS blob, "
                                            + "FIELDFONT blob, "
                                            + "FIELDITEMS blob, "
                                            + "SETTING blob, "
                                            + "FORMAT blob, "
                                            + "PARAMETERS blob, "
                                            + "IMAGES blob, "
                                            + "MAILSETTING blob, "
                                            + "DATASOURCE_PROVIDER nVarchar2(50),"
                                           + "DATASOURCES blob,"
                                           + "CLIENT_QUERY blob,"
                                           + "REPORT_TYPE nVarchar2(1),"
                                           + "TEMPLATE_DESC nVarchar2(50),"
                                            + "PRIMARY Key (REPORTID,FILENAME)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REPORT table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_PERSONAL"
                                           + "("
                                           + "FORMNAME NVARCHAR2(60) NOT NULL,"
                                           + "COMPNAME NVARCHAR2(30) NOT NULL,"
                                           + "USERID NVARCHAR2(20) NOT NULL,"
                                           + "REMARK NVARCHAR2(30),"
                                           + "PROPCONTENT LONG,"
                                           + "CREATEDATE DATE,"
                                           + "PRIMARY KEY (FORMNAME,COMPNAME,USERID)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_PERSONAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_EEP_USERS"
                                           + "("
                                           + "USERID NVARCHAR2(50) NOT NULL,"
                                           + "USERNAME NVARCHAR2(50) NULL,"
                                           + "COMPUTER NVARCHAR2(50) NOT NULL,"
                                           + "LOGINTIME NVARCHAR2(50),"
                                           + "LASTACTIVETIME NVARCHAR2(50),"
                                           + "LOGINCOUNT INT,"
                                           + "PRIMARY KEY (USERID,COMPUTER)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_EEP_USERS table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE UserDevices"
                                           + "("
                                           + "UserID Varchar2(50) NULL,"
                                           + "UUID Varchar2(50) NULL,"
                                           + "Active Varchar2(1) NULL,"
                                           + "CreateDate date,"
                                           + "LoginDate date,"
                                           + "ExpiryDate date,"
                                           + "RegID NVARCHAR2(255),"
                                           + "TokenID NVARCHAR2(255)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create UserDevices table.\n\r";
                        }
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP7m)
                {
                    #region EEP7m
                    command.CommandText = "Alter Table COLDEF add ("
                                                       + "DEFAULT_VALUE VARCHAR2(100) NULL, "
                                                       + "CHECK_NULL VARCHAR2(1) NULL, "
                                                       + "QUERYMODE VARCHAR2(20), "
                                                       + "CAPTION1 VARCHAR2 (40), "
                                                       + "CAPTION2 VARCHAR2 (40), "
                                                       + "CAPTION3 VARCHAR2 (40), "
                                                       + "CAPTION4 VARCHAR2 (40), "
                                                       + "CAPTION5 VARCHAR2 (40), "
                                                       + "CAPTION6 VARCHAR2 (40), "
                                                       + "CAPTION7 VARCHAR2 (40), "
                                                       + "CAPTION8 VARCHAR2 (40))";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPMENUS modify (MENUID varchar2 (30))";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table GROUPS add (MSAD VARCHAR2 (1))";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS modify (GROUPNAME varchar2 (50))";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS modify (DESCRIPTION varchar2 (200))";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table MENUTABLE add("
                                           + "MODULETYPE VARCHAR2(1), "
                                           + "PACKAGEDATE DATE, "
                                           + "IMAGE blob, "
                                           + "OWNER VARCHAR2(20), "
                                           + "ISSERVER VARCHAR2(1), "
                                           + "VERSIONNO VARCHAR2(20), "
                                           + "CHECKOUT VARCHAR2(20), "
                                           + "CHECKOUTDATE DATE, "
                                           + "CAPTION0 VARCHAR2(50), "
                                           + "CAPTION1 VARCHAR2(50), "
                                           + "CAPTION2 VARCHAR2(50), "
                                           + "CAPTION3 VARCHAR2(50), "
                                           + "CAPTION4 VARCHAR2(50), "
                                           + "CAPTION5 VARCHAR2(50), "
                                           + "CAPTION6 VARCHAR2(50), "
                                           + "CAPTION7 VARCHAR2(50), "
                                           + "IMAGEURL VARCHAR2(100))";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE modify (MENUID varchar2(30))";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE modify (ITEMTYPE varchar2(20))";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table USERMENUS modify (MENUID varchar2(30))";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table USERS add("
                                           + "EMAIL VARCHAR2(40), "
                                           + "LASTTIME VARCHAR2(8), "
                                           + "AUTOLOGIN VARCHAR2(1), "
                                           + "LASTDATE VARCHAR2(8), "
                                           + "SIGNATURE VARCHAR2(30), "
                                           + "MSAD VARCHAR2(1))";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP2006m)
                {
                    #region EEP2006m
                    command.CommandText = "Alter Table MENUFAVOR add (GROUPNAME VARCHAR2(20))";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "Alter Table SYS_TODOLIST add (ATTACHMENTS VARCHAR2(255) NULL)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table SYS_TODOHIS add (ATTACHMENTS VARCHAR2(255) NULL)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.WorkFlow)
                {
                    #region WorkFlow
                    //GROUPS
                    command.CommandText = "ALTER TABLE GROUPS ADD ISROLE CHAR(1) NULL";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not add ISROLE column to GROUPS table.\n\r";
                    }
                    // SYS_ORG
                    command.CommandText = "CREATE TABLE SYS_ORG(" +
                                        "ORG_NO varchar2(8) NOT NULL," +
                                        "ORG_DESC varchar2(40) NOT NULL," +
                                        "ORG_KIND varchar2(4) NOT NULL," +
                                        "UPPER_ORG varchar2(8) NULL," +
                                        "ORG_MAN varchar2(20) NOT NULL," +
                                        "LEVEL_NO varchar2(6) NOT NULL," +
                                        "ORG_TREE varchar2(40) NULL," +
                                        "END_ORG varchar2(4) NULL," +
                                        "ORG_FULLNAME varchar2(254) NULL," +
                                        "PRIMARY KEY(ORG_NO)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORG(ORG_NO,ORG_DESC,ORG_KIND,ORG_MAN,LEVEL_NO) Values ('1','總公司','0','001','9')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORG table.\n\r";
                    }

                    // SYS_ORGKIND
                    command.CommandText = "CREATE TABLE SYS_ORGKIND(" +
                                        "ORG_KIND varchar2(4) NOT NULL," +
                                        "KIND_DESC varchar2(40) NOT NULL," +
                                        "PRIMARY KEY(ORG_KIND)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORGKIND(ORG_KIND,KIND_DESC) Values ('0','公司組織')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGKIND table.\n\r";
                    }

                    // SYS_ORGLEVEL
                    command.CommandText = "CREATE TABLE SYS_ORGLEVEL(" +
                                        "LEVEL_NO varchar2(6) NOT NULL," +
                                        "LEVEL_DESC varchar2(40) NOT NULL," +
                                        "PRIMARY KEY(LEVEL_NO)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('1','主任/課長/副理')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('2','經理')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('3','副總')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('9','總經理')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGLEVEL table.\n\r";
                    }

                    // SYS_ORGROLES
                    command.CommandText = "CREATE TABLE SYS_ORGROLES(" +
                                        "ORG_NO varchar2(8) NOT NULL," +
                                        "ROLE_ID varchar2(20) NOT NULL," +
                                        "ORG_KIND varchar2(4) NULL," +
                                        "PRIMARY KEY(ORG_NO,ROLE_ID)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGROLES table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ORGNO ON SYS_ORGROLES(ORG_NO, ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not create INDEX ORGNO on table SYS_ORGROLES .\n\r";
                    }

                    // SYS_ROLES_AGENT
                    command.CommandText = "CREATE TABLE SYS_ROLES_AGENT(" +
                                        "ROLE_ID varchar2(20) NULL," +
                                        "AGENT varchar2(20) NOT NULL," +
                                        "FLOW_DESC varchar2(40) NOT NULL," +
                                        "START_DATE varchar2(8) NOT NULL," +
                                        "START_TIME varchar2(6) NULL," +
                                        "END_DATE varchar2(8) NOT NULL," +
                                        "END_TIME varchar2(6) NULL," +
                                        "PAR_AGENT varchar2(4) NOT NULL," +
                                        "REMARK varchar2(254) NULL," +
                                        "PRIMARY KEY(ROLE_ID,AGENT)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ROLES_AGENT table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ROLEID ON SYS_ROLES_AGENT(ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not create INDEX ROLEID on table SYS_ROLES_AGENT .\n\r";
                    }

                    // SYS_TODOHIS
                    command.CommandText = "CREATE TABLE SYS_TODOHIS(" +
                                        "LISTID varchar2(40) NOT NULL," +
                                        "FLOW_ID varchar2(40) NOT NULL," +
                                        "FLOW_DESC varchar2(40) NULL," +
                                        "ROLE_ID varchar2(20) NULL," +
                                        "S_ROLE_ID varchar2(20) NULL," +
                                        "S_STEP_ID varchar2(20) NULL," +
                                        "D_STEP_ID varchar2(20) NULL," +
                                        "S_STEP_DESC varchar2(64) NULL," +
                                        "S_USER_ID varchar2(20) NULL," +
                                        "USER_ID varchar2(20) NOT NULL," +
                                        "USERNAME varchar2(30) NULL," +
                                        "FORM_NAME varchar2(30) NULL," +
                                        "WEBFORM_NAME varchar2(50) NULL," +
                                        "S_USERNAME varchar2(30) NULL," +
                                        "NAVIGATOR_MODE varchar2(2) NOT NULL," +
                                        "FLNAVIGATOR_MODE varchar2(2) NOT NULL," +
                                        "PARAMETERS varchar2(254) NULL," +
                                        "STATUS varchar2(4) NULL," +
                                        "PROC_TIME number(8, 2) NOT NULL," +
                                        "EXP_TIME number(8, 2) NOT NULL," +
                                        "TIME_UNIT varchar2(4) NOT NULL," +
                                        "FLOWIMPORTANT varchar2(1) NOT NULL," +
                                        "FLOWURGENT varchar2(1) NOT NULL," +
                                        "FORM_TABLE varchar2(30) NULL," +
                                        "FORM_KEYS varchar2(254) NULL," +
                                        "FORM_PRESENTATION varchar2(254) NULL," +
                                        "REMARK varchar2(254) NULL," +
                                        "VERSION varchar2(2) NULL," +
                                        "VDSNAME varchar2(40) NULL," +
                                        "SENDBACKSTEP varchar2(2) NULL," +
                                        "LEVEL_NO varchar2(6) NULL," +
                                        "UPDATE_DATE varchar2(10) NULL," +
                                        "UPDATE_TIME varchar2(8) NULL," +
                                        "FORM_PRESENT_CT varchar2(254) NULL," +
                                        "ATTACHMENTS varchar2(255) NULL," +
                                        "CREATE_TIME varchar2(50) NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOHIS table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOHIS(LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not create INDEX LISTID on table SYS_TODOHIS.\n\r";
                    }

                    command.CommandText = "CREATE INDEX USERID ON SYS_TODOHIS(USER_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not create INDEX USERID on table SYS_TODOHIS.\n\r";
                    }

                    // SYS_TODOLIST
                    command.CommandText = "CREATE TABLE SYS_TODOLIST(" +
                                        "LISTID varchar2(40) NOT NULL," +
                                        "FLOW_ID varchar2(40) NOT NULL," +
                                        "FLOW_DESC varchar2(40) NULL," +
                                        "APPLICANT varchar2(20) NOT NULL," +
                                        "S_USER_ID varchar2(20) NULL," +
                                        "S_STEP_ID varchar2(20) NULL," +
                                        "S_STEP_DESC varchar2(64) NULL," +
                                        "D_STEP_ID varchar2(20) NULL," +
                                        "D_STEP_DESC varchar2(64) NULL," +
                                        "EXP_TIME number(8, 2) NOT NULL," +
                                        "URGENT_TIME number(8, 2) NOT NULL," +
                                        "TIME_UNIT varchar2(4) NOT NULL," +
                                        "USERNAME varchar2(30) NULL," +
                                        "FORM_NAME varchar2(30) NULL," +
                                        "NAVIGATOR_MODE varchar2(2) NOT NULL," +
                                        "FLNAVIGATOR_MODE varchar2(2) NOT NULL," +
                                        "PARAMETERS varchar2(254) NULL," +
                                        "SENDTO_KIND varchar2(4) NOT NULL," +
                                        "SENDTO_ID varchar2(20) NOT NULL," +
                                        "FLOWIMPORTANT varchar2(1) NOT NULL," +
                                        "FLOWURGENT varchar2(1) NOT NULL," +
                                        "STATUS varchar2(4) NULL," +
                                        "FORM_TABLE varchar2(30) NULL," +
                                        "FORM_KEYS varchar2(254) NULL," +
                                        "FORM_PRESENTATION varchar2(254) NULL," +
                                        "FORM_PRESENT_CT varchar2(254) NOT NULL," +
                                        "REMARK varchar2(254) NULL," +
                                        "PROVIDER_NAME varchar2(254) NULL," +
                                        "VERSION varchar2(2) NULL," +
                                        "EMAIL_ADD varchar2(40) NULL," +
                                        "EMAIL_STATUS varchar2(1) NULL," +
                                        "VDSNAME varchar2(40) NULL," +
                                        "SENDBACKSTEP varchar2(2) NULL," +
                                        "LEVEL_NO varchar2(6) NULL," +
                                        "WEBFORM_NAME varchar2(50) NULL," +
                                        "UPDATE_DATE varchar2(10) NULL," +
                                        "UPDATE_TIME varchar2(8) NULL," +
                                        "FLOWPATH varchar2(100) NOT NULL," +
                                        "PLUSAPPROVE varchar2(1) NOT NULL," +
                                        "PLUSROLES varchar2(254) NULL," +
                                        "MULTISTEPRETURN varchar2(1) NULL," +
                                        "SENDTO_NAME varchar2(30) NULL," +
                                        "ATTACHMENTS varchar2(255) NULL," +
                                        "CREATE_TIME varchar2(50) NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOLIST table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOLIST(LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not create INDEX LISTID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX SENDTOID ON SYS_TODOLIST(SENDTO_ID, SENDTO_KIND)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not create INDEX SENDTOID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX FLOWDESC ON SYS_TODOLIST(FLOW_DESC)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not create INDEX FLOWDESC on table SYS_TODOLIST.\n\r";
                    }

                    // SYS_FLDefinition
                    command.CommandText = "CREATE TABLE SYS_FLDEFINITION"
                                            + "("
                                            + "FLTYPEID varchar2(50) NOT NULL, "
                                            + "FLTYPENAME varchar2(200) NOT NULL, "
                                            + "FLDEFINITION blob NOT NULL, "
                                            + "VERSION int NULL,"
                                            + "PRIMARY KEY(FLTYPEID)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLDEFINITION table.\n\r";
                    }

                    // SYS_FLInstanceState
                    command.CommandText = "CREATE TABLE SYS_FLINSTANCESTATE"
                                            + "("
                                            + "FLINSTANCEID varchar2(50) NOT NULL, "
                                            + "STATE blob NOT NULL, "
                                            + "STATUS int NULL, "
                                            + "INFO varchar2(200) NULL,"
                                            + "PRIMARY KEY(FLINSTANCEID)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLINSTANCESTATE table.\n\r";
                    }


                    // Sys_ExtApprove
                    command.CommandText = "CREATE TABLE SYS_EXTAPPROVE"
                                            + "("
                                            + "APPROVEID varchar2(50) NULL, "
                                            + "GROUPID varchar2(50) NULL, "
                                            + "MINIMUM varchar2(50) NULL, "
                                            + "MAXIMUM varchar2(50) NULL,"
                                            + "ROLEID varchar2(50) NULL"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_EXTAPPROVE table.\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEPCloud)
                {
                    #region EEPCloud
                    //[SYS_SDALIAS]
                    command.CommandText = "CREATE TABLE SYS_SDALIAS(" +
                                           "    USERID varchar2(20) NOT NULL,    " +
                                           "    ALIASNAME varchar2(30) NOT NULL, " +
                                           "    SYSTEMALIAS varchar2(30) NULL,   " +
                                           "    DBNAME varchar2(25) NOT NULL,    " +
                                           "    SPLIT number NULL,                 " +
                                           " CONSTRAINT PK_SYS_SDALIAS PRIMARY KEY " +
                                           "(                                                  " +
                                           "    USERID ,                                  " +
                                           "    ALIASNAME ,                               " +
                                           "    DBNAME                                    " +
                                           ")" +
                                           ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDALIAS table.\n\r";
                    }

                    //[SYS_SDGROUPS]
                    command.CommandText = "CREATE TABLE SYS_SDGROUPS(" +
                                          "    GROUPID varchar2(20) NOT NULL,               " +
                                          "    GROUPNAME varchar2(30) NULL,                 " +
                                          " CONSTRAINT PK_SYS_SDGROUPS PRIMARY KEY " +
                                          "(                                                   " +
                                          "    GROUPID                                   " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDGROUPS table.\n\r";
                    }

                    //[SYS_SDQUEUE]
                    command.CommandText = "CREATE TABLE SYS_SDQUEUE(" +
                                          "    ID int NOT NULL,                           " +
                                          "    USERID varchar2(30) NOT NULL,               " +
                                          "    PAGETYPE char(1) NOT NULL,                 " +
                                          "    CREATETIME date NULL,                  " +
                                          "    FINISHTIME date NULL,                  " +
                                          "    FINISHFLAG number NOT NULL,                   " +
                                          "    DOCUMENT blob NULL,              " +
                                          "    FILENAME varchar2(40) NULL,                 " +
                                          "    PRINTSETTING varchar2(40) NULL,             " +
                                          " CONSTRAINT PK_SYS_SDQUEUE PRIMARY KEY " +
                                          "(                                                  " +
                                          "    ID                                       " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDQUEUE table.\n\r";
                    }

                    //[SYS_SDQUEUEPAGE]
                    command.CommandText = "CREATE TABLE SYS_SDQUEUEPAGE(" +
                                          "    ID int NOT NULL,                                  " +
                                          "    DOCUMENT blob NOT NULL,                 " +
                                          "    PHOTO blob NULL,                        " +
                                          "    PAGENAME varchar2(30) NOT NULL,                    " +
                                          "    CONSTRAINT PK_SYS_SDQUEUEPAGE PRIMARY KEY " +
                                          "(                                                         " +
                                          "    ID ,                                             " +
                                          "    PAGENAME                                         " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "ALTER TABLE SYS_SDQUEUEPAGE  WITH CHECK ADD  CONSTRAINT FK_SYS_SDQUEUEPAGE_SYS_SDQUEUE FOREIGN KEY(ID) " +
                                              "REFERENCES SYS_SDQUEUE (ID)";
                        command.ExecuteNonQuery();

                        command.CommandText = "ALTER TABLE SYS_SDQUEUEPAGE CHECK CONSTRAINT FK_SYS_SDQUEUEPAGE_SYS_SDQUEUE";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDQUEUEPAGE table.\n\r";
                    }

                    //[SYS_SDSOLUTIONS]
                    command.CommandText = "CREATE TABLE SYS_SDSOLUTIONS(" +
                                          "    USERID varchar2(20) NOT NULL,                   " +
                                          "    SOLUTIONID varchar2(30) NOT NULL,               " +
                                          "    SOLUTIONNAME nvarchar2(30) NULL,                " +
                                          "    MOUDLEXMLTEXT nvarchar2(max) NOT NULL,          " +
                                          "    SETTING blob NULL,                   " +
                                          "    ALIASOPTIONS varchar2(250) NULL,                " +
                                          "    LOGONIMAGE blob NULL,                " +
                                          "    BGSTARTCOLOR varchar2(9) NULL,                  " +
                                          "    BGENDCOLOR varchar2(9) NULL,                    " +
                                          "    THEME varchar2(20) NULL,                        " +
                                          "    COMPANY nvarchar2(30) NULL,                     " +
                                          "    PAGESAVEOPTION int NULL,                       " +
                                          " CONSTRAINT PK_SYS_SDSOLUTIONS PRIMARY KEY " +
                                          "(                                                      " +
                                          "    USERID ,                                      " +
                                          "    SOLUTIONID                                    " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDSOLUTIONS table.\n\r";
                    }

                    //[SYS_SDUSERS]
                    command.CommandText = "CREATE TABLE SYS_SDUSERS(" +
                                          "    USERID varchar2(20) NOT NULL,               " +
                                          "    USERNAME nvarchar2(30) NULL,                " +
                                          "    PASSWORD varchar2(20) NULL,                 " +
                                          "    GROUPID varchar2(20) NULL,                  " +
                                          "    LASTDATE date NULL,                    " +
                                          "    EMAIL nvarchar2(50) NULL,                   " +
                                          "    SYSTYPE nvarchar2(1) NULL,                  " +
                                          " CONSTRAINT PK_SYS_SDUSERS PRIMARY KEY " +
                                          "(                                                  " +
                                          "    USERID                                    " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDUSERS table.\n\r";
                    }

                    //[SYS_SDUSERS_LOG] iii
                    command.CommandText = "CREATE TABLE SYS_SDUSERS_LOG(" +
                                          "    ID int NOT NULL,                 " +
                                          "    USERID varchar2(20) NOT NULL,                   " +
                                          "    IPADDRESS varchar2(20) NOT NULL,                " +
                                          "    LOGINTIME date NOT NULL,                   " +
                                          "    LOGOUTTIME date NOT NULL,                  " +
                                          " CONSTRAINT PK_SYS_SDUSERS_LOG PRIMARY KEY " +
                                          "(                                                      " +
                                          "    ID                                            " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDUSERS_LOG table.\n\r";
                    }

                    //[SYS_WEBPAGES]
                    command.CommandText = "CREATE TABLE SYS_WEBPAGES(" +
                                          "    PAGENAME varchar2(30) NOT NULL,              " +
                                          "    PAGETYPE char(1) NOT NULL,                  " +
                                          "    DESCRIPTION varchar2(60) NULL,               " +
                                          "    CONTENT blob NULL,                " +
                                          "    USERID varchar2(20) NOT NULL,                " +
                                          "    SOLUTIONID varchar2(30) NOT NULL,            " +
                                          "    SERVERDLL blob NULL,              " +
                                          "    CHECKOUT number NULL,                          " +
                                          "    CHECKOUTDATE date NULL,                 " +
                                          "    CHECKOUTUSER varchar2(20) NULL,              " +
                                          " CONSTRAINT PK_SYS_WEBPAGES PRIMARY KEY " +
                                          "(                                                   " +
                                          "    PAGENAME ,                                 " +
                                          "    PAGETYPE ,                                 " +
                                          "    USERID ,                                   " +
                                          "    SOLUTIONID                                 " +
                                          ")" +
                                          ") ";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_WEBPAGES table.\n\r";
                    }

                    //[SYS_WEBPAGES_LOG] iii
                    command.CommandText = "CREATE TABLE SYS_WEBPAGES_LOG(" +
                                          "    PAGENAME varchar2(30) NOT NULL,                  " +
                                          "    PAGETYPE char(1) NOT NULL,                      " +
                                          "    DESCRIPTION varchar2(60) NULL,                   " +
                                          "    CONTENT blob NULL,                    " +
                                          "    USERID varchar2(20) NOT NULL,                    " +
                                          "    SOLUTIONID varchar2(30) NOT NULL,                " +
                                          "    SERVERDLL blob NULL,                  " +
                                          "    CHECKINDATE date NOT NULL,                  " +
                                          "    ID int NOT NULL,                  " +
                                          "    CHECKINUSER varchar2(20) NULL,                   " +
                                          "    CHECKINDESCRIPTION varchar2(255) NULL,           " +
                                          " CONSTRAINT PK_SYS_WEBPAGES_LOG PRIMARY KEY " +
                                          "(                                                       " +
                                          "    ID                                             " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_WEBPAGES_LOG table.\n\r";
                    }

                    //[SYS_WEBRUNTIME]
                    command.CommandText = "CREATE TABLE SYS_WEBRUNTIME(" +
                                          "    PAGENAME varchar2(30) NOT NULL,                " +
                                          "    PAGETYPE char(1) NOT NULL,                    " +
                                          "    CONTENT blob NULL,                  " +
                                          "    USERID varchar2(20) NOT NULL,                  " +
                                          "    SOLUTIONID varchar2(30) NOT NULL,              " +
                                          " CONSTRAINT PK_SYS_WEBRUNTIME PRIMARY KEY " +
                                          "(                                                     " +
                                          "    PAGENAME ,                                   " +
                                          "    PAGETYPE ,                                   " +
                                          "    USERID ,                                     " +
                                          "    SOLUTIONID                                   " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_WEBRUNTIME table.\n\r";
                    }
                    #endregion
                }

                if (result != "")
                    MessageBox.Show(result);
                else if (cdb.x == radioType.typical)
                    MessageBox.Show("Create all System Tables successfully.");
                else if (cdb.x == radioType.simplified)
                    MessageBox.Show("Create table COLDEF table and table SYSAUTONUM successfully.");
                else if (cdb.x == radioType.EEP7m)
                    MessageBox.Show("Alter successfully.");
                else if (cdb.x == radioType.EEP2006m)
                    MessageBox.Show("EEP 2006 2.1.0.1(SP2) Migration success");
                else if (cdb.x == radioType.WorkFlow)
                    MessageBox.Show("Create WorkFlow Tables successfully.");
                else if (cdb.x == radioType.EEPCloud)
                    MessageBox.Show("Create EEPCloud system tables successfully.");
            }
        }

        public void CreateSybaseSystemTable(IDbCommand command)
        {
            frmCreateDB cdb = new frmCreateDB();
            cdb.ShowDialog();

            if (cdb.DialogResult == DialogResult.OK)
            {
                string result = "";

                if (cdb.x == radioType.simplified || cdb.x == radioType.typical)
                {
                    #region Simplifield
                    command.CommandText = "CREATE TABLE COLDEF"
                                                     + "("
                                                     + "TABLE_NAME nvarchar(20) NOT NULL, "
                                                     + "FIELD_NAME nvarchar(20) NOT NULL, "
                                                     + "SEQ NUMERIC(12,0) NULL, "
                                                     + "FIELD_TYPE nvarchar(20) NULL, "
                                                     + "IS_KEY nvarchar(1) NOT NULL, "
                                                     + "FIELD_LENGTH NUMERIC(12,0) NULL, "
                                                     + "CAPTION nvarchar(40) NULL, "
                                                     + "EDITMASK nvarchar(10) NULL, "
                                                     + "NEEDBOX nvarchar(13) NULL, "
                                                     + "CANREPORT nvarchar(1) NULL, "
                                                     + "EXT_MENUID nvarchar(20) NULL, "
                                                     + "FIELD_SCALE NUMERIC(12,0) NULL, "
                                                     + "DD_NAME nvarchar(40) NULL, "
                                                     + "DEFAULT_VALUE nvarchar(100) NULL, "
                                                     + "CHECK_NULL nvarchar(1) NULL, "
                                                     + "QUERYMODE nvarchar(20) NULL, "
                                                     + "CAPTION1 nvarchar(40) NULL, "
                                                     + "CAPTION2 nvarchar(40) NULL, "
                                                     + "CAPTION3 nvarchar(40) NULL, "
                                                     + "CAPTION4 nvarchar(40) NULL, "
                                                     + "CAPTION5 nvarchar(40) NULL, "
                                                     + "CAPTION6 nvarchar(40) NULL, "
                                                     + "CAPTION7 nvarchar(40) NULL, "
                                                     + "CAPTION8 nvarchar(40) NULL, "
                                                     + "PRIMARY KEY(TABLE_NAME,FIELD_NAME)"
                                                     + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create COLDEF table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX TABLENAME ON COLDEF (TABLE_NAME,FIELD_NAME)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX TABLENAME on table COLDEF .\n\r";
                    }

                    command.CommandText = "CREATE TABLE SYSAUTONUM "
                                            + "("
                                            + "AUTOID VARCHAR(20) NOT NULL, "
                                            + "FIXED VARCHAR(20) NOT NULL, "
                                            + "CURRNUM NUMERIC(10,0) NULL, "
                                            + "DESCRIPTION VARCHAR(50) NULL, "
                                            + "PRIMARY KEY (AUTOID,FIXED)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYSAUTONUM table.\n\r";
                    }
                    #endregion

                    #region Typical
                    if (cdb.x == radioType.typical)
                    {
                        //// Create GROUPFORMS
                        //command.CommandText = "CREATE TABLE GROUPFORMS ("
                        //    + "GROUPID nvarchar (20) NOT NULL ,"
                        //    + "PACKAGE_NAME nvarchar (50) NOT NULL ,"
                        //    + "FORM_NAME nvarchar (50) NULL ,"
                        //    + "PARENT_MENU nvarchar (50) NULL "
                        //    + ")";
                        //try
                        //{
                        //    command.ExecuteNonQuery();
                        //}
                        //catch
                        //{
                        //    result += "Can not create GROUPFORMS table.\n\r";
                        //}

                        // Create GROUPMENUS
                        command.CommandText = "CREATE TABLE GROUPMENUS ("
                            + "GROUPID varchar (20) NOT NULL ,"
                            + "MENUID nvarchar (30) NOT NULL, "
                            + "PRIMARY KEY(GROUPID,MENUID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUS table.\n\r";
                        }

                        // Create USERMENUS
                        command.CommandText = "CREATE TABLE USERMENUS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "MENUID nvarchar (30) NOT NULL, "
                            + "PRIMARY KEY(USERID,MENUID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUS table.\n\r";
                        }

                        // Create GROUPS
                        command.CommandText = "CREATE TABLE GROUPS ("
                            + "GROUPID varchar (20) NOT NULL ,"
                            + "GROUPNAME nvarchar (50) NULL ,"
                            + "DESCRIPTION nvarchar (100) NULL ,"
                            + "MSAD nvarchar (1) NULL, "
                            + "ISROLE char(1) NULL,"
                            + "PRIMARY KEY(GROUPID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('00', 'EveryOne', 'N')";
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('01', 'DEPARTMENT1', 'N')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPS table.\n\r";
                        }

                        // Create MENUITEMTYPE
                        command.CommandText = "CREATE TABLE MENUITEMTYPE ("
                            + "ITEMTYPE nvarchar (20) NOT NULL ,"
                            + "ITEMNAME nvarchar (20) NULL, "
                            + "DBALIAS nvarchar (50) NULL,"
                            + "PRIMARY KEY(ITEMTYPE)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUITEMTYPE(ITEMTYPE, ITEMNAME) VALUES('SOLUTION1', 'DEFAULT SOLUTION')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUITEMTYPE table.\n\r";
                        }

                        // Create MENUTABLE
                        command.CommandText = "CREATE TABLE MENUTABLE ("
                            + "MENUID nvarchar (30) NOT NULL ,"
                            + "CAPTION nvarchar (50) NOT NULL ,"
                            + "PARENT nvarchar (20) NULL ,"
                            + "PACKAGE nvarchar (60) NULL ,"
                            + "MODULETYPE nvarchar (1) NULL ,"
                            + "ITEMPARAM nvarchar (200) NULL ,"
                            + "FORM nvarchar (32) NULL ,"
                            + "ISSHOWMODAL nvarchar (1) NULL ,"
                            + "ITEMTYPE nvarchar (20) NULL ,"
                            + "SEQ_NO nvarchar (4) NULL,"
                            + "PACKAGEDATE DateTime NULL,"
                            + "IMAGE image NULL,"
                            + "OWNER nvarchar(20) NULL,"
                            + "ISSERVER nvarchar(1) NULL,"
                            + "VERSIONNO nvarchar(20) NULL,"
                            + "CHECKOUT nvarchar(20) NULL,"
                            + "CHECKOUTDATE datetime NULL,"
                            + "CAPTION0 nvarchar(50) NULL,"
                            + "CAPTION1 nvarchar(50) NULL,"
                            + "CAPTION2 nvarchar(50) NULL,"
                            + "CAPTION3 nvarchar(50) NULL,"
                            + "CAPTION4 nvarchar(50) NULL,"
                            + "CAPTION5 nvarchar(50) NULL,"
                            + "CAPTION6 nvarchar(50) NULL,"
                            + "CAPTION7 nvarchar(50) NULL,"
                            + "IMAGEURL nvarchar(100) NULL, "
                            + "PRIMARY KEY(MENUID)"
                            + ") ";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUTABLE(MENUID, CAPTION, ITEMTYPE, MODULETYPE) VALUES('0', 'ROOT', 'SOLUTION1', 'F')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLE table.\n\r";
                        }

                        // Create USERGROUPS
                        command.CommandText = "CREATE TABLE USERGROUPS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "GROUPID varchar (20) NOT NULL, "
                            + "PRIMARY KEY(USERID,GROUPID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERGROUPS(USERID, GROUPID) VALUES('001', '01')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERGROUPS table.\n\r";
                        }

                        // Create USERS
                        command.CommandText = "CREATE TABLE USERS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "USERNAME nvarchar (30) NULL ,"
                            + "AGENT nvarchar (20) NULL ,"
                            + "PWD nvarchar (10) NULL ,"
                            + "CREATEDATE nvarchar (8) NULL ,"
                            + "CREATER nvarchar (20) NULL ,"
                            + "DESCRIPTION nvarchar (100) NULL ,"
                            + "EMAIL nvarchar (40) NULL ,"
                            + "LASTTIME nvarchar (8) NULL ,"
                            + "AUTOLOGIN nvarchar (1) NULL,"
                            + "LASTDATE nvarchar (8) NULL ,"
                            + "SIGNATURE nvarchar (30) NULL ,"
                            + "MSAD nvarchar (1) NULL, "
                            + "PRIMARY KEY(USERID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERS(USERID, USERNAME, PWD, MSAD, AUTOLOGIN) VALUES('001', 'TEST', '', 'N','S')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERS table.\n\r";
                        }

                        // Create MENUTABLELOG
                        command.CommandText = "CREATE TABLE MENUTABLELOG"
                            + "("
                            + "LOGID numeric(8,0) IDENTITY,"
                            + "MENUID nvarchar(30) not null,"
                            + "PACKAGE nvarchar(20) not null,"
                            + "PACKAGEDATE DATETIME,"
                            + "LASTDATE DATETIME,"
                            + "OWNER nvarchar(20),"
                            + "OLDVERSION IMAGE,"
                            + "OLDDATE nvarchar(20), "
                            + "PRIMARY KEY(LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLELOG table.\n\r";
                        }

                        // Create MENUCHECKLOG
                        command.CommandText = "CREATE TABLE MENUCHECKLOG"
                            + "("
                            + "LOGID numeric(8,0) IDENTITY,"
                            + "ITEMTYPE nvarchar(20) not null,"
                            + "PACKAGE nvarchar(50) not null,"
                            + "PACKAGEDATE DateTime,"
                            + "FILETYPE nvarchar(10),"
                            + "FILENAME nvarchar(60),"
                            + "FILEDATE DateTime,"
                            + "FILECONTENT IMAGE, "
                            + "PRIMARY KEY(LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUCHECKLOG table.\n\r";
                        }

                        // Create SYSEEPLOG
                        command.CommandText = "CREATE TABLE SYSEEPLOG"
                            + "("
                            + "CONNID nvarchar(20) NOT NULL,"
                            + "LOGID numeric(8,0) IDENTITY,"
                            + "LOGSTYLE nvarchar(1) NOT NULL,"
                            + "LOGDATETIME DATETIME NOT NULL,"
                            + "DOMAINID nvarchar(30) NULL,"
                            + "USERID varchar(20) NULL,"
                            + "LOGTYPE nvarchar(1) NULL,"
                            + "TITLE nvarchar(64) NULL,"
                            + "DESCRIPTION nvarchar(128) NULL,"
                            + "COMPUTERIP nvarchar(16) NULL,"
                            + "COMPUTERNAME nvarchar(64) NULL,"
                            + "EXECUTIONTIME INT NULL, "
                            + "PRIMARY KEY(CONNID,LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSEEPLOG table.\n\r";
                        }

                        // Create SYSSQLLOG
                        command.CommandText = "CREATE TABLE SYSSQLLOG("
                             + "LOGSTYLE nvarchar(1),"
                             + "LOGDATETIME datetime,"
                             + "USERID varchar(20),"
                             + "DEVELOPERID varchar(20),"
                             + "DESCRIPTION text,"
                             + "SQLSENTENCE nvarchar(max)"
                             + ");";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSSQLLOG table.\n\r";
                        }

                        // Create SYSERRLOG
                        command.CommandText = "CREATE TABLE SYSERRLOG"
                            + "("
                            + "ERRID numeric(8,0) IDENTITY,"
                            + "USERID varchar(20), "
                            + "MODULENAME nvarchar(30),"
                            + "ERRMESSAGE nvarchar(255),"
                            + "ERRSTACK text,"
                            + "ERRDESCRIP nvarchar(255),"
                            + "ERRDATE DateTime,"
                            + "ERRSCREEN Image,"
                            + "OWNER nvarchar(20),"
                            + "PROCESSDATE DateTime,"
                            + "PRODESCRIP nvarchar(255),"
                            + "STATUS nvarchar(2), "
                            + "PRIMARY KEY(ERRID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSERRLOG table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_LANGUAGE"
                            + "("
                            + "ID numeric(8,0) IDENTITY,"
                            + "IDENTIFICATION nvarchar(80),"
                            + "KEYS nvarchar(80),"
                            + "EN nvarchar(80),"
                            + "CHT nvarchar(80),"
                            + "CHS nvarchar(80),"
                            + "HK nvarchar(80),"
                            + "JA nvarchar(80),"
                            + "KO nvarchar(80),"
                            + "LAN1 nvarchar(80),"
                            + "LAN2 nvarchar(80),"
                            + "PRIMARY KEY(ID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_LANGUAGE table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_MESSENGER"
                            + "("
                            + "USERID varchar(20) NOT NULL,"
                            + "MESSAGE nvarchar(255),"
                            + "PARAS nvarchar(255),"
                            + "SENDTIME nvarchar(14),"
                            + "SENDERID nvarchar(20),"
                            + "RECTIME nvarchar(14),"
                            + "STATUS char(1)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_MESSENGER table.\n\r";
                        }

                        //CreateMenuTableControl
                        command.CommandText = "CREATE TABLE MENUTABLECONTROL"
                                                + "("
                                                + "MENUID varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "DESCRIPTION Varchar (50) NULL, "
                                                + "TYPE Varchar (20) NULL, "
                                                + "PRIMARY KEY(MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLECONTROL table.\n\r";
                        }

                        //CreateGroupMenuControl
                        command.CommandText = "CREATE TABLE GROUPMENUCONTROL"
                                                + "("
                                                + "GROUPID Varchar (20) NOT NULL, "
                                                + "MENUID Varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "TYPE Varchar (20) NULL, "
                                                + "ENABLED CHAR (1) NULL, "
                                                + "VISIBLE CHAR (1) NULL, "
                                                + "ALLOWADD CHAR (1) NULL, "
                                                + "ALLOWUPDATE CHAR (1) NULL, "
                                                + "ALLOWDELETE CHAR (1) NULL, "
                                                + "ALLOWPRINT CHAR (1) NULL, "
                                                + "PRIMARY KEY(GROUPID,MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUCONTROL table.\n\r";
                        }

                        //CreateUserMenuControl
                        command.CommandText = "CREATE TABLE USERMENUCONTROL"
                                                + "("
                                                + "USERID Varchar (20) NOT NULL, "
                                                + "MENUID Varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "TYPE Varchar (20) NULL, "
                                                + "ENABLED CHAR (1) NULL, "
                                                + "VISIBLE CHAR (1) NULL, "
                                                + "ALLOWADD CHAR (1) NULL, "
                                                + "ALLOWUPDATE CHAR (1) NULL, "
                                                + "ALLOWDELETE CHAR (1) NULL, "
                                                + "ALLOWPRINT CHAR (1) NULL, "
                                                + "PRIMARY KEY(USERID,MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUCONTROL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL"
                                            + "("
                                            + "REFVAL_NO Varchar(100) NOT NULL, "
                                            + "DESCRIPTION Varchar(250), "
                                            + "TABLE_NAME Varchar(100), "
                                            + "CAPTION Varchar(100), "
                                            + "DISPLAY_MEMBER Varchar(100), "
                                            + "SELECT_ALIAS Varchar(250), "
                                            + "SELECT_COMMAND Varchar(250), "
                                            + "VALUE_MEMBER Varchar(100), "
                                            + "PRIMARY KEY(REFVAL_NO) "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL_D1"
                                            + "("
                                            + "REFVAL_NO Varchar(30) NOT NULL, "
                                            + "FIELD_NAME Varchar(30) NOT NULL, "
                                            + "HEADER_TEXT Varchar(20), "
                                            + "WIDTH numeric(4,0), "
                                            + "PRIMARY KEY(REFVAL_NO, FIELD_NAME) "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL_D1 table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE MENUFAVOR"
                                            + "("
                                            + "MENUID nVarchar(30) NOT NULL, "
                                            + "CAPTION nVarchar(50) NOT NULL, "
                                            + "USERID Varchar(20) NOT NULL, "
                                            + "ITEMTYPE nVarchar(20), "
                                            + "GROUPNAME nVarChar(20), "
                                            + "PRIMARY Key (MENUID,USERID)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUFAVOR table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_ANYQUERY"
                                            + "("
                                            + "QUERYID Varchar(20) NOT NULL, "
                                            + "USERID Varchar(20) NOT NULL, "
                                            + "TEMPLATEID Varchar(20) NOT NULL, "
                                            + "TABLENAME Varchar(50), "
                                            + "LASTDATE datetime, "
                                            + "CONTENT text, "
                                            + "PRIMARY Key (QUERYID,USERID,TEMPLATEID)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_ANYQUERY table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REPORT"
                                            + "("
                                            + "REPORTID nVarchar(50) NOT NULL, "
                                            + "FILENAME nVarchar(50) NOT NULL, "
                                            + "REPORTNAME nVarchar(50), "
                                            + "DESCRIPTION nVarchar(50), "
                                            + "FILEPATH nVarchar(50), "
                                            + "OUTPUTMODE nVarchar(20), "
                                            + "HEADERREPEAT nVarchar(5), "
                                            + "HEADERFONT image, "
                                            + "HEADERITEMS image, "
                                            + "FOOTERFONT image, "
                                            + "FOOTERITEMS image, "
                                            + "FIELDFONT image, "
                                            + "FIELDITEMS image, "
                                            + "SETTING image, "
                                            + "FORMAT image, "
                                            + "PARAMETERS image, "
                                            + "IMAGES image, "
                                            + "MAILSETTING image, "
                                            + "DATASOURCE_PROVIDER nVarchar(50),"
                                            + "DATASOURCES image,"
                                            + "CLIENT_QUERY image,"
                                            + "REPORT_TYPE nVarchar(1),"
                                            + "TEMPLATE_DESC nVarchar(50),"
                                            + "PRIMARY Key (REPORTID,FILENAME)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REPORT table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_PERSONAL"
                                           + "("
                                           + "FORMNAME NVARCHAR(60) NOT NULL,"
                                           + "COMPNAME NVARCHAR(30) NOT NULL,"
                                           + "USERID NVARCHAR(20) NOT NULL,"
                                           + "REMARK NVARCHAR(30),"
                                           + "PROPCONTENT TEXT,"
                                           + "CREATEDATE DATETIME,"
                                           + "PRIMARY KEY (FORMNAME,COMPNAME,USERID)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_PERSONAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_EEP_USERS"
                                           + "("
                                           + "USERID NVARCHAR(50) NOT NULL,"
                                           + "USERNAME NVARCHAR(50) NULL,"
                                           + "COMPUTER NVARCHAR(50) NOT NULL,"
                                           + "LOGINTIME NVARCHAR(50),"
                                           + "LASTACTIVETIME NVARCHAR(50),"
                                           + "LOGINCOUNT INT,"
                                           + "PRIMARY KEY (USERID,COMPUTER)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_EEP_USERS table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE UserDevices"
                                           + "("
                                           + "UserID varchar (50) NULL,"
                                           + "UUID varchar (50) NULL,"
                                           + "Active varchar (1) NULL,"
                                           + "CreateDate datetime,"
                                           + "LoginDate datetime,"
                                           + "ExpiryDate datetime,"
                                           + "RegID nvarchar (255),"
                                           + "TokenID nvarchar (255)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create UserDevices table.\n\r";
                        }
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP7m)
                {
                    #region EEP7m
                    command.CommandText = "Alter Table COLDEF add "
                                                      + "DEFAULT_VALUE NVARCHAR(100) NULL, "
                                                      + "CHECK_NULL NVARCHAR(1) NULL, "
                                                      + "QUERYMODE NVARCHAR(20) NULL, "
                                                      + "CAPTION1 NVARCHAR (40), "
                                                      + "CAPTION2 NVARCHAR (40), "
                                                      + "CAPTION3 NVARCHAR (40), "
                                                      + "CAPTION4 NVARCHAR (40), "
                                                      + "CAPTION5 NVARCHAR (40), "
                                                      + "CAPTION6 NVARCHAR (40), "
                                                      + "CAPTION7 NVARCHAR (40), "
                                                      + "CAPTION8 NVARCHAR (40)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPMENUS alter column MENUID nvarchar (30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table GROUPS add MSAD NVARCHAR (1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS alter column GROUPNAME nvarchar (50)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS alter column DESCRIPTION nvarchar (200)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table MENUTABLE add "
                                           + "MODULETYPE NVARCHAR(1), "
                                           + "PACKAGEDATE DATETIME, "
                                           + "IMAGE IMAGE, "
                                           + "OWNER NVARCHAR(20), "
                                           + "ISSERVER NVARCHAR(1), "
                                           + "VERSIONNO NVARCHAR(20), "
                                           + "CHECKOUT NVARCHAR(20), "
                                           + "CHECKOUTDATE DATETIME, "
                                           + "CAPTION0 NVARCHAR(50), "
                                           + "CAPTION1 NVARCHAR(50), "
                                           + "CAPTION2 NVARCHAR(50), "
                                           + "CAPTION3 NVARCHAR(50), "
                                           + "CAPTION4 NVARCHAR(50), "
                                           + "CAPTION5 NVARCHAR(50), "
                                           + "CAPTION6 NVARCHAR(50), "
                                           + "CAPTION7 NVARCHAR(50), "
                                           + "IMAGEURL NVARCHAR(100)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE alter column MENUID nvarchar(30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE alter column ITEMTYPE nvarchar(20)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table USERMENUS alter column MENUID nvarchar(30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table USERS add "
                                           + "EMAIL NVARCHAR(40), "
                                           + "LASTTIME NVARCHAR(8), "
                                           + "AUTOLOGIN NVARCHAR(1), "
                                           + "LASTDATE NVARCHAR(8), "
                                           + "SIGNATURE NVARCHAR(30), "
                                           + "MSAD NVARCHAR(1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP2006m)
                {
                    #region EEP2006m
                    command.CommandText = "if exists (select * from sysobjects where id = object_id(N'[MENUFAVOR]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"
                                            + "Alter Table MENUFAVOR ADD GROUPNAME NVARCHAR(20)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "if exists (select * from sysobjects where id = object_id(N'[SYS_ORGLEVEL]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"
                                            + "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "if exists (select * from sysobjects where id = object_id(N'[SYS_TODOLIST]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"
                                            + "ALTER table SYS_TODOLIST  ADD  ATTACHMENTS nVarchar(255)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "if exists (select * from sysobjects where id = object_id(N'[SYS_TODOHIS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"
                                            + "ALTER table SYS_TODOHIS  ADD  ATTACHMENTS nVarchar(255)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.WorkFlow)
                {
                    #region WorkFlow
                    //GROUPS
                    command.CommandText = "ALTER TABLE GROUPS ADD ISROLE CHAR(1) NULL";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not add ISROLE column to GROUPS table.\n\r";
                    }

                    // SYS_ORG
                    command.CommandText = "CREATE TABLE SYS_ORG(" +
                                        "ORG_NO nvarchar(8) NOT NULL," +
                                        "ORG_DESC nvarchar(40) NOT NULL," +
                                        "ORG_KIND nvarchar(4) NOT NULL," +
                                        "UPPER_ORG nvarchar(8) NULL," +
                                        "ORG_MAN nvarchar(20) NOT NULL," +
                                        "LEVEL_NO nvarchar(6) NOT NULL," +
                                        "ORG_TREE nvarchar(40) NULL," +
                                        "END_ORG nvarchar(4) NULL," +
                                        "ORG_FULLNAME nvarchar(254) NULL," +
                                        "PRIMARY KEY(ORG_NO)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORG(ORG_NO,ORG_DESC,ORG_KIND,ORG_MAN,LEVEL_NO) Values ('1','總公司','0','001','9')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORG table.\n\r";
                    }

                    // SYS_ORGKIND
                    command.CommandText = "CREATE TABLE SYS_ORGKIND(" +
                                        "ORG_KIND nvarchar(4) NOT NULL," +
                                        "KIND_DESC nvarchar(40) NOT NULL," +
                                        "PRIMARY KEY(ORG_KIND)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORGKIND(ORG_KIND,KIND_DESC) Values ('0','公司組織')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGKIND table.\n\r";
                    }

                    // SYS_ORGLEVEL
                    command.CommandText = "CREATE TABLE SYS_ORGLEVEL(" +
                                        "LEVEL_NO nvarchar(6) NOT NULL," +
                                        "LEVEL_DESC nvarchar(40) NOT NULL," +
                                        "PRIMARY KEY(LEVEL_NO)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('1','主任/課長/副理')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('2','經理')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('3','副總')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('9','總經理')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGLEVEL table.\n\r";
                    }

                    // SYS_ORGROLES
                    command.CommandText = "CREATE TABLE SYS_ORGROLES(" +
                                        "ORG_NO nvarchar(8) NOT NULL," +
                                        "ROLE_ID varchar(20) NOT NULL," +
                                        "ORG_KIND nvarchar(4) NULL," +
                                        "PRIMARY KEY(ORG_NO,ROLE_ID)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGROLES table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ORGNO ON SYS_ORGROLES (ORG_NO, ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX ORGNO on table SYS_ORGROLES .\n\r";
                    }

                    // SYS_ROLES_AGENT
                    command.CommandText = "CREATE TABLE SYS_ROLES_AGENT(" +
                                        "ROLE_ID varchar(20) NOT NULL," +
                                        "AGENT nvarchar(20) NOT NULL," +
                                        "FLOW_DESC nvarchar(40) NOT NULL," +
                                        "START_DATE nvarchar(8) NOT NULL," +
                                        "START_TIME nvarchar(6) NULL," +
                                        "END_DATE nvarchar(8) NOT NULL," +
                                        "END_TIME nvarchar(6) NULL," +
                                        "PAR_AGENT nvarchar(4) NOT NULL," +
                                        "REMARK nvarchar(254) NULL," +
                                        "PRIMARY KEY(ROLE_ID,AGENT)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ROLES_AGENT table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ROLEID ON SYS_ROLES_AGENT (ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX ROLEID on table SYS_ROLES_AGENT .\n\r";
                    }

                    // SYS_TODOHIS
                    command.CommandText = "CREATE TABLE SYS_TODOHIS(" +
                                        "LISTID nvarchar(40) NOT NULL," +
                                        "FLOW_ID nvarchar(40) NOT NULL," +
                                        "FLOW_DESC nvarchar(40) NULL," +
                                        "ROLE_ID varchar(20) NOT NULL," +
                                        "S_ROLE_ID varchar(20) NOT NULL," +
                                        "S_STEP_ID nvarchar(20) NOT NULL," +
                                        "D_STEP_ID nvarchar(20) NOT NULL," +
                                        "S_STEP_DESC nvarchar(64) NULL," +
                                        "S_USER_ID nvarchar(20) NOT NULL," +
                                        "USER_ID nvarchar(20) NOT NULL," +
                                        "USERNAME nvarchar(30) NULL," +
                                        "FORM_NAME nvarchar(30) NULL," +
                                        "WEBFORM_NAME nvarchar(50) NOT NULL," +
                                        "S_USERNAME nvarchar(30) NULL," +
                                        "NAVIGATOR_MODE nvarchar(2) NOT NULL," +
                                        "FLNAVIGATOR_MODE nvarchar(2) NOT NULL," +
                                        "PARAMETERS nvarchar(254) NULL," +
                                        "STATUS nvarchar(4) NULL," +
                                        "PROC_TIME decimal(8, 2) NOT NULL," +
                                        "EXP_TIME decimal(8, 2) NOT NULL," +
                                        "TIME_UNIT nvarchar(4) NOT NULL," +
                                        "FLOWIMPORTANT varchar(1) NOT NULL," +
                                        "FLOWURGENT varchar(1) NOT NULL," +
                                        "FORM_TABLE nvarchar(30) NULL," +
                                        "FORM_KEYS nvarchar(254) NULL," +
                                        "FORM_PRESENTATION nvarchar(254) NULL," +
                                        "REMARK nvarchar(254) NULL," +
                                        "VERSION nvarchar(2) NULL," +
                                        "VDSNAME nvarchar(40) NULL," +
                                        "SENDBACKSTEP nvarchar(2) NULL," +
                                        "LEVEL_NO nvarchar(6) NULL," +
                                        "UPDATE_DATE nvarchar(10) NULL," +
                                        "UPDATE_TIME nvarchar(8) NULL," +
                                        "FORM_PRESENT_CT nvarchar(254) NULL," +
                                        "ATTACHMENTS nvarchar(255) NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOHIS table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOHIS (LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX LISTID on table SYS_TODOHIS.\n\r";
                    }

                    command.CommandText = "CREATE INDEX USERID ON SYS_TODOHIS (USER_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX USERID on table SYS_TODOHIS.\n\r";
                    }

                    // SYS_TODOLIST
                    command.CommandText = "CREATE TABLE SYS_TODOLIST(" +
                                        "LISTID nvarchar(40) NOT NULL," +
                                        "FLOW_ID nvarchar(40) NOT NULL," +
                                        "FLOW_DESC nvarchar(40) NULL," +
                                        "APPLICANT nvarchar(20) NOT NULL," +
                                        "S_USER_ID nvarchar(20) NOT NULL," +
                                        "S_STEP_ID nvarchar(20) NOT NULL," +
                                        "S_STEP_DESC nvarchar(64) NULL," +
                                        "D_STEP_ID nvarchar(20) NOT NULL," +
                                        "D_STEP_DESC nvarchar(64) NULL," +
                                        "EXP_TIME decimal(8, 2) NOT NULL," +
                                        "URGENT_TIME decimal(8, 2) NOT NULL," +
                                        "TIME_UNIT nvarchar(4) NOT NULL," +
                                        "USERNAME nvarchar(30) NULL," +
                                        "FORM_NAME nvarchar(30) NULL," +
                                        "NAVIGATOR_MODE nvarchar(2) NOT NULL," +
                                        "FLNAVIGATOR_MODE nvarchar(2) NOT NULL," +
                                        "PARAMETERS nvarchar(254) NULL," +
                                        "SENDTO_KIND nvarchar(4) NOT NULL," +
                                        "SENDTO_ID nvarchar(20) NOT NULL," +
                                        "FLOWIMPORTANT varchar(1) NOT NULL," +
                                        "FLOWURGENT nvarchar(1) NOT NULL," +
                                        "STATUS nvarchar(4) NULL," +
                                        "FORM_TABLE nvarchar(30) NULL," +
                                        "FORM_KEYS nvarchar(254) NULL," +
                                        "FORM_PRESENTATION nvarchar(254) NULL," +
                                        "FORM_PRESENT_CT nvarchar(254) NOT NULL," +
                                        "REMARK nvarchar(254) NULL," +
                                        "PROVIDER_NAME nvarchar(254) NULL," +
                                        "VERSION nvarchar(2) NULL," +
                                        "EMAIL_ADD nvarchar(40) NULL," +
                                        "EMAIL_STATUS varchar(1) NULL," +
                                        "VDSNAME nvarchar(40) NULL," +
                                        "SENDBACKSTEP nvarchar(2) NULL," +
                                        "LEVEL_NO nvarchar(6) NULL," +
                                        "WEBFORM_NAME nvarchar(50) NOT NULL," +
                                        "UPDATE_DATE nvarchar(10) NULL," +
                                        "UPDATE_TIME nvarchar(8) NULL," +
                                        "FLOWPATH nvarchar(100) NOT NULL," +
                                        "PLUSAPPROVE varchar(1) NOT NULL," +
                                        "PLUSROLES nvarchar(254) NOT NULL," +
                                        "MULTISTEPRETURN varchar(1) NULL," +
                                        "SENDTO_NAME nvarchar(30) NULL," +
                                        "ATTACHMENTS nvarchar(255) NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOLIST table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOLIST (LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX LISTID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX SENDTOID ON SYS_TODOLIST (SENDTO_ID, SENDTO_KIND)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX SENDTOID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX FLOWDESC ON SYS_TODOLIST (FLOW_DESC)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX FLOWDESC on table SYS_TODOLIST.\n\r";
                    }

                    // SYS_FLDefinition
                    command.CommandText = "CREATE TABLE SYS_FLDEFINITION"
                                            + "("
                                            + "FLTYPEID nvarchar(50) NOT NULL, "
                                            + "FLTYPENAME nvarchar(200) NOT NULL, "
                                            + "FLDEFINITION text NOT NULL, "
                                            + "VERSION int NULL, "
                                            + "PRIMARY KEY(FLTYPEID) "
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLDEFINITION table.\n\r";
                    }

                    // SYS_FLInstanceState
                    command.CommandText = "CREATE TABLE SYS_FLINSTANCESTATE"
                                            + "("
                                            + "FLINSTANCEID nvarchar(50) NOT NULL, "
                                            + "STATE image NOT NULL, "
                                            + "STATUS int NULL, "
                                            + "INFO nvarchar(200) NULL,"
                                            + "PRIMARY KEY(FLINSTANCEID)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLINSTANCESTATE table.\n\r";
                    }


                    // Sys_ExtApprove
                    command.CommandText = "CREATE TABLE SYS_EXTAPPROVE"
                                            + "("
                                            + "APPROVEID nvarchar(50) NULL, "
                                            + "GROUPID nvarchar(50) NULL, "
                                            + "MINIMUM nvarchar(50) NULL, "
                                            + "MAXIMUM nvarchar(50) NULL,"
                                            + "ROLEID nvarchar(50) NULL"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_EXTAPPROVE table.\n\r";
                    }

                    #endregion
                }

                if (result != "")
                    MessageBox.Show(result);
                else if (cdb.x == radioType.typical)
                    MessageBox.Show("Create all System Tables successfully.");
                else if (cdb.x == radioType.simplified)
                    MessageBox.Show("Create table COLDEF table and table SYSAUTONUM successfully.");
                else if (cdb.x == radioType.EEP7m)
                    MessageBox.Show("Alter successfully.");
                else if (cdb.x == radioType.EEP2006m)
                    MessageBox.Show("EEP 2006 2.1.0.1(SP2) Migration success");
                else if (cdb.x == radioType.WorkFlow)
                    MessageBox.Show("Create WorkFlow Tables successfully.");
            }
        }

        public void CreateSqlServerSystemTable(IDbCommand command)
        {
            frmCreateDB cdb = new frmCreateDB();
            cdb.ShowDialog();

            if (cdb.DialogResult == DialogResult.OK)
            {
                string result = "";

                if (cdb.x == radioType.simplified || cdb.x == radioType.typical)
                {
                    #region Simplifield
                    command.CommandText = "CREATE TABLE COLDEF"
                                                     + "("
                                                     + "TABLE_NAME nvarchar(20) NOT NULL, "
                                                     + "FIELD_NAME nvarchar(20) NOT NULL, "
                                                     + "SEQ NUMERIC(12,0) NULL, "
                                                     + "FIELD_TYPE nvarchar(20) NULL, "
                                                     + "IS_KEY nvarchar(1) NOT NULL, "
                                                     + "FIELD_LENGTH NUMERIC(12,0) NULL, "
                                                     + "CAPTION nvarchar(40) NULL, "
                                                     + "EDITMASK nvarchar(10) NULL, "
                                                     + "NEEDBOX nvarchar(13) NULL, "
                                                     + "CANREPORT nvarchar(1) NULL, "
                                                     + "EXT_MENUID nvarchar(20) NULL, "
                                                     + "FIELD_SCALE NUMERIC(12,0) NULL, "
                                                     + "DD_NAME nvarchar(40) NULL, "
                                                     + "DEFAULT_VALUE nvarchar(100) NULL, "
                                                     + "CHECK_NULL nvarchar(1) NULL, "
                                                     + "QUERYMODE nvarchar(20) NULL, "
                                                     + "CAPTION1 nvarchar(40) NULL, "
                                                     + "CAPTION2 nvarchar(40) NULL, "
                                                     + "CAPTION3 nvarchar(40) NULL, "
                                                     + "CAPTION4 nvarchar(40) NULL, "
                                                     + "CAPTION5 nvarchar(40) NULL, "
                                                     + "CAPTION6 nvarchar(40) NULL, "
                                                     + "CAPTION7 nvarchar(40) NULL, "
                                                     + "CAPTION8 nvarchar(40) NULL, "
                                                     + "PRIMARY KEY(TABLE_NAME,FIELD_NAME)"
                                                     + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create COLDEF table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX TABLENAME ON COLDEF (TABLE_NAME,FIELD_NAME)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX TABLENAME on table COLDEF .\n\r";
                    }

                    command.CommandText = "CREATE TABLE SYSAUTONUM "
                                            + "("
                                            + "AUTOID VARCHAR(20) NOT NULL, "
                                            + "FIXED VARCHAR(20) NOT NULL, "
                                            + "CURRNUM NUMERIC(10,0) NULL, "
                                            + "DESCRIPTION VARCHAR(50) NULL ,"
                                            + "PRIMARY KEY (AUTOID,FIXED)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYSAUTONUM table.\n\r";
                    }
                    #endregion

                    #region Typical
                    if (cdb.x == radioType.typical)
                    {
                        //// Create GROUPFORMS
                        //command.CommandText = "CREATE TABLE GROUPFORMS ("
                        //    + "GROUPID nvarchar (20) NOT NULL ,"
                        //    + "PACKAGE_NAME nvarchar (50) NOT NULL ,"
                        //    + "FORM_NAME nvarchar (50) NULL ,"
                        //    + "PARENT_MENU nvarchar (50) NULL "
                        //    + ")";
                        //try
                        //{
                        //    command.ExecuteNonQuery();
                        //}
                        //catch
                        //{
                        //    result += "Can not create GROUPFORMS table.\n\r";
                        //}

                        // Create GROUPMENUS
                        command.CommandText = "CREATE TABLE GROUPMENUS ("
                            + "GROUPID varchar (20) NOT NULL ,"
                            + "MENUID nvarchar (30) NOT NULL, "
                            + "PRIMARY KEY(GROUPID,MENUID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUS table.\n\r";
                        }

                        // Create USERMENUS
                        command.CommandText = "CREATE TABLE USERMENUS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "MENUID nvarchar (30) NOT NULL, "
                            + "PRIMARY KEY(USERID,MENUID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUS table.\n\r";
                        }

                        // Create GROUPS
                        command.CommandText = "CREATE TABLE GROUPS ("
                            + "GROUPID varchar (20) NOT NULL ,"
                            + "GROUPNAME nvarchar (50) NULL ,"
                            + "DESCRIPTION nvarchar (100) NULL ,"
                            + "MSAD nvarchar (1) NULL, "
                            + "ISROLE char(1) NULL,"
                            + "PRIMARY KEY(GROUPID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('00', 'EveryOne', 'N')";
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('01', 'DEPARTMENT1', 'N')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPS table.\n\r";
                        }

                        // Create MENUITEMTYPE
                        command.CommandText = "CREATE TABLE MENUITEMTYPE ("
                            + "ITEMTYPE nvarchar (20) NOT NULL ,"
                            + "ITEMNAME nvarchar (20) NULL, "
                            + "DBALIAS nvarchar (50) NULL, "
                            + "PRIMARY KEY(ITEMTYPE)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUITEMTYPE(ITEMTYPE, ITEMNAME) VALUES('SOLUTION1', 'DEFAULT SOLUTION')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUITEMTYPE table.\n\r";
                        }

                        // Create MENUTABLE
                        command.CommandText = "CREATE TABLE MENUTABLE ("
                            + "MENUID nvarchar (30) NOT NULL ,"
                            + "CAPTION nvarchar (50) NOT NULL ,"
                            + "PARENT nvarchar (20) NULL ,"
                            + "PACKAGE nvarchar (60) NULL ,"
                            + "MODULETYPE nvarchar (1) NULL ,"
                            + "ITEMPARAM nvarchar (200) NULL ,"
                            + "FORM nvarchar (32) NULL ,"
                            + "ISSHOWMODAL nvarchar (1) NULL ,"
                            + "ITEMTYPE nvarchar (20) NULL ,"
                            + "SEQ_NO nvarchar (4) NULL,"
                            + "PACKAGEDATE DateTime,"
                            + "[IMAGE] image NULL,"
                            + "OWNER nvarchar(20),"
                            + "ISSERVER nvarchar(1),"
                            + "VERSIONNO nvarchar(20),"
                            + "CHECKOUT nvarchar(20),"
                            + "CHECKOUTDATE datetime,"
                            + "CAPTION0 nvarchar(50) NULL,"
                            + "CAPTION1 nvarchar(50) NULL,"
                            + "CAPTION2 nvarchar(50) NULL,"
                            + "CAPTION3 nvarchar(50) NULL,"
                            + "CAPTION4 nvarchar(50) NULL,"
                            + "CAPTION5 nvarchar(50) NULL,"
                            + "CAPTION6 nvarchar(50) NULL,"
                            + "CAPTION7 nvarchar(50) NULL,"
                            + "IMAGEURL nvarchar(100) NULL, "
                            + "PRIMARY KEY(MENUID)"
                            + ") ";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUTABLE(MENUID, CAPTION, ITEMTYPE, MODULETYPE) VALUES('0', 'ROOT', 'SOLUTION1', 'F')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLE table.\n\r";
                        }

                        // Create USERGROUPS
                        command.CommandText = "CREATE TABLE USERGROUPS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "GROUPID varchar (20) NOT NULL, "
                            + "PRIMARY KEY(USERID,GROUPID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERGROUPS(USERID, GROUPID) VALUES('001', '01')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERGROUPS table.\n\r";
                        }

                        // Create USERS
                        command.CommandText = "CREATE TABLE USERS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "USERNAME nvarchar (30) NULL ,"
                            + "AGENT nvarchar (20) NULL ,"
                            + "PWD nvarchar (10) NULL ,"
                            + "CREATEDATE nvarchar (8) NULL ,"
                            + "CREATER nvarchar (20) NULL ,"
                            + "DESCRIPTION nvarchar (100) NULL ,"
                            + "EMAIL nvarchar (40) NULL ,"
                            + "LASTTIME nvarchar (8) NULL ,"
                            + "AUTOLOGIN nvarchar (1) NULL,"
                            + "LASTDATE nvarchar (8) NULL ,"
                            + "SIGNATURE nvarchar (30) NULL ,"
                            + "MSAD nvarchar (1) NULL, "
                            + "PRIMARY KEY(USERID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERS(USERID, USERNAME, PWD, MSAD, AUTOLOGIN) VALUES('001', 'TEST', '', 'N','S')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERS table.\n\r";
                        }

                        // Create MENUTABLELOG
                        command.CommandText = "CREATE TABLE MENUTABLELOG"
                            + "("
                            + "LOGID INT IDENTITY(1,1),"
                            + "MENUID nvarchar(30) not null,"
                            + "PACKAGE nvarchar(20) not null,"
                            + "PACKAGEDATE DATETIME,"
                            + "LASTDATE DATETIME,"
                            + "OWNER nvarchar(20),"
                            + "OLDVERSION IMAGE,"
                            + "OLDDATE nvarchar(20), "
                            + "PRIMARY KEY(LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLELOG table.\n\r";
                        }

                        // Create MENUCHECKLOG
                        command.CommandText = "CREATE TABLE MENUCHECKLOG"
                            + "("
                            + "LOGID int Identity(1, 1),"
                            + "ITEMTYPE nvarchar(20) not null,"
                            + "PACKAGE nvarchar(50) not null,"
                            + "PACKAGEDATE DateTime,"
                            + "FILETYPE nvarchar(10),"
                            + "FILENAME nvarchar(60),"
                            + "FILEDATE DateTime,"
                            + "FILECONTENT IMAGE, "
                            + "PRIMARY KEY(LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUCHECKLOG table.\n\r";
                        }

                        // Create SYSEEPLOG
                        command.CommandText = "CREATE TABLE SYSEEPLOG"
                            + "("
                            + "CONNID nvarchar(20) NOT NULL,"
                            + "LOGID INT IDENTITY(1,1) NOT NULL,"
                            + "LOGSTYLE nvarchar(1) NOT NULL,"
                            + "LOGDATETIME DATETIME NOT NULL,"
                            + "DOMAINID nvarchar(30) NULL,"
                            + "USERID varchar(20) NULL,"
                            + "LOGTYPE nvarchar(1) NULL,"
                            + "TITLE nvarchar(64) NULL,"
                            + "DESCRIPTION text NULL,"
                            + "COMPUTERIP nvarchar(16) NULL,"
                            + "COMPUTERNAME nvarchar(64) NULL,"
                            + "EXECUTIONTIME INT NULL, "
                            + "PRIMARY KEY(CONNID,LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSEEPLOG table.\n\r";
                        }

                        // Create SYSSQLLOG
                        command.CommandText = "CREATE TABLE SYSSQLLOG("
                             + "LOGSTYLE nvarchar(1),"
                             + "LOGDATETIME datetime,"
                             + "USERID varchar(20),"
                             + "DEVELOPERID varchar(20),"
                             + "DESCRIPTION text,"
                             + "SQLSENTENCE nvarchar(max)"
                             + ");";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSSQLLOG table.\n\r";
                        }

                        // Create SYSERRLOG
                        command.CommandText = "CREATE TABLE SYSERRLOG"
                            + "("
                            + "ERRID int identity(1, 1),"
                            + "USERID varchar(20), "
                            + "MODULENAME nvarchar(30),"
                            + "ERRMESSAGE nvarchar(255),"
                            + "ERRSTACK ntext,"
                            + "ERRDESCRIP nvarchar(255),"
                            + "ERRDATE DateTime,"
                            + "ERRSCREEN Image,"
                            + "OWNER nvarchar(20),"
                            + "PROCESSDATE DateTime,"
                            + "PRODESCRIP nvarchar(255),"
                            + "STATUS nvarchar(2), "
                            + "PRIMARY KEY(ERRID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSERRLOG table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_LANGUAGE"
                            + "("
                            + "ID int IDENTITY(1, 1) NOT NULL,"
                            + "IDENTIFICATION nvarchar(80),"
                            + "KEYS nvarchar(80),"
                            + "EN nvarchar(80),"
                            + "CHT nvarchar(80),"
                            + "CHS nvarchar(80),"
                            + "HK nvarchar(80),"
                            + "JA nvarchar(80),"
                            + "KO nvarchar(80),"
                            + "LAN1 nvarchar(80),"
                            + "LAN2 nvarchar(80),"
                            + " CONSTRAINT [PK_SYS_LANGUAGE] PRIMARY KEY CLUSTERED"
                            + "("
                            + "[ID] "
                            + ")  ON [PRIMARY]"
                            + ")  ON [PRIMARY]";

                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_LANGUAGE table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_MESSENGER"
                            + "("
                            + "USERID varchar(20) NOT NULL,"
                            + "MESSAGE nvarchar(255),"
                            + "PARAS nvarchar(255),"
                            + "SENDTIME nvarchar(14),"
                            + "SENDERID nvarchar(20),"
                            + "RECTIME nvarchar(14),"
                            + "STATUS char(1)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_MESSENGER table.\n\r";
                        }

                        //CreateMenuTableControl
                        command.CommandText = "CREATE TABLE MENUTABLECONTROL"
                                                + "("
                                                + "MENUID varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "DESCRIPTION Varchar (50) NULL, "
                                                + "TYPE Varchar (20) NULL, "
                                                + "PRIMARY KEY(MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLECONTROL table.\n\r";
                        }

                        //CreateGroupMenuControl
                        command.CommandText = "CREATE TABLE GROUPMENUCONTROL"
                                                + "("
                                                + "GROUPID Varchar (20) NOT NULL, "
                                                + "MENUID Varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "TYPE Varchar (20) NULL, "
                                                + "ENABLED CHAR (1) NULL, "
                                                + "VISIBLE CHAR (1) NULL, "
                                                + "ALLOWADD CHAR (1) NULL, "
                                                + "ALLOWUPDATE CHAR (1) NULL, "
                                                + "ALLOWDELETE CHAR (1) NULL, "
                                                + "ALLOWPRINT CHAR (1) NULL, "
                                                + "PRIMARY KEY(GROUPID,MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUCONTROL table.\n\r";
                        }

                        //CreateUserMenuControl
                        command.CommandText = "CREATE TABLE USERMENUCONTROL"
                                                + "("
                                                + "USERID Varchar (20) NOT NULL, "
                                                + "MENUID Varchar (30) NOT NULL, "
                                                + "CONTROLNAME Varchar (50) NOT NULL, "
                                                + "TYPE Varchar (20) NULL, "
                                                + "ENABLED CHAR (1) NULL, "
                                                + "VISIBLE CHAR (1) NULL, "
                                                + "ALLOWADD CHAR (1) NULL, "
                                                + "ALLOWUPDATE CHAR (1) NULL, "
                                                + "ALLOWDELETE CHAR (1) NULL, "
                                                + "ALLOWPRINT CHAR (1) NULL, "
                                                + "PRIMARY KEY(USERID,MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUCONTROL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL"
                                            + "("
                                            + "REFVAL_NO Varchar(100) Not NULL, "
                                            + "DESCRIPTION Varchar(250), "
                                            + "TABLE_NAME Varchar(100), "
                                            + "CAPTION Varchar(100), "
                                            + "DISPLAY_MEMBER Varchar(100), "
                                            + "SELECT_ALIAS Varchar(250), "
                                            + "SELECT_COMMAND Varchar(250), "
                                            + "VALUE_MEMBER Varchar(100), "
                                            + "CONSTRAINT PK_SYS_REFVAL PRIMARY KEY(REFVAL_NO) "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL_D1"
                                            + "("
                                            + "REFVAL_NO Varchar(30) Not NULL, "
                                            + "FIELD_NAME Varchar(30) Not NULL, "
                                            + "HEADER_TEXT Varchar(20), "
                                            + "WIDTH INT, "
                                            + "CONSTRAINT PK_SYS_REFVAL_D1 PRIMARY KEY(REFVAL_NO, FIELD_NAME) "
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL_D1 table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE MENUFAVOR"
                                            + "("
                                            + "MENUID nVarchar(30) Not NULL, "
                                            + "CAPTION nVarchar(50) Not NULL, "
                                            + "USERID Varchar(20), "
                                            + "ITEMTYPE nVarchar(20), "
                                            + "GROUPNAME nVarChar(20), "
                                            + "PRIMARY Key (MENUID,USERID)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUFAVOR table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_ANYQUERY"
                                            + "("
                                            + "QUERYID Varchar(20) Not NULL, "
                                            + "USERID Varchar(20) Not NULL, "
                                            + "TEMPLATEID Varchar(20), "
                                            + "TABLENAME Varchar(50), "
                                            + "LASTDATE datetime, "
                                            + "CONTENT text, "
                                            + "PRIMARY Key (QUERYID,USERID,TEMPLATEID)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_ANYQUERY table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REPORT"
                                            + "("
                                            + "REPORTID nVarchar(50) Not NULL, "
                                            + "FILENAME nVarchar(50) Not NULL, "
                                            + "REPORTNAME nVarchar(50), "
                                            + "DESCRIPTION nVarchar(50), "
                                            + "FILEPATH nVarchar(50), "
                                            + "OUTPUTMODE nVarchar(20), "
                                            + "HEADERREPEAT nVarchar(5), "
                                            + "HEADERFONT image, "
                                            + "HEADERITEMS image, "
                                            + "FOOTERFONT image, "
                                            + "FOOTERITEMS image, "
                                            + "FIELDFONT image, "
                                            + "FIELDITEMS image, "
                                            + "SETTING image, "
                                            + "FORMAT image, "
                                            + "PARAMETERS image, "
                                            + "IMAGES image, "
                                            + "MAILSETTING image, "
                                            + "DATASOURCE_PROVIDER nVarchar(50),"
                                            + "DATASOURCES image,"
                                            + "CLIENT_QUERY image,"
                                            + "REPORT_TYPE nVarchar(1),"
                                            + "TEMPLATE_DESC nVarchar(50),"
                                            + "PRIMARY Key (REPORTID,FILENAME)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REPORT table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_PERSONAL"
                                           + "("
                                           + "FORMNAME NVARCHAR(60) NOT NULL,"
                                           + "COMPNAME NVARCHAR(30) NOT NULL,"
                                           + "USERID NVARCHAR(20) NOT NULL,"
                                           + "REMARK NVARCHAR(30),"
                                           + "PROPCONTENT NTEXT,"
                                           + "CREATEDATE DATETIME,"
                                           + "PRIMARY KEY (FORMNAME,COMPNAME,USERID)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_PERSONAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_EEP_USERS"
                                           + "("
                                           + "USERID NVARCHAR(50) NOT NULL,"
                                           + "USERNAME NVARCHAR(50) NULL,"
                                           + "COMPUTER NVARCHAR(50) NOT NULL,"
                                           + "LOGINTIME NVARCHAR(50),"
                                           + "LASTACTIVETIME NVARCHAR(50),"
                                           + "LOGINCOUNT INT,"
                                           + "PRIMARY KEY (USERID,COMPUTER)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_EEP_USERS table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE UserDevices"
                                           + "("
                                           + "UserID varchar(50) NULL,"
                                           + "UUID varchar(50) NULL,"
                                           + "Active varchar(1) NULL,"
                                           + "CreateDate datetime,"
                                           + "LoginDate datetime,"
                                           + "ExpiryDate datetime,"
                                           + "RegID nvarchar(max),"
                                           + "TokenID nvarchar(max)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create UserDevices table.\n\r";
                        }
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP7m)
                {
                    #region EEP7m
                    command.CommandText = "Alter Table COLDEF add "
                                                      + "DEFAULT_VALUE NVARCHAR(100) NULL, "
                                                      + "CHECK_NULL NVARCHAR(1) NULL, "
                                                      + "QUERYMODE NVARCHAR(20) NULL, "
                                                      + "CAPTION1 NVARCHAR (40), "
                                                      + "CAPTION2 NVARCHAR (40), "
                                                      + "CAPTION3 NVARCHAR (40), "
                                                      + "CAPTION4 NVARCHAR (40), "
                                                      + "CAPTION5 NVARCHAR (40), "
                                                      + "CAPTION6 NVARCHAR (40), "
                                                      + "CAPTION7 NVARCHAR (40), "
                                                      + "CAPTION8 NVARCHAR (40)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPMENUS alter column MENUID nvarchar (30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table GROUPS add MSAD NVARCHAR (1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS alter column GROUPNAME nvarchar (50)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS alter column DESCRIPTION nvarchar (200)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table MENUTABLE add "
                                           + "MODULETYPE NVARCHAR(1), "
                                           + "PACKAGEDATE DATETIME, "
                                           + "IMAGE IMAGE, "
                                           + "OWNER NVARCHAR(20), "
                                           + "ISSERVER NVARCHAR(1), "
                                           + "VERSIONNO NVARCHAR(20), "
                                           + "CHECKOUT NVARCHAR(20), "
                                           + "CHECKOUTDATE DATETIME, "
                                           + "CAPTION0 NVARCHAR(50), "
                                           + "CAPTION1 NVARCHAR(50), "
                                           + "CAPTION2 NVARCHAR(50), "
                                           + "CAPTION3 NVARCHAR(50), "
                                           + "CAPTION4 NVARCHAR(50), "
                                           + "CAPTION5 NVARCHAR(50), "
                                           + "CAPTION6 NVARCHAR(50), "
                                           + "CAPTION7 NVARCHAR(50), "
                                           + "IMAGEURL NVARCHAR(100)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE alter column MENUID nvarchar(30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE alter column ITEMTYPE nvarchar(20)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table USERMENUS alter column MENUID nvarchar(30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table USERS add "
                                           + "EMAIL NVARCHAR(40), "
                                           + "LASTTIME NVARCHAR(8), "
                                           + "AUTOLOGIN NVARCHAR(1), "
                                           + "LASTDATE NVARCHAR(8), "
                                           + "SIGNATURE NVARCHAR(30), "
                                           + "MSAD NVARCHAR(1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP2006m)
                {
                    #region EEP2006m
                    command.CommandText = "if exists (select * from sysobjects where id = object_id(N'[MENUFAVOR]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"
                                            + "Alter Table MENUFAVOR ADD GROUPNAME NVARCHAR(20)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "if exists (select * from sysobjects where id = object_id(N'[SYS_ORGLEVEL]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"
                                            + "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "if exists (select * from sysobjects where id = object_id(N'[SYS_TODOLIST]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"
                                            + "ALTER table SYS_TODOLIST  ADD  ATTACHMENTS nVarchar(255)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "if exists (select * from sysobjects where id = object_id(N'[SYS_TODOHIS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"
                                            + "ALTER table SYS_TODOHIS  ADD  ATTACHMENTS nVarchar(255)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.WorkFlow)
                {
                    #region WorkFlow
                    //GROUPS
                    command.CommandText = "ALTER TABLE GROUPS ADD ISROLE CHAR(1) NULL";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not add ISROLE column to GROUPS table.\n\r";
                    }

                    // SYS_ORG
                    command.CommandText = "CREATE TABLE [SYS_ORG](" +
                                        "[ORG_NO] [nvarchar](8) NOT NULL," +
                                        "[ORG_DESC] [nvarchar](40) NOT NULL," +
                                        "[ORG_KIND] [nvarchar](4) NOT NULL," +
                                        "[UPPER_ORG] [nvarchar](8) NULL," +
                                        "[ORG_MAN] [nvarchar](20) NOT NULL," +
                                        "[LEVEL_NO] [nvarchar](6) NOT NULL," +
                                        "[ORG_TREE] [nvarchar](40) NULL," +
                                        "[END_ORG] [nvarchar](4) NULL," +
                                        "[ORG_FULLNAME] [nvarchar](254) NULL," +
                                        "PRIMARY KEY(ORG_NO)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORG(ORG_NO,ORG_DESC,ORG_KIND,ORG_MAN,LEVEL_NO) Values ('1',N'總公司','0','001','9')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORG table.\n\r";
                    }

                    // SYS_ORGKIND
                    command.CommandText = "CREATE TABLE [SYS_ORGKIND](" +
                                        "[ORG_KIND] [nvarchar](4) NOT NULL," +
                                        "[KIND_DESC] [nvarchar](40) NOT NULL," +
                                        "PRIMARY KEY(ORG_KIND)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORGKIND(ORG_KIND,KIND_DESC) Values ('0',N'公司組織')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGKIND table.\n\r";
                    }

                    // SYS_ORGLEVEL
                    command.CommandText = "CREATE TABLE [SYS_ORGLEVEL](" +
                                        "[LEVEL_NO] [nvarchar](6) NOT NULL," +
                                        "[LEVEL_DESC] [nvarchar](40) NOT NULL," +
                                        "PRIMARY KEY(LEVEL_NO)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0',N'直屬主管')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('1',N'主任/課長/副理')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('2',N'經理')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('3',N'副總')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('9',N'總經理')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGLEVEL table.\n\r";
                    }

                    // SYS_ORGROLES
                    command.CommandText = "CREATE TABLE [SYS_ORGROLES](" +
                                        "[ORG_NO] [nvarchar](8) NOT NULL," +
                                        "[ROLE_ID] [varchar](20) NOT NULL," +
                                        "[ORG_KIND] [nvarchar](4) NULL," +
                                        "PRIMARY KEY(ORG_NO,ROLE_ID)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGROLES table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ORGNO ON SYS_ORGROLES (ORG_NO, ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX ORGNO on table SYS_ORGROLES .\n\r";
                    }

                    // SYS_ROLES_AGENT
                    command.CommandText = "CREATE TABLE [SYS_ROLES_AGENT](" +
                                        "[ROLE_ID] [varchar](20) NOT NULL," +
                                        "[AGENT] [nvarchar](20) NOT NULL," +
                                        "[FLOW_DESC] [nvarchar](40) NOT NULL," +
                                        "[START_DATE] [nvarchar](8) NOT NULL," +
                                        "[START_TIME] [nvarchar](6) NULL," +
                                        "[END_DATE] [nvarchar](8) NOT NULL," +
                                        "[END_TIME] [nvarchar](6) NULL," +
                                        "[PAR_AGENT] [nvarchar](4) NOT NULL," +
                                        "[REMARK] [nvarchar](254) NULL," +
                                        "PRIMARY KEY(ROLE_ID,AGENT)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ROLES_AGENT table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ROLEID ON SYS_ROLES_AGENT (ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX ROLEID on table SYS_ROLES_AGENT .\n\r";
                    }

                    // SYS_TODOHIS
                    command.CommandText = "CREATE TABLE [SYS_TODOHIS](" +
                                        "[LISTID] [nvarchar](40) NOT NULL," +
                                        "[FLOW_ID] [nvarchar](40) NOT NULL," +
                                        "[FLOW_DESC] [nvarchar](40) NULL," +
                                        "[ROLE_ID] [varchar](20) NOT NULL," +
                                        "[S_ROLE_ID] [varchar](20) NOT NULL," +
                                        "[S_STEP_ID] [nvarchar](20) NOT NULL," +
                                        "[D_STEP_ID] [nvarchar](20) NOT NULL," +
                                        "[S_STEP_DESC] [nvarchar](64) NULL," +
                                        "[S_USER_ID] [nvarchar](20) NOT NULL," +
                                        "[USER_ID] [nvarchar](20) NOT NULL," +
                                        "[USERNAME] [nvarchar](30) NULL," +
                                        "[FORM_NAME] [nvarchar](30) NULL," +
                                        "[WEBFORM_NAME] [nvarchar](50) NOT NULL," +
                                        "[S_USERNAME] [nvarchar](30) NULL," +
                                        "[NAVIGATOR_MODE] [nvarchar](2) NOT NULL," +
                                        "[FLNAVIGATOR_MODE] [nvarchar](2) NOT NULL," +
                                        "[PARAMETERS] [nvarchar](254) NULL," +
                                        "[STATUS] [nvarchar](4) NULL," +
                                        "[PROC_TIME] [decimal](8, 2) NOT NULL," +
                                        "[EXP_TIME] [decimal](8, 2) NOT NULL," +
                                        "[TIME_UNIT] [nvarchar](4) NOT NULL," +
                                        "[FLOWIMPORTANT] [varchar](1) NOT NULL," +
                                        "[FLOWURGENT] [varchar](1) NOT NULL," +
                                        "[FORM_TABLE] [nvarchar](30) NULL," +
                                        "[FORM_KEYS] [nvarchar](254) NULL," +
                                        "[FORM_PRESENTATION] [nvarchar](254) NULL," +
                                        "[REMARK] [nvarchar](254) NULL," +
                                        "[VERSION] [nvarchar](2) NULL," +
                                        "[VDSNAME] [nvarchar](40) NULL," +
                                        "[SENDBACKSTEP] [nvarchar](2) NULL," +
                                        "[LEVEL_NO] [nvarchar](6) NULL," +
                                        "[UPDATE_DATE] [nvarchar](10) NULL," +
                                        "[UPDATE_TIME] [nvarchar](8) NULL," +
                                        "[FORM_PRESENT_CT] [nvarchar](254) NULL," +
                                        "[ATTACHMENTS] [nvarchar](255) NULL, " +
                                        "[CREATE_TIME] [nvarchar](50) NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOHIS table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOHIS (LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX LISTID on table SYS_TODOHIS.\n\r";
                    }

                    command.CommandText = "CREATE INDEX USERID ON SYS_TODOHIS (USER_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX USERID on table SYS_TODOHIS.\n\r";
                    }

                    // SYS_TODOLIST
                    command.CommandText = "CREATE TABLE [SYS_TODOLIST](" +
                                        "[LISTID] [nvarchar](40) NOT NULL," +
                                        "[FLOW_ID] [nvarchar](40) NOT NULL," +
                                        "[FLOW_DESC] [nvarchar](40) NULL," +
                                        "[APPLICANT] [nvarchar](20) NOT NULL," +
                                        "[S_USER_ID] [nvarchar](20) NOT NULL," +
                                        "[S_STEP_ID] [nvarchar](20) NOT NULL," +
                                        "[S_STEP_DESC] [nvarchar](64) NULL," +
                                        "[D_STEP_ID] [nvarchar](20) NOT NULL," +
                                        "[D_STEP_DESC] [nvarchar](64) NULL," +
                                        "[EXP_TIME] [decimal](8, 2) NOT NULL," +
                                        "[URGENT_TIME] [decimal](8, 2) NOT NULL," +
                                        "[TIME_UNIT] [nvarchar](4) NOT NULL," +
                                        "[USERNAME] [nvarchar](30) NULL," +
                                        "[FORM_NAME] [nvarchar](30) NULL," +
                                        "[NAVIGATOR_MODE] [nvarchar](2) NOT NULL," +
                                        "[FLNAVIGATOR_MODE] [nvarchar](2) NOT NULL," +
                                        "[PARAMETERS] [nvarchar](254) NULL," +
                                        "[SENDTO_KIND] [nvarchar](4) NOT NULL," +
                                        "[SENDTO_ID] [nvarchar](20) NOT NULL," +
                                        "[FLOWIMPORTANT] [varchar](1) NOT NULL," +
                                        "[FLOWURGENT] [nvarchar](1) NOT NULL," +
                                        "[STATUS] [nvarchar](4) NULL," +
                                        "[FORM_TABLE] [nvarchar](30) NULL," +
                                        "[FORM_KEYS] [nvarchar](254) NULL," +
                                        "[FORM_PRESENTATION] [nvarchar](254) NULL," +
                                        "[FORM_PRESENT_CT] [nvarchar](254) NOT NULL," +
                                        "[REMARK] [nvarchar](254) NULL," +
                                        "[PROVIDER_NAME] [nvarchar](254) NULL," +
                                        "[VERSION] [nvarchar](2) NULL," +
                                        "[EMAIL_ADD] [nvarchar](40) NULL," +
                                        "[EMAIL_STATUS] [varchar](1) NULL," +
                                        "[VDSNAME] [nvarchar](40) NULL," +
                                        "[SENDBACKSTEP] [nvarchar](2) NULL," +
                                        "[LEVEL_NO] [nvarchar](6) NULL," +
                                        "[WEBFORM_NAME] [nvarchar](50) NOT NULL," +
                                        "[UPDATE_DATE] [nvarchar](10) NULL," +
                                        "[UPDATE_TIME] [nvarchar](8) NULL," +
                                        "[FLOWPATH] [nvarchar](100) NOT NULL," +
                                        "[PLUSAPPROVE] [varchar](1) NOT NULL," +
                                        "[PLUSROLES] [nvarchar](254) NOT NULL," +
                                        "[MULTISTEPRETURN] [varchar](1) NULL," +
                                        "[SENDTO_NAME] [nvarchar](30) NULL," +
                                        "[ATTACHMENTS] [nvarchar](255) NULL, " +
                                        "[CREATE_TIME] [nvarchar](50) NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOLIST table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOLIST (LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX LISTID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX SENDTOID ON SYS_TODOLIST (SENDTO_ID, SENDTO_KIND)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX SENDTOID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX FLOWDESC ON SYS_TODOLIST (FLOW_DESC)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX FLOWDESC on table SYS_TODOLIST.\n\r";
                    }

                    // SYS_FLDefinition
                    command.CommandText = "CREATE TABLE SYS_FLDEFINITION"
                                            + "("
                                            + "FLTYPEID nvarchar(50) NOT NULL, "
                                            + "FLTYPENAME nvarchar(200) NOT NULL, "
                                            + "FLDEFINITION ntext NOT NULL, "
                                            + "VERSION int NULL, "
                                            + "CONSTRAINT PK_SYS_FL PRIMARY KEY(FLTYPEID) "
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLDEFINITION table.\n\r";
                    }

                    // SYS_FLInstanceState
                    command.CommandText = "CREATE TABLE SYS_FLINSTANCESTATE"
                                            + "("
                                            + "FLINSTANCEID nvarchar(50) NOT NULL, "
                                            + "STATE image NOT NULL, "
                                            + "STATUS int NULL, "
                                            + "INFO nvarchar(200) NULL,"
                                            + "PRIMARY KEY(FLINSTANCEID)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLINSTANCESTATE table.\n\r";
                    }


                    // Sys_ExtApprove
                    command.CommandText = "CREATE TABLE SYS_EXTAPPROVE"
                                            + "("
                                            + "APPROVEID nvarchar(50) NULL, "
                                            + "GROUPID nvarchar(50) NULL, "
                                            + "MINIMUM nvarchar(50) NULL, "
                                            + "MAXIMUM nvarchar(50) NULL,"
                                            + "ROLEID nvarchar(50) NULL"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_EXTAPPROVE table.\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEPCloud)
                {
                    #region EEPCloud
                    //[SYS_SDALIAS]
                    command.CommandText = "CREATE TABLE [SYS_SDALIAS](" +
                                           "    [USERID] [varchar](20) NOT NULL,    " +
                                           "    [ALIASNAME] [varchar](30) NOT NULL, " +
                                           "    [SYSTEMALIAS] [varchar](30) NULL,   " +
                                           "    [DBNAME] [varchar](25) NOT NULL,    " +
                                           "    [SPLIT] [bit] NULL,                 " +
                                           " CONSTRAINT [PK_SYS_SDALIAS] PRIMARY KEY CLUSTERED " +
                                           "(                                                  " +
                                           "    [USERID] ASC,                                  " +
                                           "    [ALIASNAME] ASC,                               " +
                                           "    [DBNAME] ASC                                   " +
                                           ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                                           ") ON [PRIMARY]";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDALIAS table.\n\r";
                    }

                    //[SYS_SDGROUPS]
                    command.CommandText = "CREATE TABLE [SYS_SDGROUPS](" +
                                          "    [GROUPID] [varchar](20) NOT NULL,               " +
                                          "    [GROUPNAME] [varchar](30) NULL,                 " +
                                          " CONSTRAINT [PK_SYS_SDGROUPS] PRIMARY KEY CLUSTERED " +
                                          "(                                                   " +
                                          "    [GROUPID] ASC                                   " +
                                          ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                                          ") ON [PRIMARY] ";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDGROUPS table.\n\r";
                    }

                    //[SYS_SDQUEUE]
                    command.CommandText = "CREATE TABLE [SYS_SDQUEUE](" +
                                          "    [ID] [int] NOT NULL,                           " +
                                          "    [USERID] [varchar](30) NOT NULL,               " +
                                          "    [PAGETYPE] [char](1) NOT NULL,                 " +
                                          "    [CREATETIME] [datetime] NULL,                  " +
                                          "    [FINISHTIME] [datetime] NULL,                  " +
                                          "    [FINISHFLAG] [bit] NOT NULL,                   " +
                                          "    [DOCUMENT] [varbinary](max) NULL,              " +
                                          "    [FILENAME] [varchar](40) NULL,                 " +
                                          "    [PRINTSETTING] [varchar](40) NULL,             " +
                                          " CONSTRAINT [PK_SYS_SDQUEUE] PRIMARY KEY CLUSTERED " +
                                          "(                                                  " +
                                          "    [ID] ASC                                       " +
                                          ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                                          ") ON [PRIMARY]";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDQUEUE table.\n\r";
                    }

                    //[SYS_SDQUEUEPAGE]
                    command.CommandText = "CREATE TABLE [SYS_SDQUEUEPAGE](" +
                                          "    [ID] [int] NOT NULL,                                  " +
                                          "    [DOCUMENT] [varbinary](max) NOT NULL,                 " +
                                          "    [PHOTO] [varbinary](max) NULL,                        " +
                                          "    [PAGENAME] [varchar](30) NOT NULL,                    " +
                                          "    CONSTRAINT [PK_SYS_SDQUEUEPAGE] PRIMARY KEY CLUSTERED " +
                                          "(                                                         " +
                                          "    [ID] ASC,                                             " +
                                          "    [PAGENAME] ASC                                        " +
                                          ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                                          ") ON [PRIMARY]";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "ALTER TABLE [SYS_SDQUEUEPAGE]  WITH CHECK ADD  CONSTRAINT [FK_SYS_SDQUEUEPAGE_SYS_SDQUEUE] FOREIGN KEY([ID]) " +
                                              "REFERENCES [SYS_SDQUEUE] ([ID])";
                        command.ExecuteNonQuery();

                        command.CommandText = "ALTER TABLE [SYS_SDQUEUEPAGE] CHECK CONSTRAINT [FK_SYS_SDQUEUEPAGE_SYS_SDQUEUE]";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDQUEUEPAGE table.\n\r";
                    }

                    //[SYS_SDSOLUTIONS]
                    command.CommandText = "CREATE TABLE [SYS_SDSOLUTIONS](" +
                                          "    [USERID] [varchar](20) NOT NULL,                   " +
                                          "    [SOLUTIONID] [varchar](30) NOT NULL,               " +
                                          "    [SOLUTIONNAME] [nvarchar](30) NULL,                " +
                                          "    [MOUDLEXMLTEXT] [nvarchar](max) NOT NULL,          " +
                                          "    [SETTING] [varbinary](max) NULL,                   " +
                                          "    [ALIASOPTIONS] [varchar](250) NULL,                " +
                                          "    [LOGONIMAGE] [varbinary](max) NULL,                " +
                                          "    [BGSTARTCOLOR] [varchar](9) NULL,                  " +
                                          "    [BGENDCOLOR] [varchar](9) NULL,                    " +
                                          "    [THEME] [varchar](20) NULL,                        " +
                                          "    [COMPANY] [nvarchar](30) NULL,                     " +
                                          "    [PAGESAVEOPTION] [int] NULL,                       " +
                                          " CONSTRAINT [PK_SYS_SDSOLUTIONS] PRIMARY KEY CLUSTERED " +
                                          "(                                                      " +
                                          "    [USERID] ASC,                                      " +
                                          "    [SOLUTIONID] ASC                                   " +
                                          ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                                          ") ON [PRIMARY]";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDSOLUTIONS table.\n\r";
                    }

                    //[SYS_SDUSERS]
                    command.CommandText = "CREATE TABLE [SYS_SDUSERS](" +
                                          "    [USERID] [varchar](20) NOT NULL,               " +
                                          "    [USERNAME] [nvarchar](30) NULL,                " +
                                          "    [PASSWORD] [varchar](20) NULL,                 " +
                                          "    [GROUPID] [varchar](20) NULL,                  " +
                                          "    [LASTDATE] [datetime] NULL,                    " +
                                          "    [EMAIL] [nvarchar](50) NULL,                   " +
                                          "    [SYSTYPE] [nvarchar](1) NULL,                  " +
                                          " CONSTRAINT [PK_SYS_SDUSERS] PRIMARY KEY CLUSTERED " +
                                          "(                                                  " +
                                          "    [USERID] ASC                                   " +
                                          ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                                          ") ON [PRIMARY]";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDUSERS table.\n\r";
                    }

                    //[SYS_SDUSERS_LOG]
                    command.CommandText = "CREATE TABLE [SYS_SDUSERS_LOG](" +
                                          "    [ID] [int] IDENTITY(1,1) NOT NULL,                 " +
                                          "    [USERID] [varchar](20) NOT NULL,                   " +
                                          "    [IPADDRESS] [varchar](20) NOT NULL,                " +
                                          "    [LOGINTIME] [datetime] NOT NULL,                   " +
                                          "    [LOGOUTTIME] [datetime] NOT NULL,                  " +
                                          " CONSTRAINT [PK_SYS_SDUSERS_LOG] PRIMARY KEY CLUSTERED " +
                                          "(                                                      " +
                                          "    [ID] ASC                                           " +
                                          ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                                          ") ON [PRIMARY]";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDUSERS_LOG table.\n\r";
                    }

                    //[SYS_WEBPAGES]
                    command.CommandText = "CREATE TABLE [SYS_WEBPAGES](" +
                                          "    [PAGENAME] [varchar](30) NOT NULL,              " +
                                          "    [PAGETYPE] [char](1) NOT NULL,                  " +
                                          "    [DESCRIPTION] [varchar](60) NULL,               " +
                                          "    [CONTENT] [varbinary](max) NULL,                " +
                                          "    [USERID] [varchar](20) NOT NULL,                " +
                                          "    [SOLUTIONID] [varchar](30) NOT NULL,            " +
                                          "    [SERVERDLL] [varbinary](max) NULL,              " +
                                          "    [CHECKOUT] [bit] NULL,                          " +
                                          "    [CHECKOUTDATE] [datetime] NULL,                 " +
                                          "    [CHECKOUTUSER] [varchar](20) NULL,              " +
                                          " CONSTRAINT [PK_SYS_WEBPAGES] PRIMARY KEY CLUSTERED " +
                                          "(                                                   " +
                                          "    [PAGENAME] ASC,                                 " +
                                          "    [PAGETYPE] ASC,                                 " +
                                          "    [USERID] ASC,                                   " +
                                          "    [SOLUTIONID] ASC                                " +
                                          ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                                          ") ON [PRIMARY]";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_WEBPAGES table.\n\r";
                    }

                    //[SYS_WEBPAGES_LOG]
                    command.CommandText = "CREATE TABLE [SYS_WEBPAGES_LOG](" +
                                          "    [PAGENAME] [varchar](30) NOT NULL,                  " +
                                          "    [PAGETYPE] [char](1) NOT NULL,                      " +
                                          "    [DESCRIPTION] [varchar](60) NULL,                   " +
                                          "    [CONTENT] [varbinary](max) NULL,                    " +
                                          "    [USERID] [varchar](20) NOT NULL,                    " +
                                          "    [SOLUTIONID] [varchar](30) NOT NULL,                " +
                                          "    [SERVERDLL] [varbinary](max) NULL,                  " +
                                          "    [CHECKINDATE] [datetime] NOT NULL,                  " +
                                          "    [ID] [int] IDENTITY(1,1) NOT NULL,                  " +
                                          "    [CHECKINUSER] [varchar](20) NULL,                   " +
                                          "    [CHECKINDESCRIPTION] [varchar](255) NULL,           " +
                                          " CONSTRAINT [PK_SYS_WEBPAGES_LOG] PRIMARY KEY CLUSTERED " +
                                          "(                                                       " +
                                          "    [ID] ASC                                            " +
                                          ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                                          ") ON [PRIMARY]";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_WEBPAGES_LOG table.\n\r";
                    }

                    //[SYS_WEBRUNTIME]
                    command.CommandText = "CREATE TABLE [SYS_WEBRUNTIME](" +
                                          "    [PAGENAME] [varchar](30) NOT NULL,                " +
                                          "    [PAGETYPE] [char](1) NOT NULL,                    " +
                                          "    [CONTENT] [varbinary](max) NULL,                  " +
                                          "    [USERID] [varchar](20) NOT NULL,                  " +
                                          "    [SOLUTIONID] [varchar](30) NOT NULL,              " +
                                          " CONSTRAINT [PK_SYS_WEBRUNTIME] PRIMARY KEY CLUSTERED " +
                                          "(                                                     " +
                                          "    [PAGENAME] ASC,                                   " +
                                          "    [PAGETYPE] ASC,                                   " +
                                          "    [USERID] ASC,                                     " +
                                          "    [SOLUTIONID] ASC                                  " +
                                          ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                                          ") ON [PRIMARY]";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_WEBRUNTIME table.\n\r";
                    }
                    #endregion
                }

                if (result != "")
                    MessageBox.Show(result);
                else if (cdb.x == radioType.typical)
                    MessageBox.Show("Create all System Tables successfully.");
                else if (cdb.x == radioType.simplified)
                    MessageBox.Show("Create table COLDEF table and table SYSAUTONUM successfully.");
                else if (cdb.x == radioType.EEP7m)
                    MessageBox.Show("Alter successfully.");
                else if (cdb.x == radioType.EEP2006m)
                    MessageBox.Show("EEP 2006 2.1.0.1(SP2) Migration success");
                else if (cdb.x == radioType.WorkFlow)
                    MessageBox.Show("Create WorkFlow Tables successfully.");
                else if (cdb.x == radioType.EEPCloud)
                    MessageBox.Show("Create EEPCloud Tables successfully.");
            }
        }

        public void CreateMySqlSystemTable(IDbCommand command)
        {
            frmCreateDB cdb = new frmCreateDB();
            cdb.ShowDialog();

            if (cdb.DialogResult == DialogResult.OK)
            {
                string result = "";

                if (cdb.x == radioType.simplified || cdb.x == radioType.typical)
                {
                    #region Simplifield
                    command.CommandText = "CREATE TABLE COLDEF"
                                                     + "("
                                                     + "TABLE_NAME nvarchar(20) NOT NULL, "
                                                     + "FIELD_NAME nvarchar(20) NOT NULL, "
                                                     + "SEQ decimal(12,0), "
                                                     + "FIELD_TYPE nvarchar(20), "
                                                     + "IS_KEY nvarchar(1) NOT NULL, "
                                                     + "FIELD_LENGTH decimal(12,0), "
                                                     + "CAPTION nvarchar(40), "
                                                     + "EDITMASK nvarchar(10), "
                                                     + "NEEDBOX nvarchar(13), "
                                                     + "CANREPORT nvarchar(1), "
                                                     + "EXT_MENUID nvarchar(20), "
                                                     + "FIELD_SCALE decimal(12,0), "
                                                     + "DD_NAME nvarchar(40), "
                                                     + "DEFAULT_VALUE nvarchar(100), "
                                                     + "CHECK_NULL nvarchar(1), "
                                                     + "QUERYMODE nvarchar(20), "
                                                     + "CAPTION1 nvarchar(40), "
                                                     + "CAPTION2 nvarchar(40), "
                                                     + "CAPTION3 nvarchar(40), "
                                                     + "CAPTION4 nvarchar(40), "
                                                     + "CAPTION5 nvarchar(40), "
                                                     + "CAPTION6 nvarchar(40), "
                                                     + "CAPTION7 nvarchar(40), "
                                                     + "CAPTION8 nvarchar(40),"
                                                     + "PRIMARY KEY(TABLE_NAME,FIELD_NAME)"
                                                     + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create COLDEF table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX TABLENAME ON COLDEF (TABLE_NAME,FIELD_NAME)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX TABLENAME on table COLDEF .\n\r";
                    }

                    command.CommandText = "CREATE TABLE SYSAUTONUM "
                                            + "("
                                            + "AUTOID VARCHAR(20) NOT NULL, "
                                            + "FIXED VARCHAR(20) NOT NULL, "
                                            + "CURRNUM decimal(10,0), "
                                            + "DESCRIPTION VARCHAR(50),"
                                            + "PRIMARY KEY (AUTOID, FIXED)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYSAUTONUM table.\n\r";
                    }
                    #endregion

                    #region Typical
                    if (cdb.x == radioType.typical)
                    {
                        //// Create GROUPFORMS
                        //command.CommandText = "CREATE TABLE GROUPFORMS ("
                        //    + "GROUPID nvarchar (20) NOT NULL ,"
                        //    + "PACKAGE_NAME nvarchar (50) NOT NULL ,"
                        //    + "FORM_NAME nvarchar (50) NULL ,"
                        //    + "PARENT_MENU nvarchar (50) NULL "
                        //    + ")";
                        //try
                        //{
                        //    command.ExecuteNonQuery();
                        //}
                        //catch
                        //{
                        //    result += "Can not create GROUPFORMS table.\n\r";
                        //}

                        // Create GROUPMENUS
                        command.CommandText = "CREATE TABLE GROUPMENUS ("
                            + "GROUPID varchar (20) NOT NULL, "
                            + "MENUID nvarchar (30) NOT NULL, "
                            + "PRIMARY KEY(GROUPID,MENUID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUS table.\n\r";
                        }

                        // Create USERMENUS
                        command.CommandText = "CREATE TABLE USERMENUS ("
                            + "USERID varchar (20) NOT NULL, "
                            + "MENUID nvarchar (30) NOT NULL, "
                            + "PRIMARY KEY(USERID,MENUID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUS table.\n\r";
                        }

                        // Create GROUPS
                        command.CommandText = "CREATE TABLE GROUPS ("
                            + "GROUPID varchar (20) NOT NULL ,"
                            + "GROUPNAME nvarchar (50),"
                            + "DESCRIPTION nvarchar (100),"
                            + "MSAD nvarchar (1),"
                            + "ISROLE char(1),"
                            + "PRIMARY KEY(GROUPID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('00', 'EveryOne', 'N')";
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('01', 'DEPARTMENT1', 'N')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPS table.\n\r";
                        }

                        // Create MENUITEMTYPE
                        command.CommandText = "CREATE TABLE MENUITEMTYPE ("
                            + "ITEMTYPE nvarchar (20) NOT NULL ,"
                            + "ITEMNAME nvarchar (20),"
                            + "DBALIAS nvarchar (50),"
                            + "PRIMARY KEY(ITEMTYPE)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUITEMTYPE(ITEMTYPE, ITEMNAME) VALUES('SOLUTION1', 'DEFAULT SOLUTION')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUITEMTYPE table.\n\r";
                        }

                        // Create MENUTABLE
                        command.CommandText = "CREATE TABLE MENUTABLE ("
                            + "MENUID nvarchar (30) NOT NULL ,"
                            + "CAPTION nvarchar (50) NOT NULL ,"
                            + "PARENT nvarchar (20) ,"
                            + "PACKAGE nvarchar (60),"
                            + "MODULETYPE nvarchar (1),"
                            + "ITEMPARAM nvarchar (200),"
                            + "FORM nvarchar (32),"
                            + "ISSHOWMODAL nvarchar (1),"
                            + "ITEMTYPE nvarchar (20),"
                            + "SEQ_NO nvarchar (4),"
                            + "PACKAGEDATE DateTime,"
                            + "IMAGE blob,"
                            + "OWNER nvarchar(20),"
                            + "ISSERVER nvarchar(1),"
                            + "VERSIONNO nvarchar(20),"
                            + "CHECKOUT nvarchar(20),"
                            + "CHECKOUTDATE datetime,"
                            + "CAPTION0 nvarchar(50),"
                            + "CAPTION1 nvarchar(50),"
                            + "CAPTION2 nvarchar(50),"
                            + "CAPTION3 nvarchar(50),"
                            + "CAPTION4 nvarchar(50),"
                            + "CAPTION5 nvarchar(50),"
                            + "CAPTION6 nvarchar(50),"
                            + "CAPTION7 nvarchar(50),"
                            + "IMAGEURL nvarchar(100),"
                            + "PRIMARY KEY(MENUID)"
                            + ") ";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO MENUTABLE(MENUID, CAPTION, ITEMTYPE, MODULETYPE) VALUES('0', 'ROOT', 'SOLUTION1', 'F')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLE table.\n\r";
                        }

                        // Create USERGROUPS
                        command.CommandText = "CREATE TABLE USERGROUPS ("
                            + "USERID varchar (20) NOT NULL, "
                            + "GROUPID varchar (20) NOT NULL, "
                            + "PRIMARY KEY(USERID,GROUPID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERGROUPS(USERID, GROUPID) VALUES('001', '01')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERGROUPS table.\n\r";
                        }

                        // Create USERS
                        command.CommandText = "CREATE TABLE USERS ("
                            + "USERID varchar (20) NOT NULL ,"
                            + "USERNAME nvarchar (30),"
                            + "AGENT nvarchar (20),"
                            + "PWD nvarchar (10),"
                            + "CREATEDATE nvarchar (8),"
                            + "CREATER nvarchar (20),"
                            + "DESCRIPTION nvarchar (100),"
                            + "EMAIL nvarchar (40),"
                            + "LASTTIME nvarchar (8),"
                            + "AUTOLOGIN nvarchar (1),"
                            + "LASTDATE nvarchar (8),"
                            + "SIGNATURE nvarchar (30),"
                            + "MSAD nvarchar (1),"
                            + "PRIMARY KEY(USERID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO USERS(USERID, USERNAME, PWD, MSAD,AUTOLOGIN) VALUES('001', 'TEST', '', 'N','S')";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERS table.\n\r";
                        }

                        // Create MENUTABLELOG
                        command.CommandText = "CREATE TABLE MENUTABLELOG"
                            + "("
                            + "LOGID int not null auto_increment,"
                            + "MENUID nvarchar(30) not null,"
                            + "PACKAGE nvarchar(20) not null,"
                            + "PACKAGEDATE DATETIME,"
                            + "LASTDATE DATETIME,"
                            + "OWNER nvarchar(20),"
                            + "OLDVERSION blob,"
                            + "OLDDATE nvarchar(20),"
                            + "PRIMARY KEY(LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLELOG table.\n\r";
                        }

                        // Create MENUCHECKLOG
                        command.CommandText = "CREATE TABLE MENUCHECKLOG"
                            + "("
                            + "LOGID int not null auto_increment ,"
                            + "ITEMTYPE nvarchar(20) not null,"
                            + "PACKAGE nvarchar(50) not null,"
                            + "PACKAGEDATE DateTime,"
                            + "FILETYPE nvarchar(10),"
                            + "FILENAME nvarchar(60),"
                            + "FILEDATE DateTime,"
                            + "FILECONTENT blob,"
                            + "PRIMARY KEY(LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUCHECKLOG table.\n\r";
                        }

                        // Create SYSEEPLOG
                        command.CommandText = "CREATE TABLE SYSEEPLOG"
                            + "("
                            + "CONNID nvarchar(20) NOT NULL,"
                            + "LOGID int NOT NULL auto_increment,"
                            + "LOGSTYLE nvarchar(1) NOT NULL,"
                            + "LOGDATETIME DATETIME NOT NULL,"
                            + "DOMAINID nvarchar(30),"
                            + "USERID nvarchar(20),"
                            + "LOGTYPE nvarchar(1),"
                            + "TITLE nvarchar(64),"
                            + "DESCRIPTION nvarchar(128),"
                            + "COMPUTERIP nvarchar(16),"
                            + "COMPUTERNAME nvarchar(64),"
                            + "EXECUTIONTIME int,"
                            + "PRIMARY KEY(LOGID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSEEPLOG table.\n\r";
                        }

                        // Create SYSSQLLOG
                        command.CommandText = "CREATE TABLE SYSSQLLOG("
                             + "LOGSTYLE nvarchar(1),"
                             + "LOGDATETIME DateTime,"
                             + "USERID Varchar(20),"
                             + "DEVELOPERID Varchar(20),"
                             + "DESCRIPTION text,"
                             + "SQLSENTENCE nvarchar(max)"
                             + ");";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSSQLLOG table.\n\r";
                        }

                        // Create SYSERRLOG
                        command.CommandText = "CREATE TABLE SYSERRLOG"
                            + "("
                            + "ERRID int not null auto_increment,"
                            + "USERID varchar(20), "
                            + "MODULENAME nvarchar(30),"
                            + "ERRMESSAGE nvarchar(255),"
                            + "ERRSTACK text,"
                            + "ERRDESCRIP nvarchar(255),"
                            + "ERRDATE DateTime,"
                            + "ERRSCREEN blob,"
                            + "OWNER nvarchar(20),"
                            + "PROCESSDATE DateTime,"
                            + "PROCDESCRIP nvarchar(255),"
                            + "STATUS nvarchar(2),"
                            + "PRIMARY KEY(ERRID)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYSERRLOG table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_LANGUAGE"
                            + "("
                            + "ID int NOT NULL auto_increment,"
                            + "IDENTIFICATION nvarchar(80),"
                            + "KEY_S nvarchar(80),"
                            + "EN nvarchar(80),"
                            + "CHT nvarchar(80),"
                            + "CHS nvarchar(80),"
                            + "HK nvarchar(80),"
                            + "JA nvarchar(80),"
                            + "KO nvarchar(80),"
                            + "LAN1 nvarchar(80),"
                            + "LAN2 nvarchar(80),"
                            + "PRIMARY KEY(ID)"
                            + ")";

                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_LANGUAGE table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_MESSENGER"
                            + "("
                            + "USERID varchar(20) NOT NULL,"
                            + "MESSAGE nvarchar(255),"
                            + "PARAS nvarchar(255),"
                            + "SENDTIME nvarchar(14),"
                            + "SENDERID nvarchar(20),"
                            + "RECTIME nvarchar(14),"
                            + "STATUS char(1)"
                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_MESSENGER table.\n\r";
                        }

                        //CreateMenuTableControl
                        command.CommandText = "CREATE TABLE MENUTABLECONTROL"
                                                + "("
                                                + "MENUID varchar (30), "
                                                + "CONTROLNAME Varchar (50), "
                                                + "DESCRIPTION Varchar (50), "
                                                + "TYPE Varchar (20),"
                                                + "PRIMARY KEY(MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create MENUTABLECONTROL table.\n\r";
                        }

                        //CreateGroupMenuControl
                        command.CommandText = "CREATE TABLE GROUPMENUCONTROL"
                                                + "("
                                                + "GROUPID Varchar (20), "
                                                + "MENUID Varchar (30), "
                                                + "CONTROLNAME Varchar (50), "
                                                + "TYPE Varchar (20), "
                                                + "ENABLED CHAR (1), "
                                                + "VISIBLE CHAR (1), "
                                                + "ALLOWADD CHAR (1), "
                                                + "ALLOWUPDATE CHAR (1), "
                                                + "ALLOWDELETE CHAR (1), "
                                                + "ALLOWPRINT CHAR (1),"
                                                + "PRIMARY KEY(GROUPID,MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create GROUPMENUCONTROL table.\n\r";
                        }

                        //CreateUserMenuControl
                        command.CommandText = "CREATE TABLE USERMENUCONTROL"
                                                + "("
                                                + "USERID Varchar (20), "
                                                + "MENUID Varchar (30), "
                                                + "CONTROLNAME Varchar (50), "
                                                + "TYPE Varchar (20), "
                                                + "ENABLED CHAR (1), "
                                                + "VISIBLE CHAR (1), "
                                                + "ALLOWADD CHAR (1), "
                                                + "ALLOWUPDATE CHAR (1), "
                                                + "ALLOWDELETE CHAR (1), "
                                                + "ALLOWPRINT CHAR (1),"
                                                + "PRIMARY KEY(USERID,MENUID,CONTROLNAME)"
                                                + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create USERMENUCONTROL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL"
                                            + "("
                                            + "REFVAL_NO Varchar(100) Not NULL, "
                                            + "DESCRIPTION Varchar(250), "
                                            + "TABLE_NAME Varchar(100), "
                                            + "CAPTION Varchar(100), "
                                            + "DISPLAY_MEMBER Varchar(100), "
                                            + "SELECT_ALIAS Varchar(250), "
                                            + "SELECT_COMMAND Varchar(250), "
                                            + "VALUE_MEMBER Varchar(100),"
                                            + "PRIMARY KEY(REFVAL_NO)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REFVAL_D1"
                                            + "("
                                            + "REFVAL_NO Varchar(30) Not NULL, "
                                            + "FIELD_NAME Varchar(30) Not NULL, "
                                            + "HEADER_TEXT Varchar(20), "
                                            + "WIDTH integer,"
                                            + "PRIMARY KEY(REFVAL_NO,FIELD_NAME)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REFVAL_D1 table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_ANYQUERY"
                                            + "("
                                            + "QUERYID Varchar(20) Not NULL, "
                                            + "USERID Varchar(20) Not NULL, "
                                            + "TEMPLATEID Varchar(20), "
                                            + "TABLENAME Varchar(50), "
                                            + "LASTDATE datetime, "
                                            + "CONTENT text, "
                                            + "PRIMARY Key (QUERYID,USERID,TEMPLATEID)"
                                            + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_ANYQUERY table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_REPORT"
                                           + "("
                                           + "REPORTID nVarchar(50) Not NULL, "
                                           + "FILENAME nVarchar(50) Not NULL, "
                                           + "REPORTNAME nVarchar(255), "
                                           + "DESCRIPTION nVarchar(255), "
                                           + "FILEPATH nVarchar(255), "
                                           + "OUTPUTMODE nVarchar(20), "
                                           + "HEADERREPEAT nVarchar(5), "
                                           + "HEADERFONT blob, "
                                           + "HEADERITEMS blob, "
                                           + "FOOTERFONT blob, "
                                           + "FOOTERITEMS blob, "
                                           + "FIELDFONT blob, "
                                           + "FIELDITEMS blob, "
                                           + "SETTING blob, "
                                           + "FORMAT blob, "
                                           + "PARAMETERS blob, "
                                           + "IMAGES blob, "
                                           + "MAILSETTING blob, "
                                           + "DATASOURCE_PROVIDER nVarchar(50),"
                                           + "DATASOURCES blob,"
                                           + "CLIENT_QUERY blob,"
                                           + "REPORT_TYPE nVarchar(1),"
                                           + "TEMPLATE_DESC nVarchar(50),"
                                           + "PRIMARY Key (REPORTID,FILENAME)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_REPORT table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_PERSONAL"
                                           + "("
                                           + "FORMNAME NVARCHAR(60) NOT NULL,"
                                           + "COMPNAME NVARCHAR(30) NOT NULL,"
                                           + "USERID NVARCHAR(20) NOT NULL,"
                                           + "REMARK NVARCHAR(30),"
                                           + "PROPCONTENT BLOB,"
                                           + "CREATEDATE DATETIME,"
                                           + "PRIMARY KEY (FORMNAME,COMPNAME,USERID)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_PERSONAL table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE SYS_EEP_USERS"
                                           + "("
                                           + "USERID NVARCHAR(50) NOT NULL,"
                                           + "USERNAME NVARCHAR(50) NULL,"
                                           + "COMPUTER NVARCHAR(50) NOT NULL,"
                                           + "LOGINTIME NVARCHAR(50),"
                                           + "LASTACTIVETIME NVARCHAR(50),"
                                           + "LOGINCOUNT integer,"
                                           + "PRIMARY KEY (USERID,COMPUTER)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create SYS_EEP_USERS table.\n\r";
                        }

                        command.CommandText = "CREATE TABLE UserDevices"
                                           + "("
                                           + "UserID varchar(50) NULL,"
                                           + "UUID varchar(50) NULL,"
                                           + "Active varchar(1) NULL,"
                                           + "CreateDate datetime,"
                                           + "LoginDate datetime,"
                                           + "ExpiryDate datetime,"
                                           + "RegID nvarchar(255),"
                                           + "TokenID nvarchar(255)"
                                           + ")";
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result += "Can not create UserDevices table.\n\r";
                        }

                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP7m)
                {
                    #region EEP7m
                    command.CommandText = "Alter Table COLDEF add "
                                                      + "DEFAULT_VALUE NVARCHAR(100), "
                                                      + "CHECK_NULL NVARCHAR(1), "
                                                      + "QUERYMODE NVARCHAR(20), "
                                                      + "CAPTION1 NVARCHAR (40), "
                                                      + "CAPTION2 NVARCHAR (40), "
                                                      + "CAPTION3 NVARCHAR (40), "
                                                      + "CAPTION4 NVARCHAR (40), "
                                                      + "CAPTION5 NVARCHAR (40), "
                                                      + "CAPTION6 NVARCHAR (40), "
                                                      + "CAPTION7 NVARCHAR (40), "
                                                      + "CAPTION8 NVARCHAR (40)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPMENUS alter column MENUID nvarchar (30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table GROUPS add MSAD NVARCHAR (1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS alter column GROUPNAME nvarchar (50)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table GROUPS alter column DESCRIPTION nvarchar (200)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table MENUTABLE add "
                                           + "MODULETYPE NVARCHAR(1), "
                                           + "PACKAGEDATE DATETIME year to FRACTION, "
                                           + "IMAGE byte, "
                                           + "OWNER NVARCHAR(20), "
                                           + "ISSERVER NVARCHAR(1), "
                                           + "VERSIONNO NVARCHAR(20), "
                                           + "CHECKOUT NVARCHAR(20), "
                                           + "CHECKOUTDATE DATETIME year to FRACTION, "
                                           + "CAPTION0 NVARCHAR(50), "
                                           + "CAPTION1 NVARCHAR(50), "
                                           + "CAPTION2 NVARCHAR(50), "
                                           + "CAPTION3 NVARCHAR(50), "
                                           + "CAPTION4 NVARCHAR(50), "
                                           + "CAPTION5 NVARCHAR(50), "
                                           + "CAPTION6 NVARCHAR(50), "
                                           + "CAPTION7 NVARCHAR(50), "
                                           + "IMAGEURL NVARCHAR(100)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE alter column MENUID nvarchar(30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table MENUTABLE alter column ITEMTYPE nvarchar(20)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table USERMENUS alter column MENUID nvarchar(30)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter Table USERS add "
                                           + "EMAIL NVARCHAR(40), "
                                           + "LASTTIME NVARCHAR(8), "
                                           + "AUTOLOGIN NVARCHAR(1), "
                                           + "LASTDATE NVARCHAR(8), "
                                           + "SIGNATURE NVARCHAR(30), "
                                           + "MSAD NVARCHAR(1)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEP2006m)
                {
                    #region EEP2006m
                    command.CommandText = "Alter Table MENUFAVOR ADD GROUPNAME NVARCHAR(20)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }

                    command.CommandText = "Alter table SYS_TODOLIST  ADD  ATTACHMENTS VARCHAR(255)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    command.CommandText = "Alter table SYS_TODOHIS  ADD  ATTACHMENTS VARCHAR(255)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result += e.Message + "\n\r";
                    }
                    #endregion
                }
                //odbc未测试
                else if (cdb.x == radioType.WorkFlow)
                {
                    #region WorkFlow
                    //GROUPS
                    command.CommandText = "ALTER TABLE GROUPS ADD ISROLE CHAR(1) NULL";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //result += "Can not add ISROLE column to GROUPS table.\n\r";
                    }
                    // SYS_ORG
                    command.CommandText = "CREATE TABLE SYS_ORG(" +
                                        "ORG_NO Varchar(8) NOT NULL," +
                                        "ORG_DESC Varchar(40) NOT NULL," +
                                        "ORG_KIND Varchar(4) NOT NULL," +
                                        "UPPER_ORG Varchar(8) NULL," +
                                        "ORG_MAN Varchar(20) NOT NULL," +
                                        "LEVEL_NO Varchar(6) NOT NULL," +
                                        "ORG_TREE Varchar(40) NULL," +
                                        "END_ORG Varchar(4) NULL," +
                                        "ORG_FULLNAME Varchar(254) NULL," +
                                        "PRIMARY Key (ORG_NO)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORG(ORG_NO,ORG_DESC,ORG_KIND,ORG_MAN,LEVEL_NO) Values ('1','總公司','0','001','9')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORG table.\n\r";
                    }

                    // SYS_ORGKIND
                    command.CommandText = "CREATE TABLE SYS_ORGKIND(" +
                                        "ORG_KIND Varchar(4) NOT NULL," +
                                        "KIND_DESC Varchar(40) NOT NULL," +
                                        "PRIMARY Key (ORG_KIND)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO SYS_ORGKIND(ORG_KIND,KIND_DESC) Values ('0','公司組織')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGKIND table.\n\r";
                    }

                    // SYS_ORGLEVEL
                    command.CommandText = "CREATE TABLE SYS_ORGLEVEL(" +
                                        "LEVEL_NO Varchar(6) NOT NULL," +
                                        "LEVEL_DESC Varchar(40) NOT NULL," +
                                        "PRIMARY Key (LEVEL_NO)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('1','主任/課長/副理')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('2','經理')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('3','副總')";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('9','總經理')";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGLEVEL table.\n\r";
                    }

                    // SYS_ORGROLES
                    command.CommandText = "CREATE TABLE SYS_ORGROLES(" +
                                        "ORG_NO Varchar(8) NOT NULL," +
                                        "ROLE_ID Varchar(20) NOT NULL," +
                                        "ORG_KIND Varchar(4) NULL," +
                                        "PRIMARY KEY(ORG_NO,ROLE_ID)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ORGROLES table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ORGNO ON SYS_ORGROLES (ORG_NO, ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX ORGNO on table SYS_ORGROLES .\n\r";
                    }

                    // SYS_ROLES_AGENT
                    command.CommandText = "CREATE TABLE SYS_ROLES_AGENT(" +
                                        "ROLE_ID Varchar(20) NOT NULL," +
                                        "AGENT Varchar(20) NOT NULL," +
                                        "FLOW_DESC Varchar(40) NOT NULL," +
                                        "START_DATE Varchar(8) NOT NULL," +
                                        "START_TIME Varchar(6) NULL," +
                                        "END_DATE Varchar(8) NOT NULL," +
                                        "END_TIME Varchar(6) NULL," +
                                        "PAR_AGENT Varchar(4) NOT NULL," +
                                        "REMARK Varchar(254) NULL," +
                                        "PRIMARY KEY(ROLE_ID,AGENT)" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_ROLES_AGENT table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX ROLEID ON SYS_ROLES_AGENT (ROLE_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX ROLEID on table SYS_ROLES_AGENT .\n\r";
                    }

                    // SYS_TODOHIS
                    command.CommandText = "CREATE TABLE SYS_TODOHIS(" +
                                        "LISTID Varchar(40) NOT NULL," +
                                        "FLOW_ID Varchar(40) NOT NULL," +
                                        "FLOW_DESC Varchar(40) NULL," +
                                        "ROLE_ID Varchar(20) NOT NULL," +
                                        "S_ROLE_ID Varchar(20) NOT NULL," +
                                        "S_STEP_ID Varchar(20) NOT NULL," +
                                        "D_STEP_ID Varchar(20) NOT NULL," +
                                        "S_STEP_DESC Varchar(64) NULL," +
                                        "S_USER_ID Varchar(20) NOT NULL," +
                                        "USER_ID Varchar(20) NOT NULL," +
                                        "USERNAME Varchar(30) NULL," +
                                        "FORM_NAME Varchar(30) NULL," +
                                        "WEBFORM_NAME Varchar(50) NOT NULL," +
                                        "S_USERNAME Varchar(30) NULL," +
                                        "NAVIGATOR_MODE Varchar(2) NOT NULL," +
                                        "FLNAVIGATOR_MODE Varchar(2) NOT NULL," +
                                        "PARAMETERS Varchar(254) NULL," +
                                        "STATUS Varchar(4) NULL," +
                                        "PROC_TIME decimal(8, 2) NOT NULL," +
                                        "EXP_TIME decimal(8, 2) NOT NULL," +
                                        "TIME_UNIT Varchar(4) NOT NULL," +
                                        "FLOWIMPORTANT Varchar(1) NOT NULL," +
                                        "FLOWURGENT Varchar(1) NOT NULL," +
                                        "FORM_TABLE Varchar(30) NULL," +
                                        "FORM_KEYS Varchar(254) NULL," +
                                        "FORM_PRESENTATION Varchar(254) NULL," +
                                        "REMARK Varchar(254) NULL," +
                                        "VERSION Varchar(2) NULL," +
                                        "VDSNAME Varchar(40) NULL," +
                                        "SENDBACKSTEP Varchar(2) NULL," +
                                        "LEVEL_NO Varchar(6) NULL," +
                                        "UPDATE_DATE Varchar(10) NULL," +
                                        "UPDATE_TIME Varchar(8) NULL," +
                                        "FORM_PRESENT_CT Varchar(254) NULL," +
                                        "ATTACHMENTS Varchar(255) NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOHIS table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOHIS (LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX LISTID on table SYS_TODOHIS.\n\r";
                    }

                    command.CommandText = "CREATE INDEX USERID ON SYS_TODOHIS (USER_ID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX USERID on table SYS_TODOHIS.\n\r";
                    }

                    // SYS_TODOLIST
                    command.CommandText = "CREATE TABLE SYS_TODOLIST(" +
                                        "LISTID Varchar(40) NOT NULL," +
                                        "FLOW_ID Varchar(40) NOT NULL," +
                                        "FLOW_DESC Varchar(40) NULL," +
                                        "APPLICANT Varchar(20) NOT NULL," +
                                        "S_USER_ID Varchar(20) NOT NULL," +
                                        "S_STEP_ID Varchar(20) NOT NULL," +
                                        "S_STEP_DESC Varchar(64) NULL," +
                                        "D_STEP_ID Varchar(20) NOT NULL," +
                                        "D_STEP_DESC Varchar(64) NULL," +
                                        "EXP_TIME decimal(8, 2) NOT NULL," +
                                        "URGENT_TIME decimal(8, 2) NOT NULL," +
                                        "TIME_UNIT Varchar(4) NOT NULL," +
                                        "USERNAME Varchar(30) NULL," +
                                        "FORM_NAME Varchar(30) NULL," +
                                        "NAVIGATOR_MODE Varchar(2) NOT NULL," +
                                        "FLNAVIGATOR_MODE Varchar(2) NOT NULL," +
                                        "PARAMETERS Varchar(254) NULL," +
                                        "SENDTO_KIND Varchar(4) NOT NULL," +
                                        "SENDTO_ID Varchar(20) NOT NULL," +
                                        "FLOWIMPORTANT Varchar(1) NOT NULL," +
                                        "FLOWURGENT Varchar(1) NOT NULL," +
                                        "STATUS Varchar(4) NULL," +
                                        "FORM_TABLE Varchar(30) NULL," +
                                        "FORM_KEYS Varchar(254) NULL," +
                                        "FORM_PRESENTATION Varchar(254) NULL," +
                                        "FORM_PRESENT_CT Varchar(254) NOT NULL," +
                                        "REMARK Varchar(254) NULL," +
                                        "PROVIDER_NAME Varchar(254) NULL," +
                                        "VERSION Varchar(2) NULL," +
                                        "EMAIL_ADD Varchar(40) NULL," +
                                        "EMAIL_STATUS Varchar(1) NULL," +
                                        "VDSNAME Varchar(40) NULL," +
                                        "SENDBACKSTEP Varchar(2) NULL," +
                                        "LEVEL_NO Varchar(6) NULL," +
                                        "WEBFORM_NAME Varchar(50) NOT NULL," +
                                        "UPDATE_DATE Varchar(10) NULL," +
                                        "UPDATE_TIME Varchar(8) NULL," +
                                        "FLOWPATH Varchar(100) NOT NULL," +
                                        "PLUSAPPROVE Varchar(1) NOT NULL," +
                                        "PLUSROLES Varchar(254) NOT NULL," +
                                        "MULTISTEPRETURN Varchar(1) NULL," +
                                        "SENDTO_NAME Varchar(30) NULL," +
                                        "ATTACHMENTS Varchar(255) NULL" +
                                        ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_TODOLIST table.\n\r";
                    }

                    command.CommandText = "CREATE INDEX LISTID ON SYS_TODOLIST (LISTID)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX LISTID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX SENDTOID ON SYS_TODOLIST (SENDTO_ID, SENDTO_KIND)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX SENDTOID on table SYS_TODOLIST.\n\r";
                    }

                    command.CommandText = "CREATE INDEX FLOWDESC ON SYS_TODOLIST (FLOW_DESC)";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create INDEX FLOWDESC on table SYS_TODOLIST.\n\r";
                    }

                    // SYS_FLDefinition
                    command.CommandText = "CREATE TABLE SYS_FLDEFINITION"
                                            + "("
                                            + "FLTYPEID nVarchar(50) NOT NULL, "
                                            + "FLTYPENAME nVarchar(200) NOT NULL, "
                                            + "FLDEFINITION BLOB NOT NULL, "
                                            + "VERSION INT NULL, "
                                            + "PRIMARY KEY(FLTYPEID)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLDEFINITION table.\n\r";
                    }

                    // SYS_FLInstanceState
                    command.CommandText = "CREATE TABLE SYS_FLINSTANCESTATE"
                                            + "("
                                            + "FLINSTANCEID Varchar(50) NOT NULL, "
                                            + "STATE BLOB NOT NULL, "
                                            + "STATUS INTEGER NULL, "
                                            + "INFO Varchar(200) NULL,"
                                            + "PRIMARY KEY(FLINSTANCEID)"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_FLINSTANCESTATE table.\n\r";
                    }

                    // Sys_ExtApprove
                    command.CommandText = "CREATE TABLE SYS_EXTAPPROVE"
                                            + "("
                                            + "APPROVEID Varchar(50) NULL, "
                                            + "GROUPID Varchar(50) NULL, "
                                            + "MINIMUM Varchar(50) NULL, "
                                            + "MAXIMUM Varchar(50) NULL,"
                                            + "ROLEID Varchar(50) NULL"
                                            + ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_EXTAPPROVE table.\n\r";
                    }
                    #endregion
                }
                else if (cdb.x == radioType.EEPCloud)
                {
                    #region EEPCloud
                    //[SYS_SDALIAS]
                    command.CommandText = "CREATE TABLE SYS_SDALIAS(" +
                                           "    USERID varchar(20) NOT NULL,    " +
                                           "    ALIASNAME varchar(30) NOT NULL, " +
                                           "    SYSTEMALIAS varchar(30),   " +
                                           "    DBNAME varchar(25) NOT NULL,    " +
                                           "    SPLIT DECIMAL,                 " +
                                           " PRIMARY KEY " +
                                           "(                                                  " +
                                           "    USERID ,                                  " +
                                           "    ALIASNAME ,                               " +
                                           "    DBNAME                                    " +
                                           ")" +
                                           ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDALIAS table.\n\r";
                    }

                    //[SYS_SDGROUPS]
                    command.CommandText = "CREATE TABLE SYS_SDGROUPS(" +
                                          "    GROUPID varchar(20) NOT NULL,               " +
                                          "    GROUPNAME varchar(30),                 " +
                                          " PRIMARY KEY " +
                                          "(                                                   " +
                                          "    GROUPID                                   " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDGROUPS table.\n\r";
                    }

                    //[SYS_SDQUEUE]
                    command.CommandText = "CREATE TABLE SYS_SDQUEUE(" +
                                          "    ID int NOT NULL,                           " +
                                          "    USERID varchar(30) NOT NULL,               " +
                                          "    PAGETYPE char(1) NOT NULL,                 " +
                                          "    CREATETIME date NULL,                  " +
                                          "    FINISHTIME date NULL,                  " +
                                          "    FINISHFLAG DECIMAL NOT NULL,                   " +
                                          "    DOCUMENT blob NULL,              " +
                                          "    FILENAME varchar(40) NULL,                 " +
                                          "    PRINTSETTING varchar(40) NULL,             " +
                                          " PRIMARY KEY " +
                                          "(                                                  " +
                                          "    ID                                       " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDQUEUE table.\n\r";
                    }

                    //[SYS_SDQUEUEPAGE]
                    command.CommandText = "CREATE TABLE SYS_SDQUEUEPAGE(" +
                                          "    ID int NOT NULL,                                  " +
                                          "    DOCUMENT blob NOT NULL,                 " +
                                          "    PHOTO blob,                        " +
                                          "    PAGENAME varchar(30) NOT NULL,                    " +
                                          "    PRIMARY KEY " +
                                          "(                                                         " +
                                          "    ID ,                                             " +
                                          "    PAGENAME                                         " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();

                        command.CommandText = "ALTER TABLE SYS_SDQUEUEPAGE ADD  CONSTRAINT FK_SYS_SDQUEUEPAGE_SYS_SDQUEUE FOREIGN KEY(ID) " +
                                              "REFERENCES SYS_SDQUEUE (ID)";
                        command.ExecuteNonQuery();

                        //command.CommandText = "ALTER TABLE SYS_SDQUEUEPAGE CHECK CONSTRAINT FK_SYS_SDQUEUEPAGE_SYS_SDQUEUE";
                        //command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDQUEUEPAGE table.\n\r";
                    }

                    //[SYS_SDSOLUTIONS]
                    command.CommandText = "CREATE TABLE SYS_SDSOLUTIONS(" +
                                          "    USERID varchar(20) NOT NULL,                   " +
                                          "    SOLUTIONID varchar(30) NOT NULL,               " +
                                          "    SOLUTIONNAME varchar(30),                " +
                                          "    MOUDLEXMLTEXT varchar(1000) NOT NULL,          " +
                                          "    SETTING blob,                   " +
                                          "    ALIASOPTIONS varchar(250),                " +
                                          "    LOGONIMAGE blob,                " +
                                          "    BGSTARTCOLOR varchar(9),                  " +
                                          "    BGENDCOLOR varchar(9),                    " +
                                          "    THEME varchar(20),                        " +
                                          "    COMPANY varchar(30),                     " +
                                          "    PAGESAVEOPTION int,                       " +
                                          "PRIMARY KEY " +
                                          "(                                                      " +
                                          "    USERID ,                                      " +
                                          "    SOLUTIONID                                    " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDSOLUTIONS table.\n\r";
                    }

                    //[SYS_SDUSERS]
                    command.CommandText = "CREATE TABLE SYS_SDUSERS(" +
                                          "    USERID varchar(20) NOT NULL,               " +
                                          "    USERNAME nvarchar(30) ,                " +
                                          "    PASSWORD varchar(20) ,                 " +
                                          "    GROUPID varchar(20) ,                  " +
                                          "    LASTDATE date ,                    " +
                                          "    EMAIL varchar(50) ,                   " +
                                          "    SYSTYPE nvarchar(1) ,                  " +
                                          " PRIMARY KEY " +
                                          "(                                                  " +
                                          "    USERID                                    " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDUSERS table.\n\r";
                    }

                    //[SYS_SDUSERS_LOG] iii
                    command.CommandText = "CREATE TABLE SYS_SDUSERS_LOG(" +
                                          "    ID int NOT NULL,                 " +
                                          "    USERID varchar(20) NOT NULL,                   " +
                                          "    IPADDRESS varchar(20) NOT NULL,                " +
                                          "    LOGINTIME date NOT NULL,                   " +
                                          "    LOGOUTTIME date NOT NULL,                  " +
                                          " PRIMARY KEY " +
                                          "(                                                      " +
                                          "    ID                                            " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_SDUSERS_LOG table.\n\r";
                    }

                    //[SYS_WEBPAGES]
                    command.CommandText = "CREATE TABLE SYS_WEBPAGES(" +
                                          "    PAGENAME varchar(30) NOT NULL,              " +
                                          "    PAGETYPE char(1) NOT NULL,                  " +
                                          "    DESCRIPTION varchar(60) ,               " +
                                          "    CONTENT blob ,                " +
                                          "    USERID varchar(20) NOT NULL,                " +
                                          "    SOLUTIONID varchar(30) NOT NULL,            " +
                                          "    SERVERDLL blob ,              " +
                                          "    CHECKOUT DECIMAL ,                          " +
                                          "    CHECKOUTDATE date ,                 " +
                                          "    CHECKOUTUSER varchar(20) ,              " +
                                          " PRIMARY KEY " +
                                          "(                                                   " +
                                          "    PAGENAME ,                                 " +
                                          "    PAGETYPE ,                                 " +
                                          "    USERID ,                                   " +
                                          "    SOLUTIONID                                 " +
                                          ")" +
                                          ") ";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_WEBPAGES table.\n\r";
                    }

                    //[SYS_WEBPAGES_LOG] iii
                    command.CommandText = "CREATE TABLE SYS_WEBPAGES_LOG(" +
                                          "    PAGENAME varchar(30) NOT NULL,                  " +
                                          "    PAGETYPE char(1) NOT NULL,                      " +
                                          "    DESCRIPTION varchar(60) ,                   " +
                                          "    CONTENT blob ,                    " +
                                          "    USERID varchar(20) NOT NULL,                    " +
                                          "    SOLUTIONID varchar(30) NOT NULL,                " +
                                          "    SERVERDLL blob ,                  " +
                                          "    CHECKINDATE date NOT NULL,                  " +
                                          "    ID int NOT NULL,                  " +
                                          "    CHECKINUSER varchar(20) ,                   " +
                                          "    CHECKINDESCRIPTION varchar(255) ,           " +
                                          " PRIMARY KEY " +
                                          "(                                                       " +
                                          "    ID                                             " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_WEBPAGES_LOG table.\n\r";
                    }

                    //[SYS_WEBRUNTIME]
                    command.CommandText = "CREATE TABLE SYS_WEBRUNTIME(" +
                                          "    PAGENAME varchar(30) NOT NULL,                " +
                                          "    PAGETYPE char(1) NOT NULL,                    " +
                                          "    CONTENT blob ,                  " +
                                          "    USERID varchar(20) NOT NULL,                  " +
                                          "    SOLUTIONID varchar(30) NOT NULL,              " +
                                          " CONSTRAINT PK_SYS_WEBRUNTIME PRIMARY KEY " +
                                          "(                                                     " +
                                          "    PAGENAME ,                                   " +
                                          "    PAGETYPE ,                                   " +
                                          "    USERID ,                                     " +
                                          "    SOLUTIONID                                   " +
                                          ")" +
                                          ")";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result += "Can not create SYS_WEBRUNTIME table.\n\r";
                    }
                    #endregion
                }

                if (result != "")
                    MessageBox.Show(result);
                else if (cdb.x == radioType.typical)
                    MessageBox.Show("Create all System Tables successfully.");
                else if (cdb.x == radioType.simplified)
                    MessageBox.Show("Create table COLDEF table and table SYSAUTONUM successfully.");
                else if (cdb.x == radioType.EEP7m)
                    MessageBox.Show("Alter successfully.");
                else if (cdb.x == radioType.EEP2006m)
                    MessageBox.Show("EEP 2006 2.1.0.1(SP2) Migration success");
                else if (cdb.x == radioType.WorkFlow)
                    MessageBox.Show("Create WorkFlow Tables successfully.");
            }
        }

        public void CreateOleDbSystemTable(IDbCommand command)
        {
            CreateSybaseSystemTable(command);
        }

        private void frmDBMan_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!bHasSaved)
            {
                if (MessageBox.Show("Setting has been changed. Are you sure to save them?", "Confirmation", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    btnSave_Click(null, null);
                }
            }
        }

        private void cbxSysDB_DropDown(object sender, EventArgs e)
        {
            cbxSysDB.Items.Clear();
            foreach (string sx in lbxDB.Items)
            {
                cbxSysDB.Items.Add(sx);
            }
        }

        private void cbxSysDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = SystemFile.DBFile;

            if (File.Exists(s)) File.Delete(s);

            FileStream aFileStream = new FileStream(s, FileMode.Create);
            try
            {
                XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
                w.Formatting = Formatting.Indented;
                w.WriteStartElement("InfolightDB");

                w.WriteStartElement("DataBase");

                for (int i = 0; i < lbxDB.Items.Count; i++)
                {
                    w.WriteStartElement(lbxDB.Items[i].ToString());
                    w.WriteAttributeString("String", null, lbxDBString.Items[i].ToString());

                    // Add By Chenjian
                    w.WriteAttributeString("Type", lbxDbType.Items[i].ToString());
                    w.WriteAttributeString("OdbcType", lbxOdbcType.Items[i].ToString());
                    w.WriteAttributeString("MaxCount", lbxMaxCount.Items[i].ToString());
                    w.WriteAttributeString("TimeOut", lbxTimeOut.Items[i].ToString());
                    w.WriteAttributeString("Master", lbxIsMaster.Items[i].ToString());
                    w.WriteAttributeString("Encrypt", lbxEncrypt.Items[i].ToString());
                    var encoding = lbxEncoding.Items[i].ToString();
                    if(!string.IsNullOrEmpty(encoding))
                    {
                        w.WriteAttributeString("Encoding", encoding);
                    }
                    w.WriteAttributeString("Password", GetPwdString(lbxPwd.Items[i].ToString()));
                    // End

                    w.WriteEndElement();
                }

                w.WriteEndElement();

                w.WriteStartElement("SystemDB");
                w.WriteValue(cbxSysDB.Text.Trim());
                w.WriteEndElement();

                w.WriteEndElement();
                w.Close();
            }
            finally
            {
                aFileStream.Close();
                this.bHasSaved = true;
            }
        }

        private void buttonView_Click(object sender, EventArgs e)
        {
            if (lbxDB.SelectedItem != null)
            {
                DbConnectionSet.DbConnection conn = DbConnectionSet.GetDbConn(lbxDB.SelectedItem.ToString());
                if (conn != null)
                {
                    frmPooling form = new frmPooling(conn);
                    form.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show(this, string.Format("Connection:{0} does not exist, please restart server", lbxDB.SelectedItem.ToString())
                        , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAssociatedContext_Click(object sender, EventArgs e)
        {
#if WCF
            if (this.lbxDB.SelectedIndex == -1)
            {
                this.lbxDB.SelectedIndex = 0;
            }
            if (File.Exists(SystemFile.DBFile))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(SystemFile.DBFile);
                XmlNode node = doc.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", this.lbxDB.SelectedItem));
                if (node != null && node.Attributes["String"] != null)
                {
                    string connString = node.Attributes["String"].Value;
                    if (node.Attributes["Password"] != null && !string.IsNullOrEmpty(node.Attributes["Password"].Value))
                    {
                        connString += string.Format(";Password={0}", GetPwdString(node.Attributes["Password"].Value));
                    }
                    this.SetEntityConnectionConfig(string.Format("{0}\\app.config", EEPRegistry.Server), connString);
                    this.SetEntityConnectionConfig(string.Format("{0}\\EEPNetServer.exe.config", EEPRegistry.Server), connString);
                }
            }
#endif
        }

#if WCF
        private void SetEntityConnectionConfig(string config, string connString)
        {
            if (File.Exists(config))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(config);
                XmlNode node = doc.SelectSingleNode("configuration/connectionStrings");
                foreach (EdmObject edm in this.lstContext.SelectedItems)
                {
                    string ctx = edm.ContextType;
                    if (edm.ContextType.IndexOf('.') != -1)
                    {
                        ctx = edm.ContextType.Split('.')[1];
                    }
                    XmlNode curNode = null;
                    foreach (XmlNode connNode in node.ChildNodes)
                    {
                        if (connNode.Attributes["name"] != null && ctx == connNode.Attributes["name"].Value)
                        {
                            curNode = connNode;
                            break;
                        }
                    }
                    if (curNode != null)
                    {
                        XmlAttribute attConn = curNode.Attributes["connectionString"];
                        if (attConn != null)
                        {
                            attConn.Value = attConn.Value.Substring(0, attConn.Value.IndexOf("provider connection string=")) + string.Format("provider connection string=\"{0}\"", connString);
                            doc.Save(config);
                        }
                    }
                    else
                    {
                        curNode = doc.CreateElement("add");
                        XmlAttribute attName = doc.CreateAttribute("name");
                        attName.Value = ctx;
                        curNode.Attributes.Append(attName);
                        XmlAttribute attConn = doc.CreateAttribute("connectionString");
                        attConn.Value = string.Format("metadata=res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl;provider=System.Data.SqlClient;provider connection string=\"{1}\"", edm.FileName, connString);
                        curNode.Attributes.Append(attConn);
                        XmlAttribute attPovd = doc.CreateAttribute("providerName");
                        attPovd.Value = "System.Data.EntityClient";
                        curNode.Attributes.Append(attPovd);

                        node.AppendChild(curNode);
                        doc.Save(config);
                    }
                }
            }
        }
#endif
    }
}
