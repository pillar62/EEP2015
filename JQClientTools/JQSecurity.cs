using EFClientTools.EFServerReference;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Windows.Forms.Design;
using System.Xml;

namespace JQClientTools
{
    [Designer(typeof(JQSecurityEdit), typeof(IDesigner))]
    public class JQSecurity : WebControl
    {
        public JQSecurity()
        {
            //_webExportControls = new WebExportControls(this, typeof(WebExportControl));
        }

        protected override void Render(HtmlTextWriter writer) { }

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
                    var groupMenuControls = EFClientTools.ClientUtility.Client.GetAllDataByTableName(EFClientTools.ClientUtility.ClientInfo, "GROUPMENUCONTROL").Cast<GROUPMENUCONTROL>().Where(m => m.MENUID.Equals(this.MenuID)).ToList();
                    var userMenuControls = EFClientTools.ClientUtility.Client.GetAllDataByTableName(EFClientTools.ClientUtility.ClientInfo, "USERMENUCONTROL").Cast<USERMENUCONTROL>().Where(m => m.MENUID.Equals(this.MenuID)).ToList();


                    //if (string.IsNullOrEmpty(menuid))
                    //{
                    //    menuid = CliUtils.GetMenuID(this.Page.AppRelativeVirtualPath);
                    //}

