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
using Srvtools;

public partial class InnerPages_UserInFlow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GenSource();
        GenColumnCaption();
    }

    private void GenColumnCaption()
    {
        string[] UIHisTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "UserInFlow", "ByUser", true).Split(',');
        if (UIHisTexts.Length >= 4)
        {
            this.gdvByUser.HeaderRow.Cells[0].Text = UIHisTexts[0];
            this.gdvByUser.HeaderRow.Cells[1].Text = UIHisTexts[1];
            this.gdvByUser.HeaderRow.Cells[2].Text = UIHisTexts[2];
            this.gdvByUser.HeaderRow.Cells[3].Text = UIHisTexts[3];
        }

        UIHisTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "UserInFlow", "ByRole", true).Split(',');
        if (UIHisTexts.Length >= 4)
        {
            this.gdvByRole.HeaderRow.Cells[0].Text = UIHisTexts[0];
            this.gdvByRole.HeaderRow.Cells[1].Text = UIHisTexts[1];
            this.gdvByRole.HeaderRow.Cells[2].Text = UIHisTexts[2];
            this.gdvByRole.HeaderRow.Cells[3].Text = UIHisTexts[3];
        }

        this.CaptionUserId.Text = getHtmlText(0);
        this.CaptionUserName.Text = getHtmlText(1);
    }

    private void GenSource()
    {
        object[] ret = CliUtils.CallFLMethod("GetUserActivities", null);
        DataTable userTable = ret[1] as DataTable;
        DataTable roleTable = ret[2] as DataTable;
        if (ret != null && (int)ret[0] == 0)
        {
            this.gdvByUser.DataSource = (ret[1] as DataTable).DefaultView;
            this.gdvByUser.DataBind();
            
            this.gdvByRole.DataSource = (ret[2] as DataTable).DefaultView;
            this.gdvByRole.DataBind();                
        }
    }

    public string getHtmlText(int index)
    {
        string[] UITexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "UserInFlow", "UIText", true).Split(';');
        return UITexts[index];
    }
}
