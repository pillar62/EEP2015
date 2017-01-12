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
using System.Collections.Generic;

public partial class InnerPages_FlowPlus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ViewState["ListId"] = Request.QueryString["LISTID"];
            this.ViewState["FlowPath"] = Request.QueryString["FLOWPATH"];
            this.ViewState["Provider"] = Request.QueryString["PROVIDER"];
            this.ViewState["SendToId"] = Request.QueryString["SENDTOID"];
            this.ViewState["Keys"] = Request.QueryString["KEYS"];
            this.ViewState["Values"] = Request.QueryString["VALUES"].Replace("$$$", "''");
            this.ViewState["ATTACHMENTS"] = string.IsNullOrEmpty(Request.QueryString["ATTACHMENTS"]) ? "" : Request.QueryString["ATTACHMENTS"].TrimStart().TrimEnd();
            this.ViewState["ISIMPORTANT"] = Request.QueryString["ISIMPORTANT"];
            this.ViewState["ISURGENT"] = Request.QueryString["ISURGENT"];
            this.ViewState["PagePath"] = Request.QueryString["PAGEPATH"];
            this.ViewState["VDSNAME"] = Request.QueryString["VDSNAME"];

            string sqlRoles = "select GROUPID, GROUPNAME, ISROLE from GROUPS where ISROLE='Y'";
            if (Request.QueryString["SecGroups"] != null && Request.QueryString["SecGroups"] != "")
            {
                string[] secRoles = Request.QueryString["SecGroups"].Split(';');
                if (secRoles.Length > 0)
                {
                    sqlRoles += " and GROUPID in (";
                    for (int i = 0; i < secRoles.Length; i++)
                    {
                        if (i == secRoles.Length - 1)
                            sqlRoles += "'" + secRoles[i] + "'";
                        else
                            sqlRoles += "'" + secRoles[i] + "',";
                    }
                    sqlRoles += ")";
                }
            }

            object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sqlRoles });
            if (ret1 != null && (int)ret1[0] == 0)
            {
                DataTable tabRoles = ((DataSet)ret1[1]).Tables[0];
                DataColumn colDisRole = new DataColumn("DISROLE", typeof(string), "GROUPID+'('+GROUPNAME+')'");
                tabRoles.Columns.Add(colDisRole);
                this.lstRolesFrom.DataSource = tabRoles;
                this.lstRolesFrom.DataValueField = "GROUPID";
                this.lstRolesFrom.DataTextField = "DISROLE";
                this.lstRolesFrom.DataBind();
            }
        }
        GenDownLoadHref();
        string[] UITexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "NotifyForm", "UIText", true).Split(',');
        this.btnOk.Text = UITexts[5];
        this.btnCancel.Text = UITexts[6];
        this.btnGo.Text = UITexts[10];
    }

    public string getHtmlText(int index)
    {
        string[] UITexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "NotifyForm", "UIText", true).Split(',');
        return UITexts[index];
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
            //a.HRef = "../WorkflowFiles/" + attach;
            a.HRef = "../WorkflowFiles/" + (isFlowFilesBySolutions() ? this.ViewState["VDSNAME"].ToString() + "/" : "") + attach;
            cell.Controls.Add(a);
            row.Cells.Add(cell);
        }
        table.Rows.Add(row);
        this.panDownload.Controls.Add(table);
    }

    private bool isFlowFilesBySolutions()
    {
        string config_FilesBySol = ConfigurationManager.AppSettings["FlowFilesBySolutions"];
        if (!string.IsNullOrEmpty(config_FilesBySol) && string.Compare(config_FilesBySol, "true", true) == 0)
            return true;
        return false;
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        string listId = this.ViewState["ListId"].ToString();
        string flowPath = this.ViewState["FlowPath"].ToString();
        string provider = this.ViewState["Provider"].ToString();
        string sendToId = this.ViewState["SendToId"].ToString();
        int important = Convert.ToInt32(this.ViewState["ISIMPORTANT"]);
        int urgent = Convert.ToInt32(this.ViewState["ISURGENT"]);
        string keys = this.ViewState["Keys"].ToString();
        string values = this.ViewState["Values"].ToString();
        string roles = "";
        string attachments = this.ViewState["ATTACHMENTS"] == null ? "" : this.ViewState["ATTACHMENTS"].ToString();
        foreach (ListItem item in this.lstRolesTo.Items)
        {
            roles += item.Value + ";";
        }
        if (string.IsNullOrEmpty(roles)) return;
        this.panResult.Visible = true;
        this.btnOk.Enabled = false;
        this.btnCancel.Enabled = false;

        object[] objParams = CliUtils.CallFLMethod("PlusApprove", new object[] { new Guid(listId), new object[] { flowPath, flowPath, important, urgent, this.txtMessage.Text, sendToId, provider, 0, roles, attachments }, new object[] { keys, values } });
        if (Convert.ToInt16(objParams[0]) == 0)
        {
            string sendToIds = roles.Substring(0, roles.LastIndexOf(';'));
            this.result.Text = FLTools.GloFix.ShowPlusMessage(sendToIds);
            string ref_script = "var lnk_element=window.opener.document.getElementById('lnkRefresh');if(lnk_element){window.opener.__doPostBack('lnkRefresh','');}else{lnk_element=window.opener.parent.document.getElementById('lnkRefresh');if(lnk_element){window.opener.parent.__doPostBack('lnkRefresh','');}}";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "", ref_script + "window.opener.location.reload('" + this.ViewState["PagePath"].ToString() + "?&NAVMODE=2&FLNAVMODE=8');", true);
        }
        else
        {
            if (Convert.ToInt16(objParams[0]) == 2)
                this.result.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLTools", "GloFix", "FailToPlus", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Write("<script>window.close();</script>");
    }
    protected void btnLRRoles_Click(object sender, EventArgs e)
    {
        MoveListItems(lstRolesFrom, lstRolesTo);
    }
    protected void btnRLRoles_Click(object sender, EventArgs e)
    {
        MoveListItems(lstRolesTo, lstRolesFrom);
    }

    private void MoveListItems(ListBox lstFrom, ListBox lstTo)
    {
        List<ListItem> lstToDelete = new List<ListItem>();
        foreach (ListItem item in lstFrom.Items)
        {
            if (item.Selected)
            {
                lstTo.Items.Add(item);
                lstToDelete.Add(item);
            }
        }
        foreach (ListItem item in lstToDelete)
        {
            lstFrom.Items.Remove(item);
        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {

        this.lstRolesFrom.Items.Clear();

        string sqlRoles = "select GROUPID, GROUPNAME, ISROLE from GROUPS where ISROLE='Y'";
        if (Request.QueryString["SecGroups"] != null && Request.QueryString["SecGroups"] != "")
        {
            string[] secRoles = Request.QueryString["SecGroups"].Split(';');
            if (secRoles.Length > 0)
            {
                sqlRoles += " and GROUPID in (";
                for (int i = 0; i < secRoles.Length; i++)
                {
                    if (i == secRoles.Length - 1)
                        sqlRoles += "'" + secRoles[i] + "'";
                    else
                        sqlRoles += "'" + secRoles[i] + "',";
                }
                sqlRoles += ")";
            }
        }
        if (!string.IsNullOrEmpty(this.txtSearchRoleId.Text))
        {
            sqlRoles += string.Format(" and groupid like'%{0}%'", this.txtSearchRoleId.Text);
        }
        if (!string.IsNullOrEmpty(this.txtSearchRoleName.Text))
        {
            sqlRoles += string.Format(" and groupname like '%{0}%'", this.txtSearchRoleName.Text);
        }
        object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sqlRoles });
        if (ret1 != null && (int)ret1[0] == 0)
        {
            DataTable tabRoles = ((DataSet)ret1[1]).Tables[0];
            DataColumn colDisRole = new DataColumn("DISROLE", typeof(string), "GROUPID+'('+GROUPNAME+')'");
            tabRoles.Columns.Add(colDisRole);
            this.lstRolesFrom.DataSource = tabRoles;
            this.lstRolesFrom.DataValueField = "GROUPID";
            this.lstRolesFrom.DataTextField = "DISROLE";
            this.lstRolesFrom.DataBind();
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.close();", true);
    }
}
