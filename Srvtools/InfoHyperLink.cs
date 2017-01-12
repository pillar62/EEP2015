using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Reflection;
using System.IO;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Drawing;

namespace Srvtools
{
    [ToolboxBitmap(typeof(InfoHyperLink), "Resources.InfoHyperLink.ico")]
    public class InfoHyperLink : LinkLabel,ISupportInitialize
    {
        public InfoHyperLink()
        {
            _dialogbox = true;
            _dllpath = "";
            _dllname = "";
            _formname = "";
            _showform = null;
            _sourcecolumns = new LinkColumnCollection(this, typeof(LinkColumn));
        }

        public InfoHyperLink(IContainer container):this()
        {
            container.Add(this);
        }

        private string _dllpath;
        [Browsable(false)]
        public string DLLPath
        {
            get
            {
                return _dllpath;
            }
            set
            {
                _dllpath = value;
            }
        }

        private string _dllname;
        [Editor(typeof(DLLNameEditor),typeof(UITypeEditor))]
        [Category("Infolight"),
        Description("Name of assembly file")]
        public string DLLName
        {
            get
            {
                return _dllname;
            }
            set
            {
                _dllname = value;
            }
        }

        private string _formname;
        [Category("Infolight"),
        Description("The class name of the form in assembly file")]
        public string FormName
        {
            get
            {
                return _formname;
            }
            set
            {
                //Remarked by lily 2006/10/30 for cannot build referenced dll for using
                _formname = value;
                //if (this.DesignMode)
                //{
                //    if (_dllname == "")
                //    {
                //        MessageBox.Show("Select an Assembly file first.");
                //    }
                //    else
                //    {
                //        if (value != "")
                //        {
                //            if (File.Exists(DLLPath + "\\" + DLLName + ".dll"))
                //            {
                //                Assembly ass = Assembly.LoadFrom(DLLPath + "\\" + DLLName + ".dll");
                //                Type assType = ass.GetType(DLLName + "." + value);
                //                if (assType != null)
                //                {
                //                    _formname = value;
                //                }
                //                else
                //                {
                //                    MessageBox.Show(string.Format("Can't find form: {0}\nin Assembly file.", value));
                //                }
                //            }
                //            else
                //            {
                //                MessageBox.Show(DLLPath + "\\" + DLLName + ".dll" + "\nAssembly file doesn't exsit.",this.Site.Name + " warning");
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    _formname = value;
                //}
            }
        }

        private InfoBindingSource _bindingsource;
        [AttributeProvider(typeof(IListSource))]
        [RefreshProperties(RefreshProperties.All)]
        [Category("Infolight"),
        Description("The InfoBindingSource which the control is bound to")]
        public InfoBindingSource BindingSource
        {
            get
            {
                return _bindingsource;
            }
            set
            {
                if (value != _bindingsource)
                {
                    _bindingsource = value;
                }
            }     
        }

        private LinkColumnCollection _sourcecolumns;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Infolight"),
        Description("The columns which InfoHyperLink is applied to")]
        public LinkColumnCollection SourceColumns
        {
            get
            {
                return _sourcecolumns;
            }
            set
            {
                _sourcecolumns = value;
            }
        }

        private bool _dialogbox;
        [Category("Infolight"),
        Description("Indicates whether the form is shown in dialog mode")]
        public bool DialogBox
        {
            get
            {
                return _dialogbox;
            }
            set
            {
                _dialogbox = value;
            }
        }

        private Form _showform;
        [Browsable(false)]
        public Form ShowForm
        {
            get
            {
                return _showform;
            }
            set
            {
                _showform = value;
            }
        }

        private object _columntext;
        [Browsable(false)]
        public object ColumnText
        {
            get
            {
                return _columntext;
            }
            set
            {
                _columntext = value;
            }
        }

        public void DoClick()
        {
            OnLinkClicked(new LinkLabelLinkClickedEventArgs(null,MouseButtons.Left));
        }

