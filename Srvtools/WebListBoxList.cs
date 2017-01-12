using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing.Design;
using System.Data;
using System.Xml;
using System.Globalization;
using System.Resources;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Web.UI.Design;
using System.ComponentModel.Design;

namespace Srvtools
{

    [ToolboxBitmap(typeof(WebListBoxList), "Resources.WebListBoxList.ico")]
    public class WebListBoxList : TextBox, IGetValues
    {
        public WebListBoxList()
        {
            _CheckBoxColumns = 1;
        }

        public enum ListBoxStyle
        {
            ListBox,
            CheckBox
        }

        private TextBox txtValue = new TextBox();

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
                return string.Empty;
            }
            set
            {
                this.ViewState["DataSourceID"] = value;
            }
        }

        [Category("InfoLight"),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string DataTextField
        {
            get
            {
                object obj = this.ViewState["DataTextField"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["DataTextField"] = value;
            }
        }

        [Category("InfoLight"),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string DataValueField
        {
            get
            {
                object obj = this.ViewState["DataValueField"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["DataValueField"] = value;
            }
        }

        private bool _listBoxQuery = false;
        [Category("InfoLight")]
        [DefaultValue(false)]
        public bool ListBoxQuery
        {
            get { return _listBoxQuery; }
            set { _listBoxQuery = value; }
        }

        private string _openTop;
        [Category("InfoLight")]
        public string OpenTop
        {
            get { return _openTop; }
            set { _openTop = value; }
        }

        private string _openLeft;
        [Category("InfoLight")]
        public string OpenLeft
        {
            get { return _openLeft; }
            set { _openLeft = value; }
        }

        private string _openHeight;
        [Category("InfoLight")]
        public string OpenHeight
        {
            get { return _openHeight; }
            set { _openHeight = value; }
        }

        private string _openWidth;
        [Category("InfoLight")]
        public string OpenWidth
        {
            get { return _openWidth; }
            set { _openWidth = value; }
        }

        private int _CheckBoxColumns;
        [Category("InfoLight")]
        public int CheckBoxColumns
        {
            get { return _CheckBoxColumns; }
            set { _CheckBoxColumns = value; }
        }

        [Category("InfoLight")]
        public char Separator
        {
            get
            {
                object obj = this.ViewState["Separator"];
                if (obj != null)
                {
                    return (char)obj;
                }
                return ',';
            }
            set
            {
                this.ViewState["Separator"] = value;
            }
        }

        [Category("InfoLight")]
        public string Caption
        {
            get
            {
                object obj = this.ViewState["Caption"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["Caption"] = value;
            }
        }

        [Category("InfoLight"),
        Description("Caption of the InnerButton")]
        [Bindable(true)]
        public string ButtonCaption
        {
            get
            {
                object obj = this.ViewState["ButtonCaption"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["ButtonCaption"] = value;
            }
        }

        [Category("InfoLight"),
        Description("Indicate whether the InnerButton should use image for it's appearance"),
        DefaultValue(true)]
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

        [Category("InfoLight"),
        Description("Specifies the style of listboxlist to edit")]
        public ListBoxStyle ListStyle
        {
            get
            {
                if (ViewState["ListStyle"] != null)
                {
                    return (ListBoxStyle)ViewState["ListStyle"];
                }
                return ListBoxStyle.ListBox;
            }
            set
            {
                ViewState["ListStyle"] = value;
            }
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

        [Browsable(false)]
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

        /// <summary>
        /// Get the the text of textfield
        /// </summary>
        /// <returns>The text splited by seperator</returns>
        public string GetDataText()
        {
            DataTable table = new DataTable();
            object obj = this.GetObjByID(this.DataSourceID);
            if (obj is WebDataSource)
            {
                WebDataSource ds = (WebDataSource)obj;
                if (ds.SelectAlias != null && ds.SelectAlias.Length != 0 && ds.SelectCommand != null && ds.SelectCommand.Length != 0)
                {
                    table = ds.CommandTable;
                }
                else
                {
                    table = ds.InnerDataSet.Tables[ds.DataMember];
                }
            }
            string value = this.Text;
            string[] arrvalue = this.Text.Split(Separator);
            StringBuilder textbuilder = new StringBuilder();
            foreach (string str in arrvalue)
            {
                if (str != "")
                {
                    if (textbuilder.Length != 0)
                    {
                        textbuilder.Append(Separator);
                    }
                    DataRow[] arrdr = table.Select(this.DataValueField + "='" + str + "'");
                    if (arrdr.Length > 0)
                    {
                        string text = arrdr[0][this.DataTextField].ToString();
                        textbuilder.Append(text);
                    }
                }
            }
            return textbuilder.ToString();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string top = "300";
            string left = "300";
            string height = "360";
            string width = "560";
            if (OpenTop != null && OpenTop != "") top = OpenTop;
            if (OpenLeft != null && OpenLeft != "") left = OpenLeft;
            if (OpenHeight != null && OpenHeight != "") height = OpenHeight;
            if (OpenWidth != null && OpenWidth != "") width = OpenWidth;
            string DBAlias = "";
            string SelectCommand = "";
            object obj = this.GetObjByID(this.DataSourceID);
            WebDataSource ds = (WebDataSource)obj;
            if (ds != null)
            {
                DBAlias = ds.SelectAlias;
                SelectCommand = ds.SelectCommand;
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table); // <table>
            writer.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
            //this.RenderBeginTag(writer);
            //base.RenderContents(writer);
            base.Render(writer);
            //this.RenderEndTag(writer);
            writer.RenderEndTag(); // <td>
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            string buttonName = this.UniqueID + "$InnerButton";
            writer.AddAttribute(HtmlTextWriterAttribute.Id, buttonName);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, buttonName);
            if (Page.Site == null && !this.ReadOnly)
            {
                string resxFilePath = GetResxFilePath();
                string webDataSetID = GetDataSetID();
                string datamember = GetDataMember();
                string param = "?Resx=" + resxFilePath + "&DataSet=" + webDataSetID + "&DataMember=" + datamember +
                                "&WhereStr=" + (ds.WhereStr == null ? string.Empty : HttpUtility.UrlEncode(ds.WhereStr).Replace("'", "\\'")) +
                                "&ValueField=" + HttpUtility.UrlEncode(this.DataValueField) + "&ListBoxQuery=" + this.ListBoxQuery.ToString() +
                                "&TextField=" + HttpUtility.UrlEncode(this.DataTextField) + "&Columns=" + CheckBoxColumns.ToString() + "&Caption=" + HttpUtility.UrlEncode(this.Caption) +
                                "&Separator=" + HttpUtility.UrlEncode(this.Separator.ToString()) + "&ControlID=" + HttpUtility.UrlEncode(this.ClientID) +
                                "&DBAlias=" + DBAlias + "&SelectCommand=" + HttpUtility.UrlEncode(SelectCommand ?? "").Replace("'", "\\'") + "&Value='+a+'";
                string ScriptBlock = "";
                if (this.ListStyle == ListBoxStyle.ListBox)
                {

                    ScriptBlock = "var a = encodeURI(document.getElementById('" + this.ClientID + "').value);window.open('../InnerPages/frmListBoxList.aspx" + param + "', '', 'width=" + width + ",height=" + height + ",top=" + top + ",left=" + left + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no');";
                }
                else
                {
                    ScriptBlock = "var a = encodeURI(document.getElementById('" + this.ClientID + "').value);window.open('../InnerPages/frmListBoxListCheck.aspx" + param + "', '', 'width=415 ,height=415,top=" + top + ",left=" + left + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no');";
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, ScriptBlock);
            }
            if (this.UseButtonImage)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, "../Image/listboxlist/listboxlist.png");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "border-right: thin outset; border-top: thin outset; border-left: thin outset; width: 18px; border-bottom: thin outset; background-color: buttonface");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:25px; height:" + (this.Height.Value + 8).ToString() + "px;");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, this.ButtonCaption);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
            }
            writer.RenderEndTag();
            writer.RenderEndTag(); // <td>
            writer.RenderEndTag(); // <tr>
            writer.RenderEndTag(); // <table>
        }

        public string GetResxFilePath()
        {
            string value = "";
            if (this.ResxFilePath != null && this.ResxFilePath != "")
                value = this.ResxFilePath;
            else
                value = Page.Request.MapPath(Page.Request.Path) + @".vi-VN.resx";
            value = value.Replace("\\", "\\\\");
            return HttpUtility.UrlEncode(value);
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

        public string GetDataMember()
        {
            string value = "";
            object wds = this.GetObjByID(this.DataSourceID);
            if (wds is WebDataSource && ((WebDataSource)wds).WebDataSetID != null && ((WebDataSource)wds).WebDataSetID != "")
            {
                value = ((WebDataSource)wds).DataMember;
            }
            return value;

        }
    }
}
