using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.IO;
using System.Xml;

namespace Srvtools
{
    internal class DataFileReader
    {
        protected DataFileReader() : this(new DataTable(), DataFileReaderType.Xls) { }

        public DataFileReader(DataTable table) : this(table, DataFileReaderType.Xls) { }

        public DataFileReader(DataTable table, DataFileReaderType type)
        {
            _Table = table;
            _ReaderType = type;
        }

        private DataTable _Table;

        public DataTable Table
        {
            get { return _Table; }
        }

        private DataFileReaderType _ReaderType;

        public DataFileReaderType ReaderType
        {
            get { return _ReaderType; }
        }

        public void Read(string file)
        {
            Read(file, 0, 0);
        }

        public void Read(string file, int beginrow, int begincell)
        {
            if (Table == null)
            {
                throw new NullReferenceException("DataTable is null");
            }
            if (!File.Exists(file))
            {
                throw new FileNotFoundException(string.Format("Can not find file: {0}", file));
            }
            List<List<string>> list = new List<List<string>>();
            if (ReaderType == DataFileReaderType.Xls)
            {
                list = XmlRead(file);
            }
            else
            {
                list = TxtRead(file);
            }
            CheckAllowDBNull(Table, GetMinColumnCount(list, beginrow, begincell));

            DataTable tableread = Table.Clone();
            tableread.Rows.Clear();

            for (int i = beginrow; i < list.Count; i++)
            {
                DataRow dr = tableread.NewRow();
                for (int j = begincell; j < list[i].Count && j < tableread.Columns.Count; j++)
                {
                    object value = DBNull.Value;
                    try
                    {
                        if (tableread.Columns[j].DataType != typeof(Guid))
                        {
                            //modify by lily 2011-1-14 小數點後面超過兩位時Decimal類型的資料無法取出
                            if (tableread.Columns[j].DataType == typeof(decimal))
                            {
                                value = Convert.ToDecimal(double.Parse(list[i][j]));
                            }
                            else
                            {
                                value = Convert.ChangeType(list[i][j], tableread.Columns[j].DataType);
                            }
                        }
                        else
                        {
                            Guid id = new Guid(list[i][j]);
                        }
                    }
                    catch { }
                    if (value == DBNull.Value && !tableread.Columns[j].AllowDBNull)
                    {
                        throw new NoNullAllowedException(string.Format("Column:{0} in table does not allow null value,but data in excel file can not cast"
                            , tableread.Columns[j].ColumnName));
                    }
                    if (value == DBNull.Value && tableread.Columns[j].DataType == typeof(bool))
                    {
                        value = false;
                    }
                    dr[j] = value;
                }
                tableread.Rows.Add(dr);
            }

            Table.Merge(tableread);
        }

        public static string TxtSplitChar = ",\t";

        private int GetMinColumnCount(List<List<string>> list, int beginrow, int begincell)
        {
            int count = list.Count;
            int min = int.MaxValue;
            for (int i = beginrow; i < count; i++)
            {
                min = Math.Min(list[i].Count, min);
            }
            return Math.Max(min - begincell, 0);
        }

        private List<List<string>> TxtRead(string file)
        {
            List<List<string>> list = new List<List<string>>();
            StreamReader sr = new StreamReader(file, Encoding.Default);//编码有可能有问题
            while (sr.Peek() != -1)
            {
                string strrow = sr.ReadLine();
                if (strrow.Trim().Length > 0)
                {
                    string[] strcells = strrow.Split(DataFileReader.TxtSplitChar.ToCharArray());
                    List<string> listrow = new List<string>();
                    foreach (string str in strcells)
                    {
                        listrow.Add(str.Trim());
                    }
                    list.Add(listrow);
                }
            }
            return list;
        }

        private List<List<string>> XmlRead(string file)
        {
            List<List<string>> list = new List<List<string>>();
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(file);
            }
            catch
            {
                throw new XmlException("File is not stored as xml file, you can select Officetools.ExcelRead as alternative");
            }
            XmlNamespaceManager xmlmgr = new XmlNamespaceManager(xml.NameTable);
            xmlmgr.AddNamespace("sheet", "urn:schemas-microsoft-com:office:spreadsheet");
            XmlNode table = xml.SelectSingleNode("/sheet:Workbook/sheet:Worksheet/sheet:Table",xmlmgr);
            XmlNodeList rows = table.SelectNodes("sheet:Row", xmlmgr);
            //解决空行的问题
            int rowindex = 0;
            foreach (XmlNode row in rows)
            {
                if (row.Attributes["Index", "urn:schemas-microsoft-com:office:spreadsheet"] != null)
                {
                    while (rowindex < Convert.ToInt32(row.Attributes["Index", "urn:schemas-microsoft-com:office:spreadsheet"].Value) - 1)
                    {
                        list.Add(new List<string>());
                        rowindex++;
                    }
                }
                List<string> listrow = new List<string>();
                int celindex = 0;
                foreach (XmlNode cell in row.ChildNodes)
                {
                    if (cell.Attributes["Index", "urn:schemas-microsoft-com:office:spreadsheet"] != null)
                    {
                        while (celindex < Convert.ToInt32(cell.Attributes["Index", "urn:schemas-microsoft-com:office:spreadsheet"].Value) - 1)
                        {
                            listrow.Add(string.Empty);
                            celindex++;
                        }
                    }
                    listrow.Add(cell.InnerText.Trim());
                    celindex++;
                }
                list.Add(listrow);
                rowindex++;
            }
            return list;
        }

        private void CheckAllowDBNull(DataTable table, int index)
        {
            for (int i = index; i < table.Columns.Count; i++)
            {
                if (!table.Columns[i].AllowDBNull)
                {
                    throw new NoNullAllowedException(string.Format("Column:{0} in table does not allow null value,but no data in excel file"
                        , table.Columns[i].ColumnName));
                }
            }
        }

    }

    public enum DataFileReaderType
    { 
        Txt,
        Xls
    }
}
