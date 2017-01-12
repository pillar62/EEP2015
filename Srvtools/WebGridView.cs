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
using System.Web.UI.HtmlControls;
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
using EnvDTE;
using EnvDTE80;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Srvtools
{
    public enum FindControlType { BindingObject, DataSourceID, MasterDataSource, Self }

    public abstract class BaseWebGridView : GridView, IBaseWebControl
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

        internal Control ExtendedFindChildControl(string strid, FindControlType type, Type ReturnControlType)
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

    public class myColor
    {
        public string mark;
        public Color markColor;

        public myColor()
        {
            mark = "";
        }

        public myColor(string m, Color c)
        {
            mark = m;
            markColor = c;
        }
    }

    [Designer(typeof(WebGridViewDesigner))]
    [ToolboxBitmap(typeof(WebGridView), "Resources.WebGridView.ico")]
    public class WebGridView : BaseWebGridView, IGetValues
    {
        public WebGridView()
        {
            _AddNewRowControls = new AddNewRowControlCollection(this, typeof(AddNewRowControlItem));
            _NavControls = new GridViewInnerNavigatorCollection(this, typeof(ControlItem));
            _TotalColumns = new GridViewTotalCollection(this, typeof(GridViewTotalItem));
            _QueryFields = new WebQueryFiledsCollection(this, typeof(WebQueryField));
            _Params = new GridViewOpenParamCollection(this, typeof(GridViewOpenParam));
            this.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            AutoGenerateColumns = false;
            this.GetServerText = true;
            _ShowDialog = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string rscUrl = "../css/controls/WebGridView.css";
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

        [DefaultValue(false)]
        public override bool AutoGenerateColumns
        {
            get
            {
                return base.AutoGenerateColumns;
            }
            set
            {
                base.AutoGenerateColumns = value;
            }
        }

        private SYS_LANGUAGE language;

        public enum AddNewRowControlType
        {
            TextBox,
            HtmlInputText,
            Label,
            DropDownList,
            RefVal,
            CheckBox,
            DateTimePicker,
            WebImage,
            WebListBoxList,
            WebCheckBox,
            HiddenField
        }

        [DefaultValue(true)]
        public override bool AllowPaging
        {
            get
            {
                object obj = this.ViewState["AllowPaging"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                base.AllowPaging = value;
            }
        }

        private GridViewOpenParamCollection _Params;
        [Category("Infolight"),
        Description("Specifies the Paramters which are going to be sent during open mode adding.")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        public GridViewOpenParamCollection Params
        {
            get
            {
                return _Params;
            }
        }

        private Boolean FWizardDesignMode = false;
        [Browsable(false)]
        [DefaultValue(false)]
        public Boolean WizardDesignMode
        {
            get { return FWizardDesignMode; }
            set { FWizardDesignMode = value; }
        }


        private GridViewInnerNavigatorCollection _NavControls;
        [Category("Infolight"),
         Description("Specifies the Controls in the inner WebNavigator")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        public GridViewInnerNavigatorCollection NavControls
        {
            get
            {
                return _NavControls;
            }
        }

        private AddNewRowControlCollection _AddNewRowControls;
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        [Category("Infolight"),
         Description("Specifies the amount of columns in template")]
        public AddNewRowControlCollection AddNewRowControls
        {
            get
            {
                return _AddNewRowControls;
            }
        }

        private GridViewTotalCollection _TotalColumns;
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        [Category("Infolight"),
         Description("Specifies the columns need to totalize")]
        public GridViewTotalCollection TotalColumns
        {
            get
            {
                return _TotalColumns;
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
                if (this.Columns.Count > 0)
                {
                    int expCnt = 0;
                    foreach (DataControlField field in this.Columns)
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
                            this.Columns.Add(field);
                        }
                    }
                    else if (value < expCnt)
                    {
                        value = expCnt;
                        if (this.Site != null)
                        {
                            System.Windows.Forms.MessageBox.Show("It only works for adding ExpressionField."
                            + "If you want to remove columns, please try to remove in 'Columns' property first!");
                        }
                    }
                    this.ViewState["ExpressionFieldCount"] = value;
                }
            }
        }

        [Category("Infolight"),
        Description("Indicates whether the inner WebNavigator is created in the control"),
        DefaultValue(true)]
        public bool CreateInnerNavigator
        {
            get
            {
                object obj = this.ViewState["CreateInnerNavigator"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["CreateInnerNavigator"] = value;
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public bool SkipInsert
        {
            get
            {
                object obj = this.ViewState["SkipInsert"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["SkipInsert"] = value;
            }
        }

        [Category("Infolight"),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string MultiCheckColumn
        {
            get
            {
                object obj = this.ViewState["MultiCheckColumn"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["MultiCheckColumn"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string TotalCaption
        {
            get
            {
                object obj = this.ViewState["TotalCaption"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["TotalCaption"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(-1)]
        public int MultiCheckColumnIndexToTranslate
        {
            get
            {
                object obj = this.ViewState["MultiCheckColumnIndexToTranslate"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return -1;
            }
            set
            {
                this.ViewState["MultiCheckColumnIndexToTranslate"] = value;
            }
        }

        private bool _flActive = false;
        [Category("Infolight")]
        [DefaultValue(false)]
        public bool FLActive
        {
            get
            {
                return _flActive;
            }
            set
            {
                _flActive = value;
            }
        }

        private int _flActiveColumnIndex = 0;
        [Category("Infolight")]
        [DefaultValue(0)]
        public int FLActiveColumnIndex
        {
            get
            {
                return _flActiveColumnIndex;
            }
            set
            {
                _flActiveColumnIndex = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            List<string> arrItems = new List<string>();
            if (sKind.ToLower().Equals("multicheckcolumn"))
            {
                object obj = this.GetObjByID(this.DataSourceID);
                if (obj != null && obj is WebDataSource)
                {
                    WebDataSource wds = (WebDataSource)obj;
                    if (wds.DataMember != null && wds.DataMember != "")
                    {
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

                            foreach (DataColumn col in tab.Columns)
                            {
                                arrItems.Add(col.ColumnName);
                            }
                        }
                    }
                }
            }
            else if (string.Compare(sKind, "EditUrlPanel", true) == 0)
            {
                foreach (Control control in this.Page.Controls)
                {
                    if (control is IModalPanel)
                    {
                        arrItems.Add(control.ID);
                    }
                }
            }
            return arrItems.ToArray();
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

        [Category("Infolight"),
        Description("Indicate whether total is enabled or disabled")]
        [DefaultValue(false)]
        public bool TotalActive
        {
            get
            {
                object obj = this.ViewState["TotalActive"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["TotalActive"] = value;
                if (value)
                {
                    this.ShowFooter = value;
                }
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public bool GridInserting
        {
            get
            {
                object obj = this.ViewState["GridInserting"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["GridInserting"] = value;
            }
        }

        [Category("Infolight"),
         Description("Specifies the URL of page of edit")]
        [EditorAttribute(typeof(UrlEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string EditURL
        {
            get
            {
                object obj = this.ViewState["EditURL"];
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
                this.ViewState["EditURL"] = value;
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
        [DefaultValue(true)]
        public bool EditReturn
        {
            get
            {
                object obj = this.ViewState["EditReturn"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["EditReturn"] = value;
            }
        }

        [Category("Infolight"),
         Description("Indicates whether the caption of each column wrap or not")]
        [DefaultValue(true)]
        public bool HeaderStyleWrap
        {
            get
            {
                object obj = this.ViewState["HeaderStyleWrap"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["HeaderStyleWrap"] = value;
                foreach (DataControlField field in this.Columns)
                {
                    field.HeaderStyle.Wrap = value;
                }
            }
        }

        [Category("Infolight"),
         Description("Indicates whether identity field is added")]
        [DefaultValue(false)]
        public bool AddIndentityField
        {
            get
            {
                object obj = this.ViewState["AddIndentityField"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["AddIndentityField"] = value;
                if (value)
                {
                    bool fieldExist = false;
                    if (this.Columns.Count > 0)
                    {
                        foreach (DataControlField field in this.Columns)
                        {
                            if (field is IndentityField)
                            {
                                fieldExist = true;
                            }
                        }
                        if (!fieldExist)
                        {
                            IndentityField field = new IndentityField();
                            field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            this.Columns.Insert(0, field);
                        }
                    }
                }
                else
                {
                    int fieldIndex = -1;
                    int i = this.Columns.Count;
                    if (i > 0)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            if (this.Columns[j] is IndentityField)
                            {
                                fieldIndex = j;
                                break;
                            }
                        }
                    }
                    if (fieldIndex != -1)
                    {
                        this.Columns.RemoveAt(fieldIndex);
                    }
                }
            }
        }

        public enum OpenEditMode
        {
            Insert = 0,
            Update = 1,
            View = 2
        }

        private bool _ShowDialog;
        [DefaultValue(false)]
        public bool ShowDialog
        {
            get { return _ShowDialog; }
            set { _ShowDialog = value; }
        }


        [Category("Infolight"),
        DefaultValue(500),
        Description("Specifies the width of the page of edit")]
        public int OpenEditWidth
        {
            get
            {
                object obj = this.ViewState["OpenEditWidth"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 500;
            }
            set
            {
                this.ViewState["OpenEditWidth"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(400),
        Description("Specifies the height of the page of edit")]
        public int OpenEditHeight
        {
            get
            {
                object obj = this.ViewState["OpenEditHeight"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 400;
            }
            set
            {
                this.ViewState["OpenEditHeight"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(200)]
        public int OpenEditLeft
        {
            get
            {
                object obj = this.ViewState["OpenEditLeft"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 200;
            }
            set
            {
                this.ViewState["OpenEditLeft"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(200)]
        public int OpenEditTop
        {
            get
            {
                object obj = this.ViewState["OpenEditTop"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 200;
            }
            set
            {
                this.ViewState["OpenEditTop"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool OpenEditUrlInServerMode
        {
            get
            {
                object obj = this.ViewState["OpenEditUrlInServerMode"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["OpenEditUrlInServerMode"] = value;
            }
        }

#if AjaxTools
        [Category("Infolight")]
        [DefaultValue("")]
        public string UpdatePanelID
        {
            get
            {
                object obj = this.ViewState["UpdatePanelID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["UpdatePanelID"] = value;
            }
        }
#endif

        public void OpenEditURL(OpenEditMode mode)
        {
            OpenEditURL(mode, null);
        }

        public void OpenEditURL(OpenEditMode mode, object sender)
        {
            string url = this.getURL(mode, sender);
            string script = string.Empty;
            if (ShowDialog)
            {
                script = "window.showModalDialog('" + url + "','','dialogwidth=" + OpenEditWidth + "px;dialogheight="
                            + OpenEditHeight + "px,dialogleft=" + OpenEditLeft + "px,dialogtop=" + OpenEditTop
                            + "px'); return false;";
            }
            else
            {
                script = "window.open('" + url + "','Edit','height=" + OpenEditHeight + ",width=" + OpenEditWidth + ",top="
                    + OpenEditTop + ",left=" + OpenEditLeft + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no')";
            }
            RegisterAjaxScript(script);
        }

        private void RegisterAjaxScript(string script)
        {
#if AjaxTools
            object obj = this.GetObjByID(this.UpdatePanelID);
            if (obj != null && obj is UpdatePanel)
            {
                UpdatePanel panel = (UpdatePanel)obj;
                ScriptManager.RegisterStartupScript(panel, this.Page.GetType(), "ScriptBlock", script, true);
            }
            else
            {
#endif
                this.Page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), "<script>" + script + "</script>");
#if AjaxTools
            }
#endif
        }

        public string getURL(OpenEditMode mode, object sender)
        {
            string url = this.EditURL.Contains("?") ? this.EditURL + "OpenEditMode=" + mode.ToString() :
                this.EditURL + "?OpenEditMode=" + mode.ToString();
            if (this.Site == null)
            {
                WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                switch (mode)
                {
                    case OpenEditMode.Insert:
                        url += "&KeyValues=1=0";
                        break;
                    case OpenEditMode.Update:
                    case OpenEditMode.View:
                        if (sender != null)
                        {
                            string filter = "";
                            Control ctrl = (Control)sender;
                            if (ctrl != null && ctrl is GridViewRow)
                            {
                                GridViewRow row = (GridViewRow)ctrl;
                                DataRow drow = null;
                                if (row.DataItem != null)
                                {
                                    drow = ((DataRowView)row.DataItem).Row;
                                }
                                else
                                {
                                    int index = row.DataItemIndex;
                                    //int x = 0, y = 0;
                                    //while (x < index)
                                    //{
                                    //    if (ds.View.Table.Rows[y].RowState != DataRowState.Deleted)
                                    //    {
                                    //        x++;
                                    //    }
                                    //    y++;
                                    //}
                                    //drow = ds.View.Table.Rows[y];
                                    //while (drow.RowState == DataRowState.Deleted)
                                    //{
                                    //    y++;
                                    //    drow = ds.View.Table.Rows[y];
                                    //}
                                    drow = ds.View[index].Row;
                                }
                                string strModuleName = ds.RemoteName.Substring(0, ds.RemoteName.IndexOf('.'));
                                string strTableName = ds.RemoteName.Substring(ds.RemoteName.IndexOf('.') + 1);
                                string tablename = CliUtils.GetTableName(strModuleName, strTableName, CliUtils.fCurrentProject);
                                string sqlcmd = CliUtils.GetSqlCommandText(strModuleName, strTableName, CliUtils.fCurrentProject);
                                string[] quote = CliUtils.GetDataBaseQuote();

                                for (int i = 0; i < ds.PrimaryKey.Length; i++)
                                {
                                    string columnName = ds.PrimaryKey[i].ColumnName;
                                    if (drow[columnName].ToString().Length > 0)
                                    {
                                        Type type = ds.PrimaryKey[i].DataType;
                                        if (type == typeof(uint) || type == typeof(UInt16) || type == typeof(UInt32)
                                          || type == typeof(UInt64) || type == typeof(int) || type == typeof(Int16)
                                          || type == typeof(Int32) || type == typeof(Int64) || type == typeof(Single)
                                          || type == typeof(Double) || type == typeof(Decimal))
                                        {
                                            filter += HttpUtility.UrlEncode(CliUtils.GetTableNameForColumn(sqlcmd, columnName)) + "=" + HttpUtility.UrlEncode(drow[columnName].ToString()) + " and ";
                                        }
                                        else if (type == typeof(DateTime))
                                        {
                                            filter += HttpUtility.UrlEncode(CliUtils.GetTableNameForColumn(sqlcmd, columnName)) + "=$$$" + HttpUtility.UrlEncode(((DateTime)drow[columnName]).ToShortDateString()) + "$$$ and ";
                                        }
                                        else
                                        {
                                            filter += HttpUtility.UrlEncode(CliUtils.GetTableNameForColumn(sqlcmd, columnName)) + "=$$$" + HttpUtility.UrlEncode(drow[columnName].ToString()) + "$$$ and ";
                                        }
                                    }
                                }
                                if (filter != "")
                                {
                                    filter = filter.Substring(0, filter.LastIndexOf(" and "));
                                    url += "&KeyValues=" + filter;
                                }
                                url += "&SelectIndex=" + row.RowIndex;
                            }
                        }
                        string pageIndex = this.PageIndex.ToString();
                        url += "&PageIndex=" + pageIndex;
                        break;
                }
                string strFilePath = this.Page.Request.FilePath;
                url += "&PagePath=" + strFilePath;
                string MasterOrDetail = this.GetMasterOrDetail();
                url += "&MasterOrDetail=" + MasterOrDetail;
                if (MasterOrDetail == "detail")
                {
                    StringBuilder relationField = new StringBuilder();
                    StringBuilder relationValue = new StringBuilder();

                    foreach (DictionaryEntry entry in ds.RelationValues)
                    {
                        if (relationField.Length > 0)
                        {
                            relationField.Append(";");
                        }
                        if (relationValue.Length > 0)
                        {
                            relationValue.Append(";");
                        }
                        relationField.Append(entry.Key.ToString());
                        relationValue.Append(entry.Value.ToString());
                    }

                    if (relationField.Length > 0 && relationValue.Length > 0)
                    {
                        url += "&RelationFields=" + HttpUtility.UrlEncode(relationField.ToString())
                            + "&RelationValues=" + HttpUtility.UrlEncode(relationValue.ToString());
                    }
                }

                if (this.ViewState["Paramters"] != null)
                {
                    url += "&Paramters=" + HttpUtility.UrlEncode(this.ViewState["Paramters"].ToString());
                }
                else
                {
                    string sParam = "";
                    foreach (GridViewOpenParam param in this.Params)
                    {
                        string pString = this.Page.Request.QueryString[param.ParamName];
                        if (pString != null && pString != "")
                        {
                            sParam += param.ParamName + "=" + pString + "^";
                        }
                    }
                    if (sParam != "")
                    {
                        sParam = sParam.Substring(0, sParam.LastIndexOf('^'));
                    }
                    url += "&Paramters=" + HttpUtility.UrlEncode(sParam);
                }
                url += "&GridViewID=" + this.ID;
                string itemparam = this.Page.Request.QueryString["ItemParam"] != null ? HttpUtility.UrlEncode(this.Page.Request.QueryString["ItemParam"]) : string.Empty;
                url += "&ItemParam=" + itemparam;

                if (!string.IsNullOrEmpty(ds.WhereStr))
                {
                    url += "&DataSourceID=" + ds.ID;
                    url += "&WhereStr=" + HttpUtility.UrlEncode(ds.WhereStr);
                }
            }
            return url.Replace("'", "\\'");
        }

        private DataBoundControl loopControl(string dataSource, Control con)
        {
            if (con != null && con is DataBoundControl && ((DataBoundControl)con).DataSourceID == dataSource)
            {
                return (DataBoundControl)con;
            }
            else
            {
                if (con.Controls.Count > 0)
                {
                    foreach (Control ctchild in con.Controls)
                    {
                        DataBoundControl ctrtn = loopControl(dataSource, ctchild);
                        if (ctrtn != null && ctrtn is DataBoundControl && ((DataBoundControl)ctrtn).DataSourceID == dataSource)
                        {
                            return ctrtn;
                        }
                    }
                    return null;
                }
                else
                    return null;
            }
        }

        private string GetMasterOrDetail()
        {
            string mORd = "master";
            object obj = this.GetObjByID(this.DataSourceID);
            if (obj is WebDataSource)
            {
                WebDataSource ds = (WebDataSource)obj;
                if (ds.MasterDataSource != null && ds.MasterDataSource.Length != 0)
                {
                    mORd = "detail";
                }
            }
            return mORd;
        }

        private static myColor baseColor = new myColor();

        private int _baseCount;
        [DefaultValue(0)]
        public int baseCount
        {
            get { return _baseCount; }
            set
            {
                if (_baseCount == 0)
                    _baseCount = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            WebValidate validate = (WebValidate)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
            if (validate != null && validate.ValidateActive == true)
                foreach (DataControlField field in this.Columns)
                {
                    if (field is BoundField)
                    {
                        if (baseColor.mark == "")
                            baseColor = new myColor("mark", ((BoundField)field).HeaderStyle.ForeColor);
                        foreach (ValidateFieldItem vfi in validate.Fields)
                        {
                            if (vfi.FieldName == ((BoundField)field).DataField)
                            {
                                baseCount = validate.ValidateChar.Length;
                                ((BoundField)field).HeaderStyle.ForeColor = baseColor.markColor;
                                if (validate.ValidateChar != "" && ((BoundField)field).HeaderText.IndexOf(validate.ValidateChar) == 0)
                                    ((BoundField)field).HeaderText = ((BoundField)field).HeaderText.Remove(0, baseCount);
                            }
                        }
                    }
                    else if (field is TemplateField)
                    {
                        if (baseColor == null)
                            baseColor = new myColor("mark", ((BoundField)field).HeaderStyle.ForeColor);
                        foreach (ValidateFieldItem vfi in validate.Fields)
                        {
                            if (vfi.FieldName == ((TemplateField)field).SortExpression)
                            {
                                baseCount = validate.ValidateChar.Length;
                                ((TemplateField)field).HeaderStyle.ForeColor = baseColor.markColor;
                                if (validate.ValidateChar != "" && ((TemplateField)field).HeaderText.IndexOf(validate.ValidateChar) == 0)
                                    ((TemplateField)field).HeaderText = ((TemplateField)field).HeaderText.Remove(0, baseCount);
                            }
                        }
                    }
                }

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
            if (Page.Request.QueryString != null)
            {
                GridView gridView = new GridView();
                if (Page.Request.QueryString["Filter"] != null && Page.Request.QueryString["DataSourceID"] == this.DataSourceID && !Page.IsPostBack)
                {
                    Filter = Page.Request.QueryString["Filter"];
                    webDs.WhereStr = Filter;

                    // webDs.LastKeyValues = null;
                    webDs.LastIndex = -1;
                    webDs.InnerDataSet.Clear();
                    webDs.InnerDataSet.ExtendedProperties.Clear();
                    webDs.Eof = false;
                }

                if (!Page.IsPostBack)
                {
                    if (!string.IsNullOrEmpty(Page.Request.QueryString["NAVMODE"]))
                    {
                        string mode = Page.Request.QueryString["NAVMODE"];
                        if (this.FLActive && (mode == "0" || mode == "Normal" || mode == "3" || mode == "Inquery"))
                        {
                            bool hasSet = false;
                            foreach (DataControlField field in this.Columns)
                            {
                                if (field is CommandField)
                                {
                                    ((CommandField)field).ShowEditButton = false;
                                    ((CommandField)field).ShowDeleteButton = false;
                                    hasSet = true;
                                }
                            }
                            if (!hasSet)
                            {
                                this.Columns[this.FLActiveColumnIndex].Visible = false;
                            }
                        }
                    }
                    if (Page.Request.QueryString["InsertKeyValues"] != null && Page.Request.QueryString["InsertKeyValues"] != ""
                        && Page.Request.QueryString["GridViewID"] != null && Page.Request.QueryString["GridViewID"] == this.ID)
                    {
                        string LocVals = Page.Request.QueryString["InsertKeyValues"];
                        string[] vals = LocVals.Split(';');
                        int i = vals.Length;
                        object[] keys = new object[i];
                        for (int j = 0; j < i; j++)
                        {
                            string type = vals[j].Substring(0, vals[j].IndexOf(' '));
                            string value = vals[j].Substring(vals[j].IndexOf(' ') + 1);
                            if (type == "system.uint" || type == "system.uint32")
                            {
                                keys[j] = Convert.ToUInt32(value);
                            }
                            else if (type == "system.uint16")
                            {
                                keys[j] = Convert.ToUInt16(value);
                            }
                            else if (type == "system.uint64")
                            {
                                keys[j] = Convert.ToUInt64(value);
                            }
                            else if (type == "system.int" || type == "system.int32")
                            {
                                keys[j] = Convert.ToInt32(value);
                            }
                            else if (type == "system.int16")
                            {
                                keys[j] = Convert.ToInt16(value);
                            }
                            else if (type == "system.int64")
                            {
                                keys[j] = Convert.ToInt64(value);
                            }
                            else if (type == "system.single" || type == "system.float")
                            {
                                keys[j] = Convert.ToSingle(value);
                            }
                            else if (type == "system.double")
                            {
                                keys[j] = Convert.ToDouble(value);
                            }
                            else if (type == "system.decimal")
                            {
                                keys[j] = Convert.ToDecimal(value);
                            }
                            else if (type == "system.datetime")
                            {
                                keys[j] = Convert.ToDateTime(value);
                            }
                            else if (type == "system.boolean")
                            {
                                keys[j] = Convert.ToBoolean(value);
                            }
                            else
                            {
                                keys[j] = value;
                            }
                        }

                        DataTable locDt = webDs.InnerDataSet.Tables[webDs.DataMember];
                        DataRow row = locDt.Rows.Find(keys);
                        int index = -1;
                        int x = locDt.Rows.Count;
                        for (int y = 0; y < x; y++)
                        {
                            if (locDt.Rows[y] == row)
                            {
                                index = y;
                                break;
                            }
                        }
                        if (index != -1)
                        {
                            int pSize = this.PageSize;
                            this.PageIndex = index / pSize;
                            this.SelectedIndex = index % pSize;
                        }
                    }
                    else if (Page.Request.QueryString["PageIndex"] != null && Page.Request.QueryString["PageIndex"] != "")
                    {
                        string pageIndex = Page.Request.QueryString["PageIndex"];
                        this.PageIndex = Convert.ToInt32(pageIndex);
                        if (Page.Request.QueryString["SelectIndex"] != null && Page.Request.QueryString["SelectIndex"] != "")
                        {
                            this.SelectedIndex = Convert.ToInt32(Page.Request.QueryString["SelectIndex"]);
                        }
                    }
                }
            }
            //base.OnLoad(e);
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
                    if (webDs.MasterDataSource == null || webDs.MasterDataSource.Length == 0)
                    {
                        int i = webDs.View != null ? webDs.View.Count : 0;
                        while (i <= PageSize && (!webDs.Eof))
                        {
                            webDs.GetNextPacket();
                            i += webDs.PacketRecords;
                        }
                    }
                }
            }

            // openurl时要传的参数,为了openurl回来之后按setwhere的条件来取出相应的数据
            string sParam = "";
            foreach (GridViewOpenParam param in this.Params)
            {
                string pString = this.Page.Request.QueryString[param.ParamName];
                if (pString != null && pString != "")
                {
                    sParam += param.ParamName + "=" + pString + "^";
                }
            }
            if (sParam != "")
            {
                sParam = sParam.Substring(0, sParam.LastIndexOf("^"));
                if (this.ViewState["Paramters"] != null)
                {
                    this.ViewState.Add("Paramters", sParam);
                }
                else
                {
                    this.ViewState["Paramters"] = sParam;
                }
            }
            else
            {
                this.ViewState.Remove("Paramters");
            }

            if (this.BottomPagerRow != null)
            {
                switch (this.PagerSettings.Position)
                {
                    case PagerPosition.Bottom:
                        this.BottomPagerRow.Visible = true;
                        break;
                    case PagerPosition.Top:
                        this.TopPagerRow.Visible = true;
                        break;
                    case PagerPosition.TopAndBottom:
                        this.TopPagerRow.Visible = true;
                        this.BottomPagerRow.Visible = true;
                        break;
                }
            }
            if (isQueryBack && !this.Page.IsPostBack)
                DataBind();
            base.OnLoad(e);
        }

        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            base.OnPageIndexChanging(e);
            if (e.NewPageIndex == this.PageCount - 1)
            {
                Object o = (this.Page.Master == null) ? this.Page.FindControl(this.DataSourceID) : this.Parent.FindControl(this.DataSourceID);
                if (o != null)
                {
                    WebDataSource webDs = (WebDataSource)o;
                    if (webDs.MasterDataSource == null || webDs.MasterDataSource.Length == 0)
                    {
                        DataTable table = new DataTable();
                        if (webDs.CommandTable != null)
                        {
                            table = webDs.CommandTable;
                        }
                        else if (webDs.InnerDataSet != null && webDs.InnerDataSet.Tables.Count != 0)
                        {
                            table = webDs.InnerDataSet.Tables[webDs.DataMember];
                        }
                        int j = e.NewPageIndex;
                        int i = table.Rows.Count;

                        while (i <= PageSize * (j + 1) && (!webDs.Eof))
                        {
                            webDs.GetNextPacket();
                            i += webDs.PacketRecords;
                        }
                    }
                }
            }
        }

        protected override void OnPageIndexChanged(EventArgs e)
        {
            base.OnPageIndexChanged(e);
            if (this.AutoPostBackMultiCheckBoxes)
            {
                this.PostBackMultiCheckBoxes();
            }
            if (this.AutoPostBackWebGridTextBoxes)
            {
                this.PostBackMultiTextBoxes();
            }
            if (this.AutoPostBackWebGridDropDowns)
            {
                this.PostBackMultiDropDowns();
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

        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            base.OnRowCreated(e);
            if (e.Row.RowType == DataControlRowType.Footer && this.ShowFooter)
            {
                #region 如果当前生成的Row是FooterRow
                int p = this.Columns.Count;
                DataControlField[] fields = new DataControlField[p];
                for (int q = 0; q < p; q++)
                {
                    fields[q] = this.Columns[q];
                }
                if (this.GridInserting)
                {
                    bool crRowExist = false;
                    bool dvExist = false;
                    object[,] carValues = null;
                    object[,] defaultValues = null;
                    WebDefault def = new WebDefault();
                    bool asExist = false;
                    string autoseqvalue = "";
                    string autoseqfield = "";
                    Hashtable tableautoseq = new Hashtable();
                    WebAutoSeq aus = new WebAutoSeq();
                    if (this.Page.Form != null)
                    {
                        def = (WebDefault)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebDefault));
                        // CarryOn
                        // 从ViewStates读出之前Insert的那一笔的Keys和Values，以便做CarryOn
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
                            // DefaultValue
                            if (def.DefaultActive)
                            {
                                defaultValues = def.GetDefaultValues();
                                dvExist = true;
                            }
                        }
                        //autoseq add by ccm
                        //
                        ControlCollection collection = (this.Page.Master == null) ? this.Page.Form.Controls : this.Parent.Controls;
                        foreach (Control ctrl in collection)
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
                    #region Default和CarryOn赋值， 如果是detail，则带key值
                    int x = this.Columns.Count;
                    for (int y = 0; y < x; y++)
                    {
                        string FieldName = "";
                        // 如果當前Cell所在Column是CommandField或者Column的SortExpression和HeadText有一個為空時,跳岀本次循環
                        if (this.Columns[y] is CommandField
                            || ((this.Columns[y].SortExpression == null || this.Columns[y].SortExpression == "")
                            && (this.Columns[y].HeaderText == null || this.Columns[y].HeaderText == "")))
                        {
                            continue;
                        }
                        // 如果当前Cell所在Column是BoundField，那么FieldName为DataField属性
                        if (this.Columns[y] is BoundField)
                            FieldName = ((BoundField)this.Columns[y]).DataField;
                        // 否则FieldName为SortExpression属性
                        else
                            FieldName = this.Columns[y].SortExpression;
                        object value = null;

                        // DefaultValue
                        if (dvExist)
                        {
                            int m = defaultValues.Length / 2;
                            for (int n = 0; n < m; n++)
                            {
                                if (FieldName == defaultValues[n, 0].ToString()
                                    && defaultValues[n, 1] != null
                                    && defaultValues[n, 1].ToString() != "")
                                {
                                    value = defaultValues[n, 1];
                                }
                            }
                        }
                        // CarrayOn
                        if (crRowExist)
                        {
                            int m = carValues.Length / 2;
                            for (int n = 0; n < m; n++)
                            {
                                if (def.Fields[FieldName] != null &&
                                    ((DefaultFieldItem)def.Fields[FieldName]).CarryOn &&
                                    FieldName == carValues[n, 0].ToString())
                                {
                                    value = carValues[n, 1];
                                }
                            }
                        }
                        // AutoSeq
                        if (asExist)
                        {
                            if (tableautoseq.ContainsKey(FieldName))
                            {
                                value = tableautoseq[FieldName];
                            }
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
                        CellControls.Clear();
                        GetAllCellControls(e.Row.Cells[y]);
                        foreach (Control ctrl in CellControls)
                        {
                            if (value != null)
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
                                if (ctrl is Label)
                                {
                                    ((Label)ctrl).Text = value.ToString();
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (!this.GridInserting && this.TotalActive)
                {
                    int i = fields.Length;
                    for (int j = 0; j < i; j++)
                    {
                        string fieldName = "";
                        if (fields[j] is BoundField)
                        {
                            fieldName = ((BoundField)fields[j]).DataField;
                        }
                        else if (fields[j] is ExpressionField)
                        {
                            fieldName = ((ExpressionField)fields[j]).Expression;
                        }
                        else
                        {
                            fieldName = fields[j].SortExpression;
                        }
                        e.Row.Cells[j].Controls.Clear();
                        if (fieldName != "")
                        {
                            foreach (GridViewTotalItem item in this.TotalColumns)
                            {
                                if (item.FieldName == fieldName && item.ShowTotal)
                                {
                                    Label lbl = new Label();
                                    if (this.Site == null)
                                        lbl.Text = GetTotalValue(fieldName, item.TotalMode, item.Format);
                                    else
                                        lbl.Text = "Total";
                                    e.Row.Cells[j].Controls.Add(lbl);
                                }
                            }
                        }
                        else
                        {
                            if (j == 0)
                            {
                                Label lbl = new Label();
                                lbl.Text = this.TotalCaption;
                                e.Row.Cells[j].Controls.Add(lbl);
                            }
                        }
                    }
                }

                #endregion
            }
        }

        [Browsable(false)]
        [DefaultValue("")]
        public string ButtonTooltip
        {
            get
            {
                return (ViewState["ButtonTooltip"] == null ? "" : (string)ViewState["ButtonTooltip"]);
            }
            set
            {
                ViewState["ButtonTooltip"] = value;
            }
        }

        string _eidtUrlpanel = "";
        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string EditURLPanel
        {
            get { return _eidtUrlpanel; }
            set { _eidtUrlpanel = value; }
        }

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);
            if (this.DesignMode)
                return;
            object obj = this.GetObjByID(this.DataSourceID);
            WebDataSource wds = null;
            if (obj != null && obj is WebDataSource)
            {
                wds = (WebDataSource)obj;
            }
            if (ButtonTooltip == "")
            {
                ButtonTooltip = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebGridView", "ButtonName", true);
            }
            string[] arrButtonTooltip = ButtonTooltip.Split(';');
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //WebSecColumns
                WebSecColumns webSecColumns = (WebSecColumns)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                string searchTemplate = "";
                if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
                {
                    searchTemplate = "EditItemTemplate";
                }
                else if ((e.Row.RowState & DataControlRowState.Normal) == DataControlRowState.Normal)
                {
                    searchTemplate = "ItemTemplate";
                }
                if (searchTemplate != "" && webSecColumns != null)
                {
                    foreach (WebSecControl secCtrl in webSecColumns.WebSecControls)
                    {
                        if (secCtrl.ControlParent == this.ID && secCtrl.ControlTemplate == searchTemplate)
                        {
                            Control sec = e.Row.FindControl(secCtrl.ControlID);
                            if (sec != null)
                            {
                                sec.Visible = webSecColumns.ColumnsVisible;
                                Type type = sec.GetType();
                                PropertyInfo proInfo = type.GetProperty("ReadOnly");
                                if (proInfo != null)
                                {
                                    proInfo.SetValue(sec, webSecColumns.ColumnsReadOnly, null);
                                }
                                else if (sec is WebControl)
                                {
                                    (sec as WebControl).Enabled = !webSecColumns.ColumnsReadOnly;
                                }
                            }
                        }
                    }
                }
                DataRow dr = (e.Row.DataItem as DataRowView).Row;
                DataColumn[] keys = dr.Table.PrimaryKey;
                bool isemptyrow = true;
                for (int i = 0; i < keys.Length; i++)
                {
                    if ((keys[i].DataType == typeof(string) && (string)dr[keys[i]] != string.Empty)
                        || (keys[i].DataType == typeof(int) && (int)dr[keys[i]] != 0)
                        || (keys[i].DataType == typeof(DateTime) && (DateTime)dr[keys[i]] != DateTime.MaxValue)
                        || (keys[i].DataType == typeof(Guid) && (Guid)dr[keys[i]] != Guid.Empty)
                        )
                    {
                        isemptyrow = false;
                        break;
                    }
                }
                if (isemptyrow && keys.Length > 0)
                {
                    List<DataColumn> listcolumn = new List<DataColumn>(keys);
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        if (!listcolumn.Contains(dr.Table.Columns[i]))
                        {
                            if (dr[i] != DBNull.Value)
                            {
                                isemptyrow = false;
                                break;
                            }
                        }
                    }
                }

                //ToolTip
                foreach (TableCell cell in e.Row.Cells)
                {
                    foreach (Control ctrl in cell.Controls)
                    {
                        if (ctrl is IButtonControl)
                        {
                            IButtonControl btn = (IButtonControl)ctrl;
                            if (btn.CommandName == "Edit")
                            {
                                if (btn is ImageButton)
                                    ((ImageButton)btn).ToolTip = arrButtonTooltip[0];
                                else if (btn is LinkButton)
                                    ((LinkButton)btn).ToolTip = arrButtonTooltip[0];
                                else if (btn is Button)
                                    ((Button)btn).ToolTip = arrButtonTooltip[0];
                                if (isemptyrow)
                                {
                                    ((Control)btn).Visible = false;
                                }
                            }
                            else if (btn.CommandName == "Select")
                            {
                                if (btn is ImageButton)
                                    ((ImageButton)btn).ToolTip = arrButtonTooltip[2];
                                else if (btn is LinkButton)
                                    ((LinkButton)btn).ToolTip = arrButtonTooltip[2];
                                else if (btn is Button)
                                    ((Button)btn).ToolTip = arrButtonTooltip[2];

                            }
                            else if (btn.CommandName == "Delete")
                            {
                                if (btn is ImageButton)
                                    ((ImageButton)btn).ToolTip = arrButtonTooltip[1];
                                else if (btn is LinkButton)
                                    ((LinkButton)btn).ToolTip = arrButtonTooltip[1];
                                else if (btn is Button)
                                    ((Button)btn).ToolTip = arrButtonTooltip[1];
                                if (isemptyrow)
                                {
                                    ((Control)btn).Visible = false;
                                }
                            }
                        }
                    }

                }


                // OpenUrl
                if (!OpenEditUrlInServerMode && this.EditURL != null && this.EditURL != "")
                {
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        foreach (Control ctrl in cell.Controls)
                        {
                            if (ctrl is IButtonControl)
                            {
                                IButtonControl btn = (IButtonControl)ctrl;
                                string url = "";
                                if (btn.CommandName == "Edit")
                                {
                                    url = this.getURL(OpenEditMode.Update, e.Row);
                                    if (wds != null && !wds.AllowUpdate)
                                    {
                                        ((WebControl)btn).Visible = false;
                                    }
                                }
                                else if (btn.CommandName == "Select")
                                {
                                    url = this.getURL(OpenEditMode.View, e.Row);
                                }
                                else if (btn.CommandName == "Delete")
                                {
                                    if (wds != null && !wds.AllowDelete)
                                    {
                                        ((WebControl)btn).Visible = false;
                                    }
                                }
                                if (url != "")
                                {
                                    string s = string.Empty;
                                    if (ShowDialog)
                                    {
                                        s = "window.showModalDialog('" + url + "','','dialogwidth=" + OpenEditWidth + "px;dialogheight="
                                            + OpenEditHeight + "px,dialogleft=" + OpenEditLeft + "px,dialogtop=" + OpenEditTop
                                            + "px'); return false;";
                                    }
                                    else
                                    {
                                        s = "window.open('" + url + "','Edit','height=" + OpenEditHeight + ",width=" + OpenEditWidth +
                                        ",left=" + OpenEditLeft + ",top=" + OpenEditTop + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no');return false;";
                                    }

                                    if (btn is ImageButton)
                                        ((ImageButton)btn).OnClientClick = s;
                                    else if (btn is LinkButton)
                                        ((LinkButton)btn).OnClientClick = s;
                                    else if (btn is Button)
                                        ((Button)btn).OnClientClick = s;
                                }
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(this.EditURLPanel))
                {
                    object obj_editPanel = this.GetObjByID(this.EditURLPanel);
                    //object obj_rowItem = e.Row.DataItem;
                    //if (obj_editPanel != null && obj_editPanel is IModalPanel && obj_rowItem != null && obj_rowItem is DataRowView)
                    if (obj_editPanel != null && obj_editPanel is IModalPanel)
                    {
                        IModalPanel panel = obj_editPanel as IModalPanel;
                        //DataRowView row = obj_rowItem as DataRowView;

                        //string args = "";
                        //foreach (DataColumn dc in wds.PrimaryKey)
                        //{
                        //    string type = row[dc.ColumnName].GetType().ToString().ToLower();
                        //    if (type == "system.uint" || type == "system.uint16" || type == "system.uint32" || type == "system.uint64" || type == "system.int" || type == "system.int16" || type == "system.int32" || type == "system.int64" || type == "system.single" || type == "system.float" || type == "system.double" || type == "system.decimal")
                        //    {
                        //        args += dc.ColumnName + "=" + row[dc.ColumnName].ToString() + " and ";
                        //    }
                        //    else
                        //    {
                        //        args += dc.ColumnName + "='" + row[dc.ColumnName].ToString() + "' and ";
                        //    }
                        //}
                        //if (args != "")
                        //{
                        //    args = args.Substring(0, args.LastIndexOf(" and "));
                        //}
                        foreach (TableCell cell in e.Row.Cells)
                        {
                            foreach (Control ctrl in cell.Controls)
                            {
                                if (ctrl is IButtonControl)
                                {
                                    IButtonControl btn = (IButtonControl)ctrl;
                                    if (btn.CommandName == "AjaxView" || btn.CommandName == "AjaxEdit")
                                    {
                                        btn.CommandArgument = this.ID;
                                    }
                                }
                            }
                        }
                    }
                }
                // ExpressionField...
                foreach (DataControlField field in this.Columns)
                {
                    if (field is ExpressionField)
                    {
                        ExpressionField expField = (ExpressionField)field;
                        Control ctrl = e.Row.FindControl("ExpressionLabel" + expField.Expression + e.Row.RowIndex);
                        if (ctrl != null && ctrl is Label)
                        {
                            string strExpression = ((Label)ctrl).Text;
                            DataTable tab = ((DataRowView)e.Row.DataItem).Row.Table;
                            if (!tab.Columns.Contains(strExpression))
                            {
                                DataColumn col = new DataColumn(strExpression, typeof(decimal), strExpression);
                                tab.Columns.Add(col);
                            }
                            ((Label)ctrl).Text = ((DataRowView)e.Row.DataItem).Row[strExpression].ToString();
                        }
                    }
                }
                // MultiCheckBoxes
                if (this.MultiCheckColumnIndexToTranslate != -1)
                {
                    string phyPath = this.Page.Request.PhysicalPath;
                    //phyPath = phyPath.Substring(0, phyPath.LastIndexOf(".aspx"));
                    //string xmlPath = phyPath + ".xml";
                    string xmlPath = CliUtils.ReplaceFileName(phyPath, ".aspx", ".xml");

                    if (File.Exists(xmlPath))
                    {
                        FileStream stream = new FileStream(xmlPath, FileMode.Open);
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(stream);
                        XmlNode nRoot = xmlDoc.SelectSingleNode("InfoLight");
                        if (nRoot != null)
                        {
                            XmlNode nWebCheckBoxes = nRoot.SelectSingleNode("WebCheckBoxes");
                            if (nWebCheckBoxes != null)
                            {
                                XmlNode nWebGridView = nWebCheckBoxes.SelectSingleNode("WebGridView[@ID='" + this.ID + "']");
                                if (nWebGridView != null)
                                {
                                    string keyValues = "";
                                    DataRowView row = (DataRowView)e.Row.DataItem;
                                    if (row != null && wds != null)
                                    {
                                        foreach (DataColumn dc in wds.PrimaryKey)
                                        {
                                            keyValues += row[dc.ColumnName].ToString() + ";";
                                        }
                                        if (keyValues != "")
                                        {
                                            keyValues = keyValues.Substring(0, keyValues.LastIndexOf(";"));
                                            XmlNode nCheckBox = nWebGridView.FirstChild;
                                            while (nCheckBox != null)
                                            {
                                                string xmlKeyValues = nCheckBox.Attributes["KeyValues"].Value;
                                                string checkState = nCheckBox.InnerText;
                                                if (xmlKeyValues == keyValues && checkState == "true")
                                                {
                                                    foreach (Control ctrl in e.Row.Cells[this.MultiCheckColumnIndexToTranslate].Controls)
                                                    {
                                                        if (ctrl is WebCheckBox)
                                                        {
                                                            ((WebCheckBox)ctrl).Checked = true;
                                                        }
                                                    }
                                                }
                                                nCheckBox = nCheckBox.NextSibling;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        stream.Close();
                        xmlDoc.Save(xmlPath);
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                //WebSecColumns
                WebSecColumns webSecColumns = (WebSecColumns)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                if (webSecColumns != null)
                {
                    foreach (WebSecControl secCtrl in webSecColumns.WebSecControls)
                    {
                        if (secCtrl.ControlParent == this.ID && (secCtrl.ControlTemplate == "HeaderTemplate"))
                        {
                            Control sec = e.Row.FindControl(secCtrl.ControlID);
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
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                //WebSecColumns
                WebSecColumns webSecColumns = (WebSecColumns)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebSecColumns));
                if (webSecColumns != null)
                {
                    foreach (WebSecControl secCtrl in webSecColumns.WebSecControls)
                    {
                        if (secCtrl.ControlParent == this.ID && (secCtrl.ControlTemplate == "FooterTemplate"))
                        {
                            Control sec = e.Row.FindControl(secCtrl.ControlID);
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

        protected override void OnRowCommand(GridViewCommandEventArgs e)
        {
            base.OnRowCommand(e);
            object obj_editPanel = this.GetObjByID(this.EditURLPanel);
            if (obj_editPanel != null && obj_editPanel is IModalPanel)
            {
                IModalPanel panel = obj_editPanel as IModalPanel;
                if (e.CommandName == "AjaxView")
                {
                    panel.Open(OpenEditMode.View, e);
                }
                else if (e.CommandName == "AjaxEdit")
                {
                    object obj = this.GetObjByID(this.DataSourceID);
                    if (obj != null && obj is WebDataSource)
                    {
                        WebDataSource wds = (WebDataSource)obj;
                        wds.ExecuteSelect(this);
                    }
                    panel.Open(OpenEditMode.Update, e);
                }
            }
        }

        public WebNavigator GetBindingNavigator()
        {
            WebNavigator nav = null;
            object obj = this.GetObjByID(this.DataSourceID);
            if (obj != null && obj is WebDataSource)
            {
                WebDataSource wds = (WebDataSource)obj;
                if (!string.IsNullOrEmpty(wds.MasterDataSource))
                {
                    WebDataSource wdsMaster = (WebDataSource)wds.MasterWebDataSource;
                    DataBoundControl dbContrl = (DataBoundControl)this.ExtendedFindChildControl(wdsMaster.ID, FindControlType.DataSourceID, typeof(DataBoundControl));
                    if (dbContrl != null)
                    {
                        nav = (WebNavigator)this.ExtendedFindChildControl(dbContrl.ID, FindControlType.BindingObject, typeof(WebNavigator));
                    }
                }
                else
                {
                    nav = (WebNavigator)this.ExtendedFindChildControl(this.ID, FindControlType.BindingObject, typeof(WebNavigator));
                }
            }
            return nav;
        }

        protected override void OnRowEditing(GridViewEditEventArgs e)
        {
            ValidateFormat();
            WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
            if (ds.AllowUpdate)
            {
                if (ds != null && ds.AutoApply)
                {
                    DataSet dataset = ds.InnerDataSet;
                    if (dataset != null)
                    {
                        if (dataset.GetChanges() != null)
                        {
                            language = CliUtils.fClientLang;
                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "InsertedNotApply", true);
                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "InsertedNotApply", "<script>alert('" + message + "')</script>");
                            return;
                        }
                    }
                }
                if (ds.IsEmpty)
                {
                    e.Cancel = true;
                    return;
                }
                if (ds.AutoRecordLock)
                {
                    object[] value = new object[ds.PrimaryKey.Length];
                    DataRow dr = ds.View[e.NewEditIndex].Row;
                    for (int i = 0; i < ds.PrimaryKey.Length; i++)
                    {
                        value[i] = dr[ds.PrimaryKey[i].ColumnName];
                    }
                    if (!ds.AddLock("Updating", value))
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                if (this.EditURL != null && this.EditURL != "")
                {
                    this.OpenEditURL(WebGridView.OpenEditMode.Update, this.Rows[e.NewEditIndex]);
                    e.Cancel = true;
                    return;
                }
                base.OnRowEditing(e);
                this.SelectedIndex = e.NewEditIndex;
                base.OnSelectedIndexChanged(new EventArgs());
                WebNavigator nav = GetBindingNavigator();
                if (nav != null)
                {
                    nav.SetState(WebNavigator.NavigatorState.Editing);
                    nav.SetNavState("Editing");
                }
            }
            else
            {
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToUpdate", true);
                string script = "alert('" + message + "')";
                this.RegisterAjaxScript(script);
                e.Cancel = true;
            }
            if (this.AutoPostBackMultiCheckBoxes)
            {
                this.PostBackMultiCheckBoxes();
            }
            if (this.AutoPostBackWebGridTextBoxes)
            {
                this.PostBackMultiTextBoxes();
            }
            if (this.AutoPostBackWebGridDropDowns)
            {
                this.PostBackMultiDropDowns();
            }
        }

        protected override void OnSelectedIndexChanging(GridViewSelectEventArgs e)
        {
            WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
            if (ds != null && (string.IsNullOrEmpty(ds.MasterDataSource)))
            {
                if (ds.IsEmpty)
                {
                    e.Cancel = true;
                    return;
                }
            }
            if (this.EditURL != null && this.EditURL != "")
            {
                this.OpenEditURL(WebGridView.OpenEditMode.View, this.Rows[e.NewSelectedIndex]);
                e.Cancel = true;
                return;
            }
            base.OnSelectedIndexChanging(e);
            if (this.AutoPostBackMultiCheckBoxes)
            {
                this.PostBackMultiCheckBoxes();
            }
            if (this.AutoPostBackWebGridTextBoxes)
            {
                this.PostBackMultiTextBoxes();
            }
            if (this.AutoPostBackWebGridDropDowns)
            {
                this.PostBackMultiDropDowns();
            }
        }

        protected override void OnRowDeleting(GridViewDeleteEventArgs e)
        {
            WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);

            if (ds.AllowDelete)
            {
                base.OnRowDeleting(e);
                if (ds.IsEmpty)
                {
                    e.Cancel = true;
                }
                if (ds.AutoRecordLock)
                {
                    if (ds.PrimaryKey.Length > 0)
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
                    else
                    {
                        throw new EEPException(EEPException.ExceptionType.MethodNotSupported, ds.GetType(), ds.ID, "AddLock", null);
                    }
                }
            }
            else
            {
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDataSource", "rightToDelete", true);
                string script = "alert('" + message + "')";
                this.RegisterAjaxScript(script);
                e.Cancel = true;
            }
            this.SelectedIndex = e.RowIndex;
            if (this.AutoPostBackMultiCheckBoxes)
            {
                this.PostBackMultiCheckBoxes();
            }
            if (this.AutoPostBackWebGridTextBoxes)
            {
                this.PostBackMultiTextBoxes();
            }
            if (this.AutoPostBackWebGridDropDowns)
            {
                this.PostBackMultiDropDowns();
            }
        }

        protected override void OnRowDeleted(GridViewDeletedEventArgs e)
        {
            base.OnRowDeleted(e);
            WebNavigator nav = GetBindingNavigator();
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

        protected override void OnRowUpdated(GridViewUpdatedEventArgs e)
        {
            base.OnRowUpdated(e);
            WebNavigator nav = GetBindingNavigator();
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

        private List<Control> CellControls = new List<Control>();
        private void GetAllCellControls(Control baseControl)
        {
            foreach (Control ctrl in baseControl.Controls)
            {
                if (!CellControls.Contains(ctrl))
                    CellControls.Add(ctrl);
                if (!(ctrl is WebRefValBase) && ctrl.Controls.Count > 0) // exclude innner controls of refval
                    GetAllCellControls(ctrl);
            }
        }

        public void ValidateFormat()
        {
            WebValidate validate = (WebValidate)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
            if (validate != null && validate.ValidateActive == true)
            {
                foreach (DataControlField field in this.Columns)
                {
                    if (field is BoundField)
                    {
                        foreach (ValidateFieldItem vfi in validate.Fields)
                        {
                            if (vfi.FieldName == ((BoundField)field).DataField)
                            {
                                ((BoundField)field).HeaderStyle.ForeColor = validate.ValidateColor;
                                if (validate.ValidateChar != "" && ((BoundField)field).HeaderText.IndexOf(validate.ValidateChar) != 0)
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
                                if (validate.ValidateChar != "" && ((TemplateField)field).HeaderText.IndexOf(validate.ValidateChar) != 0)
                                    ((TemplateField)field).HeaderText = ((TemplateField)field).HeaderText.Insert(0, validate.ValidateChar);
                            }
                        }
                    }
                }
            }
        }

        public void OnAdding(EventArgs value)
        {

            ValidateFormat();
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

        public void OnAfterInsert(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnAfterInsert];
            if (handler != null)
            {
                handler(this, value);
            }
            doDefaultAfterInsert();
        }

        internal static readonly object EventOnAfterInsert = new object();

        public event EventHandler AfterInsert
        {
            add { base.Events.AddHandler(EventOnAfterInsert, value); }
            remove { base.Events.RemoveHandler(EventOnAfterInsert, value); }
        }


        protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        {
            base.InitializePager(row, columnSpan, pagedDataSource);
            if (this.DesignMode) return;
            if (row.RowType == DataControlRowType.Pager)
            {
                // add WebNavigator for Add and Query
                #region Add Navigator
                if (!string.IsNullOrEmpty(Page.Request.QueryString["NAVMODE"]) && this.FLActive)
                {
                    string mode = Page.Request.QueryString["NAVMODE"];
                    if (this.FLActive && (mode == "0" || mode == "Normal" || mode == "3" || mode == "Inquery"))
                    {
                        InitNavControls(true, row, null, false);
                    }
                    else
                    {
                        InitNavControls(true, row, null, CreateInnerNavigator);
                    }
                }
                else
                {
                    InitNavControls(true, row, null, CreateInnerNavigator);
                }
                #endregion
            }
        }

        WebNavigator nav1 = new WebNavigator();
        WebNavigator nav2 = new WebNavigator();
        protected void InitNavControls(bool NavForAddAndQuery, GridViewRow row, DataControlField[] fields, bool AllowInNav)
        {
            if (NavForAddAndQuery)
                nav1.ID = "InPageNavigatorForAddAndQuery";
            else
                nav2.ID = "InPageNavigatorForOKAndCancel";
            nav1.AddDefaultControls = false;
            nav2.AddDefaultControls = false;
            string[] ctrlTexts = { "add", "query", "apply", "abort", "insert", "cancel" };
            if (this.GetServerText)
            {
                language = CliUtils.fClientLang;
                String message = SysMsg.GetSystemMessage(language,
                                                         "Srvtools",
                                                         "WebGridView",
                                                         "ControlText", true);
                ctrlTexts = message.Split(';');
            }
            else
            {
                int x = ctrlTexts.Length;
                for (int y = 0; y < x; y++)
                {
                    foreach (ControlItem item in this.NavControls)
                    {
                        if (item.ControlName.ToUpper() == ctrlTexts[y].ToUpper())
                        {
                            ctrlTexts[y] = item.ControlText;
                            break;
                        }
                    }
                }
            }
            object obj = this.GetObjByID(this.DataSourceID);
            WebDataSource wds = null;
            if (obj != null && obj is WebDataSource)
            {
                wds = (WebDataSource)obj;
            }
            if (this.NavControls.Count == 0)
            {
                #region add default controls
                if (wds != null && wds.AllowAdd)
                {
                    // Add Add Control
                    ControlItem AddItem = new ControlItem
                        ("Add", ctrlTexts[0], this.InnerNavigatorShowStyle, "../image/uipics/add.gif", "../image/uipics/add2.gif", "../image/uipics/add3.gif", 26, true);
                    this.NavControls.Add(AddItem);
                }
                // Add Query Control
                ControlItem QueryItem = new ControlItem
                    ("Query", ctrlTexts[1], this.InnerNavigatorShowStyle, "../image/uipics/query.gif", "../image/uipics/query2.gif", "../image/uipics/query3.gif", 26, true);
                this.NavControls.Add(QueryItem);
                // Add Apply Control
                ControlItem ApplyItem = new ControlItem
                    ("Apply", ctrlTexts[2], this.InnerNavigatorShowStyle, "../image/uipics/apply.gif", "../image/uipics/apply2.gif", "../image/uipics/apply3.gif", 26, true);
                this.NavControls.Add(ApplyItem);
                // Add Abort Control
                ControlItem AbortItem = new ControlItem
                    ("Abort", ctrlTexts[3], this.InnerNavigatorShowStyle, "../image/uipics/abort.gif", "../image/uipics/abort2.gif", "../image/uipics/abort3.gif", 26, true);
                this.NavControls.Add(AbortItem);
                // Add OK Control
                ControlItem OKItem = new ControlItem
                    ("OK", ctrlTexts[4], this.InnerNavigatorShowStyle, "../image/uipics/ok.gif", "../image/uipics/ok2.gif", "../image/uipics/ok3.gif", 26, true);
                this.NavControls.Add(OKItem);
                // Add Cancel Control
                ControlItem CancelItem = new ControlItem
                    ("Cancel", ctrlTexts[5], this.InnerNavigatorShowStyle, "../image/uipics/cancel.gif", "../image/uipics/cancel2.gif", "../image/uipics/cancel3.gif", 26, true);
                this.NavControls.Add(CancelItem);
                #endregion
            }
            nav1.GetServerText = this.GetServerText;
            nav2.GetServerText = this.GetServerText;
            nav1.ShowDataStyle = WebNavigator.NavigatorStyle.GridStyle;
            nav2.ShowDataStyle = WebNavigator.NavigatorStyle.GridStyle;
            nav1.BindingObject = this.ID;
            nav2.BindingObject = this.ID;
            nav1.LinkLable = this.InnerNavigatorLinkLabel;
            nav2.LinkLable = this.InnerNavigatorLinkLabel;
            foreach (WebQueryField f in this.QueryFields)
            {
                nav1.QueryFields.Add(f);
            }
            if (NavForAddAndQuery)
            {
                if (AllowInNav)
                {
                    foreach (ControlItem item in this.NavControls)
                    {
                        if (item.ControlName != "OK" && item.ControlName != "Cancel")
                        {
                            if (item.ControlName == "Add")
                                item.ControlText = ctrlTexts[0];
                            else if (item.ControlName == "Query")
                                item.ControlText = ctrlTexts[1];
                            else if (item.ControlName == "Apply")
                                item.ControlText = ctrlTexts[2];
                            else if (item.ControlName == "Abort")
                                item.ControlText = ctrlTexts[3];
                            nav1.NavControls.Add(item);
                        }
                    }
                    foreach (Control ctrl in row.Cells[0].Controls)
                    {
                        if (ctrl is Table)
                        {
                            Table t = (Table)ctrl;
                            TableCell tc = new TableCell();
                            tc.Controls.Add(nav1);
                            int indx = t.Rows[0].Cells.GetCellIndex(tc);
                            if (indx == -1)
                            {
                                t.Rows[0].Cells.Add(tc);
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (ControlItem item in this.NavControls)
                {
                    if (item.ControlName == "OK" || item.ControlName == "Cancel")
                    {
                        if (item.ControlName == "OK")
                            item.ControlText = ctrlTexts[4];
                        else if (item.ControlName == "Cancel")
                            item.ControlText = ctrlTexts[5];
                        nav2.NavControls.Add(item);
                    }
                }

                int i = fields.Length;
                for (int j = 0; j < i; j++)
                {
                    if (fields[j] is CommandField ||
                        (fields[j] is TemplateField && string.IsNullOrEmpty(fields[j].SortExpression)))
                    //remarked by lily 2009-2-24 爲了可以讓操作列可以設定HeadText
                    //&& (fields[j].HeaderText == null || fields[j].HeaderText == "")))
                    {
                        row.Cells[j].Controls.Add(nav2);
                        break;
                    }
                }
            }
        }

        private static readonly object EventInserting = new object();
        [Browsable(false)]
        public event EventHandler Inserting
        {
            add { Events.AddHandler(EventInserting, value); }
            remove { Events.RemoveHandler(EventInserting, value); }
        }


        private static readonly object EventAdded = new object();
        public event EventHandler Added
        {
            add { Events.AddHandler(EventAdded, value); }
            remove { Events.RemoveHandler(EventAdded, value); }
        }


        public virtual void OnInserting(EventArgs e)
        {
            EventHandler InsertingHandler = (EventHandler)Events[EventInserting];
            if (InsertingHandler != null)
            {
                InsertingHandler(this, e);
            }
            EventHandler AddedHandler = (EventHandler)this.Events[EventAdded];
            if (AddedHandler != null)
            {
                AddedHandler(this, e);
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
                foreach (ControlItem item in this.NavControls)
                {
                    item.ControlType = value;
                }
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

        protected override void InitializeRow(GridViewRow row, DataControlField[] fields)
        {
            base.InitializeRow(row, fields);
            if (this.DesignMode) return;
            if (row.RowType == DataControlRowType.Footer)
            {
                if (this.GridInserting)
                {
                    // add WebNavigator for OK and Cancel
                    InitNavControls(false, row, fields, CreateInnerNavigator);
                    int i = fields.Length;
                    if (this.AutoGenerateEditButton || this.AutoGenerateDeleteButton || this.AutoGenerateSelectButton)
                    {
                        row.Cells[0].Controls.Add(nav2);
                        for (int j = 1; j < i; j++)
                        {
                            string ctrlID = "";
                            if (fields[j] is BoundField)
                            {
                                BoundField bField = (BoundField)fields[j];
                                ctrlID = bField.DataField;
                                if (!bField.ReadOnly)
                                {
                                    TextBox txt = new TextBox();
                                    txt.ID = "InfoTextBox" + ctrlID;
                                    txt.Width = 100;
                                    row.Cells[j].Controls.Add(txt);
                                }
                                else
                                {
                                    Label lbl = new Label();
                                    lbl.ID = "InfoLabel" + ctrlID;
                                    lbl.Width = 100;
                                    row.Cells[j].Controls.Add(lbl);
                                }
                            }
                        }
                    }
                    else
                    {
                        // add Controls for add
                        for (int j = 0; j < i; j++)
                        {
                            string ctrlID = "";
                            if (fields[j] is BoundField)
                            {
                                BoundField bField = (BoundField)fields[j];
                                ctrlID = bField.DataField;
                                if (!bField.ReadOnly)
                                {
                                    TextBox txt = new TextBox();
                                    txt.ID = "InfoTextBox" + ctrlID;
                                    txt.Width = 100;
                                    row.Cells[j].Controls.Add(txt);
                                }
                                else
                                {
                                    Label lbl = new Label();
                                    lbl.ID = "InfoLabel" + ctrlID;
                                    lbl.Width = 100;
                                    row.Cells[j].Controls.Add(lbl);
                                }
                            }
                        }
                    }
                }
            }
        }

        public string GetTotalValue(string FieldName, totalMode mode, string format)
        {
            string strValue = "";
            object obj = this.GetObjByID(this.DataSourceID);
            if (obj != null && obj is WebDataSource)
            {

                DataTable dt = (obj as WebDataSource).View.Table;
                decimal total = 0;
                decimal max = decimal.MinValue;
                decimal min = decimal.MaxValue;
                decimal average = new decimal();

                if (dt.Columns[FieldName].DataType.ToString().ToLower() == "system.uint")
                {
                    switch (mode)
                    {
                        case totalMode.count:
                            strValue = dt.Rows.Count.ToString();
                            break;
                        case totalMode.sum:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (uint)row[FieldName];
                                }
                            }
                            strValue = total.ToString();
                            break;
                        case totalMode.average:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (uint)row[FieldName];
                                }
                            }
                            int count = dt.Rows.Count;
                            average = total / count;
                            strValue = average.ToString();
                            break;
                        case totalMode.max:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    max = Math.Max((uint)row[FieldName], max);
                                }
                            }
                            if (max != decimal.MinValue)
                            {
                                strValue = max.ToString();
                            }
                            break;
                        case totalMode.min:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    min = Math.Min((uint)row[FieldName], min);
                                }
                            }
                            if (min != decimal.MinValue)
                            {
                                strValue = min.ToString();
                            }
                            break;
                    }
                }
                else if (dt.Columns[FieldName].DataType.ToString().ToLower() == "system.uint16")
                {
                    switch (mode)
                    {
                        case totalMode.count:
                            strValue = dt.Rows.Count.ToString();
                            break;
                        case totalMode.sum:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (UInt16)row[FieldName];
                                }
                            }
                            strValue = total.ToString();
                            break;
                        case totalMode.average:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (UInt16)row[FieldName];
                                }
                            }
                            int count = dt.Rows.Count;
                            average = total / count;
                            strValue = average.ToString();
                            break;
                        case totalMode.max:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    max = Math.Max((UInt16)row[FieldName], max);
                                }
                            }
                            if (max != decimal.MinValue)
                            {
                                strValue = max.ToString();
                            }
                            break;
                        case totalMode.min:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    min = Math.Min((UInt16)row[FieldName], min);
                                }
                            }
                            if (min != decimal.MinValue)
                            {
                                strValue = min.ToString();
                            }
                            break;
                    }
                }
                else if (dt.Columns[FieldName].DataType.ToString().ToLower() == "system.uint32")
                {
                    switch (mode)
                    {
                        case totalMode.count:
                            strValue = dt.Rows.Count.ToString();
                            break;
                        case totalMode.sum:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (UInt32)row[FieldName];
                                }
                            }
                            strValue = total.ToString();
                            break;
                        case totalMode.average:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (UInt32)row[FieldName];
                                }
                            }
                            int count = dt.Rows.Count;
                            average = total / count;
                            strValue = average.ToString();
                            break;
                        case totalMode.max:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    max = Math.Max((UInt32)row[FieldName], max);
                                }
                            }
                            if (max != decimal.MinValue)
                            {
                                strValue = max.ToString();
                            }
                            break;
                        case totalMode.min:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    min = Math.Min((UInt32)row[FieldName], min);
                                }
                            }
                            if (min != decimal.MinValue)
                            {
                                strValue = min.ToString();
                            }
                            break;
                    }
                }
                else if (dt.Columns[FieldName].DataType.ToString().ToLower() == "system.uint64")
                {
                    switch (mode)
                    {
                        case totalMode.count:
                            strValue = dt.Rows.Count.ToString();
                            break;
                        case totalMode.sum:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (UInt64)row[FieldName];
                                }
                            }
                            strValue = total.ToString();
                            break;
                        case totalMode.average:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (UInt64)row[FieldName];
                                }
                            }
                            int count = dt.Rows.Count;
                            average = total / count;
                            strValue = average.ToString();
                            break;
                        case totalMode.max:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    max = Math.Max((UInt64)row[FieldName], max);
                                }
                            }
                            if (max != decimal.MinValue)
                            {
                                strValue = max.ToString();
                            }
                            break;
                        case totalMode.min:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    min = Math.Min((UInt64)row[FieldName], min);
                                }
                            }
                            if (min != decimal.MinValue)
                            {
                                strValue = min.ToString();
                            }
                            break;
                    }
                }
                else if (dt.Columns[FieldName].DataType.ToString().ToLower() == "system.int")
                {
                    switch (mode)
                    {
                        case totalMode.count:
                            strValue = dt.Rows.Count.ToString();
                            break;
                        case totalMode.sum:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (int)row[FieldName];
                                }
                            }
                            strValue = total.ToString();
                            break;
                        case totalMode.average:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (int)row[FieldName];
                                }
                            }
                            int count = dt.Rows.Count;
                            average = total / count;
                            strValue = average.ToString();
                            break;
                        case totalMode.max:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    max = Math.Max((int)row[FieldName], max);
                                }
                            }
                            if (max != decimal.MinValue)
                            {
                                strValue = max.ToString();
                            }
                            break;
                        case totalMode.min:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    min = Math.Min((int)row[FieldName], min);
                                }
                            }
                            if (min != decimal.MinValue)
                            {
                                strValue = min.ToString();
                            }
                            break;
                    }
                }
                else if (dt.Columns[FieldName].DataType.ToString().ToLower() == "system.int16")
                {
                    switch (mode)
                    {
                        case totalMode.count:
                            strValue = dt.Rows.Count.ToString();
                            break;
                        case totalMode.sum:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (Int16)row[FieldName];
                                }
                            }
                            strValue = total.ToString();
                            break;
                        case totalMode.average:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (Int16)row[FieldName];
                                }
                            }
                            int count = dt.Rows.Count;
                            average = total / count;
                            strValue = average.ToString();
                            break;
                        case totalMode.max:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    max = Math.Max((Int16)row[FieldName], max);
                                }
                            }
                            if (max != decimal.MinValue)
                            {
                                strValue = max.ToString();
                            }
                            break;
                        case totalMode.min:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    min = Math.Min((Int16)row[FieldName], min);
                                }
                            }
                            if (min != decimal.MinValue)
                            {
                                strValue = min.ToString();
                            }
                            break;
                    }
                }
                else if (dt.Columns[FieldName].DataType.ToString().ToLower() == "system.int32")
                {
                    switch (mode)
                    {
                        case totalMode.count:
                            strValue = dt.Rows.Count.ToString();
                            break;
                        case totalMode.sum:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (Int32)row[FieldName];
                                }
                            }
                            strValue = total.ToString();
                            break;
                        case totalMode.average:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (Int32)row[FieldName];
                                }
                            }
                            int count = dt.Rows.Count;
                            average = total / count;
                            strValue = average.ToString();
                            break;
                        case totalMode.max:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    max = Math.Max((Int32)row[FieldName], max);
                                }
                            }
                            if (max != decimal.MinValue)
                            {
                                strValue = max.ToString();
                            }
                            break;
                        case totalMode.min:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    min = Math.Min((Int32)row[FieldName], min);
                                }
                            }
                            if (min != decimal.MinValue)
                            {
                                strValue = min.ToString();
                            }
                            break;
                    }
                }
                else if (dt.Columns[FieldName].DataType.ToString().ToLower() == "system.int64")
                {
                    switch (mode)
                    {
                        case totalMode.count:
                            strValue = dt.Rows.Count.ToString();
                            break;
                        case totalMode.sum:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (Int64)row[FieldName];
                                }
                            }
                            strValue = total.ToString();
                            break;
                        case totalMode.average:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += (Int64)row[FieldName];
                                }
                            }
                            int count = dt.Rows.Count;
                            average = total / count;
                            strValue = average.ToString();
                            break;
                        case totalMode.max:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    max = Math.Max((Int64)row[FieldName], max);
                                }
                            }
                            if (max != decimal.MinValue)
                            {
                                strValue = max.ToString();
                            }
                            break;
                        case totalMode.min:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    min = Math.Min((Int64)row[FieldName], min);
                                }
                            }
                            if (min != decimal.MinValue)
                            {
                                strValue = min.ToString();
                            }
                            break;
                    }
                }
                else if (dt.Columns[FieldName].DataType.ToString().ToLower() == "system.single")
                {
                    Single stotal = 0;
                    Single smax = Single.MinValue;
                    Single smin = Single.MaxValue;
                    Single saverage = new Single();

                    switch (mode)
                    {
                        case totalMode.count:
                            strValue = dt.Rows.Count.ToString();
                            break;
                        case totalMode.sum:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    stotal += (Single)row[FieldName];
                                }
                            }
                            strValue = stotal.ToString();
                            break;
                        case totalMode.average:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    stotal += (Single)row[FieldName];
                                }
                            }
                            int count = dt.Rows.Count;
                            saverage = stotal / count;
                            strValue = saverage.ToString();
                            break;
                        case totalMode.max:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    smax = Math.Max((Single)row[FieldName], smax);
                                }
                            }
                            if (smax != Single.MinValue)
                            {
                                strValue = smax.ToString();
                            }
                            break;
                        case totalMode.min:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    smin = Math.Min((Single)row[FieldName], smin);
                                }
                            }
                            if (smin != Single.MinValue)
                            {
                                strValue = smin.ToString();
                            }
                            break;
                    }
                }
                else if (dt.Columns[FieldName].DataType.ToString().ToLower() == "system.float")
                {
                    float ftotal = 0;
                    float fmax = float.MinValue;
                    float fmin = float.MaxValue;
                    float faverage = new float();

                    switch (mode)
                    {
                        case totalMode.count:
                            strValue = dt.Rows.Count.ToString();
                            break;
                        case totalMode.sum:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    ftotal += (float)row[FieldName];
                                }
                            }
                            strValue = ftotal.ToString();
                            break;
                        case totalMode.average:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    ftotal += (float)row[FieldName];
                                }
                            }
                            int count = dt.Rows.Count;
                            faverage = ftotal / count;
                            strValue = faverage.ToString();
                            break;
                        case totalMode.max:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    fmax = Math.Max((float)row[FieldName], fmax);
                                }
                            }
                            if (fmax != float.MinValue)
                            {
                                strValue = fmax.ToString();
                            }
                            break;
                        case totalMode.min:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    fmin = Math.Min((float)row[FieldName], fmin);
                                }
                            }
                            if (fmin != float.MinValue)
                            {
                                strValue = fmin.ToString();
                            }
                            break;
                    }
                }
                else if (dt.Columns[FieldName].DataType.ToString().ToLower() == "system.double")
                {
                    double dtotal = 0;
                    double dmax = double.MinValue;
                    double dmin = double.MaxValue;
                    double daverage = new double();
                    switch (mode)
                    {
                        case totalMode.count:
                            strValue = dt.Rows.Count.ToString();
                            break;
                        case totalMode.sum:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    dtotal += (double)row[FieldName];
                                }
                            }
                            strValue = dtotal.ToString();
                            break;
                        case totalMode.average:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    dtotal += (double)row[FieldName];
                                }
                            }
                            int count = dt.Rows.Count;
                            daverage = dtotal / count;
                            strValue = daverage.ToString();
                            break;
                        case totalMode.max:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    dmax = Math.Max((double)row[FieldName], dmax);
                                }
                            }
                            if (dmax != double.MinValue)
                            {
                                strValue = dmax.ToString();
                            }
                            break;
                        case totalMode.min:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    dmin = Math.Min((double)row[FieldName], dmin);
                                }
                            }
                            if (dmin != double.MinValue)
                            {
                                strValue = dmin.ToString();
                            }
                            break;
                    }
                }
                else if (dt.Columns[FieldName].DataType.ToString().ToLower() == "system.decimal"
                    || !string.IsNullOrEmpty(dt.Columns[FieldName].Expression))
                {
                    switch (mode)
                    {
                        case totalMode.count:
                            strValue = dt.Rows.Count.ToString();
                            break;
                        case totalMode.sum:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += Convert.ToDecimal(row[FieldName]);
                                }
                            }
                            strValue = total.ToString();
                            break;
                        case totalMode.average:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState != DataRowState.Deleted
                                    && row[FieldName] != null
                                    && row[FieldName].ToString() != "")
                                {
                                    total += Convert.ToDecimal(row[FieldName]);
                                }
                            }
                            int count = dt.Rows.Count;
                            average = total / count;
                            strValue = average.ToString();
                            break;
                        case totalMode.max:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    max = Math.Max(Convert.ToDecimal(row[FieldName]), max);
                                }
                            }
                            if (max != decimal.MinValue)
                            {
                                strValue = max.ToString();
                            }
                            break;
                        case totalMode.min:
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[FieldName] != null
                                    && row[FieldName].ToString() != ""
                                    && row.RowState != DataRowState.Deleted)
                                {
                                    min = Math.Min(Convert.ToDecimal(row[FieldName]), min);
                                }
                            }
                            if (min != decimal.MinValue)
                            {
                                strValue = min.ToString();
                            }
                            break;
                    }
                }
                else
                {
                    if (mode == totalMode.count)
                    {
                        strValue = dt.Rows.Count.ToString();
                    }
                }
                if (!string.IsNullOrEmpty(format))
                {
                    //format
                    switch (mode)
                    {
                        case totalMode.sum:
                            {
                                strValue = total.ToString(format);
                                break;
                            }
                        case totalMode.average:
                            {
                                strValue = average.ToString(format);
                                break;
                            }
                        case totalMode.max:
                            {
                                if (max != decimal.MinValue)
                                {
                                    strValue = max.ToString(format);
                                }
                                break;
                            }
                        case totalMode.min:
                            {
                                if (min != decimal.MinValue)
                                {
                                    strValue = min.ToString(format);
                                }
                                break;
                            }

                    }
                }
            }
            return strValue;
        }

        internal List<object> PreAddKeys = new List<object>();
        internal List<object> PreAddValues = new List<object>();
        private void doDefaultAfterInsert()
        {
            int i = this.FooterRow.Cells.Count;
            for (int j = 0; j < i; j++)
            {
                string FieldName = "";
                if (this.Columns[j].HeaderText != null && this.Columns[j].HeaderText != "")
                {
                    if (this.Columns[j] is BoundField)
                    {
                        FieldName = ((BoundField)this.Columns[j]).DataField;
                    }
                    else
                    {
                        FieldName = this.Columns[j].SortExpression;
                    }

                    object value = null;
                    CellControls.Clear();
                    GetAllCellControls(this.FooterRow.Cells[j]);
                    foreach (Control ctrl in CellControls)
                    {
                        if (ctrl is TextBox)
                        {
                            value = ((TextBox)ctrl).Text;
                            break;
                        }
                        else if (ctrl is Label)
                        {
                            value = ((Label)ctrl).Text;
                            break;
                        }
                        else if (ctrl is DropDownList)
                        {
                            value = ((DropDownList)ctrl).SelectedValue;
                            break;
                        }
                        else if (ctrl is CheckBox)
                        {
                            value = ((CheckBox)ctrl).Checked;
                            break;
                        }
                        else if (ctrl is WebRefValBase)
                        {
                            value = ((WebRefValBase)ctrl).BindingValue;
                            break;
                        }
                        else if (ctrl is IDateTimePicker)
                        {
                            if (((IDateTimePicker)ctrl).DateTimeType == dateTimeType.DateTime)
                            {
                                value = ((IDateTimePicker)ctrl).Text;
                            }
                            else if (((IDateTimePicker)ctrl).DateTimeType == dateTimeType.VarChar)
                            {
                                value = ((IDateTimePicker)ctrl).DateString;
                            }
                        }
                    }
                    PreAddKeys.Add(FieldName);
                    PreAddValues.Add(value);
                }
            }
            this.ViewState["PreAddKeys"] = PreAddKeys;
            this.ViewState["PreAddValues"] = PreAddValues;
            // 新增时定位
            int pageSize = this.PageSize;
            int pageCount = this.PageCount;
            int maxRecCount = pageSize * pageCount;
            object obj = this.GetObjByID(this.DataSourceID);

            if (obj != null && obj is WebDataSource)
            {
                WebDataSource wds = (WebDataSource)obj;
                DataTable table = wds.View.Table;
                int realRecCount = table.Rows.Count;
                if (realRecCount <= maxRecCount)
                {
                    this.PageIndex = pageCount - 1;
                }
                else
                {
                    this.PageIndex = pageCount;
                }

            }
        }

        protected override void OnDataBound(EventArgs e)
        {
            base.OnDataBound(e);
            if (this.DesignMode) return;
            if (this.BottomPagerRow != null)
            {
                switch (this.PagerSettings.Position)
                {
                    case PagerPosition.Bottom:
                        this.BottomPagerRow.Visible = true;
                        break;
                    case PagerPosition.Top:
                        this.TopPagerRow.Visible = true;
                        break;
                    case PagerPosition.TopAndBottom:
                        this.TopPagerRow.Visible = true;
                        this.BottomPagerRow.Visible = true;
                        break;
                }
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

        protected override void OnRowUpdating(GridViewUpdateEventArgs e)
        {
            WebValidate validate = (WebValidate)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
            bool findCtrl = (validate != null);

            IOrderedDictionary newvals = e.NewValues;
            IOrderedDictionary oldvals = e.OldValues;
            if (findCtrl)
            {
                object obj = this.GetObjByID(this.DataSourceID);
                if (validate.DuplicateCheck && obj is WebDataSource)
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
                            else if (e.OldValues.Contains(columnName))
                            {
                                value[i] = e.OldValues[columnName];
                            }
                            else if (wds.RelationValues != null && wds.RelationValues.Contains(columnName))
                            {
                                value[i] = wds.RelationValues[columnName];
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
                validate.Text = string.Empty;
                bool validateSucess = validate.CheckValidate(newvals);
                if (!validateSucess)
                {
                    e.Cancel = true;
                    ValidateFailed = true;
                    ValidateFormat();
                }
                else
                {
                    ValidateFailed = false;
                }
            }
            base.OnRowUpdating(e);
        }

        protected override void OnRowCancelingEdit(GridViewCancelEditEventArgs e)
        {
            WebValidate validate = (WebValidate)this.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
            if (validate != null)
            {
                validate.Text = string.Empty;
            }
            base.OnRowCancelingEdit(e);
            WebNavigator nav = this.GetBindingNavigator();
            WebDataSource wds = this.GetObjByID(this.DataSourceID) as WebDataSource;
            if (nav != null)
            {
                if (this.DataHasChanged)
                {
                    nav.SetState(WebNavigator.NavigatorState.Changed);
                    nav.SetNavState("Changing");
                }
                else if (wds != null && string.IsNullOrEmpty(wds.MasterDataSource))
                {
                    nav.SetState(WebNavigator.NavigatorState.Browsing);
                    nav.SetNavState("Browsed");
                }
            }
        }

        [Category("Infolight"),
        DefaultValue(true),
        Description("Indicate whether to post back MultiCheckBoxes automatically")]
        public bool AutoPostBackMultiCheckBoxes
        {
            get
            {
                object obj = this.ViewState["AutoPostBackMultiCheckBoxes"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["AutoPostBackMultiCheckBoxes"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(true),
        Description("Indicate whether to post back WebGridTextBoxes automatically")]
        public bool AutoPostBackWebGridTextBoxes
        {
            get
            {
                object obj = this.ViewState["AutoPostBackWebGridTextBoxes"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["AutoPostBackWebGridTextBoxes"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(true),
        Description("Indicate whether to post back WebGridDropDowns automatically")]
        public bool AutoPostBackWebGridDropDowns
        {
            get
            {
                object obj = this.ViewState["AutoPostBackWebGridDropDowns"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["AutoPostBackWebGridDropDowns"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(true)]
        public bool MultiEditCheckUsers
        {
            get
            {
                object obj = this.ViewState["MultiEditCheckUsers"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["MultiEditCheckUsers"] = value;
            }
        }

        public void PostBackMultiCheckBoxes()
        {
            string phyPath = this.Page.Request.PhysicalPath;
            //phyPath = phyPath.Substring(0, phyPath.LastIndexOf(".aspx"));
            //string xmlPath = phyPath + ".xml";
            string xmlPath = CliUtils.ReplaceFileName(phyPath, ".aspx", ".xml");

            if (File.Exists(xmlPath))
            {
                FileStream stream = new FileStream(xmlPath, FileMode.Open);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(stream);
                XmlNode nRoot = xmlDoc.SelectSingleNode("InfoLight");
                if (nRoot != null)
                {
                    XmlNode nWebCheckBoxes = nRoot.SelectSingleNode("WebCheckBoxes");
                    if (nWebCheckBoxes != null)
                    {
                        XmlNode nWebGridView = nWebCheckBoxes.SelectSingleNode("WebGridView[@ID='" + this.ID + "']");
                        if (nWebGridView != null)
                        {
                            object obj = this.GetObjByID(this.DataSourceID);
                            if (obj != null)
                            {
                                WebDataSource wds = (WebDataSource)obj;
                                if (!string.IsNullOrEmpty(this.MultiCheckColumn) && wds.View.Table.Columns[this.MultiCheckColumn].DataType == typeof(bool))
                                {
                                    string[] keyTypes = new string[wds.PrimaryKey.Length];
                                    for (int i = 0; i < wds.PrimaryKey.Length; i++)
                                    {
                                        keyTypes[i] = wds.PrimaryKey[i].DataType.ToString().ToLower();
                                    }
                                    XmlNode nCheckBox = nWebGridView.FirstChild;
                                    string keyValues = "", checkState = "";
                                    while (nCheckBox != null)
                                    {
                                        keyValues = nCheckBox.Attributes["KeyValues"].Value;
                                        checkState = nCheckBox.InnerText;
                                        string[] arrayValues = keyValues.Split(';');
                                        object[] objValues = FindKeyValues(arrayValues, keyTypes);
                                        DataRow row = wds.InnerDataSet.Tables[wds.DataMember].Rows.Find(objValues);
                                        if (row != null)
                                        {
                                            row[this.MultiCheckColumn] = Convert.ToBoolean(checkState);
                                        }
                                        nCheckBox = nCheckBox.NextSibling;
                                    }

                                    int x = nWebGridView.ChildNodes.Count;
                                    for (int y = x - 1; y >= 0; y--)
                                    {
                                        XmlNode cNode = nWebGridView.ChildNodes[y];
                                        if (cNode.Attributes["User"].Value == CliUtils.fLoginUser)
                                        {
                                            nWebGridView.RemoveChild(cNode);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                stream.Close();
                xmlDoc.Save(xmlPath);
            }
        }

        public void PostBackMultiTextBoxes()
        {
            string phyPath = this.Page.Request.PhysicalPath;
            //phyPath = phyPath.Substring(0, phyPath.LastIndexOf(".aspx"));
            //string xmlPath = phyPath + ".xml";
            string xmlPath = CliUtils.ReplaceFileName(phyPath, ".aspx", ".xml");

            if (File.Exists(xmlPath))
            {
                FileStream stream = new FileStream(xmlPath, FileMode.Open);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(stream);
                XmlNode nRoot = xmlDoc.SelectSingleNode("InfoLight");
                if (nRoot != null)
                {
                    XmlNode nWebGridTextBoxes = nRoot.SelectSingleNode("WebGridTextBoxes");
                    if (nWebGridTextBoxes != null)
                    {
                        XmlNode nWebGridView = nWebGridTextBoxes.SelectSingleNode("WebGridView[@ID='" + this.ID + "']");
                        if (nWebGridView != null)
                        {
                            object obj = this.GetObjByID(this.DataSourceID);
                            if (obj != null)
                            {
                                WebDataSource wds = (WebDataSource)obj;
                                string[] keyTypes = new string[wds.PrimaryKey.Length];
                                for (int i = 0; i < wds.PrimaryKey.Length; i++)
                                {
                                    keyTypes[i] = wds.PrimaryKey[i].DataType.ToString().ToLower();
                                }
                                XmlNode nRow = nWebGridView.FirstChild;
                                string keyValues = "";
                                while (nRow != null)
                                {
                                    keyValues = nRow.Attributes["KeyValues"].Value;
                                    string[] arrayValues = keyValues.Split(';');
                                    object[] objValues = FindKeyValues(arrayValues, keyTypes);
                                    DataRow row = wds.InnerDataSet.Tables[wds.DataMember].Rows.Find(objValues);
                                    if (row != null)
                                    {
                                        XmlNode nTextBox = nRow.FirstChild;
                                        while (nTextBox != null)
                                        {
                                            string fieldName = nTextBox.Attributes["FieldName"].Value;
                                            string fieldType = wds.InnerDataSet.Tables[wds.DataMember].Columns[fieldName].DataType.ToString().ToLower();
                                            row[fieldName] = GetFieldValue(nTextBox.InnerText, fieldType);
                                            nTextBox = nTextBox.NextSibling;
                                        }
                                    }
                                    nRow = nRow.NextSibling;
                                }
                                foreach (XmlNode node in nWebGridView.ChildNodes)
                                {
                                    int x = node.ChildNodes.Count;
                                    for (int y = x - 1; y >= 0; y--)
                                    {
                                        XmlNode cNode = node.ChildNodes[y];
                                        if (cNode.Attributes["User"].Value == CliUtils.fLoginUser)
                                        {
                                            node.RemoveChild(cNode);
                                        }
                                    }
                                    if (!node.HasChildNodes)
                                    {
                                        nWebGridView.RemoveChild(node);
                                    }
                                }
                            }
                        }
                    }
                }
                stream.Close();
                xmlDoc.Save(xmlPath);

            }
        }

        public void PostBackMultiDropDowns()
        {
            string phyPath = this.Page.Request.PhysicalPath;
            //phyPath = phyPath.Substring(0, phyPath.LastIndexOf(".aspx"));
            //string xmlPath = phyPath + ".xml";
            string xmlPath = CliUtils.ReplaceFileName(phyPath, ".aspx", ".xml");

            if (File.Exists(xmlPath))
            {
                FileStream stream = new FileStream(xmlPath, FileMode.Open);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(stream);
                XmlNode nRoot = xmlDoc.SelectSingleNode("InfoLight");
                if (nRoot != null)
                {
                    XmlNode nWebGridDropDowns = nRoot.SelectSingleNode("WebGridDropDowns");
                    if (nWebGridDropDowns != null)
                    {
                        XmlNode nWebGridView = nWebGridDropDowns.SelectSingleNode("WebGridView[@ID='" + this.ID + "']");
                        if (nWebGridView != null)
                        {
                            object obj = this.GetObjByID(this.DataSourceID);
                            if (obj != null)
                            {
                                WebDataSource wds = (WebDataSource)obj;
                                string[] keyTypes = new string[wds.PrimaryKey.Length];
                                for (int i = 0; i < wds.PrimaryKey.Length; i++)
                                {
                                    keyTypes[i] = wds.PrimaryKey[i].DataType.ToString().ToLower();
                                }
                                XmlNode nRow = nWebGridView.FirstChild;
                                string keyValues = "";
                                while (nRow != null)
                                {
                                    keyValues = nRow.Attributes["KeyValues"].Value;
                                    string[] arrayValues = keyValues.Split(';');
                                    object[] objValues = FindKeyValues(arrayValues, keyTypes);
                                    DataRow row = wds.InnerDataSet.Tables[wds.DataMember].Rows.Find(objValues);
                                    if (row != null)
                                    {
                                        XmlNode nDropDown = nRow.FirstChild;
                                        while (nDropDown != null)
                                        {
                                            string fieldName = nDropDown.Attributes["FieldName"].Value;
                                            string fieldType = wds.InnerDataSet.Tables[wds.DataMember].Columns[fieldName].DataType.ToString().ToLower();
                                            row[fieldName] = GetFieldValue(nDropDown.InnerText, fieldType);
                                            nDropDown = nDropDown.NextSibling;
                                        }
                                    }
                                    nRow = nRow.NextSibling;
                                }
                                foreach (XmlNode node in nWebGridView.ChildNodes)
                                {
                                    int x = node.ChildNodes.Count;
                                    for (int y = x - 1; y >= 0; y--)
                                    {
                                        XmlNode cNode = node.ChildNodes[y];
                                        if (cNode.Attributes["User"].Value == CliUtils.fLoginUser)
                                        {
                                            node.RemoveChild(cNode);
                                        }
                                    }
                                    if (!node.HasChildNodes)
                                    {
                                        nWebGridView.RemoveChild(node);
                                    }
                                }
                            }
                        }
                    }
                }
                stream.Close();
                xmlDoc.Save(xmlPath);

            }
        }

        private object[] FindKeyValues(string[] arrayValues, string[] keyTypes)
        {
            object[] objValues = new object[arrayValues.Length];

            if (objValues.Length == keyTypes.Length)
            {
                int i = keyTypes.Length;
                for (int j = 0; j < i; j++)
                {
                    string type = keyTypes[j];
                    if (type == "system.uint" || type == "system.uint32")
                    {
                        objValues[j] = Convert.ToUInt32(arrayValues[j]);
                    }
                    else if (type == "system.uint16")
                    {
                        objValues[j] = Convert.ToUInt16(arrayValues[j]);
                    }
                    else if (type == "system.uint64")
                    {
                        objValues[j] = Convert.ToUInt64(arrayValues[j]);
                    }
                    else if (type == "system.int" || type == "system.int32")
                    {
                        objValues[j] = Convert.ToInt32(arrayValues[j]);
                    }
                    else if (type == "system.int16")
                    {
                        objValues[j] = Convert.ToInt16(arrayValues[j]);
                    }
                    else if (type == "system.int64")
                    {
                        objValues[j] = Convert.ToInt64(arrayValues[j]);
                    }
                    else if (type == "system.single" || type == "system.float")
                    {
                        objValues[j] = Convert.ToSingle(arrayValues[j]);
                    }
                    else if (type == "system.double")
                    {
                        objValues[j] = Convert.ToDouble(arrayValues[j]);
                    }
                    else if (type == "system.decimal")
                    {
                        objValues[j] = Convert.ToDecimal(arrayValues[j]);
                    }
                    else if (type == "system.datetime")
                    {
                        objValues[j] = Convert.ToDateTime(arrayValues[j]);
                    }
                    else if (type == "system.boolean")
                    {
                        objValues[j] = Convert.ToBoolean(arrayValues[j]);
                    }
                    else
                    {
                        objValues[j] = arrayValues[j];
                    }
                }
            }
            return objValues;
        }

        private object GetFieldValue(string value, string type)
        {
            object retValue = null;
            if (type == "system.uint" || type == "system.uint32")
            {
                retValue = Convert.ToUInt32(value);
            }
            else if (type == "system.uint16")
            {
                retValue = Convert.ToUInt16(value);
            }
            else if (type == "system.uint64")
            {
                retValue = Convert.ToUInt64(value);
            }
            else if (type == "system.int" || type == "system.int32")
            {
                retValue = Convert.ToInt32(value);
            }
            else if (type == "system.int16")
            {
                retValue = Convert.ToInt16(value);
            }
            else if (type == "system.int64")
            {
                retValue = Convert.ToInt64(value);
            }
            else if (type == "system.single" || type == "system.float")
            {
                retValue = Convert.ToSingle(value);
            }
            else if (type == "system.double")
            {
                retValue = Convert.ToDouble(value);
            }
            else if (type == "system.decimal")
            {
                retValue = Convert.ToDecimal(value);
            }
            else if (type == "system.datetime")
            {
                retValue = Convert.ToDateTime(value);
            }
            else if (type == "system.boolean")
            {
                retValue = Convert.ToBoolean(value);
            }
            else
            {
                retValue = value;
            }
            return retValue;
        }

        public void ToExcel()
        {
            if (!string.IsNullOrEmpty(this.DataSourceID))
            {
                object obj = this.GetObjByID(this.DataSourceID);
                if (obj != null && obj is WebDataSource)
                {
                    //取出所有资料
                    WebDataSource wds = obj as WebDataSource;
                    DataView view = wds.View;
                    List<string> columns = new List<string>();
                    Hashtable controlTable = new Hashtable();//存放WebRefVal

                    InfoDataSet ids = new InfoDataSet();
                    ids.PacketRecords = -1;
                    ids.RemoteName = wds.RemoteName;
                    DataSet ds = ids.RealDataSet;
                    ds.Tables.Add(wds.View.Table.TableName);

                    for (int i = 0; i < this.Columns.Count; i++)
                    {

                        if (this.Columns[i].Visible)
                        {
                            if (this.Columns[i] is BoundField)
                            {
                                columns.Add((this.Columns[i] as BoundField).DataField);
                                ds.Tables[0].Columns.Add((this.Columns[i] as BoundField).DataField);
                            }
                            else if (this.Columns[i] is TemplateField && !string.IsNullOrEmpty(this.Columns[i].SortExpression))
                            {
                                columns.Add(this.Columns[i].SortExpression);
                                ds.Tables[0].Columns.Add(this.Columns[i].SortExpression);
                                if (this.Rows.Count > 0)
                                {
                                    foreach (Control ctrl in this.Rows[0].Cells[i].Controls)
                                    {
                                        if (ctrl is WebRefVal)
                                        {
                                            controlTable.Add(this.Columns[i].SortExpression, ctrl);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < view.Count; i++)
                    {
                        object[] values = new object[columns.Count];
                        for (int j = 0; j < columns.Count; j++)
                        {
                            object value = view[i][columns[j]];
                            if (controlTable.Contains(columns[j]))
                            {
                                if (value != null)
                                {
                                    WebRefVal webRefVal = controlTable[columns[j]] as WebRefVal;
                                    values[j] = webRefVal.GetDisplayByValue(value.ToString());
                                }
                            }
                            else
                            {
                                values[j] = value;
                            }
                        }
                        ds.Tables[0].Rows.Add(values);
                    }
                    ds.Tables[0].AcceptChanges();

                    string path = this.Page.MapPath(this.Page.AppRelativeVirtualPath);
                    string directory = Path.GetDirectoryName(path);
                    string filename = Path.GetFileNameWithoutExtension(path);
                    path = string.Format("{0}\\ExcelDoc\\{1}", directory, string.Format("{0}-{1:yyMMddHHmmss}", filename, DateTime.Now));
                    path = Path.ChangeExtension(path, "xls");
                    DataSetToExcel dste = new DataSetToExcel();
                    dste.Excelpath = path;
                    dste.DataSet = ids;
                    dste.Export();

                    FileInfo file = new FileInfo(path);
                    System.Web.HttpResponse Response = this.Page.Response;
                    Response.Clear();
                    Response.Buffer = false;
                    Response.ContentType = "application/x-msdownload";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.Filter.Close();
                    Response.WriteFile(file.FullName);
                    Response.End();
                }
            }
        }

        public void ExecuteSync(GridViewCommandEventArgs e)
        {
            var dataSource = this.Page.FindControl(this.DataSourceID) as WebDataSource;
            if (!String.IsNullOrEmpty(dataSource.MasterDataSource))
            {
                var masterDataSource = this.Page.FindControl(dataSource.MasterDataSource) as WebDataSource;
                WebFormView wfvMaster = null;
                foreach (Control item in this.Page.Controls)
                {
                    if (item is WebFormView)
                    {
                        wfvMaster = item as WebFormView;
                        break;
                    }
                    if (item.Controls != null)
                    {
                        wfvMaster = FindWebFormView(item.Controls, dataSource.MasterDataSource);
                        if (wfvMaster != null)
                            break;
                    }
                }
                if (wfvMaster != null)
                {
                    dataSource.ExecuteSelect(wfvMaster);
                    this.DataBind();
                }
            }
        }

        private WebFormView FindWebFormView(ControlCollection controls, String datasourcrid)
        {
            WebFormView returnValue = null;
            foreach (Control item in controls)
            {
                if (item is WebFormView)
                {
                    PropertyInfo piDataSourceID = item.GetType().GetProperty("DataSourceID");
                    var objDataSourceID = piDataSourceID.GetValue(item, null);
                    if (objDataSourceID != null && objDataSourceID.ToString() == datasourcrid)
                        return item as WebFormView;
                }
                if (item.Controls != null)
                {
                    returnValue = FindWebFormView(item.Controls, datasourcrid);
                    if (returnValue != null)
                        return returnValue;
                }
            }
            return returnValue;
        }
    }

    #region AddNewRowControlCollection class
    public class AddNewRowControlCollection : InfoOwnerCollection
    {
        public AddNewRowControlCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(AddNewRowControlItem))
        {
        }

        public new AddNewRowControlItem this[int index]
        {
            get
            {
                return (AddNewRowControlItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is AddNewRowControlItem)
                    {
                        //原来的Collection设置为0
                        ((AddNewRowControlItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((AddNewRowControlItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AddNewRowControlItem : InfoOwnerCollectionItem, IGetValues
    {
        public AddNewRowControlItem()
        { }

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

        private WebGridView.AddNewRowControlType _ControlType;
        [NotifyParentProperty(true)]
        public WebGridView.AddNewRowControlType ControlType
        {
            get
            {
                return _ControlType;
            }
            set
            {
                _ControlType = value;
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

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            if (sKind.ToLower().Equals("fieldname"))
            {
                if (this.Owner is WebGridView)
                {
                    WebGridView gridView = (WebGridView)this.Owner;
                    if (gridView.Page != null && gridView.DataSourceID != null && gridView.DataSourceID != "")
                    {
                        object obj = gridView.GetObjByID(gridView.DataSourceID);
                        if (obj is WebDataSource && ((WebDataSource)obj).ID == gridView.DataSourceID)
                        {
                            WebDataSource ds = (WebDataSource)obj;
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
                                DataTable dt = ds.DesignDataSet.Tables[ds.DataMember];
                                if (dt != null)
                                {
                                    int i = dt.Columns.Count;
                                    retList = new string[i];
                                    for (int j = 0; j < i; j++)
                                    {
                                        retList[j] = dt.Columns[j].ColumnName;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (sKind.ToLower().Equals("controlid"))
            {
                if (this.Owner is WebGridView)
                {
                    WebGridView gridView = (WebGridView)this.Owner;
                    List<string> ctrlNames = new List<string>();
                    IDesignerHost host = (IDesignerHost)gridView.Site.GetService(typeof(IDesignerHost));
                    foreach (DataControlField field in gridView.Columns)
                    {
                        if (field is TemplateField)
                        {
                            TemplateField tfield = (TemplateField)field;
                            if (tfield.FooterTemplate != null)
                            {
                                TemplateBuilder builder = (TemplateBuilder)tfield.FooterTemplate;
                                Control[] ctrls = ControlParser.ParseControls(host, builder.Text);
                                int i = ctrls.Length;
                                for (int j = 0; j < i; j++)
                                {
                                    if (!(ctrls[j] is LiteralControl))
                                    {
                                        ctrlNames.Add(ctrls[j].ID);
                                    }
                                }
                            }
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
        #endregion
    }
    #endregion

    #region GridViewInnerNavigatorCollection class
    [Editor(typeof(GridViewInnerNavigatorEditor), typeof(UITypeEditor))]
    public class GridViewInnerNavigatorCollection : InfoOwnerCollection
    {
        public bool bInit = false;
        public GridViewInnerNavigatorCollection(Object aOwner, Type aItemType)
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

    #region GridViewOpenParamCollection class
    public class GridViewOpenParamCollection : InfoOwnerCollection
    {
        public GridViewOpenParamCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(GridViewOpenParam))
        {
        }

        public new GridViewOpenParam this[int index]
        {
            get
            {
                return (GridViewOpenParam)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is GridViewOpenParam)
                    {
                        //原来的Collection设置为0
                        ((GridViewOpenParam)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((GridViewOpenParam)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GridViewOpenParam : InfoOwnerCollectionItem
    {
        public GridViewOpenParam()
        { }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get
            {
                return _ParamName;
            }
            set
            {
                _ParamName = value;
            }
        }

        private string _ParamName;
        [NotifyParentProperty(true)]
        public string ParamName
        {
            get
            {
                return _ParamName;
            }
            set
            {
                _ParamName = value;
            }
        }
    }
    #endregion

    #region GridViewTotalCollection class
    public class GridViewTotalCollection : InfoOwnerCollection
    {
        public GridViewTotalCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(GridViewTotalItem))
        {
        }

        public new GridViewTotalItem this[int index]
        {
            get
            {
                return (GridViewTotalItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is GridViewTotalItem)
                    {
                        //原来的Collection设置为0
                        ((GridViewTotalItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((GridViewTotalItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GridViewTotalItem : InfoOwnerCollectionItem, IGetValues
    {
        public GridViewTotalItem()
        { }

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

        private bool _ShowTotal = true;
        [NotifyParentProperty(true),
        DefaultValue(true)]
        public bool ShowTotal
        {
            get
            {
                return _ShowTotal;
            }
            set
            {
                _ShowTotal = value;
            }
        }

        private totalMode _TotalMode = totalMode.sum;
        [NotifyParentProperty(true),
        DefaultValue(totalMode.sum)]
        public totalMode TotalMode
        {
            get
            {
                return _TotalMode;
            }
            set
            {
                _TotalMode = value;
            }
        }

        private string _format;
        [NotifyParentProperty(true)]
        public string Format
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            if (sKind.ToLower().Equals("fieldname"))
            {
                if (this.Owner is WebGridView)
                {
                    DataControlFieldCollection dcfc = ((WebGridView)this.Owner).Columns;
                    if (dcfc != null && dcfc.Count > 0)
                    {
                        int i = dcfc.Count;
                        List<string> arrItems = new List<string>();
                        for (int j = 0; j < i; j++)
                        {
                            if (dcfc[j] is BoundField)
                            {
                                arrItems.Add(((BoundField)dcfc[j]).DataField);
                            }
                            else if (dcfc[j] is ExpressionField)
                            {
                                arrItems.Add(((ExpressionField)dcfc[j]).Expression);
                            }
                            else if (dcfc[j].SortExpression != null && dcfc[j].SortExpression != "")
                            {
                                arrItems.Add(dcfc[j].SortExpression);
                            }
                        }
                        int x = arrItems.Count;
                        retList = new string[x];
                        for (int y = 0; y < x; y++)
                        {
                            retList[y] = arrItems[y];
                        }
                    }
                }
            }
            return retList;
        }
    }
    #endregion

    #region WebGridViewDesigner class
    public class WebGridViewDesigner : GridViewDesigner
    {
        public WebGridViewDesigner()
        {
            DesignerVerb[] verbs = new DesignerVerb[2];
            verbs[0] = new DesignerVerb("AdjustColumns", new EventHandler(OnAdjustColumns));
            verbs[1] = new DesignerVerb("copy AddNew controls", new EventHandler(OnCopyAddNewControls));
            this.Verbs.AddRange(verbs);
        }

        public void OnAdjustColumns(object sender, EventArgs e)
        {
            WebGridView gridView = (WebGridView)this.Component;
            List<int> totalLength = TotalLength(gridView);
            int tLength = 0;
            foreach (int it in totalLength)
            {
                tLength += it;
            }
            int i = totalLength.Count;
            for (int j = 0; j < i; j++)
            {
                string swidth = ((Convert.ToSingle(totalLength[j]) / Convert.ToSingle(tLength)) * 100).ToString() + "%";
                Unit width = new Unit(swidth);
                gridView.Columns[j].HeaderStyle.Width = width;
            }
            this.Tag.SetDirty(true);
        }

        public void OnCopyAddNewControls(object sender, EventArgs e)
        {
            WebGridView gridView = (WebGridView)this.Component;
            IDesignerHost host = (IDesignerHost)this.GetService(typeof(IDesignerHost));
            ControlDesigner designer = (ControlDesigner)host.GetDesigner(gridView);
            foreach (TemplateGroup group in designer.TemplateGroups)
            {
                if (group.GroupName != "EmptyDataTemplate" && group.GroupName != "PagerTemplate")
                {
                    string fieldName = "";
                    bool foundField = false;
                    foreach (TemplateDefinition def in group.Templates)
                    {
                        string content = def.Content;
                        if (content != "")
                        {
                            Control[] addCtrls = ControlParser.ParseControls(host, content);
                            if (def.Name == "ItemTemplate" || def.Name == "EditTemplate")
                            {
                                if (!foundField)
                                {
                                    foreach (Control ctrl in addCtrls)
                                    {
                                        IDataBindingsAccessor dbAccess = (IDataBindingsAccessor)ctrl;
                                        if (dbAccess.HasDataBindings)
                                        {
                                            foreach (DataBinding binding in dbAccess.DataBindings)
                                            {
                                                string[] ExpParts = binding.Expression.Split('"');
                                                if (ExpParts.Length >= 3)
                                                {
                                                    fieldName = ExpParts[1];
                                                    foundField = true;
                                                    break;
                                                }
                                            }
                                            if (foundField)
                                                break;
                                        }
                                    }
                                }
                            }
                            else if (def.Name == "FooterTemplate")
                            {
                                foreach (Control ctrl in addCtrls)
                                {
                                    if (!(ctrl is LiteralControl))
                                    {
                                        AddNewRowControlItem item = new AddNewRowControlItem();
                                        if (ctrl is CheckBox)
                                        {
                                            item.ControlType = WebGridView.AddNewRowControlType.CheckBox;
                                        }
                                        else if (ctrl is IDateTimePicker)
                                        {
                                            item.ControlType = WebGridView.AddNewRowControlType.DateTimePicker;
                                        }
                                        else if (ctrl is DropDownList)
                                        {
                                            item.ControlType = WebGridView.AddNewRowControlType.DropDownList;
                                        }
                                        else if (ctrl is Label)
                                        {
                                            item.ControlType = WebGridView.AddNewRowControlType.Label;
                                        }
                                        else if (ctrl is WebRefValBase)
                                        {
                                            item.ControlType = WebGridView.AddNewRowControlType.RefVal;
                                        }
                                        else if (ctrl is TextBox)
                                        {
                                            item.ControlType = WebGridView.AddNewRowControlType.TextBox;
                                        }
                                        else if (ctrl is HtmlInputText)
                                        {
                                            item.ControlType = WebGridView.AddNewRowControlType.HtmlInputText;
                                        }
                                        else if (ctrl is WebImage)
                                        {
                                            item.ControlType = WebGridView.AddNewRowControlType.WebImage;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                        item.ControlID = ctrl.ID;
                                        if (foundField)
                                        {
                                            item.FieldName = fieldName;
                                        }
                                        gridView.AddNewRowControls.Add(item);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    this.Tag.SetDirty(true);
                }
            }
        }

        private List<int> TotalLength(WebGridView gridView)
        {
            List<int> totalLength = new List<int>();
            object obj = gridView.GetObjByID(gridView.DataSourceID);
            if (obj is WebDataSource)
            {
                DataTable table = DBUtils.GetDataDictionary(obj as WebDataSource, true).Tables[0];
                foreach (DataControlField field in gridView.Columns)
                {
                    if (field is BoundField)
                    {
                        BoundField bfield = (BoundField)field;
                        bool bDefined = false;
                        foreach (DataRow row in table.Rows)
                        {
                            if (bfield.DataField == row["field_name"].ToString())
                            {
                                if (row["field_length"] != null && row["field_length"].ToString() != "")
                                    totalLength.Add(Convert.ToInt16(row["field_length"]));
                                else
                                    totalLength.Add(10);
                                bDefined = true;
                                break;
                            }
                        }
                        if (!bDefined)
                        {
                            totalLength.Add(10);
                        }
                    }
                    else if (field is TemplateField)
                    {
                        TemplateField tfield = (TemplateField)field;
                        bool bDefined = false;
                        foreach (DataRow row in table.Rows)
                        {
                            if (tfield.SortExpression == row["field_name"].ToString())
                            {
                                if (row["field_length"] != null && row["field_length"].ToString() != "")
                                    totalLength.Add(Convert.ToInt16(row["field_length"]));
                                else
                                    totalLength.Add(10);
                                bDefined = true;
                                break;
                            }
                            else if (tfield.HeaderText == row["caption"].ToString())
                            {
                                if (row["field_length"] != null && row["field_length"].ToString() != "")
                                    totalLength.Add(Convert.ToInt16(row["field_length"]));
                                else
                                    totalLength.Add(10);
                                bDefined = true;
                                break;
                            }
                        }
                        if (!bDefined)
                        {
                            totalLength.Add(10);
                        }
                    }
                    else if (field is ButtonField)
                    {
                        totalLength.Add(20);
                    }
                    else
                    {
                        totalLength.Add(10);
                    }
                }
                if (totalLength.Count == gridView.Columns.Count)
                {
                    return totalLength;
                }
            }
            return new List<int>();
        }

        public void ChangeSchema(bool forceUpdateView)
        {
            WebGridView grid = (WebGridView)this.Component;
            if (this.Component is WebGridView && grid.Page != null && grid.Columns.Count == 0)
            {
                DataTable dt = new DataTable();
                ControlCollection ctrls = grid.Page.Controls;
                foreach (Control ctrl in ctrls)
                {
                    if (ctrl.ID == grid.DataSourceID && ctrl is WebDataSource)
                    {
                        dt = ((WebDataSource)ctrl).GetSchema();
                    }
                }
                if (dt.Columns != null && dt.Columns.Count > 0)
                {
                    List<string> dataKeyNames = new List<string>();
                    string colName = "";
                    int i = dt.Columns.Count;
                    DataColumn[] PrimDc = dt.PrimaryKey;
                    int m = PrimDc.Length;
                    for (int j = 0; j < i; j++)
                    {
                        colName = dt.Columns[j].ColumnName;
                        BoundField field = new BoundField();
                        field.HeaderText = colName;
                        field.DataField = colName;
                        field.SortExpression = colName;
                        for (int n = 0; n < m; n++)
                        {
                            if (dt.Columns[j] == PrimDc[n])
                            {
                                dataKeyNames.Add(colName);
                            }
                        }
                        grid.Columns.Add(field);
                    }
                    int x = dataKeyNames.Count;
                    grid.DataKeyNames = new string[x];
                    for (int y = 0; y < x; y++)
                    {
                        grid.DataKeyNames[y] = dataKeyNames[y];
                    }
                }
            }
        }

        protected override void OnSchemaRefreshed()
        {
            WebGridView wGridView = (WebGridView)this.Component;
            if (wGridView.WizardDesignMode)
                return;
            if (!wGridView.AutoGenerateEditButton && !wGridView.AutoGenerateDeleteButton && !wGridView.AutoGenerateSelectButton)
            {
                CommandField commandField = null;
                DataControlFieldCollection fields = wGridView.Columns;
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
                    commandField.SelectImageUrl = "~/Image/UIPics/Select.gif";
                    commandField.UpdateImageUrl = "~/Image/UIPics/OK.gif";
                    commandField.CancelImageUrl = "~/Image/UIPics/Cancel.gif";
                    commandField.ShowEditButton = true;
                    commandField.ShowDeleteButton = true;
                    commandField.ShowSelectButton = true;
                }
                if (!fields.Contains(commandField))
                {
                    fields.Insert(0, commandField);
                    fields.SetDirty();            // 这里在vs2008里有问题
                }
            }
            else
            {
                base.OnSchemaRefreshed();
            }
            if (wGridView.Site != null)
            {
                object obj = wGridView.GetObjByID(wGridView.DataSourceID);
                if (obj is WebDataSource)
                {
                    WebDataSource wds = (WebDataSource)obj;

                    DataSet Dset = DBUtils.GetDataDictionary(wds, true);
                    if (Dset != null && Dset.Tables.Count > 0)
                    {
                        foreach (DataControlField field in wGridView.Columns)
                        {
                            if (field is BoundField)
                            {
                                int i = Dset.Tables[0].Rows.Count;
                                for (int j = 0; j < i; j++)
                                {
                                    if (Dset.Tables[0].Rows[j]["FIELD_NAME"].ToString().ToLower() == ((BoundField)field).DataField.ToLower())
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
#if VS90
                    else if (!string.IsNullOrEmpty(wds.LinqDataSetID)) 
                    {
                        // 先不处理DD
                    }
#endif
                }
            }
        }

        private string GetRemoteName(WebGridView gridView)
        {
            string remoteName = "";
            XmlDocument xmlDoc = new XmlDocument();
            string aspxName = EditionDifference.ActiveDocumentFullName();
            string resourceName = aspxName + @".vi-VN.resx";
            ResXResourceReader reader = new ResXResourceReader(resourceName);
            IDictionaryEnumerator enumerator = reader.GetEnumerator();

            WebDataSource obj = (WebDataSource)gridView.GetObjByID(gridView.DataSourceID);
            string key = string.Empty;
            if (!string.IsNullOrEmpty(obj.WebDataSetID))
            {
                key = "WebDataSets";
            }
#if VS90
            if (!string.IsNullOrEmpty(obj.LinqDataSetID))
            {
                key = "LinqDataSets";
            }
#endif
            while (enumerator.MoveNext())
            {


                if (enumerator.Key.ToString() == key)
                {
                    string sXml = (string)enumerator.Value;
                    xmlDoc.LoadXml(sXml);
                    break;
                }
            }
            if (xmlDoc != null)
            {
                XmlNode nDataSets = null;
                XmlNode nDataSet = null;
                string dataSetID = string.Empty;
                if (!string.IsNullOrEmpty(obj.WebDataSetID))
                {
                    nDataSets = xmlDoc.SelectSingleNode("WebDataSets");
                    if (nDataSets == null) return string.Empty;
                    dataSetID = obj.WebDataSetID;
                    nDataSet = nDataSets.SelectSingleNode("WebDataSet[@Name='" + dataSetID + "']");
                }
#if VS90
                else if (!string.IsNullOrEmpty(obj.LinqDataSetID))
                {
                    nDataSets = xmlDoc.SelectSingleNode("LinqDataSets");
                    if (nDataSets == null) return string.Empty;
                    dataSetID = obj.LinqDataSetID;
                    nDataSet = nDataSets.SelectSingleNode("LinqDataSet[@Name='" + dataSetID + "']");
                }
#endif

                if (nDataSet != null)
                {
                    XmlNode nRemoteName = nDataSet.SelectSingleNode("RemoteName");
                    if (nRemoteName != null)
                        remoteName = nRemoteName.InnerText;
                }
            }
            return remoteName;
        }

        //private DesignerActionListCollection actionLists;

        /*public override DesignerActionListCollection ActionLists
        {
            get
            {
                actionLists = base.ActionLists;
                actionLists.Add(new WebGridViewActionList(this.Component));
                return actionLists;
            }
        }*/
    }

    /*public class WebGridViewActionList : DesignerActionList
    {
        private WebGridView gridview;

        public WebGridViewActionList(IComponent component)
            : base(component)
        {
            this.gridview = component as WebGridView;
        }

        public bool CreateInnerNavigator
        {
            get
            {
                return gridview.CreateInnerNavigator;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(WebGridView))["CreateInnerNavigator"].SetValue(gridview, value);
            }
        }

        public WebNavigator.CtrlType InnerNavigatorShowStyle
        {
            get
            {
                return gridview.InnerNavigatorShowStyle;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(WebGridView))["InnerNavigatorShowStyle"].SetValue(gridview, value);
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        public GridViewInnerNavigatorCollection NavControls
        {
            get
            {
                return gridview.NavControls;
            }
        }
    }*/
    #endregion

    #region GridViewInnerNavigatorEditor class
    internal class GridViewInnerNavigatorEditor : CollectionEditor
    {
        public GridViewInnerNavigatorEditor(Type type)
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
            if (((GridViewInnerNavigatorCollection)form.EditValue).Count == 0)
            {
                form.EditValue = InitControls((GridViewInnerNavigatorCollection)form.EditValue);
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

        private GridViewInnerNavigatorCollection InitControls(GridViewInnerNavigatorCollection Ctrls)
        {
            #region add default controls
            // Add Add Control
            ControlItem AddItem = new ControlItem
                ("Add", "add", WebNavigator.CtrlType.Image, "../image/uipics/add.gif", "../image/uipics/add2.gif", "../image/uipics/add3.gif", 26, true);
            Ctrls.Add(AddItem);
            // Add Query Control
            ControlItem QueryItem = new ControlItem
                ("Query", "query", WebNavigator.CtrlType.Image, "../image/uipics/query.gif", "../image/uipics/query2.gif", "../image/uipics/query3.gif", 26, true);
            Ctrls.Add(QueryItem);
            // Add Apply Control
            ControlItem ApplyItem = new ControlItem
                ("Apply", "apply", WebNavigator.CtrlType.Image, "../image/uipics/apply.gif", "../image/uipics/apply2.gif", "../image/uipics/apply3.gif", 26, true);
            Ctrls.Add(ApplyItem);
            // Add Abort Control
            ControlItem AbortItem = new ControlItem
                ("Abort", "abort", WebNavigator.CtrlType.Image, "../image/uipics/abort.gif", "../image/uipics/abort2.gif", "../image/uipics/abort3.gif", 26, true);
            Ctrls.Add(AbortItem);
            // Add OK Control
            ControlItem OKItem = new ControlItem
                ("OK", "Insert", WebNavigator.CtrlType.Image, "../image/uipics/ok.gif", "../image/uipics/ok2.gif", "../image/uipics/ok3.gif", 26, true);
            Ctrls.Add(OKItem);
            // Add Cancel Control
            ControlItem CancelItem = new ControlItem
                ("Cancel", "cancel", WebNavigator.CtrlType.Image, "../image/uipics/cancel.gif", "../image/uipics/cancel2.gif", "../image/uipics/cancel3.gif", 26, true);
            Ctrls.Add(CancelItem);
            #endregion
            return Ctrls;
        }
    }
    #endregion

    public interface IModalPanel
    {
        string TriggerUpdatePanel { get; set; }
        string DataContainer { get; set; }

        //string ViewCommandName { get; set; }
        //string EditCommandName { get; set; }
        //string InsertCommandName { get; set; }

        void Open(WebGridView.OpenEditMode mode);
        void Open(WebGridView.OpenEditMode mode, GridViewCommandEventArgs args);

        void Submit();

        void Close();
    }
}