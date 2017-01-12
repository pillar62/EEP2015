using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using Infolight.EasilyReportTools.Design;
using System.Drawing.Design;
using Infolight.EasilyReportTools.UI;
using System.Data;
using Infolight.EasilyReportTools.Tools;
using System.ComponentModel.Design;
using System.Reflection;
using Infolight.EasilyReportTools.DataCenter;
using System.Xml;
using System.IO;
using Srvtools;

namespace Infolight.EasilyReportTools
{
    /// <summary>
    /// Component of report
    /// </summary>
    [Designer(typeof(EasilyReportDesigner), typeof(IDesigner))]
    public class EasilyReport : Component, IReport
    {
        /// <summary>
        /// Create a new instance of easilyreport
        /// </summary>
        public EasilyReport()
        {
            headerItems = new ReportItemCollection(this);
            footerItems = new ReportItemCollection(this);
            fieldItems = new DataSourceItemCollection(this);
            this.reportID = ComponentInfo.DefaultReportID + DateTime.Now.ToString("yyyyMMdd");
            this.reportName = "";
            //this.reportID = ComponentInfo.DefaultReportID;
            //this.reportName = ComponentInfo.EasilyReport + DateTime.Now.ToString("yyyyMMdd");
            this.filePath = @"C:\" + ComponentInfo.EasilyReport + DateTime.Now.ToString("yyyyMMdd") + ".xls";
        }

        #region IReport Members
        private string reportID;
        /// <summary>
        /// Get or set id of report
        /// </summary>
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
        /// <summary>
        /// Get or set name of report
        /// </summary>
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
        /// <summary>
        /// Get or set description of report
        /// </summary>
        [Category("Infolight"),
        Description("Indicates the description of the report which will be used in the header or footer.")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private BindingSource headerBindingSource;
        /// <summary>
        /// Get or set bindingSource of header and footer
        /// </summary>
        [Category("Infolight"),
        Description("Indentifies the Master bindingSource in the Master-Detail mode.")]
        public BindingSource HeaderBindingSource
        {
            get { return headerBindingSource; }
            set { headerBindingSource = value; }
        }

        private List<BindingSourceItem> dataSource = new List<BindingSourceItem>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<BindingSourceItem> DataSource
        {
            get { return dataSource; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int DataSourceCount
        {
            get { return DataSource.Count; }
        }

        private object headerDataSource;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object HeaderDataSource
        {
            get 
            { 
                return headerDataSource == null ? headerBindingSource : headerDataSource; 
            }
            set { headerDataSource = value; }
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
        /// <summary>
        /// Get or set font of header
        /// </summary>
        [Category("Infolight"),
        Description("The font used to display text in the header of the report.")]
        public Font HeaderFont
        {
            get
            {
                return headerFont;
            }
            set
            {
                headerFont = value;
            }
        }

        private ReportItemCollection headerItems;
        /// <summary>
        /// Get the items of header
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ReportItemCollection HeaderItems
        {
            get { return headerItems; }
        }


        private Font footerFont = new Font("Simsun", 9.0f);
        /// <summary>
        /// Get or set the font of footer
        /// </summary>
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
        /// <summary>
        /// Get the items of footer
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ReportItemCollection FooterItems
        {
            get { return footerItems; }
        }

        private Font fieldFont = new Font("Simsun", 9.0f);
        /// <summary>
        /// Get of set the font of field
        /// </summary>
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
        /// <summary>
        /// Get the items of field
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DataSourceItemCollection FieldItems
        {
            get 
            {
                return fieldItems;
            }
        }
	
        private ReportFormat format = new ReportFormat();
        /// <summary>
        /// Get the format of report
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ReportFormat Format
        {
            get { return format; }
        }


        private ParameterItemCollection parameters = new ParameterItemCollection();
        /// <summary>
        /// Get the parameter collection of report
        /// </summary>
        [Category("Infolight"),
        Description("Specifies constants or functions as parameter passed into the report.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ParameterItemCollection Parameters
        {
            get { return parameters; }
        }


        private ImageItemCollection images = new ImageItemCollection();
        /// <summary>
        /// Get the image collection of report
        /// </summary>
        [Category("Infolight"),
        Description("Specifies the images used in the report.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ImageItemCollection Images
        {
            get { return images; }
        }

        private int currentPageIndex;
        [Browsable(false)]
        public int CurrentPageIndex
        {
            get { return currentPageIndex; }
        }

        private int pageCount;
        [Browsable(false)]
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

        private MailConfig mailSetting = new MailConfig();
        /// <summary>
        /// Get mail setting of report
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MailConfig MailSetting
        {
            get { return mailSetting; }
        }


        private string eEPAlias;
        /// <summary>
        /// Get or set the alias of System.Data.IDbConnection used by report.
        /// </summary>
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

        private string filePath;
        /// <summary>
        /// The really name of output file, while outputfilename is not set, system will create a file named by string of time
        /// </summary>
        [Category("Infolight"),
        Description("The really name of output file, while outputfilename is not set, system will create a file named by string of time")]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        IReport IReport.Copy()
        {
            IReport report = new EasilyReport();
            ((IReport)this).CopyTo(report);
            return report;
        }

        void IReport.CopyTo(IReport report)
        {
            if (!this.Equals(report))
            {
                ((EasilyReport)report).HeaderBindingSource = this.HeaderBindingSource;
                ((EasilyReport)report).ContainerControl = this.ContainerControl;
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
                ((EasilyReport)report).DataSource.Clear();
                foreach (BindingSourceItem item in this.DataSource)
                {
                    ((EasilyReport)report).DataSource.Add(item);
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

        public void Execute()
        {
            Execute(false);
        }

        /// <summary>
        /// Designer EasilyReport or export it.
        /// </summary>
        /// <param name="openDesignForm">Open Design From or not</param>
        public void Execute(bool openDesignForm)
        {
            if (openDesignForm)
            {
                fmReportExport fm = new fmReportExport(this);
                fm.ShowDialog();
            }
            else
            {
                IReportExport exporter = null;
                ExecutionResult execResult;
                execResult = new ExecutionResult();

                if (this.Format.ExportFormat == ReportFormat.ExportType.Excel)
                {
                    exporter = new ExcelReportExporter(this, ExportMode.Export, false);
                }
                else
                {
                    exporter = new PdfReportExporter(this, ExportMode.Export, false);
                }

                execResult = exporter.CheckValidate();

                if (execResult.Status)
                {
                    fmProgress fp = new fmProgress("Easily Report (" + this.Format.ExportFormat.ToString() + " Format)", exporter, this);
                    fp.Visible = false;
                    fp.ShowDialog();
                }
                else
                {
                    MessageBox.Show(execResult.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public bool Load(string fileName)
        {
            DBGateway dbGateway = new DBGateway(this);
            return dbGateway.LoadTemplate(fileName);
        }

        public void SaveAs(string fileName)
        {
            DBGateway dbGateway = new DBGateway(this);
            dbGateway.SaveTemplate(fileName);
        }

        #endregion

        private ContainerControl containerControl;
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ContainerControl ContainerControl
        {
            get { return containerControl; }
            set { containerControl = value; }
        }

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;
                if (value == null)
                {
                    return;
                }

                IDesignerHost host = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (host != null)
                {
                    IComponent componentHost = host.RootComponent;
                    if (componentHost is ContainerControl)
                    {
                        ContainerControl = componentHost as ContainerControl;
                    }
                }
            }
        }

    }

    public class BindingSourceItem
    {
        private InfoBindingSource bindingSource;

        public InfoBindingSource BindingSource
        {
            get { return bindingSource; }
            set { bindingSource = value; }
        }
    }
}