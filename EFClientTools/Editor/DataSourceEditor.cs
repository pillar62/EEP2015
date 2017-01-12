using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace EFClientTools.Editor
{
    public class DataSourceEditor : StringSelectorEditor
    {
        protected override List<string> GetListToSelect(ITypeDescriptorContext context)
        {
            List<string> lst = new List<string>();
            object instance = context.Instance;
            Control baseControl = context.Instance as Control;
            foreach (Control ctrl in baseControl.Page.Controls)
            {
                if (ctrl is IEFDataSource && ctrl.ID != baseControl.ID)
                {
                    lst.Add(ctrl.ID);
                }
            }
            return lst;
        }
    }
}
