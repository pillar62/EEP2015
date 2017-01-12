using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace AjaxTools
{
    internal sealed class SmartMenuCollectionEditor : UITypeEditor
    {
        IWindowsFormsEditorService EditorService = null;
        public SmartMenuCollectionEditor()
            : base()
        {
        }

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
            SmartMenu menu = context.Instance as SmartMenu;
            if (menu != null && EditorService != null)
            {
                if (value is SmartMenuItemCollection)
                {
                    SmartMenuCollectionEditorDialog editorDialog = new SmartMenuCollectionEditorDialog(value as SmartMenuItemCollection);
                    if (DialogResult.OK == EditorService.ShowDialog(editorDialog))
                    {
                        value = editorDialog.Collection;
                        IDesignerHost service = (IDesignerHost)context.GetService(typeof(IDesignerHost));
                        SmartMenuDesigner designer = service.GetDesigner(menu) as SmartMenuDesigner;
                        designer.SetDirty();
                    }
                }
            }
            return value;
        }
    }
}
