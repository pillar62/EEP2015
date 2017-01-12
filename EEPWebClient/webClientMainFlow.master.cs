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
using System.Collections.Generic;

public partial class webClientMainFlowMaster : System.Web.UI.MasterPage
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

            string selOption = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "SelectOption", true);

            DataTable list = CliUtils.ExecuteSql("GLModule", "cmdToDoList", "SELECT DISTINCT SYS_TODOLIST.FLOW_DESC FROM SYS_TODOLIST", true, CliUtils.fCurrentProject).Tables[0];
            this.ddlToDoListFilter.DataSource = list;
            this.ddlToDoListFilter.DataValueField = "FLOW_DESC";
            this.ddlToDoListFilter.DataTextField = "FLOW_DESC";
            this.ddlToDoListFilter.DataBind();
            this.ddlToDoListFilter.Items.Insert(0, "<--" + selOption + "-->");

            DataTable his = CliUtils.ExecuteSql("GLModule", "cmdToDoList", "SELECT DISTINCT SYS_TODOLIST.FLOW_DESC FROM SYS_TODOLIST", true, CliUtils.fCurrentProject).Tables[0];
            this.ddlToDoHisFilter.DataSource = his;
            this.ddlToDoHisFilter.DataValueField = "FLOW_DESC";
            this.ddlToDoHisFilter.DataTextField = "FLOW_DESC";
            this.ddlToDoHisFilter.DataBind();
            this.ddlToDoHisFilter.Items.Insert(0, "<--" + selOption + "-->");
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

    private void initializeMenu(ArrayList menuIDList, ArrayList menuCaptionList, ArrayList menuParentIDList, ArrayList imageList, ArrayList packageList, ArrayList formList)
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
            //if (ListMainImage[j] != null && ListMainImage[j].ToString() != "")
            //    nodeMain[j].ImageUrl = defaultDir + ListMainImage[j].ToString();
            //else
            //    nodeMain[j].ImageUrl = "~/Image/main/b4.gif";
            nodeMain[j].Selectable = false;
        }
        int p = ListChildrenID.Count;
        MenuItem[] nodeChildren = new MenuItem[p];
        for (int q = 0; q < p; q++)
        {
            nodeChildren[q] = new MenuItem();
            nodeChildren[q].Value = ListChildrenID[q].ToString();
            nodeChildren[q].Text = ListChildrenCaption[q].ToString();
            //if (ListChildrenImage[q] != null && ListChildrenImage[q].ToString() != "")
            //    nodeChildren[q].ImageUrl = defaultDir + ListChildrenImage[q].ToString();
            //else
            //    nodeChildren[q].ImageUrl = "~/Image/main/b6.gif";
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
            for (int x = 0; x < ListMainID.Count; x++)
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

    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    if(this.Request.FilePath == "/EEPWebClient/DefaultPage3.aspx")
    //        this.Response.Redirect("MasterPage/Single.aspx");
    //    else
    //        this.Response.Redirect("Single.aspx");
    //}

    public bool ConvertStringToBoolean(object i)
    {
        return Convert.ToBoolean(Convert.ToInt16(i.ToString()));
    }

    public bool ReturnVisible()
    {
        return (this.wizToDoHis.SqlMode != FLTools.ESqlMode.FlowRunOver);
    }

    public string GetImgUrl(string relUrl)
    {
        string url = this.Page.Request.Url.ToString();
        return url.Substring(0, url.IndexOf("EEPWebClient/") + 13) + relUrl;
    }

    protected void UpdatePanel1_Load(object sender, EventArgs e)
    {
        object obj = this.FindControl("UpdatePanel1");
        if (obj != null && obj is UpdatePanel)
        {
            UpdatePanel panel = (UpdatePanel)obj;
            ScriptManager.RegisterStartupScript(panel, this.Page.GetType(), "ScriptBlock1", "todolist_onload();", true);
        }
    }
    protected void UpdatePanel2_Load(object sender, EventArgs e)
    {
        object obj = this.FindControl("UpdatePanel2");
        if (obj != null && obj is UpdatePanel)
        {
            UpdatePanel panel = (UpdatePanel)obj;
            ScriptManager.RegisterStartupScript(panel, this.Page.GetType(), "ScriptBlock2", "todohis_onload();", true);
        }
    }
    protected void UpdatePanel3_Load(object sender, EventArgs e)
    {
        object obj = this.FindControl("UpdatePanel3");
        if (obj != null && obj is UpdatePanel)
        {
            UpdatePanel panel = (UpdatePanel)obj;
            ScriptManager.RegisterStartupScript(panel, this.Page.GetType(), "ScriptBlock3", "overtimeActive_onload();", true);
        }
        string[] OvertimeCols = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "OvertimeColumns", true).Split(',');
        this.lblLevel.Text = OvertimeCols[7];
        this.chkOvertimeActive.Text = OvertimeCols[8];
    }

    protected void UpdatePanel1_PreRender(object sender, EventArgs e)
    {
        setOvertimeWarning(this.dgvToDoList);
    }

    protected void UpdatePanel2_PreRender(object sender, EventArgs e)
    {
        setOvertimeWarning(this.dgvToDoHis);
    }

    protected void dgvToDoHis_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Return")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            TableCellCollection cells = dgvToDoHis.Rows[rowIndex].Cells;
            int cellLength = cells.Count;
            string listId = cells[cellLength - 42].Text;
            string stepid = cells[cellLength - 35].Text;
            string sql = "SELECT * FROM SYS_TODOLIST WHERE LISTID='" + listId + "' AND D_STEP_ID='" + stepid + "'";
            DataSet ds = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sql, true, CliUtils.fCurrentProject);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                string currentFLActivityName = row["S_STEP_ID"].ToString();
                string keys = row["FORM_KEYS"].ToString();
                string keyvalues = row["FORM_PRESENTATION"].ToString();
                keyvalues = keyvalues.Replace("'", "''");
                object[] objParams = CliUtils.CallFLMethod("Retake", new object[] { new Guid(listId), new object[] { currentFLActivityName, "" }, new object[] { keys, keyvalues } });
                object obj = this.FindControl("UpdatePanel2");
                if (obj != null && obj is UpdatePanel)
                {
                    UpdatePanel panel = (UpdatePanel)obj;
                    if (Convert.ToInt16(objParams[0]) == 0)
                    {
                        this.wizToDoHis.Refresh();
                        ScriptManager.RegisterStartupScript(panel, this.Page.GetType(), "ScriptBlock4", "alert('" + SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLWizard", "RetakeSucess", true) + "')", true);
                    }
                    else if (Convert.ToInt16(objParams[0]) == 2)
                    {
                        ScriptManager.RegisterStartupScript(panel, this.Page.GetType(), "ScriptBlock5", "alert('" + objParams[1].ToString() + "')", true);
                    }
                }
            }
        }
    }

    protected void lnkRefresh_Click(object sender, EventArgs e)
    {
        this.wizToDoList.Refresh();
    }

    protected void lnkHisRefresh_Click(object sender, EventArgs e)
    {
        this.wizToDoHis.Refresh();
    }

    protected void tmFlow_Tick(object sender, EventArgs e)
    {
        this.wizToDoList.Refresh();
        this.wizToDoHis.Refresh();
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
        if (ddlToDoHisFilter.SelectedIndex != 0)
            this.wizToDoHis.Filter = "SYS_TODOLIST.FLOW_DESC='" + ddlToDoHisFilter.SelectedValue + "'";
        else
            this.wizToDoHis.Filter = "";
        this.wizToDoHis.Refresh();
    }
    protected void chkSubmitted_CheckedChanged(object sender, EventArgs e)
    {
        if (this.chkSubmitted.Checked)
        {
            this.wizToDoHis.SqlMode = FLTools.ESqlMode.FlowRunOver;
        }
        else
        {
            this.wizToDoHis.SqlMode = FLTools.ESqlMode.ToDoHis;
        }
        this.wizToDoHis.Refresh();
    }
    protected void chkOvertimeActive_CheckedChanged(object sender, EventArgs e)
    {
        GetOvertimeList();
    }
    protected void ddlLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.chkOvertimeActive.Checked)
        {
            object[] param = new object[2] { CliUtils.fLoginUser, this.ddlLevel.SelectedIndex };
            object[] obj = CliUtils.CallMethod("GLModule", "FLOvertimeList", param);
            if (Convert.ToInt16(obj[0]) == 0)
            {
                DataTable tab = obj[1] as DataTable;
                this.dgvOvertime.DataSource = tab;
            }
        }
        else
        {
            this.dgvOvertime.DataSource = null;
        }
        this.dgvOvertime.DataBind();
        SetOvertimeGridHeaderText();
    }

    public bool IgnoreWeekends = true;
    private void GetOvertimeList()
    {
        if (this.chkOvertimeActive.Checked)
        {
            object[] param = new object[4] { CliUtils.fLoginUser, this.ddlLevel.SelectedIndex, this.IgnoreWeekends, null };
            object[] obj = CliUtils.CallMethod("GLModule", "FLOvertimeList", param);
            if (Convert.ToInt16(obj[0]) == 0)
            {
                DataTable tab = obj[1] as DataTable;
                this.dgvOvertime.DataSource = tab;
            }
        }
        else
        {
            this.dgvOvertime.DataSource = null;
        }
        this.dgvOvertime.DataBind();
        SetOvertimeGridHeaderText();
    }

    private void SetOvertimeGridHeaderText()
    {
        if (this.dgvOvertime.HeaderRow == null) return;
        string[] OvertimeCols = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "OvertimeColumns", true).Split(',');
        foreach (TableCell cell in this.dgvOvertime.HeaderRow.Cells)
        {
            cell.Wrap = false;
            if (cell.Text == "FLOW_DESC")
            {
                cell.Text = OvertimeCols[0];
            }
            else if (cell.Text == "D_STEP_ID")
            {
                cell.Text = OvertimeCols[1];
            }
            else if (cell.Text == "FORM_PRESENT_CT")
            {
                cell.Text = OvertimeCols[2];
            }
            else if (cell.Text == "REMARK")
            {
                cell.Text = OvertimeCols[4];
            }
            else if (cell.Text == "SENDTO_DETAIL")
            {
                cell.Text = OvertimeCols[3];
            }
            else if (cell.Text == "UPDATE_WHOLE_TIME")
            {
                cell.Text = OvertimeCols[5];
            }
            else if (cell.Text == "OVERTIME")
            {
                cell.Text = OvertimeCols[6];
            }
        }
    }

    private void setOvertimeWarning(GridView gridView)
    {
        foreach (GridViewRow row in gridView.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if (row.Cells.Count >= 43)
                {
                    string TIME_UNIT = row.Cells[13].Text;
                    string FLOWURGENT = row.Cells[23].Text;
                    string UPDATE_WHOLE_TIME = row.Cells[38].Text;
                    string UPDATE_DATE = UPDATE_WHOLE_TIME.Substring(0, UPDATE_WHOLE_TIME.IndexOf(' '));
                    string UPDATE_TIME = UPDATE_WHOLE_TIME.Substring(UPDATE_WHOLE_TIME.IndexOf(' ') + 1);
                    string URGENT_TIME = row.Cells[12].Text;
                    string EXP_TIME = row.Cells[11].Text;

                    if (IsOverTime(TIME_UNIT, FLOWURGENT, UPDATE_DATE, UPDATE_TIME, URGENT_TIME, EXP_TIME))
                    {
                        row.Style.Add("color", "red");
                    }
                }
            }
        }
    }

    private bool IsOverTime(string TIME_UNIT, string FLOWURGENT, string UPDATE_DATE, string UPDATE_TIME, string URGENT_TIME, string EXP_TIME)
    {
        if (TIME_UNIT == "Day" && FLOWURGENT == "1")
        {
            TimeSpan span = WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), this.IgnoreWeekends, null);
            int overtimes = span.Days - Convert.ToInt32(Convert.ToDecimal(URGENT_TIME));
            if (overtimes >= 0)
            {
                return true;
            }
        }
        else if (TIME_UNIT == "Day" && FLOWURGENT == "0")
        {
            TimeSpan span = WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), this.IgnoreWeekends, null);
            int overtimes = span.Days - Convert.ToInt32(Convert.ToDecimal(EXP_TIME));
            if (overtimes >= 0)
            {
                return true;
            }
        }
        else if (TIME_UNIT == "Hour" && FLOWURGENT == "1")
        {
            TimeSpan spanDay = WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), this.IgnoreWeekends, null);
            int spanHour = DateTime.Now.Hour - Convert.ToDateTime(UPDATE_TIME).Hour;
            int overtimes = spanDay.Days * 8 + spanHour - Convert.ToInt32(Convert.ToDecimal(URGENT_TIME));
            if (overtimes >= 0)
            {
                return true;
            }
        }
        else if (TIME_UNIT == "Hour" && FLOWURGENT == "0")
        {
            TimeSpan spanDay = WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), this.IgnoreWeekends, null);
            int spanHour = DateTime.Now.Hour - Convert.ToDateTime(UPDATE_TIME).Hour;
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
}
