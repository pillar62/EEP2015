using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Web.UI;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace Infolight.EasilyReportTools.Design
{
    internal class PropertyDropDownEditor: UITypeEditor
    {
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (service != null && context.Instance.GetType().GetInterface("IReportGetValues") != null)
            {
                IReportGetValues item = (IReportGetValues)context.Instance;
                ListBox listbox = new ListBox();
                listbox.Items.AddRange(item.GetValues(context.PropertyDescriptor.Name));
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
            return value;
        }
    }
}
