using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Srvtools;

public partial class InnerPages_FlowUrge : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ViewState["LISTID"] = Request.QueryString["LISTID"];
            this.ViewState["FLOWPATH"] = Request.QueryString["FLOWPATH"];
            this.ViewState["PROVIDER"] = Request.QueryString["PROVIDER"];
            this.ViewState["SENDTOID"] = Request.QueryString["SENDTOID"];
            this.ViewState["SENDTOKIND"] = Request.QueryString["SENDTOKIND"];
            this.ViewState["SENDTONAME"] = Request.QueryString["SENDTONAME"];
            this.ViewState["KEYS"] = Request.QueryString["KEYS"];
            //this.ViewState["VALUES"] = Request.QueryString["VALUES"].Replace("$$$", "''");
            this.ViewState["VALUES"] = Request.QueryString["VALUES"].Replace("'", "''");


            this.btnOk.Text = this.getCaption(2);
            this.btnCancel.Text = this.getCaption(3);
            this.btnClose.Text = this.getCaption(4);
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {

        string sendtos = this.ViewState["SENDTOID"].ToString();
        if (this.ViewState["SENDTOKIND"] != null && this.ViewState["SENDTOKIND"].Equals("2"))
        {
            sendtos += ":UserId";
        }

        object[] objParams = CliUtils.CallFLMethod("Notify", new object[] { new Guid(this.ViewState["LISTID"].ToString())
            , new object[] { this.ViewState["FLOWPATH"].ToString(), this.ViewState["FLOWPATH"].ToString(), 0, 0, this.txtRemmark.Text, ""
                , this.ViewState["PROVIDER"].ToString(), 0, sendtos, "" }
            , new object[] { this.ViewState["KEYS"].ToString(), this.ViewState["VALUES"].ToString() } });
        if (Convert.ToInt16(objParams[0]) == 0)
        {
            this.panResult.Visible = true;
            this.btnOk.Enabled = false;
            this.btnCancel.Enabled = false;
            this.result.Text = string.Format(SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLTools", "GloFix", "UrgeFlow", true), this.ViewState["SENDTOID"].ToString(), this.ViewState["SENDTONAME"].ToString());
        }
        else if (Convert.ToInt16(objParams[0]) == 2)
        {
            this.panResult.Visible = true;
            this.btnOk.Enabled = false;
            this.btnCancel.Enabled = false;
            this.result.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLTools", "GloFix", "FailToNotify", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "close_window", "window.close();", true);
    }

    public string getCaption(int index)
    {
        string[] UITexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "SubmitConfirm", "UIText1", true).Split(',');
        return UITexts[index];
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "close_window", "window.close();", true);
    }
}
