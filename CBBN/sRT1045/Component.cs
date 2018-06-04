using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT1045
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

        //完工結案
        public object[] smRT10451(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10451.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCustdropsndwork WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and prtno = '"+ sdata[2] + "'";
            string ss2 = "";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["DROPDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "當已作廢時，不可執行完工結案或未完工結案" };
                }
                ss1 = ds.Tables[0].Rows[0]["CLOSEDAT"].ToString();
                ss2 = ds.Tables[0].Rows[0]["UNCLOSEDAT"].ToString();
                if (ss1 != "" || ss2 != "")
                {
                    return new object[] { 0, "此拆機派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案。" };
                }
                ss1 = ds.Tables[0].Rows[0]["REALENGINEER"].ToString();
                ss2 = ds.Tables[0].Rows[0]["REALCONSIGNEE"].ToString();
                if (ss1 == "" && ss2 == "")
                {
                    return new object[] { 0, "此拆機派工單完工時，必須先輸入實際拆機人員或實際拆機經銷商。" };
                }
                ss1 = ds.Tables[0].Rows[0]["BONUSCLOSEYM"].ToString();
                ss2 = ds.Tables[0].Rows[0]["STOCKCLOSEYM"].ToString();
                if (ss1 != "" || ss2 != "")
                {
                    return new object[] { 0, "此拆機派工單已月結，不可異動。" };
                }
                ss1 = ds.Tables[0].Rows[0]["BATCHNO"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此拆機派工單已產生應收帳款，無法重複結案，請連絡資訊部。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT10451.InfoParameters[0].Value = sdata[0];
                cmdRT10451.InfoParameters[1].Value = sdata[1];
                cmdRT10451.InfoParameters[2].Value = sdata[2];
                cmdRT10451.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10451.ExecuteNonQuery();
                return new object[] { 0, "用戶拆機派工單完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行拆機派工單完工結案作業,錯誤訊息" + ex };
            }
        }
        
        //未完工結案
        public object[] smRT10452(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10452.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCustdropsndwork WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and prtno = '" + sdata[2] + "'";
            string ss2 = "";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["DROPDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "當已作廢時，不可執行完工結案或未完工結案" };
                }
                ss1 = ds.Tables[0].Rows[0]["CLOSEDAT"].ToString();
                ss2 = ds.Tables[0].Rows[0]["UNCLOSEDAT"].ToString();
                if (ss1 != "" || ss2 != "")
                {
                    return new object[] { 0, "此拆機派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案。" };
                }
                ss1 = ds.Tables[0].Rows[0]["BATCHNO"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此拆機派工單已產生應收帳款，無法重複結案，請連絡資訊部。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT10452.InfoParameters[0].Value = sdata[0];
                cmdRT10452.InfoParameters[1].Value = sdata[1];
                cmdRT10452.InfoParameters[2].Value = sdata[2];
                cmdRT10452.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10452.ExecuteNonQuery();
                return new object[] { 0, "用戶拆機派工單未完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行拆機派工單未完工結案作業,錯誤訊息" + ex };
            }
        }

        //結案返轉
        public object[] smRT10453(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10453.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCustDROPsndwork WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and prtno = '" + sdata[2] + "'";
            string ss2 = "";
            string ss1 = "";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ss1 = ds.Tables[0].Rows[0]["CLOSEDAT"].ToString();
                ss2 = ds.Tables[0].Rows[0]["UNCLOSEDAT"].ToString();
                if (ss1 == "" && ss2 == "")
                {
                    return new object[] { 0, "此拆機派工單尚未結案，不可執行結案返轉作業。" };
                }
                ss1 = ds.Tables[0].Rows[0]["DROPDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此拆機派工單已作廢，不可返轉。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT10453.InfoParameters[0].Value = sdata[0];
                cmdRT10453.InfoParameters[1].Value = sdata[1];
                cmdRT10453.InfoParameters[2].Value = sdata[2];
                cmdRT10453.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10453.ExecuteNonQuery();
                return new object[] { 0, "用戶拆機派工單結案返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行拆機派工單完工結案作業,錯誤訊息" + ex };
            }
        }

        //拆機派工單作廢
        public object[] smRT10454(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10454.Connection;
            conn.Open();
            string ss2 = "";
            string ss1 = "";
            string selectSql = "select * FROM RTLessorAVSCustDROPsndwork WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and prtno = '" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            selectSql = "select * FROM RTLessorAVSCUSTDROP WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            cmd.CommandText = selectSql;
            DataSet ds1 = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ss1 = ds.Tables[0].Rows[0]["CLOSEDAT"].ToString();
                ss2 = ds.Tables[0].Rows[0]["UNCLOSEDAT"].ToString();
                if (ss1 != "" || ss2 != "")
                {
                    return new object[] { 0, "此拆機派工單已完工結案，不可作廢(欲作廢請先執行結案返轉)" };
                }
                ss1 = ds.Tables[0].Rows[0]["DROPDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此拆機派工單已作廢，不可重覆執行作廢作業。" };
                }
            }
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ss1 = ds1.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此拆機派工單所屬退租資料已結案，不可作廢派工單。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT10454.InfoParameters[0].Value = sdata[0];
                cmdRT10454.InfoParameters[1].Value = sdata[1];
                cmdRT10454.InfoParameters[2].Value = sdata[2];
                cmdRT10454.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10454.ExecuteNonQuery();
                return new object[] { 0, "用戶拆機派工單作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行拆機派工單作廢作業,錯誤訊息：" + ex };
            }
        }

        //拆機派工單作廢返轉
        public object[] smRT10455(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10455.Connection;
            conn.Open();
            string ss2 = "";
            string ss1 = "";
            string selectSql = "select * FROM RTLessorAVSCustDROPsndwork WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and prtno = '" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            selectSql = "select * FROM RTLessorAVSCUSTDROP WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            cmd.CommandText = selectSql;
            DataSet ds1 = cmd.ExecuteDataSet();
            selectSql = "select MAX(prtno) AS XXPRTNO FROM RTLessorAVSCUSTDROPsndwork WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            cmd.CommandText = selectSql;
            DataSet ds2 = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ss1 = ds.Tables[0].Rows[0]["BONUSCLOSEYM"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "當獎金計算年月已存在資料時表示該筆資料完工日期當月之獎金已結算,不可再作廢返轉。" };
                }
                ss1 = ds.Tables[0].Rows[0]["DROPDAT"].ToString();
                if (ss1 == "")
                {
                    return new object[] { 0, "此用戶拆機派工單尚未作廢，不可執行作廢返轉作業" };
                }
            }
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ss1 = ds1.Tables[0].Rows[0]["SNDPRTNO"].ToString();
                ss2 = ds1.Tables[0].Rows[0]["SNDWORK"].ToString();
                if (ss1 != "" || ss2 != "")
                {
                    return new object[] { 0, "此拆機派工單所屬退租資料已另外產生拆機派工單，因此不能執行派工單作廢返轉。" };
                }
            }

            if (ds2.Tables[0].Rows.Count > 0)
            {
                ss1 = ds2.Tables[0].Rows[0]["XXPRTNO"].ToString();
                if (ss1.CompareTo(sdata[2]) > 0)
                {
                    return new object[] { 0, "當有其它拆機派工單存在時(且拆機單號大於本單單號，則不允許作廢返轉) 。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10455.InfoParameters[0].Value = sdata[0];
                cmdRT10455.InfoParameters[1].Value = sdata[1];
                cmdRT10455.InfoParameters[2].Value = sdata[2];
                cmdRT10455.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10455.ExecuteNonQuery();
                return new object[] { 0, "用戶拆機派工單作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶拆機派工單作廢返轉作業, 錯誤訊息：" + ex };
            }
        }

        //退租結案
        public object[] smRT10456(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10456.Connection;
            conn.Open();
            
            string selectSql = "select * FROM RTLessorAVSCUSTDROP WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            selectSql = "select * FROM RTLessorAVSCUST WHERE CUSID='" + sdata[0] + "'";
            cmd.CommandText = selectSql;
            DataSet ds1 = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "當退租單已作廢時，不可結案!!" };
                }
                ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "已退租單結案時，不可重複執行" };
                }
                /*
                ss1 = ds.Tables[0].Rows[0]["SNDPRTNO"].ToString();
                if (ss1 == "")
                {
                    return new object[] { 0, "此退租單尚未轉拆機派工單，派工單需結案後始可執行退租單結案作業" };
                }
                
                if (ss1 != "" && ds.Tables[0].Rows[0]["SNDWORKCLOSE"].ToString() == "")
                {
                    return new object[] { 0, "此退租單之拆機派工單尚未結案，不可執行退租單結案作業" };
                }*/

                if (ds1.Tables[0].Rows[0]["batchno"].ToString() == "" && ds.Tables[0].Rows[0]["DROPKIND"].ToString() == "02")
                {
                    return new object[] { 0, "此客戶之主檔中並無應收帳款編號資料，無法執行退租單結案作業。(請聯絡資訊部)" };
                }
            }
            
            //設定輸入參數的值
            try
            {
                cmdRT10456.InfoParameters[0].Value = sdata[0];
                cmdRT10456.InfoParameters[1].Value = sdata[1];
                cmdRT10456.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10456.ExecuteNonQuery();
                return new object[] { 0, "用戶退租單結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行退租單結案作業,錯誤訊息" + ex };
            }
        }

        //退租結案返轉
        public object[] smRT10457(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10457.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUSTDROP WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            selectSql = "select * FROM RTLessorAVSCUST WHERE CUSID='" + sdata[0] + "'";
            cmd.CommandText = selectSql;
            DataSet ds1 = cmd.ExecuteDataSet();
            String batchno = ds.Tables[0].Rows[0]["batchno"].ToString();
            if (batchno == "")
            {

            }
            else
            {
                selectSql = "select * FROM RTLessorAVSCUSTAR WHERE CUSID='" + sdata[0] + "' AND batchno='" + batchno + "'";
                cmd.CommandText = selectSql;
                DataSet ds2 = cmd.ExecuteDataSet();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                    if (ss1 == "")
                    {
                        return new object[] { 0, "此退租單尚未結案，不可執行結案返轉" };
                    }
                }
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    string ss1 = ds1.Tables[0].Rows[0]["dropdat"].ToString();
                    if (ss1 == "")
                    {
                        return new object[] { 0, "此客戶目前並非退租狀態，不可執行退租結案返轉" };
                    }
                }
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    string ss1 = ds2.Tables[0].Rows[0]["mdat"].ToString();
                    if (ss1 != "" && ds2.Tables[0].Rows[0]["realamt"].ToString() != "0")
                    {
                        return new object[] { 0, "此退租單之應收帳款已沖帳，不可執行結案返轉作業" };
                    }
                }
            }
            ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), conn);

            //設定輸入參數的值
            try
            {
                cmdRT10457.InfoParameters[0].Value = sdata[0];
                cmdRT10457.InfoParameters[1].Value = sdata[1];
                cmdRT10457.InfoParameters[2].Value = sdata[2];
                
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10457.ExecuteNonQuery();
                return new object[] { 0, "退租結案返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "退租結案返轉作業,錯誤訊息" + ex };
            }
        }

        //退租作廢
        public object[] smRT10458(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10458.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUSTDROP WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此用戶退租單資料已結案，不可作廢。(請改用結案返轉)" };
                }
                ss1 = ds.Tables[0].Rows[0]["SNDPRTNO"].ToString();
                /*
                if (ss1 != "")
                {
                    return new object[] { 0, "用戶退租單資料已轉派工單，不可作廢。" };
                }
                */
                ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此用戶退租單資料已作廢，不可重覆執行。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT10458.InfoParameters[0].Value = sdata[0];
                cmdRT10458.InfoParameters[1].Value = sdata[1];
                cmdRT10458.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10458.ExecuteNonQuery();
                return new object[] { 0, "用戶退租單資料作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶退租單資料作廢作業,錯誤訊息" + ex };
            }
        }

        //退租作廢返轉 
        public object[] smRT10459(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10459.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUSTDROP WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            selectSql = "select max(entryno) as XXENTRYNO FROM RTLessorAVSCustDrop WHERE  CUSID='" + sdata[0] + "'";
            cmd.CommandText = selectSql;
            DataSet ds1 = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 == "")
                {
                    return new object[] { 0, "此用戶退租單資料已作廢，不可重覆執行。" };
                }
            }

            if (ds1.Tables[0].Rows.Count > 0)
            {
                int ss1 = Convert.ToInt32(ds1.Tables[0].Rows[0]["XXENTRYNO"].ToString());
                if (ss1 > Convert.ToInt32(sdata[1]))
                {
                    return new object[] { 0, "有其它退租單存在時(且退租項次大於本單項次，則不允許作廢返轉)。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT10459.InfoParameters[0].Value = sdata[0];
                cmdRT10459.InfoParameters[1].Value = sdata[1];
                cmdRT10459.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10459.ExecuteNonQuery();
                return new object[] { 0, "用戶退租單資料作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0,"無法執行用戶退租單資料作廢作業,錯誤訊息" +  ex };
            }
        }

        //轉拆機單
        public object[] smRT1045A(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT1045A.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUSTDrop WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0) {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "當退租單已作廢時，不可轉派工單" };
                }
                ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "已退租單結案時，不可轉派工單" };
                }
                ss1 = ds.Tables[0].Rows[0]["SNDPRTNO"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "已有派工單時，不可重覆執行" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT1045A.InfoParameters[0].Value = sdata[0];
                cmdRT1045A.InfoParameters[1].Value = sdata[1];
                cmdRT1045A.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT1045A.ExecuteNonQuery();
                return new object[] { 0, "用戶退租單轉派工單成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "用戶退租單轉派工單失敗，錯誤訊息：" + ex };
            }
            
        }
    }
}
