using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data.Objects.DataClasses;
using System.IO;
using System.Reflection;
using System.Data;
using Log = EFWCFModule.EEPAdapter.LogProvider;
using User = EFWCFModule.EEPAdapter.UserProvider;
using Database = EFWCFModule.EEPAdapter.DatabaseProvider;
using Flow = EFWCFModule.EEPAdapter.FLowProvider;
using Server = EFWCFModule.EEPAdapter.ServerProvider;
//using DataModule = EFWCFModule.EEPAdapter.DataModuleProvider;
//using SDModule = EFWCFModule.EEPAdapter.SDModuleProvider;
using EFWCFModule.EEPAdapter;
using System.Diagnostics;

namespace EFWCFModule
{
    /// <summary>
    /// Interface of EFService
    /// </summary>
    [ServiceContract(Namespace = "infolight.com")]
    [ServiceKnownType("GetKnownTypes", typeof(ServiceProvider))]
    public interface IEFService
    {
        [OperationContract]
        void CanOpen();

        [OperationContract]
        bool StartServer();

        /// <summary>
        /// Gets list of module names
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>List of module names</returns>
        [OperationContract]
        List<string> GetModuleNames(ClientInfo clientInfo);

        /// <summary>
        /// Gets list of command names
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>List of command names</returns>
        [OperationContract]
        List<string> GetCommandNames(ClientInfo clientInfo, string assemblyName);

        /// <summary>
        /// Gets container name of entity
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <returns>Container name of entity</returns>
        [OperationContract]
        string GetEntityContainerName(ClientInfo clientInfo, string assemblyName, string commandName);

        /// <summary>
        /// Gets name of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <returns>Name of entity type</returns>
        [OperationContract]
        string GetObjectClassName(ClientInfo clientInfo, string assemblyName, string commandName, string entitySetName);

