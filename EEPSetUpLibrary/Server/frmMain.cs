using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections;
using Microsoft.Win32;
using System.Xml;
using System.Runtime.Remoting;

namespace EEPSetUpLibrary.Server
{
    public partial class frmMain : Form
    {
#if VS90
        /// <summary>
        /// 注册表键名
        /// </summary>
        const string REGISTRYNAME = "SOFTWARE\\infolight\\eep.net2008";
#else
        /// <summary>
        /// 注册表键名
        /// </summary>
        const string REGISTRYNAME = "SOFTWARE\\infolight\\eep.net";
#endif

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //load config

            Config.Load();
            //disable set workpath, load from register 2007/08/14
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(REGISTRYNAME,false);
            if (rk != null)
            {
                Config.WorkPath = rk.GetValue("Client Path").ToString();
            }
            else
            {
                Config.WorkPath = string.Empty;
            }
            Config.TempWorkPath = Config.WorkPath;
            //end modify

            Config.LoadFiles();
            listViewHistoryUser.Items.Clear();
            System.Xml.XmlNodeList list = Log.Read();
            if (list != null)
            {
                for (int i = 0; i < list.Count && i < 100; i++)
                {
                    ListViewItem li = listViewHistoryUser.Items.Add(list[i].Attributes["User"].Value);
                    li.SubItems.Add(new ListViewItem.ListViewSubItem(li, list[i].Attributes["DateTime"].Value));
                    li.SubItems.Add(new ListViewItem.ListViewSubItem(li, list[i].Attributes["Description"].Value));
                }
            }
            timer.Start();

#if !Remoting
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, Config.ServerPort);
            try
            {
                server.Bind(ip);
                server.Listen(10);
                server.BeginAccept(new AsyncCallback(Connect), server);
            }
            catch(SocketException es)//端口被占用
            {
                if (es.ErrorCode == 10048)
                {
                    MessageBox.Show("Port of EEPNetUpdate Server is being used now\r\nPlease change the port in the options");
                }
            }
#else
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(LoaderObject), "LoaderObject.rem", WellKnownObjectMode.Singleton);
#endif
        }
