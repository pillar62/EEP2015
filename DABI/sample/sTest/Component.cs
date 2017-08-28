using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;
using System.Data;
using Newtonsoft.Json;

namespace sTest
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

        //匯入Excel資料到DB
        public object ImportData(object[] objParam)
        {
            IDbConnection conn = (IDbConnection)AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString());
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            IDbTransaction trans = conn.BeginTransaction();
            try
            {
                string custNo = objParam[0].ToString();

                //解析傳過來的DataTable
                DataTable dtM = JsonConvert.DeserializeObject<DataTable>(objParam[1].ToString().Trim());

                //取得DataTable值
                string TestID = dtM.Rows[0]["TestID"].ToString();
                string TestSeq = dtM.Rows[0]["TestSeq"].ToString();
                string TestTry = dtM.Rows[0]["TestTry"].ToString();

                //執行 Insert SQL
                string SQL = "INSERT INTO TestDetail " +
                    " (TestID,TestSeq,TestTry)" +
                    " VALUES ('" + TestID + "','" + TestSeq + "','" + TestTry + "')";
                    this.ExecuteCommand(SQL, conn, trans);
    
                trans.Commit();
                return new object[] { 0, "Y," + "匯入成功!" };
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return new object[] { 0, "N," + ex.Message };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), conn);
            }
        }
    }
}
