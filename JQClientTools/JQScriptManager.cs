using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.ComponentModel;
using System.Security.Cryptography;
using EFClientTools.EFServerReference;
using System.Collections.Specialized;
using System.Web;

namespace JQClientTools
{
    public class JQScriptManager : WebControl
    {
        public JQScriptManager()
        {
            Locale = JQLocale.Chinese_Taiwan;
            JQueryVersion = "1.8.0";
            UseChartJS = false;
            UseQRCode = true;
            QueryString = new NameValueCollection();
            UseFlow = true;
            UseMetro = false;
        }

        public bool LocaleAuto { get; set; }

        [Category("Infolight")]
        public string Locale { get; set; }
        [Category("Infolight")]
        public string JQueryVersion { get; set; }
        [Category("Infolight")]
        public bool LocalScript { get; set; }
        [Category("Infolight")]
        public bool UseChartJS { get; set; }
        [Category("Infolight")]
        public bool UseQRCode { get; set; }
        [Category("Infolight")]
        public bool UseFlow { get; set; }
        [Category("Infolight")]
        public bool UseMetro { get; set; }
        [Category("Infolight")]
        public bool UsePivottable { get; set; }
        [Category("Infolight")]
        public string AgentUser { get; set; }
        [Category("Infolight")]
        public string AgentDeveloper { get; set; }
        [Category("Infolight")]
        public string AgentDatabase { get; set; }
        [Category("Infolight")]
        public string AgentSolution { get; set; }
        [Category("Infolight")]
        public string ParentFolder { get; set; }

        private static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
            // No settings need modifying here      
            using (System.IO.StringReader textReader = new System.IO.StringReader(xml))
            {
                using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
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
            }