#if !Remoting
        private ArrayList listthread = new ArrayList();//记录所有运行的线程

        private void Connect(IAsyncResult iar)
        {
            Socket server = iar.AsyncState as Socket;
            try
            {
                Socket client = server.EndAccept(iar);
                Thread td = new Thread(new ParameterizedThreadStart(Receive));//新建进程进行通信
                listthread.Add(td);
                td.Start(client);
            }
            catch { }
            finally
            {
                server.BeginAccept(new AsyncCallback(Connect), server);//准备接收下一个连接
            }
        }

        private UserInfoCollection ConnectedUsers = new UserInfoCollection();

        private void Receive(object param)
        {
            Socket client = param as Socket;
            bool finished = false;
            try
            {
                byte[] btbufundecoded = new byte[0];
                while (!finished)
                {
                    byte[] bufmax = new byte[Config.MAX_LENGTH];
                    int length = client.Receive(bufmax);//接收消息

                    //接收不完整的时候出现问题


                    byte[] btbuf = new byte[length + btbufundecoded.Length];
                    if (btbufundecoded.Length > 0)
                    {
                        Buffer.BlockCopy(btbufundecoded, 0, btbuf, 0, btbufundecoded.Length);//将未完成的字节加入队列的开始
                    }
                    Buffer.BlockCopy(bufmax, 0, btbuf, btbufundecoded.Length, length);//加入接收到的字节
                    MessageBuffer mb = new MessageBuffer(btbuf);
                    if (mb.Completed)
                    {
                        switch (mb.Header)
                        {
                            case MessageType.RequerySolution:
                                {
                                    XmlDocument xmldoc = new XmlDocument();
                                    if (System.IO.File.Exists(Config.SolutionFile))
                                    {
                                        try
                                        {
                                            xmldoc.Load(Config.SolutionFile);
                                        }
                                        catch { }
                                    }
                                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                    xmldoc.Save(ms);
                                    byte[] xml = ms.ToArray();

                                    MessageBuffer mbsolutionlist = new MessageBuffer(MessageType.SendSolutionList, 0, 0,xml.Length);
                                    mbsolutionlist.SetFileBytes(xml);
                                    client.Send(mbsolutionlist.GetBytes());
                                    finished = true;
                                    break;
                                }

                            case MessageType.RequeryFile:
                                {
                                    UserInfo user = new UserInfo((IPEndPoint)client.RemoteEndPoint);
                                    FileInfoCollection fc = new FileInfoCollection();
                                    fc.Load(mb.GetFileBytes());
                                    user.FilesToDownLoad = Config.LocalFiles.GetDownloadList(fc);
                                    RefreshList(RefreshType.Add, user.ID, user);//增加用户,刷新列表

                                    byte[] xml = user.FilesToDownLoad.SaveToBuffer();
                                    MessageBuffer mbfilelist = new MessageBuffer(MessageType.SendFileList, 0, 0, xml.Length);
                                    mbfilelist.SetFileBytes(xml);
                                    client.Send(mbfilelist.GetBytes());//发送更新列表

                                    user.FilesToDownLoad.Send(client);//发送文件内容  
                                    client.Send(MessageBuffer.SendFinished.GetBytes());//发送完毕
                                    break;
                                }
                            case MessageType.RequeryBlock:
                                Config.LocalFiles.Send(client, mb.FileID, mb.FileOffSet, mb.FileLength);//发送文件部分内容
                                break;

                            case MessageType.ReceiveFinished:
                                finished = true;
                                break;
                        }
                    }
                }
            }
            catch (SocketException)//客户端退出或者取消或者超时关闭
            {

            }
            catch//确保服务器稳定
            { 
                //记录错误,有心情再做
            }
            finally
            {
                RefreshList(RefreshType.Remove, ((IPEndPoint)client.RemoteEndPoint).ToString().GetHashCode(), null);//移除用户,刷新列表
                client.Close();
            }
        }

        private delegate void RefreshMethod();

        private void RefreshList(RefreshType rt, int key, UserInfo user)
        {
            lock (this)//进程间互斥,防止在刷新时修改集合
            {
                switch (rt)
                {
                    case RefreshType.Add:
                        {
                            Log.Write(user.IP, "Update start");
                            if (!this.Disposing)
                            {
                                RefreshMethod call = delegate()
                                {
                                    ListViewItem li = listViewHistoryUser.Items.Insert(0, user.IP.ToString());
                                    li.SubItems.Add(new ListViewItem.ListViewSubItem(li, DateTime.Now.ToString()));
                                    li.SubItems.Add(new ListViewItem.ListViewSubItem(li, "Update start"));
                                };
                                listViewHistoryUser.Invoke(call);
                            }
                            ConnectedUsers.Add(key, user);//刷新history user
                            break;
                        }
                    case RefreshType.Remove:
                        {
                            if (ConnectedUsers.ContainsKey(key))
                            {
                                Log.Write(ConnectedUsers[key].IP, "Update finish");//在还没有登入时会出错
                                if (!this.Disposing)
                                {
                                    RefreshMethod call = delegate()
                                    {
                                        ListViewItem li = listViewHistoryUser.Items.Insert(0, ConnectedUsers[key].IP.ToString());
                                        li.SubItems.Add(new ListViewItem.ListViewSubItem(li, DateTime.Now.ToString()));
                                        li.SubItems.Add(new ListViewItem.ListViewSubItem(li, "Update finish"));
                                    };
                                    listViewHistoryUser.Invoke(call);
                                }
                                ConnectedUsers.Remove(key);
                            }
                            break;
                        }
                    case RefreshType.Refresh:
                        break;
                }
                if (!this.Disposing)//正在中止线程
                {
                    RefreshMethod call = delegate()
                    {
                        listViewActiveUser.Items.Clear();
                        IEnumerator ie = ConnectedUsers.GetEnumerator();
                        while (ie.MoveNext())
                        {
                            UserInfo ui = ((DictionaryEntry)ie.Current).Value as UserInfo;
                            ListViewItem li = listViewActiveUser.Items.Add(ui.IP.ToString());
                            li.SubItems.Add(new ListViewItem.ListViewSubItem(li, ui.ConnectTime.ToString()));
                        }
                    };
                    listViewActiveUser.Invoke(call);//终止线程时调用会产生死锁
                }
            }
        }
#endif

        #region MenuItem click event
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmConfig frm = new frmConfig();
            frm.ShowDialog(this);
        }

        private void solutionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSolution frm = new frmSolution();
            frm.ShowDialog(this);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Config.LocalFiles.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showActiveUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showActiveUserToolStripMenuItem.Checked = true;
            showHistoryUserToolStripMenuItem.Checked = false;
            splitContainerUsers.SplitterDistance = splitContainerUsers.Width - 1;
            this.toolStripStatusLabelShow.Text = "Active User";
        }

        private void showHistoryUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showHistoryUserToolStripMenuItem.Checked = true;
            showActiveUserToolStripMenuItem.Checked = false;
            splitContainerUsers.SplitterDistance = 0;
            this.toolStripStatusLabelShow.Text = "History User";
        }
        #endregion

        //bool frmClose = false;

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (e.CloseReason != CloseReason.UserClosing)//父窗口关闭或者应用退出就关闭
            {
                //frmClose = true;
                //关闭所有线程
#if !Remoting
                foreach (Thread td in listthread)
                {
                    if (td.ThreadState != ThreadState.Stopped)
                    {
                        td.Abort();
                    }
                }
#endif
            }
            else
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
#if !Remoting
            toolStripStatusLabelLink.Text = string.Format("Current Links: {0}", ConnectedUsers.Count);

            ArrayList liststopedtd = new ArrayList();
            foreach (Thread td in listthread)
            {
                if (td.ThreadState == ThreadState.Stopped)
                {
                    liststopedtd.Add(td);
                }
            }
            foreach (Thread td in liststopedtd)
            {
                listthread.Remove(td);
            }
#endif
        }

        internal enum RefreshType
        { 
            Add,
            Remove,
            Refresh
        }

        private void clearHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Confirm to clear the history?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (System.IO.File.Exists(Log.LogFileName))
                {
                    System.IO.File.Delete(Log.LogFileName);
                }
                listViewHistoryUser.Items.Clear();
            }
        }
    }
}