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

public partial class InnerPages_FlowNotify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ViewState["ListId"] = Request.QueryString["LISTID"];
            this.ViewState["FlowPath"] = Request.QueryString["FLOWPATH"];
            this.ViewState["Provider"] = Request.QueryString["PROVIDER"];
            this.ViewState["Keys"] = Request.QueryString["KEYS"];
            this.ViewState["Values"] = Request.QueryString["VALUES"].Replace("$$$", "''");

            string sqlUsers = "select USERID, USERNAME from USERS";
            if (Request.QueryString["SecUsers"] != null && Request.QueryString["SecUsers"] != "")
            {
                string[] secUsers = Request.QueryString["SecUsers"].Split(';');
                if (secUsers.Length > 0)
                {
                    sqlUsers += " where USERID in (";
                    for (int i = 0; i < secUsers.Length; i++)
                    {
                        if (i == secUsers.Length - 1)
                            sqlUsers += "'" + secUsers[i] + "'";
                        else
                            sqlUsers += "'" + secUsers[i] + "',";
                    }
                    sqlUsers += ")";
                }
            }

            object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sqlUsers });
            if (ret1 != null && (int)ret1[0] == 0)
            {
                DataTable tabUsers = ((DataSet)ret1[1]).Tables[0];
                DataColumn colDisUser = new DataColumn("DISUSER", typeof(string), "USERID+'('+USERNAME+')'");
                tabUsers.Columns.Add(colDisUser);
                this.lstUsersFrom.DataSource = tabUsers;
                this.lstUsersFrom.DataValueField = "USERID";
                this.lstUsersFrom.DataTextField = "DISUSER";
                this.lstUsersFrom.DataBind();
            }

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

            object[] ret2 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sqlRoles });
            if (ret2 != null && (int)ret2[0] == 0)
            {
                DataTable tabRoles = ((DataSet)ret2[1]).Tables[0];
                DataColumn colDisRole = new DataColumn("DISROLE", typeof(string), "GROUPID+'('+GROUPNAME+')'");
                tabRoles.Columns.Add(colDisRole);
                this.lstRolesFrom.DataSource = tabRoles;
                this.lstRolesFrom.DataValueField = "GROUPID";
                this.lstRolesFrom.DataTextField = "DISROLE";
                this.lstRolesFrom.DataBind();
            }
        }
        string[] UITexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "NotifyForm", "UIText", true).Split(',');
        this.btnOk.Text = UITexts[5];
        this.btnCancel.Text = UITexts[6];
        this.btnUserGo.Text = UITexts[10];
        this.btnRoleGo.Text = UITexts[10];
    }

    public string getHtmlText(int index)
    {
        string[] UITexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "NotifyForm", "UIText", true).Split(',');
        return UITexts[index];
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        string listId = this.ViewState["ListId"].ToString();
        string flowPath = this.ViewState["FlowPath"].ToString();
        string provider = this.ViewState["Provider"].ToString();
        string keys = this.ViewState["Keys"].ToString();
        string values = this.ViewState["Values"].ToString();
        string users = "", roles = "";
        foreach (ListItem item in this.lstUsersTo.Items)
        {
            users += item.Value + ":UserId;";
        }
        foreach (ListItem item in this.lstRolesTo.Items)
        {
            roles += item.Value + ";";
        }
        if (string.IsNullOrEmpty(users) && string.IsNullOrEmpty(roles))
            return;

        this.panResult.Visible = true;
        this.btnOk.Enabled = false;
        this.btnCancel.Enabled = false;

        object[] objParams = CliUtils.CallFLMethod("Notify", new object[] { new Guid(listId), new object[] { flowPath, flowPath, 0, 0, this.txtMessage.Text, "", provider, 0, users + roles, "" }, new object[] { keys, values } });
        if (Convert.ToInt16(objParams[0]) == 0)
        {
            string sendToIds = users + roles;
            sendToIds = sendToIds.Substring(0, sendToIds.LastIndexOf(';'));
            this.result.Text = this.ShowNotifyMessage(sendToIds);
        }
        else
        {
            if (Convert.ToInt16(objParams[0]) == 2)
                this.result.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLTools", "GloFix", "FailToNotify", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Write("<script>window.close();</script>");
    }

    protected void btnLRUsers_Click(object sender, EventArgs e)
    {
        MoveListItems(lstUsersFrom, lstUsersTo);
    }

    protected void btnRLUsers_Click(object sender, EventArgs e)
    {
        MoveListItems(lstUsersTo, lstUsersFrom);
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

    public string ShowNotifyMessage(string sendToIds)
    {
        List<string> sendToUsers = new List<string>();
        List<string> sendToRoles = new List<string>();
        string[] toIds = sendToIds.Split(';');
        for (int i = 0; i < toIds.Length; i++)
        {
            if (toIds[i].IndexOf(':') != -1)
            {
                sendToUsers.Add(toIds[i].Substring(0, toIds[i].IndexOf(':')));
            }
            else
            {
                sendToRoles.Add(toIds[i]);
            }
        }

        Hashtable dicGroups = new Hashtable();
        Hashtable dicUsers = new Hashtable();

        // 1.取出所有的RoleId和RoleName及Role以下的UserID填入sendToUsers
        string sqlRoles = "";
        foreach (string role in sendToRoles)
        {
            sqlRoles += "'" + role + "',";
        }
        if (sqlRoles != "")
        {
            sqlRoles = sqlRoles.Substring(0, sqlRoles.LastIndexOf(','));
            DataTable tGroups = null;

            object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { "select GROUPID, GROUPNAME, ISROLE from GROUPS where GROUPID in(" + sqlRoles + ") and ISROLE='Y'" });
            if (ret1 != null && (int)ret1[0] == 0)
            {
                tGroups = ((DataSet)ret1[1]).Tables[0];
            }
            foreach (DataRow row in tGroups.Rows)
            {
                dicGroups.Add(row["GROUPID"].ToString(), row["GROUPNAME"].ToString());

                DataTable tHoldUsers = null;

                object[] ret2 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { "select USERID, USERNAME from USERS where USERID in (select USERID from USERGROUPS where GROUPID='" + row["GROUPID"].ToString() + "')" });
                if (ret2 != null && (int)ret2[0] == 0)
                {
                    tHoldUsers = ((DataSet)ret2[1]).Tables[0];
                }

                if (tHoldUsers != null && tHoldUsers.Rows.Count > 0)
                {
                    foreach (DataRow holdRow in tHoldUsers.Rows)
                    {
                        sendToUsers.Add(holdRow["USERID"].ToString());
                    }
                }
            }
        }
        // 2.取出所有的UserId和UserName
        string sqlUsers = "";
        foreach (string user in sendToUsers)
        {
            sqlUsers += "'" + user + "',";
        }
        if (sqlUsers != "")
        {
            sqlUsers = sqlUsers.Substring(0, sqlUsers.LastIndexOf(','));
            DataTable tUsers = null;

            object[] ret3 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { "select USERID, USERNAME from USERS where USERID in(" + sqlUsers + ")" });
            if (ret3 != null && (int)ret3[0] == 0)
            {
                tUsers = ((DataSet)ret3[1]).Tables[0];
            }
            foreach (DataRow row in tUsers.Rows)
            {
                dicUsers.Add(row["USERID"].ToString(), row["USERNAME"].ToString());
            }
        }
        // 3.组Message
        string allRoles = "";
        IEnumerator enumerRoles = dicGroups.GetEnumerator();
        while (enumerRoles.MoveNext())
        {
            allRoles += ((DictionaryEntry)enumerRoles.Current).Key.ToString() + "(" + ((DictionaryEntry)enumerRoles.Current).Value.ToString() + ") ";
        }
        if (allRoles != "")
            allRoles = allRoles.Substring(0, allRoles.LastIndexOf(' '));

        string allUsers = "";
        IEnumerator enumerUsers = dicUsers.GetEnumerator();
        while (enumerUsers.MoveNext())
        {
            allUsers += ((DictionaryEntry)enumerUsers.Current).Key.ToString() + "(" + ((DictionaryEntry)enumerUsers.Current).Value.ToString() + ") ";
        }
        if (allUsers != "")
            allUsers = allUsers.Substring(0, allUsers.LastIndexOf(' '));

        string message = "";
        string m = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLWizard", "NotifyUsers", true);
        message += string.Format(m, allRoles, allUsers);

        return message;
    }

    protected void btnRoleGo_Click(object sender, EventArgs e)
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
            sqlRoles += string.Format(" and groupid like '%{0}%'", this.txtSearchRoleId.Text);
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

    protected void btnUserGo_Click(object sender, EventArgs e)
    {
        this.lstUsersFrom.Items.Clear();

        string sqlUsers = "select USERID, USERNAME from USERS";
        if (Request.QueryString["SecUsers"] != null && Request.QueryString["SecUsers"] != "")
        {
            string[] secUsers = Request.QueryString["SecUsers"].Split(';');
            if (secUsers.Length > 0)
            {
                sqlUsers += " where USERID in (";
                for (int i = 0; i < secUsers.Length; i++)
                {
                    if (i == secUsers.Length - 1)
                        sqlUsers += "'" + secUsers[i] + "'";
                    else
                        sqlUsers += "'" + secUsers[i] + "',";
                }
                sqlUsers += ")";
            }
        }
        if (!string.IsNullOrEmpty(this.txtSearchUserId.Text))
        {
            if (sqlUsers.IndexOf(" where ") != -1)
            {
                sqlUsers += string.Format(" and userid like '%{0}%'", this.txtSearchUserId.Text);
            }
            else
            {
                sqlUsers += string.Format(" where userid like '%{0}%'", this.txtSearchUserId.Text);
            }
        }
        if (!string.IsNullOrEmpty(this.txtSearchUserName.Text))
        {
            if (sqlUsers.IndexOf(" where ") != -1)
            {
                sqlUsers += string.Format(" and username like '%{0}%'", this.txtSearchUserName.Text);
            }
            else
            {
                sqlUsers += string.Format(" where username like '%{0}%'", this.txtSearchUserName.Text);
            }
        }
        object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sqlUsers });
        if (ret1 != null && (int)ret1[0] == 0)
        {
            DataTable tabUsers = ((DataSet)ret1[1]).Tables[0];
            DataColumn colDisUser = new DataColumn("DISUSER", typeof(string), "USERID+'('+USERNAME+')'");
            tabUsers.Columns.Add(colDisUser);
            this.lstUsersFrom.DataSource = tabUsers;
            this.lstUsersFrom.DataValueField = "USERID";
            this.lstUsersFrom.DataTextField = "DISUSER";
            this.lstUsersFrom.DataBind();
        }
    }
}
