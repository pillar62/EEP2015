using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Security.Permissions;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Web.UI;
using EFBase;

namespace EFClientTools.Editor
{
    public abstract class StringSelectorEditor : UITypeEditor
    {
        [PermissionSet(SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        protected abstract List<string> GetListToSelect(ITypeDescriptorContext context);

        [PermissionSet(SecurityAction.Demand)]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            List<string> lstToSel = this.GetListToSelect(context);
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                System.Windows.Forms.ListBox lst = new System.Windows.Forms.ListBox();
                lst.Click += delegate(object sender, EventArgs e)
                {
                    service.CloseDropDown();
                };
                // 选中原有值
                int iPos = -1;
                foreach (string s in lstToSel)
                {
                    int i = lst.Items.Add(s);
                    if (s.Equals(value)) iPos = i;
                }
                if (iPos != -1) lst.SelectedIndex = iPos;
                // 下拉
                service.DropDownControl(lst);
                if (lst.SelectedItem != null)
                    value = lst.SelectedItem.ToString();
                else
                    value = "";
            }
            return value;
        }
    }

    public abstract class ModalSelectorEditor : UITypeEditor
    {
        [PermissionSet(SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        protected abstract EditorDialog CreateDialog(Control control, ITypeDescriptorContext context);

        [PermissionSet(SecurityAction.Demand)]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            object instance = context.Instance;
            Control control = null;
            if (context.Instance is Control)
            {
                control = instance as Control;
            }
            else if (context.Instance is EFCollectionItem)
            {
                IEFProperty collectionItem = instance as IEFProperty;
                control = collectionItem.ParentProperty.Component as Control;
            }
            EditorDialog dialog = this.CreateDialog(control, context);
            if (dialog != null)
            {
                dialog.ReturnValue = value;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return dialog.ReturnValue;
                }
            }
            return value;
        }
    }
}
