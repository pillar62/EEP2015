using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Template_JQuerySingle1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Template_JQuerySingle1));
    }

    public override void ProcessRequest(HttpContext context)
    {
        if (!JqHttpHandler.ProcessRequest(context))
        {
            base.ProcessRequest(context);
        }
    }
}