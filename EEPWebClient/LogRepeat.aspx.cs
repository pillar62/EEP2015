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

public partial class LogRepeat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.labelMessage.Text = string.Format(Session["message"].ToString(), Session["userid"]);
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session.Timeout = 15;
        string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ':' + "1";
        object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
        Response.Redirect("webClientMain.aspx", true);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Timeout = 15;
        CliUtils.fLoginUser = "";
        CliUtils.fLoginPassword = "";
        CliUtils.fLoginDB = "";
        Response.Redirect("InfoLogin.aspx", true);
    }
}
