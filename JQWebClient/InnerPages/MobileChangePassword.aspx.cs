using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InnerPages_ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.ClientScript.RegisterStartupScript(typeof(string), ""
            , "$('#userID').val('" + EFClientTools.ClientUtility.ClientInfo.UserID + "');$('#userID').attr('disabled', true);"
            , true);
    }
    public override void ProcessRequest(HttpContext context)
    {
        if (!JqHttpHandler.ProcessRequest(context))
        {
            base.ProcessRequest(context);
        }
    }
}