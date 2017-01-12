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
using System.Collections.Specialized;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

namespace Srvtools
{
    public abstract class BaseWebDetailsView : DetailsView, IBaseWebControl
    {
        internal Control FindChildControl(string strid, Control ct)
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
                        Control ctrtn = FindChildControl(strid, ctchild);
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
        internal Control FindChildControl(string strid, Control ct, FindControlType type, Type ReturnControlType)
        {
            string fieldName = "ID";
            if (type == FindControlType.DataSourceID)
            {
                fieldName = "DataSourceID";
            }
            else if (type == FindControlType.BindingObject)
            {
                fieldName = "BindingObject";
            }
            else if (type == FindControlType.MasterDataSource)
            {
                fieldName = "MasterDataSource";
            }

            Type ctType = ct.GetType();
            PropertyInfo pi = ctType.GetProperty(fieldName);
            if (pi != null && pi.GetValue(ct, null) != null && pi.GetValue(ct, null).ToString() == strid && ReturnControlType.IsInstanceOfType(ct))
            {
                return ct;
            }
            else
            {
                if (ct.HasControls())
                {
                    foreach (Control ctchild in ct.Controls)
                    {
                        Control ctrtn = FindChildControl(strid, ctchild, type, ReturnControlType);
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

        internal List<Control> FindChildControls(string strid, Control ct, FindControlType type, Type ReturnControlType)
        {
            List<Control> lControls = new List<Control>();
            string fieldName = "ID";
            if (type == FindControlType.DataSourceID)
            {
                fieldName = "DataSourceID";
            }
            else if (type == FindControlType.BindingObject)
            {
                fieldName = "BindingObject";
            }
            else if (type == FindControlType.MasterDataSource)
            {
                fieldName = "MasterDataSource";
            }

            Type ctType = ct.GetType();
            PropertyInfo pi = ctType.GetProperty(fieldName);
            if (pi != null && pi.GetValue(ct, null) != null && pi.GetValue(ct, null).ToString() == strid && ReturnControlType.IsInstanceOfType(ct))
            {
                lControls.Add(ct);
            }
            else
            {
                if (ct.HasControls())
                {
                    foreach (Control ctchild in ct.Controls)
                    {
                        Control ctrtn = FindChildControl(strid, ctchild, type, ReturnControlType);
                        if (ctrtn != null)
                        {
                            lControls.Add(ctrtn);
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            return lControls;
        }

        public Control ExtendedFindChildControl(string strid, FindControlType type, Type ReturnControlType)
        {
            Control objContentPlaceHolder = this.Page.Form.FindControl("ContentPlaceHolder1");
            if (objContentPlaceHolder != null)
            {
                return this.FindChildControl(strid, objContentPlaceHolder, type, ReturnControlType);
            }
            else
            {
                return this.FindChildControl(strid, this.Page.Form, type, ReturnControlType);
            }
        }

        public List<Control> ExtendedFindChildControls(string strid, FindControlType type, Type ReturnControlType)
        {
            Control objContentPlaceHolder = this.DesignMode ? this.Page.FindControl("ContentPlaceHolder1") : this.Page.Form.FindControl("ContentPlaceHolder1");
            if (objContentPlaceHolder != null)
            {
                return this.FindChildControls(strid, objContentPlaceHolder, type, ReturnControlType);
            }
            else
            {
                if (this.DesignMode)
                    return this.FindChildControls(strid, this.Page, type, ReturnControlType);
                else
                    return this.FindChildControls(strid, this.Page.Form, type, ReturnControlType);
            }
        }

        public object GetObjByID(string ObjID)
        {
            if (this.Site != null)
            {
                return FindChildControl(ObjID, this.Page);
            }
            else
            {
                if (this.Page.Form != null)
                    return FindChildControl(ObjID, this.Page.Form);
                else
                    return FindChildControl(ObjID, this.Page);
            }
        }
    }

    [Designer(typeof(WebDetailsViewDesigner), typeof(IDesigner))]
    [ToolboxBitmap(typeof(WebDetailsView), "Resources.WebDetailView.ico")]
    public class WebDetailsView : BaseWebDetailsView
    {
        public WebDetailsView()
        {
            _CreateInnerNavigator = true;
            _NavControls = new DetailsViewInnerNavigatorCollection(this, typeof(ControlItem));
            _QueryFields = new WebQueryFiledsCollection(this, typeof(WebQueryField));
            this.AllowPaging = true;
            this.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            this.AutoGenerateRows = false;
            this.GetServerText = true;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string rscUrl = "../css/controls/WebDetailsView.css";
            bool isCssExist = false;
            foreach (Control ctrl in this.Page.Header.Controls)
            {
                if (ctrl is HtmlLink && ((HtmlLink)ctrl).Href == rscUrl)
                    isCssExist = true;
            }
            if (!isCssExist)
            {
                HtmlLink cssLink = new HtmlLink();
                cssLink.Href = rscUrl;
                cssLink.Attributes.Add("rel", "stylesheet");
                cssLink.Attributes.Add("type", "text/css");
                this.Page.Header.Controls.Add(cssLink);
            }
        }

        private SYS_LANGUAGE language;

        private DetailsViewInnerNavigatorCollection _NavControls;
        [Category("Infolight"),
         Description("Specifies the Controls in the inner WebNavigator")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        public DetailsViewInnerNavigatorCollection NavControls
        {
            get
            {
                return _NavControls;
            }
        }

        private WebQueryFiledsCollection _QueryFields;
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        [Category("Infolight"),
         Description("Specifies the amount of fields which are going to be shown in the query page")]
        public WebQueryFiledsCollection QueryFields
        {
            get
            {
                return _QueryFields;
            }
        }

        #region Adding Event
        public void OnAdding(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnAdding];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        internal static readonly object EventOnAdding = new object();

        public event EventHandler Adding
        {
            add { base.Events.AddHandler(EventOnAdding, value); }
            remove { base.Events.RemoveHandler(EventOnAdding, value); }
        }
        #endregion

        internal static readonly object EventOnCanceled = new object();
        public event EventHandler Canceled
        {
            add { base.Events.AddHandler(EventOnCanceled, value); }
            remove { base.Events.RemoveHandler(EventOnCanceled, value); }
        }

        public void OnCanceled(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnCanceled];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        private bool _CreateInnerNavigator;
        [Category("Infolight"),
         Description("Indicates whether the inner WebNavigator is created in the control")]
        [DefaultValue(true)]
        public bool CreateInnerNavigator
        {
            get
            {
                return _CreateInnerNavigator;
            }
            set
            {
                _CreateInnerNavigator = value;
            }
        }

        private bool _GetServerText;
        [Category("Infolight"),
         Description("Indicates whether the caption of inner WebNavigator's items use server settings automatically")]
        [DefaultValue(true)]
        public bool GetServerText
        {
            get
            {
                return _GetServerText;
            }
            set
            {
                _GetServerText = value;
            }
        }

        [Editor(typeof(LinkLabelEditor), typeof(UITypeEditor))]
        [Category("Infolight"),
         Description("Specifies the property of the label of the inner WebNavigator")]
        [DefaultValue("")]
        public string InnerNavigatorLinkLabel
        {
            get
            {
                object obj = this.ViewState["InnerNavigatorLinkLabel"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["InnerNavigatorLinkLabel"] = value;
            }
        }

        [Browsable(false)]
        [DefaultValue(true)]
        public bool NeedExecuteAdd
        {
            get
            {
                object obj = this.ViewState["NeedExecuteAdd"];

                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["NeedExecuteAdd"] = value;
            }
        }

        [Category("Infolight"),
         Description("Specifies the text to be shown when data is empty")]
        [DefaultValue(false)]
        public bool AutoEmptyDataText
        {
            get
            {
                object obj = this.ViewState["AutoEmptyDataText"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["AutoEmptyDataText"] = value;
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public bool DataHasChanged
        {
            get
            {
                object obj = this.ViewState["DataHasChanged"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["DataHasChanged"] = value;
            }
        }

        [Category("Infolight"),
         Description("Specifies the style of the appearance of the inner WebNavigator")]
        [DefaultValue(typeof(WebNavigator.CtrlType), "Image")]
        public WebNavigator.CtrlType InnerNavigatorShowStyle
        {
            get
            {
                object obj = this.ViewState["InnerNavigatorShowStyle"];
                if (obj != null)
                {
                    return (WebNavigator.CtrlType)obj;
                }
                return WebNavigator.CtrlType.Image;
            }
            set
            {
                this.ViewState["InnerNavigatorShowStyle"] = value;
                foreach (ControlItem item in _NavControls)
                {
                    item.ControlType = value;
                }
            }
        }

        [Category("Infolight"),
         Description("Specifies the amount of columns of ExpressionField")]
        [DefaultValue(0)]
        public int ExpressionFieldCount
        {
            get
            {
                object obj = this.ViewState["ExpressionFieldCount"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 0;
            }
            set
            {
                if (this.Fields.Count > 0)
                {
                    int expCnt = 0;
                    foreach (DataControlField field in this.Fields)
                    {
                        if (field is ExpressionField)
                        {
                            expCnt++;
                        }
                    }
                    if (value > expCnt)
                    {
                        int addCnt = value - expCnt;
                        for (int j = 0; j < addCnt; j++)
                        {
                            ExpressionField field = new ExpressionField();
                            field.HeaderText = "Expssion";
                            this.Fields.Add(field);
                        }
                    }
                    else if (value < expCnt)
                    {
                        value = expCnt;
                        if (this.Site != null)
                        {
                            System.Windows.Forms.MessageBox.Show("It only work for adding ExpressionField."
                            + "If you want to remove fields, please try to remove in 'Fields' property first!");
                        }
                    }
                    this.ViewState["ExpressionFieldCount"] = value;
                }
            }
        }

        public void ExecuteSync(GridViewCommandEventArgs e)
        {
            Control ctrl = (Control)e.CommandSource;
            while (ctrl != null && !(ctrl is GridViewRow))
            {
                ctrl = ctrl.Parent;
            }
            if (ctrl is GridViewRow)
            {
                GridViewRow row = (GridViewRow)ctrl;
                this.PageIndex = row.DataItemIndex;
                //added by lily 2006/4/27 for refresh data
                this.DataBind();
            }
        }

        protected override void InitializePager(DetailsViewRow row, PagedDataSource pagedDataSource)
        {
            base.InitializePager(row, pagedDataSource);
            if (row.RowType == DataControlRowType.Pager && CreateInnerNavigator)
            {
                // add WebNavigator for Add and Query
                #region Add Navigator
                InitNavControls(row);
                #endregion
            }
        }

        private WebNavigator nav = new WebNavigator();
        protected void InitNavControls(DetailsViewRow row)
        {
            nav.ID = "InPageNavigatorForAddAndQuery";
            nav.AddDefaultControls = false;
            string[] ctrlTexts = { "apply", "abort", "query" };
            if (this.GetServerText)
            {
                //language = CliSysMegLag.GetClientLanguage();
                language = CliUtils.fClientLang;
                String message = SysMsg.GetSystemMessage(language,
                                                         "Srvtools",
                                                         "WebDetailsView",
                                                         "ControlText", true);
                ctrlTexts = message.Split(';');
            }
            if (this.NavControls.Count == 0)
            {
                #region add default controls
                // Add Apply Control
                ControlItem ApplyItem = new ControlItem
                    ("Apply", ctrlTexts[0], this.InnerNavigatorShowStyle, "../image/uipics/apply.gif", "../image/uipics/apply2.gif", "../image/uipics/apply3.gif", 26, true);
                this.NavControls.Add(ApplyItem);
                // Add Abort Control
                ControlItem AbortItem = new ControlItem
                    ("Abort", ctrlTexts[1], this.InnerNavigatorShowStyle, "../image/uipics/abort.gif", "../image/uipics/abort2.gif", "../image/uipics/abort3.gif", 26, true);
                this.NavControls.Add(AbortItem);
                // Add Query Control
                ControlItem QueryItem = new ControlItem
                    ("Query", ctrlTexts[2], this.InnerNavigatorShowStyle, "../image/uipics/query.gif", "../image/uipics/query2.gif", "../image/uipics/query3.gif", 26, true);
                this.NavControls.Add(QueryItem);
                #endregion
            }
            nav.ShowDataStyle = WebNavigator.NavigatorStyle.DetailStyle;
            nav.BindingObject = this.ID;
            nav.LinkLable = this.InnerNavigatorLinkLabel;
            foreach (WebQueryField f in this.QueryFields)
            {
                nav.QueryFields.Add(f);
            }
            foreach (ControlItem item in this.NavControls)
            {
                if (item.ControlName == "Apply")
                    item.ControlText = ctrlTexts[0];
                else if (item.ControlName == "Abort")
                    item.ControlText = ctrlTexts[1];
                else if (item.ControlName == "Query")
                    item.ControlText = ctrlTexts[2];
                nav.NavControls.Add(item);
            }
            foreach (Control ctrl in row.Cells[0].Controls)
            {
                if (ctrl is Table)
                {
                    Table t = (Table)ctrl;
                    TableCell tc = new TableCell();
                    tc.Controls.Add(nav);
                    t.Rows[0].Cells.Add(tc);
                    if (this.PageCount == 1)
                    {
                        t.Rows[0].Cells[0].Controls.Clear();
                    }
                    break;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (this.AutoEmptyDataText)
                {
                    language = CliUtils.fClientLang;
                    this.EmptyDataText = SysMsg.GetSystemMessage(language, "Srvtools", "WebGridView", "EmptyText", true);
                }
            }
            object obj = GetObjByID(this.DataSourceID);
            if (obj == null)
                return;
            WebDataSource webDs = (WebDataSource)obj;
            string Filter = "";
            if (Page.Request.QueryString != null
                && Page.Request.QueryString["Filter"] != null && Page.Request.QueryString["DataSourceID"] == this.DataSourceID && !Page.IsPostBack)
            {
                Filter = Page.Request.QueryString["Filter"];
                webDs.WhereStr = Filter;

                //DetailsView details = new DetailsView();
                //if (webDs.MasterDataSource != null && webDs.MasterDataSource != "")
                //    return;

                //DataSet dataset = webDs.InnerDataSet;
                //WebDataSet wds = new WebDataSet();
                //wds.RemoteName = webDs.RemoteName;
                //wds.RealDataSet = dataset;
                //if (Filter.Trim() != "")
                //{
                //    wds.SetWhere(Filter, webDs.PacketRecords);
                //    webDs.LastKeyValues = wds.LastKeyValues;
                //}

                //webDs.InnerDataSet = wds.RealDataSet;

                // webDs.LastKeyValues = null;
                webDs.LastIndex = -1;
                webDs.InnerDataSet.Clear();
                webDs.InnerDataSet.Tables[0].ExtendedProperties.Clear();
                webDs.Eof = false;
            }
            bool isQueryBack = false;
            if (Page.Request.QueryString["IsQueryBack"] != null && Page.Request.QueryString["IsQueryBack"] != "")
            {
                isQueryBack = true;
            }
            if (webDs.PacketRecords == -1)
            {
                if (!Page.IsPostBack && isQueryBack && Page.Request.QueryString["DataSourceID"] == this.DataSourceID)
                {
                    if (webDs.MasterDataSource != null && webDs.MasterDataSource != "")
                        return;

                    webDs.SetWhere(Filter);
                }
            }
            else
            {
                int j = this.PageIndex;
                if (j == 0)
                {
                    if (isQueryBack && webDs.AlwaysClose)
                    {
                        webDs.AlwaysClose = false;
                        webDs.Eof = false;
                    }
                    if (ViewExist() && webDs.AlwaysClose)
                    {
                        return;
                    }
                    if (webDs.MasterDataSource == null || webDs.MasterDataSource.Length == 0)
                    {
                        int i = webDs.View != null ? webDs.View.Count : 0;
                        while (i <= 1 && (!webDs.Eof))
                        {
                            webDs.GetNextPacket();
                            i += webDs.PacketRecords;
                        }
                    }
                }
            }
            if (isQueryBack && !this.Page.IsPostBack)
                DataBind();
            base.OnLoad(e);
        }

        private bool ViewExist()
        {
            bool viewExist = false;
            WebNavigator nav = (WebNavigator)this.ExtendedFindChildControl(this.ID, FindControlType.BindingObject, typeof(WebNavigator));
            if (nav != null && nav.ViewBindingObject != null && nav.ViewBindingObject != "")
            {
                viewExist = true;
            }
            return viewExist;
        }

        [Browsable(false)]
        [DefaultValue(-1)]
        public int OldPageIndex
        {
            get
            {
                return ViewState["OldPageIndex"] == null ? -1 : (int)ViewState["OldPageIndex"];

            }
            set
            {
                ViewState["OldPageIndex"] = value;
            }
        }

        protected override void OnPageIndexChanging(DetailsViewPageEventArgs e)
        {
            OldPageIndex = this.PageIndex;
            base.OnPageIndexChanged(e);
            if (e.NewPageIndex == this.PageCount - 1)
            {
                Object o = (this.Page.Master == null) ? this.Page.FindControl(this.DataSourceID) : this.Parent.FindControl(this.DataSourceID);
                if (o != null)
                {
                    ((WebDataSource)o).GetNextPacket();
                }
            }
        }

        protected override void OnItemUpdating(DetailsViewUpdateEventArgs e)
        {
            WebValidate validate = (WebValidate)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
            bool findCtrl = (validate != null);
            IOrderedDictionary newvals = e.NewValues;
            IOrderedDictionary oldvals = e.OldValues;
            if (findCtrl)
            {
                validate.Text = string.Empty;
                object obj = this.GetObjByID(this.DataSourceID);
                if (obj is WebDataSource)
                {
                    WebDataSource wds = (WebDataSource)obj;
                    if (wds.PrimaryKey.Length > 0)
                    {
                        bool fequal = true;
                        object[] value = new object[wds.PrimaryKey.Length];
                        for (int i = 0; i < wds.PrimaryKey.Length; i++)
                        {
                            string columnName = wds.PrimaryKey[i].ColumnName;
                            if (e.NewValues.Contains(columnName))
                            {
                                if (e.NewValues[columnName] != null && !e.NewValues[columnName].Equals(e.OldValues[columnName]))
                                {
                                    fequal = false;
                                }
                                value[i] = e.NewValues[columnName];
                            }
                            else
                            {
                                throw new EEPException(EEPException.ExceptionType.ColumnValueNotFound, wds.GetType(), wds.ID, columnName, null);
                            }
                        }
                        if (!fequal)
                        {
                            if (!validate.CheckDuplicate(wds, value))
                            {
                                e.Cancel = true;
                                this.ValidateFailed = true;
                            }
                        }
                    }
                }
                AllValidateSucess = validate.CheckValidate(newvals);
                if (!AllValidateSucess)
                {
                    e.Cancel = true;
                    this.ValidateFailed = true;
                }
                else
                {
                    this.ValidateFailed = false;
                    this.RemoveValidateFlag(validate);
                }
            }
            base.OnItemUpdating(e);
        }

        [Browsable(false)]
        [DefaultValue(true)]
        public bool AllValidateSucess
        {
            get
            {
                object obj = this.ViewState["AllValidateSucess"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["AllValidateSucess"] = value;
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public bool ValidateFailed
        {
            get
            {
                object obj = this.ViewState["ValidateFailed"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["ValidateFailed"] = value;
            }
        }

        protected override void OnItemInserting(DetailsViewInsertEventArgs e)
        {
            WebValidate validate = (WebValidate)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
            bool findCtrl = (validate != null);
            IOrderedDictionary newvals = e.Values;
            object obj = this.GetObjByID(this.DataSourceID);
            if (obj is WebDataSource)
            {
                WebDataSource wds = (WebDataSource)obj;
                if (findCtrl)
                {
                    validate.Text = string.Empty;

                    if (wds.PrimaryKey.Length > 0)
                    {
                        object[] value = new object[wds.PrimaryKey.Length];
                        for (int i = 0; i < wds.PrimaryKey.Length; i++)
                        {
                            string columnName = wds.PrimaryKey[i].ColumnName;
                            if (e.Values.Contains(columnName))
                            {
                                value[i] = e.Values[columnName];
                            }
                            else
                            {
                                throw new Exception();//
                            }
                        }
                        if (!validate.CheckDuplicate(wds, value))
                        {
                            e.Cancel = true;
                            this.ValidateFailed = true;
                            return;
                        }
                    }
                    AllValidateSucess = validate.CheckValidate(newvals);
                    if (!AllValidateSucess)
                    {
                        e.Cancel = true;
                        this.ValidateFailed = true;
                    }
                    else
                    {
                        this.ValidateFailed = false;
                    }
                }

                WebTranslate tran = (WebTranslate)this.ExtendedFindChildControl(this.ID, FindControlType.BindingObject, typeof(WebTranslate));
                if (tran != null)
                {
                    tran.InsertKeyValues = string.Empty;//连续新增时需要清空
                    for (int i = 0; i < wds.PrimaryKey.Length; i++)
                    {
                        string columnName = wds.PrimaryKey[i].ColumnName;
                        if (e.Values.Contains(columnName))
                        {
                            string type = wds.View.Table.Columns[columnName].DataType.ToString().ToLower();
                            tran.InsertKeyValues += type + " " + e.Values[columnName].ToString() + ";";
                        }
                        else
                        {
                            throw new EEPException(EEPException.ExceptionType.ColumnValueNotFound, wds.GetType(), wds.ID, columnName, null);//
                        }
                    }
                    if (tran.InsertKeyValues != "")
                    {
                        tran.InsertKeyValues = tran.InsertKeyValues.Substring(0, tran.InsertKeyValues.LastIndexOf(";"));
                    }
                }
            }

            base.OnItemInserting(e);
        }

        protected override void OnItemDeleting(DetailsViewDeleteEventArgs e)
        {
            WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
            if (ds.AllowDelete)
            {
                base.OnItemDeleting(e);
                if (ds.IsEmpty)
                {
                    e.Cancel = true;
                }
                if (ds.AutoRecordLock)
                {
                    object[] value = new object[ds.PrimaryKey.Length];
                    for (int i = 0; i < ds.PrimaryKey.Length; i++)
                    {
                        string columnName = ds.PrimaryKey[i].ColumnName;
                        if (e.Values.Contains(columnName))
                        {
                            value[i] = e.Values[columnName];
                        }
                        else if (ds.RelationValues != null && ds.RelationValues.Contains(columnName))
                        {
                            value[i] = ds.RelationValues[columnName];
                        }
                        else
                        {
                            throw new EEPException(EEPException.ExceptionType.ColumnValueNotFound, ds.GetType(), ds.ID, columnName, null);
                        }
                    }
                    if (!ds.AddLock("Deleting", value))
                    {
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                language = CliUtils.fClientLang;
                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToDelete", true);
                Page.Response.Write("<script>alert('" + message + "');</script>");
                e.Cancel = true;
            }
        }

        protected override void OnModeChanging(DetailsViewModeEventArgs e)
        {
            if (e.NewMode == DetailsViewMode.Insert)
            {
                WebDataSource source = null;
                string sourceID = DataSourceID;
                if (sourceID.Length != 0)
                {
                    source = (WebDataSource)(GetObjByID(sourceID));
                }

                DataSet dataset = source.InnerDataSet;
                if (dataset != null)
                {
                    if (dataset.GetChanges() != null)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "InsertedNotApply", "<script>alert('You must first apply.')</script>");
                        this.ChangeMode(DetailsViewMode.ReadOnly);
                        return;
                    }
                }
            }

            WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
            WebValidate validate = (WebValidate)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
            if (e.NewMode == DetailsViewMode.Edit)
            {
                if (ds.AllowUpdate)
                {
                    if (ds.IsEmpty)
                    {
                        e.Cancel = true;
                        return;
                    }
                    base.OnModeChanging(e);
                    if (validate != null)
                        SetValidateFlag(validate);
                    WebNavigator nav = this.GetBindingNavigator();
                    if (nav != null)
                    {
                        nav.SetState(WebNavigator.NavigatorState.Editing);
                        nav.SetNavState("Editing");

                    }
                }
                else
                {
                    language = CliUtils.fClientLang;
                    String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToUpdate", true);
                    Page.Response.Write("<script>alert('" + message + "');</script>");
                    e.Cancel = true;
                }
            }
            else if (e.NewMode == DetailsViewMode.Insert)
            {
                if (ds.AllowAdd)
                {
                    base.OnModeChanging(e);
                    if (validate != null)
                        SetValidateFlag(validate);
                    OnAdding(new EventArgs());
                    WebNavigator nav = this.GetBindingNavigator();
                    if (nav != null)
                    {
                        nav.SetState(WebNavigator.NavigatorState.Inserting);
                        nav.SetNavState("Inserting");
                    }
                }
                else
                {
                    language = CliUtils.fClientLang;
                    String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToAdd", true);
                    Page.Response.Write("<script>alert('" + message + "');</script>");
                    e.Cancel = true;
                }
            }
            else if (e.NewMode == DetailsViewMode.ReadOnly)
            {
                if (validate != null)
                {
                    validate.Text = string.Empty;
                    RemoveValidateFlag(validate);
                }
                base.OnModeChanging(e);
                WebNavigator nav = this.GetBindingNavigator();
                if (nav != null)
                {
                    if (this.DataHasChanged)
                    {
                        nav.SetState(WebNavigator.NavigatorState.Changed);
                        nav.SetNavState("Changing");
                    }
                    else
                    {
                        nav.SetState(WebNavigator.NavigatorState.Browsing);
                        nav.SetNavState("Browsed");
                    }
                }
            }
        }

        public WebNavigator GetBindingNavigator()
        {
            return (WebNavigator)this.ExtendedFindChildControl(this.ID, FindControlType.BindingObject, typeof(WebNavigator));
        }

        public void SetValidateFlag(WebValidate validate)
        {
            if (validate.ValidateActive)
            {
                foreach (DataControlField field in this.Fields)
                {
                    if (this.ViewState["ValidateOrigColor"] == null)
                    {
                        this.ViewState.Add("ValidateOrigColor", ((DataControlField)field).HeaderStyle.ForeColor);
                    }
                    if (field is BoundField)
                    {
                        foreach (ValidateFieldItem vfi in validate.Fields)
                        {
                            if (vfi.FieldName == ((BoundField)field).DataField)
                            {
                                ((BoundField)field).HeaderStyle.ForeColor = validate.ValidateColor;
                                if (((BoundField)field).HeaderText.IndexOf(validate.ValidateChar) != 0)
                                    ((BoundField)field).HeaderText = ((BoundField)field).HeaderText.Insert(0, validate.ValidateChar);
                            }
                        }
                    }
                    else if (field is TemplateField)
                    {
                        foreach (ValidateFieldItem vfi in validate.Fields)
                        {
                            if (vfi.FieldName == ((TemplateField)field).SortExpression)
                            {
                                ((TemplateField)field).HeaderStyle.ForeColor = validate.ValidateColor;
                                if (((TemplateField)field).HeaderText.IndexOf(validate.ValidateChar) != 0)
                                    ((TemplateField)field).HeaderText = ((TemplateField)field).HeaderText.Insert(0, validate.ValidateChar);
                            }
                        }
                    }
                }
            }
        }

        public void RemoveValidateFlag(WebValidate validate)
        {
            if (validate.ValidateActive)
            {
                foreach (DataControlField field in this.Fields)
                {
                    if (field is BoundField)
                    {
                        foreach (ValidateFieldItem vfi in validate.Fields)
                        {
                            if (vfi.FieldName == ((BoundField)field).DataField)
                            {
                                if (this.ViewState["ValidateOrigColor"] != null)
                                    ((BoundField)field).HeaderStyle.ForeColor = (Color)this.ViewState["ValidateOrigColor"];
                                else
                                    ((BoundField)field).HeaderStyle.ForeColor = SystemColors.WindowText;
                                if (((BoundField)field).HeaderText.IndexOf(validate.ValidateChar) != -1 && validate.ValidateChar != "")
                                {
                                    ((BoundField)field).HeaderText = ((BoundField)field).HeaderText.Replace(validate.ValidateChar, "");
                                }
                            }
                        }
                    }
                    else if (field is TemplateField)
                    {
                        foreach (ValidateFieldItem vfi in validate.Fields)
                        {
                            if (vfi.FieldName == ((TemplateField)field).SortExpression)
                            {
                                if (this.ViewState["ValidateOrigColor"] != null)
                                    ((TemplateField)field).HeaderStyle.ForeColor = (Color)this.ViewState["ValidateOrigColor"];
                                else
                                    ((TemplateField)field).HeaderStyle.ForeColor = SystemColors.WindowText;
                                if (((TemplateField)field).HeaderText.IndexOf(validate.ValidateChar) != -1 && validate.ValidateChar != "")
                                {
                                    ((TemplateField)field).HeaderText = ((TemplateField)field).HeaderText.Replace(validate.ValidateChar, "");
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void InitializeRow(DetailsViewRow row, DataControlField field)
        {
            base.InitializeRow(row, field);
            object obj = this.GetObjByID(this.DataSourceID);
            WebDataSource wds = null;
            if (obj != null)
            {
                wds = (WebDataSource)obj;
            }
            if (this.Site == null)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (field is ExpressionField)
                    {
                        ExpressionField expField = (ExpressionField)field;
                        Control ctrl = row.FindControl("ExpressionLabel" + expField.Expression + this.PageIndex);
                        if (ctrl != null && ctrl is Label)
                        {
                            string strExpression = ((Label)ctrl).Text;
                            if (obj is WebDataSource)
                            {
                                DataTable tab = wds.View.Table;
                                int i = this.DataItemIndex;
                                for (int j = 0; j <= i; j++)
                                {
                                    if (tab.Rows[j].RowState == DataRowState.Deleted)
                                    {
                                        i++;
                                    }
                                }
                                ((Label)ctrl).Text = tab.Rows[i][strExpression].ToString();
                            }
                        }
                    }
                    if (field is CommandField || field is TemplateField)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            foreach (Control ctrl in cell.Controls)
                            {
                                if (ctrl is IButtonControl)
                                {
                                    IButtonControl btn = (IButtonControl)ctrl;
                                    if (btn.CommandName == "Edit")
                                    {
                                        if (wds != null && !wds.AllowUpdate)
                                        {
                                            ((WebControl)btn).Visible = false;
                                        }
                                    }
                                    else if (btn.CommandName == "Delete")
                                    {
                                        if (wds != null && !wds.AllowDelete)
                                        {
                                            ((WebControl)btn).Visible = false;
                                        }
                                    }
                                    else if (btn.CommandName == "New")
                                    {
                                        if (wds != null && !wds.AllowAdd)
                                        {
                                            ((WebControl)btn).Visible = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [Browsable(false)]
        [Obsolete("The property has been abolished", false)]
        public string KeyValues
        {
            get
            {
                return (ViewState["KeyValues"] == null ? "" : (string)ViewState["KeyValues"]);
            }
            set
            {
                ViewState["KeyValues"] = value;
            }
        }

        protected override void OnDataBound(EventArgs e)
        {
            base.OnDataBound(e);
            if (this.CurrentMode == DetailsViewMode.Insert)
            {
                #region Default和CarryOn赋值
                bool crRowExist = false;
                bool dvExist = false;
                object[,] carValues = null;
                object[,] defaultValues = null;
                WebDefault def = null;
                bool asExist = false;
                string autoseqvalue = "";
                string autoseqfield = "";
                Hashtable tableautoseq = new Hashtable();
                WebAutoSeq aus = new WebAutoSeq();
                if (this.Page.Form != null)
                {
                    def = (WebDefault)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebDefault));
                    List<object> preKeys = (List<object>)this.ViewState["PreAddKeys"];
                    List<object> preValues = (List<object>)this.ViewState["PreAddValues"];
                    if (def != null)
                    {
                        if (def.CarryOnActive && preKeys != null && preValues != null && preKeys.Count != 0 && preValues.Count != 0)
                        {
                            int i = preKeys.Count;
                            carValues = new object[i, 2];
                            for (int j = 0; j < i; j++)
                            {
                                carValues[j, 0] = preKeys[j];
                                carValues[j, 1] = preValues[j];
                            }
                            crRowExist = true;
                        }

                        if (def.DefaultActive)
                        {
                            defaultValues = def.GetDefaultValues();
                            dvExist = true;
                        }
                    }
                    //autoseq add by ccm
                    foreach (Control ctrl in this.Page.Form.Controls)
                    {
                        if (ctrl is WebAutoSeq && ((WebAutoSeq)ctrl).DataSourceID == this.DataSourceID && ((WebAutoSeq)ctrl).Active)
                        {
                            aus = (WebAutoSeq)ctrl;
                            autoseqvalue = aus.GetValue();
                            autoseqfield = aus.FieldName;
                            if (!tableautoseq.ContainsKey(autoseqfield))
                            {
                                tableautoseq.Add(autoseqfield, autoseqvalue);
                            }
                            asExist = true;
                        }
                    }
                    //end add
                }
                #endregion
                #region 开始赋值
                foreach (DetailsViewRow row in this.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow
                        && row.Cells.Count == 2)
                    {
                        // 用HeaderText获取row的FieldName
                        string FieldName = GetNameByHeaderText(row.Cells[0].Text);
                        object value = null;
                        if (dvExist)
                        {
                            int i = defaultValues.Length / 2;
                            for (int j = 0; j < i; j++)
                            {
                                // 比较FieldName和需要default的FieldName，如果相同则将此Field的default值赋给value变量
                                if (FieldName == defaultValues[j, 0].ToString() && defaultValues[j, 1] != null && defaultValues[j, 1].ToString() != "")
                                {
                                    value = defaultValues[j, 1];
                                }
                            }
                        }
                        if (crRowExist)
                        {
                            int i = carValues.Length / 2;
                            for (int j = 0; j < i; j++)
                            {
                                if (def.Fields[FieldName] != null &&
                                    ((DefaultFieldItem)def.Fields[FieldName]).CarryOn &&
                                    FieldName == carValues[j, 0].ToString())
                                {
                                    value = carValues[j, 1];
                                }
                            }
                        }
                        if (asExist)
                        {
                            if (tableautoseq.ContainsKey(FieldName))
                            {
                                value = tableautoseq[FieldName];
                            }
                            //if (FieldName == autoseqfield)
                            //{
                            //    value = autoseqvalue;
                            //}
                        }
                        // 带key值给detail
                        if (this.Page != null && this.Page.Form != null)
                        {
                            WebDataSource datasource = this.GetObjByID(this.DataSourceID) as WebDataSource;
                            if (datasource != null && !string.IsNullOrEmpty(datasource.MasterDataSource) 
                                && datasource.RelationValues.Contains(FieldName))
                            {
                                value = datasource.RelationValues[FieldName];
                            }
                        }

                        foreach (Control ctrl in row.Cells[1].Controls)
                        {
                            if (value != null)
                            {
                                if (ctrl is TextBox)
                                {
                                    ((TextBox)ctrl).Text = value.ToString();
                                }
                                if (ctrl is Label)
                                {
                                    ((Label)ctrl).Text = value.ToString();
                                }
                                if (ctrl is DropDownList)
                                {
                                    ((DropDownList)ctrl).SelectedValue = value.ToString();
                                }
                                if (ctrl is CheckBox)
                                {
                                    ((CheckBox)ctrl).Checked = Convert.ToBoolean(value);
                                    if (ctrl is WebCheckBox)
                                    {
                                        (ctrl as WebCheckBox).RefreshBindingValue();
                                    }
                                }
                                if (ctrl is WebRefValBase)
                                {
                                    ((WebRefValBase)ctrl).BindingValue = value.ToString();
                                }
                                if (ctrl is IDateTimePicker)
                                {
                                    if (((IDateTimePicker)ctrl).DateTimeType == dateTimeType.DateTime)
                                    {
                                        ((IDateTimePicker)ctrl).Text = value.ToString();
                                    }
                                    else if (((IDateTimePicker)ctrl).DateTimeType == dateTimeType.VarChar)
                                    {
                                        try
                                        {
                                            DateTime dt = Convert.ToDateTime(value);
                                            ((IDateTimePicker)ctrl).DateString = dt.ToString("yyyyMMdd");
                                        }
                                        catch
                                        {
                                            ((IDateTimePicker)ctrl).DateString = value.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            if (!DesignMode)
            {
                if (this.CurrentMode == DetailsViewMode.Edit)
                {
                    WebDataSource ds = this.GetObjByID(this.DataSourceID) as WebDataSource;
                    if (ds.AutoRecordLock)
                    {
                        object[] value = new object[ds.PrimaryKey.Length];
                        DataRow row = (this.DataItem as DataRowView).Row;
                        for (int i = 0; i < ds.PrimaryKey.Length; i++)
                        {
                            string columnName = ds.PrimaryKey[i].ColumnName;
                           
                            if (row.Table.Columns.Contains(columnName))
                            {
                                value[i] = row[columnName];
                            }
                            else
                            {
                                throw new EEPException(EEPException.ExceptionType.ColumnValueNotFound, ds.GetType(), ds.ID, columnName, null);
                            }
                        }
                        if (!ds.AddLock("Updating", value))
                        {
                            if (OldPageIndex != -1)
                            {
                                this.PageIndex = OldPageIndex;
                                return;
                            }
                            else
                            {
                                this.ChangeMode(DetailsViewMode.ReadOnly);
                                WebNavigator nav = this.GetBindingNavigator();
                                if (nav != null)
                                {
                                    nav.SetState(WebNavigator.NavigatorState.Browsing);
                                    nav.SetNavState("Browsed");
                                }
                                return;
                            }
                        }
                    }
                    // WebSecColumns
                    List<Control> lWebSecColumns = this.ExtendedFindChildControls(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                    //WebSecColumns webSecColumns = (WebSecColumns)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                    if (lWebSecColumns != null && lWebSecColumns.Count >0)
                    {
                        foreach (var item in lWebSecColumns)
                        {
                            WebSecColumns webSecColumns = item as WebSecColumns;
                            foreach (WebSecControl secCtrl in webSecColumns.WebSecControls)
                            {
                                if (secCtrl.ControlParent == this.ID && secCtrl.ControlTemplate == "EditItemTemplate")
                                {
                                    Control sec = this.FindControl(secCtrl.ControlID);
                                    if (sec != null)
                                    {
                                        sec.Visible = webSecColumns.ColumnsVisible;
                                        Type type = sec.GetType();
                                        PropertyInfo proInfo = type.GetProperty("ReadOnly");
                                        if (proInfo != null)
                                        {
                                            proInfo.SetValue(sec, webSecColumns.ColumnsReadOnly, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (this.CurrentMode == DetailsViewMode.ReadOnly)
                {
                    OldPageIndex = -1;
                    // WebSecColumns
                    List<Control> lWebSecColumns = this.ExtendedFindChildControls(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                    //WebSecColumns webSecColumns = (WebSecColumns)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                    if (lWebSecColumns != null && lWebSecColumns.Count > 0)
                    {
                        foreach (var item in lWebSecColumns)
                        {
                            WebSecColumns webSecColumns = item as WebSecColumns;
                            foreach (WebSecControl secCtrl in webSecColumns.WebSecControls)
                            {
                                if (secCtrl.ControlParent == this.ID && secCtrl.ControlTemplate == "ItemTemplate")
                                {
                                    Control sec = this.FindControl(secCtrl.ControlID);
                                    if (sec != null)
                                    {
                                        sec.Visible = webSecColumns.ColumnsVisible;
                                        Type type = sec.GetType();
                                        PropertyInfo proInfo = type.GetProperty("ReadOnly");
                                        if (proInfo != null)
                                        {
                                            proInfo.SetValue(sec, webSecColumns.ColumnsReadOnly, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (this.CurrentMode == DetailsViewMode.Insert)
                {
                    List<Control> lWebSecColumns = this.ExtendedFindChildControls(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                    //WebSecColumns webSecColumns = (WebSecColumns)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                    if (lWebSecColumns != null && lWebSecColumns.Count > 0)
                    {
                        foreach (var item in lWebSecColumns)
                        {
                            WebSecColumns webSecColumns = item as WebSecColumns;
                            foreach (WebSecControl secCtrl in webSecColumns.WebSecControls)
                            {
                                if (secCtrl.ControlParent == this.ID && secCtrl.ControlTemplate == "InsertItemTemplate")
                                {
                                    Control sec = this.FindControl(secCtrl.ControlID);
                                    if (sec != null)
                                    {
                                        sec.Visible = webSecColumns.ColumnsVisible;
                                        Type type = sec.GetType();
                                        PropertyInfo proInfo = type.GetProperty("ReadOnly");
                                        if (proInfo != null)
                                        {
                                            proInfo.SetValue(sec, webSecColumns.ColumnsReadOnly, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (InsertBack)
            {
                InsertBack = false;
                this.PageIndex = this.PageCount - 1;
                OnAfterInsertLocate(EventArgs.Empty);
            }
        }

        public event EventHandler AfterInsertLocate
        {
            add { Events.AddHandler(EventAfterInsertLocate, value); }
            remove { Events.RemoveHandler(EventAfterInsertLocate, value); }
        }

        private static readonly object EventAfterInsertLocate = new object();

        protected virtual void OnAfterInsertLocate(EventArgs e)
        {
            EventHandler AfterInsertLocateHandler = (EventHandler)Events[EventAfterInsertLocate];
            if (AfterInsertLocateHandler != null)
            {
                AfterInsertLocateHandler(this, e);
            }
        }

        private string GetNameByHeaderText(string Header)
        {
            string RetValue = "";
            foreach (DataControlField field in this.Fields)
            {
                if (field.HeaderText == Header)
                {
                    return field.SortExpression;
                }
            }
            return RetValue;
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public bool InsertBack
        {
            get
            {
                object obj = this.ViewState["InsertBack"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["InsertBack"] = value;
            }
        }

        protected override void OnItemUpdated(DetailsViewUpdatedEventArgs e)
        {
            base.OnItemUpdated(e);
            WebNavigator nav = this.GetBindingNavigator();
            if (nav != null)
            {
                WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                if (ds.AutoApply)
                {
                    nav.SetState(WebNavigator.NavigatorState.ApplySucess);
                    nav.SetNavState("Browsed");
                }
                else
                {
                    nav.SetState(WebNavigator.NavigatorState.Changed);
                    nav.SetNavState("Changing");
                }
            }
        }

        protected override void OnItemDeleted(DetailsViewDeletedEventArgs e)
        {
            base.OnItemDeleted(e);
            WebNavigator nav = this.GetBindingNavigator();
            if (nav != null)
            {
                WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                if (ds.AutoApply)
                {
                    nav.SetState(WebNavigator.NavigatorState.ApplySucess);
                    nav.SetNavState("Browsed");
                }
                else
                {
                    nav.SetState(WebNavigator.NavigatorState.Changed);
                    nav.SetNavState("Changing");
                }
            }
        }

        internal List<object> PreAddKeys = new List<object>();
        internal List<object> PreAddValues = new List<object>();
        protected override void OnItemInserted(DetailsViewInsertedEventArgs e)
        {
            WebDefault def = (WebDefault)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebDefault));
            if (def != null && def.CarryOnActive)
            {
                IOrderedDictionary values = e.Values;
                IDictionaryEnumerator dicEnum = values.GetEnumerator();
                PreAddKeys.Clear();
                PreAddValues.Clear();
                while (dicEnum.MoveNext())
                {
                    PreAddKeys.Add(dicEnum.Key);
                    PreAddValues.Add(dicEnum.Value);
                }
                this.ViewState["PreAddKeys"] = PreAddKeys;
                this.ViewState["PreAddValues"] = PreAddValues;
            }
            base.OnItemInserted(e);
            InsertBack = true;

            WebNavigator nav = this.GetBindingNavigator();
            if (nav != null)
            {
                WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                if (ds.AutoApply)
                {
                    if (ds.AutoApplyForInsert)
                    {
                        nav.SetState(WebNavigator.NavigatorState.ApplySucess);
                        nav.SetNavState("Browsed");
                    }
                    else
                    {
                        if (ds.InnerDataSet.Tables[0].ChildRelations.Count > 0)
                        {
                            nav.SetState(WebNavigator.NavigatorState.Changed);
                            nav.SetNavState("Changing");
                        }
                        else
                        {
                            nav.SetState(WebNavigator.NavigatorState.ApplySucess);
                            nav.SetNavState("Browsed");
                        }
                    }
                }
                else
                {
                    nav.SetState(WebNavigator.NavigatorState.Changed);
                    nav.SetNavState("Changing");
                }
            }
        }
    }

    #region WebDetailsViewDesigner
    public class WebDetailsViewDesigner : DetailsViewDesigner
    {
        protected override void OnSchemaRefreshed()
        {
            WebDetailsView wDetailsView = (WebDetailsView)this.Component;
            if (!wDetailsView.AutoGenerateEditButton && !wDetailsView.AutoGenerateDeleteButton && !wDetailsView.AutoGenerateInsertButton)
            {
                CommandField commandField = null;
                DataControlFieldCollection fields = wDetailsView.Fields;
                if (fields != null && fields.Count != 0)
                {
                    foreach (object f in fields)
                    {
                        if (f is CommandField)
                            commandField = (CommandField)f;
                    }
                }
                base.OnSchemaRefreshed();
                if (commandField == null)
                {
                    commandField = new CommandField();
                    commandField.ButtonType = ButtonType.Image;
                    commandField.EditImageUrl = "~/Image/UIPics/Edit.gif";
                    commandField.DeleteImageUrl = "~/Image/UIPics/Delete.gif";
                    commandField.NewImageUrl = "~/Image/UIPics/Add.gif";
                    commandField.UpdateImageUrl = "~/Image/UIPics/OK.gif";
                    commandField.CancelImageUrl = "~/Image/UIPics/Cancel.gif";
                    commandField.InsertImageUrl = "~/Image/UIPics/OK.gif";
                    commandField.ShowEditButton = true;
                    commandField.ShowDeleteButton = true;
                    commandField.ShowInsertButton = true;
                }
                if (!fields.Contains(commandField))
                {
                    fields.Add(commandField);
                    fields.SetDirty();
                }
            }
            else
            {
                base.OnSchemaRefreshed();
            }

            if (wDetailsView.Site != null)
            {
                object obj = wDetailsView.GetObjByID(wDetailsView.DataSourceID);
                if (obj is WebDataSource)
                {
                    DataSet Dset = DBUtils.GetDataDictionary(obj as WebDataSource, true);

                    if (Dset != null && Dset.Tables.Count > 0)
                    {
                        foreach (DataControlField field in wDetailsView.Fields)
                        {
                            if (field is BoundField)
                            {
                                int i = Dset.Tables[0].Rows.Count;
                                for (int j = 0; j < i; j++)
                                {
                                    if (string.Compare(Dset.Tables[0].Rows[j]["FIELD_NAME"].ToString(), ((BoundField)field).DataField, true) == 0)//IgnoreCase
                                    {
                                        string strCaption = Dset.Tables[0].Rows[j]["CAPTION"].ToString();
                                        if (((BoundField)field).HeaderText == ((BoundField)field).DataField && strCaption != "")
                                        {
                                            ((BoundField)field).HeaderText = strCaption;
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

        /*private DesignerActionListCollection actionLists;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                actionLists = base.ActionLists;
                actionLists.Add(new WebDetailsViewActionList(this.Component));
                return actionLists;
            }
        }*/
    }

    /*public class WebDetailsViewActionList : DesignerActionList
    {
        private WebDetailsView detailsview;

        public WebDetailsViewActionList(IComponent component)
            : base(component)
        {
            this.detailsview = component as WebDetailsView;
        }

        public bool CreateInnerNavigator
        {
            get
            {
                return detailsview.CreateInnerNavigator;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(WebDetailsView))["CreateInnerNavigator"].SetValue(detailsview, value);
            }
        }

        public WebNavigator.CtrlType InnerNavigatorShowStyle
        {
            get
            {
                return detailsview.InnerNavigatorShowStyle;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(WebDetailsView))["InnerNavigatorShowStyle"].SetValue(detailsview, value);
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        public DetailsViewInnerNavigatorCollection NavControls
        {
            get
            {
                return detailsview.NavControls;
            }
        }
    }*/
    #endregion

    #region DetailsViewInnerNavigatorCollection class
    [Editor(typeof(DetailsViewInnerNavigatorEditor), typeof(UITypeEditor))]
    public class DetailsViewInnerNavigatorCollection : InfoOwnerCollection
    {
        public bool bInit = false;
        public DetailsViewInnerNavigatorCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(ControlItem))
        {
            bInit = true;
        }

        public new ControlItem this[int index]
        {
            get
            {
                return (ControlItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ControlItem)
                    {
                        //原来的Collection设置为0
                        ((ControlItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((ControlItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
    #endregion

    #region DetailsViewInnerNavigatorEditor class
    internal class DetailsViewInnerNavigatorEditor : CollectionEditor
    {
        public DetailsViewInnerNavigatorEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(ControlItem);
        }

        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm form = base.CreateCollectionForm();
            form.Load += new EventHandler(form_Load);
            return form;
        }

        void form_Load(object sender, EventArgs e)
        {
            CollectionForm form = (CollectionForm)sender;
            if (form == null) return;
            if (((DetailsViewInnerNavigatorCollection)form.EditValue).Count == 0)
            {
                form.EditValue = InitControls((DetailsViewInnerNavigatorCollection)form.EditValue);
                GetAllControls(form.Controls);
                foreach (System.Windows.Forms.Control ctrl in AllControlsList)
                {
                    if (ctrl is System.Windows.Forms.Button && ctrl.Text == "&Add")
                    {
                        ((System.Windows.Forms.Button)ctrl).PerformClick();
                    }
                    if (ctrl is System.Windows.Forms.Button && ctrl.Text == "&Remove")
                    {
                        ((System.Windows.Forms.Button)ctrl).PerformClick();
                    }
                }
            }
        }

        List<System.Windows.Forms.Control> AllControlsList = new List<System.Windows.Forms.Control>();
        private void GetAllControls(System.Windows.Forms.Control.ControlCollection controlCollection)
        {
            foreach (System.Windows.Forms.Control ctrl in controlCollection)
            {
                AllControlsList.Add(ctrl);
                if (ctrl.Controls.Count > 0)
                {
                    GetAllControls(ctrl.Controls);
                }
            }
        }

        private DetailsViewInnerNavigatorCollection InitControls(DetailsViewInnerNavigatorCollection Ctrls)
        {
            #region add default controls
            // Add Apply Control
            ControlItem ApplyItem = new ControlItem
                ("Apply", "apply", WebNavigator.CtrlType.Image, "../image/uipics/apply.gif", "../image/uipics/apply2.gif", "../image/uipics/apply3.gif", 26, true);
            Ctrls.Add(ApplyItem);
            // Add Abort Control
            ControlItem AbortItem = new ControlItem
                ("Abort", "abort", WebNavigator.CtrlType.Image, "../image/uipics/abort.gif", "../image/uipics/abort2.gif", "../image/uipics/abort3.gif", 26, true);
            Ctrls.Add(AbortItem);
            // Add Query Control
            ControlItem QueryItem = new ControlItem
                ("Query", "query", WebNavigator.CtrlType.Image, "../image/uipics/query.gif", "../image/uipics/query2.gif", "../image/uipics/query3.gif", 26, true);
            Ctrls.Add(QueryItem);
            #endregion
            return Ctrls;
        }
    }
    #endregion
}