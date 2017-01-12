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
using System.Xml;
using System.Resources;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

public partial class InnerPages_frmTreeView : System.Web.UI.Page
{
    string strKey = "";
    string strParent = "";
    string strText = "";
    string strKeyField = "";
    string strParentField = "";
    string strTextField = "";
    string strKeyCaption = "";
    string strParentCaption = "";
    string strTextCaption = "";
    string strMode = "";
    string strPath = "";
    string psyPath = "";
    string queryString = "";
    string dataSetID = "";
    string dataMember = "";
    string treeviewid = "";
    ArrayList lstkey = new ArrayList();
    string[] arrEditCation;
    string[] arrEditColumn;
    string[] arrEditColumnType;
    string[] arrDefaultValue;
    private string[] arrRefValCMD;
    private string[] arrRefValAlias;
    private string[] arrRefValDSID;
    private string[] arrRefValDM;
    private string[] arrRefValTF;
    private string[] arrRefValVF;
    private string[] arrRefValColumnMatch;
    private string[] arrRefValColumns;
    private string[] arrRefValWhereItem;
    private WebDataSet[] refValDateSet;
    private WebDataSource[] refValDateSource;
    private WebDataSource[] refValDateSourcecmd;

    ListItem[] lstnode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strMode = Request.QueryString["Mode"];
        treeviewid = Request.QueryString["TreeViewID"];
        this.Title = strMode + " TreeNode";
        strKey = Request.QueryString["Nodekey"];
        strParent = Request.QueryString["Nodeparent"];
        strText = Request.QueryString["Nodetext"];
        strKeyField = Request.QueryString["Keyfield"];
        strParentField = Request.QueryString["Parentfield"];
        strTextField = Request.QueryString["Textfield"];
        strKeyCaption = Request.QueryString["Keycaption"];
        strParentCaption = Request.QueryString["Parentcaption"];
        strTextCaption = Request.QueryString["Textcaption"];
        dataSetID = Request.QueryString["Datasetid"];
        dataMember = Request.QueryString["Datamember"];
        strPath = Request.QueryString["Pagepath"];
        queryString = Request.QueryString["Querystring"];
        psyPath = Request.QueryString["Psypagepath"];
        string editcaption = Request.QueryString["Editcaption"];
        string editcolumn = Request.QueryString["Editcolumn"];
        string editcolumntype = Request.QueryString["Editcolumntype"];
        string editdefaultvalue = Request.QueryString["Editdefaultvalue"];
        string refvalcmd = Request.QueryString["Refvalselcmd"];
        string refvalalias = Request.QueryString["Refvalselalias"];
        string refvaldstid = Request.QueryString["Refvaldstid"];
        string refvaldm = Request.QueryString["Refvaldm"];
        string refvaltf = Request.QueryString["Refvaltf"];
        string refvalvf = Request.QueryString["Refvalvf"];
        string refvalcolumnmatch = Request.QueryString["Refvalcolumnmatch"];
        string refvalcolumns = Request.QueryString["Refvalcolumns"];
        string refvalwhereitem = Request.QueryString["Refvalwhereitem"];
        arrEditCation = editcaption.Split(';');
        arrEditColumn = editcolumn.Split(';');
        arrEditColumnType = editcolumntype.Split(';');
        arrDefaultValue = editdefaultvalue.Split(';');
        if (editcolumn != null && editcolumn != "")
        {
            arrRefValCMD = refvalcmd.Split(';');
            arrRefValAlias = refvalalias.Split(';');
            arrRefValDSID = refvaldstid.Split(';');
            arrRefValDM = refvaldm.Split(';');
            arrRefValTF = refvaltf.Split(';');
            arrRefValVF = refvalvf.Split(';');
            arrRefValColumnMatch = refvalcolumnmatch.Split(';');
            arrRefValColumns = refvalcolumns.Split(';');
            arrRefValWhereItem = refvalwhereitem.Split(';');
            CreatDataSet(arrRefValDSID);

            int cmdnum = 0;
            foreach (string strcmd in arrRefValCMD)
            {
                if (strcmd != string.Empty)
                {
                    cmdnum++;
                }
            }
            refValDateSourcecmd = new WebDataSource[cmdnum];
        }

        CreatDataSet(dataSetID);
        
        IntialEditItem();

