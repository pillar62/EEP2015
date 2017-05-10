using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT203
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
            return string.Format("EL{0:yyMMdd}", DateTime.Now.Date);
        }

        //完工結案
        public object[] smRT2031(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            var xxadslapplydat = "";
            var xxCONTAPPLYDAT = "";
            //開啟資料連接
            IDbConnection conn = cmdRT2031.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCmtyline WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1];
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                xxadslapplydat = ds.Tables[0].Rows[0]["ADSLAPPLYDAT"].ToString();
                xxCONTAPPLYDAT = ds.Tables[0].Rows[0]["DROPDAT"].ToString();
                if (ds.Tables[0].Rows[0]["DROPDAT"].ToString() != "" || ds.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "主線已退租或作廢，無法結案(派工單必須作廢)。" };
                }                
            }
            else
            {
                return new object[] { 0, "找不到主線基本檔，無法結案。" };
            }

            selectSql = " select count(*) as CNT FROM RTLessorAVScmtylineHardware "
                      + " WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and prtno = '" + sdata[2] + "' and dropdat is null and rcvfinishdat is null ";
            cmd.CommandText = selectSql;
            ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["CNT"].ToString()) > 0)
                {
                    return new object[] { 0, "此主線派工單設備資料中，尚有設備未辦妥物品領用程序，不可執行完工結案作業。" };
                }
            }

            selectSql = " select * FROM RTLessorAVSCmtylineSndwork "
                      + " WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and prtno = '" + sdata[2] + "' ";
            cmd.CommandText = selectSql;
            ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "當已作廢時，不可執行完工結案或未完工結案。" };
                }
                if (ds.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" || ds.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此主線派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案。" };
                }
                if (ds.Tables[0].Rows[0]["REALENGINEER"].ToString() == "" && ds.Tables[0].Rows[0]["REALCONSIGNEE"].ToString() == "")
                {
                    return new object[] { 0, "此主線派工單完工時，必須先輸入實際裝機人員或實際裝機經銷商。" };
                }
                if ((ds.Tables[0].Rows[0]["EQUIPSETUPDAT"].ToString() == "" || ds.Tables[0].Rows[0]["ADSLAPPLYDAT"].ToString() == "") && ds.Tables[0].Rows[0]["SNDKIND"].ToString() == "ST")
                {
                    return new object[] { 0, "標準工程結案時，設備安裝到位日及主線測通日不可空白。" };
                }
                if ((ds.Tables[0].Rows[0]["EQUIPSETUPDAT"].ToString() != "" || ds.Tables[0].Rows[0]["ADSLAPPLYDAT"].ToString() != "") && ds.Tables[0].Rows[0]["SNDKIND"].ToString() != "ST")
                {
                    return new object[] { 0, "非標準工程結案時，設備安裝到位日及主線測通日必須空白。" };
                }
                if ((xxadslapplydat != "" || xxCONTAPPLYDAT != "") && ds.Tables[0].Rows[0]["SNDKIND"].ToString() == "ST")
                {
                    return new object[] { 0, "此主線已測通，不可再派[主線測通]的派工單。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT2031.InfoParameters[0].Value = sdata[0];
                cmdRT2031.InfoParameters[1].Value = sdata[1];
                cmdRT2031.InfoParameters[2].Value = sdata[2];
                cmdRT2031.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT2031.ExecuteNonQuery();
                return new object[] { 0, "主線派工單完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線派工單完工結案作業,錯誤訊息" + ex };
            }
        }

        //未完工結案
        public object[] smRT2032(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT2032.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCmtylinesndwork WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and prtno = '" + sdata[2] + "'";            
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["DROPDAT"].ToString();
                string ss2 = "";
                if (ss1 != "")
                {
                    return new object[] { 0, "當已作廢時，不可執行完工結案或未完工結案" };
                }
                ss1 = ds.Tables[0].Rows[0]["CLOSEDAT"].ToString();
                ss2 = ds.Tables[0].Rows[0]["UNCLOSEDAT"].ToString();
                if (ss1 != "" || ss2 != "")
                {
                    return new object[] { 0, "此主線派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案。" };
                }                
            }

            selectSql = "select count(*) as cnt FROM RTLessorAVSCmtylinehardware WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and prtno = '" + sdata[2] + "' and dropdat is null and RCVPRTNO <> '' ";
            cmd.CommandText = selectSql;
            ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["cnt"].ToString()) > 0)
                {
                    return new object[] { 0, "此主線派工單已產生物品領用單，請先返轉領用單才能執行未完工結案作業。" };
                }
            }
            try
            {
                cmdRT2032.InfoParameters[0].Value = sdata[0];
                cmdRT2032.InfoParameters[1].Value = sdata[1];
                cmdRT2032.InfoParameters[2].Value = sdata[2];
                cmdRT2032.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT2032.ExecuteNonQuery();
                return new object[] { 0, "主線派工單未完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線派工單未完工結案作業,錯誤訊息" + ex };
            }
        }

        //結案返轉
        public object[] smRT2033(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT2033.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtylinesndwork WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and prtno = '" + sdata[2] + "'";
            string sqlYY = "select * FROM RTLessorAVSCMTYLINE WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1];
            string sqlZZ = "select COUNT(*) AS CNT FROM RTLessorAVSCUST WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND canceldat is null and dropdat is null and finishdat is not null ";
            cmd.CommandText = sqlxx;
            DataSet RSxx = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            DataSet RSYY = cmd.ExecuteDataSet();
            cmd.CommandText = sqlZZ;
            DataSet RSzz = cmd.ExecuteDataSet();

            if (RSxx.Tables[0].Rows.Count > 0)
            {
                if (RSxx.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "此主線派工單已作廢，不可執行結案返轉作業。" };
                }

                if (RSxx.Tables[0].Rows[0]["CLOSEDAT"].ToString() == "" && RSxx.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() == "")
                {
                    return new object[] { 0, "此主線派工單尚未結案，不可執行結案返轉作業。" };
                }


            }

            if (RSYY.Tables[0].Rows.Count > 0)
            {
                if (RSxx.Tables[0].Rows[0]["SNDKIND"].ToString() == "ST" && RSYY.Tables[0].Rows[0]["ADSLAPPLYDAT"].ToString() == ""  && RSYY.Tables[0].Rows[0]["CONTAPPLYDAT"].ToString() == "" )
                {
                    return new object[] { 0, "主線檔目前的狀態並非[已測通]，此派工單種類為[標準工程]之返轉，因此無法執行。" };
                }
            }
            else
            {
                return new object[] { 0, "無法找到此主線派工單之主檔資料，請確認AVS-City主線主檔資料正常。" };
            }

            if (RSzz.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(RSzz.Tables[0].Rows[0]["CNT"].ToString()) > 0 && RSxx.Tables[0].Rows[0]["SNDKIND"].ToString() == "ST")
                {
                    return new object[] { 0, "此派工單所屬主線已有已完工之用戶，因此不能執行[標準工程]返轉作業。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT2033.InfoParameters[0].Value = sdata[0];
                cmdRT2033.InfoParameters[1].Value = sdata[1];
                cmdRT2033.InfoParameters[2].Value = sdata[2];
                cmdRT2033.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT2033.ExecuteNonQuery();
                return new object[] { 0, "主線派工單完工/未完工結案返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線派工單完工結案返轉作業,錯誤訊息" + ex };
            }
        }

        //作廢
        public object[] smRT2034(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT2034.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtylinesndwork WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and prtno = '" + sdata[2] + "'";
            string sqlYY = "select * FROM RTLessorAVSCustRCVHardware WHERE prtno='" + sdata[2] + "' and canceldat is null ";
            cmd.CommandText = sqlxx;
            DataSet RSxx = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            DataSet RSYY = cmd.ExecuteDataSet();

            if (RSxx.Tables[0].Rows.Count > 0)
            {
                if (RSxx.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" || RSxx.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此派工單已完工結案，不可作廢(欲作廢請先清除裝機完工日)。" };
                }

                if (RSxx.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "此派工單已作廢，不可重覆執行作廢作業。" };
                }
            }

            if (RSYY.Tables[0].Rows.Count > 0)
            {
                return new object[] { 0, "此派工單已產生物品領用單，不可直接作廢(請先返轉物品領用單)。" };
            }

            //設定輸入參數的值
            try
            {
                cmdRT2034.InfoParameters[0].Value = sdata[0];
                cmdRT2034.InfoParameters[1].Value = sdata[1];
                cmdRT2034.InfoParameters[2].Value = sdata[2];
                cmdRT2034.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT2034.ExecuteNonQuery();
                return new object[] { 0, "主線派工單作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線派工單作廢作業,錯誤訊息：" + ex };
            }
        }

        //拆機派工單作廢返轉
        public object[] smRT2035(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT2035.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVScmtylinesndwork WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and prtno = '" + sdata[2] + "'";
            cmd.CommandText = sqlxx;
            DataSet RSxx = cmd.ExecuteDataSet();

            if (RSxx.Tables[0].Rows.Count > 0)
            {
                if (RSxx.Tables[0].Rows[0]["BONUSCLOSEYM"].ToString() != "")
                {
                    return new object[] { 0, "當獎金計算年月已存在資料時表示該筆資料完工日期當月之獎金已結算,不可再作廢返轉。" };
                }

                if (RSxx.Tables[0].Rows[0]["DROPDAT"].ToString() == "")
                {
                    return new object[] { 0, "此主線派工單尚未作廢，不可執行作廢返轉作業。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT2035.InfoParameters[0].Value = sdata[0];
                cmdRT2035.InfoParameters[1].Value = sdata[1];
                cmdRT2035.InfoParameters[2].Value = sdata[2];
                cmdRT2035.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT2035.ExecuteNonQuery();
                return new object[] { 0, "主線派工單作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線派工單作廢返轉作業,錯誤訊息：" + ex };
            }
        }
    }
}
