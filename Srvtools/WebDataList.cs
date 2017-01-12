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
using System.Web.UI.HtmlControls;

namespace Srvtools
{
    [Designer(typeof(WebDataListDesigner))]
    [ToolboxBitmap(typeof(WebDataList), "Resources.WebDataList.ico")]
    public class WebDataList : DataList
    {

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string rscUrl = "../css/controls/WebDataList.css";
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

        public WebDataList()
        {
            _layOutColNum = 1;
            _AllowPaging = false;
            _PageSize = 10;
        }

        private bool _AllowPaging;
        [Category("Infolight"),
        Description("Indicate whether allow page in datalist")]
        [DefaultValue(false)]
        public bool AllowPaging
        {
            get { return _AllowPaging; }
            set { _AllowPaging = value; }
        }

        [Browsable(false)]
        public int PageCount
        {
            get { return PagedDataSource.PageCount; }
        }

        private int _PageSize;
        [Category("Infolight"),
        Description("Specifies the size of each page in datalist")]
        [DefaultValue(10)]
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

        [Browsable(false)]
        [DefaultValue(0)]
        public int PageIndex
        {
            get { return ViewState["PageIndex"] == null?0:Convert.ToInt32(ViewState["PageIndex"].ToString()); }
            set 
            {
                if (this.Page != null && value != this.PageIndex)
                {
                    ViewState["PageIndex"] = value;
                    this.DataSource = PagedDataSource;
                    this.DataSourceID = string.Empty;
                    base.DataBind();
                    OnPageIndexChanged(new EventArgs());
                }
            }
        }

        [Browsable(false)]
        public string WebDataSourceID
        {
            get 
            { 
                if(ViewState["WebDataSourceID"] == null)
                {
                    this.ViewState["WebDataSourceID"] = this.DataSourceID;
                }
                return ViewState["WebDataSourceID"].ToString(); 
            }
        }

        private PagedDataSource pds;
        [Browsable(false)]
        private PagedDataSource PagedDataSource
        {
            get
            {
                if (pds == null)
                {
                    pds = new PagedDataSource();
                    WebDataSource wds = GetAllCtrls(this.WebDataSourceID, this.Page) as WebDataSource;
                    if (wds != null)
                    {
                        pds.DataSource = wds.View;
                        pds.AllowPaging = true;
                       
                    }
                    else
                    {
                        throw new Exception(string.Format("Can not find webDataSource", this.WebDataSourceID));
                    }
                }
                pds.PageSize = this.PageSize;
                if (this.PageIndex < 0)
                {
                    ViewState["PageIndex"] = 0;
                }
                else if(this.PageIndex >= pds.PageCount)
                {
                    ViewState["PageIndex"] = pds.PageCount - 1;
                }
                pds.CurrentPageIndex = this.PageIndex;
                return pds;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (AllowPaging & !this.Page.IsPostBack)
            {
                this.DataSource = PagedDataSource;
                this.DataSourceID = string.Empty;
                base.DataBind();
            }
            base.OnLoad(e);
        }

        public override void DataBind()
        {
            if (AllowPaging)
            {
                pds = null;
                this.DataSourceID = string.Empty;
                this.DataSource = PagedDataSource;
                base.DataBind();
            }
            else
            {
                base.DataBind();
            }
        }

        internal static readonly object EventOnPageIndexChanged = new object();
        [Category("Infolight"),
        Description("The event ocured when datalist paged")]
        public event EventHandler PageIndexChanged
        {
            add { this.Events.AddHandler(EventOnPageIndexChanged,value);}
            remove { this.Events.RemoveHandler(EventOnPageIndexChanged, value); }
        }

        protected void OnPageIndexChanged(EventArgs value)
        {
            EventHandler handler = this.Events[EventOnPageIndexChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, value);
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
                if (this.Page.Form != null)
                    return GetAllCtrls(ObjID, this.Page.Form);
                else
                    return GetAllCtrls(ObjID, this.Page);
            }
        }
    }

