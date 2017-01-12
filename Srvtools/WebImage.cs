using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.Reflection;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Windows.Forms.Design;

namespace Srvtools
{
    [ToolboxBitmap(typeof(WebImage), "Resources.WebImage.png")]
    public class WebImage : System.Web.UI.WebControls.ImageButton
    {
        public WebImage()
        {
        }

        #region properties
        public enum WebImageType
        { 
            jpeg,
            gif,
            bmp,
            ico,
            png
        }

        public enum WebImageStyle
        {
            ImageField,
            VarCharField
        }

        [Category("InfoLight")]
        public WebImageStyle ImageStyle
        {
            get
            {
                object obj = this.ViewState["ImageStyle"];
                if (obj != null)
                {
                    return (WebImageStyle)obj;
                }
                return WebImageStyle.VarCharField;
            }
            set 
            {
                this.ViewState["ImageStyle"] = value; 
            }
        }

        [Category("InfoLight")]
        [EditorAttribute(typeof(UrlPathEditor), typeof(UITypeEditor))]
        public string DefaultPath
        {
            get
            {
                object obj = this.ViewState["DefaultPath"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["DefaultPath"] = value;

            }
        }
	

        [Category("InfoLight")]
        public WebImageType ImageType
        {
            get
            {
                object obj = this.ViewState["ImageType"];
                if (obj != null)
                {
                    return (WebImageType)obj;
                }
                return WebImageType.jpeg;
            }
            set
            {
                this.ViewState["ImageType"] = value;
            }
        }

        [Category("InfoLight")]
        public bool ReadOnly
        {
            get
            {
                object obj = this.ViewState["ReadOnly"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["ReadOnly"] = value;
                if (this.Site == null && value)
                {
                    this.OnClientClick = "return false;";
                }
            }
        }

        [Category("InfoLight"),
        Editor(typeof(FileUploadEditor), typeof(UITypeEditor))]
        public string FileUploadID
        {
            get
            {
                object obj = this.ViewState["FileUploadID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["FileUploadID"] = value;
            }
        }

        [Category("InfoLight"),
        Browsable(false),
        Bindable(true)]
        public byte[] FileBytes
        {
            get
            {
                object obj = this.ViewState["FileBytes"];
                if (obj != null)
                {
                    return (byte[])obj;
                }
                return null;
            }
            set
            {
                this.ViewState["FileBytes"] = value;
            }
        }
        #endregion

        protected override void OnClick(ImageClickEventArgs e)
        {
            base.OnClick(e);
            if (this.FileUploadID != null && this.FileUploadID != "")
            { 
                Control ctrl = this.NamingContainer.FindControl(this.FileUploadID);
                if (ctrl != null && ctrl is FileUpload)
                {
                    FileUpload fUpload = (FileUpload)ctrl;
                    if(fUpload.HasFile)
                    {
                        this.FileBytes = fUpload.FileBytes;
                        //string fileName = fUpload.FileName;
                        //string imagePath = Page.Server.MapPath(fileName);
                        //fUpload.SaveAs(imagePath);
                        string url = FormatImageUrl(false);
                        if (!string.IsNullOrEmpty(url))
                        {
                            this.ImageUrl = url + "?temp=" + DateTime.Now.Millisecond.ToString();
                        }
                    }
                }
            }
        }

        public override void DataBind()
        {
            try
            {
                base.DataBind();
            }
            catch
            {
                return;
            }
            if (ImageStyle == WebImageStyle.ImageField)
            {
                this.ImageUrl = FormatImageUrl(true) + "?temp=" + DateTime.Now.Millisecond.ToString(); 
            }
        }

        public string FormatImageUrl(bool DataFromDataBind)
        {
            byte[] imageBytes = this.FileBytes;
            string tempPath = "";
            string rtnPath = "";
            if (imageBytes != null)
            {
                if(this.ImageStyle == WebImageStyle.VarCharField)
                {
                    if (DefaultPath != "")
                    {
                        tempPath = Page.Server.MapPath(DefaultPath);
                        rtnPath = DefaultPath;
                    }
                    else
                    {
                        string path = Page.Request.PhysicalPath;
                        path = Directory.GetParent(path).FullName;
                        if (!Directory.Exists(path + "\\TempImage"))
                        {
                            Directory.CreateDirectory(path + "\\TempImage");
                        }
                        tempPath = path + "\\TempImage";
                        rtnPath = "TempImage";
                    }
                    string filename = "";
                    Control ctrl = this.NamingContainer.FindControl(this.FileUploadID);
                    if (ctrl != null && ctrl is FileUpload)
                    {
                        FileUpload fUpload = (FileUpload)ctrl;
                        filename = Path.GetFileName(fUpload.FileName);
                    }
                    tempPath += "\\" + filename;
                    rtnPath += "/" + filename;

                    if (!DataFromDataBind)
                    {
                        if (System.IO.File.Exists(tempPath))
                        {
                            this.Page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), "alert('圖片已經存在，請重新上傳')", true);
                            return null;
                        }
                    }

                }
                else
                {
                    string path = Page.Request.PhysicalPath;
                    path = Directory.GetParent(path).FullName;
                   
                    if (!Directory.Exists(path + "\\TempImage"))
                    {
                        Directory.CreateDirectory(path + "\\TempImage");
                    }
                    if (!DataFromDataBind)
                    {
                        tempPath = Page.Server.MapPath("TempImage\\Image." + this.ImageType.ToString());
                        rtnPath = "TempImage/Image." + this.ImageType.ToString();
                    }
                    
                    else
                    {
                        WebDataSource wds = this.GetDataSource(this.NamingContainer);
                        string pageName = Path.GetFileName(path);
                        string datasourceName = "", datamember = "", keyValue = "";
                        if (wds != null)
                        {
                            datasourceName = wds.UniqueID;
                            datamember = wds.DataMember;
                            object dataItem = this.GetDataItem(this.NamingContainer);
                            if (dataItem != null)
                            {
                                DataRowView rowView = (DataRowView)dataItem;
                                foreach (DataColumn dc in wds.PrimaryKey)
                                {
                                    keyValue += rowView[dc.ColumnName].ToString() + "_";
                                }
                                if (keyValue != "")
                                {
                                    keyValue = keyValue.Substring(0, keyValue.LastIndexOf("_"));
                                }
                            }
                        }
                        if (pageName != null && pageName != "" && datasourceName != "" && datamember != "" && keyValue != "")
                        {
                            string fileName = pageName + "_" + datasourceName + "_" + datamember + "_" + keyValue;
                            tempPath = Page.Server.MapPath("TempImage\\" + fileName + "." + this.ImageType.ToString());
                            rtnPath = "TempImage/" + fileName + "." + this.ImageType.ToString();
                        }
                    }
                }


               
                FileStream stream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.Write);
                stream.Write(imageBytes, 0, imageBytes.Length);
                stream.Flush();
                stream.Close();
            }
            this.ToolTip = rtnPath;
            return rtnPath;
        }

