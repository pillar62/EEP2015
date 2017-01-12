using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using Srvtools;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Web.UI.Design;
using System.IO;
using System.Windows.Forms.Design;

namespace OfficeTools
{
    [Designer(typeof(DataSourceDesigner), typeof(IDesigner))]
    public abstract class WebOfficePlate: WebControl, IOfficePlate
    {
        public WebOfficePlate()
        {
            _DataSource = new DataSourceCollections(this, typeof(DataSourceItem));
            _Tags = new TagCollections(this, typeof(TagItem));
            _WebDataSource = new WebDataSourceCollections(this, typeof(WebDataSourceItem));

            _EmailAddress = string.Empty;
            _EmailTitle = string.Empty;
            _OfficeFile = string.Empty;
            _OutputFileName = string.Empty;
            _OutputPath = string.Empty;
            _PlateMode = PlateModeType.Xml;
            _FilePath = string.Empty;
        }

        private string _OfficeFile;
        /// <summary>
        /// The file used as a template
        /// </summary>
        [Category("Infolight"),
        Description("The template office file")]
        [Browsable(false)]
        public string OfficeFile
        {
            get { return _OfficeFile; }
            set { _OfficeFile = value; }
        }
        private string _OutputPath;
        /// <summary>
        /// The path of file to output
        /// </summary>
        [Category("Infolight"),
        Description("The path of file to output")]
        [Editor(typeof(URLPathEditor), typeof(UITypeEditor))]
        public string OutputPath
        {
            get { return _OutputPath; }
            set { _OutputPath = value; }
        }

        private string _OutputFileName;
        /// <summary>
        /// The name of file to output
        /// </summary>
        [Category("Infolight"),
        Description("The name of file to output")]
        public string OutputFileName
        {
            get { return _OutputFileName; }
            set { _OutputFileName = value; }
        }

        private PlateModeType _PlateMode;
        /// <summary>
        /// The reference mode to plate
        /// </summary>
        [Category("Infolight"),
        Description("The reference mode to plate")]
        public PlateModeType PlateMode
        {
            get { return _PlateMode; }
            set { _PlateMode = value; }
        }


        private string _EmailAddress;
        /// <summary>
        /// The address to email
        /// </summary>
        [Category("Infolight"),
        Description("The address to email")]
        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { _EmailAddress = value; }
        }

        private string _EmailTitle;
        /// <summary>
        /// The title of email
        /// </summary>
        [Category("Infolight"),
        Description("The title of email")]
        public string EmailTitle
        {
            get { return _EmailTitle; }
            set { _EmailTitle = value; }
        }

        private bool _ShowAction;
        /// <summary>
        /// The flag indicates whether show infomation durning the plate process
        /// </summary>
        [Category("Infolight"),
        Description("Specifies whether show infomation durning the plate process")]
        public bool ShowAction
        {
            get { return _ShowAction; }
            set { _ShowAction = value; }
        }

        private bool _MarkException;
        /// <summary>
        /// The flag indicates whether mark a flag in the output file when encounter error
        /// </summary>
        [Category("Infolight"),
        Description("Specifies whether mark a flag in the output file when encounter error")]
        public bool MarkException
        {
            get { return _MarkException; }
            set { _MarkException = value; }
        }

