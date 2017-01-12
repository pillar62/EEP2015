using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Globalization;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Reflection;
using System.Web.UI.HtmlControls;

namespace Srvtools
{
    public abstract class BaseWebFormView : FormView, IBaseWebControl
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
            Control objContentPlaceHolder = this.DesignMode ? this.Page.FindControl("ContentPlaceHolder1") : this.Page.Form.FindControl("ContentPlaceHolder1");
            if (objContentPlaceHolder != null)
            {
                return this.FindChildControl(strid, objContentPlaceHolder, type, ReturnControlType);
            }
            else
            {
                if (this.DesignMode)
                    return this.FindChildControl(strid, this.Page, type, ReturnControlType);
                else
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

    [Designer(typeof(WebFormViewDesigner))]
    [ToolboxBitmap(typeof(WebFormView), "Resources.WebFormView.ico")]
    public class WebFormView : BaseWebFormView
    {
        public WebFormView()
        {
            this.DefaultMode = FormViewMode.ReadOnly;
            this.LayOutColNum = 1;
            _Fields = new FormViewFieldsCollection(this, typeof(FormViewField));
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string rscUrl = "../css/controls/WebFormView.css";
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

        private FormViewFieldsCollection _Fields;
        [Category("Infolight"),
         Description("Specifies the columns which WebDefault is applied to")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        public FormViewFieldsCollection Fields
        {
            get
            {
                return _Fields;
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

        private int _layOutColNum;
        [Category("Infolight"),
         Description("Specifies the amount of columns in template")]
        [DefaultValue(1)]
        public int LayOutColNum
        {
            get
            {
                return _layOutColNum;
            }
            set
            {
                _layOutColNum = value;
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
        protected override void OnPageIndexChanging(FormViewPageEventArgs e)
        {
            OldPageIndex = this.PageIndex;
            base.OnPageIndexChanging(e);
            if (e.NewPageIndex == this.PageCount - 1)
            {
                Object o = (this.Page.Master == null) ? this.Page.FindControl(this.DataSourceID) : this.Parent.FindControl(this.DataSourceID);
                if (o != null)
                {
                    ((WebDataSource)o).GetNextPacket();
                }
            }
        }

        protected override void OnPageIndexChanged(EventArgs e)
        {
            base.OnPageIndexChanged(e);
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

        protected override void OnItemInserting(FormViewInsertEventArgs e)
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
                                throw new EEPException(EEPException.ExceptionType.ColumnValueNotFound, wds.GetType(), wds.ID, columnName, null);
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
                        return;
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

        protected override void OnItemUpdating(FormViewUpdateEventArgs e)
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
                    return;
                }
                else
                {
                    this.ValidateFailed = false;
                }
            }
            base.OnItemUpdating(e);
        }

        protected override void OnItemDeleting(FormViewDeleteEventArgs e)
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
                //language = CliSysMegLag.GetClientLanguage();
                language = CliUtils.fClientLang;
                String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToDelete", true);
                Page.Response.Write("<script>alert('" + message + "');</script>");
                e.Cancel = true;
            }
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
        [DefaultValue("")]
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

        public void ExecuteSync(GridViewCommandEventArgs e)
        {
            Control ctrl = (Control)e.CommandSource;
            if (ctrl is GridView)
            {
                this.PageIndex = Convert.ToInt32(e.CommandArgument);
                this.DataBind();
            }
            else
            {
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
        }

        protected override void OnItemCreated(EventArgs e)
        {
            if (!this.DesignMode)
            {
                Control ctrl = this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
                if (ctrl == null || !(ctrl is WebValidate))
                    return;
                WebValidate validate = (WebValidate)ctrl;
                if (validate.ValidateActive)
                {
                    foreach (ValidateFieldItem vfi in validate.Fields)
                    {
                        if (!string.IsNullOrEmpty(vfi.ValidateLabelLink))
                        {
                            Label lbl = (Label)this.FindControl(vfi.ValidateLabelLink);
                            if (lbl != null)
                            {
                                lbl.ForeColor = validate.ValidateColor;
                                if (lbl.Text.IndexOf(validate.ValidateChar) != 0)
                                    lbl.Text = lbl.Text.Insert(0, validate.ValidateChar);
                            }
                        }
                    }
                }
            }
            base.OnItemCreated(e);
        }

        protected override void OnDataBound(EventArgs e)
        {
            base.OnDataBound(e);
            if (!this.DesignMode)
            {
                if (this.CurrentMode == FormViewMode.Insert)
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

                    AllCtrls.Clear();
                    GetAllCtrls(this.Controls);
                    foreach (FormViewField field in this.Fields)
                    {
                        object value = null;
                        if (dvExist)
                        {
                            int i = defaultValues.Length / 2;
                            for (int j = 0; j < i; j++)
                            {
                                // 比较FieldName和需要default的FieldName，如果相同则将此Field的default值赋给value变量
                                if (field.FieldName == defaultValues[j, 0].ToString() && defaultValues[j, 1] != null && defaultValues[j, 1].ToString() != "")
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
                                if (def.Fields[field.FieldName] != null &&
                                    ((DefaultFieldItem)def.Fields[field.FieldName]).CarryOn &&
                                    field.FieldName == carValues[j, 0].ToString())
                                {
                                    value = carValues[j, 1];
                                }
                            }
                        }
                        if (asExist)
                        {
                            if (tableautoseq.ContainsKey(field.FieldName))
                            {
                                value = tableautoseq[field.FieldName];
                            }
                            //if (field.FieldName == autoseqfield)
                            //{
                            //    value = autoseqvalue;
                            //}
                        }
                        // 带key值给detail

                        if (this.Page != null && this.Page.Form != null)
                        {
                            WebDataSource datasource = this.GetObjByID(this.DataSourceID) as WebDataSource;
                            if (datasource != null && datasource.RelationValues.Contains(field.FieldName))
                            {
                                value = datasource.RelationValues[field.Name];
                            }
                        }

                        foreach (Control ctrl in AllCtrls)
                        {
                            if (ctrl.ID == field.ControlID && value != null)
                            {
                                if (ctrl is TextBox)
                                {
                                    ((TextBox)ctrl).Text = value.ToString();
                                }
                                if (ctrl is HiddenField)
                                {
                                    ((HiddenField)ctrl).Value = value.ToString();
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
                    //WebSecColumns
                    List<Control> lWebSecColumns = (List<Control>)this.ExtendedFindChildControls(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                    //WebSecColumns webSecColumns = (WebSecColumns)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                    if (lWebSecColumns != null && lWebSecColumns.Count > 0)
                    {
                        foreach (var item in lWebSecColumns)
                        {
                            WebSecColumns webSecColumns = item as WebSecColumns;
                            foreach (WebSecControl secCtrl in webSecColumns.WebSecControls)
                            {
                                if (secCtrl.ControlParent == this.ID && secCtrl.ControlTemplateGroup == "InsertItemTemplate"
                                    && secCtrl.ControlTemplate == "InsertItemTemplate")
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
                else if (this.CurrentMode == FormViewMode.Edit)
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
                                this.ChangeMode(FormViewMode.ReadOnly);
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

                    //WebSecColumns
                    List<Control> lWebSecColumns = (List<Control>)this.ExtendedFindChildControls(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                    //WebSecColumns webSecColumns = (WebSecColumns)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                    if (lWebSecColumns != null && lWebSecColumns.Count > 0)
                    {
                        foreach (var item in lWebSecColumns)
                        {
                            WebSecColumns webSecColumns = item as WebSecColumns;
                            foreach (WebSecControl secCtrl in webSecColumns.WebSecControls)
                            {
                                if (secCtrl.ControlParent == this.ID && secCtrl.ControlTemplateGroup == "EditItemTemplate"
                                    && secCtrl.ControlTemplate == "EditItemTemplate")
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
                else if (this.CurrentMode == FormViewMode.ReadOnly)
                {
                    OldPageIndex = -1;
                    //WebSecColumns
                    List<Control> lWebSecColumns = (List<Control>)this.ExtendedFindChildControls(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                    //WebSecColumns webSecColumns = (WebSecColumns)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                    if (lWebSecColumns != null && lWebSecColumns.Count > 0)
                    {
                        foreach (var item in lWebSecColumns)
                        {
                            WebSecColumns webSecColumns = item as WebSecColumns;
                            foreach (WebSecControl secCtrl in webSecColumns.WebSecControls)
                            {
                                if (secCtrl.ControlParent == this.ID && secCtrl.ControlTemplateGroup == "ItemTemplate"
                                    && secCtrl.ControlTemplate == "ItemTemplate")
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
            object obj = this.GetObjByID(this.DataSourceID);
            if (obj != null && obj is WebDataSource)
            {
                WebDataSource wds = (WebDataSource)obj;
                if (wds.InnerDataSet != null)
                {
                    DataTable tab = wds.View.Table;
                    AllCtrls.Clear();
                    GetAllCtrls(this.Controls);
                    foreach (Control ctrl in AllCtrls)
                    {
                        if (ctrl is WebExpressionControl)
                        {
                            WebExpressionControl expCtrl = (WebExpressionControl)ctrl;
                            if (expCtrl.Expression != null && expCtrl.Expression != "")
                            {
                                foreach (DataColumn col in tab.Columns)
                                {
                                    if (expCtrl.Expression == col.ColumnName)
                                    {
                                        int i = this.DataItemIndex;
                                        for (int j = 0; j <= i; j++)
                                        {
                                            if (tab.Rows[j].RowState == DataRowState.Deleted)
                                            {
                                                i++;
                                            }
                                        }
                                        expCtrl.Text = tab.Rows[i][expCtrl.Expression].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /*public event EventHandler Default
        {
            add { Events.AddHandler(EventDefault, value); }
            remove { Events.RemoveHandler(EventDefault, value); }
        }

        private static readonly object EventDefault = new object();

        protected virtual void OnDefault(EventArgs e)
        {
            EventHandler DefaultHandler = (EventHandler)Events[EventDefault];
            if (DefaultHandler != null)
            {
                DefaultHandler(this, e);
            }
        }*/

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

        internal List<object> PreAddKeys = new List<object>();
        internal List<object> PreAddValues = new List<object>();
        protected override void OnItemInserted(FormViewInsertedEventArgs e)
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

        protected override void OnItemUpdated(FormViewUpdatedEventArgs e)
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

        protected override void OnItemDeleted(FormViewDeletedEventArgs e)
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

        private WebNavigator GetBindingNavigator()
        {
            return (WebNavigator)this.ExtendedFindChildControl(this.ID, FindControlType.BindingObject, typeof(WebNavigator));
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

        private List<Control> AllCtrls = new List<Control>();
        private void GetAllCtrls(ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                if (!AllCtrls.Contains(ctrl))
                    AllCtrls.Add(ctrl);
                GetAllCtrls(ctrl.Controls);
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
            //query时出现空记录而注释
            //if (this.BottomPagerRow != null)
            //{
            //    switch (this.PagerSettings.Position)
            //    {
            //        case PagerPosition.Bottom:
            //            this.BottomPagerRow.Visible = true;
            //            break;
            //        case PagerPosition.Top:
            //            this.TopPagerRow.Visible = true;
            //            break;
            //        case PagerPosition.TopAndBottom:
            //            this.TopPagerRow.Visible = true;
            //            this.BottomPagerRow.Visible = true;
            //            break;
            //    }
            //}
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
                    if (ViewExist() && webDs.AlwaysClose)
                    {
                        return;
                    }

                    if (isQueryBack && webDs.AlwaysClose)
                    {
                        webDs.AlwaysClose = false;
                        webDs.Eof = false;
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

        protected override void InitializeRow(FormViewRow row)
        {
            base.InitializeRow(row);
            if (!DesignMode && row.RowType == DataControlRowType.DataRow)
            {
                object obj = this.GetObjByID(this.DataSourceID);
                if (obj != null && obj is WebDataSource)
                {
                    WebDataSource wds = (WebDataSource)obj;
                    if (wds.InnerDataSet != null)
                    {
                        DataTable tab = wds.View.Table;
                        AllCtrls.Clear();
                        GetAllCtrls(this.Controls);
                        foreach (Control ctrl in AllCtrls)
                        {
                            if (ctrl is WebExpressionControl)
                            {
                                WebExpressionControl expCtrl = (WebExpressionControl)ctrl;
                                if (expCtrl.Expression != null && expCtrl.Expression != "")
                                {
                                    if (!tab.Columns.Contains(expCtrl.Expression))
                                    {
                                        DataColumn col = new DataColumn(expCtrl.Expression, typeof(decimal), expCtrl.Expression);
                                        tab.Columns.Add(col);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static readonly object EventOnAdding = new object();
        public event EventHandler Adding
        {
            add { base.Events.AddHandler(EventOnAdding, value); }
            remove { base.Events.RemoveHandler(EventOnAdding, value); }
        }

        public void OnAdding(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnAdding];
            if (handler != null)
            {
                handler(this, value);
            }
        }

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
    }

    public class WebFormViewDesigner : FormViewDesigner
    {
        private SYS_LANGUAGE language;

        public WebFormViewDesigner()
        {
            DesignerVerb ExportPanelVerb = new DesignerVerb("ExportPanel", new EventHandler(OnExportPanel));
            this.Verbs.Add(ExportPanelVerb);
            DesignerVerb ExportMultiLanguage = new DesignerVerb("Export to MultiLanguage", new EventHandler(OnExportMultiLanguage));
            this.Verbs.Add(ExportMultiLanguage);
        }

        public void OnExportPanel(object sender, EventArgs e)
        {
            string MenuID = null;
            string DB = null;

            foreach (System.Web.UI.Control c in this.Component.Site.Container.Components)
            {
                if (c is WebSecurity)
                {
                    MenuID = (c as WebSecurity).MenuID;
                    DB = (c as WebSecurity).DBAlias;
                    break;
                }
            }
            CliUtils.fLoginDB = DB;

            object[] paramDown = new object[1];
            paramDown[0] = MenuID;
            ArrayList listControlName = new ArrayList();
            object[] myRetDown = CliUtils.CallMethod("GLModule", "GetMenu", paramDown);
            if ((myRetDown != null) && (0 == (int)myRetDown[0]))
            {
                listControlName = (ArrayList)myRetDown[1];
            }

            foreach (TemplateGroup tempGroup in this.TemplateGroups)
            {
                foreach (TemplateDefinition tempDefin in tempGroup.Templates)
                {
                    if (tempDefin.Name == "EditItemTemplate" || tempDefin.Name == "InsertItemTemplate" || tempDefin.Name == "ItemTemplate")
                    {
                        StringBuilder builder = new StringBuilder();
                        string content = tempDefin.Content;
                        string[] ctrlTexts = content.Split("\r\n".ToCharArray());
                        for (int i = 0; i < ctrlTexts.Length; i++)
                        {
                            if (ctrlTexts[i].Trim().IndexOf("<asp:Panel") != -1)
                            {
                                bool flag = false;
                                string[] panel = ctrlTexts[i].Split(new string[1] { "asp:Panel id=\"" }, StringSplitOptions.None);
                                string panelName = null;
                                for (int x = 0; x < panel[1].Length; x++)
                                    if (panel[1][x] != '\"')
                                        panelName += panel[1][x];
                                    else
                                        break;
                                for (int j = 0; j < listControlName.Count; j++)
                                    if (panelName == listControlName[j].ToString())
                                    {
                                        flag = true;
                                        break;
                                    }
                                if (flag == false)
                                {
                                    object[] paramUp = new object[1];
                                    paramUp[0] = MenuID + ";" + panelName + ";;" + "Panel";
                                    object[] mySet = CliUtils.CallMethod("GLModule", "InsertToMenu", paramUp);
                                }
                            }
                        }
                    }
                }
            }
            CliUtils.fLoginDB = "";
        }

        public void OnExportMultiLanguage(object sender, EventArgs e)
        {

            string viewid = ((WebFormView)this.Component).ID;
            foreach (TemplateGroup tempGroup in this.TemplateGroups)
            {
                foreach (TemplateDefinition tempDefin in tempGroup.Templates)
                {
                    if (tempDefin.Name == "ItemTemplate")
                    {
                        try
                        {
                            string strtemp = EditionDifference.ActiveDocumentFullName() + ".tmp";
                            FileStream fs = new FileStream(strtemp, FileMode.OpenOrCreate);
                            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

                            string content = tempDefin.Content;
                            string[] ctrlTexts = System.Text.RegularExpressions.Regex.Split(content, "</td>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            for (int i = 0; i < ctrlTexts.Length; i++)
                            {
                                if (ctrlTexts[i].Trim().Contains("<asp:Label") && ctrlTexts[i].Trim().IndexOf("<%# Bind", StringComparison.OrdinalIgnoreCase) == -1)
                                {
                                    string id = ctrlTexts[i].Substring(ctrlTexts[i].IndexOf(" ID=\"", StringComparison.OrdinalIgnoreCase) + 5);
                                    id = id.Substring(0, id.IndexOf("\""));
                                    string text = ctrlTexts[i].Substring(ctrlTexts[i].IndexOf("Text=", StringComparison.OrdinalIgnoreCase) + 6);
                                    text = text.Substring(0, text.IndexOf("\""));
                                    sw.WriteLine(viewid + "." + id + ".HeadText=" + text);
                                }
                            }
                            // sw.Flush();
                            sw.Close();
                            // fs.Flush();
                            fs.Close();
                            File.SetAttributes(strtemp, FileAttributes.Hidden);
                            System.Windows.Forms.MessageBox.Show("Export to multilanguage success", "Info"
                                    , System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information);
                            //WebMultiLanguage wml = null;
                            //foreach (System.Web.UI.Control c in this.Component.Site.Container.Components)
                            //{
                            //    if (c is WebMultiLanguage)
                            //    {
                            //        wml = c as WebMultiLanguage;
                            //        break;
                            //    }
                            //}
                            //if (wml != null && wml.Active && wml.DataBase != "" && wml.DataBase != null)
                            //{
                            //    if (System.Windows.Forms.MessageBox.Show("MultiLanguge found, Start to Edit?", "Info"
                            //        , System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information)
                            //        == System.Windows.Forms.DialogResult.Yes)
                            //    {
                            //        frmWebMultiLanguageEditor fwm = new frmWebMultiLanguageEditor(wml);
                            //        fwm.ShowDialog();
                            //    }
                            //}
                        }
                        catch (Exception e1)
                        {
                            System.Windows.Forms.MessageBox.Show(e1.Message);
                        }
                    }
                }
            }
        }

        protected override void OnSchemaRefreshed()
        {
            WebFormView formView = (WebFormView)this.Component;
            if (formView.DataSourceID == null || formView.DataSourceID.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("DataSourceID property is null.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            if (this.InTemplateMode)
            {
                return;
            }

            //IDataSourceViewSchema schema = this.GetDataSourceSchema();
            //IDataSourceFieldSchema[] schemaArray = schema.GetFields();
            IDesignerHost host = (IDesignerHost)formView.Site.GetService(typeof(IDesignerHost));
            base.OnSchemaRefreshed();

            DataSet Dset = new DataSet();
            if (formView.Site.DesignMode)
            {
                object obj = formView.GetObjByID(formView.DataSourceID);
                if (obj is WebDataSource)
                {
                    Dset = DBUtils.GetDataDictionary(obj as WebDataSource, true);
                }
            }
            bool hasRef = true;
            foreach (TemplateGroup tempGroup in this.TemplateGroups)
            {
                foreach (TemplateDefinition tempDefin in tempGroup.Templates)
                {
                    string content = tempDefin.Content;
                    if (content.IndexOf("<table __designer") != -1)
                    {
                        hasRef = false;
                        break;
                    }
                }
            }
            if (!hasRef)
                return;

            foreach (TemplateGroup tempGroup in this.TemplateGroups)
            {
                foreach (TemplateDefinition tempDefin in tempGroup.Templates)
                {
                    if (tempDefin.Name == "EditItemTemplate" || tempDefin.Name == "InsertItemTemplate" || tempDefin.Name == "ItemTemplate")
                    {
                        StringBuilder builder = new StringBuilder();
                        string content = tempDefin.Content;
                        if (content == null || content.Length == 0)
                            continue;

                        string[] ctrlTexts = content.Split("\r\n".ToCharArray());
                        //Control[] ctrls = ControlParser.ParseControls(host, content);
                        int i = 0;
                        int j = 0;
                        int m = formView.LayOutColNum * 2;

                        List<string> lists = new List<string>();
                        foreach (string ctrlText in ctrlTexts)
                        {
                            if (ctrlText != null && ctrlText.Length != 0)
                            {
                                Control ctrl = ControlParser.ParseControl(host, ctrlText);
                                if (ctrl == null || ctrl is LinkButton)
                                    continue;

                                string[] ss = ctrlText.Split(@":<".ToCharArray());
                                lists.Add(ctrlText.Substring(0, ss[0].Length));
                                lists.Add(ctrlText.Substring(ss[0].Length + 1, ctrlText.Length - ss[0].Length - 7));
                                j++;
                            }
                        }
                        //foreach (Control ctrl in ctrls)
                        //{
                        //    if (ctrl == null || ctrl is LinkButton || ctrl is LiteralControl)
                        //        continue;
                        //    ControlDesigner designer = (ControlDesigner)host.GetDesigner(ctrl);
                        //    string ctrlText = designer.GetDesignTimeHtml();
                        //}
                        j = j * 2;

                        if (m > 0)
                        {
                            builder.Append("<table class='container_table'>");
                        }

                        foreach (string ctrlText in lists.ToArray())
                        {
                            if (ctrlText == null || ctrlText.Length == 0)
                                continue;

                            if (m > 0)
                            {
                                if (i % m == 0)
                                {
                                    builder.Append("<tr>");
                                }

                                builder.Append("<td>");
                            }
                            // add dd

                            string ddText = "";
                            string[] valstyle = GetValidateText(ctrlText);
                            ddText = GetDDText(ctrlText, Dset);

                            if (!ddText.Contains("runat=\"server\""))
                            {
                                //append label
                                //string colorhtml = "";
                                //if (tempDefin.Name != "ItemTemplate")
                                //{
                                //    ddText = valstyle[1] + ddText;
                                //    if (valstyle[0] != "")
                                //    {
                                //        colorhtml = "ForeColor=\"" + valstyle[0] + "\"";
                                //    }
                                //}
                                //builder.Append("<asp:Label ID=\"Caption");
                                //builder.Append(ctrlText + "\" runat=\"server\" " + colorhtml + " Text=\"");
                                //builder.Append(ddText + "\"></asp:Label>");

                                //Modified by andy 2007/3/28
                                string colorhtml = "";
                                if (tempDefin.Name != "ItemTemplate")
                                {
                                    ddText = valstyle[1] + ddText;
                                    if (valstyle[0] != "")
                                    {
                                        colorhtml = "ForeColor=\"" + valstyle[0] + "\"";
                                    }
                                    builder.Append("<asp:Label ID=\"Caption");
                                }
                                else
                                {
                                    builder.Append("<asp:Label ID=\"CaptionI");
                                }
                                builder.Append(ctrlText + "\" runat=\"server\" " + colorhtml + " Text=\"");
                                builder.Append(ddText + "\"></asp:Label>");

                                //end append label
                            }
                            else
                            {
                                builder.Append(ddText);
                            }
                            builder.Append("\r\n");

                            if (m > 0)
                            {
                                builder.Append("</td>");

                                if (i % m == m - 1)
                                {
                                    builder.Append("</tr>");
                                }
                            }
                            i++;
                        }

                        if (m > 0)
                        {
                            if (i % m != 0)
                            {
                                int n = m - (i % m);
                                int q = 0;
                                while (q < n)
                                {
                                    builder.Append("<td></td>");
                                    q++;
                                }
                                builder.Append("</tr>");
                            }
                            builder.Append("</table>");
                        }

                        tempDefin.Content = builder.ToString();
                    }
                }
            }
        }

        private string GetDDText(string ControlText, DataSet Dset)
        {
            string ctrlText = ControlText;

            WebFormView formView = (WebFormView)this.Component;
            IDesignerHost host = (IDesignerHost)formView.Site.GetService(typeof(IDesignerHost));
            bool b = false;
            if (ControlParser.ParseControl(host, ctrlText) == null)
                b = true;

            if (Dset == null || Dset.Tables.Count == 0)
            {
                goto Label1;
            }
            int x = Dset.Tables[0].Rows.Count;
            for (int y = 0; y < x; y++)
            {
                if (string.Compare(Dset.Tables[0].Rows[y]["FIELD_NAME"].ToString(), ctrlText, true) == 0)//IgnoreCase
                {
                    string strCaption = Dset.Tables[0].Rows[y]["CAPTION"].ToString();
                    if (strCaption != "")
                    {
                        ctrlText = strCaption;
                        break;
                    }
                }
            }
        Label1:
            if (b)
            {
                return ctrlText + ":";
            }
            else
            {
                return ctrlText;
            }
        }

        private string[] GetValidateText(string ControlText)
        {
            string fieldName = ControlText;
            string valcolor = "";
            string valchar = "";

            WebFormView formView = (WebFormView)this.Component;
            Control ctrl = formView.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
            if (ctrl != null && ctrl is WebValidate)
            {
                WebValidate wVal = (WebValidate)ctrl;
                if (wVal.ValidateActive)
                {
                    foreach (ValidateFieldItem vfi in wVal.Fields)
                    {
                        if (string.Compare(vfi.FieldName, fieldName, true) == 0)//IgnoreCase
                        {
                            valcolor = wVal.ValidateColor.Name;
                            valchar = wVal.ValidateChar;
                        }
                    }
                }
            }
            return new string[] { valcolor, valchar };
        }

        private IDataSourceViewSchema GetDataSourceSchema()
        {
            DesignerDataSourceView view = this.DesignerView;
            if (view != null)
            {
                return view.Schema;
            }
            return null;
        }

        private void CreateInnerNavigator(WebFormView formView, IDesignerHost host)
        {
            string[] ctrlTexts = { "<<", "<", ">", ">>", "add", "update", "delete", "ok", "cancel", "apply", "abort", "query", "print" };
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            String message = SysMsg.GetSystemMessage(language,
                                                     "Srvtools",
                                                     "WebNavigator",
                                                     "ControlText", true);
            ctrlTexts = message.Split(';');
            StringBuilder sbHeader = new StringBuilder();
            sbHeader.Append("<cc1:WebNavigator ID=\"WebNavigator1\" runat=\"server\">"); // BindingObject=\"" + formView.ID + "\"
            sbHeader.Append("<NavControls>");
            sbHeader.Append("<cc1:ControlItem ControlName=\"First\" ControlText=\"" + ctrlTexts[0] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/first.gif\" MouseOverImageUrl=\"../image/uipics/first2.gif\" Size=\"25\" />");
            sbHeader.Append("<cc1:ControlItem ControlName=\"Previous\" ControlText=\"" + ctrlTexts[1] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/previous.gif\" MouseOverImageUrl=\"../image/uipics/previous2.gif\" Size=\"25\" />");
            sbHeader.Append("<cc1:ControlItem ControlName=\"Next\" ControlText=\"" + ctrlTexts[2] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/next.gif\" MouseOverImageUrl=\"../image/uipics/next2.gif\" Size=\"25\" />");
            sbHeader.Append("<cc1:ControlItem ControlName=\"Last\" ControlText=\"" + ctrlTexts[3] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/last.gif\" MouseOverImageUrl=\"../image/uipics/last2.gif\" Size=\"25\" />");
            sbHeader.Append("<cc1:ControlItem ControlName=\"Add\" ControlText=\"" + ctrlTexts[4] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/add.gif\" MouseOverImageUrl=\"../image/uipics/add2.gif\" Size=\"25\" />");
            sbHeader.Append("<cc1:ControlItem ControlName=\"Update\" ControlText=\"" + ctrlTexts[5] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/edit.gif\" MouseOverImageUrl=\"../image/uipics/edit2.gif\" Size=\"25\" />");
            sbHeader.Append("<cc1:ControlItem ControlName=\"Delete\" ControlText=\"" + ctrlTexts[6] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/delete.gif\" MouseOverImageUrl=\"../image/uipics/delete2.gif\" Size=\"25\" />");
            sbHeader.Append("<cc1:ControlItem ControlName=\"OK\" ControlText=\"" + ctrlTexts[7] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/ok.gif\" MouseOverImageUrl=\"../image/uipics/ok2.gif\" Size=\"25\" />");
            sbHeader.Append("<cc1:ControlItem ControlName=\"Cancel\" ControlText=\"" + ctrlTexts[8] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/cancel.gif\" MouseOverImageUrl=\"../image/uipics/cancel2.gif\" Size=\"25\" />");
            sbHeader.Append("<cc1:ControlItem ControlName=\"Apply\" ControlText=\"" + ctrlTexts[9] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/apply.gif\" MouseOverImageUrl=\"../image/uipics/apply2.gif\" Size=\"25\" />");
            sbHeader.Append("<cc1:ControlItem ControlName=\"Abort\" ControlText=\"" + ctrlTexts[10] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/abort.gif\" MouseOverImageUrl=\"../image/uipics/abort2.gif\" Size=\"25\" />");
            sbHeader.Append("<cc1:ControlItem ControlName=\"Query\" ControlText=\"" + ctrlTexts[11] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/query.gif\" MouseOverImageUrl=\"../image/uipics/query2.gif\" Size=\"25\" />");
            sbHeader.Append("<cc1:ControlItem ControlName=\"Print\" ControlText=\"" + ctrlTexts[12] + "\" ControlType=\"Image\" ControlVisible=\"True\" ImageUrl=\"../image/uipics/print.gif\" MouseOverImageUrl=\"../image/uipics/print2.gif\" Size=\"25\" />");
            sbHeader.Append("</NavControls>");
            sbHeader.Append("</cc1:WebNavigator>");
            formView.HeaderTemplate = ControlParser.ParseTemplate(host, sbHeader.ToString());
        }

        private DesignerActionListCollection _actionLists;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                _actionLists = base.ActionLists;
                if (_actionLists != null)
                {
                    WebFormView formView = (WebFormView)this.Component;
                    IDesignerHost host = (IDesignerHost)formView.Site.GetService(typeof(IDesignerHost));
                    _actionLists.Add(new WebFormViewActionList(this.Component, this.Tag));
                }

                return _actionLists;
            }
        }
    }

    public class WebFormViewActionList : DesignerActionList
    {
        private WebFormView formView;
        IControlDesignerTag tag;

        public WebFormViewActionList(IComponent component, IControlDesignerTag designerTag)
            : base(component)
        {
            this.formView = component as WebFormView;
            this.tag = designerTag;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "CopyDefaultFields", "Copy Default Fields", true));
            return items;
        }

        public void CopyDefaultFields()
        {
            foreach (Control ctrl in formView.Page.Controls)
            {
                if (ctrl is WebDefault)
                {
                    WebDefault def = (WebDefault)ctrl;
                    object obj = formView.GetObjByID(formView.DataSourceID);
                    if (obj is WebDataSource)
                    {
                        WebDataSource wds = (WebDataSource)obj;
                        if (def.DataSourceID == formView.DataSourceID)//因为已经把View上面的DataMember都去掉了，所以不需要判断DataMember是否相同了  && def.DataMember == wds.DataMember)
                        {
                            //this.tag.SetDirty(true);
                            formView.Fields.Clear();
                            string content = this.tag.GetContent();
                            int indexinserts = content.IndexOf("<InsertItemTemplate");
                            int indexinserte = content.IndexOf("</InsertItemTemplate>");
                            if (indexinserts != -1 && indexinserte != -1)
                            {
                                content = content.Substring(indexinserts, indexinserte - indexinserts);
                            }

                            foreach (DefaultFieldItem field in def.Fields)
                            {
                                FormViewField fvf = new FormViewField();
                                fvf.FieldName = field.FieldName;
                                int index = content.IndexOf("<%# Bind(\"" + field.FieldName + "\") %>");
                                if (index != -1)
                                {
                                    string ctrlRegion = content.Substring(0, index);
                                    if (ctrlRegion.IndexOf(" ID=\"", StringComparison.OrdinalIgnoreCase) != -1)
                                    {
                                        StringBuilder builder = new StringBuilder(ctrlRegion);
                                        string ctrlID = ctrlRegion.Substring(ctrlRegion.LastIndexOf(" ID=\"", StringComparison.OrdinalIgnoreCase) + 5);
                                        ctrlID = ctrlID.Substring(0, ctrlID.IndexOf("\""));
                                        fvf.ControlID = ctrlID;
                                    }
                                }
                                formView.Fields.Add(fvf);
                            }
                            System.Windows.Forms.MessageBox.Show("copy fields success.");
                            this.tag.SetDirty(true);
                            break;
                        }
                    }
                }
            }
        }
    }

    #region FormViewFieldsCollection
    public class FormViewFieldsCollection : InfoOwnerCollection
    {
        public FormViewFieldsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(FormViewField))
        {
        }

        public new FormViewField this[int index]
        {
            get
            {
                return (FormViewField)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is FormViewField)
                    {
                        //原来的Collection设置为0
                        ((FormViewField)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((FormViewField)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FormViewField : InfoOwnerCollectionItem, IGetValues
    {
        public override string ToString()
        {
            return _FieldName;
        }

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
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
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

        private string _ControlID;
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
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

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            if (string.Compare(sKind, "fieldname", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebFormView)
                {
                    WebFormView formView = (WebFormView)this.Owner;
                    object obj = formView.GetObjByID(formView.DataSourceID);
                    if (obj is WebDataSource)
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
                        if (wds.DesignDataSet != null && wds.DesignDataSet.Tables.Contains(wds.DataMember))
                        {
                            DataTable tab = wds.DesignDataSet.Tables[wds.DataMember];
                            List<string> arrItems = new List<string>();
                            int i = tab.Columns.Count;
                            retList = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                retList[j] = tab.Columns[j].ColumnName;
                            }
                        }
                    }
                }
            }
            else if (string.Compare(sKind, "controlid", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebFormView)
                {
                    WebFormView formView = (WebFormView)this.Owner;
                    TemplateBuilder builder = (TemplateBuilder)formView.InsertItemTemplate;
                    IDesignerHost host = (IDesignerHost)formView.Site.GetService(typeof(IDesignerHost));
                    Control[] ctrls = ControlParser.ParseControls(host, builder.Text);
                    List<string> ctrlNames = new List<string>();
                    int i = ctrls.Length;
                    for (int j = 0; j < i; j++)
                    {
                        if (!(ctrls[j] is LiteralControl))
                        {
                            ctrlNames.Add(ctrls[j].ID);
                        }
                    }
                    int m = ctrlNames.Count;
                    retList = new string[m];
                    for (int n = 0; n < m; n++)
                    {
                        retList[n] = ctrlNames[n];
                    }
                }
            }
            return retList;
        }
    }
    #endregion
}
