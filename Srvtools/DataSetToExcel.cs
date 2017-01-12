using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using System.Collections;


namespace Srvtools
{
    public class DataSetToExcel
    {
        const int PACKETSIZE = 10000;
        const string SPREADSHEETSTRING = "urn:schemas-microsoft-com:office:spreadsheet";
        const string OFFICESTRING = "urn:schemas-microsoft-com:office:office";
        const string EXCELSTRING = "urn:schemas-microsoft-com:office:excel";

        private InfoDataSet _dataset;
        /// <summary>
        /// the dataset to export
        /// </summary>
        public InfoDataSet DataSet
        {
            get
            {
                return _dataset;
            }
            set
            {
                _dataset = value;
            }
        }

        private string _excelpath;
        /// <summary>
        /// the path of excel file
        /// </summary>
        public string Excelpath
        {
            get
            {
                return _excelpath;
            }   
            set
            {
                _excelpath = value;
            }
        }

        private string _filter;

        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        private string _Sort;

        public string Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }
	
        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        public List<string> IgnoreColumns = null;

        internal BeforeExportEventHandler BeforeExport = null;

        /// <summary>
        /// creat a new instance of DataSetToExcel
        /// </summary>
        public DataSetToExcel() : this(string.Empty, new InfoDataSet(), string.Empty) { }

        public DataSetToExcel(string ExcelPath, InfoDataSet InfoDataSet) : this(ExcelPath, InfoDataSet, string.Empty) { }

        public DataSetToExcel(string ExcelPath, InfoDataSet InfoDataSet, string title)
        {
            this.Excelpath = ExcelPath;
            this.DataSet = InfoDataSet;
            this.Title = title;
            _filter = string.Empty;
            _Sort = string.Empty;
        }

        public bool Export()
        {
            return Export(0);
        }

        public bool Export(string TableName)
        {
            if (!this.DataSet.RealDataSet.Tables.Contains(TableName))
            {
                throw new Exception("Table does't exsit");
            }
            if (DataSet.PacketRecords != -1)
            {
                int temp = DataSet.PacketRecords;
                DataSet.PacketRecords = PACKETSIZE;
                while (DataSet.GetNextPacket()) ;
                DataSet.PacketRecords = temp;
            }
            DataTable table = this.DataSet.RealDataSet.Tables[TableName];
            ToExcel(table);
            return true;

        }

        public bool Export(int Index)
        {
            if (Index > this.DataSet.RealDataSet.Tables.Count - 1)
            {
                throw new IndexOutOfRangeException("Index of table is out of range");
            }
            if (DataSet.PacketRecords != -1)
            {
                int temp = DataSet.PacketRecords;
                DataSet.PacketRecords = PACKETSIZE;
                while (DataSet.GetNextPacket()) ;
                DataSet.PacketRecords = temp;
            }
            DataTable table = this.DataSet.RealDataSet.Tables[Index];
            ToExcel(table);
            return true;
        }

        public bool ExportCSV(string TableName)
        {
            if (!this.DataSet.RealDataSet.Tables.Contains(TableName))
            {
                throw new Exception("Table does't exsit");
            }
            if (DataSet.PacketRecords != -1)
            {
                int temp = DataSet.PacketRecords;
                DataSet.PacketRecords = PACKETSIZE;
                while (DataSet.GetNextPacket()) ;
                DataSet.PacketRecords = temp;
            }
            return ExportCSV(this.DataSet.RealDataSet.Tables[TableName]);
        }

        public bool ExportCSV(int Index)
        {
            if (Index > this.DataSet.RealDataSet.Tables.Count - 1)
            {
                throw new Exception("Index is out of range");
            }
            return ExportCSV(this.DataSet.RealDataSet.Tables[Index]);
        }

        private void ToExcel(DataTable table)
        { 
            XmlDocument xml = CreateFile();
            XmlNode nodeworkbook = xml.SelectSingleNode("Workbook");
            Hashtable tableDD = GetDDTable(table);
            if (BeforeExport != null)
            {
                ExportArgs arg = new ExportArgs(table.TableName, Filter, Sort);
                BeforeExport(this, arg);
                Filter = arg.Filter;
                Sort = arg.Sort;
            }
            DataRow[] dr = table.Select(Filter, Sort);
            int maxrowcount = this.Title.Length > 0 ? 65534 : 65535;
            XmlNode nodetable = CreateWorkSheet(string.Format("{0}{1}", table.TableName, 0), nodeworkbook, table, tableDD);
            for (int i = 0; i < dr.Length; i++)
            {
                if (i != 0 && i % maxrowcount == 0)
                {
                    nodetable = CreateWorkSheet(string.Format("{0}{1}", table.TableName, i / maxrowcount), nodeworkbook, table, tableDD);
                }
                XmlNode noderow = xml.CreateElement("Row");
                nodetable.AppendChild(noderow);
                foreach (DataColumn dc in table.Columns)
                {
                    if (IgnoreColumns == null || !IgnoreColumns.Contains(dc.ColumnName))
                    {
                        ToExcel(noderow, dr[i][dc], dc.DataType);
                    }
                }
            }
            xml.Save(Excelpath);
        }

