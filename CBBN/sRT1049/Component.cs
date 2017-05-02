using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT1049
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
        public object[] smRT10491(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT10491.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCust WHERE CUSID='" + sdata[0] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            selectSql = "select * FROM RTLessorAVSCustADJDAY WHERE CUSID='" + sdata[0] + "' and ENTRYNO=" + sdata[1];
            cmd.CommandText = selectSql;
            DataSet ds1 = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CANCELdat"].ToString() != "" || ds.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "當用戶主檔資料為已作廢或已退租時，不可結案。" };
                }

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["PERIOD"].ToString()) + Convert.ToInt32(ds1.Tables[0].Rows[0]["ADJPERIOD"].ToString()) <0)
                    {
                        return new object[] { 0, "當用戶主檔之期數資料與調整資料之期數相加減結果小於零者，則不允許結案。" };
                    }

                    if (ds1.Tables[0].Rows[0]["ADJCLOSEDAT"].ToString() != "")
                    {
                        return new object[] { 0, "當用戶已作廢時，不可結案。" };
                    }
                    if (ds1.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                    {
                        return new object[] { 0, "當用戶主檔資料為已作廢或已退租時，不可結案。" };
                    }
                }
            }
            else
            {
                return new object[] { 0, "找不到客戶基本檔，無法結案。" };
            }                    
            
            //設定輸入參數的值
            try
            {
                cmdRT10491.InfoParameters[0].Value = sdata[0];
                cmdRT10491.InfoParameters[1].Value = sdata[1];
                cmdRT10491.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10491.ExecuteNonQuery();
                return new object[] { 0, "用戶調整到期日數資料結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶調整到期日數資料結案作業,錯誤訊息：" + ex };
            }
        }

        //結案返轉
        public object[] smRT10492(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            
            //開啟資料連接
            IDbConnection conn = cmdRT10492.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCustADJDAY WHERE CUSID='" + sdata[0] + "' and ENTRYNO=" + sdata[1];
            string sqlYY = "select * FROM RTLessorAVSCust WHERE CUSID='" + sdata[0] + "' ";
            string sqlzz = "select count(*) as cnt FROM RTLessorAVSCustADJDAY WHERE CUSID='" + sdata[0] + "' and canceldat is null and adjclosedat is null ";
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            DataSet RSYY = cmd.ExecuteDataSet();
            cmd.CommandText = sqlzz;
            DataSet RSzz = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["ADJCLOSEDAT"].ToString() == "") 
                {
                    return new object[] { 0, "當用戶尚未結案，不可結返返轉。" };
                }

                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "當用戶已作廢時，不可結案返轉。" };
                }

                if (RSYY.Tables[0].Rows.Count > 0)
                {
                    if (RSYY.Tables[0].Rows[0]["CANCELDAT"].ToString() != "" || RSYY.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                    {
                        return new object[] { 0, "當用戶主檔資料為已作廢或已退租時，不可結案返轉。" };
                    }

                    if (Convert.ToInt32(RSYY.Tables[0].Rows[0]["PERIOD"].ToString()) - Convert.ToInt32(RSXX.Tables[0].Rows[0]["ADJPERIOD"].ToString()) < 0)
                    {
                        return new object[] { 0, "當用戶主檔之期數資料與調整資料之期數相加減結果小於零者，則不允許結案返轉。" };
                    }
                }
            }
                      
            //設定輸入參數的值
            try
            {
                cmdRT10492.InfoParameters[0].Value = sdata[0];
                cmdRT10492.InfoParameters[1].Value = sdata[1];
                cmdRT10492.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10492.ExecuteNonQuery();
                return new object[] { 0, "用戶調整到期日數資料結案返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶調整到期日數資料結案返轉作業,錯誤訊息" + ex };
            }
        }

        //拆機派工單作廢
        public object[] smRT10493(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10493.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCustADJDAY WHERE CUSID='" + sdata[0] + "' and ENTRYNO=" + sdata[1];
            
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["ADJCLOSEDAT"].ToString() != "" || RSXX.Tables[0].Rows[0]["ADJCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此用戶調整到期日數資料已結案，不可作廢。(請改用結案返轉)" };
                }

                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "此用戶調整到期日數資料已作廢，不可重覆執行。" };
                }
            }
            
            //設定輸入參數的值
            try
            {
                cmdRT10493.InfoParameters[0].Value = sdata[0];
                cmdRT10493.InfoParameters[1].Value = sdata[1];
                cmdRT10493.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10493.ExecuteNonQuery();
                return new object[] { 0, "用戶調整到期日數資料作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶調整到期日數資料作廢作業,錯誤訊息：" + ex };
            }
        }

        //拆機派工單作廢返轉
        public object[] smRT10494(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10494.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCustADJDAY WHERE CUSID='" + sdata[0] + "' and ENTRYNO=" + sdata[1];
            string sqlYY = "select count(*) as cnt FROM RTLessorAVSCustADJDAY WHERE CUSID='" + sdata[0] + "' and ENTRYNO >" + sdata[1] + " and canceldat is null ";
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            DataSet RSYY = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() == "")
                {
                    return new object[] { 0, "此用戶調整到期日數資料尚未作廢，不可返轉。" };
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
                cmdRT10494.InfoParameters[0].Value = sdata[0];
                cmdRT10494.InfoParameters[1].Value = sdata[1];
                cmdRT10494.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10494.ExecuteNonQuery();
                return new object[] { 0, "用戶調整到期日數資料作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶調整到期日數資料作廢作業,錯誤訊息：" + ex };
            }
        }
    }
}
