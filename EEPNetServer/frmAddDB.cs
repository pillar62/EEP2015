using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;

namespace EEPNetServer
{
	/// <summary>
	/// Summary description for WinForm.
	/// </summary>
	public class frmAddDB : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox edtName;
		private System.Windows.Forms.TextBox edtString;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOK;
        private ComboBox cbxDatabaseType;
		private Label label2;
		private Label label4;
		private Label label3;
		private MaskedTextBox txtTimeOut;
		private MaskedTextBox txtMaxCount;
        private CheckBox cbIsMaster;
        private MaskedTextBox maskedTextBox1;
        private Label label5;
        private ComboBox cbxOdbcType;
        private Label label6;
        private CheckBox checkBoxEncryptString;
        private TextBox txtEncoding;
        private Label label7;
		private System.Windows.Forms.Button btnCancel;
		
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddDB));
            this.lblName = new System.Windows.Forms.Label();
            this.edtName = new System.Windows.Forms.TextBox();
            this.edtString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbxDatabaseType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTimeOut = new System.Windows.Forms.MaskedTextBox();
            this.txtMaxCount = new System.Windows.Forms.MaskedTextBox();
            this.cbIsMaster = new System.Windows.Forms.CheckBox();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxOdbcType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxEncryptString = new System.Windows.Forms.CheckBox();
            this.txtEncoding = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblName.Location = new System.Drawing.Point(12, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(100, 19);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "DataBase Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // edtName
            // 
            this.edtName.Location = new System.Drawing.Point(130, 16);
            this.edtName.Name = "edtName";
            this.edtName.Size = new System.Drawing.Size(322, 21);
            this.edtName.TabIndex = 1;
            // 
            // edtString
            // 
            this.edtString.Location = new System.Drawing.Point(130, 56);
            this.edtString.Name = "edtString";
            this.edtString.Size = new System.Drawing.Size(322, 21);
            this.edtString.TabIndex = 3;
            this.edtString.TextChanged += new System.EventHandler(this.edtString_TextChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(12, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "DataBase String";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(364, 229);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 24);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(364, 266);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbxDatabaseType
            // 
            this.cbxDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.cbxDatabaseType.Location = new System.Drawing.Point(130, 112);
            this.cbxDatabaseType.Name = "cbxDatabaseType";
            this.cbxDatabaseType.Size = new System.Drawing.Size(140, 20);
            this.cbxDatabaseType.TabIndex = 6;
            this.cbxDatabaseType.SelectedIndexChanged += new System.EventHandler(this.cbxDatabaseType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(12, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 19);
            this.label2.TabIndex = 7;
            this.label2.Text = "DataBase Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label4.Location = new System.Drawing.Point(12, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 19);
            this.label4.TabIndex = 10;
            this.label4.Text = "Time Out";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label3.Location = new System.Drawing.Point(12, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "Max Count";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTimeOut
            // 
            this.txtTimeOut.Enabled = false;
            this.txtTimeOut.Location = new System.Drawing.Point(130, 195);
            this.txtTimeOut.Name = "txtTimeOut";
            this.txtTimeOut.Size = new System.Drawing.Size(139, 21);
            this.txtTimeOut.TabIndex = 12;
            // 
            // txtMaxCount
            // 
            this.txtMaxCount.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.txtMaxCount.Location = new System.Drawing.Point(130, 152);
            this.txtMaxCount.Name = "txtMaxCount";
            this.txtMaxCount.Size = new System.Drawing.Size(139, 21);
            this.txtMaxCount.TabIndex = 11;
            // 
            // cbIsMaster
            // 
            this.cbIsMaster.AutoSize = true;
            this.cbIsMaster.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbIsMaster.ForeColor = System.Drawing.Color.MidnightBlue;
            this.cbIsMaster.Location = new System.Drawing.Point(130, 234);
            this.cbIsMaster.Name = "cbIsMaster";
            this.cbIsMaster.Size = new System.Drawing.Size(127, 19);
            this.cbIsMaster.TabIndex = 13;
            this.cbIsMaster.Text = "Split System Table";
            this.cbIsMaster.UseVisualStyleBackColor = true;
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.Location = new System.Drawing.Point(130, 265);
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.PasswordChar = '*';
            this.maskedTextBox1.Size = new System.Drawing.Size(139, 21);
            this.maskedTextBox1.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label5.Location = new System.Drawing.Point(12, 265);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 19);
            this.label5.TabIndex = 14;
            this.label5.Text = "Password";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxOdbcType
            // 
            this.cbxOdbcType.Enabled = false;
            this.cbxOdbcType.FormattingEnabled = true;
            this.cbxOdbcType.Items.AddRange(new object[] {
            "Informix",
            "FoxPro",
            "DB2"});
            this.cbxOdbcType.Location = new System.Drawing.Point(364, 112);
            this.cbxOdbcType.Name = "cbxOdbcType";
            this.cbxOdbcType.Size = new System.Drawing.Size(88, 20);
            this.cbxOdbcType.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label6.Location = new System.Drawing.Point(281, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 19);
            this.label6.TabIndex = 17;
            this.label6.Text = "OdbcType";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxEncryptString
            // 
            this.checkBoxEncryptString.AutoSize = true;
            this.checkBoxEncryptString.Location = new System.Drawing.Point(130, 83);
            this.checkBoxEncryptString.Name = "checkBoxEncryptString";
            this.checkBoxEncryptString.Size = new System.Drawing.Size(132, 16);
            this.checkBoxEncryptString.TabIndex = 18;
            this.checkBoxEncryptString.Text = "Use Encrypt String";
            this.checkBoxEncryptString.UseVisualStyleBackColor = true;
            this.checkBoxEncryptString.CheckedChanged += new System.EventHandler(this.checkBoxEncryptString_CheckedChanged);
            // 
            // txtEncoding
            // 
            this.txtEncoding.Location = new System.Drawing.Point(364, 83);
            this.txtEncoding.Name = "txtEncoding";
            this.txtEncoding.Size = new System.Drawing.Size(88, 21);
            this.txtEncoding.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label7.Location = new System.Drawing.Point(281, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 19);
            this.label7.TabIndex = 20;
            this.label7.Text = "Encoding";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmAddDB
            // 
            this.ClientSize = new System.Drawing.Size(456, 299);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtEncoding);
            this.Controls.Add(this.checkBoxEncryptString);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbxOdbcType);
            this.Controls.Add(this.maskedTextBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbIsMaster);
            this.Controls.Add(this.txtTimeOut);
            this.Controls.Add(this.txtMaxCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxDatabaseType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.edtString);
            this.Controls.Add(this.edtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAddDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataBase Setting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAddDB_FormClosing);
            this.Load += new System.EventHandler(this.frmAddDB_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		public string DBName {
			get { return edtName.Text;}
			set { edtName.Text = value;}
		}

		public string DBString {
			get { return edtString.Text;}
			set { edtString.Text = value;}
		}

        // Add By Chenjian
        public string DBType
        {
            get
            {
                return cbxDatabaseType.SelectedIndex.ToString();
            }
            set
            {
                if (int.Parse(value) < 0 || int.Parse(value) >= cbxDatabaseType.Items.Count)
                {
                    return;
                }
                cbxDatabaseType.SelectedIndex = int.Parse(value);
            }
        }

        //Add By Rei
        public string OdbcType
        {
            get
            {
                return cbxOdbcType.SelectedIndex.ToString();
            }
            set
            {
                if (int.Parse(value) < 0 || int.Parse(value) >= cbxOdbcType.Items.Count)
                {
                    return;
                }
                cbxOdbcType.SelectedIndex = int.Parse(value);
            }
        }

		public string MaxCount
		{
			get
			{
				return txtMaxCount.Text;
			}
			set
			{
				txtMaxCount.Text = value;
			}
		}

		public string TimeOut
		{
			get
			{
				return txtTimeOut.Text;
			}
			set
			{
				//txtTimeOut.Text = value;
			}
		}

        public string Pwd
        {
            get
            {
                return maskedTextBox1.Text;
            }
            set
            {
                maskedTextBox1.Text = value;
            }
        }


        public bool IsMaster
        {
            get
            {
                return cbIsMaster.Checked;
            }
            set
            {
                cbIsMaster.Checked = value;
            }
        }

        public bool IsEncrypt
        {
            get { return checkBoxEncryptString.Checked; }
            set { checkBoxEncryptString.Checked = value; }
        }

        public string Encoding {

            get { return txtEncoding.Text; }
            set { txtEncoding.Text = value; }
        }

        private bool OkClicked = false;

        // End

		public frmAddDB()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public frmAddDB(string sDB, string sString, string dbType, bool bMod)
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose (bool disposing)
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

		private void frmAddDB_Load(object sender, EventArgs e)
		{

		}

        private void btnOK_Click(object sender, EventArgs e)
        {
            OkClicked = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void frmAddDB_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Add By Chenjian, Data Validation

            if ( ! OkClicked)
            {
                return;
            }
            // DBName
            frmAddDB aForm = this;

            string errString = "";
            if (aForm.DBName == "")
            {
                errString += "Database Name can not be empty.\n\r";
            }
            else
            {
                for (int i = 0; i < aForm.DBName.Length; ++i)
                {
                    if (i == 0 && !char.IsLetter(aForm.DBName[i]))
                    {
                        errString += "Database Name must start with letter.\n\r";
                    }
                    else if (!char.IsLetterOrDigit(aForm.DBName[i]) && aForm.DBName[i] != '_')
                    {
                        errString += "Database Name must not include character which is not digit, letter, or underscore('_').\n\r";
                        break;
                    }
                }
            }

            // MaxCount
            try
            {
                int num = int.Parse(aForm.MaxCount);
                if (num < 0)
                {
                    errString += "MaxCount must be a positive integer.\n\r";
                }
            }
            catch
            {
                errString += "MaxCount must be a positive integer.\n\r";
            }

            // TimeOut
            try
            {
                int num = int.Parse(aForm.TimeOut);
                if (num < 0)
                {
                    errString += "TimeOut must be a positive integer.\n\r";
                }
            }
            catch
            {
                errString += "TimeOut must be a positive integer.\n\r";
            }

            // Validation ended
            if (errString != "")
            {
                MessageBox.Show(errString);
                e.Cancel = true;
                OkClicked = false;
            }

            // End Add
        }

        private void btnConnectionString_Click(object sender, EventArgs e)
        {
            ClientType ct = (ClientType)this.cbxDatabaseType.SelectedIndex;
            if (ct == ClientType.ctMsSql)
            {
                
            }
            else if (ct == ClientType.ctODBC)
            {

            }
            else if (ct == ClientType.ctOleDB)
            {
                //MSDASC.DataLinks links = new MSDASC.DataLinksClass();
                //ADODB.Connection connection = null;
                //if (this.DBString.Trim() == "")
                //{
                //    object connectionObject = links.PromptNew();
                //    connection = connectionObject as ADODB.Connection;
                //    if (connection != null && connection.ConnectionString != "")
                //    {
                //        this.DBString = connection.ConnectionString;
                //    }
                //}
                //else
                //{
                //    connection = new ADODB.ConnectionClass();
                //    connection.ConnectionString = this.DBString.Trim();

                //    object connectionObject = connection;
                //    bool success = true;
                    
                //    try
                //    {
                //        success = links.PromptEdit(ref connectionObject);
                //    }
                //    catch
                //    {
                //        connectionObject = links.PromptNew();
                //    }

                //    if (success)
                //    {
                //        connection = connectionObject as ADODB.Connection;
                //        if (connection != null && connection.ConnectionString != "")
                //        {
                //            this.DBString = connection.ConnectionString;
                //        }
                //    }
                //}
            }
            else if (ct == ClientType.ctOracle)
            {
            }
        }

        private void cbxDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == 4)
                this.cbxOdbcType.Enabled = true;
            else
                this.cbxOdbcType.Enabled = false;
        }

        private void edtString_TextChanged(object sender, EventArgs e)
        {
            Match match = Regex.Match(edtString.Text, @"connect(ion)? timeout\s*=\s*\d+", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                txtTimeOut.Text = match.Value.Split('=')[1].Trim();
            }
            else
            {
                txtTimeOut.Text = "5";
            }
        }

        private void checkBoxEncryptString_CheckedChanged(object sender, EventArgs e)
        {
            edtString.Enabled = !checkBoxEncryptString.Checked;
            if (checkBoxEncryptString.Checked)
            {
                txtTimeOut.Text = "0";
            }
        }

	}
}
