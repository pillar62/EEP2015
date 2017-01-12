using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Reflection;
using System.ComponentModel.Design;

namespace JQClientTools
{
    //public interface IJQGetValues
    //{
    //    string[] GetValues(string propertyName);
    //}

    /// <summary>
    /// 实现RemoteName和DataMember的编辑器
    /// </summary>
    public interface IJQDataSourceProvider
    {
        string RemoteName { get; set; }
        string DataMember { get; set; }
    }

    public interface IColumnCaptions
    {
        Dictionary<string, string> ColumnCaptions { get; }
    }

    public interface IDetailObject
    {
        string ParentObjectID { get; set; }
    }

    /// <summary>
    /// Property dropdown editor
    /// </summary>
    public class PropertyDropDownEditor : UITypeEditor
    {
        /// <summary>
        /// Gets edit style
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns>Edit style</returns>
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// Gets list of values
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns>List of values</returns>
        protected virtual List<string> GetListOfValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            return new List<string>();
        }

        /// <summary>
        /// Edit value
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="provider">IServiceProvider</param>
        /// <param name="value">Value</param>
        /// <returns>Value</returns>
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            var service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            try
            {
                ListBox listbox = new ListBox();
                listbox.Items.AddRange(GetListOfValues(context).ToArray());
                listbox.SelectedItem = value;
                listbox.Click += delegate(object sender, EventArgs e)
                {
                    service.CloseDropDown();
                };
                service.DropDownControl(listbox);
                if (listbox.SelectedItem != null)
                {
                    value = listbox.SelectedItem.ToString();
                }
                else
                {
                    //value = string.Empty;
                }
            }
            catch (Exception e)
            {
                var message = e.InnerException != null ? e.InnerException.Message : e.Message;
                System.Windows.Forms.MessageBox.Show(message, "Error"
                       , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return value;
        }
    }

    /// <summary>
    /// Property modal editor
    /// </summary>
    public class PropertyModalEditor : UITypeEditor
    {
        /// <summary>
        /// Gets edit style
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns>Edit style</returns>
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Gets modal form
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns>Modal form</returns>
        public virtual ModalForm GetModalForm(System.ComponentModel.ITypeDescriptorContext context)
        {
            return new ModalForm();
        }


        /// <summary>
        /// Edit value
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="provider">IServiceProvider</param>
        /// <param name="value">Value</param>
        /// <returns>Value</returns>
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            var service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            try
            {
                var form = GetModalForm(context);
                if (form != null)
                {
                    form.SelectedValue = value;
                    if (service.ShowDialog(form) == DialogResult.OK)
                    {
                        value = form.SelectedValue;
                    }
                }

            }
            catch (Exception e)
            {
                var message = e.InnerException != null ? e.InnerException.Message : e.Message;
                System.Windows.Forms.MessageBox.Show(message, "Error"
                       , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return value;
        }
    }

    public class ModalForm : Form
    {
        public virtual object SelectedValue { get; set; }
    }

    public class RemoteNameEditor : PropertyModalEditor
    {
        public override ModalForm GetModalForm(ITypeDescriptorContext context)
        {
            return new Editor.RemoteNameEditorDialog();
        }
    }

