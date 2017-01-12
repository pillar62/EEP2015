using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Windows.Forms.Design;
using System.Xml;
using Microsoft.Win32;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Reflection;
using System.Drawing;
#if UseCrystalReportDD
using CrystalDecisions.Windows.Forms;
#endif

namespace Srvtools
{
    [ToolboxItem(true)]
    [Designer(typeof(InfoSecurityEdit), typeof(IDesigner))]
    [ToolboxBitmap(typeof(InfoSecurity), "Resources.InfoSecurity.png")]
    public partial class InfoSecurity : InfoBaseComp, ISupportInitialize
    {
        public InfoSecurity(IContainer container)
        {
            container.Add(this);
            _ExportControls = new ExportControls(this, typeof(ExportControl));

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
                    string parameter = null;
                    if (this.OwnerComp is InfoForm)
                    {
                        parameter = (this.OwnerComp as InfoForm).ItemParamters;
                    }

                    string menuid = CliUtils.GetMenuID(this.OwnerComp.GetType().FullName, parameter);
                    if (string.IsNullOrEmpty(menuid))
                    {
                        menuid = this.MenuID;
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

            //for (int i = 0; i < tableUser.Rows.Count; i++)
            //{
            //    if (tableUser.Rows[i]["ControlName"] != DBNull.Value)
            //    {
            //        string controlName = (string)tableUser.Rows[i]["ControlName"];
            //        if (controlName.Length > 0)
            //        {
            //            DataRow[] drs = securityTable.Select(string.Format("ControlName='{0}'", controlName));
            //            if (drs.Length == 0)
            //            {
            //                DataRow dr = securityTable.NewRow();
            //                dr["ControlName"] = controlName;
            //                for (int j = 1; j < securityTable.Columns.Count; j++)
            //                {
            //                    dr[j] = ComputerValue(tableUser, tableGroup, controlName, securityTable.Columns[j].ColumnName);
            //                }
            //                securityTable.Rows.Add(dr);
            //            }
            //        }
            //    }
            //}
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
            else if(Priority == PriorityName.User)
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

        protected override void DoAfterSetOwner(IDataModule value)
        {

            ExecuteSecurity(value);
        }


        public void ExecuteSecurity(IDataModule value)
        {
            if (!DesignMode && this.Active)
            {
                DataTable table = SecurityTable;
                Form form = this.OwnerComp as Form;
                Type type = form.GetType();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string controlName = (string)table.Rows[i]["ControlName"];
                    FieldInfo field = type.GetField(controlName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    if (field == null)
                    {
                        field = type.GetField("_" + controlName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    }
                    if (field != null)
                    {
                        object obj = field.GetValue(form);
                        if (obj != null)
                        {
                            Type typeObj = obj.GetType();
                            for (int j = 1; j < table.Columns.Count; j++)
                            {
                                PropertyInfo property = typeObj.GetProperty(table.Columns[j].ColumnName);
                                if (property != null && property.PropertyType == typeof(bool))
                                {
                                    property.SetValue(obj, table.Rows[i][j], null);
                                }
                            }
                            //特别的
                            if(obj is InfoSecColumns)
                            {
                                (obj as InfoSecColumns).ReadOnly = !(bool)table.Rows[i]["Enabled"];
                            }
#if UseCrystalReportDD
                            else if(obj is CrystalReportViewer)
                            {
                                 (obj as CrystalReportViewer).ShowExportButton = (bool)table.Rows[i]["AllowPrint"];
                                 (obj as CrystalReportViewer).ShowPrintButton = (bool)table.Rows[i]["AllowPrint"];
                            }
#endif
                        }
                    }
                }
            }
        }

        private string _DBAlias;
        [Category("Infolight"),
        Description("Specifies the database to store the data of security")]
        [Editor(typeof(GetAlias), typeof(System.Drawing.Design.UITypeEditor))]
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
        Description("Specifies the menuID which the InfoSecurity is applied to")]
        [Editor(typeof(GetMenuID), typeof(UITypeEditor))]
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
        Description("Indicates whether InfoSecurity is enabled or disabled")]
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

        private ExportControls _ExportControls;
        [Category("Infolight"),
        Description("Specifies the controls which InfoSecurity is applied to")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ExportControls ExportControls
        {
            get
            {
                return _ExportControls;
            }
            set
            {
                _ExportControls = value;
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

        #region ISupportInitialize Members

        public void BeginInit() { }

        public void EndInit() { }

        #endregion
    }

    public enum PriorityName
    {
        Group = 0,
        User = 1
    }

    public class ExportControl : InfoOwnerCollectionItem
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
        [Editor(typeof(GetControlName), typeof(UITypeEditor))]
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

    public class ExportControls : InfoOwnerCollection
    {
        public ExportControls(object aOwner, Type aItemType)
            : base(aOwner, typeof(ExportControl))
        {

        }

        new public ExportControl this[int index]
        {
            get
            {
                return (ExportControl)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is ExportControl)
                    {
                        //原来的Collection设置为0
                        ((ExportControl)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((ExportControl)InnerList[index]).Collection = this;
                    }
            }
        }
    }

    public class GetAlias : System.Drawing.Design.UITypeEditor
    {
        public GetAlias()
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
                ListBox AliasList = new ListBox();
                AliasList.Items.Add("(none)");
                AliasList.SelectionMode = SelectionMode.One;
                InfoSecurity infoS = context.Instance as InfoSecurity;
                if (infoS != null)
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



    public class GetMenuID : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public GetMenuID()
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
                ListBox MenuIDs = new ListBox();
                ListBox MenuID = new ListBox();
                MenuIDs.SelectionMode = SelectionMode.One;
                MenuIDs.Items.Add("( None )");
                InfoSecurity infoS = context.Instance as InfoSecurity;
                if (infoS != null)
                {
                    object[] param = new object[1];
                    param[0] = infoS.DBAlias.ToString();
                    CliUtils.fLoginDB = infoS.DBAlias;
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

    public class GetControlName : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public GetControlName()
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
                ListBox ControlName = new ListBox();
                ControlName.SelectionMode = SelectionMode.One;
                ControlName.Items.Add("(none)");
                ExportControl ec = context.Instance as ExportControl;
                if (ec != null)
                {
                    IContainer ict = ((InfoSecurity)(ec.Owner)).Container;
                    for (int i = 0; i < ict.Components.Count; i++)
                    {
                        if (ict.Components[i] is InfoBindingSource)
                        {
                            string str = ict.Components[i].ToString();
                            string strName = str.Substring(0, str.IndexOf(' '));
                            ControlName.Items.Add(strName);
                        }
                        else if (ict.Components[i] is Panel)
                        {
                            string str = ict.Components[i].ToString();
                            string strName = str.Substring(0, str.IndexOf(' '));
                            ControlName.Items.Add(strName);
                        }
                        else if (ict.Components[i] is Button)
                        {
                            string str = ict.Components[i].ToString();
                            string strName = str.Substring(0, str.IndexOf(' '));
                            ControlName.Items.Add(strName);
                        }
                        else if (ict.Components[i] is ToolStripButton)
                        {
                            string str = ict.Components[i].ToString();
                            ControlName.Items.Add(str);
                        }
                    }
                }

                object[] param = new object[1];
                param[0] = ((InfoSecurity)(ec.Owner)).MenuID;
                CliUtils.fLoginDB = ((InfoSecurity)(ec.Owner)).DBAlias;
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