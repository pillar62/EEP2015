using System;
using System.Collections.Generic;
using System.Text;

namespace MWizard2015
{
    public class ReportParameter
    {
        private string _rptRootName;
        public string RptRootName
        {
            get { return _rptRootName; }
            set { _rptRootName = value; }
        }

        private string _rptProjName;
        public string RptProjName
        {
            get { return _rptProjName; }
            set { _rptProjName = value; }
        }

        private string _rptXSDFile;
        public string RptXSDFile
        {
            get { return _rptXSDFile; }
            set { _rptXSDFile = value; }
        }

        private string[] _rptFileNames;
        public string[] RptFileNames
        {
            get { return _rptFileNames; }
            set { _rptFileNames = value; }
        }

        private bool _isMasterDetails;
        public bool IsMasterDetails
        {
            get { return _isMasterDetails; }
            set { _isMasterDetails = value; }
        }

        public int _layoutColumnNum;
        public int LayoutColumnNum
        {
            get { return _layoutColumnNum; }
            set { _layoutColumnNum = value; }
        }

        private EEPReportStyle _rptStyle;
        public EEPReportStyle RptStyle
        {
            get { return _rptStyle; }
            set { _rptStyle = value; }
        }

        private ReportSet _rptSet;
        public ReportSet RptSet
        {
            get { return _rptSet; }
            set { _rptSet = value; }
        }

        private string _selectAlias;
        public string SelectAlias
        {
            get { return _selectAlias; }
            set { _selectAlias = value; }
        }

        private ClientType _clientType;
        public ClientType ClientType
        {
            get { return _clientType; }
            set { _clientType = value; }
        }

        private string _horGaps;
        public string HorGaps
        {
            get { return _horGaps; }
            set { _horGaps = value; }
        }

        private string _vertGaps;
        public string VertGaps
        {
            get { return _vertGaps; }
            set { _vertGaps = value; }
        }
    }

    public class ReportSet
    {
        public ReportSet(string setName)
        {
            this._setName = setName;
        }

        private string _setName;
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }

        private List<ReportTable> _reportTables = new List<ReportTable>();
        public List<ReportTable> ReportTables
        {
            get { return _reportTables; }
            set { _reportTables = value; }
        }
    }

    public class ReportTable
    {
        public ReportTable(string tableName, string tableDescription)
        {
            this._tableName = tableName;
            this._tableDescription = tableDescription;
        }

        public ReportTable(string tableName)
        {
            this._tableName = tableName;
        }

        private string _tableName;
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        private string _tableDescription;
        public string TableDescription
        {
            get { return _tableDescription; }
        }

        private List<ReportColumn> _reportColumns = new List<ReportColumn>();
        public List<ReportColumn> ReportColumns
        {
            get { return _reportColumns; }
            set { _reportColumns = value; }
        }
    }

    public class ReportColumn
    {
        public ReportColumn(string columnName, bool isGroupCondition)
        {
            this._columnName = columnName;
            this._isGroupCondition = isGroupCondition;
        }

        private string _columnName;
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private bool _isGroupCondition;
        public bool IsGroupCondition
        {
            get { return _isGroupCondition; }
            set { _isGroupCondition = value; }
        }

        private double _Width;
        public double Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        private string _TextAlign;
        public string TextAlign
        {
            get { return _TextAlign; }
            set { _TextAlign = value; }
        }
    }

    public enum EEPReportStyle
    {
        Table = 0,
        Label = 1
        
    }


    //edit by eva 2008/4/25
    public class WebClientParam
    {
        private string _webSiteName;
        public string WebSiteName
        {
            get { return _webSiteName; }
            set { _webSiteName = value; }
        }

        private bool _addNewFolder;
        public bool AddNewFolder
        {
            get { return _addNewFolder; }
            set { _addNewFolder = value; }
        }

        private string _folderName;
        public string FolderName
        {
            get { return _folderName; }
            set { _folderName = value; }
        }
    }

    public class WinClientParam
    {
        private string _packageName;
        public string PackageName
        {
            get { return _packageName; }
            set { _packageName = value; }
        }

        private string _outputPath;
        public string OutputPath
        {
            get { return _outputPath; }
            set { _outputPath = value; }
        }

        private string _assemblyOutputPath;
        public string AssemblyOutputPath
        {
            get { return _assemblyOutputPath; }
            set { _assemblyOutputPath = value; }
        }
    } 

    public class ClientParam
    {
        private string _folderName;
        public string FolderName
        {
            get { return _folderName; }
            set { _folderName = value; }
        }
        
        private string _formName;
        public string FormName
        {
            get { return _formName; }
            set { _formName = value; }
        }

        private string _formTitle;
        public string FormTitle
        {
            get { return _formTitle; }
            set { _formTitle = value;}
        }

        private string _providerName;
        public string ProviderName
        {
            get { return _providerName; }
            set { _providerName = value; }
        }

        private string _tableName;
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        private string _realTableName;
        public string RealTableName
        {
            get { return _realTableName; }
            set { _realTableName = value; }
        }

        private string _childTableName;
        public string ChildTableName
        {
            get { return _childTableName; }
            set { _childTableName = value; }
        }

        private string _childRealTableName;
        public string ChildRealTableName
        {
            get { return _childRealTableName; }
            set { _childRealTableName = value; }
        }

        private bool _isMasterDetails;
        public bool IsMasterDetails
        {
            get { return _isMasterDetails; }
            set { _isMasterDetails = value; }
        }

        private string _detailProviderName;
        public string DetailProviderName
        {
            get { return _detailProviderName; }
            set { _detailProviderName = value; }
        }

        private TBlockItems _blocks;
        public TBlockItems Blocks
        {
            get { return _blocks; }
            set { _blocks = value; }
        }

        private string _selectAlias;
        public string SelectAlias
        {
            get { return _selectAlias; }
            set { _selectAlias = value; }
        }

        private ClientType _clientType;
        public ClientType ClientType
        {
            get { return _clientType; }
            set { _clientType = value; }
        }

        private string _rptFileName;
        public string RptFileName
        {
            get { return _rptFileName; }
            set { _rptFileName = value; }
        }
    }
}
