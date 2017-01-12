<%@ WebHandler Language="C#" Class="ExtDefault" %>

using System;
using System.Web;
using Srvtools;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Text;

public class ExtDefault : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context)
    {
        try
        {
            if (string.IsNullOrEmpty(CliUtils.fLoginUser))
            {
                throw new Exception("75FF57F7-7AC0-43c8-9454-C92B4A2723BB");
            }
            string methods = context.Request["methods"];
            if (!string.IsNullOrEmpty(methods))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("{defObj:{");
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> dicDefMethods = serializer.DeserializeObject(methods) as Dictionary<string, object>;
                foreach (KeyValuePair<string, object> methodPair in dicDefMethods)
                {
                    string defMethod = methodPair.Value.ToString();
                    if (defMethod.IndexOf('.') != -1)
                    {
                        string module = defMethod.Split('.')[0];
                        string method = defMethod.Split('.')[1];
                        object[] defResult = CliUtils.CallMethod(module, method, new object[] { });
                        if (defResult.Length == 2 && (int)defResult[0] == 0)
                        {
                            builder.AppendFormat("{0}:'{1}',", methodPair.Key, defResult[1]);
                        }
                    }
                    else if (isSysDefaultValue(defMethod.ToLower()))
                    {
                        object[] sysValue = CliUtils.GetValue(defMethod);
                        if (sysValue.Length > 1 && (int)sysValue[0] == 0)
                        {
                            builder.AppendFormat(string.Format("{0}:'{1}',", methodPair.Key, sysValue[1]));
                        }
                    }
                }
                if (builder.Length > 0)
                {
                    if (builder.ToString().EndsWith(","))
                    {
                        builder.Remove(builder.Length - 1, 1);
                    }
                }
                builder.Append("},success:true}");

                context.Response.Write(builder.ToString());
            }
        }
        catch (Exception exception)
        {
            string errorMsg = exception.Message;
            context.Response.Write(string.Format("{{success:false,message:\"{0}\",stack:\"{1}\"}}",
                errorMsg.Replace("\r", "\\r").Replace("\n", "\\n").Replace(@"\", @"\\"),
                exception.StackTrace.Replace("\r", "\\r").Replace("\n", "\\n").Replace(@"\", @"\\")));
        }
    }

    bool isSysDefaultValue(string method)
    {
        return (method == "_usercode" || method == "_username" || method == "_solution" || method == "_database" || method == "_sitecode" || method == "_ipaddress" || method == "_language" || method == "_today" || method == "_sysdate" || method == "_servertoday" || method == "_firstday" || method == "_lastday" || method == "_firstdaylm" || method == "_lastdaylm" || method == "_firstdayty" || method == "_lastdayty" || method == "_firstdayly" || method == "_lastdayly");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}