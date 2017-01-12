using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Threading;
using System.Net;
using Srvtools;
using Settings = EEPServerMonitor.Properties.Settings;
using System.Collections;
using System.Net.Mail;

namespace EEPServerMonitor
{
    public partial class FormMain : Form
    {
        const string LOG_FILE = "EEPServerMonitor.log";

        public FormMain()
        {
            InitializeComponent();
            Application.ApplicationExit += delegate(object sender, EventArgs e)
            {
                foreach (Server srv in Config.Servers)
                {
                    if (srv.Thread != null && srv.Thread.ThreadState != ThreadState.Stopped)
                    {
                        srv.Thread.Abort();
                    }
                }
            };
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Config.Load();
            InitListView();
            timer.Interval = Config.RefreshInterval * 1000;
            timer.Start();
            timer_Tick(timer, new EventArgs());
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOptions form = new FormOptions();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                timer.Stop();
                InitListView();
                timer.Interval = Config.RefreshInterval * 1000;
                timer.Start();
                timer_Tick(timer, new EventArgs());
            }
        }

        private void InitListView()
        {
            this.listView.Items.Clear();
            foreach (Server srv in Config.Servers)
            {
                ListViewItem item = new ListViewItem(srv.Uri, (int)srv.Status);
                item.SubItems.Add(string.Empty);
                item.SubItems.Add(string.Empty);
                item.SubItems.Add(string.Empty);
                item.SubItems.Add(string.Empty);
                item.SubItems.Add("Connecting...");
                item.Tag = srv;
                listView.Items.Add(item);
            }
        }

        private delegate void RefreshListViewViewMethod(Server server);

        private void RefreshListView(Server server)
        {
            for (int i = 0; i < listView.Items.Count; i++)
            {
                if (listView.Items[i].Tag.Equals(server))
                {
                    listView.Items[i].ImageIndex = (int)server.Status;

                    listView.Items[i].SubItems[1].Text = server.Status == Server.StatusType.Error 
                        ? "Unknown" : string.Format("{0}({1})/{2}", server.LoginedUser, server.AllocatedUser, server.MaxUser);
                    listView.Items[i].SubItems[2].Text = server.Status == Server.StatusType.Error
                       ? "Unknown" : string.Format("{0:F1}%", server.Cpu);
                    listView.Items[i].SubItems[3].Text = server.Status == Server.StatusType.Error
                        ? "Unknown" : string.Format("{0:F1}MB", (float)server.Memory / 1048576);
                    listView.Items[i].SubItems[4].Text = server.Status == Server.StatusType.Error
                        ? "Unknown" : string.Format("{0:F1}ms", server.ResponseInterval);
                    listView.Items[i].SubItems[5].Text = server.Description;
                    break;
                }
            }
        
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Server[] servers = Config.Servers.ToArray();
            foreach (Server srv in servers)
            {
                if (srv.Thread == null || srv.Thread.ThreadState == ThreadState.Stopped)
                {
                    Thread td = new Thread(new ParameterizedThreadStart(RefreshServer));
                    srv.Thread = td;
                    td.Start(srv);
                }
            }
        }

