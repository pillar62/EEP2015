using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Data;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;

namespace OfficeTools
{
    /// <summary>
    /// The class to convert tags to values
    /// </summary>
    public class Automation
    {
        /// <summary>
        /// The enum of type of tag
        /// </summary>
        public enum TagType
        {
            Constant,
            DataSource,
            Function,
            UnKnown
        }

        /// <summary>
        /// Run Automation
        /// </summary>
        /// <param name="op">The officeplate whose tags defination is used</param>
        /// <param name="sTag">The whole string of Tag in Excel</param>
        /// <returns>The value of Result</returns>
        public static object[] Run(IOfficePlate op, string sTag)
        {
            object[] objaTag = AnalyzeTag(op, sTag);
            return ProcessTag((TagType)objaTag[0], (object[])objaTag[1], op);
        }

        /// <summary>
        /// Analyze the type of tag and split type of tag and parameters of tag
        /// </summary>
        /// <param name="op">the officeplate whose tags defination is used</param>
        /// <param name="sTag">the whole string of Tag in Excel</param>
        /// <returns>object consists of type of tag and parameters of tag</returns>
        public static object[] AnalyzeTag(IOfficePlate op, string sTag) 
        {
            int indexleftb = sTag.IndexOf('(');
            int indexrightb = sTag.IndexOf(')');
            if (indexleftb == -1 && indexrightb == -1)
            { 
                return new object[] { TagType.Constant, new object[] { sTag.Trim() } };
            }
            else if (indexleftb > 0 && indexrightb > indexleftb)
            {
                string tagname = sTag.Substring(0, indexleftb).Trim();
                for(int i =0; i < op.DataSource.Count; i ++)
                {
                    if((op.DataSource[i] as DataSourceItem).Caption == tagname)
                    {
                        if (indexrightb > indexleftb + 1)
                        {
                            string tagparam = sTag.Substring(indexleftb + 1, indexrightb - indexleftb - 1);
                            return new object[] { TagType.DataSource, new object[] { i, tagparam.Trim() } };
                        }
                        else
                        {
                            return new object[] { TagType.UnKnown, new object[] { "Recognized as datasource tag, but no params" } };
                        }
                    }
                }
                string tagpar = string.Empty;
                if (indexrightb > indexleftb + 1)
                {
                    tagpar = sTag.Substring(indexleftb + 1, indexrightb - indexleftb - 1);
                }
                return new object[] { TagType.Function, new object[] { tagname, tagpar } };
            }
            else
            {
                return new object[] { TagType.UnKnown, new object[] { "Tag Format can't be recognized" } };
            } 
        }

        /// <summary>
        /// Process all tag
        /// </summary>
        /// <param name="tagtp">The type of tag</param>
        /// <param name="Param">The param of tag</param>
        /// <param name="op">The officeplate whose tags defination is used</param>
        /// <returns>Object consist of plateexception and value</returns>
        public static object[] ProcessTag(TagType tagtp, object[] Param, IOfficePlate op)
        {
            switch (tagtp)
            {
                case TagType.Constant:
                    {
                        string param = Param[0].ToString();
                        return ProcessConstantTag(param,op);

                    }
                case TagType.DataSource:
                    {
                        int index = (int)Param[0];
                        string param = Param[1].ToString();
                        return ProcessDataSourceTag(index, param, op);
                    }
                case TagType.Function:
                    {
                        string name = Param[0].ToString();
                        string param = Param[1].ToString();
                        return ProcessFunctionTag(name, param, op);
                    }
                default:
                    {
                        string message = Param[0].ToString();
                        return new object[] { PlateException.InvalidTag, message };
                    }
            }
        }
       
        /// <summary>
        /// Process constant tag
        /// </summary>
        /// <param name="param">The name of constant tag</param>
        /// <param name="op">The officeplate whose tags defination is used</param>
        /// <returns>Object consist of plateexception and value</returns>
        public static object[] ProcessConstantTag(string param, IOfficePlate op)
        {
            string format = string.Empty;
            string expression = GetTagDefineExpression(param, op, ref format);
            if (expression == null)
            {
                return new object[] { PlateException.TagDefineNotFound, string.Format("Can not find Constant Tag:'{0}' in TagDefination", param) };
            }
            else
            {
                return Expression.GetExpressionValue(expression, null,op, format);            
            }
        }

