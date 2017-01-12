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
using System.IO;
using Srvtools;
using System.Collections.Generic;

public partial class InnerPages_frmApproveAll : System.Web.UI.Page
{
    DataTable sDataTable = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        String[] UITexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "SubmitConfirm", "UIText", true).Split(',');
        this.chkImportant.Text = UITexts[1];
        this.chkUrgent.Text = UITexts[2];
        this.mvCaption.Captions[0].Caption = UITexts[3];
        //this.mvCaption.Captions[1].Caption = UITexts[4];
        this.lblRole.Text = UITexts[5];
        this.btnOk.Text = UITexts[6];
        this.btnCancel.Text = UITexts[7];
        this.lblOrg.Text = UITexts[9];
        this.lblReturnStep.Text = UITexts[10];
        this.upload.Text = UITexts[12];
        this.btnPreview.Text = UITexts[14];

        if (!IsPostBack)
        {
            InitDataTable();
            int count = 0;

            while (this.Page.Request.Cookies["FlowApproveAll" + count.ToString()] != null && this.Page.Request.Cookies["FlowApproveAll" + count.ToString()].Value != null)
            {
                HttpCookie mycookie = this.Page.Request.Cookies["FlowApproveAll" + count.ToString()];
                String strFlowApproveAll = mycookie.Value;
                if (!strFlowApproveAll.Contains("&"))
                    break;
                strFlowApproveAll = this.Server.UrlDecode(strFlowApproveAll);

                DataRow dr = sDataTable.NewRow();
                InitDataRow(strFlowApproveAll, dr);

                if (dr["ATTACHMENTS"] == null || dr["ATTACHMENTS"].ToString() == String.Empty)
                {
                    if (dr["LISTID"] != null && dr["LISTID"].ToString() != String.Empty)
                    {
                        string sqlatt = string.Format("select ATTACHMENTS from SYS_TODOLIST where LISTID = '{0}' AND STATUS NOT IN ('F','A','AA')", dr["LISTID"].ToString(), dr["FlowPath"].ToString());//樓腔flowpath祥淏,蕉藉裁flowpath

                        object[] retatt = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sqlatt });
                        if (retatt != null && (int)retatt[0] == 0)
                        {
                            DataTable table = ((DataSet)retatt[1]).Tables[0];
                            if (table.Rows.Count > 0)
                            {
                                dr["ATTACHMENTS"] = table.Rows[0][0];
                            }
                        }
                    }
                    else
                    {
                        dr["ATTACHMENTS"] = string.Empty;
                    }
                }

                if (dr["Values"] != null || dr["Values"].ToString() != String.Empty)
                    dr["Values"] = dr["Values"].ToString().Replace("$$$", "''");

                dr["Important"] = GetValueFromFlowApproveAll(strFlowApproveAll, "uploadParam1");
                dr["Urgent"] = GetValueFromFlowApproveAll(strFlowApproveAll, "uploadParam2");
                dr["Suggestion"] = GetValueFromFlowApproveAll(strFlowApproveAll, "uploadParam3");
                dr["Role"] = GetValueFromFlowApproveAll(strFlowApproveAll, "uploadParam4");
                dr["Org"] = GetValueFromFlowApproveAll(strFlowApproveAll, "uploadParam5");
                dr["ReturnStep"] = GetValueFromFlowApproveAll(strFlowApproveAll, "uploadParam6");

                string curTime = FLTools.GloFix.DateTimeString(DateTime.Now);
                string curUser = CliUtils.fLoginUser;
                string flowDesc = "";
                switch (dr["OperateType"].ToString())
                {
                    case "Submit":
                        flowDesc = FLTools.GloFix.GetFlowDesc(dr["FlowFileName"].ToString() + ".xoml", false);
                        break;
                    case "Approve":
                    case "Return":
                        flowDesc = FLTools.GloFix.GetFlowDesc(dr["ListId"].ToString(), true);
                        break;
                }
                this.btnPreview.Enabled = (dr["OperateType"].ToString() != "Return");
                string sql = "SELECT GROUPID,GROUPNAME FROM GROUPS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + curUser + "')  AND ISROLE='Y' UNION SELECT ROLE_ID AS GROUPID,GROUPS.GROUPNAME  FROM SYS_ROLES_AGENT LEFT JOIN GROUPS ON SYS_ROLES_AGENT.ROLE_ID=GROUPS.GROUPID WHERE (SYS_ROLES_AGENT.FLOW_DESC='*' OR SYS_ROLES_AGENT.FLOW_DESC='" + flowDesc + "') AND AGENT='" + curUser + "' AND START_DATE+START_TIME<='" + curTime + "' AND END_DATE+END_TIME>='" + curTime + "'";
                object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sql });
                if (ret1 != null && (int)ret1[0] == 0)
                {
                    if (this.ddlRoles.DataSource == null)
                    {
                        this.ddlRoles.DataSource = ((DataSet)ret1[1]).Tables[0];
                        this.ddlRoles.DataValueField = "GROUPID";
                        this.ddlRoles.DataTextField = "GROUPNAME";
                        this.ddlRoles.DataBind();
                        if (dr["SENDTOID"] != null && dr["SENDTOID"].ToString() != "")
                        {
                            this.ddlRoles.SelectedValue = dr["SENDTOID"].ToString();
                        }
                    }
                }
                if (CliUtils.fFlowSelectedRole != "")
                    this.ddlRoles.SelectedValue = CliUtils.fFlowSelectedRole;
                if (dr["OperateType"].ToString() == "Submit" && dr["ORGCONTROL"] != null && dr["ORGCONTROL"].ToString() == "True")
                {
                    this.lblOrg.Visible = this.ddlOrg.Visible = true;
                    sql = "select * from SYS_ORGKIND";
                    object[] ret2 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sql });
                    if (ret2 != null && (int)ret2[0] == 0)
                    {
                        this.ddlOrg.DataSource = ((DataSet)ret2[1]).Tables[0];
                        this.ddlOrg.DataValueField = "ORG_KIND";
                        this.ddlOrg.DataTextField = "KIND_DESC";
                        this.ddlOrg.DataBind();
                    }
                }
                else
                    this.lblOrg.Visible = this.ddlOrg.Visible = false;

                if (dr["OperateType"].ToString() == "Return" && dr["MULTISTEPRETURN"].ToString() == "1")
                {
                    this.lblReturnStep.Visible = this.ddlReturnStep.Visible = true;
                    object[] objParams = CliUtils.CallFLMethod("GetFLPathList", new object[] { new Guid(dr["ListId"].ToString()) });
                    if (Convert.ToInt16(objParams[0]) == 0 && objParams[1] != null)
                    {
                        List<string> src = new List<string>();
                        src.Add("[" + UITexts[11] + "]");
                        src.AddRange((string[])objParams[1]);
                        this.ddlReturnStep.DataSource = src;
                        this.ddlReturnStep.DataBind();
                    }
                    else
                    {
                        this.lblReturnStep.Visible = this.ddlReturnStep.Visible = false;
                        dr["MULTISTEPRETURN"] = "0";
                    }
                }
                //else
                    this.lblReturnStep.Visible = this.ddlReturnStep.Visible = false;

                if (dr["IsImportant"] != null && dr["IsImportant"].ToString() == "1")
                {
                    this.chkImportant.Checked = true;
                }
                if (dr["IsUrgent"] != null && dr["IsUrgent"].ToString() == "1")
                {
                    this.chkUrgent.Checked = true;
                }

                // back from upload
                if (dr["Important"] != null)
                {
                    if (dr["Important"].ToString() == "false")
                        this.chkImportant.Checked = false;
                    else if (dr["Important"].ToString() == "true")
                        this.chkImportant.Checked = true;
                }
                if (dr["Urgent"] != null)
                {
                    if (dr["Urgent"].ToString() == "false")
                        this.chkUrgent.Checked = false;
                    else if (dr["Urgent"].ToString() == "true")
                        this.chkUrgent.Checked = true;
                }
                if (dr["Suggestion"] != null)
                    this.txtSuggest.Text = dr["Suggestion"].ToString();
                if (dr["Role"] != null && dr["Role"].ToString() != String.Empty)
                    this.ddlRoles.SelectedIndex = Convert.ToInt32(dr["Role"]);
                if (dr["Org"] != null && dr["Org"].ToString() != String.Empty)
                    this.ddlOrg.SelectedIndex = Convert.ToInt32(dr["Org"]);
                if (dr["ReturnStep"] != null && dr["ReturnStep"].ToString() != String.Empty)
                    this.ddlReturnStep.SelectedIndex = Convert.ToInt32(dr["ReturnStep"]);

                sDataTable.Rows.Add(dr);

                TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                mycookie.Expires = DateTime.Now.Add(ts);//立即过期 
                Response.Cookies.Remove("FlowApproveAll" + count.ToString());//清除 
                Response.Cookies.Add(mycookie);//写入立即过期的*/
                Response.Cookies["FlowApproveAll" + count.ToString()].Expires = DateTime.Now.AddDays(-1); count++;
            }
            //this.GenSource();
            //this.SetHeaderText();
            this.ViewState["FlowTable"] = sDataTable;
        }


        //GenDownLoadHref();

        //string remarkField = this.Request.QueryString["RemarkField"];
        //if (!string.IsNullOrEmpty(remarkField))
        //{
        //    DataSet host = (DataSet)Session["PreviewHost"];
        //    if (host != null && host.Tables.Count > 0)
        //    {
        //        DataTable tableHost = host.Tables[0];
        //        if (tableHost.Rows.Count > 0)
        //        {
        //            this.txtSuggest.Text = tableHost.Rows[0][remarkField].ToString();
        //        }
        //    }
        //}
        
    }

    private void InitDataTable()
    {
        sDataTable.Columns.Add(new DataColumn("FlowFileName", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("PagePath", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("NavMode", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("FLNavMode", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("PLUSAPPROVE", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("ORGCONTROL", typeof(String)));

        sDataTable.Columns.Add(new DataColumn("ListId", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("FlowPath", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("IsImportant", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("IsUrgent", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("STATUS", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("MULTISTEPRETURN", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("ATTACHMENTS", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("SENDTOID", typeof(String)));

        sDataTable.Columns.Add(new DataColumn("OperateType", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("Keys", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("Values", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("Provider", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("VDSNAME", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("Important", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("Urgent", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("Suggestion", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("Role", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("Org", typeof(String)));
        sDataTable.Columns.Add(new DataColumn("ReturnStep", typeof(String)));
    }

    private void InitDataRow(String strFlowApproveAll, DataRow dr)
    {
        foreach (DataColumn dc in dr.Table.Columns)
        {
            dr[dc.ColumnName] = GetValueFromFlowApproveAll(strFlowApproveAll, dc.ColumnName);
        }
    }

    private String GetValueFromFlowApproveAll(String strFlowApproveAll, String columnName)
    {
        String[] allString = strFlowApproveAll.Split('&');
        foreach (String param in allString)
        {
            if (!String.IsNullOrEmpty(param))
            {
                String[] values = param.Split('=');
                if (values[0].ToUpper() == columnName.ToUpper())
                {
                    if (columnName == "Values" && values.Length > 2)
                    {
                        return String.Format("{0}={1}", values[1], values[2]);
                    }
                    return values[1];
                }
            }
        }
        return String.Empty;
    }

    protected void mvCaption_TabChanging(object sender, Srvtools.TabChangingEventArgs e)
    {
        this.upload.Visible = (e.NewActiveIndex == 0);
    }

    protected void gdvHis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GenSource();
        this.gdvHis.PageIndex = e.NewPageIndex;
        this.gdvHis.DataBind();
        string[] UIHisTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "SubmitConfirm", "HisUIText", true).Split(',');

        if (UIHisTexts.Length >= 6 && this.gdvHis.HeaderRow != null)
        {
            this.gdvHis.HeaderRow.Cells[0].Text = UIHisTexts[5];
            this.gdvHis.HeaderRow.Cells[1].Text = UIHisTexts[0];
            this.gdvHis.HeaderRow.Cells[2].Text = UIHisTexts[1];
            this.gdvHis.HeaderRow.Cells[3].Text = UIHisTexts[6];
            this.gdvHis.HeaderRow.Cells[4].Text = UIHisTexts[2];
            this.gdvHis.HeaderRow.Cells[5].Text = UIHisTexts[3];
            this.gdvHis.HeaderRow.Cells[6].Text = UIHisTexts[4];
        }
    }

    protected void gdvHis_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string[] lstStatus = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLDesigner", "FLDesigner", "Item3", true).Split(',');

        if (e.Row.RowIndex != -1 && e.Row.RowType == DataControlRowType.DataRow)
        {
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
                    formatValue = lstStatus[5];
                    break;
                case "V":
                    formatValue = lstStatus[6];
                    break;
            }
            e.Row.Cells[3].Text = formatValue;
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        sDataTable = this.ViewState["FlowTable"] as DataTable;
        foreach (DataRow dr in sDataTable.Rows)
        {
            CliUtils.fFlowSelectedRole = this.ddlRoles.SelectedValue;
            string suggest = this.txtSuggest.Text, role = this.ddlRoles.SelectedValue, provider = dr["Provider"].ToString(), keys = dr["Keys"].ToString(), values = dr["Values"].ToString(), operate = dr["OperateType"].ToString();
            int isImport = Convert.ToInt16(dr["IsImportant"]), isUrgent = Convert.ToInt16(dr["IsUrgent"]);
            string[] fLActivities = null;
            if (dr["FlowPath"] != null)
                fLActivities = dr["FlowPath"].ToString().Split(';');
            string mailAddress = GetMailAddress();
            this.panResult.Visible = true;
            this.btnOk.Enabled = false;
            this.btnCancel.Enabled = false;
            this.upload.Enabled = false;
            string attachments = dr["ATTACHMENTS"] == null ? "" : dr["ATTACHMENTS"].ToString();
            string ref_script = "var lnk_element=window.opener.document.getElementById('lnkRefresh');if(lnk_element){window.opener.__doPostBack('lnkRefresh','');}else{lnk_element=window.opener.parent.document.getElementById('lnkRefresh');if(lnk_element){window.opener.parent.__doPostBack('lnkRefresh','');}}";
            string Param = Request.QueryString["UserParam"];
            System.Text.StringBuilder userParameter = new System.Text.StringBuilder();
            if (!string.IsNullOrEmpty(Param))
            {
                string[] listParam = Param.Split(';');
                foreach (string par in listParam)
                {
                    if (!string.IsNullOrEmpty(par))
                    {
                        string[] keyvalues = par.Split('^');
                        if (keyvalues.Length == 2)
                        {
                            userParameter.Append("&");
                            userParameter.Append(keyvalues[0]);
                            userParameter.Append("=");
                            userParameter.Append(keyvalues[1]);
                        }
                    }
                }
            }


            switch (operate)
            {
                case "Submit":
                    string org = "";
                    if (dr["ORGCONTROL"] != null && dr["ORGCONTROL"].ToString() == "True")
                    {
                        org = this.ddlOrg.SelectedValue;
                    }
                    object[] objParams = CliUtils.CallFLMethod("Submit", new object[] { null, new object[] { dr["FlowFileName"].ToString() + ".xoml", "", isImport, isUrgent, suggest, role, provider, mailAddress, org, attachments }, new object[] { keys, values } });
                    if (Convert.ToInt16(objParams[0]) == 0)
                    {
                        if (objParams[1].ToString() == "512F4277-0D41-441c-BF16-D96B04580C2E")
                        {
                            InitResultText(SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLNavigator", "HasRejected", true));
                            return;
                        }
                        else if (objParams[1].ToString() == "60585C77-60E1-4e6f-A2E2-3BBBAD6B4C9E")
                        {
                            InitResultText(SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLNavigator", "RunOver", true));
                            if (dr["PagePath"] != null)
                            {
                                this.ClientScript.RegisterStartupScript(typeof(string), string.Empty, "window.opener.location.reload('" + dr["PagePath"].ToString() + "?&NAVMODE=0&FLNAVMODE=6');", true);
                            }

                            return;
                        }
                        else
                        {
                            this.send.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLWizard", "Send", true);
                            InitResultText(FLTools.GloFix.ShowMessage(objParams[1].ToString(), true));
                        }
                        this.ClientScript.RegisterStartupScript(typeof(string), string.Empty, "window.opener.location.reload('" + dr["PagePath"].ToString() +
                            "?FLOWFILENAME=" + HttpUtility.UrlEncode(dr["FlowFileName"].ToString()) +
                            "&FLNAVMODE=" + dr["FLNavMode"].ToString() +
                            "&PLUSAPPROVE=" + dr["PLUSAPPROVE"].ToString() +
                            "&LISTID=" + objParams[2].ToString() +
                            "&FLOWPATH=" + HttpUtility.UrlEncode(objParams[3].ToString()) + ";" + HttpUtility.UrlEncode(objParams[3].ToString()) + userParameter.ToString() +
                            "');", true);
                        btnClose.OnClientClick = "window.close();return false;";
                    }
                    else
                    {
                        if (Convert.ToInt16(objParams[0]) == 2)
                            InitResultText(objParams[1].ToString());
                        btnClose.OnClientClick = ref_script + "window.close();return false;";
                    }
                    break;
                case "Approve":
                    if (dr["STATUS"].ToString() == "A" || dr["STATUS"].ToString() == "AA")
                    {
                        objParams = CliUtils.CallFLMethod("PlusReturn", new object[] { new Guid(dr["ListId"].ToString()), new object[] { fLActivities[0], fLActivities[1], isImport, isUrgent, suggest, role, provider, mailAddress, "", attachments }, new object[] { keys, values } });
                    }
                    else
                    {
                        objParams = CliUtils.CallFLMethod("Approve", new object[] { new Guid(dr["ListId"].ToString()), new object[] { fLActivities[0], fLActivities[1], isImport, isUrgent, suggest, role, provider, mailAddress, "", attachments }, new object[] { keys, values } });
                    }
                    if (Convert.ToInt16(objParams[0]) == 0)
                    {
                        if (objParams[1].ToString() == "60585C77-60E1-4e6f-A2E2-3BBBAD6B4C9E")
                        {
                            InitResultText(SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLNavigator", "RunOver", true));
                        }
                        else if (objParams[1].ToString() == "512F4277-0D41-441c-BF16-D96B04580C2E")
                        {
                            InitResultText( SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLNavigator", "HasRejected", true));
                        }
                        else
                        {
                            //this.send.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLWizard", "Send");
                            //this.result.Text = FLTools.GloFix.ShowMessage(objParams[1].ToString(), true);
                            if (objParams[1].ToString() == "")
                            {
                                string wait = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLTools", "GloFix", "WaitMessage", true);
                                string sql = "select SENDTO_KIND, SENDTO_ID from SYS_TODOLIST where LISTID='" + dr["ListId"].ToString() + "' and STATUS <> 'F'";
                                DataTable dtOthers = null;

                                object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sql });
                                if (ret1 != null && (int)ret1[0] == 0)
                                {
                                    dtOthers = ((DataSet)ret1[1]).Tables[0];
                                }
                                string sendToIds = "";
                                foreach (DataRow row in dtOthers.Rows)
                                {
                                    string sendtokind = row["SENDTO_KIND"].ToString();
                                    string sendtoid = row["SENDTO_ID"].ToString();
                                    if (sendtokind == "1")
                                    {
                                        sendToIds += sendtoid + ";";
                                    }
                                    else if (sendtokind == "2")
                                    {
                                        sendToIds += sendtoid + ":UserId;";
                                    }
                                }
                                if (sendToIds != "")
                                {
                                    InitResultText(FLTools.GloFix.ShowParallelMessage(sendToIds));
                                }
                            }
                            else
                            {
                                this.send.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLWizard", "Send", true);
                                InitResultText(FLTools.GloFix.ShowMessage(objParams[1].ToString(), true));
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt16(objParams[0]) == 2)
                            InitResultText(objParams[1].ToString());
                    }
                    if (dr["PagePath"] != null && dr["PagePath"].ToString() != string.Empty)
                    {
                        this.ClientScript.RegisterStartupScript(typeof(string), string.Empty, ref_script + "window.opener.location.reload('" + dr["PagePath"].ToString() + "?&NAVMODE=0&FLNAVMODE=6" + userParameter.ToString() + "');", true);
                    }
                    else
                    {
                        this.ClientScript.RegisterStartupScript(typeof(string), string.Empty, ref_script, true);
                    }
                    btnClose.OnClientClick = "window.close();return false;";

                    break;
                case "Return":
                    if (dr["MULTISTEPRETURN"].ToString() == "0")
                    {
                        objParams = CliUtils.CallFLMethod("Return", new object[] { new Guid(dr["ListId"].ToString()), new object[] { fLActivities[0], fLActivities[1], isImport, isUrgent, suggest, role, provider, mailAddress, "", attachments }, new object[] { keys, values } });
                    }
                    else if (dr["MULTISTEPRETURN"].ToString() == "1")
                    {
                        //if (this.ddlReturnStep.SelectedIndex == 0)
                        //{
                            objParams = CliUtils.CallFLMethod("Return", new object[] { new Guid(dr["ListId"].ToString()), new object[] { fLActivities[0], fLActivities[1], isImport, isUrgent, suggest, role, provider, 0, "", attachments }, new object[] { keys, values } });
                        //}
                        //else
                        //{
                        //    string retToStep = this.ddlReturnStep.Text;
                        //    objParams = CliUtils.CallFLMethod("Return2", new object[] { new Guid(dr["ListId"].ToString()), new object[] { retToStep, fLActivities[1], isImport, isUrgent, suggest, role, provider, 0, "", attachments }, new object[] { keys, values } });
                        //}
                    }
                    else return;

                    if (Convert.ToInt16(objParams[0]) == 0)
                    {
                        if (objParams[1].ToString() == "B4DAF3A4-AAE8-4b51-A391-B52E46305E9F")
                        {
                            InitResultText(SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLTools", "FLWebNavigator", "ReturnToEnd", true));
                        }
                        else
                        {
                            if (objParams[1].ToString().StartsWith("C912D847-1825-458a-8CB5-E680FACA42AF"))
                            {
                                InitResultText(FLTools.GloFix.ShowMessage3(objParams[1].ToString().Split(':')[1], true));
                            }
                            else
                            {
                                this.send.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLWizard", "Send", true);
                                InitResultText(FLTools.GloFix.ShowMessage(objParams[1].ToString(), true));
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt16(objParams[0]) == 2)
                            InitResultText(objParams[1].ToString());
                    }
                    if (dr["PagePath"] != null && dr["PagePath"].ToString() != string.Empty)
                    {
                        this.ClientScript.RegisterStartupScript(typeof(string), string.Empty, ref_script + "window.opener.location.reload('" + dr["PagePath"].ToString() + "?&NAVMODE=0&FLNAVMODE=6" + userParameter.ToString() + "');", true);
                    }
                    else
                    {
                        this.ClientScript.RegisterStartupScript(typeof(string), string.Empty, ref_script, true);
                    }
                    btnClose.OnClientClick = "window.close();return false;";
                    break;
            }
        }
    }

    private void InitResultText(String text)
    {
        if (!String.IsNullOrEmpty(this.result.Text))
            this.result.Text += "\r\n" + text;
        else
            this.result.Text = text;
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=javascript>window.close();</script>");
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string operate = this.ViewState["OperateType"].ToString();
        string keys = this.ViewState["Keys"].ToString();
        string values = this.ViewState["Values"].ToString();

        Guid id = operate == "Approve" ? new Guid(this.ViewState["ListId"].ToString()) : Guid.Empty;
        string activityname = operate == "Approve" ? this.ViewState["FlowPath"].ToString().Split(';')[1] : "";
        string role = (this.ddlRoles.Items.Count > 0) ? this.ddlRoles.SelectedValue.ToString() : "";
        DataSet host = (DataSet)Session["PreviewHost"];
        object[] ret = CliUtils.CallFLMethod("Preview", new object[] { id
                , new object[] { (string)this.ViewState["FlowFileName"] + ".xoml", "", activityname, host, role }, new object[] { keys, values }});
        if ((int)ret[0] == 0 && ret[1] != null)
        {
            if (ret[1] is byte[])
            {
                string path = this.Page.MapPath(this.Page.AppRelativeVirtualPath);
                string directory = Path.GetDirectoryName(path);


                path = directory + "\\WorkflowFiles\\PreView\\" + DateTime.Now.ToString("yyMMddHHmmss") + ".jpg";
                directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                System.IO.File.WriteAllBytes(path, (byte[])ret[1]);

                FileInfo file = new FileInfo(path);

                System.Web.HttpResponse Response = this.Page.Response;
                Response.Clear();
                Response.Buffer = false;
                Response.ContentType = "application/x-msdownload";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.Filter.Close();
                Response.WriteFile(file.FullName);
                Response.End();
            }
            else if (ret[1] is DataTable)
            {
                gdvPreview.AutoGenerateColumns = true;
                gdvPreview.DataSource = ret[1] as DataTable;
                gdvPreview.DataBind();
                mvCaption.ActiveIndex = -1;
                MultiView.ActiveViewIndex = 2;
                upload.Visible = false;

            }
        }
    }

    private void GenSource()
    {
        string sql = "";
        if (this.ViewState["ListId"] != null && this.ViewState["ListId"].ToString() != "")
        {
            sql = "SELECT S_STEP_ID,USER_ID,USERNAME,STATUS,UPDATE_DATE,UPDATE_TIME,REMARK FROM SYS_TODOHIS Where (LISTID = '" + this.ViewState["ListId"].ToString() + "') ORDER BY UPDATE_DATE,UPDATE_TIME";
            object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sql });
            if (ret1 != null && (int)ret1[0] == 0)
            {
                this.gdvHis.DataSource = ((DataSet)ret1[1]).Tables[0];
                this.gdvHis.DataBind();
            }
        }
    }

    public string getHtmlText(int index)
    {
        string[] UITexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "SubmitConfirm", "UIText", true).Split(',');
        return UITexts[index];
    }

    public void SetHeaderText()
    {
        string[] UIHisTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "SubmitConfirm", "HisUIText", true).Split(',');
        if (UIHisTexts.Length >= 7)
        {
            this.gdvHis.Columns[0].HeaderText = UIHisTexts[5];
            this.gdvHis.Columns[1].HeaderText = UIHisTexts[0];
            this.gdvHis.Columns[2].HeaderText = UIHisTexts[1];
            this.gdvHis.Columns[3].HeaderText = UIHisTexts[6];
            this.gdvHis.Columns[4].HeaderText = UIHisTexts[4];
            this.gdvHis.Columns[5].HeaderText = UIHisTexts[2];
            this.gdvHis.Columns[6].HeaderText = UIHisTexts[3];
        }
    }

    private string GetMailAddress()
    {
        string url = this.Request.Url.AbsoluteUri;
        string paramters = "?FolderName={0}&FormName={1}&LISTID={2}&FLOWPATH={3}&WHERESTRING={4}&NAVMODE={5}&FLNAVMODE={6}&Users={7}&PLUSAPPROVE={8}&STATUS={9}&SENDTOID={10}&MULTISTEPRETURN={11}";

        //paramters = HttpUtility.UrlEncode(paramters);
        url = url.Substring(0, url.IndexOf("InnerPages/")) + "webClientMainFlow.aspx" + paramters;
        return url;
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
            //HyperLink hlink = new HyperLink();
            //hlink.Text = attach;
            //hlink.Target = "_self";
            //hlink.NavigateUrl = "~/WorkflowFiles/" + attach;
            //cell.Controls.Add(hlink);
            HtmlAnchor a = new HtmlAnchor();
            a.InnerHtml = attach;
            a.Target = "_top";
            a.HRef = "../WorkflowFiles/" + (isFlowFilesBySolutions() ? this.ViewState["VDSNAME"].ToString() + "/" : "") + attach;
            //a.ServerClick += new EventHandler(a_ServerClick);
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
}
