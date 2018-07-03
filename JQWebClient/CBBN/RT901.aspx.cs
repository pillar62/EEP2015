using DevExpress.XtraPrinting;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class CBBN_RT901 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ViewState["MyDataSet"] != null && ViewState["MydataSet"] != "")
        {
            ASPxGridView1.DataSourceID = "";
            ASPxGridView1.DataSource = ViewState["MyDataSet"];
            ASPxGridView1.DataBind();
        }

    }


    protected void aspxBtnSearch_Click(object sender, EventArgs e)
    {
        //Response.Write("<Script language='JavaScript'>alert('Y2J測試！');</Script>");
        //string SQLString = GetReportCondition();
        string SQLString = "SELECT * FROM VIEW_CUSDATA";
        //sql語法寫到本機txt檔
        #region sql語法寫到本機txt檔
        //FileStream fileStream = new FileStream(@"D:\test1.txt", FileMode.Create);

        //fileStream.Close();   //切記開了要關,不然會被佔用而無法修改喔!!!

        //using (StreamWriter sw = new StreamWriter(@"D:\test1.txt"))
        //{
        //    // 欲寫入的文字資料 ~
        //    sw.Write(SQLString);
        //}
        #endregion

        string conString = WebConfigurationManager.ConnectionStrings["RTlibNConnectionString"].ConnectionString;
        //資料存到table
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        #region 重新取值
        using (SqlConnection con = new SqlConnection())
        {
            //引用全域設定的資料庫連接字串
            con.ConnectionString = conString;
            try
            {
                con.Open();

                //建立command物件，取得da
                cmd.Connection = con;
                cmd.CommandText = SQLString;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds, "CustomSqlQuery");
                ViewState["MyDataSet"] = ds;

                ASPxGridView1.DataSourceID = "";
                ASPxGridView1.DataSource = ds;
                ASPxGridView1.DataBind();
                ASPxGridView1.ExpandAll();
                ASPxGridView1.PageIndex = 0;
            }
            catch (Exception ex)
            {
                

                //MessageBox.Show(ex.Message, this.Page);
                //throw;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion
    }

    protected void ASPxButton2_Click(object sender, EventArgs e)
    {
        //ASPxGridViewExporter1.ReportHeader = "PI報表";
        ASPxGridViewExporter1.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG });
    }
}