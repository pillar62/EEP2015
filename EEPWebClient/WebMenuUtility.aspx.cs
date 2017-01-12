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

public partial class WebMenuUtility : System.Web.UI.Page
{
    private ArrayList MenuIDList = new ArrayList();
    private ArrayList CaptionList = new ArrayList();
    private ArrayList ParentList = new ArrayList();
    private ArrayList ImageList = new ArrayList();
    private ArrayList PackageList = new ArrayList();
    private ArrayList FormList = new ArrayList();
    private DataSet menuDataSet = new DataSet();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession)
        {
            Response.Redirect("InfoLogin.aspx?IsMU=true", true);
        }
        if (!IsPostBack)
        {
            DoLoad();
            ItemToGet(this.ddlSolution.SelectedValue.ToString());
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
        object[] myRet = CliUtils.CallMethod("GLModule", "FetchAllMenus", strParam);
        if ((null != myRet) && (0 == (int)myRet[0]))
        {
            menuDataSet = (DataSet)(myRet[1]);
        }
        MenuIDList.Clear();
        CaptionList.Clear();
        ParentList.Clear();
        int menuCount = menuDataSet.Tables[0].Rows.Count;

        string[] strCaptions = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPManager", "WebManager", "TreeNodeCaption").Split(',');


        MenuIDList.Add("Users");
        CaptionList.Add(strCaptions[0]);
        ParentList.Add("");
        ImageList.Add("");
        PackageList.Add("");
        FormList.Add("");

        MenuIDList.Add("Groups");
        CaptionList.Add(strCaptions[1]);
        ParentList.Add("");
        ImageList.Add("");
        PackageList.Add("");
        FormList.Add("");

        if (IsFlowPage())
        {
            MenuIDList.Add("Organization");
            CaptionList.Add(strCaptions[3]);
            ParentList.Add("");
            ImageList.Add("");
            PackageList.Add("");
            FormList.Add("");

            MenuIDList.Add("OrgLevel");
            CaptionList.Add(strCaptions[4]);
            ParentList.Add("");
            ImageList.Add("");
            PackageList.Add("");
            FormList.Add("");
        }

        MenuIDList.Add("Menus");
        CaptionList.Add(strCaptions[2]);
        ParentList.Add("");
        ImageList.Add("");
        PackageList.Add("");
        FormList.Add("");

        for (int i = 0; i < menuCount; i++)
        {
            MenuIDList.Add(menuDataSet.Tables[0].Rows[i]["MENUID"].ToString());
            CaptionList.Add(menuDataSet.Tables[0].Rows[i]["CAPTION"].ToString());
            if (menuDataSet.Tables[0].Rows[i]["PARENT"].ToString() == "")
                ParentList.Add("Menus");
            else
                ParentList.Add(menuDataSet.Tables[0].Rows[i]["PARENT"].ToString());
            ImageList.Add(menuDataSet.Tables[0].Rows[i]["IMAGEURL"]);
            PackageList.Add(menuDataSet.Tables[0].Rows[i]["PACKAGE"].ToString());
            FormList.Add(menuDataSet.Tables[0].Rows[i]["FORM"].ToString());
        }

        initializeTreeView(MenuIDList, CaptionList, ParentList, ImageList, PackageList, FormList);
    }

    private bool IsFlowPage()
    {
        bool flow = false;
        string config_IsFlow = ConfigurationManager.AppSettings["IsFlow"];
        if (!string.IsNullOrEmpty(config_IsFlow) && string.Compare(config_IsFlow, "true", true) == 0)
        {
            return true;
        }
        string config_FlowSolutions = ConfigurationManager.AppSettings["FlowSolutions"];
        if (!string.IsNullOrEmpty(config_FlowSolutions))
        {
            string[] solutions = config_FlowSolutions.Split(',');
            foreach (string sol in solutions)
            {
                if (string.Compare(sol, CliUtils.fCurrentProject, true) == 0)
                {
                    flow = true;
                    break;
                }
            }
        }
        return flow;
    }

    private void initializeTreeView(ArrayList menuIDList, ArrayList menuCaptionList, ArrayList menuParentIDList, ArrayList imageList
                                , ArrayList packageList, ArrayList formList)
    {
        string defaultDir = "~/Image/MenuTree/";

        ArrayList ListMainID = new ArrayList();
        ArrayList ListMainCaption = new ArrayList();
        ArrayList ListChildrenID = new ArrayList();
        ArrayList ListOwnerParentID = new ArrayList();
        ArrayList ListChildrenCaption = new ArrayList();
        ArrayList ListMainImage = new ArrayList();
        ArrayList ListMainPackage = new ArrayList();
        ArrayList ListMainForm = new ArrayList();
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
        TreeNode[] nodeMain = new TreeNode[i];
        for (int j = 0; j < i; j++)
        {
            nodeMain[j] = new TreeNode();
            tView.Nodes.Add(nodeMain[j]);
            nodeMain[j].Text = ListMainCaption[j].ToString();
            nodeMain[j].Value = ListMainID[j].ToString();
            if (nodeMain[j].Value == "Users")
                nodeMain[j].NavigateUrl = "WebManager/WebUControl.aspx";
            else if (nodeMain[j].Value == "Groups")
            {
                if (IsFlowPage())
                    nodeMain[j].NavigateUrl = "WebManager/WebGControlFL.aspx";
                else
                    nodeMain[j].NavigateUrl = "WebManager/WebGControl.aspx";
            }
            else if (nodeMain[j].Value == "Organization")
                nodeMain[j].NavigateUrl = "WebManager/WebOrganization.aspx";
            else if (nodeMain[j].Value == "OrgLevel")
                nodeMain[j].NavigateUrl = "WebManager/WebOrgLevel.aspx";
            else
                nodeMain[j].NavigateUrl = "WebManager/WebMenuUtilityMain.aspx?MenuID=" + ListMainID[j].ToString();
            nodeMain[j].Target = "main";
            if (ListMainImage[j] != null && ListMainImage[j].ToString() != "")
                nodeMain[j].ImageUrl = defaultDir + ListMainImage[j].ToString();
        }
        int p = ListChildrenID.Count;
        TreeNode[] nodeChildren = new TreeNode[p];
        for (int q = 0; q < p; q++)
        {
            nodeChildren[q] = new TreeNode();
            nodeChildren[q].Text = ListChildrenCaption[q].ToString();
            nodeChildren[q].Value = ListChildrenID[q].ToString();
            if (nodeChildren[q].Value == "Users")
                nodeChildren[q].NavigateUrl = "WebManager/WebUControl.aspx";
            else if (nodeChildren[q].Value == "Groups")
            {
                if (IsFlowPage())
                    nodeChildren[q].NavigateUrl = "WebManager/WebGControlFL.aspx";
                else
                    nodeChildren[q].NavigateUrl = "WebManager/WebGControl.aspx";
            }
            else if (nodeChildren[q].Value == "Organization")
                nodeChildren[q].NavigateUrl = "WebManager/WebOrganization.aspx";
            else if (nodeChildren[q].Value == "OrgLevel")
                nodeChildren[q].NavigateUrl = "WebManager/WebOrgLevel.aspx";
            else
                nodeChildren[q].NavigateUrl = "WebManager/WebMenuUtilityMain.aspx?MenuID=" + ListChildrenID[q].ToString();
            nodeChildren[q].Target = "main";
            if (ListChildrenImage[q] != null && ListChildrenImage[q].ToString() != "")
                nodeChildren[q].ImageUrl = defaultDir + ListChildrenImage[q].ToString();
            //nodeChildren[q].PopulateOnDemand = true;
        }

        for (int q = 0; q < p; q++)
        {
            for (int x = 0; x < ListMainID.Count; x++)
            {
                if (ListOwnerParentID[q].ToString() == ListMainID[x].ToString())
                {
                    nodeMain[x].ChildNodes.Add(nodeChildren[q]);
                }
            }
            for (int s = 0; s < p; s++)
            {
                if (ListOwnerParentID[q].ToString() == ListChildrenID[s].ToString())
                {
                    nodeChildren[s].ChildNodes.Add(nodeChildren[q]);
                }
            }
        }
    }

    //2009/09/11 Modify by eva為配合css套用修改程式
    //protected void btnLogOut_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        Session.Abandon();
    //    }
    //    finally
    //    {
    //        Response.Redirect("InfoLogin.aspx?IsMU=true", true);
    //    }
    //}
    
    //protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    //{
    //    if (IsFlowPage())
    //        Response.Redirect("WebManager\\WebGControlFL.aspx", true);
    //    else
    //        Response.Redirect("WebManager\\WebGControl.aspx", true);
    //}
    protected void btnChangepassword_Click(object sender, EventArgs e)
    {
        if (IsFlowPage())
            Response.Redirect("WebManager/WebGControlFL.aspx", true);
        else
            Response.Redirect("WebManager/WebGControl.aspx", true);
    }
    protected void btnLogOut_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
        }
        finally
        {
            Response.Redirect("InfoLogin.aspx?IsMU=true", true);
        }
    }
    //End Modify

    protected void ddlSolution_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.tView.Nodes.Clear();
        ItemToGet(this.ddlSolution.SelectedValue.ToString());
        if (ViewState["menuDs"] != null)
            ViewState["menuDs"] = menuDataSet;
        else
            this.ViewState.Add("menuDs", menuDataSet);
    }
    
}
