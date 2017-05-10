using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT106
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
            return string.Format("ET{0:yyMMdd}", DateTime.Now.Date);
        }

        //完工結案
        public object[] smRT1061(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT1061.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUST WHERE CUSID='" + sdata[0] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            selectSql = "select * FROM RTLessorAVSCustFAQH WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "'";
            cmd.CommandText = selectSql;
            DataSet ds1 = cmd.ExecuteDataSet();
            selectSql = "select * FROM RTLessorAVSCUSTFaqsndwork WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "' and prtno = '" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds2 = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "客戶已作廢，無法結案(派工單必須作廢)。" };
                }
            }
            else
            {
                return new object[] { 0, "找不到客戶基本檔，無法結案。" };
            }

            if (ds1.Tables[0].Rows.Count > 0)
            {
                if (ds1.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "此維修派工單所屬客服單資料已作廢，不可執行完工結案作業" };
                }
                if (ds1.Tables[0].Rows[0]["SNDCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此維修派工單所屬客服單已有派工單結案日，請連絡資訊部" };
                }
            }
            else
            {
                return new object[] { 0, "找不到此維修派工單所屬客服單資料" };
            }

            //判斷是否有領用單未結
            selectSql = "select count(*) as CNT FROM RTLessorAVSCustFAQHardware WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "' and prtno = '" + sdata[2] + "' and dropdat is null and rcvfinishdat is null ";
            cmd.CommandText = selectSql;
            ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["CNT"].ToString()) > 0)
                {
                    return new object[] { 0, "此維修派工單設備資料中，尚有設備未辦妥物品領用程序(未領用或領用未結案)，不可執行完工結案作業。" };
                }
            }

            string sqlxx = "select * FROM RTLessorAVSCustFAQsndwork WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "' and prtno='" + sdata[2] + "' ";
            string sqlyy = "select count(*) as cnt FROM RTLessorAVSCustFaqSndworkFixCode WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "' and prtno='" + sdata[2] + "' ";
            cmd.CommandText = sqlxx;
            ds = cmd.ExecuteDataSet();
            cmd.CommandText = sqlyy;
            ds1 = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "當已作廢時，不可執行完工結案或未完工結案" };
                }
                if (ds.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" && ds.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此維修派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案。" };
                }
                if (ds.Tables[0].Rows[0]["REALENGINEER"].ToString() == "" && ds.Tables[0].Rows[0]["REALCONSIGNEE"].ToString() == "")
                {
                    return new object[] { 0, "此維修派工單完工時，必須先輸入實際裝機人員或實際裝機經銷商。" };
                }
                if (ds.Tables[0].Rows[0]["BONUSCLOSEYM"].ToString() != "" && ds.Tables[0].Rows[0]["STOCKCLOSEYM"].ToString() != "")
                {
                    return new object[] { 0, "此維修派工單已月結，不可異動。" };
                }
                if (ds.Tables[0].Rows[0]["BATCHNO"].ToString() != "")
                {
                    return new object[] { 0, "此維修派工單已產生應收帳款，無法重複結案，請連絡資訊部" };
                }
                if (ds.Tables[0].Rows[0]["MEMO"].ToString() == "" && Convert.ToInt32(ds1.Tables[0].Rows[0]["cnt"].ToString()) < 1)
                {
                    return new object[] { 0, "維修派工單結案時，必須輸入維修處理過程說明。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT1061.InfoParameters[0].Value = sdata[0];
                cmdRT1061.InfoParameters[1].Value = sdata[1];
                cmdRT1061.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT1061.ExecuteNonQuery();
                return new object[] { 0, "用戶維修派工單完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行維修派工單完工結案作業,錯誤訊息" + ex };
            }
        }

        //未完工結案
        public object[] smRT1062(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT1062.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUSTFaqsndwork WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "' and prtno = '" + sdata[2] + "'";
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
                cmdRT1062.InfoParameters[0].Value = sdata[0];
                cmdRT1062.InfoParameters[1].Value = sdata[1];
                cmdRT1062.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT1062.ExecuteNonQuery();
                return new object[] { 0, "用戶拆機派工單未完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行拆機派工單未完工結案作業,錯誤訊息" + ex };
            }
        }
            
        //作廢
        public object[] smRT1063(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT1063.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtyContract WHERE comq1=" + sdata[0] + " and CONTRACTNO='" + sdata[1] + "' ";
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此社區合約資料已結案，不可作廢。(請改用結案返轉)" };
                }

                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "此社區合約資料已作廢，不可重覆執行。" };
                }
            }
            
            //設定輸入參數的值
            try
            {
                cmdRT1063.InfoParameters[0].Value = sdata[0];
                cmdRT1063.InfoParameters[1].Value = sdata[1];
                cmdRT1063.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT1063.ExecuteNonQuery();
                return new object[] { 0, "社區合約資料作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行社區合約資料作廢作業,錯誤訊息：" + ex };
            }
        }

        //拆機派工單作廢返轉
        public object[] smRT1064(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT1064.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtyContract WHERE comq1=" + sdata[0] + " and CONTRACTNO='" + sdata[1] + "' ";
            string sqlYY = "select count(*) as cnt FROM RTLessorAVSCmtyContract WHERE  COMQ1=" + sdata[0] + " and canceldat is null ";
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            DataSet RSYY = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() == "")
                {
                    return new object[] { 0, "此社區合約資料尚未作廢，不可返轉。" };
                }
            }

            if (RSYY.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(RSYY.Tables[0].Rows[0]["cnt"].ToString()) > 0)
                {
                    return new object[] { 0, "在此筆資料之後已有其它調整資料存在，因此不可執行作廢返轉。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT1064.InfoParameters[0].Value = sdata[0];
                cmdRT1064.InfoParameters[1].Value = sdata[1];
                cmdRT1064.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT1064.ExecuteNonQuery();
                return new object[] { 0, "社區合約資料作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "此社區已有其它有效的合約資料存在，不可作廢返轉：" + ex };
            }
        }    
    }
}
