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
using System.Collections.Specialized;
using System.IO;

public partial class InnerPages_FlowSubmitConfirm : System.Web.UI.Page//, ICallbackEventHandler
{
    public string jsGetAttachMents()
    {
        return HttpUtility.UrlEncode(this.ViewState["ATTACHMENTS"].ToString());
    }

    public string jsGetVDSName()
    {
        return this.ViewState["VDSNAME"].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string[] UITexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "SubmitConfirm", "UIText", true).Split(',');
        if (!IsPostBack)
        {
            // Submit
            this.ViewState["FlowFileName"] = Request.QueryString["FLOWFILENAME"];
            this.ViewState["PagePath"] = Request.QueryString["PAGEPATH"];
            this.ViewState["NavMode"] = Request.QueryString["NAVIGATOR_MODE"];
            this.ViewState["FLNavMode"] = Request.QueryString["FLNAVIGATOR_MODE"];
            this.ViewState["PLUSAPPROVE"] = Request.QueryString["PLUSAPPROVE"];
            this.ViewState["ORGCONTROL"] = Request.QueryString["ORGCONTROL"];
            // Approve, Return
            this.ViewState["ListId"] = Request.QueryString["LISTID"];
            this.ViewState["FlowPath"] = Request.QueryString["FLOWPATH"];
            this.ViewState["IsImportant"] = Request.QueryString["ISIMPORTANT"];
            this.ViewState["IsUrgent"] = Request.QueryString["ISURGENT"];
            this.ViewState["STATUS"] = Request.QueryString["STATUS"];
            this.ViewState["MULTISTEPRETURN"] = Request.QueryString["MULTISTEPRETURN"];
            if (string.IsNullOrEmpty(Request.QueryString["ATTACHMENTS"]))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["LISTID"]))
                {
                    string sqlatt = string.Format("select ATTACHMENTS from SYS_TODOLIST where LISTID = '{0}' AND STATUS NOT IN ('F','A','AA')", this.ViewState["ListId"], this.ViewState["FlowPath"]);//加签的flowpath不正确,考虑去掉flowpath

                    object[] retatt = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sqlatt });
                    if (retatt != null && (int)retatt[0] == 0)
                    {
                        DataTable table = ((DataSet)retatt[1]).Tables[0];
                        if (table.Rows.Count > 0)
                        {
                            this.ViewState["ATTACHMENTS"] = table.Rows[0][0];
                        }
                    }
                }
                else
                {
                    this.ViewState["ATTACHMENTS"] = string.Empty;
                }
            }
            else
            {
                this.ViewState["ATTACHMENTS"] = Request.QueryString["ATTACHMENTS"].Trim();
            }


            this.ViewState["SENDTOID"] = Request.QueryString["SENDTOID"];
            // All
            this.ViewState["OperateType"] = Request.QueryString["OPERATETYPE"];
            this.ViewState["Keys"] = Request.QueryString["KEYS"];
            this.ViewState["Values"] = Request.QueryString["VALUES"].Replace("$$$", "''");
            this.ViewState["Provider"] = Request.QueryString["PROVIDER"];
            this.ViewState["VDSNAME"] = Request.QueryString["VDSNAME"];

            if (!string.IsNullOrEmpty(Request.QueryString["uploadParam1"])) this.ViewState["Important"] = Request.QueryString["uploadParam1"];
            if (!string.IsNullOrEmpty(Request.QueryString["uploadParam2"])) this.ViewState["Urgent"] = Request.QueryString["uploadParam2"];
            if (!string.IsNullOrEmpty(Request.QueryString["uploadParam3"])) this.ViewState["Suggestion"] = Request.QueryString["uploadParam3"];
            if (!string.IsNullOrEmpty(Request.QueryString["uploadParam4"])) this.ViewState["Role"] = Request.QueryString["uploadParam4"];
            if (!string.IsNullOrEmpty(Request.QueryString["uploadParam5"])) this.ViewState["Org"] = Request.QueryString["uploadParam5"];
            if (!string.IsNullOrEmpty(Request.QueryString["uploadParam6"])) this.ViewState["ReturnStep"] = Request.QueryString["uploadParam6"];

            this.GenSource();
            this.SetHeaderText();
            //sql = "SELECT GROUPS.GROUPID, GROUPS.GROUPNAME FROM USERGROUPS LEFT JOIN GROUPS ON USERGROUPS.GROUPID=GROUPS.GROUPID WHERE USERID='" + CliUtils.fLoginUser + "' AND ISROLE='Y'";
            string curTime = FLTools.GloFix.DateTimeString(DateTime.Now);
            string curUser = CliUtils.fLoginUser;
            string flowDesc = "";
            switch (this.ViewState["OperateType"].ToString())
            {
                case "Submit":
                    flowDesc = FLTools.GloFix.GetFlowDesc(this.ViewState["FlowFileName"].ToString() + ".xoml", false);
                    break;
                case "Approve":
                case "Return":
                    flowDesc = FLTools.GloFix.GetFlowDesc(this.ViewState["ListId"].ToString(), true);
                    break;
            }

            this.btnPreview.Enabled = (this.ViewState["OperateType"].ToString() != "Return");

            string sql = "SELECT GROUPID,GROUPNAME FROM GROUPS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + curUser + "')  AND ISROLE='Y' UNION SELECT ROLE_ID AS GROUPID,GROUPS.GROUPNAME  FROM SYS_ROLES_AGENT LEFT JOIN GROUPS ON SYS_ROLES_AGENT.ROLE_ID=GROUPS.GROUPID WHERE (SYS_ROLES_AGENT.FLOW_DESC='*' OR SYS_ROLES_AGENT.FLOW_DESC='" + flowDesc + "') AND AGENT='" + curUser + "' AND START_DATE+START_TIME<='" + curTime + "' AND END_DATE+END_TIME>='" + curTime + "'";
            object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sql });
            if (ret1 != null && (int)ret1[0] == 0)
            {
                this.ddlRoles.DataSource = ((DataSet)ret1[1]).Tables[0];
                this.ddlRoles.DataValueField = "GROUPID";
                this.ddlRoles.DataTextField = "GROUPNAME";
                this.ddlRoles.DataBind();
                if (this.ViewState["SENDTOID"] != null && this.ViewState["SENDTOID"].ToString() != "")
                {
                    this.ddlRoles.SelectedValue = this.ViewState["SENDTOID"].ToString();
                }
            }
            if (CliUtils.fFlowSelectedRole != "")
                this.ddlRoles.SelectedValue = CliUtils.fFlowSelectedRole;
            if (this.ViewState["OperateType"].ToString() == "Submit" && this.ViewState["ORGCONTROL"] != null && this.ViewState["ORGCONTROL"].ToString() == "True")
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
            if (this.ViewState["OperateType"].ToString() == "Return" && this.ViewState["MULTISTEPRETURN"].ToString() == "1" && (this.ViewState["STATUS"] == null || this.ViewState["STATUS"].ToString() != "A" && this.ViewState["STATUS"].ToString() != "AA"))
            {
                this.lblReturnStep.Visible = this.ddlReturnStep.Visible = true;
                object[] objParams = CliUtils.CallFLMethod("GetFLPathList", new object[] { new Guid(this.ViewState["ListId"].ToString()) });
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
                    this.ViewState["MULTISTEPRETURN"] = "0";
                }
            }
            else
                this.lblReturnStep.Visible = this.ddlReturnStep.Visible = false;

            if (this.ViewState["IsImportant"] != null && this.ViewState["IsImportant"].ToString() == "1")
            {
                this.chkImportant.Checked = true;
            }
            if (this.ViewState["IsUrgent"] != null && this.ViewState["IsUrgent"].ToString() == "1")
            {
                this.chkUrgent.Checked = true;
            }

            // back from upload
            if (this.ViewState["Important"] != null)
            {
                if (this.ViewState["Important"].ToString() == "false")
                    this.chkImportant.Checked = false;
                else if (this.ViewState["Important"].ToString() == "true")
                    this.chkImportant.Checked = true;
            }
            if (this.ViewState["Urgent"] != null)
            {
                if (this.ViewState["Urgent"].ToString() == "false")
                    this.chkUrgent.Checked = false;
                else if (this.ViewState["Urgent"].ToString() == "true")
                    this.chkUrgent.Checked = true;
            }

            string remarkField = this.Request.QueryString["RemarkField"];
            if (!string.IsNullOrEmpty(remarkField))
            {
                DataSet host = (DataSet)Session["PreviewHost"];
                if (host != null && host.Tables.Count > 0)
                {
                    DataTable tableHost = host.Tables[0];
                    if (tableHost.Rows.Count > 0)
                    {
                        this.txtSuggest.Text = tableHost.Rows[0][remarkField].ToString();
                    }
                }
            }


            if (this.ViewState["Suggestion"] != null)
                this.txtSuggest.Text = this.ViewState["Suggestion"].ToString();
            if (this.ViewState["Role"] != null)
                this.ddlRoles.SelectedIndex = Convert.ToInt32(this.ViewState["Role"]);
            if (this.ViewState["Org"] != null)
                this.ddlOrg.SelectedIndex = Convert.ToInt32(this.ViewState["Org"]);
            if (this.ViewState["ReturnStep"] != null)
                this.ddlReturnStep.SelectedIndex = Convert.ToInt32(this.ViewState["ReturnStep"]);



        }
        GenDownLoadHref();
        this.chkImportant.Text = UITexts[1];
        this.chkUrgent.Text = UITexts[2];
        this.mvCaption.Captions[0].Caption = UITexts[3];
        this.mvCaption.Captions[1].Caption = UITexts[4];
        this.lblRole.Text = UITexts[5];
        //modified by ccm 2010/12/29
        string operate = this.ViewState["OperateType"].ToString();
      
        if (operate == "Return")
        {
            //this.btnOk.Text = UITexts[15];
            //this.btnOk.Attributes["onclick"] = "return confirm('" + UITexts[15] + "');";
        }
        else
        {
            this.btnOk.Text = UITexts[6];
        }
        //end modify
        this.btnCancel.Text = UITexts[7];
        this.lblOrg.Text = UITexts[9];
        this.lblReturnStep.Text = UITexts[10];
        this.upload.Text = UITexts[12];
        this.btnPreview.Text = UITexts[14];
        //ClientScriptManager csm = Page.ClientScript;
        //string cbReference = csm.GetCallbackEventReference(this, "arg", "receiveServerData", "", true);
        //String callbackScript = "function OkClick(arg) {" + cbReference + "; }";
        //csm.RegisterClientScriptBlock(this.GetType(), "OkClick", callbackScript, true);



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

    void a_ServerClick(object sender, EventArgs e)
    {
        //made link to download...
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
                Response.ContentType = "image/JPEG";
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

    protected void btnOk_Click(object sender, EventArgs e)
    {
        CliUtils.fFlowSelectedRole = this.ddlRoles.SelectedValue;
        string suggest = this.txtSuggest.Text, role = this.ddlRoles.SelectedValue, provider = this.ViewState["Provider"].ToString(), keys = this.ViewState["Keys"].ToString(), values = this.ViewState["Values"].ToString(), operate = this.ViewState["OperateType"].ToString();
        int isImport = Convert.ToInt16(this.chkImportant.Checked), isUrgent = Convert.ToInt16(this.chkUrgent.Checked);
        string[] fLActivities = null;
        if (this.ViewState["FlowPath"] != null)
            fLActivities = this.ViewState["FlowPath"].ToString().Split(';');
        string mailAddress = GetMailAddress();
        this.panResult.Visible = true;
        this.btnOk.Enabled = false;
        this.btnCancel.Enabled = false;
        this.upload.Enabled = false;
        string attachments = this.ViewState["ATTACHMENTS"] == null ? "" : this.ViewState["ATTACHMENTS"].ToString();
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
                if (this.ViewState["ORGCONTROL"] != null && this.ViewState["ORGCONTROL"].ToString() == "True")
                {
                    org = this.ddlOrg.SelectedValue;
                }
                object[] objParams = CliUtils.CallFLMethod("Submit", new object[] { null, new object[] { this.ViewState["FlowFileName"].ToString() + ".xoml", "", isImport, isUrgent, suggest, role, provider, mailAddress, org, attachments }, new object[] { keys, values } });
                if (Convert.ToInt16(objParams[0]) == 0)
                {
                    if (objParams[1].ToString() == "512F4277-0D41-441c-BF16-D96B04580C2E")
                    {
                        this.result.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLNavigator", "HasRejected", true);
                        return;
                    }
                    else if (objParams[1].ToString() == "60585C77-60E1-4e6f-A2E2-3BBBAD6B4C9E")
                    {
                        this.result.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLNavigator", "RunOver", true);
                        if (this.ViewState["PagePath"] != null)
                        {
                            this.ClientScript.RegisterStartupScript(typeof(string), string.Empty, "window.opener.location.reload('" + this.ViewState["PagePath"].ToString() + "?&NAVMODE=0&FLNAVMODE=6');", true);
                        }

                        return;
                    }
                    else
                    {
                        this.send.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLWizard", "Send", true);
                        this.result.Text = FLTools.GloFix.ShowMessage(objParams[1].ToString(), true);
                    }
                    this.ClientScript.RegisterStartupScript(typeof(string), string.Empty, ref_script + "window.opener.location.reload('" + this.ViewState["PagePath"].ToString() +
                        "?FLOWFILENAME=" + HttpUtility.UrlEncode(this.ViewState["FlowFileName"].ToString()) +
                        "&FLNAVMODE=" + this.ViewState["FLNavMode"].ToString() +
                        "&PLUSAPPROVE=" + this.ViewState["PLUSAPPROVE"].ToString() +
                        "&LISTID=" + objParams[2].ToString() +
                        "&FLOWPATH=" + HttpUtility.UrlEncode(objParams[3].ToString()) + ";" + HttpUtility.UrlEncode(objParams[3].ToString()) + userParameter.ToString() +
                        "');", true);
                    btnClose.OnClientClick = "window.close();return false;";
                }
                else
                {
                    if (Convert.ToInt16(objParams[0]) == 2)
                        this.result.Text = objParams[1].ToString();
                    btnClose.OnClientClick = ref_script + "window.close();return false;";
                }
                break;
            case "Approve":
                if (this.ViewState["STATUS"].ToString() == "A" || this.ViewState["STATUS"].ToString() == "AA")
                {
                    objParams = CliUtils.CallFLMethod("PlusReturn", new object[] { new Guid(this.ViewState["ListId"].ToString()), new object[] { fLActivities[0], fLActivities[1], isImport, isUrgent, suggest, role, provider, mailAddress, "", attachments }, new object[] { keys, values } });
                }
                else
                {
                    objParams = CliUtils.CallFLMethod("Approve", new object[] { new Guid(this.ViewState["ListId"].ToString()), new object[] { fLActivities[0], fLActivities[1], isImport, isUrgent, suggest, role, provider, mailAddress, "", attachments }, new object[] { keys, values } });
                }
                if (Convert.ToInt16(objParams[0]) == 0)
                {
                    if (objParams[1].ToString() == "60585C77-60E1-4e6f-A2E2-3BBBAD6B4C9E")
                    {
                        this.result.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLNavigator", "RunOver", true);
                    }
                    else if (objParams[1].ToString() == "512F4277-0D41-441c-BF16-D96B04580C2E")
                    {
                        this.result.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLNavigator", "HasRejected", true);
                    }
                    else
                    {
                        //this.send.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLWizard", "Send");
                        //this.result.Text = FLTools.GloFix.ShowMessage(objParams[1].ToString(), true);
                        if (objParams[1].ToString() == "")
                        {
                            string wait = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLTools", "GloFix", "WaitMessage", true);
                            string sql = "select SENDTO_KIND, SENDTO_ID from SYS_TODOLIST where LISTID='" + this.ViewState["ListId"].ToString() + "' and STATUS <> 'F'";
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
                                this.result.Text = FLTools.GloFix.ShowParallelMessage(sendToIds);
                            }
                        }
                        else
                        {
                            this.send.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLWizard", "Send", true);
                            this.result.Text = FLTools.GloFix.ShowMessage(objParams[1].ToString(), true);
                        }
                    }
                }
                else
                {
                    if (Convert.ToInt16(objParams[0]) == 2)
                    {
                        this.result.Text = objParams[1].ToString();
                        btnClose.OnClientClick = "window.close();return false;";
                        return;
                    }
                 }
                if (this.ViewState["PagePath"] != null)
                {
                    this.ClientScript.RegisterStartupScript(typeof(string), string.Empty, ref_script + "window.opener.location.reload('" + this.ViewState["PagePath"].ToString() + "?&NAVMODE=0&FLNAVMODE=6" + userParameter.ToString() + "');", true);
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(typeof(string), string.Empty, ref_script, true);
                }
                btnClose.OnClientClick = "window.close();return false;";

                break;
            case "Return":
                if (this.ViewState["STATUS"].ToString() == "A" || this.ViewState["STATUS"].ToString() == "AA")
                {
                    objParams = CliUtils.CallFLMethod("PlusReturn2", new object[] { new Guid(this.ViewState["ListId"].ToString()), new object[] { fLActivities[0], fLActivities[1], isImport, isUrgent, suggest, role, provider, mailAddress, "", attachments }, new object[] { keys, values } });
                }
                else if (this.ViewState["MULTISTEPRETURN"].ToString() == "0")
                {
                    objParams = CliUtils.CallFLMethod("Return", new object[] { new Guid(this.ViewState["ListId"].ToString()), new object[] { fLActivities[0], fLActivities[1], isImport, isUrgent, suggest, role, provider, mailAddress, "", attachments }, new object[] { keys, values } });
                }
                else if (this.ViewState["MULTISTEPRETURN"].ToString() == "1")
                {
                    if (this.ddlReturnStep.SelectedIndex == 0)
                    {
                        objParams = CliUtils.CallFLMethod("Return", new object[] { new Guid(this.ViewState["ListId"].ToString()), new object[] { fLActivities[0], fLActivities[1], isImport, isUrgent, suggest, role, provider, 0, "", attachments }, new object[] { keys, values } });
                    }
                    else
                    {
                        string retToStep = this.ddlReturnStep.Text;
                        objParams = CliUtils.CallFLMethod("Return2", new object[] { new Guid(this.ViewState["ListId"].ToString()), new object[] { retToStep, fLActivities[1], isImport, isUrgent, suggest, role, provider, 0, "", attachments }, new object[] { keys, values } });
                    }
                }
                else return;

                if (Convert.ToInt16(objParams[0]) == 0)
                {
                    if (objParams[1].ToString() == "B4DAF3A4-AAE8-4b51-A391-B52E46305E9F")
                    {
                        this.result.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLTools", "FLWebNavigator", "ReturnToEnd", true);
                    }
                    else
                    {
                        if (objParams[1].ToString().StartsWith("C912D847-1825-458a-8CB5-E680FACA42AF"))
                        {
                            this.result.Text = FLTools.GloFix.ShowMessage3(objParams[1].ToString().Split(':')[1], true);
                        }
                        else if (objParams[1].ToString() == "")
                        {
                            string wait = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLTools", "GloFix", "WaitMessage", true);
                            string sql = "select SENDTO_KIND, SENDTO_ID from SYS_TODOLIST where LISTID='" + this.ViewState["ListId"].ToString() + "' and STATUS <> 'F'";
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
                                this.result.Text = FLTools.GloFix.ShowParallelMessage(sendToIds);
                            }
                        }
                        else
                        {
                            this.send.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLWizard", "Send", true);
                            this.result.Text = FLTools.GloFix.ShowMessage(objParams[1].ToString(), true);
                        }
                    }
                }
                else
                {
                    if (Convert.ToInt16(objParams[0]) == 2)
                        this.result.Text = objParams[1].ToString();
                }
                if (this.ViewState["PagePath"] != null)
                {
                    this.ClientScript.RegisterStartupScript(typeof(string), string.Empty, ref_script + "window.opener.location.reload('" + this.ViewState["PagePath"].ToString() + "?&NAVMODE=0&FLNAVMODE=6" + userParameter.ToString() + "');", true);
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(typeof(string), string.Empty, ref_script, true);
                }
                btnClose.OnClientClick = "window.close();return false;";
                break;
        }
    }

    private string GetMailAddress()
    {
        string url = this.Request.Url.AbsoluteUri;
        string paramters = "?FolderName={0}&FormName={1}&LISTID={2}&FLOWPATH={3}&WHERESTRING={4}&NAVMODE={5}&FLNAVMODE={6}&Users={7}&PLUSAPPROVE={8}&STATUS={9}&SENDTOID={10}&MULTISTEPRETURN={11}&ATTACHMENTS={12}";

        //paramters = HttpUtility.UrlEncode(paramters);
        url = url.Substring(0, url.IndexOf("InnerPages/")) + "webClientMainFlow.aspx" + paramters;
        return url;
    }

    public string ShowMessage(string sendToIds)
    {
        if (sendToIds == "")
            return "wait for others to check!";
        string[] toIds = sendToIds.Split(';');
        List<string> sendToUsers = new List<string>();
        List<string> sendToRoles = new List<string>();
        List<string> sendToManager = new List<string>();
        for (int i = 0; i < toIds.Length; i++)
        {
            string[] parts = toIds[i].Split('|');
            sendToManager.Add(parts[0]);
            if (parts[1].IndexOf(':') != -1)
            {
                sendToUsers.Add(parts[1].Substring(0, parts[1].IndexOf(':')));
            }
            else
            {
                sendToRoles.Add(parts[1]);
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
                object[] ret2 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { "cmdDDUse", "select USERID, USERNAME from USERS where USERID in (select USERID from USERGROUPS where GROUPID='" + row["GROUPID"].ToString() + "')" });
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
        string allManager = "";
        foreach (string manager in sendToManager)
        {
            allManager += manager + " ";
        }
        if (allManager != "")
            allManager = allManager.Substring(0, allManager.LastIndexOf(' '));

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
        string m = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLWizard", "SendToManager", true);
        message += string.Format(m, allManager, allRoles, allUsers);

        return message;
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
    protected void mvCaption_TabChanging(object sender, TabChangingEventArgs e)
    {
        this.upload.Visible = (e.NewActiveIndex == 0);
    }
}
