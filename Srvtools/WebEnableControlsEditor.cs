using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace Srvtools
{
    internal class WebEnableControlsEditor : UITypeEditor
    {
        IWindowsFormsEditorService EditorService = null;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }
            WebNavigatorStateItem stateItem = context.Instance as WebNavigatorStateItem;
            WebNavigator navigator = stateItem.Owner as WebNavigator;
            if (EditorService != null && navigator != null)
            {
                WebNavigatorEnableControlsEditorDialog editorDialog = new WebNavigatorEnableControlsEditorDialog(value as string, navigator);
                if (DialogResult.OK == EditorService.ShowDialog(editorDialog))
                {
                    value = editorDialog.EnableControls;
                }
            }
            return value;
        }
    }
}
