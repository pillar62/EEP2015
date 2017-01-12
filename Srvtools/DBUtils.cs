using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Collections;
#if MySql
using MySql.Data.MySqlClient;
#endif
#if Informix
using IBM.Data.Informix;
#endif

namespace Srvtools
{
    /// <summary>
    /// Functions of database
    /// </summary>
    public class DBUtils
    {
        /// <summary>
        /// "[", "]"
        /// </summary>
        private static string[] QuoteList = new string[] { "[", "]" };

        /// <summary>
        /// +-*/%&amp;|^=&gt;&lt;~,(
        /// </summary>
        const string OPERATORS = "+-*/%&|^=><~,(";

        /// <summary>
        /// GLModule.cmdDDUse
        /// </summary>
        const string GLMODULE_DD_CMD = "GLModule.cmdDDUse";

        /// <summary>
        /// (?&lt;=[\s\]\)])
        /// </summary>
        const string PREFIX_PATTERN = @"(?<=[\s\]\)]|^)";

        /// <summary>
        /// (?=[\s\[\(])
        /// </summary>
        const string SUFFIX_PATTERN = @"(?=[\s\[\(]|$)";

        /// <summary>
        /// (\w+|\[(\w+\s*)+\])
        /// </summary>
        const string COLUMN_PATTERN = @"(\w+|\[(\w+\s*)+\])";

        /// <summary>
        /// (\w+\s*\.\s*)*(\w+|\[(\w+\s*)+\])
        /// </summary>
        const string TABLE_PATTERN = @"(\w+\s*\.\s*)*(\w+|\[(\w+\$*\s*)+\])";

        /// <summary>
        /// (?&lt;=[\+\-\*/%&amp;\|\^=&gt;&lt;~\s,\(])
        /// </summary>
        const string TABLE_PREFIX_PATTERN = @"(?<=[\+\-\*/%&\|\^=><~\s,\(])";

        [Flags]
        private enum BracketType
        {
            Square = 1,
            Round = 2,
            All = 3
        }

        private static bool IsPartInBracket(string sql, int index, BracketType type)
        {
            string leftString = sql.Substring(0, index);
            if (((int)type & (int)BracketType.Square) > 0)
            {
                if (leftString.Split('[').Length != leftString.Split(']').Length)
                {
                    return true;
                }
            }
            if (((int)type & (int)BracketType.Round) > 0)
            {
                if (leftString.Split('(').Length != leftString.Split(')').Length)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Queto the name of table or column with bracket.
        /// </summary>
        /// <param name="word">The name of table or column.</param>
        /// <param name="databaseType">The type of database.</param>
        /// <returns>The quoted name of table or column.</returns>
        public static string QuoteWords(string word, ClientType databaseType)
        {
            int index = word.LastIndexOf('.');
            if (index == word.Length - 1 || index == 0)
            {
                throw new EEPException(EEPException.ExceptionType.ArgumentInvalid, typeof(DBUtils), null, "word", word);
            }
            else
            {
                switch (databaseType)
                {
                    case ClientType.ctMsSql:
                        {
                            if (index == -1)
                            {
                                return string.Format("[{0}]", word);
                            }
                            else
                            {
                                return string.Format("{0}.[{1}]", word.Substring(0, index), word.Substring(index + 1));
                            }
                        }
                    case ClientType.ctODBC:
                    case ClientType.ctOracle:
                    case ClientType.ctOleDB:
                    case ClientType.ctMySql:
                    case ClientType.ctInformix:
                    default:
                        return word;
                }
            }
        }

        /// <summary>
        /// Remove the quote of name
        /// </summary>
        /// <param name="value">The name of table or column.</param>
        /// <returns>The name of table or column without bracket.</returns>
        public static string RemoveQuote(string value)
        {
            string rtn = value;
            foreach (string str in QuoteList)
            {
                rtn = rtn.Replace(str, string.Empty);
            }
            return rtn;
        }

        /// <summary>
        /// Get the table name of sql command text.
        /// </summary>
        /// <param name="sql">Sql Command text.</param>
        /// <param name="origianName">The value indicating whether get the origian table name.</param>
        /// <returns>The table name</returns>
        public static string GetTableName(string sql, bool origianName)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new EEPException(EEPException.ExceptionType.ArgumentNull, typeof(DBUtils), null, "sql", sql);
            }
            //删除所有括号中的东西
            if (sql.Split('(').Length != sql.Split(')').Length)
            {
                throw new EEPException(EEPException.ExceptionType.ArgumentInvalid, typeof(DBUtils), null, "sql", sql);
            }
            int rightindex = sql.IndexOf(')');
            while (rightindex != -1)
            {
                int leftindex = sql.Substring(0, rightindex).LastIndexOf('(');
                if (leftindex == -1)
                {
                    throw new EEPException(EEPException.ExceptionType.ArgumentInvalid, typeof(DBUtils), null, "sql", sql);
                }
                sql = sql.Remove(leftindex, rightindex - leftindex + 1);
                rightindex = sql.IndexOf(')');
            }

            //找From的位置
            int fromindex = -1;
            Match match = Regex.Match(sql, PREFIX_PATTERN + @"from" + SUFFIX_PATTERN, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                fromindex = match.Index + match.Length;
            }
            if (fromindex == -1 || fromindex >= sql.Length)
            {
                if (sql.StartsWith("exec", StringComparison.OrdinalIgnoreCase))
                {
                    return string.Empty;
                }
                else
                {
                    return string.Empty; //command commandtype=storeprocedure
                    //throw new EEPException(EEPException.ExceptionType.ArgumentInvalid, typeof(DBUtils), null, "sql", sql);
                }
            }

            //找Join, Where, Group by, Order by的位置
            int index = -1;
            StringBuilder patternBuilder = new StringBuilder();
            patternBuilder.Append(PREFIX_PATTERN);
            patternBuilder.Append(@"(");
            patternBuilder.Append(@"((left(\s+\bouter)?)");
            patternBuilder.Append(@"|(right(\s+\bouter)?)");
            patternBuilder.Append(@"|(full(\s+\bouter)?)");
            patternBuilder.Append(@"|(inner)");
            patternBuilder.Append(@"|(cross))");
            patternBuilder.Append(@"\b\s+\b)?");
            patternBuilder.Append(@"join");
            patternBuilder.Append(SUFFIX_PATTERN);
            string[] patterns = new string[] { patternBuilder.ToString()
                , PREFIX_PATTERN + @"where" + SUFFIX_PATTERN
                , PREFIX_PATTERN + @"group\b\s+\bby" + SUFFIX_PATTERN
                , PREFIX_PATTERN + @"having" + SUFFIX_PATTERN
                , PREFIX_PATTERN + @"order\b\s+\bby" + SUFFIX_PATTERN };

            foreach (string pattern in patterns)
            {
                match = Regex.Match(sql, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    index = match.Index;
                    break;
                }
            }
            string tablename;
            if (index == -1)
            {
                tablename = sql.Substring(fromindex).Split(',')[0];
            }
            else if (index > fromindex)
            {
                tablename = sql.Substring(fromindex, index - fromindex).Split(',')[0];
            }
            else
            {
                return string.Empty;
            }
            tablename = tablename.Trim();


            //找AS和空格, 不支持[table]alias
            patterns = new string[] { PREFIX_PATTERN + @"as" + SUFFIX_PATTERN, @"\s+" };
            foreach (string pattern in patterns)
            {
                MatchCollection matches = Regex.Matches(tablename, pattern, RegexOptions.IgnoreCase);

                foreach (Match mc in matches)
                {
                    if (!IsPartInBracket(tablename, mc.Index, BracketType.Square))
                    {
                        if (mc.Index == -1 || mc.Index + mc.Length == tablename.Length)
                        {
                            throw new EEPException(EEPException.ExceptionType.ArgumentInvalid, typeof(DBUtils), null, "sql", sql);
                        }
                        string origianname = tablename.Substring(0, mc.Index).Trim();
                        string aliasname = tablename.Substring(mc.Index + mc.Length).Trim();
                        if (origianName)
                        {
                            return origianname;
                        }
                        else
                        {
                            return aliasname;
                        }
                    }
                }
            }

            return tablename;
        }

