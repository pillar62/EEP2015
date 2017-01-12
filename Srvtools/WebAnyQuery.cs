using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing.Design;
using System.Collections;
using System.ComponentModel.Design;
using System.Web.UI.Design;
using System.Windows.Forms.Design;

namespace Srvtools
{
    [Designer(typeof(DataSourceDesigner), typeof(IDesigner))]
    [ToolboxData("<{0}:WebAnyQuery runat=server></{0}:WebAnyQuery>")]
    public class WebAnyQuery : WebControl
    {
        public WebAnyQuery()
        {
            _Columns = new WebAnyQueryColumnsCollection(this, typeof(WebAnyQueryColumns));
        }

        [Category("Infolight"),
        Description("The ID of WebDataSource which the control is bound to")]
        [Editor(typeof(wanyqueryDataSourceEditor), typeof(UITypeEditor))]
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

        private WebAnyQueryColumnsCollection _Columns;
        [Category("Infolight"),
        Description("The columns which WebClientQuery is applied to")]
        [PersistenceMode(PersistenceMode.InnerProperty),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         TypeConverter(typeof(CollectionConverter)),
         NotifyParentProperty(true)]
        public WebAnyQueryColumnsCollection Columns
        {
            get
            {
                return _Columns;
            }
        }

        private int _MaxColumnCount = -1;
        [Category("Infolight"), DefaultValue(-1)]
        public int MaxColumnCount
        {
            get
            {
                return _MaxColumnCount;
            }
            set
            {
                _MaxColumnCount = value;
            }
        }

        private bool _AutoDisableColumns = true;
        [Browsable(false)]
        [Category("Infolight"), DefaultValue(true)]
        public bool AutoDisableColumns
        {
            get
            {
                return _AutoDisableColumns;
            }
            set
            {
                _AutoDisableColumns = value;
            }
        }

        private String _AnyQueryID = String.Empty;
        [Category("Infolight")]
        public String AnyQueryID
        {
            get
            {
                if (_AnyQueryID == String.Empty && this.ID != null)
                    _AnyQueryID = this.ID;
                return _AnyQueryID;
            }
            set
            {
                _AnyQueryID = value;
            }
        }

        private bool _AllowAddQueryField = true;
        [Category("Infolight"), DefaultValue(true)]
        public bool AllowAddQueryField
        {
            get
            {
                return _AllowAddQueryField;
            }
            set
            {
                _AllowAddQueryField = value;
            }
        }

        private AnyQueryColumnMode _QueryColumnMode = AnyQueryColumnMode.ByBindingSource;
        [Category("Infolight"), DefaultValue(AnyQueryColumnMode.ByBindingSource)]
        public AnyQueryColumnMode QueryColumnMode
        {
            get
            {
                return _QueryColumnMode;
            }
            set
            {
                _QueryColumnMode = value;
            }
        }

        private bool _DisplayAllOperator;
        [Category("Infolight"), DefaultValue(false)]
        public bool DisplayAllOperator
        {
            get
            {
                return _DisplayAllOperator;
            }
            set
            {
                _DisplayAllOperator = value;
            }
        }

        public void Execute()
        {
            Execute(false);
        }

        public void Execute(bool dialog)
        {
            string url = AddParam();
            if (dialog)
            {
                url += "&Dialog=true";
            }
            if (SessionRequest.Enable)
            {
                SessionRequest sessionRequest = new SessionRequest(this.Page);
                url = "../InnerPages/frmAnyQuery.aspx?" + sessionRequest.SetRequestValue(url);
            }
            else
            {
                url = "../InnerPages/frmAnyQuery.aspx?" + QueryStringEncrypt.Encrypt(url);
            }
            if (dialog)
            {
                string script = string.Format("<script>window.open('{0}','query','left=200,top=200,width=700,scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no');</script>", url);
                Page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script);
            }
            else
            {
                Page.Response.Redirect(url);
            }
        }

        private ArrayList isShow = new ArrayList();

        public ArrayList Get_isShow()
        {
            return isShow;
        }

        //public void Execute(Panel pn)
        //{
        //    this.Execute(pn, true);
        //}

