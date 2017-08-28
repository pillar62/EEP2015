using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT2033
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

        //轉領用單
        public object[] smRT20331(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            var xxadslapplydat = "";
            var xxCONTAPPLYDAT = "";
            //開啟資料連接
            IDbConnection conn = cmdRT20331.Connection;
            conn.Open();
            //檢查該派工單是否已結案或作廢或已無可轉設備項目
            string selectSql = "select * FROM RTLessorAVScmtylineSndwork WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND PRTNO='"+ sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" || ds.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "派工單已完工結案或未完工結案，不可轉物品領用單。" };
                }
                if (ds.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "派工單已作廢，不可轉物品領用單" };
                }
                if (ds.Tables[0].Rows[0]["CDAT"].ToString() != "" || ds.Tables[0].Rows[0]["BATCHNO"].ToString() != "")
                {
                    return new object[] { 0, "派工單已產生應收帳款，不可轉物品領用單。" };
                }
            }
            else
            {
                return new object[] { 0, "找不到派工單檔，無法結案。" };
            }

            selectSql = "select count(*) as CNT FROM RTLessorAVScmtylineHardware WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND PRTNO='" + sdata[2] + "' AND dropdat is null and rcvprtno='' and rcvfinishdat is null and batchno='' and qty > 0";
            cmd.CommandText = selectSql;
            ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CNT"].ToString() == "0")
                {
                    return new object[] { 0, "派工單已無其它設備可轉物品領用單" };
                }
            }


            //設定輸入參數的值
            try
            {
                cmdRT20331.InfoParameters[0].Value = sdata[0];
                cmdRT20331.InfoParameters[1].Value = sdata[1];
                cmdRT20331.InfoParameters[2].Value = sdata[2];
                cmdRT20331.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT20331.ExecuteNonQuery();
                return new object[] { 0, "主線派工設備轉物品領用單作業成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線派工設備轉物品領用單作業,錯誤訊息" + ex };
            }
        }

        //領用單返轉
        public object[] smRT20332(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT20332.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVScmtylineHardware WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and prtno = '" + sdata[2] + "' AND SEQ="+ sdata[3];
            cmd.CommandText = selectSql;
            var XXRCVPRTNO = "";
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "此設備已作廢，不可返轉物品領用單" };
                }
                if (ds.Tables[0].Rows[0]["RCVPRTNO"].ToString() == "")
                {
                    return new object[] { 0, "此設備尚未轉物品領用單，不可返轉。" };
                }
                if (ds.Tables[0].Rows[0]["RCVFINISHDAT"].ToString() != "")
                {
                    return new object[] { 0, "此設備之物品領用單已經結案，不可返轉(欲返轉請先取消物品領用單結案作業)" };
                }

                 XXRCVPRTNO = ds.Tables[0].Rows[0]["RCVPRTNO"].ToString();
            }
            else
            {
                return new object[] { 0, "此設備之設備檔資料不存在，不可返轉(請通知資訊部)。" };
            }
            
            try
            {
                cmdRT20332.InfoParameters[0].Value = XXRCVPRTNO;
                cmdRT20332.InfoParameters[1].Value = sdata[4];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT20332.ExecuteNonQuery();
                return new object[] { 0, "主線派工設備返轉物品領用單作業成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線派工設備返轉物品領用單作業,錯誤訊息" + ex };
            }
        }

        //設備作廢
        public object[] smRT20333(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT20333.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCmtyLineHardware WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and prtno = '" + sdata[2] + "' AND SEQ=" + sdata[3];
            string sqlYY = "select * FROM RTLessorAVSCmtyLinesndwork WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and prtno = '" + sdata[2] + "'";
            cmd.CommandText = sqlxx;
            DataSet RSxx = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            DataSet RSYY = cmd.ExecuteDataSet();

            if (RSxx.Tables[0].Rows.Count > 0)
            {
                if (RSxx.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "此筆設備已作廢，不可重覆作廢。" };
                }

                if (RSxx.Tables[0].Rows[0]["BATCHNO"].ToString() != "")
                {
                    return new object[] { 0, "已轉應收帳款，不可作廢(欲作廢必須直接作廢派工單)。" };
                }

                if (RSxx.Tables[0].Rows[0]["RCVPRTNO"].ToString() != "")
                {
                    return new object[] { 0, "已轉物品領用單，不可作廢(欲作廢請先返轉物品領用單)。" };
                }
            }
            

            if (RSYY.Tables[0].Rows.Count > 0)
            {
                if (RSYY.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" && RSYY.Tables[0].Rows[0]["UNCLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "所屬派工單已結案，不可作廢設備明細。" };
                }
            }
            

            //設定輸入參數的值
            try
            {
                cmdRT20333.InfoParameters[0].Value = sdata[0];
                cmdRT20333.InfoParameters[1].Value = sdata[1];
                cmdRT20333.InfoParameters[2].Value = sdata[2];
                cmdRT20333.InfoParameters[3].Value = sdata[3];
                cmdRT20333.InfoParameters[3].Value = sdata[4];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT20333.ExecuteNonQuery();
                return new object[] { 0, "主線設備安裝資料作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行設備安裝資料作廢,錯誤訊息：" + ex };
            }
        }

        //作廢返轉
        public object[] smRT20334(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT20334.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCustReturnhardware WHERE cusid=" + sdata[0] + " and entryno=" + sdata[1] + " and prtno = '" + sdata[2] + "' and seq = " + sdata[3];
            string sqlYY = "select * FROM RTLessorAVSCmtyLinesndwork WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " and prtno='" + sdata[2] + "' ";
            cmd.CommandText = sqlxx;
            DataSet RSxx = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            DataSet RSYY = cmd.ExecuteDataSet();

            if (RSxx.Tables[0].Rows.Count > 0)
            {
                if (RSxx.Tables[0].Rows[0]["DROPDAT"].ToString() == "")
                {
                    return new object[] { 0, "此筆設備尚未作廢，不可作廢返轉。" };
                }
            }

            if (RSYY.Tables[0].Rows.Count > 0)
            {
                if (RSYY.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "" || RSYY.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "所屬派工單已結案，不可作廢設備明細。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT20334.InfoParameters[0].Value = sdata[0];
                cmdRT20334.InfoParameters[1].Value = sdata[1];
                cmdRT20334.InfoParameters[2].Value = sdata[2];
                cmdRT20334.InfoParameters[3].Value = sdata[3];
                cmdRT20334.InfoParameters[4].Value = sdata[4];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT20334.ExecuteNonQuery();
                return new object[] { 0, "設備安裝資料作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行設備安裝資料作廢返轉,錯誤訊息：" + ex };
            }
        }
    }
}
