<%@ WebHandler Language="C#" Class="T1" %>
 
 using System;
 using System.Web;
using System.Web.Script.Serialization;

public class T1 : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string a = string.Empty;
        string json = string.Empty;
        string oper = context.Request["oper"];
        string tablename = context.Request["tableName"];
        string keys = context.Request["keys"];
        if (oper == "select")
        {
            int pagesize = int.Parse(context.Request["limit"]);
            int start = int.Parse(context.Request["start"]);

            System.Data.DataSet ds = new System.Data.DataSet("AccountList");
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True");
            try
            {
                connection.Open();
                string sqlcommand = string.Format("select count(*) from {0}", tablename);
                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(sqlcommand, connection);
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(command);
                da.Fill(ds);
                da.Dispose();
                string count = ds.Tables[0].Rows[0][0].ToString();
                sqlcommand = string.Format("select top {0} * from {1}", pagesize.ToString(), tablename);
                System.Data.SqlClient.SqlCommand command2 = new System.Data.SqlClient.SqlCommand(sqlcommand, connection);
                System.Data.SqlClient.SqlDataAdapter da2 = new System.Data.SqlClient.SqlDataAdapter(command2);
                ds.Tables.Clear();
                da2.Fill(ds);
                da2.Dispose();

                //json = string.Format("{{total:{0},data:{1}}}",
                //               ds.Tables[0].Rows.Count,
                //               JsonSerializer.TableToJsonArray(ds.Tables[0]));
                json = string.Format("{{total:{0},data:{1}}}",
                                   count,
                                   AjaxTools.JsonSerializer.TableToJsonArray(ds.Tables[0]));
                a = ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            //context.Response.ContentType = "text/plain";
            string callbackFunName = context.Request["callbackparam"];
            //context.Response.Write(callbackFunName + "([ { name:\"" + a + "\"},{name:\"J2\"}] )");
            //context.Response.Write("({ \"Customers\": " + json + "})");
            context.Response.Write(json);
        }
        else if (oper == "save")
        {
            string editTypes = context.Request["editTypes"];
            string changes = context.Request["changes"];
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            object[] editTypesobj = serializer.DeserializeObject(editTypes) as object[];
            object[] records = serializer.DeserializeObject(changes) as object[];

            if (editTypesobj.Length > 0)
            {
                System.Data.DataSet ds = new System.Data.DataSet("AccountList");
                System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True");
                string sqlcommand = string.Format("select * from {0}", tablename);
                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(sqlcommand, connection);
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(command);
                System.Data.SqlClient.SqlCommandBuilder builder = new System.Data.SqlClient.SqlCommandBuilder(da);
                if (ds.Tables.Count > 0) ds.Tables.Clear();
                da.Fill(ds);
                System.Data.DataTable tab = ds.Tables[0];
                try
                {
                    connection.Open();

                    int j = 0;
                    for (int i = 0; i < editTypesobj.Length; i++)
                    {
                        System.Collections.Generic.Dictionary<string, object> dicKey = editTypesobj[i] as System.Collections.Generic.Dictionary<string, object>;
                        if (dicKey["editType"].ToString() == "delete")
                        {
                            string single = this.GenWhereString(tab, dicKey);
                            System.Data.DataRow[] rows = tab.Select(single);
                            if (rows.Length == 1)
                            {
                                rows[0].Delete();
                            }
                            j++;
                        }
                        else
                        {
                            System.Collections.Generic.Dictionary<string, object> dicRecord = records[i - j] as System.Collections.Generic.Dictionary<string, object>;
                            if (dicKey["editType"].ToString() == "insert")
                            {
                                System.Data.DataRow row = tab.NewRow();
                                foreach (System.Collections.Generic.KeyValuePair<string, object> record in dicRecord)
                                {
                                    row[record.Key] = record.Value;
                                }
                                tab.Rows.Add(row);
                            }
                            else if (dicKey["editType"].ToString() == "edit")
                            {
                                string single = this.GenWhereString(tab, dicKey);
                                System.Data.DataRow[] rows = tab.Select(single);
                                if (rows.Length == 1)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, object> record in dicRecord)
                                    {
                                        rows[0][record.Key] = record.Value;
                                    }
                                }
                            }
                        }
                        
                    }
                    if (ds.HasChanges())
                    {
                        da.Update(ds);
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    connection.Close();
                    da.Dispose();
                }
            }
        }
    }
    string GenWhereString(System.Data.DataTable table, System.Collections.Generic.Dictionary<string, object> dicKey)
    {
        System.Text.StringBuilder whereBuilder = new System.Text.StringBuilder();
        foreach (System.Collections.Generic.KeyValuePair<string, object> wherePair in dicKey)
        {
            if (wherePair.Key != "editType")
            {
                Type colType = table.Columns[wherePair.Key].DataType;
                if (AjaxTools.GloFix.IsNumeric(colType))
                {
                    whereBuilder.AppendFormat("{0}={1} AND ", wherePair.Key, wherePair.Value);
                }
                else if (colType == typeof(Boolean))
                {
                    whereBuilder.AppendFormat("{0}={1} AND ", wherePair.Key, wherePair.Value.ToString().ToLower());
                }
                else
                {
                    whereBuilder.AppendFormat("{0}='{1}' AND ", wherePair.Key, wherePair.Value);
                }
            }
        }
        if (whereBuilder.ToString().EndsWith(" AND "))
        {
            whereBuilder.Remove(whereBuilder.Length - 5, 5);
        }
        return whereBuilder.ToString();
    }
  
     public bool IsReusable {
         get {
             return false;
         }
     }
 
 }