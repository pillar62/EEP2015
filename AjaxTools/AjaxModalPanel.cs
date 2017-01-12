using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Srvtools;
using System.Data;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Reflection;

namespace AjaxTools
{
    [ParseChildren(false)]
    [Designer(typeof(ContainerControlDesigner))]
    public class AjaxModalPanel : AjaxBaseWebControl, INamingContainer, IModalPanel
    {
        private Button _hidTarget = new Button();
        private ModalPopupExtender _modalPopupExtender = new ModalPopupExtender();
        private Panel _popupContainer = new Panel();
        private ImageButton _btnClose = new ImageButton();
        private Panel _contentPanel = new Panel();

        #region Properties
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //[Browsable(false)]
        //public string DataItem
        //{
        //    get { return this.ViewState["DataItem"]; }
        //    set { this.ViewState["DataItem"] = value; }
        //}

        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        private string _triggerUpdatePanel = "";
        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(TriggerUpdatePanelEditor), typeof(UITypeEditor))]
        public string TriggerUpdatePanel
        {
            get { return _triggerUpdatePanel; }
            set { _triggerUpdatePanel = value; }
        }

        private string _dataContainer = "";
        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(DataContainerEditor), typeof(UITypeEditor))]
        public string DataContainer
        {
            get { return _dataContainer; }
            set { _dataContainer = value; }
        }

        /*private string _viewCommandName = "";
        [Category("Infolight")]
        [DefaultValue("")]
        public string ViewCommandName
        {
            get { return _viewCommandName; }
            set { _viewCommandName = value; }
        }

        private string _editCommandName = "";
        [Category("Infolight")]
        [DefaultValue("")]
        public string EditCommandName
        {
            get { return _editCommandName; }
            set { _editCommandName = value; }
        }

        private string _insertCommandName = "";
        [Category("Infolight")]
        [DefaultValue("")]
        public string InsertCommandName
        {
            get { return _insertCommandName; }
            set { _insertCommandName = value; }
        }*/

        private bool _moveable = false;
        [Category("Infolight")]
        [DefaultValue(false)]
        public bool Moveable
        {
            get { return _moveable; }
            set { _moveable = value; }
        }

        private string _caption = "";
        [Category("Infolight")]
        [DefaultValue("")]
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string OriginalWhereString
        {
            get
            {
                object obj = this.ViewState["OriginalWhereString"];
                if (obj != null && obj is string)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["OriginalWhereString"] = value;
            }
        }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Control control = this.GetTriggerUpdatePanel();
            if (control != null && control is UpdatePanel)
            {
                UpdatePanel panel = control as UpdatePanel;
                bool closeTriggerExist = false;
                foreach (AsyncPostBackTrigger trigger in panel.Triggers)
                {
                    if (trigger.ControlID == this._btnClose.UniqueID && trigger.EventName == "Click")
                    {
                        closeTriggerExist = true;
                        break;
                    }
                }
                if (!closeTriggerExist)
                {
                    AsyncPostBackTrigger triggerClose = new AsyncPostBackTrigger();
                    triggerClose.ControlID = this._btnClose.UniqueID;
                    triggerClose.EventName = "Click";
                    panel.Triggers.Add(triggerClose);
                }
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            //_hidTarget
            _hidTarget = new Button();
            _hidTarget.ID = "hidTarget";
            this.Controls.Add(_hidTarget);

            //_popupContainer
            _popupContainer = new Panel();
            _popupContainer.ID = "popupContainer";
            this.Controls.Add(_popupContainer);

            //_btnClose
            _btnClose = new ImageButton();
            _btnClose.ID = "__btnClose";
            _btnClose.ImageUrl = "~/Image/Ajax/close.gif";
            _btnClose.Click += new ImageClickEventHandler(_btnClose_Click);
            this.Controls.Add(_btnClose);

            //_contentPanel
            _contentPanel = new Panel();
            _contentPanel.ID = "contentPanel";
            this.Controls.Add(_contentPanel);

            if (!this.DesignMode)
            {
                _modalPopupExtender = new ModalPopupExtender();
                _modalPopupExtender.ID = "modalPopupExtender";
                _modalPopupExtender.TargetControlID = this._hidTarget.UniqueID;
                _modalPopupExtender.PopupControlID = this._popupContainer.UniqueID;
                _modalPopupExtender.PopupDragHandleControlID = this._popupContainer.UniqueID;
                _modalPopupExtender.BackgroundCssClass = "ajaxmodalpan_modalBackground";
                //_modalPopupExtender.CancelControlID = this._btnClose.UniqueID;
                _modalPopupExtender.Drag = this.Moveable;
                _modalPopupExtender.BehaviorID = this.ClientID + "_showModalBehavior";
                this.Controls.Add(_modalPopupExtender);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //hiden_target
            writer.AddStyleAttribute("display", "none");
            _hidTarget.RenderControl(writer);

            //popup
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxmodalpan_popup");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.IsEmpty ? "300px" : this.Width.ToString());
            writer.AddStyleAttribute("display", "none");
            _popupContainer.RenderBeginTag(writer);
            //title
            writer.AddAttribute("id", "divTitle");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxmodalpan_div_title");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.IsEmpty ? "300px" : this.Width.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Div); //<div>
            writer.AddStyleAttribute("cellpadding", "0");
            writer.AddStyleAttribute("cellspacing", "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.IsEmpty ? "286px" : (this.Width.Value - 14).ToString() + "px");
            writer.AddStyleAttribute("font-family", "Verdana");
            writer.AddStyleAttribute("font-size", "8pt");
            writer.AddStyleAttribute("text-align", "center");
            writer.AddStyleAttribute("vertical-align", "bottom");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(this.Caption);
            writer.RenderEndTag();

            writer.AddStyleAttribute("width", "14px");
            writer.AddStyleAttribute("vertical-align", "top");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            _btnClose.CssClass = "ajaxmodalpan_btnClose";
            _btnClose.RenderControl(writer);
            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderEndTag();//</div>

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxmodalpan_div_content");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.IsEmpty ? "300px" : this.Width.ToString());
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, this.Height.IsEmpty ? "300px" : this.Height.ToString());
            _contentPanel.RenderBeginTag(writer);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.ID != "hidTarget" && ctrl.ID != "popupContainer" && ctrl.ID != "__btnClose" && ctrl.ID != "contentPanel")
                    ctrl.RenderControl(writer);
            }
            _contentPanel.RenderEndTag(writer);

            _popupContainer.RenderEndTag(writer);
        }

        public Control GetTriggerUpdatePanel()
        {
            object obj = this.GetObjByID(this.TriggerUpdatePanel);
            if (obj != null && obj is UpdatePanel)
            {
                return obj as Control;
            }
            return null;
        }

        private List<CompositeDataBoundControl> GetDBControl()
        {
            List<CompositeDataBoundControl> returnValue = new List<CompositeDataBoundControl>();
            String[] dataContainers = this.DataContainer.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in dataContainers)
            {
                Control ctrl = this.FindControl(item);
                if (ctrl != null && ctrl is CompositeDataBoundControl)
                {
                    returnValue.Add(ctrl as CompositeDataBoundControl);
                }
            }

            return returnValue;
        }

        private void ReBindSource()
        {
            //CompositeDataBoundControl dbControl = this.GetDBControl();
            //if (dbControl != null)
            //{
            //    object obj_ds = this.GetObjByID(dbControl.DataSourceID);
            //    if (obj_ds != null && obj_ds is WebDataSource)
            //    {
            //        WebDataSource wds = obj_ds as WebDataSource;
            //        wds.SetWhere(this.OriginalWhereString);
            //        Control ctrl = this.GetTriggerUpdatePanel();
            //        if (ctrl != null)
            //        {
            //            ctrl.DataBind();
            //        }
            //    }
            //}
            object objGrid = this.GetObjByID(this.ViewState["GridControl"].ToString());
            if (objGrid != null && objGrid is WebGridView)
            {
                WebGridView gdv = objGrid as WebGridView;
                gdv.DataBind();
            }
        }

        void _btnClose_Click(object sender, ImageClickEventArgs e)
        {
            this.Close();
        }

        public void Open(WebGridView.OpenEditMode mode)
        {
            Open(mode, null);
        }

        public void Open(WebGridView.OpenEditMode mode, GridViewCommandEventArgs args)
        {
            List<CompositeDataBoundControl> dbControls = this.GetDBControl();
            foreach (var dbControl in dbControls)
            {
                if (dbControl != null)
                {
                    if (mode != WebGridView.OpenEditMode.Insert)
                    {
                        Type dbCtrlType = dbControl.GetType();
                        MethodInfo executeSyncMethod = dbCtrlType.GetMethod("ExecuteSync", new Type[] { typeof(GridViewCommandEventArgs) });
                        if (executeSyncMethod != null)
                        {
                            executeSyncMethod.Invoke(dbControl, new object[] { args });
                        }
                    }
                    this.ChangeMode(mode, dbControl);
                }
            }

            ScriptManager.RegisterStartupScript(this.GetTriggerUpdatePanel(), this.GetType(), "popup", "$find('" + this.ClientID + "_showModalBehavior').show();", true);
            if (args != null && args.CommandArgument != null)
                this.ViewState["GridControl"] = args.CommandArgument.ToString();
        }

        public void Submit()
        {
            Submit(true);
        }

        public void Submit(bool closePanel)
        {
            List<CompositeDataBoundControl> dbControls = this.GetDBControl();
            foreach (var dbControl in dbControls)
            {
                bool isInserting = false;
                if (dbControl != null)
                {
                    if (dbControl is FormView)
                    {
                        FormView formView = dbControl as FormView;
                        if (formView.CurrentMode == FormViewMode.Insert)
                        {
                            isInserting = true;
                            formView.InsertItem(false);
                        }
                        else if (formView.CurrentMode == FormViewMode.Edit)
                        {
                            formView.UpdateItem(false);
                        }
                        if (formView.CurrentMode != FormViewMode.ReadOnly)
                            return;
                    }
                    else if (dbControl is DetailsView)
                    {
                        DetailsView detailsView = dbControl as DetailsView;
                        if (detailsView.CurrentMode == DetailsViewMode.Insert)
                        {
                            isInserting = true;
                            detailsView.InsertItem(false);
                        }
                        else if (detailsView.CurrentMode == DetailsViewMode.Edit)
                        {
                            detailsView.UpdateItem(false);
                        }
                        if (detailsView.CurrentMode != DetailsViewMode.ReadOnly)
                            return;
                    }
                }
                ReBindSource();
                if (!closePanel && isInserting)
                {
                    this.Open(WebGridView.OpenEditMode.Insert);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.GetTriggerUpdatePanel(), this.GetType(), "popup", "$find('" + this.ClientID + "_showModalBehavior').hide();", true);
                }
            }
        }

        public void Close()
        {
            clearValidate();
            //ReBindSource();
            ScriptManager.RegisterStartupScript(this.GetTriggerUpdatePanel(), this.GetType(), "popup", "$find('" + this.ClientID + "_showModalBehavior').hide();", true);
        }

        private void clearValidate()
        {
            List<CompositeDataBoundControl> dbControls = this.GetDBControl();
            foreach (var dbControl in dbControls)
            {
                if (dbControl != null)
                {
                    WebValidate validate = null;
                    if (dbControl is WebFormView)
                    {
                        WebFormView formView = dbControl as WebFormView;
                        validate = (WebValidate)formView.ExtendedFindChildControl(formView.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
                    }
                    else if (dbControl is WebDetailsView)
                    {
                        WebDetailsView detailsView = dbControl as WebDetailsView;
                        validate = (WebValidate)detailsView.ExtendedFindChildControl(detailsView.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
                    }
                    if (validate != null)
                        validate.Text = string.Empty;
                }
            }
        }

        public void ChangeMode(WebGridView.OpenEditMode mode, CompositeDataBoundControl dbControl)
        {
            if (mode == WebGridView.OpenEditMode.View)
            {
                if (dbControl is FormView)
                {
                    ((FormView)dbControl).ChangeMode(FormViewMode.ReadOnly);
                }
                else if (dbControl is DetailsView)
                {
                    ((DetailsView)dbControl).ChangeMode(DetailsViewMode.ReadOnly);
                }
            }
            else if (mode == WebGridView.OpenEditMode.Update)
            {
                if (dbControl is FormView)
                {
                    ((FormView)dbControl).ChangeMode(FormViewMode.Edit);
                }
                else if (dbControl is DetailsView)
                {
                    ((DetailsView)dbControl).ChangeMode(DetailsViewMode.Edit);
                }
            }
            else if (mode == WebGridView.OpenEditMode.Insert)
            {
                if (dbControl is FormView)
                {
                    ((FormView)dbControl).ChangeMode(FormViewMode.Insert);
                }
                else if (dbControl is DetailsView)
                {
                    ((DetailsView)dbControl).ChangeMode(DetailsViewMode.Insert);
                }
            }
        }
    }

    public class TriggerUpdatePanelEditor : UITypeEditor
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
                    if (ctrl is UpdatePanel)
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

    public class DataContainerEditor : UITypeEditor
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
            if (context.Instance != null && context.Instance is AjaxModalPanel)
            {
                AjaxModalPanel panel = context.Instance as AjaxModalPanel;
                if (panel.Controls.Count > 0 && panel.Controls[0] is UpdatePanel)
                {
                    UpdatePanel updatepanel = panel.Controls[0] as UpdatePanel;
                    string html = (updatepanel.ContentTemplate as TemplateBuilder).Text;
                    IDesignerHost host = (IDesignerHost)updatepanel.Site.GetService(typeof(IDesignerHost));
                    Control[] ctrls = ControlParser.ParseControls(host, html);

                    foreach (Control ctrl in ctrls)
                    {
                        if (ctrl is FormView || ctrl is DetailsView)
                        {
                            objName.Add(ctrl.ID);
                        }
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
}