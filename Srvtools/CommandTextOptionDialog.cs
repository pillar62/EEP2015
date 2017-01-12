using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Reflection;
#if MySql
using MySql.Data.MySqlClient;
#endif
#if Informix
using IBM.Data.Informix;
#endif
#if Sybase
using Sybase.Data.AseClient;
#endif

namespace Srvtools
{
    public partial class CommandTextOptionDialog : Form
    {
        private String _sQL;
        private String _commandText;
        private IDbConnection _conn;
        private int _index = 1;
        private int _packCount = 1000;
        private int _count = 0;
        private bool _eof = false;
        private DataSet _cacheDs = new DataSet();
        private bool _textChanged;

        public CommandTextOptionDialog(IDbConnection conn, String commandText)
        {
            InitializeComponent();
            _conn = conn;
            _commandText = commandText;
            _sQL = commandText;
            txtCommandText.Text = _sQL;
        }

        public String CommandText
        {
            set { _commandText = value; }
            get { return _commandText; }
        }

        #region Private method.

        // isAdd8 = isAdd*
        private String BuildSQL(String commandText, String tableName, List<String> needAddColumnNamesList, Boolean isAdd8)
        {
            String sQL = "";
            if (isAdd8 == true)
            {
                if (commandText == null || commandText.Length == 0)
                { return "select " + tableName + ".* from " + tableName; }
                else
                { return commandText; }
            }

            // get existed tables's schema.
            DataTable existedTablesSchema = GetExistedTablesSchema(commandText);

            // get need join table's schema.
            DataTable needJoinTableSchema = GetNeedJoinTableSchema(tableName);

            // get existed table's list.
            List<String> existedTableList = GetExistedTablesList(existedTablesSchema, tableName);

            if (existedTableList != null && existedTableList.Count != 0)
            {
                // from and joinPart.
                String fromAndJoinPart = null;

                if (!IsExisted(existedTableList, tableName))
                {
                    // get existed and need add columns.
                    List<String> allColumnList = GetAllColumnsList(existedTableList, needJoinTableSchema);
                    CommandTextJoinDialog joinDialog = new CommandTextJoinDialog(tableName, allColumnList, _conn);
                    joinDialog.ShowDialog();
                    String joinCondition = joinDialog.JoinCondition;
                    joinDialog.Dispose();

                    // cancel
                    if (joinCondition == null || joinCondition.Length == 0)
                    { return commandText; }

                    fromAndJoinPart = GetExistedFromAndJoinPart() + joinCondition;
                }
                else
                {
                    fromAndJoinPart = GetExistedFromAndJoinPart();
                }

                // selectPart.
                List<String> selectedColumnList = GetSelectedColumnsList(existedTablesSchema);
                String selectPart = GetExistedSelectPart(selectedColumnList);

                // remove the same columns.
                List<String> returnColumnList = RemoveRepColumns(selectedColumnList, needAddColumnNamesList);
                foreach (String c in returnColumnList)
                {
                    if (selectPart != null && selectPart.Length != 0)
                    { selectPart += ","; }
                    selectPart += c;
                }

                // wherePart.
                String wherePart = GetExistedWherePart();
                sQL = selectPart + fromAndJoinPart + wherePart;
            }
            else
            {
                StringBuilder selectPartSB = new StringBuilder();
                foreach (String c in needAddColumnNamesList)
                {
                    if (selectPartSB.Length != 0)
                    { selectPartSB.Append(","); }

                    selectPartSB.Append(c);
                }

                sQL = "select " + selectPartSB.ToString() + " from " + tableName;
            }

            return sQL;
        }

        private List<String> GetExistedTablesList(DataTable schema)
        {
            return GetExistedTablesList(schema, "");
        }

