using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EFBase
{
    /// <summary>
    /// Provider of message
    /// </summary>
    public class MessageProvider
    {
        const string MESSAGE_FILE = "sysmsg.xml";

        public MessageProvider(string currentDirectory, string locale)
        {
            _locale = locale;
            var path = string.Format(@"{0}\{1}", currentDirectory, MESSAGE_FILE);
            if (System.IO.File.Exists(path))
            {
                _document = new XmlDocument();
                _document.Load(path);
            }
        }

        private string _locale;
        public string Locale
        {
            get
            {
                return _locale;
            }
        }

        public string Language
        {
            get
            {
                switch (Locale.ToLower())
                {
                    case "zh-hans-cn":
                    case "zh-cn": return "SIM";
                    case "zh-hk":
                    case "zh-hant-tw":
                    case "zh-tw": return "TRA";
                    default: return "ENG";
                }
            }
        }

        private XmlDocument _document;
        public XmlDocument Document
        {
            get
            {
                return _document;
            }
        }

        public string this[string key]
        {
            get
            {
                if (Document == null)
                {
                    return string.Empty;
                }
                else
                {
                    var languageNode = Document.DocumentElement.SelectSingleNode(string.Format("{0}/{1}", key, Language));
                    return languageNode != null ? languageNode.InnerText : string.Empty;
                }
            }
        }
    }

    public class PlaceholderProvider
    {
        const string PLACEHOLDER_FILE = "syssdmsg.xml";

        public PlaceholderProvider(string currentDirectory, string locale)
        {
            _locale = locale;
            var path = string.Format(@"{0}\{1}", currentDirectory, PLACEHOLDER_FILE);
            if (System.IO.File.Exists(path))
            {
                _document = new XmlDocument();
                _document.Load(path);
            }
        }

        private string _locale;
        public string Locale
        {
            get
            {
                return _locale;
            }
        }

        public string Language
        {
            get
            {
                switch (Locale.ToLower())
                {
                    case "zh-hans-cn":
                    case "zh-cn": return "SIM";
                    case "zh-hk":
                    case "zh-hant-tw":
                    case "zh-tw": return "TRA";
                    default: return "ENG";
                }
            }
        }

        private XmlDocument _document;
        public XmlDocument Document
        {
            get
            {
                return _document;
            }
        }

        public string this[string key]
        {
            get
            {
                if (Document == null)
                {
                    return string.Empty;
                }
                else
                {
                    var languageNode = Document.DocumentElement.SelectSingleNode(string.Format("{0}/{1}", key, Language));
                    return languageNode != null ? languageNode.InnerText : string.Empty;
                }
            }
        }
    }
}
