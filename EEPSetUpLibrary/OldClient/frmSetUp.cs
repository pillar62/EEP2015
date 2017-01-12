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

namespace EEPSetUpLibrary.OldClient
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

        private string SelectedServer = string.Empty;

        private void frmSetUp_Load(object sender, EventArgs e)
        {
            if(!Directory.Exists("temp"))                   //如果不存在临时文件夹就作出创建
            {
                Directory.CreateDirectory("temp");
            }
            Config.Load();
            if (Config.WorkPath.Length == 0)
            {
                Config.WorkPath = Application.StartupPath + "\\EEPNetClient";
                Config.TempWorkPath = Config.WorkPath;
            }
            List<string> listserver = Config.LoadServerList();
            foreach (string str in listserver)
            {
                comboBoxServer.Items.Add(str);
            }
            labelFolder.Text = Config.WorkPath;
            textBoxFolder.Text = Config.WorkPath;
            comboBoxServer.Text = Config.ServerIP;

            if (!string.IsNullOrEmpty(SelectedServer))
            {
                this.splitContainer1.Panel2.Enabled = false;
            buttonStart_Click(buttonStart, new EventArgs());
            }
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

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == "&Start")//开始
            {
                IPEndPoint iep = GetEndPoint(comboBoxServer.Text);
                if (iep != null)
                {
                    if (Config.WorkPath.Length == 0)
                    {
                        MessageBox.Show(this, "Path can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Config.ServerIP = comboBoxServer.Text;
                    Config.SaveServerList(comboBoxServer.Text);
                    SetButtonEnabled(buttonUninstall, false);
                    SetControlText(buttonStart, "&Cancel");
                    SetControlText(labelInfo, "Connect to server");
                    SetProgressBarValue(progressBarDetail, 0);
                    SetProgressBarValue(progressBarTotal, 0);
#if !Remoting
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    time = new TimeOut(Config.TimeOutInterval, new TimeOut.TimeOutCallBack(TimeOut), client);
                    time.Start();//开始计时
                    client.BeginConnect(iep, new AsyncCallback(Connect), iep);
#else
                    td = new Thread(new ParameterizedThreadStart(Connect));
                    td.Start(iep);
#endif
                }
            }
            else                      //取消
            {
                SetControlText(buttonStart, "&Start");
                SetButtonEnabled(buttonUninstall, true);
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

#if !Remoting
        TimeOut time = null;
        Socket client = null;
        private void Connect(IAsyncResult iar)
        {
            //Socket client = iar.AsyncState as Socket;
            try
            {
                client.EndConnect(iar);
            }
            catch(SocketException)
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
                        client.BeginConnect(remoteendpoint, new AsyncCallback(Connect), client);
                    }
                    else
                    {
                        SetControlText(labelInfo, "Can not connect to server");//
                        SetControlText(buttonStart, "&Start");
                    }
                    return;
                }
                catch (ObjectDisposedException)
                {
                    SetControlText(labelInfo, "Update has been canceled");
                    time.Stop();
                    SetProgressBarValue(progressBarDetail, 0);
                    SetProgressBarValue(progressBarTotal, 0);
                    return;
                } 
                #endregion
            }
            catch (ObjectDisposedException)
            {
                SetControlText(labelInfo, "Update has been canceled");
                time.Stop();
                SetProgressBarValue(progressBarDetail, 0);
                SetProgressBarValue(progressBarTotal, 0);
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
                        if (MessageBox.Show(this, "Last update has not completed, Resume it?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
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
            //else
            //{
            //    if (File.Exists("Resume.xml"))                     
            //    {
            //        File.Delete("Resume.xml");
            //    }
            //    Directory.Delete("temp", true);
            //    Directory.CreateDirectory("temp");
            //}

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
                SetControlText(labelInfo, "Downloading updated file list");

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
                                            SetControlText(labelInfo, "Downloading:" + Config.LocalFiles[fileid].ToString());
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
                                        SetControlText(labelInfo, "No updated files found");
                                    }
                                    else
                                    {
                                        SetControlText(labelInfo, "Update has finished");
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
                                            System.Diagnostics.Process pro = System.Diagnostics.Process.Start(Config.WorkPath + "\\" + Config.LaunchPath);
                                            pro.WaitForInputIdle();
                                            Application.Exit();
                                        }
                                        catch
                                        {
                                            SetMessageBox("Can not launch " + Config.LaunchPath);
                                        }
                                    }
                                    SetControlText(buttonStart, "&Start");
                                    SetButtonEnabled(buttonUninstall, true);
                                    return;
                            }
                        }
                    }
                }
            }
            catch(Exception e)//有些文件在copy时会有权限问题,还查不出
            {
                if (client.Connected)
                {
                    client.Send(MessageBuffer.ReceiveFinished.GetBytes());
                }
                if (buttonStart.Text == "&Start")
                {
                    SetControlText(labelInfo, "Update has been canceled");
                    SetProgressBarValue(progressBarDetail, 0);
                    SetProgressBarValue(progressBarTotal, 0);
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
                    SetProgressBarValue(progressBarDetail, 0);
                    SetProgressBarValue(progressBarTotal, 0);
                    SetMessageBox(e.Message);
                }
                SetControlText(buttonStart, "&Start");
                SetButtonEnabled(buttonUninstall, true);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
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
        private void Connect(object uri)
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
                        if (MessageBox.Show(this, "Last update has not completed, Resume it?", "Info",  MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
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
                SetControlText(labelInfo, "Downloading updated file list");
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
                        Connect(uri);
                        return;
                    }
                    else
                    {
                        SetControlText(labelInfo, "Can not connect to server");
                    }
                }
                else
                {
                    SetMessageBox(e.Message);
                }
                SetControlText(buttonStart, "&Start");
                SetButtonEnabled(buttonUninstall, true);
                return;
            }
            catch (ThreadAbortException) 
            {
                SetControlText(labelInfo, "Update has been canceled");
                return;
            }
            catch (Exception e)
            {
                SetMessageBox(e.Message);
                SetControlText(buttonStart, "&Start");
                SetButtonEnabled(buttonUninstall, true);
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
                        SetControlText(labelInfo, "Downloading:" + fi.ToString());
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
                    SetControlText(labelInfo, "Update has finished");
                }
                else
                {
                    SetControlText(labelInfo, "No updated files found");
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
                    System.Diagnostics.Process pro
                        = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(Config.WorkPath + "\\" + Config.LaunchPath));
                    pro.WaitForInputIdle();
                    Application.Exit();
                    return;
                }
                catch(Exception e)
                {
                    SetMessageBox(e.Message);
                    SetControlText(buttonStart, "&Start");
                    SetButtonEnabled(buttonUninstall, true);
                }
                
            }
            catch (ThreadAbortException)
            {
                SetControlText(labelInfo, "Update has been canceled");
            }
            catch (Exception e)
            {
                SetMessageBox(e.Message);
                SetControlText(buttonStart, "&Start");
                SetButtonEnabled(buttonUninstall, true);
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
            }
        }
