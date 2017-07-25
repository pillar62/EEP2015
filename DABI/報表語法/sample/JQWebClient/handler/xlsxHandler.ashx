<%@ WebHandler Language="C#" Class="xlsxHandler" %>

using System;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;
using System.Data;
using System.Xml;
using System.IO;
using System.Text;
using EFClientTools.EFServerReference;
using System.Collections.Generic;
using System.Web.SessionState;
using DocumentFormat.OpenXml;
using ClosedXML;
using ClosedXML.Excel;
using System.Linq;

public class xlsxHandler : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        var mode = context.Request.Form["mode"];
        var filename = context.Request.Form["filename"];
        var customerno = context.Request.Form["customerno"];
        if (mode == "import")
        {
            ImportData(context, filename, customerno);
        }
    }
    public const string Data = "RemoteName";
    const string SPREADSHEETSTRING = "urn:schemas-microsoft-com:office:spreadsheet";
    const string OFFICESTRING = "urn:schemas-microsoft-com:office:office";
    const string EXCELSTRING = "urn:schemas-microsoft-com:office:excel";
    
    public void ImportData(HttpContext context, string fName, string custNo)
    {         
        if (context.Request.Files.Count > 0) //如果有上傳檔案
        {
            var stream = context.Request.Files[0].InputStream;
            try
            {
                HttpPostedFile f = context.Request.Files[0];
                string FileName = Path.GetFileNameWithoutExtension(fName);//取得檔名
                
                // 改檔名後存檔
                DirectoryInfo di = new DirectoryInfo(context.Server.MapPath("../Files")); //依照ashx所在的位置找
                string FrontStr = FileName + DateTime.Now.ToString("yyyyMMdd");
                FileInfo[] fi = di.GetFiles(string.Format("{0}*.xlsx", FrontStr), SearchOption.TopDirectoryOnly);
                int seq = 0;
                if (fi.Length > 0)
                {
                    seq = Convert.ToInt16(fi[fi.Length - 1].Name.ToLower().Replace(FrontStr.ToLower(), "").Replace(".xlsx", ""));
                }
                string SavePath = context.Server.MapPath("../Files/") + FrontStr + (seq + 1).ToString().PadLeft(3, '0') + ".xlsx";
                f.SaveAs(SavePath);
                
                // 加進 ServerMethod 參數
                List<object> dtList = new List<object>();
                dtList.Add(custNo);
                
                // 開啟 Excel
                var wb = new XLWorkbook(SavePath);
                IXLWorksheets wss = wb.Worksheets;
                
                // 依 Sheet 跑迴圈
                for (int si = 0; si < wss.Count; si++)
                {
                    //宣告暫存的 DataTable(撈結構出來等等回塞資料)
                    string SQL = "SELECT * FROM TestDetail WHERE 1=0";
                    DataTable dtXLSM = EFClientTools.ClientUtility.ExecuteSQL("ERPS", SQL).Tables[0];
                                     
                    var ws = wb.Worksheet(si + 1);

                    // Insert Into dtXLSM
                    int DETAIL_START = 3; //從Excel第三列開始撈資料  
                    int rowNo = Convert.ToInt16(DETAIL_START);
                    string TestID = "";
                    string TestSeq = "";
                    string TestTry = "";
                    do
                    {
                        ws.Cell("A" + rowNo.ToString()).TryGetValue(out TestID);
                        ws.Cell("B" + rowNo.ToString()).TryGetValue(out TestSeq);
                        ws.Cell("C" + rowNo.ToString()).TryGetValue(out TestTry);                                                  
                        rowNo++;
                        
                        // Insert dtXLSD
                        DataRow drD = dtXLSM.NewRow();
                        drD["TestID"] = TestID;
                        drD["TestSeq"] = TestSeq;
                        drD["TestTry"] = TestTry;
                        dtXLSM.Rows.Add(drD);
                     }
                    while (TestID != "");//結束時的條件

                    // 加進 ServerMethod 參數                        
                    string dtJSONM = JsonConvert.SerializeObject(dtXLSM, Newtonsoft.Json.Formatting.Indented);
                    dtList.Add(dtJSONM);                          
                }
                // 全部的 Sheet 讀取完畢後再呼叫 ServerMethod
                object obj = EFClientTools.ClientUtility.CallMethod("sTest", "ImportData", dtList);

                string[] result = obj.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (result[0].ToString() == "Y")
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("{SuccessMsg:'匯入成功, 已產生出庫單號: " + result[1].ToString() + "'}");
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("{SuccessMsg:'" + result[1].ToString() + "'}");
                }
            }
            catch (Exception e)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("'"+ e.Message.Replace("\'","　") + "'");
                return;
            }
        }
    }
    //
    #region  DataSet To Json
    /// <summary>    
    /// DataSet To Json   
    /// </summary>    
    /// <param name="dataSet">DataSet Source</param>   
    /// <returns>Json String</returns>    
    public static string ToJson(DataSet dataSet)
    {
        string jsonString = "{";
        foreach (DataTable table in dataSet.Tables)
        {
            jsonString += "\"" + table.TableName + "\":" + DataTableToJson(table,table.Rows.Count,string.Empty,table.TableName) + ",";
        }
        jsonString = jsonString.TrimEnd(',');
        return jsonString + "}";
    }
    #endregion
    public static string DataTableToJson(DataTable dt, int rowscount, string totaljs, string tableName)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string js = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);//Indented縮排
        sb.Append("{\"total\":");
        sb.Append(rowscount.ToString());
        sb.Append(",\"tableName\":");
        sb.Append("\"" + tableName + "\"");
        sb.Append(",\"keys\":");
        string sPrimaryKey = "";
        foreach (var PrimaryKey in dt.PrimaryKey)
        {
            if (sPrimaryKey != "")
            {
                sPrimaryKey += ",";
            }
            sPrimaryKey += PrimaryKey.ColumnName;
        }
        sb.Append("\"" + sPrimaryKey + "\"");
        sb.Append(",\"rows\":");
        sb.Append(js);
        if (totaljs != "")
        {
            sb.Append(totaljs);
        }
        sb.Append("}");
        return sb.ToString();
    }

    public string DataTableToJson(DataTable dt)
    {
        return JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);//Indented縮排
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    public static void ImportFromExcel(Stream file)
    {
        try
        {
            
            
            
            //var workbook = new XLWorkbook(file);
            ////new a excel object 
            //var xlWorksheet = workbook.Worksheet(1);

            ////Declare a excel sheet : xlWorksheet
            //var range = xlWorksheet.Range(xlWorksheet.FirstCellUsed(), xlWorksheet.LastCellUsed());

            
            

            
            ////以下把 EXCEL 裏的圖片中Id非rId1的圖片另存到 "C:\EEP2012\JQWebClient\GreatStar\QC-016\"目錄
            //var document = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(file, false);
            //DocumentFormat.OpenXml.Packaging.WorkbookPart wbPart = document.WorkbookPart;
            //var workSheet = wbPart.WorksheetParts.FirstOrDefault();
            //foreach (DocumentFormat.OpenXml.Packaging.ImagePart i in workSheet.DrawingsPart.ImageParts)
            //{   
            //    var rId = workSheet.DrawingsPart.GetIdOfPart(i);
            //    if (rId != "rId1")
            //    {
            //        Stream stream = i.GetStream();
            //        using (var fileStream = File.Create(@"C:\EEP2012\JQWebClient\Files\QC-016\" + ProductGroup + "_" + Version + ".png"))
            //        {
            //            stream.CopyTo(fileStream);
            //        }
            //    }

            //}

        }
        catch (Exception e)
        {
            throw new Exception( e.Message );
        }

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
            list.Add(listrow);
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
    private static List<List<string>> xlsxRead(Stream file)
    {
        List<List<string>> list = new List<List<string>>();

        try
        {

            var workbook = new XLWorkbook(file);
            var xlWorksheet = workbook.Worksheet(1);
            var range = xlWorksheet.Range(xlWorksheet.FirstCellUsed(), xlWorksheet.LastCellUsed());

            int col = range.ColumnCount();
            int row = range.RowCount();

            for (int i = 1 ; i <= row ; i++ ) 
            {
                    List<string> listrow = new List<string>();
                    for (int y = 1; y <= col; y++)
                    {
                        listrow.Add(range.Row(i).Cell(y).Value.ToString());
                    }
                    list.Add(listrow);                
            }            
        }
        catch (Exception e)
        {
            throw new Exception(e.Message+"或非EXCEL2007以後版本(xlsx)的檔案!");
        }
        return list;
    }
    public MemoryStream TransferDataTableToExcel(DataTable dt)
    {
        var wb = new XLWorkbook();
        wb.Worksheets.Add(dt);
        MemoryStream ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms;
    }

    public DataTable TransferExcelToDataTable(byte[] file)
    {
        Stream fileStream = new MemoryStream(file);
        var workbook = new XLWorkbook(fileStream);
        var xlWorksheet = workbook.Worksheet(1);
        return TransferExcelToDataTable(xlWorksheet);
    }

    public DataTable TransferExcelToDataTable(byte[] file, string sheetName)
    {
        Stream fileStream = new MemoryStream(file);
        var workbook = new XLWorkbook(fileStream);
        var xlWorksheet = workbook.Worksheet(sheetName);
        return TransferExcelToDataTable(xlWorksheet);
    }

    public DataTable TransferExcelToDataTable(string filePath)
    {
        var workbook = new XLWorkbook(filePath);
        var xlWorksheet = workbook.Worksheet(1);
        return TransferExcelToDataTable(xlWorksheet);
    }

    public DataTable TransferExcelToDataTable(string filePath, string sheetName)
    {
        var workbook = new XLWorkbook(filePath);
        var xlWorksheet = workbook.Worksheet(sheetName);
        return TransferExcelToDataTable(xlWorksheet);
    }

    private DataTable TransferExcelToDataTable(IXLWorksheet xlWorksheet)
    {
        var datatable = new DataTable();
        var range = xlWorksheet.Range(xlWorksheet.FirstCellUsed(), xlWorksheet.LastCellUsed());

        int col = range.ColumnCount();
        int row = range.RowCount();

        // add columns hedars
        datatable.Clear();

        for (int i = 1; i <= col; i++)
        {
            IXLCell column = xlWorksheet.Cell(1, i);
            datatable.Columns.Add(column.Value.ToString());
        }

        // add rows data   
        int firstHeadRow = 0;
        foreach (var item in range.Rows())
        {
            if (firstHeadRow != 0)
            {
                var array = new object[col];
                for (int y = 1; y <= col; y++)
                {
                    array[y - 1] = item.Cell(y).Value;
                }
                datatable.Rows.Add(array);
            }
            firstHeadRow++;
        }
        return datatable;
    }


    public IDataReader TransferExcelToIDataReader(byte[] file)
    {
        Stream fileStream = new MemoryStream(file);
        var workbook = new XLWorkbook(fileStream);
        var xlWorksheet = workbook.Worksheet(1);
        return TransferExcelToIDataReader(xlWorksheet);
    }

    public IDataReader TransferExcelToIDataReader(byte[] file, string sheetName)
    {
        Stream fileStream = new MemoryStream(file);
        var workbook = new XLWorkbook(fileStream);
        var xlWorksheet = workbook.Worksheet(sheetName);
        return TransferExcelToIDataReader(xlWorksheet);
    }

    public IDataReader TransferExcelToIDataReader(string filePath)
    {
        var workbook = new XLWorkbook(filePath);
        var xlWorksheet = workbook.Worksheet(1);
        return TransferExcelToIDataReader(xlWorksheet);
    }

    public IDataReader TransferExcelToIDataReader(string filePath, string sheetName)
    {
        var workbook = new XLWorkbook(filePath);
        var xlWorksheet = workbook.Worksheet(sheetName);
        return TransferExcelToIDataReader(xlWorksheet);
    }

    private IDataReader TransferExcelToIDataReader(IXLWorksheet xlWorksheet)
    {
        var datatable = new DataTable();
        var range = xlWorksheet.Range(xlWorksheet.FirstCellUsed(), xlWorksheet.LastCellUsed());

        int col = range.ColumnCount();
        int row = range.RowCount();

        // add columns hedars
        datatable.Clear();

        for (int i = 1; i <= col; i++)
        {
            IXLCell column = xlWorksheet.Cell(1, i);
            datatable.Columns.Add(column.Value.ToString());
        }

        // add rows data   
        int firstHeadRow = 0;
        foreach (var item in range.Rows())
        {
            if (firstHeadRow != 0)
            {
                var array = new object[col];
                for (int y = 1; y <= col; y++)
                {
                    array[y - 1] = item.Cell(y).Value;
                }
                datatable.Rows.Add(array);
            }
            firstHeadRow++;
        }
        return datatable.CreateDataReader();
    }



    public MemoryStream TransferDataTableToCsv(DataTable dt)
    {
        MemoryStream ms = new MemoryStream();
        StreamWriter result = new StreamWriter(ms, System.Text.Encoding.UTF8);

        //Header
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            result.Write(dt.Columns[i].ColumnName);
            result.Write(i == dt.Columns.Count - 1 ? "\n" : ",");
        }

        //Content
        foreach (DataRow row in dt.Rows)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                result.Write(row[i].ToString());
                result.Write(i == dt.Columns.Count - 1 ? "\n" : ",");
            }
        }

        return ms;
    }

    public DataTable TransferCsvToDataTable(string strFilePath)
    {
        string strFileName = Path.GetFileName(strFilePath);
        string strFileDirectory = Path.GetDirectoryName(strFilePath);
        string strConn = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}\;Extended Properties='Text;HDR=Yes;'", strFileDirectory);
        string strSQL = string.Format("SELECT * FROM [{0}]", strFileName);
        System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(strSQL, strConn);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }

}