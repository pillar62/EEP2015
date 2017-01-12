using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data;

namespace EFWCFModule
{
    /// <summary>
    /// Interface of EFModule
    /// </summary>
    public interface IEFModule
    {
        /// <summary>
        /// Gets or sets information of client
        /// </summary>
        ClientInfo ClientInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets list of command names
        /// </summary>
        /// <returns>List of command names</returns>
        List<string> GetCommandNames();

        /// <summary>
        /// Gets container name of entity
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <returns>Container name of entity</returns>
        string GetEntityContainerName(string commandName);

        /// <summary>
        /// Gets name of entity object class
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <returns>Name of entity object class</returns>
        string GetObjectClassName(string commandName, string entitySetName);

        /// <summary>
        /// Gets primary keys of entity object
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Primary keys of entity object</returns>
        List<string> GetEntityPrimaryKeys(string commandName, string entityTypeName);

        /// <summary>
        /// Gets fileds of entity object
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        List<string> GetEntityFields(string commandName, string entityTypeName);

        /// <summary>
        /// Gets fileds of entity object
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Fields of entity object</returns>
        Dictionary<String, int> GetEntityFieldsLength(string commandName, string entityTypeName);


        /// <summary>
        /// Gets type of fileds of entity object
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Type of fields of entity object</returns>
        Dictionary<string, string> GetEntityFieldTypes(string commandName, string entityTypeName);


         /// <summary>
        /// Gets mapping of fileds of entity object
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Mapping of fields of entity object</returns>
        Dictionary<string, string> GetEntityFieldMappings(string commandName, string entityTypeName);

        /// <summary>
        /// Gets navigation fields of entity object
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Navigation Fields of entity object</returns>
        List<string> GetEntityNavigationFields(string commandName, string entityTypeName);

        /// <summary>
        /// Gets list of name of eneity sets which type is specialfied type
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Name of entity object</returns>
        List<string> GetEntitySetNames(string commandName, string entityTypeName);

        /// <summary>
        /// Gets count of entity objects
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>Count of entity objects</returns>
        int GetObjectCount(string commandName, PacketInfo packetInfo);

        /// <summary>
        /// Gets a list of entity objects
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="packetInfo">Information of packet</param>
        /// <returns>List of entity objects</returns>
        List<EntityObject> GetObjects(string commandName, PacketInfo packetInfo);

        /// <summary>
        /// Gets entity object by key
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="entitySetName">Name of entity set</param>
        /// <param name="keyValues">Key and values</param>
        /// <returns>Entity object</returns>
        object GetObjectByKey(string commandName, string entitySetName, Dictionary<string, object> keyValues);

        /// <summary>
        /// Gets a list of detail entity objects
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="masterObject">Master entity object</param>
        void GetDetailObjects(string commandName, EntityObject masterObject);

        /// <summary>
        /// Update a list of changed entity objects to database
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <param name="objects">List of entity objects to update</param>
        /// <param name="states">State of entity objects</param>
        /// <returns>Count of rows affected</returns>
        int UpdateObjects(string commandName, List<EntityObject> objects, Dictionary<EntityKey, EntityState> states);

        /// <summary>
        /// Excute server method
        /// </summary>
        /// <param name="methodName">Name of method</param>
        /// <param name="param">Parameters of method</param>
        /// <returns>Result of excuting server method</returns>
        object CallMethod(string methodName, object[] param);

    }
    
    /// <summary>
    /// Interface of GlobalModule
    /// </summary>
    public interface IGlobalModule : IEFModule
    {
        /// <summary>
        /// Checks user
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>Information of client returned</returns>
        ClientInfo CheckUser();

        ClientInfo LogOnDevice(string userID, string deviceID, bool check, string developerID);

        void RegisterDevice(string userID, string deviceID, string regID, string tokenID, string developerID);

        List<string> GetRegIDs(List<string> userIDs,string developerID);

        List<string> GetTokenIDs(List<string> userIDs, string developerID);

        void SendMessage(List<string> users, string subject, string body);

