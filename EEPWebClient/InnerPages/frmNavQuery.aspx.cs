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
using System.Text;
using Srvtools;
using System.Xml;
using System.Xml.Serialization;
using System.Resources;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class InnerPages_frmNavQuery : System.Web.UI.Page
{
    private int colNum = 0;
    private string[] arrParam;
    private string[] arrDataType;
    private string[] arrCondition;
    private string[] arrIsNvarchar;
    private string pagePath = "", dataSourceID = "", dataSetID = "", dbAlias = "", commandText = "", matchControls = "";
    private string flowFileName = "", listId = "", flowPath = "", whereString = "", navMode = "", flNavMode = "";
    SessionRequest sessionRequest;
    protected void Page_Load(object sender, EventArgs e)
    {
        sessionRequest = new SessionRequest(this);
        pagePath = sessionRequest["PagePath"];
        dataSourceID = sessionRequest["DataSourceID"];
        string Params = sessionRequest["Params"];
        string DataTypes = sessionRequest["DataTypes"];
        string Conditions = sessionRequest["Conditions"];
        if (!string.IsNullOrEmpty(Conditions))
        {
            arrCondition = Conditions.Split(';');
        }
        string IsNvarchars = sessionRequest["IsNvarchars"];
        if (!string.IsNullOrEmpty(IsNvarchars))
        {
            arrIsNvarchar = IsNvarchars.Split(';');
        }


        arrParam = Params.Split(';');
        arrDataType = DataTypes.Split(';');
        colNum = arrParam.Length;
        dataSetID = sessionRequest["DataSetID"];
        dbAlias = sessionRequest["DbAlias"];
        commandText = sessionRequest["CommandText"];
        if (sessionRequest["MatchControls"] != null && sessionRequest["MatchControls"] != "")
            matchControls = sessionRequest["MatchControls"];
        this.InitializeQueryConditionItem(arrParam);

        flowFileName = sessionRequest["FLOWFILENAME"];
        listId = sessionRequest["LISTID"];
        flowPath = sessionRequest["FLOWPATH"];
        whereString = sessionRequest["WHERESTRING"];
        navMode = sessionRequest["NAVMODE"];
        flNavMode = sessionRequest["FLNAVMODE"];
    }

    private List<string> GetDD(string[] fieldName)
    {
        WebDataSource wds = new WebDataSource();
        wds.SelectAlias = dbAlias;
        wds.SelectCommand = commandText;
        string strRemoteModule = GetRemoteName(pagePath, dataSetID);
        wds.RemoteName = strRemoteModule;
        if (!string.IsNullOrEmpty(wds.RemoteName))
        {
            wds.DataMember = wds.RemoteName.Split('.')[1];


        }
        DataSet ds = DBUtils.GetDataDictionary(wds, false);

        string CaptionNum = "CAPTION";
        if (string.Compare(this.sessionRequest["MultiLan"], "true", true) == 0)//IgnoreCase
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
        List<string> DD = new List<string>();
        if (ds.Tables.Count != 0)
        {
            int i = fieldName.Length;
            for (int j = 0; j < i; j++)
            {
                bool DDFound = false;
                int x = ds.Tables[0].Rows.Count;
                for (int y = 0; y < x; y++)
                {
                    if (string.Compare(ds.Tables[0].Rows[y]["FIELD_NAME"].ToString().Trim(), fieldName[j].Trim(), true) == 0//IgnoreCase
                        && ds.Tables[0].Rows[y][CaptionNum].ToString() != "")
                    {
                        DD.Add(ds.Tables[0].Rows[y][CaptionNum].ToString());
                        DDFound = true;
                    }
                }
                if (!DDFound)
                {
                    DD.Add(fieldName[j]);
                }
            }
        }
        return DD;
    }

    private string GetRemoteName(string filePath, string webDataSetID)
    {
        string Rem = "";
        XmlDocument xmlDoc = new XmlDocument();
        string resxFile = GetResxFile(filePath);
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

    private Label[] labels;
    private DropDownList[] dropDownLists;
    private TextBox[] textBoxes;
    private Table table = new Table();
    private void InitializeQueryConditionItem(string[] arrParam)
    {
        List<string> captions = GetDD(arrParam);
        labels = new Label[colNum];
        dropDownLists = new DropDownList[colNum];
        textBoxes = new TextBox[colNum];
        // Create Table
        for (int i = 0; i < colNum; i++)
        {
            // Create Label
            labels[i] = new Label();
            labels[i].ID = "lbl" + i.ToString();
            labels[i].Text = captions.Count > i ? captions[i] : arrParam[i];
            TableCell tcLabel = new TableCell();
            tcLabel.Controls.Add(labels[i]);

            // Create DropDownList
            dropDownLists[i] = new DropDownList();
            dropDownLists[i].ID = "ddl" + i.ToString();
            dropDownLists[i].Items.AddRange(new ListItem[8] { new ListItem("<="), 
                                         new ListItem("<"),
                                         new ListItem("="),
                                         new ListItem("!="),
                                         new ListItem(">"),
                                         new ListItem(">="),
                                         new ListItem("%"),
                                         new ListItem("%%")});
            dropDownLists[i].EnableViewState = true;
            if (arrCondition == null)
            {
                if (string.Compare(arrDataType[i], "system.string", true) == 0)//IgnoreCase
                    dropDownLists[i].SelectedIndex = 6;
                else
                    dropDownLists[i].SelectedIndex = 2;
            }
            else
            {
                dropDownLists[i].SelectedValue = arrCondition[i];
            }
            TableCell tcDropDownList = new TableCell();
            tcDropDownList.Controls.Add(dropDownLists[i]);

            // Create TextBox
            textBoxes[i] = new TextBox();
            textBoxes[i].ID = "txt" + i.ToString();
            TableCell tcTextBox = new TableCell();
            tcTextBox.Controls.Add(textBoxes[i]);

            TableRow tr = new TableRow();
            tr.Cells.Add(tcLabel);
            tr.Cells.Add(tcDropDownList);
            tr.Cells.Add(tcTextBox);
            table.Rows.Add(tr);
        }

        TableRow trEmpty = new TableRow();
        trEmpty.Height = 15;
        table.Rows.Add(trEmpty);

        TableRow trButton = new TableRow();
        TableCell tc1 = new TableCell();
        TableCell tc2 = new TableCell();

        TableCell tcBtn = new TableCell();
        tcBtn.HorizontalAlign = HorizontalAlign.Right;
        String message = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                         "Srvtools",
                                         "WebNavigator",
                                         "ButtonName", true);
        string[] buttons = message.Split(';');
        // Create ButtonOK
        Button btnOK = new Button();
        btnOK.ID = "btnOK";
        btnOK.Text = buttons[0];
        btnOK.Width = 65;
        btnOK.Click += new EventHandler(btnOK_Click);
        // Create ButtonCancel
        Button btnCancel = new Button();
        btnCancel.ID = "btnCancel";
        btnCancel.Text = buttons[1];
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
        //End modify
        tcBtn.Controls.Add(btnOK);
        tcBtn.Controls.Add(btnCancel);

        trButton.Cells.Add(tc1);
        trButton.Cells.Add(tc2);
        trButton.Cells.Add(tcBtn);

        TableRow trEmpty1 = new TableRow();
        trEmpty1.Height = 15;
        table.Rows.Add(trEmpty1);
        //table.Rows.Add(trButton);

        Table tabButton = new Table();
        tabButton.Rows.Add(trButton);

        //Form.Controls.Add(table);
        this.Panel1.Controls.Add(table);
        this.Panel2.Controls.Add(tabButton);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        string remotename = GetRemoteName(pagePath, dataSetID);

        string strModuleName = remotename.Substring(0, remotename.IndexOf('.'));
        string strTableName = remotename.Substring(remotename.IndexOf('.') + 1);
        string tablename = CliUtils.GetTableName(strModuleName, strTableName, CliUtils.fCurrentProject);
        string sqlcmd = CliUtils.GetSqlCommandText(strModuleName, strTableName, CliUtils.fCurrentProject);
        string[] quote = CliUtils.GetDataBaseQuote();


        string strQueryCondition = "";
        for (int i = 0; i < colNum; i++)
        {
            if (dropDownLists[i].Text != string.Empty && textBoxes[i].Text != string.Empty)
            {
                string type = arrDataType[i].ToLower();
                bool isNvarchar = false;
                if (arrIsNvarchar != null)
                {
                    isNvarchar = bool.Parse(arrIsNvarchar[i]);
                }
                string nvarCharMark = type == "system.string" && isNvarchar ? "N" : string.Empty;
                if (string.Compare(textBoxes[i].Text.Trim(), "null", true) == 0)//IgnoreCase
                {
                    if (type == "system.uint" || type == "system.uint16" || type == "system.uint32" || type == "system.uint64"
                       || type == "system.int" || type == "system.int16" || type == "system.int32" || type == "system.int64"
                       || type == "system.single" || type == "system.float" || type == "system.double" || type == "system.decimal" || type == "system.datetime")
                    {
                        if (dropDownLists[i].Text.Equals("!="))
                        {
                            strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                                + "is not null and ";
                        }
                        else
                        {
                            strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                                + "is null and ";
                        }
                    }
                    else
                    {
                        if (dropDownLists[i].Text.Equals("!="))
                        {
                            strQueryCondition += "(" + CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                              + "is not null and" + CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote) + "<>'') and ";
                        }
                        else
                        {
                            strQueryCondition += "(" + CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                                + "is null or" + CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote) + "='') and ";
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
                            strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                                + dropDownLists[i].Text + textBoxes[i].Text + " and ";
                        }
                        else if (type == "system.datetime")
                        {
                            string[] DBType = getDBType();
                            if (DBType[0] == "1")
                                strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                                    + dropDownLists[i].Text + " '" + textBoxes[i].Text + "' and ";
                            else if (DBType[0] == "2")
                                strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                                    + dropDownLists[i].Text + " '" + textBoxes[i].Text + "' and ";
                            else if (DBType[0] == "3")
                            {
                                string Date = changeDate(textBoxes[i].Text);
                                strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
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

                                    strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                                        + dropDownLists[i].Text + " to_Date('" + Date + "', '%Y%m%d%H%M%S') and ";
                                }
                                else if(DBType[1] == "1")
                                {
                                    if (textBoxes[i].Text.Contains("-") || textBoxes[i].Text.Contains("/"))
                                    {
                                        t = Convert.ToDateTime(textBoxes[i].Text);
                                        Date = t.ToString("MM/dd/yyyy");
                                    }
                                    else if (textBoxes[i].Text.Length < 14)
                                        Date = textBoxes[i].Text + "000000";

                                    strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                                        + dropDownLists[i].Text + " {" + Date + "} and ";
                                }
                            }
                            else if (DBType[0] == "5")
                            {
                                DateTime dt = Convert.ToDateTime(textBoxes[i].Text);
                                strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                                    + dropDownLists[i].Text + " " + string.Format(" '{0}-{1}-{2}'", dt.Year, dt.Month, dt.Day) + " and ";
                            }
                            else if (DBType[0] == "6")
                            {
                                DateTime dt = Convert.ToDateTime(textBoxes[i].Text);
                                strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                                    + dropDownLists[i].Text + " "
                                    + String.Format(" to_Date('{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}', '%Y%m%d%H%M%S')",
                                                    dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond)
                                    + " and ";
                            }
                            else if (DBType[0] == "7")
                            {
                                strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                                    + dropDownLists[i].Text + " '" + textBoxes[i].Text + "' and ";
                            }
                        }
                        else
                        {
                            strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                               + dropDownLists[i].Text + nvarCharMark + " '" + textBoxes[i].Text.Replace("'", "''") + "' and ";
                        }
                    }
                    else
                    {
                        if (dropDownLists[i].Text == "%")
                        {
                            strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
                                + " like " + nvarCharMark + "'" + textBoxes[i].Text.Replace("'", "''") + "%' and ";
                        }
                        if (dropDownLists[i].Text == "%%")
                        {
                            strQueryCondition += CliUtils.GetTableNameForColumn(sqlcmd, arrParam[i], tablename, quote)
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
        string strMatchControls = "";
        if (this.matchControls != "")
        {
            strMatchControls = "&MatchControls=" + this.matchControls;
        }
        string strFlow = "&FLOWFILENAME=" + HttpUtility.UrlEncode(flowFileName) + "&LISTID=" + HttpUtility.UrlEncode(listId) + "&FLOWPATH=" + HttpUtility.UrlEncode(flowPath) + "&WHERESTRING=" + HttpUtility.UrlEncode(whereString) + "&NAVMODE=" + navMode + "&FLNAVMODE=" + flNavMode;
        string itemparam = sessionRequest["ItemParam"] != null ? HttpUtility.UrlEncode(sessionRequest["ItemParam"]) : string.Empty;
        string url = pagePath + "?Filter=" + HttpUtility.UrlEncode(strQueryCondition) + "&DataSourceID=" + dataSourceID
            + "&IsQueryBack=1" + "&ItemParam=" + itemparam + strMatchControls + strFlow;
        if (this.sessionRequest["Dialog"] != null)
        {
            url = url.Replace("'", "\\'");
            string script = string.Format("<script>window.opener.location.href='{0}';window.close();</script>", url);
            this.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script);
        }
        else
        {
            Response.Redirect(pagePath + "?Filter=" + HttpUtility.UrlEncode(strQueryCondition) + "&DataSourceID=" + dataSourceID
                + "&IsQueryBack=1" + "&ItemParam=" + itemparam + strMatchControls + strFlow);
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

    private string changeDate(string str)
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

    private void btnCancel_Click(object sender, EventArgs e)
    {
        if (this.sessionRequest["Dialog"] != null)
        {
            this.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), "<script>window.close();</script>");
        }
        else
        {
            string strMatchControls = "";
            if (this.matchControls != "")
            {
                strMatchControls = "&MatchControls=" + this.matchControls;
            }
            string strFlow = "&FLOWFILENAME=" + HttpUtility.UrlEncode(flowFileName) + "&LISTID=" + HttpUtility.UrlEncode(listId) + "&FLOWPATH=" + HttpUtility.UrlEncode(flowPath) + "&WHERESTRING=" + HttpUtility.UrlEncode(whereString) + "&NAVMODE=" + navMode + "&FLNAVMODE=" + flNavMode;
            string itemparam = sessionRequest["ItemParam"] != null ? HttpUtility.UrlEncode(sessionRequest["ItemParam"]) : string.Empty;
            Response.Redirect(pagePath + "?MatchControls" + this.matchControls + strFlow + "&ItemParam=" + itemparam);
        }
    }
}
