using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Drawing;
using System.ComponentModel;

namespace Srvtools
{
    [ToolboxBitmap(typeof(WebExpressionControl), "Resources.WebExpressionControl.ico")]
    [ToolboxData("<{0}:WebExpressionControl runat=server></{0}:WebExpressionControl>")]
    public class WebExpressionControl : Label
    {
        public WebExpressionControl()
        { }

        [Category("Infolight"),
         Description("Specifies the expression of data")]
        public string Expression
        {
            get
            {
                object obj = this.ViewState["Expression"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["Expression"] = value;
            }
        }
    }
}
