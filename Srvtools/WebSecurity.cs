using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Xml;
using System.Windows.Forms.Design;
using Microsoft.Win32;
using System.IO;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Reflection;
#if UseCrystalReportDD
using CrystalDecisions.Web;
#endif

namespace Srvtools
{
    [Designer(typeof(WebSecurityEdit), typeof(IDesigner))]
    [ToolboxBitmap(typeof(WebSecurity), "Resources.InfoSecurity.png")]
    public partial class WebSecurity : WebControl
    {
        public WebSecurity()
        {
            _webExportControls = new WebExportControls(this, typeof(WebExportControl));
        }

        public WebSecurity(IContainer container)
        {
            container.Add(this);
            _webExportControls = new WebExportControls(this, typeof(WebExportControl));
        }

        private DataTable securityTable;
        /// <summary>
        /// Get the table of security infomation
        /// </summary>
        [Browsable(false)]
        public DataTable SecurityTable
        {
            get
            {
                if (securityTable == null)
                {
                    string menuid = this.MenuID;
                    if (string.IsNullOrEmpty(menuid))
                    {
                        menuid = CliUtils.GetMenuID(this.Page.AppRelativeVirtualPath);
                    }
                    object[] param = new object[] { CliUtils.fLoginUser + ";" + menuid };
                    object[] myRet = CliUtils.CallMethod("GLModule", "GetGroupControl", param);
                    DataSet dsgroup = (DataSet)myRet[1];
                    myRet = CliUtils.CallMethod("GLModule", "GetUserControl", param);
                    DataSet dsUser = (DataSet)myRet[1];
                    securityTable = new DataTable();
                    securityTable.Columns.Add("ControlName", typeof(string));
                    securityTable.Columns.Add("Enabled", typeof(bool));
                    securityTable.Columns.Add("Visible", typeof(bool));
                    securityTable.Columns.Add("AllowAdd", typeof(bool));
                    securityTable.Columns.Add("AllowUpdate", typeof(bool));
                    securityTable.Columns.Add("AllowDelete", typeof(bool));
                    securityTable.Columns.Add("AllowPrint", typeof(bool));
                    InitializeTable(securityTable, dsUser.Tables[0], dsgroup.Tables[0]);
                }
                return securityTable;
            }
        }

        private void InitializeTable(DataTable securityTable, DataTable tableUser, DataTable tableGroup)
        {
            tableUser.CaseSensitive = false; //table ignoreCase
            tableGroup.CaseSensitive = false;

            List<string> listControlName = new List<string>();
            for (int i = 0; i < tableUser.Rows.Count; i++)
            {
                string controlName = (string)tableUser.Rows[i]["ControlName"];
                if (controlName.Length > 0 && !listControlName.Contains(controlName))
                {
                    listControlName.Add(controlName);
                }
            }
            for (int i = 0; i < tableGroup.Rows.Count; i++)
            {
                string controlName = (string)tableGroup.Rows[i]["ControlName"];
                if (controlName.Length > 0 && !listControlName.Contains(controlName))
                {
                    listControlName.Add(controlName);
                }
            }
            for (int i = 0; i < listControlName.Count; i++)
            {
                DataRow dr = securityTable.NewRow();
                dr["ControlName"] = listControlName[i];
                for (int j = 1; j < securityTable.Columns.Count; j++)
                {
                    dr[j] = ComputerValue(tableUser, tableGroup, listControlName[i], securityTable.Columns[j].ColumnName);
                }
                securityTable.Rows.Add(dr);
            }
        }

        private bool ComputerValue(DataTable tableUser, DataTable tableGroup, string controlName, string propertyName)
        {
            string value = string.Empty;
            if (Priority == PriorityName.Group)
            {
                value = GetValue(tableGroup, controlName, propertyName);
                if (value.Length == 0)
                {
                    value = GetValue(tableUser, controlName, propertyName);
                }
            }
            else if (Priority == PriorityName.User)
            {
                value = GetValue(tableUser, controlName, propertyName);
                if (value.Length == 0)
                {
                    value = GetValue(tableGroup, controlName, propertyName);
                }
            }
            return string.Compare(value, "y", true) == 0;
        }

        private string GetValue(DataTable table, string controlName, string propertyName)
        {
            string str = RelaxMode ? "Max" : "Min";
            return table.Compute(string.Format("{0}({1})", str, propertyName), string.Format("ControlName='{0}'", controlName)).ToString();
        }

