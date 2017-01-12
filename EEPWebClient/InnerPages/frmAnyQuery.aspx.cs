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
using System.Xml;
using System.Resources;
using System.IO;

public partial class InnerPages_frmAnyQuery : System.Web.UI.Page
{
    private string[] arrCaption;
    private string[] arrColumn;
    private string[] arrCondition;
    private string[] arrOperators;
    private string[] arrTextWidth;
    private string[] arrText;
    private string[] arrDefaultValue;
    private string[] arrColumnType;
    private string[] arrDataType;
    private string[] arrEnabled;
    private string[] arrAutoSelect;
    private string[] arrItems;

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
    private string webDataSetID;
    private string remotename;
    private string webanyqueryid;
    private int columnNum = 0;
    private String queryColumnMode;
    private String dataMember;
    private String keepcondition;
    private int maxColumnCount;
    private bool autoDisableColumns;
    private String anyQueryID;
    private bool displayAllOperator;
    private bool allowAddQueryField;

    private WebDataSet[] refValDateSet;
    private WebDataSource[] refValDateSource;
    private WebDataSource[] refValDateSourcecmd;
    private WebDataSet wdAnyQuery;
    private WebDataSource wdsAnyQuery;

    private static DataTable allControls;

    SessionRequest sessionRequest;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (allControls == null)
        {
            allControls = new DataTable();

            allControls.Columns.Add(new DataColumn("AutoSelect", typeof(bool)));
            allControls.Columns.Add(new DataColumn("Column", typeof(String)));
            allControls.Columns.Add(new DataColumn("Caption", typeof(String)));
            allControls.Columns.Add(new DataColumn("Condition", typeof(String)));
            allControls.Columns.Add(new DataColumn("DataType", typeof(String)));
            allControls.Columns.Add(new DataColumn("Operators", typeof(String)));
            allControls.Columns.Add(new DataColumn("ColumnType", typeof(String)));
            allControls.Columns.Add(new DataColumn("Enabled", typeof(String)));
            allControls.Columns.Add(new DataColumn("DefaultValue", typeof(String)));
            allControls.Columns.Add(new DataColumn("TextWidth", typeof(String)));

            allControls.Columns.Add(new DataColumn("RefValDSID", typeof(String)));
            allControls.Columns.Add(new DataColumn("RefValWhereItem", typeof(String)));
            allControls.Columns.Add(new DataColumn("RefValTF", typeof(String)));
            allControls.Columns.Add(new DataColumn("RefValVF", typeof(String)));
            allControls.Columns.Add(new DataColumn("RefValAlias", typeof(String)));
            allControls.Columns.Add(new DataColumn("RefValCMD", typeof(String)));
            allControls.Columns.Add(new DataColumn("RefValCD", typeof(String)));
            allControls.Columns.Add(new DataColumn("RefValSize", typeof(String)));
            allControls.Columns.Add(new DataColumn("RefValColumnMatch", typeof(String)));
            allControls.Columns.Add(new DataColumn("RefValColumns", typeof(String)));
            allControls.Columns.Add(new DataColumn("Text", typeof(String)));
            allControls.Columns.Add(new DataColumn("RefButtonCaption", typeof(String)));
            allControls.Columns.Add(new DataColumn("RefButtonURL", typeof(String)));
            allControls.Columns.Add(new DataColumn("RefButtonURLSize", typeof(String)));
        }
        this.Panel2.Visible = false;
        if (!SessionRequest.Enable)
        {
            if (sessionRequest["Dialog"] != null)
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
        remotename = sessionRequest["RemoteName"];
        webanyqueryid = sessionRequest["WebAnyQueryID"];
        keepcondition = sessionRequest["Keepcondition"];
        string caption = sessionRequest["Caption"];
        string column = sessionRequest["Column"];
        string condition = sessionRequest["Condition"];
        string operators = sessionRequest["Operator"];
        string columntype = sessionRequest["Columntype"];
        string textwidth = sessionRequest["Textwidth"];
        string text = sessionRequest["Text"];
        string defaultvalue = sessionRequest["Defaultvalue"];
        string enabled = sessionRequest["Enabled"];
        string autoSelect = sessionRequest["AutoSelect"];
        string items = sessionRequest["Items"];

        String cmd = sessionRequest["Refvalselcmd"].Replace("%2B", "+");
        string refvalcmd = cmd;
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

        webDataSetID = sessionRequest["WebDataSetID"];
        queryColumnMode = sessionRequest["QueryColumnMode"];
        autoDisableColumns = Convert.ToBoolean(sessionRequest["AutoDisableColumns"]);
        dataMember = sessionRequest["DataMember"];
        maxColumnCount = Convert.ToInt16(sessionRequest["MaxColumnCount"]);
        anyQueryID = sessionRequest["AnyQueryID"];
        displayAllOperator = Convert.ToBoolean(sessionRequest["DisplayAllOperator"]);
        allowAddQueryField = Convert.ToBoolean(sessionRequest["AllowAddQueryField"]);

        arrCaption = caption.Split(';');
        arrColumn = column.Split(';');
        arrCondition = condition.Split(';');
        arrColumnType = columntype.Split(';');
        arrOperators = operators.Split(';');
        arrDataType = datatype.Split(';');
        arrTextWidth = textwidth.Split(';');
        arrText = text.Split(';');
        arrDefaultValue = defaultvalue.Split(';');
        arrEnabled = enabled.Split(';');
        arrAutoSelect = autoSelect.Split(';');
        //arrItems = items.Split(';');

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
        columnNum = arrColumn.Length;

        CreatDataSet(webDataSetID);
        CreatDataSet(arrRefValDSID);

        if (!this.IsPostBack)
        {
            allControls.Rows.Clear();
            AddAllColumns();
        }

        InitializeQueryConditionItem();
    }

    private void AddAllColumns()
    {
        for (int i = 0; i < columnNum; i++)
        {
            DataRow drNew = allControls.NewRow();

            drNew["AutoSelect"] = Convert.ToBoolean(arrAutoSelect[i]);
            drNew["Column"] = arrColumn[i];
            drNew["Caption"] = arrCaption[i];
            drNew["DataType"] = arrDataType[i];
            drNew["Condition"] = arrCondition[i];
            drNew["Operators"] = arrOperators[i];
            drNew["ColumnType"] = arrColumnType[i];
            drNew["Enabled"] = arrEnabled[i];
            drNew["DefaultValue"] = arrDefaultValue[i];
            drNew["TextWidth"] = arrTextWidth[i];
            drNew["RefValDSID"] = arrRefValDSID[i];
            drNew["RefValWhereItem"] = arrRefValWhereItem[i];
            drNew["RefValTF"] = arrRefValTF[i];
            drNew["RefValVF"] = arrRefValVF[i];
            drNew["RefValAlias"] = arrRefValAlias[i];
            drNew["RefValCMD"] = arrRefValCMD[i];
            drNew["RefValCD"] = arrRefValCD[i];
            drNew["RefValSize"] = arrRefValSize[i];
            drNew["RefValColumnMatch"] = arrRefValColumnMatch[i];
            drNew["RefValColumns"] = arrRefValColumns[i];
            drNew["Text"] = arrText[i];
            drNew["RefButtonCaption"] = arrRefButtonCaption[i];
            drNew["RefButtonURL"] = arrRefButtonURL[i];
            drNew["RefButtonURLSize"] = arrRefButtonURLSize[i];
           
            allControls.Rows.Add(drNew);
        }
    }

    TableRow[] trQuery;
    int cmdIndex;
    private Table table = new Table();
    private void InitializeQueryConditionItem()
    {
        trQuery = new TableRow[columnNum + 1];

        for (int i = 0; i < allControls.Rows.Count; i++) 
        {
            if (maxColumnCount != -1 && i >= maxColumnCount) break;

            //trQuery[i] = CreateColumn(i, allControls.Rows[i]);

            table.Controls.Add(CreateColumn(i, allControls.Rows[i]));
            TableRow trEmpty = new TableRow();
            table.Controls.Add(trEmpty);
        }

        //TableRow trEmptylast = new TableRow();
        //trEmptylast.Height = 15;
        //table.Controls.Add(trEmptylast);

        this.Panel1.Controls.Add(table);
    }

