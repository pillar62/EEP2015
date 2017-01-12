using System;
using System.ComponentModel.Design;
using System.Data;
using System.Web.UI.Design;
using System.Windows.Forms;
using Srvtools;
using System.Collections.Generic;

namespace AjaxTools
{
    public class AjaxFormViewDesigner : ControlDesigner
    {
        IDesignerHost host = null;
        IComponentChangeService svcCompChange = null;
        AjaxFormView formView = null;

        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection verbs = new DesignerVerbCollection();
                verbs.Add(new DesignerVerb("Generate Fields...", new EventHandler(GenFields)));
                //verbs.Add(new DesignerVerb("Generate ToolItems...", new EventHandler(GenToolItems)));
                verbs.Add(new DesignerVerb("Preview", new EventHandler(Preview)));
                return verbs;
            }
        }

        public void GenFields(object sender, EventArgs e)
        {
            if (formView == null)
            {
                formView = this.Component as AjaxFormView;
            }
            if (formView != null && !string.IsNullOrEmpty(formView.DataSourceID))
            {
                if (formView.Fields.Count == 0)
                {
                    WebDataSource wds = formView.GetObjByID(formView.DataSourceID) as WebDataSource;
                    DataTable srcTable = GetDesignTable(wds);
                    if (srcTable != null)
                    {
                        DataTable ddTable = DBUtils.GetDataDictionary(wds, true).Tables[0];

                        bool genAlternateColumns = (MessageBox.Show("generate alternate columns", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
                        if (host == null)
                        {
                            host = (IDesignerHost)GetService(typeof(IDesignerHost));
                        }
                        if (svcCompChange == null)
                        {
                            svcCompChange = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                        }
                        DesignerTransaction trans = host.CreateTransaction("Generate Fields");

                        for (int i = 0; i < srcTable.Columns.Count; i++)
                        {
                            AjaxFormField field = new AjaxFormField();
                            field.FieldControlId = string.Format("ctrl{0}", srcTable.Columns[i].ColumnName);
                            field.Caption = Caption(srcTable.Columns[i].ColumnName, ddTable);
                            field.IsKeyField = IsKeyField(srcTable.Columns[i].ColumnName, srcTable.PrimaryKey);
                            field.DataField = srcTable.Columns[i].ColumnName;
                            if (genAlternateColumns && i % 2 == 1)
                            {
                                field.NewLine = false;
                            }
                            this.FieldTypeSelector(srcTable.Columns[i].DataType, field);

                            svcCompChange.OnComponentChanging(formView, null);
                            formView.Fields.Add(field);
                            svcCompChange.OnComponentChanged(formView, null, null, null);
                        }
                        trans.Commit();
                        MessageBox.Show("generate fields successful!");
                    }
                }
            }
        }

        public void Preview(object sender, EventArgs e)
        {
            if (formView == null)
            {
                formView = this.Component as AjaxFormView;
            }
            if (formView.Fields.Count > 0)
            {
                List<AjaxViewFieldLayout> fieldLayouts = new List<AjaxViewFieldLayout>();
                foreach (AjaxFormField field in formView.Fields)
                {
                    AjaxViewFieldLayout fieldLayout = new AjaxViewFieldLayout();
                    fieldLayout.FieldName = field.DataField;
                    fieldLayout.Header = string.IsNullOrEmpty(field.Caption) ? field.DataField : field.Caption;
                    fieldLayout.NewLine = field.NewLine;
                    fieldLayout.FieldEditor = field.Editor;
                    fieldLayout.FieldWidth = formView.FieldWidth;
                    fieldLayout.LabelWidth = formView.LabelWidth;
                    fieldLayout.EditorWidth = field.Width;


                    fieldLayouts.Add(fieldLayout);
                }
                using (AjaxViewPreviewEditor editor = new AjaxViewPreviewEditor(fieldLayouts, "form"))
                {
                    editor.ShowDialog();
                }
            }
        }

        void FieldTypeSelector(Type fieldType, AjaxFormField field)
        {
            if (fieldType == typeof(uint) || fieldType == typeof(UInt16) || fieldType == typeof(UInt32) || fieldType == typeof(UInt64) || fieldType == typeof(int) || fieldType == typeof(Int16) || fieldType == typeof(Int32) || fieldType == typeof(Int64))
            {
                field.FieldType = "int";
                field.ValidType = ValidateType.Int;
            }
            else if (fieldType == typeof(Single) || fieldType == typeof(double) || fieldType == typeof(decimal))
            {
                field.FieldType = "float";
                field.ValidType = ValidateType.Float;
            }
            else if (fieldType == typeof(string))
            {
                field.FieldType = "string";
            }
            else if (fieldType == typeof(bool))
            {
                field.FieldType = "boolean";
            }
            else if (fieldType == typeof(DateTime))
            {
                field.FieldType = "date";
                field.Formatter = "Y/m/d";//"Ext.util.Format.dateRenderer('Y/m/d')";
                field.Editor = ExtGridEditor.DateTimePicker;
            }
        }

        bool IsKeyField(string columnName, DataColumn[] primaryKeys)
        {
            bool isPrimaryKey = false;
            foreach (DataColumn keyCol in primaryKeys)
            {
                if (keyCol.ColumnName == columnName)
                {
                    isPrimaryKey = true;
                    break;
                }
            }
            return isPrimaryKey;
        }

        string Caption(string fieldName, DataTable ddTable)
        {
            foreach (DataRow row in ddTable.Rows)
            {
                if (string.Compare(row["FIELD_NAME"].ToString(), fieldName, true) == 0)
                {
                    return row["CAPTION"].ToString();
                }
            }
            return fieldName;
        }

        DataTable GetDesignTable(WebDataSource wds)
        {
            if (wds != null)
            {
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
                    return wds.DesignDataSet.Tables[wds.DataMember];
                }
            }
            return null;
        }
    }

    public class AjaxLayoutDesigner : ControlDesigner
    {
        IDesignerHost host = null;
        IComponentChangeService svcCompChange = null;
        AjaxLayout layout = null;

        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection verbs = new DesignerVerbCollection();
                verbs.Add(new DesignerVerb("Generate ToolItems...", new EventHandler(GenToolItems)));
                return verbs;
            }
        }

        public void GenToolItems(object sender, EventArgs e)
        {
            if (layout == null)
            {
                layout = this.Component as AjaxLayout;
            }
            if (layout != null && layout.ToolItems.Count == 0)
            {
                if (host == null)
                {
                    host = (IDesignerHost)GetService(typeof(IDesignerHost));
                }
                if (svcCompChange == null)
                {
                    svcCompChange = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                }
                DesignerTransaction trans = host.CreateTransaction("Generate ToolItems");

                string msg = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ToolItems", true);
                //add,edit,delete,save,abort,refresh,query,ok,cancel
                string[] captions = string.IsNullOrEmpty(msg) ? new string[] { "add", "edit", "delete", "save", "abort", "refresh", "query", "ok", "cancel" } : msg.Split(',');

                GenToolButtonItem(ExtGridSystemHandler.Add, captions[0]);
                GenToolButtonItem(ExtGridSystemHandler.Edit, captions[1]);
                GenToolButtonItem(ExtGridSystemHandler.Delete, captions[2]);
                GenNotButtonItem(ExtGridToolItemType.Separation);
                GenToolButtonItem(ExtGridSystemHandler.OK, captions[7]);
                GenToolButtonItem(ExtGridSystemHandler.Cancel, captions[8]);
                //GenToolButtonItem(ExtGridSystemHandler.Save, captions[3]);
                //GenToolButtonItem(ExtGridSystemHandler.Abort, captions[4]);

                trans.Commit();
                MessageBox.Show("generate tool items successful!");
            }
        }

        void GenToolButtonItem(ExtGridSystemHandler handler, string caption)
        {
            svcCompChange.OnComponentChanging(layout, null);
            ExtGridToolItem item = new ExtGridToolItem();
            item.CssClass = "x-btn-text-icon details";
            switch (handler)
            {
                case ExtGridSystemHandler.Add:
                    item.IconUrl = "~/Image/Ext/add.gif";
                    item.ButtonName = "btnAdd";
                    break;
                case ExtGridSystemHandler.Edit:
                    item.IconUrl = "~/Image/Ext/edit.gif";
                    item.ButtonName = "btnEdit";
                    break;
                case ExtGridSystemHandler.Delete:
                    item.IconUrl = "~/Image/Ext/delete.gif";
                    item.ButtonName = "btnDelete";
                    break;
                case ExtGridSystemHandler.OK:
                    item.IconUrl = "~/Image/Ext/ok.gif";
                    item.ButtonName = "btnOK";
                    break;
                case ExtGridSystemHandler.Cancel:
                    item.IconUrl = "~/Image/Ext/cancel.gif";
                    item.ButtonName = "btnCancel";
                    break;
                //case ExtGridSystemHandler.Save:
                //    item.IconUrl = "~/Image/Ext/save.gif";
                //    item.ButtonName = "btnSave";
                //    break;
                //case ExtGridSystemHandler.Abort:
                //    item.IconUrl = "~/Image/Ext/abort.gif";
                //    item.ButtonName = "btnAbort";
                //    break;
            }
            item.Text = caption;
            item.SysHandlerType = handler;
            item.ToolItemType = ExtGridToolItemType.Button;
            layout.ToolItems.Add(item);
            svcCompChange.OnComponentChanged(layout, null, null, null);
        }

        void GenNotButtonItem(ExtGridToolItemType type)
        {
            if (type == ExtGridToolItemType.Button || type == ExtGridToolItemType.Label)
            {
                return;
            }
            svcCompChange.OnComponentChanging(layout, null);
            ExtGridToolItem item = new ExtGridToolItem();
            item.ToolItemType = type;
            layout.ToolItems.Add(item);
            svcCompChange.OnComponentChanged(layout, null, null, null);
        }
    }
}
