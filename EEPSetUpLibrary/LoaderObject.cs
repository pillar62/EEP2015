using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace EEPSetUpLibrary
{
    /// <summary>
    /// ClientLoader使用的Remoting对象
    /// </summary>
    public class LoaderObject:MarshalByRefObject
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

        /// <summary>
        /// 取得Solution文件
        /// </summary>
        /// <returns>文件的内容</returns>
        public byte[] GetSolutionList()
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
            return ms.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshFileList()
        {
            Config.LocalFiles.Refresh();
        }

        /// <summary>
        /// 取得下载列表
        /// </summary>
        /// <param name="fileList">已存在的文件列表</param>
        /// <returns>需要更新的文件列表</returns>
        public byte[] GetDownLoadList(byte[] fileList)
        {
            FileInfoCollection fc = new FileInfoCollection();
            fc.Load(fileList);
            FileInfoCollection fcdownload = Config.LocalFiles.GetDownloadList(fc);
            return fcdownload.SaveToBuffer();
        }

        /// <summary>
        /// 取得文件的内容
        /// </summary>
        /// <param name="fileid">文件的ID</param>
        /// <param name="start">开始位置</param>
        /// <param name="length">长度</param>
        /// <returns>文件的内容</returns>
        public byte[] GetFile(int fileid, int start, int length)
        {
            if (Config.LocalFiles.ContainsKey(fileid))
            {
                FileInfo fi = Config.LocalFiles[fileid];
                lock (this)
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream(Config.WorkPath + fi.ToString(), System.IO.FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[length];
                        fs.Position = start;
                        fs.Read(buffer, 0, length);
                        return buffer;
                    }
                }
            }
            else
            {
                return null;
            }
        
        }

        /// <summary>
        /// 取得EEPNetClient的登录图片
        /// </summary>
        /// <param name="type">图片的类型</param>
        /// <returns>图片的内容</returns>
        public byte[] GetClientImage(ImageType type)
        {
            string path = null;
            switch (type)
            {
                case ImageType.Client: path = Config.ClientImagePath; break;
                case ImageType.ClientMain: path = Config.ClientMainImagePath; break;
                case ImageType.ClientLoader: path = Config.ClientLoaderImagePath; break;
                default: path = Config.ClientLoaderImagePath; break;
            }
            lock (this)
            {
                if (File.Exists(path))
                {
                    lock (this)
                    {
                        using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, FileAccess.Read))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            return buffer;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 启动EEPNetServer
        /// </summary>
        /// <returns>是否启动成功</returns>
        public bool StartServer()
        {
            lock (this)
            {
                if (!CheckServerProcess())
                {
                    if (!StartServer(Application.StartupPath))
                    {
                        RegistryKey rk = Registry.LocalMachine.OpenSubKey(REGISTRYNAME, false);
                        if (rk != null)
                        {
                            string serverpath = rk.GetValue("Server Path").ToString();
                            return StartServer(serverpath);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        private bool CheckServerProcess()
        {
            Process[] pros = Process.GetProcessesByName("EEPNetServer");
            return pros.Length > 0;
        }

        private bool StartServer(string directory)
        {
            if (File.Exists(Application.StartupPath + "\\EEPNetServer.exe"))
            {
                StartServerProcess(directory + "\\EEPNetServer.exe");
                return true;
            }
            else
            {
                foreach (string file in Directory.GetFiles(directory, "*.exe", SearchOption.TopDirectoryOnly))
                {
                    FileVersionInfo info = FileVersionInfo.GetVersionInfo(file);
                    if (string.Compare(info.OriginalFilename, "EEPNetServer.exe") == 0)
                    {
                        StartServerProcess(file);
                        return true;
                    }
                }
                return false;
            }
        }

        private void StartServerProcess(string path)
        {
            Process pro = Process.Start(path);
            pro.WaitForInputIdle();
        }
    }

    public enum ImageType
    {
        Client,
        ClientMain,
        ClientLoader
    }
}
