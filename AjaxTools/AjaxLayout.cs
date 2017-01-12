using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Web.UI;
using System.ComponentModel.Design;
using Srvtools;
using System.Data;

namespace AjaxTools
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [Designer(typeof(AjaxLayoutDesigner), typeof(IDesigner))]
    public class AjaxLayout : AjaxBaseControl
    {
        int _width = 900;
        int _height = 700;
        bool _getServerText = true;
        string _title = "view/master+details";
        string _viewTitle = "view";
        string _layoutPanel = "";
        string _viewPanel = "";
        bool _collapsed = false;
        MultiViewItemCollection _masters;
        MultiViewItemCollection _details;
        ExtGridToolItemCollection _toolItems;

        #region Properties
        [Category("InfoLight")]
        [DefaultValue(900)]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        [Category("InfoLight")]
        [DefaultValue(700)]
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        [Category("InfoLight")]
        [DefaultValue(false)]
        public bool PanelCollapsed
        {
            get { return _collapsed; }
            set { _collapsed = value; }
        }

        [Category("InfoLight")]
        [DefaultValue(true)]
        public bool GetServerText
        {
            get { return _getServerText; }
            set { _getServerText = value; }
        }

        [Category("InfoLight")]
        [DefaultValue("view/master+details")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [Category("InfoLight")]
        [DefaultValue("view")]
        public string ViewTitle
        {
            get { return _viewTitle; }
            set { _viewTitle = value; }
        }

        [Category("Layout")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string LayoutPanel
        {
            get { return _layoutPanel; }
            set { _layoutPanel = value; }
        }
        [Category("Layout")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string View
        {
            get { return _viewPanel; }
            set { _viewPanel = value; }
        }

        [Category("Layout")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public ExtGridToolItemCollection ToolItems
        {
            get
            {
                if (_toolItems == null)
                {
                    _toolItems = new ExtGridToolItemCollection(this, typeof(ExtGridToolItem));
                }
                return _toolItems;
            }
        }

        [Category("Layout")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public MultiViewItemCollection Masters
        {
            get
            {
                if (_masters == null)
                    _masters = new MultiViewItemCollection(this, typeof(MultiViewItem));
                return _masters;
            }
        }

        [Category("Layout")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public MultiViewItemCollection Details
        {
            get
            {
                if (_details == null)
                    _details = new MultiViewItemCollection(this, typeof(MultiViewItem));
                return _details;
            }
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.RenderLayout();
        }

        public void RenderLayout()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("var {0}Config=", this.ID);
            builder.Append("{");
            if (!string.IsNullOrEmpty(this.View))
            {
                AjaxGridView view = this.GetObjByID(this.View) as AjaxGridView;
                if (view != null)
                {
                    builder.AppendFormat("view:{0},", view.GenGrid(true));
                }
            }
            if (this.Masters.Count > 0)
            {
                builder.AppendFormat("masters:[{0}],masterKeyFields:[{1}],masterFields:[{2}],masterValids:[{3}],masterTools:[{4}],masterFocusEventHandlers:{5},masterLeaveEventHandlers:{6},", 
                    this.GenMasters(),
                    this.GenMasterFields(true),
                    this.GenMasterFields(false),
                    this.GenMasterValids(),
                    this.GenMasterTools(),
                    this.GenMasterEventHandlers("focus"),
                    this.GenMasterEventHandlers("leave"));
            }
            if (this.Details.Count > 0)
            {
                builder.AppendFormat("details:[{0}],", this.GenDetails());
            }
            builder.AppendFormat("layout:{0},viewTitle:'{1}'", 
                this.GenLayoutConfig(),
                this.ViewTitle.Replace("'", @"\'"));
            builder.Append("};");
            builder.AppendFormat("Ext.onReady(function(){{Infolight.Layout.initLayout({0}Config);}});", this.ID);
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), builder.ToString(), true);
        }

        private string GenMasterEventHandlers(string eventName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (MultiViewItem item in this.Masters)
            {
                AjaxFormView master = this.GetObjByID(item.ControlId) as AjaxFormView;
                if (master != null)
                {
                    builder.Append(master.GenEventHandlers(eventName));
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("]");
            return builder.ToString();
        }

        private string GenLayoutConfig()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.AppendFormat("id:'{0}',title:'{1}',width:{2},height:{3},panelcollapsed:{4},", this.ID, this.Title.Replace("'", @"\'"), this.Width, this.Height, this.PanelCollapsed.ToString().ToLower());
            if (string.IsNullOrEmpty(this.LayoutPanel))
            {
                builder.Append("renderTo:Ext.getBody(),");
            }
            else
            {
                builder.AppendFormat("renderTo:'{0}',", this.LayoutPanel);
            }
            builder.Append("labelAlign:'left',layout:'border'");
            builder.Append("}");
            return builder.ToString();
        }

        private string GenMasters()
        {
            StringBuilder builder = new StringBuilder();
            foreach (MultiViewItem item in this.Masters)
            {
                if (!string.IsNullOrEmpty(item.ControlId))
                {
                    AjaxFormView master = this.GetObjByID(item.ControlId) as AjaxFormView;
                    if (master != null)
                    {
                        builder.AppendFormat("{0},", master.GenFormView(item.Title.Replace("'", @"\'")));
                    }
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        private string GenMasterFields(bool keyFields)
        {
            StringBuilder builder = new StringBuilder();
            foreach (MultiViewItem item in this.Masters)
            {
                if (!string.IsNullOrEmpty(item.ControlId))
                {
                    AjaxFormView master = this.GetObjByID(item.ControlId) as AjaxFormView;
                    if (master != null)
                    {
                        if (keyFields)
                        {
                            string itemKeyFields = master.GenKeyFields();
                            if (!string.IsNullOrEmpty(itemKeyFields))
                            {
                                builder.AppendFormat("{0},", itemKeyFields);
                            }
                        }
                        else
                        {
                            string itemFields = master.GenFields();
                            if (!string.IsNullOrEmpty(itemFields))
                            {
                                builder.AppendFormat("{0},", itemFields);
                            }
                        }
                    }
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        private string GenMasterValids()
        {
            StringBuilder builder = new StringBuilder();
            foreach (MultiViewItem item in this.Masters)
            {
                if (!string.IsNullOrEmpty(item.ControlId))
                {
                    AjaxFormView master = this.GetObjByID(item.ControlId) as AjaxFormView;
                    if (master != null)
                    {
                        string itemValids = master.GenValidJsonArray();
                        if (!string.IsNullOrEmpty(itemValids))
                        {
                            builder.AppendFormat("{0},", itemValids);
                        }
                    }
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        private string GenDetails()
        {
            StringBuilder builder = new StringBuilder();
            foreach (MultiViewItem item in this.Details)
            {
                if (!string.IsNullOrEmpty(item.ControlId))
                {
                    AjaxGridView detail = this.GetObjByID(item.ControlId) as AjaxGridView;
                    if (detail != null)
                    {
                        //if (genObject)
                        //{
                        builder.AppendFormat("{{grid:{0},valids:[{1}],title:'{2}'}},",
                            (this.Details.Count > 1) ? detail.GenGrid(true) : detail.GenGrid(true, item.Title.Replace("'", @"\'")),
                            detail.GenValidJsonArray(),
                            item.Title.Replace("'", @"\'"));
                        //}
                        //else
                        //{
                        //    builder.AppendFormat("{{id:'{0}',keys:{1}}},", 
                        //        detail.ID,
                        //        detail.GenKeyFieldsArray());
                        //}
                    }
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        public string GenMasterTools()
        {
            StringBuilder builder = new StringBuilder();
            if (this.ToolItems.Count > 0)
            {
                object ajaxFormView = this.Page.FindControl(this.Masters[0].ToString());
                Type type = ajaxFormView.GetType();
                System.Reflection.PropertyInfo info = type.GetProperty("DataSourceID");
                String afvDataSourceID = info.GetValue(ajaxFormView, null).ToString();
                WebDataSource wdsMaster = this.Page.FindControl(afvDataSourceID) as WebDataSource; 
                string[] toolTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ToolItems", true).Split(',');
                for (int i = 0; i < this.ToolItems.Count; i++)
                {
                    if (!wdsMaster.AllowAdd && this.ToolItems[i].SysHandlerType == ExtGridSystemHandler.Add)
                        continue;
                    if (!wdsMaster.AllowDelete && this.ToolItems[i].SysHandlerType == ExtGridSystemHandler.Delete)
                        continue;
                    if (!wdsMaster.AllowUpdate && this.ToolItems[i].SysHandlerType == ExtGridSystemHandler.Edit)
                        continue;
                    if (!wdsMaster.AllowAdd && !wdsMaster.AllowUpdate
                        && (this.ToolItems[i].SysHandlerType == ExtGridSystemHandler.OK || this.ToolItems[i].SysHandlerType == ExtGridSystemHandler.Cancel))
                        continue;

                    switch (this.ToolItems[i].ToolItemType)
                    {
                        case ExtGridToolItemType.Button:
                            builder.Append("{");
                            if (!string.IsNullOrEmpty(this.ToolItems[i].ButtonName))
                            {
                                builder.AppendFormat("id:'{0}{1}',", this.ID, this.ToolItems[i].ButtonName);
                            }
                            if (!string.IsNullOrEmpty(this.ToolItems[i].Text))
                            {
                                string text = this.ToolItems[i].Text;
                                if (this.GetServerText)
                                {
                                    switch (this.ToolItems[i].SysHandlerType)
                                    {
                                        case ExtGridSystemHandler.Add: text = toolTexts[0]; break;
                                        case ExtGridSystemHandler.Edit: text = toolTexts[1]; break;
                                        case ExtGridSystemHandler.Delete: text = toolTexts[2]; break;
                                        case ExtGridSystemHandler.OK: text = toolTexts[7]; break;
                                        case ExtGridSystemHandler.Cancel: text = toolTexts[8]; break;
                                        case ExtGridSystemHandler.Save: text = toolTexts[3]; break;
                                        case ExtGridSystemHandler.Abort: text = toolTexts[4]; break;
                                        case ExtGridSystemHandler.Query: text = toolTexts[6]; break;
                                        case ExtGridSystemHandler.Refresh: text = toolTexts[5]; break;
                                    }
                                }
                                builder.AppendFormat("text:'{0}',", text);
                            }
                            if (!string.IsNullOrEmpty(this.ToolItems[i].IconUrl))
                            {
                                builder.AppendFormat("icon:'{0}',", this.ResolveClientUrl(this.ToolItems[i].IconUrl));
                            }
                            if (!string.IsNullOrEmpty(this.ToolItems[i].CssClass))
                            {
                                builder.AppendFormat("cls:'{0}',", this.ToolItems[i].CssClass);
                            }
                            builder.Append("handler:function(sender, args){");
                            WebDataSource wds = this.GetMasterDataSource();
                            if (wds != null)
                            {
                                string custHandler = this.ToolItems[i].Handler;
                                if (!string.IsNullOrEmpty(custHandler))
                                {
                                    builder.AppendFormat("var c={0}();if(c===false){{return;}}", custHandler);
                                }
                                if (this.ToolItems[i].SysHandlerType != ExtGridSystemHandler.CustomDefine)
                                {
                                    builder.AppendFormat("Ext.getCmp('{0}').", this.ID);
                                }
                                switch (this.ToolItems[i].SysHandlerType)
                                {
                                    case ExtGridSystemHandler.Add:
                                        builder.AppendFormat("addRecord({0});", this.GenDefaultValues(wds));
                                        break;
                                    case ExtGridSystemHandler.Edit:
                                        builder.Append("editRecord();");
                                        break;
                                    case ExtGridSystemHandler.Delete:
                                        builder.AppendFormat("deleteRecord({0});", wds.AutoApply.ToString().ToLower());
                                        break;
                                    case ExtGridSystemHandler.OK:
                                        builder.AppendFormat("submitRecord({0});", wds.AutoApply.ToString().ToLower());
                                        break;
                                    case ExtGridSystemHandler.Cancel:
                                        builder.Append("cancelRecord();");
                                        break;
                                    case ExtGridSystemHandler.Save:
                                        builder.Append("saveRecords();");
                                        break;
                                    case ExtGridSystemHandler.Abort:
                                        builder.Append("abortRecords();");
                                        break;
                                    case ExtGridSystemHandler.CustomDefine:
                                        break;
                                }
                            }
                            builder.Append("}}");
                            break;
                        case ExtGridToolItemType.Label:
                            builder.AppendFormat("'{0}'", this.ToolItems[i].Text);
                            break;
                        case ExtGridToolItemType.Separation:
                            builder.AppendFormat("'-'");
                            break;
                        case ExtGridToolItemType.Fill:
                            builder.AppendFormat("'->'");
                            break;
                    }
                    if (i < this.ToolItems.Count - 1)
                    {
                        builder.Append(",");
                    }
                }
            }
            if (builder.ToString().EndsWith(","))
                builder = builder.Remove(builder.ToString().LastIndexOf(","), 1);
            return builder.ToString();
        }

        public string GenDefaultValues(WebDataSource wds)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            foreach (MultiViewItem item in this.Masters)
            {
                AjaxFormView formView = this.GetObjByID(item.ControlId) as AjaxFormView;
                string itemDefaultValues = formView.GenDefaultValues(wds);
                if (!string.IsNullOrEmpty(itemDefaultValues))
                {
                    builder.AppendFormat("{0},", itemDefaultValues);
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("}");
            return builder.ToString();
        }

        private WebDataSource GetMasterDataSource()
        {
            if (this.Masters.Count > 0)
            {
                foreach (MultiViewItem item in this.Masters)
                {
                    AjaxFormView fv = this.GetObjByID(item.ControlId) as AjaxFormView;
                    if (fv != null)
                    {
                        return this.GetObjByID(fv.DataSourceID) as WebDataSource;
                    }
                }
            }
            return null;
        }
    }

    public class MultiViewItem : InfoOwnerCollectionItem
    {
        string _controlId = "";
        string _title = "";

        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string ControlId
        {
            get { return _controlId; }
            set { _controlId = value; }
        }

        [NotifyParentProperty(true)]
        [DefaultValue("")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return _controlId; }
            set { _controlId = value; }
        }

        public override string ToString()
        {
            return _controlId;
        }
    }
}