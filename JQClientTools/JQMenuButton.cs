using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;

namespace JQClientTools
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [Designer(typeof(ControlDesigner), typeof(IDesigner))]
    public class JQMenuButton : Control
    {
        private string _menuId;
        private bool _titleMode = false;
        private bool _ignoreMenuRight = true;
        private List<EFClientTools.EFServerReference.MENUTABLE> menus;

        #region Properties
        [Category("Infolight")]
        public string MenuId
        {
            get { return _menuId; }
            set { _menuId = value; }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        public bool TitleMode
        {
            get { return _titleMode; }
            set { _titleMode = value; }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool IgnoreMenuRight
        {
            get { return _ignoreMenuRight; }
            set { _ignoreMenuRight = value; }
        }
        #endregion

        public JQMenuButton()
        {

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8230;
                base.Render(writer);

                if (TitleMode)
                {
                    foreach (EFClientTools.EFServerReference.MENUTABLE menu in menus)
                    {
                        var menuId = this.ClientID + menu.MENUID;
                        writer.WriteLine("<a href=\"#\" class=\"easyui-menubutton\" data-options=\"menu:'#" + menuId + "'\">" + menu.CAPTION + "</a>");
                    }
                }
                else
                {
                    writer.WriteLine("<a id=\"btn-" + this.ClientID + "\" href=\"#\" class=\"easyui-menubutton\" data-options=\"menu:'#" + this.ClientID + "'\">Menus</a>");
                }
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.RenderJQMenuButton();
            }
        }

        public void RenderJQMenuButton()
        {
            if (!String.IsNullOrEmpty(this.ClientID))
            {
                menus = new List<EFClientTools.EFServerReference.MENUTABLE>();
                var client = EFClientTools.ClientUtility.Client;
                EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                var userPara2 = EFClientTools.ClientUtility.ClientInfo.UserPara2;
                if (!IgnoreMenuRight)
                    EFClientTools.ClientUtility.ClientInfo.UserPara2 = "forRootMenuButton";
                if (!String.IsNullOrEmpty(MenuId))
                    //menus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().ToList();
                    menus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => m.PARENT == MenuId).ToList();

                if (menus.Count == 0)
                    menus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => String.IsNullOrEmpty(m.PARENT)).ToList();
                EFClientTools.ClientUtility.ClientInfo.UserPara2 = userPara2;
                //var client = EFClientTools.ClientUtility.Client;
                //EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                //var menus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => String.IsNullOrEmpty(m.PARENT)).ToList();
                StringBuilder renderScript = new StringBuilder();
                try
                {
                    CreateIconCSS(menus);
                }
                catch
                {
                    renderScript.Append("$(document).attr('menunoicon', 'true')\r\n");
                }

                String menuString = "";

                if (TitleMode)
                {
                    foreach (EFClientTools.EFServerReference.MENUTABLE menu in menus)
                    {
                        var menuId = this.ClientID + menu.MENUID;
                        //render menus
                        renderScript.Append("var menuItem = document.createElement(\"div\");\r\n");
                        renderScript.AppendFormat("menuItem.id = \"{0}\";\r\n", menuId);
                        List<EFClientTools.EFServerReference.MENUTABLE> chileMenus = new List<EFClientTools.EFServerReference.MENUTABLE>();
                        menuString = InitMenuItems(chileMenus, menu.MENUTABLE1);
                        renderScript.AppendFormat("menuItem.innerHTML = \"{0}\";\r\n", menuString);
                        //renderScript.AppendFormat("menuItem.innerHTML = \"{0}\";", "<div><span>JQueryRoot</span><div style="width: 150px;"><div>MasterDetailTest2</div><div>SingleTest</div></div></div><div><span>WebROOT</span><div style="width: 150px;"><div><span>Dev</span><div style="width: 150px;"><div>DESingle1</div><div>DEMasterDetail1</div><div>DEQuery1</div></div></div><div><span>ASP</span><div style="width: 150px;"><div>WSingle</div><div>WSingle0</div><div>WSingle1</div><div>WSingle2</div><div>WSingle3</div><div>WSingle4</div><div>WSingle5</div><div>WMasterDetail1</div><div>WMasterDetail2</div><div>WMasterDetail3</div><div>WMasterDetail4</div><div>WMasterDetail6</div><div>WMasterDetail7</div><div>WMasterDetail8</div><div>WQuery</div><div>WMasterDetail9</div><div>WMasterDetail10</div><div>WQuery1</div></div></div><div><span>Ext</span><div style="width: 150px;"><div>ExtSingle</div></div></div></div></div><div><span>ROOT</span><div style="width: 150px;"><div>P4</div><div>p5</div><div>P1</div><div>P2</div></div></div>");
                        renderScript.Append("document.body.appendChild(menuItem);\r\n");
                    }

                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), renderScript.ToString(), true);
                }
                else
                {
                    //render menus
                    renderScript.Append("var menuItem = document.createElement(\"div\");\r\n");
                    renderScript.AppendFormat("menuItem.id = \"{0}\";\r\n", this.ClientID);
                    List<EFClientTools.EFServerReference.MENUTABLE> chileMenus = new List<EFClientTools.EFServerReference.MENUTABLE>();
                    menuString = InitMenuItems(chileMenus, menus);
                    renderScript.AppendFormat("menuItem.innerHTML = \"{0}\";\r\n", menuString);
                    //renderScript.AppendFormat("menuItem.innerHTML = \"{0}\";", "<div><span>JQueryRoot</span><div style="width: 150px;"><div>MasterDetailTest2</div><div>SingleTest</div></div></div><div><span>WebROOT</span><div style="width: 150px;"><div><span>Dev</span><div style="width: 150px;"><div>DESingle1</div><div>DEMasterDetail1</div><div>DEQuery1</div></div></div><div><span>ASP</span><div style="width: 150px;"><div>WSingle</div><div>WSingle0</div><div>WSingle1</div><div>WSingle2</div><div>WSingle3</div><div>WSingle4</div><div>WSingle5</div><div>WMasterDetail1</div><div>WMasterDetail2</div><div>WMasterDetail3</div><div>WMasterDetail4</div><div>WMasterDetail6</div><div>WMasterDetail7</div><div>WMasterDetail8</div><div>WQuery</div><div>WMasterDetail9</div><div>WMasterDetail10</div><div>WQuery1</div></div></div><div><span>Ext</span><div style="width: 150px;"><div>ExtSingle</div></div></div></div></div><div><span>ROOT</span><div style="width: 150px;"><div>P4</div><div>p5</div><div>P1</div><div>P2</div></div></div>");
                    renderScript.Append("document.body.appendChild(menuItem);\r\n");
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), renderScript.ToString(), true);
                }
                //menu Click
                //renderScript.Append("$('#" + this.ClientID + "').menu({\r\n");
                //renderScript.Append("onClick: function (item) {\r\n");
                //foreach (var item in chileMenus)
                //{
                //    renderScript.Append("if (item.id == '" + item.MENUID + "') {\r\n");
                //    if (item.MODULETYPE == "W")
                //    {
                //        renderScript.Append("addTab('" + item.CAPTION + "','InnerPages/EEPSingleSignOn.aspx?Package=" + item.PACKAGE + "&Form=" + item.FORM + "&ItemParam=" + item.ITEMPARAM + "');}\r\n");
                //    }
                //    else if (item.MODULETYPE == "O")
                //    {
                //        renderScript.Append("addTab('" + item.CAPTION + "','InnerPages/FlowDesigner.aspx?FlowFileName=" + item.FORM + "');}\r\n");
                //    }
                //    else
                //    {
                //        renderScript.Append("addTab('" + item.CAPTION + "', '" + item.PACKAGE + "/" + item.FORM + ".aspx?ItemParam=" + item.ITEMPARAM + "');}\r\n");
                //    }
                //}
                //renderScript.Append("}});\r\n");

            }
        }

        private String InitMenuItems(List<EFClientTools.EFServerReference.MENUTABLE> menus, List<EFClientTools.EFServerReference.MENUTABLE> menuTables)
        {
            StringBuilder returnValue = new StringBuilder();// new StringBuilder(menus);
            foreach (var menuTable in menuTables)
            {
                String caption = menuTable.CAPTION;
                if (string.Compare(EFClientTools.ClientUtility.ClientInfo.Locale, "zh-cn", true) == 0 || string.Compare(EFClientTools.ClientUtility.ClientInfo.Locale, "zh-hans-cn", true) == 0)
                {
                    caption = menuTable.CAPTION2;
                }
                else if (string.Compare(EFClientTools.ClientUtility.ClientInfo.Locale, "zh-tw", true) == 0 || string.Compare(EFClientTools.ClientUtility.ClientInfo.Locale, "zh-hant-tw", true) == 0)
                {
                    caption = menuTable.CAPTION1;
                }
                else if (string.Compare(EFClientTools.ClientUtility.ClientInfo.Locale, "zh-hk", true) == 0)
                {
                    caption = menuTable.CAPTION3;
                }
                if (String.IsNullOrEmpty(caption))
                    caption = menuTable.CAPTION;

                if ((menuTable.MENUTABLE1 == null || menuTable.MENUTABLE1.Count == 0))
                {
                    if (!String.IsNullOrEmpty(menuTable.PACKAGE))
                    {
                        menus.Add(menuTable);
                        var icon = string.IsNullOrEmpty(menuTable.IMAGEURL) ? string.Empty : string.Format("data-options=\\\"iconCls:'menuicon-{0}'\\\"", menuTable.IMAGEURL.Replace(".", ""));
                        // var icon = string.IsNullOrEmpty(menuTable.IMAGEURL) ? string.Empty : string.Format("data-options=\\\"iconCls:'menuicon-368304png'\\\"", menuTable.IMAGEURL.Replace(".", ""));

                        returnValue.Append("<div id='" + menuTable.MENUID + "' " + icon + ">" + caption + "</div>");
                    }
                }
                else
                {
                    var icon = string.IsNullOrEmpty(menuTable.IMAGEURL) ? string.Empty : string.Format("data-options=\\\"iconCls:'menuicon-{0}'\\\"", menuTable.IMAGEURL.Replace(".", ""));

                    returnValue.Append("<div " + icon + ">");
                    returnValue.AppendFormat("<span>{0}</span>", caption);
                    returnValue.Append("<div>");
                    returnValue.Append(InitMenuItems(menus, menuTable.MENUTABLE1));
                    returnValue.Append("</div>");
                    returnValue.Append("</div>");
                }
            }

            return returnValue.ToString();
        }

        private void CreateIconCSS(List<EFClientTools.EFServerReference.MENUTABLE> menus)
        {
            var url = string.Format("Image/MenuTree/css/{0}/menu.css", EFClientTools.ClientUtility.ClientInfo.UserID);

            var filePath = this.Page.MapPath(url);

            var dir = System.IO.Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath, false, new UTF8Encoding()))
            {
                WriteCss(writer, menus);
                writer.Flush();
                writer.Close();
            }

            var link = new HtmlLink() { Href = url };
            link.Attributes["rel"] = "stylesheet";
            this.Page.Header.Controls.Add(link);

        }

        private void WriteCss(System.IO.StreamWriter writer, List<EFClientTools.EFServerReference.MENUTABLE> menus)
        {
            foreach (var menu in menus)
            {
                if (!string.IsNullOrEmpty(menu.IMAGEURL))
                {
                    writer.WriteLine(string.Format(".menuicon-{0}{{", menu.IMAGEURL.Replace(".", "")));
                    writer.WriteLine(string.Format("\tbackground:url('../../{0}') no-repeat center center;", menu.IMAGEURL));
                    writer.WriteLine("}");
                }
                WriteCss(writer, menu.MENUTABLE1);
            }
        }
    }
}
