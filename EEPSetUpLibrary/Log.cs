using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Collections;
using System.Xml;
using System.Windows.Forms;

namespace EEPSetUpLibrary
{
    //记录日志类,记录更新过程中的事件及异常
    public class Log
    {
        /// <summary>
        /// 日志文件
        /// </summary>
        public static readonly string LogFileName = Application.StartupPath + "\\UpdateUserLog.xml";

        /// <summary>
        /// 写入日志内容
        /// </summary>
        /// <param name="message">要写入的内容</param>
        public static void Write(IPEndPoint ip, string description)
        {
            XmlDocument xml = new XmlDocument();
            if (!File.Exists(LogFileName))
            {
                xml.AppendChild(xml.CreateElement("History"));
            }
            else
            {
                try
                {
                    xml.Load(LogFileName);
                }
                catch
                {
                    xml.AppendChild(xml.CreateElement("History"));
                }
            }
            XmlNode node = xml.CreateElement("Log");
            node.Attributes.Append(CreateAttribute(xml, "User",ip));
            node.Attributes.Append(CreateAttribute(xml, "DateTime", DateTime.Now));
            node.Attributes.Append(CreateAttribute(xml, "Description", description));
            xml.DocumentElement.PrependChild(node);
            xml.Save(LogFileName);
        }

        private static XmlAttribute CreateAttribute(XmlDocument xml, string attributename, object value)
        {
            XmlAttribute att = xml.CreateAttribute(attributename);
            att.Value = value.ToString();
            return att;
        }//为Xml的节点创建属性

        /// <summary>
        /// 从日志中读出记录
        /// </summary>
        /// <returns>存放记录的集合</returns>
        public static XmlNodeList Read()
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(LogFileName);
                XmlNodeList nodelist = xml.SelectNodes("History/Log");
                return nodelist;
            }
            catch
            {
                return null;
            }
        }
    }
}
