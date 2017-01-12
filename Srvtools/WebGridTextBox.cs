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
    [ToolboxBitmap(typeof(WebGridTextBox), "Resources.WebGridTextBox.png")]
    public class WebGridTextBox : TextBox, ICallbackEventHandler, IGetValues
    {
        public WebGridTextBox()
        { 
        }

        public override AutoCompleteType AutoCompleteType
        {
            get
            {
                return AutoCompleteType.Disabled;
            }
        }

        [Category("InfoLight"),
        DefaultValue(true)]
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

        [Category("InfoLight"),
        Description("Indicates which data field has bound to the control"),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
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

        [Category("InfoLight")]
        [DefaultValue("")]
        public string SubmitButtonID
        {
            get
            {
                object obj = this.ViewState["SubmitButtonID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["SubmitButtonID"] = value;
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
                if (designer.DataBindings["Text"] != null)
                {
                    string content = designer.DataBindings["Text"].Expression;
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
            XmlNode nWebGridTextBoxes = nRoot.SelectSingleNode("WebGridTextBoxes");
            //如果不存在WebGridTextBoxes的键,则创建该键
            if (nWebGridTextBoxes == null)
            {
                nWebGridTextBoxes = CreateWebGridTextBoxesNode(xmlDoc);
                nRoot.AppendChild(nWebGridTextBoxes);
            }
            XmlNode nWebGridView = nWebGridTextBoxes.SelectSingleNode("WebGridView[@ID='" + this.GetContainerGridView().ID + "']");
            //如果存在该WebGridView的键
            if (nWebGridView != null)
            {
                XmlNode nRow = nWebGridView.SelectSingleNode("Row[@KeyValues='" + this.KeyValues + "']");
                //如果存在该Row的键
                if (nRow != null)
                {
                    XmlNode nTextBox = nRow.SelectSingleNode("TextBox[@FieldName='" + this.DataField + "']");
                    //如果存在该TextBox的键
                    if (nTextBox != null)
                    {
                        string OwnerUser = nTextBox.Attributes["User"].Value;
                        if (OwnerUser == CliUtils.fLoginUser)
                        {
                            nTextBox.InnerText = eventArgument;
                        }
                        else
                        {
                            callBackValue = "some one is editing this record...";
                        }
                    }
                    // 否则创建该键
                    else
                    {
                        nTextBox = CreateTextBoxNode(xmlDoc, eventArgument);
                        nRow.AppendChild(nTextBox);
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
                nWebGridTextBoxes.AppendChild(nWebGridView);
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

        private XmlNode CreateWebGridTextBoxesNode(XmlDocument xmlDoc)
        {
            XmlNode nWebGridTextBoxes = xmlDoc.CreateElement("WebGridTextBoxes");
            return nWebGridTextBoxes;
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

            XmlNode nTextBox = CreateTextBoxNode(xmlDoc, Value);
            nRowNode.AppendChild(nTextBox);
            return nRowNode;
        }

        private XmlNode CreateTextBoxNode(XmlDocument xmlDoc, string Value)
        {
            XmlNode nTextBoxNode = xmlDoc.CreateElement("TextBox");

            XmlAttribute aField = xmlDoc.CreateAttribute("FieldName");
            aField.Value = this.DataField;
            nTextBoxNode.Attributes.Append(aField);

            XmlAttribute aUser = xmlDoc.CreateAttribute("User");
            aUser.Value = CliUtils.fLoginUser;
            nTextBoxNode.Attributes.Append(aUser);

            nTextBoxNode.InnerText = Value;
            return nTextBoxNode;
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
                if (!csm.IsStartupScriptRegistered("webGridTextBoxFlag"))
                {
                    //csm.RegisterStartupScript(this.GetType(), "webGridTextBoxFlag", "var webGridTextBoxFlag=new Object();", true);
                    csm.RegisterStartupScript(this.GetType(), "webGridTextBoxFlag", "var webGridTextBoxFlag=0;", true);
                }
                //string blurcontext = "function ReceiveServerData(){}";
                //string blurScript = csm.GetCallbackEventReference(this, "this.value", blurcontext, "", true);
                //writer.AddAttribute("onblur", blurScript, true);

                //if (!string.IsNullOrEmpty(this.SubmitButtonID))
                //{
                //    string edScript = "document.getElementById('{0}').disabled = {1};";
                //    writer.AddAttribute("onfoucs", string.Format(edScript, this.SubmitButtonID, "true"));
                //    writer.AddAttribute("onblur", string.Format(edScript, this.SubmitButtonID, "false"));
                //}
                string onkeyupScript = "", onkeyupcontext = "";
                if (!string.IsNullOrEmpty(this.SubmitButtonID))
                {
                    //onkeyupcontext = "function ReceiveServerData(arg, context){if(arg != ''){alert(arg);}if(--webGridTextBoxFlag[context] == 0){document.getElementById('" + this.SubmitButtonID + "').disabled = false;}}";
                    //onkeyupScript = "document.getElementById('" + this.SubmitButtonID + "').disabled = true;if(webGridTextBoxFlag." + this.ClientID + "==null){webGridTextBoxFlag." + this.ClientID + " = 1;}else{++webGridTextBoxFlag." + this.ClientID + "}" + csm.GetCallbackEventReference(this, "this.value", onkeyupcontext, "'" + this.ClientID + "'", true);

                    onkeyupcontext = "function ReceiveServerData(arg, context){if(arg != ''){alert(arg);}if(--webGridTextBoxFlag == 0){document.getElementById('" + this.SubmitButtonID + "').disabled = false;}}";
                    onkeyupScript = "document.getElementById('" + this.SubmitButtonID + "').disabled = true;++webGridTextBoxFlag;" + csm.GetCallbackEventReference(this, "this.value", onkeyupcontext, "'" + this.ClientID + "'", true);
                }
                else
                {
                    onkeyupcontext = "function ReceiveServerData(arg){if(arg != ''){alert(arg);}}";
                    onkeyupScript = csm.GetCallbackEventReference(this, "this.value", onkeyupcontext, "", true);
                }
                writer.AddAttribute("onkeyup", onkeyupScript, true);
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
            if (this.Page.Site == null)
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
