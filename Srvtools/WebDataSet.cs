using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Srvtools
{
    // [ToolboxData("<{0}:WebDataSet runat=server></{0}:WebDataSet>")]
    [Designer(typeof(WebDataSetDesigner), typeof(IDesigner))]
    public class WebDataSet : InfoDataSet
    {
        private string _guid;
        public WebDataSet() : base(false)
        {
        }

        public WebDataSet(bool isDesignMode)
            : base(isDesignMode)
        {
        }

        [Browsable(false)]
        public string Guid
        {
            get
            {
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }

        private static XmlNode GetWebDataSetNode(string webDataSetID, string pagePath)
        {
            ResXResourceReader reader = new ResXResourceReader(string.Format("{0}.vi-VN.resx", pagePath));
            try
            {
                IDictionaryEnumerator enumerator = reader.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Key.ToString() == "WebDataSets")
                    {
                        string sXml = (string)enumerator.Value;
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(sXml);
                        return xmlDoc.SelectSingleNode(string.Format("WebDataSets/WebDataSet[@Name='{0}']", webDataSetID));
                    }
                }
            }
            finally 
            {
                reader.Close();
            }
            return null;
        }

        private static XmlNodeList GetWebDataSetNodes(string pagePath)
        {
            ResXResourceReader reader = new ResXResourceReader(string.Format("{0}.vi-VN.resx", pagePath));
            try
            {
                IDictionaryEnumerator enumerator = reader.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Key.ToString() == "WebDataSets")
                    {
                        string sXml = (string)enumerator.Value;
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(sXml);
                        return xmlDoc.SelectNodes(string.Format("WebDataSets/WebDataSet"));
                    }
                }
            }
            finally 
            {
                reader.Close();
            }
            return null;
        }

        public static string[] GetAvailableDataSetID()
        {
            List<string> list = new List<string>();
            try
            {
                XmlNodeList nodeList = GetWebDataSetNodes(EditionDifference.ActiveDocumentFullName());
                if (nodeList != null)
                {
                    foreach (XmlNode node in nodeList)
                    {
                        if (node.Attributes["Name"] != null)
                        {
                            list.Add(node.Attributes["Name"].Value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return list.ToArray();
        }

        //for design
        public static string GetRemoteName(string webDataSetID)
        {
            try
            {
                XmlNode nodeWebDataSet = GetWebDataSetNode(webDataSetID, EditionDifference.ActiveDocumentFullName());
                if (nodeWebDataSet != null)
                {
                    XmlNode nodeProperty = nodeWebDataSet.SelectSingleNode("RemoteName");
                    if (nodeProperty != null && nodeProperty.InnerText.Length > 0)
                    {
                        return nodeProperty.InnerText;
                    }
                } 
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return string.Empty;
        }

        // for design
        public static WebDataSet CreateWebDataSet(string webDataSetID)
        {
            try
            {
                return CreateWebDataSet(webDataSetID, EditionDifference.ActiveDocumentFullName());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        //for design only
        private static WebDataSet CreateWebDataSet(string webDataSetID, string pagePath)
        {
             WebDataSet wds = new WebDataSet(true);
             XmlNode nodeWebDataSet = GetWebDataSetNode(webDataSetID, pagePath);

             if (nodeWebDataSet != null)
             {
                 XmlNode nodeProperty = nodeWebDataSet.SelectSingleNode("RemoteName");
                 if (nodeProperty != null && nodeProperty.InnerText.Length > 0)
                 {
                     wds.RemoteName = nodeProperty.InnerText;
                 }
                 nodeProperty = nodeWebDataSet.SelectSingleNode("PacketRecords");
                 if (nodeProperty != null && nodeProperty.InnerText.Length > 0)
                 {
                     wds.PacketRecords = Convert.ToInt32(nodeProperty.InnerText);
                 }
                 nodeProperty = nodeWebDataSet.SelectSingleNode("ServerModify");
                 if (nodeProperty != null && nodeProperty.InnerText.Length > 0)
                 {
                     wds.ServerModify = Convert.ToBoolean(nodeProperty.InnerText);
                 }
                 wds.WhereStr = "1=0";
                 wds.Active = true;
             }
            return wds;
        }
        

        /*public override bool ApplyUpdates(bool NeedToValidate)
        {
            if (this.RealDataSet.HasChanges() && NeedToValidate)
            {
                DataSet ds = this.RealDataSet.GetChanges(DataRowState.Added | DataRowState.Modified);
                
            }
            return base.ApplyUpdates(NeedToValidate);
        }*/
    }

    public class WebDataSetDesigner : ComponentDesigner
    {
        public WebDataSetDesigner()
        {
            DesignerVerb saveVerb = new DesignerVerb("Save", new EventHandler(OnSave));
            this.Verbs.Add(saveVerb);
            DesignerVerb createVerb = new DesignerVerb("Save To Report", new EventHandler(OnCreate));
            this.Verbs.Add(createVerb);
            DesignerVerb createXSDVerb = new DesignerVerb("Create XSD File", new EventHandler(OnCreateXSD));
            this.Verbs.Add(createXSDVerb);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            WebDataSet webDs = (WebDataSet)this.Component;
            webDs.Guid = (Guid.NewGuid()).ToString();
        }

        public void OnCreate(object sender, EventArgs e)
        {
            OnSave(sender, e);
            if (this.Component != null)
            {
                InfoDataSet infoDataSet = (InfoDataSet)this.Component;
                if (infoDataSet.RealDataSet == null)
                {
                    infoDataSet.Active = true;
                }
                String s = EEPRegistry.Server;
                string xmlfile = string.Format("{0}\\EEPNetReport\\", Directory.GetParent(s));
                string str = infoDataSet.RemoteName;
                xmlfile = xmlfile + str.Substring(0, str.IndexOf('.')) + "_" + str.Substring(str.IndexOf('.') + 1) + ".xml";
                infoDataSet.RealDataSet.WriteXmlSchema(xmlfile);
            }
        }

        public virtual void OnCreateXSD(object sender, EventArgs e)
        {
            if (this.Component != null)
            {
                InfoDataSet infoDataSet = (InfoDataSet)this.Component;
                if (infoDataSet.RealDataSet == null)
                {
                    infoDataSet.Active = true;
                }

                string filePath = EditionDifference.ActiveDocumentPath();
                bool CreateFileSucess = true;
                string fileName = "";
                try
                {
                    fileName = filePath + infoDataSet.Site.Name + ".xsd";
                    infoDataSet.RealDataSet.WriteXmlSchema(fileName);
                }
                catch
                {
                    CreateFileSucess = false;
                    MessageBox.Show("Failed to create xsd file!");
                }
                finally
                {
                    if (CreateFileSucess && File.Exists(fileName))
                    {
                        EditionDifference.AddProjectItem(fileName);
                    }
                }
            }
        }

        public void OnSave(object sender, EventArgs e)
        {
            string keyName = "WebDataSets";
            IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));

            CultureInfo culture = new CultureInfo("vi-VN");
            IResourceService resourceService = (IResourceService)designerHost.GetService(typeof(IResourceService));
            IResourceWriter resourceWriter = resourceService.GetResourceWriter(culture);
            IResourceReader resourceReader = resourceService.GetResourceReader(culture);
            IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();

            XmlDocument xmlDoc = new XmlDocument();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString() == keyName)
                {
                    string sXml = (string)enumerator.Value;
                    xmlDoc.LoadXml(sXml);
                    break;
                }
            }
            resourceReader.Close();

            // ---------------------------------------------------------------------
            XmlNode nWDSs = xmlDoc.SelectSingleNode("WebDataSets");
            if (nWDSs == null)
            {
                nWDSs = CreateWDSsNode(xmlDoc);
                xmlDoc.AppendChild(nWDSs);
            }

            // ---------------------------------------------------------------------
            // 删除已经不存在的WebDataSet。
            RemoveNotExisted(nWDSs);

            // ---------------------------------------------------------------------
            string name = this.Component.Site.Name;
            XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSet[@Name='" + name + "']");
            if (nWDS != null)
                nWDSs.RemoveChild(nWDS);

            nWDS = CreateWDSNode(xmlDoc);
            nWDSs.AppendChild(nWDS);

            // ---------------------------------------------------------------------
            resourceWriter.AddResource(keyName, xmlDoc.InnerXml);
            resourceWriter.Close();


            // 保存当前的文档
            EnvDTE.Document doc = EditionDifference.ActiveDocument();
            doc.Save(doc.FullName);
            doc.Saved = true;
        }

        private XmlNode CreateWDSsNode(XmlDocument xmlDoc)
        {
            XmlNode nWebDataSets = xmlDoc.CreateElement("WebDataSets");
            return nWebDataSets;
        }

        private XmlNode CreateWDSNode(XmlDocument xmlDoc)
        {
            XmlElement nWDSNode = xmlDoc.CreateElement("WebDataSet");

            XmlAttribute aWBSName = xmlDoc.CreateAttribute("Name");
            aWBSName.Value = this.Component.Site.Name;
            nWDSNode.Attributes.Append(aWBSName);

            // ---------------------------------------------------------------
            WebDataSet wds = (WebDataSet)this.Component;

            XmlNode nActive = xmlDoc.CreateElement("Active");
            nActive.InnerText = wds.Active.ToString();
            nWDSNode.AppendChild(nActive);

            XmlNode nPacketRecords = xmlDoc.CreateElement("PacketRecords");
            nPacketRecords.InnerText = wds.PacketRecords.ToString();
            nWDSNode.AppendChild(nPacketRecords);

            XmlNode nRemoteName = xmlDoc.CreateElement("RemoteName");
            nRemoteName.InnerText = wds.RemoteName;
            nWDSNode.AppendChild(nRemoteName);

            XmlNode nServerModify = xmlDoc.CreateElement("ServerModify");
            nServerModify.InnerText = wds.ServerModify.ToString();
            nWDSNode.AppendChild(nServerModify);

            XmlNode nAlwaysClose = xmlDoc.CreateElement("AlwaysClose");
            nAlwaysClose.InnerText = wds.AlwaysClose.ToString();
            nWDSNode.AppendChild(nAlwaysClose);

            XmlNode nState = xmlDoc.CreateElement("State");
            nState.InnerText = "true";
            nWDSNode.AppendChild(nState);

            return nWDSNode;
        }

        private void RemoveNotExisted(XmlNode node)
        {
            List<string> lists = new List<string>();
            WebDataSet webDs = (WebDataSet)this.Component;
            ComponentCollection comps = webDs.Site.Container.Components;
            foreach (object comp in comps)
            {
                if (comp is WebDataSet)
                {
                    lists.Add(((Component)comp).Site.Name);
                }
            }

            foreach (XmlNode nod in node.ChildNodes)
            {
                string name = nod.Attributes["Name"].Value;
                if (lists.IndexOf(name) < 0)
                {
                    node.RemoveChild(nod);
                }
            }
        }
    }
}