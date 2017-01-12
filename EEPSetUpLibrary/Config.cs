using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using Microsoft.Win32;

namespace EEPSetUpLibrary
{
    //定义连接,传输,更新的参数
    public static class Config
    {
        /// <summary>
        /// 获取或者设定更新文件的路径,Server启动后不再更改(新版本中已经和TempWorkPath相同)
        /// </summary>
        private static String workPath = String.Empty;
        public static string WorkPath
        {
            get 
            {
                if (String.IsNullOrEmpty(workPath))
                {
                    return Application.StartupPath + @"\EEPNetClient";
                }
                else
                {
                    return workPath;
                }
            }
            set { workPath = value; }
        }

        /// <summary>
        /// 获取或者设定Server修改后的更新文件的路径,如没有修改则和WorkPath相同
        /// </summary>
        public static string TempWorkPath = string.Empty;

        /// <summary>
        /// 获取或者设定是否能更改安装路径(新版本已经无效)
        /// </summary>
        public static bool ChangeFolder = false;

#if !Remoting
        /// <summary>
        /// 获取或者设定连接使用的端口号,默认为8990
        /// </summary>
        public static int ServerPort = 8990;
#else
        /// <summary>
        /// 获取或者设定连接使用的端口号,默认为8989
        /// </summary>
        public static int ServerPort = 8989;
#endif

        /// <summary>
        /// 获取或者设定ServerLoader使用的端口号,默认为8000
        /// </summary>
        public static int ServerLoaderPort = 8000;

        /// <summary>
        /// 获取或者设定EEPNetClient登录图片
        /// </summary>
        public static string ClientImagePath = string.Empty;

        /// <summary>
        /// 获取或者设定EEPNetClient背景图片
        /// </summary>
        public static string ClientMainImagePath = string.Empty;

        /// <summary>
        /// 获取或者设定EEPNetClientLoader图片
        /// </summary>
        public static string ClientLoaderImagePath = string.Empty;

        /// <summary>
        /// 更新后开启的应用程序
        /// </summary>
        public const string LaunchPath = "EEPNetClient.exe";//客户端使用

        /// <summary>
        /// 需要预安装的应用程序
        /// </summary>
        public const string PreInstall = "CRRedist2005_x86.msi";

        /// <summary>
        /// 定义Solution的文件
        /// </summary>
        public static readonly string SolutionFile = Application.StartupPath + "\\Solution.xml";

        /// <summary>
        /// 配置文件
        /// </summary>
        public static readonly string ConfigFile = Application.StartupPath + "\\UpdateConfig.xml";

        /// <summary>
        /// 更新文件列表
        /// </summary>
        public static readonly string FileListFile = Application.StartupPath + "\\UpdateFile.xml";
        
        /// <summary>
        /// 可用服务器列表
        /// </summary>
        public static readonly string ServerListFile = Application.StartupPath + "\\ServerList.xml";

        /// <summary>
        /// 获取或者设定连接的ServerIP地址
        /// </summary>
        public static string ServerIP = "127.0.0.1";

        /// <summary>
        /// 获取或者设定超时的间隔,单位为毫秒
        /// </summary>
        public static double TimeOutInterval = 120000;//客户端使用

        ///// <summary>
        ///// 获取或者设定显示的语言
        ///// </summary>
        //public static LanguageInfo Language = LanguageInfo.English;

        /// <summary>
        /// 单次发送数据的最大长度
        /// </summary>
        public const int MAX_LENGTH = 204800;//200K, 为了安全可以再调大

#if !Remoting
        /// <summary>
        /// 发送文件的块长度
        /// </summary>
        public const int FILE_BLOCK_LENGTH = 1024;//文件最小块为1K
#else
        /// <summary>
        /// 发送文件的块长度
        /// </summary>
        public const int FILE_BLOCK_LENGTH = 20480;//文件最小块为20K
#endif

