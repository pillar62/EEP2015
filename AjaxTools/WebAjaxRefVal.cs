using System;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Web.UI;
using Srvtools;
using System.Web;
using System.Security.Permissions;
using System.Drawing.Design;
using System.Data;
using System.Text;
using System.Collections;
using System.Resources;
using System.Xml;
using System.Reflection;
using System.Drawing;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;

[assembly: System.Web.UI.WebResource("AjaxTools.AjaxRefVal.js", "text/javascript")]
namespace AjaxTools
{
    public class WebAjaxRefVal : WebRefValBase, ICallbackEventHandler, IAjaxDataSource, INamingContainer
    {
        public WebAjaxRefVal()
        {
            _whereItem = new WebWhereItemCollection(this, typeof(WebWhereItem));
            _columns = new ColumnCollection(this, typeof(WebRefColumn));
            _columnMatch = new WebColumnMatchCollection(this, typeof(WebColumnMatch));
        }

        //private Button bt = new Button();
        private TextBox _txtValue = new TextBox();
        private TextBox _txtDisplay = new TextBox();
        private DropDownList _ddlSelect = new DropDownList();
        private TextBox _txtShow = new TextBox();
        private ImageButton _btnImage = new ImageButton();
        private Button _hidTarget = new Button();

        // popup window
        private Panel _popupContainer = new Panel();
        private Panel _panTitle = new Panel();
        private Button _hidCloseTarget = new Button();
        private ImageButton _btnClose = new ImageButton();
        private UpdatePanel _updatePanel = new UpdatePanel();

        private GridView _grid = new GridView();

