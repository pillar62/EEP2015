using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT1044
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
            return string.Format("EM{0:yyMMdd}", DateTime.Now.Date);
        }

        public object[] smRT10441(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10441.Connection;
            conn.Open();

            string selectSql, sqlxx, sqlYY, sqlzz;
            string tempperiod = "";
            string temprcvmoney = "";
            string temppaytype = "";
            string tempcardno = "";
            DataSet rsyy, RSXX, RSzz;
            sqlxx = "select * FROM RTLessorAVSCustReturn WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            sqlYY = "select * FROM RTLessorAVSCUST WHERE CUSID='" + sdata[0] + "' ";
            sqlzz = "select count(*) as cnt FROM RTLessorAVSCustReturnHardware WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and dropdat is null and rcvfinishdat is null ";
            selectSql = " select * FROM RTLessorAVSCustReturn WHERE  CUSID='" + sdata[0] + "' AND entryno = " + sdata[1];
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            rsyy = cmd.ExecuteDataSet();
            cmd.CommandText = sqlzz;
            RSzz = cmd.ExecuteDataSet();


            if (rsyy.Tables[0].Rows.Count <= 0)
            {
                return new object[] { 0, "找不到客戶主檔資料，無法執行轉應收帳款結案作業。" };
            }

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "約資料已作廢時，不可執行轉應收結案作業。" };
                }

                if (RSXX.Tables[0].Rows[0]["strbillingdat"].ToString() == "")
                {
                    return new object[] { 0, "開始計費日空白時不可轉應收結案作業。" };
                }

                if (RSXX.Tables[0].Rows[0]["batchno"].ToString() != "" || RSXX.Tables[0].Rows[0]["FINISHDAT"].ToString() != "")
                {
                    return new object[] { 0, "此筆復機資料已轉應收帳款，不可重複產生。" };
                }

                if (rsyy.Tables[0].Rows.Count > 0)
                {
                    if (rsyy.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                    {
                        return new object[] { 0, "客戶已退租或作廢，無法結案(派工單必須作廢)。" };
                    }

                    if (rsyy.Tables[0].Rows[0]["DROPDAT"].ToString() == "")
                    {
                        return new object[] { 0, "客戶未退租，不可執行復機作業(必須採復機作業)。" };
                    }
                }

                 tempperiod = RSXX.Tables[0].Rows[0]["period"].ToString();
                 temprcvmoney = RSXX.Tables[0].Rows[0]["amt"].ToString(); 
                 temppaytype = RSXX.Tables[0].Rows[0]["paytype"].ToString();
                 tempcardno = RSXX.Tables[0].Rows[0]["CREDITCARDNO"].ToString(); 

            }
            else
            {
                return new object[] { 0, "找不到客戶復機主檔資料，無法執行返轉應收帳款結案作業。" };
            }

            if (RSzz.Tables[0].Rows.Count <= 0)
            {
                if (Convert.ToInt32(RSzz.Tables[0].Rows[0]["cnt"].ToString()) > 0)
                    return new object[] { 0, "此收款派工單設備資料中，尚有設備未辦妥物品領用程序(未領用或領用未結案)，不可執行完工結案作業。" };
            }

            sqlxx = "select * FROM RTLessorAVSCustReturnsndwork WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and prtno='" + sdata[2] + "' ";
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            //當已作廢時，不可執行完工結案或未完工結案
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "當已作廢時，不可執行完工結案或未完工結案" };
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" || RSXX.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此收款派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["REALENGINEER"].ToString() == "" && RSXX.Tables[0].Rows[0]["REALCONSIGNEE"].ToString() == "")
                {
                    return new object[] { 0, "此收款派工單完工時，必須先輸入實際裝機人員或實際裝機經銷商。" };
                }

                if (RSXX.Tables[0].Rows[0]["BONUSCLOSEYM"].ToString() != "" || RSXX.Tables[0].Rows[0]["STOCKCLOSEYM"].ToString() != "")
                {
                    return new object[] { 0, "此收款派工單已月結，不可異動。" };
                }

                if (RSXX.Tables[0].Rows[0]["BATCHNO"].ToString() != "")
                {
                    return new object[] { 0, "此收款派工單已產生應收帳款檔，不可重複執行(請洽資訊部)。" };
                }
            }


            //設定輸入參數的值
            try
            {
                cmdRT10441.InfoParameters[0].Value = sdata[0];
                cmdRT10441.InfoParameters[1].Value = sdata[1];
                cmdRT10441.InfoParameters[2].Value = sdata[2];
                cmdRT10441.InfoParameters[3].Value = sdata[3];
                cmdRT10441.InfoParameters[4].Value = tempperiod;
                cmdRT10441.InfoParameters[5].Value = temprcvmoney;
                cmdRT10441.InfoParameters[6].Value = temppaytype;
                cmdRT10441.InfoParameters[7].Value = tempcardno;
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10441.ExecuteNonQuery();
                return new object[] { 0, "用戶收款派工單完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行收款派工單完工結案作業,錯誤訊息"+ ex };
            }
        }

        public object[] smRT10442(object[] objParam)
        {
            //返轉應收結案
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10442.Connection;
            conn.Open();

            string sqlxx, sqlyy;
            DataSet RSyy, RSXX;
            sqlxx = "select * FROM RTLessorAVSCustReturnsndwork WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and prtno='" + sdata[2] + "' ";
            sqlyy = "select count(*) as cnt FROM RTLessorAVSCustReturnhardware WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and dropdat is null and RCVPRTNO <> ''  ";
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlyy;
            RSyy = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "當已作廢時，不可執行完工結案或未完工結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" || RSXX.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此收款派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["BONUSCLOSEYM"].ToString() != "" || RSXX.Tables[0].Rows[0]["STOCKCLOSEYM"].ToString() != "")
                {
                    return new object[] { 0, "此收款派工單已產生物品領用單，請先返轉領用單才能執行未完工結案作業。" };
                }

                if (RSyy.Tables[0].Rows.Count <= 0)
                {
                    if (Convert.ToInt32(RSyy.Tables[0].Rows[0]["cnt"].ToString()) > 0)
                        return new object[] { 0, "此收款派工單已月結，不可異動。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10442.InfoParameters[0].Value = sdata[0];
                cmdRT10442.InfoParameters[1].Value = sdata[1];
                cmdRT10442.InfoParameters[2].Value = sdata[2];
                cmdRT10442.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10442.ExecuteNonQuery();
                return new object[] { 0, "用戶復機收款派工單未完工結案成功。" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行復機收款派工單未完工結案作業,錯誤訊息,錯誤訊息：" + ex };
            }
        }

        public object[] smRT10443(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10443.Connection;
            conn.Open();

            string sqlxx, sqlyy;
            string tempperiod = "";
            string temprcvmoney = "";
            string temppaytype = "";
            string tempcardno = "";
            string BATCHNOXX = "";
            int xxmaxentryno = 0;
            DataSet RSyy, RSXX;
            sqlxx = "select * FROM RTLessorAVSCUST WHERE CUSID='" + sdata[0] + "' ";
            sqlyy = "select * FROM RTLessorAVSCustReturn WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlyy;
            RSyy = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["cancelDAT"].ToString() != "")
                {
                    return new object[] { 0, "客戶已作廢,不可返轉。" };
                }
            }
            else
            {
                return new object[] { 0, "找不到客戶基本檔，無法返轉。" };

            }

            if (RSyy.Tables[0].Rows.Count > 0)
            {
                if (RSyy.Tables[0].Rows[0]["cancelDAT"].ToString() != "")
                {
                    return new object[] { 0, "客戶復機資料已作廢,不可返轉。" };
                }

                tempperiod = RSyy.Tables[0].Rows[0]["period"].ToString();
                temprcvmoney = RSyy.Tables[0].Rows[0]["amt"].ToString();
                temppaytype = RSyy.Tables[0].Rows[0]["paytype"].ToString();
                tempcardno = RSyy.Tables[0].Rows[0]["CREDITCARDNO"].ToString();

                //記錄客戶主檔異動檔的異動項次最大值(若派工單結案時已記錄的異動項次小於目前的最大值時，表示已經有其它異動發生，則不允許返轉。
                sqlyy = "select max(entryno)as entryno from RTLessorAVSCUSTlog where CUSID='" + sdata[0] + "' ";
                cmd.CommandText = sqlyy;
                RSyy = cmd.ExecuteDataSet();
                if (RSyy.Tables[0].Rows.Count > 0)
                {
                    xxmaxentryno = (Convert.ToInt32(RSyy.Tables[0].Rows[0]["entryno"].ToString()));
                }
                else
                {
                    xxmaxentryno = 0;
                }
            }
            else
            {
                return new object[] { 0, "找不到客戶復機資料，無法返轉。" };
            }

            sqlxx = "select * FROM RTLessorAVSCustReturnsndwork WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and prtno='" + sdata[2] + "' ";
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                //檢查應收帳款檔是否已沖帳
                BATCHNOXX = RSXX.Tables[0].Rows[0]["BATCHNO"].ToString();
                sqlyy = "select * FROM RTLessorAVSCUSTAR WHERE CUSID='" + sdata[0] + "' and BATCHNO='" + BATCHNOXX + "'";
                cmd.CommandText = sqlyy;
                RSyy = cmd.ExecuteDataSet();
                if (RSyy.Tables[0].Rows.Count > 0)
                {
                    if (RSyy.Tables[0].Rows[0]["mdat"].ToString() != "" || (Convert.ToInt32(RSyy.Tables[0].Rows[0]["REALAMT"].ToString())) > 0)
                    {
                        return new object[] { 0, "應收帳款已沖帳，不可執行結案返轉作業(請與資訊部連繫)。" };
                    }                    
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() == "" && RSXX.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() == "")
                {
                    return new object[] { 0, "此派工單尚未結案，不可執行結案返轉作業。" };
                }

                if (RSXX.Tables[0].Rows[0]["dropdat"].ToString() != "")
                {
                    return new object[] { 0, "此派工單已作廢，不可返轉。" };
                }

                if (xxmaxentryno > (Convert.ToInt32(RSXX.Tables[0].Rows[0]["maxentryno"].ToString())))
                {
                    return new object[] { 0, "客戶主檔已進行其它異動，因此無法執行派工單返轉作業。" };
                }
            }
            

            //設定輸入參數的值
            try
            {
                cmdRT10443.InfoParameters[0].Value = sdata[0];
                cmdRT10443.InfoParameters[1].Value = sdata[1];
                cmdRT10443.InfoParameters[2].Value = sdata[2];
                cmdRT10443.InfoParameters[3].Value = sdata[3];
                cmdRT10443.InfoParameters[4].Value = BATCHNOXX;
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10443.ExecuteNonQuery();
                return new object[] { 0, "用戶收款派工單結案返轉成功。" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行收款派工單完工結案作業,錯誤訊息"+ex };
            }
        }

        public object[] smRT10444(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10444.Connection;
            conn.Open();

            string sqlxx, sqlyy;
            DataSet RSyy, RSXX;
            sqlxx = "select * FROM RTLessorAVSCUSTReturnsndwork WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and prtno='" + sdata[2] + "' ";
            sqlyy = "select * FROM RTLessorAVSCustRCVHardware WHERE prtno='" + sdata[2] + "' and canceldat is null ";
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlyy;
            RSyy = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" || RSXX.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此復機收款派工單已完工(未完工)結案，不可作廢(欲作廢請先清除裝機完工日)。" };
                }

                if (RSXX.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "此復機收款派工單已作廢，不可重覆執行作廢作業。" };
                }
            }

            if (RSyy.Tables[0].Rows.Count > 0)
            {
               return new object[] { 0, "此派工單已產生物品領用單，不可直接作廢(請先返轉物品領用單)。" };
            }

            //設定輸入參數的值
            try
            {
                cmdRT10444.InfoParameters[0].Value = sdata[0];
                cmdRT10444.InfoParameters[1].Value = sdata[1];
                cmdRT10444.InfoParameters[2].Value = sdata[2];
                cmdRT10444.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10444.ExecuteNonQuery();
                return new object[] { 0, "用戶復機收款派工單作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行復機收款派工單作廢作業,錯誤訊息：" + ex };
            }
        }

        public object[] smRT10445(object[] objParam)
        {
            //作廢反轉
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10445.Connection;
            conn.Open();
            string sqlxx;
            DataSet RSXX;
            sqlxx = "select * FROM RTLessorAVSCUSTReturnsndwork WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and prtno='" + sdata[2] + "' ";
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["dropdat"].ToString() == "" || RSXX.Tables[0].Rows[0]["dropdat"].ToString() == "")
                {
                    return new object[] { 0, "此用戶復機收款派工單尚未作廢，不可執行作廢返轉作業。" };
                }

                if (RSXX.Tables[0].Rows[0]["bonuscloseym"].ToString() != "")
                {
                    return new object[] { 0, "當獎金計算年月已存在資料時表示該筆資料完工日期當月之獎金已結算,不可再作廢返轉。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10445.InfoParameters[0].Value = sdata[0];
                cmdRT10445.InfoParameters[1].Value = sdata[1];
                cmdRT10445.InfoParameters[2].Value = sdata[2];
                cmdRT10445.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10445.ExecuteNonQuery();
                return new object[] { 0, "用戶復機收款派工單作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶復機收款派工單作廢返轉作業,錯誤訊息：" + ex };
            }
        }

        public object[] smRT10446(object[] objParam)
        {
            //轉應收結案
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10446.Connection;
            conn.Open();
            string sqlxx, sqlYY, sqlzz;
            DataSet rsyy, RSXX, RSzz;

            sqlxx = "select * FROM RTLessorAVSCustReturn WHERE CUSID='" + sdata[0] + "' and ENTRYNO=" + sdata[1];
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            sqlYY = "select * FROM RTLessorAVSCust WHERE CUSID='" + sdata[0] + "'";
            cmd.CommandText = sqlYY;
            rsyy = cmd.ExecuteDataSet();
            sqlzz = "select count(*) as cnt FROM RTLessorAVSCustReturnsndwork  WHERE CUSID='" + sdata[0] + "' and ENTRYNO=" + sdata[1] + " and dropdat is null and unclosedat is null and closedat is null ";
            cmd.CommandText = sqlzz;
            RSzz = cmd.ExecuteDataSet();

            if (rsyy.Tables[0].Rows.Count <= 0)
            {
                return new object[] { 0, "找不到客戶主檔資料，無法執行轉應收帳款結案作業。" };
            }

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["canceldat"].ToString() != "")
                {
                    return new object[] { 0, "當復機資料已作廢時，不可執行轉應收帳款作業。" };
                }

                if (RSXX.Tables[0].Rows[0]["strbillingdat"].ToString() == "")
                {
                    return new object[] { 0, "開始計費日空白時不可轉應收結案作業。" };
                }

                /*
                if (RSXX.Tables[0].Rows[0]["paytype"].ToString() == "02")
                {
                    return new object[] { 0, "繳費方式為現金付款時，必須由收款派工單產生應收帳款。" };
                }
                */

                if (RSXX.Tables[0].Rows[0]["batchno"].ToString() != "" || RSXX.Tables[0].Rows[0]["FINISHDAT"].ToString() != "")
                {
                    return new object[] { 0, "此筆復機資料已轉應收帳款，不可重複產生。" };
                }

                if (rsyy.Tables[0].Rows.Count > 0)
                {
                    if (rsyy.Tables[0].Rows[0]["canceldat"].ToString() != "")
                    {
                        return new object[] { 0, "客戶資料已作廢，必須作廢復機資料。" };
                    }

                    if (rsyy.Tables[0].Rows[0]["DROPdat"].ToString() == "")
                    {
                        return new object[] { 0, "客戶資料尚未退租，不可執行復機轉應收帳款作業。" };
                    }
                }
            }
            else
            {
                return new object[] { 0, "找不到客戶復機主檔資料，無法執行轉應收帳款結案作業。" };
            }

            if (RSzz.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(RSzz.Tables[0].Rows[0]["cnt"].ToString()) > 0)
                {
                    return new object[] { 0, "此復機資料已存在收款派工單，必須由派工單進行結案作業。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10446.InfoParameters[0].Value = sdata[0];
                cmdRT10446.InfoParameters[1].Value = sdata[1];
                cmdRT10446.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10446.ExecuteNonQuery();
                return new object[] { 0, "用戶復機轉應收帳款成功。" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶復機轉應收帳款作業,錯誤訊息"+ex.Message};
            }
        }

        public object[] smRT10447(object[] objParam)
        {
            //轉應收結案反轉
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            var BATCHNOXX = "";

            //開啟資料連接
            IDbConnection conn = cmdRT10447.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCustReturn WHERE CUSID='" + sdata[0] + "' and ENTRYNO=" + sdata[1];
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            selectSql = "select * FROM RTLessorAVSCust WHERE CUSID='" + sdata[0] + "'";
            cmd.CommandText = selectSql;
            DataSet ds1 = cmd.ExecuteDataSet();

            if (ds1.Tables[0].Rows.Count <= 0)
            {
                return new object[] { 0, "找不到客戶主檔資料，無法執行轉應收帳款結案作業。" };
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                BATCHNOXX = ds.Tables[0].Rows[0]["BATCHNO"].ToString();

                if (ds.Tables[0].Rows[0]["CANCELdat"].ToString() != "")
                {
                    return new object[] { 0, "當復機資料已作廢時，不可執行返轉應收結案作業。" };
                }

                if (ds.Tables[0].Rows[0]["strbillingdat"].ToString() == "")
                {
                    return new object[] { 0, "開始計費日空白時不可轉應收結案作業。" };
                }

                //if (ds.Tables[0].Rows[0]["paytype"].ToString() == "02")
                //{
                //    return new object[] { 0, "繳費方式為現金付款時，必須由收款派工單返轉應收帳款。" };
                //}

                if (ds.Tables[0].Rows[0]["batchno"].ToString() == "" || ds.Tables[0].Rows[0]["FINISHDAT"].ToString() == "")
                {
                    return new object[] { 0, "此筆復機資料尚未轉應收帳款，不可執行返轉作業。" };
                }

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    if (ds1.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                    {
                        return new object[] { 0, "當客戶資料作廢時，不可執行返轉應收結案作業。" };
                    }

                    if (ds1.Tables[0].Rows[0]["DROPdat"].ToString() != "")
                    {
                        return new object[] { 0, "當客戶已退租時，不可執行返轉應收結案作業。" };
                    }
                }
            }
            else
            {
                return new object[] { 0, "找不到客戶復機主檔資料，無法執行轉應收帳款結案作業。" };
            }
            //設定輸入參數的值
            try
            {
                cmdRT10447.InfoParameters[0].Value = sdata[0];
                cmdRT10447.InfoParameters[1].Value = sdata[1];
                cmdRT10447.InfoParameters[2].Value = sdata[2];
                cmdRT10447.InfoParameters[3].Value = BATCHNOXX;
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10447.ExecuteNonQuery();
                return new object[] { 0, "用戶復機返轉應收結案作業成功。" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "用戶復機返轉應收結案作業失敗。錯誤訊息:" +ex};
            }
        }

        public object[] smRT10448(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10448.Connection;
            conn.Open();

            string sqlxx;
            DataSet RSXX;
            sqlxx = "select * FROM RTLessorAVSCustReturn WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["finishDAT"].ToString() != "")
                {
                    return new object[] { 0, "此用戶復機資料已結案，不可作廢。(請改用結案返轉)。" };
                }

                if (RSXX.Tables[0].Rows[0]["batchno"].ToString() != "")
                {
                    return new object[] { 0, "此用戶復機資料已轉應收帳款，不可作廢。" };
                }

                if (RSXX.Tables[0].Rows[0]["CANCELdat"].ToString() != "")
                {
                    return new object[] { 0, "此用戶復機資料已作廢，不可重覆執行。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10448.InfoParameters[0].Value = sdata[0];
                cmdRT10448.InfoParameters[1].Value = sdata[1];
                cmdRT10448.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10448.ExecuteNonQuery();
                return new object[] { 0, "用戶復機資料作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶復機資料作廢作業,錯誤訊息" + ex };
            }
        }

        public object[] smRT10449(object[] objParam)
        {//作廢反轉
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10449.Connection;
            conn.Open();

            string sqlxx;
            DataSet RSXX;
            sqlxx = "select * FROM RTLessorAVSCustReturn WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["canceldat"].ToString() == "")
                {
                    return new object[] { 0, "此用戶復機資料尚未作廢，不可返轉。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10449.InfoParameters[0].Value = sdata[0];
                cmdRT10449.InfoParameters[1].Value = sdata[1];
                cmdRT10449.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10449.ExecuteNonQuery();
                return new object[] { 0, "用戶復機資料作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶復機資料作廢作業,錯誤訊息"+ ex };
            }
        }
    }
}
