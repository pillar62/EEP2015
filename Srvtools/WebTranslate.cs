using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Resources;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web.UI.HtmlControls;

namespace Srvtools
{
    [DefaultEvent("OKClick")]
    [ToolboxBitmap(typeof(WebTranslate), "Resources.WebTranslate.png")]
    public class WebTranslate : WebControl
    {
        public WebTranslate()
        {
            this.OKButtonWidth = 60;
            this.CancelButtonWidth = 60;
            _RefReturnFields = new RefReturnFieldsCollection(this, typeof(RefReturnField));
            _whereitem = new WebTranslateWhereItemCollection(this, typeof(WebTranslateWhereItem));
        }

        private SYS_LANGUAGE language;
        private Button _OKButton = new Button();
        private Button _CancelButton = new Button();

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                OpenEditMode = Page.Request.QueryString["OpenEditMode"];
                PagePath = Page.Request.QueryString["PagePath"];
                string s = Page.Request.QueryString["KeyValues"];
                if (s != null && s != "")
                    s = s.Replace("$$$", "'");
                KeysValues = s;
                PageIndex = Page.Request.QueryString["PageIndex"];
                SelectIndex = Page.Request.QueryString["SelectIndex"];
                MasterOrDetail = Page.Request.QueryString["MasterOrDetail"];
                RelationFields = Page.Request.QueryString["RelationFields"];
                RelationValues = Page.Request.QueryString["RelationValues"];
                Paramters = Page.Request.QueryString["Paramters"];
                MatchControls = Page.Request.QueryString["MatchControls"];
                GridViewID = Page.Request.QueryString["GridViewID"];
                InitEditMode();
                string wheretext = Page.Request.QueryString["WebHyperLinkText"];
                if (wheretext != null)
                {
                    SetWhere(wheretext);
                }
            }
            base.OnLoad(e);
            if (this.BindingObject != null && this.BindingObject != "")
            {
                object obj = this.GetObjByID(this.BindingObject);
                if (obj != null && obj is WebGridView)
                {
                    WebGridView gdView = (WebGridView)obj;
                    gdView.SelectedIndexChanged += new EventHandler(gdView_SelectedIndexChanged);
                    gdView.RowDataBound += new GridViewRowEventHandler(gdView_RowDataBound);
                }
            }
        }

        void gdView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.BindingObject != null && this.BindingObject != "")
            {
                object obj = this.GetObjByID(this.BindingObject);
                if (obj != null && obj is WebGridView)
                {
                    WebGridView gdView = (WebGridView)obj;
                    this.ViewState["SelectedRowIndex"] = gdView.SelectedRow.RowIndex;
                    gdView.DataBind();
                }
            }
        }

        void gdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int index = this.ViewState["SelectedRowIndex"] == null ? -1 : Convert.ToInt32(this.ViewState["SelectedRowIndex"]);
            object objDs = this.GetObjByID(this.DataSourceID);

            if (objDs != null && objDs is WebDataSource)
            {
                WebDataSource wds = (WebDataSource)objDs;
                if (e.Row.RowIndex == index && index != -1)
                {
                    object obj = e.Row.DataItem;
                    if (obj != null && obj is DataRowView)
                    {
                        if (wds.PrimaryKey.Length > 0)
                        {
                            DataRowView rowView = (DataRowView)obj;
                            object[] kfs = new object[wds.PrimaryKey.Length];
                            for (int i = 0; i < wds.PrimaryKey.Length; i++)
                            {
                                string columnName = wds.PrimaryKey[i].ColumnName;
                                kfs[i] = rowView[columnName];
                            }
                            this.ViewState["SelectedRowKeyFields"] = kfs;
                        }
                    }
                    this.ViewState["SelectedRowIndex"] = -1;
                }
            }

        }

        #region Methods
        private void ApplyToReturn()
        {
            object obj = this.GetObjByID(this.BindingObject);
            bool allowToReturn = false;
            if (obj is WebFormView)
            {
                WebFormView frmView = (WebFormView)obj;
                allowToReturn = !frmView.ValidateFailed;
            }
            else if (obj is WebDetailsView)
            {
                WebDetailsView detView = (WebDetailsView)obj;
                allowToReturn = !detView.ValidateFailed;
            }
            if (!this.AllowToReturn)
            {
                allowToReturn = false;
            }
            if (allowToReturn)
            {
                if (OpenEditMode == "Insert")
                {
                    WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                    ds.ApplyUpdates();
                }
                string p = "";
                if (this.Paramters != "") // for navy use only ^_^!
                {
                    string[] pas = this.Paramters.Split('^');
                    foreach (string pa in pas)
                    {
                        p += "&" + pa.Substring(0, pa.IndexOf('=') + 1) + HttpUtility.UrlEncode(pa.Substring(pa.IndexOf('=') + 1));
                    }
                }
                string kv = (this.InsertKeyValues == "") ? "" : "&InsertKeyValues=" + HttpUtility.UrlEncode(this.InsertKeyValues);
                string wherestr = string.Empty;
                if (this.Page.Request.QueryString["WhereStr"] != null)
                {
                    wherestr = "&IsQueryBack=1&DataSourceID=" + this.Page.Request.QueryString["DataSourceID"] + "&Filter=" + HttpUtility.UrlEncode(this.Page.Request.QueryString["WhereStr"].ToString()).Replace("'", "\\'");
                }
                string itemparam = this.Page.Request.QueryString["ItemParam"] != null ? HttpUtility.UrlEncode(this.Page.Request.QueryString["ItemParam"]) : string.Empty;

                string script = "window.opener.location.reload('" + this.PagePath + "?PageIndex=" + this.PageIndex
                        + "&SelectIndex=" + this.SelectIndex + p.Replace("'", "\\'")/*for navy use only*/
                        + kv/*insert keys and values*/ + wherestr + "&GridViewID=" + GridViewID + "&ItemParam=" + itemparam + "');";

                if (this.ContinueAdd && OpenEditMode == "Insert")
                {
                    Page.Response.Write("<script>" + script + "</script>");
                }
                else
                {
                    Page.Response.Write("<script>" + script + "window.close();</script>");
                }
            }
        }

        private void InitEditMode()
        {
            object obj = this.GetObjByID(this.BindingObject);
            if (obj != null)
            {
                if (!this.ReferenceOnly)
                {
                    #region edit
                    WebDataSource ds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                    ds.SetWhere(KeysValues);
                    this.Page.DataBind();
                    switch (OpenEditMode)
                    {
                        case "Insert":
                            if (obj is WebDetailsView && this.ShowDataStyle == showDataStyle.DetailsView)
                            {
                                WebDetailsView detView = (WebDetailsView)obj;
                                detView.ChangeMode(DetailsViewMode.Insert);
                                SetDetails(detView, "Insert");
                                SyncDetailAdd(detView);
                            }
                            else if (obj is WebFormView && this.ShowDataStyle == showDataStyle.FormView)
                            {
                                WebFormView frmView = (WebFormView)obj;
                                frmView.ChangeMode(FormViewMode.Insert);
                                //SetDetails(frmView, "Insert");
                                SyncDetailAdd(frmView);
                            }
                            break;
                        case "Update":
                            if (obj is WebDetailsView && this.ShowDataStyle == showDataStyle.DetailsView)
                            {
                                WebDetailsView detView = (WebDetailsView)obj;
                                detView.ChangeMode(DetailsViewMode.Edit);
                                SetDetails(detView, "Update");
                                SyncDetailSelect(detView);
                            }
                            else if (obj is WebFormView && this.ShowDataStyle == showDataStyle.FormView)
                            {
                                WebFormView frmView = (WebFormView)obj;
                                frmView.ChangeMode(FormViewMode.Edit);
                                //SetDetails(frmView, "Update");
                                SyncDetailSelect(frmView);
                            }
                            break;
                        case "View":
                            if (obj is WebDetailsView && this.ShowDataStyle == showDataStyle.DetailsView)
                            {
                                WebDetailsView detView = (WebDetailsView)obj;
                                detView.DataBind();
                                detView.ChangeMode(DetailsViewMode.ReadOnly);
                                SyncDetailSelect(detView);
                            }
                            else if (obj is WebFormView && this.ShowDataStyle == showDataStyle.FormView)
                            {
                                WebFormView frmView = (WebFormView)obj;
                                frmView.DataBind();
                                frmView.ChangeMode(FormViewMode.ReadOnly);
                                SyncDetailSelect(frmView);
                            }
                            break;
                        default:
                            return;
                    }
                    #endregion
                }
                else
                {
                    #region ref
                    if (obj is WebGridView)
                    {
                    }
                    #endregion
                }
            }
        }

        private void SyncDetailSelect(object MasterControl)
        {
            if (this.DetailDataSourceID != null && this.DetailDataSourceID != ""
                && this.DetailBindingObject != null && this.DetailBindingObject != ""
                && MasterControl != null)
            {
                WebDataSource detDs = (WebDataSource)this.GetObjByID(this.DetailDataSourceID);
                if (detDs != null)
                {
                    detDs.ExecuteSelect((Control)MasterControl);
                    WebGridView gdView = (WebGridView)this.GetObjByID(this.DetailBindingObject);
                    if (gdView != null)
                    {
                        gdView.DataBind();
                    }
                }
            }
        }

        private void SyncDetailAdd(object MasterControl)
        {
            if (this.DetailDataSourceID != null && this.DetailDataSourceID != ""
                && this.DetailBindingObject != null && this.DetailBindingObject != ""
                && MasterControl != null)
            {
                WebDataSource detDs = (WebDataSource)this.GetObjByID(this.DetailDataSourceID);
                if (detDs != null)
                {
                    detDs.ExecuteAdd((Control)MasterControl);
                    WebGridView gdView = (WebGridView)this.GetObjByID(this.DetailBindingObject);
                    if (gdView != null)
                    {
                        gdView.DataBind();
                    }
                }
            }
        }

        private void SetDetails(WebDetailsView detView, string editMode)
        {
            if (MasterOrDetail == "detail" && RelationFields != "" && RelationValues != "")
            {
                string[] fields = RelationFields.Split(';');
                string[] values = RelationValues.Split(';');
                switch (editMode)
                {
                    case "Insert":
                        #region Insert
                        foreach (DetailsViewRow row in detView.Rows)
                        {
                            object value = null;
                            if (row.RowType == DataControlRowType.DataRow && row.Cells.Count == 2)
                            {
                                // 用HeaderText获取row的FieldName
                                string FieldName = GetNameByHeaderText(detView, row.Cells[0].Text);
                                int i = fields.Length;
                                for (int j = 0; j < i; j++)
                                {
                                    if (fields[j] == FieldName)
                                    {
                                        value = values[j];
                                    }
                                }
                            }
                            foreach (Control ctrl in row.Cells[1].Controls)
                            {
                                if (value != null)
                                {
                                    if (ctrl is TextBox)
                                    {
                                        ((TextBox)ctrl).Text = value.ToString();
                                        ((TextBox)ctrl).Enabled = false;
                                    }
                                    if (ctrl is DropDownList)
                                    {
                                        ((DropDownList)ctrl).SelectedValue = value.ToString();
                                        ((DropDownList)ctrl).Enabled = false;
                                    }
                                    if (ctrl is CheckBox)
                                    {
                                        ((CheckBox)ctrl).Checked = Convert.ToBoolean(value);
                                        ((CheckBox)ctrl).Enabled = false;
                                    }
                                    if (ctrl is WebRefVal)
                                    {
                                        ((WebRefVal)ctrl).BindingValue = value.ToString();
                                        ((WebRefVal)ctrl).Enabled = false;
                                    }
                                }
                            }
                        }
                        #endregion
                        break;
                    case "Update":
                        #region Update
                        foreach (DetailsViewRow row in detView.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow && row.Cells.Count == 2)
                            {
                                // 用HeaderText获取row的FieldName
                                string FieldName = GetNameByHeaderText(detView, row.Cells[0].Text);
                                int i = fields.Length;
                                for (int j = 0; j < i; j++)
                                {
                                    if (fields[j] == FieldName)
                                    {
                                        foreach (Control ctrl in row.Cells[1].Controls)
                                        {
                                            if (ctrl is WebControl)
                                            {
                                                ((WebControl)ctrl).Enabled = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        break;
                }
            }
        }

        private void SetDetails(WebFormView frmView, string editMode)
        {
            if (MasterOrDetail == "detail" && RelationFields != "" && RelationValues != "")
            {
                string[] fields = RelationFields.Split(';');
                string[] values = RelationValues.Split(';');
                switch (editMode)
                {
                    case "Insert":
                        break;
                    case "Update":
                        #region Update
                        #endregion
                        break;
                }
            }
        }

        private string GetNameByHeaderText(WebDetailsView detView, string Header)
        {
            string RetValue = "";
            foreach (DataControlField field in detView.Fields)
            {
                if (field.HeaderText == Header)
                {
                    return field.SortExpression;
                }
            }
            return RetValue;
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

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            bool handled = false;
            if (args is CommandEventArgs)
            {
                CommandEventArgs ce = (CommandEventArgs)args;
                if (ce.CommandName == "OK")
                {
                    OnOKClick(EventArgs.Empty);
                    handled = true;
                }
                else if (ce.CommandName == "Cancel")
                {
                    OnCancelClick(EventArgs.Empty);
                    handled = true;
                }
            }
            return handled;
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoNavigator", "ButtonName", true);
            string[] buttonNames = message.Split(';');
            _OKButton = new Button();
            _OKButton.ID = "btnOK";
            _OKButton.CommandName = "OK";
            if (this.OKButtonCaption == null || this.OKButtonCaption == "")
                _OKButton.Text = buttonNames[0];

            _CancelButton = new Button();
            _CancelButton.ID = "btnCancel";
            _CancelButton.CommandName = "Cancel";
            if (this.CancelButtonCaption == null || this.CancelButtonCaption == "")
                _CancelButton.Text = buttonNames[1];

            this.Controls.Add(_OKButton);
            this.Controls.Add(_CancelButton);
        }

        protected override void OnPreRender(EventArgs e)
        {
            string rscUrl = "../css/controls/WebTranslate.css";
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

        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table); // <table>
            writer.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ButtonContainer");
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Button_mouseout");
            writer.AddAttribute("onmouseout", "this.className='Button_mouseout';");
            writer.AddAttribute("onmouseover", "this.className='Button_mouseover';");
            _OKButton.RenderControl(writer);
            writer.RenderEndTag();// <td>
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ButtonContainer");
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Button_mouseout");
            writer.AddAttribute("onmouseout", "this.className='Button_mouseout';");
            writer.AddAttribute("onmouseover", "this.className='Button_mouseover';");
            _CancelButton.RenderControl(writer);
            writer.RenderEndTag();// <td>
            writer.RenderEndTag();// <tr>
            writer.RenderEndTag();// <table>
        }

        public void doDefOK()
        {
            if (!this.ReferenceOnly)
            {
                object bindObj = this.GetObjByID(this.BindingObject);
                if (bindObj != null)
                {
                    switch (OpenEditMode)
                    {
                        case "Insert":
                            if (bindObj is WebDetailsView && this.ShowDataStyle == showDataStyle.DetailsView)
                            {
                                WebDetailsView detView = (WebDetailsView)bindObj;
                                if (detView.CurrentMode == DetailsViewMode.Insert)
                                    detView.InsertItem(false);
                            }
                            else if (bindObj is WebFormView && this.ShowDataStyle == showDataStyle.FormView)
                            {
                                WebFormView frmView = (WebFormView)bindObj;
                                if (frmView.CurrentMode == FormViewMode.Insert)
                                    frmView.InsertItem(false);
                            }
                            break;
                        case "Update":
                            if (bindObj is WebDetailsView && this.ShowDataStyle == showDataStyle.DetailsView)
                            {
                                WebDetailsView detView = (WebDetailsView)bindObj;
                                if (detView.CurrentMode == DetailsViewMode.Edit)
                                    detView.UpdateItem(false);
                            }
                            else if (bindObj is WebFormView && this.ShowDataStyle == showDataStyle.FormView)
                            {
                                WebFormView frmView = (WebFormView)bindObj;
                                if (frmView.CurrentMode == FormViewMode.Edit)
                                    frmView.UpdateItem(false);
                            }
                            break;
                        case "View":
                            Page.Response.Write("<script>window.close();</script>");
                            return;
                        default:
                            return;
                    }
                }
                ApplyToReturn();
                if (this.ContinueAdd && OpenEditMode == "Insert")
                {
                    (bindObj as WebFormView).ChangeMode(FormViewMode.Insert);
                }
            }
            else
            {
                object obj = this.GetObjByID(this.DataSourceID);
                WebDataSource wds = null;
                WebGridView gdView = null;
                ListBox box = null;
                if (obj != null && obj is WebDataSource)
                {
                    wds = (WebDataSource)obj;
                }
                object bindObj = this.GetObjByID(this.BindingObject);
                if (bindObj != null && bindObj is WebGridView)
                {
                    gdView = (WebGridView)bindObj;
                }
                else if (bindObj != null && bindObj is ListBox)
                {
                    box = (ListBox)bindObj;
                }

                if (wds != null)
                {
                    string script = "";
                    string[] mcs = MatchControls.Split(';');
                    int i = this.RefReturnFields.Count;

                    if (!this.MultiSelectReturnOnly)
                    {
                        DataTable table = null;
                        if (wds.InnerDataSet != null)
                        {
                            table = wds.InnerDataSet.Tables[wds.DataMember];
                        }
                        else
                        {
                            table = wds.CommandTable;
                        }
                        if (i == mcs.Length)
                        {
                            DataRow row = null;
                            if (bindObj is WebGridView && gdView.SelectedRow != null)
                            {
                                if (this.ViewState["SelectedRowKeyFields"] != null)
                                {
                                    row = table.Rows.Find((object[])this.ViewState["SelectedRowKeyFields"]);
                                    this.ViewState["SelectedRowKeyFields"] = null;
                                }
                            }
                            else if (bindObj is ListBox && box.SelectedItem != null)
                            {
                                row = table.Rows[box.SelectedIndex];
                            }

                            if (row != null)
                            {
                                bool colInDs = false;
                                for (int j = 0; j < i; j++)
                                {
                                    foreach (DataColumn column in table.Columns)
                                    {
                                        if (column.ColumnName == this.RefReturnFields[j].FieldName)
                                        {
                                            colInDs = true;
                                        }
                                    }
                                    string v = "";
                                    if (this.RefReturnFields[j].GetDataSourceValue && colInDs)
                                    {
                                        v = row[this.RefReturnFields[j].FieldName].ToString();
                                    }
                                    else
                                    {
                                        v = this.RefReturnFields[j].FieldName;
                                        v = v.Replace("'", "\\'");
                                    }

                                    string temp = mcs[j].Replace('$', '_');
                                    string stemp2 = temp + "_ValueInput";
                                    string stemp3 = temp + "_ValueSelect";
                                    string stemp4 = temp + "_InnerTextBox";

                                    v = v.Replace("'", "\\'");
                                    script +=
                                        "var s=window.opener.document.getElementById('" + mcs[j] + "');" +
                                        "if(s!=undefined)" +
                                        "{" +
                                            "s.value='" + v + "';" +
                                        "}" +
                                        "else" +
                                        "{" +
                                            "s=window.opener.document.getElementById('" + temp + "');" +
                                            "if(s!=undefined && s.value!=undefined)" +
                                            "{" +
                                            "s.value='" + v + "';" +
                                            "}" +
                                            "else" +
                                            "{" +
                                                "s=window.opener.document.getElementById('" + stemp2 + "');" +
                                                "if(s!=undefined)" +
                                                "{" +
                                                    "s.value='" + v + "';" +
                                                    "window.opener.document.getElementById('" + stemp3 + "').value='" + v + "';" +
                                                    "window.opener.document.getElementById('" + stemp4 + "').focus();" +
                                                "}" +
                                            "}" +
                                        "}";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (mcs.Length == 1 && i == 1)
                        {
                            script += "window.opener.document.getElementById('" + mcs[0] + "')" + ".value='" + this.RefReturnFields[0].FieldName + "';";
                        }
                    }
                    this.Page.Response.Write("<script language=javascript>" + script + "window.close();</script>");
                }
            }
        }

        public void SetRefReturn(int index, string value)
        {
            this.RefReturnFields[index].FieldName = value;
        }

        public string GetMultiSelected(string Separator, string KeySeparator)
        {
            string selValues = "", keyValues = "", checkState = "";
            if (Separator == null || Separator == "")
            {
                Separator = ",";
            }
            if (KeySeparator == null || KeySeparator == "")
            {
                KeySeparator = ";";
            }
            object bindObj = this.GetObjByID(this.BindingObject);
            if (bindObj != null && bindObj is WebGridView)
            {
                WebGridView gdView = (WebGridView)bindObj;

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
                            XmlNode nWebGridView = nWebCheckBoxes.SelectSingleNode("WebGridView[@ID='" + gdView.ID + "']");
                            if (nWebGridView != null)
                            {
                                XmlNode nCheckBox = nWebGridView.FirstChild;
                                while (nCheckBox != null)
                                {
                                    keyValues = nCheckBox.Attributes["KeyValues"].Value;
                                    checkState = nCheckBox.InnerText;
                                    if (nCheckBox.Attributes["User"].Value == CliUtils.fLoginUser && checkState == "true")
                                    {
                                        string selValue = "";
                                        string[] arrKeyValues = keyValues.Split(';');
                                        foreach (string keyValue in arrKeyValues)
                                        {
                                            selValue += keyValue + Separator;
                                        }
                                        if (selValue != "")
                                        {
                                            selValue = selValue.Substring(0, selValue.LastIndexOf(Separator));
                                            selValues += selValue + KeySeparator;
                                        }
                                    }
                                    nCheckBox = nCheckBox.NextSibling;
                                }
                                if (selValues != "")
                                {
                                    selValues = selValues.Substring(0, selValues.LastIndexOf(KeySeparator));
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

                                stream.Close();
                                xmlDoc.Save(xmlPath);
                            }
                        }
                    }
                }
            }
            return selValues;
        }

        private void doDefCancel()
        {
            Page.Response.Write("<script>window.close();</script>");
        }

        private void SetWhere(string wheretext)
        {
            if (this.ReferenceOnly && wheretext != "")
            {
                string wherestring = "";
                string strCondition = "";
                string strOperator = "";
                string[] arrwheretext = wheretext.Split(';');
                WebDataSource wds = (WebDataSource)GetObjByID(this.DataSourceID);
                int textcount = arrwheretext.Length;
                int columncount = this.WhereItem.Count;

                string sqlcmd = DBUtils.GetCommandText(wds);
                for (int i = 0; i < textcount && i < columncount; i++)
                {
                    strCondition = ((WebTranslateWhereItem)this.WhereItem[i]).Condition;
                    strOperator = ((WebTranslateWhereItem)this.WhereItem[i]).Operator;
                    if (wherestring == "")
                    {
                        strCondition = "";
                    }

                    if (arrwheretext[i] != string.Empty)
                    {
                        string type = wds.InnerDataSet.Tables[wds.DataMember].Columns[((WebTranslateWhereItem)this.WhereItem[i]).ColumnName].DataType.ToString().ToLower();
                        if (strOperator != "%" && strOperator != "%%")
                        {
                            if (type == "system.uint" || type == "system.uint16" || type == "system.uint32"
                               || type == "system.int64" || type == "system.int" || type == "system.int16"
                               || type == "system.int32" || type == "system.uint64" || type == "system.single"
                               || type == "system.double" || type == "system.decimal")
                            {
                                wherestring += " " + strCondition + CliUtils.GetTableNameForColumn(sqlcmd, ((WebTranslateWhereItem)this.WhereItem[i]).ColumnName)
                                    + strOperator + " " + arrwheretext[i];
                            }
                            else
                            {
                                wherestring += " " + strCondition + CliUtils.GetTableNameForColumn(sqlcmd, ((WebTranslateWhereItem)this.WhereItem[i]).ColumnName)
                                    + strOperator + " '" + arrwheretext[i].Replace("'", "''") + "'";
                            }
                        }
                        else
                        {
                            if (strOperator == "%")
                            {
                                wherestring += " " + strCondition + CliUtils.GetTableNameForColumn(sqlcmd, ((WebTranslateWhereItem)this.WhereItem[i]).ColumnName)
                                    + "like '" + arrwheretext[i].Replace("'", "''") + "%'";
                            }
                            if (strOperator == "%%")
                            {
                                wherestring += " " + strCondition + CliUtils.GetTableNameForColumn(sqlcmd, ((WebTranslateWhereItem)this.WhereItem[i]).ColumnName)
                                    + "like '%" + arrwheretext[i].Replace("'", "''") + "%'";
                            }
                        }
                    }
                }
                if (wherestring != "")
                {
                    wds.SetWhere(wherestring);
                }
                this.Page.DataBind();
            }
        }

        #endregion

        #region Properties
        private RefReturnFieldsCollection _RefReturnFields;
        [Category("Infolight"),
        Description("Specifies the fields which can return. (Reference to the WebRefButton's MatchControls property)"),
        PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        TypeConverter(typeof(CollectionConverter))]
        public RefReturnFieldsCollection RefReturnFields
        {
            get
            {
                return _RefReturnFields;
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
                return "";
            }
            set
            {
                this.ViewState["DataSourceID"] = value;
            }
        }

        [Category("Infolight"),
        Description("The ID of detail WebDataSource which the control is bound to")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string DetailDataSourceID
        {
            get
            {
                object obj = this.ViewState["DetailDataSourceID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["DetailDataSourceID"] = value;
            }
        }

        [Category("Infolight"),
        Description("Specifies the type of BindingObject")]
        public showDataStyle ShowDataStyle
        {
            get
            {
                object obj = this.ViewState["ShowDataStyle"];
                if (obj != null)
                {
                    return (showDataStyle)obj;
                }
                return showDataStyle.FormView;
            }
            set
            {
                this.ViewState["ShowDataStyle"] = value;
            }
        }

        [Browsable(false)]
        public string OpenEditMode
        {
            get
            {
                object obj = this.ViewState["OpenEditMode"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["OpenEditMode"] = value;
            }
        }

        [Browsable(false)]
        public string KeysValues
        {
            get
            {
                object obj = this.ViewState["KeysValues"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["KeysValues"] = value;
            }
        }

        [Browsable(false)]
        public string PageIndex
        {
            get
            {
                object obj = this.ViewState["PageIndex"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["PageIndex"] = value;
            }
        }

        [Browsable(false)]
        public string SelectIndex
        {
            get
            {
                object obj = this.ViewState["SelectIndex"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["SelectIndex"] = value;
            }
        }

        [Browsable(false)]
        public string GridViewID
        {
            get { return (this.ViewState["GridViewID"] == null ? "" : this.ViewState["GridViewID"].ToString()); }
            set { this.ViewState["GridViewID"] = value; }
        }


        [Category("Infolight"),
        Description("Specifies the control to bind to")]
        [Editor(typeof(ViewEditor), typeof(UITypeEditor))]
        public string BindingObject
        {
            get
            {
                object obj = this.ViewState["BindingObject"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["BindingObject"] = value;
            }
        }

        [Category("Infolight"),
        Description("Specifies the detail control to bind to")]
        [Editor(typeof(DetailViewEditor), typeof(UITypeEditor))]
        public string DetailBindingObject
        {
            get
            {
                object obj = this.ViewState["DetailBindingObject"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["DetailBindingObject"] = value;
            }
        }

        [Category("Infolight"),
        Description("Specifies the Controls owned by WebTranslate")]
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        [Category("Infolight"),
        Description("Specifies the caption of the OK button")]
        [Bindable(true)]
        public string OKButtonCaption
        {
            get
            {
                EnsureChildControls();
                return _OKButton.Text;
            }
            set
            {
                EnsureChildControls();
                _OKButton.Text = value;
            }
        }

        [Category("Infolight"),
        Description("Specifies the caption of the cancel button")]
        [Bindable(true)]
        public string CancelButtonCaption
        {
            get
            {
                EnsureChildControls();
                return _CancelButton.Text;
            }
            set
            {
                EnsureChildControls();
                _CancelButton.Text = value;
            }
        }

        [Category("Infolight"),
        Description("Specifies the width of the OK button")]
        public Unit OKButtonWidth
        {
            get
            {
                EnsureChildControls();
                return _OKButton.Width;
            }
            set
            {
                EnsureChildControls();
                _OKButton.Width = value;
            }
        }

        [Category("Infolight"),
        Description("Specifies the height of the OK button")]
        public Unit OKButtonHeigth
        {
            get
            {
                EnsureChildControls();
                return _OKButton.Height;
            }
            set
            {
                EnsureChildControls();
                _OKButton.Height = value;
            }
        }

        [Category("Infolight"),
        Description("Specifies the width of the cancel button")]
        public Unit CancelButtonWidth
        {
            get
            {
                EnsureChildControls();
                return _CancelButton.Width;
            }
            set
            {
                EnsureChildControls();
                _CancelButton.Width = value;
            }
        }

        [Category("Infolight"),
        Description("Specifies the height of the cancel button")]
        public Unit CancelButtonHeight
        {
            get
            {
                EnsureChildControls();
                return _CancelButton.Height;
            }
            set
            {
                EnsureChildControls();
                _CancelButton.Height = value;
            }
        }

        [Browsable(false)]
        public string PagePath
        {
            get
            {
                object obj = this.ViewState["PagePath"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["PagePath"] = value;
            }
        }

        [Browsable(false)]
        public string MasterOrDetail
        {
            get
            {
                object obj = this.ViewState["MasterOrDetail"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["MasterOrDetail"] = value;
            }
        }

        [Browsable(false)]
        public string RelationFields
        {
            get
            {
                object obj = this.ViewState["RelationFields"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["RelationFields"] = value;
            }
        }

        [Browsable(false)]
        public string RelationValues
        {
            get
            {
                object obj = this.ViewState["RelationValues"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["RelationValues"] = value;
            }
        }

        [Category("Infolight")]
        public bool ReferenceOnly
        {
            get
            {
                object obj = this.ViewState["ReferenceOnly"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["ReferenceOnly"] = value;
            }
        }

        [Category("Infolight")]
        public bool MultiSelectReturnOnly
        {
            get
            {
                object obj = this.ViewState["MultiSelectReturnOnly"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["MultiSelectReturnOnly"] = value;
            }
        }

        private WebTranslateWhereItemCollection _whereitem;
        [Category("Infolight"),
        Description("Specifies the columns in where part to get data"),
        PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        TypeConverter(typeof(CollectionConverter))]
        public WebTranslateWhereItemCollection WhereItem
        {
            get
            {
                return _whereitem;
            }
        }

        [Browsable(false)]
        public bool AllowToReturn
        {
            get
            {
                object obj = this.ViewState["AllowToReturn"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["AllowToReturn"] = value;
            }
        }

        [Browsable(false)]
        public string Paramters
        {
            get
            {
                object obj = this.ViewState["Paramters"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["Paramters"] = value;
            }
        }

        [Browsable(false)]
        public string InsertKeyValues
        {
            get
            {
                object obj = this.ViewState["InsertKeyValues"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["InsertKeyValues"] = value;
            }
        }

        [Browsable(false)]
        public string MatchControls
        {
            get
            {
                object obj = this.ViewState["MatchControls"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["MatchControls"] = value;
            }
        }

        [Category("Infolight")]
        public bool ContinueAdd
        {
            get
            {
                object obj = this.ViewState["ContinueAdd"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["ContinueAdd"] = value;
            }
        }

        [Browsable(false)]
        public int SelectedRowIndex
        {
            get 
            {
                return this.ViewState["SelectedRowIndex"] == null ? -1 : (int)this.ViewState["SelectedRowIndex"];
            }
            set
            {
                this.ViewState["SelectedRowIndex"] = value;
            }

        }


        #endregion




        #region Event
        [Category("Action")]
        public event EventHandler OKClick
        {
            add { Events.AddHandler(EventOKClick, value); }
            remove { Events.RemoveHandler(EventOKClick, value); }
        }

        private static readonly object EventOKClick = new object();

        protected virtual void OnOKClick(EventArgs e)
        {
            EventHandler OKClickHandler = (EventHandler)Events[EventOKClick];
            if (OKClickHandler != null)
            {
                OKClickHandler(this, e);
            }
            doDefOK();
        }

        [Category("Action")]
        public event EventHandler CancelClick
        {
            add { Events.AddHandler(EventCancelClick, value); }
            remove { Events.RemoveHandler(EventCancelClick, value); }
        }

        private static readonly object EventCancelClick = new object();

        protected virtual void OnCancelClick(EventArgs e)
        {
            EventHandler CancelClickHandler = (EventHandler)Events[EventCancelClick];
            if (CancelClickHandler != null)
            {
                CancelClickHandler(this, e);
            }
            doDefCancel();
        }
        #endregion
    }

    public class WebTranslateWhereItemCollection : InfoOwnerCollection
    {
        public WebTranslateWhereItemCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebTranslateWhereItem))
        {
        }

        public new WebTranslateWhereItem this[int index]
        {
            get
            {
                return (WebTranslateWhereItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebTranslateWhereItem)
                    {
                        //原来的Collection设置为0
                        ((WebTranslateWhereItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebTranslateWhereItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class WebTranslateWhereItem : InfoOwnerCollectionItem, IGetValues
    {
        public WebTranslateWhereItem()
        {
            _columnname = "";
            _operator = "=";
            _condition = "And";
        }

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

        private string _columnname;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ColumnName
        {
            get
            {
                return _columnname;
            }
            set
            {
                _columnname = value;
                if (value != null && value != "")
                {
                    _name = value;
                }
            }
        }

        private string _operator;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
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
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
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

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is WebTranslate)
            {
                if (string.Compare(sKind, "columnname", true) == 0)//IgnoreCase
                {
                    WebTranslate tran = (WebTranslate)this.Owner;
                    object o = tran.GetObjByID(tran.DataSourceID);
                    if (o != null && o is WebDataSource)
                    {
                        WebDataSource ds = (WebDataSource)o;
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

    public enum showDataStyle
    {
        DetailsView = 0,
        FormView = 1,
        GridView = 2,
        ListBoxList = 3
    }

    public class RefReturnFieldsCollection : InfoOwnerCollection
    {
        public RefReturnFieldsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(RefReturnField))
        {
        }

        public new RefReturnField this[int index]
        {
            get
            {
                return (RefReturnField)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is RefReturnField)
                    {
                        //原来的Collection设置为0
                        ((RefReturnField)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((RefReturnField)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class RefReturnField : InfoOwnerCollectionItem, IGetValues
    {
        public RefReturnField()
        {
            _GetDataSourceValue = true;
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
        [NotifyParentProperty(true),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
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

        private bool _GetDataSourceValue;
        [NotifyParentProperty(true)]
        public bool GetDataSourceValue
        {
            get
            {
                return _GetDataSourceValue;
            }
            set
            {
                _GetDataSourceValue = value;
            }
        }

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is WebTranslate)
            {
                WebTranslate tran = (WebTranslate)this.Owner;
                if (string.Compare(sKind, "fieldname", true) == 0)//IgnoreCase
                {
                    object o = tran.GetObjByID(tran.DataSourceID);
                    if (o != null && o is WebDataSource)
                    {
                        WebDataSource ds = (WebDataSource)o;
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

        #endregion
    }

    #region ViewEditor
    public class ViewEditor : UITypeEditor
    {
        public ViewEditor()
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
            if (context.Instance != null)
            {
                ControlCollection ctrlList = ((Control)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDetailsView || ctrl is WebFormView || ctrl is WebGridView || ctrl is ListBox)
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

    public class DetailViewEditor : UITypeEditor
    {
        public DetailViewEditor()
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
            if (context.Instance != null)
            {
                ControlCollection ctrlList = ((Control)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebGridView)
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
