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
using System.Windows.Forms.Design;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Resources;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Srvtools
{
    [ToolboxBitmap(typeof(WebDefault), "Resources.WebDefault.png")]
    [ToolboxData("<{0}:WebDefault runat=\"server\"></{0}:WebDefault>")]
    [ParseChildren(true, "Fields")]
    [Designer(typeof(DataSourceDesigner), typeof(IDesigner))]
    public class WebDefault : WebControl
    {
        public WebDefault()
        {
            _Fields = new DefaultFieldsCollection(this, typeof(DefaultFieldItem));
        }

        //private SYS_LANGUAGE language;

        private Control GetAllCtrls2(string strid, Control ct)
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
                        Control ctrtn = GetAllCtrls2(strid, ctchild);
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
                return GetAllCtrls2(ObjID, this.Page);
            }
            else
            {
                if (this.Page.Form != null)
                    return GetAllCtrls2(ObjID, this.Page.Form);
                else
                    return GetAllCtrls2(ObjID, this.Page);
            }
        }

        private DefaultFieldsCollection _Fields;
        [Category("Infolight"),
        Description("The columns which WebDefault is applied to")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        public DefaultFieldsCollection Fields
        {
            get { return _Fields; }
        }

        [Category("Infolight"),
        Description("Indicates whether WebDefault is enabled or disabled")]
        [DefaultValue(false)]
        public bool DefaultActive
        {
            get
            {
                object obj = this.ViewState["DefaultActive"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["DefaultActive"] = value;
            }
        }

        [Category("Infolight"),
        Description("The ID of WebDataSource which the control is bound to")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get
            {
                object obj = this.ViewState["DataSourceID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["DataSourceID"] = value;
                //object obj = this.GetObjByID(value);
                //if (obj != null && obj is WebDataSource)
                //{
                //    WebDataSource wds = (WebDataSource)obj;
                //    this.DataMember = wds.DataMember;
                //}
            }
        }

        [Category("Infolight"),
        Description("The table of view used for binding against")]
        [Editor(typeof(DataMemberEditor), typeof(UITypeEditor))]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string DataMember
        {
            get
            {
                object obj = this.ViewState["DataMember"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["DataMember"] = value;
            }
        }

        [Category("Infolight"),
        Description("Indicates whether the last update data will be automatically to the specified column")]
        [DefaultValue(false)]
        public bool CarryOnActive
        {
            get
            {
                object obj = this.ViewState["CarryOnActive"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["CarryOnActive"] = value;
            }
        }

        public object[,] GetDefaultValues()
        {
            //WebDataSource wds = (WebDataSource)this.GetObjByID(this.DataSourceID);
            //language = CliSysMegLag.GetClientLanguage();
            //language = CliUtils.fClientLang;
            object[,] objReturn = null;
            if (this.DefaultActive)
            {
                int i = this.Fields.Count;
                objReturn = new object[i, 2];
                for (int j = 0; j < i; j++)
                {
                    objReturn[j, 0] = this.Fields[j].FieldName;
                    objReturn[j, 1] = DefaultValue(this.Fields[j].DefaultValue);
                }
            }
            return objReturn;
        }

        private object DefaultValue(string Default)
        {
            return CliUtils.GetValue(Default, this.Page);
        }
    }

    #region DefaultFieldsCollection class
    public class DefaultFieldsCollection : InfoOwnerCollection
    {
        public DefaultFieldsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(DefaultFieldItem))
        {
        }

        public new DefaultFieldItem this[int index]
        {
            get
            {
                return (DefaultFieldItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is DefaultFieldItem)
                    {
                        //原来的Collection设置为0
                        ((DefaultFieldItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((DefaultFieldItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
    #endregion

    #region DefaultFieldItem class
    public class DefaultFieldItem : InfoOwnerCollectionItem, IGetValues
    {
        public DefaultFieldItem()
        { }

        public override string ToString()
        {
            return _FieldName;
        }

        #region Properties
        [NotifyParentProperty(true)]
        public override string Name
        {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = value;
            }
        }

        private string _FieldName;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string FieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = value;
            }
        }

        private string _DefaultValue;
        [Category("Value"),
        NotifyParentProperty(true)]
        public string DefaultValue
        {
            get
            {
                return _DefaultValue;
            }
            set
            {
                _DefaultValue = value;
            }
        }

        private bool _CarryOn;
        [Category("Value"),
        NotifyParentProperty(true),
        DefaultValue(false)]
        public bool CarryOn
        {
            get
            {
                return _CarryOn;
            }
            set
            {
                _CarryOn = value;
            }
        }
        #endregion

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "fieldname", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebDefault)
                {
                    WebDefault wv = (WebDefault)this.Owner;
                    if (wv.Page != null && wv.DataSourceID != null && wv.DataSourceID != "")
                    {
                        foreach (Control ctrl in wv.Page.Controls)
                        {
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == wv.DataSourceID)
                            {
                                WebDataSource ds = (WebDataSource)ctrl;
                                if (ds.DesignDataSet == null)
                                {
                                    WebDataSet wds = WebDataSet.CreateWebDataSet(ds.WebDataSetID);
                                    if (wds != null)
                                    {
                                        ds.DesignDataSet = wds.RealDataSet;
                                    }
                                }
                                if (ds.DesignDataSet != null && ds.DesignDataSet.Tables.Contains(ds.DataMember))
                                {
                                    foreach (DataColumn column in ds.DesignDataSet.Tables[ds.DataMember].Columns)
                                    {
                                        values.Add(column.ColumnName);
                                    }
                                }
                                break;
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
            }
            return retList;
        }

        #endregion
    }
    #endregion

    #region WebDefaultDesigner
    /*public class WebDefaultDesigner : DataSourceDesigner
    {
        private DesignerActionListCollection actionLists;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                actionLists = base.ActionLists;
                actionLists.Add(new WebDefaultActionList(this.Component));
                return actionLists;
            }
        }
    }

    public class WebDefaultActionList : DesignerActionList
    {
        private WebDefault deft;

        public WebDefaultActionList(IComponent component)
            : base(component)
        {
            this.deft = component as WebDefault;
        }

        public DefaultFieldsCollection Fields
        {
            get { return deft.Fields; }
        }

        public bool DefaultActive
        {
            get
            {
                return deft.DefaultActive;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(WebDefault))["DefaultActive"].SetValue(deft, value);
            }
        }

        public bool CarryOnActive
        {
            get
            {
                return deft.CarryOnActive;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(WebDefault))["CarryOnActive"].SetValue(deft, value);
            }
        }
    }*/
    #endregion
}
