using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.Collections;
using System.Data;
using System.Linq;

namespace Srvtools
{
    public class RecordLock
    {
        public RecordLock()
        { 
        
        }



        public static string RecordFileName = SystemFile.RecordLockFile;

        public enum LockType
        { 
            Idle,
            Deleting,
            Updating,
            Other
        }

        private static XmlNode FindAlias(XmlDocument xml, string DBAlias)
        {
            XmlNodeList xnl = xml.DocumentElement.SelectNodes("DataBase");
            for (int i = 0; i < xnl.Count; i++)
            {
                if (xnl[i].Attributes["Alias"].Value == DBAlias)
                {
                    return xnl[i];
                }
            }
            return null;
        }

        private static XmlNode FindTable(XmlNode xml, string TableName)
        {
            XmlNodeList xnl = xml.SelectNodes("Table");
            for (int i = 0; i < xnl.Count; i++)
            {
                if (xnl[i].Attributes["Name"].Value == TableName)
                {
                    return xnl[i];
                }
            }
            return null;
        }

        private static XmlNode FindRecord(XmlNode xml, string KeyFields, string KeyValues)
        {
            XmlNodeList xnl = xml.SelectNodes("Record");
            for (int i = 0; i < xnl.Count; i++)
            {
                if (xnl[i].Attributes["KeyFields"].Value == KeyFields && xnl[i].Attributes["KeyValues"].Value == KeyValues)
                {
                    return xnl[i];
                }
            }
            return null;
        }

        //creat DBAlias, Table, Record node
        private static XmlNode CreatRecord(XmlDocument xml, string DBAlias, string TableName, string KeyFields, string KeyValues, string UserID)
        {
            return CreatRecord(xml, DBAlias, TableName, KeyFields, KeyValues, UserID, LockType.Updating);
        }
        private static XmlNode CreatRecord(XmlDocument xml, string DBAlias, string TableName, string KeyFields, string KeyValues, string UserID, LockType lt)
        {
            XmlElement nodeDBAlias = xml.CreateElement("DataBase");
            XmlAttribute attAlias = xml.CreateAttribute("Alias");
            attAlias.Value = DBAlias;
            nodeDBAlias.Attributes.Append(attAlias);
            xml.DocumentElement.AppendChild(nodeDBAlias);
            return CreatRecord(xml, nodeDBAlias, TableName, KeyFields, KeyValues, UserID, lt);// call creat Table, Record node
        }

        //creat Table, Record node
        private static XmlNode CreatRecord(XmlDocument xml, XmlNode nodeDBAlias, string TableName, string KeyFields, string KeyValues, string UserID)
        {
            return CreatRecord(xml, nodeDBAlias, TableName, KeyFields, KeyValues, UserID, LockType.Updating);
        }
        //creat Table, Record node
        private static XmlNode CreatRecord(XmlDocument xml, XmlNode nodeDBAlias, string TableName, string KeyFields, string KeyValues, string UserID, LockType lt)
        {
            XmlElement nodeTable = xml.CreateElement("Table");
            XmlAttribute attName = xml.CreateAttribute("Name");
            attName.Value = TableName;
            nodeTable.Attributes.Append(attName);
            nodeDBAlias.AppendChild(nodeTable);
            return CreatRecord(xml, nodeTable, KeyFields, KeyValues, UserID, lt);// call creat Record node
        }


        //creat Record node
        private static XmlNode CreatRecord(XmlDocument xml, XmlNode nodeTable, string KeyFields, string KeyValues, string UserID)
        {
            return CreatRecord(xml, nodeTable, KeyFields, KeyValues, UserID, LockType.Updating);
        }
        //creat Record node
        private static XmlNode CreatRecord(XmlDocument xml, XmlNode nodeTable, string KeyFields, string KeyValues, string UserID, LockType lt)
        {
            XmlElement nodeRecord = xml.CreateElement("Record");
            XmlAttribute attKeyFields = xml.CreateAttribute("KeyFields");
            attKeyFields.Value = KeyFields;
            nodeRecord.Attributes.Append(attKeyFields);
            XmlAttribute attKeyValues = xml.CreateAttribute("KeyValues");
            attKeyValues.Value = KeyValues;
            nodeRecord.Attributes.Append(attKeyValues);
            XmlAttribute attUserID = xml.CreateAttribute("UserID");
            attUserID.Value = UserID;
            nodeRecord.Attributes.Append(attUserID);
            XmlAttribute attStatus = xml.CreateAttribute("Status");
            switch (lt)
            {
                case LockType.Updating: attStatus.Value = "Updating"; break;
                case LockType.Deleting: attStatus.Value = "Deleting"; break;
                default: attStatus.Value = ""; break;
            }
            nodeRecord.Attributes.Append(attStatus);
            nodeTable.AppendChild(nodeRecord);
            xml.Save(RecordFileName);
            return nodeRecord;
        }


