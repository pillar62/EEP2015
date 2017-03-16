using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Srvtools;
using System.Web;

namespace sRT302
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

      
        public object[] smRT3021(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT3021.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT3021.InfoParameters[0].Value = sdata[0];
                cmdRT3021.InfoParameters[1].Value = sdata[1];
                cmdRT3021.InfoParameters[2].Value = sdata[2];
                cmdRT3021.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                cmdRT3021.ExecuteDataSet();
                return new object[] { 0, true };
            }
            catch
            {
                return new object[] { 0, false };
            }

        }

        public object[] smRT3022(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT3022.Connection;
            conn.Open();
            object[] objRet = new object[] { 0, "Y" };
            //設定輸入參數的值
            var sbatch = sdata[0];
            try
            {
                string sql = " select a.batch, convert(varchar(7), convert(int, convert(varchar(8), dateadd(m, 2, a.duedat), 112)) - 19110000) +';'+ d.csnoticeid + ';' + "
                            + " convert(varchar(3), datepart(yy, a.duedat) - 1911) + substring(convert(varchar(6), a.duedat, 12), 3, 2) + ';' + "
                            + " convert(varchar(9), case b.secondcase when 'Y' then c.amt2 else c.amt end) + ';' + "
                            + " d.cscusid + ';' +' ' + ';' +c.memo + ';' +b.cusnc + ';' as bcodesrc from RTLessorAvsCustBillingPrtSub a "
                            + " inner join RTLessorAvsCust b on a.cusid = b.cusid inner join RTLessorAVSCustBillingBarcode d on d.noticeid = a.noticeid "
                            + " inner join RTBillCharge c on c.casekind = d.casekind and c.paycycle = d.paycycle and c.casetype = '07' "
                            + " where a.batch ='"+ sbatch + "' "
                            + " order by b.comq1, b.lineq1, b.cusnc, c.paycycle";
                string js = string.Empty;
                DataSet ds = this.ExecuteSql("cmdRT3022", sql, this.GetClientInfo(ClientInfoType.LoginDB).ToString(), true);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DateTime dt = DateTime.Now;
                    var sfile = "334" + string.Format("{0:yyyyMMdd}", dt);
                    FileStream fileStream = new FileStream(@"c:\"+ sfile, FileMode.Create);

                    fileStream.Close();   //切記開了要關,不然會被佔用而無法修改喔!!!

                    using (StreamWriter sw = new StreamWriter(@"..\JQWebClient\download\"+ sfile))
                    {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            js = js + ds.Tables[0].Rows[i]["bcodesrc"].ToString() + "\r\n";
                            // 欲寫入的文字資料 ~
                            sw.Write(ds.Tables[0].Rows[i]["bcodesrc"].ToString() + "\r\n");
                        }
                    }
                    return new object[] { 0, js };
                }
                else
                {
                    return new object[] { 0, "N" };
                }
                    
            }
            catch (Exception ex)
            {                    
                return new object[] { 0, "Excel匯入BOM展開，請查看Log!"+ ex.Message };
            }
        }
    }
}
