using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Security;
using System.Security.Permissions;
using System.Reflection;
using System.Xml;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
#if MySql
using MySql.Data.MySqlClient;
#endif
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Transactions;

namespace Srvtools
{
    #region DATA_MODULE
    [ToolboxItem(false)]
    [Designer(typeof(InfoDMDesigner), typeof(IRootDesigner))]
    public class DataModule : Component, IDataModule
    {
        const string ThisModuleName = "Srvtools";
        const string ThisComponentName = "DataModule";

        #region IDataModule Member

        public object[] GetClientInfo()
        {
            return ClientInfo;
        }

        public object GetIntfObject(Type intfType)
        {
            object oRet = null;
            try
            {
                ReflectionPermission reflectionPerm1 = new ReflectionPermission(PermissionState.None);
                reflectionPerm1.Flags = ReflectionPermissionFlag.AllFlags;

                if (intfType.IsInterface)
                {
                    Type type = this.GetType();
                    FieldInfo[] myFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                    for (int i = 0; i < myFields.Length; i++)
                    {
                        object newobj = myFields[i].GetValue(this);
                        if ((null != newobj) && (null != newobj.GetType().GetInterface(intfType.Name)))
                        {
                            return newobj;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                throw new Exception(string.Format(sMess, ThisModuleName, "GetIntfObject", e.Message));
            }
            return oRet;
        }

        public ArrayList GetIntfObjects(Type intfType)
        {
            ArrayList aRet = new ArrayList();
            try
            {
                ReflectionPermission reflectionPerm1 = new ReflectionPermission(PermissionState.None);
                reflectionPerm1.Flags = ReflectionPermissionFlag.AllFlags;

                if (intfType.IsInterface)
                {
                    Type type = this.GetType();
                    FieldInfo[] myFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                    for (int i = 0; i < myFields.Length; i++)
                    {
                        object newobj = myFields[i].GetValue(this);
                        if ((null != newobj) && (null != newobj.GetType().GetInterface(intfType.Name)))
                        {
                            aRet.Add(newobj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                throw new Exception(string.Format(sMess, ThisModuleName, "GetIntfObjects", e.Message));
            }
            return aRet;
        }

        public ArrayList GetObjectsByClassName(string sClassName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object GetObjectByClassName(string sClassName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetClientInfo(object[] _ClientInfo)
        {
            ClientInfo = _ClientInfo;
        }

        public void SetOwnerComponent()
        {
            try
            {
                ArrayList myList = GetIntfObjects(typeof(IFindContainer));
                for (int i = 0; i < myList.Count; i++)
                {
                    ((IFindContainer)(myList[i])).OwnerComp = this;
                }
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                throw new Exception(string.Format(sMess, ThisModuleName, "SetOwnerComponent", e.Message));
            }
        }

        #endregion

        public object GetClientInfo(ClientInfoType type)
        {
            if (ClientInfo != null && ClientInfo.Length > 0)
            {
                object[] info = (object[])ClientInfo[0];
                if (info != null)
                {
                    return info[(int)type];
                }
            }
            return null;
        }

        public object[] ClientInfo;

        [Browsable(false)]
        public Nullable<int> StartRecord
        {
            get
            {
                if (ClientInfo.Length >= 3 && ClientInfo[2] is int)
                {
                    return (int)(ClientInfo[2]) + 1;
                }
                else
                {
                    return null;
                }
            }
        }

        [Browsable(false)]
        public Nullable<int> MaxRecords
        {
            get
            {
                if (ClientInfo.Length >= 2 && ClientInfo[1] is int)
                {
                    int record = (int)ClientInfo[1];
                    if (record > 0)
                    {
                        return record;
                    }
                }
                return null;
            }
        }

        public SYS_LANGUAGE Language
        {
            get
            {
                object lan = GetClientInfo(ClientInfoType.ClientLang);
                return lan != null ? (SYS_LANGUAGE)lan : SYS_LANGUAGE.ENG;
            }
        }

        [Browsable(false)]
        public string ReplacedCommandText
        {
            get
            {
                if (ClientInfo.Length >= 4 && ClientInfo[3] is string)
                {
                    return (string)ClientInfo[3];
                }
                else
                {
                    return null;
                }
            }
        }

        private ArrayList GetDetailCommandUp(IDbCommand sc, ArrayList salDetailCommand)
        {
            return GetDetailCommandUp(sc, salDetailCommand, false);
        }

        private ArrayList GetDetailCommandUp(IDbCommand sc, ArrayList salDetailCommand, bool isReplaceCmd)
        {
            ArrayList myDC = new ArrayList();
            try
            {
                Type myType = GetType();
                object oVal = null;
                FieldInfo[] Fields = myType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                ArrayList myList = GetIntfObjects(typeof(IInfoDataSource));
                for (int j = 0; j < myList.Count; j++)
                {
                    if (((IInfoDataSource)(myList[j])).GetMaster() == sc)
                    {
                        if (isReplaceCmd && ((IInfoDataSource)(myList[j])).DynamicTableName)
                        {
                            //myDC.Add(((IInfoDataSource)(myList[j])).GetDetail());
                        }
                        else if (!isReplaceCmd && !((IInfoDataSource)(myList[j])).DynamicTableName)
                        {
                            //myDC.Add(((IInfoDataSource)(myList[j])).GetDetail());
                        }
                        else
                        {
                            continue;
                        }

                        for (int i = 0; i < Fields.Length; i++)
                        {
                            oVal = Fields[i].GetValue(this);
                            if (null != oVal)
                            {
                                if (oVal.Equals(((IInfoDataSource)myList[j]).GetDetail()))
                                {
                                    //salDetailCommand.Add(Fields[i].Name);
                                    salDetailCommand.Insert(0, Fields[i].Name);

                                    IDbCommand aCommand = ((IInfoDataSource)(myList[j])).GetDetail();
                                    myDC.Insert(0, aCommand);
                                    ArrayList temp = GetDetailCommandUp(aCommand, salDetailCommand);
                                    myDC.InsertRange(0, temp);
                                    //foreach (IDbCommand B in temp)
                                    //    myDC.Insert(0, B);

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage((SYS_LANGUAGE)(((object[])(ClientInfo[0]))[0]), ThisModuleName, ThisComponentName, "msg_RethrowException");
                throw new Exception(string.Format(sMess, ThisModuleName, "GetDetailCommand", e.Message));
            }
            return myDC;
        }

        private ArrayList GetDetailCommandDown(IDbCommand sc, ArrayList salDetailCommand)
        {
            return GetDetailCommandDown(sc, salDetailCommand, false);
        }

        private ArrayList GetDetailCommandDown(IDbCommand sc, ArrayList salDetailCommand, bool isReplaceCmd)
        {
            ArrayList myDC = new ArrayList();
            try
            {
                Type myType = GetType();
                object oVal = null;
                FieldInfo[] Fields = myType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                ArrayList myList = GetIntfObjects(typeof(IInfoDataSource));
                for (int j = 0; j < myList.Count; j++)
                {
                    if (((IInfoDataSource)(myList[j])).GetMaster() == sc)
                    {
                        if (isReplaceCmd && ((IInfoDataSource)(myList[j])).DynamicTableName)
                        {
                            myDC.Add(((IInfoDataSource)(myList[j])).GetDetail());
                        }
                        else if (!isReplaceCmd && !((IInfoDataSource)(myList[j])).DynamicTableName)
                        {
                            myDC.Add(((IInfoDataSource)(myList[j])).GetDetail());
                        }
                        else
                        {
                            continue;
                        }

                        for (int i = 0; i < Fields.Length; i++)
                        {
                            oVal = Fields[i].GetValue(this);
                            if (null != oVal)
                            {
                                if (oVal.Equals(((IInfoDataSource)myList[j]).GetDetail()))
                                {
                                    salDetailCommand.Add(Fields[i].Name);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage((SYS_LANGUAGE)(((object[])(ClientInfo[0]))[0]), ThisModuleName, ThisComponentName, "msg_RethrowException");
                throw new Exception(string.Format(sMess, ThisModuleName, "GetDetailCommand", e.Message));
            }
            return myDC;
        }

        public List<string> _UpdateDataSet(IDbConnection nwindConn, IDbCommand cmdSql, DataSet custDS, string sSqlCommand, IDbTransaction dbTrans)
        {
            return _UpdateDataSet(nwindConn, cmdSql, custDS, sSqlCommand, dbTrans, false);
        }

        public List<string> _UpdateDataSet(IDbConnection nwindConn, IDbCommand cmdSql, DataSet custDS, string sSqlCommand, IDbTransaction dbTrans, int state)
        {
            return _UpdateDataSet(nwindConn, cmdSql, custDS, sSqlCommand, dbTrans, false, state);
        }

        public List<string> _UpdateDataSet(IDbConnection nwindConn, IDbCommand cmdSql, DataSet custDS, string sSqlCommand, IDbTransaction dbTrans, bool isReplaceCmd)
        {
            return _UpdateDataSet(nwindConn, cmdSql, custDS, sSqlCommand, dbTrans, isReplaceCmd, 0);
        }

        public List<string> _UpdateDataSet(IDbConnection nwindConn, IDbCommand cmdSql, DataSet custDS, string sSqlCommand, IDbTransaction dbTrans, bool isReplaceCmd, int state)
        {
            List<string> sqlSentences = new List<string>();
            try
            {
                UpdateComponent myuc = null;
                cmdSql.Connection = nwindConn;
                if (cmdSql is Component && (cmdSql as Component).Container != null)
                {
                    foreach (Component comp in (cmdSql as Component).Container.Components)
                    {
                        if (comp is UpdateComponent && (comp as UpdateComponent).SelectCmd == cmdSql)
                        {
                            myuc = comp as UpdateComponent;
                        }
                    }
                }

                if (myuc == null)
                {
                    string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_CmdNotExistUpdataComp");
                    throw new Exception(string.Format(sMess, ThisModuleName, ThisComponentName, sSqlCommand));
                }

                myuc.SetConnection(nwindConn);
                myuc.SetTransaction(dbTrans);

                if ((nwindConn is OdbcConnection || nwindConn is OleDbConnection) && myuc is UpdateComponent && (myuc as UpdateComponent).SelectCmd != null)
                {
                    string tableName = GetTableName((myuc as UpdateComponent).SelectCmd, false);
                    sqlSentences.AddRange(myuc.Update(custDS, sSqlCommand, isReplaceCmd, "", state, tableName));
                }
                else
                    sqlSentences.AddRange(myuc.Update(custDS, sSqlCommand, isReplaceCmd, state));
                //Add or Modify have to trace child relation  ??? for foreign key issue
                if (state == 2)
                {
                    //rich added for III-Master-Detail
                    ArrayList mySDC = new ArrayList();
                    ArrayList myDC = GetDetailCommandDown((IDbCommand)cmdSql, mySDC);

                    ArrayList mySDC1 = new ArrayList();
                    ArrayList myDC1 = GetDetailCommandDown((IDbCommand)cmdSql, mySDC1, true);

                    for (int i = 0; i < myDC.Count; i++)
                    {
                        sqlSentences.AddRange(_UpdateDataSet(nwindConn, (IDbCommand)myDC[i], custDS, (string)mySDC[i], dbTrans, state));
                    }

                    for (int i = 0; i < myDC1.Count; i++)
                    {
                        sqlSentences.AddRange(_UpdateDataSet(nwindConn, (IDbCommand)myDC1[i], custDS, (string)mySDC1[i], dbTrans, true, state));
                    }
                }
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                throw new Exception(string.Format(sMess, ThisModuleName, "_UpdateDataSet", e.Message), e.GetBaseException());
            }
            return sqlSentences;
        }

        public string GetSqlCommandText(string sDSName)
        {
            string sRet = "";
            try
            {
                IDbCommand selectCMD = null;

                Type myType = GetType();
                if (myType != null)
                {
                    FieldInfo[] Fields = myType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        if (Fields[i].Name.Equals(sDSName))
                        {
                            selectCMD = (IDbCommand)(Fields[i].GetValue(this));
                            sRet = selectCMD.CommandText;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                throw new Exception(string.Format(sMess, ThisModuleName, "GetSqlCommandText", e.Message));
            }
            return sRet;
        }

        private string GetSystemDBName()
        {
            XmlDocument DBXML = new XmlDocument();
            string sysDB = "";

            if (File.Exists(SystemFile.DBFile))
            {
                DBXML.Load(SystemFile.DBFile);
                XmlNode aNode = DBXML.DocumentElement.FirstChild;
                XmlNode sysNode = null;

                while ((null != aNode))
                {
                    if (string.Compare(aNode.Name, "SYSTEMDB", true) == 0)//IgnoreCase
                    {
                        sysNode = aNode;
                        sysDB = sysNode.InnerText.Trim();
                        break;
                    }
                    aNode = aNode.NextSibling;
                }
            }
            return sysDB;
        }

        public object[] GetSqlCommand(string sDSName, string strWhereOrSql, bool isSql)
        {
            return GetSqlCommand(sDSName, strWhereOrSql, isSql, null);
        }

        public object[] GetSqlCommand(string sDSName, string strWhereOrSql, bool isSql, ArrayList paramWhere)
        {
            return GetSqlCommand(sDSName, strWhereOrSql, isSql, paramWhere, string.Empty);
        }

        public object[] GetSqlCommand(string sDSName, string strWhereOrSql, bool isSql, ArrayList paramWhere, string strOrder)
        {
            bool designMode = GetClientInfo(ClientInfoType.LoginUser).ToString().Length == 0;
            string alias = string.Empty;
            IDbCommand cmd = AllocateCommand(sDSName, ref alias, designMode);
            try
            {
                string replaceCommandText = ReplacedCommandText;
                if (!string.IsNullOrEmpty(replaceCommandText))
                {
                    cmd.CommandText = replaceCommandText;
                }
                if (!isSql)
                {
                    if (cmd is InfoCommand && (cmd as InfoCommand).MultiSetWhere)
                    {
                        (cmd as InfoCommand).ReunionInfoCommand(strWhereOrSql);
                        strWhereOrSql = string.Empty;//清空,下面有DealAllSqlText
                    }
                    if (paramWhere != null)
                    {
                        for (int i = 0; i < paramWhere.Count; i++)
                        {
                            cmd.Parameters.Add(paramWhere[i] as IDbDataParameter);
                        }
                    }
                    if (cmd is InfoCommand)
                    {
                        (cmd as InfoCommand).DealAllSqlText(strWhereOrSql, strOrder);
                    }
                }
                bool useSelectTop = false;
                if (cmd is InfoCommand)
                {
                    if ((cmd as InfoCommand).SelectPaging && StartRecord.HasValue && MaxRecords.HasValue)
                    {
                        //清除packet，下面取所有资料
                        (cmd as InfoCommand).DealPacketText(StartRecord.Value, MaxRecords.Value);
                        useSelectTop = true;
                    }
                    SqlEventArgs e = new SqlEventArgs(cmd.CommandText);
                    (cmd as InfoCommand).OnBeforeExecuteSql(e);
                    cmd.CommandText = e.Sql;
                }
                if (!string.IsNullOrEmpty(cmd.CommandText) && cmd.CommandText.IndexOf("SysDatabases", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    throw new Exception("Invalid object name 'SysDatabases'.");
                }
                DataSet ds = new DataSet();
                ds.CaseSensitive = true;
                IDbDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);
                if (!useSelectTop && StartRecord.HasValue && MaxRecords.HasValue)
                {
                    DataTable[] dts = new DataTable[1];
                    dts[0] = new DataTable(sDSName);
                    (adpater as DbDataAdapter).Fill(StartRecord.Value, MaxRecords.Value, dts);
                    ds.Tables.Add(dts[0]);
                    //(adpater as DbDataAdapter).Fill(ds, StartRecord.Value, MaxRecords.Value, sDSName);
                }
                else
                {
                    DataTable dt = new DataTable(sDSName);
                    (adpater as DbDataAdapter).Fill(dt);
                    ds.Tables.Add(dt);
                    //(adpater as DbDataAdapter).Fill(ds, sDSName);
                }
                if (cmd is InfoCommand && !string.IsNullOrEmpty((cmd as InfoCommand).EncodingAfter))
                {
                    DecodeDataTable(ds.Tables[sDSName], (cmd as InfoCommand));
                }
                //设置主键
                if (cmd is InfoCommand && string.IsNullOrEmpty(replaceCommandText))
                {
                    ArrayList listKey = (cmd as InfoCommand).GetKeys();
                    if (listKey.Count > 0)
                    {
                        DataColumn[] columnKey = new DataColumn[listKey.Count];
                        for (int i = 0; i < listKey.Count; i++)
                        {
                            columnKey[i] = ds.Tables[sDSName].Columns[listKey[i].ToString()];
                        }
                        ds.Tables[sDSName].PrimaryKey = columnKey;
                    }
                }
                List<string> sqls = new List<string>();
                if (paramWhere != null && paramWhere.Count > 0)
                {
                    var parameters = new List<string>();
                    foreach (IDbDataParameter param in paramWhere)
                    {
                        if (param.Value != null)
                        {
                            if (param.Value is string)
                            {
                                parameters.Add(string.Format("@{0} = '{1}'", param.ParameterName, param.Value));
                            }
                            else
                            {
                                parameters.Add(string.Format("@{0} = {1}", param.ParameterName, param.Value));
                            }
                        }
                    }
                    sqls.Add(string.Format("{0}?{1}", cmd.CommandText, string.Join(",", parameters)));
                }
                else
                {
                    sqls.Add(cmd.CommandText);
                }
                if (cmd is Component && (cmd as Component).Container != null)
                {
                    foreach (Component comp in (cmd as Component).Container.Components)
                    {
                        if (comp is InfoDataSource && (comp as InfoDataSource).Master == cmd)
                        {
                            List<string> detailSqls = GetDetailDataSet(comp as InfoDataSource, cmd.Connection, ds, sDSName);
                            sqls.AddRange(detailSqls);
                        }
                    }
                }
                if (cmd is InfoCommand)
                {

                    SqlEventArgs e = new SqlEventArgs(cmd.CommandText) { DataSet = ds };
                    (cmd as InfoCommand).OnAfterExecuteSql(e);
                }
                if (!designMode && cmd.Connection.State == ConnectionState.Closed)
                {
                    string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                    throw new Exception(string.Format(sMess, ThisModuleName, "GetSqlCommand", "資料庫連線已中斷!"));
                }
                return new object[] { ds, string.Join("\r\n", sqls) };
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                throw new Exception(string.Format(sMess, ThisModuleName, "GetSqlCommand", e.Message));
            }
            finally
            {
                if (!designMode)
                {
                    ReleaseConnection(alias, cmd.Connection);
                }
                else
                {
                    cmd.Connection.Close();
                }
            }
        }

        private void DecodeDataTable(DataTable table, InfoCommand cmd)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                var dataRow = table.Rows[i];
                foreach (DataColumn column in table.Columns)
                {
                    if (column.DataType == typeof(string))
                    {
                        if (dataRow[column] != DBNull.Value && ((string)dataRow[column]).Length > 0)
                        {
                            dataRow[column] = cmd.DecodeString((string)dataRow[column]);
                            //dataRow[column] = Encoding.GetEncoding(encodeAfter).GetString(Encoding.GetEncoding(encodeBefore).GetBytes((string)dataRow[column]));
                        }
                    }
                }
            }


            table.AcceptChanges();
        }

        public string CodepageTransfer(string changeCode, string beChangeStr)
        {
            return Encoding.GetEncoding(changeCode).GetString(Encoding.Default.GetBytes(beChangeStr)).ToString().Trim();
        }

        public int Recover(string sDSName, DataTable table)
        {
            bool designMode = GetClientInfo(ClientInfoType.LoginUser).ToString().Length == 0;
            string alias = string.Empty;
            IDbCommand cmd = AllocateCommand(sDSName, ref alias, designMode);
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            IDbTransaction transaction = cmd.Connection.BeginTransaction();
            try
            {

                int count = 0;
                FieldInfo[] fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    if (field.FieldType == typeof(LogInfo))
                    {
                        //LogInfo log = field.GetValue(this) as LogInfo;
                        //if (log.Command == cmd)
                        //{
                        //    count = log.Recover(table, cmd.Connection, transaction);
                        //    break;
                        //}
                    }
                }
                transaction.Commit();
                return count;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                throw new Exception(string.Format(sMess, ThisModuleName, "Recover", e.Message));
            }
            finally
            {
                if (!designMode)
                {
                    ReleaseConnection(alias, cmd.Connection);
                }
                else
                {
                    cmd.Connection.Close();
                }
            }
        }

        private List<string> GetDetailDataSet(InfoDataSource datasSource, IDbConnection conn, DataSet ds, string masterName)
        {
            List<string> sqls = new List<string>();
            if (datasSource.MasterColumns == null || datasSource.DetailColumns == null)
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, datasSource.GetType(), null, "MasterColumns", null);
            }
            else if (datasSource.MasterColumns.Count != datasSource.DetailColumns.Count)
            {
                throw new EEPException(EEPException.ExceptionType.PropertyInvalid, datasSource.GetType(), null, "MasterColumns.Count", datasSource.MasterColumns.Count.ToString());
            }
            else if (datasSource.MasterColumns.Count == 0)
            {
                throw new EEPException(EEPException.ExceptionType.PropertyInvalid, datasSource.GetType(), null, "MasterColumns.Count", "0");
            }
            else if (datasSource.Detail != null)
            {
                //反射加载,为了取到名字
                FieldInfo[] fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (FieldInfo field in fields)
                {
                    object obj = field.GetValue(this);
                    if (obj != null && obj.GetType().GetInterface("IDbCommand") != null)
                    {
                        IDbCommand cmd = (IDbCommand)obj;
                        if (cmd == datasSource.Detail)
                        {
                            string detailName = field.Name;
                            if (detailName == masterName)
                            {
                                throw new EEPException(EEPException.ExceptionType.PropertyInvalid, datasSource.GetType(), null, "Detail", detailName);
                            }
                            cmd.Connection = conn;
                            if (cmd is InfoCommand)
                            {
                                (cmd as InfoCommand).DealAllSqlText(string.Empty, string.Empty);
                                if ((cmd as InfoCommand).DynamicTableName)
                                {
                                    if (!string.IsNullOrEmpty((string)GetClientInfo(ClientInfoType.TableName)))
                                    {
                                        cmd.CommandText = DBUtils.ReplaceTableName(cmd.CommandText, (string)GetClientInfo(ClientInfoType.TableName));
                                    }
                                }
                            }
                            if (ds.Tables[masterName].Rows.Count > 0)
                            {
                                string commandText = cmd.CommandText;//缓存Detail的CommandText
                                StringBuilder whereBuilder = new StringBuilder();
                                ClientType type = DBUtils.GetDatabaseType(conn);
                                OdbcDBType odbcType = DBUtils.GetOdbcDatabaseType(conn);
                                for (int i = 0; i < datasSource.DetailColumns.Count; i++)
                                {
                                    if (whereBuilder.Length > 0)
                                    {
                                        whereBuilder.Append(" AND ");
                                    }
                                    whereBuilder.Append(DBUtils.GetTableNameForColumn(cmd.CommandText
                                        , (datasSource.DetailColumns[i] as ColumnItem).FieldName, type));
                                    whereBuilder.Append(" = ");
                                    whereBuilder.Append(DBUtils.GetWhereFormat(ds.Tables[masterName].Columns[(datasSource.MasterColumns[i] as ColumnItem).FieldName].DataType
                                        , type, odbcType, i));
                                }
                                object[] value = new object[datasSource.MasterColumns.Count];
                                for (int i = 0; i < ds.Tables[masterName].Rows.Count; i++)
                                {
                                    DataRow row = ds.Tables[masterName].Rows[i];
                                    if (datasSource.DynamicTableName)
                                    {
                                        cmd.CommandText = string.Format("SELECT * from {0}"
                                            , DBUtils.QuoteWords(row[(datasSource.MasterColumns[i] as ColumnItem).FieldName].ToString(), type));
                                    }
                                    else
                                    {
                                        for (int j = 0; j < datasSource.MasterColumns.Count; j++)
                                        {
                                            ColumnItem item = datasSource.MasterColumns[j] as ColumnItem;
                                            value[j] = DBUtils.GetWhereValue(ds.Tables[masterName].Columns[item.FieldName].DataType, row[item.FieldName]);
                                        }
                                        cmd.CommandText = DBUtils.InsertWhere(commandText, string.Format(whereBuilder.ToString(), value));
                                    }
                                    if (cmd is InfoCommand)
                                    {
                                        SqlEventArgs e = new SqlEventArgs(cmd.CommandText);
                                        (cmd as InfoCommand).OnBeforeExecuteSql(e);
                                        cmd.CommandText = e.Sql;
                                    }

                                    IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                                    DataTable dt = new DataTable(detailName);
                                    if (ds.Tables.Contains(detailName))
                                        (adpater as DbDataAdapter).Fill(ds.Tables[detailName]);
                                    else
                                    {
                                        (adpater as DbDataAdapter).Fill(dt);
                                        ds.Tables.Add(dt);
                                    }
                                    if (!string.IsNullOrEmpty(cmd.CommandText) && cmd.CommandText.IndexOf("SysDatabases", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        throw new Exception("Invalid object name 'SysDatabases'.");
                                    }
                                    sqls.Add(cmd.CommandText);
                                    //(adpater as DbDataAdapter).Fill(ds, detailName);//Detail没有PacketRecords
                                }
                            }
                            else
                            {
                                cmd.CommandText = DBUtils.InsertWhere(cmd.CommandText, "1=0");
                                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);
                                DataTable dt = new DataTable(detailName);
                                if (ds.Tables.Contains(detailName))
                                    (adpater as DbDataAdapter).Fill(ds.Tables[detailName]);
                                else
                                {
                                    (adpater as DbDataAdapter).Fill(dt);
                                    ds.Tables.Add(dt);
                                }
                                sqls.Add(cmd.CommandText);
                                //(adpater as DbDataAdapter).Fill(ds, detailName);
                            }
                            if (cmd is InfoCommand && !string.IsNullOrEmpty((cmd as InfoCommand).EncodingAfter))
                            {
                                DecodeDataTable(ds.Tables[detailName], (cmd as InfoCommand));
                            }
                            if (cmd is InfoCommand)
                            {
                                //设置主键
                                ArrayList listKey = (cmd as InfoCommand).GetKeys();
                                if (listKey.Count > 0)
                                {
                                    DataColumn[] columnKey = new DataColumn[listKey.Count];
                                    for (int i = 0; i < listKey.Count; i++)
                                    {
                                        columnKey[i] = ds.Tables[detailName].Columns[listKey[i].ToString()];
                                    }
                                    ds.Tables[detailName].PrimaryKey = columnKey;
                                }
                            }
                            //设置Relation
                            DataColumn[] parentColumn = new DataColumn[datasSource.MasterColumns.Count];
                            DataColumn[] childColumn = new DataColumn[datasSource.DetailColumns.Count];
                            for (int i = 0; i < datasSource.MasterColumns.Count; i++)
                            {
                                parentColumn[i] = ds.Tables[masterName].Columns[(datasSource.MasterColumns[i] as ColumnItem).FieldName];
                                childColumn[i] = ds.Tables[detailName].Columns[(datasSource.DetailColumns[i] as ColumnItem).FieldName];
                            }
                            ds.Relations.Add(parentColumn, childColumn);

                            if (cmd is Component && (cmd as Component).Container != null)
                            {
                                foreach (Component comp in (cmd as Component).Container.Components)
                                {
                                    if (comp is InfoDataSource && (comp as InfoDataSource).Master == cmd)
                                    {
                                        GetDetailDataSet(comp as InfoDataSource, conn, ds, detailName);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return sqls;
        }

        public int GetRecordsCount(string sDSName, string strWhere)
        {
            bool designMode = GetClientInfo(ClientInfoType.LoginUser).ToString().Length == 0;
            string alias = string.Empty;
            IDbCommand cmd = AllocateCommand(sDSName, ref alias, designMode);
            try
            {
                if (cmd is InfoCommand && (cmd as InfoCommand).MultiSetWhere)
                {
                    (cmd as InfoCommand).ReunionInfoCommand(strWhere);
                    strWhere = string.Empty;
                }
                if (cmd is InfoCommand)
                {
                    (cmd as InfoCommand).DealAllSqlText(strWhere, string.Empty);
                }
                if (cmd is InfoCommand && (cmd as InfoCommand).CommandType == CommandType.StoredProcedure)
                {
                    DataSet dataSet = (cmd as InfoCommand).ExecuteDataSet();
                    return dataSet.Tables[0].Rows.Count;
                }

                else if (DBUtils.HasPartEffectCount(cmd.CommandText))
                {
                    DataSet dataSet = (cmd as InfoCommand).ExecuteDataSet();
                    return dataSet.Tables[0].Rows.Count;
                }
                else
                {
                    cmd.CommandText = DBUtils.InsertCount(cmd.CommandText);
                    object obj = cmd.ExecuteScalar();
                    return Convert.ToInt32(obj);
                }
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                throw new Exception(string.Format(sMess, ThisModuleName, "GetRecordsCount", e.Message));
            }
            finally
            {
                if (!designMode)
                {
                    ReleaseConnection(alias, cmd.Connection);
                }
                else
                {
                    cmd.Connection.Close();
                }
            }
        }


        public DataSet GetRecordsTotal(string sDSName, string strWhere, Dictionary<string, string> totals)
        {
            bool designMode = GetClientInfo(ClientInfoType.LoginUser).ToString().Length == 0;
            string alias = string.Empty;
            IDbCommand cmd = AllocateCommand(sDSName, ref alias, designMode);
            try
            {
                //detail replace key 
                Type myType = GetType();
                object oVal = null;
                FieldInfo[] Fields = myType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                ArrayList myList = GetIntfObjects(typeof(IInfoDataSource));
                for (int j = 0; j < myList.Count; j++)
                {
                    if (((IInfoDataSource)(myList[j])).GetDetail() == cmd) // cmd is detail
                    {
                        var detailcolumn = ((IInfoDataSource)(myList[j])).GetDetailColumn();
                        var mastercolumn = ((IInfoDataSource)(myList[j])).GetMasterColumn();

                        var detailKeyList = detailcolumn.Split(new char[] { ';' });
                        var masterKeyList = mastercolumn.Split(new char[] { ';' });
                        for (var i = 0; i < masterKeyList.Length; i++)
                        {
                            var masterKey = masterKeyList[i];
                            var detailKey = detailKeyList[i];
                            strWhere = strWhere.Replace("." + masterKey, "." + detailKey).Replace(".[" + masterKey + "]", ".[" + detailKey + "]");//column incloud [] 
                        }
                    }
                }

                if (cmd is InfoCommand && (cmd as InfoCommand).MultiSetWhere)
                {
                    (cmd as InfoCommand).ReunionInfoCommand(strWhere);
                    strWhere = string.Empty;
                }
                if (cmd is InfoCommand)
                {
                    (cmd as InfoCommand).DealAllSqlText(strWhere, string.Empty);
                }
                if (cmd is InfoCommand && (cmd as InfoCommand).CommandType == CommandType.StoredProcedure)
                {
                }
                else
                {
                    cmd.CommandText = DBUtils.InsertTotal(cmd.CommandText, totals, DBUtils.GetDatabaseType(cmd.Connection));
                }
                DataSet ds = new DataSet();
                IDbDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);
                DataTable dt = new DataTable(sDSName);
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                return ds;
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                throw new Exception(string.Format(sMess, ThisModuleName, "GetRecordsTotal", e.Message));
            }
            finally
            {
                if (!designMode)
                {
                    ReleaseConnection(alias, cmd.Connection);
                }
                else
                {
                    cmd.Connection.Close();
                }
            }

        }

        public DataSet ExecuteSql(string sDSName, string sSql, string DBAlias, bool IsCursor)
        {
            return ExecuteSql(sDSName, sSql, DBAlias, IsCursor, null);
        }

        public DataSet ExecuteSql(string sDSName, string sSql, string DBAlias, bool IsCursor, ArrayList paramWhere)
        {
            bool designMode = GetClientInfo(ClientInfoType.LoginUser).ToString().Length == 0;
            string alias = DBAlias;
            bool getSplitSystemDB = ClientInfo.Length >= 5 && ClientInfo[4] is bool && (bool)ClientInfo[4];
            IDbCommand cmd = AllocateCommand(sDSName, ref alias, designMode, getSplitSystemDB);
            try
            {
                if (cmd.CommandType == CommandType.StoredProcedure)
                {
                    cmd.CommandType = CommandType.Text;
                    if (cmd is InfoCommand)
                    {
                        (cmd as InfoCommand).InfoParameters.Clear();
                    }
                }
                cmd.CommandText = sSql;
                if (paramWhere != null)
                {
                    for (int i = 0; i < paramWhere.Count; i++)
                    {
                        cmd.Parameters.Add(paramWhere[i] as IDbDataParameter);
                    }
                }
                if (!string.IsNullOrEmpty(cmd.CommandText) && cmd.CommandText.IndexOf("SysDatabases", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    throw new Exception("Invalid object name 'SysDatabases'.");
                }
                DataSet ds = null;
                if (IsCursor)
                {
                    ds = new DataSet();
                    ds.CaseSensitive = true;
                    IDbDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);
                    if (StartRecord.HasValue && MaxRecords.HasValue)
                    {
                        DataTable[] dts = new DataTable[1];
                        dts[0] = new DataTable(sDSName);
                        (adpater as DbDataAdapter).Fill(StartRecord.Value, MaxRecords.Value, dts);
                        ds.Tables.Add(dts[0]);
                        //(adpater as DbDataAdapter).Fill(ds, StartRecord.Value, MaxRecords.Value, sDSName);
                    }
                    else
                    {
                        DataTable dt = new DataTable(sDSName);
                        (adpater as DbDataAdapter).Fill(dt);
                        ds.Tables.Add(dt);
                        //(adpater as DbDataAdapter).Fill(ds, sDSName);
                    }
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }
                cmd.Parameters.Clear();
                return ds;
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                throw new Exception(string.Format(sMess, ThisModuleName, "ExecuteSql", e.Message));
            }
            finally
            {
                if (!designMode)
                {
                    ReleaseConnection(alias, cmd.Connection);
                }
                else
                {
                    cmd.Connection.Close();
                }
            }
        }

        public DataSet ExecuteSql(string sSql, IDbConnection conn, IDbTransaction trans)
        {
            return ExecuteSql(sSql, conn, trans, null);
        }

        public DataSet ExecuteSql(string sSql, IDbConnection conn, IDbTransaction trans, ArrayList paramWhere)
        {
            try
            {
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = sSql;
                if (paramWhere != null)
                {
                    for (int i = 0; i < paramWhere.Count; i++)
                    {
                        cmd.Parameters.Add(paramWhere[i] as IDbDataParameter);
                    }
                }
                if (!string.IsNullOrEmpty(cmd.CommandText) && cmd.CommandText.IndexOf("SysDatabases", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    throw new Exception("Invalid object name 'SysDatabases'.");
                }
                cmd.Transaction = trans;
                IDbDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                ds.CaseSensitive = true;
                if (conn is OleDbConnection)
                {
                    DataTable dt = new DataTable();
                    (adpater as OleDbDataAdapter).Fill(dt);
                    ds.Tables.Add(dt);
                }
                else
                {
                    adpater.Fill(ds);
                }
                return ds;
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_ExecuteCommandException");
                throw new Exception(string.Format(sMess, ThisModuleName, "ExecuteSql", e.Message));
            }
        }

        public int ExecuteCommand(string sql, IDbConnection conn, IDbTransaction trans)
        {
            return ExecuteCommand(sql, conn, trans, null);
        }

        public int ExecuteCommand(string sql, IDbConnection conn, IDbTransaction trans, ArrayList paramWhere)
        {
            try
            {
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                if (paramWhere != null)
                {
                    for (int i = 0; i < paramWhere.Count; i++)
                    {
                        cmd.Parameters.Add(paramWhere[i] as IDbDataParameter);
                    }
                }
                cmd.Transaction = trans;
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_ExecuteCommandException");
                throw new Exception(string.Format(sMess, ThisModuleName, "ExecuteCommand", e.Message));
            }
        }

        public ArrayList GetKeyFields(string sDSName)
        {
            Type myType = GetType();
            if (myType != null)
            {
                FieldInfo[] Fields = myType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                for (int i = 0; i < Fields.Length; i++)
                {
                    if (Fields[i].Name.Equals(sDSName))
                    {
                        InfoCommand selectCMD = (InfoCommand)(Fields[i].GetValue(this));
                        return selectCMD.GetKeys();
                    }
                }
            }
            return null;
        }

        public object[] UpdateDataSet(string sSqlCommand, DataSet custDS)
        {
            bool designMode = GetClientInfo(ClientInfoType.LoginUser).ToString().Length == 0;
            string alias = string.Empty;
            IDbCommand cmd = AllocateCommand(sSqlCommand, ref alias, designMode);
            UpdateComponent uc = null;
            List<string> sqlSentences = new List<string>();
            try
            {

                if (cmd is Component && (cmd as Component).Container != null)
                {
                    foreach (Component comp in (cmd as Component).Container.Components)
                    {
                        if (comp is UpdateComponent && (comp as UpdateComponent).SelectCmd == cmd)
                        {
                            uc = comp as UpdateComponent;
                            break;
                        }
                    }
                }

                if (uc == null)
                {
                    string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_CmdNotExistUpdataComp");
                    throw new Exception(string.Format(sMess, ThisModuleName, ThisComponentName, sSqlCommand));
                }
                IDbConnection conn = cmd.Connection;

                uc.SetConnection(conn);

                if (uc.UseTranscationScope)
                {
                    TransactionOptions option = new TransactionOptions();
                    option.Timeout = uc.TranscationScopeTimeOut;
                    option.IsolationLevel = (System.Transactions.IsolationLevel)Enum.Parse(typeof(System.Transactions.IsolationLevel), uc.TransIsolationLevel.ToString());
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, option))
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                        UpdateDataSet(sSqlCommand, custDS, designMode, alias, cmd, uc, sqlSentences, conn);
                        scope.Complete();
                    }
                }
                else
                {
                    UpdateDataSet(sSqlCommand, custDS, designMode, alias, cmd, uc, sqlSentences, conn);
                }

            }
            catch (Exception e)
            {
                string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                Exception ex = new Exception(string.Format(sMess, ThisModuleName, "UpdateDataSet", e.Message), e.GetBaseException());
                throw ex;
            }
            string sql = "";
            if (sqlSentences.Count > 0)
            {
                foreach (string sentence in sqlSentences)
                {
                    sql += sentence + "\\r\\n";
                }
            }
            return new object[] { custDS, sql };
        }

        private void UpdateDataSet(string sSqlCommand, DataSet custDS, bool designMode, string alias, IDbCommand cmd, UpdateComponent uc, List<string> sqlSentences, IDbConnection conn)
        {
            IDbTransaction dbTrans = null;
            Boolean b = false;

            // q 表示master是否有更改。
            bool q = false;
            try
            {
                if (!uc.UseTranscationScope)
                {
                    if (cmd.Connection is OdbcConnection)
                    {
                        uc.AutoTrans = false;
                    }

                    if (uc.AutoTrans)
                    {
                        dbTrans = cmd.Connection.BeginTransaction(uc.TransIsolationLevel);
                        uc.SetTransaction(dbTrans);
                    }
                }

                // ------------------------------------------------------------------------------------
                // Delete先做Details，再做Master。
                DataSet setQ = custDS.GetChanges(DataRowState.Deleted);
                if (setQ != null)
                {
                    ArrayList mySDC = new ArrayList();
                    ArrayList mySDC1 = new ArrayList();

                    ArrayList myDC = GetDetailCommandUp(cmd, mySDC);
                    ArrayList myDC1 = GetDetailCommandUp(cmd, mySDC1, true);

                    for (int i = 0; i < myDC.Count; i++)
                    {
                        sqlSentences.AddRange(_UpdateDataSet(conn, (IDbCommand)myDC[i], custDS, (string)mySDC[i], dbTrans, 1));
                    }

                    for (int i = 0; i < myDC1.Count; i++)
                    {
                        sqlSentences.AddRange(_UpdateDataSet(conn, (IDbCommand)myDC1[i], custDS, (string)mySDC1[i], dbTrans, true, 1));
                    }

                    DataTable tableQ = custDS.Tables[sSqlCommand].GetChanges(DataRowState.Deleted);
                    if (tableQ != null)
                    {
                        //q = true;
                        string tablename = this.GetTableName(cmd, false);
                        string replaceCmd = (string)((object[])ClientInfo)[1];
                        if (conn is OdbcConnection || conn is OleDbConnection)
                            sqlSentences.AddRange(uc.Update(custDS, sSqlCommand, false, replaceCmd, 1, tablename));
                        else
                            sqlSentences.AddRange(uc.Update(custDS, sSqlCommand, replaceCmd, 1));
                    }
                }

                // ------------------------------------------------------------------------------------
                setQ = custDS.GetChanges(DataRowState.Added | DataRowState.Modified);
                if (setQ != null)
                {
                    DataTable tableQ = custDS.Tables[sSqlCommand].GetChanges(DataRowState.Added | DataRowState.Modified);
                    if (tableQ != null)
                    {
                        q = custDS.Tables[sSqlCommand].GetChanges(DataRowState.Added) != null;
                        string replaceCmd = (string)((object[])ClientInfo)[1];
                        string tablename = this.GetTableName(cmd, false);
                        if (conn is OdbcConnection || conn is OleDbConnection || conn.GetType().Name == "IfxConnection")
                            sqlSentences.AddRange(uc.Update(custDS, sSqlCommand, false, replaceCmd, 2, tablename));
                        else
                            sqlSentences.AddRange(uc.Update(custDS, sSqlCommand, replaceCmd, 2));
                    }

                    ArrayList mySDC = new ArrayList();
                    ArrayList mySDC1 = new ArrayList();

                    ArrayList myDC = GetDetailCommandDown(cmd, mySDC);
                    ArrayList myDC1 = GetDetailCommandDown(cmd, mySDC1, true);
                    for (int i = 0; i < myDC.Count; i++)
                    {
                        sqlSentences.AddRange(_UpdateDataSet(conn, (IDbCommand)myDC[i], custDS, (string)mySDC[i], dbTrans, 2));
                    }

                    for (int i = 0; i < myDC1.Count; i++)
                    {
                        sqlSentences.AddRange(_UpdateDataSet(conn, (IDbCommand)myDC1[i], custDS, (string)mySDC1[i], dbTrans, true, 2));
                    }
                }
            }
            catch
            {
                if (!uc.UseTranscationScope)
                {
                    if (uc.AutoTrans)
                    {
                        b = true;
                        dbTrans.Rollback();
                    }
                }
                throw;
            }
            finally
            {
                if (!uc.UseTranscationScope)
                {
                    if (uc.AutoTrans && !b)
                    {
                        dbTrans.Commit();
                    }
                }
                uc.OnAfterApplied(new EventArgs());
                // 同步identity字段
                // Added by yangdong.
                if (q && uc.ServerModify)
                {
                    string tablename = this.GetTableName(cmd, false);
                    if (conn is OdbcConnection || conn is OleDbConnection || conn.GetType().Name == "IfxConnection")
                        uc.IdentitySync(custDS, sSqlCommand, tablename);
                    else
                        uc.IdentitySync(custDS, sSqlCommand);
                }
                if (!designMode)
                {
                    ReleaseConnection(alias, cmd.Connection);
                }
                else
                {
                    cmd.Connection.Close();
                }

            }
        }

      

        public string GetTableName(string sql, bool origian)
        {
            return DBUtils.GetTableName(sql, origian);
        }

        public string GetTableName(IDbCommand cmd, bool origian)
        {
            return DBUtils.GetTableName(cmd.CommandText, origian);
        }

        public string GetTableNameForColumn(string sql, string columnName, ClientType type)
        {
            return DBUtils.GetTableNameForColumn(sql, columnName, type);
        }

        private IDbCommand AllocateCommand(string commandName, ref string eepAlias, bool designMode)
        {
            return AllocateCommand(commandName, ref eepAlias, designMode, false);
        }

        private IDbCommand AllocateCommand(string commandName, ref string eepAlias, bool designMode, bool getSplitSystemDB)
        {
            FieldInfo info = this.GetType().GetField(commandName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (info == null)
            {
                throw new EEPException(EEPException.ExceptionType.ControlNotFound, this.GetType(), null, "IDbCommand", commandName);
            }
            else
            {
                IDbCommand cmd = (IDbCommand)info.GetValue(this);
                if (string.IsNullOrEmpty(eepAlias))
                {
                    if (cmd is InfoCommand)
                    {
                        eepAlias = (cmd as InfoCommand).EEPAlias;
                    }
                    if (string.IsNullOrEmpty(eepAlias))
                    {
                        if (designMode)
                        {
                            if (cmd is InfoCommand && (cmd as InfoCommand).InfoConnection != null)
                            {
                                eepAlias = (cmd as InfoCommand).InfoConnection.EEPAlias;
                            }
                        }
                        else
                        {
                            eepAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                        }
                        if (string.IsNullOrEmpty(eepAlias))
                        {
                            eepAlias = GetSystemDBName();
                        }
                    }
                }
                if (!string.IsNullOrEmpty(eepAlias) && getSplitSystemDB)
                {
                    eepAlias = GetSplitSysDB(eepAlias);
                }
                string cacheCommandName = string.Empty;
                if (cmd is InfoCommand)
                {
                    if ((cmd as InfoCommand).CacheConnection)
                    {
                        cacheCommandName = commandName;
                    }
                }
                IDbConnection connection = AllocateConnection(eepAlias, cacheCommandName, designMode);
                cmd.Connection = connection;
                if (cmd is InfoCommand && (cmd as InfoCommand).DynamicTableName)
                {
                    if (!string.IsNullOrEmpty((string)GetClientInfo(ClientInfoType.TableName)))
                    {
                        cmd.CommandText = DBUtils.ReplaceTableName(cmd.CommandText, (string)GetClientInfo(ClientInfoType.TableName));
                    }
                }
                if (cmd is InfoCommand)
                {
                    Srvtools.DbConnectionSet.DbConnection db = Srvtools.DbConnectionSet.GetDbConn(eepAlias, DeveloperID);
                    (cmd as InfoCommand).EncodingConvert = db.Encoding;
                }
                return cmd;
            }
        }


        private string GetSplitSysDB(string sDB)
        {
            String s = SystemFile.DBFile;

            if (File.Exists(s))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(s);
                XmlNode node = xml.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", sDB));
                if (node != null)
                {
                    if (node.Attributes["Master"] != null && node.Attributes["Master"].Value.Trim() == "1")
                    {
                        XmlNode nodesys = xml.SelectSingleNode("InfolightDB/SystemDB");
                        if (nodesys != null)
                        {
                            string sysdb = nodesys.InnerText.Trim();
                            XmlNode nodecheck = xml.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", sysdb));
                            if (nodecheck != null)
                            {
                                return sysdb;
                            }
                            else
                            {
                                throw new Exception("SystemDB does not exsit in db list");
                            }
                        }
                        else
                        {
                            throw new Exception("SystemDB is Empty");
                        }
                    }
                    else
                    {
                        return sDB;
                    }
                }
                else
                {
                    throw new Exception(string.Format("EEPAlias:{0} does not exsit", sDB));
                }
            }
            else
            {
                throw new Exception(string.Format("{0} does not exsit", s));
            }

        }

        public string DeveloperID
        {
            get
            {
                if (ClientInfo != null && ClientInfo.Length > 0)
                {
                    object[] info = (object[])ClientInfo[0];
                    if (info != null && info.Length > 17)
                    {
                        return (string)info[17];
                    }
                }
                return null;
            }
        }

        private IDbConnection AllocateConnection(string DbAlias, string cacheCommandName, bool designMode)
        {
            if (designMode)
            {
                Srvtools.DbConnectionSet.DbConnection db = Srvtools.DbConnectionSet.GetDbConn(DbAlias);
                if (db == null)
                {
                    throw new EEPException(EEPException.ExceptionType.DataBaseNotDefined, this.GetType(), null, DbAlias, null);
                }
                return db.CreateConnection();
            }
            else
            {
                try
                {
                    ClientType ct = ClientType.ctNone;
                    string developerID = DeveloperID;
                    IDbConnection connection = Srvtools.DbConnectionSet.WaitForConnection(DbAlias, developerID, ref ct, GetClientInfo(ClientInfoType.LoginUser).ToString()
                            , this.GetType().FullName, cacheCommandName, DateTime.Now);
                    if (connection == null)
                    {
                        string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_AllocateConnectionTimeOutEx");
                        throw new Exception(sMess);
                    }
                    return connection;
                }
                catch (Exception e)
                {
                    string sMess = SysMsg.GetSystemMessage(Language, ThisModuleName, ThisComponentName, "msg_RethrowException");
                    throw new Exception(string.Format(sMess, ThisModuleName, "AllocateConnection", e.Message));
                }
            }
        }

        public IDbConnection AllocateConnection(string DbAlias)
        {
            return AllocateConnection(DbAlias, null, false);
        }

        public void ReleaseConnection(string DbAlias, IDbConnection conn)
        {
            string developerID = DeveloperID;
            DbConnectionSet.ReleaseConnection(DbAlias, developerID, conn);
        }

        public IDbConnection AllocateCacheConnection(string DbAlias, string cacheCommandName)
        {
            return AllocateConnection(DbAlias, cacheCommandName, false);
        }

        public void ReleaseCacheConnection(string DbAlias, string cacheCommandName)
        {
            string developerID = DeveloperID;
            DbConnectionSet.ReleaseConnection(DbAlias, developerID, GetClientInfo(ClientInfoType.LoginUser).ToString(), this.GetType().FullName, cacheCommandName);
        }

        #region Obsolete Member
        [Obsolete("The method has been abolished", false)]
        public void SetCmdDBTables(IDbCommand cmdDBTables, IDbConnection conn) { }
        #endregion
    }

    #endregion DATA_MODULE
}