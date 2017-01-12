using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing.Design;
using EFClientTools.EFServerReference;
using System.ComponentModel.Design;
using System.Web.UI.Design;
using System.Windows.Forms.Design;
using System.Xml;
using System.IO;

namespace JQMobileTools
{
    [Designer(typeof(JQMetroEdit), typeof(IDesigner))]
    public class JQMetro : WebControl
    {
        [Category("Infolight")]
        public string Locale { get; set; }
        [Category("Infolight")]
        public string JQueryVersion { get; set; }
        [Category("Infolight")]
        public bool LocalScript { get; set; }
        public JQMetro()
        {
            Locale = JQLocale.Chinese_Taiwan;
            JQueryVersion = "1.8.0";
        }

        private string _DBAlias;
        [Category("Infolight"),
        Description("Specifies the database to get menu root.")]
        [Editor(typeof(JQGetAlias), typeof(System.Drawing.Design.UITypeEditor))]
        public string DBAlias
        {
            get
            {
                return _DBAlias;
            }
            set
            {
                _DBAlias = value;
            }
        }

        private string _RootValue;
        [Category("Infolight"),
        Description("Specifies the menuID which the Root is applied to"),
        Editor(typeof(JQGetMenuID), typeof(UITypeEditor))]
        public string RootValue
        {
            get
            {
                return _RootValue;
            }
            set
            {
                _RootValue = value;
            }
        }