        private string _FilePath;
        /// <summary>
        /// The really name of output file, while outputfilename is not set, system will create a file named by string of time
        /// </summary>
        [Browsable(false)]
        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }

        private WebDataSourceCollections _WebDataSource;
        /// <summary>
        /// The datasource collection used to output
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        [Category("Infolight"),
        Description("The datasources which plate will use to output")]
        public WebDataSourceCollections WebDataSource
        {
            get
            {
                return _WebDataSource;
            }
        }

        #region IOfficePlate Members
        private DataSourceCollections _DataSource;
        /// <summary>
        /// The datasource collection used to output
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true),
        Browsable(false)]
        public DataSourceCollections DataSource
        {
            get
            {
                return _DataSource;
            }
        }

        /// <summary>
        /// The basic function of WordPlate, used to output
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns>The flag indicates whether output is successful</returns>
        public virtual bool Output(int Mode)
        {
            string sfile = Path.GetDirectoryName(this.Page.Request.PhysicalPath) + "\\" + OfficeFile;
            string psypath = this.Page.Server.MapPath(this.OutputPath);
            if (!Directory.Exists(psypath))
            {
                Directory.CreateDirectory(psypath);
            }
            if (OutputFileName.Trim().Length > 0)
            {
                if (this.OutputPath.EndsWith("\\"))
                {
                    this.FilePath = this.OutputPath + this.OutputFileName;
                }
                else
                {
                    this.FilePath = this.OutputPath + "\\" + this.OutputFileName;
                }
                File.Copy(sfile, this.Page.Server.MapPath(FilePath), true);
            }
            else
            {
                DateTime dt = DateTime.Now;
                string strext = Path.GetExtension(sfile);
                if (this.OutputPath.EndsWith("\\"))
                {
                    this.FilePath = this.OutputPath + dt.ToString("yyyyMMddHHmmss") + strext;
                }
                else
                {
                    this.FilePath = this.OutputPath + "\\" + dt.ToString("yyyyMMddHHmmss") + strext;
                }
                File.Copy(sfile, this.Page.Server.MapPath(FilePath));
            }
            return true;
        }

        private static object EventOnAfterOutput = new object();
        /// <summary>
        /// The event ocured when the the process of output accomplished
        /// </summary>
        [Category("Infolight"),
        Description("The event ocured when the the process of output accomplished")]
        public event EventHandler AfterOutput
        {
            add { this.Events.AddHandler(EventOnAfterOutput, value); }
            remove { this.Events.RemoveHandler(EventOnAfterOutput, value); }
        }

        /// <summary>
        /// Trigger the afteroutput event
        /// </summary>
        /// <param name="value">The arguments of event</param>
        public void OnAfterOutput(EventArgs value)
        {
            EventHandler handler = this.Events[EventOnAfterOutput] as EventHandler;
            if (handler != null)
            {
                handler(this, value);
            }
        }


        private TagCollections _Tags;
        /// <summary>
        /// The user-defined tag used to output
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        [Browsable(false)]
        public TagCollections Tags
        {
            get
            {
                return _Tags;
            }
        }
        #endregion

        /// <summary>
        /// The event ocured when the control is load
        /// </summary>
        /// <param name="e">The arguments of event</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitialDataSourceItem();
        }

        internal void InitialDataSourceItem()
        {
            DataSource.Clear();
            foreach (WebDataSourceItem wdi in WebDataSource)
            {
                DataSourceItem di = new DataSourceItem();
                Control datasource = FindControl(wdi.DataSourceID, this.Page);
                if (datasource != null)
                {
                    di.Caption = wdi.Caption;
                    di.DataSource = datasource;
                    di.DataMember = "Table";
                    foreach (DataSourceImageColumnItem item in wdi.ImageColumns)
                    {
                        di.ImageColumns.Add(item);
                    }
                    DataSource.Add(di);
                }
            }
        }

        internal Control FindControl(string ControlID, Control ParentControl)
        {
            if (string.Compare(ParentControl.ID, ControlID, false) == 0)
            {
                return ParentControl;
            }
            else
            {
                foreach (Control ct in ParentControl.Controls)
                {
                    Control ctrtn = FindControl(ControlID, ct);
                    if (ctrtn != null)
                    {
                        return ctrtn;
                    }
                }
                return null;
            }
        }
    }

    /// <summary>
    /// The class of WebDataSourceCollections
    /// </summary>
    public class WebDataSourceCollections : InfoOwnerCollection
    {
        /// <summary>
        /// Create a new instance of WebDataSourceCollections
        /// </summary>
        /// <param name="aOwner">The owner of collections</param>
        /// <param name="aItemType">The type of the item of collections</param>
        public WebDataSourceCollections(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebDataSourceItem))
        {

        }

        /// <summary>
        /// Get data in collections
        /// </summary>
        /// <param name="index">The index of data</param>
        /// <returns>Return data</returns>
        public new WebDataSourceItem this[int index]
        {
            get
            {
                return (WebDataSourceItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebDataSourceItem)
                    {
                        ((WebDataSourceItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((WebDataSourceItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    /// <summary>
    /// The class of WebDataSourceItem
    /// </summary>
    public class WebDataSourceItem : InfoOwnerCollectionItem, IGetValues
    {
        /// <summary>
        /// Create a new instance of WebDataSourceItem
        /// </summary>
        public WebDataSourceItem()
        {
            _DataSourceID = string.Empty;
            _Caption = string.Empty;
            _ImageColumns = new DataSourceImageColumnCollections(this, typeof(DataSourceImageColumnItem));
        }
        private string _Caption;
        /// <summary>
        /// The Id of the datatable
        /// </summary>
        [Category("Infolight"),
        Description("The Id of the dataset")]
        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }

        private string _DataSourceID;
        /// <summary>
        /// The dataset used to output
        /// </summary>
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [Category("Infolight"),
        Description("The dataset used to output")]
        public string DataSourceID
        {
            get { return _DataSourceID; }
            set 
            { 
                _DataSourceID = value;

            }
        }

        private DataSourceImageColumnCollections _ImageColumns;
        /// <summary>
        /// The fields of image
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        [Category("Infolight"),
        Description("The fields of image")]
        public DataSourceImageColumnCollections ImageColumns
        {
            get { return _ImageColumns; }
        }

        public override string Name
        {
            get
            {
                return Caption;
            }
            set
            {

            }
        }

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();

            if (this.Owner is WebOfficePlate && string.Compare(sKind, "DataSourceID",true) == 0)
            {
                ControlCollection cc = (this.Owner as WebOfficePlate).Page.Controls;
                foreach (Control ct in cc)
                {
                    if (ct is WebDataSource)
                    {
                        values.Add(ct.ID);
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

    /// <summary>
    /// The editor of file path in design time
    /// </summary>
    public class URLPathEditor : System.Web.UI.Design.UrlEditor
    {
        public URLPathEditor()
            : base()
        { }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string path = base.EditValue(context, provider, value).ToString();
            path = path.Substring(0, path.LastIndexOf("/") + 1);
            return path;
        }
    }
}
