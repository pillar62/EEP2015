using System;
using System.Xml;
using System.Data;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Srvtools;
using System.Web.UI.WebControls;

namespace AjaxTools
{
    public class FieldNameEditor : UITypeEditor
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
            IAjaxDataSource ajaxSource = null;
            ControlCollection ctrlList = null;
            if (context.Instance is IAjaxDataSource)
            {
                ajaxSource = (IAjaxDataSource)context.Instance;
                ctrlList = ((Control)context.Instance).Page.Controls;
            }
            else if (context.Instance is InfoOwnerCollectionItem && ((InfoOwnerCollectionItem)context.Instance).Owner is IAjaxDataSource)
            {
                ajaxSource = ((InfoOwnerCollectionItem)context.Instance).Owner as IAjaxDataSource;
                ctrlList = (((InfoOwnerCollectionItem)context.Instance).Owner as Control).Page.Controls;
            }
            if (ajaxSource != null)
            {
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource && ctrl.ID == ajaxSource.DataSourceID)
                    {
                        WebDataSource wds = (WebDataSource)ctrl;
                        DataTable srcTab = null;
                        if (string.IsNullOrEmpty(wds.SelectAlias) && string.IsNullOrEmpty(wds.SelectCommand))
                        {
                            if (wds.DesignDataSet == null)
                            {
                                WebDataSet ds = GloFix.CreateDataSet(wds.WebDataSetID);
                                wds.DesignDataSet = ds.RealDataSet;
                            }
                            srcTab = wds.DesignDataSet.Tables[wds.DataMember].Clone();
                        }
                        else
                        {
                            srcTab = wds.CommandTable.Clone();
                        }
                        foreach (DataColumn column in srcTab.Columns)
                        {
                            objName.Add(column.ColumnName);
                        }
                        break;
                    }
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
