using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using DevExpress.XtraReports.UI;
using Reports;
using DevExpress.XtraReports.Web;
using DevExpress.Web;
using ACTIONModel;
using JDEModel;
using ReportViewModels;

public partial class Reports_Report1106 : System.Web.UI.Page
{

    ACTIONEntity MyEntity = new ACTIONEntity();
    ReportViewEntities ReportViewEntity = new ReportViewEntities();
    JDEEntities MyJDEEntity = new JDEEntities();
    ReportDataset MyDS;//用來多選使用

    protected void Page_Load(object sender, EventArgs e)
    {
        MARQUEELabel.Text = AcClass1.GetMARQUEE();
        System.Globalization.CultureInfo cag = new System.Globalization.CultureInfo("zh-TW");
        cag.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
        System.Threading.Thread.CurrentThread.CurrentCulture = cag;
        //this.Form.DefaultButton = ImageButton11.UniqueID;

        //原邏輯，因為尚未掛上選單，所以先不透過登入畫面

        string sPERNO = Session["TPERNO"].ToString();     //員工編號
        if (sPERNO.Trim() == string.Empty)
        {
            Label MyPERNOL = (Label)Master.FindControl("LOGINPERNOL");
            Label MyOFFNOL = (Label)Master.FindControl("LOGINGOFFNOL");
            if (MyPERNOL.Text.Trim() != string.Empty)
            {
                Session["TPERNO"] = MyPERNOL.Text;
                Session["TOUTNO"] = MyOFFNOL.Text;
            }
            else
                Response.Redirect("~/WORKSTATION/LOGIN.aspx");
        }
        string sTOUTNO = Session["TOUTNO"].ToString();     //門市編號 


        //先帶預設值
        //string sPERNO = "999999";
        //string sTOUTNO = "111111";


        if (Request.QueryString["checkPOS"] != null)
        {
            //模擬如果是門市，就直接將門市條件鎖住
            //門市代號起
            cmbSCUNO.Text = Session["TOUTNO"].ToString();
            cmbSCUNO.Enabled = false;
            //門市代號迄
            cmbECUNO.Text = Session["TOUTNO"].ToString();
            cmbECUNO.Enabled = false;
            //門市多選
            IB_MultiCUS.Visible = false;
        }

        #region 報表查詢資料處理
        //報表查詢資料
        if (!IsPostBack)  //第一次進來的時候，將SReport設為nll
        {
        }
        #endregion
    }

