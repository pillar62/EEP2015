using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using System.IO;
using EFClientTools.EFServerReference;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Summary description for JQUtility
/// </summary>
public static class JQUtility
{
    const string SPREADSHEETSTRING = "urn:schemas-microsoft-com:office:spreadsheet";
    const string OFFICESTRING = "urn:schemas-microsoft-com:office:office";
    const string EXCELSTRING = "urn:schemas-microsoft-com:office:excel";


    #region Export

    public static void ExportToExcel(DataTable table, string filePath, string title, List<string> columns)
    {
        ExportToExcel(table, filePath, title, columns, null);
    }

    public static void ExportToExcel(DataTable table, string filePath, string title, List<string> columns, List<string> totals)
    {
        XmlDocument xml = CreateFile(filePath);
        XmlNode nodeworkbook = xml.SelectSingleNode("Workbook");
        if (columns.Count == 0)
        {
            foreach (DataColumn column in table.Columns)
            {
                columns.Add(column.ColumnName);
            }
        }

        int maxrowcount = title.Length > 0 ? 65534 : 65535;
        XmlNode nodetable = CreateWorkSheet(string.Format("{0}{1}", table.TableName, 0), nodeworkbook, table, title, columns);
        for (int i = 0; i < table.Rows.Count; i++)
        {
            if (i != 0 && i % maxrowcount == 0)
            {
                nodetable = CreateWorkSheet(string.Format("{0}{1}", table.TableName, i / maxrowcount), nodeworkbook, table, title, columns);
            }
            XmlNode noderow = xml.CreateElement("Row");
            nodetable.AppendChild(noderow);
            foreach (var column in columns)
            {
                DataColumn dc = table.Columns[column];
                ToExcel(noderow, table.Rows[i][dc], dc.DataType);
            }
        }
        if (totals != null)
        {
            XmlNode noderow = xml.CreateElement("Row");
            for (int i = 0; i < columns.Count; i++)
            {
                var total = i < totals.Count ? totals[i] : string.Empty;
                ToExcelTotal(noderow, total, table.Rows.Count % maxrowcount);
            }
            nodetable.AppendChild(noderow);
        }
        xml.Save(filePath);
        var content = string.Empty;
        using (StreamReader reader = new StreamReader(filePath))
        {
            content = reader.ReadToEnd();
        }
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.Write(content.Replace("\n", "&#10;"));
        }
    }

    private static void ToExcelTotal(XmlNode nodeRow, string total, int rowCount)
    {
        XmlDocument xml = nodeRow.OwnerDocument;
        XmlNode nodecell = xml.CreateElement("Cell");
        if (!string.IsNullOrEmpty(total))
        {
            XmlAttribute attFormula = xml.CreateAttribute("ss", "Formula", SPREADSHEETSTRING);
            attFormula.Value = string.Format("={0}(R[-{1}]C:R[-1]C)", total.ToUpper(), rowCount);
            nodecell.Attributes.Append(attFormula);
        }
       
        nodeRow.AppendChild(nodecell);
    }

    private static void ToExcel(XmlNode nodeRow, object value, Type type)
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
            nodedata.InnerText = value == DBNull.Value ? "" : ((DateTime)value).ToString("yyyy/MM/dd");
        }
        else
        {
            atttype.Value = "String";
            nodedata.InnerText = value.ToString().Replace(">", "&gt").Replace("<", "&lt");
            if (value.ToString().Contains("\n"))
            {
                XmlAttribute attstyle = xml.CreateAttribute("ss", "StyleID", SPREADSHEETSTRING);
                attstyle.Value = "multiline";
                nodecell.Attributes.Append(attstyle);
            }
        }
        nodedata.Attributes.Append(atttype);
        nodecell.AppendChild(nodedata);
    }

    private static XmlNode CreateWorkSheet(string sheetName, XmlNode nodeWorkbook, DataTable table, string title, List<string> columns)
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

        if (title.Length > 0)
        {
            XmlNode noderowtitle = xml.CreateElement("Row");
            nodetable.AppendChild(noderowtitle);

            XmlNode nodecell = xml.CreateElement("Cell");
            noderowtitle.AppendChild(nodecell);
            if (columns.Count > 1)
            {
                XmlAttribute attmerge = xml.CreateAttribute("ss", "MergeAcross", SPREADSHEETSTRING);
                attmerge.Value = (columns.Count - 1).ToString();
                nodecell.Attributes.Append(attmerge);
            }
            XmlAttribute attstyle = xml.CreateAttribute("ss", "StyleID", SPREADSHEETSTRING);
            attstyle.Value = "title";
            nodecell.Attributes.Append(attstyle);

            XmlNode nodedata = xml.CreateElement("Data");
            XmlAttribute atttype = xml.CreateAttribute("ss", "Type", SPREADSHEETSTRING);
            atttype.Value = "String";
            nodedata.InnerText = title;
            nodedata.Attributes.Append(atttype);
            nodecell.AppendChild(nodedata);
        }

        XmlNode noderow = xml.CreateElement("Row");
        nodetable.AppendChild(noderow);
        foreach (var column in columns)
        {
            DataColumn dc = table.Columns[column];
            ToExcel(noderow, dc.Caption, typeof(string));
        }

        return nodetable;
    }

    private static XmlDocument CreateFile(string filePath)
    {
        string directoryname = Path.GetDirectoryName(filePath);
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

        XmlNode nodeMultiLine = xml.CreateElement("Style");
        attid = xml.CreateAttribute("ss", "ID", SPREADSHEETSTRING);
        attid.Value = "multiline";
        nodeMultiLine.Attributes.Append(attid);
        nodestyles.AppendChild(nodeMultiLine);

        nodealignment = xml.CreateElement("Alignment");
        XmlAttribute attvertical = xml.CreateAttribute("ss", "Vertical", SPREADSHEETSTRING);
        attvertical.Value = "Center";
        nodealignment.Attributes.Append(attvertical);
        nodeMultiLine.AppendChild(nodealignment);

        XmlAttribute attwraptext = xml.CreateAttribute("ss", "WrapText", SPREADSHEETSTRING);
        attwraptext.Value = "1";
        nodealignment.Attributes.Append(attwraptext);
        nodeMultiLine.AppendChild(nodealignment);


        return xml;
    } 
    #endregion

    #region Import
    public static void ImportFromExcel(DataTable table, Stream file, int beginrow, int begincell)
    {
        List<List<string>> list = XmlRead(file);

        CheckAllowDBNull(table, GetMinColumnCount(list, beginrow, begincell));

        DataTable tableread = table.Clone();
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

        table.Merge(tableread);
    }

    private static int GetMinColumnCount(List<List<string>> list, int beginrow, int begincell)
    {
        int count = list.Count;
        int min = int.MaxValue;
        for (int i = beginrow; i < count; i++)
        {
            min = Math.Min(list[i].Count, min);
        }
        return Math.Max(min - begincell, 0);
    }

    private static List<List<string>> XmlRead(Stream file)
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
        xmlmgr.AddNamespace("sheet", SPREADSHEETSTRING);
        XmlNode table = xml.SelectSingleNode("/sheet:Workbook/sheet:Worksheet/sheet:Table", xmlmgr);
        XmlNodeList rows = table.SelectNodes("sheet:Row", xmlmgr);
        //解决空行的问题
        int rowindex = 0;
        foreach (XmlNode row in rows)
        {
            if (row.Attributes["Index", SPREADSHEETSTRING] != null)
            {
                while (rowindex < Convert.ToInt32(row.Attributes["Index", SPREADSHEETSTRING].Value) - 1)
                {
                    list.Add(new List<string>());
                    rowindex++;
                }
            }
            List<string> listrow = new List<string>();
            int celindex = 0;
            foreach (XmlNode cell in row.ChildNodes)
            {
                if (cell.Attributes["Index", SPREADSHEETSTRING] != null)
                {
                    while (celindex < Convert.ToInt32(cell.Attributes["Index", SPREADSHEETSTRING].Value) - 1)
                    {
                        listrow.Add(string.Empty);
                        celindex++;
                    }
                }
                listrow.Add(cell.InnerText.Trim());
                celindex++;
            }
            if (listrow.Exists(c => !string.IsNullOrEmpty(c)))
            {
                list.Add(listrow);
            }
            rowindex++;
        }
        return list;
    }

    private static void CheckAllowDBNull(DataTable table, int index)
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
    #endregion

    public static string DateTimeFormat = "yyyy-MM-dd";

    public static string GetSingleSignOnKey(string userId, string dataBase, string solution, string dataBaseType, string[] groups, string ipAddress)
    {
        var keys = new List<string>();
        keys.Add(userId);
        keys.Add(dataBase);
        keys.Add(solution);
        keys.Add(ipAddress);
        keys.Add(dataBaseType);
        keys.Add(string.Join(";", groups));
        //keys.Add("1");
        
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] pwdBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(string.Join(string.Empty, keys.ToArray())));
        byte[] result = md5.ComputeHash(pwdBytes);
        string ss = BitConverter.ToString(result);
        keys.Add(ss);

        return string.Join("-", keys);
    }

    public static EFClientTools.EFServerReference.ClientInfo CheckSingleSignOnKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new Exception("PublishKey is null");
        }
        var keys = key.Split('-');
        if (keys.Length >= 7)
        {
            var userId = keys[0];
            var dataBase = keys[1];
            var solution = keys[2];
            var ipAddress = keys[3];
            var dataBaseType = keys[4];
            var groups = keys[5].Split(';');
            if (GetSingleSignOnKey(userId, dataBase, solution, dataBaseType, groups, ipAddress) == key)
            {
                var clientInfo = new ClientInfo()
                {
                    UserID = userId,
                    Password = "",
                    Database = dataBase,
                    Solution = solution,
                    IPAddress = ipAddress,
                    DatabaseType = dataBaseType,
                    UseDataSet = true,
                    LogonResult = LogonResult.Logoned
                };
                clientInfo.Groups = new List<GroupInfo>();
                foreach (var group in groups)
                {
                    if (!string.IsNullOrEmpty(group))
                    {
                        clientInfo.Groups.Add(new GroupInfo() { ID = group, Name = group, Type = GroupType.Normal });
                    }
                }
                return clientInfo;
            }
            else
            {
                throw new Exception("PublishKey is invalid");
            }
        }
        else
        {
            throw new Exception("PublishKey is invalid");
        }

       
    }
}
