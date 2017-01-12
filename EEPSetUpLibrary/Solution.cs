using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;

namespace EEPSetUpLibrary
{
    /// <summary>
    /// Solution定义
    /// </summary>
    public class Solution
    {
        public Solution(string name, string text, string ip, int port, int apport, string logindatabase, string loginsolution, LanguageType language)
        {
            _Name = name;
            _Text = text;
            _IP = ip;
            _Port = port;
            _ApPort = apport;
            _LoginDataBase = logindatabase;
            _LoginSolution = loginsolution;
            _Language = language;
        }

        public Solution(XmlNode node)
        {
            if (node.Attributes["Name"] == null)
            {
                throw new Exception("Solution name can not be empty");
            }
            _Name = node.Attributes["Name"].Value;
            _Text = node.Attributes["Text"] == null ? string.Empty : node.Attributes["Text"].Value;
            _IP = node.Attributes["IP"] == null ? string.Empty : node.Attributes["IP"].Value;
            _Port = node.Attributes["Port"] == null ? Config.ServerPort : Convert.ToInt32(node.Attributes["Port"].Value);
            _ApPort = node.Attributes["ApPort"] == null ? Config.ServerPort : Convert.ToInt32(node.Attributes["ApPort"].Value);
            _LoginDataBase = node.Attributes["LoginDataBase"] == null ? string.Empty : node.Attributes["LoginDataBase"].Value;
            _LoginSolution = node.Attributes["LoginSolution"] == null ? string.Empty : node.Attributes["LoginSolution"].Value;
            _Language = node.Attributes["Language"] == null ? LanguageType.ENG : (LanguageType)Enum.Parse(typeof(LanguageType), node.Attributes["Language"].Value);
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
        }

        private string _Text;

        public string Text
        {
            get { return _Text; }
        }

        private string _IP;

        public string IP
        {
            get { return _IP; }
        }

        private int _Port;

        public int Port
        {
            get { return _Port; }
        }

        private int _ApPort;
        public int ApPort
        {
            get { return _ApPort; }
        }

        private string _LoginDataBase;

        public string LoginDataBase
        {
            get { return _LoginDataBase; }
        }

        private string _LoginSolution;

        public string LoginSolution
        {
            get { return _LoginSolution; }
        }

        private LanguageType _Language;

        public LanguageType Language
        {
            get { return _Language; }
        }
	
        internal XmlNode Save(XmlNode nodedocumemnt)
        {
            XmlNode node = nodedocumemnt.OwnerDocument.CreateElement("Solution");
            AddAttribute(node, "Name", Name);
            AddAttribute(node, "Text", Text);
            AddAttribute(node, "IP", IP);
            AddAttribute(node, "Port", Port);
            AddAttribute(node, "ApPort", ApPort);
            AddAttribute(node, "LoginDataBase", LoginDataBase);
            AddAttribute(node, "LoginSolution", LoginSolution);
            AddAttribute(node, "Language", Language);
            return node;
        }

        private void AddAttribute(XmlNode node, string name, object value)
        {
            XmlAttribute attribute = node.OwnerDocument.CreateAttribute(name);
            attribute.Value = (value == null) ? string.Empty : value.ToString();
            node.Attributes.Append(attribute);
        }

        public enum LanguageType
        {
            ENG,
            TRA,
            SIM,
            HKG,
            JPN,
            LAN1,
            LAN2,
            LAN3
        }
    }

    /// <summary>
    /// Solution定义集合
    /// </summary>
    public class SolutionCollection
    {
        public SolutionCollection()
        {
            List = new ArrayList();
        }

        public SolutionCollection(XmlDocument xml, String Path)
            : this()
        {
            XmlNodeList nodelist = xml.SelectNodes(Path);
            for (int i = 0; i < nodelist.Count; i++)
            {
                Add(new Solution(nodelist[i]));
            }
        }

        public Solution this[int index]
        {
            get { return List[index] as Solution; }
            set { List[index] = value; }
        }

        private ArrayList List;

        public void Add(Solution sol)
        {
            List.Add(sol);
        }

        public int Count
        {
            get { return List.Count; }
        }

        public XmlDocument Save()
        {
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateElement("Solutions"));
            for (int i = 0; i < Count; i++)
            {
                xml.DocumentElement.AppendChild(this[i].Save(xml.DocumentElement));
            }
            return xml;
        }
    }
}
