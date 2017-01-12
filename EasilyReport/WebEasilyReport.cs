using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Drawing.Text;
using System.Collections;
using System.Data;
using Infolight.EasilyReportTools.Tools;
using Infolight.EasilyReportTools.DataCenter;
using Infolight.EasilyReportTools.Design;
using System.Drawing.Design;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using Infolight.EasilyReportTools.UI;
using System.Reflection;
using Infolight.EasilyReportTools.Config;
using AjaxControlToolkit;
using System.IO;
using System.Threading;
using System.Net.Mail;
using System.Xml;
using Srvtools;

namespace Infolight.EasilyReportTools
{
    [Designer(typeof(WebEasilyReportDesigner), typeof(IDesigner))]
    public partial class WebEasilyReport : WebControl, IReport, INamingContainer, IReportGetValues
    {
        internal DBGateway dbGateway;
        internal string selectedTemplateName = null;
        internal BinarySerialize serializer;
        internal FontConverter fontConvert;
        private IRender render = null;
        

        public WebEasilyReport()
        {
            headerItems = new ReportItemCollection(this);
            footerItems = new ReportItemCollection(this);

            fieldItems = new DataSourceItemCollection(this);
            dataSource = new WebDataSourceItemCollection(this);
            mailSetting = new MailConfig();
            format = new ReportFormat();
            parameters = new ParameterItemCollection();
            images = new ImageItemCollection();
            dbGateway = new DBGateway(this);
            serializer = new BinarySerialize();

            this.reportID = ComponentInfo.DefaultReportID + DateTime.Now.ToString("yyyyMMdd");
            this.reportName = "";

            fontConvert = new FontConverter();
            Visible = false;

            if (this.UIType == EasilyReportUIType.AspNet)
            {
                render = new AspNetRender(this);
            }
            else
            {
                render = new ExtJsRender(this);
            }
        }

        [DefaultValue(false)]
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }

        #region IReport Members
        private string reportID;
        [Category("Infolight"),
        Description("Indicates the ID of the Report to record into the system table.")]
        public string ReportID
        {
            get
            {
                return reportID;
            }
            set
            {
                reportID = value;
            }
        }

        private string reportName;
        [Category("Infolight"),
        Description("Indicates the Name of the Report to record into the system table.")]
        public string ReportName
        {
            get
            {
                return reportName;
            }
            set
            {
                reportName = value;
            }
        }

