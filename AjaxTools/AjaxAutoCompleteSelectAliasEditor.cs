using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using Srvtools;

namespace AjaxTools
{
    public class AjaxAutoCompleteSelectAliasEditor : UITypeEditor
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (context != null)
            {
                object obj = context.Instance;
                if (obj is AjaxAutoComplete)
                {
                    AjaxAutoComplete autoComplete = (AjaxAutoComplete)obj;
                    IGetSelectAlias aItem = (IGetSelectAlias)autoComplete;
                    if (edSvc != null)
                    {
                        StringListSelector mySelector = new StringListSelector(edSvc, aItem.GetSelectAlias());
                        string strValue = (string)value;
                        if (mySelector.Execute(ref strValue)) value = strValue;

                        return strValue;
                    }
                }
            }
            return null;
        }
    }
}
