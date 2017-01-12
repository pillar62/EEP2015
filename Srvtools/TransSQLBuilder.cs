using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Reflection;

using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Data.OleDb;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools
{
    public class TransSQLBuilder
    {
        ClientType type;
        #region Constructor

        public TransSQLBuilder(Transaction transaction, DataRow srcRow, DataTable srcSchema, String writeBackSQLPart)
        {
            _transaction = transaction;
            _srcRow = srcRow;
            _srcSchema = srcSchema;
            _writeSQLBackWherePart = writeBackSQLPart;
            type = DBUtils.GetDatabaseType((transaction.Owner as InfoTransaction).UpdateComp.conn);
        }

        [Obsolete("The recommended alternative is TransSQLBuilder(Transaction, DataRow, DataTable, string)", false)]
        public TransSQLBuilder(Object[] clientInfos, Transaction transaction, DataRow srcRow, DataTable srcSchema, String writeBackSQLPart)
            : this(transaction, srcRow, srcSchema, writeBackSQLPart) { }
        #endregion

        #region Properties

        public Transaction transaction
        {
            get
            {
                return _transaction;
            }
        }

        public DataRow SrcRow
        {
            get
            {
                return _srcRow;
            }
        }

        public DataTable SrcSchema
        {
            get
            {
                return _srcSchema;
            }
        }


        public String QuotePrefix
        {
            get { return _quotePrefix; }
            set { _quotePrefix = value; }
        }

        public String QuoteSuffix
        {
            get
            {
                return _quoteSuffix;
            }
            set
            {
                _quoteSuffix = value;
            }
        }

        public Char Marker
        {
            get
            {
                return _marker;
            }
            set
            {
                _marker = value;
            }
        }

        private String SrcTableName
        {
            get
            {
                if (_srcSchemaName != null && _srcSchemaName.Length > 0)
                    return _srcSchemaName + "." + DBUtils.QuoteWords(_srcTableName, type);
                return DBUtils.QuoteWords(_srcTableName, type);
            }
        }

        #endregion

        #region Public methods

        //public List<String> GetTransSQL(IDbConnection connection, IDbTransaction dbTrans)
        //{
        //    List<String> transSQLs = new List<string>();

        //    if (_srcSchema != null)
        //    {
        //        GetSrcTableName(); GetSrcSchemaName();
        //    }

        //    if (_transaction == null)
        //        return transSQLs;
        //    if (_transaction.TransTableName == "")
        //        return transSQLs;

        //    String transTableName = _transaction.TransTableName;
        //    DataTable transSchema = GenerateTransTableSchema(transTableName, connection, dbTrans);

        //    List<TransKeyField> whereColumnsList = GetWhereColumnsList(_transaction, transSchema);
        //    List<TransField> fsList = GetUpdateColumnsList(_transaction, transSchema);
        //    List<TransKeyField> kFsList = GetInsertColumnsList(_transaction, transSchema);

        //    // 只有在Modified的情况下才有可能TransKeyFields被改变。
        //    // A
        //    if (_srcRow.RowState == DataRowState.Modified)
        //    {
        //        List<TransKeyField> changedTransKF = GetChangedTransKeyFields(_transaction, transSchema);
        //        if (changedTransKF != null && changedTransKF.Count != 0)
        //        {
        //            #region
        //            // Modified，TransKeyFields有被改变。
        //            // 1、
        //            String transSQL1 = "";

        //            if (_transaction.TransMode == TransMode.AlwaysAppend)
        //            {
        //                transSQL1 = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, true, false);
        //                if (transSQL1 != null && transSQL1.Length > 0)
        //                {
        //                    transSQLs.Add(transSQL1);
        //                }
        //            }
        //            else
        //            {
        //                transSQL1 = CreateUpdateTransSQL(fsList, whereColumnsList, _transaction, true, false);
        //                if (transSQL1 != null && transSQL1.Length > 0)
        //                {
        //                    transSQLs.Add(transSQL1);
        //                }
        //            }

        //            // 2、
        //            String transSQL2 = "";
        //            if (_transaction.TransMode != TransMode.AlwaysAppend)
        //            {
        //                String wherePart = CreateWherePart(whereColumnsList);

        //                DataSet queryDataSet = QueryDataSet(transSchema, transTableName, connection, dbTrans);
        //                Boolean hadRow = false;
        //                if (queryDataSet != null)
        //                {
        //                    if (queryDataSet.Tables[0].Rows.Count != 0 && queryDataSet.Tables[0].Rows[0][0].ToString() != "0")
        //                    { hadRow = true; }
        //                }

        //                if (hadRow == false)
        //                {
        //                    if (_transaction.TransMode == TransMode.Exception)
        //                    {
        //                        String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_NotExistRowsInTable");
        //                        throw new ArgumentException(String.Format(message, _transaction.TransTableName, wherePart));
        //                    }

        //                    if (_transaction.TransMode == TransMode.Ignore)
        //                    { return transSQLs; }

        //                    if (_transaction.TransMode == TransMode.AutoAppend)
        //                    {

        //                        transSQL2 = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, false, false);
        //                        if (transSQL2 != null && transSQL2.Length > 0)
        //                        {
        //                            transSQLs.Add(transSQL2);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    transSQL2 = CreateUpdateTransSQL(fsList, whereColumnsList, _transaction, true, true);
        //                    if (transSQL2 != null && transSQL2.Length > 0)
        //                    {
        //                        transSQLs.Add(transSQL2);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                transSQL2 = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, false, false);
        //                if (transSQL2 != null && transSQL2.Length > 0)
        //                {
        //                    transSQLs.Add(transSQL2);
        //                }
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            #region
        //            String transSQL = "";

        //            // Modified，TransKeyFields没有被改变。
        //            if (_transaction.TransMode != TransMode.AlwaysAppend)
        //            {
        //                String wherePart = CreateWherePart(whereColumnsList);

        //                DataSet queryDataSet = QueryDataSet(transSchema, transTableName, connection, dbTrans);
        //                Boolean hadRow = false;
        //                if (queryDataSet != null)
        //                {
        //                    if (queryDataSet.Tables[0].Rows.Count != 0 && queryDataSet.Tables[0].Rows[0][0].ToString() != "0")
        //                    { hadRow = true; }
        //                }

        //                if (hadRow == false)
        //                {
        //                    if (_transaction.TransMode == TransMode.Exception)
        //                    {
        //                        String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_NotExistRowsInTable");
        //                        throw new ArgumentException(String.Format(message, _transaction.TransTableName, wherePart));
        //                    }

        //                    if (_transaction.TransMode == TransMode.Ignore)
        //                    { return transSQLs; }

        //                    if (_transaction.TransMode == TransMode.AutoAppend)
        //                    {
        //                        transSQL = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans ,false, false);
        //                        if (transSQL != null && transSQL.Length > 0)
        //                        {
        //                            transSQLs.Add(transSQL);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    transSQL = CreateUpdateTransSQL(fsList, whereColumnsList, _transaction);
        //                    if (transSQL != null && transSQL.Length > 0)
        //                    {
        //                        transSQLs.Add(transSQL);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                transSQL = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, false, false);
        //                if (transSQL != null && transSQL.Length > 0)
        //                {
        //                    transSQLs.Add(transSQL);
        //                }
        //            }
        //            #endregion
        //        }
        //    }
        //    else // Inserted、Deleted
        //    {
        //        #region
        //        String transSQL = "";
        //        if (_transaction.TransMode != TransMode.AlwaysAppend)
        //        {
        //            String wherePart = CreateWherePart(whereColumnsList);

        //            DataSet queryDataSet = QueryDataSet(transSchema, transTableName, connection, dbTrans);
        //            Boolean hadRow = false;
        //            if (queryDataSet != null)
        //            {
        //                if (queryDataSet.Tables[0].Rows.Count != 0 && queryDataSet.Tables[0].Rows[0][0].ToString() != "0")
        //                { hadRow = true; }
        //            }

        //            if (hadRow == false)
        //            {
        //                if (_transaction.TransMode == TransMode.Exception)
        //                {
        //                    String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_NotExistRowsInTable");
        //                    throw new ArgumentException(String.Format(message, _transaction.TransTableName, wherePart));
        //                }

        //                if (_transaction.TransMode == TransMode.Ignore)
        //                { return transSQLs; }

        //                if (_transaction.TransMode == TransMode.AutoAppend)
        //                {
        //                    transSQL = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, false, false);
        //                    if (transSQL != null && transSQL.Length > 0)
        //                    {
        //                        transSQLs.Add(transSQL);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                transSQL = CreateUpdateTransSQL(fsList, whereColumnsList, _transaction);
        //                if (transSQL != null && transSQL.Length > 0)
        //                {
        //                    transSQLs.Add(transSQL);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (_srcRow.RowState == DataRowState.Added)
        //            {
        //                transSQL = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, false, false);
        //            }
        //            else
        //            {
        //                transSQL = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, false, true);
        //            }

        //            if (transSQL != null && transSQL.Length > 0)
        //            {
        //                transSQLs.Add(transSQL);
        //            }
        //        }
        //        #endregion
        //    }

        //    return transSQLs;
        //}

        public List<String> GetTransSQL(IDbConnection connection, IDbTransaction dbTrans)
        {
            // 逻辑图
            //
            // Insert\Delete ---- 1
            // Update:
            //        AlawysAppend ---- 2
            //        KeyFields is modified ? :
            //                                 No ---- 1
            //                                 Yes ---- 2
            //          
            List<String> transSQLs = new List<string>();

            if (_srcSchema != null)
            {
                GetSrcTableName(); GetSrcSchemaName();
            }

            if (_transaction == null)
                return transSQLs;
            if (_transaction.TransTableName == "")
                return transSQLs;

            String transTableName = _transaction.TransTableName;
            DataTable transSchema = GenerateTransTableSchema(transTableName, connection, dbTrans);

            List<TransKeyField> whereColumnsList = GetWhereColumnsList(_transaction, transSchema);
            List<TransField> fsList = GetUpdateColumnsList(_transaction, transSchema);
            List<TransKeyField> kFsList = GetInsertColumnsList(_transaction, transSchema);

            // 只有在Modified的情况下才有可能TransKeyFields被改变。
            // A
            if (_srcRow.RowState == DataRowState.Modified)
            {
                if (_transaction.TransMode == TransMode.AlwaysAppend)      // 两笔记录
                {
                    #region AlwaysAppend

                    String transSQL1 = "";
                    transSQL1 = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, true, true, true);
                    if (transSQL1 != null && transSQL1.Length > 0)
                    {
                        transSQLs.Add(transSQL1);
                    }

                    String transSQL2 = "";
                    transSQL2 = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, false, false, false);
                    if (transSQL2 != null && transSQL2.Length > 0)
                    {
                        transSQLs.Add(transSQL2);
                    }

                    #endregion
                }
                else
                {
                    #region != AlwaysAppend

                    List<TransKeyField> changedTransKF = GetChangedTransKeyFields(_transaction, transSchema);
                    if (changedTransKF != null && changedTransKF.Count != 0)
                    {
                        #region TransKeyFields is modified
                        // Modified：TransKeyFields有被改变，两笔记录。
                        // 1、
                        String transSQL1 = "";
                        transSQL1 = CreateUpdateTransSQL(fsList, whereColumnsList, connection, _transaction, true, false);
                        if (transSQL1 != null && transSQL1.Length > 0)
                        {
                            transSQLs.Add(transSQL1);
                        }

                        // 2、
                        String transSQL2 = "";
                        if (_transaction.TransMode != TransMode.AlwaysAppend)
                        {
                            String wherePart = CreateWherePart(whereColumnsList);

                            DataSet queryDataSet = QueryDataSet(transSchema, transTableName, connection, dbTrans);
                            Boolean hadRow = false;
                            if (queryDataSet != null)
                            {
                                if (queryDataSet.Tables[0].Rows.Count != 0 && queryDataSet.Tables[0].Rows[0][0].ToString() != "0")
                                { hadRow = true; }
                            }

                            if (hadRow == false)
                            {
                                if (_transaction.TransMode == TransMode.Exception)
                                {
                                    String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_NotExistRowsInTable");
                                    throw new ArgumentException(String.Format(message, _transaction.TransTableName, wherePart));
                                }

                                if (_transaction.TransMode == TransMode.Ignore)
                                { return transSQLs; }

                                if (_transaction.TransMode == TransMode.AutoAppend)
                                {

                                    transSQL2 = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, false, false, false);
                                    if (transSQL2 != null && transSQL2.Length > 0)
                                    {
                                        transSQLs.Add(transSQL2);
                                    }
                                }
                            }
                            else
                            {
                                transSQL2 = CreateUpdateTransSQL(fsList, whereColumnsList, connection, _transaction, true, true);
                                if (transSQL2 != null && transSQL2.Length > 0)
                                {
                                    transSQLs.Add(transSQL2);
                                }
                            }
                        }
                        else
                        {
                            transSQL2 = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, false, false, false);
                            if (transSQL2 != null && transSQL2.Length > 0)
                            {
                                transSQLs.Add(transSQL2);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region TransKeyFields is not modified

                        // Modified：TransKeyFields没有被改变，一笔记录。
                        String transSQL = "";
                        String wherePart = CreateWherePart(whereColumnsList);

                        DataSet queryDataSet = QueryDataSet(transSchema, transTableName, connection, dbTrans);
                        Boolean hadRow = false;
                        if (queryDataSet != null)
                        {
                            if (queryDataSet.Tables[0].Rows.Count != 0 && queryDataSet.Tables[0].Rows[0][0].ToString() != "0")
                            { hadRow = true; }
                        }

                        if (hadRow == false)
                        {
                            if (_transaction.TransMode == TransMode.Exception)
                            {
                                String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_NotExistRowsInTable");
                                throw new ArgumentException(String.Format(message, _transaction.TransTableName, wherePart));
                            }

                            if (_transaction.TransMode == TransMode.Ignore)
                            { return transSQLs; }

                            if (_transaction.TransMode == TransMode.AutoAppend)
                            {
                                transSQL = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, false, false, false);
                                if (transSQL != null && transSQL.Length > 0)
                                {
                                    transSQLs.Add(transSQL);
                                }
                            }
                        }
                        else
                        {
                            transSQL = CreateUpdateTransSQL(fsList, whereColumnsList, connection, _transaction);
                            if (transSQL != null && transSQL.Length > 0)
                            {
                                transSQLs.Add(transSQL);
                            }
                        }

                        #endregion
                    }

                    #endregion
                }
            }
            else // Inserted、Deleted
            {
                #region Inserted、Deleted
                String transSQL = "";
                if (_transaction.TransMode != TransMode.AlwaysAppend)
                {
                    String wherePart = CreateWherePart(whereColumnsList);

                    DataSet queryDataSet = QueryDataSet(transSchema, transTableName, connection, dbTrans);
                    Boolean hadRow = false;
                    if (queryDataSet != null)
                    {
                        if (queryDataSet.Tables[0].Rows.Count != 0 && queryDataSet.Tables[0].Rows[0][0].ToString() != "0")
                        { hadRow = true; }
                    }

                    if (hadRow == false)
                    {
                        if (_transaction.TransMode == TransMode.Exception)
                        {
                            String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_NotExistRowsInTable");
                            throw new ArgumentException(String.Format(message, _transaction.TransTableName, wherePart));
                        }

                        if (_transaction.TransMode == TransMode.Ignore)
                        { return transSQLs; }

                        if (_transaction.TransMode == TransMode.AutoAppend)
                        {
                            if (_srcRow.RowState == DataRowState.Added)
                            {
                                transSQL = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, false, false, false);
                            }
                            else
                            {
                                transSQL = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, true, true, true);
                            }
                            if (transSQL != null && transSQL.Length > 0)
                            {
                                transSQLs.Add(transSQL);
                            }
                        }
                    }
                    else
                    {
                        transSQL = CreateUpdateTransSQL(fsList, whereColumnsList, connection, _transaction);
                        if (transSQL != null && transSQL.Length > 0)
                        {
                            transSQLs.Add(transSQL);
                        }
                    }
                }
                else
                {
                    if (_srcRow.RowState == DataRowState.Added)
                    {
                        transSQL = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, false, false, false);
                    }
                    else
                    {
                        transSQL = CreateInsertTransSQL(kFsList, fsList, connection, dbTrans, true, true, true);
                    }

                    if (transSQL != null && transSQL.Length > 0)
                    {
                        transSQLs.Add(transSQL);
                    }
                }
                #endregion
            }

            return transSQLs;
        }

        public String GetWriteBackSQL(IDbConnection connection, IDbTransaction dbTrans)
        {
            //List<String> writeBackSQLs = new List<string>();
            bool b = false;
            TransFieldCollection transFieldsList = transaction.TransFields;
            foreach (TransField f in transFieldsList)
            {
                if (f.UpdateMode == UpdateMode.WriteBack)
                {
                    b = true; break;
                }
            }
            if (!b) return "";

            String writeBackSQL = "";
            if (_srcSchema != null)
            {
                GetSrcTableName();
                GetSrcSchemaName();
            }

            if (_transaction == null)
            {
                return writeBackSQL;
            }

            if (_transaction.TransTableName == "")
            {
                return writeBackSQL;
            }

            String transTableName = _transaction.TransTableName;
            DataTable transSchema = GenerateTransTableSchema(transTableName, connection, dbTrans);

            DataSet queryDataSet = QueryDataSet(transSchema, transTableName, connection, dbTrans);

            GenerateAutoNumbers(connection, dbTrans, false);

            List<TransField> writeBackColumnsList = GetWriteBackColumnsList(transaction, transSchema, queryDataSet);
            if (writeBackColumnsList == null || writeBackColumnsList.Count == 0)
            {
                return "";
            }

            String writeBackColumnPart = CreateWriteBackSQLUpdatePart(writeBackColumnsList);
            if (writeBackColumnPart.Length == 0)
                return "";

            writeBackSQL = "update " + SrcTableName + " set " + writeBackColumnPart + " where " + _writeSQLBackWherePart;

            return writeBackSQL;
        }
        #endregion

        #region Private methods

        private String GetFieldDefaultValue(TransFieldBase field)
        {
            String defaultValue = field.SrcGetValue;
            Char[] cs = defaultValue.ToCharArray();
            //if (cs.Length == 0)
            //{ return ""; }


            object[] myret = SrvUtils.GetValue(defaultValue, (DataModule)((InfoTransaction)_transaction.Owner).OwnerComp);
            if (myret != null && (int)myret[0] == 0)
            {
                return (string)myret[1];
            }
            if (cs[0] != '"' && cs[0] != '\'')
            {
                Char[] sep1 = "()".ToCharArray();
                String[] sps1 = defaultValue.Split(sep1);

                if (sps1.Length == 3)
                {
                    return InvokeOwerMethod(sps1[0], null);
                }

                if (sps1.Length == 1)
                {
                    return sps1[0];
                }

                if (sps1.Length != 1 && sps1.Length == 3)
                {
                    String message = SysMsg.GetSystemMessage(((DataModule)((InfoTransaction)_transaction.Owner).OwnerComp).Language, "Srvtools", "InfoTranscation", "msg_TransFieldBaseDefaultValueIsBad");
                    throw new ArgumentException(String.Format(message, ((InfoTransaction)(_transaction.Owner)).Name, field.DesField));
                }
            }

            Char[] sep2 = null;
            if (cs[0] == '"')
            {
                sep2 = "\"".ToCharArray();
            }
            if (cs[0] == '\'')
            {
                sep2 = "'".ToCharArray();
            }

            String[] sps2 = defaultValue.Split(sep2);
            if (sps2.Length == 3)
            {
                return sps2[1];
            }
            else
            {
                String message = SysMsg.GetSystemMessage(((DataModule)((InfoTransaction)_transaction.Owner).OwnerComp).Language, "Srvtools", "InfoTranscation", "msg_TransFieldBaseDefaultValueIsBad");
                throw new ArgumentException(String.Format(message, ((InfoTransaction)(_transaction.Owner)).Name, field.DesField));
            }
        }

        private String InvokeOwerMethod(String methodName, Object[] parameters)
        {
            MethodInfo methodInfo = ((InfoTransaction)(_transaction.Owner)).OwnerComp.GetType().GetMethod(methodName);

            Object obj = null;
            if (methodInfo != null)
            {
                obj = methodInfo.Invoke(((InfoTransaction)(_transaction.Owner)).OwnerComp, parameters);
            }

            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }

        private String TransTableName(String transTableName)
        {
            if (transTableName != "" && transTableName != null && transTableName != String.Empty)
            {
                if (_srcSchemaName != null && _srcSchemaName.Length > 0)
                {
                    if (type == ClientType.ctOracle)
                        return DBUtils.QuoteWords(transTableName, type);
                    else
                        return _srcSchemaName + "." + DBUtils.QuoteWords(transTableName, type);
                }
                return DBUtils.QuoteWords(transTableName, type);
            }
            else
            {
                return "";
            }
        }

        private void GetSrcTableName()
        {
            if (_srcTableName != "" && _srcTableName != null && _srcTableName != String.Empty) return;

            String srcTableName = null;

            srcTableName = _srcSchema.Rows[0]["BaseTableName"].ToString();
            _srcTableName = srcTableName;

        }

        private void GetSrcSchemaName()
        {
            if (_srcSchemaName != "" && _srcSchemaName != null && _srcSchemaName != String.Empty) return;

            String srcSchemaName = null;

            srcSchemaName = _srcSchema.Rows[0]["BaseSchemaName"].ToString();
            _srcSchemaName = srcSchemaName;
        }

        private Object TransformMarkerInColumnValue(String typeName, Object columnValue)
        {
            if (Type.GetType(typeName).Equals(typeof(Char)) || Type.GetType(typeName).Equals(typeof(String)))
            {
                StringBuilder sb = new StringBuilder();
                if (columnValue == null || columnValue.ToString() == "")
                {
                    return string.Empty;
                }
                else if (columnValue.ToString().Length > 0)
                {
                    Char[] cVChars = columnValue.ToString().ToCharArray();

                    foreach (Char cVChar in cVChars)
                    {
                        if (cVChar == _marker)
                        { sb.Append(cVChar.ToString()); }

                        sb.Append(cVChar.ToString());
                    }
                }
                return sb.ToString();
            }
            else
            { return columnValue; }
        }

        // 现在Array类型的只考虑了Byte[]（DB里对应的类型为Binary）。
        private String Mark(String typeName, String type, Object columnValue)
        {
            bool isN = false;
            if (string.Compare(typeName, "NChar", true) == 0 || string.Compare(typeName, "NVarChar", true) == 0
                || string.Compare(typeName, "NText", true) == 0 || string.Compare(typeName, "NClob", true) == 0)//IgnoreCase
            {
                isN = true;
            }

            if (columnValue == null || columnValue == DBNull.Value)
            {
                return "null";
            }

            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            {
                if (isN)
                    return "N" + _marker.ToString() + columnValue.ToString() + _marker.ToString();
                else
                    return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                DateTime t = Convert.ToDateTime(columnValue);
                string s = t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                    + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString();
                return _marker.ToString() + s + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private String MarkOracle(String type, Object columnValue)
        {
            if (columnValue == null || columnValue == DBNull.Value)
            {
                return "null";
            }

            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            {
                return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                DateTime t = Convert.ToDateTime(columnValue);
                return string.Format("to_date('{0:yyyy-MM-dd HH:mm:ss}','yyyy-mm-dd hh24:mi:ss')", t);
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private String MarkOdbc(String typeName, String type, Object columnValue)
        {
            bool isN = false;
            if (string.Compare(typeName, "NChar", true) == 0 || string.Compare(typeName, "NVarChar", true) == 0
                 || string.Compare(typeName, "NText", true) == 0 || string.Compare(typeName, "NClob", true) == 0)//IgnoreCase
            {
                isN = true;
            }

            if (columnValue == null || columnValue == DBNull.Value)
            {
                return "null";
            }

            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            {
                if (isN)
                    return "N" + _marker.ToString() + columnValue.ToString() + _marker.ToString();
                else
                    return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                DateTime t = Convert.ToDateTime(columnValue);
                string s = "";
                s = t.Year.ToString() + checkDate(t.Month.ToString()) + checkDate(t.Day.ToString()) + checkDate(t.Hour.ToString()) + checkDate(t.Minute.ToString()) + checkDate(t.Second.ToString());
                s = "to_date('" + s + "', '%Y%m%d%H%M%S')";
                return s;
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private String MarkInformix(String typeName, String type, Object columnValue)
        {
            bool isN = false;
            if (string.Compare(typeName, "NChar", true) == 0 || string.Compare(typeName, "NVarChar", true) == 0
                 || string.Compare(typeName, "NText", true) == 0 || string.Compare(typeName, "NClob", true) == 0)//IgnoreCase
            {
                isN = true;
            }

            if (columnValue == null || columnValue == DBNull.Value)
            {
                return "null";
            }

            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            {
                if (isN)
                    return "N" + _marker.ToString() + columnValue.ToString() + _marker.ToString();
                else
                    return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                DateTime t = Convert.ToDateTime(columnValue);
                string s = "";
                s = t.Year.ToString() + checkDate(t.Month.ToString()) + checkDate(t.Day.ToString()) + checkDate(t.Hour.ToString()) + checkDate(t.Minute.ToString()) + checkDate(t.Second.ToString());
                s = "to_date('" + s + "', '%Y%m%d%H%M%S')";
                return s;
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        public string checkDate(string date)
        {
            if (date.Length < 2)
                date = "0" + date;
            return date;
        }

        private String Operate(String fieldName, String typeName, String operatorName, Object o1, Object o2)
        {
            Object value = null;
            String operater = "";

            if (o1 == null || o1 == DBNull.Value)
            { o1 = 0; }
            if (o2 == null || o2 == DBNull.Value)
            { o2 = 0; }

            if (Type.GetTypeCode(Type.GetType(typeName)).Equals(TypeCode.Byte))
            { value = Convert.ToByte(Convert.ToByte(o1) - Convert.ToByte(o2)); }

            if (Type.GetTypeCode(Type.GetType(typeName)).Equals(TypeCode.Decimal))
            { value = Convert.ToDecimal(Convert.ToDecimal(o1) - Convert.ToDecimal(o2)); }

            if (Type.GetTypeCode(Type.GetType(typeName)).Equals(TypeCode.Double))
            { value = Convert.ToDouble(Convert.ToDouble(o1) - Convert.ToDouble(o2)); }

            if (Type.GetTypeCode(Type.GetType(typeName)).Equals(TypeCode.Int16))
            { value = Convert.ToInt16(Convert.ToInt16(o1) - Convert.ToInt16(o2)); }

            if (Type.GetTypeCode(Type.GetType(typeName)).Equals(TypeCode.Int32))
            { value = Convert.ToInt32(Convert.ToInt32(o1) - Convert.ToInt32(o2)); }

            if (Type.GetTypeCode(Type.GetType(typeName)).Equals(TypeCode.Int64))
            { value = Convert.ToInt64(Convert.ToInt64(o1) - Convert.ToInt64(o2)); }

            if (Type.GetTypeCode(Type.GetType(typeName)).Equals(TypeCode.SByte))
            { value = Convert.ToSByte(Convert.ToSByte(o1) - Convert.ToSByte(o2)); }

            if (Type.GetTypeCode(Type.GetType(typeName)).Equals(TypeCode.Single))
            { value = Convert.ToSingle(Convert.ToSingle(o1) - Convert.ToSingle(o2)); }

            if (Type.GetTypeCode(Type.GetType(typeName)).Equals(TypeCode.UInt16))
            { value = Convert.ToUInt16(Convert.ToUInt16(o1) - Convert.ToUInt16(o2)); }

            if (Type.GetTypeCode(Type.GetType(typeName)).Equals(TypeCode.UInt32))
            { value = Convert.ToUInt32(Convert.ToUInt32(o1) - Convert.ToUInt32(o2)); }

            if (Type.GetTypeCode(Type.GetType(typeName)).Equals(TypeCode.UInt64))
            { value = Convert.ToUInt64(Convert.ToUInt64(o1) - Convert.ToUInt64(o2)); }

            operater = DBUtils.QuoteWords(fieldName, type) + " " + operatorName + " " + "(" + value.ToString() + ")";

            return operater;
        }

        private DataTable GenerateTransTableSchema(String tableName, IDbConnection connection, IDbTransaction dbTrans)
        {
            //if (_transSchema != null) return _transSchema;
            if (_transaction.GetTransTableSchema() != null)
            {
                return _transaction.GetTransTableSchema();
            }

            String sQL = "select * from " + DBUtils.QuoteWords(tableName, type) + " where 1 <> 1";

            IDbCommand command = connection.CreateCommand();
            command.CommandText = sQL;
            if (dbTrans != null) command.Transaction = dbTrans;

            IDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);

            DataTable dataTable = reader.GetSchemaTable();

            reader.Close();

            if (dataTable != null)
            {
                //_transSchema = dataTable;
                _transaction.SetTransTableSchema(dataTable);
                return dataTable;
            }
            else
            {
                return null;
            }
        }

        private DataTable GenerateTransTableSchema(String tableName, IDbConnection connection)
        {
            return GenerateTransTableSchema(tableName, connection, null);
        }

        //private String CreateInsertTransSQL(List<TransKeyField> insertColumnsList, List<TransField> updateColumnsList,
        //    IDbConnection connection, IDbTransaction dbTrans, Boolean isKeyChanged, Boolean isGetDesValue)
        //{
        //    return CreateInsertTransSQL(insertColumnsList, updateColumnsList, connection, dbTrans, isKeyChanged, isGetDesValue, false);
        //}

        // autonumber 
        // b是否是KeyField改变，旧值的新增。
        // a = isKeyChanged
        // b = isGetDesValue
        // c = isAlawysAppend
        private String CreateInsertTransSQL(List<TransKeyField> insertColumnsList, List<TransField> updateColumnsList,
            IDbConnection connection, IDbTransaction dbTrans, Boolean isKeyChanged, Boolean isGetDesValue, Boolean isAlawysAppend)
        {
            StringBuilder insertColumns = new StringBuilder();
            StringBuilder columnValues = new StringBuilder();

            // autonumber
            GenerateAutoNumbers(connection, dbTrans, true); // Only insert is need autonumbers.
            if (_autoNumberList.Count != 0)
            {
                foreach (AutoNumber a in _autoNumberList)
                {
                    if (insertColumns.Length > 0)
                    {
                        insertColumns.Append(", ");
                        columnValues.Append(", ");
                    }

                    insertColumns.Append(DBUtils.QuoteWords(a.TargetColumn, type));
                    Object number = a.Number;
                    if (number == null || null == DBNull.Value)
                    {
                        columnValues.Append("null");
                    }
                    else
                    {
                        columnValues.Append(Mark("varchar", "System.String", TransformMarkerInColumnValue("System.String", a.Number)));
                    }
                }
            }


            // insert columns
            foreach (TransKeyField kF in insertColumnsList)
            {
                if (kF.ReadOnly)
                {
                    String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransKeyFieldDesIsReadOnly");
                    throw new ArgumentException(String.Format(message, new Object[] { _transaction.Name, kF.DesField, kF.DesField }));
                }

                String fieldName = kF.DesField;
                String fieldType = kF.FieldType;
                String fieldTypeName = kF.FieldTypeName == null ? "" : kF.FieldTypeName;
                Object fieldValue = null;

                if (isKeyChanged)
                {
                    fieldValue = kF.DesValue;
                }
                else
                {
                    if (isGetDesValue)
                    {
                        fieldValue = kF.DesValue;
                    }
                    else
                    {
                        fieldValue = kF.SrcValue;
                    }

                }

                if (_autoNumberList.Count != 0)
                {
                    if (NeedRemoveANcol(fieldName)) continue;
                }

                if (insertColumns.Length > 0)
                {
                    insertColumns.Append(", ");
                    columnValues.Append(", ");
                }

                insertColumns.Append(DBUtils.QuoteWords(fieldName, type));
                if (fieldValue == null || fieldValue == DBNull.Value)
                {
                    columnValues.Append("null");
                }
                else
                {
                    columnValues.Append(Mark(fieldTypeName, fieldType, TransformMarkerInColumnValue(fieldType, fieldValue)));
                }
            }

            // update columns.
            foreach (TransField f in updateColumnsList)
            {
                if (f.ReadOnly)
                {
                    String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldDesIsReadOnly");
                    throw new ArgumentException(String.Format(message, new Object[] { _transaction.Name, f.DesField, f.DesField }));
                }

                String fieldName = f.DesField;
                String fieldType = f.FieldType;
                String fieldTypeName = f.FieldTypeName == null ? "" : f.FieldTypeName;
                Object fieldValue = null;

                if (_autoNumberList.Count != 0)
                { if (NeedRemoveANcol(fieldName)) continue; }

                if (insertColumns.Length > 0)
                {
                    insertColumns.Append(", ");
                    columnValues.Append(", ");
                }

                //if (f.SrcField == null || f.SrcField.Length == 0)
                //{
                //    if (isGetDesValue)
                //        fieldValue = f.DesValue;
                //    else
                //        fieldValue = f.SrcValue;

                //    if (f.UpdateMode == UpdateMode.Inc)
                //    {
                //        if (!isAlawysAppend)
                //        {
                //            fieldValue = "(" + fieldValue + ")";
                //        }
                //        else
                //        {
                //            fieldValue = "-(" + fieldValue + ")";
                //        }
                //    }

                //    if (f.UpdateMode == UpdateMode.Dec)
                //    {
                //        if (!isAlawysAppend)
                //        {
                //            fieldValue = "-(" + fieldValue + ")";
                //        }
                //        else
                //        {
                //            fieldValue = "(" + fieldValue + ")";
                //        }
                //    }
                //}
                //else
                //{
                if (isKeyChanged && !isAlawysAppend)
                {
                    if (f.UpdateMode != UpdateMode.WriteBack)
                    {
                        fieldValue = f.DesValue;
                    }
                }
                else
                {
                    if (f.UpdateMode != UpdateMode.WriteBack)
                    {
                        if (isGetDesValue)
                        {
                            if (f.UpdateMode == UpdateMode.Replace)
                                fieldValue = f.DesValue;

                            if (f.UpdateMode == UpdateMode.Inc)
                            {
                                if (f.DesValue == null || f.DesValue == DBNull.Value)
                                { fieldValue = null; }
                                else
                                {
                                    if (!isAlawysAppend)
                                    {
                                        fieldValue = "(" + f.DesValue + ")";
                                    }
                                    else
                                    {
                                        fieldValue = "-(" + f.DesValue + ")";
                                    }
                                }
                            }

                            if (f.UpdateMode == UpdateMode.Dec)
                            {
                                if (f.DesValue == null || f.DesValue == DBNull.Value)
                                { fieldValue = null; }
                                else
                                {
                                    if (!isAlawysAppend)
                                    {
                                        fieldValue = "-(" + f.DesValue + ")";
                                    }
                                    else
                                    {
                                        fieldValue = "(" + f.DesValue + ")";
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (f.UpdateMode == UpdateMode.Replace)
                                fieldValue = f.SrcValue;

                            if (f.UpdateMode == UpdateMode.Inc)
                            {
                                if (f.SrcValue == null || f.SrcValue == DBNull.Value)
                                { fieldValue = null; }
                                else
                                {
                                    if (!isAlawysAppend)
                                    {
                                        fieldValue = "(" + f.SrcValue + ")";
                                    }
                                    else
                                    {
                                        fieldValue = "-(" + f.SrcValue + ")";
                                    }
                                }
                            }

                            if (f.UpdateMode == UpdateMode.Dec)
                            {
                                if (f.SrcValue == null || f.SrcValue == DBNull.Value)
                                { fieldValue = null; }
                                else
                                {
                                    if (!isAlawysAppend)
                                    {
                                        fieldValue = "-(" + f.SrcValue + ")";
                                    }
                                    else
                                    {
                                        fieldValue = "(" + f.SrcValue + ")";
                                    }
                                }
                            }
                        }
                    }
                }
                //}

                insertColumns.Append(DBUtils.QuoteWords(fieldName, type));
                if (fieldValue == null || fieldValue == DBNull.Value)
                {
                    columnValues.Append("null");
                }
                else
                {
                    if (connection is SqlConnection)
                        columnValues.Append(Mark(fieldTypeName, fieldType, TransformMarkerInColumnValue(fieldType, fieldValue)));
                    else if (connection is OracleConnection)
                        columnValues.Append(MarkOracle(fieldType, TransformMarkerInColumnValue(fieldType, fieldValue)));
                    else if (connection is OdbcConnection)
                        columnValues.Append(MarkOdbc(fieldTypeName, fieldType, TransformMarkerInColumnValue(fieldType, fieldValue)));
                    else if (connection is OleDbConnection)
                        columnValues.Append(Mark(fieldTypeName, fieldType, TransformMarkerInColumnValue(fieldType, fieldValue)));
                    else if (connection.GetType().Name == "MySqlConnection")
                        columnValues.Append(Mark(fieldTypeName, fieldType, TransformMarkerInColumnValue(fieldType, fieldValue)));
                    else if (connection.GetType().Name == "IfxConnection")
                        columnValues.Append(MarkInformix(fieldTypeName, fieldType, TransformMarkerInColumnValue(fieldType, fieldValue)));
                }
            }

            String insertSQL = "insert into " + TransTableName(_transaction.TransTableName) + "(" + insertColumns.ToString()
                + ") values(" + columnValues.ToString() + ")";

            return insertSQL;
        }

        private String CreateUpdateTransSQL(List<TransField> updateColumnsList, List<TransKeyField> whereColumnsList, IDbConnection connection,
            Transaction transaction, Boolean b, Boolean c)
        {
            String wherePart = CreateWherePart(whereColumnsList, b, c);
            String updateColumnPart = CreateUpdatePart(updateColumnsList, connection, b, c);

            if (updateColumnPart.Length == 0)
                return "";

            String updateSQL = "update " + TransTableName(transaction.TransTableName) + " set " + updateColumnPart + " where " + wherePart;

            return updateSQL;
        }

        private String CreateUpdateTransSQL(List<TransField> updateColumnsList, List<TransKeyField> whereColumnsList, IDbConnection connection,
            Transaction transaction)
        {
            return CreateUpdateTransSQL(updateColumnsList, whereColumnsList, connection, transaction, false, false);
        }

        // 创建Update的SQL语句的Update部分。
        // b是否有KeyFields修改。
        // c是否是第二步操作。
        private String CreateUpdatePart(List<TransField> updateColumnsList, IDbConnection connection, Boolean b, Boolean c)
        {
            StringBuilder updateColumns = new StringBuilder();
            foreach (TransField f in updateColumnsList)
            {
                if (f.UpdateMode == UpdateMode.Disable)
                    continue;

                if (f.ReadOnly)
                {
                    String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldDesIsReadOnly");
                    throw new ArgumentException(String.Format(message, new Object[] { _transaction.Name, f.DesField, f.DesField }));
                }

                String fieldName = f.DesField;
                String fieldType = f.FieldType;
                String fieldTypeName = f.FieldTypeName == null ? "" : f.FieldTypeName;
                Object fieldValue = null;

                if (b)
                {
                    if (!c)
                    {
                        if (f.UpdateMode == UpdateMode.Inc)
                        {
                            fieldValue = Operate(fieldName, fieldType, "-", f.DesValue, 0);
                        }
                        else if (f.UpdateMode == UpdateMode.Dec)
                        {
                            fieldValue = Operate(fieldName, fieldType, "+", f.DesValue, 0);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (f.UpdateMode != UpdateMode.WriteBack)
                        {
                            if (f.UpdateMode == UpdateMode.Replace)
                                fieldValue = f.SrcValue;

                            if (f.UpdateMode == UpdateMode.Inc)
                            { fieldValue = Operate(fieldName, fieldType, "+", f.SrcValue, 0); }

                            if (f.UpdateMode == UpdateMode.Dec)
                            { fieldValue = Operate(fieldName, fieldType, "-", f.SrcValue, 0); }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    if (f.UpdateMode != UpdateMode.WriteBack)
                    {
                        if (f.UpdateMode == UpdateMode.Replace)
                        {
                            if (_srcRow.RowState == DataRowState.Deleted)
                            {
                                fieldValue = f.DesValue;
                            }
                            //else
                            //{
                            else if (f.SrcValue != null && f.SrcValue != DBNull.Value)
                            {
                                fieldValue = f.SrcValue;
                            }
                            else
                            {
                                fieldValue = null;
                            }
                        }
                        //}

                        if (f.UpdateMode == UpdateMode.Inc)
                        {
                            fieldValue = Operate(fieldName, fieldType, "+", f.SrcValue, f.DesValue);
                        }

                        if (f.UpdateMode == UpdateMode.Dec)
                        {
                            fieldValue = Operate(fieldName, fieldType, "-", f.SrcValue, f.DesValue);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if (updateColumns.Length > 0)
                {
                    updateColumns.Append(", ");
                }
                object transformvalue = TransformMarkerInColumnValue(fieldType, fieldValue);
                string markvalue = Mark(fieldTypeName, fieldType, transformvalue);
                if (connection is OracleConnection)
                    markvalue = MarkOracle(fieldType, transformvalue);
                else if (connection is OdbcConnection)
                    markvalue = MarkOdbc(fieldTypeName, fieldType, transformvalue);
                else if (connection.GetType().Name == "IfxConnection")
                    markvalue = MarkInformix(fieldTypeName, fieldType, transformvalue);

                updateColumns.Append(DBUtils.QuoteWords(fieldName, type) + " = " + markvalue);
            }

            return updateColumns.ToString();
        }

        private String CreateUpdatePart(List<TransField> updateColumnsList, IDbConnection connection)
        {
            return CreateUpdatePart(updateColumnsList, connection, false, false);
        }

        private String CreateWherePart(List<TransKeyField> whereColumnsList)
        {
            return CreateWherePart(whereColumnsList, false, false);
        }

        // 创建Update的SQL语句的Where部分。
        private String CreateWherePart(List<TransKeyField> whereColumnsList, Boolean b, Boolean c)
        {
            StringBuilder whereColumns = new StringBuilder();

            foreach (TransKeyField KF in whereColumnsList)
            {
                String fieldName = KF.DesField;
                String fieldType = KF.FieldType;
                String fieldTypeName = KF.FieldTypeName == null ? "" : KF.FieldTypeName;
                Object fieldValue = null;

                if (_srcRow.RowState == DataRowState.Added)
                {
                    fieldValue = KF.SrcValue;
                }
                if (_srcRow.RowState == DataRowState.Deleted)
                {
                    fieldValue = KF.DesValue;
                }
                if (_srcRow.RowState == DataRowState.Modified)
                {
                    if (b)
                    {
                        if (!c)
                        {
                            fieldValue = KF.DesValue;
                        }
                        else
                        {
                            fieldValue = KF.SrcValue;
                        }
                    }
                    else
                    {
                        fieldValue = KF.SrcValue;
                    }
                }

                if (whereColumns.Length > 0)
                {
                    whereColumns.Append(" and ");
                }

                if (fieldValue == null || fieldValue == DBNull.Value)
                {
                    whereColumns.Append(DBUtils.QuoteWords(fieldName, type) + " is null");
                }
                else
                {
                    whereColumns.Append(DBUtils.QuoteWords(fieldName, type) + " = " + Mark(fieldTypeName, fieldType, TransformMarkerInColumnValue(fieldType, fieldValue)));
                }
            }

            return whereColumns.ToString();
        }

        // 创建WriteBack SQL的Update部分。
        private String CreateWriteBackSQLUpdatePart(List<TransField> writeBackColumnList)
        {
            StringBuilder writeBackSB = new StringBuilder();
            foreach (TransField f in writeBackColumnList)
            {
                if (f.UpdateMode == UpdateMode.Disable)
                    continue;

                if (writeBackSB.Length > 0)
                {
                    writeBackSB.Append(", ");
                }

                String fieldName = f.SrcField;
                String fieldType = f.FieldType;
                String fieldTypeName = f.FieldTypeName == null ? "" : f.FieldTypeName;
                Object fieldValue = f.DesValue;

                if (fieldValue == null || fieldValue == DBNull.Value)
                {
                    writeBackSB.Append(DBUtils.QuoteWords(fieldName, type) + " = null");
                }
                else
                {
                    writeBackSB.Append(DBUtils.QuoteWords(fieldName, type) + " = " + Mark(fieldTypeName, fieldType, TransformMarkerInColumnValue(fieldType, fieldValue)));
                }
            }

            return writeBackSB.ToString();
        }

        private List<TransField> GetWriteBackColumnList()
        {
            List<TransField> writeBackColumns = new List<TransField>();

            foreach (TransField f in _transaction.TransFields)
            {
                if (f.UpdateMode != UpdateMode.WriteBack)
                    continue;

                writeBackColumns.Add(f);
            }
            return writeBackColumns;
        }

        /// <summary>
        /// Get updatecomponent's autonumbers.
        /// </summary>
        /// <returns></returns>
        private void GenerateAutoNumbers(IDbConnection connection, IDbTransaction dbTrans, Boolean isAddStep)
        {
            if (_autoNumberList != null) return;

            List<AutoNumber> autoNumbersList = new List<AutoNumber>();
            if (_transaction.AutoNumber != null && _transaction.AutoNumber.Active)
            {
                _transaction.AutoNumber.GetNumber(connection, dbTrans, isAddStep);
                if (_transaction.AutoNumber.Number == null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (i == 4)
                        {
                            String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "AutoNumber", "msg_AutoConflict");
                            throw new Exception(message);
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(100);
                            _transaction.AutoNumber.GetNumber(connection, dbTrans, isAddStep);
                            if (_transaction.AutoNumber.Number == null)
                            {
                                break;
                            }
                        }
                    }
                }
                autoNumbersList.Add(_transaction.AutoNumber);
            }
            _autoNumberList = autoNumbersList;
        }

        private DataSet QueryDataSet(DataTable transSchema, String tableName, IDbConnection connection, IDbTransaction dbTrans)
        {
            if (_queryDateSet != null) return _queryDateSet;

            List<TransKeyField> whereColumnsList = GetWhereColumnsList(_transaction, transSchema);
            String wherePart = CreateWherePart(whereColumnsList);

            List<TransField> wBColumnsList = GetWriteBackColumnList();

            StringBuilder wBColumnsSB = new StringBuilder();
            if (wBColumnsList.Count != 0)
            {
                foreach (TransField tF in wBColumnsList)
                {
                    if (wBColumnsSB.Length > 0)
                    { wBColumnsSB.Append(", "); }

                    wBColumnsSB.Append(DBUtils.QuoteWords(tF.DesField, type));
                }
            }

            // 如果有WriteBack的列，就直接查询WriteBack的列，否则就查询count(*)。
            String sQL = "";
            if (wBColumnsSB == null || wBColumnsSB.Length == 0)
            {
                sQL = "select count(*) from " + TransTableName(tableName) + " where " + wherePart;
            }
            else
            {
                sQL = "select " + wBColumnsSB.ToString() + " from " + TransTableName(tableName) + " where " + wherePart;
            }

            IDbCommand command = connection.CreateCommand();
            command.CommandText = sQL;
            if (dbTrans != null) command.Transaction = dbTrans;

            IDataAdapter adapter = DBUtils.CreateDbDataAdapter(command);

            _queryDateSet = new DataSet();
            adapter.Fill(_queryDateSet);

            return _queryDateSet;
        }

        // Get the writeback columns.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="transTableSchema"></param>
        /// <param name="queryDataSet"></param>
        /// <returns></returns>
        private List<TransField> GetWriteBackColumnsList(Transaction transaction, DataTable transTableSchema, DataSet queryDataSet)
        {
            if (transaction == null)
                return null;

            //// --------------------------------------------------------------------------------------
            //// need the wherepart.
            //List<TransKeyField> includedInWhereColumns = GetIncludedInWhereColumns(transaction, transTableSchema);
            //String wherePart = GenerateWherePart(includedInWhereColumns);
            String transTableName = transaction.TransTableName;
            //// --------------------------------------------------------------------------------------

            List<TransField> writeBackColumnsList = new List<TransField>();
            TransFieldCollection transFieldsList = transaction.TransFields;

            foreach (TransField f in transFieldsList)
            {
                if (f.UpdateMode != UpdateMode.WriteBack)
                    continue;

                String desField = f.DesField;
                String srcField = f.SrcField;

                if (!IsInSchema(desField, transTableSchema))
                {
                    String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldDesExistInTable");
                    throw new ArgumentException(String.Format(message, _transaction.Name, f.DesField, desField, transTableSchema));
                }

                if (!IsInSchema(srcField, _srcSchema))
                {
                    String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldSrcExistInTable");
                    throw new ArgumentException(String.Format(message, _transaction.Name, f.DesField, srcField, _srcTableName));
                }

                String desFieldType = GetFieldType(desField, transTableSchema);
                String srcFieldType = GetFieldType(srcField, _srcSchema);

                String srcFieldTypeName = "";

                if (type == ClientType.ctMsSql || type == ClientType.ctOleDB)
                {
                    srcFieldTypeName = GetFieldTypeName(srcField, _srcSchema);
                }

                if (desFieldType != srcFieldType)
                {
                    String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldDesAndSrcNotSameType");
                    throw new ArgumentException(String.Format(message, new Object[] { _transaction.Name, f.DesField, desField, srcField }));
                }
                // throw new Exception("The " + desField + " and " + srcField + " are not same type.");

                Object srcFieldValue = null;
                srcFieldValue = _srcRow[srcField, DataRowVersion.Current];

                Object desFieldValue = null;
                AutoNumber a = null;
                foreach (AutoNumber au in _autoNumberList)
                {
                    if (string.Compare(f.DesField, au.TargetColumn, true) == 0)//IgnoreCase
                    {
                        a = au; break;
                    }
                }

                if (a != null)
                { desFieldValue = a.Number; }
                else
                {
                    if (queryDataSet == null || queryDataSet.Tables.Count == 0 || queryDataSet.Tables[0].Rows.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        desFieldValue = queryDataSet.Tables[0].Rows[0][desField];
                    }
                }

                f.FieldType = srcFieldType;
                f.FieldTypeName = srcFieldTypeName == null ? "" : srcFieldTypeName;
                f.SrcValue = srcFieldValue;
                f.DesValue = desFieldValue;

                writeBackColumnsList.Add(f);
            }

            return writeBackColumnsList;
        }

        /// <summary>
        /// 得到Update的列集合。
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="transTableSchema"></param>
        /// <returns></returns>
        private List<TransField> GetUpdateColumnsList(Transaction transaction, DataTable transTableSchema)
        {
            if (transaction == null)
                return null;

            // --------------------------------------------------------------------------------------
            // need the wherepart.
            List<TransKeyField> whereColumnsList = GetWhereColumnsList(transaction, transTableSchema);
            String wherePart = CreateWherePart(whereColumnsList);
            String transTableName = transaction.TransTableName;
            // --------------------------------------------------------------------------------------

            List<TransField> canUpdateColumnsList = new List<TransField>();
            TransFieldCollection transFieldsList = transaction.TransFields;

            foreach (TransField f in transFieldsList)
            {
                if (f.UpdateMode == UpdateMode.Disable)
                    continue;

                String desField = f.DesField;
                String srcField = f.SrcField;

                if (srcField == null || srcField.Length == 0)
                {
                    // if (_srcRow.RowState == DataRowState.Added && f.SrcGetValue != null && f.SrcGetValue.Length != 0)
                    if (f.SrcGetValue != null && f.SrcGetValue.Length != 0)
                    {
                        if (!IsInSchema(desField, transTableSchema))
                        {
                            String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldDesExistInTable");
                            throw new ArgumentException(String.Format(message, _transaction.Name, f.DesField, desField, transTableSchema));
                        }

                        f.FieldType = GetFieldType(desField, transTableSchema);

                        if (type == ClientType.ctMsSql || type == ClientType.ctOleDB)
                        {
                            f.FieldTypeName = GetFieldTypeName(desField, transTableSchema);
                        }

                        if (_srcRow.RowState == DataRowState.Added)
                        {
                            f.DesValue = null;
                        }
                        else
                        {
                            f.DesValue = GetFieldDefaultValue(f);
                        }

                        if (_srcRow.RowState == DataRowState.Deleted)
                        {
                            f.SrcValue = null;
                        }
                        else
                        {
                            f.SrcValue = GetFieldDefaultValue(f);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (f.UpdateMode == UpdateMode.WriteBack)
                    {
                        continue;
                    }

                    if (!IsInSchema(desField, transTableSchema))
                    {
                        String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldDesExistInTable");
                        throw new ArgumentException(String.Format(message, _transaction.Name, f.DesField, desField, transTableSchema));
                    }

                    if (!IsInSchema(srcField, _srcSchema))
                    {
                        String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldSrcExistInTable");
                        throw new ArgumentException(String.Format(message, _transaction.Name, f.DesField, srcField, _srcTableName));
                    }

                    String desFieldType = GetFieldType(desField, transTableSchema);
                    String srcFieldType = GetFieldType(srcField, _srcSchema);
                    String srcFieldTypeName = "";

                    //if (((object[])(_clientInfos[0]))[2] != null && ((object[])(_clientInfos[0]))[2].ToString() != "")
                    //{
                    //    object[] param = new object[1];
                    //    param[0] = ((object[])(_clientInfos[0]))[2].ToString();
                    //    object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", param);
                    //    if (myRet != null && myRet[0].ToString() == "0")
                    //        type = myRet[1].ToString();

                    if (type == ClientType.ctMsSql)
                    {
                        srcFieldTypeName = GetFieldTypeName(srcField, _srcSchema);
                    }
                    else if (type == ClientType.ctOracle)
                    {
                    }
                    else if (type == ClientType.ctOleDB)
                    {
                        srcFieldTypeName = GetFieldTypeName(srcField, _srcSchema);
                    }
                    else if (type == ClientType.ctODBC)
                    {
                        //srcFieldTypeName = GetFieldTypeName(srcField, _srcSchema);
                    }
                    else if (type == ClientType.ctMySql)
                    {
                        //srcFieldTypeName = GetFieldTypeName(srcField, _srcSchema);
                    }
                    else if (type == ClientType.ctInformix)
                    {
                        //srcFieldTypeName = GetFieldTypeName(srcField, _srcSchema);
                    }
                    //}

                    if (desFieldType != srcFieldType)
                    {
                        String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldDesAndSrcNotSameType");
                        //modified by lily 2007/5/24 string.Format少了一个参数
                        throw new ArgumentException(String.Format(message, new Object[] { _transaction.Name, f.DesField, desField, srcField }));
                    }

                    if (type == ClientType.ctMsSql)
                    {
                        if (IsReadOnly(desField, transTableSchema))
                            f.ReadOnly = true;
                        else
                            f.ReadOnly = false;
                    }
                    else if (type == ClientType.ctOracle)
                    {
                    }
                    else if (type == ClientType.ctOleDB)
                    {
                        if (IsReadOnly(desField, transTableSchema))
                            f.ReadOnly = true;
                        else
                            f.ReadOnly = false;
                    }
                    else if (type == ClientType.ctODBC)
                    {
                        if (IsReadOnly(desField, transTableSchema))
                            f.ReadOnly = true;
                        else
                            f.ReadOnly = false;
                    }
                    else if (type == ClientType.ctMySql)
                    {
                        if (IsReadOnly(desField, transTableSchema))
                            f.ReadOnly = true;
                        else
                            f.ReadOnly = false;
                    }
                    else if (type == ClientType.ctInformix)
                    {
                        if (IsReadOnly(desField, transTableSchema))
                            f.ReadOnly = true;
                        else
                            f.ReadOnly = false;
                    }
                    //}

                    if (_srcRow.RowState == DataRowState.Added)
                    {
                        f.DesValue = null;
                    }
                    else
                    {
                        f.DesValue = _srcRow[srcField, DataRowVersion.Original];
                    }

                    if (_srcRow.RowState == DataRowState.Deleted)
                    {
                        //Replace方式取null，其他方式取原值。
                        if (f.UpdateMode == UpdateMode.Replace)
                        {
                            f.SrcValue = _srcRow[srcField, DataRowVersion.Original];
                        }
                        else
                        {
                            f.SrcValue = null;
                        }
                    }
                    else
                    {
                        f.SrcValue = _srcRow[srcField, DataRowVersion.Current];
                    }

                    f.FieldType = srcFieldType;
                    f.FieldTypeName = srcFieldTypeName == null ? "" : srcFieldTypeName;
                }

                canUpdateColumnsList.Add(f);
            }

            return canUpdateColumnsList;
        }

        // 得到TransKeyFields是否改变。
        private List<TransKeyField> GetChangedTransKeyFields(Transaction transaction, DataTable transTableSchema)
        {
            if (transaction == null)
                return null;

            List<TransKeyField> changedKFColumnList = new List<TransKeyField>();
            TransKeyFieldCollection transKeyFieldsList = transaction.TransKeyFields;

            foreach (TransKeyField kF in transKeyFieldsList)
            {
                String desField = kF.DesField;
                String srcField = string.Empty;
                if (string.IsNullOrEmpty(kF.SrcGetValue))
                {
                    srcField = kF.SrcField;
                    if (!IsInSchema(srcField, _srcSchema))
                    {
                        String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldSrcExistInTable");
                        throw new ArgumentException(String.Format(message, _transaction.Name, kF.DesField, srcField, _srcTableName));
                    }
                    String desFieldType = GetFieldType(desField, transTableSchema);
                    String srcFieldType = GetFieldType(srcField, _srcSchema);

                    if (desFieldType != srcFieldType)
                    {
                        String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldDesAndSrcNotSameType");
                        throw new ArgumentException(String.Format(message, new Object[] { _transaction.Name, kF.DesField, desField }));
                    }
                }

                if (!IsInSchema(desField, transTableSchema))
                {
                    String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldDesExistInTable");
                    throw new ArgumentException(String.Format(message, _transaction.Name, kF.DesField, desField, transTableSchema));
                }

                if (kF.DesValue == null && kF.SrcValue == null)
                {
                    changedKFColumnList.Add(kF);
                }
                else if (kF.DesValue != null && kF.SrcValue != null && kF.DesValue.ToString() != kF.SrcValue.ToString())
                {
                    //string type = "";

                    //if (((object[])(_clientInfos[0]))[2] != null && ((object[])(_clientInfos[0]))[2].ToString() != "")
                    //{
                    //    object[] param = new object[1];
                    //    param[0] = ((object[])(_clientInfos[0]))[2].ToString();
                    //    object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", param);
                    //    if (myRet != null && myRet[0].ToString() == "0")
                    //        type = myRet[1].ToString();

                    if (type != ClientType.ctOracle)
                    {
                        if (IsReadOnly(desField, transTableSchema))
                        {
                            String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldDesIsReadOnly");
                            throw new ArgumentException(String.Format(message, new Object[] { _transaction.Name, kF.DesField, desField }));
                        }
                    }
                    //}

                    changedKFColumnList.Add(kF);
                }
            }
            return changedKFColumnList;
        }

        // parameter has transtableschema, the updatecomp's selectcmd's schema is _schema.
        private List<TransKeyField> GetInsertColumnsList(Transaction transaction, DataTable transTableSchema)
        {
            return GetTransKeyFieldsList(transaction, transTableSchema, WhereMode.WhereOnly);
        }

        private List<TransKeyField> GetWhereColumnsList(Transaction transaction, DataTable transTableSchema)
        {
            return GetTransKeyFieldsList(transaction, transTableSchema, WhereMode.InsertOnly);
        }

        private List<TransKeyField> GetTransKeyFieldsList(Transaction transaction, DataTable transTableSchema,
            WhereMode noWhereMode)
        {
            if (transaction == null)
                return null;

            List<TransKeyField> transKeyFieldsList = new List<TransKeyField>();
            TransKeyFieldCollection transKFList = transaction.TransKeyFields;

            foreach (TransKeyField kF in transKFList)
            {
                String desField = kF.DesField;
                String srcField = kF.SrcField;

                if (kF.WhereMode != noWhereMode)
                {
                    if (srcField == null || srcField.Length == 0)
                    {
                        // if (_srcRow.RowState == DataRowState.Added && kF.SrcGetValue != null && kF.SrcGetValue.Length != 0)
                        if (kF.SrcGetValue != null && kF.SrcGetValue.Length != 0)
                        {
                            if (!IsInSchema(desField, transTableSchema))
                            {
                                String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransFieldDesExistInTable");
                                throw new ArgumentException(String.Format(message, _transaction.Name, kF.DesField, desField, transTableSchema));
                            }

                            kF.FieldType = GetFieldType(desField, transTableSchema);

                            if (type == ClientType.ctMsSql || type == ClientType.ctOleDB)
                            {
                                kF.FieldTypeName = GetFieldTypeName(desField, transTableSchema);
                            }

                            if (_srcRow.RowState == DataRowState.Added)
                            {
                                kF.DesValue = null;
                            }
                            else
                            {
                                if (kF.DesValue == null)
                                {
                                    kF.DesValue = GetFieldDefaultValue(kF);
                                }
                            }

                            if (_srcRow.RowState == DataRowState.Deleted)
                            {
                                kF.SrcValue = null;
                            }
                            else
                            {
                                if (kF.SrcValue == null)
                                {
                                    kF.SrcValue = GetFieldDefaultValue(kF);
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (!IsInSchema(desField, transTableSchema))
                        {
                            String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransKeyFieldDesExistInTable");
                            throw new ArgumentException(String.Format(message, _transaction.Name, kF.DesField, desField, transTableSchema));
                        }

                        if (!IsInSchema(srcField, _srcSchema))
                        {
                            String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransKeyFieldSrcExistInTable");
                            throw new ArgumentException(String.Format(message, _transaction.Name, kF.DesField, srcField, _srcTableName));
                        }

                        //if (!IsInSchema(desField, transTableSchema))
                        //    throw new Exception("The " + desField + " is not " + transaction.TransTableName + "'s column.");

                        //if (!IsInSchema(srcField, _srcSchema))
                        //    throw new Exception("The " + srcField + " is not " + _srcTableName + "'s column.");

                        String desFieldType = GetFieldType(desField, transTableSchema);
                        String srcFieldType = GetFieldType(srcField, _srcSchema);
                        String srcFieldTypeName = "";

                        if (type == ClientType.ctMsSql || type == ClientType.ctOleDB)
                        {
                            srcFieldTypeName = GetFieldTypeName(srcField, _srcSchema);
                        }

                        if (desFieldType != srcFieldType)
                        {
                            // throw new Exception("The " + desField + " and " + srcField + " are not same type.");
                            String message = SysMsg.GetSystemMessage(((transaction.Owner as InfoTransaction).OwnerComp as DataModule).Language, "Srvtools", "InfoTransaction", "msg_TransKeyFieldDesAndSrcNotSameType");
                            throw new ArgumentException(String.Format(message, new Object[] { _transaction.Name, kF.DesField, desField, srcField }));
                        }

                        if (kF.WhereMode == WhereMode.Both || kF.WhereMode == WhereMode.InsertOnly)
                        {

                            if (type != ClientType.ctOracle)
                            {
                                if (IsReadOnly(desField, transTableSchema))
                                    kF.ReadOnly = true;
                                else
                                    kF.ReadOnly = false;
                            }
                        }

                        if (kF.WhereMode == WhereMode.WhereOnly)
                        {
                            if (!IsIncludedInWhereClause(desField, transTableSchema))
                            {
                                throw new Exception("The " + desField + " column has a error.");
                            }
                        }

                        if (_srcRow.RowState == DataRowState.Added)
                        {
                            kF.DesValue = null;
                        }
                        else
                        {
                            kF.DesValue = _srcRow[srcField, DataRowVersion.Original];
                        }

                        if (_srcRow.RowState == DataRowState.Deleted)
                        {
                            kF.SrcValue = null;
                        }
                        else
                        {
                            kF.SrcValue = _srcRow[srcField, DataRowVersion.Current];
                        }

                        kF.FieldType = srcFieldType;
                        kF.FieldTypeName = srcFieldTypeName == null ? "" : srcFieldTypeName;
                    }

                    transKeyFieldsList.Add(kF);
                }
            }

            return transKeyFieldsList;
        }

        /*
        private String GetSrcFieldValue(String value)
        {
            if (value == null || value.Length == 0)
            { return ""; }

            Char[] cs = value.ToCharArray();
            if (cs.Length == 0)
            { return ""; }


            if (cs[0] != '"' && cs[0] != '\'')
            {
                Char[] sep1 = "()".ToCharArray();
                String[] sps1 = value.Split(sep1);

                if (sps1.Length == 3)
                { return InvokeOwerMethod(sps1[0], null); }

                if (sps1.Length == 1)
                { return sps1[0]; }

                if (sps1.Length != 1 && sps1.Length == 3)
                { throw new Exception("The GetFixed property is bad."); }
            }

            Char[] sep2 = null;
            if (cs[0] == '"')
            { sep2 = "\"".ToCharArray(); }
            if (cs[0] == '\'')
            { sep2 = "'".ToCharArray(); }

            String[] sps2 = value.Split(sep2);
            if (sps2.Length == 3)
            { return sps2[1]; }
            else
            { throw new Exception("The GetFixed property is bad."); }
        }

        private String InvokeOwerMethod(String methodName, Object[] parameters)
        {
            MethodInfo methodInfo = _transaction.OwnerComp.GetType().GetMethod(methodName);

            Object obj = null;
            if (methodInfo != null)
            { obj = methodInfo.Invoke(_updateComonent.OwnerComp, parameters); }

            if (obj != null)
            { return obj.ToString(); }
            else
            { return ""; }
        }
        */


        // 

        private Boolean IsInSchema(String field, DataTable schema)
        {
            Boolean b = false;

            if (field == "" || schema == null)
                return b;

            foreach (DataRow schemaRow in schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];

                if (string.Compare(columnName, field, true) == 0)//IgnoreCase
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        // Need remove autonumber column.
        private Boolean NeedRemoveANcol(String field)
        {
            foreach (AutoNumber a in _autoNumberList)
            {
                if (string.Compare(a.TargetColumn, field, true) == 0)//IgnoreCase
                {
                    return true;
                }
            }
            return false;
        }

        private String GetFieldType(String field, DataTable schema)
        {
            String t = "";

            foreach (DataRow schemaRow in schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];

                if (string.Compare(columnName, field, true) == 0)//IgnoreCase
                {
                    t = schemaRow["DataType"].ToString();
                    break;
                }
            }

            return t;
        }

        private String GetFieldTypeName(String field, DataTable schema)
        {
            String t = "";

            foreach (DataRow schemaRow in schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];

                if (string.Compare(columnName, field, true) == 0)//IgnoreCase
                {
                    t = schemaRow["DataTypeName"].ToString();
                    break;
                }
            }

            return t;
        }

        //private String GetSrcFieldValue(String field)
        //{
        //    if(_srcRow.RowState == DataRowState.Deleted)
        //        return _srcRow[field, DataRowVersion.Original].ToString();
        //    else
        //        return _srcRow[field, DataRowVersion.Current].ToString();
        //}

        //private String GetDesFieldValue(String tableName, String field, String wherePart,
        //    IDbConnection connection, IDbTransaction dbTrans)
        //{
        //    String sQL = "select " + Quote(field) + " from " + TransTableName(tableName) + " where " + wherePart;

        //    IDbCommand command = LocateCommand(sQL, connection);
        //    if (dbTrans != null) command.Transaction = dbTrans;

        //    //OpenConn(ref connection);
        //    //IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

        //    //reader.Read();

        //    //if (reader[0] == null)
        //    //{
        //    //    OpenConn(ref connection);
        //    //    return "0";
        //    //}
        //    //else
        //    //{
        //    //    OpenConn(ref connection);
        //    //    return reader[0].ToString();
        //    //}

        //    Object scalar = command.ExecuteScalar();
        //    if (scalar == null)
        //        return "0";
        //    else
        //        return scalar.ToString();
        //}

        private Boolean IsHadRows(String tableName, String wherePart, IDbConnection connection, IDbTransaction dbTrans)
        {
            String sQL = "select count(*) from " + DBUtils.QuoteWords(tableName, type) + " where " + wherePart;

            IDbCommand command = connection.CreateCommand();
            command.CommandText = sQL;
            if (dbTrans != null) command.Transaction = dbTrans;

            Int32 count = (Int32)command.ExecuteScalar();
            if (count == 0)
                return false;
            else
                return true;
        }

        private static bool IsReadOnly(string field, DataTable schema)
        {
            foreach (DataRow schemaRow in schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];

                if (string.Compare(columnName, field, true) == 0)//IgnoreCase
                {
                    if ((bool)schemaRow["IsRowVersion"])
                        return true;
                    if ((bool)schemaRow["IsReadOnly"])    // not had one line 
                        return true;
                }
            }
            return false;
        }

        private static bool IsIncludedInWhereClause(String field, DataTable schema)
        {
            foreach (DataRow schemaRow in schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];

                if (string.Compare(columnName, field, true) == 0)//IgnoreCase
                {
                    bool hasErrors = schemaRow.HasErrors;
                }
            }
            return true;
        }

        #endregion

        #region Vars

        private String _quotePrefix = "[";
        private String _quoteSuffix = "]";
        private Char _marker = '\'';
        // private InfoTransaction _infoTransaction;
        private Transaction _transaction;
        //private DataTable _transSchema;
        private DataSet _queryDateSet;
        private List<AutoNumber> _autoNumberList;

        private DataRow _srcRow;
        private DataTable _srcSchema;
        private String _srcTableName;
        private String _srcSchemaName;
        private String _writeSQLBackWherePart;

        // private List<String> _transSQLList;

        #endregion
    }
}
