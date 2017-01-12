using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;
using System.Resources;
using System.Xml;
using System.Xml.Serialization;
using Srvtools;


public partial class InnerPages_frmClientQuery : System.Web.UI.Page
{
    private Color forecolor;
    private Color textcolor;
    private int gaphorizontal;
    private int gapvertical;
    private string[] arrCaption;
    private string[] arrColumn;
    private string[] arrCodition;
    private string[] arrOperators;
    private string[] arrTextAlign;
    private string[] arrTextWidth;
    private string[] arrText;
    private string[] arrDefaultValue;
    private string[] arrNewLine;
    private string[] arrColumnType;
    private string[] arrDataType;
    private string[] arrTextFont;
    private string[] arrIsNvarchar;

    private string[] arrRefValCMD;
    private string[] arrRefValAlias;
    private string[] arrRefValDSID;
    private string[] arrRefValDM;
    private string[] arrRefValCD;
    private string[] arrRefValTF;
    private string[] arrRefValVF;
    private string[] arrRefValSize;
    private string[] arrRefValColumnMatch;
    private string[] arrRefValColumns;
    private string[] arrRefValWhereItem;

    private string[] arrRefButtonURL;
    private string[] arrRefButtonURLSize;
    private string[] arrRefButtonCaption;

    private string pagePath;
    private string psyPagePath;
    private string dataSourceID;
    private string keepcondition;
    private string remotename;
    private string clientqueryid;
    private int columnNum = 0;

    private WebDataSet[] refValDateSet;
    private WebDataSource[] refValDateSource;
    private WebDataSource[] refValDateSourcecmd;

