using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class jqOPOR11 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string lang = EFClientTools.ClientUtility.ClientInfo.UserPara1;
        switch (lang)
        {
            case "zh-TW": JQMultiLanguage1.GroupIndex = JQClientTools.LanguageGroups.ChineseTra; break;
            case "zh-CN": JQMultiLanguage1.GroupIndex = JQClientTools.LanguageGroups.ChineseSim; break;
            case "en-us": JQMultiLanguage1.GroupIndex = JQClientTools.LanguageGroups.English; break;
        }
        JQMultiLanguage1.SetLanguage(false);
        lbMultiLanguage.Text = lang;
    }

    public override void ProcessRequest(HttpContext context)
    {
        if (!JqHttpHandler.ProcessRequest(context))
        {
            base.ProcessRequest(context);
        }
    }
}