        /// <summary>
        /// Process dataSource tag
        /// </summary>
        /// <param name="indexds">The index of datasource in offeceplate's datasource defination</param>
        /// <param name="param">The parameters of datasource tag</param>
        /// <param name="op">The officeplate whose tags defination is used</param>
        /// <returns>Object consist of plateexception and value</returns>
        public static object[] ProcessDataSourceTag(int indexds, string param, IOfficePlate op)
        {
            string[] strparam = param.Split(',');
            if (strparam.Length != 2)
            {
                return new object[] { PlateException.InvalidParameter, string.Format("Parameters of DataSource:'{0}' is invalid", ((DataSourceItem)op.DataSource[indexds]).Caption) };
            }
            //int rowindex = -1;
            DataView view = (DataView)((DataSourceItem)op.DataSource[indexds]).GetTable()[((DataSourceItem)op.DataSource[indexds]).DataMember];
            DataRow row = null;
            if(string.Compare(strparam[0].Trim(), "c",true) == 0)
            {
                if (((DataSourceItem)op.DataSource[indexds]).DataSource is Srvtools.InfoBindingSource)
                {
                    if (((((DataSourceItem)op.DataSource[indexds]).DataSource as Srvtools.InfoBindingSource).Current as DataRowView != null))
                    {
                        row = ((((DataSourceItem)op.DataSource[indexds]).DataSource as Srvtools.InfoBindingSource).Current as DataRowView).Row;
                    }
                    else
                    {
                        return new object[] { PlateException.None, string.Empty};
                    }
                }
                else if (((DataSourceItem)op.DataSource[indexds]).DataSource is Srvtools.WebDataSource)
                {
                    row = (((DataSourceItem)op.DataSource[indexds]).DataSource as Srvtools.WebDataSource).CurrentRow;
                    if (row == null)
                    {
                        return new object[] { PlateException.None, string.Empty };
                    }
                }
                else
                {
                    return new object[]{PlateException.InvalidParameter
                    ,string.Format("The first parameter of DataSource:'{0}' is invalid,'{1}' can not used in a non_bindingsource datasource",((DataSourceItem)op.DataSource[indexds]).Caption,strparam[0])};
                }
            }
            else
            {
                try
                {
                    int rowindex = Convert.ToInt32(strparam[0]);
                    if (rowindex < 0 || rowindex > view.Count - 1)
                    {
                        return new object[]{PlateException.InvalidParameter
                        ,string.Format("The first parameter of DataSource:'{0}' is invalid, row {1} does not exist",((DataSourceItem)op.DataSource[indexds]).Caption, rowindex.ToString())};
                    }
                    else
                    {
                        row = view[rowindex].Row;
                    }
                }
                catch
                {
                    return new object[]{PlateException.InvalidParameter
                    ,string.Format("The first parameter of DataSource:'{0}' is invalid,'{1}' can not be recognized as a number",((DataSourceItem)op.DataSource[indexds]).Caption,strparam[0])};
                }
            }
      
            string column = strparam[1].Trim();
            if(view.Table.Columns.Contains(column))
            {
                int indeximagecolumn = ((DataSourceItem)op.DataSource[indexds]).ImageColumns.IndexOf(column);
                if (indeximagecolumn == -1)
                {
                    return new object[] { PlateException.None, row[column] };
                }
                else
                {
                    if (view.Table.Columns[column].DataType == typeof(byte[]))
                    {
                        if (row[column] != DBNull.Value)
                        {
                            MemoryStream ms = new MemoryStream((byte[])row[column]);
                            try
                            {
                                Bitmap pic = new Bitmap(ms);
                                if (string.Compare(strparam[0].Trim(), "c", true) == 0)
                                {
                                    pic.Tag = false;
                                }
                                else
                                {
                                    pic.Tag = true;
                                }
                                return new object[] { PlateException.None, pic };
                            }
                            catch
                            {
                                return new object[] { PlateException.None, string.Empty };
                            }
                        }
                        else
                        {
                            return new object[] { PlateException.None, string.Empty };
                        }
                    }
                    else
                    { 
                        string path = (((DataSourceItem)op.DataSource[indexds]).ImageColumns[indeximagecolumn] as DataSourceImageColumnItem).DefaultPath;
                        if (path.Length > 0)
                        {
                            path += "\\";
                        }
                        string filename = row[column].ToString();
                        if (filename.Length > 0)
                        {
                            byte[] objrtn = null;
                            if (op is OfficePlate)
                            {
                                object[] objcall = Srvtools.CliUtils.CallMethod("GLModule", "DownLoadFile", new object[] { path + filename });
                                if (objcall != null && objcall.Length == 3)
                                {
                                    objrtn = ((byte[])objcall[2]);
                                }
                            }
                            else
                            {
                                string mappath = (op as WebOfficePlate).Page.Server.MapPath(path + filename);
                                if (System.IO.File.Exists(mappath))
                                {
                                    objrtn = System.IO.File.ReadAllBytes(path + filename);
                                }
                            }
                            if (objrtn != null)
                            {
                                MemoryStream ms = new MemoryStream(objrtn);
                                Bitmap pic = new Bitmap(ms);
                                if (string.Compare(strparam[0].Trim(), "c", true) == 0)
                                {
                                    pic.Tag = false;
                                }
                                else
                                {
                                    pic.Tag = true;
                                }
                                return new object[] { PlateException.None, pic };
                            }
                            else
                            {
                                return new object[] { PlateException.None, string.Empty};
                            }
                        }
                        else
                        {
                            return new object[] { PlateException.None, string.Empty };
                        }
                    }
                }
            }
            else
            {
                string format = string.Empty;
                string expression = GetTagDefineExpression(column, op, ref format);
                if (expression == null)
                {
                    return new object[] { PlateException.TagDefineNotFound, string.Format("Can not find Constant Tag:'{0}' in TagDefination", column) };
                }
                else
                {
                    return Expression.GetExpressionValue(expression, row, op, format);
                }
            }
        }

