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
using System.Resources;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public partial class InnerPages_frmRefValQuery : System.Web.UI.Page
{
    List<string> queryFields = new List<string>();
    List<string> dataTypes = new List<string>();
    int colNum = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["RefValID"] != null)
            {
                this.QueryFields = GetSessionValue("QueryFields");
                this.DataTypes = GetSessionValue("DataTypes");
                this.Captions = GetSessionValue("Captions");

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
                this.PacketRecords = GetSessionValue("PacketRecords");
                this.MultiLan = GetSessionValue("MultiLan");
            }
            else
            {
                QueryStringEncrypt.Check(this, null, false);
                this.QueryFields = Request.QueryString["QueryFields"];
                this.DataTypes = Request.QueryString["DataTypes"];
                this.Captions = Request.QueryString["Captions"];

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
                this.PacketRecords = Request.QueryString["PacketRecords"];
                this.MultiLan = Request.QueryString["MultiLan"];
            }
        }

        string[] qf = null, dt = null, queryField = this.QueryFields.Split(';'), dataType = this.DataTypes.Split(';');
        if (this.Columns != null && this.Columns != "")
        {
            List<string> qfLst = new List<string>();
            List<string> dtLst = new List<string>();

            string[] columns = this.Columns.Split(';');
            int x = columns.Length;
            for (int y = 0; y < x; y++)
            {
                string[] colParams = columns[y].Split(',');
                int m = queryField.Length;
                for (int n = 0; n < m; n++)
                {
                    if (string.Compare(colParams[0], queryField[n], true) == 0)
                    {
                        string caption = queryField[n];
                        qfLst.Add(caption);
                        dtLst.Add(dataType[n]);
                    }
                }
            }

            qf = qfLst.ToArray();
            dt = dtLst.ToArray();
        }
        else
        {
            qf = queryField;
            dt = dataType;
        }

        colNum = qf.Length;
        for (int i = 0; i < colNum; i++)
        {
            queryFields.Add(qf[i]);
            dataTypes.Add(dt[i]);
        }
        InitializeQueryConditionItem(queryFields, dataTypes, colNum);
    }

    //private WebDataSet wds = new WebDataSet(false);
    private DataSet GetDD()
    {
        WebDataSource webdataSource = new WebDataSource();
        webdataSource.SelectAlias = DBAlias;
        webdataSource.SelectCommand = CommandText;
        String strRemoteModule = GetRemoteName(PagePath, SourceDataSet);
        //string strRemoteModule = wds.RemoteName;
        webdataSource.RemoteName = strRemoteModule;
        if (!string.IsNullOrEmpty(webdataSource.RemoteName))
        {
            webdataSource.DataMember = webdataSource.RemoteName.Split('.')[1];
        }
        DataSet ds = DBUtils.GetDataDictionary(webdataSource, false);
        return ds;
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

    public string QueryFields
    {
        get
        {
            object obj = this.ViewState["QueryFields"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["QueryFields"] = value;
        }
    }

    public string DataTypes
    {
        get
        {
            object obj = this.ViewState["DataTypes"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["DataTypes"] = value;
        }
    }

    public string Captions
    {
        get
        {
            object obj = this.ViewState["Captions"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["Captions"] = value;
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

    public string PacketRecords
    {
        get
        {
            object obj = this.ViewState["PacketRecords"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "-1";
        }
        set
        {
            this.ViewState["PacketRecords"] = value;
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

    private List<string> GetTexts()
    {
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

        List<string> retList = new List<string>();
        if (this.Columns != null && this.Columns != "")
        {
            string[] columns = this.Columns.Split(';');
            int x = columns.Length;

            for (int y = 0; y < x; y++)
            {
                string[] colParams = columns[y].Split(',');
                string caption = colParams[1];
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["FIELD_NAME"].ToString() == colParams[0] && row[CaptionNum].ToString() != "")
                    {
                        caption = row[CaptionNum].ToString();
                        break;
                    }

                }
                retList.Add(caption);
            }
        }
        else
        {
            string[] Captions = this.Captions.Split(';');
            int i = Captions.Length;
            for (int j = 0; j < i; j++)
            {
                retList.Add(Captions[j]);
            }
        }
        return retList;
    }

    private Label[] labels;
    private DropDownList[] dropDownLists;
    private TextBox[] textBoxes;
    private Table table = new Table();
    private void InitializeQueryConditionItem(List<string> arrParam, List<string> arrDataType, int colNum)
    {
        List<string> captions = GetTexts();
        labels = new Label[colNum];
        dropDownLists = new DropDownList[colNum];
        textBoxes = new TextBox[colNum];
        // Create Table
        for (int i = 0; i < colNum; i++)
        {
            // Create Label
            labels[i] = new Label();
            labels[i].ID = "lbl" + arrParam[i];
            labels[i].Text = captions[i];
            TableCell tcLabel = new TableCell();
            tcLabel.Controls.Add(labels[i]);

            // Create DropDownList
            dropDownLists[i] = new DropDownList();
            dropDownLists[i].ID = "ddl" + arrParam[i];
            dropDownLists[i].Items.AddRange(new ListItem[8] { new ListItem("<="), 
                                         new ListItem("<"),
                                         new ListItem("="),
                                         new ListItem("!="),
                                         new ListItem(">"),
                                         new ListItem(">="),
                                         new ListItem("%"),
                                         new ListItem("%%")});
            dropDownLists[i].EnableViewState = true;
            if (string.Compare(arrDataType[i], "system.string", true) == 0)//IgnoreCase
                dropDownLists[i].SelectedIndex = 6;
            else
                dropDownLists[i].SelectedIndex = 2;
            TableCell tcDropDownList = new TableCell();
            tcDropDownList.Controls.Add(dropDownLists[i]);

            // Create TextBox
            textBoxes[i] = new TextBox();
            textBoxes[i].ID = "txt" + arrParam[i];
            TableCell tcTextBox = new TableCell();
            tcTextBox.Controls.Add(textBoxes[i]);

            TableRow tr = new TableRow();
            tr.Cells.Add(tcLabel);
            tr.Cells.Add(tcDropDownList);
            tr.Cells.Add(tcTextBox);
            table.Rows.Add(tr);
        }

        TableRow trEmpty = new TableRow();
        trEmpty.Height = 30;
        table.Rows.Add(trEmpty);

        TableRow trButton = new TableRow();
        TableCell tc1 = new TableCell();
        TableCell tc2 = new TableCell();

        TableCell tcBtn = new TableCell();
        tcBtn.HorizontalAlign = HorizontalAlign.Right;
        String message = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                         "Srvtools",
                                         "WebRefVal",
                                         "ButtonName", true);
        string[] buttons = message.Split(';');
        // Create ButtonOK
        Button btnOK = new Button();
        btnOK.ID = "btnOK";
        btnOK.Text = buttons[2];
        btnOK.Width = 65;
        btnOK.Click += new EventHandler(btnOK_Click);
        // Create ButtonCancel
        Button btnCancel = new Button();
        btnCancel.ID = "btnCancel";
        btnCancel.Text = buttons[3];
        btnCancel.Width = 65;
        btnCancel.Click += new EventHandler(btnCancel_Click);
        //2009/09/09 modify by eva 無?是否為VS2008?是VS2005都可以套用Css
        //if (ConfigurationManager.AppSettings["VS90"] != null && string.Compare(ConfigurationManager.AppSettings["VS90"], "true", true) == 0)//IgnoreCase
        //{
        btnOK.CssClass = "btn_mouseout";
        btnOK.Attributes.Add("onmouseout", "this.className='btn_mouseout';");
        btnOK.Attributes.Add("onmouseover", "this.className='btn_mouseover';");
        btnCancel.CssClass = "btn_mouseout";
        btnCancel.Attributes.Add("onmouseout", "this.className='btn_mouseout';");
        btnCancel.Attributes.Add("onmouseover", "this.className='btn_mouseover';");
        //}
        //End Modify
        tcBtn.Controls.Add(btnOK);
        tcBtn.Controls.Add(btnCancel);

        trButton.Cells.Add(tc1);
        trButton.Cells.Add(tc2);
        trButton.Cells.Add(tcBtn);
        //table.Rows.Add(trButton);

        Table tabButton = new Table();
        tabButton.Rows.Add(trButton);

        this.Panel1.Controls.Add(table);
        this.Panel2.Controls.Add(tabButton);
    }

    public bool IsNvarCharColumn(string columnName)
    {
        if (string.IsNullOrEmpty(Columns))
        {
            return false;
        }

        string[] columns = this.Columns.Split(';');
        for (int i = 0; i < columnName.Length; i++)
        {
            string[] colParams = columns[i].Split(',');
            if (string.Compare(colParams[0], columnName, true) == 0)
            {
                return string.Compare(colParams[3], bool.TrueString, true) == 0;
            }
        }
        return false;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        string sqlcmd;
        if (string.IsNullOrEmpty(this.CommandText))
        {
            String strRemoteModule = GetRemoteName(PagePath, SourceDataSet);
            string strModuleName = strRemoteModule.Substring(0, strRemoteModule.IndexOf('.'));
            string strTableName = strRemoteModule.Substring(strRemoteModule.IndexOf('.') + 1);
            sqlcmd = CliUtils.GetSqlCommandText(strModuleName, strTableName, CliUtils.fCurrentProject);
        }
        else
        {
            sqlcmd = this.CommandText;
        }

        string strQueryCondition = "";
        for (int i = 0; i < colNum; i++)
        {
            if (dropDownLists[i].Text != string.Empty && textBoxes[i].Text != string.Empty)
            {
                string type = dataTypes[i].ToLower();
                string columnName = CliUtils.GetTableNameForColumn(sqlcmd, queryFields[i]);
                string nvarCharMark = IsNvarCharColumn(queryFields[i]) ? "N" : string.Empty;
                if (string.Compare(textBoxes[i].Text.Trim(), "null", true) == 0)//IgnoreCase
                {
                    if (type == "system.uint" || type == "system.uint16" || type == "system.uint32" || type == "system.uint64"
                       || type == "system.int" || type == "system.int16" || type == "system.int32" || type == "system.int64"
                       || type == "system.single" || type == "system.float" || type == "system.double" || type == "system.decimal" || type == "system.datetime")
                    {
                        if (dropDownLists[i].Text.Equals("!="))
                        {
                            strQueryCondition += columnName
                                + "is not null and ";
                        }
                        else
                        {
                            strQueryCondition += columnName
                                + "is null and ";
                        }
                    }
                    else
                    {
                        if (dropDownLists[i].Text.Equals("!="))
                        {
                            strQueryCondition += "(" + columnName
                              + "is not null and" + columnName + "<>'') and ";
                        }
                        else
                        {
                            strQueryCondition += "(" + columnName
                            + "is null or" + columnName + "='') and ";
                        }
                    }
                }
                else
                {
                    if (dropDownLists[i].Text != "%" && dropDownLists[i].Text != "%%")
                    {
                        if (type == "system.uint" || type == "system.uint16" || type == "system.uint32" || type == "system.uint64"
                         || type == "system.int" || type == "system.int16" || type == "system.int32" || type == "system.int64"
                         || type == "system.single" || type == "system.float" || type == "system.double" || type == "system.decimal")
                        {
                            strQueryCondition += columnName
                                + dropDownLists[i].Text + textBoxes[i].Text + " and ";
                        }
                        else if (type == "system.datetime")
                        {
                            string[] DBType = getDBType();
                            if (DBType[0] == "1")
                                strQueryCondition += columnName
                                    + dropDownLists[i].Text + " '" + textBoxes[i].Text + "' and ";
                            else if (DBType[0] == "2")
                                strQueryCondition += columnName
                                    + dropDownLists[i].Text + " '" + textBoxes[i].Text + "' and ";
                            else if (DBType[0] == "3")
                            {
                                string Date = ChangeDate(textBoxes[i].Text);
                                strQueryCondition += columnName
                                    + dropDownLists[i].Text + " to_Date('" + Date + "', 'yyyymmdd') and ";
                            }
                            else if (DBType[0] == "4")
                            {
                                DateTime t;
                                string Date = "";
                                if (DBType[1] == "0")
                                {
                                    if (textBoxes[i].Text.Contains("-") || textBoxes[i].Text.Contains("/"))
                                    {
                                        t = Convert.ToDateTime(textBoxes[i].Text);
                                        Date = t.ToString("yyyyMMddHHmmss");
                                    }
                                    else if (textBoxes[i].Text.Length < 14)
                                        Date = textBoxes[i].Text + "000000";

                                    strQueryCondition += columnName
                                        + dropDownLists[i].Text + " to_Date('" + Date + "', '%Y%m%d%H%M%S') and ";
                                }
                                else if (DBType[1] == "1")
                                {
                                    if (textBoxes[i].Text.Contains("-") || textBoxes[i].Text.Contains("/"))
                                    {
                                        t = Convert.ToDateTime(textBoxes[i].Text);
                                        Date = t.ToString("MM/dd/yyyy");
                                    }
                                    else if (textBoxes[i].Text.Length < 14)
                                        Date = textBoxes[i].Text + "000000";

                                    strQueryCondition += columnName
                                        + dropDownLists[i].Text + " {" + Date + "} and ";
                                }
                            }
                            else if (DBType[0] == "5")
                            {
                                DateTime dt = Convert.ToDateTime(textBoxes[i].Text);
                                strQueryCondition += columnName
                                    + dropDownLists[i].Text + " " + string.Format(" '{0}-{1}-{2}'", dt.Year, dt.Month, dt.Day) + " and ";
                            }
                            else if (DBType[0] == "6")
                            {
                                DateTime dt = Convert.ToDateTime(textBoxes[i].Text);
                                strQueryCondition += columnName
                                    + dropDownLists[i].Text + " "
                                    + String.Format(" to_Date('{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}', '%Y%m%d%H%M%S')",
                                                    dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond)
                                    + " and ";
                            }
                            else if (DBType[0] == "7")
                            {
                                strQueryCondition += columnName
                                    + dropDownLists[i].Text + " '" + textBoxes[i].Text + "' and ";
                            }
                        }
                        else
                        {
                            strQueryCondition += columnName
                                + dropDownLists[i].Text + " " + nvarCharMark + "'" + textBoxes[i].Text.Replace("'", "''") + "' and ";
                        }
                    }
                    else
                    {
                        if (dropDownLists[i].Text == "%")
                        {
                            strQueryCondition += columnName
                                + " like " + nvarCharMark + "'" + textBoxes[i].Text.Replace("'", "''") + "%' and ";
                        }
                        if (dropDownLists[i].Text == "%%")
                        {
                            strQueryCondition += columnName
                                + " like " + nvarCharMark + "'%" + textBoxes[i].Text.Replace("'", "''") + "%' and ";
                        }
                    }
                }
            }
        }
        if (strQueryCondition != string.Empty)
        {
            strQueryCondition = strQueryCondition.Substring(0, strQueryCondition.LastIndexOf(" and "));
        }
        else
        {
            strQueryCondition = " ";
        }
        //string whereitem = (string)this.WhereItem.Clone();
        //whereitem = whereitem.Replace("'", "\\'");
        //string resx = (string)this.Resx.Clone();
        //resx = resx.Replace("\\", "\\\\");
        //string commandtext = (string)this.CommandText.Clone();
        //commandtext = commandtext.Replace("'", "\\'");
        string param = "QueryCondition=" + HttpUtility.UrlEncode(strQueryCondition) + "&Resx=" + HttpUtility.UrlEncode(this.Resx) +
                        "&DataSet=" + HttpUtility.UrlEncode(this.SourceDataSet) + "&ValueField=" + HttpUtility.UrlEncode(this.ValueField) +
                        "&TextField=" + HttpUtility.UrlEncode(this.TextField) + "&WhereItem=" + HttpUtility.UrlEncode(this.WhereItem) +
                        "&DBAlias=" + HttpUtility.UrlEncode(this.DBAlias) + "&CommandText=" + HttpUtility.UrlEncode(this.CommandText) +
                        "&ColumnMatch=" + HttpUtility.UrlEncode(this.ColumnMatch) + "&Columns=" + HttpUtility.UrlEncode(this.Columns) +
                        "&SourceControl=" + HttpUtility.UrlEncode(this.SourceControl) + "&PacketRecords=" + this.PacketRecords.ToString() +
                        "&PagePath=" + this.PagePath;
        //Response.Write("<script language=javascript>window.reload('../InnerPages/frmRefVal.aspx?QueryCondition=" + strQueryCondition + param + "', '', 'width=510,height=450,center=yes');window.close();</script>");
        if (Request.QueryString["RefValID"] != null)
        {
            string id = Request.QueryString["RefValID"];
            this.Session[id] = param;
            Page.Response.Redirect("../InnerPages/frmRefVal.aspx?RefValID=" + id);
        }
        else
        {
            param = QueryStringEncrypt.Encrypt(param);
            Page.Response.Redirect("../InnerPages/frmRefVal.aspx?" + param);
        }
    }

    private string[] getDBType()
    {
        object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB });
        string type = "";
        string odbcType = "";
        if (myRet != null && (int)myRet[0] == 0)
        {
            type = myRet[1].ToString();
            odbcType = myRet[2].ToString();
        }
        return new string[] { type, odbcType };
    }

    private string ChangeDate(string str)
    {
        char[] mark = { '-', '/' };
        string[] temp = str.Split(mark);
        string Date = "";
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].Length < 2)
                temp[i] = '0' + temp[i];
            Date += temp[i];
        }
        return Date;
    }

    private string GetRemoteName(string filePath, string webDataSetID)
    {
        string Rem = "";
        XmlDocument xmlDoc = new XmlDocument();
        string resxFile = string.IsNullOrEmpty(Resx) ? GetResxFile(filePath) : Resx;
        ResXResourceReader reader = new ResXResourceReader(resxFile);
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
                XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSet[@Name='" + webDataSetID + "']");
                if (nWDS != null)
                {
                    XmlNode nRemoteName = nWDS.SelectSingleNode("RemoteName");
                    if (nRemoteName != null)
                        Rem = nRemoteName.InnerText;
                }
            }
        }
        return Rem;
    }

    private string GetResxFile(string FilePath)
    {
        return Server.MapPath(FilePath) + ".vi-VN.resx";
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        //string whereitem = (string)this.WhereItem.Clone();
        //whereitem = whereitem.Replace("'", "\\'");
        //string resx = (string)this.Resx.Clone();
        //resx = resx.Replace("\\", "\\\\");
        //string commandtext = (string)this.CommandText.Clone();
        //commandtext = commandtext.Replace("'", "\\'");
        string param = "QueryCondition=&Resx=" + HttpUtility.UrlEncode(this.Resx) + "&DataSet=" + HttpUtility.UrlEncode(this.SourceDataSet) + "&ValueField=" + HttpUtility.UrlEncode(this.ValueField) + "&TextField=" + HttpUtility.UrlEncode(this.TextField) + "&WhereItem=" + HttpUtility.UrlEncode(this.WhereItem) + "&DBAlias=" + HttpUtility.UrlEncode(this.DBAlias) + "&CommandText=" + HttpUtility.UrlEncode(this.CommandText) + "&ColumnMatch=" + HttpUtility.UrlEncode(this.ColumnMatch) + "&Columns=" + HttpUtility.UrlEncode(this.Columns) + "&SourceControl=" + HttpUtility.UrlEncode(this.SourceControl) + "&PacketRecords=" + this.PacketRecords.ToString();
        //Response.Write("<script language=javascript>window.open('../InnerPages/frmRefVal.aspx=" + param + "', '', 'width=510,height=450,center=yes');window.close();</script>");

        if (Request.QueryString["RefValID"] != null)
        {
            string id = Request.QueryString["RefValID"];
            this.Session[id] = param;
            Page.Response.Redirect("../InnerPages/frmRefVal.aspx?RefValID=" + id);
        }
        else
        {
            param = QueryStringEncrypt.Encrypt(param);
            Page.Response.Redirect("../InnerPages/frmRefVal.aspx?" + param);
        }
    }
}
