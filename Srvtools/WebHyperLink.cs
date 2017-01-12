using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.Data;
using System.Collections;
using System.Xml;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Resources;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Web;
using System.Drawing;


namespace Srvtools
{
    [ToolboxBitmap(typeof(WebHyperLink), "Resources.WebHyperLink.ico")]
    public class WebHyperLink: LinkButton, IGetValues
    {
        public WebHyperLink()
        {
            _urlname = "";
            _dialogbox = false;
            _useclientclick = false;
            _urlwidth = 800;
            _urlheight = 600;
            _urlleft = 0;
            _urltop = 0;
        }

        private string _urlname;
        [EditorAttribute(typeof(UrlEditor), typeof(UITypeEditor))]
        [Category("Infolight"),
        Description("Path of page to open")]
        public string HyperLinkURL
        {
            get
            {
                return _urlname;
            }
            set
            {
                if (value.IndexOf("~") != -1)
                {
                    value = value.Replace("~", "..");
                }
                _urlname = value;
            }
        }

        private int _urlwidth;
        [Category("Infolight"),
        Description("Width of page to open")]
        public int URLWidth
        {
            get
            {
                return _urlwidth;
            }
            set
            {
                _urlwidth = value;
            }
        }

        private int _urlheight;
        [Category("Infolight"),
        Description("Height of page to open")]
        public int URLHeight
        {
            get
            {
                return _urlheight;
            }
            set
            {
                _urlheight = value;
            }
        }

        private int _urlleft;
        [Category("Infolight"),
        Description("Height of page to open")]
        public int URLLeft
        {
            get
            {
                return _urlleft;
            }
            set
            {
                _urlleft = value;
            }
        }

        private int _urltop;
        [Category("Infolight"),
        Description("Height of page to open")]
        public int URLTop
        {
            get
            {
                return _urltop;
            }
            set
            {
                _urltop = value;
            }
        }


