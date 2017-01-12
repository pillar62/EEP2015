using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Xml;
using System.IO;
using System.Resources;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Data;
using System.Reflection;

namespace Srvtools
{
    [ToolboxData("<{0}:WebSecColumns runat=\"server\"></{0}:WebSecColumns>")]
    [Designer(typeof(WebSecColumnsDesigner), typeof(IDesigner))]
    public class WebSecColumns : WebInfoBaseControl
    {
        public WebSecColumns()
        {
            _Columns = new WebSecColumnsCollection(this, typeof(WebSecColumn));
            _WebSecControls = new WebSecControlsCollection(this, typeof(WebSecControl));
        }
        public WebSecColumns(IContainer container)
        {
            _Columns = new WebSecColumnsCollection(this, typeof(WebSecColumn));
            _WebSecControls = new WebSecControlsCollection(this, typeof(WebSecControl));
        }

        protected override void OnLoad(EventArgs e)
        {
            Do();
            base.OnLoad(e);
        }

        public void Do()
        {
            ControlCollection collection = null;
            if (this.Page.Master == null)
                collection = this.Page.Form.Controls;
            else
                collection = this.Parent.Controls;
            foreach (Control ct in collection)
            {
                if (!string.IsNullOrEmpty(ct.ID))
                {
                    if (ct.GetType().FullName == "AjaxTools.AjaxFormView")
                    {
                        SetToAjaxFormView(ct);
                    }
                }
            }

            CompositeDataBoundControl ctrl = (CompositeDataBoundControl)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(CompositeDataBoundControl));
            if (ctrl != null)
            {
                DataControlFieldCollection fieldCollection = null;
                if (ctrl is GridView)
                {
                    GridView grid = (GridView)ctrl;
                    fieldCollection = grid.Columns;
                }
                else if (ctrl is DetailsView)
                {
                    DetailsView det = (DetailsView)ctrl;
                    fieldCollection = det.Fields;
                }
                // 处理GridView和DetailsView的BoundField
                if (fieldCollection != null)
                {
                    foreach (DataControlField field in fieldCollection)
                    {
                        if (field is BoundField)
                        {
                            BoundField boundField = (BoundField)field;
                            foreach (WebSecColumn column in this.Columns)
                            {
                                if (column.ColumnName == boundField.DataField)
                                {
                                    boundField.Visible = this.ColumnsVisible;
                                    boundField.ReadOnly = this.ColumnsReadOnly;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SetToAjaxFormView(object value)
        {
            if (value.GetType().FullName == "AjaxTools.AjaxFormView")
            {
                Type type = value.GetType();
                PropertyInfo info = type.GetProperty("Fields");
                IList iExtGridColumnCollection = info.GetValue(value, null) as IList;
                foreach (WebSecColumn wsc in this.Columns)
                {
                    for (int i = 0; i < iExtGridColumnCollection.Count; i++)
                    {
                        String strColumnName = iExtGridColumnCollection[i].GetType().GetProperty("DataField").GetValue(iExtGridColumnCollection[i], null).ToString();
                        if (strColumnName == wsc.ColumnName)
                        {
                            iExtGridColumnCollection[i].GetType().GetProperty("ReadOnly").SetValue(iExtGridColumnCollection[i], this.ColumnsReadOnly, null);
                            iExtGridColumnCollection[i].GetType().GetProperty("Visible").SetValue(iExtGridColumnCollection[i], this.ColumnsVisible, null);
                            break;
                        }
                    }
                }
            }
        }

        [Category("InfoLight"),
        Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get
            {
                object obj = this.ViewState["DataSourceID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["DataSourceID"] = value;
                if (!DesignMode && this.Page != null)
                    Do();
            }
        }

        [Category("InfoLight")]
        public bool ColumnsReadOnly
        {
            get
            {
                object obj = this.ViewState["ReadOnly"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["ReadOnly"] = value;
                if (!DesignMode && this.Page != null)
                    Do();
            }
        }

        [Category("InfoLight"),
        DefaultValue(true)]
        public bool ColumnsVisible
        {
            get
            {
                object obj = this.ViewState["ColumnsVisible"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["ColumnsVisible"] = value;
            }
        }

        private WebSecColumnsCollection _Columns;
        [Category("InfoLight"),
        PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        TypeConverter(typeof(CollectionConverter))]
        public WebSecColumnsCollection Columns
        {
            get
            {
                return _Columns;
            }
        }

        private WebSecControlsCollection _WebSecControls;
        [Category("InfoLight"),
        PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        TypeConverter(typeof(CollectionConverter))]
        public WebSecControlsCollection WebSecControls
        {
            get
            {
                return _WebSecControls;
            }
        }

        [Browsable(false)]
        public string AffectedControls
        {
            get
            {
                object obj = this.ViewState["AffectedControls"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["AffectedControls"] = value;
            }
        }
    }

    public class WebSecColumnsCollection : InfoOwnerCollection
    {
        public WebSecColumnsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebSecColumn))
        {
        }

        public new WebSecColumn this[int index]
        {
            get
            {
                return (WebSecColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebSecColumn)
                    {
                        //原来的Collection设置为0
                        ((WebSecColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebSecColumn)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class WebSecColumn : InfoOwnerCollectionItem, IGetValues
    {
        public WebSecColumn()
            : this("")
        {
        }

        public WebSecColumn(string columnName)
        {
            _ColumnName = columnName;
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }

        public override string ToString()
        {
            return _ColumnName;
        }

        private string _ColumnName;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor)),
        NotifyParentProperty(true)]
        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
            set
            {
                _ColumnName = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retItems = null;
            if (string.Compare(sKind, "columnname", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebSecColumns)
                {
                    WebSecColumns gcs = (WebSecColumns)this.Owner;
                    if (gcs != null)
                    {
                        List<string> arrLst = new List<string>();
                        object obj = gcs.GetObjByID(gcs.DataSourceID);
                        if (obj != null && obj is WebDataSource)
                        {
                            WebDataSource wds = (WebDataSource)obj;
                            if (wds.DesignDataSet == null)
                            {
                                WebDataSet ds = WebDataSet.CreateWebDataSet(wds.WebDataSetID);
                                if (ds != null)
                                {
                                    wds.DesignDataSet = ds.RealDataSet;
                                }
                            }
                            List<string> values = new List<string>();
                            if (wds.DesignDataSet != null && wds.DesignDataSet.Tables.Contains(wds.DataMember))
                            {
                                foreach (DataColumn column in wds.DesignDataSet.Tables[wds.DataMember].Columns)
                                {
                                    values.Add(column.ColumnName);
                                }
                            }
                            retItems = values.ToArray();
                        }
                    }
                }
            }
            return retItems;
        }
    }

    public class WebSecControlsCollection : InfoOwnerCollection
    {
        public WebSecControlsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebSecControl))
        {
        }

        public new WebSecControl this[int index]
        {
            get
            {
                return (WebSecControl)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebSecControl)
                    {
                        //原来的Collection设置为0
                        ((WebSecControl)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebSecControl)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class WebSecControl : InfoOwnerCollectionItem, IGetValues
    {
        public WebSecControl()
            : this("", "", "", "")
        {
        }

        public WebSecControl(string controlParent, string controlTemplateGroup, string controlTemplate, string controlId)
        {
            _ControlParent = controlParent;
            _ControlTemplateGroup = controlTemplateGroup;
            _ControlTemplate = controlTemplate;
            _ControlID = controlId;
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get
            {
                return GetString();
            }
            set
            {
                string[] namePart = value.Split('.');
                if (value.Length == 4)
                {
                    _ControlParent = namePart[0];
                    _ControlTemplateGroup = namePart[1];
                    _ControlTemplate = namePart[2];
                    _ControlID = namePart[3];
                }
            }
        }

        public override string ToString()
        {
            return GetString();
        }

        private string GetString()
        {
            string s = ((_ControlParent != "") ? (_ControlParent + ".") : "")
                    + ((_ControlTemplateGroup != "") ? (_ControlTemplateGroup + ".") : "")
                    + ((_ControlTemplate != "") ? (_ControlTemplate + ".") : "")
                    + ((_ControlID != "") ? (_ControlID + ".") : "");
            if (s.IndexOf('.') != -1)
            {
                s = s.Substring(0, s.LastIndexOf('.'));
            }
            return s;
        }

        private string _ControlID = "";
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor)),
        NotifyParentProperty(true)]
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

        private string _ControlParent = "";
        [Editor(typeof(GridEditor), typeof(UITypeEditor)),
        NotifyParentProperty(true)]
        public string ControlParent
        {
            get
            {
                return _ControlParent;
            }
            set
            {
                _ControlParent = value;
            }
        }

        private string _ControlTemplateGroup = "";
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor)),
        NotifyParentProperty(true)]
        public string ControlTemplateGroup
        {
            get
            {
                return _ControlTemplateGroup;
            }
            set
            {
                _ControlTemplateGroup = value;
            }
        }

        private string _ControlTemplate = "";
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor)),
        NotifyParentProperty(true)]
        public string ControlTemplate
        {
            get
            {
                return _ControlTemplate;
            }
            set
            {
                _ControlTemplate = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retItems = null;
            if (this.Owner is WebSecColumns)
            {
                WebSecColumns gcs = (WebSecColumns)this.Owner;
                if (this._ControlParent != "")
                {
                    object objParent = gcs.GetObjByID(_ControlParent);
                    if (objParent != null && objParent is CompositeDataBoundControl 
                        && ((CompositeDataBoundControl)objParent).DataSourceID == gcs.DataSourceID)
                    {
                        CompositeDataBoundControl dbCtrl = (CompositeDataBoundControl)objParent;
                        IDesignerHost host = (IDesignerHost)gcs.Site.GetService(typeof(IDesignerHost));
                        ControlDesigner viewDesigner = (ControlDesigner)host.GetDesigner(dbCtrl);
                        if (gcs != null)
                        {
                            List<string> arrLst = new List<string>();
                            if (string.Compare(sKind, "controltemplategroup", true) == 0)//IgnoreCase
                            {
                                foreach (TemplateGroup tempGroup in viewDesigner.TemplateGroups)
                                {
                                    arrLst.Add(tempGroup.GroupName);
                                }
                            }
                            else if (string.Compare(sKind, "controltemplate", true) == 0)//IgnoreCase
                            {
                                if (this._ControlParent != "" && this._ControlTemplateGroup != "")
                                {
                                    foreach (TemplateGroup tempGroup in viewDesigner.TemplateGroups)
                                    {
                                        if (tempGroup.GroupName == this._ControlTemplateGroup)
                                        {
                                            foreach (TemplateDefinition tempDef in tempGroup.Templates)
                                            {
                                                arrLst.Add(tempDef.Name);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (string.Compare(sKind, "controlid", true) == 0)//IgnoreCase
                            {
                                if (this._ControlParent != "" && this._ControlTemplateGroup != "" && this._ControlTemplate != "")
                                {
                                    foreach (TemplateGroup tempGroup in viewDesigner.TemplateGroups)
                                    {
                                        if (tempGroup.GroupName == this._ControlTemplateGroup)
                                        {
                                            foreach (TemplateDefinition tempDef in tempGroup.Templates)
                                            {
                                                if (tempDef.Name == this._ControlTemplate)
                                                {
                                                    string content = tempDef.Content;
                                                    if (content != "")
                                                    {
                                                        Control[] secControls = ControlParser.ParseControls(host, content);
                                                        foreach (Control ctrl in secControls)
                                                        {
                                                            if(ctrl.ID != null && ctrl.ID != "")
                                                                arrLst.Add(ctrl.ID);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            retItems = arrLst.ToArray();
                        }
                    }
                }
            }
            return retItems;
        }
    }

    public class WebSecColumnsDesigner : DataSourceDesigner
    {
        public WebSecColumnsDesigner()
        {
            DesignerVerb AffectedControlsVerb = new DesignerVerb("Get Security Controls", new EventHandler(OnGetSecControls));
            this.Verbs.Add(AffectedControlsVerb);
        }

        public void OnGetSecControls(object sender, EventArgs e)
        {
            WebSecColumns sec = (WebSecColumns)this.Component;
            IDesignerHost host = (IDesignerHost)this.GetService(typeof(IDesignerHost));
            foreach(Control ctrl in sec.Page.Controls)
            {
                if (ctrl is CompositeDataBoundControl && ((CompositeDataBoundControl)ctrl).DataSourceID == sec.DataSourceID)
                {
                    CompositeDataBoundControl dbCtrl = (CompositeDataBoundControl)ctrl;
                    ControlDesigner viewDesigner = (ControlDesigner)host.GetDesigner(dbCtrl);
                    foreach (TemplateGroup tempGroup in viewDesigner.TemplateGroups)
                    {
                        foreach (TemplateDefinition tempDef in tempGroup.Templates)
                        {
                            string content = tempDef.Content;
                            if (content != "")
                            {
                                Control[] secControls = ControlParser.ParseControls(host, content);
                                foreach (Control secCtrl in secControls)
                                {
                                    IDataBindingsAccessor dbAccess = (IDataBindingsAccessor)secCtrl;
                                    if (dbAccess.HasDataBindings)
                                    {
                                        bool ctrlAdded = false;
                                        foreach (DataBinding binding in dbAccess.DataBindings)
                                        {
                                            string[] ExpParts = binding.Expression.Split('"');
                                            if (ExpParts.Length >= 3)
                                            {
                                                string fieldName = ExpParts[1];
                                                foreach (WebSecColumn column in sec.Columns)
                                                {
                                                    if (fieldName == column.ColumnName)
                                                    {
                                                        WebSecControl secControl = new WebSecControl(dbCtrl.ID, tempGroup.GroupName, tempDef.Name, secCtrl.ID);
                                                        WebSecControl existSecControl = (WebSecControl)sec.WebSecControls[secControl.ToString()];
                                                        if (existSecControl == null)
                                                            sec.WebSecControls.Add(secControl);
                                                        ctrlAdded = true;
                                                        break;
                                                    }
                                                }
                                                if (ctrlAdded)
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.Tag.SetDirty(true);
        }

        private List<Control> ViewControls = new List<Control>();
        private void GetViewCtrls(ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                ViewControls.Add(ctrl);
                if (ctrl.Controls.Count > 0)
                {
                    GetViewCtrls(ctrl.Controls);
                }
            }
        }
    }
}
