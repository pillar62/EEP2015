using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Configuration;
using EFClientTools.EFServerReference;
using System.Web;
using System.ComponentModel.Design;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace JQMobileTools
{
    [Designer(typeof(JQScriptManagerDesigner), typeof(IDesigner))]
    public class JQScriptManager : WebControl, IThemeObject
    {
        public JQScriptManager()
        {
            JQueryVersion = "1.8.0";
            JQueryMobileVersion = "1.3.2";
            theme = "b";
            HeaderTemplate = "../MobileHeaderTemplate.htm";
            FooterTemplate = "../MobileFooterTemplate.htm";
            SubFolder = true;
            UseQRCode = true;
            UseMetro = false;
            UseSignature = false;
            QueryString = new NameValueCollection();
        }

        [Category("Infolight")]
        public string JQueryVersion { get; set; }

        private string jqueryMobileVersion;
        [Category("Infolight")]
        public string JQueryMobileVersion
        {
            get
            {
                var configVersion = ConfigurationManager.AppSettings["JQueryMobileVersion"];
                if (!string.IsNullOrEmpty(configVersion))
                {
                    return configVersion;
                }
                else
                {
                    return jqueryMobileVersion;
                }
            }
            set
            {
                jqueryMobileVersion = value;
            }
        }

        [Category("Infolight")]
        public bool LocalScript { get; set; }

        private string theme;
        [Category("Infolight")]
        public string Theme
        {
            get
            {
                if (!string.IsNullOrEmpty(GlobalTheme))
                {
                    return GlobalTheme;
                }
                return theme;
            }
            set
            {
                theme = value;
            }
        }
        [Category("Infolight")]
        public bool CustomStyle { get; set; }

        private const string BAIDU_AK = "fF3EzrOwkhrd7fZWVzhQp2ah";
        private const string Google_AK = "AIzaSyBxzMQ7cxFAT74sE0alciq7aFKN2CTaNmg";

        [Category("Infolight")]
        public JQMapType UseMap { get; set; }

        [Category("Infolight")]
        public bool UseChartJS { get; set; }

        [Category("Infolight")]
        public bool UseQRCode { get; set; }

        [Category("Infolight")]
        public bool UseMetro { get; set; }
        [Category("Infolight")]
        public bool UseSignature { get; set; }

        public static string GlobalTheme;

        public string HeaderTemplate { get; set; }

        public string FooterTemplate { get; set; }

        public bool SubFolder { get; set; }


        private string agentUser;
        [Category("Infolight")]
        public string AgentUser {
            get {
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AgentUser"]))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["AgentUser"];
                }
                return agentUser;
            }
            set {
                agentUser = value;
            }
        }

        [Category("Infolight")]
        public string AgentDeveloper { get; set; }

        private string agentDatabase;
        [Category("Infolight")]
        public string AgentDatabase {

            get
            {
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AgentDatabase"]))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["AgentDatabase"];
                }
                return agentDatabase;
            }
            set
            {
                agentDatabase = value;
            }
        }

        private string agentSolution;
        [Category("Infolight")]
        public string AgentSolution {
            get
            {
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AgentSolution"]))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["AgentSolution"];
                }
                return agentSolution;
            }
            set
            {
                agentSolution = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //保存WF解密后的QueryString
        public NameValueCollection QueryString
        {
            get;
            set;
        }

        public static bool? IsRunTimeWebSite(HttpContext context)
        {
            var runTimeWebClient = System.Configuration.ConfigurationManager.AppSettings["RunTimeWebClient"];
            if (!string.IsNullOrEmpty(runTimeWebClient))
            {
                var runTimeWebs = runTimeWebClient.Split(';');
                var runtime = false;
                foreach (var web in runTimeWebs)
                {
                    if (context.Request.Url.ToString().Split('?')[0].IndexOf(web, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        runtime = true;
                        break;
                    }
                }
                return runtime;
            }
            else
            {
                return null;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //logon
            if (this.Page.Session["ClientInfo"] == null)
            {
                if (!string.IsNullOrEmpty(AgentUser) && !string.IsNullOrEmpty(AgentDatabase) && !string.IsNullOrEmpty(AgentSolution))
                {
                    var locale = this.Page.Request.UserLanguages.Length > 0 ? this.Page.Request.UserLanguages[0] : "en-us";
                    var clientInfo = new ClientInfo()
                    {
                        UserID = AgentUser,
                        Password = "",
                        Database = AgentDatabase,
                        Solution = AgentSolution,
                        IPAddress = this.Page.Request.UserHostAddress,
                        Locale = locale,
                        //ServerIPAddress = serverIPAddress,
                        //UseDataSet = IsDataSet,
                        UseDataSet = true
                        //DatabaseType = databaseType
                    };
                    var runTime = IsRunTimeWebSite(HttpContext.Current);
                    if (runTime.HasValue)
                    {
                        if (runTime.Value && !string.IsNullOrEmpty(AgentDeveloper))
                        {
                            clientInfo.SDDeveloperID = AgentDeveloper;

                        }
                        else
                        {
                            return;
                        }

                    }

                    EFServiceClient client = EFClientTools.ClientUtility.Client;
                    var result = client.LogOn(clientInfo);
                    if (result.LogonResult == LogonResult.Logoned)
                    {
                        if (!string.IsNullOrEmpty(AgentDeveloper))
                        {
                            result.IsSDModule = true;
                        }
                        this.Page.Session["ClientInfo"] = result;
                    }
                }
                //else 
                //{
                //    var locale = this.Page.Request.UserLanguages.Length > 0 ? this.Page.Request.UserLanguages[0] : "en-us";
                //    var clientInfo = new ClientInfo()
                //    {
                //        UserID = "001",
                //        Password = "",
                //        Database = "EEP",
                //        Solution = "Solution1",
                //        IPAddress = this.Page.Request.UserHostAddress,
                //        Locale = locale,
                //        //ServerIPAddress = serverIPAddress,
                //        //UseDataSet = IsDataSet,
                //        UseDataSet = true
                //        //DatabaseType = databaseType
                //    };


                //    EFServiceClient client = EFClientTools.ClientUtility.Client;
                //    var result = client.LogOn(clientInfo);
                //    if (result.LogonResult == LogonResult.Logoned)
                //    {

                //        this.Page.Session["ClientInfo"] = result;
                //    }
                //}
            }

            var key = this.Page.Request.QueryString["key"];
            var param = this.Page.Request.QueryString["param"];
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(param))
            {
                var queryString = DecryptParameters(param, key);
                var queryStrs = queryString.Split('&');
                foreach (var queryStr in queryStrs)
                {
                    var index = queryStr.IndexOf('=');
                    if (index > 0 && index < queryStr.Length - 1)
                    {
                        QueryString[queryStr.Substring(0, index)] = System.Web.HttpUtility.UrlDecode(queryStr.Substring(index + 1));
                    }
                }
            }
            else
            {
                var queryNames = new string[] { "FLOWFILENAME", "NAVMODE", "FLNAVMODE" };
                foreach (var queryName in queryNames)
                {
                    if (this.Page.Request.QueryString[queryName] != null)
                    {
                        QueryString[queryName] = this.Page.Request.QueryString[queryName];
                    }
                }
            }
        }

        public static string DecryptParameters(string param, string key)
        {
            var paramters = Convert.FromBase64String(param);
            var encryptCodes = Convert.FromBase64String(key);
            for (int i = 0; i < paramters.Length; i++)
            {
                var encryptCode = encryptCodes[i % encryptCodes.Length];
                paramters[i] -= encryptCode;
            }
            var hash = paramters.Take(16).ToArray();
            var info = paramters.Skip(16).ToArray();
            var md5 = new MD5CryptoServiceProvider();
            if (Convert.ToBase64String(md5.ComputeHash(info)) != Convert.ToBase64String(hash))
            {
                throw new Exception("param is invalid");
            }
            return Encoding.UTF8.GetString(info);
        }


        protected override void OnPreRender(EventArgs e)
        {
            if (!this.DesignMode && !this.Page.IsPostBack)
            {
                if (this.Page.Request.QueryString["Cordova"] == "true")
                {
                    if (UseSignature)
                    {
                        var chartScripts = new string[] {
                        "../js/jSignature/jSignature.min.js"
                        };
                        AddClientScripts(chartScripts);
                    }
                    if (UseMap == JQMapType.Google)
                    {
                        AddClientScript(string.Format("http://maps.google.com/maps/api/js?key={0}&signed_in=true&libraries=places&", Google_AK));
                    }
                    else if (UseMap == JQMapType.Baidu)
                    {
                        AddClientScript(string.Format("http://api.map.baidu.com/api?type=quick&ak={0}&v=2.0", BAIDU_AK));
                    }
                    //qrcode js 
                    if (UseQRCode)
                    {
                        var qrcodeScript = "../js/jquery-qrcode-master/jquery.qrcode.min.js";
                        AddClientScripts(new string[] { qrcodeScript });
                    }
                    if (UseChartJS)
                    {
                        var chartScripts = new string[] {
                        "../js/jqplot/jquery.jqplot.min.js",
                        "../js/jqplot/plugins/jqplot.barRenderer.min.js",
                        "../js/jqplot/plugins/jqplot.categoryAxisRenderer.min.js",
                        "../js/jqplot/plugins/jqplot.canvasTextRenderer.min.js",
                        "../js/jqplot/plugins/jqplot.canvasAxisLabelRenderer.min.js",
                        "../js/jqplot/plugins/jqplot.pointLabels.min.js",
                        "../js/jqplot/plugins/jqplot.pieRenderer.min.js",
                        "../js/jqplot/plugins/jqplot.donutRenderer.min.js",
                        "../js/jqplot/plugins/jqplot.meterGaugeRenderer.min.js",
                        "../js/jqplot/plugins/jqplot.pointLabels.min.js",
                        "../js/jqplot/excanvas.min.js",
                        "../js/jquery.infolight.mobile.chart.js"
                        };
                        AddClientScripts(chartScripts);
                        AddCss("../jqplot/jquery.jqplot.min.css");

                    }
                    var clientScripts = new string[] {
                        "../cordova.js",
                        "../scripts/platformOverrides.js",
                        "../scripts/index.js",
                        "../js/jquery-1.11.3.js",
                        "../js/jquery.mobile-1.4.5/jquery.mobile-1.4.5.min.js",
                        "../js/jquery.json.js",
                        "../js/jquery.infolight.mobile.js",
                        "../js/jquery.push.js",
                        "../js/jquery.infolight.mobile.wf.js"
                    };
                    AddClientScripts(clientScripts);

                    var styleSheets = new string[] {
                      "../css/index.css",
                      "../js/jquery.mobile-1.4.5/jquery.mobile-1.4.5.min.css",
                      "../css/themes/my-custom-theme.min.css",
                      "../css/themes/jquery.mobile.icons.min.css"
                    };
                    foreach (var sheet in styleSheets)
                    {
                        if (!IsCssRegistered(sheet))
                        {
                            AddCss(sheet);
                        }
                    }
                    //this.HeaderTemplate = "../CordovaHeaderTemplate.htm";
                }
                else
                {

                    //chart js 
                    if (UseChartJS)
                    {
                        var chartScripts = new string[] {
                        "../jqplot/jquery.jqplot.min.js",
                        "../jqplot/plugins/jqplot.barRenderer.min.js",
                        "../jqplot/plugins/jqplot.categoryAxisRenderer.min.js",
                        "../jqplot/plugins/jqplot.canvasTextRenderer.min.js",
                        "../jqplot/plugins/jqplot.canvasAxisLabelRenderer.min.js",
                        "../jqplot/plugins/jqplot.pointLabels.min.js",
                        "../jqplot/plugins/jqplot.pieRenderer.min.js",
                        "../jqplot/plugins/jqplot.donutRenderer.min.js",
                        "../jqplot/plugins/jqplot.meterGaugeRenderer.min.js",
                        "../jqplot/plugins/jqplot.pointLabels.min.js",
                        "../jqplot/excanvas.min.js",
                        "../js/jquery.infolight.mobile.chart.js"
                        };
                        AddClientScripts(chartScripts);
                        AddCss("../jqplot/jquery.jqplot.min.css");

                    }
                    if (UseSignature)
                    {
                        var chartScripts = new string[] {
                        "../js/jSignature/jSignature.min.js"
                        };
                        AddClientScripts(chartScripts);
                    }
                    var subPath = SubFolder ? "../" : string.Empty;

                    AddMeta("viewport", "width=device-width, initial-scale=1");

                    var styleSheetsJQMetro = new string[] {
                        //"../assets/css/bootmetro.css",
                        subPath+ "assets/css/bootmetro-responsive.css",
                        //subPath+ "assets/css/bootmetro-icons.css",
                        subPath+ "assets/css/bootmetro-ui-light.css",
                        subPath+ "assets/css/datepicker.css"
                    };
                    foreach (var sheet in styleSheetsJQMetro)
                    {
                        if (!IsCssRegistered(sheet))
                        {
                            AddCss(sheet);
                        }
                    }
                    var linkMobile = new HtmlLink() { Href = subPath + "assets/css/bootmetroMobile.css" };
                    linkMobile.Attributes["rel"] = "stylesheet";
                    linkMobile.Attributes["media"] = "screen and (max-width: 640px)";
                    this.Page.Header.Controls.Add(linkMobile);
                    var link = new HtmlLink() { Href = subPath + "assets/css/bootmetro.css" };
                    link.Attributes["rel"] = "stylesheet";
                    link.Attributes["media"] = "screen and (min-width: 640px)";
                    this.Page.Header.Controls.Add(link);

                    var clientScriptsJQMetro = new string[] {
                    subPath+ "assets/js/min/bootstrap.min.js",
                    subPath+ "assets/js/bootmetro-panorama.js",
                    subPath+ "assets/js/bootmetro-pivot.js",
                    subPath+ "assets/js/bootmetro-charms.js",
                    subPath+ "assets/js/bootstrap-datepicker.js",
                    subPath+ "assets/js/jquery.mousewheel.js",
                    subPath+ "assets/js/jquery.touchSwipe.js"
                    };
                    AddClientScripts(clientScriptsJQMetro);

                    String MobileShowMetro = ConfigurationManager.AppSettings["MobileShowMetro"];
                    if (String.IsNullOrEmpty(MobileShowMetro) || MobileShowMetro.ToLower() == "false")
                    {
                        AddClientScripts(new string[] { "assets/js/HideMetroForMobile.js" });
                    }
                    else if (MobileShowMetro.ToLower() == "true")
                    {
                        AddClientScripts(new string[] { "assets/js/HideMenuForMobile.js" });
                    }


                    var jqueryMobileCss = LocalScript ? subPath + string.Format("jquery.mobile-{0}/jquery.mobile-{0}.min.css", JQueryMobileVersion)
                        : string.Format("http://code.jquery.com/mobile/{0}/jquery.mobile-{0}.min.css", JQueryMobileVersion);
                    //官方更新了导致按钮都出不来了
                    //var datecss = LocalScript ? subPath + "js/datebox/jqm-datebox.min.css" : "http://dev.jtsage.com/cdn/datebox/latest/jqm-datebox.min.css";
                    var datecss = subPath + "js/datebox/jqm-datebox.min.css";

                    var styleSheets = new string[] {
                        jqueryMobileCss,
                        datecss,
                        subPath + "js/themes/infolight.mobile.css",
                        subPath  + string.Format("jquery.mobile-{0}/infolight.mobile-{0}.css", JQueryMobileVersion),
                        subPath + string.Format("js/jquery.mobile-{0}-modify.css", JQueryMobileVersion)
                    };
                    foreach (var sheet in styleSheets)
                    {
                        if (!IsCssRegistered(sheet))
                        {
                            AddCss(sheet);
                        }
                    }


                    var jqueryScript = LocalScript ? subPath + string.Format("js/jquery-{0}.min.js", JQueryVersion)
                      : string.Format("http://code.jquery.com/jquery-{0}.min.js", JQueryVersion);
                    var jqueryMobileScript = LocalScript ? subPath + string.Format("jquery.mobile-{0}/jquery.mobile-{0}.js", JQueryMobileVersion)
                      : string.Format("http://code.jquery.com/mobile/{0}/jquery.mobile-{0}.min.js", JQueryMobileVersion);
                    if (UseMetro)
                    {
                        jqueryScript = "";
                    }

                    //datebox js
                    //官方更新了导致按钮都出不来了
                    //var dateboxcoreScript = LocalScript ? subPath + "js/datebox/jqm-datebox.core.min.js" : "http://dev.jtsage.com/cdn/datebox/latest/jqm-datebox.core.min.js";
                    //var dateboxcalboxScript = LocalScript ? subPath + "js/datebox/jqm-datebox.mode.calbox.min.js" : "http://dev.jtsage.com/cdn/datebox/latest/jqm-datebox.mode.calbox.min.js";
                    //var dateboxflipboxScript = LocalScript ? subPath + "js/datebox/jqm-datebox.mode.flipbox.min.js" : "http://dev.jtsage.com/cdn/datebox/latest/jqm-datebox.mode.flipbox.min.js";
                    //var dateboxutf8Script = LocalScript ? subPath + "js/datebox/jquery.mobile.datebox.i18n.en_US.utf8.js" : "http://dev.jtsage.com/cdn/datebox/i18n/jquery.mobile.datebox.i18n.en_US.utf8.js";
                    var dateboxcoreScript = subPath + "js/datebox/jqm-datebox.core.min.js";
                    var dateboxcalboxScript = subPath + "js/datebox/jqm-datebox.mode.calbox.min.js";
                    var dateboxdateboxScript = subPath + "js/datebox/jqm-datebox.mode.datebox.min.js";
                    var dateboxflipboxScript = subPath + "js/datebox/jqm-datebox.mode.flipbox.min.js";
                    var dateboxutf8Script = subPath + "js/datebox/jquery.mobile.datebox.i18n.en_US.utf8.js";

                    //qrcode js 
                    if (UseQRCode)
                    {
                        var qrcodeScript = subPath + "jquery-qrcode-master/jquery.qrcode.min.js";
                        AddClientScripts(new string[] { qrcodeScript });
                    }
                    var clientScripts = new string[] {
                        jqueryScript,
                        subPath + "js/jquery.json.js",
                        jqueryMobileScript,
                        dateboxcoreScript,
                        dateboxcalboxScript,
                        dateboxdateboxScript,
                        dateboxflipboxScript,
                        dateboxutf8Script,
                        subPath + "js/jquery.infolight.mobile.js",
                        subPath + "js/jquery.infolight.mobile-wf.js",
                        subPath + "MobileMainFlowPage.js",
                        subPath + string.Format("MobileMainFlowPage-{0}.js", JQueryMobileVersion),
                        subPath + "js/plugins/jquery.swipeButton.js"
                    };
                    AddClientScripts(clientScripts);
                    if (UseMap == JQMapType.Google)
                    {
                        AddClientScript(string.Format("http://maps.google.com/maps/api/js?key={0}&signed_in=true&libraries=places&", Google_AK));
                    }
                    else if (UseMap == JQMapType.Baidu)
                    {
                        AddClientScript(string.Format("http://api.map.baidu.com/api?type=quick&ak={0}&v=2.0", BAIDU_AK));
                    }
                }
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!DesignMode)
            {
                var clientInfo = (EFClientTools.EFServerReference.ClientInfo)System.Web.HttpContext.Current.Session["ClientInfo"];
                if (clientInfo != null)
                {
                    var hiddenField = new HiddenField();
                    hiddenField.ID = "_DEVELOPERID";
                    hiddenField.Value = clientInfo.SDDeveloperID;
                    hiddenField.RenderControl(writer);

                    hiddenField = new HiddenField();
                    hiddenField.ID = "_PARAMETERS";

                    var parameters = new List<string>();

                    foreach (var key in QueryString.AllKeys)
                    {
                        parameters.Add(string.Format("{0}={1}", key, QueryString[key]));
                    }

                    hiddenField.Value = string.Join("&", parameters);
                    // hiddenField.Value = string.Join("&", QueryString.Select(c => string.Format("{0}={1}", c.Key, System.Web.HttpUtility.UrlEncode(c.Value))));
                    hiddenField.RenderControl(writer);
                }
                if (UseMap == JQMapType.Baidu)
                {
                    var hiddenField = new HiddenField();
                    hiddenField.ID = "_BAIDUAK";
                    hiddenField.Value = BAIDU_AK;
                    hiddenField.RenderControl(writer);
                }
            }
        }

        public void AddMeta(string name, string content)
        {
            var literal = new LiteralControl();
            literal.Text = string.Format("<meta name=\"{0}\" content=\"{1}\">", name, content);
            this.Page.Header.Controls.Add(literal);
        }

        public void AddClientScripts(string[] urls)
        {
            var literal = new LiteralControl();
            var scripts = new StringBuilder();
            foreach (var url in urls)
            {
                scripts.Append(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", url));
            }
            literal.Text = scripts.ToString();
            this.Page.Header.Controls.AddAt(0, literal);
        }

        public void AddClientScript(string url)
        {
            var literal = new LiteralControl();
            literal.Text = string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", url);
            this.Page.Header.Controls.Add(literal);
        }

        public bool IsClientScriptRegistered(string url)
        {
            return this.Page.Header.Controls.OfType<LiteralControl>().FirstOrDefault(c => c.Text.Contains(url)) != null;

        }

        public void AddCss(string url)
        {
            var link = new HtmlLink() { Href = url };
            link.Attributes["rel"] = "stylesheet";
            this.Page.Header.Controls.Add(link);
        }

        public bool IsCssRegistered(string url)
        {
            return this.Page.Header.Controls.OfType<HtmlLink>().FirstOrDefault(c => c.Href.Equals(url)) != null;
        }

        public static void RenderPopup(System.Web.UI.HtmlTextWriter writer, string popupID)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-content");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, popupID);
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Popup);
            writer.AddAttribute(JQProperty.DataTheme, JQDataTheme.D);
            writer.AddAttribute(JQProperty.DataOverlayTheme, JQDataTheme.A);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(JQProperty.DataRel, "back");
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.Button);
            writer.AddAttribute(JQProperty.DataIcon, JQDataIcon.Delete);

            writer.AddAttribute(JQProperty.DataIconPos, JQDataIconPos.NoText);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-btn-right");
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write("Close");
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
    }

}
