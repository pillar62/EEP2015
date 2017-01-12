using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;

namespace EEPSetUpLibrary
{
    /// <summary>
    /// 存放文件信息的类
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// 为存在的文件创建FileInfo实例
        /// </summary>
        /// <param name="filename">文件存放的完整路径</param>
        public FileInfo(string filename)
            :this(new System.IO.FileInfo(filename))
        { 

        }

        /// <summary>
        /// 为存在的文件创建FileInfo实例
        /// </summary>
        /// <param name="fi">表示文件的System.IO.FileInfo对象</param>
        public FileInfo(System.IO.FileInfo fi)
        {
            _Name = fi.Name;
            SetInfo(fi.LastWriteTime, (int)fi.Length);
            _Directory = fi.Directory.FullName.Replace(Config.TempWorkPath, string.Empty);//使用更改后的TempWorkPath

            _ID = GetHashCode32(this.ToString());
        }

        /// <summary>
        /// 通过文件信息创建一个FileInfo实例
        /// </summary>
        /// <param name="id">文件的唯一标识号</param>
        /// <param name="name">文件名</param>
        /// <param name="directory">文件存放的目录</param>
        /// <param name="date">文件最后修改的日期</param>
        /// <param name="length">文件的长度</param>
        public FileInfo(int id, string name, string directory, DateTime date, int length)
        {
            _ID = id;
            _Name = name;  
            _Directory = directory;
            _Date = date;
            _Length = length;
        }

        private int _ID;
        /// <summary>
        /// 获取文件的唯一标识号
        /// </summary>
        public int ID
        {
            get { return _ID; }
        }

        private string _Name;
        /// <summary>
        /// 获取文件名
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        private DateTime _Date;
        /// <summary>
        /// 获取文件最后修改的日期
        /// </summary>
        public DateTime Date
        {
            get { return _Date; }
        }

        private int _Length;
        /// <summary>
        /// 获取文件的长度
        /// </summary>
        public int Length
        {
            get { return _Length; }
        }

        private string _Directory;
        /// <summary>
        /// 获取存放文件的目录
        /// </summary>
        public string Directory
        {
            get { return _Directory; }
        }

        private bool _OverWritable = true;
        /// <summary>
        /// 获取或者设定是否要覆盖
        /// </summary>
        public bool OverWritable
        {
            get { return _OverWritable; }
            set { _OverWritable = value; }
        }

        /// <summary>
        /// 通过文件标识号,修改日期,文件长度来比较是否与当前文件是否相同
        /// </summary>
        /// <param name="fi">待比较的文件实例</param>
        /// <returns>是否相同</returns>
        public bool Equals(FileInfo fi)//未测试
        {
            return (!OverWritable) || (!fi.OverWritable)
               || ID.Equals(fi.ID) && Math.Abs(((TimeSpan)(Date - fi.Date)).TotalSeconds) < 1.01 && Length.Equals(fi.Length);//时间类型的比较 0.5Ms会有不相等的情况
        }

        /// <summary>
        /// 获取当前实例的文字说明
        /// </summary>
        /// <returns>文件实例的文字说明</returns>
        public override string ToString()
        {
            return Directory + "\\" + Name;
        }

        /// <summary>
        /// Similar to String.GetHashCode but returns the same
        /// as the x86 version of String.GetHashCode for x64 and x86 frameworks.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static unsafe int GetHashCode32(string s)
        {
            fixed (char* str = s.ToCharArray())
            {
                char* chPtr = str;
                int num = 0x15051505;
                int num2 = num;
                int* numPtr = (int*)chPtr;
                for (int i = s.Length; i > 0; i -= 4)
                {
                    num = (((num << 5) + num) + (num >> 0x1b)) ^ numPtr[0];
                    if (i <= 2)
                    {
                        break;
                    }
                    num2 = (((num2 << 5) + num2) + (num2 >> 0x1b)) ^ numPtr[1];
                    numPtr += 2;
                }
                return (num + (num2 * 0x5d588b65));
            }
        }



