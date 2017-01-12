using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

namespace OfficeTools
{
    /// <summary>
    /// The class used to export the data of dataset to an excel file
    /// </summary>
    public class ExcelExport
    {
        /// <summary>
        /// Create a new instance of ExcelExport
        /// </summary>
        public ExcelExport():this(new DataSet(), string.Empty, PlateModeType.Xml, new Hashtable()) { }

        /// <summary>
        /// Create a new instance of ExcelExport
        /// </summary>
        /// <param name="dataset">The dataset to export</param>
        public ExcelExport(DataSet dataset) : this(dataset, string.Empty, PlateModeType.Xml, new Hashtable()) { }

        /// <summary>
        /// Create a new instance of ExcelExport
        /// </summary>
        /// <param name="dataset">The dataset to export</param>
        /// <param name="filename">The file to export to</param>
        public ExcelExport(DataSet dataset, string filename) : this(dataset, filename, PlateModeType.Xml, new Hashtable()) { }

        /// <summary>
        /// Create a new instance of ExcelExport
        /// </summary>
        /// <param name="dataset">The dataset to export</param>
        /// <param name="filename">The file to export to</param>
        /// <param name="exportmode">The mode used to export: xml or com</param>
        public ExcelExport(DataSet dataset, string filename, PlateModeType exportmode) : this(dataset, filename, exportmode, new Hashtable()) { }

        /// <summary>
        /// Create a new instance of ExcelExport
        /// </summary>
        /// <param name="dataset">The dataset to export</param>
        /// <param name="filename">The file to export to</param>
        /// <param name="exportmode">The mode used to export: xml or com</param>
        /// <param name="tabledirectory">The direcory of column defined</param>
        public ExcelExport(DataSet dataset, string filename, PlateModeType exportmode, IDictionary tabledirectory)
        {
            _DataSet = dataset;
            _FileName = filename;
            _ExportMode = exportmode;
            _TableDirectory = tabledirectory;
        }

        private DataSet _DataSet;
        /// <summary>
        /// The dataset to export
        /// </summary>
        public DataSet DataSet
        {
            get { return _DataSet; }
            set { _DataSet = value; }
        }

        private string _FileName;
        /// <summary>
        /// The file to export to
        /// </summary>
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private PlateModeType _ExportMode;
        /// <summary>
        /// The mode used to export: xml or com
        /// </summary>
        public PlateModeType ExportMode
        {
            get { return _ExportMode; }
            set { _ExportMode = value; }
        }

        private IDictionary _TableDirectory;
        /// <summary>
        /// The directory of column defined
        /// </summary>
        public IDictionary TableDirectory
        {
            get { return _TableDirectory; }
            set { _TableDirectory = value; }
        }

        /// <summary>
        /// Export the data of dataset to an excel file
        /// </summary>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="XmlException">Exception will be thrown when file can not be recognized as xml file in xml mode</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        public void Export()
        {
            Export(false);
        }

