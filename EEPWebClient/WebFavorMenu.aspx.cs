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

public partial class WebFavorMenu : System.Web.UI.Page
{
    static ArrayList menuIDList = new ArrayList();
    static ArrayList captionList = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            int maxLength = 0;
            this.ddlGroup.Items.Add("");

            DataSet dsFavorMenus = new DataSet();
            object[] strParam = new object[2];
            strParam[0] = CliUtils.fCurrentProject;
            strParam[1] = "W";
            object[] favorMenus = CliUtils.CallMethod("GLModule", "FetchFavorMenus", strParam);
            if (favorMenus != null && Convert.ToInt16(favorMenus[0]) == 0)
            {
                dsFavorMenus = favorMenus[1] as DataSet;
            }

            DataSet dsMenus = new DataSet();
            object[] menus = CliUtils.CallMethod("GLModule", "FetchMenus", strParam);
            if (menus != null && Convert.ToInt16(menus[0]) == 0)
            {
                dsMenus = menus[1] as DataSet;
            }
            ArrayList menuIDs = new ArrayList();
            for (int i = 0; i < dsMenus.Tables[0].Rows.Count; i++)
            {
                menuIDs.Add(dsMenus.Tables[0].Rows[i]["MENUID"].ToString());
            }

            if (dsFavorMenus.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsFavorMenus.Tables[0].Rows.Count; i++)
                {
                    if (!CheckSecurity(dsFavorMenus.Tables[0].Rows[i], dsMenus, menuIDs)) continue;

                    this.lbFavor.Items.Add(dsFavorMenus.Tables[0].Rows[i]["CAPTION"].ToString());
                    maxLength = getMaxLength(maxLength, dsFavorMenus.Tables[0].Rows[i]["CAPTION"].ToString().Length);
                }
            }

            if (dsMenus.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsMenus.Tables[0].Rows.Count; i++)
                {
                    if ((dsMenus.Tables[0].Rows[i]["PARENT"] == null || dsMenus.Tables[0].Rows[i]["PARENT"].ToString() == "")
                        && (dsMenus.Tables[0].Rows[i]["PACKAGE"] == null || dsMenus.Tables[0].Rows[i]["PACKAGE"].ToString() == ""))
                    {
                        this.ddlGroup.Items.Add(dsMenus.Tables[0].Rows[i]["CAPTION"].ToString());
                        continue;
                    }
                    if (!CheckSecurity(dsMenus.Tables[0].Rows[i], dsMenus, menuIDs)) continue;
                    
                    this.lbAll.Items.Add(dsMenus.Tables[0].Rows[i]["CAPTION"].ToString());
                    menuIDList.Add(dsMenus.Tables[0].Rows[i]["MENUID"].ToString());
                    captionList.Add(dsMenus.Tables[0].Rows[i]["CAPTION"].ToString());
                    maxLength = getMaxLength(maxLength, dsMenus.Tables[0].Rows[i]["CAPTION"].ToString().Length);
                }
            }

            int count = dsMenus.Tables[0].Rows.Count;
            strParam[1] = "C";
            dsMenus = new DataSet();
            menus = CliUtils.CallMethod("GLModule", "FetchMenus", strParam);
            if (menus != null && Convert.ToInt16(menus[0]) == 0)
            {
                dsMenus = menus[1] as DataSet;
            }
            if (dsMenus.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsMenus.Tables[0].Rows.Count; i++)
                {
                    if (dsMenus.Tables[0].Rows[i]["PARENT"] == null || dsMenus.Tables[0].Rows[i]["MENUID"].ToString() == "") continue;
                    if (!CheckSecurity(dsMenus.Tables[0].Rows[i], dsMenus, menuIDs)) continue;
                    
                    this.lbAll.Items.Add(dsMenus.Tables[0].Rows[i]["CAPTION"].ToString());
                    menuIDList.Add(dsMenus.Tables[0].Rows[i]["MENUID"].ToString());
                    captionList.Add(dsMenus.Tables[0].Rows[i]["CAPTION"].ToString());
                    maxLength = getMaxLength(maxLength, dsMenus.Tables[0].Rows[i]["CAPTION"].ToString().Length);
                }
            }
            for (int i = 0; i < this.lbFavor.Items.Count; i++)
            {
                this.lbAll.Items.Remove(this.lbFavor.Items[i]);
            }

            this.btnCancel.Attributes.Add("onclick", "Close()");
            this.lbAll.Height = dsFavorMenus.Tables[0].Rows.Count + (count + dsMenus.Tables[0].Rows.Count + 5) * 20;
            if (maxLength * 8 < 150)
                this.lbAll.Width = 150;
            else
                this.lbAll.Width = maxLength * 8;
            this.lbFavor.Height = dsFavorMenus.Tables[0].Rows.Count + (count + dsMenus.Tables[0].Rows.Count + 5) * 20 - 24;
            if (maxLength * 8 < 150)
                this.lbFavor.Width = 150;
            else
                this.lbFavor.Width = maxLength * 8;

            ddlGroup_SelectedIndexChanged(sender, e);
        }
    }

    private bool CheckSecurity(DataRow dr, DataSet menu, ArrayList menuIDs)
    {
        if (dr["PARENT"] == null || dr["PARENT"].ToString() == String.Empty)
        {
            return true;
        }
        else
        {
            if (!menuIDs.Contains(dr["PARENT"].ToString()))
            {
                return false;
            }
            else
            {
                for (int i = 0; i < menu.Tables[0].Rows.Count; i++)
                {
                    if (menu.Tables[0].Rows[i]["MENUID"].ToString() == dr["PARENT"].ToString())
                        return CheckSecurity(menu.Tables[0].Rows[i], menu, menuIDs);
                }
            }
        }
        return true;
    }

    private int getMaxLength(int oldMax, int length)
    {
        if (oldMax > length)
            return oldMax;
        else
            return length;
    }

    protected void btnAddAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < this.lbAll.Items.Count; i++)
            this.lbFavor.Items.Add(this.lbAll.Items[i].ToString());
        this.lbAll.Items.Clear();

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (this.lbAll.SelectedItem != null)
        {
            this.lbFavor.Items.Add(this.lbAll.SelectedItem.Text);
            this.lbAll.Items.Remove(this.lbAll.SelectedItem.Text);
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        if (this.lbFavor.SelectedItem != null)
        {
            this.lbAll.Items.Add(this.lbFavor.SelectedItem.Text);
            this.lbFavor.Items.Remove(this.lbFavor.SelectedItem.Text);
        }
    }

    protected void btnRemoveAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < this.lbFavor.Items.Count; i++)
            this.lbAll.Items.Add(this.lbFavor.Items[i].ToString());
        this.lbFavor.Items.Clear();
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        ArrayList menuID = new ArrayList();
        ArrayList caption = new ArrayList();
        for (int i = 0; i < this.lbFavor.Items.Count; i++)
        {
            caption.Add(this.lbFavor.Items[i].ToString());
            menuID.Add(getMenuID(this.lbFavor.Items[i].ToString()));
        }
        object[] param = new object[5];
        param[0] = CliUtils.fLoginUser;
        param[1] = CliUtils.fCurrentProject;
        param[2] = menuID;
        param[3] = caption;
        param[4] = ddlGroup.Text;

        object[] myRet = CliUtils.CallMethod("GLModule", "GetFavorMenuID", param);
        this.Page.Response.Write("<script>opener.location.reload();window.close();</script>");
    }

    private String getMenuID(String caption)
    {
        for (int i = 0; i < menuIDList.Count; i++)
        {
            if (captionList[i].ToString() == caption)
                return menuIDList[i].ToString();
        }
        return "";
    }
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.lbFavor.Items.Clear();

        DataSet dsFavorMenus = new DataSet();
        object[] strParam = new object[2];
        strParam[0] = CliUtils.fCurrentProject;
        strParam[1] = "W";
        object[] favorMenus = CliUtils.CallMethod("GLModule", "FetchFavorMenus", strParam);
        if (favorMenus != null && Convert.ToInt16(favorMenus[0]) == 0)
        {
            dsFavorMenus = favorMenus[1] as DataSet;
        }

        DataSet dsMenus = new DataSet();
        object[] menus = CliUtils.CallMethod("GLModule", "FetchMenus", strParam);
        if (menus != null && Convert.ToInt16(menus[0]) == 0)
        {
            dsMenus = menus[1] as DataSet;
        }
        ArrayList menuIDs = new ArrayList();
        for (int i = 0; i < dsMenus.Tables[0].Rows.Count; i++)
        {
            menuIDs.Add(dsMenus.Tables[0].Rows[i]["MENUID"].ToString());
        }

        if (dsFavorMenus.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsFavorMenus.Tables[0].Rows.Count; i++)
            {
                if (dsFavorMenus.Tables[0].Rows[i]["GROUPNAME"].ToString() == ddlGroup.Text)
                {
                    if (!CheckSecurity(dsFavorMenus.Tables[0].Rows[i], dsMenus, menuIDs)) continue;
                    
                    this.lbFavor.Items.Add(dsFavorMenus.Tables[0].Rows[i]["CAPTION"].ToString());
                }
            }
        }
    }
}