        void DeleteMessage(List<string> dateTimes);
        
        void ReadMessage(string dateTime);
        
        string GetMessages();

        /// <summary>
        /// Gets defination of columns
        /// </summary>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Defination of columns</returns>
        List<EntityObject> GetColumnDefination(string entityTypeName);

        /// <summary>
        /// Gets defination of columns
        /// </summary>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Defination of columns</returns>
        List<EntityObject> GetColumnDefination(string assemblyName, string commandName, string entityTypeName);

        /// <summary>
        /// Gets defination of columns
        /// </summary>
        /// <param name="entityTypeName">Type of entity set</param>
        /// <returns>Defination of columns</returns>
        List<EntityObject> GetDataByTableNameWhere(object[] param);

        /// <summary>
        /// Gets list of solutions
        /// </summary>
        /// <returns>List of solutions</returns>
        List<SolutionInfo> GetSolutions();

        /// <summary>
        /// Gets flow data
        /// </summary>
        /// <param name="dataType">Type of data</param>
        /// <returns>Flow data</returns>
        List<EntityObject> GetFlowData(FlowDataType dataType, FlowDataParameter parameter);

        /// <summary>
        /// Gets flow data
        /// </summary>
        /// <param name="dataType">Type of data</param>
        /// <returns>Flow data</returns>
        string GetFlowDataDS(FlowDataType dataType, FlowDataParameter parameter);

        /// <summary>
        /// Gets menus
        /// </summary>
        /// <returns>Menus</returns>
        List<EntityObject> FetchMenus();

        /// <summary>
        /// Gets SYS_REFVAL
        /// </summary>
        /// <returns>SYS_REFVAL</returns>
        List<EntityObject> GetAllDataByTableName(String tableName);

        List<EntityObject> GetAllDataByTableNameDBAlias(String dbAlias, String tableName);

        /// <summary>
        /// SaveDataToTable
        /// </summary>
        /// <returns>void</returns>
        void SaveDataToTable(object[] param, String tableName);

        /// <summary>
        /// AutoSeqMenuID
        /// </summary>
        /// <returns>int</returns>
        int AutoSeqMenuID(object[] param);

        /// <summary>
        /// DeleteDataFromTable
        /// </summary>
        /// <returns>SYS_REFVAL</returns>
        void DeleteDataFromTable(object param, String tableName);

        /// <summary>
        /// Gets server path
        /// </summary>
        /// <returns>String</returns>
        String GetServerPath();

        /// <summary>
        /// get the infomation of users for nonlogon ,for reset password
        /// </summary>
        /// <returns></returns>
        String ResetUserPassword(string email);

        #region For SD
        /// <summary>
        /// Check User For SD
        /// </summary>
        /// <returns>ClientInfo</returns>
        ClientInfo CheckUserForSDModule();

        /// <summary>
        /// For SD
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<EntityObject> GetAllDataByTableNameForSDSystemTable(String tableName, String userID);
        void SaveDataToTableForSDSystemTable(object[] param, String tableName);
        void DeleteDataFromTableForSDSystemTable(object param, String tableName);
        List<EntityObject> GetSecurityTableForSDDesign(String tableName);
        void SaveDataToTableForSDDesignSecurityTable(object[] param, String tableName);
        void DeleteDataFromTableForSDDesignSecurityTable(object param, String tableName);
        void RefreshUserLogForSD();
        
        #endregion

    }

    /// <summary>
    /// Interface of EFComponent
    /// </summary>
    public interface IEFComponent
    {
        /// <summary>
        /// Gets or sets object context of component
        /// </summary>
        ObjectContext Context
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets container module
        /// </summary>
        IEFModule Module
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Interface of Component using ICommand
    /// </summary>
    public interface IUseCommand
    {
        /// <summary>
        /// Gets or sets command of component
        /// </summary>
        ICommand Command
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Interface of Component using IUpdateComponent
    /// </summary>
    public interface IUseUpdateComponent
    {
        /// <summary>
        /// Gets or sets updatecomponent of component
        /// </summary>
        IUpdateComponent UpdateComponent
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Interface of Component using entity set
    /// </summary>
    public interface IUseEntitySet
    {
        /// <summary>
        /// Gets target entity set name of component
        /// </summary>
        string TargetEntitySet
        {
            get;
        }
    }

