using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT1043
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
        public object[] smRT10431(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10431.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10431.InfoParameters[0].Value = sdata[0];
                cmdRT10431.InfoParameters[1].Value = sdata[1];
                cmdRT10431.InfoParameters[2].Value = sdata[2];
                cmdRT10431.InfoParameters[3].Value = sdata[3];
                cmdRT10431.InfoParameters[4].Value = sdata[4];
                cmdRT10431.InfoParameters[5].Value = sdata[5];
                cmdRT10431.InfoParameters[6].Value = sdata[6];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10431.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10432(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10432.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10432.InfoParameters[0].Value = sdata[0];
                cmdRT10432.InfoParameters[1].Value = sdata[1];
                cmdRT10432.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10432.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10433(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10433.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10433.InfoParameters[0].Value = sdata[0];
                cmdRT10433.InfoParameters[1].Value = sdata[1];
                cmdRT10433.InfoParameters[2].Value = sdata[2];
                cmdRT10433.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10433.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10434(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10434.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10434.InfoParameters[0].Value = sdata[0];
                cmdRT10434.InfoParameters[1].Value = sdata[1];
                cmdRT10434.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10434.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10435(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10435.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10435.InfoParameters[0].Value = sdata[0];
                cmdRT10435.InfoParameters[1].Value = sdata[1];
                cmdRT10435.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10435.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10436(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10436.Connection;
            conn.Open();
            string selectSql, sqlxx, sqlYY, sqlzz;
            DataSet rsyy, RSXX, RSzz;
            sqlxx = "select * FROM RTLessorAVSCustCont WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1];
            sqlYY = "select * FROM RTLessorAVSCUST WHERE CUSID='" + sdata[0] + "' ";
            sqlzz = "select count(*) as cnt FROM RTLessorAVSCUSTcontsndwork WHERE CUSID='" + sdata[0] + "' and entryno=" + sdata[1] + " and dropdat is null and unclosedat is null and closedat is null ";
            selectSql = " select * FROM RTLessorAVSCustCont WHERE  CUSID='" + sdata[0] + "' AND entryno = " + sdata[1];
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
                    return new object[] { 0, "此筆續約資料已轉應收帳款，不可重複產生。" };
                }

                if (rsyy.Tables[0].Rows.Count > 0)
                {
                    if (rsyy.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                    {
                        return new object[] { 0, "客戶資料已作廢，必須作廢續約作業。" };
                    }

                    if (rsyy.Tables[0].Rows[0]["DROPdat"].ToString() != "")
                    {
                        return new object[] { 0, "客戶資料已退租，必須作廢續約資料。" };
                    }
                }
            }
            else
            {
                return new object[] { 0, "找不到客戶續約主檔資料，無法執行返轉應收帳款結案作業。" };
            }

            if (RSzz.Tables[0].Rows.Count <= 0)
            {
                if (Convert.ToInt32(RSzz.Tables[0].Rows[0]["cnt"].ToString()) > 0)
                return new object[] { 0, "此續約資料已存在收款派工單，必須由派工單進行結案作業。" };
            }

            //設定輸入參數的值
            try
            {
                cmdRT10436.InfoParameters[0].Value = sdata[0];
                cmdRT10436.InfoParameters[1].Value = sdata[1];
                cmdRT10436.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10436.ExecuteNonQuery();
                return new object[] { 0, "用戶續約轉應收帳款成功 "};
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶續約轉應收帳款作業,錯誤訊息" + ex.Message };
            }
        }

        public object[] smRT10437(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            string BATCHNOXX = "";
            //開啟資料連接
            IDbConnection conn = cmdRT10437.Connection;
            conn.Open();
            string selectSql = "select max(entryno)as entryno from RTLessorAVSCUSTlog where CUSID='" + sdata[0] + "'";
            cmd.CommandText = selectSql;
            DataSet RSXX = cmd.ExecuteDataSet();
            int xxmaxentryno;

            if (RSXX.Tables[0].Rows.Count <= 0)
            {
                xxmaxentryno = 0;
            }
            else
            {
                xxmaxentryno = Convert.ToInt32(RSXX.Tables[0].Rows[0]["entryno"].ToString());
            }

            selectSql = " select * FROM RTLessorAVSCustCont WHERE  CUSID='" + sdata[0] + "' AND entryno = " + sdata[1];
            cmd.CommandText = selectSql;
            RSXX = cmd.ExecuteDataSet();

            selectSql = "select * FROM RTLessorAVSCust WHERE CUSID='" + sdata[0] + "'";
            cmd.CommandText = selectSql;
            DataSet rsyy = cmd.ExecuteDataSet();

            if (rsyy.Tables[0].Rows.Count <= 0)
            {
                return new object[] { 0, "找不到客戶主檔資料，無法執行轉應收帳款結案作業。" };
            }

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "約資料已作廢時，不可執行返轉應收結案作業。" };
                }

                /*
                if (RSXX.Tables[0].Rows[0]["paytype"].ToString() == "02")
                {
                    return new object[] { 0, "繳費方式為現金付款時，必須由收款派工單產生應收帳款。" };
                }
                */

                if (RSXX.Tables[0].Rows[0]["batchno"].ToString() == "" || RSXX.Tables[0].Rows[0]["FINISHDAT"].ToString() == "")
                {
                    return new object[] { 0, "batchno為空白或結案日為空白時，表示此筆續約資料尚未轉應收帳款，不可執行返轉作業。" };
                }

                BATCHNOXX = RSXX.Tables[0].Rows[0]["batchno"].ToString();

                if (rsyy.Tables[0].Rows.Count > 0)
                {
                    if (rsyy.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                    {
                        return new object[] { 0, "客戶資料已作廢，不可執行返轉應收結案作業。" };
                    }

                    if (rsyy.Tables[0].Rows[0]["DROPdat"].ToString() != "")
                    {
                        return new object[] { 0, "客戶資料尚未退租，不可執行返轉應收結案作業。" };
                    }
                }
            }
            else
            {
                return new object[] { 0, "找不到客戶續約主檔資料，無法執行返轉應收帳款結案作業。" };
            }

            string sqlyy = "select * FROM RTLessorAVSCUSTAR WHERE CUSID='" + sdata[0] + "' AND BATCHNO='" + BATCHNOXX + "'";
            cmd.CommandText = sqlyy;
            RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["mdat"].ToString() != "" && Convert.ToInt32(RSXX.Tables[0].Rows[0]["REALAMT"].ToString()) > 0)
                {
                    return new object[] { 0, "應收帳款已沖帳不可結案返轉。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10437.InfoParameters[0].Value = sdata[0];
                cmdRT10437.InfoParameters[1].Value = sdata[1];
                cmdRT10437.InfoParameters[2].Value = sdata[2];
                cmdRT10437.InfoParameters[3].Value = BATCHNOXX;
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10437.ExecuteNonQuery();
                return new object[] { 0, "用戶續約返轉應收帳款成功 " };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶續約返轉應收帳款作業,錯誤訊息" + ex.Message };
            }
        }

        public object[] smRT10438(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');            
            //開啟資料連接
            IDbConnection conn = cmdRT10438.Connection;
            conn.Open();
            string selectSql = " select * FROM RTLessorAVSCUSTCONT WHERE CUSID='" + sdata[0] + "' AND entryno = " + sdata[1];
            cmd.CommandText = selectSql;
            DataSet RSXX = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["finishDAT"].ToString() != "")
                {
                    return new object[] { 0, "此用戶續約資料已結案，不可作廢。(請改用結案返轉)。" };
                }

                
                if (RSXX.Tables[0].Rows[0]["batchno"].ToString() != "")
                {
                    return new object[] { 0, "此用戶續約資料已轉應收帳款，不可作廢。" };
                }
                

                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "此用戶續約資料已作廢，不可重覆執行。" };
                }

            }
            else
            {
                return new object[] { 0, "找不到客戶續約主檔資料，無法執行作廢作業。" };
            }

            //設定輸入參數的值
            try
            {
                cmdRT10438.InfoParameters[0].Value = sdata[0];
                cmdRT10438.InfoParameters[1].Value = sdata[1];
                cmdRT10438.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10438.ExecuteNonQuery();
                return new object[] { 0, "用戶續約資料作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶續約資料作廢作業,錯誤訊息" + ex.Message };
            }
        }

        public object[] smRT10439(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10439.Connection;
            conn.Open();
            string selectSql = " select * FROM RTLessorAVSCUSTCONT WHERE CUSID='" + sdata[0] + "' AND entryno = " + sdata[1];
            cmd.CommandText = selectSql;
            DataSet RSXX = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() == "")
                {
                    return new object[] { 0, "此用戶續約資料尚未作廢，不可返轉。" };
                }
            }
            else
            {
                return new object[] { 0, "找不到客戶續約主檔資料，無法執行作廢反轉作業。" };
            }

            //設定輸入參數的值
            try
            {
                cmdRT10439.InfoParameters[0].Value = sdata[0];
                cmdRT10439.InfoParameters[1].Value = sdata[1];
                cmdRT10439.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10439.ExecuteNonQuery();
                return new object[] { 0, "用戶續約資料作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶續約資料作廢作業,錯誤訊息" + ex.Message };
            }
        }
    }
}