        private Unit _Height = 550;
        [Category("Infolight"), DefaultValue(550),
        Description("Height")]
        public Unit Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
            }
        }

        //private string _OnFormat;
        //[Category("Infolight"),
        //Description("Specifies the menuID which the WebSecurity is applied to")]
        //public string OnFormat
        //{
        //    get
        //    {
        //        return _OnFormat;
        //    }
        //    set
        //    {
        //        _OnFormat = value;
        //    }
        //}

        private bool _SubFolder = true;
        [Category("Infolight"), DefaultValue(true)]
        public bool SubFolder
        {
            get
            {
                return _SubFolder;
            }
            set
            {
                _SubFolder = value;
            }
        }

        private bool _IsForCloud = false;
        [Category("Infolight"), DefaultValue(false)]
        public bool IsForCloud
        {
            get
            {
                return _IsForCloud;
            }
            set
            {
                _IsForCloud = value;
            }
        }

        private string _UserID;
        [Category("Infolight"),
        Description("")]
        public string UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (Visible)
            {
                if (!this.DesignMode)
                {
                    //int dLevel = 0;
                    //foreach (var item in this.Page.Request.CurrentExecutionFilePath)
                    //{
                    //    if (item == '/')
                    //    {
                    //        dLevel++;
                    //    }
                    //}

                    String subFile = String.Empty;
                    //if (dLevel > 2)
                    if (SubFolder)
                        subFile = "../";
                    var styleSheets = new string[] {
                                //"../assets/css/bootmetro.css",
                                subFile+ "assets/css/bootmetro-responsive.css",
                                //subFile+ "assets/css/bootmetro-icons.css",
                                subFile+ "assets/css/bootmetro-ui-light.css",
                                subFile+ "assets/css/datepicker.css",
                                subFile+ "MetroJs.Full.0.9.75a/MetroJs.css"/*,
                                subFile+ "js/themes/default/easyui.css",
                                subFile+ "js/themes/icon.css"*/
                            };
                    foreach (var sheet in styleSheets)
                    {
                        if (!IsCssRegistered(sheet))
                        {
                            AddCss(sheet);
                        }
                    }
                    var linkMobile = new HtmlLink() { Href = subFile + "assets/css/bootmetroMobile.css" };
                    linkMobile.Attributes["rel"] = "stylesheet";
                    linkMobile.Attributes["media"] = "screen and (max-width: 640px)";
                    this.Page.Header.Controls.Add(linkMobile);
                    var link = new HtmlLink() { Href = subFile + "assets/css/bootmetro.css" };
                    link.Attributes["rel"] = "stylesheet";
                    link.Attributes["media"] = "screen and (min-width: 640px)";
                    this.Page.Header.Controls.Add(link);

                    var jqueryScript = LocalScript ? string.Format(subFile + "js/jquery-{0}.min.js", JQueryVersion)
                                        : string.Format("http://code.jquery.com/jquery-{0}.min.js", JQueryVersion);
                    var clientScripts = new string[] {
                    jqueryScript,
                    subFile+ "assets/js/min/bootstrap.min.js",
                    subFile+ "assets/js/bootmetro-panorama.js",
                    subFile+ "assets/js/bootmetro-pivot.js",
                    subFile+ "assets/js/bootmetro-charms.js",
                    subFile+ "assets/js/bootstrap-datepicker.js",
                    subFile+ "assets/js/jquery.mousewheel.js",
                    subFile+ "assets/js/jquery.touchSwipe.js",
                    subFile+ "MetroJs.Full.0.9.75a/MetroJs.js",
                    subFile+ "MainPage.js"/*,
                    subFile+ "js/jquery.easyui.min.js",
                    subFile+ "js/jquery.json.js",
                    subFile+ "js/jquery.infolight.js"*/
                    };
                    AddClientScripts(clientScripts);
                }
            }
        }

        public void AddClientScripts(string[] urls)
        {
            var literal = new LiteralControl();
            var scripts = new StringBuilder();
            foreach (var url in urls)
            {
                scripts.Append(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", url));
            }
            literal.Text = scripts.ToString();
            this.Page.Header.Controls.AddAt(0, literal);
        }

        public void AddClientScript(string url)
        {
            var literal = new LiteralControl();
            literal.Text = string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", url);
            this.Page.Header.Controls.Add(literal);
        }

        public bool IsClientScriptRegistered(string url)
        {
            return this.Page.Header.Controls.OfType<LiteralControl>().FirstOrDefault(c => c.Text.Contains(url)) != null;

        }

        public void AddCss(string url)
        {
            var link = new HtmlLink() { Href = url };
            link.Attributes["rel"] = "stylesheet";
            this.Page.Header.Controls.Add(link);
        }

        public bool IsCssRegistered(string url)
        {
            return this.Page.Header.Controls.OfType<HtmlLink>().FirstOrDefault(c => c.Href.Equals(url)) != null;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (Visible)
            {
                if (!this.DesignMode)
                {
                    var client = EFClientTools.ClientUtility.Client;
                    string userID = EFClientTools.ClientUtility.ClientInfo.UserID;
                    EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                    List<MENUTABLE> menus = null;
                    try
                    {
                        if (UserID != "")
                            EFClientTools.ClientUtility.ClientInfo.UserID = UserID;
                        var allMenus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => String.IsNullOrEmpty(m.PARENT) && m.MODULETYPE.ToLower() == "m");
                        if (!String.IsNullOrEmpty(RootValue))
                        {
                            menus = allMenus.Where(m => m.MENUID == RootValue).FirstOrDefault().MENUTABLE1;
                        }
                        else
                        {
                            menus = allMenus.ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        EFClientTools.ClientUtility.ClientInfo.UserID = userID;
                    }
                    base.Render(writer);
                    //writer.AddAttribute(HtmlTextWriterAttribute.Class, "easyui-panel");
                    ////writer.AddAttribute(HtmlTextWriterAttribute.Style, "overflow-x:scroll;");
                    //writer.AddAttribute(HtmlTextWriterAttribute.Title, "Panel");
                    //writer.AddAttribute("fit", "true");
                    //writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:2000px");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "wrap");
                    //if (this.Height > 0)
                    //    writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:" + this.Height + "px;width:2000px");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "metro panorama");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "panorama-sections");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    foreach (var menuTable in menus)
                    {
                        if (menuTable.MENUTABLE1.Count == 0 && String.IsNullOrEmpty(menuTable.PARENT))
                        {
                            continue;
                        }

                        //Root i
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "panorama-section");
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        //Title
                        writer.RenderBeginTag(HtmlTextWriterTag.H2);
                        writer.Write(getCaption(menuTable));
                        writer.RenderEndTag();//H2

                        String columnCount = String.Empty;
                        if (!String.IsNullOrEmpty(menuTable.ITEMPARAM))
                        {
                            foreach (var item in menuTable.ITEMPARAM.Split(new char[] { '?', '&' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (item.ToLower().StartsWith("columncount"))
                                {
                                    columnCount = item.Split('=')[1];
                                }
                            }
                        }
                        if (String.IsNullOrEmpty(columnCount))
                        {
                            if (menuTable.MENUTABLE1.Count > 3)
                            {
                                columnCount = "4";
                            }
                            else
                            {
                                //if (menuTable.MENUTABLE1.Count % 3 == 0)
                                //    columnCount = (menuTable.MENUTABLE1.Count / 3).ToString();
                                //else
                                //    columnCount = (menuTable.MENUTABLE1.Count / 3 + 1).ToString();
                                columnCount = menuTable.MENUTABLE1.Count.ToString();
                            }
                        }
                        resetColumnCount(menuTable, ref columnCount);
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, String.Format("tile-column-span-{0}", columnCount));
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        int iColor = 0;
                        foreach (MENUTABLE menu in menuTable.MENUTABLE1)
                        {
                            SetMetro(menu, writer,ref iColor);
                        }
                        if (menuTable.MENUTABLE1.Count == 0)
                        {
                            SetMetro(menuTable, writer, ref iColor);
                        }
                        writer.RenderEndTag();//Div tile-column-span-3

                        writer.RenderEndTag();//Div panorama-section
                    }

                    writer.RenderEndTag();//Div panorama-sections
                    writer.RenderEndTag();//Div metro panorama
                    writer.RenderEndTag();//Div wrap

                    String script = @"
                                <script type='text/javascript'>

                                    $('.panorama').panorama({
                                        //nicescroll: false,
                                        showscrollbuttons: true,
                                        keyboard: true,
                                        parallax: true
                                    });

                                    //$('.panorama').perfectScrollbar();

                                    $('#pivot').pivot();

                                </script>";
                    writer.WriteLine(script);
                    if (SubFolder && IsForCloud)
                    {
                        String script2 = @"
                                <script type='text/javascript'>
                                    function openForm(rowData) {
    var type = 'mobile' ;
    $.ajax({
        type: 'post',
        dataType: 'text',
        url: '../handler/SystemHandler.ashx?&type=Menu',
        data: { mode: 'Run', id: rowData.FORM, type: type, itemParam: rowData.ITEMPARAM },
        async: true,
        cache: false,
        success: function (data) {          
            window.location.href = '../'+data;
        }
    });
}
                                </script>";
                        writer.WriteLine(script2);
                    }
                    //writer.RenderEndTag();//Div panel

                    //this.Page.Controls
                }
            }
        }

        private void SetMetro(MENUTABLE menuTable, HtmlTextWriter writer, ref int iColor)
        {
            String bgColor = menuTable.VERSIONNO;
            if (String.IsNullOrEmpty(bgColor))
            {
                String[] bgColors = { "orange", "purple", "greenDark", "blue", "red", "green", "blueDark", "yellow", "pink", "darken", "gray", "grayLight" };
                //bgColor = bgColors[Guid.NewGuid().ToByteArray()[0] % 12];
                if (iColor >= bgColors.Length)
                    iColor = 0;
                bgColor = bgColors[iColor++];
            }
            String menuClass = String.Format("tile {0} image bg-color-{1}", getSize(menuTable), bgColor);
            menuClass = SetFMenuClassForMobileGrid(menuClass, menuTable);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, menuClass);
            String formName = String.Empty;
            if (!String.IsNullOrEmpty(menuTable.FORM))
                formName = "/" + menuTable.FORM + ".aspx";
            if (!String.IsNullOrEmpty(formName))
            {
                //href='" + url + "' data-transition='slide' rel='external'
                if (!IsForCloud)
                {
                    if (SubFolder)
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "../" + menuTable.PACKAGE + formName + "?" + menuTable.ITEMPARAM);
                    else
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, menuTable.PACKAGE + formName + "?" + menuTable.ITEMPARAM);
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0);");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("openForm({{MODULETYPE:'{0}', FORM:'{1}', MENUID:'{2}', CAPTION:'{3}'}})", menuTable.MODULETYPE, menuTable.FORM, menuTable.MENUID, menuTable.CAPTION));
                }
                writer.AddAttribute("data-transition", "slide");
                writer.AddAttribute(HtmlTextWriterAttribute.Rel, "external");
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Format("Menu{0}", menuTable.MENUID));
            writer.RenderBeginTag(HtmlTextWriterTag.A);

            if (menuTable.ISSERVER != null && menuTable.ISSERVER.ToString() != "N" && menuTable.ISSERVER != "I" && menuTable.ISSERVER != "T")
            {
                #region metro作为控件使用在页面呈现其他类型时候
                if (menuTable.OWNER != null && menuTable.OWNER != "")
                {
                    var control = this.Page.FindControl(menuTable.OWNER);
                    if (control != null)
                    {
                        if (menuTable.ISSERVER == "G")
                        {
                            control.Visible = true;
                            //control.RenderControl(writer);
                            var grid = control as JQDataGrid;
                            var gridClass = JQClass.DataGrid;
                            var gridTheme = grid.Theme;
                            if (grid.GridViewType == JQMobileTools.GridViewType.List)
                            {
                                gridClass += " " + JQClass.DataList;
                            }
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, gridClass + " table-stripe infolight-breakpoint");
                            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Table);
                            writer.AddAttribute(JQProperty.DataMode, JQDataMode.Reflow);
                            if (!string.IsNullOrEmpty(gridTheme))
                            {
                                writer.AddAttribute(JQProperty.DataTheme, gridTheme);
                            }
                            writer.AddAttribute(JQProperty.DataOptions, grid.DataOptions);

                            writer.RenderBeginTag(HtmlTextWriterTag.Table);
                            writer.RenderBeginTag(HtmlTextWriterTag.Thead);
                            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                            foreach (var column in grid.Columns)
                            {
                                if (column.Visible)
                                    column.Render(writer);
                            }

                            writer.RenderEndTag();
                            writer.RenderEndTag();
                            writer.RenderEndTag();
                            JQScriptManager.RenderPopup(writer, string.Format("{0}_popup", ID));

                            if (grid.ToolItems.Count > 0)
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Id, grid.ToolItemObjectID);
                                writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none");
                                writer.AddAttribute(JQProperty.DataRole, JQDataRole.None);
                                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                                foreach (var item in grid.ToolItems)
                                {
                                    item.Render(writer);
                                }
                                writer.RenderEndTag();
                            }
                            control.Visible = false;
                        }
                        else if (menuTable.ISSERVER == "C" || menuTable.ISSERVER == "D")
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID + "_" + control.ID);
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);
                            writer.RenderEndTag();//div
                            System.Reflection.PropertyInfo p = control.GetType().GetProperty("RenderObjectID");
                            if (p != null)
                            {
                                p.SetValue(control, this.ID + "_" + control.ID, null);
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region metro 呈现MenuTable资料
                if (menuTable.ISSERVER == "I" || menuTable.ISSERVER == "T")
                {
                    var control2 = this.Page.FindControl(menuTable.OWNER);
                    if (control2 != null && control2.GetType() == typeof(JQRotator))
                    {
                        (control2 as JQRotator).MenuID = menuTable.MENUID;
                    }
                }
                if (!String.IsNullOrEmpty(menuTable.IMAGEURL))
                {
                    var initialDir = string.Format("Image/MenuTree/Metro_{0}", menuTable.IMAGEURL);
                    if (SubFolder)
                        initialDir = "../" + initialDir;
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, initialDir);
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag();//Img
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "textover-wrapper transparent");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Format("Menu{0}DIVtext", menuTable.MENUID));
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "text2");
                var title = getCaption(menuTable);
                writer.AddAttribute(HtmlTextWriterAttribute.Title, title);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "color: white");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write(title);
                writer.RenderEndTag();//Div text2
                writer.RenderEndTag();//Div textover-wrapper bg-color-blueDark
                #endregion
            }
            writer.RenderEndTag();//A tile wide imagetext wideimage bg-color-blue

            foreach (MENUTABLE v in menuTable.MENUTABLE1)
            {
                SetMetro(v, writer, ref iColor);
            }
        }

        private string SetFMenuClassForMobileGrid(string menuClass, MENUTABLE menuTable)
        {
            if (menuTable.ISSERVER != null && menuTable.ISSERVER.ToString() == "G")
            {
                if (menuTable.OWNER != null && menuTable.OWNER != "")
                {
                    var control = this.Page.FindControl(menuTable.OWNER);
                    if (control != null)
                    {
                        control.Visible = true;
                        //control.RenderControl(writer);
                        var grid = control as JQDataGrid;
                        var gridTheme = grid.Theme;
                        return menuClass + " ui-content ui-body-" + gridTheme;
                    }
                }
            }
            return menuClass;
        }

        //当只有1个并且选了大尺寸的情况下会出现外框只有1倍宽内容却是2倍宽的重叠现象
        private void resetColumnCount(MENUTABLE menuTable, ref string columnCount)
        {
            foreach (var menu in menuTable.MENUTABLE1)
            {
                if (menu.ISSHOWMODAL == "m" || menu.ISSHOWMODAL == "h")
                {
                    if (columnCount == "1") columnCount = "2";
                }
            }
        }

        private String getCaption(MENUTABLE menuTable)
        {
            String caption = "";
            if (string.Compare(EFClientTools.ClientUtility.ClientInfo.Locale, "zh-cn", true) == 0 || string.Compare(EFClientTools.ClientUtility.ClientInfo.Locale, "zh-hans-cn", true) == 0)
                caption = menuTable.CAPTION2;
            else if (string.Compare(EFClientTools.ClientUtility.ClientInfo.Locale, "zh-tw", true) == 0 || string.Compare(EFClientTools.ClientUtility.ClientInfo.Locale, "zh-hant-tw", true) == 0)
                caption = menuTable.CAPTION1;
            else if (string.Compare(EFClientTools.ClientUtility.ClientInfo.Locale, "zh-hk", true) == 0)
                caption = menuTable.CAPTION3;
            if (String.IsNullOrEmpty(caption))
                caption = menuTable.CAPTION;
            return caption;
        }

        private String getSize(MENUTABLE menuTable)
        {
            //s:square m:wide l:squarepeek h:widepeek a:app
            String size = "";
            switch (menuTable.ISSHOWMODAL)
            {
                case "m": size = "wide"; break;
                case "l": size = "squarepeek"; break;
                case "h": size = "widepeek"; break;
                case "a": size = "app"; break;
                case "s":
                default:
                    size = "square"; break;
            }

            return size;
        }
    }

    class JQMetroEdit : System.Web.UI.Design.ControlDesigner
    {
        private JQMetroEditForm EditMenus = null;
        private DesignerActionListCollection _actionLists;

        public JQMetroEdit()
        {
            DesignerVerb EditMenusVerb = new DesignerVerb("Edit Menus", new EventHandler(OnExport));
            this.Verbs.Add(EditMenusVerb);
        }

        public void OnExport(object sender, EventArgs e)
        {
            JQMetro jqm = (JQMetro)this.Component;
            if (jqm.DBAlias == null || jqm.DBAlias == "" || jqm.DBAlias == "(none)")
            {
                System.Windows.Forms.MessageBox.Show("Please set DBAlias and MenuID first.");
            }
            else if (EditMenus == null)
            {
                EditMenus = new JQMetroEditForm(jqm, this.GetService(typeof(IDesignerHost)) as IDesignerHost);
                EditMenus.ShowDialog();
            }
            EditMenus = null;
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                _actionLists = base.ActionLists;

                if (_actionLists != null)
                    _actionLists.Add(new JQMetroActionList(this.Component));

                return _actionLists;
            }
        }
    }

    public class JQMetroActionList : DesignerActionList
    {
        private JQMetro jqm;
        private JQMetroEditForm EditMenus = null;

        public JQMetroActionList(IComponent component)
            : base(component)
        {
            jqm = component as JQMetro;
        }

        public void OnExport()
        {
            if (jqm.DBAlias == null || jqm.DBAlias == "" || jqm.DBAlias == "(none)")
            {
                System.Windows.Forms.MessageBox.Show("Please set DBAlias and MenuID first.");
            }
            else if (EditMenus == null)
            {
                EditMenus = new JQMetroEditForm(jqm, this.GetService(typeof(IDesignerHost)) as IDesignerHost);
                EditMenus.ShowDialog();
            }
            EditMenus = null;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "OnExport", "Export", "UsePreview", true));
            return items;
        }
    }

    public class JQGetAlias : System.Drawing.Design.UITypeEditor
    {
        public JQGetAlias()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService EditorService = null;
            if (provider != null)
            {
                EditorService =
                    provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            if (EditorService != null)
            {
                System.Windows.Forms.ListBox AliasList = new System.Windows.Forms.ListBox();
                AliasList.Items.Add("(none)");
                AliasList.SelectionMode = System.Windows.Forms.SelectionMode.One;
                String serverPath = EFClientTools.DesignClientUtility.GetServerPath();
                String dbFile = serverPath + "\\DB.xml";
                XmlDocument DBXML = new XmlDocument();
                if (File.Exists(dbFile))
                {
                    DBXML.Load(dbFile);
                    XmlNode aNode = DBXML.DocumentElement.FirstChild;

                    while (aNode != null)
                    {
                        if (string.Compare(aNode.Name, "DATABASE", true) == 0)//IgnoreCase
                        {
                            XmlNode bNode = aNode.FirstChild;
                            while (bNode != null)
                            {
                                AliasList.Items.Add(bNode.LocalName);
                                bNode = bNode.NextSibling;
                            }
                        }
                        aNode = aNode.NextSibling;
                    }
                }
                AliasList.SelectedIndexChanged += delegate(object sender, EventArgs e)
                {
                    int index = AliasList.SelectedIndex;
                    if (index != -1)
                    {
                        value = AliasList.Items[index].ToString();
                    }
                    EditorService.CloseDropDown();
                };
                EditorService.DropDownControl(AliasList);
            }
            return value;
        }
    }

    public class JQGetMenuID : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public JQGetMenuID()
        {

        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        //    // Displays the UI for value selection.
        //    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService EditorService = null;
            if (provider != null)
            {
                EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            if (EditorService != null)
            {
                System.Windows.Forms.ListBox MenuIDs = new System.Windows.Forms.ListBox();
                System.Windows.Forms.ListBox MenuID = new System.Windows.Forms.ListBox();
                MenuIDs.SelectionMode = System.Windows.Forms.SelectionMode.One;
                MenuIDs.Items.Add("( None )");
                if (context.Instance is JQMetro)
                {
                    JQMetro jqm = context.Instance as JQMetro;
                    if (jqm != null)
                    {
                        if (String.IsNullOrEmpty(jqm.DBAlias))
                        {
                            System.Windows.Forms.MessageBox.Show("Please set DBAlias first.");
                            return null;
                        }
                        EFClientTools.DesignClientUtility.ClientInfo.Database = jqm.DBAlias;
                        EFClientTools.DesignClientUtility.ClientInfo.UseDataSet = true;
                        var menuIds = EFClientTools.DesignClientUtility.GetAllDataByTableName("MENUTABLE").Cast<MENUTABLE>();
                        EFClientTools.DesignClientUtility.ClientInfo.Database = String.Empty;
                        if (menuIds != null && menuIds.Count() > 0)
                        {
                            foreach (MENUTABLE item in menuIds)
                            {
                                MenuIDs.Items.Add(item.MENUID + "(" + item.CAPTION + ")");
                                MenuID.Items.Add(item.MENUID);
                            }
                        }
                    }
                }
                else if (context.Instance is JQSecurity)
                {
                    JQSecurity jqs = context.Instance as JQSecurity;
                    if (jqs != null)
                    {
                        if (String.IsNullOrEmpty(jqs.DBAlias))
                        {
                            System.Windows.Forms.MessageBox.Show("Please set DBAlias first.");
                            return null;
                        }
                        EFClientTools.DesignClientUtility.ClientInfo.Database = jqs.DBAlias;
                        EFClientTools.DesignClientUtility.ClientInfo.UseDataSet = true;
                        var menuIds = EFClientTools.DesignClientUtility.GetAllDataByTableName("MENUTABLE").Cast<MENUTABLE>();
                        EFClientTools.DesignClientUtility.ClientInfo.Database = String.Empty;
                        if (menuIds != null && menuIds.Count() > 0)
                        {
                            foreach (MENUTABLE item in menuIds)
                            {
                                MenuIDs.Items.Add(item.MENUID + "(" + item.CAPTION + ")");
                                MenuID.Items.Add(item.MENUID);
                            }
                        }
                    }
                }

                MenuIDs.SelectedIndexChanged += delegate(object sender, EventArgs e)
                {
                    int index = MenuIDs.SelectedIndex;
                    if (index != -1)
                    {
                        if (index == 0)
                        {
                            value = "";
                        }
                        else
                        {
                            value = MenuID.Items[index - 1].ToString();
                        }
                    }
                    EditorService.CloseDropDown();
                };

                EditorService.DropDownControl(MenuIDs);
            }

            return value;
        }
    }

}