        private List<String> GetExistedTablesList(DataTable schema, String TableName)
        {
            List<String> joinedTableList = new List<string>();
            ClientType type = DBUtils.GetDatabaseType(_conn);
            if (schema != null)
            {
                foreach (DataRow r in schema.Rows)
                {
                    Boolean isExist = false;
                    string q = "";

                    if (r["BaseSchemaName"] != null && r["BaseSchemaName"].ToString().Length != 0)
                    {
                        string schemaName = r["BaseSchemaName"].ToString();
                        string tableName = r["BaseTableName"].ToString();
                        if (type == ClientType.ctOleDB && String.IsNullOrEmpty(tableName))
                            tableName = TableName;

                        if (schemaName.Trim().IndexOf(" ") > 0)
                            q += DBUtils.QuoteWords(schemaName, type);
                        else
                            q += schemaName;

                        q += ".";

                        if (tableName.Trim().IndexOf(" ") > 0)
                            q += DBUtils.QuoteWords(tableName, type);
                        else
                            q += tableName;
                    }
                    else
                    {
                        string tableName = r["BaseTableName"].ToString();
                        if (type == ClientType.ctOleDB)
                            tableName = TableName;

                        if (tableName.Trim().IndexOf(" ") > 0)
                            q += DBUtils.QuoteWords(tableName, type);
                        else
                            q += tableName;
                    }

                    foreach (String t in joinedTableList)
                    {
                        if (string.Compare(t, q, true) == 0)//IgnoreCase
                        {
                            isExist = true; break;
                        }
                    }
                    if (isExist == true)
                    { continue; }

                    joinedTableList.Add(q);
                }
            }
            return joinedTableList;
        }

        private List<String> GetSelectedColumnsList(DataTable schema)
        {
            List<String> selectedColumnList = new List<string>();
            ClientType type = DBUtils.GetDatabaseType(_conn);
            foreach (DataRow r in schema.Rows)
            {
                string q = "";
                if (r["BaseSchemaName"] != null && r["BaseSchemaName"].ToString().Length != 0)
                {
                    string schemaName = r["BaseSchemaName"].ToString();
                    string tableName = r["BaseTableName"].ToString();
                    string columnName = r["ColumnName"].ToString();

                    if (schemaName.Trim().IndexOf(" ") > 0)
                        q += DBUtils.QuoteWords(schemaName, type);
                    else
                        q += schemaName;

                    q += ".";

                    if (tableName.Trim().IndexOf(" ") > 0)
                        q += DBUtils.QuoteWords(tableName, type);
                    else
                        q += tableName;

                    q += ".";

                    if (columnName.Trim().IndexOf(" ") > 0)
                        q += DBUtils.QuoteWords(columnName, type);
                    else
                        q += columnName;
                }
                else
                {
                    string tableName = r["BaseTableName"].ToString();
                    string columnName = r["ColumnName"].ToString();

                    if (tableName.Trim().IndexOf(" ") > 0)
                        q += DBUtils.QuoteWords(tableName, type);
                    else
                        q += tableName;

                    q += ".";

                    if (columnName.Trim().IndexOf(" ") > 0)
                        q += DBUtils.QuoteWords(columnName, type);
                    else
                        q += columnName;
                }

                selectedColumnList.Add(q);
            }
            return selectedColumnList;
        }

        private DataTable GetExistedTablesSchema(String commandText)
        {
            if (_sQL == null || _sQL.Length == 0)
            { return null; }

            IDbCommand cmd = _conn.CreateCommand();
            cmd.CommandText = commandText;
            if (_conn.State == ConnectionState.Closed)
            { _conn.Open(); }

            IDataReader reader = null;
            DataTable table = null;
            try
            {
                //cmd.ExecuteNonQuery();
                reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);
                table = reader.GetSchemaTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _conn.Close();
            }
            return table;
        }