        private string description;
        [Category("Infolight"),
        Description("Indicates the description of the report which will be used in the header or footer.")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private WebDataSourceItemCollection dataSource;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Data")]
        public WebDataSourceItemCollection DataSource
        {
            get { return dataSource; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int DataSourceCount
        {
            get { return DataSource.Count; }
        }

        [Category("Infolight"),
        Description("Indentifies the Master DataSource in the Master-Detail mode.")]
        [Editor(typeof(Infolight.EasilyReportTools.Design.PropertyDropDownEditor), typeof(UITypeEditor))]
        public string HeaderDataSourceID
        {
            get
            {
                return (string)ViewState["HeaderDataSourceID"];
            }
            set
            {
                ViewState["HeaderDataSourceID"] = value;
            }
        }

        private object headerDataSource;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object HeaderDataSource
        {
            get
            {
                if (headerDataSource == null)
                {
                    if (!string.IsNullOrEmpty(HeaderDataSourceID))
                    {
                        headerDataSource = this.Parent.FindControl(HeaderDataSourceID);

                        if (headerDataSource == null)
                        {
                            headerDataSource = this.Page.FindControl(HeaderDataSourceID);
                        }
                    }
                }
                return headerDataSource;
            }
            set
            {
                headerDataSource = value;
            }
        }

        private bool headerRepeat = true;
        [Category("Infolight"),
        Description("The value indicating whether repeat header and footer when page changes")]
        public bool HeaderRepeat
        {
            get { return headerRepeat; }
            set { headerRepeat = value; }
        }

        private Font headerFont = new Font("Simsun", 9.0f);
        [Category("Infolight"),
        Description("The font used to display text in the header of the report.")]
        public Font HeaderFont
        {
            get { return headerFont; }
            set { headerFont = value; }
        }

        private ReportItemCollection headerItems;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ReportItemCollection HeaderItems
        {
            get { return headerItems; }
        }

        private Font footerFont = new Font("Simsun", 9.0f);
        [Category("Infolight"),
        Description("The font used to display text in the footer of the report.")]
        public Font FooterFont
        {
            get
            {
                return footerFont;
            }
            set
            {
                footerFont = value;
            }
        }

        private ReportItemCollection footerItems;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ReportItemCollection FooterItems
        {
            get { return footerItems; }
        }

        private Font fieldFont = new Font("Simsun", 9.0f);
        [Category("Infolight"),
        Description("The font used to display text in the field of the report.")]
        public Font FieldFont
        {
            get
            {
                return fieldFont;
            }
            set
            {
                fieldFont = value;
            }
        }

        private DataSourceItemCollection fieldItems;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public DataSourceItemCollection FieldItems
        {
            get { return fieldItems; }
        }

        private ReportFormat format;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ReportFormat Format
        {
            get { return format; }
        }
        private ParameterItemCollection parameters;
        [Category("Infolight"),
        Description("Specifies constants or functions as parameter passed into the report.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ParameterItemCollection Parameters
        {
            get { return parameters; }
        }

        private ImageItemCollection images;
        [Category("Infolight"),
        Description("Specifies the images used in the report.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ImageItemCollection Images
        {
            get { return images; }
        }

        private MailConfig mailSetting;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public MailConfig MailSetting
        {
            get { return mailSetting; }
        }

        private string targetID;
        [Browsable(false)]
        public string TargetID
        {
            get { return targetID; }
            set { targetID = value; }
        }


        private int currentPageIndex;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentPageIndex
        {
            get { return currentPageIndex; }
        }

        private int pageCount;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageCount
        {
            get { return pageCount; }
        }

        void IReport.SetCurrentPageIndex(int pageIndex)
        {
            currentPageIndex = pageIndex;
        }

        void IReport.SetPageCount(int count)
        {
            pageCount = count;
        }

        private string eEPAlias;
        /// <summary>
        /// Gets or sets the alias of System.Data.IDbConnection used by this instance of the EasilyReport.
        /// </summary>
        [Category("Infolight"),
        Description("The alias of System.Data.IDbConnection used by this instance of the EasilyReport")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string EEPAlias
        {
            get
            {
                if (string.IsNullOrEmpty(eEPAlias))
                {
                    if (this.DesignMode || string.IsNullOrEmpty(CliUtils.fLoginDB))
                    {
                        eEPAlias = GetSystemDBName();
                    }
                    else
                    {
                        object[] ret = CliUtils.CallMethod("GLModule", "GetSplitSysDB2", new object[] { CliUtils.fLoginDB });
                        eEPAlias = ret[1].ToString();
                    }
                }
                return eEPAlias;
            }
        }

        private string GetSystemDBName()
        {
            XmlDocument DBXML = new XmlDocument();
            string sysDB = "";

            if (File.Exists(SystemFile.DBFile))
            {
                DBXML.Load(SystemFile.DBFile);
                XmlNode aNode = DBXML.DocumentElement.FirstChild;
                XmlNode sysNode = null;

                while ((null != aNode))
                {
                    if (string.Compare(aNode.Name, "SYSTEMDB", true) == 0)//IgnoreCase
                    {
                        sysNode = aNode;
                        sysDB = sysNode.InnerText.Trim();
                        break;
                    }
                    aNode = aNode.NextSibling;
                }
            }
            return sysDB;
        }

        private OutputModeType outputMode;
        /// <summary>
        /// The action after output file
        /// </summary>
        [Category("Infolight"),
        Description("The action after output file")]
        public OutputModeType OutputMode
        {
            get { return outputMode; }
            set { outputMode = value; }
        }

        private string filePath;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        IReport IReport.Copy()
        {
            //WebEasilyReport report = new WebEasilyReport();
            //CopyTo(report);
            //return report;
            return this;
        }

        void IReport.CopyTo(IReport report)
        {
            if (!this.Equals(report))
            {
                ((WebEasilyReport)report).HeaderDataSourceID = this.HeaderDataSourceID;
                report.ReportID = this.ReportID;
                report.ReportName = this.ReportName;
                report.Description = this.Description;

                report.FilePath = this.FilePath;
                report.HeaderFont = this.HeaderFont;

                report.HeaderItems.Clear();
                foreach (ReportItem item in this.HeaderItems)
                {
                    report.HeaderItems.Add(item.Copy());
                }
                report.FieldFont = this.FieldFont;
                ((WebEasilyReport)report).DataSource.Clear();
                foreach (WebDataSourceItem item in this.DataSource)
                {
                    ((WebEasilyReport)report).DataSource.Add(item);
                }
                report.FieldItems.Clear();
                foreach (DataSourceItem item in this.FieldItems)
                {
                    report.FieldItems.Add(item.Copy());
                }
                report.FooterFont = this.FooterFont;
                report.FooterItems.Clear();
                foreach (ReportItem item in this.FooterItems)
                {
                    report.FooterItems.Add(item.Copy());
                }
                report.Parameters.Clear();
                foreach (ParameterItem item in this.Parameters)
                {
                    report.Parameters.Add(item.Copy());
                }
                report.Images.Clear();
                foreach (ImageItem item in this.Images)
                {
                    report.Images.Add(item.Copy());
                }

                this.Format.CopyTo(report.Format);
                this.MailSetting.CopyTo(report.MailSetting);
                report.OutputMode = this.OutputMode;
            }
        }

        public void Execute()
        {
            string strUrl = String.Empty;
            string strPath = GetOutputPath(this.Format.ExportFormat);

            strUrl = GetOutputUrl(this.Format.ExportFormat);

            this.FilePath = strPath;

            IReportExport exporter = null;
            if (this.Format.ExportFormat == ReportFormat.ExportType.Excel)
            {
                exporter = new ExcelReportExporter(this, ExportMode.Export, false);
                exporter.FileName = this.FilePath;
            }
            else
            {
                exporter = new PdfReportExporter(this, ExportMode.Export, false);
            }

            exporter.Export();

            AspNetScriptsProvider.DownLoadFile(this);
        }

        public new bool Load(string fileName)
        {
            bool rtn = dbGateway.LoadTemplate(fileName);
            TransImages();
            return rtn;
        }

        internal void TransImages()
        {
            foreach (ImageItem item in this.Images)
            {
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    string path = this.Page.MapPath(item.ImageUrl);
                    string directory = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    try
                    {
                        item.Image.Save(path); //原文件存在并被占用时会无法保存,暂时忽略这个错误
                    }
                    catch { }
                }
            }
        }

        public void SaveAs(string fileName)
        {
            dbGateway.SaveTemplate(fileName);
        }


        #region IReportGetValues Members

        public string[] GetValues(string propertyName)
        {
            if (this.Page != null)
            {
                List<string> list = new List<string>();
                foreach (Control ctrl in this.Page.Controls)
                {
                    if (ctrl is WebDataSource)
                    {
                        list.Add(ctrl.ID);
                    }
                }
                return list.ToArray();

            }

            return new string[] { };
        }

        #endregion

       

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureChildControls();//make   sure   CreateChildControls   is   called   before   LoadViewState   occurs,   for   example,   in   Page_Init,   call  
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.Page.IsPostBack)
            {
                render.InitialView(this);
            }
        }

        
        internal string GetOutputPath(ReportFormat.ExportType outputFormat)
        {
            string path = this.Page.MapPath(this.Page.AppRelativeVirtualPath);
            string directory = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            string extension = String.Empty;
            string dir = String.Empty;
            if (outputFormat == ReportFormat.ExportType.Excel)
            {
                extension = "xls";
            }
            else
            {
                extension = "pdf";
            }

            dir = directory + "\\" + outputFormat.ToString() + "Doc";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            path = string.Format("{0}\\" + outputFormat.ToString() + "Doc\\{1}", directory, string.Format("{0}-{1:yyMMddHHmmss}", filename, DateTime.Now));
            path = Path.ChangeExtension(path, extension);
            return path;
        }

        internal string GetOutputUrl(ReportFormat.ExportType outputFormat)
        {
            string strUrl = this.Page.AppRelativeVirtualPath.Substring(0, this.Page.AppRelativeVirtualPath.LastIndexOf("/") + 1);
            strUrl += outputFormat.ToString() + "Doc/";
            strUrl += Path.GetFileName(this.GetOutputPath(outputFormat));
            return strUrl;
        }
        #endregion

        private EasilyReportUIType uIType = EasilyReportUIType.AspNet;
        [Category("Infolight")]
        public EasilyReportUIType UIType
        {
            get { return uIType; }
            set { uIType = value; }
        }

        private MultiView multiview;
        internal MultiView MultiView
        {
            get
            {
                if (multiview == null)
                {
                    multiview = this.FindControl("multiview") as MultiView;
                }
                return multiview;
            }
        }

        internal int LastViewIndex
        {
            get { return ViewState["LastViewIndex"] == null ? -1 : (int)ViewState["LastViewIndex"]; }
            set { ViewState["LastViewIndex"] = value; }
        }

        [Browsable(false)]
        public int LastIndex
        {
            get
            {
                return this.ViewState["LastIndex"] == null ? -1 : (int)this.ViewState["LastIndex"];
            }
            set { this.ViewState["LastIndex"] = value; }
        }

        internal string FontButtonID
        {
            get { return (string)ViewState["FontButtonID"]; }
            set { ViewState["FontButtonID"] = value; }
        }

        internal Hashtable FontTable
        {
            get
            {
                return (Hashtable)ViewState["FontTable"];
            }
            set
            {
                ViewState["FontTable"] = value;
            }
        }

        internal Hashtable ItemTable
        {
            get
            {
                return (Hashtable)ViewState["ItemTable"];
            }
            set
            {
                ViewState["ItemTable"] = value;
            }
        }

        internal Hashtable ItemIndexTable
        {
            get
            {
                return (Hashtable)ViewState["ItemIndex"];
            }
            set
            {
                ViewState["ItemIndex"] = value;
            }
        }

        internal Hashtable ImageItemTable
        {
            get
            {
                return (Hashtable)ViewState["ImageItemTable"];
            }
            set
            {
                ViewState["ImageItemTable"] = value;
            }
        }

        internal Hashtable ImageItemIndexTable
        {
            get
            {
                return (Hashtable)ViewState["ImageItemIndexTable"];
            }
            set
            {
                ViewState["ImageItemIndexTable"] = value;
            }
        }

        internal ArrayList TempFieldItems
        {
            get
            {
                return (ArrayList)ViewState["TempFieldItems"];
            }
            set
            {
                ViewState["TempFieldItems"] = value;
            }
        }

        internal string PictureChangeMode
        {
            get { return (string)ViewState["PictureChangeMode"]; }
            set { ViewState["PictureChangeMode"] = value; }
        }

    }

    public class WebDataSourceItemCollection : IList, ICollection, IEnumerable
    {
        /// <summary>
        /// Create a new instance of field item collection
        /// </summary>
        /// <param name="rpt">owner report</param>
        public WebDataSourceItemCollection(WebEasilyReport rpt)
        {
            report = rpt;
        }

        private WebEasilyReport report;
        /// <summary>
        /// Get the owner report
        /// </summary>
        public WebEasilyReport Report
        {
            get { return report; }
        }

        private ArrayList list = new ArrayList();
        /// <summary>
        /// Add a webdatasource item to collection
        /// </summary>
        /// <param name="item">field item</param>
        /// <returns>index of item</returns>
        public int Add(WebDataSourceItem item)
        {
            if (item != null)
            {
                item.SetCollection(this);
            }
            return list.Add(item);
        }

        public void Clear()
        {
            foreach (WebDataSourceItem item in list)
            {
                item.SetCollection(null);
            }
            list.Clear();
        }

        public bool Contains(WebDataSourceItem item)
        {
            return list.Contains(item);
        }

        public int IndexOf(WebDataSourceItem item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, WebDataSourceItem item)
        {
            if (item != null)
            {
                item.SetCollection(this);
            }
            list.Insert(index, item);
        }

        public bool IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        public void Remove(WebDataSourceItem item)
        {
            if (item != null)
            {
                item.SetCollection(null);
            }
            list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this[index].SetCollection(null);
            list.RemoveAt(index);
        }

        public WebDataSourceItem this[int index]
        {
            get
            {
                return (WebDataSourceItem)list[index];
            }
            set
            {
                this[index].SetCollection(null);
                list[index] = value;
                this[index].SetCollection(this);
            }
        }

        #region IList Members

        int IList.Add(object value)
        {
            if (value is WebDataSourceItem)
            {
                ((WebDataSourceItem)value).SetCollection(this);
                return list.Add(value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        void IList.Clear()
        {
            foreach (WebDataSourceItem item in list)
            {
                item.SetCollection(null);
            }
            list.Clear();
        }

        bool IList.Contains(object value)
        {
            return list.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return list.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            if (value is WebDataSourceItem)
            {
                ((WebDataSourceItem)value).SetCollection(this);
                list.Insert(index, value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        bool IList.IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        bool IList.IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            if (value is WebDataSourceItem)
            {
                ((FieldItem)value).SetCollection(null);
                list.Remove(value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        void IList.RemoveAt(int index)
        {
            this[index].SetCollection(null);
            list.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                if (value is WebDataSourceItem)
                {
                    ((WebDataSourceItem)list[index]).SetCollection(null);
                    list[index] = value;
                    ((WebDataSourceItem)list[index]).SetCollection(this);
                }
                else
                {
                    throw new ArgumentException("value");
                }
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            list.CopyTo(array, index);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsSynchronized
        {
            get { return list.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return list.SyncRoot; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion
    }

    public class WebDataSourceItem : IReportGetValues
    {
        private WebDataSourceItemCollection collection;
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal WebDataSourceItemCollection Collection
        {
            get { return collection; }
        }

        internal void SetCollection(WebDataSourceItemCollection col)
        {
            collection = col;
        }

        private string datasourceID;
        [Category("Infolight"),
        Description("The id of webdatasource.")]
        [Editor(typeof(Infolight.EasilyReportTools.Design.PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string DataSourceID
        {
            get { return datasourceID; }
            set { datasourceID = value; }
        }

        #region IReportGetValues Members

        public string[] GetValues(string propertyName)
        {
            if (this.Collection != null)
            {
                WebEasilyReport report = this.Collection.Report;
                if (report != null && report.Page != null)
                {
                    List<string> list = new List<string>();
                    foreach (Control ctrl in report.Page.Controls)
                    {
                        if (ctrl is WebDataSource)
                        {
                            list.Add(ctrl.ID);
                        }
                    }
                    return list.ToArray();

                }
            }
            return new string[] { };
        }

        #endregion
    }

    public enum EasilyReportUIType
    { 
        AspNet,
        ExtJs
    }

}