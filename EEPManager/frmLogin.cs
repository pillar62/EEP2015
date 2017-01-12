using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Xml;
using Srvtools;
using System.Text.RegularExpressions;
using ProxySetting = EEPManager.Properties.Settings;

namespace EEPManager
{
	/// <summary>
	/// Summary description for WinForm1.
	/// </summary>
	public class frmLogin : System.Windows.Forms.Form
    {
        private PictureBox pictureBox1;
        private Panel panel1;
        private ComboBox cbxUserId;
        private ComboBox cbxDataBase;
        private Label lblDB;
        private TextBox edtPwd;
        private Button button2;
        private Button button1;
        private Label label2;
        private Label label1;
        private ComboBox cbxServer;
        private Label label3;
        private Button btnRefresh;
        private ToolTip toolTip1;
        private LinkLabel linkLabelProxy;
        private IContainer components;

		public string GetUserId()
		{
			return cbxUserId.Text;
		}
		public string GetPwd()
		{
			return edtPwd.Text;
		}
		public string GetDB()
		{
            if(DBSplit.Contains(cbxDataBase.Text))
            {
                CliUtils.fLoginDBSplit = (DBSplit[cbxDataBase.Text].ToString() == "1");
            }
			return cbxDataBase.Text;
		}

        public frmLogin()
        {
            InitializeComponent();
        }

        //private bool register = false;

