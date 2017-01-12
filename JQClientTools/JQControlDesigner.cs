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
using System.Web.UI.Design.WebControls;

namespace JQClientTools
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
                        aJQGridColumn.Width = 90;
                        var coldef = columnDefinations.Where(c => c.FIELD_NAME == column).FirstOrDefault();
                        if (coldef != null)
                        {
                            if (!String.IsNullOrEmpty(coldef.CAPTION))
                                aJQGridColumn.Caption = coldef.CAPTION;
                            if (coldef.FIELD_LENGTH.HasValue)
                            {
                                aJQGridColumn.MaxLength = (int)coldef.FIELD_LENGTH;
                            }
                            switch (columnsType[column])
                            {
                                case "String":
                                    if (aJQGridColumn.MaxLength > 0)
                                    {
                                        aJQGridColumn.Width = aJQGridColumn.MaxLength * 2;
                                    }
                                    else if (columnsLength[column] > 0)
                                    {
                                        aJQGridColumn.Width = columnsLength[column] * 2;
                                    }
                                    break;
                            }

                            //aJQGridColumn.Width = 90;
                            switch (coldef.NEEDBOX)
                            {
                                case "ComboBox":
                                    aJQGridColumn.Editor = "infocombobox";
                                    break;
                                case "ComboGrid":
                                    aJQGridColumn.Editor = "infocombogrid";
                                    break;
                                case "RefValBox":
                                    aJQGridColumn.Editor = "inforefval";
                                    break;
                                case "CheckBox":
                                    aJQGridColumn.Editor = "checkbox";
                                    break;
                                case "DateTimeBox":
                                    aJQGridColumn.Editor = "datebox";
                                    break;
                                case "NumberBox":
                                    aJQGridColumn.Editor = "numberbox";
                                    break;
                                default:
                                    aJQGridColumn.Editor = "text";
                                    break;
                            }
                        }
                        dataGrid.Columns.Add(aJQGridColumn);
                    }

                    dataGrid.TooItems.Clear();
                    dataGrid.TooItems.Add(JQToolItem.InsertItem);
                    dataGrid.TooItems.Add(JQToolItem.UpdateItem);
                    dataGrid.TooItems.Add(JQToolItem.DeleteItem);
                    if (commandName == DataMember)
                    {
                        dataGrid.TooItems.Add(JQToolItem.ApplyItem);
                        dataGrid.TooItems.Add(JQToolItem.CancelItem);
                        dataGrid.TooItems.Add(JQToolItem.QueryItem);
                        dataGrid.TooItems.Add(JQToolItem.ExportItem);
                    }
                    else
                    {
                        dataGrid.AutoApply = false;
                        dataGrid.Pagination = false;
                    }
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
            var dataGrid = this.Component as JQDataGrid;
            var columns = new JQCollection<JQGridColumn>(dataGrid);
            foreach (var column in dataGrid.Columns)
            {
                columns.Add(column.Clone() as JQGridColumn);
            }


            editor.GetType().GetField("currentContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(editor, context);

            var form = (System.Windows.Forms.Form)editor.GetType().GetMethod("CreateCollectionForm", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(editor, null);
            form.GetType().GetProperty("EditValue").SetValue(form, columns, null);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                componentChangeService.OnComponentChanging(dataGrid, context.PropertyDescriptor);
                dataGrid.Columns.Clear();
                foreach (var column in columns)
                {
                    dataGrid.Columns.Add(column);
                }
                componentChangeService.OnComponentChanged(dataGrid, context.PropertyDescriptor, null, null);
            }
        }

        public void AddWorkflowIcons()
        {
            if (this.Component is JQDataGrid)
            {
                var dataGrid = this.Component as JQDataGrid;
                if (dataGrid.Columns.Count > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("Would you like to add workflow icons? "
                        , "Confirm", System.Windows.Forms.MessageBoxButtons.YesNo
                        , System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }

                var componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                componentChangeService.OnComponentChanging(dataGrid, null);
                
                JQToolItem jqtiSubmit = new JQToolItem();
                jqtiSubmit.Text = "Submit";
                jqtiSubmit.OnClick = "doSubmit('winSubmit');";
                jqtiSubmit.ItemType = "easyui-linkbutton";
                dataGrid.TooItems.Add(jqtiSubmit);

                JQToolItem jqtiApprove = new JQToolItem();
                jqtiApprove.Text = "Approve";
                jqtiApprove.OnClick = "doApprove('winApprove');";
                jqtiApprove.ItemType = "easyui-linkbutton";
                dataGrid.TooItems.Add(jqtiApprove);

                JQToolItem jqtiReturn = new JQToolItem();
                jqtiReturn.Text = "Return";
                jqtiReturn.OnClick = "doReturn('winReturn');";
                jqtiReturn.ItemType = "easyui-linkbutton";
                dataGrid.TooItems.Add(jqtiReturn);

                JQToolItem jqtiReject = new JQToolItem();
                jqtiReject.Text = "Reject";
                jqtiReject.OnClick = "doReject(dataGrid);";
                jqtiReject.ItemType = "easyui-linkbutton";
                dataGrid.TooItems.Add(jqtiReject);

                JQToolItem jqtiNotify = new JQToolItem();
                jqtiNotify.Text = "Notify";
                jqtiNotify.OnClick = "doNotify('winNotify');";
                jqtiNotify.ItemType = "easyui-linkbutton";
                dataGrid.TooItems.Add(jqtiNotify);

                JQToolItem jqtiFlowDelete = new JQToolItem();
                jqtiFlowDelete.Text = "FlowDelete";
                jqtiFlowDelete.OnClick = "doFlowDelete(dataGrid);";
                jqtiFlowDelete.ItemType = "easyui-linkbutton";
                dataGrid.TooItems.Add(jqtiFlowDelete);

                JQToolItem jqtiPlus = new JQToolItem();
                jqtiPlus.Text = "Plus";
                jqtiPlus.OnClick = "doPlus('winPlus');";
                jqtiPlus.ItemType = "easyui-linkbutton";
                dataGrid.TooItems.Add(jqtiPlus);

                JQToolItem jqtiPause = new JQToolItem();
                jqtiPause.Text = "Pause";
                jqtiPause.OnClick = "doPause(dataGrid);";
                jqtiPause.ItemType = "easyui-linkbutton";
                dataGrid.TooItems.Add(jqtiPause);

                JQToolItem jqtiComment = new JQToolItem();
                jqtiComment.Text = "Comment";
                jqtiComment.OnClick = "doComment('winComment');";
                jqtiComment.ItemType = "easyui-linkbutton";
                dataGrid.TooItems.Add(jqtiComment);
                componentChangeService.OnComponentChanged(dataGrid, null, null, null);
                System.Windows.Forms.MessageBox.Show("Add Workflow Icons succeed.", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionPropertyItem("RemoteName", "RemoteName"));
            items.Add(new DesignerActionPropertyItem("DataMember", "DataMember"));
            items.Add(new DesignerActionMethodItem(this, "RefreshSchema", "Refresh Schema", true));
            items.Add(new DesignerActionMethodItem(this, "EditColumns", "Edit Columns", true));
            //items.Add(new DesignerActionMethodItem(this, "AddWorkflowIcons", "Add Workflow Icons", true));
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

            var dataForm = this.Component as JQDataForm;
            var columns = new JQCollection<JQFormColumn>(dataForm);
            foreach (var column in dataForm.Columns)
            {
                columns.Add(column.Clone() as JQFormColumn);
            }
            editor.GetType().GetField("currentContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(editor, context);
            var form = (System.Windows.Forms.Form)editor.GetType().GetMethod("CreateCollectionForm", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(editor, null);
            form.GetType().GetProperty("EditValue").SetValue(form, columns, null);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                componentChangeService.OnComponentChanging(dataForm, context.PropertyDescriptor);
                dataForm.Columns.Clear();
                foreach (var column in columns)
                {
                    dataForm.Columns.Add(column);
                }
                componentChangeService.OnComponentChanged(dataForm, context.PropertyDescriptor, null, null);
            }
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
                            if (!String.IsNullOrEmpty(coldef.CAPTION))
                                aJQFormColumn.Caption = coldef.CAPTION;
                            if (coldef.FIELD_LENGTH.HasValue)
                                aJQFormColumn.MaxLength = (int)coldef.FIELD_LENGTH;
                            aJQFormColumn.Width = 180;
                            switch (coldef.NEEDBOX)
                            {
                                case "ComboBox":
                                    aJQFormColumn.Editor = "infocombobox";
                                    break;
                                case "ComboGrid":
                                    aJQFormColumn.Editor = "infocombogrid";
                                    break;
                                case "RefValBox":
                                    aJQFormColumn.Editor = "inforefval";
                                    break;
                                case "CheckBox":
                                    aJQFormColumn.Editor = "checkbox";
                                    break;
                                case "DateTimeBox":
                                    aJQFormColumn.Editor = "datebox";
                                    break;
                                case "NumberBox":
                                    aJQFormColumn.Editor = "numberbox";
                                    break;
                                default:
                                    aJQFormColumn.Editor = "text";
                                    break;
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

            var jqDefault = this.Component as JQDefault;
            var columns = new JQCollection<JQDefaultColumn>(jqDefault);
            foreach (var column in jqDefault.Columns)
            {
                columns.Add(column.Clone() as JQDefaultColumn);
            }

            editor.GetType().GetField("currentContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(editor, context);
            var form = (System.Windows.Forms.Form)editor.GetType().GetMethod("CreateCollectionForm", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(editor, null);
            form.GetType().GetProperty("EditValue").SetValue(form, columns, null);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                componentChangeService.OnComponentChanging(jqDefault, context.PropertyDescriptor);
                jqDefault.Columns.Clear();
                foreach (var column in columns)
                {
                    jqDefault.Columns.Add(column);
                }
                componentChangeService.OnComponentChanged(jqDefault, context.PropertyDescriptor, null, null);
            }
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

            var jqValidate = this.Component as JQValidate;
            var columns = new JQCollection<JQValidateColumn>(jqValidate);
            foreach (var column in jqValidate.Columns)
            {
                columns.Add(column.Clone() as JQValidateColumn);
            }

            editor.GetType().GetField("currentContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(editor, context);
            var form = (System.Windows.Forms.Form)editor.GetType().GetMethod("CreateCollectionForm", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(editor, null);
            form.GetType().GetProperty("EditValue").SetValue(form, columns, null);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                componentChangeService.OnComponentChanging(jqValidate, context.PropertyDescriptor);
                jqValidate.Columns.Clear();
                foreach (var column in columns)
                {
                    jqValidate.Columns.Add(column);
                }
                componentChangeService.OnComponentChanged(jqValidate, context.PropertyDescriptor, null, null);
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "EditColumns", "Edit Columns", true));
            return items;
        }
    }

    public class JQTreeViewDesigner : ControlDesigner
    {
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                var actionList = base.ActionLists;
                actionList.Add(new JQTreeViewDesignerActionList(this.Component));
                return actionList;
            }
        }
    }

    public class JQTreeViewDesignerActionList : DesignerActionList, IJQDataSourceProvider
    {
        public JQTreeViewDesignerActionList(IComponent component)
            : base(component)
        { }

        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName
        {
            get
            {
                return (this.Component as JQTreeView).RemoteName;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(JQTreeView))["RemoteName"]
                 .SetValue(this.Component, value);
            }
        }

        [Editor(typeof(DataMemberEditor), typeof(UITypeEditor))]
        public string DataMember
        {
            get
            {
                return (this.Component as JQTreeView).DataMember;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(JQTreeView))["DataMember"]
                 .SetValue(this.Component, value);
            }
        }

        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string idField
        {
            get
            {
                return (this.Component as JQTreeView).idField;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(JQTreeView))["idField"]
                 .SetValue(this.Component, value);
            }
        }


        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string textField
        {
            get
            {
                return (this.Component as JQTreeView).textField;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(JQTreeView))["textField"]
                 .SetValue(this.Component, value);
            }
        }


        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string parentField
        {
            get
            {
                return (this.Component as JQTreeView).parentField;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(JQTreeView))["parentField"]
                 .SetValue(this.Component, value);
            }
        }
        public string RootValue
        {
            get
            {
                return (this.Component as JQTreeView).RootValue;
            }
            set
            {
                TypeDescriptor.GetProperties(typeof(JQTreeView))["RootValue"]
                 .SetValue(this.Component, value);
            }
        }
        public void CreateSimplifiedColumns()
        {
            if (this.Component is JQTreeView)
            {
                if (string.IsNullOrEmpty(RemoteName) || string.IsNullOrEmpty(DataMember) || string.IsNullOrEmpty(idField) || string.IsNullOrEmpty(textField) || string.IsNullOrEmpty(parentField))
                {
                    return;
                }

                var treeview = this.Component as JQTreeView;
                if (treeview.Columns.Count > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("Would you like to regenerate the treeview columns ? Warning: this will delete all existing columns"
                        , "Confirm", System.Windows.Forms.MessageBoxButtons.YesNo
                        , System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }

                try
                {
                    var componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                    componentChangeService.OnComponentChanging(treeview, null);
                    var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                    clientInfo.UseDataSet = true;
                    var assemblyName = RemoteName.Split('.')[0];
                    var commandName = RemoteName.Split('.')[1];
                    var columnDefinations = EFClientTools.DesignClientUtility.Client.GetColumnDefination(clientInfo, assemblyName, DataMember, null)
                        .OfType<EFClientTools.EFServerReference.COLDEF>();
                    
                    treeview.Columns.Clear();
                    JQTreeViewColumn idcolumn = new JQTreeViewColumn();
                    idcolumn.FieldName = idField;
                    var coldef = columnDefinations.Where(c => c.FIELD_NAME == idcolumn.FieldName).FirstOrDefault();
                    if (coldef != null && !string.IsNullOrEmpty(coldef.CAPTION))
                        idcolumn.Caption = coldef.CAPTION;
                    else
                        idcolumn.Caption = idField;
                    idcolumn.NewLine = true;
                    treeview.Columns.Add(idcolumn);
                    JQTreeViewColumn captioncolumn = new JQTreeViewColumn();
                    captioncolumn.FieldName = textField;
                    var coldef2 = columnDefinations.Where(c => c.FIELD_NAME == captioncolumn.FieldName).FirstOrDefault();
                    if (coldef2 != null && !string.IsNullOrEmpty(coldef2.CAPTION))
                        captioncolumn.Caption = coldef2.CAPTION;
                    else
                        captioncolumn.Caption = textField;
                    captioncolumn.NewLine = true;
                    treeview.Columns.Add(captioncolumn);

                    componentChangeService.OnComponentChanged(treeview, null, null, null);
                    System.Windows.Forms.MessageBox.Show("Create TreeView Columns succeed.", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }


        public void CreateSimplifiedMenus()
        {
            if (this.Component is JQTreeView)
            {
                if (string.IsNullOrEmpty(RemoteName) || string.IsNullOrEmpty(DataMember) || string.IsNullOrEmpty(idField) || string.IsNullOrEmpty(textField) || string.IsNullOrEmpty(parentField))
                {
                    return;
                }

                var treeview = this.Component as JQTreeView;
                if (treeview.Menutems.Count > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("Would you like to regenerate the menus ? Warning: this will delete all existing menus"
                        , "Confirm", System.Windows.Forms.MessageBoxButtons.YesNo
                        , System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }

                try
                {
                    var componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                    componentChangeService.OnComponentChanging(treeview, null);

                    treeview.Menutems.Clear();
                    treeview.Menutems.Add(JQTreeViewContextItem.InsertContextItem);
                    treeview.Menutems.Add(JQTreeViewContextItem.UpdateContextItem);
                    treeview.Menutems.Add(JQTreeViewContextItem.DeleteContextItem);

                    componentChangeService.OnComponentChanged(treeview, null, null, null);
                    System.Windows.Forms.MessageBox.Show("Create menus succeed.", "Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
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
            items.Add(new DesignerActionPropertyItem("idField", "idField"));
            items.Add(new DesignerActionPropertyItem("textField", "textField"));
            items.Add(new DesignerActionPropertyItem("parentField", "parentField"));
            items.Add(new DesignerActionPropertyItem("RootValue", "RootValue"));
            items.Add(new DesignerActionMethodItem(this, "CreateSimplifiedColumns", "Create Columns", true));
            items.Add(new DesignerActionMethodItem(this, "CreateSimplifiedMenus", "Create Menus", true));
            return items;
        }
    }

    public class JQTabDesigner : MultiViewDesigner
    {
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                var actionList = base.ActionLists;
                actionList.Add(new JQTabDesignerActionList(this.Component));
                return actionList;
            }
        }
    }

    public class JQTabDesignerActionList : DesignerActionList
    {
        public JQTabDesignerActionList(IComponent component)
            : base(component)
        { }

        public void AddTabItem()
        {
            var tab = this.Component as JQTab;
            var componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
            componentChangeService.OnComponentChanging(tab, null);
            tab.Views.Add(new JQTabItem());
            componentChangeService.OnComponentChanged(tab, null, null, null);
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "AddTabItem", "Add Tab Item", true));
            return items;
        }
    }



    public class JQTypeDescriptorContext : ITypeDescriptorContext
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
