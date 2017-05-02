using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT104
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
            return string.Format("E{0:yyMMdd}", DateTime.Now.Date);
        }

        //用戶作廢
        public object[] smRT104B(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT104B.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUST WHERE comq1=" + sdata[0] + " and lineq1 = "+ sdata[1] + " and CUSID ='" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {                
                if (ds.Tables[0].Rows[0]["FINISHDAT"].ToString() != "")
                {
                    return new object[] { 0, "此用戶已完工，不可作廢。" };
                }
                if (ds.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                {
                    return new object[] { 0, "此用戶已作廢，不可重覆執行。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT104B.InfoParameters[0].Value = sdata[0];
                cmdRT104B.InfoParameters[1].Value = sdata[1];
                cmdRT104B.InfoParameters[2].Value = sdata[2];
                cmdRT104B.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT104B.ExecuteNonQuery();
                return new object[] { 0, "用戶申請作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶申請作廢作業,錯誤訊息" + ex };
            }
        }

        //用戶作廢返轉 
        public object[] smRT104C(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT104C.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCUST WHERE comq1=" + sdata[0] + " and lineq1 = " + sdata[1] + " and CUSID ='" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 == "")
                {
                    return new object[] { 0, "此用戶尚未作廢，不可作廢返轉。" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT104C.InfoParameters[0].Value = sdata[0];
                cmdRT104C.InfoParameters[1].Value = sdata[1];
                cmdRT104C.InfoParameters[2].Value = sdata[2];
                cmdRT104C.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT104C.ExecuteNonQuery();
                return new object[] { 0, "用戶申請作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行用戶申請作廢作業,錯誤訊息" + ex };
            }
        }
    }
}
