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
            //設定輸入參數的值
            try
            {
                cmdRT10441.InfoParameters[0].Value = sdata[0];
                cmdRT10441.InfoParameters[1].Value = sdata[1];
                cmdRT10441.InfoParameters[2].Value = sdata[2];
                cmdRT10441.InfoParameters[3].Value = sdata[3];
                cmdRT10441.InfoParameters[4].Value = sdata[4];
                cmdRT10441.InfoParameters[5].Value = sdata[5];
                cmdRT10441.InfoParameters[6].Value = sdata[6];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10441.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
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
        

            //設定輸入參數的值
            try
            {
                cmdRT10442.InfoParameters[0].Value = sdata[0];
                cmdRT10442.InfoParameters[1].Value = sdata[1];
                cmdRT10442.InfoParameters[2].Value = sdata[2];
                cmdRT10442.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10442.ExecuteNonQuery();
                return new object[] { 0, "用戶復機返轉應收結案作業成功。" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "用戶復機返轉應收結案作業失敗,錯誤訊息：" + ex };
            }
        }

        public object[] smRT10443(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10443.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10443.InfoParameters[0].Value = sdata[0];
                cmdRT10443.InfoParameters[1].Value = sdata[1];
                cmdRT10443.InfoParameters[2].Value = sdata[2];
                cmdRT10443.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10443.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10444(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10444.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10444.InfoParameters[0].Value = sdata[0];
                cmdRT10444.InfoParameters[1].Value = sdata[1];
                cmdRT10444.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10444.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10445(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10445.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10445.InfoParameters[0].Value = sdata[0];
                cmdRT10445.InfoParameters[1].Value = sdata[1];
                cmdRT10445.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10445.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
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

            string selectSql = "select * FROM RTLessorAVSCustReturn WHERE CUSID='" + sdata[0] + "' and ENTRYNO=" + sdata[1];
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            selectSql = "select * FROM RTLessorAVSCust WHERE CUSID='" + sdata[0] + "'";
            cmd.CommandText = selectSql;
            DataSet ds1 = cmd.ExecuteDataSet();
            selectSql = "select count(*) as cnt FROM RTLessorAVSCustReturnsndwork  WHERE CUSID='" + sdata[0] + "' and ENTRYNO=" + sdata[1] + " and dropdat is null and unclosedat is null and closedat is null";
            cmd.CommandText = selectSql;
            DataSet ds2 = cmd.ExecuteDataSet();

            if (ds1.Tables[0].Rows.Count <= 0)
            {
                return new object[] { 0, "找不到客戶主檔資料，無法執行轉應收帳款結案作業。" };
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CANCELdat"].ToString() != "")
                {
                    return new object[] { 0, "當復機資料已作廢時，不可執行轉應收帳款作業。" };
                }

                if (ds.Tables[0].Rows[0]["strbillingdat"].ToString() == "")
                {
                    return new object[] { 0, "開始計費日空白時不可轉應收結案作業。" };
                }

                if (ds.Tables[0].Rows[0]["paytype"].ToString() == "02")
                {
                    return new object[] { 0, "繳費方式為現金付款時，必須由收款派工單產生應收帳款。" };
                }

                if (ds.Tables[0].Rows[0]["batchno"].ToString() != "" || ds.Tables[0].Rows[0]["FINISHDAT"].ToString() != "")
                {
                    return new object[] { 0, "此筆復機資料已轉應收帳款，不可重複產生。" };
                }

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    if (ds1.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                    {
                        return new object[] { 0, "客戶資料已作廢，必須作廢復機資料。" };
                    }

                    if (ds1.Tables[0].Rows[0]["DROPdat"].ToString() == "")
                    {
                        return new object[] { 0, "客戶資料尚未退租，不可執行復機轉應收帳款作業。" };
                    }
                }
            }
            else
            {
                return new object[] { 0, "找不到客戶復機主檔資料，無法執行轉應收帳款結案作業。" };
            }

            if (ds2.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds2.Tables[0].Rows[0]["cnt"].ToString()) > 0)
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

                if (ds.Tables[0].Rows[0]["paytype"].ToString() == "02")
                {
                    return new object[] { 0, "繳費方式為現金付款時，必須由收款派工單返轉應收帳款。" };
                }

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
            //設定輸入參數的值
            try
            {
                cmdRT10448.InfoParameters[0].Value = sdata[0];
                cmdRT10448.InfoParameters[1].Value = sdata[1];
                cmdRT10448.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10448.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10449(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10449.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10449.InfoParameters[0].Value = sdata[0];
                cmdRT10449.InfoParameters[1].Value = sdata[1];
                cmdRT10449.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10449.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }
    }
}
