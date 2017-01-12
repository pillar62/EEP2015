using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
#if MySql
using MySql.Data.MySqlClient;
#endif
using Microsoft.Win32;

namespace Srvtools
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(UpdateComponent), "Resources.UpdateComp.ico")]
    public class UpdateComponent : InfoBaseComp, IUpdateComponent
    {
        #region Constructor

        public UpdateComponent(System.ComponentModel.IContainer container)
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            //_fieldAttrs = new List<FieldAttr>();
            _fieldAttrs = new FieldAttrCollection(this, typeof(FieldAttr));
            _serverModifyColumns = new ServerModifyColumnCollection(this, typeof(ServerModifyColumn));
            _transIsolationLevel = IsolationLevel.ReadCommitted;
            _autoTrans = true;
            _serverModify = true;
            _needSyncFVs = false;
            _sendSqlCmd = true;
            _syncFieldValues = new Hashtable();
            _schemas = new Hashtable();
        }

        public UpdateComponent()
        {
            ///
            /// This call is required by the Windows.Forms Designer.
            ///
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            //_fieldAttrs = new List<FieldAttr>();
            _fieldAttrs = new FieldAttrCollection(this, typeof(FieldAttr));
            _transIsolationLevel = IsolationLevel.ReadCommitted;
            _serverModifyColumns = new ServerModifyColumnCollection(this, typeof(ServerModifyColumn));
            _autoTrans = true;
            _serverModify = true;
            _sendSqlCmd = true;
            _RowAffectsCheck = true;
            _schemas = new Hashtable();
        }

        #endregion

        #region Properties

        [Category("Infolight"),
        Description("The InfoCommand which the control is bound to")]
        public InfoCommand SelectCmd
        {
            get { return _selectCmd; }
            set { _selectCmd = value; }
        }

        [Category("Infolight"),
        Description("The InfoCommand which the control is bound to")]
        public InfoCommand SelectCmdForUpdate
        {
            get { return _selectCmdForUpdate; }
            set { _selectCmdForUpdate = value; }
        }
        
        //[Category("Data")]
        //public String KeyField
        //public KeyItem KeyField
        //{
        //    get{ return _keyField; }
        //    set{ _keyField = value; }
        //}

        [Category("Infolight"),
        Description("The mode of use where keyword to update or delete data")]
        public WhereModeType WhereMode
        {
            get { return _whereModeType; }
            set { _whereModeType = value; }
        }

        [Category("Infolight"),
        Description("Indicate whether the columns in left join part will be added to FieldAttrs and the data of them will be protected from modifies ")]
        public Boolean ExceptJoin
        {
            get { return _exceptJoin; }
            set { _exceptJoin = value; }
        }

        [Category("Infolight"),
        Description("Indicate whether server excute transactions automatically when client apply")]
        public Boolean AutoTrans
        {
            get { return _autoTrans; }
            set { _autoTrans = value; }
        }


        private bool _useTranscationScope;
        [Category("Infolight"),
        Description("Use scope transaction instead of database transaction")]
        public bool UseTranscationScope
        {
            get { return _useTranscationScope; }
            set { _useTranscationScope = value; }
        }


        private TimeSpan _transcationScopeTimeOut = new TimeSpan(0, 2, 0);
        [Category("Infolight"),
        Description("Specifies TranscationScope time out")]
        public TimeSpan TranscationScopeTimeOut
        {
            get { return _transcationScopeTimeOut; }
            set { _transcationScopeTimeOut = value; }
        }
	

        [Category("Infolight"),
        Description("Specifies isolation level")]
        public IsolationLevel TransIsolationLevel
        {
            get { return _transIsolationLevel; }
            set { _transIsolationLevel = value; }
        }

        [Category("Infolight"),
        Description("The LogInfo bound to the control")]
        public LogInfo LogInfo
        {
            get { return _logInfo; }
            set { _logInfo = value; }
        }

        // public FieldAttrCollection FieldAttrs
        [Category("Infolight"),
        Description("Specifies the settings of columns")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public List<FieldAttr> FieldAttrs
        public FieldAttrCollection FieldAttrs
        {
            get { return _fieldAttrs; }
            set { _fieldAttrs = value; }
        }

        [Category("Infolight"),
        Description("Specifies the columns used to do servermodify")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ServerModifyColumnCollection ServerModifyColumns
        {
            get { return _serverModifyColumns; }
            set { _serverModifyColumns = value; }
        }

        [Browsable(false)]
        public String Name
        {
            set
            {
                if (Site != null)
                {
                    _name = Site.Name;
                }
            }
            get { return _name; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("The recommended alternative is CurrentRow.")]
        public DataRow CurrectRow
        {
            get { return _currentRow; }
        }

        [Browsable(false)]
        public DataRow CurrentRow
        {
            get { return _currentRow; }
        }

        [Category("Infolight"),
        Description("Indicates whether server transfers the lastest date to client automatically after the data in database changes")]
        public Boolean ServerModify
        {
            get { return _serverModify; }
            set { _serverModify = value; }
        }

        [Category("Infolight"),
        Description("Indicates whether server get the the data having the max value")]
        public bool ServerModifyGetMax
        {
            get { return _serverModifyGetMax; }
            set { _serverModifyGetMax = value; }
        }

        private bool _RowAffectsCheck;
        [Category("Infolight"),
        Description("Specifies whether check the count of row effected during update process")]
        public bool RowAffectsCheck
        {
            get { return _RowAffectsCheck; }
            set { _RowAffectsCheck = value; }
        }

        private bool _sendSqlCmd;
        public bool SendSQLCmd
        {

            get
            {
                return _sendSqlCmd;
            }
            set { _sendSqlCmd = value; }
        }
      


        #endregion

        #region Methods

        public object GetFieldCurrentValue(string fieldName)
        {
            if (CurrentRow == null)
                return null;

            return CurrentRow[fieldName, DataRowVersion.Current];
        }

        public object GetFieldOldValue(string fieldName)
        {
            if (CurrentRow == null)
                return null;

            return CurrentRow[fieldName, DataRowVersion.Original];
        }

        public DataRow GetCurrentMasterRow(string sMasterTable)
        {
            if (_updateDataSet != null && CurrentRow != null)
            {
                foreach (DataRelation relation in _updateDataSet.Relations)
                {
                    if (relation.ParentTable.TableName == sMasterTable && relation.ChildTable.TableName == _updateTable)
                    {
                        if (CurrentRow.RowState != DataRowState.Deleted)
                        {
                            object[] keys = new object[CurrentRow.Table.PrimaryKey.Length];
                            for (int i = 0; i < keys.Length; i++)
                            {
                                keys[i] = CurrentRow[CurrentRow.Table.PrimaryKey[i]];
                            }
                            DataRow realrow = _updateDataSet.Tables[_updateTable].Rows.Find(keys);//find realrow
                            return realrow.GetParentRow(relation);
                        }
                        else
                        {
                            object[] keys = new object[CurrentRow.Table.PrimaryKey.Length];
                            for (int i = 0; i < keys.Length; i++)
                            {
                                keys[i] = CurrentRow[CurrentRow.Table.PrimaryKey[i], DataRowVersion.Original];
                            }
                            DataSet ds = _updateDataSet.Copy();
                            ds.RejectChanges();
                            DataRow realrow = ds.Tables[_updateTable].Rows.Find(keys);
                            DataRow parentrow = realrow.GetParentRow(relation.RelationName);
                            int index = parentrow.Table.Rows.IndexOf(parentrow);
                            return _updateDataSet.Tables[parentrow.Table.TableName].Rows[index];
                        }
                    }
                }
            }
            return null;
        }

        public void SetFieldValue(string fieldName, object value)
        {
            if (CurrentRow == null)
                return;

            _needSyncFVs = true;
            //modified by lily 2007/03/12 for detail's setFieldvalue
            if (_syncFieldValues.ContainsKey(fieldName))
                _syncFieldValues.Remove(fieldName);
            _syncFieldValues.Add(fieldName, value);
        }

        [Obsolete("The recommended alternative is SelectCmd", false)]
        public IInfoCommand GetInfoCommand()
        {
            return SelectCmd;
        }

        public void SetConnection(IDbConnection dbconn)
        {
            conn = dbconn;
        }

        public void SetTransaction(IDbTransaction dbtrans)
        {
            trans = dbtrans;
        }

        public string[] Update(DataSet dataSet, string sTable, bool isReplaceCmd)
        {
            return Update(dataSet, sTable, isReplaceCmd, "");
        }

        public string[] Update(DataSet dataSet, string sTable, bool isReplaceCmd, int state)
        {
            return Update(dataSet, sTable, isReplaceCmd, "", state);
        }

        public string[] Update(DataSet dataSet, string sTable, string replaceCmd)
        {
            return Update(dataSet, sTable, false, replaceCmd);
        }

        public string[] Update(DataSet dataSet, string sTable, string replaceCmd, int state)
        {
            return Update(dataSet, sTable, false, replaceCmd, state);
        }

        public string[] Update(DataSet dataSet, string sTable, bool isReplaceCmd, string replaceCmd)
        {
            return Update(dataSet, sTable, isReplaceCmd, replaceCmd, 0);
        }

        // state : 0 - all     1 - delete    2 - modifed,add
        public string[] Update(DataSet dataSet, string sTable, bool isReplaceCmd, string replaceCmd, int state)//dataset is all data...
        {
            return Update(dataSet, sTable, isReplaceCmd, replaceCmd, state, "");
        }

        public string[] Update(DataSet dataSet, string sTable, bool isReplaceCmd, string replaceCmd, int state, string tableName)//dataset is all data...
        {
            _updateDataSet = dataSet;
            _updateTable = sTable;
            List<string> sqlSentences = new List<string>();
            OpenConn();
            _needSyncFVs = false;

            if (replaceCmd != null && replaceCmd.Length != 0)
                this.SelectCmd.CommandText = replaceCmd;

            DataTable dataTable = dataSet.Tables[sTable];

            

            DataTable addedTable = dataTable.GetChanges(DataRowState.Added);
            DataTable modifiedTable = dataTable.GetChanges(DataRowState.Modified);
            DataTable deletedTable = dataTable.GetChanges(DataRowState.Deleted);

            if (addedTable == null && modifiedTable == null && deletedTable == null) return sqlSentences.ToArray();
            DataTable schema = new DataTable();
            if (!isReplaceCmd && this.SelectCmd.CommandType != CommandType.StoredProcedure)
            {
                schema = GetSchema();
            }
            // Get the updatecomp's infotranscations.
            List<IInfoTransaction> infoTransactions = GetInfoTransactions();

            OnBeforeApply(new UpdateComponentBeforeApplyEventArgs());

            List<IAutoNumber> autoNumbersList = null;
            KeyItems keysList = this.SelectCmd.KeyFields;
            ClientType type = DBUtils.GetDatabaseType(conn);
            OdbcDBType odbcType = DBUtils.GetOdbcDatabaseType(conn);
            #region Deleted.
            if (deletedTable != null && (state == 0 || state == 1))
            {
                foreach (DataRow row in deletedTable.Rows)
                {
                    // Set the currect row.
                    _currentRow = row;
                    if (this.SelectCmd.CommandType != CommandType.StoredProcedure)
                    {
                        // 替换Command的SQL
                        if (isReplaceCmd)
                        {
                            schema = GetSchema(isReplaceCmd, row, true);
                        }
                    }

                    OnBeforeDelete(new UpdateComponentBeforeDeleteEventArgs());
                    if (this.SelectCmd.CommandType != CommandType.StoredProcedure)
                    {
                        // Sync setfieldvalues
                        if (_needSyncFVs)
                        {
                            SetFieldValueSynchronizer syncer = new SetFieldValueSynchronizer(dataTable, row, schema, _syncFieldValues);
                            if (type == ClientType.ctOracle)
                            {
                                syncer.SyncOracle();
                            }
                            else
                            {
                                syncer.Sync();
                            }
                        }
                        _syncFieldValues.Clear();
                    }
                    if (this.SendSQLCmd && this.SelectCmd.CommandType != CommandType.StoredProcedure)
                    {
                        SQLBuilder builder = new SQLBuilder(this, row, schema);
                        String deleteSQL = "";
                        String writeBackSQLWherePart = "";

                        if (type == ClientType.ctMsSql)
                        {
                            deleteSQL = builder.GetDeleteSQL();
                            writeBackSQLWherePart = builder.GetWriteBackSQLWherePart();
                        }
                        else if (type == ClientType.ctOleDB)
                        {
                            deleteSQL = builder.GetDeleteSybase(tableName);
                            writeBackSQLWherePart = builder.GetWriteBackSybaseWherePart();
                        }
                        else if (type == ClientType.ctOracle)
                        {
                            deleteSQL = builder.GetDeleteOracle();
                            writeBackSQLWherePart = builder.GetWriteBackOracleWherePart();
                        }
                        else if (type == ClientType.ctODBC)
                        {
                            deleteSQL = builder.GetDeleteOdbc(tableName, odbcType);
                            writeBackSQLWherePart = builder.GetWriteBackOdbcWherePart(odbcType);
                        }
                        else if (type == ClientType.ctMySql)
                        {
                            deleteSQL = builder.GetDeleteMySql();
                            writeBackSQLWherePart = builder.GetWriteBackMySqlWherePart();
                        }
                        else if (type == ClientType.ctInformix)
                        {
                            deleteSQL = builder.GetDeleteInformix(tableName);
                            writeBackSQLWherePart = builder.GetWriteBackInformixWherePart();
                        }

                        IDbCommand deleteCommand = conn.CreateCommand();
                        deleteCommand.CommandText = deleteSQL;
                        if (trans != null)
                        {
                            deleteCommand.Transaction = trans;
                        }
                        sqlSentences.Add(deleteSQL);

                        if (_logInfo != null)
                        {
                            _logInfo.Log(row, schema, conn, trans, keysList);
                        }

                        try
                        {
                            Int32 i = deleteCommand.ExecuteNonQuery();
                            if (RowAffectsCheck)
                            {
                                if (i > 1)
                                {
                                    String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_AffectedMultiRows");
                                    throw new ArgumentException(message);
                                }
                                else if (i == 0)
                                {
                                    String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_AffectedNotRows");
                                    throw new ArgumentException(message);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                        if (infoTransactions != null && infoTransactions.Count != 0)
                        {
                            foreach (InfoTransaction iT in infoTransactions)
                            {
                                if (iT.Update(row, schema, writeBackSQLWherePart, conn, sqlSentences, trans) == true)
                                    continue;
                                else
                                    throw new Exception();
                            }
                        }
                    }
                    OnAfterDelete(new UpdateComponentAfterDeleteEventArgs());
                }
            }
            #endregion

            #region Modified.
            if (modifiedTable != null && (state == 0 || state == 2))
            {
                foreach (DataRow row in modifiedTable.Rows)
                {
                    // Set the currect row.
                    _currentRow = row;
                    if (this.SelectCmd.CommandType != CommandType.StoredProcedure)
                    {
                        // 替换Command的SQL
                        if (isReplaceCmd)
                        {
                            schema = GetSchema(isReplaceCmd, row);
                        }
                    }

                    OnBeforeModify(new UpdateComponentBeforeModifyEventArgs());
                    if (this.SelectCmd.CommandType != CommandType.StoredProcedure)
                    {
                        // Sync setfieldvalues
                        if (_needSyncFVs)
                        {
                            SetFieldValueSynchronizer syncer = new SetFieldValueSynchronizer(dataTable, row, schema, _syncFieldValues);
                            if (type == ClientType.ctOracle)
                            {
                                syncer.SyncOracle();
                            }
                            else
                            {
                                syncer.Sync();
                            }
                        }
                        _syncFieldValues.Clear();
                    }
                    if (this.SendSQLCmd && this.SelectCmd.CommandType != CommandType.StoredProcedure)
                    {
                        SQLBuilder builder = new SQLBuilder(this, row, schema);
                        String updateSQL = "";
                        String writeBackSQLWherePart = "";
                        IDbCommand updateCommand = conn.CreateCommand();
                        if (trans != null)
                        {
                            updateCommand.Transaction = trans;
                        }
                        if (type == ClientType.ctMsSql)
                        {
                            updateSQL = builder.GetUpdateSQL();
                            writeBackSQLWherePart = builder.GetWriteBackSQLWherePart();
                        }
                        else if (type == ClientType.ctOleDB)
                        {
                            updateSQL = builder.GetUpdateSybase(tableName, updateCommand);
                            writeBackSQLWherePart = builder.GetWriteBackSybaseWherePart();
                        }
                        else if (type == ClientType.ctOracle)
                        {
                            updateSQL = builder.GetUpdateOracle(updateCommand);
                            writeBackSQLWherePart = builder.GetWriteBackOracleWherePart();
                        }
                        else if (type == ClientType.ctODBC)
                        {
                            updateSQL = builder.GetUpdateOdbc(tableName, odbcType);
                            writeBackSQLWherePart = builder.GetWriteBackOdbcWherePart(odbcType);
                        }
                        else if (type == ClientType.ctMySql)
                        {
                            updateSQL = builder.GetUpdateMySql(updateCommand);
                            writeBackSQLWherePart = builder.GetWriteBackMySqlWherePart();
                        }
                        else if (type == ClientType.ctInformix)
                        {
                            updateSQL = builder.GetUpdateInformix(tableName);
                            writeBackSQLWherePart = builder.GetWriteBackInformixWherePart();
                        }

                        updateCommand.CommandText = updateSQL;

                        if (updateSQL == null || updateSQL.Length == 0)
                        {
                            // throw new Exception(); 
                            continue;
                        }

                        sqlSentences.Add(updateSQL);

                        if (_logInfo != null)
                        {
                            _logInfo.Log(row, schema, conn, trans, keysList);
                        }

                        try
                        {
                            Int32 i = updateCommand.ExecuteNonQuery();
                            if (RowAffectsCheck)
                            {
                                if (i > 1)
                                {
                                    String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_AffectedMultiRows");
                                    throw new ArgumentException(message);
                                }
                                else if (i == 0)
                                {
                                    String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_AffectedNotRows");
                                    throw new ArgumentException(message);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                        if (infoTransactions != null && infoTransactions.Count != 0)
                        {
                            foreach (InfoTransaction iT in infoTransactions)
                            {
                                if (iT.Update(row, schema, writeBackSQLWherePart, conn, sqlSentences, trans) == true)
                                    continue;
                                else
                                    throw new Exception();
                            }
                        }
                    }

                    OnAfterModify(new UpdateComponentAfterModifyEventArgs());
                }
            }
            #endregion

            #region Added.
            if (addedTable != null && (state == 0 || state == 2))
            {
                foreach (DataRow row in addedTable.Rows)
                {
                    // Set the currect row.
                    _currentRow = row;
                    if (this.SelectCmd.CommandType != CommandType.StoredProcedure)
                    {
                        autoNumbersList = GetAutoNumbersList(true);

                        // 替换Command的SQL
                        if (isReplaceCmd)
                        {
                            schema = GetSchema(isReplaceCmd, row);
                        }

                        // UpdateComponent.ServerModify和InfoDataSet.ServerModify、InfoNavigator.AutoApply配合使用。
                        // if (_serverModify)
                        // {
                        AutoNumberSynchronizer autoNumberSync = new AutoNumberSynchronizer(dataTable, row, schema, autoNumbersList);


                        if (type == ClientType.ctOracle)
                        {
                            autoNumberSync.SyncOracle();
                        }
                        else if (type == ClientType.ctMySql)
                        {
                            autoNumberSync.SyncMySql();
                        }
                        else
                        {
                            autoNumberSync.Sync();
                        }
                    }

                    OnBeforeInsert(new UpdateComponentBeforeInsertEventArgs());
                    if (this.SelectCmd.CommandType != CommandType.StoredProcedure)
                    {
                        // Sync setfieldvalues
                        if (_needSyncFVs)
                        {
                            SetFieldValueSynchronizer syncer = new SetFieldValueSynchronizer(dataTable, row, schema, _syncFieldValues);
                            if (type == ClientType.ctOracle)
                            {
                                syncer.SyncOracle();
                            }
                            else
                            {
                                syncer.Sync();
                            }
                        }
                        _syncFieldValues.Clear();
                    }
                    if (this.SendSQLCmd && this.SelectCmd.CommandType != CommandType.StoredProcedure)
                    {
                        SQLBuilder builder = new SQLBuilder(this, row, schema);
                        String insertSQL = "";
                        String writeBackSQLWherePart = "";
                        IDbCommand insertCommand = conn.CreateCommand();
                        if (trans != null)
                        {
                            insertCommand.Transaction = trans;
                        }
                        if (type == ClientType.ctMsSql)
                        {
                            insertSQL = builder.GetInsertSQL();
                            writeBackSQLWherePart = builder.GetWriteBackSQLWherePart();
                        }
                        else if (type == ClientType.ctOleDB)
                        {
                            insertSQL = builder.GetInsertSybase(tableName, insertCommand);
                            writeBackSQLWherePart = builder.GetWriteBackSybaseWherePart();
                        }
                        else if (type == ClientType.ctOracle)
                        {
                            insertSQL = builder.GetInsertOracle(insertCommand);
                            writeBackSQLWherePart = builder.GetWriteBackOracleWherePart();
                        }
                        else if (type == ClientType.ctODBC)
                        {
                            insertSQL = builder.GetInsertOdbc(tableName, odbcType);
                            writeBackSQLWherePart = builder.GetWriteBackOdbcWherePart(odbcType);
                        }
                        else if (type == ClientType.ctMySql)
                        {
                            insertSQL = builder.GetInsertMySql(insertCommand);
                            writeBackSQLWherePart = builder.GetWriteBackMySqlWherePart();
                        }
                        else if (type == ClientType.ctInformix)
                        {
                            insertSQL = builder.GetInsertInformix(tableName);
                            writeBackSQLWherePart = builder.GetWriteBackInformixWherePart();
                        }

                        insertCommand.CommandText = insertSQL;
                        sqlSentences.Add(insertSQL);

                        // Log
                        if (_logInfo != null)
                        {
                            _logInfo.Log(row, schema, conn, trans, keysList);
                        }

                        try
                        {
                            Int32 i = insertCommand.ExecuteNonQuery();
                            if (RowAffectsCheck)
                            {
                                if (i > 1)
                                {
                                    //Object[] clientInfos = ((DataModule)OwnerComp).ClientInfo;
                                    String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_AffectedMultiRows");
                                    throw new ArgumentException(message);
                                }
                                else if (i == 0)
                                {
                                    //Object[] clientInfos = ((DataModule)OwnerComp).ClientInfo;
                                    String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_AffectedNotRows");
                                    throw new ArgumentException(message);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                        if (infoTransactions != null && infoTransactions.Count != 0)
                        {
                            foreach (InfoTransaction iT in infoTransactions)
                            {
                                if (iT.Update(row, schema, writeBackSQLWherePart, conn, sqlSentences, trans) == true)
                                    continue;
                                else
                                    throw new Exception();
                            }
                        }
                    }
                    OnAfterInsert(new UpdateComponentAfterInsertEventArgs());


                    //String tableName = schema.Rows[0]["BaseTableName"].ToString(); ;
                    //DataRow identityRow = GetIdentityRow(tableName, writeBackSQLWherePart);
                    //if (identityRow != null)
                    //{
                    //    IdentitySynchronizer identitySync = new IdentitySynchronizer(dataTable, row, schema, identityRow);
                    //    identitySync.Sync();
                    //}
                }
            }
            #endregion
            OnAfterApply(new UpdateComponentAfterApplyEventArgs());
            return sqlSentences.ToArray();
        }

        private List<IInfoTransaction> GetInfoTransactions()
        {
            List<IInfoTransaction> infoTransList = new List<IInfoTransaction>();

            ArrayList infoTransAL = new ArrayList();
            Type infoTransType = typeof(IInfoTransaction);
            infoTransAL = this.OwnerComp.GetIntfObjects(infoTransType);
            Int32 i = infoTransAL.Count;
            if (i != 0)
            {
                for (Int32 n = 0; n < i; n++)
                {
                    InfoTransaction infoTrans = (InfoTransaction)infoTransAL[n];
                    if (infoTrans.UpdateComp != this) continue;

                    infoTransList.Add(infoTrans);
                }
            }
            return infoTransList;
        }

        private DataTable GetSchema()
        {
            return GetSchema(false, null);
        }

        private DataTable GetSchema(bool isReplaceCmd, DataRow row)
        {
            return GetSchema(isReplaceCmd, row, false);
        }

        private DataTable GetSchema(bool isReplaceCmd, DataRow row, bool isDeleted)
        {
            if (_selectCmd == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg__SelectCmdIsNull");
                throw new ArgumentException(String.Format(message, this.Name));
            }

            if (isReplaceCmd)
            {
                string tableName = "";
                if (isDeleted)
                {
                    tableName = row[((KeyItem)_selectCmd.KeyFields[0]).KeyName, DataRowVersion.Original].ToString();
                }
                else
                {
                    tableName = row[((KeyItem)_selectCmd.KeyFields[0]).KeyName].ToString();
                }

                SelectCmd.CommandText = "select * from [" + tableName + "]";
            }
            else
            {
                if (_schemas.ContainsKey(_selectCmd.Name.ToLower()))
                {
                    object obj = _schemas[_selectCmd.Name.ToLower()];
                    if (obj != null)
                    {
                        return (DataTable)_schemas[_selectCmd.Name.ToLower()];
                    }
                }
            }

            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = SelectCmd.CommandText;
            if(this.SelectCmdForUpdate != null)
                cmd.CommandText = SelectCmdForUpdate.CommandText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            IDataReader dr = cmd.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);

            DataTable schema = dr.GetSchemaTable();
            cmd.Cancel();
            dr.Close();

            _schemas.Add(_selectCmd.Name.ToLower(), schema);

            return schema;
        }

        /// <summary>
        /// Get updatecomponent's autonumbers.
        /// </summary>
        /// <returns></returns>
        private List<IAutoNumber> GetAutoNumbersList(Boolean isAddStep)
        {
            // if (_autoNumbersList != null) return _autoNumbersList;

            List<IAutoNumber> autoNumbersList = new List<IAutoNumber>();
            ArrayList autoNumberAL = new ArrayList();
            Type autoNumType = typeof(IAutoNumber);
            autoNumberAL = this.OwnerComp.GetIntfObjects(autoNumType);

            Int32 i = autoNumberAL.Count;
            if (i != 0)
            {
                for (Int32 n = 0; n < i; n++)
                {
                    AutoNumber a = (AutoNumber)autoNumberAL[n];
                    if (a.UpdateComp != this || !a.Active) continue;

                    a.GetNumber(conn, trans, isAddStep);
                    if (a.Number == null)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if (j == 4)
                            {
                                String message = SysMsg.GetSystemMessage(((DataModule)OwnerComp).Language, "Srvtools", "AutoNumber", "msg_AutoConflict");
                                throw new Exception(message);
                            }
                            else
                            {
                                System.Threading.Thread.Sleep(100);
                                a.GetNumber(conn, trans, isAddStep);
                                if (a.Number != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    autoNumbersList.Add(a);
                }
            }
            // _autoNumbersList = autoNumbersList;
            return autoNumbersList;
        }

        /// <summary>
        /// Open the selectcmd's connection.
        /// </summary>
        private void OpenConn()
        {
            if (conn == null)
            {
                if (this.SelectCmd == null)
                {
                    String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg__SelectCmdIsNull");
                    throw new ArgumentException(String.Format(message, this.Name));
                }

                if (this.SelectCmd.Connection == null)
                {
                    String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg__ConnectionIsNull");
                    throw new ArgumentException(String.Format(message, this.SelectCmd.Name));
                }
                else
                {
                    conn = this.SelectCmd.Connection;
                }
            }

            if (conn.State == ConnectionState.Closed) conn.Open();
        }

        // -------------------------------------------------------------------
        public void IdentitySync(DataSet dataSet, String sTable)
        {
            IdentitySync(dataSet, sTable, null);
        }

        public void IdentitySync(DataSet dataSet, String sTable, String realTableName)
        {
            DataSet insertedDS = dataSet.GetChanges(DataRowState.Added);
            if (insertedDS == null || insertedDS.Tables.Count == 0 || insertedDS.Tables[0].Rows.Count == 0)
            {
                return;
            }

            DataTable schema = GetSchema();
            String tableName = String.Empty;
            if (!String.IsNullOrEmpty(realTableName))
            {
                tableName = realTableName;
            }
            else
            {
                //DataTable schema = GenerateSchema();
                tableName = schema.Rows[0]["BaseTableName"].ToString();
                //给表加上owner by Rei
                String schemaName = schema.Rows[0]["BaseSchemaName"].ToString();
                if (schemaName != null && schemaName != "")
                    tableName = schemaName + "." + tableName;
            }
            DataTable dataTable = dataSet.Tables[sTable];
            ClientType type = DBUtils.GetDatabaseType(conn);
            string origiantable = DBUtils.GetTableName(SelectCmd.CommandText, true);
            foreach (DataRow r in insertedDS.Tables[0].Rows)
            {
                string wherePart = "";
                DataRow identityRow = null;
                if (ServerModifyGetMax)
                {
                    KeyItems keys = SelectCmd.KeyFields;
                    string columnName = "";
                    if (keys == null || keys.Count == 0)
                        columnName = dataTable.Columns[0].ColumnName;
                    else
                        columnName = ((KeyItem)keys[0]).KeyName;

                    wherePart = origiantable + "." + DBUtils.QuoteWords(columnName, type) + " in " + "(select max("
                        + DBUtils.QuoteWords(columnName, type) + ") from " + DBUtils.QuoteWords(tableName, type) + ")";
                }
                else
                {
                    SQLBuilder builder = new SQLBuilder(this, r, schema);

                    if (this.SelectCmd != null)
                    {
                        if (this.conn != null)
                        {
                            if (type == ClientType.ctOracle)
                            {
                                wherePart = builder.GetIdenSelectOracleWherePart();
                            }
                            else if (type == ClientType.ctOleDB)
                            {
                                wherePart = builder.GetIdenSelectSybaseWherePart();
                            }
                            else
                            {
                                wherePart = builder.GetIdenSelectSQLWherePart();
                            }
                        }
                    }
                }

                if (wherePart == null || wherePart.Length == 0)
                    continue;

                identityRow = GetIdentityRow(tableName, wherePart);
                if (identityRow != null)
                {
                    IdentitySynchronizer identitySync = new IdentitySynchronizer(dataTable, r, schema, identityRow);
                    identitySync.Sync();
                }
            }
        }

        private DataRow GetIdentityRow(String tableName, String wherePart)
        {
            ClientType type = DBUtils.GetDatabaseType(conn);
            string sQL = DBUtils.InsertWhere(SelectCmd.CommandText, wherePart);
            //String sQL = "select * from " + DBUtils.QuoteWords(tableName, type) + " where " + wherePart;
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sQL;
            IDbDataAdapter adapter = DBUtils.CreateDbDataAdapter(cmd);
            OpenConn();

            DataSet ds = new DataSet();
            if (cmd.Connection is OleDbConnection)
            {
                DataTable dt = new DataTable();
                (adapter as OleDbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
            }
            else
                adapter.Fill(ds);
            adapter.SelectCommand.Connection.Close();

            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count == 1)
            {
                return ds.Tables[0].Rows[0];
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region UpdateComponent's events

        protected void OnBeforeApply(UpdateComponentBeforeApplyEventArgs value)
        {
            UpdateComponentBeforeApplyEventHandler handler = (UpdateComponentBeforeApplyEventHandler)Events[EventBeforeApply];
            if ((handler != null) && (value is UpdateComponentBeforeApplyEventArgs))
            {
                handler(this, (UpdateComponentBeforeApplyEventArgs)value);
            }
        }

        public event UpdateComponentBeforeApplyEventHandler BeforeApply
        {
            add { Events.AddHandler(EventBeforeApply, value); }
            remove { Events.RemoveHandler(EventBeforeApply, value); }
        }

        protected void OnAfterApply(UpdateComponentAfterApplyEventArgs value)
        {
            UpdateComponentAfterApplyEventHandler handler = (UpdateComponentAfterApplyEventHandler)Events[EventAfterApply];
            if ((handler != null) && (value is UpdateComponentAfterApplyEventArgs))
            {
                handler(this, (UpdateComponentAfterApplyEventArgs)value);
            }
        }

        public event UpdateComponentAfterApplyEventHandler AfterApply
        {
            add { Events.AddHandler(EventAfterApply, value); }
            remove { Events.RemoveHandler(EventAfterApply, value); }
        }

        public void OnAfterApplied(EventArgs value)
        {
            EventHandler handler = (EventHandler)Events[EventAfterApplied];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        public event EventHandler AfterApplied
        {
            add { Events.AddHandler(EventAfterApplied, value); }
            remove { Events.RemoveHandler(EventAfterApplied, value); }
        }

        protected void OnBeforeInsert(UpdateComponentBeforeInsertEventArgs value)
        {
            UpdateComponentBeforeInsertEventHandler handler = (UpdateComponentBeforeInsertEventHandler)Events[EventBeforeInsert];
            if ((handler != null) && (value is UpdateComponentBeforeInsertEventArgs))
            {
                handler(this, (UpdateComponentBeforeInsertEventArgs)value);
            }
        }

        public event UpdateComponentBeforeInsertEventHandler BeforeInsert
        {
            add { Events.AddHandler(EventBeforeInsert, value); }
            remove { Events.RemoveHandler(EventBeforeInsert, value); }
        }

        protected void OnAfterInsert(UpdateComponentAfterInsertEventArgs value)
        {
            UpdateComponentAfterInsertEventHandler handler = (UpdateComponentAfterInsertEventHandler)Events[EventAfterInsert];
            if ((handler != null) && (value is UpdateComponentAfterInsertEventArgs))
            {
                handler(this, (UpdateComponentAfterInsertEventArgs)value);
            }
        }

        public event UpdateComponentAfterInsertEventHandler AfterInsert
        {
            add { Events.AddHandler(EventAfterInsert, value); }
            remove { Events.RemoveHandler(EventAfterInsert, value); }
        }


        protected void OnBeforeDelete(UpdateComponentBeforeDeleteEventArgs value)
        {
            UpdateComponentBeforeDeleteEventHandler handler = (UpdateComponentBeforeDeleteEventHandler)Events[EventBeforeDelete];
            if ((handler != null) && (value is UpdateComponentBeforeDeleteEventArgs))
            {
                handler(this, (UpdateComponentBeforeDeleteEventArgs)value);
            }
        }

        public event UpdateComponentBeforeDeleteEventHandler BeforeDelete
        {
            add { Events.AddHandler(EventBeforeDelete, value); }
            remove { Events.RemoveHandler(EventBeforeDelete, value); }
        }

        protected void OnAfterDelete(UpdateComponentAfterDeleteEventArgs value)
        {
            UpdateComponentAfterDeleteEventHandler handler = (UpdateComponentAfterDeleteEventHandler)Events[EventAfterDelete];
            if ((handler != null) && (value is UpdateComponentAfterDeleteEventArgs))
            {
                handler(this, (UpdateComponentAfterDeleteEventArgs)value);
            }
        }

        public event UpdateComponentAfterDeleteEventHandler AfterDelete
        {
            add { Events.AddHandler(EventAfterDelete, value); }
            remove { Events.RemoveHandler(EventAfterDelete, value); }
        }


        protected void OnBeforeModify(UpdateComponentBeforeModifyEventArgs value)
        {
            UpdateComponentBeforeModifyEventHandler handler = (UpdateComponentBeforeModifyEventHandler)Events[EventBeforeModify];
            if ((handler != null) && (value is UpdateComponentBeforeModifyEventArgs))
            {
                handler(this, (UpdateComponentBeforeModifyEventArgs)value);
            }
        }

        public event UpdateComponentBeforeModifyEventHandler BeforeModify
        {
            add { Events.AddHandler(EventBeforeModify, value); }
            remove { Events.RemoveHandler(EventBeforeModify, value); }
        }

        protected void OnAfterModify(UpdateComponentAfterModifyEventArgs value)
        {
            UpdateComponentAfterModifyEventHandler handler = (UpdateComponentAfterModifyEventHandler)Events[EventAfterModify];
            if ((handler != null) && (value is UpdateComponentAfterModifyEventArgs))
            {
                handler(this, (UpdateComponentAfterModifyEventArgs)value);
            }
        }

        public event UpdateComponentAfterModifyEventHandler AfterModify
        {
            add { Events.AddHandler(EventAfterModify, value); }
            remove { Events.RemoveHandler(EventAfterModify, value); }
        }

        #endregion

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        #region Dispose

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Vars

        private DataSet _updateDataSet;
        private string _updateTable;

        private System.ComponentModel.Container components = null;
        // private List<IAutoNumber> _autoNumbersList;
        private InfoCommand _selectCmd;
        private InfoCommand _selectCmdForUpdate;
        //private String _keyField;
        //private KeyItem _keyField;
        private WhereModeType _whereModeType;
        private Boolean _exceptJoin;
        private Boolean _autoTrans;
        private IsolationLevel _transIsolationLevel;
        private LogInfo _logInfo;
        private String _name;
        private Boolean _serverModify;
        private ServerModifyColumnCollection _serverModifyColumns;
        private bool _serverModifyGetMax;
        private Hashtable _schemas;

        private DataRow _currentRow;
        private Hashtable _syncFieldValues;
        private bool _needSyncFVs;

        //private FieldAttrCollection _fieldAttrs;
        //private List<FieldAttr> _fieldAttrs;
        private FieldAttrCollection _fieldAttrs;
        //private List<InfoTransaction> _infoTransactions;
        public IDbConnection conn = null;
        public IDbTransaction trans = null;

        internal static readonly object EventBeforeApply = new object();
        internal static readonly object EventAfterApply = new object();
        internal static readonly object EventAfterApplied = new object();
        internal static readonly object EventBeforeInsert = new object();
        internal static readonly object EventAfterInsert = new object();
        internal static readonly object EventBeforeDelete = new object();
        internal static readonly object EventAfterDelete = new object();
        internal static readonly object EventBeforeModify = new object();
        internal static readonly object EventAfterModify = new object();

        #endregion
    }

    public enum WhereModeType
    {
        /// <summary>
        /// Use PrimaryKey 
        /// </summary>
        Keyfields = 0,

        /// <summary>
        /// Use PrimaryKey and FieldAttrs
        /// </summary>
        FieldAttrs = 1,

        /// <summary>
        /// Use All FieldAttrs
        /// </summary>
        All = 2
    }
}