        protected override void OnLinkClicked(LinkLabelLinkClickedEventArgs e)
        {
            InfoTranslate itl = new InfoTranslate();
            base.OnLinkClicked(e);
            if (ShowForm == null)
            {
                if(!File.Exists( Application.StartupPath + "\\" + CliUtils.fCurrentProject + "\\" + DLLName + ".dll"))
                {
                    bool suc =  CliUtils.DownLoad( DLLPath + "\\" + DLLName + ".dll" ,
                        Application.StartupPath + "\\" + CliUtils.fCurrentProject + "\\" + DLLName + ".dll");
                    if (!suc)
                    {
                        MessageBox.Show("Assembly file doesn't exsit.");
                        return;
                    }     
                }
                Assembly ass = Assembly.LoadFrom(DLLPath + "\\" + DLLName + ".dll");
                Type assType = ass.GetType(DLLName + "." + FormName);
                if(assType != null)
                {
                    object obj = Activator.CreateInstance(assType);
                    ShowForm = (Form)obj;

                    Type typeform = ShowForm.GetType();
                    FieldInfo[] fi = typeform.GetFields(BindingFlags.Instance
                                                          | BindingFlags.NonPublic
                                                          | BindingFlags.DeclaredOnly);
                    for (int i = 0; i < fi.Length; i++)
                    {
                        object objvalue = fi[i].GetValue(ShowForm);
                        if (objvalue != null && objvalue is InfoTranslate)
                        {
                            itl = objvalue as InfoTranslate;
                            break;
                        }
                    }
                    ShowForm.Load += delegate(object sender1, EventArgs e1)
                    {
                        itl.Text = this.ColumnText;
                        itl.SetWhere();
                    };
                    ShowForm.FormClosed += delegate
                    {
                        this.ShowForm = null;
                    };
                    if (DialogBox)
                    {
                        ShowForm.ShowDialog();
                    }
                    else
                    {
                        ShowForm.Show();
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("Can't find form: {0}\nin Assembly file.", FormName));
                }  
            }
            else
            {
                Type typeform = ShowForm.GetType();
                FieldInfo[] fi = typeform.GetFields(BindingFlags.Instance
                                                      | BindingFlags.NonPublic
                                                      | BindingFlags.DeclaredOnly);
                for (int i = 0; i < fi.Length; i++)
                {
                    object objvalue = fi[i].GetValue(ShowForm);
                    if (objvalue != null && objvalue is InfoTranslate)
                    {
                        itl = objvalue as InfoTranslate;
                        break;
                    }
                }
                if (ShowForm.WindowState == FormWindowState.Minimized)
                {
                    ShowForm.WindowState = FormWindowState.Normal;
                }
                itl.Text = this.ColumnText;
                itl.SetWhere();
                ShowForm.Show();
                ShowForm.Activate();
            }     
        }

        #region ISupportInitialize Members

        public void BeginInit()
        {
            
        }

        public void EndInit()
        {
            if (!this.DesignMode)
            {
                this.FindForm().FormClosing += delegate
                {
                    if (this.ShowForm != null)
                    {
                        this.ShowForm.Close();
                    }
                };
            }
        }

        #endregion
    }
    public class DLLNameEditor : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;

        public DLLNameEditor()
        {
        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog, drop down dialog, or no UI outside of the properties window.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        // Displays the UI for value selection.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // Uses the IWindowsFormsEditorService to display a drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            //IGetValues aItem = (IGetValues)context.Instance;
            if (edSvc != null)
            {
                OpenFileDialog dfd = new OpenFileDialog();
                dfd.Filter = "Assembly Files(*.dll)| *.dll";
                dfd.Title = "Select an Assembly File:";
                if ((context.Instance as InfoHyperLink).DLLPath != string.Empty)
                {
                    dfd.InitialDirectory = (context.Instance as InfoHyperLink).DLLPath;
                    dfd.FileName = dfd.InitialDirectory + "\\" + value;
                }
                if (dfd.ShowDialog() == DialogResult.OK)
                {
                    (context.Instance as InfoHyperLink).DLLPath = Path.GetDirectoryName(dfd.FileName);
                    string filename = Path.GetFileName(dfd.FileName);
                    value = filename.Substring(0, filename.LastIndexOf('.'));
                }
            }
            return value;
        }
    }

    public class LinkColumnCollection: InfoOwnerCollection
    {   
        public LinkColumnCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(LinkColumn))
        {

        }
        public new LinkColumn this[int index]
        {
			get
            {
                return (LinkColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is LinkColumn)
                    {
                        //原来的Collection设置为0
                        ((LinkColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((LinkColumn)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class LinkColumn : InfoOwnerCollectionItem, IGetValues
    {
        public LinkColumn()
        {
            _columnname = "";
        }

        private string _name;
        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _columnname;
        [Editor(typeof(FieldNameEditor),typeof(UITypeEditor))]
        public string ColumnName
        {
            get
            {
                return _columnname;
            }
            set
            {
                _columnname = value;
                if (value != null && value != "")
                {
                    _name = value;
                }
            }
        }

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is InfoHyperLink)
            {
                if (string.Compare(sKind, "columnname", true) == 0)//IgnoreCase
                {
                    InfoHyperLink ihl = this.Owner as InfoHyperLink;
                    if (ihl.BindingSource != null && ihl.BindingSource.DataSource != null)
                    {
                        if (ihl.BindingSource.DataMember != null && ihl.BindingSource.DataMember != "")
                        {
                            int colNum = ((InfoDataSet)ihl.BindingSource.DataSource).RealDataSet.Tables[ihl.BindingSource.DataMember].Columns.Count;
                            for (int i = 0; i < colNum; i++)
                            {
                                values.Add(((InfoDataSet)ihl.BindingSource.DataSource).RealDataSet.Tables[ihl.BindingSource.DataMember].Columns[i].ColumnName);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("select a BindingSource first.");
                    }
                }
            }
            if (values.Count > 0)
            {
                int i = values.Count;
                retList = new string[i];
                for (int j = 0; j < i; j++)
                {
                    retList[j] = values[j];
                }
            }
            return retList;
        }
        #endregion
    }
}
