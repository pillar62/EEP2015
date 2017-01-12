using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Srvtools;
using System.Collections;
using System.Xml;
using System.IO;

#region License
// Copyright InfoLight 2005
// Created by yangdong
#endregion

namespace EEPNetServer
{
    public partial class frmSrvMan : Form
    {
        private String _PackageFileFullName;
        private String _PackageFileName;
        private XmlDocument _SrvXml;

        public frmSrvMan()
        {
            InitializeComponent();
        }

        public String PackageFileFullName
        {
            get { return _PackageFileFullName; }
            set { _PackageFileFullName = value; }
        }

        public String PackageFileName
        {
            get { return _PackageFileName; }
            set { _PackageFileName = value; }
        }

        private void frmSrvMan_Load(object sender, EventArgs e)
        {
            // Get the package's servicename.
            List<String> list = LoadAllServiceName();
            lbxServices.DataSource = list;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lbxServices.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a servciename", "Error");
                return;
            }

            _SrvXml = new XmlDocument();
            String srvName = SystemFile.ServiceFile;

            if (!File.Exists(srvName))
            {
                FileStream aFileStream = new FileStream(srvName, FileMode.Create);
                try
                {
                    XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
                    w.Formatting = Formatting.Indented;
                    w.WriteStartElement("InfolightSerivces");
                    w.WriteEndElement();
                    w.Close();
                }
                finally
                {
                    aFileStream.Close();
                }
            }

        
            _SrvXml.Load(srvName);

            XmlNode pkgNode = _SrvXml.DocumentElement.SelectSingleNode("PackageFile[@Name = '" + _PackageFileName + "']");

            String serviceName;
            foreach (Object obj in lbxServices.SelectedItems)
            {
                serviceName = obj.ToString();

                Int32 srvCount = pkgNode.SelectNodes("Service[@Name = '" + serviceName + "']").Count;

                if (srvCount == 0)  // Create the servie node
                {
                    #region Create service node

                    XmlElement srvElement = _SrvXml.CreateElement("Service");

                    XmlAttribute nameAttribute = _SrvXml.CreateAttribute("Name");
                    nameAttribute.Value = serviceName;

                    srvElement.Attributes.Append(nameAttribute);

                    pkgNode.AppendChild(srvElement);

                    #endregion
                }
                else if (srvCount > 1)  // Error
                {
                }
            }

            _SrvXml.Save(srvName);

            this.Close();
            
        }

        private List<String> LoadAllServiceName()
        {
            Assembly a = Assembly.LoadFrom(_PackageFileFullName);
            string ModuleName = Path.ChangeExtension(Path.GetFileName(_PackageFileFullName), ""); 
            Type myType = a.GetType(ModuleName + "Component");

            List<String> list = new List<string>();
            Object obj = Activator.CreateInstance(myType);
            ServiceManager serviceMan = (ServiceManager)((IDataModule)obj).GetIntfObject(typeof(IServiceManager));
            if (null != serviceMan) 
            {
                //List<Service> serviceCol = serviceMan.ServiceCollection;
                ServiceCollection serviceCol = serviceMan.ServiceCollection;

                if (serviceCol.Count > 0)
                {
                    foreach (Service service in serviceCol)
                    {
                        if (service.NonLogin == true)
                        {
                            list.Add(service.ServiceName);
                         }
                    }
                }
            }

            return list;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}