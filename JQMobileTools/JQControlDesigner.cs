using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing.Design;
using System.Data;
using System.Collections;
using System.IO;

namespace JQMobileTools
{
    public class JQDataGridDesigner : ControlDesigner
    {
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                var actionList = base.ActionLists;
                actionList.Add(new JQDataGridDesignerActionList(this.Component));
                return actionList;
            }
        }
    }

    public class JQDataGridDesignerActionList : DesignerActionList, IJQDataSourceProvider
    {
        public JQDataGridDesignerActionList(IComponent component)
            : base(component)
        { }

        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName
        {
            get
            {
                return (this.Component as JQDataGrid).RemoteName;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(JQDataGrid))["RemoteName"]
                 .SetValue(this.Component, value);
            }
        }

        [Editor(typeof(DataMemberEditor), typeof(UITypeEditor))]
        public string DataMember
        {
            get
            {
                return (this.Component as JQDataGrid).DataMember;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(JQDataGrid))["DataMember"]
                 .SetValue(this.Component, value);
            }
        }

        public void RefreshSchema()
        {
            if (this.Component is JQDataGrid)
            {
                if (string.IsNullOrEmpty(RemoteName) || string.IsNullOrEmpty(DataMember))
                {
                    return;
                }

                var dataGrid = this.Component as JQDataGrid;
                if (dataGrid.Columns.Count > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("Would you like to regenerate the gridview column fields and toolitems? Warning: this will delete all existing column fields"
                        , "Confirm", System.Windows.Forms.MessageBoxButtons.YesNo
                        , System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }
                var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                clientInfo.UseDataSet = true;
                try
                {
                    var assemblyName = RemoteName.Split('.')[0];
                    var commandName = RemoteName.Split('.')[1];
                    var columnsLength = EFClientTools.DesignClientUtility.Client.GetEntityFieldsLength(clientInfo, assemblyName, DataMember, null);
                    var columnsType = EFClientTools.DesignClientUtility.Client.GetEntityFieldTypes(clientInfo, assemblyName, DataMember, null);
                    var columns = EFClientTools.DesignClientUtility.Client.GetEntityFields(clientInfo, assemblyName, DataMember, null);
                    var componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                    componentChangeService.OnComponentChanging(dataGrid, null);
                    dataGrid.Columns.Clear();
                    var columnDefinations = EFClientTools.DesignClientUtility.Client.GetColumnDefination(clientInfo, assemblyName, DataMember, null)
                        .OfType<EFClientTools.EFServerReference.COLDEF>();
                    foreach (var column in columns)
                    {
                        JQGridColumn aJQGridColumn = new JQGridColumn();
                        aJQGridColumn.FieldName = column;
                        aJQGridColumn.Caption = column;
                        var coldef = columnDefinations.Where(c => c.FIELD_NAME == column).FirstOrDefault();
                        if (coldef != null)
                        {
                            if (!string.IsNullOrEmpty(coldef.CAPTION))
                            {
                                aJQGridColumn.Caption = coldef.CAPTION;
                            }
                        }

                        aJQGridColumn.Width = 90;
                        dataGrid.Columns.Add(aJQGridColumn);
                    }

                    dataGrid.ToolItems.Clear();
                    dataGrid.ToolItems.Add(JQToolItem.InsertItem);
                    dataGrid.ToolItems.Add(JQToolItem.PreviousPageItem);
                    dataGrid.ToolItems.Add(JQToolItem.NextPageItem);
                    dataGrid.ToolItems.Add(JQToolItem.QueryItem);
                    dataGrid.ToolItems.Add(JQToolItem.RefreshItem);
                    dataGrid.ToolItems.Add(JQToolItem.BackItem);
                    componentChangeService.OnComponentChanged(dataGrid, null, null, null);
                    System.Windows.Forms.MessageBox.Show("Refresh schema succeed.", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }

        public void EditColumns()
        {
            var type = (this.Component as JQDataGrid).Columns.GetType();

            var editor = (UITypeEditor)TypeDescriptor.GetEditor(type, typeof(UITypeEditor));

            var context = new JQTypeDescriptorContext()
            {
                ActionList = this,
                Component = this.Component,
                PropertyName = "Columns"
            };

            editor.GetType().GetField("currentContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(editor, context);

            var form = (System.Windows.Forms.Form)editor.GetType().GetMethod("CreateCollectionForm", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(editor, null);
            form.GetType().GetProperty("EditValue").SetValue(form, (this.Component as JQDataGrid).Columns, null);

            form.ShowDialog();
        }

        public void CreateStandardToolItems()
        {
            if (this.Component is JQDataGrid)
            {
                var dataGrid = this.Component as JQDataGrid;
                if(dataGrid.ToolItems.Count > 0)
                if (System.Windows.Forms.MessageBox.Show("Would you like to regenerate toolitems? Warning: this will delete all existing toolitems"
                        , "Confirm", System.Windows.Forms.MessageBoxButtons.YesNo
                        , System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                var componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                componentChangeService.OnComponentChanging(dataGrid, null);
                dataGrid.ToolItems.Clear();
                dataGrid.ToolItems.Add(JQToolItem.InsertItem);
                dataGrid.ToolItems.Add(JQToolItem.PreviousPageItem);
                dataGrid.ToolItems.Add(JQToolItem.NextPageItem);
                dataGrid.ToolItems.Add(JQToolItem.QueryItem);
                dataGrid.ToolItems.Add(JQToolItem.RefreshItem);
                dataGrid.ToolItems.Add(JQToolItem.BackItem);
                componentChangeService.OnComponentChanged(dataGrid, null, null, null);
                System.Windows.Forms.MessageBox.Show("Create standard toolitems succeed.", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
        }


        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionPropertyItem("RemoteName", "RemoteName"));
            items.Add(new DesignerActionPropertyItem("DataMember", "DataMember"));
            items.Add(new DesignerActionMethodItem(this, "RefreshSchema", "Refresh Schema", true));
            items.Add(new DesignerActionMethodItem(this, "EditColumns", "Edit Columns", true));
            items.Add(new DesignerActionMethodItem(this, "CreateStandardToolItems", "Create Standard ToolItems", true));
            return items;
        }

     
    }

    public class JQDataFormDesigner : ControlDesigner
    {
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                var actionList = base.ActionLists;
                actionList.Add(new JQDataFormDesignerActionList(this.Component));
                return actionList;
            }
        }
    }

    public class JQDataFormDesignerActionList : DesignerActionList, IJQDataSourceProvider
    {
        public JQDataFormDesignerActionList(IComponent component)
            : base(component)
        { }

        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName
        {
            get
            {
                return (this.Component as JQDataForm).RemoteName;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(JQDataForm))["RemoteName"]
                 .SetValue(this.Component, value);
            }
        }

        [Editor(typeof(DataMemberEditor), typeof(UITypeEditor))]
        public string DataMember
        {
            get
            {
                return (this.Component as JQDataForm).DataMember;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(JQDataForm))["DataMember"]
                 .SetValue(this.Component, value);
            }
        }

        public void EditColumns()
        {
            var type = (this.Component as JQDataForm).Columns.GetType();
            var editor = (UITypeEditor)TypeDescriptor.GetEditor(type, typeof(UITypeEditor));
            var context = new JQTypeDescriptorContext()
            {
                ActionList = this,
                Component = this.Component,
                PropertyName = "Columns"
            };

            editor.GetType().GetField("currentContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(editor, context);
            var form = (System.Windows.Forms.Form)editor.GetType().GetMethod("CreateCollectionForm", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(editor, null);
            form.GetType().GetProperty("EditValue").SetValue(form, (this.Component as JQDataForm).Columns, null);
            form.ShowDialog();
        }

        public void RefreshSchema()
        {
            if (this.Component is JQDataForm)
            {
                if (string.IsNullOrEmpty(RemoteName) || string.IsNullOrEmpty(DataMember))
                {
                    return;
                }

                var dataForm = this.Component as JQDataForm;
                if (dataForm.Columns.Count > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("Would you like to regenerate the gridview column fields and toolitems? Warning: this will delete all existing column fields"
                        , "Confirm", System.Windows.Forms.MessageBoxButtons.YesNo
                        , System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }
                var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                clientInfo.UseDataSet = true;
                try
                {
                    var assemblyName = RemoteName.Split('.')[0];
                    var commandName = RemoteName.Split('.')[1];
                    var columns = EFClientTools.DesignClientUtility.Client.GetEntityFields(clientInfo, assemblyName, DataMember, null);
                    var componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                    componentChangeService.OnComponentChanging(dataForm, null);
                    dataForm.Columns.Clear();
                    var columnDefinations = EFClientTools.DesignClientUtility.Client.GetColumnDefination(clientInfo, assemblyName, DataMember, null)
                                            .OfType<EFClientTools.EFServerReference.COLDEF>();
                    foreach (var column in columns)
                    {
                        JQFormColumn aJQFormColumn = new JQFormColumn();
                        aJQFormColumn.FieldName = column;
                        aJQFormColumn.Caption = column;
                        var coldef = columnDefinations.Where(c => c.FIELD_NAME == column).FirstOrDefault();
                        if (coldef != null)
                        {
                            if (!string.IsNullOrEmpty(coldef.CAPTION))
                            {
                                aJQFormColumn.Caption = coldef.CAPTION;
                            }
                        }
                        dataForm.Columns.Add(aJQFormColumn);
                    }

                    componentChangeService.OnComponentChanged(dataForm, null, null, null);
                    System.Windows.Forms.MessageBox.Show("Refresh schema succeed.", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionPropertyItem("RemoteName", "RemoteName"));
            items.Add(new DesignerActionPropertyItem("DataMember", "DataMember"));
            items.Add(new DesignerActionMethodItem(this, "RefreshSchema", "Refresh Schema", true));
            items.Add(new DesignerActionMethodItem(this, "EditColumns", "Edit Columns", true));
            return items;
        }
    }

    public class JQDefaultDesigner : ControlDesigner
    {
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                var actionList = base.ActionLists;
                actionList.Add(new JQDefaultDesignerActionList(this.Component));
                return actionList;
            }
        }
    }

    public class JQDefaultDesignerActionList : DesignerActionList
    {
        public JQDefaultDesignerActionList(IComponent component)
            : base(component)
        { }

        public void EditColumns()
        {
            var type = (this.Component as JQDefault).Columns.GetType();
            var editor = (UITypeEditor)TypeDescriptor.GetEditor(type, typeof(UITypeEditor));
            var context = new JQTypeDescriptorContext()
            {
                ActionList = this,
                Component = this.Component,
                PropertyName = "Columns"
            };

            editor.GetType().GetField("currentContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(editor, context);
            var form = (System.Windows.Forms.Form)editor.GetType().GetMethod("CreateCollectionForm", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(editor, null);
            form.GetType().GetProperty("EditValue").SetValue(form, (this.Component as JQDefault).Columns, null);
            form.ShowDialog();
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "EditColumns", "Edit Columns", true));
            return items;
        }
    }

    public class JQValidateDesigner : ControlDesigner
    {
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                var actionList = base.ActionLists;
                actionList.Add(new JQValidateDesignerActionList(this.Component));
                return actionList;
            }
        }
    }

    public class JQValidateDesignerActionList : DesignerActionList
    {
        public JQValidateDesignerActionList(IComponent component)
            : base(component)
        { }

        public void EditColumns()
        {
            var type = (this.Component as JQValidate).Columns.GetType();
            var editor = (UITypeEditor)TypeDescriptor.GetEditor(type, typeof(UITypeEditor));
            var context = new JQTypeDescriptorContext()
            {
                ActionList = this,
                Component = this.Component,
                PropertyName = "Columns"
            };

            editor.GetType().GetField("currentContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(editor, context);
            var form = (System.Windows.Forms.Form)editor.GetType().GetMethod("CreateCollectionForm", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(editor, null);
            form.GetType().GetProperty("EditValue").SetValue(form, (this.Component as JQValidate).Columns, null);
            form.ShowDialog();
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "EditColumns", "Edit Columns", true));
            return items;
        }
    }

    public class JQScriptManagerDesigner : ControlDesigner
    {
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                var actionList = base.ActionLists;
                actionList.Add(new JQScriptManagerActionList(this.Component));
                return actionList;
            }
        }
    }

    public class JQScriptManagerActionList : DesignerActionList
    {
        public JQScriptManagerActionList(IComponent component)
            : base(component)
        { }

        public void ExportToCordova()
        {
            var cordovaProject = EFBase.Design.DTE.GetProjectFullName("{262852C6-CD72-467D-83FE-5EEB1973A190}");
            if (!string.IsNullOrEmpty(cordovaProject))
            {
                var cordovaPath = System.IO.Path.GetDirectoryName(cordovaProject);
                var currentFile = EFBase.Design.DTE.ActiveDocumentFullName;

                var jsPath = Path.Combine(cordovaPath, "www", "js", "jquery.infolight.mobile.js");
                var virtualPath = string.Empty;
                if (System.IO.File.Exists(jsPath))
                {
                    using (var reader = new StreamReader(jsPath, true))
                    {
                        var line = reader.ReadLine();
                        if (line.StartsWith("var webSiteUrl ="))
                        {
                            virtualPath = line.Split('=')[1].Trim(" ';".ToArray()).Replace("http://", string.Empty);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(virtualPath))
                {
                    var folder = new DirectoryInfo(Path.GetDirectoryName(currentFile)).Name;
                    var page = Path.GetFileNameWithoutExtension(currentFile);

                    try
                    {
                        System.Net.WebClient client = new System.Net.WebClient();
                        client.Encoding = new UTF8Encoding();
                        client.Headers.Add("Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*");
                        client.Headers.Add("Accept-Language", "zh-cn");
                        client.Headers.Add("UA-CPU", "x86");
                        client.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");
                        var html = client.DownloadString(new Uri(string.Format("http://{0}/{1}/{2}.aspx?Cordova=true", virtualPath, folder, page)));

                        var targetDir = Path.Combine(cordovaPath, "www", folder);
                        if (!Directory.Exists(targetDir))
                        {
                            Directory.CreateDirectory(targetDir);
                        }
                        using (StreamWriter writer = new StreamWriter(Path.Combine(targetDir, string.Format("{0}.html", page)), false, new System.Text.UTF8Encoding(true)))
                        {
                            writer.WriteLine(html.Replace("&#39;", "'"));
                            writer.Flush();
                        }
                        System.Windows.Forms.MessageBox.Show("Export to Cordova successfully.", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Can not find virtual path config.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Can not find Cordova project.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "ExportToCordova", "Export To Cordova", true));
            return items;
        }
    }

    public class JQTypeDescriptorContext: ITypeDescriptorContext
    {
        public IComponent Component { get; set; }

        public DesignerActionList ActionList { get; set; }

        public string PropertyName { get; set; }


        #region ITypeDescriptorContext Members

        public IContainer Container
        {
            get { return Component.Site.Container; }
        }

        public object Instance
        {
            get { return Component; }
        }

        public void OnComponentChanged()
        {
            var componentChangeService = (IComponentChangeService)ActionList.GetService(typeof(IComponentChangeService));
            componentChangeService.OnComponentChanged(this.Component, null, null, null);
        }

        public bool OnComponentChanging()
        {
            var componentChangeService = (IComponentChangeService)ActionList.GetService(typeof(IComponentChangeService));
            componentChangeService.OnComponentChanging(this.Component, null);
            return true;
        }

        public PropertyDescriptor PropertyDescriptor
        {
            get { return TypeDescriptor.GetProperties(this.Component).Find(PropertyName, false); }
        }

        #endregion

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            return this.ActionList.GetService(serviceType);
        }

        #endregion
    }
}
