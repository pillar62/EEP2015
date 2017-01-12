using System;
using System.Collections.Generic;
using System.Text;
using Srvtools;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.IO;
using System.Data;
using System.ComponentModel;
using System.Collections;
using OfficeTools.Design;

namespace OfficeTools
{
    /// <summary>
    /// The base class of Excel Plate and Word Plate
    /// </summary>
    [Designer(typeof(OfficePlateDesigner), typeof(IDesigner))]
    public class OfficePlate: InfoBaseComp, IOfficePlate
    {
        /// <summary>
        /// Create a new instance of OfficePlate
        /// </summary>
        public OfficePlate()
        {
            _Tags = new TagCollections(this, typeof(TagItem));
            _DataSource = new DataSourceCollections(this, typeof(DataSourceItem));

            _EmailAddress = string.Empty;
            _EmailTitle = string.Empty;
            _OfficeFile = string.Empty;
            _OutputFileName = string.Empty;
            _OutputPath = string.Empty;
            _OutputMode = OutputModeType.None;
            _PlateMode = PlateModeType.Xml;
            _FilePath = string.Empty;
        }

        private DataSourceCollections _DataSource;
        /// <summary>
        /// The datasource collection used to output
        /// </summary>
        [Category("Infolight"),
        Description("The datasources which plate will use to output")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DataSourceCollections DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
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

        private TagCollections _Tags;
        /// <summary>
        /// The user-defined tag used to output
        /// </summary>
        [Category("Infolight"),
        Description("The user-defined Tag palte will use to output")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TagCollections Tags
        {
            get { return _Tags; }
            set { _Tags = value; }
        }

        private string _OutputPath;
        /// <summary>
        /// The path of file to output
        /// </summary>
        [Category("Infolight"),
        Description("The path of file to output")]
        [Editor(typeof(PathEditor), typeof(UITypeEditor))]
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

        private OutputModeType _OutputMode;
        /// <summary>
        /// The action after output file
        /// </summary>
        [Category("Infolight"),
        Description("The action after output file")]
        public OutputModeType OutputMode
        {
            get { return _OutputMode; }
            set { _OutputMode = value; }
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

        public bool _MarkException;
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

        private bool _OutPutOverWrite;
        /// <summary>
        /// The flag indicates whether overwrite the output the file without confirm
        /// </summary>
        [Category("Infolight"),
        Description("Specifies whether overwrite the output the file without confirm")]
        public bool OutPutOverWrite
        {
            get { return _OutPutOverWrite; }
            set { _OutPutOverWrite = value; }
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

	    /// <summary>
        /// The basic function of OfficePlate, used to output
	    /// </summary>
	    /// <param name="Mode"></param>
	    /// <returns>The flag indicates whether output is successful</returns>
        public virtual bool Output(int Mode)
        { 
            CliUtils.DownLoadModule(OfficeFile, true);
            string sfile = Application.StartupPath + "\\" + CliUtils.fCurrentProject + "\\" + OfficeFile;
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
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
                if (File.Exists(this.FilePath))
                {
                    if (OutPutOverWrite || MessageBox.Show("File: " + this.OutputFileName + " has already existed\nWould you like to overwrite it?"
                        , "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        File.Copy(sfile, FilePath, true);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    File.Copy(sfile, FilePath);
                }
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
                File.Copy(sfile, FilePath);
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

        internal Control FindControl(string ControlName, Control ParentControl)
        {
            if (string.Compare(ParentControl.Name, ControlName, false) == 0)
            {
                return ParentControl;
            }
            else
            {
                foreach (Control ct in ParentControl.Controls)
                {
                    Control ctrtn = FindControl(ControlName, ct);
                    if (ctrtn != null)
                    {
                        return ctrtn;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// The enum of output mode of officeplate
        /// </summary>
        public enum OutputModeType
        { 
            None,
            Launch,
            Email
        }
    }

    /// <summary>
    /// The class of TagCollections
    /// </summary>
    public class TagCollections: InfoOwnerCollection
    {
        /// <summary>
        /// Create a new instance of TagCollections
        /// </summary>
        /// <param name="aOwner">The owner of collections</param>
        /// <param name="aItemType">The type of the item of collections</param>
        public TagCollections(Object aOwner, Type aItemType)
            : base(aOwner, typeof(TagItem))
        {

        }

        /// <summary>
        /// Get data in collections
        /// </summary>
        /// <param name="index">The index of data</param>
        /// <returns>Return data</returns>
        public new TagItem this[int index]
        {
            get
            {
                return (TagItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is TagItem)
                    {
                        ((TagItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((TagItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    /// <summary>
    /// The class of tagitem
    /// </summary>
    public class TagItem : InfoOwnerCollectionItem
    {
        /// <summary>
        /// Create a new instance of TagItem
        /// </summary>
        public TagItem()
        {
            _DataField = string.Empty;
            _Exp = string.Empty;
            _Format = string.Empty;
        }

        private string _DataField;
        /// <summary>
        /// The name of tag
        /// </summary>
        [Category("Infolight"),
        Description("The name of tag")]
        public string DataField
        {
            get { return _DataField; }
            set { _DataField = value; }
        }

        public override string Name
        {
            get
            {
                return _DataField;
            }
            set
            {
                _DataField = value;
            }
        }

        private string _Exp;
        /// <summary>
        /// The expression of tag to get value
        /// </summary>
        [Category("Infolight"),
        Description("The expression of tag")]
        [Editor(typeof(ExpressionEditor), typeof(UITypeEditor))]
        public string Exp
        {
            get { return _Exp; }
            set { _Exp = value; }
        }

        private string _Format;
        /// <summary>
        /// The format of tag to output
        /// </summary>
        /// 
        [Category("Infolight"),
        Description("The format of Tag to output")]
        public string Format
        {
            get { return _Format; }
            set { _Format = value; }
        }
    }

    /// <summary>
    /// The class of DataSourceCollections
    /// </summary>
    public class DataSourceCollections : InfoOwnerCollection
    {
        /// <summary>
        /// Create a new instance of DataSourceCollections
        /// </summary>
        /// <param name="aOwner">The owner of collections</param>
        /// <param name="aItemType">The type of the item of collections</param>
        public DataSourceCollections(Object aOwner, Type aItemType)
            : base(aOwner, typeof(DataSourceItem))
        {

        }

        /// <summary>
        /// Get data in collections
        /// </summary>
        /// <param name="index">The index of data</param>
        /// <returns>Return data</returns>
        public new DataSourceItem this[int index]
        {
            get
            {
                return (DataSourceItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is DataSourceItem)
                    {
                        ((DataSourceItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((DataSourceItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    /// <summary>
    /// The class of DataSourceItem
    /// </summary>
    public class DataSourceItem : InfoOwnerCollectionItem,IGetValues
    {
        /// <summary>
        /// Create a new instance of DataSourceItem
        /// </summary>
        public DataSourceItem()
        {
            _DataSource = new InfoDataSet();
            _Caption = string.Empty;
            _DataMember = string.Empty;
            _ImageColumns = new DataSourceImageColumnCollections(this, typeof(DataSourceImageColumnItem));
        }

        private string _Caption;
        /// <summary>
        /// The Id of the datatable
        /// </summary>
        [Category("Infolight"),
        Description("The Id of the datatable")]
        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }

        private object _DataSource;
        /// <summary>
        /// The dataset used to output
        /// </summary>
        [AttributeProvider(typeof(IListSource))]
        [RefreshProperties(RefreshProperties.All)]
        [Category("Infolight"),
        Description("The dataset used to output")]
        public object DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
        }

        private string _DataMember;
        /// <summary>
        /// The datamember of dataset
        /// </summary>
        [Editor(typeof(FieldNameEditor),typeof(UITypeEditor))]
        [Category("Infolight"),
        Description("The datamember of dataset")]
        public string DataMember
        {
            get { return _DataMember; }
            set { _DataMember = value; }
        }

        private DataSourceImageColumnCollections _ImageColumns;
        /// <summary>
        /// The fields of image
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Infolight"),
        Description("The fields of image")]
        public DataSourceImageColumnCollections ImageColumns
        {
            get { return _ImageColumns; }
            set { _ImageColumns = value; }
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

        /// <summary>
        /// The function used to get the tablecollection from a dataset, an infodataset or an infobindingsource
        /// </summary>
        /// <returns>The collection of datatable</returns>
        public Hashtable GetTable()
        {
            Hashtable listtable = new Hashtable();
            if (DataSource is InfoDataSet)
            {
                for (int i = 0; i < (DataSource as InfoDataSet).RealDataSet.Tables.Count; i++)
                {
                    listtable.Add((DataSource as InfoDataSet).RealDataSet.Tables[i].TableName 
                        ,(DataSource as InfoDataSet).RealDataSet.Tables[i].DefaultView);
                }
            }
            else if(DataSource is DataSet)
            {
                for (int i = 0; i < (DataSource as DataSet).Tables.Count; i++)
                {
                    listtable.Add((DataSource as DataSet).Tables[i].TableName,(DataSource as DataSet).Tables[i].DefaultView);
                }
            }
            else if (DataSource is InfoBindingSource)
            {
                if ((DataSource as InfoBindingSource).List as DataView != null)
                {
                    listtable.Add(((DataSource as InfoBindingSource).List as DataView).Table.TableName
                        , (DataSource as InfoBindingSource).List as DataView);
                }
                else
                {
                    InfoDataSet dataset = (DataSource as InfoBindingSource).GetDataSource();
                    if (dataset != null)
                    {
                        string tableName = (DataSource as InfoBindingSource).GetTableName();
                        listtable.Add(tableName, dataset.RealDataSet.Tables[tableName].DefaultView);
                    }
                }
            }
            else if (DataSource is WebDataSource)
            {
                if ((DataSource as WebDataSource).Site == null)
                {
                     listtable.Add("Table", (DataSource as WebDataSource).View);
                }
                else
                {
                    if ((DataSource as WebDataSource).DesignDataSet != null)
                    {
                        listtable.Add("Table", (DataSource as WebDataSource).DesignDataSet.Tables[(DataSource as WebDataSource).DataMember].DefaultView);
                    }
                    else
                    {
                        listtable.Add("Table", (DataSource as WebDataSource).GetSchema().DefaultView);//bug
                    }
                }
            }
            return listtable;
        }

        #region IGetValues Members
        /// <summary>
        /// Get the list of value of property
        /// </summary>
        /// <param name="sKind">Name of property</param>
        /// <returns>List of value of property</returns>
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is OfficePlate && string.Compare(sKind, "Datamember",true) == 0)
            {
                Hashtable dtc = this.GetTable();
                foreach (string var in dtc.Keys)
                {
                    values.Add(var);
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
    /// The class of DataSourceImageColumnCollections
    /// </summary>
    public class DataSourceImageColumnCollections : InfoOwnerCollection
    {
        /// <summary>
        /// Create a new instance of DataSourceImageColumnCollections
        /// </summary>
        /// <param name="aOwner">The owner of Collections</param>
        /// <param name="aItemType">The type of the item of Collections</param>
        public DataSourceImageColumnCollections(Object aOwner, Type aItemType)
            : base(aOwner, typeof(DataSourceImageColumnItem))
        {

        }

        /// <summary>
        /// Get data in collections
        /// </summary>
        /// <param name="index">The index of data</param>
        /// <returns>Return data</returns>
        public new DataSourceImageColumnItem this[int index]
        {
            get
            {
                return (DataSourceImageColumnItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is DataSourceImageColumnItem)
                    {
                        ((DataSourceImageColumnItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((DataSourceImageColumnItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }

        /// <summary>
        /// Get the index of item
        /// </summary>
        /// <param name="ColumnName">The name of item</param>
        /// <returns>The index of item</returns>
        public new int IndexOf(string ColumnName)
        {
            int index = -1;
            for (int i = 0; i < this.Count; i++)
            {
                if (string.Compare((this[i] as DataSourceImageColumnItem).ColumnName, ColumnName, true) == 0)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }

    /// <summary>
    /// The class of DataSourceImageColumnItem
    /// </summary>
    public class DataSourceImageColumnItem : InfoOwnerCollectionItem, IGetValues
    {
        /// <summary>
        /// Create a new instance of DataSourceImageColumnItem
        /// </summary>
        public DataSourceImageColumnItem()
        {
            _ColumnName = string.Empty;
            _DefaultPath = string.Empty;
        }

        private string _ColumnName;
        /// <summary>
        /// The name of field
        /// </summary>
        [Category("Infolight"),
        Description("The name of field to store the data of image")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ColumnName
        {
            get { return _ColumnName; }
            set 
            { 
                _ColumnName = value;
            }
        }

        private string _DefaultPath;
        /// <summary>
        /// The path to store the image
        /// </summary>
        [Category("Infolight"),
        Description("The path to store the image")]
        [Editor(typeof(PathEditor), typeof(UITypeEditor))]
        public string DefaultPath
        {
            get { return _DefaultPath; }
            set { _DefaultPath = value; }
        }

        public override string Name
        {
            get
            {
                return _ColumnName;
            }
            set
            {

            }
        }

        #region IGetValues Members
        /// <summary>
        /// Get the list of value of property
        /// </summary>
        /// <param name="sKind">Name of property</param>
        /// <returns>List of value of property</returns>
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is DataSourceItem && string.Compare(sKind, "ColumnName", true) == 0)
            {
                DataSourceItem di = this.Owner as DataSourceItem;
                Hashtable hashtable = di.GetTable();
                if(hashtable.ContainsKey(di.DataMember))
                {
                    DataTable table = (hashtable[di.DataMember] as DataView).Table;
                    foreach (DataColumn dc in table.Columns)
                    {
                        values.Add(dc.ColumnName);
                    }
                }
            }
            else if (this.Owner is WebDataSourceItem && string.Compare(sKind, "ColumnName", true) == 0)
            { 
                WebDataSourceItem wdsi = this.Owner as WebDataSourceItem;
                WebOfficePlate plate = wdsi.Owner as WebOfficePlate;
                DataSourceItem di = new DataSourceItem();
                System.Web.UI.Control datasource = plate.FindControl(wdsi.DataSourceID, plate.Page);
                if (datasource != null)
                {
                    di.DataSource = datasource;
                    di.DataMember = "Table";
                    DataTable table =  (di.GetTable()["Table"] as DataView).Table;
                    if (table != null)
                    {
                        foreach (DataColumn dc in table.Columns)
                        {
                            values.Add(dc.ColumnName);
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

    /// <summary>
    /// The editor of file path in design time
    /// </summary>
    public class PathEditor : System.Drawing.Design.UITypeEditor
    {
        public PathEditor()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Select Path:";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    value = fbd.SelectedPath;
                }
            }
            return value;
        }
    }

    /// <summary>
    /// The editor of expression in design time
    /// </summary>
    public class ExpressionEditor : System.Drawing.Design.UITypeEditor
    {
        public ExpressionEditor()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                string oldvalue = (context.Instance as TagItem).Exp;
                IOfficePlate op = (context.Instance as TagItem).Collection.Owner as IOfficePlate;
                List<IComponent> listcomponent = new List<IComponent>();
                if (op is OfficePlate)
                {
                    IContainer container = (op as OfficePlate).Container;
                    for (int i = 0; i < container.Components.Count; i++)
                    {
                        if (container.Components[i] is Control)
                        {
                            listcomponent.Add(container.Components[i]);
                        }
                    }
                }
                else
                {
                    System.Web.UI.Page page = (op as WebOfficePlate).Page;
                    for (int i = 0; i < page.Controls.Count; i++)
                    {
                        listcomponent.Add(page.Controls[i]);
                    }

                    WebExcelPlate newplate = new WebExcelPlate();
                    foreach (WebDataSourceItem item in (op as WebOfficePlate).WebDataSource)
                    {
                        newplate.WebDataSource.Add(item);
                    }
                    page.Controls.Add(newplate);//avoid null page exception, modified on 2007/9/11
                    newplate.InitialDataSourceItem();
                    op = newplate;
                }
                frmValue fv = new frmValue(oldvalue, op, listcomponent);
                if (fv.ShowDialog() == DialogResult.OK)
                {
                    value = fv.tbScript.Text.Replace("\r", string.Empty).Replace("\n", string.Empty);
                }
                if (op is WebOfficePlate)
                {
                    (op as WebOfficePlate).Page.Controls.Remove(op as WebOfficePlate);//remove the control in page avoid problem
                }
            }
            return value;
        }
    }

    /// <summary>
    /// The designer of OfficePlate
    /// </summary>
    public class OfficePlateDesigner :ComponentDesigner
    { 
        /// <summary>
        /// Create a new instance of OfficePlateDesigner
        /// </summary>
        public OfficePlateDesigner()
        {
        }

        /// <summary>
        /// Show the form to design the component
        /// </summary>
        public override void DoDefaultAction()
        {
            frmDesigner fd = new frmDesigner(this.Component as OfficePlate, this.GetService(typeof(IDesignerHost)) as IDesignerHost);
            fd.ShowDialog();
        }
    }
}
