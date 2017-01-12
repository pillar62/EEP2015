using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.IO;

namespace Infolight.EasilyReportTools.UI
{
    internal class AspNetScriptsProvider
    {
        public static string GetScripts()
        {
            string script = "";

            script = @"function CheckDecimal(el, ev) 
                        {
                            var event = ev || window.event;                        
                            var currentKey = event.charCode||event.keyCode;
                            
                            if (currentKey == 110 || currentKey == 190) {
                                if (el.value.indexOf('.')>=0) 
                                    ReturnFalse();

                            }else 
                                if (currentKey!=8 && currentKey !=9 && currentKey != 46 && (currentKey<37 || currentKey>40) && (currentKey<48 || currentKey>57) && (currentKey<96 || currentKey>105))
                                    ReturnFalse();

                        }";

            script += "function CheckNum(){var objRegex=/[\\d]/ig;if (String.fromCharCode(event.keyCode).match(objRegex) == null && event.keyCode!=9)ReturnFalse();}";

            script += @"function ReturnFalse()
                        {
                            if (window.event) 
                                        event.returnValue=false; 
                                    else                       
                                        event.preventDefault();
                        }";

            return script;
        }

        public static void RegisterStartupScript(Control control, string script)
        {
            Control panel = GetUpdatePanel(control);
            if (panel != null)
            {
                ScriptManager.RegisterStartupScript(panel as UpdatePanel, control.Page.GetType(), "RegisterScript", script, true);
            }
            else
            {
                control.Page.ClientScript.RegisterStartupScript(control.Page.GetType(), "RegisterScript", script, true);
            }
        }

        public static Control GetUpdatePanel(Control control)
        {
            Control panel = control.Parent;
            while (panel != null && panel.GetType() != typeof(UpdatePanel))
            {
                panel = panel.Parent;
            }
            return panel;
        }

        public static void ShowMessage(Control control, string message)
        {
            Control panel = GetUpdatePanel(control);
            if (panel != null)
            {
                ScriptManager.RegisterStartupScript(panel as UpdatePanel, control.Page.GetType(), "alertForm", "alert('" + message + "')", true);
            }
            else
            {
                control.Page.ClientScript.RegisterStartupScript(control.Page.GetType(), "alertForm", "<script>alert('" + message + "')</script>");
            }
        }

        public static void DownLoadFile(WebEasilyReport report)
        {
            FileInfo file = new FileInfo(report.FilePath);
            if (file.Exists)
            {
                System.Web.HttpResponse Response = report.Page.Response;
                Response.Clear();
                Response.Buffer = false;
                Response.ContentType = "application/x-msdownload";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.Filter.Close();
                Response.WriteFile(file.FullName);
                Response.End();
            }
        }

    }
}