    private string GetReportCondition(ref DevReport1106 Report)
    {
        string SDATE = "", EDATE = "";
        SDATE = txtSDATE.Text.Trim();

        //if (SDATE == "")
        //{
        //    Reports_Report1106 Rep;
        //    MessageBox.Show("請輸入結算日!!", Rep);
        //    return "";

        //}

        string SQLString = "SELECT OFFNO, OFFNM, FIXDATE, FIXNO, PROD2, PROD, PRODNM " + "\r\n"
                         + " , IMEI1, SN, KIND1, KIND1NM, STATUS1, STATUS1NM, ITEM1, ITEM1NM " + "\r\n"
                         + " , CUSTNA, TEL1, SUPCUSNM, RMANNO, RMANNM, SPRNO, SPRNM, ITEMS, 1 AS QTY " + "\r\n"
                         + " FROM REPORT1106 " + "\r\n"                        
                         + " WHERE 1=1 " + "\r\n";

        //產生報表條件字串與SQL語法
        #region 門市代號
        string SCUNO = "", ECUNO = "";
        if (cmbSCUNO.SelectedItem == null)
            SCUNO = "";
        else
            SCUNO = cmbSCUNO.SelectedItem.Value.ToString().Trim();

        if (SCUNO != string.Empty)
        {
            SQLString += "AND OFFNO >= '" + SCUNO + "'\n";
        }

        if (cmbECUNO.SelectedItem == null)
            ECUNO = "";
        else
            ECUNO = cmbECUNO.SelectedItem.Value.ToString().Trim();
        if (ECUNO != string.Empty)
        {
            SQLString += "AND OFFNO <= '" + ECUNO + "'\n";
        }
        Report.Parameters["CUNO"].Value = "門市代號：" + SCUNO + " ~ " + ECUNO;
        #endregion

        #region 維修日期

        if (txtSDATE.Text != "")
        {
            SDATE = txtSDATE.Text.Trim();
            SQLString += "AND FIXDATE <= '" + SDATE.Replace("/", "") + "'\n";
        }
        //if (txtEDATE.Text != "")
        //{
        //    EDATE = txtEDATE.Text.Trim();
        //    SQLString += "AND FIXDATE <= '" + EDATE.Replace("/", "").Trim() + "'\n";
        //}

        Report.Parameters["DATE1"].Value = "維修日期：" + SDATE;//+ " ~ " + EDATE
        #endregion

        #region 產品編號多選
        string QueryPROD = "";
        if (lblMultiPROD.Text != "")
        {
            QueryPROD = lblMultiPROD.Text;
            SQLString += "AND PROD IN (" + QueryPROD + ")" + "\n";
            Report.Parameters["PROD"].Value = "產品多選：" + lblMultiPRODCount;
        }
        else
        {
            Report.Parameters["PROD"].Value = "產品多選：不限";
        }
        #endregion

        #region 門市多選
        string QueryCUS = "";
        if (lblMultiCUNO.Text != "")
        {
            QueryCUS = lblMultiCUNO.Text;
            SQLString += "AND OFFNO IN (" + QueryCUS + ")";
            Report.Parameters["MCUNO"].Value = "門市多選：" + lblMultiCUNOCount.Text;
        }
        else
        {
            Report.Parameters["MCUNO"].Value = "門市多選：不限";
        }
        #endregion

 
        #region 其它
        //製表人員
        Report.Parameters["CreateMan"].Value = "製表人員： " + AcClass1.GetPERNA(Session["TPERNO"].ToString());
        #endregion

        string aSQL = SQLString
                    + " ORDER BY OFFNO, FIXDATE DESC, FIXNO DESC";

        return aSQL;
    }

    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        //產出報表，然後丟給session
        try
        {
            //產生Report
            DevReport1106 Report = new DevReport1106();
            Report.Name = "門市維修機盤點表";

            //這裡只要產生條件後的SQL語法即可
            string SQLString = "" + "\r\n";//預設資料
            //畫面條件產生報表列印條件與SQL語法字串
            SQLString = GetReportCondition(ref Report);


            //sql語法寫到本機txt檔
            #region sql語法寫到本機txt檔
            //FileStream fileStream = new FileStream(@"D:\test1.txt", FileMode.Create);

            //fileStream.Close();   //切記開了要關,不然會被佔用而無法修改喔!!!

            //using (StreamWriter sw = new StreamWriter(@"D:\test1.txt"))
            //{
            //     欲寫入的文字資料 ~

            //    sw.Write(SQLString);
            //}
            #endregion

            //產生報表資料條件與SQL語法
            QueryReport(ref Report, SQLString);
            //把之前記錄的session key的報表cache的id清除掉，重新產生
            if (ViewState["ReportSessionID"] != null && ViewState["ReportSessionID"].ToString() != string.Empty)
            {
                string ReportSessionID = ViewState["ReportSessionID"].ToString();
                if (Session[ReportSessionID] != null && Session[ReportSessionID].ToString() != string.Empty)
                {
                    Session.Remove(ReportSessionID);
                }

            }
            //產生報表
            Report.CreateDocument();
            //Session["DevReport"] = Report;
            ASPxDocumentViewer1.Report = Report;


            //這裡將所有的查詢條件清除
            #region 查詢條件清除
            if (Request.QueryString["checkPOS"] == null)
            {
                //門市代號起迄
                cmbSCUNO.Value = string.Empty;
                cmbECUNO.Value = string.Empty;
            }
            //日期起迄
            txtSDATE.Text = string.Empty;
            txtEDATE.Text = string.Empty;

            //多選
            //產品編號
            lblMultiPROD.Text = "";
            lblMultiPRODCount.Text = "";
            //門市多選 
            lblMultiCUNO.Text = "";
            lblMultiCUNOCount.Text = "";

            //清除所有多選的開窗資料
            ViewState["TEMPDS"] = "";
            if (MyDS != null)
            {
                MyDS.Clear();
            }


            #endregion
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
        }
    }

    //增加查詢功能
    private void QueryReport(ref DevReport1106 report, string SQLString)
    {
        //string conString = "User Id=POS;Password=Ac28085857;" +
        //            "Data Source=59.124.220.17:1521/orcl.ac2008.com.tw;";
        string conString = WebConfigurationManager.ConnectionStrings["DevReport_ActionConnectionString"].ConnectionString;
        //資料存到table
        DataSet ds = new DataSet();
        OracleCommand cmd = new OracleCommand();

        #region 重新取值
        using (OracleConnection con = new OracleConnection())
        {
            //引用全域設定的資料庫連接字串
            con.ConnectionString = conString;
            try
            {
                con.Open();

                //建立command物件，取得da
                cmd.Connection = con;
                cmd.CommandText = SQLString;
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(ds, "CustomSqlQuery");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, HttpContext.Current.Handler as Page);
                //throw;
            }
            finally
            {
                con.Close();
            }
            //將報表載入TABLE資料
            report.DataSource = ds;
            report.DataMember = "CustomSqlQuery";
            GridView1.DataSource = ds;
            GridView1.DataBind();
        }
        #endregion

    }

    protected void ASPxDocumentViewer1_CacheReportDocument(object sender, CacheReportDocumentEventArgs e)
    {
        e.Key = Guid.NewGuid().ToString();

        //用viewstate記錄這個id，重新查詢時，要將這個id清除。
        ViewState["ReportSessionID"] = e.Key;
        Page.Session[e.Key] = e.SaveDocumentToMemoryStream();
    }
    protected void ASPxDocumentViewer1_RestoreReportDocumentFromCache(object sender, RestoreReportDocumentFromCacheEventArgs e)
    {
        Stream stream = Page.Session[e.Key] as Stream;
        if (stream != null)
            e.RestoreDocumentFromStream(stream);
    }

    //重置DataSet
    protected void NewDT()
    {
        if (MyDS == null)
        {
            if (ViewState["TEMPDS"] != null && ViewState["TEMPDS"].ToString().Trim() != string.Empty)
            {
                MyDS = (ReportDataset)ViewState["TEMPDS"];
            }
            else
                MyDS = new ReportDataset();
        }
    }

    #region 門市代號多選開窗相關控制
    protected void chkBoxCus_CheckedChanged(object sender, EventArgs e)
    {
        //勾選時將資料寫入暫存
        NewDT();
        //如果勾選起來，就增加到MyDS的MCMCU
        CheckBox MycheckBox = (CheckBox)sender;
        //取得所有的索引
        int grRowIndex = (MycheckBox.NamingContainer as GridViewRow).RowIndex;
        //取得專案編號
        Label MyKeyLabel = (Label)gdvSearchCus.Rows[grRowIndex].FindControl("lblLinkCus");
        if (MycheckBox.Checked)
        {
            MyDS.dtCUS.AdddtCUSRow(MyKeyLabel.Text, "");
        }
        else
        {
            var MyCheckedRecord = MyDS.dtCUS.FirstOrDefault(o => o.CUNO == MyKeyLabel.Text);
            if (MyCheckedRecord != null)
            {
                MyDS.dtCUS.RemovedtCUSRow(MyCheckedRecord);
            }
        }
        ViewState["TEMPDS"] = MyDS;


    }
    protected void ibPopSearchWinCus_Click(object sender, ImageClickEventArgs e)
    {
        //篩選門市資料
        gdvSearchCus.PageIndex = 0;
        gdvSearchCus.DataBind();
        mpeMultiCus.Show();
    }
    protected void gdvSearchCus_PageIndexChanged(object sender, EventArgs e)
    {
        //換頁時
        //gdvSearchCus.PageIndex = 0;
        mpeMultiCus.Show();
    }
    protected void CustEntityDataSource1_QueryCreated(object sender, QueryCreatedEventArgs e)
    {
        //篩選資料
        //重新組合sql語法
        var MyCus = MyEntity.VIEW_F0006.Select(a => a);

        if (Request.QueryString["checkPOS"] != null)
        {
            string sTOUTNO = Session["TOUTNO"].ToString();
            MyCus = MyCus.Where(o => o.MCMCU == sTOUTNO);
        }

        if (tbBaseSearch1Cus.Text.Trim() != string.Empty)
        {
            string sSEARCH = tbBaseSearch1Cus.Text.Replace("-", "").Replace("(", "").Replace(")", "").Trim().ToLower().Replace(" ", "");
            MyCus = MyCus.Where(o => o.MCDL01.Replace("-", "").Replace("(", "").Replace(")", "").ToLower().Trim().Contains(sSEARCH)
                    || o.MCMCU.Replace("-", "").Replace("(", "").Replace(")", "").ToLower().Trim().Contains(sSEARCH));
        }
        if (tbBaseSearch2Cus.Text.Trim() != string.Empty)
        {
            string sSEARCH = tbBaseSearch2Cus.Text.Replace("-", "").Replace("(", "").Replace(")", "").Trim().ToLower().Replace(" ", "");
            MyCus = MyCus.Where(o => o.MCDL01.Replace("-", "").Replace("(", "").Replace(")", "").ToLower().Trim().Contains(sSEARCH)
                    || o.MCMCU.Replace("-", "").Replace("(", "").Replace(")", "").ToLower().Trim().Contains(sSEARCH));
        }
        if (tbBaseSearch3Cus.Text.Trim() != string.Empty)
        {
            string sSEARCH = tbBaseSearch3Cus.Text.Replace("-", "").Replace("(", "").Replace(")", "").Trim().ToLower().Replace(" ", "");
            MyCus = MyCus.Where(o => o.MCDL01.Replace("-", "").Replace("(", "").Replace(")", "").ToLower().Trim().Contains(sSEARCH)
                    || o.MCMCU.Replace("-", "").Replace("(", "").Replace(")", "").ToLower().Trim().Contains(sSEARCH));
        }
        e.Query = MyCus.OrderBy(o => o.MCMCU);

    }
    protected void gdvSearchCus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //重新開窗恢復勾選
        NewDT();
        //每一筆如果在MyDT裡面，代表要勾選
        CheckBox MyCheckBox = (CheckBox)e.Row.FindControl("chkBoxCus");
        Label MylblKeyNO = (Label)e.Row.FindControl("lblLinkCus");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //James : 這裡還要再確認是否要做變數
            var MyDT = MyDS.dtCUS.FirstOrDefault(o => o.CUNO == MylblKeyNO.Text);
            if ((MyDT != null))
            {
                MyCheckBox.Checked = true;
            }
        }

    }
    protected void btnMultiSelectConfirmCus_Click(object sender, EventArgs e)
    {
        //開窗確認按鈕 客戶別
        //確認之後要做的事，把有勾選的專案編號，寫到主畫面的條件label
        NewDT();//取得目前的裡面有勾選的資料
        //接著寫到畫面的lblMultiProject 這個LABEL顯示即可。
        var strkeyNolist = "";
        int RecordCounti = 0;
        foreach (var KeyNOList in MyDS.dtCUS)
        {
            strkeyNolist += "'" + KeyNOList.CUNO + "',";
            RecordCounti++;
        }

        if (strkeyNolist != string.Empty)
        {
            lblMultiCUNO.Text = strkeyNolist.Substring(0, strkeyNolist.Length - 1);
            lblMultiCUNOCount.Text = "共選擇 " + RecordCounti.ToString() + " 筆";
        }
    }
    protected void IB_MultiCUS_Click(object sender, ImageClickEventArgs e)
    {
        //開窗選擇
        //開窗--業務別
        //如果開窗進去，要取消勾選的話，這一行要打開將暫存清除
        //ViewState["TEMPDS"] = "";

        //準備一個dataset來記錄勾選的資料
        NewDT();
        //開窗選擇專案
        ImageButton MyIB = sender as ImageButton;

        //查詢條件
        tbBaseSearch1Cus.Text = tbBaseSearch2Cus.Text = tbBaseSearch3Cus.Text = "";

        //查詢的產品資料繫結
        gdvSearchCus.PageIndex = 0;
        gdvSearchCus.DataBind();

        //開窗
        mpeMultiCus.Show();

    }
    protected void btnCancelCus_Click(object sender, EventArgs e)
    {
        mpeMultiCus.Hide();
    }
    #endregion

    #region 產品項目多選開窗相關控制
    protected void IB_MultiPROD_Click(object sender, ImageClickEventArgs e)
    {
        //開窗--產品別
        //如果開窗進去，要取消勾選的話，這一行要打開將暫存清除
        //ViewState["TEMPDS"] = "";

        //準備一個dataset來記錄勾選的資料
        NewDT();
        //開窗選擇專案
        ImageButton MyIB = sender as ImageButton;

        //查詢條件
        tbBaseSearch1Prod.Text = tbBaseSearch2Prod.Text = tbBaseSearch3Prod.Text = "";

        //查詢的產品資料繫結
        gdvSearchProd.PageIndex = 0;
        gdvSearchProd.DataBind();

        //開窗
        mpeMultiProd.Show();
    }
    protected void chBoxSelectAllPord_CheckedChanged(object sender, EventArgs e)
    {
        //全選/全不選
    }
    protected void chkBoxProd_CheckedChanged(object sender, EventArgs e)
    {
        //勾選時將資料寫入暫存
        NewDT();
        //如果勾選起來，就增加到MyDS的SALEMAN
        CheckBox MycheckBox = (CheckBox)sender;
        //取得所有的索引
        int grRowIndex = (MycheckBox.NamingContainer as GridViewRow).RowIndex;
        //取得專案編號
        Label MyKeyLabel = (Label)gdvSearchProd.Rows[grRowIndex].FindControl("lblLinkProd");
        if (MycheckBox.Checked)
        {
            MyDS.dtProduct.AdddtProductRow(MyKeyLabel.Text, "");
        }
        else
        {
            var MyCheckedRecord = MyDS.dtProduct.FirstOrDefault(o => o.IMLITM == MyKeyLabel.Text);
            if (MyCheckedRecord != null)
            {
                MyDS.dtProduct.RemovedtProductRow(MyCheckedRecord);
            }
        }
        ViewState["TEMPDS"] = MyDS;

    }
    protected void ibPopSearchWinProd_Click(object sender, ImageClickEventArgs e)
    {
        //篩選產品
        gdvSearchProd.PageIndex = 0;
        gdvSearchProd.DataBind();
        mpeMultiProd.Show();
    }
    protected void btnCancelProd_Click(object sender, EventArgs e)
    {
        //關閉視窗
        mpeMultiProd.Hide();
    }
    protected void btnMultiSelectConfirmProd_Click(object sender, EventArgs e)
    {
        //開窗確認按鈕 產品
        //確認之後要做的事，把有勾選的專案編號，寫到主畫面的條件label
        NewDT();//取得目前的裡面有勾選的資料
        //接著寫到畫面的lblMultiProject 這個LABEL顯示即可。
        var strkeyNolist = "";
        int RecordCounti = 0;
        foreach (var KeyNOList in MyDS.dtProduct)
        {
            strkeyNolist += "'" + KeyNOList.IMLITM + "',";
            RecordCounti++;
        }

        if (strkeyNolist != string.Empty)
        {
            lblMultiPROD.Text = strkeyNolist.Substring(0, strkeyNolist.Length - 1);
            lblMultiPRODCount.Text = "共選擇 " + RecordCounti.ToString() + " 筆";
        }
    }
    protected void PRODEntityDataSource1_QueryCreated(object sender, QueryCreatedEventArgs e)
    {
        //組合sql資料
        //重新組合sql語法
        var MyProd = ReportViewEntity.VIEW_F4101.Select(a => a);

        if (tbBaseSearch1Prod.Text.Trim() != string.Empty)
        {
            string sSEARCH = tbBaseSearch1Prod.Text.Replace("-", "").Replace("(", "").Replace(")", "").Trim().ToLower().Replace(" ", "");
            MyProd = MyProd.Where(o => o.SGNNAMESTRING.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").ToLower().Trim().Contains(sSEARCH)
                    || o.IMLITM.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").ToLower().Trim().Contains(sSEARCH));
        }
        if (tbBaseSearch2Prod.Text.Trim() != string.Empty)
        {
            string sSEARCH = tbBaseSearch2Prod.Text.Replace("-", "").Replace("(", "").Replace(")", "").Trim().ToLower().Replace(" ", "");
            MyProd = MyProd.Where(o => o.SGNNAMESTRING.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").ToLower().Trim().Contains(sSEARCH)
                    || o.IMLITM.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").ToLower().Trim().Contains(sSEARCH));
        }
        if (tbBaseSearch3Prod.Text.Trim() != string.Empty)
        {
            string sSEARCH = tbBaseSearch3Prod.Text.Replace("-", "").Replace("(", "").Replace(")", "").Trim().ToLower().Replace(" ", "");
            MyProd = MyProd.Where(o => o.SGNNAMESTRING.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").ToLower().Trim().Contains(sSEARCH)
                    || o.IMLITM.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").ToLower().Trim().Contains(sSEARCH));
        }
        e.Query = MyProd.OrderBy(o => o.IMLITM);
    }
    protected void gdvSearchProd_PageIndexChanged(object sender, EventArgs e)
    {
        //換頁
        mpeMultiProd.Show();
    }
    protected void gdvSearchProd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //重新綁定時，恢復原來的框框
        NewDT();
        //每一筆如果在MyDT裡面，代表要勾選
        CheckBox MyCheckBox = (CheckBox)e.Row.FindControl("chkBoxProd");
        Label MylblKeyNO = (Label)e.Row.FindControl("lblLinkProd");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //James : 這裡還要再確認是否要做變數
            var MyRecord = MyDS.dtProduct.FirstOrDefault(o => o.IMLITM == MylblKeyNO.Text);
            if ((MyRecord != null))
            {
                MyCheckBox.Checked = true;
            }
        }
    }
    #endregion


}