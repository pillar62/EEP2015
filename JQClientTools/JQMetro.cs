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

namespace JQClientTools
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
            JQueryVersion = "1.10.0";
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
        public override Unit Height
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
                    subFile+ "assets/js/bootmetro.js",
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
                        var allMenus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => String.IsNullOrEmpty(m.PARENT));
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
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "wrap");
                    if (this.Height.Value > 0)
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:" + this.Height.Value + "px;margin:0 20px");
                    else
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, "margin:0 20px");
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
                            resetColumnCount(menuTable, ref columnCount);
                        }
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, String.Format("tile-column-span-{0}", columnCount));
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        int iColor = 0;
                        foreach (MENUTABLE menu in menuTable.MENUTABLE1)
                        {
                            SetMetro(menu, writer, ref iColor);
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
                    //writer.RenderEndTag();//Div panel

                    //this.Page.Controls
                }
            }
        }

        //当只有1个并且选了大尺寸的情况下会出现外框只有1倍宽内容却是2倍宽的重叠现象
        private void resetColumnCount(MENUTABLE menuTable, ref string columnCount)
        {
            int count = 0;
            if (Int32.TryParse(columnCount, out count))
            {
                if (count > 3) return;
                int p = 0;
                foreach (var menu in menuTable.MENUTABLE1)
                {
                    if (menu.ISSHOWMODAL == "m" || menu.ISSHOWMODAL == "h")
                    {
                        p++;
                    }
                }
                if (columnCount == "1" && p == 1) columnCount = "2";
                else if (columnCount == "2" && p == 1) columnCount = "3";
                else if (columnCount == "2" && p == 2) columnCount = "4";
                else if (columnCount == "3" && p >= 1) columnCount = "4";
            }
        }

        private void SetMetro(MENUTABLE menu, HtmlTextWriter writer, ref int iColor)
        {
            String bgColor = menu.VERSIONNO;
            if (String.IsNullOrEmpty(bgColor))
            {
                String[] bgColors = { "orange", "purple", "greenDark", "blue", "red", "green", "blueDark", "yellow", "pink", "darken", "gray", "grayLight" };
                //bgColor = bgColors[Guid.NewGuid().ToByteArray()[0] % 12];
                if (iColor >= bgColors.Length)
                    iColor = 0;
                bgColor = bgColors[iColor++];
            }
            String menuClass = String.Format("tile {0} image bg-color-{1}", getSize(menu), bgColor);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, menuClass);
            String formName = String.Empty;
            if (!String.IsNullOrEmpty(menu.FORM))
                formName = "/" + menu.FORM + ".aspx";
            if (!String.IsNullOrEmpty(formName))
            {
                //writer.AddAttribute(HtmlTextWriterAttribute.Href, System.IO.Path.Combine(menu.PACKAGE, formName));
                if (!IsForCloud)
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("addTab('{0}', '{1}?{2}')", menu.CAPTION, menu.PACKAGE + formName, menu.ITEMPARAM));
                else
                {
                    if(!SubFolder)
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("openForm({{MODULETYPE:'{0}', FORM:'{1}', MENUID:'{2}', CAPTION:'{3}'}})", menu.MODULETYPE, menu.FORM, menu.MENUID, menu.CAPTION));
                    else
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("self.parent.openForm({{MODULETYPE:'{0}', FORM:'{1}', MENUID:'{2}', CAPTION:'{3}'}})", menu.MODULETYPE, menu.FORM, menu.MENUID, menu.CAPTION));
                }
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Format("Menu{0}", menu.MENUID));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            if (menu.ISSERVER != null && menu.ISSERVER.ToString() != "N" && menu.ISSERVER != "I" && menu.ISSERVER != "T")
            {
                #region metro作为控件使用在页面呈现其他类型时候
                if (menu.OWNER != null && menu.OWNER != "")
                {
                    var control = this.Page.FindControl(menu.OWNER);
                    if (control != null)
                    {
                        if (menu.ISSERVER == "G" || menu.ISSERVER == "F")
                        {
                            control.Visible = true;
                            control.RenderControl(writer);
                            control.Visible = false;
                        }
                        else if (menu.ISSERVER == "C" || menu.ISSERVER == "D")
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
                if (menu.ISSERVER == "I" || menu.ISSERVER == "T")
                {
                    var control2 = this.Page.FindControl(menu.OWNER);
                    if (control2 != null && control2.GetType() == typeof(JQRotator))
                    {
                        (control2 as JQRotator).MenuID = menu.MENUID;
                    }
                }

                if (!String.IsNullOrEmpty(menu.IMAGEURL))
                {
                    var initialDir = string.Format("Image/MenuTree/Metro_{0}", menu.IMAGEURL);
                    if (SubFolder)
                        initialDir = "../" + initialDir;
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, initialDir);
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Format("Menu{0}IMG", menu.MENUID));
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag();//Img
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "textover-wrapper transparent");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Format("Menu{0}DIV", menu.MENUID));
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Format("Menu{0}DIVtext", menu.MENUID));
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "text2");
                var title = getCaption(menu);
                writer.AddAttribute(HtmlTextWriterAttribute.Title, title);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write(title);
                writer.RenderEndTag();//Div text2
                writer.RenderEndTag();//Div textover-wrapper bg-color-blueDark
                #endregion
            }
            writer.RenderEndTag();//A tile wide imagetext wideimage bg-color-blue
            foreach (MENUTABLE v in menu.MENUTABLE1)
            {
                SetMetro(v, writer, ref iColor);
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

        private string getStyle(MENUTABLE menuTable)
        {
            String Style = "";
            switch (menuTable.ISSERVER)
            {
                case "G": Style = "DataGrid"; break;
                case "F": Style = "DataForm"; break;
                case "C": Style = "Chart"; break;
                case "D": Style = "Dashboard"; break;
                case "I": Style = "Image"; break;
                case "T": Style = "Text"; break;
                case "N":
                default:
                    Style = "None"; break;
            }

            return Style;

        }
    }

    class JQMetroEdit : ControlDesigner
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
            items.Add(new DesignerActionMethodItem(this, "OnExport", "Edit", "UsePreview", true));
            return items;
        }
    }
}