        if (!IsPostBack)
        {
            InitialText();
        }
    }


    private Label[] labels;
    private TextBox[] textBoxes;
    private DropDownList[] dropDownlists;
    private WebRefVal[] webRefVals;
    private WebDateTimePicker[] webDateTimePickers;
    private Table table = new Table();
    private void IntialEditItem()
    {
        int columnNum = arrEditColumn.Length;
        labels = new Label[columnNum];
        dropDownlists = new DropDownList[columnNum];
        textBoxes = new TextBox[columnNum];
        webRefVals = new WebRefVal[columnNum];
        webDateTimePickers = new WebDateTimePicker[columnNum];
        TableRow[] trText= new TableRow[columnNum];
        int cmdIndex = 0;

        for (int i = 0; i < columnNum; i++)
        {
            TableCell tcLabel = new TableCell();
            TableCell tcText = new TableCell();
            tcLabel.HorizontalAlign = HorizontalAlign.Center;
            tcLabel.VerticalAlign = VerticalAlign.Top;
            tcText.HorizontalAlign = HorizontalAlign.Left;
            tcText.VerticalAlign = VerticalAlign.Top;


            //create captionlabel
            labels[i] = new Label();
            labels[i].ID = "lbl" + i.ToString();
            labels[i].Width = 48;
            labels[i].Text = arrEditCation[i];
            tcLabel.Controls.Add(labels[i]);
            switch (arrEditColumnType[i])
            {
                case "TextBoxColumn":
                    {
                        textBoxes[i] = new TextBox();
                        textBoxes[i].ID = "txt" + i.ToString();
                        textBoxes[i].Width = 112;
                        if (string.Compare(strMode, "update", true) == 0)//IgnoreCase
                        {
                            textBoxes[i].Text = GetText(arrEditColumn[i]);
                        }
                        else
                        {
                            textBoxes[i].Text = arrDefaultValue[i];
                        }
                        tcText.Controls.Add(textBoxes[i]);
                        break;

                    }


                case "ComboBoxColumn": 
                    {
                        dropDownlists[i] = new DropDownList();
                        dropDownlists[i].ID = "txt" + i.ToString();
                        dropDownlists[i].Width = 112;
                        int dsidnum = lstDataSetID.IndexOf(arrRefValDSID[i]);
                        if (dsidnum >= 0)
                        {
                            dropDownlists[i].DataSourceID = "refvalds" + dsidnum.ToString();
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


                        if (string.Compare(strMode, "update", true) == 0)//IgnoreCase
                        {
                            if (GetText(arrEditColumn[i]) != "")
                            {
                                try
                                {
                                    dropDownlists[i].SelectedValue = GetText(arrEditColumn[i]);
                                }
                                catch
                                { }
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
                        tcText.Controls.Add(dropDownlists[i]);
                        break;
                    }
                case "RefValColumn":
                    {
                        webRefVals[i] = new WebRefVal();
                        webRefVals[i].ID = "txt" + i.ToString();
                        webRefVals[i].Width = 112;
                        int dsidnum = lstDataSetID.IndexOf(arrRefValDSID[i]);
                        if (dsidnum >= 0)
                        {
                            webRefVals[i].DataSourceID = "refvalds" + dsidnum.ToString();
                            webRefVals[i].ResxDataSet = arrRefValDSID[i];
                            webRefVals[i].ResxFilePath = psyPath + ".vi-VN.resx";
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

                        if (string.Compare(strMode, "update", true) == 0)//IgnoreCase
                        {
                            if (GetText(arrEditColumn[i]) != "")
                            {
                                try
                                {
                                    webRefVals[i].BindingValue = GetText(arrEditColumn[i]);
                                }
                                catch
                                { }
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

                        tcText.Controls.Add(webRefVals[i]);


                        break;
                    }
                case "CalendarColumn":
                    {
                        webDateTimePickers[i] = new WebDateTimePicker();
                        webDateTimePickers[i].ID = "txt" + i.ToString();
                        webDateTimePickers[i].DateFormat = dateFormat.ShortDate;
                        webDateTimePickers[i].Width = 112;

                        if (string.Compare(strMode, "update", true) == 0)//IgnoreCase
                        {
                            if (GetText(arrEditColumn[i]) != "")
                            {
                                try
                                {
                                    webDateTimePickers[i].Text = GetText(arrEditColumn[i]);
                                }
                                catch
                                { }
                            }
                        }
                        else
                        {
                            try
                            {
                                webDateTimePickers[i].Text = arrDefaultValue[i];
                            }
                            catch
                            { }
                        }

                        tcText.Controls.Add(webDateTimePickers[i]);



                        break;
                    }
                case "RadioButtonColumn":
                    {





                        break;
                    }
            }

            trText[i] = new TableRow();
            trText[i].Cells.Add(tcLabel);
            trText[i].Cells.Add(tcText);
            table.Controls.Add(trText[i]);

        }
        pnTreeView.Controls.Add(table);






    }

    private string GetText(string columnname)
    {
        string retVal = "";
        int rowcount = dt.Rows.Count;
        for (int i = 0; i < rowcount; i++)
        {
            if (dt.Rows[i][strKeyField].ToString() == strKey)
            {
                retVal = dt.Rows[i][columnname].ToString();
            }      
        }
        return retVal;
    }



    DataTable dt;
    private void CreatDataSet(string datasetid)
    {
        XmlDocument xmlDoc = new XmlDocument();
        string resourceName = psyPath + ".vi-VN.resx";
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

                    WebDataSet wds = new WebDataSet();
                    wds.RemoteName = remoteName;
                    wds.PacketRecords = packetRecords;
                    wds.ServerModify = serverModify;
                    wds.Active = true;


                    dt = wds.RealDataSet.Tables[dataMember];
                    int rowcount = dt.Rows.Count;
                    lstnode = new ListItem[rowcount];
                    for (int i = 0; i < rowcount; i++)
                    {
                        lstnode[i] = new ListItem();
                        lstnode[i].Text = dt.Rows[i][strTextField].ToString();
                        lstnode[i].Value = dt.Rows[i][strKeyField].ToString();
                        lstkey.Add(dt.Rows[i][strKeyField]);
                    }                  
                }
            }
        }
                    
    }


    private List<string> lstDataSetID = new List<string>();
    private void CreatDataSet(string[] datasetid)
    {
        int datasetnum = 0;

        foreach (string strdsid in datasetid)
        {
            if (strdsid != string.Empty)
            {
                datasetnum++;
            }
        }
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
                    string resourceName = psyPath + ".vi-VN.resx";
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

    private void InitialText()
    {
        string caption = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebTreeView", "Caption", true);
        string[] arrcaption = caption.Split(';');
        if (strKeyCaption != "")
        {
            lblKey.Text = strKeyCaption;
        }
        else
        {
            lblKey.Text = arrcaption[0];
        }
        if (strParentCaption != "")
        {
            lblParent.Text = strParentCaption;
        }
        else
        {
            lblParent.Text = arrcaption[1];
        }
        if (strTextCaption != "")
        {
            lblText.Text = strTextCaption;
        }
        else
        {
            lblText.Text = arrcaption[2];
        }

        ddlParent.Items.Add(new ListItem("(None)", ""));
        for (int i = 0; i < lstnode.Length; i++)
        {
            if (lstnode[i].Value != strKey || string.Compare(strMode, "add", true) == 0)//IgnoreCase
            {
                ddlParent.Items.Add(lstnode[i]);
            }
        }

        if (string.Compare(strMode, "add", true) == 0)//IgnoreCase
        {
            txtKey.Text = FindMaxKey(lstkey).ToString();
            ddlParent.SelectedValue = strKey;
        }

        else if (string.Compare(strMode, "update", true) == 0)//IgnoreCase
        {
            txtKey.Text = strKey;
            ddlParent.SelectedValue = strParent;
            txtText.Text = strText;
        }
        
       
        caption = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoNavigator", "NavText", true);
        for (int i = 0; i < 7; i++)
        {
            caption = caption.Substring(caption.IndexOf(";") + 1);
        }
        
        btnOk.Text = caption.Substring(0, caption.IndexOf(";"));
        caption = caption.Substring(caption.IndexOf(";") + 1);
        btnCancel.Text = caption.Substring(0, caption.IndexOf(";"));
    }

    private int FindMaxKey(ArrayList arrlst)
    {
        int maxkey = -1;
        if (arrlst.Count > 0)
        {
            try
            {
                maxkey = Convert.ToInt32(arrlst[0]);

                for (int i = 1; i < arrlst.Count; i++)
                {
                    int key = Convert.ToInt32(arrlst[i]);
                    if (key > maxkey)
                    {
                        maxkey = key;
                    }
                }
            }
            catch
            {
                throw new Exception("Can't Convert Key to int32");
            }
        }

        return (maxkey + 1);
    }



    protected void BtnOk_Click(object sender, EventArgs e)
    {
      //  Response.Write("<script>window.opener.location.reload('')</script>");
      //  Response.Write("<script>window.parent.location.herf=''</script>)");
        string strparent = ddlParent.SelectedValue;
        string strkey = txtKey.Text;
        string strtext = txtText.Text;
        strtext = HttpUtility.UrlEncode(strtext);
        queryString = queryString.Replace(';', '&');


        int columnNum = arrEditColumn.Length;
        string[] editText = new string[columnNum];
        for (int i = 0; i < columnNum; i++)
        { 
            switch(arrEditColumnType[i])
            {
                case "TextBoxColumn": editText[i] = textBoxes[i].Text; break;
                case "ComboBoxColumn":  editText[i] = dropDownlists[i].SelectedValue.ToString();break;
                case "RefValColumn": editText[i] = webRefVals[i].BindingValue.ToString(); break;
                case "CalendarColumn":  editText[i] = webDateTimePickers[i].Text;break;
                case "RadioButtonColumn":break;
            }
        }

        string url = "";
        if (queryString == "")
        {
            url = strPath + "?Nodekey=" + strkey + "&Nodeparent=" + strparent + "&Nodetext=" + strtext + "&Nodemode=" + strMode + "&Edittext=";
        }     

        else
        {
            url = strPath + "?" + queryString + "&Nodekey=" + strkey + "&Nodeparent=" + strparent + "&Nodetext=" + strtext + "&Nodemode=" + strMode + "&Edittext=";
        }

        foreach( string str in editText)
        {
            string strencode = HttpUtility.UrlEncode(str);
            url += strencode + ";";
        }
        url = url.Substring(0, url.LastIndexOf(';'));
        url += "&TreeViewID=" + treeviewid;
        Response.Write("<script>window.opener.location.reload('"+ url + "')</script>");
        Response.Write("<script language=javascript>window.close();</script>");
        
    }




    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=javascript>window.close();</script>");
    }
}
