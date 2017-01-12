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
using System.Text;

public partial class InnerPages_FlowPause : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ViewState["FLOWFILENAME"] = Request.QueryString["FLOWFILENAME"];
            this.ViewState["PROVIDER"] = Request.QueryString["PROVIDER"];
            this.ViewState["KEYS"] = Request.QueryString["KEYS"];
            this.ViewState["VALUES"] = Request.QueryString["VALUES"].Replace("$$$", "''");

            object[] ret1 = CliUtils.CallMethod("GLModule","ExcuteWorkFlow", new object[]{ "select * from SYS_ORGKIND" });
            if (ret1 != null && (int)ret1[0] == 0)
            {
                this.ddlOrgKind.DataSource = ((DataSet)ret1[1]).Tables[0];
                this.ddlOrgKind.DataValueField = "ORG_KIND";
                this.ddlOrgKind.DataTextField = "KIND_DESC";
                this.ddlOrgKind.DataBind();
            }
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        object[] objParams = CliUtils.CallFLMethod("Pause", new object[] { null, new object[] { this.ViewState["FLOWFILENAME"].ToString() + ".xoml", "", 0, 0, "", "", this.ViewState["PROVIDER"].ToString(), 0, this.ddlOrgKind.SelectedValue, "" }, new object[] { this.ViewState["KEYS"].ToString(), this.ViewState["VALUES"].ToString() } });
        Response.Write("<script language='javascript'>window.close();</script>");
    }
}
