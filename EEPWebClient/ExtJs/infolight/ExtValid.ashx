<%@ WebHandler Language="C#" Class="ExtValid" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Srvtools;
using System.Collections;
using System.Text;

public class ExtValid : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            if (string.IsNullOrEmpty(CliUtils.fLoginUser))
            {
                throw new Exception("75FF57F7-7AC0-43c8-9454-C92B4A2723BB");
            }
            string oper = context.Request["oper"];
            StringBuilder builder = new StringBuilder();
            if (oper == "cell")
            {
                string method = context.Request["method"];
                if (!string.IsNullOrEmpty(method) && method.IndexOf('.') != -1)
                {
                    string module = method.Split('.')[0];
                    method = method.Split('.')[1];
                    string value = context.Request["value"];
                    string record = context.Request["record"];
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Dictionary<string, object> dickeyValues = serializer.DeserializeObject(record) as Dictionary<string, object>;
                    object[] validResult = CliUtils.CallMethod(module, method, new object[] { value, hashKeyValues(dickeyValues) });
                    if (validResult.Length > 1 && (int)validResult[0] == 0)
                    {
                        if (validResult[1] is Boolean)
                        {
                            builder.AppendFormat("{{success:true,validSuccess:{0}}}", validResult[1].ToString().ToLower());
                        }
                    }
                }
            }
            else
            {
                string svalidMethods = context.Request["validMethods"];
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> validMethods = serializer.DeserializeObject(svalidMethods) as Dictionary<string, object>;
                if (oper == "record")
                {
                    string svalidRecord = context.Request["validRecord"];
                    Dictionary<string, object> validRecord = serializer.DeserializeObject(svalidRecord) as Dictionary<string, object>;
                    foreach (KeyValuePair<string, object> validMethod in validMethods)
                    {
                        string method = validMethod.Value.ToString();
                        if (!string.IsNullOrEmpty(method) && method.IndexOf('.') != -1)
                        {
                            string module = method.Split('.')[0];
                            method = method.Split('.')[1];
                            object[] validResult = CliUtils.CallMethod(module, method, new object[] { validRecord[validMethod.Key], hashKeyValues(validRecord) });
                            if (validResult.Length > 1 && (int)validResult[0] == 0)
                            {
                                if (!(Boolean)validResult[1])
                                {
                                    builder.AppendFormat("{{success:true,validSuccess:false,field:'{0}'}}", validMethod.Key);
                                    goto Return;
                                }
                            }
                        }
                    }
                }
                else if (oper == "all")
                {
                    string srecords = context.Request["modiRecords"];
                    object[] records = serializer.DeserializeObject(srecords) as object[];
                    foreach (Dictionary<string, object> record in records)
                    {
                        foreach (KeyValuePair<string, object> validMethod in validMethods)
                        {
                            string method = validMethod.Value.ToString();
                            if (!string.IsNullOrEmpty(method) && method.IndexOf('.') != -1)
                            {
                                string module = method.Split('.')[0];
                                method = method.Split('.')[1];
                                object[] validResult = CliUtils.CallMethod(module, method, new object[] { record[validMethod.Key], hashKeyValues(record) });
                                if (validResult.Length > 1 && (int)validResult[0] == 0)
                                {
                                    if (!(Boolean)validResult[1])
                                    {
                                        string invalidRecord = serializer.Serialize(record);
                                        builder.AppendFormat("{{success:true,validSuccess:false,field:'{0}', record:{1}}}", validMethod.Key, invalidRecord);
                                        goto Return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        Return:
            if (builder.Length > 0)
            {
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

    Hashtable hashKeyValues(Dictionary<string, object> record)
    {
        Hashtable hashkeyValues = new Hashtable();
        foreach (KeyValuePair<string, object> pair in record)
        {
            hashkeyValues.Add(pair.Key, pair.Value);
        }
        return hashkeyValues;
    }
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}