        /// <summary>
        /// process function tag
        /// </summary>
        /// <param name="funcname">The name of function to invoke</param>
        /// <param name="param">The parameters of function to invoke</param>
        /// <param name="op">The officeplate whose tags defination is used</param>
        /// <returns>Object consist of plateexception and value</returns>
        public static object[] ProcessFunctionTag(string funcname, string param, IOfficePlate op)
        {
            string format = string.Empty;
            string expression = GetTagDefineExpression(funcname, op, param, ref format);
            if (expression == null)
            {
                string[] strparam = param.Split(',');
                object[] objparam = new object[strparam.Length];
                if(strparam.Length == 1 && strparam[0].Trim().Length == 0)
                {
                    objparam = null;
                }
                else
                {
                    for (int i = 0; i < strparam.Length; i++)
			        {
			            objparam[i] = strparam[i].Trim();
			        }
                }
                if (op is OfficePlate)
                {
                    return Expression.InvokeMethod(funcname, objparam, (op as OfficePlate).OwnerComp);
                }
                else
                {
                    return Expression.InvokeMethod(funcname, objparam, (op as WebOfficePlate).Page);
                }
            }
            else
            {
                return Expression.GetExpressionValue(expression, null, op, format);
            }
        }
        
        /// <summary>
        /// Get define expression tag
        /// </summary>
        /// <param name="tagName">The name of tag</param>
        /// <param name="op">The officeplate whose tags defination is used</param>
        /// <param name="format">String of format</param>
        /// <returns>The expression of tag</returns>
        public static string GetTagDefineExpression(string tagName, IOfficePlate op, ref string format)
        {
            foreach (TagItem ti in op.Tags)
            {
                if (ti.DataField == tagName)
                {
                    format = ti.Format;
                    return ti.Exp;
                }
            }
            return null;
        }

        /// <summary>
        /// Get define expression tag of function tag
        /// </summary>
        /// <param name="tagName">The name of tag</param>
        /// <param name="op">The officeplate whose tags defination is used</param>
        /// <param name="param">The param of function</param>
        /// <param name="format">String of format</param>
        /// <returns>The expression of tag</returns>
        public static string GetTagDefineExpression(string tagName, IOfficePlate op, string param, ref string format)
        {
            foreach (TagItem ti in op.Tags)
            {
                if (ti.DataField.StartsWith(tagName) && ti.DataField.Length > tagName.Length)
                {
                    string defparam = ti.DataField.Substring(tagName.Length).Trim();
                    if (defparam[0] == '(' && defparam[defparam.Length - 1] == ')')
                    {
                        defparam = defparam.Replace("(", string.Empty).Replace(")", string.Empty);
                        string[] arrparam = param.Split(',');
                        string[] arrdefparm = defparam.Split(',');
                        if (arrparam.Length == arrdefparm.Length)
                        {
                            format = ti.Format;
                            string exp = ti.Exp;
                            for (int i = 0; i < arrparam.Length; i++)
                            {
                                exp = exp.Replace("@" + arrdefparm[i].Trim(), arrparam[i].Trim());
                            }
                            return exp;
                        }
                    }
                }
            }
            return null;
        }
    }