        [Category("Infolight"),
        Description("The ID of WebDataSource which the control is bound to")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get
            {
                object obj = this.ViewState["DataSourceID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["DataSourceID"] = value;
            }
        }

        //private WebLinkColumnCollection _sourcecolumns;
        //[PersistenceMode(PersistenceMode.InnerProperty),
        //DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        //TypeConverter(typeof(CollectionConverter)),
        //NotifyParentProperty(true)]
        //[Category("Infolight"),
        //Description("The columns which InfoHyperLink is applied to")]
        //public WebLinkColumnCollection SourceColumns
        //{
        //    get
        //    {
        //        return _sourcecolumns;
        //    }
        //}
        
        [Category("Infolight"),
        Description("The columns which WebHyperLink is applied to")]
        [Editor(typeof(WebHyperLinkEditor), typeof(UITypeEditor))]
        public string SourceColumns
        {
            get
            {
                object obj = this.ViewState["SourceColumn"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["SourceColumn"] = value;
            }
        }

        private bool _dialogbox;
        [Category("Infolight"),
        Description("Indicates whether the page is shown in dialog mode")]
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

        [Browsable(false)]
        public bool Cancel
        {
            get
            {
                return (ViewState["Cancel"] == null? false: Convert.ToBoolean(ViewState["Cancel"].ToString()));
            }
            set
            {
                ViewState["Cancel"] = value;
            }
        }


        [Category("Infolight"),
        Description("Param to send to the page to open")]
        public string ItemParam
        {
            get
            {
                object obj = this.ViewState["ItemParam"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["ItemParam"] = value;
            }
        }


        [Browsable(false)]
        public string ColumnText
        {
            get
            {
                object obj = this.ViewState["ColumnText"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["ColumnText"] = value;
            }
        }

        private bool _useclientclick;
        [Category("Infolight"),
        Description("Indicates whether the client-side script will execute when user click")]
        public bool UseClientClick
        {
            get
            {
                return _useclientclick;
            }
            set
            {
                _useclientclick = value;
            }
        }

        public override void DataBind()
        {
            if (!this.DesignMode)
            {
                if (SourceColumns != "")
                {
                    string[] columnname = SourceColumns.Split(';');
                    int count = columnname.Length;
                    if (this.NamingContainer is GridViewRow)
                    {  
                        GridViewRow grv = this.NamingContainer as GridViewRow;
                        if (grv.DataItem != null)
                        {
                            DataRowView drv = (DataRowView)grv.DataItem;
                            for (int i = 0; i < count; i++)
                            {
                                this.ColumnText += HttpUtility.UrlEncode(drv[columnname[i]].ToString()) + ";";
                            }
                            if (this.ColumnText != "")
                            {
                                this.ColumnText = this.ColumnText.Substring(0, this.ColumnText.LastIndexOf(';'));
                            }
                        }
                    }
                    if (this.NamingContainer is WebDetailsView)
                    {
                        this.ColumnText = "";
                        WebDetailsView wdv = this.NamingContainer as WebDetailsView;
                        if (wdv.DataItem != null)
                        {
                            DataRowView drv = (DataRowView)wdv.DataItem;
                            for (int i = 0; i < count; i++)
                            {
                                this.ColumnText += HttpUtility.UrlEncode(drv[columnname[i]].ToString()) + ";";
                            }
                            if (this.ColumnText != "")
                            {
                                this.ColumnText = this.ColumnText.Substring(0, this.ColumnText.LastIndexOf(';'));
                            }
                        }
                    }
                    else if (this.NamingContainer is WebFormView)
                    {
                        this.ColumnText = "";
                        WebFormView wfv = this.NamingContainer as WebFormView;
                        if (wfv.DataItem != null)
                        {
                            DataRowView drv = (DataRowView)wfv.DataItem;
                            for (int i = 0; i < count; i++)
                            {
                                this.ColumnText += HttpUtility.UrlEncode(drv[columnname[i]].ToString()) + ";";
                            }
                            if (this.ColumnText != "")
                            {
                                this.ColumnText = this.ColumnText.Substring(0, this.ColumnText.LastIndexOf(';'));
                            }
                        }
                    }
                } 
            }
            base.DataBind();
            if (UseClientClick)
            {
                if (DialogBox)
                {
                    this.OnClientClick = "window.showModalDialog('../InnerPages/frmWebHyperLinkDialog.aspx?WebHyperLinkURL=" + this.HyperLinkURL
                      + "&WebHyperLinkText=" + this.ColumnText.Replace("'", "\\'") + "&itemparam=" + this.ItemParam.Replace("'", "\\'")
                      + "','','dialogwidth=" + this.URLWidth.ToString() + "px;dialogheight=" + this.URLHeight.ToString()
                        + "px,dialogleft=" + this.URLLeft.ToString() + "px,dialogtop=" + this.URLTop.ToString() + "px'); return false;";
                }
                else
                {
                    this.OnClientClick = "form = window.open('" + this.HyperLinkURL
                        + "?WebHyperLinkText=" + this.ColumnText.Replace("'", "\\'") + "&itemparam=" + this.ItemParam.Replace("'", "\\'")
                        + "','HyperLinkpage','width=" + this.URLWidth.ToString() + ",height=" + this.URLHeight.ToString()
                        + ",left=" + this.URLLeft.ToString() + ",top=" + this.URLTop.ToString() + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no' ); return false;";
                }
            }
        }

        public void CancelClick()
        {
            this.Cancel = true; 
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (!this.Cancel)
            {
                string script = string.Empty;
                if (DialogBox)
                {
                    script = "<script>window.showModalDialog('../InnerPages/frmWebHyperLinkDialog.aspx?WebHyperLinkURL=" + this.HyperLinkURL
                        + "&WebHyperLinkText=" + this.ColumnText.Replace("'", "\\'") + "&itemparam=" + this.ItemParam.Replace("'", "\\'")
                        + "','','dialogwidth=" + this.URLWidth.ToString() + "px;dialogheight=" + this.URLHeight.ToString()
                        + "px,dialogleft=" + this.URLLeft.ToString() + "px,dialogtop=" + this.URLTop.ToString() + "px')</script>";
                }
                else
                {
                    script = "<script>form = window.open('" + this.HyperLinkURL
                        + "?WebHyperLinkText=" + this.ColumnText.Replace("'", "\\'") + "&itemparam=" + this.ItemParam.Replace("'", "\\'")
                        + "','HyperLinkpage','width=" + this.URLWidth.ToString() + ",height=" + this.URLHeight.ToString()
                        + ",left=" + this.URLLeft.ToString() + ",top=" + this.URLTop.ToString() + ",scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no');</script>";
                }
                this.Page.ClientScript.RegisterStartupScript(typeof(string), string.Empty, script);
            }
            else
            {
                this.Cancel = false;
            }
        }

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this  is WebHyperLink)
            {
                if (string.Compare(sKind, "datasourceid", true) == 0)//IgnoreCase
                {
                    ControlCollection ctrlList = ((WebHyperLink)this).Page.Controls;
                    foreach (Control ctrl in ctrlList)
                    {
                        if (ctrl is WebDataSource)
                        {
                            values.Add(ctrl.ID);
                        }
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

    public class WebHyperLinkEditor : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;

        public WebHyperLinkEditor()
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
                ArrayList lst = new ArrayList();
                WebHyperLink whl = (WebHyperLink)context.Instance;
                WebDataSource ds = (WebDataSource)whl.Page.FindControl(whl.DataSourceID);
                if (ds != null)
                {
                    if (ds.DesignDataSet == null)
                    {
                        WebDataSet wds = WebDataSet.CreateWebDataSet(ds.WebDataSetID);
                        if (wds != null)
                        {
                            ds.DesignDataSet = wds.RealDataSet;
                        }
                    }
                    if (ds.DesignDataSet != null && ds.DesignDataSet.Tables.Contains(ds.DataMember))
                    {
                        foreach (DataColumn column in ds.DesignDataSet.Tables[ds.DataMember].Columns)
                        {
                            lst.Add(column.ColumnName);
                        }
                    }
                }

                frmWebHyperLink fwhl = new frmWebHyperLink(value.ToString(), lst);
                fwhl.ShowDialog();
                value = fwhl.selectedColumn;
            }
            return value;
        }
    }
}
