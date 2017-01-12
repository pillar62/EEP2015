using System;
using System.Data;
using System.Configuration;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text;
using System.Text;
using System.Data;
using System.Web.Script.Serialization;
using System.Collections;
//using System.Xml.Linq;

/// <summary>
/// Summary description for JsonHelper
/// </summary>
public static class JsonHelper
{
    public static string ToJSON(object obj)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(obj);
    }

    public static string ToJSON(object obj, int recursionDepth)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        serializer.RecursionLimit = recursionDepth;
        return serializer.Serialize(obj);
    }

    public static string dataTabletoJson(DataTable dt)
    {
        return dataRowtoJson(dt.Columns, dt.Select(), "", "", "");
    }

    public static string dataTabletoJson(DataTable dt, string fields, string specialfields)
    {
        return dataRowtoJson(dt.Columns, dt.Select(), fields, specialfields, "");
    }

    public static string dataTabletoJson(DataTable dt, string fields, string specialfields, string checkfields)
    {
        return dataRowtoJson(dt.Columns, dt.Select(), fields, specialfields, checkfields);
    }

    public static string dataTabletoJson(DataTable dt, string fields, string specialfields, string checkfields,string setsort)
    {
        //$edit100407 by navy ,排序問題
        //string sortStr = "";
        //setsort = setsort.Trim().ToUpper();
        //if (setsort.IndexOf("ORDER BY") > -1)
        //    sortStr = setsort.Substring(setsort.IndexOf("ORDER BY") + 9);

        //DataView dv = dt.DefaultView;
        //dv.Sort = sortStr;
        //DataTable dt2 = dv.ToTable();

        return dataRowtoJson(dt.Columns, dt.Select("", setsort), fields, specialfields, checkfields);
    }

    public static string dataRowtoJson(DataColumnCollection dataColumns, DataRow[] rows, string fields, string specialfields, string checkfields)
    {
        string reJson = "";
        List<Hashtable> hashList = new List<Hashtable>();

        for (int k = 0; k < rows.Length; k++)
        {
            DataRow row = rows[k];
            Hashtable ht = new Hashtable();
            foreach (DataColumn col in dataColumns)
            {
                if (fields == null || fields.Trim() == "" || checkArray(fields.Split(','), col.ColumnName))
                {
                    //這里還要再修改一下的，前面傳一個參數過來，這里處理特殊的欄位
                    //原来是按照栏位类型来处理的，譬如DataTime,但是现在需求看来，可能一些原本String的栏位来存放DateTime的数据，所以特殊栏位要特殊处理一下，这里对应到 Grid产生的代码页要处理
                    if (specialfields != null && specialfields.ToString() != "" && checkArray(specialfields.Split(','), col.ColumnName))
                    {
                        try
                        {
                            //$edit20100524 by navy For这里原来是处理日期类型的栏位，在DB中是设定的varchar的类型，但是现在新增一种情况，可能是设定varchar(8)的情况，
                            if (row[col.ColumnName].ToString().Trim().Length == 8)
                            {
                                string getdatastring = row[col.ColumnName].ToString().Trim();
                                string setdatastring = getdatastring.Substring(0, 4) + "/" + getdatastring.Substring(4, 2) + "/" + getdatastring.Substring(6, 2);
                                ht.Add(col.ColumnName, setdatastring);
                            }
                            else if (row[col.ColumnName].ToString().Trim().Length == 10)
                                ht.Add(col.ColumnName, Convert.ToDateTime(row[col.ColumnName]).ToString("yyyy/MM/dd", System.Globalization.CultureInfo.CreateSpecificCulture("en-us")));
                        }
                        catch
                        {
                            ht.Add(col.ColumnName, row[col.ColumnName]);
                        }
                    }
                    else
                    {
                        //$edit090813,增加特殊bool類型
                        if (checkArray(checkfields.Split(','), col.ColumnName))
                        {
                            if (row[col.ColumnName].ToString() == "Y")
                                ht.Add(col.ColumnName, true);
                            else
                                ht.Add(col.ColumnName, false);
                        }
                        else
                        {
                            string coltype = col.DataType.FullName.ToLower();
                            if (coltype == "system.datetime" && row[col.ColumnName] != DBNull.Value)
                                ht.Add(col.ColumnName, Convert.ToDateTime(row[col.ColumnName]).ToString("yyyy/MM/dd", System.Globalization.CultureInfo.CreateSpecificCulture("en-us")));
                            //$edit090813,增加bool類型
                            else if (genieJsonSerializer.IsNumeric(coltype) || coltype == "system.boolean")
                                ht.Add(col.ColumnName, row[col.ColumnName]);
                            else
                                ht.Add(col.ColumnName, row[col.ColumnName].ToString().Replace("\n","<br>"));
                        }
                    }
                }
            }
            hashList.Add(ht);
        }
        reJson = JsonHelper.ToJSON(hashList);

        return reJson;
    }

    public static bool checkArray(string[] ary, string str)
    {
        bool flag = false;

        for (int i = 0; i < ary.Length; i++)
        {
            string aryValue = "";
            if (ary[i].IndexOf('.') > -1)
                aryValue = ary[i].Substring(ary[i].IndexOf('.') + 1);
            else
                aryValue = ary[i];

            if (str.Trim().ToLower() == aryValue.Trim().ToLower())
            {
                flag = true;
                break;
            }
        }

        return flag;
    }
}

public static class genieJsonSerializer
{
    public static bool IsNumeric(Type dataType)
    {
        return IsNumeric(dataType.ToString());
    }

    public static bool IsNumeric(string dataType)
    {
        string type = dataType.ToLower();
        if (type == "system.uint" || type == "system.uint16" || type == "system.uint32" || type == "system.uint64"
          || type == "system.int" || type == "system.int16" || type == "system.int32" || type == "system.int64"
          || type == "system.single" || type == "system.double" || type == "system.decimal")
        {
            return true;
        }
        return false;
    }