        private DataTable GetNeedJoinTableSchema(String tableName)
        {
            String sQL = "select * from " + tableName;

            IDbCommand cmd = _conn.CreateCommand();
            cmd.CommandText = sQL;
            if (_conn.State == ConnectionState.Closed)
            { _conn.Open(); }

            IDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);
            DataTable table = reader.GetSchemaTable();
            _conn.Close();
            return table;
        }

        private void GetNeedSelectColumn(List<String> needAddColumnsList, List<String> existedsColumnList)
        {
            foreach (String nC in needAddColumnsList)
            {
                Boolean isExist = false;
                foreach (String c in existedsColumnList)
                {
                    if (string.Compare(c, nC, true) == 0)//IgnoreCase
                    { isExist = true; break; }
                }
                if (isExist == true)
                { continue; }
                else
                { existedsColumnList.Add(nC); }
            }
        }

        private String GetExistedSelectPart(List<String> columnsList)
        {
            StringBuilder selectBuilder = new StringBuilder();
            if (columnsList != null && columnsList.Count != 0)
            {
                foreach (String c in columnsList)
                {
                    if (selectBuilder.Length != 0)
                    { selectBuilder.Append(","); }

                    selectBuilder.Append(c);
                }
            }
            return "select " + selectBuilder.ToString();
        }

        private String GetExistedFromAndJoinPart()
        {
            if (_sQL == null || _sQL.Length == 0)
            { return ""; }

            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(_sQL, @"[\s\]\)]\bfrom\b[\s\[\(]"
                , System.Text.RegularExpressions.RegexOptions.IgnoreCase);//IgnoreCase
            Int32 index1 = mc[mc.Count - 1].Index + 1;
            //Int32 index1 = lowerSQL.LastIndexOf("from");
            Int32 index2 = _sQL.IndexOf("where", StringComparison.OrdinalIgnoreCase);//IgnoreCase

            if (index1 == 0 || index1 == -1)
            {
                // error
            }

            if (index2 != -1)
            { return " " + _sQL.Substring(index1, (index2 - index1)); }
            else
            { return " " + _sQL.Substring(index1); }
        }

        private String GetExistedWherePart()
        {
            if (_sQL == null || _sQL.Length == 0)
            { return ""; }

            Int32 index = _sQL.IndexOf("where", StringComparison.OrdinalIgnoreCase);//IgnoreCase

            if (index == -1)
            { return ""; }
            else
            { return " " + _sQL.Substring(index); }
        }

        private Boolean IsExisted(List<String> existedTableList, String tableName)
        {
            foreach (String t in existedTableList)
            {
                if (string.Compare(t, tableName.Replace("[", "").Replace("]", ""), true) == 0)//IgnoreCase
                { return true; }
            }
            return false;
        }

        private List<String> RemoveRepColumns(List<String> selectedColumnList, List<String> needSelectColumnNameList)
        {
            List<String> returnColumnList = new List<string>();

            foreach (String c in needSelectColumnNameList)
            {
                Boolean isNeedAdd = true;
                foreach (String i in selectedColumnList)
                {
                    if (string.Compare(c, i, true) == 0)//IgnoreCase
                    { isNeedAdd = false; break; }
                }

                foreach (String j in returnColumnList)
                {
                    if (string.Compare(c, j, true) == 0)//IgnoreCase
                    { isNeedAdd = false; break; }
                }

                if (isNeedAdd == true)
                { returnColumnList.Add(c); }
            }

            return returnColumnList;
        }

        private List<String> GetAllColumnsList(List<String> existedTableList, DataTable needJoinTShema)
        {
            List<String> allColumnsList = new List<string>();
            ClientType type = DBUtils.GetDatabaseType(_conn);
            foreach (String t in existedTableList)
            {
                String sQL = "select * from " + t;

                IDbCommand cmd = _conn.CreateCommand();
                cmd.CommandText = sQL;
                if (_conn.State == ConnectionState.Closed)
                { _conn.Open(); }

                IDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);
                DataTable table = reader.GetSchemaTable();
                _conn.Close();

                foreach (DataRow r in table.Rows)
                {
                    string q = "";
                    if (r["BaseSchemaName"] != null && r["BaseSchemaName"].ToString().Length != 0)
                    {
                        string schemaName = r["BaseSchemaName"].ToString();
                        string tableName = r["BaseTableName"].ToString();
                        string columnName = r["ColumnName"].ToString();

                        if (schemaName.Trim().IndexOf(" ") > 0)
                            q += DBUtils.QuoteWords(schemaName, type);
                        else
                            q += schemaName;

                        q += ".";

                        if (tableName.Trim().IndexOf(" ") > 0)
                            q += DBUtils.QuoteWords(tableName, type);
                        else
                            q += tableName;

                        q += ".";

                        if (columnName.Trim().IndexOf(" ") > 0)
                            q += DBUtils.QuoteWords(columnName, type);
                        else
                            q += columnName;
                    }
                    else
                    {
                        string tableName = r["BaseTableName"].ToString();
                        string columnName = r["ColumnName"].ToString();

                        if (tableName.Trim().IndexOf(" ") > 0)
                            q += DBUtils.QuoteWords(tableName, type);
                        else
                            q += tableName;

                        q += ".";

                        if (columnName.Trim().IndexOf(" ") > 0)
                            q += DBUtils.QuoteWords(columnName, type);
                        else
                            q += columnName;
                    }

                    allColumnsList.Add(q);
                }
            }

            foreach (DataRow r in needJoinTShema.Rows)
            {
                string q = "";
                if (r["BaseSchemaName"] != null && r["BaseSchemaName"].ToString().Length != 0)
                {
                    string schemaName = r["BaseSchemaName"].ToString();
                    string tableName = r["BaseTableName"].ToString();
                    string columnName = r["ColumnName"].ToString();

                    if (schemaName.Trim().IndexOf(" ") > 0)
                        q += DBUtils.QuoteWords(schemaName, type);
                    else
                        q += schemaName;

                    q += ".";

                    if (tableName.Trim().IndexOf(" ") > 0)
                        q += DBUtils.QuoteWords(tableName, type);
                    else
                        q += tableName;

                    q += ".";

                    if (columnName.Trim().IndexOf(" ") > 0)
                        q += DBUtils.QuoteWords(columnName, type);
                    else
                        q += columnName;
                }
                else
                {
                    string tableName = r["BaseTableName"].ToString();
                    string columnName = r["ColumnName"].ToString();

                    if (tableName.Trim().IndexOf(" ") > 0)
                        q += DBUtils.QuoteWords(tableName, type);
                    else
                        q += tableName;

                    q += ".";

                    if (columnName.Trim().IndexOf(" ") > 0)
                        q += DBUtils.QuoteWords(columnName, type);
                    else
                        q += columnName;
                }

                allColumnsList.Add(q);
            }

            return allColumnsList;
        }

        private List<String> GetAllTablesList()
        {
            List<String> tablesList = new List<string>();
            String sQL = "";
            if (_conn is SqlConnection)
            {
                sQL = "select @@version as version";
                IDbCommand cmd0 = _conn.CreateCommand();
                cmd0.CommandText = sQL;
                if (_conn.State == ConnectionState.Closed)
                    _conn.Open();

                Object o = cmd0.ExecuteScalar();
                _conn.Close();
                if (o.ToString().IndexOf("microsoft sql server 2005", StringComparison.OrdinalIgnoreCase) >= 0)//IgnoreCase
                {
                    sQL = @"select (
                            case when b.name != 'dbo' then 
	                            case when (Charindex(' ',Rtrim(Ltrim(b.name)),0) > 0) then
		                            '[' + b.[name] + ']'
	                            else
		                            b.[name]
	                            end
	                            + '.' +
	                            case when (Charindex(' ',Rtrim(Ltrim(a.name)),0) != 0) then
		                            '[' + a.[name] + ']'
	                            else
		                            a.[name]
	                            end
                            else 
	                            case when (Charindex(' ',Rtrim(Ltrim(a.name)),0) != 0) then
		                            '[' + a.[name] + ']'
	                            else
		                            a.[name]
	                            end
                            end
                        )as name from sysobjects a,sys.schemas b where a.uid=b.schema_id and a.xtype in ('u','U','v','V') order by a.[name]";
                }
                else
                {
                    sQL = @"select(
                            case when (Charindex(' ',Rtrim(Ltrim(name)),0) != 0) then
		                        '[' + [name] + ']'
	                        else
		                        [name]
                            end
                            ) as name from sysobjects where xtype in ('u','U','v','V')  order by [name]";
                }
            }
            else if (_conn is OdbcConnection)
            {
                //sQL = "select * from systables where (tabtype = 'T' or tabtype = 'V') and tabid >= 100 order by tabname";
                DbConnection dbc = _conn as DbConnection;
                String[] Params = null;
                String UserID = GetFieldParam(_conn.ConnectionString.ToLower(), "user id");
                Params = new String[] { UserID.ToUpper() };
                if (dbc.State == ConnectionState.Closed) dbc.Open();
                DataTable T = dbc.GetSchema("Tables", Params);
                T.Select("", "TABLE_NAME DESC");
                foreach (DataRow DR in T.Rows)
                {
                    tablesList.Add(DR["TABLE_NAME"].ToString());
                }
                T = dbc.GetSchema("Views", Params);
                foreach (DataRow DR in T.Rows)
                {
                    tablesList.Add(DR["TABLE_NAME"].ToString());
                }
                tablesList.Sort();
                return tablesList;
            }
            else if (_conn is OracleConnection)
            {
                DbConnection dbc = _conn as DbConnection;
                String[] Params = null;
                String UserID = GetFieldParam(_conn.ConnectionString.ToLower(), "user id");
                Params = new String[] { UserID.ToUpper() };
                if (dbc.State == ConnectionState.Closed) dbc.Open();
                DataTable T = dbc.GetSchema("Tables", Params);
                T.Select("", "TABLE_NAME DESC");
                foreach (DataRow DR in T.Rows)
                {
                    String S = "";
                    if (!String.IsNullOrEmpty(DR["OWNER"].ToString()))
                        S = DR["OWNER"].ToString() + '.';
                    tablesList.Add(S + DR["TABLE_NAME"].ToString());
                }
                T = dbc.GetSchema("Views", Params);
                T.Select("", "VIEW_NAME DESC");
                foreach (DataRow DR in T.Rows)
                {
                    String S = "";
                    if (!String.IsNullOrEmpty(DR["OWNER"].ToString()))
                        S = DR["OWNER"].ToString() + '.';
                    tablesList.Add(S + DR["VIEW_NAME"].ToString());
                }
                T = dbc.GetSchema("Synonyms", Params);
                T.Select("", "SYNONYM_NAME DESC");
                foreach (DataRow DR in T.Rows)
                {
                    String S = "";
                    if (!String.IsNullOrEmpty(DR["OWNER"].ToString()))
                        S = DR["OWNER"].ToString() + '.';
                    tablesList.Add(S + DR["SYNONYM_NAME"].ToString());
                }
                return tablesList;
            }
            else if (_conn is OleDbConnection)
            {
                sQL = "sp_help";
            }
            else if (_conn.GetType().Name == "MySqlConnection")
                sQL = "show tables;";
            else if (_conn.GetType().Name == "IfxConnection")
                sQL = "select * from systables where (tabtype = 'T' or tabtype = 'V' or tabtype = 'S') and tabid >= 100 order by tabname";

            IDbCommand cmd = _conn.CreateCommand();
            if (_conn is OleDbConnection)
                cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sQL;
            if (_conn.State == ConnectionState.Closed)
            { _conn.Open(); }

            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                if (_conn is SqlConnection)
                    tablesList.Add(reader["name"].ToString());
                else if (_conn is OdbcConnection)
                    tablesList.Add(reader["tabname"].ToString());
                else if (_conn is OracleConnection)
                    tablesList.Add(reader["OBJECT_NAME"].ToString());
                else if (_conn is OleDbConnection)
                    tablesList.Add(reader["NAME"].ToString());
                else if (_conn.GetType().Name == "MySqlConnection")
                    tablesList.Add(reader[0].ToString());
                else if (_conn.GetType().Name == "IfxConnection")
                    tablesList.Add(reader["tabname"].ToString());
            }

            try
            {
                _conn.Close();
            }
            catch { }
            return tablesList;
        }

        private List<String> GetTableCaptionFromCOLDEF()
        {
            int I;
            DataRow DR;
            IDbCommand aInfoCommand = _conn.CreateCommand();
            aInfoCommand.CommandText = "Select TABLE_NAME, CAPTION from COLDEF where FIELD_NAME = '*' or FIELD_NAME is null order by TABLE_NAME";
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet D = new DataSet();
            DA.Fill(D);
            List<String> aTableNameCaptionList = new List<String>();
            for (I = 0; I < D.Tables[0].Rows.Count; I++)
            {
                DR = D.Tables[0].Rows[I];
                if (DR["TABLE_NAME"].ToString().Trim() != "" && DR["CAPTION"].ToString().Trim() != "")
                    aTableNameCaptionList.Add(DR["TABLE_NAME"].ToString().Trim() + "=" + DR["CAPTION"].ToString().Trim());
            }
            return aTableNameCaptionList;
        }

        public static String GetFieldParam(String Source, String PropName)
        {
            String Result = "";
            String TempParam, S1, S2;
            Char AChar;
            if (Source == "" || Source == null)
                return "";
            AChar = Source[0];
            if (AChar == ';')
            {
                TempParam = Source.Substring(1, Source.Length - 1);
            }
            else
            {
                TempParam = Source;
            }

            while (TempParam != "")
            {
                S1 = GetToken(ref TempParam, new Char[] { ';' });
                if (S1 == "")
                    break;
                S2 = GetToken(ref S1, new Char[] { '=' });
                S2 = S2.Trim();
                if (S2.CompareTo(PropName) != 0)
                    continue;
                Result = S1;
                break;
            }
            return Result;
        }

        public static String GetToken(ref String AString, char[] Fmt)
        {
            String Result = "";
            while (AString.Length != 0 && AString[0] == ' ')
            {
                AString = AString.Remove(1, 1);
            }

            if (AString.Length == 0)
                return Result;

            Boolean Found = false;
            int I = 0;
            while (I < AString.Length)
            {
                Found = false;
                if ((byte)AString[I] <= 128)
                {
                    foreach (char C in Fmt)
                    {
                        if (AString[I] == C)
                        {
                            Found = true;
                            break;
                        }
                    }
                    if (!Found)
                        I++;
                }
                else
                {
                    I = I + 2;
                }
                if (Found)
                    break;
            }

            if (Found)
            {
                Result = AString.Substring(0, I);
                AString = AString.Remove(0, I + 1);
            }
            else
            {
                Result = AString;
                AString = "";
            }

            return Result;
        }

        private void SetDataSet(String sQL, Boolean isShowData)
        {
            if (_sQL == null || _sQL.Length == 0)
            { return; }

            DataSet ds = new DataSet();
            IDbCommand cmd = _conn.CreateCommand();
            cmd.CommandText = sQL;

            IDataAdapter adapter = DBUtils.CreateDbDataAdapter(cmd);

            if (_conn.State == ConnectionState.Closed)
            { _conn.Open(); }

            if (isShowData)
            {
                if (!_eof)
                {
                    if (adapter is SqlDataAdapter)
                    {
                        int i = ((SqlDataAdapter)adapter).Fill(ds, (_index - 1) * _packCount, _packCount + 1, "T1");

                        if (i == _packCount + 1)
                        {
                            _eof = false;
                            ds.Tables["T1"].Rows[_packCount].Delete();
                            _count += _packCount;
                        }
                        else
                        {
                            _eof = true;
                            _count += i;
                        }

                        _cacheDs.Merge(ds);
                    }
                    else if (adapter is OracleDataAdapter)
                    {
                        int i = ((OracleDataAdapter)adapter).Fill(ds, (_index - 1) * _packCount, _packCount + 1, "T1");

                        if (i == _packCount + 1)
                        {
                            _eof = false;
                            ds.Tables["T1"].Rows[_packCount].Delete();
                            _count += _packCount;
                        }
                        else
                        {
                            _eof = true;
                            _count += i;
                        }

                        _cacheDs.Merge(ds);
                    }
                    else if (adapter is OdbcDataAdapter)
                    {
                        int i = ((OdbcDataAdapter)adapter).Fill(ds, (_index - 1) * _packCount, _packCount + 1, "T1");

                        if (i == _packCount + 1)
                        {
                            _eof = false;
                            ds.Tables["T1"].Rows[_packCount].Delete();
                            _count += _packCount;
                        }
                        else
                        {
                            _eof = true;
                            _count += i;
                        }

                        _cacheDs.Merge(ds);
                    }
                    else if (adapter is OleDbDataAdapter)
                    {
                        DataTable[] dts = new DataTable[1];
                        dts[0] = new DataTable("T1");
                        int i = ((OleDbDataAdapter)adapter).Fill((_index - 1) * _packCount, _packCount + 1, dts);
                        ds.Tables.Add(dts[0]);
                        //int i = ((OleDbDataAdapter)adapter).Fill(ds, (_index - 1) * _packCount, _packCount + 1, "T1");

                        if (i == _packCount + 1)
                        {
                            _eof = false;
                            ds.Tables["T1"].Rows[_packCount].Delete();
                            _count += _packCount;
                        }
                        else
                        {
                            _eof = true;
                            _count += i;
                        }

                        _cacheDs.Merge(ds);
                    }
#if MySql
                    else if (adapter.GetType().Name == "MySqlDataAdapter")
                    {
                        int i = ((MySqlDataAdapter)adapter).Fill(ds, (_index - 1) * _packCount, _packCount + 1, "T1");

                        if (i == _packCount + 1)
                        {
                            _eof = false;
                            ds.Tables["T1"].Rows[_packCount].Delete();
                            _count += _packCount;
                        }
                        else
                        {
                            _eof = true;
                            _count += i;
                        }

                        _cacheDs.Merge(ds);
                    }
#endif
#if Informix
                    else if (adapter.GetType().Name == "IfxDataAdapter")
                    {
                        int i = ((IfxDataAdapter)adapter).Fill(ds, (_index - 1) * _packCount, _packCount + 1, "T1");

                        if (i == _packCount + 1)
                        {
                            _eof = false;
                            ds.Tables["T1"].Rows[_packCount].Delete();
                            _count += _packCount;
                        }
                        else
                        {
                            _eof = true;
                            _count += i;
                        }

                        _cacheDs.Merge(ds);
                    }
#endif
#if Sybase
                    else if (adapter.GetType().Name == "AseDataAdapter")
                    {
                        int i = ((AseDataAdapter)adapter).Fill(ds, (_index - 1) * _packCount, _packCount + 1, "T1");

                        if (i == _packCount + 1)
                        {
                            _eof = false;
                            ds.Tables["T1"].Rows[_packCount].Delete();
                            _count += _packCount;
                        }
                        else
                        {
                            _eof = true;
                            _count += i;
                        }

                        _cacheDs.Merge(ds);
                    }
#endif
                }
            }
            else
            {
                _cacheDs = new DataSet();
                adapter.FillSchema(_cacheDs, SchemaType.Mapped);
            }

            _textChanged = false;
            _conn.Close();
        }


        #endregion

        private void btnOK_Click(object sender, EventArgs e)
        {
            _commandText = _sQL;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CommandTextOptionDialog_Load(object sender, EventArgs e)
        {
            try
            {
                if (_sQL != null && _sQL.Length != 0)
                {
                    DataTable existedTableSchema = GetExistedTablesSchema(_sQL);
                    List<String> existedTableNameList = GetExistedTablesList(existedTableSchema);
                    if (existedTableNameList[0] == null || existedTableNameList[0] == "")
                    {
                        char[] mark = { ' ', ',', '\r', '\n' };
                        string[] table = _sQL.Split(mark);
                        List<String> temp = GetAllTablesList();
                        foreach (string name in temp)
                            for (int i = 0; i < table.Length; i++)
                                if (table[i] == name)
                                { listTables.Items.Add(name); break; }
                    }
                    else
                        foreach (String t in existedTableNameList)
                        { listTables.Items.Add(t); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listTables.SelectedItem == null)
                { return; }

                String sQL = "select * from " + listTables.SelectedItem.ToString();
                IDbCommand cmd = _conn.CreateCommand();
                cmd.CommandText = sQL;
                if (_conn.State == ConnectionState.Closed)
                { _conn.Open(); }

                IDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                DataTable schemaTable = reader.GetSchemaTable();

                _conn.Close();

                List<String> columnList = new List<string>();
                foreach (DataRow r in schemaTable.Rows)
                {
                    columnList.Add(r["ColumnName"].ToString());
                }

                listColumns.DataSource = columnList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddColumn_Click(object sender, EventArgs e)
        {
            try
            {
                String tableName = "";
                Object table = listTables.SelectedItem;
                if (table == null)
                { throw new Exception("Please choose a table."); }
                else
                { tableName = table.ToString(); }

                List<String> needAddColumnNamesList = new List<string>();
                foreach (Object item in listColumns.SelectedItems)
                {
                    needAddColumnNamesList.Add(tableName + "." + item.ToString());
                }
                if (needAddColumnNamesList.Count == 0)
                { throw new Exception("Please choose columns."); }

                String sQL = BuildSQL(_sQL, tableName, needAddColumnNamesList, false);
                if (sQL != null || sQL.Length != 0)
                {
                    _sQL = sQL;
                    txtCommandText.Text = _sQL;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void txtCommandText_TextChanged(object sender, EventArgs e)
        {
            _textChanged = true;
            _sQL = txtCommandText.Text;
            if (_sQL == null || _sQL.Length == 0)
            {
                btnAddAllColumn.Enabled = true;
            }
            else
            {
                btnAddAllColumn.Enabled = false;
            }
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            try
            {
                List<String> allTablesList = GetAllTablesList();
                List<String> allTablesCaptionList = GetTableCaptionFromCOLDEF();
                CommandTextAddTableDialog andTableDialog = new CommandTextAddTableDialog(allTablesList, allTablesCaptionList);
                andTableDialog.ShowDialog();
                List<String> selectedTables = andTableDialog.SelectedTables;
                andTableDialog.Dispose();

                foreach (String i in selectedTables)
                {
                    Boolean isAdd = true;
                    foreach (Object o in listTables.Items)
                    {
                        if (o.ToString() == i)
                        { isAdd = false; break; }
                    }

                    if (isAdd == false)
                    { continue; }
                    else
                    { listTables.Items.Add(i); }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void checkShowData_CheckedChanged(object sender, EventArgs e)
        {
            _index = 1;
            _eof = false;
            _cacheDs = new DataSet();
            _count = 0;
            try
            {
                SetDataSet(_sQL, checkShowData.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Reload();
        }

        private void Reload()
        {
            if (tabCommandText.SelectedIndex == 1)
            {
                dataGridView.DataSource = ((_cacheDs == null || _cacheDs.Tables.Count == 0) ? null : _cacheDs.Tables[0]);

                if (_eof || !checkShowData.Checked)
                    lklNext.Enabled = false;
                else
                    lklNext.Enabled = true;

                lblCount.Text = "Count:" + _count;
            }
        }

        private void lklNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _index++;
            try
            {
                SetDataSet(_sQL, checkShowData.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Reload();
        }

        private void btnAddAllColumn_Click(object sender, EventArgs e)
        {
            try
            {
                String tableName = "";
                Object table = listTables.SelectedItem;
                if (table == null)
                { throw new Exception("Please choose a table."); }
                else
                { tableName = table.ToString(); }

                String sQL = BuildSQL(_sQL, tableName, null, true);
                if (sQL != null || sQL.Length != 0)
                {
                    _sQL = sQL;
                    txtCommandText.Text = _sQL;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnCheckCommandText_Click(object sender, EventArgs e)
        {
            string sql = txtCommandText.Text;
            try
            {
                SetDataSet(sql, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Check CommandText successfully.");
        }

        private void tabCommandText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCommandText.SelectedIndex == 1)
            {
                checkShowData.Enabled = true;
                if (_textChanged)
                {
                    try
                    {
                        SetDataSet(_sQL, checkShowData.Checked);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Reload();
                }
            }
            else
            {
                checkShowData.Enabled = false;
            }
        }
    }
}