    /// <summary>
    /// The class to convert expressions to values
    /// </summary>
    public class Expression
    {
        /// <summary>
        /// Convert expression to value
        /// </summary>
        /// <param name="expression">String of expression</param>
        /// <param name="dr">Datarow of Table</param>
        /// <param name="op">The officeplate whose tags defination is used</param>
        /// <param name="format">String of format</param>
        /// <returns>Value of result</returns>
        public static object[] GetExpressionValue(string expression, DataRow dr, IOfficePlate op, string format)
        {
            ArrayList list = SplitExpression(expression);
            
            bool isNumExp = true;
            object[] obj = ConvertExpToValue(list, ref isNumExp, dr, op);
            if ((PlateException)obj[0] != PlateException.None)
            {
                return obj;
            }
            else
            {
                if (isNumExp)
                {
                    ((ArrayList)obj[1]).Add("#");
                    ArrayList listRpn = ReversePN.GetRPN((ArrayList)obj[1]);
                    double dbrtn = ReversePN.CaculateValue(listRpn);
                    try
                    {
                        string strrtn = dbrtn.ToString(format);
                        return new object[] { PlateException.None, strrtn };
                    }
                    catch
                    {
                        return new object[] { PlateException.InvalidFormat, string.Format("Format:{0} is invalid", format) };
                    }
                }
                else
                {
                    return BuildString((ArrayList)obj[1]);
                }
            }
        }

        /// <summary>
        /// Split expression to a list
        /// </summary>
        /// <param name="expression">The expression to split</param>
        /// <returns>The list of expression member</returns>
        public static ArrayList SplitExpression(string expression)
        {
            ArrayList lstrtn = new ArrayList();

            Regex rx = new Regex(@"[\(\)\^\*\+/-]");
            MatchCollection mc = rx.Matches(expression);
            string[] arrexpression = rx.Split(expression);
            for (int i = 0; i < mc.Count; i++)
            {
                if (arrexpression[i].Trim().Length > 0)
                {
                    lstrtn.Add(arrexpression[i].Trim());
                }
                lstrtn.Add(mc[i].Value);
            }
            if (!arrexpression[arrexpression.Length - 1].Trim().Equals(string.Empty))
            {
                lstrtn.Add(arrexpression[arrexpression.Length - 1].Trim());
            }

            return lstrtn;
        }

        /// <summary>
        /// Convert each member of list to its value
        /// </summary>
        /// <param name="list">The list to process</param>
        /// <param name="isNumExp">The flag indicates whether is an digital expression</param>
        /// <param name="dr">Datarow of Table</param>
        /// <param name="op">The officeplate whose tags defination is used</param>
        /// <returns>A new list consist of value</returns>
        public static object[] ConvertExpToValue(ArrayList list, ref bool isNumExp, DataRow dr, IOfficePlate op)
        {
            isNumExp = true;
            string strop = "+-*/()^";
            for (int i = 0; i < list.Count; i++)
            {
                string member = list[i].ToString().Trim();
                if (!strop.Contains(member))
                {
                    object objmember = null;
                    if (member.StartsWith("_"))
                    {
                        objmember = GetSysParameters(member);
                        if (objmember == null)
                        {
                            return new object[] {PlateException.SystemParameterNotFound
                                , string.Format("Can not find system parameter:'{0}'", member) };
                        }
                    }
                    else if(member.StartsWith("\\"))
                    { 
                        objmember = member.Substring(1);
                    }
                    else if (member.Split('{', '}').Length == 3)
                    {
                        string[] strfun = member.Split('{', '}');
                        string funcname = strfun[0];
                        string[] strparam = strfun[1].Split(',');
                        object[] objparam = new object[strparam.Length];
                        if (strparam.Length == 1 && strparam[0].Trim().Length == 0)
                        {
                            objparam = null;
                        }
                        else
                        {
                            for (int j = 0; j < strparam.Length; j++)
                            {
                                objparam[j] = strparam[j].Trim();
                            }
                        }
                        object[] obj;
                        if(op is OfficePlate)
                        {
                            obj = Expression.InvokeMethod(funcname, objparam, (op as OfficePlate).OwnerComp);
                        }
                        else
                        {
                            obj = Expression.InvokeMethod(funcname, objparam, (op as WebOfficePlate).Page);
                        }
                        if ((PlateException)obj[0] != PlateException.None)
                        {
                            return obj;
                        }
                        else
                        {
                            objmember = obj[1];
                        }
                    }
                    else if (member.Split('.').Length > 1)
                    {
                        try
                        {
                            objmember = Convert.ToDouble(member);
                        }
                        catch
                        {
                            object[] obj = null;
                            if (op is OfficePlate)
                            {
                                obj = GetPropertyValue(member, op as OfficePlate);
                            }
                            else
                            {
                                obj = GetPropertyValue(member, op as WebOfficePlate);
                            }
                            if ((PlateException)obj[0] != PlateException.None)
                            {
                                return obj;
                            }
                            else
                            {
                                objmember = obj[1];
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            objmember = Convert.ToDouble(member);
                        }
                        catch
                        {
                            if (dr == null)
                            {
                                return new object[] { PlateException.InvalidDataSourceItem
                                , string.Format("DataSource item can not be used in a non-datasouce tag's defination") };
                            }
                            if (dr.Table.Columns.Contains(member))
                            {
                                objmember = dr[member];
                            }
                            else
                            {
                                return new object[] { PlateException.ColumnNotFound
                                , string.Format("Can not find column:'{0}' in datasource", member) };
                            }
                        }

                    }
                    if (objmember is string || objmember is DateTime || objmember is bool)
                    {
                        isNumExp = false;
                    }
                    list[i] = objmember;
                }
            }
            return new object[] { PlateException.None, list };
        }

