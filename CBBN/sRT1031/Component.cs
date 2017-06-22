using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT1031
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

        //續約結案
        public object[] smRT10311(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT10312.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCMTYLINEcont WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND entryno=" + sdata[2];
            string sqlYY = "select * FROM RTLessorAVSCMTYLINE WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            DataSet RSYY = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "線路續約已作廢，不可結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "續約資料已結案，不可重複結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["ADSLAPPLYDAT"].ToString() == "")
                {
                    return new object[] { 0, "續約資料無測通日，不可結案。" };
                }

                if (RSXX.Tables[0].Rows[0]["LINEDUEDAT"].ToString() == "")
                {
                    return new object[] { 0, "續約資料無到期日，不可結案。" };
                }

                if (RSYY.Tables[0].Rows.Count > 0)
                {
                    if (RSYY.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                    {
                        return new object[] { 0, "此主線之主檔資料已撤線，不可執行續約結案作業。" };
                    }

                    if (RSYY.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                    {
                        return new object[] { 0, "此主線之主檔資料已作廢，不可執行續約結案作業。" };
                    }

                    if (RSYY.Tables[0].Rows[0]["ADSLAPPLYDAT"].ToString() == "")
                    {
                        return new object[] { 0, "此主線之主檔資料尚未測通，不可執行續約結案作業。" };
                    }
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10311.InfoParameters[0].Value = sdata[0];
                cmdRT10311.InfoParameters[1].Value = sdata[1];
                cmdRT10311.InfoParameters[2].Value = sdata[2];
                cmdRT10311.InfoParameters[2].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10311.ExecuteNonQuery();
                return new object[] { 0, "主線續約結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線續約結案作業,錯誤訊息" + ex };
            }
        }

        //作　　廢
        public object[] smRT10312(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT10312.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCMTYLINEcont WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND entryno=" + sdata[2];
            string sqlYY = "select * FROM RTLessorAVSCMTYLINE WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            cmd.CommandText = sqlYY;
            DataSet RSYY = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "續約資料已作廢，不可重複作廢。" };
                }

                if (RSXX.Tables[0].Rows[0]["CLOSEDAT"].ToString() != "")
                {
                    return new object[] { 0, "續約資料已結案，不可作廢。" };
                }

                if (RSXX.Tables[0].Rows[0]["ADSLAPPLYDAT"].ToString() != "")
                {
                    return new object[] { 0, "線路續約已測通，不可作廢(請清除測通日)。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10312.InfoParameters[0].Value = sdata[0];
                cmdRT10312.InfoParameters[1].Value = sdata[1];
                cmdRT10312.InfoParameters[2].Value = sdata[2];
                cmdRT10312.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10312.ExecuteNonQuery();
                return new object[] { 0, "主線續約作廢成功。" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線續約作廢作業,錯誤訊息" + ex };
            }
        }

        //主線續約作廢返轉
        public object[] smRT10313(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10313.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCMTYLINEcont WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND entryno=" + sdata[2];

            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() == "")
                {
                    return new object[] { 0, "此續約資料尚未作廢，不可執行作廢返轉。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT10313.InfoParameters[0].Value = sdata[0];
                cmdRT10313.InfoParameters[1].Value = sdata[1];
                cmdRT10313.InfoParameters[2].Value = sdata[2];
                cmdRT10313.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10313.ExecuteNonQuery();
                return new object[] { 0, "主線續約作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行續約申請作廢返轉作業,錯誤訊息：" + ex };
            }
        }
    }
}
