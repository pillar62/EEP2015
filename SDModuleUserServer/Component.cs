using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Reflection;
using Microsoft.Win32;
using System.IO;
using Srvtools;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using System.Data.OracleClient;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.Odbc;
using System.IO.Packaging;
#if Oracle2
//using Oracle.DataAccess.Client;
#endif

namespace SDModuleUserServer
{
    /// <summary>
    /// Summary description for Component.
    /// </summary>
    public class Component : DataModule
    {
        private ServiceManager serviceManager;
        private InfoConnection InfoConnection1;
        private InfoCommand GetTableName;
        private InfoCommand GetSchema;
        private InfoCommand SQLGOON;
        private InfoCommand COLDEF;
        private UpdateComponent ucCOLDEF;
        private InfoCommand REFVAL;
        private UpdateComponent updateComponent1;
        private UpdateComponent updateComponent2;
        private InfoCommand REFVAL_DETAIL;
        private InfoDataSource infoDataSource1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

        public Component(System.ComponentModel.IContainer container)
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public Component()
        {
            ///
            /// This call is required by the Windows.Forms Designer.
            ///
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

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

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Srvtools.Service service1 = new Srvtools.Service();
            Srvtools.Service service2 = new Srvtools.Service();
            Srvtools.Service service3 = new Srvtools.Service();
            Srvtools.Service service4 = new Srvtools.Service();
            Srvtools.Service service5 = new Srvtools.Service();
            Srvtools.Service service6 = new Srvtools.Service();
            Srvtools.Service service7 = new Srvtools.Service();
            Srvtools.Service service8 = new Srvtools.Service();
            Srvtools.Service service9 = new Srvtools.Service();
            Srvtools.Service service10 = new Srvtools.Service();
            Srvtools.Service service11 = new Srvtools.Service();
            Srvtools.Service service12 = new Srvtools.Service();
            Srvtools.Service service13 = new Srvtools.Service();
            Srvtools.Service service14 = new Srvtools.Service();
            Srvtools.Service service15 = new Srvtools.Service();
            Srvtools.Service service16 = new Srvtools.Service();
            Srvtools.Service service17 = new Srvtools.Service();
            Srvtools.Service service18 = new Srvtools.Service();
            Srvtools.Service service19 = new Srvtools.Service();
            Srvtools.Service service20 = new Srvtools.Service();
            Srvtools.Service service21 = new Srvtools.Service();
            Srvtools.Service service22 = new Srvtools.Service();
            Srvtools.Service service23 = new Srvtools.Service();
            Srvtools.Service service24 = new Srvtools.Service();
            Srvtools.Service service25 = new Srvtools.Service();
            Srvtools.Service service26 = new Srvtools.Service();
            Srvtools.Service service27 = new Srvtools.Service();
            Srvtools.Service service28 = new Srvtools.Service();
            Srvtools.Service service29 = new Srvtools.Service();
            Srvtools.Service service30 = new Srvtools.Service();
            Srvtools.Service service31 = new Srvtools.Service();
            Srvtools.Service service32 = new Srvtools.Service();
            Srvtools.Service service33 = new Srvtools.Service();
            Srvtools.KeyItem keyItem1 = new Srvtools.KeyItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            Srvtools.KeyItem keyItem2 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem5 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem6 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem7 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem8 = new Srvtools.KeyItem();
            Srvtools.ColumnItem columnItem1 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem2 = new Srvtools.ColumnItem();
            this.serviceManager = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.GetTableName = new Srvtools.InfoCommand(this.components);
            this.GetSchema = new Srvtools.InfoCommand(this.components);
            this.SQLGOON = new Srvtools.InfoCommand(this.components);
            this.COLDEF = new Srvtools.InfoCommand(this.components);
            this.ucCOLDEF = new Srvtools.UpdateComponent(this.components);
            this.REFVAL = new Srvtools.InfoCommand(this.components);
            this.updateComponent1 = new Srvtools.UpdateComponent(this.components);
            this.updateComponent2 = new Srvtools.UpdateComponent(this.components);
            this.REFVAL_DETAIL = new Srvtools.InfoCommand(this.components);
            this.infoDataSource1 = new Srvtools.InfoDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GetTableName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GetSchema)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SQLGOON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.COLDEF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.REFVAL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.REFVAL_DETAIL)).BeginInit();
            // 
            // serviceManager
            // 
            service1.DelegateName = "CreateTable";
            service1.NonLogin = false;
            service1.ServiceName = "CreateTable";
            service2.DelegateName = "CallTableSchema";
            service2.NonLogin = false;
            service2.ServiceName = "CallTableSchema";
            service3.DelegateName = "IsTableExist";
            service3.NonLogin = false;
            service3.ServiceName = "IsTableExist";
            service4.DelegateName = "ExcuteSqlStr";
            service4.NonLogin = false;
            service4.ServiceName = "ExcuteSqlStr";
            service5.DelegateName = "CreateExistTable";
            service5.NonLogin = false;
            service5.ServiceName = "CreateExistTable";
            service6.DelegateName = "Print";
            service6.NonLogin = false;
            service6.ServiceName = "Print";
            service7.DelegateName = "CallRefVal";
            service7.NonLogin = false;
            service7.ServiceName = "CallRefVal";
            service8.DelegateName = "GetServerPath";
            service8.NonLogin = false;
            service8.ServiceName = "GetServerPath";
            service9.DelegateName = "GetTableNamesFromSql";
            service9.NonLogin = false;
            service9.ServiceName = "GetTableNamesFromSql";
            service10.DelegateName = "GetColumnNamesByTableName";
            service10.NonLogin = false;
            service10.ServiceName = "GetColumnNamesByTableName";
            service11.DelegateName = "GetColumnNamesFromSQL";
            service11.NonLogin = false;
            service11.ServiceName = "GetColumnNamesFromSQL";
            service12.DelegateName = "GetAllTables";
            service12.NonLogin = false;
            service12.ServiceName = "GetAllTables";
            service13.DelegateName = "CreateNewDB";
            service13.NonLogin = false;
            service13.ServiceName = "CreateNewDB";
            service14.DelegateName = "DeleteDB";
            service14.NonLogin = false;
            service14.ServiceName = "DeleteDB";
            service15.DelegateName = "CreateSystemTable";
            service15.NonLogin = false;
            service15.ServiceName = "CreateSystemTable";
            service16.DelegateName = "SetSystemTableForDBXML";
            service16.NonLogin = false;
            service16.ServiceName = "SetSystemTableForDBXML";
            service17.DelegateName = "GetDBXml";
            service17.NonLogin = false;
            service17.ServiceName = "GetDBXml";
            service18.DelegateName = "AutoSeqMenuID";
            service18.NonLogin = false;
            service18.ServiceName = "AutoSeqMenuID";
            service19.DelegateName = "GetInsertString";
            service19.NonLogin = false;
            service19.ServiceName = "GetInsertString";
            service20.DelegateName = "RemotePrint";
            service20.NonLogin = false;
            service20.ServiceName = "RemotePrint";
            service21.DelegateName = "UpdateDB";
            service21.NonLogin = false;
            service21.ServiceName = "UpdateDB";
            service22.DelegateName = "SetAllSolutionsToDB";
            service22.NonLogin = false;
            service22.ServiceName = "SetAllSolutionsToDB";
            service23.DelegateName = "DeleteMenuItemType";
            service23.NonLogin = false;
            service23.ServiceName = "DeleteMenuItemType";
            service24.DelegateName = "ExcuteSqlStr2";
            service24.NonLogin = false;
            service24.ServiceName = "ExcuteSqlStr2";
            service25.DelegateName = "UpdateSolutionIDBecauseModity";
            service25.NonLogin = false;
            service25.ServiceName = "UpdateSolutionIDBecauseModity";
            service26.DelegateName = "ConnectionTest";
            service26.NonLogin = false;
            service26.ServiceName = "ConnectionTest";
            service27.DelegateName = "GetExistDBXml";
            service27.NonLogin = false;
            service27.ServiceName = "GetExistDBXml";
            service28.DelegateName = "PrintSchedul";
            service28.NonLogin = false;
            service28.ServiceName = "PrintSchedul";
            service29.DelegateName = "ExpiryDateEmailSchedul";
            service29.NonLogin = false;
            service29.ServiceName = "ExpiryDateEmailSchedul";
            service30.DelegateName = "GetAllViewAndSP";
            service30.NonLogin = false;
            service30.ServiceName = "GetAllViewAndSP";
            service31.DelegateName = "updateViewAndSP";
            service31.NonLogin = false;
            service31.ServiceName = "updateViewAndSP";
            service32.DelegateName = "DBNameTest";
            service32.NonLogin = false;
            service32.ServiceName = "DBNameTest";
            service33.DelegateName = "BuildCordova";
            service33.NonLogin = false;
            service33.ServiceName = "BuildCordova";
            this.serviceManager.ServiceCollection.Add(service1);
            this.serviceManager.ServiceCollection.Add(service2);
            this.serviceManager.ServiceCollection.Add(service3);
            this.serviceManager.ServiceCollection.Add(service4);
            this.serviceManager.ServiceCollection.Add(service5);
            this.serviceManager.ServiceCollection.Add(service6);
            this.serviceManager.ServiceCollection.Add(service7);
            this.serviceManager.ServiceCollection.Add(service8);
            this.serviceManager.ServiceCollection.Add(service9);
            this.serviceManager.ServiceCollection.Add(service10);
            this.serviceManager.ServiceCollection.Add(service11);
            this.serviceManager.ServiceCollection.Add(service12);
            this.serviceManager.ServiceCollection.Add(service13);
            this.serviceManager.ServiceCollection.Add(service14);
            this.serviceManager.ServiceCollection.Add(service15);
            this.serviceManager.ServiceCollection.Add(service16);
            this.serviceManager.ServiceCollection.Add(service17);
            this.serviceManager.ServiceCollection.Add(service18);
            this.serviceManager.ServiceCollection.Add(service19);
            this.serviceManager.ServiceCollection.Add(service20);
            this.serviceManager.ServiceCollection.Add(service21);
            this.serviceManager.ServiceCollection.Add(service22);
            this.serviceManager.ServiceCollection.Add(service23);
            this.serviceManager.ServiceCollection.Add(service24);
            this.serviceManager.ServiceCollection.Add(service25);
            this.serviceManager.ServiceCollection.Add(service26);
            this.serviceManager.ServiceCollection.Add(service27);
            this.serviceManager.ServiceCollection.Add(service28);
            this.serviceManager.ServiceCollection.Add(service29);
            this.serviceManager.ServiceCollection.Add(service30);
            this.serviceManager.ServiceCollection.Add(service31);
            this.serviceManager.ServiceCollection.Add(service32);
            this.serviceManager.ServiceCollection.Add(service33);
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "ERPS";
            // 
            // GetTableName
            // 
            this.GetTableName.CacheConnection = false;
            this.GetTableName.CommandText = "select(case when (Charindex(\' \',Rtrim(Ltrim(name)),0) != 0) then \'[\' + [name] + \'" +
    "]\' else [name] end ) as name from sysobjects where xtype in (\'u\',\'U\')  order by " +
    "[name]";
            this.GetTableName.CommandTimeout = 30;
            this.GetTableName.CommandType = System.Data.CommandType.Text;
            this.GetTableName.DynamicTableName = false;
            this.GetTableName.EEPAlias = null;
            this.GetTableName.EncodingAfter = null;
            this.GetTableName.EncodingBefore = "Windows-1252";
            this.GetTableName.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "name";
            this.GetTableName.KeyFields.Add(keyItem1);
            this.GetTableName.MultiSetWhere = false;
            this.GetTableName.Name = "GetTableName";
            this.GetTableName.NotificationAutoEnlist = false;
            this.GetTableName.SecExcept = null;
            this.GetTableName.SecFieldName = null;
            this.GetTableName.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.GetTableName.SelectPaging = false;
            this.GetTableName.SelectTop = 0;
            this.GetTableName.SiteControl = false;
            this.GetTableName.SiteFieldName = null;
            this.GetTableName.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // GetSchema
            // 
            this.GetSchema.CacheConnection = false;
            this.GetSchema.CommandText = resources.GetString("GetSchema.CommandText");
            this.GetSchema.CommandTimeout = 30;
            this.GetSchema.CommandType = System.Data.CommandType.Text;
            this.GetSchema.DynamicTableName = false;
            this.GetSchema.EEPAlias = null;
            this.GetSchema.EncodingAfter = null;
            this.GetSchema.EncodingBefore = "Windows-1252";
            this.GetSchema.InfoConnection = this.InfoConnection1;
            keyItem2.KeyName = "TableName";
            keyItem3.KeyName = "ColumnName";
            this.GetSchema.KeyFields.Add(keyItem2);
            this.GetSchema.KeyFields.Add(keyItem3);
            this.GetSchema.MultiSetWhere = false;
            this.GetSchema.Name = "GetSchema";
            this.GetSchema.NotificationAutoEnlist = false;
            this.GetSchema.SecExcept = null;
            this.GetSchema.SecFieldName = null;
            this.GetSchema.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.GetSchema.SelectPaging = false;
            this.GetSchema.SelectTop = 0;
            this.GetSchema.SiteControl = false;
            this.GetSchema.SiteFieldName = null;
            this.GetSchema.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // SQLGOON
            // 
            this.SQLGOON.CacheConnection = false;
            this.SQLGOON.CommandText = null;
            this.SQLGOON.CommandTimeout = 30;
            this.SQLGOON.CommandType = System.Data.CommandType.Text;
            this.SQLGOON.DynamicTableName = false;
            this.SQLGOON.EEPAlias = null;
            this.SQLGOON.EncodingAfter = null;
            this.SQLGOON.EncodingBefore = "Windows-1252";
            this.SQLGOON.InfoConnection = this.InfoConnection1;
            this.SQLGOON.MultiSetWhere = false;
            this.SQLGOON.Name = "SQLGOON";
            this.SQLGOON.NotificationAutoEnlist = false;
            this.SQLGOON.SecExcept = null;
            this.SQLGOON.SecFieldName = null;
            this.SQLGOON.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.SQLGOON.SelectPaging = false;
            this.SQLGOON.SelectTop = 0;
            this.SQLGOON.SiteControl = false;
            this.SQLGOON.SiteFieldName = null;
            this.SQLGOON.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // COLDEF
            // 
            this.COLDEF.CacheConnection = false;
            this.COLDEF.CommandText = "select * from COLDEF";
            this.COLDEF.CommandTimeout = 30;
            this.COLDEF.CommandType = System.Data.CommandType.Text;
            this.COLDEF.DynamicTableName = false;
            this.COLDEF.EEPAlias = null;
            this.COLDEF.EncodingAfter = null;
            this.COLDEF.EncodingBefore = "Windows-1252";
            this.COLDEF.InfoConnection = this.InfoConnection1;
            keyItem4.KeyName = "TABLE_NAME";
            keyItem5.KeyName = "FIELD_NAME";
            this.COLDEF.KeyFields.Add(keyItem4);
            this.COLDEF.KeyFields.Add(keyItem5);
            this.COLDEF.MultiSetWhere = false;
            this.COLDEF.Name = "COLDEF";
            this.COLDEF.NotificationAutoEnlist = false;
            this.COLDEF.SecExcept = null;
            this.COLDEF.SecFieldName = null;
            this.COLDEF.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.COLDEF.SelectPaging = false;
            this.COLDEF.SelectTop = 0;
            this.COLDEF.SiteControl = false;
            this.COLDEF.SiteFieldName = null;
            this.COLDEF.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucCOLDEF
            // 
            this.ucCOLDEF.AutoTrans = true;
            this.ucCOLDEF.ExceptJoin = false;
            this.ucCOLDEF.LogInfo = null;
            this.ucCOLDEF.Name = "ucCOLDEF";
            this.ucCOLDEF.RowAffectsCheck = true;
            this.ucCOLDEF.SelectCmd = this.COLDEF;
            this.ucCOLDEF.SelectCmdForUpdate = null;
            this.ucCOLDEF.ServerModify = true;
            this.ucCOLDEF.ServerModifyGetMax = false;
            this.ucCOLDEF.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucCOLDEF.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucCOLDEF.UseTranscationScope = false;
            this.ucCOLDEF.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // REFVAL
            // 
            this.REFVAL.CacheConnection = false;
            this.REFVAL.CommandText = "select SYS_REFVAL.* from SYS_REFVAL";
            this.REFVAL.CommandTimeout = 30;
            this.REFVAL.CommandType = System.Data.CommandType.Text;
            this.REFVAL.DynamicTableName = false;
            this.REFVAL.EEPAlias = null;
            this.REFVAL.EncodingAfter = null;
            this.REFVAL.EncodingBefore = "Windows-1252";
            this.REFVAL.InfoConnection = this.InfoConnection1;
            keyItem6.KeyName = "REFVAL_NO";
            this.REFVAL.KeyFields.Add(keyItem6);
            this.REFVAL.MultiSetWhere = false;
            this.REFVAL.Name = "REFVAL";
            this.REFVAL.NotificationAutoEnlist = false;
            this.REFVAL.SecExcept = null;
            this.REFVAL.SecFieldName = null;
            this.REFVAL.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.REFVAL.SelectPaging = false;
            this.REFVAL.SelectTop = 0;
            this.REFVAL.SiteControl = false;
            this.REFVAL.SiteFieldName = null;
            this.REFVAL.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // updateComponent1
            // 
            this.updateComponent1.AutoTrans = true;
            this.updateComponent1.ExceptJoin = false;
            this.updateComponent1.LogInfo = null;
            this.updateComponent1.Name = "updateComponent1";
            this.updateComponent1.RowAffectsCheck = true;
            this.updateComponent1.SelectCmd = this.REFVAL;
            this.updateComponent1.SelectCmdForUpdate = null;
            this.updateComponent1.ServerModify = true;
            this.updateComponent1.ServerModifyGetMax = false;
            this.updateComponent1.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateComponent1.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateComponent1.UseTranscationScope = false;
            this.updateComponent1.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // updateComponent2
            // 
            this.updateComponent2.AutoTrans = true;
            this.updateComponent2.ExceptJoin = false;
            this.updateComponent2.LogInfo = null;
            this.updateComponent2.Name = "updateComponent2";
            this.updateComponent2.RowAffectsCheck = true;
            this.updateComponent2.SelectCmd = this.REFVAL_DETAIL;
            this.updateComponent2.SelectCmdForUpdate = null;
            this.updateComponent2.ServerModify = true;
            this.updateComponent2.ServerModifyGetMax = false;
            this.updateComponent2.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateComponent2.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateComponent2.UseTranscationScope = false;
            this.updateComponent2.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // REFVAL_DETAIL
            // 
            this.REFVAL_DETAIL.CacheConnection = false;
            this.REFVAL_DETAIL.CommandText = "select SYS_REFVAL_D1.* from SYS_REFVAL_D1";
            this.REFVAL_DETAIL.CommandTimeout = 30;
            this.REFVAL_DETAIL.CommandType = System.Data.CommandType.Text;
            this.REFVAL_DETAIL.DynamicTableName = false;
            this.REFVAL_DETAIL.EEPAlias = null;
            this.REFVAL_DETAIL.EncodingAfter = null;
            this.REFVAL_DETAIL.EncodingBefore = "Windows-1252";
            this.REFVAL_DETAIL.InfoConnection = this.InfoConnection1;
            keyItem7.KeyName = "REFVAL_NO";
            keyItem8.KeyName = "FIELD_NAME";
            this.REFVAL_DETAIL.KeyFields.Add(keyItem7);
            this.REFVAL_DETAIL.KeyFields.Add(keyItem8);
            this.REFVAL_DETAIL.MultiSetWhere = false;
            this.REFVAL_DETAIL.Name = "REFVAL_DETAIL";
            this.REFVAL_DETAIL.NotificationAutoEnlist = false;
            this.REFVAL_DETAIL.SecExcept = null;
            this.REFVAL_DETAIL.SecFieldName = null;
            this.REFVAL_DETAIL.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.REFVAL_DETAIL.SelectPaging = false;
            this.REFVAL_DETAIL.SelectTop = 0;
            this.REFVAL_DETAIL.SiteControl = false;
            this.REFVAL_DETAIL.SiteFieldName = null;
            this.REFVAL_DETAIL.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // infoDataSource1
            // 
            this.infoDataSource1.Detail = this.REFVAL_DETAIL;
            columnItem1.FieldName = "REFVAL_NO";
            this.infoDataSource1.DetailColumns.Add(columnItem1);
            this.infoDataSource1.DynamicTableName = false;
            this.infoDataSource1.Master = this.REFVAL;
            columnItem2.FieldName = "REFVAL_NO";
            this.infoDataSource1.MasterColumns.Add(columnItem2);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GetTableName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GetSchema)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SQLGOON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.COLDEF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.REFVAL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.REFVAL_DETAIL)).EndInit();

        }

        #endregion

        public object CreateNewDB(object[] paramters)
        {
            string userid = paramters[0] as string;
            string aliasName = paramters[1] as string;
            bool split = (bool)paramters[2];
            bool isExist = paramters.Length > 3;
            string connectionString = paramters.Length > 3 ? paramters[3].ToString() : "";
            string connnectionPassword = paramters.Length > 3 ? paramters[4].ToString() : "";
            string type = paramters.Length > 3 ? paramters[5].ToString() : "";
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string databasename = string.Empty;
            string dbAlias = string.Empty;
            string ret = string.Empty;
            int index = 0;
            if (!isExist)
            {
                try
                {
                    dbAlias = DbConnectionSet.SystemDatabase;
                    connection = AllocateConnection(dbAlias);
                    transaction = connection.BeginTransaction();
                    string strSql = "select max(DBNAME) from SYS_SDALIAS where USERID ='" + userid + "'";
                    DataSet ds = this.ExecuteSql(strSql, connection, transaction);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString() != "")
                        {
                            index = Int32.Parse(ds.Tables[0].Rows[0][0].ToString().Substring(userid.Length));
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    ret = ex.Message;
                    transaction.Rollback();
                    return new object[] { 1, ret };
                }
                finally
                {
                    transaction.Dispose();
                    ReleaseConnection(dbAlias, connection);
                }
            }
            try
            {
                if (!isExist)
                {
                    connection = AllocateConnection(dbAlias);
                    databasename = userid + (index + 1).ToString("00");
                    string strSql = "create database " + databasename;
                    this.ExecuteCommand(strSql, connection, null);

                    if (File.Exists(Path.Combine(EEPRegistry.Server, "SDModule", "SYSDB.xml")))
                    {
                        XmlDocument xmlsys = new XmlDocument();
                        xmlsys.Load(Path.Combine(EEPRegistry.Server, "SDModule", "SYSDB.xml"));
                        XmlNode nodeDataBaseSys = xmlsys.SelectSingleNode("DataBase");
                        if (nodeDataBaseSys != null)
                        {
                            string datapath = nodeDataBaseSys.Attributes["String"].Value;
                            datapath = datapath.Replace("SYS_TARGET", databasename);
                            if (!Directory.Exists(Path.Combine(EEPRegistry.Server, "SDModule", userid)))
                            {
                                Directory.CreateDirectory(Path.Combine(EEPRegistry.Server, "SDModule", userid));
                            }

                            string filepath = Path.Combine(EEPRegistry.Server, "SDModule", userid, "DB.xml");

                            XmlDocument xml = new XmlDocument();

                            if (File.Exists(filepath))
                            {
                                xml.Load(filepath);
                            }
                            XmlNode nodeInfolight = xml.SelectSingleNode("InfolightDB");
                            if (nodeInfolight == null)
                            {
                                nodeInfolight = xml.CreateElement("InfolightDB");
                                xml.AppendChild(nodeInfolight);
                            }
                            XmlNode nodeDataBase = nodeInfolight.SelectSingleNode("DataBase");
                            if (nodeDataBase == null)
                            {
                                nodeDataBase = xml.CreateElement("DataBase");
                                nodeInfolight.AppendChild(nodeDataBase);
                            }

                            XmlNode nodeDB = nodeInfolight.SelectSingleNode(aliasName);
                            if (nodeDB != null)
                            {
                                nodeDataBase.RemoveChild(nodeDB);
                            }

                            XmlNode node = xml.CreateElement(aliasName);
                            XmlAttribute att = xml.CreateAttribute("String");
                            att.Value = datapath;
                            //att.Value = "Data Source=.;Initial Catalog=" + databasename + ";Integrated Security=True";
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("Type");
                            att.Value = nodeDataBaseSys.Attributes["Type"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("OdbcType");
                            att.Value = nodeDataBaseSys.Attributes["OdbcType"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("MaxCount");
                            att.Value = nodeDataBaseSys.Attributes["MaxCount"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("TimeOut");
                            att.Value = nodeDataBaseSys.Attributes["TimeOut"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("Master");
                            att.Value = split ? "1" : "0";
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("Encrypt");
                            att.Value = nodeDataBaseSys.Attributes["Encrypt"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("Password");
                            att.Value = nodeDataBaseSys.Attributes["Password"].Value;
                            node.Attributes.Append(att);
                            nodeDataBase.AppendChild(node);

                            XmlNode nodeSystemDB = nodeInfolight.SelectSingleNode("SystemDB");
                            if (nodeSystemDB == null)
                            {
                                nodeSystemDB = xml.CreateElement("SystemDB");
                                nodeInfolight.AppendChild(nodeSystemDB);
                            }
                            xml.Save(filepath);

                            ret = databasename;
                        }
                    }
                }
                else
                {
                    if (File.Exists(Path.Combine(EEPRegistry.Server, "SDModule", "SYSDB.xml")))
                    {
                        XmlDocument xmlsys = new XmlDocument();
                        xmlsys.Load(Path.Combine(EEPRegistry.Server, "SDModule", "SYSDB.xml"));
                        XmlNode nodeDataBaseSys = xmlsys.SelectSingleNode("DataBase");
                        if (nodeDataBaseSys != null)
                        {
                            if (!Directory.Exists(Path.Combine(EEPRegistry.Server, "SDModule", userid)))
                            {
                                Directory.CreateDirectory(Path.Combine(EEPRegistry.Server, "SDModule", userid));
                            }

                            string filepath = Path.Combine(EEPRegistry.Server, "SDModule", userid, "DB.xml");

                            XmlDocument xml = new XmlDocument();

                            if (File.Exists(filepath))
                            {
                                xml.Load(filepath);
                            }
                            XmlNode nodeInfolight = xml.SelectSingleNode("InfolightDB");
                            if (nodeInfolight == null)
                            {
                                nodeInfolight = xml.CreateElement("InfolightDB");
                                xml.AppendChild(nodeInfolight);
                            }
                            XmlNode nodeDataBase = nodeInfolight.SelectSingleNode("DataBase");
                            if (nodeDataBase == null)
                            {
                                nodeDataBase = xml.CreateElement("DataBase");
                                nodeInfolight.AppendChild(nodeDataBase);
                            }

                            XmlNode nodeDB = nodeInfolight.SelectSingleNode(aliasName);
                            if (nodeDB != null)
                            {
                                nodeDataBase.RemoveChild(nodeDB);
                            }

                            XmlNode node = xml.CreateElement(aliasName);
                            XmlAttribute att = xml.CreateAttribute("String");
                            att.Value = connectionString;
                            //att.Value = "Data Source=.;Initial Catalog=" + databasename + ";Integrated Security=True";
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("Type");
                            if (type.ToLower() == "sql")
                                att.Value = "1";
                            else if (type.ToLower() == "oracle")
                                att.Value = "2";
                            else att.Value = "1";
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("OdbcType");
                            att.Value = nodeDataBaseSys.Attributes["OdbcType"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("MaxCount");
                            att.Value = nodeDataBaseSys.Attributes["MaxCount"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("TimeOut");
                            att.Value = nodeDataBaseSys.Attributes["TimeOut"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("Master");
                            att.Value = split ? "1" : "0";
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("Encrypt");
                            att.Value = nodeDataBaseSys.Attributes["Encrypt"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("Password");
                            att.Value = Srvtools.CliUtils.EncodePassword(userid, connnectionPassword);
                            node.Attributes.Append(att);
                            nodeDataBase.AppendChild(node);

                            XmlNode nodeSystemDB = nodeInfolight.SelectSingleNode("SystemDB");
                            if (nodeSystemDB == null)
                            {
                                nodeSystemDB = xml.CreateElement("SystemDB");
                                nodeInfolight.AppendChild(nodeSystemDB);
                            }
                            xml.Save(filepath);

                            ret = databasename;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                return new object[] { 1, ret };
            }
            finally
            {
                if (connection != null)
                    ReleaseConnection(dbAlias, connection);
            }


            return new object[] { 0, ret };

        }
        public object DeleteDB(object[] paramters)
        {
            string userid = paramters[0] as string;
            string aliasName = paramters[1] as string;
            string dbName = paramters[2] as string;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string dbAlias = string.Empty;
            string ret = string.Empty;
            try
            {
                dbAlias = DbConnectionSet.SystemDatabase;
                connection = AllocateConnection(dbAlias);
                transaction = connection.BeginTransaction();
                string strSql = "delete sys_sdalias where userid ='" + userid + "' and aliasname ='" + aliasName + "'";
                this.ExecuteSql(strSql, connection, transaction);
                strSql = "Select * from SYS_SDSOLUTIONS where userid ='" + userid + "'";
                DataSet ds = this.ExecuteSql(strSql, connection, transaction);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string solutionID = dr["SolutionID"].ToString();
                    bool ismodify = false;
                    string sAliasOptions = dr["AliasOptions"].ToString();
                    string[] arraysAliasOptions = sAliasOptions.Split(',');
                    List<string> snew = new List<string>();
                    foreach (string sold in arraysAliasOptions)
                    {
                        if (!sold.Equals(aliasName))
                            snew.Add(sold);
                        else ismodify = true;
                    }
                    if (ismodify)
                    {
                        string newstring = "";
                        foreach (string s in snew)
                        {
                            if (newstring != "")
                                newstring += ",";
                            newstring += s;
                        }
                        strSql = "update SYS_SDSOLUTIONS set AliasOptions='" + newstring + "' where userid = '" + userid + "' and SolutionID ='" + solutionID + "'";
                        this.ExecuteSql(strSql, connection, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                transaction.Rollback();
                return new object[] { 1, ret };
            }
            finally
            {
                transaction.Dispose();
                ReleaseConnection(dbAlias, connection);
            }
            try
            {
                connection = AllocateConnection(dbAlias);
                if (dbName != "ExistDB")
                {
                    string strSql = @"use master;ALTER DATABASE " + dbName + " SET OFFLINE WITH ROLLBACK IMMEDIATE;ALTER DATABASE " + dbName + " SET ONLINE;drop database " + dbName;
                    this.ExecuteCommand(strSql, connection, null);
                }

                string filepath = Path.Combine(EEPRegistry.Server, "SDModule", userid, "DB.xml");

                XmlDocument xml = new XmlDocument();

                if (File.Exists(filepath))
                {
                    xml.Load(filepath);
                }
                XmlNode nodeInfolight = xml.SelectSingleNode("InfolightDB");
                if (nodeInfolight == null)
                {
                    nodeInfolight = xml.CreateElement("InfolightDB");
                    xml.AppendChild(nodeInfolight);
                }
                XmlNode nodeDataBase = nodeInfolight.SelectSingleNode("DataBase");
                if (nodeDataBase == null)
                {
                    nodeDataBase = xml.CreateElement("DataBase");
                    nodeInfolight.AppendChild(nodeDataBase);
                }

                XmlNode nodeDB = nodeDataBase.SelectSingleNode(aliasName);
                if (nodeDB != null)
                {
                    nodeDataBase.RemoveChild(nodeDB);
                }

                XmlNode nodeSystemDB = nodeInfolight.SelectSingleNode("SystemDB");
                if (nodeSystemDB != null && nodeSystemDB.InnerText == aliasName)
                {
                    nodeSystemDB.InnerText = "";
                }

                xml.Save(filepath);
                ret = aliasName;
            }

            catch (Exception ex)
            {
                ret = ex.Message;
                return new object[] { 1, ret };
            }
            finally
            {
                ReleaseConnection(dbAlias, connection);
            }

            return new object[] { 0, ret };
        }
        public object CreateSystemTable(object[] paramters)
        {
            string databasename = paramters[0].ToString();
            string dbAlias = paramters[1].ToString();
            String type = paramters[4].ToString();
            bool exist = false;
            //当传入的参数是3个时，表示是给已经存在的连接字符串数据库添加系统表,第3个参数传入的是连接字符串
            if (paramters.Length > 2)
                exist = true;
            IDbConnection connection = null;
            IDbCommand command = null;
            string result = string.Empty;
            try
            {
                if (type == "1")
                {
#if Oracle2
                    System.Data.OracleClient.OracleConnection.ClearAllPools();
                    if (exist)
                    {
                        string connectionString = paramters[2].ToString();
                        string password = paramters[3].ToString();
                        if (password != "")
                            connectionString = connectionString + ";Password=" + password;

                        connection = new System.Data.OracleClient.OracleConnection(connectionString);
                    }
                    else
                        connection = AllocateConnection(dbAlias);
                    if (connection == null)
                    {
                        return new object[] { 1, "Connection is null" };
                    }
                    command = connection.CreateCommand();
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    result = CreateSqlServerSystemTable(command);
#endif
                }
                else
                {
                    SqlConnection.ClearAllPools();
                    if (exist)
                    {
                        string connectionString = paramters[2].ToString();
                        string password = paramters[3].ToString();
                        if (password != "")
                            connectionString = connectionString + ";Password=" + password;

                        connection = new SqlConnection(connectionString);
                    }
                    else
                        connection = AllocateConnection(dbAlias);
                    if (connection == null)
                    {
                        return new object[] { 1, "Connection is null" };
                    }
                    command = connection.CreateCommand();
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    result = CreateSqlServerSystemTable(command);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return new object[] { 1, result };
            }
            finally
            {
                if (exist)
                    connection.Dispose();
                else
                    ReleaseConnection(dbAlias, connection);
            }
            return new object[] { 0, result };

        }

        private String CreateSqlServerSystemTable(IDbCommand command)
        {
            string result = string.Empty;

            #region sql string
            command.CommandText = "CREATE TABLE COLDEF"
                                                 + "("
                                                 + "TABLE_NAME nvarchar(20) NOT NULL, "
                                                 + "FIELD_NAME nvarchar(20) NOT NULL, "
                                                 + "SEQ NUMERIC(12,0) NULL, "
                                                 + "FIELD_TYPE nvarchar(20) NULL, "
                                                 + "IS_KEY nvarchar(1) NOT NULL, "
                                                 + "FIELD_LENGTH NUMERIC(12,0) NULL, "
                                                 + "CAPTION nvarchar(40) NULL, "
                                                 + "EDITMASK nvarchar(10) NULL, "
                                                 + "NEEDBOX nvarchar(13) NULL, "
                                                 + "CANREPORT nvarchar(1) NULL, "
                                                 + "EXT_MENUID nvarchar(20) NULL, "
                                                 + "FIELD_SCALE NUMERIC(12,0) NULL, "
                                                 + "DD_NAME nvarchar(40) NULL, "
                                                 + "DEFAULT_VALUE nvarchar(100) NULL, "
                                                 + "CHECK_NULL nvarchar(1) NULL, "
                                                 + "QUERYMODE nvarchar(20) NULL, "
                                                 + "CAPTION1 nvarchar(40) NULL, "
                                                 + "CAPTION2 nvarchar(40) NULL, "
                                                 + "CAPTION3 nvarchar(40) NULL, "
                                                 + "CAPTION4 nvarchar(40) NULL, "
                                                 + "CAPTION5 nvarchar(40) NULL, "
                                                 + "CAPTION6 nvarchar(40) NULL, "
                                                 + "CAPTION7 nvarchar(40) NULL, "
                                                 + "CAPTION8 nvarchar(40) NULL, "
                                                 + "PRIMARY KEY(TABLE_NAME,FIELD_NAME)"
                                                 + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create COLDEF table.\n\r";
            }

            command.CommandText = "CREATE INDEX TABLENAME ON COLDEF (TABLE_NAME,FIELD_NAME)";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create INDEX TABLENAME on table COLDEF .\n\r";
            }

            command.CommandText = "CREATE TABLE SYSAUTONUM "
                                    + "("
                                    + "AUTOID VARCHAR(20) NOT NULL, "
                                    + "FIXED VARCHAR(20) NOT NULL, "
                                    + "CURRNUM NUMERIC(10,0) NULL, "
                                    + "DESCRIPTION VARCHAR(50) NULL ,"
                                    + "PRIMARY KEY (AUTOID,FIXED)"
                                    + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYSAUTONUM table.\n\r";
            }
            command.CommandText = "CREATE TABLE GROUPMENUS ("
                + "GROUPID varchar (20) NOT NULL ,"
                + "MENUID nvarchar (30) NOT NULL, "
                + "PRIMARY KEY(GROUPID,MENUID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create GROUPMENUS table.\n\r";
            }

            // Create USERMENUS
            command.CommandText = "CREATE TABLE USERMENUS ("
                + "USERID varchar (20) NOT NULL ,"
                + "MENUID nvarchar (30) NOT NULL, "
                + "PRIMARY KEY(USERID,MENUID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create USERMENUS table.\n\r";
            }

            // Create GROUPS
            command.CommandText = "CREATE TABLE GROUPS ("
                + "GROUPID varchar (20) NOT NULL ,"
                + "GROUPNAME nvarchar (50) NULL ,"
                + "DESCRIPTION nvarchar (100) NULL ,"
                + "MSAD nvarchar (1) NULL, "
                + "PRIMARY KEY(GROUPID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('00', 'EveryOne', 'N')";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('01', 'DEPARTMENT1', 'N')";
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create GROUPS table.\n\r";
            }

            // Create MENUITEMTYPE
            command.CommandText = "CREATE TABLE MENUITEMTYPE ("
                + "ITEMTYPE nvarchar (20) NOT NULL ,"
                + "ITEMNAME nvarchar (20) NULL, "
                + "DBALIAS nvarchar (50) NULL, "
                + "PRIMARY KEY(ITEMTYPE)"
                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create MENUITEMTYPE table.\n\r";
            }

            // Create MENUTABLE
            command.CommandText = "CREATE TABLE MENUTABLE ("
                + "MENUID nvarchar (30) NOT NULL ,"
                + "CAPTION nvarchar (50) NOT NULL ,"
                + "PARENT nvarchar (20) NULL ,"
                + "PACKAGE nvarchar (60) NULL ,"
                + "MODULETYPE nvarchar (1) NULL ,"
                + "ITEMPARAM nvarchar (200) NULL ,"
                + "FORM nvarchar (32) NULL ,"
                + "ISSHOWMODAL nvarchar (1) NULL ,"
                + "ITEMTYPE nvarchar (20) NULL ,"
                + "SEQ_NO nvarchar (4) NULL,"
                + "PACKAGEDATE DateTime,"
                + "[IMAGE] image NULL,"
                + "OWNER nvarchar(20),"
                + "ISSERVER nvarchar(1),"
                + "VERSIONNO nvarchar(20),"
                + "CHECKOUT nvarchar(20),"
                + "CHECKOUTDATE datetime,"
                + "CAPTION0 nvarchar(50) NULL,"
                + "CAPTION1 nvarchar(50) NULL,"
                + "CAPTION2 nvarchar(50) NULL,"
                + "CAPTION3 nvarchar(50) NULL,"
                + "CAPTION4 nvarchar(50) NULL,"
                + "CAPTION5 nvarchar(50) NULL,"
                + "CAPTION6 nvarchar(50) NULL,"
                + "CAPTION7 nvarchar(50) NULL,"
                + "IMAGEURL nvarchar(100) NULL, "
                + "PRIMARY KEY(MENUID)"
                + ") ";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create MENUTABLE table.\n\r";
            }

            // Create USERGROUPS
            command.CommandText = "CREATE TABLE USERGROUPS ("
                + "USERID varchar (20) NOT NULL ,"
                + "GROUPID varchar (20) NOT NULL, "
                + "PRIMARY KEY(USERID,GROUPID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO USERGROUPS(USERID, GROUPID) VALUES('001', '01')";
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create USERGROUPS table.\n\r";
            }

            // Create USERS
            command.CommandText = "CREATE TABLE USERS ("
                + "USERID varchar (20) NOT NULL ,"
                + "USERNAME nvarchar (30) NULL ,"
                + "AGENT nvarchar (20) NULL ,"
                + "PWD nvarchar (10) NULL ,"
                + "CREATEDATE nvarchar (8) NULL ,"
                + "CREATER nvarchar (20) NULL ,"
                + "DESCRIPTION nvarchar (100) NULL ,"
                + "EMAIL nvarchar (40) NULL ,"
                + "LASTTIME nvarchar (8) NULL ,"
                + "AUTOLOGIN nvarchar (1) NULL,"
                + "LASTDATE nvarchar (8) NULL ,"
                + "SIGNATURE nvarchar (30) NULL ,"
                + "MSAD nvarchar (1) NULL, "
                + "PRIMARY KEY(USERID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO USERS(USERID, USERNAME, PWD, MSAD, AUTOLOGIN) VALUES('001', 'TEST', '', 'N','S')";
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create USERS table.\n\r";
            }

            #region 暂时不要的
            //// Create MENUTABLELOG
            //command.CommandText = "CREATE TABLE MENUTABLELOG"
            //    + "("
            //    + "LOGID INT IDENTITY(1,1),"
            //    + "MENUID nvarchar(30) not null,"
            //    + "PACKAGE nvarchar(20) not null,"
            //    + "PACKAGEDATE DATETIME,"
            //    + "LASTDATE DATETIME,"
            //    + "OWNER nvarchar(20),"
            //    + "OLDVERSION IMAGE,"
            //    + "OLDDATE nvarchar(20), "
            //    + "PRIMARY KEY(LOGID)"
            //    + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create MENUTABLELOG table.\n\r";
            //}

            //// Create MENUCHECKLOG
            //command.CommandText = "CREATE TABLE MENUCHECKLOG"
            //    + "("
            //    + "LOGID int Identity(1, 1),"
            //    + "ITEMTYPE nvarchar(20) not null,"
            //    + "PACKAGE nvarchar(50) not null,"
            //    + "PACKAGEDATE DateTime,"
            //    + "FILETYPE nvarchar(10),"
            //    + "FILENAME nvarchar(60),"
            //    + "FILEDATE DateTime,"
            //    + "FILECONTENT IMAGE, "
            //    + "PRIMARY KEY(LOGID)"
            //    + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create MENUCHECKLOG table.\n\r";
            //}
            #endregion

            // Create SYSEEPLOG
            command.CommandText = "CREATE TABLE SYSEEPLOG"
                + "("
                + "CONNID nvarchar(20) NOT NULL,"
                + "LOGID INT IDENTITY(1,1) NOT NULL,"
                + "LOGSTYLE nvarchar(1) NOT NULL,"
                + "LOGDATETIME DATETIME NOT NULL,"
                + "DOMAINID nvarchar(30) NULL,"
                + "USERID varchar(20) NULL,"
                + "LOGTYPE nvarchar(1) NULL,"
                + "TITLE nvarchar(64) NULL,"
                + "DESCRIPTION text NULL,"
                + "COMPUTERIP nvarchar(16) NULL,"
                + "COMPUTERNAME nvarchar(64) NULL,"
                + "EXECUTIONTIME INT NULL, "
                + "PRIMARY KEY(CONNID,LOGID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYSEEPLOG table.\n\r";
            }

            // Create SYSERRLOG
            command.CommandText = "CREATE TABLE SYSERRLOG"
                + "("
                + "ERRID int identity(1, 1),"
                + "USERID varchar(20), "
                + "MODULENAME nvarchar(30),"
                + "ERRMESSAGE nvarchar(255),"
                + "ERRSTACK text,"
                + "ERRDESCRIP nvarchar(255),"
                + "ERRDATE DateTime,"
                + "ERRSCREEN Image,"
                + "OWNER nvarchar(20),"
                + "PROCESSDATE DateTime,"
                + "PRODESCRIP nvarchar(255),"
                + "STATUS nvarchar(2), "
                + "PRIMARY KEY(ERRID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYSERRLOG table.\n\r";
            }

            #region 暂时不要的
            //command.CommandText = "CREATE TABLE SYS_LANGUAGE"
            //    + "("
            //    + "ID int IDENTITY(1, 1) NOT NULL,"
            //    + "IDENTIFICATION nvarchar(80),"
            //    + "KEYS nvarchar(80),"
            //    + "EN nvarchar(80),"
            //    + "CHT nvarchar(80),"
            //    + "CHS nvarchar(80),"
            //    + "HK nvarchar(80),"
            //    + "JA nvarchar(80),"
            //    + "KO nvarchar(80),"
            //    + "LAN1 nvarchar(80),"
            //    + "LAN2 nvarchar(80),"
            //    + " CONSTRAINT [PK_SYS_LANGUAGE] PRIMARY KEY CLUSTERED"
            //    + "("
            //    + "[ID] "
            //    + ")  ON [PRIMARY]"
            //    + ")  ON [PRIMARY]";

            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_LANGUAGE table.\n\r";
            //}

            //command.CommandText = "CREATE TABLE SYS_MESSENGER"
            //    + "("
            //    + "USERID varchar(20) NOT NULL,"
            //    + "MESSAGE nvarchar(255),"
            //    + "PARAS nvarchar(255),"
            //    + "SENDTIME nvarchar(14),"
            //    + "SENDERID nvarchar(20),"
            //    + "RECTIME nvarchar(14),"
            //    + "STATUS char(1)"
            //    + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_MESSENGER table.\n\r";
            //}

            ////CreateMenuTableControl
            //command.CommandText = "CREATE TABLE MENUTABLECONTROL"
            //                        + "("
            //                        + "MENUID varchar (30) NOT NULL, "
            //                        + "CONTROLNAME Varchar (50) NOT NULL, "
            //                        + "DESCRIPTION Varchar (50) NULL, "
            //                        + "TYPE Varchar (20) NULL, "
            //                        + "PRIMARY KEY(MENUID,CONTROLNAME)"
            //                        + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create MENUTABLECONTROL table.\n\r";
            //}

            ////CreateGroupMenuControl
            //command.CommandText = "CREATE TABLE GROUPMENUCONTROL"
            //                        + "("
            //                        + "GROUPID Varchar (20) NOT NULL, "
            //                        + "MENUID Varchar (30) NOT NULL, "
            //                        + "CONTROLNAME Varchar (50) NOT NULL, "
            //                        + "TYPE Varchar (20) NULL, "
            //                        + "ENABLED CHAR (1) NULL, "
            //                        + "VISIBLE CHAR (1) NULL, "
            //                        + "ALLOWADD CHAR (1) NULL, "
            //                        + "ALLOWUPDATE CHAR (1) NULL, "
            //                        + "ALLOWDELETE CHAR (1) NULL, "
            //                        + "ALLOWPRINT CHAR (1) NULL, "
            //                        + "PRIMARY KEY(GROUPID,MENUID,CONTROLNAME)"
            //                        + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create GROUPMENUCONTROL table.\n\r";
            //}

            ////CreateUserMenuControl
            //command.CommandText = "CREATE TABLE USERMENUCONTROL"
            //                        + "("
            //                        + "USERID Varchar (20) NOT NULL, "
            //                        + "MENUID Varchar (30) NOT NULL, "
            //                        + "CONTROLNAME Varchar (50) NOT NULL, "
            //                        + "TYPE Varchar (20) NULL, "
            //                        + "ENABLED CHAR (1) NULL, "
            //                        + "VISIBLE CHAR (1) NULL, "
            //                        + "ALLOWADD CHAR (1) NULL, "
            //                        + "ALLOWUPDATE CHAR (1) NULL, "
            //                        + "ALLOWDELETE CHAR (1) NULL, "
            //                        + "ALLOWPRINT CHAR (1) NULL, "
            //                        + "PRIMARY KEY(USERID,MENUID,CONTROLNAME)"
            //                        + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create USERMENUCONTROL table.\n\r";
            //}
            #endregion

            command.CommandText = "CREATE TABLE SYS_REFVAL"
                                + "("
                                + "REFVAL_NO Varchar(30) Not NULL, "
                                + "DESCRIPTION Varchar(250), "
                                + "TABLE_NAME Varchar(30), "
                                + "CAPTION Varchar(30), "
                                + "DISPLAY_MEMBER Varchar(30), "
                                + "SELECT_ALIAS Varchar(250), "
                                + "SELECT_COMMAND Varchar(250), "
                                + "VALUE_MEMBER Varchar(30), "
                                + "CONSTRAINT PK_SYS_REFVAL PRIMARY KEY(REFVAL_NO) "
                                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYS_REFVAL table.\n\r";
            }

            command.CommandText = "CREATE TABLE SYS_REFVAL_D1"
                                + "("
                                + "REFVAL_NO Varchar(30) Not NULL, "
                                + "FIELD_NAME Varchar(30) Not NULL, "
                                + "HEADER_TEXT Varchar(20), "
                                + "WIDTH INT, "
                                + "CONSTRAINT PK_SYS_REFVAL_D1 PRIMARY KEY(REFVAL_NO, FIELD_NAME) "
                                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYS_REFVAL_D1 table.\n\r";
            }

            command.CommandText = "CREATE TABLE MENUFAVOR"
                                + "("
                                + "MENUID nVarchar(30) Not NULL, "
                                + "CAPTION nVarchar(50) Not NULL, "
                                + "USERID Varchar(20), "
                                + "ITEMTYPE nVarchar(20), "
                                + "GROUPNAME nVarChar(20), "
                                + "PRIMARY Key (MENUID,USERID)"
                                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create MENUFAVOR table.\n\r";
            }

            #region 暂时不要的
            //command.CommandText = "CREATE TABLE SYS_ANYQUERY"
            //                    + "("
            //                    + "QUERYID Varchar(20) Not NULL, "
            //                    + "USERID Varchar(20) Not NULL, "
            //                    + "TEMPLATEID Varchar(20), "
            //                    + "TABLENAME Varchar(50), "
            //                    + "LASTDATE datetime, "
            //                    + "CONTENT text, "
            //                    + "PRIMARY Key (QUERYID,USERID,TEMPLATEID)"
            //                    + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_ANYQUERY table.\n\r";
            //}
            #endregion

            command.CommandText = "CREATE TABLE SYS_REPORT"
                                + "("
                                + "REPORTID nVarchar(50) Not NULL, "
                                + "FILENAME nVarchar(50) Not NULL, "
                                + "REPORTNAME nVarchar(50), "
                                + "DESCRIPTION nVarchar(50), "
                                + "FILEPATH nVarchar(50), "
                                + "OUTPUTMODE nVarchar(20), "
                                + "HEADERREPEAT nVarchar(5), "
                                + "HEADERFONT image, "
                                + "HEADERITEMS image, "
                                + "FOOTERFONT image, "
                                + "FOOTERITEMS image, "
                                + "FIELDFONT image, "
                                + "FIELDITEMS image, "
                                + "SETTING image, "
                                + "FORMAT image, "
                                + "PARAMETERS image, "
                                + "IMAGES image, "
                                + "MAILSETTING image, "
                                + "DATASOURCE_PROVIDER nVarchar(50),"
                                + "DATASOURCES image,"
                                + "CLIENT_QUERY image,"
                                + "REPORT_TYPE nVarchar(1),"
                                + "TEMPLATE_DESC nVarchar(50),"
                                + "PRIMARY Key (REPORTID,FILENAME)"
                                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYS_REPORT table.\n\r";
            }

            command.CommandText = "CREATE TABLE SYS_PERSONAL"
                               + "("
                               + "FORMNAME NVARCHAR(60) NOT NULL,"
                               + "COMPNAME NVARCHAR(30) NOT NULL,"
                               + "USERID NVARCHAR(20) NOT NULL,"
                               + "REMARK NVARCHAR(30),"
                               + "PROPCONTENT NTEXT,"
                               + "CREATEDATE DATETIME,"
                               + "PRIMARY KEY (FORMNAME,COMPNAME,USERID)"
                               + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYS_PERSONAL table.\n\r";
            }
            #endregion

            #region WorkFlow
            //GROUPS
            command.CommandText = "ALTER TABLE GROUPS ADD ISROLE CHAR(1) NULL";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not add ISROLE column to GROUPS table.\n\r";
            }

            #region 暂时不要的
            //// SYS_ORG
            //command.CommandText = "CREATE TABLE [SYS_ORG](" +
            //                    "[ORG_NO] [nvarchar](8) NOT NULL," +
            //                    "[ORG_DESC] [nvarchar](40) NOT NULL," +
            //                    "[ORG_KIND] [nvarchar](4) NOT NULL," +
            //                    "[UPPER_ORG] [nvarchar](8) NULL," +
            //                    "[ORG_MAN] [nvarchar](20) NOT NULL," +
            //                    "[LEVEL_NO] [nvarchar](6) NOT NULL," +
            //                    "[ORG_TREE] [nvarchar](40) NULL," +
            //                    "[END_ORG] [nvarchar](4) NULL," +
            //                    "[ORG_FULLNAME] [nvarchar](254) NULL," +
            //                    "PRIMARY KEY(ORG_NO)" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();

            //    command.CommandText = "INSERT INTO SYS_ORG(ORG_NO,ORG_DESC,ORG_KIND,ORG_MAN,LEVEL_NO) Values ('1',N'總公司','0','001','9')";
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_ORG table.\n\r";
            //}

            //// SYS_ORGKIND
            //command.CommandText = "CREATE TABLE [SYS_ORGKIND](" +
            //                    "[ORG_KIND] [nvarchar](4) NOT NULL," +
            //                    "[KIND_DESC] [nvarchar](40) NOT NULL," +
            //                    "PRIMARY KEY(ORG_KIND)" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();

            //    command.CommandText = "INSERT INTO SYS_ORGKIND(ORG_KIND,KIND_DESC) Values ('0',N'公司組織')";
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_ORGKIND table.\n\r";
            //}

            //// SYS_ORGLEVEL
            //command.CommandText = "CREATE TABLE [SYS_ORGLEVEL](" +
            //                    "[LEVEL_NO] [nvarchar](6) NOT NULL," +
            //                    "[LEVEL_DESC] [nvarchar](40) NOT NULL," +
            //                    "PRIMARY KEY(LEVEL_NO)" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0',N'直屬主管')";
            //    command.ExecuteNonQuery();
            //    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('1',N'主任/課長/副理')";
            //    command.ExecuteNonQuery();
            //    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('2',N'經理')";
            //    command.ExecuteNonQuery();
            //    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('3',N'副總')";
            //    command.ExecuteNonQuery();
            //    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('9',N'總經理')";
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_ORGLEVEL table.\n\r";
            //}

            //// SYS_ORGROLES
            //command.CommandText = "CREATE TABLE [SYS_ORGROLES](" +
            //                    "[ORG_NO] [nvarchar](8) NOT NULL," +
            //                    "[ROLE_ID] [varchar](20) NOT NULL," +
            //                    "[ORG_KIND] [nvarchar](4) NULL," +
            //                    "PRIMARY KEY(ORG_NO,ROLE_ID)" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_ORGROLES table.\n\r";
            //}

            //command.CommandText = "CREATE INDEX ORGNO ON SYS_ORGROLES (ORG_NO, ROLE_ID)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create INDEX ORGNO on table SYS_ORGROLES .\n\r";
            //}

            //// SYS_ROLES_AGENT
            //command.CommandText = "CREATE TABLE [SYS_ROLES_AGENT](" +
            //                    "[ROLE_ID] [varchar](20) NOT NULL," +
            //                    "[AGENT] [nvarchar](20) NOT NULL," +
            //                    "[FLOW_DESC] [nvarchar](40) NOT NULL," +
            //                    "[START_DATE] [nvarchar](8) NOT NULL," +
            //                    "[START_TIME] [nvarchar](6) NULL," +
            //                    "[END_DATE] [nvarchar](8) NOT NULL," +
            //                    "[END_TIME] [nvarchar](6) NULL," +
            //                    "[PAR_AGENT] [nvarchar](4) NOT NULL," +
            //                    "[REMARK] [nvarchar](254) NULL," +
            //                    "PRIMARY KEY(ROLE_ID,AGENT)" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_ROLES_AGENT table.\n\r";
            //}

            //command.CommandText = "CREATE INDEX ROLEID ON SYS_ROLES_AGENT (ROLE_ID)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create INDEX ROLEID on table SYS_ROLES_AGENT .\n\r";
            //}

            //// SYS_TODOHIS
            //command.CommandText = "CREATE TABLE [SYS_TODOHIS](" +
            //                    "[LISTID] [nvarchar](40) NOT NULL," +
            //                    "[FLOW_ID] [nvarchar](40) NOT NULL," +
            //                    "[FLOW_DESC] [nvarchar](40) NULL," +
            //                    "[ROLE_ID] [varchar](20) NOT NULL," +
            //                    "[S_ROLE_ID] [varchar](20) NOT NULL," +
            //                    "[S_STEP_ID] [nvarchar](20) NOT NULL," +
            //                    "[D_STEP_ID] [nvarchar](20) NOT NULL," +
            //                    "[S_STEP_DESC] [nvarchar](64) NULL," +
            //                    "[S_USER_ID] [nvarchar](20) NOT NULL," +
            //                    "[USER_ID] [nvarchar](20) NOT NULL," +
            //                    "[USERNAME] [nvarchar](30) NULL," +
            //                    "[FORM_NAME] [nvarchar](30) NULL," +
            //                    "[WEBFORM_NAME] [nvarchar](50) NOT NULL," +
            //                    "[S_USERNAME] [nvarchar](30) NULL," +
            //                    "[NAVIGATOR_MODE] [nvarchar](2) NOT NULL," +
            //                    "[FLNAVIGATOR_MODE] [nvarchar](2) NOT NULL," +
            //                    "[PARAMETERS] [nvarchar](254) NULL," +
            //                    "[STATUS] [nvarchar](4) NULL," +
            //                    "[PROC_TIME] [decimal](8, 2) NOT NULL," +
            //                    "[EXP_TIME] [decimal](8, 2) NOT NULL," +
            //                    "[TIME_UNIT] [nvarchar](4) NOT NULL," +
            //                    "[FLOWIMPORTANT] [varchar](1) NOT NULL," +
            //                    "[FLOWURGENT] [varchar](1) NOT NULL," +
            //                    "[FORM_TABLE] [nvarchar](30) NULL," +
            //                    "[FORM_KEYS] [nvarchar](254) NULL," +
            //                    "[FORM_PRESENTATION] [nvarchar](254) NULL," +
            //                    "[REMARK] [nvarchar](254) NULL," +
            //                    "[VERSION] [nvarchar](2) NULL," +
            //                    "[VDSNAME] [nvarchar](40) NULL," +
            //                    "[SENDBACKSTEP] [nvarchar](2) NULL," +
            //                    "[LEVEL_NO] [nvarchar](6) NULL," +
            //                    "[UPDATE_DATE] [nvarchar](10) NULL," +
            //                    "[UPDATE_TIME] [nvarchar](8) NULL," +
            //                    "[FORM_PRESENT_CT] [nvarchar](254) NULL," +
            //                    "[ATTACHMENTS] [nvarchar](255) NULL, " +
            //                    "[CREATE_TIME] [nvarchar](50) NULL" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_TODOHIS table.\n\r";
            //}

            //command.CommandText = "CREATE INDEX LISTID ON SYS_TODOHIS (LISTID)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create INDEX LISTID on table SYS_TODOHIS.\n\r";
            //}

            //command.CommandText = "CREATE INDEX USERID ON SYS_TODOHIS (USER_ID)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create INDEX USERID on table SYS_TODOHIS.\n\r";
            //}

            //// SYS_TODOLIST
            //command.CommandText = "CREATE TABLE [SYS_TODOLIST](" +
            //                    "[LISTID] [nvarchar](40) NOT NULL," +
            //                    "[FLOW_ID] [nvarchar](40) NOT NULL," +
            //                    "[FLOW_DESC] [nvarchar](40) NULL," +
            //                    "[APPLICANT] [nvarchar](20) NOT NULL," +
            //                    "[S_USER_ID] [nvarchar](20) NOT NULL," +
            //                    "[S_STEP_ID] [nvarchar](20) NOT NULL," +
            //                    "[S_STEP_DESC] [nvarchar](64) NULL," +
            //                    "[D_STEP_ID] [nvarchar](20) NOT NULL," +
            //                    "[D_STEP_DESC] [nvarchar](64) NULL," +
            //                    "[EXP_TIME] [decimal](8, 2) NOT NULL," +
            //                    "[URGENT_TIME] [decimal](8, 2) NOT NULL," +
            //                    "[TIME_UNIT] [nvarchar](4) NOT NULL," +
            //                    "[USERNAME] [nvarchar](30) NULL," +
            //                    "[FORM_NAME] [nvarchar](30) NULL," +
            //                    "[NAVIGATOR_MODE] [nvarchar](2) NOT NULL," +
            //                    "[FLNAVIGATOR_MODE] [nvarchar](2) NOT NULL," +
            //                    "[PARAMETERS] [nvarchar](254) NULL," +
            //                    "[SENDTO_KIND] [nvarchar](4) NOT NULL," +
            //                    "[SENDTO_ID] [nvarchar](20) NOT NULL," +
            //                    "[FLOWIMPORTANT] [varchar](1) NOT NULL," +
            //                    "[FLOWURGENT] [nvarchar](1) NOT NULL," +
            //                    "[STATUS] [nvarchar](4) NULL," +
            //                    "[FORM_TABLE] [nvarchar](30) NULL," +
            //                    "[FORM_KEYS] [nvarchar](254) NULL," +
            //                    "[FORM_PRESENTATION] [nvarchar](254) NULL," +
            //                    "[FORM_PRESENT_CT] [nvarchar](254) NOT NULL," +
            //                    "[REMARK] [nvarchar](254) NULL," +
            //                    "[PROVIDER_NAME] [nvarchar](254) NULL," +
            //                    "[VERSION] [nvarchar](2) NULL," +
            //                    "[EMAIL_ADD] [nvarchar](40) NULL," +
            //                    "[EMAIL_STATUS] [varchar](1) NULL," +
            //                    "[VDSNAME] [nvarchar](40) NULL," +
            //                    "[SENDBACKSTEP] [nvarchar](2) NULL," +
            //                    "[LEVEL_NO] [nvarchar](6) NULL," +
            //                    "[WEBFORM_NAME] [nvarchar](50) NOT NULL," +
            //                    "[UPDATE_DATE] [nvarchar](10) NULL," +
            //                    "[UPDATE_TIME] [nvarchar](8) NULL," +
            //                    "[FLOWPATH] [nvarchar](100) NOT NULL," +
            //                    "[PLUSAPPROVE] [varchar](1) NOT NULL," +
            //                    "[PLUSROLES] [nvarchar](254) NOT NULL," +
            //                    "[MULTISTEPRETURN] [varchar](1) NULL," +
            //                    "[SENDTO_NAME] [nvarchar](30) NULL," +
            //                    "[ATTACHMENTS] [nvarchar](255) NULL, " +
            //                    "[CREATE_TIME] [nvarchar](50) NULL" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_TODOLIST table.\n\r";
            //}

            //command.CommandText = "CREATE INDEX LISTID ON SYS_TODOLIST (LISTID)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create INDEX LISTID on table SYS_TODOLIST.\n\r";
            //}

            //command.CommandText = "CREATE INDEX SENDTOID ON SYS_TODOLIST (SENDTO_ID, SENDTO_KIND)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create INDEX SENDTOID on table SYS_TODOLIST.\n\r";
            //}

            //command.CommandText = "CREATE INDEX FLOWDESC ON SYS_TODOLIST (FLOW_DESC)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create INDEX FLOWDESC on table SYS_TODOLIST.\n\r";
            //}

            //// SYS_FLDefinition
            //command.CommandText = "CREATE TABLE SYS_FLDEFINITION"
            //                        + "("
            //                        + "FLTYPEID nvarchar(50) NOT NULL, "
            //                        + "FLTYPENAME nvarchar(200) NOT NULL, "
            //                        + "FLDEFINITION ntext NOT NULL, "
            //                        + "VERSION int NULL, "
            //                        + "CONSTRAINT PK_SYS_FL PRIMARY KEY(FLTYPEID) "
            //                        + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_FLDEFINITION table.\n\r";
            //}

            //// SYS_FLInstanceState
            //command.CommandText = "CREATE TABLE SYS_FLINSTANCESTATE"
            //                        + "("
            //                        + "FLINSTANCEID nvarchar(50) NOT NULL, "
            //                        + "STATE image NOT NULL, "
            //                        + "STATUS int NULL, "
            //                        + "INFO nvarchar(200) NULL,"
            //                        + "PRIMARY KEY(FLINSTANCEID)"
            //                        + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_FLINSTANCESTATE table.\n\r";
            //}


            //// Sys_ExtApprove
            //command.CommandText = "CREATE TABLE SYS_EXTAPPROVE"
            //                        + "("
            //                        + "APPROVEID nvarchar(50) NULL, "
            //                        + "GROUPID nvarchar(50) NULL, "
            //                        + "MINIMUM nvarchar(50) NULL, "
            //                        + "MAXIMUM nvarchar(50) NULL,"
            //                        + "ROLEID nvarchar(50) NULL"
            //                        + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_EXTAPPROVE table.\n\r";
            //}
            #endregion
            #endregion

            #region Insert Proc
            command.CommandText =
@"CREATE proc spGenInsertSQL (@tablename varchar(256))
as
begin
declare @sql varchar(max)
declare @sqlValues varchar(max)
set @sql =' ('
set @sqlValues = 'values (''+'
select @sqlValues = @sqlValues + cols + ' + '','' + ' ,@sql = @sql + '[' + name + '],'
  from
      (select case
           when xtype in (48,52,56,59,60,62,104,106,108,122,127)      
                then 'case when '+ name +' is null then ''NULL'' else ' + 'cast('+ name + ' as varchar)'+' end'
           when xtype in (58,61)
                then 'case when '+ name +' is null then ''NULL'' else '+''''''''' + ' + 'cast('+ name +' as varchar)'+ '+'''''''''+' end'
           when xtype in (167)
                then 'case when '+ name +' is null then ''NULL'' else '+''''''''' + ' + 'replace('+ name+','''''''','''''''''''')' + '+'''''''''+' end'
           when xtype in (231)
                then 'case when '+ name +' is null then ''NULL'' else '+'''N'''''' + ' + 'replace('+ name+','''''''','''''''''''')' + '+'''''''''+' end'
           when xtype in (175)
                then 'case when '+ name +' is null then ''NULL'' else '+''''''''' + ' + 'cast(replace('+ name+','''''''','''''''''''') as Char(' + cast(length as varchar)  + '))+'''''''''+' end'
           when xtype in (239)
                then 'case when '+ name +' is null then ''NULL'' else '+'''N'''''' + ' + 'cast(replace('+ name+','''''''','''''''''''') as Char(' + cast(length as varchar)  + '))+'''''''''+' end'
           else '''NULL'''
         end as Cols,name
         from syscolumns
     where id = object_id(@tablename)
  ) T
set @sql ='select ''INSERT INTO ['+ @tablename + ']' + left(@sql,len(@sql)-1)+') ' + left(@sqlValues,len(@sqlValues)-4) + ')'' from '+@tablename
print @sql
exec (@sql)
end
";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not add spGenInsertSQL Proc.\n\r";
            }

            #endregion

            return result;
        }

        private String CreateOracleSystemTable(IDbCommand command)
        {
            string result = string.Empty;
            #region Simplified
            command.CommandText = "CREATE TABLE COLDEF"
                          + "("
                          + "TABLE_NAME varchar2(20) NOT NULL, "
                          + "FIELD_NAME varchar2(20) NOT NULL, "
                          + "SEQ NUMERIC(12,0) NULL, "
                          + "FIELD_TYPE varchar2(20) NULL, "
                          + "IS_KEY varchar2(1) NOT NULL, "
                          + "FIELD_LENGTH NUMERIC(12,0) NULL, "
                          + "CAPTION varchar2(40) NULL, "
                          + "EDITMASK varchar2(10) NULL, "
                          + "NEEDBOX varchar2(13) NULL, "
                          + "CANREPORT varchar2(1) NULL, "
                          + "EXT_MENUID varchar2(20) NULL, "
                          + "FIELD_SCALE NUMERIC(12,0) NULL, "
                          + "DD_NAME varchar2(40) NULL, "
                          + "DEFAULT_VALUE varchar2(100) NULL, "
                          + "CHECK_NULL varchar2(1) NULL, "
                          + "QUERYMODE varchar2(20) NULL, "
                          + "CAPTION1 varchar2(40) NULL, "
                          + "CAPTION2 varchar2(40) NULL, "
                          + "CAPTION3 varchar2(40) NULL, "
                          + "CAPTION4 varchar2(40) NULL, "
                          + "CAPTION5 varchar2(40) NULL, "
                          + "CAPTION6 varchar2(40) NULL, "
                          + "CAPTION7 varchar2(40) NULL, "
                          + "CAPTION8 varchar2(40) NULL, "
                          + "PRIMARY KEY(TABLE_NAME,FIELD_NAME)"
                          + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create COLDEF table.\n\r";
            }

            command.CommandText = "CREATE INDEX TABLENAME ON COLDEF(TABLE_NAME,FIELD_NAME)";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                //result += "Can not create INDEX TABLENAME on table COLDEF .\n\r";
            }

            command.CommandText = "CREATE TABLE SYSAUTONUM "
                                    + "("
                                    + "AUTOID VARCHAR2(20) NOT NULL, "
                                    + "FIXED VARCHAR2(20) NOT NULL, "
                                    + "CURRNUM NUMERIC(10,0) NULL, "
                                    + "DESCRIPTION VARCHAR2(50) NULL, "
                                    + "PRIMARY KEY (AUTOID, FIXED)"
                                    + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYSAUTONUM table.\n\r";
            }

            //// Create GROUPFORMS
            //command.CommandText = "CREATE TABLE GROUPFORMS ("
            //    + "GROUPID nvarchar (20) NOT NULL ,"
            //    + "PACKAGE_NAME nvarchar (50) NOT NULL ,"
            //    + "FORM_NAME nvarchar (50) NULL ,"
            //    + "PARENT_MENU nvarchar (50) NULL "
            //    + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create GROUPFORMS table.\n\r";
            //}

            // Create GROUPMENUS
            command.CommandText = "CREATE TABLE GROUPMENUS ("
                + "GROUPID varchar2 (20) NOT NULL ,"
                + "MENUID varchar2 (30) NOT NULL, "
                + "PRIMARY KEY(GROUPID,MENUID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create GROUPMENUS table.\n\r";
            }

            // Create USERMENUS
            command.CommandText = "CREATE TABLE USERMENUS ("
                + "USERID varchar2 (20) NOT NULL ,"
                + "MENUID varchar2 (30) NOT NULL, "
                + "PRIMARY KEY(USERID,MENUID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create USERMENUS table.\n\r";
            }

            // Create GROUPS
            command.CommandText = "CREATE TABLE GROUPS ("
                + "GROUPID varchar2 (20) NOT NULL ,"
                + "GROUPNAME varchar2 (50) NULL ,"
                + "DESCRIPTION varchar2 (100) NULL ,"
                + "MSAD varchar2 (1) NULL, "
                + "PRIMARY KEY(GROUPID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('00', 'EveryOne', 'N')";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO GROUPS(GROUPID, GROUPNAME, MSAD) VALUES('01', 'DEPARTMENT1', 'N')";
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create GROUPS table.\n\r";
            }

            // Create MENUITEMTYPE
            command.CommandText = "CREATE TABLE MENUITEMTYPE ("
                + "ITEMTYPE varchar2 (20) NOT NULL ,"
                + "ITEMNAME varchar2 (20) NULL, "
                + "DBALIAS varchar2(50) NULL, "
                + "PRIMARY KEY(ITEMTYPE)"
                + ")";
            try
            {
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO MENUITEMTYPE(ITEMTYPE, ITEMNAME) VALUES('SOLUTION1', 'DEFAULT SOLUTION')";
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create MENUITEMTYPE table.\n\r";
            }

            // Create MENUTABLE
            command.CommandText = "CREATE TABLE MENUTABLE ("
                + "MENUID nvarchar2 (30) NOT NULL ,"
                + "CAPTION nvarchar2 (50) NOT NULL ,"
                + "PARENT nvarchar2 (20) NULL ,"
                + "PACKAGE nvarchar2 (60) NULL ,"
                + "MODULETYPE nvarchar2 (1) NULL ,"
                + "ITEMPARAM nvarchar2 (200) NULL ,"
                + "FORM nvarchar2 (32) NULL ,"
                + "ISSHOWMODAL nvarchar2 (1) NULL ,"
                + "ITEMTYPE nvarchar2 (20) NULL ,"
                + "SEQ_NO nvarchar2 (4) NULL,"
                + "PACKAGEDATE Date,"
                + "ISSERVER nvarchar2(1),"
                + "VERSIONNO nvarchar2(20),"
                + "CHECKOUT nvarchar2(20),"
                + "CHECKOUTDATE date,"
                + "CAPTION0 nvarchar2(50) NULL,"
                + "CAPTION1 nvarchar2(50) NULL,"
                + "CAPTION2 nvarchar2(50) NULL,"
                + "CAPTION3 nvarchar2(50) NULL,"
                + "CAPTION4 nvarchar2(50) NULL,"
                + "CAPTION5 nvarchar2(50) NULL,"
                + "CAPTION6 nvarchar2(50) NULL,"
                + "CAPTION7 nvarchar2(50) NULL,"
                + "IMAGE blob NULL,"
                + "IMAGEURL varchar2(100) NULL, "
                + "OWNER varchar2(20),"
                + "PRIMARY KEY(MENUID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO MENUTABLE(MENUID, CAPTION, ITEMTYPE, MODULETYPE) VALUES('0', 'ROOT', 'SOLUTION1', 'F')";
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create MENUTABLE table.\n\r";
            }

            // Create USERGROUPS
            command.CommandText = "CREATE TABLE USERGROUPS ("
                + "USERID varchar2 (20) NOT NULL ,"
                + "GROUPID varchar2 (20) NOT NULL, "
                + "PRIMARY KEY(USERID,GROUPID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO USERGROUPS(USERID, GROUPID) VALUES('001', '01')";
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create USERGROUPS table.\n\r";
            }

            // Create USERS
            command.CommandText = "CREATE TABLE USERS ("
                + "USERID varchar2 (20) NOT NULL ,"
                + "USERNAME varchar2 (30) NULL ,"
                + "AGENT varchar2 (20) NULL ,"
                + "PWD varchar2 (10) NULL ,"
                + "CREATEDATE varchar2 (8) NULL ,"
                + "CREATER varchar2 (20) NULL ,"
                + "DESCRIPTION varchar2 (100) NULL ,"
                + "EMAIL varchar2 (40) NULL ,"
                + "LASTTIME varchar2 (8) NULL ,"
                + "AUTOLOGIN varchar2 (1),"
                + "LASTDATE varchar2 (8) NULL ,"
                + "SIGNATURE varchar2 (30) NULL ,"
                + "MSAD varchar2 (1) NULL, "
                + "PRIMARY KEY(USERID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO USERS(USERID, USERNAME, PWD, MSAD,AUTOLOGIN) VALUES('001', 'TEST', '', 'N','S')";
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create USERS table.\n\r";
            }

            #region 暂时不要的
            //// Create MENUTABLELOG
            //command.CommandText = "CREATE TABLE MENUTABLELOG"
            //    + "("
            //    + "LOGID number(10),"
            //    + "MENUID varchar2(30) not null,"
            //    + "PACKAGE varchar2(20) not null,"
            //    + "PACKAGEDATE DATE,"
            //    + "LASTDATE DATE,"
            //    + "OWNER varchar2(20),"
            //    + "OLDVERSION blob,"
            //    + "OLDDATE varchar2(20), "
            //    + "PRIMARY KEY(LOGID)"
            //    + ")";
            //try
            //{
            //    command.ExecuteNonQuery();

            //    command.CommandText = "CREATE SEQUENCE MENUTABLELOG_LODID_SEQ " +
            //                            "START WITH 1 " +
            //                            "MAXVALUE 9999999999 " +
            //                            "MINVALUE 1 " +
            //                            "NOCYCLE " +
            //                            "NOCACHE " +
            //                            "NOORDER";
            //    command.ExecuteNonQuery();

            //    command.CommandText = "CREATE OR REPLACE TRIGGER MENUTABLELOG_LODID" +
            //                " BEFORE INSERT" +
            //                " ON MENUTABLELOG" +
            //                " FOR EACH ROW" +
            //                " DECLARE" +
            //                " NEXT_ID NUMBER;" +
            //                " BEGIN" +
            //                " SELECT MENUTABLELOG_LODID_SEQ.NEXTVAL INTO NEXT_ID FROM dual;" +
            //                " :NEW.LOGID := NEXT_ID;" +
            //                " END;";
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create MENUTABLELOG table.\n\r";
            //}


            //// Create MENUCHECKLOG
            //command.CommandText = "CREATE TABLE MENUCHECKLOG"
            //    + "("
            //    + "LOGID number(10),"
            //    + "ITEMTYPE varchar2(20) not null,"
            //    + "PACKAGE varchar2(50) not null,"
            //    + "PACKAGEDATE Date,"
            //    + "FILETYPE varchar2(10),"
            //    + "FILENAME varchar2(60),"
            //    + "FILEDATE Date,"
            //    + "FILECONTENT blob, "
            //    + "PRIMARY KEY(LOGID)"
            //    + ")";
            //try
            //{
            //    command.ExecuteNonQuery();

            //    command.CommandText = "CREATE SEQUENCE MENUCHECKLOG_LODID_SEQ " +
            //                            "START WITH 1 " +
            //                            "MAXVALUE 9999999999 " +
            //                            "MINVALUE 1 " +
            //                            "NOCYCLE " +
            //                            "NOCACHE " +
            //                            "NOORDER";
            //    command.ExecuteNonQuery();

            //    command.CommandText = "CREATE OR REPLACE TRIGGER MENUCHECKLOG_LODID" +
            //                            " BEFORE INSERT" +
            //                            " ON MENUCHECKLOG" +
            //                            " FOR EACH ROW" +
            //                            " DECLARE" +
            //                            " NEXT_ID NUMBER;" +
            //                            " BEGIN" +
            //                            " SELECT MENUCHECKLOG_LODID_SEQ.NEXTVAL INTO NEXT_ID FROM dual;" +
            //                            " :NEW.LOGID := NEXT_ID;" +
            //                            " END;";
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create MENUCHECKLOG table.\n\r";
            //}
            #endregion

            // Create SYSEEPLOG
            command.CommandText = "CREATE TABLE SYSEEPLOG"
                + "("
                + "CONNID varchar2(20) NOT NULL,"
                + "LOGID number(10) NOT NULL,"
                + "LOGSTYLE varchar2(1) NOT NULL,"
                + "LOGDATETIME DATE NOT NULL,"
                + "DOMAINID varchar2(30) NULL,"
                + "USERID varchar2(20) NULL,"
                + "LOGTYPE varchar2(1) NULL,"
                + "TITLE varchar2(64) NULL,"
                + "DESCRIPTION LONG NULL,"
                + "COMPUTERIP varchar2(16) NULL,"
                + "COMPUTERNAME varchar2(64) NULL,"
                + "EXECUTIONTIME INT NULL,"
                + "PRIMARY KEY(CONNID,LOGID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();

                command.CommandText = "CREATE SEQUENCE SYSEEPLOG_LOGID_SEQ " +
                                        "START WITH 1 " +
                                        "MAXVALUE 9999999999 " +
                                        "MINVALUE 1 " +
                                        "CYCLE " +
                                        "NOCACHE " +
                                        "NOORDER";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE OR REPLACE TRIGGER SYSEEPLOG_LOGID" +
                                        " BEFORE INSERT" +
                                        " ON SYSEEPLOG" +
                                        " FOR EACH ROW" +
                                        " DECLARE" +
                                        " NEXT_ID NUMBER;" +
                                        " BEGIN" +
                                        " SELECT SYSEEPLOG_LOGID_SEQ.NEXTVAL INTO NEXT_ID FROM dual;" +
                                        " :NEW.LOGID := NEXT_ID;" +
                                        " END;";
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYSEEPLOG table.\n\r";
            }

            // Create SYSERRLOG
            command.CommandText = "CREATE TABLE SYSERRLOG"
                + "("
                + "ERRID number(10),"
                + "USERID varchar2(20), "
                + "MODULENAME varchar2(30),"
                + "ERRMESSAGE varchar2(255),"
                + "ERRSTACK varchar2(255),"
                + "ERRDESCRIP varchar2(255),"
                + "ERRDATE Date,"
                + "ERRSCREEN blob,"
                + "OWNER varchar2(20),"
                + "PROCESSDATE Date,"
                + "PROCDESCRIP varchar2(255),"
                + "STATUS varchar2(2), "
                + "PRIMARY KEY(ERRID)"
                + ")";
            try
            {
                command.ExecuteNonQuery();

                command.CommandText = "CREATE SEQUENCE SYSERRLOG_ErrID_SEQ " +
                                        "START WITH 1 " +
                                        "MAXVALUE 9999999999 " +
                                        "MINVALUE 1 " +
                                        "NOCYCLE " +
                                        "NOCACHE " +
                                        "NOORDER";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE OR REPLACE TRIGGER SYSERRLOG_ErrID" +
                                        " BEFORE INSERT" +
                                        " ON SYSERRLOG" +
                                        " FOR EACH ROW" +
                                        " DECLARE" +
                                        " NEXT_ID NUMBER;" +
                                        " BEGIN" +
                                        " SELECT SYSERRLOG_ErrID_SEQ.NEXTVAL INTO NEXT_ID FROM dual;" +
                                        " :NEW.ErrID := NEXT_ID;" +
                                        " END;";
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYSERRLOG table.\n\r";
            }

            #region 暂时不要的
            //command.CommandText = "CREATE TABLE SYS_LANGUAGE"
            //    + "("
            //    + "ID number(10) NOT NULL,"
            //    + "IDENTIFICATION varchar2(80),"
            //    + "KEYS varchar2(80),"
            //    + "EN varchar2(80),"
            //    + "CHT varchar2(80),"
            //    + "CHS varchar2(80),"
            //    + "HK varchar2(80),"
            //    + "JA varchar2(80),"
            //    + "KO varchar2(80),"
            //    + "LAN1 varchar2(80),"
            //    + "LAN2 varchar2(80), "
            //    + "PRIMARY KEY(ID)"
            //    + ")";

            //try
            //{
            //    command.ExecuteNonQuery();

            //    command.CommandText = "CREATE SEQUENCE SYS_LANGUAGE_ID_SEQ " +
            //                            "START WITH 1 " +
            //                            "MAXVALUE 9999999999 " +
            //                            "MINVALUE 1 " +
            //                            "NOCYCLE " +
            //                            "NOCACHE " +
            //                            "NOORDER";
            //    command.ExecuteNonQuery();

            //    command.CommandText = "CREATE OR REPLACE TRIGGER SYS_LANGUAGE_ID" +
            //                            " BEFORE INSERT" +
            //                            " ON SYS_LANGUAGE" +
            //                            " FOR EACH ROW" +
            //                            " DECLARE" +
            //                            " NEXT_ID NUMBER;" +
            //                            " BEGIN" +
            //                            " SELECT SYS_LANGUAGE_ID_SEQ.NEXTVAL INTO NEXT_ID FROM dual;" +
            //                            " :NEW.ID := NEXT_ID;" +
            //                            " END;";
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_LANGUAGE table.\n\r";
            //}

            //command.CommandText = "CREATE TABLE SYS_MESSENGER"
            //    + "("
            //    + "USERID varchar2(20) NOT NULL,"
            //    + "MESSAGE varchar2(255),"
            //    + "PARAS varchar2(255),"
            //    + "SENDTIME varchar2(14),"
            //    + "SENDERID varchar2(20),"
            //    + "RECTIME varchar2(14),"
            //    + "STATUS char(1)"
            //    + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_MESSENGER table.\n\r";
            //}

            ////CreateMenuTableControl
            //command.CommandText = "CREATE TABLE MENUTABLECONTROL"
            //                        + "("
            //                        + "MENUID varchar2 (30) NOT NULL, "
            //                        + "CONTROLNAME Varchar2 (50) NOT NULL, "
            //                        + "DESCRIPTION Varchar2 (50) NULL, "
            //                        + "TYPE Varchar (20) NULL, "
            //                        + "PRIMARY KEY(MENUID,CONTROLNAME)"
            //                        + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create MENUTABLECONTROL table.\n\r";
            //}

            //CreateGroupMenuControl
            //command.CommandText = "CREATE TABLE GROUPMENUCONTROL"
            //                        + "("
            //                        + "GROUPID Varchar2 (20) NOT NULL, "
            //                        + "MENUID Varchar2 (30) NOT NULL, "
            //                        + "CONTROLNAME Varchar2 (50) NOT NULL, "
            //                        + "TYPE Varchar2 (20) NULL, "
            //                        + "ENABLED CHAR (1) NULL, "
            //                        + "VISIBLE CHAR (1) NULL, "
            //                        + "ALLOWADD CHAR (1) NULL, "
            //                        + "ALLOWUPDATE CHAR (1) NULL, "
            //                        + "ALLOWDELETE CHAR (1) NULL, "
            //                        + "ALLOWPRINT CHAR (1) NULL, "
            //                        + "PRIMARY KEY(GROUPID,MENUID,CONTROLNAME)"
            //                        + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create GROUPMENUCONTROL table.\n\r";
            //}

            ////CreateUserMenuControl
            //command.CommandText = "CREATE TABLE USERMENUCONTROL"
            //                        + "("
            //                        + "USERID Varchar2 (20) NOT NULL, "
            //                        + "MENUID Varchar2 (30) NOT NULL, "
            //                        + "CONTROLNAME Varchar2 (50) NOT NULL, "
            //                        + "TYPE Varchar2 (20) NULL, "
            //                        + "ENABLED CHAR (1) NULL, "
            //                        + "VISIBLE CHAR (1) NULL, "
            //                        + "ALLOWADD CHAR (1) NULL, "
            //                        + "ALLOWUPDATE CHAR (1) NULL, "
            //                        + "ALLOWDELETE CHAR (1) NULL, "
            //                        + "ALLOWPRINT CHAR (1) NULL, "
            //                        + "PRIMARY KEY(USERID,MENUID,CONTROLNAME)"
            //                        + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create USERMENUCONTROL table.\n\r";
            //}
            #endregion

            command.CommandText = "CREATE TABLE SYS_REFVAL"
                                + "("
                                + "REFVAL_NO Varchar2(30) Not NULL, "
                                + "DESCRIPTION Varchar2(250), "
                                + "TABLE_NAME Varchar2(30), "
                                + "CAPTION Varchar2(30), "
                                + "DISPLAY_MEMBER Varchar2(30), "
                                + "SELECT_ALIAS Varchar2(250), "
                                + "SELECT_COMMAND Varchar2(250), "
                                + "VALUE_MEMBER Varchar2(30), "
                                + "CONSTRAINT PK_SYS_REFVAL PRIMARY KEY(REFVAL_NO) "
                                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYS_REFVAL table.\n\r";
            }

            command.CommandText = "CREATE TABLE SYS_REFVAL_D1"
                                + "("
                                + "REFVAL_NO Varchar2(30) NOT NULL, "
                                + "FIELD_NAME Varchar2(30) NOT NULL, "
                                + "HEADER_TEXT Varchar2(20), "
                                + "WIDTH INT, "
                                + "CONSTRAINT PK_SYS_REFVAL_D1 PRIMARY KEY(REFVAL_NO, FIELD_NAME) "
                                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYS_REFVAL table.\n\r";
            }

            command.CommandText = "CREATE TABLE MENUFAVOR"
                                + "("
                                + "MENUID Varchar2(30) NOT NULL, "
                                + "CAPTION Varchar2(50) NOT NULL, "
                                + "USERID Varchar2(20) NOT NULL, "
                                + "ITEMTYPE Varchar(20), "
                                + "GROUPNAME VarChar(20), "
                                + "PRIMARY KEY (MENUID,USERID)"
                                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create MENUFAVOR table.\n\r";
            }

            #region 暂时不要的
            //command.CommandText = "CREATE TABLE SYS_ANYQUERY"
            //                    + "("
            //                    + "QUERYID Varchar2(20) Not NULL, "
            //                    + "USERID Varchar2(20) Not NULL, "
            //                    + "TEMPLATEID Varchar2(20), "
            //                    + "TABLENAME Varchar2(50), "
            //                    + "LASTDATE date, "
            //                    + "CONTENT blob, "
            //                    + "PRIMARY Key (QUERYID,USERID,TEMPLATEID)"
            //                    + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_ANYQUERY table.\n\r";
            //}
            #endregion

            command.CommandText = "CREATE TABLE SYS_REPORT"
                                + "("
                                + "REPORTID nVarchar2(50) Not NULL, "
                                + "FILENAME nVarchar2(50) Not NULL, "
                                + "REPORTNAME nVarchar2(255), "
                                + "DESCRIPTION nVarchar2(255), "
                                + "FILEPATH nVarchar2(255), "
                                + "OUTPUTMODE nVarchar2(20), "
                                + "HEADERREPEAT nVarchar2(5), "
                                + "HEADERFONT blob, "
                                + "HEADERITEMS blob, "
                                + "FOOTERFONT blob, "
                                + "FOOTERITEMS blob, "
                                + "FIELDFONT blob, "
                                + "FIELDITEMS blob, "
                                + "SETTING blob, "
                                + "FORMAT blob, "
                                + "PARAMETERS blob, "
                                + "IMAGES blob, "
                                + "MAILSETTING blob, "
                                + "DATASOURCE_PROVIDER nVarchar2(50),"
                               + "DATASOURCES blob,"
                               + "CLIENT_QUERY blob,"
                               + "REPORT_TYPE nVarchar2(1),"
                               + "TEMPLATE_DESC nVarchar2(50),"
                                + "PRIMARY Key (REPORTID,FILENAME)"
                                + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYS_REPORT table.\n\r";
            }

            command.CommandText = "CREATE TABLE SYS_PERSONAL"
                               + "("
                               + "FORMNAME NVARCHAR2(60) NOT NULL,"
                               + "COMPNAME NVARCHAR2(30) NOT NULL,"
                               + "USERID NVARCHAR2(20) NOT NULL,"
                               + "REMARK NVARCHAR2(30),"
                               + "PROPCONTENT LONG,"
                               + "CREATEDATE DATE,"
                               + "PRIMARY KEY (FORMNAME,COMPNAME,USERID)"
                               + ")";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not create SYS_PERSONAL table.\n\r";
            }
            #endregion

            #region WorkFlow
            //GROUPS
            command.CommandText = "ALTER TABLE GROUPS ADD ISROLE CHAR(1) NULL";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not add ISROLE column to GROUPS table.\n\r";
            }

            #region 暂时不用
            //// SYS_ORG
            //command.CommandText = "CREATE TABLE SYS_ORG(" +
            //                    "ORG_NO varchar2(8) NOT NULL," +
            //                    "ORG_DESC varchar2(40) NOT NULL," +
            //                    "ORG_KIND varchar2(4) NOT NULL," +
            //                    "UPPER_ORG varchar2(8) NULL," +
            //                    "ORG_MAN varchar2(20) NOT NULL," +
            //                    "LEVEL_NO varchar2(6) NOT NULL," +
            //                    "ORG_TREE varchar2(40) NULL," +
            //                    "END_ORG varchar2(4) NULL," +
            //                    "ORG_FULLNAME varchar2(254) NULL," +
            //                    "PRIMARY KEY(ORG_NO)" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();

            //    command.CommandText = "INSERT INTO SYS_ORG(ORG_NO,ORG_DESC,ORG_KIND,ORG_MAN,LEVEL_NO) Values ('1','總公司','0','001','9')";
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_ORG table.\n\r";
            //}

            //// SYS_ORGKIND
            //command.CommandText = "CREATE TABLE SYS_ORGKIND(" +
            //                    "ORG_KIND varchar2(4) NOT NULL," +
            //                    "KIND_DESC varchar2(40) NOT NULL," +
            //                    "PRIMARY KEY(ORG_KIND)" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();

            //    command.CommandText = "INSERT INTO SYS_ORGKIND(ORG_KIND,KIND_DESC) Values ('0','公司組織')";
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_ORGKIND table.\n\r";
            //}

            //// SYS_ORGLEVEL
            //command.CommandText = "CREATE TABLE SYS_ORGLEVEL(" +
            //                    "LEVEL_NO varchar2(6) NOT NULL," +
            //                    "LEVEL_DESC varchar2(40) NOT NULL," +
            //                    "PRIMARY KEY(LEVEL_NO)" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('0','直屬主管')";
            //    command.ExecuteNonQuery();
            //    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('1','主任/課長/副理')";
            //    command.ExecuteNonQuery();
            //    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('2','經理')";
            //    command.ExecuteNonQuery();
            //    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('3','副總')";
            //    command.ExecuteNonQuery();
            //    command.CommandText = "INSERT INTO SYS_ORGLEVEL(LEVEL_NO,LEVEL_DESC) Values ('9','總經理')";
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_ORGLEVEL table.\n\r";
            //}

            //// SYS_ORGROLES
            //command.CommandText = "CREATE TABLE SYS_ORGROLES(" +
            //                    "ORG_NO varchar2(8) NOT NULL," +
            //                    "ROLE_ID varchar2(20) NOT NULL," +
            //                    "ORG_KIND varchar2(4) NULL," +
            //                    "PRIMARY KEY(ORG_NO,ROLE_ID)" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_ORGROLES table.\n\r";
            //}

            //command.CommandText = "CREATE INDEX ORGNO ON SYS_ORGROLES(ORG_NO, ROLE_ID)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    //result += "Can not create INDEX ORGNO on table SYS_ORGROLES .\n\r";
            //}

            //// SYS_ROLES_AGENT
            //command.CommandText = "CREATE TABLE SYS_ROLES_AGENT(" +
            //                    "ROLE_ID varchar2(20) NULL," +
            //                    "AGENT varchar2(20) NOT NULL," +
            //                    "FLOW_DESC varchar2(40) NOT NULL," +
            //                    "START_DATE varchar2(8) NOT NULL," +
            //                    "START_TIME varchar2(6) NULL," +
            //                    "END_DATE varchar2(8) NOT NULL," +
            //                    "END_TIME varchar2(6) NULL," +
            //                    "PAR_AGENT varchar2(4) NOT NULL," +
            //                    "REMARK varchar2(254) NULL," +
            //                    "PRIMARY KEY(ROLE_ID,AGENT)" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_ROLES_AGENT table.\n\r";
            //}

            //command.CommandText = "CREATE INDEX ROLEID ON SYS_ROLES_AGENT(ROLE_ID)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    //result += "Can not create INDEX ROLEID on table SYS_ROLES_AGENT .\n\r";
            //}

            //// SYS_TODOHIS
            //command.CommandText = "CREATE TABLE SYS_TODOHIS(" +
            //                    "LISTID varchar2(40) NOT NULL," +
            //                    "FLOW_ID varchar2(40) NOT NULL," +
            //                    "FLOW_DESC varchar2(40) NULL," +
            //                    "ROLE_ID varchar2(20) NULL," +
            //                    "S_ROLE_ID varchar2(20) NULL," +
            //                    "S_STEP_ID varchar2(20) NULL," +
            //                    "D_STEP_ID varchar2(20) NULL," +
            //                    "S_STEP_DESC varchar2(64) NULL," +
            //                    "S_USER_ID varchar2(20) NULL," +
            //                    "USER_ID varchar2(20) NOT NULL," +
            //                    "USERNAME varchar2(30) NULL," +
            //                    "FORM_NAME varchar2(30) NULL," +
            //                    "WEBFORM_NAME varchar2(50) NULL," +
            //                    "S_USERNAME varchar2(30) NULL," +
            //                    "NAVIGATOR_MODE varchar2(2) NOT NULL," +
            //                    "FLNAVIGATOR_MODE varchar2(2) NOT NULL," +
            //                    "PARAMETERS varchar2(254) NULL," +
            //                    "STATUS varchar2(4) NULL," +
            //                    "PROC_TIME number(8, 2) NOT NULL," +
            //                    "EXP_TIME number(8, 2) NOT NULL," +
            //                    "TIME_UNIT varchar2(4) NOT NULL," +
            //                    "FLOWIMPORTANT varchar2(1) NOT NULL," +
            //                    "FLOWURGENT varchar2(1) NOT NULL," +
            //                    "FORM_TABLE varchar2(30) NULL," +
            //                    "FORM_KEYS varchar2(254) NULL," +
            //                    "FORM_PRESENTATION varchar2(254) NULL," +
            //                    "REMARK varchar2(254) NULL," +
            //                    "VERSION varchar2(2) NULL," +
            //                    "VDSNAME varchar2(40) NULL," +
            //                    "SENDBACKSTEP varchar2(2) NULL," +
            //                    "LEVEL_NO varchar2(6) NULL," +
            //                    "UPDATE_DATE varchar2(10) NULL," +
            //                    "UPDATE_TIME varchar2(8) NULL," +
            //                    "FORM_PRESENT_CT varchar2(254) NULL," +
            //                    "ATTACHMENTS varchar2(255) NULL," +
            //                    "CREATE_TIME varchar2(50) NULL" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_TODOHIS table.\n\r";
            //}

            //command.CommandText = "CREATE INDEX LISTID ON SYS_TODOHIS(LISTID)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    //result += "Can not create INDEX LISTID on table SYS_TODOHIS.\n\r";
            //}

            //command.CommandText = "CREATE INDEX USERID ON SYS_TODOHIS(USER_ID)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    //result += "Can not create INDEX USERID on table SYS_TODOHIS.\n\r";
            //}

            //// SYS_TODOLIST
            //command.CommandText = "CREATE TABLE SYS_TODOLIST(" +
            //                    "LISTID varchar2(40) NOT NULL," +
            //                    "FLOW_ID varchar2(40) NOT NULL," +
            //                    "FLOW_DESC varchar2(40) NULL," +
            //                    "APPLICANT varchar2(20) NOT NULL," +
            //                    "S_USER_ID varchar2(20) NULL," +
            //                    "S_STEP_ID varchar2(20) NULL," +
            //                    "S_STEP_DESC varchar2(64) NULL," +
            //                    "D_STEP_ID varchar2(20) NULL," +
            //                    "D_STEP_DESC varchar2(64) NULL," +
            //                    "EXP_TIME number(8, 2) NOT NULL," +
            //                    "URGENT_TIME number(8, 2) NOT NULL," +
            //                    "TIME_UNIT varchar2(4) NOT NULL," +
            //                    "USERNAME varchar2(30) NULL," +
            //                    "FORM_NAME varchar2(30) NULL," +
            //                    "NAVIGATOR_MODE varchar2(2) NOT NULL," +
            //                    "FLNAVIGATOR_MODE varchar2(2) NOT NULL," +
            //                    "PARAMETERS varchar2(254) NULL," +
            //                    "SENDTO_KIND varchar2(4) NOT NULL," +
            //                    "SENDTO_ID varchar2(20) NOT NULL," +
            //                    "FLOWIMPORTANT varchar2(1) NOT NULL," +
            //                    "FLOWURGENT varchar2(1) NOT NULL," +
            //                    "STATUS varchar2(4) NULL," +
            //                    "FORM_TABLE varchar2(30) NULL," +
            //                    "FORM_KEYS varchar2(254) NULL," +
            //                    "FORM_PRESENTATION varchar2(254) NULL," +
            //                    "FORM_PRESENT_CT varchar2(254) NOT NULL," +
            //                    "REMARK varchar2(254) NULL," +
            //                    "PROVIDER_NAME varchar2(254) NULL," +
            //                    "VERSION varchar2(2) NULL," +
            //                    "EMAIL_ADD varchar2(40) NULL," +
            //                    "EMAIL_STATUS varchar2(1) NULL," +
            //                    "VDSNAME varchar2(40) NULL," +
            //                    "SENDBACKSTEP varchar2(2) NULL," +
            //                    "LEVEL_NO varchar2(6) NULL," +
            //                    "WEBFORM_NAME varchar2(50) NULL," +
            //                    "UPDATE_DATE varchar2(10) NULL," +
            //                    "UPDATE_TIME varchar2(8) NULL," +
            //                    "FLOWPATH varchar2(100) NOT NULL," +
            //                    "PLUSAPPROVE varchar2(1) NOT NULL," +
            //                    "PLUSROLES varchar2(254) NULL," +
            //                    "MULTISTEPRETURN varchar2(1) NULL," +
            //                    "SENDTO_NAME varchar2(30) NULL," +
            //                    "ATTACHMENTS varchar2(255) NULL," +
            //                    "CREATE_TIME varchar2(50) NULL" +
            //                    ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_TODOLIST table.\n\r";
            //}

            //command.CommandText = "CREATE INDEX LISTID ON SYS_TODOLIST(LISTID)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    //result += "Can not create INDEX LISTID on table SYS_TODOLIST.\n\r";
            //}

            //command.CommandText = "CREATE INDEX SENDTOID ON SYS_TODOLIST(SENDTO_ID, SENDTO_KIND)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    //result += "Can not create INDEX SENDTOID on table SYS_TODOLIST.\n\r";
            //}

            //command.CommandText = "CREATE INDEX FLOWDESC ON SYS_TODOLIST(FLOW_DESC)";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    //result += "Can not create INDEX FLOWDESC on table SYS_TODOLIST.\n\r";
            //}

            //// SYS_FLDefinition
            //command.CommandText = "CREATE TABLE SYS_FLDEFINITION"
            //                        + "("
            //                        + "FLTYPEID varchar2(50) NOT NULL, "
            //                        + "FLTYPENAME varchar2(200) NOT NULL, "
            //                        + "FLDEFINITION blob NOT NULL, "
            //                        + "VERSION int NULL,"
            //                        + "PRIMARY KEY(FLTYPEID)"
            //                        + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_FLDEFINITION table.\n\r";
            //}

            //// SYS_FLInstanceState
            //command.CommandText = "CREATE TABLE SYS_FLINSTANCESTATE"
            //                        + "("
            //                        + "FLINSTANCEID varchar2(50) NOT NULL, "
            //                        + "STATE blob NOT NULL, "
            //                        + "STATUS int NULL, "
            //                        + "INFO varchar2(200) NULL,"
            //                        + "PRIMARY KEY(FLINSTANCEID)"
            //                        + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_FLINSTANCESTATE table.\n\r";
            //}


            //// Sys_ExtApprove
            //command.CommandText = "CREATE TABLE SYS_EXTAPPROVE"
            //                        + "("
            //                        + "APPROVEID varchar2(50) NULL, "
            //                        + "GROUPID varchar2(50) NULL, "
            //                        + "MINIMUM varchar2(50) NULL, "
            //                        + "MAXIMUM varchar2(50) NULL,"
            //                        + "ROLEID varchar2(50) NULL"
            //                        + ")";
            //try
            //{
            //    command.ExecuteNonQuery();
            //}
            //catch
            //{
            //    result += "Can not create SYS_EXTAPPROVE table.\n\r";
            //}
            #endregion
            #endregion

            #region Insert Proc
            command.CommandText =
@"CREATE proc spGenInsertSQL (@tablename varchar2(256))
as
begin
declare @sql varchar2(8000)
declare @sqlValues varchar2(8000)
set @sql =' ('
set @sqlValues = 'values (''+'
select @sqlValues = @sqlValues + cols + ' + '','' + ' ,@sql = @sql + '[' + name + '],'
  from
      (select case
           when xtype in (48,52,56,59,60,62,104,106,108,122,127)      
                then 'case when '+ name +' is null then ''NULL'' else ' + 'cast('+ name + ' as varchar)'+' end'
           when xtype in (58,61)
                then 'case when '+ name +' is null then ''NULL'' else '+''''''''' + ' + 'cast('+ name +' as varchar)'+ '+'''''''''+' end'
           when xtype in (167)
                then 'case when '+ name +' is null then ''NULL'' else '+''''''''' + ' + 'replace('+ name+','''''''','''''''''''')' + '+'''''''''+' end'
           when xtype in (231)
                then 'case when '+ name +' is null then ''NULL'' else '+'''N'''''' + ' + 'replace('+ name+','''''''','''''''''''')' + '+'''''''''+' end'
           when xtype in (175)
                then 'case when '+ name +' is null then ''NULL'' else '+''''''''' + ' + 'cast(replace('+ name+','''''''','''''''''''') as Char(' + cast(length as varchar2)  + '))+'''''''''+' end'
           when xtype in (239)
                then 'case when '+ name +' is null then ''NULL'' else '+'''N'''''' + ' + 'cast(replace('+ name+','''''''','''''''''''') as Char(' + cast(length as varchar2)  + '))+'''''''''+' end'
           else '''NULL'''
         end as Cols,name
         from syscolumns
     where id = object_id(@tablename)
  ) T
set @sql ='select ''INSERT INTO '+ @tablename + '' + left(@sql,len(@sql)-1)+') ' + left(@sqlValues,len(@sqlValues)-4) + ')'' from '+@tablename
print @sql
exec (@sql)
end
";
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result += "Can not add spGenInsertSQL Proc.\n\r";
            }

            #endregion

            return result;
        }

        public object DBNameTest(object[] paramters)
        {
            string databasename = paramters[0] as string;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string ret = string.Empty;
            try
            {
                connection = AllocateConnection(databasename);
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                return new object[] { 1, ret };
            }
            finally
            {
                ReleaseConnection(databasename, connection);
            }
            return new object[] { 0, ret };
        }

        public object ConnectionTest(object[] paramters)
        {
            string connectionString = paramters[0].ToString();
            string password = paramters[1].ToString();
            string type = paramters[2].ToString();
            connectionString = connectionString + ";Password=" + password;
            IDbConnection connection = null;
            string result = string.Empty;

            try
            {
                if (type == "1")
                {
#if Oracle2
                    System.Data.OracleClient.OracleConnection.ClearAllPools();
                    connection = new System.Data.OracleClient.OracleConnection(connectionString);
#endif
                }
                else
                {
                    SqlConnection.ClearAllPools();
                    connection = new SqlConnection(connectionString);

                    if (connection == null)
                    {
                        return new object[] { 1, "Connection is null" };
                    }
                }
                connection.Open();
                result = "Connection OK";
                connection.Close();
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return new object[] { 1, result };
            }
            finally
            {
                connection.Dispose();
            }
            return new object[] { 0, result };

        }
        public object UpdateDB(object[] paramters)
        {
            string userid = paramters[0] as string;
            string oldAliasName = paramters[1] as string;
            string aliasName = paramters[2] as string;
            bool split = (bool)paramters[3];
            bool isExist = paramters.Length > 4;
            string connectionString = paramters.Length > 4 ? paramters[4].ToString() : "";
            string connnectionPassword = paramters.Length > 4 ? paramters[5].ToString() : "";
            string type = paramters.Length > 4 ? paramters[6].ToString() : "";

            string ret = string.Empty;

            if (!Directory.Exists(Path.Combine(EEPRegistry.Server, "SDModule", userid)))
            {
                Directory.CreateDirectory(Path.Combine(EEPRegistry.Server, "SDModule", userid));
            }

            string filepath = Path.Combine(EEPRegistry.Server, "SDModule", userid, "DB.xml");

            XmlDocument xml = new XmlDocument();

            if (File.Exists(filepath))
            {
                xml.Load(filepath);
                XmlNode nodeInfolight = xml.SelectSingleNode("InfolightDB");
                if (nodeInfolight != null)
                {
                    XmlNode nodeDataBase = nodeInfolight.SelectSingleNode("DataBase");
                    if (nodeDataBase != null)
                    {
                        XmlNode nodeDB = nodeDataBase.SelectSingleNode(oldAliasName);
                        if (nodeDB != null)
                        {
                            XmlNode node = xml.CreateElement(aliasName);
                            XmlAttribute att = xml.CreateAttribute("String");
                            if (isExist)
                                att.Value = connectionString;
                            else
                                att.Value = nodeDB.Attributes["String"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("Type");
                            if (isExist)
                            {
                                if (type.ToLower() == "sql")
                                    att.Value = "1";
                                else if (type.ToLower() == "oracle")
                                    att.Value = "2";
                                else att.Value = "1";
                            }
                            else
                                att.Value = nodeDB.Attributes["Type"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("OdbcType");
                            att.Value = nodeDB.Attributes["OdbcType"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("MaxCount");
                            att.Value = nodeDB.Attributes["MaxCount"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("TimeOut");
                            att.Value = nodeDB.Attributes["TimeOut"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("Master");
                            att.Value = split ? "1" : "0";
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("Encrypt");
                            att.Value = nodeDB.Attributes["Encrypt"].Value;
                            node.Attributes.Append(att);

                            att = xml.CreateAttribute("Password");
                            if (isExist)
                            {
                                att.Value = Srvtools.CliUtils.EncodePassword(userid, connnectionPassword);
                            }
                            else
                                att.Value = nodeDB.Attributes["Password"].Value;
                            node.Attributes.Append(att);

                            nodeDataBase.RemoveChild(nodeDB);
                            nodeDataBase.AppendChild(node);
                            xml.Save(filepath);
                        }
                    }
                }
            }

            return new object[] { 0, ret };

        }

        public object SetSystemTableForDBXML(object[] paramters)
        {
            string userid = paramters[0].ToString();
            string aliasName = paramters[1].ToString();
            string result = string.Empty;
            try
            {
                string filepath = Path.Combine(EEPRegistry.Server, "SDModule", userid, "DB.xml");
                XmlDocument xml = new XmlDocument();

                if (File.Exists(filepath))
                {
                    xml.Load(filepath);

                    XmlNode nodeInfolight = xml.SelectSingleNode("InfolightDB");
                    if (nodeInfolight == null)
                    {
                        nodeInfolight = xml.CreateElement("InfolightDB");
                        xml.AppendChild(nodeInfolight);
                    }
                    XmlNode nodeSystemDB = nodeInfolight.SelectSingleNode("SystemDB");
                    if (nodeSystemDB == null)
                    {
                        nodeSystemDB = xml.CreateElement("SystemDB");
                        nodeSystemDB.InnerText = aliasName;
                        nodeInfolight.AppendChild(nodeSystemDB);
                    }
                    else
                    {
                        nodeSystemDB.InnerText = aliasName;
                    }
                    xml.Save(filepath);
                }
                else
                    result = "NoXML";
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return new object[] { 1, result };
            }
            finally
            {
            }
            return new object[] { 0, result };

        }
        public object GetDBXml(object[] paramters)
        {
            string userid = paramters[0].ToString();
            string result = string.Empty;
            try
            {
                string filepath = Path.Combine(EEPRegistry.Server, "SDModule", userid, "DB.xml");

                XmlDocument xml = new XmlDocument();

                if (File.Exists(filepath))
                {
                    xml.Load(filepath);

                    XmlNode nodeInfolight = xml.SelectSingleNode("InfolightDB");
                    if (nodeInfolight == null)
                    {
                        nodeInfolight = xml.CreateElement("InfolightDB");
                        xml.AppendChild(nodeInfolight);
                    }
                    XmlNode nodeSystemDB = nodeInfolight.SelectSingleNode("SystemDB");
                    result = nodeSystemDB != null ? nodeSystemDB.InnerText : "";
                }
                else
                    result = "NoXML";
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return new object[] { 1, result };
            }
            finally
            {
            }
            return new object[] { 0, result };

        }
        public object GetExistDBXml(object[] paramters)
        {
            string userid = paramters[0].ToString();
            string oldAliasName = paramters[1].ToString();
            object result = string.Empty;
            try
            {
                string filepath = Path.Combine(EEPRegistry.Server, "SDModule", userid, "DB.xml");

                XmlDocument xml = new XmlDocument();

                if (File.Exists(filepath))
                {
                    xml.Load(filepath);

                    XmlNode nodeInfolight = xml.SelectSingleNode("InfolightDB");
                    if (nodeInfolight == null)
                    {
                        nodeInfolight = xml.CreateElement("InfolightDB");
                        xml.AppendChild(nodeInfolight);
                    }
                    XmlNode nodeDataBase = nodeInfolight.SelectSingleNode("DataBase");
                    if (nodeDataBase != null)
                    {
                        XmlNode nodeDB = nodeDataBase.SelectSingleNode(oldAliasName);
                        if (nodeDB != null)
                        {
                            string connectionstring = nodeDB.Attributes["String"].Value;
                            string split = nodeDB.Attributes["Master"].Value;
                            string type = nodeDB.Attributes["Type"].Value;
                            string password = nodeDB.Attributes["Password"].Value;

                            result = connectionstring + "\"" + GetPwdString(password) + "\"" + type;
                        }
                    }
                }
                else
                    result = "NoXML";
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return new object[] { 1, result };
            }
            finally
            {
            }
            return new object[] { 0, result };

        }
        private string GetPwdString(string s)
        {
            string sRet = "";
            for (int i = 0; i < s.Length; i++)
            {
                sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
            }
            return sRet;
        }
        public object SetAllSolutionsToDB(object[] paramters)
        {
            string userid = paramters[0].ToString();
            string dbid = paramters[1].ToString();
            string dbname = paramters[2].ToString();
            string dboption = paramters[3].ToString();

            string ret = "";
            string[] dbsplit = dboption.Split(',');
            foreach (string alias in dbsplit)
            {
                string sysAlias = GetSystemAlias(userid, alias);
                IDbConnection connection = null;
                IDbTransaction transaction = null;
                try
                {
                    connection = AllocateConnection(sysAlias);
                    transaction = connection.BeginTransaction();
                    IDbCommand command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "DELETE MENUITEMTYPE WHERE ITEMTYPE='" + dbid + "'";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO MENUITEMTYPE(ITEMTYPE, ITEMNAME) VALUES('" + dbid + "', '" + dbname + "')";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (ret != "")
                    {
                        ret += "\r\n";
                    }
                    ret += ex.Message;
                }
                finally
                {
                    transaction.Dispose();
                    ReleaseConnection(alias, connection);
                }
            }
            if (ret != "")
                return new object[] { 1, ret };
            else
                return new object[] { 0, "Success" };
        }
        public object UpdateSolutionIDBecauseModity(object[] paramters)
        {
            string oldSolutionId = paramters[0].ToString();
            string newSolutionId = paramters[1].ToString();

            string ret = string.Empty;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string Alias = DbConnectionSet.GetSystemDatabase(null);
            ClientType ct = ClientType.ctNone;
            connection = Srvtools.DbConnectionSet.WaitForConnection(Alias, "", ref ct, GetClientInfo(ClientInfoType.LoginUser).ToString()
                        , this.GetType().FullName, DateTime.Now);
            transaction = connection.BeginTransaction();
            try
            {
                string getsql = "update SYS_WEBPAGES set SolutionID='" + newSolutionId + "' where SolutionID='" + oldSolutionId + "'";
                this.ExecuteSql(getsql, connection, transaction);
                transaction.Commit();
            }
            catch (Exception exception) { ret += exception.Message; }
            finally
            {
                transaction.Dispose();
                DbConnectionSet.ReleaseConnection(Alias, "", connection);
            }
            return new object[] { 0, ret };
        }
        public object DeleteMenuItemType(object[] paramters)
        {
            string userid = paramters[0].ToString();
            string solutionid = paramters[1].ToString();
            string aliasoptions = paramters[2].ToString();
            string[] dbsplit = aliasoptions.Split('.');

            string ret = "";
            foreach (string alias in dbsplit)
            {
                string sysAlias = GetSystemAlias(userid, alias);
                IDbConnection connection = null;
                IDbTransaction transaction = null;
                try
                {
                    connection = AllocateConnection(sysAlias);
                    transaction = connection.BeginTransaction();
                    IDbCommand command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "DELETE MENUITEMTYPE WHERE ITEMTYPE='" + solutionid + "'";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (ret != "")
                    {
                        ret += "\r\n";
                    }
                    ret += ex.Message;
                }
                finally
                {
                    transaction.Dispose();
                    ReleaseConnection(alias, connection);
                }
            }
            if (ret != "")
                return new object[] { 1, ret };
            else
                return new object[] { 0, "Success" };

        }
        private string GetSystemAlias(string userid, string alias)
        {
            try
            {
                string filepath = Path.Combine(EEPRegistry.Server, "SDModule", userid, "DB.xml");
                XmlDocument xml = new XmlDocument();
                if (File.Exists(filepath))
                {
                    xml.Load(filepath);

                    XmlNode nodeInfolight = xml.SelectSingleNode("InfolightDB");
                    if (nodeInfolight != null)
                    {
                        XmlNode nodeDataBase = nodeInfolight.SelectSingleNode("DataBase");
                        if (nodeDataBase != null)
                        {
                            XmlNode nodeDB = nodeDataBase.SelectSingleNode(alias);
                            if (nodeDB != null)
                            {
                                string split = nodeDB.Attributes["Master"].Value;
                                if (split == "1")
                                {
                                    XmlNode nodeSystemDB = nodeInfolight.SelectSingleNode("SystemDB");
                                    string systemAlias = nodeSystemDB.InnerText;
                                    if (systemAlias != "")
                                        return systemAlias;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
            }
            return alias;
        }

        public object CreateExistTable(object[] paramters)
        {
            string sourceTableName = paramters[0] as string;
            string tempTableName = paramters[1] as string;
            string newschema = paramters[2] as string;
            string columnValueInsert = paramters[3] as string;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            IDbCommand command = null;
            string dbAlias = string.Empty;
            string ret = string.Empty;
            try
            {
                dbAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                connection = AllocateConnection(dbAlias);
                command = connection.CreateCommand();
                transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                List<List<string>> fkList = new List<List<string>>();
                DataSet dsconstraint = ExecuteSql("sp_helpconstraint " + sourceTableName, connection, transaction);
                if (dsconstraint.Tables.Count > 1 && dsconstraint.Tables[1].Rows.Count > 0 && dsconstraint.Tables[1].Rows[0][0].ToString() != "")
                {
                    for (int i = 0; i < dsconstraint.Tables[1].Rows.Count; i++)
                    {
                        DataRow dr = dsconstraint.Tables[1].Rows[i];
                        if (dr["constraint_type"].ToString() == "FOREIGN KEY")
                        {
                            string name = dr["constraint_name"].ToString();
                            string keys = dr["constraint_keys"].ToString();
                            if (dsconstraint.Tables[1].Rows[i + 1]["constraint_type"].ToString() == " " && dsconstraint.Tables[1].Rows[i + 1]["constraint_keys"].ToString() != "")
                            {
                                List<string> fk = new List<string>();
                                fk.Add(name);
                                fk.Add(keys);
                                fk.Add(dsconstraint.Tables[1].Rows[i + 1]["constraint_keys"].ToString());
                                fkList.Add(fk);
                                i++;
                            }
                        }
                    }
                }
                try
                {
                    foreach (var fk in fkList)
                    {
                        command.CommandText = "ALTER TABLE " + sourceTableName + " DROP CONSTRAINT " + fk[0];
                        command.ExecuteNonQuery();
                    }
                }
                finally { }
                command.CommandText = "select * into " + tempTableName + " from " + sourceTableName + "";//建表和复制数据一起完成
                command.ExecuteNonQuery();
                command.CommandText = "Drop Table " + sourceTableName;//删除原表
                command.ExecuteNonQuery();
                command.CommandText = newschema;//建新表
                command.ExecuteNonQuery();
                command.CommandText = "select name from syscolumns where  id=object_id(N'" + sourceTableName + "') and COLUMNPROPERTY(id,name,'IsIdentity')=1";
                object identity = command.ExecuteScalar();
                command.CommandText = columnValueInsert;//复制数据到新表
                if (identity != null)
                {
                    command.CommandText = "set identity_insert " + sourceTableName + " on;" + command.CommandText + ";set identity_insert " + sourceTableName + " off;";
                }
                command.ExecuteNonQuery();
                command.CommandText = "Drop Table " + tempTableName;//删除临时表
                command.ExecuteNonQuery();
                //this.ExecuteSql(sqlstr, connection, transaction);

                try
                {
                    foreach (var fk in fkList)
                    {
                        command.CommandText = "ALTER TABLE " + sourceTableName + " ADD FOREIGN KEY (" + fk[1] + ") " + fk[2];
                        command.ExecuteNonQuery();
                    }
                }
                finally { }
                transaction.Commit();
                ret = "Success!" + sourceTableName;
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                transaction.Rollback();
                return new object[] { 1, ret };
            }
            finally
            {
                transaction.Dispose();
                ReleaseConnection(dbAlias, connection);
            }
            return new object[] { 0, ret };

        }

        public object CreateTable(object[] paramters)
        {
            string sqlstr = paramters[0] as string;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string dbAlias = string.Empty;
            string ret = string.Empty;
            try
            {
                dbAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                connection = AllocateConnection(dbAlias);
                transaction = connection.BeginTransaction();
                this.ExecuteSql(sqlstr, connection, transaction);
                transaction.Commit();
                ret = "Success!";
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                transaction.Rollback();
                return new object[] { 1, ret };

            }
            finally
            {
                transaction.Dispose();
                ReleaseConnection(dbAlias, connection);
            }
            return new object[] { 0, ret };
        }

        public object ExcuteSqlStr(object[] paramters)
        {
            string tablename = paramters[0] as string;
            string sqlstr = paramters[1] as string;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string dbAlias = string.Empty;
            string ret = string.Empty;
            try
            {
                dbAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                connection = AllocateConnection(dbAlias);
                transaction = connection.BeginTransaction();
                this.ExecuteSql(sqlstr, connection, transaction);
                transaction.Commit();
                ret = "Success!" + tablename;
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                transaction.Rollback();
                return new object[] { 1, ret };
            }
            finally
            {
                transaction.Dispose();
                ReleaseConnection(dbAlias, connection);
            }
            return new object[] { 0, ret };
        }
        /// <summary>
        /// 復隅岆瘁湔婓硌隅靡想腔Table
        /// </summary>
        /// <param name="paramters">珨?ㄛTableName</param>
        /// <returns>object瞎ㄛ彆菴珨弇0桶尨綅俴傖髡ㄛ菴媼弇0桶尨祥湔婓ㄛ1桶尨眒湔婓˙彆菴珨弇岆1桶尨綅俴堤嶒ㄛ菴媼弇岆Exception趼睫揹</returns>
        public object IsTableExist(object[] paramters)
        {
            string tableName = paramters[0] as string;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string dbAlias = string.Empty;
            string ret = string.Empty;
            try
            {
                dbAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                connection = AllocateConnection(dbAlias);
                transaction = connection.BeginTransaction();

                string strSqlGetMenu = "select count(*) from sysobjects where id=object_id('" + tableName + "')";

                DataSet ds = this.ExecuteSql(strSqlGetMenu, connection, transaction);
                string tablecount = ds.Tables[0].Rows[0][0].ToString();
                transaction.Commit();
                if (Convert.ToInt16(tablecount) > 0)
                    ret = tableName;
                else
                    ret = "0," + tableName;
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                transaction.Rollback();
                return new object[] { 1, ret };
            }
            finally
            {
                transaction.Dispose();
                ReleaseConnection(dbAlias, connection);
            }
            return new object[] { 0, ret };
        }

        /// <summary>
        /// Get Insert String
        /// </summary>
        /// <param name="paramters">Table Nmae</param>
        /// <returns>String DataSet</returns>
        public object GetInsertString(object[] paramters)
        {
            string tablename = paramters[0] as string;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string dbAlias = string.Empty;
            DataSet ret = new DataSet();
            try
            {
                dbAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                connection = AllocateConnection(dbAlias);
                transaction = connection.BeginTransaction();

                string strSql = @"exec spGenInsertSQL '" + tablename + "'";
                DataSet ds = this.ExecuteSql(strSql, connection, transaction);
                ds.Tables[0].TableName = tablename;
                transaction.Commit();
                ret = ds;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new object[] { 1, ex.Message };
            }
            finally
            {
                transaction.Dispose();
                ReleaseConnection(dbAlias, connection);
            }
            return new object[] { 0, ret };
        }

        /// <summary>
        /// 陂硌隅table靡腔Schema
        /// </summary>
        /// <param name="paramters">珨?ㄛwhere 惤曆</param>
        /// <returns></returns>
        public object CallTableSchema(object[] paramters)
        {
            string where = paramters[0] as string;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string dbAlias = string.Empty;
            DataSet ret = new DataSet();
            try
            {
                dbAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                connection = AllocateConnection(dbAlias);
                transaction = connection.BeginTransaction();

                string strSql = @"SELECT D.NAME as TableName,
ColumnName=A.NAME,IsIdentity = COLUMNPROPERTY(A.ID,A.NAME,'ISIDENTITY'),
IsPrimaryKey=CASE WHEN EXISTS(Select 1 FROM SYSOBJECTS Where XTYPE='PK' AND PARENT_OBJ=A.ID AND NAME IN (
SELECT NAME FROM SYSINDEXES Where INDID IN(
SELECT INDID FROM SYSINDEXKEYS Where ID = A.ID AND COLID=A.COLID))) THEN 1 ELSE 0 END,
ColumnType=B.NAME,(case when A.prec is null then A.length else A.prec end) as LENGTH,A.ISNULLABLE,DefaultValue=ISNULL(E.TEXT,''),
Scale=ISNULL(COLUMNPROPERTY(A.ID,A.NAME,'SCALE'),0) 
FROM SYSCOLUMNS A LEFT JOIN SYSTYPES B ON A.XUSERTYPE=B.XUSERTYPE 
INNER JOIN SYSOBJECTS D ON A.ID=D.ID AND D.XTYPE='U' AND D.NAME<>'DTPROPERTIES' 
LEFT JOIN SYSCOMMENTS E ON A.CDEFAULT=E.ID LEFT JOIN sys.extended_properties G 
ON A.ID=G.major_id AND A.COLID=G.minor_id LEFT JOIN sys.extended_properties F 
ON D.ID=F.major_id AND F.minor_id=0 ";
                strSql += " where D.Name in (" + where + ")";
                DataSet ds = this.ExecuteSql(strSql, connection, transaction);
                transaction.Commit();
                ret = ds;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new object[] { 1, ex.Message };
            }
            finally
            {
                transaction.Dispose();
                ReleaseConnection(dbAlias, connection);
            }
            return new object[] { 0, ret };
        }
        public object ExcuteSqlStr2(object[] paramters)
        {
            string sqlstr = paramters[0] as string;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string dbAlias = string.Empty;
            object ret;
            try
            {
                dbAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                connection = AllocateConnection(dbAlias);
                transaction = connection.BeginTransaction();
                DataSet ds = this.ExecuteSql(sqlstr, connection, transaction);
                transaction.Commit();
                ret = ds;
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                transaction.Rollback();
                return new object[] { 1, ret };
            }
            finally
            {
                transaction.Dispose();
                ReleaseConnection(dbAlias, connection);
            }
            return new object[] { 0, ret };
        }

        public object CallRefVal(object[] paramters)
        {
            string where = paramters[0] as string;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string dbAlias = string.Empty;
            DataSet ret = new DataSet();
            try
            {
                dbAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                connection = AllocateConnection(dbAlias);
                transaction = connection.BeginTransaction();

                string strSql = @"select * from SYS_REFVAL left join SYS_REFVAL_D1 on SYS_REFVAL.REFVAL_NO = SYS_REFVAL_D1.REFVAL_NO  ";
                strSql += " where SYS_REFVAL.REFVAL_NO in (" + where + ")";
                DataSet ds = this.ExecuteSql(strSql, connection, transaction);
                transaction.Commit();
                ret = ds;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new object[] { 1, ex.Message };
            }
            finally
            {
                transaction.Dispose();
                ReleaseConnection(dbAlias, connection);
            }
            return new object[] { 0, ret };
        }

        object oMissing = System.Reflection.Missing.Value;
        object WDWord9TableBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
        object WDAutoFitWindow = WdAutoFitBehavior.wdAutoFitWindow;
        object WdLine = Microsoft.Office.Interop.Word.WdUnits.wdLine;
        object Wdstory = Microsoft.Office.Interop.Word.WdUnits.wdStory;
        object WdTable = Microsoft.Office.Interop.Word.WdUnits.wdTable;
        object WDCount1 = 1;
        object oHeadingStyle1 = Microsoft.Office.Interop.Word.WdBuiltinStyle.wdStyleHeading1;
        object oHeadingStyle2 = Microsoft.Office.Interop.Word.WdBuiltinStyle.wdStyleHeading2;
        object oTrue = true;
        object oFalse = false;


        public object BuildCordova(object[] parameters)
        {
            DateTime dt = DateTime.Now;
            string userid = this.GetClientInfo( ClientInfoType.LoginUser).ToString();
            var dbAlias = string.Empty;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            try
            {
                dbAlias = DbConnectionSet.GetSystemDatabase(null);
                ClientType ct = ClientType.ctNone;
                connection = Srvtools.DbConnectionSet.WaitForConnection(dbAlias, "", ref ct, GetClientInfo(ClientInfoType.LoginUser).ToString()
                            , this.GetType().FullName, dt);
                //connection = AllocateConnection(dbAlias);
                transaction = connection.BeginTransaction();
                int id = 1;
                string strSql = "select max(ID) from SYS_SDQUEUE";
                DataSet ds = this.ExecuteSql(strSql, connection, transaction);
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0].ToString() != "")
                {
                    id = Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;
                }
                var type = "C";
                if (parameters[7].ToString().Length > 0)
                {
                    type = "I";
                }
                strSql = "insert into SYS_SDQUEUE(ID,UserID,PageType,CreateTime,PrintSetting,FinishFlag) values (" + id + ",'" + userid + "','" + type + "','" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "','" + "" + "',0)";
                this.ExecuteCommand(strSql, connection, transaction);

                var command = connection.CreateCommand();
                var parameter = command.CreateParameter();
                String sDocument = "@document";
                if (connection.GetType().Name == "SqlConnection")
                {
                    sDocument = "@document";
                    (parameter as SqlParameter).SqlDbType = SqlDbType.Image;
                }
                else if (connection.GetType().Name == "OleDbConnection")
                {
                    sDocument = "?";
                    (parameter as OleDbParameter).OleDbType = OleDbType.Binary;
                }
                else if (connection.GetType().Name == "OracleConnection")
                {
                    sDocument = ":document";
                    (parameter as OracleParameter).OracleType = OracleType.Blob;
                }
                else if (connection.GetType().Name == "OdbcConnection")
                {
                    sDocument = "?";
                    (parameter as OdbcParameter).OdbcType = OdbcType.Binary;
                }
                else if (connection.GetType().Name == "MySqlConnection")
                {
                    sDocument = "@document";
                    parameter.DbType = DbType.Binary;
                }
                else if (connection.GetType().Name == "Ifxonnection")
                {
                    sDocument = "@document";
                    parameter.DbType = DbType.Binary;
                }
                else if (connection.GetType().Name == "AseConnection")
                {
                    sDocument = "@document";
                    parameter.DbType = DbType.Binary;
                }
                command.CommandText = string.Format("insert into SYS_SDQUEUEPAGE (ID,Document,PageName) values ({0}, {1}, 'cordova')", id, sDocument);
                command.Transaction = transaction;
                parameter.ParameterName = "document";
                parameter.Value = Encoding.UTF8.GetBytes(string.Join(";", parameters));
                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                transaction.Commit();
                return new object[] { 0, "Success" };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                WriteError(ex.Message, "Print");
                return new object[] { 1, ex.Message };
            }
            finally
            {
                transaction.Dispose();
                DbConnectionSet.ReleaseConnection(dbAlias, "", connection);
            }
        
        }


        /// <summary>
        /// Print
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public object Print(object[] paramters)
        {
            DateTime dt = DateTime.Now;
            string userid = paramters[0].ToString();
            string solutionid = paramters[1].ToString();
            string pagetype = paramters[2].ToString();
            string printSettingString = paramters[3].ToString();
            string pagenames = paramters[4].ToString();
            string password = paramters[5].ToString();
            string curDatabase = paramters[6].ToString();
            string[] pagenamelist = pagenames.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string databasename = string.Empty;
            string dbAlias = string.Empty;
            string ret = string.Empty;
            try
            {
                dbAlias = DbConnectionSet.GetSystemDatabase(null);
                ClientType ct = ClientType.ctNone;
                connection = Srvtools.DbConnectionSet.WaitForConnection(dbAlias, "", ref ct, GetClientInfo(ClientInfoType.LoginUser).ToString()
                            , this.GetType().FullName, dt);
                //connection = AllocateConnection(dbAlias);
                transaction = connection.BeginTransaction();
                int id = 1;
                string strSql = "select max(ID) from SYS_SDQUEUE";
                DataSet ds = this.ExecuteSql(strSql, connection, transaction);
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0].ToString() != "")
                {
                    id = Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;
                }
                strSql = "insert into SYS_SDQUEUE(ID,UserID,PageType,CreateTime,PrintSetting,FinishFlag) values (" + id + ",'" + userid + "','" + pagetype + "','" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "','" + printSettingString + "',0)";
                this.ExecuteSql(strSql, connection, transaction);
                foreach (string pagename in pagenamelist)
                {
                    strSql = "insert into SYS_SDQUEUEPAGE (ID,Document,PageName) select " + id + ",Content,PageName from SYS_WEBPAGES where PageName='" + pagename + "' and PageType='" + pagetype + "' and UserID='" + userid + "' and SolutionID ='" + solutionid + "'";
                    this.ExecuteSql(strSql, connection, transaction);
                    if (pagetype.ToLower() == "w" || pagetype.ToLower() == "m" || pagetype.ToLower() == "r")
                    {
                        var newpassword = Encode(password);
                        newpassword = System.Web.HttpUtility.UrlEncode(newpassword);
                        var previewpath = GetPreviewPath();
                        string s = previewpath + "/SDLogOnSingleSign.aspx?userid=" + userid + "&password=" + newpassword + "&soluid=" + solutionid + "&db=" + curDatabase + "&target=" + pagename;
                        //string s = "http://223.26.68.213/JQSDWebClient/SDLogOnSingleSign.aspx?userid=" + userid + "&password=" + newpassword + "&soluid=" + solutionid + "&db=" +curDatabase+ "&target=" + pagename;
                        //string s = "http://localhost/JQSDWebClient/SDLogOnSingleSign.aspx?userid=" + userid + "&password=" + newpassword + "&soluid=" + solutionid + "&db=" +curDatabase+ "&target=" + pagename;
                        //string s = "http://www.infolight.com.tw/WebClient/sdcloud/SDLogOnSingleSign.aspx?userid=" + userid + "&password=" + newpassword + "&soluid=" + solutionid + "&db=" + curDatabase + "&target=" + pagename;
                        IDbCommand cm = connection.CreateCommand();
                        String sFile = "@file";
                        IDataParameter spFile = cm.CreateParameter();
                        if (connection.GetType().Name == "SqlConnection")
                        {
                            sFile = "@file";
                            (spFile as SqlParameter).SqlDbType = SqlDbType.Image;
                        }
                        else if (connection.GetType().Name == "OleDbConnection")
                        {
                            sFile = "?";
                            (spFile as OleDbParameter).OleDbType = OleDbType.Binary;
                        }
                        else if (connection.GetType().Name == "OracleConnection")
                        {
                            sFile = ":file";
                            (spFile as OracleParameter).OracleType = OracleType.Blob;
                        }
                        else if (connection.GetType().Name == "OdbcConnection")
                        {
                            sFile = "?";
                            (spFile as OdbcParameter).OdbcType = OdbcType.Binary;
                        }
                        else if (connection.GetType().Name == "MySqlConnection")
                        {
                            sFile = "@file";
                            spFile.DbType = DbType.Binary;
                        }
                        else if (connection.GetType().Name == "Ifxonnection")
                        {
                            sFile = "@file";
                            spFile.DbType = DbType.Binary;
                        }
                        else if (connection.GetType().Name == "AseConnection")
                        {
                            sFile = "@file";
                            spFile.DbType = DbType.Binary;
                        }
                        spFile.ParameterName = sFile;
                        spFile.Value = System.Text.Encoding.UTF8.GetBytes(s);
                        cm.Transaction = transaction;
                        cm.Parameters.Add(spFile);

                        strSql = "update SYS_SDQUEUEPAGE set Photo = @file where ID='" + id + "' and PageName='" + pagename + "'";
                        cm.CommandText = strSql;
                        cm.ExecuteNonQuery();
                    }
                    else if (pagetype.ToLower() == "o")
                    {
                        var newpassword = Encode(password);
                        newpassword = System.Web.HttpUtility.UrlEncode(newpassword);
                        var previewpath = GetPreviewPath();

                        string s = previewpath + "/SDLogOnSingleSign.aspx?userid=" + userid + "&password=" + newpassword + "&soluid=" + solutionid + "&db=" + curDatabase + "&target=" + pagename + "&type=o";
                        //string s = "http://223.26.68.213/JQSDWebClient/SDLogOnSingleSign.aspx?userid=" + userid + "&password=" + newpassword + "&soluid=" + solutionid + "&db=" +curDatabase+ "&target=" + pagename+ "&type=O";
                        //string s = "http://localhost/JQSDWebClient/SDLogOnSingleSign.aspx?userid=" + userid + "&password=" + newpassword + "&soluid=" + solutionid + "&db=" +curDatabase+ "&target=" + pagename+ "&type=O";
                        //string s = "http://www.infolight.com.tw/WebClient/sdcloud/SDLogOnSingleSign.aspx?userid=" + userid + "&password=" + newpassword + "&soluid=" + solutionid + "&db=" + curDatabase + "&target=" + pagename+ "&type=O";
                        IDbCommand cm = connection.CreateCommand();
                        String sFile = "@file";
                        IDataParameter spFile = cm.CreateParameter();
                        if (connection.GetType().Name == "SqlConnection")
                        {
                            sFile = "@file";
                            (spFile as SqlParameter).SqlDbType = SqlDbType.Image;
                        }
                        else if (connection.GetType().Name == "OleDbConnection")
                        {
                            sFile = "?";
                            (spFile as OleDbParameter).OleDbType = OleDbType.Binary;
                        }
                        else if (connection.GetType().Name == "OracleConnection")
                        {
                            sFile = ":file";
                            (spFile as OracleParameter).OracleType = OracleType.Blob;
                        }
                        else if (connection.GetType().Name == "OdbcConnection")
                        {
                            sFile = "?";
                            (spFile as OdbcParameter).OdbcType = OdbcType.Binary;
                        }
                        else if (connection.GetType().Name == "MySqlConnection")
                        {
                            sFile = "@file";
                            spFile.DbType = DbType.Binary;
                        }
                        else if (connection.GetType().Name == "Ifxonnection")
                        {
                            sFile = "@file";
                            spFile.DbType = DbType.Binary;
                        }
                        else if (connection.GetType().Name == "AseConnection")
                        {
                            sFile = "@file";
                            spFile.DbType = DbType.Binary;
                        }
                        spFile.ParameterName = sFile;
                        cm.Transaction = transaction;
                        spFile.Value = System.Text.Encoding.UTF8.GetBytes(s);
                        cm.Parameters.Add(spFile);

                        strSql = "update SYS_SDQUEUEPAGE set Photo = " + sFile + " where ID='" + id + "' and PageName='" + pagename + "'";
                        cm.CommandText = strSql;
                        cm.ExecuteNonQuery();

                    }
                    else if (pagetype.ToLower() == "g")
                    {
                        var newpassword = Encode(password);
                        newpassword = System.Web.HttpUtility.UrlEncode(newpassword);
                        var previewpath = GetPreviewPath();

                        string s = previewpath + "/SDLogOnSingleSign.aspx?userid=" + userid + "&password=" + newpassword + "&soluid=" + solutionid + "&db=" + curDatabase + "&target=" + pagename + "&type=g";
                        String sFile = "@file";
                        IDbCommand cm = connection.CreateCommand();
                        IDataParameter spFile = cm.CreateParameter();
                        if (connection.GetType().Name == "SqlConnection")
                        {
                            sFile = "@file";
                            (spFile as SqlParameter).SqlDbType = SqlDbType.Image;
                        }
                        else if (connection.GetType().Name == "OleDbConnection")
                        {
                            sFile = "?";
                            (spFile as OleDbParameter).OleDbType = OleDbType.Binary;
                        }
                        else if (connection.GetType().Name == "OracleConnection")
                        {
                            sFile = ":file";
                            (spFile as OracleParameter).OracleType = OracleType.Blob;
                        }
                        else if (connection.GetType().Name == "OdbcConnection")
                        {
                            sFile = "?";
                            (spFile as OdbcParameter).OdbcType = OdbcType.Binary;
                        }
                        else if (connection.GetType().Name == "MySqlConnection")
                        {
                            sFile = "@file";
                            spFile.DbType = DbType.Binary;
                        }
                        else if (connection.GetType().Name == "Ifxonnection")
                        {
                            sFile = "@file";
                            spFile.DbType = DbType.Binary;
                        }
                        else if (connection.GetType().Name == "AseConnection")
                        {
                            sFile = "@file";
                            spFile.DbType = DbType.Binary;
                        }
                        spFile.ParameterName = sFile;
                        spFile.Value = System.Text.Encoding.UTF8.GetBytes(s);
                        cm.Transaction = transaction;
                        cm.Parameters.Add(spFile);

                        strSql = "update SYS_SDQUEUEPAGE set Photo = " + sFile + " where ID='" + id + "' and PageName='" + pagename + "'";
                        cm.CommandText = strSql;
                        cm.ExecuteNonQuery();
                    }
                }
                if (pagetype.ToLower() == "t")
                {
                    IDbCommand cm = connection.CreateCommand();
                    cm.Transaction = transaction;
                    String sFile = "@file";
                    IDataParameter spFile = cm.CreateParameter();
                    if (connection.GetType().Name == "SqlConnection")
                    {
                        sFile = "@file";
                        (spFile as SqlParameter).SqlDbType = SqlDbType.Image;
                    }
                    else if (connection.GetType().Name == "OleDbConnection")
                    {
                        sFile = "?";
                        (spFile as OleDbParameter).OleDbType = OleDbType.Binary;
                    }
                    else if (connection.GetType().Name == "OracleConnection")
                    {
                        sFile = ":file";
                        (spFile as OracleParameter).OracleType = OracleType.Blob;
                    }
                    else if (connection.GetType().Name == "OdbcConnection")
                    {
                        sFile = "?";
                        (spFile as OdbcParameter).OdbcType = OdbcType.Binary;
                    }
                    else if (connection.GetType().Name == "MySqlConnection")
                    {
                        sFile = "@file";
                        spFile.DbType = DbType.Binary;
                    }
                    else if (connection.GetType().Name == "Ifxonnection")
                    {
                        sFile = "@file";
                        spFile.DbType = DbType.Binary;
                    }
                    else if (connection.GetType().Name == "AseConnection")
                    {
                        sFile = "@file";
                        spFile.DbType = DbType.Binary;
                    }
                    spFile.ParameterName = sFile;
                    byte[] bdocument = System.Text.Encoding.UTF8.GetBytes(pagenames);
                    spFile.Value = bdocument;
                    cm.Parameters.Add(spFile);
                    strSql = "insert into SYS_SDQUEUEPAGE(Document,ID,PageName) Values (" + sFile + "," + id + ",'TableSchema')";
                    cm.CommandText = strSql;
                    cm.ExecuteNonQuery();
                }

                #region oldpart
                //if (((string)paramters[1]) == "P")
                //{
                //    for (int i = 3; i < paramters.Length; i = i + 3)
                //    {
                //        string pageName = paramters[i].ToString();
                //        Byte[] imagebyte = paramters[i + 1] as byte[];
                //        string pagexml = paramters[i + 2].ToString();
                //        SqlParameter spFile = new SqlParameter("@file", SqlDbType.Image);
                //        spFile.Value = imagebyte;
                //        byte[] bdocument = System.Text.Encoding.UTF8.GetBytes(pagexml);
                //        SqlParameter spDocument = new SqlParameter("@Document", SqlDbType.Image);
                //        spDocument.Value = bdocument;
                //        SqlCommand cm = connection.CreateCommand() as SqlCommand;
                //        cm.Transaction = transaction as SqlTransaction;
                //        cm.Parameters.Add(spFile);
                //        cm.Parameters.Add(spDocument);
                //        strSql = "insert into SYS_SDQUEUEPAGE(Photo,ID,PageName,Document) Values (@file," + id + ",'" + pageName + "',@Document)";
                //        cm.CommandText = strSql;
                //        cm.ExecuteNonQuery();
                //    }
                //}
                //if (((string)paramters[1]) == "W")
                //{
                //    for (int i = 3; i < paramters.Length; i = i + 3)
                //    {
                //        string pageName = paramters[i].ToString();
                //        Byte[] imagebyte = paramters[i + 1] as byte[];
                //        string pagexml = paramters[i + 2].ToString();
                //        SqlParameter spFile = new SqlParameter("@file", SqlDbType.Image);
                //        spFile.Value = imagebyte;
                //        byte[] bdocument = System.Text.Encoding.UTF8.GetBytes(pagexml);
                //        SqlParameter spDocument = new SqlParameter("@Document", SqlDbType.Image);
                //        spDocument.Value = bdocument;
                //        SqlCommand cm = connection.CreateCommand() as SqlCommand;
                //        cm.Transaction = transaction as SqlTransaction;
                //        cm.Parameters.Add(spFile);
                //        cm.Parameters.Add(spDocument);
                //        strSql = "insert into SYS_SDQUEUEPAGE(Photo,ID,PageName,Document) Values (@file," + id + ",'" + pageName + "',@Document)";
                //        cm.CommandText = strSql;
                //        cm.ExecuteNonQuery();
                //    }
                //}
                //else if (((string)paramters[1]) == "S")
                //{
                //    for (int i = 3; i < paramters.Length; i = i + 2)
                //    {
                //        string pageName = paramters[i].ToString();
                //        byte[] pagexml = paramters[i + 1] as byte[];

                //        SqlParameter spFile = new SqlParameter("@file", SqlDbType.Image);
                //        spFile.Value = pagexml;
                //        SqlCommand cm = connection.CreateCommand() as SqlCommand;
                //        cm.Transaction = transaction as SqlTransaction;
                //        cm.Parameters.Add(spFile);

                //        strSql = "insert into SYS_SDQUEUEPAGE(Document,ID,PageName) Values (@file," + id + ",'" + pageName + "')";
                //        cm.CommandText = strSql;
                //        cm.ExecuteNonQuery();
                //    }
                //}
                //else if (((string)paramters[1]) == "T")
                //{
                //    SqlParameter spFile = new SqlParameter("@file", SqlDbType.Image);
                //    spFile.Value = paramters[2] as byte[];
                //    SqlCommand cm = connection.CreateCommand() as SqlCommand;
                //    cm.Transaction = transaction as SqlTransaction;
                //    cm.Parameters.Add(spFile);

                //    strSql = "insert into SYS_SDQUEUEPAGE(Document,ID,PageName) Values (@file," + id + ",'TableSchema')";
                //    cm.CommandText = strSql;
                //    cm.ExecuteNonQuery();
                //}
                #endregion
                transaction.Commit();
                return new object[] { 0, "Success" };
                //return PrintSchedul(null);
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                transaction.Rollback();
                WriteError(ex.Message, "Print");
                return new object[] { 1, ret };
            }
            finally
            {
                transaction.Dispose();
                DbConnectionSet.ReleaseConnection(dbAlias, "", connection);
            }

        }
        const string KEY_64 = "CLOUDAPP";
        const string IV_64 = "EEPCloud";
        public string Encode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            System.Security.Cryptography.DESCryptoServiceProvider cryptoProvider = new System.Security.Cryptography.DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            System.Security.Cryptography.CryptoStream cst = new System.Security.Cryptography.CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), System.Security.Cryptography.CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

        }
        public string Decode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            System.Security.Cryptography.DESCryptoServiceProvider cryptoProvider = new System.Security.Cryptography.DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            System.Security.Cryptography.CryptoStream cst = new System.Security.Cryptography.CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), System.Security.Cryptography.CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }

        #region getPrintWindow
        [DllImport("User32.Dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern IntPtr SetActiveWindow(IntPtr wHandle);
        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern int SetForegroundWindow(IntPtr hwnd);

        private IntPtr FindIEHandle(string PageTitle)
        {
            foreach (System.Diagnostics.Process pro in System.Diagnostics.Process.GetProcesses())
            {
                if (pro.MainWindowTitle.StartsWith(PageTitle))
                {
                    return pro.MainWindowHandle;
                }
                else if (pro.ProcessName.ToLower() == "firefox")
                {
                    return pro.MainWindowHandle;
                }
            }
            return (IntPtr)0;
        }
        private IntPtr FindIEHandle2(string pagename)
        {
            foreach (System.Diagnostics.Process pro in System.Diagnostics.Process.GetProcesses())
            {
                if (pro.MainWindowTitle.Contains(pagename))
                {
                    return pro.MainWindowHandle;
                }
                else if (pro.ProcessName.ToLower() == "firefox")
                {
                    return pro.MainWindowHandle;
                }
            }
            return (IntPtr)0;
        }
        #endregion

        public object PrintSchedul(object[] paramters)
        {
            string ret = string.Empty;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string Alias = DbConnectionSet.GetSystemDatabase(null);
            ClientType ct = ClientType.ctNone;
            connection = Srvtools.DbConnectionSet.WaitForConnection(Alias, "", ref ct, GetClientInfo(ClientInfoType.LoginUser).ToString()
                        , this.GetType().FullName, DateTime.Now);
            transaction = connection.BeginTransaction();
            try
            {
                string getsql = "select * from SYS_SDQUEUE where FinishFlag = 0 and (Status != 1 or Status is null)";
                DataSet ds = this.ExecuteSql(getsql, connection, transaction);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string getsql2 = "update SYS_SDQUEUE set Status =1 where  FinishFlag = 0 and Status is null";
                    this.ExecuteSql(getsql2, connection, transaction);
                    transaction.Commit();
                }
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    string id = r["ID"].ToString();
                    string userid = r["UserID"].ToString();
                    string pagetype = r["PageType"].ToString();
                    string printsetting = r["PrintSetting"].ToString();
                    string detailsql = "select * from SYS_SDQUEUEPAGE where ID=" + id;
                    DataSet detail = this.ExecuteSql(detailsql, connection, transaction);
                    List<object> l = new List<object>();
                    l.Add(userid);
                    l.Add(pagetype);
                    if (pagetype == "P")
                    {
                        l.Add(printsetting);
                        if (detail.Tables[0].Rows.Count > 0 && detail.Tables[0].Rows[0][0].ToString() != string.Empty)
                        {
                            foreach (DataRow dr in detail.Tables[0].Rows)
                            {
                                byte[] xmlstring = dr["Document"] as byte[];
                                byte[] image = dr["Photo"] as byte[];
                                //photo栏位存预览地址了
                                byte[] image2 = PrintScreen(image);
                                string pagename = dr["PageName"].ToString();
                                l.Add(pagename);
                                //l.Add(image);
                                l.Add(image2);
                                l.Add(xmlstring);
                            }
                        }
                        else
                        {
                            ret += ",";
                            ret += "error";
                            continue;
                        }
                    }
                    else if (pagetype == "W")
                    {
                        l.Add(printsetting);
                        if (detail.Tables[0].Rows.Count > 0 && detail.Tables[0].Rows[0][0].ToString() != string.Empty)
                        {
                            foreach (DataRow dr in detail.Tables[0].Rows)
                            {
                                byte[] xmlstring = dr["Document"] as byte[];
                                byte[] image = dr["Photo"] as byte[];
                                //photo栏位存预览地址了

                                //获取第一个有EditDialogID属性的DataGrid，为了打开Dialog列印图片
                                var firstdatagridid = "";
                                string pagexml = System.Text.Encoding.UTF8.GetString(xmlstring);
                                JObject context = (JObject)JsonConvert.DeserializeObject(pagexml);
                                JArray controls = (JArray)context["controls"];
                                var datagridlist = new List<object>();
                                var dataformlist = new List<object>();
                                var defaultslist = new List<object>();
                                var validatelist = new List<object>();
                                foreach (var component in controls)
                                {
                                    var componentType = (string)component["type"];
                                    var cts = componentType.Split('.');
                                    var type = cts[1].ToString();
                                    var properties = component["properties"] as JObject;

                                    //add name 
                                    if (type.ToLower() == "jqdatagrid")
                                        datagridlist.Add(new object[] { id, properties });
                                    if (type.ToLower() == "jqdialog")
                                        getHTMLChildren((JObject)component, datagridlist, dataformlist, defaultslist, validatelist);
                                }
                                if (datagridlist.Count > 0)
                                {
                                    for (int i = 0; i < datagridlist.Count; i++)
                                    {
                                        JObject datagrid = (datagridlist[i] as object[])[1] as JObject;
                                        if ((datagrid["editDialog"] != null && datagrid["editDialog"].ToString() != "") || (datagrid["EditDialogID"] != null && datagrid["EditDialogID"].ToString() != ""))
                                        {
                                            firstdatagridid = datagrid["ID"].ToString();
                                            break;
                                        }
                                    }
                                }

                                byte[] image2 = PrintScreen(image);

                                string pagename = dr["PageName"].ToString();
                                l.Add(pagename);
                                //l.Add(image);
                                l.Add(image2);
                                l.Add(xmlstring);
                                if (firstdatagridid != "")
                                {
                                    byte[] image3 = PrintScreen(image, firstdatagridid);
                                    l.Add(image3);
                                }
                                else
                                    l.Add(null);
                            }
                        }
                        else
                        {
                            ret += ",";
                            ret += "error";
                            continue;
                        }
                    }
                    else if (pagetype == "M")
                    {
                        l.Add(printsetting);
                        if (detail.Tables[0].Rows.Count > 0 && detail.Tables[0].Rows[0][0].ToString() != string.Empty)
                        {
                            foreach (DataRow dr in detail.Tables[0].Rows)
                            {
                                byte[] xmlstring = dr["Document"] as byte[];
                                byte[] image = dr["Photo"] as byte[];
                                //photo栏位存预览地址了

                                byte[] image2 = PrintScreen(image);
                                string pagename = dr["PageName"].ToString();
                                l.Add(pagename);
                                //l.Add(image);
                                l.Add(image2);
                                l.Add(xmlstring);
                            }
                        }
                        else
                        {
                            ret += ",";
                            ret += "error";
                            continue;
                        }
                    }
                    else if (pagetype == "R")
                    {
                        l.Add(printsetting);
                        if (detail.Tables[0].Rows.Count > 0 && detail.Tables[0].Rows[0][0].ToString() != string.Empty)
                        {
                            foreach (DataRow dr in detail.Tables[0].Rows)
                            {
                                byte[] xmlstring = dr["Document"] as byte[];
                                byte[] image = dr["Photo"] as byte[];
                                //photo栏位存预览地址了

                                byte[] image2 = PrintScreen(image);
                                string pagename = dr["PageName"].ToString();
                                l.Add(pagename);
                                //l.Add(image);
                                l.Add(image2);
                                l.Add(xmlstring);
                            }
                        }
                        else
                        {
                            ret += ",";
                            ret += "error";
                            continue;
                        }
                    }
                    else if (pagetype == "O")
                    {
                        l.Add(printsetting);
                        if (detail.Tables[0].Rows.Count > 0 && detail.Tables[0].Rows[0][0].ToString() != string.Empty)
                        {
                            foreach (DataRow dr in detail.Tables[0].Rows)
                            {
                                byte[] xmlstring = dr["Document"] as byte[];
                                byte[] image = dr["Photo"] as byte[];
                                //photo栏位存预览地址了

                                byte[] image2 = PrintScreen(image);
                                string pagename = dr["PageName"].ToString();
                                l.Add(pagename);
                                //l.Add(image);
                                l.Add(image2);
                                l.Add(xmlstring);
                            }
                        }
                        else
                        {
                            ret += ",";
                            ret += "error";
                            continue;
                        }
                    }

                    else if (pagetype == "S")
                    {
                        l.Add(printsetting);
                        if (detail.Tables[0].Rows.Count > 0 && detail.Tables[0].Rows[0][0].ToString() != string.Empty)
                        {
                            foreach (DataRow dr in detail.Tables[0].Rows)
                            {
                                byte[] xmlstring = dr["Document"] as byte[];
                                string pagename = dr["PageName"].ToString();
                                l.Add(pagename);
                                l.Add(xmlstring);
                            }
                        }
                        else
                        {
                            ret += ",";
                            ret += "error";
                            continue;
                        }
                    }
                    else if (pagetype == "G")
                    {
                        l.Add(printsetting);
                        if (detail.Tables[0].Rows.Count > 0 && detail.Tables[0].Rows[0][0].ToString() != string.Empty)
                        {
                            foreach (DataRow dr in detail.Tables[0].Rows)
                            {
                                byte[] xmlstring = dr["Document"] as byte[];
                                byte[] image = dr["Photo"] as byte[];
                                //photo栏位存预览地址了

                                byte[] image2 = PrintScreen(image);
                                string pagename = dr["PageName"].ToString();
                                l.Add(pagename);
                                //l.Add(image);
                                l.Add(image2);
                                l.Add(xmlstring);
                            }
                        }
                        else
                        {
                            ret += ",";
                            ret += "error";
                            continue;
                        }
                    }
                    else if (pagetype == "T")
                    {
                        if (detail.Tables[0].Rows.Count > 0 && detail.Tables[0].Rows[0][0].ToString() != string.Empty)
                        {
                            DataRow dr = detail.Tables[0].Rows[0];
                            byte[] xmlstring = dr["Document"] as byte[];
                            l.Add(xmlstring);
                        }
                        else
                        {
                            ret += ",";
                            ret += "error";
                            continue;
                        }
                    }
                    //EEPRemoteModule packageService = Activator.GetObject(typeof(EEPRemoteModule),
                    //            string.Format("http://{0}:{1}/InfoRemoteModule.rem", "192.168.1.100", "8989")) as EEPRemoteModule;
                    //object[] returnobj = packageService.CallMethod(GetClientInfo(), "SDModuleUserServer", "RemotePrint", l.ToArray());
                    object[] returnobj = null;
                    if (pagetype == "C" || pagetype == "I")
                    {
                        //build apk
                        if (detail.Tables[0].Rows.Count > 0 && detail.Tables[0].Rows[0][0].ToString() != string.Empty)
                        {
                            DataRow dr = detail.Tables[0].Rows[0];
                            byte[] xmlstring = dr["Document"] as byte[];
                            var settings = Encoding.UTF8.GetString(xmlstring).Split(';');
                            var cordovaPath = settings[0];
                            var folder = settings[1];
                            var targetPath = settings[2];
                            var webClient = settings[3];
                            var developerID = settings[4];
                            var database = settings[5];
                            var solution = settings[6];
                            var iosOptions = settings[7];
                            

                            CopyFiles(cordovaPath, targetPath);
                            CopyFiles(folder, Path.Combine(targetPath, "www", developerID));

                            //setting 
                            var jsFile = Path.Combine(targetPath, @"www\js\jquery.infolight.mobile.js");
                            var js = string.Empty;
                            using (var reader = new StreamReader(jsFile, true))
                            {
                                js = reader.ReadToEnd();
                            }
                            var line = js.Split('\r')[0];
                            if (line.StartsWith("var webSiteUrl ="))
                            {
                                js = js.Replace(line, string.Format("var webSiteUrl = '{0}';var developerID='{1}';var database='{2}';var solution='{3}'", webClient, developerID, database, solution));
                                using (var writer = new StreamWriter(jsFile, false, new System.Text.UTF8Encoding(true)))
                                {
                                    writer.Write(js);
                                }
                            }
                            CopyFiles(Path.Combine(targetPath, "www"), Path.Combine(targetPath, @"platforms\android\assets\www"));
                            var batPath = Path.Combine(targetPath, @"platforms\android\cordova\build.bat");
                            if (pagetype == "I")
                            {
                                var buildurl = iosOptions.Split(',')[0];
                                batPath = Path.Combine(targetPath, "ios.bat");
                                //create bat
                                using (var writer = new StreamWriter(batPath, false, new System.Text.ASCIIEncoding()))
                                {
                                    writer.WriteLine("set current_dir={0}", targetPath);
                                    writer.WriteLine("pushd %current_dir%");
                                    writer.WriteLine("node \"{0}\\npm\\node_modules\\vs-tac\\app.js\" build --platform \"iOS\" --configuration \"Release\" --projectDir . --projectName \"EEPApp\" --language \"en-US\" --buildServerUrl \"{1}\" --buildTarget \"iOSLocalDevice\""
                                        , Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), buildurl);
                                    //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                                }
                            }

                            System.Diagnostics.Process pro = new System.Diagnostics.Process();
                            //var batPath = Path.Combine(targetPath, @"platforms\android\cordova\build.bat");
                            FileInfo file = new FileInfo(batPath);
                            pro.StartInfo.WorkingDirectory = file.Directory.FullName;
                            pro.StartInfo.FileName = batPath;
                            pro.StartInfo.CreateNoWindow = true;
                            pro.Start();
                            pro.WaitForExit();
                            Directory.Delete(folder, true);
                            if (pagetype == "I")
                            {
                                //var iosFolder = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                //var iosUrl = iosOptions.Split(',')[1];
                                //var iosPUrl = iosOptions.Split(',')[2];
                                //modify plist
                                //using (var reader = new StreamReader(Path.Combine(targetPath, @"bin\iOS\Release\EEPApp.plist"), true))
                                //{
                                //     var xml = reader.ReadToEnd();
                                //     using (var writer = new StreamWriter(Path.Combine(targetPath, @"bin\iOS\Release\EEPApp.plist"), false, new System.Text.UTF8Encoding(true)))
                                //     {
                                //         writer.Write(xml.Replace("<string>http://${YOUR_DOMAIN_DOTCOM}/${PATH_TO_BETA_IF_ANY}/${APPLICATION_NAME}.ipa</string>", Path.Combine(iosUrl, iosFolder, "EEPApp.ipa")));
                                //     }
                                //}
                              
                                PackageFolder(Path.Combine(targetPath, @"bin\iOS\Release"), Path.Combine(targetPath, @"bin\iOS\EEPApp.zip"));


                                var iosUrl = "https://www.infolight.com/ios";
                              
                                //modify plist
                                var xml = string.Empty;
                                using (var reader = new StreamReader(Path.Combine(targetPath, @"bin\iOS\Release", @"EEPApp.plist"), true))
                                {
                                    xml = reader.ReadToEnd();
                                }
                                using (var writer = new StreamWriter(Path.Combine(targetPath, @"bin\iOS\Release", @"EEPApp.plist"), false, new System.Text.UTF8Encoding(true)))
                                {
                                    writer.Write(xml.Replace("http://${YOUR_DOMAIN_DOTCOM}/${PATH_TO_BETA_IF_ANY}/${APPLICATION_NAME}.ipa", iosUrl + "/app/" + developerID + "/EEPApp.ipa"));
                                }
                                System.Net.WebClient client = new System.Net.WebClient();
                                client.UploadFile(string.Format("{0}/iosAppPublish.ashx?UserID={1}", iosUrl, developerID), Path.Combine(targetPath, @"bin\iOS\Release", @"EEPApp.plist"));
                                client.UploadFile(string.Format("{0}/iosAppPublish.ashx?UserID={1}", iosUrl, developerID), Path.Combine(targetPath, @"bin\iOS\Release", @"EEPApp.ipa"));


                                var ipa = File.ReadAllBytes(Path.Combine(targetPath, @"bin\iOS\EEPApp.zip"));
                                returnobj = new object[] { ipa, "eep", "C", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                            }
                            else
                            {
                                var apk = File.ReadAllBytes(Path.Combine(targetPath, @"platforms\android\ant-build\MainActivity-debug-unaligned.apk"));
                                //Directory.Delete(targetPath, true);
                                returnobj = new object[] { apk, "eep", "C", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                            }
                        }
                    }
                    else
                    {
                        returnobj = RemotePrint(l.ToArray()) as object[];
                    }
                    if (returnobj.Length > 2)
                    {
                        byte[] filebyte = returnobj[0] as byte[];
                        string filename = returnobj[1].ToString();
                        string filetype = returnobj[2].ToString();
                        string dt = returnobj[3].ToString();
                        IDbConnection connection2 = null;
                        IDbTransaction transaction2 = null;
                        string databasename = string.Empty;
                        string dbAlias = string.Empty;

                        try
                        {
                            dbAlias = DbConnectionSet.GetSystemDatabase(null);
                            connection2 = Srvtools.DbConnectionSet.WaitForConnection(Alias, "", ref ct, userid, this.GetType().FullName, DateTime.Now);
                            transaction2 = connection2.BeginTransaction();
                            IDbCommand cm = connection2.CreateCommand();
                            String sFile = "@file";
                            IDataParameter spFile = cm.CreateParameter();
                            if (connection.GetType().Name == "SqlConnection")
                            {
                                sFile = "@file";
                                (spFile as SqlParameter).SqlDbType = SqlDbType.Image;
                            }
                            else if (connection.GetType().Name == "OleDbConnection")
                            {
                                sFile = "?";
                                (spFile as OleDbParameter).OleDbType = OleDbType.Binary;
                            }
                            else if (connection.GetType().Name == "OracleConnection")
                            {
                                sFile = ":file";
                                (spFile as OracleParameter).OracleType = OracleType.Blob;
                            }
                            else if (connection.GetType().Name == "OdbcConnection")
                            {
                                sFile = "?";
                                (spFile as OdbcParameter).OdbcType = OdbcType.Binary;
                            }
                            else if (connection.GetType().Name == "MySqlConnection")
                            {
                                sFile = "@file";
                                spFile.DbType = DbType.Binary;
                            }
                            else if (connection.GetType().Name == "Ifxonnection")
                            {
                                sFile = "@file";
                                spFile.DbType = DbType.Binary;
                            }
                            else if (connection.GetType().Name == "AseConnection")
                            {
                                sFile = "@file";
                                spFile.DbType = DbType.Binary;
                            }
                            spFile.ParameterName = sFile;
                            spFile.Value = filebyte;
                            cm.Transaction = transaction2;
                            cm.Parameters.Add(spFile);

                            string strSql = "update SYS_SDQUEUE set Document = " + sFile + " , FileName='" + filename + "' , FinishTime='" + dt + "' , FinishFlag =1 where ID='" + id + "'";
                            cm.CommandText = strSql;
                            cm.ExecuteNonQuery();

                            transaction2.Commit();
                            ret += ",";
                            ret += "Success";
                        }
                        catch (Exception ex)
                        {
                            ret += ",";
                            ret += ex.Message;
                            transaction2.Rollback();
                            WriteError(ex.Message, "PrintSchedul");
                        }
                        finally
                        {
                            transaction2.Dispose();
                            DbConnectionSet.ReleaseConnection(Alias, "", connection2);
                        }
                    }
                    else
                    {
                        ret += ",";
                        ret += returnobj[1].ToString();
                    }
                    System.Threading.Thread.Sleep(10000);
                }
            }
            catch (Exception exception)
            {
                ret += ","; ret += exception.Message;
                WriteError(exception.Message, "PrintSchedulFinally");
            }
            finally
            {
                transaction.Dispose();
                DbConnectionSet.ReleaseConnection(Alias, "", connection);
            }
            return new object[] { 0, ret };
        }

        private void CopyFiles(string sourcePath, string targetPath)
        {
            if (System.IO.Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                var files = System.IO.Directory.GetFiles(sourcePath);

                string[] filesArray = Directory.GetFileSystemEntries(sourcePath);
                foreach (string file in filesArray)
                {
                    if (Directory.Exists(file))                     //如果当前是文件夹，递归
                    {
                        var dirName = System.IO.Path.GetFileName(file);
                        CopyFiles(file, Path.Combine(targetPath, dirName));
                    }
                    else
                    {
                        var fileName = System.IO.Path.GetFileName(file);
                        var destFile = System.IO.Path.Combine(targetPath, fileName);
                        System.IO.File.Copy(file, destFile, true);
                    }
                }
            }
        }

        private bool PackageFolder(string folderName, string compressedFileName)
        {
            if (folderName.EndsWith(@"\"))
                folderName = folderName.Remove(folderName.Length - 1);
            bool result = false;
            if (!Directory.Exists(folderName))
            {
                return result;
            }
         
            try
            {
                using (Package package = Package.Open(compressedFileName, FileMode.Create))
                {
                    var fileList = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);
                    foreach (string fileName in fileList)
                    {

                        //The path in the package is all of the subfolders after folderName
                        string pathInPackage;
                        pathInPackage = Path.GetDirectoryName(fileName).Replace(folderName, string.Empty) + "/" + Path.GetFileName(fileName);

                        Uri partUriDocument = PackUriHelper.CreatePartUri(new Uri(pathInPackage, UriKind.Relative));
                        PackagePart packagePartDocument = package.CreatePart(partUriDocument, "", CompressionOption.Maximum);
                        using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                        {
                            fileStream.CopyTo(packagePartDocument.GetStream());
                        }
                    }
                }
                result = true;
            }
            catch (Exception e)
            {
                throw new Exception("Error zipping folder " + folderName, e);
            }

            return result;
        }

        private byte[] PrintScreen(byte[] options)
        {
            return PrintScreen(options, "");
        }
        private byte[] PrintScreen(byte[] options, string datagridid)
        {
            //Shell32.ShellClass slc = new Shell32.ShellClass();
            //slc.MinimizeAll(); // Win+M 
            ////sc.UnminimizeAll(); // Shift+Win+M 
            //System.Windows.Forms.Application.DoEvents();
            //System.Threading.Thread.Sleep(1000);

            ////List<int> ieProcess = new List<int>();
            ////foreach (Process ps1 in System.Diagnostics.Process.GetProcessesByName("iexplore"))
            ////{
            ////    ieProcess.Add(ps1.Id);
            ////}

            string url = System.Text.Encoding.UTF8.GetString(options);
            ////getsinglesignon
            //Process ps = new Process();
            //ps.StartInfo.FileName = "iexplore.exe";
            //ps.StartInfo.Arguments = url;
            ////ps.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            //ps.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            //ps.Start();
            ////ps.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

            ////cut image
            //int PrintWaitingTime = 10;
            //IntPtr IEHandle = ps.Handle;
            //System.Windows.Forms.Application.DoEvents();
            //System.Threading.Thread.Sleep(1000);

            //foreach (Process ps1 in System.Diagnostics.Process.GetProcessesByName("iexplore"))
            //{
            //    if (ps1.Id == ps.Id)
            //    {
            //        IEHandle = ps1.Handle;
            //    }
            //}


            //ShowWindow(IEHandle, 3);
            ////SetActiveWindow(IEHandle);
            ////SetForegroundWindow(IEHandle);

            //System.Threading.Thread.Sleep(PrintWaitingTime * 1000);

            //Srvtools.ScreenCapture SC = new ScreenCapture();
            //StringBuilder SB = new StringBuilder(200);
            //MemoryStream ms = SC.CaptureScreenToStream(System.Drawing.Imaging.ImageFormat.Png);         
            //byte[] byteImage = new Byte[ms.Length];
            //byteImage = ms.ToArray();
            //end

            //System.Drawing.Rectangle rec = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            //Bitmap bmp = new Bitmap(rec.Width, rec.Height-20);
            //Graphics gra = Graphics.FromImage(bmp);
            //gra.CopyFromScreen(0, 0, 0, 0, new Size(rec.Width, rec.Height));
            //PostMessage(IEHandle, 16, 0, 0);
            //MemoryStream ms = new MemoryStream();
            //bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            //byte[] byteImage = new Byte[ms.Length];
            //byteImage = ms.ToArray();

            //PostMessage(IEHandle2, 16, 0, 0);
            //ps.Close();
            //ps.Kill();
            //foreach (Process ps1 in System.Diagnostics.Process.GetProcessesByName("iexplore"))
            //{
            //    if (!ieProcess.Contains(ps1.Id))
            //    {
            //        ps1.Kill();
            //        ps1.WaitForExit(1000);
            //    }
            //}
            //ps.Kill();
            //ps.WaitForExit(5000);

            //SqlParameter spFile = new SqlParameter("@file", SqlDbType.Image);
            //spFile.Value = byteImage;
            //SqlCommand cm = connection.CreateCommand() as SqlCommand;
            //cm.Transaction = transaction as SqlTransaction;
            //cm.Parameters.Add(spFile);

            //strSql = "update SYS_SDQUEUEPAGE set Photo = @file where ID='" + id + "'";
            //cm.CommandText = strSql;
            //cm.ExecuteNonQuery();
            //ms.Close();
            Bitmap m_Bitmap = WebSiteThumbnail.GetWebSiteThumbnail(url, 800, 600, 800, 600, datagridid);
            //var FileName2 = @"C:\EEPCloud\" + Path.GetRandomFileName() + ".jpg";
            //using (FileStream ms2 = new FileStream(FileName2, FileMode.Create))
            //{
            //    m_Bitmap.Save(ms2, System.Drawing.Imaging.ImageFormat.Png);//JPG、GIF、PNG等均可 
            //}
            MemoryStream ms2 = new MemoryStream();
            m_Bitmap.Save(ms2, System.Drawing.Imaging.ImageFormat.Png);//JPG、GIF、PNG等均可 
            var byteImage = new Byte[ms2.Length];
            byteImage = ms2.ToArray();

            return byteImage;
        }

        #region Remote Print
        readonly string PrintTempFilePath = EEPRegistry.Server + @"\SDModuleFile\";
        public object RemotePrint(object[] paramters)
        {
            Microsoft.Office.Interop.Word._Application oWord;
            Microsoft.Office.Interop.Word._Document oDoc;
            string filepath = string.Empty;
            string filename = string.Empty;
            string filetype = string.Empty;
            DateTime dt;
            Selection selection;
            if (!Directory.Exists(PrintTempFilePath))//如果不存在就创建Log文件夹
            {
                Directory.CreateDirectory(PrintTempFilePath);
            }
            try
            {
                oWord = new Microsoft.Office.Interop.Word.Application();
            }
            catch (Exception ex)
            {
                WriteError(ex.Message, "RemotePrint1");
                return null;
            }
            try
            {
                if (((string)paramters[1]) == "P")
                {
                    #region Print Page
                    string printSettingString = paramters[2] as string;
                    string[] printSetting = printSettingString.Split(',');
                    List<string> printName = new List<string>();
                    List<byte[]> printImage = new List<byte[]>();
                    List<string> printXaml = new List<string>();
                    for (int i = 3; i < paramters.Length; i = i + 3)
                    {
                        printName.Add(paramters[i].ToString());
                        printImage.Add((byte[])paramters[i + 1]);
                        string pagexml = System.Text.Encoding.UTF8.GetString(paramters[i + 2] as byte[], 0, (paramters[i + 2] as byte[]).Length);
                        printXaml.Add(pagexml);
                    }

                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                                ref oMissing, ref oMissing);
                    object type = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
                    //oWord.Visible = true;
                    selection = oWord.Selection;
                    selection.PageSetup.LeftMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.PageSetup.RightMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.set_Style(ref oHeadingStyle1);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("Page List");
                    selection.TypeParagraph();

                    for (int i = 0; i < printName.Count; i++)
                    {
                        Byte[] imagebyte = printImage[i] as byte[];
                        string pagexml = printXaml[i] as string;

                        selection.set_Style(ref oHeadingStyle1);
                        selection.Font.Bold = 2;
                        selection.Font.Size = 14;
                        selection.TypeText(printName[i].ToString());
                        selection.TypeParagraph();
                        string name = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                        string imagepath = PrintTempFilePath + @"\image" + name + ".jpg";
                        if (imagebyte != null)
                        {
                            MemoryStream ms = new MemoryStream(i);

                            if (File.Exists(imagepath))
                            {
                                File.Delete(imagepath);
                            }

                            FileStream fs = File.Create(imagepath);
                            fs.Write(imagebyte, 0, imagebyte.Length);
                            //fs.Flush();
                            fs.Close();
                            //img = Drawing.Image.FromStream(ms);

                        }
                        try
                        {
                            string csJPGFile = imagepath;
                            object LinkToFile = false;
                            object SaveWithDocument = true;
                            selection.InlineShapes.AddPicture(csJPGFile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                        }
                        catch { }
                        selection.TypeParagraph();
                        selection.InsertBreak(ref type);
                        LoadPage(pagexml, selection, oWord, oDoc, printSetting);
                        selection.InsertBreak(ref type);
                    }
                    selection.HomeKey(ref Wdstory, ref oMissing);
                    object level3 = 3;
                    oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                    oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                    dt = DateTime.Now;
                    filename = "SilverlightPage" + dt.ToString("yyyyMMddhhmmssfff");
                    object path = PrintTempFilePath + @"Page" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    filepath = (string)path;
                    filetype = "P";
                    #endregion
                }
                else if (((string)paramters[1]) == "S")
                {
                    #region Print Service
                    string printSettingString = paramters[2] as string;
                    string[] printSetting = printSettingString.Split(',');
                    List<string> printName = new List<string>();
                    List<string> printXaml = new List<string>();
                    for (int i = 3; i < paramters.Length; i = i + 2)
                    {
                        printName.Add(paramters[i].ToString());
                        string pagexml = System.Text.Encoding.UTF8.GetString(paramters[i + 1] as byte[], 0, (paramters[i + 1] as byte[]).Length);
                        printXaml.Add(pagexml);
                    }

                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                                ref oMissing, ref oMissing);
                    object type = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
                    //oWord.Visible = true;
                    selection = oWord.Selection;
                    selection.PageSetup.LeftMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.PageSetup.RightMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.set_Style(ref oHeadingStyle1);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("Service  List");
                    selection.TypeParagraph();

                    for (int i = 0; i < printName.Count; i++)
                    {
                        string pagexml = printXaml[i] as string;

                        selection.set_Style(ref oHeadingStyle1);
                        selection.Font.Bold = 2;
                        selection.Font.Size = 14;
                        selection.TypeText(printName[i].ToString());

                        selection.TypeParagraph();
                        LoadService2(pagexml, selection, oWord, oDoc, printSetting);
                        selection.InsertBreak(ref type);
                    }
                    selection.HomeKey(ref Wdstory, ref oMissing);
                    object level3 = 3;
                    oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                    oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                    dt = DateTime.Now;
                    filename = "Service" + dt.ToString("yyyyMMddhhmmssfff");
                    object path = PrintTempFilePath + @"Service" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    filepath = (string)path;
                    filetype = "S";
                    #endregion
                }
                else if (((string)paramters[1]) == "T")
                {
                    #region Print Web Page
                    //string printXaml = paramters[2].ToString();
                    string printXaml = System.Text.Encoding.UTF8.GetString(paramters[2] as byte[], 0, (paramters[2] as byte[]).Length);

                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                                ref oMissing, ref oMissing);
                    object type = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
                    //oWord.Visible = true;
                    selection = oWord.Selection;
                    selection.PageSetup.LeftMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.PageSetup.RightMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.set_Style(ref oHeadingStyle1);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("Table List");
                    selection.TypeParagraph();

                    LoadTable2(printXaml, selection, oWord, oDoc);
                    selection.InsertBreak(ref type);

                    selection.HomeKey(ref Wdstory, ref oMissing);
                    object level3 = 3;
                    oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                    oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                    dt = DateTime.Now;
                    filename = "TableSchema" + dt.ToString("yyyyMMddhhmmssfff");
                    object path = PrintTempFilePath + @"Table" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    filepath = (string)path;
                    filetype = "W";
                    #endregion
                }
                else if (((string)paramters[1]) == "W")
                {
                    #region Print Web Page
                    string printSettingString = paramters[2] as string;
                    string[] printSetting = printSettingString.Split(',');
                    List<string> printName = new List<string>();
                    List<byte[]> printImage = new List<byte[]>();
                    List<string> printXaml = new List<string>();
                    //第二张图片
                    List<byte[]> printImage2 = new List<byte[]>();
                    for (int i = 3; i < paramters.Length; i = i + 4)
                    {
                        printName.Add(paramters[i].ToString());
                        printImage.Add((byte[])paramters[i + 1]);
                        string pagexml = System.Text.Encoding.UTF8.GetString(paramters[i + 2] as byte[], 0, (paramters[i + 2] as byte[]).Length);
                        printXaml.Add(pagexml);
                        printImage2.Add(paramters[i + 3] != null ? (byte[])paramters[i + 3] : null);
                    }

                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                                ref oMissing, ref oMissing);
                    object type = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
                    //oWord.Visible = true;
                    selection = oWord.Selection;
                    selection.PageSetup.LeftMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.PageSetup.RightMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.set_Style(ref oHeadingStyle1);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("Web Page List");
                    selection.TypeParagraph();

                    for (int i = 0; i < printName.Count; i++)
                    {
                        Byte[] imagebyte = printImage[i] as byte[];
                        string pagexml = printXaml[i] as string;
                        //第二张图
                        Byte[] imagebyte2 = printImage2[i] as byte[];

                        selection.set_Style(ref oHeadingStyle1);
                        selection.Font.Bold = 2;
                        selection.Font.Size = 14;
                        selection.TypeText(printName[i].ToString());
                        selection.TypeParagraph();
                        string name = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                        string imagepath = PrintTempFilePath + @"\image" + name + ".jpg";
                        if (imagebyte != null)
                        {
                            MemoryStream ms = new MemoryStream(i);

                            if (File.Exists(imagepath))
                            {
                                File.Delete(imagepath);
                            }

                            FileStream fs = File.Create(imagepath);
                            fs.Write(imagebyte, 0, imagebyte.Length);
                            //fs.Flush();
                            fs.Close();
                            //img = Drawing.Image.FromStream(ms);

                        }
                        try
                        {
                            string csJPGFile = imagepath;
                            object LinkToFile = false;
                            object SaveWithDocument = true;
                            selection.InlineShapes.AddPicture(csJPGFile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                        }
                        catch { }
                        if (imagebyte2 != null)
                        {
                            string name2 = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                            string imagepath2 = PrintTempFilePath + @"\image" + name2 + ".jpg";
                            if (imagebyte2 != null)
                            {
                                MemoryStream ms = new MemoryStream(i);

                                if (File.Exists(imagepath2))
                                {
                                    File.Delete(imagepath2);
                                }

                                FileStream fs = File.Create(imagepath2);
                                fs.Write(imagebyte2, 0, imagebyte2.Length);
                                //fs.Flush();
                                fs.Close();
                                //img = Drawing.Image.FromStream(ms);

                            }
                            try
                            {
                                string csJPGFile = imagepath2;
                                object LinkToFile = false;
                                object SaveWithDocument = true;
                                selection.InlineShapes.AddPicture(csJPGFile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                            }
                            catch { }

                        }
                        selection.TypeParagraph();
                        selection.InsertBreak(ref type);
                        LoadWebPage2(pagexml, selection, oWord, oDoc, printSetting);
                    }
                    selection.HomeKey(ref Wdstory, ref oMissing);
                    object level3 = 3;
                    oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                    oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                    dt = DateTime.Now;
                    filename = "HTMLPage" + dt.ToString("yyyyMMddhhmmssfff");
                    object path = PrintTempFilePath + @"Page" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    filepath = (string)path;
                    filetype = "W";
                    #endregion
                }
                else if (((string)paramters[1]) == "M")
                {
                    #region Print Mobile Page
                    string printSettingString = paramters[2] as string;
                    string[] printSetting = printSettingString.Split(',');
                    List<string> printName = new List<string>();
                    List<byte[]> printImage = new List<byte[]>();
                    List<string> printXaml = new List<string>();
                    for (int i = 3; i < paramters.Length; i = i + 3)
                    {
                        printName.Add(paramters[i].ToString());
                        printImage.Add((byte[])paramters[i + 1]);
                        string pagexml = System.Text.Encoding.UTF8.GetString(paramters[i + 2] as byte[], 0, (paramters[i + 2] as byte[]).Length);
                        printXaml.Add(pagexml);
                    }

                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                                ref oMissing, ref oMissing);
                    object type = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
                    //oWord.Visible = true;
                    selection = oWord.Selection;
                    selection.PageSetup.LeftMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.PageSetup.RightMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.set_Style(ref oHeadingStyle1);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("Mobile Page List");
                    selection.TypeParagraph();

                    for (int i = 0; i < printName.Count; i++)
                    {
                        Byte[] imagebyte = printImage[i] as byte[];
                        string pagexml = printXaml[i] as string;

                        selection.set_Style(ref oHeadingStyle1);
                        selection.Font.Bold = 2;
                        selection.Font.Size = 14;
                        selection.TypeText(printName[i].ToString());
                        selection.TypeParagraph();
                        string name = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                        string imagepath = PrintTempFilePath + @"\image" + name + ".jpg";
                        if (imagebyte != null)
                        {
                            MemoryStream ms = new MemoryStream(i);

                            if (File.Exists(imagepath))
                            {
                                File.Delete(imagepath);
                            }

                            FileStream fs = File.Create(imagepath);
                            fs.Write(imagebyte, 0, imagebyte.Length);
                            //fs.Flush();
                            fs.Close();
                            //img = Drawing.Image.FromStream(ms);

                        }
                        try
                        {
                            string csJPGFile = imagepath;
                            object LinkToFile = false;
                            object SaveWithDocument = true;
                            selection.InlineShapes.AddPicture(csJPGFile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                        }
                        catch { }
                        selection.TypeParagraph();
                        selection.InsertBreak(ref type);
                        LoadMobilePage2(pagexml, selection, oWord, oDoc, printSetting);
                    }
                    selection.HomeKey(ref Wdstory, ref oMissing);
                    object level3 = 3;
                    oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                    oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                    dt = DateTime.Now;
                    filename = "MobilePage" + dt.ToString("yyyyMMddhhmmssfff");
                    object path = PrintTempFilePath + @"MobilePage" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    filepath = (string)path;
                    filetype = "M";
                    #endregion
                }
                else if (((string)paramters[1]) == "R")
                {
                    #region Print Report
                    string printSettingString = paramters[2] as string;
                    string[] printSetting = printSettingString.Split(',');
                    List<string> printName = new List<string>();
                    List<byte[]> printImage = new List<byte[]>();
                    List<string> printXaml = new List<string>();
                    for (int i = 3; i < paramters.Length; i = i + 3)
                    {
                        printName.Add(paramters[i].ToString());
                        printImage.Add((byte[])paramters[i + 1]);
                        string pagexml = System.Text.Encoding.UTF8.GetString(paramters[i + 2] as byte[], 0, (paramters[i + 2] as byte[]).Length);
                        printXaml.Add(pagexml);
                    }

                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                                ref oMissing, ref oMissing);
                    object type = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
                    //oWord.Visible = true;
                    selection = oWord.Selection;
                    selection.PageSetup.LeftMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.PageSetup.RightMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.set_Style(ref oHeadingStyle1);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("Report List");
                    selection.TypeParagraph();

                    for (int i = 0; i < printName.Count; i++)
                    {
                        Byte[] imagebyte = printImage[i] as byte[];
                        string pagexml = printXaml[i] as string;

                        selection.set_Style(ref oHeadingStyle1);
                        selection.Font.Bold = 2;
                        selection.Font.Size = 14;
                        selection.TypeText(printName[i].ToString());
                        selection.TypeParagraph();
                        string name = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                        string imagepath = PrintTempFilePath + @"\image" + name + ".jpg";
                        if (imagebyte != null)
                        {
                            MemoryStream ms = new MemoryStream(i);

                            if (File.Exists(imagepath))
                            {
                                File.Delete(imagepath);
                            }

                            FileStream fs = File.Create(imagepath);
                            fs.Write(imagebyte, 0, imagebyte.Length);
                            //fs.Flush();
                            fs.Close();
                            //img = Drawing.Image.FromStream(ms);

                        }
                        try
                        {
                            string csJPGFile = imagepath;
                            object LinkToFile = false;
                            object SaveWithDocument = true;
                            selection.InlineShapes.AddPicture(csJPGFile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                        }
                        catch { }
                        selection.TypeParagraph();
                        selection.InsertBreak(ref type);
                        LoadReport2(pagexml, selection, oWord, oDoc, printSetting);
                    }
                    selection.HomeKey(ref Wdstory, ref oMissing);
                    object level3 = 3;
                    oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                    oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                    dt = DateTime.Now;
                    filename = "Report" + dt.ToString("yyyyMMddhhmmssfff");
                    object path = PrintTempFilePath + @"Report" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    filepath = (string)path;
                    filetype = "R";

                    #endregion
                }
                else if (((string)paramters[1]) == "O")
                {
                    #region Print Flow
                    string printSettingString = paramters[2] as string;
                    string[] printSetting = printSettingString.Split(',');
                    List<string> printName = new List<string>();
                    List<byte[]> printImage = new List<byte[]>();
                    List<string> printXaml = new List<string>();
                    for (int i = 3; i < paramters.Length; i = i + 3)
                    {
                        printName.Add(paramters[i].ToString());
                        printImage.Add((byte[])paramters[i + 1]);
                        string pagexml = System.Text.Encoding.UTF8.GetString(paramters[i + 2] as byte[], 0, (paramters[i + 2] as byte[]).Length);
                        printXaml.Add(pagexml);
                    }

                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                                ref oMissing, ref oMissing);
                    object type = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
                    //oWord.Visible = true;
                    selection = oWord.Selection;
                    selection.PageSetup.LeftMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.PageSetup.RightMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.set_Style(ref oHeadingStyle1);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("Flow List");
                    selection.TypeParagraph();

                    for (int i = 0; i < printName.Count; i++)
                    {
                        Byte[] imagebyte = printImage[i] as byte[];
                        string pagexml = printXaml[i] as string;

                        selection.set_Style(ref oHeadingStyle1);
                        selection.Font.Bold = 2;
                        selection.Font.Size = 14;
                        selection.TypeText(printName[i].ToString());
                        selection.TypeParagraph();
                        string name = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                        string imagepath = PrintTempFilePath + @"\image" + name + ".jpg";
                        if (imagebyte != null)
                        {
                            MemoryStream ms = new MemoryStream(i);

                            if (File.Exists(imagepath))
                            {
                                File.Delete(imagepath);
                            }

                            FileStream fs = File.Create(imagepath);
                            fs.Write(imagebyte, 0, imagebyte.Length);
                            //fs.Flush();
                            fs.Close();
                            //img = Drawing.Image.FromStream(ms);

                        }
                        try
                        {
                            string csJPGFile = imagepath;
                            object LinkToFile = false;
                            object SaveWithDocument = true;
                            selection.InlineShapes.AddPicture(csJPGFile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                        }
                        catch { }
                        selection.TypeParagraph();
                        selection.InsertBreak(ref type);
                        LoadFlow2(pagexml, selection, oWord, oDoc, printSetting, printName[i].ToString());
                    }
                    selection.HomeKey(ref Wdstory, ref oMissing);
                    object level3 = 3;
                    oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                    oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                    dt = DateTime.Now;
                    filename = "Flow" + dt.ToString("yyyyMMddhhmmssfff");
                    object path = PrintTempFilePath + @"Flow" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    filepath = (string)path;
                    filetype = "O";

                    #endregion
                }
                else if (((string)paramters[1]) == "G")
                {
                    #region Print Diagram
                    string printSettingString = paramters[2] as string;
                    string[] printSetting = printSettingString.Split(',');
                    List<string> printName = new List<string>();
                    List<byte[]> printImage = new List<byte[]>();
                    List<string> printXaml = new List<string>();
                    for (int i = 3; i < paramters.Length; i = i + 3)
                    {
                        printName.Add(paramters[i].ToString());
                        printImage.Add((byte[])paramters[i + 1]);
                        string pagexml = System.Text.Encoding.UTF8.GetString(paramters[i + 2] as byte[], 0, (paramters[i + 2] as byte[]).Length);
                        printXaml.Add(pagexml);
                    }
                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                                ref oMissing, ref oMissing);
                    object type = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
                    //oWord.Visible = true;
                    selection = oWord.Selection;
                    selection.PageSetup.LeftMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.PageSetup.RightMargin = oWord.CentimetersToPoints((float)1.5);
                    selection.set_Style(ref oHeadingStyle1);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("Diagram  List");
                    selection.TypeParagraph();

                    for (int i = 0; i < printName.Count; i++)
                    {
                        Byte[] imagebyte = printImage[i] as byte[];
                        string pagexml = printXaml[i] as string;

                        selection.set_Style(ref oHeadingStyle1);
                        selection.Font.Bold = 2;
                        selection.Font.Size = 14;
                        selection.TypeText(printName[i].ToString());
                        selection.TypeParagraph();
                        string name = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                        string imagepath = PrintTempFilePath + @"\image" + name + ".jpg";
                        if (imagebyte != null)
                        {
                            MemoryStream ms = new MemoryStream(i);

                            if (File.Exists(imagepath))
                            {
                                File.Delete(imagepath);
                            }

                            FileStream fs = File.Create(imagepath);
                            fs.Write(imagebyte, 0, imagebyte.Length);
                            //fs.Flush();
                            fs.Close();
                            //img = Drawing.Image.FromStream(ms);

                        }
                        try
                        {
                            string csJPGFile = imagepath;
                            object LinkToFile = false;
                            object SaveWithDocument = true;
                            selection.InlineShapes.AddPicture(csJPGFile, ref LinkToFile, ref SaveWithDocument, ref oMissing);
                        }
                        catch { }
                        selection.TypeParagraph();
                        selection.InsertBreak(ref type);
                        LoadDiagram2(pagexml, selection, oWord, oDoc, printSetting);
                    }
                    selection.HomeKey(ref Wdstory, ref oMissing);
                    object level3 = 3;
                    oDoc.TablesOfContents.Add(selection.Range, ref oTrue, ref WDCount1, ref level3, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
                    oDoc.TablesOfContents[1].TabLeader = WdTabLeader.wdTabLeaderDots;

                    dt = DateTime.Now;
                    filename = "Diagram" + dt.ToString("yyyyMMddhhmmssfff");
                    object path = PrintTempFilePath + @"Service" + dt.ToString("yyyyMMddhhmmssfff") + ".doc";
                    oDoc.SaveAs(ref path, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    filepath = (string)path;
                    filetype = "S";
                    #endregion
                }
                else
                    return new object[] { 1, "Error" };
            }
            catch (Exception ex)
            {
                WriteError(ex.Message, "RemotePrint2");
                return new object[] { 1, ex.Message };
            }
            finally
            {
                object closesave = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
                object option = Microsoft.Office.Interop.Word.WdOriginalFormat.wdWordDocument;
                object f = false;
                oWord.Quit(ref closesave, ref option, ref f);
                System.Threading.Thread.Sleep(2000);
                //oWord.Quit(ref closesave, ref option, ref f);
            }
            if (filename != string.Empty)
            {
                byte[] filebyte = File.ReadAllBytes((string)filepath);
                return new object[] { filebyte, filename, filetype, dt.ToString("yyyy-MM-dd HH:mm:ss") };
            }
            else
            {
                WriteError("No Print Success", "RemotePrint3");
                return new object[] { 1, "No Print Success" };
            }
        }
        public void LoadService(string xaml, Selection selection, Microsoft.Office.Interop.Word._Application oWord,
    Microsoft.Office.Interop.Word._Document oDoc, object[] printSetting)
        {
            XmlDocument xml = new XmlDocument();
            xaml = System.Text.RegularExpressions.Regex.Replace(xaml, "^[^<]+", "");
            TextReader tr = new StringReader(xaml);
            xml.Load(tr);
            if (xml.SelectSingleNode("WrapPanel") != null)
            {
                XmlNode children = xml.SelectSingleNode("WrapPanel").SelectSingleNode("Children");
                if (children != null)
                {
                    #region SDCommand
                    //if ((String)printSetting[0] == "true")
                    //{
                    try
                    {
                        XmlNodeList SDCommandList = children.SelectNodes("SDCommand");
                        if (SDCommandList != null && SDCommandList.Count > 0)
                        {
                            selection.TypeText("【SDCommand】");
                            selection.Font.Bold = 0;
                            selection.Font.Size = 10;
                            selection.TypeParagraph();

                            Table GridViewTable = oDoc.Tables.Add(selection.Range, SDCommandList.Count + 1, 3, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "CommandText";
                            GridViewTable.Cell(1, 3).Range.Text = "KeyFields";
                            //}
                            for (int j = 0; j < SDCommandList.Count; j++)
                            {
                                XmlNode co = (SDCommandList[j] as XmlNode).SelectSingleNode("Name");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 1).Range.Text = co.InnerText;
                                co = (SDCommandList[j] as XmlNode).SelectSingleNode("CommandText");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 2).Range.Text = co.InnerText;
                                co = (SDCommandList[j] as XmlNode).SelectSingleNode("KeyFields");
                                if (co != null)
                                {
                                    XmlNodeList keylist = co.SelectNodes("KeyItem");
                                    if (keylist != null && keylist.Count > 0)
                                    {
                                        string keyfieldsstring = "";
                                        foreach (XmlNode key in keylist)
                                        {
                                            if (key.FirstChild != null)
                                            {
                                                if (keyfieldsstring != "")
                                                    keyfieldsstring += ",";
                                                keyfieldsstring += key.FirstChild.InnerText;
                                            }
                                        }
                                        GridViewTable.Cell(j + 2, 3).Range.Text = keyfieldsstring;
                                    }
                                }
                            }
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                    catch (Exception e)
                    {
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeText(e.Message);
                        selection.TypeParagraph();
                    }
                    //}
                    #endregion

                    #region SDUpdateComponent
                    //if ((String)printSetting[1] == "true")
                    //{
                    try
                    {
                        XmlNodeList SDUpdateComponentList = children.SelectNodes("SDUpdateComponent");
                        if (SDUpdateComponentList != null && SDUpdateComponentList.Count > 0)
                        {
                            selection.TypeText("【SDUpdateComponent】");
                            selection.Font.Bold = 0;
                            selection.Font.Size = 10;
                            selection.TypeParagraph();

                            Table GridViewTable = oDoc.Tables.Add(selection.Range, SDUpdateComponentList.Count + 1, 3, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "SelectCmd";
                            GridViewTable.Cell(1, 3).Range.Text = "ServerModify";
                            //}
                            for (int j = 0; j < SDUpdateComponentList.Count; j++)
                            {
                                XmlNode co = (SDUpdateComponentList[j] as XmlNode).SelectSingleNode("Name");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 1).Range.Text = co.InnerText;
                                co = (SDUpdateComponentList[j] as XmlNode).SelectSingleNode("SelectCmd");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 2).Range.Text = co.InnerText;
                                co = (SDUpdateComponentList[j] as XmlNode).SelectSingleNode("ServerModify");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 3).Range.Text = co.InnerText;

                            }
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                    catch (Exception e)
                    {
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeText(e.Message);
                        selection.TypeParagraph();
                    }
                    //}
                    #endregion

                    #region SDDataSource
                    //if ((String)printSetting[1] == "true")
                    //{
                    try
                    {
                        XmlNodeList SDDataSource = children.SelectNodes("SDDataSource");
                        if (SDDataSource != null && SDDataSource.Count > 0)
                        {
                            selection.TypeText("【SDDataSource】");
                            selection.Font.Bold = 0;
                            selection.Font.Size = 10;
                            selection.TypeParagraph();

                            Table GridViewTable = oDoc.Tables.Add(selection.Range, SDDataSource.Count + 1, 5, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "Master";
                            GridViewTable.Cell(1, 3).Range.Text = "MasterColumns";
                            GridViewTable.Cell(1, 4).Range.Text = "Detail";
                            GridViewTable.Cell(1, 5).Range.Text = "DetailColumns";
                            //}
                            for (int j = 0; j < SDDataSource.Count; j++)
                            {
                                XmlNode co = (SDDataSource[j] as XmlNode).SelectSingleNode("Name");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 1).Range.Text = co.InnerText;
                                co = (SDDataSource[j] as XmlNode).SelectSingleNode("Master");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 2).Range.Text = co.InnerText;
                                co = (SDDataSource[j] as XmlNode).SelectSingleNode("MasterColumns");
                                if (co != null)
                                {
                                    XmlNodeList ColumnItemlist = co.SelectNodes("ColumnItem");
                                    if (ColumnItemlist != null && ColumnItemlist.Count > 0)
                                    {
                                        string ColumnItemString = "";
                                        foreach (XmlNode columnItem in ColumnItemlist)
                                        {
                                            if (columnItem.FirstChild != null)
                                            {
                                                if (ColumnItemString != "")
                                                    ColumnItemString += ",";
                                                ColumnItemString += columnItem.FirstChild.InnerText;
                                            }
                                        }
                                        GridViewTable.Cell(j + 2, 3).Range.Text = ColumnItemString;
                                    }
                                }
                                co = (SDDataSource[j] as XmlNode).SelectSingleNode("Detail");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 4).Range.Text = co.InnerText;
                                co = (SDDataSource[j] as XmlNode).SelectSingleNode("DetailColumns");
                                if (co != null)
                                {
                                    XmlNodeList ColumnItemlist = co.SelectNodes("ColumnItem");
                                    if (ColumnItemlist != null && ColumnItemlist.Count > 0)
                                    {
                                        string ColumnItemString = "";
                                        foreach (XmlNode columnItem in ColumnItemlist)
                                        {
                                            if (columnItem.FirstChild != null)
                                            {
                                                if (ColumnItemString != "")
                                                    ColumnItemString += ",";
                                                ColumnItemString += columnItem.FirstChild.InnerText;
                                            }
                                        }
                                        GridViewTable.Cell(j + 2, 5).Range.Text = ColumnItemString;
                                    }
                                }
                            }
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                    catch (Exception e)
                    {
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeText(e.Message);
                        selection.TypeParagraph();
                    }
                    //}
                    #endregion

                    #region SDAutoNumber
                    //if ((String)printSetting[0] == "true")
                    //{
                    try
                    {
                        XmlNodeList SDAutoNumberList = children.SelectNodes("SDAutoNumber");
                        if (SDAutoNumberList != null && SDAutoNumberList.Count > 0)
                        {
                            selection.TypeText("【SDAutoNumber】");
                            selection.Font.Bold = 0;
                            selection.Font.Size = 10;
                            selection.TypeParagraph();

                            Table GridViewTable = oDoc.Tables.Add(selection.Range, SDAutoNumberList.Count + 1, 10, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "Active";
                            GridViewTable.Cell(1, 3).Range.Text = "UpdateComp";
                            GridViewTable.Cell(1, 4).Range.Text = "AutoNoID";
                            GridViewTable.Cell(1, 5).Range.Text = "isNumFill";
                            GridViewTable.Cell(1, 6).Range.Text = "NumDig";
                            GridViewTable.Cell(1, 7).Range.Text = "OverFlow";
                            GridViewTable.Cell(1, 8).Range.Text = "StartValue";
                            GridViewTable.Cell(1, 9).Range.Text = "Step";
                            GridViewTable.Cell(1, 10).Range.Text = "TargetColumn";

                            //}
                            for (int j = 0; j < SDAutoNumberList.Count; j++)
                            {
                                XmlNode co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("Name");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 1).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("Active");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 2).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("UpdateComp");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 3).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("AutoNoID");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 4).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("isNumFill");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 5).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("NumDig");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 6).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("OverFlow");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 7).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("StartValue");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 8).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("Step");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 9).Range.Text = co.InnerText;
                                co = (SDAutoNumberList[j] as XmlNode).SelectSingleNode("TargetColumn");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 10).Range.Text = co.InnerText;
                            }
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                    catch (Exception e)
                    {
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeText(e.Message);
                        selection.TypeParagraph();
                    }
                    //}
                    #endregion

                    #region SDTransaction
                    //if ((String)printSetting[0] == "true")
                    //{
                    try
                    {
                        XmlNodeList SDTransactionList = children.SelectNodes("SDTransaction");
                        if (SDTransactionList != null && SDTransactionList.Count > 0)
                        {
                            selection.TypeText("【SDTransaction】");
                            selection.Font.Bold = 0;
                            selection.Font.Size = 10;
                            selection.TypeParagraph();

                            Table GridViewTable = oDoc.Tables.Add(selection.Range, SDTransactionList.Count + 1, 3, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[1].PreferredWidth = 25;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable.Columns[2].PreferredWidth = 100;
                            //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "UpdateComp";
                            GridViewTable.Cell(1, 3).Range.Text = "Transactions";
                            //}
                            for (int j = 0; j < SDTransactionList.Count; j++)
                            {
                                XmlNode co = (SDTransactionList[j] as XmlNode).SelectSingleNode("Name");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 1).Range.Text = co.InnerText;
                                co = (SDTransactionList[j] as XmlNode).SelectSingleNode("UpdateComp");
                                if (co != null)
                                    GridViewTable.Cell(j + 2, 2).Range.Text = co.InnerText;
                                co = (SDTransactionList[j] as XmlNode).SelectSingleNode("Transactions");
                                if (co != null)
                                {
                                    XmlNodeList keylist = co.SelectNodes("KeyItem");
                                    if (keylist != null && keylist.Count > 0)
                                    {
                                        string keyfieldsstring = "";
                                        foreach (XmlNode key in keylist)
                                        {
                                            if (key.FirstChild != null)
                                            {
                                                if (keyfieldsstring != "")
                                                    keyfieldsstring += ",";
                                                keyfieldsstring += key.FirstChild.InnerText;
                                            }
                                        }
                                        GridViewTable.Cell(j + 2, 3).Range.Text = keyfieldsstring;
                                    }
                                }
                            }
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                    catch (Exception e)
                    {
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeText(e.Message);
                        selection.TypeParagraph();
                    }
                    //}
                    #endregion

                }
            }
        }

        public void LoadPage(string xaml, Selection selection, Microsoft.Office.Interop.Word._Application oWord,
            Microsoft.Office.Interop.Word._Document oDoc, object[] printSetting)
        {
            XmlDocument xml = new XmlDocument();

            //Page里面有一个ClientCS:的标签，在xml的外面，所以读取报错了，要拿走这个标签及之后的内容，保留之前的XML文本即可
            if (xaml.IndexOf("ClientCS:") != -1)
            {
                xaml = xaml.Substring(0, xaml.IndexOf("ClientCS:"));
            }
            TextReader tr = new StringReader(xaml);
            xml.Load(tr);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("SLTools", "clr-namespace:SLTools;assembly=SLTools");

            #region servicedatasource
            if (printSetting[0].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList ServiceDataSourceList = xml.SelectNodes("descendant::SLTools:ServiceDataSource", nsmgr);
                    if (ServiceDataSourceList != null && ServiceDataSourceList.Count > 0)
                    {
                        selection.TypeText("【ServiceDataSource】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, ServiceDataSourceList.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[1].PreferredWidth = 25;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[2].PreferredWidth = 100;
                        //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[1].PreferredWidth = 25;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[2].PreferredWidth = 100;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Rows[1].Range.Font.Bold = 2;

                        GridViewTable.Cell(1, 1).Range.Text = "Name";
                        GridViewTable.Cell(1, 2).Range.Text = "RemoteName";
                        GridViewTable.Cell(1, 3).Range.Text = "PacketRecords";
                        GridViewTable.Cell(1, 4).Range.Text = "AlwaysClose";
                        //}
                        for (int j = 0; j < ServiceDataSourceList.Count; j++)
                        {
                            XmlAttributeCollection co = (ServiceDataSourceList[j] as XmlNode).Attributes;

                            GridViewTable.Cell(j + 2, 1).Range.Text = co["x:Name"].Value;
                            GridViewTable.Cell(j + 2, 2).Range.Text = co["RemoteName"].Value;
                            GridViewTable.Cell(j + 2, 3).Range.Text = co["PacketRecords"].Value;
                            GridViewTable.Cell(j + 2, 4).Range.Text = co["AlwaysClose"].Value;
                        }
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion

            #region navigator
            if (printSetting[1].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList SLNavigatorList = xml.SelectNodes("descendant::SLTools:SLNavigator", nsmgr);
                    if (SLNavigatorList != null && SLNavigatorList.Count > 0)
                    {
                        selection.TypeText("【SLNavigator】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, SLNavigatorList.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[1].PreferredWidth = 25;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[2].PreferredWidth = 100;
                        //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[1].PreferredWidth = 25;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[2].PreferredWidth = 100;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Rows[1].Range.Font.Bold = 2;

                        GridViewTable.Cell(1, 1).Range.Text = "Name";
                        GridViewTable.Cell(1, 2).Range.Text = "DataSourceID";
                        GridViewTable.Cell(1, 3).Range.Text = "DataObjectID";
                        GridViewTable.Cell(1, 4).Range.Text = "NavMode";
                        //}
                        for (int j = 0; j < SLNavigatorList.Count; j++)
                        {
                            XmlAttributeCollection co = (SLNavigatorList[j] as XmlNode).Attributes;

                            GridViewTable.Cell(j + 2, 1).Range.Text = co["x:Name"].Value;
                            GridViewTable.Cell(j + 2, 2).Range.Text = co["DataSourceID"].Value;
                            GridViewTable.Cell(j + 2, 3).Range.Text = co["DataObjectID"].Value;
                            GridViewTable.Cell(j + 2, 4).Range.Text = co["NavMode"].Value;
                        }
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion

            #region datagrid
            if (printSetting[2].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList SLDataGridList = xml.SelectNodes("descendant::SLTools:SLDataGrid", nsmgr);
                    if (SLDataGridList != null && SLDataGridList.Count > 0)
                    {
                        selection.TypeText("【SLDataGrid】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        for (int j = 0; j < SLDataGridList.Count; j++)
                        {
                            XmlAttributeCollection co = (SLDataGridList[j] as XmlNode).Attributes;
                            Table GridViewTable = oDoc.Tables.Add(selection.Range, 2, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "DataSourceID";
                            GridViewTable.Cell(2, 1).Range.Text = co["x:Name"].Value;
                            GridViewTable.Cell(2, 2).Range.Text = co["DataSourceID"].Value;
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                            #region DataGridFieldList
                            XmlNodeList DataGridFieldList = (SLDataGridList[j] as XmlNode).ChildNodes;
                            Dictionary<string, object[]> DataFieldDictionary = new Dictionary<string, object[]>();
                            foreach (XmlNode Columns in DataGridFieldList)
                            {
                                if (Columns.LocalName == "SLDataGrid.Columns")
                                {
                                    foreach (XmlNode Column in Columns.ChildNodes)
                                    {
                                        if (Column.LocalName == "DataGridTextColumn")
                                        {
                                            XmlAttributeCollection innerControlAttris = Column.Attributes;
                                            string width = innerControlAttris["Width"] != null ? innerControlAttris["Width"].Value : "";
                                            string Header = innerControlAttris["Header"].Value;
                                            string Binding = innerControlAttris["Binding"].Value;
                                            string fieldname = Binding.Substring(Binding.IndexOf('=') + 1, Binding.IndexOf('}') - Binding.IndexOf('=') - 1);
                                            DataFieldDictionary.Add(fieldname, new object[] { fieldname, "TextColumn", Header, width });
                                        }
                                    }
                                }
                            }
                            #endregion

                            Table GridViewTable2 = oDoc.Tables.Add(selection.Range, DataFieldDictionary.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            //GridViewTable2.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[1].PreferredWidth = 25;
                            //GridViewTable2.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[2].PreferredWidth = 100;


                            GridViewTable2.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable2.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable2.Rows[1].Range.Font.Bold = 2;

                            GridViewTable2.Cell(1, 1).Range.Text = "Field Name";
                            GridViewTable2.Cell(1, 2).Range.Text = "Field Type";
                            GridViewTable2.Cell(1, 3).Range.Text = "Header";
                            GridViewTable2.Cell(1, 4).Range.Text = "Width";
                            //}
                            int count = 2;
                            foreach (var d in DataFieldDictionary)
                            {
                                object[] value = d.Value;
                                if (value != null)
                                {
                                    GridViewTable2.Cell(count, 1).Range.Text = value[0].ToString();
                                    GridViewTable2.Cell(count, 2).Range.Text = value[1].ToString();
                                    GridViewTable2.Cell(count, 3).Range.Text = value[2].ToString();
                                    GridViewTable2.Cell(count, 4).Range.Text = value[3].ToString();
                                }
                                count++;
                            }

                            GridViewTable2.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion

            #region dataform
            if (printSetting[3].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList SLDataFormList = xml.SelectNodes("descendant::SLTools:SLDataForm", nsmgr);
                    if (SLDataFormList != null && SLDataFormList.Count > 0)
                    {
                        selection.TypeText("【SLDataForm】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        for (int j = 0; j < SLDataFormList.Count; j++)
                        {
                            XmlAttributeCollection co = (SLDataFormList[j] as XmlNode).Attributes;
                            Table GridViewTable = oDoc.Tables.Add(selection.Range, 2, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "DataSourceID";
                            GridViewTable.Cell(2, 1).Range.Text = co["x:Name"].Value;
                            GridViewTable.Cell(2, 2).Range.Text = co["DataSourceID"].Value;
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                            #region DataFormFieldList
                            XmlNodeList DataFormFieldList = (SLDataFormList[j] as XmlNode).ChildNodes;
                            Dictionary<string, object[]> DataFieldDictionary = new Dictionary<string, object[]>();
                            foreach (XmlNode ReadOnlyTemplateNode in DataFormFieldList)
                            {
                                if (ReadOnlyTemplateNode.LocalName == "SLDataForm.ReadOnlyTemplate")
                                {
                                    foreach (XmlNode DataTemplateNode in ReadOnlyTemplateNode.ChildNodes)
                                    {
                                        if (DataTemplateNode.LocalName == "DataTemplate")
                                        {
                                            foreach (XmlNode GridNode in DataTemplateNode.ChildNodes)
                                            {
                                                if (GridNode.LocalName == "Grid")
                                                {
                                                    foreach (XmlNode FormFieldNode in GridNode.ChildNodes)
                                                    {
                                                        if (FormFieldNode.LocalName == "DataField")
                                                        {
                                                            XmlAttributeCollection fieldAttriCol = FormFieldNode.Attributes;
                                                            string name = fieldAttriCol["x:Name"].Value;
                                                            string label = fieldAttriCol["Label"].Value;
                                                            DataFieldDictionary.Add(name, null);
                                                            if (FormFieldNode.FirstChild != null)
                                                            {
                                                                if (FormFieldNode.FirstChild.LocalName == "TextBox")
                                                                {
                                                                    XmlAttributeCollection innerControlAttris = FormFieldNode.FirstChild.Attributes;
                                                                    string width = innerControlAttris["Width"].Value;
                                                                    string text = innerControlAttris["Text"].Value;
                                                                    string fieldname = text.Substring(text.IndexOf('=') + 1, text.IndexOf('}') - text.IndexOf('=') - 1);
                                                                    DataFieldDictionary[name] = new object[] { fieldname, "Textbox", label, width };
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            Table GridViewTable2 = oDoc.Tables.Add(selection.Range, DataFieldDictionary.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            //GridViewTable2.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[1].PreferredWidth = 25;
                            //GridViewTable2.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[2].PreferredWidth = 100;


                            GridViewTable2.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable2.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable2.Rows[1].Range.Font.Bold = 2;

                            GridViewTable2.Cell(1, 1).Range.Text = "Field Name";
                            GridViewTable2.Cell(1, 2).Range.Text = "Field Type";
                            GridViewTable2.Cell(1, 3).Range.Text = "Caption";
                            GridViewTable2.Cell(1, 4).Range.Text = "Width";
                            //}
                            int count = 2;
                            foreach (var d in DataFieldDictionary)
                            {
                                object[] value = d.Value;
                                if (value != null)
                                {
                                    GridViewTable2.Cell(count, 1).Range.Text = value[0].ToString();
                                    GridViewTable2.Cell(count, 2).Range.Text = value[1].ToString();
                                    GridViewTable2.Cell(count, 3).Range.Text = value[2].ToString();
                                    GridViewTable2.Cell(count, 4).Range.Text = value[3].ToString();
                                }
                                count++;
                            }

                            GridViewTable2.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion

            #region default
            if (printSetting[4].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList SLDefaultList = xml.SelectNodes("descendant::SLTools:SLDefault", nsmgr);
                    if (SLDefaultList != null && SLDefaultList.Count > 0)
                    {
                        selection.TypeText("【SLDefault】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        for (int j = 0; j < SLDefaultList.Count; j++)
                        {
                            XmlAttributeCollection co = (SLDefaultList[j] as XmlNode).Attributes;
                            Table GridViewTable = oDoc.Tables.Add(selection.Range, 2, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "DataObjectID";
                            GridViewTable.Cell(2, 1).Range.Text = co["x:Name"].Value;
                            GridViewTable.Cell(2, 2).Range.Text = co["DataObjectID"].Value;
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();

                            #region DefaultList
                            XmlNodeList DefaultList = (SLDefaultList[j] as XmlNode).ChildNodes;
                            Dictionary<string, object[]> DefaultDictionary = new Dictionary<string, object[]>();
                            foreach (XmlNode DefaultValues in DefaultList)
                            {
                                if (DefaultValues.LocalName == "SLDefault.DefaultValues")
                                {
                                    foreach (XmlNode DefaultItem in DefaultValues.ChildNodes)
                                    {
                                        if (DefaultItem.LocalName == "SLDefaultItem")
                                        {
                                            XmlAttributeCollection fieldAttriCol = DefaultItem.Attributes;
                                            string name = fieldAttriCol["FieldName"].Value;
                                            string DefaultValue = fieldAttriCol["DefaultValue"] != null ? fieldAttriCol["DefaultValue"].Value : "";
                                            string DefaultMethod = fieldAttriCol["DefaultMethod"] != null ? fieldAttriCol["DefaultMethod"].Value : "";
                                            DefaultDictionary.Add(name, new object[] { name, DefaultValue, DefaultMethod });
                                        }
                                    }
                                }
                            }
                            #endregion

                            Table GridViewTable2 = oDoc.Tables.Add(selection.Range, DefaultDictionary.Count + 1, 3, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            //GridViewTable2.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[1].PreferredWidth = 25;
                            //GridViewTable2.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[2].PreferredWidth = 100;


                            GridViewTable2.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable2.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable2.Rows[1].Range.Font.Bold = 2;

                            GridViewTable2.Cell(1, 1).Range.Text = "Field Name";
                            GridViewTable2.Cell(1, 2).Range.Text = "DefaultValue";
                            GridViewTable2.Cell(1, 3).Range.Text = "DefaultMethod";
                            //}
                            int count = 2;
                            foreach (var d in DefaultDictionary)
                            {
                                object[] value = d.Value;
                                if (value != null)
                                {
                                    GridViewTable2.Cell(count, 1).Range.Text = value[0].ToString();
                                    GridViewTable2.Cell(count, 2).Range.Text = value[1].ToString();
                                    GridViewTable2.Cell(count, 3).Range.Text = value[2].ToString();
                                }
                                count++;
                            }

                            GridViewTable2.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion

            #region validator
            if (printSetting[5].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList SLValidatorList = xml.SelectNodes("descendant::SLTools:SLValidator", nsmgr);
                    if (SLValidatorList != null && SLValidatorList.Count > 0)
                    {
                        selection.TypeText("【SLValidator】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        for (int j = 0; j < SLValidatorList.Count; j++)
                        {
                            XmlAttributeCollection co = (SLValidatorList[j] as XmlNode).Attributes;
                            Table GridViewTable = oDoc.Tables.Add(selection.Range, 2, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "Name";
                            GridViewTable.Cell(1, 2).Range.Text = "DataObjectID";
                            GridViewTable.Cell(2, 1).Range.Text = co["x:Name"].Value;
                            GridViewTable.Cell(2, 2).Range.Text = co["DataObjectID"].Value;
                            GridViewTable.Cell(1, 3).Range.Text = "DuplicateCheck";
                            GridViewTable.Cell(1, 4).Range.Text = "DuplicateCheckMode";
                            GridViewTable.Cell(2, 3).Range.Text = co["DuplicateCheck"].Value;
                            GridViewTable.Cell(2, 4).Range.Text = co["DuplicateCheckMode"].Value;

                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();

                            #region ValidatortList
                            XmlNodeList ValidatortList = (SLValidatorList[j] as XmlNode).ChildNodes;
                            Dictionary<string, object[]> ValidatortDictionary = new Dictionary<string, object[]>();
                            foreach (XmlNode Validatorts in ValidatortList)
                            {
                                if (Validatorts.LocalName == "SLValidator.Validates")
                                {
                                    foreach (XmlNode Validatort in Validatorts.ChildNodes)
                                    {
                                        if (Validatort.LocalName == "SLValidateItem")
                                        {
                                            XmlAttributeCollection fieldAttriCol = Validatort.Attributes;
                                            string name = fieldAttriCol["FieldName"].Value;
                                            string CheckNull = fieldAttriCol["CheckNull"] != null ? fieldAttriCol["CheckNull"].Value : "";
                                            string CHeckNullMessage = fieldAttriCol["CHeckNullMessage"] != null ? fieldAttriCol["CHeckNullMessage"].Value : "";
                                            string CaptionControlName = fieldAttriCol["CaptionControlName"] != null ? fieldAttriCol["CaptionControlName"].Value : "";
                                            string ValidatortMethod = fieldAttriCol["ValidatortMethod"] != null ? fieldAttriCol["ValidatortMethod"].Value : "";
                                            string InvalidMessage = fieldAttriCol["InvalidMessage"] != null ? fieldAttriCol["InvalidMessage"].Value : "";

                                            ValidatortDictionary.Add(name, new object[] { name, CaptionControlName, CheckNull, CHeckNullMessage, InvalidMessage, ValidatortMethod });
                                        }
                                    }
                                }
                            }
                            #endregion

                            Table GridViewTable2 = oDoc.Tables.Add(selection.Range, ValidatortDictionary.Count + 1, 6, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            //GridViewTable2.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[1].PreferredWidth = 25;
                            //GridViewTable2.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[2].PreferredWidth = 100;


                            GridViewTable2.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable2.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable2.Rows[1].Range.Font.Bold = 2;

                            GridViewTable2.Cell(1, 1).Range.Text = "Field Name";
                            GridViewTable2.Cell(1, 2).Range.Text = "CaptionControlName";
                            GridViewTable2.Cell(1, 3).Range.Text = "CheckNull";
                            GridViewTable2.Cell(1, 4).Range.Text = "CHeckNullMessage";
                            GridViewTable2.Cell(1, 5).Range.Text = "InvalidMessage";
                            GridViewTable2.Cell(1, 6).Range.Text = "ValidatortMethod";
                            //}
                            int count = 2;
                            foreach (var d in ValidatortDictionary)
                            {
                                object[] value = d.Value;
                                if (value != null)
                                {
                                    GridViewTable2.Cell(count, 1).Range.Text = value[0].ToString();
                                    GridViewTable2.Cell(count, 2).Range.Text = value[1].ToString();
                                    GridViewTable2.Cell(count, 3).Range.Text = value[2].ToString();
                                    GridViewTable2.Cell(count, 4).Range.Text = value[3].ToString();
                                    GridViewTable2.Cell(count, 5).Range.Text = value[4].ToString();
                                    GridViewTable2.Cell(count, 6).Range.Text = value[5].ToString();
                                }
                                count++;
                            }

                            GridViewTable2.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion
        }

        public void LoadWebPage(string xaml, Selection selection, Microsoft.Office.Interop.Word._Application oWord,
            Microsoft.Office.Interop.Word._Document oDoc, object[] printSetting)
        {
            XmlDocument xml = new XmlDocument();

            //Page里面有一个ClientCS:的标签，在xml的外面，所以读取报错了，要拿走这个标签及之后的内容，保留之前的XML文本即可
            if (xaml.IndexOf("ClientCS:") != -1)
            {
                xaml = xaml.Substring(0, xaml.IndexOf("ClientCS:"));
            }
            if (xaml.StartsWith("<div>"))
            {
                string s = " xmlns:cc1=\"clr-namespace:cc1\" xmlns:dx=\"clr-namespace:dx\"";
                xaml = xaml.Insert(4, s);
            }
            TextReader tr = new StringReader(xaml);
            xml.Load(tr);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("cc1", "clr-namespace:cc1");
            nsmgr.AddNamespace("dx", "clr-namespace:dx");

            #region servicedatasource
            if (printSetting[0].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList ServiceDataSourceList = xml.SelectNodes("descendant::cc1:WebDataSource", nsmgr);
                    if (ServiceDataSourceList != null && ServiceDataSourceList.Count > 0)
                    {
                        selection.TypeText("【WebDataSource】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, ServiceDataSourceList.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[1].PreferredWidth = 25;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[2].PreferredWidth = 100;
                        //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[1].PreferredWidth = 25;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        //GridViewTable.Columns[2].PreferredWidth = 100;
                        //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Rows[1].Range.Font.Bold = 2;

                        GridViewTable.Cell(1, 1).Range.Text = "ID";
                        GridViewTable.Cell(1, 2).Range.Text = "RemoteName";
                        GridViewTable.Cell(1, 3).Range.Text = "PreviewSolution";
                        GridViewTable.Cell(1, 4).Range.Text = "PreviewDatabase";
                        //}
                        for (int j = 0; j < ServiceDataSourceList.Count; j++)
                        {
                            XmlAttributeCollection co = (ServiceDataSourceList[j] as XmlNode).Attributes;

                            GridViewTable.Cell(j + 2, 1).Range.Text = co["ID"].Value;
                            GridViewTable.Cell(j + 2, 2).Range.Text = co["RemoteName"].Value;
                            GridViewTable.Cell(j + 2, 3).Range.Text = co["PreviewSolution"].Value;
                            GridViewTable.Cell(j + 2, 4).Range.Text = co["PreviewDatabase"].Value;
                        }
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion

            #region datagrid
            if (printSetting[2].ToString().ToLower() == "true")
            {
                try
                {
                    XmlNodeList SLDataGridList = xml.SelectNodes("descendant::dx:ASPxGridView", nsmgr);
                    if (SLDataGridList != null && SLDataGridList.Count > 0)
                    {
                        selection.TypeText("【ASPxGridView】");
                        selection.Font.Bold = 0;
                        selection.Font.Size = 10;
                        selection.TypeParagraph();

                        for (int j = 0; j < SLDataGridList.Count; j++)
                        {
                            XmlAttributeCollection co = (SLDataGridList[j] as XmlNode).Attributes;
                            Table GridViewTable = oDoc.Tables.Add(selection.Range, 2, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable.Rows[1].Range.Font.Bold = 2;

                            GridViewTable.Cell(1, 1).Range.Text = "ID";
                            GridViewTable.Cell(1, 2).Range.Text = "DataSourceID";
                            GridViewTable.Cell(2, 1).Range.Text = co["ID"].Value;
                            GridViewTable.Cell(2, 2).Range.Text = co["DataSourceID"].Value;
                            GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                            #region DataGridFieldList
                            XmlNodeList DataGridFieldList = (SLDataGridList[j] as XmlNode).ChildNodes;
                            Dictionary<string, object[]> DataFieldDictionary = new Dictionary<string, object[]>();
                            foreach (XmlNode Columns in DataGridFieldList)
                            {
                                if (Columns.LocalName == "Columns")
                                {
                                    foreach (XmlNode Column in Columns.ChildNodes)
                                    {
                                        if (Column.LocalName == "GridViewDataTextColumn")
                                        {
                                            XmlAttributeCollection innerControlAttris = Column.Attributes;
                                            string width = innerControlAttris["Width"] != null ? innerControlAttris["Width"].Value : "";
                                            string fieldname = innerControlAttris["FieldName"].Value;
                                            string Caption = innerControlAttris["Caption"].Value;
                                            DataFieldDictionary.Add(fieldname, new object[] { fieldname, Caption, width });
                                        }
                                    }
                                }
                            }
                            #endregion

                            Table GridViewTable2 = oDoc.Tables.Add(selection.Range, DataFieldDictionary.Count + 1, 3, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            //GridViewTable2.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[1].PreferredWidth = 25;
                            //GridViewTable2.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                            //GridViewTable2.Columns[2].PreferredWidth = 100;


                            GridViewTable2.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewTable2.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewTable2.Rows[1].Range.Font.Bold = 2;

                            GridViewTable2.Cell(1, 1).Range.Text = "Field Name";
                            GridViewTable2.Cell(1, 2).Range.Text = "Caption";
                            GridViewTable2.Cell(1, 3).Range.Text = "Width";
                            //}
                            int count = 2;
                            foreach (var d in DataFieldDictionary)
                            {
                                object[] value = d.Value;
                                if (value != null)
                                {
                                    GridViewTable2.Cell(count, 1).Range.Text = value[0].ToString();
                                    GridViewTable2.Cell(count, 2).Range.Text = value[1].ToString();
                                    GridViewTable2.Cell(count, 3).Range.Text = value[2].ToString();
                                }
                                count++;
                            }

                            GridViewTable2.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }
                    }
                }
                catch (Exception e)
                {
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeText(e.Message);
                    selection.TypeParagraph();
                }
            }
            #endregion


        }
        #endregion

        public void LoadService2(string xaml, Selection selection, Microsoft.Office.Interop.Word._Application oWord,
Microsoft.Office.Interop.Word._Document oDoc, object[] printSetting)
        {
            if (printSetting == null)
            {
                printSetting = new string[] { "true", "true", "true", "true" };
            }
            JObject context = (JObject)JsonConvert.DeserializeObject(xaml);
            JArray controls = (JArray)context["controls"];
            JValue codes = (JValue)context["code"];
            var memberPart = new List<object>();
            var commandlist = new List<object>();
            var updatecomponentlist = new List<object>();
            var infoDataSourcelist = new List<object>();
            var autonumberlist = new List<object>();
            try
            {
                foreach (var component in controls)
                {
                    var componentType = (string)component["type"];
                    var ct = componentType.Split('.');
                    var type = ct[1].ToString();
                    var properties = component["properties"] as JObject;
                    var id = (string)properties["ID"];

                    //add name 
                    properties["Name"] = id;
                    if (type.ToLower() == "infocommand")
                        commandlist.Add(new object[] { id, properties });
                    if (type.ToLower() == "updatecomponent")
                        updatecomponentlist.Add(new object[] { id, properties });
                    if (type.ToLower() == "infodatasource")
                        infoDataSourcelist.Add(new object[] { id, properties });
                    if (type.ToLower() == "autonumber")
                        autonumberlist.Add(new object[] { id, properties });
                }
                #region InfoCommand
                if (printSetting.Length > 0 && printSetting[0].ToString().ToLower() == "true" && commandlist.Count > 0)
                {
                    selection.TypeText("【InfoCommand】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    Table GridViewTable = oDoc.Tables.Add(selection.Range, commandlist.Count + 1, 7, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    //GridViewTable.Columns[1].PreferredWidth = 25;
                    //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    //GridViewTable.Columns[2].PreferredWidth = 100;
                    //GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    //GridViewTable.Columns[1].PreferredWidth = 25;
                    //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    //GridViewTable.Columns[2].PreferredWidth = 100;
                    //GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    GridViewTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    GridViewTable.Rows[1].Range.Font.Bold = 2;

                    GridViewTable.Cell(1, 1).Range.Text = "ID";
                    GridViewTable.Cell(1, 2).Range.Text = "CommandText";
                    GridViewTable.Cell(1, 3).Range.Text = "KeyFields";
                    GridViewTable.Cell(1, 4).Range.Text = "EEPAlias";
                    GridViewTable.Cell(1, 5).Range.Text = "SecStyle";
                    GridViewTable.Cell(1, 6).Range.Text = "SiteControl";
                    GridViewTable.Cell(1, 7).Range.Text = "SelectPaging";
                    for (int j = 0; j < commandlist.Count; j++)
                    {
                        JObject command = (commandlist[j] as object[])[1] as JObject;
                        GridViewTable.Cell(j + 2, 1).Range.Text = command["ID"].ToString();
                        GridViewTable.Cell(j + 2, 2).Range.Text = command["CommandText"] != null ? command["CommandText"].ToString() : "";
                        string keyfieldsstring = "";
                        var keyFields = command["KeyFields"];
                        for (int k = 0; k < keyFields.Count(); k++)
                        {
                            if (keyfieldsstring != "")
                                keyfieldsstring += ",";
                            keyfieldsstring += keyFields[k]["KeyName"].ToString();
                        }
                        GridViewTable.Cell(j + 2, 3).Range.Text = keyfieldsstring;
                        GridViewTable.Cell(j + 2, 4).Range.Text = command["EEPAlias"] != null ? command["EEPAlias"].ToString() : "";
                        GridViewTable.Cell(j + 2, 5).Range.Text = command["SecStyle"] != null ? command["SecStyle"].ToString() : "";
                        GridViewTable.Cell(j + 2, 6).Range.Text = command["SiteControl"] != null ? command["SiteControl"].ToString() : "";
                        GridViewTable.Cell(j + 2, 7).Range.Text = command["SelectPaging"] != null ? command["SelectPaging"].ToString() : "";

                        //XmlNodeList keylist = co.SelectNodes("KeyItem");
                        //if (keylist != null && keylist.Count > 0)
                        //{
                        //    string keyfieldsstring = "";
                        //    foreach (XmlNode key in keylist)
                        //    {
                        //        if (key.FirstChild != null)
                        //        {
                        //            if (keyfieldsstring != "")
                        //                keyfieldsstring += ",";
                        //            keyfieldsstring += key.FirstChild.InnerText;
                        //        }
                        //    }
                        //    GridViewTable.Cell(j + 2, 3).Range.Text = keyfieldsstring;
                        //}

                    }
                    GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();
                }
                #endregion

                #region Updatecomponent
                if (printSetting.Length > 1 && printSetting[1].ToString().ToLower() == "true" && updatecomponentlist.Count > 0)
                {
                    selection.TypeText("【UpdateComponent】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    Table GridViewTable2 = oDoc.Tables.Add(selection.Range, updatecomponentlist.Count + 1, 5, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    GridViewTable2.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    GridViewTable2.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    GridViewTable2.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    GridViewTable2.Rows[1].Range.Font.Bold = 2;

                    GridViewTable2.Cell(1, 1).Range.Text = "ID";
                    GridViewTable2.Cell(1, 2).Range.Text = "SelectCmd";
                    GridViewTable2.Cell(1, 3).Range.Text = "ServerModify";
                    GridViewTable2.Cell(1, 4).Range.Text = "WhereMode";
                    GridViewTable2.Cell(1, 5).Range.Text = "ServerModifyGetMax";
                    for (int j = 0; j < updatecomponentlist.Count; j++)
                    {
                        JObject updatecomponent = (updatecomponentlist[j] as object[])[1] as JObject;
                        GridViewTable2.Cell(j + 2, 1).Range.Text = updatecomponent["ID"].ToString();
                        GridViewTable2.Cell(j + 2, 2).Range.Text = updatecomponent["SelectCmd"] != null ? updatecomponent["SelectCmd"].ToString() : "";
                        GridViewTable2.Cell(j + 2, 3).Range.Text = updatecomponent["ServerModify"] != null ? updatecomponent["ServerModify"].ToString() : "";
                        GridViewTable2.Cell(j + 2, 4).Range.Text = updatecomponent["WhereMode"] != null ? updatecomponent["WhereMode"].ToString() : "";
                        GridViewTable2.Cell(j + 2, 5).Range.Text = updatecomponent["ServerModifyGetMax"] != null ? updatecomponent["ServerModifyGetMax"].ToString() : "";
                    }
                    GridViewTable2.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();
                }
                #endregion

                #region InfoDataSource
                if (printSetting.Length > 2 && printSetting[2].ToString().ToLower() == "true" && infoDataSourcelist.Count > 0)
                {
                    selection.TypeText("【InfoDataSource】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    Table GridViewTable3 = oDoc.Tables.Add(selection.Range, infoDataSourcelist.Count + 1, 5, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    GridViewTable3.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    GridViewTable3.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    GridViewTable3.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    GridViewTable3.Rows[1].Range.Font.Bold = 2;

                    GridViewTable3.Cell(1, 1).Range.Text = "ID";
                    GridViewTable3.Cell(1, 2).Range.Text = "Master";
                    GridViewTable3.Cell(1, 3).Range.Text = "MasterColumns";
                    GridViewTable3.Cell(1, 4).Range.Text = "Detail";
                    GridViewTable3.Cell(1, 5).Range.Text = "DetailColumns";
                    for (int j = 0; j < infoDataSourcelist.Count; j++)
                    {
                        JObject infoDataSource = (infoDataSourcelist[j] as object[])[1] as JObject;
                        GridViewTable3.Cell(j + 2, 1).Range.Text = infoDataSource["ID"].ToString();
                        GridViewTable3.Cell(j + 2, 2).Range.Text = infoDataSource["Master"] != null ? infoDataSource["Master"].ToString() : "";
                        JArray MasterColumns = infoDataSource["MasterColumns"] as JArray;
                        string MasterColumnsString = "";
                        for (int k = 0; k < MasterColumns.Count(); k++)
                        {
                            if (MasterColumnsString != "")
                                MasterColumnsString += ",";
                            MasterColumnsString += MasterColumns[k]["FieldName"].ToString();
                        }
                        GridViewTable3.Cell(j + 2, 3).Range.Text = MasterColumnsString;
                        GridViewTable3.Cell(j + 2, 4).Range.Text = infoDataSource["Detail"] != null ? infoDataSource["Detail"].ToString() : "";
                        JArray DetailColumns = infoDataSource["DetailColumns"] as JArray;
                        string DetailColumnsString = "";
                        for (int k = 0; k < DetailColumns.Count(); k++)
                        {
                            if (DetailColumnsString != "")
                                DetailColumnsString += ",";
                            DetailColumnsString += DetailColumns[k]["FieldName"].ToString();
                        }
                        GridViewTable3.Cell(j + 2, 5).Range.Text = DetailColumnsString;

                    }
                    GridViewTable3.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();
                }
                #endregion

                #region AutoNumber
                if (printSetting.Length > 3 && printSetting[3].ToString().ToLower() == "true" && autonumberlist.Count > 0)
                {
                    selection.TypeText("【AutoNumber】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    Table GridViewTable4 = oDoc.Tables.Add(selection.Range, autonumberlist.Count + 1, 9, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    GridViewTable4.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    GridViewTable4.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    GridViewTable4.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    GridViewTable4.Rows[1].Range.Font.Bold = 2;

                    GridViewTable4.Cell(1, 1).Range.Text = "ID";
                    GridViewTable4.Cell(1, 2).Range.Text = "AutoNoID";
                    GridViewTable4.Cell(1, 3).Range.Text = "UpdateComp";
                    GridViewTable4.Cell(1, 4).Range.Text = "TargetColumn";
                    GridViewTable4.Cell(1, 5).Range.Text = "GetFixed";
                    GridViewTable4.Cell(1, 6).Range.Text = "NumDig";
                    GridViewTable4.Cell(1, 7).Range.Text = "StartValue";
                    GridViewTable4.Cell(1, 8).Range.Text = "Step";
                    GridViewTable4.Cell(1, 9).Range.Text = "OverFlow";
                    for (int j = 0; j < autonumberlist.Count; j++)
                    {
                        JObject AutoNumber = (autonumberlist[j] as object[])[1] as JObject;
                        GridViewTable4.Cell(j + 2, 1).Range.Text = AutoNumber["ID"].ToString();
                        GridViewTable4.Cell(j + 2, 2).Range.Text = AutoNumber["AutoNoID"] != null ? AutoNumber["AutoNoID"].ToString() : "";
                        GridViewTable4.Cell(j + 2, 3).Range.Text = AutoNumber["UpdateComp"] != null ? AutoNumber["UpdateComp"].ToString() : "";
                        GridViewTable4.Cell(j + 2, 4).Range.Text = AutoNumber["TargetColumn"] != null ? AutoNumber["TargetColumn"].ToString() : "";
                        GridViewTable4.Cell(j + 2, 5).Range.Text = AutoNumber["GetFixed"] != null ? AutoNumber["GetFixed"].ToString() : "";
                        GridViewTable4.Cell(j + 2, 6).Range.Text = AutoNumber["NumDig"] != null ? AutoNumber["NumDig"].ToString() : "";
                        GridViewTable4.Cell(j + 2, 7).Range.Text = AutoNumber["StartValue"] != null ? AutoNumber["StartValue"].ToString() : "";
                        GridViewTable4.Cell(j + 2, 8).Range.Text = AutoNumber["Step"] != null ? AutoNumber["Step"].ToString() : "";
                        GridViewTable4.Cell(j + 2, 9).Range.Text = AutoNumber["OverFlow"] != null ? AutoNumber["OverFlow"].ToString() : "";
                    }
                    GridViewTable4.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();
                }
                #endregion

                #region code
                if (codes != null && codes.Value.ToString() != "")
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【Code】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    selection.TypeText(codes.Value.ToString());
                    selection.TypeParagraph();
                    selection.TypeParagraph();
                }
                #endregion

            }
            catch (Exception ex)
            {
                WriteError(ex.Message, "ServerPrint");
            }

        }
        public void LoadWebPage2(string xaml, Selection selection, Microsoft.Office.Interop.Word._Application oWord,
    Microsoft.Office.Interop.Word._Document oDoc, object[] printSetting)
        {
            if (printSetting == null)
            {
                printSetting = new string[] { "true", "true", "true", "true" };
            }
            JObject context = (JObject)JsonConvert.DeserializeObject(xaml);
            var memberPart = new List<object>();
            var datagridlist = new List<object>();
            var dataformlist = new List<object>();
            var defaultslist = new List<object>();
            var validatelist = new List<object>();
            JArray controls = (JArray)context["controls"];
            JValue codes = (JValue)context["code"];
            JValue script = (JValue)context["script"];
            try
            {
                foreach (var component in controls)
                {
                    var componentType = (string)component["type"];
                    var ct = componentType.Split('.');
                    var type = ct[1].ToString();
                    var properties = component["properties"] as JObject;
                    var id = (string)properties["ID"];
                    //add name 
                    properties["Name"] = id;
                    if (type.ToLower() == "jqdatagrid")
                        datagridlist.Add(new object[] { id, properties });
                    if (type.ToLower() == "jqdataform")
                        dataformlist.Add(new object[] { id, properties });
                    if (type.ToLower() == "jqdefault")
                        defaultslist.Add(new object[] { id, properties });
                    if (type.ToLower() == "jqvalidate")
                        validatelist.Add(new object[] { id, properties });
                    if (type.ToLower() == "jqdialog")
                        getHTMLChildren((JObject)component, datagridlist, dataformlist, defaultslist, validatelist);
                }
                #region datagrid
                if (printSetting.Length > 0 && printSetting[0].ToString().ToLower() == "true" && datagridlist.Count > 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【DataGrid】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    for (int i = 0; i < datagridlist.Count; i++)
                    {
                        JObject datagrid = (datagridlist[i] as object[])[1] as JObject;
                        selection.TypeText("【" + datagrid["ID"].ToString() + "】");
                        selection.TypeParagraph();

                        //datagrid property
                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 5, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "RemoteName";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 3).Range.Text = "DataMember";
                        GridViewTable.Cell(1, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = datagrid["RemoteName"] != null ? datagrid["RemoteName"].ToString() : "";
                        GridViewTable.Cell(1, 4).Range.Text = datagrid["DataMember"] != null ? datagrid["DataMember"].ToString() : "";
                        GridViewTable.Cell(2, 1).Range.Text = "Title";
                        GridViewTable.Cell(2, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 3).Range.Text = "AutoApply";
                        GridViewTable.Cell(2, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 2).Range.Text = datagrid["Title"] != null ? datagrid["Title"].ToString() : "";
                        GridViewTable.Cell(2, 4).Range.Text = datagrid["AutoApply"] != null ? datagrid["AutoApply"].ToString() : "";
                        GridViewTable.Cell(3, 1).Range.Text = "AlwaysClose";
                        GridViewTable.Cell(3, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 3).Range.Text = "Pagination";
                        GridViewTable.Cell(3, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 2).Range.Text = datagrid["AlwaysClose"] != null ? datagrid["AlwaysClose"].ToString() : "";
                        GridViewTable.Cell(3, 4).Range.Text = datagrid["Pagination"] != null ? datagrid["Pagination"].ToString() : "";
                        GridViewTable.Cell(4, 1).Range.Text = "PageSize";
                        GridViewTable.Cell(4, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(4, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(4, 3).Range.Text = "QueryAutoColumn";
                        GridViewTable.Cell(4, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(4, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(4, 2).Range.Text = datagrid["PageSize"] != null ? datagrid["PageSize"].ToString() : "";
                        GridViewTable.Cell(4, 4).Range.Text = datagrid["QueryAutoColumn"] != null ? datagrid["QueryAutoColumn"].ToString() : "";
                        GridViewTable.Cell(5, 1).Range.Text = "DuplicateCheck";
                        GridViewTable.Cell(5, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(5, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(5, 3).Range.Text = "";
                        GridViewTable.Cell(5, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(5, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(5, 2).Range.Text = datagrid["DuplicateCheck"] != null ? datagrid["DuplicateCheck"].ToString() : "";
                        GridViewTable.Cell(5, 4).Range.Text = "";
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        //columns
                        JArray fieldlist = datagrid["Columns"] as JArray;
                        Table GridViewFieldTable = oDoc.Tables.Add(selection.Range, fieldlist.Count + 1, 8, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                        GridViewFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewFieldTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewFieldTable.Rows[1].Range.Font.Bold = 2;

                        GridViewFieldTable.Cell(1, 1).Range.Text = "FieldName";
                        GridViewFieldTable.Cell(1, 2).Range.Text = "Caption";
                        GridViewFieldTable.Cell(1, 3).Range.Text = "Editor";
                        GridViewFieldTable.Cell(1, 4).Range.Text = "EditorOptions";
                        GridViewFieldTable.Columns[4].PreferredWidth = 20;
                        GridViewFieldTable.Cell(1, 5).Range.Text = "Format";
                        GridViewFieldTable.Cell(1, 6).Range.Text = "Sortable";
                        GridViewFieldTable.Cell(1, 7).Range.Text = "Frozen";
                        GridViewFieldTable.Cell(1, 8).Range.Text = "Total";
                        for (int j = 0; j < fieldlist.Count; j++)
                        {
                            JObject field = fieldlist[j] as JObject;
                            GridViewFieldTable.Cell(j + 2, 1).Range.Text = field["FieldName"] != null ? field["FieldName"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 2).Range.Text = field["Caption"] != null ? field["Caption"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 3).Range.Text = field["Editor"] != null ? field["Editor"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 4).Range.Text = field["EditorOptions"] != null ? field["EditorOptions"].ToString().Replace("\r\n", "") : "";
                            GridViewFieldTable.Cell(j + 2, 5).Range.Text = field["Format"] != null ? field["Format"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 6).Range.Text = field["Sortable"] != null ? field["Sortable"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 7).Range.Text = field["Frozen"] != null ? field["Frozen"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 8).Range.Text = field["Total"] != null ? field["Total"].ToString() : "";
                        }
                        GridViewFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        //query columns
                        if (datagrid["QueryColumns"] != null)
                        {
                            JArray queryFieldlist = datagrid["QueryColumns"] as JArray;
                            if (queryFieldlist.Count > 0)
                            {
                                selection.TypeText("【Query Columns】");
                                selection.TypeParagraph();

                                Table GridViewQueryFieldTable = oDoc.Tables.Add(selection.Range, queryFieldlist.Count + 1, 7, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                                GridViewQueryFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                                GridViewQueryFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                                GridViewQueryFieldTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                                GridViewQueryFieldTable.Rows[1].Range.Font.Bold = 2;

                                GridViewQueryFieldTable.Cell(1, 1).Range.Text = "FieldName";
                                GridViewQueryFieldTable.Cell(1, 2).Range.Text = "Caption";
                                GridViewQueryFieldTable.Cell(1, 3).Range.Text = "Condition";
                                GridViewQueryFieldTable.Cell(1, 4).Range.Text = "Editor";
                                GridViewQueryFieldTable.Cell(1, 5).Range.Text = "DefaultValue";
                                GridViewQueryFieldTable.Cell(1, 6).Range.Text = "AndOr";
                                GridViewQueryFieldTable.Cell(1, 7).Range.Text = "NewLine";
                                for (int j = 0; j < queryFieldlist.Count; j++)
                                {
                                    JObject field = queryFieldlist[j] as JObject;
                                    GridViewQueryFieldTable.Cell(j + 2, 1).Range.Text = field["FieldName"] != null ? field["FieldName"].ToString() : "";
                                    GridViewQueryFieldTable.Cell(j + 2, 2).Range.Text = field["Caption"] != null ? field["Caption"].ToString() : "";
                                    GridViewQueryFieldTable.Cell(j + 2, 3).Range.Text = field["Condition"] != null ? field["Condition"].ToString() : "";
                                    GridViewQueryFieldTable.Cell(j + 2, 4).Range.Text = field["Editor"] != null ? field["Editor"].ToString() : "";
                                    GridViewQueryFieldTable.Cell(j + 2, 5).Range.Text = field["DefaultValue"] != null ? field["DefaultValue"].ToString() : "";
                                    GridViewQueryFieldTable.Cell(j + 2, 6).Range.Text = field["AndOr"] != null ? field["AndOr"].ToString() : "";
                                    GridViewQueryFieldTable.Cell(j + 2, 7).Range.Text = field["NewLine"] != null ? field["NewLine"].ToString() : "";
                                }
                                GridViewQueryFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                                selection.EndKey(ref Wdstory, ref oMissing);
                                selection.TypeParagraph();
                            }
                        }
                        //ToolItems
                        if (datagrid["TooItems"] != null)
                        {
                            JArray tooItemslist = datagrid["TooItems"] as JArray;
                            if (tooItemslist.Count > 0)
                            {
                                selection.TypeText("【Tool Items】");
                                selection.TypeParagraph();

                                Table GridViewtooItemsTable = oDoc.Tables.Add(selection.Range, tooItemslist.Count + 1, 5, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                                GridViewtooItemsTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                                GridViewtooItemsTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                                GridViewtooItemsTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                                GridViewtooItemsTable.Rows[1].Range.Font.Bold = 2;

                                GridViewtooItemsTable.Cell(1, 1).Range.Text = "ID";
                                GridViewtooItemsTable.Cell(1, 2).Range.Text = "Icon";
                                GridViewtooItemsTable.Cell(1, 3).Range.Text = "ItemType";
                                GridViewtooItemsTable.Cell(1, 4).Range.Text = "Text";
                                GridViewtooItemsTable.Cell(1, 5).Range.Text = "OnClick";
                                for (int j = 0; j < tooItemslist.Count; j++)
                                {
                                    JObject field = tooItemslist[j] as JObject;
                                    GridViewtooItemsTable.Cell(j + 2, 1).Range.Text = field["ID"] != null ? field["ID"].ToString() : "";
                                    GridViewtooItemsTable.Cell(j + 2, 2).Range.Text = field["Icon"] != null ? field["Icon"].ToString() : "";
                                    GridViewtooItemsTable.Cell(j + 2, 3).Range.Text = field["ItemType"] != null ? field["ItemType"].ToString() : "";
                                    GridViewtooItemsTable.Cell(j + 2, 4).Range.Text = field["Text"] != null ? field["Text"].ToString() : "";
                                    GridViewtooItemsTable.Cell(j + 2, 5).Range.Text = field["OnClick"] != null ? field["OnClick"].ToString() : "";
                                }
                                GridViewtooItemsTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                                selection.EndKey(ref Wdstory, ref oMissing);
                                selection.TypeParagraph();
                            }
                        }
                        selection.TypeParagraph();
                    }
                }
                #endregion

                #region dataform
                if (printSetting.Length > 1 && printSetting[1].ToString().ToLower() == "true" && dataformlist.Count > 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【DataForm】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    for (int i = 0; i < dataformlist.Count; i++)
                    {
                        JObject dataform = (dataformlist[i] as object[])[1] as JObject;
                        selection.TypeText("【" + dataform["ID"].ToString() + "】");
                        selection.TypeParagraph();
                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 3, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "RemoteName";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 3).Range.Text = "DataMember";
                        GridViewTable.Cell(1, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = dataform["RemoteName"] != null ? dataform["RemoteName"].ToString() : "";
                        GridViewTable.Cell(1, 4).Range.Text = dataform["DataMember"] != null ? dataform["DataMember"].ToString() : "";
                        GridViewTable.Cell(2, 1).Range.Text = "IsShowFlowIcon";
                        GridViewTable.Cell(2, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 3).Range.Text = "DuplicateCheck";
                        GridViewTable.Cell(2, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 2).Range.Text = dataform["IsShowFlowIcon"] != null ? dataform["IsShowFlowIcon"].ToString() : "";
                        GridViewTable.Cell(2, 4).Range.Text = dataform["DuplicateCheck"] != null ? dataform["DuplicateCheck"].ToString() : "";
                        GridViewTable.Cell(3, 1).Range.Text = "ValidateStyle";
                        GridViewTable.Cell(3, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 3).Range.Text = "ContinueAdd";
                        GridViewTable.Cell(3, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 2).Range.Text = dataform["ValidateStyle"] != null ? dataform["ValidateStyle"].ToString() : "";
                        GridViewTable.Cell(3, 4).Range.Text = dataform["ContinueAdd"] != null ? dataform["ContinueAdd"].ToString() : "";
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        JArray fieldlist = dataform["Columns"] as JArray;
                        Table GridViewFieldTable = oDoc.Tables.Add(selection.Range, fieldlist.Count + 1, 5, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewFieldTable.Columns[4].PreferredWidth = 20;
                        GridViewFieldTable.Columns[4].PreferredWidth = 30;

                        GridViewFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewFieldTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewFieldTable.Rows[1].Range.Font.Bold = 2;

                        GridViewFieldTable.Cell(1, 1).Range.Text = "FieldName";
                        GridViewFieldTable.Cell(1, 2).Range.Text = "Caption";
                        GridViewFieldTable.Cell(1, 3).Range.Text = "Editor";
                        GridViewFieldTable.Cell(1, 4).Range.Text = "EditorOptions";
                        GridViewFieldTable.Cell(1, 5).Range.Text = "Format";

                        for (int j = 0; j < fieldlist.Count; j++)
                        {
                            JObject field = fieldlist[j] as JObject;
                            GridViewFieldTable.Cell(j + 2, 1).Range.Text = field["FieldName"].ToString();
                            GridViewFieldTable.Cell(j + 2, 2).Range.Text = field["Caption"] != null ? field["Caption"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 3).Range.Text = field["Editor"] != null ? field["Editor"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 4).Range.Text = field["EditorOptions"] != null ? field["EditorOptions"].ToString().Replace("\r\n", "") : "";
                            GridViewFieldTable.Cell(j + 2, 5).Range.Text = field["Format"] != null ? field["Format"].ToString() : "";
                        }
                        GridViewFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                        selection.TypeParagraph();
                    }
                }
                #endregion

                #region default
                if (printSetting.Length > 2 && printSetting[2].ToString().ToLower() == "true" && defaultslist.Count > 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【Default】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    for (int i = 0; i < defaultslist.Count; i++)
                    {
                        JObject defaults = (defaultslist[i] as object[])[1] as JObject;
                        selection.TypeText("【" + defaults["ID"].ToString() + "】");
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 1, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "BindingOjbectID";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = defaults["BindingObjectID"] != null ? defaults["BindingObjectID"].ToString() : "";
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        JArray fieldlist = defaults["Columns"] as JArray;
                        Table GridViewFieldTable = oDoc.Tables.Add(selection.Range, fieldlist.Count + 1, 3, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                        GridViewFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewFieldTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewFieldTable.Rows[1].Range.Font.Bold = 2;

                        GridViewFieldTable.Cell(1, 1).Range.Text = "FieldName";
                        GridViewFieldTable.Cell(1, 2).Range.Text = "DefaultValue";
                        GridViewFieldTable.Cell(1, 3).Range.Text = "DefaultMethod";

                        for (int j = 0; j < fieldlist.Count; j++)
                        {
                            JObject field = fieldlist[j] as JObject;
                            GridViewFieldTable.Cell(j + 2, 1).Range.Text = field["FieldName"].ToString();
                            GridViewFieldTable.Cell(j + 2, 2).Range.Text = field["DefaultValue"] != null ? field["DefaultValue"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 3).Range.Text = field["DefaultMethod"] != null ? field["DefaultMethod"].ToString() : "";
                        }
                        GridViewFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                        selection.TypeParagraph();
                    }
                }
                #endregion

                #region validate
                if (printSetting.Length > 3 && printSetting[3].ToString().ToLower() == "true" && validatelist.Count > 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【Validate】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    for (int i = 0; i < validatelist.Count; i++)
                    {
                        JObject validates = (validatelist[i] as object[])[1] as JObject;
                        selection.TypeText("【" + validates["ID"].ToString() + "】");
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 1, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "BindingOjbectID";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = validates["BindingObjectID"] != null ? validates["BindingObjectID"].ToString() : "";
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        JArray fieldlist = validates["Columns"] as JArray;
                        Table GridViewFieldTable = oDoc.Tables.Add(selection.Range, fieldlist.Count + 1, 7, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                        GridViewFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewFieldTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewFieldTable.Rows[1].Range.Font.Bold = 2;

                        GridViewFieldTable.Cell(1, 1).Range.Text = "FieldName";
                        GridViewFieldTable.Cell(1, 2).Range.Text = "CheckNull";
                        GridViewFieldTable.Cell(1, 3).Range.Text = "ValidateType";
                        GridViewFieldTable.Cell(1, 4).Range.Text = "CheckRangeFrom";
                        GridViewFieldTable.Cell(1, 5).Range.Text = "CheckRangeTo";
                        GridViewFieldTable.Cell(1, 6).Range.Text = "CheckMethod";
                        GridViewFieldTable.Cell(1, 7).Range.Text = "Message";

                        for (int j = 0; j < fieldlist.Count; j++)
                        {
                            JObject field = fieldlist[j] as JObject;
                            GridViewFieldTable.Cell(j + 2, 1).Range.Text = field["FieldName"] != null ? field["FieldName"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 2).Range.Text = field["CheckNull"] != null ? field["CheckNull"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 3).Range.Text = field["ValidateType"] != null ? field["ValidateType"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 4).Range.Text = field["CheckRangeFrom"] != null ? field["CheckRangeFrom"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 5).Range.Text = field["CheckRangeTo"] != null ? field["CheckRangeTo"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 6).Range.Text = field["CheckMethod"] != null ? field["CheckMethod"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 7).Range.Text = field["Message"] != null ? field["Message"].ToString() : "";
                        }
                        GridViewFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                        selection.TypeParagraph();
                    }
                }
                #endregion

                #region code
                if (codes != null && codes.Value != null && codes.Value.ToString() != "")
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【Code】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    selection.TypeText(codes.Value.ToString());
                    selection.TypeParagraph();
                    selection.TypeParagraph();
                }
                #endregion

                #region script
                if (script != null && script.Value != null && script.Value.ToString() != "")
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【Script】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    selection.TypeText(script.Value.ToString());
                    selection.TypeParagraph();
                    selection.TypeParagraph();
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteError(ex.Message, "WebPagePrint");
            }

        }
        public void LoadMobilePage2(string xaml, Selection selection, Microsoft.Office.Interop.Word._Application oWord,
Microsoft.Office.Interop.Word._Document oDoc, object[] printSetting)
        {
            if (printSetting == null)
            {
                printSetting = new string[] { "true", "true", "true", "true" };
            }

            JObject context = (JObject)JsonConvert.DeserializeObject(xaml);
            var memberPart = new List<object>();
            var datagridlist = new List<object>();
            var dataformlist = new List<object>();
            var defaultslist = new List<object>();
            var validatelist = new List<object>();
            JArray controls = (JArray)context["controls"];
            JValue codes = (JValue)context["code"];
            JValue script = (JValue)context["script"];

            try
            {
                foreach (var component in controls)
                {
                    var componentType = (string)component["type"];
                    var ct = componentType.Split('.');
                    var type = ct[1].ToString();
                    var properties = component["properties"] as JObject;
                    var id = (string)properties["ID"];
                    //add name 
                    properties["Name"] = id;
                    if (type.ToLower() == "jqdatagrid")
                        datagridlist.Add(new object[] { id, properties });
                    if (type.ToLower() == "jqdataform")
                        dataformlist.Add(new object[] { id, properties });
                    if (type.ToLower() == "jqdefault")
                        defaultslist.Add(new object[] { id, properties });
                    if (type.ToLower() == "jqvalidate")
                        validatelist.Add(new object[] { id, properties });
                    if (type.ToLower() == "jqdialog")
                        getHTMLChildren((JObject)component, datagridlist, dataformlist, defaultslist, validatelist);
                }
                #region datagrid
                if (printSetting.Length > 0 && printSetting[0].ToString().ToLower() == "true" && datagridlist.Count > 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【DataGrid】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    for (int i = 0; i < datagridlist.Count; i++)
                    {
                        JObject datagrid = (datagridlist[i] as object[])[1] as JObject;
                        selection.TypeText("【" + datagrid["ID"].ToString() + "】");
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 4, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "RemoteName";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 3).Range.Text = "DataMember";
                        GridViewTable.Cell(1, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = datagrid["RemoteName"] != null ? datagrid["RemoteName"].ToString() : "";
                        GridViewTable.Cell(1, 4).Range.Text = datagrid["DataMember"] != null ? datagrid["DataMember"].ToString() : "";
                        GridViewTable.Cell(2, 1).Range.Text = "Title";
                        GridViewTable.Cell(2, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 3).Range.Text = "AlwaysClose";
                        GridViewTable.Cell(2, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 2).Range.Text = datagrid["Title"] != null ? datagrid["Title"].ToString() : "";
                        GridViewTable.Cell(2, 4).Range.Text = datagrid["AlwaysClose"] != null ? datagrid["AlwaysClose"].ToString() : "";
                        GridViewTable.Cell(3, 1).Range.Text = "EditFormID";
                        GridViewTable.Cell(3, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 3).Range.Text = "EditMode";
                        GridViewTable.Cell(3, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 2).Range.Text = datagrid["EditFormID"] != null ? datagrid["EditFormID"].ToString() : "";
                        GridViewTable.Cell(3, 4).Range.Text = datagrid["EditMode"] != null ? datagrid["EditMode"].ToString() : "";
                        GridViewTable.Cell(4, 1).Range.Text = "DetailObjectID";
                        GridViewTable.Cell(4, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(4, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(4, 3).Range.Text = "";
                        GridViewTable.Cell(4, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(4, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(4, 2).Range.Text = datagrid["DetailObjectID"] != null ? datagrid["DetailObjectID"].ToString() : "";
                        GridViewTable.Cell(4, 4).Range.Text = "";
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        JArray fieldlist = datagrid["Columns"] as JArray;
                        Table GridViewFieldTable = oDoc.Tables.Add(selection.Range, fieldlist.Count + 1, 6, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                        GridViewFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewFieldTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewFieldTable.Rows[1].Range.Font.Bold = 2;

                        GridViewFieldTable.Cell(1, 1).Range.Text = "FieldName";
                        GridViewFieldTable.Cell(1, 2).Range.Text = "Caption";
                        GridViewFieldTable.Cell(1, 3).Range.Text = "Alignment";
                        GridViewFieldTable.Cell(1, 4).Range.Text = "Width";
                        GridViewFieldTable.Cell(1, 5).Range.Text = "Format";
                        GridViewFieldTable.Cell(1, 6).Range.Text = "FormatScript";

                        for (int j = 0; j < fieldlist.Count; j++)
                        {
                            JObject field = fieldlist[j] as JObject;
                            GridViewFieldTable.Cell(j + 2, 1).Range.Text = field["FieldName"].ToString();
                            GridViewFieldTable.Cell(j + 2, 2).Range.Text = field["Caption"] != null ? field["Caption"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 3).Range.Text = field["Alignment"] != null ? field["Alignment"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 4).Range.Text = field["Width"] != null ? field["Width"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 5).Range.Text = field["Format"] != null ? field["Format"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 6).Range.Text = field["FormatScript"] != null ? field["FormatScript"].ToString() : "";
                        }
                        GridViewFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        //query columns
                        JArray queryFieldlist = datagrid["QueryColumns"] as JArray;
                        if (queryFieldlist != null && queryFieldlist.Count > 0)
                        {
                            selection.TypeText("【Query Columns】");
                            selection.TypeParagraph();
                            Table GridViewQueryFieldTable = oDoc.Tables.Add(selection.Range, queryFieldlist.Count + 1, 7, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewQueryFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                            GridViewQueryFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewQueryFieldTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewQueryFieldTable.Rows[1].Range.Font.Bold = 2;

                            GridViewQueryFieldTable.Cell(1, 1).Range.Text = "FieldName";
                            GridViewQueryFieldTable.Cell(1, 2).Range.Text = "Caption";
                            GridViewQueryFieldTable.Cell(1, 3).Range.Text = "Condition";
                            GridViewQueryFieldTable.Cell(1, 4).Range.Text = "Editor";
                            GridViewQueryFieldTable.Cell(1, 5).Range.Text = "EditorOptions";
                            GridViewFieldTable.Columns[5].PreferredWidth = 20;
                            GridViewQueryFieldTable.Cell(1, 6).Range.Text = "DataType";
                            GridViewQueryFieldTable.Cell(1, 7).Range.Text = "IsNvarChar";
                            for (int j = 0; j < queryFieldlist.Count; j++)
                            {
                                JObject field = queryFieldlist[j] as JObject;
                                GridViewQueryFieldTable.Cell(j + 2, 1).Range.Text = field["FieldName"].ToString();
                                GridViewQueryFieldTable.Cell(j + 2, 2).Range.Text = field["Caption"] != null ? field["Caption"].ToString() : "";
                                GridViewQueryFieldTable.Cell(j + 2, 3).Range.Text = field["Condition"] != null ? field["Condition"].ToString() : "";
                                GridViewQueryFieldTable.Cell(j + 2, 4).Range.Text = field["Editor"] != null ? field["Editor"].ToString() : "";
                                GridViewQueryFieldTable.Cell(j + 2, 5).Range.Text = field["EditorOptions"] != null ? field["EditorOptions"].ToString().Replace("\r\n", "") : "";
                                GridViewQueryFieldTable.Cell(j + 2, 6).Range.Text = field["DataType"] != null ? field["DataType"].ToString() : "";
                                GridViewQueryFieldTable.Cell(j + 2, 7).Range.Text = field["IsNvarChar"] != null ? field["IsNvarChar"].ToString() : "";
                            }
                            GridViewQueryFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }

                        //ToolItems
                        JArray toolItemslist = datagrid["ToolItems"] as JArray;
                        if (toolItemslist != null && toolItemslist.Count > 0)
                        {
                            selection.TypeText("【Tool Items】");
                            selection.TypeParagraph();
                            Table GridViewtooItemsTable = oDoc.Tables.Add(selection.Range, toolItemslist.Count + 1, 5, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                            GridViewtooItemsTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                            GridViewtooItemsTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                            GridViewtooItemsTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                            GridViewtooItemsTable.Rows[1].Range.Font.Bold = 2;

                            GridViewtooItemsTable.Cell(1, 1).Range.Text = "Name";
                            GridViewtooItemsTable.Cell(1, 2).Range.Text = "Icon";
                            GridViewtooItemsTable.Cell(1, 3).Range.Text = "Visible";
                            GridViewtooItemsTable.Cell(1, 4).Range.Text = "Text";
                            GridViewtooItemsTable.Cell(1, 5).Range.Text = "OnClick";
                            for (int j = 0; j < toolItemslist.Count; j++)
                            {
                                JObject field = toolItemslist[j] as JObject;
                                GridViewtooItemsTable.Cell(j + 2, 1).Range.Text = field["Name"] != null ? field["Name"].ToString() : "";
                                GridViewtooItemsTable.Cell(j + 2, 2).Range.Text = field["Icon"] != null ? field["Icon"].ToString() : "";
                                GridViewtooItemsTable.Cell(j + 2, 3).Range.Text = field["Visible"] != null ? field["Visible"].ToString() : "";
                                GridViewtooItemsTable.Cell(j + 2, 4).Range.Text = field["Text"] != null ? field["Text"].ToString() : "";
                                GridViewtooItemsTable.Cell(j + 2, 5).Range.Text = field["OnClick"] != null ? field["OnClick"].ToString() : "";
                            }
                            GridViewtooItemsTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                            selection.EndKey(ref Wdstory, ref oMissing);
                            selection.TypeParagraph();
                        }

                        selection.TypeParagraph();
                    }
                }
                #endregion

                #region dataform
                if (printSetting.Length > 1 && printSetting[1].ToString().ToLower() == "true" && dataformlist.Count > 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【DataForm】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    for (int i = 0; i < dataformlist.Count; i++)
                    {
                        JObject dataform = (dataformlist[i] as object[])[1] as JObject;
                        selection.TypeText("【" + dataform["ID"].ToString() + "】");
                        selection.TypeParagraph();
                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 3, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "RemoteName";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 3).Range.Text = "DataMember";
                        GridViewTable.Cell(1, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = dataform["RemoteName"] != null ? dataform["RemoteName"].ToString() : "";
                        GridViewTable.Cell(1, 4).Range.Text = dataform["DataMember"] != null ? dataform["DataMember"].ToString() : "";
                        GridViewTable.Cell(2, 1).Range.Text = "IsShowFlowIcon";
                        GridViewTable.Cell(2, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 3).Range.Text = "DuplicateCheck";
                        GridViewTable.Cell(2, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 2).Range.Text = dataform["IsShowFlowIcon"] != null ? dataform["IsShowFlowIcon"].ToString() : "";
                        GridViewTable.Cell(2, 4).Range.Text = dataform["DuplicateCheck"] != null ? dataform["DuplicateCheck"].ToString() : "";
                        GridViewTable.Cell(3, 1).Range.Text = "IsNotifyOFF";
                        GridViewTable.Cell(3, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 3).Range.Text = "Title";
                        GridViewTable.Cell(3, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 2).Range.Text = dataform["IsNotifyOFF"] != null ? dataform["IsNotifyOFF"].ToString() : "";
                        GridViewTable.Cell(3, 4).Range.Text = dataform["Title"] != null ? dataform["Title"].ToString() : "";
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        JArray fieldlist = dataform["Columns"] as JArray;
                        Table GridViewFieldTable = oDoc.Tables.Add(selection.Range, fieldlist.Count + 1, 5, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                        GridViewFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewFieldTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewFieldTable.Rows[1].Range.Font.Bold = 2;

                        GridViewFieldTable.Cell(1, 1).Range.Text = "FieldName";
                        GridViewFieldTable.Cell(1, 2).Range.Text = "Caption";
                        GridViewFieldTable.Cell(1, 3).Range.Text = "Editor";
                        GridViewFieldTable.Cell(1, 4).Range.Text = "EditorOptions";
                        GridViewFieldTable.Columns[4].PreferredWidth = 20;
                        GridViewFieldTable.Cell(1, 5).Range.Text = "Visible";
                        for (int j = 0; j < fieldlist.Count; j++)
                        {
                            JObject field = fieldlist[j] as JObject;
                            GridViewFieldTable.Cell(j + 2, 1).Range.Text = field["FieldName"].ToString();
                            GridViewFieldTable.Cell(j + 2, 2).Range.Text = field["Caption"] != null ? field["Caption"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 3).Range.Text = field["Editor"] != null ? field["Editor"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 4).Range.Text = field["EditorOptions"] != null ? field["EditorOptions"].ToString().Replace("\r\n", "") : "";
                            GridViewFieldTable.Cell(j + 2, 5).Range.Text = field["Visible"] != null ? field["Visible"].ToString() : "";
                        }
                        GridViewFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                        selection.TypeParagraph();
                    }
                }
                #endregion

                #region default
                if (printSetting.Length > 2 && printSetting[2].ToString().ToLower() == "true" && defaultslist.Count > 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【Default】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    for (int i = 0; i < defaultslist.Count; i++)
                    {
                        JObject defaults = (defaultslist[i] as object[])[1] as JObject;
                        selection.TypeText("【" + defaults["ID"].ToString() + "】");
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 1, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "BindingOjbectID";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = defaults["BindingObjectID"] != null ? defaults["BindingObjectID"].ToString() : "";
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        JArray fieldlist = defaults["Columns"] as JArray;
                        Table GridViewFieldTable = oDoc.Tables.Add(selection.Range, fieldlist.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                        GridViewFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewFieldTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewFieldTable.Rows[1].Range.Font.Bold = 2;

                        GridViewFieldTable.Cell(1, 1).Range.Text = "FieldName";
                        GridViewFieldTable.Cell(1, 2).Range.Text = "DefaultValue";
                        GridViewFieldTable.Cell(1, 3).Range.Text = "DefaultMethod";
                        GridViewFieldTable.Cell(1, 4).Range.Text = "RemoteMethod";

                        for (int j = 0; j < fieldlist.Count; j++)
                        {
                            JObject field = fieldlist[j] as JObject;
                            GridViewFieldTable.Cell(j + 2, 1).Range.Text = field["FieldName"].ToString();
                            GridViewFieldTable.Cell(j + 2, 2).Range.Text = field["DefaultValue"] != null ? field["DefaultValue"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 3).Range.Text = field["DefaultMethod"] != null ? field["DefaultMethod"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 4).Range.Text = field["RemoteMethod"] != null ? field["RemoteMethod"].ToString() : "";
                        }
                        GridViewFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                        selection.TypeParagraph();
                    }
                }
                #endregion

                #region validate
                if (printSetting.Length > 3 && printSetting[3].ToString().ToLower() == "true" && validatelist.Count > 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【Validate】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    for (int i = 0; i < validatelist.Count; i++)
                    {
                        JObject validates = (validatelist[i] as object[])[1] as JObject;
                        selection.TypeText("【" + validates["ID"].ToString() + "】");
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 1, 2, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "BindingOjbectID";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = validates["BindingObjectID"] != null ? validates["BindingObjectID"].ToString() : "";
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        JArray fieldlist = validates["Columns"] as JArray;
                        Table GridViewFieldTable = oDoc.Tables.Add(selection.Range, fieldlist.Count + 1, 7, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                        GridViewFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewFieldTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewFieldTable.Rows[1].Range.Font.Bold = 2;

                        GridViewFieldTable.Cell(1, 1).Range.Text = "FieldName";
                        GridViewFieldTable.Cell(1, 2).Range.Text = "CheckNull";
                        GridViewFieldTable.Cell(1, 3).Range.Text = "ValidateType";
                        GridViewFieldTable.Cell(1, 4).Range.Text = "RangeFrom";
                        GridViewFieldTable.Cell(1, 5).Range.Text = "RangeTo";
                        GridViewFieldTable.Cell(1, 6).Range.Text = "CheckMethod";
                        GridViewFieldTable.Cell(1, 7).Range.Text = "ValidateMessage";
                        for (int j = 0; j < fieldlist.Count; j++)
                        {
                            JObject field = fieldlist[j] as JObject;
                            GridViewFieldTable.Cell(j + 2, 1).Range.Text = field["FieldName"].ToString();
                            GridViewFieldTable.Cell(j + 2, 2).Range.Text = field["CheckNull"] != null ? field["CheckNull"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 3).Range.Text = field["ValidateType"] != null ? field["ValidateType"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 4).Range.Text = field["RangeFrom"] != null ? field["RangeFrom"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 5).Range.Text = field["RangeTo"] != null ? field["RangeTo"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 6).Range.Text = field["CheckMethod"] != null ? field["CheckMethod"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 7).Range.Text = field["ValidateMessage"] != null ? field["ValidateMessage"].ToString() : "";
                        }
                        GridViewFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                        selection.TypeParagraph();
                    }
                }
                #endregion

                #region code
                if (codes.Value.ToString() != "")
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【Code】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    selection.TypeText(codes.Value.ToString());
                    selection.TypeParagraph();
                    selection.TypeParagraph();
                }
                #endregion

                #region script
                if (script.Value.ToString() != "")
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【Script】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    selection.TypeText(script.Value.ToString());
                    selection.TypeParagraph();
                    selection.TypeParagraph();
                }
                #endregion

            }
            catch (Exception ex)
            {
                WriteError(ex.Message, "mobilePagePrint");
            }

        }

        public void LoadReport2(string xaml, Selection selection, Microsoft.Office.Interop.Word._Application oWord,
Microsoft.Office.Interop.Word._Document oDoc, object[] printSetting)
        {
            if (printSetting == null)
            {
                printSetting = new string[] { "true", "true", "true", "true" };
            }

            JObject context = (JObject)JsonConvert.DeserializeObject(xaml);
            var jqreportlist = new List<object>();
            JArray controls = (JArray)context["controls"];
            JValue codes = (JValue)context["code"];
            JValue script = (JValue)context["script"];

            try
            {
                foreach (var component in controls)
                {
                    var componentType = (string)component["type"];
                    var ct = componentType.Split('.');
                    var type = ct[1].ToString();
                    var properties = component["properties"] as JObject;
                    var id = (string)properties["ID"];
                    //add name 
                    properties["Name"] = id;
                    if (type.ToLower() == "jqreport")
                        jqreportlist.Add(new object[] { id, properties });
                }
                #region datagrid
                if (printSetting.Length > 0 && printSetting[0].ToString().ToLower() == "true" && jqreportlist.Count > 0)
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【JQReport】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    for (int i = 0; i < jqreportlist.Count; i++)
                    {
                        JObject datagrid = (jqreportlist[i] as object[])[1] as JObject;
                        selection.TypeText("【" + datagrid["ReportID"].ToString() + "】");
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 2, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "ReportID";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 3).Range.Text = "ReportName";
                        GridViewTable.Cell(1, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = datagrid["ReportID"] != null ? datagrid["ReportID"].ToString() : "";
                        GridViewTable.Cell(1, 4).Range.Text = datagrid["ReportName"] != null ? datagrid["ReportName"].ToString() : "";
                        GridViewTable.Cell(2, 1).Range.Text = "RemoteName";
                        GridViewTable.Cell(2, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 3).Range.Text = "Description";
                        GridViewTable.Cell(2, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 2).Range.Text = datagrid["RemoteName"] != null ? datagrid["RemoteName"].ToString() : "";
                        GridViewTable.Cell(2, 4).Range.Text = datagrid["Description"] != null ? datagrid["Description"].ToString() : "";
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        JArray fieldlist = datagrid["FieldItems"] as JArray;
                        Table GridViewFieldTable = oDoc.Tables.Add(selection.Range, fieldlist.Count + 1, 7, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                        GridViewFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewFieldTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewFieldTable.Rows[1].Range.Font.Bold = 2;
                        GridViewFieldTable.Cell(1, 1).Range.Text = "Field";
                        GridViewFieldTable.Cell(1, 2).Range.Text = "Caption";
                        GridViewFieldTable.Cell(1, 3).Range.Text = "Width";
                        GridViewFieldTable.Cell(1, 4).Range.Text = "Format";
                        GridViewFieldTable.Cell(1, 5).Range.Text = "Total";
                        GridViewFieldTable.Cell(1, 6).Range.Text = "GroupTotal";
                        GridViewFieldTable.Cell(1, 7).Range.Text = "Group";

                        for (int j = 0; j < fieldlist.Count; j++)
                        {
                            JObject field = fieldlist[j] as JObject;
                            GridViewFieldTable.Cell(j + 2, 1).Range.Text = field["Field"].ToString();
                            GridViewFieldTable.Cell(j + 2, 2).Range.Text = field["Caption"] != null ? field["Caption"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 3).Range.Text = field["Width"] != null ? field["Width"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 4).Range.Text = field["Format"] != null ? field["Format"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 5).Range.Text = field["Total"] != null ? field["Total"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 6).Range.Text = field["GroupTotal"] != null ? field["GroupTotal"].ToString() : "";
                            GridViewFieldTable.Cell(j + 2, 7).Range.Text = field["Group"] != null ? field["Group"].ToString() : "";
                        }
                        GridViewFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();

                        selection.TypeParagraph();
                    }
                }
                #endregion

                #region code
                if (codes.Value.ToString() != "")
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【Code】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    selection.TypeText(codes.Value.ToString());
                    selection.TypeParagraph();
                    selection.TypeParagraph();
                }
                #endregion

                #region script
                if (script.Value.ToString() != "")
                {
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText("【Script】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    selection.TypeText(script.Value.ToString());
                    selection.TypeParagraph();
                    selection.TypeParagraph();
                }
                #endregion

            }
            catch (Exception ex)
            {
                WriteError(ex.Message, "reportPagePrint");
            }

        }

        public void LoadFlow2(string xaml, Selection selection, Microsoft.Office.Interop.Word._Application oWord,
Microsoft.Office.Interop.Word._Document oDoc, object[] printSetting, string name)
        {
            JObject context = (JObject)JsonConvert.DeserializeObject(xaml);
            var mainlist = new List<object>();
            var fLActivitylist = new List<object>();
            JArray controls = (JArray)context["controls"];

            try
            {
                foreach (var component in controls)
                {
                    var componentType = (string)component["type"];
                    var ct = componentType.Split('.');
                    if (ct.Length > 1)
                    {
                        if (ct[0].ToString().ToLower() == "fltools")
                        {
                            var type = ct[1].ToString();
                            var properties = component["properties"] as JArray;
                            var mainproperties = new JObject();
                            var id = component["name"];
                            mainproperties["type"] = type;
                            mainproperties["Name"] = id;
                            mainproperties["ID"] = id;
                            foreach (var property in properties)
                            {
                                if (property["name"].ToString() == "ID")
                                {
                                    mainproperties["ID"] = property["value"];
                                    mainproperties["Name"] = property["value"];
                                    id = property["value"];
                                }
                                else if (property["name"].ToString() == "SendToKind")
                                    mainproperties["SendToKind"] = property["value"];
                                else if (property["name"].ToString() == "SendToRole")
                                    mainproperties["SendToRole"] = property["value"];
                                else if (property["name"].ToString() == "NavigatorMode")
                                    mainproperties["NavigatorMode"] = property["value"];
                                else if (property["name"].ToString() == "FLNavigatorMode")
                                    mainproperties["FLNavigatorMode"] = property["value"];
                                else if (property["name"].ToString() == "ApproveRights")
                                    mainproperties["ApproveRights"] = property["value"];
                            }

                            fLActivitylist.Add(mainproperties);
                        }
                    }
                    else if (componentType == "mainProperty")
                    {
                        var properties = component["properties"] as JObject;
                        var mainproperties = new JObject();
                        mainproperties["Name"] = name;
                        foreach (var property in properties["rows"] as JArray)
                        {
                            if (property["name"].ToString() == "Description")
                                mainproperties["Description"] = property["value"];
                            else if (property["name"].ToString() == "TableName")
                                mainproperties["TableName"] = property["value"];
                            else if (property["name"].ToString() == "PresentFields")
                                mainproperties["PresentFields"] = property["value"];
                            else if (property["name"].ToString() == "WebFormName")
                                mainproperties["WebFormName"] = property["value"];
                            else if (property["name"].ToString() == "FormName")
                                mainproperties["FormName"] = property["value"];
                            else if (property["name"].ToString() == "ExpTime")
                                mainproperties["ExpTime"] = property["value"];
                            else if (property["name"].ToString() == "UrgentTime")
                                mainproperties["UrgentTime"] = property["value"];
                            else if (property["name"].ToString() == "TimeUnit")
                                mainproperties["TimeUnit"] = property["value"];
                        }
                        mainlist.Add(new object[] { name, mainproperties });
                    }
                }
                #region Main
                if (mainlist.Count > 0)
                {
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    for (int i = 0; i < mainlist.Count; i++)
                    {
                        JObject main = (mainlist[i] as object[])[1] as JObject;
                        selection.TypeText("【" + main["Name"].ToString() + "】");
                        selection.TypeParagraph();

                        Table GridViewTable = oDoc.Tables.Add(selection.Range, 4, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                        GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].PreferredWidth = 25;
                        GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                        GridViewTable.Columns[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                        GridViewTable.Cell(1, 1).Range.Text = "Description";
                        GridViewTable.Cell(1, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 3).Range.Text = "TableName";
                        GridViewTable.Cell(1, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(1, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(1, 2).Range.Text = main["Description"] != null ? main["Description"].ToString() : "";
                        GridViewTable.Cell(1, 4).Range.Text = main["TableName"] != null ? main["TableName"].ToString() : "";
                        GridViewTable.Cell(2, 1).Range.Text = "PresentFields";
                        GridViewTable.Cell(2, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 3).Range.Text = "WebFormName";
                        GridViewTable.Cell(2, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(2, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(2, 2).Range.Text = main["PresentFields"] != null ? main["PresentFields"].ToString() : "";
                        GridViewTable.Cell(2, 4).Range.Text = main["WebFormName"] != null ? main["WebFormName"].ToString() : "";
                        GridViewTable.Cell(3, 1).Range.Text = "FormName";
                        GridViewTable.Cell(3, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 3).Range.Text = "ExpTime";
                        GridViewTable.Cell(3, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(3, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(3, 2).Range.Text = main["FormName"] != null ? main["FormName"].ToString() : "";
                        GridViewTable.Cell(3, 4).Range.Text = main["ExpTime"] != null ? main["ExpTime"].ToString() : "";
                        GridViewTable.Cell(4, 1).Range.Text = "UrgentTime";
                        GridViewTable.Cell(4, 1).Range.Font.Bold = 2;
                        GridViewTable.Cell(4, 1).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(4, 3).Range.Text = "TimeUnit";
                        GridViewTable.Cell(4, 3).Range.Font.Bold = 2;
                        GridViewTable.Cell(4, 3).Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        GridViewTable.Cell(4, 2).Range.Text = main["UrgentTime"] != null ? main["UrgentTime"].ToString() : "";
                        GridViewTable.Cell(4, 4).Range.Text = main["TimeUnit"] != null ? main["TimeUnit"].ToString() : "";
                        GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                        selection.EndKey(ref Wdstory, ref oMissing);
                        selection.TypeParagraph();
                        selection.TypeParagraph();
                    }
                }
                #endregion

                #region Activity
                if (fLActivitylist.Count > 0)
                {
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();
                    Table GridViewFieldTable = oDoc.Tables.Add(selection.Range, fLActivitylist.Count + 1, 7, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    GridViewFieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;

                    GridViewFieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    GridViewFieldTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    GridViewFieldTable.Rows[1].Range.Font.Bold = 2;
                    GridViewFieldTable.Cell(1, 1).Range.Text = "ID";
                    GridViewFieldTable.Cell(1, 2).Range.Text = "Type";
                    GridViewFieldTable.Cell(1, 3).Range.Text = "SendToKind";
                    GridViewFieldTable.Cell(1, 4).Range.Text = "SendToRole";
                    GridViewFieldTable.Cell(1, 5).Range.Text = "NavigatorMode";
                    GridViewFieldTable.Cell(1, 6).Range.Text = "FLNavigatorMode";
                    GridViewFieldTable.Cell(1, 7).Range.Text = "ApproveRights";

                    for (int j = 0; j < fLActivitylist.Count; j++)
                    {
                        JObject field = fLActivitylist[j] as JObject;
                        GridViewFieldTable.Cell(j + 2, 1).Range.Text = field["ID"].ToString();
                        GridViewFieldTable.Cell(j + 2, 2).Range.Text = field["type"] != null ? field["type"].ToString() : "";
                        GridViewFieldTable.Cell(j + 2, 3).Range.Text = field["SendToKind"] != null ? field["SendToKind"].ToString() : "";
                        GridViewFieldTable.Cell(j + 2, 4).Range.Text = field["SendToRole"] != null ? field["SendToRole"].ToString() : "";
                        GridViewFieldTable.Cell(j + 2, 5).Range.Text = field["NavigatorMode"] != null ? field["NavigatorMode"].ToString() : "";
                        GridViewFieldTable.Cell(j + 2, 6).Range.Text = field["FLNavigatorMode"] != null ? field["FLNavigatorMode"].ToString() : "";
                        GridViewFieldTable.Cell(j + 2, 7).Range.Text = field["ApproveRights"] != null ? field["ApproveRights"].ToString() : "";
                    }
                    GridViewFieldTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();

                    selection.TypeParagraph();

                }
                #endregion

            }
            catch (Exception ex)
            {
                WriteError(ex.Message, "FLowPrint");
            }
        }

        public void LoadDiagram2(string xaml, Selection selection, Microsoft.Office.Interop.Word._Application oWord,
Microsoft.Office.Interop.Word._Document oDoc, object[] printSetting)
        {
            if (printSetting == null)
            {
                printSetting = new string[] { "true", "true", "true", "true" };
            }
            JObject context = (JObject)JsonConvert.DeserializeObject(xaml);
            JArray controls = (JArray)context["controls"];
            var diagramlist = new List<object>();
            var tablelist = new List<object>();
            try
            {
                foreach (var component in controls)
                {
                    var componentType = (string)component["type"];
                    var ct = componentType.Split('.');
                    var type = ct[1].ToString();
                    var options = component["options"] as JObject;
                    var id = (string)options["id"];

                    //add name 
                    if (type.ToLower() == "jqinoutput" || type.ToLower() == "jqshape")
                    {
                        diagramlist.Add(new object[] { id, options });
                    }
                    if (type.ToLower() == "jqtable")
                    {
                        tablelist.Add(new object[] { id, options });
                    }
                }
                #region diagram
                if (diagramlist.Count > 0)
                {
                    selection.TypeText("【Diagram Object】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    Table GridViewTable = oDoc.Tables.Add(selection.Range, diagramlist.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    GridViewTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    GridViewTable.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    GridViewTable.Rows[1].Range.Font.Bold = 2;
                    GridViewTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    GridViewTable.Columns[1].PreferredWidth = 20;
                    GridViewTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    GridViewTable.Columns[2].PreferredWidth = 20;
                    GridViewTable.Columns[3].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    GridViewTable.Columns[3].PreferredWidth = 20;
                    GridViewTable.Columns[4].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    GridViewTable.Columns[4].PreferredWidth = 40;

                    GridViewTable.Cell(1, 1).Range.Text = "ID";
                    GridViewTable.Cell(1, 2).Range.Text = "Text";
                    GridViewTable.Cell(1, 3).Range.Text = "LinkDiagram";
                    GridViewTable.Cell(1, 4).Range.Text = "Description";

                    for (int j = 0; j < diagramlist.Count; j++)
                    {
                        JObject diagram = (diagramlist[j] as object[])[1] as JObject;
                        var properties = diagram["properties"] as JObject;
                        GridViewTable.Cell(j + 2, 1).Range.Text = properties["ID"].ToString();
                        GridViewTable.Cell(j + 2, 2).Range.Text = properties["text"] != null ? properties["text"].ToString() : "";
                        GridViewTable.Cell(j + 2, 3).Range.Text = properties["linkDiagram"] != null ? properties["linkDiagram"].ToString() : "";
                        GridViewTable.Cell(j + 2, 4).Range.Text = "";
                    }
                    GridViewTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();
                }
                #endregion
                #region table
                if (tablelist.Count > 0)
                {
                    selection.TypeText("【Table Object】");
                    selection.Font.Bold = 0;
                    selection.Font.Size = 10;
                    selection.TypeParagraph();

                    Table GridViewTable2 = oDoc.Tables.Add(selection.Range, tablelist.Count + 1, 4, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    GridViewTable2.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    GridViewTable2.Rows[1].Range.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    GridViewTable2.Rows[1].Range.Font.Bold = 2;
                    GridViewTable2.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    GridViewTable2.Columns[1].PreferredWidth = 20;
                    GridViewTable2.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    GridViewTable2.Columns[2].PreferredWidth = 20;
                    GridViewTable2.Columns[3].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    GridViewTable2.Columns[3].PreferredWidth = 20;
                    GridViewTable2.Columns[4].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    GridViewTable2.Columns[4].PreferredWidth = 40;
                    GridViewTable2.Cell(1, 1).Range.Text = "ID";
                    GridViewTable2.Cell(1, 2).Range.Text = "Title";
                    GridViewTable2.Cell(1, 3).Range.Text = "TableName";
                    GridViewTable2.Cell(1, 4).Range.Text = "Description";

                    for (int j = 0; j < tablelist.Count; j++)
                    {
                        JObject table = (tablelist[j] as object[])[1] as JObject;
                        var properties = table["properties"] as JObject;
                        GridViewTable2.Cell(j + 2, 1).Range.Text = properties["ID"].ToString();
                        GridViewTable2.Cell(j + 2, 2).Range.Text = properties["title"] != null ? properties["title"].ToString() : "";
                        GridViewTable2.Cell(j + 2, 3).Range.Text = properties["tableName"] != null ? properties["tableName"].ToString() : "";
                        GridViewTable2.Cell(j + 2, 4).Range.Text = "";
                    }
                    GridViewTable2.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                    selection.EndKey(ref Wdstory, ref oMissing);
                    selection.TypeParagraph();
                }
                #endregion

            }
            catch (Exception ex)
            {
                WriteError(ex.Message, "diagramPrint");
            }

        }


        public void LoadTable2(string xaml, Selection selection, Microsoft.Office.Interop.Word._Application oWord,
Microsoft.Office.Interop.Word._Document oDoc)
        {
            JArray context = (JArray)JsonConvert.DeserializeObject(xaml);
            try
            {
                var tables = new List<object>();
                foreach (var component in context)
                {
                    var tablename = component["table"].ToString();
                    var fields = component["fields"];
                    tables.Add(new object[] { tablename, fields });

                    selection.set_Style(ref oHeadingStyle2);
                    selection.Font.Bold = 2;
                    selection.Font.Size = 14;
                    selection.TypeText(tablename);
                    selection.TypeParagraph();

                    Table fieldTable = oDoc.Tables.Add(selection.Range, fields.Count() + 1, 7, ref WDWord9TableBehavior, ref WDAutoFitWindow);
                    fieldTable.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[1].PreferredWidth = 20;
                    fieldTable.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[2].PreferredWidth = 18;
                    fieldTable.Columns[3].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[3].PreferredWidth = 17;
                    fieldTable.Columns[4].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[4].PreferredWidth = 10;
                    fieldTable.Columns[5].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[5].PreferredWidth = 7;
                    fieldTable.Columns[6].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[6].PreferredWidth = 8;
                    fieldTable.Columns[7].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent;
                    fieldTable.Columns[7].PreferredWidth = 20;
                    fieldTable.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray125;
                    fieldTable.Rows[1].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    fieldTable.Rows[1].Range.Font.Bold = 2;
                    //string fieldtext = SDSysMsg.GetSystemMessage(language, "SDModule", "Document", "field");
                    string fieldtext = "Name,Caption,DataType,Length,ControlType,IsNull,Remark";
                    string[] fieldt = fieldtext.Split(new char[] { ',' });
                    fieldTable.Cell(1, 1).Range.Text = fieldt[0];
                    fieldTable.Cell(1, 2).Range.Text = fieldt[1];
                    fieldTable.Cell(1, 3).Range.Text = fieldt[2];
                    fieldTable.Cell(1, 4).Range.Text = fieldt[3];
                    fieldTable.Cell(1, 5).Range.Text = fieldt[4];
                    fieldTable.Cell(1, 6).Range.Text = fieldt[5];
                    fieldTable.Cell(1, 7).Range.Text = fieldt[6];

                    //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    for (int j = 0; j < fields.Count(); j++)
                    {
                        JObject xac = fields[j] as JObject;
                        fieldTable.Cell(j + 2, 1).Range.Text = xac["IsKey"] != null ? (xac["IsKey"].ToString().ToLower() == "true" ? xac["ColumnName"].ToString() + "(*)" : xac["ColumnName"].ToString()) : xac["ColumnName"].ToString();
                        fieldTable.Cell(j + 2, 2).Range.Text = xac["Caption"] != null ? xac["Caption"].ToString() : "";
                        fieldTable.Cell(j + 2, 3).Range.Text = xac["DataType"] != null ? xac["DataType"].ToString() : "";
                        fieldTable.Cell(j + 2, 4).Range.Text = xac["Length"] != null ? xac["Length"].ToString() : "";
                        fieldTable.Cell(j + 2, 5).Range.Text = xac["ControlType"] != null ? xac["ControlType"].ToString() : "";
                        fieldTable.Cell(j + 2, 6).Range.Text = xac["isNullable"] != null ? xac["isNullable"].ToString() : "";
                        fieldTable.Cell(j + 2, 7).Range.Text = xac["Description"] != null ? xac["Description"].ToString() : "";
                        //selection.MoveDown(ref WdLine, ref WDCount1, ref oMissing);
                    }
                    selection.EndKey(ref Wdstory, ref oMissing);
                }
            }
            catch (Exception ex)
            {
                WriteError(ex.Message, "tablePrint");
            }

        }

        private void getHTMLChildren(JObject control, List<object> datagridlist, List<object> dataformlist, List<object> defaultslist, List<object> validatelist)
        {
            JArray controls = (JArray)control["children"];

            foreach (var component in controls)
            {
                var componentType = (string)component["type"];
                var ct = componentType.Split('.');
                var type = ct[1].ToString();
                var properties = component["properties"] as JObject;
                var id = (string)properties["ID"];
                //add name 
                properties["Name"] = id;
                if (type.ToLower() == "jqdatagrid")
                    datagridlist.Add(new object[] { id, properties });
                if (type.ToLower() == "jqdataform")
                    dataformlist.Add(new object[] { id, properties });
                if (type.ToLower() == "jqdefault")
                    defaultslist.Add(new object[] { id, properties });
                if (type.ToLower() == "jqvalidate")
                    validatelist.Add(new object[] { id, properties });
                if (type.ToLower() == "jqdialog")
                    getHTMLChildren((JObject)component, datagridlist, dataformlist, defaultslist, validatelist);
            }

        }

        public string GetPreviewPath()
        {
            var preview = "";
            DirectoryInfo directorInfo = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);//獲得當前DLL的目錄
            string sPath = directorInfo.Parent.FullName;//獲得當前DLL的上级目錄的全路径
            sPath = sPath + "/serverPath.txt";
            if (File.Exists(sPath))
            {
                using (FileStream fsMyfile = new FileStream(sPath, FileMode.Open, FileAccess.Read))
                {
                    StreamReader sr = new StreamReader(fsMyfile);
                    preview = sr.ReadToEnd();
                }
            }
            else
                WriteError("comman和solution目录下缺少serverPath.txt文件，这个文件是配置列印时截图的singonsignon的路径的", "");
            return preview;
        }

        public void WriteError(string errormsg, string fromwhere)
        {
            DirectoryInfo directorInfo = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);//獲得當前DLL的目錄
            string sPath = directorInfo.Parent.FullName;//獲得當前DLL的上级目錄的全路径
            string dataTimeString = DateTime.Now.ToString("yy-MM-dd");
            if (!Directory.Exists(sPath + "/Log"))//如果不存在就创建Log文件夹
            {
                Directory.CreateDirectory(sPath + "/Log");
            }
            sPath = sPath + "/Log/" + "ErrorLog-" + dataTimeString + ".txt";
            using (FileStream fsMyfile = new FileStream(sPath, FileMode.Append, FileAccess.Write))
            {    // 创建一个数据流写入器，和打开的文件关联
                StreamWriter swMyfile = new StreamWriter(fsMyfile);

                swMyfile.WriteLine(DateTime.Now.ToLongTimeString() + "  from :" + fromwhere + "  Error MSG: " + errormsg);

                swMyfile.WriteLine(); // 冲刷数据(把数据真正写到文件中去)
                // 注释该句试试看，程序将报错
                swMyfile.Flush();
            }
        }

        public class WebSiteThumbnail
        {
            Bitmap m_Bitmap;
            string m_Url;
            int m_BrowserWidth, m_BrowserHeight, m_ThumbnailWidth, m_ThumbnailHeight;
            string m_datagridid;
            public WebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight, string datagridid)
            {
                m_Url = Url;
                m_BrowserHeight = BrowserHeight;
                m_BrowserWidth = BrowserWidth;
                m_ThumbnailWidth = ThumbnailWidth;
                m_ThumbnailHeight = ThumbnailHeight;
                m_datagridid = datagridid;
            }
            public static Bitmap GetWebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight, string datagridid)
            {
                WebSiteThumbnail thumbnailGenerator = new WebSiteThumbnail(Url, BrowserWidth, BrowserHeight, ThumbnailWidth, ThumbnailHeight, datagridid);
                return thumbnailGenerator.GenerateWebSiteThumbnailImage();
            }
            public Bitmap GenerateWebSiteThumbnailImage()
            {
                Thread m_thread = new Thread(new ThreadStart(_GenerateWebSiteThumbnailImage));
                m_thread.SetApartmentState(ApartmentState.STA);
                m_thread.Start();
                m_thread.Join(20000);

                return m_Bitmap;
            }
            private void _GenerateWebSiteThumbnailImage()
            {
                WebBrowser m_WebBrowser = new WebBrowser();
                m_WebBrowser.ScrollBarsEnabled = false;
                m_WebBrowser.Navigate(m_Url);
                m_WebBrowser.Show();
                m_WebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                //while (m_WebBrowser.ReadyState != WebBrowserReadyState.Complete)
                while (WaitWebLoad(m_WebBrowser))
                {
                    System.Windows.Forms.Application.DoEvents();
                }

                m_WebBrowser.Dispose();
            }
            private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
                WebBrowser m_WebBrowser = (WebBrowser)sender;

                m_WebBrowser.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight);
                m_WebBrowser.ScrollBarsEnabled = false;

                m_Bitmap = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height);
                m_WebBrowser.BringToFront();
                m_WebBrowser.DrawToBitmap(m_Bitmap, m_WebBrowser.Bounds);
                m_Bitmap = (Bitmap)m_Bitmap.GetThumbnailImage(m_ThumbnailWidth, m_ThumbnailHeight, null, IntPtr.Zero);
            }
            private void Delay(Int32 DateTimes)
            {
                DateTime curr = DateTime.Now;
                while (curr.AddMilliseconds(DateTimes) > DateTime.Now)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                return;
            }
            private bool WaitWebLoad(WebBrowser webBrowser1)
            {
                int i = 0;
                string sUrl;
                while (true)
                {
                    Delay(500);//系统延迟500毫秒
                    if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
                    {
                        if (!webBrowser1.IsBusy)
                        {
                            i = i + 1;
                            if (i == 2)
                            {
                                sUrl = webBrowser1.Url.ToString();
                                if (sUrl.Contains("preview"))
                                {
                                    WaitTime(100, 5);
                                    if (m_datagridid != "")
                                    {
                                        webBrowser1.Document.InvokeScript("updateItem", new object[] { "#" + m_datagridid });
                                        WaitTime(500, 5);
                                    }
                                    WaitTime(1000,10);
                                    webBrowser1.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight);
                                    webBrowser1.ScrollBarsEnabled = false;
                                    m_Bitmap = new Bitmap(webBrowser1.Bounds.Width, webBrowser1.Bounds.Height);
                                    webBrowser1.BringToFront();
                                    webBrowser1.DrawToBitmap(m_Bitmap, webBrowser1.Bounds);
                                    m_Bitmap = (Bitmap)m_Bitmap.GetThumbnailImage(m_ThumbnailWidth, m_ThumbnailHeight, null, IntPtr.Zero);
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                            continue;
                        }
                        i = 0;
                    }
                }
            }
            private bool WaitTime(int time, int times)
            {
                var j = 0;
                while (true)
                {
                    Delay(time);//系统延迟2500毫秒
                    j++;
                    if (j > times) return false;
                    continue;
                }
            }
            public void WriteError(string errormsg, string fromwhere)
            {
                DirectoryInfo directorInfo = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);//獲得當前DLL的目錄
                string sPath = directorInfo.Parent.FullName;//獲得當前DLL的上级目錄的全路径
                string dataTimeString = DateTime.Now.ToString("yy-MM-dd");
                if (!Directory.Exists(sPath + "/Log"))//如果不存在就创建Log文件夹
                {
                    Directory.CreateDirectory(sPath + "/Log");
                }
                sPath = sPath + "/Log/" + "testlog-" + dataTimeString + ".txt";
                using (FileStream fsMyfile = new FileStream(sPath, FileMode.Append, FileAccess.Write))
                {    // 创建一个数据流写入器，和打开的文件关联
                    StreamWriter swMyfile = new StreamWriter(fsMyfile);

                    swMyfile.WriteLine(DateTime.Now.ToLongTimeString() + "  from :" + fromwhere + "  Error MSG: " + errormsg);

                    swMyfile.WriteLine(); // 冲刷数据(把数据真正写到文件中去)
                    // 注释该句试试看，程序将报错
                    swMyfile.Flush();
                }
            }

        }
        public object ExpiryDateEmailSchedul(object[] paramters)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            string databasename = string.Empty;
            string dbAlias = string.Empty;
            string ret = string.Empty;
            int index = 0;
            try
            {
                dbAlias = DbConnectionSet.SystemDatabase;
                connection = AllocateConnection(dbAlias);
                transaction = connection.BeginTransaction();
                string strSql = "select * from SYS_SDUSERS where Active = 'Y'";
                DataSet ds = this.ExecuteSql(strSql, connection, transaction);
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0].ToString() != "")
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        String message = "";
                        var userid = dr["USERID"].ToString();
                        var email = dr["EMAIL"];
                        var ExpiryDate = dr["ExpiryDate"];
                        if (ExpiryDate != null && email != null && ExpiryDate != DBNull.Value && email != DBNull.Value)
                        {
                            DateTime expirydate = DateTime.Parse(ExpiryDate.ToString());
                            var edate = expirydate.ToString("yyyyMMdd");
                            var ndate = DateTime.Now.ToString("yyyyMMdd");
                            if (expirydate.AddDays(-7).ToString("yyyyMMdd") == ndate || expirydate.AddDays(-3).ToString("yyyyMMdd") == ndate)
                            {
                                try
                                {
                                    String Email = "297247674@qq.com";
                                    String EmailPwd = "lnfox810729";
                                    String Host = "smtp.qq.com";
                                    String Port = "25";
                                    String SSL = "false";

                                    System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                                    msg.To.Add(email.ToString());
                                    msg.From = new System.Net.Mail.MailAddress(Email, "Infolight", System.Text.Encoding.UTF8);
                                    msg.Subject = "EEPCloud帳號即將過期通知";
                                    msg.SubjectEncoding = System.Text.Encoding.UTF8;
                                    msg.Body = string.Format(@"    親愛的用戶, 你的EEPCloud帳號:{0} 將於 {1} 到期, 請保握測試的時間, 如果你希望保留你目前所設計的系統, 請利用我們所提供的Export或Backup功能來進行備份作業, 謝謝你的使用.            

訊光科技 
EEPCloud客服小組.", userid, expirydate.ToString("MM/dd/yyyy"));
                                    msg.BodyEncoding = System.Text.Encoding.UTF8;
                                    msg.IsBodyHtml = false;
                                    msg.Priority = System.Net.Mail.MailPriority.High;

                                    //Add the Creddentials
                                    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                                    client.Credentials = new System.Net.NetworkCredential(Email, EmailPwd);
                                    client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                                    client.EnableSsl = SSL.ToLower() == "true" ? true : false;

                                    if (Host != "")
                                        client.Host = Host;
                                    if (Port != "")
                                        client.Port = Convert.ToInt16(Port);
                                    //client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
                                    object userState = msg;
                                    if (client != null)
                                    {
                                        try
                                        {
                                            //you can also call client.Send(msg)
                                            //client.SendAsync(msg, userState);
                                            client.Send(msg);
                                        }
                                        catch (Exception ex)
                                        {
                                            message = ex.Message;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    message = ex.Message;
                                }


                            }
                        }
                        if (message != "")
                            WriteError(message, "mailtest");
                    }
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                WriteError(ret, "mailtest2");
                transaction.Rollback();
                return new object[] { 1, ret };
            }
            finally
            {
                transaction.Dispose();
                ReleaseConnection(dbAlias, connection);
            }
            return new object[] { 0, ret };

        }

        #region Rei
        public object GetServerPath(object[] oParam)
        {
            return new object[] { 0, "111" };
        }

        public object GetTableNamesFromSql(object[] oParams)
        {
            String sReturnValue = String.Empty;

            String sSql = oParams[0].ToString();
            String sDBAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
            IDbConnection _conn = AllocateConnection(sDBAlias);
            try
            {
                System.Data.DataTable dtTableSchema = GetTableSchemaFromSQL(_conn, sSql);
                List<String> existedTableNameList = GetExistedTablesList(_conn, dtTableSchema, "");
                foreach (var item in existedTableNameList)
                {
                    sReturnValue += item + ";";
                }
            }
            finally
            {
                ReleaseConnection(sDBAlias, _conn);
            }

            return new object[] { 0, sReturnValue };
        }

        public object GetColumnNamesByTableName(object[] oParams)
        {
            String sReturnValue = String.Empty;
            String sTableName = oParams[0].ToString();
            String sDBAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
            IDbConnection _conn = AllocateConnection(sDBAlias);
            try
            {
                String sQL = "select * from " + sTableName;
                IDbCommand cmd = _conn.CreateCommand();
                cmd.CommandText = sQL;
                if (_conn.State == ConnectionState.Closed)
                { _conn.Open(); }

                IDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                System.Data.DataTable schemaTable = reader.GetSchemaTable();

                _conn.Close();

                List<String> lColumns = new List<string>();
                foreach (DataRow r in schemaTable.Rows)
                {
                    lColumns.Add(r["ColumnName"].ToString());
                }

                foreach (var item in lColumns)
                {
                    sReturnValue += item + ";";
                }
            }
            finally
            {
                ReleaseConnection(sDBAlias, _conn);
            }
            return new object[] { 0, sReturnValue };
        }

        public object GetColumnNamesFromSQL(object[] oParams)
        {
            String sReturnValue = String.Empty;
            String sSql = oParams[0].ToString();
            if (!String.IsNullOrEmpty(sSql))
            {
                String sDBAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                IDbConnection _conn = AllocateConnection(sDBAlias);
                try
                {
                    System.Data.DataTable dtSchema = GetTableSchemaFromSQL(_conn, sSql);

                    List<String> lColumns = new List<string>();
                    foreach (DataRow r in dtSchema.Rows)
                    {
                        lColumns.Add(String.Format("{0}.{1}", r["BaseTableName"].ToString(), r["ColumnName"].ToString()));
                    }

                    foreach (var item in lColumns)
                    {
                        sReturnValue += item + ";";
                    }
                }
                finally
                {
                    ReleaseConnection(sDBAlias, _conn);
                }
            }
            return new object[] { 0, sReturnValue };
        }

        private System.Data.DataTable GetTableSchemaFromSQL(IDbConnection _conn, String sSql)
        {
            try
            {
                IDbCommand cmd = _conn.CreateCommand();
                cmd.CommandText = sSql;
                if (_conn.State == ConnectionState.Closed)
                { _conn.Open(); }

                IDataReader reader = null;
                System.Data.DataTable dtSchema = null;

                cmd.ExecuteNonQuery();
                reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);
                dtSchema = reader.GetSchemaTable();

                return dtSchema;
            }
            finally
            {

            }
        }

        private List<String> GetExistedTablesList(IDbConnection _conn, System.Data.DataTable schema, String TableName)
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

        public object[] GetAllTables(object[] oParams)
        {
            String sReturnValue = String.Empty;
            String sDBAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
            IDbConnection _conn = AllocateConnection(sDBAlias);
            try
            {
                String sQL = String.Empty;
                String sName = String.Empty;
                String sType = ((int)Srvtools.DbConnectionSet.GetDbConn(sDBAlias, this.DeveloperID).DbType).ToString();

                switch (sType)
                {
                    case "1":
                        sQL = "select name from sysobjects where (xtype='U') order by name";
                        sName = "name";
                        break;
                    case "2":
                        sQL = "sp_help";
                        sName = "tabname";
                        break;
                    case "3":
                        sQL = "select OBJECT_NAME from USER_OBJECTS where (OBJECT_TYPE = 'TABLE') order by OBJECT_NAME";
                        sName = "OBJECT_NAME";
                        break;
                    case "4":
                        sQL = "select * from SYSTABLES where (TABTYPE = 'T') and TABID >= 100 order by TABNAME";
                        sName = "NAME";
                        break;
                    case "5":
                        sQL = "show tables;";
                        sName = "name";
                        break;
                    case "6":
                        sQL = "select * from SYSTABLES where (TABTYPE = 'T') and TABID >= 100 order by TABNAME";
                        sName = "tabname";
                        break;
                }

                IDbCommand cmd = _conn.CreateCommand();
                cmd.CommandText = sQL;
                if (_conn.State == ConnectionState.Closed)
                { _conn.Open(); }

                if (sType != "5")
                {
                    IDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
                    while (reader.Read())
                    {

                        String sTableName = reader[sName].ToString();
                        if (reader[sName].ToString().Contains(" "))
                        {
                            sTableName = String.Format("[{0}]", reader[sName].ToString());
                        }
                        sReturnValue += sTableName + ";";

                    }
                }
                else
                {
                    DataSet dsReturn = new DataSet();
                    IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);
                    adpater.Fill(dsReturn);
                    foreach (DataRow row in dsReturn.Tables[0].Rows)
                    {
                        String sTableName = row[0].ToString();
                        sReturnValue += sTableName + ";";
                    }
                }
            }
            finally
            {
                ReleaseConnection(sDBAlias, _conn);
            }
            return new object[] { 0, sReturnValue };
        }
        public object[] GetAllViewAndSP(object[] oParams)
        {
            string type = oParams[0].ToString();
            String sReturnValue = String.Empty;
            String sDBAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
            IDbConnection _conn = AllocateConnection(sDBAlias);
            try
            {
                String sQL = String.Empty;
                String sName = String.Empty;
                String sType = ((int)Srvtools.DbConnectionSet.GetDbConn(sDBAlias, this.DeveloperID).DbType).ToString();
                var xtype = "";
                switch (sType)
                {
                    case "1":
                        if (type == "sp") xtype = "P";
                        else if (type == "view") xtype = "V";
                        sQL = "select name from sysobjects where (xtype='" + xtype + "') order by name";
                        sName = "name";
                        break;
                    case "2":
                        sQL = "sp_help";
                        sName = "tabname";
                        break;
                    case "3":
                        if (type == "sp") xtype = "VIEW";
                        else if (type == "view") xtype = "VIEW";
                        sQL = "select OBJECT_NAME from USER_OBJECTS where (OBJECT_TYPE = 'VIEW') order by OBJECT_NAME";
                        sName = "OBJECT_NAME";
                        break;
                    case "4":
                        sQL = "select * from SYSTABLES where (TABTYPE = 'V') and TABID >= 100 order by TABNAME";
                        sName = "NAME";
                        break;
                    case "5":
                        if (type == "sp")
                            sQL = "select `name` from mysql.proc where db = '" + _conn.Database.ToLower() + "' and `type` = 'PROCEDURE'";//"show procedure status  where db='" + _conn.Database + "';";
                        else if (type == "view")
                            sQL = "show table status where comment='view';";
                        sName = "name";
                        break;
                    case "6":
                        sQL = "select * from SYSTABLES where (TABTYPE = 'V') and TABID >= 100 order by TABNAME";
                        sName = "tabname";
                        break;
                }

                IDbCommand cmd = _conn.CreateCommand();
                cmd.CommandText = sQL;
                if (_conn.State == ConnectionState.Closed)
                { _conn.Open(); }

                IDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);

                while (reader.Read())
                {
                    String sTableName = String.Empty;
                    if (sType == "5")
                    {
                        sTableName = reader[0].ToString();
                    }
                    else
                    {
                        sTableName = reader[sName].ToString();
                    }
                    if (sTableName.Contains(" "))
                    {
                        sTableName = String.Format("[{0}]", sTableName);
                    }
                    sReturnValue += sTableName + ";";
                }
            }
            finally
            {
                ReleaseConnection(sDBAlias, _conn);
            }
            return new object[] { 0, sReturnValue };
        }

        public object[] updateViewAndSP(object[] oParams)
        {
            string type = oParams[0].ToString();
            string name = oParams[1].ToString();
            string sql = oParams[2].ToString();
            String sDBAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
            IDbConnection _conn = AllocateConnection(sDBAlias);
            IDbTransaction transaction = null;
            try
            {
                transaction = _conn.BeginTransaction();
                String sQL = String.Empty;
                String sName = String.Empty;
                String sType = ((int)Srvtools.DbConnectionSet.GetDbConn(sDBAlias, this.DeveloperID).DbType).ToString();
                var xtype = "";
                switch (sType)
                {
                    case "1":
                        sQL = "DROP " + type + " [" + name + "]";
                        break;
                }
                if (_conn.State == ConnectionState.Closed)
                { _conn.Open(); }
                this.ExecuteSql(sQL, _conn, transaction);
                switch (sType)
                {
                    case "1":
                        sQL = sql;
                        break;
                }
                this.ExecuteSql(sQL, _conn, transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                ReleaseConnection(sDBAlias, _conn);
            }
            return new object[] { 0, "" };
        }

        private string GetDataBaseTypeforString(String sDBAlias)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemFile.DBFile);

            XmlNode node = xmlDoc.FirstChild.FirstChild.SelectSingleNode(sDBAlias);
            if (node == null)
                return "";
            string DBType = node.Attributes["Type"].Value.Trim();
            return DBType;
        }

        public object AutoSeqMenuID(object[] objParam)
        {
            int iReturnValue = 0;
            String sDBAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
            IDbConnection _conn = AllocateConnection(sDBAlias);
            try
            {
                String sQL = String.Empty;
                String sType = ((int)Srvtools.DbConnectionSet.GetDbConn(sDBAlias, this.DeveloperID).DbType).ToString();
                switch (sType)
                {
                    case "1":
                        sQL = "select max(convert(int,MENUID)) from MENUTABLE where isnumeric(MENUID)=1";
                        break;
                    case "2":
                        sQL = "select max(convert(int,MENUID)) from MENUTABLE";
                        break;
                    case "3":
                        sQL = "select max(to_number(MENUID)) from MENUTABLE";
                        break;
                    case "4":
                        sQL = "select max(MENUID) from MENUTABLE";
                        break;
                    case "5":
                        sQL = "select max(MENUID) from MENUTABLE";
                        break;
                    case "6":
                        sQL = "select max(MENUID) from MENUTABLE";
                        break;
                }

                IDbCommand cmd = _conn.CreateCommand();
                cmd.CommandText = sQL;
                IDataReader dr = cmd.ExecuteReader();
                dr.Read();
                string count = dr[0].ToString();
                cmd.Cancel();
                dr.Close();
                if (count == "") count = "0";
                iReturnValue = Convert.ToInt32(count) + 1;
            }
            finally
            {
                ReleaseConnection(sDBAlias, _conn);
            }
            return new object[] { 0, iReturnValue };
        }
        #endregion
    }
}