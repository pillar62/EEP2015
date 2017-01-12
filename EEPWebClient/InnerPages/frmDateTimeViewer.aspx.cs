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
using System.Xml.Serialization;
using System.IO;
using System.Resources;
using Srvtools;
using System.Drawing;
using System.Collections.Generic;

public partial class InnerPages_frmDateTimeViewer : System.Web.UI.Page
{
    string strField = "";
    string psyPath = "";
    string strPath = "";
    string dataSetID = "";
    string dataMember = "";
    string dateFromField = "";
    string dateToField = "";
    string timeFromField = "";
    string timeToField = "";
    string weekField = "";
    string monthField = "";
    string strWhere = "";
    string dayLightOnly = "";
    //string dateType = "";
    //string dateTypeField = "";
    string strDay = "";
    DateTime dtday;
    WebDataSet wdsDay = new WebDataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        strField = Request.QueryString["Field"];
        psyPath = Request.QueryString["Psypagepath"];
        strPath = Request.QueryString["Path"];
        dataSetID = Request.QueryString["Datasetid"];
        dataMember = Request.QueryString["Datamember"];
        strDay = Request.QueryString["Day"];
        dayLightOnly = Request.QueryString["Daylightonly"];
        this.Title = Request.QueryString["Caption"];
        if (Request.QueryString["Wherestring"] != null)
        {
            strWhere = Request.QueryString["Wherestring"];
            strWhere = strWhere.Replace("*_*", "'");
        }

        string[] arrField = strField.Split(';');

        dateFromField = arrField[0];
        dateToField = arrField[1];
        timeFromField = arrField[2];
        timeToField = arrField[3];
        weekField = arrField[4];
        monthField = arrField[5];

