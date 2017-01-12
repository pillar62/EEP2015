using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using EFClientTools.EFServerReference;
//using Utility;

namespace EFClientTools
{
    public static class EntityObjectExtensionMethods
    {
        #region 实体扩展方法

        /// <summary>
        /// 转换实体信息为Json
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns></returns>
        public static String ToEntitiesJson(this EntityObject obj)
        {
            return new EntitiesTools().EntitiesToJson(obj, "");
        }

        /// <summary>
        /// 转换实体信息为Json
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <param name="Include">获取子对象：使用，号分开</param>
        /// <returns></returns>
        public static String ToEntitiesJson(this EntityObject obj, String Include)
        {
            return new EntitiesTools().EntitiesToJson(obj, Include);
        }
        /// <summary>
        /// 转换实体信息为Json
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns></returns>
        public static String ToEntitiesJson(this IQueryable obj)
        {
            return new EntitiesTools().EntitiesToJson(obj, "");
        }

        /// <summary>
        /// 转换实体信息为Json
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <param name="Include">获取子对象：使用，号分开</param>
        /// <returns></returns>
        public static String ToEntitiesJson(this IQueryable obj, String Include)
        {
            return new EntitiesTools().EntitiesToJson(obj, Include);
        }

        /// <summary>
        /// 转换实体信息为Json
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <param name="Include">获取子对象：使用，号分开</param>
        /// <returns></returns>
        public static String ToEntitiesJson(this IList obj, String Include)
        {
            return new EntitiesTools().EntitiesToJson(obj, Include);
        }

        #endregion
    }


    #region 转换实体信息为Json

