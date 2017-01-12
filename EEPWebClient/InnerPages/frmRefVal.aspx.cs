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
using System.Resources;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

public partial class InnerPages_frmRefVal : System.Web.UI.Page
{
    private SYS_LANGUAGE language;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            String message = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                   "Srvtools",
                                   "WebRefVal",
                                   "ButtonName", true);
            string[] buttons = message.Split(';');
            this.btnQuery.Text = buttons[0];
            this.btnRefresh.Text = buttons[1];
            this.btnCancel.Text = buttons[3];
            this.btnAdd.Text = buttons[4];
            bool allowAddData = false;
            if (Request.QueryString["RefValID"] != null)
            {
                this.PagePath = GetSessionValue("PagePath");
                this.Resx = GetSessionValue("Resx");
                this.SourceDataSet = GetSessionValue("DataSet");
                this.ValueField = GetSessionValue("ValueField");
                this.TextField = GetSessionValue("TextField");
                this.WhereItem = GetSessionValue("WhereItem");
                this.DBAlias = GetSessionValue("DBAlias");
                this.CommandText = GetSessionValue("CommandText");
                this.ColumnMatch = GetSessionValue("ColumnMatch");
                this.Columns = GetSessionValue("Columns");
                this.SourceControl = GetSessionValue("SourceControl");
                this.QueryCondition = GetSessionValue("QueryCondition");
                this.PacketRecords = GetSessionValue("PacketRecords") == null ? -1 : Convert.ToInt32(GetSessionValue("PacketRecords"));
                this.Caption = GetSessionValue("Caption");
                this.MultiLan = GetSessionValue("MultiLan");
                allowAddData = Convert.ToBoolean(GetSessionValue("AllowAddData"));
            }
            else
            {

                if (Request.QueryString["QueryCondition"] == null)
                {
                    QueryStringEncrypt.Check(this, new string[] { "&TypeAhead" }, false);
                }
                else
                {
                    QueryStringEncrypt.Check(this, new string[] { "&TypeAhead" }, false);
                }

                this.PagePath = Request.QueryString["PagePath"];
                this.Resx = Request.QueryString["Resx"];
                this.SourceDataSet = Request.QueryString["DataSet"];
                this.ValueField = Request.QueryString["ValueField"];
                this.TextField = Request.QueryString["TextField"];
                this.WhereItem = Request.QueryString["WhereItem"];
                this.DBAlias = Request.QueryString["DBAlias"];
                this.CommandText = Request.QueryString["CommandText"];
                this.ColumnMatch = Request.QueryString["ColumnMatch"];
                this.Columns = Request.QueryString["Columns"];
                this.SourceControl = Request.QueryString["SourceControl"];
                this.QueryCondition = Request.QueryString["QueryCondition"];
                this.PacketRecords = Request.QueryString["PacketRecords"] == null ? -1 : Convert.ToInt32(Request.QueryString["PacketRecords"]);
                this.Caption = Request.QueryString["Caption"];
                this.MultiLan = Request.QueryString["MultiLan"];
                allowAddData = Convert.ToBoolean(Request.QueryString["AllowAddData"]);
            }
           
            this.TypeAhead = Request.QueryString["TypeAhead"];

            if ((this.Resx != "" && this.SourceDataSet != "") || (this.DBAlias != "" && this.CommandText != "")
                && this.ValueField != "" && this.TextField != "")
            {
                if (this.QueryCondition == "")
                    GetData(false, 1, true);
                else
                    GetData(true, 1, true);
                string QueryFields = "", DataTypes = "", Captions = "";
                foreach (DataColumn field in ((DataSet)this.ViewState["WebDataSet"]).Tables[0].Columns)
                {
                    QueryFields += field.ColumnName + ";";
                    DataTypes += field.DataType.ToString() + ";";
                }
                Captions = this.HeadTexts;
                QueryFields = QueryFields.Substring(0, QueryFields.LastIndexOf(';'));
                DataTypes = DataTypes.Substring(0, DataTypes.LastIndexOf(';'));

                this.btnQuery.Visible = true;
                this.btnRefresh.Visible = true;
                this.btnCancel.Visible = true;
                //bool allowAddData = Convert.ToBoolean(Request.QueryString["AllowAddData"]);
                if (allowAddData/* && this.DBAlias == "" && this.CommandText == ""*/)
                {
                    this.btnAdd.Visible = true;
                }


                string param = "QueryFields=" + HttpUtility.UrlEncode(QueryFields) + "&DataTypes=" + HttpUtility.UrlEncode(DataTypes) +
                            "&Captions=" + HttpUtility.UrlEncode(Captions) + "&Resx=" + HttpUtility.UrlEncode(this.Resx) +
                            "&DataSet=" + HttpUtility.UrlEncode(this.SourceDataSet) + "&ValueField=" + HttpUtility.UrlEncode(this.ValueField) +
                            "&TextField=" + HttpUtility.UrlEncode(this.TextField) + "&WhereItem=" + HttpUtility.UrlEncode(this.WhereItem) +
                            "&DBAlias=" + HttpUtility.UrlEncode(this.DBAlias) + "&CommandText=" + HttpUtility.UrlEncode(this.CommandText) +
                            "&ColumnMatch=" + HttpUtility.UrlEncode(this.ColumnMatch) + "&Columns=" + HttpUtility.UrlEncode(this.Columns) +
                            "&SourceControl=" + HttpUtility.UrlEncode(this.SourceControl) + "&PacketRecords=" + this.PacketRecords.ToString() +
                            "&PagePath=" + this.PagePath + "&MultiLan=" + this.MultiLan;
                if (Request.QueryString["RefValID"] != null)
                {
                    string id = Request.QueryString["RefValID"];
                    this.Session[id] = param;
                    this.QueryParams = "../InnerPages/frmRefValQuery.aspx?RefValID=" + id;
                }
                else
                {
                    param = QueryStringEncrypt.Encrypt(param);
                    this.QueryParams = "../InnerPages/frmRefValQuery.aspx?" + param;
                }
            }
        }
        ////////////////////////////////////////////////////////////////////!ispostback
        this.Title = this.Caption;
        if (this.gridView.HeaderRow != null)
        {
            int x = this.gridView.HeaderRow.Cells.Count;
            for (int y = 0; y < x; y++)
            {
                this.gridView.HeaderRow.Cells[y].Wrap = false;
            }
        }
    }


    private string GetSessionValue(string key)
    {
        if (!string.IsNullOrEmpty(key))
        {
            string content = string.Empty;
            if (this.Request.QueryString["RefValID"] != null)
            {
                content = (string)this.Session[this.Request.QueryString["RefValID"]];
            }
            string[] list = content.Split('&');
            foreach (string str in list)
            {
                if (str.StartsWith(string.Format("{0}=", key)))
                {
                    if (str.Length > key.Length + 1)
                    {
                        return HttpUtility.UrlDecode(str.Substring(key.Length + 1));
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
        }
        return null;
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (this.QueryParams != "")
        {
            Page.Response.Redirect(this.QueryParams);
        }
    }

    #region Properties
    public string Caption
    {
        get
        {
            object obj = this.ViewState["Caption"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["Caption"] = value;
        }
    }

    public string QueryParams
    {
        get
        {
            object obj = this.ViewState["QueryParams"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["QueryParams"] = value;
        }
    }

    public string HeadTexts
    {
        get
        {
            object obj = this.ViewState["HeadTexts"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["HeadTexts"] = value;
        }
    }

    public string Resx
    {
        get
        {
            object obj = this.ViewState["Resx"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["Resx"] = value;
        }
    }

    public string PagePath
    {
        get
        {
            object obj = this.ViewState["PagePath"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["PagePath"] = value;
        }
    }

    public string SourceDataSet
    {
        get
        {
            object obj = this.ViewState["SourceDataSet"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["SourceDataSet"] = value;
        }
    }

    public string ValueField
    {
        get
        {
            object obj = this.ViewState["ValueField"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["ValueField"] = value;
        }
    }

    public string TextField
    {
        get
        {
            object obj = this.ViewState["TextField"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["TextField"] = value;
        }
    }

    public string WhereItem
    {
        get
        {
            object obj = this.ViewState["WhereItem"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["WhereItem"] = value;
        }
    }

    public string DBAlias
    {
        get
        {
            object obj = this.ViewState["DBAlias"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["DBAlias"] = value;
        }
    }

    public string CommandText
    {
        get
        {
            object obj = this.ViewState["CommandText"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["CommandText"] = value;
        }
    }

    public string ColumnMatch
    {
        get
        {
            object obj = this.ViewState["ColumnMatch"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["ColumnMatch"] = value;
        }
    }

    public string Columns
    {
        get
        {
            object obj = this.ViewState["Columns"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["Columns"] = value;
        }
    }

    public string SourceControl
    {
        get
        {
            object obj = this.ViewState["SourceControl"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["SourceControl"] = value;
        }
    }

    public string QueryCondition
    {
        get
        {
            object obj = this.ViewState["QueryCondition"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["QueryCondition"] = value;
        }
    }

    public int PacketRecords
    {
        get
        {
            object obj = this.ViewState["PacketRecords"];
            if (obj != null)
            {
                return (int)obj;
            }
            return 100;
        }
        set
        {
            this.ViewState["PacketRecords"] = value;
        }
    }

    public string PackageName
    {
        get
        {
            object obj = this.ViewState["PackageName"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["PackageName"] = value;
        }
    }

    public string InfoCommandName
    {
        get
        {
            object obj = this.ViewState["InfoCommandName"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["InfoCommandName"] = value;
        }
    }

    public string TypeAhead
    {
        get
        {
            object obj = this.ViewState["TypeAhead"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["TypeAhead"] = value;
        }
    }

    public string MultiLan
    {
        get
        {
            object obj = this.ViewState["MultiLan"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["MultiLan"] = value;
        }
    }

    #endregion

    private WebDataSet wds = new WebDataSet(false);
    private void GetData(bool QueryReturn, int packageCount, bool reload)
    {
        if (reload)
        {
            this.gridView.PageIndex = 0;
        }
        if (this.DBAlias != "" && this.CommandText != "")
        {
            DataSet tempDs = new DataSet();
            string where = "";
            if (this.WhereItem != "")
            {
                where = this.WhereItem;
            }
            if (QueryReturn && this.QueryCondition.Trim() != "")
            {
                if (where != "")
                {
                    where += " and " + this.QueryCondition;
                }
                else
                {
                    where += this.QueryCondition;
                }
            }
            string strSql = this.CommandText;
            if (where != "")
            {
                strSql = CliUtils.InsertWhere(strSql, where);
            }
            if(!string.IsNullOrEmpty(this.TypeAhead) && this.TypeAhead.EndsWith("?"))
            {
                string ta = this.TypeAhead.Substring(0, this.TypeAhead.Length - 1);
                strSql = CliUtils.InsertWhere(strSql, string.Format("{0} like '{1}%'", this.ValueField, ta));
            }

            int lastIndex = (packageCount - 1) * this.PacketRecords - 1;
            tempDs = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", strSql, "", true, CliUtils.fCurrentProject, new object[] { this.PacketRecords, lastIndex });

            if (this.ViewState["WebDataSet"] != null && lastIndex != -1 && PacketRecords != -1)
            {
                ((DataSet)this.ViewState["WebDataSet"]).Merge(tempDs);
            }
            else
            {
                this.ViewState["WebDataSet"] = tempDs;
            }
        }
        else
        {
            #region CreateWebDataSet
            XmlDocument xmlDoc = new XmlDocument();
            string resourceName = this.Resx;
            ResXResourceReader reader = new ResXResourceReader(resourceName);
            IDictionaryEnumerator enumerator = reader.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString() == "WebDataSets")
                {
                    string sXml = (string)enumerator.Value;
                    xmlDoc.LoadXml(sXml);
                    break;
                }
            }
            if (xmlDoc != null)
            {
                XmlNode nWDSs = xmlDoc.SelectSingleNode("WebDataSets");
                if (nWDSs != null)
                {
                    string webDataSetID = this.SourceDataSet;
                    XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSet[@Name='" + webDataSetID + "']");
                    if (nWDS != null)
                    {
                        string remoteName = "";
                        int packetRecords = 100;
                        bool active = false;
                        bool serverModify = false;

                        XmlNode nRemoteName = nWDS.SelectSingleNode("RemoteName");
                        if (nRemoteName != null)
                            remoteName = nRemoteName.InnerText;

                        XmlNode nPacketRecords = nWDS.SelectSingleNode("PacketRecords");
                        if (nPacketRecords != null)
                            packetRecords = nPacketRecords.InnerText.Length == 0 ? 100 : Convert.ToInt32(nPacketRecords.InnerText);

                        XmlNode nActive = nWDS.SelectSingleNode("Active");
                        if (nActive != null)
                            active = nActive.InnerText.Length == 0 ? false : Convert.ToBoolean(nActive.InnerText);

                        XmlNode nServerModify = nWDS.SelectSingleNode("ServerModify");
                        if (nServerModify != null)
                            serverModify = nServerModify.InnerText.Length == 0 ? false : Convert.ToBoolean(nServerModify.InnerText);

                        wds.RemoteName = remoteName;
                        if (remoteName.IndexOf('.') != -1)
                        {
                            this.PackageName = remoteName.Split('.')[0];
                            this.InfoCommandName = remoteName.Split('.')[1];
                        }
                        wds.PacketRecords = packetRecords;
                        wds.ServerModify = serverModify;
                        this.PacketRecords = packetRecords;
                        string where = "";
                        if (QueryReturn && this.QueryCondition.Trim() != "")
                        {
                            if (this.WhereItem != "")
                                where = this.WhereItem + " and " + this.QueryCondition;
                            else
                                where = this.QueryCondition;
                        }
                        else
                        {
                            if (this.WhereItem != "")
                                where = this.WhereItem;
                        }
                        if (!string.IsNullOrEmpty(this.TypeAhead) && this.TypeAhead.EndsWith("?"))
                        {
                            string ta = this.TypeAhead.Substring(0, this.TypeAhead.Length - 1);
                            if (where != "")
                            {
                                where += string.Format(" and {0} like '{1}%'", this.ValueField, ta);
                            }
                            else
                            {
                                where = string.Format(" {0} like '{1}%'", this.ValueField, ta); ;
                            }
                        }
                        if (where != "")
                        {
                            wds.SetWhere(where);
                        }

                        wds.Active = true;
                        if (packageCount > 1)
                        {
                            for (int i = 1; i < packageCount; i++)
                            {
                                if (!wds.GetNextPacket())
                                {
                                    break;
                                }
                            }
                        }
                        this.ViewState["WebDataSet"] = wds.RealDataSet;
                    }
                }
            }
            #endregion
        }

        if (this.Columns == "")
        {
            this.gridView.AutoGenerateColumns = true;
        }
        else
        {
            this.gridView.AutoGenerateColumns = false;
            if ((this.gridView.Columns[0] is TemplateField && this.gridView.Columns.Count == 1)
                || this.gridView.Columns.Count == 0)
            {
                List<string> colName = new List<string>();
                List<string> headText = new List<string>();
                List<int> width = new List<int>();
                string[] columns = this.Columns.Split(';');
                int i = columns.Length;
                for (int j = 0; j < i; j++)
                {
                    string[] colparams = columns[j].Split(',');
                    if (colparams.Length > 3)
                    {
                        colName.Add(colparams[0]);
                        headText.Add(colparams[1]);
                        width.Add(Convert.ToInt32(colparams[2]));
                    }
                }
                int m = colName.Count;
                for (int n = 0; n < m; n++)
                {
                    BoundField field = new BoundField();
                    field.DataField = colName[n];
                    field.HeaderText = headText[n];
                    field.HeaderStyle.Width = width[n];
                    this.gridView.Columns.Add(field);
                }
            }
        }
        this.gridView.DataSource = (DataSet)this.ViewState["WebDataSet"];
        this.gridView.DataMember = ((DataSet)this.ViewState["WebDataSet"]).Tables[0].TableName;
        this.gridView.DataBind();

        // get dd...
        string CaptionNum = "CAPTION";
        if (string.Compare(MultiLan, "true", true) == 0)//IgnoreCase
        {
            switch (CliUtils.fClientLang)
            {
                case SYS_LANGUAGE.ENG:
                    CaptionNum = "CAPTION1"; break;
                case SYS_LANGUAGE.TRA:
                    CaptionNum = "CAPTION2"; break;
                case SYS_LANGUAGE.SIM:
                    CaptionNum = "CAPTION3"; break;
                case SYS_LANGUAGE.HKG:
                    CaptionNum = "CAPTION4"; break;
                case SYS_LANGUAGE.JPN:
                    CaptionNum = "CAPTION5"; break;
                case SYS_LANGUAGE.LAN1:
                    CaptionNum = "CAPTION6"; break;
                case SYS_LANGUAGE.LAN2:
                    CaptionNum = "CAPTION7"; break;
                case SYS_LANGUAGE.LAN3:
                    CaptionNum = "CAPTION8"; break;
            }
        }
        DataSet ds = GetDD();
        if (ds.Tables.Count != 0 && this.gridView.HeaderRow != null)
        {
            foreach (TableCell cell in this.gridView.HeaderRow.Cells)
            {
                if (cell.Text != null && cell.Text != "")
                {
                    int x = ds.Tables[0].Rows.Count;
                    for (int y = 0; y < x; y++)
                    {
                        if (string.Compare(ds.Tables[0].Rows[y]["FIELD_NAME"].ToString(), cell.Text, true) == 0//IgnoreCase
                            && ds.Tables[0].Rows[y][CaptionNum].ToString() != "")
                        {
                            cell.Text = ds.Tables[0].Rows[y][CaptionNum].ToString();
                        }
                    }
                }
            }
        }

        //Save HeadTexts
        HeadTexts = "";
        foreach (DataColumn field in ((DataSet)this.ViewState["WebDataSet"]).Tables[0].Columns)
        {
            bool ddFound = false;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (string.Compare(field.ColumnName, row["FIELD_NAME"].ToString(), true) == 0//IgnoreCase
                    && row[CaptionNum] != null && row[CaptionNum].ToString() != "")
                {
                    HeadTexts += row[CaptionNum].ToString() + ";";
                    ddFound = true;
                    break;
                }
            }
            if (!ddFound)
            {
                HeadTexts += field.ColumnName + ";";
            }
        }
    }

    private DataSet GetDD()
    {
        WebDataSource webdataSource = new WebDataSource();
        webdataSource.SelectAlias = DBAlias;
        webdataSource.SelectCommand = CommandText;
        string strRemoteModule = wds.RemoteName;
        webdataSource.RemoteName = strRemoteModule;
        if (!string.IsNullOrEmpty(webdataSource.RemoteName))
        {
            webdataSource.DataMember = webdataSource.RemoteName.Split('.')[1];
        }
        DataSet ds = DBUtils.GetDataDictionary(webdataSource, false);
        return ds;
    }

    protected void gridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet tempDs = (DataSet)this.ViewState["WebDataSet"];
        DataColumnCollection dcc = null;
        if (tempDs != null)
        {
            dcc = tempDs.Tables[0].Columns;
        }
        string strValue = "";
        string strText = "";
        int rowIndex = gridView.PageSize * gridView.PageIndex + gridView.SelectedIndex;
        DataRow row = tempDs.Tables[0].Rows[rowIndex];
        string strColumnMatch = this.ColumnMatch;
        //List<string> SrcColumns = new List<string>();
        //List<string> DestCtrls = new List<string>();
        //List<string> DestValues = new List<string>();
        Hashtable tab = new Hashtable();
        if (strColumnMatch != "")
        {
            string[] ColumnMatchList = strColumnMatch.Split(';');
            foreach (string colMatch in ColumnMatchList)
            {
                string[] matchfield = Regex.Split(colMatch, "%%%");
                if (matchfield.Length == 3)
                {
                    string key = matchfield[0];
                    string value = string.Empty;
                    if (matchfield[1].Length > 0)
                    {
                        tab.Add(key, row[matchfield[1]].ToString());
                    }
                    else
                    {
                        tab.Add(key, matchfield[2]);
                    }
                }
            }
        }
        strValue = row[ValueField].ToString();
        strText = row[TextField].ToString();

        strValue = strValue.Replace("'", "\\'");
        strText = strText.Replace("'", "\\'");

        string script = "window.opener.document.getElementById('" + SourceControl + "_ValueInput').value = '" + strValue + "';"
            + "window.opener.document.getElementById('" + SourceControl + "_TextInput').value = '" + strText + "';"
            + "window.opener.document.getElementById('" + SourceControl + "_ValueSelect').value = '" + strValue + "';"
            + "window.opener.document.getElementById('" + SourceControl + "_InnerTextBox').value = '" + strText + "';"
            //+ "window.opener.document.getElementById('" + SourceControl + "_InnerTextBox').fireEvent(\"onchange\");"
            + GetOnChangeJS(SourceControl)
            + "var focusControls=[];";
        string stemp = SourceControl.Substring(0, SourceControl.LastIndexOf('_') + 1);
        IDictionaryEnumerator enumerator = tab.GetEnumerator();
        while (enumerator.MoveNext())
        {
            string stemp1 = stemp + enumerator.Key.ToString() + "_TextInput";
            string stemp2 = stemp + enumerator.Key.ToString() + "_ValueInput";
            string stemp3 = stemp + enumerator.Key.ToString() + "_ValueSelect";
            string stemp4 = stemp + enumerator.Key.ToString() + "_InnerTextBox";
            string value = enumerator.Value.ToString().Replace("'", "\\'");
            script += "ctrl=window.opener.document.getElementById('" + stemp + enumerator.Key.ToString() + "');" +
                        "if(ctrl != undefined && ctrl.value != undefined)" +
                        "{" +
                            "ctrl.value = '" + enumerator.Value.ToString().Replace("'", "\\'") + "';" +
                        "}" +
                        "else" +
                        "{" +
                            "ctrl=window.opener.document.getElementById('" + stemp2 + "');" +
                            "if(ctrl != undefined)" +
                            "{" +
                                "ctrl.value = '" + value + "';" +
                                "window.opener.document.getElementById('" + stemp3 + "').value='" + value + "';" +
                                string.Format("window.opener._refMatch({{ddl:'{0}',show:'{1}'}},'{2}');", stemp3, stemp4, value) +
                                //"var refBox=window.opener.document.getElementById('" + stemp4 + "');" +
                                //"focusControls.push(window.opener.document.getElementById('" + stemp4 + "'));" +
                                //"window.opener.document.getElementById('" + stemp4 + "').focus();" +
                            "}" +
                        "}"
                        //"window.close();" +
                        //"for(i=0;i<focusControls.length;i++)" + 
                        //"{" +
                            //"window.setTimeout(function(){focusControls[i].focus();}, 0);" +
                            //"window.setTimeout(function(){focusControls[i].focus();if(i==focusControls.length-1){window.close();}}, 0);" +
                            //"focusControls[i].focus();" + 
                        //"}"
                        ;
        }
        //Response.Write("<script language=javascript>" + script + "</script>");
        Response.Write("<script language=javascript>" + script + "window.close();</script>");
    }

    private string GetOnChangeJS(string controlID)
    {
        return "var t = window.opener.document.getElementById('" + SourceControl + "_InnerTextBox');"
            + "if(document.all)"
            + "{"
                + "t.fireEvent(\"onchange\");"
            + "}"
            + "else"
            + "{"
                + "var evt=document.createEvent('HTMLEvents');"
                + "evt.initEvent('change',true,true);"
                + "t.dispatchEvent(evt); "
            + "}";
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        this.gridView.ShowFooter = false;
        GetData(false, 1, true);
        this.QueryCondition = "";
        if (this.gridView.HeaderRow != null)
        {
            int x = this.gridView.HeaderRow.Cells.Count;
            for (int y = 0; y < x; y++)
            {
                this.gridView.HeaderRow.Cells[y].Wrap = false;
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=javascript>window.close();</script>");
    }

    protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int packageCount = this.PacketRecords;
        if (this.PacketRecords != -1)
        {
            int currentDataCount = this.gridView.PageCount * this.gridView.PageSize;
            packageCount = currentDataCount / this.PacketRecords;
        }
        this.gridView.PageIndex = e.NewPageIndex;
        if (this.QueryCondition == "")
            GetData(false, packageCount + 1, false);
        else
            GetData(true, packageCount + 1, false);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ChangeInsertMode(true);
    }

    private void ChangeInsertMode(bool Insert)
    {
        this.gridView.ShowFooter = Insert;
        int packageCount = this.PacketRecords;
        if (this.PacketRecords != 0)
        {
            int currentDataCount = this.gridView.PageCount * this.gridView.PageSize;
            packageCount = currentDataCount / this.PacketRecords;
        }
        if (this.QueryCondition == "")
            GetData(false, packageCount + 1, false);
        else
            GetData(true, packageCount + 1, false);
    }

    protected void imgBtnOK_Click(object sender, ImageClickEventArgs e)
    {
        if (this.ViewState["WebDataSet"] != null && this.ViewState["WebDataSet"] is DataSet)
        {
            DataTable table = ((DataSet)this.ViewState["WebDataSet"]).Tables[0];
            GridViewRow footRow = this.gridView.FooterRow;
            string cmd = "";
            int i = footRow.Cells.Count;
            List<string> fields = new List<string>();
            List<string> values = new List<string>();
            List<string> types = new List<string>();
            for (int j = 1; j < i; j++)
            {
                foreach (Control ctrl in footRow.Cells[j].Controls)
                {
                    if (ctrl is TextBox)
                    {
                        TextBox txt = (TextBox)ctrl;
                        fields.Add(txt.ID);
                        values.Add(txt.Text);
                        foreach (DataColumn column in table.Columns)
                        {
                            if (column.ColumnName == txt.ID)
                            {
                                types.Add(column.DataType.ToString().ToLower());
                            }
                        }
                    }
                }
            }
            string strFields = "";
            string strValues = "";
            if (fields.Count == values.Count && fields.Count == types.Count)
            {
                int x = fields.Count;
                for (int y = 0; y < x; y++)
                {
                    if (values[y] != "")
                    {
                        strFields += fields[y] + ",";
                        string type = types[y];
                        if (type == "system.uint" || type == "system.uint16" || type == "system.uint32"
                        || type == "system.uint64" || type == "system.int" || type == "system.int16"
                        || type == "system.int32" || type == "system.int64" || type == "system.single"
                        || type == "system.double" || type == "system.decimal")
                        {
                            strValues += values[y] + ",";
                        }
                        else
                        {
                            strValues += "'" + values[y] + "',";
                        }
                    }

                }
                if (strFields != "")
                {
                    strFields = strFields.Substring(0, strFields.LastIndexOf(','));
                }
                if (strValues != "")
                {
                    strValues = strValues.Substring(0, strValues.LastIndexOf(','));
                }
            }
            string tabName = "";
            if (this.DBAlias != "" && this.CommandText != "")
            {
                tabName = CliUtils.GetTableName(this.CommandText, true);
            }
            else if (this.PackageName != "" && this.InfoCommandName != "")
            {
                tabName = CliUtils.GetTableName(this.PackageName, this.InfoCommandName, CliUtils.fCurrentProject, null, true);

            }
            if (tabName != "" && strFields != "" && strValues != "")
            {
                cmd = "insert into " + tabName + " (" + strFields + ") values (" + strValues + ")";
                CliUtils.ExecuteSql("GLModule", "cmdRefValUse", cmd, true, CliUtils.fCurrentProject);
            }
            ChangeInsertMode(false);
        }
    }

    protected void imgBtnCancel_Click(object sender, ImageClickEventArgs e)
    {
        ChangeInsertMode(false);
    }

    protected void gridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            int i = (this.Columns == null || this.Columns == "") ? e.Row.Cells.Count - 1 : this.Columns.Split(';').Length;
            for (int j = 0; j < i; j++)
            {
                string ctrlID = "";
                if (this.Columns != null && this.Columns != "")
                {
                    string columnParams = this.Columns.Split(';')[j];
                    ctrlID = columnParams.Split(',')[0];
                }
                else
                {
                    if (this.ViewState["WebDataSet"] != null && this.ViewState["WebDataSet"] is DataSet)
                    {
                        DataTable table = ((DataSet)this.ViewState["WebDataSet"]).Tables[0];
                        ctrlID = table.Columns[j].ColumnName;
                    }
                }
                TextBox txt = new TextBox();
                txt.ID = ctrlID;
                txt.Width = 100;
                e.Row.Cells[j + 1].Controls.Add(txt);
            }
        }
    }
}