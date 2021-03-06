using Newtonsoft;
using Newtonsoft.Json;
using Srvtools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Extractor;


namespace sRT401
{
    public partial class Component : DataModule
    {
        public Component()
        {
            InitializeComponent();
        }

        public Component(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
                
        public object[] smRT401(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            string str = System.Environment.CurrentDirectory + @"\..\"; 
            var srcPath = str + "\\JQWebClient\\excel\\" + sdata[0];
            //srcPath = @"C:\EEP2015\JQWebClient\excel\106012.xls";
            //開啟資料連接
            IDbConnection conn = cmd.Connection;
            conn.Open();

            //刪除暫存檔
            string selectSql = " delete from rtinvtemp ";
            cmd.CommandText = selectSql;
            double i1 = cmd.ExecuteNonQuery();

            //指定 excel 檔
            FileStream fs = new FileStream(srcPath, FileMode.Open);
            HSSFWorkbook workbook = new HSSFWorkbook(fs);

            //指定第一個業面
            HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(0);

            //標題列
            HSSFRow headerRow =  (HSSFRow)sheet.GetRow(0);
            //取得最後一個欄位的位址
            int cellCount = headerRow.LastCellNum; 

            //指定標題列的處理 這一段可以省略
            /*
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            */

            //取得資料筆數
            var s_row = "";
            var s1 = "";
            var sql = "";
            var i2_count = 0;
            var sf = "";
            var sbatch = "";
            try
            {
               
                int rowCount = sheet.LastRowNum;
                int colCount = cellCount;
                DateTime dtDate = new DateTime(1900, 1, 1);

                //iterate over the rows and columns and print to the console as it appears in the file
                //excel is not zero based!!
                for (int i = 1; i <= rowCount; i++)
                {
                    HSSFRow row = (HSSFRow)sheet.GetRow(i);
                    s_row = "2";
                    i2_count++;
                    for (int j = 0; j <= colCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (row.GetCell(j).CellType == NPOI.SS.UserModel.CellType.Formula)
                            { s1 = row.GetCell(j).NumericCellValue.ToString(); }
                            else
                            if (row.GetCell(j).CellType == NPOI.SS.UserModel.CellType.Numeric)
                            { s1 = row.GetCell(j).NumericCellValue.ToString(); }
                            else
                            if (row.GetCell(j).CellType == NPOI.SS.UserModel.CellType.String)
                            { s1 = row.GetCell(j).StringCellValue; }
                            else
                            { s1 = row.GetCell(j).ToString(); }
                            s1 = s1.Replace("'", "''");
                        }
                        else
                        {
                            s1 = "";
                        }

                        switch (j+1)
                        {
                            case 1: //文字欄位
                            case 2:
                                s_row = s_row + ",'" + s1 + "'";
                                break;
                            case 3://數字欄位
                            case 4://數字欄位
                            case 5://數字欄位
                                s_row = s_row + "," + s1;
                                break;
                            case 6:
                                s_row = s_row + ",'" + s1 + "'";
                                break;
                            case 7://數字欄位
                            case 8://數字欄位
                                s_row = s_row + "," + s1;
                                break;
                            case 9://日期欄位
                                {
                                   
                                        dtDate = DateTime.FromOADate(Convert.ToInt32(s1));
                                        ss = dtDate.ToString("yyyy/MM/dd");
                                        s_row = s_row + ",'" + ss + "'";
                                   
                                    break;
                                }
                            case 10: //文字欄位
                            case 11: //文字欄位
                            case 12: //文字欄位
                            case 13: //文字欄位
                            case 14: //文字欄位
                            case 15: //文字欄位
                            case 16: //文字欄位                                
                                s_row = s_row + ",'" + s1 + "'";
                                break;
                            case 17:
                                break;
                            case 18: //文字欄位
                            case 19: //文字欄位
                            case 20: //文字欄位
                            case 21: //文字欄位
                                s_row = s_row + ",'" + s1 + "'";
                                break;
                            default:
                                break;
                        }
                    }


                    sql = @"INSERT INTO rtinvtemp (INVTYPE, GROUPNC, PRODNC, QTY, UNITAMT
                            , RCVAMT, TAXTYPE, SALEAMT, TAXAMT, INVDAT
                            , INVNO, UNINO, INVTITLE, 社區名稱, 用戶名稱
                            , 地址, 聯絡電話, 施工人員, 業務開發單位, 業務開發人員, 備註) VALUES (" + s_row + ")";
                    //System.IO.File.WriteAllText(@"D:\WriteText.txt", sql);
                    cmd.CommandText = sql;
                    try
                    {
                        i1 = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return new object[] { 0, "執行失敗!!" + ex + sql };
                    }
                }

                sf = "二聯式處理筆數：" + i2_count.ToString();

                //處理三聯式的部分
                //指定第2個業面
                HSSFSheet sheet2 = (HSSFSheet)workbook.GetSheetAt(1);

                //標題列
                HSSFRow headerRow2 = (HSSFRow)sheet2.GetRow(0);
                //取得最後一個欄位的位址
                int cellCount2 = headerRow2.LastCellNum;                

                cellCount = headerRow2.LastCellNum;
                i2_count = 0;
                rowCount = sheet2.LastRowNum;
                colCount = cellCount2;

                //iterate over the rows and columns and print to the console as it appears in the file
                //excel is not zero based!!
                for (int i = 1; i <= rowCount; i++)
                {
                    HSSFRow row2 = (HSSFRow)sheet2.GetRow(i);
                    i2_count++;
                    s_row = "3";
                    for (int j = 0; j <= colCount; j++)
                    {
                        if (row2.GetCell(j) != null)
                        {
                            if (row2.GetCell(j).CellType == NPOI.SS.UserModel.CellType.Formula)
                            { s1 = row2.GetCell(j).NumericCellValue.ToString(); }
                            else
                            if (row2.GetCell(j).CellType == NPOI.SS.UserModel.CellType.Numeric)
                            { s1 = row2.GetCell(j).NumericCellValue.ToString(); }
                            else
                            if (row2.GetCell(j).CellType == NPOI.SS.UserModel.CellType.String)
                            { s1 = row2.GetCell(j).StringCellValue; }
                            else
                            { s1 = row2.GetCell(j).ToString(); }

                            s1 = s1.Replace("'", "''");
                        }
                        else
                        {
                            s1 = "";
                        }

                        switch (j+1)
                        {
                            case 1: //文字欄位
                            case 2:
                                s_row = s_row + ",'" + s1 + "'";
                                break;
                            case 3://數字欄位
                            case 4://數字欄位
                            case 5://數字欄位
                                s_row = s_row + "," + s1;
                                break;
                            case 6:
                                s_row = s_row + ",'" + s1 + "'";
                                break;
                            case 7://數字欄位
                            case 8://數字欄位
                                s_row = s_row + "," + s1;
                                break;
                            case 9://日期欄位
                                {
                                    dtDate = DateTime.FromOADate(Convert.ToInt32(s1));
                                    ss = dtDate.ToString("yyyy/MM/dd");
                                    s_row = s_row + ",'" + ss + "'";
                                    break;
                                }
                            case 10: //文字欄位
                            case 11: //文字欄位
                            case 12: //文字欄位
                            case 13: //文字欄位
                            case 14: //文字欄位
                            case 15: //文字欄位
                            case 16: //文字欄位                                
                                s_row = s_row + ",'" + s1 + "'";
                                break;
                            case 17:
                                break;
                            case 18: //文字欄位
                            case 19: //文字欄位
                            case 20: //文字欄位
                            case 21: //文字欄位
                                s_row = s_row + ",'" + s1 + "'";
                                break;
                            default:
                                break;
                        }
                    }


                    sql = @"INSERT INTO rtinvtemp (INVTYPE, GROUPNC, PRODNC, QTY, UNITAMT
                            , RCVAMT, TAXTYPE, SALEAMT, TAXAMT, INVDAT
                            , INVNO, UNINO, INVTITLE, 社區名稱, 用戶名稱
                            , 地址, 聯絡電話, 施工人員, 業務開發單位, 業務開發人員, 備註) VALUES (" + s_row + ")";
                    System.IO.File.WriteAllText(@"D:\WriteText.txt", sql);
                    cmd.CommandText = sql;
                    try
                    {
                        i1 = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return new object[] { 0, "執行失敗!!" + ex + sql };
                    }
                }

                sf = sf + "三聯式處理筆數：" + i2_count.ToString();

                //跑預儲程序 
                //conn = cmdRT4011.Connection;
                //conn.Open();
                //設定輸入參數的值
                conn = cmdRT4011.Connection;
                conn.Open();
                try
                {
                    var suser = GetClientInfo(ClientInfoType.LoginUser);
                    cmdRT4011.InfoParameters[0].Value = suser;
                    i1 = cmdRT4011.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return new object[] { 0, ex };
                }

                selectSql = " SELECT Max(BATCH) as maxbatch FROM RTInvoice ";
                cmd.CommandText = selectSql;
                DataSet ds = cmd.ExecuteDataSet();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["maxbatch"].ToString() != "")
                    {
                        sbatch = ds.Tables[0].Rows[0]["maxbatch"].ToString();
                    }
                    else
                    {
                        sbatch = "";
                    }
                }

                workbook = null;
                sheet = null;
                sheet2 = null;
            }
            catch (Exception ex)
            {
                return new object[] { 0, "開啟現存 Excel 檔案失敗!!" + ex };
            }

            return new object[] { 0, "處理完畢!!"+ sf + " 列印批號："+ sbatch};
        }
    }
}
