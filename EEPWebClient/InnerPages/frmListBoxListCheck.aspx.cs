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
using System.Xml;
using System.Resources;
using Srvtools;

public partial class InnerPages_frmListBoxListCheck : System.Web.UI.Page
{
    public string DataSouceID
    {
        get { return (this.ViewState["DataSourceID"] == null?"":(string)this.ViewState["DataSourceID"]); }
        set { this.ViewState["DataSourceID"] = value; }
    }

    public string DataMember
    {
        get { return (this.ViewState["DataMember"] == null ? "" : (string)this.ViewState["DataMember"]); }
        set { this.ViewState["DataMember"] = value; }
    }

    public string DBAlias
    {
        get { return (this.ViewState["DBAlias"] == null ? "" : (string)this.ViewState["DBAlias"]); }
        set { this.ViewState["DBAlias"] = value; }
    }
    public string SelectCommand
    {
        get { return (this.ViewState["SelectCommand"] == null ? "" : (string)this.ViewState["SelectCommand"]); }
        set { this.ViewState["SelectCommand"] = value; }
    }

    public string TextField
    {
        get { return (this.ViewState["TextField"] == null ? "" : (string)this.ViewState["TextField"]); }
        set { this.ViewState["TextField"] = value; }
    }

    public string ValueField
    {
        get { return (this.ViewState["ValueField"] == null ? "" : (string)this.ViewState["ValueField"]); }
        set { this.ViewState["ValueField"] = value; }
    }

    public string Value
    {
        get { return (this.ViewState["Value"] == null ? "" : (string)this.ViewState["Value"]); }
        set { this.ViewState["Value"] = value; }
    }

    public int Columns
    {
        get { return (this.ViewState["Columns"] == null ? 1 : (int)this.ViewState["Columns"]); }
        set { this.ViewState["Columns"] = value; }
    }

    public string ResxPath
    {
        get { return (this.ViewState["ResxPath"] == null ? "" : (string)this.ViewState["ResxPath"]); }
        set { this.ViewState["ResxPath"] = value; }
    }

    public char Separator
    {
        get { return (this.ViewState["Separator"] == null ? ',' : (char)this.ViewState["Separator"]); }
        set { this.ViewState["Separator"] = value; }
    }

    public string ControlID
    {
        get { return (this.ViewState["ControlID"] == null ? "" : (string)this.ViewState["ControlID"]); }
        set { this.ViewState["ControlID"] = value; }
    }
	

    public DataTable DataTable
    {
        get
        {
            if (this.ViewState["DataTable"] == null)
            {
                this.ViewState["DataTable"] = GetDataSource();
            }
            return (DataTable)this.ViewState["DataTable"];
        }
        set { this.ViewState["DataTable"] = value; }
    }

    private DataTable GetDataSource()
    {
        #region CreateWebDataSet
        XmlDocument xmlDoc = new XmlDocument();
        string resourceName = ResxPath;
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
                XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSet[@Name='" + DataSouceID + "']");
                if (nWDS != null)
                {
                    string remoteName = "";
                    bool active = false;
                    bool serverModify = false;

                    XmlNode nRemoteName = nWDS.SelectSingleNode("RemoteName");
                    if (nRemoteName != null)
                        remoteName = nRemoteName.InnerText;

                    XmlNode nActive = nWDS.SelectSingleNode("Active");
                    if (nActive != null)
                        active = nActive.InnerText.Length == 0 ? false : Convert.ToBoolean(nActive.InnerText);

                    XmlNode nServerModify = nWDS.SelectSingleNode("ServerModify");
                    if (nServerModify != null)
                        serverModify = nServerModify.InnerText.Length == 0 ? false : Convert.ToBoolean(nServerModify.InnerText);
                    WebDataSet wds = new WebDataSet();
                    wds.RemoteName = remoteName;
                    wds.PacketRecords = -1;
                    wds.ServerModify = serverModify;
                    if (Request.QueryString["WhereStr"] != null)
                    {
                        wds.WhereStr = Request.QueryString["WhereStr"];
                    }
                    wds.Active = true;
                    return wds.RealDataSet.Tables[DataMember];
                }
                else
                {
                    DataSet temp = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", this.ViewState["SelectCommand"].ToString(), true, CliUtils.fCurrentProject);
                    return temp.Tables[0];
                }
            }
        }
        return new DataTable();
        #endregion
    }
	

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            DataSouceID = Request.QueryString["DataSet"];
            DataMember = Request.QueryString["DataMember"];
            DBAlias = Request.QueryString["DBAlias"];
            SelectCommand = Request.QueryString["SelectCommand"];
            ValueField = Request.QueryString["ValueField"];
            TextField = Request.QueryString["TextField"];
            Value = Request.QueryString["Value"];
            Columns = int.Parse(Request.QueryString["Columns"]);
            ResxPath = Request.QueryString["Resx"];
            ControlID = Request.QueryString["ControlID"];
        }
        InitialButton();
        CreateCheckBox();
        SetButtonEvent();

        this.Title = Request.QueryString["Caption"];
    }

    private void InitialButton()
    {
        string caption = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebListBoxList", "ButtonCaption", true);
        string[] arrcaption = caption.Split(';');
        BtnSelectAll.Text = arrcaption[0];
        BtnUnSelectAll.Text = arrcaption[1];
        btnOK.Text = arrcaption[2];
        btnCancel.Text = arrcaption[3];
    }

    private void SetButtonEvent()
    {
        BtnSelectAll.OnClientClick = "SetChecked(" + DataTable.Rows.Count + ",true); return false;";
        BtnUnSelectAll.OnClientClick = "SetChecked(" + DataTable.Rows.Count + ",false); return false;";
    }

    private void CreateCheckBox()
    {
        string[] arrvalue = Value.Split(Separator);
        int rowscount = DataTable.Rows.Count / Columns;
        if(DataTable.Rows.Count / Columns != 0)
        {
            rowscount ++;
        }
        for (int i = 0; i < rowscount; i++)
        {
            TableRow tr = new TableRow();
            for (int j = 0; j < Columns; j++)
            {
                TableCell td = new TableCell();
                int index = i * Columns + j;
                if (index < DataTable.Rows.Count)
                {
                    string text = DataTable.Rows[index][TextField].ToString();
                    string value = DataTable.Rows[index][ValueField].ToString();

                    CheckBox box = new CheckBox();
                    foreach (string str in arrvalue)
                    {
                        if (str == value)
                        {
                            box.Checked = true;
                        }
                    }
                    box.ID = "check" + index.ToString();
                    box.Text = text;
                    box.ToolTip = value;
                    td.HorizontalAlign = HorizontalAlign.Left;
                    td.Controls.Add(box);
                   
                }
                tr.Cells.Add(td);
            }
            TableCheckBox.Rows.Add(tr);
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string strrtn = "";
        for (int i = 0; i < DataTable.Rows.Count; i++)
        {
            CheckBox box = this.FindControl("check" + i.ToString()) as CheckBox;
            if (box.Checked)
            {
                strrtn += box.ToolTip + Separator.ToString();
            }
        }
        if (strrtn != "")
        {
            strrtn = strrtn.Substring(0, strrtn.Length - 1);
        }

        string script = "window.opener.document.getElementById('" + ControlID + "').value= '" + strrtn.Replace("'", "\\'") + "';";
        Response.Write("<script>" + script + "window.close();</script>"); 
    }
}
