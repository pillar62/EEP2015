using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Web.UI.Design.WebControls;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Web.UI.Design;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using System.Resources;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Srvtools
{
    [Designer(typeof(WebClientQueryEditor), typeof(IDesigner))]
    [ToolboxBitmap(typeof(WebClientQuery), "Resources.WebClientQuery.ICO")]
    public class WebClientQuery : WebControl
    {
        public WebClientQuery()
        {
            _column = new WebQueryColumnsCollection(this, typeof(QueryColumns));
            _gaphorizontal = 10;
            _gapvertical = 10;
            _keepcondition = false;
            this.Font.Name = "Simsun";
            this.Font.Size = FontUnit.Medium;
            _font = this.Font;

            _forecolor = SystemColors.ControlText;
            _textcolor = SystemColors.ControlText;
            _bordercolor = Color.Black;
            _innertable = false;
        }

        #region Properties

        [Category("Infolight"),
        Description("The ID of WebDataSource which the control is bound to")]
        [Editor(typeof(wcqDataSourceEditor), typeof(UITypeEditor))]
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

        private int _gaphorizontal;
        [Category("Infolight"),
        Description("Specifies horizontal distance between the controls WebClientQuery created")]
        public int GapHorizontal
        {
            get
            {
                return _gaphorizontal;
            }
            set
            {
                _gaphorizontal = value;
            }
        }

        private int _gapvertical;
        [Category("Infolight"),
        Description("Specifies vertical distance between the controls WebClientQuery created")]
        public int GapVertical
        {
            get
            {
                return _gapvertical;
            }
            set
            {
                _gapvertical = value;
            }
        }

        private WebQueryColumnsCollection _column;
        [Category("Infolight"),
        Description("The columns which WebClientQuery is applied to")]
        [PersistenceMode(PersistenceMode.InnerProperty),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         TypeConverter(typeof(CollectionConverter)),
         NotifyParentProperty(true)]
        public WebQueryColumnsCollection Columns
        {
            get
            {
                return _column;
            }
        }

        private bool _keepcondition;
        [Category("Infolight"),
        Description("Indicates whether the text will be cleared after excute query")]
        public bool KeepCondition
        {
            get
            {
                return _keepcondition;
            }
            set
            {
                _keepcondition = value;
            }
        }

        [Category("Infolight")]
        private DateFormatString _dateformatstring = DateFormatString.yyyyMMdd;
        public DateFormatString DateFormatString
        {
            get
            {
                return _dateformatstring;
            }
            set
            {
                _dateformatstring = value;
            }
        }


        private FontInfo _font;
        [Category("Infolight"),
        Description("The font used for text in the controls which ClientQuery creates")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public FontInfo TextFont
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
            }

        }

        private Color _forecolor;
        [Category("Infolight"),
        Description("The color of the label's text which ClientQuery creates")]
        public Color LabelColor
        {
            get
            {
                return _forecolor;
            }
            set
            {
                _forecolor = value;
            }
        }

        private Color _textcolor;
        [Category("Infolight"),
        Description("The color of the text which users input")]
        public Color TextColor
        {
            get
            {
                return _textcolor;
            }
            set
            {
                _textcolor = value;
            }
        }

        private Color _bordercolor;
        [Category("Infolight"),
        Description("The color of the border of table")]
        public new Color BorderColor
        {
            get
            {
                return _bordercolor;
            }
            set
            {
                _bordercolor = value;
            }
        }

        private BorderStyle _borderstyle;
        [Category("Infolight"),
        Description("The style of the border of table")]
        public new BorderStyle BorderStyle
        {
            get
            {
                return _borderstyle;
            }
            set
            {
                _borderstyle = value;
            }
        }

        private bool _innertable;
        [Category("Infolight"),
        Description("Indicates whether the border of table is shown")]
        public bool InnerTable
        {
            get
            {
                return _innertable;
            }
            set
            {
                _innertable = value;
            }
        }

        private String _panelID;
        [Category("Infolight")]
        public String PanelID
        {
            get
            {
                return _panelID;
            }
            set
            {
                _panelID = value;
            }
        }

        //private ArrayList isShow
        //{
        //    get { return ViewState["isShow"] == null ? new ArrayList() : (ArrayList)ViewState["isShow"]; }
        //    set { ViewState["isShow"] = value; }
        //}
        private ArrayList isShow = new ArrayList();

        public ArrayList Get_isShow()
        {
            return isShow;
        }

        #endregion


        protected override void OnLoad(EventArgs e)
        {
            if (this.Page.Request.QueryString != null && this.Page.Request.QueryString["ClientQueryID"] == this.ID)
            {
                if (this.Page.Request.QueryString != null && this.Page.Request.QueryString["Querytext"] != null
                    && this.Page.Request.QueryString["PanelID"] != null)
                {
                    string strQueryText = Page.Request.QueryString["Querytext"];
                    string[] arrQueryText = strQueryText.Split(';');
                    int columnNum = arrQueryText.Length;
                    string strPanelID = Page.Request.QueryString["PanelID"];
                    Panel pn = (Panel)GetObjByID(strPanelID);

                    for (int i = 0; i < columnNum; i++)
                    {
                        string strControlName = "txt" + i.ToString();
                        Control ct = this.GetControlByID(strControlName, pn);

                        if (ct is TextBox)
                        {
                            (ct as TextBox).Text = arrQueryText[i];
                        }
                        else if (ct is WebDropDownList)
                        {
                            if ((ct as WebDropDownList).DataSourceID != this.DataSourceID)
                            {
                                (ct as WebDropDownList).SelectedValue = arrQueryText[i];
                            }
                        }
                        else if (ct is WebRefVal)
                        {
                            if ((ct as WebRefVal).DataSourceID != this.DataSourceID)
                            {
                                (ct as WebRefVal).BindingValue = arrQueryText[i];
                            }
                        }
                        else if (ct is WebDateTimePicker)
                        {
                            (ct as WebDateTimePicker).Text = arrQueryText[i];
                        }

                    }
                }
                else if (this.Page.Request.QueryString != null && this.Page.Request.QueryString["Querytext"] != null)
                {
                    string strQueryText = Page.Request.QueryString["Querytext"];
                    string[] arrQueryText = strQueryText.Split(';');
                    for (int i = 0; i < this.Columns.Count; i++)
                    {
                        ((WebQueryColumns)this.Columns[i]).Text = arrQueryText[i];
                    }

                }
            }
        }

        #region method

        private object GetDefaultValue(string Default)
        {
            return CliUtils.GetValue(Default, this.Page);
        }

        private string AddParam()
        {
            WebDataSource wds = new WebDataSource();
            wds = (WebDataSource)GetObjByID(this.DataSourceID);

            if (this.Columns.Count == 0)
            {
                throw new Exception("no columns in ClientQuery");
            }

            string url = "Caption=";
            foreach (WebQueryColumns qc in this.Columns)
            {
                url += HttpUtility.UrlEncode(qc.Caption) + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Column=";
            foreach (WebQueryColumns qc in this.Columns)
            {
                if (qc.Column == string.Empty)
                {
                    throw new Exception(string.Format("The columnname of column[{0}] is empty", this.Columns.GetItemIndex(qc).ToString()));
                }
                url += qc.Column + ";";
            }

            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Condition=";
            foreach (WebQueryColumns qc in this.Columns)
            {
                url += qc.Condition + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Operator=";
            foreach (WebQueryColumns qc in this.Columns)
            {
                url += qc.Operator + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Columntype=";
            foreach (WebQueryColumns qc in this.Columns)
            {
                url += qc.ColumnType + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Newline=";
            foreach (WebQueryColumns qc in this.Columns)
            {
                url += qc.NewLine.ToString() + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Textwidth=";
            foreach (WebQueryColumns qc in this.Columns)
            {
                if (qc.Visible)
                {
                    url += qc.Width.ToString() + ";";
                }
                else
                {
                    url += "0;";
                }
            }
            url = url.Substring(0, url.LastIndexOf(';'));
            url += "&Textalign=";
            foreach (WebQueryColumns qc in this.Columns)
            {
                url += qc.TextAlign.ToString() + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));


            url += "&Text=";
            foreach (WebQueryColumns qc in this.Columns)
            {
                url += qc.Text + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));

            url += "&Defaultvalue=";
            foreach (WebQueryColumns qc in this.Columns)
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

            url += "&IsNvarchars=";
            foreach (WebQueryColumns qc in this.Columns)
            {
                url += qc.IsNvarChar + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));


            #region refval
            url += "&Refvalvf=";
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
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
                            url += wrc.ColumnName + "," + wrc.HeadText + "," + wrc.Width.ToString() + "," + wrc.IsNvarChar + ":";

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
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
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
            foreach (WebQueryColumns qc in this.Columns)
            {
                string strtype = wds.InnerDataSet.Tables[wds.DataMember].Columns[qc.Column].DataType.ToString();
                url += strtype + ";";
            }
            url = url.Substring(0, url.LastIndexOf(';'));

            url += "&Textfont=" + this.TextFont.Name + ";" + this.TextFont.Size.ToString() + ";" + this.TextFont.Bold.ToString()
            + ";" + this.TextFont.Italic.ToString() + ";" + this.TextFont.Overline.ToString() + ";" + this.TextFont.Strikeout.ToString()
            + ";" + this.TextFont.Underline.ToString();

            url += "&Keepcondition=" + this.KeepCondition.ToString();
            url += "&DateFormatString=" + this.DateFormatString.ToString();
            url += "&Labelcolor=" + this.LabelColor.Name;
            url += "&Textcolor=" + this.TextColor.Name;
            url += "&Gaphorizontal=" + this.GapHorizontal.ToString();
            url += "&Gapvertical=" + this.GapVertical.ToString();
            url += "&Pagepath=" + this.Page.Request.FilePath;
            url += "&Psypagepath=" + HttpUtility.UrlEncode(this.Page.Request.PhysicalPath);
            url += "&DataSourceID=" + this.DataSourceID;
            url += "&RemoteName=" + wds.RemoteName;
            url += "&ClientQueryID=" + this.ID;
            string itemparam = this.Page.Request.QueryString["ItemParam"] != null ? HttpUtility.UrlEncode(this.Page.Request.QueryString["ItemParam"]) : string.Empty;
            url += "&ItemParam=" + itemparam;
            if (this.Page.Request.QueryString["MatchControls"] != null && this.Page.Request.QueryString["MatchControls"] != "")
            {
                url += "&MatchControls=" + this.Page.Request.QueryString["MatchControls"];
            }
            return url;

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

        public void Execute()
        {
            Panel p = null;
            if (!String.IsNullOrEmpty(this.PanelID))
            {
                p = this.Page.FindControl(this.PanelID) as Panel;
            }
            if (p == null)
            {
                Execute(false);
            }
            else
            {
                Execute(p);
            }
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
                url = "../InnerPages/frmClientQuery.aspx?" + sessionRequest.SetRequestValue(url);
            }
            else
            {
                url = "../InnerPages/frmClientQuery.aspx?" + QueryStringEncrypt.Encrypt(url);
            }
            if (dialog)
            {
                string script = string.Format("<script>window.open('{0}','query','left=200,top=200,width=600,scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no')</script>", url);
                Page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script);
            }
            else
            {
                Page.Response.Redirect(url);
            }
        }

        public void Execute(Panel pn)
        {
            this.Execute(pn, true);
        }

        public void Execute(Panel pn, bool noredirect)
        {
            var datasource = GetObjByID(this.DataSourceID);
            if (datasource.GetType().FullName == "Srvtools.WebDataSource")
            {
                string wherestring = GetWhere(pn);
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    WebQueryColumns column = this.Columns[i] as WebQueryColumns;
                    if (column.NotNull && string.IsNullOrEmpty(column.Text))
                    {
                        String message = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                               "Srvtools",
                                                                               "WebValidate",
                                                                               "msg_WebValidateCheckNull", true);
                        string script = string.Format("alert('" + message + "');", column.Caption);

                        this.Page.ClientScript.RegisterStartupScript(typeof(string), "", script, true);
                        return;
                    }
                }
                if (noredirect)
                {
                    WebDataSource wds = (WebDataSource)GetObjByID(this.DataSourceID);
                    wds.SetWhere(wherestring);
                }
                else
                {
                    this.Page.Response.Redirect(this.Page.Request.FilePath + "?Filter=" + HttpUtility.UrlEncode(wherestring) + "&DataSourceID=" + this.DataSourceID
                      + "&QueryText=" + querytext + "&PanelID=" + pn.ID + "&IsQueryBack=1" + "&ClientQueryID=" + this.ID);
                }
            }
            else if (datasource.GetType().FullName == "EFClientTools.WebDataSource")
            {
                List<string> columns = new List<string>();
                List<string> conditions = new List<string>();
                List<object> values = new List<object>();
                int columnNum = this.Columns.Count;
                string[] strText = new string[columnNum];
                string[] queryText = new string[columnNum];

                for (int i = 0; i < columnNum; i++)
                {
                    string strControlName = "txt" + i.ToString();
                    Control ct = this.GetControlByID(strControlName, pn);

                    string columntype = ((WebQueryColumns)this.Columns[i]).ColumnType;
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
                        //if ((ct as CheckBox).Checked)
                        //{
                        //    if (type == typeof(string))
                        //    {
                        //        strText[i] = "Y";
                        //    }
                        //    else
                        //    {
                        //        strText[i] = "1";
                        //    }
                        //}
                        //else
                        //{
                        //    if (type == typeof(string))
                        //    {
                        //        strText[i] = "N";
                        //    }
                        //    else
                        //    {
                        //        strText[i] = "0";
                        //    }
                        //}
                        //queryText[i] = (ct as CheckBox).Checked.ToString();
                    }
                    else if (ct != null && columntype == "ClientQueryRefValColumn")
                    {
                        strText[i] = (ct as WebRefVal).BindingValue.ToString();
                        queryText[i] = strText[i];
                    }
                    else if (ct != null && columntype == "ClientQueryCalendarColumn")
                    {
                        //if (type == typeof(string))
                        //{
                        //    if ((ct as WebDateTimePicker).Text != string.Empty)
                        //    {
                        //        queryText[i] = (ct as WebDateTimePicker).Text;
                        //        try
                        //        {
                        //            DateTime dttext = DateTime.Parse((ct as WebDateTimePicker).Text);
                        //            strText[i] = dttext.Year.ToString("0000")
                        //            + dttext.Month.ToString("00")
                        //            + dttext.Day.ToString("00");
                        //        }
                        //        catch
                        //        {
                        //            strText[i] = "";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    strText[i] = (ct as WebDateTimePicker).Text;
                        //    queryText[i] = strText[i];
                        //}
                    }

                    columns.Add(this.Columns[i].Column);
                    conditions.Add(this.Columns[i].Condition);
                    values.Add(strText[i]);
                }
                //datasource.GetTypeof
                //object list = listType

                var miSetWhere = datasource.GetType().GetMethod("SetWhere", new Type[] { typeof(List<string>), typeof(List<string>), typeof(List<object>) });
                miSetWhere.Invoke(datasource, new object[] { columns, conditions, values });
                this.Page.DataBind();
            }
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
                    Type type = wds.InnerDataSet.Tables[wds.DataMember].Columns[((WebQueryColumns)this.Columns[i]).Column].DataType;
                    string columntype = ((WebQueryColumns)this.Columns[i]).ColumnType;

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
                    ((WebQueryColumns)this.Columns[i]).Text = queryText[i];
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
                    strCondition = ((WebQueryColumns)this.Columns[i]).Condition;
                    strOperator = ((WebQueryColumns)this.Columns[i]).Operator;
                    if (strQueryCondition == "")
                    {
                        strCondition = "";
                    }
                    if (strText[i] != string.Empty)
                    {
                        strText[i] = strText[i].Replace("'", "''");
                        string columnname = CliUtils.GetTableNameForColumn(sqlcmd, ((WebQueryColumns)this.Columns[i]).Column);
                        Type datatype = wds.InnerDataSet.Tables[wds.DataMember].Columns[((WebQueryColumns)this.Columns[i]).Column].DataType;
                        string valuequote = (datatype == typeof(string) || datatype == typeof(char) || datatype == typeof(Guid))
                            ? "'" : string.Empty;
                        string nvarCharMark = datatype == typeof(string) && ((WebQueryColumns)this.Columns[i]).IsNvarChar ? "N" : string.Empty;
                        if (string.Compare(strText[i].Trim(), "null", true) == 0)//IgnoreCase
                        {
                            strQueryCondition += " " + strCondition;
                            if (valuequote.Length > 0)
                            {
                                strQueryCondition += "(";
                            }
                            if (strOperator.Equals("!="))
                            {
                                strQueryCondition += columnname + "is not null";
                            }
                            else
                            {
                                strQueryCondition += columnname + "is null";
                            }
                            if (valuequote.Length > 0)
                            {
                                if (strOperator.Equals("!="))
                                {
                                    strQueryCondition += " and " + columnname + "<>'')";
                                }
                                else
                                {
                                    strQueryCondition += " or " + columnname + "='')";
                                }
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
                                    strQueryCondition += nvarCharMark + valuequote + liststring[j] + valuequote;
                                }
                                strQueryCondition += ")";
                            }
                            else if (strOperator == "%")
                            {
                                strQueryCondition += " " + strCondition + columnname + "like " + nvarCharMark + "'" + strText[i] + "%'";
                            }
                            else if (strOperator == "%%")
                            {
                                strQueryCondition += " " + strCondition + columnname + "like " + nvarCharMark + "'%" + strText[i] + "%'";
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
                                    {
                                        string Date = changeDate(strText[i]);
                                        strQueryCondition += " " + strCondition + columnname + strOperator + " to_Date('" + Date + "', '%Y%m%d')";
                                    }
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
                                    strQueryCondition += " " + strCondition + columnname + strOperator + nvarCharMark + valuequote + strText[i] + valuequote;
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

        public string GetWhereText(Panel pn)
        {
            GetWhere(pn);
            return QueryTranslate.Translate(this);
        }

        public string GetWhereText()
        {
            return QueryTranslate.Translate(this);
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

        private string WhereItemToFileter(WebRefVal wrf) //这个....
        {
            WebDataSource wds = GetObjByID(wrf.DataSourceID) as WebDataSource;
            string sqlcmd = DBUtils.GetCommandText(wds);
            string filter = "";
            foreach (WebWhereItem wi in wrf.WhereItem)
            {
                string strOperator = wi.Condition;

                if (filter != "")
                {
                    filter += " and ";
                }
                string type = wds.InnerDataSet.Tables[wds.DataMember].Columns[wi.FieldName].DataType.ToString().ToLower();
                if (strOperator != "%" && strOperator != "%%")
                {
                    if (type == "system.uint" || type == "system.uint16" || type == "system.uint32"
                       || type == "system.int64" || type == "system.int" || type == "system.int16"
                       || type == "system.int32" || type == "system.uint64" || type == "system.single"
                       || type == "system.double" || type == "system.decimal")
                    {
                        filter += CliUtils.GetTableNameForColumn(sqlcmd, wi.FieldName) + strOperator + " " + wi.Value;
                    }
                    else
                    {
                        filter += CliUtils.GetTableNameForColumn(sqlcmd, wi.FieldName) + strOperator + " '" + wi.Value + "'";
                    }
                }
                else
                {
                    if (strOperator == "%")
                    {
                        filter += CliUtils.GetTableNameForColumn(sqlcmd, wi.FieldName) + "like '" + wi.Value + "%'";
                    }
                    if (strOperator == "%%")
                    {
                        filter += CliUtils.GetTableNameForColumn(sqlcmd, wi.FieldName) + "like '%" + wi.Value + "%'";
                    }
                }
            }
            return filter;
        }

        public void Show()
        {
            Panel p = null;
            if (!String.IsNullOrEmpty(this.PanelID))
            {
                p = this.Page.FindControl(this.PanelID) as Panel;
            }
            if (p == null)
            {

            }
            else
            {
                Show(p);
            }
        }

        public void Show(Panel pn)
        {
            Label[] labels;
            TextBox[] textBoxes;
            CheckBox[] checkBoxes;
            WebDropDownList[] dropDownlists;
            WebRefVal[] webRefVals;
            WebDateTimePicker[] webDateTimePickers;
            WebRefButton[] webRefButtons;

            Table table = new Table();
            table.CellSpacing = 0;
            if (this.InnerTable)
            {
                table.CellSpacing = 1;
                table.BackColor = this.BorderColor;
                table.BorderWidth = 1;
                table.BorderColor = this.BorderColor;
                table.BorderStyle = BorderStyle.Solid;
            }

            //WebDataSource wds = new WebDataSource();
            //wds = (WebDataSource)GetObjByID(this.DataSourceID);

            if (!this.isShow.Contains(pn.ID))
            {
                int columnNum = this.Columns.Count;
                if (columnNum == 0)
                {
                    return;
                }
                labels = new Label[columnNum];
                dropDownlists = new WebDropDownList[columnNum];
                textBoxes = new TextBox[columnNum];
                checkBoxes = new CheckBox[columnNum];
                webRefVals = new WebRefVal[columnNum];
                webDateTimePickers = new WebDateTimePicker[columnNum];
                webRefButtons = new WebRefButton[columnNum];

                pn.ScrollBars = ScrollBars.Auto;

                TableRow[] trQuery = new TableRow[columnNum];
                int intRowCount = 0;
                trQuery[0] = new TableRow();
                //trQuery[0].BackColor = pn.BackColor;

                for (int i = 0; i < columnNum; i++)
                {
                    if (((WebQueryColumns)this.Columns[i]).Column == string.Empty)
                    {
                        throw new Exception(string.Format("The columnname of column[{0}] is empty", i.ToString()));
                    }

                    TableCell tcLabel = new TableCell();
                    TableCell tcQueryText = new TableCell();
                    if (this.InnerTable)
                    {
                        if (!pn.BackColor.IsEmpty)
                        {
                            tcLabel.BackColor = pn.BackColor;
                            tcQueryText.BackColor = pn.BackColor;
                        }
                        else
                        {
                            tcLabel.BackColor = Color.White;
                            tcQueryText.BackColor = Color.White;
                        }

                    }
                    tcLabel.HorizontalAlign = HorizontalAlign.Right;
                    tcLabel.VerticalAlign = VerticalAlign.Middle;
                    tcLabel.Wrap = false;
                    tcQueryText.HorizontalAlign = HorizontalAlign.Left;
                    tcQueryText.VerticalAlign = VerticalAlign.Middle;
                    if (InnerTable)
                    {
                        tcLabel.BorderStyle = this.BorderStyle;
                        tcQueryText.BorderStyle = this.BorderStyle;
                    }

                    //create captionlabel
                    labels[i] = new Label();
                    labels[i].ID = "lbl" + i.ToString();
                    labels[i].Text = ((WebQueryColumns)this.Columns[i]).Caption;
                    labels[i].ForeColor = this.LabelColor;
                    labels[i].Font.CopyFrom(this.TextFont);
                    tcLabel.Controls.Add(labels[i]);

                    switch (((WebQueryColumns)this.Columns[i]).ColumnType)
                    {
                        case "ClientQueryComboBoxColumn":
                            {
                                dropDownlists[i] = new WebDropDownList();
                                dropDownlists[i].ID = "txt" + i.ToString();
                                dropDownlists[i].Width = ((WebQueryColumns)this.Columns[i]).Width;
                                dropDownlists[i].ForeColor = this.TextColor;
                                WebRefVal wrv = (WebRefVal)GetObjByID(((WebQueryColumns)this.Columns[i]).WebRefVal);
                                dropDownlists[i].DataSourceID = wrv.DataSourceID;

                                dropDownlists[i].DataTextField = wrv.DataTextField;
                                dropDownlists[i].DataValueField = wrv.DataValueField;
                                dropDownlists[i].AppendDataBoundItems = true;
                                dropDownlists[i].AutoInsertEmptyData = true;
                                if (wrv.WhereItem.Count > 0)
                                {
                                    dropDownlists[i].Filter = WhereItemToFileter(wrv);
                                    //dropDownlists[i].DataBind();
                                }

                                if (GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue) != null)
                                {
                                    try
                                    {
                                        if (GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue) != null)
                                        {
                                            dropDownlists[i].SelectedValue = GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue).ToString();
                                        }
                                    }
                                    catch
                                    { }
                                }
                                dropDownlists[i].Font.CopyFrom(this.TextFont);
                                tcQueryText.Controls.Add(dropDownlists[i]);
                                break;
                            }
                        case "ClientQueryTextBoxColumn":
                            {
                                textBoxes[i] = new TextBox();
                                textBoxes[i].ID = "txt" + i.ToString();
                                textBoxes[i].Width = ((WebQueryColumns)this.Columns[i]).Width;
                                if (GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue) != null)
                                {
                                    textBoxes[i].Text = GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue).ToString();
                                }
                                textBoxes[i].ForeColor = this.TextColor;
                                if (((WebQueryColumns)this.Columns[i]).NotNull)
                                {
                                    textBoxes[i].BackColor = ((WebQueryColumns)this.Columns[i]).NotNullBackColor;
                                }
                                textBoxes[i].Font.CopyFrom(this.TextFont);
                                tcQueryText.Controls.Add(textBoxes[i]);
                                break;
                            }
                        case "ClientQueryCheckBoxColumn":
                            {
                                checkBoxes[i] = new CheckBox();
                                checkBoxes[i].ID = "txt" + i.ToString();
                                checkBoxes[i].Text = string.Empty;
                                checkBoxes[i].Width = ((WebQueryColumns)this.Columns[i]).Width;
                                object defaultvalue = GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue);
                                if (defaultvalue != null)
                                {
                                    if (string.Compare(defaultvalue.ToString(), "true", true) == 0 || string.Compare(defaultvalue.ToString(), "1") == 0)
                                    {
                                        checkBoxes[i].Checked = true;
                                    }
                                    else
                                    {
                                        checkBoxes[i].Checked = false;
                                    }
                                }
                                else
                                {
                                    checkBoxes[i].Checked = false;
                                }
                                tcQueryText.Controls.Add(checkBoxes[i]);
                                break;
                            }
                        case "ClientQueryRefValColumn":
                            {
                                webRefVals[i] = new WebRefVal();
                                webRefVals[i].ID = "txt" + i.ToString();
                                webRefVals[i].Width = ((WebQueryColumns)this.Columns[i]).Width;
                                //       webRefVals[i].ForeColor = this.TextColor;
                                webRefVals[i].Font.CopyFrom(this.TextFont);

                                WebRefVal wrv = (WebRefVal)GetObjByID(((WebQueryColumns)this.Columns[i]).WebRefVal);
                                webRefVals[i].DataSourceID = wrv.DataSourceID;
                                webRefVals[i].MultiLanguage = wrv.MultiLanguage;
                                webRefVals[i].DataTextField = wrv.DataTextField;
                                webRefVals[i].DataValueField = wrv.DataValueField;
                                webRefVals[i].CheckData = wrv.CheckData;
                                webRefVals[i].OpenRefHeight = wrv.OpenRefHeight;
                                webRefVals[i].OpenRefLeft = wrv.OpenRefLeft;
                                webRefVals[i].OpenRefTop = wrv.OpenRefTop;
                                webRefVals[i].OpenRefWidth = wrv.OpenRefWidth;
                                webRefVals[i].PostBackButonClick = wrv.PostBackButonClick;
                                webRefVals[i].UpdatePanelID = wrv.UpdatePanelID;
                                webRefVals[i].SessionMode = true;

                                foreach (WebColumnMatch wcm in wrv.ColumnMatch)
                                {
                                    webRefVals[i].ColumnMatch.Add(wcm);
                                }
                                foreach (WebRefColumn wrc in wrv.Columns)
                                {
                                    webRefVals[i].Columns.Add(wrc);
                                }
                                foreach (WebWhereItem wwi in wrv.WhereItem)
                                {
                                    webRefVals[i].WhereItem.Add(wwi);
                                }
                                if (GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue) != null)
                                {
                                    try
                                    {
                                        if (GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue) != null)
                                        {
                                            webRefVals[i].BindingValue = GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue).ToString();
                                        }
                                    }
                                    catch
                                    { }
                                }

                                tcQueryText.Controls.Add(webRefVals[i]);

                                break;
                            }
                        case "ClientQueryRefButtonColumn":
                            {
                                textBoxes[i] = new TextBox();
                                textBoxes[i].ID = "txt" + i.ToString();
                                if (GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue) != null)
                                {
                                    textBoxes[i].Text = GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue).ToString();
                                }
                                textBoxes[i].Width = ((WebQueryColumns)this.Columns[i]).Width;
                                textBoxes[i].ForeColor = this.TextColor;
                                textBoxes[i].Font.CopyFrom(this.TextFont);
                                webRefButtons[i] = new WebRefButton();
                                webRefButtons[i].ID = "btn" + i.ToString();
                                WebRefButton wrf = (WebRefButton)GetObjByID(((WebQueryColumns)this.Columns[i]).WebRefButton);
                                webRefButtons[i].Caption = wrf.Caption;
                                webRefButtons[i].RefURL = wrf.RefURL;
                                webRefButtons[i].RefURLHeight = wrf.RefURLHeight;
                                webRefButtons[i].RefURLWidth = wrf.RefURLWidth;
                                webRefButtons[i].RefURLLeft = wrf.RefURLLeft;
                                webRefButtons[i].RefURLTop = wrf.RefURLTop;
                                MatchControl mc = new MatchControl();
                                mc.ControlID = "txt" + i.ToString();
                                webRefButtons[i].MatchControls.Add(mc);
                                tcQueryText.Controls.Add(textBoxes[i]);
                                tcQueryText.Controls.Add(webRefButtons[i]);
                                break;
                            }
                        case "ClientQueryCalendarColumn":
                            {
                                webDateTimePickers[i] = new WebDateTimePicker();
                                webDateTimePickers[i].ID = "txt" + i.ToString();
                                webDateTimePickers[i].Width = ((WebQueryColumns)this.Columns[i]).Width;
                                webDateTimePickers[i].CheckDate = true;
                                webDateTimePickers[i].ForeColor = this.TextColor;
                                webDateTimePickers[i].Font.CopyFrom(this.TextFont);
                                if (GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue) != null)
                                {
                                    try
                                    {
                                        if (GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue) != null)
                                        {
                                            webDateTimePickers[i].Text = GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue).ToString();
                                        }
                                    }
                                    catch
                                    {
                                        webDateTimePickers[i].Text = DateTime.Now.ToShortDateString();
                                    }
                                }
                                webDateTimePickers[i].DateFormat = dateFormat.ShortDate;
                                webDateTimePickers[i].DateFormatString = this.DateFormatString;
                                tcQueryText.Controls.Add(webDateTimePickers[i]);
                                break;
                            }
                    }
                    if (!((WebQueryColumns)this.Columns[i]).Visible)
                    {
                        foreach (Control ctrl in tcQueryText.Controls)
                        {
                            ctrl.Visible = false;
                            pn.Controls.Add(ctrl);
                        }
                        continue;
                    }

                    //is newline
                    if (((WebQueryColumns)this.Columns[i]).NewLine == true)
                    {
                        if (trQuery[intRowCount].Cells.Count > 0)
                        {
                            table.Controls.Add(trQuery[intRowCount]);
                            if (!this.InnerTable)
                            {
                                TableRow trEmpty = new TableRow();
                                trEmpty.Height = this.GapVertical;
                                if (GapVertical > 0)
                                {
                                    table.Controls.Add(trEmpty);
                                }
                            }
                            intRowCount++;
                            trQuery[intRowCount] = new TableRow();
                            //trQuery[intRowCount].BackColor = pn.BackColor;
                        }

                        trQuery[intRowCount].Cells.Add(tcLabel);
                        trQuery[intRowCount].Cells.Add(tcQueryText);
                    }
                    else
                    {
                        if (trQuery[intRowCount].Cells.Count > 0)
                        {
                            if (!this.InnerTable)
                            {
                                TableCell tcEmpty = new TableCell();
                                tcEmpty.Width = this.GapHorizontal;
                                if (GapHorizontal > 0)
                                {
                                    trQuery[intRowCount].Cells.Add(tcEmpty);
                                }
                            }

                        }
                        trQuery[intRowCount].Cells.Add(tcLabel);
                        trQuery[intRowCount].Cells.Add(tcQueryText);

                    }
                }

                table.Controls.Add(trQuery[intRowCount]);
                //new add to fill tr

                int maxcontrol = 0;
                for (int i = 0; i < intRowCount + 1; i++)
                {
                    maxcontrol = Math.Max(maxcontrol, trQuery[i].Controls.Count);
                }
                for (int i = 0; i < table.Controls.Count; i++)
                {
                    if (table.Controls[i].Controls.Count < maxcontrol)
                    {
                        TableCell tccolspan = new TableCell();
                        if (this.InnerTable)
                        {
                            if (!pn.BackColor.IsEmpty)
                            {
                                tccolspan.BackColor = pn.BackColor;
                            }
                            else
                            {
                                tccolspan.BackColor = Color.White;
                            }
                            tccolspan.BorderColor = tccolspan.BackColor;
                            //tccolspan.BorderStyle = this.BorderStyle;
                        }
                        tccolspan.ColumnSpan = maxcontrol - table.Controls[i].Controls.Count;
                        table.Controls[i].Controls.Add(tccolspan);
                    }
                }



                pn.Controls.Add(table);
                if (!Page.IsPostBack)//postback do not add item again
                {
                    foreach (WebDropDownList wdl in dropDownlists)
                    {
                        if (wdl != null)
                        {
                            wdl.DataBind();
                        }
                    }
                }
                this.isShow.Add(pn.ID);
            }
        }

        public void Cancel()
        {
            Panel p = null;
            if (!String.IsNullOrEmpty(this.PanelID))
            {
                p = this.Page.FindControl(this.PanelID) as Panel;
            }
            if (p == null)
            {

            }
            else
            {
                Clear(p);
            }
        }


        public void Clear(Panel pn)
        {
            if (this.isShow.Contains(pn.ID))
            {
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    string controlname = "txt" + i.ToString();
                    Control ct = GetControlByID(controlname, pn);
                    if (ct != null)
                    {

                        if (ct is TextBox)
                        {
                            if (GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue) != null)
                            {
                                (ct as TextBox).Text = GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue).ToString();
                            }
                            else
                            {
                                (ct as TextBox).Text = "";
                            }
                        }
                        else if (ct is WebDateTimePicker)
                        {
                            if (GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue) != null)
                            {
                                try
                                {
                                    (ct as WebDateTimePicker).Text = GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue).ToString();
                                }
                                catch
                                {
                                    (ct as WebDateTimePicker).Text = DateTime.Now.ToShortDateString();
                                }
                            }
                            else
                            {
                                (ct as WebDateTimePicker).Text = DateTime.Now.ToShortDateString();
                            }

                        }
                        else if (ct is CheckBox)
                        {
                            object defaultvalue = GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue);
                            if (defaultvalue != null)
                            {
                                if (string.Compare(defaultvalue.ToString(), "true", true) == 0 || string.Compare(defaultvalue.ToString(), "1") == 0)
                                {
                                    (ct as CheckBox).Checked = true;
                                }
                                else
                                {
                                    (ct as CheckBox).Checked = false;
                                }
                            }
                            else
                            {
                                (ct as CheckBox).Checked = false;
                            }

                        }
                        else if (ct is WebRefVal)
                        {
                            if (GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue) != null)
                            {
                                try
                                {
                                    (ct as WebRefVal).BindingValue = GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue).ToString();
                                }
                                catch
                                {

                                }
                            }
                            else
                            {
                                (ct as WebRefVal).BindingValue = "";
                            }
                            //ct.Focus();
                        }
                        else if (ct is WebDropDownList)
                        {
                            if (GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue) != null)
                            {
                                try
                                {
                                    (ct as WebDropDownList).SelectedValue = GetDefaultValue(((WebQueryColumns)this.Columns[i]).DefaultValue).ToString();
                                }
                                catch
                                {
                                    (ct as WebDropDownList).SelectedIndex = 0;
                                }
                            }
                            else
                            {
                                (ct as WebDropDownList).SelectedIndex = 0;
                            }

                        }
                    }
                }
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

        public string GetText()
        {
            string strQueryText = "";

            string[] arrQueryText;
            if (this.Page.Request.QueryString != null && this.Page.Request.QueryString["Querytext"] != null)
            {
                strQueryText = this.Page.Request.QueryString["Querytext"];
                arrQueryText = strQueryText.Split(';');
                strQueryText = "";
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    if (arrQueryText[i] != string.Empty)
                    {
                        strQueryText += ((WebQueryColumns)this.Columns[i]).Caption
                                        + ((WebQueryColumns)this.Columns[i]).Operator
                                        + arrQueryText[i] + "\n";
                    }
                }
            }


            return strQueryText;
        }

        public string GetText(Panel pn)
        {
            string strQueryText = "";
            for (int i = 0; i < this.Columns.Count; i++)
            {
                string text = "";
                //if (pn.FindControl("txt" + i.ToString()) == null)
                //{
                //    return "GetText Error!Can't find control in panel: " + pn.ID;
                //}
                switch (((WebQueryColumns)this.Columns[i]).ColumnType)
                {
                    case "ClientQueryTextBoxColumn":
                        text = (pn.FindControl("txt" + i.ToString()) as TextBox).Text;
                        break;
                    case "ClientQueryComboBoxColumn":
                        text = (pn.FindControl("txt" + i.ToString()) as WebDropDownList).SelectedValue;
                        break;
                    case "ClientQueryRefValColumn":
                        text = (pn.FindControl("txt" + i.ToString()) as WebRefVal).BindingValue;
                        break;
                    case "ClientQueryCalendarColumn":
                        text = (pn.FindControl("txt" + i.ToString()) as WebDateTimePicker).Text;
                        break;
                }
                if (text != string.Empty)
                {
                    strQueryText += ((WebQueryColumns)this.Columns[i]).Caption
                                       + ((WebQueryColumns)this.Columns[i]).Operator
                                       + text + "\n";
                }
            }
            return strQueryText;
        }

        public string GetTableName(string sqlCommand, string columnName, string tbName)
        {
            int index = sqlCommand.IndexOf(".[" + columnName + "]");
            if (index > 0)
            {
                string tablename = sqlCommand.Substring(0, index);
                index = tablename.LastIndexOf("[");
                tablename = tablename.Substring(index);
                string[] arrtablename = tablename.Split('[', ']');
                if (arrtablename.Length == 3)
                {
                    tablename = arrtablename[1];
                    return tablename;
                }
                else
                {
                    return tbName;
                }
            }
            else
            {
                return tbName;
            }
        }

        #endregion

    }

    #region WebQueryColumnsCollection

    public class WebQueryColumnsCollection : InfoOwnerCollection
    {
        public WebQueryColumnsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebQueryColumns))
        {

        }
        public DataSet DsForDD = new DataSet();
        public new WebQueryColumns this[int index]
        {
            get
            {
                return (WebQueryColumns)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebQueryColumns)
                    {
                        //原来的Collection设置为0
                        ((WebQueryColumns)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebQueryColumns)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
    #endregion

    #region WebQueryColumns
    public class WebQueryColumns : InfoOwnerCollectionItem, IGetValues
    {
        #region Constructor

        public WebQueryColumns()
            : this("clientquery", true, "", "", 120, "ClientQueryTextBoxColumn", "And", "=", "Left")
        {

        }

        public WebQueryColumns(string name, bool newline, string column, string caption, int width, string columntype, string condition
            , string operators, string textalign)
        {
            _name = name;
            _newline = newline;
            _column = column;
            _caption = caption;
            _width = width;
            _columntype = columntype;
            _condition = condition;
            _operator = operators;
            _textalign = textalign;
            _defaultvalue = "";
            _text = "";
            _NotNullBackColor = Color.White;
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

        private bool _newline;
        public bool NewLine
        {
            get
            {
                return _newline;
            }
            set
            {
                _newline = value;
            }
        }

        private string _column;
        [Editor(typeof(Srvtools.WebQueryColumns.QueryColumnEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Column
        {
            get
            {
                return _column;
            }
            set
            {
                _column = value;
                //if (this.Owner != null)
                //{
                //    if (((WebClientQuery)this.Owner).Site == null)
                //    {
                //        this.Caption = GetHeaderText(_column);
                //    }
                //    else if (((WebClientQuery)this.Owner).Site.DesignMode)
                //    {
                //        this.Caption = GetHeaderText(_column);
                //    }
                //}

                if (this.Owner != null)
                {
                    if (((WebClientQuery)this.Owner).Site == null)
                    {
                        this.Caption = GetDDText(_column);
                    }
                    else if (((WebClientQuery)this.Owner).Site.DesignMode)
                    {
                        this.Caption = GetDDText(_column);
                    }
                }
            }
        }

        private string _columntype;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("ClientQueryTextBoxColumn")]
        public string ColumnType
        {
            get
            {
                return _columntype;
            }
            set
            {
                _columntype = value;

                if (_columntype != "ClientQueryComboBoxColumn" && _columntype != "ClientQueryRefValColumn")
                {
                    _webrefval = null;
                }
                if (_columntype != "ClientQueryRefButtonColumn")
                {
                    _webrefbutton = null;
                }
            }
        }

        private string _caption;
        [NotifyParentProperty(true)]
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
                    if (_column != null && _column != "")
                    {
                        _caption = _column;
                        _name = _column;
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
                //if (_columntype != "ClientQueryComboBoxColumn" && _columntype != "ClientQueryRefValColumn" && value != null)
                //{
                //    //MessageBox.Show("WebRefval can be set only when\ncolumntype is combobox & refval.");
                //}
                //else
                //{
                _webrefval = value;
                //}
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
                if (_columntype != "ClientQueryRefButtonColumn" && value != null)
                {
                    //MessageBox.Show("WebRefValButton can be set only when\ncolumntype is refbutton.");
                }
                else
                {
                    _webrefbutton = value;
                }
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

        private string _textalign;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("Left")]
        public string TextAlign
        {
            get
            {
                return _textalign;
            }
            set
            {
                _textalign = value;
            }
        }

        private int _width;
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

        private bool _NotNull;

        public bool NotNull
        {
            get { return _NotNull; }
            set { _NotNull = value; }
        }

        private Color _NotNullBackColor;

        public Color NotNullBackColor
        {
            get { return _NotNullBackColor; }
            set { _NotNullBackColor = value; }
        }

        private bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        private bool isNvarChar;

        public bool IsNvarChar
        {
            get { return isNvarChar; }
            set { isNvarChar = value; }
        }


        #endregion

        #region method

        private string GetDDText(string fieldName)
        {
            DataSet Dset = ((WebQueryColumnsCollection)this.Collection).DsForDD;
            string strCaption = "";

            if (Dset.Tables.Count == 0)
            {
                WebClientQuery wcq = (WebClientQuery)this.Owner;
                object obj = wcq.GetObjByID(wcq.DataSourceID);
                if (obj is WebDataSource)
                {
                    ((WebQueryColumnsCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(obj as WebDataSource, true);
                    Dset = ((WebQueryColumnsCollection)this.Collection).DsForDD;
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
            this._column = colname;
        }

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is WebClientQuery)
            {
                if (string.Compare(sKind, "column", true) == 0)//IgnoreCase
                {
                    if (this.Owner is WebClientQuery)
                    {
                        WebClientQuery wcq = (WebClientQuery)this.Owner;
                        if (wcq.Page != null && wcq.DataSourceID != null && wcq.DataSourceID != "")
                        {
                            foreach (Control ctrl in wcq.Page.Controls)
                            {
                                if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == wcq.DataSourceID)
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
                else if (string.Compare(sKind, "operator", true) == 0)//IgnoreCase
                {
                    values.Add("=");
                    values.Add("!=");
                    values.Add(">");
                    values.Add("<");
                    values.Add(">=");
                    values.Add("<=");
                    values.Add("%");
                    values.Add("%%");
                    values.Add("in");
                }
                else if (string.Compare(sKind, "condition", true) == 0)//IgnoreCase
                {
                    values.Add("And");
                    values.Add("Or");
                }
                else if (string.Compare(sKind, "columntype", true) == 0)//IgnoreCase
                {
                    values.Add("ClientQueryTextBoxColumn");
                    values.Add("ClientQueryComboBoxColumn");
                    values.Add("ClientQueryCheckBoxColumn");
                    values.Add("ClientQueryRefValColumn");
                    values.Add("ClientQueryCalendarColumn");
                    values.Add("ClientQueryRefButtonColumn");
                }
                else if (string.Compare(sKind, "textalign", true) == 0)//IgnoreCase
                {
                    values.Add("Left");
                    values.Add("Center");
                    values.Add("Right");
                }
                else if (string.Compare(sKind, "webrefval", true) == 0)//IgnoreCase
                {
                    if (this.Owner is WebClientQuery)
                    {
                        WebClientQuery wcq = (WebClientQuery)this.Owner;
                        foreach (Control ct in wcq.Page.Controls)
                        {
                            if (ct is WebRefVal)
                            {
                                values.Add(ct.ID);
                            }
                        }
                        if (wcq.Page.Form != null)
                        {
                            foreach (Control ct in wcq.Page.Form.Controls)
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
                    if (this.Owner is WebClientQuery)
                    {
                        WebClientQuery wcq = (WebClientQuery)this.Owner;
                        foreach (Control ct in wcq.Page.Controls)
                        {
                            if (ct is WebRefButton)
                            {
                                values.Add(ct.ID);
                            }
                        }
                        if (wcq.Page.Form != null)
                        {
                            foreach (Control ct in wcq.Page.Form.Controls)
                            {
                                if (ct is WebRefButton)
                                {
                                    values.Add(ct.ID);
                                }
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
        #endregion

        #endregion

        #region QueryColumnEditor
        public class QueryColumnEditor : System.Drawing.Design.UITypeEditor
        {
            //private IWindowsFormsEditorService edSvc;
            public QueryColumnEditor()
            {

            }

            // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
            // drop down dialog, or no UI outside of the properties window.
            [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
            public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }

            //    // Displays the UI for value selection.
            //    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
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
    #endregion

    #region wcqDataSourceEditor
    public class wcqDataSourceEditor : UITypeEditor
    {
        public wcqDataSourceEditor()
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
            if (context.Instance is WebClientQuery)
            {
                ControlCollection ctrlList = ((WebClientQuery)context.Instance).Page.Controls;
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


}
