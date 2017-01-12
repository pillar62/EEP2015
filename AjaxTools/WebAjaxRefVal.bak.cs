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
        private ModalPopupExtender _modalPopupExtender = new ModalPopupExtender();

        // popup window
        private Panel _popupContainer = new Panel();
        private Panel _panTitle = new Panel();
        private Button _hidCloseTarget = new Button();
        private ImageButton _btnClose = new ImageButton();
        private UpdatePanel _updatePanel = new UpdatePanel();

        //query
        private TableRow _innerRow1;
        private TableRow _innerRow2;
        private DropDownList[] dropDownLists;
        private TextBox[] textBoxes;
        private string[] queryFields;
        private string[] dataTypes;

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

        //[Category("Infolight")]
        //public override ControlCollection Controls
        //{
        //    get
        //    {
        //        EnsureChildControls();
        //        return base.Controls;
        //    }
        //}
        #endregion

        #region ICallbackEventHandler
        private string callBackRetVal = "";
        public string GetCallbackResult()
        {
            return callBackRetVal;
        }
        public void RaiseCallbackEvent(string eventArgument)
        {
            string dbAlias = GetDBAlias(), commandText = GetCommandText();
            if (dbAlias == "" || commandText == "")
            {
                string sCurProject = CliUtils.fCurrentProject, strModuleName = "", strSourceTab = "", tabName = "", strSql = "";
                WebDataSource ds = this.GetDataSource();
                int i = ds.InnerDataSet.Tables[ds.DataMember].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (eventArgument == ds.InnerDataSet.Tables[ds.DataMember].Rows[j][this.DataValueField].ToString())
                    {
                        callBackRetVal = ds.InnerDataSet.Tables[ds.DataMember].Rows[j][this.DataTextField].ToString() + ";" + this.GetColumnMatchScript(ds.InnerDataSet.Tables[ds.DataMember].Rows[j]);
                        return;
                    }
                }
                //在原有DataSource的PacketRecords之外，则必须下sql语句将此笔资料找出
                strModuleName = this.GetRemoteName(ds.WebDataSetID);
                strModuleName = strModuleName.Substring(0, strModuleName.IndexOf('.'));
                strSourceTab = ds.DataMember;
                string[] quote = CliUtils.GetDataBaseQuote();
                tabName = CliUtils.GetTableName(strModuleName, strSourceTab, sCurProject, "", true);
                string sqlcmd = CliUtils.GetSqlCommandText(strModuleName, strSourceTab, sCurProject);

                if (GloFix.IsNumeric(ds.InnerDataSet.Tables[ds.DataMember].Columns[this.DataValueField].DataType))
                {
                    if (eventArgument != null && eventArgument != "")
                        //strSql = "select * from " + tabName + " where " + CliUtils.GetTableNameForColumn(sqlcmd, this.DataValueField, tabName, quote) + " = " + eventArgument;
                        strSql = "select * from " + tabName + " where " + CliUtils.GetTableNameForColumn(sqlcmd, this.DataValueField) + " = " + eventArgument;
                }
                else
                {
                    //strSql = "select * from " + tabName + " where " + CliUtils.GetTableNameForColumn(sqlcmd, this.DataValueField, tabName, quote) + " = '" + eventArgument + "'";
                    strSql = "select * from " + tabName + " where " + CliUtils.GetTableNameForColumn(sqlcmd, this.DataValueField) + " = '" + eventArgument + "'";
                }
                DataSet dset = CliUtils.ExecuteSql(strModuleName, strSourceTab, strSql, true, sCurProject);
                if (dset != null && dset.Tables[0].Rows.Count > 0)
                {
                    callBackRetVal = dset.Tables[0].Rows[0][this.DataTextField].ToString() + ";" + this.GetColumnMatchScript(dset.Tables[0].Rows[0]);

                    this.Page.Session[this.ClientID + "TempRefVal"] = eventArgument;
                    this.Page.Session[this.ClientID + "TempRefDis"] = (callBackRetVal.IndexOf(';') == -1) ? "" : callBackRetVal.Substring(0, callBackRetVal.IndexOf(';'));
                }
            }
            else
            {
                string sCurProject = CliUtils.fCurrentProject;
                WebDataSource ds = this.GetDataSource();
                ds.CommandTable.PrimaryKey = new DataColumn[] { ds.CommandTable.Columns[this.DataValueField] };
                DataRowCollection rows = ds.CommandTable.Rows;
                string type = ds.CommandTable.Columns[this.DataValueField].DataType.ToString().ToLower();
                DataRow row = GloFix.FindRow(rows, type, eventArgument);
                if (row != null)
                {
                    callBackRetVal = row[this.DataTextField].ToString() + ";" + this.GetColumnMatchScript(row);
                    return;
                }
                //在原有DataSource的PacketRecords之外，则必须下sql语句将此笔资料找出
                string strSql = this.GetCommandText();
                if (GloFix.IsNumeric(ds.CommandTable.Columns[this.DataValueField].DataType))
                {
                    if (eventArgument != null && eventArgument != "")
                        strSql = GloFix.InsertWhereCondition(strSql, this.DataValueField + " = " + eventArgument);
                }
                else
                    strSql = GloFix.InsertWhereCondition(strSql, this.DataValueField + " = '" + eventArgument + "'");
                DataSet dset = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", strSql, true, sCurProject);
                if (dset != null && dset.Tables[0].Rows.Count > 0)
                {
                    callBackRetVal = dset.Tables[0].Rows[0][this.DataTextField].ToString() + ";" + this.GetColumnMatchScript(dset.Tables[0].Rows[0]);
                }
            }
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
                //QueryParts
                _innerRow1 = new TableRow();
                _innerRow1.Visible = false;
                DataTable tempTab = this.GetRefTable();
                if (tempTab != null)
                {
                    Table _tabQuery = this.CreateQueryTable(tempTab);
                    Panel _panQuery = new Panel();
                    _panQuery.CssClass = "ajaxref_query_bg";
                    _panQuery.Controls.Add(_tabQuery);
                    TableCell _innerCell11 = new TableCell();
                    _innerCell11.Controls.Add(_panQuery);
                    _innerRow1.Cells.Add(_innerCell11);

                    _innerRow2 = new TableRow();
                    _innerRow2.Visible = false;
                    TableCell _innerCell21 = new TableCell();
                    _innerCell21.HorizontalAlign = HorizontalAlign.Center;
                    _innerCell21.ColumnSpan = 3;
                    Button _btnQuery = new Button();
                    _btnQuery.ID = "btnQuery";
                    _btnQuery.Text = "to query according conditions above...";
                    _btnQuery.CssClass = "ajaxref_btn_mouseout";
                    _btnQuery.Attributes.Add("onmouseout", "this.className='_ajaxref_btn_mouseout';");
                    _btnQuery.Attributes.Add("onmouseover", "this.className='_ajaxref_btn_mouseover';");
                    _btnQuery.Click += new EventHandler(_btnQuery_Click);
                    _innerCell21.Controls.Add(_btnQuery);
                    _innerRow2.Cells.Add(_innerCell21);
                }
                _innerTable.Rows.AddRange(new TableRow[] { _innerRow1, _innerRow2, _innerRow3 });
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

                //_modalPopupExtender
                _modalPopupExtender = new ModalPopupExtender();
                _modalPopupExtender.ID = "modalPopupExtender";
                _modalPopupExtender.TargetControlID = this._hidTarget.UniqueID;
                _modalPopupExtender.PopupControlID = this._popupContainer.UniqueID;
                _modalPopupExtender.PopupDragHandleControlID = this._panTitle.UniqueID;
                _modalPopupExtender.BackgroundCssClass = "ajaxref_modalBackground";
                _modalPopupExtender.CancelControlID = this._hidCloseTarget.UniqueID;
                _modalPopupExtender.BehaviorID = this.ClientID + "_refvalShowModalBehavior";
                //_modalPopupExtender.DropShadow = true;
                this.Controls.Add(_modalPopupExtender);
            }
        }

        void _txtValue_TextChanged(object sender, EventArgs e)
        {
            this.BindingValue = _txtValue.Text;
        }

        private Table CreateQueryTable(DataTable tempTab)
        {
            Table _tabQuery = new Table();
            int colCount = (this.Columns.Count == 0) ? tempTab.Columns.Count : this.Columns.Count;
            dropDownLists = new DropDownList[colCount];
            textBoxes = new TextBox[colCount];
            queryFields = new string[colCount];
            dataTypes = new string[colCount];
            for (int i = 0; i < colCount; i++)
            {
                string colName = (this.Columns.Count == 0) ? tempTab.Columns[i].ColumnName : this.Columns[i].ColumnName;
                queryFields[i] = colName;
                dataTypes[i] = tempTab.Columns[colName].DataType.ToString();
                TableRow _rowQuery = new TableRow();
                TableCell _cellQuery1 = new TableCell();
                _cellQuery1.HorizontalAlign = HorizontalAlign.Right;
                _cellQuery1.Text = (this.Columns.Count == 0) ? tempTab.Columns[i].ColumnName : this.Columns[i].HeadText;
                TableCell _cellQuery2 = new TableCell();
                dropDownLists[i] = new DropDownList();
                dropDownLists[i].ID = "ddl" + colName;
                dropDownLists[i].Items.AddRange(new ListItem[8] { new ListItem("<="), 
                                         new ListItem("<"),
                                         new ListItem("="),
                                         new ListItem("!="),
                                         new ListItem(">"),
                                         new ListItem(">="),
                                         new ListItem("%"),
                                         new ListItem("%%")});
                if (dataTypes[i].ToLower() == "system.string")
                    dropDownLists[i].SelectedIndex = 6;
                else
                    dropDownLists[i].SelectedIndex = 2;
                _cellQuery2.Controls.Add(dropDownLists[i]);
                TableCell _cellQuery3 = new TableCell();
                textBoxes[i] = new TextBox();
                textBoxes[i].ID = "txt" + colName;
                _cellQuery3.Controls.Add(textBoxes[i]);
                _rowQuery.Cells.AddRange(new TableCell[] { _cellQuery1, _cellQuery2, _cellQuery3 });

                _tabQuery.Rows.Add(_rowQuery);
            }
            return _tabQuery;
        }

        private string ExcuteSetWhereItem()
        {
            WebDataSource wds = this.GetDataSource();
            string sql = this.GetWhereItem();
            wds.SetWhere(sql);
            if (this._grid != null)
            {
                this._grid.DataBind();
                this._grid.PageIndex = 0;
            }

            if (this._innerRow1 != null)
                this._innerRow1.Visible = false;
            if (this._innerRow2 != null)
                this._innerRow2.Visible = false;

            return sql;
        }

        private void ExcuteClearWhereItem()
        {
            WebDataSource wds = this.GetDataSource();
            wds.SetWhere("");
            this._grid.DataBind();
        }

        void _btnQuery_Click(object sender, EventArgs e)
        {
            this._innerRow1.Visible = false;
            this._innerRow2.Visible = false;
            string strQueryCondition = "";
            DataTable tempTab = this.GetRefTable();
            int colCount = (this.Columns.Count == 0) ? tempTab.Columns.Count : this.Columns.Count;
            for (int i = 0; i < colCount; i++)
            {
                if (dropDownLists[i].Text != string.Empty && textBoxes[i].Text != string.Empty)
                {
                    string type = dataTypes[i].ToLower();
                    if (dropDownLists[i].Text != "%" && dropDownLists[i].Text != "%%")
                    {
                        if (GloFix.IsNumeric(type))
                        {
                            strQueryCondition += queryFields[i] + dropDownLists[i].Text + textBoxes[i].Text + " and ";
                        }
                        else
                        {
                            strQueryCondition += queryFields[i] + dropDownLists[i].Text + " '" + textBoxes[i].Text.Replace("'", "''") + "' and ";
                        }
                    }
                    else
                    {
                        if (dropDownLists[i].Text == "%")
                        {
                            strQueryCondition += queryFields[i] + " like '" + textBoxes[i].Text.Replace("'", "''") + "%' and ";
                        }
                        if (dropDownLists[i].Text == "%%")
                        {
                            strQueryCondition += queryFields[i] + " like '%" + textBoxes[i].Text.Replace("'", "''") + "%' and ";
                        }
                    }
                }
            }
            if (strQueryCondition != string.Empty)
            {
                strQueryCondition = strQueryCondition.Substring(0, strQueryCondition.LastIndexOf(" and "));
            }
            WebDataSource wds = this.GetDataSource();
            string sql = this.GetWhereItem();

            if (!string.IsNullOrEmpty(strQueryCondition))
            {
                if (string.IsNullOrEmpty(sql))
                    sql = strQueryCondition;
                else
                    sql += " and " + strQueryCondition;
            }
            this.QueryCondition = strQueryCondition;
            wds.SetWhere(sql);
            this._grid.DataBind();
            this._grid.PageIndex = 0;
        }

        void _grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int inx = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).DataItemIndex;
                DataTable refTable = this.GetRefTable();
                string val = refTable.Rows[inx][this.DataValueField].ToString();
                string dip = refTable.Rows[inx][this.DataTextField].ToString();
                val = val.Replace(@"\", @"\\").Replace("'", "\\'");
                dip = dip.Replace(@"\", @"\\").Replace("'", "\\'");
                string script =
                    "$find('" + _modalPopupExtender.BehaviorID + "').hide();" +
                    "$get('" + this._txtValue.ClientID + "').value='" + val + "';" +
                    "$get('" + this._txtDisplay.ClientID + "').value='" + dip + "';" +
                    "$get('" + this._txtShow.ClientID + "').value='" + val + "';";
                script += GetColumnMatchScript(refTable.Rows[inx]);
                script += "$get('" + this._txtShow.ClientID + "').focus();";
                ScriptManager.RegisterStartupScript(this._updatePanel, this.GetType(), "datescript", script, true);
                this.ExcuteClearWhereItem();
            }
            else if (e.CommandName == "Query")
            {
                this._innerRow1.Visible = true;
                this._innerRow2.Visible = true;
            }
            else if (e.CommandName == "Refresh")
            {
                this.QueryCondition = this.ExcuteSetWhereItem();
            }
        }

        void _grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebDataSource wds = this.GetDataSource();
            if (e.NewPageIndex == this._grid.PageCount - 1)
            {
                wds.GetNextPacket();
            }
        }

        void _btnClose_Click(object sender, ImageClickEventArgs e)
        {
            this.ExcuteClearWhereItem();

            string script = "$find('" + _modalPopupExtender.BehaviorID + "').hide();";
            ScriptManager.RegisterStartupScript(this._updatePanel, this.GetType(), "datescript", script, true);
        }

        void _btnImage_Click(object sender, ImageClickEventArgs e)
        {
            this.QueryCondition = this.ExcuteSetWhereItem();
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
                //赋初值
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
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebRefVal", "CheckDataMessage", true);
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
            refVal.AppendFormat("param.refMethorControlID = \"{0}\";", "wfvMaster");
            //refVal.AppendFormat("param.refTitle = \"{0}\";", this.RefValTitle);
            refVal.Append("param.refModuleName = \"GLModule\";");
            refVal.Append("param.refCommandName = \"cmdRefValUse\";");
            refVal.Append("param.refDynamic = \"Y\";");
            object o = this.GetObjByID(this.DataSourceID);
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    refVal.AppendFormat("param.refCmdsql = \"{0}\";", wds.SelectCommand);
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
                    strMatchSrcColumns += "|";
                if (strMatchDestcontrolIDs != String.Empty)
                    strMatchDestcontrolIDs += "|";
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

                _modalPopupExtender.RenderControl(writer);
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
