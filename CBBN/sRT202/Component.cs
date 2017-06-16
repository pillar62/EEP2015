using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT202
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

        //轉派工單
        public object[] smRT2021(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT2021.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCMTYLINEFaqH WHERE COMQ1='" + sdata[0] + "' and LINEQ1=" + sdata[1] + " and FAQNO = '" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "當已作廢時，不可轉派工單" };
                }
                ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();                
                if (ss1 != "")
                {
                    return new object[] { 0, "已結案時，不可轉派工單。" };
                }
                ss1 = ds.Tables[0].Rows[0]["SNDPRTNO"].ToString();                
                if (ss1 != "")
                {
                    return new object[] { 0, "已有派工單時，不可重覆執行。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT2021.InfoParameters[0].Value = sdata[0];
                cmdRT2021.InfoParameters[1].Value = sdata[1];
                cmdRT2021.InfoParameters[2].Value = sdata[2];
                cmdRT2021.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT2021.ExecuteNonQuery();
                return new object[] { 0, "主線客服單轉派工單成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行客服單轉派工單作業,錯誤訊息" + ex };
            }
        }

        //客服結案
        public object[] smRT2022(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT2022.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCMTYLINEFaqH WHERE COMQ1='" + sdata[0] + "' and LINEQ1=" + sdata[1] + " and FAQNO = '" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "當已作廢時，不可執行客服單結案" };
                }
                ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此客服單已結案，不可重複執行。" };
                }
                ss1 = ds.Tables[0].Rows[0]["SNDPRTNO"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此客服單已轉派工單，派工單需結案後始可執行客服單結案作業。" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT2022.InfoParameters[0].Value = sdata[0];
                cmdRT2022.InfoParameters[1].Value = sdata[1];
                cmdRT2022.InfoParameters[2].Value = sdata[2];
                cmdRT2022.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT2022.ExecuteNonQuery();
                return new object[] { 0, "主線客服單結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行客服單結案作業,錯誤訊息" + ex };
            }
        }

        //結案返轉
        public object[] smRT2023(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT2023.Connection;
            conn.Open();
            string selectSql = "select * FROM RTLessorAVSCMTYLINEFaqH WHERE COMQ1='" + sdata[0] + "' and LINEQ1=" + sdata[1] + " and FAQNO = '" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此客服單尚未結案，不可執行客服單結案返轉" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT2023.InfoParameters[0].Value = sdata[0];
                cmdRT2023.InfoParameters[1].Value = sdata[1];
                cmdRT2023.InfoParameters[2].Value = sdata[2];
                cmdRT2023.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT2023.ExecuteNonQuery();
                return new object[] { 0, "主線客服單結案返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行客服單結案返轉作業,錯誤訊息" + ex };
            }
        }

        //主線客服單作廢
        public object[] smRT2024(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT2024.Connection;
            conn.Open();
            string ss1 = "";
            string selectSql = "select * FROM RTLessorAVSCMTYLINEFaqH WHERE COMQ1='" + sdata[0] + "' and LINEQ1=" + sdata[1] + " and FAQNO = '" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ss1 = ds.Tables[0].Rows[0]["FINISHDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此主線客服單資料已結案，不可作廢。(請改用結案返轉)。" };
                }
                ss1 = ds.Tables[0].Rows[0]["SNDPRTNO"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此主線客服單資料已轉派工單，不可作廢。" };
                }
                ss1 = ds.Tables[0].Rows[0]["CALLBACKDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "已押CALLBACK(回覆日)，不可作廢。(請先取消回覆日)" };
                }
                ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 != "")
                {
                    return new object[] { 0, "此主線客服單資料已作廢，不可重覆執行" };
                }
            }
            //設定輸入參數的值
            try
            {
                cmdRT2024.InfoParameters[0].Value = sdata[0];
                cmdRT2024.InfoParameters[1].Value = sdata[1];
                cmdRT2024.InfoParameters[2].Value = sdata[2];
                cmdRT2024.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT2024.ExecuteNonQuery();
                return new object[] { 0, "主線客服單資料作廢成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線客服單資料作廢作業,錯誤訊息：" + ex };
            }
        }

        //主線客服單作廢返轉
        public object[] smRT2025(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT2025.Connection;
            conn.Open();
            string ss2 = "";
            string ss1 = "";
            string selectSql = "select * FROM RTLessorAVSCMTYLINEFaqH WHERE COMQ1='" + sdata[0] + "' and LINEQ1=" + sdata[1] + " and FAQNO = '" + sdata[2] + "'";
            cmd.CommandText = selectSql;
            DataSet ds = cmd.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ss1 = ds.Tables[0].Rows[0]["CANCELDAT"].ToString();
                if (ss1 == "")
                {
                    return new object[] { 0, "此主線客服單資料尚未作廢，不可返轉" };
                }
            }

            //設定輸入參數的值
            try
            {
                cmdRT2025.InfoParameters[0].Value = sdata[0];
                cmdRT2025.InfoParameters[1].Value = sdata[1];
                cmdRT2025.InfoParameters[2].Value = sdata[2];
                cmdRT2025.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT2025.ExecuteNonQuery();
                return new object[] { 0, "主線客服單資料作廢返轉成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行主線客服單資料作廢作業,錯誤訊息：" + ex };
            }
        }
    }
}
