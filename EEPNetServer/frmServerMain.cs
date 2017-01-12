using System;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Runtime.Remoting;

using System.IO;
using System.Reflection;
using System.Xml;
using System.Threading;
using Srvtools;
using System.Diagnostics;
using System.Collections.Generic;


#if WCF
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Data.Services;
#endif

namespace EEPNetServer
{
    public class frmServerMain : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.StatusBar StatusBar;
        private System.Windows.Forms.Timer tmUser;
        private StatusBarPanel sbp1;
        private StatusBarPanel sbp2;
        private ListView lvUsers;
        private ColumnHeader chUserId;
        private ColumnHeader chUserName;
        private ColumnHeader chComputer;
        private ColumnHeader chLoginTime;
        private ColumnHeader chCount;
        private ContextMenuStrip clearContextMenu;
        private ToolStripMenuItem cleanToolStripMenuItem;
        private MainMenu mainMenu;
        private MenuItem mFile;
        private MenuItem mDBMan;
        private MenuItem menuItem1;
        private MenuItem menuItem3;
        private MenuItem menuItemServerConfig;
        private MenuItem menuItemPackageTranfserInfo;
        private MenuItem mExit;
        private MenuItem mHelp;
        private MenuItem mAbout;
        private MenuItem menuItem2;
        private MenuItem menuItemExitandUpdate;
        private System.Windows.Forms.Timer tmMaxTimeOut;
        private MenuItem menuItemClientUpdate;
        private MenuItem menuItemWorkflowConfig;
        private MenuItem menuItemAboutEEPNetServer;
        private ToolTip toolTip;
        private ToolStripMenuItem detailsToolStripMenuItem;
        private StatusBarPanel sbp3;
        private MenuItem menuItem4;
        private MenuItem menuItemPackageManager;
        private MenuItem menuItem5;
        private MenuItem menuItemIDC;
        private MenuItem menuItem6;
        private MenuItem menuItem7;
        private ToolStripMenuItem clearAllToolStripMenuItem;

