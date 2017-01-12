using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms.Design;
using AjaxControlToolkit;
using Srvtools;


namespace AjaxTools
{
    [Designer(typeof(SmartMenuDesigner), typeof(IDesigner))]
    public class AjaxTreeViewPopup : AjaxBaseWebControl, INamingContainer, IAjaxDataSource
    {

        public AjaxTreeViewPopup()
        {
            _editColumn = new AjaxTreeViewItemCollection(this, typeof(AjaxTreeViewItem));
        }
        SYS_LANGUAGE language;

        //TreeView _tview = new TreeView();
        Panel _popupContainer = new Panel();
        ImageButton _btnClose = new ImageButton();
        Button _btnTarget = new Button();
        ModalPopupExtender _modalPopupExtender = new ModalPopupExtender();
        UpdatePanel _updatePanel = new UpdatePanel();

        #region properties
        private string _dataSourceId;
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get
            {
                return _dataSourceId;
            }
            set
            {
                _dataSourceId = value;
            }
        }

        private string _treeViewId;
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(TreeViewSelectEditor), typeof(UITypeEditor))]
        public string TreeViewId
        {
            get
            {
                return _treeViewId;
            }
            set
            {
                _treeViewId = value;
            }
        }

        private string _parentfield;
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ParentField
        {
            get
            {
                return _parentfield;
            }
            set
            {
                _parentfield = value;
            }
        }

        private string _keyfield;
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string KeyField
        {
            get
            {
                return _keyfield;
            }
            set
            {
                _keyfield = value;
            }
        }

        private string _textfield;
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string TextField
        {
            get
            {
                return _textfield;
            }
            set
            {
                _textfield = value;
            }
        }

        private string _parentcaption;
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        public string ParentCaption
        {
            get
            {
                return _parentcaption;
            }
            set
            {
                _parentcaption = value;
            }
        }

        private string _keycaption;
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        public string KeyCaption
        {
            get
            {
                return _keycaption;
            }
            set
            {
                _keycaption = value;
            }
        }

        private string _textcaption;
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        public string TextCaption
        {
            get
            {
                return _textcaption;
            }
            set
            {
                _textcaption = value;
            }
        }

        public string _updatePanelId;
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(TriggerUpdatePanelEditor), typeof(UITypeEditor))]
        public string UpdatePanelID
        {
            get
            {
                return _updatePanelId;
            }
            set
            {
                _updatePanelId = value;
            }
        }

        private string _insertButtonId;
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ButtonSelectEditor), typeof(UITypeEditor))]
        public string InsertButtonId
        {
            get
            {
                return _insertButtonId;
            }
            set
            {
                _insertButtonId = value;
            }
        }

        private string _updateButtonId;
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ButtonSelectEditor), typeof(UITypeEditor))]
        public string UpdateButtonId
        {
            get
            {
                return _updateButtonId;
            }
            set
            {
                _updateButtonId = value;
            }
        }

        private AjaxTreeViewItemCollection _editColumn;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public AjaxTreeViewItemCollection EditColumn
        {
            get
            {
                return _editColumn;
            }
        }
        #endregion

        public DataTable GetMenuTable()
        {
            WebDataSource wds = this.GetDataSource();
            if (wds != null)
            {
                return wds.InnerDataSet.Tables[wds.DataMember];
            }
            return null;
        }

        private WebDataSource GetDataSource()
        {
            object obj = GetObjByID(this.DataSourceID);
            if (obj != null && obj is WebDataSource)
            {
                return obj as WebDataSource;
            }
            return null;
        }

        private TreeView GetTree()
        {
            object obj = GetObjByID(this.TreeViewId);
            if (obj != null && obj is TreeView)
            {
                return obj as TreeView;
            }
            return null;
        }

        public void Initial()
        {
            ArrayList lstkey = new ArrayList();
            ArrayList lstparent = new ArrayList();
            ArrayList lsttext = new ArrayList();

            ArrayList lstmainkey = new ArrayList();
            ArrayList lstmaintext = new ArrayList();
            ArrayList lstchildkey = new ArrayList();
            ArrayList lstchildparent = new ArrayList();
            ArrayList lstchildtext = new ArrayList();

            TreeView tree = this.GetTree();
            if (tree == null) return;
            tree.Nodes.Clear();
            DataTable dt = GetMenuTable();
            int nodecount = dt.Rows.Count;
            for (int i = 0; i < nodecount; i++)
            {
                lstkey.Add(dt.Rows[i][this.KeyField]);
                lstparent.Add(dt.Rows[i][this.ParentField]);
                lsttext.Add(dt.Rows[i][this.TextField]);
            }

            for (int i = 0; i < nodecount; i++)
            {
                if (lstkey[i].ToString() != lstparent[i].ToString())
                {
                    if (lstparent[i].ToString() == string.Empty)
                    {
                        lstmainkey.Add(lstkey[i]);
                        lstmaintext.Add(lsttext[i]);
                    }
                    else
                    {
                        lstchildkey.Add(lstkey[i]);
                        lstchildparent.Add(lstparent[i]);
                        lstchildtext.Add(lsttext[i]);
                    }
                }
            }

            int mainnodecount = lstmainkey.Count;
            TreeNode[] nodemain = new TreeNode[mainnodecount];

            for (int i = 0; i < mainnodecount; i++)
            {
                nodemain[i] = new TreeNode();
                nodemain[i].Text = lstmaintext[i].ToString();
                nodemain[i].Value = lstmainkey[i].ToString();
                tree.Nodes.Add(nodemain[i]);
            }

            int childnodecount = lstchildkey.Count;
            TreeNode[] nodechild = new TreeNode[childnodecount];
            for (int i = 0; i < childnodecount; i++)
            {
                nodechild[i] = new TreeNode();
                nodechild[i].Text = lstchildtext[i].ToString();
                nodechild[i].Value = lstchildkey[i].ToString();
            }

            for (int i = 0; i < childnodecount; i++)
            {
                for (int j = 0; j < mainnodecount; j++)
                {
                    if (lstchildparent[i].ToString() == lstmainkey[j].ToString())
                    {
                        nodemain[j].ChildNodes.Add(nodechild[i]);
                    }
                }
                for (int k = 0; k < childnodecount; k++)
                {
                    if (lstchildparent[i].ToString() == lstchildkey[k].ToString())
                    {
                        nodechild[k].ChildNodes.Add(nodechild[i]);
                    }
                }
            }
            tree.ExpandAll();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.ShowModal();
            if(!this.Page.IsPostBack)
                this.Initial();
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            //_btnTarget
            _btnTarget = new Button();
            _btnTarget.ID = "btnTarget";
            _btnTarget.Text = "click";
            this.Controls.Add(_btnTarget);

            //_btnClose
            _btnClose = new ImageButton();
            _btnClose.ID = "btnClose";
            _btnClose.ImageUrl = "~/Image/Ajax/close.gif";
            this.Controls.Add(_btnClose);

            //_popupContainer
            _popupContainer = new Panel();
            _popupContainer.ID = "popupContainer";
            this.Controls.Add(_popupContainer);

            if (!this.DesignMode)
            {
                _updatePanel = new UpdatePanel();
                _updatePanel.ID = "updateContent";
                _updatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
                _updatePanel.ChildrenAsTriggers = true;
                _updatePanel.EnableViewState = true;
                _updatePanel.ContentTemplateContainer.Controls.Add(this.GenTable());
                if (!string.IsNullOrEmpty(this.InsertButtonId))
                {
                    AsyncPostBackTrigger triggerInsertOpen = new AsyncPostBackTrigger();
                    triggerInsertOpen.ControlID = this.InsertButtonId;
                    triggerInsertOpen.EventName = "Click";
                    _updatePanel.Triggers.Add(triggerInsertOpen);
                }
                if (!string.IsNullOrEmpty(this.UpdateButtonId))
                {
                    AsyncPostBackTrigger triggerUpdateOpen = new AsyncPostBackTrigger();
                    triggerUpdateOpen.ControlID = this.UpdateButtonId;
                    triggerUpdateOpen.EventName = "Click";
                    _updatePanel.Triggers.Add(triggerUpdateOpen);
                }

                this.Controls.Add(_updatePanel);


                _modalPopupExtender = new ModalPopupExtender();
                _modalPopupExtender.ID = "modalPopupExtender";
                _modalPopupExtender.TargetControlID = this._btnTarget.UniqueID;
                _modalPopupExtender.PopupControlID = this._popupContainer.UniqueID;
                _modalPopupExtender.PopupDragHandleControlID = this._popupContainer.UniqueID;
                _modalPopupExtender.BackgroundCssClass = "ajaxtree_modalBackground";
                _modalPopupExtender.CancelControlID = this._btnClose.UniqueID;
                _modalPopupExtender.BehaviorID = this.ClientID + "_treeShowModalBehavior";
                //_modalPopupExtender.DropShadow = true;
                this.Controls.Add(_modalPopupExtender);
            }
        }

        public void TreeSelectChange()
        {
            TreeView tree = this.GetTree();
            object obj = this.GetObjByID(this.UpdatePanelID);
            if (obj != null && obj is UpdatePanel)
            {
                string script = "selectedNode='" + tree.SelectedNode.Value + "';";
                ScriptManager.RegisterStartupScript((UpdatePanel)obj, this.GetType(), "selectAjaxTree", script, true);
            }

            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "selectAjaxTree", script, true);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            _btnTarget.RenderControl(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxtree_popupContainer");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            _popupContainer.RenderBeginTag(writer);
            #region title
            writer.AddAttribute("id", "divTitle");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxtree_div_title");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddStyleAttribute("cursor", "pointer");
            writer.AddStyleAttribute("position", "relative");
            writer.AddStyleAttribute("top", "3px");
            _btnClose.RenderControl(writer);
            writer.RenderEndTag();
            #endregion
            #region content
            writer.AddAttribute("id", "divContent");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxtree_div_content");
            //writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            _updatePanel.RenderControl(writer);
            writer.RenderEndTag();
            #endregion
            _popupContainer.RenderEndTag(writer);

            if (!this.DesignMode)
                _modalPopupExtender.RenderControl(writer);
        }

        private Table GenTable()
        {
            Table table = new Table();
            table.CellPadding = 0;
            table.CellSpacing = 5;
            table.CssClass = "ajaxtree_insert_or_udpate_table";
            language = CliUtils.fClientLang;
            string message = SysMsg.GetSystemMessage(language, "Srvtools", "WebTreeView", "Caption", true);
            string[] captions = message.Split(';');

            TableRow rowKey = new TableRow();
            TableCell cellKey1 = new TableCell();
            Label lblKey = new Label();
            lblKey.ID = "lblKey";
            lblKey.Width = new Unit(50);
            lblKey.Text = string.IsNullOrEmpty(this.KeyCaption) ? captions[0] : this.KeyCaption;
            cellKey1.Controls.Add(lblKey);
            TableCell cellKey2 = new TableCell();
            TextBox txtKey = new TextBox();
            txtKey.Width = new Unit(120);
            txtKey.ID = "txtKey";
            cellKey2.Controls.Add(txtKey);
            rowKey.Cells.AddRange(new TableCell[] { cellKey1, cellKey2 });

            TableRow rowParent = new TableRow();
            TableCell cellParent1 = new TableCell();
            Label lblParent = new Label();
            lblParent.ID = "lblParent";
            lblParent.Width = new Unit(50);
            lblParent.Text = string.IsNullOrEmpty(this.ParentCaption) ? captions[1] : this.ParentCaption;
            cellParent1.Controls.Add(lblParent);
            TableCell cellParent2 = new TableCell();
            DropDownList ddlParent = new DropDownList();
            ddlParent.ID = "ddlParent";
            ddlParent.Width = new Unit(125);
            cellParent2.Controls.Add(ddlParent);
            rowParent.Cells.AddRange(new TableCell[] { cellParent1, cellParent2 });

            TableRow rowCaption = new TableRow();
            TableCell cellCaption1 = new TableCell();
            Label lblCaption = new Label();
            lblCaption.ID = "lblCaption";
            lblCaption.Width = new Unit(50);
            lblCaption.Text = string.IsNullOrEmpty(this.TextCaption) ? captions[2] : this.TextCaption;
            cellCaption1.Controls.Add(lblCaption);
            TableCell cellCaption2 = new TableCell();
            TextBox txtCaption = new TextBox();
            txtCaption.ID = "txtCaption";
            txtCaption.Width = new Unit(120);
            cellCaption2.Controls.Add(txtCaption);
            rowCaption.Cells.AddRange(new TableCell[] { cellCaption1, cellCaption2 });

            table.Rows.AddRange(new TableRow[] { rowKey, rowParent, rowCaption });

            foreach (AjaxTreeViewItem item in this.EditColumn)
            {
                TableRow row = new TableRow();
                TableCell cell1 = new TableCell();
                Label lbl = new Label();
                lbl.ID = "lbl" + item.Column;
                lbl.Width = new Unit(50);
                lbl.Text = item.Caption;
                cell1.Controls.Add(lbl);
                TableCell cell2 = new TableCell();
                switch (item.ColumnType)
                { 
                    case AjaxTreeViewColumnType.TextBoxColumn:
                        TextBox txt = new TextBox();
                        txt.ID = "txt" + item.Column;
                        txt.Width = new Unit(120);
                        cell2.Controls.Add(txt);
                        break;
                    case AjaxTreeViewColumnType.ComboBoxColumn:
                        DropDownList ddl = new DropDownList();
                        ddl.ID = "ddl" + item.Column;
                        ddl.Width = new Unit(125);
                        ddl.DataSourceID = item.DataSourceID;
                        ddl.DataTextField = item.DataTextField;
                        ddl.DataValueField = item.DataValueField;
                        cell2.Controls.Add(ddl);
                        break;
                    case AjaxTreeViewColumnType.RefValColumn:
                        WebRefVal refval = new WebRefVal();
                        refval.ID = "ref" + item.Column;
                        refval.Width = new Unit(125);
                        refval.DataSourceID = item.DataSourceID;
                        refval.DataTextField = item.DataTextField;
                        refval.DataValueField = item.DataValueField;
                        cell2.Controls.Add(refval);
                        break;
                    case AjaxTreeViewColumnType.CalendarColumn:
                        AjaxCalendar cal = new AjaxCalendar();
                        cal.ID = "cal" + item.Column;
                        cal.Width = new Unit(120);
                        cal.Format = "yyyy/MM/dd";
                        cell2.Controls.Add(cal);
                        break;
                    case AjaxTreeViewColumnType.RadioButtonColumn:
                        break;
                }
                row.Cells.AddRange(new TableCell[] { cell1, cell2 });
                table.Rows.Add(row);
            }

            TableRow buttonRow = new TableRow();
            TableCell cellButton = new TableCell();
            cellButton.ColumnSpan = 2;
            cellButton.CssClass = "ajaxtree_buttonContainer";
            Button btnOK = new Button();
            btnOK.ID = "btnOK";
            btnOK.Text = "OK";
            btnOK.OnClientClick = "var behavior = $find('" + this.ClientID + "_treeShowModalBehavior');if (behavior) {behavior.hide();}";
            btnOK.CssClass = "ajaxtree_btn_rect_mouseout";
            btnOK.Attributes.Add("onmouseout", "this.className='_ajaxtree_btn_rect_mouseout'");
            btnOK.Attributes.Add("onmouseover", "this.className='_ajaxtree_btn_rect_mouseover'");
            btnOK.Click += new EventHandler(btnOK_Click);
            cellButton.Controls.Add(btnOK);
            Button btnCancel = new Button();
            btnCancel.ID = "btnCancel";
            btnCancel.Text = "Cancel";
            btnCancel.OnClientClick = "var behavior = $find('" + this.ClientID + "_treeShowModalBehavior');if (behavior) {behavior.hide();}return false;";
            btnCancel.CssClass = "ajaxtree_btn_rect_mouseout";
            btnCancel.Attributes.Add("onmouseout", "this.className='_ajaxtree_btn_rect_mouseout'");
            btnCancel.Attributes.Add("onmouseover", "this.className='_ajaxtree_btn_rect_mouseover'");
            cellButton.Controls.Add(btnCancel);
            buttonRow.Cells.Add(cellButton);

            table.Rows.Add(buttonRow);

            return table;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.OnSubmit(EventArgs.Empty);
        }

        public void ShowModal()
        {
            object obj = this.GetObjByID(this.UpdatePanelID);
            if (obj != null && obj is UpdatePanel)
            {
                string script =
                    "var selectedNode = null;" +
                    "function ajaxTreeViewShowModal(mode) {" +
                        "if (mode == 1 && !selectedNode) return;" +
                        "var behavior = $find('" + this.ClientID + "_treeShowModalBehavior');" +
                        "if (behavior) {" +
                        "behavior.show();" + 
                        "}" +
                    "}";
                //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "showAjaxTree", script, true);
                ScriptManager.RegisterStartupScript((UpdatePanel)obj, this.GetType(), "showAjaxTree", script, true);
            }
        }

        public void InsertItem()
        {
            InsertItem(true);
        }

        public void InsertItem(bool autoGenKey)
        {
            DataTable table = this.GetMenuTable();
            Control key_ctrl = this._updatePanel.ContentTemplateContainer.FindControl("txtKey");
            if (key_ctrl != null && key_ctrl is TextBox)
            {
                TextBox textKey = key_ctrl as TextBox;
                if (autoGenKey)
                {
                    List<int> keys = new List<int>();
                    foreach (DataRow row in table.Rows)
                    {
                        keys.Add(Convert.ToInt32(row[this.KeyField]));
                    }
                    int maxKey = GetMaxKey(keys);
                    textKey.Text = maxKey.ToString();
                }
                else
                {
                    textKey.Text = "";
                }
                textKey.Enabled = true;
            }
            Control parent_ctrl = this._updatePanel.ContentTemplateContainer.FindControl("ddlParent");
            if (parent_ctrl != null && parent_ctrl is DropDownList)
            {
                DropDownList ddlParent = parent_ctrl as DropDownList;
                ddlParent.Items.Clear();
                ddlParent.Items.Add(new ListItem("(none)", ""));
                foreach (DataRow row in table.Rows)
                {
                    ListItem item = new ListItem(row[this.TextField].ToString(), row[this.KeyField].ToString());
                    ddlParent.Items.Add(item);
                }
                TreeView tree = this.GetTree();
                if (tree.SelectedNode != null)
                {
                    ddlParent.SelectedValue = tree.SelectedNode.Value;
                }
            }

            Control caption_ctrl = this._updatePanel.ContentTemplateContainer.FindControl("txtCaption");
            if (caption_ctrl != null && caption_ctrl is TextBox)
            {
                TextBox txtCaption = caption_ctrl as TextBox;
                txtCaption.Text = "";
            }

            foreach (AjaxTreeViewItem item in this.EditColumn)
            {
                string ctrlId = "";
                Control ctrl = null;
                switch (item.ColumnType)
                { 
                    case AjaxTreeViewColumnType.TextBoxColumn:
                        ctrlId = "txt" + item.Column;
                        ctrl = this._updatePanel.ContentTemplateContainer.FindControl(ctrlId);
                        if (ctrl != null && ctrl is TextBox)
                        {
                            TextBox txt = ctrl as TextBox;
                            txt.Text = "";
                        }
                        break;
                    case AjaxTreeViewColumnType.ComboBoxColumn:
                        ctrlId = "ddl" + item.Column;
                        ctrl = this._updatePanel.ContentTemplateContainer.FindControl(ctrlId);
                        if (ctrl != null && ctrl is DropDownList)
                        {
                            DropDownList ddl = ctrl as DropDownList;
                            ddl.SelectedIndex = -1;
                        }
                        break;
                    case AjaxTreeViewColumnType.RefValColumn:
                        ctrlId = "ref" + item.Column;
                        ctrl = this._updatePanel.ContentTemplateContainer.FindControl(ctrlId);
                        if (ctrl != null && ctrl is WebRefVal)
                        {
                            WebRefVal refval = ctrl as WebRefVal;
                            refval.BindingValue = "";
                        }
                        break;
                    case AjaxTreeViewColumnType.CalendarColumn:
                        ctrlId = "cal" + item.Column;
                        ctrl = this._updatePanel.ContentTemplateContainer.FindControl(ctrlId);
                        if (ctrl != null && ctrl is AjaxCalendar)
                        {
                            AjaxCalendar cal = ctrl as AjaxCalendar;
                            cal.Text = "";
                        }
                        break;
                    case AjaxTreeViewColumnType.RadioButtonColumn:
                        ctrlId = "rad" + item.Column;
                        break;
                }
            }
            this.ViewState["State"] = "Inserting";
        }

        public void UpdateItem()
        {
            TreeView tree = this.GetTree();
            if (tree.SelectedNode == null) return;

            DataTable table = this.GetMenuTable();
            Control key_ctrl = this._updatePanel.ContentTemplateContainer.FindControl("txtKey");
            if (key_ctrl != null && key_ctrl is TextBox)
            {
                TextBox txtKey = key_ctrl as TextBox;
                txtKey.Text = tree.SelectedNode.Value;
                txtKey.Enabled = false;
            }

            Control parent_ctrl = this._updatePanel.ContentTemplateContainer.FindControl("ddlParent");
            if (parent_ctrl != null && parent_ctrl is DropDownList)
            {
                DropDownList ddlParent = parent_ctrl as DropDownList;
                ddlParent.Items.Clear();
                ddlParent.Items.Add(new ListItem("(none)", ""));
                foreach (DataRow row in table.Rows)
                {
                    ListItem item = new ListItem(row[this.TextField].ToString(), row[this.KeyField].ToString());
                    ddlParent.Items.Add(item);
                }
                ddlParent.SelectedValue = tree.SelectedNode.Parent == null ? "" : tree.SelectedNode.Parent.Value;
            }

            Control caption_ctrl = this._updatePanel.ContentTemplateContainer.FindControl("txtCaption");
            if (caption_ctrl != null && caption_ctrl is TextBox)
            {
                TextBox txtCaption = caption_ctrl as TextBox;
                txtCaption.Text = tree.SelectedNode.Text;
            }

            WebDataSource wds = this.GetDataSource();
            DataTable menuTable = wds.InnerDataSet.Tables[wds.DataMember];
            DataRow editRow = menuTable.Rows.Find(tree.SelectedNode.Value);
            foreach (AjaxTreeViewItem item in this.EditColumn)
            {
                string ctrlId = "";
                Control ctrl = null;
                switch (item.ColumnType)
                {
                    case AjaxTreeViewColumnType.TextBoxColumn:
                        ctrlId = "txt" + item.Column;
                        ctrl = this._updatePanel.ContentTemplateContainer.FindControl(ctrlId);
                        if (ctrl != null && ctrl is TextBox)
                        {
                            TextBox txt = ctrl as TextBox;
                            txt.Text = editRow[item.Column].ToString();
                        }
                        break;
                    case AjaxTreeViewColumnType.ComboBoxColumn:
                        ctrlId = "ddl" + item.Column;
                        ctrl = this._updatePanel.ContentTemplateContainer.FindControl(ctrlId);
                        if (ctrl != null && ctrl is DropDownList)
                        {
                            DropDownList ddl = ctrl as DropDownList;
                            ddl.SelectedValue = editRow[item.Column].ToString();
                        }
                        break;
                    case AjaxTreeViewColumnType.RefValColumn:
                        ctrlId = "ref" + item.Column;
                        ctrl = this._updatePanel.ContentTemplateContainer.FindControl(ctrlId);
                        if (ctrl != null && ctrl is WebRefVal)
                        {
                            WebRefVal refval = ctrl as WebRefVal;
                            refval.BindingValue = editRow[item.Column].ToString();
                        }
                        break;
                    case AjaxTreeViewColumnType.CalendarColumn:
                        ctrlId = "cal" + item.Column;
                        ctrl = this._updatePanel.ContentTemplateContainer.FindControl(ctrlId);
                        if (ctrl != null && ctrl is AjaxCalendar)
                        {
                            AjaxCalendar cal = ctrl as AjaxCalendar;
                            if (editRow[item.Column] is DBNull)
                            {
                                cal.Text = "";
                            }
                            else
                            {
                                cal.Text = Convert.ToDateTime(editRow[item.Column]).ToString("yyyy/MM/dd");
                            }
                        }
                        break;
                    case AjaxTreeViewColumnType.RadioButtonColumn:
                        ctrlId = "rad" + item.Column;
                        break;
                }
            }
            this.ViewState["State"] = "Updating";
        }

        public void DeleteItem()
        {
            TreeView tree = this.GetTree();
            TreeNode selNode = tree.SelectedNode;
            if (selNode == null) return;

            WebDataSource wds = this.GetDataSource();
            DataTable menuTable = wds.InnerDataSet.Tables[wds.DataMember];
            DataRow delRow = menuTable.Rows.Find(selNode.Value);
            if (delRow != null)
            {
                delRow.Delete();
                wds.ApplyUpdates();
                this.Initial();
            }
        }

        private int GetMaxKey(List<int> keys)
        {
            if (keys.Count > 0)
            {
                int maxkey = keys[0];
                for (int i = 1; i < keys.Count; i++)
                {
                    maxkey = Math.Max(maxkey, keys[i]);
                }
                return ++maxkey;
            }
            return -1;
        }

        #region Event
        static readonly object submitEventKey = new object();

        protected virtual void OnSubmit(EventArgs e)
        {
            TextBox txtKey = null;
            DropDownList ddlParent = null;
            TextBox txtCaption = null;
            Control key_ctrl = this._updatePanel.ContentTemplateContainer.FindControl("txtKey");
            if (key_ctrl != null && key_ctrl is TextBox)
            {
                txtKey = key_ctrl as TextBox;
            }
            Control parent_ctrl = this._updatePanel.ContentTemplateContainer.FindControl("ddlParent");
            if (parent_ctrl != null && parent_ctrl is DropDownList)
            {
                ddlParent = parent_ctrl as DropDownList;
            }
            Control caption_ctrl = this._updatePanel.ContentTemplateContainer.FindControl("txtCaption");
            if (caption_ctrl != null && caption_ctrl is TextBox)
            {
                txtCaption = caption_ctrl as TextBox;
            }
            if (txtKey == null || ddlParent == null || txtCaption == null) return;
            WebDataSource wds = this.GetDataSource();
            DataTable menuTable = wds.InnerDataSet.Tables[wds.DataMember];
            TreeView tree = this.GetTree();
            switch (this.ViewState["State"].ToString())
            {
                case "Inserting":
                    DataRow newRow = menuTable.NewRow();
                    newRow[this.KeyField] = txtKey.Text;
                    newRow[this.ParentField] = ddlParent.SelectedValue;
                    newRow[this.TextField] = txtCaption.Text;
                    this.FillRow(newRow);
                    menuTable.Rows.Add(newRow);
                    break;
                case "Updating":
                    TreeNode selNode = tree.SelectedNode;
                    DataRow editRow = menuTable.Rows.Find(txtKey.Text);
                    editRow[this.ParentField] = ddlParent.SelectedValue;
                    editRow[this.TextField] = txtCaption.Text;
                    this.FillRow(editRow);
                    break;
            }
            wds.ApplyUpdates();
            this.Initial();
            EventHandler<EventArgs> submitHandler = (EventHandler<EventArgs>)Events[submitEventKey];
            if (submitHandler != null)
            {
                submitHandler(this, e);
            }
        }

        public event EventHandler<EventArgs> Submit
        {
            add { Events.AddHandler(submitEventKey, value); }
            remove { Events.RemoveHandler(submitEventKey, value); }
        }
        #endregion

        private void FillRow(DataRow row)
        {
            foreach (AjaxTreeViewItem item in this.EditColumn)
            {
                string ctrlId = "";
                Control ctrl = null;
                switch (item.ColumnType)
                { 
                    case AjaxTreeViewColumnType.TextBoxColumn:
                        ctrlId = "txt" + item.Column;
                        ctrl = this._updatePanel.ContentTemplateContainer.FindControl(ctrlId);
                        if (ctrl != null && ctrl is TextBox)
                        {
                            TextBox txt = ctrl as TextBox;
                            row[item.Column] = txt.Text;
                        }
                        break;
                    case AjaxTreeViewColumnType.ComboBoxColumn:
                        ctrlId = "ddl" + item.Column;
                        ctrl = this._updatePanel.ContentTemplateContainer.FindControl(ctrlId);
                        if (ctrl != null && ctrl is DropDownList)
                        {
                            DropDownList ddl = ctrl as DropDownList;
                            row[item.Column] = ddl.SelectedValue;
                        }
                        break;
                    case AjaxTreeViewColumnType.RefValColumn:
                        ctrlId = "ref" + item.Column;
                        ctrl = this._updatePanel.ContentTemplateContainer.FindControl(ctrlId);
                        if (ctrl != null && ctrl is WebRefVal)
                        {
                            WebRefVal refval = ctrl as WebRefVal;
                            row[item.Column] = refval.BindingValue;
                        }
                        break;
                    case AjaxTreeViewColumnType.CalendarColumn:
                        ctrlId = "cal" + item.Column;
                        ctrl = this._updatePanel.ContentTemplateContainer.FindControl(ctrlId);
                        if (ctrl != null && ctrl is AjaxCalendar)
                        {
                            AjaxCalendar cal = ctrl as AjaxCalendar;
                            if(!string.IsNullOrEmpty(cal.Text))
                                row[item.Column] = Convert.ToDateTime(cal.Text);
                            else
                                row[item.Column] = DateTime.Now;
                        }
                        break;
                    case AjaxTreeViewColumnType.RadioButtonColumn:
                        break;
                }
            }
        }
    }

    public class ButtonSelectEditor : UITypeEditor
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [PermissionSet(SecurityAction.Demand)]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance != null)
            {
                ControlCollection ctrlList = ((Control)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is IButtonControl)
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
                if (selector.Execute(ref strValue))
                    value = strValue;
            }
            return value;
        }
    }

    public class TreeViewSelectEditor : UITypeEditor
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [PermissionSet(SecurityAction.Demand)]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance != null)
            {
                ControlCollection ctrlList = ((Control)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is TreeView)
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
                if (selector.Execute(ref strValue))
                    value = strValue;
            }
            return value;
        }
    }

    public class RefDataFieldEditor : UITypeEditor
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [PermissionSet(SecurityAction.Demand)]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance != null && context.Instance is IRefDataSource)
            {
                IRefDataSource refSource = context.Instance as IRefDataSource;
                ControlCollection ctrlList = null;
                if (context.Instance is WebControl)
                {
                    ctrlList = ((WebControl)context.Instance).Page.Controls;
                }
                else if (context.Instance is InfoOwnerCollectionItem && ((InfoOwnerCollectionItem)context.Instance).Owner is WebControl)
                {
                    ctrlList = (((InfoOwnerCollectionItem)context.Instance).Owner as WebControl).Page.Controls;
                }
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource && ctrl.ID == refSource.DataSourceID)
                    {
                        WebDataSource wds = (WebDataSource)ctrl;
                        DataTable srcTab = null;
                        if (string.IsNullOrEmpty(wds.SelectAlias) && string.IsNullOrEmpty(wds.SelectCommand))
                        {
                            if (wds.DesignDataSet == null)
                            {
                                WebDataSet ds = GloFix.CreateDataSet(wds.WebDataSetID);
                                wds.DesignDataSet = ds.RealDataSet;
                            }
                            srcTab = wds.DesignDataSet.Tables[wds.DataMember].Clone();
                        }
                        else
                        {
                            srcTab = wds.CommandTable.Clone();
                        }
                        foreach (DataColumn column in srcTab.Columns)
                        {
                            objName.Add(column.ColumnName);
                        }
                        break;
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

    public class AjaxTreeViewItemCollection : InfoOwnerCollection
    {
        public AjaxTreeViewItemCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(AjaxTreeViewItem))
        {

        }
        public new AjaxTreeViewItem this[int index]
        {
            get
            {
                return (AjaxTreeViewItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is AjaxTreeViewItem)
                    {
                        //原来的Collection设置为0
                        ((AjaxTreeViewItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((AjaxTreeViewItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class AjaxTreeViewItem : InfoOwnerCollectionItem, IRefDataSource
    {
        public AjaxTreeViewItem()
        {
        }

        public override string Name
        {
            get { return _column; }
            set { _column = value; }
        }

        private string _caption;
        [Category("InfoLight")]
        [NotifyParentProperty(true)]
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        private string _column;
        [Category("InfoLight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string Column
        {
            get
            {
                return _column;
            }
            set
            {
                _column = value;
                if (string.IsNullOrEmpty(_caption))
                    _caption = _column;
            }
        }

        private AjaxTreeViewColumnType _columnType;
        [Category("InfoLight")]
        [NotifyParentProperty(true)]
        public AjaxTreeViewColumnType ColumnType
        {
            get { return _columnType; }
            set { _columnType = value; }
        }

        private string _dataSourceId;
        [Category("Data")]
        [NotifyParentProperty(true)]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string DataSourceID
        {
            get
            {
                return _dataSourceId;
            }
            set
            {
                if (this.Owner != null && (this.Owner as AjaxTreeViewPopup).Site.DesignMode 
                    && _columnType != AjaxTreeViewColumnType.ComboBoxColumn 
                    && _columnType != AjaxTreeViewColumnType.RefValColumn)
                {
                    System.Windows.Forms.MessageBox.Show("Only when the property of 'ColumnType' is set to ComboBoxColumn or RefValColumn can DataSourceId be set.");
                    return;
                }
                _dataSourceId = value;
            }
        }

        private string _dataTextField;
        [Category("Data")]
        [NotifyParentProperty(true)]
        [Editor(typeof(RefDataFieldEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string DataTextField
        {
            get
            {
                return _dataTextField;
            }
            set
            {
                if (this.Owner != null && (this.Owner as AjaxTreeViewPopup).Site.DesignMode 
                    && _columnType != AjaxTreeViewColumnType.ComboBoxColumn 
                    && _columnType != AjaxTreeViewColumnType.RefValColumn)
                {
                    System.Windows.Forms.MessageBox.Show("Only when the property of 'ColumnType' is set to ComboBoxColumn or RefValColumn can DataTextField be set.");
                    return;
                }
                _dataTextField = value;
            }
        }

        private string _dataValueField;
        [Category("Data")]
        [NotifyParentProperty(true)]
        [Editor(typeof(RefDataFieldEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string DataValueField
        {
            get
            {
                return _dataValueField;
            }
            set
            {
                if (this.Owner != null && (this.Owner as AjaxTreeViewPopup).Site.DesignMode 
                    && _columnType != AjaxTreeViewColumnType.ComboBoxColumn 
                    && _columnType != AjaxTreeViewColumnType.RefValColumn)
                {
                    System.Windows.Forms.MessageBox.Show("Only when the property of 'ColumnType' is set to ComboBoxColumn or RefValColumn can DataValueField be set.");
                    return;
                }
                _dataValueField = value;
            }
        }
    }
}
