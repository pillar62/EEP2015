using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.ComponentModel;
using System.Xml;
using System.Data;
using System.IO;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing;

namespace Srvtools
{
    public class WebGridDropDown : DropDownList, ICallbackEventHandler, IGetValues
    {
        public WebGridDropDown()
        { 
        }

        private string callBackValue = "";

        [Category("InfoLight")]
        [DefaultValue(true)]
        public bool MultiEditPostBack
        {
            get
            {
                object obj = this.ViewState["MultiEditPostBack"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["MultiEditPostBack"] = value;
            }
        }

        [Category("InfoLight")]
        [Description("Indicates which data field has bound to the control")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string DataField
        {
            get
            {
                object obj = this.ViewState["DataField"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["DataField"] = value;
            }
        }

        [Browsable(false)]
        public string KeyValues
        {
            get
            {
                object obj = this.ViewState["KeyValues"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["KeyValues"] = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "datafield", true) == 0)//IgnoreCase
            {
                IDesignerHost host = (IDesignerHost)this.Site.GetService(typeof(IDesignerHost));
                ControlDesigner designer = (ControlDesigner)host.GetDesigner(this);
                if (designer.DataBindings["SelectedValue"] != null)
                {
                    string content = designer.DataBindings["SelectedValue"].Expression;
                    string[] contentPart = content.Split('"');
                    values.Add(contentPart[1]);
                }
            }
            if (values.Count > 0)
            {
                int i = values.Count;
                retList = new string[i];
                for (int j = 0; j < i; j++)
                {
                    retList[j] = values[j];
                }
            }
            return retList;
        }

        public virtual void RaiseCallbackEvent(string eventArgument)
        {
            string phyPath = this.Page.Request.PhysicalPath;
            //phyPath = phyPath.Substring(0, phyPath.LastIndexOf(".aspx"));
            //string xmlPath = phyPath + ".xml";
            string xmlPath = CliUtils.ReplaceFileName(phyPath, ".aspx", ".xml");
            FileStream stream;
            if (!File.Exists(xmlPath))
            {
                stream = new FileStream(xmlPath, FileMode.Create);
                try
                {
                    XmlTextWriter xmlWriter = new XmlTextWriter(stream, new System.Text.ASCIIEncoding());
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.WriteStartElement("InfoLight");
                    xmlWriter.WriteEndElement();
                    xmlWriter.Close();
                }
                finally
                {
                    stream.Close();
                }
            }
            stream = new FileStream(xmlPath, FileMode.Open);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);
            XmlNode nRoot = xmlDoc.SelectSingleNode("InfoLight");
            //如果不存在Root，则创建Root
            if (nRoot == null)
            {
                nRoot = CreateRootNode(xmlDoc);
                xmlDoc.AppendChild(nRoot);
            }
            XmlNode nWebGridDropDowns = nRoot.SelectSingleNode("WebGridDropDowns");
            //如果不存在WebGridDropDowns的键,则创建该键
            if (nWebGridDropDowns == null)
            {
                nWebGridDropDowns = CreateWebGridDropDownsNode(xmlDoc);
                nRoot.AppendChild(nWebGridDropDowns);
            }
            XmlNode nWebGridView = nWebGridDropDowns.SelectSingleNode("WebGridView[@ID='" + this.GetContainerGridView().ID + "']");
            //如果存在该WebGridView的键
            if (nWebGridView != null)
            {
                XmlNode nRow = nWebGridView.SelectSingleNode("Row[@KeyValues='" + this.KeyValues + "']");
                //如果存在该Row的键
                if (nRow != null)
                {
                    XmlNode nDropDown = nRow.SelectSingleNode("DropDown[@FieldName='" + this.DataField + "']");
                    //如果存在该TextBox的键
                    if (nDropDown != null)
                    {
                        string OwnerUser = nDropDown.Attributes["User"].Value;
                        if (OwnerUser == CliUtils.fLoginUser)
                        {
                            nDropDown.InnerText = eventArgument;
                        }
                        else
                        {
                            callBackValue = "some one is editing this record...";
                        }
                    }
                    // 否则创建该键
                    else
                    {
                        nDropDown = CreateDropDownNode(xmlDoc, eventArgument);
                        nRow.AppendChild(nDropDown);
                    }
                }
                //否则创建该键
                else
                {
                    nRow = CreateRowNode(xmlDoc, eventArgument);
                    nWebGridView.AppendChild(nRow);
                }
            }
            //否则创建该键
            else
            {
                nWebGridView = CreateWebGridViewNode(xmlDoc, eventArgument);
                nWebGridDropDowns.AppendChild(nWebGridView);
            }
            stream.Close();
            xmlDoc.Save(xmlPath);
        }

        public virtual string GetCallbackResult()
        {
            return callBackValue;
        }

        private XmlNode CreateRootNode(XmlDocument xmlDoc)
        {
            XmlNode nRoot = xmlDoc.CreateElement("InfoLight");
            return nRoot;
        }

        private XmlNode CreateWebGridDropDownsNode(XmlDocument xmlDoc)
        {
            XmlNode nWebGridDropDowns = xmlDoc.CreateElement("WebGridDropDowns");
            return nWebGridDropDowns;
        }

        private XmlNode CreateWebGridViewNode(XmlDocument xmlDoc, string Value)
        {
            XmlNode nWebGridViewNode = xmlDoc.CreateElement("WebGridView");

            XmlAttribute aWebGridViewID = xmlDoc.CreateAttribute("ID");
            aWebGridViewID.Value = this.GetContainerGridView().ID;
            nWebGridViewNode.Attributes.Append(aWebGridViewID);

            XmlNode nRow = CreateRowNode(xmlDoc, Value);
            nWebGridViewNode.AppendChild(nRow);

            return nWebGridViewNode;
        }

        private XmlNode CreateRowNode(XmlDocument xmlDoc, string Value)
        {
            XmlNode nRowNode = xmlDoc.CreateElement("Row");

            XmlAttribute aKeyValues = xmlDoc.CreateAttribute("KeyValues");
            aKeyValues.Value = this.KeyValues;
            nRowNode.Attributes.Append(aKeyValues);

            XmlNode nDropDown = CreateDropDownNode(xmlDoc, Value);
            nRowNode.AppendChild(nDropDown);
            return nRowNode;
        }

        private XmlNode CreateDropDownNode(XmlDocument xmlDoc, string Value)
        {
            XmlNode nDropDownNode = xmlDoc.CreateElement("DropDown");

            XmlAttribute aField = xmlDoc.CreateAttribute("FieldName");
            aField.Value = this.DataField;
            nDropDownNode.Attributes.Append(aField);

            XmlAttribute aUser = xmlDoc.CreateAttribute("User");
            aUser.Value = CliUtils.fLoginUser;
            nDropDownNode.Attributes.Append(aUser);

            nDropDownNode.InnerText = Value;
            return nDropDownNode;
        }

        private WebGridView GetContainerGridView()
        {
            Control ctrl = this.NamingContainer;
            if (ctrl != null && ctrl is GridViewRow)
            {
                GridViewRow row = (GridViewRow)ctrl;
                return (WebGridView)row.NamingContainer;
            }
            return null;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode && this.MultiEditPostBack)
            {
                ClientScriptManager csm = this.Page.ClientScript;
                string onSelectCallBack =
                    "function ReceiveServerData(arg)" +
                    "{" +
                        "if(arg != '')" +
                        "{" +
                            "alert(arg);" +
                        "}" +
                    "}";
                string onChangeScript = csm.GetCallbackEventReference(this, "this.value", onSelectCallBack, "", true);
                writer.AddAttribute("onchange", onChangeScript, true);

            }
            base.Render(writer);
        }

        public override void DataBind()
        {
            try
            {
                base.DataBind();
            }
            catch
            {
            }
            if (!this.DesignMode)
            {
                object obj = this.GetDataItem();
                WebDataSource wds = this.GetContainerDataSource();
                if (obj != null && obj is DataRowView)
                {
                    DataRowView row = (DataRowView)obj;
                    foreach (DataColumn dc in wds.PrimaryKey)
                    {
                        this.KeyValues += row[dc.ColumnName].ToString() + ";";
                    }
                    if (this.KeyValues != "")
                    {
                        this.KeyValues = this.KeyValues.Substring(0, this.KeyValues.LastIndexOf(";"));
                    }
                }
            }
        }

        private object GetDataItem()
        {
            object obj = null;
            Control ctrl = this.NamingContainer;
            if (ctrl != null && ctrl is GridViewRow)
            {
                GridViewRow row = (GridViewRow)ctrl;
                obj = row.DataItem;
            }
            return obj;
        }

        private WebDataSource GetContainerDataSource()
        {
            Control ctrl = this.NamingContainer;
            if (ctrl != null && ctrl is GridViewRow)
            {
                GridViewRow row = (GridViewRow)ctrl;
                WebGridView gdView = (WebGridView)row.NamingContainer;
                object obj = this.GetObjByID(gdView.DataSourceID);
                if (obj != null && obj is WebDataSource)
                {
                    return (WebDataSource)obj;
                }
            }
            return null;
        }

        private Control GetAllCtrls(string strid, Control ct)
        {
            if (ct.ID == strid)
            {
                return ct;
            }
            else
            {
                if (ct.HasControls())
                {
                    foreach (Control ctchild in ct.Controls)
                    {
                        Control ctrtn = GetAllCtrls(strid, ctchild);
                        if (ctrtn != null)
                        {
                            return ctrtn;
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }
        public object GetObjByID(string ObjID)
        {
            if (this.Site != null)
            {
                return GetAllCtrls(ObjID, this.Page);
            }
            else
            {
                if (this.Page.Form != null)
                    return GetAllCtrls(ObjID, this.Page.Form);
                else
                    return GetAllCtrls(ObjID, this.Page);
            }
        }
    }
}