    /// <summary>
    /// 转换实体信息为Json
    /// </summary>
    public class EntitiesTools
    {
        #region 转换实体信息为Json
        /// <summary>
        /// 转换实体信息为Json
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <param name="Include">获取子对象：使用，号分开</param>
        /// <returns></returns>
        public String EntitiesToJson(Object obj, String Include)
        {
            int Level = 5;
            Type type = obj.GetType();
            String sJosnString = "";
            StringBuilder TempStringJson = new StringBuilder();
            StringBuilder json = new StringBuilder();
            switch (type.Name)
            {
                case "ObjectQuery`1":
                    TempStringJson.Clear();
                    json.Append("[");
                    foreach (Object _obj in obj as IQueryable)
                    {
                        TempStringJson.Append(EntitiesToJson(_obj, Include, Level));
                        TempStringJson.Append(",");
                    }
                    sJosnString = TempStringJson.ToString();
                    if (sJosnString.Length != 0)
                    {
                        json.Append(sJosnString.Substring(0, sJosnString.Length - 1));
                    }
                    json.Append("]");
                    break;
                case "List`1":
                    TempStringJson.Clear();
                    json.Append("[");
                    if ((obj as IList).Count > 0)
                    {
                        foreach (Object _obj in obj as IList)
                        {
                            TempStringJson.Append(EntitiesToJson(_obj, Include, Level));
                            TempStringJson.Append(",");
                        }
                        sJosnString = TempStringJson.ToString();
                        json.Append(sJosnString.Substring(0, sJosnString.Length - 1));
                    }
                    json.Append("]");
                    break;
                default:
                    json.Append(EntitiesToJson(obj, Include, Level));
                    break;
            }
            return json.ToString();

        }
        /// <summary>
        /// 转换实体信息为Json
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <param name="Include">获取子对象：使用，号分开</param>
        /// <param name="Level">当前递归的层级</param>
        /// <returns></returns>
        private String EntitiesToJson(Object obj, String Include, int Level)
        {
            if (obj == null) { return null; }
            StringBuilder TempStringJson = new StringBuilder();

            List<String> IncludeA = Include.Split(',').ToList<String>();

            //需要排除的属性类型
            List<String> ignoreEntityTypes = new List<String> { "EntityReference`1", "EntityState", "EntityKey", "EntitySetName" };
            //实体集合
            List<String> ListType = new List<String> { "EntityCollection`1", "List`1" };
            //数据类型(非实体集合类型)
            List<String> EntitesType = new List<String> { "Binary", "Boolean", "DateTime", "DateTimeOffset", "Decimal", "Double", "Guid", "Int16", "Int32", "Int64", "Single", "String", "Time" };
            //未支持的数据类型
            List<String> NotType = new List<String> { "Byte", "SByte" };


            StringBuilder json = new StringBuilder();
            json.Append("{");
            Type type = obj.GetType();
            PropertyInfo[] propertyInfoList = type.GetProperties();

            string doubleQuote = "\"";

            #region 迭代属性
            foreach (PropertyInfo _PropertyInfo in propertyInfoList)
            {
                #region 排除类型
                if (ignoreEntityTypes.Contains(_PropertyInfo.PropertyType.Name))
                {
                    continue;
                }
                #endregion

                var propertyName = _PropertyInfo.Name;
                var propertyType = _PropertyInfo.PropertyType;
                var propertyValue = _PropertyInfo.GetValue(obj, null);

                try
                {

                    #region 值=null
                    if (propertyValue == null)
                    {
                        continue;
                        json.Append(doubleQuote + propertyName + doubleQuote +
                                        ":" + "null");
                        json.Append(",");
                        continue;
                    }
                    #endregion

                    #region 非实体集合类型
                    if (EntitesType.Contains(propertyValue.GetType().Name))
                    {
                        try
                        {
                            TempStringJson.Clear();
                            TempStringJson.Append(doubleQuote + propertyName + doubleQuote +
                                        ":" + StringFormat(propertyValue, propertyType));
                            TempStringJson.Append(",");

                            json.Append(TempStringJson);
                            continue;
                        }
                        catch (Exception Err)
                        {
                            throw Err;
                        }

                    }
                    #endregion

                    #region 非Include对象
                    if (!IncludeA.Contains(propertyName))
                    {
                        continue;
                        json.Append(doubleQuote + propertyName + doubleQuote +
                                        ":" + "null");
                        json.Append(",");
                        continue;
                    }
                    #endregion

                    #region 实体集合类型
                    if (ListType.Contains(propertyValue.GetType().Name))
                    {
                        try
                        {
                            TempStringJson.Clear();

                            bool IsNull = true;
                            foreach (Object p in (IEnumerable)propertyValue)
                            {
                                IsNull = false;
                                break;
                            }

                            if (IsNull)
                            {
                                continue;
                                TempStringJson.Append(doubleQuote + propertyName + doubleQuote +
                                                ":" + "null");
                                TempStringJson.Append(",");
                                continue;
                            }

                            StringBuilder ListJosnString = new StringBuilder();
                            json.Append(doubleQuote + propertyName + doubleQuote + ":[");
                            foreach (Object p in (IEnumerable)propertyValue)
                            {
                                String Child = EntitiesToJson(p, Include, 1);
                                ListJosnString.Append(Child);
                                ListJosnString.Append(",");
                            }
                            String sListJosnString = ListJosnString.ToString();
                            TempStringJson.Append(sListJosnString.Substring(0, sListJosnString.Length - 1));
                            TempStringJson.Append("]");
                            TempStringJson.Append(",");


                            json.Append(TempStringJson);
                            continue;
                        }
                        catch (Exception Err)
                        {
                            throw Err;
                        }
                    }
                    #endregion

                    #region
                    try
                    {
                        TempStringJson.Clear();
                        TempStringJson.Append(doubleQuote + propertyName + doubleQuote +
                                        ":" + EntitiesToJson(propertyValue, Include, 1));
                        TempStringJson.Append(",");


                        json.Append(TempStringJson);
                        continue;
                    }
                    catch (Exception Err)
                    {
                        throw Err;
                    }
                    #endregion
                }
                catch (Exception Err)
                {
                    continue;
                    json.Append(doubleQuote + propertyName + doubleQuote +
                                    ":" + "null");
                    json.Append(",");
                    continue;
                }

            }
            #endregion

            String sJosnString = json.ToString();
            return sJosnString.Substring(0, sJosnString.Length - 1) + "}";

        }

        private string StringFormat(object val, Type type)
        {
            string str = val.ToString();
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime) || Nullable.GetUnderlyingType(type) == typeof(DateTime))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
                //str = DateTimeUtils.DateNetToJs((DateTime)val);
                //str = "/" + str + "";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            return str;
        }

        /// <summary>   
        /// 过滤特殊字符   
        /// </summary>   
        /// <param name="s"></param>   
        /// <returns></returns>   
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("////"); break;
                    case '/':
                        sb.Append("///"); break;
                    case '\b':
                        sb.Append("//b"); break;
                    case '\f':
                        sb.Append("//f"); break;
                    case '\n':
                        sb.Append("//n"); break;
                    case '\r':
                        sb.Append("//r"); break;
                    case '\t':
                        sb.Append("//t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        #endregion


    }

    #endregion
}