        /// <summary>
        /// 设置文件信息的修改时间和长度信息
        /// </summary>
        /// <param name="dt">修改时间</param>
        /// <param name="length">文件长度</param>
        public void SetInfo(DateTime dt, int length)
        {
            _Date = new DateTime(dt.Year,dt.Month,dt.Day,dt.Hour,dt.Minute,dt.Second);
            _Length = length;
        }
    }

    /// <summary>
    /// 存放多个文件信息类的集合,用以管理,并可以保存到xml文档及缓冲数组中
    /// </summary>
    public class FileInfoCollection : Hashtable
    {
        /// <summary>
        /// 创建FileInfoCollection实例
        /// </summary>
        public FileInfoCollection()
        {
            
        }

        /// <summary>
        /// 获取集合中的某个文件信息实例
        /// </summary>
        /// <param name="key">要获取文件的标识号</param>
        /// <returns>获取的文件信息实例</returns>
        public new FileInfo this[object key]
        {
	        get 
	        { 
		         return (FileInfo)base[key];
	        }
	          set 
	        { 
		        base[key] = value;
	        }
        }

        /// <summary>
        /// 获取所有文件的总长度
        /// </summary>
        public int Length
        {
            get 
            {
                int length = 0;
                IEnumerator ie = this.GetEnumerator();
                while (ie.MoveNext())
                {
                    FileInfo fi = ((DictionaryEntry)ie.Current).Value as FileInfo;
                    length += fi.Length;
                }
                length -= FileOffSet;
                return length;
            }
        }

        private int _FileOffSet = 0;//续传使用
        /// <summary>
        /// 获取续传文件传输的开始位置
        /// </summary>
        public int FileOffSet
        {
            get { return _FileOffSet; }
        }

        private FileInfo _FileResume = null;
        /// <summary>
        /// 获取续传文件的信息
        /// </summary>
        public FileInfo FileResume
        {
            get { return _FileResume; }
        }

        /// <summary>
        /// 设置续传文件的信息和开始位置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="offset"></param>
        public void SetFiletoResumeInfo(int id, DateTime dt, int length, int offset)
        {
            _FileResume = new FileInfo(id, null, null, dt, length);
            _FileOffSet = offset;
        }

        /// <summary>
        /// 清空续传信息
        /// </summary>
        private void ClearFiletoResume()
        {
            _FileResume = null;
            _FileOffSet = 0;
        }

        /// <summary>
        /// 从数组中读取文件信息集合
        /// </summary>
        /// <param name="btxml">要读取信息的数组</param>
        public void Load(byte[] btxml)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(btxml);
            XmlDocument xml = new XmlDocument();
            xml.Load(ms);
            Load(xml);
        }

        /// <summary>
        /// 从Xml文档中读取文件信息集合
        /// </summary>
        /// <param name="xml">要读取信息的Xml文档</param>
        public void Load(XmlDocument xml)
        {
            lock (this)
            {
                this.Clear();
                Load(xml.DocumentElement);
            }
        }

        private void Load(XmlNode nodeparent)//递归读出每个Xml节点表示的文件信息
        {
            foreach (XmlNode node in nodeparent.ChildNodes)
            {
                if (node.Name.Equals("Folder"))
                {
                    Load(node);
                }
                else if(node.Name.Equals("File"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    string name = node.Attributes["Name"].Value;
                    DateTime date = Convert.ToDateTime(node.Attributes["Date"].Value);
                    int length = Convert.ToInt32(node.Attributes["Length"].Value);
                    string directoryname = string.Empty;
                    if (nodeparent.Name.Equals("Folder"))
                    {
                        directoryname = nodeparent.Attributes["Name"].Value;
                    }
                    FileInfo finew = new FileInfo(id, name, directoryname, date, length);
                    if (node.Attributes["OverWritable"] != null)
                    {
                        finew.OverWritable = Convert.ToBoolean(node.Attributes["OverWritable"].Value);
                    }
                    this.Add(finew.ID,finew);
                }
                else if (node.Name.Equals("FileResume"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    DateTime date = Convert.ToDateTime(node.Attributes["Date"].Value);
                    int length = Convert.ToInt32(node.Attributes["Length"].Value);
                    int offset = Convert.ToInt32(node.Attributes["Offset"].Value);
                    SetFiletoResumeInfo(id, date, length, offset);
                }
            }
        }

        /// <summary>
        /// 从一个已知目录中读取所有文件的信息
        /// </summary>
        /// <param name="directorypath">一个存放文件的目录</param>
        public void Load(string directorypath)
        {
            lock (this)
            {
                this.Clear();
                Load(new System.IO.DirectoryInfo(directorypath));
            }
        }

        private void Load(System.IO.DirectoryInfo diparent)//递归从每个子目录中读取文件信息
        {
            foreach (System.IO.DirectoryInfo di in diparent.GetDirectories())
            {
                Load(di);
            }
            foreach (System.IO.FileInfo fi in diparent.GetFiles())
            {
                FileInfo finew = new FileInfo(fi);
                this.Add(finew.ID, finew);
            }
        }

        /// <summary>
        /// 将文件信息集合保存到一个Xml文档
        /// </summary>
        /// <returns>保存后的Xml文档</returns>
        public XmlDocument Save()
        {
            XmlDocument xml = new XmlDocument();

            XmlNode nodedocument = xml.CreateElement("FileList");
            xml.AppendChild(nodedocument);

            IEnumerator ie = this.GetEnumerator();
            while (ie.MoveNext())
            {
                FileInfo fi = ((DictionaryEntry)ie.Current).Value as FileInfo;
                XmlNode nodefile = xml.CreateElement("File");
                nodefile.Attributes.Append(CreateAttribute(xml, "ID", fi.ID));
                nodefile.Attributes.Append(CreateAttribute(xml, "Name", fi.Name));
                nodefile.Attributes.Append(CreateAttribute(xml, "Date", fi.Date.ToString("yyyy-MM-dd HH:mm:ss")));//固定时间格式
                nodefile.Attributes.Append(CreateAttribute(xml, "Length", fi.Length));
                if (!fi.OverWritable)
                { 
                    nodefile.Attributes.Append(CreateAttribute(xml, "OverWritable", fi.OverWritable));
                }

                XmlNode nodedirectory = GetDirectoryNode(xml, fi.Directory);
                nodedirectory.AppendChild(nodefile);
            }

            if (FileResume != null)//读入续传文件的信息
            { 
                XmlNode noderesume = xml.CreateElement("FileResume");
                noderesume.Attributes.Append(CreateAttribute(xml,"ID",FileResume.ID));
                noderesume.Attributes.Append(CreateAttribute(xml, "Date", FileResume.Date.ToString("yyyy-MM-dd HH:mm:ss")));
                noderesume.Attributes.Append(CreateAttribute(xml, "Length", FileResume.Length));
                noderesume.Attributes.Append(CreateAttribute(xml, "Offset", FileOffSet));
                nodedocument.AppendChild(noderesume);
            }

            return xml;
        }

        /// <summary>
        /// 将文件信息集合保存到一个缓冲数组
        /// </summary>
        /// <returns>保存后的数组</returns>
        public byte[] SaveToBuffer()
        {
            XmlDocument xml = Save();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            xml.Save(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// 刷新文件信息集合中的信息
        /// </summary>
        public void Refresh()
        {
            lock (this)
            {
                IEnumerator ie = this.GetEnumerator();
                while (ie.MoveNext())
                {
                    FileInfo fi = ((DictionaryEntry)ie.Current).Value as FileInfo;
                    int id = (int)((DictionaryEntry)ie.Current).Key;
                    System.IO.FileInfo finew = new System.IO.FileInfo(Config.WorkPath + fi.ToString());
                    if (finew.Exists)
                    {
                        fi.SetInfo(finew.LastWriteTime, (int)finew.Length);
                    }
                }
            }
        }

        private XmlAttribute CreateAttribute(XmlDocument xml, string attributename, object value)
        {
            XmlAttribute att = xml.CreateAttribute(attributename);
            att.Value = value.ToString();
            return att;
        }//为Xml的节点创建属性

        private XmlNode GetDirectoryNode(XmlDocument xml, string directory)//为Xml的文件信息节点寻找父节点,如果不存在则新建一个
        {
            if (directory.Length == 0)
            {
                return xml.DocumentElement;
            }
            else
            {
                XmlNode nodedirectory = xml.SelectSingleNode(string.Format("descendant::Folder[@Name='{0}']",directory));//文件名中含有'会出错
                if (nodedirectory == null)
                {
                    nodedirectory = xml.CreateElement("Folder");
                    nodedirectory.Attributes.Append(CreateAttribute(xml, "Name", directory));
                    xml.DocumentElement.PrependChild(nodedirectory);
                }
                return nodedirectory;
            }
        }

        /// <summary>
        /// 获取两个文件信息集合的差集,用于得到更新列表
        /// </summary>
        /// <param name="clientfiles">客户端上传的已存在文件集合</param>
        /// <returns>需要更新的文件集合</returns>
        public FileInfoCollection GetDownloadList(FileInfoCollection clientfiles)
        {
            lock (this)
            {
                FileInfoCollection fc = new FileInfoCollection();
                IEnumerator ie = this.GetEnumerator();
                while (ie.MoveNext())
                {
                    FileInfo fi = ((DictionaryEntry)ie.Current).Value as FileInfo;
                    int id = (int)((DictionaryEntry)ie.Current).Key;
                    if (clientfiles.ContainsKey(id) && clientfiles[id].Equals(fi))
                    {
                        //重新取得修改日期及时间
                    }
                    else
                    {
                        fc.Add(fi.ID, fi);
                    }
                }
                if (clientfiles.FileResume != null)//如果存在续传文件
                {
                    if (this.ContainsKey(clientfiles.FileResume.ID) && this[clientfiles.FileResume.ID].Equals(clientfiles.FileResume))//比对续传文件的信息,如果不相同则重新发送
                    {
                        fc.SetFiletoResumeInfo(clientfiles.FileResume.ID, clientfiles.FileResume.Date
                            , clientfiles.FileResume.Length, clientfiles.FileOffSet);
                    }
                }
                return fc;
            }
        }

        /// <summary>
        /// 向一个Socket发送集合中的所有文件
        /// </summary>
        /// <param name="client">已连接的Socket</param>
        public void Send(System.Net.Sockets.Socket client)
        {
            lock (this)
            {
                IEnumerator ie = this.GetEnumerator();
                while (ie.MoveNext())
                {
                    FileInfo fi = ((DictionaryEntry)ie.Current).Value as FileInfo;
                    int id = (int)((DictionaryEntry)ie.Current).Key;

                    lock (this)//防止多线程同时打开文件
                    {
                        System.IO.FileStream fs = new System.IO.FileStream(Config.WorkPath + fi.ToString(), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        try
                        {
                            int i = 0;
                            if (FileResume != null && FileResume.ID == id)
                            {
                                i = FileOffSet;//续传文件从续传位置开始
                            }
                            for (; i < fs.Length; i += Config.FILE_BLOCK_LENGTH)
                            {
                                MessageBuffer mb = new MessageBuffer(MessageType.SendFile, id, i, Config.FILE_BLOCK_LENGTH);
                                mb.ReadFileBytes(fs);
                                client.Send(mb.GetBytes());
                            }
                        }
                        finally
                        {
                            fs.Close();//确保关闭文件流
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 向一个Socket发送集合中某个文件的一部分内容
        /// </summary>
        /// <param name="client">已连接的Socket</param>
        /// <param name="id">文件的标识号</param>
        /// <param name="offset">发送内容的起始位置</param>
        /// <param name="length">发送内容的长度</param>
        public void Send(System.Net.Sockets.Socket client, int id, int offset, int length)
        {
            if (this.ContainsKey(id))
            {
                FileInfo fi = this[id];
                lock (this)
                {
                    System.IO.FileStream fs = new System.IO.FileStream(Config.WorkPath + fi.ToString(), System.IO.FileMode.Open);

                    MessageBuffer mb = new MessageBuffer(MessageType.SendFile, id, offset, length);
                    mb.ReadFileBytes(fs);
                    client.Send(mb.GetBytes());
                    fs.Close();
                }
            }
            else
            {
                MessageBuffer mb = new MessageBuffer(MessageType.RequeryBlockError, id, offset, 0);
                client.Send(mb.GetBytes());
            }
        }

        /// <summary>
        /// 复制一个相同的文件信息集合
        /// </summary>
        /// <returns>复制得到的集合</returns>
        public new FileInfoCollection Clone()
        {
            lock (this)
            {
                FileInfoCollection fc = new FileInfoCollection();
                IEnumerator ie = this.GetEnumerator();
                while (ie.MoveNext())
                {
                    FileInfo fi = ((DictionaryEntry)ie.Current).Value as FileInfo;
                    fc.Add(fi.ID, fi);
                }
                return fc;
            }
        }

        /// <summary>
        /// 清空集合,包括续传信息
        /// </summary>
        public override void Clear()
        {
            lock (this)
            {
                base.Clear();
                ClearFiletoResume();
            }
        }
    }
}
