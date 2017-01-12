using System;
using System.ComponentModel.Design;
using System.Data;
using System.Web.UI.Design;
using System.Windows.Forms;
using Srvtools;
using System.Collections.Generic;

namespace AjaxTools
{
    public class ExtGridDesigner : ControlDesigner
    {
        IDesignerHost host = null;
        IComponentChangeService svcCompChange = null;
        AjaxGridView grid = null;

        public override System.ComponentModel.Design.DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection verbs = new DesignerVerbCollection();
                verbs.Add(new DesignerVerb("Generate Columns...", new EventHandler(GenColumns)));
                verbs.Add(new DesignerVerb("Generate ToolItems...", new EventHandler(GenToolItems)));
                verbs.Add(new DesignerVerb("Preview", new EventHandler(Preview)));
                return verbs;
            }
        }

        public void GenColumns(object sender, EventArgs e)
        {
            if (grid == null)
            {
                grid = this.Component as AjaxGridView;
            }
            if (grid != null && !string.IsNullOrEmpty(grid.DataSourceID))
            {
                if (grid.Columns.Count == 0)
                {
                    WebDataSource wds = grid.GetObjByID(grid.DataSourceID) as WebDataSource;
                    DataTable srcTable = GetDesignTable(wds);
                    if (srcTable != null)
                    {
                        DataTable ddTable = DBUtils.GetDataDictionary(wds, true).Tables[0];

                        if (host == null)
                        {
                            host = (IDesignerHost)GetService(typeof(IDesignerHost));
                        }
                        if (svcCompChange == null)
                        {
                            svcCompChange = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                        }
                        DesignerTransaction trans = host.CreateTransaction("Generate Columns");
                        foreach (DataColumn column in srcTable.Columns)
                        {
                            ExtGridColumn extCol = new ExtGridColumn();
                            extCol.AllowSort = false;
                            extCol.ColumnName = string.Format("col{0}", column.ColumnName);
                            extCol.DataField = column.ColumnName;
                            extCol.ExpandColumn = true;
                            extCol.HeaderText = HeaderText(column.ColumnName, ddTable);
                            extCol.IsKeyField = IsKeyField(column.ColumnName, srcTable.PrimaryKey);
                            extCol.NewLine = (column.Ordinal % 2 == 0);
                            extCol.Resizable = true;
                            extCol.TextAlign = "left";
                            extCol.Visible = true;
                            extCol.Width = 75;
                            this.FieldTypeSelector(column.DataType, extCol);

                            svcCompChange.OnComponentChanging(grid, null);
                            grid.Columns.Add(extCol);
                            svcCompChange.OnComponentChanged(grid, null, null, null);
                        }
                        trans.Commit();
                        MessageBox.Show("generate columns successful!");
                    }
                }
            }
        }

        public void GenToolItems(object sender, EventArgs e)
        {
            if (grid == null)
            {
                grid = this.Component as AjaxGridView;
            }
            if (grid != null && grid.ToolItems.Count == 0)
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
                string[] captions = string.IsNullOrEmpty(msg) ? new string[] { "add", "edit", "delete", "save", "abort", "refresh", "save personal", "load personal" } : msg.Split(',');

                GenToolButtonItem(ExtGridSystemHandler.Add, captions[0]);
                GenToolButtonItem(ExtGridSystemHandler.Edit, captions[1]);
                GenToolButtonItem(ExtGridSystemHandler.Delete, captions[2]);
                GenNotButtonItem(ExtGridToolItemType.Separation);
                GenToolButtonItem(ExtGridSystemHandler.Save, captions[3]);
                GenToolButtonItem(ExtGridSystemHandler.Abort, captions[4]);
                GenNotButtonItem(ExtGridToolItemType.Separation);
                GenToolButtonItem(ExtGridSystemHandler.Query, captions[6]);
                GenNotButtonItem(ExtGridToolItemType.Fill);
                GenToolButtonItem(ExtGridSystemHandler.Refresh, captions[5]);
                GenToolButtonItem(ExtGridSystemHandler.SavePersonal, captions[9]);
                GenToolButtonItem(ExtGridSystemHandler.LoadPersonal, captions[10]);

                trans.Commit();
                MessageBox.Show("generate tool items successful!");
            }
        }

        public void Preview(object sender, EventArgs e)
        {
            if (grid == null)
            {
                grid = this.Component as AjaxGridView;
            }
            if (grid.Columns.Count > 0)
            {
                List<AjaxViewFieldLayout> fieldLayouts = new List<AjaxViewFieldLayout>();
                foreach (ExtGridColumn column in grid.Columns)
                {
                    AjaxViewFieldLayout fieldLayout = new AjaxViewFieldLayout();
                    fieldLayout.FieldName = column.DataField;
                    fieldLayout.Header = string.IsNullOrEmpty(column.HeaderText) ? column.DataField : column.HeaderText;
                    fieldLayout.NewLine = column.NewLine;
                    fieldLayout.FieldEditor = column.Editor;

                    fieldLayouts.Add(fieldLayout);
                }
                using (AjaxViewPreviewEditor editor = new AjaxViewPreviewEditor(fieldLayouts, "grid"))
                {
                    editor.ShowDialog();
                }
            }
        }

        void GenNotButtonItem(ExtGridToolItemType type)
        {
            if (type == ExtGridToolItemType.Button || type == ExtGridToolItemType.Label)
            {
                return;
            }
            svcCompChange.OnComponentChanging(grid, null);
            ExtGridToolItem item = new ExtGridToolItem();
            item.ToolItemType = type;
            grid.ToolItems.Add(item);
            svcCompChange.OnComponentChanged(grid, null, null, null);
        }

        void GenToolButtonItem(ExtGridSystemHandler handler, string caption)
        {
            svcCompChange.OnComponentChanging(grid, null);
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
                case ExtGridSystemHandler.Save:
                    item.IconUrl = "~/Image/Ext/save.gif";
                    item.ButtonName = "btnSave";
                    break;
                case ExtGridSystemHandler.Abort:
                    item.IconUrl = "~/Image/Ext/abort.gif";
                    item.ButtonName = "btnAbort";
                    break;
                case ExtGridSystemHandler.Query:
                    item.IconUrl = "~/Image/Ext/query.gif";
                    item.ButtonName = "btnQuery";
                    break;
                case ExtGridSystemHandler.Refresh:
                    item.IconUrl = "~/Image/Ext/refresh.gif";
                    item.ButtonName = "btnRefresh";
                    break;
                case ExtGridSystemHandler.SavePersonal:
                    item.IconUrl = "~/Image/Ext/savePersonal.png";
                    item.ButtonName = "btnSavePersonal";
                    break;
                case ExtGridSystemHandler.LoadPersonal:
                    item.IconUrl = "~/Image/Ext/loadPersonal.png";
                    item.ButtonName = "btnLoadPersonal";
                    break;
            }
            item.Text = caption;
            item.SysHandlerType = handler;
            item.ToolItemType = ExtGridToolItemType.Button;
            grid.ToolItems.Add(item);
            svcCompChange.OnComponentChanged(grid, null, null, null);
        }

        void FieldTypeSelector(Type fieldType, ExtGridColumn extCol)
        {
            if (fieldType == typeof(uint) || fieldType == typeof(UInt16) || fieldType == typeof(UInt32) || fieldType == typeof(UInt64) || fieldType == typeof(int) || fieldType == typeof(Int16) || fieldType == typeof(Int32) || fieldType == typeof(Int64))
            {
                extCol.FieldType = "int";
                extCol.ValidType = ValidateType.Int;
            }
            else if (fieldType == typeof(Single) || fieldType == typeof(double) || fieldType == typeof(decimal))
            {
                extCol.FieldType = "float";
                extCol.ValidType = ValidateType.Float;
            }
            else if (fieldType == typeof(string))
            {
                extCol.FieldType = "string";
            }
            else if (fieldType == typeof(bool))
            {
                extCol.FieldType = "boolean";
            }
            else if (fieldType == typeof(DateTime))
            {
                extCol.FieldType = "date";
                extCol.Formatter = "Y/m/d";//"Ext.util.Format.dateRenderer('Y/m/d')";
                extCol.Editor = ExtGridEditor.DateTimePicker;
            }
        }

        string HeaderText(string fieldName, DataTable ddTable)
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
}