using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT1044
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
            return string.Format("EM{0:yyMMdd}", DateTime.Now.Date);
        }

        public object[] smRT10441(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10441.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10441.InfoParameters[0].Value = sdata[0];
                cmdRT10441.InfoParameters[1].Value = sdata[1];
                cmdRT10441.InfoParameters[2].Value = sdata[2];
                cmdRT10441.InfoParameters[3].Value = sdata[3];
                cmdRT10441.InfoParameters[4].Value = sdata[4];
                cmdRT10441.InfoParameters[5].Value = sdata[5];
                cmdRT10441.InfoParameters[6].Value = sdata[6];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10441.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10442(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10442.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10442.InfoParameters[0].Value = sdata[0];
                cmdRT10442.InfoParameters[1].Value = sdata[1];
                cmdRT10442.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10442.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10443(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10443.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10443.InfoParameters[0].Value = sdata[0];
                cmdRT10443.InfoParameters[1].Value = sdata[1];
                cmdRT10443.InfoParameters[2].Value = sdata[2];
                cmdRT10443.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10443.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10444(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10444.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10444.InfoParameters[0].Value = sdata[0];
                cmdRT10444.InfoParameters[1].Value = sdata[1];
                cmdRT10444.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10444.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10445(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10445.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10445.InfoParameters[0].Value = sdata[0];
                cmdRT10445.InfoParameters[1].Value = sdata[1];
                cmdRT10445.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10445.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10446(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10446.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10446.InfoParameters[0].Value = sdata[0];
                cmdRT10446.InfoParameters[1].Value = sdata[1];
                cmdRT10446.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10446.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10447(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10447.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10447.InfoParameters[0].Value = sdata[0];
                cmdRT10447.InfoParameters[1].Value = sdata[1];
                cmdRT10447.InfoParameters[2].Value = sdata[2];
                cmdRT10447.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10447.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10448(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10448.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10448.InfoParameters[0].Value = sdata[0];
                cmdRT10448.InfoParameters[1].Value = sdata[1];
                cmdRT10448.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10448.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10449(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10449.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10449.InfoParameters[0].Value = sdata[0];
                cmdRT10449.InfoParameters[1].Value = sdata[1];
                cmdRT10449.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10449.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }
    }
}
