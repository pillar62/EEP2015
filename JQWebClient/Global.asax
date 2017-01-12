<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        
        
        string virPath = Request.ApplicationPath;
        if (Server.GetLastError() != null)
        {
            //if (Server.GetLastError().Message == "Timeout, relogon please")
            //{
            //    Server.ClearError();
            //    Response.Redirect(virPath + "/LogOn.aspx");
            //}
            //else if (Request.CurrentExecutionFilePathExtension == ".ashx")
            //{
            //    return;//不处理ashx抛出的错误
            //}
            //else
            //{
                Exception error = Server.GetLastError().InnerException;
                if (error == null)
                    return;
                if (error.Message == "Timeout, relogon please")
                {
                    Server.ClearError();
                    Server.Transfer(virPath + "/TimeOut.aspx?p=true");
                }
                else
                {
                    System.Web.HttpContext.Current.Session["LastError"] = error;
                    Server.Transfer(virPath + "/Error.aspx");
                }
            //}
        }
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
