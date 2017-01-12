using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Xml;
using Srvtools;
using System.Data;
using System.IO;

public partial class InnerPages_FLSubWorkflow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            alllist = new List<List<string[]>>();
            SetUserAndRoles();
            GetXomlList();
            string[] Type = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLRuntime", "SubFl", "ActivityType", true).Split(';');
            activityString = Type[0];
            ddlType.Items.Clear();
            ddlType.Items.Add(Type[1]);
            ddlType.Items.Add(Type[2]);
            if (!string.IsNullOrEmpty(this.Request["ControlID"]))
            {
                this.ControlID = this.Request["ControlID"].ToString();
            }
            if (!string.IsNullOrEmpty(this.Request["fileName"]) && ddlFiles.Items.FindByValue(this.Request["fileName"].ToString()) != null)
            {
                ddlFiles.SelectedValue = this.Request["fileName"].ToString();
                btLoad_Click(this, new EventArgs());
            }
        }
        string[] UITexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLRuntime", "SubFl", "UIText", true).Split(';');
        renderPanel(true);
        btAdd.Text = UITexts[0];
        btAddSrow.Text = UITexts[1];
        btDelete.Text = UITexts[2];
        btUp.Text = UITexts[3];
        btDown.Text = UITexts[4];
        btSave.Text = UITexts[5];
        btLoad.Text = UITexts[6];
        lbSFilename.Text = UITexts[7];
        lbLFileName.Text = UITexts[8];
        btClose.Text = UITexts[9];

        string[] Property = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLRuntime", "SubFl", "Property", true).Split(';');
        lbName.Text = Property[0];
        lbPlusA.Text = Property[1];
        lbRole.Text = Property[2];
        lbUser.Text = Property[3];
        lbType.Text = Property[4];

    }

    private string ControlID
    {
        get
        {
            object obj = this.ViewState["ControlID"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["ControlID"] = value;
        }
    }

    private string activityString
    {
        get
        {
            object obj = this.ViewState["activityString"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["activityString"] = value;
        }

    }

    private int index
    {
        get
        {
            object obj = this.ViewState["index"];
            if (obj != null)
            {
                return (int)obj;
            }
            return 1;
        }
        set
        {
            this.ViewState["index"] = value;
        }
    }

    private string selectedName
    {
        get
        {
            object obj = this.ViewState["selectedName"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["selectedName"] = value;
        }
    }

    private string selectedFileName
    {
        get
        {
            object obj = this.ViewState["selectedFileName"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["selectedFileName"] = value;
        }
    }

    //0:id,1:name,2:selected or not,3:plusApprove,4:role,5:user,6:type
    private List<List<string[]>> alllist
    {
        get
        {

            object obj = this.ViewState["alllist"];
            if (obj != null)
            {
                return (List<List<string[]>>)obj;
            }
            return null;
        }
        set { this.ViewState["alllist"] = value; }
    }

    /// <summary>
    /// add a new stand
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btAdd_Click(object sender, EventArgs e)
    {
        //string s = "node" + index.ToString();
        string id = Guid.NewGuid().ToString();
        string name = activityString + index.ToString();
        //0:id,1:name,2:selected or not,3:plusApprove,4:role,5:user,6:type
        string[] ss = new string[] { id, name, "0"/*是否选中，0表示false,1表示true*/, "False", "", "", "FLStand" };
        List<string[]> stringlist = new List<string[]>();
        stringlist.Add(ss);
        string[] newliststring = new string[8];
        int stringindex = GetSelectedItemindex("", ref newliststring);
        if (stringindex == -1 || stringindex == alllist.Count - 1)
            alllist.Add(stringlist);
        else alllist.Insert(stringindex+1, stringlist);
        index++;
        renderPanel(false);
    }

    /// <summary>
    /// add a stand at the selected row
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btAddSrow_Click(object sender, EventArgs e)
    {
        string[] newliststring = new string[8];
        int stringindex = GetSelectedItemindex(selectedName, ref newliststring);
        if (stringindex == -1 )
            stringindex = alllist.Count-1;
        List<string[]> oo = alllist[stringindex];
        if (oo.Count > 0)
        {
            string id = Guid.NewGuid().ToString();
            string[] sfirst = oo[0];
            oo.Add(new string[] { id, sfirst[1], "0", sfirst[3], sfirst[4], sfirst[5], sfirst[6] });
        }
        renderPanel(false);
    }

    /// <summary>
    /// render whole panel
    /// </summary>
    /// <param name="isLoad"></param>
    private void renderPanel(bool isLoad)
    {
        if (isLoad)
        {
            Change();
        }
        Panel2.Controls.Clear();

        List<List<string[]>> o = alllist;
        foreach (List<string[]> oo in o)
        {
            if (Panel2.Controls.Count > 0)
            {
                Table t2 = new Table();
                t2.HorizontalAlign = HorizontalAlign.Center;
                //t2.ViewStateMode = System.Web.UI.ViewStateMode.Disabled;
                Panel2.Controls.Add(t2);

                TableRow tr2 = new TableRow();
                tr2.Height = 12;
                tr2.ForeColor = Color.White;
                System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();
                image.ImageUrl = "~/Image/FL/DownArrow.png";
                TableCell tc2 = new TableCell();
                tc2.Controls.Add(image);
                tr2.Controls.Add(tc2);
                tr2.HorizontalAlign = HorizontalAlign.Center;
                t2.Rows.Add(tr2);
            }
            Table t = new Table();
            t.HorizontalAlign = HorizontalAlign.Center;
            //t.ViewStateMode = System.Web.UI.ViewStateMode.Disabled;
            Panel2.Controls.Add(t);

            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Center;
            t.Rows.Add(tr);
            foreach (string[] ss in oo)
            {
                FLTools.FLRunTimeSubActivity FlSubActivity = new FLTools.FLRunTimeSubActivity();
                FlSubActivity.ID = ss[0];
                FlSubActivity.Name = ss[1];
                FlSubActivity.Layer = o.IndexOf(oo);
                List<string> flStandProperty = new List<string>();
                for (int j = 3; j < ss.Length; j++)
                {
                    flStandProperty.Add(ss[j]);
                }
                FlSubActivity.FLStandProperty = flStandProperty;
                if (ss[2] == "1")
                {
                    FlSubActivity.isSelected();
                }
                tc.Controls.Add(FlSubActivity);
                FlSubActivity.OKClick += new EventHandler(FlSubActivity_OKClick);
            }
            tr.Cells.Add(tc);
            tr.HorizontalAlign = HorizontalAlign.Center;
        }

    }

    /// <summary>
    /// select a stand
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void FlSubActivity_OKClick(object sender, EventArgs e)
    {
        tbName.Text = (sender as FLTools.FLRunTimeSubActivity).Name;
        selectedName = (sender as FLTools.FLRunTimeSubActivity).ID;
        List<string> flStandProperty = (sender as FLTools.FLRunTimeSubActivity).FLStandProperty;
        if (ddlPlusApprove.Items.Contains(new ListItem(flStandProperty[0])))
            ddlPlusApprove.SelectedValue = flStandProperty[0];
        else ddlPlusApprove.SelectedIndex = -1;
        if (ddlRole.Items.Contains(new ListItem(flStandProperty[1])))
            ddlRole.SelectedValue = flStandProperty[1];
        else ddlRole.SelectedIndex = -1;
        if (ddlUser.Items.FindByValue(flStandProperty[2]) != null)
            ddlUser.SelectedValue = flStandProperty[2];
        else ddlUser.SelectedIndex = -1;
        if (flStandProperty[3] == "FLStand")
            ddlType.SelectedIndex = 0;
        else ddlType.SelectedIndex = 1;
        (sender as FLTools.FLRunTimeSubActivity).Selected = true;
        (sender as FLTools.FLRunTimeSubActivity).isSelected();
        SetSelected(selectedName);
        renderPanel(false);
    }

    /// <summary>
    /// set selected stand 's 'selected'=1
    /// </summary>
    /// <param name="selectedName"></param>
    private void SetSelected(string selectedName)
    {
        foreach (List<string[]> liststrings in alllist)
        {
            foreach (string[] liststring in liststrings)
            {
                liststring[2] = "0";
                if (selectedName == liststring[0])
                { liststring[2] = "1"; }
            }
        }
    }

    /// <summary>
    /// delete the selected button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btDelete_Click(object sender, EventArgs e)
    {
        if (selectedName != null)
        {
            foreach (List<string[]> liststrings in alllist)
            {
                bool selected = false;
                int i = 0;
                for (i = 0; i < liststrings.Count; i++)
                {
                    string[] liststring = liststrings[i];
                    if (selectedName == liststring[0])
                    {
                        selected = true;
                        break;
                    }
                }
                if (selected)
                {
                    liststrings.RemoveAt(i);
                }
            }
            renderPanel(false);
        }
    }

    /// <summary>
    /// move up
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btUp_Click(object sender, EventArgs e)
    {
        if (selectedName != null)
        {
            bool selected = false;
            int index = -1;
            foreach (List<string[]> liststrings in alllist)
            {
                int i = 0;
                for (i = 0; i < liststrings.Count; i++)
                {
                    string[] liststring = liststrings[i];
                    if (selectedName == liststring[0] /*&& alllist.IndexOf(liststrings) >0*/)
                    {
                        index = alllist.IndexOf(liststrings);
                        selected = true;
                        break;
                    }
                }
                if (selected) break;
            }
            if (selected)
            {
                if (index > 0)
                {
                    List<string[]> newlist = alllist[index];
                    alllist.RemoveAt(index);
                    alllist.Insert(index - 1, newlist);
                }
            }
            renderPanel(false);
        }
    }

    /// <summary>
    /// move down
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btDown_Click(object sender, EventArgs e)
    {
        if (selectedName != null)
        {
            bool selected = false;
            int index = -1;
            foreach (List<string[]> liststrings in alllist)
            {
                int i = 0;
                for (i = 0; i < liststrings.Count; i++)
                {
                    string[] liststring = liststrings[i];
                    if (selectedName == liststring[0] /*&& alllist.IndexOf(liststrings) + 1 != alllist.Count*/)
                    {
                        index = alllist.IndexOf(liststrings);
                        selected = true;
                        break;
                    }
                }
                if (selected)
                {
                    break;
                }
            }
            if (selected)
            {
                if (index + 1 != alllist.Count)
                {
                    List<string[]> newlist = alllist[index];
                    alllist.RemoveAt(index);
                    alllist.Insert(index + 1,newlist);
                }
            }
            renderPanel(false);
        }
    }

    /// <summary>
    /// Get selected stand 's index
    /// </summary>
    /// <param name="slecteedname"></param>
    /// <param name="newliststring"></param>
    /// <returns></returns>
    private int GetSelectedItemindex(string slecteedname ,ref string[] newliststring)
    {
        bool selected = false;
        int index = -1;
        foreach (List<string[]> liststrings in alllist)
        {
            int i = 0;
            for (i = 0; i < liststrings.Count; i++)
            {
                string[] liststring = liststrings[i];
                if (selectedName == liststring[0] /*&& alllist.IndexOf(liststrings) + 1 != alllist.Count*/)
                {
                    index = alllist.IndexOf(liststrings);
                    newliststring = liststring;
                    selected = true;
                    break;
                }
            }
            if (selected)
            {
                break;
            }
        }
        return index;
    }

    /// <summary>
    /// change the property of the selected stand
    /// </summary>
    protected void Change()
    {
        string text = tbName.Text;
        string plusapprove = ddlPlusApprove.Text;
        string type = ddlType.SelectedIndex == 0 ? "FLStand" : "FLNotify";
        string role = ddlRole.SelectedValue;
        string user = ddlUser.SelectedValue;
        if (selectedName != null)
        {
            bool selected = false;
            int index = -1;
            foreach (List<string[]> liststrings in alllist)
            {
                int i = 0;
                for (i = 0; i < liststrings.Count; i++)
                {
                    string[] liststring = liststrings[i];
                    if (selectedName == liststring[0] /*&& alllist.IndexOf(liststrings) + 1 != alllist.Count*/)
                    {
                        index = alllist.IndexOf(liststrings);
                        liststrings[i] = new string[] { liststring[0], text, liststring[2], plusapprove, role, user,type };
                        selected = true;
                        break;
                    }
                }
                if (selected)
                {
                    break;
                }
            }
            renderPanel(false);
        }

    }

    /// <summary>
    /// set values for users and roles dropdownlist
    /// </summary>
    private void SetUserAndRoles()
    {
        ddlRole.Items.Clear();
        ddlRole.Items.Add("");
        ddlUser.Items.Clear();
        ddlUser.Items.Add("");
        object[] rolesobjs = CliUtils.CallMethod("GLModule", "GetRoles", null);
        if (rolesobjs[0].ToString() == "0")
        {
            string[] objarray = (string[])rolesobjs[1];
            foreach (string role in objarray)
            {
                ddlRole.Items.Add(role);
            }
        }
        object[] userobjs = CliUtils.CallMethod("GLModule", "GetAllUsers", null);
        if (userobjs[0].ToString() == "0")
        {
            DataSet ds = (DataSet)userobjs[1];
            System.Collections.Generic.List<string[]> list = new System.Collections.Generic.List<string[]>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                list.Add(new string[2] {ds.Tables[0].Rows[i]["USERID"].ToString(),ds.Tables[0].Rows[i]["USERNAME"].ToString()});
            }
            foreach (string[] user in list)
            {
                ddlUser.Items.Add(new ListItem(user[0] + " ; " + user[1], user[0]));
            }
        }
    }

    /// <summary>
    /// save xoml
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btSave_Click(object sender, EventArgs e)
    {
        XomlDocument xomlDocument = new XomlDocument();
        SaveDocumentActivity(xomlDocument);

        string filename = tbFileName.Text;
        if(!string.IsNullOrEmpty(filename))
        {
            if (filename.Length <6 || filename.Substring(filename.Length - 5) != ".xoml")
            {
                filename = filename + ".xoml";
            }
            string clientPath = GetClientPath(filename);
            string dir = Path.GetDirectoryName(clientPath);
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            xomlDocument.Save(clientPath);

            string serverPath = GetServerPath(filename);
            if (!string.IsNullOrEmpty(serverPath))
            {
                byte[] bfile = File.ReadAllBytes(clientPath);
                object[] myRet = CliUtils.CallMethod("GLModule", "UpLoadFile", new object[] { serverPath, bfile });
                if (myRet != null && (int)myRet[0] == 0)
                {
                    if ((int)myRet[1] == 0)
                    {
                        this.selectedFileName = filename;
                        this.ClientScript.RegisterStartupScript(typeof(string), "", "alert('保存成功')", true);
                        GetXomlList();
                        ddlFiles.SelectedValue = selectedFileName;
                    }
                    else
                    {
                        this.ClientScript.RegisterStartupScript(typeof(string), "", "alert('文件已存在')", true);
                    }
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(typeof(string), "", "alert('保存失敗')", true);
                }
            }
        }
        
       
    }

    private void GetXomlList()
    {
        object[] fileobjs = CliUtils.CallFLMethod("GetSubFlowFiles", null);
        if (fileobjs[0].ToString() == "0")
        {
            ddlFiles.Items.Clear();
            string[] objarray = (string[])fileobjs[1];
            foreach (string file in objarray)
            {
                ddlFiles.Items.Add(file);
            }
        }
    }

    private string GetServerPath(string fileName)
    {
        object[] obj = CliUtils.CallMethod("GLModule", "GetServerPath", null);
        if (obj[0].ToString() == "0")
        {
            return string.Format(@"{0}\WorkFlow\FL\SubFlows\{1}", obj[1], fileName);
        }
        return string.Empty;
    }


    private string GetClientPath(string fileName)
    {
        return string.Format(@"{0}\{1}\{2}", this.Page.Request.PhysicalApplicationPath, "Temp", fileName);
    }

    /// <summary>
    /// load xoml
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btLoad_Click(object sender, EventArgs e)
    {
        //CliUtils.DownLoad(
        alllist.Clear();
        string filename = ddlFiles.SelectedItem.ToString();

        string clientPath = GetClientPath(filename);
        string dir = Path.GetDirectoryName(clientPath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string serverPath = GetServerPath(filename);
        CliUtils.DownLoad(serverPath, clientPath);
        
        XomlDocument xomlDocument = new XomlDocument();
        xomlDocument.Load(clientPath);
        this.selectedFileName = filename;
        LoadDocumentActivityAndRender(xomlDocument);
    }

    private void SaveDocumentActivity(XomlDocument xomlDocument)
    {
        int sequenceactivityindex = 1;
        int parallelactivity = 1;
        foreach (List<string[]> lstring in alllist)
        {
            if (lstring.Count > 1)
            {
                DocumentActivity da2 = xomlDocument.CreateActivity(typeof(System.Workflow.Activities.ParallelActivity).Name);
                da2.Name = "ParallelActivity" + parallelactivity.ToString();
                parallelactivity++;
                for (int i = 0; i < lstring.Count; i++)
                {
                    DocumentActivity sa = xomlDocument.CreateActivity(typeof(System.Workflow.Activities.SequenceActivity).Name);
                    sa.Name = "SequenceActivity" + sequenceactivityindex.ToString();
                    sequenceactivityindex++;
                    Type t;
                    if ((lstring[i] as string[])[6] == "FLStand")
                    {
                        t = typeof(FLTools.FLStand);
                    }
                    else
                    {
                        t = typeof(FLTools.FLNotify);
                    }
                    DocumentActivity da = xomlDocument.CreateFLActivity(t.Name);
                    da.Name = (lstring[i] as string[])[1];
                    //da.Description = (lstring[i] as string[])[2];
                    if ((lstring[i] as string[])[6] == "FLStand")
                    {
                        da["PlusApprove"] = (lstring[i] as string[])[3];
                    }
                    if ((lstring[i] as string[])[5] != "")
                    { da["SendToKind"] = "User"; }
                    else if ((lstring[i] as string[])[4] != "")
                    { da["SendToKind"] = "Role"; }
                    else da["SendToKind"] = "";
                    da["SendToRole"] = (lstring[i] as string[])[4];
                    da["SendToUser"] = (lstring[i] as string[])[5];
                    sa.AppendChildActivity(da);
                    da2.AppendChildActivity(sa);
                }
                xomlDocument.RootActivity.AppendChildActivity(da2);
            }
            else
            {
                Type t;
                if ((lstring[0] as string[])[6] == "FLStand")
                {
                    t = typeof(FLTools.FLStand);
                }
                else
                {
                    t = typeof(FLTools.FLNotify);
                }
                DocumentActivity da = xomlDocument.CreateFLActivity(t.Name);
                da.Name = (lstring[0] as string[])[1];
                //da.Description = (lstring[0] as string[])[2];
                if ((lstring[0] as string[])[6] == "FLStand")
                {
                    da["PlusApprove"] = (lstring[0] as string[])[3];
                }
                if ((lstring[0] as string[])[5] != "")
                { da["SendToKind"] = "User"; }
                else if ((lstring[0] as string[])[4] != "")
                { da["SendToKind"] = "Role"; }
                else da["SendToKind"] = "";
                da["SendToRole"] = (lstring[0] as string[])[4];
                da["SendToUser"] = (lstring[0] as string[])[5];
                xomlDocument.RootActivity.AppendChildActivity(da);
            }
        }

    }

    private void LoadDocumentActivityAndRender(XomlDocument xomlDocument)
    {
        foreach (DocumentActivity documentActivity in xomlDocument.RootActivity.ChildActivities)
        {
            if (documentActivity.Type == "FLStand")
            {
                List<string[]> list = new List<string[]>();
                string id = Guid.NewGuid().ToString();
                string name = documentActivity.Name;
                //string description = documentActivity.Description;
                string plusArrpove = documentActivity["PlusApprove"];
                string sendToRole = documentActivity["SendToRole"];
                string sendToUser = documentActivity["SendToUser"];
                //0:id,1:name,2:description,3:selected or not,4:plusApprove,5:kind,6:role,7:user
                string[] sarray = new string[] { id, name, "0"/*是否选中，0表示false,1表示true*/, plusArrpove, sendToRole, sendToUser, "FLStand" };
                list.Add(sarray);
                alllist.Add(list);
            }
            else if (documentActivity.Type == "FLNotify")
            {
                List<string[]> list = new List<string[]>();
                string id = Guid.NewGuid().ToString();
                string name = documentActivity.Name;
                //string description = documentActivity.Description;
                string sendToRole = documentActivity["SendToRole"];
                string sendToUser = documentActivity["SendToUser"];
                //0:id,1:name,2:description,3:selected or not,4:plusApprove,5:kind,6:role,7:user
                string[] sarray = new string[] { id, name, "0"/*是否选中，0表示false,1表示true*/, "False", sendToRole, sendToUser, "FLNotify" };
                list.Add(sarray);
                alllist.Add(list);
            }
            else if (documentActivity.Type == "ParallelActivity")
            {
                List<string[]> list = new List<string[]>();
                foreach (DocumentActivity sequenceActivity in documentActivity.ChildActivities)
                {
                    if (sequenceActivity.ChildActivities.Count > 0)
                    {
                        DocumentActivity standActivity = sequenceActivity.ChildActivities[0];
                        string id = Guid.NewGuid().ToString();
                        string name = standActivity.Name;
                        string type = standActivity.Type;
                        //string description = standActivity.Description;
                        string plusArrpove ="False";
                        if (type == "FLStand")
                            plusArrpove = standActivity["PlusApprove"];
                        string sendToRole = standActivity["SendToRole"];
                        string sendToUser = standActivity["SendToUser"];
                        
                        //0:id,1:name,2:description,3:selected or not,4:plusApprove,5:kind,6:role,7:user
                        string[] sarray = new string[] { id, name, "0"/*是否选中，0表示false,1表示true*/, plusArrpove, sendToRole, sendToUser, type };
                        list.Add(sarray);
                    }
                }
                alllist.Add(list);
            }
            renderPanel(true);
        }
    }

    protected void btClose_Click(object sender, EventArgs e)
    {
        string script = "";
        if (ControlID != "")
            script += "window.opener.document.getElementById('" + ControlID + "')" + ".value='" + this.selectedFileName + "';";
        this.Page.Response.Write("<script language=javascript>" + script + "window.close();</script>");
    }
}