#endif

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
            linkLabelSetUp.Visible = false;
            linkLabelOK.Visible = true;
            linkLabelCancel.Visible = true;
            labelFolder.Visible = false;
            textBoxFolder.Visible = true;
            buttonFolder.Visible = true;
        }

        private void buttonFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Config.WorkPath;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBoxFolder.Text = fbd.SelectedPath + @"\EEPNetClient";
            }
        }

        private void linkLableCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabelSetUp.Visible = true;
            linkLabelOK.Visible = false;
            linkLabelCancel.Visible = false;
            labelFolder.Visible = true;
            textBoxFolder.Visible = false;
            buttonFolder.Visible = false;
            textBoxFolder.Text = Config.WorkPath;
        }

        private void linkLabelOK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabelSetUp.Visible = true;
            linkLabelOK.Visible = false;
            linkLabelCancel.Visible = false;
            labelFolder.Visible = true;
            textBoxFolder.Visible = false;
            buttonFolder.Visible = false;
            Config.WorkPath = textBoxFolder.Text;
            Config.TempWorkPath = textBoxFolder.Text;
            labelFolder.Text = textBoxFolder.Text;
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
                catch(Exception ed)
                {
                    MessageBox.Show(this, string.Format("Uninstall encounter errors:\r\n{0}", ed.Message)
                        ,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}