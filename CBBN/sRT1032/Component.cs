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
                cmdRT10321.InfoParameters[2].Value = sdata[3];
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
                cmdRT10322.InfoParameters[2].Value = sdata[3];
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
    }
}