        private Hashtable GetDDTable(DataTable table)
        {
            Hashtable tableDD = new Hashtable();

            DataTable tableColdef = DBUtils.GetDataDictionary(DataSet, table.TableName, false).Tables[0];//DBAlias不同可能会出问题
            foreach (DataColumn dc in table.Columns)
	        {
        		DataRow[] drColdef = tableColdef.Select(string.Format("FIELD_NAME ='{0}'", dc.ColumnName.Replace("'","''")));
                if (drColdef.Length == 0)
                {
                    tableDD.Add(dc.ColumnName, dc.ColumnName);
                }
                else
                {
                    string value = drColdef[0]["CAPTION" + (((int)CliUtils.fClientLang) + 1).ToString()].ToString();
                    if (value.Length == 0)
                    {
                        value = drColdef[0]["CAPTION"].ToString();
                        if (value.Length == 0)
                        {
                            value = dc.ColumnName;
                        }
                    }
                    tableDD.Add(dc.ColumnName, value);
                }
            }
            return tableDD;
        }

        private void ToExcel(XmlNode nodeRow, object value, Type type)
        {
            XmlDocument xml = nodeRow.OwnerDocument;
            XmlNode nodecell = xml.CreateElement("Cell");
            nodeRow.AppendChild(nodecell);
            XmlNode nodedata = xml.CreateElement("Data");
            XmlAttribute atttype = xml.CreateAttribute("ss", "Type", SPREADSHEETSTRING);
            if (type == typeof(uint) || type == typeof(UInt16) || type == typeof(UInt32)
                         || type == typeof(UInt64) || type == typeof(int) || type == typeof(Int16)
                         || type == typeof(Int32) || type == typeof(Int64) || type == typeof(Single)
                         || type == typeof(Double) || type == typeof(Decimal))
            {
                atttype.Value = "Number";
                if (value == DBNull.Value)
                {
                    nodedata.InnerText = "0";      //empty value can not set to number column
                }
                else
                {
                    nodedata.InnerText = value.ToString();
                }
            }
            else if (type == typeof(DateTime))
            {
                atttype.Value = "String";
                nodedata.InnerText = value == DBNull.Value? "": ((DateTime)value).ToString("yyyy/MM/dd");
            }
            else
            {
                atttype.Value = "String";
                nodedata.InnerText = value.ToString().Replace(">", "&gt").Replace("<", "&lt");
            }
            nodedata.Attributes.Append(atttype);
            nodecell.AppendChild(nodedata);
        }

        private XmlNode CreateWorkSheet(string sheetName, XmlNode nodeWorkbook, DataTable table, Hashtable tableDD)
        {
            XmlDocument xml = nodeWorkbook.OwnerDocument;
            XmlNode nodeworksheet = xml.CreateElement("Worksheet");

            XmlAttribute attname = xml.CreateAttribute("ss", "Name", SPREADSHEETSTRING);
            attname.Value = sheetName;
            nodeworksheet.Attributes.Append(attname);
            XmlAttribute attxmln = xml.CreateAttribute("xmlns");
            attxmln.Value = SPREADSHEETSTRING;
            nodeworksheet.Attributes.Append(attxmln);
            nodeWorkbook.AppendChild(nodeworksheet);

            XmlNode nodetable = xml.CreateElement("Table");
            nodeworksheet.AppendChild(nodetable);
            if (this.Title.Length > 0)
            {
                XmlNode noderowtitle = xml.CreateElement("Row");
                nodetable.AppendChild(noderowtitle);

                XmlNode nodecell = xml.CreateElement("Cell");
                noderowtitle.AppendChild(nodecell);
                if (table.Columns.Count > 1)
                {
                    XmlAttribute attmerge = xml.CreateAttribute("ss", "MergeAcross", SPREADSHEETSTRING);
                    attmerge.Value = (table.Columns.Count - 1).ToString();
                    nodecell.Attributes.Append(attmerge);
                }
                XmlAttribute attstyle = xml.CreateAttribute("ss", "StyleID", SPREADSHEETSTRING);
                attstyle.Value = "title";
                nodecell.Attributes.Append(attstyle);
          
                XmlNode nodedata = xml.CreateElement("Data");
                XmlAttribute atttype = xml.CreateAttribute("ss", "Type", SPREADSHEETSTRING);
                atttype.Value = "String";
                nodedata.InnerText = this.Title;
                nodedata.Attributes.Append(atttype);
                nodecell.AppendChild(nodedata);
            }

            XmlNode noderow = xml.CreateElement("Row");
            nodetable.AppendChild(noderow);
            foreach (DataColumn dc in table.Columns)
            {
                if (IgnoreColumns == null || !IgnoreColumns.Contains(dc.ColumnName))
                {
                    ToExcel(noderow, tableDD[dc.ColumnName], typeof(string));
                }
            }

            return nodetable;
        }