        private void RefreshServer(object objserver)
        {
            Server connectserver = (Server)objserver;
            try
            {
                DateTime timestart = DateTime.Now;
                LoginService service = (LoginService)Activator.GetObject(typeof(LoginService), string.Format("http://{0}/Srvtools.rem", connectserver.Uri));
                int luser = 0;
                int auser = 0;
                int muser = 0;
                float cpu = 0.0f;
                long memory = 0;
                service.GetLoginInfo(ref luser, ref muser, ref auser);

                try
                {
                    cpu = service.GetCpuInfo();
                    memory = service.GetMemoryInfo();
                }
                catch
                {
                    connectserver.SetRequireUpdate();
                }
                DateTime timeend = DateTime.Now;
                Server.StatusType type = connectserver.Status;
                connectserver.SetInfo(luser, auser, muser, cpu, memory, ((TimeSpan)(timeend - timestart)).TotalMilliseconds);
                if (connectserver.Status == Server.StatusType.Warning && Config.WarningNotify)
                {
                    if (CheckNotifyCycle() || type != Server.StatusType.Warning)
                    {
                        lastNotifyTime = DateTime.Now;
                        SendMail(connectserver);
                        Log(connectserver);
                    }
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception e)
            {
                Server.StatusType type = connectserver.Status;
                connectserver.SetError(e.Message);
                if (Config.ErrorNotify)
                {
                    if (CheckNotifyCycle() || type != Server.StatusType.Error)
                    {
                        lastNotifyTime = DateTime.Now;
                        SendMail(connectserver);
                        Log(connectserver);
                    }
                }
            }

            if (!this.Disposing)
            {
                if (listView.InvokeRequired)
                {
                    RefreshListViewViewMethod call = delegate(Server server)
                    {
                        RefreshListView(server);
                    };
                    listView.Invoke(call, new object[] { connectserver });
                }
                else
                {
                    RefreshListView(connectserver);
                }
            }
        }

        private DateTime lastNotifyTime = DateTime.Now;

        private bool CheckNotifyCycle()
        {
            TimeSpan span = DateTime.Now - lastNotifyTime;
            return (int)span.TotalSeconds > Config.NotifyCycle;
        }

        private void SendMail(Server server)
        {
            if (!string.IsNullOrEmpty(Config.EmailServer) && !string.IsNullOrEmpty(Config.EmailAddress) && !string.IsNullOrEmpty(Config.EmailUser))
            {
                try
                {
                    SendMailMethod call = delegate(Server srv)
                    {
                        MailMessage mailMessage = new MailMessage(Config.EmailUser, Config.EmailAddress);
                        mailMessage.Subject = string.Format("EEPServer {0} at {1:yyyy-MM-dd HH:mm:ss}", srv.Status, DateTime.Now);
                        mailMessage.Body = srv.ToString();
                        SmtpClient smtpClient = new SmtpClient(Config.EmailServer);
                        smtpClient.EnableSsl = Config.EnableSSL;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(Config.EmailUser, Config.EmailPassword);
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
                        smtpClient.SendAsync(mailMessage, null);
                    };
                    this.Invoke(call, new object[] { server });
                }
                catch { }
            }
        }
        public delegate void SendMailMethod(Server server);

        private void Log(Server server)
        {
            lock (this)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(LOG_FILE, true, Encoding.UTF8))
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append('-', 30);
                        builder.AppendLine();
                        builder.Append(server.ToString());
                        builder.AppendLine();
                        writer.WriteLine(builder.ToString());
                    }
                }
                catch { }
            }
        }

        void smtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            
        }

        private void copyInfomationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyInfomation();
        }

        private void CopyInfomation()
        {
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem item = listView.SelectedItems[0];
                Server server = item.Tag as Server;
                if (server != null)
                {
                    Clipboard.SetText(server.ToString());
                    MessageBox.Show(this, "Infomation has been copied to clipboard", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }

    /// <summary>
    /// 服务器类
    /// </summary>
    public class Server
    {
        /// <summary>
        /// 初始化服务器
        /// </summary>
        /// <param name="serverUri">服务器地址</param>
        public Server(string serverUri)
        {
            uri = serverUri;
        }

        private string uri;
        /// <summary>
        /// 获取服务器地址
        /// </summary>
        public string Uri
        {
            get { return uri; }
        }

        private StatusType status = StatusType.Connecting;
        /// <summary>
        /// 获取服务器状态
        /// </summary>
        public StatusType Status
        {
            get { return status; }
        }
	
        private int loginedUser;
        /// <summary>
        /// 获取服务器已经登录的用户数
        /// </summary>
        public int LoginedUser
        {
            get { return loginedUser; }
        }

        private int allocatedUser;
        /// <summary>
        /// 获取服务器已经分配的用户数
        /// </summary>
        public int AllocatedUser
        {
            get { return allocatedUser; }
        }

        private int maxUser;
        /// <summary>
        /// 获取服务器的最大用户数
        /// </summary>
        public int MaxUser
        {
            get { return maxUser; }
        }

        private float cpu;
        /// <summary>
        /// 获取服务器的CPU占用率
        /// </summary>
        public float Cpu
        {
            get { return cpu; }
        }

        private long memory;
        /// <summary>
        /// 获取Server的占用内存数
        /// </summary>
        public long Memory
        {
            get { return memory; }
        }

        private string description;
        /// <summary>
        /// 获取服务器的其它说明
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        private double responseInterval = 0;
        /// <summary>
        /// 获取服务器响应时间
        /// </summary>
        public double ResponseInterval
        {
            get { return responseInterval; }
        }
	

        private Thread thread;
        /// <summary>
        /// 获取服务器信息刷新线程
        /// </summary>
        public Thread Thread
        {
            get { return thread; }
            set { thread = value; }
        }

        /// <summary>
        /// 设置服务器信息
        /// </summary>
        /// <param name="ServerLUser">已经登录的用户数</param>
        /// <param name="serverAUser">已经分配的用户数</param>
        /// <param name="serverMUser">最大用户数</param>
        /// <param name="serverCpu">CPU占用率</param>
        /// <param name="serverMemory">内存占用数</param>
        /// <param name="interval">响应时间</param>
        public void SetInfo(int ServerLUser, int serverAUser, int serverMUser, float serverCpu, long serverMemory, double interval)
        {
            loginedUser = ServerLUser;
            allocatedUser = serverAUser;
            maxUser = serverMUser;
            cpu = serverCpu;
            memory = serverMemory;
            responseInterval = interval;
            StringBuilder builder = new StringBuilder();
            if (Status != StatusType.RequireUpdate)
            {
                status = StatusType.Normal;
                if (loginedUser + allocatedUser >= maxUser)
                {
                    status = StatusType.Warning;
                    builder.AppendLine("User has achieved max count");
                }
                if (responseInterval > Config.IntervalWarning)
                {
                    status = StatusType.Warning;
                    builder.AppendLine("Response interval has achieved warning value");
                }
                if (cpu > Config.CpuWarning)
                {
                    status = StatusType.Warning;
                    builder.AppendLine("Cpu has achieved warning value");
                }
                if ((double)memory > Config.MemoryWarning * 1048576.0f)
                {
                    status = StatusType.Warning;
                    builder.AppendLine("Memory has achieved warning value");
                }
                description = builder.ToString();
            }
        }

        /// <summary>
        /// 服务机宕机
        /// <param name="reason">原因</param>
        /// </summary>
        public void SetError(string reason)
        {
            status = StatusType.Error;
            description = reason;
        }

        /// <summary>
        /// 服务器需要更新
        /// </summary>
        public void SetRequireUpdate()
        {
            status = StatusType.RequireUpdate;
            description = "Server need to update";
        }

        /// <summary>
        /// 获取服务器的状态信息
        /// </summary>
        /// <returns>状态信息</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(string.Format("EEPServer {0} at {1:yyyy-MM-dd HH:mm:ss}", Status, DateTime.Now));
            builder.AppendLine(string.Format("Server:{0}", Uri));
            builder.Append(string.Format("Description:{0}", Description));
            if (Status == Server.StatusType.Warning)
            {
                builder.AppendLine(string.Format("Current user:{0}({1})/{2}", LoginedUser, AllocatedUser, MaxUser));
                builder.AppendLine(string.Format("Cpu:{0:F1}%", Cpu));
                builder.AppendLine(string.Format("Memory:{0:F1}MB", (float)Memory / 1048576));
                builder.AppendLine(string.Format("Response Interval:{0:F1}ms", ResponseInterval));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 状态类型
        /// </summary>
        public enum StatusType
        { 
            /// <summary>
            /// 正在连接
            /// </summary>
            Connecting = 0,
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 1,
            /// <summary>
            /// 警告
            /// </summary>
            Warning = 2,
            /// <summary>
            /// 错误
            /// </summary>
            Error = 3,
            /// <summary>
            /// 需要升级版本
            /// </summary>
            RequireUpdate =4
        }
    }

    /// <summary>
    /// 配置类
    /// </summary>
    public class Config
    {
        private static int refreshInterval = 10;
        /// <summary>
        /// 获取或者设置刷新间隔时间
        /// </summary>
        public static int RefreshInterval
        {
            get { return refreshInterval; }
            set { refreshInterval = value; }
        }

        private static float cpuWarning = 90.0f;
        /// <summary>
        /// 获取或者设置Cpu警告阈值
        /// </summary>
        public static float CpuWarning
        {
            get { return cpuWarning; }
            set { cpuWarning = value; }
        }

        private static float memoryWarning = 256.0f;
        /// <summary>
        /// 获取或者设置Memory警告阈值
        /// </summary>
        public static float MemoryWarning
        {
            get { return memoryWarning; }
            set { memoryWarning = value; }
        }

        private static double intervalWarning = 1000;
        /// <summary>
        /// 获取或者设置响应时间警告阈值
        /// </summary>
        public static double IntervalWarning
        {
            get { return intervalWarning; }
            set { intervalWarning = value; }
        }

        private static bool warningNotify;
        /// <summary>
        /// 获取或者设置是否通知警告
        /// </summary>
        public static bool WarningNotify
        {
            get { return warningNotify; }
            set { warningNotify = value; }
        }

        private static bool errorNotify;
        /// <summary>
        /// 获取或者设置是否通知错误
        /// </summary>
        public static bool ErrorNotify
        {
            get { return errorNotify; }
            set { errorNotify = value; }
        }
	
        private static string emailServer;
        /// <summary>
        /// 获取或者设置Email服务器
        /// </summary>
        public static string EmailServer
        {
            get { return emailServer; }
            set { emailServer = value; }
        }

        private static string emailUser;
        /// <summary>
        /// 获取或者设置Email用户名
        /// </summary>
        public static string EmailUser
        {
            get { return emailUser; }
            set { emailUser = value; }
        }

        private static string emailPassword;
        /// <summary>
        /// 获取或者设置Email密码
        /// </summary>
        public static string EmailPassword
        {
            get { return emailPassword; }
            set { emailPassword = value; }
        }

        private static string emailAddress;
        /// <summary>
        /// 获取或者设置Email发送的地址
        /// </summary>
        public static string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        private static bool enableSSL;
        /// <summary>
        /// 获取或者设置Email是否启用SSL
        /// </summary>
        public static bool EnableSSL
        {
            get { return enableSSL; }
            set { enableSSL = value; }
        }

        private static int notifyCycle = 300;
        /// <summary>
        /// 获取或者设置发送Email的周期
        /// </summary>
        public static int NotifyCycle
        {
            get { return notifyCycle; }
            set { notifyCycle = value; }
        }

        private static List<Server> servers = new List<Server>();
        /// <summary>
        /// 获取服务器列表
        /// </summary>
        public static List<Server> Servers
        {
            get { return servers; }
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        public static void Load()
        {
            RefreshInterval = Settings.Default.RefreshInterval;
            CpuWarning = Settings.Default.CpuWarning;
            MemoryWarning = Settings.Default.MemoryWarning;
            IntervalWarning = Settings.Default.IntervalWarning;
            EmailServer = Settings.Default.EmailServer;
            EmailUser = Settings.Default.EmailUser;
            EmailPassword = Settings.Default.EmailPassword;
            EmailAddress = Settings.Default.EmailAddress;
            WarningNotify = Settings.Default.WarningNotify;
            ErrorNotify = Settings.Default.ErrorNotify;
            EnableSSL = Settings.Default.EnableSSL;
            NotifyCycle = Settings.Default.NotifyCycle;
            ArrayList listservers = Settings.Default.Servers;
            if (listservers != null)
            {
                foreach (string str in listservers)
                {
                    Config.Servers.Add(new Server(str));
                }
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public static void Save()
        {
            Settings.Default.RefreshInterval = RefreshInterval;
            Settings.Default.CpuWarning = CpuWarning;
            Settings.Default.MemoryWarning = MemoryWarning;
            Settings.Default.IntervalWarning = IntervalWarning;
            Settings.Default.EmailServer = EmailServer;
            Settings.Default.EmailUser = EmailUser;
            Settings.Default.EmailPassword = EmailPassword;
            Settings.Default.EmailAddress = EmailAddress;
            Settings.Default.WarningNotify = WarningNotify;
            Settings.Default.ErrorNotify = ErrorNotify;
            Settings.Default.EnableSSL = EnableSSL;
            Settings.Default.NotifyCycle = NotifyCycle;
            ArrayList listservers = new ArrayList();
            foreach (Server server in Config.Servers)
            {
                listservers.Add(server.Uri);
            }
            Settings.Default.Servers = listservers;
            Settings.Default.Save();
        }
    }
}