		public frmLogin(bool reg)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            //register = reg;
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabelProxy = new System.Windows.Forms.LinkLabel();
            this.cbxServer = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxUserId = new System.Windows.Forms.ComboBox();
            this.cbxDataBase = new System.Windows.Forms.ComboBox();
            this.lblDB = new System.Windows.Forms.Label();
            this.edtPwd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(328, 93);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(38, 21);
            this.btnRefresh.TabIndex = 18;
            this.toolTip1.SetToolTip(this.btnRefresh, "Refresh");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(110, 125);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 28);
            this.button1.TabIndex = 14;
            this.button1.Text = "OK";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(232, 125);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 28);
            this.button2.TabIndex = 15;
            this.button2.Text = "Cancel";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.linkLabelProxy);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.cbxServer);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbxUserId);
            this.panel1.Controls.Add(this.cbxDataBase);
            this.panel1.Controls.Add(this.lblDB);
            this.panel1.Controls.Add(this.edtPwd);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 88);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(437, 170);
            this.panel1.TabIndex = 10;
            // 
            // linkLabelProxy
            // 
            this.linkLabelProxy.AutoSize = true;
            this.linkLabelProxy.BackColor = System.Drawing.Color.Transparent;
            this.linkLabelProxy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.linkLabelProxy.Location = new System.Drawing.Point(348, 149);
            this.linkLabelProxy.Name = "linkLabelProxy";
            this.linkLabelProxy.Size = new System.Drawing.Size(66, 13);
            this.linkLabelProxy.TabIndex = 19;
            this.linkLabelProxy.TabStop = true;
            this.linkLabelProxy.Text = "Config Proxy";
            this.linkLabelProxy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelProxy_LinkClicked);
            // 
            // cbxServer
            // 
            this.cbxServer.FormattingEnabled = true;
            this.cbxServer.Location = new System.Drawing.Point(142, 69);
            this.cbxServer.Name = "cbxServer";
            this.cbxServer.Size = new System.Drawing.Size(224, 21);
            this.cbxServer.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label3.Location = new System.Drawing.Point(23, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Server";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxUserId
            // 
            this.cbxUserId.FormattingEnabled = true;
            this.cbxUserId.Location = new System.Drawing.Point(142, 18);
            this.cbxUserId.Name = "cbxUserId";
            this.cbxUserId.Size = new System.Drawing.Size(224, 21);
            this.cbxUserId.TabIndex = 9;
            // 
            // cbxDataBase
            // 
            this.cbxDataBase.FormattingEnabled = true;
            this.cbxDataBase.Location = new System.Drawing.Point(142, 94);
            this.cbxDataBase.Name = "cbxDataBase";
            this.cbxDataBase.Size = new System.Drawing.Size(185, 21);
            this.cbxDataBase.TabIndex = 12;
            // 
            // lblDB
            // 
            this.lblDB.BackColor = System.Drawing.Color.Transparent;
            this.lblDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDB.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblDB.Location = new System.Drawing.Point(23, 94);
            this.lblDB.Name = "lblDB";
            this.lblDB.Size = new System.Drawing.Size(101, 20);
            this.lblDB.TabIndex = 13;
            this.lblDB.Text = "DataBase";
            this.lblDB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // edtPwd
            // 
            this.edtPwd.Location = new System.Drawing.Point(142, 43);
            this.edtPwd.Name = "edtPwd";
            this.edtPwd.PasswordChar = '*';
            this.edtPwd.Size = new System.Drawing.Size(224, 20);
            this.edtPwd.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(24, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 22);
            this.label2.TabIndex = 10;
            this.label2.Text = "Password";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(21, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "User";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(437, 88);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // frmLogin
            // 
            this.AcceptButton = this.button1;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(437, 258);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.WinForm1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
		
		private void WinForm1_Load(object sender, System.EventArgs e)
		{
            String s = Application.StartupPath + "\\EEPManager.xml";
            string sUser = "";
            string sDB = "";
            string sServer = string.Empty;
            if (File.Exists(s))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(s);
                XmlNode el = xml.DocumentElement;
                foreach (XmlNode xNode in el.ChildNodes)
                {
                    if (string.Compare(xNode.Name, "USER", true) == 0)//IgnoreCase
                    {
                        sUser = xNode.InnerText.Trim();
                    }
                    else if (string.Compare(xNode.Name, "DATABASE", true) == 0)//IgnoreCase
                    {
                        sDB = xNode.InnerText.Trim();
                    }
                    else if (string.Compare(xNode.Name, "SERVER", true) == 0)//IgnoreCase
                    {
                        sServer = xNode.InnerText.Trim();
                    }
                }
            }

            if (cbxUserId.Items.Count == 0)
            {
                if (sUser != "")
                {
                    string[] sUsers = sUser.Split(new char[] { ',' });
                    cbxUserId.Items.AddRange(sUsers);
                    cbxUserId.SelectedIndex = 0;
                }
            }
            if (cbxServer.Items.Count == 0)
            {
                if (sServer.Length > 0)
                {
                    string[] sServers = sServer.Split(',');
                    cbxServer.Items.AddRange(sServers);
                    cbxServer.SelectedIndex = 0;
                }
                else
                { 
                    cbxServer.Items.Add(string.Format("{0}:{1}",CliUtils.fRemoteIP, CliUtils.fRemotePort));
                }
                cbxServer.SelectedIndex = 0;
            }
            cbxDataBase.Text = sDB.Split(',')[0];
		}

        private bool SetIPandPort()
        {
            string url = cbxServer.Text;
            string[] urls = url.Split(':');
            string ip = urls[0].Trim();
            string sport = urls.Length > 1 ? urls[1].Trim() : "8989";
            int port = 0;

            string message = string.Empty;
            try
            {
                port = Convert.ToInt32(sport);
            }
            catch
            {
                message = "port is invalid";
            }
            if (Regex.IsMatch(ip, @"^([1-9]|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])(\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])){3}$"))
            {
                if (port > 0 && port < 65536)
                {
                    CliUtils.fRemoteIP = ip;
                    CliUtils.fRemotePort = port;
                    return true;
                }
                else
                {
                    message = "port is invalid";
                }
            }
            else
            {
                message = "IPAddress is invalid\n" + message; 
            }
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;

            if (cbxDataBase.Text.Trim().Length == 0)
            {
                MessageBox.Show("Select the database first");
            }
            else
            {
                if (SetIPandPort())
                {
                    frmProgress pro = new frmProgress(CliUtils.fRemoteIP, CliUtils.fRemotePort, "Connect to server");
                    if (pro.ShowDialog() == DialogResult.OK)
                    {
                        if (pro.Module != null)
                        {
                            object[] myRet = pro.Module.CallMethod(new object[] { CliUtils.GetBaseClientInfo() } , "GLModule", "GetDB", null);
                            if (myRet[1] != null && myRet[1] is ArrayList && myRet[2] != null && myRet[2] is ArrayList)
                            {
                                ArrayList dbList = (ArrayList)myRet[1];
                                for (int i = 0; i < dbList.Count; i++)
                                {
                                    string db = dbList[i].ToString();
                                    if (DBSplit.Contains(db))
                                    {
                                        DBSplit[db] = ((ArrayList)myRet[2])[i].ToString();
                                    }
                                    else
                                    {
                                        DBSplit.Add(db, ((ArrayList)myRet[2])[i].ToString());
                                    }
                                }
                            }
                            this.DialogResult = DialogResult.OK;
                        }
                    }

                }
            }
        }

        private Hashtable DBSplit = new Hashtable();

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cbxDataBase.Text = string.Empty;
            cbxDataBase.Items.Clear();
            if (SetIPandPort())
            {
                frmProgress pro = new frmProgress(CliUtils.fRemoteIP, CliUtils.fRemotePort, "Refresh database list");
                if (pro.ShowDialog() == DialogResult.OK)
                {
                    EEPRemoteModule module = pro.Module;
                    if (module != null)
                    {
                        try
                        {
                            object[] myRet = module.CallMethod(new object[]{ CliUtils.GetBaseClientInfo() } , "GLModule", "GetDB", null);
                            if (myRet[1] != null && myRet[1] is ArrayList && myRet[2] != null && myRet[2] is ArrayList)
                            {
                                ArrayList dbList = (ArrayList)myRet[1];
                                for (int i = 0; i < dbList.Count; i++)
                                {
                                    string db = dbList[i].ToString();
                                    this.cbxDataBase.Items.Add(db);
                                    if (DBSplit.Contains(db))
                                    {
                                        DBSplit[db] = ((ArrayList)myRet[2])[i].ToString();
                                    }
                                    else
                                    {
                                        DBSplit.Add(db, ((ArrayList)myRet[2])[i].ToString());
                                    }
                                }
                            }
                        }
                        catch (Exception e1)
                        {
                            MessageBox.Show(e1.Message);
                        }
                    }
                }
            }
        }

        private void linkLabelProxy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //A6-Sensitive Data Exposure
            //Srvtools.frmProxy form = new frmProxy(ProxySetting.Default.ProxyEnable, ProxySetting.Default.ProxyAddress
            //   , ProxySetting.Default.ProxyPort, ProxySetting.Default.ProxyUser
            //    , ProxySetting.Default.ProxyPassword);
            Srvtools.frmProxy form = new frmProxy(false, string.Empty, 0, string.Empty, string.Empty);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                //A6-Sensitive Data Exposure
                //ProxySetting.Default.ProxyEnable = form.ProxyEnable;
                //ProxySetting.Default.ProxyAddress = form.ProxyAddress;
                //ProxySetting.Default.ProxyPort = form.ProxyPort;
                //ProxySetting.Default.ProxyUser = form.ProxyUser;
                //ProxySetting.Default.ProxyPassword = form.ProxyPassword;
                //ProxySetting.Default.Save();
                //Srvtools.CliUtils.RegisterProxy(ProxySetting.Default.ProxyEnable, string.Format("{0}:{1}", ProxySetting.Default.ProxyAddress
                //    , ProxySetting.Default.ProxyPort), ProxySetting.Default.ProxyUser, ProxySetting.Default.ProxyPassword);
                Srvtools.CliUtils.RegisterProxy(form.ProxyEnable, string.Format("{0}:{1}", form.ProxyAddress
                    , form.ProxyPort), form.ProxyUser, form.textBoxPassword.Text);
            }
        }
	}
}
