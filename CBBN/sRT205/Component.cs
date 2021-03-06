using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT205
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
            return string.Format("{0:yyMMdd}", DateTime.Now.Date);
        }

       
        public object[] smRT2055(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmd.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                string selectSql = "select * FROM RTFaqM WHERE caseno='" + sdata[0] + "'";
                cmd.CommandText = selectSql;
                DataSet ds = cmd.ExecuteDataSet();
                selectSql = "select * FROM RTSndWork WHERE linkno='" + sdata[0] + "' and canceldat is null and finishdat is null ";
                cmd.CommandText = selectSql;
                DataSet yy = cmd.ExecuteDataSet();

                if (yy.Tables[0].Rows.Count > 0)
                {
                    return new object[] { 0, "派工單尚未完工，無法結案!!" };
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["CANCELdat"].ToString() != "")
                    {
                        return new object[] { 0, "客訴單已作廢，不能結案。" };
                    }
                    if (ds.Tables[0].Rows[0]["closedat"].ToString() != "")
                    {
                        return new object[] { 0, "請勿重覆結案。" };
                    }
                }

                /*取得統計的結果，並將結果返回*/
                selectSql = " update RTFaqM set closedat=getdate(),closeusr='" + sdata[1] + "' WHERE caseno='" + sdata[0] + "' ";
                cmd.CommandText = selectSql;
                double ii = cmd.ExecuteNonQuery();
                return new object[] { 0, "客訴單結案成功" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行客訴單作廢作業,錯誤訊息：" + ex };
            }
        }

        public object[] smRT2056(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmd.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                string selectSql = "select * FROM RTFaqM WHERE caseno='" + sdata[0] + "'";
                cmd.CommandText = selectSql;
                DataSet ds = cmd.ExecuteDataSet();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["CANCELdat"].ToString() != "")
                    {
                        return new object[] { 0, "客訴單已作廢，不能返轉。" };
                    }
                    if (ds.Tables[0].Rows[0]["closedat"].ToString() != "" && ds.Tables[0].Rows[0]["closeusr"].ToString() != sdata[1])
                    {
                        return new object[] { 0, "僅有原結案人可返轉此客訴單，返轉失敗 !!"+ ds.Tables[0].Rows[0]["closeusr"].ToString() };
                    }
                    if (ds.Tables[0].Rows[0]["closedat"].ToString() == "")
                    {
                        return new object[] { 0, "客訴單尚未結案，不能返轉。" };
                    }
                }

                /*取得統計的結果，並將結果返回*/
                selectSql = " update RTFaqM set closedat=null,closeusr='' WHERE caseno='" + sdata[0] + "' ";
                cmd.CommandText = selectSql;
                double ii = cmd.ExecuteNonQuery();
                return new object[] { 0, "客訴單返轉成功。" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行客訴單返轉作業,錯誤訊息：" + ex };
            }
        }

        public object[] smRT2057(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmd.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                string selectSql = "select * FROM RTFaqM WHERE caseno='" + sdata[0] + "'";
                cmd.CommandText = selectSql;
                DataSet ds = cmd.ExecuteDataSet();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["CANCELdat"].ToString() != "")
                    {
                        return new object[] { 0, "客訴單已作廢，不可重覆執行。" };
                    }
                }

                /*取得統計的結果，並將結果返回*/
                selectSql = " update RTFaqM set canceldat=getdate(),cancelusr='" + sdata[1] + "' WHERE caseno='" + sdata[0] + "' ";
                cmd.CommandText = selectSql;
                double ii = cmd.ExecuteNonQuery();
                selectSql = " update RTSndWork set canceldat=getdate(),cancelusr='" + sdata[1] + "', memo = memo+' [因客訴單作廢而取消派工單]' WHERE linkno='" + sdata[0] + "' and (worktype ='01' or worktype ='09') and canceldat is null ";
                cmd.CommandText = selectSql;
                ii = cmd.ExecuteNonQuery();
                return new object[] { 0, "客訴單作廢成功。" };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行客訴單作廢作業,錯誤訊息：" + ex };
            }
        }

        public object[] smRT20531(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmd.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                string selectSql = "select * FROM RTFaqAdd WHERE caseno='" + sdata[0] + "' and entryno=" + sdata[1];
                cmd.CommandText = selectSql;
                DataSet ds = cmd.ExecuteDataSet();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["CANCELDAT"].ToString() != "")
                    {
                        return new object[] { 0, "客訴追件已作廢，不可重覆執行" };
                    }
                }

                /*取得統計的結果，並將結果返回*/
                selectSql = " update RTFaqAdd set canceldat=getdate(),cancelusr='" + sdata[2] + "' WHERE caseno='" + sdata[0] + "' and entryno=" + sdata[1];
                cmd.CommandText = selectSql;
                double ii = cmd.ExecuteNonQuery();
                return new object[] { 0, "客訴追件作廢成功" + selectSql };
            }
            catch (Exception ex)
            {
                return new object[] { 0, "無法執行追件作廢作業,錯誤訊息：" + ex };
            }
        }

        public object[] smRT2059(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmd.Connection;
            conn.Open();
            string sSTR = "";
            //設定輸入參數的值
            try
            {
                string selectSql = "select CUSNC, CONTACTTEL, MOBILE FROM RTLessorAVSCust WHERE CUSID ='" + sdata[0] + "'";
                cmd.CommandText = selectSql;
                DataSet ds = cmd.ExecuteDataSet();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    sSTR = ds.Tables[0].Rows[0]["CUSNC"].ToString() + "," + ds.Tables[0].Rows[0]["CONTACTTEL"].ToString() + "," + ds.Tables[0].Rows[0]["MOBILE"].ToString();
                }
                else
                    sSTR = ",,";

                return new object[] { 0, sSTR};
            }
            catch (Exception ex)
            {
                return new object[] { 0, sSTR};
            }
        }

        private void ucRTFaqAdd_BeforeInsert(object sender, UpdateComponentBeforeInsertEventArgs e)
        {
            //在新增之前取得最大流水號 加一寫入項次
            object obj = ucRTFaqAdd.GetFieldCurrentValue("CASENO");
            string caseno = obj.ToString();
            string ssql = "select max(entryno) AS maxentryno from RTFaqAdd where caseno = '" + caseno + "' ";
            int ii = 0;
            cmd.CommandText = ssql;
            IDbConnection conn = cmd.Connection;
            conn.Open();
            DataSet ds = cmd.ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["maxentryno"].ToString() != "")
                {
                    string ss = ds.Tables[0].Rows[0]["maxentryno"].ToString();
                    ii = Int32.Parse(ss);
                }
            }

            ii++;
            ucRTFaqAdd.SetFieldValue("ENTRYNO", ii);
        }
    }
}
