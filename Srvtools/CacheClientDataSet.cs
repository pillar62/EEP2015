using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Data;
using System.Windows.Forms;

namespace Srvtools
{
    public class CacheDataSet : InfoDataSet, ISupportInitialize
    {
        public CacheDataSet()
            : base()
        {
            Eof = true;
            PacketRecords = -1;
        }

        public CacheDataSet(System.ComponentModel.IContainer container):this()
		{
			container.Add(this);
		}

        private bool active;

        public new bool Active
        {
            get
            {
                if (DesignMode)
                {
                    return base.Active;
                }
                else
                {
                    return active;
                }
            }
            set
            {
                if (DesignMode)
                {
                    base.Active = value;
                }
                else
                {
                    if (value != active)
                    {
                        if (value)
                        {
                            if (!string.IsNullOrEmpty(RemoteName))
                            {
                                if (!DesignMode)
                                {
                                    Load();
                                }
                                LoadDataBaseData();
                                active = true;
                            }
                            else
                            {
                                active = init;
                            }
                        }
                        else
                        {
                            RealDataSet.Clear();
                            active = false;
                        }
                    }
                }
            }
        }

        [Category("Infolight"),
        Description("Remote Name of InfoDataSet"),
        Editor(typeof(RemoteNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public new string RemoteName
        {
            get { return base.RemoteName; }
            set 
            { 
                base.RemoteName = value;
                if(Active && RealDataSet.Tables.Count == 0)
                {
                    Active = false;
                    Active = true;
                }
            }
        }

        private string cacheFile;
        [Category("Infolight")]
        public string CacheFile
        {
            get { return cacheFile; }
            set { cacheFile = value; }
        }

        private CacheModeType cacheMode;

        public CacheModeType CacheMode
        {
            get { return cacheMode; }
            set { cacheMode = value; }
        }

        private bool loadData;
        [Category("Infolight")]
        public bool LoadData
        {
            get { return loadData; }
            set { loadData = value; }
        }

        private bool readOnly;
        [Category("Infolight")]
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new int PacketRecords
        {
            get
            {
                return base.PacketRecords;
            }
            set
            {
                base.PacketRecords = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool DeleteIncomplete
        {
            get
            {
                return base.DeleteIncomplete;
            }
            set
            {
                base.DeleteIncomplete = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool AlwaysClose
        {
            get
            {
                return base.AlwaysClose;
            }
            set
            {
                base.AlwaysClose = value;
            }
        }

        bool init = false;

        #region ISupportInitialize Members

        void ISupportInitialize.BeginInit()
        {
            init = true;
        }

        void ISupportInitialize.EndInit()
        {
            init = false;
        }

        #endregion

        public void Save()
        {
            if (string.IsNullOrEmpty(CacheFile))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, this.GetType(), null, "CacheFile", null);
            }
            string fileName = string.Format("{0}\\Cache\\{1}", Application.StartupPath, CacheFile);
            Save(fileName);
        }

        public void Save(string fileName)
        {
            string schemafile = string.Format("{0}.xsd", fileName);
            string dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            this.RealDataSet.WriteXmlSchema(string.Format("{0}.xsd", fileName));
            this.RealDataSet.WriteXml(fileName, XmlWriteMode.DiffGram);
        }

        public void Load()
        {
            if (string.IsNullOrEmpty(CacheFile))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, this.GetType(), null, "CacheFile", null);
            }
            string fileName = string.Format("{0}\\Cache\\{1}", Application.StartupPath, CacheFile);
            Load(fileName);
        }

        public void Load(string fileName)
        {
            string schemafile = string.Format("{0}.xsd", fileName);
            if (File.Exists(fileName) && File.Exists(schemafile))
            {
                this.RealDataSet.ReadXmlSchema(schemafile);
                this.RealDataSet.ReadXml(fileName, XmlReadMode.DiffGram);
            }
        }

        const string UPDATE_DATE = "UPDATE_DATE";
        const string UPDATE_TIME = "UPDATE_TIME";
        const string UPDATE_STATE = "UPDATE_STATE";

        private void LoadDataBaseData()
        {
            //设计时或者文件中没数据要取架构
            if (DesignMode || RealDataSet.Tables.Count == 0)
            {
                base.SetWhere("1=0");
                if (!DesignMode)
                {
                    Save();
                }
            }

            if (LoadData && !DesignMode)
            {
                GetData(CacheMode);
            }
        }

        public void GetData(CacheModeType mode)
        {
            string lastDateTime = LastDateTime;
            InfoDataSet ids = new InfoDataSet();
            ids.RemoteName = this.RemoteName;
            ids.PacketRecords = -1;
            ClientType dbType = CliUtils.GetDataBaseType();
            string mark = (dbType == ClientType.ctMsSql || dbType == ClientType.ctOleDB) ? "+" : "||";
            ids.WhereStr = string.Format("{0}{1}{2}>'{3}'", UPDATE_DATE, mark, UPDATE_TIME, lastDateTime);
            DateTime datetime =DateTime.Now;
         
            ids.Active = true;

            if (ids.RealDataSet.Tables[0].Rows.Count > 0)
            {
                if (mode == CacheModeType.All)
                {
                    RealDataSet.Clear();
                    ids.SetWhere(string.Format("{0}='I'or {0}='M'", UPDATE_STATE));
                }
                else if (mode == CacheModeType.Smart)
                {
                    DataRow[] rowDeleteDB = ids.RealDataSet.Tables[0].Select(string.Format("{0}='D'", UPDATE_STATE));
                    foreach (DataRow dr in rowDeleteDB)
                    {
                        int pklength = ids.RealDataSet.Tables[0].PrimaryKey.Length;
                        if (pklength > 0)
                        {
                            object[] values = new object[pklength];
                            for (int i = 0; i < pklength; i++)
                            {
                                values[i] = dr[ids.RealDataSet.Tables[0].PrimaryKey[i]];
                            }
                            DataRow filerow = this.RealDataSet.Tables[0].Rows.Find(values);
                            if (filerow != null)
                            {
                                this.RealDataSet.Tables[0].Rows.Remove(filerow);
                            }
                        }
                        ids.RealDataSet.Tables[0].Rows.Remove(dr);
                    }
                }
                this.RealDataSet.Merge(ids.RealDataSet);

                //save
                Save();
            }
            LastDateTime = datetime.ToString("yyyyMMddHHmmss", new System.Globalization.CultureInfo("en-us"));
        }

        const string XML_ROOT = "CACS";

        const string XML_NODE = "CAC";

        private string LastDateTime
        {
            get 
            {
                XmlNode node = DateTimeNode;
                return node.Attributes["LastTime"] != null ? node.Attributes["LastTime"].Value : string.Empty;
            }
            set
            {
                XmlNode node = DateTimeNode;
                XmlAttribute att = node.Attributes["LastTime"];
                if (att == null)
                {
                    att = node.OwnerDocument.CreateAttribute("LastTime");
                    node.Attributes.Append(att);
                }
                att.Value = value;
                string fileName = string.Format("{0}\\Cache\\CAC.xml", Application.StartupPath, CacheFile);
                node.OwnerDocument.Save(fileName);
            }
        }

        private XmlNode DateTimeNode
        {
            get
            {
                if (string.IsNullOrEmpty(CacheFile))
                {
                    throw new EEPException(EEPException.ExceptionType.PropertyNull, this.GetType(), null, "CacheFile", null);
                }
                string fileName = string.Format("{0}\\Cache\\CAC.xml", Application.StartupPath, CacheFile);
                XmlDocument xml = new XmlDocument();
                if (File.Exists(fileName))
                {
                    
                    xml.Load(fileName);
                }
                else
                { 
                    xml.AppendChild(xml.CreateElement(XML_ROOT));
                }
                XmlNode node = xml.SelectSingleNode(string.Format("{0}/{1}[@Name='{2}']", XML_ROOT, XML_NODE, CacheFile));
                if (node == null)
                {
                    node = xml.CreateElement(XML_NODE);
                    XmlAttribute att = xml.CreateAttribute("Name");
                    att.Value = CacheFile;
                    node.Attributes.Append(att);
                    xml.DocumentElement.AppendChild(node);
                    xml.Save(fileName);
                }
                return node;
            }
        }

        public new bool ApplyUpdates(bool isClear)
        {
            if (!this.ReadOnly)
            {
                if (base.ApplyUpdates())
                {
                    if (isClear)
                    {
                        this.RealDataSet.Clear();
                    }
                    Save();
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        
        }

        public new bool ApplyUpdates()
        {
            return this.ApplyUpdates(false);
        }
    }

    public enum CacheModeType
    {
        All = 0,
        Smart = 1
    }
}