        #region Properties
        [Bindable(true)]
        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public override string BindingValue
        //{
        //    get
        //    {
        //        EnsureChildControls();
        //        return _txtValue.Text;
        //    }
        //    set
        //    {
        //        EnsureChildControls();
        //        _txtValue.Text = value;
        //    }
        //}
        public override string BindingValue
        {
            get
            {
                object obj = this.ViewState["BindingValue"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["BindingValue"] = value;
            }
        }

        [Category("Infolight")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public override string DataSourceID
        {
            get
            {
                object obj = this.ViewState["DataSourceID"];
                if (obj != null)
                    return (string)obj;
                return "";
            }
            set
            {
                this.ViewState["DataSourceID"] = value;
            }
        }

        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public override string DataTextField
        {
            get
            {
                object obj = this.ViewState["DataTextField"];
                if (obj != null)
                    return (string)obj;
                return "";
            }
            set
            {
                this.ViewState["DataTextField"] = value;
            }
        }

        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public override string DataValueField
        {
            get
            {
                object obj = this.ViewState["DataValueField"];
                if (obj != null)
                    return (string)obj;
                return "";
            }
            set
            {
                this.ViewState["DataValueField"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
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

        [DefaultValue(typeof(Unit), "120px")]
        public override Unit Width
        {
            get
            {
                object obj = this.ViewState["Width"];
                if (obj != null)
                {
                    return (Unit)obj;
                }
                return new Unit(120, UnitType.Pixel);
            }
            set
            {
                this.ViewState["Width"] = value;
            }
        }

        [DefaultValue(typeof(Unit), "16px")]
        public override Unit Height
        {
            get
            {
                object obj = this.ViewState["Height"];
                if (obj != null)
                {
                    return (Unit)obj;
                }
                return new Unit(16, UnitType.Pixel);
            }
            set
            {
                this.ViewState["Height"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(450)]
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

        [Category("Infolight")]
        [DefaultValue(510)]
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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string QueryCondition
        {
            get
            {
                object obj = this.ViewState["QueryCondition"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["QueryCondition"] = value;
            }
        }

        private WebWhereItemCollection _whereItem;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [TypeConverter(typeof(CollectionConverter))]
        public WebWhereItemCollection WhereItem
        {
            get
            {
                return _whereItem;
            }
        }

        private ColumnCollection _columns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [TypeConverter(typeof(CollectionConverter))]
        public ColumnCollection Columns
        {
            get
            {
                return _columns;
            }
        }

        private WebColumnMatchCollection _columnMatch;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [TypeConverter(typeof(CollectionConverter))]
        public WebColumnMatchCollection ColumnMatch
        {
            get
            {
                return _columnMatch;
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

        #endregion

        #region ICallbackEventHandler
        private string callBackRetVal = "";
        public string GetCallbackResult()
        {
            return callBackRetVal;
        }
        public void RaiseCallbackEvent(string eventArgument)
        {

        }
        #endregion

        public override void DataBind()
        {
            if (!this.DesignMode)
            {
                WebDataSource wds = this.GetDataSource();
                string sql = this.GetWhereItem();
                wds.SetWhere(sql);
            }
            base.DataBind();
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

        protected override void CreateChildControls()
        {
            Controls.Clear();
            //bt = new Button();
            //bt.ID = "bt";
            //this.Controls.Add(bt);
            //_txtValue
            _txtValue = new TextBox();
            _txtValue.ID = "txtValue";
            _txtValue.TextChanged += new EventHandler(_txtValue_TextChanged);
            this.Controls.Add(_txtValue);
            //_txtDisplay
            _txtDisplay = new TextBox();
            _txtDisplay.ID = "txtDisplay";
            this.Controls.Add(_txtDisplay);
            //_ddlSelect
            _ddlSelect = new DropDownList();
            _ddlSelect.ID = "ddlSelect";
            _ddlSelect.AppendDataBoundItems = true;
            _ddlSelect.DataSourceID = this.DataSourceID;
            _ddlSelect.DataTextField = this.DataTextField;
            _ddlSelect.DataValueField = this.DataValueField;
            this.Controls.Add(_ddlSelect);
            //_txtShow
            _txtShow = new TextBox();
            _txtShow.ID = "txtShow";
            _txtShow.BackColor = this.BackColor;
            this.Controls.Add(_txtShow);

            //_btnImage
            _btnImage = new ImageButton();
            _btnImage.ID = "btnImage";
            _btnImage.Click += new ImageClickEventHandler(_btnImage_Click);
            this.Controls.Add(_btnImage);

            if (!this.DesignMode)
            {
                //_hidTarget
                _hidTarget = new Button();
                _hidTarget.ID = "hidTarget";
                _hidTarget.UseSubmitBehavior = false;
                this.Controls.Add(_hidTarget);

                //_popupContainer
                _popupContainer = new Panel();
                _popupContainer.ID = "popupContainer";
                this.Controls.Add(_popupContainer);

                //_panTitle
                _panTitle = new Panel();
                _panTitle.ID = "panTitle";
                this.Controls.Add(_panTitle);

                //_btnClose
                _btnClose = new ImageButton();
                _btnClose.ID = "btnClose";
                _btnClose.ImageUrl = "~/Image/Ajax/close.gif";
                _btnClose.Click += new ImageClickEventHandler(_btnClose_Click);
                this.Controls.Add(_btnClose);

                //_hidCloseTarget
                _hidCloseTarget = new Button();
                _hidCloseTarget.UseSubmitBehavior = false;
                _hidCloseTarget.ID = "hidCloseTarget";
                this.Controls.Add(_hidCloseTarget);

                //Table
                Table _innerTable = new Table();
                //Row2
                TableRow _innerRow3 = new TableRow();
                TableCell _innerCell31 = new TableCell();
                _innerCell31.ColumnSpan = 3;
                _grid = new GridView();
                _grid.ID = "gridView";
                _grid.DataSourceID = this.DataSourceID;
                _grid.AllowPaging = true;
                _grid.ShowFooter = true;
                _grid.SkinID = "InnerGridViewSkin1";
                this.ShowFields(_grid);
                _grid.RowCommand += new GridViewCommandEventHandler(_grid_RowCommand);
                _grid.PageIndexChanging += new GridViewPageEventHandler(_grid_PageIndexChanging);
                _innerCell31.Controls.Add(_grid);
                _innerRow3.Cells.Add(_innerCell31);

                //_updatePanel
                _updatePanel = new UpdatePanel();
                _updatePanel.ID = "updateContent";
                _updatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
                _updatePanel.ChildrenAsTriggers = true;
                _updatePanel.EnableViewState = true;
                _updatePanel.ContentTemplateContainer.Controls.Add(_innerTable);
                AsyncPostBackTrigger triggerOpen = new AsyncPostBackTrigger();
                triggerOpen.ControlID = this._btnImage.UniqueID;
                triggerOpen.EventName = "Click";
                _updatePanel.Triggers.Add(triggerOpen);
                AsyncPostBackTrigger triggerClose = new AsyncPostBackTrigger();
                triggerClose.ControlID = this._btnClose.UniqueID;
                triggerClose.EventName = "Click";
                _updatePanel.Triggers.Add(triggerClose);
                this.Controls.Add(_updatePanel);
            }
        }

        void _txtValue_TextChanged(object sender, EventArgs e)
        {
            this.BindingValue = _txtValue.Text;
        }

        private Table CreateQueryTable(DataTable tempTab)
        {
            Table _tabQuery = new Table();
            return _tabQuery;
        }

        void _grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        void _grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        void _btnClose_Click(object sender, ImageClickEventArgs e)
        {
        }

        void _btnImage_Click(object sender, ImageClickEventArgs e)
        {

        }

        private string GetDisplayByValue(string value)
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

        protected override void Render(HtmlTextWriter writer)
        {
            ClientScriptManager csm = Page.ClientScript;

            writer.AddAttribute("ID", this.ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table); // <table>

            writer.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td> value
            //writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "0px");
            //bt.RenderControl(writer);
            #region txtValue
            if (!this.DesignMode)
            {
                if (this.Page.Session[this.ClientID + "TempRefVal"] != null)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Page.Session[this.ClientID + "TempRefVal"].ToString());
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, this.BindingValue);
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxref_nondisplay");
                _txtValue.RenderControl(writer);
            }
            #endregion
            writer.RenderEndTag();  // </td>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td> display
            #region txtDisplay
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxref_nondisplay");
                _txtDisplay.RenderControl(writer);
            }
            #endregion
            writer.RenderEndTag();  // </td>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td> select
            #region ddlSelect
            bool itemExist = false;
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxref_nondisplay");
                if (this.BindingValue != "")
                {
                    for (int i = 0; i < _ddlSelect.Items.Count; i++)
                    {
                        if (_ddlSelect.Items[i].Value == this.BindingValue)
                        {
                            _ddlSelect.SelectedValue = this.BindingValue;
                            itemExist = true;
                            break;
                        }
                    }
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Value, this.BindingValue);
                _ddlSelect.RenderControl(writer);
            }
            #endregion
            writer.RenderEndTag();  // </td>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td> show
            if (!this.DesignMode && _ddlSelect.SelectedItem != null)
            {
                //∏≥≥ı÷µ
                if (itemExist)
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, this.BindingValue == "" ? "" : this._ddlSelect.SelectedItem.Text);
                else
                {
                    if (_txtDisplay.Text != null && _txtDisplay.Text != "")
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Value, _txtDisplay.Text);
                    }
                    else
                    {
                        if (this.Page.Session[this.ClientID + "TempRefDis"] != null)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Page.Session[this.ClientID + "TempRefDis"].ToString());
                        }
                        else
                        {
                            string dbv = GetDisplayByValue(this.BindingValue);
                            writer.AddAttribute(HtmlTextWriterAttribute.Value, (string.IsNullOrEmpty(dbv) ? this.BindingValue : dbv));
                            //writer.AddAttribute(HtmlTextWriterAttribute.Value, this.BindingValue);
                        }
                    }
                }

                #region changeBehavior
                string changeScript = string.Format("var ctrls={{ddl:'{0}',val:'{1}',txt:'{2}',show:'{3}'}};_refChangeField(ctrls,{4},{5});",
                                                    _ddlSelect.ClientID,
                                                    _txtValue.ClientID,
                                                    _txtDisplay.ClientID,
                                                    _txtShow.ClientID,
                                                    this.AutoUpperCase.ToString().ToLower(),
                                                    this.CheckData.ToString().ToLower());
                writer.AddAttribute("onchange", changeScript, true);
                #endregion

                #region blurBehavior
                string dbAlias = this.GetDBAlias();
                string commandText = this.GetCommandText();
                string message = "'aaa'"; SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebRefVal", "CheckDataMessage", true);
                string callbackScript = "_refReceiveServerData";
                string valArg = this.AutoUpperCase ? _txtShow.ClientID + ".value.toUpperCase()" : _txtShow.ClientID + ".value";
                string blurContext = string.Format("{{ddl:'{0}',val:'{1}',txt:'{2}',show:'{3}',ref:'{4}',autoUpperCase:{5},checkData:{6},msg:{7}}}",
                                                   _ddlSelect.ClientID,
                                                   _txtValue.ClientID,
                                                   _txtDisplay.ClientID,
                                                   _txtShow.ClientID,
                                                   this.ClientID,
                                                   this.AutoUpperCase.ToString().ToLower(),
                                                   this.CheckData.ToString().ToLower(),
                                                   string.Format(message, _txtShow.ClientID));
                string blurScript = csm.GetCallbackEventReference(this, valArg, callbackScript, blurContext, true);
                writer.AddAttribute("onblur", blurScript, true);
                #endregion

                #region focusBehavior
                string focusScript = string.Format("var ctrls={{show:'{0}',val:'{1}'}};_refFocus(ctrls);",
                                   this._txtShow.ClientID,
                                   this._txtValue.ClientID);
                writer.AddAttribute("onfocus", focusScript, true);
                #endregion

                this.Page.Session.Remove(this.ClientID + "TempRefVal");
                this.Page.Session.Remove(this.ClientID + "TempRefDis");
            }
            writer.AddStyleAttribute("width", this.Width.Value.ToString() + "px");
            writer.AddStyleAttribute("height", this.Height.Value.ToString() + "px");
            _txtShow.RenderControl(writer);
            writer.RenderEndTag();  // </td>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td> image
            writer.AddAttribute("src", "../Image/refval/RefVal.gif");
            writer.AddStyleAttribute("border", "thin outset");
            writer.AddStyleAttribute("width", "25px");
            writer.AddStyleAttribute("background-color", "buttonface");
            //string clickScript = "$find('" + this.ClientID + "_refvalShowModalBehavior').show();";
            StringBuilder refVal = new StringBuilder();

            refVal.Append("var param = new setRefvalparam();");
            refVal.AppendFormat("param.refvalID = \"{0}\";", this.ClientID);
            refVal.AppendFormat("param.refMethorControlID = \"{0}\";", this.ClientID.Replace(this.ID, "").Replace("_", ""));
            //refVal.AppendFormat("param.refTitle = \"{0}\";", this.RefValTitle);
            refVal.Append("param.refDynamic = \"Y\";");
            object o = this.GetObjByID(this.DataSourceID);
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    refVal.AppendFormat("param.refCmdsql = \"{0}\";", wds.SelectCommand);
                }

                WebDataSet wd = wds.DataSource as WebDataSet;
                if (wd != null && !String.IsNullOrEmpty(wd.RemoteName))
                {
                    refVal.AppendFormat("param.refModuleName = \"{0}\";", wd.RemoteName.Split('.')[0]);
                    refVal.AppendFormat("param.refCommandName = \"{0}\";", wd.RemoteName.Split('.')[1]);

                }
                else
                {
                    refVal.Append("param.refModuleName = \"GLModule\";");
                    refVal.Append("param.refCommandName = \"cmdRefValUse\";");
                }
            }
            String ShowFields = String.Empty;
            String ShowFieldsWidth = String.Empty;
            foreach (WebRefColumn column in this.Columns)
            {
                if (ShowFields != String.Empty)
                    ShowFields += ",";
                if (ShowFieldsWidth != String.Empty)
                    ShowFieldsWidth += ",";
                ShowFields += column.ColumnName;
                ShowFieldsWidth += column.Width;
            }
            refVal.AppendFormat("param.refShowFields = \"{0}\";", ShowFields);
            refVal.AppendFormat("param.refShowFieldsWidth = \"{0}\";", ShowFieldsWidth);
            refVal.AppendFormat("param.refBindControlID = \"{0}\";", this.ID);
            refVal.AppendFormat("param.refBindValueColumn = \"{0}\";", this.DataValueField);
            refVal.AppendFormat("param.refBindTextColumn = \"{0}\";", this.DataTextField);
            String strMatchSrcColumns = String.Empty;
            String strMatchDestcontrolIDs = String.Empty;
            foreach (WebColumnMatch column in this.ColumnMatch)
            {
                if (strMatchSrcColumns != String.Empty)
                    strMatchSrcColumns += ",";
                if (strMatchDestcontrolIDs != String.Empty)
                    strMatchDestcontrolIDs += ",";
                strMatchSrcColumns += "\'" + column.SrcField + "\'";
                strMatchDestcontrolIDs += "\'" + column.DestControlID + "\'";
            }
            if (strMatchSrcColumns != String.Empty && strMatchDestcontrolIDs != String.Empty)
            {
                refVal.AppendFormat("param.refMatchSrcColumns = [{0}];", strMatchSrcColumns);
                refVal.AppendFormat("param.refMatchDestcontrolIDs = [{0}];", strMatchDestcontrolIDs);
            }
            String strFilterColumns = String.Empty;
            String strFilterControls = String.Empty;
            foreach (WebWhereItem column in this.WhereItem)
            {
                if (strFilterColumns != String.Empty)
                    strFilterColumns += "|";
                if (strFilterControls != String.Empty)
                    strFilterControls += "|";
                strFilterColumns += "\'" + column.FieldName + "\'";
                strFilterControls += "\'" + column.Value + "\'";
            }
            if (strFilterColumns != String.Empty && strFilterControls != String.Empty)
            {
                refVal.AppendFormat("param.refFilterColumns = [{0}];", strFilterColumns);
                refVal.AppendFormat("param.refFilterControls = [{0}];", strFilterControls);
            }
            refVal.Append("param.refAutoShow = true;");
            refVal.Append("var GexRef_ID = createGexRef(param);");
            refVal.Append("return GexRef_ID;");

            writer.AddAttribute("onclick", refVal.ToString());

            _btnImage.RenderControl(writer);
            writer.RenderEndTag();  // </td> 

            writer.RenderEndTag();  // </tr>

            writer.RenderEndTag();  // </table>

            if (!this.DesignMode)
            {
                // HidenTarget
                writer.AddStyleAttribute("display", "none");
                _hidTarget.RenderControl(writer);

                //RenderPopup
                writer.AddStyleAttribute("background-color", "#buttonface");
                writer.AddStyleAttribute("border-width", "3px");
                writer.AddStyleAttribute("border-style", "solid");
                writer.AddStyleAttribute("border-color", "Gray");
                writer.AddStyleAttribute("padding", "3px");
                writer.AddStyleAttribute("width", this.OpenRefWidth.ToString() + "px");
                writer.AddStyleAttribute("display", "none");
                _popupContainer.RenderBeginTag(writer);

                //title
                writer.AddAttribute("id", "divTitle");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxref_div_title");
                _panTitle.RenderBeginTag(writer);
                writer.AddStyleAttribute("cursor", "pointer");
                writer.AddStyleAttribute("position", "relative");
                writer.AddStyleAttribute("top", "3px");
                _btnClose.RenderControl(writer);
                writer.AddStyleAttribute("display", "none");
                _hidCloseTarget.RenderControl(writer);
                _panTitle.RenderEndTag(writer);

                //content
                writer.AddAttribute("id", "divContent");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxref_div_content");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                _updatePanel.RenderControl(writer);
                writer.RenderEndTag();

                _popupContainer.RenderEndTag(writer);
            }
        }

