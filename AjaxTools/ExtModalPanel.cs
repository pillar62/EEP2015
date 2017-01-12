using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Drawing.Design;
using Srvtools;

namespace AjaxTools
{
    [ParseChildren(false)]
    [Designer(typeof(ContainerControlDesigner))]
    public class ExtModalPanel : AjaxBaseWebControl, INamingContainer
    {
        public ExtModalPanel()
        {
        }

        string _title = "";

        ExtModalPanelMode _mode = ExtModalPanelMode.OkCancel;
        ExtModalPanelType _modalType = ExtModalPanelType.InsertUpdate;

        Button btnSubmit = new Button();
        Button btnCancel = new Button();

        [Category("Infolight")]
        [DefaultValue("")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(ExtModalPanelMode), "OkCancel")]
        public ExtModalPanelMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(ExtModalPanelType), "InsertUpdate")]
        public ExtModalPanelType ModalType
        {
            get { return _modalType; }
            set { _modalType = value; }
        }

        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            string[] uiTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtModelPanel", "UIText", true).Split(',');

            this.btnSubmit = new Button();
            this.btnSubmit.ID = "btnSubmit";
            this.btnSubmit.Text = uiTexts[0];
            this.btnSubmit.Width = new Unit(60);
            this.btnSubmit.Click += new EventHandler(btnSubmit_Click);
            this.Controls.Add(btnSubmit);

            this.btnCancel = new Button();
            this.btnCancel.ID = "btnCancel";
            this.btnCancel.Text = uiTexts[1];
            this.btnCancel.Width = new Unit(60);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.Controls.Add(btnCancel);
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            this.OnSubmit(EventArgs.Empty);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.OnCancel(EventArgs.Empty);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", this.ID);
            writer.AddAttribute("class", "x-window");
            writer.AddStyleAttribute("width", this.Width.ToString());
            writer.AddStyleAttribute("display", "none");
            writer.RenderBeginTag("div");
            // header
            writer.AddAttribute("class", "x-window-tl");
            writer.AddAttribute("id", string.Format("{0}header", this.ID));
            writer.RenderBeginTag("div");
            writer.AddAttribute("class", "x-window-tr");
            writer.RenderBeginTag("div");
            writer.AddAttribute("class", "x-window-tc");
            writer.RenderBeginTag("div");
            writer.AddAttribute("class", "x-window-header");
            if (string.IsNullOrEmpty(this.Title))
            {
                writer.AddStyleAttribute("height", "18px");
            }
            writer.RenderBeginTag("div");
            writer.AddAttribute("class", "x-window-header-text");
            writer.RenderBeginTag("span");
            writer.Write(this.Title);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            // content
            writer.AddAttribute("class", "x-window-ml");
            writer.RenderBeginTag("div");
            writer.AddAttribute("class", "x-window-mr");
            writer.RenderBeginTag("div");
            writer.AddAttribute("class", "x-window-mc");
            writer.RenderBeginTag("div");
            writer.AddAttribute("class", "x-window-body");
            writer.AddStyleAttribute("overflow", "auto");
            writer.RenderBeginTag("div");
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.ID != "btnSubmit" && ctrl.ID != "btnCancel")
                    ctrl.RenderControl(writer);
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            // footer
            writer.AddAttribute("class", "x-window-bl");
            writer.RenderBeginTag("div");
            writer.AddAttribute("class", "x-window-br");
            writer.RenderBeginTag("div");
            writer.AddAttribute("class", "x-window-bc");
            writer.RenderBeginTag("div");
            writer.AddAttribute("class", "x-window-footer");
            writer.AddStyleAttribute("padding", "10px");
            writer.RenderBeginTag("div");
            if (this.Mode == ExtModalPanelMode.OkCancel)
            {
                writer.AddAttribute("class", "x-panel-btns-ct");
                writer.RenderBeginTag("div");
                writer.AddAttribute("class", "x-panel-btns x-panel-btns-center");
                writer.RenderBeginTag("div");
                writer.AddAttribute("cellspacing", "0");
                writer.RenderBeginTag("table");
                writer.RenderBeginTag("tbody");
                writer.RenderBeginTag("tr");
                writer.AddAttribute("class", "x-panel-btn-td");
                writer.RenderBeginTag("td");
                //btnSubmit
                writer.AddAttribute("class", "quid_grid_button");
                btnSubmit.RenderControl(writer);

                writer.RenderEndTag();
                writer.AddAttribute("class", "x-panel-btn-td");
                writer.RenderBeginTag("td");
                //btnCancel
                writer.AddAttribute("class", "quid_grid_button");
                if (this.ModalType == ExtModalPanelType.Query || this.ModalType == ExtModalPanelType.RefButton)
                {
                    writer.AddAttribute("onclick", string.Format("var behavior=$find('{0}behavior');if(behavior){{behavior.hide();}};return false;", this.ID));
                }
                btnCancel.RenderControl(writer);
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        static readonly object submitEventKey = new object();

        public event EventHandler<EventArgs> Submit
        {
            add { this.Events.AddHandler(submitEventKey, value); }
            remove { this.Events.RemoveHandler(submitEventKey, value); }
        }

        protected void OnSubmit(EventArgs e)
        {
            EventHandler<EventArgs> submitHandler = (EventHandler<EventArgs>)this.Events[submitEventKey];
            if (submitHandler != null)
            {
                submitHandler(this, e);
            }
        }

        static readonly object cancelEventKey = new object();

        public event EventHandler<EventArgs> Cancel
        {
            add { this.Events.AddHandler(cancelEventKey, value); }
            remove { this.Events.RemoveHandler(cancelEventKey, value); }
        }

        protected void OnCancel(EventArgs e)
        {
            EventHandler<EventArgs> cancelHandler = (EventHandler<EventArgs>)this.Events[cancelEventKey];
            if (cancelHandler != null)
            {
                cancelHandler(this, e);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is UpdatePanel)
                {
                    UpdatePanel upan = ctrl as UpdatePanel;

                    AsyncPostBackTrigger triggerSubmit = new AsyncPostBackTrigger();
                    triggerSubmit.ControlID = this.btnSubmit.UniqueID;
                    triggerSubmit.EventName = "Click";
                    upan.Triggers.Add(triggerSubmit);

                    AsyncPostBackTrigger triggerCancel = new AsyncPostBackTrigger();
                    triggerCancel.ControlID = this.btnCancel.UniqueID;
                    triggerCancel.EventName = "Click";
                    upan.Triggers.Add(triggerCancel);
                }
            }
        }
    }
}