            if (LocaleAuto)
            {
                switch (EFClientTools.ClientUtility.ClientInfo.Locale.ToLower())
                {
                    case "zh-hans-cn":
                    case "zh-cn": this.Locale = "zh_CN"; break;
                    case "zh-hk":
                    case "zh-hant-tw":
                    case "zh-tw": this.Locale = "zh_TW"; break;
                    default: this.Locale = "en"; break;
                }
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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.DesignMode)
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
                    "../js/jquery.infolight.chart.js"
                    };
                    AddClientScripts(chartScripts);
                    AddCss("../jqplot/jquery.jqplot.min.css");

                }
                if (UseQRCode)
                {
                    var qrcodeScripts = new string[] {
                    //qrcode
                    "../jquery-qrcode-master/jquery.qrcode.min.js",
                    "../js/schedule/jquery.schedule.js"
                    };
                    AddClientScripts(qrcodeScripts);
                }

                if (UsePivottable)
                {
                    var PivottableScripts = new string[] {
                    
                    "https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js",
                    "https://cdnjs.cloudflare.com/ajax/libs/d3/3.5.5/d3.min.js",
                    "https://cdnjs.cloudflare.com/ajax/libs/c3/0.4.10/c3.min.js",
                    "../js/pivottable-master/dist/pivot.js"
                    };
                    AddClientScripts(PivottableScripts);
                    AddCss("https://cdnjs.cloudflare.com/ajax/libs/c3/0.4.10/c3.min.css");
                    AddCss("../js/pivottable-master/dist/pivot.css");

                }
                var isFlow = System.Configuration.ConfigurationManager.AppSettings["IsFlow"];
                if (UseFlow && !string.IsNullOrEmpty(isFlow) && string.Compare(isFlow, bool.TrueString, true) == 0)
                {
                    var flowScripts = new string[] {
                        "../js/jquery.infolight-wf.js",
                        "../WorkflowBoot.js",
                    };
                    AddClientScripts(flowScripts);
                }

                //var RunTimeWebClient = System.Configuration.ConfigurationManager.AppSettings["RunTimeWebClient"];
                if (UseFlow && !String.IsNullOrEmpty(EFClientTools.ClientUtility.ClientInfo.SDDeveloperID))
                {
                    var xml = (string)EFClientTools.ClientUtility.Client.SDGetSolutions(EFClientTools.ClientUtility.ClientInfo);
                    if (xml.Contains("UseWorkflow=1"))
                    {
                        var flowScripts = new string[] {
                        "../js/jquery.infolight-wf.js",
                        "../WorkflowBoot.js",
                        "../js/jquery.infolight-wf.js",
                        "../InnerPages/FormApprove.js",
                        "../InnerPages/FormApproveAll.js" ,
                        "../InnerPages/FormNotify.js",
                        "../InnerPages/FormPlusApprove.js" ,
                        "../InnerPages/FormHasten.js" ,
                        "../InnerPages/FormReturn.js" ,
                        "../InnerPages/FormReturnAll.js" ,
                        "../InnerPages/FormSubmit.js" ,
                        "../InnerPages/FormComment.js" ,
                        "../InnerPages/FormFileUpload.js" ,
                        "../InnerPages/FormFlowQuery.js" 

                        };
                        AddClientScripts(flowScripts);
                    }
                }
                var jqueryScript = LocalScript ? string.Format("../js/jquery-{0}.min.js", JQueryVersion)
                    : string.Format("http://code.jquery.com/jquery-{0}.min.js", JQueryVersion);
                var easyuiScript = "../js/jquery.easyui.min.js";
                if (UseMetro)
                {
                    //jqueryScript = "../js/jquery-1.9.0.min.js";
                    jqueryScript = "";
                    easyuiScript = "../InnerPages/jquery.easyui.min.1.3.5.js";
                }
          

                var clientScripts = new string[] {
                    jqueryScript,
                    easyuiScript,
                    "../js/jquery.json.js",
                    "../js/datagrid-detailview.js",
                    "../js/datagrid-bufferview.js",
                    "../js/jquery.infolight.js",
                    "../js/jquery.userdefine.js",
                    
                    string.Format("../js/locale/easyui-lang-{0}.js", Locale)
                };
                AddClientScripts(clientScripts);
                //foreach (var script in clientScripts)
                //{
                //    //if (!IsClientScriptRegistered(script))
                //    //{
                //    AddClientScript(script);
                //    //}
                //}
                if (!String.IsNullOrEmpty(EFClientTools.ClientUtility.ClientInfo.SDDeveloperID))
                {
                    AddClientScript("jquery.userdefine.js");


                    var xml = (string)EFClientTools.ClientUtility.Client.SDGetSolutions(EFClientTools.ClientUtility.ClientInfo);
                    if (xml.Contains("Theme=default"))
                        AddCss("easyuiTheme", "../js/themes/default/easyui.css");
                    else if (xml.Contains("Theme=black"))
                        AddCss("easyuiTheme", "../js/themes/black/easyui.css");
                    else if (xml.Contains("Theme=bootstrap"))
                        AddCss("easyuiTheme", "../js/themes/bootstrap/easyui.css");
                    else if (xml.Contains("Theme=gray"))
                        AddCss("easyuiTheme", "../js/themes/gray/easyui.css");
                    else if (xml.Contains("Theme=metro"))
                        AddCss("easyuiTheme", "../js/themes/metro/easyui.css");
                    else
                        AddCss("easyuiTheme", "../js/themes/default/easyui.css");
                }
                else
                {
                    AddClientScript("../js/jquery.userdefine.js");

                    var theme = System.Configuration.ConfigurationManager.AppSettings["Theme"];
                    if (String.IsNullOrEmpty(theme))
                        AddCss("easyuiTheme", "../js/themes/default/easyui.css");
                    else
                        AddCss("easyuiTheme", "../js/themes/" + theme + "/easyui.css");
                }
                var styleSheets = new string[] {
                    "../js/themes/icon.css",
                    "../js/themes/infolight.css",
                    "../js/schedule/jquery.schedule.css"
                };
                foreach (var sheet in styleSheets)
                {
                    if (!IsCssRegistered(sheet))
                    {
                        AddCss(sheet);
                    }
                }

               


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

                    hiddenField = new HiddenField();
                    hiddenField.ID = "_APPLICATIONPATH";
                    hiddenField.Value = this.Page.Request.ApplicationPath;
                    hiddenField.RenderControl(writer);


                    hiddenField = new HiddenField();
                    hiddenField.ID = "_PARENTFOLDER";
                    hiddenField.Value = this.ParentFolder;
                    hiddenField.RenderControl(writer);
                }
            }
        }

        public static string EncryptParameters(string param, string key)
        {
            var info = Encoding.UTF8.GetBytes(param);
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(info);
            var paramters = hash.Concat(info).ToArray();

            var encryptCodes = Convert.FromBase64String(key);
            for (int i = 0; i < paramters.Length; i++)
            {
                var encryptCode = encryptCodes[i % encryptCodes.Length];
                paramters[i] += encryptCode;
            }
            return Convert.ToBase64String(paramters);
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

        public void AddClientScripts(string[] urls)
        {
            var literal = new LiteralControl();
            var scripts = new StringBuilder();
           
            foreach (var url in urls)
            {
                var u = url;
                if (!string.IsNullOrEmpty(ParentFolder) && u.IndexOf("http") != 0)
                {
                    u = ParentFolder + url;
                }
                scripts.Append(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", u));
            }
            literal.Text = scripts.ToString();
            this.Page.Header.Controls.AddAt(0, literal);
        }

        public void AddClientScript(string url)
        {

            var literal = new LiteralControl();
            var u = url;
            if (!string.IsNullOrEmpty(ParentFolder) && u.IndexOf("http") != 0)
            {
                u = ParentFolder + url;
            }
            literal.Text = string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", u);
            this.Page.Header.Controls.Add(literal);
        }

        public bool IsClientScriptRegistered(string url)
        {
            return this.Page.Header.Controls.OfType<LiteralControl>().FirstOrDefault(c => c.Text.Contains(url)) != null;

        }

        public void AddCss(string url)
        {
            var u = url;
            if (!string.IsNullOrEmpty(ParentFolder) && u.IndexOf("http") != 0)
            {
                u = ParentFolder + url;
            }
            var link = new HtmlLink() { Href = u };
            link.Attributes["rel"] = "stylesheet";
            this.Page.Header.Controls.Add(link);
        }

        public void AddCss(string id, string url)
        {
            var u = url;
            if (!string.IsNullOrEmpty(ParentFolder) && u.IndexOf("http") != 0)
            {
                u = ParentFolder + url;
            }
            var link = new HtmlLink() { ID = id, Href = u };
            link.Attributes["rel"] = "stylesheet";
            this.Page.Header.Controls.Add(link);
        }

        public bool IsCssRegistered(string url)
        {
            return this.Page.Header.Controls.OfType<HtmlLink>().FirstOrDefault(c => c.Href.Equals(url)) != null;
        }
    }
}
