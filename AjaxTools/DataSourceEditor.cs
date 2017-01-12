using System.Collections.Generic;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Security.Permissions;
using Srvtools;
using System.Web.UI.WebControls;

namespace AjaxTools
{
    public class DataSourceEditor : UITypeEditor
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
            ControlCollection ctrlList = null;
            if (context.Instance is Control)
            {
                ctrlList = ((Control)context.Instance).Page.Controls;
            }
            else if (context.Instance is InfoOwnerCollectionItem && ((InfoOwnerCollectionItem)context.Instance).Owner is Control)
            {
                ctrlList = ((Control)((InfoOwnerCollectionItem)context.Instance).Owner).Page.Controls;
            }

            foreach (Control ctrl in ctrlList)
            {
                if (ctrl is WebDataSource)
                {
                    objName.Add(ctrl.ID);
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
    }
}