        public WebDataSource GetDataSource()
        {
            return GetObjByID(this.DataSourceID) as WebDataSource;
        }
        public string GetDBAlias()
        {
            WebDataSource wds = this.GetDataSource();
            if (wds != null && wds.SelectAlias != null && wds.SelectAlias != "")
            {
                return wds.SelectAlias;
            }
            return "";
        }
        public string GetCommandText()
        {
            WebDataSource wds = this.GetDataSource();
            if (wds != null && wds.SelectCommand != null && wds.SelectCommand != "")
            {
                return wds.SelectCommand;
            }
            return "";
        }
        private string GetRemoteName(string WebDataSetID)
        {
            string remoteName = "";
            XmlDocument xmlDoc = new XmlDocument();
            string aspxName = this.Page.MapPath(this.Page.Request.Path);
            string resourceName = aspxName + @".vi-VN.resx";
            ResXResourceReader reader = new ResXResourceReader(resourceName);
            IDictionaryEnumerator enumerator = reader.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString() == "WebDataSets")
                {
                    string sXml = (string)enumerator.Value;
                    xmlDoc.LoadXml(sXml);
                    break;
                }
            }
            if (xmlDoc != null)
            {
                XmlNode nWDSs = xmlDoc.SelectSingleNode("WebDataSets");
                if (nWDSs != null)
                {
                    string webDataSetID = WebDataSetID;
                    XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSet[@Name='" + webDataSetID + "']");
                    if (nWDS != null)
                    {
                        XmlNode nRemoteName = nWDS.SelectSingleNode("RemoteName");
                        if (nRemoteName != null)
                            remoteName = nRemoteName.InnerText;
                    }
                }
            }
            return remoteName;
        }
        private DataTable GetRefTable()
        {
            WebDataSource wds = this.GetDataSource();
            if (wds == null) return null;
            string dbAlias = this.GetDBAlias();
            string cmdText = this.GetCommandText();

            if (dbAlias == "" || cmdText == "")
            {
                return wds.InnerDataSet.Tables[0];
            }
            else
            {
                return wds.CommandTable;
            }
        }

        /// <summary>
        /// get where item value
        /// </summary>
        /// <param name="value">property value</param>
        /// <returns>formatted value</returns>
        public string GetValue(string value)
        {
            Char[] cs = value.ToCharArray();
            object[] myret = CliUtils.GetValue(value);
            if (myret != null && (int)myret[0] == 0)
            {
                return (string)myret[1];
            }
            if (cs[0] != '"' && cs[0] != '\'')
            {
                Char[] sep1 = "()".ToCharArray();
                String[] sps1 = value.Split(sep1);

                if (sps1.Length == 3)
                {
                    return InvokeOwerMethod(sps1[0], null);
                }

                if (sps1.Length == 1)
                {
                    return sps1[0];
                }

                if (sps1.Length != 1 && sps1.Length == 3)
                {
                    System.Windows.Forms.MessageBox.Show("The 'Value' property's style is not supported");
                }
            }
            Char[] sep2 = null;
            if (cs[0] == '"')
            {
                sep2 = "\"".ToCharArray();
            }

            if (cs[0] == '\'')
            {
                sep2 = "'".ToCharArray();
            }

            String[] sps2 = value.Split(sep2);
            if (sps2.Length == 3)
            { return sps2[1]; }
            else
            {
                System.Windows.Forms.MessageBox.Show("The 'Value' property's style is not supported");
                return "";
            }
        }
        private String InvokeOwerMethod(String methodName, Object[] parameters)
        {
            MethodInfo methodInfo = Page.GetType().GetMethod(methodName);

            Object obj = null;
            if (methodInfo != null)
            {
                obj = methodInfo.Invoke(Page, parameters);
            }

            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// get where sql string
        /// </summary>
        /// <returns>sql string</returns>
        public string GetWhereItem()
        {
            WebDataSource wds = this.GetDataSource();
            string dbAlias = this.GetDBAlias();
            string commandText = this.GetCommandText();
            string whereitem = "";
            foreach (WebWhereItem item in this.WhereItem)
            {
                Type type;
                if (dbAlias == "" || commandText == "")
                {
                    type = wds.InnerDataSet.Tables[wds.DataMember].Columns[item.FieldName].DataType;
                }
                else
                {
                    type = wds.CommandTable.Columns[item.FieldName].DataType;
                }
                string val = this.GetValue(item.Value);
                if (item.Condition != "%" && item.Condition != "%%")
                {
                    if (GloFix.IsNumeric(type))
                    {
                        whereitem += item.FieldName + item.Condition + val + " and ";
                    }
                    else
                    {
                        whereitem += item.FieldName + item.Condition + "'" + val + "' and ";
                    }
                }
                else
                {
                    if (item.Condition == "%")
                    {
                        whereitem += item.FieldName + " like '" + val + "%' and ";
                    }
                    else if (item.Condition == "%%")
                    {
                        whereitem += item.FieldName + " like '%" + val + "%' and ";
                    }
                }
            }
            if (whereitem != "")
            {
                whereitem = whereitem.Substring(0, whereitem.LastIndexOf(" and "));
            }
            return whereitem;
        }

        private void ShowFields(GridView grid)
        {
            TemplateField field = new TemplateField();
            //AjaxRefValTemplateField field = new AjaxRefValTemplateField();
            field.ItemTemplate = new GridViewSelectTemplate(DataControlRowType.DataRow, "SelectRefValField");
            field.FooterTemplate = new GridViewSelectTemplate(DataControlRowType.Footer, "SelectRefValField");
            grid.Columns.Add(field);
            //CommandField cmdField = new CommandField();
            //cmdField.ShowSelectButton = true;
            //cmdField.ShowEditButton = false;
            //cmdField.ShowInsertButton = false;
            //grid.Columns.Add(cmdField);
            if (this.Columns.Count != 0)
            {
                grid.AutoGenerateColumns = false;
                foreach (WebRefColumn column in this.Columns)
                {
                    BoundField boundField = new BoundField();
                    boundField.DataField = column.ColumnName;
                    boundField.HeaderText = column.HeadText;
                    boundField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    boundField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    boundField.ItemStyle.Width = new Unit(column.Width, UnitType.Pixel);
                    grid.Columns.Add(boundField);
                }
            }
            else
            {
                grid.AutoGenerateColumns = true;
            }
        }

        private string GetColumnMatchScript(DataRow matchRow)
        {
            string containerId = this.ClientID.Substring(0, this.ClientID.LastIndexOf('_') + 1);
            int i = 0;
            string script = "";
            foreach (WebColumnMatch cm in this.ColumnMatch)
            {
                object value = CliUtils.GetValue(cm.SrcGetValue, this.Page);
                string strValue = "";
                if (value != DBNull.Value)
                {
                    strValue = value.ToString();
                }
                else
                {
                    strValue = matchRow[cm.SrcField].ToString();
                }
                script +=
                    "var matchControl" + i.ToString() + "=$get('" + containerId + cm.DestControlID + "');" +
                    "if(matchControl" + i.ToString() + "==null){" +
                        "if($get('" + containerId + cm.DestControlID + "_txtValue')!=null){" +
                            "$get('" + containerId + cm.DestControlID + "_txtValue').value='" + strValue + "';" +
                            "$get('" + containerId + cm.DestControlID + "_txtShow').value='" + strValue + "';" +
                    //"$get('" + containerId + cm.DestControlID + "_txtShow').focus();" +
                        "}" +
                        "else{" +
                            "matchControl" + i.ToString() + ".value='" + strValue + "';" +
                    //"matchControl" + i.ToString() + ".focus();" +
                        "}" +
                    "}" +
                    "else{" +
                        "matchControl" + i.ToString() + ".value='" + strValue + "';" +
                    //"matchControl" + i.ToString() + ".focus();" +
                    "}";
                i++;
            }
            return script;
        }
    }
}
