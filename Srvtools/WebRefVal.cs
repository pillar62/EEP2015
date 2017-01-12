/*由于UpdatePanel中无法使用Response.Write，如果需要在UpdatePanel中设置WebRefVal的PostBackButtonClick属性为True的话，则给Srvtools添加Web.Extensions引用(安装asp.net ajax)，并设置WebRefVal的UpdatePanelID属性到相应的UpdatePanel*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI.Design.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Resources;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;

namespace Srvtools
{
    public abstract class WebRefValBase : WebControl, INamingContainer
    {
        public abstract string BindingValue { get; set; }
        public abstract string DataSourceID { get; set; }
        public abstract string DataValueField { get; set; }
        public abstract string DataTextField { get; set; }

        [Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ResxDataSet
        {
            get
            {
                object obj = this.ViewState["ResxDataSet"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["ResxDataSet"] = value;
            }
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
                if (this.Page != null)
                {
                    if (this.Page.Form != null)
                        return GetAllCtrls(ObjID, this.Page.Form);
                    else
                        return GetAllCtrls(ObjID, this.Page);
                }
                else
                {
                    return GetAllCtrls(ObjID, this.NamingContainer);
                }
            }
        }
        public string GetDataSetID()
        {
            string value = "";
            if (this.ResxDataSet != null && this.ResxDataSet != "")
                value = this.ResxDataSet;
            else
            {
                object wds = this.GetObjByID(this.DataSourceID);
                if (wds is WebDataSource && ((WebDataSource)wds).WebDataSetID != null && ((WebDataSource)wds).WebDataSetID != "")
                {
                    value = ((WebDataSource)wds).WebDataSetID;
                }
            }
            return HttpUtility.UrlEncode(value);
        }
    }

    [ToolboxData("<{0}:WebRefVal runat=server></{0}:WebRefVal>")]
    [ToolboxBitmap(typeof(WebRefVal), "Resources.WebRefVal.ico")]
    [ValidationPropertyAttribute("BindingValue")]
    public class WebRefVal : WebRefValBase, ICallbackEventHandler, IPostBackEventHandler, INamingContainer, IGetValues
    {
        public WebRefVal()
        {
            _WhereItem = new WebWhereItemCollection(this, typeof(WebWhereItem));
            _ColumnMatch = new WebColumnMatchCollection(this, typeof(WebColumnMatch));
            _Columns = new ColumnCollection(this, typeof(WebRefColumn));
        }

        private TextBox txtValue = new TextBox();
        private TextBox txtText = new TextBox();
        private TextBox txtShow = new TextBox();
        private DropDownList ddlValue = new DropDownList();

        [Browsable(false)]
        public string BindingText
        {
            get
            {
                return txtText.Text;
            }
            set
            {
                txtText.Text = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        public bool AutoUpperCase
        {
            get
            {
                object obj = this.ViewState["AutoUpperCase"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["AutoUpperCase"] = value;
            }
        }

        string _whereItemControl = "";
        [Category("Infolight")]
        [DefaultValue("")]
        public string WhereItemControl
        {
            get { return _whereItemControl; }
            set { _whereItemControl = value; }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            txtValue = new TextBox();
            txtValue.ID = "ValueInput";
            txtValue.TextChanged += new EventHandler(txtValue_TextChanged);

            txtText = new TextBox();
            txtText.ID = "TextInput";

            txtShow = new TextBox();
            txtShow.ID = "InnerTextBox";

            ddlValue = new DropDownList();
            ddlValue.ID = "ValueSelect";
            if (!this.ReadOnly)
            {
                ddlValue.AppendDataBoundItems = true;
                ddlValue.DataSourceID = this.DataSourceID;
                ddlValue.DataTextField = this.DataTextField;
                ddlValue.DataValueField = this.DataValueField;
            }

            this.Controls.Add(txtValue);
            this.Controls.Add(txtText);
            this.Controls.Add(txtShow);
            this.Controls.Add(ddlValue);
        }

        void txtValue_TextChanged(object sender, EventArgs e)
        {
            this.BindingValue = txtValue.Text;
        }

        public string GetValue(string value)
        {
            return CliUtils.GetValue(value, this.Page).ToString();
        }

        private WebWhereItemCollection _WhereItem;
        [Category("Infolight"),
        Description("Specifies the columns in where part to get data")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        TypeConverter(typeof(CollectionConverter))]
        public WebWhereItemCollection WhereItem
        {
            get
            {
                return _WhereItem;
            }
        }

        private WebColumnMatchCollection _ColumnMatch;
        [Category("Infolight"),
        Description("Specifies the columns in which data can be copied from source table to destination table")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        TypeConverter(typeof(CollectionConverter))]
        public WebColumnMatchCollection ColumnMatch
        {
            get
            {
                return _ColumnMatch;
            }
        }

        private ColumnCollection _Columns;
        [Category("Infolight"),
        Description("Specifies the columns to display on the page")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        TypeConverter(typeof(CollectionConverter))]
        public ColumnCollection Columns
        {
            get
            {
                return _Columns;
            }
        }

        [Category("Infolight"),
        Description("Caption of the InnerButton")]
        public bool ReadOnly
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
            }
        }

        private string _ButtonCaption;
        [Category("Infolight"),
        Description("Caption of the InnerButton")]
        public string ButtonCaption
        {
            get
            {
                return _ButtonCaption;
            }
            set
            {
                _ButtonCaption = value;
            }
        }

        [Category("Infolight"),
        Description("Indicates whether the InnerButton should use image for it's appearance")]
        public bool UseButtonImage
        {
            get
            {
                if (ViewState["UseButtonImage"] != null)
                {
                    return (bool)ViewState["UseButtonImage"];
                }
                return true;
            }
            set
            {
                ViewState["UseButtonImage"] = value;
            }
        }

        [Category("Infolight"),
        Description("ImageUrl of the InnerButton")]
        [EditorAttribute(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        public string ButtonImageUrl
        {
            get
            {
                if (ViewState["ButtonImageUrl"] != null && ViewState["ButtonImageUrl"].ToString() != "")
                {
                    return (string)ViewState["ButtonImageUrl"];
                }
                return "../Image/refval/RefVal.gif";
            }
            set
            {
                string strUrl = value;
                if (value.StartsWith("~"))
                {
                    strUrl = strUrl.Substring(1);
                    strUrl = ".." + strUrl;
                }
                ViewState["ButtonImageUrl"] = strUrl;
            }
        }

        [Category("Infolight"),
        DefaultValue(false),
        Description("Ignore up case and low case")]
        public bool IgnoreCase
        {
            get
            {
                object obj = this.ViewState["IgnoreCase"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["IgnoreCase"] = value;
            }
        }

        [Category("Infolight"),
        Description("Indicates which data field has bound to the control"),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string DataBindingField
        {
            get
            {
                object obj = this.ViewState["DataBindingField"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["DataBindingField"] = value;
            }
        }

        [Category("Infolight"),
        Description("Specifies the borderstyle of the control")]
        [Browsable(true)]
        public override BorderStyle BorderStyle
        {
            get
            {
                return base.BorderStyle;
            }
            set
            {
                base.BorderStyle = value;
            }
        }

        private string _Caption;
        [Category("Infolight"),
        Description("The caption of the page of WebRefVal")]
        public string Caption
        {
            get
            {
                return _Caption;
            }
            set
            {
                _Caption = value;
            }
        }

        [Category("Infolight"),
        Description("The datasource of WebRefVal")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public override string DataSourceID
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
            }
        }

        [Category("Infolight"),
        Description("The display member of WebRefVa")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public override string DataTextField
        {
            get
            {
                object obj = this.ViewState["DataTextField"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["DataTextField"] = value;
            }
        }

        [Category("Infolight"),
        Description("The value member of WebRefVal")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public override string DataValueField
        {
            get
            {
                object obj = this.ViewState["DataValueField"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["DataValueField"] = value;
            }
        }

        private void DoWhere(WebDataSource ds)
        {
            if (this.WhereItem.Count > 0)
            {
                string dbAlias = GetDBAlias();
                string commandText = GetCommandText();
                string whereitem = "";
                if (dbAlias != "" && commandText != "")
                {
                    whereitem = GetWhereItem(true, false);
                    string comand = ds.SelectCommand;
                    ds.SelectCommand = CliUtils.InsertWhere(ds.SelectCommand, whereitem);
                    ds.CommandTable = ds.GetCommandTable();
                    ds.SelectCommand = comand;
                }
                else
                {
                    whereitem = GetWhereItem(false, false);
                    ds.SetWhere(whereitem);
                }
            }
        }

        public override void DataBind()
        {
            if (!this.DesignMode)
            {
                WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                DoWhere(ds);
                try
                {
                    string dbAlias = GetDBAlias(), commandText = GetCommandText();
                    if ((dbAlias == null || dbAlias.Length == 0 || commandText == null || commandText.Length == 0)
                        && ds.WebDataSetID != null && ds.WebDataSetID.Length != 0)
                    {
                        object dataItem = null;
                        object obj = this.NamingContainer;
                        if (obj is WebDetailsView)
                        {
                            WebDetailsView detView = (WebDetailsView)obj;
                            dataItem = detView.DataItem;
                        }
                        else if (obj is WebFormView)
                        {
                            WebFormView formView = (WebFormView)obj;
                            dataItem = formView.DataItem;
                        }
                        else if (obj is GridViewRow)
                        {
                            GridViewRow gdViewRow = (GridViewRow)obj;
                            dataItem = gdViewRow.DataItem;
                        }

                        if (dataItem != null && dataItem is DataRowView)
                        {
                            DataRowView row = (DataRowView)dataItem;
                            object o = row[this.DataBindingField];
                            RefValByWhere(o);
                        }
                    }
                    base.DataBind();
                }
                catch
                {
                    ListItem item = new ListItem(null, null);
                    if (!this.ddlValue.Items.Contains(item))
                        this.ddlValue.Items.Insert(0, item);
                }
            }
            else
            {
                base.DataBind();
            }
        }

        public void RefValByWhere(object Value)
        {
            string sCurProject = CliUtils.fCurrentProject, strModuleName = "", strSourceTab = "", tabName = "", strSql = "";
            WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
            DataTable table = ds.InnerDataSet.Tables[ds.DataMember];
            int i = table.Rows.Count;
            for (int j = 0; j < i; j++)
            {
                if (Value.ToString() == ds.InnerDataSet.Tables[ds.DataMember].Rows[j][this.DataValueField].ToString())
                {
                    return;
                }
            }
            strModuleName = ds.RemoteName;
            strModuleName = strModuleName.Substring(0, strModuleName.IndexOf('.'));
            strSourceTab = ds.DataMember;
            //modified by lily 2007/6/8 此處應該取From後面的tablename，而不考慮別名
            tabName = CliUtils.GetTableName(strModuleName, strSourceTab, sCurProject, "", true);
            string type = table.Columns[this.DataValueField].DataType.ToString().ToLower();
            if (type == "system.uint" || type == "system.uint16" || type == "system.uint32" || type == "system.uint64"
             || type == "system.int" || type == "system.int16" || type == "system.int32" || type == "system.int64"
             || type == "system.single" || type == "system.float" || type == "system.double" || type == "system.decimal")
            {
                strSql = "select * from " + tabName + " where " + this.DataValueField + "=" + Value;
            }
            else
            {
                strSql = "select * from " + tabName + " where " + this.DataValueField + "= '" + Value + "'";
            }
            DataSet dset = CliUtils.ExecuteSql(strModuleName, strSourceTab, strSql, true, sCurProject);
            if (dset != null && dset.Tables[0].Rows.Count > 0)
            {
                ds.InnerDataSet.Tables[strSourceTab].Merge(dset.Tables[0]);
            }
        }

        [Bindable(true),
        Category("Infolight"),
        Description("Data binding property"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string BindingValue
        {
            get
            {
                object obj = this.ViewState["BindingValue"];
                if (obj != null)
                {
                    return ((string)obj).Trim();
                }
                return "";
            }
            set
            {
                this.ViewState["BindingValue"] = value;
                if (value.Length == 0)
                {
                    txtText.Text = string.Empty;
                }
            }
        }

        [Category("Infolight")]
        public bool PostBackButonClick
        {
            get
            {
                object obj = this.ViewState["PostBackButonClick"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["PostBackButonClick"] = value;
            }
        }


        public bool SessionMode
        {
            get
            {
                object obj = this.ViewState["SessionMode"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["SessionMode"] = value;
            }
        }


        [Category("Infolight"),
        Description("Check data"),
        DefaultValue(true)]
        public bool CheckData
        {
            get
            {
                object obj = this.ViewState["CheckData"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["CheckData"] = value;
            }
        }

        [Category("Infolight")]
        public bool AllowAddData
        {
            get
            {
                object obj = this.ViewState["AllowAddData"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["AllowAddData"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(450)]
        public int OpenRefHeight
        {
            get
            {
                object obj = this.ViewState["OpenRefHeight"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 450;
            }
            set
            {
                this.ViewState["OpenRefHeight"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(510)]
        public int OpenRefWidth
        {
            get
            {
                object obj = this.ViewState["OpenRefWidth"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 510;
            }
            set
            {
                this.ViewState["OpenRefWidth"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(200)]
        public int OpenRefLeft
        {
            get
            {
                object obj = this.ViewState["OpenRefLeft"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 200;
            }
            set
            {
                this.ViewState["OpenRefLeft"] = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(200)]
        public int OpenRefTop
        {
            get
            {
                object obj = this.ViewState["OpenRefTop"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 200;
            }
            set
            {
                this.ViewState["OpenRefTop"] = value;
            }
        }

        private bool _MultiLanguage;
        [Category("Infolight")]
        public bool MultiLanguage
        {
            get
            {
                return _MultiLanguage;
            }
            set
            {
                _MultiLanguage = value;
            }
        }

#if AjaxTools
        [Category("Infolight")]
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

        string _valueChangedJs = "";
        [Category("Infolight")]
        public string ValueChangedJs
        {
            get { return _valueChangedJs; }
            set { _valueChangedJs = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.Page.IsPostBack)
            {
                this.Page.Session.Remove(this.ClientID + "TempRefVal");
                this.Page.Session.Remove(this.ClientID + "TempRefDis");
            }
        }

        public void SynchronizeBindingValue()
        {
            if (this.txtValue.Text == this.txtShow.Text)
                this.BindingValue = txtShow.Text;
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            string resxFilePath = GetResxFilePath();
            string dbAlias = GetDBAlias();
            string commandText = GetCommandText();
            string whereitem = "";
            if (dbAlias != "" && commandText != "")
            {
                commandText = commandText = commandText.Replace("'", "\\'");
                whereitem = GetWhereItem(true, true);
            }
            else
            {
                whereitem = GetWhereItem(false, true);
            }
            string columnmatch = GetColumnMatch(true);
            string webDataSetID = GetDataSetID();
            string columns = GetColumns();
            string caption = (this.Caption == null) ? "" : HttpUtility.UrlEncode(this.Caption);

            string param = "MultiLan=" + this.MultiLanguage.ToString() + "&Resx=" + resxFilePath + "&DataSet=" + webDataSetID + "&ValueField=" + HttpUtility.UrlEncode(this.DataValueField) +
                "&TextField=" + HttpUtility.UrlEncode(this.DataTextField) + "&WhereItem=" + whereitem + "&DBAlias=" + dbAlias +
                "&CommandText=" + commandText + "&ColumnMatch=" + columnmatch + "&Columns=" + columns +
                "&SourceControl=" + HttpUtility.UrlEncode(this.ClientID) + "&Caption=" + caption + "&AllowAddData=" + this.AllowAddData.ToString() +
                "&PagePath=" + Page.Request.FilePath;
            if (dbAlias != "" && commandText != "")
            {
                param += "&PacketRecords=" + this.GetCommandPacketRecords().ToString();
            }
            string ScriptBlock = string.Empty;
            if (SessionMode)
            {
                string id = string.Format("RefVal{0}Session", this.UniqueID);
                this.Page.Session[id] = param;
                param = string.Format("RefValID={0}", id);
                ScriptBlock = "window.open('../InnerPages/frmRefVal.aspx?" + param + "', '', 'width=" + this.OpenRefWidth.ToString()
                        + ",height=" + this.OpenRefHeight.ToString() + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no,top=" + this.OpenRefTop + ",left=" + this.OpenRefLeft + "')";
            }
            else
            {
                param = QueryStringEncrypt.Encrypt(param);
                ScriptBlock = "window.open('../InnerPages/frmRefVal.aspx?" + param + "', '', 'width=" + this.OpenRefWidth.ToString()
                    + ",height=" + this.OpenRefHeight.ToString() + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no,top=" + this.OpenRefTop + ",left=" + this.OpenRefLeft + "')";
            }
#if AjaxTools
            object obj = this.GetObjByID(this.UpdatePanelID);
            if (obj != null && obj is UpdatePanel)
            {
                UpdatePanel panel = (UpdatePanel)obj;
                ScriptManager.RegisterStartupScript(panel, this.Page.GetType(), "ScriptBlock", ScriptBlock, true);
            }
            else
            {
#endif
                //modified by ccm for postback would change the font.
                this.Page.ClientScript.RegisterStartupScript(typeof(string), "", "<script>" + ScriptBlock + "</script>");
#if AjaxTools
            }
#endif
            WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
            DoWhere(ds);
            this.ddlValue.Items.Clear();
            this.ddlValue.DataBind();
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> dicArgs = serializer.DeserializeObject(eventArgument) as Dictionary<string, object>;
            string myvalue = dicArgs["value"].ToString();
            if (dicArgs.ContainsKey("whereitemcontrols"))
            {
                object[] whereitemcontrols = dicArgs["whereitemcontrols"] as object[];
                foreach (object whereitemcontrol in whereitemcontrols)
                {
                    Dictionary<string, object> wictrl = whereitemcontrol as Dictionary<string, object>;
                    if (wictrl.ContainsKey("controlid") && wictrl.ContainsKey("controlvalue"))
                    {
                        string controlid = wictrl["controlid"].ToString();
                        string controlvalue = wictrl["controlvalue"].ToString();
                        Control ctrl = this.Parent.FindControl(controlid);
                        if (ctrl is TextBox)
                        {
                            (ctrl as TextBox).Text = controlvalue;
                        }
                        else if (ctrl is WebRefVal)
                        {
                            (ctrl as WebRefVal).BindingValue = controlvalue;
                        }
                    }
                }
            }
            string dbAlias = GetDBAlias(), commandText = GetCommandText();
            if (string.IsNullOrEmpty(dbAlias) || string.IsNullOrEmpty(commandText))
            {
                string sCurProject = CliUtils.fCurrentProject, strModuleName = "", strSourceTab = "", tabName = "", strSql = "";
                WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                int i = ds.InnerDataSet.Tables[ds.DataMember].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (myvalue == ds.InnerDataSet.Tables[ds.DataMember].Rows[j][this.DataValueField].ToString())
                    {
                        callBackRetVal = ds.InnerDataSet.Tables[ds.DataMember].Rows[j][this.DataTextField].ToString() + ";";
                        genColumnMatchScript(ds.InnerDataSet.Tables[ds.DataMember].Rows[j]);
                        return;
                    }
                }
                strModuleName = ds.RemoteName;
                strModuleName = strModuleName.Substring(0, strModuleName.IndexOf('.'));
                strSourceTab = ds.DataMember;
                //modified by lily 2007/6/8 此處應該取From後面的tablename，而不考慮別名
                tabName = /*quote[0] + */CliUtils.GetTableName(strModuleName, strSourceTab, sCurProject, "", true)/* + quote[1]*/;
                string sqlcmd = CliUtils.GetSqlCommandText(strModuleName, strSourceTab, sCurProject);
                string type = ds.InnerDataSet.Tables[ds.DataMember].Columns[this.DataValueField].DataType.ToString().ToLower();
                if (type == "system.uint" || type == "system.uint16" || type == "system.uint32"
                || type == "system.uint64" || type == "system.int" || type == "system.int16"
                || type == "system.int32" || type == "system.int64" || type == "system.single"
                || type == "system.double" || type == "system.decimal")
                {
                    if (!string.IsNullOrEmpty(myvalue))
                        strSql = "select * from " + tabName + " where " + CliUtils.GetTableNameForColumn(sqlcmd, this.DataValueField) + " = " + myvalue;
                }
                else
                {
                    strSql = "select * from " + tabName + " where " + CliUtils.GetTableNameForColumn(sqlcmd, this.DataValueField) + " = '" + myvalue + "'";
                }
                strSql = CliUtils.InsertWhere(strSql, GetWhereItem(false, false));
                DataSet dset = CliUtils.ExecuteSql(strModuleName, strSourceTab, strSql, true, sCurProject);
                if (dset != null && dset.Tables[0].Rows.Count > 0)
                {
                    callBackRetVal = dset.Tables[0].Rows[0][this.DataTextField].ToString() + ";";
                    genColumnMatchScript(dset.Tables[0].Rows[0]);
                }
                this.Page.Session[this.ClientID + "TempRefVal"] = myvalue;
                this.Page.Session[this.ClientID + "TempRefDis"] = (callBackRetVal.IndexOf(';') == -1) ? "" : callBackRetVal.Substring(0, callBackRetVal.IndexOf(';'));
            }
            else
            {
                string sCurProject = CliUtils.fCurrentProject;
                WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                ds.CommandTable.PrimaryKey = new DataColumn[] { ds.CommandTable.Columns[this.DataValueField] };
                DataRowCollection rows = ds.CommandTable.Rows;
                Type valueType = ds.CommandTable.Columns[this.DataValueField].DataType;
                DataRow row = findRow(rows, valueType, myvalue);
                if (row != null)
                {
                    callBackRetVal = ";";
                    genColumnMatchScript(row);
                }
                else
                {
                    string strSql = "";
                    string type = valueType.ToString().ToLower();
                    if (type == "system.uint" || type == "system.uint16" || type == "system.uint32"
                    || type == "system.uint64" || type == "system.int" || type == "system.int16"
                    || type == "system.int32" || type == "system.int64" || type == "system.single"
                    || type == "system.double" || type == "system.decimal")
                    {
                        if (!string.IsNullOrEmpty(myvalue))
                            strSql = CliUtils.InsertWhere(ds.SelectCommand, string.Format("{0}={1}", CliUtils.GetTableNameForColumn(ds.SelectCommand, this.DataValueField), myvalue));
                    }
                    else
                    {
                        strSql = CliUtils.InsertWhere(ds.SelectCommand, string.Format("{0}='{1}'", CliUtils.GetTableNameForColumn(ds.SelectCommand, this.DataValueField), myvalue));
                    }
                    string whereitem = GetWhereItem(true, false);
                    strSql = CliUtils.InsertWhere(strSql, whereitem);

                    DataSet dset = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", strSql, true, sCurProject);
                    if (dset != null && dset.Tables[0].Rows.Count > 0)
                    {
                        callBackRetVal = dset.Tables[0].Rows[0][this.DataTextField].ToString() + ";";
                        genColumnMatchScript(dset.Tables[0].Rows[0]);
                    }
                }
            }
        }

        private void genColumnMatchScript(DataRow row)
        {
            string strMatch = this.GetColumnMatch(false);
            string[] matchs = strMatch.Split('/');
            foreach (string match in matchs)
            {
                string[] matchCtrls = match.Split(',');
                if (matchCtrls.Length == 3)
                {
                    string ctrlId = this.NamingContainer.FindControl(matchCtrls[0]).ClientID;

                    Control ctrl = this.NamingContainer.FindControl(matchCtrls[0]);
                    string value = (matchCtrls[1].Length > 0) ? row[matchCtrls[1]].ToString() : value = matchCtrls[2];

                    if (ctrl is WebRefVal)
                    {
                        callBackRetVal += string.Format("document.getElementById('{0}_ValueInput').value='{1}';", ctrlId, value)
                             + string.Format("document.getElementById('{0}_ValueSelect').value='{1}';", ctrlId, value)
                             + string.Format("document.getElementById('{0}_InnerTextBox').focus();", ctrlId);
                    }
                    else
                    {
                        callBackRetVal += "document.getElementById('" + ctrlId + "').value='" + value.Replace("\\", "\\\\").Replace("\'", "\\'").Replace("\"", "\\\"") + "';";
                        //string valueTextBoxID = string.Format("{0}_ValueInput", ctrlId);
                        //string valueDropdownList = string.Format("{0}_ValueSelect", ctrlId);
                        //string innerTextBoxID = string.Format("{0}_InnerTextBox", ctrlId);

                    }
                }
            }
        }

        private DataRow findRow(DataRowCollection rows, Type type, string keyvalue)
        {
            DataRow row = null;
            if (type != typeof(Guid))
            {
                row = rows.Find(Convert.ChangeType(keyvalue, type));
            }
            else
            {
                row = rows.Find(new Guid(keyvalue));
            }
            return row;
        }

        public string GetCallbackResult()
        {
            return callBackRetVal;
        }

        private string callBackRetVal = "";

        public string GetDisplayByValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            WebDataSource wds = (WebDataSource)this.GetObjByID(this.DataSourceID);
            string dbAlias = GetDBAlias(), commandText = GetCommandText();
            string sCurProject = CliUtils.fCurrentProject;
            string strSql = "";
            if (string.IsNullOrEmpty(dbAlias) || string.IsNullOrEmpty(commandText))
            {
                foreach (DataRow row in wds.InnerDataSet.Tables[wds.DataMember].Rows)
                {
                    if (value == row[this.DataValueField].ToString())
                    {
                        return row[this.DataTextField].ToString();
                    }
                }
                value = value.Replace("'", "''");
                string strModuleName = wds.RemoteName.Substring(0, wds.RemoteName.IndexOf('.'));
                string strSourceTab = wds.DataMember;
                string tabName = CliUtils.GetTableName(strModuleName, strSourceTab, sCurProject, "", true);
                string sqlcmd = CliUtils.GetSqlCommandText(strModuleName, strSourceTab, sCurProject);
                string type = wds.InnerDataSet.Tables[strSourceTab].Columns[this.DataValueField].DataType.ToString().ToLower();
                if (type == "system.uint" || type == "system.uint16" || type == "system.uint32"
                || type == "system.uint64" || type == "system.int" || type == "system.int16"
                || type == "system.int32" || type == "system.int64" || type == "system.single"
                || type == "system.double" || type == "system.decimal")
                {
                    if (!string.IsNullOrEmpty(value))
                        strSql = "select * from " + tabName + " where " + CliUtils.GetTableNameForColumn(sqlcmd, this.DataValueField) + " = " + value;
                }
                else
                {
                    strSql = "select * from " + tabName + " where " + CliUtils.GetTableNameForColumn(sqlcmd, this.DataValueField) + " = '" + value + "'";
                }
                DataSet dset = CliUtils.ExecuteSql(strModuleName, strSourceTab, strSql, true, sCurProject);
                if (dset != null && dset.Tables[0].Rows.Count > 0)
                {
                    return dset.Tables[0].Rows[0][this.DataTextField].ToString();
                }
            }
            else
            {
                foreach (DataRow row in wds.CommandTable.Rows)
                {
                    if (value == row[this.DataValueField].ToString())
                    {
                        return row[this.DataTextField].ToString();
                    }
                }
                string type = wds.CommandTable.Columns[this.DataValueField].DataType.ToString().ToLower();
                if (type == "system.uint" || type == "system.uint16" || type == "system.uint32"
                || type == "system.uint64" || type == "system.int" || type == "system.int16"
                || type == "system.int32" || type == "system.int64" || type == "system.single"
                || type == "system.double" || type == "system.decimal")
                {
                    if (!string.IsNullOrEmpty(value))
                        strSql = CliUtils.InsertWhere(wds.SelectCommand, string.Format("{0}={1}", CliUtils.GetTableNameForColumn(wds.SelectCommand, this.DataValueField), value));
                }
                else
                {
                    strSql = CliUtils.InsertWhere(wds.SelectCommand, string.Format("{0}='{1}'", CliUtils.GetTableNameForColumn(wds.SelectCommand, this.DataValueField), value));
                }
                DataSet dset = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", strSql, true, sCurProject);
                if (dset != null && dset.Tables[0].Rows.Count > 0)
                {
                    return dset.Tables[0].Rows[0][this.DataTextField].ToString();
                }
            }
            return null;
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Page != null)
            {
#if AjaxTools
                object obj = this.GetObjByID(this.UpdatePanelID);
                if (obj != null && obj is UpdatePanel)
                {
                    UpdatePanel panel = (UpdatePanel)obj;
                    ScriptManager.RegisterClientScriptResource(panel, this.GetType(), "Srvtools.WebRefVal.js");

                }
                else
                {
#endif
                    Page.ClientScript.RegisterClientScriptResource(this.GetType(), "Srvtools.WebRefVal.js");
#if AjaxTools
                }
#endif
                if (this.TabIndex != 0)
                {
                    txtShow.TabIndex = this.TabIndex;
                    this.TabIndex = 0;
                }
                base.OnPreRender(e);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            ClientScriptManager csm = Page.ClientScript;
            if (this.CssClass != "") { writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass); }
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");

            writer.RenderBeginTag(HtmlTextWriterTag.Table); // <table>

            writer.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            #region Value TextBox
            if (Page.Site == null)
            {
                if (this.Page.Session[this.ClientID + "TempRefVal"] != null)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Page.Session[this.ClientID + "TempRefVal"].ToString());
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, this.BindingValue.Trim());
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:0px;visibility:hidden;display:none;");
                txtValue.RenderControl(writer);
            }
            #endregion
            writer.RenderEndTag();  // </td>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            #region Text TextBox
            if (Page.Site == null)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:0px;visibility:hidden;display:none;");
                txtText.RenderControl(writer);
            }
            #endregion
            writer.RenderEndTag();  // </td>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            #region InnerDropDownList
            bool itemExist = false;
            if (Page.Site == null)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:0px;visibility:hidden;display:none;");
                if (this.BindingValue.Trim() != "")
                {
                    for (int i = 0; i < ddlValue.Items.Count; i++)
                    {
                        if (ddlValue.Items[i].Value == this.BindingValue.Trim())
                        {
                            ddlValue.SelectedValue = this.BindingValue.Trim();
                            itemExist = true;
                            break;
                        }
                    }
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Value, this.BindingValue.Trim());
                ddlValue.RenderControl(writer);
            }
            #endregion
            writer.RenderEndTag();  // </td>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            #region InnerInput
            if (this.Page.Site == null)
            {
                if (!this.ReadOnly)
                {
                    if (itemExist)
                        writer.AddAttribute(HtmlTextWriterAttribute.Value, this.BindingValue.Trim() == "" ? "" : this.ddlValue.SelectedItem.Text);
                    else
                    {
                        if (txtText.Text != null && txtText.Text != "")
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Value, txtText.Text);
                        }
                        else
                        {
                            if (this.Page.Session[this.ClientID + "TempRefDis"] != null)
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Page.Session[this.ClientID + "TempRefDis"].ToString());
                            }
                            else
                            {
                                string dbv = GetDisplayByValue(this.BindingValue.Trim());
                                writer.AddAttribute(HtmlTextWriterAttribute.Value, (string.IsNullOrEmpty(dbv) ? this.BindingValue.Trim() : dbv));
                            }
                        }
                    }
                    string changeScript = string.Format("var ctrls={{ddl:'{0}',val:'{1}',txt:'{2}',show:'{3}'}};var queryConfig={{onAfterChange:{4}}};_refChangeField(ctrls,{5},{6},queryConfig);",
                                                        ddlValue.ClientID,
                                                        txtValue.ClientID,
                                                        txtText.ClientID,
                                                        txtShow.ClientID,
                        //GetCommandText(false),
                                                        string.IsNullOrEmpty(this.ValueChangedJs) ? "null" : this.ValueChangedJs,
                                                        this.AutoUpperCase.ToString().ToLower(),
                                                        this.CheckData.ToString().ToLower());
                    writer.AddAttribute("onchange", changeScript, true);
                    string dbAlias = GetDBAlias(), commandText = GetCommandText();

                    String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebRefVal", "CheckDataMessage", true);
                    string callbackScript = "_refReceiveServerData";
                    StringBuilder valArgs = new StringBuilder();
                    valArgs.Append("\"{value:'\"");
                    valArgs.AppendFormat("+{0}", this.AutoUpperCase ? txtShow.ClientID + ".value.toUpperCase()" : txtShow.ClientID + ".value");
                    valArgs.Append("+\"'\"");
                    if (!string.IsNullOrEmpty(this.WhereItemControl))
                    {
                        valArgs.Append("+\",whereitemcontrols:[\"");
                        string[] ctrls = this.WhereItemControl.Split(';');
                        for (int i = 0; i < ctrls.Length; i++)
                        {
                            valArgs.AppendFormat("+\"{{controlid:'{0}'\"", ctrls[i]);
                            valArgs.Append("+\",controlvalue:'\"");
                            Control ctrl = this.Parent.FindControl(ctrls[i]);
                            if (ctrl is TextBox)
                            {
                                valArgs.AppendFormat("+{0}", (ctrl as TextBox).ClientID + ".value");
                            }
                            else if (ctrl is WebRefVal)
                            {
                                valArgs.AppendFormat("+{0}", (ctrl as WebRefVal).ClientID + "_ValueInput.value");
                            }
                            if (i < ctrls.Length - 1)
                            {
                                valArgs.Append("+\"'},\"");
                            }
                            else
                            {
                                valArgs.Append("+\"'}\"");
                            }
                        }
                        valArgs.Append("+\"]\"");
                    }
                    valArgs.Append("+\"}\"");

                    //valArgs.Append("\"{value:'xxx',controlid:'txtRefType',controlvalue:'yyy'}\"");
                    //valArgs.Append("\"aaaaa\"");
                    //string valArg = this.AutoUpperCase ? txtShow.ClientID + ".value.toUpperCase()" : txtShow.ClientID + ".value";

                    string blurContext = string.Format("{{ddl:'{0}',val:'{1}',txt:'{2}',show:'{3}',ref:'{4}',autoUpperCase:{5},checkData:{6},msg:{7}}}",
                                                       ddlValue.ClientID,
                                                       txtValue.ClientID,
                                                       txtText.ClientID,
                                                       txtShow.ClientID,
                                                       this.ClientID,
                                                       this.AutoUpperCase.ToString().ToLower(),
                                                       this.CheckData.ToString().ToLower(),
                                                       string.Format(message, txtShow.ClientID));
                    string blurScript = "if(document.getElementById('" + txtShow.ClientID + "').value == ''){document.getElementById('" + txtValue.ClientID + "').value = ' ';document.getElementById('" + txtText.ClientID + "').value = '';}else{" + csm.GetCallbackEventReference(this, valArgs.ToString(), callbackScript, blurContext, true) + "}";
                    writer.AddAttribute("onblur", blurScript, true);
                    string focusScript = string.Format("var ctrls={{show:'{0}',val:'{1}'}};_refFocus(ctrls);",
                                                       this.txtShow.ClientID,
                                                       this.txtValue.ClientID);
                    writer.AddAttribute("onfocus", focusScript, true);
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, GetDisplayByValue(this.BindingValue.Trim()));
                }
                this.Page.Session.Remove(this.ClientID + "TempRefVal");
                this.Page.Session.Remove(this.ClientID + "TempRefDis");
            }
            int wid = Convert.ToInt16(this.Width.Value) - 25;
            if (this.ReadOnly)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "readonly");
                wid += 25;
            }
            if (wid > 0)
            {
                txtShow.Width = wid;
            }
            txtShow.CssClass = this.CssClass;
            txtShow.ForeColor = this.ForeColor;
            txtShow.BackColor = this.BackColor;
            txtShow.Font.CopyFrom(this.Font);
            txtShow.BorderStyle = this.BorderStyle;
            txtShow.RenderControl(writer);
            #endregion
            writer.RenderEndTag();  // </td>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            #region InnerButton
            if (Page.Site == null)
            {
                #region PreFormClick
                if (this.PostBackButonClick)
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, csm.GetPostBackEventReference(this, "ButtonClick"));
                else
                {
                    string resxFilePath = GetResxFilePath();
                    string dbAlias = GetDBAlias();
                    string commandText = GetCommandText();
                    string whereitem = "";
                    if (dbAlias != "" && commandText != "")
                    {
                        if (!SessionMode)
                        {
                            commandText = commandText.Replace("'", "\\'");
                        }
                        whereitem = GetWhereItem(true, true);
                    }
                    else
                    {
                        whereitem = GetWhereItem(false, true);
                    }
                    //whereitem = whereitem.Replace("'", "\\'");
                    string columnmatch = GetColumnMatch(true);
                    string webDataSetID = GetDataSetID();
                    string columns = GetColumns();
                    string caption = (this.Caption == null) ? "" : HttpUtility.UrlEncode(this.Caption);
                    StringBuilder paramBuilder = new StringBuilder();
                    paramBuilder.AppendFormat(
                        @"MultiLan={0}&Resx={1}&DataSet={2}&ValueField={3}&TextField={4}&WhereItem={5}&DBAlias={6}&CommandText={7}&ColumnMatch={8}&Columns={9}&SourceControl={10}&Caption={11}&AllowAddData={12}&PagePath={13}",
                        this.MultiLanguage.ToString(),
                        resxFilePath,
                        webDataSetID,
                        HttpUtility.UrlEncode(this.DataValueField),
                        HttpUtility.UrlEncode(this.DataTextField),
                        whereitem,
                        dbAlias,
                        commandText,
                        columnmatch,
                        columns,
                        HttpUtility.UrlEncode(this.ClientID),
                        caption,
                        this.AllowAddData.ToString(),
                        Page.Request.FilePath);

                    if (!string.IsNullOrEmpty(dbAlias) && !string.IsNullOrEmpty(commandText))
                    {
                        paramBuilder.AppendFormat("&PacketRecords={0}", this.GetCommandPacketRecords().ToString());
                    }

                    //use session instead of querystring
                    if (SessionMode)
                    {
                        string id = string.Format("RefVal{0}Session", this.UniqueID);
                        this.Page.Session[id] = paramBuilder.ToString();
                        string param = string.Format("RefValID={0}&TypeAhead='+typeahead+'", id);
                        string ScriptBlock =
                            string.Format(@"var typeahead=document.getElementById('{5}').value;window.open('../InnerPages/frmRefVal.aspx?{0}','','width={1},height={2},scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no,top={3},left={4}');return false;",
                            param, this.OpenRefWidth, this.OpenRefHeight, this.OpenRefTop, this.OpenRefLeft, this.txtShow.ClientID);

                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, ScriptBlock);
                    }
                    else
                    {
                        string param = QueryStringEncrypt.Encrypt(paramBuilder.ToString()) + "&TypeAhead='+typeahead+'";
                        string ScriptBlock =
                            string.Format(@"var typeahead=document.getElementById('{5}').value;window.open('../InnerPages/frmRefVal.aspx?{0}','','width={1},height={2},scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no,top={3},left={4}');return false;",
                            param, this.OpenRefWidth, this.OpenRefHeight, this.OpenRefTop, this.OpenRefLeft, this.txtShow.ClientID);

                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, ScriptBlock);
                    }
                }
                #endregion
            }
            string buttonName = this.ClientID + "_InnerButton";
            writer.AddAttribute(HtmlTextWriterAttribute.Id, buttonName);
            string btnSytle = "";
            if (!this.UseButtonImage)
            {
                if (this.ReadOnly)
                    btnSytle = "width:0px;visibility:hidden;display:none;";
                else
                    btnSytle = "width: 25px;";
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, this.ButtonCaption);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, ButtonImageUrl);
                if (this.ReadOnly)
                    btnSytle = "width:0px;visibility:hidden;display:none;";
                else
                    btnSytle = "border-right: thin outset;border-top: thin outset;border-left: thin outset; border-bottom: thin outset; background-color: buttonface";
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "image");
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Style, btnSytle);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            #endregion
            writer.RenderEndTag();  // </td>

            writer.RenderEndTag();  // </tr>

            writer.RenderEndTag();  // </table>
        }

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "databindingfield", true) == 0)//IgnoreCase
            {
                IDesignerHost host = (IDesignerHost)this.Site.GetService(typeof(IDesignerHost));
                ControlDesigner designer = (ControlDesigner)host.GetDesigner(this);
                if (designer.DataBindings["BindingValue"] != null)
                {
                    string content = designer.DataBindings["BindingValue"].Expression;
                    string[] contentPart = content.Split('"');
                    values.Add(contentPart[1]);
                }
            }
            else if (string.Compare(sKind, "datatextfield", true) == 0 || string.Compare(sKind, "datavaluefield", true) == 0)//IgnoreCase
            {
                object obj = this.GetObjByID(this.DataSourceID);
                if (obj is WebDataSource)
                {
                    WebDataSource ds = (WebDataSource)obj;
                    if (ds.SelectAlias != null && ds.SelectAlias.Length != 0 && ds.SelectCommand != null && ds.SelectCommand.Length != 0)
                    {
                        DataTable dt = ds.CommandTable;//ds.CommandTable;
                        foreach (DataColumn col in dt.Columns)
                        {
                            values.Add(col.ColumnName);
                        }
                    }
                    else
                    {
                        if (ds.DesignDataSet == null)
                        {
                            WebDataSet wds = WebDataSet.CreateWebDataSet(ds.WebDataSetID);
                            ds.DesignDataSet = wds.RealDataSet;
                        }
                        if (ds.DesignDataSet != null && ds.DesignDataSet.Tables.Contains(ds.DataMember))
                        {
                            foreach (DataColumn column in ds.DesignDataSet.Tables[ds.DataMember].Columns)
                            {
                                values.Add(column.ColumnName);
                            }
                        }
                    }
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

        List<Control> AllControls = new List<Control>();
        private void GetAllControls(ControlCollection Ctrls)
        {
            foreach (Control ctrl in Ctrls)
            {
                AllControls.Add(ctrl);
                GetAllControls(ctrl.Controls);
            }
        }

        public string GetDBAlias()
        {
            string value = "";
            object wds = this.GetObjByID(this.DataSourceID);
            if (wds is WebDataSource && ((WebDataSource)wds).SelectAlias != null && ((WebDataSource)wds).SelectAlias != "")
            {
                value = ((WebDataSource)wds).SelectAlias;
            }
            return HttpUtility.UrlEncode(value);
        }

        public int GetCommandPacketRecords()
        {
            int value = -1;
            object wds = this.GetObjByID(this.DataSourceID);
            if (wds is WebDataSource)
            {
                value = ((WebDataSource)wds).CommandPacketRecords;
            }
            return value;
        }

        public string GetCommandText(bool encodeUrl)
        {
            string value = "";
            object o = this.GetObjByID(this.DataSourceID);
            if (o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                if (!string.IsNullOrEmpty(wds.SelectCommand))
                {
                    if (encodeUrl)
                    {
                        value = HttpUtility.UrlEncode(wds.SelectCommand);
                    }
                    else
                    {
                        value = wds.SelectCommand;
                    }
                }
            }
            return value;
        }

        public string GetCommandText()
        {
            return GetCommandText(true);
        }

        public string GetWhereItem(bool UseCommand, bool transfer)
        {
            string whereitem = "";
            transfer = transfer && !SessionMode;
            WebDataSource wds = (WebDataSource)this.GetObjByID(this.DataSourceID);
            foreach (WebWhereItem item in this.WhereItem)
            {
                Type type;
                string tablename = string.Empty;
                string sqlcmd = string.Empty;
                if (!UseCommand)
                {
                    type = wds.InnerDataSet.Tables[wds.DataMember].Columns[item.FieldName].DataType;
                }
                else
                {
                    type = wds.CommandTable.Columns[item.FieldName].DataType;
                }
                string val = this.GetValue(item.Value);
                string transvalue;
                if (type == typeof(DateTime))
                {
                    val = CliUtils.FormatDateString(val);
                    transvalue = HttpUtility.UrlEncode(val).Replace("'", "\\'");
                }
                else
                {
                    transvalue = HttpUtility.UrlEncode(val).Replace("'", "\\'\\'");
                    val = val.Replace("'", "''");
                }

                if (item.Condition != "%" && item.Condition != "%%")
                {
                    if (type == typeof(uint) || type == typeof(UInt16) || type == typeof(UInt32)
                        || type == typeof(UInt64) || type == typeof(int) || type == typeof(Int16)
                        || type == typeof(Int32) || type == typeof(Int64) || type == typeof(Single)
                        || type == typeof(Double) || type == typeof(Decimal) || type == typeof(DateTime))
                    {
                        if (transfer)
                            whereitem += HttpUtility.UrlEncode(CliUtils.GetTableNameForColumn(wds.CommandText, item.FieldName)) + item.Condition + transvalue + " and ";
                        else
                            whereitem += CliUtils.GetTableNameForColumn(wds.CommandText, item.FieldName) + item.Condition + val + " and ";
                    }
                    else
                    {
                        if (transfer)
                            whereitem += HttpUtility.UrlEncode(CliUtils.GetTableNameForColumn(wds.CommandText, item.FieldName)) + item.Condition + "\\'" + transvalue + "\\' and ";
                        else
                            whereitem += CliUtils.GetTableNameForColumn(wds.CommandText, item.FieldName) + item.Condition + "'" + val + "' and ";
                    }
                }
                else
                {
                    if (item.Condition == "%")
                    {
                        if (transfer)
                            whereitem += HttpUtility.UrlEncode(CliUtils.GetTableNameForColumn(wds.CommandText, item.FieldName)) + " like \\'" + transvalue + "%\\' and ";
                        else
                            whereitem += CliUtils.GetTableNameForColumn(wds.CommandText, item.FieldName) + " like '" + val + "%' and ";

                    }
                    else if (item.Condition == "%%")
                    {
                        if (transfer)
                            whereitem += HttpUtility.UrlEncode(CliUtils.GetTableNameForColumn(wds.CommandText, item.FieldName)) + " like \\'%" + transvalue + "%\\' and ";
                        else
                            whereitem += CliUtils.GetTableNameForColumn(wds.CommandText, item.FieldName) + " like '%" + val + "%' and ";
                    }
                }
            }
            if (whereitem != "")
            {
                whereitem = whereitem.Substring(0, whereitem.LastIndexOf(" and "));
            }
            return whereitem;
        }

        public string GetColumnMatch(bool transfer)
        {
            string columnMatch = "";
            foreach (WebColumnMatch cm in this.ColumnMatch)
            {
                object value = CliUtils.GetValue(cm.SrcGetValue, this.Page);
                string strvalue = (value == null) ? string.Empty : value.ToString();
                if (transfer)
                    columnMatch += HttpUtility.UrlEncode(cm.DestControlID) + "%%%" + cm.SrcField + "%%%" + strvalue + ";";
                else
                    columnMatch += cm.DestControlID + "," + cm.SrcField + "," + strvalue + "/";
            }

            if (columnMatch != "")
            {
                if (transfer)
                {
                    columnMatch = columnMatch.Substring(0, columnMatch.LastIndexOf(';'));
                    columnMatch = HttpUtility.UrlEncode(columnMatch);
                }
                else
                {
                    columnMatch = columnMatch.Substring(0, columnMatch.LastIndexOf('/'));
                }
            }
            return columnMatch;
        }

        public string GetColumns()
        {
            string columns = "";
            foreach (WebRefColumn column in this.Columns)
            {
                columns += column.ColumnName + "," + column.HeadText + "," + column.Width.ToString() + "," + column.IsNvarChar.ToString() + ";";
            }
            if (columns != "")
            {
                columns = columns.Substring(0, columns.LastIndexOf(';'));
            }
            return HttpUtility.UrlEncode(columns);
        }

        public string GetResxFilePath()
        {
            string value = "";
            if (this.ResxFilePath != null && this.ResxFilePath != "") //2006-07-03，解传入的datasource取其页面无法得到webdataset信息
                value = this.ResxFilePath;
            else
                value = Page.Request.MapPath(Page.Request.Path) + @".vi-VN.resx";
            string[] str = value.Split('\\');
            value = "";
            int i = str.Length;
            for (int j = 0; j < i; j++)
            {
                if (j < i - 1)
                {
                    value += str[j] + "\\\\";
                }
                else
                {
                    value += str[j];
                }
            }
            return HttpUtility.UrlEncode(value);
        }

        [Browsable(false)]
        public string ResxFilePath
        {
            get
            {
                object obj = this.ViewState["ResxFilePath"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["ResxFilePath"] = value;
            }
        }
    }

    #region WhereItem
    public class WebWhereItemCollection : InfoOwnerCollection
    {
        public WebWhereItemCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebWhereItem))
        {
        }

        public new WebWhereItem this[int index]
        {
            get
            {
                return (WebWhereItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebWhereItem)
                    {
                        //原来的Collection设置为0
                        ((WebWhereItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebWhereItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class WebWhereItem : InfoOwnerCollectionItem, IGetValues
    {
        public WebWhereItem()
        {
        }

        public WebWhereItem(string fieldName, string condition, string value)
        {
            _FieldName = fieldName;
            _Condition = condition;
            _Value = value;
        }

        private string _FieldName;
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        private string _Condition;
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string Condition
        {
            get
            {
                return _Condition;
            }
            set
            {
                _Condition = value;
            }
        }

        private string _Value;
        [NotifyParentProperty(true)]
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        public override string ToString()
        {
            return _FieldName;
        }

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "fieldname", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebRefValBase)
                {
                    WebRefValBase refval = (WebRefValBase)this.Owner;
                    if (refval.DataSourceID != null && refval.DataSourceID != "" && refval.Page != null && refval.DataTextField != null &&
                        refval.DataTextField != "" && refval.DataValueField != null && refval.DataValueField != "")
                    {
                        WebDataSource ds = (WebDataSource)refval.GetObjByID(refval.DataSourceID);

                        if (ds.SelectAlias != null && ds.SelectAlias != "" && ds.SelectCommand != null && ds.SelectCommand != "")
                        {
                            foreach (DataColumn column in ds.CommandTable.Columns)
                            {
                                values.Add(column.ColumnName);
                            }
                        }
                        else
                        {
                            if (ds.DesignDataSet == null)
                            {
                                WebDataSet wds = WebDataSet.CreateWebDataSet(ds.WebDataSetID);
                                ds.DesignDataSet = wds.RealDataSet;
                            }
                            if (ds.DesignDataSet != null && ds.DesignDataSet.Tables.Contains(ds.DataMember))
                            {
                                foreach (DataColumn column in ds.DesignDataSet.Tables[ds.DataMember].Columns)
                                {
                                    values.Add(column.ColumnName);
                                }
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
            else if (string.Compare(sKind, "condition", true) == 0)
            {
                retList = new string[] { "<=", "<", "=", "!=", ">", ">=", "%", "%%" };
            }
            return retList;
        }
        #endregion
    }
    #endregion

    #region ColumnMatch
    public class WebColumnMatchCollection : InfoOwnerCollection
    {
        public WebColumnMatchCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebColumnMatch))
        {
        }

        public DataSet CacheDs = new DataSet();

        public new WebColumnMatch this[int index]
        {
            get
            {
                return (WebColumnMatch)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebColumnMatch)
                    {
                        //原来的Collection设置为0
                        ((WebColumnMatch)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebColumnMatch)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class WebColumnMatch : InfoOwnerCollectionItem, IGetValues
    {
        public WebColumnMatch()
        {
        }

        public WebColumnMatch(string srcField, string strGetValue, string destctrl)
        {
            _SrcField = srcField;
            _SrcGetValue = strGetValue;
            _DestControlID = destctrl;
        }

        private string _SrcField;
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string SrcField
        {
            get
            {
                return _SrcField;
            }
            set
            {
                _SrcField = value;
            }
        }

        private string _SrcGetValue;
        [NotifyParentProperty(true)]
        public string SrcGetValue
        {
            get
            {
                return _SrcGetValue;
            }
            set
            {
                _SrcGetValue = value;
            }
        }

        private string _DestControlID;
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string DestControlID
        {
            get
            {
                return _DestControlID;
            }
            set
            {
                _DestControlID = value;
            }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return _DestControlID; }
            set { _DestControlID = value; }
        }

        public override string ToString()
        {
            return _DestControlID;
        }

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is WebRefValBase)
            {
                WebRefValBase refval = (WebRefValBase)this.Owner;
                if (string.Compare(sKind, "srcfield", true) == 0)//IgnoreCase
                {
                    if (refval.DataSourceID != null && refval.DataSourceID != ""
                        && refval.DataTextField != null && refval.DataTextField != ""
                        && refval.DataValueField != null && refval.DataValueField != ""
                        && refval.Page != null)
                    {
                        if (refval.Page != null && refval.DataSourceID != null && refval.DataSourceID != "")
                        {
                            foreach (Control ctrl in refval.Page.Controls)
                            {
                                if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == refval.DataSourceID)
                                {
                                    WebDataSource ds = (WebDataSource)ctrl;
                                    if (ds.SelectAlias != null && ds.SelectAlias != "" && ds.SelectCommand != null && ds.SelectCommand != "")
                                    {
                                        DataTable table = ds.CommandTable;
                                        foreach (DataColumn column in table.Columns)
                                        {
                                            values.Add(column.ColumnName);
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        if (ds.DesignDataSet == null)
                                        {
                                            WebDataSet wds = WebDataSet.CreateWebDataSet(ds.WebDataSetID);
                                            ds.DesignDataSet = wds.RealDataSet;
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

                    //    foreach (Control ctrl in refval.Page.Controls)
                    //    {
                    //        if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == refval.DataSourceID)
                    //        {
                    //            WebDataSource ds = (WebDataSource)ctrl;
                    //            if (ds.DesignDataSet == null)
                    //            {
                    //                if (ds.SelectAlias != null && ds.SelectAlias != "" && ds.SelectCommand != null && ds.SelectCommand != "")
                    //                {
                    //                    foreach (DataColumn column in ds.CommandTable.Columns)
                    //                    {
                    //                        values.Add(column.ColumnName);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    DataSet cads = ((WebColumnMatchCollection)this.Collection).CacheDs;
                    //                    if (cads.Tables.Count == 0)
                    //                    {
                    //                        WebDataSet wds = CreateDataSet(ds.WebDataSetID);
                    //                        ((WebColumnMatchCollection)this.Collection).CacheDs = wds.RealDataSet;
                    //                        cads = wds.RealDataSet;
                    //                    }
                    //                    foreach (DataColumn column in ds.DesignDataSet.Tables[((WebDataSource)ctrl).DataMember].Columns)
                    //                    {
                    //                        values.Add(column.ColumnName);
                    //                    }
                    //                }
                    //            }
                    //            break;
                    //        }
                    //    }
                    //    if (values.Count > 0)
                    //    {
                    //        int i = values.Count;
                    //        retList = new string[i];
                    //        for (int j = 0; j < i; j++)
                    //        {
                    //            retList[j] = values[j];
                    //        }
                    //    }
                    //}
                }
                else if (string.Compare(sKind, "destcontrolid", true) == 0)//IgnoreCase
                {
                    string Temp = "";
                    bool findControl = false;
                    foreach (Control ctrl in refval.Page.Controls)
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
                                        findControl = HasRefValIn(detView, EditBuilder.Text, refval.ID);
                                        Temp = "EditItemTemplate";
                                        bInEditTemplate = findControl;
                                    }
                                    if (!bInEditTemplate)
                                    {
                                        findControl = HasRefValIn(detView, InsertBuilder.Text, refval.ID);
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
                                                values.AddRange(this.GetControlNames(detView, EditBuilder.Text, refval.ID));
                                            }
                                        }
                                        else if (Temp == "InsertItemTemplate")
                                        {
                                            TemplateBuilder InsertBuilder = (TemplateBuilder)((TemplateField)field).InsertItemTemplate;
                                            if (InsertBuilder != null)
                                            {
                                                values.AddRange(this.GetControlNames(detView, InsertBuilder.Text, refval.ID));
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
                                        findControl = HasRefValIn(gdView, EditBuilder.Text, refval.ID);
                                        Temp = "EditItemTemplate";
                                        bInEditTemplate = findControl;
                                    }
                                    if (!bInEditTemplate && FooterBuilder != null)
                                    {
                                        findControl = HasRefValIn(gdView, FooterBuilder.Text, refval.ID);
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
                                                values.AddRange(this.GetControlNames(gdView, EditBuilder.Text, refval.ID));
                                            }
                                        }
                                        else if (Temp == "FooterTemplate")
                                        {
                                            TemplateBuilder FooterBuilder = (TemplateBuilder)((TemplateField)field).FooterTemplate;
                                            if (FooterBuilder != null)
                                            {
                                                values.AddRange(this.GetControlNames(gdView, FooterBuilder.Text, refval.ID));
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
                                findControl = HasRefValIn(frmView, EditBuilder.Text, refval.ID);
                                Temp = "EditItemTemplate";
                                bInEditTemplate = findControl;
                            }
                            if (!bInEditTemplate)
                            {
                                findControl = HasRefValIn(frmView, InsertBuilder.Text, refval.ID);
                                Temp = "InsertItemTemplate";
                            }
                            if (findControl)
                            {
                                if (Temp == "EditItemTemplate")
                                {
                                    retList = GetControlNames(frmView, EditBuilder.Text, refval.ID);
                                }
                                else if (Temp == "InsertItemTemplate")
                                {
                                    retList = GetControlNames(frmView, InsertBuilder.Text, refval.ID);
                                }
                                Array.Sort<String>(retList);
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
                        Array.Sort<String>(retList);
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
                if (ctrls[j] is WebRefValBase && ctrls[j].ID == refID)
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
    #endregion

    #region Columns
    public class ColumnCollection : InfoOwnerCollection
    {
        public ColumnCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebRefColumn))
        {
        }

        public DataSet DsForDD = new DataSet();

        public new WebRefColumn this[int index]
        {
            get
            {
                return (WebRefColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebRefColumn)
                    {
                        //原来的Collection设置为0
                        ((WebRefColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebRefColumn)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class WebRefColumn : InfoOwnerCollectionItem, IGetValues
    {
        public WebRefColumn()
            : this("", "", 100)
        {
        }

        public WebRefColumn(string columnName, string headText, int width)
        {
            _ColumnName = columnName;
            _HeadText = headText;
            _Width = width;
        }

        private string _ColumnName;
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
            set
            {
                _ColumnName = value;
                if (this.Owner != null && ((WebRefValBase)this.Owner).Site.DesignMode && _ColumnName != null && _ColumnName != "")
                {
                    string header = GetHeaderText(_ColumnName);
                    if (header != "")
                    {
                        HeadText = header;
                    }
                    else
                    {
                        HeadText = _ColumnName;
                    }
                }
            }
        }

        private string _HeadText;
        [NotifyParentProperty(true)]
        public string HeadText
        {
            get
            {
                return _HeadText;
            }
            set
            {
                _HeadText = value;
            }
        }

        private int _Width;
        [NotifyParentProperty(true)]
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }

        private bool isNvarChar;

        public bool IsNvarChar
        {
            get { return isNvarChar; }
            set { isNvarChar = value; }
        }


        public override string ToString()
        {
            return _ColumnName;
        }

        private string GetHeaderText(string ColName)
        {
            string strHeaderText = "";
            DataSet ds = ((ColumnCollection)this.Collection).DsForDD;
            WebRefValBase refVal = (WebRefValBase)this.Owner;
            string strTableName = ((WebDataSource)refVal.GetObjByID(refVal.DataSourceID)).DataMember;
            if (ds.Tables.Count == 0)
            {
                WebDataSource wds = (WebDataSource)refVal.GetObjByID(refVal.DataSourceID);
                ((ColumnCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(wds, true);
                ds = ((ColumnCollection)this.Collection).DsForDD;
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (string.Compare(ds.Tables[0].Rows[j]["FIELD_NAME"].ToString(), ColName, true) == 0)//IgnoreCase
                    {
                        strHeaderText = ds.Tables[0].Rows[j]["CAPTION"].ToString();
                    }
                }
            }
            return strHeaderText;
        }

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is WebRefValBase)
            {
                if (string.Compare(sKind, "columnname", true) == 0)//IgnoreCase
                {
                    WebRefValBase refval = (WebRefValBase)this.Owner;
                    if (refval.Page != null && refval.DataSourceID != null && refval.DataSourceID != "")
                    {
                        foreach (Control ctrl in refval.Page.Controls)
                        {
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == refval.DataSourceID)
                            {
                                WebDataSource ds = (WebDataSource)ctrl;
                                if (ds.SelectAlias != null && ds.SelectAlias != "" && ds.SelectCommand != null && ds.SelectCommand != "")
                                {
                                    DataTable table = ds.CommandTable;
                                    foreach (DataColumn column in table.Columns)
                                    {
                                        values.Add(column.ColumnName);
                                    }
                                    break;
                                }
                                else
                                {
                                    if (ds.DesignDataSet == null)
                                    {
                                        WebDataSet wds = WebDataSet.CreateWebDataSet(ds.WebDataSetID);
                                        ds.DesignDataSet = wds.RealDataSet;
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
}