        //public void Execute(Panel pn, bool noredirect)
        //{
        //    string wherestring = GetWhere(pn);
        //    for (int i = 0; i < this.Columns.Count; i++)
        //    {
        //        WebAnyQueryColumns column = this.Columns[i] as WebAnyQueryColumns;
        //        if (column.NotNull && string.IsNullOrEmpty(column.Text))
        //        {
        //            this.Page.ClientScript.RegisterStartupScript(typeof(string), ""
        //                , string.Format("<script>alert('{0} can not be empty')</script>", column.Caption));
        //            return;
        //        }
        //    }
        //    if (noredirect)
        //    {
        //        WebDataSource wds = (WebDataSource)GetObjByID(this.DataSourceID);
        //        wds.SetWhere(wherestring);
        //    }
        //    else
        //    {
        //        this.Page.Response.Redirect(this.Page.Request.FilePath + "?Filter=" + HttpUtility.UrlEncode(wherestring) + "&DataSourceID=" + this.DataSourceID
        //          + "&QueryText=" + querytext + "&PanelID=" + pn.ID + "&IsQueryBack=1" + "&WebAnyQueryID=" + this.ID);
        //    }
        //}

        private string AddParam()
        {
            WebDataSource wds = new WebDataSource();
            wds = (WebDataSource)GetObjByID(this.DataSourceID);

            if (this.Columns.Count == 0)
            {
                throw new Exception("no columns in ClientQuery");
            }

            string url = "Caption=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                url += HttpUtility.UrlEncode(qc.Caption) + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Column=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.Column == string.Empty)
                {
                    throw new Exception(string.Format("The columnname of column[{0}] is empty", this.Columns.GetItemIndex(qc).ToString()));
                }
                url += qc.Column + ";";
            }

            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Condition=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                url += qc.Condition + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Operator=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                url += qc.Operator + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Columntype=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                url += qc.ColumnType + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Textwidth=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                url += qc.Width.ToString() + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Text=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                url += qc.Text + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Defaultvalue=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (GetDefaultValue(qc.DefaultValue) != null)
                {
                    url += GetDefaultValue(qc.DefaultValue).ToString() + ";";
                }
                else
                {
                    url += ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Enabled=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                url += qc.Enabled.ToString() + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&AutoSelect=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                url += qc.AutoSelect.ToString() + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            //url += "&Items=";
            //foreach (WebAnyQueryColumns qc in this.Columns)
            //{
            //    url += qc + ";";
            //}
            //url = url.Substring(0, url.LastIndexOf(';'));

            #region refval
            url += "&Refvalvf=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefVal == string.Empty || qc.WebRefVal == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefVal wrv = (WebRefVal)GetObjByID(qc.WebRefVal);
                    url += wrv.DataValueField + ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));

            url += "&Refvaltf=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefVal == string.Empty || qc.WebRefVal == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefVal wrv = (WebRefVal)GetObjByID(qc.WebRefVal);
                    url += wrv.DataTextField + ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));

            url += "&Refvalcd=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefVal == string.Empty || qc.WebRefVal == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefVal wrv = (WebRefVal)GetObjByID(qc.WebRefVal);
                    url += wrv.CheckData.ToString() + ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));

            url += "&Refvalselcmd=";
            String strRefvalselcmd = String.Empty;
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefVal == string.Empty || qc.WebRefVal == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefVal wrv = (WebRefVal)GetObjByID(qc.WebRefVal);
                    WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                    url += HttpUtility.UrlEncode(refvalwds.SelectCommand.Replace("\r\n", " ")) + ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));

            url += "&Refvalselalias=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefVal == string.Empty || qc.WebRefVal == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefVal wrv = (WebRefVal)GetObjByID(qc.WebRefVal);
                    WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                    url += refvalwds.SelectAlias + ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));

            url += "&Refvaldstid=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefVal == string.Empty || qc.WebRefVal == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefVal wrv = (WebRefVal)GetObjByID(qc.WebRefVal);
                    WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                    url += refvalwds.WebDataSetID + ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));

            url += "&Refvaldm=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefVal == string.Empty || qc.WebRefVal == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefVal wrv = (WebRefVal)GetObjByID(qc.WebRefVal);
                    WebDataSource refvalwds = (WebDataSource)GetObjByID(wrv.DataSourceID);
                    url += refvalwds.DataMember + ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));

            url += "&RefvalSize=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefVal == string.Empty || qc.WebRefVal == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefVal wrv = (WebRefVal)GetObjByID(qc.WebRefVal);

                    url += wrv.OpenRefHeight.ToString() + "," + wrv.OpenRefLeft.ToString() + "," + wrv.OpenRefTop.ToString() + "," + wrv.OpenRefWidth.ToString() + ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));


            #region Refvalcolumnmatch
            url += "&Refvalcolumnmatch=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefVal == string.Empty || qc.WebRefVal == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefVal wrv = (WebRefVal)GetObjByID(qc.WebRefVal);

                    if (wrv.ColumnMatch.Count > 0)
                    {
                        foreach (WebColumnMatch wcm in wrv.ColumnMatch)
                        {
                            url += wcm.DestControlID + "," + wcm.SrcField + "," + wcm.SrcGetValue + ":";

                        }
                        url = url.Substring(0, url.LastIndexOf(':'));
                    }
                    url += ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            #endregion

            #region Refvalcolumns
            url += "&Refvalcolumns=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefVal == string.Empty || qc.WebRefVal == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefVal wrv = (WebRefVal)GetObjByID(qc.WebRefVal);

                    if (wrv.Columns.Count > 0)
                    {
                        foreach (WebRefColumn wrc in wrv.Columns)
                        {
                            url += wrc.ColumnName + "," + wrc.HeadText + "," + wrc.Width.ToString() + ":";

                        }
                        url = url.Substring(0, url.LastIndexOf(':'));
                    }
                    url += ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            #endregion

            #region Refvalwhereitem
            url += "&Refvalwhereitem=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefVal == string.Empty || qc.WebRefVal == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefVal wrv = (WebRefVal)GetObjByID(qc.WebRefVal);

                    if (wrv.WhereItem.Count > 0)
                    {
                        foreach (WebWhereItem wwi in wrv.WhereItem)
                        {
                            url += wwi.Condition + "," + wwi.FieldName + "," + wwi.Value + ":";

                        }
                        url = url.Substring(0, url.LastIndexOf(':'));
                    }
                    url += ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            #endregion

            #endregion

            #region refbutton
            url += "&RefButtonurl=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefButton == string.Empty || qc.WebRefButton == null)
                {
                    url += "@";
                }
                else
                {
                    WebRefButton wrb = (WebRefButton)GetObjByID(qc.WebRefButton);
                    url += wrb.RefURL + "@";
                }
            }
            url = url.Substring(0, url.LastIndexOf("@"));

            url += "&RefButtonurlSize=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefButton == string.Empty || qc.WebRefButton == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefButton wrb = (WebRefButton)GetObjByID(qc.WebRefButton);
                    url += wrb.RefURLHeight.ToString() + "," + wrb.RefURLLeft.ToString() + "," + wrb.RefURLTop.ToString() + "," + wrb.RefURLWidth.ToString() + ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(";"));

            url += "&RefButtoncaption=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                if (qc.WebRefButton == string.Empty || qc.WebRefButton == null)
                {
                    url += ";";
                }
                else
                {
                    WebRefButton wrb = (WebRefButton)GetObjByID(qc.WebRefButton);
                    url += wrb.Caption + ";";
                }
            }
            url = url.Substring(0, url.LastIndexOf(";"));
            #endregion

            url += "&Datatype=";
            foreach (WebAnyQueryColumns qc in this.Columns)
            {
                string strtype = wds.InnerDataSet.Tables[wds.DataMember].Columns[qc.Column].DataType.ToString();
                url += strtype + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));

            url += "&Pagepath=" + this.Page.Request.FilePath;
            url += "&Psypagepath=" + HttpUtility.UrlEncode(this.Page.Request.PhysicalPath);
            url += "&DataSourceID=" + this.DataSourceID;
            url += "&RemoteName=" + wds.RemoteName;
            url += "&WebAnyQueryID=" + this.ID;
            url += "&WebDataSetID=" + wds.WebDataSetID;
            url += "&QueryColumnMode=" + this.QueryColumnMode;
            url += "&AutoDisableColumns=" + this.AutoDisableColumns;
            url += "&DataMember=" + wds.DataMember;
            url += "&MaxColumnCount=" + this.MaxColumnCount;
            url += "&AnyQueryID=" + this.AnyQueryID;
            url += "&AllowAddQueryField=" + this.AllowAddQueryField;
            url += "&DisplayAllOperator=" + this.DisplayAllOperator;

            string itemparam = this.Page.Request.QueryString["ItemParam"] != null ? HttpUtility.UrlEncode(this.Page.Request.QueryString["ItemParam"]) : string.Empty;
            url += "&ItemParam=" + itemparam;
            return url;
        }

        private string querytext = "";
        public string GetWhere(Panel pn)
        {
            if (this.isShow.Contains(pn.ID))
            {
                int columnNum = this.Columns.Count;
                WebDataSource wds = (WebDataSource)GetObjByID(this.DataSourceID);

                string[] strText = new string[columnNum];
                string[] queryText = new string[columnNum];

                for (int i = 0; i < columnNum; i++)
                {
                    string strControlName = "txt" + i.ToString();
                    Control ct = this.GetControlByID(strControlName, pn);
                    strText[i] = "";
                    queryText[i] = "";
                    Type type = wds.InnerDataSet.Tables[wds.DataMember].Columns[((WebAnyQueryColumns)this.Columns[i]).Column].DataType;
                    string columntype = ((WebAnyQueryColumns)this.Columns[i]).ColumnType;

                    if (ct != null && columntype == "ClientQueryTextBoxColumn")
                    {
                        strText[i] = (ct as TextBox).Text;
                        queryText[i] = strText[i];
                    }
                    else if (ct != null && columntype == "ClientQueryRefButtonColumn")
                    {
                        strText[i] = (ct as TextBox).Text;
                        queryText[i] = strText[i];
                    }

                    else if (ct != null && columntype == "ClientQueryComboBoxColumn")
                    {
                        strText[i] = (ct as WebDropDownList).SelectedValue.ToString();
                        queryText[i] = strText[i];
                    }
                    else if (ct != null && columntype == "ClientQueryCheckBoxColumn")
                    {
                        if ((ct as CheckBox).Checked)
                        {
                            if (type == typeof(string))
                            {
                                strText[i] = "Y";
                            }
                            else
                            {
                                strText[i] = "1";
                            }
                        }
                        else
                        {
                            if (type == typeof(string))
                            {
                                strText[i] = "N";
                            }
                            else
                            {
                                strText[i] = "0";
                            }
                        }
                        queryText[i] = (ct as CheckBox).Checked.ToString();
                    }
                    else if (ct != null && columntype == "ClientQueryRefValColumn")
                    {
                        strText[i] = (ct as WebRefVal).BindingValue.ToString();
                        queryText[i] = strText[i];
                    }
                    else if (ct != null && columntype == "ClientQueryCalendarColumn")
                    {
                        if (type == typeof(string))
                        {
                            if ((ct as WebDateTimePicker).Text != string.Empty)
                            {
                                queryText[i] = (ct as WebDateTimePicker).Text;
                                try
                                {
                                    DateTime dttext = DateTime.Parse((ct as WebDateTimePicker).Text);
                                    strText[i] = dttext.Year.ToString("0000")
                                    + dttext.Month.ToString("00")
                                    + dttext.Day.ToString("00");
                                }
                                catch
                                {
                                    strText[i] = "";
                                }
                            }
                        }
                        else
                        {
                            strText[i] = (ct as WebDateTimePicker).Text;
                            queryText[i] = strText[i];
                        }
                    }
                    querytext += queryText[i] + ";";
                    ((WebAnyQueryColumns)this.Columns[i]).Text = queryText[i];
                }

                if (querytext != string.Empty)
                {
                    querytext = querytext.Substring(0, querytext.LastIndexOf(';'));
                }


                string sqlcmd = DBUtils.GetCommandText(wds);

                string strQueryCondition = "";
                string strCondition = "";
                string strOperator = "";

                for (int i = 0; i < columnNum; i++)
                {
                    strCondition = ((WebAnyQueryColumns)this.Columns[i]).Condition;
                    strOperator = ((WebAnyQueryColumns)this.Columns[i]).Operator;
                    if (strQueryCondition == "")
                    {
                        strCondition = "";
                    }
                    if (strText[i] != string.Empty)
                    {
                        strText[i] = strText[i].Replace("'", "''");
                        string columnname = CliUtils.GetTableNameForColumn(sqlcmd, ((WebAnyQueryColumns)this.Columns[i]).Column);
                        Type datatype = wds.InnerDataSet.Tables[wds.DataMember].Columns[((WebAnyQueryColumns)this.Columns[i]).Column].DataType;
                        string valuequote = (datatype == typeof(string) || datatype == typeof(char) || datatype == typeof(Guid))
                            ? "'" : string.Empty;
                        if (string.Compare(strText[i].Trim(), "null", true) == 0)//IgnoreCase
                        {
                            strQueryCondition += " " + strCondition + columnname + "is null";
                            if (valuequote.Length > 0)
                            {
                                strQueryCondition += " or " + columnname + "='')";
                            }
                        }
                        else
                        {
                            if (strOperator == "in")
                            {
                                string[] liststring = strText[i].Split(',');
                                strQueryCondition += " " + strCondition + columnname + " in (";
                                for (int j = 0; j < liststring.Length; j++)
                                {
                                    if (j > 0)
                                    {
                                        strQueryCondition += ",";
                                    }
                                    strQueryCondition += valuequote + liststring[j] + valuequote;
                                }
                                strQueryCondition += ")";
                            }
                            else if (strOperator == "%")
                            {
                                strQueryCondition += " " + strCondition + columnname + "like '" + strText[i] + "%'";
                            }
                            else if (strOperator == "%%")
                            {
                                strQueryCondition += " " + strCondition + columnname + "like '%" + strText[i] + "%'";
                            }
                            else
                            {
                                if (datatype == typeof(DateTime))
                                {
                                    int DBType = getDBType();
                                    if (DBType == 1)
                                        strQueryCondition += " " + strCondition + columnname + strOperator + " '" + strText[i] + "'";
                                    else if (DBType == 2)
                                        strQueryCondition += " " + strCondition + columnname + strOperator + " '" + strText[i] + "'";
                                    else if (DBType == 3)
                                    {
                                        string Date = changeDate(strText[i]);
                                        strQueryCondition += " " + strCondition + columnname + strOperator + " to_Date('" + Date + "', 'yyyymmdd')";
                                    }
                                    else if (DBType == 4)
                                        strQueryCondition += " " + strCondition + columnname + strOperator + " '" + strText[i] + "'";
                                    else if (DBType == 5)
                                    {
                                        DateTime dt = (DateTime)Convert.ChangeType(strText[i], typeof(DateTime));
                                        strQueryCondition += " " + strCondition + columnname + strOperator + " " + string.Format("'{0}-{1}-{2}'", dt.Year, dt.Month, dt.Day) + "";
                                    }
                                    else if (DBType == 6)
                                    {
                                        DateTime dt = (DateTime)Convert.ChangeType(strText[i], typeof(DateTime));
                                        String strwhere = string.Format("to_date('{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}', '%Y%m%d%H%M%S')", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
                                        strQueryCondition += " " + strCondition + columnname + strOperator + " " + strwhere;
                                    }
                                    else if (DBType == 7)
                                        strQueryCondition += " " + strCondition + columnname + strOperator + " '" + strText[i] + "'";
                                }
                                else
                                {
                                    strQueryCondition += " " + strCondition + columnname + strOperator + valuequote + strText[i] + valuequote;
                                }
                            }

                        }

                    }
                }
                return strQueryCondition;
            }
            else
            {
                throw new Exception(string.Format("Please show QueryText in panel first!"));
            }
        }

        private Control GetControlByID(string strid, Control ct)
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
                        Control ctrtn = GetControlByID(strid, ctchild);
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

        private object GetDefaultValue(string Default)
        {
            return CliUtils.GetValue(Default, this.Page);
        }

        private int getDBType()
        {
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB });
            string type = "";
            if (myRet != null && (int)myRet[0] == 0)
                type = myRet[1].ToString();
            switch (type)
            {
                case "1": return 1;
                case "2": return 2;
                case "3": return 3;
                case "4": return 4;
                case "5": return 5;
                case "6": return 6;
            }
            return 0;
        }

        private string changeDate(string str)
        {
            char[] mark = { '-', '/' };
            string[] temp = str.Split(mark);
            string Date = "";
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].Length < 2)
                    temp[i] = '0' + temp[i];
                Date += temp[i];
            }
            return Date;
        }

    }

    public class WebAnyQueryColumnsCollection : InfoOwnerCollection
    {
        public WebAnyQueryColumnsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebAnyQueryColumns))
        {

        }

        public DataSet DsForDD = new DataSet();
        public new WebAnyQueryColumns this[int index]
        {
            get
            {
                return (WebAnyQueryColumns)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebAnyQueryColumns)
                    {
                        //原来的Collection设置为0
                        ((WebAnyQueryColumns)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebAnyQueryColumns)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class WebAnyQueryColumns : InfoOwnerCollectionItem, IGetValues
    {
        #region Constructor

        public WebAnyQueryColumns()
            : this("webanyquery", "", "", 120)
        {

        }

        public WebAnyQueryColumns(string name, string column, string caption, int width)
        {
            _name = name;
            _Column = column;
            _caption = caption;
            _width = width;
            _columntype = "AnyQueryTextBoxColumn";
            _condition = "And";
            _operator = "=";
            _defaultvalue = "";
            _text = "";
        }

        public WebAnyQueryColumns(string name, string column, string caption, int width, String columntype, String oprator)
        {
            _name = name;
            _Column = column;
            _caption = caption;
            _width = width;
            _columntype = columntype;
            _condition = "And";
            _operator = oprator;
            _defaultvalue = "";
            _text = "";
        }

        #endregion

        #region Properties

        private string _name;
        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _Column;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Column
        {
            get
            {
                return _Column;
            }
            set
            {
                _Column = value;
                if (this.Owner != null)
                {
                    if (((WebAnyQuery)this.Owner).Site == null)
                    {
                        this.Caption = GetDDText(_Column);
                    }
                    else if (((WebAnyQuery)this.Owner).Site.DesignMode)
                    {
                        this.Caption = GetDDText(_Column);
                    }
                }
            }
        }

        private string _columntype;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("AnyQueryTextBoxColumn")]
        public string ColumnType
        {
            get
            {
                return _columntype;
            }
            set
            {
                _columntype = value;

                if (_columntype != "AnyQueryComboBoxColumn" && _columntype != "AnyQueryRefValColumn")
                {
                    _webrefval = null;
                }
            }
        }

        private string _caption;
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                if (value != null && value != "")
                {
                    _caption = value;
                    _name = _caption;
                }
                else
                {
                    if (_Column != null && _Column != "")
                    {
                        _caption = _Column;
                        _name = _Column;
                    }
                    else
                    {
                        _name = "clientquery";
                    }
                }
            }
        }

        private string _operator;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("=")]
        public string Operator
        {
            get
            {
                return _operator;
            }
            set
            {
                _operator = value;
            }
        }

        private string _condition;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("And")]
        public string Condition
        {
            get
            {
                return _condition;
            }
            set
            {
                _condition = value;
            }
        }

        private string _webrefval;
        [Editor(typeof(Srvtools.WebQueryColumns.QueryColumnEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        public string WebRefVal
        {
            get
            {
                return _webrefval;
            }
            set
            {
                if (_columntype != "AnyQueryComboBoxColumn" && _columntype != "AnyQueryRefValColumn" && value != null)
                {
                    //MessageBox.Show("WebRefval can be set only when\ncolumntype is combobox & refval.");
                }
                else
                {
                    _webrefval = value;
                }
            }
        }
        private string _webrefbutton;
        [Editor(typeof(Srvtools.WebQueryColumns.QueryColumnEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        public string WebRefButton
        {
            get
            {
                return _webrefbutton;
            }
            set
            {
                if (_columntype != "AnyQueryRefButtonColumn" && value != null)
                {
                    //MessageBox.Show("WebRefValButton can be set only when\ncolumntype is refbutton.");
                }
                else
                {
                    _webrefbutton = value;
                }
            }
        }

        private string _defaultvalue;
        public string DefaultValue
        {
            get
            {
                return _defaultvalue;
            }
            set
            {
                _defaultvalue = value;
            }
        }

        private string _text;
        [Browsable(false)]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        private int _width;
        [DefaultValue(120)]
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        private bool _dateConver;
        [Browsable(false)]
        [DefaultValue(false)]
        public bool DateConver
        {
            get
            {
                return _dateConver;
            }
            set
            {
                _dateConver = value;
            }
        }

        private String[] _items;
        [Browsable(false)]
        public String[] Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }

        private bool _enabled = true;
        [DefaultValue(true)]
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        private bool _autoSelect = false;
        [DefaultValue(false)]
        public bool AutoSelect
        {
            get
            {
                return _autoSelect;
            }
            set
            {
                _autoSelect = value;
            }
        }
        #endregion

        #region method

        private string GetDDText(string fieldName)
        {
            DataSet Dset = ((WebAnyQueryColumnsCollection)this.Collection).DsForDD;
            string strCaption = "";

            if (Dset.Tables.Count == 0)
            {
                WebAnyQuery waq = (WebAnyQuery)this.Owner;
                object obj = waq.GetObjByID(waq.DataSourceID);
                if (obj is WebDataSource)
                {
                    ((WebAnyQueryColumnsCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(obj as WebDataSource, true);
                    Dset = ((WebAnyQueryColumnsCollection)this.Collection).DsForDD;
                }
            }

            if (Dset.Tables.Count > 0)
            {
                int x = Dset.Tables[0].Rows.Count;
                for (int y = 0; y < x; y++)
                {
                    if (string.Compare(Dset.Tables[0].Rows[y]["FIELD_NAME"].ToString(), fieldName, true) == 0)//IgnoreCase
                    {
                        strCaption = Dset.Tables[0].Rows[y]["CAPTION"].ToString();
                    }
                }
            }
            return strCaption;
        }

        public void setcolumn(string colname)
        {
            this._Column = colname;

        }

        #endregion


        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is WebAnyQuery)
            {
                if (sKind.ToLower().Equals("operator"))
                {
                    values.Add("=");
                    values.Add("!=");
                    values.Add(">");
                    values.Add("<");
                    values.Add(">=");
                    values.Add("<=");
                    values.Add("%**");
                    values.Add("**%");
                    values.Add("%%");
                    values.Add("!%%");
                    values.Add("<->");
                    values.Add("!<->");
                    values.Add("IN");
                    values.Add("NOT IN");
                }
                else if (sKind.ToLower().Equals("columntype"))
                {
                    values.Add("AnyQueryTextBoxColumn");
                    values.Add("AnyQueryComboBoxColumn");
                    values.Add("AnyQueryCheckBoxColumn");
                    values.Add("AnyQueryRefValColumn");
                    values.Add("AnyQueryCalendarColumn");
                    values.Add("AnyQueryRefButtonColumn");
                }
                else if (sKind.ToLower().Equals("column"))
                {
                    if (this.Owner is WebAnyQuery)
                    {
                        WebAnyQuery waq = (WebAnyQuery)this.Owner;
                        if (waq.Page != null && waq.DataSourceID != null && waq.DataSourceID != "")
                        {
                            foreach (Control ctrl in waq.Page.Controls)
                            {
                                if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == waq.DataSourceID)
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
                                    if (ds.DesignDataSet != null)
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
                else if (string.Compare(sKind, "webrefval", true) == 0)//IgnoreCase
                {
                    if (this.Owner is WebAnyQuery)
                    {
                        WebAnyQuery waq = (WebAnyQuery)this.Owner;
                        foreach (Control ct in waq.Page.Controls)
                        {
                            if (ct is WebRefVal)
                            {
                                values.Add(ct.ID);
                            }
                        }
                        if (waq.Page.Form != null)
                        {
                            foreach (Control ct in waq.Page.Form.Controls)
                            {
                                if (ct is WebRefVal)
                                {
                                    values.Add(ct.ID);
                                }
                            }
                        }
                    }
                }
                else if (string.Compare(sKind, "webrefbutton", true) == 0)//IgnoreCase
                {
                    if (this.Owner is WebAnyQuery)
                    {
                        WebAnyQuery waq = (WebAnyQuery)this.Owner;
                        foreach (Control ct in waq.Page.Controls)
                        {
                            if (ct is WebRefButton)
                            {
                                values.Add(ct.ID);
                            }
                        }
                        if (waq.Page.Form != null)
                        {
                            foreach (Control ct in waq.Page.Form.Controls)
                            {
                                if (ct is WebRefButton)
                                {
                                    values.Add(ct.ID);
                                }
                            }
                        }
                    }
                }
                else if (string.Compare(sKind, "condition", true) == 0)//IgnoreCase
                {
                    values.Add("And");
                    values.Add("Or");
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

        #endregion
    }

    public class wanyqueryDataSourceEditor : UITypeEditor
    {
        public wanyqueryDataSourceEditor()
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
            if (context.Instance is WebAnyQuery)
            {
                ControlCollection ctrlList = ((WebAnyQuery)context.Instance).Page.Controls;
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
}
