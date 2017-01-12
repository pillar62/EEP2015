using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using AjaxControlToolkit;
using System.Web.UI.HtmlControls;
using System.Data;
using Srvtools;
using System.ComponentModel.Design;
using System.Web;

namespace AjaxTools
{
    [ParseChildren(false)]
    [Designer(typeof(DataSourceDesigner), typeof(IDesigner))]
    public class AjaxCollapseMenu : AjaxBaseWebControl, IAjaxDataSource, INamingContainer
    {
        Accordion _accordion = new Accordion();

        #region Properties
        [Category("Infolight")]
        [DefaultValue(true)]
        public bool UseMenuTable
        {
            get
            {
                object obj = this.ViewState["UseMenuTable"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["UseMenuTable"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
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
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuIDField
        {
            get
            {
                object obj = this.ViewState["MenuIDField"];
                if (obj != null)
                    return (string)obj;
                return "";
            }
            set
            {
                this.ViewState["MenuIDField"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuParentField
        {
            get
            {
                object obj = this.ViewState["MenuParentField"];
                if (obj != null)
                    return (string)obj;
                return "";
            }
            set
            {
                this.ViewState["MenuParentField"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuTextField
        {
            get
            {
                object obj = this.ViewState["MenuTextField"];
                if (obj != null)
                    return (string)obj;
                return "";
            }
            set
            {
                this.ViewState["MenuTextField"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuImageField
        {
            get
            {
                object obj = this.ViewState["MenuImageField"];
                if (obj != null)
                    return (string)obj;
                return "";
            }
            set
            {
                this.ViewState["MenuImageField"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuUrlField
        {
            get
            {
                object obj = this.ViewState["MenuUrlField"];
                if (obj != null)
                    return (string)obj;
                return "";
            }
            set
            {
                this.ViewState["MenuUrlField"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string MenuIDStartValue
        {
            get
            {
                object obj = this.ViewState["MenuIDStartValue"];
                if (obj != null)
                    return (string)obj;
                return "";
            }
            set
            {
                this.ViewState["MenuIDStartValue"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string Target
        {
            get
            {
                object obj = this.ViewState["Target"];
                if (obj != null)
                    return (string)obj;
                return "";
            }
            set
            {
                this.ViewState["Target"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(AjaxColorStyle), "Blue")]
        public AjaxColorStyle ColorStyle
        {
            get
            {
                object obj = this.ViewState["ColorStyle"];
                if (obj != null)
                    return (AjaxColorStyle)obj;
                return AjaxColorStyle.Blue;
            }
            set
            {
                this.ViewState["ColorStyle"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        public bool UserProvideStyle
        {
            get
            {
                object obj = this.ViewState["UserProvideStyle"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["UserProvideStyle"] = value;
            }
        }
        #endregion

        private List<AjaxMenuRootItem> GenItems()
        {
            List<AjaxMenuRootItem> items = null;
            DataTable tab = null;
            string parentSql = "";
            if (this.UseMenuTable)
            {
                parentSql = string.IsNullOrEmpty(this.MenuIDStartValue) ? "ISNULL([PARENT],'')=''" : "[PARENT]='{0}'";
                //string sql = string.Format("SELECT * FROM MENUTABLE WHERE (" + parentSql + " OR [PARENT] IN (SELECT MENUID FROM MENUTABLE WHERE " + parentSql + ")) AND MODULETYPE IN ('W','O') AND ITEMTYPE IN (SELECT ITEMTYPE FROM MENUITEMTYPE WHERE ITEMTYPE = '{2}') AND (MENUID IN (SELECT MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='{1}') OR GROUPID='00') OR MENUID IN (SELECT MENUID FROM USERMENUS WHERE USERID = '{1}') OR (ISNULL([PARENT],'')='' AND ISNULL([PACKAGE],'')='')) ORDER BY SEQ_NO", this.MenuIDStartValue, CliUtils.fLoginUser, CliUtils.fCurrentProject);
                //tab = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", sql, true, CliUtils.fCurrentProject).Tables[0];
                object[] menuResult = CliUtils.CallMethod("GLModule", "FetchMenus", new object[] { CliUtils.fCurrentProject, "W" });
                if (menuResult != null && (int)menuResult[0] == 0)
                {
                    tab = ((DataSet)menuResult[1]).Tables[0];
                }
            }
            else
            {
                WebDataSource wds = this.GetDataSource();
                if (wds != null)
                {
                    string dbAlias = this.GetDBAlias();
                    string cmdText = this.GetCommandText();
                    if (dbAlias != "" && cmdText != "")
                        tab = wds.CommandTable;
                    else
                        tab = wds.InnerDataSet.Tables[0];
                }
                parentSql = string.IsNullOrEmpty(this.MenuIDStartValue) ? string.Format("ISNULL([{0}],'')=''", this.MenuParentField) : ("[" + this.MenuParentField + "]='{0}'");
            }
            if (tab != null && tab.Rows.Count > 0)
            {
                DataRow[] rootRows = tab.Select(string.Format(parentSql, this.MenuIDStartValue)).Clone() as DataRow[];
                string LanIndex = ((int)CliUtils.fClientLang).ToString();
                foreach (DataRow rootRow in rootRows)
                {
                    AjaxMenuRootItem rootItem = new AjaxMenuRootItem();
                    rootItem.MenuId = rootRow[this.UseMenuTable ? "MENUID" : this.MenuIDField].ToString();
                    rootItem.Caption = rootRow[this.UseMenuTable ? "CAPTION" + LanIndex : this.MenuTextField].ToString();
                    if (rootItem.Caption == "")
                        rootItem.Caption = rootRow[this.UseMenuTable ? "CAPTION" : this.MenuTextField].ToString();
                    rootItem.ImageUrl = rootRow[this.UseMenuTable ? "IMAGEURL" : this.MenuImageField].ToString();

                    DataRow[] leafRows = tab.Select(string.Format(this.UseMenuTable ? "[PARENT]='{0}'" : "[" + this.MenuParentField + "]='{0}'", rootItem.MenuId)).Clone() as DataRow[];
                    foreach (DataRow leafRow in leafRows)
                    {
                        AjaxMenuLeafItem leafItem = new AjaxMenuLeafItem();
                        leafItem.MenuId = leafRow[this.UseMenuTable ? "MENUID" : this.MenuIDField].ToString();
                        leafItem.Parent = leafRow[this.UseMenuTable ? "PARENT" : this.MenuParentField].ToString();
                        leafItem.Caption = leafRow[this.UseMenuTable ? "CAPTION" + LanIndex : this.MenuTextField].ToString();
                        if (leafItem.Caption == "")
                            leafItem.Caption = leafRow[this.UseMenuTable ? "CAPTION" : this.MenuTextField].ToString();
                        leafItem.ImageUrl = leafRow[this.UseMenuTable ? "IMAGEURL" : this.MenuImageField].ToString();
                        if (this.UseMenuTable)
                        {
                            if (leafRow["MODULETYPE"].ToString() == "O")
                            {
                                leafItem.Href = string.Format("InnerPages/FlowDesigner.aspx?FlowFileName={0}", HttpUtility.UrlEncode(leafRow["FORM"].ToString()));
                            }
                            else
                            {
                                string param = "";
                                if (leafRow["ITEMPARAM"] != null && !string.IsNullOrEmpty(leafRow["ITEMPARAM"].ToString()))
                                {
                                    param = "?" + ConvertParamter(leafRow["ITEMPARAM"].ToString());
                                }
                                leafItem.Href = string.Format("{0}/{1}.aspx{2}", 
                                    leafRow["PACKAGE"].ToString(), 
                                    leafRow["FORM"].ToString(),
                                    param);
                            }
                        }
                        else
                        {
                            leafItem.Href = leafRow[this.MenuUrlField].ToString();
                        }

                        rootItem.Items.Add(leafItem);
                    }
                    if (leafRows.Length > 0)
                    {
                        if (items == null) items = new List<AjaxMenuRootItem>();
                        items.Add(rootItem);
                    }
                }
            }
            return items;
        }

        private string ConvertParamter(string obj)
        {
            string param = "";
            if (obj.ToLower().StartsWith("flowfilename"))
            {
                string[] arrParam = obj.Split(';');
                foreach (string pa in arrParam)
                {
                    param += HttpUtility.UrlEncode(pa).Replace(HttpUtility.UrlEncode("="), "=") + "&";
                }
                if (param != "")
                    param = param.Substring(0, param.LastIndexOf('&'));
            }
            else
            {
                param = "ItemParam=" + HttpUtility.UrlEncode(obj);
            }
            return param;
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            List<AjaxMenuRootItem> rootItems = this.GenItems();
            if (rootItems == null)
            {
                throw new Exception("root item is not exist, please check the 'MenuIDStartValue' property!");
            }

            _accordion = new Accordion();
            _accordion.Width = this.Width;
            _accordion.Height = this.Height;
            _accordion.ID = "InnerAccordion";
            if (this.UserProvideStyle)
            {
                _accordion.HeaderCssClass = "ajaxcm_user_header_unselected";
#if VS90
                _accordion.HeaderSelectedCssClass = "ajaxcm_user_header_selected";
#endif
                _accordion.ContentCssClass = "ajaxcm_user_content";
            }
            else
            {
                switch (this.ColorStyle)
                {
                    case AjaxColorStyle.Blue:
                        _accordion.HeaderCssClass = "ajaxcm_header_unselected1";
#if VS90
                        _accordion.HeaderSelectedCssClass = "ajaxcm_header_selected1";
#endif
                        _accordion.ContentCssClass = "ajaxcm_content1";
                        break;
                    case AjaxColorStyle.Green:
                        _accordion.HeaderCssClass = "ajaxcm_header_unselected1";
#if VS90
                        _accordion.HeaderSelectedCssClass = "ajaxcm_header_selected1";
#endif
                        _accordion.ContentCssClass = "ajaxcm_content2";
                        break;
                    case AjaxColorStyle.White:
                        _accordion.HeaderCssClass = "ajaxcm_header_unselected2";
#if VS90
                        _accordion.HeaderSelectedCssClass = "ajaxcm_header_selected2";
#endif
                        _accordion.ContentCssClass = "ajaxcm_content3";
                        break;
                    case AjaxColorStyle.Black:
                        _accordion.HeaderCssClass = "ajaxcm_header_unselected2";
#if VS90
                        _accordion.HeaderSelectedCssClass = "ajaxcm_header_selected1";
#endif
                        _accordion.ContentCssClass = "ajaxcm_content4";
                        break;
                }
            }
            _accordion.FadeTransitions = true;
            _accordion.FramesPerSecond = 40;
            _accordion.TransitionDuration = 250;
            _accordion.AutoSize = AutoSize.Limit;
#if VS90
            _accordion.RequireOpenedPane = true;
            _accordion.SuppressHeaderPostbacks = true;
#endif
            foreach (AjaxMenuRootItem rootItem in rootItems)
            {
                AccordionPane pane = new AccordionPane();
                pane.Header = this.UserProvideStyle ? new CollapseMenuHeaderTemplate(rootItem, true, this.ColorStyle) : new CollapseMenuHeaderTemplate(rootItem, false, this.ColorStyle);
                pane.Content = new CollapseMenuContentTemplate(rootItem.Items, this.Target);
                _accordion.Panes.Add(pane);
            }
            this.Controls.Add(_accordion);
        }

        public void Refresh()
        {
            CreateChildControls();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            _accordion.RenderControl(writer);
        }

        public WebDataSource GetDataSource()
        {
            return this.GetObjByID(this.DataSourceID) as WebDataSource;
        }

        public string GetDBAlias()
        {
            WebDataSource wds = this.GetDataSource();
            if (wds != null && !string.IsNullOrEmpty(wds.SelectAlias))
            {
                return wds.SelectAlias;
            }
            return "";
        }

        public string GetCommandText()
        {
            WebDataSource wds = this.GetDataSource();
            if (wds != null && !string.IsNullOrEmpty(wds.SelectCommand))
            {
                return wds.SelectCommand;
            }
            return "";
        }
    }

    public class CollapseMenuHeaderTemplate : ITemplate
    {
        public CollapseMenuHeaderTemplate(AjaxMenuRootItem rootItem, bool userProvide, AjaxColorStyle style)
        {
            _item = rootItem;
            _style = style;
            _userProvide = userProvide;
        }

        AjaxMenuRootItem _item = null;
        AjaxColorStyle _style = AjaxColorStyle.Blue;
        bool _userProvide = false;

        public void InstantiateIn(Control container)
        {
            if (_userProvide)
            {
                if (!string.IsNullOrEmpty(_item.ImageUrl))
                {
                    Image img = new Image();
                    img.CssClass = "img";
                    img.ImageUrl = string.Format("~/Image/MenuTree/{0}", _item.ImageUrl);
                    img.AlternateText = _item.MenuId;
                    container.Controls.Add(img);
                }

                Label lbl = new Label();
                lbl.Text = _item.Caption;
                lbl.CssClass = "ajaxcm_lbl";
                container.Controls.Add(lbl);
            }
            else
            {
                Table tab = new Table();
                tab.CellPadding = 0;
                tab.CellSpacing = 0;
                tab.Width = new Unit(100, UnitType.Percentage);
                TableRow row = new TableRow();
                TableCell cell_left = new TableCell();
                switch (_style)
                {
                    case AjaxColorStyle.Blue: cell_left.CssClass = "ajaxcm_header_left_1"; break;
                    case AjaxColorStyle.Green: cell_left.CssClass = "ajaxcm_header_left_2"; break;
                    case AjaxColorStyle.White: cell_left.CssClass = "ajaxcm_header_left_3"; break;
                    case AjaxColorStyle.Black: cell_left.CssClass = "ajaxcm_header_left_4"; break;
                }
                row.Cells.Add(cell_left);

                TableCell cell_middle = new TableCell();
                switch (_style)
                {
                    case AjaxColorStyle.Blue: cell_middle.CssClass = "ajaxcm_header_middle_1"; break;
                    case AjaxColorStyle.Green: cell_middle.CssClass = "ajaxcm_header_middle_2"; break;
                    case AjaxColorStyle.White: cell_middle.CssClass = "ajaxcm_header_middle_3"; break;
                    case AjaxColorStyle.Black: cell_middle.CssClass = "ajaxcm_header_middle_4"; break;
                }
                if (!string.IsNullOrEmpty(_item.ImageUrl))
                {
                    Image img = new Image();
                    img.CssClass = "ajaxcm_img";
                    img.ImageUrl = string.Format("~/Image/MenuTree/{0}", _item.ImageUrl);
                    img.AlternateText = _item.MenuId;
                    cell_middle.Controls.Add(img);
                }

                Label lbl = new Label();
                lbl.Text = _item.Caption;
                lbl.CssClass = "ajaxcm_lbl";
                cell_middle.Controls.Add(lbl);
                row.Cells.Add(cell_middle);

                TableCell cell_right = new TableCell();
                switch (_style)
                {
                    case AjaxColorStyle.Blue: cell_right.CssClass = "ajaxcm_header_right_1"; break;
                    case AjaxColorStyle.Green: cell_right.CssClass = "ajaxcm_header_right_2"; break;
                    case AjaxColorStyle.White: cell_right.CssClass = "ajaxcm_header_right_3"; break;
                    case AjaxColorStyle.Black: cell_right.CssClass = "ajaxcm_header_right_4"; break;
                }
                row.Cells.Add(cell_right);

                tab.Rows.Add(row);
                container.Controls.Add(tab);
            }
        }
    }

    public class CollapseMenuContentTemplate : ITemplate
    {
        public CollapseMenuContentTemplate(List<AjaxMenuLeafItem> items, string target)
        {
            _items = items;
            _target = target;
        }

        List<AjaxMenuLeafItem> _items = null;
        string _target = "";

        public void InstantiateIn(Control container)
        {
            foreach (AjaxMenuLeafItem item in _items)
            {
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    Image img = new Image();
                    img.CssClass = "ajaxcm_img";
                    img.ImageUrl = string.Format("~/Image/MenuTree/{0}", item.ImageUrl);
                    img.AlternateText = item.MenuId;
                    container.Controls.Add(img);
                }

                HtmlAnchor anchor = new HtmlAnchor();
                anchor.InnerText = item.Caption;
                anchor.HRef = string.IsNullOrEmpty(item.Href) ? "#" : item.Href;
                anchor.Style[HtmlTextWriterStyle.TextDecoration] = "none";
                anchor.Attributes["onmouseover"] = "this.style.textDecoration='underline'";
                anchor.Attributes["onmouseleave"] = "this.style.textDecoration='none'";
                if (!string.IsNullOrEmpty(_target))
                    anchor.Target = _target;
                container.Controls.Add(anchor);

                HtmlGenericControl br = new HtmlGenericControl("br");
                container.Controls.Add(br);
            }
        }
    }
}