        dtday = DateTime.Parse(strDay);
        if(!IsPostBack)
        {
            InitialControl();
        }
        CreatDataSet(dataSetID);
        InitialCalendar(); 
    }

    private void InitialControl()
    {
        //this.Title = "Calendar";

        string caption = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebDateTimeViewer", "Text", true);

        string[] text = caption.Split(';');
        lblYear.Text = text[0];
        lblMonth.Text = text[1];
        lbPrevious.Text = text[2];
        lbNext.Text = text[3];
        lblSun.Text = text[4];
        lblMon.Text = text[5];
        lblTue.Text = text[6];
        lblWed.Text = text[7];
        lblThu.Text = text[8];
        lblFri.Text = text[9];
        lblSat.Text = text[10];

        for (int i = 1; i < 13; i++)
        {
            ddlMonth.Items.Add(i.ToString());
        }
        for (int i = 1951; i < 2051; i++)
        {
            ddlYear.Items.Add(i.ToString());
        }
        ddlYear.SelectedValue = dtday.Year.ToString();
        ddlMonth.SelectedValue = dtday.Month.ToString();
    }

    private void InitialCalendar()
    {
        DateTime firstday = dtday.AddDays(1 - dtday.Day);
        DateTime lastday = firstday.AddMonths(1);
        lastday = lastday.AddDays(-1);
        int dayNum = lastday.DayOfYear - firstday.DayOfYear + 1;
        while (firstday.DayOfWeek != DayOfWeek.Sunday)
        {
            firstday = firstday.AddDays(-1);
            dayNum ++;
        }
        while (lastday.DayOfWeek != DayOfWeek.Saturday)
        {
            lastday = lastday.AddDays(1);
            dayNum ++;
        }

       
        List<object> lstprogress = (List<object>)InitialProgress(firstday, lastday);
        
        LinkButton[] labelCalendar = new LinkButton[2 * dayNum];
       // Label[] labelCalendar = new Label[2 * dayNum];
       // System.Web.UI.WebControls.Image[] imgCalendar = new System.Web.UI.WebControls.Image[3 * dayNum];//new add
        int imageCount = 0;
        for (int i = 0; i < lstprogress.Count; i++)
        {
           if(((List<string>)lstprogress[i]).Count > 0)
            imageCount += ((List<string>)lstprogress[i]).Count + 1;
        }
        System.Web.UI.WebControls.Image[] imgCalendar = new System.Web.UI.WebControls.Image[imageCount];
        int imageIndex = 0;

        TableRow[] rowCalendar = new TableRow[2 * (dayNum / 7)];
        for (int i = 0; i < rowCalendar.Length; i++)
        {
            rowCalendar[i] = new TableRow();
        }
        Table tableCalendar = new Table();
        //tableCalendar.BorderStyle = BorderStyle.Solid;
        tableCalendar.BackColor = Color.FromName("#C0C7FE");
        tableCalendar.CellSpacing = 1;
        tableCalendar.CellPadding = 0;

        for (int i = 0; i < dayNum; i++)
        {
            TableCell cellCalendar = new TableCell();
            TableCell cellProgress = new TableCell();
            cellCalendar.BackColor = Color.FromName("#FFFFCC");
            cellProgress.BackColor = Color.White;
            cellCalendar.Width = 50;
            labelCalendar[i] = new LinkButton();
            labelCalendar[i].Width = 50;
            labelCalendar[i].Text = firstday.AddDays(i).Month.ToString() + "/" + firstday.AddDays(i).Day.ToString();
            labelCalendar[i].ID = i.ToString();
            labelCalendar[i].Click += new EventHandler(InnerPages_frmDateTimeViewer_Click);
            //labelCalendar[i].BorderStyle = BorderStyle.Solid;
            //labelCalendar[i].BorderColor = Color.Black; 
            if (firstday.AddDays(i).Month == (DateTime.Parse(strDay).Month))
            {
                labelCalendar[i].ForeColor = Color.FromName("#663399");
            }
            else
            {
                labelCalendar[i].ForeColor = Color.FromName("#FF9966");
            }

            cellCalendar.Controls.Add(labelCalendar[i]);

            List<string> lsttime = (List<string>)lstprogress[i];
            if (lsttime.Count > 0)
            {
                int[] progresswidth = new int[lsttime.Count];
                if (string.Compare(dayLightOnly, "true", true) == 0)//IgnoreCase
                {
                    for (int j = 0; j < lsttime.Count / 2; j++)
                    {

                        imgCalendar[imageIndex] = new System.Web.UI.WebControls.Image();
                        imgCalendar[imageIndex].ImageUrl = "../Image/datetimeviewer/blank.gif";
                        imgCalendar[imageIndex].Height = 10;
                        //int width1 = int.Parse(lsttime[2 * j].Substring(0, lsttime[2 *j].IndexOf(':')));
                        progresswidth[2 * j] = int.Parse(lsttime[2 * j].Substring(0, lsttime[2 * j].IndexOf(':')));
                        progresswidth[2 * j] = Math.Min(Math.Max(0, 2 * (progresswidth[2 * j] - 8)), 24);
                        if (j > 0)
                        {
                            imgCalendar[imageIndex].Width = 2 * (progresswidth[2 * j] - progresswidth[2 * j - 1]);
                        }
                        else
                        {
                            imgCalendar[imageIndex].Width = 2 * progresswidth[2 * j];
                        }

                        cellProgress.Controls.Add(imgCalendar[imageIndex]);
                        imageIndex++;

                        imgCalendar[imageIndex] = new System.Web.UI.WebControls.Image();
                        imgCalendar[imageIndex].ImageUrl = "../Image/datetimeviewer/purple.gif";
                        imgCalendar[imageIndex].Height = 10;
                        // int width2 = int.Parse(lsttime[2 * j + 1].Substring(0, lsttime[2 * j + 1].IndexOf(':')));
                        progresswidth[2 * j + 1] = int.Parse(lsttime[2 * j + 1].Substring(0, lsttime[2 * j + 1].IndexOf(':')));
                        progresswidth[2 * j + 1] = Math.Min(Math.Max(0, 2 * (progresswidth[2 * j + 1] - 8)), 24);
                        // imgCalendar[imageIndex].Width = 2 * (width2 - width1);
                        imgCalendar[imageIndex].Width = 2 * (progresswidth[2 * j + 1] - progresswidth[2 * j]);
                        imgCalendar[imageIndex].ToolTip = lsttime[2 * j] + " - " + lsttime[2 * j + 1];

                        cellProgress.Controls.Add(imgCalendar[imageIndex]);
                        imageIndex++;

                    }

                    imgCalendar[imageIndex] = new System.Web.UI.WebControls.Image();
                    imgCalendar[imageIndex].ImageUrl = "../Image/datetimeviewer/blank.gif";
                    imgCalendar[imageIndex].Height = 10;
                    int widthend = int.Parse(lsttime[lsttime.Count - 1].Substring(0, lsttime[lsttime.Count - 1].IndexOf(':')));
                    widthend = Math.Min(Math.Max(0, 2 * (widthend - 8)), 24);
                    imgCalendar[imageIndex].Width = 2 * (24 - widthend);
                    cellProgress.Controls.Add(imgCalendar[imageIndex]);

                    imageIndex++;
                }
                else
                {
                    for (int j = 0; j < lsttime.Count / 2; j++)
                    {

                        imgCalendar[imageIndex] = new System.Web.UI.WebControls.Image();
                        imgCalendar[imageIndex].ImageUrl = "../Image/datetimeviewer/blank.gif";
                        imgCalendar[imageIndex].Height = 10;
                        //int width1 = int.Parse(lsttime[2 * j].Substring(0, lsttime[2 *j].IndexOf(':')));
                        progresswidth[2 * j] = int.Parse(lsttime[2 * j].Substring(0, lsttime[2 * j].IndexOf(':')));
                        if (j > 0)
                        {
                            imgCalendar[imageIndex].Width = 2 * (progresswidth[2 * j] - progresswidth[2 * j - 1]);
                        }
                        else
                        {
                            imgCalendar[imageIndex].Width = 2 * progresswidth[2 * j];
                        }

                        cellProgress.Controls.Add(imgCalendar[imageIndex]);
                        imageIndex++;

                        imgCalendar[imageIndex] = new System.Web.UI.WebControls.Image();
                        imgCalendar[imageIndex].ImageUrl = "../Image/datetimeviewer/purple.gif";
                        imgCalendar[imageIndex].Height = 10;
                        // int width2 = int.Parse(lsttime[2 * j + 1].Substring(0, lsttime[2 * j + 1].IndexOf(':')));
                        progresswidth[2 * j + 1] = int.Parse(lsttime[2 * j + 1].Substring(0, lsttime[2 * j + 1].IndexOf(':')));
                        // imgCalendar[imageIndex].Width = 2 * (width2 - width1);
                        imgCalendar[imageIndex].Width = 2 * (progresswidth[2 * j + 1] - progresswidth[2 * j]);
                        imgCalendar[imageIndex].ToolTip = lsttime[2 * j] + " - " + lsttime[2 * j + 1];

                        cellProgress.Controls.Add(imgCalendar[imageIndex]);
                        imageIndex++;

                    }

                    imgCalendar[imageIndex] = new System.Web.UI.WebControls.Image();
                    imgCalendar[imageIndex].ImageUrl = "../Image/datetimeviewer/blank.gif";
                    imgCalendar[imageIndex].Height = 10;
                    int widthend = int.Parse(lsttime[lsttime.Count - 1].Substring(0, lsttime[lsttime.Count - 1].IndexOf(':')));
                    imgCalendar[imageIndex].Width = 2 * (24 - widthend);
                    cellProgress.Controls.Add(imgCalendar[imageIndex]);

                    imageIndex++; 
                }
            }
            cellProgress.Height = 20;
            rowCalendar[2 * (i / 7)].Cells.Add(cellCalendar);
            rowCalendar[2 * (i / 7) + 1].Cells.Add(cellProgress);
        }

        for (int i = 0; i < rowCalendar.Length; i++)
        {
            tableCalendar.Controls.Add(rowCalendar[i]);
        }

        pnCalendar.Controls.Add(tableCalendar);

    }

    void InnerPages_frmDateTimeViewer_Click(object sender, EventArgs e)
    {
        string strdate = (sender as LinkButton).Text;
        DateTime dtSelected = new DateTime(int.Parse(ddlYear.SelectedValue), int.Parse(strdate.Substring(0,strdate.IndexOf('/'))),
            int.Parse(strdate.Substring(strdate.IndexOf('/') + 1)));

        string url = strPath + "?DateTimeViewerTime=" + dtSelected.ToShortDateString();
        Response.Write("<script>window.opener.location.reload('" + url + "')</script>");
        Response.Write("<script language=javascript>window.close();</script>");
    }

    private object InitialProgress(DateTime firstday, DateTime lastday)
    {
        List<object> lstrtn = new List<object>();
        DateTime dti = firstday;
        while (dti <= lastday)
        {
            object value = InitialProgressDay(dti);
            FormatProgress(value);
            SortProgress(value);
            lstrtn.Add(value);

            dti = dti.AddDays(1);
        }
        return lstrtn;
    }

    private void SortProgress(object dayprogress)
    {
        List<string> lstProgress = (List<string>)dayprogress;
        int proNum = lstProgress.Count / 2;
        {

            for (int i = 0; i < proNum - 1; i++)
            {
                for (int j = 0; j < proNum - 1; j++)
                {

                    if (StringTimeCompare(lstProgress[2 * j], lstProgress[2 * (j + 1)]) > 0)
                    {
                        string tempdayfrom = lstProgress[2 * j];
                        string tempdayto = lstProgress[2 * j + 1];

                        lstProgress[2 * j] = lstProgress[2 * (j + 1)];
                        lstProgress[2 * j + 1] = lstProgress[2 * (j + 1) + 1];
                        lstProgress[2 * (j + 1)] = tempdayfrom;
                        lstProgress[2 * (j + 1) + 1] = tempdayto;
                    }

                }
            }
        }
    }


    private object FormatProgress(object dayprogress)
    {
        List<string> lstProgress = (List<string>)dayprogress;
        int proNum = lstProgress.Count / 2;
        
        for (int i = 0; i < proNum - 1; i ++)
        {
            for (int j = i + 1; j < proNum; j++)
            {
                if (StringTimeCompare(lstProgress[2 * i], lstProgress[2 * j + 1]) * StringTimeCompare(lstProgress[2 * i + 1], lstProgress[2 * j]) <= 0)
                {
                    string newtimefrom = "";
                    string newtimeto = "";
                    string oldtimefrom1 = lstProgress[2 * i]; 
                    string oldtimeto1 = lstProgress[2 * i + 1];
                    string oldtimefrom2 = lstProgress[2 * j];
                    string oldtimeto2 = lstProgress[2 * j + 1];
                    if (StringTimeCompare(lstProgress[2 * i], lstProgress[2 * j]) < 0)
                    {
                        newtimefrom = lstProgress[2 * i];
                    }
                    else
                    {
                        newtimefrom = lstProgress[2 * j];
                    }
                    if (StringTimeCompare(lstProgress[2 * i + 1], lstProgress[2 * j + 1]) > 0)
                    {
                        newtimeto = lstProgress[2 * i + 1];
                    }
                    else
                    {
                        newtimeto = lstProgress[2 * j + 1];
                    }

                    lstProgress.Remove(oldtimefrom1);
                    lstProgress.Remove(oldtimeto1);
                    lstProgress.Remove(oldtimefrom2);
                    lstProgress.Remove(oldtimeto2);
                    lstProgress.Add(newtimefrom);
                    lstProgress.Add(newtimeto);
                    return FormatProgress(lstProgress);
                }
            }
        }
        return lstProgress;
    }

    private int StringTimeCompare(string str1, string str2)
    {
        int hour1 = int.Parse(str1.Substring(0, str1.IndexOf(':')));
        int hour2 = int.Parse(str2.Substring(0, str2.IndexOf(':')));

        if (hour1 < hour2)
        {
            return -1;
        }
        else if (hour1 > hour2)
        {
            return 1;
        }
        else
        {
            int minute1 = int.Parse(str1.Substring(str1.IndexOf(':') + 1));
            int minute2 = int.Parse(str2.Substring(str2.IndexOf(':') + 1));
            if (minute1 < minute2)
            {
                return -1;
            }
            else if (minute1 > minute2)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
    }

    private object InitialProgressDay(DateTime day)
    {
        List<string> lstrtn = new List<string>();
        
        int rowNum = wdsDay.RealDataSet.Tables[dataMember].Rows.Count;
        DataTable dt = wdsDay.RealDataSet.Tables[dataMember];
      
            //switch (dateType)
            //{
            //    case "DayMode":
            //        {
            //            for (int i = 0; i < rowNum; i++)
            //            {
            //                DateTime dayfrom = DateTime.Parse(dt.Rows[i][dateFromField].ToString());
            //                DateTime datto = DateTime.Parse(dt.Rows[i][dateToField].ToString());

            //                if (day >= dayfrom && day <= datto)
            //                {
            //                    lstrtn.Add(dt.Rows[i][timeFromField].ToString());
            //                    lstrtn.Add(dt.Rows[i][timeToField].ToString());
            //                }
            //            }
            //            break;        
            //        }
            //    case "WeekMode":
            //        {
            //            for (int i = 0; i < rowNum; i++)
            //            {
            //                if (day.DayOfWeek == GetDayOfWeek(dt.Rows[i][weekField].ToString()))
            //                {
            //                    lstrtn.Add(dt.Rows[i][timeFromField].ToString());
            //                    lstrtn.Add(dt.Rows[i][timeToField].ToString());
            //                }
            //            }
            //            break;
            //        }//
            //    case "MonthMode":
            //        {
            //            for (int i = 0; i < rowNum; i++)
            //            {
            //                if (day.Day == int.Parse(dt.Rows[i][monthField].ToString()))
            //                {
            //                    lstrtn.Add(dt.Rows[i][timeFromField].ToString());
            //                    lstrtn.Add(dt.Rows[i][timeToField].ToString());
            //                }
            //            }
            //            break;
            //        }
            //}
        #region NewModify
       
        for (int i = 0; i < rowNum; i++)
        {
            if (dt.Columns.Contains(dateFromField) && dt.Columns.Contains(dateToField)
                 && dt.Rows[i][dateFromField].ToString() != string.Empty && dt.Rows[i][dateToField].ToString() != string.Empty)
            {
                  DateTime dayfrom = DateTime.Parse(dt.Rows[i][dateFromField].ToString());
                  DateTime datto = DateTime.Parse(dt.Rows[i][dateToField].ToString());

                  if (day >= dayfrom && day <= datto)
                  {
                      if (dt.Columns[timeFromField].DataType != typeof(string) && dt.Columns[timeToField].DataType != typeof(string))
                      {
                          lstrtn.Add(dt.Rows[i][timeFromField].ToString().Substring(0, dt.Rows[i][timeFromField].ToString().Length - 2) + ":"
                          + dt.Rows[i][timeFromField].ToString().Substring(dt.Rows[i][timeFromField].ToString().Length - 2));
                          lstrtn.Add(dt.Rows[i][timeToField].ToString().Substring(0, dt.Rows[i][timeToField].ToString().Length - 2) + ":"
                          + dt.Rows[i][timeToField].ToString().Substring(dt.Rows[i][timeToField].ToString().Length - 2));
                      }
                      else
                      {
                          lstrtn.Add(dt.Rows[i][timeFromField].ToString());
                          lstrtn.Add(dt.Rows[i][timeToField].ToString());
                      }
                  }
               
            }
            if (dt.Columns.Contains(dateFromField) && dt.Rows[i][dateFromField].ToString() != string.Empty 
                && (!dt.Columns.Contains(dateToField) || dt.Rows[i][dateToField].ToString() == string.Empty))
            {
                DateTime dayfrom = DateTime.Parse(dt.Rows[i][dateFromField].ToString());

                if (day.ToShortDateString() == dayfrom.ToShortDateString())
                {
                    if (dt.Columns[timeFromField].DataType != typeof(string) && dt.Columns[timeToField].DataType != typeof(string))
                    {
                        lstrtn.Add(dt.Rows[i][timeFromField].ToString().Substring(0, dt.Rows[i][timeFromField].ToString().Length - 2) + ":"
                        + dt.Rows[i][timeFromField].ToString().Substring(dt.Rows[i][timeFromField].ToString().Length - 2));
                        lstrtn.Add(dt.Rows[i][timeToField].ToString().Substring(0, dt.Rows[i][timeToField].ToString().Length - 2) + ":"
                        + dt.Rows[i][timeToField].ToString().Substring(dt.Rows[i][timeToField].ToString().Length - 2));
                    }
                    else
                    {
                        lstrtn.Add(dt.Rows[i][timeFromField].ToString());
                        lstrtn.Add(dt.Rows[i][timeToField].ToString());
                    }
                }
            }
            if (dt.Columns.Contains(weekField) && dt.Rows[i][weekField].ToString() != string.Empty)
            { 
                 if (day.DayOfWeek == GetDayOfWeek(dt.Rows[i][weekField].ToString()))
                 {
                     if (dt.Columns[timeFromField].DataType != typeof(string))
                     {
                         lstrtn.Add(dt.Rows[i][timeFromField].ToString().Substring(0, dt.Rows[i][timeFromField].ToString().Length - 2) + ":"
                         + dt.Rows[i][timeFromField].ToString().Substring(dt.Rows[i][timeFromField].ToString().Length - 2));
                         lstrtn.Add(dt.Rows[i][timeToField].ToString().Substring(0, dt.Rows[i][timeToField].ToString().Length - 2) + ":"
                         + dt.Rows[i][timeToField].ToString().Substring(dt.Rows[i][timeToField].ToString().Length - 2));
                     }
                     else
                     {
                         lstrtn.Add(dt.Rows[i][timeFromField].ToString());
                         lstrtn.Add(dt.Rows[i][timeToField].ToString());
                     }
                 }
            }
            if (dt.Columns.Contains(monthField) && dt.Rows[i][monthField].ToString() != string.Empty)
            {
                if (day.Day == int.Parse(dt.Rows[i][monthField].ToString()))
                {
                    if (dt.Columns[timeFromField].DataType != typeof(string))
                    {
                        lstrtn.Add(dt.Rows[i][timeFromField].ToString().Substring(0, dt.Rows[i][timeFromField].ToString().Length - 2) + ":"
                        + dt.Rows[i][timeFromField].ToString().Substring(dt.Rows[i][timeFromField].ToString().Length - 2));
                        lstrtn.Add(dt.Rows[i][timeToField].ToString().Substring(0, dt.Rows[i][timeToField].ToString().Length - 2) + ":"
                        + dt.Rows[i][timeToField].ToString().Substring(dt.Rows[i][timeToField].ToString().Length - 2));
                    }
                    else
                    {
                        lstrtn.Add(dt.Rows[i][timeFromField].ToString());
                        lstrtn.Add(dt.Rows[i][timeToField].ToString());
                    }
                }
            }
        } 
        #endregion

        return lstrtn;
    }

    private DayOfWeek GetDayOfWeek(string strday)
    {
        DayOfWeek rtn;
        try
        {
            if (strday.Length == 1)
            {
                int value = Convert.ToInt32(strday) % 7;
                rtn = (DayOfWeek)value;
            }
            else
            {
                rtn = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), strday, true);
            }
        }
        catch
        {
            rtn = DayOfWeek.Sunday;
        }
        return rtn;
    }

    private void CreatDataSet(object datasetid)
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

                    wdsDay = wds;
                    if (strWhere != string.Empty)
                    {
                        wdsDay.SetWhere(strWhere);
                    }


                }
            }
        }

    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        DateTime newday = new DateTime(int.Parse(ddlYear.SelectedValue),int.Parse(ddlMonth.SelectedValue),1);
        string querystring = this.Page.Request.QueryString.ToString();
        querystring = querystring.Replace("Day=" + strDay, "Day=" + newday.ToShortDateString().Replace('/', '-'));
        this.Page.Response.Redirect(this.Page.Request.Path + "?" + querystring);
    }
    
    protected void lbPrevious_Click(object sender, EventArgs e)
    {
        if (dtday.AddMonths(-1).Year > 1950)
        {
            dtday = dtday.AddMonths(-1);
            DateTime newday = new DateTime(dtday.Year, dtday.Month, 1);
            string querystring = this.Page.Request.QueryString.ToString();
            querystring = querystring.Replace("Day=" + strDay, "Day=" + newday.ToShortDateString().Replace('/', '-'));
            this.Page.Response.Redirect(this.Page.Request.Path + "?" + querystring);
        }
        else
        {
            string querystring = this.Page.Request.QueryString.ToString();
            this.Page.Response.Redirect(this.Page.Request.Path + "?" + querystring);
        }
    }
   
    protected void lbNext_Click(object sender, EventArgs e)
    {
        if (dtday.AddMonths(1).Year < 2051)
        {
            dtday = dtday.AddMonths(1);
            DateTime newday = new DateTime(dtday.Year, dtday.Month, 1);
            string querystring = this.Page.Request.QueryString.ToString();
            querystring = querystring.Replace("Day=" + strDay, "Day=" + newday.ToShortDateString().Replace('/', '-'));
            this.Page.Response.Redirect(this.Page.Request.Path + "?" + querystring);
        }
        else
        {
            string querystring = this.Page.Request.QueryString.ToString();
            this.Page.Response.Redirect(this.Page.Request.Path + "?" + querystring);
        }
    }
}
