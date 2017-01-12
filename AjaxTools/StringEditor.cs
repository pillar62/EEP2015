using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Reflection;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms.Design;
using Srvtools;

namespace AjaxTools
{
    public class StringEditor : UITypeEditor
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
            if (context.Instance is InfoOwnerCollectionItem)
            {
                InfoOwnerCollectionItem collectionItem = context.Instance as InfoOwnerCollectionItem;
                AjaxBaseControl baseControl = collectionItem.Owner as AjaxBaseControl;
                //if (context.PropertyDescriptor.Name == "ButtonType" && collectionItem is AjaxScheduleToolItem)
                //{
                //    objName.AddRange(new string[] { "previousYear", "previousMonth", "nextYear", "nextMonth", "today", "title", "month", "week", "day" });
                //}
            }
            else if (context.Instance is AjaxSchedule)
            {
                DataTable srcTab = GetSourceTable(context.Instance as AjaxBaseControl);
                if (srcTab != null)
                {
                    if (context.PropertyDescriptor.Name == "IdField"
                        || context.PropertyDescriptor.Name == "CaptionField"
                        || context.PropertyDescriptor.Name == "DescriptionField"
                        || context.PropertyDescriptor.Name == "StartDateTimeField"
                        || context.PropertyDescriptor.Name == "EndDateTimeField"
                        || context.PropertyDescriptor.Name == "AllDayField")
                    {
                        foreach (DataColumn field in srcTab.Columns)
                        {
                            objName.Add(field.ColumnName);
                        }
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

        DataTable GetSourceTable(AjaxBaseControl ctrl)
        {
            if (ctrl is IAjaxDataSource)
            {
                object src = ctrl.GetObjByID(((IAjaxDataSource)ctrl).DataSourceID);
                if (src != null && src is WebDataSource)
                {
                    WebDataSource wds = src as WebDataSource;
                    if (string.IsNullOrEmpty(wds.SelectAlias) && string.IsNullOrEmpty(wds.SelectCommand))
                    {
                        if (wds.DesignDataSet == null)
                        {
                            WebDataSet ds = GloFix.CreateDataSet(wds.WebDataSetID);
                            wds.DesignDataSet = ds.RealDataSet;
                        }
                        return wds.DesignDataSet.Tables[wds.DataMember].Clone();
                    }
                    else
                    {
                        return wds.CommandTable.Clone();
                    }
                }
            }
            return null;
        }
    }
}
