using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Data;

namespace EFBase.Design
{
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
        public virtual List<string> GetListOfValues(System.ComponentModel.ITypeDescriptorContext context)
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
                //throw new MissingMethodException(
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
                    value = string.Empty;
                }
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Error"
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
                form.SelectedValue = value;
                if (service.ShowDialog(form) == DialogResult.OK)
                {
                    value = form.SelectedValue;
                }

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Error"
                       , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return value;
        }
    }

    public class ModalForm: Form
    {
        public virtual object SelectedValue { get; set; }
    }
}
