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
using System.Drawing;
using System.Collections.Generic;

public partial class webClientMainFlow : System.Web.UI.Page
{
    private bool AutoHidePanel = true;

    private ArrayList MenuIDList = new ArrayList();
    private ArrayList CaptionList = new ArrayList();
    private ArrayList ParentList = new ArrayList();
    private ArrayList ImageList = new ArrayList();
    private ArrayList PackageList = new ArrayList();
    private ArrayList FormList = new ArrayList();
    private ArrayList ItemParam = new ArrayList();
    private ArrayList ModuleTypeList = new ArrayList();
    private DataSet menuDataSet = new DataSet();
    private DataSet menuFavorDataSet = new DataSet();
    private DataSet groupDataSet = new DataSet();
    private InfoDataSet dsSol = new InfoDataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession
            || Session["fLoginUser"] == null
            || Session["fLoginDB"] == null || Session["fCurrentProject"] == null)
        {
            if (this.Request["FLOWPATH"] != null)
            {


                throw new Exception("75FF57F7-7AC0-43c8-9454-C92B4A2723BB");
              
                return;
             
            }
            else
            {
                Response.Redirect("InfoLogin.aspx", true);
            }
        }
        if (!IsPostBack)
        {
            DoLoad();
            ItemToGet(this.ddlSolution.SelectedValue.ToString());
            this.ViewState.Add("menuDs", menuDataSet);

            string selOption = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "SelectOption", true);

            //DataTable list = null;
            //object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { "SELECT DISTINCT SYS_TODOLIST.FLOW_DESC FROM SYS_TODOLIST" });
            //if (ret1 != null && (int)ret1[0] == 0)
            //{
            //    list = ((DataSet)ret1[1]).Tables[0];
            //}
            //this.ddlToDoListFilter.DataSource = list;
            //this.ddlToDoListFilter.DataValueField = "FLOW_DESC";
            //this.ddlToDoListFilter.DataTextField = "FLOW_DESC";
            //this.ddlToDoListFilter.DataBind();
            //this.ddlToDoListFilter.Items.Insert(0, "<--" + selOption + "-->");

            //DataTable notify = list.Copy();
            //this.ddlNotifyFilter.DataSource = notify;
            //this.ddlNotifyFilter.DataValueField = "FLOW_DESC";
            //this.ddlNotifyFilter.DataTextField = "FLOW_DESC";
            //this.ddlNotifyFilter.DataBind();
            //this.ddlNotifyFilter.Items.Insert(0, "<--" + selOption + "-->");

            //DataTable his = list.Copy();
            //this.ddlToDoHisFilter.DataSource = his;
            //this.ddlToDoHisFilter.DataValueField = "FLOW_DESC";
            //this.ddlToDoHisFilter.DataTextField = "FLOW_DESC";
            //this.ddlToDoHisFilter.DataBind();
            //this.ddlToDoHisFilter.Items.Insert(0, "<--" + selOption + "-->");

            //DataTable lstQueryFlow = list.Copy();
            //this.ddlLstQueryFlow.DataSource = lstQueryFlow;
            //this.ddlLstQueryFlow.DataValueField = "FLOW_DESC";
            //this.ddlLstQueryFlow.DataTextField = "FLOW_DESC";
            //this.ddlLstQueryFlow.DataBind();
            //this.ddlLstQueryFlow.Items.Insert(0, "<--" + selOption + "-->");

            //DataTable hisQueryFlow = list.Copy();
            //this.ddlHisQueryFlow.DataSource = hisQueryFlow;
            //this.ddlHisQueryFlow.DataValueField = "FLOW_DESC";
            //this.ddlHisQueryFlow.DataTextField = "FLOW_DESC";
            //this.ddlHisQueryFlow.DataBind();
            //this.ddlHisQueryFlow.Items.Insert(0, "<--" + selOption + "-->");

            //DataTable hisQuery2Flow = list.Copy();
            //this.ddlHisQuery2Flow.DataSource = hisQuery2Flow;
            //this.ddlHisQuery2Flow.DataValueField = "FLOW_DESC";
            //this.ddlHisQuery2Flow.DataTextField = "FLOW_DESC";
            //this.ddlHisQuery2Flow.DataBind();
            //this.ddlHisQuery2Flow.Items.Insert(0, "<--" + selOption + "-->");

            DataTable users = null;
            object[] ret2 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { "SELECT USERS.USERID, USERS.USERNAME FROM USERS WHERE USERS.USERID IN (SELECT USERGROUPS.USERID FROM USERGROUPS WHERE USERGROUPS.GROUPID IN(SELECT GROUPS.GROUPID FROM GROUPS WHERE GROUPS.ISROLE='Y'))" });
            if (ret2 != null && (int)ret2[0] == 0)
            {
                users = ((DataSet)ret2[1]).Tables[0];
            }
            this.ddlLstQueryUser.DataSource = users;
            this.ddlLstQueryUser.DataValueField = "USERID";
            this.ddlLstQueryUser.DataValueField = "USERNAME";
            this.ddlLstQueryUser.DataBind();
            this.ddlLstQueryUser.Items.Insert(0, "<--" + selOption + "-->");

            DataTable roles = null;
            object[] ret3 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { "SELECT GROUPID, GROUPNAME FROM GROUPS WHERE ISROLE='Y'" });
            if (ret3 != null && (int)ret3[0] == 0)
            {
                roles = ((DataSet)ret3[1]).Tables[0];
            }
            this.ddlHisQuerySendTo.DataSource = roles;
            this.ddlHisQuerySendTo.DataValueField = "GROUPID";
            this.ddlHisQuerySendTo.DataValueField = "GROUPNAME";
            this.ddlHisQuerySendTo.DataBind();
            this.ddlHisQuerySendTo.Items.Insert(0, "<--" + selOption + "-->");

            if (this.AutoHidePanel)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "adjustpanel", "var autoHidePanel=true;", true);
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "adjustpanel", "var autoHidePanel=false;", true);
            }

            this.ibMyFavor.Attributes.Add("onclick", "openFaverMenu();return false");
        }
    }

    protected void wizToDoList_Refreshed(object sender, EventArgs e)
    {
        FillFilterCombo(this.dgvToDoList, this.ddlToDoListFilter);
    }

    protected void wizToDoHis_Refreshed(object sender, EventArgs e)
    {
        FillFilterCombo(this.dgvToDoHis, this.ddlToDoHisFilter);
    }

    protected void wizFlowRunOver_Refreshed(object sender, EventArgs e)
    {
        FillFilterCombo(this.dgvFlowRunOver, this.ddlToDoHisFilter);
    }

    protected void wizNotify_Refreshed(object sender, EventArgs e)
    {
        FillFilterCombo(this.dgvNotify, this.ddlNotifyFilter);
    }

    void FillFilterCombo(GridView grid, DropDownList list)
    {
        DataView view = grid.DataSource as DataView;
        if (view != null)
        {
            foreach (DataRowView row in view)
            {
                string value = row["FLOW_DESC"].ToString();
                bool exist = false;
                foreach (ListItem item in list.Items)
                {
                    if (item.Text == value)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                {
                    list.Items.Add(new ListItem(value));
                }
            }
            string selOption = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "SelectOption", true);
            list.Items.Insert(0, "<--" + selOption + "-->");
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
            if (dsSolution.Tables[0].Rows[j][0].ToString().ToUpper() == Session["fCurrentProject"].ToString().ToUpper())
            {
                this.ddlSolution.SelectedValue = dsSolution.Tables[0].Rows[j][0].ToString();
                break;
            }
        }
        string[] toDoListCols = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "ToDoListColumns", true).Split(',');
        this.chkSubmitted.Text = toDoListCols[10];
        this.ddlLevel.SelectedIndex = 2;


        string btnNames = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "Buttons", true);
        if (!string.IsNullOrEmpty(btnNames))
        {
            this.btnChangepassword.Text = btnNames.Split(';')[0];
            this.btnLogOut.Text = btnNames.Split(';')[1];
        }
        string queryBtnNames = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebNavigator", "ButtonName", true);
        if (!string.IsNullOrEmpty(queryBtnNames))
        {
            this.lstQueryOK.Text = this.hisQueryOK.Text = this.hisQuery2OK.Text = queryBtnNames.Split(';')[0];
            this.lstQueryCancel.Text = this.hisQueryCancel.Text = this.hisQuery2Cancel.Text = queryBtnNames.Split(';')[1];
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

        //���Favor
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
                String favor = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FavorMenu", "Favor", true);

                MenuIDList.Add("MyFavor");
                CaptionList.Add(favor);
                ParentList.Add("");
                ImageList.Add("");
                PackageList.Add("");
                FormList.Add("");
                ItemParam.Add("");
                ModuleTypeList.Add("W");

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
                        ModuleTypeList.Add("W");
                    }
                }

                for (int i = 0; i < menuCount; i++)
                {
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
                    ModuleTypeList.Add(menuFavorDataSet.Tables[0].Rows[i]["MODULETYPE"].ToString());
                }
            }
        }

        menuCount = menuDataSet.Tables[0].Rows.Count;
        for (int i = 0; i < menuCount; i++)
        {
            if (menuDataSet.Tables[0].Rows[i]["MODULETYPE"].ToString() == "W" || menuDataSet.Tables[0].Rows[i]["MODULETYPE"].ToString() == "O")
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
                ModuleTypeList.Add(menuDataSet.Tables[0].Rows[i]["MODULETYPE"].ToString());
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
                ModuleTypeList.Add(menuDataSet.Tables[0].Rows[i]["MODULETYPE"].ToString());
            }
        }

        initializeTreeView(MenuIDList, CaptionList, ParentList, ImageList, ModuleTypeList, PackageList, FormList, ItemParam);
        SetNodeEnable();
        SetLanguage();
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

    private void SetLanguage()
    {
        string[] uiText = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Web", "webClientMainFlow", "UIText", true).Split(';');
        this.dgvToDoList.EmptyDataText = uiText[0];
        this.dgvToDoHis.EmptyDataText = uiText[0];
        this.dgvNotify.EmptyDataText = uiText[0];
        this.dgvOvertime.EmptyDataText = uiText[0];

        this.lnkRefresh.Text = uiText[1];
        this.lnkHisRefresh.Text = uiText[1];
        this.lnkNotifyRefresh.Text = uiText[1];
        this.lnkOvertimeRefresh.Text = uiText[1];
        //this.legLst.InnerHtml = uiText[2];
        //this.legHis.InnerHtml = uiText[3];
        //this.legOvertime.InnerHtml = uiText[4];
        this.wftitle.InnerHtml = uiText[11];
        this.Label1.Text = uiText[13];
        this.cpeDemo.CollapsedText = uiText[12];
        this.cpeDemo.ExpandedText = uiText[13];
        this.lnkToDoListQuery.Text = uiText[17];
        this.lnkToDoHisQuery.Text = uiText[17];

        this.lbApproveAll.Text = uiText[19];
        this.lbReturnAll.Text = uiText[20];
    }

    public string GetToolTip(int i)
    {
        return SysMsg.GetSystemMessage(CliUtils.fClientLang, "Web", "webClientMainFlow", "UIText", true).Split(';')[i];
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
    ArrayList ListChildrenParam = new ArrayList();
    ArrayList listChildModuleType = new ArrayList();

    private void initializeTreeView(ArrayList menuIDList, ArrayList menuCaptionList,
    ArrayList menuParentIDList, ArrayList imageList, ArrayList moduleTypeList,
    ArrayList packageList, ArrayList formList, ArrayList paramList)
    {
        string defaultDir = "~/Image/MenuTree/";
        ArrayList ListMainID = new ArrayList();
        ArrayList ListMainCaption = new ArrayList();
        ArrayList ListMainForm = new ArrayList();
        ArrayList ListMainImage = new ArrayList();
        ArrayList ListMainPackage = new ArrayList();
        ArrayList ListMainItemParam = new ArrayList();
        ArrayList listMainModuleType = new ArrayList();

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
                ListMainItemParam.Add(paramList[ix].ToString());
                listMainModuleType.Add(moduleTypeList[ix].ToString());
            }
            else
            {
                ListChildrenID.Add(menuIDList[ix].ToString());
                ListOwnerParentID.Add(menuParentIDList[ix].ToString());
                ListChildrenCaption.Add(menuCaptionList[ix].ToString());
                ListChildrenImage.Add(imageList[ix].ToString());
                ListChildrenPackage.Add(packageList[ix].ToString());
                ListChildrenForm.Add(formList[ix].ToString());
                ListChildrenParam.Add(paramList[ix].ToString());
                listChildModuleType.Add(moduleTypeList[ix].ToString());
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
            if (ListMainForm[j] != null && ListMainForm[j].ToString() != "")
            {
                if (listMainModuleType[j].ToString() == "W")
                {
                    object obj = ListMainItemParam[j];
                    if (obj != null && obj.ToString() != "")
                    {
                        string param = this.ConvertParamter(obj.ToString());
                        nodeMain[j].NavigateUrl = "~\\" + ListMainPackage[j].ToString() + "\\" + ListMainForm[j].ToString() + ".aspx?" + param;
                    }
                    else
                    {
                        nodeMain[j].NavigateUrl = "~\\" + ListMainPackage[j].ToString() + "\\" + ListMainForm[j].ToString() + ".aspx";
                    }
                }
                else if (listMainModuleType[j].ToString() == "O")
                {
                    nodeMain[j].NavigateUrl = "~\\InnerPages\\FlowDesigner.aspx?FlowFileName=" + HttpUtility.UrlEncode(ListMainForm[j].ToString());
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
                string param = ConvertParamter(ListChildrenParam[i].ToString());
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
                        if (listChildModuleType[i].ToString() == "W")
                        {
                            if (!string.IsNullOrEmpty(param))
                            {
                                nodeChild.NavigateUrl = "~\\" + ListChildrenPackage[i].ToString() + "\\" + ListChildrenForm[i].ToString() + ".aspx?" + param;
                            }
                            else
                            {
                                nodeChild.NavigateUrl = "~\\" + ListChildrenPackage[i].ToString() + "\\" + ListChildrenForm[i].ToString() + ".aspx";
                            }
                        }
                        else if (listChildModuleType[i].ToString() == "O")
                        {
                            nodeChild.NavigateUrl = "~\\InnerPages\\FlowDesigner.aspx?FlowFileName=" + HttpUtility.UrlEncode(ListChildrenForm[i].ToString());
                        }
                    }
                    String str = nodeChild.Text;
                    nodeChild.Target = "main";
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


    protected void ddlSolution_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.MultiView1.ActiveViewIndex = 0;
        this.WebMultiViewCaptions1.ActiveIndex = 0;

        this.tView.Nodes.Clear();
        ItemToGet(this.ddlSolution.SelectedValue.ToString());
        CliUtils.fCurrentProject = this.ddlSolution.SelectedValue.ToString();
        if (ViewState["menuDs"] != null)
            ViewState["menuDs"] = menuDataSet;
        else
            this.ViewState.Add("menuDs", menuDataSet);
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

    protected void btnChangepassword_Click(object sender, EventArgs e)
    {
        Response.Redirect("WebUPWDControl.aspx", true);
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

    protected void tView_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        //ArrayList ListChildrenID = (ArrayList)this.ViewState["ListChildrenID"];
        //ArrayList ListOwnerParentID = (ArrayList)this.ViewState["ListOwnerParentID"];
        //ArrayList ListChildrenCaption = (ArrayList)this.ViewState["ListChildrenCaption"];
        //ArrayList ListChildrenImage = (ArrayList)this.ViewState["ListChildrenImage"];
        //ArrayList ListChildrenPackage = (ArrayList)this.ViewState["ListChildrenPackage"];
        //ArrayList ListChildrenForm = (ArrayList)this.ViewState["ListChildrenForm"];
        //ArrayList ListChildrenParam = (ArrayList)this.ViewState["ListChildrenParam"];
        //ArrayList ListChildrenModuleType = (ArrayList)this.ViewState["ListChildrenModuleType"];

        //int p = ListChildrenID.Count;
        //string defaultDir = "~/Image/MenuTree/";
        //for (int q = p - 1; q >= 0; q--)
        //{
        //    if (e.Node.Value == ListOwnerParentID[q].ToString())
        //    {
        //        TreeNode nodeChild = new TreeNode();
        //        nodeChild.Value = ListChildrenID[q].ToString();
        //        nodeChild.Text = ListChildrenCaption[q].ToString();
        //        if (ListChildrenPackage[q] != null && ListChildrenPackage[q].ToString() != "" &&
        //            ListChildrenForm[q] != null && ListChildrenForm[q].ToString() != "")
        //        {
        //            if (ListChildrenModuleType[q].ToString() == "W")
        //            {
        //                object obj = ListChildrenParam[q];
        //                if (obj != null && obj.ToString() != "")
        //                {
        //                    string param = this.ConvertParamter(obj.ToString());
        //                    nodeChild.NavigateUrl = "~\\" + ListChildrenPackage[q].ToString() + "\\" + ListChildrenForm[q].ToString() + ".aspx?" + param;
        //                }
        //                else
        //                {
        //                    nodeChild.NavigateUrl = "~\\" + ListChildrenPackage[q].ToString() + "\\" + ListChildrenForm[q].ToString() + ".aspx";
        //                }
        //            }
        //            else if (ListChildrenModuleType[q].ToString() == "O")
        //            {
        //                nodeChild.NavigateUrl = "~\\InnerPages\\FlowDesigner.aspx?FlowFileName=" + HttpUtility.UrlEncode(ListChildrenForm[q].ToString());
        //            }
        //        }
        //        else
        //        {
        //            nodeChild.NavigateUrl = "~\\DefaultPage.aspx";
        //            nodeChild.PopulateOnDemand = true;
        //        }
        //        nodeChild.Target = "main";
        //        if (ListChildrenImage[q] != null && ListChildrenImage[q].ToString() != "")
        //            nodeChild.ImageUrl = defaultDir + ListChildrenImage[q].ToString();
        //        e.Node.ChildNodes.AddAt(0, nodeChild);
        //    }
        //}
        //e.Node.SelectAction = TreeNodeSelectAction.None;
    }

    protected void dgvToDoList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //setToDoListLinkEnabled(this.dgvToDoList.SelectedRow.Cells[17].Text);

        //this.lnkOpen.NavigateUrl = this.wizToDoList.OpenUrl();
        //if (this.lnkApprove.Enabled)
        //    this.lnkApprove.OnClientClick = this.wizToDoList.ApproveUrl();
        //else
        //    this.lnkApprove.OnClientClick = "";

        //if(this.lnkReturn.Enabled)
        //    this.lnkReturn.OnClientClick = this.wizToDoList.ReturnUrl();
        //else
        //    this.lnkReturn.OnClientClick = this.lnkApprove.OnClientClick = "";
    }

    protected void dgvToDoHis_SelectedIndexChanged(object sender, EventArgs e)
    {
        //this.lnkHisOpen.NavigateUrl = this.wizToDoHis.OpenUrl();
    }

    /*private void setToDoListLinkEnabled(string flNavMode)
    {
        switch (flNavMode)
        {
            case "0": //Submit
            case "3": //Notify
            case "4": //Inquery
            case "5": //Continue
            case "6": //None
                this.lnkApprove.Enabled = false;
                this.lnkReject.Enabled = false;
                this.lnkReturn.Enabled = false;
                break;
            case "1": //Approve
                this.lnkApprove.Enabled = true;
                this.lnkReject.Enabled = false;
                this.lnkReturn.Enabled = true;
                break;
            case "2": //Return
                this.lnkApprove.Enabled = false;
                this.lnkReject.Enabled = true;
                this.lnkReturn.Enabled = false;
                break;
        }
    }*/

    protected void lnkRefresh_Click(object sender, EventArgs e)
    {
        this.wizToDoList.Refresh();
        string script = " var cpe = $find(\"flowCollaspibleBehavior\");if (cpe.get_Collapsed()) {cpe._doOpen();}";
        CliUtils.RegisterStartupScript(lnkRefresh, script);
    }

    protected void lnkNotifyRefresh_Click(object sender, EventArgs e)
    {
        this.wizNotify.Refresh();
    }

    protected void lnkHisRefresh_Click(object sender, EventArgs e)
    {
        if (chkSubmitted.Checked)
        {
            this.wizFlowRunOver.Refresh();
        }
        else
        {
            this.wizToDoHis.Refresh();
        }
    }

    public bool ConvertStringToBoolean(object i)
    {
        return Convert.ToBoolean(Convert.ToInt16(i.ToString()));
    }

    public bool ReturnVisible()
    {
        return (this.wizToDoHis.SqlMode != FLTools.ESqlMode.FlowRunOver);
    }

    public bool RetakeVisible(object row)
    {
        if (((DataRowView)row).DataView.Table.Columns.Contains("Status"))
        {
            object status = ((DataRowView)row)["Status"];
            if (status != null && (status.ToString() == "NR" || status.ToString() == "A"))
            {
                return false;
            }
        }
        if (((DataRowView)row).DataView.Table.Columns.Contains("PLUSROLES"))
        {
            object plusroles = ((DataRowView)row)["PLUSROLES"];
            if (plusroles != null && plusroles.ToString().Length > 0)
            {
                return false;
            }
        }
        if (((DataRowView)row).DataView.Table.Columns.Contains("S_USER_ID"))
        {
            object user = ((DataRowView)row)["S_USER_ID"];
            if (user != null && user.ToString() == CliUtils.fLoginUser)
            {
                return true;
            }
        }


        return (this.wizToDoHis.SqlMode != FLTools.ESqlMode.FlowRunOver);
    }

    protected void dgvNotify_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "FlowDelete")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            TableCellCollection cells = dgvNotify.Rows[rowIndex].Cells;
            int cellLength = cells.Count;
            string listId = cells[cellLength - 42].Text;
            string flowPath = cells[cellLength - 5].Text;
            object[] objParams = CliUtils.CallFLMethod("DeleteNotify", new object[] { listId, flowPath });
            if (Convert.ToInt16(objParams[0]) == 0)
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLNavigator", "FlowReject", true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel5, this.GetType(), "FlowDelete1", "alert('" + message + "');", true);
            }
            else if (Convert.ToInt16(objParams[0]) == 2)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel5, this.GetType(), "FlowDelete2", "alert('" + objParams[1].ToString() + "');", true);
            }
            this.wizNotify.Refresh();
        }
    }

    protected void dgvToDoHis_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Return" || e.CommandName == "Notify")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            TableCellCollection cells = dgvToDoHis.Rows[rowIndex].Cells;
            int cellLength = cells.Count;
            string listId = cells[cellLength - 42].Text;
            string stepid = cells[cellLength - 35].Text;
            if (e.CommandName == "Return")
            {
                string sql = "SELECT * FROM SYS_TODOHIS WHERE LISTID='" + listId + "' AND D_STEP_ID='" + stepid + "' ORDER BY UPDATE_TIME DESC";
                DataSet ds = null;
                object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sql });
                if (ret1 != null && (int)ret1[0] == 0)
                {
                    ds = (DataSet)ret1[1];
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    string currentFLActivityName = row["S_STEP_ID"].ToString();
                    string keys = row["FORM_KEYS"].ToString();
                    string keyvalues = row["FORM_PRESENTATION"].ToString();
                    keyvalues = keyvalues.Replace("'", "''");

                    object[] objParams = CliUtils.CallFLMethod("Retake", new object[] { new Guid(listId), new object[] { currentFLActivityName, "" }, new object[] { keys, keyvalues } });
                    if (Convert.ToInt16(objParams[0]) == 0)
                    {
                        this.wizToDoHis.Refresh();
                        ScriptManager.RegisterStartupScript(this.UpdatePanel2, this.Page.GetType(), "ScriptBlock4", "alert('" + SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLWizard", "RetakeSucess", true) + "')", true);
                    }
                    else if (Convert.ToInt16(objParams[0]) == 2)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel2, this.Page.GetType(), "ScriptBlock5", "alert('" + objParams[1].ToString() + "')", true);
                    }
                }
            }
            else if (e.CommandName == "Notify")
            {
                string flowPath = HttpUtility.UrlEncode(cells[cellLength - 5].Text.Split(';')[1]);
                string provider = HttpUtility.UrlEncode(cells[cellLength - 14].Text);
                string sendToId = HttpUtility.UrlEncode(cells[cellLength - 24].Text);
                string sendToKind = HttpUtility.UrlEncode(cells[cellLength - 25].Text);
                string sendToName = HttpUtility.UrlEncode(cells[cellLength - 23].Text);
                string nkeys = HttpUtility.UrlEncode(cells[cellLength - 18].Text);
                //string nvalues = HttpUtility.UrlEncode(cells[cellLength - 17].Text.Replace("''", "$$$"));
                string nvalues = HttpUtility.HtmlDecode(cells[cellLength - 17].Text).Replace("'", "\\'");

                string script = string.Format("var values = encodeURIComponent('{7}');window.open('InnerPages/FlowUrge.aspx?LISTID={0}&FLOWPATH={1}&PROVIDER={2}&SENDTOID={3}&SENDTOKIND={4}&SENDTONAME={5}&KEYS={6}&VALUES='+ values,'','left=200px,top=180px,width=500px,height=330px,toolbar=no,status=yes,scrollbars,resizable');", listId, flowPath, provider, sendToId, sendToKind, sendToName, nkeys, nvalues);
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, this.Page.GetType(), "ScriptBlock5", script, true);
            }
        }
    }

    protected void ddlToDoListFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlToDoListFilter.SelectedIndex != 0)
            this.wizToDoList.Filter = "SYS_TODOLIST.FLOW_DESC='" + ddlToDoListFilter.SelectedValue + "'";
        else
            this.wizToDoList.Filter = "";
        this.wizToDoList.Refresh();
    }
    protected void ddlToDoHisFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlToDoHisFilter.SelectedIndex > 0)
        {
            if (wizToDoHis.SqlMode == FLTools.ESqlMode.ToDoHis)
                this.wizToDoHis.Filter = "SYS_TODOLIST.FLOW_DESC='" + ddlToDoHisFilter.SelectedValue + "'";
            else if (wizToDoHis.SqlMode == FLTools.ESqlMode.FlowRunOver)
                this.wizToDoHis.Filter = "SYS_TODOHIS.FLOW_DESC='" + ddlToDoHisFilter.SelectedValue + "'";
        }
        else
            this.wizToDoHis.Filter = "";
        this.wizToDoHis.Refresh();
    }
    protected void ddlNotifyFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlNotifyFilter.SelectedIndex != 0)
            this.wizNotify.Filter = "SYS_TODOLIST.FLOW_DESC='" + ddlNotifyFilter.SelectedValue + "'";
        else
            this.wizNotify.Filter = "";
        this.wizNotify.Refresh();
    }
    protected void chkSubmitted_CheckedChanged(object sender, EventArgs e)
    {
        if (this.chkSubmitted.Checked)
        {
            dgvFlowRunOver.Visible = true;
            dgvToDoHis.Visible = false;
            //this.wizToDoHis.SqlMode = FLTools.ESqlMode.FlowRunOver;
            //this.wizToDoHis.Filter = "";
        }
        else
        {
            dgvFlowRunOver.Visible = false;
            dgvToDoHis.Visible = true;
            //this.wizToDoHis.SqlMode = FLTools.ESqlMode.ToDoHis;
            //this.wizToDoHis.Filter = "";
        }
        //this.wizToDoHis.Refresh();
    }
    protected void UpdatePanel1_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.Page.GetType(), "ScriptBlock1", "todolist_onload();", true);
    }
    protected void UpdatePanel2_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, this.Page.GetType(), "ScriptBlock2", "todohis_onload();", true);
    }
    protected void UpdatePanel3_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel3, this.Page.GetType(), "ScriptBlock3", "overtimeActive_onload();", true);

        string[] OvertimeCols = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "OvertimeColumns", true).Split(',');
        this.lblLevel.Text = OvertimeCols[7];
    }
    protected void UpdatePanel5_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel5, this.Page.GetType(), "ScriptBlock5", "notify_onload();", true);
    }

    public bool IgnoreWeekends = true;
    private void GetOvertimeList()
    {
        this.wizOvertime.Refresh(this.ddlLevel.SelectedIndex, this.IgnoreWeekends, null);
    }

    protected void ddlLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetOvertimeList();
    }

    private bool IsOverTime(string TIME_UNIT, string FLOWURGENT, string UPDATE_DATE, string UPDATE_TIME, string URGENT_TIME, string EXP_TIME)
    {
        if (TIME_UNIT == "Day" && FLOWURGENT == "1")
        {
            if (Convert.ToDecimal(URGENT_TIME) == Decimal.Zero) return false;
            TimeSpan span = WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), this.IgnoreWeekends, null);
            int overtimes = span.Days - Convert.ToInt32(Convert.ToDecimal(URGENT_TIME));
            if (overtimes >= 0)
            {
                return true;
            }
        }
        else if (TIME_UNIT == "Day" && FLOWURGENT == "0")
        {
            if (Convert.ToDecimal(EXP_TIME) == Decimal.Zero) return false;
            TimeSpan span = WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), this.IgnoreWeekends, null);
            int overtimes = span.Days - Convert.ToInt32(Convert.ToDecimal(EXP_TIME));
            if (overtimes >= 0)
            {
                return true;
            }
        }
        else if (TIME_UNIT == "Hour" && FLOWURGENT == "1")
        {
            if (Convert.ToDecimal(URGENT_TIME) == Decimal.Zero) return false;
            TimeSpan spanDay = WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), this.IgnoreWeekends, null);
            int spanHour = DateTime.Now.Hour - Convert.ToDateTime(UPDATE_TIME).Hour;
            if (DateTime.Now.Minute < Convert.ToDateTime(UPDATE_TIME).Minute)
            {
                spanHour -= 1;
            }
            int overtimes = spanDay.Days * 8 + spanHour - Convert.ToInt32(Convert.ToDecimal(URGENT_TIME));
            if (overtimes >= 0)
            {
                return true;
            }
        }
        else if (TIME_UNIT == "Hour" && FLOWURGENT == "0")
        {
            if (Convert.ToDecimal(EXP_TIME) == Decimal.Zero) return false;
            TimeSpan spanDay = WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), this.IgnoreWeekends, null);
            int spanHour = DateTime.Now.Hour - Convert.ToDateTime(UPDATE_TIME).Hour;
            if (DateTime.Now.Minute < Convert.ToDateTime(UPDATE_TIME).Minute)
            {
                spanHour -= 1;
            }
            int overtimes = spanDay.Days * 8 + spanHour - Convert.ToInt32(Convert.ToDecimal(EXP_TIME));
            if (overtimes >= 0)
            {
                return true;
            }
        }
        return false;
    }

    private TimeSpan WorkTimeSpan(DateTime nowTime, DateTime updateTime, bool weekendSensible, List<string> extDates)
    {
        TimeSpan span = new TimeSpan();
        if (weekendSensible)
        {
            if (nowTime.DayOfWeek == DayOfWeek.Saturday)
            {
                nowTime = nowTime.Date.AddSeconds(-1);
            }
            else if (nowTime.DayOfWeek == DayOfWeek.Sunday)
            {
                nowTime = nowTime.Date.AddDays(-1).AddSeconds(-1);
            }

            if (updateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                updateTime = updateTime.Date.AddDays(2);
            }
            else if (updateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                updateTime = updateTime.Date.AddDays(1);
            }
        }
        span = nowTime - updateTime;
        if (weekendSensible)
        {
            int weekends = span.Days / 7;
            int i = nowTime.DayOfWeek - updateTime.DayOfWeek;
            if (i < 0)
                weekends++;
            span = span.Subtract(new TimeSpan(2 * weekends, 0, 0, 0));
        }
        int extDays = 0;
        if (extDates == null) return span;
        foreach (string extDate in extDates)
        {
            if (Convert.ToDateTime(extDate).CompareTo(nowTime) < 0
                && Convert.ToDateTime(extDate).CompareTo(updateTime) > 0)
            {
                if (weekendSensible)
                {
                    if (Convert.ToDateTime(extDate).DayOfWeek != DayOfWeek.Saturday
                        && Convert.ToDateTime(extDate).DayOfWeek != DayOfWeek.Sunday)
                    {
                        extDays++;
                    }
                }
                else
                {
                    extDays++;
                }
            }
        }
        span = span.Subtract(new TimeSpan(extDays, 0, 0, 0));
        return span;
    }

    protected void UpdatePanel1_PreRender(object sender, EventArgs e)
    {
        setOvertimeWarning(this.dgvToDoList);
    }
    protected void UpdatePanel2_PreRender(object sender, EventArgs e)
    {
        if(!chkSubmitted.Checked)
        //if (this.wizToDoHis.SqlMode != FLTools.ESqlMode.FlowRunOver)
        {
            setOvertimeWarning(this.dgvToDoHis);
        }
    }
    protected void UpdatePanel5_PreRender(object sender, EventArgs e)
    {
        //setOvertimeWarning(this.dgvToDoList);
    }

    private void setOvertimeWarning(GridView gridView)
    {
        int count = 0;
        if (gridView.ID == "dgvToDoList")
            count = 1;
        foreach (GridViewRow row in gridView.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if (row.Cells.Count >= 43)
                {
                    string TIME_UNIT = row.Cells[13 + count].Text;
                    string FLOWURGENT = row.Cells[23 + count].Text;
                    string UPDATE_WHOLE_TIME = row.Cells[38 + count].Text;
                    string UPDATE_DATE = UPDATE_WHOLE_TIME.Substring(0, UPDATE_WHOLE_TIME.IndexOf(' '));
                    string UPDATE_TIME = UPDATE_WHOLE_TIME.Substring(UPDATE_WHOLE_TIME.IndexOf(' ') + 1);
                    string URGENT_TIME = row.Cells[12 + count].Text;
                    string EXP_TIME = row.Cells[11 + count].Text;

                    if (IsOverTime(TIME_UNIT, FLOWURGENT, UPDATE_DATE, UPDATE_TIME, URGENT_TIME, EXP_TIME))
                    {
                        row.Style.Add("color", "red");
                    }
                }
            }
        }
    }

    public string IsUserInList()
    {
        string Users = this.Request.QueryString["Users"];
        if (Users == null || Users == "")
            return "true";
        string curUser = Session["fLoginUser"].ToString();
        string[] userList = Users.Split(',');
        foreach (string user in userList)
        {
            if (user == curUser)
                return "true";
        }
        return "false";
    }

    public string IsMailCurrent()
    {
        string listid = this.Request.QueryString["LISTID"];
        string flowpath = this.Request.QueryString["FLOWPATH"];
        string status = this.Request.QueryString["STATUS"];
        if (string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(status))
        {
            if (status.Equals("A") || FLTools.GloFix.IsCurrentActivity(flowpath, listid))
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
        
        return null;
    }

    public string UserIdCompareError()
    {
        //string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Web", "webClientMainFlow", "USERNOTFIT");
        return "";
    }
    protected void tmFlow_Tick(object sender, EventArgs e)
    {
        this.wizToDoList.Refresh();
        this.wizToDoHis.Refresh();
    }

    protected void MultiView1_ActiveViewChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel4, this.Page.GetType(), "ScriptBlock4", "setMaxSize();", true);
        if (this.MultiView1.ActiveViewIndex == 1)
        {
            //if (this.chkSubmitted.Checked)
            //{
            //    this.wizToDoHis.SqlMode = FLTools.ESqlMode.FlowRunOver;
            //}
            //else
            //{
            //    this.wizToDoHis.SqlMode = FLTools.ESqlMode.ToDoHis;
            //}
            this.wizToDoHis.Refresh();
        }
        else if (this.MultiView1.ActiveViewIndex == 3)
        {
            GetOvertimeList();
        }
    }
    protected void WebMultiViewCaptions1_Load(object sender, EventArgs e)
    {
        string[] uiText = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Web", "webClientMainFlow", "UIText", true).Split(';');
        this.WebMultiViewCaptions1.Captions[0].Caption = uiText[2];
        this.WebMultiViewCaptions1.Captions[1].Caption = uiText[3];
        this.WebMultiViewCaptions1.Captions[3].Caption = uiText[4];
        this.WebMultiViewCaptions1.Captions[2].Caption = uiText[16];
    }
    protected void lnkOvertimeRefresh_Click(object sender, EventArgs e)
    {
        GetOvertimeList();
    }

    public string GetQueryCaption(int i)
    {
        //0(FLOW_DESC,�����Q);
        //1(D_STEP_ID,��I��Q);
        //2(USERNAME,�ļ��?;
        //3(FORM_PRESENT_CT,??�̖�a);
        //4(REMARK,?Ϣ);
        //5(�����?;
        //6(ڵ����);
        //7(SENDTO_NAME,ڽ�k��);
        //8(STATUS,��r)
        string[] queryConditions = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Web", "webClientMainFlow", "QueryCaption", true).Split(';');
        if (i < queryConditions.Length)
        {
            return queryConditions[i];
        }
        return "";
    }

    const string oracleDateFormat = "'yyyy/MM/dd hh24:mi:ss'";
    const String informixDateFormat = "'%Y-%m-%d %H:%M:%S'";
    protected void lstQueryOK_Click(object sender, EventArgs e)
    {
        String connectMark = "+";
        String DBAlias = CliUtils.fLoginDB;
        object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { DBAlias });
        if (myRet != null && myRet[0].ToString() == "0")
        {
            switch (myRet[1].ToString())
            {
                case "1": connectMark = "+"; break;
                case "2": connectMark = "+"; break;
                case "3": connectMark = "||"; break;
                case "4": connectMark = "||"; break;
                case "5": connectMark = "||"; break;
                case "6": connectMark = "||"; break;
            }
        }

        string filter = "";
        if (!string.IsNullOrEmpty(this.ddlLstQueryFlow.SelectedValue) && this.ddlLstQueryFlow.SelectedIndex != 0)
        {
            filter += "SYS_TODOLIST.FLOW_DESC='" + this.ddlLstQueryFlow.SelectedValue + "' AND ";
        }
        if (!string.IsNullOrEmpty(this.txtLstQueryDStep.Text))
        {
            filter += "SYS_TODOLIST.D_STEP_ID LIKE '" + this.txtLstQueryDStep.Text + "%' AND ";
        }
        if (!string.IsNullOrEmpty(this.ddlLstQueryUser.SelectedValue) && this.ddlLstQueryUser.SelectedIndex != 0)
        {
            filter += "SYS_TODOLIST.USERNAME='" + this.ddlLstQueryUser.SelectedValue + "' AND ";
        }
        if (!string.IsNullOrEmpty(this.txtLstQueryFormPresent.Text))
        {
            filter += "SYS_TODOLIST.FORM_PRESENT_CT LIKE '%" + this.txtLstQueryFormPresent.Text + "%' AND ";
        }
        if (!string.IsNullOrEmpty(this.txtLstQueryDateFrom.Text))
        {
            if (myRet != null && myRet[0].ToString() == "0")
            {
                switch (myRet[1].ToString())
                {
                    case "1":
                        filter += "CAST((SYS_TODOLIST.UPDATE_DATE + ' ' + SYS_TODOLIST.UPDATE_TIME) AS DATETIME) > '" + this.txtLstQueryDateFrom.Text + "' AND ";
                        break;
                    case "2":
                        filter += "CAST((SYS_TODOLIST.UPDATE_DATE + ' ' + SYS_TODOLIST.UPDATE_TIME) AS DATETIME) > '" + this.txtLstQueryDateFrom.Text + "' AND ";
                        break;
                    case "3":
                        filter += "to_date(SYS_TODOLIST.UPDATE_DATE " + connectMark + " ' ' " + connectMark + " SYS_TODOLIST.UPDATE_TIME, " + oracleDateFormat + ") > to_date('" + this.txtLstQueryDateFrom.Text + "', " + oracleDateFormat + ") AND ";
                        break;
                    case "4":
                        filter += string.Format("to_date(SYS_TODOLIST.UPDATE_DATE {0} ' ' {0} SYS_TODOLIST.UPDATE_TIME, {1}) > to_date('{2}', {1})  AND ", connectMark, informixDateFormat, this.txtLstQueryDateFrom.Text);
                        break;
                    case "5":
                        filter += string.Format("CONCAT(SYS_TODOLIST.UPDATE_DATE, ' ', SYS_TODOLIST.UPDATE_TIME) > '{0}'  AND ", this.txtLstQueryDateFrom.Text);
                        break;
                    case "6":
                        filter += string.Format("to_date(SYS_TODOLIST.UPDATE_DATE {0} ' ' {0} SYS_TODOLIST.UPDATE_TIME, {1}) > to_date('{2}', {1})  AND ", connectMark, informixDateFormat, this.txtLstQueryDateFrom.Text + " 00:00:00");
                        break;
                }
            }
        }
        if (!string.IsNullOrEmpty(this.txtLstQueryDateTo.Text))
        {
            if (myRet != null && myRet[0].ToString() == "0")
            {
                switch (myRet[1].ToString())
                {
                    case "1":
                        filter += "CAST((SYS_TODOLIST.UPDATE_DATE + ' ' + SYS_TODOLIST.UPDATE_TIME) AS DATETIME) < '" + this.txtLstQueryDateTo.Text + " 23:59:59' AND ";
                        break;
                    case "2":
                        filter += "CAST((SYS_TODOLIST.UPDATE_DATE + ' ' + SYS_TODOLIST.UPDATE_TIME) AS DATETIME) < '" + this.txtLstQueryDateTo.Text + " 23:59:59' AND ";
                        break;
                    case "3":
                        filter += "to_date(SYS_TODOLIST.UPDATE_DATE " + connectMark + " ' ' " + connectMark + " SYS_TODOLIST.UPDATE_TIME, " + oracleDateFormat + ") < to_date('" + this.txtLstQueryDateTo.Text + " 23:59:59', " + oracleDateFormat + ") AND ";
                        break;
                    case "4":
                        filter += string.Format("to_date(SYS_TODOLIST.UPDATE_DATE {0} ' ' {0} SYS_TODOLIST.UPDATE_TIME, {1}) < to_date('{2}', {1})  AND ", connectMark, informixDateFormat, this.txtLstQueryDateTo.Text + " 23:59:59");
                        break;
                    case "5":
                        filter += "CONCAT(SYS_TODOLIST.UPDATE_DATE, ' ', SYS_TODOLIST.UPDATE_TIME) < '" + this.txtLstQueryDateTo.Text + " 23:59:59' AND ";
                        break;
                    case "6":
                        filter += string.Format("to_date(SYS_TODOLIST.UPDATE_DATE {0} ' ' {0} SYS_TODOLIST.UPDATE_TIME, {1}) < to_date('{2}', {1})  AND ", connectMark, informixDateFormat, this.txtLstQueryDateTo.Text + " 23:59:59");
                        break;
                }
            }
        }
        if (!string.IsNullOrEmpty(this.txtLstQueryRemark.Text))
        {
            filter += "SYS_TODOLIST.REMARK LIKE '%" + this.txtLstQueryRemark.Text + "%' AND ";
        }

        if (!string.IsNullOrEmpty(filter))
        {
            filter = filter.Substring(0, filter.LastIndexOf(" AND "));
        }

        this.wizToDoList.Refresh("", filter);
    }

    protected void hisQueryOK_Click(object sender, EventArgs e)
    {
        String connectMark = "+";
        String DBAlias = CliUtils.fLoginDB;
        object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { DBAlias });
        if (myRet != null && myRet[0].ToString() == "0")
        {
            switch (myRet[1].ToString())
            {
                case "1": connectMark = "+"; break;
                case "2": connectMark = "+"; break;
                case "3": connectMark = "||"; break;
                case "4": connectMark = "||"; break;
                case "5": connectMark = "||"; break;
                case "6": connectMark = "||"; break;
            }
        }

        string filter = "";
        if (!string.IsNullOrEmpty(this.ddlHisQueryFlow.SelectedValue) && this.ddlHisQueryFlow.SelectedIndex != 0)
        {
            filter += "SYS_TODOLIST.FLOW_DESC='" + this.ddlHisQueryFlow.SelectedValue + "' AND ";
        }
        if (!string.IsNullOrEmpty(this.txtHisQueryDStep.Text))
        {
            filter += "SYS_TODOLIST.D_STEP_ID LIKE '" + this.txtHisQueryDStep.Text + "%' AND ";
        }
        if (!string.IsNullOrEmpty(this.ddlHisQuerySendTo.SelectedValue) && this.ddlHisQuerySendTo.SelectedIndex != 0)
        {
            filter += "SYS_TODOLIST.SENDTO_NAME LIKE '%" + this.ddlHisQuerySendTo.SelectedValue + "%' AND ";
        }
        //if (!string.IsNullOrEmpty(this.ddlHisQueryStatus.SelectedValue) && this.ddlHisQueryStatus.SelectedIndex != 0)
        //{
        //    filter += "SYS_TODOLIST.STATUS='" + this.ddlHisQueryStatus.SelectedValue + "' AND ";
        //}
        if (!string.IsNullOrEmpty(this.txtHisQueryFormPresent.Text))
        {
            filter += "SYS_TODOLIST.FORM_PRESENT_CT LIKE '%" + this.txtHisQueryFormPresent.Text + "%' AND ";
        }
        if (!string.IsNullOrEmpty(this.txtHisQueryDateFrom.Text))
        {
            if (myRet != null && myRet[0].ToString() == "0")
            {
                switch (myRet[1].ToString())
                {
                    case "1":
                        filter += "CAST((SYS_TODOLIST.UPDATE_DATE + ' ' + SYS_TODOLIST.UPDATE_TIME) AS DATETIME) > '" + this.txtHisQueryDateFrom.Text + "' AND ";
                        break;
                    case "2":
                        filter += "CAST((SYS_TODOLIST.UPDATE_DATE + ' ' + SYS_TODOLIST.UPDATE_TIME) AS DATETIME) > '" + this.txtHisQueryDateFrom.Text + "' AND ";
                        break;
                    case "3":
                        filter += "to_date(SYS_TODOLIST.UPDATE_DATE " + connectMark + " ' ' " + connectMark + " SYS_TODOLIST.UPDATE_TIME, " + oracleDateFormat + ") > to_date('" + this.txtHisQueryDateFrom.Text + "', " + oracleDateFormat + ") AND ";
                        break;
                    case "4":
                        filter += string.Format("to_date(SYS_TODOLIST.UPDATE_DATE {0} ' ' {0} SYS_TODOLIST.UPDATE_TIME, {1}) > to_date('{2}', {1})  AND ", connectMark, informixDateFormat, this.txtHisQueryDateFrom.Text);
                        break;
                    case "5":
                        filter += "CONCAT(SYS_TODOLIST.UPDATE_DATE, ' ', SYS_TODOLIST.UPDATE_TIME) > '" + this.txtHisQueryDateFrom.Text + "' AND ";
                        break;
                    case "6":
                        filter += string.Format("to_date(SYS_TODOLIST.UPDATE_DATE {0} ' ' {0} SYS_TODOLIST.UPDATE_TIME, {1}) > to_date('{2}', {1})  AND ", connectMark, informixDateFormat, this.txtHisQueryDateFrom.Text + " 00:00:00");
                        break;
                }
            }
        }
        if (!string.IsNullOrEmpty(this.txtHisQueryDateTo.Text))
        {
            if (myRet != null && myRet[0].ToString() == "0")
            {
                switch (myRet[1].ToString())
                {
                    case "1":
                        filter += "CAST((SYS_TODOLIST.UPDATE_DATE + ' ' + SYS_TODOLIST.UPDATE_TIME) AS DATETIME) < '" + this.txtHisQueryDateTo.Text + " 23:59:59' AND ";
                        break;
                    case "2":
                        filter += "CAST((SYS_TODOLIST.UPDATE_DATE + ' ' + SYS_TODOLIST.UPDATE_TIME) AS DATETIME) < '" + this.txtHisQueryDateTo.Text + " 23:59:59' AND ";
                        break;
                    case "3":
                        filter += "to_date(SYS_TODOLIST.UPDATE_DATE " + connectMark + " ' ' " + connectMark + " SYS_TODOLIST.UPDATE_TIME, " + oracleDateFormat + ") < to_date('" + this.txtHisQueryDateTo.Text + " 23:59:59', " + oracleDateFormat + ") AND ";
                        break;
                    case "4":
                        filter += string.Format("to_date(SYS_TODOLIST.UPDATE_DATE {0} ' ' {0} SYS_TODOLIST.UPDATE_TIME, {1}) < to_date('{2}', {1})  AND ", connectMark, informixDateFormat, this.txtHisQueryDateTo.Text + " 23:59:59");
                        break;
                    case "5":
                        filter += "CONCAT(SYS_TODOLIST.UPDATE_DATE, ' ', SYS_TODOLIST.UPDATE_TIME) < '" + this.txtHisQueryDateTo.Text + " 23:59:59' AND ";
                        break;
                    case "6":
                        filter += string.Format("to_date(SYS_TODOLIST.UPDATE_DATE {0} ' ' {0} SYS_TODOLIST.UPDATE_TIME, {1}) < to_date('{2}', {1})  AND ", connectMark, informixDateFormat, this.txtHisQueryDateTo.Text + " 23:59:59");
                        break;
                }
            }
        }
        if (!string.IsNullOrEmpty(this.txtHisQueryRemark.Text))
        {
            filter += "SYS_TODOLIST.REMARK LIKE '%" + this.txtHisQueryRemark.Text + "%' AND ";
        }

        if (!string.IsNullOrEmpty(filter))
        {
            filter = filter.Substring(0, filter.LastIndexOf(" AND "));
        }

        this.wizToDoHis.Refresh("", filter);
    }

    protected void hisQuery2OK_Click(object sender, EventArgs e)
    {
        String connectMark = "+";
        String DBAlias = CliUtils.fLoginDB;
        object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { DBAlias });
        if (myRet != null && myRet[0].ToString() == "0")
        {
            switch (myRet[1].ToString())
            {
                case "1": connectMark = "+"; break;
                case "2": connectMark = "+"; break;
                case "3": connectMark = "||"; break;
                case "4": connectMark = "||"; break;
                case "5": connectMark = "||"; break;
                case "6": connectMark = "||"; break;
            }
        }

        string filter = "";
        if (!string.IsNullOrEmpty(this.ddlHisQuery2Flow.SelectedValue) && this.ddlHisQuery2Flow.SelectedIndex != 0)
        {
            filter += "SYS_TODOHIS.FLOW_DESC='" + this.ddlHisQuery2Flow.SelectedValue + "' AND ";
        }
        if (!string.IsNullOrEmpty(this.txtHisQuery2DStep.Text))
        {
            filter += "SYS_TODOHIS.D_STEP_ID LIKE '" + this.txtHisQuery2DStep.Text + "%' AND ";
        }
        if (!string.IsNullOrEmpty(this.txtHisQuery2FormPresent.Text))
        {
            filter += "SYS_TODOHIS.FORM_PRESENT_CT LIKE '%" + this.txtHisQuery2FormPresent.Text + "%' AND ";
        }
        if (!string.IsNullOrEmpty(this.txtHisQuery2DateFrom.Text))
        {
            if (myRet != null && myRet[0].ToString() == "0")
            {
                switch (myRet[1].ToString())
                {
                    case "1":
                        filter += "CAST((SYS_TODOHIS.UPDATE_DATE + ' ' + SYS_TODOHIS.UPDATE_TIME) AS DATETIME) > '" + this.txtHisQuery2DateFrom.Text + "' AND ";
                        break;
                    case "2":
                        filter += "CAST((SYS_TODOHIS.UPDATE_DATE + ' ' + SYS_TODOHIS.UPDATE_TIME) AS DATETIME) > '" + this.txtHisQuery2DateFrom.Text + "' AND ";
                        break;
                    case "3":
                        filter += "to_date(SYS_TODOHIS.UPDATE_DATE " + connectMark + " ' ' " + connectMark + " SYS_TODOHIS.UPDATE_TIME, " + oracleDateFormat + ") > to_date('" + this.txtHisQuery2DateFrom.Text + "', " + oracleDateFormat + ") AND ";
                        break;
                    case "4":
                        filter += string.Format("to_date(SYS_TODOHIS.UPDATE_DATE {0} ' ' {0} SYS_TODOHIS.UPDATE_TIME, {1}) > to_date('{2}', {1})  AND ", connectMark, informixDateFormat, this.txtHisQuery2DateFrom.Text);
                        break;
                    case "5":
                        filter += "CONCAT(SYS_TODOHIS.UPDATE_DATE, ' ', SYS_TODOHIS.UPDATE_TIME) > '" + this.txtHisQuery2DateFrom.Text + "' AND ";
                        break;
                    case "6":
                        filter += string.Format("to_date(SYS_TODOHIS.UPDATE_DATE {0} ' ' {0} SYS_TODOHIS.UPDATE_TIME, {1}) > to_date('{2}', {1})  AND ", connectMark, informixDateFormat, this.txtHisQuery2DateFrom.Text + " 00:00:00");
                        break;
                }
            }
        }
        if (!string.IsNullOrEmpty(this.txtHisQuery2DateTo.Text))
        {
            if (myRet != null && myRet[0].ToString() == "0")
            {
                switch (myRet[1].ToString())
                {
                    case "1":
                        filter += "CAST((SYS_TODOHIS.UPDATE_DATE + ' ' + SYS_TODOHIS.UPDATE_TIME) AS DATETIME) < '" + this.txtHisQuery2DateTo.Text + " 23:59:59' AND ";
                        break;
                    case "2":
                        filter += "CAST((SYS_TODOHIS.UPDATE_DATE + ' ' + SYS_TODOHIS.UPDATE_TIME) AS DATETIME) < '" + this.txtHisQuery2DateTo.Text + " 23:59:59' AND ";
                        break;
                    case "3":
                        filter += "to_date(SYS_TODOHIS.UPDATE_DATE " + connectMark + " ' ' " + connectMark + " SYS_TODOHIS.UPDATE_TIME, " + oracleDateFormat + ") < to_date('" + this.txtHisQuery2DateTo.Text + " 23:59:59', " + oracleDateFormat + ") AND ";
                        break;
                    case "4":
                        filter += string.Format("to_date(SYS_TODOHIS.UPDATE_DATE {0} ' ' {0} SYS_TODOHIS.UPDATE_TIME, {1}) < to_date('{2}', {1})  AND ", connectMark, informixDateFormat, this.txtHisQuery2DateTo.Text + " 23:59:59");
                        break;
                    case "5":
                        filter += "CONCAT(SYS_TODOHIS.UPDATE_DATE, ' ', SYS_TODOHIS.UPDATE_TIME) < '" + this.txtHisQuery2DateTo.Text + " 23:59:59' AND ";
                        break;
                    case "6":
                        filter += string.Format("to_date(SYS_TODOHIS.UPDATE_DATE {0} ' ' {0} SYS_TODOHIS.UPDATE_TIME, {1}) < to_date('{2}', {1})  AND ", connectMark, informixDateFormat, this.txtHisQuery2DateTo.Text + " 23:59:59");
                        break;
                }
            }
        }
        if (!string.IsNullOrEmpty(this.txtHisQuery2Remark.Text))
        {
            filter += "SYS_TODOHIS.REMARK LIKE '%" + this.txtHisQuery2Remark.Text + "%' AND ";
        }

        if (!string.IsNullOrEmpty(filter))
        {
            filter = filter.Substring(0, filter.LastIndexOf(" AND "));
        }

        this.wizFlowRunOver.Refresh("", filter);
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
        if (nodeText.StartsWith(text))
            return true;
        else
            return false;
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


    public SortDirection ListSortDirection
    {
        get
        {
            if (ViewState["ListSortDirection"] == null)
                ViewState["ListSortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["ListSortDirection"];
        }
        set { ViewState["ListSortDirection"] = value; }
    }

    public SortDirection HisSortDirection
    {
        get
        {
            if (ViewState["HisSortDirection"] == null)
                ViewState["HisSortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["HisSortDirection"];
        }
        set { ViewState["HisSortDirection"] = value; }
    }

    public SortDirection NotifySortDirection
    {
        get
        {
            if (ViewState["NotifySortDirection"] == null)
                ViewState["NotifySortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["NotifySortDirection"];
        }
        set { ViewState["NotifySortDirection"] = value; }
    }

    public SortDirection DelaySortDirection
    {
        get
        {
            if (ViewState["DelaySortDirection"] == null)
                ViewState["DelaySortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["DelaySortDirection"];
        }
        set { ViewState["DelaySortDirection"] = value; }
    }

    protected void dgvToDoList_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (ListSortDirection == SortDirection.Ascending)
        {
            SortGridView(FLTools.ESqlMode.ToDoList, sortExpression, " DESC");
            ListSortDirection = SortDirection.Descending;
        }
        else
        {
            SortGridView(FLTools.ESqlMode.ToDoList, sortExpression, " ASC");
            ListSortDirection = SortDirection.Ascending;
        }
    }

    protected void dgvToDoHis_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (HisSortDirection == SortDirection.Ascending)
        {
            SortGridView(FLTools.ESqlMode.ToDoHis, sortExpression, " DESC");
            HisSortDirection = SortDirection.Descending;
        }
        else
        {
            SortGridView(FLTools.ESqlMode.ToDoHis, sortExpression, " ASC");
            HisSortDirection = SortDirection.Ascending;
        }
    }

    protected void dgvFlowRunOver_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (HisSortDirection == SortDirection.Ascending)
        {
            SortGridView(FLTools.ESqlMode.FlowRunOver, sortExpression, " DESC");
            HisSortDirection = SortDirection.Descending;
        }
        else
        {
            SortGridView(FLTools.ESqlMode.FlowRunOver, sortExpression, " ASC");
            HisSortDirection = SortDirection.Ascending;
        }
    }

    protected void dgvNotify_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (ListSortDirection == SortDirection.Ascending)
        {
            SortGridView(FLTools.ESqlMode.Notify, sortExpression, " DESC");
            ListSortDirection = SortDirection.Descending;
        }
        else
        {
            SortGridView(FLTools.ESqlMode.Notify, sortExpression, " ASC");
            ListSortDirection = SortDirection.Ascending;
        }
    }

    protected void dgvOvertime_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (DelaySortDirection == SortDirection.Ascending)
        {
            SortGridView(FLTools.ESqlMode.Delay, sortExpression, " DESC");
            DelaySortDirection = SortDirection.Descending;
        }
        else
        {
            SortGridView(FLTools.ESqlMode.Delay, sortExpression, " ASC");
            DelaySortDirection = SortDirection.Ascending;
        }
    }

    private void SortGridView(FLTools.ESqlMode sqlMode, string sortExpression, string direction)
    {
        switch (sqlMode)
        {
            case FLTools.ESqlMode.ToDoList:
                this.wizToDoList.Refresh(sortExpression + direction, "");
                break;
            case FLTools.ESqlMode.ToDoHis:
                this.wizToDoHis.Refresh(sortExpression + direction, "");
                break;
            case FLTools.ESqlMode.FlowRunOver:
                this.wizFlowRunOver.Refresh(sortExpression + direction, "");
                break;
            case FLTools.ESqlMode.Notify:
                this.wizNotify.Refresh(sortExpression + direction, "");
                break;
            case FLTools.ESqlMode.Delay:
                this.wizOvertime.Refresh(sortExpression + direction, "");
                break;
        }
    }
    protected void dgvToDoList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Control ctrl = e.Row.FindControl("openA");
            if (ctrl != null && ctrl is HyperLink)
            {
                HyperLink link = ctrl as HyperLink;
                link.ToolTip = this.GetToolTip(9);
            }

            formatStatus(e, 25);
        }
    }
    protected void dgvToDoHis_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Control ctrlSelect = e.Row.FindControl("openA");
            if (ctrlSelect != null && ctrlSelect is HyperLink)
            {
                HyperLink link = ctrlSelect as HyperLink;
                link.ToolTip = this.GetToolTip(9);
            }
            Control ctrlReturn = e.Row.FindControl("returnImg");
            if (ctrlReturn != null && ctrlReturn is ImageButton)
            {
                ImageButton btn = ctrlReturn as ImageButton;
                btn.ToolTip = this.GetToolTip(14);
            }
            Control ctrlNotify = e.Row.FindControl("notifyImg");
            if (ctrlNotify != null && ctrlNotify is ImageButton)
            {
                ImageButton btn = ctrlNotify as ImageButton;
                btn.ToolTip = this.GetToolTip(18);
            }
            formatStatus(e, 24);
        }
    }
    protected void dgvNotify_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Control ctrlSelect = e.Row.FindControl("openA");
            if (ctrlSelect != null && ctrlSelect is HyperLink)
            {
                HyperLink link = ctrlSelect as HyperLink;
                link.ToolTip = this.GetToolTip(9);
            }
            Control ctrlDelete = e.Row.FindControl("deleteImg");
            if (ctrlDelete != null && ctrlDelete is ImageButton)
            {
                ImageButton btn = ctrlDelete as ImageButton;
                btn.ToolTip = this.GetToolTip(15);
            }
            formatStatus(e, 24);
        }
    }
    protected void dgvOvertime_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Control ctrlSelect = e.Row.FindControl("openA");
            if (ctrlSelect != null && ctrlSelect is HyperLink)
            {
                HyperLink link = ctrlSelect as HyperLink;
                link.ToolTip = this.GetToolTip(9);
            }
        }
    }

    void formatStatus(GridViewRowEventArgs e, int index)
    {
        string[] lstStatus = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLDesigner", "FLDesigner", "Item3", true).Split(',');
        if (e.Row.Cells.Count >= index - 1)
        {
            switch (e.Row.Cells[index].Text)
            {
                case "Z":
                    e.Row.Cells[index].Text = lstStatus[0];
                    break;
                case "N":
                    e.Row.Cells[index].Text = lstStatus[1];
                    break;
                case "NR":
                    e.Row.Cells[index].Text = lstStatus[2];
                    break;
                case "NF":
                    e.Row.Cells[index].Text = lstStatus[3];
                    break;
                case "X":
                    e.Row.Cells[index].Text = lstStatus[4];
                    break;
                case "A":
                    e.Row.Cells[index].Text = lstStatus[5];
                    break;
                case "V":
                    e.Row.Cells[index].Text = lstStatus[6];
                    break;
            }
        }
    }
}