    private TableRow CreateColumn(int count, DataRow dr)
    {
        CheckBox cbActive = new CheckBox();
        DropDownList ddlCondition = new DropDownList();
        DropDownList ddlColumn = new DropDownList();
        DropDownList dllOperator = new DropDownList();
        TextBox tbValue1 = new TextBox();
        WebDateTimePicker wdtpValue1 = new WebDateTimePicker();
        CheckBox cbValue1 = new CheckBox();
        WebDropDownList wddlValue1 = new WebDropDownList();
        WebRefVal wrvValue1 = new WebRefVal();
        TextBox tbValue1RB = new TextBox();
        WebRefButton wrbValue1 = new WebRefButton();

        TextBox tbValue2 = new TextBox();
        WebDateTimePicker wdtpValue2 = new WebDateTimePicker();
        CheckBox cbValue2 = new CheckBox();
        WebDropDownList wddlValue2 = new WebDropDownList();
        WebRefVal wrvValue2 = new WebRefVal();
        TextBox tbValue2RB = new TextBox();
        WebRefButton wrbValue2 = new WebRefButton();

        TableRow tr = new TableRow();

        TableCell tcActive = new TableCell();
        tcActive.HorizontalAlign = HorizontalAlign.Right;
        tcActive.VerticalAlign = VerticalAlign.Middle;

        //create CheckBoxActive
        cbActive = new CheckBox();
        cbActive.ID = "Web" + count + "AnyQueryActiveCheckBox";
        if (autoDisableColumns == false && !(bool)dr["AutoSelect"])
            cbActive.Checked = false;
        else
            cbActive.Checked = (bool)dr["AutoSelect"];
        cbActive.AutoPostBack = true;
        cbActive.CheckedChanged += new EventHandler(InnerPages_frmAnyQuery_cbActive_CheckedChanged);
        tcActive.Controls.Add(cbActive);

        if (autoDisableColumns == false)
            dr["AutoSelect"] = "True";

        TableCell tcCondition = new TableCell();
        tcCondition.HorizontalAlign = HorizontalAlign.Right;
        tcCondition.VerticalAlign = VerticalAlign.Middle;

        //create ConditionDropDownList
        ddlCondition = new DropDownList();
        ddlCondition.ID = "Web" + count + "AnyQueryConditionDropDownList";
        ddlCondition.Width = 60;
        ddlCondition.Items.Add("AND");
        ddlCondition.Items.Add("OR");
        ddlCondition.Text = dr["Condition"].ToString().ToUpper();
        ddlCondition.Enabled = Convert.ToBoolean(dr["AutoSelect"]);
        tcCondition.Controls.Add(ddlCondition);

        TableCell tcColumn = new TableCell();
        tcColumn.HorizontalAlign = HorizontalAlign.Right;
        tcColumn.VerticalAlign = VerticalAlign.Middle;

        //create ColumnDropDownList
        ddlColumn = new DropDownList();
        ddlColumn.ID = "Web" + count + "AnyQuery" + dr["Column"].ToString();
        alColumn = CreateArrayListColumn();
        for (int j = 0; j < alColumn.Count; j++)
            ddlColumn.Items.Add(alColumn[j].ToString());
        if (ddlColumn.Items.Contains(new ListItem(dr["Caption"].ToString())))
            ddlColumn.Text = dr["Caption"].ToString();
        else
            ddlColumn.Text = dr["Column"].ToString();
        ddlColumn.AutoPostBack = true;
        ddlColumn.Enabled = Convert.ToBoolean(dr["AutoSelect"]);
        ddlColumn.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));
        ddlColumn.SelectedIndexChanged += new EventHandler(InnerPages_frmAnyQuery_ddlColumn_SelectedIndexChanged);
        tcColumn.Controls.Add(ddlColumn);

        TableCell tcOperator = new TableCell();
        tcOperator.HorizontalAlign = HorizontalAlign.Left;
        tcOperator.VerticalAlign = VerticalAlign.Middle;

        //create OperatDropDownList
        dllOperator = new DropDownList();
        dllOperator.ID = "Web" + count + "AnyQueryOperatorDropDownList";
        ArrayList op = new ArrayList();
        op.AddRange(new String[] { "=", "!=", ">", "<", ">=", "<=", "%**", "**%", "%%", "!%%", "<->", "!<->", "IN", "NOT IN" });
        if (!this.displayAllOperator)
        {
            if (dr["DataType"].ToString() == typeof(Char).ToString() || dr["DataType"].ToString() == typeof(String).ToString())
            {
                op.Clear();
                op.AddRange(new String[] { "=", "!=", "%**", "**%", "%%", "!%%", "IN", "NOT IN" });
            }
            else if (dr["DataType"].ToString() == typeof(int).ToString() || dr["DataType"].ToString() == typeof(float).ToString()
                    || dr["DataType"].ToString() == typeof(double).ToString() || dr["DataType"].ToString() == typeof(DateTime).ToString())
            {
                op.Clear();
                op.AddRange(new String[] { "=", "!=", "<", ">", "<=", ">=", "<->", "!<->", "IN", "NOT IN" });
            }
        }
        for (int j = 0; j < op.Count; j++)
            dllOperator.Items.Add(op[j].ToString());
        if (dr["Operators"].ToString() == "%")
            dllOperator.Text = "**%";
        else
            dllOperator.Text = dr["Operators"].ToString();
        dllOperator.Width = 60;
        dllOperator.AutoPostBack = true;
        dllOperator.Enabled = Convert.ToBoolean(dr["AutoSelect"]);
        dllOperator.SelectedIndexChanged += new EventHandler(InnerPages_frmAnyQuery_dllOperator_SelectedIndexChanged);
        tcOperator.Controls.Add(dllOperator);

        TableCell tcValue1 = new TableCell();
        tcValue1.HorizontalAlign = HorizontalAlign.Left;
        tcValue1.VerticalAlign = VerticalAlign.Middle;

        //create Value
        int dsidnum;
        switch (dr["ColumnType"].ToString())
        {
            case "AnyQueryTextBoxColumn":
                tbValue1 = new TextBox();
                tbValue1.ID = "Web" + count + "AnyQueryValue1TextBox" + dr["Enabled"].ToString();
                tbValue1.Text = dr["DefaultValue"].ToString();
                tbValue1.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));
                if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                    tbValue1.Enabled = Convert.ToBoolean(dr["Enabled"].ToString());
                else
                    tbValue1.Enabled = Convert.ToBoolean(dr["AutoSelect"]);
                tcValue1.Controls.Add(tbValue1);
                break;
            case "AnyQueryComboBoxColumn":
                wddlValue1 = new WebDropDownList();
                wddlValue1.ID = "Web" + count + "AnyQueryValue1WebDropDownList" + dr["Enabled"].ToString();
                wddlValue1.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));
                if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                    wddlValue1.Enabled = Convert.ToBoolean(dr["Enabled"].ToString());
                else
                    wddlValue1.Enabled = Convert.ToBoolean(dr["AutoSelect"]);

                dsidnum = lstDataSetID.IndexOf(dr["RefValDSID"].ToString());
                if (dsidnum >= 0)
                {
                    wddlValue1.DataSourceID = "refvalds" + dsidnum.ToString();
                    if (dr["RefValWhereItem"].ToString() != "")
                    {
                        string filter = WhereItemToFileter(dr["RefValWhereItem"].ToString(), "refvalds" + dsidnum.ToString());
                        (this.FindControl("refvalds" + dsidnum.ToString()) as WebDataSource).SetWhere(filter);
                    }
                    wddlValue1.DataTextField = dr["RefValTF"].ToString();
                    wddlValue1.DataValueField = dr["RefValVF"].ToString();
                    wddlValue1.AppendDataBoundItems = true;
                    wddlValue1.AutoInsertEmptyData = true;
                }
                else if (dr["RefValAlias"].ToString() != String.Empty && dr["RefValCMD"].ToString() != String.Empty)
                {
                    refValDateSourcecmd[cmdIndex] = new WebDataSource();
                    refValDateSourcecmd[cmdIndex].SelectAlias = dr["RefValAlias"].ToString();
                    refValDateSourcecmd[cmdIndex].SelectCommand = dr["RefValCMD"].ToString();
                    refValDateSourcecmd[cmdIndex].ID = "refvalcmd" + cmdIndex.ToString();
                    this.Form.Controls.Add(refValDateSourcecmd[cmdIndex]);
                    wddlValue1.DataSourceID = "refvalcmd" + cmdIndex.ToString();
                    cmdIndex++;
                    wddlValue1.DataTextField = dr["RefValTF"].ToString();
                    wddlValue1.DataValueField = dr["RefValVF"].ToString();
                    wddlValue1.AppendDataBoundItems = true;
                    wddlValue1.AutoInsertEmptyData = true;
                }
                else
                {
                    //String[] item = arrItems.Split('!');
                    //foreach (String temp in item)
                    //    wddlValue1.Items.Add(temp);
                }

                if (string.Compare(keepcondition, "true", true) == 0)//IgnoreCase
                {
                    if (dr["Text"].ToString() != "")
                    {
                        wddlValue1.SelectedValue = dr["Text"].ToString();
                    }
                }
                else
                {
                    try
                    {
                        if (dr["DefaultValue"].ToString() != "")
                        {
                            wddlValue1.SelectedValue = dr["DefaultValue"].ToString();
                        }
                    }
                    catch
                    { }
                }
                tcValue1.Controls.Add(wddlValue1);
                break;
            case "AnyQueryCheckBoxColumn":
                cbValue1 = new CheckBox();
                cbValue1.ID = "Web" + count + "AnyQueryValue1CheckBox" + dr["Enabled"].ToString();
                if (dr["DefaultValue"].ToString() == "1")
                    cbValue1.Checked = true;
                cbValue1.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));
                if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                    cbValue1.Enabled = Convert.ToBoolean(dr["Enabled"].ToString());
                else
                    cbValue1.Enabled = Convert.ToBoolean(dr["AutoSelect"]);
                tcValue1.Controls.Add(cbValue1);
                break;
            case "AnyQueryRefValColumn":
                wrvValue1 = new WebRefVal();
                wrvValue1.ID = "Web" + count + "AnyQueryValue1RefVal" + dr["Enabled"].ToString();
                wrvValue1.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));

                dsidnum = lstDataSetID.IndexOf(dr["RefValDSID"].ToString());
                if (dsidnum >= 0)
                {
                    wrvValue1.DataSourceID = "refvalds" + dsidnum.ToString();

                    wrvValue1.ResxDataSet = dr["RefValDSID"].ToString();
                    wrvValue1.ResxFilePath = psyPagePath + ".vi-VN.resx";
                }
                else
                {
                    int refCount = cmdIndex;
                    for (int i = 0; i < refValDateSourcecmd.Length; i++)
                    {
                        if (refValDateSourcecmd[i] != null)
                        {
                            if (refValDateSourcecmd[i].SelectAlias == dr["RefValAlias"].ToString()
                            && refValDateSourcecmd[i].SelectCommand == dr["RefValCMD"].ToString()
                            && refValDateSourcecmd[i].ID == "refvalcmd" + i.ToString())
                            {
                                refCount = i;
                                break;
                            }
                        }
                    }

                    refValDateSourcecmd[refCount] = new WebDataSource();
                    refValDateSourcecmd[refCount].SelectAlias = dr["RefValAlias"].ToString();
                    refValDateSourcecmd[refCount].SelectCommand = dr["RefValCMD"].ToString();
                    refValDateSourcecmd[refCount].ID = "refvalcmd" + refCount.ToString();
                    object obj = this.Form.FindControl(refValDateSourcecmd[refCount].ID);
                    if (obj == null)
                        this.Form.Controls.Add(refValDateSourcecmd[refCount]);
                    wrvValue1.DataSourceID = "refvalcmd" + refCount.ToString();
                    cmdIndex++;
                }
                wrvValue1.DataTextField = dr["RefValTF"].ToString();
                wrvValue1.DataValueField = dr["RefValVF"].ToString();
                wrvValue1.CheckData = Convert.ToBoolean(dr["RefValCD"].ToString());

                string[] aSize = dr["RefValSize"].ToString().Split(',');
                if (aSize.Length == 4)
                {
                    wrvValue1.OpenRefHeight = int.Parse(aSize[0]);
                    wrvValue1.OpenRefLeft = int.Parse(aSize[1]);
                    wrvValue1.OpenRefTop = int.Parse(aSize[2]);
                    wrvValue1.OpenRefWidth = int.Parse(aSize[3]);
                }

                #region Add WebRefval other properties
                if (dr["RefValColumnMatch"].ToString() != string.Empty)
                {
                    string[] columnmatch = dr["RefValColumnMatch"].ToString().Split(':');
                    WebColumnMatch[] wcm = new WebColumnMatch[columnmatch.Length];
                    for (int j = 0; j < columnmatch.Length; j++)
                    {
                        string[] columnmatchcontext = columnmatch[j].Split(',');
                        wcm[j] = new WebColumnMatch(columnmatchcontext[1], columnmatchcontext[2], columnmatchcontext[0]);
                        wrvValue1.ColumnMatch.Add(wcm[j]);
                    }
                }
                if (dr["RefValColumns"].ToString() != string.Empty)
                {
                    string[] columns = dr["RefValColumns"].ToString().Split(':');
                    WebRefColumn[] wrc = new WebRefColumn[columns.Length];
                    for (int j = 0; j < columns.Length; j++)
                    {
                        string[] columnscontext = columns[j].Split(',');
                        wrc[j] = new WebRefColumn(columnscontext[0], columnscontext[1], int.Parse(columnscontext[2]));
                        wrvValue1.Columns.Add(wrc[j]);
                    }
                }
                if (dr["RefValWhereItem"].ToString() != string.Empty)
                {
                    string[] whereitem = dr["RefValWhereItem"].ToString().Split(':');
                    WebWhereItem[] wwi = new WebWhereItem[whereitem.Length];
                    for (int j = 0; j < whereitem.Length; j++)
                    {
                        string[] whereitemcontext = whereitem[j].Split(',');
                        wwi[j] = new WebWhereItem(whereitemcontext[1], whereitemcontext[0], whereitemcontext[2]);
                        wrvValue1.WhereItem.Add(wwi[j]);
                    }
                }
                #endregion

                if (string.Compare(keepcondition, "true", true) == 0)//IgnoreCase
                {
                    if (dr["Text"].ToString() != "")
                    {
                        wrvValue1.BindingValue = dr["Text"].ToString();
                    }
                }
                else
                {
                    try
                    {
                        wrvValue1.BindingValue = dr["DefaultValue"].ToString();
                    }
                    catch
                    { }
                }
                if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                    wrvValue1.ReadOnly = !Convert.ToBoolean(dr["Enabled"].ToString());
                else
                    wrvValue1.ReadOnly = !Convert.ToBoolean(dr["AutoSelect"]);
                tcValue1.Controls.Add(wrvValue1);
                break;
            case "AnyQueryCalendarColumn":
                wdtpValue1 = new WebDateTimePicker();
                wdtpValue1.ID = "Web" + count + "AnyQueryValue1WebDateTimePicker" + dr["Enabled"].ToString();
                wdtpValue1.Text = dr["DefaultValue"].ToString();
                wdtpValue1.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));
                wdtpValue1.CheckDate = false;
                wdtpValue1.DateFormat = dateFormat.ShortDate;
                if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                    wdtpValue1.Enabled = Convert.ToBoolean(dr["Enabled"].ToString());
                else
                    wdtpValue1.Enabled = Convert.ToBoolean(dr["AutoSelect"]);

                tcValue1.Controls.Add(wdtpValue1);
                break;
            case "AnyQueryRefButtonColumn":
                tbValue1RB.ID = "Web" + count + "AnyQueryValue1WebRefButtonTextBox" + dr["Enabled"].ToString();
                tbValue1RB.Text = dr["DefaultValue"].ToString();
                tbValue1RB.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));
                if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                    tbValue1RB.Enabled = Convert.ToBoolean(dr["Enabled"].ToString());
                else
                    tbValue1RB.Enabled = Convert.ToBoolean(dr["AutoSelect"]);

                wrbValue1.ID = "Web" + count + "AnyQueryValue1WebRefButton" + dr["Enabled"].ToString();
                wrbValue1.Caption = dr["RefButtonCaption"].ToString();
                wrbValue1.RefURL = dr["RefButtonURL"].ToString();
                if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                    wrbValue1.Visible = Convert.ToBoolean(dr["Enabled"].ToString());
                else
                    wrbValue1.Visible = Convert.ToBoolean(dr["AutoSelect"]);

                string[] arrsize = dr["RefButtonURLSize"].ToString().Split(',');
                if (arrsize.Length == 4)
                {
                    wrbValue1.RefURLHeight = int.Parse(arrsize[0]);
                    wrbValue1.RefURLLeft = int.Parse(arrsize[1]);
                    wrbValue1.RefURLTop = int.Parse(arrsize[2]);
                    wrbValue1.RefURLWidth = int.Parse(arrsize[3]);
                }

                MatchControl mc = new MatchControl();
                mc.ControlID = "Web" + count + "AnyQueryValue1WebRefButtonTextBox" + dr["Enabled"].ToString();
                wrbValue1.MatchControls.Add(mc);
                tcValue1.Controls.Add(tbValue1RB);
                tcValue1.Controls.Add(wrbValue1);
                break;
        }

        TableCell tcValue2 = new TableCell();
        tcValue2.HorizontalAlign = HorizontalAlign.Left;
        tcValue2.VerticalAlign = VerticalAlign.Middle;
        if (dllOperator.Text == "<->" || dllOperator.Text == "!<->")
        {
            switch (dr["ColumnType"].ToString())
            {
                case "AnyQueryTextBoxColumn":
                    tbValue2 = new TextBox();
                    tbValue2.ID = "Web" + count + "AnyQueryValue2TextBox" + dr["Enabled"].ToString();
                    tbValue2.Text = dr["DefaultValue"].ToString();
                    tbValue2.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));
                    if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                        tbValue2.Enabled = Convert.ToBoolean(dr["Enabled"].ToString());
                    else
                        tbValue2.Enabled = Convert.ToBoolean(dr["AutoSelect"]);

                    tcValue2.Controls.Add(tbValue2);
                    break;
                case "AnyQueryComboBoxColumn":
                    wddlValue2 = new WebDropDownList();
                    wddlValue2.ID = "Web" + count + "AnyQueryValue2WebDropDownList" + dr["Enabled"].ToString();
                    wddlValue2.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));
                    if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                        wddlValue2.Enabled = Convert.ToBoolean(dr["Enabled"].ToString());
                    else
                        wddlValue2.Enabled = Convert.ToBoolean(dr["AutoSelect"]);

                    dsidnum = lstDataSetID.IndexOf(dr["RefValDSID"].ToString());
                    if (dsidnum >= 0)
                    {
                        wddlValue2.DataSourceID = "refvalds" + dsidnum.ToString();
                        if (dr["RefValWhereItem"].ToString() != "")
                        {
                            string filter = WhereItemToFileter(dr["RefValWhereItem"].ToString(), "refvalds" + dsidnum.ToString());
                            (this.FindControl("refvalds" + dsidnum.ToString()) as WebDataSource).SetWhere(filter);
                        }
                        wddlValue2.DataTextField = dr["RefValTF"].ToString();
                        wddlValue2.DataValueField = dr["RefValVF"].ToString();
                        wddlValue2.AppendDataBoundItems = true;
                        wddlValue2.AutoInsertEmptyData = true;
                    }
                    else if (dr["RefValAlias"].ToString() != String.Empty && dr["RefValCMD"].ToString() != String.Empty)
                    {
                        refValDateSourcecmd[cmdIndex] = new WebDataSource();
                        refValDateSourcecmd[cmdIndex].SelectAlias = dr["RefValAlias"].ToString();
                        refValDateSourcecmd[cmdIndex].SelectCommand = dr["RefValCMD"].ToString();
                        refValDateSourcecmd[cmdIndex].ID = "refvalcmd" + cmdIndex.ToString();
                        this.Form.Controls.Add(refValDateSourcecmd[cmdIndex]);
                        wddlValue2.DataSourceID = "refvalcmd" + cmdIndex.ToString();
                        cmdIndex++;
                        wddlValue2.DataTextField = dr["RefValTF"].ToString();
                        wddlValue2.DataValueField = dr["RefValVF"].ToString();
                        wddlValue2.AppendDataBoundItems = true;
                        wddlValue2.AutoInsertEmptyData = true;
                    }
                    else
                    {
                        //String[] item = arrItems.Split('!');
                        //foreach (String temp in item)
                        //    wddlValue2.Items.Add(temp);
                    }

                    if (string.Compare(keepcondition, "true", true) == 0)//IgnoreCase
                    {
                        if (dr["Text"].ToString() != "")
                        {
                            wddlValue2.SelectedValue = dr["Text"].ToString();
                        }
                    }
                    else
                    {
                        try
                        {
                            if (dr["DefaultValue"].ToString() != "")
                            {
                                wddlValue2.SelectedValue = dr["DefaultValue"].ToString();
                            }
                        }
                        catch
                        { }
                    }
                    tcValue2.Controls.Add(wddlValue2);
                    break;
                case "AnyQueryCheckBoxColumn":
                    cbValue2 = new CheckBox();
                    cbValue2.ID = "Web" + count + "AnyQueryValue2CheckBox" + dr["Enabled"].ToString();
                    if (dr["DefaultValue"].ToString() == "1")
                        cbValue2.Checked = true;
                    cbValue2.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));
                    if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                        cbValue2.Enabled = Convert.ToBoolean(dr["Enabled"].ToString());
                    else
                        cbValue2.Enabled = Convert.ToBoolean(dr["AutoSelect"]);
                    tcValue2.Controls.Add(cbValue2);
                    break;
                case "AnyQueryRefValColumn":
                    wrvValue2 = new WebRefVal();
                    wrvValue2.ID = "Web" + count + "AnyQueryValue2RefVal" + dr["Enabled"].ToString();
                    wrvValue2.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));


                    dsidnum = lstDataSetID.IndexOf(dr["RefValDSID"].ToString());
                    if (dsidnum >= 0)
                    {
                        wrvValue2.DataSourceID = "refvalds" + dsidnum.ToString();

                        wrvValue2.ResxDataSet = dr["RefValDSID"].ToString();
                        wrvValue2.ResxFilePath = psyPagePath + ".vi-VN.resx";
                    }
                    else
                    {
                        refValDateSourcecmd[cmdIndex] = new WebDataSource();
                        refValDateSourcecmd[cmdIndex].SelectAlias = dr["RefValAlias"].ToString();
                        refValDateSourcecmd[cmdIndex].SelectCommand = dr["RefValCMD"].ToString();
                        refValDateSourcecmd[cmdIndex].ID = "refvalcmd" + cmdIndex.ToString();
                        this.Form.Controls.Add(refValDateSourcecmd[cmdIndex]);
                        wrvValue2.DataSourceID = "refvalcmd" + cmdIndex.ToString();
                        cmdIndex++;
                    }
                    wrvValue2.DataTextField = dr["RefValTF"].ToString();
                    wrvValue2.DataValueField = dr["RefValVF"].ToString();
                    wrvValue2.CheckData = Convert.ToBoolean(dr["RefValCD"].ToString());

                    string[] aSize = dr["RefValSize"].ToString().Split(',');
                    if (aSize.Length == 4)
                    {
                        wrvValue2.OpenRefHeight = int.Parse(aSize[0]);
                        wrvValue2.OpenRefLeft = int.Parse(aSize[1]);
                        wrvValue2.OpenRefTop = int.Parse(aSize[2]);
                        wrvValue2.OpenRefWidth = int.Parse(aSize[3]);
                    }

                    #region Add WebRefval other properties
                    if (dr["RefValColumnMatch"].ToString() != string.Empty)
                    {
                        string[] columnmatch = dr["RefValColumnMatch"].ToString().Split(':');
                        WebColumnMatch[] wcm = new WebColumnMatch[columnmatch.Length];
                        for (int j = 0; j < columnmatch.Length; j++)
                        {
                            string[] columnmatchcontext = columnmatch[j].Split(',');
                            wcm[j] = new WebColumnMatch(columnmatchcontext[1], columnmatchcontext[2], columnmatchcontext[0]);
                            wrvValue2.ColumnMatch.Add(wcm[j]);
                        }
                    }
                    if (dr["RefValColumns"].ToString() != string.Empty)
                    {
                        string[] columns = dr["RefValColumns"].ToString().Split(':');
                        WebRefColumn[] wrc = new WebRefColumn[columns.Length];
                        for (int j = 0; j < columns.Length; j++)
                        {
                            string[] columnscontext = columns[j].Split(',');
                            wrc[j] = new WebRefColumn(columnscontext[0], columnscontext[1], int.Parse(columnscontext[2]));
                            wrvValue2.Columns.Add(wrc[j]);
                        }
                    }
                    if (dr["RefValWhereItem"].ToString() != string.Empty)
                    {
                        string[] whereitem = dr["RefValWhereItem"].ToString().Split(':');
                        WebWhereItem[] wwi = new WebWhereItem[whereitem.Length];
                        for (int j = 0; j < whereitem.Length; j++)
                        {
                            string[] whereitemcontext = whereitem[j].Split(',');
                            wwi[j] = new WebWhereItem(whereitemcontext[1], whereitemcontext[0], whereitemcontext[2]);
                            wrvValue2.WhereItem.Add(wwi[j]);
                        }
                    }
                    #endregion

                    if (string.Compare(keepcondition, "true", true) == 0)//IgnoreCase
                    {
                        if (dr["Text"].ToString() != "")
                        {
                            wrvValue2.BindingValue = dr["Text"].ToString();
                        }
                    }
                    else
                    {
                        try
                        {
                            wrvValue2.BindingValue = dr["DefaultValue"].ToString();
                        }
                        catch
                        { }
                    }
                    if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                        wrvValue2.ReadOnly = !Convert.ToBoolean(dr["Enabled"].ToString());
                    else
                        wrvValue2.ReadOnly = !Convert.ToBoolean(dr["AutoSelect"]);
                    tcValue2.Controls.Add(wrvValue2);
                    break;
                case "AnyQueryCalendarColumn":
                    wdtpValue2 = new WebDateTimePicker();
                    wdtpValue2.ID = "Web" + count + "AnyQueryValue2WebDateTimePicker" + dr["Enabled"].ToString();
                    wdtpValue2.Text = dr["DefaultValue"].ToString();
                    wdtpValue2.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));
                    wdtpValue2.DateFormat = dateFormat.ShortDate;
                    if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                        wdtpValue2.Enabled = Convert.ToBoolean(dr["Enabled"].ToString());
                    else
                        wdtpValue2.Enabled = Convert.ToBoolean(dr["AutoSelect"]);
                    tcValue2.Controls.Add(wdtpValue2);
                    break;
                case "AnyQueryRefButtonColumn":
                    tbValue2RB.ID = "Web" + count + "AnyQueryValue2WebRefButtonTextBox" + dr["Enabled"].ToString();
                    tbValue2RB.Text = dr["DefaultValue"].ToString();
                    tbValue2RB.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));
                    if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                        tbValue2RB.Enabled = Convert.ToBoolean(dr["Enabled"].ToString());
                    else
                        tbValue2RB.Enabled = Convert.ToBoolean(dr["AutoSelect"]);

                    wrbValue2.ID = "Web" + count + "AnyQueryValue2WebRefButton" + dr["Enabled"].ToString();
                    wrbValue2.Caption = dr["RefButtonCaption"].ToString();
                    wrbValue2.RefURL = dr["RefButtonURL"].ToString();
                    if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                        wrbValue2.Visible = Convert.ToBoolean(dr["Enabled"].ToString());
                    else
                        wrbValue2.Visible = Convert.ToBoolean(dr["AutoSelect"]);

                    string[] arrsize = dr["RefButtonURLSize"].ToString().Split(',');
                    if (arrsize.Length == 4)
                    {
                        wrbValue2.RefURLHeight = int.Parse(arrsize[0]);
                        wrbValue2.RefURLLeft = int.Parse(arrsize[1]);
                        wrbValue2.RefURLTop = int.Parse(arrsize[2]);
                        wrbValue2.RefURLWidth = int.Parse(arrsize[3]);
                    }

                    MatchControl mc = new MatchControl();
                    mc.ControlID = "Web" + count + "AnyQueryValue2WebRefButtonTextBox" + dr["Enabled"].ToString();
                    wrbValue2.MatchControls.Add(mc);
                    tcValue2.Controls.Add(tbValue2RB);
                    tcValue2.Controls.Add(wrbValue2);
                    break;
            }
        }

        tr.Cells.Add(tcActive);
        tr.Cells.Add(tcCondition);
        tr.Cells.Add(tcColumn);
        tr.Cells.Add(tcOperator);
        tr.Cells.Add(tcValue1);
        tr.Cells.Add(tcValue2);

        //if (autoDisableColumns)
        //{
        //    InnerPages_frmAnyQuery_cbActive_CheckedChanged(cbActive, new EventArgs());
        //}

        return tr;
    }

    void InnerPages_frmAnyQuery_ddlColumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;

        String[] temp = (sender as DropDownList).ID.Split(new String[] { "Web", "AnyQuery" }, StringSplitOptions.RemoveEmptyEntries);
        count = Convert.ToInt16(temp[0]);

        //for (int i = 0; i < (sender as DropDownList).ID.Length; i++)
        //{
        //    if (Char.IsDigit((sender as DropDownList).ID[i]))
        //    {
        //        count = count * 10 ^ i + Convert.ToInt16(Char.ToString((sender as DropDownList).ID[i]));
        //    }
        //}

        bool flag = false;
        int x = 0;
        for (; x < arrCaption.Length; x++)
        {
            if (arrCaption[x] == (sender as DropDownList).Text)
            {
                flag = true;
                break;
            }
        }

        DataRow dr = allControls.Rows[count];
        if (flag)
        {
            dr["AutoSelect"] = true;
            dr["Column"] = arrColumn[x];
            dr["Caption"] = arrCaption[x];
            dr["DataType"] = arrDataType[x];
            dr["Condition"] = arrCondition[x];
            dr["Operators"] = arrOperators[x];
            dr["ColumnType"] = arrColumnType[x];
            dr["Enabled"] = arrEnabled[x];
            dr["DefaultValue"] = arrDefaultValue[x];
            dr["TextWidth"] = arrTextWidth[x];
            dr["RefValDSID"] = arrRefValDSID[x];
            dr["RefValWhereItem"] = arrRefValWhereItem[x];
            dr["RefValTF"] = arrRefValTF[x];
            dr["RefValVF"] = arrRefValVF[x];
            dr["RefValAlias"] = arrRefValAlias[x];
            dr["RefValCMD"] = arrRefValCMD[x];
            dr["RefValCD"] = arrRefValCD[x];
            dr["RefValSize"] = arrRefValSize[x];
            dr["RefValColumnMatch"] = arrRefValColumnMatch[x];
            dr["RefValColumns"] = arrRefValColumns[x];
            dr["Text"] = arrText[x];
            dr["RefButtonCaption"] = arrRefButtonCaption[x];
            dr["RefButtonURL"] = arrColumn[x];
            dr["RefButtonURLSize"] = arrRefButtonURLSize[x];
        }
        else
        {
            String dataType = String.Empty;
            String column = String.Empty;
            String caption = String.Empty;
            String width = String.Empty;
            foreach (DataColumn aColumn in wdsAnyQuery.DesignDataSet.Tables[wdsAnyQuery.DataMember].Columns)
            {
                if (GetHeaderText(aColumn.ColumnName) == (sender as DropDownList).Text)
                {
                    column = aColumn.ColumnName;
                    caption = (sender as DropDownList).Text;
                    dataType = aColumn.DataType.ToString();
                    width = (sender as DropDownList).Width.Value.ToString();
                    break;
                }
            }

            dr["AutoSelect"] = true;
            dr["Column"] = column;
            dr["Caption"] = caption;
            dr["DataType"] = dataType;
            dr["Operators"] = "=";
            if (dataType == typeof(DateTime).ToString())
                dr["ColumnType"] = "AnyQueryCalendarColumn";
            else
                dr["ColumnType"] = "AnyQueryTextBoxColumn";
            dr["Enabled"] = true;
            dr["DefaultValue"] = String.Empty;
            dr["TextWidth"] = width;
            dr["RefValDSID"] = String.Empty;
            dr["RefValWhereItem"] = String.Empty;
            dr["RefValTF"] = String.Empty;
            dr["RefValVF"] = String.Empty;
            dr["RefValAlias"] = String.Empty;
            dr["RefValCMD"] = String.Empty;
            dr["RefValCD"] = String.Empty;
            dr["RefValSize"] = String.Empty;
            dr["RefValColumnMatch"] = String.Empty;
            dr["RefValColumns"] = String.Empty;
            dr["Text"] = String.Empty;
            dr["RefButtonCaption"] = String.Empty;
            dr["RefButtonURL"] = String.Empty;
            dr["RefButtonURLSize"] = String.Empty;
        }

        TableRow tr = CreateColumn(count, dr);
        table.Controls.RemoveAt(count * 2);
        table.Controls.AddAt(count * 2, tr);
    }

    void InnerPages_frmAnyQuery_dllOperator_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;
        object cValue1 = null;
        object cValue2 = null;
        ControlCollection ccValue1 = null;
        ControlCollection ccValue2 = null;
        String[] temp = (sender as DropDownList).ID.Split(new String[] { "Web", "AnyQuery" }, StringSplitOptions.RemoveEmptyEntries);
        count = Convert.ToInt16(temp[0]);
        //for (int i = 0; i < (sender as DropDownList).ID.Length; i++)
        //{
        //    if (Char.IsDigit((sender as DropDownList).ID[i]))
        //    {
        //        count = count * Convert.ToInt16(Math.Pow(10, i)) + Convert.ToInt16(Char.ToString((sender as DropDownList).ID[i]));
        //    }
        //}

        foreach (Control c in table.Controls)
        {
            object[] value1 = FindTableControl(c, count, "AnyQueryValue1");
            if (value1 != null)
            {
                cValue1 = value1[0];
                ccValue1 = value1[1] as ControlCollection;
            }

            object[] value2 = FindTableControl(c, count, "AnyQueryValue2");
            if (value2 != null)
            {
                cValue2 = value2[0];
                ccValue2 = value2[1] as ControlCollection;
            }
        }

        TableCell tcValue2 = new TableCell();
        tcValue2.HorizontalAlign = HorizontalAlign.Left;
        tcValue2.VerticalAlign = VerticalAlign.Middle;
        if (cValue1 != null && (sender as DropDownList).Text == "<->" || (sender as DropDownList).Text == "!<->")
        {
            if (cValue2 == null)
            {
                if (cValue1.GetType() == typeof(TextBox))
                {
                    cValue2 = new TextBox();
                    (cValue2 as TextBox).ID = (cValue1 as TextBox).ID.Replace("Value1", "Value2");
                    (cValue2 as TextBox).Text = (cValue1 as TextBox).Text;
                    (cValue2 as TextBox).Width = (cValue1 as TextBox).Width;
                    (cValue2 as TextBox).Enabled = (cValue1 as TextBox).Enabled;
                    tcValue2.Controls.Add((cValue2 as TextBox));
                }
                else if (cValue1.GetType() == typeof(WebDropDownList))
                {
                    cValue2 = new WebDropDownList();
                    (cValue2 as WebDropDownList).ID = (cValue1 as WebDropDownList).ID.Replace("Value1", "Value2");
                    (cValue2 as WebDropDownList).Width = (cValue1 as WebDropDownList).Width;
                    (cValue2 as WebDropDownList).Enabled = (cValue1 as WebDropDownList).Enabled;

                    (cValue2 as WebDropDownList).DataSourceID = (cValue1 as WebDropDownList).DataSourceID;
                    (cValue2 as WebDropDownList).DataTextField = (cValue1 as WebDropDownList).DataTextField;
                    (cValue2 as WebDropDownList).DataValueField = (cValue1 as WebDropDownList).DataValueField;
                    (cValue2 as WebDropDownList).AppendDataBoundItems = true;
                    (cValue2 as WebDropDownList).AutoInsertEmptyData = true;
                    (cValue2 as WebDropDownList).SelectedValue = (cValue1 as WebDropDownList).SelectedValue;

                    tcValue2.Controls.Add((cValue2 as WebDropDownList));

                }
                else if (cValue1.GetType() == typeof(WebRefVal))
                {
                    cValue2 = new WebRefVal();
                    (cValue2 as WebRefVal).ID = (cValue1 as WebRefVal).ID.Replace("Value1", "Value2");
                    (cValue2 as WebRefVal).Width = (cValue1 as WebRefVal).Width;

                    (cValue2 as WebRefVal).DataSourceID = (cValue1 as WebRefVal).DataSourceID;
                    (cValue2 as WebRefVal).ResxDataSet = (cValue1 as WebRefVal).ResxDataSet;
                    (cValue2 as WebRefVal).ResxFilePath = (cValue1 as WebRefVal).ResxFilePath;
                    (cValue2 as WebRefVal).DataTextField = (cValue1 as WebRefVal).DataTextField;
                    (cValue2 as WebRefVal).DataValueField = (cValue1 as WebRefVal).DataValueField;
                    (cValue2 as WebRefVal).CheckData = (cValue1 as WebRefVal).CheckData;

                    (cValue2 as WebRefVal).OpenRefHeight = (cValue1 as WebRefVal).OpenRefHeight;
                    (cValue2 as WebRefVal).OpenRefLeft = (cValue1 as WebRefVal).OpenRefLeft;
                    (cValue2 as WebRefVal).OpenRefTop = (cValue1 as WebRefVal).OpenRefTop;
                    (cValue2 as WebRefVal).OpenRefWidth = (cValue1 as WebRefVal).OpenRefWidth;

                    if ((cValue1 as WebRefVal).ColumnMatch.Count > 0)
                    {
                        for (int j = 0; j < (cValue1 as WebRefVal).ColumnMatch.Count; j++)
                        {
                            (cValue2 as WebRefVal).ColumnMatch[j] = (cValue1 as WebRefVal).ColumnMatch[j];
                        }
                    }
                    if ((cValue1 as WebRefVal).Columns.Count > 0)
                    {
                        for (int j = 0; j < (cValue1 as WebRefVal).Columns.Count; j++)
                        {
                            (cValue2 as WebRefVal).Columns[j] = (cValue1 as WebRefVal).Columns[j];
                        }
                    }
                    if ((cValue1 as WebRefVal).WhereItem.Count > 0)
                    {
                        for (int j = 0; j < (cValue1 as WebRefVal).WhereItem.Count; j++)
                        {
                            (cValue2 as WebRefVal).WhereItem[j] = (cValue1 as WebRefVal).WhereItem[j];
                        }
                    }

                    (cValue2 as WebRefVal).BindingValue = (cValue1 as WebRefVal).BindingValue;
                    (cValue2 as WebRefVal).ReadOnly = (cValue1 as WebRefVal).ReadOnly;
                    tcValue2.Controls.Add((cValue2 as WebRefVal));
                }
                else if (cValue1.GetType() == typeof(WebDateTimePicker))
                {
                    cValue2 = new WebDateTimePicker();
                    (cValue2 as WebDateTimePicker).ID = (cValue1 as WebDateTimePicker).ID.Replace("Value1", "Value2");
                    (cValue2 as WebDateTimePicker).Text = (cValue1 as WebDateTimePicker).Text;
                    (cValue2 as WebDateTimePicker).Width = (cValue1 as WebDateTimePicker).Width;
                    (cValue2 as WebDateTimePicker).Enabled = (cValue1 as WebDateTimePicker).Enabled;
                    (cValue2 as WebDateTimePicker).DateFormat = (cValue1 as WebDateTimePicker).DateFormat;
                    tcValue2.Controls.Add((cValue2 as WebDateTimePicker));
                }
                else if (cValue1.GetType() == typeof(WebRefButton))
                {
                    //tbValue2RB.ID = count + "WebAnyQueryValue2WebRefButtonTextBox" + dr["Enabled"].ToString();
                    //tbValue2RB.Text = dr["DefaultValue"].ToString();
                    //tbValue2RB.Width = Unit.Pixel(int.Parse(dr["TextWidth"].ToString()));
                    //if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                    //    tbValue2RB.Enabled = Convert.ToBoolean(dr["Enabled"].ToString());
                    //else
                    //    tbValue2RB.Enabled = Convert.ToBoolean(dr["AutoSelect"]);

                    //wrbValue2.ID = count + "WebAnyQueryValue1WebRefButton" + dr["Enabled"].ToString();
                    //wrbValue2.Caption = dr["RefButtonCaption"].ToString();
                    //wrbValue2.RefURL = dr["RefButtonURL"].ToString();
                    //if (Convert.ToBoolean(dr["Enabled"].ToString()) == false)
                    //    wrbValue2.Visible = Convert.ToBoolean(dr["Enabled"].ToString());
                    //else
                    //    wrbValue2.Visible = Convert.ToBoolean(dr["AutoSelect"]);

                    //string[] arrsize = dr["RefButtonURLSize"].ToString().Split(',');
                    //if (arrsize.Length == 4)
                    //{
                    //    wrbValue2.RefURLHeight = int.Parse(arrsize[0]);
                    //    wrbValue2.RefURLLeft = int.Parse(arrsize[1]);
                    //    wrbValue2.RefURLTop = int.Parse(arrsize[2]);
                    //    wrbValue2.RefURLWidth = int.Parse(arrsize[3]);
                    //}

                    //MatchControl mc = new MatchControl();
                    //mc.ControlID = count + "WebAnyQueryValue2WebRefButtonTextBox" + dr["Enabled"].ToString();
                    //wrbValue2.MatchControls.Add(mc);
                    //tcValue2.Controls.Add(tbValue2RB);
                    //tcValue2.Controls.Add(wrbValue2);
                }

                allControls.Rows[count]["Operators"] = (sender as DropDownList).Text;
                ccValue1.Add(tcValue2);
            }
        }
        else
        {
            if (cValue2 != null)
            {
                allControls.Rows[count]["Operators"] = (sender as DropDownList).Text;

                foreach (Control c in table.Controls)
                {
                    RemoveTableControl(c, count, "AnyQueryValue2");
                }
            }
        }
    }

    void InnerPages_frmAnyQuery_cbActive_CheckedChanged(object sender, EventArgs e)
    {
        if (this.autoDisableColumns)
        {
            int count = 0;
            String[] temp = (sender as CheckBox).ID.Split(new String[] { "Web", "AnyQuery" }, StringSplitOptions.RemoveEmptyEntries);
            count = Convert.ToInt16(temp[0]);
            //for (int i = 0; i < (sender as CheckBox).ID.Length; i++)
            //{
            //    if (Char.IsDigit((sender as CheckBox).ID[i]))
            //    {
            //        count = count * Convert.ToInt16(Math.Pow(10, i)) + Convert.ToInt16(Char.ToString((sender as CheckBox).ID[i]));
            //    }
            //}

            foreach (Control c in table.Controls)
            {
                EnabledControls(c, (sender as CheckBox).Checked, (sender as CheckBox).ID, count);
            }
        }
    }

    private object[] FindTableControl(Control cc, int count, String name)
    {
        foreach (Control c in cc.Controls)
        {
            if (c.ID != null)
            {
                if (c.ID.StartsWith(count + "Web" + name) || c.ID.StartsWith("Web" + count + name))
                {
                    return new object[] { c, cc.Controls };
                }
                else if (c.HasControls())
                {
                    object[] temp = FindTableControl(c, count, name);
                    if (temp != null)
                        return temp;
                }
            }
            else if (c.HasControls())
            {
                object[] temp = FindTableControl(c, count, name);
                if (temp != null)
                    return temp;
            }
        }
        return null;
    }

    private void RemoveTableControl(Control cc, int count, String name)
    {
        foreach (Control c in cc.Controls)
        {
            if (c.ID != null)
            {
                if (c.ID.StartsWith(count + "Web" + name) || c.ID.StartsWith("Web" + count + name))
                {
                    cc.Controls.Remove(c);
                }
                else if (c.HasControls())
                {
                    RemoveTableControl(c, count, name);
                }
            }
            else if (c.HasControls())
            {
                RemoveTableControl(c, count, name);
            }
        }
    }

    private void EnabledControls(Control cc, bool enabled, String controlID, int count)
    {
        foreach (Control c in cc.Controls)
        {
            if (c.ID != null)
            {
                if (c.ID.StartsWith(count + "WebAnyQueryValue") || c.ID.StartsWith("Web" + count + "AnyQueryValue"))
                {
                    if (c.ID.EndsWith("False"))
                    {
                        if (c is DropDownList)
                        {
                            (c as DropDownList).Enabled = false;
                        }
                        else if (c is WebDateTimePicker)
                        {
                            (c as WebDateTimePicker).Enabled = false;
                        }
                        else if (c is WebRefVal)
                        {
                            (c as WebRefVal).ReadOnly = true;
                        }
                        else if (c is WebRefButton)
                        {
                            (c as WebRefButton).Visible = false;
                        }
                        else if (c is TextBox)
                        {
                            (c as TextBox).Enabled = false;
                        }
                    }
                    else
                    {
                        if (c is DropDownList)
                        {
                            (c as DropDownList).Enabled = enabled;
                        }
                        else if (c is WebRefButton)
                        {
                            (c as WebRefButton).Visible = enabled;
                        }
                        else if (c is WebRefVal)
                        {
                            (c as WebRefVal).ReadOnly = !enabled;
                        }
                        else if (c is WebDateTimePicker)
                        {
                            (c as WebDateTimePicker).Enabled = enabled;
                        }
                        else if (c is TextBox)
                        {
                            (c as TextBox).Enabled = enabled;
                        }
                    }
                }
                else if ((c.ID.StartsWith(count.ToString()) || c.ID.StartsWith("Web" + count.ToString())) && c.ID != controlID)
                {
                    if (c is DropDownList)
                    {
                        (c as DropDownList).Enabled = enabled;
                    }
                    else if (c is WebDateTimePicker)
                    {
                        (c as WebDateTimePicker).Enabled = enabled;
                    }
                    else if (c is WebRefVal)
                    {
                        (c as WebRefVal).ReadOnly = !enabled;
                    }
                    else if (c is WebRefButton)
                    {
                        (c as WebRefButton).Visible = enabled;
                    }
                    else if (c is TextBox)
                    {
                        (c as TextBox).Enabled = enabled;
                    }
                }
                else if (c.HasControls())
                {
                    EnabledControls(c, enabled, controlID, count);
                }
            }
            else if (c.HasControls())
            {
                EnabledControls(c, enabled, controlID, count);
            }
        }
    }

    ArrayList alColumn = null;
    private ArrayList CreateArrayListColumn()
    {
        if (alColumn == null)
        {
            alColumn = new ArrayList();
            if (queryColumnMode == "ByBindingSource")
            {
                foreach (DataColumn column in wdsAnyQuery.DesignDataSet.Tables[wdsAnyQuery.DataMember].Columns)
                {
                    alColumn.Add(GetHeaderText(column.ColumnName));
                }
            }
            else if (queryColumnMode == "ByColumns")
            {
                for (int i = 0; i < arrCaption.Length; i++)
                    alColumn.Add(arrCaption[i]);
            }
        }
        return alColumn;
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
                                refValDateSource[intcount].WebDataSetID = datasetid[i];
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

    private void CreatDataSet(String datasetid)
    {
        wdAnyQuery = new WebDataSet();
        wdsAnyQuery = new WebDataSource();

        if (datasetid != string.Empty && (!lstDataSetID.Contains(datasetid)))
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
                    XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSet[@Name='" + datasetid + "']");
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

                        wdAnyQuery.RemoteName = remoteName;
                        wdAnyQuery.PacketRecords = packetRecords;
                        wdAnyQuery.ServerModify = serverModify;
                        wdAnyQuery.Active = true;


                        wdsAnyQuery = new WebDataSource();
                        wdsAnyQuery.ID = "webanyqueryds";
                        wdsAnyQuery.DesignDataSet = wdAnyQuery.RealDataSet;
                        wdsAnyQuery.DataMember = dataMember;

                        this.Form.Controls.Add(wdsAnyQuery);
                        //lstDataSetID.Add(datasetid);
                    }
                }
            }
            #endregion
        }
    }

    DataSet dsDD = null;
    private DataSet GetDD(String strTableName)
    {
        if (dsDD == null)
        {
            dsDD = new DataSet();
            String tabName = String.Empty;
            String strModuleName = this.wdAnyQuery.RemoteName;
            strModuleName = strModuleName.Substring(0, strModuleName.IndexOf('.'));
            tabName = CliUtils.GetTableName(strModuleName, strTableName, CliUtils.fCurrentProject, "", true);
            if (tabName.Contains("[") && tabName.Contains("]"))
            {
                String[] str = tabName.Split(new char[] { '[', ']' });
                tabName = str[1];
            }
            String strSql = "select * from COLDEF where TABLE_NAME = '" + tabName + "'";
            dsDD = CliUtils.ExecuteSql(strModuleName, strTableName, strSql, true, CliUtils.fCurrentProject);
        }
        return dsDD;
    }

    private String GetHeaderText(String ColName)
    {
        String strTableName = this.wdsAnyQuery.DataMember;
        DataSet ds = GetDD(strTableName);

        String strHeaderText = String.Empty;
        if (ds != null && ds.Tables.Count > 0)
        {
            int i = ds.Tables[strTableName].Rows.Count;
            for (int j = 0; j < i; j++)
            {
                if (ds.Tables[strTableName].Rows[j]["FIELD_NAME"].ToString().ToLower() == ColName.ToLower())
                {
                    strHeaderText = ds.Tables[strTableName].Rows[j]["CAPTION"].ToString();
                }
            }
        }
        if (strHeaderText == String.Empty)
            strHeaderText = ColName;
        return strHeaderText;
    }

    protected void ImageButtonQuery_Click(object sender, ImageClickEventArgs e)
    {
        String whereString = String.Empty;
        String[] remoteName = this.remotename.Split('.');
        String strModuleName = remoteName[0];
        String strTableName = remoteName[1];
        String tablename = CliUtils.GetTableName(strModuleName, strTableName, CliUtils.fCurrentProject);
        String sqlcmd = CliUtils.GetSqlCommandText(strModuleName, strTableName, CliUtils.fCurrentProject);
        String[] quote = CliUtils.GetDataBaseQuote();
        int count = 0;
        for (int i = 0; count < table.Controls.Count - 1; i++, count = i * 2)
        {
            String strWhere = String.Empty;
            foreach (DataColumn dc in wdsAnyQuery.DesignDataSet.Tables[wdsAnyQuery.DataMember].Columns)
            {
                String caption = String.Empty;
                String columnName = String.Empty;
                String operatorMask = String.Empty;
                String value1 = String.Empty;
                String value2 = String.Empty;
                Type valueType = null;
                String condition = String.Empty;
                object realValue1 = null;
                object realValue2 = null;
                bool DateConver = false;

                CheckBox cbActive = table.Controls[count].Controls[0].Controls[0] as CheckBox;
                if (!cbActive.Checked) continue;

                DropDownList ddlColumn = table.Controls[count].Controls[2].Controls[0] as DropDownList;
                caption = ddlColumn.Text;

                //if (dc.ColumnName == arrColumn[i] && caption == arrCaption[i])
                //{
                //    columnName = arrColumn[i];
                //    valueType = dc.DataType;
                //}

                if (columnName == String.Empty)
                {
                    if (GetHeaderText(dc.Caption) != caption)
                        continue;
                    else
                    {
                        valueType = dc.DataType;
                        columnName = dc.ColumnName;
                    }
                }


                DropDownList ddlCondition = table.Controls[count].Controls[1].Controls[0] as DropDownList;
                condition = ddlCondition.Text;

                DropDownList ddlOperator = table.Controls[count].Controls[3].Controls[0] as DropDownList;
                operatorMask = ddlOperator.Text;
                DateConver = false;// GetDataConver(dc.ColumnName);

                Control c = table.Controls[count].Controls[4].Controls[0];
                if (c.ID.StartsWith(i + "WebAnyQueryValue1") || c.ID.StartsWith("Web" + i + "AnyQueryValue1"))
                {
                    if (c.ID.StartsWith("Web" + i + "AnyQueryValue1CheckBox"))
                    {
                        if ((c as CheckBox).Checked)
                            value1 = "Y";
                        else
                            value1 = "N";
                    }
                    else if (c.ID.StartsWith("Web" + i + "AnyQueryValue1RefVal"))
                    {
                        value1 = (c as WebRefVal).BindingValue;
                        realValue1 = (c as WebRefVal).BindingValue;
                    }
                    else if (c.ID.StartsWith("Web" + i + "AnyQueryValue1WebRefvalBox"))
                    {
                        //String[] temp = c.Text.Split(',');
                        //for (int j = 0; j < temp.Length; j++)
                        //{
                        //    if (temp[j] != String.Empty)
                        //    {
                        //        value1 += Mark(valueType, dc.ColumnName, operatorMask, temp[j], DateConver) + ",";
                        //    }
                        //}
                        //if (value1.EndsWith(","))
                        //{
                        //    value1 = value1.Remove(value1.LastIndexOf(","));
                        //}
                    }
                    else if (c.ID.StartsWith("Web" + i + "AnyQueryValue1WebDateTimePicker"))
                    {
                        value1 = (c as WebDateTimePicker).Text;
                        realValue1 = Convert.ToDateTime((c as WebDateTimePicker).Text);
                    }
                    else if (c.ID.StartsWith("Web" + i + "AnyQueryValue1WebDropDownList"))
                    {
                        value1 = (c as WebDropDownList).Text;
                        realValue1 = (c as WebDropDownList).Text;
                    }
                    else
                    {
                        value1 = (c as TextBox).Text;
                        realValue1 = (c as TextBox).Text;
                    }
                }

                if (value1 == String.Empty)
                {
                    //string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ValueNull");
                    //MessageBox.Show(String.Format(message, columnName));
                    return;
                }

                value1 = IsLike(operatorMask, value1);
                if (operatorMask != "IN" && operatorMask != "NOT IN")
                    value1 = Mark(valueType, dc.ColumnName, operatorMask, value1, DateConver);
                else
                    value1 = String.Format("({0})", value1);


                if (operatorMask == "<->" || operatorMask == "!<->")
                {
                    Control c2 = null;
                    foreach (Control con in table.Controls)
                    {
                        object[] tempValue2 = FindTableControl(con, i, "AnyQueryValue2");
                        if (tempValue2 != null)
                        {
                            c2 = tempValue2[0] as Control;
                        }
                    }

                    if (c2 != null)
                    {
                        if (c2.ID.StartsWith("Web" + i + "AnyQueryValue2WebRefvalBox"))
                        {
                            //value2 = (c2 as WebRefvalBox).TextBoxSelectedValue;
                            //realValue2 = (c2 as InfoRefvalBox).TextBoxSelectedValue;
                            //break;
                        }
                        else if (c2.ID.StartsWith("Web" + i + "AnyQueryValue2WebDateTimePicker"))
                        {
                            value2 = (c2 as WebDateTimePicker).Text;
                            realValue2 = Convert.ToDateTime((c2 as WebDateTimePicker).Text);
                        }
                        else if (c.ID.StartsWith("Web" + i + "AnyQueryValue2WebDropDownList"))
                        {
                            value2 = (c2 as WebDropDownList).Text;
                            realValue2 = (c2 as WebDropDownList).Text;
                        }
                        else if (c2.ID.StartsWith("Web" + i + "AnyQueryValue2"))
                        {
                            value2 = (c2 as TextBox).Text;
                            realValue2 = (c2 as TextBox).Text;
                        }

                        value2 = Mark(valueType, dc.ColumnName, operatorMask, value2, DateConver);
                    }
                }

                columnName = CliUtils.GetTableNameForColumn(sqlcmd, columnName, tablename, quote);

                if (caption != String.Empty)
                {
                    if (operatorMask == "<->")
                    {
                        if (realValue2 != null)
                        {
                            bool flag = false;
                            if (realValue2 is DateTime)
                            {
                                if ((DateTime)realValue2 < (DateTime)realValue1)
                                    flag = true;
                            }
                            else if (Convert.ToInt32(realValue2) < Convert.ToInt32(realValue1))
                                flag = true;

                            if (flag)
                            {
                                //string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ValueOverFlow");
                                //MessageBox.Show(message);
                                return;
                            }
                        }
                        strWhere = columnName + ">" + value1 + " AND " + columnName + "<" + value2;
                    }
                    else if (operatorMask == "!<->")
                    {
                        if (realValue2 != null)
                        {
                            bool flag = false;
                            if (realValue2 is DateTime)
                            {
                                if ((DateTime)realValue2 < (DateTime)realValue1)
                                    flag = true;
                            }
                            else if (Convert.ToInt32(realValue2) < Convert.ToInt32(realValue1))
                                flag = true;

                            if (flag)
                            {
                                //string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ValueOverFlow");
                                //MessageBox.Show(message);
                                return;
                            }
                        }
                        strWhere = columnName + "<" + value1 + " OR " + columnName + ">" + value2;
                    }
                    else if (operatorMask == "%**" || operatorMask == "**%" || operatorMask == "%%")
                        strWhere = columnName + " like " + value1;
                    else if (operatorMask == "!%%")
                        strWhere = columnName + " not like " + value1;
                    else
                        strWhere = columnName + " " + operatorMask + " " + value1;

                    strWhere = String.Format("({0})", strWhere);
                    whereString += strWhere + " " + condition + " ";
                    break;
                }
            }
        }
        if (whereString.EndsWith("OR "))
            whereString = whereString.Remove(whereString.LastIndexOf("OR "));
        else if (whereString.EndsWith("AND "))
            whereString = whereString.Remove(whereString.LastIndexOf("AND "));
        //Where = whereString;

        string itemparam = sessionRequest["ItemParam"] != null ? HttpUtility.UrlEncode(sessionRequest["ItemParam"]) : string.Empty;
        string url = pagePath + "?Filter=" + HttpUtility.UrlEncode(whereString) + "&DataSourceID=" + dataSourceID //+ "&QueryText=" + querytext
                + "&IsQueryBack=1" + "&QueryID=" + anyQueryID + "&ItemParam=" + itemparam;
        if (sessionRequest["Dialog"] != null)
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

    private String IsLike(String oprator, String value)
    {
        if (oprator == "%**")
            value = "%" + value;
        else if (oprator == "**%")
            value = value + "%";
        else if (oprator == "%%" || oprator == "!%%")
            value = "%" + value + "%";

        return value;
    }

    private String Mark(Type valueType, String column, String operatorMask, String value, bool dateConvert)
    {
        String strwhere = String.Empty;
        String[] DBType = GetDBType();
        try
        {
            if (value.ToString().Length == 0 || string.Compare(value.ToString(), "null", true) == 0)
            {
                strwhere = value.ToString();
                if (valueType == typeof(String))
                {
                    strwhere = "\'" + strwhere.ToString() + "\'";
                }

            }
            else if (valueType == typeof(DateTime))
            {
                if (dateConvert)
                {
                    switch (DBType[0])
                    {
                        case "1": strwhere = "Convert(varchar," + column + ",111)" + operatorMask + value; break;
                        case "2": strwhere = "Convert(varchar," + column + ",111)" + operatorMask + value; break;
                        case "3": strwhere = column + "{0}to_Date('" + value + "', 'yyyy/mm/dd')"; break;
                        case "4":
                            {
                                if (DBType[1] == "0")
                                {
                                    strwhere = string.Format("to_Date('{0}', '%Y%m%d%H%M%S')", value);
                                }
                                else
                                {
                                    strwhere = value;
                                }
                                break;
                            }
                        case "5": strwhere = "Convert(varchar," + column + ",111)" + operatorMask + value; break;
                    }
                }
                else
                {
                    DateTime dt = (DateTime)Convert.ChangeType(value, typeof(DateTime));//所有时间类型分数据库
                    switch (DBType[0])
                    {
                        case "1":
                            strwhere = string.Format("'{0}-{1}-{2} {3}:{4}:{5}.{6}'", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
                            break;
                        case "2":
                            strwhere = string.Format("'{0}-{1}-{2} {3}:{4}:{5}.{6}'", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
                            break;
                        case "3":
                            strwhere = string.Format("to_Date('{0:0000}{1:00}{2:00}, {3:00}:{4:00}:{5:00} ', 'yyyymmdd hh24:mi:ss')", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                            break;
                        case "4":
                            {
                                if (DBType[1] == "0")
                                {
                                    strwhere = string.Format("to_Date('{0:0000}{1:00}{2:00}000000', '%Y%m%d%H%M%S')", dt.Year, dt.Month, dt.Day);
                                }
                                else
                                {
                                    strwhere = string.Format("{{1:00}/{2:00}/{0:0000}}", dt.Year, dt.Month, dt.Day);
                                }
                                break;
                            }
                        case "5":
                            strwhere = string.Format("'{0}-{1}-{2} {3}:{4}:{5}.{6}'", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
                            break;
                    }
                }
            }
            else if (valueType == typeof(bool))//checkbox redefination
            {
                strwhere = bool.Equals(value, "Y") ? "1" : "0";
            }
            else if (valueType == typeof(Guid))
            {
                try
                {
                    Guid id = new Guid(value.ToString());
                    strwhere = value.ToString();
                }
                catch (FormatException)
                {
                    throw new InvalidCastException(string.Format("Can not convert '{0}' to {1} type", value, valueType.Name));
                }
            }
            else if (valueType == typeof(String))
            {
                strwhere = value.ToString().Replace("'", "''");
                strwhere = "\'" + strwhere.ToString() + "\'";
            }
            else
            {
                Convert.ChangeType(value, valueType);
                strwhere = value.ToString().Replace("'", "''");
            }
        }
        catch (InvalidCastException)
        {
            throw new InvalidCastException(string.Format("Can not convert '{0}' to {1} type", value, valueType.Name));
        }
        return strwhere;
    }

    private string[] GetDBType()
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

    TableRow trNew = new TableRow();
    protected void ImageButtonAdd_Click(object sender, ImageClickEventArgs e)
    {
        if (allowAddQueryField)
        {
            int Count = table.Rows.Count / 2;
            if (maxColumnCount == -1 || Count < maxColumnCount)
            {
                if (queryColumnMode == "ByBindingSource")
                {
                    if (Count >= wdsAnyQuery.DesignDataSet.Tables[wdsAnyQuery.DataMember].Columns.Count)
                        return;
                }
                else if (queryColumnMode == "ByColumns")
                {
                    if (Count >= arrColumn.Length)
                        return;
                }

                Count++;
                DataRow drNew = allControls.NewRow();

                drNew["AutoSelect"] = false;// Convert.ToBoolean(arrAutoSelect[0]);
                drNew["Column"] = arrColumn[0];
                drNew["Caption"] = arrCaption[0];
                drNew["DataType"] = arrDataType[0];
                drNew["Condition"] = arrCondition[0];
                drNew["Operators"] = arrOperators[0];
                drNew["ColumnType"] = arrColumnType[0];
                drNew["Enabled"] = arrEnabled[0];
                drNew["DefaultValue"] = arrDefaultValue[0];
                drNew["TextWidth"] = arrTextWidth[0];
                drNew["RefValDSID"] = arrRefValDSID[0];
                drNew["RefValWhereItem"] = arrRefValWhereItem[0];
                drNew["RefValTF"] = arrRefValTF[0];
                drNew["RefValVF"] = arrRefValVF[0];
                drNew["RefValAlias"] = arrRefValAlias[0];
                drNew["RefValCMD"] = arrRefValCMD[0];
                drNew["RefValCD"] = arrRefValCD[0];
                drNew["RefValSize"] = arrRefValSize[0];
                drNew["RefValColumnMatch"] = arrRefValColumnMatch[0];
                drNew["RefValColumns"] = arrRefValColumns[0];
                drNew["Text"] = arrText[0];
                drNew["RefButtonCaption"] = arrRefButtonCaption[0];
                drNew["RefButtonURL"] = arrColumn[0];
                drNew["RefButtonURLSize"] = arrRefButtonURLSize[0];

                allControls.Rows.Add(drNew);

                trNew = CreateColumn(Count, drNew);
                table.Controls.Add(trNew);
                TableRow trEmpty = new TableRow();
                table.Controls.Add(trEmpty);
            }

            //Page_Load(this, new EventArgs());
            //table.Controls.Clear();
            //this.InitializeQueryConditionItem();
        }
    }

    private static String Status;
    protected void ImageButtonSave_Click(object sender, ImageClickEventArgs e)
    {
        Status = "Save";
        this.Panel2.Visible = true;
        this.Label1.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "SaveLoadMessage", true);
        this.DropDownList1.Visible = false;
        this.TextBox1.Visible = true;
    }

    protected void ImageButtonLoad_Click(object sender, ImageClickEventArgs e)
    {
        Status = "Load";
        this.Panel2.Visible = true;
        this.Label1.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "SaveLoadMessage", true);

        Reload();
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        if (Status == "Save")
        {
            object[] param = new object[1];
            param[0] = anyQueryID;
            object[] myRet = CliUtils.CallMethod("GLModule", "AnyQueryLoadFile", param);

            DataSet dsLoadFile = new DataSet();
            if (myRet != null && myRet[0].ToString() == "0")
                dsLoadFile = myRet[1] as DataSet;

            String fileName = this.TextBox1.Text;

            if (dsLoadFile.Tables[0].Rows.Count > 0 && dsLoadFile.Tables[0].Rows[0][0].ToString() == fileName)
            {
                String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "SaveWarning2", true);
                string script = string.Format("<script>alert('{0}')</script>", message);
                Page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script);
                this.Panel2.Visible = true;
                return;
            }

            if (anyQueryID == String.Empty)
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "QueryIDNull", true);
                string script = string.Format("<script>alert('{0}')</script>", message);
                Page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script);
                this.Panel2.Visible = true;
                return;
            }

            if (fileName != String.Empty)
            {
                String text = String.Empty;
                XmlDocument DBXML = new XmlDocument();
                XmlElement xeMain = DBXML.CreateElement("AnyQuery");

                int count = 0;
                for (int i = 0; count < table.Controls.Count - 1; i++, count = i * 2)
                {
                    String isActive = "False";
                    String caption = String.Empty;
                    String operatorMask = String.Empty;
                    String value1 = String.Empty;
                    String value2 = String.Empty;
                    String valueType = String.Empty;
                    String Width = String.Empty;
                    String selectAlias = String.Empty;
                    String selectCommand = String.Empty;
                    String displayMember = String.Empty;
                    String valueMember = String.Empty;
                    String condition = String.Empty;
                    String formart = String.Empty;
                    String items = String.Empty;
                    String remoteName = String.Empty;
                    String enabled = String.Empty;
                    String whereItem = String.Empty;
                    String columnMatch = String.Empty;
                    String columns = String.Empty;
                    String refvalSize = String.Empty;
                    String refvalCD = String.Empty;
                    String refValDSID = String.Empty;
                    String refButtonCaption = String.Empty;
                    String refButtonURL = String.Empty;
                    String refButtonURLSize = String.Empty;

                    CheckBox cbActive = table.Controls[count].Controls[0].Controls[0] as CheckBox; 
                    if (cbActive.Checked) isActive = "True";

                    DropDownList ddlColumn = table.Controls[count].Controls[2].Controls[0] as DropDownList;
                    caption = ddlColumn.Text;

                    DropDownList ddlCondition = table.Controls[count].Controls[1].Controls[0] as DropDownList;
                    condition = ddlCondition.Text;

                    DropDownList ddlOperator = table.Controls[count].Controls[3].Controls[0] as DropDownList;
                    operatorMask = ddlOperator.Text;

                    Control c = table.Controls[count].Controls[4].Controls[0];
                    if (c.ID.StartsWith(i + "WebAnyQueryValue1") || c.ID.StartsWith("Web" + i + "AnyQueryValue1"))
                    {
                        if (c.ID.StartsWith("Web" + i + "AnyQueryValue1CheckBox"))
                        {
                            if ((c as CheckBox).Checked)
                                value1 = "True";
                            else
                                value1 = "False";
                            valueType = "AnyQueryCheckBoxColumn";
                            Width = (c as CheckBox).Width.Value.ToString();
                            enabled = (c as CheckBox).Enabled.ToString();
                        }
                        else if (c.ID.StartsWith("Web" + i + "AnyQueryValue1RefVal"))
                        {
                            valueType = "AnyQueryRefValColumn";
                            value1 = (c as WebRefVal).BindingValue;
                            Width = (c as WebRefVal).Width.Value.ToString();
                            enabled = (c as WebRefVal).Enabled.ToString();

                            WebDataSource wdsRefVal = FindControl((c as WebRefVal).DataSourceID) as WebDataSource;
                            refValDSID = wdsRefVal.WebDataSetID;
                            String refValDataSourceID = wdsRefVal.ID;
                            displayMember = (c as WebRefVal).DataTextField;
                            valueMember = (c as WebRefVal).DataValueField;
                            if ((c as WebRefVal).WhereItem.Count > 0)
                            {
                                foreach (WebWhereItem wwi in (c as WebRefVal).WhereItem)
                                {
                                    whereItem += wwi.Condition + "," + wwi.FieldName + "," + wwi.Value + ":";

                                }
                                whereItem = whereItem.Substring(0, whereItem.LastIndexOf(':'));
                            }

                            if ((c as WebRefVal).ColumnMatch.Count > 0)
                            {
                                foreach (WebColumnMatch wcm in (c as WebRefVal).ColumnMatch)
                                {
                                    columnMatch += wcm.DestControlID + "," + wcm.SrcField + "," + wcm.SrcGetValue + ":";

                                }
                                columnMatch = columnMatch.Substring(0, columnMatch.LastIndexOf(':'));
                            }

                            if ((c as WebRefVal).Columns.Count > 0)
                            {
                                foreach (WebRefColumn wrc in (c as WebRefVal).Columns)
                                {
                                    columns += wrc.ColumnName + "," + wrc.HeadText + "," + wrc.Width.ToString() + ":";

                                }
                                columns = columns.Substring(0, columns.LastIndexOf(':'));
                            }
                            refvalSize = (c as WebRefVal).OpenRefHeight.ToString() + "," + (c as WebRefVal).OpenRefLeft.ToString() + "," + (c as WebRefVal).OpenRefTop.ToString() + "," + (c as WebRefVal).OpenRefWidth.ToString();
                            refvalCD = (c as WebRefVal).CheckData.ToString();

                            WebDataSource refValDS = this.Page.FindControl(refValDataSourceID) as WebDataSource;
                            selectAlias = refValDS.SelectAlias;
                            selectCommand = refValDS.SelectCommand;
                        }
                        else if (c.ID.StartsWith("Web" + i + "AnyQueryValue1WebRefButtonTextBox"))
                        {
                            valueType = "AnyQueryRefButtonColumn";
                            value1 = (c as TextBox).Text;
                            Width = (c as TextBox).Width.Value.ToString(); ;
                            enabled = (c as TextBox).Enabled.ToString();
                        }
                        else if (c.ID.StartsWith("Web" + i + "AnyQueryValue1WebRefButton"))
                        {
                            valueType = "AnyQueryRefButtonColumn";
                            refButtonCaption = (c as WebRefButton).Caption;
                            refButtonURL = (c as WebRefButton).RefURL;
                            refButtonURLSize = (c as WebRefButton).RefURLHeight + "," + (c as WebRefButton).RefURLLeft + "," + (c as WebRefButton).RefURLTop + "," + (c as WebRefButton).RefURLWidth;
                        }
                        else if (c.ID.StartsWith("Web" + i + "AnyQueryValue1WebDateTimePicker"))
                        {
                            valueType = "AnyQueryCalendarColumn";
                            value1 = (c as WebDateTimePicker).Text;
                            Width = (c as WebDateTimePicker).Width.Value.ToString();
                            enabled = (c as WebDateTimePicker).Enabled.ToString();
                        }
                        else if (c.ID.StartsWith("Web" + i + "AnyQueryValue1WebDropDownList"))
                        {
                            valueType = "AnyQueryComboBoxColumn";
                            value1 = (c as WebDropDownList).Text;
                            Width = (c as WebDropDownList).Width.Value.ToString();
                            enabled = (c as WebDropDownList).Enabled.ToString();

                            refValDSID = (c as WebDropDownList).DataSourceID;
                            displayMember = (c as WebDropDownList).DataTextField;
                            valueMember = (c as WebDropDownList).DataValueField;

                            WebDataSource refValDS = this.Page.FindControl(refValDSID) as WebDataSource;
                            selectAlias = refValDS.SelectAlias;
                            selectCommand = refValDS.SelectCommand;
                        }
                        else if (c.ID.StartsWith("Web" + i + "AnyQueryValue1TextBox"))
                        {
                            valueType = "AnyQueryTextBoxColumn";
                            value1 = (c as TextBox).Text;
                            Width = (c as TextBox).Width.Value.ToString(); ;
                            enabled = (c as TextBox).Enabled.ToString();
                        }
                    }

                    if (table.Controls[count].Controls[0].Controls.Count > 5)
                    {
                        Control c2 = table.Controls[count].Controls[5].Controls[0];
                        if (c2.ID.StartsWith("Web" + i + "AnyQueryValue2RefVal"))
                        {
                            value2 = (c2 as WebRefVal).BindingValue;
                        }
                        else if (c2.ID.StartsWith("Web" + i + "AnyQueryValue2WebDateTimePicker"))
                        {
                            value2 = (c2 as WebDateTimePicker).Text;
                        }
                        else if (c2.ID.StartsWith("Web" + i + "AnyQueryValue2TextBox"))
                        {
                            value2 = (c2 as TextBox).Text;
                        }
                        else if (c.ID.StartsWith("Web" + i + "AnyQueryValue1WebDropDownList"))
                        {
                            value2 = (c2 as WebDropDownList).Text;
                        }
                    }

                    XmlElement elem = DBXML.CreateElement("String");
                    XmlAttribute attr = DBXML.CreateAttribute("IsActive");
                    attr.Value = isActive;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("Caption");
                    attr.Value = caption;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("Operator");
                    attr.Value = operatorMask;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("ValueType");
                    attr.Value = valueType;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("Width");
                    attr.Value = Width;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("Value1");
                    attr.Value = value1;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("Value2");
                    attr.Value = value2;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("SelectAlias");
                    attr.Value = selectAlias;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("SelectCommand");
                    attr.Value = selectCommand;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("DisplayMember");
                    attr.Value = displayMember;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("ValueMember");
                    attr.Value = valueMember;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("Condition");
                    attr.Value = condition;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("Items");
                    attr.Value = items;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("RemoteName");
                    attr.Value = remoteName;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("Enabled");
                    attr.Value = enabled;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("WhereItem");
                    attr.Value = whereItem;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("ColumnMatch");
                    attr.Value = columnMatch;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("Columns");
                    attr.Value = columns;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("RefvalSize");
                    attr.Value = refvalSize;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("RefvalCD");
                    attr.Value = refvalCD;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("RefValDSID");
                    attr.Value = refValDSID;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("RefButtonCaption");
                    attr.Value = refButtonCaption;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("RefButtonURL");
                    attr.Value = refButtonURL;
                    elem.Attributes.Append(attr);

                    attr = DBXML.CreateAttribute("RefButtonURLSize");
                    attr.Value = refButtonURLSize;
                    elem.Attributes.Append(attr);

                    xeMain.AppendChild(elem);
                }

                object[] param1 = new object[4];
                param1[0] = anyQueryID;
                param1[1] = fileName;
                param1[2] = xeMain.OuterXml;
                param1[3] = GetTableName();
                object[] myRet1 = CliUtils.CallMethod("GLModule", "AnyQuerySave", param1);
                if (myRet1 != null && myRet1[0].ToString() == "0")
                {
                    string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "SaveSuccess", true);
                    string script = string.Format("<script>alert('{0}')</script>", message);
                    Page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script);
                }
            }
        }
        else if (Status == "Load")
        {
            allControls.Rows.Clear();

            if (anyQueryID == String.Empty)
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "QueryIDNull", true);
                string script = string.Format("<script>alert('{0}')</script>", message);
                Page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script);
                this.Panel2.Visible = true;
                return;
            }

            String fileName = this.DropDownList1.Text;
            if (fileName == String.Empty)
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "QueryIDNull", true);
                string script = string.Format("<script>alert('{0}')</script>", message);
                Page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script);
                this.Panel2.Visible = true;
                return;
            }
            if (fileName != String.Empty)
            {
                String templateID = fileName;
                String text = String.Empty;
                String tableName = String.Empty;
                object[] param = new object[2];
                param[0] = anyQueryID;
                param[1] = templateID;
                object[] myRet = CliUtils.CallMethod("GLModule", "AnyQueryLoad", param);
                if (myRet != null && myRet[0].ToString() == "0")
                {
                    text = myRet[1].ToString();
                    tableName = myRet[2].ToString();
                }

                String message = String.Empty;
                if (tableName == "" || tableName != GetTableName())
                {
                    message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "CheckTable", true);
                    string script = string.Format("<script>alert('{0}')</script>", message);
                    Page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script);
                    this.Panel2.Visible = true;
                    return;
                }

                table.Controls.Clear();

                XmlDocument DBXML = new XmlDocument();
                DBXML.LoadXml(text);
                XmlNode aNode = DBXML.DocumentElement.FirstChild;
                int count = 0;
                while (aNode != null)
                {
                    String column = String.Empty;
                    String dataType = String.Empty;
                    foreach (DataColumn dc in wdsAnyQuery.DesignDataSet.Tables[wdsAnyQuery.DataMember].Columns)
                    {
                        if (GetHeaderText(dc.ColumnName) == aNode.Attributes["Caption"].ToString())
                        {
                            column = dc.ColumnName;
                            dataType = dc.DataType.ToString();
                            break;
                        }
                    }

                    DataRow drNew = allControls.NewRow();
                    drNew["AutoSelect"] = Convert.ToBoolean(aNode.Attributes["IsActive"].Value.ToString());
                    drNew["Column"] = column;
                    drNew["Caption"] = aNode.Attributes["Caption"].Value.ToString();
                    drNew["DataType"] = dataType;
                    drNew["Condition"] = aNode.Attributes["Condition"].Value.ToString();
                    drNew["Operators"] = aNode.Attributes["Operator"].Value.ToString();
                    drNew["ColumnType"] = aNode.Attributes["ValueType"].Value.ToString();
                    drNew["Enabled"] = aNode.Attributes["Enabled"].Value.ToString();
                    drNew["DefaultValue"] = aNode.Attributes["Value1"].Value.ToString();
                    drNew["TextWidth"] = aNode.Attributes["Width"].Value.ToString();
                    drNew["RefValDSID"] = aNode.Attributes["RefValDSID"].Value.ToString();
                    drNew["RefValWhereItem"] = aNode.Attributes["WhereItem"].Value.ToString();
                    drNew["RefValTF"] = aNode.Attributes["DisplayMember"].Value.ToString();
                    drNew["RefValVF"] = aNode.Attributes["ValueMember"].Value.ToString();
                    drNew["RefValAlias"] = aNode.Attributes["SelectAlias"].Value.ToString();
                    drNew["RefValCMD"] = aNode.Attributes["SelectCommand"].Value.ToString();
                    drNew["RefValCD"] = aNode.Attributes["RefvalCD"].Value.ToString();
                    drNew["RefValSize"] = aNode.Attributes["RefvalSize"].Value.ToString();
                    drNew["RefValColumnMatch"] = aNode.Attributes["ColumnMatch"].Value.ToString();
                    drNew["RefValColumns"] = aNode.Attributes["Columns"].Value.ToString();
                    drNew["Text"] = aNode.Attributes["Value2"].Value.ToString();
                    drNew["RefButtonCaption"] = aNode.Attributes["RefButtonCaption"].Value.ToString();
                    drNew["RefButtonURL"] = aNode.Attributes["RefButtonURL"].Value.ToString();
                    drNew["RefButtonURLSize"] = aNode.Attributes["RefButtonURLSize"].Value.ToString();

                    allControls.Rows.Add(drNew);

                    table.Controls.Add(CreateColumn(count, drNew));
                    TableRow trEmpty = new TableRow();
                    table.Controls.Add(trEmpty);

                    aNode = aNode.NextSibling;
                    count++;
                }
            }
        }
        this.TextBox1.Text = String.Empty;
    }

    private String GetTableName()
    {
        String tabName = String.Empty;
        String strModuleName = remotename;
        strModuleName = strModuleName.Substring(0, strModuleName.IndexOf('.'));
        tabName = CliUtils.GetTableName(strModuleName, dataMember, CliUtils.fCurrentProject, "", true);

        return tabName;
    }

    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        if (this.DropDownList1.Items.Count > 0)
            this.DropDownList1.SelectedIndex = 0;
        this.TextBox1.Text = String.Empty;
    }

    protected void ButtonDelete_Click(object sender, EventArgs e)
    {
        String fileName = String.Empty;
        if (Status == "Save")
            fileName = this.TextBox1.Text;
        else if (Status == "Load")
            fileName = this.DropDownList1.Text;

        if (fileName != String.Empty)
        {
            //String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "DeleteSure");
            //message = String.Format(message, fileName);
            //if (MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            //{
                object[] param = new object[2];
                param[0] = anyQueryID;
                param[1] = fileName;
               object[] myRet = CliUtils.CallMethod("GLModule", "AnyQueryDeleteFile", param);
               if (myRet[0] != null && myRet[0].ToString() == "0")
               {
                   String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "DeleteSuccess", true);
                   string script = string.Format("<script>alert('{0}')</script>", message);
                   Page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script);
               }

                this.TextBox1.Text = String.Empty;
                if (this.DropDownList1.Items.Count > 0)
                    this.DropDownList1.SelectedIndex = 0;
                Reload();
                this.Panel2.Visible = true;
                //}
        }
    }

    private void Reload()
    {
        this.DropDownList1.Items.Clear();

        object[] param = new object[1];
        param[0] = anyQueryID;
        object[] myRet = CliUtils.CallMethod("GLModule", "AnyQueryLoadFile", param);

        DataSet dsLoadFile = new DataSet();
        if (myRet != null && myRet[0].ToString() == "0")
            dsLoadFile = myRet[1] as DataSet;

        this.TextBox1.Visible = false;
        this.DropDownList1.Visible = true;
        foreach (DataRow dr in dsLoadFile.Tables[0].Rows)
        {
            this.DropDownList1.Items.Add(dr[0].ToString());
        }
    }

    protected void ImageButtonSubtract_Click(object sender, ImageClickEventArgs e)
    {
        if (allowAddQueryField)
        {
            if (allControls.Rows.Count > 0)
            {
                allControls.Rows.RemoveAt(allControls.Rows.Count - 1);
                table.Controls.RemoveAt(table.Controls.Count - 1);
                table.Controls.RemoveAt(table.Controls.Count - 1);
            }
        }
    }

    protected void ImageButtonQuit_Click(object sender, ImageClickEventArgs e)
    {
        if (sessionRequest["Dialog"] != null)
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