        private WebDataSource GetDataSource(object container)
        {
            WebDataSource wds = null;
            if (container is WebFormView)
            {
                WebFormView formView = (WebFormView)container;
                object obj = formView.GetObjByID(formView.DataSourceID);
                if (obj != null && obj is WebDataSource)
                {
                    wds = (WebDataSource)obj;
                }
            }
            else if (container is WebDetailsView)
            {
                WebDetailsView detView = (WebDetailsView)container;
                object obj = detView.GetObjByID(detView.DataSourceID);
                if (obj != null && obj is WebDataSource)
                {
                    wds = (WebDataSource)obj;
                }
            }
            else if (container is GridViewRow)
            {
                GridViewRow row = (GridViewRow)container;
                WebGridView gdView = (WebGridView)row.NamingContainer;
                object obj = gdView.GetObjByID(gdView.DataSourceID);
                if (obj != null && obj is WebDataSource)
                {
                    wds = (WebDataSource)obj;
                }
            }
            return wds;
        }

        private object GetDataItem(object container)
        {
            object dataitem = null;
            if (container is WebFormView)
            {
                WebFormView formView = (WebFormView)container;
                dataitem = formView.DataItem;
            }
            else if (container is WebDetailsView)
            {
                WebDetailsView detView = (WebDetailsView)container;
                dataitem = detView.DataItem;

            }
            else if (container is GridViewRow)
            {
                GridViewRow row = (GridViewRow)container;
                dataitem = row.DataItem;
            }
            return dataitem;
        }
    }

    public class UrlPathEditor : UrlEditor
    {
        public UrlPathEditor()
            : base()
        { }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string path = base.EditValue(context, provider, value).ToString();
            path = path.Substring(0, path.LastIndexOf("/") + 1);
            return path;
        }
    }

    public class FileUploadEditor : UITypeEditor
    {
        public FileUploadEditor()
            : base()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance != null)
            {
                ControlCollection ctrlList = ((Control)context.Instance).NamingContainer.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is FileUpload)
                    {
                        objName.Add(ctrl.ID);
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
