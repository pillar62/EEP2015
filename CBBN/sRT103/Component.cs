using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT103
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
            //預設值要帶入社區編號
            return ucRTLessorAVSCmtyLine.GetFieldCurrentValue("COMQ1").ToString();

        }

        //作　　廢
        public object[] smRT1031(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');

            //開啟資料連接
            IDbConnection conn = cmdRT1031.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCMTYLINE WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();
            string sqlyy = "select COUNT(*) AS CNT FROM RTLessorAVSCUST WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1] + " AND CANCELDAT IS NULL AND DROPDAT IS NULL AND (STRBILLINGDAT IS NOT NULL OR NEWBILLINGDAT IS NOT NULL OR DOCKETDAT IS NOT NULL OR FINISHDAT IS NOT NULL ) ";
            cmd.CommandText = sqlyy;
            DataSet RSyy = cmd.ExecuteDataSet();
            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["APPLYDAT"].ToString() != "")
                {
                    return new object[] { 0, "線路已提出申請，不可作廢(請清除申請日)。" };
                }

                if (RSXX.Tables[0].Rows[0]["CANCELdat"].ToString() != "")
                {
                    return new object[] { 0, "線路已作廢，不可重複作廢。" };
                }

                if (RSXX.Tables[0].Rows[0]["DROPDAT"].ToString() != "")
                {
                    return new object[] { 0, "線路已撤線，不須再作廢。" };
                }

                if (RSXX.Tables[0].Rows[0]["ADSLAPPLYDAT"].ToString() != "")
                {
                    return new object[] { 0, "線路已測通，不可作廢。" };
                }
            }

            if (RSyy.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(RSyy.Tables[0].Rows[0]["CNT"].ToString()) > 0)
                {
                    return new object[] { 0, "此線路有尚未退租的用戶，不可直接作廢主線。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT1031.InfoParameters[0].Value = sdata[0];
                cmdRT1031.InfoParameters[1].Value = sdata[1];
                cmdRT1031.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT1031.ExecuteNonQuery();
                return new object[] { 0, "主線作廢成功。" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶申請作廢作業,錯誤訊息" + ex };
            }
        }

        //主線續約作廢返轉
        public object[] smRT1032(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT1032.Connection;
            conn.Open();
            string sqlxx = "select * FROM RTLessorAVSCMTYLINE WHERE comq1=" + sdata[0] + " and lineq1=" + sdata[1];
            cmd.CommandText = sqlxx;
            DataSet RSXX = cmd.ExecuteDataSet();

            if (RSXX.Tables[0].Rows.Count > 0)
            {
                if (RSXX.Tables[0].Rows[0]["CANCELDAT"].ToString() == "")
                {
                    return new object[] { 0, "線路尚未作廢，不可執行作廢返轉。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT1032.InfoParameters[0].Value = sdata[0];
                cmdRT1032.InfoParameters[1].Value = sdata[1];
                cmdRT1032.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT1032.ExecuteNonQuery();
                return new object[] { 0, "主線作廢返轉成功  " };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶申請作廢返轉作業,錯誤訊息：" + ex };
            }
        }
    }
}
