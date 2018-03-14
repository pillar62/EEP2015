using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;

namespace sRT1043
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
        public object[] smRT10431(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10431.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10431.InfoParameters[0].Value = sdata[0];
                cmdRT10431.InfoParameters[1].Value = sdata[1];
                cmdRT10431.InfoParameters[2].Value = sdata[2];
                cmdRT10431.InfoParameters[3].Value = sdata[3];
                cmdRT10431.InfoParameters[4].Value = sdata[4];
                cmdRT10431.InfoParameters[5].Value = sdata[5];
                cmdRT10431.InfoParameters[6].Value = sdata[6];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10431.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10432(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10432.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10432.InfoParameters[0].Value = sdata[0];
                cmdRT10432.InfoParameters[1].Value = sdata[1];
                cmdRT10432.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10432.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10433(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10433.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10433.InfoParameters[0].Value = sdata[0];
                cmdRT10433.InfoParameters[1].Value = sdata[1];
                cmdRT10433.InfoParameters[2].Value = sdata[2];
                cmdRT10433.InfoParameters[3].Value = sdata[3];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10433.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10434(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10434.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10434.InfoParameters[0].Value = sdata[0];
                cmdRT10434.InfoParameters[1].Value = sdata[1];
                cmdRT10434.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10434.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10435(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10435.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10435.InfoParameters[0].Value = sdata[0];
                cmdRT10435.InfoParameters[1].Value = sdata[1];
                cmdRT10435.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10435.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }

        public object[] smRT10436(object[] objParam)
        {
            var ss = (string)objParam[0];
            var sdata = ss.Split(',');
            //開啟資料連接
            IDbConnection conn = cmdRT10436.Connection;
            conn.Open();
            //設定輸入參數的值
            try
            {
                cmdRT10436.InfoParameters[0].Value = sdata[0];
                cmdRT10436.InfoParameters[1].Value = sdata[1];
                cmdRT10436.InfoParameters[2].Value = sdata[2];
                /*取得統計的結果，並將結果返回*/
                double ii = cmdRT10436.ExecuteNonQuery();
                return new object[] { 0, ii };
            }
            catch (Exception ex)
            {
                return new object[] { 0, ex };
            }
        }
    }
}
