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

public partial class WebFrame : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private string url = "";
    public string Url
    {
        set { url = value; }
        get { return url; }
    }

    protected override void Render(HtmlTextWriter output)
    {
        output.Write("<iframe id='WebFrame' name='content' frameborder=1 width='99%' height='520' src=" + url + "></iframe>");
    }
}