    public static string FormatESC(string value)
    {
        return value.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "\\r").Replace("\n", "\\n");
    }

    public static string TableToJavascriptArray(DataTable dt, string[] fields)
    {
        StringBuilder JsonString = new StringBuilder();
        if (dt != null && dt.Rows.Count > 0)
        {
            JsonString.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                JsonString.Append("[");
                for (int j = 0; j < fields.Length; j++)
                {
                    object fieldValue = dt.Rows[i][fields[j]];
                    if (IsNumeric(fieldValue.GetType()))
                    {
                        JsonString.Append(fieldValue.ToString() + ((j < fields.Length - 1) ? "," : ""));
                    }
                    else
                    {
                        JsonString.Append("'" + FormatESC(fieldValue.ToString()) + ((j < fields.Length - 1) ? "'," : "'"));
                    }
                }
                if (i < dt.Rows.Count - 1)
                {
                    JsonString.Append("],");
                }
                else
                {
                    JsonString.Append("]");
                }
            }
            JsonString.Append("]");
            return JsonString.ToString();
        }
        return "";
    }

    public static string TableToJsonArray(DataTable dt, string[] fields)
    {
        //StringBuilder JsonString = new StringBuilder();
        //if (dt != null && dt.Rows.Count > 0)
        //{
        //    JsonString.Append("[");
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        JsonString.Append("{");
        //        for (int j = 0; j < fields.Length; j++)
        //        {
        //            Type fieldDataType = dt.Columns[fields[j]].DataType;
        //            if (GloFix.IsNumeric(fieldDataType) || fieldDataType == typeof(bool))
        //            {
        //                JsonString.Append(string.Format("{0}:{1}", fields[j], dt.Rows[i][fields[j]].ToString()) + ((j < fields.Length - 1) ? "," : ""));
        //            }
        //            else
        //            {
        //                JsonString.Append(string.Format("{0}:'{1}'", fields[j], GloFix.FormatESC(dt.Rows[i][fields[j]].ToString())) + ((j < fields.Length - 1) ? "," : ""));
        //            }
        //        }
        //        if (i < dt.Rows.Count - 1)
        //        {
        //            JsonString.Append("},");
        //        }
        //        else
        //        {
        //            JsonString.Append("}");
        //        }
        //    }
        //    JsonString.Append("]");
        //}
        //return JsonString.ToString();
        return RowsToJsonArray(dt.Select(), fields);
    }

    public static string RowsToJsonArray(DataRow[] rows, string[] fields)
    {
        StringBuilder JsonString = new StringBuilder();
        if (rows != null && rows.Length > 0)
        {
            JsonString.Append("[");
            for (int i = 0; i < rows.Length; i++)
            {
                JsonString.Append("{");
                for (int j = 0; j < fields.Length; j++)
                {
                    try
                    {
                        Type fieldDataType = rows[i][fields[j]].GetType();
                        if (IsNumeric(fieldDataType) || fieldDataType == typeof(bool))
                        {
                            JsonString.Append(string.Format("{0}:{1}", fields[j], rows[i][fields[j]].ToString()) + ((j < fields.Length - 1) ? "," : ""));
                        }
                        else
                        {
                            JsonString.Append(string.Format("{0}:'{1}'", fields[j], FormatESC(rows[i][fields[j]].ToString())) + ((j < fields.Length - 1) ? "," : ""));
                        }
                    }
                    catch {
                        JsonString.Append(string.Format("{0}:'{1}'", fields[j], " ") + ((j < fields.Length - 1) ? "," : ""));
                    }
                }
                if (i < rows.Length - 1)
                {
                    JsonString.Append("},");
                }
                else
                {
                    JsonString.Append("}");
                }
            }
            JsonString.Append("]");
        }
        else
            JsonString.Append("[]");

        return JsonString.ToString();
    }
}

public class GeneralSearchResult
{
    //=============================C#解析Json============

    private DataTable fieldDefine = new DataTable();
    /// <summary>
    /// 返回的数据结构定义，无数据
    /// </summary>
    public DataTable FieldDefine
    {
        get { return fieldDefine; }
        set { fieldDefine = value; }
    }

    private DataTable retrunData = new DataTable();
    /// <summary>
    /// 返回的数据，格式为DataTable，结构和FieldDefine中的结构一样
    /// </summary>
    public DataTable RetrunData
    {
        get { return retrunData; }
        set { retrunData = value; }
    }

    /// <summary>
    /// 将json数据转换为定义好的对象，数据转换为DataTable
    /// </summary>
    /// <param name="jsonText"></param>
    /// <returns></returns>
    public static GeneralSearchResult GetTransformData(string jsonText)
    {
        GeneralSearchResult gsr = new GeneralSearchResult();
        JavaScriptSerializer s = new JavaScriptSerializer();

        object[] rows = (object[])s.DeserializeObject(jsonText);
        Dictionary<string, object> dicFieldDefine = (Dictionary<string, object>)rows[0];
        foreach (KeyValuePair<string, object> ss in dicFieldDefine)
        {
            gsr.FieldDefine.Columns.Add(ss.Key, typeof(string));
        }
        gsr.RetrunData = gsr.FieldDefine.Clone();
        foreach (object ob in rows)
        {
            Dictionary<string, object> val = (Dictionary<string, object>)ob;
            DataRow dr = gsr.RetrunData.NewRow();
            foreach (KeyValuePair<string, object> sss in val)
            {
                dr[sss.Key] = sss.Value;
            }
            gsr.RetrunData.Rows.Add(dr);
        }
        return gsr;
    }
}

