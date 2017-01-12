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

namespace JQMobileTools
{
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

    public interface IThemeObject
    {
        string Theme { get; set; }
    }

    public interface IQueryObject
    {
        string QueryObjectID { get;}
    }

    public interface IDetailObject
    {
        string ParentObjectID { get;}
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
                var control = (context.Instance as IJQProperty).ParentProperty.Component as JQControl;
                if (control.Context != null)
                {
                    var parentComponent = (control.Context.Instance as IJQProperty).ParentProperty.Component;
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

    public class FormControlEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            if (context.Instance is System.Web.UI.WebControls.WebControl)
            {
                foreach (var control in (context.Instance as System.Web.UI.WebControls.WebControl).Parent
                    .Controls.OfType<System.Web.UI.Control>())
                {
                    if (control is JQDataForm || control is JQTab)
                    {
                        list.Add(control.ID);
                    }
                }
            }
            return list;
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


    public class EditControlEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQEditorControl).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class EditorOptionsEditor : PropertyModalEditor
    {
        public override ModalForm GetModalForm(ITypeDescriptorContext context)
        {
            if (context.Instance is JQFormColumn)
            {
               var editor = (context.Instance as JQFormColumn).Editor;
               if (!string.IsNullOrEmpty(editor))
               {
                   return new Editor.JQDataFormEditorDialog(editor, context);

               }
            }
            else if (context.Instance is JQQueryColumn)
            {
                var editor = (context.Instance as JQQueryColumn).Editor;
                if (!string.IsNullOrEmpty(editor))
                {
                    return new Editor.JQDataFormEditorDialog(editor, context);
                }
            }
            else if (context.Instance is JQGridColumn)
            {
                return new Editor.JQDataFormEditorDialog(JQDisplayControl.Relation, context);
            }
            return null;
        }
    }

    public class ConditionEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQCondtion).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class DataTypeEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQDataType).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class IconEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQDataIcon).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class TotalTypeEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQTotalType).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class JQAlignmentEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            list.AddRange(typeof(JQAlignment).GetFields(BindingFlags.Static | BindingFlags.Public).Where(c => c.IsLiteral).Select(c => (string)c.GetValue(null)));
            return list;
        }
    }

    public class RDLCUrlEditor : System.Web.UI.Design.UrlEditor
    {
        protected override string Filter
        {
            get
            {
                return "RDLC Files (*.rdlc)|*.rdlc|All Files (*.*)|*.*";
            }
        }
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
    public class RDCLUrlEditor : System.Web.UI.Design.UrlEditor
    {
        protected override string Filter
        {
            get
            {
                return "RDLC Files (*.rdlc)|*.rdlc|All Files (*.*)|*.*";
            }
        }
    }
}
