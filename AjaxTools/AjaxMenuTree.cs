using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Text;
using System.Web.UI;
using Srvtools;

namespace AjaxTools
{


    [ParseChildren(true)]
    [PersistChildren(false)]
    public class AjaxMenuTree : AjaxBaseControl, IAjaxDataSource
    {
        bool _useMenuTable = true;
        bool _treeDefaultIcon = true;
        string _target = "";
        string _renderTo = "";
        string _title = "";
        string _rootParentId = "";
        int _width = 0;
        int _height = 0;
        string _dataSourceId = "";
        string _menuIdField = "";
        string _menuCaptionField = "";
        string _menuParentField = "";
        string _menuUrlField = "";
        string _menuImageUrlField = "";

        #region Properties
        [Category("Infolight")]
        [DefaultValue(true)]
        public bool UseMenuTable
        {
            get { return _useMenuTable; }
            set { _useMenuTable = value; }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool TreeDefaultIcon
        {
            get { return _treeDefaultIcon; }
            set { _treeDefaultIcon = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string RenderTo
        {
            get { return _renderTo; }
            set { _renderTo = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string RootParentId
        {
            get { return _rootParentId; }
            set { _rootParentId = value; }
        }

        [Category("Infolight")]
        [DefaultValue(0)]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        [Category("Infolight")]
        [DefaultValue(0)]
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get { return _dataSourceId; }
            set { _dataSourceId = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuIdField
        {
            get { return _menuIdField; }
            set { _menuIdField = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuCaptionField
        {
            get { return _menuCaptionField; }
            set { _menuCaptionField = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuParentField
        {
            get { return _menuParentField; }
            set { _menuParentField = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuUrlField
        {
            get { return _menuUrlField; }
            set { _menuUrlField = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuImageUrlField
        {
            get { return _menuImageUrlField; }
            set { _menuImageUrlField = value; }
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RenderMenu();
        }

        public void RenderMenu()
        {
            StringBuilder builder = new StringBuilder();

            DataTable tab = null;
            //string sql = "";
            MenuTreeFieldConfig config = new MenuTreeFieldConfig();
            if (this.UseMenuTable)
            {
                //sql = string.Format("SELECT * FROM MENUTABLE WHERE ({0} OR [PARENT] IN (SELECT MENUID FROM MENUTABLE WHERE {0})) AND MODULETYPE IN ('W','O') AND ITEMTYPE IN (SELECT ITEMTYPE FROM MENUITEMTYPE WHERE ITEMTYPE = '{1}') AND (MENUID IN (SELECT MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='{2}') OR GROUPID='00') OR MENUID IN (SELECT MENUID FROM USERMENUS WHERE USERID = '{2}') OR (ISNULL([PARENT],'')='' AND ISNULL([PACKAGE],'')='')) ORDER BY SEQ_NO",
                //    string.IsNullOrEmpty(this.RootParentId) ? "ISNULL([PARENT],'')=''" : string.Format("[PARENT]='{0}'", this.RootParentId),
                //    CliUtils.fCurrentProject,
                //    CliUtils.fLoginUser);
                //tab = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", sql, true, CliUtils.fCurrentProject).Tables[0];

                object[] menuResult = CliUtils.CallMethod("GLModule", "FetchMenus", new object[] { CliUtils.fCurrentProject, "W" });
                if (menuResult != null && (int)menuResult[0] == 0)
                {
                    tab = ((DataSet)menuResult[1]).Tables[0];
                }
                config.IdField = "MENUID";
                config.CaptionField = "CAPTION";
                config.ParentField = "PARENT";
                config.ImageUrlField = "IMAGEURL";
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

                    config.IdField = this.MenuIdField;
                    config.CaptionField = this.MenuCaptionField;
                    config.ParentField = this.MenuParentField;
                    config.ImageUrlField = this.MenuImageUrlField;
                    config.UrlField = this.MenuUrlField;

                    if (string.IsNullOrEmpty(config.IdField) || string.IsNullOrEmpty(config.CaptionField) || string.IsNullOrEmpty(config.ParentField)
                        || string.IsNullOrEmpty(config.ImageUrlField) || string.IsNullOrEmpty(config.UrlField)) return;
                }
            }

            if (tab == null || tab.Rows.Count == 0) return;

            builder.AppendFormat("var {0}Config={{{1}{2}{3}renderTo:{4},items:[{5}],menuCls:[{6}]}};Ext.onReady(renderMenuTree, {0}Config);",
                this.ID,
                string.IsNullOrEmpty(this.Title) ? "" : string.Format("title:'{0}',", this.Title),
                this.Width <= 0 ? "" : string.Format("width:{0},", this.Width),
                this.Height <= 0 ? "" : string.Format("height:{0},", this.Height),
                string.IsNullOrEmpty(this.RenderTo) ? "Ext.getBody()" : string.Format("'{0}'", this.RenderTo),
                this.GenMenuItems(tab, config),
                this.GenMenuCssClass(tab, config));

            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), builder.ToString(), true);
        }

        public string GenMenuItems(DataTable table, MenuTreeFieldConfig fieldConfig)
        {
            StringBuilder builder = new StringBuilder();

            DataRow[] rows = table.Select(
                string.IsNullOrEmpty(this.RootParentId) ?
                string.Format("ISNULL([{0}],'')=''", fieldConfig.ParentField) :
                string.Format("[{0}]='{1}'", fieldConfig.ParentField, this.RootParentId));
            if (rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    string csscls = "";
                    //if (row[fieldConfig.ImageUrlField] != null && row[fieldConfig.ImageUrlField].ToString() != "")
                    //{
                    //    csscls = string.Format("iconCls:'{0}_menu{1}_icon',", this.ID, row[fieldConfig.IdField]);
                    //}
                    //builder.AppendFormat("new Ext.Panel({{ title:'{0}',{1}items:[genTree({2})]}}),",
                    //    row[fieldConfig.CaptionField],
                    //    csscls,
                    //    this.GenTreeItems(table, fieldConfig, row));
                    if (row[fieldConfig.ImageUrlField] != null && row[fieldConfig.ImageUrlField].ToString() != "")
                    {
                        csscls = string.Format("{0}_menu{1}_icon", this.ID, row[fieldConfig.IdField]);
                    }
                    builder.AppendFormat("genTree('{0}','{1}',{2}),",
                        row[fieldConfig.CaptionField],
                        csscls,
                        this.GenTreeItems(table, fieldConfig, row));
                }
                if (builder.ToString().EndsWith(","))
                {
                    builder.Remove(builder.Length - 1, 1);
                }
            }
            return builder.ToString();
        }

        public string GenTreeItems(DataTable table, MenuTreeFieldConfig fieldConfig, DataRow parentRow)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            DataRow[] rows = table.Select(string.Format("[{0}]='{1}'", fieldConfig.ParentField, parentRow[fieldConfig.IdField]));
            if (rows.Length > 0)
            {
                string subPath = "";
                for (int i = 0; i < this.Page.Request.FilePath.Split('/').Length - 3; i++)
                {
                    subPath += "../";
                }
                foreach (DataRow row in rows)
                {
                    builder.Append("{");
                    DataRow[] childRows = table.Select(string.Format("[{0}]='{1}'", fieldConfig.ParentField, row[fieldConfig.IdField]));
                    string image = "";
                    if (!TreeDefaultIcon)
                    {
                        image = "iconCls:'icon-none',";
                    }
                    if (row[fieldConfig.ImageUrlField] != null && row[fieldConfig.ImageUrlField].ToString() != "")
                    {
                        image = string.Format("icon:'{0}Image/MenuTree/{1}',", subPath, row[fieldConfig.ImageUrlField]);
                    }
                    if (childRows.Length > 0)
                    {
                        builder.AppendFormat("text:'{0}',{1}children:{2}",
                            row[fieldConfig.CaptionField],
                            image,
                            this.GenTreeItems(table, fieldConfig, row));
                    }
                    else
                    {
                        string href = "";
                        if (this.UseMenuTable)
                        {
                            if (row["PACKAGE"] != null && row["PACKAGE"].ToString() != "" && row["FORM"] != null && row["FORM"].ToString() != "")
                            {
                                href = string.Format("{0}{1}/{2}.aspx", subPath, row["PACKAGE"], row["FORM"]);
                            }
                        }
                        else
                        {
                            if (row[fieldConfig.UrlField] != null && row[fieldConfig.UrlField].ToString() != "")
                            {
                                href = this.ResolveClientUrl(row[fieldConfig.UrlField].ToString());
                            }
                        }

                        string target = "";
                        if (!string.IsNullOrEmpty(href))
                        {
                            href = string.Format("href:'{0}',", href);
                            target = string.IsNullOrEmpty(this.Target) ? "hrefTarget:'_blank'," : string.Format("hrefTarget:'{0}',", this.Target);
                        }
                        builder.AppendFormat("text:'{0}',{1}{2}{3}leaf:true",
                            row[fieldConfig.CaptionField],
                            image,
                            href,
                            target);
                    }
                    builder.Append("},");
                }
                if (builder.ToString().EndsWith(","))
                {
                    builder.Remove(builder.Length - 1, 1);
                }
            }
            builder.Append("]");
            return builder.ToString();
        }

        public string GenMenuCssClass(DataTable table, MenuTreeFieldConfig fieldConfig)
        {
            StringBuilder builder = new StringBuilder();

            DataRow[] rows = table.Select(
                string.IsNullOrEmpty(this.RootParentId) ?
                string.Format("ISNULL([{0}],'')=''", fieldConfig.ParentField) :
                string.Format("[{0}]='{1}'", fieldConfig.ParentField, this.RootParentId));
            if (rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    if (row[fieldConfig.ImageUrlField] != null && row[fieldConfig.ImageUrlField].ToString() != "")
                    {
                        builder.Append("{");
                        builder.AppendFormat("iconCls:'{0}_menu{1}_icon',iconFile:'{2}'", this.ID, row[fieldConfig.IdField], row[fieldConfig.ImageUrlField]);
                        builder.Append("},");
                    }
                }
                if (builder.ToString().EndsWith(","))
                {
                    builder.Remove(builder.Length - 1, 1);
                }
            }
            return builder.ToString();
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

        public struct MenuTreeFieldConfig
        {
            internal string IdField;
            internal string CaptionField;
            internal string ParentField;
            internal string ImageUrlField;

            internal string UrlField;

        }
    }
}
