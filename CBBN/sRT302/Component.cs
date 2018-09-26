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
                return new object[] { 0, "轉續約單成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行轉續約單,錯誤訊息" + ex };
            }
        }

        public object[] smRT3023(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT3023.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT3023.InfoParameters[0].Value = sdata[0];
                cmdRT3023.InfoParameters[1].Value = sdata[1];
                cmdRT3023.InfoParameters[2].Value = sdata[2];
                cmdRT3023.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                cmdRT3023.ExecuteDataSet();
                return new object[] { 0, "轉續約單成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行轉續約單,錯誤訊息" + ex };
            }
        }

        public object[] smRT3024(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT3024.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT3024.InfoParameters[0].Value = sdata[0];
                cmdRT3024.InfoParameters[1].Value = sdata[1];
                cmdRT3024.InfoParameters[2].Value = sdata[2];
                cmdRT3024.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                cmdRT3024.ExecuteDataSet();
                return new object[] { 0, "續約資料處理成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "續約資料處理失敗,錯誤訊息" + ex };
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

        public object[] smRT3025(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            string ssql, sbar1, sbar2, sbar3;
            var sfile = sdata[0];
            int counter = 0;
            string line, sSRC, sBATCHNO;
            string sresult = "";
            DataSet ds, ds1, ds2, ds3;
            string sDT = string.Format("{0:yyMMdd}", DateTime.Now.Date);
            //開啟資料連接
            IDbConnection conn = cmd.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {

                // Read the file and display it line by line.
                System.IO.StreamReader file =
                   new System.IO.StreamReader("..\\JQWebClient\\barcode\\"+sfile);
                while ((line = file.ReadLine()) != null)
                {
                    //判斷是明細再做處理
                    if (line.Substring(0,1)=="2") { 
                        sbar1 = line.Substring(60, 9);
                        sbar2 = line.Substring(69, 20);
                        sbar3 = line.Substring(89, 15);
                        sSRC = line.Substring(9, 8); //收款店別

                        //先判斷應收帳款資料是否已經存在，如果已經存在就不要重複新增
                        ssql = @" select * from RTLessorAVSCustAR 
                                  WHERE COD3='"+sbar1+ "' AND  COD4='" + sbar2 + "' AND  COD5='" + sbar3 + "'   ";
                        cmd.CommandText = ssql;
                        ds = cmd.ExecuteDataSet();
                    
                        //如果已經存在就不要新增
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            sresult = sresult + "重複轉入:" + ds.Tables[0].Rows[0]["BATCHNO"].ToString()+" ";
                        }
                        else
                        {
                            //
                            ssql = @"select ISNULL(CAST(RIGHT(MAX(BATCHNO), 4) AS int)+1, 1) AS NO 
                                    from RTLessorAVSCustAR
                                    WHERE BATCHNO LIKE 'BM"+ sDT + "%'";
                            cmd.CommandText = ssql;
                            ds1 = cmd.ExecuteDataSet();
                            sBATCHNO = "BM" + sDT + ds1.Tables[0].Rows[0]["NO"].ToString().PadLeft(4, '0');

                            //直接用語法新增到資料庫裡面去
                            ssql = " INSERT INTO RTLessorAVSCustAR "
                                 + "  (CUSID, BATCHNO, PERIOD, ARTYPE, AMT, COD1, COD2, COD3, COD4, COD5, CDAT) "
                                 + " select b.CUSID,'" + sBATCHNO + "' AS BATCHNO,  C.PERIOD, 'AR' AS ARTYPE, C.AMT, '" + sSRC + "' AS COD1, '超商單號：'+A.CSBARCOD2 AS COD2"
                                 + " , A.CSBARCOD1, A.CSBARCOD2, A.CSBARCOD3, GETDATE()"
                                 + "   from RTLessorAVSCustBillingBarcode A"
                                 + " LEFT JOIN RTLessorAVSCustBillingPrtSub B ON B.NOTICEID=A.NOTICEID"
                                 + " LEFT JOIN RTLessorAVSCust E ON E.CUSID=B.CUSID"
                                 + " LEFT JOIN rtcode d on d.kind = 'L5' and d.PARM1=E.comtype"
                                 + " LEFT JOIN RTBillCharge c on c.CASETYPE=d.CODE AND c.CASEKIND=A.CASEKIND AND C.PAYCYCLE=A.PAYCYCLE"
                                 + " where CSBARCOD1='"+sbar1+ "' and CSBARCOD2='" + sbar2 + "' and CSBARCOD3 ='" + sbar3 + "'";
                            cmd.CommandText = ssql;
                            ds2 = cmd.ExecuteDataSet();

                            //異動用戶的 週期 金額 期間 最近繳費日 到期日期
                            ssql = " UPDATE E SET E.PAYCYCLE=A.PAYCYCLE, E.RCVMONEY= C.AMT, E.PERIOD= C.PERIOD, E.NEWBILLINGDAT=dateadd(D,1, B.DUEDAT), E.DUEDAT=dateadd(m,C.PERIOD, B.DUEDAT) "
                                 + " , E.PAYTYPE='05' "
                                 + "  from RTLessorAVSCustBillingBarcode A "
                                 + "  LEFT JOIN RTLessorAVSCustBillingPrtSub B ON B.NOTICEID = A.NOTICEID "
                                 + "  LEFT JOIN RTLessorAVSCust E ON E.CUSID = B.CUSID "
                                 + "  LEFT JOIN rtcode d on d.kind = 'L5' and d.PARM1 = E.comtype "
                                 + "  LEFT JOIN RTBillCharge c on c.CASETYPE = d.CODE AND c.CASEKIND = A.CASEKIND AND C.PAYCYCLE = A.PAYCYCLE "
                                 + "  where CSBARCOD1='" + sbar1 + "' and CSBARCOD2='" + sbar2 + "' and CSBARCOD3 ='" + sbar3 + "'";
                            cmd.CommandText = ssql;
                            ds2 = cmd.ExecuteDataSet();

                            //查詢出資料後 要跑回圈將每一期資料寫入明細中
                            ssql = " select b.CUSID,'" + sBATCHNO + "' AS BATCHNO,  C.PERIOD, 'AR' AS ARTYPE, C.AMT, '" + sSRC + "' AS COD1, '超商單號：'+A.CSBARCOD2 AS COD2"
                                 + " , A.CSBARCOD1, A.CSBARCOD2, A.CSBARCOD3, GETDATE() "
                                 + " , DATEPART (YYYY, E.NEWBILLINGDAT) AS SYY, DATEPART(MM, E.NEWBILLINGDAT) AS SMM, DATEPART(DD, E.NEWBILLINGDAT) AS SDD  "
                                 + " , DATEPART(YYYY, E.DUEDAT) AS TYY, DATEPART(MM, E.DUEDAT) AS TMM , C.AMT / C.PERIOD as AM_AVG "
                                 + "   from RTLessorAVSCustBillingBarcode A"
                                 + " LEFT JOIN RTLessorAVSCustBillingPrtSub B ON B.NOTICEID=A.NOTICEID"
                                 + " LEFT JOIN RTLessorAVSCust E ON E.CUSID=B.CUSID"
                                 + " LEFT JOIN rtcode d on d.kind = 'L5' and d.PARM1=E.comtype"
                                 + " LEFT JOIN RTBillCharge c on c.CASETYPE=d.CODE AND c.CASEKIND=A.CASEKIND AND C.PAYCYCLE=A.PAYCYCLE"
                                 + " where CSBARCOD1='" + sbar1 + "' and CSBARCOD2='" + sbar2 + "' and CSBARCOD3 ='" + sbar3 + "'"; 
                            cmd.CommandText = ssql;
                            ds3 = cmd.ExecuteDataSet();

                            for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                            {
                                var ip = Convert.ToInt32(ds3.Tables[0].Rows[i]["PERIOD"].ToString());
                                var iy = Convert.ToInt32(ds3.Tables[0].Rows[i]["SYY"].ToString());
                                var im = Convert.ToInt32(ds3.Tables[0].Rows[i]["SMM"].ToString());
                                var id = Convert.ToInt32(ds3.Tables[0].Rows[i]["SDD"].ToString());
                                var iam = Convert.ToInt32(ds3.Tables[0].Rows[i]["AM_AVG"].ToString());
                                var itolday = DateTime.DaysInMonth(iy, im);
                                double iamv = 0;
                                double ipre = 0;
                                double rat = 0;                            

                                //判斷期數
                                for (int j = 0; j < ip; j++)
                                {
                                    im = im + 1;
                                    if (im == 13)
                                    {
                                        iy = iy + 1;
                                        im = 1;
                                    }

                                    if (id != 1 && j == 0)
                                    {
                                        rat = (itolday - id) *100 / itolday;
                                        iamv = Math.Round(iam * rat / 100, 0);
                                        ipre = iam - iamv;
                                    }
                                    else
                                    {
                                        iamv = iam;
                                    }

                                    //新增帳款明細
                                    ssql = " INSERT INTO RTLessorAVSCustARDTL "
                                         + " (CUSID, BATCHNO, SEQ, SYY, SMM, TYY, TMM, ITEMNC, PORM, AMT, REALAMT, CDAT, L14, L23)  "
                                         + " select b.CUSID,'" + sBATCHNO + "' AS BATCHNO, "+j+" AS SEQ "
                                         + " , DATEPART (YYYY, E.NEWBILLINGDAT) AS SYY, DATEPART(MM, E.NEWBILLINGDAT) AS SMM "
                                         + " , "+iy+" AS TYY, "+im+" AS TMM "
                                         + " , '"+iy+"'+'-'+'"+im+"' + '　網路服務費(續約)' AS ITEMNC, '+' AS FORM, "+iamv+", 0 AS REALAMT "
                                         + " , GETDATE() AS CDAT, '4642' AS L14, '016' AS L23 "
                                         + " from RTLessorAVSCustBillingBarcode A "
                                         + " LEFT JOIN RTLessorAVSCustBillingPrtSub B ON B.NOTICEID = A.NOTICEID "
                                         + " LEFT JOIN RTLessorAVSCust E ON E.CUSID = B.CUSID "
                                         + " LEFT JOIN rtcode d on d.kind = 'L5' and d.PARM1 = E.comtype "
                                         + " LEFT JOIN RTBillCharge c on c.CASETYPE = d.CODE AND c.CASEKIND = A.CASEKIND AND C.PAYCYCLE = A.PAYCYCLE "
                                         + " where CSBARCOD1='" + sbar1 + "' and CSBARCOD2='" + sbar2 + "' and CSBARCOD3 ='" + sbar3 + "'";
                                    cmd.CommandText = ssql;
                                    ds2 = cmd.ExecuteDataSet();
                                }

                                if (ipre !=0)
                                {
                                    im = im + 1;
                                    if (im == 13)
                                    {
                                        iy = iy + 1;
                                        im = 1;
                                    }
                                    //新增帳款明細
                                    ssql = " INSERT INTO RTLessorAVSCustARDTL "
                                         + " (CUSID, BATCHNO, SEQ, SYY, SMM, TYY, TMM, ITEMNC, PORM, AMT, REALAMT, CDAT, L14, L23)  "
                                         + " select b.CUSID,'" + sBATCHNO + "' AS BATCHNO, " + ip+1 + " AS SEQ "
                                         + " , DATEPART (YYYY, E.NEWBILLINGDAT) AS SYY, DATEPART(MM, E.NEWBILLINGDAT) AS SMM "
                                         + " , " + iy + " AS TYY, " + im + " AS TMM "
                                         + " , '" + iy + "'+'-'+'" + im + "' + '　網路服務費(續約)' AS ITEMNC, '+' AS FORM, " + ipre + ", 0 AS REALAMT "
                                         + " , GETDATE() AS CDAT, '4642' AS L14, '016' AS L23 "
                                         + " from RTLessorAVSCustBillingBarcode A "
                                         + " LEFT JOIN RTLessorAVSCustBillingPrtSub B ON B.NOTICEID = A.NOTICEID "
                                         + " LEFT JOIN RTLessorAVSCust E ON E.CUSID = B.CUSID "
                                         + " LEFT JOIN rtcode d on d.kind = 'L5' and d.PARM1 = E.comtype "
                                         + " LEFT JOIN RTBillCharge c on c.CASETYPE = d.CODE AND c.CASEKIND = A.CASEKIND AND C.PAYCYCLE = A.PAYCYCLE "
                                         + " where CSBARCOD1='" + sbar1 + "' and CSBARCOD2='" + sbar2 + "' and CSBARCOD3 ='" + sbar3 + "'";
                                    cmd.CommandText = ssql;
                                    ds2 = cmd.ExecuteDataSet();
                                }
                            }

                            

                            //新增資料到續約資料檔中 RTLessorAVSCustCont
                            ssql = " INSERT INTO RTLessorAVSCustCont "
                                 + " (CUSID, ENTRYNO, APPLYDAT, PAYCYCLE, PERIOD, SECONDCASE, AMT, PAYTYPE, REALAMT, TARDAT, BATCHNO, TUSR, FINISHDAT, EDAT, EUSR, UDAT, ADJUSTDAY "
                                 + " , STRBILLINGDAT, MAXENTRYNO, CASEKIND, RCVMONEYDAT, CSNOTICEID) "
                                 + " SELECT A.CUSID, ISNULL(D.MM, 0) + 1 AS ENTRYNO, GETDATE() AS APPLYDAT, B.PAYCYCLE, A.PERIOD, C.SECONDCASE "
                                 + "   , A.AMT, '05' AS PAYTYPE, 0 AS REALAMT, GETDATE() AS TARDAT, A.BATCHNO, 'USER' AS TUSR "
                                 + " , GETDATE() AS FINISHDAT, GETDATE() AS EDAT, 'USER' AS EUSR, GETDATE() AS UDAT, 0 AS ADJUSTDAY "
                                 + " , dateadd(d, 1, E.DUEDAT) AS STRBILLINGDAT, 0 AS MAXENTRYNO, C.CASEKIND, A.CDAT AS RCVMONEYDAT, B.CSNOTICEID "
                                 + " FROM RTLessorAVSCustAR A "
                                 + " LEFT JOIN RTLessorAVSCustBillingBarcode B ON B.CSBARCOD1 = A.COD3 AND B.CSBARCOD2 = A.COD4 AND  B.CSBARCOD3 = A.COD5 "
                                 + " LEFT JOIN RTLessorAVSCust C ON C.CUSID = A.CUSID "
                                 + " LEFT JOIN(SELECT CUSID, MAX(ENTRYNO) AS MM FROM RTLessorAVSCustCont GROUP BY CUSID) D ON D.CUSID = A.CUSID "
                                 + " LEFT JOIN RTLessorAVSCustBillingPrtSub E ON E.NOTICEID = B.NOTICEID "
                                 + " where B.CSBARCOD1='" + sbar1 + "' and B.CSBARCOD2='" + sbar2 + "' and B.CSBARCOD3 ='" + sbar3 + "' "
                                 + " AND A.BATCHNO NOT IN (SELECT BATCHNO FROM RTLessorAVSCustCont) ";
                            cmd.CommandText = ssql;
                            ds2 = cmd.ExecuteDataSet();
                        }

                        counter++;
                    }
                }
                file.Close();

                // Suspend the screen.
                Console.ReadLine();

                return new object[] { 0, "處理完成"+ sresult + "，處理筆數："+counter};
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行產生應收付帳款轉入資料作業,錯誤訊息"+ ex};
            }
        }
    }
}
