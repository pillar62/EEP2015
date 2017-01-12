using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Xml;
using System.Globalization;
using System.Resources;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Srvtools
{
    [ToolboxData("<{0}:WebRefButton runat=server></{0}:WebRefButton>")]
    [ToolboxBitmap(typeof(WebRefButton), "Resources.WebRefButton.png")]
    public class WebRefButton : WebControl
    {
        public WebRefButton()
        {
            _MatchControls = new MatchControlsCollection(this, typeof(MatchControl));
        }

        private MatchControlsCollection _MatchControls;
        [Category("Infolight"),
        Description("Specifies the controls whose binding data can be copied from source table to destination table"),
        PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        TypeConverter(typeof(CollectionConverter))]
        public MatchControlsCollection MatchControls
        {
            get
            {
                return _MatchControls;
            }
        }

        [Category("Infolight"),
         Description("Specifies the reference URL")]
        [EditorAttribute(typeof(UrlEditor), typeof(UITypeEditor))]
        public string RefURL
        {
            get
            {
                object obj = this.ViewState["RefURL"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                if (value.IndexOf("~") != -1)
                {
                    value = value.Replace("~", "..");
                }
                this.ViewState["RefURL"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(400),
        Description("Specifies the height of the reference page")]
        public int RefURLHeight
        {
            get
            {
                object obj = this.ViewState["RefURLHeight"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 400;
            }
            set
            {
                this.ViewState["RefURLHeight"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(500),
        Description("Specifies the width of the reference page")]
        public int RefURLWidth
        {
            get
            {
                object obj = this.ViewState["RefURLWidth"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 500;
            }
            set
            {
                this.ViewState["RefURLWidth"] = value;
            }
        }

        [Category("Infolight"),
        Description("Specifies the caption of the button")]
        public string Caption
        {
            get
            {
                object obj = this.ViewState["Caption"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["Caption"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(200)]
        public int RefURLLeft
        {
            get
            {
                object obj = this.ViewState["RefURLLeft"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 200;
            }
            set
            {
                this.ViewState["RefURLLeft"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(200)]
        public int RefURLTop
        {
            get
            {
                object obj = this.ViewState["RefURLTop"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 200;
            }
            set
            {
                this.ViewState["RefURLTop"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Caption);
            if (this.Site == null)
            {
                string ctrls = this.GetMatchControls();
                string script = "window.open('" + this.RefURL + "?MatchControls=" + ctrls + "', '', 'width=" +
                    this.RefURLWidth + ",height=" + this.RefURLHeight + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no,top=" + this.RefURLTop + ",left=" + this.RefURLLeft + "')";
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, script);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        public string GetMatchControls()
        {
            string retValue = "";
            foreach (MatchControl ctrl in this.MatchControls)
            {
                Control parent = this.NamingContainer.FindControl(ctrl.ControlID);
                if (parent != null)
                    retValue += HttpUtility.UrlEncode(parent.UniqueID) + ";";
                else
                    retValue += HttpUtility.UrlEncode(ctrl.ControlID) + ";";
            }
            if (retValue != "")
            {
                retValue = retValue.Substring(0, retValue.LastIndexOf(';'));
            }
            return retValue;
        }
    }

    public class MatchControlsCollection : InfoOwnerCollection
    {
        public MatchControlsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(MatchControl))
        {
        }

        public new MatchControl this[int index]
        {
            get
            {
                return (MatchControl)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is MatchControl)
                    {
                        //原来的Collection设置为0
                        ((MatchControl)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((MatchControl)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class MatchControl : InfoOwnerCollectionItem, IGetValues
    {
        public MatchControl()
        {
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get 
            {
                return _ControlID; 
            }
            set 
            {
                _ControlID = value; 
            }
        }

        private string _ControlID;
        [NotifyParentProperty(true),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ControlID
        {
            get
            {
                return _ControlID;
            }
            set
            {
                _ControlID = value;
            }
        }

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is WebRefButton)
            {
                WebRefButton refbtn = (WebRefButton)this.Owner;
                if (string.Compare(sKind, "controlid", true) == 0)//IgnoreCase
                {
                    string Temp = "";
                    bool findControl = false;
                    foreach (Control ctrl in refbtn.Page.Controls)
                    {
                        if (ctrl is WebDetailsView)
                        {
                            #region container is WebDetailsView
                            WebDetailsView detView = (WebDetailsView)ctrl;
                            foreach (DataControlField field in detView.Fields)
                            {
                                if (field is TemplateField)
                                {
                                    TemplateBuilder EditBuilder = (TemplateBuilder)((TemplateField)field).EditItemTemplate;
                                    TemplateBuilder InsertBuilder = (TemplateBuilder)((TemplateField)field).InsertItemTemplate;
                                    bool bInEditTemplate = false;
                                    if (EditBuilder != null)
                                    {
                                        findControl = HasRefValIn(detView, EditBuilder.Text, refbtn.ID);
                                        Temp = "EditItemTemplate";
                                        bInEditTemplate = findControl;
                                    }
                                    if (!bInEditTemplate && InsertBuilder != null)
                                    {
                                        findControl = HasRefValIn(detView, InsertBuilder.Text, refbtn.ID);
                                        Temp = "InsertItemTemplate";
                                    }
                                    if (findControl)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (findControl)
                            {
                                foreach (DataControlField field in detView.Fields)
                                {
                                    //string sText = "";
                                    if (field is TemplateField)
                                    {
                                        if (Temp == "EditItemTemplate")
                                        {
                                            TemplateBuilder EditBuilder = (TemplateBuilder)((TemplateField)field).EditItemTemplate;
                                            if (EditBuilder != null)
                                            {
                                                values.AddRange(this.GetControlNames(detView, EditBuilder.Text, refbtn.ID));
                                            }
                                        }
                                        else if (Temp == "InsertItemTemplate")
                                        {
                                            TemplateBuilder InsertBuilder = (TemplateBuilder)((TemplateField)field).InsertItemTemplate;
                                            if (InsertBuilder != null)
                                            {
                                                values.AddRange(this.GetControlNames(detView, InsertBuilder.Text, refbtn.ID));
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                            #endregion
                        }
                        else if (ctrl is WebGridView)
                        {
                            #region container is WebGridView
                            WebGridView gdView = (WebGridView)ctrl;
                            foreach (DataControlField field in gdView.Columns)
                            {
                                if (field is TemplateField)
                                {
                                    TemplateBuilder EditBuilder = (TemplateBuilder)((TemplateField)field).EditItemTemplate;
                                    TemplateBuilder FooterBuilder = (TemplateBuilder)((TemplateField)field).FooterTemplate;
                                    bool bInEditTemplate = false;
                                    if (EditBuilder != null)
                                    {
                                        findControl = HasRefValIn(gdView, EditBuilder.Text, refbtn.ID);
                                        Temp = "EditItemTemplate";
                                        bInEditTemplate = findControl;
                                    }
                                    //modified by lily 2007/4/24 for add Controlid error when a TemplateField(Gridview buttons) exists. 
                                    if (!bInEditTemplate && FooterBuilder != null)
                                    {
                                        findControl = HasRefValIn(gdView, FooterBuilder.Text, refbtn.ID);
                                        Temp = "FooterTemplate";
                                    }
                                    if (findControl)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (findControl)
                            {
                                foreach (DataControlField field in gdView.Columns)
                                {
                                    //string sText = "";
                                    if (field is TemplateField)
                                    {
                                        if (Temp == "EditItemTemplate")
                                        {
                                            TemplateBuilder EditBuilder = (TemplateBuilder)((TemplateField)field).EditItemTemplate;
                                            if (EditBuilder != null)
                                            {
                                                values.AddRange(this.GetControlNames(gdView, EditBuilder.Text, refbtn.ID));
                                            }
                                        }
                                        else if (Temp == "FooterTemplate")
                                        {
                                            TemplateBuilder FooterBuilder = (TemplateBuilder)((TemplateField)field).FooterTemplate;
                                            if (FooterBuilder != null)
                                            {
                                                values.AddRange(this.GetControlNames(gdView, FooterBuilder.Text, refbtn.ID));
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                            #endregion
                        }
                        else if (ctrl is WebFormView)
                        {
                            #region container is WebFormView
                            WebFormView frmView = (WebFormView)ctrl;
                            TemplateBuilder EditBuilder = (TemplateBuilder)frmView.EditItemTemplate;
                            TemplateBuilder InsertBuilder = (TemplateBuilder)frmView.InsertItemTemplate;
                            bool bInEditTemplate = false;
                            if (EditBuilder != null)
                            {
                                findControl = HasRefValIn(frmView, EditBuilder.Text, refbtn.ID);
                                Temp = "EditItemTemplate";
                                bInEditTemplate = findControl;
                            }
                            if (!bInEditTemplate && InsertBuilder != null)
                            {
                                findControl = HasRefValIn(frmView, InsertBuilder.Text, refbtn.ID);
                                Temp = "InsertItemTemplate";
                            }
                            if (findControl)
                            {
                                if (Temp == "EditItemTemplate")
                                {
                                    retList = GetControlNames(frmView, EditBuilder.Text, refbtn.ID);
                                }
                                else if (Temp == "InsertItemTemplate")
                                {
                                    retList = GetControlNames(frmView, InsertBuilder.Text, refbtn.ID);
                                }
                                break;
                            }
                            #endregion
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
                }
            }
            return retList;
        }

        private bool HasRefValIn(Control Container, string BuilderText, string refID)
        {
            IDesignerHost host = (IDesignerHost)Container.Site.GetService(typeof(IDesignerHost));
            Control[] ctrls = ControlParser.ParseControls(host, BuilderText);
            int i = ctrls.Length;
            for (int j = 0; j < i; j++)
            {
                if (ctrls[j] is WebRefButton && ctrls[j].ID == refID)
                {
                    return true;
                }
            }
            return false;
        }

        private string[] GetControlNames(Control Container, string BuilderText, string refID)
        {
            IDesignerHost host = (IDesignerHost)Container.Site.GetService(typeof(IDesignerHost));
            Control[] ctrls = ControlParser.ParseControls(host, BuilderText);
            List<string> ctrlNames = new List<string>();
            int i = ctrls.Length;
            for (int j = 0; j < i; j++)
            {
                if (!(ctrls[j] is LiteralControl) && ctrls[j].ID != refID)
                {
                    ctrlNames.Add(ctrls[j].ID);
                }
            }
            int m = ctrlNames.Count;
            string[] retList = new string[m];
            for (int n = 0; n < m; n++)
            {
                retList[n] = ctrlNames[n];
            }
            return retList;
        }
        #endregion
    }
}