        private void SetControl(Control parent)
        {
            if (parent.Controls.Count > 0)
            {
                DataTable table = SecurityTable;
                foreach (Control ctrl in parent.Controls)
                {
                    if (!string.IsNullOrEmpty(ctrl.ID))
                    {
                        DataRow[] rows = table.Select(string.Format("ControlName='{0}'", ctrl.ID));
                        if (rows.Length > 0)
                        {
                            Type typeObj = ctrl.GetType();
                            for (int i = 0; i < table.Columns.Count; i++)
                            {
                                PropertyInfo property = typeObj.GetProperty(table.Columns[i].ColumnName);
                                object[] att = typeObj.GetCustomAttributes(typeof(System.Web.UI.NonVisualControlAttribute), true);
                                if (att.Length == 0 || table.Columns[i].ColumnName != "Visible")
                                {
                                    if (property != null && property.PropertyType == typeof(bool))
                                    {
                                        property.SetValue(ctrl, rows[0][i], null);
                                    }
                                }
                            }
                            if (ctrl is WebSecColumns)
                            {
                                (ctrl as WebSecColumns).ColumnsReadOnly = !(bool)rows[0]["Enabled"];
                                (ctrl as WebSecColumns).ColumnsVisible = (bool)rows[0]["Visible"];
                                (ctrl as WebSecColumns).Do();
                            }
                            else if (ctrl.GetType().Name == "ExtSecColumns")
                            {
                                ctrl.GetType().GetProperty("ReadOnly").SetValue(ctrl, !(bool)rows[0]["Enabled"], null);
                                ctrl.GetType().GetProperty("Visible").SetValue(ctrl, (bool)rows[0]["Visible"], null);
                                ctrl.GetType().GetMethod("Do").Invoke(ctrl, null);
                            }
                            else if (ctrl.GetType().Name == "AjaxSecColumns")
                            {
                                ctrl.GetType().GetProperty("ReadOnly").SetValue(ctrl, !(bool)rows[0]["Enabled"], null);
                                ctrl.GetType().GetProperty("Visible").SetValue(ctrl, (bool)rows[0]["Visible"], null);
                                ctrl.GetType().GetMethod("Do").Invoke(ctrl, null);
                            }

#if UseCrystalReportDD
                            else if (ctrl is CrystalReportViewer)
                            {
                                (ctrl as CrystalReportViewer).HasExportButton = (bool)rows[0]["AllowPrint"];
                                (ctrl as CrystalReportViewer).HasPrintButton = (bool)rows[0]["AllowPrint"];
                            } 
#endif
                        }
                    }
                    SetControl(ctrl);
                }

            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.Active)
            {
                if (CheckMenuRight)
                {
                    if (!CliUtils.CheckMenuRightsByName(this.Page.AppRelativeVirtualPath))
                    {
                        System.Web.HttpContext.Current.Session.Abandon();
                        Page.Response.Redirect(string.Format("{0}/Infologin.aspx", Page.Request.ApplicationPath));
                    }
                }
                if (!Page.IsPostBack && !string.IsNullOrEmpty(CliUtils.fLoginUser))
                {
                    SetControl(this.Parent);
                }
            }
        }

        private string _DBAlias;
        [Category("Infolight"),
        Description("Specifies the database to store the data of security")]
        [Editor(typeof(WebGetAlias), typeof(System.Drawing.Design.UITypeEditor))]
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

        private string _MenuID;
        [Category("Infolight"),
        Description("Specifies the menuID which the WebSecurity is applied to"),
        Editor(typeof(WebGetMenuID), typeof(UITypeEditor))]
        public string MenuID
        {
            get
            {
                return _MenuID;
            }
            set
            {
                _MenuID = value;
            }
        }

        private bool _Active = true;
        [Category("Infolight"),
        Description("Indicates whether WebSecurity is enabled or disabled")]
        public bool Active
        {
            get
            {
                return _Active;
            }
            set
            {
                _Active = value;
            }
        }

        private bool checkMenuRight;
        [Category("Infolight"),
        Description("Indicates whether to check the right of user to view current page")]
        public bool CheckMenuRight
        {
            get { return checkMenuRight; }
            set { checkMenuRight = value; }
        }

        private bool _RelaxMode = true;
        [Category("Infolight"),
         Description("Indicates whether the mode of security certification")]
        public bool RelaxMode
        {
            get
            {
                return _RelaxMode;
            }
            set
            {
                _RelaxMode = value;
            }
        }

        private WebExportControls _webExportControls;
        [Category("Infolight"),
        Description("Specifies the controls which WebSecurity is applied to")]
        [PersistenceMode(PersistenceMode.InnerProperty),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         TypeConverter(typeof(CollectionConverter)),
         NotifyParentProperty(true)]
        public WebExportControls webExportControls
        {
            get
            {
                return _webExportControls;
            }
        }