        /// <summary>
        /// Get the name of table column owned.
        /// </summary>
        /// <param name="sql">Sql Command text.</param>
        /// <param name="columnName">The name of column.</param>
        /// <param name="databaseType">The database type.</param>
        /// <returns>Yhe name of table column owned.</returns>
        public static string GetTableNameForColumn(string sql, string columnName, ClientType databaseType)
        {
            //找[columnName]
            StringBuilder patternBuilder = new StringBuilder();
            patternBuilder.Append(@"\[");
            patternBuilder.Append(columnName);
            patternBuilder.Append(@"\s*\]");
            Match mc = Regex.Match(sql, patternBuilder.ToString(), RegexOptions.IgnoreCase);
            if (!mc.Success)
            {
                //找columnName
                patternBuilder = new StringBuilder();
                patternBuilder.Append(@"(?<=[\+\-\*/%&\|\^=><~\s,.\(\)])");
                patternBuilder.Append(columnName);
                patternBuilder.Append(@"(?=[\+\-\*/%&\|\^=><~\s,\)!]|$)");
                MatchCollection mcs = Regex.Matches(sql, patternBuilder.ToString(), RegexOptions.IgnoreCase);
                foreach (Match mcv in mcs)
                {
                    if (!IsPartInBracket(sql, mcv.Index, BracketType.Square))
                    {
                        mc = mcv;
                        break;
                    }
                }
            }
            if (mc.Success)
            {
                if (mc.Index > 0)
                {
                    //判断栏位后面是否是等号
                    if (mc.Index + mc.Length < sql.Length - 1)
                    {
                        string subDefine = sql.Substring(mc.Index + mc.Length).Trim();
                        if (subDefine.StartsWith("="))
                        {
                            subDefine = subDefine.TrimStart('=');
                            //找到From的位置
                            MatchCollection matches = Regex.Matches(subDefine, PREFIX_PATTERN + @"from" + SUFFIX_PATTERN, RegexOptions.IgnoreCase);
                            foreach (Match mcFrom in matches)
                            {
                                if (mcFrom.Index > 0 && !IsPartInBracket(subDefine, mcFrom.Index, BracketType.All))
                                {
                                    //找逗号
                                    MatchCollection commas = Regex.Matches(subDefine, "[,|)]");
                                    foreach (Match comma in commas)
                                    {
                                        if (comma.Index > 0 && !IsPartInBracket(subDefine, comma.Index, BracketType.Round))
                                        {
                                            return subDefine.Substring(0, comma.Index);
                                        }
                                    }
                                    return subDefine.Substring(0, mcFrom.Index).Trim();
                                }
                            }
                        }

                    }

                    string subString = sql.Substring(0, mc.Index).Trim();
                    //如果前面是关键字,直接返回
                    Match mcSub = Regex.Match(subString, PREFIX_PATTERN + @"(select|where|by|having|all|and|any|between|exists|in|like|not|or|some|distinct|from)"
                        , RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                    if (mcSub.Success && mcSub.Index + mcSub.Length == subString.Length)
                    {
                        return mc.Value;
                    }
                    char lastChar = subString[subString.Length - 1];
                    //如果前面是操作符,直接返回
                    if (OPERATORS.Contains(lastChar.ToString()))
                    {
                        return mc.Value;
                    }
                    //如果前面是.,找表名
                    else if (lastChar.Equals('.'))
                    {
                        subString = subString.TrimEnd(". ".ToCharArray());
                        mcSub = Regex.Match(subString, TABLE_PREFIX_PATTERN + TABLE_PATTERN
                            , RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                        if (mcSub.Success)
                        {
                            return string.Format("{0}.{1}", mcSub.Value, mc.Value);
                        }
                    }
                    else
                    {
                        //如果前面有as,就去掉as
                        mcSub = Regex.Match(subString, PREFIX_PATTERN + "as", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                        if (mcSub.Success && mcSub.Index + mcSub.Length == subString.Length)
                        {
                            subString = subString.Substring(0, mcSub.Index).Trim();
                        }
                        lastChar = subString[subString.Length - 1];
                        //如果前面是右括号,就找到匹配的左括号
                        if (lastChar.Equals(')'))
                        {
                            MatchCollection mcs = Regex.Matches(subString, TABLE_PREFIX_PATTERN + @"\w*\(", RegexOptions.RightToLeft);
                            foreach (Match mcv in mcs)
                            {
                                string subBracketString = subString.Substring(mcv.Index);
                                if (subBracketString.Split('(').Length == subBracketString.Split(')').Length)
                                {
                                    if (mcv.Value.StartsWith("top", StringComparison.OrdinalIgnoreCase))
                                    {
                                        return mc.Value;
                                    }
                                    else
                                    {
                                        return subBracketString;
                                    }
                                }
                            }
                        }
                        //连表名一起找column的原名
                        else
                        {
                            mcSub = Regex.Match(subString, TABLE_PREFIX_PATTERN + "(" + TABLE_PATTERN + @"\s*\.\s*)?" + COLUMN_PATTERN
                                , RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                            if (mcSub.Success)
                            {
                                return mcSub.Value;
                            }
                        }
                    }
                }
            }
            string tableName = GetTableName(sql, false);
            return string.Format("{0}.{1}", tableName, QuoteWords(columnName, databaseType));
        }

        /// <summary>
        /// Insert the where part to a sql command text.
        /// </summary>
        /// <param name="sql">Sql Command text.</param>
        /// <param name="where">The where part.</param>
        /// <returns>The sql command text after where part inserted.</returns>
        public static string InsertWhere(string sql, string where)
        {
            if (string.IsNullOrEmpty(where))
            {
                return sql;
            }
            else
            {
                //找到Where, 就插入在后面
                MatchCollection matches = Regex.Matches(sql, PREFIX_PATTERN + @"where" + SUFFIX_PATTERN, RegexOptions.IgnoreCase);
                foreach (Match mc in matches)
                {
                    if (!IsPartInBracket(sql, mc.Index, BracketType.All))
                    {
                        return sql.Insert(mc.Index + mc.Length, string.Format(" ({0}) AND", where));
                    }
                }
                //找到group by, having, order by就插在前面
                string[] patterns = new string[] { PREFIX_PATTERN + @"group\b\s+\bby" + SUFFIX_PATTERN
                    , PREFIX_PATTERN + @"having" + SUFFIX_PATTERN
                    , PREFIX_PATTERN + @"order\b\s+\bby" + SUFFIX_PATTERN };

                foreach (string pattern in patterns)
                {
                    matches = Regex.Matches(sql, pattern, RegexOptions.IgnoreCase);
                    foreach (Match mc in matches)
                    {
                        if (!IsPartInBracket(sql, mc.Index, BracketType.All))
                        {
                            return sql.Insert(mc.Index, string.Format("WHERE ({0}) ", where));
                        }
                    }
                }
                //什么都没找到,就插在最后面
                return string.Format("{0} WHERE ({1})", sql, where);
            }
        }

        /// <summary>
        /// Insert the order part to a sql command text.
        /// </summary>
        /// <param name="sql">Sql Command text.</param>
        /// <param name="order">The order part.</param>
        /// <returns>The sql command text after order part inserted.</returns>
        public static string InsertOrder(string sql, string order)
        {
            if (string.IsNullOrEmpty(order))
            {
                return sql;
            }
            else
            {
                //找到Order by, 就插入在后面
                MatchCollection matches = Regex.Matches(sql, PREFIX_PATTERN + @"order\b(\s+)\bby" + SUFFIX_PATTERN, RegexOptions.IgnoreCase);
                foreach (Match mc in matches)
                {
                    if (!IsPartInBracket(sql, mc.Index, BracketType.All))
                    {
                        int orderindex = mc.Index;
                        sql = sql.Remove(mc.Index + mc.Length);
                        return sql.Insert(mc.Index + mc.Length, string.Format(" {0}", order));

                    }
                }
                return string.Format("{0} ORDER BY {1}", sql, order);
            }
        }


        /// <summary>
        /// Insert the top part to a sql command text.
        /// </summary>
        /// <param name="sql">Sql Command text.</param>
        /// <param name="top">The number of top.</param>
        /// <param name="databaseType">The database type.</param>
        /// <returns>The sql command text after top part inserted.</returns>
        public static string InsertTop(string sql, int top, ClientType databaseType)
        {
            switch (databaseType)
            {
                case ClientType.ctMsSql:
                case ClientType.ctODBC:
                case ClientType.ctOleDB:
                    {
                        string topString = databaseType == ClientType.ctODBC ? "FIRST" : "TOP";
                        Match mc = Regex.Match(sql, @"\bselect(?=[\s\[\(])", RegexOptions.IgnoreCase);
                        if (mc.Success)
                        {
                            return sql.Insert(mc.Index + mc.Length, string.Format(" {0} {1}", topString, top));
                        }
                        else
                        {
                            throw new EEPException(EEPException.ExceptionType.MethodNotSupported, typeof(DBUtils), null, "InsertTop", null);
                        }
                    }
                case ClientType.ctOracle:
                    {
                        return DBUtils.InsertWhere(sql, string.Format("ROWNUM <= {0}", top));
                    }
                case ClientType.ctMySql:
                    {
                        return string.Format("{0} LIMIT 0, {1}", sql, top);
                    }
                case ClientType.ctInformix:
                    {
                        string topString = "FIRST";
                        Match mc = Regex.Match(sql, @"\bselect(?=[\s\[\(])", RegexOptions.IgnoreCase);
                        if (mc.Success)
                        {
                            return sql.Insert(mc.Index + mc.Length, string.Format(" {0} {1}", topString, top));
                        }
                        else
                        {
                            throw new EEPException(EEPException.ExceptionType.MethodNotSupported, typeof(DBUtils), null, "InsertTop", null);
                        }
                    }
                default:
                    {
                        throw new EEPException(EEPException.ExceptionType.MethodNotSupported, typeof(DBUtils), null, "InsertTop", null);
                    }
            }
        }

        /// <summary>
        /// Insert the top part to a sql command text.
        /// </summary>
        /// <param name="sql">Sql Command text.</param>
        /// <param name="count">The number of top.</param>
        /// <param name="startIndex"></param>
        /// <param name="databaseType">The database type.</param>
        /// <returns>The sql command text after top part inserted.</returns>
        public static string InsertTop(string sql, int startIndex, int count, string order, ClientType datasbaseType)
        {
            MatchCollection mcs = Regex.Matches(sql, PREFIX_PATTERN + @"from\s+" + TABLE_PATTERN, RegexOptions.IgnoreCase);
            foreach (Match mc in mcs)
            {
                if (!IsPartInBracket(sql, mc.Index, BracketType.All))
                {
                    MatchCollection matches = Regex.Matches(sql, PREFIX_PATTERN + @"order\b(\s+)\bby" + SUFFIX_PATTERN, RegexOptions.IgnoreCase);
                    foreach (Match mco in matches)
                    {
                        if (!IsPartInBracket(sql, mco.Index, BracketType.All))
                        {
                            if (mco.Index > mc.Index)
                            {
                                order = sql.Substring(mco.Index + mco.Length);
                                sql = sql.Remove(mco.Index);
                                break;
                            }
                        }
                    }

                    string leftPart = sql.Substring(0, mc.Index);

                    string rightPart = sql.Substring(mc.Index);

                    //return string.Format("SELECT COUNT(*) {0}", sql.Substring(mc.Index));

                    if (datasbaseType == ClientType.ctMsSql)
                    {
                        return string.Format("SELECT TOP {0} * FROM ({1}, row_number() over (order by {2}) as ROW_NUM {3}) AS O WHERE ROW_NUM > {4} ORDER BY ROW_NUM"
                            , count, leftPart, order, rightPart, startIndex);
                    }
                    else if (datasbaseType == ClientType.ctOracle)
                    {
                        String sReturnValue = "SELECT * FROM ({0}, ROWNUM ROWNUM_ORACLE {1}) WHERE ROWNUM_ORACLE > {2} ORDER BY ROWNUM_ORACLE";
                        rightPart = InsertWhere(rightPart, String.Format("ROWNUM <={0}", count + startIndex));
                        return string.Format(sReturnValue, leftPart, rightPart, startIndex);

                        //select * from(select rownum no ,id,name from student) where no>2;
                    }
                    else if (datasbaseType == ClientType.ctInformix || datasbaseType == ClientType.ctODBC)
                    {
                        leftPart = leftPart.TrimStart().Remove(0, 6);
                        return string.Format("SELECT SKIP {0} FIRST {1} {2} {3}"
                            , startIndex, count, leftPart, rightPart);
                    }
                    else if (datasbaseType == ClientType.ctMySql)
                    {
                        //select * from mytable LIMIT offset, recnum
                        return string.Format(" {0} LIMIT {1},{2} ", sql, startIndex, count);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            throw new EEPException(EEPException.ExceptionType.ArgumentInvalid, typeof(DBUtils), null, "sql", sql);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool HasPartEffectCount(string sql)
        {
            if(sql.StartsWith("exec", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return Regex.IsMatch(sql, PREFIX_PATTERN + @"(distinct|group\s+by|union)" + SUFFIX_PATTERN, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Insert the count part to a sql command text.
        /// </summary>
        /// <param name="sql">Sql Command text.</param>
        /// <returns>The sql command text after count part inserted.</returns>
        public static string InsertCount(string sql)
        {
            MatchCollection mcs = Regex.Matches(sql, PREFIX_PATTERN + @"from\s+" + TABLE_PATTERN, RegexOptions.IgnoreCase);
            foreach (Match mc in mcs)
            {
                if (!IsPartInBracket(sql, mc.Index, BracketType.All))
                {
                    MatchCollection matches = Regex.Matches(sql, PREFIX_PATTERN + @"order\b(\s+)\bby" + SUFFIX_PATTERN, RegexOptions.IgnoreCase);
                    foreach (Match mco in matches)
                    {
                        if (!IsPartInBracket(sql, mco.Index, BracketType.All))
                        {
                            if (mco.Index > mc.Index)
                            {
                                sql = sql.Remove(mco.Index);
                                break;
                            }
                        }
                    }

                    return string.Format("SELECT COUNT(*) {0}", sql.Substring(mc.Index));
                }
            }
            throw new EEPException(EEPException.ExceptionType.ArgumentInvalid, typeof(DBUtils), null, "sql", sql);
        }

        public static string InsertTotal(string sql, Dictionary<string, string> totals, ClientType databaseType)
        {
            if (totals.Count == 0)
            {
                throw new EEPException(EEPException.ExceptionType.PropertyInvalid, typeof(DBUtils), null, "Totals.Count", "0"); ;
            }
            MatchCollection mcs = Regex.Matches(sql, PREFIX_PATTERN + @"from\s+" + TABLE_PATTERN, RegexOptions.IgnoreCase);
            foreach (Match mc in mcs)
            {
                if (!IsPartInBracket(sql, mc.Index, BracketType.All))
                {
                    MatchCollection matches = Regex.Matches(sql, PREFIX_PATTERN + @"order\b(\s+)\bby" + SUFFIX_PATTERN, RegexOptions.IgnoreCase);
                    foreach (Match mco in matches)
                    {
                        if (!IsPartInBracket(sql, mco.Index, BracketType.All))
                        {
                            if (mco.Index > mc.Index)
                            {
                                sql = sql.Remove(mco.Index);
                                break;
                            }
                        }
                    }

                    List<string> totalParts = new List<string>();
                    foreach (string key in totals.Keys)
                    {
                        totalParts.Add(string.Format("{0}({1}) as {2}", totals[key], DBUtils.GetTableNameForColumn(sql, key, databaseType), key));
                    }

                    return string.Format("SELECT {0} {1}", string.Join(",", totalParts), sql.Substring(mc.Index));
                }
            }
            throw new EEPException(EEPException.ExceptionType.ArgumentInvalid, typeof(DBUtils), null, "sql", sql);

        }

        /// <summary>
        /// Replace the table name of sql command text.
        /// </summary>
        /// <param name="sql">Sql Command text.</param>
        /// <param name="tableSuffix">The suffix of table name.</param>
        /// <returns>The sql command text after replace table name</returns>
        public static string ReplaceTableName(string sql, string tableSuffix)
        {
            MatchCollection matches = Regex.Matches(sql, PREFIX_PATTERN + @"from\s+" + TABLE_PATTERN, RegexOptions.IgnoreCase);
            foreach (Match mc in matches)
            {
                if (!IsPartInBracket(sql, mc.Index, BracketType.All))
                {
                    if (mc.Value.EndsWith("]"))
                    {
                        return sql.Insert(mc.Index + mc.Length - 1, tableSuffix);
                    }
                    else
                    {
                        return sql.Insert(mc.Index + mc.Length, tableSuffix);
                    }
                }
            }
            throw new EEPException(EEPException.ExceptionType.ArgumentInvalid, typeof(DBUtils), null, "sql", sql);
        }

        /// <summary>
        /// Get format of column.
        /// </summary>
        /// <param name="dataType">The datatype of column.</param>
        /// <param name="dataBaseType">The database type.</param>
        /// <param name="odbcType">The odbc database type.</param>
        /// <param name="index">The index of column.</param>
        /// <returns>The format of column.</returns>
        public static string GetWhereFormat(Type dataType, ClientType dataBaseType, OdbcDBType odbcType, int index)
        {
            if (dataType == typeof(string) || dataType == typeof(char) || dataType == typeof(Guid))
            {
                return string.Format("'{{{0}}}'", index);
            }
            else if (dataType == typeof(DateTime))
            {
                switch (dataBaseType)
                {
                    case ClientType.ctMsSql:
                    case ClientType.ctOleDB:
                    case ClientType.ctMySql:
                        {
                            return string.Format("'{{{0}:yyyy-MM-dd HH:mm:ss}}'", index);
                        }
                    case ClientType.ctOracle:
                        {
                            return string.Format("to_date('{{{0}:yyyy-MM-dd HH:mm:ss}}','yyyy-mm-dd hh24:mi:ss')", index);
                        }
                    case ClientType.ctODBC:
                        {
                            if (odbcType == OdbcDBType.InfoMix)
                            {
                                return string.Format("to_Date('{{{0}:yyyyMMddHHmmss}}', '%Y%m%d%H%M%S')", index);
                            }
                            else if (odbcType == OdbcDBType.FoxPro)
                            {
                                return string.Format("{{{{{{{0}:MM/dd/yyyy}}}}}}", index);
                            }
                            else
                            {
                                return string.Format("'{{{0}:yyyy-MM-dd HH:mm:ss}}'", index);
                            }
                        }
                    case ClientType.ctInformix:
                        {
                            return string.Format("to_Date('{{{0}:yyyyMMddHHmmss}}', '%Y%m%d%H%M%S')", index);
                        }
                    default: throw new EEPException(EEPException.ExceptionType.MethodNotSupported, typeof(DBUtils), null, "GetWhereFormat", null);
                }
            }
            else
            {
                return string.Format("{{{0}}}", index);
            }
        }

        /// <summary>
        /// Get value of column.
        /// </summary>
        /// <param name="dataType">The datatype of column.</param>
        /// <param name="value">The value of column.</param>
        /// <returns>The value of column.</returns>
        public static object GetWhereValue(Type dataType, object value)
        {
            if (dataType == typeof(string))
            {
                if (!string.IsNullOrEmpty((string)value))
                {
                    return ((string)value).Replace("'", "''");
                }
            }
            else if (dataType == typeof(bool))
            {
                return value.Equals(true) ? 1 : 0;
            }
            return value;
        }

        /// <summary>
        /// Get format of column filter.
        /// </summary>
        /// <param name="dataType">The dataType of column</param>
        /// <param name="index">The index of column.</param>
        /// <returns>The format of column filter.</returns>
        public static string GetFilterFormat(Type dataType, int index)
        {
            if (dataType == typeof(string) || dataType == typeof(char) || dataType == typeof(Guid))
            {
                return string.Format("'{{{0}}}'", index);
            }
            else if (dataType == typeof(DateTime))
            {
                return string.Format("'{{{0}:yyyy-MM-dd HH:mm:ss.fff}}'", index);
            }
            else
            {
                return string.Format("{{{0}}}", index);
            }
        }

        /// <summary>
        /// Get command text of infodataset.
        /// </summary>
        /// <param name="dataSet">The infodataset.</param>
        /// <param name="dataMember">The datamember of dataset.</param>
        /// <returns>The command text of infodataset.</returns>
        public static string GetCommandText(InfoDataSet dataSet, string dataMember)
        {
            return GetCommandText(dataSet, dataMember, false);
        }

        private static string GetCommandText(InfoDataSet dataSet, string dataMember, bool designTime)
        {
            string project = designTime ? EditionDifference.ActiveSolutionName() : CliUtils.fCurrentProject;
            if (!string.IsNullOrEmpty(dataSet.RefCommandText))
            {
                return dataSet.RefCommandText;
            }
            else if (!string.IsNullOrEmpty(dataSet.RemoteName))
            {
                int index = dataSet.RemoteName.LastIndexOf('.');
                if (index <= 0 || index == dataSet.RemoteName.Length - 1)
                {
                    throw new EEPException(EEPException.ExceptionType.PropertyInvalid, dataSet.GetType(), null, "RemoteName", dataSet.RemoteName);
                }
                if (string.IsNullOrEmpty(dataMember))
                {
                    dataMember = dataSet.RemoteName.Substring(index + 1);
                }
                return CliUtils.GetSqlCommandText(dataSet.RemoteName.Substring(0, index), dataMember, project);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get command text of infobindingsource.
        /// </summary>
        /// <param name="bindingSource">The infobindingsource.</param>
        /// <returns>The command text of infobindingsource.</returns>
        public static string GetCommandText(InfoBindingSource bindingSource)
        {
            return GetCommandText(bindingSource, false);
        }

        private static string GetCommandText(InfoBindingSource bindingSource, bool designTime)
        {
            string project = designTime ? EditionDifference.ActiveSolutionName() : CliUtils.fCurrentProject;
            InfoDataSet dataSet = bindingSource.GetDataSource();
            if (dataSet == null)
            {
                throw new EEPException(EEPException.ExceptionType.PropertyInvalid, bindingSource.GetType(), null, "DataSource", null);
            }
            string dataMember = bindingSource.GetTableName();
            if (string.IsNullOrEmpty(dataMember))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, bindingSource.GetType(), null, "DataMember", null);
            }
            return GetCommandText(dataSet, dataMember, designTime);
        }

        /// <summary>
        /// Get command text of webdatasource.
        /// </summary>
        /// <param name="dataSource">The WebDataSource.</param>
        /// <returns>The command text of webdatasource.</returns>
        public static string GetCommandText(WebDataSource dataSource)
        {
            return GetCommandText(dataSource, false);
        }

        private static string GetCommandText(WebDataSource dataSource, bool designTime)
        {
            string project = designTime ? EditionDifference.ActiveSolutionName() : CliUtils.fCurrentProject;
            if (!string.IsNullOrEmpty(dataSource.SelectCommand) && !string.IsNullOrEmpty(dataSource.SelectAlias))
            {
                return dataSource.SelectCommand;
            }
            else
            {
                WebDataSource master = dataSource.MasterWebDataSource;
                if (!string.IsNullOrEmpty(master.RemoteName))
                {
                    string tableName = dataSource.DataMember;
                    if (string.IsNullOrEmpty(tableName))
                    {
                        throw new EEPException(EEPException.ExceptionType.PropertyNull, dataSource.GetType(), dataSource.ID, "DataMember", null);
                    }
                    int index = master.RemoteName.LastIndexOf('.');
                    if (index <= 0 || index == master.RemoteName.Length - 1)
                    {
                        throw new EEPException(EEPException.ExceptionType.PropertyInvalid, master.GetType(), master.ID, "RemoteName", master.RemoteName);
                    }
                    return CliUtils.GetSqlCommandText(master.RemoteName.Substring(0, index), tableName, project);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Get data dictionary of infodataset.
        /// </summary>
        /// <param name="dataSet">The infodataset.</param>
        /// <param name="dataMember">The datamember of dataset.</param>
        /// <param name="designTime">A value indicating whether is in design mode.</param>
        /// <returns>The data dictionary of infodataset.</returns>
        public static DataSet GetDataDictionary(InfoDataSet dataSet, string dataMember, bool designTime)
        {
            string project = designTime ? EditionDifference.ActiveSolutionName() : CliUtils.fCurrentProject;
            string dataBase = string.IsNullOrEmpty(dataSet.RefCommandText) ? string.Empty : dataSet.RefDBAlias;
            string remoteName = dataSet.RemoteName;
            string sql = GetCommandText(dataSet, dataMember, designTime);
            return GetDataDictionary(sql, remoteName, dataBase, project);
        }

        /// <summary>
        ///  Get data dictionary of infobindingsource.
        /// </summary>
        /// <param name="bindingSource">The infobindingsource.</param>
        /// <param name="designTime">A value indicating whether is in design mode.</param>
        /// <returns>The data dictionary of infobindingsource.</returns>
        public static DataSet GetDataDictionary(InfoBindingSource bindingSource, bool designTime)
        {
            InfoDataSet dataSet = bindingSource.GetDataSource();
            if (dataSet == null)
            {
                throw new EEPException(EEPException.ExceptionType.PropertyInvalid, bindingSource.GetType(), null, "DataSource", null);
            }
            string dataMember = bindingSource.GetTableName();
            if (string.IsNullOrEmpty(dataMember))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, bindingSource.GetType(), null, "DataMember", null);
            }
            return GetDataDictionary(dataSet, dataMember, designTime);
        }

        /// <summary>
        /// Get data dictionary of webdatasource.
        /// </summary>
        /// <param name="dataSource">The WebDataSource.</param>
        /// <param name="designTime">A value indicating whether is in design mode.</param>
        /// <returns>The data dictionary of webdatasource.</returns>
        public static DataSet GetDataDictionary(WebDataSource dataSource, bool designTime)
        {
            string project = designTime ? EditionDifference.ActiveSolutionName() : CliUtils.fCurrentProject;
            string dataBase = !string.IsNullOrEmpty(dataSource.SelectCommand) && !string.IsNullOrEmpty(dataSource.SelectAlias) && designTime
                ? dataSource.SelectAlias : string.Empty;
            string remoteName = string.Empty;
            if (string.IsNullOrEmpty(dataSource.SelectCommand) || string.IsNullOrEmpty(dataSource.SelectAlias))
            {
                WebDataSource master = dataSource.MasterWebDataSource;
                remoteName = master.RemoteName;
            }
            string sql = GetCommandText(dataSource, designTime);
            return GetDataDictionary(sql, remoteName, dataBase, project);
        }

        private static DataSet GetDataDictionary(string sql, string remoteName, string dataBase, string project)
        {
            string tableName = string.IsNullOrEmpty(sql) ? string.Empty : RemoveQuote(DBUtils.GetTableName(sql, true));
            if (tableName.Contains("."))
                tableName = tableName.Split('.')[1];
            string selectSql = string.Format("select * from COLDEF where TABLE_NAME='{0}'", tableName);
            if (string.IsNullOrEmpty(remoteName))
            {
                remoteName = GLMODULE_DD_CMD;
            }
            int index = remoteName.LastIndexOf('.');
            if (index <= 0 || index == remoteName.Length - 1)
            {
                throw new EEPException(EEPException.ExceptionType.PropertyInvalid, typeof(DBUtils), null, "RemoteName", remoteName);
            }
            return CliUtils.ExecuteSql(remoteName.Substring(0, index), remoteName.Substring(index + 1), selectSql, dataBase, true, project, null, null, false);
        }

        /// <summary>
        /// Create idbdataadapter on an idbcommand
        /// </summary>
        /// <param name="cmd">The idbcomand to create adapter</param>
        /// <returns>The idbdataadapter on an idbcommand</returns>
        public static IDbDataAdapter CreateDbDataAdapter(IDbCommand cmd)
        {
            IDbDataAdapter adapter = null;
            if (cmd is SqlCommand)
            {
                adapter = new SqlDataAdapter();
            }
            else if (cmd is OracleCommand)
            {
                adapter = new OracleDataAdapter();
            }
            else if (cmd is OdbcCommand)
            {
                adapter = new OdbcDataAdapter();
            }
            else if (cmd is OleDbCommand)
            {
                adapter = new OleDbDataAdapter();
            }
#if MySql
            else if (cmd.GetType().FullName == "MySql.Data.MySqlClient.MySqlCommand")
            {
                adapter = new MySqlDataAdapter();
            }
#endif
#if Informix
            else if (cmd is IfxCommand)
            {
                adapter = new IfxDataAdapter();
            }
#endif
#if Sybase
            else if (cmd is Sybase.Data.AseClient.AseCommand)
            {
                adapter = new Sybase.Data.AseClient.AseDataAdapter();
            }
#endif
            else if (cmd is InfoCommand)
            {
                return (cmd as InfoCommand).CreateDbDataAdapter();
            }
            else
            {
                throw new EEPException(EEPException.ExceptionType.MethodNotSupported, typeof(DBUtils), null, "CreateDbDataAdapter", null);
            }
            adapter.SelectCommand = cmd;
            return adapter;
        }

        /// <summary>
        /// Get database type.
        /// </summary>
        /// <param name="connection">The connection of database.</param>
        /// <returns>The database type.</returns>
        public static ClientType GetDatabaseType(IDbConnection connection)
        {
            if (connection is SqlConnection)
            {
                return ClientType.ctMsSql;
            }
            else if (connection is OracleConnection)
            {
                return ClientType.ctOracle;
            }
            else if (connection is OdbcConnection)
            {
                return ClientType.ctODBC;
            }
            else if (connection is OleDbConnection)
            {
                return ClientType.ctOleDB;

            }
#if MySql
            else if (connection.GetType().Name == "MySqlConnection")
            {
                return ClientType.ctMySql;
            }
#endif
#if Informix
            else if (connection is IfxConnection)
            {
                return ClientType.ctInformix;
            }
#endif
#if Sybase
            else if (connection is Sybase.Data.AseClient.AseConnection)
            {
                return ClientType.ctSybase;
            }
#endif
            else if (connection is InfoConnection)
            {
                return (connection as InfoConnection).Type;
            }
            else
            {
                throw new EEPException(EEPException.ExceptionType.MethodNotSupported, typeof(DBUtils), null, "GetdatabaseType", null);
            }
        }

        private static Hashtable OdbcTable = new Hashtable();//缓存odbc类型

        /// <summary>
        /// Get odbc database type.
        /// </summary>
        /// <param name="connection">The connection of database.</param>
        /// <returns>The odbc database type.</returns>
        public static OdbcDBType GetOdbcDatabaseType(IDbConnection connection)
        {
            if (connection is OdbcConnection)
            {
                string connectionString = connection.ConnectionString;
                lock (typeof(DBUtils))
                {
                    if (OdbcTable.Contains(connectionString))
                    {
                        return (OdbcDBType)OdbcTable[connectionString];
                    }
                    else
                    {
                        OdbcDBType type = OdbcDBType.None;
                        Match mc = Regex.Match(connectionString, @"driver\b.+?;", RegexOptions.IgnoreCase);
                        if (mc.Success)
                        {
                            if (mc.Value.IndexOf("Informix", StringComparison.OrdinalIgnoreCase) != -1)//大小写
                            {
                                type = OdbcDBType.InfoMix;
                            }
                            else if (mc.Value.IndexOf("FoxPro", StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                type = OdbcDBType.FoxPro;
                            }
                        }
                        else
                        {
                            string[] listAlias = DbConnectionSet.GetAvaliableAlias();
                            foreach (string alias in listAlias)
                            {
                                DbConnectionSet.DbConnection db = DbConnectionSet.GetDbConn(alias);
                                if (db != null && db.ConnectionString.StartsWith(connectionString, StringComparison.OrdinalIgnoreCase))
                                {
                                    type = db.OdbcType;
                                    break;
                                }
                            }
                        }
                        OdbcTable.Add(connectionString, type);
                        return type;
                    }
                }
            }
            else if (connection is InfoConnection)
            {
                return (connection as InfoConnection).OdbcType;
            }
            else
            {
                return OdbcDBType.None;
            }
        }
    }
}