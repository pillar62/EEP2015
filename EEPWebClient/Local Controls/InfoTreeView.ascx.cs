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

public partial class InfoTreeView : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public DropDownList InnerDropDownList
    {
        get
        {
            return this.ddlSolution;
        }
    }

    public TreeView InnerTreeView
    {
        get
        {
            return this.tView;
        }
    }
}
