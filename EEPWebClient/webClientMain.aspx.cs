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
using System.IO;
using Srvtools;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Text;

public partial class webClientMain : System.Web.UI.Page, ICallbackEventHandler//, IPostBackEventHandler
{
    private ArrayList MenuIDList = new ArrayList();
    private ArrayList CaptionList = new ArrayList();
    private ArrayList ParentList = new ArrayList();
    private ArrayList ImageList = new ArrayList();
    private ArrayList PackageList = new ArrayList();
    private ArrayList FormList = new ArrayList();
    private ArrayList ItemParam = new ArrayList();
    private DataSet menuDataSet = new DataSet();
    private DataSet menuFavorDataSet = new DataSet();
    private DataSet groupDataSet = new DataSet();
    private InfoDataSet dsSol = new InfoDataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession
            || Session["fLoginUser"] == null  /*||Session["fLoginPassword"] == null*/
            || Session["fLoginDB"] == null || Session["fCurrentProject"] == null)
        {
            Response.Redirect("InfoLogin.aspx", true);
        }
        if (!IsPostBack)
        {
            DoLoad();
            ItemToGet(this.ddlSolution.SelectedValue.ToString());
            this.ViewState.Add("menuDs", menuDataSet);

            object[] isTableExist = CliUtils.CallMethod("GLModule", "isTableExist", new object[] { "MENUFAVOR" });
            if (isTableExist != null && Convert.ToInt16(isTableExist[0]) == 0 && Convert.ToInt16(isTableExist[1]) == 1)
            {
                this.ibGO.Visible = false;
                this.ibMyFavor.Visible = false;
                this.tbCaption.Visible = false;
            }

            this.ibMyFavor.Attributes.Add("onclick", "openFaverMenu();return false");

            ClientScriptManager csm = Page.ClientScript;
            String cbReference = csm.GetCallbackEventReference(this, "arg", "receiveServerData", "", true);
            String callbackScript = "function  OnNodeClick(arg) {" + cbReference + "}";
            csm.RegisterClientScriptBlock(this.GetType(), "OnNodeClick", callbackScript, true);

            this.Page.Response.Write("<script language=\"JavaScript\" type=\"text/JavaScript\">\n");
            this.Page.Response.Write("function receiveServerData(arg) {}\n");
            this.Page.Response.Write("</script>\n");

            //this.Xaml1.InitParameters = this.GenInitParameters();
        }
    }

    private string GenInitParameters()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("startroot=SilverlightTools.SLMenu,");
        builder.Append(",serviceaddress=" + HttpUtility.UrlEncode("http://localhost:4131/EEPWebClient/MenuService.svc"));

        return builder.ToString();
    }

    public void DoLoad()
    {
        DataSet dsSolution = new DataSet();
        if (CliUtils.fSolutionSecurity)
        {
            object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolutionSecurity", new object[] { CliUtils.fLoginUser, CliUtils.fGroupID });
            if ((null != myRet1) && (0 == (int)myRet1[0]))
                dsSolution = ((DataSet)myRet1[1]);
        }
        else
        {
            object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolution", null);
            if ((null != myRet1) && (0 == (int)myRet1[0]))
                dsSolution = ((DataSet)myRet1[1]);
        }
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

        SYS_LANGUAGE language = CliUtils.fClientLang;
        string btnNames = SysMsg.GetSystemMessage(language, "EEPNetClient", "FrmClientMain", "Buttons", true);
        if (!string.IsNullOrEmpty(btnNames))
        {
            this.btnChangepassword.Text = btnNames.Split(';')[0];
            this.btnLogOut.Text = btnNames.Split(';')[1];
        }
    }

    private void ItemToGet(string selValue)
    {
        object[] LoginUser = new object[1];
        LoginUser[0] = CliUtils.fLoginUser;
        object[] strParam = new object[2];
        strParam[0] = selValue;
        strParam[1] = "W";
        MenuIDList.Clear();
        CaptionList.Clear();
        ParentList.Clear();
        int menuCount = 0;
        string strCaption = SetMenuLanguage();
        menuDataSet.Clear();
        object[] myRet = CliUtils.CallMethod("GLModule", "FetchMenus", strParam);
        if ((null != myRet) && (0 == (int)myRet[0]))
        {
            menuDataSet = (DataSet)(myRet[1]);
        }
        ArrayList menuIDs = new ArrayList();
        for (int i = 0; i < menuDataSet.Tables[0].Rows.Count; i++)
        {
            menuIDs.Add(menuDataSet.Tables[0].Rows[i]["MENUID"].ToString());
        }

        //加入Favor
        object[] isTableExist = CliUtils.CallMethod("GLModule", "isTableExist", new object[] { "MENUFAVOR" });
        if (isTableExist != null && Convert.ToInt16(isTableExist[0]) == 0 && Convert.ToInt16(isTableExist[1]) == 0)
        {
            menuFavorDataSet.Clear();
            object[] myRet1 = CliUtils.CallMethod("GLModule", "FetchFavorMenus", strParam);
            if ((null != myRet1) && (0 == (int)myRet1[0]))
            {
                menuFavorDataSet = (DataSet)(myRet1[1]);
                groupDataSet = (DataSet)(myRet1[2]);
            }
            menuCount = menuFavorDataSet.Tables[0].Rows.Count;
            if (menuCount > 0)
            {
                SYS_LANGUAGE language = CliUtils.fClientLang;
                String favor = SysMsg.GetSystemMessage(language, "EEPNetClient", "FavorMenu", "Favor", true);

                MenuIDList.Add("MyFavor");
                CaptionList.Add(favor);
                ParentList.Add("");
                ImageList.Add("");
                PackageList.Add("");
                FormList.Add("");
                ItemParam.Add("");

                for (int i = 0; i < groupDataSet.Tables[0].Rows.Count; i++)
                {
                    if (groupDataSet.Tables[0].Rows[i]["GROUPNAME"] != null && groupDataSet.Tables[0].Rows[i]["GROUPNAME"].ToString() != "")
                    {
                        MenuIDList.Add(groupDataSet.Tables[0].Rows[i]["GROUPNAME"].ToString());
                        CaptionList.Add(groupDataSet.Tables[0].Rows[i]["GROUPNAME"].ToString());
                        ParentList.Add("MyFavor");
                        ImageList.Add("");
                        PackageList.Add("");
                        FormList.Add("");
                        ItemParam.Add("");
                    }
                }

                for (int i = 0; i < menuCount; i++)
                {
                    if (MenuIDList.Contains(menuFavorDataSet.Tables[0].Rows[i]["MENUID"].ToString()))
                        continue;
                    if (menuFavorDataSet.Tables[0].Rows[i]["PARENT"] == null || menuFavorDataSet.Tables[0].Rows[i]["MENUID"].ToString() == "") continue;

                    if (!CheckSecurity(menuFavorDataSet.Tables[0].Rows[i], menuDataSet, menuIDs)) continue;

                    MenuIDList.Add(menuFavorDataSet.Tables[0].Rows[i]["MENUID"].ToString());
                    if (menuFavorDataSet.Tables[0].Rows[i][strCaption].ToString() != "")
                    {
                        CaptionList.Add(menuFavorDataSet.Tables[0].Rows[i][strCaption].ToString());
                    }
                    else
                    {
                        CaptionList.Add(menuFavorDataSet.Tables[0].Rows[i]["CAPTION"].ToString());
                    }
                    if (menuFavorDataSet.Tables[0].Rows[i]["GROUPNAME"] == null || menuFavorDataSet.Tables[0].Rows[i]["GROUPNAME"].ToString() == "")
                        ParentList.Add("MyFavor");
                    else
                        ParentList.Add(menuFavorDataSet.Tables[0].Rows[i]["GROUPNAME"].ToString());
                    ImageList.Add(menuFavorDataSet.Tables[0].Rows[i]["IMAGEURL"]);
                    if (menuFavorDataSet.Tables[0].Rows[i]["ModuleType"].ToString() == "C")
                        PackageList.Add("!" + menuFavorDataSet.Tables[0].Rows[i]["PACKAGE"].ToString());
                    else
                        PackageList.Add(menuFavorDataSet.Tables[0].Rows[i]["PACKAGE"].ToString());
                    FormList.Add(menuFavorDataSet.Tables[0].Rows[i]["FORM"].ToString());
                    ItemParam.Add(menuFavorDataSet.Tables[0].Rows[i]["ITEMPARAM"].ToString());
                }
            }
        }

        menuCount = menuDataSet.Tables[0].Rows.Count;
        for (int i = 0; i < menuCount; i++)
        {
            if (menuDataSet.Tables[0].Rows[i]["MODULETYPE"].ToString() == "W")
            {
                MenuIDList.Add(menuDataSet.Tables[0].Rows[i]["MENUID"].ToString());
                if (menuDataSet.Tables[0].Rows[i][strCaption].ToString() != "")
                {
                    CaptionList.Add(menuDataSet.Tables[0].Rows[i][strCaption].ToString());
                }
                else
                {
                    CaptionList.Add(menuDataSet.Tables[0].Rows[i]["CAPTION"].ToString());
                }
                ParentList.Add(menuDataSet.Tables[0].Rows[i]["PARENT"].ToString());
                ImageList.Add(menuDataSet.Tables[0].Rows[i]["IMAGEURL"]);
                PackageList.Add(menuDataSet.Tables[0].Rows[i]["PACKAGE"].ToString());
                FormList.Add(menuDataSet.Tables[0].Rows[i]["FORM"].ToString());
                ItemParam.Add(menuDataSet.Tables[0].Rows[i]["ITEMPARAM"].ToString());
            }
        }

        strParam[0] = selValue;
        strParam[1] = "C";
        myRet = CliUtils.CallMethod("GLModule", "FetchMenus", strParam);
        if ((null != myRet) && (0 == (int)myRet[0]))
        {
            menuDataSet = (DataSet)(myRet[1]);
        }
        menuCount = menuDataSet.Tables[0].Rows.Count;
        strCaption = SetMenuLanguage();
        for (int i = 0; i < menuCount; i++)
        {
            if (menuDataSet.Tables[0].Rows[i]["MODULETYPE"].ToString() == "C")
            {
                MenuIDList.Add(menuDataSet.Tables[0].Rows[i]["MENUID"].ToString());
                if (menuDataSet.Tables[0].Rows[i][strCaption].ToString() != "")
                {
                    CaptionList.Add(menuDataSet.Tables[0].Rows[i][strCaption].ToString());
                }
                else
                {
                    CaptionList.Add(menuDataSet.Tables[0].Rows[i]["CAPTION"].ToString());
                }
                ParentList.Add(menuDataSet.Tables[0].Rows[i]["PARENT"].ToString());
                ImageList.Add(menuDataSet.Tables[0].Rows[i]["IMAGEURL"]);
                PackageList.Add("!" + menuDataSet.Tables[0].Rows[i]["PACKAGE"].ToString());
                FormList.Add(menuDataSet.Tables[0].Rows[i]["FORM"].ToString());
                ItemParam.Add(menuDataSet.Tables[0].Rows[i]["ITEMPARAM"].ToString());
            }
        }

        initializeTreeView(MenuIDList, CaptionList, ParentList, ImageList, PackageList, FormList);
        SetNodeEnable();
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

    private string SetMenuLanguage()
    {
        string strCaption = "";
        switch (CliUtils.fClientLang)
        {
            case SYS_LANGUAGE.ENG:
                strCaption = "CAPTION0";
                break;
            case SYS_LANGUAGE.TRA:
                strCaption = "CAPTION1";
                break;
            case SYS_LANGUAGE.SIM:
                strCaption = "CAPTION2";
                break;
            case SYS_LANGUAGE.HKG:
                strCaption = "CAPTION3";
                break;
            case SYS_LANGUAGE.JPN:
                strCaption = "CAPTION4";
                break;
            case SYS_LANGUAGE.LAN1:
                strCaption = "CAPTION5";
                break;
            case SYS_LANGUAGE.LAN2:
                strCaption = "CAPTION6";
                break;
            case SYS_LANGUAGE.LAN3:
                strCaption = "CAPTION7";
                break;
            default:
                strCaption = "CAPTION";
                break;
        }
        return strCaption;
    }

    private void _GetAllNodes(TreeNode aNode, ref ArrayList myList)
    {
        for (int i = 0; i < aNode.ChildNodes.Count; i++)
        {
            myList.Add(aNode.ChildNodes[i]);
            if (aNode.ChildNodes[i].ChildNodes.Count > 0)
                _GetAllNodes(aNode.ChildNodes[i], ref myList);
        }
    }

    private ArrayList myList = new ArrayList();
    private ArrayList GetAllNodes(TreeView tree)
    {
        for (int i = 0; i < tree.Nodes.Count; i++)
        {
            myList.Add(tree.Nodes[i]);
            _GetAllNodes(tree.Nodes[i], ref myList);
        }
        return myList;
    }

    private void SetNodeEnable()
    {
        ArrayList nodesList = GetAllNodes(tView);
        int i = nodesList.Count;
        for (int j = 0; j < i; j++)
        {
            if (((TreeNode)nodesList[j]).ChildNodes.Count != 0)
            {
                ((TreeNode)nodesList[j]).SelectAction = TreeNodeSelectAction.None;
            }
        }
    }

    ArrayList ListChildrenID = new ArrayList();
    ArrayList ListOwnerParentID = new ArrayList();
    ArrayList ListChildrenCaption = new ArrayList();
    ArrayList ListChildrenImage = new ArrayList();
    ArrayList ListChildrenPackage = new ArrayList();
    ArrayList ListChildrenForm = new ArrayList();
    ArrayList ListChildrenItemParam = new ArrayList();

    private void initializeTreeView(ArrayList menuIDList, ArrayList menuCaptionList, 
        ArrayList menuParentIDList, ArrayList imageList,
        ArrayList packageList, ArrayList formList)
    {
        string defaultDir = "~/Image/MenuTree/";
        ArrayList ListMainID = new ArrayList();
        ArrayList ListMainCaption = new ArrayList();
        ArrayList ListMainForm = new ArrayList();
        ArrayList ListMainImage = new ArrayList();
        ArrayList ListMainPackage = new ArrayList();
        ArrayList ListMainItemParam = new ArrayList();

        int MenuCount = menuIDList.Count;
        for (int ix = 0; ix < MenuCount; ix++)
        {
            if (menuParentIDList[ix].ToString() == string.Empty || menuParentIDList[ix].ToString() == " ")
            {
                ListMainID.Add(menuIDList[ix].ToString());
                ListMainCaption.Add(menuCaptionList[ix].ToString());
                ListMainImage.Add(imageList[ix].ToString());
                ListMainPackage.Add(packageList[ix].ToString());
                ListMainForm.Add(formList[ix].ToString());
                ListMainItemParam.Add(ItemParam[ix].ToString());
            }
            else
            {
                ListChildrenID.Add(menuIDList[ix].ToString());
                ListOwnerParentID.Add(menuParentIDList[ix].ToString());
                ListChildrenCaption.Add(menuCaptionList[ix].ToString());
                ListChildrenImage.Add(imageList[ix].ToString());
                ListChildrenPackage.Add(packageList[ix].ToString());
                ListChildrenForm.Add(formList[ix].ToString());
                ListChildrenItemParam.Add(ItemParam[ix].ToString());
            }
        }

        int i = ListMainID.Count;
        TreeNode[] nodeMain = new TreeNode[i];
        for (int j = 0; j < i; j++)
        {
            nodeMain[j] = new TreeNode();
            tView.Nodes.Add(nodeMain[j]);
            nodeMain[j].Value = ListMainID[j].ToString();
            nodeMain[j].Text = ListMainCaption[j].ToString();
            if (ListMainPackage[j] != null && ListMainPackage[j].ToString() != "" &&
                ListMainForm[j] != null && ListMainForm[j].ToString() != "")
            {
                string param = ConvertParamter(ListMainItemParam[j].ToString());
                if (ListMainPackage[j].ToString().StartsWith("!"))
                {
                    nodeMain[j].NavigateUrl = "~\\WebSingleSignOn.aspx?Package=" + ListMainPackage[j].ToString().Substring(1) + "&Form=" + ListMainForm[j].ToString() + "&" + param;
                }
                else
                {
                    nodeMain[j].NavigateUrl = "~\\" + ListMainPackage[j].ToString() + "\\" + ListMainForm[j].ToString() + ".aspx?" + param;
                }
            }
            else
            {
                nodeMain[j].NavigateUrl = "~\\DefaultPage.aspx";
            }
            nodeMain[j].Target = "main";
            if (ListMainImage[j] != null && ListMainImage[j].ToString().Trim() != "")
                nodeMain[j].ImageUrl = defaultDir + ListMainImage[j].ToString();
        }

        List<TreeNode> emptynodes = new List<TreeNode>();
        for (int j = 0; j < tView.Nodes.Count; j++)
        {
            InitialNode(tView.Nodes[j]);
            if (tView.Nodes[j].ChildNodes.Count == 0)
            {
                if (ListMainPackage[j].ToString().Length == 0)
                {
                    emptynodes.Add(tView.Nodes[j]);
                }
            }
        }
        foreach (TreeNode node in emptynodes)
        {
            tView.Nodes.Remove(node);
        }
    }

    private void InitialNode(TreeNode node)
    {
        for (int i = 0; i < ListChildrenID.Count; i++)
        {
            if (node.Value.Equals(ListOwnerParentID[i].ToString()))
            {
                TreeNode nodeChild = new TreeNode();
                nodeChild.Value = ListChildrenID[i].ToString();
                nodeChild.Text = ListChildrenCaption[i].ToString();
                string param = ConvertParamter(ListChildrenItemParam[i].ToString());
                if (ListChildrenPackage[i].ToString().Contains(":"))
                {
                    nodeChild.NavigateUrl = ListChildrenPackage[i].ToString() + "?" + param;
                    nodeChild.Target = "_blank";
                }
                else
                {
                    if (ListChildrenPackage[i].ToString().StartsWith("!"))
                    {
                        nodeChild.NavigateUrl = "~\\WebSingleSignOn.aspx?Package=" + ListChildrenPackage[i].ToString().Substring(1) + "&Form=" + ListChildrenForm[i].ToString() + "&" + param;
                    }
                    else
                    {
                        nodeChild.NavigateUrl = "~\\" + ListChildrenPackage[i].ToString() + "\\" + ListChildrenForm[i].ToString() + ".aspx?" + param;
                    }
                    String str = nodeChild.Text;
                    nodeChild.Target = "main" + "\" onclick=\"javascript: OnNodeClick(\'" + str + "\');";
                }
                if (ListChildrenImage[i] != null && ListChildrenImage[i].ToString().Trim() != "")
                    nodeChild.ImageUrl = "~/Image/MenuTree/" + ListChildrenImage[i].ToString();
                node.ChildNodes.Add(nodeChild);
            }
        }
        for (int i = 0; i < node.ChildNodes.Count; i++)
        {
            InitialNode(node.ChildNodes[i]);
        }
    }

    private string ConvertParamter(string obj)
    {
        string param = "";
        if (obj.ToLower().StartsWith("flowfilename"))
        {
            string[] arrParam = obj.Split(';');
            foreach (string pa in arrParam)
            {
                param += HttpUtility.UrlEncode(pa).Replace(HttpUtility.UrlEncode("="), "=") + "&";
            }
            if (param != "")
                param = param.Substring(0, param.LastIndexOf('&'));
        }
        else
        {
            param = "ItemParam=" + HttpUtility.UrlEncode(obj);
        }
        return param;
    }

    protected void ddlSolution_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.tView.Nodes.Clear();
        ItemToGet(this.ddlSolution.SelectedValue.ToString());
        CliUtils.fCurrentProject = this.ddlSolution.SelectedValue.ToString();
        if (ViewState["menuDs"] != null)
            ViewState["menuDs"] = menuDataSet;
        else
            this.ViewState.Add("menuDs", menuDataSet);
    }

    protected void btnLogOut_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Session.Abandon();
        }
        finally
        {
            Response.Redirect("InfoLogin.aspx", true);
        }
    }

    protected void btnLogOut_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
        }
        finally
        {
            Response.Redirect("InfoLogin.aspx", true);
        }
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("WebUPWDControl.aspx", true);
    }

    protected void btnChangepassword_Click(object sender, EventArgs e)
    {
        Response.Redirect("WebUPWDControl.aspx", true);
    }

    protected void ibMyFavor_Click(object sender, ImageClickEventArgs e)
    {
        if (this.tView.SelectedNode != null)
        {
            char[] temp = { '\\', '.' };
            String[] package = this.tView.SelectedNode.NavigateUrl.Split(temp);
            object[] param = new object[2];
            param[0] = package[1] + "\\" + package[2];
            param[1] = CliUtils.fLoginUser;
            object[] myRet = CliUtils.CallMethod("GLModule", "GetFavorMenuID", param);
            if (myRet != null)
            {
                switch (myRet[1].ToString())
                {
                    case "0":
                        this.Page.Response.Write("<script>alert(\"Add successed.\");</script>");
                        break;
                    case "1":
                        this.Page.Response.Write("<script>alert(\"This menu is already exsit.\");</script>");
                        break;
                }
            }
        }
        else
        {
            this.Page.Response.Write("<script>alert(\"Please choose a menu first.\");</script>");
        }
    }

    public String getChildNode(TreeNode tn, String text)
    {
        String url = "";
        if (compareCaption(text, tn.Text))
        {
            if (tn.ChildNodes.Count == 0)
            {
                url = tn.NavigateUrl;
            }
            else
            {
                url = "DefaultPage.aspx";
            }
            return url;
        }
        if (tn.ChildNodes.Count > 0)
        {
            for (int i = 0; i < tn.ChildNodes.Count; i++)
            {
                url = getChildNode(tn.ChildNodes[i], text);
                if (url != "")
                    return url;
            }
        }
        return url;
    }

    private bool compareCaption(String text, String nodeText)
    {
        if (nodeText.Contains(text))
            return true;
        else
            return false;
    }

    protected void ibGO_Click(object sender, ImageClickEventArgs e)
    {
        String url = "";
        for (int i = 0; i < this.tView.Nodes.Count; i++)
        {
            url = getChildNode(this.tView.Nodes[i], this.tbCaption.Text);
            if (url != "")
                break;
        }
        if (url != "")
        {
            if (url.StartsWith("~\\"))
                url = url.Substring(url.IndexOf("~\\") + 2);
            this.main.Attributes["src"] = url;
        }
        else
            this.Page.Response.Write("<script>alert(\"The menu you entered is not exsit.\");</script>");
    }

    protected void ibGO_Click(object sender, EventArgs e)
    {
        String url = "";
        for (int i = 0; i < this.tView.Nodes.Count; i++)
        {
            url = getChildNode(this.tView.Nodes[i], this.tbCaption.Text);
            if (url != "")
                break;
        }
        if (url != "")
        {
            if (url.StartsWith("~\\"))
                url = url.Substring(url.IndexOf("~\\") + 2);
            this.main.Attributes["src"] = url;
        }
        else
            this.Page.Response.Write("<script>alert(\"The menu you entered is not exsit.\");</script>");
    }

    protected void tView_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {

    }

    #region ICallbackEventHandler 成员

    public string GetCallbackResult()
    {
        return "";
    }

    public void RaiseCallbackEvent(string eventArgument)
    {
        if (CliUtils.fLogMenuOpenForm)
        {
            CliUtils.LogToSystem("Open WebForm", eventArgument, true, 7, 0);
        }
    }

    #endregion

}