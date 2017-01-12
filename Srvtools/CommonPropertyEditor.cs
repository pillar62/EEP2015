using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;

namespace Srvtools
{
    #region COMMON_PROPERTYEDITOR
    public class StringListSelector
    {
        private ListBox StringListBox = null;
        private string[] fSelectedList = null;
        private IWindowsFormsEditorService fEditService = null;

        private void ListClicked(object sender, EventArgs e)
        {
            if (null != fEditService)
            {
                fEditService.CloseDropDown();
            }
        }

        public StringListSelector(IWindowsFormsEditorService edSvc, string[] SelectedList)
        {
            fSelectedList = SelectedList;
            fEditService = edSvc;
        }

        public bool Execute(ref string strSelected)
        {
            if (null == fEditService) return false;
            if (null == fSelectedList) return false;
            StringListBox = new ListBox();
            int iPos = -1;
            if (fSelectedList != null)
            {
                foreach (string s in fSelectedList)
                {
                    int iCur = StringListBox.Items.Add(s);
                    if (s.Equals(strSelected)) iPos = iCur;
                }
            }
            if (-1 != iPos) StringListBox.SelectedIndex = iPos;

            StringListBox.Click += new EventHandler(ListClicked);
            fEditService.DropDownControl(StringListBox);
            if (null != StringListBox.SelectedItem)
            {
                strSelected = StringListBox.SelectedItem.ToString();
                return true;
            }
            else
            {
                strSelected = "";
                return false;
            }
        }
    }

    public class RemoteNameSelector
    {
        //private ListBox StringListBox = null;
        private string[] fSelectedList = null;
        private IWindowsFormsEditorService fEditService = null;

        private void ListClicked(object sender, EventArgs e)
        {
            if (null != fEditService)
            {
                fEditService.CloseDropDown();
            }
        }

        public RemoteNameSelector(IWindowsFormsEditorService edSvc, string[] SelectedList)
        {
            fSelectedList = SelectedList;
            fEditService = edSvc;
        }

        public bool Execute(ref string strSelected)
        {
            if (null == fEditService) return false;
            if (null == fSelectedList) return false;

            PERemoteName form = new PERemoteName(fSelectedList, strSelected);
            if (form.ShowDialog() == DialogResult.OK)
            {
                strSelected = form.RemoteName;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    #endregion COMMON_PROPERTYEDITOR

    public class PropertyDropDownEditor : System.Drawing.Design.UITypeEditor
    {
        // private IWindowsFormsEditorService edSvc;

        public PropertyDropDownEditor()
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
                try
                {
                    StringListSelector mySelector = new StringListSelector(edSvc, aItem.GetValues(context.PropertyDescriptor.Name));
                    string strValue = (string)value;
                    if (mySelector.Execute(ref strValue)) value = strValue;
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message, "Error",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return value;
        }
    }
}
