using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var maxUser = 0;
        var locale = Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : "en-us";
        EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Request.PhysicalApplicationPath, locale);
        string content = provider["EEPWebNetClient/WinSysMsg/txt_Login"];
        if (!string.IsNullOrEmpty(content))
        {
            var tooltiplist = content.Split(';');
            UserIDLabel.Text = tooltiplist[0];
           
        }
        content = provider["JQWebClient/logon"];
        if (!string.IsNullOrEmpty(content))
        {
            var tooltiplist = content.Split(',');
            BackLabel.Text = tooltiplist[6];
            OKLabel.Text = this.Request.QueryString["p"] == "register" ? tooltiplist[7] : tooltiplist[8];
        }
    }

    public override void ProcessRequest(HttpContext context)
    {
        
        base.ProcessRequest(context);
    }
}