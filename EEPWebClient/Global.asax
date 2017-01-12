<%@ Application Language="C#"%>

<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        
    }
    
    void Application_End(object sender, EventArgs e)
    {
        
    }

    void Application_Error(object sender, EventArgs e)
    {
        Exception error = null;
        if (Server.GetLastError() != null)
        {
            error = Server.GetLastError().InnerException;
            if (error == null)
                return;
        }

        string virPath = Request.ApplicationPath;
        if (virPath != null && virPath.Length != 0)
        {
            if (error is Srvtools.PageAuthorityException)
            {
                Server.Transfer(virPath + "/AccessDenied.aspx");
            }
            else if (error.Message == "75FF57F7-7AC0-43c8-9454-C92B4A2723BB")
            {
                Server.ClearError();
                if (!string.IsNullOrEmpty(Request.RawUrl) && Request.RawUrl.Contains("FLOWPATH"))
                {
                    StringBuilder url = new StringBuilder(Request.AppRelativeCurrentExecutionFilePath);
                    string queryString = Request.QueryString.ToString();
                    if (!string.IsNullOrEmpty(queryString))
                    {
                        url.Append("?");
                        url.Append(queryString.Replace("&", "^"));
                    }

                    Response.Redirect(virPath + "/InfoLogin.aspx?Redirect=" + url, true);
                }
                else
                {
                    Server.Transfer(virPath + "/Timeout.aspx");
                }
            }
            //else if (error.Message == "6A8F8FE2-60B3-4cc3-8C43-FC9FDA58360E")
            //{
            //    Server.Transfer(virPath + "/Timeout.aspx?IsFlow=1");
            //}
            else
            {
                Server.Transfer(virPath + "/Error.aspx");
            }
        }
        else
        {
            if (error.Message == "75FF57F7-7AC0-43c8-9454-C92B4A2723BB")
            {
                Server.Transfer(virPath + "/Timeout.aspx?IsFlow=0");
            }
            else if (error.Message == "6A8F8FE2-60B3-4cc3-8C43-FC9FDA58360E")
            {
                Server.Transfer(virPath + "/Timeout.aspx?IsFlow=1");
            }
            else
            {
                Server.Transfer("../Error.aspx");
            }
        }
    }

    void Session_Start(object sender, EventArgs e)
    {

    }
        
    void Session_End(object sender, EventArgs e) 
    {
        if (Session != null && Session["fLoginUser"] != null)
        {
            object user = Session["fLoginUser"];
            object lang = Session["fClientLang"];
            object db = Session["fLoginDB"];
            object site = Session["fSiteCode"];
            object compName = Session["fComputerName"];
            object compIp = Session["fComputerIp"];
            object currPrj = Session["fCurrentProject"];
            
            object cliInfo = (object)(new object[] { lang,user, db, site, compName, compIp, currPrj });
            Srvtools.CliUtils.WebLogOut("GLModule", "LogOut", new object[] { (object)(Session["fLoginUser"]) }, new object[] { cliInfo });
        }
    }






















    
    
    
    
    
    
    
    
    
</script>
