using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Diagnostics;
using System.Net;

public partial class RT3021RF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string remoteName = Request.QueryString["RemoteName"];
            this.DataSource.RemoteName = remoteName;
            string ReportPath = Request.QueryString["ReportPath"];
            ReportPath = ReportPath.Replace("~/", String.Empty);

            //this.ReportViewer1.LocalReport.ReportPath = ReportPath;
            //依參數取得資料
            string TableName = Request.QueryString["TableName"];
            this.DataSource.DataMember = TableName;
            string WhereString = Request.QueryString["WhereString"];
            this.DataSource.SetWhere(WhereString);
            this.DataSource.DataBind();

            #region 原來ReportView1套資料方式
            //string DataSetName = "NewDataSet";
            //if (!string.IsNullOrEmpty(Request.QueryString["DataSetName"]))
            //{
            //    DataSetName = Request.QueryString["DataSetName"];
            //}
            //string fileString = Server.MapPath(ReportPath);
            //System.IO.StreamReader sw = new System.IO.StreamReader(fileString);
            //string allfilestring = sw.ReadToEnd();
            //if (allfilestring.Contains("<DataSet Name=\"NewDataSet\">"))
            //{
            //    DataSetName = "NewDataSet";
            //}
            #endregion

            #region 原來ReportView的後端處理函式：用參數送入之後，用後端來呼叫ap端函式(該函式應回傳資料)
            //string SP = Request.QueryString["SP"];
            //if (SP == "Y")
            //{
            //    string SPParam = Request.QueryString["SPParam"];
            //    string AssemblyName = Request.QueryString["AssemblyName"];
            //    string MethodName = Request.QueryString["MethodName"];
            //    var param = new List<object>();
            //    param.Add(SPParam);
            //    object ret = EFClientTools.ClientUtility.Client.CallServerMethod(EFClientTools.ClientUtility.ClientInfo, AssemblyName, MethodName, param);

            //    DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(ret.ToString());

            //    ReportViewer1.LocalReport.ReportPath = ReportPath;
            //    ReportViewer1.LocalReport.ReportEmbeddedResource = ReportPath;
            //    ReportDataSource rpt = new ReportDataSource(DataSetName, dataTable);
            //    ReportViewer1.LocalReport.DataSources.Clear();
            //    ReportViewer1.LocalReport.DataSources.Add(rpt);
            //    ReportViewer1.LocalReport.EnableHyperlinks = false;
            //}
            //else
            //    ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(DataSetName, DataSource.InnerDataSet.Tables[0]));
            #endregion

            //資料存到dataset(注意報表的預設table名稱就是CustomSqlQuery)
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = this.DataSource.InnerDataSet.Tables[0].Copy();
            dt.TableName = "CustomSqlQuery";
            ds.Tables.Add(dt);

            //將資料寫入宣告的report，然後再指定給view
            //this.DataSource.DataMember
            RT3021R report = new RT3021R();
            report.DataSource = ds;
            report.DataMember = "CustomSqlQuery";
            //套到VIEWER
            ASPxDocumentViewer1.Report = report;
            //ASPxDocumentViewer1.DataBind();
        }
    }

    protected void ASPxDocumentViewer1_RestoreReportDocumentFromCache(object sender, DevExpress.XtraReports.Web.RestoreReportDocumentFromCacheEventArgs e)
    {
        Stream stream = Page.Session[e.Key] as Stream;
        if (stream != null)
            e.RestoreDocumentFromStream(stream);
    }

    protected void ASPxDocumentViewer1_CacheReportDocument(object sender, DevExpress.XtraReports.Web.CacheReportDocumentEventArgs e)
    {
        e.Key = Guid.NewGuid().ToString();

        //用viewstate記錄這個id，重新查詢時，要將這個id清除。
        ViewState["ReportSessionID"] = e.Key;
        Page.Session[e.Key] = e.SaveDocumentToMemoryStream();
    }
}