using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections;
using EEPSetUpLibrary.Proxy;
using ProxySetting = EEPSetUpLibrary.Properties.Settings;

namespace EEPSetUpLibrary.Client
{
    public partial class frmSetUp : Form
    {
        public frmSetUp()
        {
            InitializeComponent();
#if Remoting
            Application.ApplicationExit += delegate(object sender, EventArgs e)
            {
                if (td != null && td.ThreadState != ThreadState.Stopped)
                {
                    td.Abort();
                }
            };
#endif
        }

        private void frmSetUp_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("temp"))                  
            {
                Directory.CreateDirectory("temp");
            }
            Config.Load();
#if Remoting
            linkLabelProxy.Visible = true;
            ConfigProxy();
#endif
            buttonRefresh_Click(buttonRefresh, new EventArgs());
            if (Config.WorkPath.Length == 0)
            {
                Config.WorkPath = Application.StartupPath + @"\EEPNetClient";
            }
            Config.TempWorkPath = Config.WorkPath;
            labelFolder.Text = Config.WorkPath;
            textBoxFolder.Text = Config.WorkPath;
        }

        private Solution _CurrentSolution;
        /// <summary>
        /// 获取或者设置当前正在更新的方案
        /// </summary>
        public Solution CurrentSolution
        {
            get { return _CurrentSolution; }
            set { _CurrentSolution = value; }
        }

        private IPEndPoint GetEndPoint(string ipaddress)
        {
            IPAddress ip = IPAddress.None;
            int port = Config.ServerPort;

            string[] strip = ipaddress.Split(':');
            try
            {
                ip = IPAddress.Parse(strip[0]);
            }
            catch (FormatException fe)//IP不正确
            {
                try
                {
                    ip = Dns.GetHostAddresses(strip[0])[0];
                }
                catch
                {
                    MessageBox.Show(this, fe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            try
            {
                if (strip.Length == 2)
                {
                    port = Convert.ToInt32(strip[1]);
                }
                if (strip.Length > 2 || port > 65535 || port < 100)
                {
                    MessageBox.Show(this, "Port is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (FormatException fe)//IP不正确
            {
                MessageBox.Show(this, fe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return new IPEndPoint(ip, port);
        }

        private void SetState(State state)
        {
            switch (state)
            {
                case State.Refreshing:
                    SetControlText(buttonRefresh, labelCancel.Text);
                    SetButtonEnabled(buttonRefresh, true);
                    SetButtonEnabled(buttonStart, false);
                    SetButtonEnabled(buttonUninstall, true);

                    break;
                case State.Downloading:
                    SetControlText(buttonStart, labelCancel.Text);
                    SetButtonEnabled(buttonRefresh, false);
                    SetButtonEnabled(buttonStart, true);
                    SetButtonEnabled(buttonUninstall, false);
                    break;
                case State.Idle:
                    SetControlText(buttonRefresh, labelRefresh.Text);
                    SetControlText(buttonStart, labelStart.Text);
                    SetButtonEnabled(buttonRefresh, true);
                    SetButtonEnabled(buttonStart, true);
                    SetButtonEnabled(buttonUninstall, true);
                    break;
            }
            SetProgressBarValue(progressBarDetail, 0);
            SetProgressBarValue(progressBarTotal, 0);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            if (buttonRefresh.Text == labelRefresh.Text)
            {
                if (Config.ServerIP.Length == 0)
                {

                }
                IPEndPoint iep = GetEndPoint(string.Format("{0}:{1}", Config.ServerIP, Config.ServerPort));
                if (iep != null)
                {
                    SetState(State.Refreshing);
                    SetControlText(labelInfo, labelConnecting.Text);
#if !Remoting
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    time = new TimeOut(Config.TimeOutInterval, new TimeOut.TimeOutCallBack(TimeOut), client);
                    time.Start();//开始计时
                    client.BeginConnect(iep, new AsyncCallback(Refresh), iep);
#else
                    td = new Thread(new ParameterizedThreadStart(Refresh));
                    td.Start(iep);
#endif
                }
            }
            else
            {
                SetState(State.Idle);
#if !Remoting
                if (client != null)
                {
                    client.Close();
                }
#else
                if (td != null && td.ThreadState != ThreadState.Stopped)
                {
                    td.Abort();
                }
#endif
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == labelStart.Text)//开始
            {
                if (treeViewSolution.SelectedNode == null || treeViewSolution.SelectedNode.Level == 0)
                {
                    MessageBox.Show(this, "Select a solution first", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (Config.WorkPath.Length == 0)
                {
                    MessageBox.Show(this, "Path can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int port = Config.ServerPort;
                IPAddress ip = IPAddress.None;

                Solution sol = (Solution)treeViewSolution.SelectedNode.Tag;
                IPEndPoint iep = GetEndPoint(string.Format("{0}:{1}", sol.IP, sol.Port));
                if (iep != null)
                {
                    SetState(State.Downloading);
                    CurrentSolution = sol;
                    SetControlText(labelInfo, labelConnecting.Text);
#if !Remoting
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    time = new TimeOut(Config.TimeOutInterval, new TimeOut.TimeOutCallBack(TimeOut), client);
                    time.Start();//开始计时
                    client.BeginConnect(iep, new AsyncCallback(Download), iep);
#else
                    td = new Thread(new ParameterizedThreadStart(DownLoad));
                    td.Start(iep);
#endif
                }
            }
            else                      //取消
            {
                SetState(State.Idle);
#if !Remoting
                if (client != null)
                {
                    client.Close();
                }
#else
                if (td != null && td.ThreadState != ThreadState.Stopped)
                {
                    td.Abort();
                }
#endif
            }
        }

        private void buttonUninstall_Click(object sender, EventArgs e)
        {
            if (Config.WorkPath.Length > 0 && MessageBox.Show(this, string.Format("Confirm to Delete files in {0} ?", Config.WorkPath)
                , "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    Directory.Delete(Config.WorkPath, true);
                    MessageBox.Show(this, "Uninstall finish", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ed)
                {
                    MessageBox.Show(this, string.Format("Uninstall encounter errors:\r\n{0}", ed.Message)
                        , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
#if !Remoting
        TimeOut time = null;
        Socket client = null;

        private void Download(IAsyncResult iar)
        {
            try
            {
                client.EndConnect(iar);
            }
            catch (SocketException)
            {
        #region load Server
                IPEndPoint remoteendpoint = (IPEndPoint)iar.AsyncState;
                time.Stop();//停止计时
                try
                {
                    if (LoadServer(remoteendpoint))
                    {
                        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        time.Start();//开始计时
                        client.BeginConnect(remoteendpoint, new AsyncCallback(Download), client);
                    }
                    else
                    {
                        SetControlText(labelInfo, labelCanNotConnect.Text);//
                        SetState(State.Idle);
                    }
                    return;
                }
                catch (ObjectDisposedException)
                {
                    SetControlText(labelInfo, labelUpdateCancel.Text);
                    SetState(State.Idle);
                    time.Stop();
                    return;
                }
                #endregion
            }
            catch (ObjectDisposedException)
            {
                SetControlText(labelInfo, labelUpdateCancel.Text);
                SetState(State.Idle);
                time.Stop();
                return;
            }
            time.Stop();//停止计时

            FileInfoCollection fc = new FileInfoCollection();
            if (true)//是否只更新有修改过的文件
            {
                try
                {
                    fc.Load(Config.WorkPath);
                    if (File.Exists("Resume.xml"))                      //存在续传记录
                    {
                        if (MessageBox.Show(this, labelResume.Text, "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            XmlDocument xmlresume = new XmlDocument();
                            try
                            {
                                xmlresume.Load("Resume.xml");
                                XmlNode noderesume = xmlresume.SelectSingleNode("Resume/FileResume");
                                int id = Convert.ToInt32(noderesume.Attributes["ID"].Value);
                                DateTime date = Convert.ToDateTime(noderesume.Attributes["Date"].Value);
                                int length = Convert.ToInt32(noderesume.Attributes["Length"].Value);
                                int offset = Convert.ToInt32(noderesume.Attributes["Offset"].Value);
                                fc.SetFiletoResumeInfo(id, date, length, offset);
                            }
                            catch
                            { }
                        }
                        else//清空临时文件夹
                        {
                            Directory.Delete("temp", true);
                            Directory.CreateDirectory("temp");
                        }
                    }
                }
                catch
                {
                    fc.Clear();
                }
                File.Delete("Resume.xml");
            }

            time.Start();//开启计时
            byte[] xml = fc.SaveToBuffer();

            MessageBuffer buf = new MessageBuffer(MessageType.RequeryFile, 0, 0, xml.Length);
            FileStream fs = null;
            buf.SetFileBytes(xml);

            int fileid = 0; //记录当前下载的文件ID
            int fileoffset = 0;//记录当前下载的文件偏移量
            try
            {
                client.Send(buf.GetBytes());//发送已经存在文件列表
                SetControlText(labelInfo, labelDownloadingList.Text);

                int filelength = 0;//记录所有文件的大小
                int filelengthfinished = 0; //记录已经完成的文件的大小

                byte[] btbufundecoded = new byte[0];//存放上次未处理的娄组

                while (true)
                {
                    byte[] bufmax = new byte[Config.MAX_LENGTH];//存放接收到的数组
                    int length = client.Receive(bufmax);//接收数据
                    time.Restart();//重启计时
                    //重写算法
                    byte[] btbuf = new byte[length + btbufundecoded.Length];//存放接收到及未处理的数组
                    if (btbufundecoded.Length > 0)
                    {
                        Buffer.BlockCopy(btbufundecoded, 0, btbuf, 0, btbufundecoded.Length);//将未完成的字节加入队列的开始
                    }
                    Buffer.BlockCopy(bufmax, 0, btbuf, btbufundecoded.Length, length);//加入接收到的字节
                    int bufindex = 0;
                    bool iscompleted = true;
                    while (bufindex < btbuf.Length) //全部处理完也要跳出循环
                    {
                        btbufundecoded = new byte[0];//清空
                        byte[] btmb = new byte[btbuf.Length - bufindex];
                        Buffer.BlockCopy(btbuf, bufindex, btmb, 0, btmb.Length);//将已经处理完的字节移出
                        MessageBuffer mb = new MessageBuffer(btmb);

                        iscompleted = mb.Completed;//判断这个Message是否完整
                        if (!mb.Completed)//是否完整
                        {
                            btbufundecoded = btmb;
                            break;//跳出循环,未处理的在下次接收后一同处理
                        }
                        else
                        {
                            bufindex += mb.FileLength + 13;//修改已经处理的位置
                            switch (mb.Header)
                            {
                                case MessageType.SendFileList:
                                    Config.LocalFiles.Load(mb.GetFileBytes());
                                    filelength = Config.LocalFiles.Length;
                                    break;
                                case MessageType.SendFile:
                                    {
                                        if (fileid != mb.FileID)    //新文件
                                        {
                                            fileid = mb.FileID;
                                            fileoffset = mb.FileOffSet;

                                            if (fs != null)
                                            {
                                                fs.Close();//写回到文件
                                            }
                                            fs = new FileStream(string.Format("temp\\{0}.tmp", fileid), FileMode.OpenOrCreate);
                                            SetControlText(labelInfo, labelDownloading.Text + Config.LocalFiles[fileid].ToString());
                                        }

                                        fs.Position = mb.FileOffSet;
                                        fs.Write(mb.GetFileBytes(), 0, mb.FileLength);//写入到流
                                        fileoffset += mb.FileLength;
                                        filelengthfinished += mb.FileLength;

                                        SetProgressBarValue(progressBarDetail, (int)((double)fileoffset / (double)Config.LocalFiles[fileid].Length * 100));
                                        SetProgressBarValue(progressBarTotal, (int)((double)filelengthfinished / (double)filelength * 100));
                                        //刷新进度条

                                        if (fileoffset == Config.LocalFiles[fileid].Length)//完成这个文件
                                        {
                                            fs.Close();//从流写回到文件
                                            fs = null;
                                            string filename = Config.WorkPath + Config.LocalFiles[fileid].ToString();
                                            if (!Directory.Exists(Path.GetDirectoryName(filename)))//创建目的文件夹
                                            {
                                                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                                            }
                                      
                                            if (File.Exists(filename))//如果文件存在则删除
                                            {
                                                File.Delete(filename);
                                            }
                                            File.Move(string.Format("temp\\{0}.tmp", mb.FileID), filename);//移动文件到目的文件夹
                                            File.SetLastWriteTime(filename, Config.LocalFiles[fileid].Date);//设置最后修改时间
                                       

                                            Config.LocalFiles.Remove(fileid);
                                        }
                                        break;
                                    }
                                case MessageType.SendFinished:
                                    if (true)
                                    {
                                        client.Send(MessageBuffer.ReceiveFinished.GetBytes());
                                    }
                                    else// 用来申请某个文件中的一部分,可能用不到
                                    {
                                        //MessageBuffer mbreqblock = new MessageBuffer(MessageType.RequeryBlock, "id", "offset", "length");
                                        //client.Send(mbreqblock.GetBytes());
                                    }
                                    if (filelength == 0)
                                    {
                                        SetControlText(labelInfo, labelNoUpdateFound.Text);
                                    }
                                    else
                                    {
                                        SetControlText(labelInfo, labelUpdateFinished.Text);
                                    }
                                    SetProgressBarValue(progressBarDetail, 100);
                                    SetProgressBarValue(progressBarTotal, 100);
                                    client.Shutdown(SocketShutdown.Both);
                                    client.Close(100);
                                    if (true)
                                    {
                                        if (File.Exists(Config.WorkPath + "\\" + Config.PreInstall) && !Config.CheckPreInstall())
                                        {
                                            try
                                            {
                                                System.Diagnostics.Process pro = System.Diagnostics.Process.Start(Config.WorkPath + "\\" + Config.PreInstall);
                                                pro.WaitForInputIdle();
                                            }
                                            catch { }
                                        }
                                        try
                                        {
                                            if (CurrentSolution != null)
                                            {
                                                XmlDocument clientxml = CreateClientXml(CurrentSolution);
                                                clientxml.Save(Config.WorkPath + @"\EEPNetClient.xml");
                                            }
                                        }
                                        catch(Exception e)
                                        {
                                            SetMessageBox(e.Message);
                                        }
                                        try
                                        {
                                            String S = CurrentSolution.Language.ToString() + " " + CurrentSolution.IP +
                                                " " + CurrentSolution.ApPort.ToString();
                                            System.Diagnostics.Process pro
                                                = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(Config.WorkPath + "\\" + Config.LaunchPath, S));
                                            pro.WaitForInputIdle();
                                            SetState(State.Idle);
                                            //Application.Exit();
                                        }
                                        catch(Exception e)
                                        {
                                            SetMessageBox(e.Message);
                                        }
                                    }
                                    return;
                            }
                        }
                    }
                }
            }
            catch (Exception e)//有些文件在copy时会有权限问题,还查不出
            {
                if (client.Connected)
                {
                    client.Send(MessageBuffer.ReceiveFinished.GetBytes());
                }
                if (buttonStart.Text == labelStart.Text)
                {
                    SetControlText(labelInfo, labelUpdateCancel.Text);
                    if (fileid != 0 && fileoffset != Config.LocalFiles[fileid].Length)//当前文件未完成
                    {
                        //存入文件
                        XmlDocument xmlresume = new XmlDocument();
                        XmlNode nodedoc = xmlresume.CreateElement("Resume");
                        xmlresume.AppendChild(nodedoc);
                        XmlNode nodefileresume = xmlresume.CreateElement("FileResume");
                        XmlAttribute att = xmlresume.CreateAttribute("ID");
                        att.Value = fileid.ToString();
                        nodefileresume.Attributes.Append(att);
                        att = xmlresume.CreateAttribute("Offset");
                        att.Value = fileoffset.ToString();
                        nodefileresume.Attributes.Append(att);
                        att = xmlresume.CreateAttribute("Date");
                        att.Value = Config.LocalFiles[fileid].Date.ToString();
                        nodefileresume.Attributes.Append(att);
                        att = xmlresume.CreateAttribute("Length");
                        att.Value = Config.LocalFiles[fileid].Length.ToString();
                        nodefileresume.Attributes.Append(att);

                        nodedoc.AppendChild(nodefileresume);
                        xmlresume.Save("Resume.xml");//记录续传文件的信息
                    }
                }
                else
                {
                    SetControlText(labelInfo, string.Empty);
                    SetMessageBox(e.Message);
                }
                SetState(State.Idle);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                //if (!this.Disposing)
                //{
                //    SetState(State.Idle);
                //}
                client.Close();
                time.Stop();
            }
        }

        private void Refresh(IAsyncResult iar)
        {
            try
            {
                client.EndConnect(iar);
            }
            catch (SocketException)
            {
        #region load Server
                IPEndPoint remoteendpoint = (IPEndPoint)iar.AsyncState;
                time.Stop();//停止计时
                try
                {
                    if (LoadServer(remoteendpoint))
                    {
                        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        time.Start();//开始计时
                        client.BeginConnect(remoteendpoint, new AsyncCallback(Refresh), client);
                    }
                    else
                    {
                        SetControlText(labelInfo, labelCanNotConnect.Text);//
                        SetState(State.Idle);
                    }
                    return;
                }
                catch (ObjectDisposedException)
                {
                    SetControlText(labelInfo, labelUpdateCancel.Text);
                    SetState(State.Idle);
                    time.Stop();
                    return;
                }
        #endregion
            }
            catch (ObjectDisposedException)
            {
                SetControlText(labelInfo, labelUpdateCancel.Text);
                SetState(State.Idle);
                time.Stop();
                return;
            }

            time.Restart();//开启计时
            try
            {
                client.Send(MessageBuffer.RequerySolution.GetBytes());//发送已经存在文件列表
                byte[] btbufundecoded = new byte[0];//存放上次未处理的娄组

                while (true)
                {
                    byte[] bufmax = new byte[Config.MAX_LENGTH];//存放接收到的数组
                    int length = client.Receive(bufmax);//接收数据
                    time.Restart();//重启计时
                    //重写算法
                    byte[] btbuf = new byte[length + btbufundecoded.Length];//存放接收到及未处理的数组
                    if (btbufundecoded.Length > 0)
                    {
                        Buffer.BlockCopy(btbufundecoded, 0, btbuf, 0, btbufundecoded.Length);//将未完成的字节加入队列的开始
                    }
                    Buffer.BlockCopy(bufmax, 0, btbuf, btbufundecoded.Length, length);//加入接收到的字节

                    btbufundecoded = new byte[0];//清空
                    MessageBuffer mb = new MessageBuffer(btbuf);

                    if (!mb.Completed)//是否完整
                    {
                        btbufundecoded = btbuf;
                    }
                    else
                    {
                        switch (mb.Header)
                        {
                            case MessageType.SendSolutionList:
                                if (mb.FileLength > 0)
                                {
                                    File.WriteAllBytes(Config.SolutionFile, mb.GetFileBytes());
                                    RefreshTreeView();
                                    SetControlText(labelInfo, labelUpdateFinished.Text);
                                    return;
                                }
                                break;
                            default:
                                return;
                                break;
                        }
                    }
                }
            }
            catch (ObjectDisposedException)//按取消连接
            {
                SetControlText(labelInfo, labelUpdateCancel.Text);
            }
            catch (Exception e)//有些文件在copy时会有权限问题,还查不出
            {
                SetControlText(labelInfo, e.Message);
            }
            finally
            {
                SetState(State.Idle);
                client.Close();
                time.Stop();
            }
        }

        private void TimeOut(object param)//超出一定时间没收到数据
        {
            time.Stop();
            Socket client = param as Socket;
            client.Close();//断开连接
            SetMessageBox("Connection time out");
        }
#else
        Thread td = null;
        private void DownLoad(object uri)
        {
            IPEndPoint iep = (IPEndPoint)uri;
            LoaderObject loader = (LoaderObject)Activator.GetObject(typeof(LoaderObject), string.Format("http://{0}/LoaderObject.rem", uri));
            int fileid = 0; //记录当前下载的文件ID
            int fileoffset = 0;//记录当前下载的文件偏移量
            try
            {
                FileInfoCollection fc = new FileInfoCollection();
                try
                {
                    fc.Load(Config.WorkPath);
                    if (File.Exists("Resume.xml"))                      //存在续传记录
                    {
                        if (MessageBox.Show(this, labelResume.Text, "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            XmlDocument xmlresume = new XmlDocument();
                            try
                            {
                                xmlresume.Load("Resume.xml");
                                XmlNode noderesume = xmlresume.SelectSingleNode("Resume/FileResume");
                                int id = Convert.ToInt32(noderesume.Attributes["ID"].Value);
                                DateTime date = Convert.ToDateTime(noderesume.Attributes["Date"].Value);
                                int length = Convert.ToInt32(noderesume.Attributes["Length"].Value);
                                int offset = Convert.ToInt32(noderesume.Attributes["Offset"].Value);
                                fc.SetFiletoResumeInfo(id, date, length, offset);
                            }
                            catch
                            { }
                        }
                        else//清空临时文件夹
                        {
                            Directory.Delete("temp", true);
                            Directory.CreateDirectory("temp");
                            File.Delete("Resume.xml");
                        }
                    }
                }
                catch
                {
                    fc.Clear();
                }
                SetControlText(labelInfo, labelDownloadingList.Text);
                byte[] bt = loader.GetDownLoadList(fc.SaveToBuffer());
                if (bt != null)
                {
                    Config.LocalFiles.Load(bt);
                }
                byte[] btimage = loader.GetClientImage(ImageType.Client);
                if (btimage != null)
                {
                    try
                    {
                        if (!Directory.Exists(Config.WorkPath))
                        {
                            Directory.CreateDirectory(Config.WorkPath);
                        }
                        using (FileStream fs = new FileStream(string.Format("{0}\\EEPNetClient.jpg", Config.WorkPath), FileMode.OpenOrCreate))
                        {
                            fs.Write(btimage, 0, btimage.Length);
                        }
                    }
                    catch { }
                }
                btimage = loader.GetClientImage(ImageType.ClientMain);
                if (btimage != null)
                {
                    try
                    {
                        if (!Directory.Exists(Config.WorkPath))
                        {
                            Directory.CreateDirectory(Config.WorkPath);
                        }
                        using (FileStream fs = new FileStream(string.Format("{0}\\EEPNetClientMain.jpg", Config.WorkPath), FileMode.OpenOrCreate))
                        {
                            fs.Write(btimage, 0, btimage.Length);
                        }
                    }
                    catch { }
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure)
                {
                    if (LoadServer(iep))
                    {
                        Thread.Sleep(1000);
                        DownLoad(uri);
                        return;
                    }
                    else
                    {
                        SetControlText(labelInfo, labelCanNotConnect.Text);
                    }
                }
                else
                {
                    SetMessageBox(e.Message);
                }
                SetState(State.Idle);
                return;
            }
            catch (ThreadAbortException) 
            {
                SetControlText(labelInfo, labelUpdateCancel.Text);
                return;
            }
            catch (Exception e)
            {
                SetMessageBox(e.Message);
                SetState(State.Idle);
                return;
            }
            try
            {
                File.Delete("Resume.xml");
                System.Collections.IEnumerator ie = Config.LocalFiles.GetEnumerator();
                int filelength = Config.LocalFiles.Length;
                int filelengthfinished = 0;
                if(filelength > 0)
                {
                    while (ie.MoveNext())
                    {
                        FileInfo fi = ((DictionaryEntry)ie.Current).Value as FileInfo;
                        SetControlText(labelInfo, labelDownloading.Text + fi.ToString());
                        using (FileStream fs = new FileStream(string.Format("temp\\{0}.tmp", fi.ID), FileMode.OpenOrCreate))
                        {
                            fileid = fi.ID;
                            int length = fi.Length;
                            int i = 0;
                            if (Config.LocalFiles.FileResume != null && Config.LocalFiles.FileResume.ID == fi.ID)
                            {
                                i = Config.LocalFiles.FileOffSet;//续传文件从续传位置开始
                            }
                            for (; i < length; i += Config.FILE_BLOCK_LENGTH)
                            {
                                int len = Math.Min(Config.FILE_BLOCK_LENGTH, length - i);
                                byte[] btfile = loader.GetFile(fi.ID, i, len);
                                fs.Position = i;
                                fs.Write(btfile, 0, len);
                                fileoffset = i + len;
                                filelengthfinished += len;

                                SetProgressBarValue(progressBarDetail, (int)((double)(i + len) / (double)fi.Length * 100));
                                SetProgressBarValue(progressBarTotal, (int)((double)filelengthfinished / (double)filelength * 100));
                            }
                        }
                        string filename = Config.WorkPath + fi.ToString();
                        if (!Directory.Exists(Path.GetDirectoryName(filename)))//创建目的文件夹
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(filename));
                        }
                        if (File.Exists(filename))//如果文件存在则删除
                        {
                            File.Delete(filename);
                        }
                        File.Move(string.Format("temp\\{0}.tmp", fi.ID), filename);//移动文件到目的文件夹
                        File.SetLastWriteTime(filename, fi.Date);//设置最后修改时间
                    }
                    SetControlText(labelInfo, labelUpdateFinished.Text);
                }
                else
                {
                    SetControlText(labelInfo, labelNoUpdateFound.Text);
                }
                SetProgressBarValue(progressBarDetail, 100);
                SetProgressBarValue(progressBarTotal, 100);
                if (File.Exists(Config.WorkPath + "\\" + Config.PreInstall) && !Config.CheckPreInstall())
                {
                    try
                    {
                        System.Diagnostics.Process pro = System.Diagnostics.Process.Start(Config.WorkPath + "\\" + Config.PreInstall);
                        pro.WaitForInputIdle();
                    }
                    catch { }
                }
                try
                {
                    if (CurrentSolution != null)
                    {
                        XmlDocument clientxml = CreateClientXml(CurrentSolution);
                        clientxml.Save(Config.WorkPath + @"\EEPNetClient.xml");
                    }
                }
                catch
                {
                    SetMessageBox("Can not Refresh client xml, check it's property not be readonly.");
                }
                try
                {
                    String S = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", CurrentSolution.Language, CurrentSolution.IP, CurrentSolution.ApPort
                    , CurrentSolution.Text, ProxySetting.Default.ProxyEnable,
                    string.Format("{0}:{1}", ProxySetting.Default.ProxyAddress, ProxySetting.Default.ProxyPort)
                    , ProxySetting.Default.ProxyUser, ProxySetting.Default.ProxyPassword);
                    System.Diagnostics.Process pro
                        = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(Config.WorkPath + "\\" + Config.LaunchPath, S));
                    pro.WaitForInputIdle();
                    SetState(State.Idle);
                    //Application.Exit();
                    return;
                }
                catch(Exception e)
                {
                    SetMessageBox(e.Message);
                    SetState(State.Idle);
                }
                
            }
            catch (ThreadAbortException)
            {
                SetControlText(labelInfo, labelUpdateCancel.Text);
                SetState(State.Idle);
            }
            catch (Exception e)
            {
                SetMessageBox(e.Message);
                SetState(State.Idle);
            }
            finally
            {
                if (fileid != 0 && fileoffset != Config.LocalFiles[fileid].Length)//当前文件未完成
                {
                    //存入文件
                    XmlDocument xmlresume = new XmlDocument();
                    XmlNode nodedoc = xmlresume.CreateElement("Resume");
                    xmlresume.AppendChild(nodedoc);
                    XmlNode nodefileresume = xmlresume.CreateElement("FileResume");
                    XmlAttribute att = xmlresume.CreateAttribute("ID");
                    att.Value = fileid.ToString();
                    nodefileresume.Attributes.Append(att);
                    att = xmlresume.CreateAttribute("Offset");
                    att.Value = fileoffset.ToString();
                    nodefileresume.Attributes.Append(att);
                    att = xmlresume.CreateAttribute("Date");
                    att.Value = Config.LocalFiles[fileid].Date.ToString();
                    nodefileresume.Attributes.Append(att);
                    att = xmlresume.CreateAttribute("Length");
                    att.Value = Config.LocalFiles[fileid].Length.ToString();
                    nodefileresume.Attributes.Append(att);

                    nodedoc.AppendChild(nodefileresume);
                    xmlresume.Save("Resume.xml");//记录续传文件的信息
                }

                //SetState(State.Idle);
            }
        }

        private void Refresh(object uri)
        {
            IPEndPoint iep = (IPEndPoint)uri;
            LoaderObject loader = (LoaderObject)Activator.GetObject(typeof(LoaderObject), string.Format("http://{0}/LoaderObject.rem", uri));
            try
            {
                byte[] bt = loader.GetSolutionList();
                if (bt != null)
                {
                    File.WriteAllBytes(Config.SolutionFile, bt);
                    RefreshTreeView();
                    byte[] btimage = loader.GetClientImage(ImageType.ClientLoader);
                    if (btimage != null)
                    {
                        System.IO.MemoryStream ms = new MemoryStream(btimage);
                        SetImage(Image.FromStream(ms));
                    }
                    SetControlText(labelInfo, labelUpdateFinished.Text);
                }
            }
            catch (ThreadAbortException)
            {
                SetControlText(labelInfo, labelUpdateCancel.Text);
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure)
                {
                    if (LoadServer(iep))
                    {
                        Thread.Sleep(1000);
                        Refresh(uri);
                        return;
                    }
                    else
                    {
                        SetControlText(labelInfo, labelCanNotConnect.Text);
                    }
                }
                else
                {
                    SetMessageBox(e.Message);
                }
            }
            catch (Exception e)
            {
                SetMessageBox(e.Message);
            }
            finally
            {
                if (!this.Disposing)
                {
                    SetState(State.Idle);
                }
            }
        }
#endif
        private void ConfigProxy()
        {
            WebRequest.DefaultWebProxy = null;
           
            if (EEPSetUpLibrary.Properties.Settings.Default.ProxyEnable)
            {
                WebProxy proxy = new WebProxy(ProxySetting.Default.ProxyAddress, ProxySetting.Default.ProxyPort);
                proxy.Credentials = new NetworkCredential(ProxySetting.Default.ProxyUser, ProxySetting.Default.ProxyPassword);
                WebRequest.DefaultWebProxy = proxy;
            }
        }

        private bool LoadServer(IPEndPoint remoteendpoint)
        {
            LoaderObject loader = (LoaderObject)Activator.GetObject(typeof(LoaderObject), string.Format("http://{0}/LoaderObject.rem"
                , new IPEndPoint(remoteendpoint.Address, Config.ServerLoaderPort)));
            try
            {
                return loader.StartServer();
            }
            catch
            {
                return false;
            }
        }

        private XmlDocument CreateClientXml(Solution sol)
        {
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateElement("LoginInfo"));
            XmlNode nodedatabase = xml.DocumentElement.AppendChild(xml.CreateElement("DataBase"));
            nodedatabase.InnerText = sol.LoginDataBase;
            XmlNode nodesolution = xml.DocumentElement.AppendChild(xml.CreateElement("Solution"));
            nodesolution.InnerText = sol.LoginSolution;
            return xml;
        }

        #region delegate
        public delegate void SetProgressBarValueMethod(ProgressBar bar, int value);

        public void SetProgressBarValue(ProgressBar bar, int value)
        {
            if (value > bar.Maximum)
                value = bar.Maximum % value;
            if (!this.Disposing)
            {
                if (bar.InvokeRequired)
                {
                    SetProgressBarValueMethod call = delegate(ProgressBar bard, int valued)
                    {
                        bard.Value = valued;
                    };
                    bar.Invoke(call, new object[] { bar, value });
                }
                else
                {
                    bar.Value = value;
                }
            }
        }

        public delegate void SetControlTextMethod(Control ct, string text);

        public void SetControlText(Control ct, string text)
        {
            if (!this.Disposing)
            {
                if (ct.InvokeRequired)
                {
                    SetControlTextMethod call = delegate(Control ctd, string textd)
                    {
                        ctd.Text = textd;
                    };
                    ct.Invoke(call, new object[] { ct, text });
                }
                else
                {
                    ct.Text = text;
                }
            }
        }

        public delegate void SetButtonEnabledMethod(Button btn, bool visible);

        public void SetButtonEnabled(Button btn, bool enable)
        {
            if (!this.Disposing)
            {
                if (btn.InvokeRequired)
                {
                    SetButtonEnabledMethod call = delegate(Button btnd, bool enabled)
                    {
                        btnd.Enabled = enabled;
                    };
                    btn.Invoke(call, new object[] { btn, enable });
                }
                else
                {
                    btn.Enabled = enable;
                }
            }
        }

        public delegate void RefreshMethod();

        public void RefreshTreeView()
        {
            if (!this.Disposing)
            {
                XmlDocument xml = new XmlDocument();
                try
                {
                    xml.Load(Config.SolutionFile);
                    if (treeViewSolution.InvokeRequired)
                    {
                        //SolutionCollection solutions = new SolutionCollection(xml, "");
                        //RefreshMethod call = delegate()
                        //{
                        //    treeViewSolution.Nodes[0].Nodes.Clear();
                        //    for (int i = 0; i < solutions.Count; i++)
                        //    {
                        //        TreeNode node = treeViewSolution.Nodes[0].Nodes.Add(solutions[i].Name, solutions[i].Text, "solution", "solution");
                        //        node.Tag = solutions[i];
                        //        node.ToolTipText = string.Format("{0}:{1}", solutions[i].IP, solutions[i].Port);
                        //    }
                        //    treeViewSolution.ExpandAll();
                        //};
                        //RefreshMethod call = delegate()
                        RefreshMethod call = delegate()
                        {
                            treeViewSolution.Nodes.Clear();
                            XmlNode SolutionNode = null;
                            foreach (XmlNode aNode in xml.ChildNodes)
                                if (aNode.Name.CompareTo("Solutions") == 0)
                                {
                                    SolutionNode = aNode;
                                    break;
                                }
                            foreach (XmlNode GroupXmlNode in SolutionNode)
                            {
                                TreeNode GroupTreeNode = treeViewSolution.Nodes.Add(GroupXmlNode.Attributes["Text"].Value);
                                foreach (XmlNode DeptXmlNode in GroupXmlNode.ChildNodes)
                                {
                                    TreeNode DeptTreeNode = GroupTreeNode.Nodes.Add(DeptXmlNode.Attributes["Text"].Value);
                                    SolutionCollection solutions = new SolutionCollection(xml, String.Format("Solutions/{0}/{1}/Solution", GroupXmlNode.Name, DeptXmlNode.Name));
                                    for (int i = 0; i < solutions.Count; i++)
                                    {
                                        TreeNode node = DeptTreeNode.Nodes.Add(solutions[i].Name, solutions[i].Text, "solution", "solution");
                                        node.Tag = solutions[i];
                                        node.ToolTipText = string.Format("{0}:{1}", solutions[i].IP, solutions[i].Port);
                                    }
                                }
                            }
                            treeViewSolution.ExpandAll();
                        };
                        treeViewSolution.Invoke(call, null);
                    }
                    else
                    {
                        treeViewSolution.Nodes.Clear();
                        XmlNode SolutionNode = null;
                        foreach (XmlNode aNode in xml.ChildNodes)
                            if (aNode.Name.CompareTo("Solutions") == 0)
                            {
                                SolutionNode = aNode;
                                break;
                            }
                        foreach (XmlNode GroupXmlNode in SolutionNode)
                        {
                            TreeNode GroupTreeNode = treeViewSolution.Nodes.Add(GroupXmlNode.Attributes["Text"].Value);
                            foreach (XmlNode DeptXmlNode in GroupXmlNode.ChildNodes)
                            {
                                TreeNode DeptTreeNode = GroupTreeNode.Nodes.Add(DeptXmlNode.Attributes["Text"].Value);
                                SolutionCollection solutions = new SolutionCollection(xml, String.Format("Solutions/{0}/{1}/Solution", GroupXmlNode.Name, DeptXmlNode.Name));
                                for (int i = 0; i < solutions.Count; i++)
                                {
                                    TreeNode node = DeptTreeNode.Nodes.Add(solutions[i].Name, solutions[i].Text, "solution", "solution");
                                    node.Tag = solutions[i];
                                    node.ToolTipText = string.Format("{0}:{1}", solutions[i].IP, solutions[i].Port);
                                }
                            }
                        }
                        treeViewSolution.ExpandAll();
                    }
                }
                catch
                {
                    //MessageBox.Show("RefreshTreeView=" + E.Message);
                }
            }
        }

        public delegate void SetImageMethod(Image image);

        public void SetImage(Image image)
        {
            if (!this.Disposing)
            {
                if (this.pictureBox.InvokeRequired)
                {
                    SetImageMethod call = delegate(Image imaged)
                    {
                        this.pictureBox.Image = imaged;
                    };
                    this.pictureBox.Invoke(call, new object[] { image });
                }
                else
                {
                    this.pictureBox.Image = image;
                }
            }
        }

        public delegate void SetMessageBoxMethod(string text);

        public void SetMessageBox(string text)
        {
            if (!this.Disposing)
            {
                SetMessageBoxMethod call = delegate(string textd)
                {
                    MessageBox.Show(this, textd, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };
                this.Invoke(call, new object[] { text });
            }
        }
        #endregion

        private void frmSetUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.Save();
        }

        private void linkLabelSetUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //linkLabelSetUp.Visible = false;
            //linkLabelOK.Visible = true;
            //linkLabelCancel.Visible = true;
            //labelFolder.Visible = false;
            //label1.Visible = true;
            //textBoxFolder.Visible = true;
            //buttonFolder.Visible = true;
        }

        private void buttonFolder_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.SelectedPath = Config.WorkPath;
            //if (fbd.ShowDialog() == DialogResult.OK)
            //{
            //    textBoxFolder.Text = fbd.SelectedPath + @"\EEPNetClient";
            //}
        }

        private void linkLableCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //linkLabelSetUp.Visible = true;
            //linkLabelOK.Visible = false;
            //linkLabelCancel.Visible = false;
            //labelFolder.Visible = true;
            //label1.Visible = false;
            //textBoxFolder.Visible = false;
            //buttonFolder.Visible = false;
            //textBoxFolder.Text = Config.WorkPath;
        }

        private void linkLabelOK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //linkLabelSetUp.Visible = true;
            //linkLabelOK.Visible = false;
            //linkLabelCancel.Visible = false;
            //labelFolder.Visible = true;
            //label1.Visible = false;
            //textBoxFolder.Visible = false;
            //buttonFolder.Visible = false;
            //Config.WorkPath = textBoxFolder.Text;
            //Config.TempWorkPath = textBoxFolder.Text;
            //labelFolder.Text = textBoxFolder.Text;
        }

        private void linkLabelProxy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmProxy form = new frmProxy();
            if (form.ShowDialog() == DialogResult.OK)
            {
                ConfigProxy();
            }
        }

        private void treeViewSolution_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.Level == 2 && buttonStart.Text == labelStart.Text)
            {
                buttonStart_Click(buttonStart, new EventArgs());
            }
        }
    }

    public enum State
    {
        Refreshing,
        Downloading,
        Idle
    }
}