        /// <summary>
        /// Return value of system parameters
        /// </summary>
        /// <param name="sysparam">Name of system parameters</param>
        /// <returns>Value of system parameters</returns>
        public static object GetSysParameters(string sysparam)
        {
            switch (sysparam.ToLower())
            {
                case "_today": return DateTime.Now.ToShortDateString();
                case "_userid": return Srvtools.CliUtils.fLoginUser;
                case "_username": return Srvtools.CliUtils.fUserName;
                case "_sitecode": return Srvtools.CliUtils.fSiteCode;
                case "_groupid": return Srvtools.CliUtils.fGroupID;
                case "_database": return Srvtools.CliUtils.fLoginDB;
                case "_ipaddress": return Srvtools.CliUtils.fComputerIp;
                case "_solution": return Srvtools.CliUtils.fCurrentProject;
                case "_language": return Srvtools.CliUtils.fClientLang.ToString();
                default: return null;
            }        
        }

        /// <summary>
        /// Invoke a method with param in component
        /// </summary>
        /// <param name="Name">The name of method</param>
        /// <param name="param">The parameters of the method</param>
        /// <param name="Component">The component to invoke method</param>
        /// <returns>The result of invoke method</returns>
        public static object[] InvokeMethod(string Name, object[] param, object Component)
        {
            Type tp = Component.GetType();
            MethodInfo mi = tp.GetMethod(Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            object retvalue = null;
            if (mi != null)
            {
                try
                {
                     retvalue = mi.Invoke(Component, param);
                }
                catch(Exception e)
                {
                    return new object[] { PlateException.InvokeMethodError
                        , string.Format("Invoke method:'{0}()' encounter error:{1}", Name,e.Message) };
                }
            }
            else
            {
                 return new object[]{PlateException.InvokeMethodNotFound, string.Format("Can not find public method:'{0}()'", Name)};
            }
            return new object[]{PlateException.None, retvalue};
        }


        /// <summary>
        /// Get value of a property of a control
        /// </summary>
        /// <param name="prop">The name of property and control</param>
        /// <param name="Owner">The owner of officeplate</param>
        /// <param name="Cont">The container of officeplate</param>
        /// <returns>The value of property</returns>
        public static object[] GetPropertyValue(string prop, OfficePlate plate)
        {
            string[] strprop = prop.Split('.');
            object obj = plate.FindControl(strprop[0], plate.OwnerComp as Srvtools.InfoForm);
            if (obj == null)
            {
                return new object[]{PlateException.InvalidExpression
                    , string.Format("Can not find control:'{0}'",strprop[0])};
            }

            return GetPropertyValue(obj, strprop);
        }

        /// <summary>
        /// Get value of a property of a control
        /// </summary>
        /// <param name="prop">The name of property and control</param>
        /// <param name="Page">The page of webofficeplate</param>
        /// <returns>The value of property</returns>
        public static object[] GetPropertyValue(string prop, WebOfficePlate plate)
        {
            string[] strprop = prop.Split('.');
            object obj = plate.FindControl(strprop[0], plate.Page);
            if (obj == null)
            {
                return new object[]{PlateException.InvalidExpression
                    , string.Format("Can not find control:'{0}'",strprop[0])};
            }
            return GetPropertyValue(obj, strprop);
        }

