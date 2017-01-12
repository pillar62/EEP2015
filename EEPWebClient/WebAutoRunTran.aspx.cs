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
using System.IO;
using System.Text;

public partial class WebAutoRunTran : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (Convert.ToInt32(this.Page.Request.QueryString["Times"].ToString()) > 1000)
            this.Session.Timeout = 1000;
        else
            this.Session.Timeout += Convert.ToInt32(this.Page.Request.QueryString["times"].ToString()) * 10;

        string[] user = this.Page.Request.QueryString["usermessage"].Split(';');
        CliUtils.fLoginUser = user[0];
        CliUtils.fLoginPassword = user[1];
        CliUtils.fLoginDB = user[2];
        CliUtils.fCurrentProject = user[3];
        CliUtils.fClientSystem = "Web";
        CliUtils.fComputerIp = Request.UserHostAddress;
        CliUtils.fComputerName = Request.UserHostName;

        Session.Timeout = Convert.ToInt32(this.Page.Request.QueryString["times"].ToString());
        Session.Add("active", "true");
        Session.Add("packagename", this.Page.Request.QueryString["packagename"]);
        Session.Add("times", this.Page.Request.QueryString["times"]);
        Session.Add("Interval", this.Page.Request.QueryString["Interval"]);
        Session.Add("Log", this.Page.Request.QueryString["Log"]);
        Session.Add("userid", user[0]);

        Response.Redirect(this.Page.Request.QueryString["packagename"], true);
        Session.Abandon();

        //Process.Start("IExplore.exe", this.Page.Request.QueryString["Path"] + "\\" + this.Page.Request.QueryString["Package"] + "?active=true&userid=" + user[0] + "&packagename=" + PackageName + "." + FormName
        //                                    + "&times=" + Times + "&Interval=" + Interval + "&Log=" + this.Page.Request.QueryString["Log"]);
    }
}