        /// <summary>
        /// 获取或者设定是否启用日志,默认为启用
        /// </summary>
        public static bool LogEnable = true;

        /// <summary>
        /// 获取或者设定需要更新文件的集合,Server启动后不再更改(新版本中已经和TempLocalFiles相同)
        /// </summary>
        public static FileInfoCollection LocalFiles = new FileInfoCollection();

        /// <summary>
        /// 获取或者设定Server修改后的需要更新文件的集合,如没有修改则和LocalFiles相同
        /// </summary>
        public static FileInfoCollection TempLocalFiles = new FileInfoCollection();

        /// <summary>
        /// 从文件中读取参数
        /// </summary>
        /// <returns>是否读取成功</returns>
        public static bool Load()//from xml
        {
            if (File.Exists(ConfigFile))
            {
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(ConfigFile);
                    XmlNode nodeworkpath = xml.SelectSingleNode("Config/WorkPath");
                    if (nodeworkpath != null)
                    {
                        WorkPath = nodeworkpath.InnerText.Trim();
                    }

                    TempWorkPath = WorkPath;

                    XmlNode nodeport = xml.SelectSingleNode("Config/Port");
                    if (nodeport != null)
                    {
                        ServerPort = Convert.ToInt32(nodeport.InnerText.Trim());
                    }
                    XmlNode nodeip = xml.SelectSingleNode("Config/IP");
                    if (nodeip != null)
                    {
                        ServerIP = nodeip.InnerText.Trim();
                    }
                    XmlNode nodeclientimagepath = xml.SelectSingleNode("Config/ClientImagePath");
                    if (nodeclientimagepath != null)
                    {
                        ClientImagePath = nodeclientimagepath.InnerText.Trim();
                    }
                    XmlNode nodeclientmainpath = xml.SelectSingleNode("Config/ClientMainImagePath");
                    if (nodeclientmainpath != null)
                    {
                        ClientMainImagePath = nodeclientmainpath.InnerText.Trim();
                    }
                    XmlNode nodeclientloaderimagepath = xml.SelectSingleNode("Config/ClientLoaderImagePath");
                    if (nodeclientloaderimagepath != null)
                    {
                        ClientLoaderImagePath = nodeclientloaderimagepath.InnerText.Trim();
                    }
                    XmlNode nodechangefolder = xml.SelectSingleNode("Config/ChangeFolder");
                    if (nodechangefolder != null)
                    {
                        ChangeFolder = Convert.ToBoolean(nodechangefolder.InnerText.Trim());
                    }
                    return true;
                }
                catch
                { 
                
                }
            }
            return false;
        }

        /// <summary>
        /// 保存参数到文件
        /// </summary>
        public static void Save()//to xml
        {
            XmlDocument xml = new XmlDocument();
            XmlNode nodedocument = xml.CreateElement("Config");
            xml.AppendChild(nodedocument);

            if (!String.IsNullOrEmpty(workPath))
            {
                XmlNode nodeworkpath = xml.CreateElement("WorkPath");
                nodeworkpath.InnerText = TempWorkPath;
                nodedocument.AppendChild(nodeworkpath);
            }

            XmlNode nodeport = xml.CreateElement("Port");
            nodeport.InnerText = ServerPort.ToString();
            nodedocument.AppendChild(nodeport);

            XmlNode nodeip = xml.CreateElement("IP");
            nodeip.InnerText = ServerIP;
            nodedocument.AppendChild(nodeip);

            if (ClientImagePath.Length > 0)
            {
                XmlNode nodeclientimagepath = xml.CreateElement("ClientImagePath");
                nodeclientimagepath.InnerText = ClientImagePath;
                nodedocument.AppendChild(nodeclientimagepath);
            }
            if (ClientMainImagePath.Length > 0)
            {
                XmlNode nodeclientmainimagepath = xml.CreateElement("ClientMainImagePath");
                nodeclientmainimagepath.InnerText = ClientMainImagePath;
                nodedocument.AppendChild(nodeclientmainimagepath);
            }
            if (ClientLoaderImagePath.Length > 0)
            {
                XmlNode nodeclientloaderimagepath = xml.CreateElement("ClientLoaderImagePath");
                nodeclientloaderimagepath.InnerText = ClientLoaderImagePath;
                nodedocument.AppendChild(nodeclientloaderimagepath);
            }
            if (ChangeFolder)
            {
                XmlNode nodechangefolder = xml.CreateElement("ChangeFolder");
                nodechangefolder.InnerText = ChangeFolder.ToString();
                nodedocument.AppendChild(nodechangefolder);
            }
            xml.Save(ConfigFile);
        }

