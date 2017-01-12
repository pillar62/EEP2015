using System;
using System.Web.UI.Design;


namespace AjaxTools
{
    public class SmartMenuDesigner : DataSourceDesigner
    {
        public void SetDirty()
        {
            this.Tag.SetDirty(true);
        }
    }
}
