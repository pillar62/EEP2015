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
using Srvtools;
using System.Resources;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

public partial class InnerPages_frmListBoxList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ViewState.Add("ResxPath", Request.QueryString["Resx"]);
            this.ViewState.Add("DataSet", Request.QueryString["DataSet"]);
            this.ViewState.Add("ValueField", Request.QueryString["ValueField"]);
            this.ViewState.Add("TextField", Request.QueryString["TextField"]);
            this.ViewState.Add("Value", Request.QueryString["Value"]);
            this.ViewState.Add("Caption", Request.QueryString["Caption"]);
            this.ViewState.Add("Separator", Request.QueryString["Separator"]);
            this.ViewState.Add("ControlID", Request.QueryString["ControlID"]);
            this.ViewState.Add("DBAlias", Request.QueryString["DBAlias"]);
            this.ViewState.Add("SelectCommand", Request.QueryString["SelectCommand"]);
            //this.ViewState.Add("ListBoxQuery", Request.QueryString["ListBoxQuery"]);

            if (this.ViewState["ResxPath"] != null && this.ViewState["ResxPath"].ToString() != ""
                && this.ViewState["ValueField"] != null && this.ViewState["ValueField"].ToString() != ""
                && this.ViewState["TextField"] != null && this.ViewState["TextField"].ToString() != "")
            {
                this.GetData("");
            }
        }
        string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoRefPanel", "Text", true);
        string[] buttonNames = message.Split(';');
        if (buttonNames.Length >= 3)
        {
            this.btnOK.Text = buttonNames[0];
            this.btnCancel.Text = buttonNames[1];
            this.btnSearch.Text = buttonNames[2];
        }

        this.Title = Request.QueryString["Caption"];
    }

    private WebDataSet wds = new WebDataSet(false);
    private void GetData(string filter)
    {
        #region CreateWebDataSet
        XmlDocument xmlDoc = new XmlDocument();
        string resourceName = this.ViewState["ResxPath"].ToString();
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
                string webDataSetID = this.ViewState["DataSet"].ToString();
                XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSet[@Name='" + webDataSetID + "']");
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

                    wds.RemoteName = remoteName;
                    this.ViewState["RemoteName"] = remoteName;
                    wds.PacketRecords = -1;
                    wds.ServerModify = serverModify;
                    if (Request.QueryString["WhereStr"] != null)
                    {
                        wds.WhereStr = Request.QueryString["WhereStr"];
                    }
                    if (!string.IsNullOrEmpty(filter))
                    {
                        if (string.IsNullOrEmpty(wds.WhereStr))
                        {
                            wds.WhereStr = filter;
                        }
                        else
                        {
                            wds.WhereStr += string.Format(" and {0}", filter);
                        }
                    }
                    wds.Active = true;
                    this.ViewState["WebDataSet"] = wds.RealDataSet;
                    

                    //绑定list
                    this.ListBox1.DataSource = (DataSet)this.ViewState["WebDataSet"];
                    this.ListBox1.DataMember = ((DataSet)this.ViewState["WebDataSet"]).Tables[0].TableName;
                    this.ListBox1.DataTextField = this.ViewState["TextField"].ToString();
                    this.ListBox1.DataValueField = this.ViewState["ValueField"].ToString();

                    this.ListBox1.DataBind();

                    if (this.ViewState["Value"] != null && this.ViewState["Value"].ToString() != ""
                        && this.ViewState["Separator"] != null && this.ViewState["Separator"].ToString() != "")
                    {
                        string value = this.ViewState["Value"].ToString();
                        char sepa = this.ViewState["Separator"].ToString()[0];
                        string[] lstValues = value.Split(sepa);
                        List<ListItem> selItem = new List<ListItem>();
                        foreach (string val in lstValues)
                        {
                            ListItem item = this.ListBox1.Items.FindByValue(val);
                            if (item != null)
                            {
                                selItem.Add(item);
                                this.ListBox1.Items.Remove(item);
                            }
                        }

                        foreach (ListItem item in selItem)
                        {
                            this.ListBox2.Items.Add(item);
                        }
                    }
                }
                else
                {
                    string sql = this.ViewState["SelectCommand"].ToString();
                    if (!string.IsNullOrEmpty(filter))
                    {
                        sql = CliUtils.InsertWhere(sql, filter);
                    }
                    DataSet temp = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", sql, true, CliUtils.fCurrentProject);
                    this.ListBox1.DataSource = temp;
                    this.ListBox1.DataMember = temp.Tables[0].TableName;
                    this.ListBox1.DataTextField = this.ViewState["TextField"].ToString();
                    this.ListBox1.DataValueField = this.ViewState["ValueField"].ToString();
                    this.ListBox1.DataBind();

                    if (this.ViewState["Value"] != null && this.ViewState["Value"].ToString() != ""
                        && this.ViewState["Separator"] != null && this.ViewState["Separator"].ToString() != "")
                    {
                        string value = this.ViewState["Value"].ToString();
                        char sepa = this.ViewState["Separator"].ToString()[0];
                        string[] lstValues = value.Split(sepa);
                        List<ListItem> selItem = new List<ListItem>();
                        foreach (string val in lstValues)
                        {
                            ListItem item = this.ListBox1.Items.FindByValue(val);
                            if (item != null)
                            {
                                selItem.Add(item);
                                this.ListBox1.Items.Remove(item);
                            }
                        }

                        foreach (ListItem item in selItem)
                        {
                            this.ListBox2.Items.Add(item);
                        }
                    }
                }
            }
        }
        #endregion
    }

    public string GetField(bool isValueField)
    {
        if (isValueField)
        {
            return this.ViewState["ValueField"].ToString();
        }
        else
        {
            return this.ViewState["TextField"].ToString();
        }
    }

    protected void btnSelAll_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in this.ListBox1.Items)
        {
            if (!this.ListBox2.Items.Contains(item))
            {
                this.ListBox2.Items.Add(item);
            }
        }
        this.ListBox1.Items.Clear();
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        int[] selIndices = this.ListBox1.GetSelectedIndices();
        List<ListItem> items = new List<ListItem>();
        for (int i = 0; i < selIndices.Length; i++)
        {
            ListItem item = this.ListBox1.Items[selIndices[i]];
            items.Add(item);
            if (!this.ListBox2.Items.Contains(item))
            {
                this.ListBox2.Items.Add(item);
            }
        }

        foreach (ListItem item in items)
        {
            this.ListBox1.Items.Remove(item);
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        int[] selIndices = this.ListBox2.GetSelectedIndices();
        List<ListItem> items = new List<ListItem>();
        for (int i = 0; i < selIndices.Length; i++)
        {
            ListItem item = this.ListBox2.Items[selIndices[i]];
            items.Add(item);
            if (!this.ListBox1.Items.Contains(item))
            {
                this.ListBox1.Items.Add(item);
            }
        }
        foreach (ListItem item in items)
        {
            this.ListBox2.Items.Remove(item);
        }
    }

    protected void btnRemAll_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in this.ListBox2.Items)
        {
            if (!this.ListBox1.Items.Contains(item))
            {
                this.ListBox1.Items.Add(item);
            }
        }
        this.ListBox2.Items.Clear();
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (this.ViewState["Separator"] != null && this.ViewState["Separator"].ToString() != ""
            && this.ViewState["ControlID"] != null && this.ViewState["ControlID"].ToString() != "")
        {
            char sepa = this.ViewState["Separator"].ToString()[0];
            string retValue = "";
            foreach (ListItem item in this.ListBox2.Items)
            {
                retValue += item.Value + sepa;
            }
            if (retValue != "")
                retValue = retValue.Substring(0, retValue.LastIndexOf(sepa));
            string script = "window.opener.document.getElementById('" + this.ViewState["ControlID"].ToString() + "').value= '" + retValue.Replace("'", "\\'") + "';";
            Response.Write("<script>" + script + "window.close();</script>"); 
        }
        else
        {
            Response.Write("<script>window.close();</script>"); 
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script>window.close();</script>");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string tablename = "";
        string sqlcmd = "";
        if (this.ViewState["RemoteName"] != null)
        {
            string remotename = this.ViewState["RemoteName"].ToString();
            string strModuleName = remotename.Substring(0, remotename.IndexOf('.'));
            string strTableName = remotename.Substring(remotename.IndexOf('.') + 1);
            tablename = CliUtils.GetTableName(strModuleName, strTableName, CliUtils.fCurrentProject);
            sqlcmd = CliUtils.GetSqlCommandText(strModuleName, strTableName, CliUtils.fCurrentProject);
        }
        else
        {
            sqlcmd = this.ViewState["SelectCommand"].ToString();
            tablename = CliUtils.GetTableName(sqlcmd);
        }
        string[] quote = CliUtils.GetDataBaseQuote();

        string valueField = this.ViewState["ValueField"].ToString();
        string textField = this.ViewState["TextField"].ToString();
        string whereString = "";
        if (!string.IsNullOrEmpty(this.txtSearchValue.Text) && !string.IsNullOrEmpty(valueField))
        {
            whereString += string.Format("{0} like '%{1}%'", CliUtils.GetTableNameForColumn(sqlcmd, valueField, tablename, quote), this.txtSearchValue.Text);
        }
        if (!string.IsNullOrEmpty(this.txtSearchText.Text) && !string.IsNullOrEmpty(textField))
        {
            if (string.IsNullOrEmpty(whereString))
            {
                whereString += string.Format("{0} like '%{1}%'", CliUtils.GetTableNameForColumn(sqlcmd, textField, tablename, quote), this.txtSearchText.Text);
            }
            else
            {
                whereString += string.Format(" and {0} like '%{1}%'", CliUtils.GetTableNameForColumn(sqlcmd, textField, tablename, quote), this.txtSearchText.Text);
            }
        }
        if (!string.IsNullOrEmpty(whereString))
        {
            GetData(whereString);
        }
    }
}
