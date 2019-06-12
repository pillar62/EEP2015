using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Srvtools;
using System.Web;

namespace sRT307
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

        public object[] smRT3071(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmd.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                //會員編號
                string selectSql = @"SELECT KIND, CODE, CODENC FROM RTCode WHERE KIND='S4' AND CODE='1'";
                cmd.CommandText = selectSql;
                DataSet ds = cmd.ExecuteDataSet();
                string P1 = ds.Tables[0].Rows[0]["CODENC"].ToString();
                
                //商店代號
                selectSql = @"SELECT KIND, CODE, CODENC FROM RTCode WHERE KIND='S4' AND CODE='2'";
                cmd.CommandText = selectSql;
                ds = cmd.ExecuteDataSet();
                string P2 = ds.Tables[0].Rows[0]["CODENC"].ToString();
                //發票明細錄(S開頭)
                /*
                 1、明細錄別(S)。
                 2、商店自訂編號。
                 3、發票種類。B2C或B2B
                 4、買受人統一編號，B2C的時候帶空值
                 5、買受人名稱。
                 6、電子信箱。
                 7、買受人地址。
                 8、載具類別：0、手條條碼，1、自然人，2、智付寶載具。
                 9、載具編號：放客戶代號。
                 10、愛心碼
                 11、索取紙本發票。
                 ---第一行做到這邊。
                 12、稅別
                 13、稅率
                 14、銷售合計
                 15、稅額
                 16、發票金額
                 17、備註
                 */
                selectSql = @"SELECT 'S,'+ PRTNO +','+CASE 
                            WHEN ISNULL(UNINO, '') = '' AND ISNULL(LTRIM(EMAIL), '') = '' THEN 'B2C,,' + CUSNC + ',' + 'invoice@cbbn.com.tw' + ',' + raddr + ',2,' + PICODE + ',,N,'
                            WHEN ISNULL(UNINO, '') = '' THEN 'B2C,,' + CUSNC + ',' + EMAIL + ',' + raddr + ',2,' + PICODE + ',,N,'
                            ELSE 'B2B,' + UNINO + ',' + invtitle + ',' + EMAIL + ',' + raddr + ',,,,Y,' END 
                                        + '1,5,' + Convert(varchar(50), Convert(FLOAT(50), ROUND(AMT / 1.05, 0)))
                                        + ',' + Convert(varchar(50), Convert(FLOAT(50), AMT - ROUND(AMT / 1.05, 0))) + ',' + Convert(varchar(50), Convert(FLOAT(50), AMT))
                                        + ',' + CASE WHEN CODENC = '信用卡' THEN '末四碼：'+RIGHT(CREDITCARDNO, 4) + '　' ELSE '' END 
                                        + CASE WHEN ISNULL(STRBILLINGDAT, '') <> '' AND ISNULL(DUEDAT, '') <> '' 
                                          THEN CONVERT(VARCHAR(10), ISNULL(STRBILLINGDAT, ''), 111) + '~' + CONVERT(VARCHAR(10), ISNULL(DUEDAT, ''), 111) ELSE '' END AS S1,
                            'I,' + PRTNO + ',' + amtnc + ',' + CAST(QTY AS VARCHAR(10)) + ',個,' + CAST(amt AS VARCHAR(10)) + ',' + CAST(amt AS VARCHAR(10)) AS S2
                            FROM V_RT3071 WHERE AMTNC <> '保證金' and " + sdata[0] + " order  by rcvmoneydat, belongnc, comn, cusnc, amtnc ";
                cmd.CommandText = selectSql;
                ds = cmd.ExecuteDataSet();
                string js = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DateTime dt = DateTime.Now;
                    var sfile = P2 + "_" + string.Format("{0:yyyyMMdd}", dt)+".txt";

                    FileStream fileStream = new FileStream(@"c:\" + sfile, FileMode.Create);

                    fileStream.Close();   //切記開了要關,不然會被佔用而無法修改喔!!!

                    using (StreamWriter sw = new StreamWriter(@"..\JQWebClient\download\" + sfile))
                    {
                        js = "H,INVO,"+P1+","+P2+"," + string.Format("{0:yyyyMMdd}", DateTime.Now.Date) + "\r\n";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            js = js + ds.Tables[0].Rows[i]["S1"].ToString() + "\r\n";
                            if (i+1== ds.Tables[0].Rows.Count)
                                js = js + ds.Tables[0].Rows[i]["S2"].ToString();
                            else
                                js = js + ds.Tables[0].Rows[i]["S2"].ToString() + "\r\n";
                        }
                        // 欲寫入的文字資料 ~
                        sw.Write(js);
                    }
                    return new object[] { 0, sfile };
                }
                else
                {
                    return new object[] { 0, "N" };
                }
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行電子發票轉出,錯誤訊息：" + ex };
            }
        }
    }
}
