using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using Srvtools;

namespace AjaxTools
{
    public class AjaxScriptManager :
#if VS90
 ToolkitScriptManager
#else
 ScriptManager
#endif
    {
        ExtResourceFileCollection _resourceFiles;
        bool _renderExtSysResources = true;
        bool _renderExtShowModelScripts = false;
        bool _renderJQuery = true;
        bool _debugMode = false;
        ExtTheme _extTheme = ExtTheme.Default;

        #region Properties
        [Category("Infolight")]
        [DefaultValue(true)]
        public bool CatchErrorMessage
        {
            get
            {
                object obj = this.ViewState["CatchErrorMessage"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["CatchErrorMessage"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        public bool CatchTimeOut
        {
            get
            {
                object obj = this.ViewState["CatchTimeOut"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["CatchTimeOut"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("~/Timeout.aspx?IsFlow=0")]
        public string TimeOutUrl
        {
            get
            {
                object obj = this.ViewState["TimeOutUrl"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "~/Timeout.aspx?IsFlow=0";
            }
            set
            {
                this.ViewState["TimeOutUrl"] = value;
            }
        }

        [Category("ExtJs")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public ExtResourceFileCollection ResourceFiles
        {
            get
            {
                if (_resourceFiles == null)
                    _resourceFiles = new ExtResourceFileCollection(this, typeof(ExtResourceFile));
                return _resourceFiles;
            }
        }

        [Category("ExtJs")]
        [DefaultValue(true)]
        public bool RenderExtSysResources
        {
            get { return _renderExtSysResources; }
            set { _renderExtSysResources = value; }
        }

        [Category("ExtJs")]
        [DefaultValue(false)]
        public bool RenderExtShowModelScripts
        {
            get { return _renderExtShowModelScripts; }
            set { _renderExtShowModelScripts = value; }
        }

        [Category("ExtJs")]
        [DefaultValue(typeof(ExtTheme), "Default")]
        public ExtTheme ExtTheme
        {
            get { return _extTheme; }
            set { _extTheme = value; }
        }

        [Category("JQuery")]
        [DefaultValue(true)]
        public bool RenderJQuery
        {
            get { return _renderJQuery; }
            set { _renderJQuery = value; }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        public bool DebugMode
        {
            get { return _debugMode; }
            set { _debugMode = value; }
        }
        #endregion

        protected override void OnAsyncPostBackError(AsyncPostBackErrorEventArgs e)
        {
            if ((this.CatchTimeOut && e.Exception.Message == "75FF57F7-7AC0-43c8-9454-C92B4A2723BB") || string.IsNullOrEmpty(CliUtils.fLoginUser))
            {
                this.Page.Response.Redirect(TimeOutUrl);
            }
            else if (CatchErrorMessage)
            {
                this.Page.Session["LastAjaxError"] = e.Exception;
                this.Page.Response.Redirect("~/Error.aspx");
            }
            base.OnAsyncPostBackError(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string script =
                "var lastPostBackButtonId = null;" +
                "Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(function (sender, e)" +
                "{" +
                "var prm = Sys.WebForms.PageRequestManager.getInstance();" +
                "if (prm.get_isInAsyncPostBack())" +
                "{" +
                    "if(lastPostBackButtonId == e.get_postBackElement().id)" +
                    "{" +
                        "e.set_cancel(true);" +
                    "}" +
                "}" +
                "lastPostBackButtonId = e.get_postBackElement().id;" +
                "});";
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "avoidRepeatedPostBack", script, true);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            object[] o = CliUtils.CallMethod("GLModule", "GetSrvoolAssemblyName", null);
            if (Convert.ToInt32(o[0]) != 1)
            {
                string assemblyName = o[1].ToString();
                this.Scripts.Add(new ScriptReference("Srvtools.WebRefVal.js", assemblyName));
                this.Scripts.Add(new ScriptReference("Srvtools.WebDateTimePicker.js", assemblyName));
                if (RenderExtShowModelScripts)
                {
                    if (!ScriptRegistered("AjaxControlToolkit.ExtenderBase.BaseScripts.js"))
                        this.Scripts.Add(new ScriptReference("AjaxControlToolkit.ExtenderBase.BaseScripts.js", "AjaxControlToolkit"));
                    if (!ScriptRegistered("AjaxControlToolkit.Common.Common.js"))
                        this.Scripts.Add(new ScriptReference("AjaxControlToolkit.Common.Common.js", "AjaxControlToolkit"));
                    if (!ScriptRegistered("AjaxControlToolkit.Compat.DragDrop.DragDropScripts.js"))
                        this.Scripts.Add(new ScriptReference("AjaxControlToolkit.Compat.DragDrop.DragDropScripts.js", "AjaxControlToolkit"));
                    if (!ScriptRegistered("AjaxControlToolkit.DragPanel.FloatingBehavior.js"))
                        this.Scripts.Add(new ScriptReference("AjaxControlToolkit.DragPanel.FloatingBehavior.js", "AjaxControlToolkit"));
                    if (!ScriptRegistered("AjaxControlToolkit.RoundedCorners.RoundedCornersBehavior.js"))
                        this.Scripts.Add(new ScriptReference("AjaxControlToolkit.RoundedCorners.RoundedCornersBehavior.js", "AjaxControlToolkit"));
                    if (!ScriptRegistered("AjaxControlToolkit.Compat.Timer.Timer.js"))
                        this.Scripts.Add(new ScriptReference("AjaxControlToolkit.Compat.Timer.Timer.js", "AjaxControlToolkit"));
                    if (!ScriptRegistered("AjaxControlToolkit.DropShadow.DropShadowBehavior.js"))
                        this.Scripts.Add(new ScriptReference("AjaxControlToolkit.DropShadow.DropShadowBehavior.js", "AjaxControlToolkit"));
                    if (!ScriptRegistered("AjaxControlToolkit.DynamicPopulate.DynamicPopulateBehavior.js"))
                        this.Scripts.Add(new ScriptReference("AjaxControlToolkit.DynamicPopulate.DynamicPopulateBehavior.js", "AjaxControlToolkit"));
                    if (!ScriptRegistered("AjaxControlToolkit.ModalPopup.ModalPopupBehavior.js"))
                        this.Scripts.Add(new ScriptReference("AjaxControlToolkit.ModalPopup.ModalPopupBehavior.js", "AjaxControlToolkit"));
                }
            }
            AddJsRef();
            string[] cssRefs = new string[] 
            { 
                "UpdateProgressPanel",
                "AjaxDateTimePicker",
                "AjaxRefVal",
                "AjaxModalPanel",
                "AjaxTreeView",
                "AjaxTextBoxWatermark",
                "AjaxResizableControl",
                "AjaxMaskedEdit",
                "AjaxDragPanel",
                "AjaxCollapsiblePanel",
                "AjaxCollapseMenu",
                "AjaxCalendar",
                "ExtModelPanel"
            };
            AddCssRef(cssRefs);

            string virtualPath = this.VirtualPath();
            if (RenderExtSysResources)
            {
                // render necessary css files
                RenderFile(virtualPath + "ExtJs/resources/css/ext-all.css", ResourceFileType.Css);
                switch (this.ExtTheme)
                {
                    case ExtTheme.Access:
                        RenderFile(virtualPath + "ExtJs/resources/css/xtheme-access.css", ResourceFileType.Css);
                        break;
                    case ExtTheme.Default:
                        break;
                    case ExtTheme.Gray:
                        RenderFile(virtualPath + "ExtJs/resources/css/xtheme-gray.css", ResourceFileType.Css);
                        break;
                    case ExtTheme.Slate:
                        RenderFile(virtualPath + "ExtJs/resources/css/xtheme-slate.css", ResourceFileType.Css);
                        break;
                    //case ExtTheme.Vista:
                    //    RenderFile(virtualPath + "ExtJs/resources/css/xtheme-gray.css", ResourceFileType.Css);
                    //    break;
                }
                RenderFile(virtualPath + "ExtJs/infolight/grid/grid.css", ResourceFileType.Css);
                RenderFile(virtualPath + "ExtJs/infolight/menutree/menutree.css", ResourceFileType.Css);
                RenderFile(virtualPath + "ExtJs/infolight/form/form.css", ResourceFileType.Css);
                RenderFile(virtualPath + "ExtJs/ux/css/CheckHeader.css", ResourceFileType.Css);
                // render necessary javascript files
                //if (this.DebugMode)
                //{
                    //RenderFile(virtualPath + "ExtJs/adapter/ext/ext-base-debug.js", ResourceFileType.Javascript);
                    //RenderFile(virtualPath + "ExtJs/ext-debug.js", ResourceFileType.Javascript);
                    RenderFile(virtualPath + "ExtJs/ext-all-debug.js", ResourceFileType.Javascript);
                    //RenderFile(virtualPath + "ExtJs/ext-debug.js", ResourceFileType.Javascript);
                    //RenderFile(virtualPath + "ExtJs/ext-neptune-debug.js", ResourceFileType.Javascript);
                //}
                //else
                //{
                //RenderFile(virtualPath + "ExtJs/adapter/ext/ext-base.js", ResourceFileType.Javascript);
                //RenderFile(virtualPath + "ExtJs/ext-all.js", ResourceFileType.Javascript);
                //RenderFile(virtualPath + "ExtJs/ext-dev.js", ResourceFileType.Javascript);
                //RenderFile(virtualPath + "ExtJs/ext-all-dev.js", ResourceFileType.Javascript);
               // RenderFile(virtualPath + "ExtJs/ext-neptune.js", ResourceFileType.Javascript);
                //RenderFile(virtualPath + "ExtJs/ext.js", ResourceFileType.Javascript);
                //}
                RenderFile(virtualPath + "ExtJs/ux/RowExpander.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/ux/CheckColumn.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/ux/PreviewPlugin.js", ResourceFileType.Javascript);
                //RenderFile(virtualPath + "ExtJs/plugins/NonCollapsingAccordion.js", ResourceFileType.Javascript);
                //RenderFile(virtualPath + "ExtJs/plugins/InfoPagingToolbar.js", ResourceFileType.Javascript);

                // render infolight javascript files
                RenderFile(virtualPath + "ExtJs/infolight/infolight.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/exception/exception.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/validate/validate.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/default/default.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/combo/combo.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/grid/gridHelper.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/grid/gridHelper2.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/grid/grid.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/form/formHelper.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/form/form.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/layout/layout.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/menutree/menutree.js", ResourceFileType.Javascript);
                //RenderFile(virtualPath + "ExtJs/infolight/form/TextTest.js", ResourceFileType.Javascript);

                RenderFile(virtualPath + "ExtJs/infolight/refval/GexRefVal.source.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/refval/ReadWriteXMLJS.source.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/infolight/refval/RefVal.js", ResourceFileType.Javascript);
                //RenderFile(virtualPath + "ExtJs/plugins/ux-all.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "ExtJs/ux/ColumnModel.js", ResourceFileType.Javascript);

                switch (CliUtils.fClientLang)
                {
                    case SYS_LANGUAGE.SIM:
                        RenderFile(virtualPath + "ExtJs/src/locale/ext-lang-zh_CN.js", ResourceFileType.Javascript);
                        break;
                    case SYS_LANGUAGE.TRA:
                    case SYS_LANGUAGE.HKG:
                        RenderFile(virtualPath + "ExtJs/src/locale/ext-lang-zh_TW.js", ResourceFileType.Javascript);
                        break;
                    case SYS_LANGUAGE.JPN:
                        RenderFile(virtualPath + "ExtJs/src/locale/ext-lang-ja.js", ResourceFileType.Javascript);
                        break;
                }
            }
            if (RenderJQuery)
            {
                RenderFile(virtualPath + "JQuery/jquery-1.3.2.min.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "JQuery/ui.core.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "JQuery/ui.draggable.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "JQuery/ui.resizable.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "JQuery/extender/infoExtend.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "JQuery/FullCalendar/fullcalendar.js", ResourceFileType.Javascript);
                RenderFile(virtualPath + "JQuery/infolight/schedule/schedule.js", ResourceFileType.Javascript);
            }

            // render user added resource files
            foreach (ExtResourceFile file in this.ResourceFiles)
            {
                RenderFile(this.ResolveClientUrl(file.FileUrl), file.FileType);
            }
        }

        protected virtual void AddJsRef()
        {
            this.Scripts.Add(new ScriptReference("AjaxTools.AjaxDateTimePicker.js", "AjaxTools"));
            //AjaxRefVal?�js???仌WebRefVal?�js???仸?,?�此这??�?�引用
            //this.Scripts.Add(new ScriptReference("AjaxTools.AjaxRefVal.js", "AjaxTools"));
        }

        protected virtual void AddCssRef(string[] cssNames)
        {
            foreach (string cssName in cssNames)
            {
                RenderFile(string.Format("{0}css/controls/{1}.css", this.VirtualPath(), cssName), ResourceFileType.Css);
            }
        }

        void RenderFile(string filePath, ResourceFileType fileType)
        {
            switch (fileType)
            {
                case ResourceFileType.Css:
                    HtmlLink cssLink = new HtmlLink();
                    cssLink.Href = filePath;
                    cssLink.Attributes.Add("rel", "stylesheet");
                    cssLink.Attributes.Add("type", "text/css");
                    this.Page.Header.Controls.Add(cssLink);
                    break;
                case ResourceFileType.Javascript:
                    //this.Page.ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), filePath);
                    this.Scripts.Add(new ScriptReference(filePath));
                    break;
            }
        }

        string VirtualPath()
        {
            string subPath = "";
            for (int i = 0; i < this.Page.Request.FilePath.Split('/').Length - 3; i++)
            {
                subPath += "../";
            }
            return subPath;
        }

        bool ScriptRegistered(string script)
        {
            foreach (ScriptReference refer in this.Scripts)
            {
                if (refer.Name == script)
                {
                    return true;
                }
            }
            return false;
        }
    }
}