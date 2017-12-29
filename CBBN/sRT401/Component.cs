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

            var s_row = "";
            var s1 = "";
            var sql = "";
            var i2_count = 0;
            var sf = "";
            var sbatch = "";
            try
            {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(srcPath);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                int jj = 1;
                DateTime dtDate = new DateTime(1900, 1, 1);

                //iterate over the rows and columns and print to the console as it appears in the file
                //excel is not zero based!!
                for (int i = 2; i <= rowCount; i++)
                {
                    s_row = "2";
                    i2_count++;
                    for (int j = 1; j <= colCount; j++)
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                        {
                            s1 = xlRange.Cells[i, j].Value2.ToString();
                            s1 = s1.Replace("'", "''");
                        }
                        else
                        {
                            s1 = "";
                        }

                        switch (j)
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
                                    if (xlRange.Cells[i, j].Value2 is DateTime)
                                    {
                                        dtDate = DateTime.Parse(xlRange.Cells[i, j].Value2);
                                        ss = dtDate.ToString("yyyy/MM/dd");
                                        s_row = s_row + ",'" + ss + "'";
                                    }
                                    else
                                    {
                                        dtDate = DateTime.FromOADate(Convert.ToInt32(xlRange.Cells[i, j].Value2));
                                        ss = dtDate.ToString("yyyy/MM/dd");
                                        s_row = s_row + ",'" + ss + "'";
                                    }
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

                sf = "二聯式處理筆數：" + i2_count.ToString();

                //處理三聯式的部分
                xlWorksheet = xlWorkbook.Sheets[2];
                xlRange = xlWorksheet.UsedRange;

                rowCount = xlRange.Rows.Count;
                colCount = xlRange.Columns.Count;
                jj = 1;
                i2_count = 0;

                //iterate over the rows and columns and print to the console as it appears in the file
                //excel is not zero based!!
                for (int i = 2; i <= rowCount; i++)
                {
                    i2_count++;
                    s_row = "3";
                    for (int j = 1; j <= colCount; j++)
                    {

                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                        {
                            s1 = xlRange.Cells[i, j].Value2.ToString();
                            s1 = s1.Replace("'", "''");
                        }
                        else
                        {
                            s1 = "";
                        }

                        switch (j)
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
                                    if (xlRange.Cells[i, j].Value2 is DateTime)
                                    {
                                        dtDate = DateTime.Parse(xlRange.Cells[i, j].Value2);
                                        ss = dtDate.ToString("yyyy/MM/dd");
                                        s_row = s_row + ",'" + ss + "'";
                                    }
                                    else
                                    {
                                        dtDate = DateTime.FromOADate(Convert.ToInt32(xlRange.Cells[i, j].Value2));
                                        ss = dtDate.ToString("yyyy/MM/dd");
                                        s_row = s_row + ",'" + ss + "'";
                                    }
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
                //cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //rule of thumb for releasing com objects:
                //  never use two dots, all COM objects must be referenced and released individually
                //  ex: [somthing].[something].[something] is bad

                //release com objects to fully kill excel process from running in the background
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);

                //close and release
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);

                //quit and release
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);

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
            }
            catch (Exception ex)
            {
                return new object[] { 0, "開啟現存 Excel 檔案失敗!!" + ex };
            }

            return new object[] { 0, "處理完畢!!"+ sf + " 列印批號："+ sbatch};
        }
    }
}
