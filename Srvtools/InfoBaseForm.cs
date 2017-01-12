using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace Srvtools
{
    public partial class InfoForm : Form, IDataModule, IInfoForm
    {
        public InfoForm()
        {
            InitializeComponent();
        }

        public bool GetMultiInstance()
        {
            return MultiInstance;
        }

        public void SetPackageForm(string pkg, string frm)
        {
            this.PackageName = pkg;
            this.FormName = frm;
        }

        public string GetPackageName()
        {
            return PackageName;
        }

        public string GetFormName()
        {
            return FormName;
        }

        private bool fMultiInstance = false;
        public bool MultiInstance
        {
            get
            {
                return fMultiInstance;
            }
            set
            {
                fMultiInstance = value;
            }
        }

        private string fPackageName = "";
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PackageName
        {
            get{
                return fPackageName;}
            set
            {
                fPackageName = value;
            }
        }

        private string fFormName = "";
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FormName
        {
            get
            {
                return fFormName;
            }
            set
            {
                fFormName = value;
            }
        }

        private string fItemParamters;
        public string ItemParamters
        {
            get { return fItemParamters; }
            set { fItemParamters = value; }
        }

        private bool showMenuText;

        public bool ShowMenuText
        {
            get { return showMenuText; }
            set { showMenuText = value; }
        }

        private StringDictionary _flItemParamters;
        [Browsable(false)]
        public StringDictionary fLItemParamters
        {
            get
            {
                return _flItemParamters;
            }
            set
            {
                _flItemParamters = value;
            }
        }

        ToolStripMenuItem item;
        private void InfoForm_Load(object sender, EventArgs e)
        {
            if (CliUtils.fLogMenuOpenForm)
            {
                CliUtils.LogToSystem("Open Form",  this.GetType().Namespace + ":" + this.Name, false, 7, 0);
            }

            SetOwnerComponent();
            if (this.ParentForm != null && this.ParentForm.Name == "frmClientMain" && item == null) 
            {
                item = new ToolStripMenuItem();
                item.Text = this.Text;
                item.Click += new EventHandler(frmmenu_Click);
                if (this.ParentForm.MainMenuStrip != null)
                    (this.ParentForm.MainMenuStrip.Items[this.ParentForm.MainMenuStrip.Items.Count - 2] as ToolStripMenuItem).DropDownItems.Add(item);
                helpProviderInfolight.HelpNamespace = Application.StartupPath + "\\" + CliUtils.fCurrentProject + "\\" + this.GetType().Namespace + ".html";
                helpProviderInfolight.SetHelpNavigator(this, HelpNavigator.TableOfContents);
            }
        }

        void frmmenu_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        public object GetIntfObject(Type intfType)
        {
            object oRet = null;

            ReflectionPermission reflectionPerm1 = new ReflectionPermission(PermissionState.None);
            reflectionPerm1.Flags = ReflectionPermissionFlag.AllFlags;

            if (intfType.IsInterface)
            {
                Type type = this.GetType();
                FieldInfo[] myFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                for (int i = 0; i < myFields.Length; i++)
                {
                    object newobj = myFields[i].GetValue(this);
                    if ((null != newobj) && (null != newobj.GetType().GetInterface(intfType.Name)))
                    {
                        return newobj;
                    }
                }
            }

            return oRet;
        }

        public ArrayList GetIntfObjects(Type intfType)
        {
            ArrayList aRet = new ArrayList();

            ReflectionPermission reflectionPerm1 = new ReflectionPermission(PermissionState.None);
            reflectionPerm1.Flags = ReflectionPermissionFlag.AllFlags;

            if (intfType.IsInterface)
            {
                Type type = this.GetType();
                FieldInfo[] myFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                for (int i = 0; i < myFields.Length; i++)
                {
                    object newobj = myFields[i].GetValue(this);
                    if ((null != newobj) && (null != newobj.GetType().GetInterface(intfType.Name)))
                    {
                        aRet.Add(newobj);
                    }
                }
            }
            return aRet;
        }

        /// <summary>
        /// Find Control(Recursive) in this form
        /// </summary>
        /// <param name="controlType">Type of control to find</param>
        /// <param name="controlCollection">List of control found</param>
        public void FindControl(Type controlType, List<Control> controlCollection)
        {
            FindControl(this, controlType, controlCollection);
        }

        /// <summary>
        /// Find Control(Recursive)
        /// </summary>
        /// <param name="startControl">Control to start</param>
        /// <param name="controlType">Type of control to find</param>
        /// <param name="controlCollection">List of control found</param>
        private void FindControl(Control startControl, Type controlType, List<Control> controlCollection)
        {
            if (startControl.GetType() == controlType || startControl.GetType().BaseType == controlType)
            {
                controlCollection.Add(startControl);
            }
            if (startControl.Controls.Count == 0)
            {
                return;
            }
            else
            {
                foreach (Control ct in startControl.Controls)
                {
                    FindControl(ct, controlType, controlCollection);
                }
            }
        }

        public object GetObjectByClassName(string sClassName)
        {
            object oRet = null;
            return oRet;
        }

        public ArrayList GetObjectsByClassName(string sClassName)
        {
            ArrayList oRet = new ArrayList();
            return oRet;
        }

        public void SetClientInfo(object[] _ClientInfo)
        {
            //ClientInfo = _ClientInfo;
            //This function is not used...
        }

        public object[] GetClientInfo()
        {
            return null;
        }

        public void SetOwnerComponent()
        {
            ArrayList myList = GetIntfObjects(typeof(IFindContainer));
            for (int i = 0; i < myList.Count; i++)
            {
                if (myList[i] is InfoSecurity)
                {
                    ((IFindContainer)(myList[i])).OwnerComp = this;
                }
            }

            for (int i = 0; i < myList.Count; i++)
            {
                if (myList[i].GetType() != typeof(InfoSecurity))
                {
                    ((IFindContainer)(myList[i])).OwnerComp = this;
                }
                if (myList[i] is AnyQuery && (myList[i] as AnyQuery).AnyQueryID == String.Empty)
                {
                    (myList[i] as AnyQuery).PackageForm = this.PackageName + "." + this.FormName;
                }
            }
        }

        private void InfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CliUtils.fLogMenuOpenForm)
            {
                CliUtils.LogToSystem("Close Form", this.GetType().Namespace + ":" + this.Name, false, 7, 0);
            }

            if (this.ParentForm != null && this.ParentForm.Name == "frmClientMain")
            {
                if (this.ParentForm.MainMenuStrip != null && item != null)
                {
                    (this.ParentForm.MainMenuStrip.Items[this.ParentForm.MainMenuStrip.Items.Count - 2] as ToolStripMenuItem).DropDownItems.Remove(item);
                    item = null;
                }
            }
        }
    }
}