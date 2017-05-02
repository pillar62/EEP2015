using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT1047
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
            return string.Format("FAQ{0:yyMMdd}", DateTime.Now.Date);
        }

        //完工結案
        public object[] smRT10471(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT10471.Connection;
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
            string sqlyy = "select count(*) as cnt FROM RTLessorAVSCustFaqSndworkFixCode WHERE CUSID='" + sdata[0]+ "' and FAQNO='"+ sdata[1] + "' and prtno='" + sdata[2] + "' ";
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
                cmdRT10471.InfoParameters[0].Value = sdata[0];
                cmdRT10471.InfoParameters[1].Value = sdata[1];
                cmdRT10471.InfoParameters[2].Value = sdata[2];
                cmdRT10471.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10471.ExecuteNonQuery();
                return new object[] { 0, "用戶維修派工單完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行維修派工單完工結案作業,錯誤訊息" + ex };
            }
        }

        //未完工結案
        public object[] smRT10472(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10472.Connection;
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
                cmdRT10472.InfoParameters[0].Value = sdata[0];
                cmdRT10472.InfoParameters[1].Value = sdata[1];
                cmdRT10472.InfoParameters[2].Value = sdata[2];
                cmdRT10472.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10472.ExecuteNonQuery();
                return new object[] { 0, "用戶拆機派工單未完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行拆機派工單未完工結案作業,錯誤訊息" + ex };
            }
        }

        //結案返轉
        public object[] smRT10473(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            string selectSql = "";
            string XXSNDWORK = "";
            string XXSNDUSR = "";
            string XXSNDPRTNO = "";
        //開啟資料連接
        IDbConnection conn = cmdRT10473.Connection;
            conn.Open();
            selectSql = "select * FROM RTLessorAvsCUST WHERE CUSID='" + sdata[0] + "' ";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            selectSql = "select * FROM RTLessorAvsCustFaqH WHERE CUSID='" + sdata[0] + "' and Faqno='" + sdata[1] + "' ";
            cmd.CommandText = selectSql;
            DataSet ds1 = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "客戶資料已作廢，無法返轉。" };
                }
            }
            else
            {
                return new object[] { 0, "找不到客戶基本檔，無法返轉。" };
            }

            if (ds1.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CALLBACKDAT"].ToString() != "")
                {
                    return new object[] { 0, "客服單已押回覆日，不可執行派工單結案返轉。" };
                }
                if (ds.Tables[0].Rows[0]["FINISHDAT"].ToString() != "")
                {
                    return new object[] { 0, "客服單已結案，不可執行派工單結案返轉。" };
                }
                if (ds.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "客戶客服單資料已作廢，不可返轉。" };
                }
                XXSNDWORK = ds.Tables[0].Rows[0]["SNDWORK"].ToString();  
                XXSNDUSR = ds.Tables[0].Rows[0]["SNDUSR"].ToString(); 
                XXSNDPRTNO = ds.Tables[0].Rows[0]["SNDPRTNO"].ToString(); 
            }
            else
            {
                return new object[] { 0, "找不到客戶客服單資料檔，無法返轉。" };
            }

            selectSql = "select * FROM RTLessorAvsCustfaqsndwork WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "' and prtno = '" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            ds = cmd.ExecuteDataSet();
            selectSql = "select * FROM RTLessorAvsCUSTAR WHERE CUSID='" + sdata[0] + "' AND BATCHNO='" + ds.Tables[0].Rows[0]["BATCHNO"].ToString() + "'";
            cmd.CommandText = selectSql;
            ds1 = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    if (ds1.Tables[0].Rows[0]["MDAT"].ToString() != "" && Convert.ToInt32(ds1.Tables[0].Rows[0]["REALAMT"].ToString()) > 1)
                    {
                        return new object[] { 0, "應收帳款已沖帳，不可執行結案返轉作業(請與資訊部連繫)。" };
                    }
                }

                if (ds.Tables[0].Rows[0]["CLOSEDAT"].ToString() == "" && ds.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() == "")
                {
                    return new object[] { 0, "此派工單尚未結案，不可執行結案返轉作業。" };
                }
                if (ds.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "此維修派工單已作廢，不可返轉。" };
                }
                if ((XXSNDPRTNO != "" || XXSNDWORK != "") && ds.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "") 
                {
                    return new object[] { 0, "此維修派工單所屬客服單已產生其它派工單，因此不能執行此派工單返轉作業。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10473.InfoParameters[0].Value = sdata[0];
                cmdRT10473.InfoParameters[1].Value = sdata[1];
                cmdRT10473.InfoParameters[2].Value = sdata[2];
                cmdRT10473.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10473.ExecuteNonQuery();
                return new object[] { 0, "用戶維修派工單結案返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行維修派工單完工結案返轉作業,錯誤訊息" + ex };
            }
        }

        //拆機派工單作廢
        public object[] smRT10474(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10474.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCUSTFAQsndwork WHERE CUSID='" + sdata[0] + "' and faqno='" + sdata[1] + "' and prtno='" + sdata[2] + "' ";
            string sqlYY = "select * FROM RTLessorAVSCUSTFAQH WHERE CUSID='" + sdata[0] + "' and faqno='" + sdata[1] + "' ";
            string sqlzz = "select * FROM RTLessorAVSCustRCVHardware WHERE prtno='" + sdata[2] + "' and canceldat is null ";
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            DataSet RSYY = cmd.ExecuteDataSet();
            cmd.CommandText = sqlzz;
            DataSet RSzz = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {                
                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" || RSXX.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此派工單已完工結案，不可作廢(欲作廢請先執行結案返轉)" };
                }
                
                if (RSXX.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "此派工單已作廢，不可重覆執行作廢作業。" };
                }
            }
            if (RSYY.Tables[0].Rows.Count > 0)
            {                
                if (RSYY.Tables[0].Rows[0]["CALLBACKDAT"].ToString() != "")
                {
                    return new object[] { 0, "此派工單所屬客服單已執行CALLBACK(押回覆日)，不可作廢派工單(請先取消回覆)。" };
                }
                if (RSYY.Tables[0].Rows[0]["FINISHDAT"].ToString() != "")
                {
                    return new object[] { 0, "此派工單所屬客服單已結案，不可作廢派工單。" };
                }
            }
            if (RSYY.Tables[0].Rows.Count > 0)
            {
                return new object[] { 0, "此派工單已產生物品領用單，不可直接作廢(請先返轉物品領用單)。" };
            }
            //設定輸入參數的值
            try
            {
                cmdRT10474.InfoParameters[0].Value = sdata[0];
                cmdRT10474.InfoParameters[1].Value = sdata[1];
                cmdRT10474.InfoParameters[2].Value = sdata[2];
                cmdRT10474.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10474.ExecuteNonQuery();
                return new object[] { 0, "用戶維修派工單作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行派工單作廢作業,錯誤訊息：" + ex };
            }
        }

        //拆機派工單作廢返轉
        public object[] smRT10475(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10475.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCUSTFAQsndwork WHERE CUSID='" + sdata[0] + "' and faqno='" + sdata[1] + "' and prtno='" + sdata[2] + "' ";
            string sqlYY = "select * FROM RTLessorAVSCUSTFAQH WHERE CUSID='" + sdata[0] + "' and faqno='" + sdata[1] + "' ";
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            DataSet RSYY = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["BONUSCLOSEYM"].ToString() != "")
                {
                    return new object[] { 0, "當獎金計算年月已存在資料時表示該筆資料完工日期當月之獎金已結算,不可再作廢返轉。" };
                }

                if (RSXX.Tables[0].Rows[0]["DROPDAT"].ToString() == "")
                {
                    return new object[] { 0, "此用戶派工單尚未作廢，不可執行作廢返轉作業。" };
                }
            }
            if (RSYY.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["SNDPRTNO"].ToString() != "" || RSXX.Tables[0].Rows[0]["SNDWORK"].ToString() != "")
                {
                    return new object[] { 0, "此派工單所屬客服單已另外產生派工單，因此不能執行派工單作廢返轉" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT10475.InfoParameters[0].Value = sdata[0];
                cmdRT10475.InfoParameters[1].Value = sdata[1];
                cmdRT10475.InfoParameters[2].Value = sdata[2];
                cmdRT10475.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10475.ExecuteNonQuery();
                return new object[] { 0, "用戶維修派工單作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶派工單作廢返轉作業,錯誤訊息：" + ex };
            }
        }

        //用戶客服單押客服CALLBACK日期
        public object[] smRT10476(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10476.Connection;
            conn.Open();

            string selectSql = "select * FROM RTLessorAVSCUSTFaqH WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "當已作廢時，不可執行押客服回覆日" };
                }
                ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "已結案時，不可執行押客服CALLBACK日期" };
                }
                ss1 = ds.Tables[0].Rows[0]["SNDPRTNO"].ToString();
                if (ss1 == "")
                {
                    return new object[] { 0, "未產生派工單時，不可執行押客服CALLBACK日期" };
                }

                if (ss1 != "" && ds.Tables[0].Rows[0]["CALLBACKDAT"].ToString() != "")
                {
                    return new object[] { 0, "已有CALLBACK日期時，不可重覆執行" };
                }

                if (ds.Tables[0].Rows[0]["SNDCLOSEDAT"].ToString() == "" && ds.Tables[0].Rows[0]["SNDPRTNO"].ToString() != "")
                {
                    return new object[] { 0, "派工單尚未結案，不可執行押回覆日作業" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10476.InfoParameters[0].Value = sdata[0];
                cmdRT10476.InfoParameters[1].Value = sdata[1];
                cmdRT10476.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10476.ExecuteNonQuery();
                return new object[] { 0, "用戶客服單押客服CALLBACK日期成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行押客服CALLBACK日期作業,錯誤訊息" + ex };
            }
        }

        //用戶客服單取消客服CALLBACK日期
        public object[] smRT10477(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10477.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUSTFaqH WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 == "")
                {
                    return new object[] { 0, "當尚未callback時，不可取消callback日期" };
                }

                ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "已客服單結案時，不可執行取消客服CALLBACK日期(請先執行結案返轉)" };
                }
            }
            ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), conn);

            //設定輸入參數的值
            try
            {
                cmdRT10477.InfoParameters[0].Value = sdata[0];
                cmdRT10477.InfoParameters[1].Value = sdata[1];
                cmdRT10477.InfoParameters[2].Value = sdata[2];
                cmdRT10477.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10477.ExecuteNonQuery();
                return new object[] { 0, "用戶客服單取消客服CALLBACK日期成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行取消客服CALLBACK日期作業,錯誤訊息" + ex };
            }
        }

        //客服結案
        public object[] smRT10478(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            string ss1 = "";
            //開啟資料連接
            IDbConnection conn = cmdRT10478.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUSTFaqH WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "當已作廢時，不可執行客服單結案。" };
                }
                ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 != "" && ds.Tables[0].Rows[0]["SNDCLOSEDAT"].ToString() == "")
                {
                    return new object[] { 0, "此客服單已結案，不可重複執行。" };
                }

                ss1 = ds.Tables[0].Rows[0]["SNDPRTNO"].ToString();
                if (ss1 != "" && ds.Tables[0].Rows[0]["SNDCLOSEDAT"].ToString()=="")
                {
                    return new object[] { 0, "此客服單已轉派工單，派工單需結案後始可執行客服單結案作業。" };
                }

                if (ss1 != "" && ds.Tables[0].Rows[0]["SNDCLOSEDAT"].ToString() != "" && ds.Tables[0].Rows[0]["CALLBACKDAT"].ToString() == "")
                {
                    return new object[] { 0, "此客服單已轉派工單，請先回覆用戶確認故障已排除再執行押回覆日後，始可執行客服單結案作業" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT10478.InfoParameters[0].Value = sdata[0];
                cmdRT10478.InfoParameters[1].Value = sdata[1];
                cmdRT10478.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10478.ExecuteNonQuery();
                return new object[] { 0, "用戶客服單結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行客服單結案作業,錯誤訊息" + ex };
            }
        }

        //客服結案返轉 
        public object[] smRT10479(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10479.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUSTFaqH WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] +"'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 == "")
                {
                    return new object[] { 0, "此客服單尚未結案，不可執行客服單結案返轉。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10479.InfoParameters[0].Value = sdata[0];
                cmdRT10479.InfoParameters[1].Value = sdata[1];
                cmdRT10479.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10479.ExecuteNonQuery();
                return new object[] { 0, "用戶客服單結案返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行客服單結案返轉作業,錯誤訊息" + ex };
            }
        }

        //轉拆機單
        public object[] smRT1047A(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT1047A.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUSTFaqH WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "當已作廢時，不可轉派工單" };
                }
                ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "已結案時，不可轉派工單" };
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
                cmdRT1047A.InfoParameters[0].Value = sdata[0];
                cmdRT1047A.InfoParameters[1].Value = sdata[1];
                cmdRT1047A.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT1047A.ExecuteNonQuery();
                return new object[] { 0, "用戶客服單轉派工單成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行客服單轉派工單作業,錯誤訊息：" + ex };
            }
        }
        //客服作廢
        public object[] smRT1047B(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT1047B.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUSTFaqH WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此用戶客服單資料已結案，不可作廢。(請改用結案返轉)" };
                }
                ss1 = ds.Tables[0].Rows[0]["SNDPRTNO"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此用戶客服單資料已轉派工單，不可作廢。" };
                }
                ss1 = ds.Tables[0].Rows[0]["CALLBACKDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "已押CALLBACK(回覆日)，不可作廢。(請先取消回覆日)" };
                }
                ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此用戶客服單資料已作廢，不可重覆執行。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT1047B.InfoParameters[0].Value = sdata[0];
                cmdRT1047B.InfoParameters[1].Value = sdata[1];
                cmdRT1047B.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT1047B.ExecuteNonQuery();
                return new object[] { 0, "用戶客服單資料作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶客服單資料作廢作業,錯誤訊息" + ex };
            }
        }

        //退租作廢返轉 
        public object[] smRT1047C(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT1047C.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUSTFaqH WHERE CUSID='" + sdata[0] + "' and FAQNO='" + sdata[1] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 == "")
                {
                    return new object[] { 0, "此用戶客服單資料尚未作廢，不可返轉。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT1047C.InfoParameters[0].Value = sdata[0];
                cmdRT1047C.InfoParameters[1].Value = sdata[1];
                cmdRT1047C.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT1047C.ExecuteNonQuery();
                return new object[] { 0, "用戶客服單資料作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶客服單資料作廢作業,錯誤訊息" + ex };
            }
        }
    }
}
