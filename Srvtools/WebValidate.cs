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
using System.Globalization;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms.Design;
using System.Collections.Specialized;
using System.IO;
using System.Xml;
using System.Resources;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

namespace Srvtools
{
    public abstract class WebBaseLabel : Label, IBaseWebControl
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
            Control objContentPlaceHolder = null;
            if (this.Page.Form != null)
            {
                objContentPlaceHolder = this.Page.Form.FindControl("ContentPlaceHolder1");
            }
            if (objContentPlaceHolder != null)
            {
                return this.FindChildControl(strid, objContentPlaceHolder, type, ReturnControlType);
            }
            else
            {
                return this.FindChildControl(strid, this.Page, type, ReturnControlType);
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

    [ToolboxBitmap(typeof(WebValidate), "Resources.WebValidate.png")]
    [ToolboxData("<{0}:WebValidate runat=\"server\"></{0}:WebValidate>")]
    [ParseChildren(true, "Fields")]
    public class WebValidate : WebBaseLabel
    {
        public WebValidate()
        {
            this.ForeColor = System.Drawing.Color.Red;
            _Fields = new ValidateFieldsCollection(this, typeof(ValidateFieldItem));
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

        private ValidateFieldsCollection _Fields;
        [Category("Infolight"),
        Description("The columns which WebValidate is applied to")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        public ValidateFieldsCollection Fields
        {
            get { return _Fields; }
        }

        [Category("Infolight"),
        Description("Indicates whether WebValidate is enabled or disabled")]
        [DefaultValue(true)]
        public bool ValidateActive
        {
            get
            {
                object obj = this.ViewState["ValidateActive"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["ValidateActive"] = value;
            }
        }

        [Category("Infolight"),
        Description("Color of the validate message")]
        [DefaultValue(typeof(Color), "Red")]
        public Color ValidateColor
        {
            get
            {
                object obj = this.ViewState["ValidateColor"];
                if (obj != null)
                {
                    return (Color)obj;
                }
                return Color.Red;
            }
            set
            {
                this.ViewState["ValidateColor"] = value;
            }
        }

        [Category("Infolight"),
        Description("Character of validate")]
        [DefaultValue("")]
        public string ValidateChar
        {
            get
            {
                object obj = this.ViewState["ValidateChar"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["ValidateChar"] = value;
            }
        }

        [Category("Infolight"),
        Description("Indicate whether data need to check if it has exsit in dataset or database")]
        [DefaultValue(false)]
        public bool DuplicateCheck
        {
            get
            {
                object obj = this.ViewState["DuplicateCheck"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["DuplicateCheck"] = value;
            }
        }

        public enum DupCheckMode
        {
            ByLocal = 0,
            ByWhere = 1
        }

        [Category("Infolight"),
        Description("Mode of DuplicateCheck")]
        [DefaultValue(typeof(DupCheckMode), "ByLocal")]
        public DupCheckMode DuplicateCheckMode
        {
            get
            {
                object obj = this.ViewState["DuplicateCheckMode"];
                if (obj != null)
                {
                    return (DupCheckMode)obj;
                }
                return DupCheckMode.ByLocal;
            }
            set
            {
                this.ViewState["DuplicateCheckMode"] = value;
            }
        }

        public enum ValidateStyles
        {
            ShowLable = 0,
            ShowDialog = 1
        }

        [Category("Infolight"),
        Description("Style of validate")]
        [DefaultValue(typeof(ValidateStyles), "ShowLable")]
        public ValidateStyles ValidateStyle
        {
            get
            {
                object obj = this.ViewState["ValidateStyle"];
                if (obj != null)
                {
                    return (ValidateStyles)obj;
                }
                return ValidateStyles.ShowLable;
            }
            set
            {
                this.ViewState["ValidateStyle"] = value;
            }
        }

        private bool _MultiLanguage;
        [Category("Infolight")]
        [DefaultValue(false)]
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

        private List<Control> AllControls = new List<Control>();
        public void GetAllControls(Control ParentControl)
        {
            foreach (Control ctrl in ParentControl.Controls)
            { 
                AllControls.Add(ctrl);
                if (ctrl.Controls.Count > 0)
                {
                    GetAllControls(ctrl);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            string rscUrl = "../css/controls/WebValidate.css";
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

        public bool CheckDuplicate(WebDataSource wds, object[] value)
        {
            if (this.DuplicateCheck)
            {
                if (this.DuplicateCheckMode == DupCheckMode.ByLocal)
                {
                    DataRow row = wds.View.Table.Rows.Find(value);
                    if (row != null)
                    {
                        String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebValidate", "DeplicateWarning", true);
                        string script = "alert('" + message + "');";
                        CliUtils.RegisterStartupScript(this, script);
                        return false;
                    }
                }
                else
                {
                    StringBuilder whereBuilder = new StringBuilder();
                    for (int i = 0; i < wds.PrimaryKey.Length; i++)
                    {
                        if (whereBuilder.Length > 0)
                        {
                            whereBuilder.Append(" AND ");
                        }
                        whereBuilder.Append(string.Format("{0}", CliUtils.GetTableNameForColumn(wds.CommandText, wds.PrimaryKey[i].ColumnName)));
                        if (value[i] == null || value[i].ToString() == "")
                        {
                            whereBuilder.Append(" is null");
                        }
                        else if (wds.PrimaryKey[i].DataType == typeof(string) || wds.PrimaryKey[i].DataType == typeof(char)
                            || wds.PrimaryKey[i].DataType == typeof(Guid) || wds.PrimaryKey[i].DataType == typeof(DateTime))
                        {
                            whereBuilder.Append(string.Format("='{0}'", value[i]));
                        }
                        else
                        {

                            whereBuilder.Append(string.Format("={0}", value[i]));
                        }
                    }
                    string sql = CliUtils.InsertWhere(wds.CommandText, whereBuilder.ToString());
                    string remoteName = (string.IsNullOrEmpty(wds.SelectAlias) || string.IsNullOrEmpty(wds.SelectAlias)) ? wds.RemoteName : string.Empty;
                    int index = remoteName.LastIndexOf('.');
                    if (index <= 0 || index == remoteName.Length - 1)
                    {
                        throw new EEPException(EEPException.ExceptionType.PropertyInvalid, typeof(DBUtils), null, "RemoteName", remoteName);
                    }
                    DataSet ds = CliUtils.ExecuteSql(remoteName.Substring(0, index), remoteName.Substring(index + 1), sql, true, CliUtils.fCurrentProject);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebValidate", "DeplicateWarning", true);
                        string script = "alert('" + message + "');";
                        CliUtils.RegisterStartupScript(this, script);
                        return false;
                    }
                }
            }
            return true;
        }

        [Obsolete("The recommended alternative is CheckDuplicate", true)]
        public bool CheckDeplicate(ArrayList keyFields, ArrayList keyValues)
        {
            bool bRet = true;
            if (this.DuplicateCheck)
            {
                object obj = this.GetObjByID(this.DataSourceID);
                if (obj is WebDataSource)
                {
                    WebDataSource wds = (WebDataSource)obj;
                    if (this.DuplicateCheckMode == DupCheckMode.ByLocal)
                    {
                        List<bool> b = new List<bool>();
                        if (wds.SelectAlias != null && wds.SelectAlias != "" && wds.SelectCommand != null && wds.SelectCommand != "")
                        {
                            foreach (DataRow row in wds.CommandTable.Rows)
                            {
                                int i = keyFields.Count;
                                for (int j = 0; j < i; j++)
                                {
                                    if (row[keyFields[j].ToString()] == keyValues[j])
                                    {
                                        b.Add(true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (wds.InnerDataSet != null)
                            {
                                foreach (DataRow row in wds.View.Table.Rows)
                                {
                                    int i = keyFields.Count;
                                    int matchCount = 0;
                                    for (int j = 0; j < i; j++)
                                    {
                                        if (string.Compare(row[keyFields[j].ToString()].ToString(), keyValues[j].ToString()) == 0)
                                        {
                                            b.Add(true);
                                            matchCount++;
                                        }
                                    }
                                    if (matchCount == keyFields.Count)
                                        break;
                                    else
                                        b.Clear();
                                }
                            }
                        }

                        if (keyFields.Count == b.Count)
                        {
                            bool AllMatch = true;
                            foreach (bool bo in b)
                            {
                                if (!bo)
                                {
                                    AllMatch = false;
                                }
                            }
                            if (AllMatch)
                            {
                                //language = CliSysMegLag.GetClientLanguage();
                                String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebValidate", "DeplicateWarning", true);
                                string script = "alert('" + message + "')";
#if AjaxTools
                                Control panel = this.Parent;
                                while(panel != null && panel.GetType() != typeof(UpdatePanel))
                                {
                                    panel = panel.Parent;
                                }
                                if(panel != null)
                                {
                                    ScriptManager.RegisterStartupScript(panel as UpdatePanel, this.Page.GetType(), "ScriptBlock", script, true);
                                }
                                else
                                {
#endif
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), Guid.NewGuid().ToString(), "<script>" + script + "</script>");
#if AjaxTools
                                }
#endif
                                bRet = false;
                            }
                        }
                    }
                    else if (this.DuplicateCheckMode == DupCheckMode.ByWhere)
                    {
//                        string tabName = CliUtils.GetTableName(wds.CommandText, true);
//                        if (tabName != "")
//                        {
//                            sql = "select * from " + tabName + " where ";
//                            int i = keyFields.Count;
//                            for (int j = 0; j < i; j++)
//                            { 
//                                string type = keyValues[j].GetType().ToString().ToLower();
//                                if (type == "system.uint" || type == "system.uint16" || type == "system.uint32"
//                                    || type == "system.int64" || type == "system.int" || type == "system.int16"
//                                    || type == "system.uint32" || type == "system.uint64" || type == "system.single"
//                                    || type == "system.double" || type == "system.decimal" || type == "system.int32")
//                                {
//                                    sql += keyFields[j].ToString() + "=" + keyValues[j].ToString() + " and ";
//                                }
//                                else
//                                {
//                                    sql += keyFields[j].ToString() + "='" + keyValues[j].ToString() + "' and ";
//                                }
//                            }
//                            if (sql.IndexOf(" and ") != -1)
//                            {
//                                sql = sql.Substring(0, sql.LastIndexOf(" and "));
//                            }
//                        }
//                        DataSet dsTemp = new DataSet();
//                        if (strModuleName != "" && strTableName != "" && sql != "" && sCurProject != "")
//                        {
//                            //if (wds.SelectAlias != null && wds.SelectAlias != "" && wds.SelectCommand != null && wds.SelectCommand != "")
//                            //{
//                            //    dsTemp = CliUtils.ExecuteSql(strModuleName, strTableName, sql, wds.SelectAlias, true, sCurProject);
//                            //}
//                            //else
//                            //{
//                                dsTemp = CliUtils.ExecuteSql(strModuleName, strTableName, sql, true, sCurProject);
//                            //}
//                        }
//                        if (dsTemp.Tables.Count != 0 && dsTemp.Tables[0].Rows.Count != 0)
//                        {
//                            String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebValidate", "DeplicateWarning", true);
//                            string script = "alert('" + message + "')";
//#if AjaxTools
//                            Control panel = this.Parent;
//                            while(panel != null && panel.GetType() != typeof(UpdatePanel))
//                            {
//                                panel = panel.Parent;
//                            }
//                            if(panel != null)
//                            {
//                                ScriptManager.RegisterStartupScript(panel as UpdatePanel, this.Page.GetType(), "ScriptBlock", script, true);
//                            }
//                            else
//                            {
//#endif
//                                Page.ClientScript.RegisterStartupScript(Page.GetType(), Guid.NewGuid().ToString(), "<script>" + script + "</script>");
//#if AjaxTools
//                            }
//#endif
//                            bRet = false;
//                        }
                    }
                }
            }
            return bRet;
        }

        //private String _quotePrefix = "[";
        //private String _quoteSuffix = "]";
        ////Modified by lily 2008/5/4 for Owner.table should be Ownder.[table] not [Owner.table]
        //private String Quote(String table_or_column)
        //{
        //    int i = table_or_column.IndexOf(".");
        //    object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB });
        //    if (myRet != null && (int)myRet[0] == 0)
        //    {
        //        switch (myRet[1].ToString())
        //        {
        //            case "1":
        //                if (i >= 0)
        //                {
        //                    table_or_column = table_or_column.Substring(0, i + 1) + _quotePrefix + table_or_column.Substring(i + 1) + _quoteSuffix;
        //                    break;
        //                }
        //                else
        //                {
        //                    table_or_column = _quotePrefix + table_or_column + _quoteSuffix; break;
        //                }
        //        }
        //    }
        //    return table_or_column;
        //}

        private string _rowCss = "";
        [Category("Infolight")]
        public string RowCss
        {
            get { return _rowCss; }
            set { _rowCss = value; }
        }

        private string _altRowCss = "";
        [Category("Infolight")]
        public string AltRowCss
        {
            get { return _altRowCss; }
            set { _altRowCss = value; }
        }

        private DataSet dataSetDD;

        private string GetDDCatption(string columnname)
        {
            if (dataSetDD == null)
            {
                object obj = this.GetObjByID(this.DataSourceID);
                if (obj is WebDataSource)
                {
                    dataSetDD = DBUtils.GetDataDictionary(obj as WebDataSource, false);
                }
            }
            string CaptionNum = "CAPTION";
            if (this.MultiLanguage)
            {
                switch (CliUtils.fClientLang)
                {
                    case SYS_LANGUAGE.ENG:
                        CaptionNum = "CAPTION1"; break;
                    case SYS_LANGUAGE.TRA:
                        CaptionNum = "CAPTION2"; break;
                    case SYS_LANGUAGE.SIM:
                        CaptionNum = "CAPTION3"; break;
                    case SYS_LANGUAGE.HKG:
                        CaptionNum = "CAPTION4"; break;
                    case SYS_LANGUAGE.JPN:
                        CaptionNum = "CAPTION5"; break;
                    case SYS_LANGUAGE.LAN1:
                        CaptionNum = "CAPTION6"; break;
                    case SYS_LANGUAGE.LAN2:
                        CaptionNum = "CAPTION7"; break;
                    case SYS_LANGUAGE.LAN3:
                        CaptionNum = "CAPTION8"; break;
                }
            }
            string caption = columnname;
            foreach (DataRow dr in dataSetDD.Tables[0].Rows)
            {
                if (string.Compare(dr["FIELD_NAME"].ToString(), columnname, true) == 0 && !string.IsNullOrEmpty(dr[CaptionNum].ToString()))
                {
                    caption = dr[CaptionNum].ToString();
                }
            }
            return caption;
        }

        public bool CheckValidate(IDictionary newValues)
        {
            bool isValidateSuccessful = true;
            IDictionaryEnumerator ide = newValues.GetEnumerator();
            if (this.ValidateActive)
            {
                DataTable dt = new DataTable();
                object obj = this.GetObjByID(this.DataSourceID);
                if (obj is WebDataSource)
                {
                    WebDataSource wds = (WebDataSource)obj;
                    dt = wds.View.Table;
                }
                string aMessage = "";
                bool isAltRow = false;
                while (ide.MoveNext())
                {
                    foreach (ValidateFieldItem vfi in this.Fields)
                    {
                        if (vfi.FieldName != null && vfi.FieldName != "" 
                            && ide.Key != null && ide.Key.ToString() != "" 
                            && ide.Key.ToString() == vfi.FieldName)
                        {
                            // CheckNull
                            if (vfi.CheckNull)
                            {
                                if (ide.Value == null || ide.Value.ToString() == "")
                                {
                                    String message = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                             "Srvtools",
                                                                             "WebValidate",
                                                                             "msg_WebValidateCheckNull",true);
                                    string caption = GetDDCatption(vfi.FieldName);
                                    if (this.ValidateStyle == ValidateStyles.ShowLable)
                                    {
                                        if (isAltRow)
                                        {
                                            if (string.IsNullOrEmpty(this.AltRowCss))
                                                this.Text += string.Format(message, caption) + "<br>";
                                            else
                                                this.Text += "<div class='" + this.AltRowCss + "'>" + string.Format(message, caption) + "</div>";
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(this.RowCss))
                                                this.Text += string.Format(message, caption) + "<br>";
                                            else
                                                this.Text += "<div class='" + this.RowCss + "'>" + string.Format(message, caption) + "</div>";
                                        }
                                        isAltRow = !isAltRow;
                                    }
                                    else
                                    {
                                        aMessage += string.Format(message, caption) + "\\n\\r";
                                        //Page.Response.Write("<script>alert('" + string.Format(message, caption) + "')</script>");
                                    }
                                    isValidateSuccessful = false;
                                }
                            }
                            // CheckRange
   
                            int belowRangeFlag = 1;
                            int aboveRangeFlag = -1;
                            int rangeExsitFlag = 0;
                            if (vfi.CheckRangeFrom != null && vfi.CheckRangeFrom != "")
                            {
                                belowRangeFlag = CompareRange(dt.Columns[ide.Key.ToString()].DataType, ide.Value, vfi.CheckRangeFrom);
                                rangeExsitFlag |= 1;
                            }
                            if (vfi.CheckRangeTo != null && vfi.CheckRangeTo != "")
                            {
                                aboveRangeFlag = CompareRange(dt.Columns[ide.Key.ToString()].DataType, ide.Value, vfi.CheckRangeTo);
                                rangeExsitFlag |= 2;
                            }

                            if ((int)belowRangeFlag < 0 || (int)aboveRangeFlag > 0)
                            {
                                string caption = GetDDCatption(vfi.FieldName);
                                string message = "";
                                if (rangeExsitFlag == 1)
                                {
                                    string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                         "Srvtools",
                                                                         "WebValidate",
                                                                         "msg_WebValidateCheckRangeFrom", true);
                                    message = string.Format(mess, caption, vfi.CheckRangeFrom);

                                }
                                else if (rangeExsitFlag == 2)
                                {
                                    string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                         "Srvtools",
                                                                         "WebValidate",
                                                                         "msg_WebValidateCheckRangeTo", true);
                                    message = string.Format(mess, caption, vfi.CheckRangeTo);

                                }
                                else
                                {
                                    string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                         "Srvtools",
                                                                         "WebValidate",
                                                                         "msg_WebValidateCheckRange", true);
                                    message = string.Format(mess, caption, vfi.CheckRangeFrom, vfi.CheckRangeTo);
                                }
                                if (this.ValidateStyle == ValidateStyles.ShowLable)
                                {
                                    if (isAltRow)
                                    {
                                        if (string.IsNullOrEmpty(this.AltRowCss))
                                            this.Text += message + "<br>";
                                        else
                                            this.Text += "<div class='" + this.AltRowCss + "'>" + message + "</div>";
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(this.RowCss))
                                            this.Text += message + "<br>";
                                        else
                                            this.Text += "<div class='" + this.RowCss + "'>" + message + "</div>";
                                    }
                                    isAltRow = !isAltRow;
                                }
                                else
                                {
                                    aMessage += message + "\\n\\r";
                                    //Page.Response.Write("<script>alert('" + message + "')</script>");
                                }
                                isValidateSuccessful = false;
                            }
                            
                            // CheckValidateMethod
                            if (vfi.Validate != null && vfi.Validate != "")
                            {
                                string functionName = vfi.Validate;
                                object validateValue = ide.Value;
                                bool RetValue = InvokeOwerMethod(functionName, validateValue);
                                if (!RetValue)
                                {
                                    if (vfi.WarningMsg != null && vfi.WarningMsg != "")
                                    {
                                        if (this.ValidateStyle == ValidateStyles.ShowLable)
                                        {
                                            if (isAltRow)
                                            {
                                                if (string.IsNullOrEmpty(this.AltRowCss))
                                                    this.Text += (string)vfi.WarningMsg.Clone() + "<br>";
                                                else
                                                    this.Text += "<div class='" + this.AltRowCss + "'>" + (string)vfi.WarningMsg.Clone() + "</div>";
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(this.RowCss))
                                                    this.Text += (string)vfi.WarningMsg.Clone() + "<br>";
                                                else
                                                    this.Text += "<div class='" + this.RowCss + "'>" + (string)vfi.WarningMsg.Clone() + "</div>";
                                            }
                                            isAltRow = !isAltRow;
                                        }
                                        else
                                        {
                                            aMessage += (string)vfi.WarningMsg.Clone() + "\\n\\r";
                                            //Page.Response.Write("<script>alert('" + (string)vfi.WarningMsg.Clone() + "')</script>");
                                        }
                                    }
                                    else
                                    {
                                        string caption = GetDDCatption(vfi.FieldName);
                                        String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebValidate", "Error_Msg", true);
                                        if (this.ValidateStyle == ValidateStyles.ShowLable)
                                        {
                                            if (isAltRow)
                                            {
                                                if (string.IsNullOrEmpty(this.AltRowCss))
                                                    this.Text += string.Format(message, caption) + "<br>";
                                                else
                                                    this.Text += "<div class='" + this.AltRowCss + "'>" + string.Format(message, caption) + "</div>";
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(this.RowCss))
                                                    this.Text += string.Format(message, caption) + "<br>";
                                                else
                                                    this.Text += "<div class='" + this.RowCss + "'>" + string.Format(message, caption) + "</div>";
                                            }
                                            isAltRow = !isAltRow;
                                        }
                                        else
                                        {
                                            aMessage += string.Format(message, caption) + "\\n\\r";
                                            //Page.Response.Write("<script>alert('" + string.Format(message, caption) + "')</script>");
                                        }
                                    }
                                    isValidateSuccessful = false;
                                }
                            }
                        }
                    }
                }
                if (aMessage != "" && this.ValidateStyle == ValidateStyles.ShowDialog)
                {
                    string script = "alert('" + aMessage + "')";
#if AjaxTools
                    Control panel = this.Parent;
                    while(panel != null && panel.GetType() != typeof(UpdatePanel))
                    {
                        panel = panel.Parent;
                    }
                    if(panel != null)
                    {
                        ScriptManager.RegisterStartupScript(panel as UpdatePanel, this.Page.GetType(), "ScriptBlock", script, true);
                    }
                    else
                    {
#endif
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), Guid.NewGuid().ToString(), "<script>" + script + "</script>");
#if AjaxTools
                    }
#endif
                }
            }
            return isValidateSuccessful;
        }

        public object[] CheckValidate(string fieldName, string value)
        {
            object[] retObj = new object[4] 
                             { 
                                 true, // all validate sucess
                                 "",   // check null
                                 "",   // check range
                                 ""    // check method
                             };
            if (this.ValidateActive)
            {
                DataTable dt = new DataTable();
                object obj = this.GetObjByID(this.DataSourceID);
                if (obj is WebDataSource)
                {
                    WebDataSource wds = (WebDataSource)obj;
                    dt = wds.View.Table;
                }
                foreach (ValidateFieldItem vfi in this.Fields)
                {
                    if (vfi.FieldName == fieldName)
                    {
                       
                        if (vfi.CheckNull)
                        {
                            if (value == null || value == "")
                            {
                                string fieldNameCaption = GetDDCatption(fieldName);
                                retObj[0] = false;
                                String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebValidate", "msg_WebValidateCheckNull", true);
                                retObj[1] = string.Format(message, fieldNameCaption);
                            }
                        }
                        int belowRangeFlag = 1;
                        int aboveRangeFlag = -1;
                        int rangeExsitFlag = 0;
                        if (vfi.CheckRangeFrom != null && vfi.CheckRangeFrom != "")
                        {
                            belowRangeFlag = CompareRange(dt.Columns[fieldName].DataType, value, vfi.CheckRangeFrom);
                            rangeExsitFlag |= 1;
                        }
                        if (vfi.CheckRangeTo != null && vfi.CheckRangeTo != "")
                        {
                            aboveRangeFlag = CompareRange(dt.Columns[fieldName].DataType, value, vfi.CheckRangeTo);
                            rangeExsitFlag |= 2;
                        }
                        if (belowRangeFlag < 0 || aboveRangeFlag > 0)
                        {
                            retObj[0] = false;
                            string message = "";
                            string fieldNameCaption = GetDDCatption(fieldName);
                            if (rangeExsitFlag == 1)
                            {
                                string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                     "Srvtools",
                                                                     "WebValidate",
                                                                     "msg_WebValidateCheckRangeFrom", true);
                                message = string.Format(mess, fieldNameCaption, vfi.CheckRangeFrom);

                            }
                            else if (rangeExsitFlag == 2)
                            {
                                string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                     "Srvtools",
                                                                     "WebValidate",
                                                                     "msg_WebValidateCheckRangeTo", true);
                                message = string.Format(mess, fieldNameCaption, vfi.CheckRangeTo);

                            }
                            else
                            {
                                string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                     "Srvtools",
                                                                     "WebValidate",
                                                                     "msg_WebValidateCheckRange", true);
                                message = string.Format(mess, fieldNameCaption, vfi.CheckRangeFrom, vfi.CheckRangeTo);
                            }
                            retObj[2] = message;
                        }
                        if (vfi.Validate != null && vfi.Validate != "")
                        {
                            string functionName = vfi.Validate;
                            object validateValue = value;
                            bool RetValue = InvokeOwerMethod(functionName, validateValue);
                            if (!RetValue)
                            {
                                retObj[0] = false;
                                if (vfi.WarningMsg != null && vfi.WarningMsg != "")
                                {
                                    retObj[3] = vfi.WarningMsg;
                                }
                                else
                                {
                                    string fieldNameCaption = GetDDCatption(fieldName);
                                    String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebValidate", "Error_Msg", true);
                                    retObj[3] = string.Format(message, fieldNameCaption);
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return retObj;
        }

        private bool InvokeOwerMethod(string functionName, object parameters)
        {
            Char[] cs = functionName.ToCharArray();
            if (cs.Length == 0)
            {
                return true;
            }
            Char[] sep1 = "()".ToCharArray();
            String[] sps1 = functionName.Split(sep1);
            if (sps1.Length == 3)
            {
                Type type = this.Page.GetType();
                MethodInfo function = type.GetMethod(sps1[0], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (function == null || function.ReturnType.FullName != "System.Boolean" || function.GetParameters().GetLength(0) != 1)
                {
                    return true;
                }
                if (parameters == null) 
                    parameters = "";
                bool retValue = (bool)function.Invoke(this.Page, new object[1] { parameters });
                return retValue;
            }
            return true;
        }

        private int CompareRange(Type valType, object value, string strRange)
        {
            int range = 0;
            if (value == null || value.ToString() == "")
            {
                range = -1;
                return range;
            }
            if (valType == typeof(string))
            {
                //range = ((string)value).CompareTo(strRange);
                int count = 0;
                string s = value.ToString();
                int i = s.Length;
                int j = strRange.Length;
                if (i >= j)
                    count = j;
                else
                    count = i;
                for (int m = 0; m < count; m++)
                {
                    if (s[m] > strRange[m])
                    {
                        range = 2;
                        break;
                    }
                    else if (s[m] < strRange[m])
                    {
                        range = -2;
                        break;
                    }
                }
                if (range == -1)
                {
                    if (i > j)
                        range = 1;
                    else if (i == j)
                        range = 0;
                }
            }
            else if (valType == typeof(Int16))
            {
                range = Convert.ToInt16(value).CompareTo(Convert.ToInt16(strRange));
            }
            else if (valType == typeof(Int32))
            {
                range = Convert.ToInt32(value).CompareTo(Convert.ToInt32(strRange));
            }
            else if (valType == typeof(Int64))
            {
                range = Convert.ToInt64(value).CompareTo(Convert.ToInt64(strRange));
            }
            else if (valType == typeof(UInt16))
            {
                range = Convert.ToUInt16(value).CompareTo(Convert.ToUInt16(strRange));
            }
            else if (valType == typeof(UInt32))
            {
                range = Convert.ToUInt32(value).CompareTo(Convert.ToUInt32(strRange));
            }
            else if (valType == typeof(UInt64))
            {
                range = Convert.ToUInt64(value).CompareTo(Convert.ToUInt64(strRange));
            }
            else if (valType == typeof(float))
            {
                range = Convert.ToSingle(value).CompareTo(Convert.ToSingle(strRange));
            }
            else if (valType == typeof(double))
            {
                range = Convert.ToDouble(value).CompareTo(Convert.ToDouble(strRange));
            }
            else if (valType == typeof(decimal))
            {
                range = Convert.ToDecimal(value).CompareTo(Convert.ToDecimal(strRange));
            }
            else if (valType == typeof(DateTime))
            {
                range = Convert.ToDateTime(value).CompareTo(Convert.ToDateTime(strRange));
            }
            return range;
        }
    }
    
    #region ValidateFieldsCollection class
    public class ValidateFieldsCollection : InfoOwnerCollection
    {
        public ValidateFieldsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(ValidateFieldItem))
        {
        }

        public new ValidateFieldItem this[int index]
        {
            get
            {
                return (ValidateFieldItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ValidateFieldItem)
                    {
                        //原来的Collection设置为0
                        ((ValidateFieldItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((ValidateFieldItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
    #endregion

    #region ValidateFieldItem class
    public class ValidateFieldItem : InfoOwnerCollectionItem, IGetValues
    {
        public ValidateFieldItem()
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

        private string _CheckRangeFrom;
        [Category("Check Validate"),
        NotifyParentProperty(true)]
        public string CheckRangeFrom
        {
            get
            {
                return _CheckRangeFrom;
            }
            set
            {
                _CheckRangeFrom = value;
            }
        }

        private string _CheckRangeTo;
        [Category("Check Validate"),
        NotifyParentProperty(true)]
        public string CheckRangeTo
        {
            get
            {
                return _CheckRangeTo;
            }
            set
            {
                _CheckRangeTo = value;
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

        private bool _CheckNull;
        [Category("Check Validate"),
        NotifyParentProperty(true),
        DefaultValue(false)]
        public bool CheckNull
        {
            get
            {
                return _CheckNull;
            }
            set
            {
                _CheckNull = value;
            }
        }

        private string _Validate;
        [Category("Check Validate"),
        NotifyParentProperty(true)]
        public string Validate
        {
            get
            {
                return _Validate;
            }
            set
            {
                _Validate = value;
            }
        }

        private string _WarningMsg;
        [Category("Message"),
        NotifyParentProperty(true)]
        public string WarningMsg
        {
            get
            {
                return _WarningMsg;
            }
            set
            {
                _WarningMsg = value;
            }
        }

        private string _ValidateLabelLink;
        [NotifyParentProperty(true), 
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        //[Browsable(false)]
        //[Obsolete("Property of ValidateLabelLink is obsolete", false)]
        public string ValidateLabelLink
        {
            get
            {
                return _ValidateLabelLink;
            }
            set
            {
                _ValidateLabelLink = value;
                if (_ValidateLabelLink != null && _ValidateLabelLink != "" && _ValidateLabelLink.IndexOf(" (") != -1)
                {
                    _ValidateLabelLink = _ValidateLabelLink.Substring(0, _ValidateLabelLink.IndexOf(" ("));
                }
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
                if (this.Owner is WebValidate)
                {
                    WebValidate wv = (WebValidate)this.Owner;
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
                    }
                }
            }
            else if (string.Compare(sKind, "validatelabellink", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebValidate)
                {
                    WebValidate wv = (WebValidate)this.Owner;
                    Control ctrl = wv.ExtendedFindChildControl(wv.DataSourceID, FindControlType.DataSourceID, typeof(WebFormView));
                    if (ctrl != null && ctrl is WebFormView)
                    {
                        WebFormView formView = (WebFormView)ctrl;
                        IDesignerHost host = (IDesignerHost)formView.Site.GetService(typeof(IDesignerHost));
                        //这里先偷懒这样写，照理说要写3个Template，去取得其中的所有Label，但是.net FormView中显示字段的Label的Name是一样的
                        BindableTemplateBuilder editTemplate = (BindableTemplateBuilder)formView.EditItemTemplate; 
                        string content = editTemplate.Text;
                        Control[] ctrls = ControlParser.ParseControls(host, content);
                        foreach (Control control in ctrls)
                        {
                            if (control.ID != null && control.ID != "" && control is Label)
                            {
                                values.Add(control.ID);
                            }
                        }
                    }
                }
            }
            if (values.Count > 0)
            {
                retList = values.ToArray();
            }
            return retList;
        }
        #endregion
    }
    #endregion

    #region DataSourceEditor
    public class DataSourceEditor : UITypeEditor
    {
        public DataSourceEditor()
            : base()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance is WebValidate)
            {
                ControlCollection ctrlList = ((WebValidate)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource)
                    {
                        objName.Add(ctrl.ID);
                    }
                }
            }
            else if (context.Instance is WebDefault)
            {
                ControlCollection ctrlList = ((WebDefault)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource)
                    {
                        objName.Add(ctrl.ID);
                    }
                }
            }
            else if (context.Instance is WebTranslate)
            {
                ControlCollection ctrlList = ((WebTranslate)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource)
                    {
                        objName.Add(ctrl.ID);
                    }
                }
            }
            else if (context.Instance is WebRefValBase)
            {
                ControlCollection ctrlList = ((WebRefValBase)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource)
                    {
                        objName.Add(ctrl.ID);
                    }
                }
            }
            else if (context.Instance is WebMenu)
            {
                ControlCollection ctrlList = ((WebMenu)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource)
                    {
                        objName.Add(ctrl.ID);
                    }
                }
            }
            else if (context.Instance is WebListBoxList)
            {
                ControlCollection ctrlList = ((WebListBoxList)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource)
                    {
                        objName.Add(ctrl.ID);
                    }
                }
            }
            else if (context.Instance is WebSecColumns)
            {
                ControlCollection ctrlList = ((WebSecColumns)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource)
                    {
                        objName.Add(ctrl.ID);
                    }
                }
            }
            else if (context.Instance is WebDropDownList)
            {
                ControlCollection ctrlList = ((WebDropDownList)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource)
                    {
                        objName.Add(ctrl.ID);
                    }
                }
            }
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, objName.ToArray());
                string strValue = (string)value;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }
    #endregion

    #region FieldNameEditor
    public class FieldNameEditor : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;

        public FieldNameEditor()
        {
        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog, drop down dialog, or no UI outside of the properties window.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // Uses the IWindowsFormsEditorService to display a drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            IGetValues aItem = (IGetValues)context.Instance;
            if (edSvc != null)
            {
                StringListSelector mySelector = new StringListSelector(edSvc, aItem.GetValues(context.PropertyDescriptor.Name));
                string strValue = (string)value;
                if (mySelector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }
    #endregion
}