using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using Newtonsoft.Json;

public partial class ReportViewerTemplate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string remoteName = Request.QueryString["RemoteName"];
            this.DataSource.RemoteName = remoteName;
            string ReportPath = Request.QueryString["ReportPath"];
            ReportPath = ReportPath.Replace("~/", String.Empty);
            this.ReportViewer1.LocalReport.ReportPath = ReportPath;
            string TableName = Request.QueryString["TableName"];
            this.DataSource.DataMember = TableName;
            string WhereString = Request.QueryString["WhereString"];
            this.DataSource.SetWhere(WhereString);
            this.DataSource.DataBind();
            string DataSetName = "NewDataSet";
            if (!string.IsNullOrEmpty(Request.QueryString["DataSetName"]))
            {
                DataSetName = Request.QueryString["DataSetName"];
            }
            string fileString = Server.MapPath(ReportPath);
            System.IO.StreamReader sw = new System.IO.StreamReader(fileString);
            string allfilestring = sw.ReadToEnd();
            if (allfilestring.Contains("<DataSet Name=\"NewDataSet\">"))
            {
                DataSetName = "NewDataSet";
            }


            string SP = Request.QueryString["SP"];
            if (SP == "Y")
            {
                string SPParam = Request.QueryString["SPParam"];
                string AssemblyName = Request.QueryString["AssemblyName"];
                string MethodName = Request.QueryString["MethodName"];
                var param = new List<object>();
                param.Add(SPParam);
                object ret = EFClientTools.ClientUtility.Client.CallServerMethod(EFClientTools.ClientUtility.ClientInfo, AssemblyName, MethodName, param);

                DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(ret.ToString());

                ReportViewer1.LocalReport.ReportPath = ReportPath;
                ReportViewer1.LocalReport.ReportEmbeddedResource = ReportPath;
                ReportDataSource rpt = new ReportDataSource(DataSetName, dataTable);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rpt);
                ReportViewer1.LocalReport.EnableHyperlinks = false;
            }
            else
                ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(DataSetName, DataSource.InnerDataSet.Tables[0]));
        }
    }

    protected void SubreportProcessing(object sender, Microsoft.Reporting.WebForms.SubreportProcessingEventArgs e)
    {
        if (DataSource.InnerDataSet.Tables.Count > 1)
        {
            String DataSetName = "NewDataSet";
            if (!string.IsNullOrEmpty(Request.QueryString["DataSetName"]))
            {
                DataSetName = Request.QueryString["DataSetName"];
                for (int i = 1; i < DataSource.InnerDataSet.Tables.Count; i++)
                {
                    e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(DataSetName, DataSource.InnerDataSet.Tables[i]));
                }
            }
        }
    }

    protected void ReportViewer_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ReportViewer report = sender as ReportViewer;
            var parameters = new List<ReportParameter>();
            //ReportParameter parameterQueryCondition = new ReportParameter("QueryCondition", Request.QueryString["WhereTextString"]);
            //ReportParameter parameterReportDate = new ReportParameter("ReportDate", DateTime.Today.ToString("yyyy/MM/dd"));
            //ReportParameter parameterReportDateTime = new ReportParameter("ReportDateTime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            //ReportParameter parameterUserID = new ReportParameter("UserID", EFClientTools.ClientUtility.ClientInfo.UserID);
            //ReportParameter parameterUserName = new ReportParameter("UserName", EFClientTools.ClientUtility.ClientInfo.UserName);

            //var companyName = string.Empty;
            //var logoAddress = string.Empty;
            //if (!string.IsNullOrEmpty(EFClientTools.ClientUtility.ClientInfo.SDDeveloperID))
            //{
            //    var dataSet = DataSetHelper.GetSolutions(EFClientTools.ClientUtility.ClientInfo.SDDeveloperID); 
            //    var dataTable = dataSet.Tables[0];
            //    for (int i = 0; i < dataTable.Rows.Count; i++)
            //    {
            //        if (dataTable.Rows[i]["SolutionID"] != null && dataTable.Rows[i]["SolutionID"].ToString() == EFClientTools.ClientUtility.ClientInfo.Solution)
            //        {
            //            var images = dataTable.Rows[i]["Images"].ToString().Split(';');
            //            var reportLogo = images.FirstOrDefault(c => c.IndexOf("ReportLogo") >= 0);
            //            if (reportLogo != null && reportLogo.Split('=').Length == 2)
            //            {
            //                logoAddress = reportLogo.Split('=')[1];
            //            }

            //            var company = images.FirstOrDefault(c => c.IndexOf("CompanyName") >= 0);
            //            if (company != null && company.Split('=').Length == 2)
            //            {
            //                companyName = company.Split('=')[1];
            //            }
            //            break;
            //        }
            //    }
            //}
            //ReportParameter parameterCompanyName = new ReportParameter("CompanyName", companyName);

            //替換為本地地址
            //ReportParameter parameterLogo = new ReportParameter("Logo", logoAddress);
            //string ReportPath = Request.QueryString["ReportPath"];
            //if (ReportPath.IndexOf("rdlc/", StringComparison.OrdinalIgnoreCase) < 0)
            //{
            //    parameters.AddRange(new ReportParameter[] { parameterCompanyName, parameterQueryCondition, parameterReportDate
            //    ,parameterReportDateTime, parameterUserID, parameterUserName, parameterLogo});
            //}

            for (int i = 0; i < Request.QueryString.Keys.Count; i++)
            {
                var key = Request.QueryString.Keys[i];
                if (key != null && key.StartsWith("RP"))
                {
                    var value = Request.QueryString[key];
                    var parameter = new ReportParameter(key.Substring(2), value);
                    parameters.Add(parameter);
                }
            }

            report.LocalReport.SetParameters(parameters);
            report.LocalReport.Refresh();
            //add by lu 2014.2.10 
            //直接輸出成PDF，沒有預覽功能，要使用此功能請將下行註釋拿掉。
            if (Request.QueryString["pdf"] != null && Request.QueryString["pdf"] == "true")
            {
                Export();
            }
            if (Request.QueryString["word"] != null && Request.QueryString["word"] == "true")
            {
                WordExport();
            }
            if (Request.QueryString["excel"] != null && Request.QueryString["excel"] == "true")
            {
                ExcelExport();
            }

        }
    }

    private void Export()
    {
        var reportname = "output.pdf";
        if (Request.QueryString["reportname"] != null)
        {
            reportname = Request.QueryString["reportname"] + ".pdf";
        }
        //Export report file
        string mimeType, encoding, extension, deviceInfo;
        string[] streamids;
        Microsoft.Reporting.WebForms.Warning[] warnings;
        string format = "PDF";
        //Desired format goes here (PDF, Excel, or Image)
        deviceInfo = "<DeviceInfo>" + "<SimplePageHeaders>True</SimplePageHeaders>" + "</DeviceInfo>";
        byte[] bytes = ReportViewer1.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-disposition", "filename=" + reportname);

        Response.OutputStream.Write(bytes, 0, bytes.Length);
        Response.OutputStream.Flush();
        Response.OutputStream.Close();
        Response.Flush();
        Response.Close();
    }

    private void WordExport()
    {
        var reportname = "output.doc";
        if (Request.QueryString["reportname"] != null)
        {
            reportname = Request.QueryString["reportname"] + ".doc";
        }
        string mimeType, encoding, extension, deviceInfo;
        string[] streamids;
        Microsoft.Reporting.WebForms.Warning[] warnings;
        string format = "Word";
        //Desired format goes here (PDF, Excel, or Image)
        deviceInfo = "<DeviceInfo>" + "<SimplePageHeaders>True</SimplePageHeaders>" + "</DeviceInfo>";
        byte[] bytes = ReportViewer1.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
        Response.Clear();
        Response.ContentType = "application/msword";
        Response.AddHeader("Content-disposition", "filename=" + reportname);

        Response.OutputStream.Write(bytes, 0, bytes.Length);
        Response.OutputStream.Flush();
        Response.OutputStream.Close();
        Response.Flush();
        Response.Close();

    }

    private void ExcelExport()
    {
        var reportname = "output.xls";
        if (Request.QueryString["reportname"] != null)
        {
            reportname = Request.QueryString["reportname"] + ".xls";
        }

        string mimeType, encoding, extension, deviceInfo;
        string[] streamids;
        Microsoft.Reporting.WebForms.Warning[] warnings;
        string format = "Excel";
        //Desired format goes here (PDF, Excel, or Image)
        deviceInfo = "<DeviceInfo>" + "<SimplePageHeaders>True</SimplePageHeaders>" + "</DeviceInfo>";
        byte[] bytes = ReportViewer1.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
        Response.Clear();
        Response.ContentType = "application/msexcel";
        Response.AddHeader("Content-disposition", "filename=" + reportname);

        Response.OutputStream.Write(bytes, 0, bytes.Length);
        Response.OutputStream.Flush();
        Response.OutputStream.Close();
        Response.Flush();
        Response.Close();


    
    }
}