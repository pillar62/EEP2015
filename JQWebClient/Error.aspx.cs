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

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Exception error = Session["LastError"] as Exception;
            if (error == null)
                return;

            string msg = error.Message;   //.Replace("\n", "\r\r");
            string source = error.Source;      // Replace("\r\n", "\r\r");
            string stackTrace = error.StackTrace;         // Replace("\r\n", "\r\r");
            if (error.InnerException != null)
            {
                TextBoxServerStack.Text = error.InnerException.StackTrace;
            }

            txtMsg.Text = msg;
            txtStackTrace.Text = stackTrace;

            Server.ClearError();
        }
    }

    protected void btnFeedback_Click(object sender, EventArgs e)
    {
        //if (txtDescription.Text == null || txtDescription.Text.Length == 0)
        //    return;

        //if (txtMsg.Text == null || txtMsg.Text.Length == 0)
        //    return;

        //ClientScriptManager manager = Page.ClientScript;
        //try
        //{
        //    Srvtools.CliUtils.FeedbackBug(txtMsg.Text, txtStackTrace.Text, txtDescription.Text, new byte[] { },true);
        //    String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WebSysMsg", "msg_FeedbackBugSucceeded", true);
        //    manager.RegisterStartupScript(this.GetType(), "form1", "<script lanuage='javascript'>alert('" +  message + "')</script>");
        //}
        //catch
        //{
        //    String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WebSysMsg", "msg_FeedbackBugUnSucceeded", true);
        //    manager.RegisterStartupScript(this.GetType(), "form1", "<script lanuage='javascript'>alert('" + message + "')</script>");
        //}
    }
    protected void ButtonServerInfo_Click(object sender, EventArgs e)
    {
        if (string.Compare(ButtonServerInfo.Text, "server stack", true) == 0)
        {
            this.txtStackTrace.Visible = false;
            this.TextBoxServerStack.Visible = true;
            this.ButtonServerInfo.Text = "Client Stack";
        }
        else
        {
            this.txtStackTrace.Visible = true;
            this.TextBoxServerStack.Visible = false;
            this.ButtonServerInfo.Text = "Server Stack";
        }
    }
}