        /// <summary>
        /// Gets primary keys of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Primary keys of entity object</returns>
        [OperationContract]
        List<string> GetEntityPrimaryKeys(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets fields of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        [OperationContract]
        List<string> GetEntityFields(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets fields of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        [OperationContract]
        Dictionary<String, int> GetEntityFieldsLength(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName);


        /// <summary>
        /// Gets type of fileds of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Type of fields of entity object</returns>
        [OperationContract]
        Dictionary<string, string> GetEntityFieldTypes(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets mapping of fileds of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Mapping of fields of entity object</returns>
        [OperationContract]
        Dictionary<string, string> GetEntityFieldMappings(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets navigation fields of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Navigation Fields of entity object</returns>
        [OperationContract]
        List<string> GetEntityNavigationFields(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets list of name of eneity sets which type is specialfied type
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Name of entity object</returns>
        [OperationContract]
        List<string> GetEntitySetNames(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets defination of columns
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Defination of columns</returns>
        [OperationContract]
        List<EntityObject> GetColumnDefination(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets defination of columns
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="param">paraments</param>
        /// <returns>Datas</returns>
        [OperationContract]
        List<EntityObject> GetDataByTableNameWhere(ClientInfo clientInfo, object[] param);

        /// <summary>
        /// Gets server ip address
        /// </summary>
        /// <returns>Server ip address</returns>
        [OperationContract]
        string GetServerIPAddress();

        /// <summary>
        /// Gets list of databases
        /// </summary>
        /// <param name="developerID">Developer ID</param>
        /// <returns>List of databases</returns>
        [OperationContract]
        List<string> GetDatabases(string developerID);

        /// <summary>
        /// Gets list of solutions
        /// </summary>
        /// <returns>List of solutions</returns>
        [OperationContract]
        List<SolutionInfo> GetSolutions(ClientInfo clientInfo);

        /// <summary>
        /// Logon
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>Information of client returned</returns>
        [OperationContract]
        ClientInfo LogOn(ClientInfo clientInfo);

        [OperationContract]
        ClientInfo LogOnDevice(string userID, string deviceID, bool check, string developerID);

        [OperationContract]
        void RegisterDevice(string userID, string deviceID, string regID, string tokenID, string developerID);

        [OperationContract]
        List<string> GetRegIDs(List<string> userIDs, string developerID);

        [OperationContract]
        List<string> GetTokenIDs(List<string> userIDs, string developerID);

        [OperationContract]
        void SendMessage(ClientInfo clientInfo, List<string> users, string subject, string body);
        [OperationContract]
        void DeleteMessage(ClientInfo clientInfo, List<string> dateTimes);
        [OperationContract]
        void ReadMessage(ClientInfo clientInfo, string dateTime);
        [OperationContract]
        object GetMessages(ClientInfo clientInfo);

        /// <summary>
        /// Logoff
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>Information of client returned</returns>
        [OperationContract]
        ClientInfo LogOff(ClientInfo clientInfo);


        /// <summary>
        /// Logs user define method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="status">Status</param>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        [OperationContract]
        void UserDefineLog(ClientInfo clientInfo, LogStatus status, string title, string description);


        /// <summary>
        /// Excute server method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters of method</param>
        /// <returns>Result of excuting server method</returns>
        [OperationContract]
        [ServiceKnownType(typeof(System.Collections.ArrayList))]
        object CallServerMethod(ClientInfo clientInfo, string assemblyName, string methodName, object[] paramters);

        /// <summary>
        /// Excute server method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters of method</param>
        /// <returns>Result of excuting server method</returns>
        [OperationContract]
        object CallMethod(ClientInfo clientInfo, string assemblyName, string methodName, object[] parameters);

        /// <summary>
        /// Gets count of entity objects
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Count of entity objects</returns>
        [OperationContract]
        int GetObjectCount(ClientInfo clientInfo, string assemblyName, string commandName, PacketInfo packetInfo);



        /// <summary>
        /// Gets a list of entity objects
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>List of entity objects</returns>
        [OperationContract]
        List<EntityObject> GetObjects(ClientInfo clientInfo, string assemblyName, string commandName, PacketInfo packetInfo);

        /// <summary>
        /// Gets entity object by key
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <param name="keyValues">Key and values</param>
        /// <returns>Entity object</returns>
        [OperationContract]
        object GetObjectByKey(ClientInfo clientInfo, string assemblyName, string commandName, string entitySetName, Dictionary<string, object> keyValues);

        /// <summary>
        /// Gets a list of detail entity objects
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="masterObject">Master entity object</param>
        /// <returns>Master entity object with detail entity objects</returns>
        [OperationContract]
        EntityObject GetDetailObjects(ClientInfo clientInfo, string assemblyName, string commandName, EntityObject masterObject);

        /// <summary>
        /// Update entity objects to database
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="objects">List of entity objects</param>
        /// <param name="states">State of entity objects</param>
        /// <returns>List of entity objects</returns>
        [OperationContract]
        List<EntityObject> UpdateObjects(ClientInfo clientInfo, string assemblyName, string commandName, List<EntityObject> objects, Dictionary<EntityKey, EntityState> states);

        /// <summary>
        /// Calls flow method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of flow method</returns>
        [OperationContract]
        FlowResult CallFlowMethod(ClientInfo clientInfo, FlowParameter parameter);

        /// <summary>
        /// Gets flow data
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="dataType">Type of flow data</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Flow data</returns>
        [OperationContract]
        List<EntityObject> GetFlowData(ClientInfo clientInfo, FlowDataType dataType, FlowDataParameter parameter);

        /// <summary>
        /// Gets flow data
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="dataType">Type of flow data</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Flow data</returns>
        [OperationContract]
        string GetFlowDataDS(ClientInfo clientInfo, FlowDataType dataType, FlowDataParameter parameter);

        /// <summary>
        /// Executes SQL method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="Database">Database</param>
        /// <param name="CommandText">CommandText</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>DataSet</returns>
        [OperationContract]
        object ExecuteSQL(ClientInfo clientInfo, string Database, string CommandText, PacketInfo packetInfo);

        /// <summary>
        /// Executes SQL method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="Database">Database</param>
        /// <param name="CommandText">CommmandText</param>
        [OperationContract]
        void ExecuteCommand(ClientInfo clientInfo, string Database, string CommandText);

        /// <summary>
        /// Gets count of data
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Count of data</returns>
        [OperationContract]
        int GetDataCount(ClientInfo clientInfo, string assemblyName, string commandName, PacketInfo packetInfo);

        /// <summary>
        /// Gets total
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <param name="totals">Information of total</param>
        /// <returns>Total of data</returns>
        [OperationContract]
        object GetDataTotal(ClientInfo clientInfo, string assemblyName, string commandName, PacketInfo packetInfo, Dictionary<string, string> totals);

        /// <summary>
        /// Gets dataset from database
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Dataset</returns>
        [OperationContract]
        object GetDataSet(ClientInfo clientInfo, string assemblyName, string commandName, PacketInfo packetInfo);

        /// <summary>
        /// Updates dataset to database
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="dataset">Dataset</param>
        [OperationContract]
        object UpdateDataSet(ClientInfo clientInfo, string assemblyName, string commandName, object dataset);

        [OperationContract]
        object ExecuteIOTMethod(ClientInfo clientInfo, string assemblyName, string commandName, object dataset);

        /// <summary>
        /// Do record lock
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <param name="type">Lock type</param>
        /// <returns>Lock</returns>
        [OperationContract]
        LockStatus DoRecordLock(ClientInfo clientInfo, string assemblyName, string commandName, PacketInfo packetInfo, EFWCFModule.EEPAdapter.LockType type);

        [OperationContract]
        void RemoveRecordLock(ClientInfo clientInfo, string userID);

        /// <summary>
        /// Gets menus
        /// </summary>
        /// <returns>Menus</returns>
        [OperationContract]
        List<EntityObject> FetchMenus(ClientInfo clientInfo);

        /// <summary>
        /// Get SYS_REFVALs
        /// </summary>
        /// <returns>SYS_REFVAL</returns>
        [OperationContract]
        List<EntityObject> GetAllDataByTableName(ClientInfo clientInfo, String tableName);

        /// <summary>
        /// Get SYS_REFVALs
        /// </summary>
        /// <returns>SYS_REFVAL</returns>
        [OperationContract]
        List<EntityObject> GetAllDataByTableNameDBAlias(ClientInfo clientInfo, String dbAlias, String tableName);

        /// <summary>
        /// SaveDataToTable
        /// </summary>
        /// <returns>void</returns>
        [OperationContract]
        void SaveDataToTable(ClientInfo clientInfo, object[] param, String tableName);

        /// <summary>
        /// AutoSeqMenuID
        /// </summary>
        /// <returns>int</returns>
        [OperationContract]
        int AutoSeqMenuID(ClientInfo clientInfo, object[] param);

        /// <summary>
        /// DeleteDataFromTable
        /// </summary>
        /// <returns>void</returns>
        [OperationContract]
        void DeleteDataFromTable(ClientInfo clientInfo, object param, String tableName);

        /// <summary>
        /// Gets server path
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>String</returns>
        [OperationContract]
        String GetServerPath(ClientInfo clientInfo);

        /// <summary>
        /// Creates sd entity reference assembly
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="commandText">Command Text</param>
        [OperationContract]
        Stream CreateSDEntityReferenceAssembly(ClientInfo clientInfo, string assemblyName, string commandName, string commandText);

        /// <summary>
        /// Create html page
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="pageName">Name of page</param>
        /// <param name="aspxContent">Content of aspx</param>
        /// <param name="code">Code</param>
        /// <param name="reportContent">Content of report</param>
        /// <param name="websitePath">Path of website</param>
        /// <returns>path of page</returns>
        [OperationContract]
        string CreateHtmlPage(ClientInfo clientInfo, string pageName, string aspxContent, string code, string reportContent, string websitePath);

        /// <summary>
        /// Get SDSolutions and webpages
        /// </summary>
        /// <returns>Sys_SDSolutions</returns>
        [OperationContract]
        List<EntityObject> GetAllDataByTableNameForSD(ClientInfo clientInfo, String tableName, String UserID);

        /// <summary>
        /// SaveDataToTable For SD System Table
        /// </summary>
        [OperationContract]
        void SaveDataToTableForSDSysTb(ClientInfo clientInfo, object[] param, String tableName);

        /// <summary>
        /// DeleteDataFromTable For SD System Table
        /// </summary>
        [OperationContract]
        void DeleteDataFromTableForSDSysTb(ClientInfo clientInfo, object param, String tableName);

        /// <summary>
        /// Execute SQL method for SD System Database
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="CommandText">CommmandText</param>
        [OperationContract]
        void ExecuteCommandForSDSyeDatabase(ClientInfo clientInfo, string commandText);

        /// <summary>
        /// log sd user
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        [OperationContract]
        void RefreshUserLogForSD(ClientInfo clientInfo);

        /// <summary>
        /// reset user password for jq and mobile
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="email"></param>
        /// <param name="developer"></param>
        /// <returns></returns>
        [OperationContract]
        string ResetUser(ClientInfo clientInfo,string email);

        #region SDModule Design time user . Menu.Security.users/menus/groups because of split system table ,so write new funtion
        /// <summary>
        /// Get Data from table which SD user's Security Table like user or groups For Design time
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="CommandText">CommmandText</param>
        [OperationContract]
        List<EntityObject> GetSecurityTableForSDDesign(ClientInfo clientInfo, String tableName);
        /// <summary>
        /// SaveDataToTable For SD user's Sceurity Table like user or groups For Design time
        /// </summary>
        [OperationContract]
        void SaveDataToTableForSDDesignSecurityTable(ClientInfo clientInfo, object[] param, String tableName);
        /// <summary>
        /// DeleteDataFromTable For  SD user's Sceurity Table like user or groups For Design time
        /// </summary>
        [OperationContract]
        void DeleteDataFromTableForSDDesignSecurityTable(ClientInfo clientInfo, object param, String tableName);
        #endregion


        #region sdmoudle new ver
        [OperationContract]
        string SDExcuteSql(ClientInfo clientInfo, SQLCommandInfo sqlCommand, PacketInfo packetInfo, SDTableType tableType);
        //[OperationContract]
        //int SDExcuteCommand(ClientInfo clientInfo, string commandText, SDTableType tableType, Dictionary<string, object> paramters);
        [OperationContract]
        int SDExcuteCommands(ClientInfo clientInfo, List<SQLCommandInfo> sqlCommands, SDTableType tableType);
        [OperationContract]
        string SDUpdateTable(ClientInfo clientInfo, string tableName, List<UpdateRow> updateRows, SDTableType tableType);
        [OperationContract]
        string SDGetSolutions(ClientInfo clientInfo);
        [OperationContract]
        string SDRegisterMachine(string key);
        [OperationContract]
        bool SDCheckActiveUserCount(int maxUser);
        [OperationContract]
        string SDBuildCordova(string directory, string type);
        [OperationContract]
        string SDRegisterUser(string userID, string email, string phone, string password, string description, string developer, string owner);
        [OperationContract]
        string SDResetUser(string userID, string email, string developer);
        [OperationContract]
        string SDActiveUser(string userID, string encryptPassword);
        #endregion

        [OperationContract]
        string GetConnectionInfo(ClientInfo clientInfo, SDTableType tableType);
    }

    /// <summary>
    /// Service of EF
    /// </summary>
    [ServiceBehavior(UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    public class EFService : IEFService
    {
        private IModuleProvider GetModuleProvider(ClientInfo clientInfo)
        {
            IModuleProvider provider = null;
            if (clientInfo.UseDataSet)
            {
                if (clientInfo.IsSDModule)
                {
                    provider = new EEPAdapter.SDModuleProvider();
                }
                else
                {
                    provider = new EEPAdapter.DataModuleProvider();
                }
            }
            else
            {
                provider = new EntityModuleProvider();
            }
            provider.ClientInfo = clientInfo;
            return provider;
        }


        #region IEFService Members

        public void CanOpen() { }

        public bool StartServer()
        {
            ListenerService service = new ListenerService();
            return service.StartServer();
        }

        /// <summary>
        /// Gets list of module names
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>List of module names</returns>
        public List<string> GetModuleNames(ClientInfo clientInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }

            return GetModuleProvider(clientInfo).GetModules();
        }

        /// <summary>
        /// Gets list of command names
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <returns>List of command names</returns>
        public List<string> GetCommandNames(ClientInfo clientInfo, string assemblyName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }

            return GetModuleProvider(clientInfo).GetCommandNames(assemblyName);
        }

        /// <summary>
        /// Gets container name of entity
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <returns>Container name of entity</returns>
        public string GetEntityContainerName(ClientInfo clientInfo, string assemblyName, string commandName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var module = PackageProvider.LoadModule(clientInfo, assemblyName);
            return module.GetEntityContainerName(commandName);
        }

        /// <summary>
        /// Gets name of entity type
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <returns>Name of entity type</returns>
        public string GetObjectClassName(ClientInfo clientInfo, string assemblyName, string commandName, string entitySetName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            return GetModuleProvider(clientInfo).GetObjectClassName(assemblyName, commandName, entitySetName);
        }

        /// <summary>
        /// Gets primary keys of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Primary keys of entity object</returns>
        public List<string> GetEntityPrimaryKeys(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            return GetModuleProvider(clientInfo).GetPrimaryKeys(assemblyName, commandName, entityTypeName);
        }

        /// <summary>
        /// Gets fileds of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public List<string> GetEntityFields(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }

            return GetModuleProvider(clientInfo).GetFields(assemblyName, commandName, entityTypeName);
        }

        /// <summary>
        /// Gets fileds of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        public Dictionary<String, int> GetEntityFieldsLength(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }

            return GetModuleProvider(clientInfo).GetFieldsLength(assemblyName, commandName, entityTypeName);
        }

        /// <summary>
        /// Gets type of fileds of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Type of fields of entity object</returns>
        public Dictionary<string, string> GetEntityFieldTypes(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            return GetModuleProvider(clientInfo).GetEntityFieldTypes(assemblyName, commandName, entityTypeName);
        }

        /// <summary>
        /// Gets mapping of fileds of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Mapping of fields of entity object</returns>
        public Dictionary<string, string> GetEntityFieldMappings(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var module = PackageProvider.LoadModule(clientInfo, assemblyName);
            return module.GetEntityFieldMappings(commandName, entityTypeName);
        }

        /// <summary>
        /// Gets navigation fields of entity object
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Navigation Fields of entity object</returns>

        public List<string> GetEntityNavigationFields(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            return GetModuleProvider(clientInfo).GetEntityNavigationFields(assemblyName, commandName, entityTypeName);
        }

        /// <summary>
        /// Gets list of name of eneity sets which type is specialfied type
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Name of entity object</returns>
        public List<string> GetEntitySetNames(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            var module = PackageProvider.LoadModule(clientInfo, assemblyName);
            return module.GetEntitySetNames(commandName, entityTypeName);
        }

        /// <summary>
        /// Gets defination of columns
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Defination of columns</returns>
        public List<EntityObject> GetColumnDefination(ClientInfo clientInfo, string assemblyName, string commandName, string entityTypeName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (string.IsNullOrEmpty(entityTypeName))
            {
                entityTypeName = GetObjectClassName(clientInfo, assemblyName, commandName, string.Empty);
            }
            if (clientInfo.UseDataSet && !clientInfo.IsSDModule)
            {
                var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
                return globalModule.GetColumnDefination(assemblyName, commandName, entityTypeName);
            }
            else
            {
                var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
                return globalModule.GetColumnDefination(entityTypeName);
            }
        }

        /// <summary>
        /// Gets defination of columns
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="entityTypeName">paraments</param>
        /// <returns>Datas</returns>
        public List<EntityObject> GetDataByTableNameWhere(ClientInfo clientInfo, object[] param)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            return globalModule.GetDataByTableNameWhere(param);
        }

        /// <summary>
        /// Gets list of databases
        /// </summary>
        /// <param name="developerID">Developer ID</param>
        /// <returns>List of databases</returns>
        public List<string> GetDatabases(string developerID)
        {
            return Database.GetDatabases(developerID);
        }

        /// <summary>
        /// Gets list of solutions
        /// </summary>
        /// <returns></returns>
        public List<SolutionInfo> GetSolutions(ClientInfo clientInfo)
        {
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            return globalModule.GetSolutions();
        }

        /// <summary>
        /// Gets server ip address
        /// </summary>
        /// <returns>Server ip address</returns>
        public string GetServerIPAddress()
        {
            return Server.GetServerIP();
        }



        /// <summary>
        /// Logs on
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>Information of client returned</returns>
        public ClientInfo LogOn(ClientInfo clientInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "LogOn");
            try
            {
#warning 标记借用
                if (clientInfo.IsSDModule)
                {
                    var returnInfo = globalModule.CheckUserForSDModule();
                    if (returnInfo.LogonResult == LogonResult.Logoned)
                    {
                        User.LogOn(clientInfo);
                    }
                    var timeEnd = DateTime.Now;
                    Log.SystemLogEnd(clientInfo, timeStart, "LogOn", timeEnd - timeStart);
                    return returnInfo;
                }
                else
                {
                    var returnInfo = globalModule.CheckUser();
                    if (returnInfo.LogonResult == LogonResult.Logoned)
                    {
                        User.LogOn(clientInfo);
                    }
                    var timeEnd = DateTime.Now;
                    Log.SystemLogEnd(clientInfo, timeStart, "LogOn", timeEnd - timeStart);
                    return returnInfo;
                }
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "LogOn", e);
                throw new Exception(message, e);
            }
        }

        public ClientInfo LogOnDevice(string userID, string deviceID, bool check, string developerID)
        {
            var globalModule = PackageProvider.LoadGlobalModule(null);
            return globalModule.LogOnDevice(userID, deviceID, check, developerID);
        }

        public void RegisterDevice(string userID, string deviceID, string regID, string tokenID, string developerID)
        {
            var globalModule = PackageProvider.LoadGlobalModule(null);
            globalModule.RegisterDevice(userID, deviceID, regID, tokenID, developerID);
        }

        public List<string> GetRegIDs(List<string> userIDs, string developerID)
        {
            var globalModule = PackageProvider.LoadGlobalModule(null);
            return globalModule.GetRegIDs(userIDs, developerID);
        }

        public List<string> GetTokenIDs(List<string> userIDs, string developerID)
        {
            var globalModule = PackageProvider.LoadGlobalModule(null);
            return globalModule.GetTokenIDs(userIDs, developerID);
        }


        public void SendMessage(ClientInfo clientInfo, List<string> users, string subject, string body)
        {
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            globalModule.SendMessage(users, subject,body);
        }
        public void DeleteMessage(ClientInfo clientInfo, List<string> dateTimes)
        {
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            globalModule.DeleteMessage(dateTimes);
        }
        public void ReadMessage(ClientInfo clientInfo, string dateTime)
        {
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            globalModule.ReadMessage(dateTime);
        }

        public object GetMessages(ClientInfo clientInfo)
        {
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            return globalModule.GetMessages();
        }

        /// <summary>
        /// Logs off
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        public ClientInfo LogOff(ClientInfo clientInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "LogOff");
            try
            {
                User.LogOff(clientInfo);

                clientInfo.LogonResult = LogonResult.NotLogoned;
                if (!SrvGL.IsUserLogined(clientInfo.UserID.ToLower(), clientInfo.SDDeveloperID, clientInfo.SecurityKey))
                {
                    RecordLock.ClearRecordFile(clientInfo.UserID.ToLower());
                }
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "LogOff", timeEnd - timeStart);
                return clientInfo;
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "LogOff", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Logs user define method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="status">Status</param>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        public void UserDefineLog(ClientInfo clientInfo, LogStatus status, string title, string description)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            Log.UserDefineLog(clientInfo, status, DateTime.Now, title, description);
        }

        /// <summary>
        /// Excute server method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters of method</param>
        /// <returns>Result of excuting server method</returns>
        public object CallMethod(ClientInfo clientInfo, string assemblyName, string methodName, object[] parameters)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            if (clientInfo.LogonResult != LogonResult.Logoned)
            {
                PackageProvider.CheckMethodLogOnRequired(clientInfo.Solution, assemblyName, methodName);
            }
            else
            {
                User.CheckUserLogoned(clientInfo);
            }
            var provider = GetModuleProvider(clientInfo);
            var timeStart = DateTime.Now;
            Log.CallMethodLogBegin(clientInfo, timeStart, assemblyName, methodName);
            try
            {
                var returnObject = provider.CallMethod(assemblyName, methodName, parameters);
                var timeEnd = DateTime.Now;
                Log.CallMethodLogEnd(clientInfo, timeStart, assemblyName, methodName, timeEnd - timeStart);
                return returnObject;
            }
            catch (TargetInvocationException e)
            {
                var exception = e.InnerException;
                var message = Log.CallMethodLogError(clientInfo, timeStart, assemblyName, methodName, exception);
                throw new Exception(message, exception);
            }
            catch (Exception e)
            {
                var message = Log.CallMethodLogError(clientInfo, timeStart, assemblyName, methodName, e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Excute server method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters of method</param>
        /// <returns>Result of excuting server method</returns>
        public object CallServerMethod(ClientInfo clientInfo, string assemblyName, string methodName, object[] parameters)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            if (clientInfo.LogonResult != LogonResult.Logoned)
            {
                PackageProvider.CheckMethodLogOnRequired(clientInfo.Solution, assemblyName, methodName);
            }
            else
            {
                User.CheckUserLogoned(clientInfo);
            }
            IModuleProvider provider = new EEPAdapter.DataModuleProvider();
            //if (clientInfo.IsSDModule)
            //{
            //    provider = new EEPAdapter.SDModuleProvider();
            //}
            provider.ClientInfo = clientInfo;
            var timeStart = DateTime.Now;
            Log.CallMethodLogBegin(clientInfo, timeStart, assemblyName, methodName);
            try
            {
                var returnObject = provider.CallMethod(assemblyName, methodName, parameters);
                var timeEnd = DateTime.Now;
                Log.CallMethodLogEnd(clientInfo, timeStart, assemblyName, methodName, timeEnd - timeStart);
                return returnObject;
            }
            catch (TargetInvocationException e)
            {
                var exception = e.InnerException;
                var message = Log.CallMethodLogError(clientInfo, timeStart, assemblyName, methodName, exception);
                throw new Exception(message, exception);
            }
            catch (Exception e)
            {
                var message = Log.CallMethodLogError(clientInfo, timeStart, assemblyName, methodName, e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Gets count of entity objects
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Count of entity objects</returns>
        public int GetObjectCount(ClientInfo clientInfo, string assemblyName, string commandName, PacketInfo packetInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
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
            User.CheckUserLogoned(clientInfo);
            var provider = GetModuleProvider(clientInfo);
            var timeStart = DateTime.Now;
            Log.ProviderLogBegin(clientInfo, timeStart, assemblyName, commandName, "GetObjectCount");
            try
            {
                var count = provider.GetDataCount(assemblyName, commandName, packetInfo);
                var timeEnd = DateTime.Now;
                Log.ProviderLogEnd(clientInfo, timeStart, assemblyName, commandName, "GetObjectCount", timeEnd - timeStart, -1, string.Empty);
                return count;
            }
            catch (Exception e)
            {
                var message = Log.ProviderLogError(clientInfo, timeStart, assemblyName, commandName, "GetObjectCount", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Gets a list of entity objects
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>List of entity objects</returns>
        public List<EntityObject> GetObjects(ClientInfo clientInfo, string assemblyName, string commandName, PacketInfo packetInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
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
            User.CheckUserLogoned(clientInfo);
            var module = PackageProvider.LoadModule(clientInfo, assemblyName);
            var timeStart = DateTime.Now;
            Log.ProviderLogBegin(clientInfo, timeStart, assemblyName, commandName, "GetObjects");
            try
            {
                var returnObjects = module.GetObjects(commandName, packetInfo);
                var timeEnd = DateTime.Now;
                Log.ProviderLogEnd(clientInfo, timeStart, assemblyName, commandName, "GetObjects", timeEnd - timeStart, -1, string.Empty);
                return returnObjects;
            }
            catch (Exception e)
            {
                var message = Log.ProviderLogError(clientInfo, timeStart, assemblyName, commandName, "GetObjects", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Gets entity object by key
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <param name="keyValues">Key and values</param>
        /// <returns>Entity object</returns>
        public object GetObjectByKey(ClientInfo clientInfo, string assemblyName, string commandName, string entitySetName, Dictionary<string, object> keyValues)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (keyValues == null)
            {
                throw new ArgumentNullException("keyValues");
            }
            User.CheckUserLogoned(clientInfo);
            var module = PackageProvider.LoadModule(clientInfo, assemblyName);
            var timeStart = DateTime.Now;
            Log.ProviderLogBegin(clientInfo, timeStart, assemblyName, commandName, "GetObjectByKey");
            try
            {
                var returnObject = module.GetObjectByKey(commandName, entitySetName, keyValues);
                var timeEnd = DateTime.Now;
                Log.ProviderLogEnd(clientInfo, timeStart, assemblyName, commandName, "GetObjectByKey", timeEnd - timeStart, -1, string.Empty);
                return returnObject;
            }
            catch (Exception e)
            {
                var message = Log.ProviderLogError(clientInfo, timeStart, assemblyName, commandName, "GetObjectByKey", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Gets a list of detail entity objects
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="masterObject">Master entity object</param>
        /// <returns>Master entity object with detail entity objects</returns>
        public EntityObject GetDetailObjects(ClientInfo clientInfo, string assemblyName, string commandName, EntityObject masterObject)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (masterObject == null)
            {
                throw new ArgumentNullException("masterObject");
            }
            User.CheckUserLogoned(clientInfo);
            var module = PackageProvider.LoadModule(clientInfo, assemblyName);
            var timeStart = DateTime.Now;
            Log.ProviderLogBegin(clientInfo, timeStart, assemblyName, commandName, "GetDetailObjects");
            try
            {
                var returnObject = masterObject;
                module.GetDetailObjects(commandName, masterObject);
                var timeEnd = DateTime.Now;
                Log.ProviderLogEnd(clientInfo, timeStart, assemblyName, commandName, "GetDetailObjects", timeEnd - timeStart, -1, string.Empty);
                return returnObject;
            }
            catch (Exception e)
            {
                var message = Log.ProviderLogError(clientInfo, timeStart, assemblyName, commandName, "GetDetailObjects", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Update entity objects to database
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="objects">List of entity objects</param>
        /// <param name="states">State of entity objects</param>
        /// <returns>List of entity objects</returns>
        public List<EntityObject> UpdateObjects(ClientInfo clientInfo, string assemblyName, string commandName, List<EntityObject> objects, Dictionary<EntityKey, EntityState> states)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }
            if (objects == null)
            {
                throw new ArgumentNullException("objects");
            }
            User.CheckUserLogoned(clientInfo);
            var module = PackageProvider.LoadModule(clientInfo, assemblyName);
            var timeStart = DateTime.Now;
            Log.ProviderLogBegin(clientInfo, timeStart, assemblyName, commandName, "UpdateObjects");
            try
            {
                var returnObjects = objects;
                module.UpdateObjects(commandName, objects, states);
                var timeEnd = DateTime.Now;
                Log.ProviderLogEnd(clientInfo, timeStart, assemblyName, commandName, "UpdateObjects", timeEnd - timeStart, -1, string.Empty);
                return returnObjects;
            }
            catch (Exception e)
            {
                var message = Log.ProviderLogError(clientInfo, timeStart, assemblyName, commandName, "UpdateObjects", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Calls flow method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="parameter">Flow parameter</param>
        /// <returns>Result of flow method</returns>
        public FlowResult CallFlowMethod(ClientInfo clientInfo, FlowParameter parameter)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }
            User.CheckUserLogoned(clientInfo);
            var timeStart = DateTime.Now;
            Log.CallMethodLogBegin(clientInfo, timeStart, string.Empty, parameter.Operation.ToString());
            try
            {
                var returnObject = Flow.CallFlowMethod(clientInfo, parameter);
                var timeEnd = DateTime.Now;
                Log.CallMethodLogEnd(clientInfo, timeStart, string.Empty, parameter.Operation.ToString(), timeEnd - timeStart);
                return returnObject;
            }
            catch (TargetInvocationException e)
            {
                var exception = e.InnerException;
                var message = Log.CallMethodLogError(clientInfo, timeStart, string.Empty, parameter.Operation.ToString(), exception);
                throw new Exception(message, exception);
            }
            catch (Exception e)
            {
                var message = Log.CallMethodLogError(clientInfo, timeStart, string.Empty, parameter.Operation.ToString(), e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Gets flow data
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="dataType">Type of flow data</param>
        /// <returns>Flow data</returns>
        public List<EntityObject> GetFlowData(ClientInfo clientInfo, FlowDataType dataType, FlowDataParameter parameter)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "GetFlowData");
            try
            {
                var data = globalModule.GetFlowData(dataType, parameter);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "GetFlowData", timeEnd - timeStart);
                return data;
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "GetFlowData", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Gets flow data
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="dataType">Type of flow data</param>
        /// <returns>Flow data</returns>
        public string GetFlowDataDS(ClientInfo clientInfo, FlowDataType dataType, FlowDataParameter parameter)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "GetFlowDataDS");
            try
            {
                var data = globalModule.GetFlowDataDS(dataType, parameter);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "GetFlowDataDS", timeEnd - timeStart);
                return data;
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "GetFlowDataDS", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Execute SQL method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="database">Database</param>
        /// <param name="CommandText">CommandText</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>DataSet</returns>
        public object ExecuteSQL(ClientInfo clientInfo, string database, string commandText, PacketInfo packetInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException("commandText");
            }
            if (string.IsNullOrEmpty(database))
            {
                database = clientInfo.Database;
            }
            User.CheckUserLogoned(clientInfo);

            var timeStart = DateTime.Now;
            Log.ProviderLogBegin(clientInfo, timeStart, string.Empty, string.Empty, "ExecuteSQL");
            try
            {
                var returnObject = Database.ExecuteSQL(database, clientInfo.SDDeveloperID, packetInfo.InsertQueryString(clientInfo, commandText), null, packetInfo);
                var timeEnd = DateTime.Now;
                Log.ProviderLogEnd(clientInfo, timeStart, string.Empty, string.Empty, "ExecuteSQL", timeEnd - timeStart, 0, commandText);
                return returnObject;
            }
            catch (Exception e)
            {
                var message = Log.ProviderLogError(clientInfo, timeStart, string.Empty, string.Empty, "ExecuteSQL", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Execute SQL method
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="database">Database</param>
        /// <param name="CommandText">CommmandText</param>
        public void ExecuteCommand(ClientInfo clientInfo, string database, string commandText)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException("commandText");
            }
            if (string.IsNullOrEmpty(database))
            {
                database = clientInfo.Database;
            }
            User.CheckUserLogoned(clientInfo);

            var timeStart = DateTime.Now;
            Log.ProviderLogBegin(clientInfo, timeStart, string.Empty, string.Empty, "ExecuteCommand");
            try
            {
                Database.ExecuteCommand(database, commandText);
                var timeEnd = DateTime.Now;
                Log.ProviderLogEnd(clientInfo, timeStart, string.Empty, string.Empty, "ExecuteCommand", timeEnd - timeStart, 0, commandText);
            }
            catch (Exception e)
            {
                var message = Log.ProviderLogError(clientInfo, timeStart, string.Empty, string.Empty, "ExecuteCommand", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Gets count of data
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Count of data</returns>
        public int GetDataCount(ClientInfo clientInfo, string assemblyName, string commandName, PacketInfo packetInfo)
        {
            return GetObjectCount(clientInfo, assemblyName, commandName, packetInfo);
        }

        /// <summary>
        /// Gets total
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <param name="totals">Information of total</param>
        /// <returns>Total of data</returns>
        public object GetDataTotal(ClientInfo clientInfo, string assemblyName, string commandName, PacketInfo packetInfo, Dictionary<string, string> totals)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
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
            User.CheckUserLogoned(clientInfo);
            clientInfo.UseDataSet = true;
            var provider = GetModuleProvider(clientInfo);
            provider.ClientInfo = clientInfo;
            var timeStart = DateTime.Now;
            Log.ProviderLogBegin(clientInfo, timeStart, assemblyName, commandName, "GetTotal");
            try
            {
                var returnObject = provider.GetDataTotal(assemblyName, commandName, packetInfo, totals);
                var timeEnd = DateTime.Now;
                Log.ProviderLogEnd(clientInfo, timeStart, assemblyName, commandName, "GetTotal", timeEnd - timeStart, -1, string.Empty);
                return returnObject;
            }
            catch (Exception e)
            {
                var message = Log.ProviderLogError(clientInfo, timeStart, assemblyName, commandName, "GetTotal", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Gets dataset from database
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Dataset</returns>
        public object GetDataSet(ClientInfo clientInfo, string assemblyName, string commandName, PacketInfo packetInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
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
            User.CheckUserLogoned(clientInfo);
            clientInfo.UseDataSet = true;
            var provider = GetModuleProvider(clientInfo);
            var timeStart = DateTime.Now;
            Log.ProviderLogBegin(clientInfo, timeStart, assemblyName, commandName, "GetDataSet");
            try
            {
                var returnObject = provider.GetSerializedDataSet(assemblyName, commandName, packetInfo);
                var timeEnd = DateTime.Now;
                Log.ProviderLogEnd(clientInfo, timeStart, assemblyName, commandName, "GetDataSet", timeEnd - timeStart, 1, provider.SqlSentence);
                return returnObject;
            }
            catch (Exception e)
            {
                var message = Log.ProviderLogError(clientInfo, timeStart, assemblyName, commandName, "GetDataSet", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Updates dataset to database
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="dataset">Dataset</param>
        /// <returns>Dataset</returns>
        public object UpdateDataSet(ClientInfo clientInfo, string assemblyName, string commandName, object dataset)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
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
                throw new ArgumentNullException("objects");
            }
            User.CheckUserLogoned(clientInfo);
            clientInfo.UseDataSet = true;
            var provider = GetModuleProvider(clientInfo);
            var timeStart = DateTime.Now;
            Log.ProviderLogBegin(clientInfo, timeStart, assemblyName, commandName, "UpdateDataSet");
            try
            {
                var returnObjects = provider.UpdateDataSet(assemblyName, commandName, dataset);
                var timeEnd = DateTime.Now;
                Log.ProviderLogEnd(clientInfo, timeStart, assemblyName, commandName, "UpdateDataSet", timeEnd - timeStart, 2, provider.SqlSentence);
                return returnObjects;
            }
            catch (Exception e)
            {
                var message = Log.ProviderLogError(clientInfo, timeStart, assemblyName, commandName, "UpdateDataSet", e);
                throw new Exception(message, e);
            }
        }

        public object ExecuteIOTMethod(ClientInfo clientInfo, string assemblyName, string commandName, object dataset)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
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
                throw new ArgumentNullException("objects");
            }
            User.CheckUserLogoned(clientInfo);
            var provider = GetModuleProvider(clientInfo);
            var timeStart = DateTime.Now;
            Log.ProviderLogBegin(clientInfo, timeStart, assemblyName, commandName, "ExecuteIOTMethod");
            try
            {
                var returnObjects = provider.ExecuteIOTMethod(assemblyName, commandName, dataset);
                var timeEnd = DateTime.Now;
                Log.ProviderLogEnd(clientInfo, timeStart, assemblyName, commandName, "ExecuteIOTMethod", timeEnd - timeStart, 2, provider.SqlSentence);
                return returnObjects;
            }
            catch (Exception e)
            {
                var message = Log.ProviderLogError(clientInfo, timeStart, assemblyName, commandName, "ExecuteIOTMethod", e);
                throw new Exception(message, e);
            }

        }

        /// <summary>
        /// Do record lock
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <param name="type">Lock type</param>
        /// <returns>Lock</returns>
        public LockStatus DoRecordLock(ClientInfo clientInfo, string assemblyName, string commandName, PacketInfo packetInfo, EFWCFModule.EEPAdapter.LockType type)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
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
            User.CheckUserLogoned(clientInfo);
            var provider = GetModuleProvider(clientInfo);
            var timeStart = DateTime.Now;
            Log.ProviderLogBegin(clientInfo, timeStart, assemblyName, commandName, "DoRecordLock");
            try
            {
                var user = string.Empty;
                var returnObject = provider.DoRecordLock(assemblyName, commandName, packetInfo, type, ref user);
                var timeEnd = DateTime.Now;
                Log.ProviderLogEnd(clientInfo, timeStart, assemblyName, commandName, "DoRecordLock", timeEnd - timeStart, -1, string.Empty);
                return new LockStatus() { LockType = returnObject, UserID = user };
            }
            catch (Exception e)
            {
                var message = Log.ProviderLogError(clientInfo, timeStart, assemblyName, commandName, "DoRecordLock", e);
                throw new Exception(message, e);
            }
        }

        public void RemoveRecordLock(ClientInfo clientInfo, string userID)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            User.CheckUserLogoned(clientInfo);
            RecordLock.ClearRecordFile(userID);
        }

        /// <summary>
        /// Gets menus
        /// </summary>
        /// <returns>Menus</returns>
        public List<EntityObject> FetchMenus(ClientInfo clientInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "FetchMenus");
            //try
            //{
            var data = globalModule.FetchMenus();
            var timeEnd = DateTime.Now;
            Log.SystemLogEnd(clientInfo, timeStart, "FetchMenus", timeEnd - timeStart);
            return data;
            //}
            //catch (Exception e)
            //{
            //var message = Log.SystemLogError(clientInfo, timeStart, "FetchMenus", e);
            //throw new Exception(message, e);
            //}
        }

        /// <summary>
        /// Gets sys_refval
        /// </summary>
        /// <returns>sys_refval</returns>
        public List<EntityObject> GetAllDataByTableName(ClientInfo clientInfo, String tableName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "GetAllDataByTableName");
            try
            {
                var data = globalModule.GetAllDataByTableName(tableName);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "GetAllDataByTableName", timeEnd - timeStart);
                return data;
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "GetAllDataByTableName", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Gets sys_refval
        /// </summary>
        /// <returns>sys_refval</returns>
        public List<EntityObject> GetAllDataByTableNameDBAlias(ClientInfo clientInfo, String dbAlias, String tableName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "GetAllDataByTableName");
            try
            {
                var data = globalModule.GetAllDataByTableNameDBAlias(dbAlias, tableName);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "GetAllDataByTableName", timeEnd - timeStart);
                return data;
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "GetAllDataByTableName", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// SaveDataToTable
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>void</returns>
        public void SaveDataToTable(ClientInfo clientInfo, object[] param, String tableName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "SaveDataToTable");
            try
            {
                globalModule.SaveDataToTable(param, tableName);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "SaveDataToTable", timeEnd - timeStart);
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "SaveDataToTable", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// AutoSeqMenuID
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>int</returns>
        public int AutoSeqMenuID(ClientInfo clientInfo, object[] param)
        {
            int returnValue = 0;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "AutoSeqMenuID");
            try
            {
                returnValue = globalModule.AutoSeqMenuID(param);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "AutoSeqMenuID", timeEnd - timeStart);
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "AutoSeqMenuID", e);
                throw new Exception(message, e);
            }
            return returnValue;
        }

        /// <summary>
        /// DeleteDataFromTable
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>void</returns>
        public void DeleteDataFromTable(ClientInfo clientInfo, object param, String tableName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "DeleteDataFromTable");
            try
            {
                globalModule.DeleteDataFromTable(param, tableName);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "DeleteDataFromTable", timeEnd - timeStart);
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "DeleteDataFromTable", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Gets server path
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>String</returns>
        public String GetServerPath(ClientInfo clientInfo)
        {
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            return globalModule.GetServerPath();
        }

        /// <summary>
        /// Creates sd entity reference assembly
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="commandName">Name of command</param>
        /// <param name="commmandText">Command Text</param>
        public Stream CreateSDEntityReferenceAssembly(ClientInfo clientInfo, string assemblyName, string commandName, string commmandText)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }

            User.CheckUserLogoned(clientInfo);
            var timeStart = DateTime.Now;
            var provider = new EEPAdapter.SDModuleProvider();
            provider.ClientInfo = clientInfo;
            Log.SystemLogBegin(clientInfo, timeStart, "CreateSDEntityReferenceAssembly");
            try
            {
                var dataSets = new List<DataSet>();
                if (assemblyName == "SYS_REFVAL")
                {
                    var dataSet = provider.GetSysRefVals();
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        var refvalID = (string)dataSet.Tables[0].Rows[i]["REFVAL_NO"];
                        var commandText = (string)dataSet.Tables[0].Rows[i]["SELECT_COMMAND"];
                        try
                        {
                            dataSets.Add(Database.ExecuteDataSet(clientInfo.Database, clientInfo.SDDeveloperID, commandText, refvalID));
                        }
                        catch { }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(commmandText))
                    {
                        dataSets.Add(Database.ExecuteDataSet(clientInfo.Database, clientInfo.SDDeveloperID, commmandText, commandName));
                    }
                    else
                    {
                        var commandNames = provider.GetCommandNames(assemblyName);
                        foreach (var command in commandNames)
                        {
                            try
                            {
                                dataSets.Add(provider.GetDataSet(assemblyName, command
                                    , new PacketInfo() { OnlySchema = true, OrderParameters = new List<OrderParameter>(), WhereParameters = new List<WhereParameter>() }));
                            }
                            catch { }
                        }
                    }
                }
                var stream = EEPAdapter.SDModuleProvider.CreateEntityReferenceAssembly(dataSets, assemblyName, clientInfo);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "CreateSDEntityReferenceAssembly", timeEnd - timeStart);
                return stream;
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "CreateSDEntityReferenceAssembly", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Create html page
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="pageName">Name of page</param>
        /// <param name="aspxContent">Content of aspx</param>
        /// <param name="code">Code</param>
        /// <param name="reportContent">Content of report</param>
        /// <param name="websitePath">Path of website</param>
        /// <returns>path of page</returns>
        public string CreateHtmlPage(ClientInfo clientInfo, string pageName, string aspxContent, string code, string reportContent, string websitePath)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(pageName))
            {
                throw new ArgumentNullException("pageName");
            }

            User.CheckUserLogoned(clientInfo);
            var timeStart = DateTime.Now;
            var provider = new EEPAdapter.SDModuleProvider();
            provider.ClientInfo = clientInfo;
            Log.SystemLogBegin(clientInfo, timeStart, "CreateHtmlPage");
            try
            {
                var path = EEPAdapter.SDModuleProvider.CreateHtmlPage(pageName, aspxContent, code, reportContent, clientInfo, websitePath);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "CreateHtmlPage", timeEnd - timeStart);
                return path;
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "CreateHtmlPage", e);
                throw new Exception(message, e);
            }

        }

        /// <summary>
        /// Gets sys_sdsolutions
        /// </summary>
        /// <returns>sys_sdsolutions</returns>
        public List<EntityObject> GetAllDataByTableNameForSD(ClientInfo clientInfo, String tableName, String UserID)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "GetAllDataByTableNameForSD");
            try
            {
                var data = globalModule.GetAllDataByTableNameForSDSystemTable(tableName, UserID);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "GetAllDataByTableNameForSD", timeEnd - timeStart);
                return data;
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "GetAllDataByTableNameForSD", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// DeleteDataFromTable For SD System Talble
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>void</returns>
        public void DeleteDataFromTableForSDSysTb(ClientInfo clientInfo, object param, String tableName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "DeleteDataFromTableForSDSysTb");
            try
            {
                globalModule.DeleteDataFromTableForSDSystemTable(param, tableName);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "DeleteDataFromTableForSDSysTb", timeEnd - timeStart);
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "DeleteDataFromTableForSDSysTb", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Save For SD System Talble
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>void</returns>
        public void SaveDataToTableForSDSysTb(ClientInfo clientInfo, object[] param, String tableName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "SaveDataToTableForSDSysTb");
            try
            {
                globalModule.SaveDataToTableForSDSystemTable(param, tableName);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "SaveDataToTableForSDSysTb", timeEnd - timeStart);
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "SaveDataToTableForSDSysTb", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// Execute SQL method for SD System Database
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="CommandText">CommmandText</param>
        public void ExecuteCommandForSDSyeDatabase(ClientInfo clientInfo, string commandText)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException("commandText");
            }
            User.CheckUserLogoned(clientInfo);

            var timeStart = DateTime.Now;
            Log.CallMethodLogBegin(clientInfo, timeStart, string.Empty, "ExecuteCommandForSDSyeDatabase");
            try
            {
                Database.ExecuteCommand(Database.SystemDatabase, commandText);
                var timeEnd = DateTime.Now;
                Log.CallMethodLogEnd(clientInfo, timeStart, string.Empty, "ExecuteCommandForSDSyeDatabase", timeEnd - timeStart);
            }
            catch (Exception e)
            {
                var message = Log.CallMethodLogError(clientInfo, timeStart, string.Empty, "ExecuteCommandForSDSyeDatabase", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// log sd user
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        public void RefreshUserLogForSD(ClientInfo clientInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            try
            {
                globalModule.RefreshUserLogForSD();
            }
            catch { }
        }

        /// <summary>
        /// Get Data from table which SD user's Security Table like user or groups For Design time
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<EntityObject> GetSecurityTableForSDDesign(ClientInfo clientInfo, String tableName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "GetAllDataByTableNameForSD");
            try
            {
                var data = globalModule.GetSecurityTableForSDDesign(tableName);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "GetAllDataByTableNameForSD", timeEnd - timeStart);
                return data;
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "GetAllDataByTableNameForSD", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// DeleteDataFromTable For  SD user's Sceurity Table like user or groups For Design time
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>void</returns>
        public void DeleteDataFromTableForSDDesignSecurityTable(ClientInfo clientInfo, object param, String tableName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "DeleteDataFromTableForSDSysTb");
            try
            {
                globalModule.DeleteDataFromTableForSDDesignSecurityTable(param, tableName);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "DeleteDataFromTableForSDSysTb", timeEnd - timeStart);
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "DeleteDataFromTableForSDSysTb", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// aveDataToTable For SD user's Sceurity Table like user or groups For Design time
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>void</returns>
        public void SaveDataToTableForSDDesignSecurityTable(ClientInfo clientInfo, object[] param, String tableName)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "SaveDataToTableForSDSysTb");
            try
            {
                globalModule.SaveDataToTableForSDDesignSecurityTable(param, tableName);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "SaveDataToTableForSDSysTb", timeEnd - timeStart);
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "SaveDataToTableForSDSysTb", e);
                throw new Exception(message, e);
            }
        }

        /// <summary>
        /// get mail address for reset user passward
        /// </summary>
        /// <returns>mail address</returns>
        public string ResetUser(ClientInfo clientInfo,string email)
        {
            string newPWD = "";

            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var globalModule = PackageProvider.LoadGlobalModule(clientInfo);
            var timeStart = DateTime.Now;
            Log.SystemLogBegin(clientInfo, timeStart, "GetUserInfoForNotLogon");
            try
            {
                newPWD = globalModule.ResetUserPassword(email);
                var timeEnd = DateTime.Now;
                Log.SystemLogEnd(clientInfo, timeStart, "GetUserInfoForNotLogon", timeEnd - timeStart);
            }
            catch (Exception e)
            {
                var message = Log.SystemLogError(clientInfo, timeStart, "GetUserInfoForNotLogon", e);
                throw new Exception(message, e);
            }
            return newPWD;
        }
        #endregion

        public string GetConnectionInfo(ClientInfo clientInfo, SDTableType tableType)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            User.CheckUserLogoned(clientInfo);
            var database = GetSDDatabase(clientInfo, tableType);
            clientInfo.Database = database;
            var developerID = tableType == SDTableType.SDSystemTable ? string.Empty : clientInfo.SDDeveloperID;
            clientInfo.SDDeveloperID = developerID;

            var timeStart = DateTime.Now;
            Log.CallMethodLogBegin(clientInfo, timeStart, string.Empty, "ExecuteSQL");
            try
            {
                var connection = DbConnectionSet.WaitForConnection(database, developerID, string.Empty, "EFModule", DateTime.Now);
                var connectionString = DbConnectionSet.GetConnectionString("SDUser");
                connectionString += ";DB=" + connection.Database;
                return connectionString;
            }
            catch (Exception e)
            {
                var message = Log.CallMethodLogError(clientInfo, timeStart, string.Empty, "ExecuteSQL", e);
                throw new Exception(message, e);
            }
        }

        public string SDExcuteSql(ClientInfo clientInfo, SQLCommandInfo sqlCommand, PacketInfo packetInfo, SDTableType tableType)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            User.CheckUserLogoned(clientInfo);
            var database = GetSDDatabase(clientInfo, tableType);
            clientInfo.Database = database;
            var developerID = tableType == SDTableType.SDSystemTable ? string.Empty : clientInfo.SDDeveloperID;
            clientInfo.SDDeveloperID = developerID;

            var timeStart = DateTime.Now;
            Log.CallMethodLogBegin(clientInfo, timeStart, string.Empty, "ExecuteSQL");
            try
            {
                if (!string.IsNullOrEmpty(sqlCommand.CommandText) && sqlCommand.CommandText.IndexOf("SysDatabases", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    throw new Exception("Invalid object name 'SysDatabases'.");
                }

                var returnObject = string.Empty;
                if (packetInfo.OnlySchema)
                {
                    returnObject = (string)Database.ExecuteSchema(database, developerID, sqlCommand.CommandText);
                }
                else
                {
                    returnObject = (string)Database.ExecuteSQL(database, developerID, sqlCommand, null, packetInfo);
                }
                var timeEnd = DateTime.Now;
                Log.CallMethodLogEnd(clientInfo, timeStart, string.Empty, "ExecuteSQL", timeEnd - timeStart);
                return returnObject;
            }
            catch (Exception e)
            {
                var message = Log.CallMethodLogError(clientInfo, timeStart, string.Empty, "ExecuteSQL", e);
                throw new Exception(message, e);
            }
        }

        //public int SDExcuteCommand(ClientInfo clientInfo, string commandText, SDTableType tableType, Dictionary<string, object> parameters)
        //{
        //    if (clientInfo == null)
        //    {
        //        throw new ArgumentNullException("clientInfo");
        //    }
        //    if (string.IsNullOrEmpty(commandText))
        //    {
        //        throw new ArgumentNullException("commandText");
        //    }
        //    var database = GetSDDatabase(clientInfo, tableType);
        //    clientInfo.Database = database;
        //    var developerID = tableType == SDTableType.SDSystemTable ? string.Empty : clientInfo.SDDeveloperID;
        //    clientInfo.SDDeveloperID = developerID;
        //    User.CheckUserLogoned(clientInfo);
        //    var timeStart = DateTime.Now;
        //    Log.CallMethodLogBegin(clientInfo, timeStart, string.Empty, "ExecuteCommand");
        //    try
        //    {
        //        var returnObject = Database.ExecuteCommand(database, developerID, commandText, parameters);
        //        var timeEnd = DateTime.Now;
        //        Log.CallMethodLogEnd(clientInfo, timeStart, string.Empty, "ExecuteCommand", timeEnd - timeStart);
        //        return returnObject;
        //    }
        //    catch (Exception e)
        //    {
        //        var message = Log.CallMethodLogError(clientInfo, timeStart, string.Empty, "ExecuteCommand", e);
        //        throw new Exception(message, e);
        //    }
        //}

        public int SDExcuteCommands(ClientInfo clientInfo, List<SQLCommandInfo> sqlCommands, SDTableType tableType)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            User.CheckUserLogoned(clientInfo);
            var database = GetSDDatabase(clientInfo, tableType);
            clientInfo.Database = database;
            var developerID = tableType == SDTableType.SDSystemTable ? string.Empty : clientInfo.SDDeveloperID;
            clientInfo.SDDeveloperID = developerID;
            var timeStart = DateTime.Now;
            Log.CallMethodLogBegin(clientInfo, timeStart, string.Empty, "ExecuteCommand");
            try
            {
                var returnObject = Database.ExecuteCommands(database, developerID, sqlCommands);
                var timeEnd = DateTime.Now;
                Log.CallMethodLogEnd(clientInfo, timeStart, string.Empty, "ExecuteCommand", timeEnd - timeStart);
                return returnObject;
            }
            catch (Exception e)
            {
                var message = Log.CallMethodLogError(clientInfo, timeStart, string.Empty, "ExecuteCommand", e);
                throw new Exception(message, e);
            }
        }

        public string SDUpdateTable(ClientInfo clientInfo, string tableName, List<UpdateRow> updateRows, SDTableType tableType)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            User.CheckUserLogoned(clientInfo);
            var database = GetSDDatabase(clientInfo, tableType);
            clientInfo.Database = database;
            var developerID = tableType == SDTableType.SDSystemTable ? string.Empty : clientInfo.SDDeveloperID;
            clientInfo.SDDeveloperID = developerID;

            var timeStart = DateTime.Now;
            Log.CallMethodLogBegin(clientInfo, timeStart, string.Empty, "ExecuteCommand");
            try
            {
                if (tableName == "SYS_WEBPAGES" && tableType == SDTableType.SDSystemTable)
                {
                    foreach (var updateRow in updateRows)
                    {
                        if ((string)updateRow.NewValues["PageType"] == "S")
                        {
                            if (updateRow.RowState == DataRowState.Added || updateRow.RowState == DataRowState.Modified)
                            {
                                var sComponentCS = (string)updateRow.NewValues["SERVERDLL"];

                                var resx = string.Empty;
                                if (updateRow.NewValues.ContainsKey("RESX"))
                                {
                                    resx = (string)updateRow.NewValues["RESX"];
                                    updateRow.NewValues.Remove("RESX");
                                }

                                var assemblyBytes = SDModuleProvider.CreateServerDllAssembly(sComponentCS, (string)updateRow.NewValues["PageName"], resx, clientInfo);
                                updateRow.NewValues["SERVERDLL"] = assemblyBytes;

                                if (!updateRow.NewValues.ContainsKey("Content"))
                                {
                                    updateRow.NewValues["Content"] = System.Text.Encoding.UTF8.GetBytes(SDModuleProvider.GetServerContent(assemblyBytes, sComponentCS));
                                }
                            }
                        }
                    }
                }

                var returnObject = Database.UpdateTable(database, tableName, developerID, updateRows);
                var timeEnd = DateTime.Now;
                Log.CallMethodLogEnd(clientInfo, timeStart, string.Empty, "ExecuteCommand", timeEnd - timeStart);
                return returnObject;
            }
            catch (Exception e)
            {
                var message = Log.CallMethodLogError(clientInfo, timeStart, string.Empty, "ExecuteCommand", e);
                throw new Exception(message, e);
            }
        }