        public frmServerMain()
        {
            InitializeComponent();
        }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmServerMain));
            this.StatusBar = new System.Windows.Forms.StatusBar();
            this.sbp1 = new System.Windows.Forms.StatusBarPanel();
            this.sbp2 = new System.Windows.Forms.StatusBarPanel();
            this.sbp3 = new System.Windows.Forms.StatusBarPanel();
            this.tmUser = new System.Windows.Forms.Timer(this.components);
            this.lvUsers = new System.Windows.Forms.ListView();
            this.chUserId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chUserName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chComputer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLoginTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clearContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.mFile = new System.Windows.Forms.MenuItem();
            this.mDBMan = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItemServerConfig = new System.Windows.Forms.MenuItem();
            this.menuItemWorkflowConfig = new System.Windows.Forms.MenuItem();
            this.menuItemClientUpdate = new System.Windows.Forms.MenuItem();
            this.menuItemPackageTranfserInfo = new System.Windows.Forms.MenuItem();
            this.mExit = new System.Windows.Forms.MenuItem();
            this.menuItemExitandUpdate = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItemPackageManager = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItemIDC = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.mHelp = new System.Windows.Forms.MenuItem();
            this.mAbout = new System.Windows.Forms.MenuItem();
            this.menuItemAboutEEPNetServer = new System.Windows.Forms.MenuItem();
            this.tmMaxTimeOut = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.sbp1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbp2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbp3)).BeginInit();
            this.clearContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusBar
            // 
            this.StatusBar.Location = new System.Drawing.Point(0, 270);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.sbp1,
            this.sbp2,
            this.sbp3});
            this.StatusBar.ShowPanels = true;
            this.StatusBar.Size = new System.Drawing.Size(517, 23);
            this.StatusBar.TabIndex = 1;
            // 
            // sbp1
            // 
            this.sbp1.Name = "sbp1";
            this.sbp1.Text = "Welcome to Infolight.EEP.net";
            this.sbp1.Width = 180;
            // 
            // sbp2
            // 
            this.sbp2.Name = "sbp2";
            this.sbp2.Text = "Serial No:  [Lite001]";
            this.sbp2.Width = 300;
            // 
            // sbp3
            // 
            this.sbp3.Name = "sbp3";
            // 
            // tmUser
            // 
            this.tmUser.Interval = 3000;
            this.tmUser.Tick += new System.EventHandler(this.tmUser_Tick);
            // 
            // lvUsers
            // 
            this.lvUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chUserId,
            this.chUserName,
            this.chComputer,
            this.chLoginTime,
            this.chCount});
            this.lvUsers.ContextMenuStrip = this.clearContextMenu;
            this.lvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvUsers.FullRowSelect = true;
            this.lvUsers.GridLines = true;
            this.lvUsers.HideSelection = false;
            this.lvUsers.Location = new System.Drawing.Point(0, 0);
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.Size = new System.Drawing.Size(517, 270);
            this.lvUsers.TabIndex = 2;
            this.lvUsers.UseCompatibleStateImageBehavior = false;
            this.lvUsers.View = System.Windows.Forms.View.Details;
            this.lvUsers.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvUsers_MouseClick);
            // 
            // chUserId
            // 
            this.chUserId.Text = "User Id";
            // 
            // chUserName
            // 
            this.chUserName.Text = "User Name";
            this.chUserName.Width = 100;
            // 
            // chComputer
            // 
            this.chComputer.Text = "Computer";
            this.chComputer.Width = 150;
            // 
            // chLoginTime
            // 
            this.chLoginTime.Text = "Login Time";
            this.chLoginTime.Width = 150;
            // 
            // chCount
            // 
            this.chCount.Text = "Count";
            this.chCount.Width = 50;
            // 
            // clearContextMenu
            // 
            this.clearContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.detailsToolStripMenuItem,
            this.cleanToolStripMenuItem,
            this.clearAllToolStripMenuItem});
            this.clearContextMenu.Name = "clearContextMenu";
            this.clearContextMenu.Size = new System.Drawing.Size(125, 70);
            this.clearContextMenu.Opened += new System.EventHandler(this.clearContextMenu_Opened);
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.detailsToolStripMenuItem.Text = "Details";
            this.detailsToolStripMenuItem.Click += new System.EventHandler(this.detailsToolStripMenuItem_Click);
            // 
            // cleanToolStripMenuItem
            // 
            this.cleanToolStripMenuItem.Name = "cleanToolStripMenuItem";
            this.cleanToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.cleanToolStripMenuItem.Text = "Clear";
            this.cleanToolStripMenuItem.Click += new System.EventHandler(this.cleanToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mFile,
            this.menuItem4,
            this.mHelp});
            // 
            // mFile
            // 
            this.mFile.Index = 0;
            this.mFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mDBMan,
            this.menuItem1,
            this.menuItem3,
            this.menuItem2,
            this.menuItemServerConfig,
            this.menuItemWorkflowConfig,
            this.menuItemClientUpdate,
            this.menuItemPackageTranfserInfo,
            this.mExit,
            this.menuItemExitandUpdate});
            this.mFile.Text = "&File";
            // 
            // mDBMan
            // 
            this.mDBMan.Index = 0;
            this.mDBMan.Text = "DB Manager";
            this.mDBMan.Click += new System.EventHandler(this.mDBMan_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "Package Manager";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "System Log Manager";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.Text = "Login Manager";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click_1);
            // 
            // menuItemServerConfig
            // 
            this.menuItemServerConfig.Index = 4;
            this.menuItemServerConfig.Text = "Server Config";
            this.menuItemServerConfig.Click += new System.EventHandler(this.menuItemServerConfig_Click);
            // 
            // menuItemWorkflowConfig
            // 
            this.menuItemWorkflowConfig.Index = 5;
            this.menuItemWorkflowConfig.Text = "Workflow Config";
            this.menuItemWorkflowConfig.Click += new System.EventHandler(this.menuItemWorkflowConfig_Click);
            // 
            // menuItemClientUpdate
            // 
            this.menuItemClientUpdate.Index = 6;
            this.menuItemClientUpdate.Text = "Client Update Manager";
            this.menuItemClientUpdate.Click += new System.EventHandler(this.menuItemClientUpdate_Click);
            // 
            // menuItemPackageTranfserInfo
            // 
            this.menuItemPackageTranfserInfo.Index = 7;
            this.menuItemPackageTranfserInfo.Text = "Package Transfer Info";
            this.menuItemPackageTranfserInfo.Click += new System.EventHandler(this.menuItemPackageTranfserInfo_Click);
            // 
            // mExit
            // 
            this.mExit.Index = 8;
            this.mExit.Text = "E&xit";
            this.mExit.Click += new System.EventHandler(this.mExit_Click);
            // 
            // menuItemExitandUpdate
            // 
            this.menuItemExitandUpdate.Index = 9;
            this.menuItemExitandUpdate.Text = "Update Package";
            this.menuItemExitandUpdate.Click += new System.EventHandler(this.menuItemExitandUpdate_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPackageManager,
            this.menuItem5,
            this.menuItemIDC,
            this.menuItem6,
            this.menuItem7});
            this.menuItem4.Text = "WCF";
            // 
            // menuItemPackageManager
            // 
            this.menuItemPackageManager.Index = 0;
            this.menuItemPackageManager.Tag = "WCFServer.FormPackage";
            this.menuItemPackageManager.Text = "Package Manager";
            this.menuItemPackageManager.Visible = false;
            this.menuItemPackageManager.Click += new System.EventHandler(this.menuItemWCF_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.Tag = "WCFServer.FormService";
            this.menuItem5.Text = "Service Manager";
            this.menuItem5.Visible = false;
            this.menuItem5.Click += new System.EventHandler(this.menuItemWCF_Click);
            // 
            // menuItemIDC
            // 
            this.menuItemIDC.Index = 2;
            this.menuItemIDC.Text = "Install Design Component";
            this.menuItemIDC.Click += new System.EventHandler(this.menuItemIDC_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 3;
            this.menuItem6.Tag = "WCFServer.FormInitial";
            this.menuItem6.Text = "Set WebClient Path";
            this.menuItem6.Visible = false;
            this.menuItem6.Click += new System.EventHandler(this.menuItemWCF_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 4;
            this.menuItem7.Tag = "WCFServer.FormSDOption";
            this.menuItem7.Text = "SD Assemly Option";
            this.menuItem7.Visible = false;
            this.menuItem7.Click += new System.EventHandler(this.menuItemWCF_Click);
            // 
            // mHelp
            // 
            this.mHelp.Index = 2;
            this.mHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mAbout,
            this.menuItemAboutEEPNetServer});
            this.mHelp.Text = "&Help";
            // 
            // mAbout
            // 
            this.mAbout.Index = 0;
            this.mAbout.Text = "&About";
            this.mAbout.Click += new System.EventHandler(this.mAbout_Click);
            // 
            // menuItemAboutEEPNetServer
            // 
            this.menuItemAboutEEPNetServer.Index = 1;
            this.menuItemAboutEEPNetServer.Text = "About EEP.Net Server";
            this.menuItemAboutEEPNetServer.Click += new System.EventHandler(this.menuItemAboutEEPNetServer_Click);
            // 
            // tmMaxTimeOut
            // 
            this.tmMaxTimeOut.Interval = 60000;
            this.tmMaxTimeOut.Tick += new System.EventHandler(this.tmMaxTimeOut_Tick);
            // 
            // frmServerMain
            // 
            this.ClientSize = new System.Drawing.Size(517, 293);
            this.Controls.Add(this.lvUsers);
            this.Controls.Add(this.StatusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.Name = "frmServerMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EEP.NET Server (Ver:1.0.0.0)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmServerMain_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sbp1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbp2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbp3)).EndInit();
            this.clearContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        [STAThread]
        static void Main()
        {
            try
            {
                RemotingConfiguration.Configure(string.Format("{0}\\EEPNetServer.exe.config", EEPRegistry.Server), false);
            }
            catch (RemotingException)
            {
                MessageBox.Show("EEPNetServer can run once!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(string.Format("{0}\\EEPNetServer.exe.config", EEPRegistry.Server));

            if (xmlDoc != null && xmlDoc.FirstChild != null)
            {
                XmlNode n = xmlDoc.SelectSingleNode("configuration/system.runtime.remoting/application/channels/channel");
                if (n != null)
                {
                    string port = n.Attributes["port"].InnerText;
                    SrvUtils.RemotePort = Convert.ToInt32(port);
                }
            }

            SrvGL.LoadUsers();

           
#if UseFL
            Assembly assembly = Assembly.LoadFrom("FLRuntime.dll");

            object persistenceService = assembly.CreateInstance("FLRuntime.Hosting.FLPersistenceService");

            object runtime = assembly.CreateInstance("FLRuntime.FLRuntime");
            Type runtimeType = assembly.GetType("FLRuntime.FLRuntime");
            
            MethodInfo addService = runtimeType.GetMethod("AddService");
            addService.Invoke(runtime, new object[] { persistenceService });

            MethodInfo start = runtimeType.GetMethod("Start");
            start.Invoke(runtime, new object[] { });
#endif
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmServerMain());
        }

        Form formupdateserver = null;
        Form formwcfserver = null;
        private void MainForm_Load(object sender, System.EventArgs e)
        {
            if (!x64f2717168e0a936.x2402b6eabad5b567(true))
            {
                MessageBox.Show("EEP is not registered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (SysEEPLogService.Enable)
            {
                SysEEPLog eeplog = new SysEEPLog(null, SysEEPLog.LogStyleType.System
                    , SysEEPLog.LogTypeType.Normal, DateTime.Now, "Server Start", string.Empty);
                eeplog.Log();
            }
            // 在Users.xml加载到内存中去。
            tmUser.Enabled = true;
            tmUser_Tick(tmUser, null);
            tmMaxTimeOut.Enabled = true;
            tmMaxTimeOut_Tick(tmMaxTimeOut, null);
            this.StatusBar.Panels[1].Text = string.Format("Licensed To {0}: [{1}]", x64f2717168e0a936.Company, x64f2717168e0a936.Text);
            this.Text = string.Format("EEP.NET Server(Version:{0})", GetFileVersion(typeof(CliUtils).Assembly));
            //加载EEPClient Update Server
            try
            {
                //Assembly assembly = Assembly.LoadFrom(EEPRegistry.Server + "\\EEPSetUpLibrary.dll");
                //frmupdateserver = (Form)assembly.CreateInstance("EEPSetUpLibrary.Server.frmMain");
                //frmupdateserver.Show(this);
                //frmupdateserver.Hide();
                formupdateserver = ShowForm("EEPSetUpLibrary", "EEPSetUpLibrary.Server.frmMain");
            }
            catch
            {
                menuItemClientUpdate.Enabled = false;
            }
            try
            {
                //formwcfserver = ShowForm("WCFServer", "WCFServer.FormMain");
                Assembly assembly = Assembly.LoadFrom(string.Format(@"{0}\{1}.dll", EEPRegistry.Server, "EFWCFModule"));
                var start = assembly.GetType("EFWCFModule.ServiceProvider").GetMethod("StartService", new Type[] { typeof(Type), typeof(Type[]) });
                start.Invoke(null, new object[] { assembly.GetType("EFWCFModule.EFService"), new Type[] { assembly.GetType("EFWCFModule.IEFService") } });
            }
            catch { }

            try
            {
                ShowForm("EEPCloudSetupLibrary", "EEPCloudSetupLibrary.FormServer");
            }
            catch { }


            if (!Directory.Exists(Application.ExecutablePath.Substring(0, Application.ExecutablePath.IndexOf("\\EEPNetServer")) + "\\EFWCFModule"))
                menuItem4.Enabled = false;
            ConnectRemoteServers();
        }

        private string GetFileVersion(Assembly assembly)
        {
            bool isdef = Attribute.IsDefined(assembly,  typeof(AssemblyDescriptionAttribute));
            string assyName = assembly.GetName().Name;

            if (isdef)
            {
                AssemblyDescriptionAttribute adAttr = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute));
                return adAttr.Description;
            }
            else
            {
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
                return info.FileVersion;
            }
        }

        private Form ShowForm(string assemblyName, string typeName)
        {
            Assembly assembly = Assembly.LoadFrom(string.Format(@"{0}\{1}.dll", EEPRegistry.Server, assemblyName));
            Form form  = (Form)assembly.CreateInstance(typeName);
            //form.WindowState = FormWindowState.Minimized;
            form.Show(this);
            form.Hide();
            return form;
        }

        private void ShowDialog(string assemblyName, string typeName)
        {
            Assembly assembly = Assembly.LoadFrom(string.Format(@"{0}\{1}.dll", EEPRegistry.Server, assemblyName));
            Form form = (Form)assembly.CreateInstance(typeName);
            form.ShowDialog(this);
        }

        private void ConnectRemoteServers()
        {
            string s = SystemFile.ServerConfigFile;
            XmlDocument CfgXml = new XmlDocument();
            if (File.Exists(s))
            {
                CfgXml.Load(s);
                XmlNode aNode = CfgXml.DocumentElement.FirstChild;
                while (aNode != null)
                {
                    var serverType = aNode.LocalName;
                    if (string.Compare(serverType, "remoteserver", true) == 0)//IgnoreCase
                    {
                        bool active = aNode.Attributes["Active"] == null ? false : bool.Parse(aNode.Attributes["Active"].Value);
                        string ipAddress = aNode.Attributes["IpAddress"].InnerText;
                        try
                        {
                            string ip = aNode.Attributes["IpAddress"].Value;
                            int port = (aNode.Attributes["Port"] != null && aNode.Attributes["Port"].Value.Length > 0)
                                ? int.Parse(aNode.Attributes["Port"].Value) : 8989;
                            var rs = new RemoteServer(0, ip, port, active);
                            frmProgress frm = new frmProgress(rs.IpAddress, rs.Port, string.Format("Connect to {0}", rs));
                            frm.StartPosition = FormStartPosition.CenterScreen;
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                Srvtools.LoginService service = frm.Module;
                                if (service != null)
                                {
                                    try
                                    {
                                        ServerConfig.RegisterRemoteServer(rs.IpAddress, rs.Port);
                                    }
                                    catch (Exception e)
                                    {
                                        //MessageBox.Show(e.Message);
                                        rs.Activated = false;
                                        ServerConfig.DeRegisterRemoteServer(rs.IpAddress, rs.Port);
                                    }
                                }
                                else
                                {
                                    rs.Activated = false;
                                    ServerConfig.DeRegisterRemoteServer(rs.IpAddress, rs.Port);
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    aNode = aNode.NextSibling;
                }
            }
        }


        private void mAbout_Click(object sender, System.EventArgs e)
        {
            frmAbout aFrmAbout = new frmAbout();
            try
            {
                aFrmAbout.ShowDialog();
                this.StatusBar.Panels[1].Text = string.Format("Licensed To {0}: [{1}]", x64f2717168e0a936.Company, x64f2717168e0a936.Text);
            }
            finally
            {
                aFrmAbout.Dispose();
            }
        }

        private void mExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void mDBMan_Click(object sender, System.EventArgs e)
        {
            frmDBMan aFrmDBMan = new frmDBMan();
            aFrmDBMan.ShowDialog();
            aFrmDBMan.Dispose();
        }

        private void lvUsers_MouseClick(object sender, MouseEventArgs e)
        {
           
        }

        bool isBusy = false;
        private void tmUser_Tick(object sender, System.EventArgs e)
        {
            if (isBusy)
            {
                return;
            }
            isBusy = true;
            //lock (this)
            //{
            try
            {
                string selectedid = lvUsers.SelectedItems.Count > 0 ? lvUsers.SelectedItems[0].Text : string.Empty;
                UserInfo[] infos = SrvGL.GetUsersInfos();
                lvUsers.Items.Clear();
                foreach (UserInfo info in infos)
                {
                    string userId = info.UserID;

                    ListViewItem aItem = lvUsers.Items.Add(userId);
                    aItem.Tag = info;
                    aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, info.UserName));
                    aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, "(Details)"));
                    aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, "(Details)"));
                    aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, info.LoginCount.ToString()));
                }
                if (selectedid.Length > 0)
                {
                    for (int i = 0; i < lvUsers.Items.Count; i++)
                    {
                        if (lvUsers.Items[i].Text == selectedid)
                        {
                            lvUsers.Items[i].Selected = true;
                            break;
                        }
                    }
                }
                if (SysEEPLogService.Enable)
                {
                    StatusBar.Panels[2].Icon = Properties.Resources.LogOn;//
                    StatusBar.Panels[2].ToolTipText = "Log is on";
                }
                else
                {
                    StatusBar.Panels[2].Icon = Properties.Resources.LogOff;
                    StatusBar.Panels[2].ToolTipText = "Log is off";
                }
            }
            finally
            {
                isBusy = false;
            }
        }
        private void menuItem1_Click(object sender, System.EventArgs e)
        {
            frmPkgMan afrmPkgMan = new frmPkgMan();
            afrmPkgMan.ShowDialog();
            afrmPkgMan.Dispose();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            frmSystemLogMan fSysLogMan = new frmSystemLogMan();
            fSysLogMan.ShowDialog();
            fSysLogMan.Dispose();
        }

        private void menuItemServerConfig_Click(object sender, EventArgs e)
        {
            frmServerConfig fServerConfig = new frmServerConfig();
            fServerConfig.ShowDialog();
            fServerConfig.Dispose();
        }

        private void menuItemPackageTranfserInfo_Click(object sender, EventArgs e)
        {
            PackageTransferInfoForm transferInfoForm = new PackageTransferInfoForm();
            transferInfoForm.ShowDialog();
        }

        private void frmServerMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing
                && MessageBox.Show(this, "Do you want to close EEPNetServer?"
             , "Close Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                if (SysEEPLogService.Enable)
                {
                    SysEEPLog eeplog = new SysEEPLog(null, SysEEPLog.LogStyleType.System
                        , SysEEPLog.LogTypeType.Normal, DateTime.Now, "Server Close", string.Empty);
                    eeplog.Log();
                }
                string s = SystemFile.PackageTransferListFile;

                if (File.Exists(s)) File.Delete(s);

                FileStream aFileStream = new FileStream(s, FileMode.Create);
                try
                {
                    XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
                    w.Formatting = Formatting.Indented;
                    w.WriteStartElement("PackageTransferList");
                    w.WriteAttributeString("AutoTransfer", ServerConfig.AutoTransferPackage.ToString());

                    DataRowCollection dataRowCollection = ServerConfig.PackageTransferList.Tables[0].Rows;
                    for (int i = 0; i < dataRowCollection.Count; i++)
                    {
                        w.WriteStartElement("Package");

                        w.WriteAttributeString("IP", dataRowCollection[i]["IP"].ToString());
                        w.WriteAttributeString("FileName", dataRowCollection[i]["FileName"].ToString());
                        w.WriteAttributeString("SolutionName", dataRowCollection[i]["SolutionName"].ToString());
                        w.WriteAttributeString("PackageType", dataRowCollection[i]["PackageType"].ToString());
                        w.WriteAttributeString("DateTime", dataRowCollection[i]["DateTime"].ToString());
                        w.WriteAttributeString("TransferInfo", dataRowCollection[i]["TransferInfo"].ToString());

                        w.WriteEndElement();
                    }

                    w.WriteEndElement();
                    w.Close();
                }
                finally
                {
                    aFileStream.Close();
                }
            }
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count > 0)
            {
                UserInfo info = lvUsers.SelectedItems[0].Tag as UserInfo;
                if (info != null)
                {
                    frmLoginComputers form = new frmLoginComputers(info);
                    form.ShowDialog(this);
                }
            }
        }


        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Clear all users?", "Confirm", MessageBoxButtons.YesNo
                , MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                lock (typeof(SrvGL))
                {
                    SrvGL.ClearUsers();
                }

                if (File.Exists(RecordLock.RecordFileName))
                {
                    RecordLock.ClearRecordFile();
                }
            }
        }

        private void cleanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string userid = "";
            if (lvUsers.SelectedItems.Count == 0 || lvUsers.SelectedItems[0].SubItems.Count == 0)
            { return; }
            else
            { userid = lvUsers.SelectedItems[0].SubItems[0].Text; }
            if (MessageBox.Show(this, string.Format("Clear user:{0}?", userid), "Confirm", MessageBoxButtons.YesNo
                , MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                SrvGL.ClearUser(userid.ToLower());
                if (File.Exists(RecordLock.RecordFileName))
                {
                    RecordLock.ClearRecordFile(userid);
                }
            }
        }

        private void clearContextMenu_Opened(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count == 0)
            {
                cleanToolStripMenuItem.Enabled = false;
                detailsToolStripMenuItem.Enabled = false;
            }
            else
            {
                cleanToolStripMenuItem.Enabled = true;
                detailsToolStripMenuItem.Enabled = true;
            }
        }

        private void menuItem2_Click_1(object sender, EventArgs e)
        {
            frmLoginMan aFrmLoginMan = new frmLoginMan();
            aFrmLoginMan.ShowDialog();
            aFrmLoginMan.Dispose();
        }

        private void menuItemExitandUpdate_Click(object sender, EventArgs e)
        {
            //没办法知道是否用户在服务, 有风险
            if (MessageBox.Show("Update will close Server for a period time\nmake sure no user is using service now\n"
                , "warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    Application.Exit();
                    Process.Start("EEPServerUpdate.exe");
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                }
            }
        }

        private void tmMaxTimeOut_Tick(object sender, EventArgs e)
        {
            XmlDocument DBXML = new XmlDocument();

            double maxTimeOut = 0;
            #region
            if (File.Exists(SystemFile.ServerConfigFile))
            {
            Label1:
                try
                {
                    DBXML.Load(SystemFile.ServerConfigFile);
                }
                catch
                {
                    Thread.Sleep(100);
                    goto Label1;
                }

                XmlNode aNode = DBXML.DocumentElement.SelectSingleNode("MaxTimeOut");
                if (aNode != null)
                {
                    string sMaxTimeOut = aNode.Attributes["Value"].InnerText;
                    if (sMaxTimeOut != null)
                    {
                        if (!double.TryParse(sMaxTimeOut, out maxTimeOut))
                        {
                            maxTimeOut = 0;
                        }
                    }
                }
            }

            #endregion

            if (maxTimeOut != 0)
            {
                List<string> listUserId = new List<string>();
                UserInfo[] infos = SrvGL.GetUsersInfos();
                foreach (UserInfo info in infos)
                {
                    ComputerInfo[] infopcs = info.Computers;
                    foreach (ComputerInfo infopc in infopcs)
                    {
                        if (infopc.LastActiveTime.AddHours(maxTimeOut) < DateTime.Now)
                        {
                            SrvGL.LogUser(info.UserID, info.UserName, infopc.ComputerName, -1);
                        }
                    }
                }
            }
        }

        private void menuItemClientUpdate_Click(object sender, EventArgs e)
        {
            formupdateserver.WindowState = FormWindowState.Normal;
            formupdateserver.ShowDialog(this);
        }

        private void menuItemWorkflowConfig_Click(object sender, EventArgs e)
        {
            frmWorkflowConfig workfowConfig = new frmWorkflowConfig();
            workfowConfig.ShowDialog();
        }

        private void menuItemAboutEEPNetServer_Click(object sender, EventArgs e)
        {
            FormVersion form = new FormVersion("About EEP.Net Server");
            form.ShowDialog(this);
        }

        private void menuItemWCF_Click(object sender, EventArgs e)
        {
            ShowDialog("WCFServer", (sender as MenuItem).Tag.ToString());
        }

        private void menuItemIDC_Click(object sender, EventArgs e)
        {
            var path = Application.StartupPath;
            var gacFile = string.Format(@"{0}\GACUtil.exe", path);
            var argument = " -u EFDesign";
            var startInfo = new ProcessStartInfo(gacFile, argument) { UseShellExecute = false, RedirectStandardOutput = true };
            var result = Process.Start(startInfo).StandardOutput.ReadToEnd();

            result += "\r\n------------------------------------------------------------------------------\r\n";

            argument = " -i \"EFDesign.dll\"";
            startInfo = new ProcessStartInfo(gacFile, argument) { UseShellExecute = false, RedirectStandardOutput = true };
            result += Process.Start(startInfo).StandardOutput.ReadToEnd();
            MessageBox.Show(result);
        }
    }
}