        private PriorityName _Priority;
        [Category("Infolight"),
          Description("Specifies security is set by group or user first")]
        public PriorityName Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                _Priority = value;
            }
        }
    }

    public class WebExportControl : InfoOwnerCollectionItem
    {
        private string _Name = "";
        override public string Name
        {
            get
            {
                if (ControlName != null)
                    return this.ControlName;
                else
                    return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        private string _ControlName;
        [Editor(typeof(WebGetControlName), typeof(UITypeEditor))]
        public string ControlName
        {
            get
            {
                return _ControlName;
            }
            set
            {
                _ControlName = value;
            }
        }

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {

                _Description = value;
            }
        }

        private string _Type;
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {

                _Type = value;
            }
        }
    }

    public class WebExportControls : InfoOwnerCollection
    {
        public WebExportControls() { }

        public WebExportControls(object aOwner, Type aItemType)
            : base(aOwner, typeof(WebExportControl))
        {

        }

        new public WebExportControl this[int index]
        {
            get
            {
                return (WebExportControl)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is WebExportControl)
                    {
                        //原来的Collection设置为0
                        ((WebExportControl)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebExportControl)InnerList[index]).Collection = this;
                    }
            }
        }
    }

    public class WebGetAlias : System.Drawing.Design.UITypeEditor
    {
        public WebGetAlias()
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
                WebSecurity ws = context.Instance as WebSecurity;
                if (ws != null)
                {
                    XmlDocument DBXML = new XmlDocument();
                    if (File.Exists(SystemFile.DBFile))
                    {
                        DBXML.Load(SystemFile.DBFile);
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

    public class WebGetMenuID : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public WebGetMenuID()
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
                WebSecurity ws = context.Instance as WebSecurity;
                if (ws != null)
                {
                    object[] param = new object[1];
                    param[0] = ws.DBAlias.ToString();
                    CliUtils.fLoginDB = ws.DBAlias;
                    object[] myRet = CliUtils.CallMethod("GLModule", "GetMenuID", param);
                    CliUtils.fLoginDB = "";
                    if ((myRet != null) && (0 == (int)myRet[0]))
                    {
                        //List<string> listMenuID = (List<string>)myRet[1];
                        //List<string> listCaption = (List<string>)myRet[2];
                        ArrayList listMenuID = (ArrayList)myRet[1];
                        ArrayList listCaption = (ArrayList)myRet[2];
                        for (int i = 0; i < listMenuID.Count; i++)
                        {
                            MenuIDs.Items.Add(listMenuID[i] + "(" + listCaption[i] + ")");
                            MenuID.Items.Add(listMenuID[i]);
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

    public class WebGetControlName : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public WebGetControlName()
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
                System.Windows.Forms.ListBox ControlName = new System.Windows.Forms.ListBox();
                ControlName.SelectionMode = System.Windows.Forms.SelectionMode.One;
                ControlName.Items.Add("(none)");
                WebExportControl ec = context.Instance as WebExportControl;
                if (ec != null)
                {
                    WebSecurity ict = ((WebSecurity)(ec.Owner));
                }

                object[] param = new object[1];
                param[0] = ((WebSecurity)(ec.Owner)).MenuID;
                CliUtils.fLoginDB = ((WebSecurity)(ec.Owner)).DBAlias;
                object[] myRet = CliUtils.CallMethod("GLModule", "GetMenu", param);
                CliUtils.fLoginDB = "";
                ControlName.SelectedIndexChanged += delegate(object sender, EventArgs e)
                {
                    int index = ControlName.SelectedIndex;
                    if (index != -1)
                    {
                        if (index == 0)
                        {
                            value = "";
                        }
                        else
                        {
                            value = ControlName.Items[index].ToString();
                            if ((myRet != null) && (0 == (int)myRet[0]))
                            {
                                //List<string> listControlName = (List<string>)myRet[1];
                                //List<string> listDescription = (List<string>)myRet[2];
                                ArrayList listControlName = (ArrayList)myRet[1];
                                ArrayList listDescription = (ArrayList)myRet[2];
                                for (int i = 0; i < listControlName.Count; i++)
                                {
                                    if (listControlName[i].ToString() == value.ToString())
                                        ec.Description = listDescription[i].ToString();
                                }
                            }
                        }
                    }
                    EditorService.CloseDropDown();

                };

                EditorService.DropDownControl(ControlName);

            }

            return value;
        }
    }
}
