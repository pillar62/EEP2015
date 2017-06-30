using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT1032
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

        //轉拆機單
        public object[] smRT10321(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT10321.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCMTYLINEdrop WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND entryno=" + sdata[2];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "當已作廢時，不可轉拆機派工單。" };
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "已結案時，不可轉拆機派工單。" };
                }

                if (RSXX.Tables[0].Rows[0]["SNDPRTNO"].ToString() != "")
                {
                    return new object[] { 0, "已有拆機派工單時，不可重覆執行。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10321.InfoParameters[0].Value = sdata[0];
                cmdRT10321.InfoParameters[1].Value = sdata[1];
                cmdRT10321.InfoParameters[2].Value = sdata[2];
                cmdRT10321.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10321.ExecuteNonQuery();
                return new object[] { 0, "主線撤線單轉拆機派工單成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行撤線單轉拆機派工單作業,錯誤訊息" + ex };
            }
        }

        //撤線結案
        public object[] smRT10322(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT10322.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtylinedrop WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND entryno=" + sdata[2];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "當撤線資料已作廢時，不可結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "當撤線資料已結案時，不可重覆結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["SNDPRTNO"].ToString() == ""|| RSXX.Tables[0].Rows[0]["SNDWORKDAT"].ToString() == "")
                {
                    return new object[] { 0, "當撤線資料尚未產生拆機派工單時，不可結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["SNDPRTNO"].ToString() != "" || RSXX.Tables[0].Rows[0]["SNDCLOSEDAT"].ToString() == "")
                {
                    return new object[] { 0, "當撤線資料之拆機派工單尚未結案時，不可結案。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10322.InfoParameters[0].Value = sdata[0];
                cmdRT10322.InfoParameters[1].Value = sdata[1];
                cmdRT10322.InfoParameters[2].Value = sdata[2];
                cmdRT10322.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10322.ExecuteNonQuery();
                return new object[] { 0, "主線撤線完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線撤線完工結案作業,錯誤訊息" + ex };
            }
        }

        //作　　廢
        public object[] smRT10323(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT10323.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtylinedrop WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND entryno=" + sdata[2];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "當撤線資料已作廢時，不可重複作廢。" };
                }

                if (RSXX.Tables[0].Rows[0]["SNDPRTNO"].ToString() != "")
                {
                    return new object[] { 0, "當撤線資料已產生拆機派工單時，不可作廢。" };
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "當撤線資料已結案時，不可作廢。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10323.InfoParameters[0].Value = sdata[0];
                cmdRT10323.InfoParameters[1].Value = sdata[1];
                cmdRT10323.InfoParameters[2].Value = sdata[2];
                cmdRT10323.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10323.ExecuteNonQuery();
                return new object[] { 0, "主線撤線資料作廢成功。" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線撤線資料作廢作業,錯誤訊息" + ex };
            }
        }

        //主線續約作廢返轉
        public object[] smRT10324(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10324.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtylinedrop WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND entryno=" + sdata[2];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() == "")
                {
                    return new object[] { 0, "此主線撤線資料尚未作廢，不可返轉。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10324.InfoParameters[0].Value = sdata[0];
                cmdRT10324.InfoParameters[1].Value = sdata[1];
                cmdRT10324.InfoParameters[2].Value = sdata[2];
                cmdRT10324.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10324.ExecuteNonQuery();
                return new object[] { 0, "主線撤線資料作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線撤線資料作廢返轉作業,錯誤訊息：" + ex };
            }
        }

        //主線撤線派工單完工結案
        public object[] smRT103213(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT103213.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCMTYLINE WHERE COMQ1=" + sdata[0] + " AND LINEQ1=" + sdata[1];
            string sqlyy = "select * FROM RTLessorAVSCmtyLineDROP WHERE COMQ1=" + sdata[0] + " AND LINEQ1=" + sdata[1] + " and ENTRYNO=" + sdata[2];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlyy;
            DataSet RSYY = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "主線已作廢，無法結案(派工單必須作廢)。" };
                }
            }
            else
            {
                return new object[] { 0, "找不到主線基本檔，無法結案。" };
            }

            if (RSYY.Tables[0].Rows.Count > 0)
            {
                if (RSYY.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "此撤線派工單所屬撤線單資料已作廢，不可執行完工結案作業。" };
                }
            }
            else
            {
                return new object[] { 0, "找不到此撤線派工單所屬撤線單資料。" };

            }

            sqlxx = "select count(*) as CNT FROM RTLessorAVSCMTYLINEDROPHardware WHERE COMQ1=" + sdata[0] + " AND LINEQ1=" + sdata[1]
                  + " AND ENTRYNO=" + sdata[2] + " and prtno='" + sdata[3] + "' and dropdat is null and rcvprtno <> '' ";
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(RSXX.Tables[0].Rows[0]["CNT"].ToString()) > 0)
                {
                    return new object[] { 0, "此撤線派工單已產生物品移轉單，請先返轉移轉單才能執行未完工結案作業。。" };
                }
            }

            sqlxx = "select * FROM RTLessorAVSCmtylineDROPsndwork WHERE COMQ1=" + sdata[0] + " AND LINEQ1=" + sdata[1] + " AND ENTRYNO=" + sdata[2] + " and prtno='" + sdata[3] + "' ";

            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "當已作廢時，不可執行完工結案或未完工結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" || RSYY.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此撤線派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["REALENGINEER"].ToString() == "" && RSYY.Tables[0].Rows[0]["REALCONSIGNEE"].ToString() == "")
                {
                    return new object[] { 0, "此撤線派工單完工時，必須先輸入實際裝機人員或實際裝機經銷商。" };
                }

                if (RSXX.Tables[0].Rows[0]["BONUSCLOSEYM"].ToString() != "" || RSYY.Tables[0].Rows[0]["STOCKCLOSEYM"].ToString() != "")
                {
                    return new object[] { 0, "此撤線派工單已月結，不可異動。" };
                }

                if (RSXX.Tables[0].Rows[0]["BATCHNO"].ToString() != "")
                {
                    return new object[] { 0, "此撤線派工單已產生應收帳款，無法重複結案，請連絡資訊部。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT103213.InfoParameters[0].Value = sdata[0];
                cmdRT103213.InfoParameters[1].Value = sdata[1];
                cmdRT103213.InfoParameters[2].Value = sdata[2];
                cmdRT103213.InfoParameters[3].Value = sdata[3];
                cmdRT103213.InfoParameters[4].Value = sdata[4];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT103213.ExecuteNonQuery();
                return new object[] { 0, "主線撤線派工單完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行撤線派工單完工結案作業,錯誤訊息" + ex };
            }
        }

        //撤線結案
        public object[] smRT103214(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT103214.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCMTYLINE WHERE COMQ1=" + sdata[0] + " AND LINEQ1=" + sdata[1];
            string sqlyy = "select * FROM RTLessorAVSCmtyLineDROP WHERE COMQ1=" + sdata[0] + " AND LINEQ1=" + sdata[1] + " and ENTRYNO=" + sdata[2];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlyy;
            DataSet RSYY = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "主線已作廢，無法結案(派工單必須作廢)。" };
                }
            }
            else
            {
                return new object[] { 0, "找不到主線基本檔，無法結案。" };
            }

            if (RSYY.Tables[0].Rows.Count > 0)
            {
                if (RSYY.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "此撤線派工單所屬撤線單資料已作廢，不可執行完工結案作業。" };
                }
            }
            else
            {
                return new object[] { 0, "找不到此撤線派工單所屬撤線單資料。" };

            }

            sqlxx = "select count(*) as CNT FROM RTLessorAVSCMTYLINEDROPHardware WHERE COMQ1=" + sdata[0] + " AND LINEQ1=" + sdata[1]
                  + " AND ENTRYNO=" + sdata[2] + " and prtno='" + sdata[3] + "' and dropdat is null and rcvfinishdat is null ";
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(RSXX.Tables[0].Rows[0]["CNT"].ToString()) > 0)
                {
                    return new object[] { 0, "此撤線派工單設備資料中，尚有設備未辦妥物品移轉程序(未移轉或移轉未結案)，不可執行完工結案作業。" };
                }
            }

            sqlxx = "select * FROM RTLessorAVSCmtylineDROPsndwork WHERE COMQ1=" + sdata[0] + " AND LINEQ1=" + sdata[1] + " AND ENTRYNO=" + sdata[2] + " and prtno='" + sdata[3] + "' ";

            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "當已作廢時，不可執行完工結案或未完工結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" || RSYY.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此撤線派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["BATCHNO"].ToString() != "")
                {
                    return new object[] { 0, "此撤線派工單已產生應收帳款，無法重複結案，請連絡資訊部。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT103214.InfoParameters[0].Value = sdata[0];
                cmdRT103214.InfoParameters[1].Value = sdata[1];
                cmdRT103214.InfoParameters[2].Value = sdata[2];
                cmdRT103214.InfoParameters[3].Value = sdata[3];
                cmdRT103214.InfoParameters[4].Value = sdata[4];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT103214.ExecuteNonQuery();
                return new object[] { 0, "主線撤線派工單未完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行撤線派工單未完工結案作業,錯誤訊息" + ex };
            }
        }

        //結案返轉
        public object[] smRT103215(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT103215.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCMTYLINE WHERE COMQ1=" + sdata[0] + " AND LINEQ1=" + sdata[1];
            string sqlyy = "select * FROM RTLessorAVSCmtyLineDROP WHERE COMQ1=" + sdata[0] + " AND LINEQ1=" + sdata[1] + " and ENTRYNO=" + sdata[2];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlyy;
            DataSet RSYY = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "主線已作廢，無法結案(派工單必須作廢)。" };
                }

                if (RSXX.Tables[0].Rows[0]["DROPDAT"].ToString() == "")
                {
                    return new object[] { 0, "主線尚未撤線，無法執行派工單結案返轉作業。" };
                }
            }
            else
            {
                return new object[] { 0, "找不到主線基本檔，無法結案。" };
            }

            if (RSYY.Tables[0].Rows.Count > 0)
            {
                if (RSYY.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "撤線單已結案，不可執行派工單結案返轉。" };
                }

                if (RSYY.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "此撤線派工單所屬撤線單資料已作廢，不可執行完工結案作業。" };
                }
            }
            else
            {
                return new object[] { 0, "找不到此撤線派工單所屬撤線單資料。" };

            }

            sqlxx = "select count(*) as CNT FROM RTLessorAVSCMTYLINEDROPHardware WHERE COMQ1=" + sdata[0] + " AND LINEQ1=" + sdata[1]
                  + " AND ENTRYNO=" + sdata[2] + " and prtno='" + sdata[3] + "' and dropdat is null and rcvfinishdat is null ";
            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(RSXX.Tables[0].Rows[0]["CNT"].ToString()) > 0)
                {
                    return new object[] { 0, "此撤線派工單設備資料中，尚有設備未辦妥物品移轉程序(未移轉或移轉未結案)，不可執行完工結案作業。" };
                }
            }

            sqlxx = "select * FROM RTLessorAVSCmtylineDROPsndwork WHERE COMQ1=" + sdata[0] + " AND LINEQ1=" + sdata[1] + " AND ENTRYNO=" + sdata[2] + " and prtno='" + sdata[3] + "' ";

            cmd.CommandText = sqlxx;
            RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() == "" || RSYY.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() == "")
                {
                    return new object[] { 0, "此派工單尚未結案，不可執行結案返轉作業。" };
                }

                if (RSXX.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "此撤線派工單已作廢，不可返轉。" };
                }

                if ((RSYY.Tables[0].Rows[0]["SNDWORKDAT"].ToString() != "" || RSYY.Tables[0].Rows[0]["SNDPRTNO"].ToString() != "") && RSYY.Tables[0].Rows[0]["SNDWORKDAT"].ToString() != "")
                {
                    return new object[] { 0, "此撤線派工單所屬撤線單資料已作廢，不可執行完工結案作業。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT103215.InfoParameters[0].Value = sdata[0];
                cmdRT103215.InfoParameters[1].Value = sdata[1];
                cmdRT103215.InfoParameters[2].Value = sdata[2];
                cmdRT103215.InfoParameters[3].Value = sdata[3];
                cmdRT103215.InfoParameters[4].Value = sdata[4];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT103215.ExecuteNonQuery();
                return new object[] { 0, "主線撤線完工結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線撤線完工結案作業,錯誤訊息" + ex };
            }
        }

        //主線撤線派工作廢
        public object[] smRT103216(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT103216.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtyLineDROPsndwork WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and entryno=" + sdata[2] + " and prtno='" + sdata[3] + "' ";
            string sqlYY = "select * FROM RTLessorAVSCmtyLineDROP WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and entryno=" + sdata[2];
            string sqlzz = "select count(*) as cnt FROM RTLessorAVSCustRTNHardware WHERE prtno='" + sdata[3] + "' and canceldat is null ";
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            DataSet RSYY = cmd.ExecuteDataSet();
            cmd.CommandText = sqlzz;
            DataSet RSZZ = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" || RSYY.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "")
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
                if (RSYY.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "此派工單所屬撤線單已結案，不可作廢派工單。" };
                }
            }

            if (RSZZ.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(RSZZ.Tables[0].Rows[0]["CNT"].ToString()) > 0)
                {
                    return new object[] { 0, "此派工單已產生物品領用單，不可直接作廢(請先返轉物品領用單)。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT103216.InfoParameters[0].Value = sdata[0];
                cmdRT103216.InfoParameters[1].Value = sdata[1];
                cmdRT103216.InfoParameters[2].Value = sdata[2];
                cmdRT103216.InfoParameters[3].Value = sdata[3];
                cmdRT103216.InfoParameters[4].Value = sdata[4];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT103216.ExecuteNonQuery();
                return new object[] { 0, "主線撤線派工單作廢成功。" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行派工單作廢作業,錯誤訊息" + ex };
            }
        }

        //主線續約派工作廢返轉
        public object[] smRT103217(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT103217.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtyLinedropsndwork WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and entryno=" + sdata[2] + " and prtno='" + sdata[3] + "' ";
            string sqlYY = "select * FROM RTLessorAVSCmtyLinedrop WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and entryno=" + sdata[2];

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
                    return new object[] { 0, "此主線撤線派工單尚未作廢，不可執行作廢返轉作業。" };
                }
            }

            if (RSYY.Tables[0].Rows.Count > 0)
            {
                if (RSYY.Tables[0].Rows[0]["SNDPRTNO"].ToString() != "" || RSYY.Tables[0].Rows[0]["SNDWORKDAT"].ToString() != "")
                {
                    return new object[] { 0, "此派工單所屬撤線單已另外產生派工單，因此不能執行派工單作廢返轉。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT103217.InfoParameters[0].Value = sdata[0];
                cmdRT103217.InfoParameters[1].Value = sdata[1];
                cmdRT103217.InfoParameters[2].Value = sdata[2];
                cmdRT103217.InfoParameters[3].Value = sdata[3];
                cmdRT103217.InfoParameters[4].Value = sdata[4];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT103217.ExecuteNonQuery();
                return new object[] { 0, "主線撤線派工單作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線派工單作廢返轉作業,錯誤訊息：" + ex };
            }
        }
    }
}
