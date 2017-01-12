using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;

namespace Infolight.EasilyReportTools.DataCenter
{
    public class XmlConverter
    {
        private static List<byte> BufferXml = new List<byte>();

        private static byte[] BufferObject;

        public static object ConvertTo(byte[] buffer)
        {
            int length = BitConverter.ToInt32(buffer, 0);
            byte[] xmlbuffer = new byte[length];
            Buffer.BlockCopy(buffer, 4, xmlbuffer, 0, xmlbuffer.Length); //取出xml部分
            if (buffer.Length - length - 4 > 0)
            {
                BufferObject = new byte[buffer.Length - length - 4];//取出image部分
                Buffer.BlockCopy(buffer, length + 4, BufferObject, 0, BufferObject.Length);
            }
            MemoryStream ms = new MemoryStream(xmlbuffer);
            XmlDocument xml = new XmlDocument();
            xml.Load(ms);
            return ConvertToXml(xml);
        }

        private static object ConvertToXml(XmlDocument xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }
            return CreateItem(xml.DocumentElement, null);
        }

        private static object CreateItem(XmlNode node, object parent)
        {
            string strtype = node.Attributes["Type"].Value;
            Type type = Type.GetType(strtype);
            object value = null;
            if (type == null || !type.Namespace.StartsWith(typeof(EasilyReport).Namespace) )
            {
                int postion = Convert.ToInt32(node.InnerText);
                int length = BitConverter.ToInt32(BufferObject, postion);
                byte[] buff = new byte[length];
                Buffer.BlockCopy(BufferObject, postion + 4, buff, 0, buff.Length);
                MemoryStream ms = new MemoryStream(buff);
                BinaryFormatter formatter = new BinaryFormatter();
                value = formatter.Deserialize(ms);
            }
            else if (type.IsEnum)
            {
                value = Enum.Parse(type, node.InnerText);
            }
            //else if (type.IsValueType || type == typeof(string))
            //{
            //    value = Convert.ChangeType(node.InnerText, type);
            //}
            else
            {
                object obj = Activator.CreateInstance(type);
                foreach (XmlNode nodechild in node.ChildNodes)
                {
                    CreateItem(nodechild, obj);
                }
                value = obj;
            }
            if (value != null && parent != null)
            {
                string name = node.Name;
                PropertyInfo property = parent.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
                if (property != null)
                {
                    if ((property.PropertyType.GetInterface("IList") != null && property.PropertyType.GetInterface("ICollection") != null))
                    {
                        IList list = (IList)property.GetValue(parent, null);
                        for (int i = 0; i < ((ICollection)value).Count; i++)
                        {
                            list.Add(((IList)value)[i]);
                        }
                    }   
                    else
                    {
                        property.SetValue(parent, value, null);
                    }
                }
                else if (parent.GetType().GetInterface("IList") != null)
                {
                    ((IList)parent).Add(value);
                }
            }
            return value;
        }

        public static byte[] ConvertFrom(object obj)
        {
            BufferXml.Clear();
            XmlDocument xml = ConvertFromXml(obj);
            MemoryStream ms = new MemoryStream();
            StreamWriter writer = new StreamWriter(ms, new UTF8Encoding(true));
            xml.Save(writer);
            writer.Flush();
            int length = (int)ms.Length;
            writer.Close();
            List<byte> buffer = new List<byte>();
            //加入xml部分
            buffer.AddRange(BitConverter.GetBytes(length));
            buffer.AddRange(ms.ToArray());
            //加入image部分
            buffer.AddRange(BufferXml);
            BufferXml.Clear();
            return buffer.ToArray();
        }

        private static XmlDocument ConvertFromXml(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            XmlDocument xml = new XmlDocument();
            CreateItemNode(obj, null, xml);
            return xml;
        }

        private static void CreateItemNode(object item, string name, XmlNode parent)
        {
            try
            {
                Type type = item.GetType();
                if(string.IsNullOrEmpty(name))
                {
                    name = type.Name;
                }
                XmlNode node = parent is XmlDocument? (parent as XmlDocument).CreateElement(name): parent.OwnerDocument.CreateElement(name);
                AppendAttribute(node, "Type", type.FullName);
                //if (type.IsValueType || type == typeof(string))
                //{
                //    node.InnerText = item.ToString();
                //}
                if (!type.Namespace.StartsWith(typeof(EasilyReport).Namespace))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream ms = new MemoryStream();
                    formatter.Serialize(ms, item);
                    int length = (int)ms.Length;
                    node.InnerText = BufferXml.Count.ToString();
                    BufferXml.AddRange(BitConverter.GetBytes(length));
                    BufferXml.AddRange(ms.ToArray());
                }
                else if (type.IsEnum)
                {
                    node.InnerText = item.ToString();
                }
                else
                {
                    if (type.GetInterface("IList") != null && type.GetInterface("ICollection") != null) //collection
                    {
                        int count = ((ICollection)item).Count;
                        IList list = (IList)item;
                        for (int i = 0; i < count; i++)
                        {
                            CreateItemNode(list[i], null, node);
                        }
                    }
                    PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    foreach (PropertyInfo pi in properties)
                    {
                        if (pi.GetIndexParameters().Length == 0)
                        {
                            if ((pi.CanRead && pi.CanWrite) || (pi.PropertyType.GetInterface("IList") != null && pi.PropertyType.GetInterface("ICollection") != null))
                            {
                                object value = pi.GetValue(item, null);
                                if (value != null)
                                {
                                    CreateItemNode(value, pi.Name, node);
                                }
                            }
                        }
                    }
                }
                parent.AppendChild(node);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static void AppendAttribute(XmlNode node, string name, string value)
        {
            XmlAttribute attribute = node.OwnerDocument.CreateAttribute(name);
            attribute.Value = value;
            node.Attributes.Append(attribute);
        }
    }
}