        private static object[] GetPropertyValue(object obj, string[] strprop)
        {
            Type tp = obj.GetType();
            PropertyInfo pi = tp.GetProperty(strprop[1], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (pi == null)
            {
                return new object[]{PlateException.InvalidExpression
                        , string.Format("Can not find property:'{0}' in component:'{1}'",strprop[1], strprop[0])};
            }
            object objvalue = pi.GetValue(obj, null);
            if (strprop.Length == 2)
            {
                return new object[] { PlateException.None, objvalue };
            }
            else
            {
                PropertyInfo pic = pi.PropertyType.GetProperty("Count", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (pic == null)
                {
                    return new object[]{PlateException.InvalidExpression
                        , string.Format("Can not find property:'Count' in '{0}.{1}'",strprop[0], strprop[1])};
                }
                else
                {
                    return new object[] { PlateException.None, pic.GetValue(objvalue, null) };
                }
            }
        }

        /// <summary>
        /// Build string from a list
        /// </summary>
        /// <param name="list">The list to build</param>
        /// <returns>The result of bulid string</returns>
        public static object[] BuildString(ArrayList list)
        {
            StringBuilder sbuild = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i % 2 == 0)
                {
                    sbuild.Append(list[i].ToString());
                }
                else
                {
                    if (list[i].ToString() != "+")
                        return new object[]{PlateException.InvalidExpression
                        , string.Format("String Expression Only can use operator:'+','{0}' is invalid",list[i].ToString())};
                }
            }
            return new object[] { PlateException.None, sbuild.ToString() };
        }
    }

    /// <summary>
    /// The class of Reverse Polish Novation
    /// </summary>
    public class ReversePN
    {
        /// <summary>
        /// Convert expression to a RPN
        /// </summary>
        /// <param name="lst">The list of expression to do RPN</param>
        /// <returns>The list of expression after RPN</returns>
        public static ArrayList GetRPN(ArrayList lst)
        {
            Stack stkop = new Stack();
            ArrayList rtnlist = new ArrayList();
            for (int i = 0; i < lst.Count; i++)
            {
                try
                {
                    double db = Convert.ToDouble(lst[i]);
                    rtnlist.Add(db);
                }
                catch
                {
                    if (lst[i].ToString() == "(")
                    {
                        stkop.Push("(");
                    }
                    else if (lst[i].ToString() == ")")
                    {
                        while (stkop.Peek().ToString() != "(")
                        {
                            try
                            {
                                rtnlist.Add(stkop.Pop());
                            }
                            catch
                            {
                                return null;
                            }
                        }
                        stkop.Pop();
                    }
                    else if (lst[i].ToString() == "#")
                    {
                        while (stkop.Count > 0)
                        {
                            try
                            {
                                rtnlist.Add(stkop.Pop());
                            }
                            catch
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        while (stkop.Count > 0 && PriOp(stkop.Peek().ToString()) >= PriOp(lst[i].ToString()))
                        {
                            rtnlist.Add(stkop.Pop());
                        }
                        stkop.Push(lst[i]);
                    }
                }

            }
            return rtnlist;
        }

        /// <summary>
        /// Caculate the RPN
        /// </summary>
        /// <param name="lst">The list of expression after RPN</param>
        /// <returns>The result of caculate</returns>
        public static double CaculateValue(ArrayList lst)
        {
            Stack stkop = new Stack();
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i] is double)
                {
                    stkop.Push(lst[i]);
                }
                else
                {
                    double x;
                    double y;
                    try
                    {
                        y = (double)stkop.Pop();
                        x = (double)stkop.Pop();
                    }
                    catch
                    {
                        return Double.NaN;
                    }
                    switch (lst[i].ToString())
                    {
                        case "+": stkop.Push(x + y); break;
                        case "-": stkop.Push(x - y); break;
                        case "*": stkop.Push(x * y); break;
                        case "/": stkop.Push(x / y); break;
                        case "^": stkop.Push(Math.Pow(x, y)); break;
                        default: return double.NaN;
                    }
                }
            }
            return (double)stkop.Pop();
        }

        /// <summary>
        /// Get priority of operator
        /// </summary>
        /// <param name="opr">String of operator</param>
        /// <returns>Integer of priority</returns>
        protected static int PriOp(string opr)
        {
            
            switch (opr)
            {
                case "-":
                case "+": return 1;
                case "*":
                case "/": return 2;
                case "^": return 3;
            }
            return 0;
        }
    }
}
