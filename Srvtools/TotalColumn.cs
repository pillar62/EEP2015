using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Srvtools
{
    public enum totalMode
    {
        none = 0,
        sum = 1,
        count = 2,
        max = 3,
        min = 4,
        average = 5
    }

    public class TotalColumn : InfoOwnerCollectionItem, IGetValues
    {
        public TotalColumn()
            : this("", totalMode.sum, true, TotalAlign.left)
        {
        }

        public TotalColumn(string columnname, totalMode totalmode, bool showtotal, TotalAlign totalAlign)
        {
            _ColumnName = columnname;
            _TotalMode = totalmode;
            _ShowTotal = showtotal;
            _TotalAlignment = totalAlign;
        }

        private string _ColumnName;
        [Editor(typeof(ColumnNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
            set
            {
                _ColumnName = value;
            }
        }

        [Category("Infolight")]
        private totalMode _TotalMode;
        public totalMode TotalMode
        {
            get
            {
                return _TotalMode;
            }
            set
            {
                _TotalMode = value;
            }
        }

        [Category("Infolight")]
        private bool _ShowTotal;
        public bool ShowTotal
        {
            get
            {
                return _ShowTotal;
            }
            set
            {
                _ShowTotal = value;
            }
        }

        public enum TotalAlign
        {
            right,
            center,
            left
        }
        private TotalAlign _TotalAlignment;
        public TotalAlign TotalAlignment
        {
            get
            {
                return _TotalAlignment;
            }
            set
            {
                _TotalAlignment = value;
            }
        }

        public override string Name
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }

        public override string ToString()
        {
            return _ColumnName;
        }

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            if (string.Compare(sKind, "columnname", true) == 0)//IgnoreCase
            {
                if (this.Owner is InfoDataGridView)
                {
                    int i = ((InfoDataGridView)this.Owner).Columns.Count;
                    string[] arrItems = new string[i];
                    for (int j = 0; j < i; j++)
                    {
                        arrItems[j] = ((InfoDataGridView)this.Owner).Columns[j].Name;
                    }
                    return arrItems;
                }
                else
                    return null;
            }
            else
                return null;
        }
        #endregion
    }

    public class ColumnNameEditor : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;

        public ColumnNameEditor()
        {
        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // Uses the IWindowsFormsEditorService to display a
            // drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            IGetValues aItem = (IGetValues)context.Instance;
            if (edSvc != null)
            {
                StringListSelector mySelector = new StringListSelector(edSvc, aItem.GetValues(context.PropertyDescriptor.Name));
                string strValue = (string)value;
                if (mySelector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }

}
