using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EFClientTools.EFServerReference;
using System.Configuration;

public partial class SingleSignOn1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strFullUrl = Request.Url.ToString();                                 
        string url = strFullUrl.Substring(strFullUrl.IndexOf("RedirectUrl") + 12);
        var publicKey = this.Request.QueryString["PublicKey"];

        if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(publicKey))
        {
            try
            {
                var clientInfo = new ClientInfo() { SecurityKey = publicKey, UseDataSet = true };
                EFServiceClient client = EFClientTools.ClientUtility.Client;
                clientInfo = client.LogOn(clientInfo);
                var locale = Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : "en-us";
                clientInfo.Locale = locale;
                clientInfo.LogonResult = LogonResult.Logoned;
                Session["ClientInfo"] = clientInfo;
                if (url.TrimStart().StartsWith("http"))
                {
                }
                else
                {
                    Response.Redirect(url);
                }
            }
            catch(Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        else
        {
            Response.Write("PublishKey is null or url is null.");
        }
    }
}