    /// <summary>
    /// Interface of Command Component
    /// </summary>
    public interface ICommand : IEFComponent
    {
        /// <summary>
        /// Gets or sets command type
        /// </summary>
        CommandType CommandType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets command text
        /// </summary>
        string CommandText
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets database
        /// </summary>
        string DataBase
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether use system database
        /// </summary>
        bool UseSystemDB
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets metadata file
        /// </summary>
        string MetadataFile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets name of context
        /// </summary>
        string ContextName
        {
            get;
        }

        /// <summary>
        /// Gets name of eneity set
        /// </summary>
        string EntitySetName
        {
            get;
        }

        /// <summary>
        ///  Gets or sets servermodify
        /// </summary>
        bool ServerModify
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a list of entity objects
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>List of entity objects</returns>
        ObjectQuery<EntityObject> GetObjects(ClientInfo clientInfo);

        /// <summary>
        /// Gets a list of entity objects
        /// </summary>
        /// <returns>List of entity objects</returns>
        object ExecuteStoredProcedure();

        /// <summary>
        /// Gets entity object by key
        /// </summary>
        /// <param name="keyValues">Key and values</param>
        /// <returns>Entity object</returns>
        object GetObjectByKey(Dictionary<string, object> keyValues);
    }

    /// <summary>
    /// Interface of UpdateComponent Component
    /// </summary>
    public interface IUpdateComponent : IEFComponent, IUseCommand
    {
        /// <summary>
        /// Update a list of changed entity objects to database
        /// </summary>
        /// <param name="objects">List of entity objects to update</param>
        /// <param name="states">State of entity objects</param>
        /// <param name="masterEntitySetName">Name of master entity set</param>
        /// <returns>Count of rows affected</returns>
        int Update(List<EntityObject> objects, Dictionary<EntityKey, EntityState> states, string masterEntitySetName);
    }

    /// <summary>
    /// Interface of Relation Component
    /// </summary>
    public interface IRelation : IEFComponent
    {
        /// <summary>
        /// Gets or sets master command of component
        /// </summary>
        ICommand MasterCommand
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets detail command of component
        /// </summary>
        ICommand DetailCommand
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a list of detail entity objects from database
        /// </summary>
        /// <param name="clientInfo">Information of client</param>
        /// <param name="masterObject">Master entity object</param>
        void GetDetailObjects(EntityObject masterObject, ClientInfo clientInfo);
    }

    /// <summary>
    /// Interface of Transaction Component
    /// </summary>
    public interface ITransaction : IEFComponent, IUseUpdateComponent
    {
        /// <summary>
        /// Executes transaction of object
        /// </summary>
        /// <param name="objects">List of entity objects to update</param>
        /// <param name="states">State of entity objects</param>
        void Execute(List<EntityObject> objects, Dictionary<EntityKey, EntityState> states);
    }

    /// <summary>
    /// Interface of AutoNumber Component
    /// </summary>
    public interface IAutoNumber : IEFComponent, IUseUpdateComponent
    {
        /// <summary>
        /// Executes auto number of object
        /// </summary>
        /// <param name="objects">List of entity objects to update</param>
        /// <param name="states">State of entity objects</param>
        void Execute(List<EntityObject> objects, Dictionary<EntityKey, EntityState> states);
    }

    /// <summary>
    /// Interface of Log Component
    /// </summary>
    public interface ILog : IEFComponent, IUseUpdateComponent
    {
        /// <summary>
        /// Executes log of object
        /// </summary>
        /// <param name="objects">List of entity objects to update</param>
        /// <param name="states">State of entity objects</param>
        void Log(List<EntityObject> objects, Dictionary<EntityKey, EntityState> states);
    }
}
