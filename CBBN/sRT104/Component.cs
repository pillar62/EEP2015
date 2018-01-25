using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT104
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

        public string getFix()
        {
            return string.Format("E{0:yyMMdd}", DateTime.Now.Date);
        }

        //用戶作廢
        public object[] smRT104B(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT104B.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUST WHERE comq1=" + sdata[0] + " and lineq1 = "+ sdata[1] + " and CUSID ='" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {                
                if (ds.Tables[0].Rows[0]["FINISHDAT"].ToString() != "")
                {
                    return new object[] { 0, "此用戶已完工，不可作廢。" };
                }
                if (ds.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "此用戶已作廢，不可重覆執行。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT104B.InfoParameters[0].Value = sdata[0];
                cmdRT104B.InfoParameters[1].Value = sdata[1];
                cmdRT104B.InfoParameters[2].Value = sdata[2];
                cmdRT104B.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT104B.ExecuteNonQuery();
                return new object[] { 0, "用戶申請作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶申請作廢作業,錯誤訊息" + ex };
            }
        }

        //用戶作廢返轉 
        public object[] smRT104C(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT104C.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUST WHERE comq1=" + sdata[0] + " and lineq1 = " + sdata[1] + " and CUSID ='" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 == "")
                {
                    return new object[] { 0, "此用戶尚未作廢，不可作廢返轉。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT104C.InfoParameters[0].Value = sdata[0];
                cmdRT104C.InfoParameters[1].Value = sdata[1];
                cmdRT104C.InfoParameters[2].Value = sdata[2];
                cmdRT104C.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT104C.ExecuteNonQuery();
                return new object[] { 0, "用戶申請作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶申請作廢作業,錯誤訊息" + ex };
            }
        }

        private void ucRTLessorAVSCust_BeforeModify(object sender, UpdateComponentBeforeModifyEventArgs e)
        {
            //取得保證金序號 以及 報竣日
            string sgtserial = ucRTLessorAVSCust.GetFieldCurrentValue("GTSERIAL").ToString(); //保證金序號
            string sDOCKETDAT  = ucRTLessorAVSCust.GetFieldCurrentValue("DOCKETDAT").ToString();; //報竣日
            string sFINISHDAT = ucRTLessorAVSCust.GetFieldCurrentValue("FINISHDAT").ToString();//完工日
            string sSTRBILLINGDAT = ucRTLessorAVSCust.GetFieldCurrentValue("STRBILLINGDAT").ToString(); //開始計費日
            string sGTMONEY = ucRTLessorAVSCust.GetFieldCurrentValue("GTMONEY").ToString(); //保證金 金額
            double dGTMONEY = 0;

            string ss = "";
            if (sFINISHDAT!="" && (sDOCKETDAT == "" || sSTRBILLINGDAT == ""))
            {
                DateTime dDT = Convert.ToDateTime(ucRTLessorAVSCust.GetFieldCurrentValue("FINISHDAT"));
                ss = Convert.ToString(dDT.AddDays(7));
                
                //判斷 如果完工日不為空白 且竣工日或是開始計費日為空白 就要押完工日+七天 作為報竣日 及 開始計費日
                if (sDOCKETDAT == "")
                {                    
                    ucRTLessorAVSCust.SetFieldValue("DOCKETDAT", ss);
                    sDOCKETDAT = ss;
                }

                if (sSTRBILLINGDAT == "")
                {
                    ucRTLessorAVSCust.SetFieldValue("STRBILLINGDAT", ss);
                }
            }

            //判斷 如果保證金金額欄位="" 就給0 不然就轉換成數字
            if (sGTMONEY != "")
            {
                dGTMONEY = Convert.ToInt64(sGTMONEY);
            }
            else
            {
                dGTMONEY = 0;
            }

            //判斷 保證金序號欄位為空 且保證金有金額 再處理
            if (sgtserial == "" && dGTMONEY > 0)
            {
                string ssql = " select isnull(max(gtserial),'') as maxgtserial from RTlessoravsCust where gtserial <> '' and substring(gtserial, 1,3) = 'AVS' ";

                //開啟資料連接
                IDbConnection conn = cmdRT104C.Connection;
                conn.Open();
                cmd.CommandText = ssql;
                DataSet ds = cmd.ExecuteDataSet();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string ss1 = ds.Tables[0].Rows[0]["maxgtserial"].ToString();
                    if (ss1 == "")
                    {
                        ss = "AVS000000001";
                    }
                    else
                    {
                        ss = ss1.Substring(3, 9);
                        ss = (Convert.ToInt64(ss) + 1).ToString();
                        ss = "000000000" + ss;
                        ss = "AVS" + ss.Substring(ss.Length-9, 9);
                    }
                }
                ucRTLessorAVSCust.SetFieldValue("GTSERIAL", ss);
            }
        }

        private void ucRTLessorAVSCust_BeforeInsert(object sender, UpdateComponentBeforeInsertEventArgs e)
        {
            //取得保證金序號 以及 報竣日
            string sgtserial = ucRTLessorAVSCust.GetFieldCurrentValue("GTSERIAL").ToString();
            string sDOCKETDAT = ucRTLessorAVSCust.GetFieldCurrentValue("DOCKETDAT").ToString();
            string sFINISHDAT = ucRTLessorAVSCust.GetFieldCurrentValue("FINISHDAT").ToString();//完工日
            string sSTRBILLINGDAT = ucRTLessorAVSCust.GetFieldCurrentValue("STRBILLINGDAT").ToString(); //開始計費日
            string sGTMONEY = ucRTLessorAVSCust.GetFieldCurrentValue("GTMONEY").ToString(); //保證金 金額
            double dGTMONEY = 0;

            string ss = "";
            if (sFINISHDAT != "" && (sDOCKETDAT == "" || sSTRBILLINGDAT == ""))
            {
                DateTime dDT = Convert.ToDateTime(ucRTLessorAVSCust.GetFieldCurrentValue("FINISHDAT"));
                ss = Convert.ToString(dDT.AddDays(7));

                //判斷 如果完工日不為空白 且竣工日或是開始計費日為空白 就要押完工日+七天 作為報竣日 及 開始計費日
                if (sDOCKETDAT == "")
                {
                    ucRTLessorAVSCust.SetFieldValue("DOCKETDAT", ss);
                    sDOCKETDAT = ss;
                }

                if (sSTRBILLINGDAT == "")
                {
                    ucRTLessorAVSCust.SetFieldValue("STRBILLINGDAT", ss);
                }
            }

            //判斷 如果保證金金額欄位="" 就給0 不然就轉換成數字
            if (sGTMONEY!="")
            {
                dGTMONEY = Convert.ToInt64(sGTMONEY);
            }
            else
            {
                dGTMONEY = 0;
            }

            //判斷 保證金序號欄位為空 且保證金有金額 再處理
            if (sgtserial == "" && dGTMONEY > 0)
            {
                string ssql = " select isnull(max(gtserial),'') as maxgtserial from RTlessoravsCust where gtserial <> '' and substring(gtserial, 1,3) = 'AVS' ";

                //開啟資料連接
                IDbConnection conn = cmdRT104C.Connection;
                conn.Open();
                cmd.CommandText = ssql;
                DataSet ds = cmd.ExecuteDataSet();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string ss1 = ds.Tables[0].Rows[0]["maxgtserial"].ToString();
                    if (ss1 == "")
                    {
                        ss = "AVS000000001";
                    }
                    else
                    {
                        ss = ss1.Substring(3, 9);
                        ss = (Convert.ToInt64(ss) + 1).ToString();
                        ss = "000000000" + ss;
                        ss = "AVS" + ss.Substring(ss.Length - 9, 9);
                    }
                }
                ucRTLessorAVSCust.SetFieldValue("GTSERIAL", ss);
            }
        }    
    }
}
