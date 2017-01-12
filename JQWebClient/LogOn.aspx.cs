using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using EFClientTools.EFServerReference;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web.UI.HtmlControls;
using System.Net;

public partial class LogOn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {

                EFServiceClient client = EFClientTools.ClientUtility.Client;
                EFClientTools.ClientUtility.ServerIPAddress = client.GetServerIPAddress();
                client = EFClientTools.ClientUtility.Client;
                if (!string.IsNullOrEmpty(DefaultDatabase))
                {
                    (Login1.FindControl("Database") as DropDownList).Visible = false;
                    (Login1.FindControl("DatabaseLabel") as Label).Visible = false;
                }
                else
                {
                    var listDatabase = client.GetDatabases(null);
                    foreach (var database in listDatabase)
                    {
                        (Login1.FindControl("Database") as DropDownList).Items.Add(database);
                    }
                }
                if (!string.IsNullOrEmpty(DefaultSolution))
                {
                    (Login1.FindControl("Solution") as DropDownList).Visible = false;
                    (Login1.FindControl("SolutionLabel") as Label).Visible = false;
                }
                else
                {
                    var locale = Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : "en-us";
                    var ipAddress = Request.UserHostAddress;

                    //object[] myRets = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { SelectedDataBase });
                    //String databaseType = String.Empty;
                    //if (myRets[0].ToString() == "0")
                    //{
                    //    databaseType = myRets[1].ToString();
                    //}

                    var clientInfo = new ClientInfo()
                    {
                        UserID = (Login1.FindControl("UserName") as TextBox).Text,
                        Password = (Login1.FindControl("Password") as TextBox).Text,
                        Database = SelectedDataBase,
                        Solution = SelectedSolution,
                        IPAddress = ipAddress,
                        Locale = locale,
                        UseDataSet = true,
                        DatabaseType = "1"
                    };
                    var listSolution = client.GetSolutions(clientInfo);
                    foreach (var solution in listSolution)
                    {
                        (Login1.FindControl("Solution") as DropDownList).Items.Add(new ListItem(solution.Name, solution.ID));
                    }

                }
                //EFBase.MessageProvider provider = new EFBase.MessageProvider(System.Web.HttpContext.Current.Request.PhysicalApplicationPath, Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : "en-us");
                //SetLabelText(provider);
                LoadCookie();

            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                string messageKey = "EEPWebNetClient/WinSysMsg/msg_CanNotFindServer";
                var locale = Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : "en-us";
                EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Request.PhysicalApplicationPath, locale);
                (Login1.FindControl("FailureLabel") as Literal).Text = string.Format("<font color=\"red\">{0}</font>", provider[messageKey]);
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
        var locale2 = Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : "en-us";
        EFBase.MessageProvider provider2 = new EFBase.MessageProvider(this.Request.PhysicalApplicationPath, locale2);
        SetLabelText(provider2);
    }

    private void SetLabelText(EFBase.MessageProvider provider)
    {
        string messagekey = "EEPWebNetClient/WinSysMsg/txt_Login";
        string content = provider[messagekey];
        if (!string.IsNullOrEmpty(content))
        {
            var tooltiplist = content.Split(';');

            if (tooltiplist.Length >= 1)
            {
                (Login1.FindControl("UserNameLabel") as Label).Text = tooltiplist[0];
                (Login1.FindControl("PasswordLabel") as Label).Text = tooltiplist[1];
                (Login1.FindControl("DatabaseLabel") as Label).Text = tooltiplist[2];
                (Login1.FindControl("SolutionLabel") as Label).Text = tooltiplist[3];
                (Login1.FindControl("Remember") as CheckBox).Text = tooltiplist[4];
                (Login1.FindControl("LoginButton") as Button).Text = tooltiplist[6];
                (Login1.FindControl("Logout") as Button).Text = tooltiplist[8];

                String loginVerify = ConfigurationManager.AppSettings["IsLoginVerify"];
                String userIP = GetIP4Address();
                String loginVerifyIP = ConfigurationManager.AppSettings["LoginVerifyIP"];
                if (String.IsNullOrEmpty(loginVerify) || loginVerify == "false" || (!String.IsNullOrEmpty(loginVerifyIP) && userIP.StartsWith(loginVerifyIP)))
                {
                    (Login1.FindControl("checkcodetr") as HtmlTableRow).Visible = false;
                }
                else
                {
                    (Login1.FindControl("lbCheckCode") as Label).Text = tooltiplist[5];
                    String VNum = hfNValue.Value;
                    String ONum = hfOValue.Value;

                    ONum = VNum;
                    VNum = MakeValidateCode();
                    Bitmap validateimage;
                    Graphics g;
                    Graphics tmpG = Graphics.FromImage(new Bitmap(1, 1));
                    tmpG.PageUnit = GraphicsUnit.Pixel;
                    SizeF size = tmpG.MeasureString(VNum, new Font("Verdana", 13));
                    validateimage = new Bitmap((int)size.Width + 10, (int)size.Height - 4, PixelFormat.Format24bppRgb);
                    g = Graphics.FromImage(validateimage);
                    g.DrawString(VNum, new Font("Verdana", 13), new SolidBrush(Color.White), new PointF(8, -2));
                    g.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(110, 20), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 255, 60, 40)), 0, 0, 120, 30);
                    g.Save();
                    MemoryStream ms = new MemoryStream();
                    validateimage.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                    String imagePath = this.Page.Request.ApplicationPath;
                    imagePath = imagePath.Remove(imagePath.LastIndexOf('/'));
                    String mapPath = this.Page.MapPath(imagePath);
                    if (!Directory.Exists(mapPath + "\\InfolightTemp"))
                    {
                        Directory.CreateDirectory(mapPath + "\\InfolightTemp");
                    }
                    else
                    {
                        String[] files = Directory.GetFiles(mapPath + "\\InfolightTemp");
                        for (int i = 0; i < files.Length; i++)
                        {
                            File.Delete(files[i]);
                        }
                    }
                    Guid filename = new Guid();
                    mapPath += "\\InfolightTemp\\" + filename + ".gif";
                    imagePath = "~/InfolightTemp/" + filename + ".gif";

                    FileStream f = new FileStream(mapPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    ms.WriteTo(f);
                    f.Close();
                    f.Dispose();
                    ms.Close();
                    (Login1.FindControl("Image1") as System.Web.UI.WebControls.Image).ImageUrl = imagePath;

                    hfNValue.Value = VNum;
                    hfOValue.Value = ONum;
                }
            }
        }
    }

    private string GetIP4Address()
    {
        string IP4Address = String.Empty;

        foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
        {
            if (IPA.AddressFamily.ToString() == "InterNetwork")
            {
                IP4Address = IPA.ToString();
                break;
            }
        }

        if (IP4Address != String.Empty)
        {
            return IP4Address;
        }

        foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
        {
            if (IPA.AddressFamily.ToString() == "InterNetwork")
            {
                IP4Address = IPA.ToString();
                break;
            }
        }

        return IP4Address;
    }

    private string MakeValidateCode()
    {
        char[] s = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        string num = "";
        Random r = new Random();
        for (int i = 0; i < 4; i++)
            num += s[r.Next(0, s.Length)].ToString();
        return num;
    }

    private string DefaultDatabase
    {
        get
        {
            return ConfigurationManager.AppSettings["DefaultDatabase"];
        }
    }

    private string DefaultSolution
    {
        get
        {
            return ConfigurationManager.AppSettings["DefaultSolution"];
        }
    }

    private string SelectedDataBase
    {
        get
        {
            if (string.IsNullOrEmpty(DefaultDatabase))
            {
                var dropdownlistDatabase = Login1.FindControl("Database") as DropDownList;
                return dropdownlistDatabase.SelectedValue;
            }
            else
            {
                return DefaultDatabase;
            }
        }
    }

    private string SelectedSolution
    {
        get
        {
            if (string.IsNullOrEmpty(DefaultSolution))
            {
                var dropdownlistSolution = Login1.FindControl("Solution") as DropDownList;
                return dropdownlistSolution.SelectedValue;
            }
            else
            {
                return DefaultSolution;
            }
        }
    }

    private bool IsDataSet
    {
        get
        {
            return ConfigurationManager.AppSettings["IsDataSet"] != null && ConfigurationManager.AppSettings["IsDataSet"].Equals("true", StringComparison.OrdinalIgnoreCase);
        }
    }

    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        Session.Timeout = 60;
        EFServiceClient client = EFClientTools.ClientUtility.Client;
        var locale = Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : "en-us";
        var ipAddress = Request.UserHostAddress;
        if ((Login1.FindControl("tbCheckCode") as TextBox).Text != hfOValue.Value)
        {
            string messageKey = "EEPWebNetClient/WinSysMsg/msg_CheckCode";
            EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Request.PhysicalApplicationPath, locale);
            (Login1.FindControl("FailureLabel") as Literal).Text = string.Format("<font color=\"red\">{0}</font>", provider[messageKey]);
            e.Authenticated = false;
            return;
        }
        //CliUtils.fClientSystem = "Web";
        //CliUtils.fLoginUser = (Login1.FindControl("UserName") as TextBox).Text;
        //CliUtils.fLoginPassword = (Login1.FindControl("Password") as TextBox).Text;
        //CliUtils.fLoginDB = SelectedDataBase;
        //CliUtils.fCurrentProject = SelectedSolution;

        //CliUtils.fComputerIp = serverIPAddress;

        //object[] myRets = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB });
        //String databaseType = String.Empty;
        //if (myRets[0].ToString() == "0")
        //{
        //    databaseType = myRets[1].ToString();
        //}
        //CliUtils.fLoginDBType = (ClientType)Enum.Parse(typeof(ClientType), databaseType);

        var clientInfo = new ClientInfo()
        {
            UserID = (Login1.FindControl("UserName") as TextBox).Text,
            Password = (Login1.FindControl("Password") as TextBox).Text,
            Database = SelectedDataBase,
            Solution = SelectedSolution,
            IPAddress = ipAddress,
            Locale = locale,
            //ServerIPAddress = serverIPAddress,
            //UseDataSet = IsDataSet,
            UseDataSet = true
            //DatabaseType = databaseType
        };

        try
        {
            var result = new ClientInfo();
            if (clientInfo.UserID.Contains("'"))
            {
                result = new ClientInfo() { LogonResult = LogonResult.UserNotFound };
            }
            else
            {
                result = client.LogOn(clientInfo);
            }
            if (result.LogonResult == LogonResult.Logoned)
            {
                result.Locale = Request.UserLanguages.FirstOrDefault();
                Session["ClientInfo"] = result;
                e.Authenticated = true;
                SaveCookie(clientInfo);
                //this.Response.Redirect("Test/JQSingleTest.aspx"); 
                //this.Response.Redirect("Test/MDTest2.aspx");
                //String flow = ConfigurationManager.AppSettings["IsFlow"];
                //if (String.IsNullOrEmpty(flow))
                //    this.Response.Redirect("MainPage.aspx");
                //else if (flow.ToLower() == "true")
                //{
                //    this.Response.Redirect("MainPage_Flow.aspx");
                //    this.Page.Session["IsFlow"] = flow;
                //}
                //else
                //{
                //    this.Response.Redirect("MainPage.aspx");
                //}

                var obj = client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "GLModule", "GetPasswordLastDate", new List<object>() { result.UserID });
                if (obj != null && obj != "")
                {
                    var date = DateTime.ParseExact(obj.ToString(), "yyyyMMdd", null);
                    DateTime today = DateTime.Today;
                    String value = "";
                    TimeSpan ts = today - date;
                    value = ts.TotalDays.ToString();
                    var obj2 = client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "GLModule", "GetLoginFile", new List<object>() { new DateTime() });
                    var PasswordPolicy = obj2.ToString();
                    var Expiry = 0;
                    try
                    {
                        System.Text.RegularExpressions.Regex rpwe = new System.Text.RegularExpressions.Regex("PassWrodExpiry=\"\\d*\"");
                        if (rpwe.IsMatch(PasswordPolicy))
                        {
                            var spwe = rpwe.Match(PasswordPolicy).Value;
                            Int32.TryParse(spwe.Substring(16, spwe.Length - 17), out Expiry);
                        }
                    }

                    finally { }
                    if (Expiry > 0 && Convert.ToInt32(value) > Expiry)
                    {
                        string messageKey = "EEPNetClient/FrmClientMain/PasswordAnnulment";
                        EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Request.PhysicalApplicationPath, locale);
                        ClientScript.RegisterStartupScript(this.GetType(), new Guid().ToString(), "alert('" + provider[messageKey] + "');window.location.href='MainPage_Flow.aspx';", true);
                        e.Authenticated = false;
                        return;
                    }
                }
                this.Response.Redirect("MainPage_Flow.aspx");
                return;

                //if (this.Page.Request["IsReportManager"] != null && string.Compare(this.Page.Request["IsReportManager"].ToString(), "true", true) == 0)
                //{
                //    this.Response.Redirect("SLEasilyReportManager.Web/ReportManagerMain.aspx");
                //}
                //else
                //{
                //    String flow = ConfigurationManager.AppSettings["IsFlow"];
                //    if (String.IsNullOrEmpty(flow))
                //        this.Response.Redirect("Main2.aspx");
                //    else if (flow.ToLower() == "true")
                //    {
                //        //this.Response.Redirect("Main_Flow.aspx"); 
                //        this.Response.Redirect("Main_Flow2.aspx");
                //        this.Page.Session["IsFlow"] = flow;
                //    }
                //    else
                //    {
                //        this.Response.Redirect("Main2.aspx");
                //    }
                //}
            }
            else
            {
                string messageKey = string.Empty;
                switch (result.LogonResult)
                {
                    case LogonResult.UserNotFound: messageKey = "EEPWebNetClient/WinSysMsg/msg_UserNotFound"; break;
                    case LogonResult.UserDisabled: messageKey = "EEPWebNetClient/WinSysMsg/msg_UserDisabled"; break;
                    case LogonResult.PasswordError: messageKey = "EEPWebNetClient/WinSysMsg/msg_UserOrPasswordError"; break;
                    default: messageKey = "EEPWebNetClient/WinSysMsg/msg_UserOrPasswordError"; break;
                }
                EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Request.PhysicalApplicationPath, locale);
                (Login1.FindControl("FailureLabel") as Literal).Text = string.Format("<font color=\"red\">{0}</font>", provider[messageKey]);
                e.Authenticated = false;
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string messageKey = "EEPWebNetClient/WinSysMsg/msg_CanNotFindServer";
            EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Request.PhysicalApplicationPath, locale);
            (Login1.FindControl("FailureLabel") as Literal).Text = string.Format("<font color=\"red\">{0}</font>", provider[messageKey]);
            e.Authenticated = false;
        }
    }

    private void SaveCookie(ClientInfo clientInfo)
    {
        SetCookie("username", clientInfo.UserID);
        var checkRemember = Login1.FindControl("Remember") as CheckBox;
        if (checkRemember.Checked)
        {
            SetCookie("password", HttpUtility.UrlEncode( GetPwdString(clientInfo.Password)));
        }
        else
        {
            SetCookie("password", null);
        }
        SetCookie("database", clientInfo.Database);
        SetCookie("solution", clientInfo.Solution);
    }

    private string GetPwdString(string s)
    {
        string sRet = "";
        for (int i = 0; i < s.Length; i++)
        {
            sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
        }
        return sRet;
    }

    private void LoadCookie()
    {
        var userName = GetCookie("username");
        if (!string.IsNullOrEmpty(userName))
        {
            (Login1.FindControl("UserName") as TextBox).Text = userName;
        }
        var code = GetCookie("password");
        if (!string.IsNullOrEmpty(code))
        {
            (Login1.FindControl("Password") as TextBox).Attributes.Add("value", GetPwdString(System.Web.HttpUtility.UrlDecode(code)));
            (Login1.FindControl("Remember") as CheckBox).Checked = true;
        }
        var database = GetCookie("database");
        if (!string.IsNullOrEmpty(database))
        {
            (Login1.FindControl("Database") as DropDownList).SelectedValue = database;
        }
        var solution = GetCookie("solution");
        if (!string.IsNullOrEmpty(solution))
        {
            (Login1.FindControl("Solution") as DropDownList).SelectedValue = solution;
        }
    }

    private void SetCookie(string key, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Response.Cookies[key].Value = null;
        }
        else
        {
            Response.Cookies[key].Value = value;
            Response.Cookies[key].Expires = DateTime.Now.AddYears(1);
        }
    }

    private string GetCookie(string key)
    {
        var cookie = Request.Cookies[key];
        return cookie == null ? null : cookie.Value;
    }
    protected void Logout_Click(object sender, EventArgs e)
    {
        Page.Response.Redirect("InnerPages/ResetPWD.aspx?db=" + HttpUtility.UrlEncode(SelectedDataBase));
    }
}
