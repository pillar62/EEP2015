namespace EEPNetServer
{
    partial class frmLoginMan
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
            this.chkAllowPerLogin = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridViewAD = new System.Windows.Forms.DataGridView();
            this.ColumnPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPassword = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxUserTable = new System.Windows.Forms.GroupBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.textBoxUserID = new System.Windows.Forms.TextBox();
            this.textBoxTable = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxUserTable = new System.Windows.Forms.CheckBox();
            this.checkBoxLoginInSameIP = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxPassWrodExpiry = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBoxPasswordCharNum = new System.Windows.Forms.CheckBox();
            this.textBoxPasswordMaxSize = new System.Windows.Forms.TextBox();
            this.textBoxPasswordMinSize = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxEnableDatabase = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonTest = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAD)).BeginInit();
            this.groupBoxUserTable.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkAllowPerLogin
            // 
            this.chkAllowPerLogin.AutoSize = true;
            this.chkAllowPerLogin.Checked = true;
            this.chkAllowPerLogin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowPerLogin.Location = new System.Drawing.Point(6, 6);
            this.chkAllowPerLogin.Name = "chkAllowPerLogin";
            this.chkAllowPerLogin.Size = new System.Drawing.Size(204, 16);
            this.chkAllowPerLogin.TabIndex = 1;
            this.chkAllowPerLogin.Text = "Allow user to multiple  login.";
            this.chkAllowPerLogin.UseVisualStyleBackColor = true;
            this.chkAllowPerLogin.CheckedChanged += new System.EventHandler(this.chkAllowPerLogin_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(219, 288);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(300, 288);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridViewAD);
            this.groupBox1.Location = new System.Drawing.Point(3, 78);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 142);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Active Directory";
            // 
            // dataGridViewAD
            // 
            this.dataGridViewAD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAD.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnPath,
            this.ColumnUser,
            this.ColumnPassword});
            this.dataGridViewAD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAD.Location = new System.Drawing.Point(3, 17);
            this.dataGridViewAD.MultiSelect = false;
            this.dataGridViewAD.Name = "dataGridViewAD";
            this.dataGridViewAD.RowTemplate.Height = 23;
            this.dataGridViewAD.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAD.Size = new System.Drawing.Size(370, 122);
            this.dataGridViewAD.TabIndex = 0;
            this.dataGridViewAD.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewAD_CellFormatting);
            this.dataGridViewAD.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewAD_EditingControlShowing);
            // 
            // ColumnPath
            // 
            this.ColumnPath.HeaderText = "Path";
            this.ColumnPath.Name = "ColumnPath";
            this.ColumnPath.Width = 115;
            // 
            // ColumnUser
            // 
            this.ColumnUser.HeaderText = "User";
            this.ColumnUser.Name = "ColumnUser";
            // 
            // ColumnPassword
            // 
            this.ColumnPassword.HeaderText = "Password";
            this.ColumnPassword.Name = "ColumnPassword";
            // 
            // groupBoxUserTable
            // 
            this.groupBoxUserTable.Controls.Add(this.textBoxPassword);
            this.groupBoxUserTable.Controls.Add(this.textBoxUserName);
            this.groupBoxUserTable.Controls.Add(this.textBoxUserID);
            this.groupBoxUserTable.Controls.Add(this.textBoxTable);
            this.groupBoxUserTable.Controls.Add(this.label7);
            this.groupBoxUserTable.Controls.Add(this.label6);
            this.groupBoxUserTable.Controls.Add(this.label5);
            this.groupBoxUserTable.Controls.Add(this.label4);
            this.groupBoxUserTable.Location = new System.Drawing.Point(14, 25);
            this.groupBoxUserTable.Name = "groupBoxUserTable";
            this.groupBoxUserTable.Size = new System.Drawing.Size(277, 120);
            this.groupBoxUserTable.TabIndex = 6;
            this.groupBoxUserTable.TabStop = false;
            this.groupBoxUserTable.Text = "User Table Defination";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(109, 92);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(156, 21);
            this.textBoxPassword.TabIndex = 9;
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(109, 67);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(156, 21);
            this.textBoxUserName.TabIndex = 8;
            // 
            // textBoxUserID
            // 
            this.textBoxUserID.Location = new System.Drawing.Point(109, 42);
            this.textBoxUserID.Name = "textBoxUserID";
            this.textBoxUserID.Size = new System.Drawing.Size(156, 21);
            this.textBoxUserID.TabIndex = 7;
            // 
            // textBoxTable
            // 
            this.textBoxTable.Location = new System.Drawing.Point(109, 17);
            this.textBoxTable.Name = "textBoxTable";
            this.textBoxTable.Size = new System.Drawing.Size(156, 21);
            this.textBoxTable.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(41, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "Password:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "UserName:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "UserID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "Table:";
            // 
            // checkBoxUserTable
            // 
            this.checkBoxUserTable.AutoSize = true;
            this.checkBoxUserTable.Location = new System.Drawing.Point(17, 6);
            this.checkBoxUserTable.Name = "checkBoxUserTable";
            this.checkBoxUserTable.Size = new System.Drawing.Size(162, 16);
            this.checkBoxUserTable.TabIndex = 10;
            this.checkBoxUserTable.Text = "User Table ReDefination";
            this.checkBoxUserTable.UseVisualStyleBackColor = true;
            this.checkBoxUserTable.CheckedChanged += new System.EventHandler(this.checkBoxUserTable_CheckedChanged);
            // 
            // checkBoxLoginInSameIP
            // 
            this.checkBoxLoginInSameIP.AutoSize = true;
            this.checkBoxLoginInSameIP.Checked = true;
            this.checkBoxLoginInSameIP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLoginInSameIP.Location = new System.Drawing.Point(6, 27);
            this.checkBoxLoginInSameIP.Name = "checkBoxLoginInSameIP";
            this.checkBoxLoginInSameIP.Size = new System.Drawing.Size(204, 16);
            this.checkBoxLoginInSameIP.TabIndex = 11;
            this.checkBoxLoginInSameIP.Text = "Allow user to login in same IP";
            this.checkBoxLoginInSameIP.UseVisualStyleBackColor = true;
            this.checkBoxLoginInSameIP.CheckedChanged += new System.EventHandler(this.chkAllowPerLogin_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxPassWrodExpiry);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.checkBoxPasswordCharNum);
            this.groupBox2.Controls.Add(this.textBoxPasswordMaxSize);
            this.groupBox2.Controls.Add(this.textBoxPasswordMinSize);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(8, 14);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 130);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Password Policy";
            // 
            // textBoxPassWrodExpiry
            // 
            this.textBoxPassWrodExpiry.Location = new System.Drawing.Point(109, 68);
            this.textBoxPassWrodExpiry.Name = "textBoxPassWrodExpiry";
            this.textBoxPassWrodExpiry.Size = new System.Drawing.Size(152, 21);
            this.textBoxPassWrodExpiry.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 12);
            this.label10.TabIndex = 6;
            this.label10.Text = "PassWrod Expiry:";
            // 
            // checkBoxPasswordCharNum
            // 
            this.checkBoxPasswordCharNum.AutoSize = true;
            this.checkBoxPasswordCharNum.Location = new System.Drawing.Point(109, 106);
            this.checkBoxPasswordCharNum.Name = "checkBoxPasswordCharNum";
            this.checkBoxPasswordCharNum.Size = new System.Drawing.Size(126, 16);
            this.checkBoxPasswordCharNum.TabIndex = 5;
            this.checkBoxPasswordCharNum.Text = "Password Char&&Num";
            this.checkBoxPasswordCharNum.UseVisualStyleBackColor = true;
            // 
            // textBoxPasswordMaxSize
            // 
            this.textBoxPasswordMaxSize.Location = new System.Drawing.Point(109, 41);
            this.textBoxPasswordMaxSize.Name = "textBoxPasswordMaxSize";
            this.textBoxPasswordMaxSize.Size = new System.Drawing.Size(152, 21);
            this.textBoxPasswordMaxSize.TabIndex = 4;
            // 
            // textBoxPasswordMinSize
            // 
            this.textBoxPasswordMinSize.Location = new System.Drawing.Point(109, 15);
            this.textBoxPasswordMinSize.Name = "textBoxPasswordMinSize";
            this.textBoxPasswordMinSize.Size = new System.Drawing.Size(152, 21);
            this.textBoxPasswordMinSize.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 12);
            this.label9.TabIndex = 1;
            this.label9.Text = "PassWord MaxSize:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "PassWord MinSize:";
            // 
            // checkBoxEnableDatabase
            // 
            this.checkBoxEnableDatabase.AutoSize = true;
            this.checkBoxEnableDatabase.Checked = true;
            this.checkBoxEnableDatabase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableDatabase.Location = new System.Drawing.Point(7, 49);
            this.checkBoxEnableDatabase.Name = "checkBoxEnableDatabase";
            this.checkBoxEnableDatabase.Size = new System.Drawing.Size(156, 16);
            this.checkBoxEnableDatabase.TabIndex = 13;
            this.checkBoxEnableDatabase.Text = "Save users in database";
            this.checkBoxEnableDatabase.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(387, 282);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonTest);
            this.tabPage1.Controls.Add(this.chkAllowPerLogin);
            this.tabPage1.Controls.Add(this.checkBoxEnableDatabase);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.checkBoxLoginInSameIP);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(379, 256);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Domain";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBoxUserTable);
            this.tabPage2.Controls.Add(this.groupBoxUserTable);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(379, 226);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Table Defination";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(379, 226);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Password Policy";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(275, 85);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 8;
            this.label11.Text = "(Date)";
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(298, 223);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 15;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // frmLoginMan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 323);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmLoginMan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLoginMan_FormClosing);
            this.Load += new System.EventHandler(this.frmLoginMan_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAD)).EndInit();
            this.groupBoxUserTable.ResumeLayout(false);
            this.groupBoxUserTable.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAllowPerLogin;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxUserTable;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.TextBox textBoxUserID;
        private System.Windows.Forms.TextBox textBoxTable;
        private System.Windows.Forms.CheckBox checkBoxUserTable;
        private System.Windows.Forms.CheckBox checkBoxLoginInSameIP;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxPasswordMinSize;
        private System.Windows.Forms.TextBox textBoxPasswordMaxSize;
        private System.Windows.Forms.CheckBox checkBoxPasswordCharNum;
        private System.Windows.Forms.CheckBox checkBoxEnableDatabase;
        private System.Windows.Forms.TextBox textBoxPassWrodExpiry;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridView dataGridViewAD;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPassword;
        private System.Windows.Forms.Button buttonTest;

    }
}