        /// <summary>
        /// Export the data of dataset to an excel file
        /// </summary>
        /// <param name="append">The value indicates whether to append or replace the file</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="XmlException">Exception will be thrown when file can not be recognized as xml file in xml mode</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        public void Export(bool append)
        {
            CheckProperties();
            if (DataSet.Tables.Count == 0)
            {
                throw new Exception("DataSet can not export data since it is empty");
            }
            if (!File.Exists(this.FileName))
            {
                CreateFile(this.FileName);
            }
            if (ExportMode == PlateModeType.Xml)
            {
                XmlDocument xmldoc = new XmlDocument();
                try
                {
                    xmldoc.Load(this.FileName);
                }
                catch
                {
                    throw new XmlException("File can not be recognized as a xml file");
                }
                XmlNamespaceManager xmlmgr = new XmlNamespaceManager(xmldoc.NameTable);
                xmlmgr.AddNamespace("sheet", "urn:schemas-microsoft-com:office:spreadsheet");
                XmlNode nodebook = xmldoc.DocumentElement.SelectSingleNode("/sheet:Workbook", xmlmgr);
                if (!append)
                {
                    nodebook.RemoveAll();
                }
                for (int i = 0; i < this.DataSet.Tables.Count; i++)
                {
                    DataTable table = this.DataSet.Tables[i];
                    object[,] objdata = GetData(table, string.Empty);
                    ExportTableXml(objdata, table.TableName, nodebook, xmldoc);

                }
                xmldoc.Save(this.FileName);
            }
            else
            {
                Excel.Application objExcel = new Excel.Application();
                if (objExcel == null)
                {
                    throw new InvalidComObjectException("Excel could not be started. Check that your office installation and project references are correct");
                }
                objExcel.Visible = false;
                objExcel.DisplayAlerts = false;
                Excel.Workbook objWorkBook = objExcel.Workbooks.Open(this.FileName, Missing.Value, Missing.Value, Missing.Value
               , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
               , Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                try
                {
                    for (int i = 0; i < this.DataSet.Tables.Count; i++)
                    {
                        DataTable table = this.DataSet.Tables[i];
                        object[,] objdata = GetData(table, string.Empty);
                        ExportTableCom(objdata, table.TableName, objWorkBook);
                    }
                    if (!append)
                    {
                        int deletecount = objWorkBook.Worksheets.Count - this.DataSet.Tables.Count;
                        for (int i = 0; i < deletecount; i++)
                        {
                            ((Excel.Worksheet)objWorkBook.Worksheets[this.DataSet.Tables.Count + 1]).Delete();
                        }
                    }
                } 
                finally
                {
                    objWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value
               , Missing.Value, Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    objExcel.Quit();
                    Marshal.ReleaseComObject(objWorkBook);
                    Marshal.ReleaseComObject(objExcel);
                    objWorkBook = null;
                    objExcel = null;
                    GC.Collect();
                }

            }
        }

        /// <summary>
        /// Export the data of dataset to an excel file
        /// </summary>
        /// <param name="tablename">The name of table to export to the excel file</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="Exception">Exception will be thrown when name of table is invalid</exception>
        /// <exception cref="XmlException">Exception will be thrown when file can not be recognized as xml file in xml mode</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        public void Export(string tablename)
        {
            Export(tablename, string.Empty, string.Empty, false);
        }

        /// <summary>
        /// Export the data of dataset to an excel file
        /// </summary>
        /// <param name="tablename">The name of table to export to the excel file</param>
        /// <param name="filter">The filter to export the data</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="Exception">Exception will be thrown when name of table is invalid</exception>
        /// <exception cref="XmlException">Exception will be thrown when file can not be recognized as xml file in xml mode</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        public void Export(string tablename, string filter)
        {
            Export(tablename, filter, string.Empty, false);
        }

        /// <summary>
        /// Export the data of dataset to an excel file
        /// </summary>
        /// <param name="tablename">The name of table to export to the excel file</param>
        /// <param name="filter">The filter to export the data</param>
        /// <param name="append">The value indicates whether to append or replace the file</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="Exception">Exception will be thrown when name of table is invalid</exception>
        /// <exception cref="XmlException">Exception will be thrown when file can not be recognized as xml file in xml mode</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        public void Export(string tablename, string filter, bool append)
        {
            Export(tablename, filter, string.Empty, append);
        }

        /// <summary>
        /// Export the data of dataset to an excel file
        /// </summary>
        /// <param name="tablename">The name of table to export to the excel file</param>
        /// <param name="filter">The filter to export the data</param>
        /// <param name="sort">The sort expression of the data</param>
        /// <param name="append">The value indicates whether to append or replace the file</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="Exception">Exception will be thrown when name of table is invalid</exception>
        /// <exception cref="XmlException">Exception will be thrown when file can not be recognized as xml file in xml mode</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        public void Export(string tablename, string filter, string sort, bool append)
        {
            CheckProperties();
            if (!DataSet.Tables.Contains(tablename))
            {
                throw new Exception(string.Format("Name is invalid, table: {0} does not exist", tablename));
            }
            ExportTable(DataSet.Tables[tablename], filter, sort, append);
        }

        /// <summary>
        /// Export the data of dataset to an excel file
        /// </summary>
        /// <param name="tableindex">The index of table to export to the excel file</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of table is out of range</exception>
        /// <exception cref="XmlException">Exception will be thrown when file can not be recognized as xml file in xml mode</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        public void Export(int tableindex)
        {
            Export(tableindex, string.Empty, string.Empty, false);
        }

        /// <summary>
        /// Export the data of dataset to an excel file
        /// </summary>
        /// <param name="tableindex">The index of table to export to the excel file</param>
        /// <param name="filter">The filter to export the data</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of table is out of range</exception>
        /// <exception cref="XmlException">Exception will be thrown when file can not be recognized as xml file in xml mode</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        public void Export(int tableindex, string filter)
        {
            Export(tableindex, filter, string.Empty, false);
        }

        /// <summary>
        /// Export the data of dataset to an excel file
        /// </summary>
        /// <param name="tableindex">The index of table to export to the excel file</param>
        /// <param name="filter">The filter to export the data</param>
        /// <param name="append">The value indicates whether to append or replace the file</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of table is out of range</exception>
        /// <exception cref="XmlException">Exception will be thrown when file can not be recognized as xml file in xml mode</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        public void Export(int tableindex, string filter, bool append)
        {
            Export(tableindex, filter, string.Empty, append);
        }

        /// <summary>
        /// Export the data of dataset to an excel file
        /// </summary>
        /// <param name="tableindex">The index of table to export to the excel file</param>
        /// <param name="filter">The filter to export the data</param>
        /// <param name="sort">The sort expression of the data</param>
        /// <param name="append">The value indicates whether to append or replace the file</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of table is out of range</exception>
        /// <exception cref="XmlException">Exception will be thrown when file can not be recognized as xml file in xml mode</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        public void Export(int tableindex, string filter, string sort, bool append)
        {
            CheckProperties();
            if (tableindex >= DataSet.Tables.Count || tableindex < 0)
            {
                throw new IndexOutOfRangeException(string.Format("Index is out of range, table {0} does not exist", tableindex.ToString()));
            }
            ExportTable(DataSet.Tables[tableindex], filter, sort, append);
        }

        /// <summary>
        /// Export the data of datatable to an excel file
        /// </summary>
        /// <param name="table">The datatable to export</param>
        /// <param name="filter">The filter to export the data</param>
        /// <param name="append">The value indicates whether to append or replace the file</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="XmlException">Exception will be thrown when file can not be recognized as xml file in xml mode</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        public void ExportTable(DataTable table, string filter, bool append)
        {
            ExportTable(table, filter, string.Empty, append);
        }

        /// <summary>
        /// Export the data of datatable to an excel file
        /// </summary>
        /// <param name="table">The datatable to export</param>
        /// <param name="filter">The filter to export the data</param>
        /// <param name="sort">The sort expression of data</param>
        /// <param name="append">The value indicates whether to append or replace the file</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="XmlException">Exception will be thrown when file can not be recognized as xml file in xml mode</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        public void ExportTable(DataTable table, string filter, string sort, bool append)
        {
            if (!File.Exists(this.FileName))
            {
                CreateFile(this.FileName);
            }
            object[,] objdata = GetData(table, filter);
            if (ExportMode == PlateModeType.Xml)
            {
                XmlDocument xmldoc = new XmlDocument();
                try
                {
                    xmldoc.Load(this.FileName);
                }
                catch
                {
                    throw new XmlException("File can not be recognized as a xml file");
                }
                XmlNamespaceManager xmlmgr = new XmlNamespaceManager(xmldoc.NameTable);
                xmlmgr.AddNamespace("sheet", "urn:schemas-microsoft-com:office:spreadsheet");
                XmlNode nodebook = xmldoc.DocumentElement.SelectSingleNode("/sheet:Workbook", xmlmgr);
                if (!append)
                {
                    nodebook.RemoveAll();
                }
                ExportTableXml(objdata, table.TableName, nodebook, xmldoc);
                xmldoc.Save(this.FileName);
            }
            else
            { 
                Excel.Application objExcel = new Excel.Application();
                objExcel.Visible = false;
                objExcel.DisplayAlerts = false;
                if (objExcel == null)
                {
                    throw new InvalidComObjectException("Excel could not be started. Check that your office installation and project references are correct");
                }
                Excel.Workbook objWorkBook = objExcel.Workbooks.Open(this.FileName, Missing.Value, Missing.Value, Missing.Value
               , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
               , Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                try
                {
                    ExportTableCom(objdata, table.TableName, objWorkBook);
                    if (!append)
                    {
                        int deletecount = objWorkBook.Worksheets.Count - 1;
                        for (int i = 0; i < deletecount; i++)
                        { 
                            ((Excel.Worksheet)objWorkBook.Worksheets[2]).Delete();
                        }
                    }
                }
                finally
                {
                    objWorkBook.Save();
                    objExcel.Quit();
                    Marshal.ReleaseComObject(objWorkBook);
                    Marshal.ReleaseComObject(objExcel);
                    objWorkBook = null;
                    objExcel = null;
                    GC.Collect();
                }
            }
        }

        private void ExportTableXml(object[,] objdata, string tablename, XmlNode nodebook, XmlDocument xml)
        {
            string sheetname = tablename;
            foreach (XmlNode node in nodebook.ChildNodes)
            {
                if (node.Name == "Worksheet" && node.Attributes["Name", "urn:schemas-microsoft-com:office:spreadsheet"].Value == tablename)
                {
                    sheetname = tablename + DateTime.Now.ToString("yyyyMMddHHmmss");
                    break;
                }
            }
            XmlNode nodesheet = xml.CreateElement("Worksheet");
            XmlAttribute attsheet = xml.CreateAttribute("ss", "Name", "urn:schemas-microsoft-com:office:spreadsheet");
            attsheet.Value = sheetname;
            nodesheet.Attributes.Append(attsheet);
            XmlAttribute attxmln = xml.CreateAttribute("xmlns");
            attxmln.Value = "urn:schemas-microsoft-com:office:spreadsheet";
            nodesheet.Attributes.Append(attxmln);

            nodebook.AppendChild(nodesheet);

            XmlNode nodetable = xml.CreateElement("Table");
            nodesheet.AppendChild(nodetable);

            int rowcount = objdata.GetUpperBound(0) + 1;
            int columncount = objdata.GetUpperBound(1) + 1 ;

            for (int i = 0; i < rowcount; i++)
            {
                XmlNode noderow = xml.CreateElement("Row");
                nodetable.AppendChild(noderow);         
                for (int j = 0; j < columncount; j++)
                {
                    ExportCell(objdata[i, j], noderow, xml);
                }
            }
        }

        private void ExportCell(object data, XmlNode noderow, XmlDocument xml)
        {
            XmlNode nodecell = xml.CreateElement("Cell");
            noderow.AppendChild(nodecell);
            XmlNode nodedata = xml.CreateElement("Data");
            nodecell.AppendChild(nodedata);
            XmlAttribute atttype = xml.CreateAttribute("ss", "Type", "urn:schemas-microsoft-com:office:spreadsheet");
            Type typedata = data.GetType();
            string strcell = string.Empty;
            if (typedata == typeof(String))
            {
                atttype.Value = "String";
                strcell = data.ToString();
            }
            else if (typedata == typeof(Boolean))
            {
                atttype.Value = "Boolean";
                strcell = (((bool)data) == true) ? "1" : "0";
            }
            else if (typedata == typeof(DateTime))
            {
                atttype.Value = "String";
                strcell = ((DateTime)data).ToShortDateString();
            }
            else if (typedata == typeof(byte[]) || typedata == typeof(DBNull))
            {
                atttype.Value = "String";
                strcell = string.Empty;
            }
            else
            {
                atttype.Value = "Number";
                strcell = data.ToString();
            }
            nodedata.Attributes.Append(atttype);
            nodedata.InnerText = strcell;
        }

        private void ExportTableCom(object[,] objdata, string tablename, Excel.Workbook objbook)
        {
            string sheetname = tablename;
            for (int i = 1; i <=  objbook.Worksheets.Count; i++)
            {
                if ((objbook.Worksheets[i] as Excel.Worksheet).Name == sheetname)
                {
                    sheetname = tablename + DateTime.Now.ToString("yyyyMMddHHmmss");
                    break;
                }
            }
            Excel.Worksheet objsheet = (Excel.Worksheet)objbook.Worksheets.Add(Missing.Value, Missing.Value, 1, Missing.Value);
            objsheet.Name = sheetname;
            Excel.Range objRange = objsheet.get_Range("A1", "A1");
            int rowcount = objdata.GetUpperBound(0) + 1;
            int columncount = objdata.GetUpperBound(1) + 1;

            objRange = objRange.get_Resize(rowcount, columncount);
            objRange.set_Value(Missing.Value, objdata);

            Marshal.ReleaseComObject(objRange);
            Marshal.ReleaseComObject(objsheet);

            objRange = null;
            objsheet = null;

            GC.Collect();
        }

        private object[,] GetData(DataTable table, string filter)
        {
            DataRow[] arrdr = table.Select(filter);
            int rowcount = arrdr.Length;
            int columncount = table.Columns.Count;
            object[,] arrobj = new object[rowcount + 1, columncount];
            for (int i = 0; i < columncount; i++)
            {
                if (TableDirectory.Contains(table.Columns[i].ColumnName))
                {
                    arrobj[0, i] = TableDirectory[table.Columns[i].ColumnName];
                }
                else
                {
                    arrobj[0, i] = table.Columns[i].ColumnName;
                }
            }
            for (int i = 1; i <= rowcount; i++)
            {
                for (int j = 0; j < columncount; j++)
                {
                    arrobj[i, j] = arrdr[i - 1][j];
                }
            }
            return arrobj;
        }

        private void CreateFile(string filename)
        {
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            StreamWriter sw = new StreamWriter(filename, false);

            sw.WriteLine("<?xml version=\"1.0\"?>");
            sw.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
            sw.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sw.WriteLine("xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            sw.WriteLine("xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            sw.WriteLine("xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
            sw.WriteLine("<Worksheet ss:Name=\"Sheet1\">");
            sw.WriteLine("</Worksheet>");
            sw.WriteLine("</Workbook>");

            sw.Close();
        }

        private void CheckProperties()
        {
            if (DataSet == null)
            {
                throw new Exception("Propery of dataset can not be null");
            }
            else if (FileName.Equals(string.Empty))
            {
                throw new Exception("Property of filename is empty");
            }
        }
    }


    /// <summary>
    /// The class used to read data from a excel file
    /// </summary>
    public class ExcelReader
    {
        /// <summary>
        /// Create a new instance of ExcelExport
        /// </summary>
        public ExcelReader() : this(new DataSet(), string.Empty, false, MissingSchemaAction.Ignore) { }

        /// <summary>
        /// Create a new instance of ExcelExport
        /// </summary>
        /// <param name="dataset">The dataset to read</param>
        /// <param name="filename">The file to read from </param>
        public ExcelReader(DataSet dataset, string filename) : this(dataset, filename, false, MissingSchemaAction.Ignore) { }

        /// <summary>
        /// Create a new instance of ExcelExport
        /// </summary>
        /// <param name="dataset">The dataset to read</param>
        /// <param name="filename">The file to read from </param>
        /// <param name="includecolumnname">Read Column defination from file</param>
        public ExcelReader(DataSet dataset, string filename, bool includecolumnname) 
            : this(dataset, filename, includecolumnname, MissingSchemaAction.Ignore) { }

        /// <summary>
        /// Create a new instance of ExcelExport
        /// </summary>
        /// <param name="dataset">The dataset to read</param>
        /// <param name="filename">The file to read from </param>
        /// <param name="includecolumnname">Read Column defination from file</param>
        /// <param name="action">Action of missing schema</param>
        public ExcelReader(DataSet dataset, string filename, bool includecolumnname, MissingSchemaAction action)
        {
            _DataSet = dataset;
            _FileName = filename;
            _IncludeColumnName = includecolumnname;
            _Action = action;
        }

        private DataSet _DataSet;
        /// <summary>
        /// The dataset to read
        /// </summary>
        public DataSet DataSet
        {
            get { return _DataSet; }
            set { _DataSet = value; }
        }

        private string _FileName;
        /// <summary>
        /// The file to read from 
        /// </summary>
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private bool _IncludeColumnName;
        /// <summary>
        /// Read Column defination from file
        /// </summary>
        public bool IncludeColumnName
        {
            get { return _IncludeColumnName; }
            set { _IncludeColumnName = value; }
        }

        private MissingSchemaAction _Action;
        /// <summary>
        /// Action of missing schema
        /// </summary>
        public MissingSchemaAction Action
        {
            get { return _Action; }
            set { _Action = value; }
        }

        /// <summary>
        /// Read data from file
        /// </summary>
        /// <param name="table">Datatable</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when table is null</exception>
        /// <exception cref="FileNotFoundException">Exception will be thrown when file does not exist</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        /// <exception cref="NullReferenceException">Exception will be thrown when name of column in excel</exception>
        /// <exception cref="NoNullAllowedException">Exception will be thrown when some not null allowed columns in table has no match in file</exception>
        /// <exception cref="InvalidOperationException">Exception will be thrown when some columns in files has no match in table in MissingSchemaAction.Error mode</exception>
        /// <exception cref="ArgumentException">Exception will be thrown when columns in file has duplicated name</exception>
        ///<exception cref="NoNullAllowedException">Exception will be thrown when some data of not null allowed columns in table can not cast in file</exception>
        public void Read(DataTable table)
        {
            Read(0, table);
        }

        /// <summary>
        /// Read data from file
        /// </summary>
        /// <param name="tablename">Name of datatable</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="ArgumentException">Exception will be thrown when name of table is invalid</exception>
        /// <exception cref="FileNotFoundException">Exception will be thrown when file does not exist</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        /// <exception cref="NullReferenceException">Exception will be thrown when name of column in excel</exception>
        /// <exception cref="NoNullAllowedException">Exception will be thrown when some not null allowed columns in table has no match in file</exception>
        /// <exception cref="InvalidOperationException">Exception will be thrown when some columns in files has no match in table in MissingSchemaAction.Error mode</exception>
        /// <exception cref="ArgumentException">Exception will be thrown when columns in file has duplicated name</exception>
        ///<exception cref="NoNullAllowedException">Exception will be thrown when some data of not null allowed columns in table can not cast in file</exception>
        public void Read(string tablename)
        {
            Read(0, tablename);
        }

        /// <summary>
        /// Read data from file
        /// </summary>
        /// <param name="tableindex">Index of datatable</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of table is out of range</exception>
        /// <exception cref="FileNotFoundException">Exception will be thrown when file does not exist</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        /// <exception cref="NullReferenceException">Exception will be thrown when name of column in excel</exception>
        /// <exception cref="NoNullAllowedException">Exception will be thrown when some not null allowed columns in table has no match in file</exception>
        /// <exception cref="InvalidOperationException">Exception will be thrown when some columns in files has no match in table in MissingSchemaAction.Error mode</exception>
        /// <exception cref="ArgumentException">Exception will be thrown when columns in file has duplicated name</exception>
        ///<exception cref="NoNullAllowedException">Exception will be thrown when some data of not null allowed columns in table can not cast in file</exception>
        public void Read(int tableindex)
        {
            Read(0, tableindex);
        }

        /// <summary>
        /// Read data from file
        /// </summary>
        /// <param name="sheetindex">Index of worksheet</param>
        /// <param name="table">Datatable</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when table is null</exception>
        /// <exception cref="FileNotFoundException">Exception will be thrown when file does not exist</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of worksheet is out of range</exception>
        /// <exception cref="NullReferenceException">Exception will be thrown when name of column in excel</exception>
        /// <exception cref="NoNullAllowedException">Exception will be thrown when some not null allowed columns in table has no match in file</exception>
        /// <exception cref="InvalidOperationException">Exception will be thrown when some columns in files has no match in table in MissingSchemaAction.Error mode</exception>
        /// <exception cref="ArgumentException">Exception will be thrown when columns in file has duplicated name</exception>
        ///<exception cref="NoNullAllowedException">Exception will be thrown when some data of not null allowed columns in table can not cast in file</exception>
        public void Read(int sheetindex, DataTable table)
        {
            Read(0, 0, sheetindex, table);
        }

        /// <summary>
        /// Read data from file
        /// </summary>
        /// <param name="sheetindex">Index of worksheet</param>
        /// <param name="tablename">Name of datatable</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="ArgumentException">Exception will be thrown when name of table is invalid</exception>
        /// <exception cref="FileNotFoundException">Exception will be thrown when file does not exist</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of worksheet is out of range</exception>
        /// <exception cref="NullReferenceException">Exception will be thrown when name of column in excel</exception>
        /// <exception cref="NoNullAllowedException">Exception will be thrown when some not null allowed columns in table has no match in file</exception>
        /// <exception cref="InvalidOperationException">Exception will be thrown when some columns in files has no match in table in MissingSchemaAction.Error mode</exception>
        /// <exception cref="ArgumentException">Exception will be thrown when columns in file has duplicated name</exception>
        ///<exception cref="NoNullAllowedException">Exception will be thrown when some data of not null allowed columns in table can not cast in file</exception>
        public void Read(int sheetindex, string tablename)
        {
            Read(0, 0, sheetindex, tablename);
        }

        /// <summary>
        /// Read data from file
        /// </summary>
        /// <param name="sheetindex">Index of worksheet</param>
        /// <param name="tableindex">Index of datatable</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of table is out of range</exception>
        /// <exception cref="FileNotFoundException">Exception will be thrown when file does not exist</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of worksheet is out of range</exception>
        /// <exception cref="NullReferenceException">Exception will be thrown when name of column in excel</exception>
        /// <exception cref="NoNullAllowedException">Exception will be thrown when some not null allowed columns in table has no match in file</exception>
        /// <exception cref="InvalidOperationException">Exception will be thrown when some columns in files has no match in table in MissingSchemaAction.Error mode</exception>
        /// <exception cref="ArgumentException">Exception will be thrown when columns in file has duplicated name</exception>
        ///<exception cref="NoNullAllowedException">Exception will be thrown when some data of not null allowed columns in table can not cast in file</exception>
        public void Read(int sheetindex, int tableindex)
        {
            Read(0, 0, sheetindex, tableindex);
        }

        /// <summary>
        /// Read data from file
        /// </summary>
        /// <param name="beginrow">The start row to read data</param>
        /// <param name="begincolumn">The start column to read data</param>
        /// <param name="sheetindex">Index of worksheet</param>
        /// <param name="table">Datatable</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when table is null</exception>
        /// <exception cref="FileNotFoundException">Exception will be thrown when file does not exist</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of worksheet is out of range</exception>
        /// <exception cref="NullReferenceException">Exception will be thrown when name of column in excel</exception>
        /// <exception cref="NoNullAllowedException">Exception will be thrown when some not null allowed columns in table has no match in file</exception>
        /// <exception cref="InvalidOperationException">Exception will be thrown when some columns in files has no match in table in MissingSchemaAction.Error mode</exception>
        /// <exception cref="ArgumentException">Exception will be thrown when columns in file has duplicated name</exception>
        ///<exception cref="NoNullAllowedException">Exception will be thrown when some data of not null allowed columns in table can not cast in file</exception>
        public void Read(int beginrow, int begincolumn, int sheetindex, DataTable table)
        {
            if (table == null)
            {
                throw new NullReferenceException("Table can not be null");
            }
            if (!File.Exists(FileName))
            {
                throw new FileNotFoundException("Can not find file: " + FileName);
            }
            Excel.Application objExcel = new Excel.Application();
            if (objExcel == null)
            {
                throw new InvalidComObjectException("Excel could not be started. Check that your office installation and project references are correct");
            }
            objExcel.Visible = false;
            objExcel.DisplayAlerts = false;
            Excel.Workbook objWorkBook = objExcel.Workbooks.Open(this.FileName, Missing.Value, Missing.Value, Missing.Value
           , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
           , Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            try
            {
                if (sheetindex >= objWorkBook.Worksheets.Count)
                {
                    throw new IndexOutOfRangeException(string.Format("Index is out of range, sheet: {0} does not exist", sheetindex));
                }
                string cellrange = 
                    ((Excel.Worksheet)objWorkBook.Worksheets[sheetindex + 1]).UsedRange.get_Address(false, false, Excel.XlReferenceStyle.xlA1, null, null);
                string[] arrcellrange = cellrange.Split(':');
                string lastcelladdress = arrcellrange[arrcellrange.Length - 1];
                string firstcelladdress
                    = (((Excel.Worksheet)objWorkBook.Worksheets[sheetindex + 1]).Cells[beginrow + 1, begincolumn + 1] 
                    as Excel.Range).get_Address(false, false, Excel.XlReferenceStyle.xlA1, null, null);
                object data = ((Excel.Worksheet)objWorkBook.Worksheets[sheetindex + 1]).get_Range(firstcelladdress, lastcelladdress).get_Value(null);
                if (data is object[])
                {
                    throw new Exception("Only one value is invalid");//leave a bug
                }
                if (this.IncludeColumnName)
                {
                    ReadByName((object[,])data, table);
                }
                else
                {
                    ReadByIndex((object[,])data, table);
                }
            }
            finally
            {
                objExcel.Quit();
                Marshal.ReleaseComObject(objWorkBook);
                Marshal.ReleaseComObject(objExcel);
                objWorkBook = null;
                objExcel = null;
                GC.Collect();
            }
        }

        /// <summary>
        /// Read data from file
        /// </summary>
        /// <param name="beginrow">The start row to read data</param>
        /// <param name="begincolumn">The start column to read data</param>
        /// <param name="sheetindex">Index of worksheet</param>
        /// <param name="tablename">Name of datatable</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="ArgumentException">Exception will be thrown when name of table is invalid</exception>
        /// <exception cref="FileNotFoundException">Exception will be thrown when file does not exist</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of worksheet is out of range</exception>
        /// <exception cref="NullReferenceException">Exception will be thrown when name of column in excel</exception>
        /// <exception cref="NoNullAllowedException">Exception will be thrown when some not null allowed columns in table has no match in file</exception>
        /// <exception cref="InvalidOperationException">Exception will be thrown when some columns in files has no match in table in MissingSchemaAction.Error mode</exception>
        /// <exception cref="ArgumentException">Exception will be thrown when columns in file has duplicated name</exception>
        ///<exception cref="NoNullAllowedException">Exception will be thrown when some data of not null allowed columns in table can not cast in file</exception>
        public void Read(int beginrow, int begincolumn, int sheetindex, string tablename)
        {
            CheckProperties();
            if (!DataSet.Tables.Contains(tablename))
            {
                throw new ArgumentException(string.Format("Name is invalid, table: {0} does not exist", tablename));
            }
            Read(beginrow, begincolumn, sheetindex, DataSet.Tables[tablename]);
        }

        /// <summary>
        /// Read data from file
        /// </summary>
        /// <param name="beginrow">The start row to read data</param>
        /// <param name="begincolumn">The start column to read data</param>
        /// <param name="sheetindex">Index of worksheet</param>
        /// <param name="tableindex">Index of datatable</param>
        /// <exception cref="NullReferenceException">Exception will be thrown when dataset or filename is null</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of table is out of range</exception>
        /// <exception cref="FileNotFoundException">Exception will be thrown when file does not exist</exception>
        /// <exception cref="InvalidComObjectException">Excption will be thrown when excel has not installed in com mode</exception>
        /// <exception cref="IndexOutOfRangeException">Exception will be thrown when index of worksheet is out of range</exception>
        /// <exception cref="NullReferenceException">Exception will be thrown when name of column in excel</exception>
        /// <exception cref="NoNullAllowedException">Exception will be thrown when some not null allowed columns in table has no match in file</exception>
        /// <exception cref="InvalidOperationException">Exception will be thrown when some columns in files has no match in table in MissingSchemaAction.Error mode</exception>
        /// <exception cref="ArgumentException">Exception will be thrown when columns in file has duplicated name</exception>
        ///<exception cref="NoNullAllowedException">Exception will be thrown when some data of not null allowed columns in table can not cast in file</exception>
        public void Read(int beginrow, int begincolumn, int sheetindex, int tableindex)
        {
            CheckProperties();
            if (tableindex >= DataSet.Tables.Count || tableindex < 0)
            {
                throw new IndexOutOfRangeException(string.Format("Index is out of range, table: {0} does not exist", tableindex.ToString()));
            }
            Read(beginrow, begincolumn, sheetindex, DataSet.Tables[tableindex]);
        }

        private void ReadByIndex(object[,] data, DataTable table)
        {
            int columncount = data.GetLength(1);
            List<int> index = new List<int>();
            for (int i = 0; i < columncount; i++)
            {
                index.Add(i);
            }
            ReadByIndex(data, 0, table, index);
        }

        private void ReadByIndex(object[,] data, int beginrow, DataTable table, List<int> index)
        {
            CheckAllowDBNull(table, index);

            DataTable newdata = table.Clone();
            newdata.Rows.Clear();

            int startindex = data.GetLowerBound(1);
            int endindex = data.GetUpperBound(1);
            int firstrow = data.GetLowerBound(0);
            int lastrow = data.GetUpperBound(0);

            int columncount = data.GetLength(0);

            for (int i = firstrow + 1; i <= lastrow; i++)
            {
                DataRow dr = newdata.NewRow();
                for (int j = startindex; j <= endindex; j++)
                {
                    int indexintable = index[j - startindex];
                    if (indexintable != -1 && indexintable <  table.Columns.Count)
                    {
                        object datacell = data[i, j];
                        try
                        {
                            datacell = Convert.ChangeType(datacell, table.Columns[indexintable].DataType);
                        }
                        catch
                        {
                            datacell = DBNull.Value;
                        }
                        if (datacell == DBNull.Value && !table.Columns[indexintable].AllowDBNull)
                        {
                            throw new NoNullAllowedException(string.Format("Column:{0} in table does not allow null value,but data in excel file can not cast"
                                , table.Columns[indexintable].ColumnName));
                        }
                        dr[indexintable] = datacell;
                    }
                }
                newdata.Rows.Add(dr);
            }
            table.Merge(newdata);
        }

        private void ReadByName(object[,] data, DataTable table)
        {
            int startindex = data.GetLowerBound(1);
            int endindex = data.GetUpperBound(1);
            int firstrow = data.GetLowerBound(0);
            int lastrow = data.GetUpperBound(0);
            List<string> name = new List<string>();
            List<Type> type = new List<Type>();
            for (int i = startindex; i <= endindex; i++)
			{
                if (data[firstrow, i] == null || data[firstrow, i].ToString().Trim().Length == 0)
                {
                    throw new NullReferenceException("Name of Column can not be empty");
                }
                name.Add(data[firstrow, i].ToString());
                Type newtype = typeof(string);
                for (int j = firstrow; j <= lastrow; j++)
                {
                    if (data[j, i] != null)
                    {
                        newtype = data[j, i].GetType();
                        break;
                    }
                }
                type.Add(newtype);
			}
            List<int> index = ConvertNameToIndex(name, type, table);
            ReadByIndex(data, 1, table, index);
        }

        private List<int> ConvertNameToIndex(List<string> name, List<Type> type, DataTable table)
        {
            List<int> listindex = new List<int>();
            for (int i = 0; i < name.Count; i++)
            {
                if (!table.Columns.Contains(name[i]))
                {
                    if (Action == MissingSchemaAction.Add)
                    {
                        DataColumn dc = new DataColumn(name[i], type[i]);
                        table.Columns.Add(dc);

                        listindex.Add(table.Columns.IndexOf(dc));
                    }
                    else if (Action == MissingSchemaAction.Error)
                    {
                        throw new InvalidOperationException(string.Format("Column: {0} does not exist in table", name[i]));
                    }
                    else
                    {
                        listindex.Add(-1);
                    }
                }
                else
                {
                    int index = table.Columns.IndexOf(name[i]);
                    if (listindex.Contains(index))
                    {
                        throw new ArgumentException(string.Format("Column: {0} appears twice in file", name[i]));
                    }
                    listindex.Add(index);
                }
            }
            return listindex;
        }

        private void CheckAllowDBNull(DataTable table, List<int> index)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (!index.Contains(i) && !table.Columns[i].AllowDBNull)
                { 
                    throw new NoNullAllowedException(string.Format("Column:{0} in table does not allow null value,but no data in excel file"
                        , table.Columns[i].ColumnName));
                }
            }
        }

        private void CheckProperties()
        {
            if (DataSet == null)
            {
                throw new NullReferenceException("Propery of dataset can not be null");
            }
            else if (FileName.Equals(string.Empty))
            {
                throw new NullReferenceException("Property of filename is empty");
            }
        }
    }


    /// <summary>
    /// The class used to kill the process of office   
    /// </summary>
    public class OfficeKiller
    {
        /// <summary>
        /// Kill the process of office
        /// </summary>
        /// <param name="type">The type of office: excel or word</param>
        /// <returns>The count of processes have been killed </returns>
        public static int Kill(OfficeType type)
        {
            return Kill(type, KillMode.All);
        }

        /// <summary>
        /// Kill the process of office
        /// </summary>
        /// <param name="type">The type of office: excel or word</param>
        /// <param name="mode">The mode of kill: confirm or not</param>
        /// <returns>The count of processes have been killed </returns>
        public static int Kill(OfficeType type, KillMode mode)
        {
            int killcount = 0;
            foreach (Process process in Process.GetProcesses())
            {
                if (string.Compare(process.ProcessName, type.ToString(), true) == 0)
                {
                    if (mode == KillMode.Confirm)
                    {
                        if (MessageBox.Show(string.Format("End {0} process:{1} ?", process.ProcessName, process.MainWindowTitle)
                            , "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            continue;
                        }
                    }
                    try
                    {
                        process.Kill();
                        killcount++;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(string.Format("{0} can not be stoped:\n{1}", process.ProcessName, e.Message)
                            , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return killcount;
        }

        /// <summary>
        /// The enum of type of office
        /// </summary>
        public enum OfficeType
        {
            Excel,
            Winword
        }

        /// <summary>
        /// The enum of mode of kill
        /// </summary>
        public enum KillMode
        {
            Confirm,
            All
        }
    }
}

