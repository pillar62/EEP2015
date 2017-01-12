using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Reflection;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms.Design;
using Srvtools;
using System.Collections;

namespace AjaxTools
{
    public class ExtStaticStringEditor : UITypeEditor
    {
        [PermissionSet(SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [PermissionSet(SecurityAction.Demand)]
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance is InfoOwnerCollectionItem)
            {
                InfoOwnerCollectionItem collectionItem = context.Instance as InfoOwnerCollectionItem;
                AjaxBaseControl baseControl = collectionItem.Owner as AjaxBaseControl;

                if (context.PropertyDescriptor.Name == "FieldType" && (collectionItem is ExtGridColumn || collectionItem is AjaxFormField))
                {
                    objName.AddRange(new string[] { "string", "int", "float", "boolean", "date" });
                }
                else if (context.PropertyDescriptor.Name == "TextAlign" && collectionItem is ExtGridColumn)
                {
                    objName.AddRange(new string[] { "left", "center", "right" });
                }
                else if ((context.PropertyDescriptor.Name == "Operator" && collectionItem is ExtQueryField) ||
                    (context.PropertyDescriptor.Name == "Condition" && collectionItem is ExtWhereItem))
                {
                    objName.AddRange(new string[] { "=", "!=", ">", "<", ">=", "<=", "%", "%%" });
                }
                else if (context.PropertyDescriptor.Name == "Condition" && collectionItem is ExtQueryField)
                {
                    objName.AddRange(new string[] { "None", "And", "Or" });
                }
                else if ((context.PropertyDescriptor.Name == "DataField" && (collectionItem is ExtSimpleColumn || collectionItem is ExtGridColumn || collectionItem is AjaxFormField || collectionItem is ExtQueryField)) || (context.PropertyDescriptor.Name == "FieldName" && collectionItem is ExtWhereItem))
                {
                    DataTable srcTab = GetSourceTable(baseControl);
                    if (srcTab != null)
                    {
                        foreach (DataColumn field in srcTab.Columns)
                        {
                            objName.Add(field.ColumnName);
                        }
                    }
                }
                else if (context.PropertyDescriptor.Name == "ColumnName" && collectionItem is AjaxSecColumnItem)
                {
                    DataTable srcTab = GetSourceTable(baseControl);
                    if (srcTab != null)
                    {
                        foreach (DataColumn field in srcTab.Columns)
                        {
                            objName.Add(field.ColumnName);
                        }
                    }
                }
                else if (context.PropertyDescriptor.Name == "EditControlId" && (collectionItem is ExtGridColumn || collectionItem is AjaxFormField))
                {
                    PropertyInfo propEditor = collectionItem.GetType().GetProperty("Editor");
                    if (propEditor != null
                        && propEditor.PropertyType == typeof(ExtGridEditor))
                    {
                        if ((ExtGridEditor)propEditor.GetValue(collectionItem, null) == ExtGridEditor.ComboBox)
                        {
                            foreach (Control ctrl in baseControl.Page.Controls)
                            {
                                if (ctrl is ExtComboBox)
                                {
                                    objName.Add(ctrl.ID);
                                }
                            }
                        }
                        else if ((ExtGridEditor)propEditor.GetValue(collectionItem, null) == ExtGridEditor.RefButton)
                        {
                            foreach (Control ctrl in baseControl.Page.Controls)
                            {
                                if (ctrl is ExtRefButton)
                                {
                                    objName.Add(ctrl.ID);
                                }
                            }
                        }
                        else if ((ExtGridEditor)propEditor.GetValue(collectionItem, null) == ExtGridEditor.RefVal)
                        {
                            foreach (Control ctrl in baseControl.Page.Controls)
                            {
                                if (ctrl is ExtRefVal)
                                {
                                    objName.Add(ctrl.ID);
                                }
                            }
                        }
                    }
                }
                else if (context.PropertyDescriptor.Name == "ControlId" && collectionItem is MultiViewItem)
                {
                    MultiViewItem item = context.Instance as MultiViewItem;
                    foreach (Control ctrl in baseControl.Page.Controls)
                    {
                        if (ctrl is AjaxGridView || ctrl is AjaxFormView)
                        {
                            objName.Add(ctrl.ID);
                        }
                    }
                }
                else if (context.Instance is ExtColumnMatch)
                {
                    ExtColumnMatch extColumnMatch = context.Instance as ExtColumnMatch;
                    if (context.PropertyDescriptor.Name == "DestField")
                    {
                        foreach (Control ctrl in baseControl.Page.Controls)
                        {
                            bool isView = false;
                            if (ctrl is AjaxGridView)
                            {
                                AjaxGridView grid = ctrl as AjaxGridView;
                                foreach (ExtGridColumn col in grid.Columns)
                                {
                                    if (col.EditControlId == baseControl.ID)
                                    {
                                        isView = true;
                                        break;
                                    }
                                }
                            }
                            else if (ctrl is AjaxFormView)
                            {
                                AjaxFormView form = ctrl as AjaxFormView;
                                foreach (AjaxFormField field in form.Fields)
                                {
                                    if (field.EditControlId == baseControl.ID)
                                    {
                                        isView = true;
                                        break;
                                    }
                                }
                            }
                            if (isView)
                            {
                                DataTable srcTab = GetSourceTable(ctrl as AjaxBaseControl);
                                if (srcTab != null)
                                {
                                    foreach (DataColumn field in srcTab.Columns)
                                    {
                                        objName.Add(field.ColumnName);
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else if (context.PropertyDescriptor.Name == "SrcField")
                    {
                        DataTable srcTab = GetSourceTable(baseControl);
                        if (srcTab != null)
                        {
                            foreach (DataColumn field in srcTab.Columns)
                            {
                                objName.Add(field.ColumnName);
                            }
                        }
                    }
                }
                else if (context.Instance is ExtRefButtonColumnMatch)
                {
                    ExtRefButton refButton = baseControl as ExtRefButton;
                    ExtRefButtonColumnMatch refButtonColMatch = context.Instance as ExtRefButtonColumnMatch;
                    if (refButton != null)
                    {
                        if (context.PropertyDescriptor.Name == "DestField")
                        {
                            foreach (Control ctrl in baseControl.Page.Controls)
                            {
                                bool isView = false;
                                if (ctrl is AjaxFormView)
                                {
                                    AjaxFormView form = ctrl as AjaxFormView;
                                    foreach (AjaxFormField field in form.Fields)
                                    {
                                        if (field.EditControlId == baseControl.ID)
                                        {
                                            isView = true;
                                            break;
                                        }
                                    }
                                }
                                if (isView)
                                {
                                    DataTable srcTab = GetSourceTable(ctrl as AjaxBaseControl);
                                    if (srcTab != null)
                                    {
                                        foreach (DataColumn field in srcTab.Columns)
                                        {
                                            objName.Add(field.ColumnName);
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else if (context.PropertyDescriptor.Name == "SrcField")
                        {
                            if (string.IsNullOrEmpty(refButton.SourceDataSourceID))
                            {
                                System.Windows.Forms.MessageBox.Show("The 'SourceDataSourceID' property must be set first.");
                            }
                            else
                            {
                                DataTable srcTab = GetSourceTable(baseControl);
                                if (srcTab != null)
                                {
                                    foreach (DataColumn field in srcTab.Columns)
                                    {
                                        objName.Add(field.ColumnName);
                                    }
                                }
                            }
                        }
                        else if (context.PropertyDescriptor.Name == "SrcControlId")
                        {
                            UpdatePanel upan = refButton.GetModalUpdatePanel();
                            if (upan != null)
                            {
                                FileLevelPageControlBuilder ctrlBuilder = upan.ContentTemplate as FileLevelPageControlBuilder;
                                if (ctrlBuilder != null)
                                {
                                    foreach (DictionaryEntry control in ctrlBuilder.BuiltObjects)
                                    {
                                        if (control.Key is WebControl)
                                        {
                                            objName.Add(((WebControl)control.Key).ID);
                                        }
                                    }
                                }
                            }
                        }
                        else if (context.PropertyDescriptor.Name == "SrcControlValueProperty")
                        {
                            objName.AddRange(new string[] { "BindingValue", "Checked", "SelectedValue", "Text" });
                        }
                    }
                }
            }
            else if (context.Instance is IChildSet)
            {
                IChildSet set = context.Instance as IChildSet;
                if (context.PropertyDescriptor.Name == "AutoFillingColumn")
                {
                    foreach (ExtGridColumn col in (set.OwnerView as AjaxGridView).Columns)
                    {
                        if (!string.IsNullOrEmpty(col.ColumnName) && col.Visible)
                        {
                            objName.Add(col.ColumnName);
                        }
                    }
                }
                else if (context.PropertyDescriptor.Name == "GridPanel")
                {
                    foreach (Control ctrl in set.OwnerView.Page.Controls)
                    {
                        if (ctrl is UpdatePanel || ctrl is Panel)
                        {
                            objName.Add(ctrl.ID);
                        }
                    }
                }
            }
            else if (context.Instance is AjaxGridView)
            {
                AjaxGridView grid = context.Instance as AjaxGridView;
                if (context.PropertyDescriptor.Name == "EditPanelID")
                {
                    foreach (Control ctrl in grid.Page.Controls)
                    {
                        if (ctrl is ExtModalPanel)
                        {
                            objName.Add(ctrl.ID);
                        }
                    }
                }
                else if (context.PropertyDescriptor.Name == "QueryPanelID")
                {
                    foreach (Control ctrl in grid.Page.Controls)
                    {
                        if (ctrl is ExtModalPanel)
                        {
                            objName.Add(ctrl.ID);
                        }
                    }
                }
            }
            else if (context.Instance is ExtComboBox)
            {
                ExtComboBox cmb = context.Instance as ExtComboBox;
                if (context.PropertyDescriptor.Name == "ComboPanel")
                {
                    foreach (Control ctrl in cmb.Page.Controls)
                    {
                        if (ctrl is Panel)
                        {
                            objName.Add(ctrl.ID);
                        }
                    }
                }
            }
            else if (context.Instance is AjaxLayout)
            {
                AjaxLayout layout = context.Instance as AjaxLayout;
                if (context.PropertyDescriptor.Name == "View"/* || context.PropertyDescriptor.Name == "Details"*/)
                {
                    foreach (Control ctrl in layout.Page.Controls)
                    {
                        if (ctrl is AjaxGridView)
                        {
                            objName.Add(ctrl.ID);
                        }
                    }
                }
                //else if (context.PropertyDescriptor.Name == "Master")
                //{
                //    foreach (Control ctrl in layout.Page.Controls)
                //    {
                //        if (ctrl is AjaxFormView)
                //        {
                //            objName.Add(ctrl.ID);
                //        }
                //    }
                //}
                else if (context.PropertyDescriptor.Name == "LayoutPanel")
                {
                    foreach (Control ctrl in layout.Page.Controls)
                    {
                        if (ctrl is Panel)
                        {
                            objName.Add(ctrl.ID);
                        }
                    }
                }
            }
            else if (context.Instance is ExtRefButton)
            {
                ExtRefButton refButton = context.Instance as ExtRefButton;
                if (context.PropertyDescriptor.Name == "DestinDataSourceID" || context.PropertyDescriptor.Name == "SourceDataSourceID")
                {
                    foreach (Control ctrl in refButton.Page.Controls)
                    {
                        if (ctrl is WebDataSource)
                        {
                            objName.Add(ctrl.ID);
                        }
                    }
                }
                else if (context.PropertyDescriptor.Name == "ModalPanelID")
                {
                    foreach (Control ctrl in refButton.Page.Controls)
                    {
                        if (ctrl is ExtModalPanel)
                        {
                            objName.Add(ctrl.ID);
                        }
                    }
                }
            }

            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, objName.ToArray());
                string strValue = (string)value;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }

        DataTable GetSourceTable(AjaxBaseControl ctrl)
        {
            object src = null;
            if (ctrl is IAjaxDataSource)
            {
                src = ctrl.GetObjByID(((IAjaxDataSource)ctrl).DataSourceID);
            }
            else if (ctrl is ExtRefButton)
            {
                src = ctrl.GetObjByID(((ExtRefButton)ctrl).SourceDataSourceID);
            }
            if (src != null && src is WebDataSource)
            {
                WebDataSource wds = src as WebDataSource;
                if (string.IsNullOrEmpty(wds.SelectAlias) && string.IsNullOrEmpty(wds.SelectCommand))
                {
                    if (wds.DesignDataSet == null)
                    {
                        WebDataSet ds = GloFix.CreateDataSet(wds.WebDataSetID);
                        wds.DesignDataSet = ds.RealDataSet;
                    }
                    return wds.DesignDataSet.Tables[wds.DataMember].Clone();
                }
                else
                {
                    return wds.CommandTable.Clone();
                }
            }
            return null;
        }
    }
}