        public string SDGetSolutions(ClientInfo clientInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var provider = new SDModuleProvider();
            provider.ClientInfo = clientInfo;
            return provider.GetSolutions();
        }

        public string SDRegisterMachine(string key)
        {
            var provider = new SDModuleProvider();
            return provider.RegisterMachine(key);
        }

        public string SDBuildCordova(string directory, string type)
        {
            var provider = new SDModuleProvider();
            return provider.BuildCordova(directory, type);
        }

        public bool SDCheckActiveUserCount(int maxUser)
        {
            var provider = new SDModuleProvider();
            return provider.CheckActiveUserCount(maxUser);
        }

        public string SDRegisterUser(string userID, string email, string phone, string password, string description, string developer, string owner)
        {
            var provider = new SDModuleProvider();
            return provider.RegisterUser(userID, email, phone, password, description, developer, owner);
        }

        public string SDResetUser(string userID, string email, string developer)
        {
            var provider = new SDModuleProvider();
            return provider.ResetUser(userID, email, developer);
        }

        public string SDActiveUser(string userID, string encryptPassword)
        {
            var provider = new SDModuleProvider();
            return provider.ActiveUser(userID, encryptPassword);
        }

        private string GetSDDatabase(ClientInfo clientInfo, SDTableType tableType)
        {
            if (tableType == SDTableType.SDSystemTable)
            {
                return Database.GetSystemDatabase(null);
            }
            else
            {
                if (tableType == SDTableType.SystemTable && Database.GetSplitSystemTable(clientInfo.Database, clientInfo.SDDeveloperID))
                {
                    return Database.GetSystemDatabase(clientInfo.SDDeveloperID);
                }
                return clientInfo.Database;
            }
        }
    }
}