        /// <summary>
        /// 从文件中读取更新文件列表
        /// </summary>
        /// <returns>读取是否成功</returns>
        public static bool LoadFiles()
        {
            if (File.Exists(FileListFile))
            {
                XmlDocument xml = new XmlDocument();
                try
                {
                    xml.Load(FileListFile);
                    LocalFiles.Load(xml);
                    LocalFiles.Refresh();//刷新文件信息
                    SaveFiles();
                    TempLocalFiles = LocalFiles.Clone();//复制列表
                    return true;
                }
                catch 
                { 
                    LocalFiles.Clear(); 
                }
            }
            return false;
        }

        /// <summary>
        /// 保存更新文件集合到文件中
        /// </summary>
        public static void SaveFiles()
        {
            SaveFiles(LocalFiles);
        }

        /// <summary>
        /// 从文件读取服务器列表
        /// </summary>
        /// <returns></returns>
        public static List<string> LoadServerList()
        {
            List<string> listserver = new List<string>();
            if (File.Exists(ServerListFile))
            {
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(ServerListFile);
                    XmlNodeList nodeserver = xml.SelectNodes("List/Server");
                    foreach (XmlNode node in nodeserver)
                    {
                        listserver.Add(node.InnerText);
                    }
                }
                catch
                {

                }
            }
            return listserver;
        }

        /// <summary>
        /// 保存服务器IP到文档
        /// </summary>
        /// <param name="server">服务器IP</param>
        public static void SaveServerList(string server)
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(ServerListFile);
            }
            catch
            {
                xml.AppendChild(xml.CreateElement("List"));
            }
            XmlNodeList nodeserver = xml.SelectNodes("List/Server");
            foreach (XmlNode node in nodeserver)
            {
                if (node.InnerText.Equals(server.Trim()))
                {
                    return;
                }
            }
            XmlNode nodenew = xml.CreateElement("Server");
            nodenew.InnerText = server.Trim();
            xml.DocumentElement.AppendChild(nodenew);
            xml.Save(ServerListFile);
        }

        /// <summary>
        /// 保存更新文件集合到文件中
        /// </summary>
        /// <param name="fc">要保存的更新文件集合</param>
        public static void SaveFiles(FileInfoCollection fc)
        {
            XmlDocument xml = fc.Save();
            xml.Save(FileListFile);
        }

        /// <summary>
        /// 检查是否已经安装预安装应用程序
        /// </summary>
        /// <returns>是否已经安装应用程序</returns>
        public static bool CheckPreInstall()
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Crystal Decisions\10.2\Crystal Reports", false);
            return (rk != null);
        }
    }

    /// <summary>
    /// 语言类别
    /// </summary>
    public enum LanguageInfo
    {
        /// <summary>
        /// 英文
        /// </summary>
        English,
        /// <summary>
        /// 简体中文
        /// </summary>
        SimplifiedChinese,
        /// <summary>
        /// 繁体中文
        /// </summary>
        TraditionalChinese,
        /// <summary>
        /// 日文
        /// </summary>
        Japanese,
        /// <summary>
        /// 韩文
        /// </summary>
        Korea
    }
}