        //public static LockType AddRecordLock(string DBAlias, string TableName, string KeyFields, string KeyValues, ref string UserID)
        //{
        //    return AddRecordLock(DBAlias, TableName, KeyFields, KeyValues, ref UserID, LockType.Updating);
        //}

        public static LockType AddRecordLock(string DBAlias,string TableName, string KeyFields, string KeyValues, ref string UserID, LockType lt)
        {
            if (ServerConfig.RecordLockInDatabase)
            {
                return RecordLockInDB.AddRecordLock(DBAlias, TableName, KeyFields, KeyValues, ref UserID, lt);
            }
            else
            {
                //while (lockinuse)
                //{
                //    Thread.Sleep(100);
                //}
                LockType retval;
                //lockinuse = true;
                XmlDocument xml = new XmlDocument();
                xml.Load(RecordFileName);
                XmlNode nodeDBAlias = FindAlias(xml, DBAlias);
                if (nodeDBAlias == null)
                {
                    CreatRecord(xml, DBAlias, TableName, KeyFields, KeyValues, UserID, lt);
                    return LockType.Idle;
                }
                else
                {
                    XmlNode nodeTable = FindTable(nodeDBAlias, TableName);
                    if (nodeTable == null)
                    {
                        CreatRecord(xml, nodeDBAlias, TableName, KeyFields, KeyValues, UserID, lt);
                        retval = LockType.Idle;
                    }
                    else
                    {
                        XmlNode nodeRecord = FindRecord(nodeTable, KeyFields, KeyValues);
                        if (nodeRecord == null)
                        {
                            CreatRecord(xml, nodeTable, KeyFields, KeyValues, UserID, lt);
                            retval = LockType.Idle;
                        }
                        else
                        {
                            if (nodeRecord.Attributes["UserID"].Value == UserID)
                            {
                                switch (lt)
                                {
                                    case LockType.Updating: nodeRecord.Attributes["Status"].Value = "Updating"; break;
                                    case LockType.Deleting: nodeRecord.Attributes["Status"].Value = "Deleting"; break;
                                    default: nodeRecord.Attributes["Status"].Value = ""; break;
                                }
                                retval = LockType.Idle;
                                xml.Save(RecordFileName);
                            }
                            else
                            {
                                UserID = nodeRecord.Attributes["UserID"].Value;
                                switch (nodeRecord.Attributes["Status"].Value)
                                {
                                    case "Updating": retval = LockType.Updating; break;
                                    case "Deleting": retval = LockType.Deleting; break;
                                    default: retval = LockType.Other; break;
                                }
                            }
                        }

                    }
                }
                //lockinuse = false;
                return retval;
            }
        }