                    securityTable = new DataTable();
                    securityTable.Columns.Add("ControlName", typeof(string));
                    securityTable.Columns.Add("Type", typeof(string));
                    securityTable.Columns.Add("Enabled", typeof(bool));
                    securityTable.Columns.Add("Visible", typeof(bool));
                    securityTable.Columns.Add("AllowAdd", typeof(bool));
                    securityTable.Columns.Add("AllowUpdate", typeof(bool));
                    securityTable.Columns.Add("AllowDelete", typeof(bool));
                    securityTable.Columns.Add("AllowPrint", typeof(bool));
                    InitializeTable(securityTable, userMenuControls, groupMenuControls);
                }
                return securityTable;
            }
        }

        private void InitializeTable(DataTable securityTable, List<USERMENUCONTROL> userMenuControls, List<GROUPMENUCONTROL> groupMenuControls)
        {
            List<string> listControlName = new List<string>();
            List<string> listType = new List<string>();
            for (int i = 0; i < userMenuControls.Count; i++)
            {
                string controlName = userMenuControls[i].CONTROLNAME;
                string type = userMenuControls[i].TYPE;
                if (controlName.Length > 0 && !listControlName.Contains(controlName))
                {
                    listControlName.Add(controlName);
                    listType.Add(type);
                }
            }
            for (int i = 0; i < groupMenuControls.Count; i++)
            {
                string controlName = (string)groupMenuControls[i].CONTROLNAME;
                string type = groupMenuControls[i].TYPE;
                if (controlName.Length > 0 && !listControlName.Contains(controlName))
                {
                    listControlName.Add(controlName);
                    listType.Add(type);
                }
            }
            for (int i = 0; i < listControlName.Count; i++)
            {
                DataRow dr = securityTable.NewRow();
                dr["ControlName"] = listControlName[i];
                dr["Type"] = listType[i];
                for (int j = 2; j < securityTable.Columns.Count; j++)
                {
                    dr[j] = ComputerValue(userMenuControls, groupMenuControls, listControlName[i], securityTable.Columns[j].ColumnName);
                }
                securityTable.Rows.Add(dr);
            }
        }

        private bool ComputerValue(List<USERMENUCONTROL> userMenuControls, List<GROUPMENUCONTROL> groupMenuControls, string controlName, string propertyName)
        {
            string value = string.Empty;
            if (Priority == PriorityName.Group)
            {
                value = GetValue(groupMenuControls, controlName, propertyName);
                if (value.Length == 0)
                {
                    value = GetValue(userMenuControls, controlName, propertyName);
                }
            }
            else if (Priority == PriorityName.User)
            {
                value = GetValue(userMenuControls, controlName, propertyName);
                if (value.Length == 0)
                {
                    value = GetValue(groupMenuControls, controlName, propertyName);
                }
            }
            return string.Compare(value, "y", true) == 0;
        }

        private string GetValue(List<USERMENUCONTROL> userMenuControls, string controlName, string propertyName)
        {
            string returnValue = string.Empty;
            List<USERMENUCONTROL> umc = userMenuControls.Where(u => u.CONTROLNAME.Equals(controlName)).ToList();
            if (RelaxMode)
            {
                if (umc.Count == 0)
                    return String.Empty;
                foreach (var item in umc)
                {
                    var value = item.GetType().GetProperty(propertyName.ToUpper()).GetValue(item, null);
                    if (value == null || value.ToString().Trim() == String.Empty)
                        return "Y";
                    if (value != null && value.ToString().ToUpper() == "Y")
                        return "Y";
                    else
                        returnValue = value.ToString();
                }
            }
            else
            {
                if (umc.Count == 0)
                    return String.Empty;
                foreach (var item in umc)
                {
                    var value = item.GetType().GetProperty(propertyName.ToUpper()).GetValue(item, null);
                    if (value != null && value.ToString().ToUpper() == "N")
                        return "N";
                    else
                        returnValue = value.ToString();
                }
            }
            return returnValue;
        }

        private string GetValue(List<GROUPMENUCONTROL> groupMenuControls, string controlName, string propertyName)
        {
            string returnValue = string.Empty;
            List<GROUPMENUCONTROL> gmc = groupMenuControls.Where(g => g.CONTROLNAME.Equals(controlName)).ToList();
            if (RelaxMode)
            {
                if (gmc.Count == 0)
                    return String.Empty;
                foreach (var item in gmc)
                {
                    var value = item.GetType().GetProperty(propertyName.ToUpper()).GetValue(item, null);
                    if (value == null || value.ToString().Trim() == String.Empty)
                        return "Y";
                    if (value != null && value.ToString().ToUpper() == "Y")
                        return "Y";
                    else
                        returnValue = value.ToString();
                }
            }
            else
            {
                if (gmc.Count == 0)
                    return String.Empty;
                foreach (var item in gmc)
                {
                    var value = item.GetType().GetProperty(propertyName.ToUpper()).GetValue(item, null);
                    if (value != null && value.ToString().ToUpper() == "N")
                        return "N";
                    else
                        returnValue = value.ToString();
                }
            }
            return returnValue;
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
                            if (ctrl is JQSecColumns)
                            {
                                (ctrl as JQSecColumns).ReadOnly = !(bool)rows[0]["Enabled"];
                                (ctrl as JQSecColumns).Visible = (bool)rows[0]["Visible"];
                                (ctrl as JQSecColumns).Do();
                            }
                        }
                        rows = table.Select(string.Format("ControlName like '%{0}'", ctrl.ID));
                        if (rows.Length > 0)
                        {
                            if (ctrl is JQDataGrid)
                            {
                                foreach (JQToolItem item in (ctrl as JQDataGrid).TooItems)
                                {
                                    if (item.Icon == "icon-add" || item.Icon == "icon-edit" || item.Icon == "icon-remove"
                                        || item.Icon == "icon-save" || item.Icon == "icon-undo")
                                        continue;

                                    DataRow[] rows1 = table.Select(string.Format("ControlName='{0}'", item.Text + "_" + ctrl.ID));
                                    if (rows1.Length > 0)
                                    {
                                        item.Visible = (bool)rows1[0]["Visible"];
                                        item.Enabled = (bool)rows1[0]["Enabled"];
                                    }
                                }

                                var editDialog = (ctrl as JQDataGrid).EditDialogID;
                                if (!string.IsNullOrEmpty(editDialog))
                                {
                                    var dialog = parent.FindControl(editDialog) as JQDialog;
                                    if (dialog != null)
                                    {
                                        var bindingObject = dialog.BindingObjectID;
                                        if (!string.IsNullOrEmpty(bindingObject))
                                        {
                                            var dataForm = dialog.FindControl(bindingObject) as JQDataForm;
                                            if (dataForm != null && dataForm.AlwaysReadOnly == true)
                                            {
                                                foreach (var item in dataForm.TooItems)
                                                {
                                                    if (item.OnClick == "insert")
                                                    {
                                                        item.Visible = (ctrl as JQDataGrid).AllowAdd;
                                                    }
                                                    if (item.OnClick == "update")
                                                    {
                                                        item.Visible = (ctrl as JQDataGrid).AllowUpdate;
                                                    }
                                                    if (item.OnClick == "remove")
                                                    {
                                                        item.Visible = (ctrl as JQDataGrid).AllowDelete;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
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
                    if (!CheckMenuRightsByName(this.Page.AppRelativeVirtualPath))
                    {
                        String messageKey = "Web/AccessDenied/accessdenied";
                        var provider = new JQMessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo, EFClientTools.ClientUtility.ClientInfo.Locale);
                        //EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Page.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                        String msg = provider[messageKey];
                        String strRenderScript = String.Format("alert('{0}');", msg);
                        //strRenderScript += String.Format("location.href='{0}/LogOn.aspx';", Page.Request.ApplicationPath);
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), strRenderScript, true);
                        System.Web.HttpContext.Current.Session.Abandon();
                        this.Page.Response.Redirect("../LogOn.aspx");
                        //Page.Response.Redirect(string.Format("", ));
                    }
                }
                if (!string.IsNullOrEmpty(EFClientTools.ClientUtility.ClientInfo.UserID))
                {
                    SetControl(this.Parent);
                    RenderJS();
                }
            }
        }

        private bool CheckMenuRightsByName(String fileName)
        {
            var client = EFClientTools.ClientUtility.Client;
            //form = System.Text.RegularExpressions.Regex.Replace(arr[arr.Length - 1], ".aspx".Replace(".", @"\."), string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var clientInfo = EFClientTools.ClientUtility.ClientInfo;
            var userPara2 = clientInfo.UserPara2;
            clientInfo.UserPara2 = "forCheckMenuRights";
            var menus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => String.IsNullOrEmpty(m.PARENT)).ToList();
            //var menus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => m.PACKAGE == package && m.FORM == form).ToList();
            clientInfo.UserPara2 = userPara2;

            return CheckMenuRights(fileName, menus);
        }

        private bool CheckMenuRights(String fileName, List<EFClientTools.EFServerReference.MENUTABLE> menus)
        {
            string package = string.Empty;
            string form = string.Empty;
            string[] arr = fileName.Split("\\/".ToCharArray());
            package = arr[arr.Length - 2];
            form = arr[arr.Length - 1].Replace(".aspx", string.Empty);
            bool flag = false;
            foreach (EFClientTools.EFServerReference.MENUTABLE menu in menus)
            {
                if (menu.PACKAGE == package && menu.FORM == form)
                {
                    return flag = true;
                }
                else if (menu.MENUTABLE1.Count > 0)
                {
                    flag = CheckMenuRights(fileName, menu.MENUTABLE1);
                    if (flag)
                        return flag;
                }
            }
            return flag;
        }

        private void RenderJS()
        {
            StringBuilder renderScript = new StringBuilder();
            DataRow[] jsTable = SecurityTable.Select("Type='HTML'");
            foreach (DataRow row in jsTable)
            {
                String visible = "inline";
                if (!(bool)row["Visible"])
                    visible = "none";
                renderScript.AppendFormat("if($('#{0}') != null)", row["ControlName"]);
                renderScript.Append("{");
                renderScript.AppendFormat("$('#{0}').css('display', '{1}');", row["ControlName"], visible);
                renderScript.Append("}");
            }
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), renderScript.ToString(), true);
        }

        private string _DBAlias;
        [Category("Infolight"),
        Description("Specifies the database to store the data of security")]
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

        private string _MenuID;
        [Category("Infolight"),
        Description("Specifies the menuID which the WebSecurity is applied to"),
        Editor(typeof(JQGetMenuID), typeof(UITypeEditor))]
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

        //private WebExportControls _webExportControls;
        //[Category("Infolight"),
        //Description("Specifies the controls which WebSecurity is applied to")]
        //[PersistenceMode(PersistenceMode.InnerProperty),
        // DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        // TypeConverter(typeof(CollectionConverter)),
        // NotifyParentProperty(true)]
        //public WebExportControls webExportControls
        //{
        //    get
        //    {
        //        return _webExportControls;
        //    }
        //}

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

    public enum PriorityName
    {
        Group = 0,
        User = 1
    }

    //public class WebExportControl : InfoOwnerCollectionItem
    //{
    //    private string _Name = "";
    //    override public string Name
    //    {
    //        get
    //        {
    //            if (ControlName != null)
    //                return this.ControlName;
    //            else
    //                return _Name;
    //        }
    //        set
    //        {
    //            _Name = value;
    //        }
    //    }

    //    private string _ControlName;
    //    [Editor(typeof(WebGetControlName), typeof(UITypeEditor))]
    //    public string ControlName
    //    {
    //        get
    //        {
    //            return _ControlName;
    //        }
    //        set
    //        {
    //            _ControlName = value;
    //        }
    //    }

    //    private string _Description;
    //    public string Description
    //    {
    //        get
    //        {
    //            return _Description;
    //        }
    //        set
    //        {

    //            _Description = value;
    //        }
    //    }

    //    private string _Type;
    //    public string Type
    //    {
    //        get
    //        {
    //            return _Type;
    //        }
    //        set
    //        {

    //            _Type = value;
    //        }
    //    }
    //}

    //public class WebExportControls : InfoOwnerCollection
    //{
    //    public WebExportControls() { }

    //    public WebExportControls(object aOwner, Type aItemType)
    //        : base(aOwner, typeof(WebExportControl))
    //    {

    //    }

    //    new public WebExportControl this[int index]
    //    {
    //        get
    //        {
    //            return (WebExportControl)InnerList[index];
    //        }
    //        set
    //        {
    //            if (index > -1 && index < Count)
    //                if (value is WebExportControl)
    //                {
    //                    //原来的Collection设置为0
    //                    ((WebExportControl)InnerList[index]).Collection = null;
    //                    InnerList[index] = value;
    //                    //Collection设置为this
    //                    ((WebExportControl)InnerList[index]).Collection = this;
    //                }
    //        }
    //    }
    //}

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
                if (context.Instance is JQSecurity)
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
                else if (context.Instance is JQMetro)
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

    public class JQGetControlName : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public JQGetControlName()
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
                //WebExportControl ec = context.Instance as WebExportControl;
                //if (ec != null)
                //{
                //    JQSecurity jqs = ((JQSecurity)(ec.Owner));
                //}

                //object[] param = new object[1];
                //param[0] = ((JQSecurity)(ec.Owner)).MenuID;
                //CliUtils.fLoginDB = ((JQSecurity)(ec.Owner)).DBAlias;
                //object[] myRet = CliUtils.CallMethod("GLModule", "GetMenu", param);
                //CliUtils.fLoginDB = "";
                //ControlName.SelectedIndexChanged += delegate(object sender, EventArgs e)
                //{
                //    int index = ControlName.SelectedIndex;
                //    if (index != -1)
                //    {
                //        if (index == 0)
                //        {
                //            value = "";
                //        }
                //        else
                //        {
                //            value = ControlName.Items[index].ToString();
                //            if ((myRet != null) && (0 == (int)myRet[0]))
                //            {
                //                //List<string> listControlName = (List<string>)myRet[1];
                //                //List<string> listDescription = (List<string>)myRet[2];
                //                ArrayList listControlName = (ArrayList)myRet[1];
                //                ArrayList listDescription = (ArrayList)myRet[2];
                //                for (int i = 0; i < listControlName.Count; i++)
                //                {
                //                    if (listControlName[i].ToString() == value.ToString())
                //                        ec.Description = listDescription[i].ToString();
                //                }
                //            }
                //        }
                //    }
                //    EditorService.CloseDropDown();

                //};

                EditorService.DropDownControl(ControlName);

            }

            return value;
        }
    }

    class JQSecurityEdit : DataSourceDesigner
    {
        private JQSecurityForm Export = null;
        private DesignerActionListCollection _actionLists;

        public JQSecurityEdit()
        {
            DesignerVerb ExportVerb = new DesignerVerb("Export", new EventHandler(OnExport));
            this.Verbs.Add(ExportVerb);
        }

        public void OnExport(object sender, EventArgs e)
        {
            JQSecurity ws = (JQSecurity)this.Component;
            if (ws.DBAlias == null || ws.DBAlias == "" || ws.DBAlias == "(none)")
            {
                System.Windows.Forms.MessageBox.Show("Please set DBAlias and MenuID first.");
            }
            else if (Export == null)
            {
                Export = new JQSecurityForm(this.Component as JQSecurity, this.GetService(typeof(IDesignerHost)) as IDesignerHost);
                Export.ShowDialog();
            }
            Export = null;
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                _actionLists = base.ActionLists;

                if (_actionLists != null)
                    _actionLists.Add(new JQSecurityActionList(this.Component));

                return _actionLists;
            }
        }
    }

    public class JQSecurityActionList : DesignerActionList
    {
        private JQSecurity ws;
        private JQSecurityForm Export = null;

        public JQSecurityActionList(IComponent component)
            : base(component)
        {
            ws = component as JQSecurity;
        }

        public void OnExport()
        {
            if (ws.DBAlias == null || ws.DBAlias == "" || ws.DBAlias == "(none)")
            {
                System.Windows.Forms.MessageBox.Show("Please set DBAlias and MenuID first.");
            }
            else if (Export == null)
            {
                Export = new JQSecurityForm(ws, this.GetService(typeof(IDesignerHost)) as IDesignerHost);
                Export.ShowDialog();
            }
            Export = null;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "OnExport", "Export", "UsePreview", true));
            return items;
        }
    }

}