    SessionRequest sessionRequest;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!SessionRequest.Enable)
        {
            if (this.sessionRequest["Dialog"] != null)
            {
                QueryStringEncrypt.Check(this, new string[] { "&Dialog" });
            }
            else
            {
                QueryStringEncrypt.Check(this, new string[] { "&Dialog" }, false);
            }
        }
        sessionRequest = new SessionRequest(this);
        pagePath = sessionRequest["Pagepath"];
        dataSourceID = sessionRequest["DataSourceID"];
        psyPagePath = sessionRequest["Psypagepath"];
        keepcondition = sessionRequest["Keepcondition"];
        remotename = sessionRequest["RemoteName"];
        clientqueryid = sessionRequest["ClientQueryID"];
        string strfont = sessionRequest["Textfont"];
        string strforecolor = sessionRequest["Labelcolor"];
        string strtextcolor = sessionRequest["Textcolor"];
        string strgaphorizontal = sessionRequest["Gaphorizontal"];
        string strgapvertical = sessionRequest["Gapvertical"];
        string caption = sessionRequest["Caption"];
        string column = sessionRequest["Column"];
        string condition = sessionRequest["Condition"];
        string operators = sessionRequest["Operator"];
        string columntype = sessionRequest["Columntype"];
        string newline = sessionRequest["NewLine"];
        string textwidth = sessionRequest["Textwidth"];
        string textAlign = sessionRequest["Textalign"];
        string text = sessionRequest["Text"];
        string defaultvalue = sessionRequest["Defaultvalue"];
        string isnvarchars = sessionRequest["IsNvarchars"];

        string refvalcmd = sessionRequest["Refvalselcmd"];
        string refvalalias = sessionRequest["Refvalselalias"];
        string refvaldstid = sessionRequest["Refvaldstid"];
        string refvaldm = sessionRequest["Refvaldm"];
        string refvaltf = sessionRequest["Refvaltf"];
        string refvalvf = sessionRequest["Refvalvf"];
        string refvalcd = sessionRequest["Refvalcd"];
        string refvalsize = sessionRequest["RefvalSize"];
        string refvalcolumnmatch = sessionRequest["Refvalcolumnmatch"];
        string refvalcolumns = sessionRequest["Refvalcolumns"];
        string refvalwhereitem = sessionRequest["Refvalwhereitem"];

        string refbuttonurl = sessionRequest["RefButtonurl"];
        string refbuttonurlsize = sessionRequest["RefButtonurlSize"];
        string refbuttoncaption = sessionRequest["RefButtoncaption"];
        string datatype = sessionRequest["Datatype"];

        arrCaption = caption.Split(';');
        arrColumn = column.Split(';');
        arrCodition = condition.Split(';');
        arrColumnType = columntype.Split(';');
        arrOperators = operators.Split(';');
        arrDataType = datatype.Split(';');
        arrNewLine = newline.Split(';');
        arrTextAlign = textAlign.Split(';');
        arrTextWidth = textwidth.Split(';');
        arrText = text.Split(';');
        arrDefaultValue = defaultvalue.Split(';');
        arrIsNvarchar = isnvarchars.Split(';');

        arrRefValCMD = refvalcmd.Split(';');
        arrRefValAlias = refvalalias.Split(';');
        arrRefValDSID = refvaldstid.Split(';');
        arrRefValDM = refvaldm.Split(';');
        arrRefValCD = refvalcd.Split(';');
        arrRefValTF = refvaltf.Split(';');
        arrRefValVF = refvalvf.Split(';');
        arrRefValSize = refvalsize.Split(';');
        arrRefValColumnMatch = refvalcolumnmatch.Split(';');
        arrRefValColumns = refvalcolumns.Split(';');
        arrRefValWhereItem = refvalwhereitem.Split(';');

        arrRefButtonURL = refbuttonurl.Split('@');
        arrRefButtonURLSize = refbuttonurlsize.Split(';');
        arrRefButtonCaption = refbuttoncaption.Split(';');
        arrTextFont = strfont.Split(';');
        columnNum = arrColumn.Length;

        forecolor = Color.FromName(strforecolor);
        textcolor = Color.FromName(strtextcolor);
        gaphorizontal = int.Parse(strgaphorizontal);
        gapvertical = int.Parse(strgapvertical);

        CreatDataSet(arrRefValDSID);
        InitializeQueryConditionItem();

    }

    private List<string> lstDataSetID = new List<string>();
    private void CreatDataSet(string[] datasetid)
    {
        int datasetnum = 0;
        int cmdnum = 0;
        foreach (string strdsid in datasetid)
        {
            if (strdsid != string.Empty)
            {
                datasetnum++;
            }
        }
        foreach (string strcmd in arrRefValCMD)
        {
            if (strcmd != string.Empty)
            {
                cmdnum++;
            }
        }
        refValDateSourcecmd = new WebDataSource[cmdnum];

        if (datasetnum > 0)
        {
            refValDateSet = new WebDataSet[datasetnum];
            refValDateSource = new WebDataSource[datasetnum];
            int intcount = 0;

            for (int i = 0; i < datasetid.Length; i++)
            {
                if (datasetid[i] != string.Empty && (!lstDataSetID.Contains(datasetid[i])))
                {
                    #region Create WebDataSet & WebDataSource
                    XmlDocument xmlDoc = new XmlDocument();
                    string resourceName = psyPagePath + ".vi-VN.resx";
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
                            XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSet[@Name='" + datasetid[i] + "']");
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

                                WebDataSet wds = new WebDataSet();
                                wds.RemoteName = remoteName;
                                wds.PacketRecords = packetRecords;
                                wds.ServerModify = serverModify;
                                wds.Active = true;

                                refValDateSet[intcount] = wds;

                                refValDateSource[intcount] = new WebDataSource();
                                refValDateSource[intcount].DataSource = refValDateSet[intcount];
                                refValDateSource[intcount].DataMember = arrRefValDM[i];
                                refValDateSource[intcount].ID = "refvalds" + intcount.ToString(); ;

                                this.Form.Controls.Add(refValDateSource[intcount]);
                                intcount++;
                                lstDataSetID.Add(datasetid[i]);
                            }
                        }
                    }
                    #endregion
                }
            }
        }
    }

    private void SetTextFont(FontInfo fi)
    {
        fi.Name = arrTextFont[0];
        fi.Size = FontUnit.Parse(arrTextFont[1]);
        fi.Bold = bool.Parse(arrTextFont[2]);
        fi.Italic = bool.Parse(arrTextFont[3]);
        fi.Overline = bool.Parse(arrTextFont[4]);
        fi.Strikeout = bool.Parse(arrTextFont[5]);
        fi.Underline = bool.Parse(arrTextFont[6]);
    }

    private string WhereItemToFileter(string arrwhereitem, string datasourceid) //这个....
    {
        string[] whereitem = arrwhereitem.Split(':');
        WebDataSource wds = this.FindControl(datasourceid) as WebDataSource;
        string strModuleName = wds.RemoteName.Substring(0, wds.RemoteName.IndexOf('.'));
        string strTableName = wds.RemoteName.Substring(wds.RemoteName.IndexOf('.') + 1);
        string tablename = CliUtils.GetTableName(strModuleName, strTableName, CliUtils.fCurrentProject);
        string sqlcmd = CliUtils.GetSqlCommandText(strModuleName, strTableName, CliUtils.fCurrentProject);
        string[] quote = CliUtils.GetDataBaseQuote();
        string filter = "";
        foreach (string wi in whereitem)
        {
            string[] whereitemcontext = wi.Split(',');
            string strOperator = whereitemcontext[0];

            if (filter != "")
            {
                filter += " and ";
            }
            string type = wds.InnerDataSet.Tables[wds.DataMember].Columns[whereitemcontext[1]].DataType.ToString().ToLower();
            if (strOperator != "%" && strOperator != "%%")
            {
                if (type == "system.uint" || type == "system.uint16" || type == "system.uint32"
                   || type == "system.int64" || type == "system.int" || type == "system.int16"
                   || type == "system.int32" || type == "system.uint64" || type == "system.single"
                   || type == "system.double" || type == "system.decimal")
                {
                    filter += CliUtils.GetTableNameForColumn(sqlcmd, whereitemcontext[1], tablename, quote)
                        + strOperator + " " + whereitemcontext[2];
                }
                else
                {
                    filter += CliUtils.GetTableNameForColumn(sqlcmd, whereitemcontext[1], tablename, quote)
                       + strOperator + " '" + whereitemcontext[2] + "'";
                }
            }
            else
            {
                if (strOperator == "%")
                {
                    filter += CliUtils.GetTableNameForColumn(sqlcmd, whereitemcontext[1], tablename, quote)
                        + "like '" + whereitemcontext[2] + "%'";
                }
                if (strOperator == "%%")
                {
                    filter += CliUtils.GetTableNameForColumn(sqlcmd, whereitemcontext[1], tablename, quote)
                       + "like '%" + whereitemcontext[2] + "%'";
                }
            }
        }
        return filter;
    }

    private Label[] labels;
    private TextBox[] textBoxes;
    private WebDropDownList[] dropDownlists;
    private CheckBox[] checkBoxes;
    private WebRefVal[] webRefVals;
    private WebRefButton[] webRefButtons;
    private WebDateTimePicker[] webDateTimePickers;
    private Table table = new Table();
    private void InitializeQueryConditionItem()
    {
        labels = new Label[columnNum];
        dropDownlists = new WebDropDownList[columnNum];
        textBoxes = new TextBox[columnNum];
        checkBoxes = new CheckBox[columnNum];
        webRefVals = new WebRefVal[columnNum];
        webRefButtons = new WebRefButton[columnNum];
        webDateTimePickers = new WebDateTimePicker[columnNum];

        TableRow[] trQuery = new TableRow[columnNum];
        int intRowCount = 0;
        int cmdIndex = 0;
        trQuery[0] = new TableRow();

        table.HorizontalAlign = HorizontalAlign.Center;

        for (int i = 0; i < columnNum; i++)
        {
            TableCell tcLabel = new TableCell();
            TableCell tcQueryText = new TableCell();
            tcLabel.HorizontalAlign = HorizontalAlign.Right;
            tcLabel.VerticalAlign = VerticalAlign.Middle;
            tcQueryText.HorizontalAlign = HorizontalAlign.Left;
            tcQueryText.VerticalAlign = VerticalAlign.Middle;

            //create captionlabel
            labels[i] = new Label();
            labels[i].ID = "lbl" + i.ToString() + arrColumn[i];
            labels[i].Text = arrCaption[i];
            labels[i].ForeColor = forecolor;
            SetTextFont(labels[i].Font);
            tcLabel.Controls.Add(labels[i]);


            //create querytext 
            switch (arrColumnType[i])
            {
                case "ClientQueryComboBoxColumn":
                    {
                        dropDownlists[i] = new WebDropDownList();

                        dropDownlists[i].ID = "ddl" + i.ToString();
                        dropDownlists[i].Width = Unit.Pixel(int.Parse(arrTextWidth[i]));
                        dropDownlists[i].ForeColor = textcolor;

                        int dsidnum = lstDataSetID.IndexOf(arrRefValDSID[i]);
                        if (dsidnum >= 0)
                        {
                            dropDownlists[i].DataSourceID = "refvalds" + dsidnum.ToString();
                            if (arrRefValWhereItem[i] != "")
                            {
                                string filter = WhereItemToFileter(arrRefValWhereItem[i], "refvalds" + dsidnum.ToString());
                                (this.FindControl("refvalds" + dsidnum.ToString()) as WebDataSource).SetWhere(filter);
                            }
                        }
                        else
                        {
                            refValDateSourcecmd[cmdIndex] = new WebDataSource();
                            refValDateSourcecmd[cmdIndex].SelectAlias = arrRefValAlias[i];
                            refValDateSourcecmd[cmdIndex].SelectCommand = arrRefValCMD[i];
                            refValDateSourcecmd[cmdIndex].ID = "refvalcmd" + cmdIndex.ToString();
                            this.Form.Controls.Add(refValDateSourcecmd[cmdIndex]);
                            dropDownlists[i].DataSourceID = "refvalcmd" + cmdIndex.ToString();
                            cmdIndex++;
                        }
                        dropDownlists[i].DataTextField = arrRefValTF[i];
                        dropDownlists[i].DataValueField = arrRefValVF[i];
                        dropDownlists[i].AppendDataBoundItems = true;
                        dropDownlists[i].AutoInsertEmptyData = true;
                        SetTextFont(dropDownlists[i].Font);

                        if (string.Compare(keepcondition, "true", true) == 0)//IgnoreCase
                        {
                            if (arrText[i] != "")
                            {
                                dropDownlists[i].SelectedValue = arrText[i];
                            }
                        }
                        else
                        {
                            try
                            {
                                if (arrDefaultValue[i] != "")
                                {
                                    dropDownlists[i].SelectedValue = arrDefaultValue[i];
                                }
                            }
                            catch
                            { }
                        }

                        tcQueryText.Controls.Add(dropDownlists[i]);
                        break;
                    }
                case "ClientQueryTextBoxColumn":
                    {
                        textBoxes[i] = new TextBox();
                        textBoxes[i].ID = "txt" + i.ToString();
                        textBoxes[i].Width = Unit.Pixel(int.Parse(arrTextWidth[i]));
                        textBoxes[i].ForeColor = textcolor;
                        SetTextFont(textBoxes[i].Font);

                        if (string.Compare(keepcondition, "true", true) == 0)//IgnoreCase
                        {
                            textBoxes[i].Text = arrText[i];
                        }
                        else
                        {
                            textBoxes[i].Text = arrDefaultValue[i];
                        }

                        tcQueryText.Controls.Add(textBoxes[i]);
                        break;
                    }
                case "ClientQueryCheckBoxColumn":
                    {
                        checkBoxes[i] = new CheckBox();
                        checkBoxes[i].ID = "txt" + i.ToString();
                        checkBoxes[i].Text = string.Empty;
                        checkBoxes[i].Width = Unit.Pixel(int.Parse(arrTextWidth[i]));
                        if (string.Compare(keepcondition, "true", true) == 0)//IgnoreCase
                        {
                            if (arrText[i] != "")
                            {
                                checkBoxes[i].Checked = Convert.ToBoolean(arrText[i]);
                            }
                        }
                        else
                        {
                            if (string.Compare(arrDefaultValue[i].Trim(), "true", true) == 0 || string.Compare(arrDefaultValue[i].Trim(), "1") == 0)
                            {
                                checkBoxes[i].Checked = true;
                            }
                            else
                            {
                                checkBoxes[i].Checked = false;
                            }
                        }
                        tcQueryText.Controls.Add(checkBoxes[i]);
                        break;
                    }
                case "ClientQueryRefValColumn":
                    {
                        webRefVals[i] = new WebRefVal();
                        webRefVals[i].ID = "wrf" + i.ToString();
                        webRefVals[i].Width = Unit.Pixel(int.Parse(arrTextWidth[i]));
                        webRefVals[i].ForeColor = textcolor;
                        SetTextFont(webRefVals[i].Font);

                        int dsidnum = lstDataSetID.IndexOf(arrRefValDSID[i]);
                        if (dsidnum >= 0)
                        {
                            webRefVals[i].DataSourceID = "refvalds" + dsidnum.ToString();

                            webRefVals[i].ResxDataSet = arrRefValDSID[i];
                            webRefVals[i].ResxFilePath = psyPagePath + ".vi-VN.resx";
                        }
                        else
                        {
                            refValDateSourcecmd[cmdIndex] = new WebDataSource();
                            refValDateSourcecmd[cmdIndex].SelectAlias = arrRefValAlias[i];
                            refValDateSourcecmd[cmdIndex].SelectCommand = arrRefValCMD[i];
                            refValDateSourcecmd[cmdIndex].ID = "refvalcmd" + cmdIndex.ToString();
                            this.Form.Controls.Add(refValDateSourcecmd[cmdIndex]);
                            webRefVals[i].DataSourceID = "refvalcmd" + cmdIndex.ToString();
                            cmdIndex++;
                        }
                        webRefVals[i].DataTextField = arrRefValTF[i];
                        webRefVals[i].DataValueField = arrRefValVF[i];
                        webRefVals[i].CheckData = Convert.ToBoolean(arrRefValCD[i]);

                        string[] arrsize = arrRefValSize[i].Split(',');
                        if (arrsize.Length == 4)
                        {
                            webRefVals[i].OpenRefHeight = int.Parse(arrsize[0]);
                            webRefVals[i].OpenRefLeft = int.Parse(arrsize[1]);
                            webRefVals[i].OpenRefTop = int.Parse(arrsize[2]);
                            webRefVals[i].OpenRefWidth = int.Parse(arrsize[3]);
                        }

                        #region Add WebRefval other properties
                        if (arrRefValColumnMatch[i] != string.Empty)
                        {
                            string[] columnmatch = arrRefValColumnMatch[i].Split(':');
                            WebColumnMatch[] wcm = new WebColumnMatch[columnmatch.Length];
                            for (int j = 0; j < columnmatch.Length; j++)
                            {
                                string[] columnmatchcontext = columnmatch[j].Split(',');
                                wcm[j] = new WebColumnMatch(columnmatchcontext[1], columnmatchcontext[2], columnmatchcontext[0]);
                                webRefVals[i].ColumnMatch.Add(wcm[j]);
                            }
                        }
                        if (arrRefValColumns[i] != string.Empty)
                        {
                            string[] columns = arrRefValColumns[i].Split(':');
                            WebRefColumn[] wrc = new WebRefColumn[columns.Length];
                            for (int j = 0; j < columns.Length; j++)
                            {
                                string[] columnscontext = columns[j].Split(',');
                                wrc[j] = new WebRefColumn(columnscontext[0], columnscontext[1], int.Parse(columnscontext[2]));
                                if (columnscontext.Length >= 4)
                                {
                                    wrc[j].IsNvarChar = bool.Parse(columnscontext[3]);
                                }
                                webRefVals[i].Columns.Add(wrc[j]);
                            }
                        }
                        if (arrRefValWhereItem[i] != string.Empty)
                        {
                            string[] whereitem = arrRefValWhereItem[i].Split(':');
                            WebWhereItem[] wwi = new WebWhereItem[whereitem.Length];
                            for (int j = 0; j < whereitem.Length; j++)
                            {
                                string[] whereitemcontext = whereitem[j].Split(',');
                                wwi[j] = new WebWhereItem(whereitemcontext[1], whereitemcontext[0], whereitemcontext[2]);
                                webRefVals[i].WhereItem.Add(wwi[j]);
                            }
                        }
                        #endregion

                        if (string.Compare(keepcondition, "true", true) == 0)//IgnoreCase
                        {
                            if (arrText[i] != "")
                            {
                                webRefVals[i].BindingValue = arrText[i];
                            }
                        }
                        else
                        {
                            try
                            {
                                webRefVals[i].BindingValue = arrDefaultValue[i];
                            }
                            catch
                            { }
                        }

                        tcQueryText.Controls.Add(webRefVals[i]);
                        break;
                    }
                case "ClientQueryRefButtonColumn":
                    {
                        textBoxes[i] = new TextBox();
                        textBoxes[i].ID = "txt" + i.ToString();
                        if (string.Compare(keepcondition, "true", true) == 0)//IgnoreCase
                        {
                            textBoxes[i].Text = arrText[i];
                        }
                        else
                        {
                            textBoxes[i].Text = arrDefaultValue[i];
                        }
                        textBoxes[i].Width = Unit.Pixel(int.Parse(arrTextWidth[i]));
                        textBoxes[i].ForeColor = textcolor;
                        SetTextFont(textBoxes[i].Font);
                        webRefButtons[i] = new WebRefButton();
                        webRefButtons[i].ID = "btn" + i.ToString();
                        webRefButtons[i].Caption = arrRefButtonCaption[i];
                        webRefButtons[i].RefURL = arrRefButtonURL[i];

                        string[] arrsize = arrRefButtonURLSize[i].Split(',');
                        if (arrsize.Length == 4)
                        {
                            webRefButtons[i].RefURLHeight = int.Parse(arrsize[0]);
                            webRefButtons[i].RefURLLeft = int.Parse(arrsize[1]);
                            webRefButtons[i].RefURLTop = int.Parse(arrsize[2]);
                            webRefButtons[i].RefURLWidth = int.Parse(arrsize[3]);
                        }

                        MatchControl mc = new MatchControl();
                        mc.ControlID = "txt" + i.ToString();
                        webRefButtons[i].MatchControls.Add(mc);
                        tcQueryText.Controls.Add(textBoxes[i]);
                        tcQueryText.Controls.Add(webRefButtons[i]);
                        break;
                    }

                case "ClientQueryCalendarColumn":
                    {
                        webDateTimePickers[i] = new WebDateTimePicker();
                        webDateTimePickers[i].ID = "wdt" + i.ToString();
                        webDateTimePickers[i].Width = Unit.Pixel(int.Parse(arrTextWidth[i]));
                        webDateTimePickers[i].ForeColor = textcolor;
                        webDateTimePickers[i].DateFormat = dateFormat.ShortDate;
                        webDateTimePickers[i].CheckDate = false;
                        SetTextFont(webDateTimePickers[i].Font);

                        if (string.Compare(keepcondition, "true", true) == 0)//IgnoreCase
                        {
                            webDateTimePickers[i].Text = arrText[i];
                        }
                        else
                        {
                            webDateTimePickers[i].Text = arrDefaultValue[i];
                        }


                        tcQueryText.Controls.Add(webDateTimePickers[i]);
                        break;
                    }
            }
            if (arrTextWidth[i] == "0")
            {
                foreach (Control ctrl in tcQueryText.Controls)
                {
                    ctrl.Visible = false;
                    Panel1.Controls.Add(ctrl);
                }
                continue;
            }


            //is newline
            if (string.Compare(arrNewLine[i], "true", true) == 0)//IgnoreCase
            {
                if (trQuery[intRowCount].Cells.Count > 0)
                {
                    table.Controls.Add(trQuery[intRowCount]);
                    TableRow trEmpty = new TableRow();
                    trEmpty.Height = Unit.Pixel(gapvertical);
                    table.Controls.Add(trEmpty);
                    intRowCount++;
                    trQuery[intRowCount] = new TableRow();
                }

                trQuery[intRowCount].Cells.Add(tcLabel);
                trQuery[intRowCount].Cells.Add(tcQueryText);
            }
            else
            {
                if (trQuery[intRowCount].Cells.Count > 0)
                {
                    TableCell tcEmpty = new TableCell();
                    tcEmpty.Width = Unit.Pixel(gaphorizontal);
                    trQuery[intRowCount].Cells.Add(tcEmpty);
                }
                trQuery[intRowCount].Cells.Add(tcLabel);
                trQuery[intRowCount].Cells.Add(tcQueryText);
            }
        }

        table.Controls.Add(trQuery[intRowCount]);
        TableRow trEmptylast = new TableRow();
        trEmptylast.Height = 15;
        table.Controls.Add(trEmptylast);

        this.Panel1.Controls.Add(table);

        Table tableBtn = new Table();
        TableRow trBtn = new TableRow();
        TableCell tcBtnOk = new TableCell();
        TableCell tcBtnCancel = new TableCell();

        tableBtn.CellSpacing = 15;
        tableBtn.HorizontalAlign = HorizontalAlign.Center;

        String message = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                  "Srvtools",
                                  "WebNavigator",
                                  "ButtonName", true);
        string[] strbuttons = message.Split(';');

        Button btnOK = new Button();
        btnOK.ID = "btnOK";
        //btnOK.Width = 65;
        btnOK.Text = strbuttons[0];
        btnOK.Click += new EventHandler(btnOK_Click);
        tcBtnOk.HorizontalAlign = HorizontalAlign.Right;
        tcBtnOk.Controls.Add(btnOK);

        Button btnCancel = new Button();
        btnCancel.ID = "btnCancel";
        //btnCancel.Width = 65;
        btnCancel.Text = strbuttons[1];
        btnCancel.Click += new EventHandler(btnCancel_Click);
        tcBtnCancel.HorizontalAlign = HorizontalAlign.Left;
        tcBtnCancel.Controls.Add(btnCancel);
        //2009/09/09 modify by eva 無論是否為VS2008還是VS2005都可以套用Css
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
        tcBtnCancel.HorizontalAlign = HorizontalAlign.Left;
        tcBtnCancel.Controls.Add(btnCancel);

        trBtn.Cells.Add(tcBtnOk);
        trBtn.Cells.Add(tcBtnCancel);
        tableBtn.Controls.Add(trBtn);

        this.Panel1.Controls.Add(tableBtn);
    }

    void btnOK_Click(object sender, EventArgs e)
    {
        string[] strText = new string[columnNum];
        string[] queryText = new string[columnNum];

        string querytext = "";

        for (int i = 0; i < columnNum; i++)
        {
            strText[i] = "";
            queryText[i] = "";
            string type = arrDataType[i];
            switch (arrColumnType[i])
            {
                case "ClientQueryComboBoxColumn":
                    {
                        strText[i] = dropDownlists[i].SelectedValue.ToString();
                        queryText[i] = strText[i];
                        break;
                    }
                case "ClientQueryTextBoxColumn":
                    {
                        strText[i] = textBoxes[i].Text;
                        queryText[i] = strText[i];
                        break;
                    }
                case "ClientQueryCheckBoxColumn":
                    {
                        if (checkBoxes[i].Checked)
                        {
                            if (string.Compare(type, "system.string", true) == 0)//IgnoreCase
                            {
                                strText[i] = "Y";
                            }
                            else
                            {
                                strText[i] = "1";
                            }
                        }
                        else
                        {
                            if (string.Compare(type, "system.string", true) == 0)//IgnoreCase
                            {
                                strText[i] = "N";
                            }
                            else
                            {
                                strText[i] = "0";
                            }
                        }
                        queryText[i] = checkBoxes[i].Checked.ToString();
                        break;
                    }
                case "ClientQueryRefButtonColumn":
                    {
                        strText[i] = textBoxes[i].Text;
                        queryText[i] = strText[i];
                        break;
                    }
                case "ClientQueryCalendarColumn":
                    {
                        if (string.Compare(type, "system.string", true) == 0)//IgnoreCase
                        {
                            queryText[i] = webDateTimePickers[i].Text;
                            try
                            {
                                DateTime dttext = DateTime.Parse(webDateTimePickers[i].Text);
                                strText[i] = dttext.Year.ToString("0000")
                                + dttext.Month.ToString("00")
                                + dttext.Day.ToString("00");
                            }
                            catch
                            {
                                strText[i] = "";
                                queryText[i] = strText[i];
                            }

                        }
                        else
                        {
                            strText[i] = webDateTimePickers[i].Text;
                            queryText[i] = strText[i];
                        }
                        break;
                    }
                case "ClientQueryRefValColumn":
                    {
                        strText[i] = webRefVals[i].BindingValue.ToString();
                        queryText[i] = strText[i];
                        break;
                    }

            }
            querytext += queryText[i] + ";";

        }

        if (querytext != string.Empty)
        {
            querytext = querytext.Substring(0, querytext.LastIndexOf(';'));
        }

        string strModuleName = remotename.Substring(0, remotename.IndexOf('.'));
        string strTableName = remotename.Substring(remotename.IndexOf('.') + 1);
        string tablename = CliUtils.GetTableName(strModuleName, strTableName, CliUtils.fCurrentProject);
        string sqlcmd = CliUtils.GetSqlCommandText(strModuleName, strTableName, CliUtils.fCurrentProject);
        string[] quote = CliUtils.GetDataBaseQuote();
        string strQueryCondition = "";
        string strCondition = "";
        string strOperator = "";

        for (int i = 0; i < columnNum; i++)
        {
            strCondition = arrCodition[i];
            strOperator = arrOperators[i];
            if (strQueryCondition == "")
            {
                strCondition = "";
            }
            if (strText[i] != string.Empty)
            {
                strText[i] = strText[i].Replace("'", "''");
                string columnname = CliUtils.GetTableNameForColumn(sqlcmd, arrColumn[i], tablename, quote);
                Type datatype = Type.GetType(arrDataType[i]);
                string valuequote = (datatype == typeof(string) || datatype == typeof(char) || datatype == typeof(Guid))
                           ? "'" : string.Empty;
                bool isNvarchar = bool.Parse(arrIsNvarchar[i]);
                string nvarCharMark = datatype == typeof(string) && isNvarchar ? "N" : string.Empty;
                if (string.Compare(strText[i].Trim(), "null", true) == 0)//IgnoreCase
                {
                    strQueryCondition += " " + strCondition;
                    if (valuequote.Length > 0)
                    {
                        strQueryCondition += "(";
                    }
                    if (strOperator.Equals("!="))
                    {
                        strQueryCondition += columnname + "is not null";
                    }
                    else
                    {
                        strQueryCondition += columnname + "is null";
                    }
                    if (valuequote.Length > 0)
                    {
                        if (strOperator.Equals("!="))
                        {
                            strQueryCondition += " and " + columnname + "<>'')";
                        }
                        else
                        {
                            strQueryCondition += " or " + columnname + "='')";
                        }
                    }
                }
                else
                {
                    if (strOperator == "in")
                    {
                        string[] liststring = strText[i].Split(',');
                        strQueryCondition += " " + strCondition + columnname + " in (";
                        for (int j = 0; j < liststring.Length; j++)
                        {
                            if (j > 0)
                            {
                                strQueryCondition += ",";
                            }
                            strQueryCondition += nvarCharMark + valuequote + liststring[j] + valuequote;
                        }
                        strQueryCondition += ")";
                    }
                    else if (strOperator == "%")
                    {
                        strQueryCondition += " " + strCondition + columnname + "like " + nvarCharMark + "'" + strText[i] + "%'";
                    }
                    else if (strOperator == "%%")
                    {
                        strQueryCondition += " " + strCondition + columnname + "like " + nvarCharMark + "'%" + strText[i] + "%'";
                    }
                    else
                    {
                        if (datatype == typeof(DateTime))
                        {
                            string[] DBType = getDBType();
                            if (DBType[0] == "1")
                                strQueryCondition += " " + strCondition + columnname
                                    + strOperator + " '" + strText[i] + "'";
                            else if (DBType[0] == "2")
                                strQueryCondition += " " + strCondition + columnname
                                    + strOperator + " '" + strText[i] + "'";
                            else if (DBType[0] == "3")
                            {
                                string Date = changeDate(strText[i]);
                                strQueryCondition += " " + strCondition + columnname
                                                    + strOperator + " to_Date('" + Date + "', 'yyyymmdd')";
                            }
                            else if (DBType[0] == "4")
                            {
                                DateTime t;
                                string Date = "";
                                if (DBType[1] == "0")
                                {
                                    if (strText[i].Contains("-") || strText[i].Contains("/"))
                                    {
                                        t = Convert.ToDateTime(strText[i]);
                                        Date = t.ToString("yyyyMMddHHmmss");
                                    }
                                    else if (strText[i].Length < 14)
                                        Date = strText[i] + "000000";

                                    strQueryCondition += " " + strCondition + columnname
                                                        + strOperator + " to_Date('" + Date + "', '%Y%m%d%H%M%S')";
                                }
                                else if (DBType[1] == "1")
                                {
                                    if (strText[i].Contains("-") || strText[i].Contains("/"))
                                    {
                                        t = Convert.ToDateTime(strText[i]);
                                        Date = t.ToString("MM/dd/yyyy");
                                    }
                                    else if (strText[i].Length < 14)
                                        Date = strText[i] + "000000";

                                    strQueryCondition += " " + strCondition + columnname + strOperator + " {" + Date + "}";
                                }
                            }
                            else if (DBType[0] == "5")
                            {
                                DateTime dt = Convert.ToDateTime(strText[i]);
                                strQueryCondition += " " + strCondition + columnname
                                                    + strOperator + string.Format(" '{0}-{1}-{2}'", dt.Year, dt.Month, dt.Day);
                            }
                            else if (DBType[0] == "6")
                            {
                                DateTime dt = Convert.ToDateTime(strText[i]);
                                strQueryCondition += " " + strCondition + columnname
                                                    + strOperator
                                                    + String.Format(" to_Date('{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}', '%Y%m%d%H%M%S')",
                                                    dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
                            }
                            else if (DBType[0] == "7")
                            {
                                strQueryCondition += " " + strCondition + columnname
                                    + strOperator + " '" + strText[i] + "'";
                            }
                        }
                        else
                        {
                            strQueryCondition += " " + strCondition + columnname + strOperator + nvarCharMark + valuequote + strText[i] + valuequote;
                        }
                    }
                }
            }
        }
        string itemparam = sessionRequest["ItemParam"] != null ? HttpUtility.UrlEncode(sessionRequest["ItemParam"]) : string.Empty;
        string url = pagePath + "?Filter=" + HttpUtility.UrlEncode(strQueryCondition) + "&DataSourceID=" + dataSourceID + "&QueryText=" + querytext
                + "&IsQueryBack=1" + "&ClientQueryID=" + clientqueryid + "&ItemParam=" + itemparam;
        if (sessionRequest["MatchControls"] != null && sessionRequest["MatchControls"] != "")
        {
            url += "&MatchControls=" + sessionRequest["MatchControls"];
        }
        if (this.sessionRequest["Dialog"] != null)
        {
            url = url.Replace("'", "\\'");
            string script = string.Format("<script>window.opener.location.href='{0}';window.close();</script>", url);
            this.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script);
        }
        else
        {
            Response.Redirect(url);
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

    void btnCancel_Click(object sender, EventArgs e)
    {
        if (this.sessionRequest["Dialog"] != null)
        {
            this.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), "<script>window.close();</script>");
        }
        else
        {
            string itemparam = sessionRequest["ItemParam"] != null
              ? HttpUtility.UrlEncode(sessionRequest["ItemParam"]) : string.Empty;
            Response.Redirect(pagePath + "?ItemParam=" + itemparam);
        }
    }

}