    public class DataMemberEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            if (context.Instance.GetType().GetInterface("IJQDataSourceProvider") != null)
            {
                IJQDataSourceProvider item = (IJQDataSourceProvider)context.Instance;
                if (!string.IsNullOrEmpty(item.RemoteName))
                {
                    var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                    clientInfo.UseDataSet = true;
                    var assemblyName = item.RemoteName.Split('.')[0];
                    var commandName = item.RemoteName.Split('.')[1];

                    var dataMembers = EFClientTools.DesignClientUtility.Client.GetEntityNavigationFields(clientInfo, assemblyName, commandName, null);
                    list.Add(commandName);
                    foreach (var dataMember in dataMembers)
                    {
                        list.Add(dataMember);
                    }
                }
            }
            return list;
        }
    }

    public class FieldEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            if (context.Instance.GetType().GetInterface("IJQDataSourceProvider") != null)
            {
                IJQDataSourceProvider item = (IJQDataSourceProvider)context.Instance;
                if (!string.IsNullOrEmpty(item.RemoteName) && !string.IsNullOrEmpty(item.DataMember))
                {
                    var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                    clientInfo.UseDataSet = true;
                    var assemblyName = item.RemoteName.Split('.')[0];
                    var commandName = item.DataMember;

                    var fields = EFClientTools.DesignClientUtility.Client.GetEntityFields(clientInfo, assemblyName, commandName , null);
                    foreach (var field in fields)
                    {
                        list.Add(field);
                    }
                }
            }
            return list;
        }
    }

    public class ParentFieldEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            if (context.Instance is JQRelationColumn)
            {
                var detail = (context.Instance as IJQProperty).ParentProperty.Component as IDetailObject;
                if (!string.IsNullOrEmpty(detail.ParentObjectID))
                {
                    var parentComponent = (detail as System.Web.UI.WebControls.WebControl).Parent.FindControl(detail.ParentObjectID);
                    if (parentComponent is IJQDataSourceProvider)
                    {
                        IJQDataSourceProvider item = (IJQDataSourceProvider)parentComponent;
                        if (!string.IsNullOrEmpty(item.RemoteName) && !string.IsNullOrEmpty(item.DataMember))
                        {
                            var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                            clientInfo.UseDataSet = true;
                            var assemblyName = item.RemoteName.Split('.')[0];
                            var commandName = item.DataMember;

                            var fields = EFClientTools.DesignClientUtility.Client.GetEntityFields(clientInfo, assemblyName, commandName, null);
                            foreach (var field in fields)
                            {
                                list.Add(field);
                            }
                        }
                    }
                }
            }
            else if (context.Instance is JQColumnMatch || context.Instance is JQWhereItem)
            {
                var refval = (context.Instance as IJQProperty).ParentProperty.Component as JQRefval;
                if (refval.ParentObject != null)
                {
                    var parentComponent = refval.ParentObject;
                    if (parentComponent is IJQDataSourceProvider)
                    {
                        IJQDataSourceProvider item = (IJQDataSourceProvider)parentComponent;
                        if (!string.IsNullOrEmpty(item.RemoteName) && !string.IsNullOrEmpty(item.DataMember))
                        {
                            var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                            clientInfo.UseDataSet = true;
                            var assemblyName = item.RemoteName.Split('.')[0];
                            var commandName = item.DataMember;

                            var fields = EFClientTools.DesignClientUtility.Client.GetEntityFields(clientInfo, assemblyName, commandName, null);
                            foreach (var field in fields)
                            {
                                list.Add(field);
                            }
                        }
                    }
                }
            }
            return list;
        }
    }

    public class DataControlEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {

            var list = new List<string>();
            if (context.Instance is System.Web.UI.WebControls.WebControl)
            {
                foreach (var control in (context.Instance as System.Web.UI.WebControls.WebControl).Page
                    .Controls.OfType<System.Web.UI.WebControls.WebControl>())
                {
                    if (control is JQDataGrid || control is JQDataForm)
                    {
                        list.Add(control.ID);
                    }
                    if (control.Controls.Count > 0)
                    {
                        GetChildrenControls(control, list);
                    }
                }
            }
            return list;
        }

        private void GetChildrenControls(System.Web.UI.WebControls.WebControl control, List<string> list)
        {
            foreach (var item in control.Controls.OfType<System.Web.UI.WebControls.WebControl>())
            {
                if (item is JQDataGrid || item is JQDataForm)
                {
                    list.Add(item.ID);
                }
                if (item.Controls.Count > 0)
                {
                    GetChildrenControls(item, list);
                }
            }
        }
    }

    public class GridControlEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            if (context.Instance is System.Web.UI.WebControls.WebControl)
            {
                foreach (var control in (context.Instance as System.Web.UI.WebControls.WebControl).Parent
                    .Controls.OfType<System.Web.UI.WebControls.WebControl>())
                {
                    if (control is JQDataGrid)
                    {
                        list.Add(control.ID);
                    }
                }
            }
            return list;
        }
    }

    public class FormControlEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {

            var list = new List<string>();
            if (context.Instance is System.Web.UI.WebControls.WebControl)
            {
                foreach (var control in (context.Instance as System.Web.UI.WebControls.WebControl).Parent
                    .Controls.OfType<System.Web.UI.WebControls.WebControl>())
                {
                    if (control is JQDataForm)
                    {
                        list.Add(control.ID);
                    }
                }
            }
            return list;
        }
    }

    public class WindowControlEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            if (context.Instance is System.Web.UI.WebControls.WebControl)
            {
                foreach (var control in (context.Instance as System.Web.UI.WebControls.WebControl).Parent
                    .Controls.OfType<System.Web.UI.WebControls.WebControl>())
                {
                    if (control is JQDialog)
                    {
                        list.Add(control.ID);
                    }
                }
            }
            return list;
        }
    }

    public class EditControlEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQClientTools.JQEditorControl).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class EditorOptionsEditor : PropertyModalEditor
    {
        public override ModalForm GetModalForm(ITypeDescriptorContext context)
        {
            if (context.Instance is JQGridColumn)
            {
                if (context.PropertyDescriptor.Name == "RelationOptions")
                {
                    return new Editor.RelationEditorDialog();
                }
                else  if((context.Instance as JQGridColumn).Editor == JQEditorControl.ComboBox)
                {
                    return new Editor.ComboBoxEditorDialog();
                }
                else if ((context.Instance as JQGridColumn).Editor == JQEditorControl.RefValBox)
                {
                    return new Editor.RefvalEditorDialog(context);
                }
                else if ((context.Instance as JQGridColumn).Editor == JQEditorControl.ComboGrid)
                {
                    return new Editor.ComboGridEditorDialog();
                }
                else if ((context.Instance as JQGridColumn).Editor == JQEditorControl.CheckBox)
                {
                    return new Editor.CheckBoxEditorDialog();
                }
                else if ((context.Instance as JQGridColumn).Editor == JQEditorControl.DateBox)
                {
                    return new Editor.DateBoxEditorDialog();
                }
                else if ((context.Instance as JQGridColumn).Editor == JQEditorControl.TimeSpinner)
                {
                    return new Editor.TimeSpinnerEditorDialog();
                }
                else if ((context.Instance as JQGridColumn).Editor == JQEditorControl.AutoComplete)
                {
                    return new Editor.AutoCompleteEditorDialog();
                }
                else if ((context.Instance as JQGridColumn).Editor == JQEditorControl.NumberBox)
                {
                    return new Editor.NumberBoxEditorDialog();
                }
                else if ((context.Instance as JQGridColumn).Editor == JQEditorControl.TextArea)
                {
                    return new Editor.TextAreaEditorDialog();
                }
                else if ((context.Instance as JQGridColumn).Editor == JQEditorControl.YearMonth)
                {
                    return new Editor.YearMonthEditorDialog();
                }
                else if ((context.Instance as JQGridColumn).Editor == JQEditorControl.TextBox || (context.Instance as JQGridColumn).Editor == JQEditorControl.FixTextBox)
                {
                    return new Editor.TextBoxEditorDialog();
                }
            }
            else if (context.Instance is JQQueryColumn)
            {
                if ((context.Instance as JQQueryColumn).Editor == JQEditorControl.ComboBox)
                {
                    return new Editor.ComboBoxEditorDialog();
                }
                else if ((context.Instance as JQQueryColumn).Editor == JQEditorControl.RefValBox)
                {
                    return new Editor.RefvalEditorDialog(context);
                }
                else if ((context.Instance as JQQueryColumn).Editor == JQEditorControl.ComboGrid)
                {
                    return new Editor.ComboGridEditorDialog();
                }
                else if ((context.Instance as JQQueryColumn).Editor == JQEditorControl.Options)
                {
                    return new Editor.OptionsEditorDialog();
                }
                else if ((context.Instance as JQQueryColumn).Editor == JQEditorControl.CheckBox)
                {
                    return new Editor.CheckBoxEditorDialog();
                }
                else if ((context.Instance as JQQueryColumn).Editor == JQEditorControl.YearMonth)
                {
                    return new Editor.YearMonthEditorDialog();
                }
                else if ((context.Instance as JQQueryColumn).Editor == JQEditorControl.TextBox)
                {
                    return new Editor.TextBoxEditorDialog();
                }
            }
            else if (context.Instance is JQFormColumn)
            {
                if ((context.Instance as JQFormColumn).Editor == JQEditorControl.ComboBox)
                {
                    return new Editor.ComboBoxEditorDialog();
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.RefValBox)
                {
                    return new Editor.RefvalEditorDialog(context);
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.ComboGrid)
                {
                    return new Editor.ComboGridEditorDialog();
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.CheckBox)
                {
                    return new Editor.CheckBoxEditorDialog();
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.DateBox)
                {
                    return new Editor.DateBoxEditorDialog();
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.TimeSpinner)
                {
                    return new Editor.TimeSpinnerEditorDialog();
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.FileUpload)
                {
                    return new Editor.FileUploadEditorDialog();
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.TextArea)
                {
                    return new Editor.TextAreaEditorDialog();
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.AutoComplete)
                {
                    return new Editor.AutoCompleteEditorDialog();
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.Options)
                {
                    return new Editor.OptionsEditorDialog();
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.Qrcode)
                {
                    return new Editor.QrcodeEditorDialog();
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.NumberBox)
                {
                    return new Editor.NumberBoxEditorDialog();
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.YearMonth)
                {
                    return new Editor.YearMonthEditorDialog();
                }
                else if ((context.Instance as JQFormColumn).Editor == JQEditorControl.TextBox)
                {
                    return new Editor.TextBoxEditorDialog();
                }

            }
            return null;
        }
    }

    public class ConditionEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQClientTools.JQCondtion).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class AndOrEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQClientTools.JQAndOr).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class DataTypeEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQClientTools.JQDataType).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class IconEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQClientTools.JQIcon).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class TotalTypeEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQClientTools.JQTotalType).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class JQAlignmentEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQClientTools.JQAlignment).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class JQTimeFormatEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(new string[] { "hh:mm", "hhmm"});
            return list;
        }
    }

    public class JQReportAlignmentEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQClientTools.JQReportAlignment).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class JQReportTotalEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQClientTools.JQReportTotal).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class ColorEditor : PropertyModalEditor
    {
       
    }

    public class DrillDownControlEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {

            var list = new List<string>();
            if (context.Instance is JQGridColumn)
            {
                if ((context.Instance as JQGridColumn).Component != null && (context.Instance as JQGridColumn).Component is JQDataGrid)
                {
                    foreach (var control in ((context.Instance as JQGridColumn).Component as System.Web.UI.WebControls.WebControl).Parent
                    .Controls.OfType<System.Web.UI.WebControls.WebControl>())
                    {
                        if (control is JQDrillDown)
                        {
                            list.Add(control.ID);
                        }
                    }

                }
            }
            return list;
        }
    }

    public class PivotTableRenderersTypeEditor : UITypeEditor
    {
        /// <summary>
        /// Gets edit style
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns>Edit style</returns>
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Gets modal form
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns>Modal form</returns>
        public virtual ModalForm GetModalForm(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context.Instance is JQPivotTable)
            {
                return new Editor.PivotTableRenderersTypeForm();
            }
            return null;
        }


        /// <summary>
        /// Edit value
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="provider">IServiceProvider</param>
        /// <param name="value">Value</param>
        /// <returns>Value</returns>
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            var service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            try
            {
                var form = GetModalForm(context);
                if (form != null && context.Instance is JQPivotTable)
                {
                    form.SelectedValue = (context.Instance as JQPivotTable).PivotTableRenderersI == null ? "" : (context.Instance as JQPivotTable).PivotTableRenderersI;
                    if (service.ShowDialog(form) == DialogResult.OK)
                    {
                        var selectedValue = form.SelectedValue;
                        value = selectedValue.ToString(); ;
                        var componentChangeService = (IComponentChangeService)context.GetService(typeof(IComponentChangeService));
                        componentChangeService.OnComponentChanging((context.Instance as JQPivotTable), null);
                        (context.Instance as JQPivotTable).PivotTableRenderersI = (form as Editor.PivotTableRenderersTypeForm).TrueValue.ToString(); ;
                        componentChangeService.OnComponentChanged((context.Instance as JQPivotTable), null, null, null);
                    }
                }

            }
            catch (Exception e)
            {
                var message = e.InnerException != null ? e.InnerException.Message : e.Message;
                System.Windows.Forms.MessageBox.Show(message, "Error"
                       , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return value;
        }
    }
    public class PivotTableAggregatorsModeEditor : UITypeEditor
    {
        /// <summary>
        /// Gets edit style
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns>Edit style</returns>
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Gets modal form
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns>Modal form</returns>
        public virtual ModalForm GetModalForm(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context.Instance is JQPivotTable)
            {
                return new Editor.PivotTableAggregatorsTypeForm();
            }
            return null;
        }


        /// <summary>
        /// Edit value
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="provider">IServiceProvider</param>
        /// <param name="value">Value</param>
        /// <returns>Value</returns>
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            var service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            try
            {
                var form = GetModalForm(context);
                if (form != null && context.Instance is JQPivotTable)
                {
                    form.SelectedValue = (context.Instance as JQPivotTable).PivotTableAggregatorsI == null ? "" : (context.Instance as JQPivotTable).PivotTableAggregatorsI;
                    if (service.ShowDialog(form) == DialogResult.OK)
                    {
                        var selectedValue = form.SelectedValue;
                        value = selectedValue.ToString();
                        var componentChangeService = (IComponentChangeService)context.GetService(typeof(IComponentChangeService));
                        componentChangeService.OnComponentChanging((context.Instance as JQPivotTable), null);
                        (context.Instance as JQPivotTable).PivotTableAggregatorsI = (form as Editor.PivotTableAggregatorsTypeForm).TrueValue.ToString(); ;
                        componentChangeService.OnComponentChanged((context.Instance as JQPivotTable), null, null, null);
                    }
                }

            }
            catch (Exception e)
            {
                var message = e.InnerException != null ? e.InnerException.Message : e.Message;
                System.Windows.Forms.MessageBox.Show(message, "Error"
                       , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return value;
        }
    }

}