        public static void RemoveRecordLock(string DBAlias, string TableName, string KeyFields, ArrayList KeyValues, string UserID)
        {
            if (ServerConfig.RecordLockInDatabase)
            {
                RecordLockInDB.RemoveRecordLock(DBAlias, TableName, KeyFields, KeyValues, UserID);
            }
            else
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(RecordFileName);
                XmlNode nodeDBAlias = FindAlias(xml, DBAlias);
                if (nodeDBAlias == null)
                {
                    return;
                }
                else
                {
                    XmlNode nodeTable = FindTable(nodeDBAlias, TableName);
                    if (nodeTable == null)
                    {
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < KeyValues.Count; i++)
                        {
                            RemoveRecordLock(nodeTable, KeyFields, KeyValues[i].ToString(), UserID);
                        }
                        xml.Save(RecordFileName);
                    }

                }
            }
        }

        private static void RemoveRecordLock(XmlNode nodeTable, string KeyFields, string KeyValues, string UserID)
        {
            XmlNode nodeRecord = FindRecord(nodeTable, KeyFields, KeyValues);
            if (nodeRecord == null)
            {
                return;
            }
            else
            {
                if (nodeRecord.Attributes["UserID"].Value == UserID)
                {
                    nodeTable.RemoveChild(nodeRecord);
                }
                else
                {
                    return;
                }
            }
        
        
        }

        public static void CreateRecordFile()
        {
            //if (File.Exists(RecordFileName))
            //{
            //    File.Delete(RecordFileName);
            //}
            FileStream xmlstream = new FileStream(RecordFileName, FileMode.Create);
            XmlTextWriter w = new XmlTextWriter(xmlstream, new System.Text.UnicodeEncoding());
            w.Formatting = Formatting.Indented;
            w.WriteStartElement("InfoLight");
            w.WriteEndElement();
            w.Close();
            xmlstream.Close();
        }

        public static void ClearRecordFile()
        {
            if (ServerConfig.RecordLockInDatabase)
            {
                RecordLockInDB.ClearRecordFile();
            }
            else
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(RecordFileName);
                xml.DocumentElement.RemoveAll();
                xml.Save(RecordFileName);
            }
        }

        public static void ClearRecordFile(string UserID)
        {
            if (ServerConfig.RecordLockInDatabase)
            {
                RecordLockInDB.ClearRecordFile(UserID);
            }
            else
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(RecordFileName);
                XmlNodeList xnldb = xml.DocumentElement.SelectNodes("DataBase");
                foreach (XmlNode xndb in xnldb)
                {
                    XmlNodeList xnltable = xndb.SelectNodes("Table");
                    foreach (XmlNode xntable in xnltable)
                    {
                        XmlNodeList xnlrecord = xntable.SelectNodes("Record");
                        foreach (XmlNode xnrecord in xnlrecord)
                        {
                            if (xnrecord.Attributes["UserID"].Value == UserID)
                            {
                                xntable.RemoveChild(xnrecord);
                            }
                        }
                    }
                }
                xml.Save(RecordFileName);
            }
        }

        public static class RecordLockInDB
        {
            private static IDbConnection AllocateConnection()
            {
                var systemDatabase = DbConnectionSet.GetSystemDatabase(null);
                return DbConnectionSet.GetDbConn(systemDatabase).CreateConnection();
            }

            private static void CheckTable()
            {
                lock (typeof(RecordLockInDB))
                {
                    using (var connection = AllocateConnection())
                    {
                        connection.Open();
                        string checksql = string.Empty;
                        if (connection is System.Data.OracleClient.OracleConnection)
                        {
                            checksql = "select count(*) from USER_OBJECTS where OBJECT_TYPE = 'TABLE' and OBJECT_NAME='SYS_RECORDLOCK'";
                        }
                        else
                        {
                            checksql = "select count(*) from sysobjects where xtype in('u','U') and name='SYS_RECORDLOCK'";
                        }
                        
                        IDbCommand cmd = connection.CreateCommand();
                        cmd.CommandText = checksql;
                        object obj = cmd.ExecuteScalar();
                        if (obj != null && Convert.ToInt32(obj) > 0)
                        {

                        }
                        else
                        {
                            var createsql = string.Empty;
                            if (connection is System.Data.OracleClient.OracleConnection)
                            {
                                createsql = "CREATE TABLE SYS_RECORDLOCK ("
                               + "USERID varchar2 (20) NULL,"
                               + "DBALIAS nvarchar2 (50) NULL,"
                               + "TABLENAME nvarchar2 (100) NULL,"
                               + "KEYFIELDS nvarchar2 (max) NULL, "
                               + "KEYVALUES nvarchar2 (max) NULL,"
                               + "STATUS varchar2 (10) NULL"
                               + ")";
                            }
                            else
                            {
                                createsql = "CREATE TABLE SYS_RECORDLOCK ("
                                + "USERID varchar (20) NULL,"
                                + "DBALIAS nvarchar (50) NULL,"
                                + "TABLENAME nvarchar (100) NULL,"
                                + "KEYFIELDS nvarchar (max) NULL, "
                                + "KEYVALUES nvarchar (max) NULL,"
                                + "STATUS varchar (10) NULL"
                                + ")";
                            }


                            cmd.CommandText = createsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            private static string GetParameterName(string column, IDbCommand command)
            {
                if (command is System.Data.OracleClient.OracleCommand)
                {
                    return string.Format(":{0}", column);
                }
                else
                {
                    return string.Format("@{0}", column);
                }
            }

            public static object[] AddParameters(IEnumerable<string> keys, IEnumerable<object> values, IDbCommand command)
            {
                var parameters = new List<object>();
                var keyArray = keys.ToArray();
                var valueArray = values.ToArray();
                for (int i = 0; i < keyArray.Length && i < valueArray.Length; i++)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = keyArray[i];
                    parameter.Value = valueArray[i];
                    command.Parameters.Add(parameter);

                    if (command is System.Data.OracleClient.OracleCommand)
                    {
                        parameters.Add(string.Format(":{0}", keyArray[i]));
                    }
                    else
                    {
                        parameters.Add(string.Format("@{0}", keyArray[i]));
                    }

                }
                return parameters.ToArray();
            }

            public static LockType AddRecordLock(string DBAlias, string TableName, string KeyFields, string KeyValues, ref string UserID, LockType lt)
            {
                CheckTable();
                var result = LockType.Idle;
                using (var connection = AllocateConnection())
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    try
                    {
                        var parameters = AddParameters(new string[] { "DBALIAS", "TABLENAME", "KEYFIELDS", "KEYVALUES" }, new object[] { DBAlias, TableName, KeyFields, KeyValues }, command);
                        command.CommandText = string.Format("SELECT * FROM SYS_RECORDLOCK WHERE DBALIAS = {0} AND TABLENAME = {1} AND KEYFIELDS = {2} AND KEYVALUES = {3}", parameters);
                        var adpater = DBUtils.CreateDbDataAdapter(command);
                        var dataSet = new DataSet();
                        adpater.Fill(dataSet);
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            var lockUser = dataSet.Tables[0].Rows[0]["USERID"].ToString();
                            if (string.Compare(lockUser, UserID, true) != 0)
                            {
                                UserID = lockUser;
                                result = (LockType)Enum.Parse(typeof(LockType), dataSet.Tables[0].Rows[0]["STATUS"].ToString(), true);
                            }
                        }
                        else
                        {
                            command.Parameters.Clear();
                            parameters = AddParameters(new string[] { "USERID", "DBALIAS", "TABLENAME", "KEYFIELDS", "KEYVALUES", "STATUS" }, new object[] { UserID, DBAlias, TableName, KeyFields, KeyValues, lt.ToString() }, command);
                            command.CommandText = string.Format("INSERT INTO SYS_RECORDLOCK (USERID, DBALIAS, TABLENAME, KEYFIELDS, KEYVALUES, STATUS) VALUES ({0}, {1}, {2}, {3}, {4}, {5})", parameters);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                return result;

            }

            public static void RemoveRecordLock(string DBAlias, string TableName, string KeyFields, ArrayList KeyValues, string UserID)
            {
                CheckTable();
                using (var connection = AllocateConnection())
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    try
                    {
                        foreach (var keyValues in KeyValues)
                        {
                            command.Parameters.Clear();
                            var parameters = AddParameters(new string[] { "USERID", "DBALIAS", "TABLENAME", "KEYFIELDS", "KEYVALUES" }, new object[] { UserID, DBAlias, TableName, KeyFields, keyValues }, command);
                            command.CommandText = string.Format("DELETE FROM SYS_RECORDLOCK WHERE USERID = {0} AND DBALIAS = {1} AND TABLENAME = {2} AND KEYFIELDS = {3} AND KEYVALUES = {4}", parameters);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();

                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }

            public static void ClearRecordFile()
            {
                CheckTable();
                using (var connection = AllocateConnection())
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    try
                    {
                        command.CommandText = "DELETE FROM SYS_RECORDLOCK";
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            public static void ClearRecordFile(string UserID)
            {
                CheckTable();
                using (var connection = AllocateConnection())
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    try
                    {
                        var parameters = AddParameters(new string[] { "USERID" }, new object[] { UserID }, command);
                        command.CommandText = string.Format("DELETE FROM SYS_RECORDLOCK WHERE USERID = {0}", parameters);
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }


        }
    }
}