        private XmlDocument CreateFile()
        {
            string directoryname = Path.GetDirectoryName(Excelpath);
            if (!Directory.Exists(directoryname))
            {
                Directory.CreateDirectory(directoryname);
            }
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", null, null));

            XmlNode nodeworkbook = xml.CreateElement("Workbook");
            XmlAttribute attxmlns = xml.CreateAttribute("xmlns");
            attxmlns.Value = SPREADSHEETSTRING;
            XmlAttribute attxmlnso = xml.CreateAttribute("xmlns:o");
            attxmlnso.Value = OFFICESTRING;
            XmlAttribute attxmlnsx = xml.CreateAttribute("xmlns:x");
            attxmlnsx.Value = EXCELSTRING;
            XmlAttribute attxmlnsss = xml.CreateAttribute("xmlns:ss");
            attxmlnsss.Value = SPREADSHEETSTRING;
            nodeworkbook.Attributes.Append(attxmlns);
            nodeworkbook.Attributes.Append(attxmlnso);
            nodeworkbook.Attributes.Append(attxmlnsx);
            nodeworkbook.Attributes.Append(attxmlnsss);
            xml.AppendChild(nodeworkbook);

            XmlNode nodestyles = xml.CreateElement("Styles");
            nodeworkbook.AppendChild(nodestyles);

            XmlNode nodestyle = xml.CreateElement("Style");
            XmlAttribute attid = xml.CreateAttribute("ss", "ID", SPREADSHEETSTRING);
            attid.Value = "title";
            nodestyle.Attributes.Append(attid);
            nodestyles.AppendChild(nodestyle);

            XmlElement nodealignment = xml.CreateElement("Alignment");
            XmlAttribute atthorizontal = xml.CreateAttribute("ss", "Horizontal", SPREADSHEETSTRING);
            atthorizontal.Value = "Center";
            nodealignment.Attributes.Append(atthorizontal);
            nodestyle.AppendChild(nodealignment);

            return xml;
        }

        private bool ExportCSV(DataTable dt)
        {
            FileStream fs = new System.IO.FileStream(this.Excelpath, FileMode.Create);
            StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.Unicode);
            try
            {
                string column = "";


                #region ColDef
                Hashtable tableDD = GetDDTable(dt);
               
                #endregion

                string[] value = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    column += tableDD[dt.Columns[i].ColumnName].ToString() + "\t";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        value[j] += dt.Rows[j][i].ToString().Replace("\r\n", " ").Replace("\n\r", " ").Replace("\t", " ") + "\t";
                    }
                }
                if (column != "")
                {
                    column = column.Substring(0, column.LastIndexOf("\t"));
                }
                sw.WriteLine(column);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (value[i] != "")
                    {
                        value[i] = value[i].Substring(0, value[i].LastIndexOf("\t"));
                    }
                    sw.WriteLine(value[i]);
                }
              

                //column.Replace("\t", ",");

                //OleDbConnection objConn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.Excelpath +
                //    ";Extended Properties=Excel 8.0;");
                //objConn.Open();
                //System.Data.OleDb.OleDbCommand objCmd = new System.Data.OleDb.OleDbCommand();
                //objCmd.Connection = objConn;

                //for (int i = 0; i < rowcount; i++)
                //{
                //    string value = "";
                //    for (int j = 0; j < columncount; j++)
                //    {
                //        value += "'" + dt.Rows[i][j].ToString() + "',";
                //    }
                //    if (value != "")
                //    {
                //        value = value.Substring(0, column.LastIndexOf(','));
                //    }

                //    objCmd.CommandText = "Insert into " + sheetname + " (" + column + ")" + " values (" + value + ")";
                //    objCmd.ExecuteNonQuery();
                //}

                //objConn.Close();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                sw.Flush();
                fs.Close();
            }
        }

    }
}
