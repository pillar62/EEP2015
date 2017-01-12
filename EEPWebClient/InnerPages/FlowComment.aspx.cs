using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Srvtools;

public partial class InnerPages_FlowComment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ViewState["ListId"] = Request.QueryString["LISTID"];
            this.ViewState["ATTACHMENTS"] = Request.QueryString["ATTACHMENTS"];
            this.ViewState["VDSNAME"] = Request.QueryString["VDSNAME"];
            GenSource();
            if (this.gdvHis.Rows.Count > 0)
            {
                string[] UIHisTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "SubmitConfirm", "HisUIText", true).Split(',');
                if (UIHisTexts.Length >= 7)
                {
                    this.gdvHis.HeaderRow.Cells[0].Text = UIHisTexts[5];
                    this.gdvHis.HeaderRow.Cells[1].Text = UIHisTexts[0];
                    this.gdvHis.HeaderRow.Cells[2].Text = UIHisTexts[1];
                    this.gdvHis.HeaderRow.Cells[3].Text = UIHisTexts[6];
                    this.gdvHis.HeaderRow.Cells[4].Text = UIHisTexts[4];
                    this.gdvHis.HeaderRow.Cells[5].Text = UIHisTexts[7];
                    this.gdvHis.HeaderRow.Cells[6].Text = UIHisTexts[2];
                    this.gdvHis.HeaderRow.Cells[7].Text = UIHisTexts[3];
                }
            }
        }
        GenDownLoadHref();
    }

    public string getIndexTitle(int i)
    {
        string[] UICommitTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "SubmitConfirm", "CommitText", true).Split(',');
        if (i < UICommitTexts.Length)
            return UICommitTexts[i];
        return "";
    }

    private bool isFlowFilesBySolutions()
    {
        string config_FilesBySol = ConfigurationManager.AppSettings["FlowFilesBySolutions"];
        if (!string.IsNullOrEmpty(config_FilesBySol) && string.Compare(config_FilesBySol, "true", true) == 0)
            return true;
        return false;
    }

    private void GenDownLoadHref()
    {
        if (this.ViewState["ATTACHMENTS"] == null || this.ViewState["ATTACHMENTS"].ToString() == "") return;
        string[] lstAttachments = this.ViewState["ATTACHMENTS"].ToString().Split(';');
        Table table = new Table();
        TableRow row = new TableRow();
        foreach (string attach in lstAttachments)
        {
            TableCell cell = new TableCell();
            HtmlAnchor a = new HtmlAnchor();
            a.InnerHtml = attach;
            a.Target = "_top";
            a.HRef = "../WorkflowFiles/" + (isFlowFilesBySolutions() ? this.ViewState["VDSNAME"].ToString() + "/" : "") + attach;
            cell.Controls.Add(a);
            row.Cells.Add(cell);
        }
        table.Rows.Add(row);
        this.panDownload.Controls.Add(table);
    }

    private void GenSource()
    {
        string sql = "";
        if (this.ViewState["ListId"] != null && this.ViewState["ListId"].ToString() != "")
        {
            //sql = "SELECT FLOW_DESC,S_ROLE_ID,S_STEP_ID,USER_ID,USERNAME,STATUS,UPDATE_DATE,UPDATE_TIME,REMARK,ATTACHMENTS FROM SYS_TODOHIS Where (LISTID = '" + this.ViewState["ListId"].ToString() + "') ORDER BY UPDATE_DATE,UPDATE_TIME";
            sql = "SELECT SYS_TODOHIS.FLOW_DESC,SYS_TODOHIS.S_ROLE_ID,SYS_TODOHIS.S_STEP_ID,SYS_TODOHIS.USER_ID,SYS_TODOHIS.USERNAME,SYS_TODOHIS.STATUS,SYS_TODOHIS.UPDATE_DATE,SYS_TODOHIS.UPDATE_TIME,SYS_TODOHIS.REMARK,SYS_TODOHIS.ATTACHMENTS,SYS_TODOHIS.FORM_PRESENT_CT,GROUPS.GROUPNAME FROM SYS_TODOHIS LEFT JOIN GROUPS ON SYS_TODOHIS.S_ROLE_ID = GROUPS.GROUPID WHERE (SYS_TODOHIS.LISTID = '" + this.ViewState["ListId"].ToString() + "') ORDER BY SYS_TODOHIS.UPDATE_DATE,SYS_TODOHIS.UPDATE_TIME";
            object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sql });
            if (ret1 != null && (int)ret1[0] == 0)
            {
                this.gdvHis.DataSource = ((DataSet)ret1[1]).Tables[0].DefaultView;
                this.gdvHis.DataBind();

                this.fvHis.DataSource = ((DataSet)ret1[1]).Tables[0].DefaultView;
                this.fvHis.DataBind();
            }
        }
    }

    protected void gdvHis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GenSource();
        this.gdvHis.PageIndex = e.NewPageIndex;
        this.gdvHis.DataBind();
        if (this.gdvHis.Rows.Count > 0)
        {
            string[] UIHisTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "SubmitConfirm", "HisUIText", true).Split(',');
            if (UIHisTexts.Length >= 7)
            {
                this.gdvHis.HeaderRow.Cells[0].Text = UIHisTexts[5];
                this.gdvHis.HeaderRow.Cells[1].Text = UIHisTexts[0];
                this.gdvHis.HeaderRow.Cells[2].Text = UIHisTexts[1];
                this.gdvHis.HeaderRow.Cells[3].Text = UIHisTexts[6];
                this.gdvHis.HeaderRow.Cells[4].Text = UIHisTexts[2];
                this.gdvHis.HeaderRow.Cells[5].Text = UIHisTexts[3];
                this.gdvHis.HeaderRow.Cells[6].Text = UIHisTexts[4];
                this.gdvHis.HeaderRow.Cells[7].Text = UIHisTexts[7];
            }
        }
    }
    protected void gdvHis_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string[] lstStatus = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLDesigner", "FLDesigner", "Item3", true).Split(',');
        if (e.Row.RowIndex != -1 && e.Row.RowType == DataControlRowType.DataRow)
        {
            //status
            string formatValue = "";
            switch (e.Row.Cells[3].Text)
            {
                case "Z":
                    formatValue = lstStatus[0];
                    break;
                case "N":
                    formatValue = lstStatus[1];
                    break;
                case "NR":
                    formatValue = lstStatus[2];
                    break;
                case "NF":
                    formatValue = lstStatus[3];
                    break;
                case "X":
                    formatValue = lstStatus[4];
                    break;
                case "A":
                case "AA":
                    formatValue = lstStatus[5];
                    break;
                case "V":
                    formatValue = lstStatus[6];
                    break;
            }
            e.Row.Cells[3].Text = formatValue;

            //attachments
            DataRowView rowView = e.Row.DataItem as DataRowView;
            if (rowView["ATTACHMENTS"] != null && rowView["ATTACHMENTS"].ToString() != "")
            {
                Panel pan = new Panel();
                pan.Style.Add(HtmlTextWriterStyle.Width, "100%");
                pan.Style.Add(HtmlTextWriterStyle.Height, "30px");
                pan.Style.Add("overflow-x", "hidden");
                pan.ScrollBars = ScrollBars.Vertical;

                string[] attachments = rowView["ATTACHMENTS"].ToString().Split(';');
                foreach (string attachment in attachments)
                {
                    HtmlAnchor a = new HtmlAnchor();
                    a.InnerHtml = attachment + "</br>";
                    a.Target = "_top";
                    a.HRef = "../WorkflowFiles/" + (isFlowFilesBySolutions() ? this.ViewState["VDSNAME"].ToString() + "/" : "") + attachment;

                    pan.Controls.Add(a);
                }
                e.Row.Cells[5].Controls.Add(pan);
            }
        }
    }
}
