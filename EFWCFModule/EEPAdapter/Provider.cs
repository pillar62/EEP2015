using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace EFWCFModule.EEPAdapter
{
    /// <summary>
    /// Provider of datamodule
    /// </summary>
    public class DataModuleProvider : IModuleProvider
    {
        #region IModuleProvider Members

        /// <summary>
        /// Gets or sets information of client
        /// </summary>
        public ClientInfo ClientInfo { get; set; }

        public string SqlSentence { get; set; }

        /// <summary>
        /// Gets modules of solution
        /// </summary>
        /// <returns>Modules of solution</returns>
        public List<string> GetModules()
        {
            var remoteModule = new EEPRemoteModule();
            var obj = remoteModule.GetSqlCommandList(ClientInfo.ToBaseArray());
            if (obj[0].Equals(0))
            {
                var RemoteModules = new List<RemoteModule>();
                var remoteNames = (string[])obj[1];
                foreach (var remoteName in remoteNames)
                {
                    RemoteModules.Add(new RemoteModule(remoteName));
                }
                return RemoteModules.Select(c => c.ModuleName).Distinct().ToList();
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        /// <summary>
        /// Gets list of command names
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>List of command names</returns>
        public List<string> GetCommandNames(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            var remoteModule = new EEPRemoteModule();
            var obj = remoteModule.GetSqlCommandList(ClientInfo.ToBaseArray());
            if (obj[0].Equals(0))
            {
                var RemoteModules = new List<RemoteModule>();
                var remoteNames = (string[])obj[1];
                foreach (var remoteName in remoteNames)
                {
                    RemoteModules.Add(new RemoteModule(remoteName));
                }
                return RemoteModules.Where(c => c.ModuleName.Equals(assemblyName)).Select(c => c.CommandName).Distinct().ToList();
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        private static string Serialize<T>(T value)
        {
            if (value == null)
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CheckCharacters = false; // \v 序列化时会报错
            settings.Encoding = new UnicodeEncoding(false, false);
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }

        private static DataSet Deserialize(string value)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            using (StringReader textReader = new StringReader(value))
            {

                using (XmlReader xmlReader = XmlReader.Create(textReader))
                {
                    DataSet dataset = new DataSet();
                    dataset.ReadXml(xmlReader, XmlReadMode.DiffGram);
                    return dataset;
                }
            }
        }

        /// <summary>
        /// Gets name of entity type
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <returns>Name of entity type</returns>
        public string GetObjectClassName(string assemblyName, string commandName, string entitySetName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }

            var processer = new EEPRemoteModule();

            var obj = processer.GetSqlCommandText(ClientInfo.ToArray(new PacketInfo()), assemblyName, commandName);
            if (obj[0].Equals(0))
            {
                obj = processer.GetTableName(ClientInfo.ToArray(new PacketInfo()), assemblyName, (string)obj[1], true);
                if (obj[0].Equals(0))
                {
                    return (string)obj[1];
                }
                else
                {
                    throw new Exception((string)obj[1]);
                }
            }
            else
            {
                throw new Exception((string)obj[1]);
            }

        }

        /// <summary>
        /// Gets count of data
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Count of data</returns>
        public int GetDataCount(string assemblyName, string commandName, PacketInfo packetInfo)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (packetInfo == null)
            {
                throw new ArgumentNullException("packetInfo");
            }

            var processer = new EEPRemoteModule();
            var sql = string.Empty;
            if (packetInfo.WhereParameters.Count > 0)
            {
                var objSql = processer.GetSqlCommandText(ClientInfo.ToArray(new PacketInfo()), assemblyName, commandName);
                if (objSql[0].Equals(0))
                {
                    sql = (string)objSql[1];
                }
            }
            string where = packetInfo.ToQueryString(ClientInfo, sql);
            var obj = processer.GetRecordsCount(ClientInfo.ToArray(packetInfo), assemblyName, commandName, where);
            if (obj[0].Equals(0))
            {
                return (int)obj[1];
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        /// <summary>
        /// Gets total
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <param name="totals">Information of total</param>
        /// <returns>Total of data</returns>
        public object GetDataTotal(string assemblyName, string commandName, PacketInfo packetInfo, Dictionary<string, string> totals)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (packetInfo == null)
            {
                throw new ArgumentNullException("packetInfo");
            }

            var processer = new EEPRemoteModule();
            var sql = string.Empty;
            if (packetInfo.WhereParameters.Count > 0)
            {
                var objSql = processer.GetSqlCommandText(ClientInfo.ToArray(new PacketInfo()), assemblyName, commandName);
                if (objSql[0].Equals(0))
                {
                    sql = (string)objSql[1];
                }
            }
            string where = packetInfo.ToQueryString(ClientInfo, sql);
            var obj = processer.GetRecordsTotal(ClientInfo.ToArray(packetInfo), assemblyName, commandName, where, totals);
            if (obj[0].Equals(0))
            {
                return Serialize<DataSet>((DataSet)obj[1]);
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        /// <summary>
        /// Gets fileds of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public List<string> GetFields(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }

            var processer = new EEPRemoteModule();
            if (ClientInfo.UserID == null)
            {
                ClientInfo.UserID = string.Empty;
            }
            var obj = processer.GetSqlCommand(ClientInfo.ToArray(new PacketInfo()), assemblyName, commandName, "1=0", false, null, string.Empty);
            if (obj[0].Equals(0))
            {
                var dataset = (DataSet)obj[1];
                var dataTable = string.IsNullOrEmpty(entityTypeName) ? dataset.Tables[0] : dataset.Tables[entityTypeName];
                return dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        /// <summary>
        /// Gets fileds of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public Dictionary<String, int> GetFieldsLength(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }

            var processer = new EEPRemoteModule();
            if (ClientInfo.UserID == null)
            {
                ClientInfo.UserID = string.Empty;
            }
            var obj = processer.GetSqlCommand(ClientInfo.ToArray(new PacketInfo()), assemblyName, commandName, "1=0", false, null, string.Empty);
            if (obj[0].Equals(0))
            {
                var dataset = (DataSet)obj[1];
                var dataTable = string.IsNullOrEmpty(entityTypeName) ? dataset.Tables[0] : dataset.Tables[entityTypeName];
                Dictionary<String, int> returnValue = new Dictionary<String, int>();
                List<DataColumn> columns = dataTable.Columns.Cast<DataColumn>().ToList();
                foreach (var item in columns)
                {
                    returnValue.Add(item.ColumnName, item.MaxLength);
                }
                return returnValue;
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        /// <summary>
        /// Gets navigation fields of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Navigation Fields of entity object</returns>
        public List<string> GetEntityNavigationFields(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var processer = new EEPRemoteModule();
            if (ClientInfo.UserID == null)
            {
                ClientInfo.UserID = string.Empty;
            }
            var obj = processer.GetSqlCommand(ClientInfo.ToArray(new PacketInfo()), assemblyName, commandName, "1=0", false, null, string.Empty);
            if (obj[0].Equals(0))
            {
                var dataset = (DataSet)obj[1];
                var dataTable = string.IsNullOrEmpty(entityTypeName) ? dataset.Tables[0] : dataset.Tables[entityTypeName];
                return dataset.Relations.OfType<DataRelation>().Where(c => c.ParentTable.Equals(dataTable)).Select(c => c.ChildTable.TableName).ToList();
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        /// <summary>
        /// Gets primary fields of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public List<string> GetPrimaryKeys(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }

            var processer = new EEPRemoteModule();
            if (ClientInfo.UserID == null)
            {
                ClientInfo.UserID = string.Empty;
            }
            var obj = processer.GetSqlCommand(ClientInfo.ToArray(new PacketInfo()), assemblyName, commandName, "1=0", false, null, string.Empty);
            if (obj[0].Equals(0))
            {
                var dataset = (DataSet)obj[1];
                var dataTable = string.IsNullOrEmpty(entityTypeName) ? dataset.Tables[0] : dataset.Tables[entityTypeName];
                return dataTable.PrimaryKey.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }


        /// <summary>
        /// Gets type of fileds of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Type of fields of entity object</returns>
        public Dictionary<string, string> GetEntityFieldTypes(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }

            var processer = new EEPRemoteModule();
            if (ClientInfo.UserID == null)
            {
                ClientInfo.UserID = string.Empty;
            }
            var obj = processer.GetSqlCommand(ClientInfo.ToArray(new PacketInfo()), assemblyName, commandName, "1=0", false, null, string.Empty);
            if (obj[0].Equals(0))
            {
                var dataset = (DataSet)obj[1];
                var dataTable = string.IsNullOrEmpty(entityTypeName) ? dataset.Tables[0] : dataset.Tables[entityTypeName];
                var dic = new Dictionary<string, string>();
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    dic.Add(dataColumn.ColumnName, dataColumn.DataType.Name);
                }
                return dic;
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        /// <summary>
        /// Gets serialized dataset from database
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Dataset</returns>
        public object GetSerializedDataSet(string assemblyName, string commandName, PacketInfo packetInfo)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (packetInfo == null)
            {
                throw new ArgumentNullException("packetInfo");
            }
            var dataset = GetDataSet(assemblyName, commandName, packetInfo);
            return Serialize<DataSet>(dataset);
        }

        /// <summary>
        /// Gets dataset from database
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Dataset</returns>
        public DataSet GetDataSet(string assemblyName, string commandName, PacketInfo packetInfo)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (packetInfo == null)
            {
                throw new ArgumentNullException("packetInfo");
            }

            var processer = new EEPRemoteModule();
            var sql = string.Empty;
            if (packetInfo.WhereParameters.Count > 0 || packetInfo.OrderParameters.Count > 0)
            {
                var objSql = processer.GetSqlCommandText(ClientInfo.ToArray(new PacketInfo()), assemblyName, commandName);
                if (objSql[0].Equals(0))
                {
                    sql = (string)objSql[1];
                }
            }
            string where = packetInfo.ToQueryString(ClientInfo, sql);
            string order = packetInfo.ToOrderString(ClientInfo, sql);
            var obj = processer.GetSqlCommand(ClientInfo.ToArray(packetInfo), assemblyName, commandName, where, false, null, order);
            if (obj[0].Equals(0))
            {
                var dataset = (DataSet)obj[1];
                return dataset;
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        /// <summary>
        /// Updates dataset to database
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="dataset">Dataset</param>
        /// <returns>Dataset</returns>
        public object UpdateDataSet(string assemblyName, string commandName, object dataset)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (dataset == null)
            {
                throw new ArgumentNullException("dataset");
            }

            var processer = new EEPRemoteModule();
            var obj = processer.UpdateDataSet(ClientInfo.ToArray(string.Empty), assemblyName, commandName, Deserialize((string)dataset));
            if (obj[0].Equals(0))
            {
                var returnDataset = (DataSet)obj[1];
                return Serialize<DataSet>(returnDataset);
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        public object ExecuteIOTMethod(string assemblyName, string commandName, object dataset)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (dataset == null)
            {
                throw new ArgumentNullException("dataset");
            }

            var processer = new EEPRemoteModule();
            var obj = processer.ExecuteIOTMethod(ClientInfo.ToArray(string.Empty), assemblyName, commandName, Deserialize((string)dataset));
            if (obj[0].Equals(0))
            {
                var returnDataset = (DataSet)obj[1];
                return Serialize<DataSet>(returnDataset);
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        /// <summary>
        /// Excute server method
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters of method</param>
        /// <returns>Result of excuting server method</returns>
        public object CallMethod(string assemblyName, string methodName, object[] parameters)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            var processor = new EEPRemoteModule();
            var obj = processor.CallMethod(ClientInfo.ToArray(), assemblyName, methodName, parameters);
            if (obj[0].Equals(0))
            {
                if (obj.Length > 1)
                {
#warning 标记借用
                    if (ClientInfo.UseDataSet && obj[1] is DataSet)
                    {
                        return Serialize<DataSet>((DataSet)obj[1]);
                    }

                    return obj[1];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (obj.Length > 1)
                {
                    throw new Exception((string)obj[1]);
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// Do record lock
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Lock</returns>
        public LockType DoRecordLock(string assemblyName, string commandName, PacketInfo packetInfo, LockType type, ref string user)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (packetInfo == null)
            {
                throw new ArgumentNullException("packetInfo");
            }

            var processer = new EEPRemoteModule();
            var obj = processer.GetSqlCommandText(ClientInfo.ToArray(new PacketInfo()), assemblyName, commandName);
            if (obj[0].Equals(0))
            {
                obj = processer.GetTableName(ClientInfo.ToArray(new PacketInfo()), assemblyName, (string)obj[1], true);
                if (obj[0].Equals(0))
                {
                    var tableName = ((string)obj[1]).Trim("[]".ToArray());
                    LockType returnType = LockType.Idle;
                    if (!File.Exists(RecordLock.RecordFileName))
                    {
                        RecordLock.CreateRecordFile();
                    }
                    var keyFields = string.Join(";", packetInfo.WhereParameters.Select(c => c.Field));
                    var keyValue = string.Join(";", packetInfo.WhereParameters.Select(c => c.Value.ToString()));
                    if (type == LockType.Idle)     //remove lock
                    {
                        var keyValues = new ArrayList();
                        keyValues.Add(keyValue);
                        RecordLock.RemoveRecordLock(ClientInfo.Database, tableName, keyFields, keyValues, ClientInfo.UserID);
                    }
                    else                           //add lock
                    {
                        user = ClientInfo.UserID;
                        returnType = RecordLock.AddRecordLock(ClientInfo.Database, tableName, keyFields, keyValue, ref user, type);
                    }
                    return returnType;
                }
                else
                {
                    throw new Exception((string)obj[1]);
                }
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        #endregion

        internal class RemoteModule
        {
            internal RemoteModule(string remoteName)
            {
                if (string.IsNullOrEmpty(remoteName))
                {
                    throw new ArgumentNullException("remoteName");
                }
                var index = remoteName.IndexOf('.');
                if (index <= 0 || index >= remoteName.Length - 1)
                {
                    throw new ArgumentException("remoteName is invalid.");
                }
                ModuleName = remoteName.Substring(0, index);
                CommandName = remoteName.Substring(index + 1);
            }

            public string ModuleName { get; set; }
            public string CommandName { get; set; }
        }
    }

    /// <summary>
    /// Provider of sd module
    /// </summary>
    public class SDModuleProvider : IModuleProvider
    {
        private static Dictionary<string, object> CreateParameters(string key, object value)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add(key, value);
            return parameters;
        }

        private static Dictionary<string, object> CreateParameters(IEnumerable<string> keys, IEnumerable<object> values)
        {
            var keyArray = keys.ToArray();
            var valueArray = values.ToArray();
            var parameters = new Dictionary<string, object>();
            for (int i = 0; i < keyArray.Length && i < valueArray.Length; i++)
            {
                parameters.Add(keyArray[i], valueArray[i]);
            }
            return parameters;
        }

        private static string CreateSelectSql(string table, IEnumerable<string> selectColumns, IEnumerable<string> whereColumns)
        {
            var sql = new StringBuilder();
            var selectPart = string.Empty;
            if (selectColumns != null)
            {
                selectPart = string.Join(",", selectColumns);
            }
            if (string.IsNullOrEmpty(selectPart))
            {
                selectPart = "*";
            }
            sql.AppendFormat("SELECT {1} FROM {0}", table, selectPart);
            sql.Append(CreateWherePart(whereColumns));
            return sql.ToString();
        }

        private static string CreateInsertSql(string table, IEnumerable<string> insertColumns)
        {
            return string.Format("INSERT INTO {0} ({1}) VALUES ({2})", table, string.Join(",", insertColumns), string.Join(",", insertColumns.Select(c => string.Format("@{0}", c))));
        }

        private static string CreateUpdateSql(string table, IEnumerable<string> updateColumns, IEnumerable<string> whereColumns)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("UPDATE {0} SET {1}", table, string.Join(",", updateColumns.Select(c => string.Format("{0}=@{0}", c))));
            sql.Append(CreateWherePart(whereColumns));
            return sql.ToString();
        }

        private static string CreateDeleteSql(string table, IEnumerable<string> whereColumns)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("DELETE FROM {0}", table);
            sql.Append(CreateWherePart(whereColumns));
            return sql.ToString();
        }

        private static string CreateWherePart(IEnumerable<string> whereColumns)
        {
            var sql = new StringBuilder();
            if (whereColumns != null)
            {
                var wherePart = string.Join(" AND ", whereColumns.Select(c => string.Format("{0}=@{0}", c)));
                if (!string.IsNullOrEmpty(wherePart))
                {
                    sql.AppendFormat(" WHERE {0}", wherePart);
                }
            }
            return sql.ToString();
        }


        #region IModuleProvider Members

        /// <summary>
        /// Gets or sets information of client
        /// </summary>
        public ClientInfo ClientInfo { get; set; }

        public string SqlSentence { get; set; }

        /// <summary>
        /// Gets modules of solution
        /// </summary>
        /// <returns>Modules of solution</returns>
        public List<string> GetModules()
        {
            var dataSet = DatabaseProvider.ExecuteDataSet(DatabaseProvider.SystemDatabase, null,
                CreateSelectSql("SYS_WEBPAGES", null, new string[] { "PageType", "UserID", "SolutionID" }),
                "SYS_WEBPAGES", new PacketInfo(), true,
                CreateParameters(new string[] { "PageType", "UserID", "SolutionID" }, new object[] { "S", ClientInfo.SDDeveloperID, ClientInfo.Solution }));
            return dataSet.Tables["SYS_WEBPAGES"].Rows.OfType<DataRow>().Select(c => c["PageName"].ToString()).ToList();
        }

        public string GetSolutions()
        {
            var sql = string.Empty;
            var sqlCommand = new SQLCommandInfo();
            if (string.IsNullOrEmpty(ClientInfo.Solution))
            {

                sqlCommand.CommandText = CreateSelectSql("SYS_SDSOLUTIONS", null, new string[] { "UserID" });
                sqlCommand.Parameters = CreateParameters("UserID", ClientInfo.SDDeveloperID);
            }
            else
            {
                sqlCommand.CommandText = CreateSelectSql("SYS_SDSOLUTIONS", null, new string[] { "UserID", "SolutionID" });
                sqlCommand.Parameters = CreateParameters(new string[] { "UserID", "SolutionID" }, new object[] { ClientInfo.SDDeveloperID, ClientInfo.Solution });
            }
            return (string)DatabaseProvider.ExecuteSQL(DatabaseProvider.SystemDatabase, null, sqlCommand, null, new PacketInfo());
        }

        const int LICENSE_COUNT = 1;

        public string RegisterMachine(string key)
        {
            var assemblyFile = Path.Combine(PackageProvider.CurrentDirectory, "EFWCFModule.EEPCloud.dll");
            var assembly = Assembly.LoadFrom(assemblyFile);
            var info = (string)assembly.GetType("EFWCFModule.EEPCloud.Provider").GetMethod("DecryptKey", BindingFlags.Static | BindingFlags.Public)
                .Invoke(null, new object[] { key });

            //var info = DecryptKey(key);
            var infos = info.Split(';');
            var sn = infos[0];
            var company = infos[1];
            var ns = infos[2];
            var mac = infos[3];


            if (!string.IsNullOrEmpty(ns))
            {
                //checksum
                var dataSet = DatabaseProvider.ExecuteDataSet(DatabaseProvider.SystemDatabase, null,
                       CreateSelectSql("SYS_SDModule_Register", null, new string[] { "EEP_SN" }),
                       null, new PacketInfo(), true,
                       CreateParameters("EEP_SN", sn));
                if (dataSet.Tables[0].Rows.Count >= LICENSE_COUNT)
                {
                    if (dataSet.Tables[0].Select(string.Format("MAC='{0}'", mac)).Length == 0)
                    {
                        throw new Exception(string.Format("EEPCloud License exceed  max count:{0}", LICENSE_COUNT));
                    }
                }
                else
                {
                    var sqlCommands = new List<SQLCommandInfo>();
                    sqlCommands.Add(new SQLCommandInfo()
                    {
                        CommandText = CreateInsertSql("SYS_SDModule_Register", new string[] { "EEP_SN", "COMPANY", "MAC", "REGISTER_DATE" }),
                        Parameters = CreateParameters(new string[] { "EEP_SN", "COMPANY", "MAC", "REGISTER_DATE" },
                        new object[] { sn, company, mac, DateTime.Now })
                    });
                    DatabaseProvider.ExecuteCommands(DatabaseProvider.SystemDatabase, null, sqlCommands);
                }
            }
            else
            {
                var sqlCommands = new List<SQLCommandInfo>();
                sqlCommands.Add(new SQLCommandInfo()
                {
                    CommandText = CreateDeleteSql("SYS_SDModule_Register", new string[] { "EEP_SN", "COMPANY", "MAC" }),
                    Parameters = CreateParameters(new string[] { "EEP_SN", "COMPANY", "MAC", },
                    new object[] { sn, company, mac })
                });
                DatabaseProvider.ExecuteCommands(DatabaseProvider.SystemDatabase, null, sqlCommands);
            }
            return info;
        }

        public bool CheckActiveUserCount(int maxUser)
        {
            if (maxUser > 0)
            {
                var countDataSet = DatabaseProvider.ExecuteDataSet(DatabaseProvider.SystemDatabase, null,
                 CreateSelectSql("SYS_SDUSERS", new string[] { "Count(*)" }, new string[] { "Active" }) + " AND ExpiryDate > @ExpiryDate",
                 null, new PacketInfo(), true, CreateParameters(new string[] { "Active", "ExpiryDate" }, new object[] { "Y", DateTime.Now }));
                var count = int.Parse(countDataSet.Tables[0].Rows[0][0].ToString());
                return count < maxUser;
            }
            return true;
        }

        public string RegisterUser(string userID, string email, string phone, string password, string description, string developer, string owner)
        {
            if (string.IsNullOrEmpty(developer))
            {
               

                var dataSet = DatabaseProvider.ExecuteDataSet(DatabaseProvider.SystemDatabase, null,
                   CreateSelectSql("SYS_SDUSERS", null, new string[] { "UserID" }),
                   null, new PacketInfo(), true,
                   CreateParameters("UserID", userID));
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("UserExist");
                }
                else
                {
                    dataSet = DatabaseProvider.ExecuteDataSet(DatabaseProvider.SystemDatabase, null,
                      CreateSelectSql("SYS_SDUSERS", null, new string[] { "Phone" }),
                      null, new PacketInfo(), true,
                      CreateParameters("Phone", phone));
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        throw new Exception("PhoneExist");
                    }
                    var now = DateTime.Now;
                    var expireDate = now.AddDays(60);
                    var encrpytPassword = EncryptPassword(userID, password);
                    var sqlCommands = new List<SQLCommandInfo>();
                    sqlCommands.Add(new SQLCommandInfo()
                    {
                        CommandText = CreateInsertSql("SYS_SDUSERS", new string[] { "UserID", "UserName", "Email", "Phone", "Active", "SYSTYPE", "DatabaseQty", "CreateDate", "Password", "ExpiryDate", "Description", "Owner" }),
                        Parameters = CreateParameters(new string[] { "UserID", "UserName", "Email", "Phone", "Active", "SYSTYPE", "DatabaseQty", "CreateDate", "Password", "ExpiryDate", "Description", "Owner" },
                        new object[] { userID, description, email, phone, "N", "N", 2, DateTime.Now, encrpytPassword, expireDate, description, owner })
                    });
                    sqlCommands.Add(new SQLCommandInfo()
                    {
                        CommandText = CreateInsertSql("SYS_SDMESSAGE", new string[] { "UserID", "Type", "Content", "ExpiryDate" }),
                        Parameters = CreateParameters(new string[] { "UserID", "Type", "Content", "ExpiryDate" },
                        new object[] { userID, 'P'
                            , @"歡迎你 EEPCloud, 線上使用手冊請參考功能表Help/OnLine Help 或者點擊此<a target=""_blank"" href=""http://www.infolight.com.tw/WebClient/chm/(6)EEP2012-EEPCloud/index.html"">鏈接</a>"
                            , now.AddDays(7) })
                    });
                    DatabaseProvider.ExecuteCommands(DatabaseProvider.SystemDatabase, null, sqlCommands);
                    var ret = string.Empty;
                    var dataSet2 = DatabaseProvider.ExecuteDataSet(DatabaseProvider.SystemDatabase, null,
                    CreateSelectSql("SYS_SDUSERS", new string[] { "AdminEmail", "Email" }, new string[] { "SysType" }),
                    null, new PacketInfo(), true,
                    CreateParameters("SysType", "S"));
                    if (dataSet2.Tables[0].Rows.Count > 0)
                    {
                        if (dataSet2.Tables[0].Rows[0]["AdminEmail"].ToString().ToLower() == "true" && dataSet2.Tables[0].Rows[0]["Email"].ToString() != "")
                        {
                            ret = dataSet2.Tables[0].Rows[0]["Email"].ToString();
                        }
                    }

                    return ret;
                }
            }
            else
            {
                var systemDatabase = DatabaseProvider.GetSystemDatabase(developer);
                var dataSet = DatabaseProvider.ExecuteDataSet(systemDatabase, developer,
                       CreateSelectSql("USERS", null, new string[] { "USERID" }),
                       null, new PacketInfo(), true,
                       CreateParameters("USERID", userID));
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("UserExist");
                }
                else
                {
                    var now = DateTime.Now;
                    var expireDate = now.AddDays(30);
                    var encrpytPassword = EncryptPassword(userID, password);
                    var sqlCommands = new List<SQLCommandInfo>();
                    sqlCommands.Add(new SQLCommandInfo()
                    {
                        CommandText = CreateInsertSql("USERS", new string[] { "USERID", "USERNAME", "EMAIL", "AUTOLOGIN", "CREATEDATE", "PWD", "EXPIRYDATE", "DESCRIPTION" }),
                        Parameters = CreateParameters(new string[] { "USERID", "USERNAME", "EMAIL", "AUTOLOGIN", "CREATEDATE", "PWD", "EXPIRYDATE", "DESCRIPTION" },
                        new object[] { userID, description, email, "X", DateTime.Now.ToString("yyyyMMdd"), encrpytPassword, expireDate, description })
                    });
                    DatabaseProvider.ExecuteCommands(systemDatabase, developer, sqlCommands);
                    var ret = string.Empty;
                    var dataSet2 = DatabaseProvider.ExecuteDataSet(DatabaseProvider.SystemDatabase, null,
                    CreateSelectSql("SYS_SDUSERS", new string[] { "Email" }, new string[] { "UserID" }),
                    null, new PacketInfo(), true,
                    CreateParameters("UserID", developer));
                    if (dataSet2.Tables[0].Rows.Count > 0)
                    {
                        if (dataSet2.Tables[0].Rows[0]["Email"].ToString() != "")
                        {
                            ret = dataSet2.Tables[0].Rows[0]["Email"].ToString();
                        }
                    }

                    return ret;
                }
            }
        }

        private string EncryptPassword(string user, string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                char[] p = new char[] { };
                EFWCFModule.EEPAdapter.Encrypt.EncryptPassword(user, password, 10, ref p, false);
                return new string(p);
            }
            else
            {
                return string.Empty;
            }
        }

        public string ResetUser(string userID, string email, string developer)
        {
            if (string.IsNullOrEmpty(developer))
            {
                var emailDataSet = DatabaseProvider.ExecuteDataSet(DatabaseProvider.SystemDatabase, null,
                  CreateSelectSql("SYS_SDUSERS", new string[] { "Email" }, new string[] { "UserID" }),
                  null, new PacketInfo(), true,
                  CreateParameters("UserID", userID));

                if (emailDataSet.Tables[0].Rows.Count > 0)
                {
                    if (emailDataSet.Tables[0].Rows[0]["Email"].ToString() == email)
                    {
                        var random = new Random(DateTime.Now.Millisecond);
                        var newPassword = random.Next(100000, 1000000).ToString();
                        var sqlCommands = new List<SQLCommandInfo>();
                        sqlCommands.Add(new SQLCommandInfo()
                        {
                            CommandText = CreateUpdateSql("SYS_SDUSERS", new string[] { "Password" }, new string[] { "UserID" }),
                            Parameters = CreateParameters(new string[] { "Password", "UserID" }, new object[] { EncryptPassword(userID, newPassword), userID })
                        });
                        DatabaseProvider.ExecuteCommands(DatabaseProvider.SystemDatabase, null, sqlCommands);
                        return newPassword;
                    }
                    else
                    {
                        throw new Exception("EmailError");
                    }
                }
                else
                {
                    throw new Exception("UserNotFound");
                }
            }
            else
            {
                var systemDatabase = DatabaseProvider.GetSystemDatabase(developer);
                var emailDataSet = DatabaseProvider.ExecuteDataSet(systemDatabase, developer,
                 CreateSelectSql("USERS", new string[] { "EMAIL" }, new string[] { "USERID" }),
                 null, new PacketInfo(), true,
                 CreateParameters("USERID", userID));

                if (emailDataSet.Tables[0].Rows.Count > 0)
                {
                    if (emailDataSet.Tables[0].Rows[0]["EMAIL"].ToString() == email)
                    {
                        var random = new Random(DateTime.Now.Millisecond);
                        var newPassword = random.Next(100000, 1000000).ToString();
                        var sqlCommands = new List<SQLCommandInfo>();
                        sqlCommands.Add(new SQLCommandInfo()
                        {
                            CommandText = CreateUpdateSql("USERS", new string[] { "PWD" }, new string[] { "USERID" }),
                            Parameters = CreateParameters(new string[] { "PWD", "USERID" }, new object[] { EncryptPassword(userID, newPassword), userID })
                        });
                        DatabaseProvider.ExecuteCommands(systemDatabase, developer, sqlCommands);
                        return newPassword;
                    }
                    else
                    {
                        throw new Exception("EmailError");
                    }
                }
                else
                {
                    throw new Exception("UserNotFound");
                }
            }
        }

        public string ActiveUser(string userID, string encrytPassword)
        {
            var dataSet = DatabaseProvider.ExecuteDataSet(DatabaseProvider.SystemDatabase, null,
                  CreateSelectSql("SYS_SDUSERS", null, new string[] { "UserID" }),
                  null, new PacketInfo(), true,
                  CreateParameters("UserID", userID));
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                if (dataSet.Tables[0].Rows[0]["Password"].ToString().Replace("&", "%26") == encrytPassword)
                {
                    var sqlCommands = new List<SQLCommandInfo>();
                    sqlCommands.Add(new SQLCommandInfo()
                    {
                        CommandText = CreateUpdateSql("SYS_SDUSERS", new string[] { "Active" }, new string[] { "UserID" }),
                        Parameters = CreateParameters(new string[] { "UserID", "Active" },
                        new object[] { userID, "Y" })
                    });
                    DatabaseProvider.ExecuteCommands(DatabaseProvider.SystemDatabase, null, sqlCommands);
                }
                else
                {
                    throw new Exception("KeyError");
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets list of command names
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>List of command names</returns>
        public List<string> GetCommandNames(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            var dataModuleInstance = CreateModuleInstance(assemblyName);
            return dataModuleInstance.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(c => typeof(IDbCommand).IsAssignableFrom(c.FieldType)).Select(c => c.Name).ToList();
        }

        public DataSet GetSysRefVals()
        {
            var database = ClientInfo.Database;
            if (DatabaseProvider.GetSplitSystemTable(database, ClientInfo.SDDeveloperID))
            {
                database = DatabaseProvider.GetSystemDatabase(ClientInfo.SDDeveloperID);
            }
            var dataset = DatabaseProvider.ExecuteDataSet(database, ClientInfo.SDDeveloperID, "Select * from SYS_REFVAL", "");
            return dataset;
        }

        private string Serialize<T>(T value)
        {
            if (value == null)
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false);
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }

        private DataSet Deserialize(string value)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            using (StringReader textReader = new StringReader(value))
            {

                using (XmlReader xmlReader = XmlReader.Create(textReader))
                {
                    DataSet dataset = new DataSet();
                    dataset.ReadXml(xmlReader, XmlReadMode.DiffGram);
                    return dataset;
                }
            }
        }

        /// <summary>
        /// Gets name of entity type
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <returns>Name of entity type</returns>
        public string GetObjectClassName(string assemblyName, string commandName, string entitySetName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }

            using (var dataModule = CreateModule(assemblyName))
            {
                dataModule.SetClientInfo(ClientInfo.ToArray(new PacketInfo()));
                dataModule.SetOwnerComponent();
                var sql = dataModule.GetSqlCommandText(commandName);
                return dataModule.GetTableName(sql, true);
            }
        }

        /// <summary>
        /// Gets count of data
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Count of data</returns>
        public int GetDataCount(string assemblyName, string commandName, PacketInfo packetInfo)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (packetInfo == null)
            {
                throw new ArgumentNullException("packetInfo");
            }

            using (var dataModule = CreateModule(assemblyName))
            {
                dataModule.SetClientInfo(ClientInfo.ToArray(packetInfo));
                dataModule.SetOwnerComponent();
                var sql = string.Empty;
                if (packetInfo.WhereParameters.Count > 0)
                {
                    sql = dataModule.GetSqlCommandText(commandName);
                }
                string where = packetInfo.ToQueryString(ClientInfo, sql);
                return dataModule.GetRecordsCount(commandName, packetInfo.ToQueryString(ClientInfo, sql));
            }
        }

        /// <summary>
        /// Gets fileds of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public List<string> GetFields(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (ClientInfo.UserID == null)
            {
                ClientInfo.UserID = string.Empty;
            }
            using (var dataModule = CreateModule(assemblyName))
            {
                dataModule.SetClientInfo(ClientInfo.ToArray(new PacketInfo()));
                dataModule.SetOwnerComponent();
                var obj = dataModule.GetSqlCommand(commandName, "1=0", false, null, string.Empty);
                var dataset = (DataSet)obj[0];
                var dataTable = string.IsNullOrEmpty(entityTypeName) ? dataset.Tables[0] : dataset.Tables[entityTypeName];
                return dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            }
        }

        /// <summary>
        /// Gets fileds of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public Dictionary<String, int> GetFieldsLength(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (ClientInfo.UserID == null)
            {
                ClientInfo.UserID = string.Empty;
            }
            using (var dataModule = CreateModule(assemblyName))
            {
                dataModule.SetClientInfo(ClientInfo.ToArray(new PacketInfo()));
                dataModule.SetOwnerComponent();
                var obj = dataModule.GetSqlCommand(commandName, "1=0", false, null, string.Empty);
                var dataset = (DataSet)obj[0];
                var dataTable = string.IsNullOrEmpty(entityTypeName) ? dataset.Tables[0] : dataset.Tables[entityTypeName];
                Dictionary<String, int> returnValue = new Dictionary<String, int>();
                List<DataColumn> columns = dataTable.Columns.Cast<DataColumn>().ToList();
                foreach (var item in columns)
                {
                    returnValue.Add(item.ColumnName, item.MaxLength);
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Gets navigation fields of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Navigation Fields of entity object</returns>
        public List<string> GetEntityNavigationFields(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (ClientInfo.UserID == null)
            {
                ClientInfo.UserID = string.Empty;
            }
            using (var dataModule = CreateModule(assemblyName))
            {
                dataModule.SetClientInfo(ClientInfo.ToArray(new PacketInfo()));
                dataModule.SetOwnerComponent();
                var obj = dataModule.GetSqlCommand(commandName, "1=0", false, null, string.Empty);
                var dataset = (DataSet)obj[0];
                var dataTable = string.IsNullOrEmpty(entityTypeName) ? dataset.Tables[0] : dataset.Tables[entityTypeName];
                return dataset.Relations.OfType<DataRelation>().Where(c => c.ParentTable.Equals(dataTable)).Select(c => c.ChildTable.TableName).ToList();
            }
        }

        /// <summary>
        /// Gets primary fields of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public List<string> GetPrimaryKeys(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (ClientInfo.UserID == null)
            {
                ClientInfo.UserID = string.Empty;
            }
            using (var dataModule = CreateModule(assemblyName))
            {
                dataModule.SetClientInfo(ClientInfo.ToArray(new PacketInfo()));
                dataModule.SetOwnerComponent();
                var obj = dataModule.GetSqlCommand(commandName, "1=0", false, null, string.Empty);
                var dataset = (DataSet)obj[0];
                var dataTable = string.IsNullOrEmpty(entityTypeName) ? dataset.Tables[0] : dataset.Tables[entityTypeName];
                return dataTable.PrimaryKey.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            }
        }

        /// <summary>
        /// Gets type of fileds of entity object
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Type of fields of entity object</returns>
        public Dictionary<string, string> GetEntityFieldTypes(string assemblyName, string commandName, string entityTypeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }

            if (ClientInfo.UserID == null)
            {
                ClientInfo.UserID = string.Empty;
            }

            using (var dataModule = CreateModule(assemblyName))
            {
                dataModule.SetClientInfo(ClientInfo.ToArray(new PacketInfo()));
                dataModule.SetOwnerComponent();
                var obj = dataModule.GetSqlCommand(commandName, "1=0", false, null, string.Empty);
                var dataset = (DataSet)obj[0];
                var dataTable = string.IsNullOrEmpty(entityTypeName) ? dataset.Tables[0] : dataset.Tables[entityTypeName];
                var dic = new Dictionary<string, string>();
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    dic.Add(dataColumn.ColumnName, dataColumn.DataType.Name);
                }
                return dic;
            }
        }

        /// <summary>
        /// Gets serialized dataset from database
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Dataset</returns>
        public object GetSerializedDataSet(string assemblyName, string commandName, PacketInfo packetInfo)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (packetInfo == null)
            {
                throw new ArgumentNullException("packetInfo");
            }
            var dataset = GetDataSet(assemblyName, commandName, packetInfo);
            return Serialize<DataSet>(dataset);
        }

        /// <summary>
        /// Gets dataset from database
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Dataset</returns>
        public DataSet GetDataSet(string assemblyName, string commandName, PacketInfo packetInfo)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (packetInfo == null)
            {
                throw new ArgumentNullException("packetInfo");
            }

            using (var dataModule = CreateModule(assemblyName))
            {
                dataModule.SetClientInfo(ClientInfo.ToArray(packetInfo));
                dataModule.SetOwnerComponent();
                var sql = string.Empty;
                if (packetInfo.WhereParameters.Count > 0 || packetInfo.OrderParameters.Count > 0)
                {
                    sql = dataModule.GetSqlCommandText(commandName);
                }
                string where = packetInfo.ToQueryString(ClientInfo, sql);
                string order = packetInfo.ToOrderString(ClientInfo, sql);
                var obj = dataModule.GetSqlCommand(commandName, where, false, null, order);
                var dataset = (DataSet)obj[0];
                SqlSentence = (string)obj[1];

                return dataset;
            }
        }

        public object GetDataTotal(string assemblyName, string commandName, PacketInfo packetInfo, Dictionary<string, string> totals)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (packetInfo == null)
            {
                throw new ArgumentNullException("packetInfo");
            }

            using (var dataModule = CreateModule(assemblyName))
            {
                dataModule.SetClientInfo(ClientInfo.ToArray(packetInfo));
                dataModule.SetOwnerComponent();
                var sql = string.Empty;
                if (packetInfo.WhereParameters.Count > 0 || packetInfo.OrderParameters.Count > 0)
                {
                    sql = dataModule.GetSqlCommandText(commandName);
                }
                string where = packetInfo.ToQueryString(ClientInfo, sql);
                return Serialize<DataSet>(dataModule.GetRecordsTotal(commandName, where, totals));
            }
        }

        /// <summary>
        /// Updates dataset to database
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="dataset">Dataset</param>
        /// <returns>Dataset</returns>
        public object UpdateDataSet(string assemblyName, string commandName, object dataset)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (dataset == null)
            {
                throw new ArgumentNullException("dataset");
            }

            using (var dataModule = CreateModule(assemblyName))
            {
                dataModule.SetClientInfo(ClientInfo.ToArray(string.Empty));
                dataModule.SetOwnerComponent();
                var obj = dataModule.UpdateDataSet(commandName, Deserialize((string)dataset));
                var returnDataset = (DataSet)obj[0];
                SqlSentence = (string)obj[1];
                return Serialize<DataSet>(returnDataset);
            }
        }

        public object ExecuteIOTMethod(string assemblyName, string commandName, object dataset)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (dataset == null)
            {
                throw new ArgumentNullException("dataset");
            }

            var dataModule = CreateModule(assemblyName);
            dataModule.SetClientInfo(ClientInfo.ToArray(string.Empty));
            dataModule.SetOwnerComponent();
            var obj = dataModule.ExecuteIOTMethod(commandName, Deserialize((string)dataset));
            var returnDataset = (DataSet)obj[0];
            SqlSentence = (string)obj[1];
            return Serialize<DataSet>(returnDataset);
        }

        /// <summary>
        /// Excute server method
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters of method</param>
        /// <returns>Result of excuting server method</returns>
        public object CallMethod(string assemblyName, string methodName, object[] parameters)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            var dataModuleInstance = CreateModuleInstance(assemblyName);
            using (var dataModule = new DataModule(dataModuleInstance))
            {
               
                dataModule.SetClientInfo(ClientInfo.ToArray());
                dataModule.SetOwnerComponent();
                var method = dataModuleInstance.GetType().GetMethod(methodName);
                if (method != null)
                {
                    var obj = (object[])method.Invoke(dataModuleInstance, new object[] { parameters });
                    if (obj[0].Equals(0))
                    {
                        if (obj.Length > 1)
                        {
                            return obj[1];
                        }
                        else
                        {
                            return null;
                        }
                    }
                    if (obj.Length > 1)
                    {
                        throw new Exception((string)obj[1]);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Do record lock
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Lock</returns>
        public EFWCFModule.EEPAdapter.LockType DoRecordLock(string assemblyName, string commandName, PacketInfo packetInfo, EFWCFModule.EEPAdapter.LockType type, ref string user)
        {
            throw new NotSupportedException();
        }

        #endregion


        private static Hashtable AssemblyTable = new Hashtable();

        private Assembly GetAssembly(string assemblyName)
        {
            var assemblyKey = string.Format("{0}_{1}_{2}", assemblyName, ClientInfo.SDDeveloperID, ClientInfo.Solution);
            lock (typeof(SDModuleProvider))
            {
                if (AssemblyTable.ContainsKey(assemblyKey))
                {
                    return (Assembly)AssemblyTable[assemblyKey];
                }
                else
                {
                    var dataSet = DatabaseProvider.ExecuteDataSet(DatabaseProvider.SystemDatabase, null,
                      CreateSelectSql("SYS_WEBPAGES", null, new string[] { "PageType", "PageName", "UserID", "SolutionID" }),
                      "SYS_WEBPAGES", new PacketInfo(), true,
                      CreateParameters(new string[] { "PageType", "PageName", "UserID", "SolutionID" }, new object[] { "S", assemblyName, ClientInfo.SDDeveloperID, ClientInfo.Solution }));
                    if (dataSet.Tables["SYS_WEBPAGES"].Rows.Count == 0)
                    {
                        throw new Exception(string.Format("Package: {0} not active", assemblyName));
                    }
                    var content = (byte[])dataSet.Tables["SYS_WEBPAGES"].Rows[0]["SERVERDLL"];
                    var assembly = Assembly.Load(content);
                    AssemblyTable.Add(assemblyKey, assembly);
                    return assembly;
                }

            }
        }

        private static void RemoveAssemblyCache(string assemblyName, ClientInfo clientInfo)
        {
            var assemblyKey = string.Format("{0}_{1}_{2}", assemblyName, clientInfo.UserID, clientInfo.Solution);
            lock (typeof(SDModuleProvider))
            {
                if (AssemblyTable.ContainsKey(assemblyKey))
                {
                    AssemblyTable.Remove(assemblyKey);
                }
            }
        }

        private object CreateModuleInstance(string assemblyName)
        {
            var assembly = GetAssembly(assemblyName);
            var componentType = assembly.GetType(string.Format("{0}.Component", assemblyName));
            return Activator.CreateInstance(componentType);
        }

        private DataModule CreateModule(string assemblyName)
        {
            return new DataModule(CreateModuleInstance(assemblyName));
        }

        readonly static string TemplatePath = string.Format(@"{0}\SLReference", Environment.CurrentDirectory);
        readonly static string TemplateServerPath = string.Format(@"{0}\SDTemplateServer", Environment.CurrentDirectory);
        readonly static string TemplateOutputPath = string.Format(@"{0}\SLReference\Output", Environment.CurrentDirectory);

        readonly static string TemplateClientPath = string.Format(@"{0}\SDTemplateClient", Environment.CurrentDirectory);

        public static Stream CreateEntityReferenceAssembly(IEnumerable<DataSet> datasets, string assemblyName, ClientInfo clientInfo)
        {
            //Copy project
            lock (typeof(SDModuleProvider))
            {
                var sourcePath = TemplatePath;
                var datetimeString = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var targetPath = Path.Combine(TemplatePath, "Build", datetimeString);
                CopyFiles(sourcePath, targetPath);
                CopyFiles(Path.Combine(sourcePath, "Properties"), Path.Combine(targetPath, "Properties"));

                //Create Code
                var referenceFile = Path.Combine(targetPath, "Reference.cs");
                CreateEntityReferenceCode(datasets, referenceFile, assemblyName);

                //Build project
                var projectFile = Path.Combine(targetPath, "SLReference.csproj");
                var newReplaceAssemblyName = ReplaceAssemblyName(projectFile, "SLReference", datetimeString);
                Build(projectFile);

                //Copy to website
                var assemblyFile = Path.Combine(targetPath, "Bin", "Debug", string.Format("{0}.dll", newReplaceAssemblyName));

                while (!File.Exists(assemblyFile))
                {
                    System.Threading.Thread.Sleep(500);
                }

                var outputPath = Path.Combine(TemplateOutputPath, clientInfo.UserID);

                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                var targetFile = Path.Combine(outputPath, string.Format("{0}.dll", assemblyName));

                System.IO.File.Copy(assemblyFile, targetFile, true);

                //Delete template project
                Directory.Delete(targetPath, true);

                return File.OpenRead(targetFile);
            }
        }

        private static void CopyFiles(string sourcePath, string targetPath)
        {
            if (System.IO.Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                var files = System.IO.Directory.GetFiles(sourcePath);

                foreach (string file in files)
                {
                    var fileName = System.IO.Path.GetFileName(file);
                    var destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(file, destFile, true);
                }
            }
        }

        private static string ReplaceAssemblyName(string projectFile, string projectName, string datetimeString)
        {
            var NewReplaceAssemblyName = string.Format("Temp{0}", datetimeString);
            string text = string.Empty;
            using (StreamReader reader = new StreamReader(projectFile))
            {
                text = reader.ReadToEnd();
            }
            text = text.Replace(string.Format("<AssemblyName>{0}</AssemblyName>", projectName)
                , string.Format("<AssemblyName>{0}</AssemblyName>", NewReplaceAssemblyName));
            using (StreamWriter writer = new StreamWriter(projectFile, false))
            {
                writer.Write(text);
                writer.Flush();
            }
            return NewReplaceAssemblyName;
        }

        private static void CreateEntityReferenceCode(IEnumerable<DataSet> datasets, string fileName, string assemblyName)
        {
            EntityCodeBuilder codeBuilder = new EntityCodeBuilder() { FileName = fileName, NameSpace = "SLTools", AssemblyName = assemblyName };
            codeBuilder.GenerateCode(datasets);
        }

        public static void Build(string projectFileName)
        {
            //msbuild
            var startInfo = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"),
                Arguments = string.Format("\"{0}\"", projectFileName),
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            System.Diagnostics.Process process = new System.Diagnostics.Process() { StartInfo = startInfo };
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            if (!result.Contains("Build succeeded") && !result.Contains("已成功生成") && !result.Contains("建置成功"))
            {
                //简化错误信息
                var index = result.IndexOf("Build FAILED.", StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                {
                    throw new Exception(result.Substring(index));
                }
                else
                {

                    throw new Exception(result);
                }
            }
        }

        public string BuildCordova(string directory, string type)
        {
            System.Diagnostics.Process pro = new System.Diagnostics.Process();
            var batPath = Path.Combine(directory, @"platforms\android\cordova\build.bat");
            FileInfo file = new FileInfo(batPath);
            pro.StartInfo.WorkingDirectory = file.Directory.FullName;
            pro.StartInfo.FileName = batPath;
            pro.StartInfo.CreateNoWindow = true;
            pro.Start();
            pro.WaitForExit();
            return string.Empty;
        }

        public static string CreateHtmlPage(string pageName, string aspxContent, string code, string reportContent, ClientInfo clientInfo, string websitePath)
        {
            lock (typeof(SDModuleProvider))
            {
                //var websitePath = @"E:\EEP2010\SDRunTimeWebClient\HtmlPages";
                var directory = Path.Combine(websitePath, clientInfo.UserID);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                var pagePath = string.Format(@"{0}.aspx", pageName);
                var codePath = string.Format(@"{0}.aspx.cs", pageName);
                var reportPath = string.Format(@"{0}.rdlc", pageName);

                var pageFileContent = File.ReadAllText(Path.Combine(websitePath, "Template.aspx"));
                using (StreamWriter writer = new StreamWriter(Path.Combine(directory, pagePath), false, Encoding.UTF8))
                {
                    writer.Write(pageFileContent.Replace("Template.aspx.cs", codePath)
                        .Replace("HtmlPages_Template", string.Format("HtmlPages_{0}_{1}", clientInfo.UserID, pageName))
                        .Replace("<!--User code-->", aspxContent));
                }

                var codeFileContent = File.ReadAllText(Path.Combine(websitePath, "Template.aspx.cs"));
                using (StreamWriter writer = new StreamWriter(Path.Combine(directory, codePath), false, Encoding.UTF8))
                {
                    writer.Write(codeFileContent
                        .Replace("HtmlPages_Template", string.Format("HtmlPages_{0}_{1}", clientInfo.UserID, pageName))
                        .Replace("/*User Code*/", code));
                }

                if (!string.IsNullOrEmpty(reportContent))
                {
                    using (StreamWriter writer = new StreamWriter(Path.Combine(directory, reportPath), false))
                    {
                        writer.Write(reportContent);
                    }
                }

                return Path.Combine("HtmlPages", clientInfo.UserID, pagePath);
            }
        }


        public static byte[] CreateServerDllAssembly(String sComponentCS, string assemblyName, string resx, ClientInfo clientInfo)
        {
            //Copy project
            var sourcePath = TemplateServerPath;
            var targetPath = Path.Combine(TemplateServerPath, "Build", DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + clientInfo.UserID);
            CopyFiles(sourcePath, targetPath);
            CopyFiles(Path.Combine(sourcePath, "Properties"), Path.Combine(targetPath, "Properties"));

            //Create Code
            var referenceFile = Path.Combine(targetPath, "Component.cs");
            CreateComponentCS(sComponentCS, referenceFile);

            //Create Resx
            if (!string.IsNullOrEmpty(resx))
            {
                var resxFile = Path.Combine(targetPath, "Component.resx");
                CreateComponentCS(resx, resxFile);
            }

            //Build project
            var projectFile = Path.Combine(targetPath, "SDTemplateServer.csproj");
            ReplaceServerAssemblyName(projectFile, assemblyName);
            Build(projectFile);

            //Change to bytes
            var assemblyFile = Path.Combine(targetPath, "Bin", "Debug", string.Format("{0}.dll", assemblyName));

            while (!File.Exists(assemblyFile))
            {
                System.Threading.Thread.Sleep(500);
            }

            byte[] bDll = null;
            using (FileStream fsDll = File.Open(assemblyFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                BinaryReader brDll = new BinaryReader(fsDll);
                bDll = brDll.ReadBytes((int)fsDll.Length);
            }
            
            //Delete template project
            Directory.Delete(targetPath, true);
            RemoveAssemblyCache(assemblyName, clientInfo);
            return bDll;
        }

        private static void ReplaceServerAssemblyName(string projectFile, String assemblyName)
        {
            string text = string.Empty;
            using (StreamReader reader = new StreamReader(projectFile))
            {
                text = reader.ReadToEnd();
            }
            text = text.Replace("SDTemplateServer", assemblyName);
            using (StreamWriter writer = new StreamWriter(projectFile, false))
            {
                writer.Write(text);
                writer.Flush();
            }
        }

        private static void CreateComponentCS(string sComponentCS, string referenceFile)
        {
            using (StreamWriter sw = new StreamWriter(referenceFile, false, new UTF8Encoding(true)))
            {
                sw.Write(sComponentCS);
            }
        }

        public static string GetServerContent(byte[] assemblyBytes, string content)
        {
            var obj = new JObject();
            obj["controls"] = new JArray();
            var assembly = Assembly.Load(assemblyBytes);
            var componentType = assembly.GetTypes().First(c => typeof(System.ComponentModel.Component).IsAssignableFrom(c));
            var component = Activator.CreateInstance(componentType);
            foreach (var field in componentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                if (typeof(System.ComponentModel.Component).IsAssignableFrom(field.FieldType))
                {
                    if (FilteredComponents.Contains(field.FieldType.FullName))
                    {
                        continue;
                    }
                    var control = field.GetValue(component);
                    if (control != null)
                    {
                        var objControl = new JObject();
                        objControl["type"] = field.FieldType.FullName;
                        var objProperties = GetItemProperties(control, component);

                        objProperties["ID"] = field.Name;
                        objControl["properties"] = objProperties;
                        (obj["controls"] as JArray).Add(objControl);
                    }
                }
            }

            //code
            var code = new StringBuilder();
            var medthods = componentType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var method in medthods)
            {
                if (FilteredMethods.Contains(method.Name))
                {
                    continue;
                }
                var match = Regex.Match(content, string.Format(@"(public|private|protect)\s+.+\s+{0}\s*\(.*\)", method.Name));
                if (match.Success)
                {
                    var startIndex = content.LastIndexOf("\n", match.Index);
                    startIndex += 1;

                    var bracket = 0;
                    var quot = 0;
                    for (int i = match.Index; i < content.Length; i++)
                    {
                        if (content[i] == '"')
                        {
                            quot++;
                        }
                        if (quot % 2 == 0)
                        {
                            if (content[i] == '{')
                            {
                                bracket++;
                            }
                            else if (content[i] == '}')
                            {
                                bracket--;
                                if (bracket == 0)
                                {
                                    code.AppendLine(content.Substring(startIndex, i + 1 - startIndex));
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            obj["code"] = code.ToString();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return json;
        }

        private static string[] FilteredMethods = new string[]{
            "Dispose",
            "InitializeComponent"
        };

        private static string[] FilteredComponents = new string[] { 
            "Srvtools.ServiceManager",
            "Srvtools.InfoConnection"
        };

        public static JObject GetItemProperties(object item, object component)
        {
            var objProperties = new JObject();
            foreach (var property in item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                if (CanEdit(property))
                {
                    var value = property.GetValue(item, null);
                    if (value != null)
                    {
                        if (property.PropertyType == typeof(int))
                        {
                            objProperties[property.Name] = new JValue((int)value);
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            objProperties[property.Name] = new JValue(value.ToString().ToLower());
                        }
                        else if (typeof(System.Collections.IList).IsAssignableFrom(property.PropertyType))
                        {
                            var objItems = new JArray();
                            foreach (var collectionItem in (IEnumerable)value)
                            {
                                objItems.Add(GetItemProperties(collectionItem, component));
                            }
                            objProperties[property.Name] = objItems;
                        }
                        else if (typeof(Component).IsAssignableFrom(property.PropertyType) || typeof(Component).IsAssignableFrom(value.GetType()))
                        {
                            if (!FilteredComponents.Contains(property.PropertyType.FullName))
                            {
                                foreach (var field in component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                                {
                                    if (typeof(System.ComponentModel.Component).IsAssignableFrom(field.FieldType))
                                    {
                                        var control = field.GetValue(component);
                                        if (control != null && control == value)
                                        {
                                            objProperties[property.Name] = field.Name;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            objProperties[property.Name] = new JValue(value.ToString());
                        }
                    }
                }
            }
            if (item is Component)
            {
                var EventHandlerList = item.GetType().GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(item, null) as System.ComponentModel.EventHandlerList;
                foreach (var eventInfo in item.GetType().GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                {
                    var eventKeyField = item.GetType().GetField(string.Format("Event{0}", eventInfo.Name), BindingFlags.Static | BindingFlags.NonPublic);
                    if (eventKeyField != null)
                    {
                        var eventKey = eventKeyField.GetValue(null);
                        if (eventKey != null)
                        {
                            var eventDelegate = EventHandlerList[eventKey];
                            if (eventDelegate != null && eventDelegate.Method != null)
                            {
                                objProperties[eventInfo.Name] = new JValue(eventDelegate.Method.Name);
                            }
                        }
                    }
                }

            }
            //(item.GetType().GetProperty("Events", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(item)  as System.ComponentModel.EventHandlerList)[item.GetType().GetField("EventBeforeApply", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null)].Method
            return objProperties;

        }

        private static bool CanEdit(PropertyInfo property)
        {
            if (!property.CanRead)
            {
                return false;
            }

            var browsableAttribute = (BrowsableAttribute)Attribute.GetCustomAttribute(property, typeof(BrowsableAttribute), false);
            if (browsableAttribute != null && !browsableAttribute.Browsable)
            {
                return false;
            }

            var designerSerializationVisibilityAttribute = (DesignerSerializationVisibilityAttribute)Attribute.GetCustomAttribute(property, typeof(DesignerSerializationVisibilityAttribute), false);
            if (designerSerializationVisibilityAttribute != null && designerSerializationVisibilityAttribute.Visibility == DesignerSerializationVisibility.Hidden)
            {
                return false;
            }

            if (!property.CanWrite && !typeof(System.Collections.IList).IsAssignableFrom(property.PropertyType))
            {
                return false;
            }

            return true;
        }

        public static byte[] CreateSilverlightClientAssembly(string xaml, string code)
        {
            //Copy project
            lock (typeof(SDModuleProvider))
            {
                var sourcePath = TemplateClientPath;
                var datetimeString = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var targetPath = Path.Combine(TemplateClientPath, "Build", datetimeString);
                CopyFiles(sourcePath, targetPath);
                CopyFiles(Path.Combine(sourcePath, "Properties"), Path.Combine(targetPath, "Properties"));
                CopyFiles(Path.Combine(sourcePath, "Image"), Path.Combine(targetPath, "Image"));
                CopyFiles(Path.Combine(sourcePath, "Image", "Refval"), Path.Combine(targetPath, "Image", "Refval"));
                CopyFiles(Path.Combine(sourcePath, "Image", "TabHead"), Path.Combine(targetPath, "Image", "TabHead"));
                CopyFiles(Path.Combine(sourcePath, "Image", "TabIcon"), Path.Combine(targetPath, "Image", "TabIcon"));

                //Create Code use region tag
                var xamlFile = Path.Combine(targetPath, "Page.xaml");
                var xamlFileContent = File.ReadAllText(xamlFile);
                using (StreamWriter writer = new StreamWriter(xamlFile, false))
                {
                    writer.Write(xamlFileContent.Replace("<!--User code-->", xaml));
                }
                var codeFile = Path.Combine(targetPath, "Page.xaml.cs");
                var codeFileContent = File.ReadAllText(codeFile);
                using (StreamWriter writer = new StreamWriter(codeFile, false))
                {
                    writer.Write(codeFileContent.Replace("/*User Code*/", code));
                }

                //Build project
                var projectFile = Path.Combine(targetPath, "SDTemplateClient.csproj");

                var newReplaceAssemblyName = ReplaceAssemblyName(projectFile, "SDTemplateClient", datetimeString);
                Build(projectFile);

                //Copy to website
                var assemblyFile = Path.Combine(targetPath, "Bin", "Debug", string.Format("{0}.dll", newReplaceAssemblyName));

                while (!File.Exists(assemblyFile))
                {
                    System.Threading.Thread.Sleep(500);
                }

                var bytes = File.ReadAllBytes(assemblyFile);
                Directory.Delete(targetPath, true);
                return bytes;
            }
        }
    }

    /// <summary>
    /// Provider of database
    /// </summary>
    public static class DatabaseProvider
    {
        /// <summary>
        /// Gets system database
        /// </summary>
        public static string SystemDatabase
        {
            get
            {
                return DbConnectionSet.SystemDatabase;
            }
        }

        /// <summary>
        /// Gets system database
        /// </summary>
        /// <param name="developerID">Developer ID</param>
        /// <returns>System database</returns>
        public static string GetSystemDatabase(string developerID)
        {
            return DbConnectionSet.GetSystemDatabase(developerID);
        }

        /// <summary>
        /// Gets list of databases
        /// </summary>
        /// <param name="developerID">Developer ID</param>
        /// <returns>List of databases</returns>
        public static List<string> GetDatabases(string deverloperID)
        {
            return DbConnectionSet.GetAvaliableAlias(deverloperID).ToList();
        }

        ///// <summary>
        ///// Gets list of databases
        ///// </summary>
        ///// <param name="developerID">Developer ID</param>
        ///// <returns>List of databases</returns>
        //public static List<string> GetDatabaseType(string dbName)
        //{
        //    return DbConnectionSet.GetDatabaseType(dbName);
        //}

        /// <summary>
        /// Gets provider  type of database
        /// </summary>
        /// <param name="database">Name of database</param>
        /// <param name="developerID">Developer ID</param>
        /// <returns>Provider type of database</returns>
        public static DataBaseType GetProviderType(string database, string deverloperID)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            var databaseConnection = DbConnectionSet.GetDbConn(database, deverloperID);
            if (databaseConnection == null)
            {
                throw new ObjectNotFoundException(string.Format("Database:{0} not found."));
            }
            return databaseConnection.DbType;
        }

        /// <summary>
        /// Gets provider of database
        /// </summary>
        /// <param name="database">Name of database</param>
        /// <param name="developerID">Developer ID</param>
        /// <returns>Provider of database</returns>
        public static string GetProvider(string database, string deverloperID)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            var databaseConnection = DbConnectionSet.GetDbConn(database, deverloperID);
            if (databaseConnection == null)
            {
                throw new ObjectNotFoundException(string.Format("Database:{0} not found."));
            }
            return GetProviderString(databaseConnection.DbType);
        }

        /// <summary>
        /// Gets connection string of database
        /// </summary>
        /// <param name="database">Name of database</param>
        /// <param name="developerID">Developer ID</param>
        /// <returns>Connection string of database</returns>
        public static string GetProviderConnectionString(string database, string deverloperID)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            var databaseConnection = DbConnectionSet.GetDbConn(database, deverloperID);
            if (databaseConnection == null)
            {
                throw new ObjectNotFoundException(string.Format("Database:{0} not found."));
            }
            return databaseConnection.ConnectionString;
        }

        /// <summary>
        /// Gets split stystem table of database
        /// </summary>
        /// <param name="database">Name of database</param>
        /// <param name="developerID">Developer ID</param>
        /// <returns>Split stystem table</returns>
        public static bool GetSplitSystemTable(string database, string deverloperID)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            var databaseConnection = DbConnectionSet.GetDbConn(database, deverloperID);
            if (databaseConnection == null)
            {
                throw new ObjectNotFoundException(string.Format("Database:{0} not found."));
            }
            return databaseConnection.SplitSystemTable;
        }


        private static string Serialize<T>(T value)
        {
            if (value == null)
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false);
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }

        private static DataSet Deserialize(string value)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            using (StringReader textReader = new StringReader(value))
            {

                using (XmlReader xmlReader = XmlReader.Create(textReader))
                {
                    DataSet dataset = new DataSet();
                    dataset.ReadXml(xmlReader, XmlReadMode.DiffGram);
                    return dataset;
                }
            }
        }

        /// <summary>
        /// Execute SQL method
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="commmandText">CommandText</param>
        /// <returns>DataSet</returns>
        public static object ExecuteSQL(string database, string commmandText)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (string.IsNullOrEmpty(commmandText))
            {
                throw new ArgumentNullException("commmandText");
            }
            return ExecuteSQL(database, commmandText, null);
        }

        /// <summary>
        /// Execute SQL method
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="commmandText">CommandText</param>
        /// <returns>DataSet</returns>
        public static object ExecuteSQL(string database, string commmandText, string tableName)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (string.IsNullOrEmpty(commmandText))
            {
                throw new ArgumentNullException("commmandText");
            }
            return ExecuteSQL(database, null, commmandText, tableName);
        }

        /// <summary>
        /// Execute SQL method
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="developerID">Developer ID</param>
        /// <param name="commmandText">CommandText</param>
        /// <param name="tableName">Name of datatable</param>
        /// <returns>DataSet</returns>
        public static object ExecuteSQL(string database, string developerID, string commmandText, string tableName)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (string.IsNullOrEmpty(commmandText))
            {
                throw new ArgumentNullException("commmandText");
            }
            return ExecuteSQL(database, developerID, commmandText, tableName, null);
        }

        /// <summary>
        /// Execute SQL method
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="developerID">Developer ID</param>
        /// <param name="commmandText">CommandText</param>
        /// <param name="tableName">Name of datatable</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>DataSet</returns>
        public static object ExecuteSQL(string database, string developerID, string commmandText, string tableName, PacketInfo packetInfo)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (string.IsNullOrEmpty(commmandText))
            {
                throw new ArgumentNullException("commmandText");
            }
            var dataset = ExecuteDataSet(database, developerID, commmandText, tableName, packetInfo);
            return Serialize<DataSet>(dataset);
        }


        public static object ExecuteSchema(string database, string developerID, string commmandText)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (string.IsNullOrEmpty(commmandText))
            {
                throw new ArgumentNullException("commmandText");
            }
            var connection = DbConnectionSet.WaitForConnection(database, developerID, string.Empty, "EFModule", DateTime.Now);
            if (connection == null)
            {
                throw new Exception("Database Connection Time Out!");
            }
            try
            {
                var dataset = new DataSet();
                var command = connection.CreateCommand();
                command.CommandText = commmandText;
                IDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);

                DataTable schemaTable = reader.GetSchemaTable();

                dataset.Tables.Add(schemaTable);
                return Serialize<DataSet>(dataset);
            }
            finally
            {
                DbConnectionSet.ReleaseConnection(database, developerID, connection);
            }

        }

        public static object ExecuteSQL(string database, string developerID, SQLCommandInfo sqlCommand, string tableName, PacketInfo packetInfo)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            var dataset = ExecuteDataSet(database, developerID, sqlCommand.CommandText, tableName, packetInfo, true, sqlCommand.Parameters);
            return Serialize<DataSet>(dataset);
        }

        /// <summary>
        /// Execute SQL method
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="commmandText">CommandText</param>
        /// <param name="tableName">Name of datatable</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(string database, string commmandText, string tableName)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (string.IsNullOrEmpty(commmandText))
            {
                throw new ArgumentNullException("commmandText");
            }
            return ExecuteDataSet(database, null, commmandText, tableName);
        }

        /// <summary>
        /// Execute SQL method
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="developerID">Developer ID</param>
        /// <param name="commmandText">CommandText</param>
        /// <param name="tableName">Name of datatable</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(string database, string developerID, string commmandText, string tableName)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (string.IsNullOrEmpty(commmandText))
            {
                throw new ArgumentNullException("commmandText");
            }
            return ExecuteDataSet(database, developerID, commmandText, tableName, null);
        }

        /// <summary>
        /// Execute SQL method
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="developerID">Developer ID</param>
        /// <param name="commmandText">CommandText</param>
        /// <param name="tableName">Name of datatable</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(string database, string developerID, string commmandText, string tableName, PacketInfo packetInfo)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (string.IsNullOrEmpty(commmandText))
            {
                throw new ArgumentNullException("commmandText");
            }
            return ExecuteDataSet(database, developerID, commmandText, tableName, packetInfo, false, null);
        }


        /// <summary>
        /// Execute SQL method
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="developerID">Developer ID</param>
        /// <param name="commmandText">CommandText</param>
        /// <param name="tableName">Name of datatable</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <param name="includeKey">Include Key</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(string database, string developerID, string commmandText, string tableName, PacketInfo packetInfo, bool includeKey, Dictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (string.IsNullOrEmpty(commmandText))
            {
                throw new ArgumentNullException("commmandText");
            }
            var connection = DbConnectionSet.WaitForConnection(database, developerID, string.Empty, "EFModule", DateTime.Now);
            if (connection == null)
            {
                throw new Exception("Database Connection Time Out!");
            }
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = commmandText;
                AddParameters(command, parameters);
                var dbDataAdpater = (System.Data.Common.DbDataAdapter)DBUtils.CreateDbDataAdapter(command);
                if (includeKey)
                {
                    dbDataAdpater.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                }
                var dataset = new DataSet();
                if (packetInfo != null && packetInfo.Count > 0)
                {
                    dbDataAdpater.Fill(dataset, packetInfo.StartIndex, packetInfo.Count, "Table");
                }
                else
                {
                    dbDataAdpater.Fill(dataset);
                }

                if (!string.IsNullOrEmpty(tableName))
                {
                    dataset.Tables[0].TableName = tableName;
                }
                return dataset;
            }
            finally
            {
                DbConnectionSet.ReleaseConnection(database, developerID, connection);
            }
        }

        private static void AddParameters(IDbCommand command, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = p.Key;
                    parameter.Value = p.Value;
                    command.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// Execute SQL method
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="commmandText">CommmandText</param>
        public static int ExecuteCommand(string database, string commmandText)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (string.IsNullOrEmpty(commmandText))
            {
                throw new ArgumentNullException("commmandText");
            }
            return ExecuteCommand(database, null, commmandText);
        }

        /// <summary>
        /// Execute SQL method
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="commmandText">CommmandText</param>
        public static int ExecuteCommand(string database, string developerID, string commmandText)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (string.IsNullOrEmpty(commmandText))
            {
                throw new ArgumentNullException("commmandText");
            }
            var connection = DbConnectionSet.WaitForConnection(database, developerID, string.Empty, "EFModule", DateTime.Now);
            if (connection == null)
            {
                throw new Exception("Database Connection Time Out!");
            }
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = commmandText;
                return command.ExecuteNonQuery();
            }
            finally
            {
                DbConnectionSet.ReleaseConnection(database, developerID, connection);
            }
        }

        public static int ExecuteCommands(string database, string developerID, List<SQLCommandInfo> sqlCommands)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            var connection = DbConnectionSet.WaitForConnection(database, developerID, string.Empty, "EFModule", DateTime.Now);
            if (connection == null)
            {
                throw new Exception("Database Connection Time Out!");
            }
            IDbTransaction transation = null;
            if (sqlCommands.Count > 1)
            {
                transation = connection.BeginTransaction();
            }
            try
            {
                var count = 0;
                foreach (var sqlCommand in sqlCommands)
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transation;
                    command.CommandText = sqlCommand.CommandText;
                    AddParameters(command, sqlCommand.Parameters);
                    count += command.ExecuteNonQuery();
                }
                if (transation != null)
                {
                    transation.Commit();
                }
                return count;
            }
            catch
            {
                if (transation != null)
                {
                    transation.Rollback();
                }
                throw;
            }
            finally
            {
                DbConnectionSet.ReleaseConnection(database, developerID, connection);
            }
        }

        public static string UpdateTable(string database, string tableName, string developerID, List<UpdateRow> updateRows)
        {
            var connection = DbConnectionSet.WaitForConnection(database, developerID, string.Empty, "EFModule", DateTime.Now);
            if (connection == null)
            {
                throw new Exception("Database Connection Time Out!");
            }
            var transaction = connection.BeginTransaction();
            try
            {

                var command = connection.CreateCommand();
                command.Transaction = transaction;
                var dbDataAdpater = (System.Data.Common.DbDataAdapter)DBUtils.CreateDbDataAdapter(command);
                dbDataAdpater.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                var dataTable = new DataTable();
                command.CommandText = string.Format("SELECT * FROM {0} WHERE 1=0", tableName);
                dbDataAdpater.Fill(dataTable);
                if (dataTable.PrimaryKey.Length == 0)
                {
                    throw new Exception(string.Format("Can not modify {0} without primary keys", tableName));
                }
                foreach (var updateRow in updateRows.Where(c => c.RowState == DataRowState.Deleted))
                {
                    CreateCommand(command, tableName, updateRow, dataTable);
                    //command.CommandText = string.Format("SELECT * FROM {0} WHERE {1}", tableName, GetWhereString(updateRow, dataTable));
                    dbDataAdpater.Fill(dataTable);
                    var row = dataTable.Rows[dataTable.Rows.Count - 1];
                    row.Delete();
                    CreateCommandBuilder(dbDataAdpater);
                    dbDataAdpater.Update(dataTable);
                }
                foreach (var updateRow in updateRows.Where(c => c.RowState == DataRowState.Modified))
                {
                    CreateCommand(command, tableName, updateRow, dataTable);
                    //command.CommandText = string.Format("SELECT * FROM {0} WHERE {1}", tableName, GetWhereString(updateRow, dataTable));
                    dbDataAdpater.Fill(dataTable);
                    var row = dataTable.Rows[dataTable.Rows.Count - 1];
                    foreach (var item in updateRow.NewValues)
                    {
                        if (dataTable.Columns.Contains(item.Key))
                        {
                            if (item.Value == null || (item.Value.ToString() == string.Empty && dataTable.Columns[item.Key].DataType != typeof(string)))
                            {
                                row[item.Key] = DBNull.Value;
                            }
                            else if (dataTable.Columns[item.Key].DataType == typeof(byte[]) && item.Value.GetType() != typeof(byte[]))
                            {
                                continue;
                            }
                            else if (!dataTable.Columns[item.Key].ReadOnly)
                            {
                                row[item.Key] = item.Value;
                            }
                        }
                    }
                    CreateCommandBuilder(dbDataAdpater);
                    dbDataAdpater.Update(dataTable);
                }
                foreach (var updateRow in updateRows.Where(c => c.RowState == DataRowState.Added))
                {
                    var row = dataTable.NewRow();
                    foreach (var item in updateRow.NewValues)
                    {
                        if (dataTable.Columns.Contains(item.Key))
                        {
                            if (item.Value == null || (item.Value.ToString() == string.Empty && dataTable.Columns[item.Key].DataType != typeof(string)))
                            {
                                row[item.Key] = DBNull.Value;
                            }
                            else if (dataTable.Columns[item.Key].DataType == typeof(byte[]) && item.Value.GetType() != typeof(byte[]))
                            {
                                continue;
                            }
                            else if (!dataTable.Columns[item.Key].ReadOnly)
                            {
                                row[item.Key] = item.Value;
                            }
                        }
                    }
                    dataTable.Rows.Add(row);
                    CreateCommandBuilder(dbDataAdpater);
                    dbDataAdpater.Update(dataTable);
                }
                transaction.Commit();
                return string.Empty;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                DbConnectionSet.ReleaseConnection(database, developerID, connection);
            }
        }

        private static void CreateCommandBuilder(System.Data.Common.DbDataAdapter adapter)
        {
            if (adapter.GetType().Name == "SqlDataAdapter")
            {
                System.Data.SqlClient.SqlCommandBuilder builder = new System.Data.SqlClient.SqlCommandBuilder((System.Data.SqlClient.SqlDataAdapter)adapter);
            }
            if (adapter.GetType().Name == "OracleDataAdapter")
            {
                System.Data.OracleClient.OracleCommandBuilder builder = new System.Data.OracleClient.OracleCommandBuilder((System.Data.OracleClient.OracleDataAdapter)adapter);
            }
            else if (adapter.GetType().Name == "MySqlDataAdapter")
            {
                Assembly assembly = Assembly.LoadFrom("MySql.Data.dll");
                var builder = assembly.CreateInstance("MySql.Data.MySqlClient.MySqlCommandBuilder", false, BindingFlags.CreateInstance, null, new object[] { adapter },null, null);
            }
        }

        private static void CreateCommand(IDbCommand command, string tableName, UpdateRow row, DataTable table)
        {
            var parameters = new Dictionary<string, object>();
            foreach (var primaryKey in table.PrimaryKey)
            {
                parameters.Add(primaryKey.ColumnName, row.OldValues[primaryKey.ColumnName]);
            }
            command.Parameters.Clear();
            AddParameters(command, parameters);
            var wherePart = string.Join(" AND ", table.PrimaryKey.Select(c => string.Format("{0}=@{0}", c.ColumnName)));
            command.CommandText = string.Format("SELECT * FROM {0} WHERE {1}", tableName, string.Join(" AND ", wherePart));
        }

        private static string GetWhereString(UpdateRow row, DataTable table)
        {
            var whereStrings = new List<string>();
            foreach (var key in table.PrimaryKey)
            {
                if (key.DataType == typeof(string))
                {
                    whereStrings.Add(string.Format("{0} = '{1}'", key.ColumnName, row.OldValues[key.ColumnName]));
                }
                else
                {
                    whereStrings.Add(string.Format("{0} = {1}", key.ColumnName, row.OldValues[key.ColumnName]));
                }
            }
            return string.Join(" AND ", whereStrings);
        }

        /// <summary>
        /// Gets provider string of database
        /// </summary>
        /// <param name="dataBaseType">Type of database</param>
        /// <returns>Provider string</returns>
        private static string GetProviderString(EFWCFModule.EEPAdapter.DataBaseType dataBaseType)
        {
            switch (dataBaseType)
            {
                case EEPAdapter.DataBaseType.MsSql: return typeof(System.Data.SqlClient.SqlConnection).Namespace;
#if Oracle
                case EEPAdapter.DataBaseType.Oracle: return typeof(DDTek.Oracle.OracleConnection).Namespace;
#elif Oracle2
                case EEPAdapter.DataBaseType.Oracle: return typeof(Oracle.DataAccess.Client.OracleConnection).Namespace;
#endif
                case DataBaseType.Informix: return "IBM.Data.Informix";
                default:
                    throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// Provider of log
    /// </summary>
    internal static class LogProvider
    {
        /// <summary>
        /// Logs system method begin
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="time">Time</param>
        /// <param name="methodName">Name of method</param>
        public static void SystemLogBegin(ClientInfo clientInfo, DateTime time, string methodName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            Log(clientInfo, LogStyle.System, LogStatus.Unknown, time, methodName, null);
        }

        /// <summary>
        /// Logs system method error
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="time">Time</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="exception">Exception</param>
        /// <returns>Title of log</returns>
        public static string SystemLogError(ClientInfo clientInfo, DateTime time, string methodName, Exception exception)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            var title = methodName;
            exception = GetRealException(exception);
            Log(clientInfo, LogStyle.System, LogStatus.Error, time, title, exception.Message);
            return string.Format("{0}:{1} {2}", LogStyle.System, title, exception.Message);
        }

        /// <summary>
        /// Logs system method start
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="time">Time</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="executionTime">Time span of execution</param>
        public static void SystemLogEnd(ClientInfo clientInfo, DateTime time, string methodName, TimeSpan executionTime)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            Log(clientInfo, LogStyle.System, LogStatus.Normal, time, methodName, null, executionTime, -1, string.Empty);
        }

        /// <summary>
        /// Logs provider method begin
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="time">Time</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="methodName">Name of method</param>
        public static void ProviderLogBegin(ClientInfo clientInfo, DateTime time, string assemblyName, string commandName, string methodName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }

            Log(clientInfo, LogStyle.Provider, LogStatus.Unknown, time, string.Format("{0}.{1}.{2}", assemblyName, commandName, methodName), null);
        }

        /// <summary>
        /// Logs provider method start
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="time">Time</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="exception">Exception</param>
        /// <returns>Title of log</returns>
        public static string ProviderLogError(ClientInfo clientInfo, DateTime time, string assemblyName, string commandName, string methodName
            , Exception exception)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            var title = string.Format("{0}.{1}.{2}", assemblyName, commandName, methodName);
            exception = GetRealException(exception);
            Log(clientInfo, LogStyle.Provider, LogStatus.Error, time, title, exception.Message);
            return string.Format("{0}:{1} {2}", LogStyle.Provider, title, exception.Message);
        }

        /// <summary>
        /// Logs provider method start
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="time">Time</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="executionTime">Time span of execution</param>
        public static void ProviderLogEnd(ClientInfo clientInfo, DateTime time, string assemblyName, string commandName, string methodName
            , TimeSpan executionTime, int sqlLogType, string sqlSentence)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            Log(clientInfo, LogStyle.Provider, LogStatus.Normal, time
                , string.Format("{0}.{1}.{2}", assemblyName, commandName, methodName), null, executionTime, sqlLogType, sqlSentence);
        }

        /// <summary>
        /// Logs provider method start
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="time">Time</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        public static void CallMethodLogBegin(ClientInfo clientInfo, DateTime time, string assemblyName, string methodName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            Log(clientInfo, LogStyle.CallMethod, LogStatus.Unknown, time, string.Format("{0}.{1}", assemblyName, methodName), null);
        }

        /// <summary>
        /// Logs provider method start
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="time">Time</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="exception">Exception</param>
        /// <returns>Title of log</returns>
        public static string CallMethodLogError(ClientInfo clientInfo, DateTime time, string assemblyName, string methodName, Exception exception)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            var title = string.Format("{0}.{1}", assemblyName, methodName);
            exception = GetRealException(exception);
            Log(clientInfo, LogStyle.CallMethod, LogStatus.Error, time, title, exception.Message);
            return string.Format("{0}:{1} {2}", LogStyle.CallMethod, title, exception.Message);
        }

        /// <summary>
        /// Logs provider method start
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="time">Time</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="executionTime">Time span of execution</param>
        public static void CallMethodLogEnd(ClientInfo clientInfo, DateTime time, string assemblyName, string methodName, TimeSpan executionTime)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            Log(clientInfo, LogStyle.CallMethod, LogStatus.Normal, time, string.Format("{0}.{1}", assemblyName, methodName), null, executionTime, -1, string.Empty);
        }

        /// <summary>
        /// Logs user define method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="status">Status</param>
        /// <param name="time">Time</param>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        public static void UserDefineLog(ClientInfo clientInfo, LogStatus status, DateTime time, string title, string description)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            Log(clientInfo, LogStyle.UserDefine, status, time, title, description);
        }

        /// <summary>
        /// Logs
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="style">Style of log</param>
        /// <param name="status">Status of log</param>
        /// <param name="time">Time</param>
        /// <param name="title">Title of log</param>
        /// <param name="description">description of log</param>
        private static void Log(ClientInfo clientInfo, LogStyle style, LogStatus status, DateTime time, string title, string description)
        {
            Log(clientInfo, style, status, time, title, description, TimeSpan.Zero, -1, string.Empty);
        }

        /// <summary>
        /// Logs
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="style">Style of log</param>
        /// <param name="status">Status of log</param>
        /// <param name="time">Time</param>
        /// <param name="title">Title of log</param>
        /// <param name="description">description of log</param>
        /// <param name="executionTime">Time span of execution</param>
        private static void Log(ClientInfo clientInfo, LogStyle style, LogStatus status, DateTime time, string title, string description
            , TimeSpan executionTime, int sqlLogType, string sqlSentence)
        {
            if (SysEEPLogService.Enable && (!clientInfo.UseDataSet || clientInfo.IsSDModule || style == LogStyle.System))
            {
                var log = new SysEEPLog(clientInfo.ToArray(), style, status, time, title, description);
                log.SqlLogType = sqlLogType;
                log.SqlSentence = sqlSentence;
                log.ExecutionTime = executionTime.TotalMilliseconds;
                log.Log();
            }
        }

        private static Exception GetRealException(Exception exception)
        {
            while (exception.InnerException != null && !string.IsNullOrEmpty(exception.InnerException.Message))
            {
                exception = exception.InnerException;
            }
            return exception;
        }

    }

    /// <summary>
    /// Provider of user
    /// </summary>
    public static class UserProvider
    {
        /// <summary>
        /// Logs on
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        internal static void LogOn(ClientInfo clientInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (clientInfo.IPAddress == null)
            {
                throw new ArgumentNullException("clientInfo.IPAddress");
            }
            SrvGL.LogUser(clientInfo.UserID.ToLower(), clientInfo.UserName, clientInfo.IPAddress.ToString(), 1);
        }

        /// <summary>
        /// Logs off
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        internal static void LogOff(ClientInfo clientInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (clientInfo.IPAddress == null)
            {
                throw new ArgumentNullException("clientInfo.IPAddress");
            }
            SrvGL.LogUser(clientInfo.UserID.ToLower(), clientInfo.UserName, clientInfo.IPAddress.ToString(), -1);
        }

        /// <summary>
        /// Checks whether user is logoned
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        internal static void CheckUserLogoned(ClientInfo clientInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            try
            {
                if (clientInfo.LogonResult != LogonResult.Logoned || !SrvGL.IsUserLogined(clientInfo.UserID.ToLower(), clientInfo.SDDeveloperID, clientInfo.SecurityKey))
                {
                    throw new InvalidOperationException(string.Format("User:{0} not logoned.", clientInfo.UserID));
                }
                else
                {
                    if (SrvGL.isUserLoginedInOtherPC(clientInfo.UserID.ToLower(), clientInfo.IPAddress))
                    {
                        throw new InvalidOperationException(string.Format("User:{0} is logoned in other place.", clientInfo.UserID));
                    }
                }
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        public static int LoginedCount
        {
            get
            {
                return EFWCFModule.EEPAdapter.SrvGL.UserLoginCount;
            }
        }
    }

    /// <summary>
    /// Provider of work flow
    /// </summary>
    internal static class FLowProvider
    {

        /// <summary>
        /// 512F4277-0D41-441c-BF16-D96B04580C2E
        /// </summary>
        const string REJECTED_CODE = "512F4277-0D41-441c-BF16-D96B04580C2E";
        /// <summary>
        /// 60585C77-60E1-4e6f-A2E2-3BBBAD6B4C9E
        /// </summary>
        const string END_CODE = "60585C77-60E1-4e6f-A2E2-3BBBAD6B4C9E";
        /// <summary>
        /// 906F766E-3736-403b-BB1D-132ADEC3F2E9
        /// </summary>
        const string WAITING_CODE = "906F766E-3736-403b-BB1D-132ADEC3F2E9";
        /// <summary>
        /// B4DAF3A4-AAE8-4b51-A391-B52E46305E9F;
        /// </summary>
        const string RETURN_END_CODE = "B4DAF3A4-AAE8-4b51-A391-B52E46305E9F";

        /// <summary>
        /// Calls flow method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow patameter</param>
        /// <returns>Result of flow method</returns>
        public static FlowResult CallFlowMethod(ClientInfo clientInfo, FlowParameter parameter)
        {
            switch (parameter.Operation)
            {
                case FlowOperation.Submit: return Submit(clientInfo, parameter);
                case FlowOperation.Approve: return Approve(clientInfo, parameter);
                case FlowOperation.Return: return Return(clientInfo, parameter);
                case FlowOperation.ReturnToStep: return ReturnToStep(clientInfo, parameter);
                case FlowOperation.Retake: return Retake(clientInfo, parameter);
                case FlowOperation.Reject: return Reject(clientInfo, parameter);
                case FlowOperation.PlusApprove: return PlusApprove(clientInfo, parameter);
                case FlowOperation.PlusReturn: return PlusReturn(clientInfo, parameter);
                case FlowOperation.PlusReturnToSender: return PlusReturn2(clientInfo, parameter);
                case FlowOperation.Pause: return Pause(clientInfo, parameter);
                case FlowOperation.Notify: return Notify(clientInfo, parameter);
                case FlowOperation.DeleteNotify: return DeleteNotify(clientInfo, parameter);
                case FlowOperation.GetFLPathList: return GetFLPathList(clientInfo, parameter);
                case FlowOperation.Preview: return Preview(clientInfo, parameter);
                case FlowOperation.ChangeSendTo: return ChangeSendTo(clientInfo, parameter);
                default: return null;
            }
        }

        /// <summary>
        /// Submit
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of submit</returns>
        private static FlowResult Submit(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.Submit(
                new object[]
                {
                    null,
                    new object[]{
                        parameter.XomlName,
                        string.Empty,
                        parameter.Important ? 1: 0,
                        parameter.Urgent ? 1: 0,
                        parameter.Remark,
                        parameter.RoleID,
                        parameter.Provider,
                        parameter.MailAddress,
                        parameter.OrgKind,
                        parameter.Attachments 
                    },
                    new object[]{
                        parameter.Keys,
                        parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                if (obj[1].Equals(null))
                {
                    return new FlowResult() { Status = FlowStatus.Waiting };
                }
                else if (obj[1].Equals(END_CODE))
                {
                    return new FlowResult() { Status = FlowStatus.End };
                }
                else if (obj[1].Equals(REJECTED_CODE))
                {
                    return new FlowResult() { Status = FlowStatus.Rejected };
                }
                else
                {
                    return new FlowResult() { Status = FlowStatus.Normal, SendToIDs = (string)obj[1], InstanceID = (Guid)obj[2], NextActivity = (string)obj[3] };
                }
            }
            else
            {
                return ThrowException(obj);
            }
        }

        /// <summary>
        /// Approve
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of approve</returns>
        private static FlowResult Approve(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.Approve(
                new object[]
                {
                    parameter.InstanceID,
                    new object[]{
                        parameter.CurrentActivity,
                        parameter.NextActivity,
                        parameter.Important ? 1: 0,
                        parameter.Urgent ? 1: 0,
                        parameter.Remark,
                        parameter.RoleID,
                        parameter.Provider,
                        parameter.MailAddress,
                        parameter.OrgKind,
                        parameter.Attachments 
                    },
                    new object[]{
                            parameter.Keys,
                            parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                if (obj[1].Equals(WAITING_CODE))
                {
                    return new FlowResult() { Status = FlowStatus.Waiting };
                }
                else if (obj[1].Equals(END_CODE))
                {
                    return new FlowResult() { Status = FlowStatus.End };
                }
                else if (obj[1].Equals(REJECTED_CODE))
                {
                    return new FlowResult() { Status = FlowStatus.Rejected };
                }
                else
                {
                    return new FlowResult() { Status = FlowStatus.Normal, SendToIDs = (string)obj[1] };
                }
            }
            else
            {
                return ThrowException(obj);
            }
        }

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="parameter"></param>
        /// <returns>Result of return</returns>
        private static FlowResult Return(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.Return(
                new object[]
                {
                    parameter.InstanceID,
                    new object[]{
                        parameter.CurrentActivity,
                        parameter.NextActivity,
                        parameter.Important ? 1: 0,
                        parameter.Urgent ? 1: 0,
                        parameter.Remark,
                        parameter.RoleID,
                        parameter.Provider,
                        parameter.MailAddress,
                        parameter.OrgKind,
                        parameter.Attachments 
                    },
                    new object[]{
                            parameter.Keys,
                            parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                if (obj[1].Equals(WAITING_CODE))
                {
                    return new FlowResult() { Status = FlowStatus.Waiting };
                }
                else if (obj[1].Equals(RETURN_END_CODE))
                {
                    return new FlowResult() { Status = FlowStatus.End };
                }
                else
                {
                    return new FlowResult() { Status = FlowStatus.Normal, SendToIDs = (string)obj[1] };
                }
            }
            else
            {
                return ThrowException(obj);
            }
        }



        /// <summary>
        /// Return
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="parameter"></param>
        /// <returns>Result of return</returns>
        private static FlowResult ReturnToStep(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.Return2(
                new object[]
                {
                    parameter.InstanceID,
                    new object[]{
                        parameter.CurrentActivity,
                        parameter.NextActivity,
                        parameter.Important ? 1: 0,
                        parameter.Urgent ? 1: 0,
                        parameter.Remark,
                        parameter.RoleID,
                        parameter.Provider,
                        parameter.MailAddress,
                        parameter.OrgKind,
                        string.Empty 
                    },
                    new object[]{
                            parameter.Keys,
                            parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                if (obj[1].Equals(WAITING_CODE))
                {
                    return new FlowResult() { Status = FlowStatus.Waiting };
                }
                else if (obj[1].Equals(RETURN_END_CODE))
                {
                    return new FlowResult() { Status = FlowStatus.End };
                }
                else
                {
                    return new FlowResult() { Status = FlowStatus.Normal, SendToIDs = (string)obj[1] };
                }
            }
            else
            {
                return ThrowException(obj);
            }
        }

        /// <summary>
        /// Retake
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of retake</returns>
        private static FlowResult Retake(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.Retake(
                new object[]
                {
                    parameter.InstanceID,
                    new object[]{
                        parameter.CurrentActivity,
                        string.Empty
                    },
                    new object[]{
                            parameter.Keys,
                            parameter.KeyValues
                    },
                    new object[]{
                            parameter.Important,
                            parameter.Urgent,
                            parameter.Attachments
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                return new FlowResult() { Status = FlowStatus.Normal };
            }
            else
            {
                return ThrowException(obj);
            }
        }

        /// <summary>
        /// Reject
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of reject</returns>
        private static FlowResult Reject(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.Reject(
                new object[]
                {
                    parameter.InstanceID,
                    new object[]{
                        parameter.CurrentActivity,
                        parameter.NextActivity,
                        parameter.NotifyAllRoles ? 1: 0,
                        parameter.Provider,
                        parameter.Remark
                    },
                    new object[]{
                            parameter.Keys,
                            parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                return new FlowResult() { Status = FlowStatus.Normal };
            }
            else
            {
                return ThrowException(obj);
            }
        }

        /// <summary>
        /// PlusApprove
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of plusapprove</returns>
        private static FlowResult PlusApprove(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.PlusApprove(
                new object[]
                {
                    parameter.InstanceID,
                    new object[]{
                        parameter.CurrentActivity,
                        parameter.NextActivity,
                        parameter.Important ? 1: 0,
                        parameter.Urgent? 1: 0,
                        parameter.Remark,
                        parameter.RoleID,
                        parameter.Provider,
                        parameter.MailAddress,
                        parameter.SendToIDs,
                        parameter.Attachments 
                    },
                    new object[]{
                            parameter.Keys,
                            parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                return new FlowResult() { Status = FlowStatus.Normal };
            }
            else
            {
                return ThrowException(obj);
            }
        }

        /// <summary>
        /// PlusReturn
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of plusreturn</returns>
        private static FlowResult PlusReturn(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.PlusReturn(
                new object[]
                {
                    parameter.InstanceID,
                    new object[]{
                        parameter.CurrentActivity,
                        parameter.NextActivity,
                        0,
                        0,
                        parameter.Remark,
                        parameter.RoleID,
                        parameter.Provider,
                        parameter.MailAddress,
                        string.Empty,
                        parameter.Attachments 
                    },
                    new object[]{
                            parameter.Keys,
                            parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                return new FlowResult() { Status = FlowStatus.Normal };
            }
            else
            {
                return ThrowException(obj);
            }
        }

        /// <summary>
        /// PlusReturn2
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of plusreturn</returns>
        private static FlowResult PlusReturn2(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.PlusReturn2(
                new object[]
                {
                    parameter.InstanceID,
                    new object[]{
                        parameter.CurrentActivity,
                        parameter.NextActivity,
                        0,
                        0,
                        parameter.Remark,
                        parameter.RoleID,
                        parameter.Provider,
                        parameter.MailAddress,
                        string.Empty,
                        string.Empty 
                    },
                    new object[]{
                            parameter.Keys,
                            parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                return new FlowResult() { Status = FlowStatus.Normal, SendToIDs = (string)obj[1] };
            }
            else
            {
                return ThrowException(obj);
            }
        }

        /// <summary>
        /// Pause
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of pause</returns>
        private static FlowResult Pause(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.Pause(
                new object[]
                {
                    null,
                    new object[]{
                        parameter.XomlName,
                        string.Empty,
                        0,
                        0,
                        string.Empty,
                        string.Empty,
                        parameter.Provider,
                        0,
                        string.Empty,
                        string.Empty
                    },
                    new object[]{
                            parameter.Keys,
                            parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                return new FlowResult() { Status = FlowStatus.Normal, InstanceID = (Guid)obj[2] };
            }
            else
            {
                return ThrowException(obj);
            }
        }

        /// <summary>
        /// Notify
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of notify</returns>
        private static FlowResult Notify(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.Notify(
                new object[]
                {
                    parameter.InstanceID,
                    new object[]{
                        parameter.CurrentActivity,
                        parameter.NextActivity,
                        0,
                        0,
                        parameter.Remark,
                        parameter.RoleID,
                        parameter.Provider,
                        0,
                        parameter.SendToIDs,
                        string.Empty
                    },
                    new object[]{
                            parameter.Keys,
                            parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                return new FlowResult() { Status = FlowStatus.Normal };
            }
            else
            {
                return ThrowException(obj);
            }
        }

        /// <summary>
        /// Delete Notify
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of notify</returns>
        private static FlowResult DeleteNotify(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.DeleteNotify(
                new object[]
                {
                    parameter.InstanceID,
                    parameter.CurrentActivity + ";" + parameter.NextActivity,
                    parameter.SendToIDs,
                    new object[]{
                            parameter.Keys,
                            parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                return new FlowResult() { Status = FlowStatus.Normal };
            }
            else
            {
                return ThrowException(obj);
            }
        }

        /// <summary>
        /// GetFLPathList
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of notify</returns>
        private static FlowResult GetFLPathList(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.GetFLPathList(
                new object[]
                {
                    parameter.InstanceID,
                    new object[]{
                        parameter.CurrentActivity,
                        parameter.NextActivity,
                        0,
                        0,
                        parameter.Remark,
                        parameter.RoleID,
                        parameter.Provider,
                        0,
                        parameter.SendToIDs,
                        string.Empty
                    },
                    new object[]{
                            parameter.Keys,
                            parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                return new FlowResult() { FLPathList = (String[])obj[1] };
            }
            else
            {
                return ThrowException(obj);
            }
        }

        /// <summary>
        /// Preview
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of submit</returns>
        private static FlowResult Preview(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.Preview(
                new object[]
                {
                    parameter.InstanceID,
                    new object[]{
                        parameter.XomlName,
                        string.Empty,
                        parameter.NextActivity,
                        new DataSet(),
                        parameter.RoleID,
                        null,
                        parameter.Provider,
                        0,
                        parameter.OrgKind,
                        parameter.Attachments 
                    },
                    new object[]{
                        parameter.Keys,
                        parameter.KeyValues
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                if (obj[1].Equals(null))
                {
                    return new FlowResult() { Status = FlowStatus.Waiting };
                }
                else if (obj[1].Equals(END_CODE))
                {
                    return new FlowResult() { Status = FlowStatus.End };
                }
                else if (obj[1].Equals(REJECTED_CODE))
                {
                    return new FlowResult() { Status = FlowStatus.Rejected };
                }
                else
                {
                    return new FlowResult() { Status = FlowStatus.Normal, FLOther = (byte[])obj[1] };
                }
            }
            else
            {
                return ThrowException(obj);
            }
        }

        private static FlowResult ChangeSendTo(ClientInfo clientInfo, FlowParameter parameter)
        {
            var instanceManager = new InstanceManager();
            var obj = instanceManager.ChangeSendTo(
                new object[]
                {
                    parameter.InstanceID,
                    new object[]{
                       parameter.CurrentActivity,
                       parameter.SendToIDs,
                       parameter.Remark
                    }
                },
                clientInfo.ToArray()
            );
            if (obj[0].Equals(0))
            {
                return new FlowResult();
            }
            else
            {
                return ThrowException(obj);
            }
        }

        private static FlowResult ThrowException(object[] obj)
        {
            if (obj[0].Equals(1))
            {
                throw new Exception((string)obj[1]);
            }
            else
            {
                return new FlowResult() { Status = FlowStatus.Exception, Message = (string)obj[1] };
            }
        }
    }

    /// <summary>
    /// Provider of server
    /// </summary>
    internal static class ServerProvider
    {
        const string EXCEEDSMAX_CODE = "000";
        const string LOCAL_CODE = "001";

        /// <summary>
        /// Gets server ip address
        /// </summary>
        /// <returns>Server ip address</returns>
        public static string GetServerIP()
        {
            var loginService = new LoginService();
            var obj = loginService.GetServerIP(true);
            var ip = obj[0];
            if (ip.Equals(LOCAL_CODE))
            {
                return string.Empty;
            }
            else if (!ip.Equals(EXCEEDSMAX_CODE))
            {
                var port = obj[1];
                return string.Format("{0}:8990", ip, port);
            }
            return string.Empty;
        }
    }

    public static class ClientInfoExtension
    {
        public static object[] ToArray(this ClientInfo clientInfo)
        {
            return ToArray(clientInfo, new PacketInfo() { StartIndex = 0, Count = 0 });
        }

        public static object[] ToArray(this ClientInfo clientInfo, PacketInfo packetInfo)
        {
            return new object[] 
            { 
                clientInfo.ToBaseArray(),
                packetInfo.Count,
                packetInfo.StartIndex -1,
                string.Empty
            };
        }

        public static object[] ToArray(this ClientInfo clientInfo, string replaceCommand)
        {
            return new object[] 
            { 
                clientInfo.ToBaseArray(),
                replaceCommand
            };
        }

        public static object[] ToBaseArray(this ClientInfo clientInfo)
        {
            return new object[] 
            { 
                (int)clientInfo.GetLanguage(),           //fClientLang
                clientInfo.UserID == null? string.Empty: clientInfo.UserID,                  //fLoginUser
                clientInfo.Database == null? string.Empty: clientInfo.Database,                //fLoginDB
                clientInfo.Site,                    //fSiteCode
                clientInfo.IPAddress,               //fComputerName
                clientInfo.IPAddress,               //fComputerIp
                clientInfo.Solution,                //fCurrentProject
                null,                               //fClientSystem
                clientInfo.Groups == null? string.Empty: string.Join(";",clientInfo.Groups.Where(c=>c.Type== GroupType.Normal || c.Type == GroupType.Role).Select(c=>c.ID).ToArray()),                               //fGroupID
                clientInfo.UserPara1,                               //UserPara1
                clientInfo.UserPara2,                               //UserPara2
                null,                               //ReplaceTableName
                clientInfo.Groups == null? string.Empty: string.Join(";",clientInfo.Groups.Where(c=>c.Type == GroupType.Role).Select(c=>c.ID).ToArray()),                                //Roles               
                clientInfo.Groups == null? string.Empty: string.Join(";",clientInfo.Groups.Where(c=>c.Type == GroupType.Role || c.Type == GroupType.Org).Select(c=>c.ID).ToArray()),                               //OrgRoles   
                clientInfo.Groups == null? string.Empty: string.Join(";",clientInfo.Groups.Where(c=>c.Type == GroupType.Role || c.Type== GroupType.OrgShare || c.Type == GroupType.Org).Select(c=>c.ID).ToArray()),                               //OrgShares  
                null,                               //OrgKind  
                clientInfo.Password,                //fLoginPassword  
                clientInfo.SDDeveloperID,
                clientInfo.AUTOLOGIN
            };
        }

        private static SYS_LANGUAGE GetLanguage(this ClientInfo clientInfo)
        {
            if (clientInfo.Locale == null)
            {
                return SYS_LANGUAGE.ENG;
            }
            var language = SYS_LANGUAGE.ENG;
            if (string.Compare(clientInfo.Locale, "zh-cn", true) == 0 || string.Compare(clientInfo.Locale, "zh-hans-cn", true) == 0)
            {
                language = SYS_LANGUAGE.SIM;
            }
            else if (string.Compare(clientInfo.Locale, "zh-tw", true) == 0 || string.Compare(clientInfo.Locale, "zh-hant-tw", true) == 0)
            {
                language = SYS_LANGUAGE.TRA;
            }
            else if (string.Compare(clientInfo.Locale, "zh-hk", true) == 0)
            {
                language = SYS_LANGUAGE.HKG;
            }
            return language;
        }
    }

    public static class PacketInfoExtension
    {
        public static string ToQueryString(this PacketInfo packetInfo, ClientInfo clientInfo, string sql)
        {
            if (packetInfo.OnlySchema)
            {
                return "1=0";
            }
            else if (!string.IsNullOrEmpty(packetInfo.WhereString))
            {
                return packetInfo.WhereString;
            }
            System.Collections.Hashtable tableopr = new System.Collections.Hashtable();
            string[] arroprkey = new string[] { "=", "!=", ">", ">=", "<", "<=", "%", "%%", "in" };
            for (int i = 0; i < arroprkey.Length; i++)
            {
                tableopr.Add((WhereCondition)i, arroprkey[i]);
            }
            var databaseType = DbConnectionSet.GetDbConn(clientInfo.Database, clientInfo.SDDeveloperID).DbType;
#warning 目前只考虑了2个数据库。另外，Nvarchar类型的N也没有加上，这个存在如何判断的问题。
            if (packetInfo != null)
            {
                if (packetInfo.WhereParameters.Count == 0)
                {
                }
                else
                {
                    StringBuilder sBuilder = new StringBuilder();
                    foreach (WhereParameter wp in packetInfo.WhereParameters)
                    {
                        string fieldname = wp.Field;
                        //增加表名
                        fieldname = DBUtils.GetTableNameForColumn(sql, fieldname, databaseType);
                        string cond = tableopr[wp.Condition].ToString();
                        string andor = wp.And ? "and" : "or";
                        string strvalue = string.Empty;
                        object value = wp.Value;
                        Type datatype = wp.Value.GetType();
                        string valuequote = (datatype == typeof(string) || datatype == typeof(char) || datatype == typeof(Guid))
                                            ? "'" : string.Empty;

                        if (value == null)
                        {
                            value = string.Empty;
                        }
                        try
                        {
                            if (value.ToString().Length == 0 || string.Compare(value.ToString(), "null", true) == 0)
                            {
                                strvalue = value.ToString();
                            }
                            else if (datatype == typeof(DateTime))
                            {
                                DateTime dt = (DateTime)Convert.ChangeType(value, typeof(DateTime));//所有时间类型分数据库
                                switch (databaseType)
                                {
                                    case DataBaseType.MsSql: strvalue = string.Format("'{0}-{1}-{2}'", dt.Year, dt.Month, dt.Day); break;
                                    case DataBaseType.Oracle: 
                                        String s = "";
                                        s = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " "
                                            + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();
                                        strvalue = string.Format("to_date('{0}', 'yyyy-mm-dd hh24:mi:ss')", s); break;
                                        //strvalue = string.Format("to_Date('{0:0000}{1:00}{2:00}', 'yyyymmdd')", dt.Year, dt.Month, dt.Day); break;
                                }
                            }
                            else
                            {
                                if (value.GetType() == typeof(bool))//checkbox redefination
                                {
                                    strvalue = bool.Equals(value, true) ? "1" : "0";
                                }
                                else if (datatype == typeof(Guid))
                                {
                                    try
                                    {
                                        Guid id = new Guid(value.ToString());
                                        strvalue = value.ToString();
                                    }
                                    catch (FormatException)
                                    {
                                        throw new InvalidCastException(string.Format("Can not convert '{0}' to {1} type", value, datatype.Name));
                                    }
                                }
                                else
                                {
                                    if (!cond.Equals("in"))
                                    {
                                        strvalue = value.ToString().Replace("'", "''");
                                    }
                                    else
                                    {
                                        string[] liststring = value.ToString().Split(',');
                                        StringBuilder inBuilder = new StringBuilder("(");
                                        foreach (string str in liststring)
                                        {
                                            Convert.ChangeType(str, datatype);
                                            if (inBuilder.Length > 1)
                                            {
                                                inBuilder.Append(",");
                                            }
                                            inBuilder.Append(string.Format("{0}{1}{0}", valuequote, str.Replace("'", "''")));
                                        }
                                        inBuilder.Append(")");
                                        strvalue = inBuilder.ToString();
                                        valuequote = string.Empty;//下面不加了
                                    }
                                }
                            }
                        }
                        catch (InvalidCastException)
                        {
                            throw new InvalidCastException(string.Format("Can not convert '{0}' to {1} type", value, datatype.Name));
                        }
                        catch (FormatException)
                        {
                            throw new InvalidCastException(string.Format("Can not convert '{0}' to {1} type", value, datatype.Name));
                        }
                        if (strvalue.Length > 0)
                        {
                            if (sBuilder.Length > 0)
                                sBuilder.Append(string.Format(" {0} ", andor));
                            if (string.Compare(strvalue.Trim(), "null", true) == 0) //null
                            {
                                if (valuequote.Length > 0)
                                {
                                    sBuilder.Append("(");
                                }
                                sBuilder.Append(string.Format("{0}", fieldname));
                                if (cond.Equals("!="))
                                {
                                    sBuilder.Append(" is not null");
                                }
                                else
                                {
                                    sBuilder.Append(" is null");
                                }
                                if (valuequote.Length > 0)
                                {
                                    if (cond.Equals("!="))
                                    {
                                        sBuilder.Append(string.Format(" and {0} <> '')", fieldname));
                                    }
                                    else
                                    {
                                        sBuilder.Append(string.Format(" or {0} = '')", fieldname));
                                    }
                                }
                            }
                            else
                            {
                                sBuilder.Append(string.Format("{0}", fieldname));
                                if (!cond.StartsWith("%"))
                                {
                                    sBuilder.Append(string.Format(" {0} {1}{2}{1}", cond, valuequote, strvalue));
                                }
                                else
                                {
                                    sBuilder.Append(string.Format(" like '{0}{1}%'", (cond == "%%") ? "%" : string.Empty, strvalue));
                                }
                            }
                        }
                    }
                    return sBuilder.ToString();
                }
            }
            return string.Empty;
        }

        public static string ToOrderString(this PacketInfo packetInfo, ClientInfo clientInfo, string sql)
        {
            StringBuilder sBuilder = new StringBuilder();
            var databaseType = DbConnectionSet.GetDbConn(clientInfo.Database, clientInfo.SDDeveloperID).DbType;
            foreach (var order in packetInfo.OrderParameters)
            {
                if (sBuilder.Length > 0)
                    sBuilder.Append(",");
                sBuilder.Append(DBUtils.GetTableNameForColumn(sql, order.Field, databaseType));
                if (order.Direction == OrderDirection.Descending)
                {
                    sBuilder.Append(" desc");
                }
            }
            return sBuilder.ToString();
        }

        public static string InsertQueryString(this PacketInfo packetInfo, ClientInfo clientInfo, string sql)
        {
            var where = packetInfo.ToQueryString(clientInfo, sql);

            return DBUtils.InsertWhere(sql, where);
        }
    }
}
