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

public partial class WebClientMain2 : System.Web.UI.MasterPage
{
    private ArrayList MenuIDList = new ArrayList();
    private ArrayList CaptionList = new ArrayList();
    private ArrayList ParentList = new ArrayList();
    private ArrayList ImageList = new ArrayList();
    private ArrayList PackageList = new ArrayList();
    private ArrayList FormList = new ArrayList();
    private ArrayList ItemParam = new ArrayList();
    private DataSet menuDataSet = new DataSet();
    private InfoDataSet dsSol = new InfoDataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession)
        {
            Response.Redirect("InfoLogin.aspx?IsMasterPage=true", true);
        }
        if (!IsPostBack)
        {
            DoLoad();
            ItemToGet(this.ddlSolution.SelectedValue.ToString());
            this.ViewState.Add("menuDs", menuDataSet);
        }
    }

    public void DoLoad()
    {
        DataSet dsSolution = new DataSet();
        object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolution", null);
        if ((null != myRet1) && (0 == (int)myRet1[0]))
            dsSolution = ((DataSet)myRet1[1]);
        this.ddlSolution.DataSource = dsSolution.Tables[0];
        this.ddlSolution.DataTextField = "itemname";
        this.ddlSolution.DataValueField = "itemtype";
        this.ddlSolution.DataBind();
        int i = dsSolution.Tables[0].Rows.Count;
        for (int j = 0; j < i; j++)
        {
            if (string.Compare(dsSolution.Tables[0].Rows[j][0].ToString(), Session["fCurrentProject"].ToString(), true) == 0)//IgnoreCase
            {
                this.ddlSolution.SelectedValue = dsSolution.Tables[0].Rows[j][0].ToString();
                break;
            }
        }
    }

    private void ItemToGet(string selValue)
    {
        object[] LoginUser = new object[1];
        LoginUser[0] = CliUtils.fLoginUser;
        object[] strParam = new object[2];
        strParam[0] = selValue;
        strParam[1] = "W";
        object[] myRet = CliUtils.CallMethod("GLModule", "FetchMenus", strParam);
        if ((null != myRet) && (0 == (int)myRet[0]))
        {
            menuDataSet = (DataSet)(myRet[1]);
        }
        MenuIDList.Clear();
        CaptionList.Clear();
        ParentList.Clear();
        int menuCount = menuDataSet.Tables[0].Rows.Count;
        string strCaption = SetMenuLanguage();
        for (int i = 0; i < menuCount; i++)
        {
            MenuIDList.Add(menuDataSet.Tables[0].Rows[i]["menuid"].ToString());
            if (menuDataSet.Tables[0].Rows[i][strCaption].ToString() != "")
            {
                CaptionList.Add(menuDataSet.Tables[0].Rows[i][strCaption].ToString());
            }
            else
            {
                CaptionList.Add(menuDataSet.Tables[0].Rows[i]["caption"].ToString());
            }
            ParentList.Add(menuDataSet.Tables[0].Rows[i]["parent"].ToString());
            ImageList.Add(menuDataSet.Tables[0].Rows[i]["imageurl"]);
            PackageList.Add(menuDataSet.Tables[0].Rows[i]["package"].ToString());
            FormList.Add(menuDataSet.Tables[0].Rows[i]["form"].ToString());
            ItemParam.Add(menuDataSet.Tables[0].Rows[i]["itemparam"].ToString());
        }

        initializeMenu(MenuIDList, CaptionList, ParentList, ImageList, PackageList, FormList);
    }

    private void initializeMenu(ArrayList menuIDList, ArrayList menuCaptionList, ArrayList menuParentIDList, ArrayList imageList
                                , ArrayList packageList, ArrayList formList)
    {
        string defaultDir = "~/Image/MenuTree/";
        ArrayList ListMainID = new ArrayList();
        ArrayList ListMainCaption = new ArrayList();
        ArrayList ListMainForm = new ArrayList();
        ArrayList ListMainImage = new ArrayList();
        ArrayList ListMainPackage = new ArrayList();

        ArrayList ListChildrenID = new ArrayList();
        ArrayList ListOwnerParentID = new ArrayList();
        ArrayList ListChildrenCaption = new ArrayList();
        ArrayList ListChildrenImage = new ArrayList();
        ArrayList ListChildrenPackage = new ArrayList();
        ArrayList ListChildrenForm = new ArrayList();
        int MenuCount = menuIDList.Count;
        for (int ix = 0; ix < MenuCount; ix++)
        {
            if (menuParentIDList[ix].ToString() == string.Empty)
            {
                ListMainID.Add(menuIDList[ix].ToString());
                ListMainCaption.Add(menuCaptionList[ix].ToString());
                ListMainImage.Add(imageList[ix].ToString());
                ListMainPackage.Add(packageList[ix].ToString());
                ListMainForm.Add(formList[ix].ToString());
            }
            else
            {
                ListChildrenID.Add(menuIDList[ix].ToString());
                ListOwnerParentID.Add(menuParentIDList[ix].ToString());
                ListChildrenCaption.Add(menuCaptionList[ix].ToString());
                ListChildrenImage.Add(imageList[ix].ToString());
                ListChildrenPackage.Add(packageList[ix].ToString());
                ListChildrenForm.Add(formList[ix].ToString());
            }
        }

        int i = ListMainID.Count;
        MenuItem[] nodeMain = new MenuItem[i];
        for (int j = 0; j < i; j++)
        {
            nodeMain[j] = new MenuItem();
            menu1.Items.Add(nodeMain[j]);
            nodeMain[j].Value = ListMainID[j].ToString();
            nodeMain[j].Text = ListMainCaption[j].ToString();
            if (ListMainImage[j] != null && ListMainImage[j].ToString() != "")
                nodeMain[j].ImageUrl = defaultDir + ListMainImage[j].ToString();
            else
                nodeMain[j].ImageUrl = "~/Image/main/b4.gif";
            nodeMain[j].Selectable = false;
        }
        int p = ListChildrenID.Count;
        MenuItem[] nodeChildren = new MenuItem[p];
        for (int q = 0; q < p; q++)
        {
            nodeChildren[q] = new MenuItem();
            nodeChildren[q].Value = ListChildrenID[q].ToString();
            nodeChildren[q].Text = ListChildrenCaption[q].ToString();
            if (ListChildrenImage[q] != null && ListChildrenImage[q].ToString() != "")
                nodeChildren[q].ImageUrl = defaultDir + ListChildrenImage[q].ToString();
            else
                nodeChildren[q].ImageUrl = "~/Image/main/b6.gif";
            if (ListChildrenPackage[q] != null && ListChildrenPackage[q].ToString() != "" &&
                ListChildrenForm[q] != null && ListChildrenForm[q].ToString() != "")
            {
                nodeChildren[q].NavigateUrl = "~\\" + ListChildrenPackage[q].ToString() + "\\" + ListChildrenForm[q].ToString() + ".aspx";
            }
            else
            {
                nodeChildren[q].NavigateUrl = "~\\DefaultPage2.aspx";
            }
        }

        for (int q = 0; q < p; q++)
        { 
            for(int x = 0; x < ListMainID.Count; x++)
            {
                if (ListOwnerParentID[q].ToString() == ListMainID[x].ToString())
                {
                    nodeMain[x].ChildItems.Add(nodeChildren[q]);
                }
            }
            for (int s = 0; s < p; s++)
            {
                if (ListOwnerParentID[q].ToString() == ListChildrenID[s].ToString())
                {
                    nodeChildren[s].ChildItems.Add(nodeChildren[q]);
                    nodeChildren[s].Selectable = false;
                }
            }
        }

        //for (int q = p - 1; q >= 0; q--)
        //{
        //    if (ListMainID.Contains(ListOwnerParentID[q]))
        //    {
        //        int x = ListMainID.IndexOf(ListOwnerParentID[q]);
        //        MenuItem nodeChild = new MenuItem();
        //        nodeChild.Value = ListChildrenID[q].ToString();
        //        nodeChild.Text = ListChildrenCaption[q].ToString();
        //        if (ListChildrenPackage[q] != null && ListChildrenPackage[q].ToString() != "" &&
        //            ListChildrenForm[q] != null && ListChildrenForm[q].ToString() != "")
        //        {
        //            nodeChild.PopOutImageUrl = "~\\" + ListChildrenPackage[q].ToString() + "\\" + ListChildrenForm[q].ToString() + ".aspx";
        //        }
        //        else
        //        {
        //            nodeChild.PopOutImageUrl = "~\\DefaultPage2.aspx";
        //        }
        //        if (ListChildrenImage[q] != null && ListChildrenImage[q].ToString() != "")
        //            nodeChild.ImageUrl = defaultDir + ListChildrenImage[q].ToString();
        //        else
        //            nodeChild.ImageUrl = "~/Image/main/b6.gif";
        //        nodeMain[x].ChildItems.AddAt(0, nodeChild);
        //    }
        //}
    }

    private string SetMenuLanguage()
    {
        string strCaption = "";
        switch (CliUtils.fClientLang)
        {
            case SYS_LANGUAGE.ENG:
                strCaption = "caption0";
                break;
            case SYS_LANGUAGE.TRA:
                strCaption = "caption1";
                break;
            case SYS_LANGUAGE.SIM:
                strCaption = "caption2";
                break;
            case SYS_LANGUAGE.HKG:
                strCaption = "caption3";
                break;
            case SYS_LANGUAGE.JPN:
                strCaption = "caption4";
                break;
            case SYS_LANGUAGE.LAN1:
                strCaption = "caption5";
                break;
            case SYS_LANGUAGE.LAN2:
                strCaption = "caption6";
                break;
            case SYS_LANGUAGE.LAN3:
                strCaption = "caption7";
                break;
            default:
                strCaption = "caption";
                break;
        }
        return strCaption;
    }

    protected void ddlSolution_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.menu1.Items.Clear();
        ItemToGet(this.ddlSolution.SelectedValue.ToString());
        CliUtils.fCurrentProject = this.ddlSolution.SelectedValue.ToString();
        if (this.ViewState["menuDs"] != null)
            this.ViewState["menuDs"] = menuDataSet;
        else
            this.ViewState.Add("menuDs", menuDataSet);
    }

    //protected void menu1_MenuItemClick(object sender, MenuEventArgs e)
    //{
    //    if(e.Item.ChildItems.Count == 0)
    //        Page.Response.Redirect(e.Item.PopOutImageUrl);
    //}
}
