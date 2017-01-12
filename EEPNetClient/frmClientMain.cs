using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.Remoting;
using System.Reflection;
using System.Data.SqlClient;
using Srvtools;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace EEPNetClient
{
    /// <summary>
    /// Summary description for WinForm.
    /// </summary>
    public partial class frmClientMain : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// 
        private System.ComponentModel.IContainer components;

        private frmLogin fFrmLogin = null;
        public DataSet CurDataSet = null;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem testToolStripMenuItem;
        private Panel panel1;
        private ComboBox infoCmbSolution;
        private System.Windows.Forms.Timer tmMessage;
        private Splitter splitter1;
        private ImageList imglst;
        private Panel panel2;
        private PictureBox pictureBox1;
        private Panel panel3;
        private Panel panel4;
        private PictureBox pbMyFavor;
        private DataSet menuDataSet;
        private TreeView tView;
        private PictureBox pbGo;
        private TextBox tbGO;
        private DataSet menuFavorDataSet;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem systemToolStripMenuItem;
        private ToolStripMenuItem dataBaseToolStripMenuItem;
        private ToolStripMenuItem changePasswordToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem windowsToolStripMenuItem;
        private ToolStripMenuItem tileHorizontalToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem tileVerticalToolStripMenuItem;
        private ToolStripMenuItem treeViewToolStripMenuItem;
        private ToolStripMenuItem aboutEEPNetClientToolStripMenuItem;
        private ToolStripMenuItem solutionToolStripMenuItem;
        private DataSet groupFavorDataSet;

        //frmMessage frm = null;
        //private SqlConnection nwindConn;
        [DllImport("KERNEL32.DLL", EntryPoint = "GetThreadLocale", SetLastError = true,
        CharSet = CharSet.Unicode, ExactSpelling = true,
        CallingConvention = CallingConvention.StdCall)]
        public static extern uint GetThreadLocale();

        private FormCollection fFormCollection;

        public frmClientMain()
        {
            //
            // Required for Windows Form Designer support
            //
            Application.EnableVisualStyles();
            InitializeComponent();
            fFormCollection = new FormCollection(this, typeof(FormItem));
            CliUtils.fCliMainHandle = this.Handle;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            if (ClientLoaderSolution != null)
            {
                infoCmbSolution.Visible = false;
            }
            if (File.Exists(Application.StartupPath + "\\EEPNetClientMain.jpg"))
            {
                this.BackgroundImage = Image.FromFile(Application.StartupPath + "\\EEPNetClientMain.jpg");
            }
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClientMain));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tView = new System.Windows.Forms.TreeView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pbGo = new System.Windows.Forms.PictureBox();
            this.tbGO = new System.Windows.Forms.TextBox();
            this.pbMyFavor = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.infoCmbSolution = new System.Windows.Forms.ComboBox();
            this.tmMessage = new System.Windows.Forms.Timer(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.imglst = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataBaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutEEPNetClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMyFavor)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "EEP.Net Client";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 26);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.testToolStripMenuItem.Text = "Fetch Message...";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click_1);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.infoCmbSolution);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 431);
            this.panel1.TabIndex = 11;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.tView);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 20);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(176, 411);
            this.panel3.TabIndex = 17;
            // 
            // tView
            // 
            this.tView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tView.Location = new System.Drawing.Point(0, 27);
            this.tView.Name = "tView";
            this.tView.Size = new System.Drawing.Size(176, 384);
            this.tView.TabIndex = 10;
            this.tView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tView_BeforeSelect);
            this.tView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tView_NodeMouseDoubleClick);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pbGo);
            this.panel4.Controls.Add(this.tbGO);
            this.panel4.Controls.Add(this.pbMyFavor);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(176, 27);
            this.panel4.TabIndex = 9;
            // 
            // pbGo
            // 
            this.pbGo.Image = global::EEPNetClient.Properties.Resources.MenuGo;
            this.pbGo.Location = new System.Drawing.Point(120, 4);
            this.pbGo.Name = "pbGo";
            this.pbGo.Size = new System.Drawing.Size(24, 17);
            this.pbGo.TabIndex = 12;
            this.pbGo.TabStop = false;
            this.pbGo.Click += new System.EventHandler(this.pbGo_Click);
            this.pbGo.MouseLeave += new System.EventHandler(this.pbGo_MouseLeave);
            this.pbGo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbGo_MouseMove);
            // 
            // tbGO
            // 
            this.tbGO.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbGO.Location = new System.Drawing.Point(6, 3);
            this.tbGO.Name = "tbGO";
            this.tbGO.Size = new System.Drawing.Size(111, 22);
            this.tbGO.TabIndex = 11;
            this.tbGO.TextChanged += new System.EventHandler(this.tbGO_TextChanged);
            // 
            // pbMyFavor
            // 
            this.pbMyFavor.Image = global::EEPNetClient.Properties.Resources.AddFavor;
            this.pbMyFavor.Location = new System.Drawing.Point(147, 4);
            this.pbMyFavor.Name = "pbMyFavor";
            this.pbMyFavor.Size = new System.Drawing.Size(23, 17);
            this.pbMyFavor.TabIndex = 0;
            this.pbMyFavor.TabStop = false;
            this.pbMyFavor.Click += new System.EventHandler(this.pbMyFavor_Click);
            this.pbMyFavor.MouseLeave += new System.EventHandler(this.pbMyFavor_MouseLeave);
            this.pbMyFavor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbMyFavor_MouseMove);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(176, 20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(18, 411);
            this.panel2.TabIndex = 16;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::EEPNetClient.Properties.Resources.d2;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(18, 21);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // infoCmbSolution
            // 
            this.infoCmbSolution.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoCmbSolution.FormattingEnabled = true;
            this.infoCmbSolution.Location = new System.Drawing.Point(0, 0);
            this.infoCmbSolution.Name = "infoCmbSolution";
            this.infoCmbSolution.Size = new System.Drawing.Size(194, 20);
            this.infoCmbSolution.TabIndex = 5;
            this.infoCmbSolution.SelectedIndexChanged += new System.EventHandler(this.infoCmbSolution_SelectedIndexChanged);
            // 
            // tmMessage
            // 
            this.tmMessage.Interval = 30000;
            this.tmMessage.Tick += new System.EventHandler(this.tmMessage_Tick);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(194, 25);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 431);
            this.splitter1.TabIndex = 13;
            this.splitter1.TabStop = false;
            // 
            // imglst
            // 
            this.imglst.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglst.ImageStream")));
            this.imglst.TransparentColor = System.Drawing.Color.Transparent;
            this.imglst.Images.SetKeyName(0, "default.ico");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemToolStripMenuItem,
            this.windowsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(874, 25);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solutionToolStripMenuItem,
            this.dataBaseToolStripMenuItem,
            this.changePasswordToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.systemToolStripMenuItem.Text = "System";
            // 
            // solutionToolStripMenuItem
            // 
            this.solutionToolStripMenuItem.Name = "solutionToolStripMenuItem";
            this.solutionToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.solutionToolStripMenuItem.Text = "Solution";
            this.solutionToolStripMenuItem.Click += new System.EventHandler(this.menuItemSolution_Click);
            // 
            // dataBaseToolStripMenuItem
            // 
            this.dataBaseToolStripMenuItem.Name = "dataBaseToolStripMenuItem";
            this.dataBaseToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.dataBaseToolStripMenuItem.Text = "DataBase";
            this.dataBaseToolStripMenuItem.Click += new System.EventHandler(this.menuItemDataBase_Click);
            // 
            // changePasswordToolStripMenuItem
            // 
            this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            this.changePasswordToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.changePasswordToolStripMenuItem.Text = "ChangePassword";
            this.changePasswordToolStripMenuItem.Click += new System.EventHandler(this.menuItemCP_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tileHorizontalToolStripMenuItem,
            this.tileVerticalToolStripMenuItem,
            this.treeViewToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(73, 21);
            this.windowsToolStripMenuItem.Text = "Windows";
            // 
            // tileHorizontalToolStripMenuItem
            // 
            this.tileHorizontalToolStripMenuItem.Name = "tileHorizontalToolStripMenuItem";
            this.tileHorizontalToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.tileHorizontalToolStripMenuItem.Text = "TileHorizontal";
            // 
            // tileVerticalToolStripMenuItem
            // 
            this.tileVerticalToolStripMenuItem.Name = "tileVerticalToolStripMenuItem";
            this.tileVerticalToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.tileVerticalToolStripMenuItem.Text = "TileVertical";
            this.tileVerticalToolStripMenuItem.Click += new System.EventHandler(this.menuItemTileV_Click);
            // 
            // treeViewToolStripMenuItem
            // 
            this.treeViewToolStripMenuItem.Checked = true;
            this.treeViewToolStripMenuItem.CheckOnClick = true;
            this.treeViewToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.treeViewToolStripMenuItem.Name = "treeViewToolStripMenuItem";
            this.treeViewToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.treeViewToolStripMenuItem.Text = "TreeView";
            this.treeViewToolStripMenuItem.CheckedChanged += new System.EventHandler(this.treeViewToolStripMenuItem_CheckedChanged);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutEEPNetClientToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutEEPNetClientToolStripMenuItem
            // 
            this.aboutEEPNetClientToolStripMenuItem.Name = "aboutEEPNetClientToolStripMenuItem";
            this.aboutEEPNetClientToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.aboutEEPNetClientToolStripMenuItem.Text = "About EEP.Net Client";
            this.aboutEEPNetClientToolStripMenuItem.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // frmClientMain
            // 
            this.BackgroundImage = global::EEPNetClient.Properties.Resources.BG;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(874, 456);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmClientMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EEP.NET Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmClientMain_FormClosed);
            this.Load += new System.EventHandler(this.WinForm_Load);
            this.SizeChanged += new System.EventHandler(this.frmClientMain_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMyFavor)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

     
        #endregion

        public static string ClientLoaderSolution = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Modify By Chenjian
            String s;
            s = Application.StartupPath + "\\";
            //#if winxp
            //            //RemotingConfiguration.Configure(s + "EEPNetClient.exe.config", true);
            //#endif
            CliUtils.LoadLoginServiceConfig(s + "EEPNetClient.exe.config");
          
            CliUtils.fClientLang = GetClientLanguage();
            CliUtils.fClientSystem = "Win";

            string solution = string.Empty;
            //args: language, ip, port, solution, proxyenable, proxyaddress, proxyuser, proxypassword
            if (args.Length >= 1)
            {
                try
                {
                    CliUtils.fClientLang = (SYS_LANGUAGE)Enum.Parse(typeof(SYS_LANGUAGE), args[0]);
                }
                catch (ArgumentException) { }
                if (args.Length >= 3)
                {
                    CliUtils.fRemoteIP = args[1];
                    try
                    {
                        CliUtils.fRemotePort = Convert.ToInt32(args[2]);
                    }
                    catch { }
                    if (args.Length >= 4)
                    {
                        ClientLoaderSolution = args[3];
                        if (args.Length >= 8)
                        {
                            try
                            {
                                CliUtils.RegisterProxy(Convert.ToBoolean(args[4]), args[5], args[6], args[7]);
                            }
                            catch { }
                        }
                    }
                }
            }
         
            try
            {
                Application.Run(new frmClientMain());
            }
            catch
            {
                Application.Exit();
            }
        }

        static private SYS_LANGUAGE GetClientLanguage()
        {
            //uint dwlang = GetThreadLocale();
            uint dwlang = (uint)System.Globalization.CultureInfo.CurrentCulture.LCID;
            ushort wlang = (ushort)dwlang;
            ushort wprilangid = (ushort)(wlang & 0x3FF);
            ushort wsublangid = (ushort)(wlang >> 10);

            if (0x09 == wprilangid)
                return SYS_LANGUAGE.ENG;
            else if (0x04 == wprilangid)
            {
                if (0x01 == wsublangid)
                    return SYS_LANGUAGE.TRA;
                else if (0x02 == wsublangid)
                    return SYS_LANGUAGE.SIM;
                else if (0x03 == wsublangid)
                    return SYS_LANGUAGE.HKG;
                else
                    return SYS_LANGUAGE.TRA;
            }
            else if (0x11 == wprilangid)
                return SYS_LANGUAGE.JPN;
            else
                return SYS_LANGUAGE.ENG;
        }

        private void SaveToClientXML(string sLoginUser, string sLoginDB, string sCurrentProject)
        {
            String sfile = Application.StartupPath + "\\EEPNetClient.xml";
            string sUser = sLoginUser;
            string sDB = sLoginDB;
            string sSol = sCurrentProject;
            string stemp = "";
            XmlDocument xml = new XmlDocument();
            if (File.Exists(sfile))
            {
                xml.Load(sfile);
                XmlNode el = xml.DocumentElement;
                foreach (XmlNode xNode in el.ChildNodes)
                {
                    if (string.Compare(xNode.Name, "USER", true) == 0)//IgnoreCase
                    {
                        stemp = xNode.InnerText.Trim();
                        string[] ss = stemp.Split(new char[] { ',' });
                        foreach (string s in ss)
                        {
                            if (!s.Equals(sLoginUser))
                                sUser = sUser + "," + s;
                        }
                    }
                    else if (string.Compare(xNode.Name, "DATABASE", true) == 0)//IgnoreCase
                    {
                        stemp = xNode.InnerText.Trim();
                        string[] ss = stemp.Split(new char[] { ',' });
                        foreach (string s in ss)
                        {
                            if (!s.Equals(sLoginDB))
                                sDB = sDB + "," + s;
                        }
                    }
                    else if (string.Compare(xNode.Name, "SOLUTION", true) == 0)//IgnoreCase
                    {
                        stemp = xNode.InnerText.Trim();
                        string[] ss = stemp.Split(new char[] { ',' });
                        foreach (string s in ss)
                        {
                            if (!s.Equals(sCurrentProject))
                                sSol = sSol + "," + s;
                        }
                    }
                }

                File.Delete(sfile);
            }
            else
            {
                sUser = sLoginUser; sDB = sLoginDB; sSol = sCurrentProject;
            }

            FileStream aFileStream = new FileStream(sfile, FileMode.Create);
            try
            {
                XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
                w.Formatting = Formatting.Indented;
                w.WriteStartElement("LoginInfo");

                w.WriteStartElement("User");
                w.WriteValue(sUser);
                w.WriteEndElement();

                w.WriteStartElement("DataBase");
                w.WriteValue(sDB);
                w.WriteEndElement();

                w.WriteStartElement("Solution");
                w.WriteValue(sSol);
                w.WriteEndElement();

                w.WriteEndElement();
                w.Close();
            }
            finally
            {
                aFileStream.Close();
            }
        }

        private bool Register(bool isShowMessage)
        {
            string message = "";
            bool rtn = CliUtils.Register(ref message);
            if (rtn)
            {
                CliUtils.GetSysXml(Application.StartupPath + @"\sysmsg.xml");
            }
            else
            {
                if (isShowMessage)
                {
                    string showmessage = string.Empty;
                    try
                    {
                        showmessage = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "ExceedsMax");
                    }
                    catch
                    {
                        showmessage = "The current number of users exceeds the max number set by the AP server!try again?";
                    }
                    if (message == showmessage)
                    {
                        if (MessageBox.Show(this, showmessage, "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                            == DialogResult.Yes)
                        {
                            this.Close();
                            Application.Restart();
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return rtn;
        }
        //private static ArrayList ReMenu = new ArrayList();
        private void WinForm_Load(object sender, System.EventArgs e)
        {
            bool freg = Register(true);
            fFrmLogin = new frmLogin(freg);

            if (fFrmLogin.ShowDialog(this) == DialogResult.OK)
            {
                if (freg == false)
                {
                    if (!Register(true))
                    {
                        this.Close();
                        return;
                    }
                }
                CheckUser();
                //TrayIcon support...
                //tmMessage.Enabled = true;
                this.WindowState = FormWindowState.Maximized;
                SetMenuText();
            }
            else
            {
                CliUtils.fLoginUser = string.Empty;
                Close();
            }
        }

        private void CheckUser()
        {
            CheckUser(false);
        }

        private void CheckUser(bool relogin)
        {
            CliUtils.fLoginUser = fFrmLogin.GetUserId();
            CliUtils.fLoginPassword = fFrmLogin.GetPwd();
            CliUtils.fLoginDB = fFrmLogin.GetDB();
            CliUtils.fCurrentProject = fFrmLogin.GetCurProject();
            LoginResult result = LoginResult.Success;
            if (CliUtils.fLoginUser.Contains("'"))
            {
                result = LoginResult.UserNotFound;
            }
            else
            {
                string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB;
                if (relogin)
                {
                    sParam += ":1";
                }
                else
                {
                    sParam += ":0";
                }
                if (fFrmLogin.DomainLogin)
                {
                    sParam += ":" + Environment.UserDomainName + ":" + CliUtils.DomainCheckSum(Environment.UserDomainName);
                }
                object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
                result = (LoginResult)myRet[1];
                switch (result)
                {
                    case LoginResult.UserNotFound:
                        {
                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserNotFound");
                            MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    case LoginResult.PasswordError:
                        {
                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserOrPasswordError");
                            MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    case LoginResult.Disabled:
                        {
                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserDisabled");
                            MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    case LoginResult.UserLogined:
                        {
                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserIsLogined");
                            MessageBox.Show(this, string.Format(message, CliUtils.fLoginUser), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    case LoginResult.RequestReLogin:
                        {
                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserReLogined");
                            if (MessageBox.Show(string.Format(message, CliUtils.fLoginUser)
                                , "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                CheckUser(true);
                            }
                            else
                            {
                                CliUtils.fLoginUser = string.Empty;
                                this.Close();
                            }
                            return;
                        }
                    default:
                        {
                            CliUtils.GetPasswordPolicy();
                            if (CliUtils.fPassWordNotify != 0)
                                CheckPassword(CliUtils.fLoginUser);

                            object[] isTableExist = CliUtils.CallMethod("GLModule", "isTableExist", new object[] { "MENUFAVOR" });
                            if (isTableExist != null && Convert.ToInt16(isTableExist[0]) == 0 && Convert.ToInt16(isTableExist[1]) == 1)
                                panel4.Visible = false;

                            CliUtils.fUserName = myRet[2].ToString();
                            CliUtils.fLoginUser = myRet[3].ToString();
                           
                            myRet = CliUtils.CallMethod("GLModule", "GetUserGroup", new object[] { CliUtils.fLoginUser });
                            if (myRet != null && (int)myRet[0] == 0)
                            {
                                CliUtils.fGroupID = myRet[1].ToString();
                            }

                            DataSet dsSolution = new DataSet();
                            if (CliUtils.fSolutionSecurity)
                            {
                                object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolutionSecurity", new object[] { CliUtils.fLoginUser, CliUtils.fGroupID });
                                if ((null != myRet1) && (0 == (int)myRet1[0]))
                                    dsSolution = ((DataSet)myRet1[1]);
                                bool flag = false;
                                for (int i = 0; i < dsSolution.Tables[0].Rows.Count; i++)
                                {
                                    if (dsSolution.Tables[0].Rows[i]["ITEMTYPE"].ToString() == CliUtils.fCurrentProject)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (!flag)
                                {
                                    SYS_LANGUAGE language = CliUtils.fClientLang;
                                    string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_SolutionSecurity");
                                    MessageBox.Show(String.Format(message, CliUtils.fCurrentProject));
                                }
                            }
                            else
                            {
                                object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolution", null);
                                if ((null != myRet1) && (0 == (int)myRet1[0]))
                                    dsSolution = ((DataSet)myRet1[1]);
                            }
                            DoLoad();
                            SaveToClientXML(CliUtils.fLoginUser, CliUtils.fLoginDB, CliUtils.fCurrentProject);
                            break;
                        }
                }
            }
            if (result != LoginResult.Success)
            {
                if (fFrmLogin.ShowDialog(this) == DialogResult.OK)
                {
                    CheckUser();
                }
                else
                {
                    CliUtils.fLoginUser = string.Empty;
                    this.Close();
                }
            }
        }

        private void SetMenuText()
        {
            string menutext = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "Menu");
            if (menutext.Length > 0)
            {
                string[] list = menutext.Split(';');
                systemToolStripMenuItem.Text = list[0];
                solutionToolStripMenuItem.Text = list[1];
                dataBaseToolStripMenuItem.Text = list[2];
                changePasswordToolStripMenuItem.Text = list[3];
                exitToolStripMenuItem.Text = list[4];
                windowsToolStripMenuItem.Text = list[5];
                tileHorizontalToolStripMenuItem.Text = list[6];
                tileVerticalToolStripMenuItem.Text = list[7];
                treeViewToolStripMenuItem.Text = list[8];
                helpToolStripMenuItem.Text = list[9];
                aboutEEPNetClientToolStripMenuItem.Text = list[10];
            }
        }

        bool solutionLoad = false;
        public void DoLoad()
        {
            DataSet dsSolution = new DataSet();
            if (CliUtils.fSolutionSecurity)
            {
                object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolutionSecurity", new object[] { CliUtils.fLoginUser, CliUtils.fGroupID });
                if ((null != myRet1) && (0 == (int)myRet1[0]))
                    dsSolution = ((DataSet)myRet1[1]);
            }
            else
            {
                object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolution", null);
                if ((null != myRet1) && (0 == (int)myRet1[0]))
                    dsSolution = ((DataSet)myRet1[1]);
            }
            this.infoCmbSolution.DataSource = dsSolution;
            string strTableName = dsSolution.Tables[0].TableName;
            this.infoCmbSolution.DisplayMember = strTableName + ".itemname";
            this.infoCmbSolution.ValueMember = strTableName + ".itemtype";
            int i = dsSolution.Tables[0].Rows.Count;
            for (int j = 0; j < i; j++)
            {
                if (string.Compare(dsSolution.Tables[0].Rows[j]["itemtype"].ToString(), CliUtils.fCurrentProject, true) == 0)//IgnoreCase
                {
                    this.infoCmbSolution.SelectedValue = dsSolution.Tables[0].Rows[j]["itemtype"].ToString();
                }
            }
            solutionLoad = true;
            bool flag = false;
            for (int x = 0; x < dsSolution.Tables[0].Rows.Count; x++)
            {
                if (CliUtils.fCurrentProject == dsSolution.Tables[0].Rows[x]["itemtype"].ToString())
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                ItemToGet();
            }
        }

     

        private ArrayList MenuIDList = new ArrayList();
        private ArrayList CaptionList = new ArrayList();
        private ArrayList ParentList = new ArrayList();
        ArrayList ListMainID = new ArrayList();
        ArrayList ListMainCaption = new ArrayList();
        ArrayList ListChildrenID = new ArrayList();
        ArrayList ListOwnerParentID = new ArrayList();
        ArrayList ListChildrenCaption = new ArrayList();
        private ImageList IconList = new ImageList();

        private void ClearItems()
        {
            MenuIDList.Clear();
            CaptionList.Clear();
            ParentList.Clear();
            IconList.Images.Clear();
            ListMainID.Clear();
            ListMainCaption.Clear();
            ListChildrenID.Clear();
            ListOwnerParentID.Clear();
            ListChildrenCaption.Clear();
            tView.Nodes.Clear();
            List<ToolStripItem> items = new List<ToolStripItem>();
            for (int i = 0; i < menuStrip1.Items.Count; i++)
            {
                if (menuStrip1.Items[i].Tag != null)
                {
                    items.Add(menuStrip1.Items[i]);
                }
            }
            for (int i = 0; i < items.Count; i++)
            {
                menuStrip1.Items.Remove(items[i]);
            }
        }

        private void ItemToGet()
        {
            ClearItems();
            if (this.infoCmbSolution.SelectedValue != null)
            {             
                object[] LoginUser = new object[1];
                LoginUser[0] = CliUtils.fLoginUser;
                object[] strParam = new object[2];
                strParam[0] = this.infoCmbSolution.SelectedValue.ToString();
                strParam[1] = "F";

                string strCaption = SetMenuLanguage(); 
                
                object[] isTableExist = CliUtils.CallMethod("GLModule", "isTableExist", new object[] { "MENUFAVOR" });
                if (isTableExist != null && Convert.ToInt16(isTableExist[0]) == 0 && Convert.ToInt16(isTableExist[1]) == 0)
                {
                    object[] myRet1 = CliUtils.CallMethod("GLModule", "FetchFavorMenus", strParam);
                    if ((null != myRet1) && (0 == (int)myRet1[0]))
                    {
                        menuFavorDataSet = (DataSet)(myRet1[1]);
                        groupFavorDataSet = (DataSet)(myRet1[2]);
                    }
                    int menuFavorCount = menuFavorDataSet.Tables[0].Rows.Count;
                    if (menuFavorCount > 0)
                    {
                        SYS_LANGUAGE language = CliUtils.fClientLang;
                        String favor = SysMsg.GetSystemMessage(language, "EEPNetClient", "FavorMenu", "Favor");

                        MenuIDList.Add("MyFavor");
                        CaptionList.Add(favor);
                        ParentList.Add("");
                        IconList.Images.Add("MyFavor", imglst.Images[0]);

                        for (int i = 0; i < groupFavorDataSet.Tables[0].Rows.Count; i++)
                        {
                            if (groupFavorDataSet.Tables[0].Rows[i]["GROUPNAME"] != null && groupFavorDataSet.Tables[0].Rows[i]["GROUPNAME"].ToString() != "")
                            {
                                MenuIDList.Add(groupFavorDataSet.Tables[0].Rows[i]["GROUPNAME"].ToString());
                                CaptionList.Add(groupFavorDataSet.Tables[0].Rows[i]["GROUPNAME"].ToString());
                                ParentList.Add("MyFavor");
                            }
                        }

                        for (int i = 0; i < menuFavorCount; i++)
                        {
                            if (!MenuIDList.Contains(menuFavorDataSet.Tables[0].Rows[i]["MENUID"].ToString()))
                            {
                                MenuIDList.Add(menuFavorDataSet.Tables[0].Rows[i]["MENUID"].ToString());
                                if (strCaption != "")
                                {
                                    if (menuFavorDataSet.Tables[0].Rows[i][strCaption].ToString() != "")
                                        CaptionList.Add(menuFavorDataSet.Tables[0].Rows[i][strCaption].ToString());
                                    else
                                        CaptionList.Add(menuFavorDataSet.Tables[0].Rows[i]["CAPTION"].ToString());
                                }
                                else
                                {
                                    CaptionList.Add(menuFavorDataSet.Tables[0].Rows[i]["CAPTION"].ToString());
                                }

                                if (menuFavorDataSet.Tables[0].Rows[i]["GROUPNAME"] == null || menuFavorDataSet.Tables[0].Rows[i]["GROUPNAME"].ToString() == "")
                                    ParentList.Add("MyFavor");
                                else
                                    ParentList.Add(menuFavorDataSet.Tables[0].Rows[i]["GROUPNAME"].ToString());

                                //new add by ccm
                                try
                                {
                                    byte[] blob = (byte[])menuFavorDataSet.Tables[0].Rows[i]["IMAGE"];

                                    MemoryStream stmblob = new MemoryStream(blob);

                                    try
                                    {
                                        IconList.Images.Add(menuFavorDataSet.Tables[0].Rows[i]["MENUID"].ToString(), Image.FromStream(stmblob));
                                    }
                                    catch
                                    {
                                        IconList.Images.Add(menuFavorDataSet.Tables[0].Rows[i]["MENUID"].ToString(), imglst.Images[0]);
                                    }
                                }
                                catch
                                {
                                    IconList.Images.Add(menuFavorDataSet.Tables[0].Rows[i]["MENUID"].ToString(), imglst.Images[0]);
                                }
                            }
                        }
                    }
                }

                object[] myRet = CliUtils.CallMethod("GLModule", "FetchMenus", strParam);
                if ((null != myRet) && (0 == (int)myRet[0]))
                {
                    menuDataSet = (DataSet)(myRet[1]);
                }
                int menuCount = menuDataSet.Tables[0].Rows.Count;
                for (int i = 0; i < menuCount; i++)
                {
                    MenuIDList.Add(menuDataSet.Tables[0].Rows[i]["MENUID"].ToString());
                    if (strCaption != "")
                    {
                        if (menuDataSet.Tables[0].Rows[i][strCaption].ToString() != "")
                            CaptionList.Add(menuDataSet.Tables[0].Rows[i][strCaption].ToString());
                        else
                            CaptionList.Add(menuDataSet.Tables[0].Rows[i]["CAPTION"].ToString());
                    }
                    else
                    {
                        CaptionList.Add(menuDataSet.Tables[0].Rows[i]["CAPTION"].ToString());
                    }
                    ParentList.Add(menuDataSet.Tables[0].Rows[i]["PARENT"].ToString());
                    //new add by ccm
                    try
                    {
                        byte[] blob = (byte[])menuDataSet.Tables[0].Rows[i]["IMAGE"];

                        MemoryStream stmblob = new MemoryStream(blob);

                        try
                        {
                            IconList.Images.Add(menuDataSet.Tables[0].Rows[i]["MENUID"].ToString(), Image.FromStream(stmblob));
                        }
                        catch
                        {
                            IconList.Images.Add(menuDataSet.Tables[0].Rows[i]["MENUID"].ToString(), imglst.Images[0]);
                        }
                    }
                    catch
                    {
                        IconList.Images.Add(menuDataSet.Tables[0].Rows[i]["MENUID"].ToString(), imglst.Images[0]);
                    }
                }

                
                for (int i = 0; i < MenuIDList.Count; i++)
                {
                    if (ParentList[i].ToString().Length == 0)
                    {
                        ListMainID.Add(MenuIDList[i].ToString());
                        ListMainCaption.Add(CaptionList[i].ToString());
                    }
                    else
                    {
                        ListChildrenID.Add(MenuIDList[i].ToString());
                        ListOwnerParentID.Add(ParentList[i].ToString());
                        ListChildrenCaption.Add(CaptionList[i].ToString());
                    }
                }
                tView.ImageList = IconList;
                initializeMenu();
                initializeTreeView();
            }
        }

        private void CheckPassword(String userid)
        {
            DateTime date = new DateTime();
            DateTime today = DateTime.Today;
            String value = "";
            object[] param = new object[3];
            param[0] = userid;

            object[] myRet = CliUtils.CallMethod("GLModule", "GetPasswordLastDate", param);
            if (myRet != null && myRet[0].ToString() == "0")
            {
                if (myRet[1] == DBNull.Value || myRet[1].ToString() == "")
                    value = "new";
                else
                {
                    date = DateTime.ParseExact(myRet[1].ToString(), "yyyyMMdd", null);
                    TimeSpan ts = today - date;
                    value = ts.TotalDays.ToString();
                }
            }

            if (value == "new")
            {
                SYS_LANGUAGE language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "NewPassword");
                MessageBox.Show(message);

                CliUtils.CallMethod("GLModule", "LogOut", new object[] { (object)(CliUtils.fLoginUser) });
                frmUserPWD fupwd = new frmUserPWD();
                fupwd.ShowDialog();
                if (!fupwd.upwdControl1.isOK)
                    Environment.Exit(0);
                else
                {
                    string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ':' + "0";
                    CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
                }
            }
            else
            {
                if (Convert.ToInt32(value) > CliUtils.fPassWordExpiry)
                {
                    SYS_LANGUAGE language = CliUtils.fClientLang;
                    string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "PasswordAnnulment");
                    MessageBox.Show(message);
                    CliUtils.CallMethod("GLModule", "LogOut", new object[] { (object)(CliUtils.fLoginUser) });
                    frmUserPWD fupwd = new frmUserPWD();
                    fupwd.ShowDialog();
                    if (!fupwd.upwdControl1.isOK)
                        Environment.Exit(0);
                }
                else if ((CliUtils.fPassWordExpiry - Convert.ToInt32(value)) <= CliUtils.fPassWordNotify)
                {
                    SYS_LANGUAGE language = CliUtils.fClientLang;
                    string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "PasswordNotify");
                    MessageBox.Show(String.Format(message, CliUtils.fPassWordExpiry - Convert.ToInt32(value)));
                }
            }
        }

        private string SetMenuLanguage()
        {
            string strCaption = "";
            switch (CliUtils.fClientLang)
            {
                case SYS_LANGUAGE.ENG:
                    strCaption = "CAPTION0";
                    break;
                case SYS_LANGUAGE.TRA:
                    strCaption = "CAPTION1";
                    break;
                case SYS_LANGUAGE.SIM:
                    strCaption = "CAPTION2";
                    break;
                case SYS_LANGUAGE.HKG:
                    strCaption = "CAPTION3";
                    break;
                case SYS_LANGUAGE.JPN:
                    strCaption = "CAPTION4";
                    break;
                case SYS_LANGUAGE.LAN1:
                    strCaption = "CAPTION5";
                    break;
                case SYS_LANGUAGE.LAN2:
                    strCaption = "CAPTION6";
                    break;
                case SYS_LANGUAGE.LAN3:
                    strCaption = "CAPTION7";
                    break;
            }
            return strCaption;
        }

        private void initializeMenu()
        {
            for (int i = 0; i < ListMainID.Count; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = ListMainCaption[i].ToString();
                item.Image = IconList.Images[ListMainID[i].ToString()];
                item.Tag = ListMainID[i].ToString();
                item.Click += new EventHandler(menu_Click);
                this.menuStrip1.Items.Insert(this.menuStrip1.Items.Count - 2, item);
            }

            List<ToolStripItem> emptyItems = new List<ToolStripItem>();

            for (int i = 0; i < menuStrip1.Items.Count; i++)
            {
                if (menuStrip1.Items[i].Tag != null)
                {
                    InitializeItem((ToolStripMenuItem)menuStrip1.Items[i]);
                    if (IsEmptyFolderItem((ToolStripMenuItem)menuStrip1.Items[i]))
                    {
                        emptyItems.Add(menuStrip1.Items[i]);
                    }
                }
            }
            foreach (ToolStripMenuItem item in emptyItems)
            {
                menuStrip1.Items.Remove(item);
            }
        }

        private bool IsEmptyFolderItem(ToolStripMenuItem item)
        {
            if (item != null && item.DropDownItems.Count == 0)
            {
                if (item.Tag != null)
                {
                    DataRow[] dr = menuDataSet.Tables[0].Select(string.Format("MENUID='{0}'", item.Tag));
                    if (dr.Length > 0)
                    {
                        return (dr[0]["PACKAGE"] == DBNull.Value || dr[0]["PACKAGE"].ToString().Length == 0);
                    }
                }
            }
            return false;
        }

        private void InitializeItem(ToolStripMenuItem item)
        {
            for (int i = 0; i < ListChildrenID.Count; i++)
            {
                if (item.Tag.ToString() == ListOwnerParentID[i].ToString())
                {
                    ToolStripMenuItem itemChild = new ToolStripMenuItem();
                    itemChild.Text = ListChildrenCaption[i].ToString();
                    itemChild.Image = IconList.Images[ListChildrenID[i].ToString()];
                    itemChild.Tag = ListChildrenID[i].ToString();
                    itemChild.Click += new EventHandler(menu_Click);
                    item.DropDownItems.Add(itemChild);
                }
            }

            for (int i = 0; i < item.DropDownItems.Count; i++)
            {
                InitializeItem((ToolStripMenuItem)item.DropDownItems[i]);
            }
        }

        private void initializeTreeView()
        {
            for (int i = 0; i < ListMainID.Count; i++)
            {
                TreeNode nodeMain = new TreeNode();
                tView.Nodes.Add(nodeMain);
                nodeMain.Text = ListMainCaption[i].ToString();
                nodeMain.SelectedImageKey = ListMainID[i].ToString();
                nodeMain.ImageKey = ListMainID[i].ToString();
                nodeMain.Tag = ListMainID[i].ToString();
            }

            List<TreeNode> emptynodes = new List<TreeNode>();
            for (int i = 0; i < tView.Nodes.Count; i++)
            {
                InitializeNode(tView.Nodes[i]);
                if (TreeViewLevel != 1)
                {
                    if (IsEmptyFolderNode(tView.Nodes[i]))
                    {
                        emptynodes.Add(tView.Nodes[i]);
                    }
                } 
            }
            foreach (TreeNode node in emptynodes)
            {
                tView.Nodes.Remove(node);
            }
            tView.ExpandAll();
        }

        private bool IsEmptyFolderNode(TreeNode node)
        {
            if (node != null && node.Nodes.Count == 0)
            {
                if (node.Tag != null)
                {
                    DataRow[] dr = menuDataSet.Tables[0].Select(string.Format("MENUID='{0}'", node.Tag));
                    if (dr.Length > 0)
                    {
                        return (dr[0]["PACKAGE"] == DBNull.Value || dr[0]["PACKAGE"].ToString().Length == 0);
                    }
                }
            }
            return false;
        }

        private void InitializeNode(TreeNode node)
        {
            if (TreeViewLevel == -1 || node.Level < TreeViewLevel - 1)
            {
                for (int i = 0; i < ListChildrenID.Count; i++)
                {
                    if (node.Tag.ToString() == ListOwnerParentID[i].ToString())
                    {
                        TreeNode nodeChild = new TreeNode();
                        nodeChild.Text = ListChildrenCaption[i].ToString();
                        nodeChild.SelectedImageKey = ListChildrenID[i].ToString();
                        nodeChild.ImageKey = ListChildrenID[i].ToString();
                        nodeChild.Tag = ListChildrenID[i].ToString();
                        node.Nodes.Add(nodeChild);
                    }
                }

                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    InitializeNode(node.Nodes[i]);
                }
            }
        }

        void frmClientMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CliUtils.fLoginUser.Length > 0)
            {
                if (!CliUtils.closeProtected)
                    CliUtils.CallMethod("GLModule", "LogOut", new object[] { (object)(CliUtils.fLoginUser) });
            }
        }

        private void menu_Click(object sender, EventArgs e)
        {
            string strText = ((ToolStripMenuItem)sender).Tag.ToString();
            showForm(strText, ((ToolStripMenuItem)sender).Text);
        }

        private void tView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (tView.SelectedNode != null)
            {
                int ChildrenNum = tView.SelectedNode.GetNodeCount(true);
                if (ChildrenNum == 0)
                {
                    string strText = ((TreeView)sender).SelectedNode.Tag.ToString();
                    showForm(strText, ((TreeView)sender).SelectedNode.Text);
                }
            }
        }

        public class FormItem : InfoOwnerCollectionItem
        {
            private string fPackageName = "";
            public string PackageName
            {
                get
                {
                    return fPackageName;
                }
                set
                {
                    fPackageName = value;
                }
            }

            private string fFormName = "";
            public string FormName
            {
                get
                {
                    return fFormName;
                }
                set
                {
                    fFormName = value;
                }
            }

            public override string Name
            {
                get
                {
                    return fPackageName + "." + fFormName;
                }
                set
                {
                    int iPos = value.IndexOf('.');
                    if (-1 != iPos)
                    {
                        fPackageName = value.Substring(0, iPos);
                        fFormName = value.Substring(iPos + 1);
                    }
                }
            }

            private bool fMultiInstance = false;
            public bool MultiInstance
            {
                get
                {
                    return fMultiInstance;
                }
                set
                {
                    fMultiInstance = value;
                }
            }

            private int fCount = 0;
            public int Count
            {
                get
                {
                    return fCount;
                }
                set
                {
                    fCount = value;
                }
            }
        }

        public class FormCollection : InfoOwnerCollection
        {
            public FormCollection(Component aOwner, Type aItemType)
                : base(aOwner, typeof(FormItem))
            {

            }

            new public FormItem this[int index]
            {
                get
                {
                    return (FormItem)InnerList[index];
                }
                set
                {
                    if (index > -1 && index < Count)
                        if (value is FormItem)
                        {
                            //原来的Collection设置为0
                            ((FormItem)InnerList[index]).Collection = null;
                            InnerList[index] = value;
                            //Collection设置为this
                            ((FormItem)InnerList[index]).Collection = this;
                        }
                }
            }

            public FormItem FindFormItem(string pkg, string frm)
            {
                FormItem fRet = null;
                foreach (FormItem f in this)
                {
                    if (string.Compare(f.PackageName, pkg, true) == 0 && string.Compare(f.FormName, frm, true) == 0)//IgnoreCase
                    {
                        fRet = f;
                        break;
                    }
                }
                return fRet;
            }

            public FormItem AddPackageForm(string pkg, string frm)
            {
                FormItem fRet = FindFormItem(pkg, frm);

                if (null == fRet)
                {
                    fRet = new FormItem();
                    fRet.PackageName = pkg; fRet.FormName = frm;
                    this.Add(fRet);
                }

                fRet.Count++;
                return fRet;
            }

            public void RemovePackageForm(string pkg, string frm)
            {
                FormItem fRet = FindFormItem(pkg, frm);

                if (null != fRet)
                {
                    fRet.Count--;
                    if (fRet.Count == 0)
                        this.Remove(fRet);
                }
            }
        }

        private void InternalFormClosed(object sender, FormClosedEventArgs e)
        {
            IInfoForm aForm = (IInfoForm)sender;
            fFormCollection.RemovePackageForm(aForm.GetPackageName(), aForm.GetFormName());
        }

        private bool bAbort = false;
        private void ShowProgressBar()
        {
            ProgressForm aForm = new ProgressForm();
            aForm.Show();
            while (!bAbort || aForm.progressBar1.Value < 99)
            {
                if (aForm.progressBar1.Value + 3 > 100)
                {
                    aForm.progressBar1.Value = 1;
                }
                else
                {
                    aForm.progressBar1.Value += 3;
                }
                Thread.Sleep(5);
            }
        }

        private void CheckAndDownLoad(string sFullPath, string sDll)
        {
            object[] oRet = null;
            DateTime d = File.GetLastWriteTime(sFullPath);//取得最后时间

            Thread t = new Thread(new ThreadStart(ShowProgressBar));
            t.Start();
            try
            {
                oRet = CliUtils.CallMethod("GLModule", "CheckAndDownLoad", new object[] { CliUtils.fCurrentProject, sDll, d });
            }
            finally
            {
                bAbort = true;
                t.Join();
            }

            if (null != oRet && ((int)oRet[0] == 0) && ((int)oRet[1] == 0))
            {
                byte[] bs = (byte[])oRet[3];
                DateTime ds = (DateTime)oRet[2];
                string sPath = Path.GetDirectoryName(sFullPath);
                if (!Directory.Exists(sPath))
                    Directory.CreateDirectory(sPath);
                File.WriteAllBytes(sFullPath, bs);
                File.SetLastWriteTime(sFullPath, ds);
            }
        }

        private void showForm(string id, string text)
        {
            string PackageName = "";
            string FormName = "";
            string ItemParam = "";
            int i = menuDataSet.Tables[0].Rows.Count;
            for (int j = 0; j < i; j++)
            {
                if (id == menuDataSet.Tables[0].Rows[j]["MENUID"].ToString())
                {
                    PackageName = menuDataSet.Tables[0].Rows[j]["PACKAGE"].ToString();
                    FormName = menuDataSet.Tables[0].Rows[j]["FORM"].ToString();
                    ItemParam = menuDataSet.Tables[0].Rows[j]["ITEMPARAM"].ToString();
                    break;
                }
            }
            if (PackageName == "")
            {
                return;
            }
            FormItem f = fFormCollection.FindFormItem(PackageName, FormName);
            if ((null != f) && (!f.MultiInstance))
            {
                foreach (Form frm in this.MdiChildren)
                {
                    Type t = frm.GetType();
                    if (t.Namespace == f.PackageName && frm.Name == f.FormName)
                    {
                        if (frm.WindowState == FormWindowState.Minimized)
                        {
                            frm.WindowState = FormWindowState.Normal;
                        }
                        frm.Activate();
                    }
                }
                return;//如果已经有对象，并且不许多个，则退出
            }

            String s;
            s = Application.StartupPath + "\\" + CliUtils.fCurrentProject + "\\";

            string strPackage = s + PackageName + ".dll";
            Assembly a = null;
            bool bLoaded = DllContainer.DllLoaded(strPackage);

            if (!bLoaded || !File.Exists(strPackage)) CheckAndDownLoad(strPackage, PackageName + ".dll");
            try
            {
                a = Assembly.LoadFrom(strPackage);
            }
            finally
            {
                if (!bLoaded) DllContainer.AddDll(strPackage);
            }

            Type myType = a.GetType(PackageName + "." + FormName);
            if (myType != null)
            {
                object obj = Activator.CreateInstance(myType);
                PropertyInfo myprop = myType.GetProperty("MdiParent");
                myprop.SetValue(obj, this, null);

                ((IInfoForm)obj).SetPackageForm(PackageName, FormName);
                fFormCollection.AddPackageForm(PackageName, FormName).MultiInstance = ((IInfoForm)obj).GetMultiInstance();
                ((Form)obj).FormClosed += InternalFormClosed;
                ((InfoForm)obj).ItemParamters = ItemParam;
                if (((InfoForm)obj).ShowMenuText)
                {
                    ((InfoForm)obj).Text = text;
                }
                ((Control)obj).Show();
                if (((Form)obj).WindowState == FormWindowState.Maximized)
                {
                    ((Form)obj).Hide();
                    ((Form)obj).WindowState = FormWindowState.Normal;
                    ((Form)obj).WindowState = FormWindowState.Maximized;
                    ((Form)obj).Show();
                }
            }
            //new add by ccm, indicate form doesn't exist
            else
            {
                MessageBox.Show(string.Format("Form: {0} doesn't exist", FormName));
            } //end add
        }

        public ComboBox cmbSolution
        {
            get
            {
                return this.infoCmbSolution;
            }
        }

        private void menuItemSolution_Click(object sender, EventArgs e)
        {
            CurProject aForm = new CurProject(this);
            aForm.ShowDialog();
        }

        private void menuItemDataBase_Click(object sender, EventArgs e)
        {
            frmDataBase dbForm = new frmDataBase(this);
            dbForm.ShowDialog();
        }

        private void menuItemUG_Click(object sender, EventArgs e)
        {
            frmUsersAndGroups ugForm = new frmUsersAndGroups();
            ugForm.ShowDialog();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void infoCmbSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (solutionLoad)
            {
                int length = this.MdiChildren.Length;
                for (int i = 0; i < length; i++)
                {
                    this.MdiChildren[0].Close();
                    if (i + MdiChildren.Length >= length)
                    {
                        cmbSolution.SelectedIndexChanged -= new EventHandler(infoCmbSolution_SelectedIndexChanged);
                        this.cmbSolution.SelectedValue = CliUtils.fCurrentProject;//设回原来的solution
                        cmbSolution.SelectedIndexChanged += new EventHandler(infoCmbSolution_SelectedIndexChanged);
                        return;
                    }
                }
                ItemToGet();
                if (this.cmbSolution.SelectedValue == null)
                    CliUtils.fCurrentProject = "";
                else
                    CliUtils.fCurrentProject = this.cmbSolution.SelectedValue.ToString();
            }
        }

        private void testToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmMessage frm = new frmMessage();

            DataSet ds = CliUtils.GetMessage();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                frm.dataGridView1.DataSource = ds;
                frm.dataGridView1.DataMember = "Message";
                frm.Location = new Point(Screen.AllScreens[0].WorkingArea.Width - frm.Size.Width, Screen.AllScreens[0].WorkingArea.Height - frm.Size.Height);
                frm.Show();
            }
        }

        private void tmMessage_Tick(object sender, EventArgs e)
        {
            frmMessage frm = new frmMessage();

            DataSet ds = CliUtils.GetMessage();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                frm.dataGridView1.DataSource = ds;
                frm.dataGridView1.DataMember = "Message";
                frm.Location = new Point(Screen.AllScreens[0].WorkingArea.Width - frm.Size.Width, Screen.AllScreens[0].WorkingArea.Height - frm.Size.Height);
                frm.Show();
            }
        }

        private int lx;
        private int sx;
        bool flag = false;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (flag == false)
            {
                lx = pictureBox1.Location.X;
                sx = panel1.Size.Width;
                this.pictureBox1.Image = global::EEPNetClient.Properties.Resources.d1;
                this.pictureBox1.Location = new System.Drawing.Point(0, this.pictureBox1.Location.Y);
                this.panel1.Size = new System.Drawing.Size(this.pictureBox1.Size.Width, this.panel1.Size.Height);
                flag = true;
            }
            else
            {
                this.pictureBox1.Image = global::EEPNetClient.Properties.Resources.d2;
                this.pictureBox1.Location = new System.Drawing.Point(lx, this.pictureBox1.Location.Y);
                this.panel1.Size = new System.Drawing.Size(sx, this.panel1.Size.Height);
                flag = false;
            }
            this.Refresh();
        }

        private void menuItemCP_Click(object sender, EventArgs e)
        {
            frmUserPWD fupwd = new frmUserPWD();
            fupwd.ShowDialog();
        }

        private void tView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                for (int i = 0; i < ListChildrenID.Count; i++)
                {
                    if (e.Node.Tag.ToString() == ListOwnerParentID[i].ToString())
                    {
                        TreeNode nodeChild = new TreeNode();
                        nodeChild.Text = ListChildrenCaption[i].ToString();
                        nodeChild.SelectedImageKey = ListChildrenID[i].ToString();
                        nodeChild.ImageKey = ListChildrenID[i].ToString();
                        nodeChild.Tag = ListChildrenID[i].ToString();
                        e.Node.Nodes.Add(nodeChild);
                    }
                }
            }
        }

        private void menuItemTileH_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void menuItemTileV_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void pbMyFavor_Click(object sender, EventArgs e)
        {
            frmFavorMenu ffm = new frmFavorMenu(MenuIDList, CaptionList, ParentList);
            ffm.ShowDialog();
            if (ffm.result)
            {
                ItemToGet();
            }
        }

        private void pbGo_Click(object sender, EventArgs e)
        {
            if (tbGO.Text == "")
            {
                MessageBox.Show("Please enter a menu first.");
            }
            else
            {
                Boolean flag = false;
                for (int i = 0; i < this.tView.Nodes.Count; i++)
                {
                    if (compareCaption(tbGO.Text, this.tView.Nodes[i].Text.ToString()))
                    {
                        showForm(this.tView.Nodes[i].Tag.ToString(), tView.Nodes[i].Text);
                        flag = true;
                        break;
                    }
                    flag = getChildNode(this.tView.Nodes[i], tbGO.Text);
                    if (flag) break;
                }
                if (!flag)
                {
                    MessageBox.Show("The menu you entered is not exist.");
                }
            }
        }

        public Boolean getChildNode(TreeNode tn, String text)
        {
            if (compareCaption(text, tn.Text.ToString()))
            {
                showForm(tn.Tag.ToString(), tn.Text);
                return true; ;
            }
            if (tn.Nodes.Count > 0)
            {
                for (int i = 0; i < tn.Nodes.Count; i++)
                {
                    if (compareCaption(text, tn.Nodes[i].Text.ToString()))
                    {
                        showForm(tn.Nodes[i].Tag.ToString(), tn.Nodes[i].Text);
                        return true; ;
                    }
                    if (getChildNode(tn.Nodes[i], text)) return true;
                }
            }
            return false;
        }

        private bool compareCaption(String text, String nodeText)
        {
            if (nodeText.StartsWith(text))
                return true;
            else
                return false;
        }

        private void pbGo_MouseMove(object sender, MouseEventArgs e)
        {
            this.pbGo.Image = Properties.Resources.MenuGo2;
            //string s = Application.StartupPath + "\\Resources\\MenuGO2.gif";
            //this.pbGo.ImageLocation = s;
        }

        private void pbGo_MouseLeave(object sender, EventArgs e)
        {
            this.pbGo.Image = Properties.Resources.MenuGo;
            //string s = Application.StartupPath + "\\Resources\\MenuGO.gif";
            //this.pbGo.ImageLocation = s;
        }

        private void pbMyFavor_MouseMove(object sender, MouseEventArgs e)
        {
            this.pbMyFavor.Image = Properties.Resources.AddFavor2;
            //string s = Application.StartupPath + "\\Resources\\AddFavor2.png";
            //this.pbMyFavor.ImageLocation = s;
        }

        private void pbMyFavor_MouseLeave(object sender, EventArgs e)
        {
            this.pbMyFavor.Image = Properties.Resources.AddFavor;
            //string s = Application.StartupPath + "\\Resources\\AddFavor.png";
            //this.pbMyFavor.ImageLocation = s;
        }

        private void tbGO_TextChanged(object sender, EventArgs e)
        {

        }

        private void treeViewToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            this.panel1.Visible = treeViewToolStripMenuItem.Checked;
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            FormVersion form = new FormVersion("About EEP.Net Client");
            form.ShowDialog(this);
        }

        private void frmClientMain_SizeChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }

    public static class DllContainer
    {
        public static ArrayList NameList = new ArrayList();

        public static bool DllLoaded(string sName)
        {
            bool bRet = false;
            for (int i = 0; i < NameList.Count; i++)
            {
                if (string.Compare(((string)NameList[i]).Trim(), sName.Trim(), true) == 0)//IgnoreCase
                {
                    bRet = true;
                    break;
                }
            }

            return bRet;
        }

        public static void AddDll(string sName)
        {
            NameList.Add(sName);
        }
    }

}