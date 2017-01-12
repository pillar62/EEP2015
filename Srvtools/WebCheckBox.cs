using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.Reflection;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Srvtools
{
    [ToolboxBitmap(typeof(InfoTranslate), "Resources.WebCheckBox.png")]
    public class WebCheckBox : CheckBox, ICallbackEventHandler
    {
        public WebCheckBox()
        {
            
        }

        [Category("InfoLight"),
        DefaultValue(true)]
        public bool MultiCheckPostBack
        {
            get
            {
                object obj = this.ViewState["MultiCheckPostBack"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["MultiCheckPostBack"] = value;
            }
        }

        [Bindable(true)]
        [Browsable(false)]
        public object CheckBinding
        {
            get
            {
                return this.ViewState["CheckBinding"];
            }
            set
            {
                this.ViewState["CheckBinding"] = value;
            }
        }

        private CheckBindingType bindingType;

        public CheckBindingType BindingType
        {
            get { return bindingType; }
            set { bindingType = value; }
        }

        private string callBackValue = "";

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
            XmlNode nWebCheckBoxes = nRoot.SelectSingleNode("WebCheckBoxes");
            //如果不存在WebCheckBoxes的键,则创建该键
            if (nWebCheckBoxes == null)
            {
                nWebCheckBoxes = CreateWebCheckBoxesNode(xmlDoc);
                nRoot.AppendChild(nWebCheckBoxes);
            }
            XmlNode nWebGridView = nWebCheckBoxes.SelectSingleNode("WebGridView[@ID='" + this.GetContainerGridView().ID + "']");
            //如果存在该WebGridView的键
            if (nWebGridView != null)
            {
                XmlNode nCheckBox = nWebGridView.SelectSingleNode("CheckBox[@KeyValues='" + this.KeyValues + "']");
                //如果存在该CheckBox的键,则设定其CheckState的值
                if (nCheckBox != null)
                {
                    string OwnerUser = nCheckBox.Attributes["User"].Value;
                    if (OwnerUser == CliUtils.fLoginUser)
                    {
                        nCheckBox.InnerText = eventArgument;
                    }
                    else
                    {
                        callBackValue = "some one is editing this record...";
                    }
                }
                //否则创建该键
                else
                {
                    nCheckBox = CreateCheckBoxNode(xmlDoc, eventArgument);
                    nWebGridView.AppendChild(nCheckBox);
                }
            }
            //否则创建该键
            else
            {
                nWebGridView = CreateWebGridViewNode(xmlDoc, eventArgument);
                nWebCheckBoxes.AppendChild(nWebGridView);
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

        private XmlNode CreateWebCheckBoxesNode(XmlDocument xmlDoc)
        {
            XmlNode nWebCheckBoxes = xmlDoc.CreateElement("WebCheckBoxes");
            return nWebCheckBoxes;
        }

        private XmlNode CreateWebGridViewNode(XmlDocument xmlDoc, string checkState)
        {
            XmlElement nWebGridViewNode = xmlDoc.CreateElement("WebGridView");

            XmlAttribute aWebGridViewID = xmlDoc.CreateAttribute("ID");
            aWebGridViewID.Value = this.GetContainerGridView().ID;
            nWebGridViewNode.Attributes.Append(aWebGridViewID);

            XmlNode nCheckBox = CreateCheckBoxNode(xmlDoc, checkState);
            nWebGridViewNode.AppendChild(nCheckBox);

            return nWebGridViewNode;
        }

        private XmlNode CreateCheckBoxNode(XmlDocument xmlDoc, string checkState)
        {
            XmlNode nCheckBox = xmlDoc.CreateElement("CheckBox");

            XmlAttribute aKeyValues = xmlDoc.CreateAttribute("KeyValues");
            aKeyValues.Value = this.KeyValues;
            nCheckBox.Attributes.Append(aKeyValues);

            XmlAttribute aUser = xmlDoc.CreateAttribute("User");
            aUser.Value = CliUtils.fLoginUser;
            nCheckBox.Attributes.Append(aUser);

            nCheckBox.InnerText = checkState;
            return nCheckBox;
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

        public override void DataBind()
        {
            try
            {
                base.DataBind();
            }
            catch 
            {
            }
            if (this.Page.Site == null)
            {
                DataRowView row = this.GetDataItem() as DataRowView;
                WebDataSource wds = this.GetContainerDataSource();
                if (row != null)
                {
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
            if (!MultiCheckPostBack)
            {
                if (this.CheckBinding == null) { }
                else if (this.CheckBinding == DBNull.Value)
                {
                    this.Checked = false;
                }
                else
                {
                    if (BindingType == CheckBindingType.Boolean)
                    {
                        this.Checked = (bool)this.CheckBinding;
                    }
                    else if (BindingType == CheckBindingType.Int)
                    {
                        this.Checked = (string.Compare(CheckBinding.ToString(), 0, "1", 0, 1, true) == 0);
                    }
                    else if (BindingType == CheckBindingType.String)
                    {
                        this.Checked = (string.Compare(CheckBinding.ToString(), 0, "y", 0, 1, true) == 0);
                    }
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site == null && this.MultiCheckPostBack)
            {
                ClientScriptManager csm = this.Page.ClientScript;
                string context =
                    "function ReceiveServerData(arg)" +
                    "{" +
                        "if(arg != '')" +
                        "{" +
                            "alert(arg);" +
                            this.UniqueID + ".checked=!" + this.UniqueID + ".checked;" +
                        "}" +
                    "}";
                string clickScript = csm.GetCallbackEventReference(this, "this.checked", context, "");
                writer.AddAttribute("onclick", clickScript, true);
            }
            base.Render(writer);
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

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            if (!MultiCheckPostBack)
            {
                RefreshBindingValue();
            }
        }

        public void RefreshBindingValue()
        {
            if (BindingType == CheckBindingType.Boolean)
            {
                this.CheckBinding = this.Checked;
            }
            else if (BindingType == CheckBindingType.Int)
            {
                this.CheckBinding = this.Checked ? 1 : 0;
            }
            else if (BindingType == CheckBindingType.String)
            {
                this.CheckBinding = this.Checked ? "Y" : "N";
            }
        }

        public enum CheckBindingType
        {
            Boolean,
            String,
            Int
        }
    }
}