    public class WebDataListDesigner : DataListDesigner
    {
        public WebDataListDesigner()
        {
            
        }

        protected override void OnSchemaRefreshed()
        {
            WebDataList dataList = (WebDataList)this.Component;
            if (dataList.DataSourceID == null || dataList.DataSourceID.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("DataSourceID property is null.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            //IDataSourceViewSchema schema = this.GetDataSourceSchema();
            //IDataSourceFieldSchema[] schemaArray = schema.GetFields();
            IDesignerHost host = (IDesignerHost)dataList.Site.GetService(typeof(IDesignerHost));

            base.OnSchemaRefreshed();

            DataSet Dset = new DataSet();
            if (dataList.Site.DesignMode)
            {
                object obj = dataList.GetObjByID(dataList.DataSourceID);
                if (obj is WebDataSource)
                {
                    Dset = DBUtils.GetDataDictionary(obj as WebDataSource, true);
                }
            }

            foreach (TemplateGroup tempGroup in this.TemplateGroups)
            {
                foreach (TemplateDefinition tempDefin in tempGroup.Templates)
                {
                    if (tempDefin.Name == "EditItemTemplate" || tempDefin.Name == "ItemTemplate" || tempDefin.Name == "SelectedItemTemplate")
                    {
                        StringBuilder builder = new StringBuilder();
                        string content = tempDefin.Content;
                        if (content == null || content.Length == 0)
                            continue;

                        string[] ctrlTexts = content.Split("\r\n".ToCharArray());
                        int i = 0;
                        int j = 0;
                        int m = dataList.LayOutColNum * 2;

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
                        j = j * 2;

                        if (m > 0)
                        {
                            builder.Append("<table>");
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
                            if (tempDefin.Name != "ItemTemplate")
                            {
                                ddText = GetValidateText(ctrlText);
                                ddText = GetDDText(ddText, Dset);
                            }
                            else
                            {
                                ddText = GetDDText(ctrlText, Dset);
                            }
                            builder.Append(ddText);
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

            WebDataList dataList = (WebDataList)this.Component;
            IDesignerHost host = (IDesignerHost)dataList.Site.GetService(typeof(IDesignerHost));
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
                bool valGot = false;
                if (ctrlText.IndexOf("</span>") != -1)
                {
                    valGot = true;
                }
                if (valGot)
                {
                    string fieldName = ctrlText.Substring(0, ctrlText.IndexOf("</span>"));
                    fieldName = fieldName.Substring(ControlText.IndexOf("\">*") + 3);
                    if (string.Compare(Dset.Tables[0].Rows[y]["FIELD_NAME"].ToString(), fieldName, true) == 0)//IgnoreCase
                    {
                        string strCaption = Dset.Tables[0].Rows[y]["CAPTION"].ToString();
                        if (strCaption != "")
                        {
                            ctrlText = ctrlText.Replace(fieldName, strCaption);
                            break;
                        }
                    }
                }
                else if (string.Compare(Dset.Tables[0].Rows[y]["FIELD_NAME"].ToString(), ctrlText, true) == 0)//IgnoreCase
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

        private string GetValidateText(string ControlText)
        {
            string fieldName = ControlText.Substring(0, ControlText.IndexOf(": <") + 1);
            WebFormView formView = (WebFormView)this.Component;
            foreach (Control ctrl in formView.Page.Controls)
            {
                if (ctrl is WebValidate && ((WebValidate)ctrl).DataSourceID == formView.DataSourceID)
                {
                    WebValidate wVal = (WebValidate)ctrl;
                    if (wVal.ValidateActive)
                    {
                        foreach (ValidateFieldItem vfi in wVal.Fields)
                        {
                            if (string.Compare(vfi.FieldName + ":", fieldName, true) == 0)//IgnoreCase
                            {
                                ControlText = "<span style=\"color:" + wVal.ValidateColor.Name + "\">*" + fieldName + "</span>" + ControlText.Substring(ControlText.IndexOf(": <") + 1);
                            }
                        }
                    }
                }
            }
            return ControlText;
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
    }
}
