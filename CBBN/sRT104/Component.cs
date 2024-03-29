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
            string selectSql = "select * FROM RTLessorAVSCUST WHERE comq1=" + sdata[0] + " and lineq1 = " + sdata[1] + " and CUSID ='" + sdata[2] + "'";
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

        public object[] smRT104FAR(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            string COMTYPE;
            //開啟資料連接
            IDbConnection conn = cmd.Connection;
            conn.Open();
            string ZIP, sKEY;
            string selectSql = " SELECT RTCtyTown.ZIP AS zip, RTLessorAVSCmtyH.COMTYPE  "
                             + " FROM RTLessorAVSCmtyH "
                             + " INNER JOIN RTCtyTown ON RTLessorAVSCmtyH.CUTID = RTCtyTown.CUTID AND RTLessorAVSCmtyH.TOWNSHIP = RTCtyTown.TOWNSHIP "
                             + " where RTLessorAVSCmtyH.comq1 = " + sdata[0];
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ZIP = ds.Tables[0].Rows[0]["zip"].ToString();
                COMTYPE = ds.Tables[0].Rows[0]["COMTYPE"].ToString();
            }
            else
            {
                ZIP = "";
                COMTYPE = "";
            }

            string CMTYSEQ = sdata[0];
            CMTYSEQ = CMTYSEQ.PadLeft(4, '0'); //社區編號左補零            

            selectSql = "select max(MEMBERID) AS MEMBERID from RTLessorAVSCust where MEMBERID like 'FBB_" + ZIP + CMTYSEQ + "%' ";
            cmd.CommandText = selectSql;
            ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                var MEMBERID = ds.Tables[0].Rows[0]["MEMBERID"].ToString().Replace("FBB_" + ZIP + CMTYSEQ, "");
                if (MEMBERID == "")
                    sKEY = "FBB_" + ZIP + CMTYSEQ + "001";
                else
                {
                    MEMBERID = (int.Parse(MEMBERID) + 1).ToString();
                    sKEY = "FBB_" + ZIP + CMTYSEQ + MEMBERID.PadLeft(3, '0');

                }
            }
            else
            {
                sKEY = "FBB_" + ZIP + CMTYSEQ + "001";
            }
            //設定輸入參數的值
            try
            {
                if (COMTYPE == "B")
                    return new object[] { 0, sKEY };
                else
                    return new object[] { 0, "" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶申請作廢作業,錯誤訊息" + ex };
            }
        }

        public object[] smRT104GetLINEQ1(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            string LINEQ1 = "";
            //開啟資料連接
            IDbConnection conn = cmd.Connection;
            conn.Open();
            string selectSql = " SELECT LINEQ1 FROM RTLessorAVSCmtyLine"
                             + " WHERE COMQ1 = " + sdata[0];
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                LINEQ1 = ds.Tables[0].Rows[0]["LINEQ1"].ToString();
            }
            else
            {
                LINEQ1 = "";
            }

            //設定輸入參數的值
            try
            {
                return new object[] { 0, LINEQ1 };
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
            string sDOCKETDAT = ucRTLessorAVSCust.GetFieldCurrentValue("DOCKETDAT").ToString(); ; //報竣日
            string sFINISHDAT = ucRTLessorAVSCust.GetFieldCurrentValue("FINISHDAT").ToString();//完工日
            string sSTRBILLINGDAT = ucRTLessorAVSCust.GetFieldCurrentValue("STRBILLINGDAT").ToString(); //開始計費日
            string sGTMONEY = ucRTLessorAVSCust.GetFieldCurrentValue("GTMONEY").ToString(); //保證金 金額
            double dGTMONEY = 0;
            //修改人員、修改者
            string sNow = DateTime.Now.Date.ToString("yyyyMMdd");
            string UUSR = this.GetClientInfo(ClientInfoType.LoginUser).ToString();
            ucRTLessorAVSCust.SetFieldValue("UDAT", DateTime.Now);
            ucRTLessorAVSCust.SetFieldValue("UUSR", UUSR);

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
                        ss = "AVS" + ss.Substring(ss.Length - 9, 9);
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
                        ss = "AVS" + ss.Substring(ss.Length - 9, 9);
                    }
                }
                ucRTLessorAVSCust.SetFieldValue("GTSERIAL", ss);
            }
        }
    }
}
