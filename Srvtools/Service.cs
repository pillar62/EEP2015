using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;

namespace Srvtools
{
    public class Service : InfoOwnerCollectionItem , IGetValues
    {
        #region Constructor

        public Service() : this("", "", false)
        {

        }

        public Service(String serviceName, String delegateName, Boolean nonLogin)
        {
            _serviceName = serviceName;
            _delegateName = delegateName;
            _nonLogin = nonLogin;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Get or set service's name.
        /// </summary>
        [Category("Design")]
        public String ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        /// <summary>
        /// Get or set service's delegatename.
        /// </summary>
        // [Category("Data"), Editor(typeof(DelegateName), typeof(System.Drawing.Design.UITypeEditor))]
        public String DelegateName
        {
            get { return _delegateName; }
            set { _delegateName = value; }
        }

        /// <summary>
        /// Get or set service's nonlogin.
        /// </summary>
        [Category("Design")]
        public Boolean NonLogin
        {
            get { return _nonLogin; }
            set { _nonLogin = value; }
        }

        [Browsable(false)]
        public override string Name
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        #endregion

        #region IGetValues

        public string[] GetValues(string sKind)
        {
            if (string.Compare(sKind, "delegatename", true) == 0)//IgnoreCase
            {
                MessageBox.Show(((ServiceManager)Owner).Site.GetType().ToString());
                MessageBox.Show(((ServiceManager)Owner).OwnerComp.GetType().GetMethod("GetTime", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Name);

                MethodInfo[] infos = ((ServiceManager)Owner).OwnerComp.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                List<string> infosList = new List<string>();
                foreach (MethodInfo info in infos)
                {
                    infosList.Add(info.Name);
                }
                return infosList.ToArray();
            }
            else
                return null;
        }

        #endregion

        #region Method
        /// <summary>
        /// Override the ToString method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _serviceName;
        }
        #endregion

        #region Private Vars

        private String _serviceName = "";
        private String _delegateName = "";
        private Boolean _nonLogin = false;

        #endregion
    }

    public class DelegateName : System.Drawing.Design.UITypeEditor
    {
        // private IWindowsFormsEditorService edSvc;

        public DelegateName()
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
