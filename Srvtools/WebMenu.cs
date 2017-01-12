using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing.Design;
using System.Data;
using System.Xml;
using System.Globalization;
using System.Resources;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Srvtools
{
    [ToolboxBitmap(typeof(WebMenu), "Resources.WebMenu.png")]
    public class WebMenu : Menu, IGetValues
    {
        public WebMenu()
        {
            
        }

        [Category("InfoLight"), 
        Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string WebDataSourceID
        {
            get
            {
                object obj = this.ViewState["WebDataSourceID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["WebDataSourceID"] = value;
            }
        }

        [Category("InfoLight"),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuIDField
        {
            get
            {
                object obj = this.ViewState["MenuIDField"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["MenuIDField"] = value;
            }
        }

        [Category("InfoLight"),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuTextField
        {
            get
            {
                object obj = this.ViewState["MenuTextField"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["MenuTextField"] = value;
            }
        }

        [Category("InfoLight"),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuParentField
        {
            get
            {
                object obj = this.ViewState["MenuParentField"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["MenuParentField"] = value;
            }
        }

        private string menuUrlField;
        [Category("InfoLight"),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string MenuUrlField
        {
            get { return menuUrlField; }
            set { menuUrlField = value; }
        }

        private bool useMenuTable;

        public bool UseMenuTable
        {
            get { return useMenuTable; }
            set { useMenuTable = value; }
        }

        public string[] GetValues(string sKind)
        {
            List<string> values = new List<string>();
            if (string.Compare(sKind, "menuidfield", true) == 0 || string.Compare(sKind, "menutextfield", true) == 0
                || string.Compare(sKind, "menuparentfield", true) == 0 || string.Compare(sKind, "menuurlfield", true) == 0)//IgnoreCase
            {
                if (this.WebDataSourceID != null && this.WebDataSourceID != "")
                {
                    object obj = this.GetObjByID(this.WebDataSourceID);
                    if (obj !=null && obj is WebDataSource)
                    {
                        WebDataSource wds = (WebDataSource)obj;
                        if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                        {
                            DataTable table = wds.CommandTable;
                            foreach (DataColumn column in table.Columns)
                            {
                                values.Add(column.ColumnName);
                            }
                        }
                        if (wds.DesignDataSet == null)
                        {
                            WebDataSet ds = WebDataSet.CreateWebDataSet(wds.WebDataSetID);
                            if (ds != null)
                            {
                                wds.DesignDataSet = ds.RealDataSet;
                            }
                        }
                        if (wds.DesignDataSet != null && wds.DesignDataSet.Tables.Contains(wds.DataMember))
                        {
                            foreach (DataColumn column in wds.DesignDataSet.Tables[wds.DataMember].Columns)
                            {
                                values.Add(column.ColumnName);
                            }
                        }
                    }
                }
            }
            return values.ToArray();
        }

        private Control GetAllCtrls(string strid, Control ct)
        {
            if (ct.ID == strid)
            {
                return ct;
            }
            else
            {
                if (ct.HasControls())
                {
                    foreach (Control ctchild in ct.Controls)
                    {
                        Control ctrtn = GetAllCtrls(strid, ctchild);
                        if (ctrtn != null)
                        {
                            return ctrtn;
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }
        public object GetObjByID(string ObjID)
        {
            if (this.Site != null)
            {
                return GetAllCtrls(ObjID, this.Page);
            }
            else
            {
                if (this.Page.Form != null)
                    return GetAllCtrls(ObjID, this.Page.Form);
                else
                    return GetAllCtrls(ObjID, this.Page);
            }
        }

        [Browsable(false)]
        public DataTable MenuTable
        {
            get 
            {
                object[] myRet = CliUtils.CallMethod("GLModule", "FetchMenus", new object[] {CliUtils.fCurrentProject, "W"});
                if (myRet != null && (int)myRet[0] == 0)
                {
                    DataSet ds = (DataSet)(myRet[1]);
                    if (ds.Tables.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                }
                return null;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                DataTable table = null;
                if (UseMenuTable)
                {
                    table = MenuTable;
                    MenuIDField = "MENUID";
                    MenuParentField = "PARENT";
                    MenuTextField = "CAPTION";
                    MenuUrlField = "'~\\' + PACKAGE + '\\' + FORM + '.aspx?ItemParam=' + ITEMPARAM";
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.WebDataSourceID) && !string.IsNullOrEmpty(this.MenuIDField)
                        && !string.IsNullOrEmpty(this.MenuParentField))
                    {
                        object obj = this.GetObjByID(this.WebDataSourceID);
                        if (obj != null && obj is WebDataSource)
                        {
                            WebDataSource wds = (WebDataSource)obj;
                            table = (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand)) ?
                                wds.CommandTable : wds.InnerDataSet.Tables[wds.DataMember];
                        }
                    }
                }
                if (table != null)
                {
                    List<string> MenuIDList = new List<string>();
                    List<string> MenuTextList = new List<string>();
                    List<string> MenuParentList = new List<string>();
                    List<string> MenuUrlList = new List<string>();
                    DataColumn column = null;
                    if (!string.IsNullOrEmpty(MenuUrlField) && !table.Columns.Contains(MenuUrlField))
                    {
                        //create a expression field
                        column = new DataColumn();
                        column.ColumnName = MenuUrlField;
                        column.Expression = MenuUrlField;
                        table.Columns.Add(column);
                    }
                    foreach (DataRow row in table.Rows)
                    {
                        MenuIDList.Add(row[this.MenuIDField].ToString());
                        if (!string.IsNullOrEmpty(this.MenuTextField))
                        {
                            MenuTextList.Add(row[this.MenuTextField].ToString());
                        }
                        else
                        {
                            MenuTextList.Add(row[this.MenuIDField].ToString());
                        }
                        MenuParentList.Add(row[this.MenuParentField].ToString());
                        if (!string.IsNullOrEmpty(MenuUrlField))
                        {
                            MenuUrlList.Add(row[this.MenuUrlField].ToString());
                        }
                        else
                        {
                            MenuUrlList.Add(string.Empty);
                        }
                    }
                    if (column != null)
                    {
                        table.Columns.Remove(column);
                    }
                    this.initializeMenu(MenuIDList, MenuTextList, MenuParentList, MenuUrlList);
                }
            }
            base.OnLoad(e);
        }

        private void initializeMenu(List<string> menuIDList, List<string> menuCaptionList, List<string> menuParentIDList, List<string> menuUrlList)
        {
            List<string> ListMainID = new List<string>();
            List<string> ListMainCaption = new List<string>();
            List<string> ListMainUrl = new List<string>();
            List<string> ListChildrenID = new List<string>();
            List<string> ListOwnerParentID = new List<string>();
            List<string> ListChildrenCaption = new List<string>();
            List<string> ListChildernUrl = new List<string>();
            for (int i = 0; i < menuIDList.Count; i++)
            {
                if (string.IsNullOrEmpty(menuParentIDList[i]))
                {
                    ListMainID.Add(menuIDList[i]);
                    ListMainCaption.Add(menuCaptionList[i]);
                    if (menuUrlList.Count > 0)
                    {
                        ListMainUrl.Add(menuUrlList[i]);
                    }
                }
                else
                {
                    ListChildrenID.Add(menuIDList[i]);
                    ListOwnerParentID.Add(menuParentIDList[i]);
                    ListChildrenCaption.Add(menuCaptionList[i]);
                    if (menuUrlList.Count > 0)
                    {
                        ListChildernUrl.Add(menuUrlList[i]);
                    }
                }
            }
            MenuItem[] nodeMain = new MenuItem[ListMainID.Count];
            for (int i = 0; i < ListMainID.Count; i++)
            {
                nodeMain[i] = new MenuItem();
                nodeMain[i].Text = ListMainCaption[i].ToString();
                if (ListMainUrl.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Target))
                    {
                        nodeMain[i].Target = Target;
                    }
                    if (!string.IsNullOrEmpty(ListMainUrl[i]))
                    {
                        nodeMain[i].NavigateUrl = ListMainUrl[i];
                    }
                }
                OnSetItem(new MenuEventArgs(nodeMain[i]));
                this.Items.Add(nodeMain[i]);
            }
            MenuItem[] nodeChildren = new MenuItem[ListChildrenID.Count];
            for (int i = 0; i < ListChildrenID.Count; i++)
            {
                nodeChildren[i] = new MenuItem();
                nodeChildren[i].Text = ListChildrenCaption[i].ToString();
                if (ListChildernUrl.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Target))
                    {
                        nodeChildren[i].Target = Target;
                    }
                    if (!string.IsNullOrEmpty(ListChildernUrl[i]))
                    {
                        nodeChildren[i].NavigateUrl = ListChildernUrl[i];
                    }
                }
                OnSetItem(new MenuEventArgs(nodeChildren[i]));
            }

            for (int i = 0; i < ListChildrenID.Count; i++)
            {
                for (int j = 0; j < ListMainID.Count; j++)
                {
                    if (ListOwnerParentID[i] == ListMainID[j])
                    {
                        nodeMain[j].ChildItems.Add(nodeChildren[i]);
                        nodeMain[j].Selectable = false;
                    }
                }
                for (int j = 0; j < ListChildrenID.Count; j++)
                {
                    if (ListOwnerParentID[i] == ListChildrenID[j])
                    {
                        nodeChildren[j].ChildItems.Add(nodeChildren[i]);
                        nodeChildren[j].Selectable = false;
                    }
                }
            }
        }

        private static readonly object EventSetItem = new object();

        protected virtual void OnSetItem(MenuEventArgs e)
        {
            MenuEventHandler menuEventHandler = (MenuEventHandler)Events[EventSetItem];
            if (menuEventHandler != null)
            {
                menuEventHandler(this, e);
            }
        }

        [Category("InfoLight"),
        Description("OnSetItem event")]
        public event MenuEventHandler SetItem
        {
            add
            {
                Events.AddHandler(EventSetItem, value);
            }
            remove
            {
                Events.RemoveHandler(EventSetItem, value);
            }
        }
    }
}
